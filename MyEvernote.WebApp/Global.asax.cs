using MyEvernote.Common;
using MyEvernote.WebApp.App_Start;
using MyEvernote.WebApp.Init;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MyEvernote.WebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            App.Common = new WebCommon();
        }
    }
}