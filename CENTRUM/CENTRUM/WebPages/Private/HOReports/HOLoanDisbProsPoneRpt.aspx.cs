using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOLoanDisbProsPoneRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Disbursement Prospone";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOLoanDisbProspone);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan  Disbursement Prospone", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            string vTag = "N", vRptPath = "", vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            if (rdbOpt.SelectedValue == "rdDisb")
            {
                vTag = "D";
                vTitle = "Loan Disbursed Report.";
            }
            else if (rdbOpt.SelectedValue == "rdCDisb")
            {
                vTag = "F";
                vTitle = "Approve for Cash Disbursed Report.";
            }
            else if (rdbOpt.SelectedValue == "rdCN")
            {
                vTag = "C";
                vTitle = "Cancel Report.";
            }
            else if (rdbOpt.SelectedValue == "rdPros")
            {
                vTag = "P";
                vTitle = "Postponed Report.";
            }
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbProspon.rpt";
            dt = oRpt.rptLoanDisbProspone(vTag, vFromDt, vToDt, vBrCode);
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", vTitle);
                rptDoc.SetParameterValue("dtFrom", txtFromDt.Text);
                rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "HO_Loan_ Disbursement_Prospone_Report");
                else if (pMode == "Excel")
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "HO_Loan_ Disbursement_Prospone_Report");

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (ddlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (ddlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (ddlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            ViewState["BrCode"] = strin;
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}