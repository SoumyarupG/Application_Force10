using System;
using CENTRUMCA;
//using NEREFSHCL;

namespace CENTRUMCF.Public
{
    public partial class PageAccess : CENTRUMBAse 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageHeading = Request.QueryString["mnuTxt"];
            //this.ShowPageHeading = true;
            //this.ShowMenu = true;
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString();
            lblCap.Text = "You Does Not Have Right To Access :: " + Request.QueryString["mnuTxt"] + " :: ";
            //this.EnableMenu = true;  
        }
    }
}
