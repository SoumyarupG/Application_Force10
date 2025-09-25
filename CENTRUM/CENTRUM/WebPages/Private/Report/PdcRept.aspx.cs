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

namespace CENTRUM.WebPages.Private.Report
{
    public partial class PdcRept : CENTRUMBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                FromDt.Text = Session[gblValue.LoginDate].ToString();
                ToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "PDC Analysis (Branchwise)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrPdc2Rpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PDC Analysis (Branchwise)", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["List"] = null;
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                DateTime vStDate = gblFuction.setDate(FromDt.Text);
                DateTime vEndDate = gblFuction.setDate(ToDt.Text);                
                oRpt = new CReports();
                ds = oRpt.rptPdcBranchAnalysis(vStDate, vEndDate, Session[gblValue.BrnchCode].ToString());
                DataGrid1.DataSource = ds.Tables[0];
                DataGrid1.DataBind();
                ViewState["List"] = ds;
            }
            finally
            {
                ds = null;
                oRpt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)ViewState["List"];
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();           
            string vFileNm = "";
            vFileNm = "attachment;filename=PDC_Analysis_Branchwise";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>PDC Analysis (Branchwise)</font></u></b></td></tr>"); 
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>Branch: " + Session[gblValue.BrName].ToString() + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>From Date: " + FromDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + ToDt.Text + "</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}