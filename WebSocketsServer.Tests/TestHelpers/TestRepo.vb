Imports System.Net.Http
Imports WebSocketsServer.Server

Public Class TestRepo

    Public Shared ReadOnly FilesPath = AppDomain.CurrentDomain.BaseDirectory + "\TestFilesRepo"
    Public Shared ReadOnly InMemoryFormItems = New List(Of FormItem) From
    {New FormItem With {.FileName = "SomeFileName", .Raw = InMemoryKnownJSON}}

    Public Shared ReadOnly InMemoryRequest = New HttpRequestMessage() With
        {
            .Method = New HttpMethod("POST"),
            .RequestUri = New Uri("http://localhost:56412/api/HttpAccess/PostMultipart")
        }

    Public Shared ReadOnly InMemoryKnownJSON = "{
	""ipAddress"":	""192.168.23.195"",
	""protocol"":	""HTTP"",
	""macAddress"":	""64:db:8b:7f:1f:7a"",
	""channelID"":	1,
	""dateTime"":	""2019-11-29T11:04:46+01:00"",
	""activePostCount"":	1,
	""eventState"":	""active"",
	""eventType"":	""faceCapture"",
	""eventDescription"":	""faceCapture"",
	""channelName"":	""Ingresso Showroom"",
	""faceCapture"":	[{
			""targetAttrs"":	{
				""deviceName"":	""IP CAMERA"",
				""deviceChannel"":	1,
				""deviceId"":	""c79e8000-84c2-11b2-8083-64db8b7f1f7a"",
				""faceTime"":	""2019-11-29T11:04:45+01:00"",
				""contentID"":	""backgroundImage"",
				""pId"":	""2019112911044610400xIj53ivE90Jd""
			},
			""faces"":	[{
					""faceId"":	55,
					""faceRect"":	{
						""x"":	0.383000,
						""y"":	0.400000,
						""width"":	0.170000,
						""height"":	0.596000
					},
					""contentID"":	""faceImage"",
					""pId"":	""2019112911044610600N50XF4v550Y3"",
					""faceScore"":	62,
					""stayDuration"":	0,
					""captureEndMark"":	false,
					""swingAngle"":	0,
					""tiltAngle"":	0,
					""pupilDistance"":	96,
					""livenessDetectionStatus"":	""notLiveFace"",
					""faceSnapThermometryEnabled"":	""true"",
					""currTemperature"":	36.8,
					""isAbnomalTemperature"":	""true"",
					""thermometryUnit"":	""celsius"",
					""alarmTemperature"":	36.6
				}]
		}],
	""uid"":	""2019112911044604700I0KP54zkoQA5661Mg003E5lRr7I2le0C7LYVFIKnFCON""
}"


End Class
