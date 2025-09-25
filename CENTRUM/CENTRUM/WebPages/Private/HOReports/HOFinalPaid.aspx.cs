using System;
using System.Data;
using FORCEBA;
using FORCECA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOFinalPaid : CENTRUMBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                PopFund();
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Agora.aspx", false);

                this.Menu = false;
                this.PageHeading = "Settled Client (HO)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOSetlClnt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidate Portfolio Ageing", false);
                }
            }
            catch
            {
                Response.Redirect("~/Agora.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            ViewState["ID"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                cblBr.DataSource = dt;
                cblBr.DataTextField = "Name";
                cblBr.DataValueField = "BranchCode";
                cblBr.DataBind();
                CheckAll("B");
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
                    cblBr.DataSource = dt;
                    cblBr.DataTextField = "BranchName";
                    cblBr.DataValueField = "BranchCode";
                    cblBr.DataBind();
                    CheckAll("B");
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
        private void PopFund()
        {
            ViewState["FundID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            try
            {
                cblFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceID", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                cblFund.DataSource = dt;
                cblFund.DataTextField = "FundSource";
                cblFund.DataValueField = "FundSourceID";
                cblFund.DataBind();
                CheckAll("F");
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
        private void PopLoanType()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            ViewState["FundID"] = null;
            try
            {
                cblFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                cblFund.DataSource = dt;
                cblFund.DataTextField = "LoanType";
                cblFund.DataValueField = "LoanTypeId";
                cblFund.DataBind();
                CheckAll("F");
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbMode.SelectedValue == "A")
                cblFund.Items.Clear();
            else if (rdbMode.SelectedValue == "F")
                PopFund();
            else if (rdbMode.SelectedValue == "L")
                PopLoanType();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll(string vMode)
        {
            Int32 vRow;
            string strin = "";
            if (vMode == "F")
            {
                if (ddlFund.SelectedValue == "A")
                {
                    cblFund.Enabled = false;
                    for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                    {
                        cblFund.Items[vRow].Selected = true;
                        if (strin == "")
                            strin = cblFund.Items[vRow].Value;
                        else
                            strin = strin + "," + cblFund.Items[vRow].Value + "";
                    }
                }
                else if (ddlFund.SelectedValue == "S")
                {
                    ViewState["FundID"] = null;
                    cblFund.Enabled = true;
                    for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
                        cblFund.Items[vRow].Selected = false;
                }
                ViewState["FundID"] = strin;
            }
            if (vMode == "B")
            {
                if (ddlBr.SelectedValue == "A")
                {
                    cblBr.Enabled = false;
                    for (vRow = 0; vRow < cblBr.Items.Count; vRow++)
                    {
                        cblBr.Items[vRow].Selected = true;
                        if (strin == "")
                            strin = cblBr.Items[vRow].Value;
                        else
                            strin = strin + "," + cblBr.Items[vRow].Value + "";
                    }
                }
                else if (ddlBr.SelectedValue == "S")
                {
                    ViewState["ID"] = null;
                    cblBr.Enabled = true;
                    for (vRow = 0; vRow < cblBr.Items.Count; vRow++)
                        cblBr.Items[vRow].Selected = false;

                }
                ViewState["ID"] = strin;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetFund()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < cblFund.Items.Count; vRow++)
            {
                if (cblFund.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = Convert.ToString(cblFund.Items[vRow].Value);
                    else
                        strin = strin + "," + Convert.ToString(cblFund.Items[vRow].Value);
                }
            }
            ViewState["FundID"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < cblBr.Items.Count; vRow++)
            {
                if (cblBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = cblBr.Items[vRow].Value;
                    else
                        strin = strin + "," + cblBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll("F");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll("B");
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
                ViewState["FundID"] = null;
                ViewState["ID"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vCType = "";
            string vBranch = Convert.ToString(ViewState["ID"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vPrm = Convert.ToString(ViewState["FundID"]);
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\HOPortfolioAgeing.rpt";
            string vMode = rdbMode.SelectedValue;
            DataTable dt = null;
            ReportDocument rptDoc = null;
            CReports oRpt = null;
            try
            {
                GetBranch();
                vBranch = Convert.ToString(ViewState["ID"]);
                GetFund();
                oRpt = new CReports();
                rptDoc = new ReportDocument();
                DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                if (rdbGrp.SelectedValue == "rdbNormal")
                    vCType = "N";
                else if (rdbGrp.SelectedValue == "rdbAll")
                    vCType = "A";
                else if (rdbGrp.SelectedValue == "rdbPrem")
                    vCType = "P";
                else if (rdbGrp.SelectedValue == "rdbDeath")
                    vCType = "D";
                if (rdbMode.SelectedValue == "A" && ddlFund.SelectedValue == "A")
                {
                    dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, "0", "0", "", "", "", vBranch);
                }
                if (rdbMode.SelectedValue == "F" && ddlFund.SelectedValue == "A")
                {
                    dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, "0", "0", "", "", "", vBranch);
                }
                else if (rdbMode.SelectedValue == "F" && ddlFund.SelectedValue == "S")
                {
                    dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, vPrm, "0", "", "", "", vBranch);
                }
                if (rdbMode.SelectedValue == "L" && ddlFund.SelectedValue == "A")
                {
                    dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, "0", "0", "", "", "", vBranch);
                }
                else if (rdbMode.SelectedValue == "L" && ddlFund.SelectedValue == "S")
                {
                    dt = oRpt.rptFinalPaidList(vFromDt, vToDt, vCType, "0", vPrm, "", "", "", vBranch);
                }
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptHOFinalPaid.rpt";
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("FromDt", txtDtFrm.Text);
                rptDoc.SetParameterValue("ToDt", txtToDt.Text);
                rptDoc.SetParameterValue("pTitle", "Settled Client List");
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Settled_Client_List_Report");
                else if (pFormat == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Settled_Client_List_Report");

                rptDoc.Close();
                rptDoc.Dispose();
            }
            finally
            {
                dt = null;
                rptDoc = null;
                oRpt = null;
            }
        }

    }
}
