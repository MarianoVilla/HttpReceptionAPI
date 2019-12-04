Imports System.IO
Imports System.Net.Http
Imports WebSocketsServer.Server

Public Class HttpFileSystemDataHandler
    Implements IHttpDataHandler

    Public Property FormItems As List(Of FormItem) = New List(Of FormItem) Implements IHttpDataHandler.FormItems

    Public Property Request As HttpRequestMessage Implements IHttpDataHandler.Request

    Public Property FilesPath As String

    Public Sub SaveItems() Implements IHttpDataHandler.SaveItems
        For Each formItem In FormItems
            If formItem.IsAFile Then
                Try
                    Dim Stream = File.Create(FilesPath)
                    AddText(Stream, formItem.FileName)
                    AddText(Stream, formItem.JsonObject.ToString())
                    AddText(Stream, formItem.MediaType)
                    AddText(Stream, formItem.Raw)
                    Stream.Close()
                Catch ex As Exception

                End Try
            Else

            End If
        Next
    End Sub


    Private Shared Sub AddText(ByVal fs As FileStream, ByVal value As String)
        Dim info As Byte() = New UTF8Encoding(True).GetBytes(value)
        fs.Write(info, 0, info.Length)
    End Sub


    Public Async Function SaveItemsAsync() As Threading.Tasks.Task Implements IHttpDataHandler.SaveItemsAsync
        Throw New NotImplementedException()
    End Function

    Public Sub ProcessItems() Implements IHttpDataHandler.ProcessItems
        Throw New NotImplementedException()
    End Sub

    Public Async Function ProcessItemsAsync() As Threading.Tasks.Task Implements IHttpDataHandler.ProcessItemsAsync
        Throw New NotImplementedException()
    End Function
End Class
