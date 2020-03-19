Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

''' <summary>
''' Summary description for CallbackException
''' </summary>
Public Class CallbackException
	Inherits Exception

'INSTANT VB NOTE: The variable message was renamed since Visual Basic does not handle local variables named the same as class members well:
	Public Sub New(ByVal message_Conflict As String)
		MyBase.New(message_Conflict)
	End Sub
End Class