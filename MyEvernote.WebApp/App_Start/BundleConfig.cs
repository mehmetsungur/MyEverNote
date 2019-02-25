using System.Web.Optimization;

namespace MyEvernote.WebApp.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //CSS - StyleBundles
            bundles.Add(new StyleBundle("~/css/using").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/shop-homepage.css"));

            //JS - ScriptsBundles
            bundles.Add(new ScriptBundle("~/js/using").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/bootstrap.min.js"));
        }
    }
}