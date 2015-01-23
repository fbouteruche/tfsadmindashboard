using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TFSAdminDashboard
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "DashboardView", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProjectLevel",
                url: "{controller}/{action}/{id}/{projectid}"
            );
        }
    }
}
