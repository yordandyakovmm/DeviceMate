using System.Web;
using System.Web.Optimization;

namespace DeviceMate.Web
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryDataTable").Include(
                        "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/bundles/multiselectcss").Include(
                "~/Content/jquery.ui.all.css",
                "~/Content/jquery.multiselect.css"
            ));
            bundles.Add(new StyleBundle("~/bundles/themes/base/css").Include(
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

            bundles.Add(new StyleBundle("~/bundles/themes/bootstrap/css").Include(
                       "~/Content/themes/bootstrap/css/bootstrap.css",
                       "~/Content/themes/bootstrap/css/bootstrap-responsive.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/openid").Include(
                        "~/Scripts/openid-jquery.js",
                        "~/Scripts/openid-en.js"));

            bundles.Add(new StyleBundle("~/bundles/css/openid").Include(
                        "~/Content/openid-shadow.css",
                        "~/Content/openid.css"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxhandlers").Include(
                        "~/Scripts/Common/GlobalAjaxHandlers.js"));
            // -------------------------------------------------------------------
            // -------------------------------UI V2------------------------------
            // ------------------------------------------------------------------
            bundles.Add(new ScriptBundle("~/bundles/UI/js")
                .Include(
                    // External directives:
                    "~/UI/js/directives/dropdownDirective.js",
                    "~/UI/js/app.js",
                    "~/UI/js/config.js",
                    "~/UI/js/app.constants.js",
                    // Internal directives:
                    "~/UI/js/directives/filterListDirective.js",
                    "~/UI/js/directives/itemHistoryDirective.js",
                    "~/UI/js/directives/scrollDirective.js",
                    "~/UI/js/directives/usernavDirective.js",

                    "~/UI/js/controllers/AccessoryCtrl.js",
                    "~/UI/js/controllers/AccessorySearchCtrl.js",
                    "~/UI/js/controllers/DeviceEditCtrl.js",
                    "~/UI/js/controllers/DeviceSearchCtrl.js",
                    "~/UI/js/controllers/HomeCtrl.js",
                    "~/UI/js/controllers/ModalCtrl.js",
                    "~/UI/js/controllers/ModalNewFilterCtrl.js",
                    "~/UI/js/controllers/RootCtrl.js",
                    "~/UI/js/controllers/SearchCtrl.js",

                    "~/UI/js/models/accessoryModel.js",
                    "~/UI/js/models/deviceModel.js",
                    "~/UI/js/models/filterModel.js",
                    "~/UI/js/models/teamModel.js",
                    "~/UI/js/models/userModel.js",

                    "~/UI/js/services/accessorySvc.js",
                    "~/UI/js/services/colorHexSvc.js",
                    "~/UI/js/services/filterSvc.js",
                    "~/UI/js/services/itemHistorySvc.js",
                    "~/UI/js/services/modalSvc.js",
                    "~/UI/js/services/restSvc.js",
                    "~/UI/js/services/userSvc.js",
                    "~/UI/js/services/configSvc.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/UI/vendor")
                .Include(
                    "~/UI/js/vendor/angular/angular.js",
                    "~/UI/js/vendor/angular-ui-router/release/angular-ui-router.js",
                    "~/UI/js/vendor/angular-ui-bootstrap/src/dropdown/dropdown.js",
                    "~/UI/js/vendor/moment/moment.js",
                    "~/UI/js/vendor/angular-moment/angular-moment.js",
                    "~/UI/js/vendor/ng-dialog/js/ngDialog.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/UI/css")
                .Include(
                    "~/UI/css/normalize.css",
                    "~/UI/js/vendor/bootstrap/dist/css/bootstrap.css",
                    "~/UI/js/vendor/ng-dialog/css/ngDialog.css",
                    "~/UI/js/vendor/ng-dialog/css/ngDialog-theme-default.css",
                    "~/UI/js/vendor/ladda/css/ladda-themeless.css",
                    "~/UI/css/app.css"
                ));
        }
    }
}
