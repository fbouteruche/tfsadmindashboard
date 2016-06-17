using System;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.Models;

namespace TFSAdminDashboard.Controllers
{
    public class ServerOverviewController : Controller
    {
       
        // GET: TfsOverview
        public ActionResult Index()
        {
            ServerOverviewModel som = new ServerOverviewModel() { Url = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)  };
                        
            return View(som);
        }

        public ActionResult CollectionOverview()
        {
            CollectionOverviewModel com = new CollectionOverviewModel();
            com.ProjectCollections = DashTeamProjectHelper.GetCollections();
            return PartialView(com);
        }

        public ActionResult IdentityOverview()
        {
            IdentityOverviewModel iom = new IdentityOverviewModel();
            //TODO re-plug this
            iom.SetApplicationAndUserGroupCollection(null);
            return PartialView(iom);
        }
    }
}