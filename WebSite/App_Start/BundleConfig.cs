using System.Web;
using System.Web.Optimization;

namespace WebSite
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/cache/scripts/MasterHead").Include(
                "/Content/scripts/libs/modernizr-2.0.6.min.js"
                , "/Content/scripts/libs/mdetect.min.js"
                , "/Content/scripts/libs/jquery-1.7.1.min.js"
                , "/Content/scripts/plugins/jquery.cookie.js"
                , "/Content/scripts/plugins/jquery.touchwipe.min.js"
                , "/Content/scripts/plugins/jquery.anystretch.min.js"
                , "/Content/scripts/app.js"
                , "/Content/scripts/Routings.js"
                , "/Content/scripts/plugins/jquery.routes.js"));
            
            bundles.Add(new StyleBundle("~/Content/cache/css/ScreenMaster").Include(
                "~/Content/css/main.css"
                ,"~/Content/scripts/plugins/qtip/jquery.qtip.css"
                ,"~/Content/scripts/plugins/qtip/dark/dark.css"));

            bundles.Add(new StyleBundle("~/Content/cache/css/StaticScreenMaster.css").Include(
                "/Content/css/main.css"));

            bundles.Add(new ScriptBundle("~/Content/cache/css/StaticMasterHead").Include(
                "/Content/scripts/libs/modernizr-2.0.6.min.js"
                ,"/Content/scripts/libs/mdetect.min.js"
                ,"/Content/scripts/libs/jquery-1.7.1.min.js"));

            bundles.Add(new StyleBundle("~/Content/cache/css/TextAdvAllMaster").Include(
                "/Content/css/textadv.css"));

            bundles.Add(new ScriptBundle("~/Content/cache/scripts/TextAdvHead").Include(
                "/Content/scripts/libs/modernizr-2.0.6.min.js"
                , "/Content/scripts/libs/jquery-1.7.1.min.js"
                , "/Content/scripts/plugins/jquery.tooltip.js"
                , "/Content/scripts/TextADV.js"));

            bundles.Add(new StyleBundle("~/Content/cache/css/MobileAllMaster").Include(
                "/Content/css/main.css"
                , "/Content/css/mobile.css"));

            bundles.Add(new ScriptBundle("~/Content/cache/scripts/MobileMasterHead").Include(
                "/Content/scripts/libs/modernizr-2.0.6.min.js"
                ,"/Content/scripts/libs/jquery-1.7.1.min.js"
                ,"/Content/scripts/app.js"));

            bundles.Add(new ScriptBundle("~/Content/cache/scripts/StaticMasterFoot").Include(
                "/Content/scripts/plugins/jquery.easing.1.3.js"
                ,"/Content/scripts/plugins/jquery.cycle.all.min.js"));

            bundles.Add(new ScriptBundle("~/Content/cache/scripts/MasterFoot").Include(
                "/Content/scripts/plugins/jquery.easing.1.3.js"
                ,"/Content/scripts/plugins/jquery.cookie.js"
                ,"/Content/scripts/plugins/jquery.cycle.all.min.js"
                ,"/Content/scripts/plugins/qtip/jquery.qtip.min.js"
                ,"/Content/scripts/plugins/jquery.fitvids.js"
                ,"/Content/scripts/main.js"
                ,"/Content/scripts/subscribe.js"));
        }
    }
}
