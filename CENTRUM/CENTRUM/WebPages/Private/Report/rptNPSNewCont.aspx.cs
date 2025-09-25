using System.Web;
using System.Web.UI;
using System.Data;
using System.IO;
using FORCECA;
using FORCEBA;
using System;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptNPSNewCont : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.FinFromDt]);
                txtToDt.Text = Convert.ToString(Session[gblValue.FinToDt]);
            }
        }
             private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NPS Bank Statement";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuNpsLiteCollection);
		        if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NPS Member", false);
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
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            
            DataTable dt = null;
            string vBranch = Session[gblValue.BrName].ToString();
            

            CReports oRpt = new CReports();
            dt = oRpt.rptNPSNewCont(vFromDt, vToDt, Session[gblValue.BrnchCode].ToString());

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            string vFileNm = "attachment;filename=NPSLiteColl";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='8' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='8'><b><u><font size='2'>NPS Lite Collection Report for the period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='8'><b><u><font size='28'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vAdmDt = gblFuction.setDate(txtDtFrm.Text.Trim());
            if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            {
                gblFuction.MsgPopup("As on Date should be Within this Financial Year");
                return;
            }
            vAdmDt = gblFuction.setDate(txtToDt.Text.Trim());
            if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            {
                gblFuction.MsgPopup("As on Date should be Within this Financial Year");
                return;
            }
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vAdmDt = gblFuction.setDate(txtDtFrm.Text.Trim());
            if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            {
                gblFuction.MsgPopup("As on Date should be Within this Financial Year");
                return;
            }
            vAdmDt = gblFuction.setDate(txtToDt.Text.Trim());
            if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            {
                gblFuction.MsgPopup("As on Date should be Within this Financial Year");
                return;
            }
            SetParameterForRptData("Excel");
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

        }
    }