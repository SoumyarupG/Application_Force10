using System;
using CENTRUMCA;
using System.Web.UI;

namespace CENTRUMCF.Public
{
    public partial class Main : CENTRUMBAse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToString(Session[gblValue.BrnchCode]).Trim() == "" || Session[gblValue.BrnchCode] == null || Session[gblValue.LoginDate] == null
                    || Convert.ToString(Session[gblValue.LoginDate]).Trim() == "" || Session[gblValue.UserId] == null || Convert.ToString(Session[gblValue.UserId]).Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                this.ShowBranchName = Convert.ToString(Session[gblValue.BrnchCode]) + " - " + Convert.ToString(Session[gblValue.BrName]);
                this.ShowFinYear = Convert.ToString(Session[gblValue.FinYear]);

            }
        }
    }
}