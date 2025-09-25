using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using FORCECA;
using FORCEBA;
using System;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptNpsMember : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.FinFromDt]);
                txtToDt.Text = Convert.ToString(Session[gblValue.FinToDt]);
                PopBranch();
                CheckAll();
                popDetail();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NPS Member";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuNpsMemReport);
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
        private void PopBranch()
        {
            DataTable dt = null;
            DataTable dt1 = null;
            //CGblIdGenerator oCG = null;
            CUser oUsr = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]),"R");
            if (vBrCode != "0000")
            {

                dt1 = dt.Select("BranchCode =" + vBrCode).CopyToDataTable();
                chkDtl.DataSource = dt1;
            }
            else
            {
                chkDtl.DataSource = dt;
            }
            chkDtl.DataTextField = "BranchName";
            chkDtl.DataValueField = "BranchCode";
            chkDtl.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rblAlSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = false;
                chkDtl.Enabled = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void popDetail()
        {
            ViewState["Dtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkDtl.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkDtl.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["Dtl"] = 0;
            else
                ViewState["Dtl"] = str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
            popDetail();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ViewState["Dtl"].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            
            DataTable dt = null;
            string vBranch = Session[gblValue.BrName].ToString();

            CReports oRpt = new CReports();
            dt = oRpt.rptNpsMember(vFromDt, vToDt, vBrCode);

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            string vFileNm = "attachment;filename=NPSMmberList";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='1' cellpadding='24' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='24'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='24'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBranch) + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='24'><b><u><font size='2'>NPS Member Report for the period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='24'><b><u><font size='28'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
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
            //DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            //DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            //DateTime vAdmDt = gblFuction.setDate(txtDtFrm.Text.Trim());
            //if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            //{
            //    gblFuction.MsgPopup("As on Date should be Within this Financial Year");
            //    return;
            //}
            //vAdmDt = gblFuction.setDate(txtToDt.Text.Trim());
            //if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            //{
            //    gblFuction.MsgPopup("As on Date should be Within this Financial Year");
            //    return;
            //}
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //DateTime vFinFrmDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            //DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            //DateTime vAdmDt = gblFuction.setDate(txtDtFrm.Text.Trim());
            //if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            //{
            //    gblFuction.MsgPopup("As on Date should be Within this Financial Year");
            //    return;
            //}
            //vAdmDt = gblFuction.setDate(txtToDt.Text.Trim());
            //if (vAdmDt < vFinFrmDt || vAdmDt > vFinToDt)
            //{
            //    gblFuction.MsgPopup("As on Date should be Within this Financial Year");
            //    return;
            //}
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

