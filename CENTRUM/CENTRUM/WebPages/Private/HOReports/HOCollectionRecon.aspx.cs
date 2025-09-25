using System;
using System.Web;
using System.Data;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HOCollectionRecon : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["BrCode"] = null;
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
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
                this.PageHeading = "HO Collection Reconciliation";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHOCollRecon);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO Collection Reconciliation", false);
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
            CGblIdGenerator oCG = null;
            //string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");

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
        private void GetData(string pFormat)
        {
            string vFileNm = "";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = ViewState["BrCode"].ToString();
            DateTime vFinYrFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            Int32 vFinYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vAcMst = Session[gblValue.ACVouMst].ToString();
            string vAcDtl = Session[gblValue.ACVouDtl].ToString();

            DataTable dt = null;
            CReports oRpt = new CReports();
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            if (vFinFrom > gblFuction.setDate(txtFromDt.Text) || gblFuction.setDate(txtFromDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("Form Date should be within this financial year.");
                return;
            }
            if (vFinFrom > gblFuction.setDate(txtToDt.Text) || gblFuction.setDate(txtToDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("To date should be within this financial year.");
                return;
            }


            dt = oRpt.rptCollRecon(gblFuction.setDate(txtFromDt.Text), gblFuction.setDate(txtToDt.Text), vFinFrom, vAcMst,
            vAcDtl, vFinYrNo, vBrCode);
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();

            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;

            vFileNm = "attachment;filename=" + gblFuction.setDate(txtFromDt.Text).ToString("yyyyMMdd") + "_Collection_Reconciliation_Report";

            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='3' widht='100%'>");


            htw.WriteLine("<tr><td align='center'>" + gblValue.CompName + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Text) + 3) + "'><b><u><font size='2'>"+CGblIdGenerator.GetBranchAddress1(vBrCode)+"</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='" + Convert.ToString(Convert.ToInt32(TxtInDays.Text) + 3) + "'><b><u><font size='2'>"+CGblIdGenerator.GetBranchAddress2(vBrCode)+"</font></u></b></td></tr>");

            htw.WriteLine("<tr><td align='center'><b><u><font size='3'>Collection Reconciliation Report From " + txtFromDt.Text.ToString() + " To " + txtToDt.Text.ToString() + "</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='4'><b><u><font size='2'></font></u></b></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

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
