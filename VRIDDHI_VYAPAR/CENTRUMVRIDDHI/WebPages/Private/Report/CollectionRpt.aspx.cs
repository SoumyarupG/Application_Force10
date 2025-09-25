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

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Report
{
    public partial class CollectionRpt : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //PopList(ddlFundType.SelectedValue);
                //CheckAll();
                //popDetail();
                //PopBranch();
                PopBranch(Session[gblValue.UserName].ToString());
                CheckAll();
                popDetail();
                //PopState();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Collection Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuCollectionSaralRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void ddlFundType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList(ddlFundType.SelectedValue);
            CheckAll();
            popDetail();
        }

        private void PopList(string pMode)
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oCM = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblSel.SelectedValue == "rbEO")
            {
                oCM = new CEO();
                dt = oCM.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "EoName";
                chkDtl.DataValueField = "EoId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanTypeName", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "LoanTypeName";
                chkDtl.DataValueField = "LoanTypeId";
                chkDtl.DataBind();
            }
            if (rblSel.SelectedValue == "rbFund")
            {
                oCG = new CGblIdGenerator();
                if (pMode == "A")
                    dt = oCG.PopComboMIS("N", "N", "AA", "FunderID", "FunderName", "FunderMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                else
                    dt = oCG.PopComboMIS("S", "N", "AA", "FunderID", "FunderName", "FunderMst", pMode, "ManUnYN", "AA", vLogDt, "0000");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "FunderName";
                chkDtl.DataValueField = "FunderID";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbGrp")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("S", "N", "AA", "GroupId", "GroupName", "GroupMst", vBrId, "BranchCode", "AA", vLogDt, vBrCode);
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "GroupName";
                chkDtl.DataValueField = "GroupId";
                chkDtl.DataBind();
            }

            if (rblSel.SelectedValue == "rbCyl")
            {
                chkDtl.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "1st Cycle";
                oLs1.Value = "1";
                chkDtl.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "2nd Cycle";
                oLs2.Value = "2";
                chkDtl.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "3rd Cycle";
                oLs3.Value = "3";
                chkDtl.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "4th Cycle";
                oLs4.Value = "4";
                chkDtl.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "5th Cycle";
                oLs5.Value = "5";
                chkDtl.Items.Add(oLs5);
            }

            if (rblSel.SelectedValue == "rbWhole")
                chkDtl.Items.Clear();
        }
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkBranch.Items.Count; vRow++)
                    chkBranch.Items[vRow].Selected = true;
                chkBranch.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBranch.Items.Count; vRow++)
                    chkBranch.Items[vRow].Selected = false;
                chkBranch.Enabled = true;
            }
        }
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkBranch.Items.Count; vRow++)
            {
                if (chkBranch.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkBranch.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkBranch.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }

        protected void rblSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList(ddlFundType.SelectedValue);
            CheckAll();
            popDetail();
        }
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTypeId = "", vType = "";//vRptPath = "", 
            string vBranch = Session[gblValue.BrName].ToString();
            popDetail();
            DataTable dt = null;
            CReports oRpt = null;
            string vBranchCode = ViewState["Dtl"].ToString();
            try
            {
                dt = new DataTable();
                oRpt = new CReports();
                vTypeId = ViewState["Dtl"].ToString();
                if (rblSel.SelectedValue == "rbEO")
                    vType = "E";
                if (rblSel.SelectedValue == "rbLType")
                    vType = "L";
                if (rblSel.SelectedValue == "rbFund")
                    vType = "F";
                if (rblSel.SelectedValue == "rbGrp")
                    vType = "G";
                if (rblSel.SelectedValue == "rbCyl")
                    vType = "C";
                if (rblSel.SelectedValue == "rbWhole")
                    vType = "A";
                
                
                if (pMode == "Excel")
                {
                    dt = oRpt.rptLoanCollectionXL(vFromDt, vToDt, vBranchCode, vType, vTypeId, Convert.ToInt32(Session[gblValue.BCProductId]));
                    string vFileNm = "";//, vStr = ""
                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                    vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_CollectionReport.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";

                    HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='0' width='100%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'> Collection Report</font></u></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtDtFrm.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
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
                            if (dt.Columns[j].ColumnName == "BranchCode")
                            {
                                Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                            }
                            else
                            {
                                Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                            }
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
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
        //private void PopBranch()
        //{
        //    DataTable dt = null;
        //    CLoanRecovery oCD = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    Int32 vUserID = Convert.ToInt32(Session[gblValue.UserId]);
        //    try
        //    {
        //        oCD = new CLoanRecovery();
        //        dt = oCD.GetBranchByBrCode(vBrCode, vUserID);
        //        CheckBoxList1.DataSource = dt;
        //        CheckBoxList1.DataTextField = "BranchName";
        //        CheckBoxList1.DataValueField = "BranchCode";
        //        CheckBoxList1.DataBind();
        //        if (vBrCode == "0000")
        //        {
        //            ListItem oli = new ListItem("All", "A");
        //            ddlBranch.Items.Insert(0, oli);
        //        }
        //    }
        //    finally
        //    {
        //        oCD = null;
        //        dt = null;
        //    }
        //}
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
                    chkBranch.DataSource = dt;
                    chkBranch.DataTextField = "BranchName";
                    chkBranch.DataValueField = "BranchCode";
                    chkBranch.DataBind();
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
    }
}
