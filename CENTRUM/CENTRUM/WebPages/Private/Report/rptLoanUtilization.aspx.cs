using System;
using FORCECA;
using System.Data;
using FORCEBA;
using System.Web;
using System.IO;
using System.Net;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptLoanUtilization : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                rblAlSel.Items.FindByValue("rbAll").Selected = true;
                PopBranch();
                PopState();
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
            }
        }

        #region "Populate Functions"
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Utilization Check Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanUtilRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PAR Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
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
        private void SetRptData(string pMode)
        {
            DataTable dt = null;
            CReports oRpt = null;
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            try
            {
                TimeSpan t = vToDt - vFromDt;
                if (t.TotalDays > 2)
                {
                    gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                    return;
                }

                oRpt = new CReports();
                dt = oRpt.rptLUC(vFromDt, vToDt, vBrCode);
                if (pMode == "Excel")
                {
                    string vFileNm = "attachment;filename=" + txtFromDt.Text + "_LoanUtilization.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> PAR </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>From - " + txtFromDt.Text + " To - " + txtToDt.Text + "</font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                    string tab = string.Empty;
                    Response.Write("<tr>");
                    foreach (DataColumn dtcol in dt.Columns)
                    {
                        Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                        //String ColumnHeader = "";
                        //switch (dtcol.ColumnName)
                        //{
                        //    case "BranchId":
                        //        ColumnHeader = "Branch Id";
                        //        break;
                        //    case "BranchName":
                        //        ColumnHeader = "Branch Name";
                        //        break;
                        //    case "GroupId":
                        //        ColumnHeader = "Group Id";
                        //        break;
                        //    case "GroupName":
                        //        ColumnHeader = "Group Name";
                        //        break;
                        //    case "AccountIDOld":
                        //        ColumnHeader = "Account ID Old";
                        //        break;
                        //    case "AccountIDNew":
                        //        ColumnHeader = "Account ID New";
                        //        break;
                        //    case "AccountName":
                        //        ColumnHeader = "Account Name";
                        //        break;
                        //    case "DisbursementDate":
                        //        ColumnHeader = "Disbursement Date";
                        //        break;
                        //    case "DisbursedAmount":
                        //        ColumnHeader = "Disbursed Amount";
                        //        break;
                        //    case "LoanPurpose":
                        //        ColumnHeader = "Loan Purpose";
                        //        break;
                        //    case "LoanSubPurpose":
                        //        ColumnHeader = "Loan Sub Purpose";
                        //        break;
                        //    case "LUCDonevia":
                        //        ColumnHeader = "LUC Done via";
                        //        break;
                        //    case "UtilizationStatus":
                        //        ColumnHeader = "Utilization Status";
                        //        break;
                        //    case "VerificationDate":
                        //        ColumnHeader = "Verification Date";
                        //        break;
                        //    case "VerifiedBy":
                        //        ColumnHeader = "Verified By";
                        //        break;
                        //    case "UtilizationAmount":
                        //        ColumnHeader = "Utilization Amount";
                        //        break;
                        //    case "UtilizationType":
                        //        ColumnHeader = "Utilization Type";
                        //        break;
                        //    case "UtilizationRemarks":
                        //        ColumnHeader = "Utilization Remarks";
                        //        break;
                        //    case "IsSamePurpose":
                        //        ColumnHeader = "Is Same Purpose?";
                        //        break;
                        //}
                        //Response.Write("<td><b>" + (dtcol.ColumnName != ColumnHeader ? ColumnHeader : dtcol.ColumnName) + "<b></td>");
                    }
                    Response.Write("</tr>");
                    foreach (DataRow dtrow in dt.Rows)
                    {
                        Response.Write("<tr style='height:20px;'>");
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Columns[j].ColumnName == "BranchCode")
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
                    PrintTxt(dt);
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
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
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
        }
        #endregion

        #region "Click Operation"
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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            SetRptData("CSV");
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
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\LoanUtilizationReport_UNITY_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

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
                //CSV(dt, vFolderPath, vFileNm);
                Write(dt, vFileNm);
                downloadfile(vFileNm);
                // downloadfile(vFileNm, "C:\\LoanUtilizationReport_Centrum.txt", "New");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            finally
            {
                gblFuction.MsgPopup("Done");
                btnExit.Enabled = true;
                File.Delete(vFileNm);

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
            if (File.Exists(filename))
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ContentType = "text/plain";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filename));
                Response.WriteFile(filename);
                Response.End();
                //byte[] vDoc = null;              
                //vDoc = File.ReadAllBytes(filename);
                //Response.AddHeader("Content-Type", "Application/octet-stream");
                //Response.AddHeader("Content-Disposition", "attachment;   filename="+Path.GetFileName(filename));
                //Response.BinaryWrite(vDoc);
                //Response.Flush();
                //Response.End();
                File.Delete(filename);
            }
            else
            {
                gblFuction.AjxMsgPopup("File could not be found");
            }
        }

        private void CSV(DataTable dt, string pFolderPath, string pFileName)
        {
            string vFileNm = "";
            vFileNm = pFolderPath + "/" + pFileName + "_" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".csv";
            try
            {
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
                            if (value.Contains(","))
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

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        private void downloadfile(String pPath, string filename, string vMode)
        {

            // check to see that the file exists 
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(pPath, filename);
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
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
            DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
            chkBrDtl.Items.Clear();
            ViewState["Id"] = null;
            try
            {
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
    }
}