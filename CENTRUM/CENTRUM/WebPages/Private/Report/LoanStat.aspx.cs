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
    public partial class LoanStat : CENTRUMBase
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
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Loan Status";
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Status Report", false);
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
            CGblIdGenerator oCG = null;
            CEO oRO = null;
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
                if (rdbOpt.SelectedValue == "rdbLoanType")
                { 
                    oCG = new CGblIdGenerator();
                    dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "LoanType";
                    chkDtl.DataValueField = "LoanTypeId";
                    chkDtl.DataBind();
                }
                if (rdbOpt.SelectedValue == "rdbProduct")
                {
                    oCG = new CGblIdGenerator();
                    dt = oCG.PopComboMIS("N", "N", "AA", "ProductId", "Product", "LoanProductMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Product";
                    chkDtl.DataValueField = "ProductId";
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
            string vWithDef = "", vWithPDDDt = "";
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
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
                if (rdbOpt.SelectedValue != "rdbAll")
                {
                    if (strID == null || strID == "")
                    {
                        gblFuction.MsgPopup("Please Select Group from List");
                        return;
                    }
                }
                if (rdbSummary.SelectedValue == "rdbDtls")
                    vType = "D";
                else
                    vType = "S";

                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    vMode = "A";
                    vTitle = "Loan Status Report - All";
                }
                else if (rdbOpt.SelectedValue == "rdbCo")
                {
                    vMode = "C";
                    vTitle = "Loan Status Report - L.O Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbShg")
                {
                    vMode = "G";
                    vTitle = "Loan Status Report - Group Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbMarket")
                {
                    vMode = "V"; // For Market
                    vTitle = "Loan Status Report - L.O Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbPurps")
                {
                    vMode = "P";
                    vTitle = "Loan Status Report - Purpose Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbFndSrc")
                {
                    vMode = "F";
                    vTitle = "Loan Status Report - Fund Source Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbLoanType")
                {
                    vMode = "L";
                    vTitle = "Loan Status Report - Loan Type Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbProduct")
                {
                    vMode = "R";
                    vTitle = "Loan Status Report - Product Wise";
                }

                if (chkOnlyDef.Checked == true)
                    vWithDef = "Y";
                else
                    vWithDef = "N";

                if (chkWithPddDt.Checked == true)
                    vWithPDDDt = "Y";
                else
                    vWithPDDDt = "N";
                dt = oRpt.rptLoanStatus("", vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatus.rpt";
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("AsOnDt", txtAsDt.Text);
                    rptDoc.SetParameterValue("pTitle", vTitle);
                    rptDoc.SetParameterValue("pType", vType);
                    if (pMode == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan_Status");
                    else if (pMode == "Excel")
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan_Status");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
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
            if (rdbSummary.SelectedValue == "rdbDtls")
                Export();
            else
                SetRptData("Excel");
        }

        private void Export()
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vWithDef = "", vWithPDDDt = "";
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
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
                if (rdbOpt.SelectedValue != "rdbAll")
                {
                    if (strID == null || strID == "")
                    {
                        gblFuction.MsgPopup("Please Select Group from List");
                        return;
                    }
                }
                if (rdbSummary.SelectedValue == "rdbDtls")
                    vType = "D";
                else
                    vType = "S";

                if (rdbOpt.SelectedValue == "rdbAll")
                {
                    vMode = "A";
                    vTitle = "Loan Status Report - All";
                }
                else if (rdbOpt.SelectedValue == "rdbCo")
                {
                    vMode = "C";
                    vTitle = "Loan Status Report - L.O Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbShg")
                {
                    vMode = "G";
                    vTitle = "Loan Status Report - Group Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbMarket")
                {
                    vMode = "V"; // For Market
                    vTitle = "Loan Status Report - L.O Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbPurps")
                {
                    vMode = "P";
                    vTitle = "Loan Status Report - Purpose Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbFndSrc")
                {
                    vMode = "F";
                    vTitle = "Loan Status Report - Fund Source Wise";
                }
                else if (rdbOpt.SelectedValue == "rdbProduct")
                {
                    vMode = "R";
                    vTitle = "Loan Status Report - Product Wise";
                }
                if (chkOnlyDef.Checked == true)
                    vWithDef = "Y";
                else
                    vWithDef = "N";

                if (chkWithPddDt.Checked == true)
                    vWithPDDDt = "Y";
                else
                    vWithPDDDt = "N";

                //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                dt = oRpt.rptLoanStatusXlsx("", vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    var ws = wb.Worksheets.Add(dt, "Customers");
                //    ws.Cell(1, 1).Value = gblValue.CompName;                   
                //    ws.Cell(1, 1).Style.Font.Bold = true;
                //    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //    ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();
                //    ws.Cell(2, 1).Value = CGblIdGenerator.GetBranchAddress1(vBrCode);
                //    ws.Cell(2, 1).Style.Font.Bold = true;
                //    ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //    ws.Range(ws.Cell(2, 1).Address, ws.Cell(2, dt.Columns.Count).Address).Row(1).Merge();
                //    ws.Cell(3, 1).Value = "As On : " + txtAsDt.Text;
                //    ws.Cell(3, 1).Style.Font.Bold = true;
                //    ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //    ws.Range(ws.Cell(3, 1).Address, ws.Cell(3, dt.Columns.Count).Address).Row(1).Merge();
                    
                //    ws.Cell(4, 1).InsertTable(dt);
                //    ws.SheetView.FreezeRows(3); //freeze rows
                //    ws.Columns().AdjustToContents();
                //    Response.Clear();
                //    Response.Buffer = true;
                //    Response.Charset = "";
                //    string vFileNm = "";
                //    vFileNm = "attachment;filename=Loan_Status.xlsx";
                //    Response.AddHeader("content-disposition", vFileNm);
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //        wb.SaveAs(MyMemoryStream);
                //        MyMemoryStream.WriteTo(Response.OutputStream);
                //        Response.Flush();
                //        Response.End();
                //    }
                //}

                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                string vFileNm = "attachment;filename=Loan_Status.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>Loan Status Report for " + vBranch + " Branch As On " + txtAsDt.Text + "</font></u></b></td></tr>");
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
                

                //dgLoanStatus.DataSource = dt;
                //dgLoanStatus.DataBind();

                //DataGrid1.DataSource = dt;
                //DataGrid1.DataBind();

                //tdx.Controls.Add(DataGrid1);
                //tdx.Visible = false;
                //string vFileNm = "attachment;filename=Loan_Status.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", vFileNm);
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "application/vnd.ms-excel";
                //HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                //Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + "</font></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='4'>Loan Status Report</font></u></b></td></tr>");
                //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>As On : " + txtAsDt.Text + "</font></td></tr>");
                //string tab = string.Empty;
                //Response.Write("<tr>");
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                //}
                //Response.Write("</tr>");
                //foreach (DataRow dtrow in dt.Rows)
                //{
                //    Response.Write("<tr style='height:20px;'>");
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {

                //        if (dt.Columns[j].ColumnName == "LoanNo")
                //        {
                //            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                //        }

                //        else
                //        {
                //            Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                //        }
                //    }
                //    Response.Write("</tr>");
                //}
                //Response.Write("</table>");
                //Response.End();
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
    }
}
