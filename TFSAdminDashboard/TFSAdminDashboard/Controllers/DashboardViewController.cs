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
            OrganizationalOverviewModel dashb = new OrganizationalOverviewModel()
            {
                ProjectCollectionCollection = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode),
                ProjectCount = CatalogNodeBrowsingHelper.GetTeamProjects(configurationServer.CatalogNode, true).Count(),
                UserCount = IdentityServiceManagementHelper.GetAllIdentityCount(configurationServer.GetService<IIdentityManagementService>())
            };

            return View(dashb);
        }
    }
}