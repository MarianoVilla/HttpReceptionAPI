Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json

Public Class HttpAccessControllerHelper

    Public Shared Sub ProcessFiles(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler, root As String)

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

    Public Shared Sub ProcessKeys(Provider As MultipartFormDataStreamProvider, DataHandler As HttpDefaultDataHandler)
        Dim i = 0
        For Each key In Provider.FormData.AllKeys
            Dim formItem = New FormItem()
            For Each value In Provider.FormData.GetValues(key)
                formItem.Name = Provider.Contents.ElementAt(i).Headers.ContentDisposition.Name?.Trim("""")
                formItem.Raw = value
                Dim KnownJsonObject As String = FormItem.SupportedJsonObjects.FirstOrDefault(Function(o) o.Key = key).Value
                If (KnownJsonObject IsNot Nothing) Then
                    formItem.JsonObject = DeserializeFromTypeName(value, KnownJsonObject)
                End If
                i = i + 1
            Next
            DataHandler.FormItems.Add(formItem)
        Next
    End Sub

    Private Shared Function DeserializeFromTypeName(JsonValue As String, TypeName As String)
        Dim TheTypeOfTheJSON As Type = Type.GetType(TypeName)
        Return JsonConvert.DeserializeObject(JsonValue, TheTypeOfTheJSON)
    End Function

    Private Shared Function GetMediaType(Headers As HttpContentHeaders)
        Return If(Headers.ContentType Is Nothing, "", If(String.IsNullOrEmpty(Headers.ContentType.MediaType), "", Headers.ContentType.MediaType))
    End Function

End Class
