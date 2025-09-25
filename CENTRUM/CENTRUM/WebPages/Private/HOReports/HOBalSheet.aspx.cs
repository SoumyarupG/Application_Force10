using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOBalSheet : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtAsOn.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
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
                this.GetModuleByRole(mnuID.mnuHOBalSheet);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Balance Sheet", false);
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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            GetBranch();

            string vBrCode = Convert.ToString(ViewState["ID"]);
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BalanceSheet.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtDtAsOn.Text) || gblFuction.setDate(txtDtAsOn.Text) > vFinTo)
            {
                gblFuction.MsgPopup("Date should be within this financial year.");
                return;
            }

            dt = oRpt.rptBSheet(vAcMst, vAcDtl, vBrCode, gblFuction.setStrDate(txtDtAsOn.Text), vFinYr,
                        gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()));
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Consolidate Balance Sheet");
                rptDoc.SetParameterValue("DtFrom", txtDtAsOn.Text);
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtDtAsOn.Text).ToString("yyyyMMdd") + "_Consolidate_Balance_Sheet");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtDtAsOn.Text).ToString("yyyyMMdd") + "_Consolidate_Balance_Sheet");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }


        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            //Int32 vBrId = 0;
            //string vBrCode = "";
            ViewState["ID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            try
            {
                //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                //vBrCode = (string)Session[gblValue.BrnchCode];
                //vBrId = Convert.ToInt32(vBrCode);

                DateTime vLogDt = gblFuction.setDate(txtDtAsOn.Text.ToString());
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, "0000");
                chkBr.DataSource = dt;
                chkBr.DataTextField = "Name";
                chkBr.DataValueField = "BranchCode";
                chkBr.DataBind();
                CheckAll();
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
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
        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
