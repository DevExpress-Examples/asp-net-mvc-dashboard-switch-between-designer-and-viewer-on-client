using System;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using DevExpress.Web;

namespace Dashboard_MvcDashboard_2010
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            routes.MapDashboardRoute();

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);            
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            ASPxWebControl.CallbackError += Application_Error;

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage("~/App_Data/Dashboards");
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);

            DashboardSqlDataSource xmlDataSource = new DashboardSqlDataSource("XML Data Source", "xmlConnection");
            SelectQuery countriesQuery = SelectQueryFluentBuilder
                .AddTable("Countries")
                .SelectColumns("Country", "Latitude", "Longitude", "Year", "EnergyType", "Production", "Import")
                .Build("Countries");
            xmlDataSource.Queries.Add(countriesQuery);

            DashboardOlapDataSource olapDataSource = new DashboardOlapDataSource("OLAP Data Source", "olapConnection");

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            dataSourceStorage.RegisterDataSource("xmlDataSource1", xmlDataSource.SaveToXml());
            dataSourceStorage.RegisterDataSource("olapDataSource1", olapDataSource.SaveToXml());
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);

            DashboardConfigurator.Default.ConfigureDataConnection += 
                new ConfigureDataConnectionWebEventHandler(Default_ConfigureDataConnection);
        }

        void Default_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
            if (e.ConnectionName == "xmlConnection") {
                string databasePath = HostingEnvironment.MapPath("~/App_Data/DashboardEnergyStatictics.xml");
                e.ConnectionParameters = new XmlFileConnectionParameters(databasePath);
            }
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
        }
    }
}