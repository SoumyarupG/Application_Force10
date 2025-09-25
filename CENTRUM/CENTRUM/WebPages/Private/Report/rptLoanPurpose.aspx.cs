using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptLoanPurpose : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
		if (!IsPostBack)
		{
			rbDtlsSumm_SelectedIndexChanged(sender, e);
			txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
			txtToDt.Text = Session[gblValue.FinToDt].ToString();
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
                this.PageHeading = "Loan Purpose";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBankBookRpt);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance Report", false);
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
        private void SetParameterForRptData(string pMode, string pSecId)
        {
            string vFileNm="";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vPurposeId = "";
            if (ddlPurpose.SelectedIndex > 0)
                vPurposeId = ddlPurpose.SelectedValue.ToString();


            DataTable dt = null;
            CLPurpose oRpt = new CLPurpose();

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
	        dt = oRpt.rptLoanPurpose(vFromDt, vToDt, vBrCode, vPurposeId);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();


            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=Loan Purpose List";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='7' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='3'>" + gblValue.CompName +"</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='7'><b><u><font size='2'>Loan Purpose Report for the period from " + txtDtFrm.Text + " to " + txtToDt.Text +"</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='7'><b><u><font size='7'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.Write(sw.ToString());
            Response.End();


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
                Response.Redirect("~/WebPages/Public/Main.aspx");
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
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ddlPurpose.SelectedValue == "-1" && rbDtlsSumm.SelectedValue!="0")
                gblFuction.MsgPopup("Please select a sector");
            else
                SetParameterForRptData("PDF", ddlPurpose.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ddlPurpose.SelectedValue == "-1" && rbDtlsSumm.SelectedValue != "0")
                gblFuction.MsgPopup("Please select a sector");
            else
                SetParameterForRptData("Excel", ddlPurpose.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopPurpose()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
		dt = oGbl.PopComboMIS("N", "N", "AA", "SubPurposeId", "SubPurpose", "SubPurPoseMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlPurpose.DataSource = dt;
		ddlPurpose.DataTextField = "SubPurpose";
		ddlPurpose.DataValueField = "SubPurposeId";
                ddlPurpose.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlPurpose.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbDtlsSumm_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopPurpose();
            if (rbDtlsSumm.SelectedIndex == 0)
            {
		ddlPurpose.SelectedIndex = -1;
                ddlPurpose.Enabled = false;
            }
            else
                ddlPurpose.Enabled = true;
        }
    }
}
