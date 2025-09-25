using System;
using System.Web.Security;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Admin
{
    public partial class SsnExpr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //CDayEnd oDE = new CDayEnd();
            //oDE.UpdateUserBranch("", Convert.ToInt32(Session[gblValue.UserId].ToString()), "O");
            //oDE = null;

            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
        }
    }
}