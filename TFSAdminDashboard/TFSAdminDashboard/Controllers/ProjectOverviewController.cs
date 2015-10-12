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

        private static List<BuildDefinition> FeedBuildData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashBuildHelper.FeedBuildData(tpc, projectName);
        }

        private static List<VersionControlItem> FeedVersionControlData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashVersionControlHelper.FeedVersionControlData(tpc, projectName);
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

                wio.SetWorkItemDefinitionCollection(FeedWorkItemData(tpc, projectName));
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

                vcom.SetVersionControlItemCollection(FeedVersionControlData(tpc, projectName));
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

                bom.SetBuildDefinitionCollection(FeedBuildData(tpc, projectName));
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

        private static List<WorkItemDefinition> FeedWorkItemData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashWorkItemHelper.FeedWorkItemData(tpc, projectName);
        }

    }
}