using System;
using System.Data;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using FORCECA;
using FORCEBA;
using System.Web.UI;
using System.Web;


namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptMaturity : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["BrCode"] = null;
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                //txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch();

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
                this.PageHeading = "Maturity Analysis Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuMaturityAnl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Maturity Analysis Report", false);
                }
            }
            catch
            {
                Response.Redirect("~/Sarala.aspx", false);
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
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }
        /// <summary>
        /// 
        /// </summary>
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkBrDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
            {
                if (chkBrDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }

            ViewState["BrCode"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            //CGblIdGenerator oCG = null;
            //string vBrCode = "";
            //Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CUser oUsr = null;
            oUsr = new CUser();
            //oCG = new CGblIdGenerator();
            //dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "000");
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = false;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = chkBrDtl.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + chkBrDtl.Items[vRow].Value + "";
                    }
                }
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vAsOn = gblFuction.setDate(txtFromDt.Text);
            DateTime vFinyear = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            string vRptPath = "";
            DataTable dt = null;
            CReports oRpt = new CReports();
            try
            {
                if (pMode == "PDF")
                {
                    //dt = oRpt.rptMaturity(vAsOn, vFinyear, vBrCode, vLnSchem, pMode);
                    //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\MaturityRpt.rpt";
                    //using (ReportDocument rptDoc = new ReportDocument())
                    //{
                    //    rptDoc.Load(vRptPath);
                    //    rptDoc.SetDataSource(dt);
                    //    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    //    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1("000"));
                    //    rptDoc.SetParameterValue("pAddress2", "");
                    //    rptDoc.SetParameterValue("pAsOnDt", txtFromDt.Text);
                    //    rptDoc.SetParameterValue("pTitle", "Maturity Analysis Report");
                    //    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Maturity_Analysis_Report");
                    //    rptDoc.Dispose();
                    //    Response.ClearHeaders();
                    //    Response.ClearContent();
                    //}
                }
                else
                {
                    dt = oRpt.rptMaturity(vAsOn, vBrCode, rdbSumDel.SelectedValue);
                    if (rdbSumDel.SelectedValue == "D")
                    {
                        dgMaturity.DataSource = dt;
                        dgMaturity.DataBind();
                    }
                    else
                    {
                        DgSum.DataSource = dt;
                        DgSum.DataBind();
                    }
                    string vFileNm = "attachment;filename=Maturity_Analysis_Report.xls";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                    htw.WriteLine("<tr><td></td><td></td><td></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress1("000") + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + CGblIdGenerator.GetBranchAddress2("000") + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>" + vBranch + "</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=center' colspan='23'><b><u><font size='4'>Maturity Analysis Report</font></u></b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    htw.WriteLine("<tr><td align=right' colspan='11'><b>As on Date : " + txtFromDt.Text + "</b></td></tr>");
                    htw.WriteLine("<tr><td></td></tr>");
                    if (rdbSumDel.SelectedValue == "D")
                    {
                        dgMaturity.RenderControl(htw);
                    }
                    else
                    {
                        DgSum.RenderControl(htw);
                    }

                    htw.WriteLine("</table>");
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Write(sw.ToString());
                    Response.End();
                }
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
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("PDF");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            if (ValidateDate() == false)
            {
                gblFuction.MsgPopup("Please Set The Valid Date Range.");
                return;
            }
            else
            {
                GetData("Excel");
            }
        }

    }
}