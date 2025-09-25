using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class LucBranch : CENTRUMBase
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
                popBranch();
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
                this.PageHeading = "LUC Analysis (Branchwise)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLucBrnch);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "LUC Analysis (Branchwise)", false);
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
        private void popBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem(" ALL ", "-1");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {

            if (txtFrmDt.Text == "" || txtToDt.Text == "")
            {
                gblFuction.MsgPopup("Please select From To date.");
                return;
            }
            DataSet ds = null;
            CReports oRpt = null;
            try
            {
                DateTime vStDate = gblFuction.setDate(txtFrmDt.Text);
                DateTime vEndDate = gblFuction.setDate(txtToDt.Text);
                oRpt = new CReports();
                ds = oRpt.rptLucBranchAnalysis(vStDate, vEndDate, ddlBranch.SelectedValue);
                DataGrid1.DataSource = ds;
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
            if (txtFrmDt.Text == "" || txtToDt.Text == "")
            {
                gblFuction.MsgPopup("Please select From To date.");
                return;
            }

            DataSet ds = (DataSet)ViewState["List"];
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();            
            string vFileNm = "";            
            vFileNm = "attachment;filename=LUC_Analysis_Branch.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'>" + CGblIdGenerator.GetBranchAddress1("0000") + "</td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>LUC Analysis (Branchwise)</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>From Date: " + txtFrmDt.Text + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; To Date: " + txtToDt.Text + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>Branch: " + ddlBranch.SelectedItem.Text + "</font></u></b></td></tr>");
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