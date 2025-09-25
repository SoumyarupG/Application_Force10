using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;
using System.IO;
namespace CENTRUM.WebPages.Private.Inventory
{
    public partial class RptStckSummVeri : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Stock Verification Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuStckVeriRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "At a Glance", false);
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
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DateTime vFrmDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBranch = Session[gblValue.BrnchCode].ToString();
            CReports oRpt = null;
            try
            {
                oRpt = new CReports();
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                dt = oRpt.GetStckVerifyData(vFrmDt, vToDt, vBranch);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();


                string vFileNm = "attachment;filename=Stock_Verification_Report.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='14' widht='100%'>");
                //htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='5'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='5'><b><font size='3'>" + "Branch: " + Session[gblValue.BrName].ToString() + " </font></b></td></tr>");
                //htw.WriteLine("<tr><td align=center' colspan='14'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='5'><b><font size='3'>" + "From date: " + txtDtFrm.Text + " </font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='5'><b><font size='3'>" + "To Date: " + txtToDt.Text + " </font></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='5'><b><u><font size='5'>Stock Verification Report</font></u></b></td></tr>");
                DataGrid1.RenderControl(htw);
                htw.WriteLine("</td></tr>");
                htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
                htw.WriteLine("</table>");

                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Write(sw.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}