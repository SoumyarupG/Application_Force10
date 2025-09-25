using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using System.Web.Security;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class TransDisburse : CENTRUMBAse
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (((Session[gblValue.BrnchCode] == null) ? "expired" : Session[gblValue.BrnchCode].ToString()) != "0000")
                {
                    StatusButton("Exit");
                }
                else if (Session[gblValue.BrnchCode] == null)
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Session.RemoveAll();
                    Response.Redirect("~/Login.aspx?e=random");
                }
                else
                {
                    StatusButton("View");
                }

                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtVoucherDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                //StatusButton("View");

                this.LoadBankLedger();
                LoanTransBrCode();
                this.LoadGrid(1);
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Tranche Disbursement";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTransDisb);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CDisburse oDis = new CDisburse();
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);

                dt = oDis.GetTransDisbList(vFrmDt, vToDt, txtLoanNo.Text, vBrCode, Convert.ToInt32(ViewState["vBusinessId"]), pPgIndx, ref vRows);

                ViewState["dt"] = dt;
                gvData.DataSource = dt.DefaultView;
                gvData.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo <= 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                    Btn_Previous.Enabled = true;
                if (cPgNo == Int32.Parse(lblTotalPages.Text))
                    Btn_Next.Enabled = false;
                else
                    Btn_Next.Enabled = true;
            }
            finally
            {
                dt = null;
                oDis = null;
            }
        }

        private void LoadBankLedger()
        {
            DataTable dt = new DataTable();
            CVoucher oVoucher = new CVoucher();
            string vBranch = Session[gblValue.BrnchCode].ToString();
            dt = oVoucher.GetAcGenLedCB(vBranch, "B", "");
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlBank.Items.Insert(0, liSel);
        }

        private void LoanTransBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetTransDisbBranchCode();
            if (dt.Rows.Count > 0)
            {
                ddlTrBr.DataSource = dt;
                ddlTrBr.DataValueField = "BranchCode";
                ddlTrBr.DataTextField = "BranchName";
            }
            else
            {
                ddlTrBr.DataSource = null;

            }
            ddlTrBr.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlTrBr.Items.Insert(0, liSel);
        }
        private void LoadCustomer()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            string vBranch = (Request[ddlTrBr.UniqueID] as string == null) ? ddlTrBr.SelectedValue : Request[ddlTrBr.UniqueID] as string;
            DateTime vLoginDt = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            dt = oDisb.GetCustForTrans(vBranch);
            if (dt.Rows.Count > 0)
            {
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
                ddlCust.DataValueField = "CustId";
            }
            else
            {
                ViewState["LoanDtl"] = null;
                ddlCust.DataSource = null;
            }
            ddlCust.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlCust.Items.Insert(0, liSel);
        }

        //private void LoadLoanNo()
        //{
        //    ddlLoanNo.Items.Clear();
        //    DataTable dt = new DataTable();
        //    dt = (DataTable)ViewState["LoanDtl"];
        //    dt.DefaultView.RowFilter = "CustNo = '" + ddlCust.SelectedValue + "'"; //if CoLenId string
        //    DataTable dt1 = (dt.DefaultView).ToTable();

        //    if (dt1.Rows.Count > 0)
        //    {
        //        ddlLoanNo.DataSource = dt1;
        //        ddlLoanNo.DataTextField = "LoanNo";
        //        ddlLoanNo.DataValueField = "LoanId";
        //    }
        //    else
        //    {
        //        ddlLoanNo.DataSource = dt1;
        //    }
        //    ddlLoanNo.DataBind();
        //    ListItem liSel = new ListItem("<--- Select --->", "-1");
        //    ddlLoanNo.Items.Insert(0, liSel);
        //}
        private void LoadLoanNo()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            string vBranch = (Request[ddlTrBr.UniqueID] as string == null) ? ddlTrBr.SelectedValue : Request[ddlTrBr.UniqueID] as string;
            string vCustID = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            dt = oDisb.GetLoanNoForTrans(vCustID, vBranch);
            if (dt.Rows.Count > 0)
            {
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanId";
                ddlLoanNo.DataValueField = "LoanId";
            }
            else
            {
                ddlLoanNo.DataSource = null;
            }
            ddlLoanNo.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlLoanNo.Items.Insert(0, liSel);
        }

        protected void ddlTrBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLoanNo();
        }

        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable (), dt2 = new DataTable();
            CDisburse oCd = new CDisburse();
            string vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            dt = oCd.GetLoanDtlForTrans(vLoanId);
            if (dt.Rows.Count > 0)
            {
                txtLoanAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                txtRemainingAmt.Text = Convert.ToString(dt.Rows[0]["RemainingAmt"]);
                lblRepStDat.Text = Convert.ToString(dt.Rows[0]["RepayStartDt"]);

                gvTrnDtls.DataSource = null;
                dt2 = oCd.GetTrnsDtls(vLoanId);
                if (dt2.Rows.Count > 0)
                    gvTrnDtls.DataSource = dt2;
                else
                    gvTrnDtls.DataSource = null;
                gvTrnDtls.DataBind();

            }
            else
            {
                txtLoanAmt.Text = "0";
                txtRemainingAmt.Text = "0";
                txtCurrentDisbAmt.Text = "0";
                txtChequeNo.Text = string.Empty;
                txtChequeDt.Text = string.Empty;
                ddlBank.SelectedIndex = -1;
                gvTrnDtls.DataSource = null;
                gvTrnDtls.DataBind();
            }

        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            StatusButton("View");
            LoadGrid(1);
        }

        protected void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ClearControls();
            string vTransId = string.Empty, vLoanId = string.Empty;
            CDisburse oDis = new CDisburse();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            string vBrCode = Session[gblValue.BrnchCode].ToString();

            try
            {
                if (e.CommandName == "cmdShow")
                {
                    ddlLoanNo.Items.Clear();
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    LinkButton vLoanNo = (LinkButton)gvRow.FindControl("btnShow");
                    vTransId = commandArgs[0];
                    ViewState["TransId"] = vTransId;
                    vLoanId = commandArgs[1];
                    ViewState["LoanId"] = vLoanId;
                    dt = (DataTable)ViewState["dt"];

                    //*************LoanNo and LoanId bind for edit/delete*/
                    ListItem liSel = new ListItem(vLoanNo.Text, vLoanId);
                    ddlLoanNo.Items.Insert(0, liSel);


                    StatusButton("Show");
                    ds = oDis.GetTransDisDtlByLoanId(vLoanId, vTransId);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtLoanAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                        txtVoucherDt.Text = Convert.ToString(dt.Rows[0]["VoucherDt"]);
                        txtRemainingAmt.Text = Convert.ToString(dt.Rows[0]["RemainingAmt"]);
                        txtCurrentDisbAmt.Text = Convert.ToString(dt.Rows[0]["Amt"]);
                        ddlTrBr.SelectedIndex = ddlTrBr.Items.IndexOf(ddlTrBr.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));

                        ddlCust.Items.Clear();
                        ListItem liSel1 = new ListItem(dt.Rows[0]["CompanyName"].ToString(), dt.Rows[0]["CustNo"].ToString());
                        ddlCust.Items.Insert(0, liSel1);

                        ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(dt.Rows[0]["DescId"].ToString()));
                        txtChequeNo.Text = Convert.ToString(dt.Rows[0]["ChequeNo"]);
                        lblLnPrd.Text = Convert.ToString(dt.Rows[0]["ProductName"]);
                        if (txtChequeNo.Text == "")
                        {
                            txtChequeDt.Text = "";
                            ddlMode.SelectedValue = "C";
                        }
                        else
                        {
                            txtChequeDt.Text = Convert.ToString(dt.Rows[0]["ChequeDt"]);
                            ddlMode.SelectedValue = "N";
                        }
                        if (ddlMode.SelectedValue == "C")
                        {
                            txtChequeDt.Enabled = false;
                            txtChequeNo.Enabled = false;
                            ddlBank.Enabled = false;
                        }
                        else
                        {
                            txtChequeDt.Enabled = true;
                            txtChequeNo.Enabled = true;
                            ddlBank.Enabled = true;
                        }
                        ViewState["SlNo"] = gvData.Rows[gvRow.RowIndex].Cells[0].Text;
                        ViewState["MaxSlNo"] = gvData.Rows[gvData.Rows.Count - 1].Cells[0].Text;

                        gvTrnDtls.DataSource = null;
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            gvTrnDtls.DataSource = ds.Tables[1];
                        }
                        gvTrnDtls.DataBind();

                        //upMv.Update();
                        EnableControl(true);
                        tabMem.ActiveTabIndex = 1;
                        btnDelete.Enabled = true;
                    }
                }
            }
            finally
            {
                ds = null;
                dt = null;

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "false")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                StatusButton("Add");
                ClearControls();
                tabMem.ActiveTabIndex = 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 vSlNo = 0, vMaxSlNo = 0;
                vSlNo = Convert.ToInt32(ViewState["SlNo"] == null ? "0" : ViewState["SlNo"]);
                vMaxSlNo = Convert.ToInt32(ViewState["MaxSlNo"] == null ? "0" : ViewState["MaxSlNo"]);
                if (vMaxSlNo != vSlNo)
                {
                    gblFuction.MsgPopup("Only Last Disbursed Loan can be Edited");
                    return;
                }

                if (this.CanEdit == "false")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vMode = Convert.ToString(ViewState["StateEdit"]);
            if ((vMode == "Add") || (vMode == null))
                vMode = "Save";

            if (Validate() == true)
            {
                if (SaveRecords(vMode) == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    ClearControls();
                    EnableControl(false);
                    tabMem.ActiveTabIndex = 0;
                    btnShow_Click(sender, e);
                    btnSave.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabMem.ActiveTabIndex = 0;
            ClearControls();
            EnableControl(false);
            //btnExit.Enabled = true;
            //btnSave.Enabled = false;
            //btnEdit.Enabled = false;
            //btnDelete.Enabled = false;
            //btnCancel.Enabled = false;
            StatusButton("View");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 vSlNo = 0, vMaxSlNo = 0;
            vSlNo = Convert.ToInt32(ViewState["SlNo"] == null ? "0" : ViewState["SlNo"]);
            vMaxSlNo = Convert.ToInt32(ViewState["MaxSlNo"] == null ? "0" : ViewState["MaxSlNo"]);

            if (vMaxSlNo != vSlNo)
            {
                gblFuction.MsgPopup("Only Last Disbursed Loan can be Deleted");
                return;
            }
            if (vSlNo == 1)
            {
                gblFuction.MsgPopup("First Tranche Can Not Be Delete from Here. Please Delete Loan Disbursement");
                return;
            }

            if (this.CanDelete == "false")
            {
                gblFuction.MsgPopup(MsgAccess.Del);
                return;
            }
            if (SaveRecords("Delete") == true)
            {
                gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                ClearControls();
                StatusButton("Delete");
                gvData.DataSource = null;
                gvData.DataBind();
                tabMem.ActiveTabIndex = 0;
            }
        }

        private new Boolean Validate()
        {
            Boolean vResult = true;
            if (Convert.ToDecimal(txtRemainingAmt.Text) < Convert.ToDecimal(txtCurrentDisbAmt.Text))
            {
                gblFuction.AjxMsgPopup("Disburse amount should not be more than remaining amount.");
                return false;
            }
            if (ddlMode.SelectedValue == "N")
            {
                if (ddlBank.SelectedIndex <= 0)
                {
                    gblFuction.AjxMsgPopup("Please select bank name.");
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtChequeNo.Text))
                {
                    gblFuction.AjxMsgPopup("Please set cheque no.");
                    return false;
                }
                if (string.IsNullOrEmpty(this.txtChequeDt.Text))
                {
                    gblFuction.AjxMsgPopup("Please set cheque date.");
                    return false;
                }
            }
            DataTable dt1 = null;
            CDisburse oDis = new CDisburse();
            double ComRemAmt = 0;
            string vBankType = "";
            vBankType = "O";
            string vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            dt1 = oDis.GetRemCompAmtCoLnAmt(vLoanId, vBankType);
            ComRemAmt = Convert.ToDouble(dt1.Rows[0]["remAmt"]);
            if (Convert.ToDouble(txtCurrentDisbAmt.Text) > ComRemAmt)
            {
                gblFuction.AjxMsgPopup("Current Disburse Amount cannot be greater thann Remaining Disbursement Amount.");
                return false;
            }

            return vResult;
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = true;
            CDisburse oDis = new CDisburse();
            Int32 vErr = 0;
            string vBankType = string.Empty;
            double vCurrentDisbAmt = 0;
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            int userid = Convert.ToInt32(Session[gblValue.UserId]);
            string vLedger = "", vChqDt = "";

            if (txtVoucherDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Tranche Disb. Date Can Not Be Blank..");
                return false;
            }
            if (lblRepStDat.Text == "")
            {
                gblFuction.AjxMsgPopup("Repayment Date Can Not Be Blank..");
                return false;
            }
            DateTime vVoucherDt = gblFuction.setDate(txtVoucherDt.Text);
            DateTime vCollStDt = gblFuction.setDate(lblRepStDat.Text);
            if (gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) > vVoucherDt || gblFuction.setDate(Session[gblValue.FinToDt].ToString()) < vVoucherDt)
            {
                gblFuction.AjxMsgPopup("Voucher Date Should be in Financial Year..");
                return false;
            }
            //if (vVoucherDt > vCollStDt)
            //{
            //    gblFuction.AjxMsgPopup("Tranche Date Should Be Before Collection Start Date..");
            //    return false;
            //}
            if (!string.IsNullOrEmpty(this.txtCurrentDisbAmt.Text.Trim()))
            {
                vCurrentDisbAmt = Convert.ToDouble(txtCurrentDisbAmt.Text);
            }
            else
            {
                gblFuction.AjxMsgPopup("Please set disburse amount.");
                return false;
            }
            if (vCurrentDisbAmt <= 0)
            {
                gblFuction.AjxMsgPopup("Please set disburse amount.");
                return false;
            }

            if (ddlMode.SelectedValue == "C")
            {
                vLedger = "C0001";
                vChqDt = "";
            }
            else
            {
                vLedger = this.ddlBank.SelectedValue;
                vChqDt = txtChequeDt.Text;
            }

            try
            {
                string vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;


                if (Mode == "Save")
                {
                    vErr = oDis.SaveTransDisb(vLoanId, vCurrentDisbAmt, txtChequeNo.Text, gblFuction.setDate(vChqDt), vLedger,
                                                        Session[gblValue.BrnchCode].ToString(), userid, vTblMst, vTblDtl, vFinYear, Mode, vBankType,
                                                        gblFuction.setDate(txtVoucherDt.Text));
                    if (vErr == 0)
                    {
                        vResult = true;
                        this.LoadGrid(1);
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                if (Mode == "Edit")
                {
                    vErr = oDis.SaveTransDisb(vLoanId, vCurrentDisbAmt, txtChequeNo.Text, gblFuction.setDate(vChqDt), vLedger,
                                                        Session[gblValue.BrnchCode].ToString(), userid, vTblMst, vTblDtl, vFinYear, Mode, vBankType,
                                                        gblFuction.setDate(txtVoucherDt.Text));
                    if (vErr == 0)
                    {
                        vResult = true;
                        this.LoadGrid(1);
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                if (Mode == "Delete")
                {
                    vErr = oDis.SaveTransDisb(vLoanId, vCurrentDisbAmt, txtChequeNo.Text, gblFuction.setDate(vChqDt), vLedger,
                                                        Session[gblValue.BrnchCode].ToString(), userid, vTblMst, vTblDtl, vFinYear, Mode, vBankType,
                                                        gblFuction.setDate(txtVoucherDt.Text));
                    if (vErr == 0)
                    {
                        vResult = true;
                        this.LoadGrid(1);
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oDis = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabMem.ActiveTabIndex = 0;
        }

        private void ClearControls()
        {
            txtLoanAmt.Text = "0";
            txtRemainingAmt.Text = "0";
            txtCurrentDisbAmt.Text = "0";
            txtChequeNo.Text = string.Empty;
            txtChequeDt.Text = string.Empty;
            ddlBank.SelectedIndex = -1;
        }

        private void EnableControl(Boolean Status)
        {
            txtLoanAmt.Enabled = false;
            //txtTolDisAmt.Enabled = false;
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        //protected void chkCash_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkCash.Checked == true)
        //    {
        //        ddlBank.Enabled = false;
        //        txtChequeNo.Enabled = false;
        //        txtChequeDt.Enabled = false;
        //    }
        //    else
        //    {
        //        ddlBank.Enabled = true;
        //        txtChequeNo.Enabled = true;
        //        txtChequeDt.Enabled = true;
        //    }

        //}
        protected void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMode.SelectedValue.ToString()=="C")
            {
                ddlBank.Enabled = false;
                txtChequeNo.Enabled = false;
                txtChequeDt.Enabled = false;
            }
            else
            {
                ddlBank.Enabled = true;
                txtChequeNo.Enabled = true;
                txtChequeDt.Enabled = true;
            }

        }
    }
}