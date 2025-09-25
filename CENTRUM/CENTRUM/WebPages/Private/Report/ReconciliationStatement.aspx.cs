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
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class ReconciliationStatement : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopList();
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
                this.PageHeading = "Bank Reconciliation Statement";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuBankRconsilStatRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Book", false);
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
        private void PopList()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            CVoucher oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB(vBrCode, "B", "");
            ddlLedger.DataSource = dt;
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescId";
            ddlLedger.DataBind();
        }


        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BankReconRpt.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vDescID = ddlLedger.SelectedValue;
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();


            dt = oRpt.rptBankRecon(vAcMst, vAcDtl, gblFuction.setDate(txtDt.Text), vFinFrom, vDescID, vFinYrNo, vBrCode);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Bank Reconcilition Statement");
                rptDoc.SetParameterValue("pDt", txtDt.Text);
                rptDoc.SetParameterValue("pLedger", ddlLedger.SelectedItem.Text);
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Bank Reconcilition Statement");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Bank Reconcilition Statement");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            GetData("Excel");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
