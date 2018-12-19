using System.Web;
using System.Web.Optimization;

namespace MowerHelper
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/ThScripts/Apptwizard").Include("~/Scripts/jquery-ui-1.8.11.js",
                "~/Scripts/Expire.js"));
            bundles.Add(new ScriptBundle("~/ThScripts/ElectricianHelpscripts").Include(
                "~/Scripts/jquery-1.7.1.js",
                "~/Scripts/jquery-ui-1.8.20.js",
                "~/Scripts/scriptCognetwork.js",
                "~/Scripts/flytooltip.js"));
            bundles.Add(new ScriptBundle("~/ThScripts/THViewscripts").Include(
                "~/Scripts/jquery-1.7.1.js",
                "~/Scripts/jquery-ui-1.8.20.js",
                "~/Scripts/flytooltip.js"));
            bundles.Add(new ScriptBundle("~/ThScripts/ThJquery").Include("~/Scripts/jquery-1.7.1.js",
                "~/Scripts/jquery-ui-1.8.20.js"));
            bundles.Add(new ScriptBundle("~/ThScripts/ThAjaxscripts").Include("~/Scripts/jquery-1.7.1.js",
                "~/Scripts/jquery-ui-1.8.20.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));
            bundles.Add(new ScriptBundle("~/ThScripts/ThOtherscripts").Include("~/SCRIPTS/scriptCognetwork.js",
                "~/SCRIPTS/flytooltip.js"));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/DietitianHelpCSS").Include("~/Content/MowerHelperCSS/style.css"));
            bundles.Add(new StyleBundle("~/Content/themes/base/TherpistStyles").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.theme.css",
                "~/Content/flytooltip.css",
                "~/Content/MowerHelperCSS/style.css"));
            bundles.Add(new StyleBundle("~/content/ProviderProfile/css").Include("~/Content/ProviderProfile.css"));
            bundles.Add(new StyleBundle("~/Content/flytooltip.css").Include
            (
            "~/Content/flytooltip.css"
            ));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));


            bundles.Add(new StyleBundle("~/Content/themes/base/autocomplete").Include(
                "~/Content/themes/base/jquery.ui.autocomplete.css"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Thscripts").Include(
                "~/Scripts/jquery-1.7.1.js",
                "~/Scripts/jquery-ui-1.8.20.js"
                ));
            bundles.Add(new ScriptBundle("~/Scripts/Thscriptscript").Include(
               "~/Scripts/jquery-1.7.1.js",
               "~/Scripts/jquery-ui-1.8.20.js",
               "~/Scripts/scriptCognetwork.js"
               ));
            bundles.Add(new ScriptBundle("~/ThScripts/mautocomplete").Include(
            "~/Scripts/jquery.mcautocomplete.js"
           ));

            bundles.Add(new ScriptBundle("~/ThScripts/Thjquerycripts").Include(
             "~/Scripts/jquery.unobtrusive-ajax.js"
            ));
            bundles.Add(new StyleBundle("~/Content/themes/base/jqueryCSS").Include(

             "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.theme.css",
                 "~/Content/MowerHelperCSS/style.css",
                  "~/Content/flytooltip.css"
              ));
            bundles.Add(new StyleBundle("~/Content/themes/base/PublicjqueryCSS").Include(

             "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.theme.css"
              ));
            bundles.Add(new StyleBundle("~/Content/styles").Include(
                "~/Content/common.css",
                "~/Content/style.css"

                ));
            bundles.Add(new ScriptBundle("~/Scripts/jscript_zjcarousellite.js").Include(
                "~/Scripts/jscript_zjcarousellite.js"

                  ));
            bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/common.css"
                ));
            bundles.Add(new StyleBundle("~/Content/MainCSS").Include(
                 "~/Content/style.css"
                 ));
            bundles.Add(new ScriptBundle("~/Scripts/PublicPagesScript").Include(
                "~/Scripts/PublicPagesScript.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/Printprofile.css").Include(
                "~/Content/Printprofile.css"
                ));
            bundles.Add(new ScriptBundle("~/SCRIPTS/scriptCognetwork").Include(
                "~/SCRIPTS/scriptCognetwork.js"));
            bundles.Add(new ScriptBundle("~/SCRIPTS/outflytooltip").Include(
               "~/SCRIPTS/outflytooltip.js"));
        }
    }
}