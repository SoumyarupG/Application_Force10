using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CENTRUM.WebPages.Public
{
    public partial class SsnExpr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            //Response.Cookies["LoginYN"].Expires = DateTime.Now.AddDays(-1); 
        }
    }
}