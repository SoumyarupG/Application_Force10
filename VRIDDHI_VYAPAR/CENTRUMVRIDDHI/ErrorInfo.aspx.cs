using System;
using System.Web.Security;

namespace CENTRUM_VRIDDHIVYAPAR
{
    public partial class ErrorInfo : CENTRUMBAse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Menu = false;
            lblErr.Text = Convert.ToString(Session["ErrMsg"]);
            Server.ClearError();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnOk_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Response.RedirectPermanent("~/Login.aspx", false);
        }
    }
}