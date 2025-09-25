using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PettyCashReq : CENTRUMBase 
    {
        /// <summary>
        /// 
        /// </summary>
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                LoadGrid(1);
                StatusButton("View");
                tabPCash.ActiveTabIndex = 0;
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
                this.PageHeading = "Petty Cash Requisition";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPrtyCashReq);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtPcashDt.Enabled = Status;
            txtOffRent.Enabled = Status;
            txtCook.Enabled = Status;
            txtBMTravel.Enabled = Status;
            txtCookFuel.Enabled = Status;
            txtOthTravel.Enabled = Status;
            txtPrinStan.Enabled = Status;
            txtStfWfPMonth.Enabled = Status;
            txtNewsPaper.Enabled = Status;
            txtBiCycleRep.Enabled = Status;
            txtTelephone.Enabled = Status;
            txtRepMaint.Enabled = Status;
            txtElectric.Enabled = Status;
            txtTrain.Enabled = Status;
            txtOffExp.Enabled = Status;
            txtTradeLic.Enabled = Status;
            txtFurFix.Enabled = Status;
            txtOffEquip.Enabled = Status;
            txtGrossTotal.Enabled = Status;
            txtNetBalance.Enabled = Status;
            txtCloseBalance.Enabled = Status;
            txtRoundOff.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtPcashDt.Text = Session[gblValue.LoginDate].ToString();
            txtOffRent.Text = "0";
            txtCook.Text = "0";
            txtBMTravel.Text = "0";
            txtCookFuel.Text = "0";
            txtOthTravel.Text = "0";
            txtPrinStan.Text = "0";
            txtStfWfPMonth.Text = "0";
            txtNewsPaper.Text = "0";
            txtBiCycleRep.Text = "0";
            txtTelephone.Text = "0";
            txtRepMaint.Text = "0";
            txtElectric.Text = "0";
            txtTrain.Text = "0";
            txtOffExp.Text = "0";
            txtTradeLic.Text = "0";
            txtFurFix.Text = "0";
            txtOffEquip.Text = "0";
            txtGrossTotal.Text = "0";
            txtNetBalance.Text = "0";
            txtCloseBalance.Text = "0";
            txtRoundOff.Text = "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CPettyCash oPc = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oPc = new CPettyCash();
                dt = oPc.GetPettyCashPG(vBrCode, pPgIndx, ref vRows);
                gvList.DataSource = dt.DefaultView;
                gvList.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 1)
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oPc = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabPCash.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tabPCash.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtPcashDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabPCash.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }


        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            //DataTable dt = null;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vPCashId = "";
            Int32 vErr = 0;
            double OffRent = 0, BMTravel = 0, OthTravel = 0, StfWfPMonth = 0, BiCycleRep = 0, RepMaint = 0, Train = 0, TradeLic = 0;
            double OffEquip = 0, Cook = 0, CookFuel = 0, PrinStan = 0, NewsPaper = 0, Telephone = 0, Electric = 0, OffExp = 0;
            double FurFix = 0, CloseBalance = 0, GrossTotal = 0, NetBalance = 0, RoundOff = 0;
            DateTime vPCashDt = gblFuction.setDate(txtPcashDt.Text);

            CPettyCash oPc = null;
            try
            {
                if (txtOffRent.Text != "") OffRent = Convert.ToDouble(txtOffRent.Text);
                if (txtBMTravel.Text != "") BMTravel = Convert.ToDouble(txtBMTravel.Text);
                if (txtOthTravel.Text != "") OthTravel = Convert.ToDouble(txtOthTravel.Text);
                if (txtStfWfPMonth.Text != "") StfWfPMonth = Convert.ToDouble(txtStfWfPMonth.Text);
                if (txtBiCycleRep.Text != "") BiCycleRep = Convert.ToDouble(txtBiCycleRep.Text);
                if (txtRepMaint.Text != "") RepMaint = Convert.ToDouble(txtRepMaint.Text);
                if (txtTrain.Text != "") Train = Convert.ToDouble(txtTrain.Text);
                if (txtTradeLic.Text != "") TradeLic = Convert.ToDouble(txtTradeLic.Text);
                if (txtOffEquip.Text != "") OffEquip = Convert.ToDouble(txtOffEquip.Text);
                if (txtCook.Text != "") Cook = Convert.ToDouble(txtCook.Text);
                if (txtCookFuel.Text != "") CookFuel = Convert.ToDouble(txtCookFuel.Text);
                if (txtPrinStan.Text != "") PrinStan = Convert.ToDouble(txtPrinStan.Text);
                if (txtNewsPaper.Text != "") NewsPaper = Convert.ToDouble(txtNewsPaper.Text);
                if (txtTelephone.Text != "") Telephone = Convert.ToDouble(txtTelephone.Text);
                if (txtElectric.Text != "") Electric = Convert.ToDouble(txtElectric.Text);
                if (txtOffExp.Text != "") OffExp = Convert.ToDouble(txtOffExp.Text);
                if (txtFurFix.Text != "") FurFix = Convert.ToDouble(txtFurFix.Text);
                if (txtCloseBalance.Text != "") CloseBalance = Convert.ToDouble(txtCloseBalance.Text);
                if (txtGrossTotal.Text != "") GrossTotal = Convert.ToDouble(txtGrossTotal.Text);
                if (txtNetBalance.Text != "") NetBalance = Convert.ToDouble(txtNetBalance.Text);
                if (txtRoundOff.Text != "") RoundOff = Convert.ToDouble(txtRoundOff.Text);

                vBrCode = (string)Session[gblValue.BrnchCode];
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vPCashId = Convert.ToString(ViewState["PCashId"]);

                if (Mode == "Save")
                {
                    if (ValidateFields() == false) return false;
                    oPc = new CPettyCash();
                    vErr = oPc.ChkPettyCash("", vPCashDt,"A");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Petty Cash Already Entered For This Date");
                        return false;
                    }
                    //vErr = oPc.InsertPettyCash(ref vPCashId, vPCashDt, OffRent, BMTravel, OthTravel, StfWfPMonth, BiCycleRep,
                    //                    RepMaint, Train, TradeLic, OffEquip, Cook, CookFuel, PrinStan, NewsPaper, Telephone,
                    //                    Electric, OffExp, FurFix, GrossTotal, NetBalance, CloseBalance, RoundOff, vBrCode, this.UserID);
                    if (vErr == 0)
                    {
                        ViewState["PCashId"] = vPCashId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (ValidateFields() == false) return false;

                    oPc = new CPettyCash();
                    vErr = oPc.ChkPettyCash("", vPCashDt, "E");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Petty Cash Already Entered For This Date");
                        return false;
                    }
                    vErr = oPc.UpdatePettyCash(vPCashId, vPCashDt, OffRent, BMTravel, OthTravel, StfWfPMonth, BiCycleRep,
                                        RepMaint, Train, TradeLic, OffEquip, Cook, CookFuel, PrinStan, NewsPaper, Telephone,
                                        Electric, OffExp, FurFix, GrossTotal, NetBalance, CloseBalance, RoundOff, vBrCode, this.UserID, "E");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oPc = new CPettyCash();
                    vErr = oPc.UpdatePettyCash(vPCashId, vPCashDt, OffRent, BMTravel, OthTravel, StfWfPMonth, BiCycleRep,
                                        RepMaint, Train, TradeLic, OffEquip, Cook, CookFuel, PrinStan, NewsPaper, Telephone,
                                        Electric, OffExp, FurFix, GrossTotal, NetBalance, CloseBalance, RoundOff, vBrCode, this.UserID, "D");
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oPc = null;
                //dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            CApplication oCG = oCG = new CApplication();

            if (txtPcashDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Petty Cash Date cannot be empty.");
                gblFuction.focus("ctl00_cph_Main_tabPCash_pnlDtl_txtPCashDt");
                vResult = false;
            }
            if (txtGrossTotal.Text.Trim() == "" || txtGrossTotal.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Gross Total should be grater than Zero.");
                vResult = false;
            }
            if (txtNetBalance.Text.Trim() == "" || txtNetBalance.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Net Balance should be grater than Zero.");
                gblFuction.focus("ctl00_cph_Main_tabPCash_pnlDtl_txtNetBalance");
                vResult = false;
            }

            if (txtRoundOff.Text.Trim() == "" || txtRoundOff.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Round-off should be grater than Zero.");
                gblFuction.focus("ctl00_cph_Main_tabPCash_pnlDtl_txtRoundOff");
                vResult = false;
            }

            return vResult;
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vPCashId = "";
            DataTable dt = null;
            CPettyCash oPc = null;
            try
            {
                vPCashId = Convert.ToString(e.CommandArgument);
                ViewState["PCashId"] = vPCashId;
                if (e.CommandName == "cmdShow")
                {
                    oPc = new CPettyCash();
                    dt = oPc.GetPettyCashDtl(vPCashId);
                    if (dt.Rows.Count > 0)
                    {
                        txtPcashDt.Text = dt.Rows[0]["PcashDt"].ToString();
                        txtOffRent.Text = dt.Rows[0]["OffRent"].ToString();
                        txtBMTravel.Text = dt.Rows[0]["BMTravel"].ToString();
                        txtOthTravel.Text = dt.Rows[0]["OthTravel"].ToString();
                        txtStfWfPMonth.Text = dt.Rows[0]["StfWfPMonth"].ToString();
                        txtBiCycleRep.Text = dt.Rows[0]["BiCycleRep"].ToString();
                        txtRepMaint.Text = dt.Rows[0]["RepMaint"].ToString();
                        txtTrain.Text = dt.Rows[0]["Train"].ToString();
                        txtTradeLic.Text = dt.Rows[0]["TradeLic"].ToString();
                        txtOffEquip.Text = dt.Rows[0]["OffEquip"].ToString();
                        txtCook.Text = dt.Rows[0]["Cook"].ToString();
                        txtCookFuel.Text = dt.Rows[0]["CookFuel"].ToString();
                        txtPrinStan.Text = dt.Rows[0]["PrinStan"].ToString();
                        txtNewsPaper.Text = dt.Rows[0]["NewsPaper"].ToString();
                        txtTelephone.Text = dt.Rows[0]["Telephone"].ToString();
                        txtElectric.Text = dt.Rows[0]["Electric"].ToString();
                        txtOffExp.Text = dt.Rows[0]["OffExp"].ToString();
                        txtFurFix.Text = dt.Rows[0]["FurFix"].ToString();
                        txtGrossTotal.Text = dt.Rows[0]["GrossTotal"].ToString();
                        txtNetBalance.Text = dt.Rows[0]["NetBalance"].ToString();
                        txtCloseBalance.Text = dt.Rows[0]["CloseBalance"].ToString();
                        txtRoundOff.Text = dt.Rows[0]["RoundOff"].ToString();
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabPCash.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPc = null;
            }
        }


        private void CalculateTot()
        {
            double OffRent = 0, BMTravel = 0, OthTravel = 0, StfWfPMonth = 0, BiCycleRep = 0, RepMaint = 0, Train = 0, TradeLic = 0;
            double OffEquip = 0, Cook = 0, CookFuel = 0, PrinStan = 0, NewsPaper = 0, Telephone = 0, Electric = 0, OffExp = 0;
            double FurFix = 0, CloseBalance = 0, GrossTotal = 0, NetBalance = 0, RoundOff = 0;

            if (txtOffRent.Text != "") OffRent = Convert.ToDouble(txtOffRent.Text);
            if (txtBMTravel.Text != "") BMTravel = Convert.ToDouble(txtBMTravel.Text);
            if (txtOthTravel.Text != "") OthTravel = Convert.ToDouble(txtOthTravel.Text);
            if (txtStfWfPMonth.Text != "") StfWfPMonth = Convert.ToDouble(txtStfWfPMonth.Text);
            if (txtBiCycleRep.Text != "") BiCycleRep = Convert.ToDouble(txtBiCycleRep.Text);
            if (txtRepMaint.Text != "") RepMaint = Convert.ToDouble(txtRepMaint.Text);
            if (txtTrain.Text != "") Train = Convert.ToDouble(txtTrain.Text);
            if (txtTradeLic.Text != "") TradeLic = Convert.ToDouble(txtTradeLic.Text);
            if (txtOffEquip.Text != "") OffEquip = Convert.ToDouble(txtOffEquip.Text);
            if (txtCook.Text != "") Cook = Convert.ToDouble(txtCook.Text);
            if (txtCookFuel.Text != "") CookFuel = Convert.ToDouble(txtCookFuel.Text);
            if (txtPrinStan.Text != "") PrinStan = Convert.ToDouble(txtPrinStan.Text);
            if (txtNewsPaper.Text != "") NewsPaper = Convert.ToDouble(txtNewsPaper.Text);
            if (txtTelephone.Text != "") Telephone = Convert.ToDouble(txtTelephone.Text);
            if (txtElectric.Text != "") Electric = Convert.ToDouble(txtElectric.Text);
            if (txtOffExp.Text != "") OffExp = Convert.ToDouble(txtOffExp.Text);
            if (txtFurFix.Text != "") FurFix = Convert.ToDouble(txtFurFix.Text);
            if (txtCloseBalance.Text != "") CloseBalance = Convert.ToDouble(txtCloseBalance.Text);

            GrossTotal = OffRent + BMTravel + OthTravel + StfWfPMonth + BiCycleRep + RepMaint + Train + TradeLic + OffEquip + Cook + CookFuel + PrinStan + NewsPaper + Telephone + Electric + OffExp + FurFix;
            txtGrossTotal.Text = Convert.ToString(GrossTotal);
            NetBalance = GrossTotal - CloseBalance;
            txtNetBalance.Text = Convert.ToString(NetBalance);
            RoundOff = Math.Round(NetBalance);
            txtRoundOff.Text =Convert.ToString(RoundOff);

        }

    }
}
