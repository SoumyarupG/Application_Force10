using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class FundGiven : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.FinFromDt].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Insurance Claim Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOFundGiven);
                if (this.UserID == 1) return;
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance Claim Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void Export()
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = new CReports();
            ReportDocument rptDoc = new ReportDocument();

            TimeSpan t = vToDt - vFrmDt;
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            dt = oRpt.rptFundGivenToMember(vFrmDt, vToDt, vBrCode);
            dgFundGiven.DataSource = dt;
            dgFundGiven.DataBind();

            string vFileNm = "attachment;filename=Insurance_Report";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress2("0000") + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>Insurance Report</font></u></b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            htw.WriteLine("<tr><td align=right' colspan='11'><b>From Date : " + txtFrmDt.Text + "</b></td><td colspan='1'></td><td align=left' colspan='11'><b>To Date : " + txtToDt.Text + "</b></td></tr>");
            htw.WriteLine("<tr><td></td></tr>");
            dgFundGiven.RenderControl(htw);
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
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
            Export();
        }
    }
}