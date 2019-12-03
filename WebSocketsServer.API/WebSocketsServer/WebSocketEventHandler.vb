Imports System.Web.Script.Serialization
Imports Microsoft.Web.WebSockets

Public Class WebSocketEventHandler
    Inherits WebSocketHandler

    Public Overrides Sub OnOpen()


        Dim Serializer As JavaScriptSerializer = New JavaScriptSerializer()
        Send(Serializer.Serialize("Connected!"))
        While (True)
            MyBase.Send(Serializer.Serialize("Time is: " + DateTime.Now))
            Threading.Thread.Sleep(1000)
        End While

    End Sub

    Public Overrides Sub OnMessage(message As String)
        MyBase.OnMessage(message)
    End Sub

    Public Overrides Sub OnClose()
        MyBase.OnClose()
    End Sub

End Class
