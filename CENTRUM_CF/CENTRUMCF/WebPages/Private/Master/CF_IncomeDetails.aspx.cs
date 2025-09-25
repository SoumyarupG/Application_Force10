using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_IncomeDetails : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopAssesMethod();
                if (Session[gblValue.LeadID] != null)
                {
                    GetIncomeDetails();                  
                    StatusButton("Edit");
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbBasicDet.ActiveTabIndex = 0;
            }

        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Income Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFIncDtl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Income Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void CheckOprtnStatus(Int64 vLeadID)
        {
            Int32 vErr = 0;
            CMember oMem = null;
            oMem = new CMember();
            vErr = oMem.CF_chkOperatnStatus(vLeadID);
            if (vErr == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                gblFuction.MsgPopup("This Lead is Under Process at Operation Stage.You can not Change or Update it...");
                return;
            }
            else
            {
                btnSave.Enabled = true;
                btnEdit.Enabled = true;
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
                // ViewState["StateEdit"] = "Edit";

                //   StatusButton("Edit");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            else vStateEdit = "Edit";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                //  LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                // ClearControls();
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 0;
            //  EnableControl(false);
            StatusButton("Edit");
            btnExit.Enabled = true;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
                    // btnExit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
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
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":
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
        private void EnableControl(Boolean Status)
        {
            ddlAssMethod.Enabled = false;

            if (hdnAssMthId.Value == "1") txtGrossIncMonthly.Enabled = Status;
            else txtGrossIncMonthly.Enabled = false;

            if (hdnAssMthId.Value == "3") txtGrossProfitMargin.Enabled = false;
            else txtGrossProfitMargin.Enabled = Status;


            txtProfAfterTax.Enabled = Status;
            txtDepreciation.Enabled = Status;
            txtAmortization.Enabled = Status;
            txtInterest.Enabled = Status;
            txtTaxes.Enabled = Status;

            txtTurnover.Enabled = Status;


            txtNoMonthsConsiBS.Enabled = Status;
            txtBankTurnMonthBS.Enabled = Status;
            txtProfMargMonthBS.Enabled = Status;

            txtOtherInc.Enabled = Status;
            txtAverElecBill.Enabled = Status;

            txtDeviFoirPerc.Enabled = Status;

            txtExistOblig.Enabled = Status;

            //  txtPerLakhEmi.Enabled = Status;


        }
        private void ClearControls()
        {
            lblBCPNo.Text = "";
            lblAppName.Text = "";
            hdnAssMthId.Value = "";
            hdnLeadId.Value = "";
            txtProfAfterTax.Text = "0";
            txtDepreciation.Text = "0";
            txtAmortization.Text = "0";
            txtInterest.Text = "0";
            txtTaxes.Text = "0";
            txtTotalIncAnnual.Text = "0";
            txtTurnover.Text = "0";
            txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";
            txtGrossIncAnnual.Text = "0";
            txtNoMonthsConsiBS.Text = "0";
            txtBankTurnMonthBS.Text = "0";
            txtProfMargMonthBS.Text = "0";
            txtGrossIncMonthly.Text = "0";
            txtOtherInc.Text = "0";
            txtAverElecBill.Text = "0";
            txtTotalIncMonthly.Text = "0";
            txtApplFoirPerc.Text = "70";
            txtFinalFoirConsi.Text = "70";
            txtDeviFoirPerc.Text = "0";
            txtFinalFoirConsi.Text = "0";
            txtFoirInc.Text = "0";
            txtExistOblig.Text = "0";
            txtNetIncToUfsbEmi.Text = "0";
            txtPerLakhEmi.Text = "0";
            txtLoanEligiblity.Text = "0";
            ddlAssMethod.SelectedIndex = -1;
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
                tbBasicDet.ActiveTabIndex = 0;
                StatusButton("Add");
                ClearControls();
                txtPerLakhEmi.Enabled = false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void PopAssesMethod()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(0);

                ddlAssMethod.DataSource = dt;
                ddlAssMethod.DataTextField = "MethodName";
                ddlAssMethod.DataValueField = "MethodID";
                ddlAssMethod.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethod.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        protected void GetIncomeDetails()
        {
            ClearControls();
            Int32 pAssMtdId = 0;
            string vBrCode = "", vIsFileUpload = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            string vStatus = ""; double vTotalIncome=0;
            ClearControls();
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblAppName.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNo.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }

                vBrCode = Session[gblValue.BrnchCode].ToString();

                if (Session[gblValue.LeadID] != null)
                {
                    hdnLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                }
                if (Session[gblValue.IncomeStatus] != null)
                {
                    vStatus = (Convert.ToString(Session[gblValue.IncomeStatus]));
                }
                if (Session[gblValue.AssMtdTypId] != null)
                {
                    pAssMtdId = Convert.ToInt32(Session[gblValue.AssMtdTypId]);
                }


                hdnAssMthId.Value = Convert.ToString(pAssMtdId);


                CCFIncDtl oDist = new CCFIncDtl();
                //if (vStatus != "Pending")
                //{
                ddlAssMethod.SelectedIndex = ddlAssMethod.Items.IndexOf(ddlAssMethod.Items.FindByValue(Convert.ToString(pAssMtdId)));


                StatusButton("Edit");
                ds = oDist.CF_GetIncomeDtlByLeadID(Convert.ToInt64(hdnLeadId.Value));
                dt = ds.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    ViewState["StateEdit"] = "Edit";
                    txtProfAfterTax.Text = Convert.ToString(dt.Rows[0]["ProfAfterTax"]);
                    txtDepreciation.Text = Convert.ToString(dt.Rows[0]["Depreciation"]);
                    txtAmortization.Text = Convert.ToString(dt.Rows[0]["Amortization"]);
                    txtInterest.Text = Convert.ToString(dt.Rows[0]["Interest"]);
                    txtTaxes.Text = Convert.ToString(dt.Rows[0]["Taxes"]);
                    txtTotalIncAnnual.Text = Convert.ToString(dt.Rows[0]["TotalIncAnnual"]);
                    txtTurnover.Text = Convert.ToString(dt.Rows[0]["Turnover"]);
                    txtGrossProfitMargin.Text = Convert.ToString(dt.Rows[0]["GrossProfitMargin"]);
                    txtGrossIncAnnual.Text = Convert.ToString(dt.Rows[0]["GrossIncAnnual"]);
                    txtNoMonthsConsiBS.Text = Convert.ToString(dt.Rows[0]["NoMonthsConsiBS"]);
                    txtBankTurnMonthBS.Text = Convert.ToString(dt.Rows[0]["BankTurnMonthBS"]);
                    txtProfMargMonthBS.Text = Convert.ToString(dt.Rows[0]["ProfMargMonthBS"]);
                    txtGrossIncMonthly.Text = Convert.ToString(dt.Rows[0]["GrossIncMonthly"]);
                    txtOtherInc.Text = Convert.ToString(dt.Rows[0]["OtherInc"]);
                    txtAverElecBill.Text = Convert.ToString(dt.Rows[0]["AverElecBill"]);
                    txtTotalIncMonthly.Text = Convert.ToString(dt.Rows[0]["TotalIncMonthly"]);
                    txtApplFoirPerc.Text = Convert.ToString(dt.Rows[0]["ApplFoirPerc"]);
                    txtDeviFoirPerc.Text = Convert.ToString(dt.Rows[0]["DeviFoirPerc"]);
                    txtFinalFoirConsi.Text = Convert.ToString(dt.Rows[0]["FinalFoirConsi"]);
                    txtFoirInc.Text = Convert.ToString(dt.Rows[0]["FoirInc"]);
                    txtExistOblig.Text = Convert.ToString(dt.Rows[0]["ExistOblig"]);
                    txtNetIncToUfsbEmi.Text = Convert.ToString(dt.Rows[0]["NetIncToUfsbEmi"]);
                    txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["PerLakhEmi"]);
                    txtLoanEligiblity.Text = Convert.ToString(dt.Rows[0]["LoanEligiblity"]);

                    vTotalIncome = Math.Round(Convert.ToDouble(txtGrossIncMonthly.Text) + Convert.ToDouble(txtOtherInc.Text) + Convert.ToDouble(txtAverElecBill.Text), 2);
                    txtTotalIncMonthly.Text = Convert.ToString(vTotalIncome);

                    tbBasicDet.ActiveTabIndex = 0;
                    EnableControl(true);
                    txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : txtGrossProfitMargin.Text;
                }
                else
                {

                    ddlAssMethod.SelectedIndex = ddlAssMethod.Items.IndexOf(ddlAssMethod.Items.FindByValue(Convert.ToString(pAssMtdId)));

                    EnableControl(true);
                    txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";

                    ViewState["StateEdit"] = "Add";
                    StatusButton("Add");
                    tbBasicDet.ActiveTabIndex = 0;
                    txtFinalFoirConsi.Text = "70";

                    ds = oDist.CF_CalculateEMIAmount(Convert.ToInt64(hdnLeadId.Value));
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["EMIAmt"]);
                        txtLoanEligiblity.Text = Convert.ToString(dt.Rows[0]["LoanEligiblity"]);
                    }


                }

                DynamicTableRow(pAssMtdId);

            }
            finally
            {
                dt = null;
            }
        }
        protected void ddlAssMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            hdnAssMthId.Value = ddlAssMethod.SelectedValue;
            DynamicTableRow(Convert.ToInt32(ddlAssMethod.SelectedValue));
            EnableControl(true);
            txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";
        }
        public void DynamicTableRow(int AssMthId)
        {
            switch (AssMthId)
            {
                case 1:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 2:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = true;
                    tr2IncDtlMonthBS.Visible = true;

                    break;
                case 3:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 4:
                    trHIncDtlFiP.Visible = true;
                    tr1IncDtlFiP.Visible = true;
                    tr2IncDtlFiP.Visible = true;
                    tr3IncDtlFiP.Visible = true;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;
                    break;
                case 5:

                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 6:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
            }

        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0; Int32 vUserID = Convert.ToInt32(Session[gblValue.UserId]);
            CCFIncDtl oMem = null; string vBrCode = ""; string vErrMsg = "";
            if (Convert.ToString(hdnLeadId.Value) == "")
            {
                gblFuction.MsgPopup("Please Select Lead From Grid");
                return false;
            }
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (Mode == "Save")
                {
                    oMem = new CCFIncDtl();
                    vErr = oMem.CF_SaveIncomeDetail(Convert.ToInt64(hdnLeadId.Value), Convert.ToInt32(hdnAssMthId.Value), Convert.ToDouble(txtProfAfterTax.Text),
                        Convert.ToDouble(txtDepreciation.Text), Convert.ToDouble(txtAmortization.Text), Convert.ToDouble(txtInterest.Text),
                        Convert.ToDouble(txtTaxes.Text), Convert.ToDouble(txtTotalIncAnnual.Text), Convert.ToDouble(txtTurnover.Text), Convert.ToDouble(txtGrossProfitMargin.Text)
                        , Convert.ToDouble(txtGrossIncAnnual.Text), Convert.ToDouble(txtNoMonthsConsiBS.Text), Convert.ToDouble(txtBankTurnMonthBS.Text),
                        Convert.ToDouble(txtProfMargMonthBS.Text), Convert.ToDouble(txtGrossIncMonthly.Text), Convert.ToDouble(txtOtherInc.Text),
                        Convert.ToDouble(txtAverElecBill.Text), Convert.ToDouble(txtTotalIncMonthly.Text), Convert.ToDouble(txtApplFoirPerc.Text),
                        Convert.ToDouble(txtDeviFoirPerc.Text), Convert.ToDouble(txtFinalFoirConsi.Text), Convert.ToDouble(txtFoirInc.Text), Convert.ToDouble(txtExistOblig.Text)
                        , Convert.ToDouble(txtNetIncToUfsbEmi.Text), Convert.ToDouble(txtPerLakhEmi.Text), Convert.ToDouble(txtLoanEligiblity.Text),
                        vBrCode, vUserID, "Save", ref vErrMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oMem = new CCFIncDtl();
                    vErr = oMem.CF_SaveIncomeDetail(Convert.ToInt64(hdnLeadId.Value), Convert.ToInt32(hdnAssMthId.Value), Convert.ToDouble(txtProfAfterTax.Text),
                    Convert.ToDouble(txtDepreciation.Text), Convert.ToDouble(txtAmortization.Text), Convert.ToDouble(txtInterest.Text),
                    Convert.ToDouble(txtTaxes.Text), Convert.ToDouble(txtTotalIncAnnual.Text), Convert.ToDouble(txtTurnover.Text), Convert.ToDouble(txtGrossProfitMargin.Text)
                    , Convert.ToDouble(txtGrossIncAnnual.Text), Convert.ToDouble(txtNoMonthsConsiBS.Text), Convert.ToDouble(txtBankTurnMonthBS.Text),
                    Convert.ToDouble(txtProfMargMonthBS.Text), Convert.ToDouble(txtGrossIncMonthly.Text), Convert.ToDouble(txtOtherInc.Text),
                    Convert.ToDouble(txtAverElecBill.Text), Convert.ToDouble(txtTotalIncMonthly.Text), Convert.ToDouble(txtApplFoirPerc.Text),
                    Convert.ToDouble(txtDeviFoirPerc.Text), Convert.ToDouble(txtFinalFoirConsi.Text), Convert.ToDouble(txtFoirInc.Text), Convert.ToDouble(txtExistOblig.Text)
                    , Convert.ToDouble(txtNetIncToUfsbEmi.Text), Convert.ToDouble(txtPerLakhEmi.Text), Convert.ToDouble(txtLoanEligiblity.Text),
                    vBrCode, vUserID, "Edit", ref vErrMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else
                {
                    oMem = new CCFIncDtl();
                    vErr = oMem.CF_SaveIncomeDetail(Convert.ToInt64(hdnLeadId.Value), Convert.ToInt32(hdnAssMthId.Value), Convert.ToDouble(txtProfAfterTax.Text),
                        Convert.ToDouble(txtDepreciation.Text), Convert.ToDouble(txtAmortization.Text), Convert.ToDouble(txtInterest.Text),
                        Convert.ToDouble(txtTaxes.Text), Convert.ToDouble(txtTotalIncAnnual.Text), Convert.ToDouble(txtTurnover.Text), Convert.ToDouble(txtGrossProfitMargin.Text)
                        , Convert.ToDouble(txtGrossIncAnnual.Text), Convert.ToDouble(txtNoMonthsConsiBS.Text), Convert.ToDouble(txtBankTurnMonthBS.Text),
                        Convert.ToDouble(txtProfMargMonthBS.Text), Convert.ToDouble(txtGrossIncMonthly.Text), Convert.ToDouble(txtOtherInc.Text),
                        Convert.ToDouble(txtAverElecBill.Text), Convert.ToDouble(txtTotalIncMonthly.Text), Convert.ToDouble(txtApplFoirPerc.Text),
                        Convert.ToDouble(txtDeviFoirPerc.Text), Convert.ToDouble(txtFinalFoirConsi.Text), Convert.ToDouble(txtFoirInc.Text), Convert.ToDouble(txtExistOblig.Text)
                        , Convert.ToDouble(txtNetIncToUfsbEmi.Text), Convert.ToDouble(txtPerLakhEmi.Text), Convert.ToDouble(txtLoanEligiblity.Text),
                        vBrCode, vUserID, "Delete", ref vErrMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
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
                oMem = null;
            }
        }
    }
}