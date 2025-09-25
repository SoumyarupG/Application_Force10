using System;
using CENTRUMCA;

namespace CENTRUM_VRIDDHIVYAPAR.Public
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
                if (Session[gblValue.BrnchCode] == null || Session[gblValue.LoginDate] == null || Session[gblValue.UserId] == null)
                {
                    Response.Redirect("~/login.aspx", false);
                }
                else if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == "")
                {
                    Response.Redirect("~/login.aspx", false);
                }
                else
                {
                    this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                    this.ShowFinYear = Session[gblValue.FinYear].ToString();
                }
            }
        }
    }
}