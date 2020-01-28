using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ManageLiveSessionWeb.utility;

namespace ManageLiveSessionWeb
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                string firstname = username.Text;
                string password = password1.Text;

                //begin temp code mgouda  ohasan zhassan    hahassan mkhalil  maabdalla مشاريع , hashem@kfu.edu.sa   mkhalil
                if (firstname == "hahassan" && password == "123456@123")
                {

                    Utilities.IsAuthenticated = true;
                    Utilities.UserName = firstname;
                    Utilities.Email = firstname;
                    Utilities.FullName = firstname;

                    string path = VirtualPathUtility.ToAbsolute("~/models/live_doctors/DoctorsLiveSessions.aspx");
                    Response.Redirect(path);
                    return;
                }
                //end temp code

                PrincipalContext pContxt = new PrincipalContext(ContextType.Domain, "192.168.5.50", "OU=User,DC=EKFU,DC=LOCAL", firstname, password);
                bool checkUser = pContxt.ValidateCredentials(firstname, password);
                if (checkUser == false)
                {
                    pContxt = new PrincipalContext(ContextType.Domain, "192.168.5.51", "OU=User,DC=EKFU,DC=LOCAL", firstname, password);
                    checkUser = pContxt.ValidateCredentials(firstname, password);
                }
                if (checkUser == false)
                {
                    pContxt = new PrincipalContext(ContextType.Domain, "192.168.93.52", "OU=User,DC=EKFU,DC=LOCAL", firstname, password);
                    checkUser = pContxt.ValidateCredentials(firstname, password);
                }
                if (checkUser == false)
                {

                    Response.Redirect("login.aspx");
                }
                else
                {
                    PrincipalSearcher se = new PrincipalSearcher();
                    UserPrincipal pc = UserPrincipal.FindByIdentity(pContxt, IdentityType.SamAccountName, firstname);
                    DirectoryEntry de = (DirectoryEntry)pc.GetUnderlyingObject();
                    String userType = de.Properties["employeeType"].Value.ToString();

                    if (userType == "staff" || userType == "faculty")
                    {

                        System.DirectoryServices.AccountManagement.UserPrincipal up = ((System.DirectoryServices.AccountManagement.UserPrincipal)pc);


                        Utilities.IsAuthenticated = true;
                        Utilities.UserName = firstname;
                        Utilities.Email = firstname;
                        Utilities.FullName = firstname;
                        string path = VirtualPathUtility.ToAbsolute("~/models/live_doctors/DoctorsLiveSessions.aspx");
                        Response.Redirect(path);
                    }
                    else
                    {
                        Utilities.IsAuthenticated = false;
                        Utilities.UserName = null;
                      // todo error message not authenticated
                    }
                }

            }
        }
    }
}