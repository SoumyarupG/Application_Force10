using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCEBA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class RptFDDtls : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAsOnDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "FD Details Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuRptFdDtls);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/Public/PageAccess.aspx?mnuTxt=" + "MonthlyRepaySchedule", false);
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
        /// <param name="pMode"></param>
        private void SetParameterForRptData()
        {
            DateTime vAsOnDt = gblFuction.setDate(txtAsOnDt.Text);
            //DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vFileNm = "";
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                    dt = new DataTable();
                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                    oRpt = new CReports();
                    dt = oRpt.rptFDdetails(vAsOnDt);
                    DataGrid1.DataSource = dt;
                    DataGrid1.DataBind();

                    tdx.Controls.Add(DataGrid1);
                    tdx.Visible = false;
                    vFileNm = "attachment;filename=FD Detail Report.xls";


                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                    //htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='12' style='background-color:Orange;'><b><u><font size='5'>Unity Small Finance Bank Ltd.</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='12'><b><u><font size='5'>81/2, Acharya Jagadish Chandra Bose Rd</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='12'><b><u><font size='5'>As On Date " + txtAsOnDt.Text + "</font></u></b></td></tr>");

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
            finally
            {
                dt = null;
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
            SetParameterForRptData();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

    }
}