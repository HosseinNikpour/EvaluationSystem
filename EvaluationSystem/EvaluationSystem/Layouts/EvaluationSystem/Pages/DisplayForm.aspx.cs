using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace EvaluationSystem.Layouts.EvaluationSystem.Pages
{
    public partial class DisplayForm : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPWeb web = SPContext.Current.Web;
            if (!base.IsPostBack)
            {
                string str2 = base.Request.QueryString["ListName"];
                int num = int.Parse(base.Request.QueryString["ID"]);
                SPList list = web.GetList("/Lists/" + str2);
                this.lit1.Text = "<script>listFaName='" + list.Title + "'</script>";
            }
        }
    }
}
