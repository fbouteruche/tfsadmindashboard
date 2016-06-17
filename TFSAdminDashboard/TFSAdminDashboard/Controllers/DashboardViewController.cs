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
using TFSDataService;

namespace TFSAdminDashboard.Controllers
{
    public class DashboardViewController : TFAdminControllerBase
    {
        // GET: DashboardView
        public ActionResult Index()
        {
            var projectCollections = DashTeamProjectHelper.GetCollections();

            OrganizationalOverviewModel dashb = new OrganizationalOverviewModel()
            {
                ProjectCollectionCollection = projectCollections,

                ProjectCount = projectCollections.Sum(x => x.ProjectCount)

            };

            return View(dashb);
        }
    }
}