using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class UpcomingClsLoan : CENTRUMBase
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
                txtDtFrom.Text = Session[gblValue.LoginDate].ToString();
                txtDtTo.Text = Session[gblValue.LoginDate].ToString();
                ViewState["ID"] = null;
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
                this.PageHeading = "Upcoming Loan to be Closed";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuUpcmgClsLoanRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Upcoming Loan to be Closed Report", false);
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
            Int32 vRow = 0, vBrId = 0; ;
            string strin = "", vBrCode = "";
            ViewState["ID"] = null;
            DataTable dt = null;
            CEO oRO = null;
            CGblIdGenerator oCG = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                if (rdbOpt.SelectedValue == "rdbCo")
                {
                    vBrCode = (string)Session[gblValue.BrnchCode];
                    vBrId = Convert.ToInt32(vBrCode);
                    //oCG = new CGblIdGenerator();
                    //dt = oCG.PopComboMIS("D", "N", "AA", "EoId", "EoName", "EoMst", vBrCode, "BranchCode", "Tra_DropDate", vLogDt, vBrCode);
                    //chkDtl.DataSource = dt;
                    //chkDtl.DataTextField = "EoName";
                    //chkDtl.DataValueField = "EoID";
                    //chkDtl.DataBind();
                    oRO = new CEO();
                    dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "EoName";
                    chkDtl.DataValueField = "Eoid";
                    chkDtl.DataBind();
                }
                if (rdbOpt.SelectedValue == "rdbShg")
                {
                    vBrCode = (string)Session[gblValue.BrnchCode];
                    vBrId = Convert.ToInt32(vBrCode);
                    oCG = new CGblIdGenerator();
                    dt = oCG.PopComboMIS("N", "N", "GroupCode", "GroupID", "GroupName", "GroupMst", 0, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "GroupName";
                    chkDtl.DataValueField = "GroupID";
                    chkDtl.DataBind();
                }
                if (rdbOpt.SelectedValue == "rdbMarket")
                {
                    vBrCode = (string)Session[gblValue.BrnchCode];
                    vBrId = Convert.ToInt32(vBrCode);
                    oCG = new CGblIdGenerator();
                    dt = oCG.PopComboMIS("N", "N", "AA", "MarketID", "Market", "MarketMst", vBrId, "BranchCode", "AA", gblFuction.setDate("01/01/1900"), vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Market";
                    chkDtl.DataValueField = "MarketID";
                    chkDtl.DataBind();
                }
                if (rdbOpt.SelectedValue == "rdbPurps")
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
                if (rdbOpt.SelectedValue == "rdbFndSrc")
                {
                    vBrCode = (string)Session[gblValue.BrnchCode];
                    vBrId = Convert.ToInt32(vBrCode);
                    oCG = new CGblIdGenerator();
                    dt = oCG.PopComboMIS("N", "N", "AA", "FundSourceId", "FundSource", "FundSourceMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "FundSource";
                    chkDtl.DataValueField = "FundSourceId";
                    chkDtl.DataBind();
                }
                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    chkDtl.DataSource = null;
                    chkDtl.DataBind();
                }
                if (rdbSel.SelectedValue == "rdbAll")
                {
                    for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    {
                        chkDtl.Items[vRow].Selected = true;
                        if (strin == "")
                            strin = chkDtl.Items[vRow].Value;
                        else
                            strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
                else if (rdbSel.SelectedValue == "rdbSelct")
                {
                    for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    {
                        chkDtl.Items[vRow].Selected = false;
                    }
                }
                ViewState["ID"] = strin;
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
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                chkDtl.Enabled = false;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                {
                    chkDtl.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkDtl.Items[vRow].Value;
                    else
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
            {
                ViewState["ID"] = null;
                chkDtl.Enabled = true;
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vWithDef = "";
            DateTime vFrmDt = gblFuction.setDate(txtDtFrom.Text);
            DateTime vToDt = gblFuction.setDate(txtDtTo.Text);
            string strID = ViewState["ID"].ToString(), vMode = "A";
            DataTable dt = null;
            CReports oRpt = null;
            ReportDocument rptDoc = null;

            try
            {
                oRpt = new CReports();
                rptDoc = new ReportDocument();
                if (rdbOpt.SelectedValue == "")
                {
                    gblFuction.AjxMsgPopup("Please select atleast one option...");
                    return;
                }
                if (rdbOpt.SelectedValue != "rdbAll")
                {
                    if (strID == null || strID == "")
                    {
                        gblFuction.MsgPopup("Please Select Group from List");
                        return;
                    }
                }
                

                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    vMode = "A";
                }
                else if (rdbOpt.SelectedValue == "rdbCo")
                {
                    vMode = "C";
                }
                else if (rdbOpt.SelectedValue == "rdbShg")
                {
                    vMode = "G";
                }
                else if (rdbOpt.SelectedValue == "rdbMarket")
                {
                    vMode = "V"; // For Market
                }
                else if (rdbOpt.SelectedValue == "rdbPurps")
                {
                    vMode = "P";
                }
                else if (rdbOpt.SelectedValue == "rdbFndSrc")
                {
                    vMode = "F";
                }

                dt = oRpt.rptLoanUpComing(vFrmDt, vToDt, strID, vBrCode, vMode);
                //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatus.rpt";
                //rptDoc.Load(vRptPath);
                //rptDoc.SetDataSource(dt);
                //rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                //rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //rptDoc.SetParameterValue("pAddress2", "");
                //rptDoc.SetParameterValue("pBranch", vBranch);
                //rptDoc.SetParameterValue("AsOnDt", txtAsDt.Text);
                //rptDoc.SetParameterValue("pTitle", vTitle);
                //rptDoc.SetParameterValue("pType", vType);
                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Upcoming_Loan_to_be_Closed");
                else if (pMode == "Excel")
                    dgLoanStatus.DataSource = dt;
                    dgLoanStatus.DataBind();

                    string vFileNm = "attachment;filename=sheet1";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                    htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    //htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + Session[gblValue.CompName].ToString() + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Upcoming Loan to be Closed</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    htw.WriteLine("<tr><td align='right' colspan='" + ((dgLoanStatus.Columns.Count - 1) / 2) + "'><b>Date From : " + txtDtFrom.Text + "'&nbsp&nbsp&nbsp&nbsp<b>Date To : " + txtDtTo.Text);
                    htw.WriteLine("<tr><td></td></tr>");
                    dgLoanStatus.RenderControl(htw);
                    htw.WriteLine("</table>");
                    dgLoanStatus.DataSource = null;
                    dgLoanStatus.DataBind();
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                    Response.Write(sw.ToString());
                    Response.End();
            }
            finally
            {
                dt = null;
                oRpt = null;
                rptDoc = null;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetRptData("PDF");
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
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkDtl.Items[vRow].Value;
                    else
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetRptData("Excel");
        }
    }
}
