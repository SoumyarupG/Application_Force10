using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class AuditSubBranch : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                popBranch();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branch Inspection";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAuditSubmission);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void popBranch()
        {
            DataTable dt = null;
            CBranch oGb = null;
            try
            {
                oGb = new CBranch();
                dt = oGb.GetBranchList();
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vAsonDt = gblFuction.setDate(txtDt.Text);
            string vRptPath = "", vTitle = "Branch Wise Online Audit Submission", vBrCode = "";
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();
            //try
            //{
            if (ddlBranch.SelectedIndex > 0)
            {
                vBrCode = ddlBranch.SelectedValue;
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Select Branch");
                return;
            }
            string vAddress1 = CGblIdGenerator.GetBranchAddress1(vBrCode);
            dt = oRpt.rptAuditSubBranch(vAsonDt, vBrCode);

            if (pMode == "PDF")
            {
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AuditSubBranch.rpt";
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pBranch", ddlBranch.SelectedItem.Text);
                    rptDoc.SetParameterValue("pAddress1", vAddress1);
                    rptDoc.SetParameterValue("dtAsOn", txtDt.Text);
                    rptDoc.SetParameterValue("pTitle", vTitle);

                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Branch Inspection Summary Report");
                    else
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Branch Inspection Summary Report");
                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");

        }
    }
}
