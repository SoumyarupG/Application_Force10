using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web;
using FORCECA;
using FORCEBA;
using ClosedXML.Excel;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class DmdCollStatus : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["ID"] = null;
                PopList();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Demand And Collection Status";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLoanStatusRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Demand And Collection Status Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopList()
        {
            Int32 vRow;
            string strin = "";
            ViewState["ID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oRo = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            if (rdbOpt.SelectedValue == "rdbLoanType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
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
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
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
            string vWithDef = "", vWithPDDDt = "";
            DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string strID = ViewState["ID"].ToString(), vMode = "A", vRptPath = "", vTitle = "", vType = "D";
            DataTable dt = null;
            CReports oRpt = null;
            //ReportDocument rptDoc = null;

            try
            {
                oRpt = new CReports();
                //rptDoc = new ReportDocument();
                if (rdbOpt.SelectedValue == "")
                {
                    gblFuction.AjxMsgPopup("Please select atleast one option...");
                    return;
                }                
                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    vMode = "A";
                    vTitle = "Demand & Collection Status Report - All";
                }                
                else if (rdbOpt.SelectedValue == "rdbLoanType")
                {
                    vMode = "L";
                    vTitle = "Demand & Collection Status Report - Loan Type Wise";
                }
                vWithDef = "N";
                vWithPDDDt = "N";
                dt = oRpt.rptLoanStatus("", vFromDt , strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DemandAndCollectionStatus.rpt";
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    string vFileNm = "";
                    vFileNm = "attachment;filename=Demand_Collection_Status.xlsx";
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
                //using (ReportDocument rptDoc = new ReportDocument())
                //{
                //    rptDoc.Load(vRptPath);
                //    rptDoc.SetDataSource(dt);
                //    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                //    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //    rptDoc.SetParameterValue("pAddress2", "");
                //    rptDoc.SetParameterValue("pBranch", vBranch);
                //    rptDoc.SetParameterValue("dtFrom", txtFDt.Text);
                //    //rptDoc.SetParameterValue("dtTo", txtTDt.Text);
                //    rptDoc.SetParameterValue("pTitle", vTitle);
                //    rptDoc.SetParameterValue("pType", vType);
                //    //if (pMode == "PDF")
                //    //    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan_Status");
                //    if (pMode == "Excel")
                //        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Demand_Collection _Status");

                //    rptDoc.Dispose();
                //    Response.ClearHeaders();
                //    Response.ClearContent();
                //}
            }
            finally
            {
                dt = null;
                oRpt = null;
                //rptDoc = null;

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
            //if (rdbSummary.SelectedValue == "rdbDtls")
            Export();
            //else
            //    SetRptData("Excel");
        }

        private void Export()
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vWithDef = "", vWithPDDDt = "";
            DateTime vFromDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string strID = ViewState["ID"].ToString(), vMode = "A", vRptPath = "", vTitle = "", vType = "D";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                if (rdbOpt.SelectedValue == "")
                {
                    gblFuction.AjxMsgPopup("Please select atleast one option...");
                    return;
                }                
                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    vMode = "A";
                    vTitle = "Demand & Collection Status Report - All";
                }
                else if (rdbOpt.SelectedValue == "rdbLoanType")
                {
                    vMode = "L";
                    vTitle = "Demand & Collection Status Report - Loan Type Wise";
                }
                vWithDef = "N";
                vWithPDDDt = "N";
                ReportDocument rptDoc = new ReportDocument();
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();                
                dt = oRpt.rptLoanStatus("", vFromDt, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                string vFileNm = "attachment;filename=Demand_Collection_Status_Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address1 + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + gblValue.Address2 + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Demand Collection Status Report</font></u></b></td></tr>");
                Response.Write("<tr><td></td></tr>");
                Response.Write("<tr><td align='center' colspan='" + dt.Columns.Count + "'><b>From : " + gblFuction.setDate(txtFDt.Text) + " To : " + gblFuction.setDate(txtTDt.Text) + "</b></td></tr>");
                Response.Write("<tr><td></td></tr>");
                string tab = string.Empty;
                Response.Write("<tr>");
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (dt.Columns[j].ColumnName == "LoanNo")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }

                        else
                        {
                            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
                Response.End();
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
    }
}
