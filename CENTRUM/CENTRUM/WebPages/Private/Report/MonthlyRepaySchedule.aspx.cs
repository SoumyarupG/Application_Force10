using System;
using System.Data;
using FORCECA;
using CrystalDecisions.CrystalReports.Engine;
using FORCEBA;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class MonthlyRepaySchedule : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Monthly RepaySchedule";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuMonthlyRepaySch);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/Public/PageAccess.aspx?mnuTxt=" + "MonthlyRepaySchedule", false);
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
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBranch = Session[gblValue.BrName].ToString();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vRptPath = "";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    oRpt = new CReports();
                    dt = oRpt.rptdetail(vFromDt, vToDt);
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptrptdetail.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pTitle", "Monthly RepaySchedule");
                    rptDoc.SetParameterValue("pFrmDt", txtDtFrm.Text);
                    rptDoc.SetParameterValue("pToDt", txtToDt.Text);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Monyhly_RepaySchedule");
                    else if (pMode == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Monyhly_RepaySchedule");
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnExcl_Click(object sender, EventArgs e)
        //{
        //    SetParameterForRptData("Excel");
        //}


        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnExit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("~/WebPages/Public/Main.aspx");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        protected void txtToDt_TextChanged(object sender, EventArgs e)
        {
            if (gblFuction.IsDate(txtToDt.Text) == true)
            {
            }
        }
    }
}