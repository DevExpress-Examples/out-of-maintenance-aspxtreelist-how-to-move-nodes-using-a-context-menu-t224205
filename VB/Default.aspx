<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Dashboard.v14.2.Web, Version=14.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.DashboardWeb" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v14.2, Version=14.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v14.2, Version=14.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPxTreeList - How to move nodes using a context menu</title>
    <style>
        .cutRow {
            background-color:  lightblue;
        }
    </style>
    <script type="text/javascript" src="https://code.jquery.com/jquery-1.9.1.min.js"></script>
    <script>
        function OnCallbackError(s, e) {
           // if (e.message == "You're trying to move a parent node to its child") - uncomment this line to reset styles only in certain cases
                myCallback = false;

        }
        var myCallback = false;
        function OnBeginCallback(s, e) {
            if (e.command == "CustomCallback") {
                myCallback = true;
            }

        }
        var copyNodeKey = null;
        var currentNodeKey = null;
        function OnContextMenu(s, e) {
            if (e.objectType != 'Node') return;
            s.SetFocusedNodeKey(e.objectKey);
            var mouseX = ASPxClientUtils.GetEventX(e.htmlEvent);
            var mouseY = ASPxClientUtils.GetEventY(e.htmlEvent);
            ShowMenu(e.objectKey, mouseX, mouseY);
        }
        var currentNodeKey = -1;
        function ShowMenu(nodeKey, x, y) {
            clientPopupMenu.ShowAtPos(x, y);
            currentNodeKey = nodeKey;
            var menu = ASPxClientPopupMenu.Cast(clientPopupMenu);
            menu.GetItemByName("paste").SetEnabled(copyNodeKey != null);

        }
        function OnEndCallback(s, e) {
            if (myCallback) {
                ResetValues();
            }
        }
        function ResetValues() {                       
            currentNodeKey = null; copyNodeKey = null;
        }
        function ProcessNodeClick(itemName) {
            switch (itemName) {
                case "cut":
                    {
                        if (copyNodeKey != currentNodeKey) {
                            $("tr[rowKey='" + copyNodeKey + "']").removeClass("cutRow");
                            $("tr[rowKey='" + currentNodeKey + "']").addClass("cutRow");
                            copyNodeKey = currentNodeKey;
                        }
                        break;
                    }
                case "paste":
                    {
                        if (copyNodeKey == null) {
                            alert("There is nothing to paste");
                            return;
                        }
                        var parameter = currentNodeKey + ";" + copyNodeKey;
                        clientTreeList.PerformCallback(parameter);

                        break;
                    }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
         <dx:ASPxTreeList runat="server" ID="tree" Width="500px" OnCustomErrorText="tree_CustomErrorText" OnHtmlRowPrepared="tree_HtmlRowPrepared" ClientInstanceName="clientTreeList" DataSourceID="source" ParentFieldName="ReportsTo" KeyFieldName="EmployeeID" OnCustomCallback="tree_CustomCallback" AutoGenerateColumns="False">
            <Columns>
                <dx:TreeListDataColumn FieldName="EmployeeID" Width="30"  ReadOnly="True" VisibleIndex="0">
                    <CellStyle HorizontalAlign="Left"></CellStyle>
                </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="LastName" Width="200"  ReadOnly="True" VisibleIndex="1">
                </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="FirstName" Width="200" ReadOnly="True" VisibleIndex="2">
                </dx:TreeListDataColumn>
                <dx:TreeListDataColumn FieldName="ReportsTo" Width="70" ReadOnly="True" VisibleIndex="3">
                </dx:TreeListDataColumn>
            </Columns>
            <ClientSideEvents BeginCallback="OnBeginCallback" CallbackError="OnCallbackError" EndCallback="OnEndCallback" ContextMenu="function(s, e) {
            OnContextMenu(s,e);
         }" />
        </dx:ASPxTreeList>
        <dx:ASPxPopupMenu ID="ASPxPopupMenu1" runat="server" ClientInstanceName="clientPopupMenu">
            <Items>
                <dx:MenuItem Name="cut" Text="Cut">
                </dx:MenuItem>
                <dx:MenuItem Name="paste" Text="Paste">
                </dx:MenuItem>
            </Items>
            <ClientSideEvents ItemClick="function(s, e) { ProcessNodeClick (e.item.name);}" />
        </dx:ASPxPopupMenu>

        <asp:LinqDataSource runat="server" ID="source" ContextTypeName="NorthwindDataClassesDataContext" EntityTypeName="" Select="new (EmployeeID, LastName, FirstName, ReportsTo)" TableName="Employees"></asp:LinqDataSource>
    </form>
</body>
</html>
