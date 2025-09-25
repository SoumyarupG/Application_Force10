using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class RptSancCanAppList : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtTDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch();
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Sanction Cancel Application Report";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuRptSancCanAppList);
                if (this.UserID == 1) return;
                //if (this.CanReport == "Y")
                //{
                //}
                //else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                //{
                //    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Repayment Schedule", false);
                //}
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            string vFileNm = "";
            
            DataSet ds = null;
            string vBrCode = ViewState["BrCode"].ToString();
            CReports oRpt = new CReports();
            DateTime vFrDt = gblFuction.setDate(txtFDt.Text);
            DateTime vToDt = gblFuction.setDate(txtTDt.Text);
            string vBranch = Session[gblValue.BrName].ToString();

            //***************************************
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            ds = oRpt.RptSancCanAppList(vFrDt, vToDt, vBrCode);
            DataGrid1.DataSource = ds;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=RptSancCanAppList.xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='5'>Unity Small Finance Bank Ltd.</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='4'>Sanction Cancel Application Report</font></u></b></td></tr>");

            htw.WriteLine("<tr><td align=center' colspan='13'><b><u><font size='3'>Branch Name "+ vBranch +"</font></u></b></td></tr>");
            htw.WriteLine("<tr><td colspan='4'><b>From Date : " + txtFDt.Text + "</b></td><td colspan='4'><b>To Date : " + txtTDt.Text + "</b></td></tr>");
           
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

        }


        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());


            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (Convert.ToString(dr["BranchCode"]) != Session[gblValue.BrnchCode].ToString())
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }
            }
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

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}

 