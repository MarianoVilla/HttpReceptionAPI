Imports Microsoft.Web.WebSockets

Public Class WebSocketsServerSample
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