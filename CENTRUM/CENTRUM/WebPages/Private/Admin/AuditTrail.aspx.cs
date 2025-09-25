using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class AuditTrail : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["Dtl"] = null;
                ViewState["ProdDtl"] = null;

                txtDtFrm.Text = Session[gblValue.FinFromDt].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopUser();
                //CheckAll();
                popLoanProduct();
                //CheckAllProd();
                popDetail();
                popProdDetail();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Audit Trail";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAreaMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Audit Trail", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopUser()
        {
            DataTable dt = null;
            CUser oUser = null;
            ViewState["Dtl"] = null;
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oUser = new CUser();
                dt = oUser.GetUserList(vBranch, "");
                chkDtl.DataSource = dt;
                chkDtl.DataTextField = "UserName";
                chkDtl.DataValueField = "UserId";
                chkDtl.DataBind();
                CheckAll();
            }
            finally
            {
                dt = null;
                oUser = null;
            }
        }
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GetData("Excel");
        }

        private void popLoanProduct()
        {
            DataTable dt = null;
            CGenParameter oGb = null;
            ViewState["ProdDtl"] = null;
            try
            {
                oGb = new CGenParameter();
                dt = oGb.GetAuditTrailModuleName();
                chkProd.DataSource = dt;
                chkProd.DataTextField = "ModuleName";
                chkProd.DataValueField = "ModuleName";
                chkProd.DataBind();
                CheckAllProd();
            }
            finally
            {
                oGb = null;
                dt = null;
            }
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

        protected void rblProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllProd();
            popProdDetail();
        }

        protected void chkDtl_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDetail();
        }

        private void popProdDetail()
        {
            ViewState["ProdDtl"] = null;
            string str = "";
            for (int vRow = 0; vRow < chkProd.Items.Count; vRow++)
            {
                if (chkProd.Items[vRow].Selected == true)
                {
                    if (str == "")
                        str = chkProd.Items[vRow].Value;
                    else if (str != "")
                        str = str + "," + chkProd.Items[vRow].Value;
                }
            }
            if (str == "")
                ViewState["ProdDtl"] = 0;
            else
                ViewState["ProdDtl"] = str;
        }


        protected void chkProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            popProdDetail();
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

        private void CheckAllProd()
        {
            Int32 vRow;
            if (rblProd.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkProd.Items.Count; vRow++)
                    chkProd.Items[vRow].Selected = true;
                chkProd.Enabled = false;
            }
            else if (rblProd.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkProd.Items.Count; vRow++)
                    chkProd.Items[vRow].Selected = false;
                chkProd.Enabled = true;
            }
        }
        private void GetData(string pFormat)
        {
            string vBranch = "", vBrCode = "", vFileNm = "";
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                vBranch = Session[gblValue.BrName].ToString();
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oRpt = new CReports();
                //CheckAll();
                //popDetail();
                //popProdDetail();
                string vUid = Convert.ToString(ViewState["Dtl"]);
                string vProduct = Convert.ToString(ViewState["ProdDtl"]);
                if (vUid == "" || vProduct == "")
                {
                    gblFuction.MsgPopup("Nothing Selected....");
                    return;
                }
                dt = oRpt.rptAuditTrail(vUid, vProduct, vFromDt, vToDt);
                vFileNm = "attachment;filename=" + gblFuction.setDate(txtToDt.Text).ToString("yyyyMMdd") + "_Audit_Trail.xls";
                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + "</font></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>Audit Trail</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><u><font size='3'>From : " + txtDtFrm.Text + "  To: " + txtToDt.Text + "</font></u></td></tr>");
                string tab = string.Empty;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
                }
                Response.Write("</tr>");
                foreach (DataRow dtrow in dt.Rows)
                {
                    Response.Write("<tr height='20px'>");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName == "CreationDate" || dt.Columns[j].ColumnName == "TrailId" || dt.Columns[j].ColumnName == "TransactionDate")
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
                Response.ClearContent();
                Response.ClearHeaders();
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Dtl"] = null;
                ViewState["ProdDtl"] = null;
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
    }
}