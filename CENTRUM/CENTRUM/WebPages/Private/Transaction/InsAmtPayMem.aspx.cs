using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM
{
    public partial class InsAmtPayMem : CENTRUMBase
    {
        protected int cPgNo = 0;
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
                ViewState["StateEdit"] = null;
                ViewState["FundTrMember"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtFndDt.Text = Session[gblValue.LoginDate].ToString();

                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                popBank();
                PopBranch();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void popBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToString(row["DescId"]) == "C0001")
                        row.Delete();
                }
                dt.AcceptChanges();
                ddlBank.DataSource = dt;
                ddlBank.DataTextField = "Desc";
                ddlBank.DataValueField = "DescId";
                ddlBank.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlBank.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            vBrCode = (string)Session[gblValue.BrnchCode];
            vBrId = Convert.ToInt32(vBrCode);
            oCG = new CGblIdGenerator();
            dt = oCG.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", vBrId, "BranchCode", "AA", vLogDt, "0000");
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
            ddlBranch.DataSource = dt;
            ddlBranch.DataTextField = "BranchName";
            ddlBranch.DataValueField = "BranchCode";
            ddlBranch.DataBind();
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlBranch.Items.Insert(0, Li);
                ddlBranch.Enabled = true;
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
                this.PageHeading = "Insurance Amount Payment To Member";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuInsAmtPayMem);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance Amount Pay To Member", false);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = ddlBranch.SelectedValue.ToString();
                    LoadGrid(txtFrmDt.Text, txtToDt.Text, vBrCode);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            if (txtFndDt.Text.Trim() != "" || txtFndDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFndDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFndDt");
                    vResult = false;
                }
            }
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            DateTime vEffDt = gblFuction.setDate(txtFndDt.Text);
            if (vFrmDt > vToDt)
            {
                gblFuction.MsgPopup("From Date can not be greater than To Date");
                vResult = false;
            }
            if (vToDt > vEffDt)
            {
                gblFuction.MsgPopup("To Date can not be greater than Fund Given Date");
                vResult = false;
            }
            if (ddlBranch.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Select the Branch");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CClaimFundTransfer oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "", vBankDescid = null, vPayType = null;
            DateTime vEffDate = gblFuction.setDate(txtFndDt.Text);
            DateTime vChqDate = gblFuction.setDate(txtChqDt.Text);
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            if (rdbPay.SelectedValue == "C")
            {
                vBankDescid = "C0001";
                vPayType = "C";
            }
            else
            {
                vBankDescid = ddlBank.SelectedValue.ToString();
                vPayType = "B";
            }
            if (this.RoleId != 1)
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vEffDate)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return ;
                    }
                }
            }
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["FundTrMember"];
                    if (dt == null)
                    {
                        gblFuction.MsgPopup("Nothing to Save");
                        return;
                    }
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CClaimFundTransfer();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.SaveGivenFund(vTblMst, vTblDtl, vFinYear, vXmlData, this.UserID, ddlBranch.SelectedValue.ToString(), vEffDate, vPayType, vBankDescid, txtCheque.Text.Replace("'", "''"), vChqDate, txtPayeeNm.Text.Replace("'", "''"));
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid(txtFrmDt.Text, txtToDt.Text, ddlBranch.SelectedValue.ToString());

                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtFndDt_TextChanged(object sender, EventArgs e)
        {
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDt = gblFuction.setDate(txtFndDt.Text);
            if (vAppDt > vLoginDt)
            {
                gblFuction.MsgPopup("Approved date should not grater than login date..");
                txtFndDt.Text = Session[gblValue.LoginDate].ToString();  //Convert.ToString(vLoginDt);
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()//To Check
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pFromDt, string pToDt, string pBranch)
        {
            DataTable dt = null;
            CClaimFundTransfer oLS = null;
            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CClaimFundTransfer();
                dt = oLS.GetAllFundTrMember(vFromDt, vToDt, vBrCode);
                ViewState["FundTrMember"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPaid_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["FundTrMember"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkPaid = (CheckBox)row.FindControl("chkPaid");
                if (chkPaid.Checked == true)
                {
                    dt.Rows[row.RowIndex]["FundGivenYN"] = "Y";
                }
                else
                {
                    dt.Rows[row.RowIndex]["FundGivenYN"] = "N";

                }
                dt.AcceptChanges();
                ViewState["FundTrMember"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["FundTrMember"];
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkClaim = (CheckBox)row.FindControl("chkClaim");
            CheckBox chkFndTra = (CheckBox)row.FindControl("chkFndTra");
            if (chkFndTra.Checked == true)
            {
                if (chkClaim.Checked == false)
                {
                    gblFuction.AjxMsgPopup(" You can not Checked the Fundtransfer as Claim not done");
                    chkFndTra.Checked = false;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["ClaimYN"] = "Y";
                    dt.Rows[row.RowIndex]["FundYN"] = "Y";
                }
            }

            dt.AcceptChanges();
            dt = (DataTable)ViewState["FundTrMember"];

            ViewState["FundTrMember"] = dt;
            upSanc.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            //Bind Approval
        //            CheckBox chkClaim = (CheckBox)e.Row.FindControl("chkClaim");
        //            CheckBox chkFndTra = (CheckBox)e.Row.FindControl("chkFndTra");

        //            if (e.Row.Cells[9].Text == "Y")
        //            {
        //                chkClaim.Checked = true;
        //            }

        //        }
        //    }
        //    finally
        //    {

        //    }
        //}

    }
}
