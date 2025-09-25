using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class Trial : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopGenLedger();
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
                this.PageHeading = "Trial Balance";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.nmuTrailRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Trial Balance", false);
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
        private void PopGenLedger()
        {
            txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Trial.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
            string vRptType = "T";
            if (rbDtlsSumm.SelectedValue == "1")
                vRptType = "S";

            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            dt = oRpt.rptTrial(vAcMst, vAcDtl, gblFuction.setStrDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setStrDate(txtDtFrm.Text), gblFuction.setStrDate(txtToDt.Text), vBrCode, vFinYr);
            if (pFormat == "PDF")
            {
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", "");
                    rptDoc.SetParameterValue("pBranch", vBranch);
                    rptDoc.SetParameterValue("pTitle", "Trial Balance");
                    rptDoc.SetParameterValue("DtFrom", txtDtFrm.Text);
                    rptDoc.SetParameterValue("DtTo", txtToDt.Text);
                    rptDoc.SetParameterValue("pRptType", vRptType);
                    if (Session[gblValue.BCBranchYN].ToString() == "Y")
                    {
                        rptDoc.SetParameterValue("pBCBranch", Session[gblValue.BrName].ToString());
                    }
                    else
                    {
                        rptDoc.SetParameterValue("pBCBranch", "");
                    }
                    rptDoc.SetParameterValue("pParentBranch", Session[gblValue.ParentBranchName].ToString() + "-" + Session[gblValue.ParentBranchCode].ToString());
                    //if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Trial Balance");
                    //else
                    //    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Trial Balance");

                    rptDoc.Dispose();
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            else if (pFormat == "Excel")
            {
                string vFileNm = "attachment;filename=Trial Balance.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> Branch Trial Balance - " + Session[gblValue.BrnchCode].ToString() + "</font></b></td></tr>");
                if (Session[gblValue.BCBranchYN].ToString() == "Y")
                {
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> BC Branch - " + Session[gblValue.BrName].ToString() + ", Parent Branch - " + Session[gblValue.ParentBranchName].ToString() + "-" + Session[gblValue.ParentBranchCode].ToString() + "</font></b></td></tr>");
                }
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Date From:  - " + txtDtFrm.Text + " To:" + txtToDt.Text + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
                string tab = string.Empty;
                Response.Write("<tr>");
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("<tr style='height:20px;'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    Response.Write("</tr>");
                }
                Response.Write("<tr><td colspan='5'><b>Grand Total</b></td><td><b>" + dt.Compute("Sum(OpDrAmt)", string.Empty).ToString() + "<b></td><td><b>" + dt.Compute("Sum(OpCrAmt)", string.Empty).ToString() + "<b></td>");
                Response.Write("<td><b>" + dt.Compute("Sum(DrAmt)", string.Empty).ToString() + "<b></td><td><b>" + dt.Compute("Sum(CrAmt)", string.Empty).ToString() + "<b></td>");
                Response.Write("<td><b>" + dt.Compute("Sum(ClDrAmt)", string.Empty).ToString() + "<b></td><td><b>" + dt.Compute("Sum(ClCrAmt)", string.Empty).ToString() + "<b></td></tr>");
                Response.Write("</table>");
                Response.End();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool ValidateDate()
        {
            bool vRst = true;
            //if (gblFuction.CheckDtRange(txtFromDt.Text, txtToDate.Text) == false)
            //{
            //    vRst = false;
            //}
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
        protected void btnExcel_Click(object sender, EventArgs e)
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
