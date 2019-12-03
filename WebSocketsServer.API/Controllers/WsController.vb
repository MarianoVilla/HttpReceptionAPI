Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Mvc
Imports Microsoft.Web.WebSockets

Namespace Controllers
    Public Class WsController
        Inherits ApiController

        ' GET: Ws
        Public Function GetValues() As IHttpActionResult
            Dim con As HttpContext = HttpContext.Current
            If con.IsWebSocketRequest Or con.IsWebSocketRequestUpgrading Then
                con.AcceptWebSocketRequest(New WebSocketEventHandler())
            End If
            Return Request.CreateResponse(HttpStatusCode.SwitchingProtocols)
        End Function

        Function Index() As HttpResponseMessage
            Dim con As HttpContext = HttpContext.Current
            If con.IsWebSocketRequest Or con.IsWebSocketRequestUpgrading Then
                con.AcceptWebSocketRequest(New WebSocketEventHandler())
            End If
            Return Request.CreateResponse(HttpStatusCode.SwitchingProtocols)

        End Function
    End Class
End Namespace