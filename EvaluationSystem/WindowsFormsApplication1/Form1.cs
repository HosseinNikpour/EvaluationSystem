using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SPSite site = new SPSite("http://net-sp");
            SPWeb web = site.OpenWeb();
            SPList list = web.GetList("/Lists/WeeklyPlanConstructions");
            SPContentType ct = list.ContentTypes[0];
            ct.DisplayFormUrl = "/_Layouts/15/ProjectInfoSystem/Pages/Plan/DisplayForm.aspx";
            ct.EditFormUrl = "/_Layouts/15/ProjectInfoSystem/Pages/Plan/EditForm.aspx";
            ct.NewFormUrl = "/_Layouts/15/ProjectInfoSystem/Pages/Plan/NewForm.aspx";
            ct.Update();
            list.Update();
        }
    }
}
