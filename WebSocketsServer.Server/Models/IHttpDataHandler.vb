Imports System.Net.Http
Imports System.Threading.Tasks
''' <summary>
''' The IHttpDataHandler interface is supposed to abstract some standard operations one might one to do with the data.
''' Since there's no specific operation to do with the data, we can use this interface to polymorphically handle it.
''' For instance, we could have a "HttpSQLDataHandler" that implements SaveItems() to insert stuff in a DB.
''' The <see cref="HttpDefaultDataHandler"/> class implements it as an example.
''' </summary>
Friend Interface IHttpDataHandler

    Property FormItems As List(Of FormItem)
    Property Request As HttpRequestMessage

    Sub SaveItems()
    Function SaveItemsAsync() As Task
    Sub ProcessItems()
    Function ProcessItemsAsync() As Task

End Interface
