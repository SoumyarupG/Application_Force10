using System;
using System.Collections;
using System.Collections.Generic;
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
using System.IO;
using FORCECA;
using FORCEBA;


namespace CENTRUM.WebPages.Private.Report
{
    public partial class LoanDisbursement : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //rblSel.SelectedValue = "rbWhole";
                PopList();
                CheckAll();
                popDetail();
                //txtDtFrm.Enabled = false;
                //txtToDt.Enabled = false;
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Disbursement List";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuLoanDisbRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Portfolio Ageing", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        private void PopList()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oRO = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblSel.SelectedValue == "rbEO")
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "Eoid";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbFund")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "FundSource";
                chkDtl.DataValueField = "FundSourceId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rdbPurps")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "PurposeID", "Purpose", "LoanPurposeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "Purpose";
                chkDtl.DataValueField = "PurposeID";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rdbCycle")
            {
                Dictionary<string, Int32> oDic = new Dictionary<string, Int32>();
                oDic.Add("Cycle 0", 0);
                oDic.Add("Cycle 1", 1);
                oDic.Add("Cycle 2", 2);
                oDic.Add("Cycle 3", 3);
                oDic.Add("Cycle 4", 4);
                oDic.Add("Cycle 5", 5);
                oDic.Add("Cycle 6", 6);
                oDic.Add("Cycle 7", 7);
                oDic.Add("Cycle 8", 8);
                oDic.Add("Cycle 9", 9);
                oDic.Add("Cycle 10", 10);
                chkDtl.DataSource = oDic;
                chkDtl.DataValueField = "value";
                chkDtl.DataTextField = "key";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbWhole")
            {
                chkDtl.DataSource = null;
                chkDtl.DataBind();
                chkDtl.Items.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            string pBranch = ViewState["Dtl"].ToString();
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vRptPath = "", vTypeId = "", vType = "";
            string vBranch = Session[gblValue.BrName].ToString();
            int vNEFT = 0;
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            vTypeId = ViewState["Dtl"].ToString();

            if (chkNEFT.Checked == true) vNEFT = 1;

            if (rblSel.SelectedValue == "rbEO") vType = "E";
            if (rblSel.SelectedValue == "rbLType") vType = "L";
            if (rblSel.SelectedValue == "rbFund") vType = "F";
            if (rblSel.SelectedValue == "rbWhole") vType = "A";
            if (rblSel.SelectedValue == "rdbPurps") vType = "P";
            if (rblSel.SelectedValue == "rdbCycle") vType = "C";

            if (rbOpt.SelectedValue == "rbDtl")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbDtl.rpt";
            if (rbOpt.SelectedValue == "rbSum")
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbSum.rpt";

            TimeSpan t = vToDt - vFromDt;
            if (this.RoleId != 1 && this.RoleId != 5 && this.RoleId != 11 && this.RoleId != 51)
            {
                if (t.TotalDays > 0)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 1 days report.");
                    return;
                }
            }
            else
            {
                if (t.TotalDays > 30)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 30 days report.");
                    return;
                }
            }

            if (pMode == "PDF")
            {
                dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pType", vType);
                    rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                    rptDoc.SetParameterValue("dtTo", txtToDt.Text);

                    //if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Disbursement List");
                    //else if (pMode == "Excel")
                    //    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan Disbursement List");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else
            {
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                dt = oRpt.rptLoanDisbXls(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();

                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                string vFileNm = "attachment;filename=Disbursement Report.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");             
                htw.WriteLine("<tr><td align=center' colspan='20'><b><u><font size='5'>Loan Disbursement Report</font></u></b></td></tr>");              
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
                htw.WriteLine("</table>");
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(sw.ToString());
                Response.End();

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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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
    }
}
