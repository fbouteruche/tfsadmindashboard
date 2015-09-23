using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TFSAdminDashboard.Models;

namespace TFSAdminDashboard.Controllers
{
    public class ProjectOverviewController : TFAdminControllerBase
    {
        // GET: ProjectOverview
        public ActionResult Index(string id)
        {
            List<SelectListItem> tpcList = new List<SelectListItem>();
            List<SelectListItem> tpList = new List<SelectListItem>();
            string collectionId  = string.Empty;

            // Get the catalog of team project collections
            ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(
                new[] { CatalogResourceTypes.ProjectCollection },
                false, CatalogQueryOptions.None);


            // a project Id is supplied
            if(!string.IsNullOrWhiteSpace(id))
            {
                // Get the catalog node of the project
                ReadOnlyCollection<CatalogNode> projectNode = configurationServer.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[]{ new KeyValuePair<string, string>("ProjectId", id) },
                    true, CatalogQueryOptions.None);
                if (projectNode.Count == 1)
                {
                    CatalogNode project = projectNode[0];
                    ReadOnlyCollection<CatalogNode> collectionNode = project.QueryParents(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                    if(collectionNode.Count == 1)
                    {
                        CatalogNode collection = collectionNode[0];
                        collectionId = collection.Resource.Properties["InstanceId"];

                        if (!string.IsNullOrWhiteSpace(collectionId))
                        {
                            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(collectionId));

                            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                                new[] { CatalogResourceTypes.TeamProject },
                                false, CatalogQueryOptions.None);

                            foreach (CatalogNode teamProjectNode in teamProjectNodes)
                            {
                                tpList.Add(new SelectListItem { Text = teamProjectNode.Resource.DisplayName, Value = teamProjectNode.Resource.Properties["ProjectId"], Selected = teamProjectNode.Resource.Properties["ProjectId"] == id });
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("One and only one collection is expected for being the parent of the project id " + id);
                    }
                }
                else
                {
                    throw new Exception("One and only one project is expected for the project id " + id);
                }
            }

            foreach (CatalogNode collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                tpcList.Add(new SelectListItem { Text = collectionNode.Resource.DisplayName, Value = collectionNode.Resource.Properties["InstanceId"], Selected = collectionNode.Resource.Properties["InstanceId"] == collectionId });
            }

            ViewBag.TpcList = tpcList.OrderBy(x => x.Text).ToList();
            ViewBag.TpList = tpList.OrderBy(x => x.Text).ToList();

            return View();
        }

        public ActionResult ProjectList(string id)
        {
            Dictionary<string, string> teamProjecList = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(id))
            {
                TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));

                ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject },
                    false, CatalogQueryOptions.None);

                foreach (CatalogNode teamProjectNode in teamProjectNodes)
                {
                    teamProjecList.Add(teamProjectNode.Resource.DisplayName, teamProjectNode.Resource.Properties["ProjectId"]);
                }

            }

            var orderedProjects = teamProjecList.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            return this.PartialView(orderedProjects);
        }

        

        /// <summary>
        /// Feed project properties
        /// </summary>
        /// <remarks>keep in the source code for futur use</remarks>
        /// <param name="tpc"></param>
        /// <param name="teamProjectNodes"></param>
        private static void FeedProjectPropertiesData(TfsTeamProjectCollection tpc, string projectUri)
        {
            ICommonStructureService css = tpc.GetService<ICommonStructureService3>();
            string projectName2;
            int templateId;
            string state;
            ProjectProperty[] properties;
            css.GetProjectProperties(projectUri, out projectName2, out state, out templateId, out properties);
            ProjectInfo projectInfo = css.GetProject(projectUri);
            NodeInfo[] nodeInfo = css.ListStructures(projectUri);
        }

        private static void FeedTestManagementData(ICollection<TestPlanDefinition> testPlanDefinitionCollection, TfsTeamProjectCollection tpc, string projectName)
        {
            ITestManagementService tms = tpc.GetService<ITestManagementService>();
            ITestManagementTeamProject tmtp = tms.GetTeamProject(projectName);
            ITestPlanHelper testPlanHelper = tmtp.TestPlans;
            ITestPlanCollection testPlanCollection = testPlanHelper.Query("Select * From TestPlan");
            foreach (ITestPlan testPlan in testPlanCollection)
            {
                TestPlanDefinition testPlanDefinition = new TestPlanDefinition()
                {
                    Name = testPlan.Name,
                    AreaPath = testPlan.AreaPath,
                    IterationPath = testPlan.Iteration,
                    Description = testPlan.Description,
                    Owner = testPlan.Owner.DisplayName,
                    State = testPlan.State.ToString(),
                    LastUpdate = testPlan.LastUpdated,
                    StartDate = testPlan.StartDate,
                    EndDate = testPlan.EndDate,
                    Revision = testPlan.Revision
                };
                testPlanDefinitionCollection.Add(testPlanDefinition);
            }
        }

        private static void FeedBuildData(ICollection<BuildDefinition> buildDefinitionCollection, TfsTeamProjectCollection tpc, string projectName)
        {
            IBuildServer bs = tpc.GetService<IBuildServer>();
            IBuildDefinition[] buildDefinitions = bs.QueryBuildDefinitions(projectName, QueryOptions.All);
            
            foreach (IBuildDefinition buildDefinition in buildDefinitions)
            {
                IBuildDetail[] buildDetails = bs.QueryBuilds(buildDefinition);
                int failedOrPartialCount = buildDetails.Count(x => x.Status == BuildStatus.Failed || x.Status == BuildStatus.PartiallySucceeded);
                IBuildDetail lastFailedBuild = buildDetails.Where(x => x.Status == BuildStatus.Failed).OrderBy(x => x.FinishTime).LastOrDefault();
                IBuildDetail lastSucceededBuild = buildDetails.Where(x => x.Status == BuildStatus.Succeeded).OrderBy(x => x.FinishTime).LastOrDefault();
                int buildCount = buildDetails.Count();
                
                BuildDefinition buildDef = new BuildDefinition()
                {
                    Name = buildDefinition.Name,
                    Enabled = buildDefinition.Enabled,
                    ContinuousIntegrationType = buildDefinition.ContinuousIntegrationType.ToString(),
                    FailedOrPartialRatio = failedOrPartialCount,
                    RetainedBuild = buildCount,
                    LastSuccess = lastSucceededBuild != null ? lastSucceededBuild.FinishTime : DateTime.MinValue,
                    LastFail = lastFailedBuild != null ? lastFailedBuild.FinishTime : DateTime.MinValue

                };
                buildDefinitionCollection.Add(buildDef);
            }
        }

        private static void FeedVersionControlData(ICollection<VersionControlItem> versionControlItemCollection, TfsTeamProjectCollection tpc, string projectName)
        {
            VersionControlServer vcs = tpc.GetService<VersionControlServer>();
            ItemSet items = vcs.GetItems("$/" + projectName + "/*", RecursionType.None);

            foreach (Item item in items.Items)
            {
                int itemChangeSetId = item.ChangesetId;
                DateTime lastInnerCheckInDate = DateTime.MinValue;
                int lastInnerChangeSetId = 0;
                IEnumerable history = vcs.QueryHistory(item.ServerItem, VersionSpec.Latest,
                    item.DeletionId,
                    RecursionType.Full,
                    null,
                    new ChangesetVersionSpec(itemChangeSetId),
                    VersionSpec.Latest,
                    Int32.MaxValue,
                    false,
                    false);
                IEnumerator enumerator = history.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    Changeset lastChangeSet = enumerator.Current as Changeset;
                    if (lastChangeSet != null)
                    {
                        lastInnerCheckInDate = lastChangeSet.CreationDate;
                        lastInnerChangeSetId = lastChangeSet.ChangesetId;
                    }
                }

                VersionControlItem vci = new VersionControlItem()
                {
                    DisplayName = item.ServerItem,
                    ItemChangeSetId = itemChangeSetId,
                    ItemLastCheckIn = item.CheckinDate,
                    InnerChangeSetId = lastInnerChangeSetId,
                    InnerLastCheckIn = lastInnerCheckInDate
                };
                versionControlItemCollection.Add(vci);


            }
        }

        public ActionResult WorkItemOverview(string id, string projectid)
        {
            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[] { new KeyValuePair<string, string>("ProjectID", projectid) },
                    false, CatalogQueryOptions.None);

            WorkItemOverviewModel wio = new WorkItemOverviewModel();
            if (teamProjectNodes.Count() == 1)
            {
                string projectName = teamProjectNodes[0].Resource.DisplayName;

                FeedWorkItemData(wio.WorkItemDefinitionCollection, tpc, projectName);
            }
            return PartialView(wio);
        }

        public ActionResult IdentityOverview(string id, string projectid)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(projectid))
            {
                return RedirectToAction("Index");
            }

            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[] { new KeyValuePair<string, string>("ProjectID", projectid) },
                    false, CatalogQueryOptions.None);
            IIdentityManagementService ims = tpc.GetService<IIdentityManagementService>();
            string projectUri;
            IdentityOverviewModel iom = new IdentityOverviewModel();
            if(teamProjectNodes.Count() == 1)
            {
                projectUri = teamProjectNodes[0].Resource.Properties["ProjectUri"];
                IdentityServiceManagementHelper.FeedIdentityData(iom.ApplicationGroupCollection, iom.UserCollection, ims, projectUri);
            }
            return PartialView(iom);
        }

        public ActionResult VersionControlOverview(string id, string projectid)
        {
            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[] { new KeyValuePair<string, string>("ProjectID", projectid) },
                    false, CatalogQueryOptions.None);

            string projectName;
            VersionControlOverviewModel vcom = new VersionControlOverviewModel();
            if (teamProjectNodes.Count() == 1)
            {
                projectName = teamProjectNodes[0].Resource.DisplayName;

                FeedVersionControlData(vcom.VersionControlItemCollection, tpc, projectName);
            }
            return PartialView(vcom);
        }

        public ActionResult BuildOverview(string id, string projectid)
        {
            if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(projectid))
            {
                return RedirectToAction("Index");
            }

            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[] { new KeyValuePair<string, string>("ProjectID", projectid) },
                    false, CatalogQueryOptions.None);

            string projectName;
            BuildOverviewModel bom = new BuildOverviewModel();
            if (teamProjectNodes.Count() == 1)
            {
                projectName = teamProjectNodes[0].Resource.DisplayName;

                FeedBuildData(bom.BuildDefinitionCollection, tpc, projectName);
            }
            return PartialView(bom);
        }

        public ActionResult TestManagementOverview(string id, string projectid)
        {
            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
            ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject }, new[] { new KeyValuePair<string, string>("ProjectID", projectid) },
                    false, CatalogQueryOptions.None);

            string projectName;
            TestManagementOverviewModel tmom = new TestManagementOverviewModel();
            if (teamProjectNodes.Count() == 1)
            {
                projectName = teamProjectNodes[0].Resource.DisplayName;

                FeedTestManagementData(tmom.TestPlanDefinitionCollection, tpc, projectName);
            }
            return PartialView(tmom);
        }

        private static void FeedWorkItemData(ICollection<WorkItemDefinition> workItemDefinitionCollection,
            TfsTeamProjectCollection tpc, string projectName)
        {
            WorkItemStore wis = tpc.GetService<WorkItemStore>();
            Microsoft.TeamFoundation.WorkItemTracking.Client.Project project = wis.Projects[projectName];

            foreach (WorkItemType wit in project.WorkItemTypes)
            {
                WorkItemDefinition witDefinition = new WorkItemDefinition() { Name = wit.Name, Description = wit.Description };

                IEnumerable<Category> categories = project.Categories.Where(x => x.WorkItemTypes.Contains(wit));
                foreach (Category item in categories)
	            {
                    witDefinition.Categories.Add(item.Name);
	            }

                FieldDefinition systemState = wit.FieldDefinitions.TryGetByName("System.State");
                foreach (string allowedValue in systemState.AllowedValues)
                {
                    int stateCount = wis.QueryCount("Select System.Id From WorkItems Where System.TeamProject = '"
                        + projectName
                        + "' And System.WorkItemType = '"
                        + witDefinition.Name
                        + "' And System.State = '"
                        + allowedValue
                        + "'");

                    witDefinition.StateCollection.Add(allowedValue, stateCount);
                }
                workItemDefinitionCollection.Add(witDefinition);

            }
        }

    }
}