using MarkIT.Controllers;
using MarkIT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MarkIT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<BookmarkDBContext>(new DropCreateDatabaseIfModelChanges<BookmarkDBContext>());
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

		protected void Application_EndRequest()
		{
			if (Context.Response.StatusCode == 404)
			{
				Response.Clear();
				var routedata = new RouteData();
				routedata.Values["controller"] = "Bookmark";
				routedata.Values["action"] = "Index";

				IController c = new BookmarkController();
				c.Execute(new RequestContext(new HttpContextWrapper(Context), routedata));
			}
		}
	}
}
