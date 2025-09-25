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

namespace CENTRUM_SARALVYAPAR.WebPages.Private.HOReports
{
    public partial class RptHindSighting : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                // PopList();
                // popDetail();
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                PopState();
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Hind Sighting";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                //this.GetModuleByRole(mnuID.mnuHOCleintProfile);
                this.GetModuleByRole(mnuID.mnuRptHindSighting);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Hind Sighting", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            //dt = oUsr.GetBranch1(Session[gblValue.BrnchCode].ToString());
            ViewState["ID"] = null;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
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

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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
        protected void btnCSV_Click(object sender, EventArgs e)
        {
            GetData("CSV");
        }
        protected void btnStateWise_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            string pUser = Session[gblValue.UserName].ToString();
            DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
            chkBr.Items.Clear();
            ViewState["Id"] = null;
            try
            {
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValues.Replace("|", ","));
                    if (dt.Rows.Count > 0)
                    {
                        chkBr.DataSource = dt;
                        chkBr.DataTextField = "BranchName";
                        chkBr.DataValueField = "BranchCode";
                        chkBr.DataBind();
                        if (rblAlSel.SelectedValue == "rbAll")
                            CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "Name";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //if (ValidateDate() == false)
            //{
            //    gblFuction.MsgPopup("Please Set The Valid Date Range.");
            //    this.Page.SetFocus(txtFrmDt);
            //    return;
            //}
            //else
            //{
            GetData("Excel");
            //}
        }
        private void GetData(string pMode)
        {
            GetBranch();
            string vTitleType = "";
            string vFileNm = "";
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DateTime vFinStDt = Convert.ToDateTime(gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()));
            DateTime vFinToDt = Convert.ToDateTime(gblFuction.setStrDate(Session[gblValue.FinToDt].ToString()));
            string vBrCode = "";
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;

            if (rblAlSel.SelectedValue == "rbAll")
                vBrCode = Convert.ToString(ViewState["ID"]);
            else
            {
                Int32 vRow;
                string strin = "";

                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    if (chkBr.Items[vRow].Selected == true)
                    {
                        if (strin == "")
                            strin = chkBr.Items[vRow].Value;
                        else
                            strin = strin + "," + chkBr.Items[vRow].Value + "";
                    }
                }
                vBrCode = strin;
            }
            try
            {
                    oRpt = new CReports();
                    dt = oRpt.RptHindSighting(vFromDt, vToDt, vBrCode);
                    if (pMode == "Excel")
                    {
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                        //dt = oRpt.rptHOLoanSanction(vFromDt, vToDt, vBrCode);rptMemFormation
                        //dt = oRpt.rptMemFormation(vFromDt, vToDt, vBrCode);
                        

                        DataGrid1.DataSource = dt;
                        DataGrid1.DataBind();

                        if (dt.Rows.Count > 0)
                        {
                            //string month = dt.Rows[0]["Month"].ToString();
                            //dt.Columns.Remove("Month");
                            DataGrid1.DataSource = dt;
                            DataGrid1.DataBind();
                            tdx.Controls.Add(DataGrid1);
                            tdx.Visible = false;
                            //vFileNm = "attachment;filename=" + gblFuction.setDate(txtFrmDt.Text).ToString("yyyyMMdd") + "_Bike_Log_Book.xls";
                            vFileNm = "attachment;filename=Hind_Sighting_Report.xls";
                            Response.ClearContent();
                            Response.AddHeader("content-disposition", vFileNm);
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.ms-excel";
                            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                            Response.Write("<table border='1' cellpadding=" + dt.Columns.Count + " widht='120%'>");
                            Response.Write("<tr><td align=center' colspan=" + dt.Columns.Count + "><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                            Response.Write("<tr><td align=center' colspan=" + dt.Columns.Count + "><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                            Response.Write("<tr><td align=center' colspan=" + dt.Columns.Count + "><b><font size='3'>Hind Sighting List   </font></b></td></tr>");
                            Response.Write("<tr><td align='center' colspan=" + dt.Columns.Count + "><b>From" + txtDtFrm.Text + "To" + txtToDt.Text + "</td></tr>");
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
                                    //if (dt.Columns[j].ColumnName == "BranchCode")
                                    //{
                                    //    Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                                    //}
                                    //else
                                    //{
                                    Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                                    //}
                                }
                                Response.Write("</tr>");
                            }
                            //DataGrid1.RenderControl(htw);
                            //htw.WriteLine("</td></tr>");
                            //htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");

                            Response.Write("</table>");
                            Response.End();
                        }
                        else
                        {
                            gblFuction.MsgPopup("No data found...");
                        }
                    }

                    else if (pMode == "CSV")
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
        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\Hind_Sighting_Report.csv";

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
        public object vType { get; set; }
    }
}