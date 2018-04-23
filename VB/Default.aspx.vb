Imports DevExpress.Web.ASPxTreeList
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub tree_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs)
        If String.IsNullOrWhiteSpace(e.Argument) Then
            Return
        End If
        Dim param = e.Argument.Split(";"c)
        If param.Length < 2 Then
            Return
        End If

        Dim context_Renamed = New NorthwindDataClassesDataContext()
        Dim currentNode = context_Renamed.Employees.Where(Function(empl) empl.EmployeeID = Convert.ToInt32(param(0))).FirstOrDefault()
        Dim copyNode = context_Renamed.Employees.Where(Function(empl) empl.EmployeeID = Convert.ToInt32(param(1))).FirstOrDefault()
        If ProcessNodes(tree.FindNodeByKeyValue(copyNode.EmployeeID.ToString()), currentNode.EmployeeID.ToString()) Then
            copyNode.ReportsTo = currentNode.EmployeeID
            'coment the below line to enable updates
            Throw New CallbackException("Updates aren't allowed in online demo")
            context_Renamed.SubmitChanges()
            tree.DataBind()
        Else
            Throw New CallbackException("You're trying to move a parent node to its child")
        End If
    End Sub


    Private Function ProcessNodes(ByVal startNode As TreeListNode, ByVal keyToCheck As String) As Boolean
        Dim result = True
        If startNode Is Nothing Then
            Return False
        End If
        Dim [iterator] As New TreeListNodeIterator(startNode)

        Do While [iterator].Current IsNot Nothing
            If Not CheckCurrentNodeKey([iterator].Current,keyToCheck) Then
                [iterator].GetNext()
            Else
                result = False
                Exit Do
            End If
        Loop
        Return result
    End Function

    Private Function CheckCurrentNodeKey(ByVal treeListNode As TreeListNode, ByVal keyToCheck As String) As Boolean
        Dim result As Boolean = treeListNode.Key = keyToCheck
        Return result


    End Function
    Protected Sub tree_HtmlRowPrepared(ByVal sender As Object, ByVal e As TreeListHtmlRowEventArgs)
        If e.RowKind = TreeListRowKind.Data Then
            e.Row.CssClass &= " customRow"
            e.Row.Attributes.Add("rowKey", e.NodeKey)
        End If
    End Sub
    Protected Sub tree_CustomErrorText(ByVal sender As Object, ByVal e As TreeListCustomErrorTextEventArgs)
        If TypeOf e.Exception Is CallbackException Then
            e.ErrorText = e.Exception.Message
        End If
    End Sub
End Class