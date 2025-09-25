using System;
using System.Data;
using System.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class MemberVerifyRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Member Verification Report";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.nmuMemVerifyRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Sanction", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = Convert.ToString(ViewState["BrCode"]);
            string vFileNm = "", vTitle="";
            string vBranch = Session[gblValue.BrName].ToString();
            //ReportDocument rptDoc = new ReportDocument();
            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

            if (rdbOpt.SelectedValue == "N") vTitle = "Member Verification Report (New)";
            if (rdbOpt.SelectedValue == "Y") vTitle = "Member Verification Report (Approved)";
            if (rdbOpt.SelectedValue == "C") vTitle = "Member Verification Report (Cancel)";

            dt = oRpt.rptMemVerification(rdbOpt.SelectedValue, vFromDt, vToDt, vBrCode);
            if (dt.Rows.Count > 0)
            {
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_Member_Verification.xls";

                Response.ClearContent();
                Response.AddHeader("content-disposition", vFileNm);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                Response.Write("<table border='1' cellpadding='0' widht='100%'>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + vTitle + "</font></u></b></td></tr>");
                Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>From : " + txtFromDt.Text + "  To: " + txtToDt.Text + "</font></td></tr>");

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
                        if (dt.Columns[j].ColumnName == "MEMBER NO")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "APPLICATION NO")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "APPLICATION DATE")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "SANCTION DATE")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "EXPECTED DISBURSEMENT DATE")
                        {
                            Response.Write("<td nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                        }
                        else if (dt.Columns[j].ColumnName == "ACCOUNT NO")
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
            }
            else
            {
                gblFuction.AjxMsgPopup("No Records");
            }
        }

        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = new DataTable();
            CUser oUsr = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oUsr = new CUser();
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
            }
            else
            {
                DataRow dr;
                DataColumn dc = new DataColumn("BranchCode");
                dt.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("BranchName");
                dt.Columns.Add(dc1);
                dr = dt.NewRow();

                dr["BranchCode"] = Session[gblValue.BrnchCode].ToString();
                dr["BranchName"] = Session[gblValue.BrName].ToString();
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            if (ddlSel.SelectedValue == "rbAll")
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
            else if (ddlSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                {
                    chkBrDtl.Items[vRow].Selected = false;
                }
            }
            ViewState["BrCode"] = strin;
        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "rbAll")
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
            else if (ddlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

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


        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

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
    }
}