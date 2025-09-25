using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.UI;
using System.Web;

namespace CENTRUM.WebPages.Private.HOReports
{
    public partial class HODmndColl : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString()); //PopListBranch();
                CheckAll();
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Branchwise Demand and Collection Report With Advance & Arrear";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuHODtWsDmndCol);
                if (this.UserID == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/Public/PageAccess.aspx?mnuTxt=" + "Demand and Collection", false);
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void SetParameterForRptData(string pMode)
        {
            DateTime vFromDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            string vBrCode = "";
            string vRptPath = "", vTitle = "", vRptType = "", vCategory = "", vDescID = "";
            string vBranch = Session[gblValue.BrName].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            string vEOID = "";  //vLTID = 0, vFSID = 0, vLCID = 0, 

            GetBranch();
            GetProdFund();


            vBrCode = Convert.ToString(ViewState["Id"]);
            vDescID = Convert.ToString(ViewState["CatId"]);

            if (rblSummDtl.SelectedValue == "rbSumm")
                vRptType = "S";
            else
                vRptType = "D";

            if (rbProdFund.SelectedValue == "rbProd")
                vCategory = "P";
            else if (rbProdFund.SelectedValue == "rbFund")
                vCategory = "F";
            else
                vCategory = "D";


            oRpt = new CReports();
            dt = new DataTable();

            if (pMode == "Excel")
            {
                System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                dt = oRpt.rptDCBalance(vFromDt, vToDt, vEOID, vBrCode, vRptType, vCategory, vDescID);
                DataGrid1.DataSource = dt;
                DataGrid1.DataBind();
                tdx.Controls.Add(DataGrid1);
                tdx.Visible = false;
                string vFileNm = "attachment;filename=HODemadnCollection.xls";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
                htw.WriteLine("<tr><td align=center' colspan='21'><b><u><font size='3'>Consolidated Demand Collection Report with Advance & Arrear Bifurcation</font></u></b></td></tr>");
                htw.WriteLine("<tr><td align=center' colspan='21'><b><u><font size='2'>From Date " + txtDtFrm.Text.ToString() + " To Date " + txtToDt.Text.ToString() + " </font></u></b></td></tr>");
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("PDF");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcl_Click(object sender, EventArgs e)
        {
            SetParameterForRptData("Excel");
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oCG = null;
            string vBrCode = (string)Session[gblValue.BrnchCode];
            ViewState["Id"] = null;
            try
            {
                DateTime vLogDt = gblFuction.setDate(txtToDt.Text.ToString());
                oCG = new CGblIdGenerator();
                oUsr = new CUser();
                ViewState["Id"] = null;
                if (vBrCode == "0000")
                {
                    dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                    if (dt.Rows.Count > 0)
                    {
                        chkDtl.DataSource = dt;
                        chkDtl.DataTextField = "BranchName";
                        chkDtl.DataValueField = "BranchCode";
                        chkDtl.DataBind();
                        CheckAll();
                    }
                }
                else
                {
                    dt = oCG.PopComboMIS("N", "Y", "BranchName", "BranchCode", "BranchCode", "BranchMst", 0, "AA", "AA", vLogDt, vBrCode);
                    chkDtl.DataSource = dt;
                    chkDtl.DataTextField = "Name";
                    chkDtl.DataValueField = "BranchCode";
                    chkDtl.DataBind();
                    CheckAll();
                }


            }
            finally
            {
                dt = null;
                oUsr = null;
                oCG = null;
            }
        }


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
            GetBranch();
        }

        private void GetBranch()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkDtl.Items.Count; vRow++)
            {
                if (chkDtl.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkDtl.Items[vRow].Value;
                    else
                        strin = strin + "," + chkDtl.Items[vRow].Value + "";
                }
            }
            ViewState["Id"] = strin;
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAll();
        }

        protected void rbProdFund_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            ViewState["CatId"] = null;

            if (rbProdFund.SelectedValue == "rbDef")
            {
                chkProdFund.Items.Clear();
                chkProdFund.DataSource = null;
            }
            else if (rbProdFund.SelectedValue == "rbProd")
            {
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "ProductId", "Product", "LoanProductMst", "0000", "BranchCode", "AA", vLogDt, "0000");
                chkProdFund.DataSource = dt;
                chkProdFund.DataTextField = "Product";
                chkProdFund.DataValueField = "ProductId";
                chkProdFund.DataBind();
            }
            else if (rbProdFund.SelectedValue == "rbFund")
            {
                chkProdFund.Items.Clear();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("N", "N", "AA", "FundsourceID", "Fundsource", "FundsourceMst", "", "", "AA", vLogDt, "0000");
                chkProdFund.DataSource = dt;
                chkProdFund.DataTextField = "Fundsource";
                chkProdFund.DataValueField = "FundsourceID";
                chkProdFund.DataBind();
            }
            CheckAllProdFund();
        }


        protected void rbProdFundAllSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckAllProdFund();
        }

        private void CheckAllProdFund()
        {
            Int32 vRow;
            if (rbProdFundAllSel.SelectedValue == "rbAll")
            {
                for (vRow = 0; vRow < chkProdFund.Items.Count; vRow++)
                    chkProdFund.Items[vRow].Selected = true;
                chkProdFund.Enabled = false;
            }
            else if (rbProdFundAllSel.SelectedValue == "rbSel")
            {
                for (vRow = 0; vRow < chkProdFund.Items.Count; vRow++)
                    chkProdFund.Items[vRow].Selected = false;
                chkProdFund.Enabled = true;
            }
            GetProdFund();
        }

        private void GetProdFund()
        {
            Int32 vRow;
            string strin = "";
            for (vRow = 0; vRow < chkProdFund.Items.Count; vRow++)
            {
                if (chkProdFund.Items[vRow].Selected == true)
                {
                    if (strin == "")
                        strin = chkProdFund.Items[vRow].Value;
                    else
                        strin = strin + "," + chkProdFund.Items[vRow].Value + "";
                }
            }
            ViewState["CatId"] = strin;
        }
    }
}