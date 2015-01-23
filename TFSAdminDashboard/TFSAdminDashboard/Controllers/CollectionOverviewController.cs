using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TFSAdminDashboard.Models;

namespace TFSAdminDashboard.Controllers
{
    public class CollectionOverviewController : Controller
    {
        private static Uri tfsUri = new Uri(TFSAdminDashboard.Properties.Settings.Default.TfsUrl);

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(tfsUri, new NetworkCredential(TFSAdminDashboard.Properties.Settings.Default.TfsUserName, Environment.GetEnvironmentVariable(TFSAdminDashboard.Properties.Settings.Default.TfsPasswordEnvironmentVariable)));

        // GET: CollectionOverview
        public ActionResult Index(string id)
        {
            // Get the catalog of team project collections
            ICollection<ProjectCollectionDefinition> projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);

            List<SelectListItem> tpcList = new List<SelectListItem>();

            foreach (ProjectCollectionDefinition collection in projectCollections)
            {
                // Use the InstanceId property to get the team project collection

                tpcList.Add(new SelectListItem { 
                    Text = collection.Name,
                    Value = collection.InstanceId.ToString(),
                    Selected = collection.InstanceId.ToString() == id
                });
            }

            ViewBag.TpcList = tpcList;

            return View();
        }

        public ActionResult CollectionOverview()
        {
            return PartialView();
        }

        public ActionResult IdentityOverview(string id)
        {
            IdentityOverviewModel iom = new IdentityOverviewModel();
            if (!string.IsNullOrWhiteSpace(id))
            {
                TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
                IIdentityManagementService ims = tpc.GetService<IIdentityManagementService>();

                IdentityServiceManagementHelper.FeedIdentityData(iom.ApplicationGroupCollection, iom.UserCollection, ims, null);
            }
            return PartialView(iom);
        }

        public ActionResult ProjectOverview(string id)
        {
            ProjectOverviewModel pom = new ProjectOverviewModel();
            if(!string.IsNullOrWhiteSpace(id))
            {
                TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
                pom.Projects = CatalogNodeBrowsingHelper.GetTeamProjects(tpc.CatalogNode, false);
            }
            return PartialView(pom);
        }

        public ActionResult BuildOverview(string id)
        { 
            BuildOverviewModel bom = new BuildOverviewModel();
            if(!string.IsNullOrWhiteSpace(id))
            {
                TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(new Guid(id));
                ReadOnlyCollection<CatalogNode> teamProjectNodes = tpc.CatalogNode.QueryChildren(
                    new[] { CatalogResourceTypes.TeamProject },
                    false, CatalogQueryOptions.None);

                IBuildServer bs = tpc.GetService<IBuildServer>();
                foreach (CatalogNode teamProjectNode in teamProjectNodes)
                {
                    BuildServerHelper.FeedBuildDefinition(bom.BuildDefinitionCollection, bs, teamProjectNode.Resource.DisplayName);
                }
            }
            return PartialView(bom);
        }
    }
}