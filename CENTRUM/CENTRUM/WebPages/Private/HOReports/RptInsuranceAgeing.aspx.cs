using System;
using System.Data;
using System.Web;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptInsuranceAgeing : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                txtAsDt.Text = Session[gblValue.LoginDate].ToString();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Insurance Ageing";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRptInsuAgeing);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance At a Glance", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
            {
                if (chkBr.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
            }
            ViewState["ID"] = strin;
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                            {
                                row.Delete();
                            }
                        }
                        dt.AcceptChanges();
                    }
                    chkBr.DataSource = dt;
                    chkBr.DataTextField = "BranchName";
                    chkBr.DataValueField = "BranchCode";
                    chkBr.DataBind();
                    CheckAll();
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }

        }

        private void CheckAll()
        {
            Int32 vRow;
            string strin = "";
            if (ddlSel.SelectedValue == "C")
            {
                chkBr.Enabled = false;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                {
                    chkBr.Items[vRow].Selected = true;
                    if (strin == "")
                        strin = chkBr.Items[vRow].Value;
                    else
                        strin = strin + "," + chkBr.Items[vRow].Value + "";
                }
                ViewState["ID"] = strin;
            }
            else if (ddlSel.SelectedValue == "B")
            {
                ViewState["ID"] = null;
                chkBr.Enabled = true;
                for (vRow = 0; vRow < chkBr.Items.Count; vRow++)
                    chkBr.Items[vRow].Selected = false;
            }
        }

        protected void ddlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GetBranch();
            string vFileNm = "";
            DateTime vAsOnDt = gblFuction.setDate(txtAsDt.Text);
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            string mode = "";
            string vBrCode = Convert.ToString(ViewState["ID"]);
            if (rdbOpt.SelectedValue == "rdbDocSub")
                mode = "rdbDocSub";
            else if (rdbOpt.SelectedValue == "rdbClmRcv")
                mode = "rdbClmRcv";
            try
            {
                oRpt = new CReports();
                dt = oRpt.RptInsuranceAgeing(vBrCode, vAsOnDt, mode);
                if (dt.Rows.Count > 0)
                {
                    vFileNm = "attachment;filename=" + gblFuction.setDate(txtAsDt.Text).ToString("yyyyMMdd") + "InsuranceAgeing.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='5' widht='120%'>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>Insurance Ageing </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>As On Date:- " + txtAsDt.Text + "</font></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'> </font></b></td></tr>");
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
                            if (dt.Columns[j].ColumnName == "Ageing" || dt.Columns[j].ColumnName == "Total Amount" )
                            {
                                Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
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
                    gblFuction.MsgPopup("No data found...");
                }

            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }


    }
}