using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ManageLiveSessionWeb.utility;

namespace ManageLiveSessionWeb.Models
{
    public partial class Doctors : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utilities.IsAuthenticated)
            { 
                username.Text = Utilities.FullName;
            }
            else
            {
                Response.Redirect("../login.aspx");
            }
        }
    }
}