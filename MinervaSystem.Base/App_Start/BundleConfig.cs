using System.Web.Optimization;

namespace MinervaSystem.Base
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //------------------------ Script Bundle ----------------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(   //Default load
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/GlobalFunctions.js"));

            bundles.Add(new ScriptBundle("~/bundles/cldr").Include(
                "~/Scripts/cldr.js",
                "~/Scripts/cldr/event.js",
                "~/Scripts/cldr/supplemental.js"));


            bundles.Add(new ScriptBundle("~/bundles/globalize").Include(
                "~/Scripts/globalize.js",
                "~/Scripts/globalize/date.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.globalize.js",
                "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/chosen.jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(    //Default load
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(    //Default load
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js",
                "~/Scripts/jquery.dataTables.columnFilter.custom.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/telerik/kendo.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/jquery-2.2.3.min.js",
                "~/Scripts/layout.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/GlobalFunctions.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap.min.js",
                "~/Scripts/DataTables/dataTables.responsive.min.js",
                "~/Scripts/DataTables/moment.js",
                "~/Scripts/DataTables/datetime-moment.js",
                "~/Scripts/DataTables/responsive.bootstrap.min.js",
                "~/Scripts/Admin/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/DataTables/Allbuttons").Include(
                "~/Scripts/DataTables/dataTables.buttons.js",
                "~/Scripts/DataTables/buttons.bootstrap.min.js",
                "~/Scripts/DataTables/jszip.min.js",
                "~/Scripts/DataTables/pdfmake.min.js",
                "~/Scripts/DataTables/vfs_fonts.js",
                "~/Scripts/DataTables/buttons.print.min.js",
                "~/Scripts/DataTables/buttons.html5.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Common").Include(
                "~/Scripts/Common/utility.js"));

            //------------------------ Style Bundle ----------------------------
            bundles.Add(new StyleBundle("~/Content/css").Include(   //Default load
                "~/Content/bootstrap.css",
                //"~/Content/DataTables/dataTables.bootstrap.min.css",
                //"~/Content/DataTables/responsive.bootstrap.min.css",
                //"~/Content/site.bootstrap.css",
                //"~/Content/simple-sidebar.css",
                "~/Content/font-awesome.min.css",
                "~/Content/Admin/AdminLTE.css",
                "~/Content/Admin/skin-purple.css"
                //"~/Content/site.css"
                ));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                "~/Content/themes/base/all.css",
                "~/Content/chosen.css"));

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                "~/Content/DataTables/css/dataTables.bootstrap.min.css",
                "~/Content/DataTables/css/responsive.bootstrap.min.css",
                "~/Content/site.datatables.css"));

            bundles.Add(new StyleBundle("~/Content/kendo.office365").Include(
                "~/Content/telerik/kendo.common-office365.min.css",
                "~/Content/site.kendo.css",
                "~/Content/telerik/kendo.rtl.css",
                "~/Content/telerik/kendo.office365.min.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
