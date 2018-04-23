using DevExpress.Web.ASPxTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void tree_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
    {
        if (String.IsNullOrWhiteSpace(e.Argument))
            return;
        var param = e.Argument.Split(';');
        if (param.Length < 2)
            return;
        var context = new NorthwindDataClassesDataContext();
        var currentNode = context.Employees.Where(empl => empl.EmployeeID == Convert.ToInt32(param[0])).FirstOrDefault();         
        var copyNode = context.Employees.Where(empl => empl.EmployeeID == Convert.ToInt32(param[1])).FirstOrDefault();
        if (ProcessNodes(tree.FindNodeByKeyValue(copyNode.EmployeeID.ToString()), currentNode.EmployeeID.ToString()))
        {
            copyNode.ReportsTo = currentNode.EmployeeID;
            //coment the below line to enable updates
            throw new CallbackException("Updates aren't allowed in online demo");
            context.SubmitChanges();
            tree.DataBind();
        }
        else {
            throw new CallbackException("You're trying to move a parent node to its child");
        }
    }
   

    bool ProcessNodes(TreeListNode startNode, string keyToCheck)
    {
        var result = true;
        if (startNode == null) return false;
        TreeListNodeIterator iterator = new TreeListNodeIterator(startNode);
       
        while (iterator.Current != null)
        {
            if (!CheckCurrentNodeKey(iterator.Current,keyToCheck))
                iterator.GetNext();
            else
            {
                result = false;
                break;
            }
        }
        return result;
    }

    private bool CheckCurrentNodeKey(TreeListNode treeListNode,string keyToCheck)
    {
        bool result = treeListNode.Key == keyToCheck;
        return result;

     
    }
    protected void tree_HtmlRowPrepared(object sender, TreeListHtmlRowEventArgs e)
    {
        if (e.RowKind == TreeListRowKind.Data)
        {
            e.Row.CssClass += " customRow";
            e.Row.Attributes.Add("rowKey", e.NodeKey);
        }
    }
    protected void tree_CustomErrorText(object sender, TreeListCustomErrorTextEventArgs e)
    {
        if (e.Exception is CallbackException)
        {
            e.ErrorText = e.Exception.Message;
        }
    }
}