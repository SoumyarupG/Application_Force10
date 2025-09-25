using System;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.HOReports
{
    public partial class HOAtaGlance : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                BindYear();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Dashboard Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                // this.GetModuleByRole(mnuID.mnuRecPayRpt);
                this.GetModuleByRole(mnuID.mnuHOAtGlanceRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Dashboard Report", false);
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            // GetData("PDF");
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ddlMonth.SelectedValue == "")
            {
                gblFuction.MsgPopup("Please Select Month...");
                return;
            }
            string Month = ddlMonth.SelectedValue.ToString();
            string Year = ddlYear.SelectedValue.ToString();
            string Date = Month + Year;
            DateTime InputDate = gblFuction.setDate(Date);
            string vFileNm = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.rptAtAGlance(InputDate, vBrCode);
            vFileNm = "attachment;filename=" + ddlMonth.SelectedValue + "-" + ddlYear.SelectedValue + "_Dashboard_Report.xls";
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
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>  Dashboard Report </font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> For the Month - " + ddlMonth.SelectedItem.Text.ToString() + " And Year - " + ddlYear.SelectedValue + " </font></b></td></tr>");
            Response.Write("<tr>");

            foreach (DataColumn dtCol in dt.Columns)
            {
                Response.Write("<td ><b>" + dtCol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "SlNo")
                    {
                        Response.Write("<td align=left' class='txt' style='width:50px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Description")
                    {
                        Response.Write("<td align=left' class='txt' style='width:400px;'>" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "Value")
                    {
                        Response.Write("<td align=left'  style='width:200px;' >" + Convert.ToString(dr[j]) + "</td>");
                    }
                    else
                        Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                }
            }
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Flush();
            Response.End();

        }
        protected void BindYear()
        {
            string LogDate = Session[gblValue.LoginDate].ToString();
            DateTime Date = gblFuction.setDate(LogDate);
           // DateTime Date = Convert.ToDateTime(LogDate);
            var currentYear = Date.Year;
            ddlYear.Items.Clear();
            //var currentYear = DateTime.Today.Year;
            for (int i = 2; i >= 0; i--)
            {
                ddlYear.Items.Add(new ListItem((currentYear - i).ToString(), (currentYear - i).ToString()));
            }
            for (int i = 1; i <= 2; i++)
            {
                ddlYear.Items.Add(new ListItem((currentYear + i).ToString(), (currentYear + i).ToString()));
            }
            ddlYear.SelectedValue = currentYear.ToString();
        }
    }
}
