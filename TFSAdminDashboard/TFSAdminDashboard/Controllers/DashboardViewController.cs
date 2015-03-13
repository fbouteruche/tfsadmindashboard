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
    public class DashboardViewController : TFAdminControllerBase
    {
        // GET: DashboardView
        public ActionResult Index()
        {
            //ICollection<ProjectDefinition> projects = CatalogNodeBrowsingHelper.GetTeamProjects(configurationServer.CatalogNode, true);
            return View();
        }
    }
}