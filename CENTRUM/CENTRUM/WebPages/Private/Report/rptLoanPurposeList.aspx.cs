using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptLoanPurposeList : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                rbDtlsSumm_SelectedIndexChanged(sender, e);
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
                this.PageHeading = "Loan Purpose";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBankBookRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Purpose", false);
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
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode, string pSecId)
        {
            string vRptPath = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            string pa = "";
            if (rbDtlsSumm.SelectedValue == "0")
                pa = "A";
            else
                pa = "S";
            dt = oRpt.rptLPurposeList(pa, pSecId);
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanPurposeList.rpt";
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pBrCode", vBrCode);
                rptDoc.SetParameterValue("pTitle", "Loan Purpose Master Report");
                //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //rptDoc.SetParameterValue("pFrmDt", txtFrmDt.Text);
                //rptDoc.SetParameterValue("pToDt", txtToDt.Text);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan_Purpose.pdf");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan_Purpose.xls");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ddlSector.SelectedValue == "-1" && rbDtlsSumm.SelectedValue != "0")
                gblFuction.MsgPopup("Please select a sector");
            else
                SetParameterForRptData("PDF", ddlSector.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ddlSector.SelectedValue == "-1" && rbDtlsSumm.SelectedValue != "0")
                gblFuction.MsgPopup("Please select a sector");
            else
                SetParameterForRptData("Excel", ddlSector.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopSector()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PurposeID", "Purpose", "LoanPurposeMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlSector.DataSource = dt;
                ddlSector.DataTextField = "Purpose";
                ddlSector.DataValueField = "PurposeID";
                ddlSector.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlSector.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbDtlsSumm_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopSector();
            if (rbDtlsSumm.SelectedIndex == 0)
            {
                ddlSector.Enabled = false;
            }
            else
                ddlSector.Enabled = true;
        }
    }
}
