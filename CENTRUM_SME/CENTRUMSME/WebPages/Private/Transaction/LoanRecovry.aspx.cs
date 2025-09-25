using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanRecovry : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable DtBrCntrl = null;
            InitBasePage();
            if (!IsPostBack)
            {
                //txtBankChrgPay.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                //txtTotPaid.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                ClearControls();
                PopBranch();
                popCustomer();
                //PopBank();
                GetCashBank();
                ViewState["StateEdit"] = null;
                txtRecovryDt.Text = (string)Session[gblValue.LoginDate];
                //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

                if (Session["BrCntrl"] != null)
                {
                    DtBrCntrl = (DataTable)Session["BrCntrl"];
                    ViewState["AllowAdvYN"] = Convert.ToString(DtBrCntrl.Rows[0]["AdvCollMEL"]);
                    ViewState["CashCollMEL"] = Convert.ToString(DtBrCntrl.Rows[0]["CashCollMEL"]);
                }
            }
        }
        private void InitBasePage()
        {

            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Collection";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanRecoveryInd);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    //btnDone.Visible = false;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    // btnDone.Visible = true;
                    //btnDel.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Collection", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }
        private void PopBranch()
        {
            ddlBranchName.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();

                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranchName.DataTextField = "BranchName";
                    ddlBranchName.DataValueField = "BranchCode";
                    ddlBranchName.DataSource = dt;
                    ddlBranchName.DataBind();
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
        private void popCustomer()
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = ddlBranchName.SelectedValue.ToString();
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetCustForRecovery(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlCustName.DataSource = dt;
                    ddlCustName.DataTextField = "CustName";
                    ddlCustName.DataValueField = "CustId";
                    ddlCustName.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCustName.Items.Insert(0, oli);
                }
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void ddlBranchName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pBranch = (Request[ddlBranchName.UniqueID] as string == null) ? ddlBranchName.SelectedValue : Request[ddlBranchName.UniqueID] as string;
            if (pBranch == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Branch...");
                return;
            }
            else
            {
                popCustomer();
            }
        }
        private void ClearControls()
        {
            txtLnAmt.Text = "0.00";
            txtLnDt.Text = "";
            txtIntRate.Text = "0.00";
            txtPrinOS.Text = "0.00";
            txtIntOs.Text = "0.00";
            txtPrinPay.Text = "0.00";
            txtIntPay.Text = "0.00";
            txtTotAmtPay.Text = "0.00";
            txtPrinPaid.Text = "0.00";
            txtIntPaid.Text = "0.00";
            txtGrandTotal.Text = "0.00";
            txtBounceAmt.Text = "0.00";
            txtBounceRec.Text = "0.00";
            txtBounceWave.Text = "0.00";
            txtBounceDue.Text = "0.00";
            txtPreCloseWaived.Text = "0.00";
            txtPreCloseWaived.Text = "0.00";
            txtFLDGBal.Text = "0.00";
            ddlBankLedgr.SelectedIndex = -1;
            txtBankNm.Text = "";
            txtChqRefNo.Text = "";
            gvLed.DataSource = null;
            gvLed.DataBind();
            txtPenAmt.Text = "0";
            txtPenWaiveAmt.Text = "0";
            txtPenCollAmt.Text = "0";
            txtPenCGST.Text = "0";
            txtPenSGST.Text = "0";
            txtPenDue.Text = "0";
            txtVisitChrge.Text = "0";
            txtVisitChrgeWaive.Text = "0";
            txtVisitChrgeRec.Text = "0";
            txtVisitChrgeDue.Text = "0";
            txtVisitCGST.Text = "0";
            txtVisitSGST.Text = "0";
            txtBounceCGST.Text = "0";
            txtBounceSGST.Text = "0";
            txtODAmount.Text = "0";
            txtODDays.Text = "0";
            txtPreDelayPay.Text = "0";
            txtPreVisitChrge.Text = "0";
            txtExcessCharge.Text = "0";
            txtForeClsLettChrge.Text = "0";
            txtPrinAdjust.Text = "0";
            chkLoanCancel.Checked = false;
            txtPrevAdvance.Text = "0";
            txtNewAdvance.Text = "0";
        }
        protected void rdbColl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rdbColl.SelectedValue == "I")
            //{
            //    IblGroupName.Visible = false;
            //    ddlGrp.Visible = false;
            //    ClearControls();
            //}
            //else if (rdbColl.SelectedValue == "G")
            //{
            //    IblGroupName.Visible = true;
            //    ddlGrp.Visible = true;
            //    ClearControls();
            //}
        }
        protected void ddlCustName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string;
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                oMem = new CLoanRecovery();
                dt = oMem.GetLoanNoForBounce(pCustId);
                ddlLoanNo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLoanNo.Items.Insert(0, oli);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        private void PopBank()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CVoucher oVou = new CVoucher();
            dt = oVou.GetBank(vBrCode);
            if (dt.Rows.Count > 0)
            {
                ddlBankLedgr.DataTextField = "Desc";
                ddlBankLedgr.DataValueField = "DescID";
                ddlBankLedgr.DataSource = dt;
                ddlBankLedgr.DataBind();
                ListItem oItem = new ListItem();
                oItem.Text = "<--- Select --->";
                oItem.Value = "-1";
                ddlBankLedgr.Items.Insert(0, oItem);
            }
        }

        private void GetCashBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (Convert.ToString(dr["DescId"]) == "C0001")
                    {
                        dr.Delete();
                        dt.AcceptChanges();
                    }
                }
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
        private void PopFunder()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                //ddlFSId.Items.Clear();
                //string vBrCode = Session[gblValue.BrnchCode].ToString();
                //oGbl = new CGblIdGenerator();
                //dt = oGbl.PopComboMIS("N", "N", "AA", "FunderId", "FunderName", "FunderMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                //ddlFSId.DataSource = dt;
                //ddlFSId.DataTextField = "FunderName";
                //ddlFSId.DataValueField = "FunderId";
                //ddlFSId.DataBind();
                //ListItem oLi = new ListItem("<--Select-->", "-1");
                //ddlFSId.Items.Insert(0, oLi);
                //ddlFSId.SelectedIndex = ddlFSId.Items.IndexOf(ddlFSId.Items.FindByValue("20"));
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        protected void rdbCashBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rdbCashBank.SelectedValue == "C")
            //{
            //    ClearControls();
            //    ddlBank.Enabled = false;
            //    ddlBank.SelectedIndex = -1;
            //}
            //else if (rdbCashBank.SelectedValue == "B")
            //{
            //    ClearControls();
            //    ddlBank.Enabled = true;
            //    ddlBank.SelectedIndex = -1;
            //}

        }
        private void popCO()
        {
            //DataTable dt = null;
            //CEO oCM = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            //try
            //{
            //    oCM = new CEO();
            //    dt = oCM.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
            //    ddlCo.DataSource = dt;
            //    ddlCo.DataTextField = "EOName";
            //    ddlCo.DataValueField = "EOID";
            //    ddlCo.DataBind();
            //    ListItem oli = new ListItem("<--Select-->", "-1");
            //    ddlCo.Items.Insert(0, oli);
            //}
            //finally
            //{
            //    oCM = null;
            //    dt = null;
            //}
        }
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string vEoId = ddlCo.SelectedItem.Value;
            //PopCenter(vEoId);
        }
        private void PopLedgerList()
        {
            //DataTable dt = null;
            //CAcGenled oGen = null;
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            //oGen = new CAcGenled();
            //dt = oGen.GetLedgerList();
            //ddlLedger.DataTextField = "Desc";
            //ddlLedger.DataValueField = "DescID";
            //ddlLedger.DataSource = dt;
            //ddlLedger.DataBind();
            //ListItem oItem = new ListItem();
            //oItem.Text = "<--- Select --->";
            //oItem.Value = "-1";
            //ddlLedger.Items.Insert(0, oItem);
        }
        private void popLoanNo(string pCustId)
        {
            DataTable dt = null;
            CLoanRecovery oMem = null;
            oMem = new CLoanRecovery();
            dt = oMem.GetLoanNoByCustId(pCustId);
            if (dt.Rows.Count > 0)
            {
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataSource = dt;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            //String vLnStatus = "", vLoanType = "", vDescId = "C0001", vCollType = "C";
            //string vGroupID = "", vLoanId = "", vMemberID = "", vMarketID = "", vEoId = "";
            //Int32 vRoutine = 0, vFunderId=0;
            //DataTable dt = null;
            //CLoanRecovery oLR = null;
            //Session["dtRst"] = null;
            //DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            //DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());

            //vFunderId = Convert.ToInt32(ddlFSId.SelectedValue.ToString());

            //if (Request[ddlCo.UniqueID] as string == "-1")
            //{
            //    gblFuction.AjxMsgPopup("Please Select the CRO Name..");
            //    return;
            //}
            //if (gblFuction.setDate(txtRecovryDt.Text) < vFinFromDt || gblFuction.setDate(txtRecovryDt.Text) > vFinToDt)
            //{
            //    gblFuction.AjxMsgPopup("Recovery Date should be login financial year.");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
            //    return;
            //}
            //try
            //{
            //    string vBrCode = Session[gblValue.BrnchCode].ToString();
            //    DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);
            //    if (chkRoutin.Checked == true)
            //        vRoutine = 1;
            //    else
            //        vRoutine = 0;

            //    if (rdbLoan.SelectedValue == "O")
            //        vLnStatus = "O";
            //    else if (rdbLoan.SelectedValue == "C")
            //        vLnStatus = "C";
            //    if (rdbColl.SelectedValue == "I")
            //        vLoanType = "I";
            //    else if (rdbColl.SelectedValue == "G")
            //        vLoanType = "G";
            //    vEoId = Request[ddlCo.UniqueID] as string;
            //    vMarketID = Request[ddlCenter.UniqueID] as string;
            //    if (string.IsNullOrEmpty(vMarketID) == true)
            //        vMarketID = "-1";
            //    vGroupID = Request[ddlGrp.UniqueID] as string;
            //    if (string.IsNullOrEmpty(vGroupID) == true)
            //        vGroupID = "-1";
            //    vMemberID = Request[ddlMember.UniqueID] as string;
            //    if (string.IsNullOrEmpty(vMemberID) == true)
            //        vMemberID = "-1";

            //    if(rdbCashBank.SelectedValue == "B")
            //    {
            //        vDescId = ddlBank.SelectedValue.ToString();
            //        vCollType="B";
            //    }

            //    oLR = new CLoanRecovery();
            //    dt = oLR.GetAllLoanCollection(vLnStatus, vRoutine, vLoanType, vRecvDt, vEoId, vMarketID, vGroupID, vMemberID, vBrCode, vFunderId, vDescId, vCollType);
            //    dt.PrimaryKey = new DataColumn[] { dt.Columns["LoanID"] };
            //    Session["dtRst"] = dt;
            //    gvRecvry.DataSource = dt;
            //    gvRecvry.DataBind();
            //    hdRowCount.Value = dt.Rows.Count.ToString();
            //    if (dt.Rows.Count > 0)
            //    {
            //        vLoanId = Convert.ToString(dt.Rows[0]["LoanID"]);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //    oLR = null;
            //}
        }
        protected void GetDetails(string pLoanId, DateTime pRecvDt, string pBrCode)
        {
            //DataTable dt = null;
            //DataTable dtDtl = null;
            //CLoanRecovery oLR = null;
            //decimal vTotDue = 0, vTotPaid = 0;
            //oLR = new CLoanRecovery();
            //Session["LoanId"] = pLoanId;
            //dt = oLR.GetCollectionByLoanId(pLoanId, pRecvDt, pBrCode);
            //if (dt.Rows.Count > 0)
            //{
            //    lblLnScme.Text = Convert.ToString(dt.Rows[0]["LoanTypeName"]);
            //    lblFund.Text = Convert.ToString(dt.Rows[0]["FunderName"]);
            //    txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt"]);
            //    txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]);
            //    txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
            //    txtPrinOS.Text = Convert.ToString(dt.Rows[0]["PrncOutStd"]);
            //    txtIntOs.Text = Convert.ToString(dt.Rows[0]["IntOutStd"]);
            //    txtPrinPay.Text = string.Format("{0:N}", dt.Rows[0]["PrincpalDue"]);
            //    txtIntPay.Text = string.Format("{0:N}", dt.Rows[0]["InterestDue"]);
            //    txtBankChrgPay.Text = string.Format("{0:N}", dt.Rows[0]["OthersAmt"]);
            //    vTotDue = Math.Round(Convert.ToDecimal(dt.Rows[0]["PrincpalDue"]) + Convert.ToDecimal(dt.Rows[0]["InterestDue"]) + Convert.ToDecimal(dt.Rows[0]["OthersAmt"]), 2);
            //    txtTotAmtPay.Text = string.Format("{0:N}", vTotDue);
            //    txtPrinPaid.Text = "0.00";
            //    txtIntPaid.Text = "0.00";
            //    vTotPaid = Convert.ToDecimal(dt.Rows[0]["PaidPric"]) + Convert.ToDecimal(dt.Rows[0]["PaidInt"]);
            //    txtTotPaid.Text = "0.00";
            //    dtDtl = oLR.GetCollectionDtlByLoanId(pLoanId, "M", pBrCode);
            //    if (dtDtl.Rows.Count > 0)
            //    {
            //        gvLed.DataSource = dtDtl;
            //        gvLed.DataBind();
            //        dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
            //        Session["dtDtlRst"] = dtDtl;
            //    }
            //    else
            //    {
            //        gvLed.DataSource = null;
            //        gvLed.DataBind();
            //    }
            //}
        }
        protected void txtRecDt_TextChanged(object sender, EventArgs e)
        {
            ClearControls();
        }
        protected void txtBankChrgPay_TextChanged(object sender, EventArgs e)
        {
            //txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text) + Convert.ToDecimal(txtBankChrgPay.Text));
        }
        protected void txtTotPaid_TextChanged(object sender, EventArgs e)
        {
            //DataTable dt = null;
            //CLoanRecovery oLR = null;
            //decimal vBalAmt = 0, vPrinAmt = 0, vIntAmt = 0;
            //decimal vPTot = 0, vITot = 0;
            //try
            //{
            //    if (Session["LoanId"] == null) return;
            //    if (txtTotPaid.Text == "")
            //    {
            //        txtTotPaid.Text = "0.00";
            //    }
            //    string vLoanId = Convert.ToString(Session["LoanId"]);
            //    string vBrCode = Session[gblValue.BrnchCode].ToString();
            //    decimal vTotPOS = Convert.ToDecimal(txtPrinOS.Text);
            //    decimal vTotIOS = Convert.ToDecimal(txtIntOs.Text);
            //    decimal vTotOS = vTotPOS + vTotIOS;
            //    if (chkPreClose.Checked == false)
            //    {
            //        oLR = new CLoanRecovery();
            //        dt = oLR.GetInstallAllocation(vLoanId, vBrCode);
            //        if (dt.Rows.Count > 0)
            //        {
            //            vPTot = Convert.ToDecimal(dt.Compute("Sum(PrinceAmt)", ""));
            //            vITot = Convert.ToDecimal(dt.Compute("Sum(InstAmt)", ""));

            //            if (vPTot > vTotPOS)
            //                dt.Rows[0]["PrinceAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) - (vPTot - vTotPOS);
            //            if (vITot > vTotIOS)
            //                dt.Rows[0]["InstAmt"] = Convert.ToDecimal(dt.Rows[0]["InstAmt"]) - (vITot - vTotIOS);

            //            dt.Rows[0]["TotAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) + Convert.ToDecimal(dt.Rows[0]["InstAmt"]);

            //            txtBankPaid.Text = string.Format("{0:N}", txtBankChrgPay.Text);
            //            vBalAmt = Convert.ToDecimal(txtTotPaid.Text) - Convert.ToDecimal(txtBankChrgPay.Text);

            //            if (vBalAmt > vTotOS)           // For Invalid Paid Total Value as Paid Total can not be > then Total OS
            //            {
            //                gblFuction.AjxMsgPopup("Total paid Amount should less than equal to total Principal and Interest OS..");
            //                return;
            //            }
            //            if (vTotOS == vBalAmt)      // For  Paid Total =  Total OS
            //            {
            //                txtPrinPaid.Text = txtPrinPay.Text;
            //                txtIntPaid.Text = txtIntPay.Text;
            //            }
            //            else if (vBalAmt < vTotOS) //Total Amt <=Total OS
            //            {
            //                foreach (DataRow dr in dt.Rows)
            //                {
            //                    if (vBalAmt >= Convert.ToDecimal(dr["TotAmt"].ToString()))
            //                    {
            //                        vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
            //                        vPrinAmt += Convert.ToDecimal(dr["PrinceAmt"].ToString());
            //                        vBalAmt = vBalAmt - (Convert.ToDecimal(dr["PrinceAmt"].ToString()) + Convert.ToDecimal(dr["InstAmt"].ToString()));
            //                    }
            //                    else if (vBalAmt > 0)
            //                    {
            //                        if (vBalAmt > Convert.ToDecimal(dr["InstAmt"].ToString()))
            //                        {
            //                            vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
            //                            vBalAmt = vBalAmt - Convert.ToDecimal(dr["InstAmt"].ToString());
            //                            vPrinAmt += vBalAmt;
            //                            vBalAmt = 0;
            //                        }
            //                        else
            //                        {
            //                            vIntAmt += vBalAmt;
            //                            vBalAmt = 0;
            //                        }
            //                    }
            //                }
            //                txtPrinPaid.Text = string.Format("{0:N}", vPrinAmt);
            //                txtIntPaid.Text = string.Format("{0:N}", vIntAmt);
            //            }
            //        }
            //        else
            //        {
            //            if (txtTotAmtPay.Text == txtTotPaid.Text)
            //            {
            //                txtPrinPaid.Text = string.Format("{0:N}", txtPrinPay.Text);
            //                txtIntPaid.Text = string.Format("{0:N}", txtIntPay.Text);
            //            }
            //            else
            //            {
            //                gblFuction.AjxMsgPopup("Invalied Collection Amount.");
            //                gblFuction.focus("ctl00_cph_Main_txtPDTotAmt");
            //                return;
            //            }
            //        }
            //        txtTotPaid.Focus();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //    oLR = null;
            //}
        }
        protected void chkJourEnt_CheckedChanged(object sender, EventArgs e)
        {
            //if (chkJurnl.Checked == true)
            //{
            //    ddlLedger.Enabled = true;
            //    ddlLedger.SelectedIndex = -1;
            //}
            //else
            //{
            //    ddlLedger.Enabled = false;
            //    ddlLedger.SelectedIndex = -1;
            //}
        }
        protected void chkPreClose_CheckedChanged(object sender, EventArgs e)
        {
            //DataTable dt = null;
            //DataTable dtM = null;
            //Int32 vRow = 0;
            //string vLoanID = "";
            //dt = (DataTable)Session["dtRst"];
            //if (Session["LoanId"] == null) return;
            //vLoanID = (string)Session["LoanId"];
            //vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //CCollectionRoutine oCC = new CCollectionRoutine();
            //dtM = oCC.GetMaxCollDate(vLoanID, vBrCode, "M");
            //if (dtM.Rows.Count > 0)
            //{
            //    if (Convert.ToString(dtM.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dtM.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
            //    {
            //        gblFuction.AjxMsgPopup(" Last collection date is" + Convert.ToString(dtM.Rows[0]["MaxCollDt"]));
            //        chkPreClose.Checked = false;
            //        return;
            //    }
            //}
            //try
            //{
            //    if (chkPreClose.Checked == true)
            //    {
            //        txtPrinPay.Text = string.Format("{0:N}", txtPrinOS.Text);
            //        txtBankPaid.Text = string.Format("{0:N}", txtBankChrgPay.Text);
            //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text) + Convert.ToDecimal(txtBankChrgPay.Text));
            //        txtIntPay.Text = Convert.ToString(Convert.ToDecimal(txtTotAmtPay.Text) - Convert.ToDecimal(txtPrinPay.Text) - Convert.ToDecimal(txtBankChrgPay.Text));
            //        txtPrinPaid.Text = txtPrinPay.Text;
            //        txtIntPaid.Text = txtIntPay.Text;
            //        txtBankPaid.Text = txtBankChrgPay.Text;
            //        txtTotPaid.Text = txtTotAmtPay.Text;
            //    }
            //    else
            //    {
            //        txtIntPay.Text = Convert.ToString(dt.Rows[vRow]["InterestDue"]);
            //        txtPrinPay.Text = Convert.ToString(dt.Rows[vRow]["PrincpalDue"]);
            //        txtBankChrgPay.Text = Convert.ToString(dt.Rows[vRow]["OthersAmt"]);
            //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text) + Convert.ToDecimal(txtBankChrgPay.Text));
            //        txtPrinPaid.Text = txtPrinPay.Text;
            //        txtIntPaid.Text = txtIntPay.Text;
            //        txtBankPaid.Text = txtBankChrgPay.Text;
            //        txtTotPaid.Text = txtTotAmtPay.Text;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //}
        }
        protected void chkDeathClose_CheckedChanged(object sender, EventArgs e)
        {
            //DataTable dt = null;
            //DataTable dtM = null;
            //Int32 vRow = 0;
            //string vLoanID = "";
            //dt = (DataTable)Session["dtRst"];
            //if (Session["LoanId"] == null) return;
            //vLoanID = (string)Session["LoanId"];
            //vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //CCollectionRoutine oCC = new CCollectionRoutine();
            //dtM = oCC.GetMaxCollDate(vLoanID, vBrCode, "M");
            //if (dtM.Rows.Count > 0)
            //{
            //    if (Convert.ToString(dtM.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dtM.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
            //    {
            //        gblFuction.AjxMsgPopup(" Last collection date is" + Convert.ToString(dtM.Rows[0]["MaxCollDt"]));
            //        chkDeathClose.Checked = false;
            //        return;
            //    }
            //}
            //try
            //{
            //    if (chkDeathClose.Checked == true)
            //    {
            //        txtPrinPay.Text = string.Format("{0:N}", txtPrinOS.Text);
            //        txtIntPay.Text = string.Format("{0:N}", txtIntOs.Text);
            //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));
            //        txtPrinPaid.Text = txtPrinPay.Text;
            //        txtIntPaid.Text = txtIntPay.Text;
            //        txtTotPaid.Text = txtTotAmtPay.Text;
            //        txtTotPaid.Enabled = false;
            //    }
            //    else
            //    {
            //        txtIntPay.Text = Convert.ToString(dt.Rows[vRow]["InterestDue"]);
            //        txtPrinPay.Text = Convert.ToString(dt.Rows[vRow]["PrincpalDue"]);
            //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));
            //        txtPrinPaid.Text = txtPrinPay.Text;
            //        txtIntPaid.Text = txtIntPay.Text;
            //        txtTotPaid.Text = txtTotAmtPay.Text;
            //        txtTotPaid.Enabled = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //}
        }
        protected void ddlPyn_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataTable dt = null;
            //Int32 vRow = 0;
            //try
            //{
            //    string vBrCode = Session[gblValue.BrnchCode].ToString();
            //    DropDownList ddlPA = (DropDownList)sender;
            //    GridViewRow gvRow = (GridViewRow)ddlPA.NamingContainer;
            //    vRow = gvRow.RowIndex;
            //    DropDownList ddlPAList = (DropDownList)gvRow.FindControl("ddlPyn");
            //    dt = (DataTable)Session["dtRst"];
            //    if (ddlPAList.SelectedValue == "N")
            //        dt.Rows[vRow]["PA"] = "N";
            //    Session["dtRst"] = dt;
            //}
            //catch { }
            //finally
            //{
            //    dt = null;
            //}
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            CLoanRecovery oLR = null;
            string vNaration = "", vCollMode = "", vTransMode = "", vBankName = "", vChqRefNo = "", vSancId = "", vLoanId = "",vLoanNo = "", vCustId = "", vColsingType = "";
            Int32 vErr = 0, vODDays = 0;
            // Boolean vCheck = false;
            Decimal vPrinColl = 0, vIntColl = 0, vWaveInt = 0, vTotColl = 0, vPrinOS = 0, vIntOS = 0, vBounceRecv = 0, vBounnceWave = 0, vBounnceDue = 0,
                vPreCloseChrge = 0, vPreCloseWaiveCharge = 0, vFlDGBal = 0, vIntDue = 0,
                vPenColl = 0, vPenAmt = 0, vPenWaiveAmt = 0, vPenDue = 0, vVisitCharge = 0, vVisitingWaive = 0, vVisitingRec = 0, vVisitingDue = 0, vODAmt = 0,
                vPenCGST = 0, vPenSGST = 0, vVisitCGST = 0, vVisitSGST = 0, vExcessCharge = 0, vForeCloseLetterCharge = 0, vPrincAdjustment = 0, vPrevAdvance = 0, vNewAdvance;
            string vBankLedgrAC = "", vPreCollMode = "";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vXmlAC = string.Empty;
            string vNarationL = string.Empty;
            string vNarationF = string.Empty;
            string FLDGAmt = (Request[txtFLDGBal.UniqueID] as string == null) ? txtFLDGBal.Text : Request[txtFLDGBal.UniqueID] as string;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime FinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString()),
               FinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            try
            {
                DateTime vAccDate = gblFuction.setDate(txtRecovryDt.Text);
                if ((vAccDate < FinFrom) || (vAccDate > FinTo))
                {
                    gblFuction.AjxMsgPopup("Loan Recovery Date must be with in Login Financial Year");
                    return;
                }

                if (Convert.ToString(ViewState["AllowAdvYN"]) == "N" && Convert.ToDouble(txtNewAdvance.Text) > 0)
                {
                    gblFuction.AjxMsgPopup("Advance collection is not allowed in this Branch...");
                    return;
                }

                if (Convert.ToString(ViewState["CashCollMEL"]) == "N" && rdbPayMode.SelectedValue == "C")
                {
                    gblFuction.AjxMsgPopup("Cash collection is not allowed in this Branch...");
                    return;
                }
                // string vBranch = Session[gblValue.BrnchCode].ToString();
                string vBranch = ddlBranchName.SelectedValue.ToString();
                if (vBranch == "-1" || vBranch == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Branch...");
                    return;
                }

                string vBank = ddlBank.SelectedValue.ToString();
                
                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                vNaration = "Being the Amt of Loan Collection On ";
                // vLoanId = (Request[txtLoanNo.UniqueID] as string == null) ? txtLoanNo.Text : Request[txtLoanNo.UniqueID] as string;

                vPreCollMode = (Request[hdPreCollMode.UniqueID] as string == null) ? hdPreCollMode.Value : Request[hdPreCollMode.UniqueID] as string;

                vPrevAdvance = Convert.ToDecimal((Request[txtPrevAdvance.UniqueID] as string == null) ? txtPrevAdvance.Text : Request[txtPrevAdvance.UniqueID] as string);
                vIntDue = Convert.ToDecimal((Request[hdIntDue.UniqueID] as string == null) ? hdIntDue.Value : Request[hdIntDue.UniqueID] as string);

                vPrinColl = Convert.ToDecimal((Request[txtPrinPaid.UniqueID] as string == null) ? txtPrinPaid.Text : Request[txtPrinPaid.UniqueID] as string);
                vIntColl = Convert.ToDecimal((Request[txtIntPaid.UniqueID] as string == null) ? txtIntPaid.Text : Request[txtIntPaid.UniqueID] as string);
                vPenColl = Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string);
                vTotColl = Convert.ToDecimal((Request[txtGrandTotal.UniqueID] as string == null) ? txtGrandTotal.Text : Request[txtGrandTotal.UniqueID] as string);
                vPrinOS = Convert.ToDecimal((Request[txtPrinOS.UniqueID] as string == null) ? txtPrinOS.Text : Request[txtPrinOS.UniqueID] as string);
                vIntOS = Convert.ToDecimal((Request[txtIntOs.UniqueID] as string == null) ? txtIntOs.Text : Request[txtIntOs.UniqueID] as string);

                decimal vBounceAmt = 0, vBounceCGST = 0, vBounceSGST = 0, vTotalWebOffAmt = 0;
                vBounceRecv = Convert.ToDecimal((Request[txtBounceRec.UniqueID] as string == null) ? txtBounceRec.Text : Request[txtBounceRec.UniqueID] as string);
                vBounceAmt = Convert.ToDecimal((Request[txtBounceAmt.UniqueID] as string == null) ? txtBounceAmt.Text : Request[txtBounceAmt.UniqueID] as string);
                vBounceCGST = Convert.ToDecimal((Request[txtBounceCGST.UniqueID] as string == null) ? txtBounceCGST.Text : Request[txtBounceCGST.UniqueID] as string);
                vBounceSGST = Convert.ToDecimal((Request[txtBounceSGST.UniqueID] as string == null) ? txtBounceSGST.Text : Request[txtBounceSGST.UniqueID] as string);
                //if (vBounceRecv > 0)
                //{
                //    vBounceAmt = Convert.ToDecimal(vBounceRecv * 100 / 118);
                //    vBounceCGST = Convert.ToDecimal(vBounceAmt * 9 / 100);
                //    vBounceSGST = Convert.ToDecimal(vBounceAmt * 9 / 100);
                //}

                vBounnceWave = Convert.ToDecimal((Request[txtBounceWave.UniqueID] as string == null) ? txtBounceWave.Text : Request[txtBounceWave.UniqueID] as string);
                vBounnceDue = Convert.ToDecimal((Request[txtBounceDue.UniqueID] as string == null) ? txtBounceDue.Text : Request[txtBounceDue.UniqueID] as string);
                //vFlDGBal = Convert.ToDecimal((Request[txtFLDGBal.UniqueID] as string == null) ? txtFLDGBal.Text : Request[txtFLDGBal.UniqueID] as string);
                //vColsingType = (Request[hdClosingType.UniqueID] as string == null) ? hdClosingType.Value : Request[hdClosingType.UniqueID] as string;
                //if (vColsingType == "")
                //    vColsingType = "N";
                //vPreCloseChrge = Convert.ToDecimal((Request[txtPreCloseChrge.UniqueID] as string == null) ? txtPreCloseChrge.Text : Request[txtPreCloseChrge.UniqueID] as string);
                //vPreCloseWaiveCharge = Convert.ToDecimal((Request[txtPreCloseWaived.UniqueID] as string == null) ? txtPreCloseWaived.Text : Request[txtPreCloseWaived.UniqueID] as string);
                //vExcessCharge = Convert.ToDecimal((Request[txtExcessCharge.UniqueID] as string == null) ? txtExcessCharge.Text : Request[txtExcessCharge.UniqueID] as string);

                vCustId = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string;
                vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                vLoanNo = ddlLoanNo.SelectedItem.Text;

                // vPenColl = 0,vPenAmt=0,vPenWaiveAmt=0,vPenDue=0,vVisitCharge=0,vVisitingWaive=0,vVisitingRec=0,vVisitingDue=0,vODAmt=0,vODDays=0,
                // vPenCGST=0,vPenSGST=0,vVisitCGST=0,vVisitSGST=0;
                //vPenAmt,vPenWaiveAmt,vPenDue,vVisitCharge,vVisitingWaive,vVisitingRec,vVisitingDue,vPenCGST,vPenSGST,vVisitCGST,vVisitSGST,vODDays,vODAmt
                vPenAmt = Convert.ToDecimal((Request[txtPenAmt.UniqueID] as string == null) ? txtPenAmt.Text : Request[txtPenAmt.UniqueID] as string);
                vPenWaiveAmt = Convert.ToDecimal((Request[txtPenWaiveAmt.UniqueID] as string == null) ? txtPenWaiveAmt.Text : Request[txtPenWaiveAmt.UniqueID] as string);
                vPenCGST = Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string);
                vPenSGST = Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string);
                vPenDue = Convert.ToDecimal((Request[txtPenDue.UniqueID] as string == null) ? txtPenDue.Text : Request[txtPenDue.UniqueID] as string);

                vVisitCharge = Convert.ToDecimal((Request[txtVisitChrge.UniqueID] as string == null) ? txtVisitChrge.Text : Request[txtVisitChrge.UniqueID] as string);
                vVisitingWaive = Convert.ToDecimal((Request[txtVisitChrgeWaive.UniqueID] as string == null) ? txtVisitChrgeWaive.Text : Request[txtVisitChrgeWaive.UniqueID] as string);
                vVisitingRec = Convert.ToDecimal((Request[txtVisitChrgeRec.UniqueID] as string == null) ? txtVisitChrgeRec.Text : Request[txtVisitChrgeRec.UniqueID] as string);
                vVisitCGST = Convert.ToDecimal((Request[txtVisitCGST.UniqueID] as string == null) ? txtVisitCGST.Text : Request[txtVisitCGST.UniqueID] as string);
                vVisitSGST = Convert.ToDecimal((Request[txtVisitSGST.UniqueID] as string == null) ? txtVisitSGST.Text : Request[txtVisitSGST.UniqueID] as string);
                vVisitingDue = Convert.ToDecimal((Request[txtVisitChrgeDue.UniqueID] as string == null) ? txtVisitChrgeDue.Text : Request[txtVisitChrgeDue.UniqueID] as string);
                vNewAdvance = Convert.ToDecimal((Request[txtNewAdvance.UniqueID] as string == null) ? txtNewAdvance.Text : Request[txtNewAdvance.UniqueID] as string);
                vTotalWebOffAmt = Convert.ToDecimal((Request[txtTotWaiveAmt.UniqueID] as string == null) ? txtTotWaiveAmt.Text : Request[txtTotWaiveAmt.UniqueID] as string);

                //if (((Request[txtODAmount.UniqueID] as string == null) ? txtODAmount.Text : Request[txtODAmount.UniqueID] as string) != "")
                //    vODAmt = Convert.ToDecimal((Request[txtODAmount.UniqueID] as string == null) ? txtODAmount.Text : Request[txtODAmount.UniqueID] as string);
                //if (((Request[txtODDays.UniqueID] as string == null) ? txtODDays.Text : Request[txtODDays.UniqueID] as string) != "")
                //    vODDays = Convert.ToInt32((Request[txtODDays.UniqueID] as string == null) ? txtODDays.Text : Request[txtODDays.UniqueID] as string);
                //if (((Request[txtForeClsLettChrge.UniqueID] as string == null) ? txtForeClsLettChrge.Text : Request[txtForeClsLettChrge.UniqueID] as string) != "")
                //    vForeCloseLetterCharge = Convert.ToDecimal((Request[txtForeClsLettChrge.UniqueID] as string == null) ? txtForeClsLettChrge.Text : Request[txtForeClsLettChrge.UniqueID] as string);
                // if (((Request[txtPrinAdjust.UniqueID] as string == null) ? txtPrinAdjust.Text : Request[txtPrinAdjust.UniqueID] as string) != "")
                //    vPrincAdjustment = Convert.ToDecimal((Request[txtPrinAdjust.UniqueID] as string == null) ? txtPrinAdjust.Text : Request[txtPrinAdjust.UniqueID] as string);

                 //if (chkLoanCancel.Checked == false && vPrincAdjustment > 0)
                 //{
                 //    gblFuction.AjxMsgPopup("Principal Collection Amount can not be greater than Principal Outstanding");
                 //    return;
                 //}
                 //if (chkLoanCancel.Checked == true)
                 //{
                 //    if ((vPrinOS - vPrinColl - vPrincAdjustment) > 0)
                 //    {
                 //        gblFuction.AjxMsgPopup("In Case of Loan Cancellation POS Amount will be sum of Principal Collected Amount + Principal Adjustment Amount,Otherwise loan will not be closed.");
                 //        return;
                 //    }
                 //}
                if (vPrinColl > vPrinOS)
                {
                    gblFuction.AjxMsgPopup("Principal Collection Amount can not be greater than Principal Outstanding");
                    return;
                }

                if (this.AdvAllow == "N")
                {
                    if (vNewAdvance > 0)
                    {
                        gblFuction.AjxMsgPopup("Advance collection is not allowed for this user");
                    }
                }
                else
                {
                    if (vNewAdvance > 0)
                    {
                        if (vNewAdvance > ((vPrinOS - vPrinColl) + (vIntOS - vIntColl)))
                        {
                            gblFuction.AjxMsgPopup("Advance amount should not be more than total outstanding.");
                        }
                    }
                }

                if(vTotColl != 0)
                {
                    if (vBank == "-1" || vBank == "")
                    {
                        gblFuction.AjxMsgPopup("Please Select Collection Mode...");
                        return;
                    }
                }
                if (vTotColl + vTotalWebOffAmt <= 0)
                {
                    gblFuction.AjxMsgPopup("Collected amount should not be zero..");
                    return;
                }

                Int32 vUserId = 0;
                vUserId = Convert.ToInt32(Session[gblValue.UserId]);
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vAccDate)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                }

                // Check Max Collection Date
                oLR = new CLoanRecovery();
                DataTable dtMaxCollDt = new DataTable();
                dtMaxCollDt = oLR.GetLastCollDate(vLoanId);
                if (dtMaxCollDt.Rows.Count > 0)
                {
                    DateTime MaxCollDate = gblFuction.setDate(dtMaxCollDt.Rows[0]["MaxCollDt"].ToString());
                    if (vAccDate < MaxCollDate)
                    {
                        gblFuction.AjxMsgPopup("You can not take collection before Last Collection Date");
                        return;
                    }
                }

                if ((vPrinColl + vIntColl) > 0)
                {
                    if (hdIsPDD.Value == "Y")
                    {
                        gblFuction.AjxMsgPopup("Member declare as a provisional death so you can not take Collection.");
                        return;
                    }
                }


                DataTable dtWaivePermission = new DataTable();
                dtWaivePermission = oLR.GetWaiverPremission(vUserId);
                if (vTotalWebOffAmt > 0)
                {
                    if (dtWaivePermission.Rows.Count > 0)
                    {
                        if (dtWaivePermission.Rows[0]["MEL_Waive_Allow"].ToString() == "N")
                        {
                            gblFuction.AjxMsgPopup("Waiver posting is not allowed for your Role.");
                            return;
                        }
                    }
                }
                

                //// In Case of FLDG Adjusment
                //if (rdbPayMode.SelectedValue == "F")
                //{
                //    if (vTotColl > vFlDGBal)
                //    {
                //        gblFuction.AjxMsgPopup("In Case of FLDG Adjusment Total Collected Amount Can Not Be Greater than FLDG balance Amount");
                //        return;
                //    }
                //}
                
                //if (chkPreClose.Checked == true)
                //{
                //    if (vPrinOS > vPrinColl)
                //    {
                //        gblFuction.AjxMsgPopup("In case of Pre-Close Principal Collection Amount and Principal Outstanding should be same");
                //        return;
                //    }
                //}
                // Incase of zero or minus collection
                //if (Convert.ToDecimal(Request[txtTotPaid.UniqueID] as string) <= 0)
                //{
                //    gblFuction.AjxMsgPopup("Total Collection (P + I) Amount Can Not be Zero or Negetive");
                //    return;
                //}
                CDisburse oLD = new CDisburse();
                DataTable dt = null;
                //int vLoanTypeID = 0;
                //string vLoanAc = "", vLoanIntAc = "", vPenalAc = "", vIntWeaveAc = "", vBounceChrgAC = "", vBounceChrgWaveAC = "", vPreCloseChrgAC = "",
                //    vPreCloseWaiveAC = "", vIntDueAC = "", vVisitChrgeAC = "", vCGSTAC, vSGSTAC = "", vExcessChargeAC = "", vTrnsDisbAc = "";
                //dt = oLD.GetDisbDtlbyLoanId(vLoanId, vBranch);
                //if (dt.Rows.Count > 0)
                //{
                //    vLoanTypeID = Convert.ToInt32(dt.Rows[0]["LoanTypeId"]);
                //    vLoanAc = Convert.ToString(dt.Rows[0]["LoanAC"]);
                //    vLoanIntAc = Convert.ToString(dt.Rows[0]["InstAC"]);
                //    vPenalAc = Convert.ToString(dt.Rows[0]["PenaltyChargeAC"]);
                //    vIntWeaveAc = Convert.ToString(dt.Rows[0]["IntWaveAC"]);
                //    vBounceChrgAC = Convert.ToString(dt.Rows[0]["BounceChrgAC"]);
                //    vBounceChrgWaveAC = Convert.ToString(dt.Rows[0]["BounceChrgWaveAC"]);
                //    vPreCloseChrgAC = Convert.ToString(dt.Rows[0]["PreCloseChrgAC"]);
                //    vPreCloseWaiveAC = Convert.ToString(dt.Rows[0]["PreCloseChrgWaiveAC"]);
                //    vIntDueAC = Convert.ToString(dt.Rows[0]["IntDueAC"]);
                //    vVisitChrgeAC = Convert.ToString(dt.Rows[0]["VisitChargeAC"]);
                //    vCGSTAC = Convert.ToString(dt.Rows[0]["CGSTAC"]);
                //    vSGSTAC = Convert.ToString(dt.Rows[0]["SGSTAC"]);
                //    vExcessChargeAC = Convert.ToString(dt.Rows[0]["ExcessChargeAC"]);
                //}
                //else
                //{
                //    gblFuction.AjxMsgPopup("Invalid Loan.");
                //    return;
                //}
                //string vPreCloseCGSTAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.CGSTAC, vLoanTypeID, vBrCode);
                //string vPreCloseSGSTAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.SGSTAC, vLoanTypeID, vBrCode);
                //string vFLDGAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.FLDGAC, vLoanTypeID, vBrCode);

                //if (((Request[hdIsTranche.UniqueID] as string == null) ? hdIsTranche.Value : Request[hdIsTranche.UniqueID] as string) == "Y")
                //{
                //    CGblIdGenerator oCG = new CGblIdGenerator();
                //    DataTable dtTrans = oCG.GetTransDisbAc();
                //    vTrnsDisbAc = dtTrans.Rows[0]["ToBeDisburseAC"].ToString();
                //    if (vTrnsDisbAc == "")
                //    {
                //        gblFuction.MsgPopup("Please Set Tranche Disbursement Pending Account");
                //        return;
                //    }
                //}
                //#region ForAccounts
                //// Collection Mode---  Cash
                //if (rdbPayMode.SelectedValue == "C")
                //{
                //    vCollMode = "C";
                //    vBankLedgrAC = "C0001";
                //    vBankName = "";
                //    vChqRefNo = "";
                //    vTransMode = "C";
                //}
                //// Collection Mode---  Bank
                //else if (rdbPayMode.SelectedValue == "B")
                //{
                //    if (ddlBankLedgr.SelectedValue == "-1")
                //    {
                //        gblFuction.AjxMsgPopup("Please Select Ledger Account...");
                //        return;
                //    }
                //    vCollMode = "B";
                //    vBankLedgrAC = ddlBankLedgr.SelectedValue;
                //    vBankName = txtBankNm.Text.ToString().Trim();
                //    if (txtChqRefNo.Text.ToString() == "")
                //    {
                //        gblFuction.AjxMsgPopup("Please Give Cheque No...");
                //        return;
                //    }
                //    vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                //    vTransMode = "B";
                //}
                //// Collection Mode---  NEFT/RTGS
                //else if (rdbPayMode.SelectedValue == "N")
                //{
                //    if (ddlBankLedgr.SelectedValue == "-1")
                //    {
                //        gblFuction.AjxMsgPopup("Please Select Ledger Account");
                //        return;
                //    }
                //    vCollMode = "N";
                //    vBankLedgrAC = ddlBankLedgr.SelectedValue;
                //    vBankName = txtBankNm.Text.ToString().Trim();
                //    vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                //    vTransMode = "B";
                //}
                //// Collection Mode---  ECS/NACH
                //else if (rdbPayMode.SelectedValue == "H")
                //{
                //    if (ddlBankLedgr.SelectedValue == "-1")
                //    {
                //        gblFuction.AjxMsgPopup("Please Select Ledger Account");
                //        return;
                //    }
                //    vCollMode = "H";
                //    vBankLedgrAC = ddlBankLedgr.SelectedValue;
                //    vBankName = txtBankNm.Text.ToString().Trim();
                //    vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                //    vTransMode = "B";
                //}
                //// Collection Mode--- PDC
                //else if (rdbPayMode.SelectedValue == "P")
                //{
                //    if (ddlBankLedgr.SelectedValue == "-1")
                //    {
                //        gblFuction.AjxMsgPopup("Please Select Ledger Account");
                //        return;
                //    }
                //    vCollMode = "P";
                //    vBankLedgrAC = ddlBankLedgr.SelectedValue;
                //    vBankName = txtBankNm.Text.ToString().Trim();
                //    vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                //    vTransMode = "B";
                //}
                //// Collection Mode--- UPI/EFT
                //else if (rdbPayMode.SelectedValue == "U")
                //{
                //    if (ddlBankLedgr.SelectedValue == "-1")
                //    {
                //        gblFuction.AjxMsgPopup("Please Select Ledger Account");
                //        return;
                //    }
                //    vCollMode = "U";
                //    vBankLedgrAC = ddlBankLedgr.SelectedValue;
                //    vBankName = txtBankNm.Text.ToString().Trim();
                //    vChqRefNo = txtChqRefNo.Text.ToString().Trim();
                //    vTransMode = "B";
                //}
                //// Collection Mode--- FLDG
                //else if (rdbPayMode.SelectedValue == "F")
                //{
                //    vCollMode = "F";
                //    vBankLedgrAC = vFLDGAC;
                //    vBankName = "";
                //    vChqRefNo = "";
                //    vTransMode = "J";
                //}

                //DataTable dtAccount = new DataTable();
                //DataRow dr;
                //DataColumn dc = new DataColumn();
                //dc.ColumnName = "DC";
                //dtAccount.Columns.Add(dc);
                //DataColumn dc1 = new DataColumn();
                //dc1.ColumnName = "Amt";
                //dc1.DataType = System.Type.GetType("System.Decimal");
                //dtAccount.Columns.Add(dc1);
                //DataColumn dc2 = new DataColumn();
                //dc2.ColumnName = "DescId";
                //dtAccount.Columns.Add(dc2);
                //DataColumn dc3 = new DataColumn();
                //dc3.ColumnName = "DtlId";
                //dtAccount.Columns.Add(dc3);
                //dtAccount.TableName = "Table1";

                //if (Convert.ToDecimal((Request[txtGrandTotal.UniqueID] as string == null) ? txtGrandTotal.Text : Request[txtGrandTotal.UniqueID] as string) > 0)
                //{
                //    int i = 1;
                //    dr = dtAccount.NewRow();
                //    dr["DescId"] = vBankLedgrAC;
                //    dr["DC"] = "D";
                //    dr["Amt"] = Convert.ToDecimal((Request[txtGrandTotal.UniqueID] as string == null) ? txtGrandTotal.Text : Request[txtGrandTotal.UniqueID] as string);
                //    dr["DtlId"] = i;
                //    dtAccount.Rows.Add(dr);
                //    dtAccount.AcceptChanges();
                //    if (chkLoanCancel.Checked == true)
                //    {
                //        if (Convert.ToDecimal((Request[txtPrinAdjust.UniqueID] as string == null) ? txtPrinAdjust.Text : Request[txtPrinAdjust.UniqueID] as string) > 0)
                //        {
                //            i = i + 1;
                //            dr = dtAccount.NewRow();
                //            dr["DescId"] = vTrnsDisbAc;
                //            dr["DC"] = "D";
                //            dr["Amt"] = Convert.ToDecimal((Request[txtPrinAdjust.UniqueID] as string == null) ? txtPrinAdjust.Text : Request[txtPrinAdjust.UniqueID] as string);
                //            dr["DtlId"] = i;
                //            dtAccount.Rows.Add(dr);
                //            dtAccount.AcceptChanges();
                //        }
                //        if (Convert.ToDecimal((Request[txtLnAmt.UniqueID] as string == null) ? txtLnAmt.Text : Request[txtLnAmt.UniqueID] as string) > 0)
                //        {
                //            if (string.IsNullOrEmpty(vLoanAc) == true)
                //            {
                //                gblFuction.AjxMsgPopup("Loan Account Not Set In Loan Parameter");
                //                return;
                //            }
                //            i = i + 1;
                //            dr = dtAccount.NewRow();
                //            dr["DescId"] = vLoanAc;
                //            dr["DC"] = "C";
                //            dr["Amt"] = Convert.ToDecimal((Request[txtLnAmt.UniqueID] as string == null) ? txtLnAmt.Text : Request[txtLnAmt.UniqueID] as string);
                //            dr["DtlId"] = i;
                //            dtAccount.Rows.Add(dr);
                //            dtAccount.AcceptChanges();
                //        }
                //        if (vIntColl > 0)
                //        {
                //            if (string.IsNullOrEmpty(vLoanIntAc) == true)
                //            {
                //                gblFuction.AjxMsgPopup("Interest Account Not Set In Loan Parameter");
                //                return;
                //            }
                //            i = i + 1;
                //            dr = dtAccount.NewRow();
                //            dr["DescId"] = vLoanIntAc;
                //            dr["DC"] = "C";
                //            dr["Amt"] = vIntColl;
                //            dr["DtlId"] = i;
                //            dtAccount.Rows.Add(dr);
                //            dtAccount.AcceptChanges();
                //        }
                //    }
                //    else
                //    {
                //        if (Convert.ToDecimal((Request[txtPrinPaid.UniqueID] as string == null) ? txtPrinPaid.Text : Request[txtPrinPaid.UniqueID] as string) > 0)
                //        {
                //            if (string.IsNullOrEmpty(vLoanAc) == true)
                //            {
                //                gblFuction.AjxMsgPopup("Loan Account Not Set In Loan Parameter");
                //                return;
                //            }
                //            i = i + 1;
                //            dr = dtAccount.NewRow();
                //            dr["DescId"] = vLoanAc;
                //            dr["DC"] = "C";
                //            dr["Amt"] = Convert.ToDecimal((Request[txtPrinPaid.UniqueID] as string == null) ? txtPrinPaid.Text : Request[txtPrinPaid.UniqueID] as string);
                //            dr["DtlId"] = i;
                //            dtAccount.Rows.Add(dr);
                //            dtAccount.AcceptChanges();
                //        }
                //        if ((vIntColl - vIntDue) > 0)
                //        {
                //            if (vIntDue > 0)
                //            {
                //                if (string.IsNullOrEmpty(vIntDueAC) == true)
                //                {
                //                    gblFuction.AjxMsgPopup("Interest Due Account Not Set In Loan Parameter");
                //                    return;
                //                }
                //                i = i + 1;
                //                dr = dtAccount.NewRow();
                //                dr["DescId"] = vIntDueAC;
                //                dr["DC"] = "C";
                //                dr["Amt"] = vIntDue;
                //                dr["DtlId"] = i;
                //                dtAccount.Rows.Add(dr);
                //                dtAccount.AcceptChanges();


                //                if (string.IsNullOrEmpty(vLoanIntAc) == true)
                //                {
                //                    gblFuction.AjxMsgPopup("Interest Account Not Set In Loan Parameter");
                //                    return;
                //                }
                //                i = i + 1;
                //                dr = dtAccount.NewRow();
                //                dr["DescId"] = vLoanIntAc;
                //                dr["DC"] = "C";
                //                dr["Amt"] = vIntColl - vIntDue;
                //                dr["DtlId"] = i;
                //                dtAccount.Rows.Add(dr);
                //                dtAccount.AcceptChanges();
                //            }
                //            else
                //            {
                //                if (vIntColl > 0)
                //                {
                //                    if (string.IsNullOrEmpty(vLoanIntAc) == true)
                //                    {
                //                        gblFuction.AjxMsgPopup("Interest Account Not Set In Loan Parameter");
                //                        return;
                //                    }
                //                    i = i + 1;
                //                    dr = dtAccount.NewRow();
                //                    dr["DescId"] = vLoanIntAc;
                //                    dr["DC"] = "C";
                //                    dr["Amt"] = vIntColl;
                //                    dr["DtlId"] = i;
                //                    dtAccount.Rows.Add(dr);
                //                    dtAccount.AcceptChanges();
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if (string.IsNullOrEmpty(vIntDueAC) == true)
                //            {
                //                gblFuction.AjxMsgPopup("Interest Due Account Not Set In Loan Parameter");
                //                return;
                //            }
                //            i = i + 1;
                //            dr = dtAccount.NewRow();
                //            dr["DescId"] = vIntDueAC;
                //            dr["DC"] = "C";
                //            dr["Amt"] = vIntColl;
                //            dr["DtlId"] = i;
                //            dtAccount.Rows.Add(dr);
                //            dtAccount.AcceptChanges();
                //        }
                //    }
                //    if (Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string) > 0)
                //    {
                //        if (vPenalAc == "")
                //        {
                //            gblFuction.AjxMsgPopup("Delay Payment AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPenalAc;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string) > 0)
                //    {
                //        if (vCGSTAC == "")
                //        {
                //            gblFuction.AjxMsgPopup("CGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vCGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string) > 0)
                //    {
                //        if (vSGSTAC == "")
                //        {
                //            gblFuction.AjxMsgPopup("SGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vSGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtWaveInt.UniqueID] as string == null) ? txtWaveInt.Text : Request[txtWaveInt.UniqueID] as string) > 0)
                //    {
                //        if (vIntWeaveAc == "" || vIntWeaveAc == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("Interest Waive AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vIntWeaveAc;
                //        dr["DC"] = "D";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtWaveInt.UniqueID] as string == null) ? txtWaveInt.Text : Request[txtWaveInt.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (vBounceAmt > 0)
                //    {
                //        if (vBounceChrgAC == "" || vBounceChrgAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("Bounce Charge AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vBounceChrgAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Math.Round(vBounceAmt, 0);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (vBounceCGST > 0)
                //    {
                //        if (vCGSTAC == "" || vCGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("CGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vCGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Math.Round(vBounceCGST, 0);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (vBounceSGST > 0)
                //    {
                //        if (vSGSTAC == "" || vSGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("SGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vSGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Math.Round(vBounceSGST, 0);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    //if (Convert.ToDecimal((Request[txtBounceWave.UniqueID] as string == null) ? txtBounceWave.Text : Request[txtBounceWave.UniqueID] as string) > 0)
                //    //{
                //    //    i = i + 1;
                //    //    dr = dtAccount.NewRow();
                //    //    dr["DescId"] = vBounceChrgWaveAC;
                //    //    dr["DC"] = "D";
                //    //    dr["Amt"] = Convert.ToDecimal((Request[txtBounceWave.UniqueID] as string == null) ? txtBounceWave.Text : Request[txtBounceWave.UniqueID] as string);
                //    //    dr["DtlId"] = i;
                //    //    dtAccount.Rows.Add(dr);
                //    //    dtAccount.AcceptChanges();
                //    //}
                //    if (Convert.ToDecimal((Request[txtPreCloseChrge.UniqueID] as string == null) ? txtPreCloseChrge.Text : Request[txtPreCloseChrge.UniqueID] as string) > 0)
                //    {
                //        if (vPreCloseChrgAC == "" || vPreCloseChrgAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("Please Set PreClose Charge AC in Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPreCloseChrgAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPreCloseChrge.UniqueID] as string == null) ? txtPreCloseChrge.Text : Request[txtPreCloseChrge.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtPreCloseWaived.UniqueID] as string == null) ? txtPreCloseWaived.Text : Request[txtPreCloseWaived.UniqueID] as string) > 0)
                //    {
                //        if (vPreCloseWaiveAC == "" || vPreCloseWaiveAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("PreClose Charge Waive AC Not Set in Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPreCloseWaiveAC;
                //        dr["DC"] = "D";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPreCloseWaived.UniqueID] as string == null) ? txtPreCloseWaived.Text : Request[txtPreCloseWaived.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtPreCloseCGST.UniqueID] as string == null) ? txtPreCloseCGST.Text : Request[txtPreCloseCGST.UniqueID] as string) > 0)
                //    {
                //        if (vPreCloseCGSTAC == "" || vPreCloseCGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("PreClose CGST AC Not Set in Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPreCloseCGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPreCloseCGST.UniqueID] as string == null) ? txtPreCloseCGST.Text : Request[txtPreCloseCGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtPreCloseSGST.UniqueID] as string == null) ? txtPreCloseSGST.Text : Request[txtPreCloseSGST.UniqueID] as string) > 0)
                //    {
                //        if (vPreCloseSGSTAC == "" || vPreCloseSGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("PreClose SGST AC Not Set in Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPreCloseSGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtPreCloseSGST.UniqueID] as string == null) ? txtPreCloseSGST.Text : Request[txtPreCloseSGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }

                //    if (Convert.ToDecimal((Request[txtVisitChrgeRec.UniqueID] as string == null) ? txtVisitChrgeRec.Text : Request[txtVisitChrgeRec.UniqueID] as string) > 0)
                //    {
                //        if (vVisitChrgeAC == "" || vVisitChrgeAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("Visiting Charge  AC Not Set in Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vVisitChrgeAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtVisitChrgeRec.UniqueID] as string == null) ? txtVisitChrgeRec.Text : Request[txtVisitChrgeRec.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtVisitCGST.UniqueID] as string == null) ? txtVisitCGST.Text : Request[txtVisitCGST.UniqueID] as string) > 0)
                //    {
                //        if (vCGSTAC == "" || vCGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("CGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vCGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtVisitCGST.UniqueID] as string == null) ? txtVisitCGST.Text : Request[txtVisitCGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (Convert.ToDecimal((Request[txtVisitSGST.UniqueID] as string == null) ? txtVisitSGST.Text : Request[txtVisitSGST.UniqueID] as string) > 0)
                //    {
                //        if (vSGSTAC == "" || vSGSTAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("SGST AC Not Set In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vSGSTAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = Convert.ToDecimal((Request[txtVisitSGST.UniqueID] as string == null) ? txtVisitSGST.Text : Request[txtVisitSGST.UniqueID] as string);
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (vExcessCharge > 0)
                //    {
                //        if (vExcessChargeAC == "" || vExcessChargeAC == "-1")
                //        {
                //            gblFuction.AjxMsgPopup("Please Set ExcessChargeAC  In Loan Parameter");
                //            return;
                //        }
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vExcessChargeAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = vExcessCharge;
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //    if (vForeCloseLetterCharge > 0)
                //    {
                //        i = i + 1;
                //        dr = dtAccount.NewRow();
                //        dr["DescId"] = vPreCloseChrgAC;
                //        dr["DC"] = "C";
                //        dr["Amt"] = vForeCloseLetterCharge;
                //        dr["DtlId"] = i;
                //        dtAccount.Rows.Add(dr);
                //        dtAccount.AcceptChanges();
                //    }
                //}

                //#endregion
                string vCustName = ddlCustName.SelectedItem.Text.ToString();

                //string vCustName = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedItem.Text : Request[ddlCustName.UniqueID] as string;
                vNarationL = "Being the Amt Collection of Loan Recovery for  " + vCustName + " and Loan No " + vLoanNo;
                //using (StringWriter oSW = new StringWriter())
                //{
                //    dtAccount.WriteXml(oSW);
                //    vXmlAC = oSW.ToString();
                //}

            

                oLR = new CLoanRecovery();
                vErr = oLR.InsertCollection(vLoanId, vAccDate, vCustId, vPrinColl, vIntColl, 
                    vPrevAdvance,vPenAmt,vPenWaiveAmt, vPenColl,vPenCGST,vPenSGST,vPenDue,
                    vBounceAmt,vBounnceWave,vBounceRecv, vBounceCGST, vBounceSGST, vBounnceDue, 
                    vVisitCharge,vVisitingWaive,vVisitingRec,vVisitCGST,vVisitSGST,vVisitingDue,
                    vNewAdvance,vTotColl,vPrinOS,
                    vActMstTbl, vActDtlTbl, vFinYear, vBranch, vUserId, vNarationL, "W", vBank);
                if (vErr == 0)
                {
                    //lblMsg.Text = gblPRATAM.SaveMsg;
                    
                    
                    // Trigger SMS For Recovery
                    DataTable dt_Sms = new DataTable();
                    CSMS oSms = null;
                    AuthSms oAuth = null;
                    string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                    oSms = new CSMS();
                    oAuth = new AuthSms();

                    // For Applicant  (LC----> Loan Collection)
                    //dt_Sms = oSms.Get_ToSend_SMS(vLoanId, vAccDate, "LC");
                    //if (dt_Sms.Rows.Count > 0)
                    //{
                    //    foreach (DataRow drSMS in dt_Sms.Rows)
                    //    {
                    //        if (drSMS["MobNo"].ToString().Length >= 10)
                    //        {
                    //            vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                    //            System.Threading.Thread.Sleep(500);
                    //            if (!string.IsNullOrEmpty(vRtnGuid))
                    //            {
                    //                vRtnGuid = vRtnGuid.Remove(0, 7);
                    //                if (vRtnGuid != "")
                    //                {
                    //                    vStatusDesc = "Message Delivered";
                    //                    vStatusCode = "Message Delivered Successfully";
                    //                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //                }
                    //                else
                    //                {
                    //                    vStatusDesc = "Unknown Error";
                    //                    vStatusCode = "Unknown Error";
                    //                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                vStatusDesc = "Unknown Error";
                    //                vStatusCode = "Unknown Error";
                    //                oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //            }
                    //        }
                    //    }
                    //}
                    btnSave.Enabled = false;
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                }
                else
                {
                    //lblMsg.Text = gblPRATAM.DBError;
                    // vResult = false;
                    gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                }
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oLR = null;
            }
        }
        private void TotCalculation(DataTable dt, string pgvName)
        {
            //    decimal TotPrin = 0, TotInst = 0, TotAmt = 0;
            //    if (pgvName == "gvRecvry")
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            TotPrin = TotPrin + Convert.ToDecimal(dr["PrincpalPaid"]);
            //            TotInst = TotInst + Convert.ToDecimal(dr["InterestPaid"]);
            //            TotAmt = TotAmt + Convert.ToDecimal(dr["Total"]);
            //        }
            //        txtTotPrin.Text = string.Format("{0:N}", TotPrin);
            //        txtTotInt.Text = string.Format("{0:N}", TotInst);
            //        txtTotal.Text = string.Format("{0:N}", TotAmt);
            //    }
            //    if (pgvName == "gvLed")
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            TotPrin = TotPrin + Convert.ToDecimal(dr["PrinCollAmt"]);
            //            TotInst = TotInst + Convert.ToDecimal(dr["IntCollAmt"]);
            //        }
            //        txtPrinPaidL.Text = string.Format("{0:N}", TotPrin);
            //        txtInstPaidL.Text = string.Format("{0:N}", TotInst);
            //        txtTotalPaidL.Text = string.Format("{0:N}", TotPrin + TotInst);
            //    }
            //}

            ///// <summary>
            ///// 
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            //protected void btnLdgr_Click(object sender, EventArgs e)
            //{
            //    DataTable dtDtl = null;
            //    CLoanRecovery oLR = null;
            //    string vLedTyp = "A";
            //    try
            //    {
            //        string vBrCode = Session[gblValue.BrnchCode].ToString();
            //        string pLoanID = Convert.ToString(Session["LoanId"]);
            //        oLR = new CLoanRecovery();
            //        dtDtl = oLR.GetCollectionDtlByLoanId(pLoanID, vLedTyp, vBrCode);
            //        gvLed.DataSource = dtDtl;
            //        gvLed.DataBind();
            //        dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
            //        Session["dtDtlRst"] = dtDtl;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //    finally
            //    {
            //        dtDtl = null;
            //        oLR = null;
            //    }
        }
        protected void gvLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            CLoanRecovery oLR = new CLoanRecovery();
            try
            {
                // string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vBrCode = ddlBranchName.SelectedValue.ToString();
                string vAcMst = Session[gblValue.ACVouMst].ToString();
                string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.FinYear].ToString();
                string vReffID = "", vCollMode = "";
                Int32 vErr = 0;

                if (e.CommandName == "cmdDelete")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnSlct");
                    string RowIndex = e.CommandArgument.ToString();
                    int totalrow = gvLed.Rows.Count;
                    if (Convert.ToInt32(RowIndex) != 0)
                    {
                        gblFuction.AjxMsgPopup("Only Last Collection Details can be deleted...");
                        return;
                    }
                    else
                    {
                        string vLoanId = gvLed.Rows[0].Cells[1].Text;
                        Int32 vSlNo = Convert.ToInt32(gvLed.Rows[0].Cells[17].Text);
                        vReffID = vLoanId + "-" + vSlNo;
                        DateTime vAccDate = gblFuction.setDate(gvLed.Rows[0].Cells[2].Text);
                        DateTime vLnDate = gblFuction.setDate(gvLed.Rows[0].Cells[19].Text);
                        vCollMode = gvLed.Rows[0].Cells[15].Text;
                        String FYear = GetCurrentFinancialYear(vAccDate);
                        if (vFinYear != FYear)
                        {
                            gblFuction.AjxMsgPopup("Collection Details can not be deleted as Collection Date is not in same Login  Financial Year...");
                            return;
                        }
                        if (vAccDate == vLnDate)
                        {
                            gblFuction.AjxMsgPopup("Collection Details can not be deleted as Collection Done during Loan Disbursement as Advance EMI...");
                            return;
                        }
                        vErr = oLR.DeleteCollection(vLoanId, vSlNo, vReffID, vAcMst, vAcDtl, vBrCode, this.UserID, vAccDate, vCollMode);
                        if (vErr == 0)
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
                            DataSet ds = new DataSet();
                            ds = oLR.GetCollectionDtlByLoanId(vLoanId);
                            dtDtl = ds.Tables[0];
                            if (dtDtl.Rows.Count > 0)
                            {
                                gvLed.DataSource = dtDtl;
                                gvLed.DataBind();
                            }
                            else
                            {
                                gvLed.DataSource = null;
                                gvLed.DataBind();
                            }
                            Session["dtDtlRst"] = dtDtl;
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }
        public static string GetCurrentFinancialYear(DateTime dt)
        {
            int CurrentYear = dt.Year;
            int PreviousYear = dt.Year - 1;
            int NextYear = dt.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (dt.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnLdgr_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = null;
            CLoanRecovery oLR = null;
            string vLedTyp = "A";
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string pCustId = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string;
                string pLoanID = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                oLR = new CLoanRecovery();
                DataSet ds = new DataSet();
                ds = oLR.GetCollectionDtlByLoanId(pLoanID);
                dtDtl = ds.Tables[0];
                //popLoanNo(pCustId);
                if (dtDtl.Rows.Count > 0)
                {
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                }
                else
                {
                    gvLed.DataSource = null;
                    gvLed.DataBind();
                }
                dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                Session["dtDtlRst"] = dtDtl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
                oLR = null;
            }
        }
        protected void btnForeCloseLtr_Click(object sender, EventArgs e)
        {
            string vLoanId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
            decimal vIntDue = 0, vPreClsChrg = 0, vPreClsCGST = 0, vPreClsSGST = 0, vBounceAmt = 0, vDelayPayment = 0, vVisitCharge = 0, vOtherCharge = 0, vPrinOS = 0;
            decimal vPenColl = 0, vPenCGST = 0, vPenSGST = 0, vVisitColl = 0, vVisitCGST = 0, vVisitSGST = 0;
            if (vLoanId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Loan No...");
                return;
            }
            else
            {
                vIntDue = Convert.ToDecimal((Request[txtIntPaid.UniqueID] as string == null) ? txtIntPaid.Text : Request[txtIntPaid.UniqueID] as string);
                vPreClsChrg = Convert.ToDecimal(txtPreCloseChrge.Text.Trim()) - Convert.ToDecimal(txtPreCloseWaived.Text.Trim());
                vPreClsCGST = Convert.ToDecimal(txtPreCloseCGST.Text.Trim());
                vPreClsSGST = Convert.ToDecimal(txtPreCloseSGST.Text.Trim());
                vBounceAmt = Convert.ToDecimal((Request[txtBounceRec.UniqueID] as string == null) ? txtBounceRec.Text : Request[txtBounceRec.UniqueID] as string);
                vPrinOS = Convert.ToDecimal((Request[txtPrinOS.UniqueID] as string == null) ? txtPrinOS.Text : Request[txtPrinOS.UniqueID] as string);
                // For Delay Payment
                if (((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string) != "")
                    vPenColl = Convert.ToDecimal((Request[txtPenCollAmt.UniqueID] as string == null) ? txtPenCollAmt.Text : Request[txtPenCollAmt.UniqueID] as string);
                if (((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string) != "")
                    vPenCGST = Convert.ToDecimal((Request[txtPenCGST.UniqueID] as string == null) ? txtPenCGST.Text : Request[txtPenCGST.UniqueID] as string);
                if (((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string) != "")
                    vPenSGST = Convert.ToDecimal((Request[txtPenSGST.UniqueID] as string == null) ? txtPenSGST.Text : Request[txtPenSGST.UniqueID] as string);
                vDelayPayment = vPenColl + vPenCGST + vPenSGST;
                // For Visiting Charge
                if (((Request[txtVisitChrgeRec.UniqueID] as string == null) ? txtVisitChrgeRec.Text : Request[txtVisitChrgeRec.UniqueID] as string) != "")
                    vVisitColl = Convert.ToDecimal((Request[txtVisitChrgeRec.UniqueID] as string == null) ? txtVisitChrgeRec.Text : Request[txtVisitChrgeRec.UniqueID] as string);
                if (((Request[txtVisitCGST.UniqueID] as string == null) ? txtVisitCGST.Text : Request[txtVisitCGST.UniqueID] as string) != "")
                    vVisitCGST = Convert.ToDecimal((Request[txtVisitCGST.UniqueID] as string == null) ? txtVisitCGST.Text : Request[txtVisitCGST.UniqueID] as string);
                if (((Request[txtVisitSGST.UniqueID] as string == null) ? txtVisitSGST.Text : Request[txtVisitSGST.UniqueID] as string) != "")
                    vVisitSGST = Convert.ToDecimal((Request[txtVisitSGST.UniqueID] as string == null) ? txtVisitSGST.Text : Request[txtVisitSGST.UniqueID] as string);
                vVisitCharge = vVisitColl + vVisitCGST + vVisitSGST;
                Decimal vForeClosureLetterCharge = 0,vExcessCharge=0;

                if (((Request[txtForeClsLettChrge.UniqueID] as string == null) ? txtForeClsLettChrge.Text : Request[txtForeClsLettChrge.UniqueID] as string) != "")
                    vForeClosureLetterCharge = Convert.ToDecimal((Request[txtForeClsLettChrge.UniqueID] as string == null) ? txtForeClsLettChrge.Text : Request[txtForeClsLettChrge.UniqueID] as string);

                if (((Request[txtExcessCharge.UniqueID] as string == null) ? txtExcessCharge.Text : Request[txtExcessCharge.UniqueID] as string) != "")
                    vExcessCharge = Convert.ToDecimal((Request[txtExcessCharge.UniqueID] as string == null) ? txtExcessCharge.Text : Request[txtExcessCharge.UniqueID] as string);


                DataTable dt = new DataTable();
                CLoanRecovery oLR = new CLoanRecovery();
                string pLastTransDate = (Request[txtLastTransDate.UniqueID] as string == null) ? txtLastTransDate.Text.Trim() : Request[txtLastTransDate.UniqueID] as string;
                if (pLastTransDate == "")
                {
                    gblFuction.AjxMsgPopup("Last Transaction Date Can Not Be Blank..");
                    return;
                }
                DateTime RecoveryDate = gblFuction.setDate(txtRecovryDt.Text.Trim());
                DateTime LastCollDate = gblFuction.setDate(pLastTransDate);
                dt = oLR.RptForeCloseLtr(vLoanId, RecoveryDate, LastCollDate, vPreClsChrg, vPreClsCGST, vPreClsSGST,
                    vBounceAmt, vDelayPayment, vVisitCharge, vOtherCharge, vPrinOS, vIntDue, vForeClosureLetterCharge, vExcessCharge);
                if (dt.Rows.Count > 0)
                {
                    Session["ForeCloseLtr"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('../Report/RptForeCloseLtr.aspx','Foreclose Letter','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Loan...");
                    return;
                }

            }
        }
    }
}