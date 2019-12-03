Imports System.Web
Imports System.Web.Services
Imports Microsoft.Web.WebSockets

''' <summary>
''' WebSocketsServer. In case you need to receive data though sockets, here's a standard implementation.
''' This handles the requests coming through WS protocol (ws://TheIP:ThePort/WebSocketsServer/WebSocketServer.ashx).
''' </summary>
Public Class WebSocketsServer
    Implements System.Web.IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

        If (context.IsWebSocketRequest) Then
            context.AcceptWebSocketRequest(New WebSocketEventHandler())
            Return
        End If

    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class