using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class CollectionDisbursement : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //InitBasePage();
            if (!IsPostBack)
            {

            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Collection Disbursement";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBankBookRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Disbursement", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnExit_Click1(object sender, EventArgs e)
        {
            try
            {
                Response.RedirectPermanent("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
