using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptAdvanceReschedule : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
                PopState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Advance Reschedule Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAdvanceRescheduleReport);
                if (this.UserID == 1) return;
                if (this.CanView == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Advance Reschedule Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["Dtl"] = null;
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
            ViewState["Dtl"] = strin;
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
                ViewState["Dtl"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["Dtl"] = null;
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
            ViewState["Dtl"] = strin;
        }

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtDtFrm.Text.Trim());
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
             SetParameterForRptData("Excel");
        }

        protected void btnCSV_Click(object sender, EventArgs e)
        {
            TimeSpan t = gblFuction.setDate(txtToDt.Text.Trim()) - gblFuction.setDate(txtDtFrm.Text.Trim());
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }
            SetParameterForRptData("CSV");
        }

        private void SetParameterForRptData(string pMode)
        {
            DataTable dt = null;
            CReports oRpt = new CReports();            
            string vFileNm = "", vBrCode="";
            vBrCode = ViewState["Dtl"].ToString();
            try
            {
                oRpt = new CReports();
                dt = oRpt.RptAdvanceReschedule(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtToDt.Text), vBrCode);
                if (dt.Rows.Count > 0)
                {
                    if (pMode == "Excel")
                    {
                        vFileNm = "attachment;filename=Advance_Reschedule_Report_" + DateTime.Now + ".xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='5' widht='120%'>");
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
                                Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
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
                else
                {
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\Advance_Reschedule_Report.csv";

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

        //private void GenerateReport(string pApiName, string pRequestdata)
        //{
        //    string vMsg = "";
        //    CApiCalling oAPI = new CApiCalling();
        //    try
        //    {
        //        vMsg = oAPI.GenerateReport(pApiName, pRequestdata, vReportUrl);
        //    }
        //    finally
        //    {
        //        gblFuction.AjxMsgPopup(vMsg);
        //        btnCSV.Enabled = false;
        //        btnExcl.Enabled = false;
        //    }
        //}
    }
}