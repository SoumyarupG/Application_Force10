using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class ProfitLoss : CENTRUMBase
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
                this.PageHeading = "Profit & Loss";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuProfitLossRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Profit & Loss A/c Report", false);
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
            txtDtFrm.Text = Convert.ToString(Session[gblValue.FinFromDt]);
            txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\PL.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            double vInc = 0, vExp = 0, vTot = 0;
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            dt = oRpt.rptPL(vAcMst, vAcDtl, vBrCode, gblFuction.setStrDate(txtDtFrm.Text), gblFuction.setStrDate(txtToDt.Text), vFinYr);
            foreach (DataRow dR in dt.Rows)
            {
                if (dR["ACHead"].ToString() == "Income")
                    vInc += Convert.ToDouble(dR["Amt"]);

                if (dR["ACHead"].ToString() == "Expense")
                    vExp += Convert.ToDouble(dR["Amt"]);
            }
            vTot = (vInc - vExp);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);

                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Profit And Loss");
                rptDoc.SetParameterValue("DtFrom", txtDtFrm.Text);
                rptDoc.SetParameterValue("DtTo", txtToDt.Text);
                rptDoc.SetParameterValue("Profit", vTot);
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Profit_Loss");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Profit_Loss");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            //if (gblFuction.CheckDtRange(txtFromDt.Text, txtToDate.Text) == false)
            //{
            //    vRst = false;
            //}
            return vRst;
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("Excel");
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

    }
}
