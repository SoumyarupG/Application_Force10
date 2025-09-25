using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
//using NEREFSHCL;

namespace CENTRUM.Public
{
    public partial class PageAccess : CENTRUMBase 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.PageHeading = Request.QueryString["mnuTxt"];
            //this.ShowPageHeading = true;           
            //this.ShowMenu = true; 
            //this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
            //this.ShowFinYear =  Session[gblValue.FinYear].ToString();
            //lblCap.Text = "You Does Not Have Right To Access :: " + Request.QueryString["mnuTxt"] + " :: ";
            //this.EnableMenu = true;  
        }
    }
}
