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

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.HOReports
{
    public partial class CBSubmissionRpt : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "CB Data Submission";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuCBDataSubmissionRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "CIBIL Data Submission", false);
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
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vUserID = Convert.ToInt32(Session[gblValue.UserId]);
            oCD = new CLoanRecovery();
            dt = oCD.GetBranchByBrCode(vBrCode, vUserID);
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
                oCD = null;
            }
        }
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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
        private void SetParameterForRptData(string pMode)
        {
            if (ddlDataFormat.SelectedValue == "C")
            {
                if (txtToDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                    return;
                }
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ViewState["BrCode"].ToString();
                string vTitle = "CIBIL Data Submission";
                string vBranch = Session[gblValue.BrName].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                CReports oRpt = null;
                try
                {
                    oRpt = new CReports();
                    ds = oRpt.rptCIBILXls(vToDt, vBrCode, ddlDataFormat.SelectedValue);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }

                    if (pMode == "Excel")
                    {
                        string vFileNm = "";
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                        vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_CIBILSubmission.TUDF";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFromDt.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("<tr>");
                        foreach (DataColumn dtCol in dt1.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");

                        Response.Flush();
                        Response.End();
                    }
                }
                finally
                {
                    dt = null;
                    oRpt = null;
                }
            }
            if (ddlDataFormat.SelectedValue == "T")
            {
                if (txtToDt.Text == "" || txtFromDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                    return;
                }
                DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ViewState["BrCode"].ToString();
                string vTitle = "Transunion Data Submission";
                string vBranch = Session[gblValue.BrName].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                CReports oRpt = null;
                try
                {
                    oRpt = new CReports();
                    //ds = oRpt.rptTransUnionXls(vFromDt,vToDt, vBrCode);
                    ds = oRpt.rptCIBILXls(vToDt, vBrCode, ddlDataFormat.SelectedValue);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }

                    if (pMode == "Excel")
                    {
                        string vFileNm = "";
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                        vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_TransunionSubmission.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFromDt.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("<tr>");
                        foreach (DataColumn dtCol in dt1.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");

                        Response.Flush();
                        Response.End();
                    }
                }

                finally
                {
                    dt = null;
                    oRpt = null;
                }
            }
            if (ddlDataFormat.SelectedValue == "CR")
            {
                if (txtToDt.Text == "" || txtFromDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                    return;
                }
                DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ViewState["BrCode"].ToString();
                string vTitle = "CRIFF Data Submission";
                string vBranch = Session[gblValue.BrName].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                CReports oRpt = null;
                try
                {
                    oRpt = new CReports();
                    //ds = oRpt.rptCRIFFXls(vFromDt, vToDt, vBrCode);
                    ds = oRpt.rptCIBILXls(vToDt, vBrCode, ddlDataFormat.SelectedValue);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }

                    if (pMode == "Excel")
                    {
                        string vFileNm = "";
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                        vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_CRIFFSubmission.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFromDt.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("<tr>");
                        foreach (DataColumn dtCol in dt1.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");

                        Response.Flush();
                        Response.End();
                    }
                }

                finally
                {
                    dt = null;
                    oRpt = null;
                }
            }
            if (ddlDataFormat.SelectedValue == "E")
            {
                if (txtToDt.Text == "" || txtFromDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                    return;
                }
                DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ViewState["BrCode"].ToString();
                string vTitle = "EXPERIAN Data Submission";
                string vBranch = Session[gblValue.BrName].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                CReports oRpt = null;
                try
                {
                    oRpt = new CReports();
                    //ds = oRpt.rptExperianXls(vFromDt, vToDt, vBrCode);
                    ds = oRpt.rptCIBILXls(vToDt, vBrCode, ddlDataFormat.SelectedValue);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }

                    if (pMode == "Excel")
                    {
                        string vFileNm = "";
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                        vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_ExperianSubmission.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFromDt.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("<tr>");
                        foreach (DataColumn dtCol in dt1.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");

                        Response.Flush();
                        Response.End();
                    }
                }

                finally
                {
                    dt = null;
                    oRpt = null;
                }
            }
            if (ddlDataFormat.SelectedValue == "EF")
            {
                if (txtToDt.Text == "" || txtFromDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Date Can Not Be Blank");
                    return;
                }
                DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ViewState["BrCode"].ToString();
                string vTitle = "EQUIFAX Data Submission";
                string vBranch = Session[gblValue.BrName].ToString();
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                CReports oRpt = null;
                try
                {
                    oRpt = new CReports();
                    //ds = oRpt.rptEquifaxXls(vFromDt, vToDt, vBrCode);
                    ds = oRpt.rptCIBILXls(vToDt, vBrCode, ddlDataFormat.SelectedValue);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }

                    if (pMode == "Excel")
                    {
                        string vFileNm = "";
                        System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                        vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_EquifaxSubmission.xls";
                        Response.ClearContent();
                        Response.AddHeader("content-disposition", vFileNm);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                        Response.Write("<table border='1' cellpadding='0' width='100%'>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");
                        //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtFromDt.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                        Response.Write("<tr>");

                        foreach (DataColumn dtCol in dt.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("<tr>");
                        foreach (DataColumn dtCol in dt1.Columns)
                        {
                            Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                        }
                        Response.Write("</tr>");
                        foreach (DataRow dr in dt1.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                Response.Write("<td nowrap class='txt' >" + Convert.ToString(dr[j]) + "</td>");
                            }
                        }
                        Response.Write("</tr>");
                        Response.Write("</table>");

                        Response.Flush();
                        Response.End();
                    }
                }

                finally
                {
                    dt = null;
                    oRpt = null;
                }
            }
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }
        protected void btnTUDF_Click(object sender, EventArgs e)
        {
            GetTUDF();
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

        private void GetTUDF()
        {
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DateTime vFrmDt = gblFuction.setDate(txtFromDt.Text);
            string vBrCode = ViewState["BrCode"].ToString();
            DataTable dt1 = new DataTable();
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                dt1 = oRpt.TUDFCIBILEnquiryFile(vFrmDt, vToDt, ddlDataFormat.SelectedValue);
                if (dt1.Rows.Count > 0)
                {
                    PrintTxt(dt1);
                }
                else
                {
                    gblFuction.MsgPopup("No data found.");
                }
            }
            finally { }
        }

        private void PrintTxt(DataTable dt)
        {
            string vFolderPath = "C:\\BijliReport";
            string vFileNm = "";
            vFileNm = vFolderPath + "\\TUDF.txt";
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
                File.WriteAllText(vFileNm, Convert.ToString(dt.Rows[0]["TUDF"]));
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
      

        private void downloadfile(string filename)
        {            
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
    }
}
