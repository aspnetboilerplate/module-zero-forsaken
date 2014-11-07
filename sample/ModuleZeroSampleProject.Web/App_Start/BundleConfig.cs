using System.Web.Optimization;

namespace ModuleZeroSampleProject.Web
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            //VENDOR RESOURCES

            //~/Bundles/App/vendor/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/vendor/css")
                    .Include(
                        "~/Content/themes/base/all.css",

                        "~/Content/bootstrap.min.css",

                        "~/bower_components/bootstrap-material-design/dist/css/ripples.min.css",
                        "~/bower_components/bootstrap-material-design/dist/css/material-wfont.min.css",
                        "~/bower_components/angular-material/angular-material.min.css",
                        "~/bower_components/bower_components/angular-material/themes/default-theme.css",

                        "~/Content/toastr.min.css",
                        "~/Content/flags/famfamfam-flags.css",
                        "~/Content/font-awesome.min.css"
                    )
                );

            //~/Bundles/App/vendor/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/vendor/js")
                    .Include(
                        "~/Abp/Framework/scripts/utils/ie10fix.js",
                        "~/Scripts/json2.min.js",

                        "~/Scripts/modernizr-2.8.3.js",
                        
                        "~/Scripts/jquery-2.1.1.min.js",
                        "~/Scripts/jquery-ui.min-1.11.1.js",

                        "~/Scripts/bootstrap.min.js",
                        "~/bower_components/bootstrap-material-design/dist/css/ripples.min.js",
                        "~/bower_components/bootstrap-material-design/dist/css/material.min.js",

                        "~/Scripts/moment-with-locales.min.js",
                        "~/Scripts/jquery.blockUI.min.js",
                        "~/Scripts/toastr.min.js",
                        "~/Scripts/others/spinjs/spin.js",
                        "~/Scripts/others/spinjs/jquery.spin.js",

                        "~/bower_components/angular/angular.min.js",
                        "~/bower_components/angular-animate/angular-animate.min.js",
                        "~/bower_components/angular-sanitize/angular-sanitize.min.js",
                        "~/bower_components/angular-aria/angular-aria.min.js",
                        "~/bower_components/angular-ui-router/release/angular-ui-router.min.js",

                        "~/bower_components/hammerjs/hammer.min.js",

                        "~/bower_components/angular-material/angular-material.min.js",
                        
                        "~/Scripts/angular-ui/ui-bootstrap.min.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.min.js",
                        "~/Scripts/angular-ui/ui-utils.min.js",

                        "~/Abp/Framework/scripts/abp.js",
                        "~/Abp/Framework/scripts/libs/abp.jquery.js",
                        "~/Abp/Framework/scripts/libs/abp.toastr.js",
                        "~/Abp/Framework/scripts/libs/abp.blockUI.js",
                        "~/Abp/Framework/scripts/libs/abp.spin.js",
                        "~/Abp/Framework/scripts/libs/angularjs/abp.ng.js"
                    )
                );

            //APPLICATION RESOURCES

            //~/Bundles/App/Main/css
            bundles.Add(
                new StyleBundle("~/Bundles/App/Main/css")
                    .IncludeDirectory("~/App/Main", "*.css", true)
                );

            //~/Bundles/App/Main/js
            bundles.Add(
                new ScriptBundle("~/Bundles/App/Main/js")
                    .IncludeDirectory("~/App/Main", "*.js", true)
                );
        }
    }
}