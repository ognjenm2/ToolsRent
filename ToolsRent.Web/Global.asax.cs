﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;

namespace ToolsRent.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(System.Web.Hosting.HostingEnvironment.MapPath("~/bin/Logs/ToolReservationsLog.txt"))
                .CreateLogger();
            Log.Debug("App start..");
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            // Log unhandled exceptions
            var exception = Server.GetLastError();
            Log.Error(exception, "Unhandled exception occurred");
        }

    }
    
}
