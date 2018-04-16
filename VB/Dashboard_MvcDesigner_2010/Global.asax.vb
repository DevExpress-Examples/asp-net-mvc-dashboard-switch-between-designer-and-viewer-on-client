Imports System
Imports System.Web.Hosting
Imports System.Web.Mvc
Imports System.Web.Routing
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWeb
Imports DevExpress.DashboardWeb.Mvc
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.Web

Namespace Dashboard_MvcDesigner_2010
    Public Class MvcApplication
        Inherits System.Web.HttpApplication

        Public Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
            filters.Add(New HandleErrorAttribute())
        End Sub

        Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}")
            routes.MapDashboardRoute()

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                            New With {Key .controller = "Home", Key .action = "Index", Key .id = UrlParameter.Optional})

        End Sub
        Protected Sub Application_Start()
            AreaRegistration.RegisterAllAreas()
            RegisterGlobalFilters(GlobalFilters.Filters)
            RegisterRoutes(RouteTable.Routes)
            ModelBinders.Binders.DefaultBinder = New DevExpress.Web.Mvc.DevExpressEditorsBinder()
            AddHandler ASPxWebControl.CallbackError, AddressOf Application_Error

            Dim dashboardFileStorage As New DashboardFileStorage("~/App_Data/Dashboards")
            DashboardService.SetDashboardStorage(dashboardFileStorage)

            Dim xmlDataSource As New DashboardSqlDataSource("XML Data Source", "xmlConnection")
            Dim countriesQuery As SelectQuery =
                SelectQueryFluentBuilder.AddTable("Countries").SelectColumns("Country", "Latitude", _
                                                                             "Longitude", "Year", _
                                                                             "EnergyType", "Production", _
                                                                             "Import").Build("Countries")
            xmlDataSource.Queries.Add(countriesQuery)

            Dim olapDataSource As New DashboardOlapDataSource("OLAP Data Source", "olapConnection")

            Dim dataSourceStorage As New DataSourceInMemoryStorage()
            dataSourceStorage.RegisterDataSource("xmlDataSource1", xmlDataSource.SaveToXml())
            dataSourceStorage.RegisterDataSource("olapDataSource1", olapDataSource.SaveToXml())
            DashboardService.SetDataSourceStorage(dataSourceStorage)

            AddHandler DashboardService.DataApi.ConfigureDataConnection, AddressOf DataApi_ConfigureDataConnection
        End Sub

        Private Sub DataApi_ConfigureDataConnection(ByVal sender As Object, _
                                                    ByVal e As ServiceConfigureDataConnectionEventArgs)
            If e.ConnectionName = "xmlConnection" Then
                Dim databasePath As String = HostingEnvironment.MapPath("~/App_Data/DashboardEnergyStatictics.xml")
                e.ConnectionParameters = New XmlFileConnectionParameters(databasePath)
            End If
        End Sub

        Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
            Dim exception As Exception = System.Web.HttpContext.Current.Server.GetLastError()
        End Sub
    End Class
End Namespace