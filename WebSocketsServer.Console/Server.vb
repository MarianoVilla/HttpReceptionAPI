Imports System.Net
Imports System.Net.Sockets

Public Class Server


    Public Property tcpListener() As TcpListener


    Sub Start()

        tcpListener = New TcpListener(IPAddress.Parse("127.0.0.1"), 80)

        tcpListener.Start()

    End Sub

End Class
