Imports System.Net.Http
''' <summary>
''' The default implementation of a handler for the received data.
''' </summary>
Public Class HttpDefaultDataHandler
    Implements IHttpDataHandler

    ''' <summary>
    ''' A List of <see cref="FormItem"/>. This list would have everything that came in a given POST.
    ''' </summary>
    ''' <returns></returns>
    Public Property FormItems As List(Of FormItem) = New List(Of FormItem)

    Public Property Request As HttpRequestMessage


    Public Sub SaveItems() Implements IHttpDataHandler.SaveItems

        'Here you can do something like this:
        For Each formItem In FormItems
            'You can give files a special treatment (Like saving then in the file system)
            If formItem.IsAFile Then
                'Save it.
            Else
                'Do something else.
            End If
        Next
        Throw New NotImplementedException()
    End Sub

    Public Sub ProcessItems() Implements IHttpDataHandler.ProcessItems

        'You might want to do something like sending the FormItems from this request to another service or so; here's where you would put that code.
        Throw New NotImplementedException()

    End Sub

#Disable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously
    Public Async Sub ProcessItemsAsync() Implements IHttpDataHandler.ProcessItemsAsync

        'Await some work.

    End Sub

    Public Async Sub SaveItemsAsync() Implements IHttpDataHandler.SaveItemsAsync

        'Await some saving work.

    End Sub

#Enable Warning BC42356 ' This async method lacks 'Await' operators and so will run synchronously


End Class
