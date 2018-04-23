using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Sql;
using System.Web.Hosting;
using System.Web.Routing;

namespace MVCxDashboard_PredefinedDataSources {
    public static class DashboardConfig {
        public static void RegisterService(RouteCollection routes) {
            routes.MapDashboardRoute("dashboardControl");

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

            DashboardConfigurator.Default.ConfigureDataConnection += Default_ConfigureDataConnection;
        }

        private static void Default_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
            if (e.ConnectionName == "xmlConnection") {
                string databasePath = HostingEnvironment.MapPath("~/App_Data/DashboardEnergyStatictics.xml");
                e.ConnectionParameters = new XmlFileConnectionParameters(databasePath);
            }
        }
    }
}