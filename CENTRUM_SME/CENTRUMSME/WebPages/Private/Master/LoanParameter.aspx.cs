using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class LoanParameter : CENTRUMBAse
    {
        protected int cPgNo = 1;
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
                LoadGrid(0);
                PopAssets();
                PopIncome();
                PopExpenses();
                PopLiability();
                PopIncomeLiability();
                popMonatorium();
                popAllAcHead();
                PopIncomeExpense();
                PopLiabilityExpense();

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Parameter Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanParam);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Parameter Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
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
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oLS = null;
            // Int32 vRows = 0;
            try
            {
                oLS = new CLoanScheme();
                dt = oLS.GetActiveLnSchemePG();//pPgIndx, ref vRows
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();

            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            //switch (e.CommandName)
            //{
            //    case "Previous":
            //        cPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
            //        break;
            //    case "Next":
            //        cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
            //        break;
            //}
            //LoadGrid(cPgNo);
        }
        private void PopAssets()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vAssets = 1;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";

            ddlLoanAc.DataTextField = "Desc";
            ddlLoanAc.DataValueField = "DescId";
            ddlLoanAc.DataSource = dt;
            ddlLoanAc.DataBind();
            ddlLoanAc.Items.Insert(0, Lst1);

            ddlIntDueAC.DataTextField = "Desc";
            ddlIntDueAC.DataValueField = "DescId";
            ddlIntDueAC.DataSource = dt;
            ddlIntDueAC.DataBind();
            ddlIntDueAC.Items.Insert(0, Lst1);

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
            ddlLnIntAc.DataTextField = "Desc";
            ddlLnIntAc.DataValueField = "DescId";
            ddlLnIntAc.DataSource = dt;
            ddlLnIntAc.DataBind();
            ddlLnIntAc.Items.Insert(0, Lst2);

            ddlFLDGAc.DataTextField = "Desc";
            ddlFLDGAc.DataValueField = "DescId";
            ddlFLDGAc.DataSource = dt;
            ddlFLDGAc.DataBind();
            ddlFLDGAc.Items.Insert(0, Lst2);

            ddlCersai.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlCersai.DataSource = dt;
                ddlCersai.DataTextField = "Desc";
                ddlCersai.DataValueField = "DescId";
            }
            else
            {
                ddlCersai.DataSource = null;
            }
            ddlCersai.DataBind();
            ddlCersai.Items.Insert(0, Lst2);       

        }
        private void PopIncomeExpense()
        {
            DataTable dt = null;
            string vGenAcType = "W";            // for Income  and Expense
            Int32 vIncLi = 0;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncLi);
            ListItem Lst15 = new ListItem();
            Lst15.Text = "<--- Select --->";
            Lst15.Value = "-1";
            ddlWaveInt.DataTextField = "Desc";
            ddlWaveInt.DataValueField = "DescId";
            ddlWaveInt.DataSource = dt;
            ddlWaveInt.DataBind();
            ddlWaveInt.Items.Insert(0, Lst15);


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
        private void PopLiabilityExpense()
        {
            DataTable dt = null;
            string vGenAcType = "Y";            // for Liability  and Expense
            Int32 vIncLi = 0;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncLi);
            ListItem Lst11 = new ListItem();
            Lst11.Text = "<--- Select --->";
            Lst11.Value = "-1";
            ddlStmpChrgAC.DataTextField = "Desc";
            ddlStmpChrgAC.DataValueField = "DescId";
            ddlStmpChrgAC.DataSource = dt;
            ddlStmpChrgAC.DataBind();
            ddlStmpChrgAC.Items.Insert(0, Lst11);
        }
        private void PopIncome()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vIncome = 3;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncome);

            ListItem Lst4 = new ListItem();
            Lst4.Text = "<--- Select --->";
            Lst4.Value = "-1";
            ddlProcFeeAc.DataTextField = "Desc";
            ddlProcFeeAc.DataValueField = "DescId";
            ddlProcFeeAc.DataSource = dt;
            ddlProcFeeAc.DataBind();
            ddlProcFeeAc.Items.Insert(0, Lst4);




            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";
            ddlApplChargeAC.DataTextField = "Desc";
            ddlApplChargeAC.DataValueField = "DescId";
            ddlApplChargeAC.DataSource = dt;
            ddlApplChargeAC.DataBind();
            ddlApplChargeAC.Items.Insert(0, Lst1);


            ListItem Lst5 = new ListItem();
            Lst5.Text = "<--- Select --->";
            Lst5.Value = "-1";
            ddlPenChrge.DataTextField = "Desc";
            ddlPenChrge.DataValueField = "DescId";
            ddlPenChrge.DataSource = dt;
            ddlPenChrge.DataBind();
            ddlPenChrge.Items.Insert(0, Lst5);

            ListItem Lst6 = new ListItem();
            Lst6.Text = "<--- Select --->";
            Lst6.Value = "-1";
            ddlBounceChrgAC.DataTextField = "Desc";
            ddlBounceChrgAC.DataValueField = "DescId";
            ddlBounceChrgAC.DataSource = dt;
            ddlBounceChrgAC.DataBind();
            ddlBounceChrgAC.Items.Insert(0, Lst6);

            ListItem Lst7 = new ListItem();
            Lst7.Text = "<--- Select --->";
            Lst7.Value = "-1";
            ddlWoffRec.DataTextField = "Desc";
            ddlWoffRec.DataValueField = "DescId";
            ddlWoffRec.DataSource = dt;
            ddlWoffRec.DataBind();
            ddlWoffRec.Items.Insert(0, Lst7);

            ListItem Lst8 = new ListItem();
            Lst8.Text = "<--- Select --->";
            Lst8.Value = "-1";
            ddlAdminFees.DataTextField = "Desc";
            ddlAdminFees.DataValueField = "DescId";
            ddlAdminFees.DataSource = dt;
            ddlAdminFees.DataBind();
            ddlAdminFees.Items.Insert(0, Lst8);


            ListItem Lst9 = new ListItem();
            Lst9.Text = "<--- Select --->";
            Lst9.Value = "-1";
            ddlTechFees.DataTextField = "Desc";
            ddlTechFees.DataValueField = "DescId";
            ddlTechFees.DataSource = dt;
            ddlTechFees.DataBind();
            ddlTechFees.Items.Insert(0, Lst9);

            ListItem Lst10 = new ListItem();
            Lst10.Text = "<--- Select --->";
            Lst10.Value = "-1";
            ddlVisitChrge.DataTextField = "Desc";
            ddlVisitChrge.DataValueField = "DescId";
            ddlVisitChrge.DataSource = dt;
            ddlVisitChrge.DataBind();
            ddlVisitChrge.Items.Insert(0, Lst10);


            ddlBrkPrdInt.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlBrkPrdInt.DataSource = dt;
                ddlBrkPrdInt.DataTextField = "Desc";
                ddlBrkPrdInt.DataValueField = "DescId";
            }
            else
            {
                ddlBrkPrdInt.DataSource = null;
            }
            ddlBrkPrdInt.DataBind();
            ddlBrkPrdInt.Items.Insert(0, Lst7);

            
            ddlExcessChargeAC.DataTextField = "Desc";
            ddlExcessChargeAC.DataValueField = "DescId";
            ddlExcessChargeAC.DataSource = dt;
            ddlExcessChargeAC.DataBind();
            ddlExcessChargeAC.Items.Insert(0, Lst7);

        }
        private void PopExpenses()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vExp = 2;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vExp);
            ListItem Lst16 = new ListItem();
            Lst16.Text = "<--- Select --->";
            Lst16.Value = "-1";
            ddlWoff.DataTextField = "Desc";
            ddlWoff.DataValueField = "DescId";
            ddlWoff.DataSource = dt;
            ddlWoff.DataBind();
            ddlWoff.Items.Insert(0, Lst16);
        }
        private void PopLiability()
        {
            DataTable dtLib = null;
            string vGenAcType = "G";
            Int32 vLib = 4;
            CGenParameter oGen = new CGenParameter();
            dtLib = oGen.GetLedgerByAcHeadId(vGenAcType, vLib);

            ListItem Lst5 = new ListItem();
            Lst5.Text = "<--- Select --->";
            Lst5.Value = "-1";
            ddlInsure.DataTextField = "Desc";
            ddlInsure.DataValueField = "DescId";
            ddlInsure.DataSource = dtLib;
            ddlInsure.DataBind();
            ddlInsure.Items.Insert(0, Lst5);

            
            ListItem Lst7 = new ListItem();
            Lst7.Text = "<--- Select --->";
            Lst7.Value = "-1";
            ddlServTax.DataTextField = "Desc";
            ddlServTax.DataValueField = "DescId";
            ddlServTax.DataSource = dtLib;
            ddlServTax.DataBind();
            ddlServTax.Items.Insert(0, Lst7);

            ddlInsServTax.DataTextField = "Desc";
            ddlInsServTax.DataValueField = "DescId";
            ddlInsServTax.DataSource = dtLib;
            ddlInsServTax.DataBind();
            ddlInsServTax.Items.Insert(0, Lst7);

            ddlLPFKKTax.DataTextField = "Desc";
            ddlLPFKKTax.DataValueField = "DescId";
            ddlLPFKKTax.DataSource = dtLib;
            ddlLPFKKTax.DataBind();
            ddlLPFKKTax.Items.Insert(0, Lst7);

            ddlLPFSBTax.DataTextField = "Desc";
            ddlLPFSBTax.DataValueField = "DescId";
            ddlLPFSBTax.DataSource = dtLib;
            ddlLPFSBTax.DataBind();
            ddlLPFSBTax.Items.Insert(0, Lst7);

            ddlCGSTAc.DataTextField = "Desc";
            ddlCGSTAc.DataValueField = "DescId";
            ddlCGSTAc.DataSource = dtLib;
            ddlCGSTAc.DataBind();
            ddlCGSTAc.Items.Insert(0, Lst7);

            ddlSGSTAc.DataTextField = "Desc";
            ddlSGSTAc.DataValueField = "DescId";
            ddlSGSTAc.DataSource = dtLib;
            ddlSGSTAc.DataBind();
            ddlSGSTAc.Items.Insert(0, Lst7);

            ddlIGSTAc.DataTextField = "Desc";
            ddlIGSTAc.DataValueField = "DescId";
            ddlIGSTAc.DataSource = dtLib;
            ddlIGSTAc.DataBind();
            ddlIGSTAc.Items.Insert(0, Lst7);

            ddlPropInsuAC.DataTextField = "Desc";
            ddlPropInsuAC.DataValueField = "DescId";
            ddlPropInsuAC.DataSource = dtLib;
            ddlPropInsuAC.DataBind();
            ddlPropInsuAC.Items.Insert(0, Lst7);

            ddlInsuCGSTAC.DataTextField = "Desc";
            ddlInsuCGSTAC.DataValueField = "DescId";
            ddlInsuCGSTAC.DataSource = dtLib;
            ddlInsuCGSTAC.DataBind();
            ddlInsuCGSTAC.Items.Insert(0, Lst7);

            ddlInsuSGSTAC.DataTextField = "Desc";
            ddlInsuSGSTAC.DataValueField = "DescId";
            ddlInsuSGSTAC.DataSource = dtLib;
            ddlInsuSGSTAC.DataBind();
            ddlInsuSGSTAC.Items.Insert(0, Lst7);

            ddlAdvAc.DataTextField = "Desc";
            ddlAdvAc.DataValueField = "DescId";
            ddlAdvAc.DataSource = dtLib;
            ddlAdvAc.DataBind();
            ddlAdvAc.Items.Insert(0, Lst7);

            ddlSusIntInc.DataTextField = "Desc";
            ddlSusIntInc.DataValueField = "DescId";
            ddlSusIntInc.DataSource = dtLib;
            ddlSusIntInc.DataBind();
            ddlSusIntInc.Items.Insert(0, Lst7);
        }
        private void ClearControls()
        {
            ddlLoanAc.SelectedIndex = -1;
            ddlLnIntAc.SelectedIndex = -1;
            ddlProcFeeAc.SelectedIndex = -1;
            ddlInsure.SelectedIndex = -1;
            ddlStmpChrgAC.SelectedIndex = -1;
            ddlWoff.SelectedIndex = -1;
            ddlServTax.SelectedIndex = -1;
            ddlInsServTax.SelectedIndex = -1;
            ddlApplChargeAC.SelectedIndex = -1;
            ddlLPFKKTax.SelectedIndex = -1;
            ddlLPFSBTax.SelectedIndex = -1;
            ddlPenChrge.SelectedIndex = -1;
            ddlWaveInt.SelectedIndex = -1;
            ddlBounceChrgAC.SelectedIndex = -1;
            ddlBounceChrgWaveAC.SelectedIndex = -1;
            ddlPreCloseChrgeAC.SelectedIndex = -1;
            ddlPreCloseChrgeWaiveAC.SelectedIndex = -1;
            txtProcFee.Text = "0.0";
            txtMaxIntRate.Text = "0.0";
            txtMaxIntRateFlat.Text = "0.0";
            txtMinInstRate.Text = "0.0";
            ddlPaySchedul.SelectedIndex = 3;
            ddlLnIntTyp.SelectedIndex = 1;
            txtMaxIntsNo.Text = "0";
            txtMinIntsNo.Text = "0";
            chkApplyMon.Checked = false;
            chbISevTx.Checked = false;
            chbPsevTx.Checked = false;
            chkBulletPaymnt.Checked = false;
            chkApplyLegVer.Checked = true;
            gvSchedule.DataSource = null;
            gvSchedule.DataBind();
            ddlCGSTAc.SelectedIndex = -1;
            ddlSGSTAc.SelectedIndex = -1;
            ddlFLDGAc.SelectedIndex = -1;
            ddlWoffRec.SelectedIndex = -1;
            ddlIGSTAc.SelectedIndex = -1;
            ddlBrkPrdInt.SelectedIndex = -1;
            ddlAdminFees.SelectedIndex = -1;
            ddlTechFees.SelectedIndex = -1;
            ddlPropInsuAC.SelectedIndex = -1;
            ddlVisitChrge.SelectedIndex = -1;
            ddlCersai.SelectedIndex = -1;
            ddlIntDueAC.SelectedIndex = -1;
            ddlExcessChargeAC.SelectedIndex = -1;
            ddlInsuSGSTAC.SelectedIndex = -1;
            ddlInsuCGSTAC.SelectedIndex = -1;
            ddlAdvAc.SelectedIndex = -1;
            ddlIntAccAc.SelectedIndex = -1;
            ddlODIntRec.SelectedIndex = -1;
            ddlSusIntInc.SelectedIndex = -1;
            txtPenCharges.Text = "0";
            txtBounceCharges.Text = "0";
            ddlBounceChargesGSTType.SelectedIndex = -1;
            txtVisitCharges.Text = "0";
            ddlVisitChargesGSTType.SelectedIndex = -1;
            txtMinAmtReschudle.Text = "0";
            txtMaxAdvAmt.Text = "0";
            ddlPenalChargesGSTType.SelectedIndex = -1;
        }
        private void EnableControl(bool Status)
        {
            ddlLoanAc.Enabled = Status;
            ddlLnIntAc.Enabled = Status;
            ddlProcFeeAc.Enabled = Status;
            ddlInsure.Enabled = Status;
            ddlStmpChrgAC.Enabled = Status;
            ddlWoff.Enabled = Status;
            ddlServTax.Enabled = Status;
            chkApplyMon.Enabled = Status;
            ddlApplChargeAC.Enabled = Status;
            ddlPenChrge.Enabled = Status;
            ddlWaveInt.Enabled = Status;
            ddlLPFKKTax.Enabled = Status;
            ddlLPFSBTax.Enabled = Status;
            txtProcFee.Enabled = Status;
            txtMaxIntRate.Enabled = Status;
            txtMaxIntRateFlat.Enabled = Status;
            txtMinInstRate.Enabled = Status;
            ddlPaySchedul.Enabled = Status;
            ddlLnIntTyp.Enabled = Status;
            txtMaxIntsNo.Enabled = Status;
            txtMinIntsNo.Enabled = Status;
            gvSchedule.Enabled = false;
            chbPsevTx.Enabled = Status;
            chbISevTx.Enabled = Status;
            ddlInsServTax.Enabled = Status;
            chkBulletPaymnt.Enabled = Status;
            ddlBounceChrgAC.Enabled = Status;
            ddlBounceChrgWaveAC.Enabled = Status;
            ddlPreCloseChrgeAC.Enabled = Status;
            ddlPreCloseChrgeWaiveAC.Enabled = Status;
            ddlCGSTAc.Enabled = Status;
            ddlSGSTAc.Enabled = Status;
            ddlFLDGAc.Enabled = Status;
            ddlWoffRec.Enabled = Status;
            ddlIGSTAc.Enabled = Status;
            ddlBrkPrdInt.Enabled = Status;
            txtEffRedIntRate.Enabled = Status;
            ddlAdminFees.Enabled = Status;
            ddlTechFees.Enabled = Status;
            ddlPropInsuAC.Enabled = Status;
            ddlVisitChrge.Enabled = Status;
            ddlCersai.Enabled = Status;
            ddlIntDueAC.Enabled = Status;
            ddlExcessChargeAC.Enabled = Status;
            ddlInsuSGSTAC.Enabled = Status;
            ddlInsuCGSTAC.Enabled = Status;
            ddlAdvAc.Enabled = Status;
            ddlIntAccAc.Enabled = Status;
            ddlODIntRec.Enabled = Status;
            ddlSusIntInc.Enabled = Status;
            txtPenCharges.Enabled = Status;
            txtBounceCharges.Enabled = Status;
            ddlBounceChargesGSTType.Enabled = Status;
            txtVisitCharges.Enabled = Status;
            ddlVisitChargesGSTType.Enabled = Status;
            txtMinAmtReschudle.Enabled = Status;
            txtMaxAdvAmt.Enabled = Status;
            ddlPenalChargesGSTType.Enabled = Status;
        }
        private bool ValidateField()
        {
            bool vRes = true;
            if (txtProcFee.Text != "" && txtProcFee.Text != "0.0" && txtProcFee.Text != "0" && ddlProcFeeAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Processing Fee A/c Can not be Empty");
                return vRes = false;
            }
            if (ddlLoanAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Loan A/c Can not be Empty");
                return vRes = false;
            }
            if (ddlPenChrge.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Delay Payment Charge A/C Can not be Empty");
                return vRes = false;
            }
            //if (ddlVisitChrge.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Visit Charge A/C Can not be Empty");
            //    return vRes = false;
            //}
            if (ddlProcFeeAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Processing A/c Can not be Empty");
                return vRes = false;
            }
            //if (ddlInsure.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Credit Shield Insurance A/c Can not be Empty");
            //    return vRes = false;
            //}
            //if (ddlPropInsuAC.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Insurance A/c Can not be Empty");
            //    return vRes = false;
            //}
            
            if (ddlCGSTAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Tech  CGST A/c ..");
                return vRes = false;
            }
            if (ddlSGSTAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Tech  SGST A/c ..");
                return vRes = false;
            }
            //if (ddlIGSTAc.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Tech  IGST A/c ..");
            //    return vRes = false;
            //}
            if (ddlBounceChrgAC.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Bounce Charge A/c..");
                return vRes = false;
            }
            //ddlBrkPrdInt
            //if (ddlBrkPrdInt.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Tech  Broken Period Interest A/c ..");
            //    return vRes = false;
            //}
            //if (ddlAdminFees.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Administation Fees AC ..");
            //    return vRes = false;
            //}
            //if (ddlTechFees.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Tech Fees AC ..");
            //    return vRes = false;
            //}
            if (ddlCersai.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Mediclaim AC ..");
                return vRes = false;
            }
            //if (ddlIntDueAC.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Interest Outstanding AC ..");
            //    return vRes = false;
            //}
            //ddlBounceChrgAC
            //if (ddlPaySchedul.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Pay Schedule..");
            //    return vRes = false;
            //}
            //if (ddlLnIntTyp.SelectedIndex <= 0)
            //{
            //    gblFuction.MsgPopup("Select Loan Interest Type..");
            //    return vRes = false;
            //}

            if (ddlLnIntAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Loan Interest  Account..");
                return vRes = false;
            }
            if (ddlCGSTAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select CGST Ac ..");
                return vRes = false;
            }
            if (ddlSGSTAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select SGST Ac ..");
                return vRes = false;
            }
            if (ddlExcessChargeAC.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select ExcessChargeAC ..");
                return vRes = false;
            }
            //if (txtMaxIntRate.Text == "" || txtMaxIntRate.Text == "0")
            //{
            //    gblFuction.MsgPopup("Max Interest Rate can not be empty or zero");
            //    return vRes = false;
            //}
            //if (txtMaxIntRateFlat.Text == "" || txtMaxIntRateFlat.Text == "0")
            //{
            //    gblFuction.MsgPopup("Max Interest Rate Flat can not be empty or zero");
            //    return vRes = false;
            //}
            //if (txtMinInstRate.Text == "" || txtMinInstRate.Text == "0")
            //{
            //    gblFuction.MsgPopup("Min Interest Rate can not be empty or zero");
            //    return vRes = false;
            //}
            //if (Convert.ToDecimal(txtMinInstRate.Text) > Convert.ToDecimal(txtMaxIntRate.Text))
            //{
            //    gblFuction.MsgPopup("Min Interest Rate can not be greater than Max Interest Rate");
            //    return vRes = false;
            //}
            //if (Convert.ToDecimal(txtMinIntsNo.Text) > Convert.ToDecimal(txtMaxIntsNo.Text))
            //{
            //    gblFuction.MsgPopup("Min Installment No can not be greater than Max Installment No");
            //    return vRes = false;
            //}

            //if (txtMaxIntsNo.Text == "" || txtMaxIntsNo.Text == "0")
            //{
            //    gblFuction.MsgPopup("Max Installment No can not be empty or zero");
            //    return vRes = false;

            //}
            //if (txtMinIntsNo.Text == "" || txtMinIntsNo.Text == "0")
            //{
            //    gblFuction.MsgPopup("Min Installment No can not be empty or zero");
            //    return vRes = false;
            //}
            if (txtEffRedIntRate.Text == "" || txtEffRedIntRate.Text == "0")
            {
                gblFuction.MsgPopup("Effective Reducing Interest Rate Can Not Be Empty or zero");
                return vRes = false;
            }
            return vRes;
        }
        private Boolean SaveRecords(string Mode)
        {
            //  DataTable dtXml = (DataTable)ViewState["Monatorium"];
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add(new DataColumn("SlNo"));
            dtXml.Columns.Add(new DataColumn("FromSlNo"));
            dtXml.Columns.Add(new DataColumn("ToSlNo"));
            dtXml.Columns.Add(new DataColumn("PrinYN"));

            foreach (GridViewRow gr in gvSchedule.Rows)
            {
                dr = dtXml.NewRow();
                dr["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;
                dr["FromSlNo"] = ((Label)gr.FindControl("lblFSLno")).Text;
                dr["ToSlNo"] = ((TextBox)gr.FindControl("txtMoTo")).Text;
                dr["PrinYN"] = ((DropDownList)gr.FindControl("ddlPrinYN")).SelectedValue;
                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vXml = "", vApplyMonatorium = "", vApplyBulletPaymnt = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0, vMaxInstNo = 0, vMinInstNo = 0;
            double vProc = 0, vMaxIntrate = 0, vMinIntrate = 0, vMaxIntFlat = 0, vEffRedIntRate = 0, vMinAmtReschudle = 0, vMaxAdvAmt = 0;
            string vPSevtx = "", vISevTax = "",IsLegVerProcAppYN="Y";

            CGenParameter oGP = null;
            CGblIdGenerator oGbl = null;


            oGP = new CGenParameter();


            try
            {
                if (ValidateField() == false)
                    return false;
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }

                if (txtProcFee.Text.Trim() != "")
                    vProc = Convert.ToDouble(txtProcFee.Text.Trim());
                else
                    vProc = 0.0;

                if (txtMaxIntRate.Text.Trim() != "")
                    vMaxIntrate = Convert.ToDouble(txtMaxIntRate.Text.Trim());
                else
                    vMaxIntrate = 0.0;
                if (txtMaxIntRateFlat.Text.Trim() != "")
                    vMaxIntFlat = Convert.ToDouble(txtMaxIntRateFlat.Text.Trim());
                else
                    vMaxIntFlat = 0.0;
                if (txtMinInstRate.Text.Trim() != "")
                    vMinIntrate = Convert.ToDouble(txtMinInstRate.Text.Trim());
                else
                    vMinIntrate = 0.0;

                if (txtMaxIntsNo.Text.Trim() != "")
                    vMaxInstNo = Convert.ToInt32(txtMaxIntsNo.Text.Trim());
                else
                    vMaxInstNo = 0;
                if (txtMinIntsNo.Text.Trim() != "")
                    vMinInstNo = Convert.ToInt32(txtMinIntsNo.Text.Trim());
                else
                    vMinInstNo = 0;

                if (txtEffRedIntRate.Text != "")
                    vEffRedIntRate = Convert.ToDouble(txtEffRedIntRate.Text);

                if (txtMinAmtReschudle.Text.Trim() != "")
                    vMinAmtReschudle = Convert.ToDouble(txtMinAmtReschudle.Text.Trim());

                if (txtMaxAdvAmt.Text.Trim() != "")
                    vMaxAdvAmt = Convert.ToDouble(txtMaxAdvAmt.Text.Trim());

                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);

                if (this.chkApplyMon.Checked == true) { vApplyMonatorium = "Y"; } else { vApplyMonatorium = "N"; }
                if (this.chkBulletPaymnt.Checked == true) { vApplyBulletPaymnt = "Y"; } else { vApplyBulletPaymnt = "N"; }
                if (this.chbPsevTx.Checked == true) { vPSevtx = "Y"; } else { vPSevtx = "N"; }
                if (this.chbISevTx.Checked == true) { vISevTax = "Y"; } else { vISevTax = "N"; }
                if (this.chkApplyLegVer.Checked == true) { IsLegVerProcAppYN = "Y"; } else { IsLegVerProcAppYN = "N"; }
                if (Mode == "Save")
                {
                    oGP = new CGenParameter();

                    vErr = oGP.InsertLnParameter(vLoanTypeId, vApplyMonatorium, ddlPaySchedul.SelectedValue, ddlLnIntTyp.SelectedValue, vProc, ddlLoanAc.SelectedValue,
                        ddlLnIntAc.SelectedValue, ddlPenChrge.SelectedValue,
                        ddlWaveInt.SelectedValue, ddlLPFKKTax.SelectedValue, ddlLPFSBTax.SelectedValue, ddlApplChargeAC.SelectedValue, ddlProcFeeAc.SelectedValue,
                        ddlInsure.SelectedValue, ddlWoff.SelectedValue, ddlServTax.SelectedValue, ddlStmpChrgAC.SelectedValue,
                        vXml, this.UserID, vPSevtx, vISevTax, ddlInsServTax.SelectedValue, vMaxInstNo, vMinInstNo, vMaxIntrate, vMinIntrate, vMaxIntFlat,
                        vApplyBulletPaymnt, ddlBounceChrgAC.SelectedValue, ddlBounceChrgWaveAC.SelectedValue, ddlPreCloseChrgeAC.SelectedValue,
                        ddlPreCloseChrgeWaiveAC.SelectedValue, ddlCGSTAc.SelectedValue, ddlSGSTAc.SelectedValue,ddlFLDGAc.SelectedValue,
                        ddlWoffRec.SelectedValue, ddlBrkPrdInt.SelectedValue, ddlIGSTAc.SelectedValue, vEffRedIntRate,ddlAdminFees.SelectedValue,
                        ddlTechFees.SelectedValue, ddlPropInsuAC.SelectedValue, ddlVisitChrge.SelectedValue, IsLegVerProcAppYN,ddlCersai.SelectedValue,
                        ddlIntDueAC.SelectedValue,ddlExcessChargeAC.SelectedValue.ToString(),ddlInsuCGSTAC.SelectedValue,ddlInsuSGSTAC.SelectedValue,
                        ddlODIntRec.SelectedValue, ddlSusIntInc.SelectedValue,ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue,
                        Convert.ToDouble(txtPenCharges.Text), Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue,
                        Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue, vMinAmtReschudle, vMaxAdvAmt, ddlPenalChargesGSTType.SelectedValue);
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
                    vErr = oGP.UpdateParameter(vLoanTypeId, vApplyMonatorium, ddlPaySchedul.SelectedValue, ddlLnIntTyp.SelectedValue, vProc, ddlLoanAc.SelectedValue,
                        ddlLnIntAc.SelectedValue, ddlPenChrge.SelectedValue,
                        ddlWaveInt.SelectedValue, ddlLPFKKTax.SelectedValue, ddlLPFSBTax.SelectedValue, ddlApplChargeAC.SelectedValue, ddlProcFeeAc.SelectedValue,
                        ddlInsure.SelectedValue, ddlWoff.SelectedValue, ddlServTax.SelectedValue, ddlStmpChrgAC.SelectedValue,
                        vXml, this.UserID, vPSevtx, vISevTax, ddlInsServTax.SelectedValue, vMaxInstNo, vMinInstNo, vMaxIntrate, vMinIntrate, vMaxIntFlat,
                        vApplyBulletPaymnt, ddlBounceChrgAC.SelectedValue, ddlBounceChrgWaveAC.SelectedValue, ddlPreCloseChrgeAC.SelectedValue,
                        ddlPreCloseChrgeWaiveAC.SelectedValue, ddlCGSTAc.SelectedValue, ddlSGSTAc.SelectedValue, ddlFLDGAc.SelectedValue,
                        ddlWoffRec.SelectedValue, ddlBrkPrdInt.SelectedValue, ddlIGSTAc.SelectedValue, vEffRedIntRate,
                        ddlAdminFees.SelectedValue, ddlTechFees.SelectedValue, ddlPropInsuAC.SelectedValue, ddlVisitChrge.SelectedValue, IsLegVerProcAppYN,
                        ddlCersai.SelectedValue, ddlIntDueAC.SelectedValue,ddlExcessChargeAC.SelectedValue.ToString(),ddlInsuCGSTAC.SelectedValue
                        ,ddlInsuSGSTAC.SelectedValue, ddlODIntRec.SelectedValue, ddlSusIntInc.SelectedValue, ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue,
                        Convert.ToDouble(txtPenCharges.Text), Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue,
                        Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue, vMinAmtReschudle, vMaxAdvAmt, ddlPenalChargesGSTType.SelectedValue);
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oGP = null;
                oGbl = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["LoanTypeId"] = null;
        }
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
                //LoadBranch("Add", 0);
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
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                if (chkApplyMon.Checked == true)
                    gvSchedule.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("Show");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    //tabLnScheme.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvLnScheme_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lblPrd = (LinkButton)e.Row.FindControl("btnShow");
                if (e.Row.Cells[2].Text == "Y")
                    lblPrd.ForeColor = System.Drawing.Color.Blue;
            }
        }
        protected void gvLnScheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vLnTypId = 0, vStat = 0;
            DataTable dt = null;
            DataTable dtDtl = null;
            CGenParameter oGP = null;
            try
            {
                vLnTypId = Convert.ToInt32(e.CommandArgument);
                ViewState["LoanTypeId"] = vLnTypId;
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLnScheme.Rows)
                {
                    if ((gr.RowIndex) % 2 == 0)
                    {
                        gr.BackColor = backColor;
                        gr.ForeColor = foreColor;
                    }
                    else
                    {
                        gr.BackColor = System.Drawing.Color.White;
                        gr.ForeColor = foreColor;
                    }
                    gr.Font.Bold = false;
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;

                    if (gr.Cells[2].Text == "N")
                        lb.ForeColor = System.Drawing.Color.Black;
                    else
                        lb.ForeColor = System.Drawing.Color.Blue;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                if (e.CommandName == "cmdShow")
                {
                    oGP = new CGenParameter();
                    dt = oGP.GetParameterDetails(vLnTypId);
                    if (dt.Rows.Count > 0)
                    {
                        txtMaxIntRate.Text = Convert.ToString(dt.Rows[0]["MaxInstRate"]);
                        ddlLoanAc.SelectedIndex = ddlLoanAc.Items.IndexOf(ddlLoanAc.Items.FindByValue(dt.Rows[0]["LoanAC"].ToString().Trim()));
                        ddlLnIntAc.SelectedIndex = ddlLnIntAc.Items.IndexOf(ddlLnIntAc.Items.FindByValue(dt.Rows[0]["InstAC"].ToString().Trim()));
                        ddlProcFeeAc.SelectedIndex = ddlProcFeeAc.Items.IndexOf(ddlProcFeeAc.Items.FindByValue(dt.Rows[0]["ProcAC"].ToString().Trim()));
                        ddlInsure.SelectedIndex = ddlInsure.Items.IndexOf(ddlInsure.Items.FindByValue(dt.Rows[0]["InsureAC"].ToString().Trim()));
                        ddlWoff.SelectedIndex = ddlWoff.Items.IndexOf(ddlWoff.Items.FindByValue(dt.Rows[0]["WriteOffAC"].ToString().Trim()));
                        ddlInsuCGSTAC.SelectedIndex = ddlInsuCGSTAC.Items.IndexOf(ddlInsuCGSTAC.Items.FindByValue(dt.Rows[0]["InsuCGSTAC"].ToString().Trim()));
                        ddlInsuSGSTAC.SelectedIndex = ddlInsuSGSTAC.Items.IndexOf(ddlInsuSGSTAC.Items.FindByValue(dt.Rows[0]["InsuSGSTAC"].ToString().Trim()));

                        if (dt.Rows[0]["WriteOffRecAC"].ToString() != "")
                            ddlWoffRec.SelectedIndex = ddlWoffRec.Items.IndexOf(ddlWoffRec.Items.FindByValue(dt.Rows[0]["WriteOffRecAC"].ToString().Trim()));
                        else
                            ddlWoffRec.SelectedIndex = -1;
                        ddlServTax.SelectedIndex = ddlServTax.Items.IndexOf(ddlServTax.Items.FindByValue(dt.Rows[0]["ServiceTaxAC"].ToString().Trim()));
                        ddlInsServTax.SelectedIndex = ddlInsServTax.Items.IndexOf(ddlInsServTax.Items.FindByValue(dt.Rows[0]["InsServTaxAC"].ToString().Trim()));
                        ddlStmpChrgAC.SelectedIndex = ddlStmpChrgAC.Items.IndexOf(ddlStmpChrgAC.Items.FindByValue(dt.Rows[0]["StampChargeAC"].ToString().Trim()));
                        ddlApplChargeAC.SelectedIndex = ddlApplChargeAC.Items.IndexOf(ddlApplChargeAC.Items.FindByValue(dt.Rows[0]["ApplicationCargeAC"].ToString().Trim()));

                        if (dt.Rows[0]["PenaltyChargeAC"].ToString() != "")
                            ddlPenChrge.SelectedIndex = ddlPenChrge.Items.IndexOf(ddlPenChrge.Items.FindByValue(dt.Rows[0]["PenaltyChargeAC"].ToString().Trim()));
                        else
                            ddlPenChrge.SelectedIndex = -1;
                        if (dt.Rows[0]["IntWaveAC"].ToString() != "")
                            ddlWaveInt.SelectedIndex = ddlWaveInt.Items.IndexOf(ddlWaveInt.Items.FindByValue(dt.Rows[0]["IntWaveAC"].ToString().Trim()));
                        else
                            ddlWaveInt.SelectedIndex = -1;
                        if (dt.Rows[0]["LPFKKTaxAC"].ToString() != "")
                            ddlLPFKKTax.SelectedIndex = ddlLPFKKTax.Items.IndexOf(ddlLPFKKTax.Items.FindByValue(dt.Rows[0]["LPFKKTaxAC"].ToString().Trim()));
                        else
                            ddlLPFKKTax.SelectedIndex = -1;

                        if (dt.Rows[0]["LPFSBTaxAC"].ToString() != "")
                            ddlLPFSBTax.SelectedIndex = ddlLPFSBTax.Items.IndexOf(ddlLPFSBTax.Items.FindByValue(dt.Rows[0]["LPFSBTaxAC"].ToString().Trim()));
                        else
                            ddlLPFSBTax.SelectedIndex = -1;


                        if (dt.Rows[0]["BounceChrgAC"].ToString() != "")
                            ddlBounceChrgAC.SelectedIndex = ddlBounceChrgAC.Items.IndexOf(ddlBounceChrgAC.Items.FindByValue(dt.Rows[0]["BounceChrgAC"].ToString().Trim()));
                        else
                            ddlBounceChrgAC.SelectedIndex = -1;
                        if (dt.Rows[0]["BounceChrgWaveAC"].ToString() != "")
                            ddlBounceChrgWaveAC.SelectedIndex = ddlBounceChrgWaveAC.Items.IndexOf(ddlBounceChrgWaveAC.Items.FindByValue(dt.Rows[0]["BounceChrgWaveAC"].ToString().Trim()));
                        else
                            ddlBounceChrgWaveAC.SelectedIndex = -1;
                        if (dt.Rows[0]["PreCloseChrgAC"].ToString() != "")
                            ddlPreCloseChrgeAC.SelectedIndex = ddlPreCloseChrgeAC.Items.IndexOf(ddlPreCloseChrgeAC.Items.FindByValue(dt.Rows[0]["PreCloseChrgAC"].ToString().Trim()));
                        else
                            ddlPreCloseChrgeAC.SelectedIndex = -1;
                        if (dt.Rows[0]["PreCloseChrgWaiveAC"].ToString() != "")
                            ddlPreCloseChrgeWaiveAC.SelectedIndex = ddlPreCloseChrgeWaiveAC.Items.IndexOf(ddlPreCloseChrgeWaiveAC.Items.FindByValue(dt.Rows[0]["PreCloseChrgWaiveAC"].ToString().Trim()));
                        else
                            ddlPreCloseChrgeWaiveAC.SelectedIndex = -1;

                        if (dt.Rows[0]["CGSTAC"].ToString() != "")
                            ddlCGSTAc.SelectedIndex = ddlCGSTAc.Items.IndexOf(ddlCGSTAc.Items.FindByValue(dt.Rows[0]["CGSTAC"].ToString().Trim()));
                        else
                            ddlCGSTAc.SelectedIndex = -1;
                        if (dt.Rows[0]["SGSTAC"].ToString() != "")
                            ddlSGSTAc.SelectedIndex = ddlSGSTAc.Items.IndexOf(ddlSGSTAc.Items.FindByValue(dt.Rows[0]["SGSTAC"].ToString().Trim()));
                        else
                            ddlSGSTAc.SelectedIndex = -1;
                        if (dt.Rows[0]["IGSTAC"].ToString() != "")
                            ddlIGSTAc.SelectedIndex = ddlIGSTAc.Items.IndexOf(ddlIGSTAc.Items.FindByValue(dt.Rows[0]["IGSTAC"].ToString().Trim()));
                        else
                            ddlIGSTAc.SelectedIndex = -1;
                        if (dt.Rows[0]["FLDGAC"].ToString() != "")
                            ddlFLDGAc.SelectedIndex = ddlFLDGAc.Items.IndexOf(ddlFLDGAc.Items.FindByValue(dt.Rows[0]["FLDGAC"].ToString().Trim()));
                        else
                            ddlFLDGAc.SelectedIndex = -1;
                        if (dt.Rows[0]["BrkPrdIntAc"].ToString() != "")
                            ddlBrkPrdInt.SelectedIndex = ddlBrkPrdInt.Items.IndexOf(ddlBrkPrdInt.Items.FindByValue(dt.Rows[0]["BrkPrdIntAc"].ToString().Trim()));
                        else
                            ddlBrkPrdInt.SelectedIndex = -1;

                        if (dt.Rows[0]["AdminFeesAC"].ToString() != "")
                            ddlAdminFees.SelectedIndex = ddlAdminFees.Items.IndexOf(ddlAdminFees.Items.FindByValue(dt.Rows[0]["AdminFeesAC"].ToString().Trim()));
                        else
                            ddlAdminFees.SelectedIndex = -1;
                        if (dt.Rows[0]["TechFeesAC"].ToString() != "")
                            ddlTechFees.SelectedIndex = ddlTechFees.Items.IndexOf(ddlTechFees.Items.FindByValue(dt.Rows[0]["TechFeesAC"].ToString().Trim()));
                        else
                            ddlTechFees.SelectedIndex = -1;
                        if (dt.Rows[0]["PropInsureAC"].ToString() != "")
                            ddlPropInsuAC.SelectedIndex = ddlPropInsuAC.Items.IndexOf(ddlPropInsuAC.Items.FindByValue(dt.Rows[0]["PropInsureAC"].ToString().Trim()));
                        else
                            ddlPropInsuAC.SelectedIndex = -1;
                        if (dt.Rows[0]["VisitChargeAC"].ToString() != "")
                            ddlVisitChrge.SelectedIndex = ddlVisitChrge.Items.IndexOf(ddlVisitChrge.Items.FindByValue(dt.Rows[0]["VisitChargeAC"].ToString().Trim()));
                        else
                            ddlVisitChrge.SelectedIndex = -1;

                        if (dt.Rows[0]["CERSAIAC"].ToString() != "")
                            ddlCersai.SelectedIndex = ddlCersai.Items.IndexOf(ddlCersai.Items.FindByValue(dt.Rows[0]["CERSAIAC"].ToString().Trim()));
                        else
                            ddlCersai.SelectedIndex = -1;

                        if (dt.Rows[0]["IntDueAC"].ToString() != "")
                            ddlIntDueAC.SelectedIndex = ddlIntDueAC.Items.IndexOf(ddlIntDueAC.Items.FindByValue(dt.Rows[0]["IntDueAC"].ToString().Trim()));
                        else
                            ddlIntDueAC.SelectedIndex = -1;
                        if (dt.Rows[0]["ExcessChargeAC"].ToString() != "")
                            ddlExcessChargeAC.SelectedIndex = ddlExcessChargeAC.Items.IndexOf(ddlExcessChargeAC.Items.FindByValue(dt.Rows[0]["ExcessChargeAC"].ToString().Trim()));
                        else
                            ddlExcessChargeAC.SelectedIndex = -1;

                        ddlAdvAc.SelectedIndex = ddlAdvAc.Items.IndexOf(ddlAdvAc.Items.FindByValue(dt.Rows[0]["AdvAC"].ToString().Trim()));
                        ddlIntAccAc.SelectedIndex = ddlIntAccAc.Items.IndexOf(ddlIntAccAc.Items.FindByValue(dt.Rows[0]["IntAccruedAc"].ToString().Trim()));
                        ddlODIntRec.SelectedIndex = ddlODIntRec.Items.IndexOf(ddlODIntRec.Items.FindByValue(dt.Rows[0]["ODIntRec"].ToString().Trim()));
                        ddlSusIntInc.SelectedIndex = ddlSusIntInc.Items.IndexOf(ddlSusIntInc.Items.FindByValue(dt.Rows[0]["SusIntInc"].ToString().Trim()));

                        txtProcFee.Text = Convert.ToString(dt.Rows[0]["ProcFeeAmt"]);

                        if (Convert.ToString(dt.Rows[0]["InsSerTax"]) == "Y")
                        {
                            chbISevTx.Checked = true;
                        }
                        else
                        {
                            chbISevTx.Checked = false;
                        }
                        if (Convert.ToString(dt.Rows[0]["ProSerTax"]) == "Y")
                        {
                            chbPsevTx.Checked = true;
                        }
                        else
                        {
                            chbPsevTx.Checked = false;
                        }

                        if (Convert.ToString(dt.Rows[0]["ApplyBulletPayment"]) == "Y")
                        {
                            chkBulletPaymnt.Checked = true;
                        }
                        else
                        {
                            chkBulletPaymnt.Checked = false;
                        }
                        if (Convert.ToString(dt.Rows[0]["AppLegVerProYN"]) == "Y")
                        {
                            chkApplyLegVer.Checked = true;
                        }
                        else
                        {
                            chkApplyLegVer.Checked = false;
                        }
                        ddlPaySchedul.SelectedIndex = ddlPaySchedul.Items.IndexOf(ddlPaySchedul.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString().Trim()));
                        ddlLnIntTyp.SelectedIndex = ddlLnIntTyp.Items.IndexOf(ddlLnIntTyp.Items.FindByValue(dt.Rows[0]["InstType"].ToString().Trim()));
                        txtMaxIntsNo.Text = Convert.ToString(dt.Rows[0]["MaxInstallNo"]);
                        txtMinIntsNo.Text = Convert.ToString(dt.Rows[0]["MinInstallNo"]);
                        txtMaxIntRate.Text = Convert.ToString(dt.Rows[0]["MaxInstRate"]);
                        txtMinInstRate.Text = Convert.ToString(dt.Rows[0]["MinInstRate"]);
                        txtMaxIntRateFlat.Text = Convert.ToString(dt.Rows[0]["MaxInstRateFlat"]);
                        txtEffRedIntRate.Text = Convert.ToString(dt.Rows[0]["EffRedIntRate"]);
                        txtPenCharges.Text = Convert.ToString(dt.Rows[0]["PenCharges"]);
                        txtBounceCharges.Text = Convert.ToString(dt.Rows[0]["BounceCharges"]);
                        ddlBounceChargesGSTType.SelectedIndex = ddlBounceChargesGSTType.Items.IndexOf(ddlBounceChargesGSTType.Items.FindByValue(dt.Rows[0]["BounceChargesGSTType"].ToString().Trim()));
                        txtVisitCharges.Text = Convert.ToString(dt.Rows[0]["VisitCharges"]);
                        ddlVisitChargesGSTType.SelectedIndex = ddlVisitChargesGSTType.Items.IndexOf(ddlVisitChargesGSTType.Items.FindByValue(dt.Rows[0]["VisitChargesGSTType"].ToString().Trim()));
                        txtMinAmtReschudle.Text = Convert.ToString(dt.Rows[0]["MinAmtReschudle"]);
                        txtMaxAdvAmt.Text = Convert.ToString(dt.Rows[0]["MaxAdvAmt"]);
                        ddlPenalChargesGSTType.SelectedIndex = ddlPenalChargesGSTType.Items.IndexOf(ddlPenalChargesGSTType.Items.FindByValue(dt.Rows[0]["PenalChargesGSTType"].ToString().Trim()));
                        if (Convert.ToString(dt.Rows[0]["ApplyMonatorium"]) == "Y")
                        {
                            chkApplyMon.Checked = true;
                        }
                        else
                        {
                            chkApplyMon.Checked = false;
                        }
                        if (dt.Rows[0]["ApplyMonatorium"].ToString().Trim() == "Y")
                        {
                            dtDtl = new DataTable();
                            dtDtl = oGP.GetPayScheduleById(vLnTypId);
                            ViewState["Monatorium"] = dtDtl;
                            gvSchedule.DataSource = dtDtl;
                            gvSchedule.DataBind();
                        }
                        else
                        {
                            gvSchedule.DataSource = null;
                            gvSchedule.DataBind();
                        }

                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        StatusButton("Show");
                        btnAdd.Enabled = false;
                        btnEdit.Enabled = true;
                    }
                    else
                    {
                        ClearControls();
                        StatusButton("Show");
                        btnAdd.Enabled = true;
                        btnEdit.Enabled = false;
                    }
                }
            }
            finally
            {
                dt = null;
                oGP = null;
            }
        }
        private DataTable GetMonatorium()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("FromSlNo");
            dt.Columns.Add("ToSlNo", typeof(int));
            dt.Columns.Add("PrinYN", typeof(char));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["FromSlNo"] = 1;
            dt.Rows[0]["SlNo"] = 1;
            dt.Rows[0]["PrinYN"] = "N";
            return dt;
        }
        private void popMonatorium()
        {
            DataTable dt = null;
            dt = GetMonatorium();
            ViewState["Monatorium"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }
        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Monatorium"];
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            maxRow = gvSchedule.Rows.Count;
            vRow = dt.Rows.Count - 1;
            DataRow dr;
            TextBox txtMoTo = (TextBox)gvSchedule.Rows[curRow].FindControl("txtMoTo");
            Label lblSlNo = (Label)gvSchedule.Rows[curRow].FindControl("lblSlNo");
            Label lblFSLno = (Label)gvSchedule.Rows[curRow].FindControl("lblFSLno");
            TextBox txtToMax = (TextBox)gvSchedule.Rows[maxRow - 1].FindControl("txtTo");
            DropDownList ddlPYN = (DropDownList)gvSchedule.Rows[curRow].FindControl("ddlPrinYN");
            if (curRow < maxRow - 1)
            {
                if (Convert.ToInt32(lblFSLno.Text) < Convert.ToInt32(txtMaxIntsNo.Text))
                {
                    dt.Rows[curRow][2] = Convert.ToInt32(txtMoTo.Text);
                    dt.Rows[curRow + 1][1] = Convert.ToInt32(txtMoTo.Text) + 1;
                }
            }
            else
            {
                if (lblFSLno.Text != "")
                {
                    if (Convert.ToInt32(txtMoTo.Text) <= Convert.ToInt32(lblFSLno.Text))
                    {
                        gblFuction.MsgPopup("To Can not be Less than From");
                        dt.Rows[curRow][2] = Convert.ToInt32(dt.Rows[curRow][1]) + 1;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("From Can not be Greater than Installment No");
                    return;
                }
            }
            if (Convert.ToInt32(txtMoTo.Text) > Convert.ToInt32(txtMaxIntsNo.Text))
            {
                gblFuction.MsgPopup("To Can not be Greater than Installment No");
                dt.Rows[curRow][2] = txtMaxIntsNo.Text;
            }
            else
            {
                dt.Rows[curRow][2] = txtMoTo.Text;
                dt.Rows[curRow][3] = ddlPYN.SelectedValue;
            }

            if ((Convert.ToInt32(txtMoTo.Text)) >= Convert.ToInt32(txtMaxIntsNo.Text))
            {
                // dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(gvSchedule.Rows[vRow].Cells[0].Text) + 1;
                // dt.Rows[vRow + 1]["ApplyMo"] = "Y";
            }
            else
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["FromSlNo"] = Convert.ToInt32(dt.Rows[vRow]["ToSlNo"]) + 1;
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvSchedule.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.Rows[vRow + 1]["PrinYN"] = "N";
                dt.AcceptChanges();
            }

            ViewState["Monatorium"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();
        }
        protected void ImDel1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            dt = (DataTable)ViewState["Monatorium"];
            if (dt.Rows.Count > 1)
            {
                dt.Rows[gR.RowIndex].Delete();
                dt.AcceptChanges();
                ViewState["Monatorium"] = dt;
                gvSchedule.DataSource = dt;
                gvSchedule.DataBind();
            }
            else
            {
                popMonatorium();
                gblFuction.MsgPopup("First Row can not be deleted.");
                return;
            }

        }
        protected void chkApplyMon_CheckedChanged(Object sender, EventArgs args)
        {
            if (chkApplyMon.Checked == true)
            {
                if (txtMaxIntsNo.Text == "" || txtMaxIntsNo.Text == "0")
                {
                    gvSchedule.Enabled = false;
                    gblFuction.MsgPopup("Please Enter Installment No.");
                    chkApplyMon.Checked = false;
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
                    }
                    else
                        popMonatorium();
                }
            }

        }
        private void popAllAcHead()
        {
            DataTable dt = null;
            string vGenAcType = "X";
            Int32 vAllAcHD = 1;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAllAcHD);
            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";

        }
        protected void ddlPrinYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Monatorium"];
            int curRow = 0;
            DropDownList Cur = (DropDownList)sender;
            GridViewRow gR = (GridViewRow)Cur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtMoTo = (TextBox)gvSchedule.Rows[curRow].FindControl("txtMoTo");
            Label lblSlNo = (Label)gvSchedule.Rows[curRow].FindControl("lblSlNo");
            Label lblFSLno = (Label)gvSchedule.Rows[curRow].FindControl("lblFSLno");
            TextBox txtToMax = (TextBox)gvSchedule.Rows[curRow].FindControl("txtTo");
            DropDownList ddlPYN = (DropDownList)gvSchedule.Rows[curRow].FindControl("ddlPrinYN");

            dt.Rows[curRow][0] = lblSlNo.Text;
            dt.Rows[curRow][1] = lblFSLno.Text;
            dt.Rows[curRow][2] = txtMoTo.Text;
            dt.Rows[curRow][3] = ddlPYN.SelectedValue;
            ViewState["Monatorium"] = dt;
            gvSchedule.DataSource = dt;
            gvSchedule.DataBind();

        }
    }
}
