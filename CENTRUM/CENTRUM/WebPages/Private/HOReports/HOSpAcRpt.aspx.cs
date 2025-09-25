using System;
using System.Web;
using System.Data;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOSpAcRpt : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["BrCode"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch();
            }

        }
        
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Special Accounts";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOSpclAcctRpt);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Special Accounts Report", false);
                }
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
        
        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }
        
        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                chkBrDtl.Enabled = true;
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
                ViewState["BrCode"] = null;
                chkBrDtl.Enabled = true;
                for (vRow = 0; vRow < chkBrDtl.Items.Count; vRow++)
                    chkBrDtl.Items[vRow].Selected = false;

            }
            ViewState["BrCode"] = strin;
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

        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");

            chkBrDtl.DataSource = dt;
            chkBrDtl.DataTextField = "BranchName";
            chkBrDtl.DataValueField = "BranchCode";
            chkBrDtl.DataBind();

            CheckBrAll();
        }

        private void GetData(string pFormat)
        {
            string vFileNm = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vFinYrFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());

            if (vFinYrFrom > gblFuction.setDate(txtFrmDt.Text) || gblFuction.setDate(txtFrmDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("From Date should be within this financial year.");
                return;
            }
            if (vFinYrFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }

            if (ValidateFields() == false) return;

            if (vBrCode == "")
            {
                gblFuction.MsgPopup("Please Select At Least 1 Branch.");
                return;
            }

            DataTable dt = null;
            CReports oRpt = new CReports();

            if (ddlRptType.SelectedValue == "G0235" || ddlRptType.SelectedValue == "G0419" || ddlRptType.SelectedValue == "G0516" || ddlRptType.SelectedValue == "G0291")
            {
                dt = oRpt.rptInterBranchTransferAc(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                                   gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode,
                                                   ddlRptType.SelectedValue, vFinYrNo);
            }
            if(ddlRptType.SelectedValue == "2" || ddlRptType.SelectedValue == "3")
            {
                dt = oRpt.rptIncomeExpenseAc(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                             gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode,
                                             Convert.ToInt32(ddlRptType.SelectedValue), vFinYrNo);
            }
            if (ddlRptType.SelectedValue == "4")
            {
                dt = oRpt.rptJournalExlAc(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                                             gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), vBrCode,
                                             vFinYrNo);
            }
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=" + gblFuction.setDate(Session[gblValue.LoginDate].ToString()).ToString("yyyyMMdd") + "_Special_Accounts_Report";
            
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='3' widht='100%'>");
            
            htw.WriteLine("<tr><td align='center' colspan='8'" + "'><b><u><font size='4'>" + gblValue.CompName + "</font></u></b></td></tr>");

            htw.WriteLine("<tr><td align='center' colspan='8'" + "'><b><u><font size='3'>" + ddlRptType.SelectedItem.Text + " Report From " + txtFrmDt.Text + "  To " + txtToDt.Text + "</font></u></b></td></tr>");
            
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='8'><b><u><font size='2'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

        }

        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            if (ddlRptType.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Enter Report Type.");
                vResult = false;
            }
                       
            return vResult;
        }

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