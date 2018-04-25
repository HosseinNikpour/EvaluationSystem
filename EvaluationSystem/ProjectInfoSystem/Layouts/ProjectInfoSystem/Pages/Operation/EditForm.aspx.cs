using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace ProjectInfoSystem.Layouts.ProjectInfoSystem.Pages.Operation
{
    public partial class EditForm : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            if (!base.IsPostBack)
            {
                SPUser currentUser = web.CurrentUser;
                string g = base.Request.QueryString["List"];
                int id = int.Parse(base.Request.QueryString["ID"]);
                SPList list = web.Lists[new Guid(g)];
                if (!list.GetItemById(id).DoesUserHavePermissions(currentUser, SPBasePermissions.EditListItems))
                {
                    string defaultViewUrl = list.DefaultViewUrl;
                    base.ClientScript.RegisterStartupScript(base.GetType(), "callfunction", "alert('شما دسترسی لازم برای ویرایش این فرم را ندارید');window.location.href = '" + defaultViewUrl + "';", true);
                }
            }
        }
    }
}
