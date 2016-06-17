using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TFSAdminDashboard.Models;

namespace TFSAdminDashboard.Controllers
{
    public class ProjectOverviewController : Controller
    {
        // GET: ProjectOverview
        public ActionResult Index(string collectionName)
        {
            List<SelectListItem> tpcList = new List<SelectListItem>();
            List<SelectListItem> tpList = new List<SelectListItem>();

            // Get the catalog of team project collections
            ICollection<ProjectCollectionDefinition> collectionNodes = DashTeamProjectHelper.GetCollections();

            foreach (ProjectCollectionDefinition collectionNode in collectionNodes)
            {
                // Use the InstanceId property to get the team project collection
                tpcList.Add(
                    new SelectListItem
                    {
                        Text = collectionNode.Name,
                        Value = collectionNode.Name,
                        Selected = collectionNode.Name == collectionName
                    });
            }

            // a collection Id is supplied
            if (!string.IsNullOrWhiteSpace(collectionName))
            {
                foreach (ProjectDefinition proj in DashTeamProjectHelper.GetProjects(collectionName))
                {
                    tpList.Add(
                   new SelectListItem
                   {
                       Text = proj.Name,
                       Value = proj.Name,
                   });
                }

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

        private static List<TestPlanDefinition> FeedTestManagementData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashTestPlanHelper.FeedTestPlanData(tpc, projectName);
        }

        private static List<BuildDefinition> FeedBuildData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashBuildHelper.FeedBuildData(tpc.DisplayName.Split('\\')[1], projectName);
        }

        private static List<VersionControlItem> FeedVersionControlData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashGitHelper.FeedGitData(tpc.DisplayName.Split('\\')[1], projectName);
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
            if (teamProjectNodes.Count() == 1)
            {
                projectUri = teamProjectNodes[0].Resource.Properties["ProjectUri"];
                iom.SetApplicationAndUserGroupCollection(DashIdentityManagementHelper.FeedIdentityData(tpc, projectUri));
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
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(projectid))
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

                tmom.SetTestPlanDefinitionCollection(FeedTestManagementData(tpc, projectName));
            }
            return PartialView(tmom);
        }

        private static List<WorkItemDefinition> FeedWorkItemData(TfsTeamProjectCollection tpc, string projectName)
        {
            return DashWorkItemHelper.FeedWorkItemData(tpc, projectName);
        }

    }
}