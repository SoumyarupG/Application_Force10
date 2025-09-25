using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Master
{
    public partial class LoanParameter : CENTRUMBAse
    {
        protected int cPgNo = 1;

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
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                //StatusButton("View");
                LoadGrid(0);
                PopAssets();
                PopIncome();
                PopExpenses();
                PopIncomeExpense();
                PopIncomeLiability();
                PopLiability();
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
                this.PageHeading = "Loan Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanParam);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    {
                        btnAdd.Visible = false;
                        btnEdit.Visible = false;
                        btnCancel.Visible = false;
                        btnSave.Visible = false;
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    {
                        btnEdit.Visible = false;
                        //btnCancel.Visible = false;
                        //btnSave.Visible = false;
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                    {
                        //btnCancel.Visible = false;
                        //btnSave.Visible = false;
                    }
                    else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                    {
                    }
                    else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    {
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Parameter Master", false);
                    }
                }
                else
                {
                    btnCancel.Visible = true;
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnSave.Visible = false;
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
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oLS = null;
            Int32 vRows = 0;
            try
            {
                oLS = new CLoanScheme();
                dt = oLS.GetActiveLnSchemePG(pPgIndx, ref vRows, txtLnTypNm.Text);
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
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
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotalPages
                    break;
            }
            LoadGrid(cPgNo);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopAssets()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vAssets = 1;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
                ListItem Lst1 = new ListItem("<--- Select --->", "-1");
                ddlLoanAc.DataTextField = "Desc";
                ddlLoanAc.DataValueField = "DescId";
                ddlLoanAc.DataSource = dt;
                ddlLoanAc.DataBind();
                ddlLoanAc.Items.Insert(0, Lst1);

                ddlIntAccAc.DataTextField = "Desc";
                ddlIntAccAc.DataValueField = "DescId";
                ddlIntAccAc.DataSource = dt;
                ddlIntAccAc.DataBind();
                ddlIntAccAc.Items.Insert(0, Lst1);

                ddlODIntRec.DataTextField = "Desc";
                ddlODIntRec.DataValueField = "DescId";
                ddlODIntRec.DataSource = dt;
                ddlODIntRec.DataBind();
                ddlODIntRec.Items.Insert(0, Lst1);

                ddlDefODIntRec.DataTextField = "Desc";
                ddlDefODIntRec.DataValueField = "DescId";
                ddlDefODIntRec.DataSource = dt;
                ddlDefODIntRec.DataBind();
                ddlDefODIntRec.Items.Insert(0, Lst1);

                ddlSusIntInc.DataTextField = "Desc";
                ddlSusIntInc.DataValueField = "DescId";
                ddlSusIntInc.DataSource = dt;
                ddlSusIntInc.DataBind();
                ddlSusIntInc.Items.Insert(0, Lst1);
                
            }
            finally
            {
                dt = null;
                oGen = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopIncome()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vIncome = 3;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncome);
                ListItem Lst2 = new ListItem("<--- Select --->", "-1");
                ddlLnIntAc.DataTextField = "Desc";
                ddlLnIntAc.DataValueField = "DescId";
                ddlLnIntAc.DataSource = dt;
                ddlLnIntAc.DataBind();
                ddlLnIntAc.Items.Insert(0, Lst2);

                ListItem Lst4 = new ListItem("<--- Select --->", "-1");
                ddlProcFeeAc.DataTextField = "Desc";
                ddlProcFeeAc.DataValueField = "DescId";
                ddlProcFeeAc.DataSource = dt;
                ddlProcFeeAc.DataBind();
                ddlProcFeeAc.Items.Insert(0, Lst4);

                ddlExcesAc.DataTextField = "Desc";
                ddlExcesAc.DataValueField = "DescId";
                ddlExcesAc.DataSource = dt;
                ddlExcesAc.DataBind();
                ddlExcesAc.Items.Insert(0, Lst4);

                ListItem Lst5 = null;
                Lst5 = new ListItem("<--- Select --->", "-1");
                ddlWoffRec.DataTextField = "Desc";
                ddlWoffRec.DataValueField = "DescId";
                ddlWoffRec.DataSource = dt;
                ddlWoffRec.DataBind();
                ddlWoffRec.Items.Insert(0, Lst5);

                Lst5 = new ListItem("<--- Select --->", "-1");
                ddlPenChrge.DataTextField = "Desc";
                ddlPenChrge.DataValueField = "DescId";
                ddlPenChrge.DataSource = dt;
                ddlPenChrge.DataBind();
                ddlPenChrge.Items.Insert(0, Lst5);

                Lst5 = new ListItem("<--- Select --->", "-1");
                ddlPenIntAc.DataTextField = "Desc";
                ddlPenIntAc.DataValueField = "DescId";
                ddlPenIntAc.DataSource = dt;
                ddlPenIntAc.DataBind();
                ddlPenIntAc.Items.Insert(0, Lst5);

                ListItem Lst6 = null;
                Lst6 = new ListItem("<--- Select --->", "-1");
                ddlBounceChrgAC.DataTextField = "Desc";
                ddlBounceChrgAC.DataValueField = "DescId";
                ddlBounceChrgAC.DataSource = dt;
                ddlBounceChrgAC.DataBind();
                ddlBounceChrgAC.Items.Insert(0, Lst6);

                Lst6 = new ListItem("<--- Select --->", "-1");
                ddlOthFees.DataTextField = "Desc";
                ddlOthFees.DataValueField = "DescId";
                ddlOthFees.DataSource = dt;
                ddlOthFees.DataBind();
                ddlOthFees.Items.Insert(0, Lst6);

                dt = oGen.GetLedgerByAcHeadId(vGenAcType, 4);
                ListItem Lst7 = new ListItem("<--- Select --->", "-1");
                ddlPAI.DataTextField = "Desc";
                ddlPAI.DataValueField = "DescId";
                ddlPAI.DataSource = dt;
                ddlPAI.DataBind();
                ddlPAI.Items.Insert(0, Lst7);

                ListItem Lst8 = new ListItem("<--- Select --->", "-1");
                ddlDisbLedger.DataTextField = "Desc";
                ddlDisbLedger.DataValueField = "DescId";
                ddlDisbLedger.DataSource = dt;
                ddlDisbLedger.DataBind();
                ddlDisbLedger.Items.Insert(0, Lst8);

                

            }
            finally
            {
                dt = null;
                oGen = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopExpenses()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vExp = 2;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vExp);
                ListItem Lst16 = new ListItem("<--- Select --->", "-1");
                ddlWoff.DataTextField = "Desc";
                ddlWoff.DataValueField = "DescId";
                ddlWoff.DataSource = dt;
                ddlWoff.DataBind();
                ddlWoff.Items.Insert(0, Lst16);
            }
            finally
            {
                dt = null;
                oGen = null;
            }
        }


        private void PopIncomeExpense()
        {
            DataTable dt = null;
            string vGenAcType = "W";            // for Income  and Expense
            Int32 vIncLi = 0;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncLi);

            ListItem Lst16 = new ListItem();
            Lst16.Text = "<--- Select --->";
            Lst16.Value = "-1";
            ddlBounceChrgWaveAC.DataTextField = "Desc";
            ddlBounceChrgWaveAC.DataValueField = "DescId";
            ddlBounceChrgWaveAC.DataSource = dt;
            ddlBounceChrgWaveAC.DataBind();
            ddlBounceChrgWaveAC.Items.Insert(0, Lst16);

            ListItem Lst17 = new ListItem();
            Lst17.Text = "<--- Select --->";
            Lst17.Value = "-1";
            ddlPreCloseChrgeAC.DataTextField = "Desc";
            ddlPreCloseChrgeAC.DataValueField = "DescId";
            ddlPreCloseChrgeAC.DataSource = dt;
            ddlPreCloseChrgeAC.DataBind();
            ddlPreCloseChrgeAC.Items.Insert(0, Lst17);

            ListItem Lst18 = new ListItem();
            Lst18.Text = "<--- Select --->";
            Lst18.Value = "-1";
            ddlPreCloseChrgeWaiveAC.DataTextField = "Desc";
            ddlPreCloseChrgeWaiveAC.DataValueField = "DescId";
            ddlPreCloseChrgeWaiveAC.DataSource = dt;
            ddlPreCloseChrgeWaiveAC.DataBind();
            ddlPreCloseChrgeWaiveAC.Items.Insert(0, Lst18);
        }

        private void PopIncomeLiability()
        {
            DataTable dt = null;
            string vGenAcType = "Z";            // for Income  and liability
            Int32 vIncLi = 0;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncLi);
            ListItem Lst2 = new ListItem();
            Lst2.Text = "<--- Select --->";
            Lst2.Value = "-1";

            ddlMediClaimAc.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlMediClaimAc.DataSource = dt;
                ddlMediClaimAc.DataTextField = "Desc";
                ddlMediClaimAc.DataValueField = "DescId";
            }
            else
            {
                ddlMediClaimAc.DataSource = null;
            }
            ddlMediClaimAc.DataBind();
            ddlMediClaimAc.Items.Insert(0, Lst2);
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopLiability()
        {
            DataTable dtLib = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vLib = 4;
            try
            {
                oGen = new CGenParameter();
                dtLib = oGen.GetLedgerByAcHeadId(vGenAcType, vLib);
                ListItem Lst5 = new ListItem("<--- Select --->", "-1");
                ddlInsure.DataTextField = "Desc";
                ddlInsure.DataValueField = "DescId";
                ddlInsure.DataSource = dtLib;
                ddlInsure.DataBind();
                ddlInsure.Items.Insert(0, Lst5);

                ListItem Lst7 = null;
                Lst7 = new ListItem("<--- Select --->", "-1");
                ddlServTax.DataTextField = "Desc";
                ddlServTax.DataValueField = "DescId";
                ddlServTax.DataSource = dtLib;
                ddlServTax.DataBind();
                ddlServTax.Items.Insert(0, Lst7);

                Lst7 = new ListItem("<--- Select --->", "-1");
                ddlInsuCGSTAC.DataTextField = "Desc";
                ddlInsuCGSTAC.DataValueField = "DescId";
                ddlInsuCGSTAC.DataSource = dtLib;
                ddlInsuCGSTAC.DataBind();
                ddlInsuCGSTAC.Items.Insert(0, Lst7);

                Lst7 = new ListItem("<--- Select --->", "-1");
                ddlInsuSGSTAC.DataTextField = "Desc";
                ddlInsuSGSTAC.DataValueField = "DescId";
                ddlInsuSGSTAC.DataSource = dtLib;
                ddlInsuSGSTAC.DataBind();
                ddlInsuSGSTAC.Items.Insert(0, Lst7);

                ListItem Lst8 = new ListItem("<--- Select --->", "-1");
                ddlMedAc.DataTextField = "Desc";
                ddlMedAc.DataValueField = "DescId";
                ddlMedAc.DataSource = dtLib;
                ddlMedAc.DataBind();
                ddlMedAc.Items.Insert(0, Lst8);

                ListItem Lst9 = new ListItem("<--- Select --->", "-1");
                ddlSGSTAc.DataTextField = "Desc";
                ddlSGSTAc.DataValueField = "DescId";
                ddlSGSTAc.DataSource = dtLib;
                ddlSGSTAc.DataBind();
                ddlSGSTAc.Items.Insert(0, Lst9);

                //ListItem Lst10 = new ListItem("<--- Select --->", "-1");
                ddlAdvAc.DataTextField = "Desc";
                ddlAdvAc.DataValueField = "DescId";
                ddlAdvAc.DataSource = dtLib;
                ddlAdvAc.DataBind();
                ddlAdvAc.Items.Insert(0, Lst9);               

                ddlDefSusIntInc.DataTextField = "Desc";
                ddlDefSusIntInc.DataValueField = "DescId";
                ddlDefSusIntInc.DataSource = dtLib;
                ddlDefSusIntInc.DataBind();
                ddlDefSusIntInc.Items.Insert(0, Lst9);
            }
            finally
            {
                dtLib = null;
                oGen = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlLoanAc.SelectedIndex = -1;
            ddlLnIntAc.SelectedIndex = -1;
            ddlPenIntAc.SelectedIndex = -1;
            ddlProcFeeAc.SelectedIndex = -1;
            ddlInsure.SelectedIndex = -1;
            ddlWoff.SelectedIndex = -1;
            ddlServTax.SelectedIndex = -1;
            ddlSGSTAc.SelectedIndex = -1;
            ddlOthFees.SelectedIndex = -1;
            ddlPAI.SelectedIndex = -1;
            ddlDisbLedger.SelectedIndex = -1;
            ddlMedAc.SelectedIndex = -1;
            ddlExcesAc.SelectedIndex = -1;

            ddlInsuCGSTAC.SelectedIndex = -1;
            ddlInsuSGSTAC.SelectedIndex = -1;
            ddlPenChrge.SelectedIndex = -1;
            ddlBounceChrgAC.SelectedIndex = -1;
            ddlBounceChrgWaveAC.SelectedIndex = -1;
            ddlPreCloseChrgeAC.SelectedIndex = -1;
            ddlPreCloseChrgeWaiveAC.SelectedIndex = -1;
            ddlMediClaimAc.SelectedIndex = -1;

            txtProcFee.Text = "0.0";
            txtInsure.Text = "0.0";
            txtSrvTax.Text = "0.0";
            txtSGST.Text = "0.0";
            txtInstall.Text = "0.0";
            txtOthFees.Text = "0.0";
            txtPAI.Text = "0.0";
            txtMedAmt.Text = "0.0";
            rdbSchedule.SelectedValue = "A";
            txtIntRate.Text = "0.0";
            txtPenInt.Text = "0.0";
            txtIntPeriod.Text = "0";
            ddlPaySchedul.SelectedIndex = -1;
            ddlLnIntTyp.SelectedIndex = -1;
            txtIntsNo.Text = "0";
            //txtIntsNo.Enabled = false;
            chkInActv.Checked = false;
            txtProsFee.Text = "0";
            txtServTax.Text = "0";
            chkIsTopUp.Checked = false;
            ddlWoffRec.SelectedIndex = -1;
            txtInActiveDate.Text = "";
            txtEffDate.Text = "";
            ckhAllowAdv.Checked = false;
            ChkVerify.Checked = false;
            ddlIntAccAc.SelectedIndex = -1;
            ddlAdvAc.SelectedIndex = -1;
            ddlODIntRec.SelectedIndex = -1;
            ddlSusIntInc.SelectedIndex = -1;
            chkPreAppLoan.Checked = false;
            txtInstallmentRange.Text = "";
            ddlDefSusIntInc.SelectedIndex = -1;
            ddlDefODIntRec.SelectedIndex = -1;
            chkIsOpenMkt.Checked = false;
            txtBounceCharges.Text = "0";
            ddlBounceChargesGSTType.SelectedIndex = -1;
            txtVisitCharges.Text = "0";
            ddlVisitChargesGSTType.SelectedIndex = -1;
            txtMinAmtReschudle.Text = "0";
            txtMaxAdvAmt.Text = "0";
            ddlPenalChargesGSTType.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {
            ddlLoanAc.Enabled = Status;
            ddlLnIntAc.Enabled = Status;
            ddlPenIntAc.Enabled = Status;
            ddlProcFeeAc.Enabled = Status;
            ddlExcesAc.Enabled = Status;
            ddlInsure.Enabled = Status;
            ddlWoff.Enabled = Status;
            ddlServTax.Enabled = Status;
            ddlSGSTAc.Enabled = Status;
            ddlOthFees.Enabled = Status;
            ddlPAI.Enabled = Status;
            ddlDisbLedger.Enabled = Status;
            ddlMedAc.Enabled = Status;
            txtProcFee.Enabled = Status;
            txtInsure.Enabled = Status;
            txtSrvTax.Enabled = Status;
            txtSGST.Enabled = Status;
            txtInstall.Enabled = Status;
            txtOthFees.Enabled = Status;
            txtPAI.Enabled = Status;
            rdbSchedule.Enabled = Status;
            txtIntRate.Enabled = Status;
            txtPenInt.Enabled = Status;
            txtIntPeriod.Enabled = Status;
            ddlPaySchedul.Enabled = Status;
            ddlLnIntTyp.Enabled = Status;
            txtIntsNo.Enabled = Status;
            chkInActv.Enabled = Status;
            txtProsFee.Enabled = Status;
            txtServTax.Enabled = Status;
            gvSchedule.Enabled = false;
            chkIsTopUp.Enabled = Status;
            ddlWoffRec.Enabled = Status;
            txtInActiveDate.Enabled = false;
            ckhAllowAdv.Enabled = Status;
            ChkVerify.Enabled = Status;
            txtEffDate.Enabled = Status;
            txtMedAmt.Enabled = Status;
            ddlAdvAc.Enabled = Status;
            ddlIntAccAc.Enabled = Status;
            ddlODIntRec.Enabled = Status;
            ddlSusIntInc.Enabled = Status;
            chkPreAppLoan.Enabled = Status;
            txtInstallmentRange.Enabled = Status;
            ddlDefSusIntInc.Enabled = Status;
            ddlDefODIntRec.Enabled = Status;


            ddlInsuCGSTAC.Enabled = Status;
            ddlInsuSGSTAC.Enabled = Status;
            ddlPenChrge.Enabled = Status;
            ddlBounceChrgAC.Enabled = Status;
            ddlBounceChrgWaveAC.Enabled = Status;
            ddlPreCloseChrgeAC.Enabled = Status;
            ddlPreCloseChrgeWaiveAC.Enabled = Status;
            ddlMediClaimAc.Enabled = Status;
            chkIsOpenMkt.Enabled = Status;

            txtBounceCharges.Enabled = Status;
            ddlBounceChargesGSTType.Enabled = Status;
            txtVisitCharges.Enabled = Status;
            ddlVisitChargesGSTType.Enabled = Status;
            txtMinAmtReschudle.Enabled = Status;
            txtMaxAdvAmt.Enabled = Status;
            ddlPenalChargesGSTType.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField()
        {
            bool vRes = true;
            DataTable dt = (DataTable)ViewState["Manual"];
            int vMaxRow = Convert.ToInt32(txtIntsNo.Text.Trim());
            if (txtProcFee.Text != "" && Convert.ToDouble(txtProcFee.Text) != 0 && ddlProcFeeAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Processing Fee A/c Can not be Empty");
                return vRes = false;
            }
            //if (ddlInsure.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Insurance A/c Can not be Empty");
            //    return vRes = false;
            //}
            if (txtSrvTax.Text != "" && txtSrvTax.Text != "0.0" && ddlServTax.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("CGST A/c Can not be Empty");
                return vRes = false;
            }
            if (txtSGST.Text != "" && txtSGST.Text != "0.0" && ddlSGSTAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("SGST A/c Can not be Empty");
                return vRes = false;
            }
            if (txtOthFees.Text != "" && Convert.ToDouble(txtOthFees.Text) != 0 && ddlOthFees.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Others Fees A/c Can not be Empty");
                return vRes = false;
            }
            //if (txtMedAmt.Text != "" && Convert.ToDouble(txtMedAmt.Text) != 0 && ddlMedAc.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Mediclaim A/c Can not be Empty");
            //    return vRes = false;
            //}
            if (txtPAI.Text != "" && Convert.ToDouble(txtPAI.Text) != 0 && ddlPAI.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("P.A.I A/c Can not be Empty");
                return vRes = false;
            }
            if (dt != null && dt.Rows.Count > 0 && rdbSchedule.SelectedValue == "M")
            {
                if (Convert.ToString(dt.Rows[vMaxRow - 1]["Total"]) == "" && rdbSchedule.SelectedValue == "M")
                {
                    gblFuction.MsgPopup("Please Check and Correct Schedule");
                    return vRes = false;
                }
            }
            if (dt != null && dt.Rows.Count > 0 && Convert.ToDouble(lblAmount.Text.Trim()) != Convert.ToDouble(txtPrinTot.Text.Trim()) && rdbSchedule.SelectedValue == "M")
            {
                gblFuction.MsgPopup("Please Check and Correct Schedule");
                return vRes = false;
            }
            if (chkInActv.Checked == true)
            {
                if (txtInActiveDate.Text == "")
                {
                    gblFuction.MsgPopup("Inactive Date should not be blank..");
                    return vRes = false;
                }
            }
            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dtXml = (DataTable)ViewState["Manual"];
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vActv = "", vXml = "", vTopup = "", vAllowAdv = "", vVerify = "", vIndividual = "", vPreAppLoan = "", vOpenMkt = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0, vIntPrd = 0, vInstNo = 0;
            double vProc = 0, vInsure = 0, vSrvTax = 0, vSGST = 0, vInstal = 0, vIntrate = 0, vPenInt = 0, vInstSize = 0, 
                vAppProc = 0, vAppServ = 0, vOthFees = 0, vPAIAmt = 0, vMedAmt = 0,vMinAmtReschudle = 0, vMaxAdvAmt = 0;
            DateTime vInactiveDate = gblFuction.setDate("");
            CGenParameter oGP = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ValidateField() == false)
                    return false;
                if (rdbSchedule.SelectedValue == "M")
                {
                    using (StringWriter oSW = new StringWriter())
                    {
                        dtXml.WriteXml(oSW);
                        vXml = oSW.ToString();
                    }
                }

                if (chkInActv.Checked == true)
                {
                    vActv = "N";
                    vInactiveDate = gblFuction.setDate(txtInActiveDate.Text);
                }
                else
                {
                    vActv = "Y";
                }
                if (chkIsTopUp.Checked == true)
                    vTopup = "Y";
                else
                    vTopup = "N";

                if (chkIsOpenMkt.Checked == true)
                    vOpenMkt = "Y";
                else
                    vOpenMkt = "N";


                if (ckhAllowAdv.Checked == true)
                    vAllowAdv = "Y";
                else
                    vAllowAdv = "N";

                if (ChkVerify.Checked == true)
                    vVerify = "Y";
                else
                    vVerify = "N";

                if (chkIsIndividual.Checked == true)
                    vIndividual = "Y";
                else
                    vIndividual = "N";

                if (txtProcFee.Text.Trim() != "")
                    vProc = Convert.ToDouble(txtProcFee.Text.Trim());
                else
                    vProc = 0.0;

                if (txtInsure.Text.Trim() != "")
                    vInsure = Convert.ToDouble(txtInsure.Text.Trim());
                else
                    vInsure = 0.0;

                if (txtInstall.Text.Trim() != "")
                    vInstSize = Convert.ToDouble(txtInstall.Text.Trim());
                else
                    vInstSize = 0.0;

                if (txtSrvTax.Text.Trim() != "")
                    vSrvTax = Convert.ToDouble(txtSrvTax.Text.Trim());
                else
                    vSrvTax = 0.0;

                if (txtSGST.Text.Trim() != "")
                    vSGST = Convert.ToDouble(txtSGST.Text.Trim());
                else
                    vSGST = 0.0;

                if (txtInstall.Text.Trim() != "")
                    vInstal = Convert.ToDouble(txtInstall.Text.Trim());
                else
                    vInstal = 0.0;

                if (txtProsFee.Text.Trim() != "")
                    vAppProc = Convert.ToDouble(txtProsFee.Text.Trim());
                else
                    vAppProc = 0.0;

                if (txtIntRate.Text.Trim() != "")
                    vIntrate = Convert.ToDouble(txtIntRate.Text.Trim());
                else
                    vIntrate = 0.0;

                if (txtPenInt.Text.Trim() != "")
                    vPenInt = Convert.ToDouble(txtPenInt.Text.Trim());
                else
                    vPenInt = 0.0;

                if (txtServTax.Text.Trim() != "")
                    vAppServ = Convert.ToDouble(txtServTax.Text.Trim());
                else
                    vAppServ = 0.0;

                if (txtIntPeriod.Text.Trim() != "")
                    vIntPrd = Convert.ToInt32(txtIntPeriod.Text.Trim());

                if (txtIntsNo.Text.Trim() != "")
                    vInstNo = Convert.ToInt32(txtIntsNo.Text.Trim());

                if (txtOthFees.Text.Trim() != "")
                    vOthFees = Convert.ToDouble(txtOthFees.Text.Trim());
                else
                    vOthFees = 0.0;

                if (txtPAI.Text.Trim() != "")
                    vPAIAmt = Convert.ToDouble(txtPAI.Text.Trim());
                else
                    vPAIAmt = 0.0;
                if (txtMedAmt.Text.Trim() != "")
                    vMedAmt = Convert.ToDouble(txtMedAmt.Text.Trim());
                else
                    vMedAmt = 0.0;

                if (chkPreAppLoan.Checked == true)
                    vPreAppLoan = "Y";
                else
                    vPreAppLoan = "N";

                if (txtMinAmtReschudle.Text.Trim() != "")
                    vMinAmtReschudle = Convert.ToDouble(txtMinAmtReschudle.Text.Trim());

                if (txtMaxAdvAmt.Text.Trim() != "")
                    vMaxAdvAmt = Convert.ToDouble(txtMaxAdvAmt.Text.Trim());

                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);
                if (Mode == "Save")
                {
                    oGP = new CGenParameter();
                    this.GetModuleByRole(mnuID.mnuLoanParam);
                    vErr = oGP.InsertLnParameter(vLoanTypeId, vIntrate, vIntPrd, ddlPaySchedul.SelectedValue, ddlLnIntTyp.SelectedValue, vInstNo, vProc,
                        vInsure, vSrvTax, vSGST, rdbSchedule.SelectedValue, ddlLoanAc.SelectedValue, ddlLnIntAc.SelectedValue,
                        ddlProcFeeAc.SelectedValue, ddlWoff.SelectedValue, vInstSize, vAppProc, vAppServ, ddlServTax.SelectedValue, ddlSGSTAc.SelectedValue, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), vActv,
                        ddlInsure.SelectedValue, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vTopup, ddlWoffRec.SelectedValue, vAllowAdv,
                        ddlOthFees.SelectedValue, vOthFees, ddlPAI.SelectedValue, vPAIAmt, ddlDisbLedger.SelectedValue, gblFuction.setDate(txtEffDate.Text),
                        vVerify, vMedAmt, ddlMediClaimAc.SelectedValue, ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue, ddlExcesAc.SelectedValue, ddlODIntRec.SelectedValue,
                        ddlSusIntInc.SelectedValue, vIndividual, vPreAppLoan, txtInstallmentRange.Text, ddlDefSusIntInc.SelectedValue, ddlDefODIntRec.SelectedValue,
                        ddlInsuCGSTAC.SelectedValue,ddlInsuSGSTAC.SelectedValue,ddlBounceChrgAC.SelectedValue,ddlBounceChrgWaveAC.SelectedValue,
                        ddlPreCloseChrgeAC.SelectedValue, ddlPreCloseChrgeWaiveAC.SelectedValue, ddlPenChrge.SelectedValue, vOpenMkt, ddlPenIntAc.SelectedValue, vPenInt,
                        Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue, Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue
                        , vMinAmtReschudle, vMaxAdvAmt, ddlPenalChargesGSTType.SelectedValue);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["LoanTypeId"] = vLoanTypeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oGP = new CGenParameter();
                    this.GetModuleByRole(mnuID.mnuLoanParam);
                    vErr = oGP.UpdateParameter(vLoanTypeId, vIntrate, vIntPrd, ddlPaySchedul.SelectedValue, ddlLnIntTyp.SelectedValue, vInstNo, vProc,
                        vInsure, vSrvTax, vSGST, rdbSchedule.SelectedValue, ddlLoanAc.SelectedValue, ddlLnIntAc.SelectedValue,
                        ddlProcFeeAc.SelectedValue, ddlWoff.SelectedValue, vInstSize, vAppProc, vAppServ, ddlServTax.SelectedValue, ddlSGSTAc.SelectedValue, vXml, Convert.ToInt32(Session[gblValue.UserId].ToString()), vActv,
                        ddlInsure.SelectedValue, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vTopup, ddlWoffRec.SelectedValue, vInactiveDate, vAllowAdv,
                        ddlOthFees.SelectedValue, vOthFees, ddlPAI.SelectedValue, vPAIAmt, ddlDisbLedger.SelectedValue, gblFuction.setDate(txtEffDate.Text), vVerify, vMedAmt,
                        ddlMediClaimAc.SelectedValue, ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue, ddlExcesAc.SelectedValue, ddlODIntRec.SelectedValue,
                        ddlSusIntInc.SelectedValue, vIndividual, vPreAppLoan, txtInstallmentRange.Text, ddlDefSusIntInc.SelectedValue, ddlDefODIntRec.SelectedValue,
                         ddlInsuCGSTAC.SelectedValue, ddlInsuSGSTAC.SelectedValue, ddlBounceChrgAC.SelectedValue, ddlBounceChrgWaveAC.SelectedValue,
                        ddlPreCloseChrgeAC.SelectedValue, ddlPreCloseChrgeWaiveAC.SelectedValue, ddlPenChrge.SelectedValue, vOpenMkt, ddlPenIntAc.SelectedValue, vPenInt,
                        Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue, Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue
                        , vMinAmtReschudle, vMaxAdvAmt, ddlPenalChargesGSTType.SelectedValue);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vLoanTypeId, "LoanTypeId", "LoanApplication");
                    if (vRec <= 0)
                    {
                        oGP = new CGenParameter();
                        vErr = oGP.DeleteParameter(vLoanTypeId);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oGP = null;
                oGbl = null;
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
            ViewState["LoanTypeId"] = null;
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
                StatusButton("Add");
                ClearControls();
                //txtIntsNo.Text = Convert.ToString(ViewState["InstNo"]);
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                if (rdbSchedule.SelectedValue == "M")
                    gvSchedule.Enabled = true;
                if (chkInActv.Checked == false)
                    txtInActiveDate.Enabled = false;
                else
                    txtInActiveDate.Enabled = true;
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("Show");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnScheme_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lblPrd = (LinkButton)e.Row.FindControl("btnShow");
                if (e.Row.Cells[2].Text == "Y")
                    lblPrd.ForeColor = System.Drawing.Color.Blue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnScheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vLnTypId = 0;
            DataTable dt = null;
            DataTable dtDtl = null;
            CGenParameter oGP = null;
            double vInstAmt = 0.0;
            ViewState["InstNo"] = null;
            try
            {
                vLnTypId = Convert.ToInt32(e.CommandArgument);
                ViewState["LoanTypeId"] = vLnTypId;
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                vInstAmt = Convert.ToDouble(gvRow.Cells[3].Text);
                if (vInstAmt <= 20000)
                    ViewState["InstNo"] = "12";
                else if (vInstAmt > 20000)
                    ViewState["InstNo"] = "24";
                foreach (GridViewRow gr in gvLnScheme.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");

                    if (gr.Cells[2].Text == "N")
                        lb.ForeColor = System.Drawing.Color.Black;
                    else
                        lb.ForeColor = System.Drawing.Color.Blue;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                lblAmount.Text = gvRow.Cells[3].Text + ".00";
                if (e.CommandName == "cmdShow")
                {
                    oGP = new CGenParameter();
                    dt = oGP.GetParameterDetails(vLnTypId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        txtIntRate.Text = Convert.ToString(dt.Rows[0]["InstRate"]);
                        txtPenInt.Text = Convert.ToString(dt.Rows[0]["PenInt"]);
                        ddlLoanAc.SelectedIndex = ddlLoanAc.Items.IndexOf(ddlLoanAc.Items.FindByValue(dt.Rows[0]["LoanAC"].ToString().Trim()));
                        ddlLnIntAc.SelectedIndex = ddlLnIntAc.Items.IndexOf(ddlLnIntAc.Items.FindByValue(dt.Rows[0]["InstAC"].ToString().Trim()));
                        ddlPenIntAc.SelectedIndex = ddlPenIntAc.Items.IndexOf(ddlPenIntAc.Items.FindByValue(dt.Rows[0]["PenIntAc"].ToString().Trim()));
                        ddlProcFeeAc.SelectedIndex = ddlProcFeeAc.Items.IndexOf(ddlProcFeeAc.Items.FindByValue(dt.Rows[0]["ProcAC"].ToString().Trim()));
                        ddlInsure.SelectedIndex = ddlInsure.Items.IndexOf(ddlInsure.Items.FindByValue(dt.Rows[0]["InsureAC"].ToString().Trim()));
                        ddlWoff.SelectedIndex = ddlWoff.Items.IndexOf(ddlWoff.Items.FindByValue(dt.Rows[0]["WriteOffAC"].ToString().Trim()));
                        ddlServTax.SelectedIndex = ddlServTax.Items.IndexOf(ddlServTax.Items.FindByValue(dt.Rows[0]["ServiceTaxAC"].ToString().Trim()));
                        ddlSGSTAc.SelectedIndex = ddlSGSTAc.Items.IndexOf(ddlSGSTAc.Items.FindByValue(dt.Rows[0]["SGSTAc"].ToString().Trim()));
                        ddlWoffRec.SelectedIndex = ddlWoffRec.Items.IndexOf(ddlWoffRec.Items.FindByValue(dt.Rows[0]["WriteOffRecAC"].ToString().Trim()));
                        ddlOthFees.SelectedIndex = ddlOthFees.Items.IndexOf(ddlOthFees.Items.FindByValue(dt.Rows[0]["OthersFeesAC"].ToString().Trim()));
                        ddlPAI.SelectedIndex = ddlPAI.Items.IndexOf(ddlPAI.Items.FindByValue(dt.Rows[0]["PAIAmountAC"].ToString().Trim()));
                        ddlMediClaimAc.SelectedIndex = ddlMediClaimAc.Items.IndexOf(ddlMediClaimAc.Items.FindByValue(dt.Rows[0]["MedClaimAc"].ToString().Trim()));
                        ddlDisbLedger.SelectedIndex = ddlDisbLedger.Items.IndexOf(ddlDisbLedger.Items.FindByValue(dt.Rows[0]["DisbLedger"].ToString().Trim()));
                        ddlAdvAc.SelectedIndex = ddlAdvAc.Items.IndexOf(ddlAdvAc.Items.FindByValue(dt.Rows[0]["AdvAC"].ToString().Trim()));
                        ddlExcesAc.SelectedIndex = ddlExcesAc.Items.IndexOf(ddlExcesAc.Items.FindByValue(dt.Rows[0]["ExcessAC"].ToString().Trim()));
                        ddlIntAccAc.SelectedIndex = ddlIntAccAc.Items.IndexOf(ddlIntAccAc.Items.FindByValue(dt.Rows[0]["IntAccruedAc"].ToString().Trim()));
                        ddlODIntRec.SelectedIndex = ddlODIntRec.Items.IndexOf(ddlODIntRec.Items.FindByValue(dt.Rows[0]["ODIntRec"].ToString().Trim()));
                        ddlSusIntInc.SelectedIndex = ddlSusIntInc.Items.IndexOf(ddlSusIntInc.Items.FindByValue(dt.Rows[0]["SusIntInc"].ToString().Trim()));
                        ddlDefSusIntInc.SelectedIndex = ddlDefSusIntInc.Items.IndexOf(ddlDefSusIntInc.Items.FindByValue(dt.Rows[0]["DefSusIntInc"].ToString().Trim()));
                        ddlDefODIntRec.SelectedIndex = ddlDefODIntRec.Items.IndexOf(ddlDefODIntRec.Items.FindByValue(dt.Rows[0]["DefODIntRec"].ToString().Trim()));
                        txtProcFee.Text = Convert.ToString(dt.Rows[0]["ProcFeeAmt"]);
                        txtInsure.Text = Convert.ToString(dt.Rows[0]["IncFeesAmt"]);
                        txtInstall.Text = Convert.ToString(dt.Rows[0]["InstallmentSize"]);
                        txtSrvTax.Text = Convert.ToString(dt.Rows[0]["ServiceTaxAmt"]);
                        txtSGST.Text = Convert.ToString(dt.Rows[0]["SGST"]);
                        txtProsFee.Text = Convert.ToString(dt.Rows[0]["AppProcFee"]);
                        txtServTax.Text = Convert.ToString(dt.Rows[0]["AppSerTax"]);
                        txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["InstPeriod"]);
                        txtOthFees.Text = Convert.ToString(dt.Rows[0]["OthersFeesAmt"]);
                        txtMedAmt.Text = Convert.ToString(dt.Rows[0]["MedClaimAmt"]);
                        txtPAI.Text = Convert.ToString(dt.Rows[0]["PAIAmt"]);
                        ddlPaySchedul.SelectedIndex = ddlPaySchedul.Items.IndexOf(ddlPaySchedul.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString().Trim()));
                        ddlLnIntTyp.SelectedIndex = ddlLnIntTyp.Items.IndexOf(ddlLnIntTyp.Items.FindByValue(dt.Rows[0]["InstType"].ToString().Trim()));

                        ddlInsuCGSTAC.SelectedIndex = ddlInsuCGSTAC.Items.IndexOf(ddlInsuCGSTAC.Items.FindByValue(dt.Rows[0]["InsuCGSTAC"].ToString().Trim()));
                        ddlInsuSGSTAC.SelectedIndex = ddlInsuSGSTAC.Items.IndexOf(ddlInsuSGSTAC.Items.FindByValue(dt.Rows[0]["InsuSGSTAC"].ToString().Trim()));
                        ddlBounceChrgAC.SelectedIndex = ddlBounceChrgAC.Items.IndexOf(ddlBounceChrgAC.Items.FindByValue(dt.Rows[0]["BounceChrgAC"].ToString().Trim()));
                        ddlBounceChrgWaveAC.SelectedIndex = ddlBounceChrgWaveAC.Items.IndexOf(ddlBounceChrgWaveAC.Items.FindByValue(dt.Rows[0]["BounceChrgWaveAC"].ToString().Trim()));
                        ddlPreCloseChrgeAC.SelectedIndex = ddlPreCloseChrgeAC.Items.IndexOf(ddlPreCloseChrgeAC.Items.FindByValue(dt.Rows[0]["PreCloseChrgAC"].ToString().Trim()));
                        ddlPreCloseChrgeWaiveAC.SelectedIndex = ddlPreCloseChrgeWaiveAC.Items.IndexOf(ddlPreCloseChrgeWaiveAC.Items.FindByValue(dt.Rows[0]["PreCloseChrgWaiveAC"].ToString().Trim()));
                        ddlPenChrge.SelectedIndex = ddlPenChrge.Items.IndexOf(ddlPenChrge.Items.FindByValue(dt.Rows[0]["DelayPaymentAC"].ToString().Trim()));
                        txtIntsNo.Text = Convert.ToString(dt.Rows[0]["InstallNo"]);
                        txtMinAmtReschudle.Text = Convert.ToString(dt.Rows[0]["MinAmtReschudle"]);
                        txtMaxAdvAmt.Text = Convert.ToString(dt.Rows[0]["MaxAdvAmt"]);
                        if (dt.Rows[0]["ActiveYN"].ToString().Trim() == "Y")
                            chkInActv.Checked = false;
                        else
                            chkInActv.Checked = true;
                        txtInActiveDate.Text = Convert.ToString(dt.Rows[0]["InActiveDate"]);

                        if (dt.Rows[0]["IsTopup"].ToString().Trim() == "Y")
                            chkIsTopUp.Checked = true;
                        else
                            chkIsTopUp.Checked = false;

                        if (dt.Rows[0]["IsOpenMarket"].ToString().Trim() == "Y")
                            chkIsOpenMkt.Checked = true;
                        else
                            chkIsOpenMkt.Checked = false;

                        if (dt.Rows[0]["AllowAdv"].ToString().Trim() == "Y")
                            ckhAllowAdv.Checked = true;
                        else
                            ckhAllowAdv.Checked = false;

                        txtEffDate.Text = Convert.ToString(dt.Rows[0]["EffDate"]);

                        if (dt.Rows[0]["ChkVerify"].ToString().Trim() == "Y")
                            ChkVerify.Checked = true;
                        else
                            ChkVerify.Checked = false;

                        chkIsIndividual.Checked = dt.Rows[0]["IsIndividual"].ToString() == "Y" ? true : false;

                        if (dt.Rows[0]["PreAppLoan"].ToString().Trim() == "Y")
                            chkPreAppLoan.Checked = true;
                        else
                            chkPreAppLoan.Checked = false;
                        txtInstallmentRange.Text = Convert.ToString(dt.Rows[0]["InstallmentRange"]);
                        rdbSchedule.SelectedValue = dt.Rows[0]["SchduleType"].ToString().Trim();

                        txtBounceCharges.Text = Convert.ToString(dt.Rows[0]["BounceCharges"]);
                        ddlBounceChargesGSTType.SelectedIndex = ddlBounceChargesGSTType.Items.IndexOf(ddlBounceChargesGSTType.Items.FindByValue(dt.Rows[0]["BounceChargesGSTType"].ToString().Trim()));
                        txtVisitCharges.Text = Convert.ToString(dt.Rows[0]["VisitCharges"]);
                        ddlVisitChargesGSTType.SelectedIndex = ddlVisitChargesGSTType.Items.IndexOf(ddlVisitChargesGSTType.Items.FindByValue(dt.Rows[0]["VisitChargesGSTType"].ToString().Trim()));
                        ddlPenalChargesGSTType.SelectedIndex = ddlPenalChargesGSTType.Items.IndexOf(ddlPenalChargesGSTType.Items.FindByValue(dt.Rows[0]["PenalChargesGSTType"].ToString().Trim()));

                        if (dt.Rows[0]["SchduleType"].ToString().Trim() == "M")
                        {
                            dtDtl = new DataTable();
                            dtDtl = oGP.GetPayScheduleById(vLnTypId);
                            ViewState["Manual"] = dtDtl;
                            gvSchedule.DataSource = dtDtl;
                            gvSchedule.DataBind();
                            txtPrinTot.Text = dtDtl.Compute("Sum(Princ)", "").ToString();
                            txtIntTot.Text = dtDtl.Compute("Sum(Inst)", "").ToString();
                            TotalSum();
                        }
                        else
                        {
                            gvSchedule.DataSource = null;
                            gvSchedule.DataBind();
                        }

                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        btnAdd.Enabled = false;
                        btnEdit.Enabled = true;
                    }
                    else
                    {
                        gvSchedule.DataSource = null;
                        gvSchedule.DataBind();
                        ClearControls();
                        StatusButton("Show");
                        btnAdd.Enabled = true;
                        btnEdit.Enabled = false;
                    }
                }
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //    StatusButton("Exit");
                //else
                //    StatusButton("Show");
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oGP = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetManualSchedule(int vRow)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("Princ", typeof(double));
            dt.Columns.Add("POS");
            dt.Columns.Add("Inst");
            dt.Columns.Add("Total");
            for (int i = 0; i < vRow; i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[i]["SlNo"] = i + 1;
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        private void popManual(int vRow)
        {
            DataTable dt = null;
            dt = GetManualSchedule(vRow);
            ViewState["Manual"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }


        /// <summary>
        /// 
        /// </summary>
        private void TotalPrin()
        {
            double vPrTot = 0, vTotal = 0;
            foreach (GridViewRow gr in gvSchedule.Rows)
            {
                TextBox txtPrinS = (TextBox)gr.FindControl("txtPrin");
                if (txtPrinS.Text.Trim() != "")
                    vPrTot = Convert.ToDouble(txtPrinS.Text.Trim());
                else
                    vPrTot = 0;
                vTotal += vPrTot;
            }
            txtPrinTot.Text = vTotal.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TotalInt()
        {
            double vIntTot = 0, vTotal = 0;
            foreach (GridViewRow gr in gvSchedule.Rows)
            {
                TextBox txtInt = (TextBox)gr.FindControl("txtInt");
                if (txtInt.Text.Trim() != "")
                    vIntTot = Convert.ToDouble(txtInt.Text.Trim());
                else
                    vIntTot = 0;
                vTotal += vIntTot;
            }
            txtIntTot.Text = vTotal.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private void TotalSum()
        {
            TotalAmt.Text = (Convert.ToDouble(txtPrinTot.Text) + Convert.ToDouble(txtIntTot.Text)).ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtPrin_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            int curRow = 0;
            double vPrin = 0, vInt = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            TextBox txtPrin = (TextBox)gvSchedule.Rows[curRow].FindControl("txtPrin");
            TextBox txtInt = (TextBox)gvSchedule.Rows[curRow].FindControl("txtInt");
            if (txtPrin.Text != "")
                vPrin = Convert.ToDouble(txtPrin.Text);
            else
                vPrin = 0;

            if (txtInt.Text != "")
                vInt = Convert.ToDouble(txtInt.Text);
            else
                vInt = 0;
            dt.Rows[curRow]["Princ"] = Convert.ToDouble(vPrin);
            dt.Rows[curRow]["Total"] = (vPrin + vInt);

            double drSum = Convert.ToDouble(dt.Compute("Sum(Princ)", ""));
            if (Math.Floor(drSum) > Convert.ToDouble(lblAmount.Text))
            {
                gblFuction.AjxMsgPopup("Loan Amount is Exceeded Rs." + (drSum - Convert.ToDouble(lblAmount.Text)));
                txtPrin.Text = "";
                return;
            }
            Outstanding(curRow, Convert.ToInt32(txtIntsNo.Text));
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
            TotalPrin();
            TotalSum();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Outstanding(int curRow, int vSchNo)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            for (int j = curRow; j < vSchNo; j++)
            {
                if (j == 0)
                    dt.Rows[j]["POS"] = Convert.ToDouble(lblAmount.Text) - Convert.ToDouble(dt.Rows[j]["Princ"]);
                else if (Convert.ToString(dt.Rows[j]["Princ"]) != "")
                {
                    if (Convert.ToString(dt.Rows[j - 1]["Princ"]) != "" && Convert.ToString(dt.Rows[j - 1]["POS"]) != "")
                    {
                        if (j == vSchNo)
                            dt.Rows[j]["POS"] = (Math.Floor(Convert.ToDouble(dt.Rows[j - 1]["POS"]) - Convert.ToDouble(dt.Rows[j]["Princ"]))).ToString();
                        else
                            dt.Rows[j]["POS"] = (Convert.ToDouble(dt.Rows[j - 1]["POS"]) - Convert.ToDouble(dt.Rows[j]["Princ"])).ToString();
                    }
                    else
                    {
                        dt.Rows[j]["Total"] = "";
                        dt.Rows[j]["Princ"] = 0;
                        gblFuction.AjxMsgPopup("Please Fill Previos Rows");
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtInt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Manual"];
            int curRow = 0;
            double vPrin = 0, vInt = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            TextBox txtPrin = (TextBox)gvSchedule.Rows[curRow].FindControl("txtPrin");
            TextBox txtInt = (TextBox)gvSchedule.Rows[curRow].FindControl("txtInt");
            if (txtPrin.Text != "")
                vPrin = Convert.ToDouble(txtPrin.Text);
            else
                vPrin = 0;

            if (txtInt.Text != "")
                vInt = Convert.ToDouble(txtInt.Text);
            else
                vInt = 0;
            dt.Rows[curRow]["Inst"] = vInt;
            dt.Rows[curRow]["Total"] = (vPrin + vInt);
            dt.AcceptChanges();
            ViewState["Manual"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
            TotalInt();
            TotalSum();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbSchedule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbSchedule.SelectedValue == "M")
            {
                if (txtIntsNo.Text == "" || txtIntsNo.Text == "0")
                {
                    gvSchedule.Enabled = false;
                    gblFuction.MsgPopup("Please Enter Installment No.");
                    rdbSchedule.SelectedValue = "A";
                }
                else
                {
                    CGenParameter oGP = new CGenParameter();
                    gvSchedule.Enabled = true;
                    DataTable dt = oGP.GetPayScheduleById(Convert.ToInt32(ViewState["LoanTypeId"]));
                    if (dt.Rows.Count > 0)
                    {
                        gvSchedule.DataSource = dt;
                        gvSchedule.DataBind();
                        ViewState["Manual"] = dt;
                    }
                    else
                        popManual(Convert.ToInt32(txtIntsNo.Text.Trim()));
                }
            }
            else
            {
                gvSchedule.Enabled = false;
                gvSchedule.DataSource = null;
                gvSchedule.DataBind();
            }
        }
        protected void txtInActiveDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkInActv.Checked)
                txtInActiveDate.Enabled = false;
            else
                txtInActiveDate.Enabled = true;
        }

        protected void BtnShowLn_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }
    }
}