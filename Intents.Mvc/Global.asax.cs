using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Intents;
using Intents.Web;

namespace Intents.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IntentManager.GetIntentManager(new SessionRepository()).Register(new Intent<string>()
            {
                name = "page-request",
                trigger = "page-request",
                action = (data) =>
                {
                    Console.WriteLine("New Page Request");
                }
            });
        }

        protected void Session_Start()
        {
            IntentManager.GetIntentManager().CreateSessionStorage();
        }

        protected void Application_BeginRequest()
        {
            IntentManager.GetIntentManager().Trigger("login");
        }
    }
}
