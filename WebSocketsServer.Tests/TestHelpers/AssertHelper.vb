Imports System.Runtime.CompilerServices
Imports NUnit.Framework

Public Module AssertHelper

    Public Sub AssertAreNotNull(ParamArray Inputs() As Object)
        For Each Input As Object In Inputs
            Assert.IsNotNull(Input)
        Next
    End Sub

End Module
