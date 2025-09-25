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
    public partial class BankRepaySchedule : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBankersName();
            }
        }

        private void PopBankersName()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlBn.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "BankerId", "BankerName", "BankerMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlBn.DataSource = dt;
                ddlBn.DataTextField = "BankerName";
                ddlBn.DataValueField = "BankerId";
                ddlBn.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlBn.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "BankRepaySchedule";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBankRepaySch);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/Public/PageAccess.aspx?mnuTxt=" + "BankRepaySchedule", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }
        protected void ddlBn_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            ddlLn.Items.Clear();
            Int32 vGrpId = Convert.ToInt32(ddlBn.SelectedValue);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = new DataTable();
            CGblIdGenerator oGbl = new CGblIdGenerator();
            dt = oGbl.PopComboMIS("S", "N", "AA", "LoanId", "LoanAcNo", "BankLoanMst", vGrpId, "BankerID", "AA", System.DateTime.Now, "0000");
            ddlLn.DataSource = dt;
            ddlLn.DataTextField = "LoanAcNo";
            ddlLn.DataValueField = "LoanId";
            ddlLn.DataBind();
            ListItem oLi = new ListItem("<--Select-->", "-1");
            ddlLn.Items.Insert(0, oLi);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptrptBankRepaySchedule.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanId = "";
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CReports oRpt = null;
            try
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    if (ddlLn.SelectedIndex > 0)
                        vLoanId = ddlLn.SelectedValue;
                    else
                    {
                        gblFuction.MsgPopup("No Records Found.");
                        return;
                    }
                    ds = oRpt.rptBankRepaySche(vLoanId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt1);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pTitle", "Bank Repayment Schedule");
                    rptDoc.SetParameterValue("pBankersName", ddlBn.SelectedItem.Text);
                    rptDoc.SetParameterValue("pLoanAmt", Convert.ToString(dt.Rows[0]["LoanAmt"]));
                    rptDoc.SetParameterValue("pLoanDt", Convert.ToString(dt.Rows[0]["LoanDt"]).Substring(0, 10));
                    rptDoc.SetParameterValue("pFLDGAmt", Convert.ToString(dt.Rows[0]["FLDGAmt"]));
                    //if (Convert.ToString(dt.Rows[0]["FLDGBy"]) == "P")
                    //    rptDoc.SetParameterValue("pFLDGBy", gblValue.CompName);
                    //if (Convert.ToString(dt.Rows[0]["FLDGBy"]) == "A")
                    //    rptDoc.SetParameterValue("pFLDGBy", "AIF");
                    if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    else
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            finally
            {
                ds = null;
                dt = null;
                dt1 = null;
                oRpt = null;
            }
        }
    }
}