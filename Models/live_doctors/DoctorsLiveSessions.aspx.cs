using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ManageLiveSessionWeb.utility;

namespace ManageLiveSessionWeb.Models.live_doctors
{
    public partial class DoctorsLiveSessions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Utilities.IsAuthenticated == false)
            {
                string path = VirtualPathUtility.ToAbsolute("~/login.aspx");
                Response.Redirect(path);
            }

            //if (!IsPostBack)
            //{


            //}
        }
    }
}