Imports Newtonsoft.Json.Linq
Imports NUnit.Framework
Imports WebSocketsServer.Server

Public Class JsonHandlingTest

    <Test>
    Public Sub TestGetItemsFromJson_ShouldWork()

        Dim RawJson = TestRepo.InMemoryKnownJSON

        Dim Json = JObject.Parse(RawJson)

        Dim IpAddress = Json.SelectToken("ipAddress")
        Dim FaceCapture = Json.SelectToken("faceCapture")
        Dim FirstFace = Json.SelectToken("faceCapture[0].faces[0]")
        Dim FirstFaceCurrTemp = Json.SelectToken("faceCapture[0].faces[0].currTemperature")

        'If you need a first level prop, you can make it even simpler:
        Dim IpAddressAlternative = Json("ipAddress")


        AssertAreNotNull(IpAddress, FaceCapture, FirstFace, FirstFaceCurrTemp, IpAddressAlternative)

        Assert.AreEqual(IpAddress.ToString(), "192.168.23.195")
        Assert.AreEqual(IpAddressAlternative.ToString(), "192.168.23.195")
        Assert.AreEqual(FirstFaceCurrTemp.ToString(), "36.8")

    End Sub

    <Test>
    Public Sub TestGetItemsFromJson_ShouldFail()

        Dim RawJson = TestRepo.InMemoryKnownJSON

        Dim Json = JObject.Parse(RawJson)

        Dim NonExistent = Json.SelectToken("someNonExistentNode")

        Assert.IsNull(NonExistent)

    End Sub

End Class
