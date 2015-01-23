using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Controllers
{
    public class DashboardViewController : Controller
    {
        private static Uri tfsUri = new Uri(TFSAdminDashboard.Properties.Settings.Default.TfsUrl);

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(tfsUri, new NetworkCredential(TFSAdminDashboard.Properties.Settings.Default.TfsUserName, TFSAdminDashboard.Properties.Settings.Default.TfsPassword));


        // GET: DashboardView
        public ActionResult Index()
        {
            ICollection<ProjectDefinition> projects = CatalogNodeBrowsingHelper.GetTeamProjects(configurationServer.CatalogNode, true);
            return View();
        }
    }
}