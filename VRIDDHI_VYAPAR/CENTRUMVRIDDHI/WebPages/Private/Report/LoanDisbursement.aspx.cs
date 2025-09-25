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
    public partial class LoanDisbursement : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
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
                this.PageHeading = "Loan Disbursement Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuLoanDisbSaralRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void PopList(string pMode)
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            CEO oCM = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            Int32 vBrId = Convert.ToInt32(vBrCode);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (rblAlSel.SelectedValue == "rbEO")
            {
                oCM = new CEO();
                dt = oCM.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ChkDetail.DataSource = dt;
                ChkDetail.DataTextField = "EoName";
                ChkDetail.DataValueField = "EoId";
                ChkDetail.DataBind();
            }

            if (rblAlSel.SelectedValue == "rbLType")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "LoanTypeId", "LoanTypeName", "LoanTypeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ChkDetail.DataSource = dt;
                ChkDetail.DataTextField = "LoanTypeName";
                ChkDetail.DataValueField = "LoanTypeId";
                ChkDetail.DataBind();
            }
            if (rblAlSel.SelectedValue == "rbFund")
            {
                oCG = new CGblIdGenerator();
                if (pMode == "A")
                    dt = oCG.PopComboMIS("N", "N", "AA", "FunderID", "FunderName", "FunderMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                else
                    dt = oCG.PopComboMIS("S", "N", "AA", "FunderID", "FunderName", "FunderMst", pMode, "ManUnYN", "AA", vLogDt, "0000");

                ChkDetail.DataSource = dt;
                ChkDetail.DataTextField = "FunderName";
                ChkDetail.DataValueField = "FunderID";
                ChkDetail.DataBind();
            }

            if (rblAlSel.SelectedValue == "rbSec")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "SectorId", "SectorName", "SectorMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ChkDetail.DataSource = dt;
                ChkDetail.DataTextField = "SectorName";
                ChkDetail.DataValueField = "SectorId";
                ChkDetail.DataBind();
            }

            if (rblAlSel.SelectedValue == "rbPur")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "PurposeId", "PurposeName", "PurposeMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
                ChkDetail.DataSource = dt;
                ChkDetail.DataTextField = "PurposeName";
                ChkDetail.DataValueField = "PurposeId";
                ChkDetail.DataBind();
            }

            if (rblAlSel.SelectedValue == "rbGen")
            {
                ChkDetail.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "Male";
                oLs1.Value = "1";
                ChkDetail.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "Female";
                oLs2.Value = "2";
                ChkDetail.Items.Add(oLs2);
            }

            if (rblAlSel.SelectedValue == "rbCyl")
            {
                ChkDetail.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "1st Cycle";
                oLs1.Value = "1";
                ChkDetail.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "2nd Cycle";
                oLs2.Value = "2";
                ChkDetail.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "3rd Cycle";
                oLs3.Value = "3";
                ChkDetail.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "4th Cycle";
                oLs4.Value = "4";
                ChkDetail.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "5th Cycle";
                oLs5.Value = "5";
                ChkDetail.Items.Add(oLs5);

                ListItem oLs6 = new ListItem();
                oLs6.Text = "6th Cycle";
                oLs6.Value = "6";
                ChkDetail.Items.Add(oLs6);

                ListItem oLs7 = new ListItem();
                oLs7.Text = "7th Cycle";
                oLs7.Value = "7";
                ChkDetail.Items.Add(oLs7);

                ListItem oLs8 = new ListItem();
                oLs8.Text = "8th Cycle";
                oLs8.Value = "8";
                ChkDetail.Items.Add(oLs8);

                ListItem oLs9 = new ListItem();
                oLs9.Text = "9th Cycle";
                oLs9.Value = "9";
                ChkDetail.Items.Add(oLs9);

                ListItem oLs10 = new ListItem();
                oLs10.Text = "10th Cycle and More";
                oLs10.Value = "10";
                ChkDetail.Items.Add(oLs10);
            }


            if (rblAlSel.SelectedValue == "rbCast")
            {
                ChkDetail.Items.Clear();
                ListItem oLs1 = new ListItem();
                oLs1.Text = "General";
                oLs1.Value = "1";
                ChkDetail.Items.Add(oLs1);

                ListItem oLs2 = new ListItem();
                oLs2.Text = "OBC";
                oLs2.Value = "2";
                ChkDetail.Items.Add(oLs2);

                ListItem oLs3 = new ListItem();
                oLs3.Text = "SC";
                oLs3.Value = "3";
                ChkDetail.Items.Add(oLs3);

                ListItem oLs4 = new ListItem();
                oLs4.Text = "ST";
                oLs4.Value = "4";
                ChkDetail.Items.Add(oLs4);

                ListItem oLs5 = new ListItem();
                oLs5.Text = "Others";
                oLs5.Value = "5";
                ChkDetail.Items.Add(oLs5);
            }

            if (rblAlSel.SelectedValue == "rbWhole")
                ChkDetail.Items.Clear();
        }
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue =="rbAll")
            {
                for (vRow = 0; vRow < ChkDetail.Items.Count; vRow++)
                    ChkDetail.Items[vRow].Selected = true;
                ChkDetail.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < ChkDetail.Items.Count; vRow++)
                    ChkDetail.Items[vRow].Selected = false;
                ChkDetail.Enabled = true;
            }
        }
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < ChkDetail.Items.Count; vRow++)
            {
                if (ChkDetail.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = ChkDetail.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + ChkDetail.Items[vRow].Value;
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
        protected void ddlFundType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList(ddlFundType.SelectedValue);
            CheckAll();
            popDetail();
        }
        private void SetParameterForRptData(string pMode)
        {
            string vFileNm = "", pBranch = ViewState["Dtl"].ToString();
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
           
            string vTypeId = "", vType = "";//vRptPath = "", 
            string vBranch = Session[gblValue.BrName].ToString();
            popDetail();
            DataTable dt = null;
            CReports oRpt = new CReports();           
            string vBranchCode =  ViewState["Dtl"].ToString();

            try
            {

                if (rblAlSel.SelectedValue == "rbLType")
                    vType = "L";
                if (rblAlSel.SelectedValue == "rbFund")
                    vType = "F";
                if (rblAlSel.SelectedValue == "rbSec")
                    vType = "S";
                if (rblAlSel.SelectedValue == "rbPur")
                    vType = "P";
                if (rblAlSel.SelectedValue == "rbGen")
                    vType = "G";
                if (rblAlSel.SelectedValue == "rbCyl")
                    vType = "C";
                if (rblAlSel.SelectedValue == "rbCast")
                    vType = "T";
                if (rblAlSel.SelectedValue == "rbWhole")
                    vType = "A";


                //string vStr = "";

                dt = oRpt.rptLoanDisbXls(vFromDt, vToDt, vBranchCode, vType, vTypeId, Convert.ToInt32(Session[gblValue.BCProductId]));

                if (dt.Rows.Count > 0)
                {
                    vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Disbursement_Report.xls";
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
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Loan Disbursement Report</font></u></b></td></tr>");
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
                            if (dt.Columns[j].ColumnName == "Customer Id")
                            {
                                Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                            }
                            else if (dt.Columns[j].ColumnName == "Branch Code")
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
                else
                {
                    gblFuction.AjxMsgPopup("No record found");
                }
                //using (ReportDocument rptDoc = new ReportDocument())
                //{
                //    oRpt = new CReports();
                //    if (pMode == "PDF")
                //    {
                //        if (rbOpt.SelectedValue == "rbDtl")
                //            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbDtl.rpt";
                //        if (rbOpt.SelectedValue == "rbSum")
                //            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbSum.rpt";
                //        dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId);
                //        rptDoc.Load(vRptPath);
                //        rptDoc.SetDataSource(dt);
                //        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                //        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                //        rptDoc.SetParameterValue("pBranch", vBranch);
                //        rptDoc.SetParameterValue("pType", vType);
                //        rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                //        rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                //        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Loan_Disbursement_List");
                //        Response.ClearContent();
                //        Response.ClearHeaders();
                //    }
                //    else if (pMode == "Excel")
                //    {
                //        if (rbOpt.SelectedValue == "rbSum")
                //        {
                //            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanDisbSum.rpt";
                //            dt = oRpt.rptLoanDisb(vFromDt, vToDt, vBrCode, vType, vTypeId);
                //            rptDoc.Load(vRptPath);
                //            rptDoc.SetDataSource(dt);
                //            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                //            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                //            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                //            rptDoc.SetParameterValue("pBranch", vBranch);
                //            rptDoc.SetParameterValue("pType", vType);
                //            rptDoc.SetParameterValue("dtFrom", txtDtFrm.Text);
                //            rptDoc.SetParameterValue("dtTo", txtToDt.Text);
                //            rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Loan_Disbursement_List");
                //            Response.ClearContent();
                //            Response.ClearHeaders();
                //        }
                //        else
                //        {
                //            string vStr = "";

                //            dt = oRpt.rptLoanDisbXls(vFromDt, vToDt, vBranchCode, vType, vTypeId);


                //            vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Disbursement_Report.xls";
                //            Response.ClearContent();
                //            Response.AddHeader("content-disposition", vFileNm);
                //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //            Response.ContentType = "application/vnd.ms-excel";

                //            HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                //            Response.Write("<table border='1' cellpadding='0' width='100%'>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Loan Disbursement Report</font></u></b></td></tr>");
                //            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> From Date - " + txtDtFrm.Text + " - " + txtToDt.Text + " </font></b></td></tr>");
                //            Response.Write("<tr>");

                //            foreach (DataColumn dtCol in dt.Columns)
                //            {
                //                Response.Write("<td><b>" + dtCol.ColumnName + "<b></td>");
                //            }
                //            Response.Write("</tr>");
                //            foreach (DataRow dr in dt.Rows)
                //            {
                //                Response.Write("<tr style='height:20px;'>");
                //                for (int j = 0; j < dt.Columns.Count; j++)
                //                {
                //                    if (dt.Columns[j].ColumnName == "Customer Id")
                //                    {
                //                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                    else if (dt.Columns[j].ColumnName == "Branch Code")
                //                    {
                //                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                    else if (dt.Columns[j].ColumnName == "Loan Application No")
                //                    {
                //                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                    else if (dt.Columns[j].ColumnName == "Loan No")
                //                    {
                //                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                    else if (dt.Columns[j].ColumnName == "Applicant Contact No")
                //                    {
                //                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                    else
                //                    {
                //                        Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                //                    }
                //                }
                //            }
                //            Response.Write("</tr>");
                //            Response.Write("</table>");

                //            Response.Flush();
                //            Response.End();
                //        }
                //    }
                //}
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
                    ChkDetail.DataSource = dt;
                    ChkDetail.DataTextField = "BranchName";
                    ChkDetail.DataValueField = "BranchCode";
                    ChkDetail.DataBind();
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