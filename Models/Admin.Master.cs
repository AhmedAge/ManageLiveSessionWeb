using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ManageLiveSessionWeb.Models
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Utility.IsAuthenticated)
            //{ 
            //    username.Text = Utility.FullName;
            //}
            //else
            //{
            //    Response.Redirect("../login.aspx");
            //}
        }
    }
}