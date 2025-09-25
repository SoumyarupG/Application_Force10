using System;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using ClosedXML.Excel;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.HOReports
{
    public partial class RptHOInsurance : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch(Session[gblValue.UserName].ToString());
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Consolidated Insurance Data";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOInsuranceRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Consolidated Insurance Data", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //GetData("Excell");
            GetData("Excel");
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
        private void GetData_Old(string pFormat)
        {
            string vBranch = "", vBrCode = "", vFileNm = "", vStr = "";
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                vBrCode = ViewState["BrCode"].ToString();
                oRpt = new CReports();
                if (rbInsuranceParty.SelectedValue == "rbBhartiAXA")
                {
                    dt = oRpt.rptInsuranceReportBhartiAXA(vFromDt, vToDt, vBrCode, Convert.ToInt32(Session[gblValue.BCProductId]));

                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        var ws = wb.Worksheets.Add(dt, "BHARTI AXA");

                        ws.Cell(1, 1).Value = "Insurance Report Date From: " + vFromDt.ToString("dd/MM/yyyy") + "    Date To: " + vToDt.ToString("dd/MM/yyyy");
                        ws.Cell(1, 1).Style.Font.FontSize = 12;
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                        ws.Cell(2, 1).InsertTable(dt);
                        ws.SheetView.FreezeRows(4); //freeze rows
                        //ws.Columns().AdjustToContents();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        vFileNm = "attachment;filename=" + vFromDt.ToString("yyyyMMdd") + "_BHARTIAXA_Insurance_Report.xlsx";
                        Response.AddHeader("content-disposition", vFileNm);
                        // Response.ContentType = "application/vnd.ms-excel.sheet.binary.macroeEnabled.12";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                        wb.Dispose();
                    }
                }
                if (rbInsuranceParty.SelectedValue == "rbKotak")
                {
                    dt = oRpt.rptInsuranceReportKotak(vFromDt, vToDt, vBrCode);

                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        var ws = wb.Worksheets.Add(dt, "KOTAK");

                        ws.Cell(1, 1).Value = "Insurance Report Date From: " + vFromDt.ToString("dd/MM/yyyy") + "    Date To: " + vToDt.ToString("dd/MM/yyyy");
                        ws.Cell(1, 1).Style.Font.FontSize = 12;
                        ws.Cell(1, 1).Style.Font.Bold = true;
                        ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                        ws.Cell(2, 1).InsertTable(dt);
                        ws.SheetView.FreezeRows(4); //freeze rows
                        //ws.Columns().AdjustToContents();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        vFileNm = "attachment;filename=" + vFromDt.ToString("yyyyMMdd") + "_KOTAK_Insurance_Report.xlsx";
                        Response.AddHeader("content-disposition", vFileNm);
                        // Response.ContentType = "application/vnd.ms-excel.sheet.binary.macroeEnabled.12";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                        wb.Dispose();
                    }
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        private void GetData(string pFormat)
        {
            string vBranch = "", vBrCode = "", vFileNm = "", vStr = "";
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                vBrCode = ViewState["BrCode"].ToString();
                oRpt = new CReports();
                dt = oRpt.rptInsuranceReport(vFromDt, vToDt, vBrCode, Convert.ToInt32(Session[gblValue.BCProductId]));
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dt, "Insurance");
                    ws.Cell(1, 1).Value = "Insurance Report Date From: " + vFromDt.ToString("dd/MM/yyyy") + "    Date To: " + vToDt.ToString("dd/MM/yyyy");
                    ws.Cell(1, 1).Style.Font.FontSize = 12;
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                    ws.Cell(2, 1).InsertTable(dt);
                    ws.Table("Table1").ShowAutoFilter = false; //remove default filter
                    ws.SheetView.FreezeRows(4); //freeze rows                                       
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    vFileNm = "attachment;filename=" + vFromDt.ToString("yyyyMMdd") + "_Insurance_Report.xlsx";
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    wb.Dispose();
                }                
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["BrCode"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {
                    chkBrDtl.DataSource = dt;
                    chkBrDtl.DataTextField = "BranchName";
                    chkBrDtl.DataValueField = "BranchCode";
                    chkBrDtl.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private void CheckAll()
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
                        strin = chkBrDtl.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
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
            CheckAll();
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
    }
}
