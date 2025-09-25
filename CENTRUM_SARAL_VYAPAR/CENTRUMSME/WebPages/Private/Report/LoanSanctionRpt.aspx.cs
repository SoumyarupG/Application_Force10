using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Report
{
    public partial class LoanSanctionRpt : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                popDetail();
                PopState();
                //PopState();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Sanction";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuLoanSpficRpt);
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

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;

                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                if (dt.Rows.Count > 0)
                {
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "BranchName";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    CheckAll();
                }


            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }
        }
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
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
            //string s = "$('#divDel').hide();";
            //ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
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
            chkDtl.Items.Clear();
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
                        chkDtl.DataSource = dt;
                        chkDtl.DataTextField = "BranchName";
                        chkDtl.DataValueField = "BranchCode";
                        chkDtl.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Name";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    if (rblAlSel.SelectedValue == "rbAll")
                        CheckAll();
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }

        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = "";
            string vTag = "N", vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
            popDetail();
            vBrCode = ViewState["Dtl"].ToString();

            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            if (rdbOpt.SelectedValue == "rdbNA")
            {
                vTag = "N";
                vTitle = "Loan Application(New).";
            }
            else if (rdbOpt.SelectedValue == "rdbAP")
            {
                vTag = "Y";
                vTitle = "Loan Sanction.";
            }
            else if (rdbOpt.SelectedValue == "rdbAD")
            {
                vTag = "D";
                vTitle = "Pre Disbursement Approval";
            }
            else if (rdbOpt.SelectedValue == "rdbCN")
            {
                vTag = "C";
                vTitle = "Cancel Report.";
            }
            else if (rdbOpt.SelectedValue == "rdbHOAD")
            {
                vTag = "H";
                vTitle = "HO Disbursement Approval.";
            }
            else if (rdbOpt.SelectedValue == "rdbPreDisb")
            {
                vTag = "P";
                vTitle = "Pre Disbursement Approval";
            }
            dt = oRpt.rptLoanSanction(vTag, vFromDt, vToDt, vBrCode, Convert.ToInt32(Session[gblValue.BCProductId]), Convert.ToInt32(Session[gblValue.UserId]));
            //---------------------------------------Aadhar Musking----------------------------------------
            if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
            {
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (Convert.ToString(dr["Id_Type"].ToString()) == "AADHAAR")
                    {
                        dr["Idnum"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Idnum"]).Substring(Convert.ToString(dr["Idnum"]).Length - 4, 4));
                    }
                    if (Convert.ToString(dr["Id_Type2"].ToString()) == "AADHAAR")
                    {
                        dr["Idnum2"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Idnum2"]).Substring(Convert.ToString(dr["Idnum2"]).Length - 4, 4));
                    }
                    if (rdbOpt.SelectedValue == "rdbAD")
                    {
                        if (Convert.ToString(dr["Co Id Proof"].ToString()) == "AADHAAR")
                        {
                            dr["Co Id Proof No"] = String.Format("{0}{1}", "********", Convert.ToString(dr["Co Id Proof No"]).Substring(Convert.ToString(dr["Co Id Proof No"]).Length - 4, 4));
                        }
                    }

                }
            }
            //------------------------------------------------------------------------------------
            ////using (ReportDocument rptDoc = new ReportDocument())
            ////{
            ////    rptDoc.Load(vRptPath);
            ////    rptDoc.SetDataSource(dt);
            ////    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            ////    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            ////    rptDoc.SetParameterValue("pAddress2", "");
            ////    rptDoc.SetParameterValue("pBranch", vBranch);
            ////    rptDoc.SetParameterValue("pTitle", vTitle);
            ////    rptDoc.SetParameterValue("dtFrom", txtFromDt.Text);
            ////    rptDoc.SetParameterValue("dtTo", txtToDt.Text);
            ////    if (pMode == "PDF")
            ////        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Loan_Sanction_Report");
            ////    else if (pMode == "Excel")
            ////        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Loan_Sanction_Report");
            ////    rptDoc.Dispose();
            ////    Response.ClearHeaders();
            ////    Response.ClearContent();
            ////}
            if (pMode == "Excel")
            {
                string vFileNm = "attachment;filename=" + vTitle + ".xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + vTitle + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>For the Period from " + txtFromDt.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
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
                        if (dt.Columns[j].ColumnName == "MemberNo" || dt.Columns[j].ColumnName == "BranchCode" ||
                            dt.Columns[j].ColumnName == "Groupid" || dt.Columns[j].ColumnName == "Bank Account Number"
                            || dt.Columns[j].ColumnName == "Idnum2" || dt.Columns[j].ColumnName == "Idnum")
                        {
                            Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
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
            else
            {
                PrintTxt(dt, vTitle);
            }
        }

        private void PrintTxt(DataTable dt, string pFileName)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\" + pFileName + ".txt";

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

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }
        protected void btnCSV_Click(object sender, EventArgs e)
        {
            //SetParameterForRptData("CSV");
            SetParameterForRptData("CSV");
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
