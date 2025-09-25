using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMCF.WebPages.Private.Master
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
            CGenParameter oGep = null;
            // Int32 vRows = 0;
            try
            {
                oGep = new CGenParameter();
                dt = oGep.GetActiveLnSchemePG();//pPgIndx, ref vRows
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();

            }
            finally
            {
                dt = null;
                oGep = null;
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

            ListItem Lst2 = new ListItem();
            Lst2.Text = "<--- Select --->";
            Lst2.Value = "-1";
            ddlODIntRec.DataTextField = "Desc";
            ddlODIntRec.DataValueField = "DescId";
            ddlODIntRec.DataSource = dt;
            ddlODIntRec.DataBind();
            ddlODIntRec.Items.Insert(0, Lst2);

            ListItem Lst3 = new ListItem();
            Lst3.Text = "<--- Select --->";
            Lst3.Value = "-1";
            ddlIntAccAc.DataTextField = "Desc";
            ddlIntAccAc.DataValueField = "DescId";
            ddlIntAccAc.DataSource = dt;
            ddlIntAccAc.DataBind();
            ddlIntAccAc.Items.Insert(0, Lst3);
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
            //ddlStmpChrgAC.DataTextField = "Desc";
            //ddlStmpChrgAC.DataValueField = "DescId";
            //ddlStmpChrgAC.DataSource = dt;
            //ddlStmpChrgAC.DataBind();
            //ddlStmpChrgAC.Items.Insert(0, Lst11);
        }
        private void PopIncome()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vIncome = 3;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncome);

            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";
            ddlProcFeeAc.DataTextField = "Desc";
            ddlProcFeeAc.DataValueField = "DescId";
            ddlProcFeeAc.DataSource = dt;
            ddlProcFeeAc.DataBind();
            ddlProcFeeAc.Items.Insert(0, Lst1);

            ListItem Lst2 = new ListItem();
            Lst2.Text = "<--- Select --->";
            Lst2.Value = "-1";
            ddlPenChrge.DataTextField = "Desc";
            ddlPenChrge.DataValueField = "DescId";
            ddlPenChrge.DataSource = dt;
            ddlPenChrge.DataBind();
            ddlPenChrge.Items.Insert(0, Lst2);

            ListItem Lst3 = new ListItem();
            Lst3.Text = "<--- Select --->";
            Lst3.Value = "-1";
            ddlBounceChrgAC.DataTextField = "Desc";
            ddlBounceChrgAC.DataValueField = "DescId";
            ddlBounceChrgAC.DataSource = dt;
            ddlBounceChrgAC.DataBind();
            ddlBounceChrgAC.Items.Insert(0, Lst3);

            ListItem Lst4 = new ListItem();
            Lst4.Text = "<--- Select --->";
            Lst4.Value = "-1";
            ddlWoffRec.DataTextField = "Desc";
            ddlWoffRec.DataValueField = "DescId";
            ddlWoffRec.DataSource = dt;
            ddlWoffRec.DataBind();
            ddlWoffRec.Items.Insert(0, Lst4);

            ListItem Lst5 = new ListItem();
            Lst5.Text = "<--- Select --->";
            Lst5.Value = "-1";
            ddlExcessChargeAC.DataTextField = "Desc";
            ddlExcessChargeAC.DataValueField = "DescId";
            ddlExcessChargeAC.DataSource = dt;
            ddlExcessChargeAC.DataBind();
            ddlExcessChargeAC.Items.Insert(0, Lst5);

            ListItem Lst6 = new ListItem();
            Lst6.Text = "<--- Select --->";
            Lst6.Value = "-1";
            ddlDocCh.DataTextField = "Desc";
            ddlDocCh.DataValueField = "DescId";
            ddlDocCh.DataSource = dt;
            ddlDocCh.DataBind();
            ddlDocCh.Items.Insert(0, Lst6);

            ListItem Lst7 = new ListItem();
            Lst7.Text = "<--- Select --->";
            Lst7.Value = "-1";
            ddlDocCGSTCh.DataTextField = "Desc";
            ddlDocCGSTCh.DataValueField = "DescId";
            ddlDocCGSTCh.DataSource = dt;
            ddlDocCGSTCh.DataBind();
            ddlDocCGSTCh.Items.Insert(0, Lst7);

            ListItem Lst8 = new ListItem();
            Lst8.Text = "<--- Select --->";
            Lst8.Value = "-1";
            ddlDocSGSTCh.DataTextField = "Desc";
            ddlDocSGSTCh.DataValueField = "DescId";
            ddlDocSGSTCh.DataSource = dt;
            ddlDocSGSTCh.DataBind();
            ddlDocSGSTCh.Items.Insert(0, Lst8);

            ListItem Lst9 = new ListItem();
            Lst9.Text = "<--- Select --->";
            Lst9.Value = "-1";
            ddlLoginFee.DataTextField = "Desc";
            ddlLoginFee.DataValueField = "DescId";
            ddlLoginFee.DataSource = dt;
            ddlLoginFee.DataBind();
            ddlLoginFee.Items.Insert(0, Lst9);

            ListItem Lst10 = new ListItem();
            Lst10.Text = "<--- Select --->";
            Lst10.Value = "-1";
            ddlLoginFeeCGST.DataTextField = "Desc";
            ddlLoginFeeCGST.DataValueField = "DescId";
            ddlLoginFeeCGST.DataSource = dt;
            ddlLoginFeeCGST.DataBind();
            ddlLoginFeeCGST.Items.Insert(0, Lst10);

            ListItem Lst11 = new ListItem();
            Lst11.Text = "<--- Select --->";
            Lst11.Value = "-1";
            ddlLoginFeeSGST.DataTextField = "Desc";
            ddlLoginFeeSGST.DataValueField = "DescId";
            ddlLoginFeeSGST.DataSource = dt;
            ddlLoginFeeSGST.DataBind();
            ddlLoginFeeSGST.Items.Insert(0, Lst11);

            ListItem Lst12 = new ListItem();
            Lst12.Text = "<--- Select --->";
            Lst12.Value = "-1";
            ddlPreEMI.DataTextField = "Desc";
            ddlPreEMI.DataValueField = "DescId";
            ddlPreEMI.DataSource = dt;
            ddlPreEMI.DataBind();
            ddlPreEMI.Items.Insert(0, Lst12);

            ListItem Lst13 = new ListItem();
            Lst13.Text = "<--- Select --->";
            Lst13.Value = "-1";
            ddlPreEMICGST.DataTextField = "Desc";
            ddlPreEMICGST.DataValueField = "DescId";
            ddlPreEMICGST.DataSource = dt;
            ddlPreEMICGST.DataBind();
            ddlPreEMICGST.Items.Insert(0, Lst13);

            ListItem Lst14 = new ListItem();
            Lst14.Text = "<--- Select --->";
            Lst14.Value = "-1";
            ddlPreEMISGST.DataTextField = "Desc";
            ddlPreEMISGST.DataValueField = "DescId";
            ddlPreEMISGST.DataSource = dt;
            ddlPreEMISGST.DataBind();
            ddlPreEMISGST.Items.Insert(0, Lst14);

            ListItem Lst15 = new ListItem();
            Lst15.Text = "<--- Select --->";
            Lst15.Value = "-1";
            ddlStampDuty.DataTextField = "Desc";
            ddlStampDuty.DataValueField = "DescId";
            ddlStampDuty.DataSource = dt;
            ddlStampDuty.DataBind();
            ddlStampDuty.Items.Insert(0, Lst15);

            ListItem Lst16 = new ListItem();
            Lst16.Text = "<--- Select --->";
            Lst16.Value = "-1";
            ddlStampDutyCGST.DataTextField = "Desc";
            ddlStampDutyCGST.DataValueField = "DescId";
            ddlStampDutyCGST.DataSource = dt;
            ddlStampDutyCGST.DataBind();
            ddlStampDutyCGST.Items.Insert(0, Lst16);

            ListItem Lst17 = new ListItem();
            Lst17.Text = "<--- Select --->";
            Lst17.Value = "-1";
            ddlStampDutySGST.DataTextField = "Desc";
            ddlStampDutySGST.DataValueField = "DescId";
            ddlStampDutySGST.DataSource = dt;
            ddlStampDutySGST.DataBind();
            ddlStampDutySGST.Items.Insert(0, Lst17);

            ListItem Lst18 = new ListItem();
            Lst18.Text = "<--- Select --->";
            Lst18.Value = "-1";
            ddlOthrCh.DataTextField = "Desc";
            ddlOthrCh.DataValueField = "DescId";
            ddlOthrCh.DataSource = dt;
            ddlOthrCh.DataBind();
            ddlOthrCh.Items.Insert(0, Lst18);

            ListItem Lst19 = new ListItem();
            Lst19.Text = "<--- Select --->";
            Lst19.Value = "-1";
            ddlOthrChCGST.DataTextField = "Desc";
            ddlOthrChCGST.DataValueField = "DescId";
            ddlOthrChCGST.DataSource = dt;
            ddlOthrChCGST.DataBind();
            ddlOthrChCGST.Items.Insert(0, Lst19);

            ListItem Lst20 = new ListItem();
            Lst20.Text = "<--- Select --->";
            Lst20.Value = "-1";
            ddlOthrChSGST.DataTextField = "Desc";
            ddlOthrChSGST.DataValueField = "DescId";
            ddlOthrChSGST.DataSource = dt;
            ddlOthrChSGST.DataBind();
            ddlOthrChSGST.Items.Insert(0, Lst20);

            ListItem Lst21 = new ListItem();
            Lst21.Text = "<--- Select --->";
            Lst21.Value = "-1";
            ddlVisitCh.DataTextField = "Desc";
            ddlVisitCh.DataValueField = "DescId";
            ddlVisitCh.DataSource = dt;
            ddlVisitCh.DataBind();
            ddlVisitCh.Items.Insert(0, Lst21);

            ListItem Lst22 = new ListItem();
            Lst22.Text = "<--- Select --->";
            Lst22.Value = "-1";
            ddlVisitCGST.DataTextField = "Desc";
            ddlVisitCGST.DataValueField = "DescId";
            ddlVisitCGST.DataSource = dt;
            ddlVisitCGST.DataBind();
            ddlVisitCGST.Items.Insert(0, Lst22);

            ListItem Lst23 = new ListItem();
            Lst23.Text = "<--- Select --->";
            Lst23.Value = "-1";
            ddlVisitSGST.DataTextField = "Desc";
            ddlVisitSGST.DataValueField = "DescId";
            ddlVisitSGST.DataSource = dt;
            ddlVisitSGST.DataBind();
            ddlVisitSGST.Items.Insert(0, Lst23);

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

           
            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";

            ddlCGSTAc.DataTextField = "Desc";
            ddlCGSTAc.DataValueField = "DescId";
            ddlCGSTAc.DataSource = dtLib;
            ddlCGSTAc.DataBind();
            ddlCGSTAc.Items.Insert(0, Lst1);

            ListItem Lst2 = new ListItem();
            Lst2.Text = "<--- Select --->";
            Lst2.Value = "-1";
            ddlSGSTAc.DataTextField = "Desc";
            ddlSGSTAc.DataValueField = "DescId";
            ddlSGSTAc.DataSource = dtLib;
            ddlSGSTAc.DataBind();
            ddlSGSTAc.Items.Insert(0, Lst2);

            ListItem Lst3 = new ListItem();
            Lst3.Text = "<--- Select --->";
            Lst3.Value = "-1";
            ddlAssetInsAC.DataTextField = "Desc";
            ddlAssetInsAC.DataValueField = "DescId";
            ddlAssetInsAC.DataSource = dtLib;
            ddlAssetInsAC.DataBind();
            ddlAssetInsAC.Items.Insert(0, Lst3);

            ListItem Lst4 = new ListItem();
            Lst4.Text = "<--- Select --->";
            Lst4.Value = "-1";
            ddlAssetInsCGST.DataTextField = "Desc";
            ddlAssetInsCGST.DataValueField = "DescId";
            ddlAssetInsCGST.DataSource = dtLib;
            ddlAssetInsCGST.DataBind();
            ddlAssetInsCGST.Items.Insert(0, Lst4);

            ListItem Lst5 = new ListItem();
            Lst5.Text = "<--- Select --->";
            Lst5.Value = "-1";
            ddlAssetInsSGST.DataTextField = "Desc";
            ddlAssetInsSGST.DataValueField = "DescId";
            ddlAssetInsSGST.DataSource = dtLib;
            ddlAssetInsSGST.DataBind();
            ddlAssetInsSGST.Items.Insert(0, Lst5);

            ListItem Lst6 = new ListItem();
            Lst6.Text = "<--- Select --->";
            Lst6.Value = "-1";
            ddlLifeInsAC.DataTextField = "Desc";
            ddlLifeInsAC.DataValueField = "DescId";
            ddlLifeInsAC.DataSource = dtLib;
            ddlLifeInsAC.DataBind();
            ddlLifeInsAC.Items.Insert(0, Lst6);

            ListItem Lst7 = new ListItem();
            Lst7.Text = "<--- Select --->";
            Lst7.Value = "-1";
            ddlLifeInsCGST.DataTextField = "Desc";
            ddlLifeInsCGST.DataValueField = "DescId";
            ddlLifeInsCGST.DataSource = dtLib;
            ddlLifeInsCGST.DataBind();
            ddlLifeInsCGST.Items.Insert(0, Lst7);

            ListItem Lst8 = new ListItem();
            Lst8.Text = "<--- Select --->";
            Lst8.Value = "-1";
            ddlLifeInsSGST.DataTextField = "Desc";
            ddlLifeInsSGST.DataValueField = "DescId";
            ddlLifeInsSGST.DataSource = dtLib;
            ddlLifeInsSGST.DataBind();
            ddlLifeInsSGST.Items.Insert(0, Lst8);

            ListItem Lst9 = new ListItem();
            Lst9.Text = "<--- Select --->";
            Lst9.Value = "-1";
            ddlAdvAc.DataTextField = "Desc";
            ddlAdvAc.DataValueField = "DescId";
            ddlAdvAc.DataSource = dtLib;
            ddlAdvAc.DataBind();
            ddlAdvAc.Items.Insert(0, Lst9);

            ListItem Lst10 = new ListItem();
            Lst10.Text = "<--- Select --->";
            Lst10.Value = "-1";
            ddlSusIntInc.DataTextField = "Desc";
            ddlSusIntInc.DataValueField = "DescId";
            ddlSusIntInc.DataSource = dtLib;
            ddlSusIntInc.DataBind();
            ddlSusIntInc.Items.Insert(0, Lst10);

            ListItem Lst11 = new ListItem();
            Lst11.Text = "<--- Select --->";
            Lst11.Value = "-1";
            ddlServTax.DataTextField = "Desc";
            ddlServTax.DataValueField = "DescId";
            ddlServTax.DataSource = dtLib;
            ddlServTax.DataBind();
            ddlServTax.Items.Insert(0, Lst11);
        }
        private void ClearControls()
        {
            ddlLoanAc.SelectedIndex = -1;
            ddlLnIntAc.SelectedIndex = -1;
            ddlProcFeeAc.SelectedIndex = -1;            
            ddlWoff.SelectedIndex = -1;            
            ddlPenChrge.SelectedIndex = -1;            
            ddlBounceChrgAC.SelectedIndex = -1;
            ddlBounceChrgWaveAC.SelectedIndex = -1;
            ddlPreCloseChrgeAC.SelectedIndex = -1;
            ddlPreCloseChrgeWaiveAC.SelectedIndex = -1;                      
            ddlCGSTAc.SelectedIndex = -1;
            ddlSGSTAc.SelectedIndex = -1;       
            ddlWoffRec.SelectedIndex = -1;           
            ddlCersai.SelectedIndex = -1;          
            ddlExcessChargeAC.SelectedIndex = -1;
            ddlAssetInsSGST.SelectedIndex = -1;
            ddlAssetInsCGST.SelectedIndex = -1;
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
            
            ddlLnIntTyp.SelectedIndex = -1;
            ddlPaySchedul.SelectedIndex = -1;
            ddlAssetInsAC.SelectedIndex = -1;
            ddlAssetInsCGST.SelectedIndex = -1;
            ddlAssetInsSGST.SelectedIndex = -1;
            ddlLifeInsAC.SelectedIndex = -1;
            ddlLifeInsCGST.SelectedIndex = -1;
            ddlLifeInsSGST.SelectedIndex = -1;
            ddlDocCh.SelectedIndex = -1;
            ddlDocCGSTCh.SelectedIndex = -1;
            ddlDocSGSTCh.SelectedIndex = -1;
            ddlLoginFee.SelectedIndex = -1;
            ddlLoginFeeCGST.SelectedIndex = -1;
            ddlLoginFeeSGST.SelectedIndex = -1;
            ddlPreEMI.SelectedIndex = -1;
            ddlPreEMICGST.SelectedIndex = -1;
            ddlPreEMISGST.SelectedIndex = -1;
            ddlStampDuty.SelectedIndex = -1;
            ddlStampDutyCGST.SelectedIndex = -1;
            ddlStampDutySGST.SelectedIndex = -1;
            ddlOthrCh.SelectedIndex = -1;
            ddlOthrChCGST.SelectedIndex = -1;
            ddlOthrChSGST.SelectedIndex = -1;
            ddlVisitCh.SelectedIndex = -1;
            ddlVisitCGST.SelectedIndex = -1;
            ddlVisitSGST.SelectedIndex = -1;
            ddlServTax.SelectedIndex = -1;
          
        }
        private void EnableControl(bool Status)
        {
            ddlLoanAc.Enabled = Status;
            ddlLnIntAc.Enabled = Status;
            ddlProcFeeAc.Enabled = Status;
          
            ddlWoff.Enabled = Status;           
            ddlPenChrge.Enabled = Status;                   
            ddlBounceChrgAC.Enabled = Status;
            ddlBounceChrgWaveAC.Enabled = Status;
            ddlPreCloseChrgeAC.Enabled = Status;
            ddlPreCloseChrgeWaiveAC.Enabled = Status;
            ddlCGSTAc.Enabled = Status;
            ddlSGSTAc.Enabled = Status;           
            ddlWoffRec.Enabled = Status;          
            txtEffRedIntRate.Enabled = Status;            
            ddlCersai.Enabled = Status;        
            ddlExcessChargeAC.Enabled = Status;
            ddlAssetInsSGST.Enabled = Status;
            ddlAssetInsCGST.Enabled = Status;
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

            ddlLnIntTyp.Enabled = Status;
            ddlPaySchedul.Enabled = Status;
            ddlAssetInsAC.Enabled = Status;
            ddlAssetInsCGST.Enabled = Status;
            ddlAssetInsSGST.Enabled = Status;
            ddlLifeInsAC.Enabled = Status;
            ddlLifeInsCGST.Enabled = Status;
            ddlLifeInsSGST.Enabled = Status;
            ddlDocCh.Enabled = Status;
            ddlDocCGSTCh.Enabled = Status;
            ddlDocSGSTCh.Enabled = Status;
            ddlLoginFee.Enabled = Status;
            ddlLoginFeeCGST.Enabled = Status;
            ddlLoginFeeSGST.Enabled = Status;
            ddlPreEMI.Enabled = Status;
            ddlPreEMICGST.Enabled = Status;
            ddlPreEMISGST.Enabled = Status;
            ddlStampDuty.Enabled = Status;
            ddlStampDutyCGST.Enabled = Status;
            ddlStampDutySGST.Enabled = Status;
            ddlOthrCh.Enabled = Status;
            ddlOthrChCGST.Enabled = Status;
            ddlOthrChSGST.Enabled = Status;
            ddlVisitCh.Enabled = Status;
            ddlVisitCGST.Enabled = Status;
            ddlVisitSGST.Enabled = Status;
            ddlServTax.Enabled = Status;
        }
        private bool ValidateField()
        {
            bool vRes = true;
            
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
          
            if (ddlProcFeeAc.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Processing A/c Can not be Empty");
                return vRes = false;
            }
            
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
            if (ddlBounceChrgAC.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Bounce Charge A/c..");
                return vRes = false;
            }           
            if (ddlCersai.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Select Mediclaim AC ..");
                return vRes = false;
            }            
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
            if (txtEffRedIntRate.Text == "" || txtEffRedIntRate.Text == "0")
            {
                gblFuction.MsgPopup("Effective Reducing Interest Rate Can Not Be Empty or zero");
                return vRes = false;
            }
            return vRes;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Int32 vLnTypId = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("Show");
                LoadGrid(0);
                if (ViewState["LoanTypeId"] != null)
                {
                    vLnTypId = Convert.ToInt32(ViewState["LoanTypeId"]);
                    GetParameterDetails(vLnTypId);
                }
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {                    
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vXml = "", vApplyMonatorium = "", vApplyBulletPaymnt = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0, vMaxInstNo = 0, vMinInstNo = 0;
            double vProc = 0, vMaxIntrate = 0, vMinIntrate = 0, vMaxIntFlat = 0, vEffRedIntRate = 0, vMinAmtReschudle = 0, vMaxAdvAmt = 0;
            string vPSevtx = "", vISevTax = "",IsLegVerProcAppYN="Y";

            CGenParameter oGP = null;
          
            oGP = new CGenParameter();


            try
            {
                if (ValidateField() == false)
                    return false;
                              
                if (txtEffRedIntRate.Text != "")
                    vEffRedIntRate = Convert.ToDouble(txtEffRedIntRate.Text);

                if (txtMinAmtReschudle.Text.Trim() != "")
                    vMinAmtReschudle = Convert.ToDouble(txtMinAmtReschudle.Text.Trim());

                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);

                
                if (Mode == "Save")
                {
                    oGP = new CGenParameter();

                    vErr = oGP.InsertLnParameter(vLoanTypeId, vApplyMonatorium, ddlPaySchedul.SelectedValue, ddlLnIntTyp.SelectedValue, vProc, ddlLoanAc.SelectedValue,
                    ddlLnIntAc.SelectedValue, ddlPenChrge.SelectedValue,
                    ddlProcFeeAc.SelectedValue,
                    ddlWoff.SelectedValue, 
                    this.UserID, vPSevtx, vISevTax,  vMaxInstNo, vMinInstNo, vMaxIntrate, vMinIntrate, vMaxIntFlat,
                    vApplyBulletPaymnt, ddlBounceChrgAC.SelectedValue, ddlBounceChrgWaveAC.SelectedValue, ddlPreCloseChrgeAC.SelectedValue,
                    ddlPreCloseChrgeWaiveAC.SelectedValue, ddlCGSTAc.SelectedValue, ddlSGSTAc.SelectedValue,
                    ddlWoffRec.SelectedValue,  vEffRedIntRate,
                    IsLegVerProcAppYN,ddlCersai.SelectedValue,
                    ddlExcessChargeAC.SelectedValue.ToString(),
                    ddlODIntRec.SelectedValue, ddlSusIntInc.SelectedValue,ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue,
                    Convert.ToDouble(txtPenCharges.Text), Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue,
                    Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue, vMinAmtReschudle, vMaxAdvAmt,
                    ddlAssetInsAC.SelectedValue, ddlAssetInsCGST.SelectedValue,ddlAssetInsSGST.SelectedValue,ddlLifeInsAC.SelectedValue,
                    ddlLifeInsCGST.SelectedValue, ddlLifeInsSGST.SelectedValue, ddlDocCh.SelectedValue,
                    ddlDocCGSTCh.SelectedValue,ddlDocSGSTCh.SelectedValue,ddlLoginFee.SelectedValue,ddlLoginFeeCGST.SelectedValue,ddlLoginFeeSGST.SelectedValue,
                    ddlPreEMI.SelectedValue,ddlPreEMICGST.SelectedValue,ddlPreEMISGST.SelectedValue,ddlStampDuty.SelectedValue,ddlStampDutyCGST.SelectedValue,
                    ddlStampDutySGST.SelectedValue, ddlOthrCh.SelectedValue, ddlOthrChCGST.SelectedValue, ddlOthrChSGST.SelectedValue,
                    ddlVisitCh.SelectedValue, ddlVisitCGST.SelectedValue, ddlVisitSGST.SelectedValue, ddlServTax.SelectedValue);

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
                    ddlProcFeeAc.SelectedValue,
                    ddlWoff.SelectedValue,
                    this.UserID, vPSevtx, vISevTax, vMaxInstNo, vMinInstNo, vMaxIntrate, vMinIntrate, vMaxIntFlat,
                    vApplyBulletPaymnt, ddlBounceChrgAC.SelectedValue, ddlBounceChrgWaveAC.SelectedValue, ddlPreCloseChrgeAC.SelectedValue,
                    ddlPreCloseChrgeWaiveAC.SelectedValue, ddlCGSTAc.SelectedValue, ddlSGSTAc.SelectedValue,
                    ddlWoffRec.SelectedValue, vEffRedIntRate,
                    IsLegVerProcAppYN, ddlCersai.SelectedValue,
                    ddlExcessChargeAC.SelectedValue.ToString(),
                    ddlODIntRec.SelectedValue, ddlSusIntInc.SelectedValue, ddlAdvAc.SelectedValue, ddlIntAccAc.SelectedValue,
                    Convert.ToDouble(txtPenCharges.Text), Convert.ToDouble(txtBounceCharges.Text), ddlBounceChargesGSTType.SelectedValue,
                    Convert.ToDouble(txtVisitCharges.Text), ddlVisitChargesGSTType.SelectedValue, vMinAmtReschudle, vMaxAdvAmt,
                    ddlAssetInsAC.SelectedValue, ddlAssetInsCGST.SelectedValue, ddlAssetInsSGST.SelectedValue, ddlLifeInsAC.SelectedValue,
                    ddlLifeInsCGST.SelectedValue, ddlLifeInsSGST.SelectedValue, ddlDocCh.SelectedValue,
                    ddlDocCGSTCh.SelectedValue, ddlDocSGSTCh.SelectedValue, ddlLoginFee.SelectedValue, ddlLoginFeeCGST.SelectedValue, ddlLoginFeeSGST.SelectedValue,
                    ddlPreEMI.SelectedValue, ddlPreEMICGST.SelectedValue, ddlPreEMISGST.SelectedValue, ddlStampDuty.SelectedValue, ddlStampDutyCGST.SelectedValue,
                    ddlStampDutySGST.SelectedValue, ddlOthrCh.SelectedValue, ddlOthrChCGST.SelectedValue, ddlOthrChSGST.SelectedValue,
                    ddlVisitCh.SelectedValue, ddlVisitCGST.SelectedValue, ddlVisitSGST.SelectedValue, ddlServTax.SelectedValue);
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
                    oGP = new CGenParameter();
                    vRec = oGP.ChkDelete(vLoanTypeId, "LoanTypeId", "LoanApplication");
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
                    GetParameterDetails(vLnTypId);
                                      
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oGP = null;
            }
        }
        protected void GetParameterDetails(Int32 LnTypId)
        {
            CGenParameter oGP = null; DataTable dt = null; 
            try
            {             
                oGP = new CGenParameter();
                dt = oGP.GetParameterDetails(LnTypId);
                if (dt.Rows.Count > 0)
                {
                    ddlLoanAc.SelectedIndex = ddlLoanAc.Items.IndexOf(ddlLoanAc.Items.FindByValue(dt.Rows[0]["LoanAC"].ToString().Trim()));
                    ddlLnIntAc.SelectedIndex = ddlLnIntAc.Items.IndexOf(ddlLnIntAc.Items.FindByValue(dt.Rows[0]["InstAC"].ToString().Trim()));
                    ddlProcFeeAc.SelectedIndex = ddlProcFeeAc.Items.IndexOf(ddlProcFeeAc.Items.FindByValue(dt.Rows[0]["ProcAC"].ToString().Trim()));
                    ddlWoff.SelectedIndex = ddlWoff.Items.IndexOf(ddlWoff.Items.FindByValue(dt.Rows[0]["WriteOffAC"].ToString().Trim()));

                    if (dt.Rows[0]["WriteOffRecAC"].ToString() != "")
                        ddlWoffRec.SelectedIndex = ddlWoffRec.Items.IndexOf(ddlWoffRec.Items.FindByValue(dt.Rows[0]["WriteOffRecAC"].ToString().Trim()));
                    else
                        ddlWoffRec.SelectedIndex = -1;


                    if (dt.Rows[0]["PenaltyChargeAC"].ToString() != "")
                        ddlPenChrge.SelectedIndex = ddlPenChrge.Items.IndexOf(ddlPenChrge.Items.FindByValue(dt.Rows[0]["PenaltyChargeAC"].ToString().Trim()));
                    else
                        ddlPenChrge.SelectedIndex = -1;

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

                    if (dt.Rows[0]["CERSAIAC"].ToString() != "")
                        ddlCersai.SelectedIndex = ddlCersai.Items.IndexOf(ddlCersai.Items.FindByValue(dt.Rows[0]["CERSAIAC"].ToString().Trim()));
                    else
                        ddlCersai.SelectedIndex = -1;


                    if (dt.Rows[0]["ExcessChargeAC"].ToString() != "")
                        ddlExcessChargeAC.SelectedIndex = ddlExcessChargeAC.Items.IndexOf(ddlExcessChargeAC.Items.FindByValue(dt.Rows[0]["ExcessChargeAC"].ToString().Trim()));
                    else
                        ddlExcessChargeAC.SelectedIndex = -1;

                    ddlAdvAc.SelectedIndex = ddlAdvAc.Items.IndexOf(ddlAdvAc.Items.FindByValue(dt.Rows[0]["AdvAC"].ToString().Trim()));
                    ddlIntAccAc.SelectedIndex = ddlIntAccAc.Items.IndexOf(ddlIntAccAc.Items.FindByValue(dt.Rows[0]["IntAccruedAc"].ToString().Trim()));
                    ddlODIntRec.SelectedIndex = ddlODIntRec.Items.IndexOf(ddlODIntRec.Items.FindByValue(dt.Rows[0]["ODIntRec"].ToString().Trim()));
                    ddlSusIntInc.SelectedIndex = ddlSusIntInc.Items.IndexOf(ddlSusIntInc.Items.FindByValue(dt.Rows[0]["SusIntInc"].ToString().Trim()));
                    txtEffRedIntRate.Text = Convert.ToString(dt.Rows[0]["EffRedIntRate"]);
                    txtPenCharges.Text = Convert.ToString(dt.Rows[0]["PenCharges"]);
                    txtBounceCharges.Text = Convert.ToString(dt.Rows[0]["BounceCharges"]);
                    ddlBounceChargesGSTType.SelectedIndex = ddlBounceChargesGSTType.Items.IndexOf(ddlBounceChargesGSTType.Items.FindByValue(dt.Rows[0]["BounceChargesGSTType"].ToString().Trim()));
                    txtVisitCharges.Text = Convert.ToString(dt.Rows[0]["VisitCharges"]);
                    ddlVisitChargesGSTType.SelectedIndex = ddlVisitChargesGSTType.Items.IndexOf(ddlVisitChargesGSTType.Items.FindByValue(dt.Rows[0]["VisitChargesGSTType"].ToString().Trim()));
                    txtMinAmtReschudle.Text = Convert.ToString(dt.Rows[0]["MinAmtReschudle"]);

                    // ASSET INS
                    if (dt.Rows[0]["AssetInsAC"].ToString() != "")
                        ddlAssetInsAC.SelectedIndex = ddlAssetInsAC.Items.IndexOf(ddlAssetInsAC.Items.FindByValue(dt.Rows[0]["AssetInsAC"].ToString().Trim()));
                    else
                        ddlAssetInsAC.SelectedIndex = -1;

                    if (dt.Rows[0]["AssetInsCGST"].ToString() != "")
                        ddlAssetInsCGST.SelectedIndex = ddlAssetInsCGST.Items.IndexOf(ddlAssetInsCGST.Items.FindByValue(dt.Rows[0]["AssetInsCGST"].ToString().Trim()));
                    else
                        ddlAssetInsCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["AssetInsSGST"].ToString() != "")
                        ddlAssetInsSGST.SelectedIndex = ddlAssetInsSGST.Items.IndexOf(ddlAssetInsSGST.Items.FindByValue(dt.Rows[0]["AssetInsSGST"].ToString().Trim()));
                    else
                        ddlAssetInsSGST.SelectedIndex = -1;
                    // LIFE INSU
                    if (dt.Rows[0]["LifeInsAC"].ToString() != "")
                        ddlLifeInsAC.SelectedIndex = ddlLifeInsAC.Items.IndexOf(ddlLifeInsAC.Items.FindByValue(dt.Rows[0]["LifeInsAC"].ToString().Trim()));
                    else
                        ddlLifeInsAC.SelectedIndex = -1;

                    if (dt.Rows[0]["LifeInsCGST"].ToString() != "")
                        ddlLifeInsCGST.SelectedIndex = ddlLifeInsCGST.Items.IndexOf(ddlLifeInsCGST.Items.FindByValue(dt.Rows[0]["LifeInsCGST"].ToString().Trim()));
                    else
                        ddlLifeInsCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["LifeInsSGST"].ToString() != "")
                        ddlLifeInsSGST.SelectedIndex = ddlLifeInsSGST.Items.IndexOf(ddlLifeInsSGST.Items.FindByValue(dt.Rows[0]["LifeInsSGST"].ToString().Trim()));
                    else
                        ddlLifeInsSGST.SelectedIndex = -1;

                    // DOCUMENT HANDLING
                    if (dt.Rows[0]["DocCh"].ToString() != "")
                        ddlDocCh.SelectedIndex = ddlDocCh.Items.IndexOf(ddlDocCh.Items.FindByValue(dt.Rows[0]["DocCh"].ToString().Trim()));
                    else
                        ddlDocCh.SelectedIndex = -1;

                    if (dt.Rows[0]["DocChCGST"].ToString() != "")
                        ddlDocCGSTCh.SelectedIndex = ddlDocCGSTCh.Items.IndexOf(ddlDocCGSTCh.Items.FindByValue(dt.Rows[0]["DocChCGST"].ToString().Trim()));
                    else
                        ddlDocCGSTCh.SelectedIndex = -1;

                    if (dt.Rows[0]["DocChSGST"].ToString() != "")
                        ddlDocSGSTCh.SelectedIndex = ddlDocSGSTCh.Items.IndexOf(ddlDocSGSTCh.Items.FindByValue(dt.Rows[0]["DocChSGST"].ToString().Trim()));
                    else
                        ddlDocSGSTCh.SelectedIndex = -1;


                    // lOGIN FEE
                    if (dt.Rows[0]["LoginFee"].ToString() != "")
                        ddlLoginFee.SelectedIndex = ddlLoginFee.Items.IndexOf(ddlLoginFee.Items.FindByValue(dt.Rows[0]["LoginFee"].ToString().Trim()));
                    else
                        ddlLoginFee.SelectedIndex = -1;

                    if (dt.Rows[0]["LoginFeeCGST"].ToString() != "")
                        ddlLoginFeeCGST.SelectedIndex = ddlLoginFeeCGST.Items.IndexOf(ddlLoginFeeCGST.Items.FindByValue(dt.Rows[0]["LoginFeeCGST"].ToString().Trim()));
                    else
                        ddlLoginFeeCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["LoginFeeSGST"].ToString() != "")
                        ddlLoginFeeSGST.SelectedIndex = ddlLoginFeeSGST.Items.IndexOf(ddlLoginFeeSGST.Items.FindByValue(dt.Rows[0]["LoginFeeSGST"].ToString().Trim()));
                    else
                        ddlLoginFeeSGST.SelectedIndex = -1;

                    // PRE-EMI
                    if (dt.Rows[0]["PreEMI"].ToString() != "")
                        ddlPreEMI.SelectedIndex = ddlPreEMI.Items.IndexOf(ddlPreEMI.Items.FindByValue(dt.Rows[0]["PreEMI"].ToString().Trim()));
                    else
                        ddlPreEMI.SelectedIndex = -1;

                    if (dt.Rows[0]["PreEMICGST"].ToString() != "")
                        ddlPreEMICGST.SelectedIndex = ddlPreEMICGST.Items.IndexOf(ddlPreEMICGST.Items.FindByValue(dt.Rows[0]["PreEMICGST"].ToString().Trim()));
                    else
                        ddlPreEMICGST.SelectedIndex = -1;

                    if (dt.Rows[0]["PreEMISGST"].ToString() != "")
                        ddlPreEMISGST.SelectedIndex = ddlPreEMISGST.Items.IndexOf(ddlPreEMISGST.Items.FindByValue(dt.Rows[0]["PreEMISGST"].ToString().Trim()));
                    else
                        ddlPreEMISGST.SelectedIndex = -1;

                    // STAMP DUTY
                    if (dt.Rows[0]["StampDuty"].ToString() != "")
                        ddlStampDuty.SelectedIndex = ddlStampDuty.Items.IndexOf(ddlStampDuty.Items.FindByValue(dt.Rows[0]["StampDuty"].ToString().Trim()));
                    else
                        ddlStampDuty.SelectedIndex = -1;

                    if (dt.Rows[0]["StampDutyCGST"].ToString() != "")
                        ddlStampDutyCGST.SelectedIndex = ddlStampDutyCGST.Items.IndexOf(ddlStampDutyCGST.Items.FindByValue(dt.Rows[0]["StampDutyCGST"].ToString().Trim()));
                    else
                        ddlStampDutyCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["StampDutySGST"].ToString() != "")
                        ddlStampDutySGST.SelectedIndex = ddlStampDutySGST.Items.IndexOf(ddlStampDutySGST.Items.FindByValue(dt.Rows[0]["StampDutySGST"].ToString().Trim()));
                    else
                        ddlStampDutySGST.SelectedIndex = -1;

                    // OTHER CH. 
                    if (dt.Rows[0]["OthrCh"].ToString() != "")
                        ddlOthrCh.SelectedIndex = ddlOthrCh.Items.IndexOf(ddlOthrCh.Items.FindByValue(dt.Rows[0]["OthrCh"].ToString().Trim()));
                    else
                        ddlOthrCh.SelectedIndex = -1;

                    if (dt.Rows[0]["OthrChCGST"].ToString() != "")
                        ddlOthrChCGST.SelectedIndex = ddlOthrChCGST.Items.IndexOf(ddlOthrChCGST.Items.FindByValue(dt.Rows[0]["OthrChCGST"].ToString().Trim()));
                    else
                        ddlOthrChCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["OthrChSGST"].ToString() != "")
                        ddlOthrChSGST.SelectedIndex = ddlOthrChSGST.Items.IndexOf(ddlOthrChSGST.Items.FindByValue(dt.Rows[0]["OthrChSGST"].ToString().Trim()));
                    else
                        ddlOthrChSGST.SelectedIndex = -1;

                    // VISIT CH. 
                    if (dt.Rows[0]["VisitChargeAC"].ToString() != "")
                        ddlVisitCh.SelectedIndex = ddlVisitCh.Items.IndexOf(ddlVisitCh.Items.FindByValue(dt.Rows[0]["VisitChargeAC"].ToString().Trim()));
                    else
                        ddlVisitCh.SelectedIndex = -1;

                    if (dt.Rows[0]["VisitChargeCGST"].ToString() != "")
                        ddlVisitCGST.SelectedIndex = ddlVisitCGST.Items.IndexOf(ddlVisitCGST.Items.FindByValue(dt.Rows[0]["VisitChargeCGST"].ToString().Trim()));
                    else
                        ddlVisitCGST.SelectedIndex = -1;

                    if (dt.Rows[0]["VisitChargeSGST"].ToString() != "")
                        ddlVisitSGST.SelectedIndex = ddlVisitSGST.Items.IndexOf(ddlVisitSGST.Items.FindByValue(dt.Rows[0]["VisitChargeSGST"].ToString().Trim()));
                    else
                        ddlVisitSGST.SelectedIndex = -1;

                    if (dt.Rows[0]["ServiceTaxAC"].ToString() != "")
                        ddlServTax.SelectedIndex = ddlServTax.Items.IndexOf(ddlServTax.Items.FindByValue(dt.Rows[0]["ServiceTaxAC"].ToString().Trim()));
                    else
                        ddlServTax.SelectedIndex = -1;

                    if (dt.Rows[0]["PaySchedule"].ToString() != "")
                        ddlPaySchedul.SelectedIndex = ddlPaySchedul.Items.IndexOf(ddlPaySchedul.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString().Trim()));
                    else
                        ddlPaySchedul.SelectedIndex = -1;

                    if (dt.Rows[0]["InstType"].ToString() != "")
                        ddlLnIntTyp.SelectedIndex = ddlLnIntTyp.Items.IndexOf(ddlLnIntTyp.Items.FindByValue(dt.Rows[0]["InstType"].ToString().Trim()));
                    else
                        ddlLnIntTyp.SelectedIndex = -1;




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
            catch (Exception ex)
            {
                throw ex;
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
       
    }
}
