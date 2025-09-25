using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class BalSheet : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopGenLedger();
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
                this.PageHeading = "Balance Sheet";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuBalanceSheetRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Balance Sheet", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopGenLedger()
        {
            txtDtAsOn.Text = Convert.ToString(Session[gblValue.LoginDate]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BalanceSheet.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            dt = oRpt.rptBSheet(vAcMst, vAcDtl, vBrCode, gblFuction.setStrDate(txtDtAsOn.Text), vFinYr,
                        gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()));
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Balance Sheet");
                rptDoc.SetParameterValue("DtFrom", txtDtAsOn.Text);
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Balance Sheet");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Balance Sheet");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            GetData("Excel");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
