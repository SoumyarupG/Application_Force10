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
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class EndDate : CENTRUMBase 
    {
        DateTime LstEndDt = System.DateTime.Now;    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {          
            InitBasePage();
            if (!IsPostBack)
            {
                txtEndDt.Text = Session[gblValue.LoginDate].ToString();
                LstEndDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {            
            try
            {
                this.Menu = false;
                this.PageHeading = "Day End Process";            
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";         
                this.GetModuleByRole(mnuID.mnuDayendProc);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx", false);  
        }
    }
}