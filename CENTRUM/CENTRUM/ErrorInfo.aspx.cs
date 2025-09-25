using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace CENTRUM
{
    public partial class ErrorInfo : CENTRUMBase
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
            Response.Redirect("~/Login.aspx", false);
        }
    }
}