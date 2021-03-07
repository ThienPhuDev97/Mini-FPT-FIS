using System.Web;
using System.Web.Optimization;

namespace WebApplication1
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            //Admin
            bundles.Add(new ScriptBundle("~/bundles/bootstrapAdmin").Include(
                       "~/Asset/plugins/jquery/jquery.min.js",
                        "~/Asset/plugins/jquery-ui/jquery-ui.min.js",
                        "~/Asset/plugins/bootstrap/js/bootstrap.bundle.min.js",
                        "~/Asset/plugins/chart.js/Chart.min.js",
                        "~/Asset/plugins/sparklines/sparkline.js",
                         "~/Asset/plugins/jqvmap/jquery.vmap.min.js",
                         "~/Asset/plugins/jqvmap/maps/jquery.vmap.usa.js",
                         "~/Asset/plugins/jquery-knob/jquery.knob.min.js",
                         "~/Asset/plugins/moment/moment.min.js",
                         "~/Asset/plugins/daterangepicker/daterangepicker.js",
                        "~/Asset/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                        "~/Asset/plugins/summernote/summernote-bs4.min.js",
                         "~/Asset/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                         "~/Asset/dist/js/adminlte.js",
                         "~/Asset/dist/js/demo.js",
                          "~/Asset/dist/js/pages/dashboard.js",

                           //"~/Scripts/HandleEdit.js",
                           "~/Scripts/bootbox/bootbox.all.js",
                           "~/Scripts/bootbox/bootbox.all.min.js",
                           "~/Scripts/bootbox/bootbox.js",
                           "~/Scripts/bootbox/bootbox.locales.js",
                           "~/Scripts/bootbox/bootbox.locales.min.js",
                           "~/Scripts/bootbox/bootbox.min.js"

                      ));

            bundles.Add(new StyleBundle("~/Content/cssAdmin").Include(
                      "~/Asset/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                      "~/Asset/plugins/fontawesome-free/css/all.min.css",
                      "~/Asset/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                      "~/Asset/plugins/jqvmap/jqvmap.min.css",
                      "~/Asset/dist/css/adminlte.min.css",
                      "~/Asset/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                       "~/Asset/plugins/daterangepicker/daterangepicker.css",
                        "~/Asset/plugins/summernote/summernote-bs4.min.css",
                         "~/Asset/dist/css/Custom.css"));
        }
    }
}
//"~/Scripts/HandleCustom.js",