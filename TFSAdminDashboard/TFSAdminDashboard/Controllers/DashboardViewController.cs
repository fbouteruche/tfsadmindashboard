using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TFSAdminDashboard.Models;
using Microsoft.TeamFoundation.Framework.Client;

namespace TFSAdminDashboard.Controllers
{
    public class DashboardViewController : TFAdminControllerBase
    {
        // GET: DashboardView
        public ActionResult Index()
        {
            var projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);

            OrganizationalOverviewModel dashb = new OrganizationalOverviewModel()
            {
                ProjectCollectionCollection = projectCollections,
                ProjectCount = CatalogNodeBrowsingHelper.GetTeamProjects(configurationServer.CatalogNode, true).Count(),
                UserCount = IdentityServiceManagementHelper.GetAllIdentityCount(configurationServer, projectCollections)
            };

            return View(dashb);
        }
    }
}