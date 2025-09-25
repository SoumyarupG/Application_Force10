using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class MonthlyAttendance : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFrmDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Monthly Attendance Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOMonthlyAttn);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Monthly Attendance Report", false);
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

        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            if (vFrmDt.Month != vToDt.Month)
            {
                gblFuction.AjxMsgPopup("From Date and To Date Should be Within Same Month.");
                return;
            }

            SetParameterForRptData("Excel");
        }

        protected void btnExclNew_Click(object sender, EventArgs e)
        {            
            SetParameterForRptData_New("Excel");
        }

        private void SetParameterForRptData(string pMode)
        {

            DataTable dt = null;
            CReports oRpt = new CReports();
            DateTime vFrmDt, vToDt;
            vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            vToDt = gblFuction.setDate(txtToDt.Text);

            TimeSpan t = vToDt - vFrmDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            string vFileNm = "attachment;filename=Monthly Attendance Report_" + DateTime.Now + ".xls";
            try
            {
                oRpt = new CReports();
                dt = oRpt.rptEmpAttendanceSummary(vFrmDt, vToDt);
                if (dt.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Attendance Report Status For the Period from " + txtFrmDt.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
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
                            if (dt.Columns[j].ColumnName == "Empcode")
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
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        private void SetParameterForRptData_New(string pMode)
        {

            DataTable dt = null;
            CReports oRpt = new CReports();
            DateTime vFrmDt, vToDt;
            vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            vToDt = gblFuction.setDate(txtToDt.Text);

            string vFileNm = "attachment;filename=Monthly Attendance Report_" + DateTime.Now + ".xls";
            try
            {
                oRpt = new CReports();
                dt = oRpt.rptEmpAttendanceSummary_New(vFrmDt, vToDt);
                if (dt.Rows.Count > 0)
                {
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Attendance Report Status For the Period from " + txtFrmDt.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
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
                            if (dt.Columns[j].ColumnName == "Empcode")
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
                    gblFuction.AjxMsgPopup("No Data Found.");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }
    }
}