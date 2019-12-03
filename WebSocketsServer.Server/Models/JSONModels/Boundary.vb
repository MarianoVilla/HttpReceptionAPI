''' <summary>
''' 'VB implementation of the JSON object "boundary" given in the examples.
''' </summary>
Public Class Boundary
    Public Property ipAddress As String
    Public Property portNo As Integer
    Public Property protocol As String
    Public Property macAddress As String
    Public Property channelID As Integer
    Public Property dateTime As DateTime
    Public Property activePostCount As Integer
    Public Property eventType As String
    Public Property eventState As String
    Public Property eventDescription As String
    Public Property deviceID As Integer
    Public Property RegionCapture As RegionCapture
    Public Property contentID As String
End Class

Public Class HumanCounting
        Public Property count As Integer
    End Class

    Public Class Region
        Public Property x As Double
        Public Property y As Double
    End Class

    Public Class Rule
        Public Property ruleID As Integer
        Public Property alarmCount As Integer
        Public Property regionColor As String
        Public Property Region As Region()
        Public Property countTriggerType As String
    End Class

    Public Class RegionCapture
        Public Property humanCounting As HumanCounting
        Public Property rule As Rule
    End Class

