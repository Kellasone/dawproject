using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MarkIT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.MapRoute(
				name: "SaveBookmark"
				url: "Bookmark/SaveBookmark/{id}/{categoryid}");
            routes.MapRoute(
                name: "Bookmarks",
                url: "{controller}/{action}/{id}",
                defaults: new
                {
                    controller = "Bookmark",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
			
        }
    }
}
