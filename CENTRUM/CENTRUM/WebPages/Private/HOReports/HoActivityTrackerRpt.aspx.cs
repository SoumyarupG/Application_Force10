using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Web.UI;
using System.Web;
using ClosedXML.Excel;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HoActivityTrackerRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                InitBasePage();
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopState();
                PopBranch(Session[gblValue.UserName].ToString());
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Activity Tracker Report";
                this.ShowBranchName = Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuActivityTrackerRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Activity Tracker Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void GetEoByBranch()
        {
            CEO oGb = null;
            DataTable dt = null;
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oGb = new CEO();
                dt = oGb.PopRO(vBranch, "0", "0", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), this.UserID);
                chkEo.DataSource = dt;
                chkEo.DataTextField = "EOName";
                chkEo.DataValueField = "EOId";
                chkEo.DataBind();
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void GetData(string pFormat)
        {
            string vBranch = "", vFileNm = "", vType = "";
            DateTime vFinFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vFinToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                GetSelectedID();
                string vBranchCode = Convert.ToString(ViewState["SelectedID"]);
                vType = rblAlType.SelectedValue;
                oRpt = new CReports();
                dt = oRpt.RptActivityTracker(vBranchCode, vFinFrmDt, vFinToDt,"D");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dt, "Activity Tracker Report");

                    ws.Cell(1, 1).Value = gblValue.CompName;
                    ws.Cell(1, 1).Style.Font.FontSize = 14;
                    ws.Cell(1, 1).Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(1, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();
                    ws.Cell(2, 1).Value = CGblIdGenerator.GetBranchAddress1("0000");
                    ws.Cell(2, 1).Style.Font.FontSize = 14;
                    ws.Cell(2, 1).Style.Font.Bold = true;
                    ws.Cell(2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(2, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();
                    ws.Cell(3, 1).Value = CGblIdGenerator.GetBranchAddress2("0000");
                    ws.Cell(3, 1).Style.Font.FontSize = 14;
                    ws.Cell(3, 1).Style.Font.Bold = true;
                    ws.Cell(3, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(3, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                    ws.Cell(4, 1).Value = "Activity Tracker Report";
                    ws.Cell(4, 1).Style.Font.FontSize = 14;
                    ws.Cell(4, 1).Style.Font.Bold = true;
                    ws.Cell(4, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(4, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                    ws.Cell(5, 1).Value = "Date From: " + vFinFrmDt.ToString("dd/MM/yyyy") + "    Date To: " + vFinToDt.ToString("dd/MM/yyyy");
                    ws.Cell(5, 1).Style.Font.FontSize = 12;
                    ws.Cell(5, 1).Style.Font.Bold = true;
                    ws.Cell(5, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Range(ws.Cell(5, 1).Address, ws.Cell(1, dt.Columns.Count).Address).Row(1).Merge();

                    ws.Cell(6, 1).InsertTable(dt);
                    ws.SheetView.FreezeRows(6); //freeze rows
                    //ws.Columns().AdjustToContents();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    vFileNm = "attachment;filename=" + vFinFrmDt.ToString("yyyyMMdd") + "_Activity_Tracker_Report.xlsx";
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
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {

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

        protected void chkEo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkEo.Items.Count; vRow++)
            {
                if (chkEo.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkEo.Items[vRow].Value;
                    else
                        strin = strin + "," + chkEo.Items[vRow].Value + "";
                }
            }
            ViewState["EOID"] = strin;
        }

        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        protected void rblEO_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllEo();
        }

        private void CheckAllEo()
        {
            Int32 vRow;
            string strin = "";
            if (rblEO.SelectedValue == "rdbAllEo")
            {
                chkEo.Enabled = false;
                for (vRow = 0; vRow < chkEo.Items.Count; vRow++)
                {
                    chkEo.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkEo.Items[vRow].Value;
                    else
                        strin = strin + "," + chkEo.Items[vRow].Value + "";
                }
                ViewState["EOID"] = strin;
            }
            else if (rblEO.SelectedValue == "rdbSelctEo")
            {
                ViewState["EOID"] = null;
                chkEo.Enabled = true;
                for (vRow = 0; vRow < chkEo.Items.Count; vRow++)
                    chkEo.Items[vRow].Selected = false;
            }
        }

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rbSelect.SelectedValue == "A")
            {
                chkFilterList.Enabled = false;
                for (vRow = 0; vRow < chkFilterList.Items.Count; vRow++)
                {
                    chkFilterList.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkFilterList.Items[vRow].Value;
                    else
                        strin = strin + "," + chkFilterList.Items[vRow].Value + "";
                }
                ViewState["SelectedID"] = strin;
            }
            else if (rbSelect.SelectedValue == "S")
            {
                ViewState["SelectedID"] = null;
                chkFilterList.Enabled = true;
                for (vRow = 0; vRow < chkFilterList.Items.Count; vRow++)
                    chkFilterList.Items[vRow].Selected = false;
            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    dt.DefaultView.RowFilter = "BranchCode ='" + Convert.ToString(Session[gblValue.BrnchCode]) + "'";
                }
                chkFilterList.DataSource = dt;
                chkFilterList.DataTextField = "BranchName";
                chkFilterList.DataValueField = "BranchCode";
                chkFilterList.DataBind();
                CheckAll();
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        //protected void rblCGTGRT_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rblCGTGRT.SelectedValue == "Y")
        //    {
        //        btnExcl.Visible = true;
        //        btnPdf.Visible = false;
        //    }
        //    else
        //    {
        //        btnPdf.Visible = true;
        //        btnExcl.Visible = false;
        //    }
        //}

        protected void txtFrmDt_TextChanged(object sender, EventArgs e)
        {
            PopBranch();
        }

        protected void chkState_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetailByCategory("STATE");
            PopBranch();
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllCategory("STATE");
            PopBranch();
        }

        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();              
            string vSTATE = Convert.ToString(ViewState["STATE"]);
            string pUser = Session[gblValue.UserName].ToString();
            try
            {
                dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), vSTATE);
                chkFilterList.DataSource = dt;
                chkFilterList.DataTextField = "BranchName";
                chkFilterList.DataValueField = "BranchCode";
                chkFilterList.DataBind();
                CheckAll();
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private void CheckAllCategory(string vCategory)
        {
            Int32 vRow;
            string strin = "";
            if (vCategory == "STATE")
            {
                if (ddlState.SelectedValue == "C")
                {
                    chkState.Enabled = false;
                    for (vRow = 0; vRow < chkState.Items.Count; vRow++)
                    {
                        chkState.Items[vRow].Selected = true;
                        if (strin == "")
                            strin = chkState.Items[vRow].Value;
                        else
                            strin = strin + "," + chkState.Items[vRow].Value + "";
                    }
                    ViewState["STATE"] = strin;
                }
                else if (ddlState.SelectedValue == "B")
                {
                    ViewState["STATE"] = null;
                    chkState.Enabled = true;
                    for (vRow = 0; vRow < chkState.Items.Count; vRow++)
                        chkState.Items[vRow].Selected = false;
                }
            }


        }
        private void PopState()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", vBrCode, "N", "AA", vLogDt, "0000");
            chkState.DataSource = dt;
            chkState.DataTextField = "StateName";
            chkState.DataValueField = "StateId";
            chkState.DataBind();
            CheckAllCategory("STATE");
        }



        private void popDetailByCategory(string vCategory)
        {
            string str = "";

            if (vCategory == "STATE")
            {
                ViewState["STATE"] = null;
                for (int vRow = 0; vRow < chkState.Items.Count; vRow++)
                {
                    if (chkState.Items[vRow].Selected == true)
                    {
                        if (str == "")
                            str = chkState.Items[vRow].Value;
                        else if (str != "")
                            str = str + "," + chkState.Items[vRow].Value;
                    }
                }
                if (str == "")
                    ViewState["STATE"] = 0;
                else
                    ViewState["STATE"] = str;
            }

        }

        protected void rbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChkFilterList();
            GetSelectedID();
        }
        private void ChkFilterList()
        {
            Int32 vRow;
            if (rbSelect.SelectedValue == "A")
            {
                for (vRow = 0; vRow < chkFilterList.Items.Count; vRow++)
                    chkFilterList.Items[vRow].Selected = true;
                chkFilterList.Enabled = false;
            }
            else if (rbSelect.SelectedValue == "S")
            {
                for (vRow = 0; vRow < chkFilterList.Items.Count; vRow++)
                    chkFilterList.Items[vRow].Selected = false;
                chkFilterList.Enabled = true;
            }
        }

        private void GetSelectedID()
        {
            ViewState["SelectedID"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkFilterList.Items.Count; vRow++)
            {
                if (chkFilterList.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkFilterList.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkFilterList.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["SelectedID"] = "";
            else
                ViewState["SelectedID"] = str;
        }
    }
}