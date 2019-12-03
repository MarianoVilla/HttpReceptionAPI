''' <summary>
''' FormItem is a generic data handler. It's intentionally unnormalized in order to support different kinds of items without
''' needing to add preemptive abstractions.
''' </summary>

Public Class FormItem

    Public Property Raw As String
    Public Property Name As String
    Public Property Data As Byte()
    Public Property FileName As String
    Public Property MediaType As String
    Public Property JsonObject

    ''' <summary>
    ''' A dictionary of the supported JSON objects. We would use this list to see if what we got from the POST request contains a known object
    ''' If so, we can deserialize it with the corresponding class from JSONModels.
    '''For the moment, we're assuming the expected objects won't change too often.
    '''It's worth notting that we could generate static entities when reciving unexpected JSON objects, adding them to this list.
    '''This kind of solution implies emmitting code at runtime and adds a lot of complexitiy, so it's better to avoid.
    ''' </summary>
    ''' Each key-value has to hold the JSON name and the Assembly Name of the class we'll use to deserialize.
    ''' This is made to avoid coupling JSON objects names to our static classes.
    Public Shared ReadOnly SupportedJsonObjects As Dictionary(Of String, String) = New Dictionary(Of String, String) From
        {
         {"boundary", "WebSocketsServer.Server.Boundary"},
         {"faceCapture", "WebSocketsServer.Server.FaceCaptureParent"}
        }

    Public ReadOnly Property Value As String
        Get
            Return Encoding.[Default].GetString(Data)
        End Get
    End Property

    Public ReadOnly Property IsAFile As Boolean
        Get
            Return Not String.IsNullOrEmpty(FileName)
        End Get
    End Property
End Class
