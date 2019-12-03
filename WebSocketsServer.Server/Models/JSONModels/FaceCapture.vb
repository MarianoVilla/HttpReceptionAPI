''' <summary>
''' 'VB implementation of the JSON object "faceCapture" given in the examples.
''' The actual JSON is called "faceCapture". Still, it has a "faceCapture" nested, 
''' so we had to make a parent so the compiler won't yell at us.
''' </summary>
Public Class FaceCaptureParent
    Public Property ipAddress As String
    Public Property protocol As String
    Public Property macAddress As String
    Public Property channelID As Integer
    Public Property dateTime As DateTime
    Public Property activePostCount As Integer
    Public Property eventState As String
    Public Property eventType As String
    Public Property eventDescription As String
    Public Property channelName As String
    Public Property faceCapture As FaceCapture()
    Public Property uid As String
End Class


Public Class TargetAttrs
        Public Property deviceName As String
        Public Property deviceChannel As Integer
        Public Property deviceId As String
        Public Property faceTime As DateTime
        Public Property contentID As String
        Public Property pId As String
    End Class

    Public Class FaceRect
        Public Property x As Double
        Public Property y As Double
        Public Property width As Double
        Public Property height As Double
    End Class

    Public Class Face
        Public Property faceId As Integer
        Public Property faceRect As FaceRect
        Public Property contentID As String
        Public Property pId As String
        Public Property faceScore As Integer
        Public Property stayDuration As Integer
        Public Property captureEndMark As Boolean
    End Class

    Public Class FaceCapture
        Public Property targetAttrs As TargetAttrs
        Public Property faces As Face()
    End Class

