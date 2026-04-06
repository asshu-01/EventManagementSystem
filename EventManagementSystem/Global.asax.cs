using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace EventManagementSystem
{
    public class Global : HttpApplication
    {
        private static readonly Type[] ReferencedPages =
        {
            typeof(EventManagementSystem.Auth.ForgotPassword),
            typeof(EventManagementSystem.Auth.Register),
            typeof(EventManagementSystem.Auth.Login),
            typeof(EventManagementSystem.Auth.ResetPassword),
            typeof(EventManagementSystem.Admin.ManageEvents),
            typeof(EventManagementSystem.Admin.ViewBookings),
            typeof(EventManagementSystem.Admin.Dashboard)
        };

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}