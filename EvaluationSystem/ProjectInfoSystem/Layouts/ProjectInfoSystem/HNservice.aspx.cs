using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace ProjectInfoSystem.Layouts.ProjectInfoSystem
{
    public partial class HNservice : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string data = "[{\"name\":\"Joe\"},{\"name\":\"Joe2\"}]";
            Response.Clear();
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(data);
            Response.End();
        }
    }
}
