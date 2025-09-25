using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanSanction : CENTRUMBAse
    {
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        string vMobService = ConfigurationManager.AppSettings["MobService"];
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["CGTID"] = null;
                string vLogInDate = Convert.ToString(Session[gblValue.LoginDate]);
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddMonths(-1));
                txtToDt.Text = vLogInDate;
                ceToDt.EndDate = gblFuction.setDate(vLogInDate);
                ceLnSancDate.EndDate = gblFuction.setDate(vLogInDate);
                hdUserID.Value = this.UserID.ToString();
                LoadGrid(1);
                PopInsuranceCompany();
                // PopApplication();
                StatusButton("View");
                //PopPPI();
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
                this.PageHeading = "Loan Sanction";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanSanction);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnSendBack.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnSendBack.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Sanction", false);
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
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSendBack.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSendBack.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = false;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            ddlCust.Enabled = Status;
            ddlSancStatus.Enabled = Status;
            txtAppDt.Enabled = Status;
            txtLnAmt.Enabled = Status;
            ddlAppNo.Enabled = Status;
            txtLnSancDate.Enabled = Status;
            txtSancAmt.Enabled = Status;
            ddlLnSctr.Enabled = Status;
            ddlLnSchem.Enabled = Status;
            txtFIntRate.Enabled = Status;
            txtRIntRate.Enabled = Status;
            txtInstNo.Enabled = Status;
            txtPeriod.Enabled = Status;
            ddlInstType.Enabled = Status;
            txtEMI.Enabled = Status;
            txtLPF.Enabled = Status;
            txtLPFST.Enabled = Status;
            txtInsuFee.Enabled = Status;
            txtInsuCGSTAmt.Enabled = Status;
            txtRepayStDt.Enabled = Status;
            // txtSTaxAmt.Enabled = Status;
            txtLPFKKRate.Enabled = Status;
            // txtSTaxAmt.Enabled = Status;
            ddlRepayType.Enabled = Status;
            txtNetDisbAmt.Enabled = Status;
            chkAdvEMI.Enabled = Status;
            txtApprovedBy.Enabled = false;
            txtApproveDt.Enabled = false;
            txtRemarks.Enabled = Status;
            txtRIntRate.Enabled = Status;
            txtLPFKKTax.Enabled = Status;
            txtLPFSBTax.Enabled = Status;
            txtInsuSGSTAmt.Enabled = Status;
            txtInsuSBTax.Enabled = Status;
            txtLPFPer.Enabled = Status;
            txtSTaxPer.Enabled = Status;
            txtLPFSBRate.Enabled = Status;
            txtAppCharge.Enabled = Status;
            txtStampChrge.Enabled = Status;
            txtDisbDate.Enabled = Status;
            txtPreEMIInt.Enabled = Status;
            txtPreLnBal.Enabled = Status;
            txtCGSTPer.Enabled = Status;
            txtCGSTAmt.Enabled = Status;
            txtSGSTPer.Enabled = Status;
            txtSGSTAmt.Enabled = Status;
            txtIGSTAmt.Enabled = Status;
            txtFLDGPer.Enabled = Status;
            txtFLDGAmt.Enabled = Status;
            ChkSecurityChk1.Enabled = Status;
            ChkSecurityChk2.Enabled = Status;
            ChkSecurityChk3.Enabled = Status;
            ddlPreLoanId.Enabled = Status;
            txtBrkPrdIntAct.Enabled = Status;
            txtBrkPrdIntWave.Enabled = Status;
            txtBrkPrdInt.Enabled = Status;

            txtPropInsuAmt.Enabled = Status;
            txtPropInsuCGST.Enabled = Status;
            txtPropInsuSGST.Enabled = Status;
            txtPropInsuIGST.Enabled = Status;
            chkIGSTPF.Enabled = Status;
            chkIGSTInsu.Enabled = Status;
            chkIGSTPropInsu.Enabled = Status;
            txtAdminFees.Enabled = Status;
            txtTechFees.Enabled = Status;

            txtCERSAICharge.Enabled = Status;
            txtCERSAIChargeCGST.Enabled = Status;
            txtCERSAIChargeSGST.Enabled = Status;
            txtCERSAIChargeIGST.Enabled = Status;
            chkIGSTCERSAICharge.Enabled = Status;

            ddlIC.Enabled = Status;
        }
        private void ClearControls()
        {
            ddlCust.SelectedIndex = -1;
            ddlSancStatus.SelectedIndex = -1;
            txtAppDt.Text = "";
            txtLnAmt.Text = "";
            ddlAppNo.SelectedIndex = -1;
            txtLoanSancNo.Text = "";
            txtLnSancDate.Text = "";
            txtSancAmt.Text = "";
            ddlLnSctr.SelectedIndex = -1;
            ddlLnSchem.SelectedIndex = -1;
            txtFIntRate.Text = "";
            txtInstNo.Text = "";
            txtPeriod.Text = "";
            ddlInstType.SelectedIndex = -1;
            txtEMI.Text = "";
            txtLPF.Text = "";
            txtLPFST.Text = "";
            txtInsuFee.Text = "";
            txtInsuCGSTAmt.Text = "0";
            txtInsuSGSTAmt.Text = "0";
            txtInsuIGSTAmt.Text = "0";
            txtRepayStDt.Text = "";
            ddlRepayType.SelectedIndex = -1;
            txtNetDisbAmt.Text = "0";
            chkAdvEMI.Checked = false;
            txtApprovedBy.Text = "";
            txtApproveDt.Text = "";
            txtRemarks.Text = "";
            txtAppCharge.Text = "0";
            txtStampChrge.Text = "0";
            txtPreEMIInt.Text = "0";
            txtDisbDate.Text = "0";
            txtPreLnBal.Text = "0";
            txtCGSTPer.Text = "0";
            txtCGSTAmt.Text = "0";
            txtSGSTPer.Text = "0";
            txtIGSTAmt.Text = "0";
            txtSGSTAmt.Text = "0";
            txtFLDGPer.Text = "0";
            txtFLDGAmt.Text = "0";
            ChkSecurityChk1.Checked = false;
            ChkSecurityChk2.Checked = false;
            ChkSecurityChk3.Checked = false;
            ddlPreLoanId.SelectedIndex = -1;
            txtBrkPrdIntAct.Text = "0";
            txtBrkPrdIntWave.Text = "0";
            txtBrkPrdInt.Text = "0";
            txtPropInsuAmt.Text = "0";
            txtPropInsuCGST.Text = "0";
            txtPropInsuSGST.Text = "0";
            txtPropInsuIGST.Text = "0";
            chkIGSTPF.Checked = false;
            chkIGSTInsu.Checked = false;
            chkIGSTPropInsu.Checked = false;
            txtAdminFees.Text = "0";
            txtTechFees.Text = "0";
            txtCERSAICharge.Text = "0";
            txtCERSAIChargeCGST.Text = "0";
            txtCERSAIChargeSGST.Text = "0";
            txtCERSAIChargeIGST.Text = "0";
            chkIGSTCERSAICharge.Checked = false;
            ddlIC.SelectedIndex = -1;
        }
        private void PopCustomerForSanc()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetCustForSanc();
                if (dt.Rows.Count > 0)
                {
                    ddlCust.DataTextField = "CompanyName";
                    ddlCust.DataValueField = "CustId";
                    ddlCust.DataSource = dt;
                    ddlCust.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlCust.Items.Insert(0, oItm);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void PopBranch()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranch();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oItm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void FillRate()
        {
            CApplication oCM = null;
            DataTable dt = null;
            oCM = new CApplication();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetTaxRate();
                if (dt.Rows.Count > 0)
                {
                    txtSTaxPer.Text = dt.Rows[0]["STPer"].ToString();
                    txtLPFKKRate.Text = dt.Rows[0]["KKTax"].ToString();
                    txtLPFSBRate.Text = dt.Rows[0]["SBTax"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }


        }
        private void GetApplicantName()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetApplicantName();
                if (dt.Rows.Count > 0)
                {
                    ddlCust.DataTextField = "CompanyName";
                    ddlCust.DataValueField = "CustId";
                    ddlCust.DataSource = dt;
                    ddlCust.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlCust.Items.Insert(0, oItm);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            CLoanScheme oLS = new CLoanScheme();
            dt = oLS.GetActiveLnSchemePG();
            ddlLnSchem.DataTextField = "LoanTypeName";
            ddlLnSchem.DataValueField = "LoanTypeId";
            ddlLnSchem.DataSource = dt;
            ddlLnSchem.DataBind();
            ListItem oItm = new ListItem();
            oItm.Text = "<--- Select --->";
            oItm.Value = "-1";
            ddlLnSchem.Items.Insert(0, oItm);
        }
        private void PopPurpose()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetLoanPurposeList();
                if (dt.Rows.Count > 0)
                {
                    ddlLnSctr.DataSource = dt;
                    ddlLnSctr.DataTextField = "PurposeName";
                    ddlLnSctr.DataValueField = "PurposeId";
                    ddlLnSctr.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLnSctr.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopApplication()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));

            try
            {
                // string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "SectorId", "SectorName", "SectorMst", 0, "AA", "AA", vLogDate, vBrCode);
                ddlLnSctr.DataSource = dt;
                ddlLnSctr.DataTextField = "SectorName";
                ddlLnSctr.DataValueField = "SectorId";
                ddlLnSctr.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLnSctr.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        private void PopSector()
        {
            //DataTable dt = null;
            //CGblIdGenerator oGbl = null;
            //try
            //{
            //    string vBrCode = Session[gblValue.BrnchCode].ToString();
            //    oGbl = new CGblIdGenerator();
            //    dt = oGbl.PopComboMIS("N", "N", "AA", "SectorId", "SectorName", "SectorMst", 0, "AA", "AA", System.DateTime.Now, "0000");
            //    ddlLnSctr.DataSource = dt;
            //    ddlLnSctr.DataTextField = "SectorName";
            //    ddlLnSctr.DataValueField = "SectorId";
            //    ddlLnSctr.DataBind();
            //    ListItem oLi = new ListItem("<--Select-->", "-1");
            //    ddlLnSctr.Items.Insert(0, oLi);
            //}
            //finally
            //{
            //    dt = null;
            //    oGbl = null;
            //}
        }
        protected void ddlLnSctr_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ddlLnPurps.SelectedIndex = -1;
            //PopPurpose(Convert.ToInt32(ddlLnSctr.SelectedValue));
        }
        protected void ddlLnSchem_SelectedIndexChanged(object sender, EventArgs e)
        {
            int loantype = Convert.ToInt32(ddlLnSchem.SelectedValue);
            CLoanScheme Obj = new CLoanScheme();
            DataTable dt = new DataTable();
            dt = Obj.GetProcFees(loantype);
            if (dt.Rows.Count > 0)
            {
                txtLPFPer.Text = dt.Rows[0]["ProcFeeAmt"].ToString();
                hfProcFees.Value = dt.Rows[0]["ProcFeeAmt"].ToString();
                txtRIntRate.Text = dt.Rows[0]["EffRedIntRate"].ToString();
            }
            else
            {
                txtLPFPer.Text = "0";
                hfProcFees.Value = "0";
                txtRIntRate.Text = "0";
            }
            CalculateProcFees();
            CalculateTotalCharge();
        }
        private void CalculateProcFees()
        {
            decimal LPFPer = 0;
            decimal SancAmt = 0;
            decimal LPFAmt = 0;
            if (txtLPFPer.Text != "")
                LPFPer = Convert.ToDecimal(txtLPFPer.Text);
            if (txtSancAmt.Text != "")
                SancAmt = Convert.ToDecimal(txtSancAmt.Text);
            else
                SancAmt = 0;
            LPFAmt = System.Math.Round((SancAmt * LPFPer) / 100, 2);
            txtLPF.Text = LPFAmt.ToString();
        }
        private void CalculateSTKKSBAmt()
        {
            decimal stax = 0;
            decimal kktax = 0;
            decimal sbtax = 0;
            decimal CGSTPer = 0;
            decimal SGSTPer = 0;
            decimal FLDGPer = 0;
            decimal LPFAmt = 0;
            decimal SancAmt = 0;
            if (txtSTaxPer.Text != "")
                stax = Convert.ToDecimal(txtSTaxPer.Text);
            if (txtLPFKKRate.Text != "")
                kktax = Convert.ToDecimal(txtLPFKKRate.Text);
            if (txtLPFSBRate.Text != "")
                sbtax = Convert.ToDecimal(txtLPFSBRate.Text);

            if (txtCGSTPer.Text != "")
                CGSTPer = Convert.ToDecimal(txtCGSTPer.Text);
            if (txtSGSTPer.Text != "")
                SGSTPer = Convert.ToDecimal(txtSGSTPer.Text);
            if (txtFLDGPer.Text != "")
                FLDGPer = Convert.ToDecimal(txtFLDGPer.Text);


            if (txtLPF.Text != "")
                LPFAmt = Convert.ToDecimal(txtLPF.Text);
            if (txtSancAmt.Text != "")
                SancAmt = Convert.ToDecimal(txtSancAmt.Text);
            //if (CGSTPer != 0 || SGSTPer != 0)
            //{
            //    stax = 0;
            //    kktax = 0;
            //    sbtax = 0;
            //}
            txtLPFST.Text = System.Math.Round(((LPFAmt * stax) / 100), 0).ToString();
            txtLPFKKTax.Text = System.Math.Round(((LPFAmt * kktax) / 100), 0).ToString();
            txtLPFSBTax.Text = System.Math.Round(((LPFAmt * sbtax) / 100), 0).ToString();


            // FLDG Calculation & GST Calculation
            txtFLDGAmt.Text = System.Math.Round(((SancAmt * FLDGPer) / 100), 0).ToString();
            txtCGSTAmt.Text = System.Math.Round(((LPFAmt * CGSTPer) / 100), 2).ToString();
            txtSGSTAmt.Text = System.Math.Round(((LPFAmt * SGSTPer) / 100), 2).ToString();
        }
        private string ChangeMMDDDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate = "";
            DateTime dDate = System.DateTime.Now;
            if (pDate == "")
                dDate = Convert.ToDateTime("01/01/1900");
            else
            {
                if (pDate.Length == 9)
                    pDate = pDate.Insert(0, "0");

                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);

            }
            return strDate;
        }
        protected void gvLoanSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            CApplication oCA = null;
            try
            {

                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCA = new CApplication();
                    ds1 = oCA.GetSanctionDtl(vSanId);
                    if (ds1.Tables.Count > 0)
                    {
                        dt = ds1.Tables[0];
                        dt1 = ds1.Tables[1];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvLoanSanc.Rows)
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
                        }
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;

                        PopBranch();
                        FillRate();
                        PopLoanType();
                        PopPurpose();
                        PopCustomerForSanc();

                        ddlPreLoanId.Items.Clear();
                        txtLoanSancNo.Text = dt.Rows[0]["SanctionID"].ToString();
                        hdSancId.Value = dt.Rows[0]["SanctionID"].ToString();
                        GetApplicantName();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        ddlCust.SelectedIndex = ddlCust.Items.IndexOf(ddlCust.Items.FindByValue(dt.Rows[0]["CustID"].ToString()));
                        PopLoanNo(dt.Rows[0]["CustID"].ToString());
                        ViewState["CustNo"] = dt.Rows[0]["CustID"].ToString();
                        ddlAppNo.SelectedIndex = ddlAppNo.Items.IndexOf(ddlAppNo.Items.FindByValue(dt.Rows[0]["LoanAppID"].ToString()));
                        POPLoanDetails(dt.Rows[0]["LoanAppID"].ToString());
                        txtLnSancDate.Text = dt.Rows[0]["SanctionDate"].ToString();
                        txtSancAmt.Text = dt.Rows[0]["SanctionAmt"].ToString();
                        if (dt.Rows[0]["LoanTypeId"].ToString() != "0")
                            ddlLnSchem.SelectedValue = dt.Rows[0]["LoanTypeId"].ToString();
                        txtFIntRate.Text = dt.Rows[0]["FIntRate"].ToString();
                        txtRIntRate.Text = dt.Rows[0]["RIntRate"].ToString();
                        txtInstNo.Text = dt.Rows[0]["NoOfInstallment"].ToString();
                        txtPeriod.Text = dt.Rows[0]["Tenure"].ToString();
                        if (dt.Rows[0]["IntType"].ToString() != "")
                            ddlInstType.SelectedValue = dt.Rows[0]["IntType"].ToString();
                        txtEMI.Text = dt.Rows[0]["EMIAmt"].ToString();
                        txtLPF.Text = dt.Rows[0]["LPFAmt"].ToString();
                        txtLPFST.Text = dt.Rows[0]["LPFSTAmt"].ToString();
                        txtLPFKKTax.Text = dt.Rows[0]["LPFKKTax"].ToString();
                        txtLPFSBTax.Text = dt.Rows[0]["LPFSBTax"].ToString();
                        txtLPFPer.Text = dt.Rows[0]["LPFPer"].ToString();
                        txtLPFKKRate.Text = dt.Rows[0]["LPFKKRate"].ToString();
                        txtLPFSBRate.Text = dt.Rows[0]["LPFSBRate"].ToString();
                        txtSTaxPer.Text = dt.Rows[0]["LPFSTRate"].ToString();
                        txtAppCharge.Text = dt.Rows[0]["ApplCharge"].ToString();
                        txtStampChrge.Text = dt.Rows[0]["StampCharge"].ToString();
                        txtTotCharge.Text = dt.Rows[0]["TotalCharge"].ToString();
                        txtInsuFee.Text = dt.Rows[0]["InsAmt"].ToString();
                        txtInsuCGSTAmt.Text = dt.Rows[0]["InsSTAmt"].ToString();
                        txtInsuSGSTAmt.Text = dt.Rows[0]["InsuKKTax"].ToString();
                        txtInsuIGSTAmt.Text = dt.Rows[0]["InsuSBTax"].ToString();
                        txtRepayStDt.Text = dt.Rows[0]["RepayStartDate"].ToString();
                        if (dt.Rows[0]["RepayType"].ToString() != "")
                            ddlRepayType.SelectedValue = dt.Rows[0]["RepayType"].ToString();
                        txtNetDisbAmt.Text = dt.Rows[0]["NetDisbAmt"].ToString();
                        txtApprovedBy.Text = dt.Rows[0]["FinalApprovedBy"].ToString();
                        txtApproveDt.Text = dt.Rows[0]["FinalApprovedDt"].ToString();
                        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                        ddlSancStatus.SelectedValue = dt.Rows[0]["SanctionStatus"].ToString();
                        if (dt.Rows[0]["AdvEMIRcvYN"].ToString() == "Y")
                        {
                            chkAdvEMI.Checked = true;
                        }
                        else
                            chkAdvEMI.Checked = false;
                        if (dt.Rows[0]["SecurityChk1"].ToString() == "Y")
                            ChkSecurityChk1.Checked = true;
                        else
                            ChkSecurityChk1.Checked = false;
                        if (dt.Rows[0]["SecurityChk2"].ToString() == "Y")
                            ChkSecurityChk2.Checked = true;
                        else
                            ChkSecurityChk2.Checked = false;
                        if (dt.Rows[0]["SecurityChk3"].ToString() == "Y")
                            ChkSecurityChk3.Checked = true;
                        else
                            ChkSecurityChk3.Checked = false;
                        txtDisbDate.Text = dt.Rows[0]["DisbDate"].ToString();
                        txtPreEMIInt.Text = dt.Rows[0]["PreEMIInt"].ToString();
                        hdPreLnAc.Value = dt.Rows[0]["PreLnAc"].ToString();
                        txtPreLnBal.Text = dt.Rows[0]["PreLnBal"].ToString();
                        //txtCGSTPer.Text = dt.Rows[0]["LPFCGSTRate"].ToString();
                        txtCGSTAmt.Text = dt.Rows[0]["LPFCGSTAmt"].ToString();
                        // txtSGSTPer.Text = dt.Rows[0]["LPFSGSTRate"].ToString();
                        txtSGSTAmt.Text = dt.Rows[0]["LPFSGSTAmt"].ToString();
                        txtIGSTAmt.Text = dt.Rows[0]["LPFIGSTAmt"].ToString();
                        txtFLDGPer.Text = dt.Rows[0]["FLDGRate"].ToString();
                        txtFLDGAmt.Text = dt.Rows[0]["FLDGAmt"].ToString();

                        txtBrkPrdIntAct.Text = dt.Rows[0]["BrkPrdIntAct"].ToString();
                        txtBrkPrdIntWave.Text = dt.Rows[0]["BrkPrdIntWave"].ToString();
                        txtBrkPrdInt.Text = dt.Rows[0]["BrkPrdInt"].ToString();


                        txtPropInsuAmt.Text = dt.Rows[0]["PropertyInsAmt"].ToString();
                        txtPropInsuCGST.Text = dt.Rows[0]["PropertyInsCGSTAmt"].ToString();
                        txtPropInsuSGST.Text = dt.Rows[0]["PropertyInsSGSTAmt"].ToString();
                        txtPropInsuIGST.Text = dt.Rows[0]["PropertyInsIGSTAmt"].ToString();
                        txtAdminFees.Text = dt.Rows[0]["AdminFees"].ToString();
                        txtTechFees.Text = dt.Rows[0]["TechFees"].ToString();


                        txtCERSAICharge.Text = dt.Rows[0]["CERSAICharge"].ToString();
                        txtCERSAIChargeCGST.Text = dt.Rows[0]["CERSAIChargeCGST"].ToString();
                        txtCERSAIChargeSGST.Text = dt.Rows[0]["CERSAIChargeSGST"].ToString();
                        txtCERSAIChargeIGST.Text = dt.Rows[0]["CERSAIChargeIGST"].ToString();


                        if (dt.Rows[0]["IGSTAppOnLPF"].ToString() == "Y")
                            chkIGSTPF.Checked = true;
                        else
                            chkIGSTPF.Checked = false;
                        if (dt.Rows[0]["IGSTAppOnInsu"].ToString() == "Y")
                            chkIGSTInsu.Checked = true;
                        else
                            chkIGSTInsu.Checked = false;
                        if (dt.Rows[0]["IGSTAppOnPropInsu"].ToString() == "Y")
                            chkIGSTPropInsu.Checked = true;
                        else
                            chkIGSTPropInsu.Checked = false;
                        if (dt.Rows[0]["IGSTAppOnCERSAICharge"].ToString() == "Y")
                            chkIGSTCERSAICharge.Checked = true;
                        else
                            chkIGSTCERSAICharge.Checked = false;

                        txtApprovedBy.Enabled = false;
                        txtApproveDt.Enabled = false;
                        //---------------------------------------------------------------
                        bool vViewPotenMem = false;
                        CRole oRl = new CRole();
                        DataTable dt2 = new DataTable();
                        dt2 = oRl.GetRoleById(Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                        if (dt2.Rows.Count > 0)
                        {
                            vViewPotenMem = Convert.ToString(dt.Rows[0]["ShowPotential"]) == "Y" && Convert.ToString(dt2.Rows[0]["PotenMemYN"]) == "Y" ? true : false;
                        }
                        btnShwPotenMem.Visible = vViewPotenMem;
                        btnUpdateUcic.Visible = vViewPotenMem;
                        hdnProtenUrl.Value = (Convert.ToString(dt.Rows[0]["PotenURL"]));
                        //---------------------------------------------------------------
                        tabLoanSanc.ActiveTabIndex = 1;
                        StatusButton("Show");

                    }
                    if (dt1.Rows.Count > 0)
                    {
                        ddlPreLoanId.DataTextField = "PreLoanId";
                        ddlPreLoanId.DataValueField = "PreLoanId";
                        ddlPreLoanId.DataSource = dt1;
                        ddlPreLoanId.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlPreLoanId.Items.Insert(0, oItm);
                        if (dt.Rows[0]["PreLnIdTopUp"].ToString() != "")
                        {
                            ddlPreLoanId.SelectedValue = dt.Rows[0]["PreLnIdTopUp"].ToString();
                        }
                    }
                    ddlIC.SelectedIndex = ddlIC.Items.IndexOf(ddlIC.Items.FindByValue(dt.Rows[0]["ICId"].ToString()));
                    if (rdbSel.SelectedValue == "N")
                    {
                        DataTable dt2 = new DataTable();
                        CApplication oLA = null;
                        oLA = new CApplication();
                        dt2 = oLA.GetInsuAmt(dt.Rows[0]["LoanAppID"].ToString(), Convert.ToInt32(dt.Rows[0]["ICId"].ToString()), Convert.ToDecimal(dt.Rows[0]["SanctionAmt"].ToString()), Convert.ToInt32(dt.Rows[0]["NoOfInstallment"].ToString()));
                        txtPropInsuAmt.Text = dt2.Rows[0]["Amt"].ToString();
                        txtPropInsuCGST.Text = dt2.Rows[0]["CGST"].ToString();
                        txtPropInsuSGST.Text = dt2.Rows[0]["CGST"].ToString();
                    }
                    CalculateProcFees();
                    CalculateSTKKSBAmt();
                    CalculateTotalCharge();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds1 = null;
                dt1 = null;
                dt = null;
                oCA = null;
            }
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vCustId = ddlCust.SelectedItem.Value;
            PopLoanNo(vCustId);
        }
        protected void ddlPreLoanId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (txtPreLnBal.Text == "0.00")
            //{
            string vPreLnId = ddlPreLoanId.SelectedValue.ToString();
            DateTime vSancDate = gblFuction.setDate(txtLnSancDate.Text);
            CApplication OCA = new CApplication();
            DataTable dt = new DataTable();
            dt = OCA.TotDueOnSancdate(vPreLnId, vSancDate);
            if (dt.Rows.Count > 0)
            {
                txtPreLnBal.Text = dt.Rows[0]["TotPrinDue"].ToString();
                hdPreLnAc.Value = dt.Rows[0]["PreLnAc"].ToString();
                CalculateTotalCharge();
            }
            else
            {
                txtPreLnBal.Text = "0.00";
                CalculateTotalCharge();
            }
            // }
        }
        protected void ddlAppNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vLoanAppId = ddlAppNo.SelectedValue.ToString();
            POPLoanDetails(vLoanAppId);
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                tabLoanSanc.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    lblMsg.Text = gblPRATAM.DeleteMsg;
                    LoadGrid(1);
                    StatusButton("Delete");
                }
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
            tabLoanSanc.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                LoadGrid(1);
                tabLoanSanc.ActiveTabIndex = 0;
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControl();
            }
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oCG = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oCG = new CApplication();
                dt = oCG.GetSanctionList(vMode, vBrCode, vFrmDt, vToDt, pPgIndx, ref vRows);
                if (dt.Rows.Count > 0)
                {
                    gvLoanSanc.DataSource = dt;
                    gvLoanSanc.DataBind();
                }
                else
                {
                    gvLoanSanc.DataSource = null;
                    gvLoanSanc.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            string vNewId = "", vLnSancId = "", vLnAppId = "", vIntType = "", vRepayType = "", vIsDisbursed = "", vCustId = "";
            string vAdvEMIRcvYN = "", vSanctionStatus = "", vFinalApprovedBy = "", vRemarks = "", vBranchCode = "", vSecCheck1 = "",
                vSecCheck2 = "", vSecCheck3 = "", vPreLnIdTopUp = "", vPreLnAc = "";
            decimal vSancAmt = 0, vFIntRate = 0, vRIntRate = 0, vEMIAmt = 0, vLPFAmt = 0, vLPFSTAmt = 0, vInsAmt = 0,
                vInsuCGSTAmt = 0, vInsuSGSTAmt = 0, vInsuIGSTAmt = 0,
                vLPFKKTax = 0, vLPFSBTax = 0, vInsuSBTax = 0,
                vLPFPer = 0, vLPFSTxrate = 0, vLPFKKRate = 0, vLPFSBRate = 0, vAppChrge = 0,
                vStampChrge = 0, vTotChrge = 0, vPreEMIInt = 0, vPreLnBal = 0, vCGSTPer = 0, vCGSTAmt = 0,
                vSGSTPer = 0, vSGSTAmt = 0, vIGSTAmt = 0, vFLDGPer = 0, vFLDGAmt = 0, vBrkPdIntAct = 0, vBrkPrdIntWaive = 0, vBrkPrdInt = 0,
                vCERSAIAmt = 0, vCERSAIAmtCGST = 0, vCERSAIAmtSGST = 0, vCERSAIAmtIGST = 0;
            int vCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            decimal vNetDisbAmt = 0;
            Int32 vErr = 0, vLnTypeId = 0, vNoOfInstal = 0, vTenure = 0;
            DateTime vSancDate = gblFuction.setDate(txtLnSancDate.Text);
            DateTime vFinalSancDate = gblFuction.setDate(txtApproveDt.Text);
            if (txtDisbDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Input Expected Disburse Date..");
                return false;
            }
            //if (txtRepayStDt.Text == "")
            //{
            //    gblFuction.AjxMsgPopup("Please Input Repay Start Date..");
            //    return false;
            //}
            if (txtRIntRate.Text == "")
            {
                gblFuction.AjxMsgPopup("Reducing Interest rate can not be blank....");
                return false;
            }
            if (txtInstNo.Text == "")
            {
                gblFuction.AjxMsgPopup("No of Installment can not be blank....");
                return false;
            }
            //if (Convert.ToDouble(txtLPFPer.Text == "" ? "0" : txtLPFPer.Text) == 0)
            //{
            //    gblFuction.AjxMsgPopup("Loan Processing fee can not be Zero....");
            //    return false;
            //}

            DateTime vDisbDate = gblFuction.setDate(txtDisbDate.Text);
            DateTime vRepayStartDate = gblFuction.setDate(txtRepayStDt.Text);
            DateTime vFinalApprvDate = gblFuction.setDate(txtApproveDt.Text);
            CApplication oCA = new CApplication();
            try
            {

                if (hdSancId.Value != "")
                {
                    vLnSancId = hdSancId.Value.ToString();
                }
                if (hdPreLnAc.Value != "")
                {
                    vPreLnAc = hdPreLnAc.Value.ToString();
                }
                vLnAppId = (Request[ddlAppNo.UniqueID] as string == null) ? ddlAppNo.SelectedValue : Request[ddlAppNo.UniqueID] as string;
                vCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
                vSancAmt = Convert.ToDecimal(txtSancAmt.Text);
                vLnTypeId = Convert.ToInt32(ddlLnSchem.SelectedValue);
                vFIntRate = Convert.ToDecimal(txtFIntRate.Text);

                vRIntRate = Convert.ToDecimal(txtRIntRate.Text);
                vNoOfInstal = Convert.ToInt32(txtInstNo.Text);
                vTenure = Convert.ToInt32(txtPeriod.Text);
                if (ddlPreLoanId.SelectedValue != "-1")
                    vPreLnIdTopUp = (Request[ddlPreLoanId.UniqueID] as string == null) ? ddlPreLoanId.SelectedValue : Request[ddlPreLoanId.UniqueID] as string;
                vIntType = (Request[ddlInstType.UniqueID] as string == null) ? ddlInstType.SelectedValue : Request[ddlInstType.UniqueID] as string;
                vRepayType = (Request[ddlRepayType.UniqueID] as string == null) ? ddlRepayType.SelectedValue : Request[ddlRepayType.UniqueID] as string;

                if (vNoOfInstal != vTenure)
                {
                    gblFuction.AjxMsgPopup("No of Installment and Tenure Should Be Equal");
                    return false;
                }
                vEMIAmt = Convert.ToDecimal(txtEMI.Text);
                vLPFAmt = Convert.ToDecimal(txtLPF.Text);
                vLPFSTAmt = Convert.ToDecimal(txtLPFST.Text);
                vInsAmt = Convert.ToDecimal(txtInsuFee.Text);
                vInsuCGSTAmt = Convert.ToDecimal(txtInsuCGSTAmt.Text);
                vInsuSGSTAmt = Convert.ToDecimal(txtInsuSGSTAmt.Text);
                vInsuIGSTAmt = Convert.ToDecimal(txtInsuIGSTAmt.Text);
                //vOthSTAmt = Convert.ToDecimal(txtSTOther.Text);
                //  vServiceTaxRate = Convert.ToDecimal(txtSTaxAmt.Text);

                vLPFKKTax = Convert.ToDecimal(txtLPFKKTax.Text);
                vLPFSBTax = Convert.ToDecimal(txtLPFSBTax.Text);

                vInsuSBTax = Convert.ToDecimal(txtInsuSBTax.Text);

                vLPFPer = Convert.ToDecimal(txtLPFPer.Text);
                vLPFKKRate = Convert.ToDecimal(txtLPFKKRate.Text);
                vLPFSBRate = Convert.ToDecimal(txtLPFSBRate.Text);
                vLPFSTxrate = Convert.ToDecimal(txtSTaxPer.Text);
                vAppChrge = Convert.ToDecimal(txtAppCharge.Text);
                vStampChrge = Convert.ToDecimal(txtStampChrge.Text);
                vTotChrge = Convert.ToDecimal(txtTotCharge.Text);

                vPreEMIInt = Convert.ToDecimal(txtPreEMIInt.Text);
                vPreLnBal = Convert.ToDecimal(txtPreLnBal.Text);

                if (txtCGSTPer.Text != "")
                    vCGSTPer = Convert.ToDecimal(txtCGSTPer.Text);
                if (txtCGSTAmt.Text != "")
                    vCGSTAmt = Convert.ToDecimal(txtCGSTAmt.Text);
                if (txtSGSTPer.Text != "")
                    vSGSTPer = Convert.ToDecimal(txtCGSTPer.Text);
                if (txtSGSTAmt.Text != "")
                    vSGSTAmt = Convert.ToDecimal(txtCGSTAmt.Text);
                if (txtFLDGPer.Text != "")
                    vFLDGPer = Convert.ToDecimal(txtFLDGPer.Text);
                if (txtFLDGAmt.Text != "")
                    vFLDGAmt = Convert.ToDecimal(txtFLDGAmt.Text);
                if (txtIGSTAmt.Text != "")
                    vIGSTAmt = Convert.ToDecimal(txtIGSTAmt.Text);
                //vIGSTAmt,vInsuIGSTAmt,vIGSTAppLPF,vIGSTAppInsu,vIGSTAppPropInsu,pPropInsuIGST
                if (txtBrkPrdIntAct.Text != "")
                    vBrkPdIntAct = Convert.ToDecimal(txtBrkPrdIntAct.Text);
                if (txtBrkPrdIntWave.Text != "")
                    vBrkPrdIntWaive = Convert.ToDecimal(txtBrkPrdIntWave.Text);
                if (txtBrkPrdInt.Text != "")
                    vBrkPrdInt = Convert.ToDecimal(txtBrkPrdInt.Text);

                if (vPreLnBal > 0)
                {
                    if (vPreLnIdTopUp == "")
                    {
                        gblFuction.AjxMsgPopup("Please Select Prevoius Loan No In case of Top Up Loan");
                        return false;
                    }
                    if (vPreLnAc == "")
                    {
                        gblFuction.AjxMsgPopup("Prevoius Loan Account Can Not Be Empty In case of Top Up Loan");
                        return false;
                    }
                }

                vIsDisbursed = "N";
                vNetDisbAmt = Convert.ToDecimal(txtNetDisbAmt.Text);
                if (chkAdvEMI.Checked == true)
                {
                    vAdvEMIRcvYN = "Y";
                }
                else
                {
                    vAdvEMIRcvYN = "N";
                }

                if (ChkSecurityChk1.Checked == true)
                {
                    vSecCheck1 = "Y";
                }
                else
                {
                    vSecCheck1 = "N";
                }
                if (ChkSecurityChk2.Checked == true)
                {
                    vSecCheck2 = "Y";
                }
                else
                {
                    vSecCheck2 = "N";
                }
                if (ChkSecurityChk3.Checked == true)
                {
                    vSecCheck3 = "Y";
                }
                else
                {
                    vSecCheck3 = "N";
                }
                string vIGSTAppLPF = "N", vIGSTAppInsu = "N", vIGSTAppPropInsu = "N", vIGSTAppCERSAIAmt = "N";
                if (chkIGSTPF.Checked == true)
                    vIGSTAppLPF = "Y";
                if (chkIGSTInsu.Checked == true)
                    vIGSTAppInsu = "Y";
                if (chkIGSTPropInsu.Checked == true)
                    vIGSTAppPropInsu = "Y";
                if (chkIGSTCERSAICharge.Checked == true)
                    vIGSTAppCERSAIAmt = "Y";

                //vIGSTAppCERSAIAmt,vCERSAIAmt, vCERSAIAmtCGST, vCERSAIAmtSGST, vCERSAIAmtIGST
                if (txtCERSAICharge.Text != "")
                    vCERSAIAmt = Convert.ToDecimal(txtCERSAICharge.Text);
                if (txtCERSAIChargeCGST.Text != "")
                    vCERSAIAmtCGST = Convert.ToDecimal(txtCERSAIChargeCGST.Text);
                if (txtCERSAIChargeSGST.Text != "")
                    vCERSAIAmtSGST = Convert.ToDecimal(txtCERSAIChargeSGST.Text);
                if (txtCERSAIChargeIGST.Text != "")
                    vCERSAIAmtIGST = Convert.ToDecimal(txtCERSAIChargeIGST.Text);

                vSanctionStatus = (Request[ddlSancStatus.UniqueID] as string == null) ? ddlSancStatus.SelectedValue : Request[ddlSancStatus.UniqueID] as string;
                vFinalApprovedBy = (Request[txtApprovedBy.UniqueID] as string == null) ? txtApprovedBy.Text : Request[txtApprovedBy.UniqueID] as string;
                vRemarks = (Request[txtRemarks.UniqueID] as string == null) ? txtRemarks.Text : Request[txtRemarks.UniqueID] as string;
                vBranchCode = Session[gblValue.BrnchCode].ToString();

                if (vRIntRate == 0)
                {
                    gblFuction.AjxMsgPopup("Reducing Interest rate can not be zero....");
                    return false;
                }
                if (vRepayType == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Repayment Type...");
                    return false;
                }
                if (vIntType == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Interest Type...");
                    return false;
                }

                decimal pPropInsuAmt = 0, pPropInsuCGST = 0, pPropInsuSGST = 0, pPropInsuIGST = 0, pAdminFees = 0, pTechFees = 0;
                if (txtPropInsuAmt.Text != "")
                    pPropInsuAmt = Convert.ToDecimal(txtPropInsuAmt.Text);
                if (txtPropInsuCGST.Text != "")
                    pPropInsuCGST = Convert.ToDecimal(txtPropInsuCGST.Text);
                if (txtPropInsuSGST.Text != "")
                    pPropInsuSGST = Convert.ToDecimal(txtPropInsuSGST.Text);
                if (txtPropInsuIGST.Text != "")
                    pPropInsuIGST = Convert.ToDecimal(txtPropInsuIGST.Text);
                if (txtAdminFees.Text != "")
                    pAdminFees = Convert.ToDecimal(txtAdminFees.Text);
                if (txtTechFees.Text != "")
                    pTechFees = Convert.ToDecimal(txtTechFees.Text);

                if (Mode == "Save")
                {
                    vErr = oCA.SaveSanction(ref vNewId, vLnSancId, vLnAppId, vCustId, vSancDate, vSancAmt, vLnTypeId, vFIntRate, vRIntRate, vNoOfInstal, vTenure,
                    vIntType, vRepayType, vEMIAmt, vLPFAmt, vLPFSTAmt, vInsAmt, vInsuCGSTAmt, vFinalSancDate, vRepayStartDate, vIsDisbursed,
                    vNetDisbAmt, vAdvEMIRcvYN, vSanctionStatus, vFinalApprvDate, vFinalApprovedBy, vRemarks, vBranchCode, vLPFKKTax, vLPFSBTax, vInsuSGSTAmt,
                    vInsuSBTax, vLPFPer, vLPFSTxrate, vLPFKKRate, vLPFSBRate, vAppChrge, vStampChrge, vTotChrge, vCreatedBy, "Save", 0, vDisbDate, vPreEMIInt,
                    vPreLnBal, vSecCheck1, vSecCheck2, vSecCheck3, vCGSTPer, vCGSTAmt, vSGSTPer, vSGSTAmt, vFLDGPer, vFLDGAmt, vPreLnIdTopUp, vPreLnAc,
                    vBrkPdIntAct, vBrkPrdIntWaive, vBrkPrdInt,
                    pPropInsuAmt, pPropInsuCGST, pPropInsuSGST, pAdminFees, pTechFees,
                    vIGSTAmt, vInsuIGSTAmt, vIGSTAppLPF, vIGSTAppInsu, vIGSTAppPropInsu, pPropInsuIGST,
                    vIGSTAppCERSAIAmt, vCERSAIAmt, vCERSAIAmtCGST, vCERSAIAmtSGST, vCERSAIAmtIGST, Convert.ToInt32(ddlIC.SelectedValue));
                    if (vErr == 0)
                    {
                        try
                        {
                            JocataRequest(vCustId, vNewId, vCreatedBy);
                        }
                        finally
                        {
                        }
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    // Check For Loan Disbursement....
                    DataTable dt = oCA.CheckDisbBySancId(vLnSancId);
                    if (Convert.ToInt32(dt.Rows[0]["CountSancData"]) > 0)
                    {
                        gblFuction.MsgPopup("This Loan has alredy been disburshed...Sanction Details can not be updated...");
                        return false;
                    }
                    vErr = oCA.SaveSanction(ref vNewId, vLnSancId, vLnAppId, vCustId, vSancDate, vSancAmt, vLnTypeId, vFIntRate, vRIntRate, vNoOfInstal, vTenure,
                    vIntType, vRepayType, vEMIAmt, vLPFAmt, vLPFSTAmt, vInsAmt, vInsuCGSTAmt, vFinalSancDate, vRepayStartDate, vIsDisbursed,
                    vNetDisbAmt, vAdvEMIRcvYN, vSanctionStatus, vFinalApprvDate, vFinalApprovedBy, vRemarks, vBranchCode, vLPFKKTax, vLPFSBTax, vInsuSGSTAmt,
                    vInsuSBTax, vLPFPer, vLPFSTxrate, vLPFKKRate, vLPFSBRate, vAppChrge, vStampChrge, vTotChrge, vCreatedBy, "Edit", 0, vDisbDate, vPreEMIInt,
                    vPreLnBal, vSecCheck1, vSecCheck2, vSecCheck3, vCGSTPer, vCGSTAmt, vSGSTPer, vSGSTAmt, vFLDGPer, vFLDGAmt, vPreLnIdTopUp, vPreLnAc,
                    vBrkPdIntAct, vBrkPrdIntWaive, vBrkPrdInt,
                    pPropInsuAmt, pPropInsuCGST, pPropInsuSGST, pAdminFees, pTechFees,
                    vIGSTAmt, vInsuIGSTAmt, vIGSTAppLPF, vIGSTAppInsu, vIGSTAppPropInsu, pPropInsuIGST,
                    vIGSTAppCERSAIAmt, vCERSAIAmt, vCERSAIAmtCGST, vCERSAIAmtSGST, vCERSAIAmtIGST, Convert.ToInt32(ddlIC.SelectedValue));
                    if (vErr == 0)
                    {
                        try
                        {
                            JocataRequest(vCustId, vLnSancId, vCreatedBy);
                        }
                        finally
                        {
                        }
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
                    DataTable dt = oCA.CheckDisbBySancId(vLnSancId);
                    if (Convert.ToInt32(dt.Rows[0]["CountSancData"]) > 0)
                    {
                        gblFuction.MsgPopup("This Loan has alredy been disburshed...Sanction Details can not be Deleted...");
                        return false;
                    }
                    vErr = oCA.SaveSanction(ref vNewId, vLnSancId, vLnAppId, vCustId, vSancDate, vSancAmt, vLnTypeId, vFIntRate, vRIntRate, vNoOfInstal, vTenure,
                    vIntType, vRepayType, vEMIAmt, vLPFAmt, vLPFSTAmt, vInsAmt, vInsuCGSTAmt, vFinalSancDate, vRepayStartDate, vIsDisbursed,
                    vNetDisbAmt, vAdvEMIRcvYN, vSanctionStatus, vFinalApprvDate, vFinalApprovedBy, vRemarks, vBranchCode, vLPFKKTax, vLPFSBTax, vInsuSGSTAmt,
                    vInsuSBTax, vLPFPer, vLPFSTxrate, vLPFKKRate, vLPFSBRate, vAppChrge, vStampChrge, vTotChrge, vCreatedBy, "Delete", 0, vDisbDate, vPreEMIInt,
                    vPreLnBal, vSecCheck1, vSecCheck2, vSecCheck3, vCGSTPer, vCGSTAmt, vSGSTPer, vSGSTAmt, vFLDGPer, vFLDGAmt, vPreLnIdTopUp, vPreLnAc,
                    vBrkPdIntAct, vBrkPrdIntWaive, vBrkPrdInt,
                    pPropInsuAmt, pPropInsuCGST, pPropInsuSGST, pAdminFees, pTechFees,
                    vIGSTAmt, vInsuIGSTAmt, vIGSTAppLPF, vIGSTAppInsu, vIGSTAppPropInsu, pPropInsuIGST,
                    vIGSTAppCERSAIAmt, vCERSAIAmt, vCERSAIAmtCGST, vCERSAIAmtSGST, vCERSAIAmtIGST, Convert.ToInt32(ddlIC.SelectedValue));
                    if (vErr == 0)
                        vResult = true;
                    else if (@vErr == 2)
                    {
                        gblFuction.MsgPopup("Final Sanction has already been done for this Sanction No... You can Not Delete this record ..");
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
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
                oCA = null;
            }
        }
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            //DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            //DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            //DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //CApplication oCG = oCG = new CApplication();
            //DateTime vAppDate = vLoginDt;

            //if (txtAppDt.Text.Trim() == "")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Loan Application Date cannot be empty.");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //    vResult = false;
            //}
            //else
            //{
            //    vAppDate = gblFuction.setDate(txtAppDt.Text);
            //    if (vAppDate < vFinFrom || vAppDate > vFinTo)
            //    {
            //        EnableControl(true);
            //        gblFuction.MsgPopup("Loan Application Date should be Financial Year.");
            //        gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //        vResult = false;
            //    }
            //    if (vAppDate > vLoginDt)
            //    {
            //        EnableControl(true);
            //        gblFuction.MsgPopup("Loan Application Date should not be greater than login date.");
            //        gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //        vResult = false;
            //    }
            //}
            //if (txtDisbDt.Text.Trim() == "")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Expected Disb. Date cannot be empty.");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //    vResult = false;
            //}
            //DateTime vExpDisbDate = gblFuction.setDate(txtDisbDt.Text);
            //if (vExpDisbDate < vAppDate)
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Expected Disb. Date can not less than Loan Application Date.");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
            //    vResult = false;
            //}

            //if (Request[ddlMemNo.UniqueID] as string == "-1")
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Please select the member...");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlMemNo");
            //    vResult = false;
            //}
            //if (ddlLnSchem.SelectedIndex <= 0)
            //{
            //    EnableControl(true);
            //    gblFuction.MsgPopup("Please select the Loan Scheme...");
            //    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlLnSchem");
            //    vResult = false;
            //}


            return vResult;
        }
        private void ClearControl()
        {
            // ddlCust.SelectedIndex = -1;
            ddlSancStatus.SelectedIndex = -1;
            ddlRepayType.SelectedIndex = -1;
            txtAppDt.Text = "";
            txtLnAmt.Text = "0";
            ddlAppNo.SelectedIndex = -1;
            txtLnSancDate.Text = "";
            txtSancAmt.Text = "0";
            ddlLnSctr.SelectedIndex = -1;
            ddlLnSchem.SelectedIndex = -1;
            txtFIntRate.Text = "0";
            txtInstNo.Text = "0";
            txtPeriod.Text = "0";
            ddlInstType.SelectedIndex = -1;
            txtEMI.Text = "0";
            txtLPF.Text = "0";
            txtLPFST.Text = "0";
            txtInsuFee.Text = "0";
            txtInsuCGSTAmt.Text = "0";
            txtInsuSGSTAmt.Text = "0";
            txtInsuIGSTAmt.Text = "0";
            txtRepayStDt.Text = "0";
            ddlRepayType.SelectedIndex = -1;
            txtNetDisbAmt.Text = "0";
            chkAdvEMI.Checked = false;
            txtApprovedBy.Text = "";
            txtApproveDt.Text = "";
            txtRemarks.Text = "";
            txtRIntRate.Text = "0";
            txtLPFKKTax.Text = "0";
            txtLPFSBTax.Text = "0";
            txtCGSTAmt.Text = "0";
            txtSGSTAmt.Text = "0";
            txtIGSTAmt.Text = "0";


            txtPropInsuAmt.Text = "0";
            txtPropInsuCGST.Text = "0";
            txtPropInsuSGST.Text = "0";
            txtPropInsuIGST.Text = "0";
            txtAdminFees.Text = "0";
            txtTechFees.Text = "0";
            txtInsuSBTax.Text = "";
            txtLPFKKRate.Text = "";
            txtLPFSBRate.Text = "";
            chkIGSTPF.Checked = false;
            chkIGSTInsu.Checked = false;
            chkIGSTPropInsu.Checked = false;
        }
        private void POPLoanDetails(string pLoanAppId)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = new DataTable();
            CApplication oLA = null;
            oLA = new CApplication();
            dt = oLA.GetLoanAppDtlLoanId(pLoanAppId);
            if (dt.Rows.Count > 0)
            {
                txtAppDt.Text = dt.Rows[0]["ApplicationDt"].ToString();
                txtLnAmt.Text = dt.Rows[0]["AppAmount"].ToString();
                ddlLnSctr.SelectedIndex = ddlLnSctr.Items.IndexOf(ddlLnSctr.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                // ddlLnSctr.SelectedValue = dt.Rows[0]["PurposeID"].ToString();
            }
        }
        private void PopLoanNo(string pCustId)
        {
            DataTable dt = null;
            CMember oMem = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            oMem = new CMember();
            try
            {
                dt = oMem.GetApplicantListByLoanid(pCustId);
                if (dt.Rows.Count > 0)
                {
                    ddlAppNo.DataTextField = "LoanAppId";
                    ddlAppNo.DataValueField = "LoanAppId";
                    ddlAppNo.DataSource = dt;
                    ddlAppNo.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlAppNo.Items.Insert(0, oItm);

                }
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
        private void CalculateEMI()
        {
            decimal pLnAmt = 0, pRIntRate = 0, pInstNo = 0, EMI = 0;
            string pRepayType = "";
            Int32 pLoanTypeId = 0;
            if (txtSancAmt.Text == "")
                pLnAmt = 0;
            else
                pLnAmt = Convert.ToDecimal(txtSancAmt.Text);
            if (txtRIntRate.Text == "")
                pRIntRate = 0;
            else
                pRIntRate = Convert.ToDecimal(txtRIntRate.Text);
            if (txtInstNo.Text == "")
                pInstNo = 0;
            else
                pInstNo = Convert.ToDecimal(txtInstNo.Text);
            if (ddlLnSchem.SelectedIndex != -1)
            {
                pLoanTypeId = Convert.ToInt32(ddlLnSchem.SelectedValue);
            }
            if (ddlRepayType.SelectedIndex != -1)
            {
                pRepayType = Convert.ToString(ddlRepayType.SelectedValue);
            }
            DataTable dt = new DataTable();
            CApplication OCA = new CApplication();
            if (pRIntRate != 0 && pInstNo != 0)
            {
                dt = OCA.CalculateEMI(pLnAmt, pRIntRate, pInstNo, pLoanTypeId, pRepayType);
                if (dt.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(dt.Rows[0]["CalculatedEMI"].ToString()) == false)
                        EMI = Convert.ToDecimal(dt.Rows[0]["CalculatedEMI"]);
                }
            }
            txtEMI.Text = EMI.ToString();
        }
        protected void txtInstNo_TextChanged(object sender, EventArgs e)
        {
            //GetIRRFromFlatInterest();
            CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtFIntRate_TextChanged(object sender, EventArgs e)
        {
            GetIRRFromFlatInterest();
            CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtRIntRate_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtSancAmt_TextChanged(object sender, EventArgs e)
        {
            GetIRRFromFlatInterest();
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtSTaxPer_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtLPFKKRate_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtLPFSBRate_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtLPFPer_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtAppCharge_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtStampChrge_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtInsuFee_TextChanged(object sender, EventArgs e)
        {
            CalculateInsuGST();
            CalculateTotalCharge();
        }
        private void CalculateInsuGST()
        {
            decimal pInsuAmt = 0, pInsuCGST = 0, pInsuSGST = 0, pInsuIGST = 0;
            if (txtInsuFee.Text != "")
                pInsuAmt = Convert.ToDecimal(txtInsuFee.Text);
            if (chkIGSTInsu.Checked == true)
            {
                pInsuCGST = 0;
                pInsuSGST = 0;
                pInsuIGST = ((pInsuAmt * 18) / 100);
            }
            else
            {
                pInsuCGST = ((pInsuAmt * 9) / 100);
                pInsuSGST = ((pInsuAmt * 9) / 100);
                pInsuIGST = 0;
            }
            txtInsuCGSTAmt.Text = Math.Round(pInsuCGST, 0).ToString();
            txtInsuSGSTAmt.Text = Math.Round(pInsuSGST, 0).ToString();
            txtInsuIGSTAmt.Text = Math.Round(pInsuIGST, 0).ToString();
            CalculateTotalCharge();
        }
        private void CalculateCERSAIGST()
        {
            decimal pCERSAIAmt = 0, pCERSAICGST = 0, pCERSAISGST = 0, pCERSAIIGST = 0;
            if (txtCERSAICharge.Text != "")
                pCERSAIAmt = Convert.ToDecimal(txtCERSAICharge.Text);
            if (chkIGSTCERSAICharge.Checked == true)
            {
                pCERSAICGST = 0;
                pCERSAISGST = 0;
                pCERSAIIGST = ((pCERSAIAmt * 18) / 100);
            }
            else
            {
                pCERSAICGST = ((pCERSAIAmt * 9) / 100);
                pCERSAISGST = ((pCERSAIAmt * 9) / 100);
                pCERSAIIGST = 0;
            }
            txtCERSAIChargeCGST.Text = Math.Round(pCERSAICGST, 0).ToString();
            txtCERSAIChargeSGST.Text = Math.Round(pCERSAISGST, 0).ToString();
            txtCERSAIChargeIGST.Text = Math.Round(pCERSAIIGST, 0).ToString();
            CalculateTotalCharge();
        }
        private void CalculatePropertyInsuGST()
        {
            decimal pPropInsuAmt = 0, pPropInsuCGST = 0, pPropInsuSGST = 0, pPropInsuIGST = 0;
            if (txtPropInsuAmt.Text != "")
                pPropInsuAmt = Convert.ToDecimal(txtPropInsuAmt.Text);
            if (chkIGSTPropInsu.Checked == true)
            {
                pPropInsuCGST = 0;
                pPropInsuSGST = 0;
                pPropInsuIGST = ((pPropInsuAmt * 18) / 100);
            }
            else
            {
                pPropInsuCGST = ((pPropInsuAmt * 9) / 100);
                pPropInsuSGST = ((pPropInsuAmt * 9) / 100);
                pPropInsuIGST = 0;
            }
            txtPropInsuCGST.Text = Math.Round(pPropInsuCGST, 0).ToString();
            txtPropInsuSGST.Text = Math.Round(pPropInsuSGST, 0).ToString();
            txtPropInsuIGST.Text = Math.Round(pPropInsuIGST, 0).ToString();
            CalculateTotalCharge();
        }
        protected void txtInsuCGSTAmt_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtInsuSGSTAmt_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtInsuIGSTAmt_TextChanged(object sender, EventArgs e)
        {
            //CalculateEMI();
            //CalculateProcFees();
            //CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtInsuSBTax_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        //protected void txtPreLnBal_TextChanged(object sender, EventArgs e)
        //{
        //    CalculateTotalCharge();
        //}
        protected void txtCGSTPer_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtSGSTPer_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtFLDGPer_TextChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        private void CalculateTotalCharge()
        {
            decimal AppCharge = 0;
            decimal StmpChrge = 0;
            decimal InsuFees = 0;
            decimal InsuCGSTAmt = 0;
            decimal InsuSGSTAmt = 0;
            decimal InsuIGSTAmt = 0;
            decimal InsuSBTax = 0;
            decimal ProcFees = 0;
            decimal LPFSTax = 0;
            decimal LPFKKtax = 0;
            decimal LPFSBtax = 0;
            decimal EMI = 0;
            decimal PEMIInt = 0;
            decimal PreLnBal = 0;
            decimal CGSTAmt = 0;
            decimal SGSTAmt = 0;
            decimal IGSTAmt = 0;
            decimal FLDGAmt = 0;
            decimal GrandTotal = 0;
            decimal TotalCharge = 0;
            decimal SanAmt = 0;
            decimal NetDisbAmt = 0;
            decimal BrkPrdInt = 0;
            decimal BrkPrdIntWaive = 0;
            decimal NetBrkPrdInt = 0;

            decimal pPropInsuAmt = 0, pPropInsuCGST = 0, pPropInsuSGST = 0, pPropInsuIGST = 0, pAdminFees = 0, pTechFees = 0;
            decimal pCERSAIAmt = 0, pCERSAICGST = 0, pCERSAISGST = 0, pCERSAIIGST = 0;

            if (txtSancAmt.Text != "")
                SanAmt = Convert.ToDecimal(txtSancAmt.Text);
            if (txtAppCharge.Text != "")
                AppCharge = Convert.ToDecimal(txtAppCharge.Text);
            if (txtStampChrge.Text != "")
                StmpChrge = Convert.ToDecimal(txtStampChrge.Text);
            if (txtInsuFee.Text != "")
                InsuFees = Convert.ToDecimal(txtInsuFee.Text);
            if (txtInsuCGSTAmt.Text != "")
                InsuCGSTAmt = Convert.ToDecimal(txtInsuCGSTAmt.Text);
            if (txtInsuSGSTAmt.Text != "")
                InsuSGSTAmt = Convert.ToDecimal(txtInsuSGSTAmt.Text);
            if (txtInsuSGSTAmt.Text != "")
                InsuIGSTAmt = Convert.ToDecimal(txtInsuIGSTAmt.Text);


            if (txtInsuSBTax.Text != "")
                InsuSBTax = Convert.ToDecimal(txtInsuSBTax.Text);
            if (txtLPF.Text != "")
                ProcFees = Convert.ToDecimal(txtLPF.Text);
            if (txtLPFST.Text != "")
                LPFSTax = Convert.ToDecimal(txtLPFST.Text);
            if (txtLPFKKTax.Text != "")
                LPFKKtax = Convert.ToDecimal(txtLPFKKTax.Text);
            if (txtLPFSBTax.Text != "")
                LPFSBtax = Convert.ToDecimal(txtLPFSBTax.Text);
            if (txtEMI.Text != "")
                EMI = Convert.ToDecimal(txtEMI.Text);
            if (txtPreEMIInt.Text != "")
                PEMIInt = Convert.ToDecimal(txtPreEMIInt.Text);
            if (txtPreLnBal.Text != "")
                PreLnBal = Convert.ToDecimal(txtPreLnBal.Text);

            if (txtCGSTAmt.Text != "")
                CGSTAmt = Convert.ToDecimal(txtCGSTAmt.Text);
            if (txtSGSTAmt.Text != "")
                SGSTAmt = Convert.ToDecimal(txtSGSTAmt.Text);
            if (txtIGSTAmt.Text != "")
                IGSTAmt = Convert.ToDecimal(txtIGSTAmt.Text);

            if (txtFLDGAmt.Text != "")
                FLDGAmt = Convert.ToDecimal(txtFLDGAmt.Text);

            if (txtBrkPrdIntAct.Text != "")
                BrkPrdInt = Convert.ToDecimal(txtBrkPrdIntAct.Text);
            if (txtBrkPrdIntWave.Text != "")
                BrkPrdIntWaive = Convert.ToDecimal(txtBrkPrdIntWave.Text);
            NetBrkPrdInt = (BrkPrdInt - BrkPrdIntWaive);
            txtBrkPrdInt.Text = NetBrkPrdInt.ToString();


            if (txtPropInsuAmt.Text != "")
                pPropInsuAmt = Convert.ToDecimal(txtPropInsuAmt.Text);
            if (txtPropInsuCGST.Text != "")
                pPropInsuCGST = Convert.ToDecimal(txtPropInsuCGST.Text);
            if (txtPropInsuSGST.Text != "")
                pPropInsuSGST = Convert.ToDecimal(txtPropInsuSGST.Text);
            if (txtPropInsuIGST.Text != "")
                pPropInsuIGST = Convert.ToDecimal(txtPropInsuIGST.Text);


            if (txtCERSAICharge.Text != "")
                pCERSAIAmt = Convert.ToDecimal(txtCERSAICharge.Text);
            if (txtCERSAIChargeCGST.Text != "")
                pCERSAICGST = Convert.ToDecimal(txtCERSAIChargeCGST.Text);
            if (txtCERSAIChargeSGST.Text != "")
                pCERSAISGST = Convert.ToDecimal(txtCERSAIChargeSGST.Text);
            if (txtCERSAIChargeIGST.Text != "")
                pCERSAIIGST = Convert.ToDecimal(txtCERSAIChargeIGST.Text);



            if (txtAdminFees.Text != "")
                pAdminFees = Convert.ToDecimal(txtAdminFees.Text);
            if (txtTechFees.Text != "")
                pTechFees = Convert.ToDecimal(txtTechFees.Text);


            GrandTotal = (AppCharge + StmpChrge + InsuFees + InsuCGSTAmt + InsuSGSTAmt + InsuIGSTAmt + InsuSBTax + ProcFees + LPFSTax + LPFKKtax + LPFSBtax
                + EMI + PEMIInt + PreLnBal + CGSTAmt + SGSTAmt + IGSTAmt + FLDGAmt + NetBrkPrdInt
                + pPropInsuAmt + pPropInsuCGST + pPropInsuSGST + pPropInsuIGST + pAdminFees + pTechFees
                + pCERSAIAmt + pCERSAICGST + pCERSAISGST + pCERSAIIGST);
            if (chkAdvEMI.Checked == true)
            {
                TotalCharge = GrandTotal;
            }
            else
            {
                TotalCharge = GrandTotal - EMI;
            }
            txtTotCharge.Text = System.Math.Round(TotalCharge, 2).ToString();
            NetDisbAmt = (SanAmt - TotalCharge);
            txtNetDisbAmt.Text = System.Math.Round(NetDisbAmt, 2).ToString();
        }
        protected void chkAdvEMI_CheckedChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        protected void txtDisbDate_TextChanged(object sender, EventArgs e)
        {
            if (txtDisbDate.Text != "" && txtRepayStDt.Text != "")
            {
                // CalBokenPeriodInt();
                //DateTime vDisbDate = gblFuction.setDate(txtDisbDate.Text);
                //DateTime vRepayStartDate = gblFuction.setDate(txtRepayStDt.Text);
                //if (vDisbDate > vRepayStartDate)
                //{
                //    gblFuction.AjxMsgPopup("Disburse Date Should be less than First Installment Date");
                //    txtPreEMIInt.Text = "0.00";
                //}
                //else
                //{
                //    if (vDisbDate.AddMonths(1).Date != vRepayStartDate.Date)
                //    {
                //        Double SanctAmt = Convert.ToDouble(txtSancAmt.Text);
                //        Double FIntRate = Convert.ToDouble(txtFIntRate.Text);
                //        Double vPreEMIInt = 0;

                //        TimeSpan ts = vRepayStartDate.AddMonths(-1).Subtract(vDisbDate);
                //        double nodays = ts.TotalDays;
                //        if (nodays > 0)
                //        {
                //            vPreEMIInt = Math.Round((SanctAmt * FIntRate * 0.01 * nodays) / 365, 0);
                //        }
                //        else
                //        {
                //            vPreEMIInt = 0.00;
                //        }

                //        txtPreEMIInt.Text = vPreEMIInt.ToString("0.00");
                //    }
                //    else
                //    {
                //        txtPreEMIInt.Text = "0.00";
                //    }
                //}
            }
            else
            {
                txtPreEMIInt.Text = "0.00";
            }
            CalculateTotalCharge();
        }
        protected void txtRepayStDt_TextChanged(object sender, EventArgs e)
        {
            if (txtDisbDate.Text != "" && txtRepayStDt.Text != "")
            {
                DateTime vDisbDate = gblFuction.setDate(txtDisbDate.Text);
                DateTime vRepayStartDate = gblFuction.setDate(txtRepayStDt.Text);
                if (vDisbDate > vRepayStartDate)
                {
                    gblFuction.AjxMsgPopup("Disburse Date Should be less than First Installment Date");
                    txtPreEMIInt.Text = "0.00";
                }
                else
                {
                    CalBokenPeriodInt();
                    //if (vDisbDate.AddMonths(1).Date != vRepayStartDate.Date)
                    //{
                    //    Double SanctAmt = Convert.ToDouble(txtSancAmt.Text);
                    //    Double FIntRate = Convert.ToDouble(txtFIntRate.Text);
                    //    Double vPreEMIInt = 0;

                    //    TimeSpan ts = vRepayStartDate.AddMonths(-1).Subtract(vDisbDate);
                    //    double nodays = ts.TotalDays;
                    //    if (nodays > 0)
                    //    {
                    //        vPreEMIInt = Math.Round((SanctAmt * FIntRate * 0.01 * nodays) / 365, 0);
                    //    }
                    //    else
                    //    {
                    //        vPreEMIInt = 0.00;
                    //    }

                    //    txtPreEMIInt.Text = vPreEMIInt.ToString("0.00");
                    //}
                    //else
                    //{
                    //    txtPreEMIInt.Text = "0.00";
                    //}
                }
            }
            else
            {
                txtPreEMIInt.Text = "0.00";
            }
            CalculateTotalCharge();
        }
        protected void ddlRepayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateEMI();
            CalculateProcFees();
            CalculateSTKKSBAmt();
            CalculateTotalCharge();
        }
        private void GetIRRFromFlatInterest()
        {
            CGenParameter oCG = new CGenParameter();
            decimal pNoOfInst = 0, pFlatRate = 0, pLoanAmt = 0, pReduceRate = 0, pEMIAmt = 0;

            try
            {
                if (txtInstNo.Text != "")
                {
                    pNoOfInst = Convert.ToDecimal(txtInstNo.Text);
                }
                //if (pNoOfInst == 0)
                //{
                //    gblFuction.MsgPopup("No Of Installment Must be Greater than Zero(0).");
                //    txtInstNo.Focus();
                //    return;
                //}
                if (txtFIntRate.Text != "")
                {
                    pFlatRate = Convert.ToDecimal(txtFIntRate.Text);
                }
                if (txtSancAmt.Text != "")
                {
                    pLoanAmt = Convert.ToDecimal(txtSancAmt.Text);
                }
                if (pNoOfInst > 0 && pFlatRate > 0)
                {
                    pEMIAmt = oCG.GetIRRFromFlatInterest(pNoOfInst, pFlatRate, pLoanAmt, Convert.ToDecimal(0), ref pReduceRate);
                }
                txtRIntRate.Text = pReduceRate.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCG = null;
            }
        }
        private void CalBokenPeriodInt()
        {
            decimal pLoanAmt = 0;
            double pReduceRate = 0;
            if (txtDisbDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Disbursement Date Can Not Be Empty");
                return;
            }
            //if (txtRepayStDt.Text == "")
            //{
            //    gblFuction.AjxMsgPopup("Repayment Start Date Can Not Be Empty");
            //    return;
            //}
            DateTime pDisbDate = gblFuction.setDate(txtDisbDate.Text);
            DateTime pRepStDate = gblFuction.setDate(txtRepayStDt.Text);
            if (txtSancAmt.Text != "")
            {
                pLoanAmt = Convert.ToDecimal(txtSancAmt.Text);
            }
            if (txtRIntRate.Text != "")
            {
                pReduceRate = Convert.ToDouble(txtRIntRate.Text);
            }
            DataTable dtDay = new DataTable();
            CDisburse oCR = new CDisburse();
            dtDay = oCR.CalCulateBrokenPrdInt(pDisbDate, pRepStDate, pLoanAmt, pReduceRate);
            txtBrkPrdIntAct.Text = dtDay.Rows[0]["BrokenPrdInt"].ToString();
            txtBrkPrdInt.Text = dtDay.Rows[0]["BrokenPrdInt"].ToString();
        }
        protected void txtBrkPrdIntWave_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtPropInsuAmt_TextChanged(object sender, EventArgs e)
        {
            CalculatePropertyInsuGST();
            CalculateTotalCharge();
        }

        protected void txtPropInsuCGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtPropInsuSGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtPropInsuIGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtAdminFees_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtTechFees_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void chkIGSTPF_CheckedChanged(object sender, EventArgs e)
        {
            decimal pPFAmt = 0, pPFCGST = 0, pPFSGST = 0, pPFIGST = 0;
            if (txtLPF.Text != "")
                pPFAmt = Convert.ToDecimal(txtLPF.Text);
            if (chkIGSTPF.Checked == true)
            {
                pPFCGST = 0;
                pPFSGST = 0;
                pPFIGST = ((pPFAmt * 18) / 100);
            }
            else
            {
                pPFCGST = ((pPFAmt * 9) / 100);
                pPFSGST = ((pPFAmt * 9) / 100);
                pPFIGST = 0;
            }
            txtCGSTAmt.Text = Math.Round(pPFCGST, 2).ToString();
            txtSGSTAmt.Text = Math.Round(pPFSGST, 2).ToString();
            txtIGSTAmt.Text = Math.Round(pPFIGST, 2).ToString();
            CalculateTotalCharge();
        }
        protected void chkIGSTInsu_CheckedChanged(object sender, EventArgs e)
        {
            CalculateInsuGST();
        }
        protected void chkIGSTPropInsu_CheckedChanged(object sender, EventArgs e)
        {
            CalculatePropertyInsuGST();
        }
        protected void txtCERSAICharge_TextChanged(object sender, EventArgs e)
        {
            CalculateCERSAIGST();
        }
        protected void chkIGSTCERSAICharge_CheckedChanged(object sender, EventArgs e)
        {
            CalculateCERSAIGST();
        }
        protected void txtCERSAIChargeCGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtCERSAIChargeSGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }
        protected void txtCERSAIChargeIGST_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalCharge();
        }

        private void PopInsuranceCompany()
        {
            DataTable dt = null;
            CLoanScheme oLS = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            oLS = new CLoanScheme();
            try
            {
                dt = oLS.GetInsuranceByBranchCode(vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlIC.DataTextField = "ICName";
                    ddlIC.DataValueField = "ICId";
                    ddlIC.DataSource = dt;
                    ddlIC.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlIC.Items.Insert(0, oItm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oLS = null;
            }

        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            CApplication oCA = new CApplication();
            Int32 vErr = 0;
            try
            {
                vErr = oCA.FinalSanctionSendBack(hdSancId.Value.ToString(), "P", Convert.ToInt32(Session[gblValue.UserId]), 0);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadGrid(1);
                    tabLoanSanc.ActiveTabIndex = 0;
                    StatusButton("View");
                    ViewState["StateEdit"] = null;
                    ClearControl();
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            finally
            {
                oCA = null;
            }
        }

        #region JocataCalling

        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vRampResponse = "";
            try
            {
                //-----------------------Ramp Request------------------------
                string postURL = "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
                string Requestdata = JsonConvert.SerializeObject(postRampRequest);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + vJokataToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vRampResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                return vRampResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            return vRampResponse;
        }

        public string JocataRequest(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            string vResponseData = "";
            DataTable dt = new DataTable();
            CDisburse oDb = null;
            try
            {
                oDb = new CDisburse();
                dt = oDb.GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    List<RequestVOList> vRVL = new List<RequestVOList>();
                    vRVL.Add(new RequestVOList
                    {
                        aadhar = dt.Rows[0]["Aadhar"].ToString(),
                        address = dt.Rows[0]["ParAddress"].ToString(),
                        city = dt.Rows[0]["District"].ToString(),
                        country = dt.Rows[0]["Country"].ToString(),
                        concatAddress = dt.Rows[0]["PreAddr"].ToString(),
                        customerId = dt.Rows[0]["MemberID"].ToString(),
                        digitalID = "",
                        din = "",
                        dob = dt.Rows[0]["DOB"].ToString(),
                        docNumber = "",
                        drivingLicence = dt.Rows[0]["DL"].ToString(),
                        email = "",
                        entityName = "",
                        name = dt.Rows[0]["MemberName"].ToString(),
                        nationality = "Indian",
                        pan = dt.Rows[0]["Pan"].ToString(),
                        passport = dt.Rows[0]["Passport"].ToString(),
                        phone = dt.Rows[0]["Mobile"].ToString(),
                        pincode = dt.Rows[0]["PinCode"].ToString(),
                        rationCardNo = dt.Rows[0]["RationCard"].ToString(),
                        ssn = "",
                        state = dt.Rows[0]["State"].ToString(),
                        tin = "",
                        voterId = dt.Rows[0]["Voter"].ToString()
                    });

                    var vLV = new RequestListVO();
                    vLV.businessUnit = "BU_Bijli";
                    vLV.subBusinessUnit = "Sub_BU_IB";
                    vLV.requestType = "API";
                    vLV.requestVOList = vRVL;

                    var vLMP = new ListMatchingPayload();
                    vLMP.requestListVO = vLV;

                    var vRR = new RampRequest();
                    vRR.listMatchingPayload = vLMP;

                    var req = new PostRampRequest();
                    req.rampRequest = vRR;

                    vResponseData = RampRequest(req);
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);
                    string vScreeningId = "";
                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vStatus = "N";
                        }
                        else
                        {
                            try
                            {
                                Prosidex(pMemberID, pLoanId, pCreatedBy);
                            }
                            finally { }
                        }
                        oDb = new CDisburse();
                        oDb.UpdateJocataStatus(pLoanId, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                    }
                    oDb = new CDisburse();
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    oDb.SaveJocataLog(pMemberID, pLoanId, vResponseXml, vScreeningId);
                }
            }
            catch
            {
                oDb = new CDisburse();
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                oDb.SaveJocataLog(pMemberID, pLoanId, vResponseXml, "");
            }
            finally { }
            return "";
        }
        #endregion

        #region Prosidex Integration
        public void Prosidex(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            string vResponse = "";
            PosidexReq vReq = new PosidexReq(pMemberID, pLoanId, pCreatedBy);
            string Requestdata = JsonConvert.SerializeObject(vReq);
            string postURL = vMobService + "/Prosidex";
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(Requestdata);
                streamWriter.Close();
            }
            StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
            vResponse = responseReader.ReadToEnd();
            request.GetResponse().Close();
        }
        #endregion

        #region Common
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
        #endregion

        #region Potential
        protected void btnShwPotenMem_Click(object sender, EventArgs e)
        {
            string vUrl = hdnProtenUrl.Value;
            string url = vUrl + "BIJLI";
            string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            return;
        }

        protected void btnUpdateUcic_Click(object sender, EventArgs e)
        {
            string pCustId = Convert.ToString(ViewState["CustNo"]);
            string pLoanId = Convert.ToString(ViewState["LnSancId"]);
            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            string pUcicId = getUcic("M" + pCustId, pCreatedBy, pLoanId);
            int pErr = -1;
            CDisburse hv = new CDisburse();
            if (pUcicId != "")
            {
                pErr = hv.UpdateUcicId(pUcicId, pCustId, pLoanId);
                if (pErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.EditMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            else
            {
                gblFuction.MsgPopup("Respose Error");
            }
        }

        public string getUcic(string pCustId, int pCreatedBy, string pLoanId)
        {
            string vResponse = "", vUcic = "";
            CDisburse oDb = new CDisburse();
            try
            {
                string Requestdata = "{\"cust_id\" :\"" + pCustId + "\",\"source_system_name\":\"BIJLI\"}";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLogUCIC(pCustId, pLoanId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLogUCIC(pCustId, pLoanId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }
        #endregion
    }

    public class PosidexReq
    {
        public string pMemberID { get; set; }
        public string pLoanId { get; set; }
        public int pCreatedBy { get; set; }
        public PosidexReq(string pMemberID, string pLoanId, int pCreatedBy)
        {
            this.pMemberID = pMemberID;
            this.pLoanId = pLoanId;
            this.pCreatedBy = pCreatedBy;
        }
    }
}
