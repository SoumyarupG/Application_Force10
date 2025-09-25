using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class DigitalConsentForm : CENTRUMBase
    {
        string vDCUrl = ConfigurationManager.AppSettings["DCUrl"];
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
                this.PageHeading = "Digital Consent Form";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDigiConsent);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport != "Y" && this.CanView == "Y")
                {
                    btnPrint.Visible = false;
                }
                else if (this.CanReport == "Y" && this.CanView == "Y")
                {
                    btnPrint.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Digital Consent Form", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CReports oCr = null;
            try
            {
                oCr = new CReports();
                dt = oCr.ChkEnqId(txtEnquiryId.Text, Convert.ToString(Session[gblValue.BrnchCode]));
                if (dt.Rows.Count > 0)
                {
                    string url = vDCUrl + "?p=" + txtEnquiryId.Text;
                    string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Invalid Initial Approach No");
                }
            }
            catch (Exception ex)
            {
                dt = null;
                oCr = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}