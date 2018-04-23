Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

''' <summary>
''' Summary description for CallbackException
''' </summary>
Public Class CallbackException
    Inherits Exception

    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class