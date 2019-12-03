Imports System.Timers
Imports System.Web.Script.Serialization
Imports Microsoft.Web.WebSockets

''' <summary>
''' WebSocketServer used to handle the testing done in Index.vbhtml.
''' </summary>
Public Class WebSocketEventHandler
    Inherits WebSocketHandler

    ''' <summary>
    ''' The socket is prepared to handle multiple clients as state. This is generally used to keep clients in sync.
    ''' </summary>
    Dim clients As WebSocketCollection = New WebSocketCollection()
    Dim theTimer As System.Timers.Timer
    Dim Serializer As JavaScriptSerializer = New JavaScriptSerializer()

    Public Overrides Sub OnOpen()
        clients.Add(Me)

        Send(Serializer.Serialize("Server says: Connected! Waiting for input..."))

        SetTimer()

    End Sub

#Region "Demo boilerplate."
    Sub SetTimer()

        theTimer = New System.Timers.Timer(2000)

        AddHandler theTimer.Elapsed, AddressOf TimerEvent
        theTimer.AutoReset = True
        theTimer.Enabled = True
    End Sub

    Private Sub TimerEvent(sender As Object, e As ElapsedEventArgs)
        clients.Broadcast(Serializer.Serialize("Server says: It is " + DateTime.Now + " and I'm waiting for a message."))
    End Sub
#End Region

    ''' <summary>
    ''' Here you would handle a byte[] input. TODO: something with the data.
    ''' </summary>
    ''' <param name="message"></param>
    Public Overrides Sub OnMessage(message As String)
        clients.Broadcast(Serializer.Serialize(Serializer.Serialize("Server says: Received: " + message)))
    End Sub


    ''' <summary>
    ''' Here you would handle a string input. TODO: something with the data.
    ''' </summary>
    ''' <param name="message"></param>
    Public Overrides Sub OnMessage(message() As Byte)
        clients.Broadcast(Serializer.Serialize("Server says: Received byte array."))
    End Sub

    Public Overrides Sub OnClose()
        clients.Remove(Me)
        clients.Broadcast("A client has desconected.")

        theTimer.Stop()
        theTimer.Dispose()
        MyBase.OnClose()
    End Sub


End Class
