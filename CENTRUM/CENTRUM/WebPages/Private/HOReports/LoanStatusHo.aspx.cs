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
using ClosedXML.Excel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Configuration;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class LoanStatusHo : CENTRUMBase
    {
        private static string vSrvName = ConfigurationManager.AppSettings["SrvName"];
        private static string vDBName = ConfigurationManager.AppSettings["DBName"];
        private static string vPw = ConfigurationManager.AppSettings["PassPW"];
        private static string vReportUrl = ConfigurationManager.AppSettings["ReportUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["ID"] = null;
                PopList();
                PopBranch();
                PopState();
            }
        }


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
                this.GetModuleByRole(mnuID.mnuHOLnSts);
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
            Int32 vRow;
            string strin = "";
            ViewState["ID"] = null;
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oRo = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rdbOpt.SelectedValue == "rdbCo")
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oRo = new CEO();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                dt = oRo.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "EoID";
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

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rdbSel.SelectedValue == "rdbAll")
            {
                //chkDtl.Enabled = false;
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
        private void SetParameterForRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            string vWithDef = "", vWithPDDDt = "";
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
            string strID = ViewState["ID"].ToString(), vMode = "A", vRptPath = "", vTitle = "", vType = "D";
            DataTable dt = null;
            CReports oRpt = new CReports();
            //ReportDocument rptDoc = new ReportDocument();
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

            if (chkSp.Checked == false)
            {
                if (chkWoff.Checked == true)
                {
                    dt = oRpt.rptWriteOffLoanStatus(rblAlSel.SelectedValue, vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                }
                else
                {
                    dt = oRpt.rptLoanStatus(rblAlSel.SelectedValue, vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
                }
            }
            else
            {
                dt = oRpt.rptAllPool("L");
            }

            if (vType == "D")
            {

                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatus.rpt";
            }
            else
            {
                vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanStatusSummary.rpt";
            }
            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2("0000"));
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("AsOnDt", txtAsDt.Text);
                if (chkWoff.Checked == true)
                {
                    vTitle = "Write Off " + vTitle;
                    rptDoc.SetParameterValue("pTitle", vTitle);
                }
                else
                    rptDoc.SetParameterValue("pTitle", vTitle);
                rptDoc.SetParameterValue("pType", vType);

                if (pMode == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan Status Report");
                else if (pMode == "Excel")
                {
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan Status Report");

                }

                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void Export(string vCalFrom)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            string vWithDef = "", vWithPDDDt = "", vMsg = "";
            DateTime vAsOn = gblFuction.setDate(txtAsDt.Text);
            string strID = ViewState["ID"].ToString(), vMode = "A", vWOff = "";
            DataTable dt = null;
            CReports oRpt = new CReports();
            ReportDocument rptDoc = new ReportDocument();
            CApiCalling oAPI = new CApiCalling();
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
            else if (rdbOpt.SelectedValue == "rdbProduct")
            {
                vMode = "R";
            }
            if (chkOnlyDef.Checked == true)
                vWithDef = "Y";
            else
                vWithDef = "N";

            if (chkWithPddDt.Checked == true)
                vWithPDDDt = "Y";
            else
                vWithPDDDt = "N";

            //if (chkWoff.Checked == true)
            //    dt = oRpt.rptWriteOffLoanStatusXlsx("", vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);
            //else
            //    dt = oRpt.rptLoanStatusXlsx("", vAsOn, strID, vBrCode, vMode, vWithDef, vWithPDDDt);

            var req = new LoanStatusReportRequest()
                   {
                       prbAll = "",
                       pAsOnDt = txtAsDt.Text,
                       pID = strID,
                       pBrCode = vBrCode,
                       pMode = vMode,
                       pWithDef = vWithDef,
                       pWithPDDDt = vWithPDDDt,
                       pFormat = vCalFrom == "E" ? "Excel" : "CSV",
                       pUserId = Convert.ToString(Session[gblValue.UserId]),
                       pDBName = vDBName,
                       pPassword = vPw,
                       pServerIP = vSrvName
                   };

            string Requestdata = JsonConvert.SerializeObject(req);


            //System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            if (chkWoff.Checked == true)
                vMsg = oAPI.GenerateReport("GenerateWriteOffLoanStatus", Requestdata, vReportUrl);
            else
                vMsg = oAPI.GenerateReport("GenerateLoanStatus", Requestdata, vReportUrl);

            gblFuction.AjxMsgPopup(vMsg);
            btnCSV.Enabled = false;
            btnExcl.Enabled = false;
            //if (vCalFrom == "E")
            //{

            //    if (chkWoff.Checked == true)
            //        GenerateReport("GenerateWriteOffLoanStatus", Requestdata);
            //    else
            //        GenerateReport("GenerateLoanStatus", Requestdata);


            //    //using (XLWorkbook wb = new XLWorkbook())
            //    //{
            //    //    var ws = wb.Worksheets.Add(dt, "LoanStatus");
            //    //    ws.Cell(1, 1).Value = gblValue.CompName;
            //    //    ws.Cell(1, 1).Style.Font.Bold = true;
            //    //    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //    //    ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();
            //    //    ws.Cell(2, 1).Value = CGblIdGenerator.GetBranchAddress1(vBrCode);
            //    //    ws.Cell(2, 1).Style.Font.Bold = true;
            //    //    ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //    //    ws.Range(ws.Cell(2, 1).Address, ws.Cell(2, dt.Columns.Count).Address).Row(1).Merge();
            //    //    ws.Cell(3, 1).Value = "As On : " + txtAsDt.Text;
            //    //    ws.Cell(3, 1).Style.Font.Bold = true;
            //    //    ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            //    //    ws.Range(ws.Cell(3, 1).Address, ws.Cell(3, dt.Columns.Count).Address).Row(1).Merge();

            //    //    ws.Cell(4, 1).InsertTable(dt);
            //    //    ws.SheetView.FreezeRows(3); //freeze rows
            //    //    ws.Columns().AdjustToContents();

            //    //    Response.Clear();
            //    //    Response.Buffer = true;
            //    //    Response.Charset = "";
            //    //    string vFileNm = "";
            //    //    vFileNm = "attachment;filename=Loan_Status.xlsx";
            //    //    Response.AddHeader("content-disposition", vFileNm);
            //    //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    //    {
            //    //        wb.SaveAs(MyMemoryStream);
            //    //        MyMemoryStream.WriteTo(Response.OutputStream);
            //    //        Response.Flush();
            //    //        Response.End();
            //    //    }
            //    //}


            //    //    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            //    //    DataGrid1.DataSource = dt;
            //    //    DataGrid1.DataBind();
            //    //    tdx.Controls.Add(DataGrid1);
            //    //    tdx.Visible = false;
            //    //    string vFileNm = "attachment;filename=Loan_Status.xls";
            //    //    StringWriter sw = new StringWriter();
            //    //    HtmlTextWriter htw = new HtmlTextWriter(sw);
            //    //    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            //    //    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            //    //    htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></u></b></td></tr>");
            //    //    if (chkWoff.Checked == true)
            //    //        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>Write Off Loan Status Report As On " + txtAsDt.Text + "</font></u></b></td></tr>");
            //    //    else
            //    //        htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>Loan Status Report As On " + txtAsDt.Text + "</font></u></b></td></tr>");
            //    //    DataGrid1.RenderControl(htw);
            //    //    htw.WriteLine("</td></tr>");
            //    //    htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            //    //    htw.WriteLine("</table>");

            //    //    Response.ClearContent();
            //    //    Response.AddHeader("content-disposition", vFileNm);
            //    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //    //    Response.ContentType = "application/vnd.ms-excel";
            //    //    Response.Write(sw.ToString());
            //    //    Response.End();
            //    //}
            //    //else
            //    //{
            //    //    PrintTxt(dt);
            //    //}

            //}
        }


        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\LoanStatusHO_UFSB.txt";

            try
            {
                if (System.IO.Directory.Exists(vFolderPath))
                {
                    foreach (var file in Directory.GetFiles(vFolderPath))
                    {
                        if (File.Exists(vFileNm) == true)
                            File.Delete(vFileNm);
                    }
                }
                else
                {
                    Directory.CreateDirectory(vFolderPath);
                }
                Write(dt, vFileNm);
                downloadfile(vFileNm);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="outputFilePath"></param>
        private void Write(DataTable dt, string outputFilePath)
        {
            int[] maxLengths = new int[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                maxLengths[i] = dt.Columns[i].ColumnName.Length;
                foreach (DataRow row in dt.Rows)
                {
                    if (!row.IsNull(i))
                    {
                        int length = row[i].ToString().Length;
                        if (length > maxLengths[i])
                        {
                            maxLengths[i] = length;
                        }
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter(outputFilePath, false))
            {

                for (int i = 0; i < dt.Columns.Count; i++)
                {

                    sw.Write(dt.Columns[i].ColumnName.ToString().Trim() + '|');

                }
                sw.WriteLine();

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            sw.Write(row[i].ToString().Trim() + '|');
                        }

                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        private void downloadfile(string filename)
        {

            // check to see that the file exists 
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.WriteFile(filename);
                Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
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
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblAlSel.SelectedValue == "rbAll")
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
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
            ddlState.DataSource = dt;
            ddlState.DataTextField = "StateName";
            ddlState.DataValueField = "StateId";
            ddlState.DataBind();
        }

        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            chkBrDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtAsDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","), ddlBrType.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        chkBrDtl.DataSource = dt;
                        chkBrDtl.DataTextField = "BranchName";
                        chkBrDtl.DataValueField = "BranchCode";
                        chkBrDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckBrAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "Name";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckBrAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
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
                    {
                        strin = chkDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                    }
                }
            }
            //ViewState["BrCode"] = strin;
            ViewState["ID"] = strin;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCSV_Click(object sender, EventArgs e)
        {
            if (rdbSummary.SelectedValue == "rdbDtls")
            {
                //Export("C");
                Int32 vLoanStatusHO = Convert.ToInt32(Session[gblValue.LoanStatusHO].ToString());
                if (vLoanStatusHO != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vLoanStatusHO > 300)
                    {
                        Session[gblValue.LoanStatusHO] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        Export("C");
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.LoanStatusHO] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    Export("C");

                }
            }
            else
            {
                gblFuction.MsgPopup("Only Details Option");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (rdbSummary.SelectedValue == "rdbDtls")
            {
                //Export("E");
                Int32 vLoanStatusHO = Convert.ToInt32(Session[gblValue.LoanStatusHO].ToString());
                if (vLoanStatusHO != 0)
                {
                    Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if (unixTicks - vLoanStatusHO > 300)
                    {
                        Session[gblValue.LoanStatusHO] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        Export("E");
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Already Report Generate Request Is Executing ...Please Wait For 5 Mins..And Re Generate..");
                    }
                }
                else
                {
                    Session[gblValue.LoanStatusHO] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                    Export("E");

                }
            }
            else
            {
                SetParameterForRptData("Excel");
            }

        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
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
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
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

        //private void GenerateReport(string pApiName, string pRequestdata)
        //{
        //    try
        //    {
        //        string Requestdata = pRequestdata;
        //        string postURL = vReportUrl + "/" + pApiName;
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Timeout = 2000;
        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        byte[] data = Encoding.UTF8.GetBytes(Requestdata);
        //        request.ContentLength = data.Length;
        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(data, 0, data.Length);
        //        requestStream.Close();
        //    }
        //    finally
        //    {
        //        gblFuction.AjxMsgPopup("Please comeback after 5 min.");
        //    }
        //}
    }


    [DataContract]
    public class LoanStatusReportRequest
    {
        [DataMember]
        public string prbAll { get; set; }
        [DataMember]
        public string pAsOnDt { get; set; }
        [DataMember]
        public string pID { get; set; }
        [DataMember]
        public string pBrCode { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string pWithDef { get; set; }
        [DataMember]
        public string pWithPDDDt { get; set; }
        [DataMember]
        public string pFormat { get; set; }
        [DataMember]
        public string pUserId { get; set; }
        [DataMember]
        public string pDBName { get; set; }
        [DataMember]
        public string pServerIP { get; set; }
        [DataMember]
        public string pPassword { get; set; }
        [DataMember]
        public string pCompanyName { get; set; }
        [DataMember]
        public string pCompanyAddress { get; set; }
    }
}
