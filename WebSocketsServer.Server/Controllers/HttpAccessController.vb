Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http
Imports Alpha.Utilidades.General
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports QLDB.Interface

Namespace Controllers
    ''' <summary>
    ''' Standard API controller.
    ''' Route: http:TheIP:ThePort/api/HttpAccess/PostMultipart
    ''' </summary>
    Public Class HttpAccessController
        Inherits ApiController


        Public Function GetValues() As String
            Return "This is the HTTP interface of the Web API."
        End Function

        ''' <summary>
        ''' In the same manner as in PostMultipart, you could route to api/HttpAccess/PostMultipart and handle strings.
        ''' </summary>
        Public Sub PostString(<FromBody()> ByVal value As String)

        End Sub

        ''' <summary>
        ''' In the same manner as in PostMultipart, you could route to api/HttpAccess/PostMultipart and handle bytes.
        ''' Route: api/HttpAccess/PostImage
        ''' </summary>

        Public Sub PostImage(<FromBody()> ByVal image As Byte())


        End Sub

        'Route: api/HttpAccess/PostMultipart 
        Public Async Function PostMultipart() As Threading.Tasks.Task(Of HttpResponseMessage)

            If Not Request.Content.IsMimeMultipartContent("form-data") Then
                LogsUtils.Loguear("Received a non-form-data request", TipoLogEnum.Debug)
                Return Request.CreateResponse(HttpStatusCode.BadRequest)
            End If

            Dim ctx As HttpContext = HttpContext.Current
            Dim root As String = ctx.Server.MapPath("~/App_Data")
            Dim provider As MultipartFormDataStreamProvider = New MultipartFormDataStreamProvider(root)
            Await Request.Content.ReadAsMultipartAsync(provider)

            Try



                Dim DataHandler As HttpDefaultDataHandler = ProcessMultipart(provider, root)

                Dim Json = JObject.Parse(DataHandler.FormItems.Where(Function(x) Not x.IsAFile).FirstOrDefault().Raw)

                Dim IPAddress = Json("ipAddress")

                Dim CurrTemperature = Json.SelectToken("faceCapture[0].faces[0].currTemperature")
                Dim Height = Json.SelectToken("faceCapture[0].faces[0].faceRect.height")


                Dim Base64String = Convert.ToBase64String(DataHandler.FormItems.Where(Function(x) x.IsAFile).FirstOrDefault().Data)






                Dim ApiKey = "vAyJVZf3zXV4qLoQc4vENFFuzkDIESH02uM9GMuqXMseQRa3o0"


                QldbClient.QldbApiUri = "https://3990aedd.ngrok.io/api/qldb"

                Dim ExampleInsert = New QldbRequestModel() With
                {
                .AccessKey = "AKIAVJZSOSR5VBHH7BX5",
                .SecretKey = "LZCospUIeajQcxli5lvChmkDXo4aDfR1qFgu9x7x",
                .LedgerName = "ProgrammaticLedger",
                .Statement = "INSERT INTO Person {'FirstName' : 'NewPerson', 'LastName' : 'NewPersonLastName', 'DOB' : `1963-08-19T`, 'GovId' : 'LEWISR261LL', 'GovIdType' : 'Driver License', 'Address' : '1719 University Street, Seattle, WA, 98109'}",
                .TableName = "Person",
                .ApiKey = ApiKey
                }

                Dim QldbResponse = QldbClient.QldbWrite(ExampleInsert)
                Debug.Write("Qldb insert response: " + QldbResponse)


                Dim ExampleReadRequest = New QldbRequestModel() With
                {
                    .AccessKey = "AKIAVJZSOSR5VBHH7BX5",
                    .SecretKey = "LZCospUIeajQcxli5lvChmkDXo4aDfR1qFgu9x7x",
                    .LedgerName = "ProgrammaticLedger",
                    .Statement = "SELECT * FROM Person;",
                    .TableName = "Person",
                    .ApiKey = ApiKey
                }

                QldbResponse = QldbClient.QldbRead(ExampleReadRequest)
                Debug.Write("Qldb read response: " + QldbResponse)


                Dim ExampleDelete = New QldbRequestModel() With
                {
                .AccessKey = "AKIAVJZSOSR5VBHH7BX5",
                .SecretKey = "LZCospUIeajQcxli5lvChmkDXo4aDfR1qFgu9x7x",
                .LedgerName = "ProgrammaticLedger",
                .Statement = "DELETE FROM Person;",
                .TableName = "Person",
                .ApiKey = ApiKey
                }

                QldbResponse = QldbClient.QldbWrite(ExampleDelete)
                Debug.Write("Qldb delete response: " + QldbResponse)













                'At this point, you have an HttpDefaultDataHandler (see HttpDefaultDataHandler) with all the items in this request.
                'To handle the data, you would save or process the data, like so:
                'DataHandler.SaveItems()
                'DataHandler.ProcessItems()
                'This methods should be implemented in the most common way.


            Catch ex As Exception
                LogsUtils.Loguear("An error ocurred while processing a request: " + ex.Message)
                Return Request.CreateResponse(HttpStatusCode.InternalServerError)
            End Try
            Return Request.CreateResponse(HttpStatusCode.OK)


        End Function

        Function QldbRead(Uri As String) As String


            Dim request As WebRequest
            request = WebRequest.Create("https://localhost:44304/api/qldb/read")
            Dim response As WebResponse
            Dim postData As String = "{""ledgerName"": ""ProgrammaticLedger"", ""accessKey"": ""AKIAVJZSOSR5VBHH7BX5"", ""secretKey"": ""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"", ""tableName"": null, ""statement"": ""Select * FROM Person;""}"
            Dim data As Byte() = Encoding.UTF8.GetBytes(postData)


            request.Method = "POST"
            request.ContentType = "application/json"
            request.ContentLength = data.Length

            Dim stream As Stream = request.GetRequestStream()
            stream.Write(data, 0, data.Length)
            stream.Close()

            response = request.GetResponse()
            Dim sr As New StreamReader(response.GetResponseStream())

            QldbRead = sr.ReadToEnd()

        End Function
        Private Function ProcessMultipart(Provider As MultipartFormDataStreamProvider, root As String) As HttpDefaultDataHandler

            'By default, the controller is using an HttpDefaultDataHandler. Still, if you want to make it more dynamic,
            'you could add the kind of handler as a parameter in the Web.config. That way, you could implement different handlers
            'and make the API use them dynamically, by changing the config.
            Dim DataHandler = New HttpDefaultDataHandler()

            DataHandler.Request = Request
            HttpAccessControllerHelper.ProcessKeys(Provider, DataHandler)
            HttpAccessControllerHelper.ProcessFiles(Provider, DataHandler, root)

            Return DataHandler

        End Function









        'Private Sub ProcessFiles(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler, root As String)

        '    For Each file In Provider.FileData
        '        Dim formItem = New FormItem()
        '        Dim Name = file.Headers.ContentDisposition.FileName?.Trim("""")
        '        Name = Path.GetFileNameWithoutExtension(Name) + "_" + Guid.NewGuid.ToString() + Path.GetExtension(Name)
        '        Dim LocalFileName = file.LocalFileName
        '        Dim FilePath = Path.Combine(root, Name)
        '        formItem.Data = IO.File.ReadAllBytes(file.LocalFileName)
        '        formItem.MediaType = GetMediaType(file.Headers)
        '        formItem.FileName = Name
        '        formItem.Name = file.Headers.ContentDisposition.FileName
        '        IO.File.Move(LocalFileName, FilePath)
        '        DataHandler.FormItems.Add(formItem)
        '    Next

        'End Sub

        'Private Sub ProcessKeys(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler)
        '    Dim i = 0
        '    For Each key In Provider.FormData.AllKeys
        '        Dim formItem = New FormItem()
        '        For Each value In Provider.FormData.GetValues(key)
        '            formItem.Name = Provider.Contents.ElementAt(i).Headers.ContentDisposition.Name?.Trim("""")
        '            formItem.Raw = value
        '            Dim KnownJsonObject As String = FormItem.SupportedJsonObjects.FirstOrDefault(Function(o) o.Key = key).Value
        '            If (KnownJsonObject IsNot Nothing) Then
        '                formItem.JsonObject = DeserializeFromTypeName(value, KnownJsonObject)
        '            End If
        '            i = i + 1
        '        Next
        '        DataHandler.FormItems.Add(formItem)
        '    Next
        'End Sub

        'Private Function DeserializeFromTypeName(JsonValue As String, TypeName As String)
        '    Dim TheTypeOfTheJSON As Type = Type.GetType(TypeName)
        '    Return JsonConvert.DeserializeObject(JsonValue, TheTypeOfTheJSON)
        'End Function

        'Private Function GetMediaType(Headers As HttpContentHeaders)
        '    Return If(Headers.ContentType Is Nothing, "", If(String.IsNullOrEmpty(Headers.ContentType.MediaType), "", Headers.ContentType.MediaType))
        'End Function

    End Class
End Namespace
