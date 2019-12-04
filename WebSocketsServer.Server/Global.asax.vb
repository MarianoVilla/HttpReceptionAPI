Imports System.Web.Configuration
Imports System.Web.Http
Imports System.Web.Optimization
Imports Alpha.Utilidades.General

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Sub Application_Start()

        ConfigureLog()

        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        AreaRegistration.RegisterAllAreas()
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub

    Sub ConfigureLog()
        Dim ConfiguracionLog = New ConfiguracionLog

        With ConfiguracionLog
            .LoguearWebApi = False
            .NombreArchivoLog = "HttpApiLog"
            .ProyectoOrigen = "HttpReceptionAPI"
        End With

        LogsUtils.Configuracion = ConfiguracionLog

        If (WebConfigurationManager.AppSettings("Debug") = "1") Then
            LogsUtils.Configuracion.Debug = True
        ElseIf (ConfigurationManager.AppSettings("Debug") = "0") Then
            LogsUtils.Configuracion.Debug = False
        End If

    End Sub
End Class
