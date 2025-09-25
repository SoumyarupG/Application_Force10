using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class ConsldtPM : CENTRUMBase
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
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "Branch Grading through PM";               
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrGrdPmRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Grading through PM", false);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataTable ds = null;
            CReports oRpt = null;
            try
            {
                if (txtFrmDt.Text != "" && txtToDt.Text != "")
                {
                    DateTime vStDate = gblFuction.setDate(txtFrmDt.Text);
                    DateTime vEndDate = gblFuction.setDate(txtToDt.Text);
                    oRpt = new CReports();
                    ds = oRpt.rptConsldBrGradPM(vStDate, vEndDate,this.UserID);
                    DataGrid1.DataSource = ds;
                    DataGrid1.DataBind();
                    ViewState["List"] = ds;
                }
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
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (txtFrmDt.Text == "" || txtToDt.Text == "")
            {
                gblFuction.MsgPopup("Please select From To Date.");
                return;
            }
            DataTable ds = (DataTable)ViewState["List"];
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();            
            string vFileNm = "";
            vFileNm = "attachment;filename=PM_Grading.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='6'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='3'>Branch Grading (Based on Process Management)</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='6'><b><u><font size='3'>From Date: " + txtFrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + txtToDt.Text + "</font></u></b></td></tr>");
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
    }
}