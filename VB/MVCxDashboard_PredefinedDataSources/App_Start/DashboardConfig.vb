Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWeb
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports System.Web.Hosting
Imports System.Web.Routing
Imports DevExpress.DashboardWeb.Mvc

Namespace MVCxDashboard_PredefinedDataSources
    Public NotInheritable Class DashboardConfig

        Private Sub New()
        End Sub

        Public Shared Sub RegisterService(ByVal routes As RouteCollection)
            routes.MapDashboardRoute("dashboardControl")

            Dim dashboardFileStorage As New DashboardFileStorage("~/App_Data/Dashboards")
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage)

            Dim xmlDataSource As New DashboardSqlDataSource("XML Data Source", "xmlConnection")
            Dim countriesQuery As SelectQuery = SelectQueryFluentBuilder.AddTable("Countries").SelectColumns("Country", "Latitude", "Longitude", "Year", "EnergyType", "Production", "Import").Build("Countries")
            xmlDataSource.Queries.Add(countriesQuery)

            Dim olapDataSource As New DashboardOlapDataSource("OLAP Data Source", "olapConnection")

            Dim dataSourceStorage As New DataSourceInMemoryStorage()
            dataSourceStorage.RegisterDataSource("xmlDataSource1", xmlDataSource.SaveToXml())
            dataSourceStorage.RegisterDataSource("olapDataSource1", olapDataSource.SaveToXml())
            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage)

            AddHandler DashboardConfigurator.Default.ConfigureDataConnection, AddressOf Default_ConfigureDataConnection
        End Sub

        Private Shared Sub Default_ConfigureDataConnection(ByVal sender As Object, ByVal e As ConfigureDataConnectionWebEventArgs)
            If e.ConnectionName = "xmlConnection" Then
                Dim databasePath As String = HostingEnvironment.MapPath("~/App_Data/DashboardEnergyStatictics.xml")
                e.ConnectionParameters = New XmlFileConnectionParameters(databasePath)
            End If
        End Sub
    End Class
End Namespace