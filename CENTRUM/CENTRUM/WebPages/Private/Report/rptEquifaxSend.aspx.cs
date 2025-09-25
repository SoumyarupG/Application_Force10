using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Report
{
    public partial class rptEquifaxSend : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.FinToDt].ToString();
                PopBranch();
                PopLoanType();
            }

        }
        /// <summary>
        /// 
        /// </summary>

        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
            chkDtl.DataSource = dt;
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "EquiFax Data Send";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOEqfxDtSnd);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Equifax Data Send", false);
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
            string vFileNm = "";
            string vBrCode = Convert.ToString(ViewState["Dtl"]);// Session[gblValue.BrnchCode].ToString();
            String vLnSchem = ddlLnSchem.SelectedValues.Replace("|", ",");
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);

            if (string.IsNullOrEmpty(vLnSchem))
            {
                gblFuction.MsgPopup("Please select a Loan Scheme");
                return;
            }

            DataTable dt = null;
            CReports oRpt = new CReports();

            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            dt = oRpt.rptEquifaxSend(vFromDt, vToDt, vBrCode, vLnSchem);
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();


            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=EQUIFax_Data_List.xls";

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='0' widht='100%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + gblValue.CompName + "</font></u></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrName].ToString()) + "</td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>EQUIFax_Data for the period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></u></b></td></tr>");
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
                    if (dt.Columns[j].ColumnName == "MEMBER_ID_UNIQUEACCOUNT_NUMBER")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "POSTAL_PIN")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "NATIONAL_ID_CARD_UIN")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "PHONE_MOBILE")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "KENDRA_ID")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else if (dt.Columns[j].ColumnName == "BRANCH_ID")
                    {
                        Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else
                    {
                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.End();
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //htw.WriteLine("<table border='1' cellpadding='28' widht='100%'>");
            //htw.WriteLine("<tr><td align=center' colspan='28'><b><u><font size='3'>" + gblValue.CompName + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='28'><b><u><font size='2'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='28'><b><u><font size='2'>EQUIFax_Data for the period from " + txtDtFrm.Text + " to " + txtToDt.Text + "</font></u></b></td></tr>");
            //DataGrid1.RenderControl(htw);
            //htw.WriteLine("</td></tr>");
            //htw.WriteLine("<tr><td colspan='28'><b><u><font size='28'></font></u></b></td></tr>");
            //htw.WriteLine("</table>");

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", vFileNm);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Write(sw.ToString());
            //Response.End();


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
            //if (ddlPurpose.SelectedValue == "-1" && rbDtlsSumm.SelectedValue!="0")
            //    gblFuction.MsgPopup("Please select a sector");
            //else
            //    SetParameterForRptData("PDF", ddlPurpose.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            //if (ddlPurpose.SelectedValue == "-1" && rbDtlsSumm.SelectedValue != "0")
            //    gblFuction.MsgPopup("Please select a sector");
            //else
            SetParameterForRptData("Excel");
        }

        private void PopLoanType()
        {
            DataTable dt = null;
            CApplication oApp = null;
            try
            {
                oApp = new CApplication();
                dt = oApp.GetLoanTypeForApp("A", Session[gblValue.BrnchCode].ToString());
                ddlLnSchem.DataSource = dt;
                ddlLnSchem.DataTextField = "LoanType";
                ddlLnSchem.DataValueField = "LoanTypeId";
                ddlLnSchem.DataBind();

            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }
    }
}
