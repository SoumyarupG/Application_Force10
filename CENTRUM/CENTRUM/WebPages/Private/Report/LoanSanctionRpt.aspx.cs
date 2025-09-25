using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.Web;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class LoanSanctionRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
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


        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTag = "N",vTitle = "";
            string vBranch = Session[gblValue.BrName].ToString();
          
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

            TimeSpan t = vToDt - vFromDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            dt = oRpt.rptLoanSanction(vTag, vFromDt, vToDt, vBrCode);
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
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>"+vTitle+"</font></b></td></tr>");
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
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
