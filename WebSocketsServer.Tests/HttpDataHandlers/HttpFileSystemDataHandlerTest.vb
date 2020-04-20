Imports System.IO
Imports NUnit.Framework
Imports WebSocketsServer.Server

Public Class HttpFileSystemDataHandlerTest

    Private FSDataHandler As HttpFileSystemDataHandler
    <SetUp>
    Public Sub SetUp()
        Directory.CreateDirectory(TestRepo.FilesPath)
        FSDataHandler = New HttpFileSystemDataHandler With
            {.FilesPath = TestRepo.FilesPath,
            .FormItems = TestRepo.InMemoryFormItems,
            .Request = TestRepo.InMemoryRequest}

    End Sub
    <TearDown>
    Public Sub TearDown()
        Directory.EnumerateFiles(TestRepo.FilesPath).ToList().ForEach(Sub(x) File.Delete(x))
    End Sub


    <Test>
    Public Sub TestSaveItems()

        Assert.True(Directory.EnumerateFiles(TestRepo.FilesPath).Count() = 0)
        'FSDataHandler.SaveItems()
        'Assert.True(Directory.EnumerateFiles(TestRepo.FilesPath).Count() = 1)

    End Sub



End Class
