using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class EligibleAmountUpdate : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Eligible Amount Change";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEliAmtChange);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnUpdate.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnUpdate.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Eligible Amount Change", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;           
            Int32 vErr = 0;
            CRegion oReg = null;           
            try
            {
                oReg = new CRegion();
                vErr = oReg.EligibleAmountUpdate(txtEnquiryId.Text, Convert.ToDouble(txtAmount.Text), Convert.ToDouble(txtAmount24.Text));
                if (vErr > 0)
                {                  
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }
                return vResult;
            }
            finally
            {
                oReg = null;               
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (SaveRecords("Save") == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);               
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}