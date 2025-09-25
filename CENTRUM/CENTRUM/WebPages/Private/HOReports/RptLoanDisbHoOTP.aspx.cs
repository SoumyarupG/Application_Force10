using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Text;
using System.Linq;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptLoanDisbHoOTP : CENTRUMBase
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
                PopBranch();
                PopState();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "OTP Based Loan Disbursement";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOLnDisbOTP);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "OTP Based Loan Disbursement", false);
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
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oRO = new CEO();
                //dt = oCM.GetCOPop(vBrCode, "SCO,CO,TCO,JTCO,GO");
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "EoID";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanType", "LoanTypeMst", 0, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanType";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbFund")
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
                oDic.Add("Cycle 11", 11);
                oDic.Add("Cycle 12", 12);
                oDic.Add("Cycle 13", 13);
                oDic.Add("Cycle 14", 14);
                oDic.Add("Cycle 15", 15);
                oDic.Add("Cycle 16", 16);
                oDic.Add("Cycle 17", 17);
                oDic.Add("Cycle 18", 18);
                oDic.Add("Cycle 19", 19);
                oDic.Add("Cycle 20", 20);
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
                //chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                //chkDtl.Enabled = true;
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
            string vBrCode = ViewState["BrCode"].ToString();
            string vRptPath = "", vTypeId = "", vType = "";
            string vBranch = Session[gblValue.BrName].ToString();
            int vNEFT = 0;           
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            vTypeId = ViewState["Dtl"].ToString();

            if (chkNEFT.Checked == true) vNEFT = 1;

            if (rblSel.SelectedValue == "rbEO") vType = "E";
            if (rblSel.SelectedValue == "rbLType") vType = "L";
            if (rblSel.SelectedValue == "rbFund") vType = "F";
            if (rblSel.SelectedValue == "rdbPurps") vType = "P";
            if (rblSel.SelectedValue == "rbWhole") vType = "A";
            if (rblSel.SelectedValue == "rdbCycle") vType = "C";

            TimeSpan t = vToDt - vFromDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            using (ReportDocument rptDoc = new ReportDocument())
            {
                if (pMode == "PDF")
                {
                    if (rbOpt.SelectedValue == "rbDtl")
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbDtl.rpt";
                    if (rbOpt.SelectedValue == "rbSum")
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbSum.rpt";


                    dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);

                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("0000"));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pType", vType);
                    rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                    rptDoc.SetParameterValue("dtTo", txtToDt.Text);

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Consolidate_Loan_Disbursement_List_" + txtToDt.Text.Replace("/", "_"));

                }
                else if (pMode == "Excel")
                {
                    if (rbOpt.SelectedValue == "rbSum")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbSum.rpt";
                        rptDoc.Load(vRptPath);

                        dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                        rptDoc.SetParameterValue("pAddress2", "");
                        rptDoc.SetParameterValue("pBranch", vBranch);
                        rptDoc.SetParameterValue("pType", vType);
                        rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                        rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Consolidate_Loan_Disbursement_List_" + txtToDt.Text.Replace("/", "_") + ".xls");

                    }
                    else
                    {

                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                        dt = oRpt.rptLoanDisbXls_OTP(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
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
                else if (pMode == "CSV")
                {
                    if (rbOpt.SelectedValue == "rbSum")
                    {
                        dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
                        PrintTxt(dt);
                    }
                    else
                    {
                        dt = oRpt.rptLoanDisbXls_OTP(vFromDt, vToDt, vBrCode, vType, vTypeId, vNEFT);
                        PrintTxt(dt);
                    }
                }
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }

            //rptDoc.Dispose();
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

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("CSV");
        }

        //private void PrintTxt(DataTable dt)
        //{
        //    string vFolderPath = "C:\\BijliReport";
        //    string vFileNm = "";
        //    vFileNm = vFolderPath + "\\LoanDisbursementReport_Centrum.txt";

        //    try
        //    {
        //        if (System.IO.Directory.Exists(vFolderPath))
        //        {
        //            foreach (var file in Directory.GetFiles(vFolderPath))
        //            {
        //                if (File.Exists(vFileNm) == true)
        //                    File.Delete(vFileNm);
        //            }
        //        }
        //        else
        //        {
        //            Directory.CreateDirectory(vFolderPath);
        //        }
        //        Write(dt, vFileNm);
        //        downloadfile(vFileNm);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        gblFuction.MsgPopup("Done");
        //        btnExit.Enabled = true;

        //    }
        //}       

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\LoanDisbursementReport_Centrum.csv";

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
                StreamWriter sw = new StreamWriter(vFileNm, false);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                // Write(dt, vFileNm);
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

            if (rblBr.SelectedValue == "rbAll")
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
            else if (rblBr.SelectedValue == "rbSel")
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
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","));
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

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblBr.SelectedValue == "rbAll")
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
            else if (rblBr.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }

        protected void rblBr_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}