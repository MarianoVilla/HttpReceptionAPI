Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Reflection
Imports System.Web.Http
Imports Newtonsoft.Json

Namespace Controllers
    ''' <summary>
    ''' Standard API controller.
    ''' Route: http:TheAPI:ThePort/api/HttpAccess/PostMultipart
    ''' </summary>
    Public Class HttpAccessController
        Inherits ApiController


        Public Function GetValues() As String
            Return "This is the HTTP interface of the Web API. To use a socket connection, you should use WS as the protocol (from a browser), add an Upgrade-connection header to the request."
        End Function

        ''' <summary>
        ''' In the same manner as in PostMultipart, you could route to api/HttpAccess/PostMultipart and handle strings.
        ''' </summary>
        Public Sub PostString(<FromBody()> ByVal value As String)

        End Sub

        ''' <summary>
        ''' In the same manner as in PostMultipart, you could route to api/HttpAccess/PostMultipart and handle bytes.
        ''' </summary>
        Public Sub PostImage(<FromBody()> ByVal image As Byte())


        End Sub

        'http://IP:Port/api/HttpAccess/PostMultipart
        Public Async Function PostMultipart() As Threading.Tasks.Task(Of HttpResponseMessage)

            If Not Request.Content.IsMimeMultipartContent("form-data") Then
                Return Request.CreateResponse(HttpStatusCode.BadRequest)
            End If

            Dim ctx As HttpContext = HttpContext.Current
            Dim root As String = ctx.Server.MapPath("~/App_Data")
            Dim provider As MultipartFormDataStreamProvider = New MultipartFormDataStreamProvider(root)
            Await Request.Content.ReadAsMultipartAsync(provider)

            Try

                Dim DataHandler As HttpDefaultDataHandler = ProcessMultipart(provider, root)

                'At this point, you have an HttpDefaultDataHandler (see HttpDefaultDataHandler) with all the items in this request.
                'To handle the data, you would save or process the data, like so:
                'DataHandler.SaveItems()
                'DataHandler.ProcessItems()
                'This methods should be implemented in the most common way.


            Catch ex As Exception

                Return Request.CreateResponse(HttpStatusCode.InternalServerError)

            End Try
            Return Request.CreateResponse(HttpStatusCode.OK)


        End Function

        Private Function ProcessMultipart(Provider As MultipartFormDataStreamProvider, root As String) As HttpDefaultDataHandler

            'By default, the controller is using an HttpDefaultDataHandler. Still, if you want to make it more dynamic,
            'you could add the kind of handler as a parameter in the Web.config. That way, you could implement different handlers
            'and make the API use them dynamically, by changing the config.
            Dim DataHandler = New HttpDefaultDataHandler()
            DataHandler.Request = Request
            ProcessKeys(Provider, DataHandler)
            ProcessFiles(Provider, DataHandler, root)

            Return DataHandler

        End Function

        Private Sub ProcessFiles(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler, root As String)
            For Each file In Provider.FileData
                Dim formItem = New FormItem()
                Dim Name = file.Headers.ContentDisposition.FileName?.Trim("""")
                Name = Path.GetFileNameWithoutExtension(Name) + "_" + Guid.NewGuid.ToString() + Path.GetExtension(Name)
                Dim LocalFileName = file.LocalFileName
                Dim FilePath = Path.Combine(root, Name)
                formItem.Data = IO.File.ReadAllBytes(file.LocalFileName)
                formItem.MediaType = GetMediaType(file.Headers)
                formItem.FileName = Name
                formItem.Name = file.Headers.ContentDisposition.FileName
                IO.File.Move(LocalFileName, FilePath)
                DataHandler.FormItems.Add(formItem)
            Next

        End Sub

        Private Sub ProcessKeys(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler)
            Dim i = 0
            For Each key In Provider.FormData.AllKeys
                Dim formItem = New FormItem()
                For Each value In Provider.FormData.GetValues(key)
                    Dim Element = Provider.Contents.ElementAt(i)
                    formItem.Name = Element.Headers.ContentDisposition.Name?.Trim("""")
                    formItem.Raw = value
                    Dim KnownJsonObject As String = FormItem.SupportedJsonObjects.FirstOrDefault(Function(o) o.Key = key).Value
                    If (KnownJsonObject IsNot Nothing) Then
                        Dim TheTypeOfTheJSON As Type = Type.GetType(KnownJsonObject)
                        formItem.JsonObject = JsonConvert.DeserializeObject(value, TheTypeOfTheJSON)
                    End If
                Next
                DataHandler.FormItems.Add(formItem)
            Next
        End Sub

        Private Function GetMediaType(Headers As HttpContentHeaders)
            Return If(Headers.ContentType Is Nothing, "", If(String.IsNullOrEmpty(Headers.ContentType.MediaType), "", Headers.ContentType.MediaType))
        End Function
    End Class
End Namespace