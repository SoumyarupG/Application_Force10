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
    public partial class BankBook : CENTRUMBase
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
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = true;
                }
                PopList();
                CheckAll();
                popDetail();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
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
                this.PageHeading = "Bank Book";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuBankBookRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Book", false);
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
        private void PopList()
        {
            DataTable dt = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }
            CVoucher oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB(vBrCode, "B", "");
            chkDtl.DataSource = dt;
            chkDtl.DataTextField = "Desc";
            chkDtl.DataValueField = "DescId";
            chkDtl.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAll()
        {
            Int32 vRow;
            if (rdbSel.SelectedValue == "rdbAll")
            {
                for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
                    chkDtl.Items[vRow].Selected = true;
                chkDtl.Enabled = false;
            }
            else if (rdbSel.SelectedValue == "rdbSelct")
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
        /// <returns></returns>
        private bool ValidateDate()
        {
            bool vRst = true;
            if (gblFuction.CheckDtRange(txtFrmDt.Text, txtToDt.Text) == false)
            {
                vRst = false;
            }
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFormat"></param>
        /// 

        private void SetParameterForRptData(string pMode)
        {
            string vBranch = Session[gblValue.BrName].ToString();          
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }
            string vBank = ViewState["Dtl"].ToString();
            DataTable dt = null;
           
            CReports oRpt = new CReports();

            TimeSpan t = gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtFrmDt.Text);
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }


            dt = oRpt.rptCashBook_ExportExcel(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vBrCode,
                       gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBank,
                       Convert.ToInt32(Session[gblValue.FinYrNo]));

            dt.Columns.Remove("DescId");
            dt.Columns.Remove("CoDescId");
            dt.Columns.Remove("VoucherDt");
            dt.Columns.Remove("VoucherNo");
            dt.AcceptChanges();

            if (pMode == "Excel")
            {
                string vFileNm = "attachment;filename=Bank_Book_Report.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Bank Book Report</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>For the Period from " + txtFrmDt.Text + " to " + txtToDt.Text + "</font></b></td></tr>");
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
                        //if (dt.Columns[j].ColumnName == "GroupNo" || dt.Columns[j].ColumnName == "MemberNo" || dt.Columns[j].ColumnName == "LO Code")
                        //{
                        //    Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        //}
                        //else
                        //{
                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                        //}
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
                Response.End();
            }


        }


        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\BankBookRpt.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }
            string vBank = ViewState["Dtl"].ToString();
            DataTable dt = null;
            //ReportDocument rptDoc = new ReportDocument();
            CReports oRpt = new CReports();

            //dt = oRpt.rptBankBook(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vBrCode,
            //            gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBank,
            //            Convert.ToInt32(Session[gblValue.FinYrNo]));

            TimeSpan t = gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtFrmDt.Text);
            if (t.TotalDays > 2)
            {
                gblFuction.AjxMsgPopup("You can not downloand more than 3 days report.");
                return;
            }

            dt = oRpt.rptCashBook(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vBrCode,
                       gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBank,
                       Convert.ToInt32(Session[gblValue.FinYrNo]));

            using (ReportDocument rptDoc = new ReportDocument())
            {
                rptDoc.Load(vRptPath);
                rptDoc.SetDataSource(dt);
                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                rptDoc.SetParameterValue("pAddress2", "");
                rptDoc.SetParameterValue("pBranch", vBranch);
                rptDoc.SetParameterValue("pTitle", "Bank Book");
                rptDoc.SetParameterValue("DtFrom", txtFrmDt.Text);
                rptDoc.SetParameterValue("DtTo", txtToDt.Text);
                rptDoc.SetParameterValue("Ledger", "");
                if (pFormat == "PDF")
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Bank Book");
                else
                    rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, "Bank Book");
                rptDoc.Dispose();
                Response.ClearHeaders();
                Response.ClearContent();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbSel_SelectedIndexChanged(object sender, EventArgs e)
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
                //GetData("Excel");
                SetParameterForRptData("Excel");
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
                Response.RedirectPermanent("~/WebPages/Public/Main.aspx", false);
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
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopList();
            CheckAll();
            popDetail();
        }
    }
}
