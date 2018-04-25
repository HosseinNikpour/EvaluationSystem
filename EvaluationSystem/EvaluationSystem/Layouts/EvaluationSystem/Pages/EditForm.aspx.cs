using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;

namespace EvaluationSystem.Layouts.EvaluationSystem.Pages
{
    public partial class EditForm : LayoutsPageBase
    {
       

        // Methods
        protected virtual void DisplayAlert(string message)
        {
            base.ClientScript.RegisterStartupScript(base.GetType(), Guid.NewGuid().ToString(), string.Format("alert('{0}');window.location.href = '{1}'", message.Replace("'", @"\'").Replace("\n", @"\n").Replace("\r", @"\r")), true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            if (!base.IsPostBack)
            {
                string str2 = base.Request.QueryString["ListName"];
                int id = int.Parse(base.Request.QueryString["ID"]);
                SPList list = web.GetList("/Lists/" + str2);
                SPListItem itemById = list.GetItemById(id);
                SPUser currentUser = web.CurrentUser;
                if (!itemById.DoesUserHavePermissions(web.CurrentUser, SPBasePermissions.EditListItems))
                {
                    string defaultViewUrl = list.DefaultViewUrl;
                    base.ClientScript.RegisterStartupScript(base.GetType(), "callfunction", "alert('شما دسترسی لازم برای ویرایش این فرم را ندارید');window.location.href = '" + defaultViewUrl + "';", true);
                }
                lit1.Text = "<script>listFaName='" + list.Title + "'</script>";
            }
        }
    }
}
