using System;
using System.Web.Security;
using CENTRUMBA;
using CENTRUMCA;
using System.Web;

namespace SBT.WebPages.Private.Admin
{
    public partial class SsnExpr : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CUser oUsr = new CUser();
            oUsr.UpdateLogOutDt(Convert.ToInt32(Session[gblValue.LoginId]));
            oUsr = null;            
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            foreach (string cookieName in Request.Cookies.AllKeys)
            {
                HttpCookie cookie = new HttpCookie(cookieName);
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            Response.Cookies["LoginYN"].Expires = DateTime.Now.AddDays(-1); 
        }
    }
}