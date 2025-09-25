using System;
using System.Web.Security;

namespace CENTRUMCF.WebPages.Public
{
    public partial class SsnExpr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
        }
    }
}