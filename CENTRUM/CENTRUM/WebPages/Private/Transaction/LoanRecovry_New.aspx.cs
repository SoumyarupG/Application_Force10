using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LoanRecovry_New : CENTRUMBase
    {
        public decimal MyTotColl;
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
                txtTotPaid.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                //PopBranch(Session[gblValue.UserName].ToString());
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                //    ddlBranch.Enabled = false;
                //}
                //else
                //{
                //    ddlBranch.Enabled = true;
                //}
                //popRO();
                PopBank();
                hdUserID.Value = Session[gblValue.UserId].ToString();
                PopLedger();
                PopWaveofReason();
                ClearControls();
                ViewState["StateEdit"] = null;
                txtRecovryDt.Text = (string)Session[gblValue.LoginDate];
                txtValueDt.Text = (string)Session[gblValue.LoginDate];
                hdnValueDt.Value = (string)Session[gblValue.LoginDate];
                btnDone.Enabled = true;
                btnDel.Enabled = false;
                btnReverse.Enabled = false;
                btnShow.Enabled = true;
                gvRecvry.Enabled = true;
                ddlBank.Enabled = false;
                ddlLedger.Enabled = false;
                txtAdvDate.Enabled = false;
                txtToDt.Enabled = false;
                ViewState["AllowAdvYN"] = Convert.ToString(Session[gblValue.AllowAdvYN]);

                /*
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 2 || Convert.ToInt32(Session[gblValue.RoleId]) == 3 || Convert.ToInt32(Session[gblValue.RoleId]) == 6)
                {
                    chkPreClose.Checked = false;
                    chkPreClose.Enabled = false;
                }*/
                hdPreClose.Value = Convert.ToString(Session[gblValue.PrematureColl]);
                hdDemise.Value = Convert.ToString(Session[gblValue.Demise]);

                //if (Convert.ToString(Session[gblValue.PrematureColl]) == "Y")
                //{

                //    chkPreClose.Enabled = true;
                //}
                //else
                //{
                //    chkPreClose.Checked = false;
                //    chkPreClose.Enabled = false;
                //}
                //if (Convert.ToString(Session[gblValue.Demise]) == "N")
                //{
                //    chkDeathClose.Checked = false;
                //    chkDeathClose.Enabled = false;
                //}
                //else
                //{
                //    chkDeathClose.Enabled = true;
                //}

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        //private void PopBranch(string pUser)
        //{
        //    DataTable dt = null;
        //    CUser oUsr = null;
        //    try
        //    {
        //        oUsr = new CUser();
        //        dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlBranch.DataSource = dt;
        //            ddlBranch.DataTextField = "BranchName";
        //            ddlBranch.DataValueField = "BranchCode";
        //            ddlBranch.DataBind();
        //            ListItem liSel = new ListItem("<--- Select --->", "-1");
        //            ddlBranch.Items.Insert(0, liSel);
        //        }
        //        else
        //        {
        //            ListItem liSel = new ListItem("<--- Select --->", "-1");
        //            ddlBranch.Items.Insert(0, liSel);
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oUsr = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Installment Collection";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanRecovery);
                if (this.RoleId == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    btnDone.Visible = false;
                    btnDel.Visible = false;
                    btnReverse.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = false;
                    btnReverse.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = false;
                    btnReverse.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    btnDone.Visible = true;
                    btnDel.Visible = true;
                    btnReverse.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Recovery", false);
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
        private void ClearControls()
        {
            rdbLoan.SelectedValue = "O";
            //rdbColl.SelectedValue = "G";
            rdbRecvry.SelectedValue = "N";
            txtLnAmt.Text = "0.00";
            txtLnDt.Text = "";
            txtReceiptNo.Text = "";
            txtDeathDt.ReadOnly = true;
            txtDeathDt.Text = "";
            txtIntRate.Text = "0.00";
            txtPrinOS.Text = "0.00";
            txtIntOs.Text = "0.00";
            txtPrinPay.Text = "0.00";
            txtIntPay.Text = "0.00";
            txtTotAmtPay.Text = "0.00";
            txtPrinPaid.Text = "0.00";
            txtIntPaid.Text = "0.00";
            txtTotPaid.Text = "0.00";
            txtTotPrin.Text = "0.00";
            txtTotInt.Text = "0.00";
            txtTotal.Text = "0.00";
            txtPrinPaidL.Text = "0.00";
            txtInstPaidL.Text = "0.00";
            txtAdvPaid.Text = "0.00";
            txtAdvPay.Text = "0.00";
            txtExcessAmt.Text = "0.00";
            txtExcessAdv.Text = "0.00";
            ddlCo.SelectedIndex = -1;
            ddlGroup.Items.Clear();
            ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
            ddlLedger.Enabled = false;
            ddlWvReason.SelectedIndex = -1;
            ddlWvReason.Enabled = false;
            ddlDPerson.SelectedIndex = -1;
            ddlDPerson.Enabled = false;
            ddlDeathType.Enabled = false;
            ddlReason.SelectedIndex = -1;
            ddlAction.SelectedIndex = -1;
            ddlReason.Enabled = false;
            ddlAction.Enabled = false;
            gvRecvry.DataSource = null;
            gvRecvry.DataBind();
            gvLed.DataSource = null;
            gvLed.DataBind();
            cbAdvColl.Checked = false;
            chkRoutin.Checked = true; ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rdbRecvry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbRecvry.SelectedValue == "N")
            {
                btnDel.Enabled = false;
                btnReverse.Enabled = false;
                btnSave.Enabled = true;
                btnShow.Enabled = true;
                btnDone.Enabled = true;
                gvRecvry.Enabled = true;
                txtTotPaid.Enabled = true;
                ClearControls();
            }
            else if (rdbRecvry.SelectedValue == "M")
            {
                btnDel.Enabled = true;
                btnReverse.Enabled = true;
                btnSave.Enabled = false;
                btnShow.Enabled = false;
                btnDone.Enabled = false;
                gvRecvry.Enabled = true;
                txtTotPaid.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopLedger()
        {
            DataTable dt = null;
            string vBrCode;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{

            //}
            //else
            //{
            //    vBrCode = ddlBranch.SelectedValue.ToString();
            //}
            CVoucher oVou = new CVoucher();
            dt = oVou.GetAcGenLedCB(vBrCode, "S", "");
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescID";
            ddlLedger.DataSource = dt;
            ddlLedger.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlLedger.Items.Insert(0, oItem);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopWaveofReason()
        {
            Int32 vRows = 0;
            DataTable dt = null;
            CWaveOff oWv = new CWaveOff();
            dt = oWv.GetWavePG(1, ref vRows);
            ddlWvReason.DataTextField = "LoanWaveReason";
            ddlWvReason.DataValueField = "LoanWaveoffId";
            ddlWvReason.DataSource = dt;
            ddlWvReason.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlWvReason.Items.Insert(0, oItem);
        }

        /// <summary>
        /// 
        /// </summary>
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{

            //}
            //else
            //{
            //    vBrCode = ddlBranch.SelectedValue.ToString();
            //}

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlBranch.SelectedIndex > 0) popRO();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        //protected void chkDeathClose_CheckedChanged(object sender, EventArgs e)
        //{
        //    DateTime vCollDt = gblFuction.setDate(txtRecovryDt.Text.ToString());
        //    PopInsLedger(vCollDt);
        //}

        private void PopInsLedger(DateTime vCollDt)
        {
            DataTable dt = null;
            CDisburse oDbr = new CDisburse();
            dt = oDbr.GetInSuranceCompany(Session[gblValue.BrnchCode].ToString(), vCollDt);
            ViewState["Insurance"] = dt;
            ddlLedger.Items.Clear();
            ddlLedger.DataSource = dt;
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescId";
            ddlLedger.DataBind();
            chkJurnl.Checked = false;
        }

        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCOID"></param>
        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CLoanRecovery oCL = null;
            try
            {
                ddlGroup.Items.Clear();
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                oCL = new CLoanRecovery();
                dtGr = oCL.PopCenterWithCollDay(vCOID, gblFuction.setDate(txtRecovryDt.Text), vBrCode, "W"); //With CollDay
                dtGr.AcceptChanges();
                ddlGroup.DataSource = dtGr;
                ddlGroup.DataTextField = "Market";
                ddlGroup.DataValueField = "MarketID";
                ddlGroup.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oCL = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            String vLnStatus = "", vGroupID = "-1", vMktID = "-1", vEoId = "-1", vLoanId = "", vMemberID = "-1", vIsHolidayColl = "N";
            Int32 vRoutine = 0;
            DataTable dt = null;
            CLoanRecovery oLR = null;
            ViewState["dtRst"] = null;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            //Clear();
            if (txtLoanNo.Text == "")
            {
                if (Request[ddlCo.UniqueID] as string == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select LO..");
                    return;
                }
                if (Request[ddlCenter.UniqueID] as string == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Center..");
                    return;
                }
            }
            if (gblFuction.setDate(txtRecovryDt.Text) < vFinFromDt || gblFuction.setDate(txtRecovryDt.Text) > vFinToDt)
            {
                gblFuction.AjxMsgPopup("Recovery Date should login financial year.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                return;
            }


            if (cbAdvColl.Checked == true)
            {
                vIsHolidayColl = "Y";
                if (txtAdvDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Holiday Collection From Date should not be blank.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtAdvDate");
                    return;
                }
                if (txtToDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Holiday Collection To Date should not be blank.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtToDt");
                    return;
                }
                if ((gblFuction.setDate(txtToDt.Text) - gblFuction.setDate(txtAdvDate.Text)).TotalDays > 7)
                {
                    gblFuction.AjxMsgPopup("For Holiday Collection  Date Difference between from date and to date Can not be more than 7 days.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtToDt");
                    return;
                }
            }
            try
            {
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //    ddlBranch.Enabled = false;
                //}
                DateTime vRecvDt = gblFuction.setDate(txtRecovryDt.Text);
                //ClearControls();

                if (chkRoutin.Checked == true)
                    vRoutine = 1;
                else
                    vRoutine = 0;

                if (rdbLoan.SelectedValue == "O")
                    vLnStatus = "O";
                else if (rdbLoan.SelectedValue == "C")
                    vLnStatus = "C";

                vEoId = Request[ddlCo.UniqueID] as string;  //CM ID

                if (Request[ddlCenter.UniqueID] as string == "-1") //Center ID
                    vMktID = "-1";

                else
                    vMktID = Request[ddlCenter.UniqueID] as string;

                if (Request[ddlGroup.UniqueID] as string == "-1") //Group ID
                    vGroupID = "-1";

                else
                    vGroupID = Request[ddlGroup.UniqueID] as string;

                if (Request[ddlMember.UniqueID] as string != null)
                {
                    vMemberID = Request[ddlMember.UniqueID] as string;
                }

                DateTime vFromDt, vToDate;

                vFromDt = gblFuction.setDate(txtAdvDate.Text);
                vToDate = gblFuction.setDate(txtToDt.Text);

                oLR = new CLoanRecovery();

                if (txtLoanNo.Text != "")
                {
                    DataTable dt1 = null;

                    dt1 = oLR.PopMemberDetailByOldMigCode(txtLoanNo.Text);

                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            vMemberID = dr["MemberId"].ToString();
                            vEoId = dr["EOID"].ToString();
                            vMktID = dr["MarketID"].ToString();
                            vGroupID = dr["Groupid"].ToString();
                            //vLoanType = dr["MemberType"].ToString();
                        }
                    }
                }
                dt = oLR.GetAllLoanCollection(vLnStatus, vRoutine, vRecvDt, vEoId, vMktID, vGroupID, vBrCode, vMemberID, vFromDt, vToDate, vIsHolidayColl,txtLoanNo.Text);
                dt.PrimaryKey = new DataColumn[] { dt.Columns["LoanID"] };
                ViewState["dtRst"] = dt;
                gvRecvry.DataSource = dt;
                gvRecvry.DataBind();
                TotCalculation(dt, "gvRecvry");
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 2 || Convert.ToInt32(Session[gblValue.RoleId]) == 3 || Convert.ToInt32(Session[gblValue.RoleId]) == 6)
                //{
                //    chkPreClose.Enabled = false;
                //}
                //else
                //{
                //    chkPreClose.Enabled = true;
                //}

                if (Convert.ToString(Session[gblValue.PrematureColl]) == "Y")
                {
                    chkPreClose.Enabled = true;
                }
                else
                {

                    chkPreClose.Enabled = false;
                }
                chkPreClose.Checked = false;

                btnSave.Enabled = true;
                if (dt.Rows.Count > 0)
                {
                    vLoanId = Convert.ToString(dt.Rows[0]["LoanID"]);
                    GetDetails(vLoanId, vRecvDt, vBrCode, vFromDt, vToDate, vIsHolidayColl);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanId"></param>
        /// <param name="pRecvDt"></param>
        /// <param name="pBrCode"></param>
        protected void GetDetails(string pLoanId, DateTime pRecvDt, string pBrCode, DateTime vHolidayFrmDt, DateTime vHolidayToDt, string vIsHolidayColl)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            CLoanRecovery oLR = null;
            decimal vTotDue = 0, vTotPaid = 0;
            oLR = new CLoanRecovery();
            ViewState["LoanId"] = pLoanId;
            dt = oLR.GetCollectionByLoanId(pLoanId, pRecvDt, pBrCode, vHolidayFrmDt, vHolidayToDt, vIsHolidayColl);
            if (dt.Rows.Count > 0)
            {
                lblGrp.Text = Convert.ToString(dt.Rows[0]["GroupName"]);
                lblMemNo.Text = Convert.ToString(dt.Rows[0]["MemberNo"]) + " - " + Convert.ToString(dt.Rows[0]["MemberName"]);
                lblFund.Text = Convert.ToString(dt.Rows[0]["FundSource"]);
                lblLnScme.Text = Convert.ToString(dt.Rows[0]["LoanType"]);
                txtLnDt.Text = Convert.ToString(dt.Rows[0]["LoanDt"]);
                txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]);
                txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                txtPrinOS.Text = Convert.ToString(dt.Rows[0]["PrncOutStd"]);
                txtIntOs.Text = Convert.ToString(dt.Rows[0]["IntOutStd"]);
                txtPrinPay.Text = string.Format("{0:N}", dt.Rows[0]["PrincpalDue"]);
                txtIntPay.Text = string.Format("{0:N}", dt.Rows[0]["InterestDue"]);
                txtAdvPay.Text = string.Format("{0:N}", dt.Rows[0]["AdvanceAmt"]);
                vTotDue = Math.Round((Convert.ToDecimal(dt.Rows[0]["PrincpalDue"]) + Convert.ToDecimal(dt.Rows[0]["InterestDue"])) - Convert.ToDecimal(dt.Rows[0]["AdvanceAmt"]), 2);
                txtTotAmtPay.Text = string.Format("{0:N}", dt.Rows[0]["Total"]);
                txtPrinPaid.Text = "0.00";
                txtIntPaid.Text = "0.00";
                vTotPaid = Convert.ToDecimal(dt.Rows[0]["PaidPric"]) + Convert.ToDecimal(dt.Rows[0]["PaidInt"]);
                txtTotPaid.Text = "0.00";
                dtDtl = oLR.GetCollectionDtlByLoanId(pLoanId, "M", pBrCode);
                if (dtDtl.Rows.Count > 0)
                {
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    TotCalculation(dtDtl, "gvLed");
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                }
                else
                {
                    gvLed.DataSource = null;
                    gvLed.DataBind();
                }

                //fillLnDtl(dt.Rows[0]["MemberNo"].ToString(), pRecvDt);
            }
            else
            {
                txtLnDt.Text = "";
                txtIntRate.Text = "0.00";
                txtLnAmt.Text = "0.00";
                txtPrinOS.Text = "0.00";
                txtIntOs.Text = "0.00";
                txtPrinPay.Text = "0.00";
                txtIntPay.Text = "0.00";
                txtTotAmtPay.Text = "0.00";
                txtPrinPaid.Text = "0.00";
                txtIntPaid.Text = "0.00";
                txtTotPaid.Text = "0.00";
            }
        }


        public void fillLnDtl(string Vmember, DateTime vAppDt)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CApplication oCG = null;

            try
            {
                oCG = new CApplication();
                ds = oCG.GetLoanDtlByMember(Vmember, vAppDt);
                dt = ds.Tables[0];
                //dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    txtspnm.Text = dt.Rows[0]["CoBrwrName"].ToString();         //Kushal
                }
                else
                {
                    txtspnm.Text = "";
                }
                //gvlndtl.DataSource = dt1;
                //gvlndtl.DataBind();
            }
            finally
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRecvry_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vLoanID = "", vHoliColl = "N";
            DateTime vCollDt, vFrmDt, vToDt;
            try
            {
                chkPreClose.Checked = false;
                chkJurnl.Checked = false;
                chkDeathClose.Checked = false;
                chkAdv.Checked = false;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 2 || Convert.ToInt32(Session[gblValue.RoleId]) == 3 || Convert.ToInt32(Session[gblValue.RoleId]) == 6)
                //{
                //    chkPreClose.Enabled = false;
                //}
                //else
                //{
                //    chkPreClose.Enabled = true;
                //}
                if (Convert.ToString(Session[gblValue.PrematureColl]) == "Y")
                {

                    chkPreClose.Enabled = true;
                }
                else
                {

                    chkPreClose.Enabled = false;
                }
                chkJurnl.Enabled = true;
                chkDeathClose.Enabled = true;
                chkAdv.Enabled = true;
                ddlAction.SelectedIndex = -1;
                ddlReason.SelectedIndex = -1;

                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                if (e.CommandName == "cmdCollect")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("lbtnShow");
                    foreach (GridViewRow gr in gvRecvry.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("lbtnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    vLoanID = Convert.ToString(e.CommandArgument);
                    vCollDt = gblFuction.setDate(txtRecovryDt.Text);
                    vFrmDt = gblFuction.setDate(txtAdvDate.Text);
                    vToDt = gblFuction.setDate(txtToDt.Text);
                    if (cbAdvColl.Checked == true)
                    {
                        vHoliColl = "Y";
                    }
                    GetDetails(vLoanID, vCollDt, vBrCode, vFrmDt, vToDt, vHoliColl);
                }

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
        protected void txtTotPaid_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLoanRecovery oLR = null;
            decimal vBalAmt = 0, vPrinAmt = 0, vIntAmt = 0;
            decimal vPTot = 0, vITot = 0, vActIntAmt = 0;
            try
            {
                if (Convert.ToDouble(txtTotPaid.Text) < Convert.ToDouble(txtTotAmtPay.Text) && txtTotPaid.Text != "")
                {
                    ddlReason.Enabled = true;
                    ddlAction.Enabled = true;
                    ddlReason.SelectedIndex = -1;
                    ddlAction.SelectedIndex = -1;
                }
                else
                {
                    ddlReason.Enabled = false;
                    ddlAction.Enabled = false;
                    ddlReason.SelectedIndex = -1;
                    ddlAction.SelectedIndex = -1;
                }
                if (ViewState["LoanId"] == null) return;

                string vLoanId = Convert.ToString(ViewState["LoanId"]);
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    
                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                decimal vTotPOS = Convert.ToDecimal(txtPrinOS.Text);
                decimal vTotIOS = Convert.ToDecimal(txtIntOs.Text);
                decimal vTotOS = vTotPOS + vTotIOS;
                if (chkPreClose.Checked == false)
                {
                    if (Convert.ToDouble(txtTotPaid.Text) > Convert.ToDouble(txtTotAmtPay.Text) && txtTotPaid.Text != "")
                    {
                        oLR = new CLoanRecovery();
                        dt = oLR.ChkAllowAdv(vLoanId);
                        if (dt.Rows.Count > 0)
                        {
                            if (Convert.ToString(dt.Rows[0]["AllowAdv"]) == "N")
                            {
                                if (chkAdv.Checked == false && chkDeathClose.Checked == false)
                                {
                                    gblFuction.AjxMsgPopup("Paid Amount cannot be greater than payable amount");
                                    txtTotPaid.Text = txtTotAmtPay.Text;
                                    txtPrinPaid.Text = txtPrinPay.Text;
                                    txtIntPaid.Text = txtIntPaid.Text;
                                    return;
                                }
                            }
                        }
                    }

                    oLR = new CLoanRecovery();
                    dt = oLR.GetInstallAllocation(vLoanId, vBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        vPTot = Convert.ToDecimal(dt.Compute("Sum(PrinceAmt)", ""));
                        vITot = Convert.ToDecimal(dt.Compute("Sum(InstAmt)", ""));

                        if (vPTot > vTotPOS)
                            dt.Rows[0]["PrinceAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) - (vPTot - vTotPOS);
                        if (vITot > vTotIOS)
                        {
                            vActIntAmt = Convert.ToDecimal(dt.Rows[0]["InstAmt"]) - Math.Round((vITot - vTotIOS), 2);
                            if (vActIntAmt < 0)
                            {
                                dt.Rows[0]["InstAmt"] = 0;
                            }
                            else
                            {
                                dt.Rows[0]["InstAmt"] = vActIntAmt;
                            }
                        }
                        dt.Rows[0]["TotAmt"] = Convert.ToDecimal(dt.Rows[0]["PrinceAmt"]) + Convert.ToDecimal(dt.Rows[0]["InstAmt"]);

                        //txtBankPaid.Text = string.Format("{0:N}", txtBankChrgPay.Text);
                        vBalAmt = Convert.ToDecimal(txtTotPaid.Text);// -Convert.ToDecimal(txtBankChrgPay.Text);

                        if (vBalAmt > vTotOS)           // For Invalid Paid Total Value as Paid Total can not be > then Total OS
                        {
                            gblFuction.AjxMsgPopup("Total paid Amount should less than equal to total Principal and Interest OS..");
                            return;
                        }
                        if (vTotOS == vBalAmt)      // For  Paid Total =  Total OS
                        {
                            txtPrinPaid.Text = txtPrinPay.Text;
                            txtIntPaid.Text = txtIntPay.Text;
                        }
                        else if (vBalAmt < vTotOS) //Total Amt <=Total OS
                        {
                            if (Convert.ToDecimal(txtTotAmtPay.Text) < Convert.ToDecimal(txtTotPaid.Text) && Convert.ToDecimal(txtTotAmtPay.Text) != 0)
                            {
                                // For Extra Payment i.e. Paid Amount >  Demand Amount
                                vIntAmt += Convert.ToDecimal(txtIntPay.Text);
                                vPrinAmt += Convert.ToDecimal(txtTotPaid.Text) - vIntAmt;// -Convert.ToDecimal(txtBankChrgPay.Text);
                            }
                            else
                            {
                                // For Part Payment and Full Payment
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (vBalAmt >= Convert.ToDecimal(dr["TotAmt"].ToString()))
                                    {
                                        vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
                                        vPrinAmt += Convert.ToDecimal(dr["PrinceAmt"].ToString());
                                        vBalAmt = vBalAmt - (Convert.ToDecimal(dr["PrinceAmt"].ToString()) + Convert.ToDecimal(dr["InstAmt"].ToString()));
                                    }
                                    else if (vBalAmt > 0)
                                    {
                                        if (vBalAmt > Convert.ToDecimal(dr["InstAmt"].ToString()))
                                        {
                                            vIntAmt += Convert.ToDecimal(dr["InstAmt"].ToString());
                                            vBalAmt = vBalAmt - Convert.ToDecimal(dr["InstAmt"].ToString());
                                            vPrinAmt += vBalAmt;
                                            vBalAmt = 0;
                                        }
                                        else
                                        {
                                            vIntAmt += vBalAmt;
                                            vBalAmt = 0;
                                        }
                                    }
                                }
                            }
                            txtPrinPaid.Text = string.Format("{0:N}", vPrinAmt);
                            txtIntPaid.Text = string.Format("{0:N}", vIntAmt);
                        }
                    }
                    else
                    {
                        if (txtTotAmtPay.Text == txtTotPaid.Text)
                        {
                            txtPrinPaid.Text = string.Format("{0:N}", txtPrinPay.Text);
                            txtIntPaid.Text = string.Format("{0:N}", txtIntPay.Text);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Invalied Collection Amount.");
                            gblFuction.focus("ctl00_cph_Main_txtPDTotAmt");
                            return;
                        }
                    }
                    txtTotPaid.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlPyn_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 vRow = 0;
            try
            {
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    
                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                DropDownList ddlPA = (DropDownList)sender;
                GridViewRow gvRow = (GridViewRow)ddlPA.NamingContainer;
                vRow = gvRow.RowIndex;
                DropDownList ddlPAList = (DropDownList)gvRow.FindControl("ddlPyn");
                dt = (DataTable)ViewState["dtRst"];
                if (ddlPAList.SelectedValue == "N")
                    dt.Rows[vRow]["PA"] = "N";

                ViewState["dtRst"] = dt;
            }
            catch { }
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
        protected void btnDone_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            //txtTotPaid.Text = "0";
            //txtTotAmtPay.Text = "0";
            Int32 vRow = 0;
            try
            {
                string vBranch;
                vBranch = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBranch = ddlBranch.SelectedValue.ToString();
                //}
                decimal vTotalPaidAmt = Convert.ToDecimal(txtTotPaid.Text);
                decimal vPayTotalAmt = Convert.ToDecimal(txtTotAmtPay.Text);
                DateTime vCollDt = gblFuction.setDate(txtRecovryDt.Text);
                if (ViewState["LoanId"] == null)
                {
                    gblFuction.AjxMsgPopup("Please Select Record before process.");
                    return;
                }
                if (Convert.ToDouble(txtTotPaid.Text) < Convert.ToDouble(txtTotAmtPay.Text) && txtTotPaid.Text != "")
                {
                    if (ddlReason.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Please Select Reason.");
                        return;
                    }
                    if (ddlAction.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Please Select Action Taken");
                        return;
                    }
                }
                string vLoanID = Convert.ToString(ViewState["LoanId"]);
                dt = (DataTable)ViewState["dtRst"];
                vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));

                //dt.Rows[vRow]["BankChDue"] = txtBankChrgPay.Text;
                dt.Rows[vRow]["PrincpalPaid"] = txtPrinPaid.Text;
                dt.Rows[vRow]["InterestPaid"] = txtIntPaid.Text;
                //if (chkDeathClose.Checked==true)
                //    dt.Rows[vRow]["DeathDt"] = txtRecovryDt.Text;
                //dt.Rows[vRow]["BankChPaid"] = txtBankPaid.Text;
                if (ddlReason.SelectedIndex > 0)
                    dt.Rows[vRow]["Reason"] = ddlReason.SelectedItem.Text;
                if (ddlAction.SelectedIndex > 0)
                    dt.Rows[vRow]["ActionTaken"] = ddlAction.SelectedItem.Text;

                dt.Rows[vRow]["DescID"] = ddlLedger.SelectedValue;

                dt.Rows[vRow]["Total"] = Convert.ToDecimal(txtPrinPaid.Text) + Convert.ToDecimal(txtIntPaid.Text);// +Convert.ToDecimal(txtBankPaid.Text);
                if (chkPreClose.Checked == true)
                {
                    dt.Rows[vRow]["ClosingType"] = "P";
                    dt.Rows[vRow]["PrincpalDue"] = txtPrinPaid.Text;
                }

                if (chkJurnl.Checked == true)
                {
                    dt.Rows[vRow]["CollType"] = "J";
                    if (ddlLedger.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Please Select ledger Account");
                        return;

                    }
                }

                if (chkDeathClose.Checked == true)
                {
                    if (ddlWvReason.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Please Select Wave Off Reason.");
                        return;
                    }

                    if (ddlDPerson.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Specify Who Died.");
                        return;
                    }

                    if (txtDeathDt.Text == "" || gblFuction.IsDate(txtDeathDt.Text) == false)
                    {
                        gblFuction.AjxMsgPopup("Please Select Death Date");
                        return;
                    }

                    dt.Rows[vRow]["ClosingType"] = "D";
                    dt.Rows[vRow]["CollType"] = "J";
                    dt.Rows[vRow]["PrincpalDue"] = txtPrinPaid.Text;
                    dt.Rows[vRow]["DeathDt"] = gblFuction.setDate(txtDeathDt.Text);
                    dt.Rows[vRow]["LWaveOffId"] = Convert.ToInt32(ddlWvReason.SelectedValue);
                    dt.Rows[vRow]["DPerson"] = ddlDPerson.SelectedValue;
                    dt.Rows[vRow]["DeathType"] = ddlDeathType.SelectedValue;
                }


                dt.AcceptChanges();
                gvRecvry.DataSource = dt;
                gvRecvry.DataBind();
                //ViewState["dtRst"]=dt ;             // chnged by dayit
                TotCalculation(dt, "gvRecvry");
                chkPreClose.Checked = false;
                txtPrinPay.Text = "0";
                txtPrinPaid.Text = "0";
                txtIntPay.Text = "0";
                txtIntPaid.Text = "0";
                txtTotAmtPay.Text = "0";
                txtTotPaid.Text = "0";
                ddlAction.SelectedIndex = -1;
                ddlReason.SelectedIndex = -1;
                ddlWvReason.SelectedIndex = -1;
                //ddlDPerson.SelectedIndex = -1;
                txtDeathDt.Text = "";
            }
            catch (Exception ex)
            {
                throw ex;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dt = null, dtLoan = null, dtdesc = null;
            CLoanRecovery oLR = null;
            string sXml = "", vNaration = "", vCollMode = "C", vEoID = "", vRetMsg = "";
            Int32 vErr = 0, vUserId = 0;

            string vModeAC = "C0001";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            decimal TotAmt = 0, vTotFrRecNo = 0;
            try
            {

                DateTime pAccDate = gblFuction.setDate(txtRecovryDt.Text);
                string pBranch;
                pBranch = Session[gblValue.BrnchCode].ToString();

                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                vNaration = "Being the Amt of Loan Collection from ";
                vEoID = ddlCo.SelectedValue;

                if (rdbCashBank.SelectedValue == "C")
                {
                    vModeAC = "C0001";
                    vCollMode = "C";
                }
                else if (rdbCashBank.SelectedValue == "B")
                {
                    vModeAC = ddlBank.SelectedValue;
                    vCollMode = "B";
                }

                dt = (DataTable)ViewState["dtRst"];
                dtLoan = dt.Clone();
                foreach (GridViewRow gr in gvRecvry.Rows)
                {
                    HiddenField hdProvDeath = (HiddenField)gr.FindControl("hdProvDeath");
                    HiddenField hdDeathType = (HiddenField)gr.FindControl("hdDeathType");
                    HiddenField lblPrnc4 = (HiddenField)gr.FindControl("hdtxtPrnc4");
                    DropDownList ddlPyn = (DropDownList)gr.FindControl("ddlPyn");
                    Label txtMem = (Label)gr.FindControl("txtMem");
                    if (hdDeathType.Value == "" && hdProvDeath.Value == "Y" && Convert.ToDouble(lblPrnc4.Value) > 0)
                    {
                        gblFuction.AjxMsgPopup(txtMem.Text + " has been declare as a provisional death so you can not take Collection.");
                        return;
                    }
                    if (ddlPyn.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please select attendance for Member -" + txtMem.Text);
                        return;
                    }
                }

                foreach (GridViewRow gr in gvRecvry.Rows)
                {
                    DataRow dr = dtLoan.NewRow();
                    dr["LoanId"] = 0;
                    dtLoan.Rows.Add(dr);
                    dtLoan.Rows[gr.RowIndex]["LoanId"] = gr.Cells[10].Text == "&nbsp;" ? "" : gr.Cells[10].Text;
                    dtLoan.Rows[gr.RowIndex]["MemberId"] = gr.Cells[16].Text;
                    dtLoan.Rows[gr.RowIndex]["SlNo"] = gr.Cells[17].Text;
                    dtLoan.Rows[gr.RowIndex]["PrincOS"] = gr.Cells[18].Text;
                    dtLoan.Rows[gr.RowIndex]["PrincpalDue"] = "0";
                    dtLoan.Rows[gr.RowIndex]["InterestDue"] = "0";
                    dtLoan.Rows[gr.RowIndex]["TotalDue"] = "0";
                    if (gr.Cells[10].Text == "&nbsp;" || gr.Cells[10].Text == "")
                    {
                        dtLoan.Rows[gr.RowIndex]["PrincpalPaid"] = "0";
                        dtLoan.Rows[gr.RowIndex]["InterestPaid"] = "0";
                        dtLoan.Rows[gr.RowIndex]["Total"] = "0";
                        dtLoan.Rows[gr.RowIndex]["Noofinst"] = "0";
                        dtLoan.Rows[gr.RowIndex]["ClosingType"] = "N";
                    }
                    else
                    {
                        HiddenField lbltxtPrnc1 = (HiddenField)gr.FindControl("hdtxtPrnc1");
                        dtLoan.Rows[gr.RowIndex]["PrincpalPaid"] = lbltxtPrnc1.Value == "" ? "0" : lbltxtPrnc1.Value;
                        HiddenField lbltxtPrnc2 = (HiddenField)gr.FindControl("hdtxtPrnc2");
                        dtLoan.Rows[gr.RowIndex]["InterestPaid"] = lbltxtPrnc2.Value == "" ? "0" : lbltxtPrnc2.Value;

                        HiddenField lblPrnc4 = (HiddenField)gr.FindControl("hdtxtPrnc4");
                        dtLoan.Rows[gr.RowIndex]["Total"] = lblPrnc4.Value == "" ? "0" : lblPrnc4.Value;
                        HiddenField hdNoofinst = (HiddenField)gr.FindControl("hdNoofinst");
                        dtLoan.Rows[gr.RowIndex]["Noofinst"] = hdNoofinst.Value == "" ? "0" : hdNoofinst.Value;
                        HiddenField hdClosingType = (HiddenField)gr.FindControl("hdClosingType");
                        dtLoan.Rows[gr.RowIndex]["ClosingType"] = hdClosingType.Value == "" ? "N" : hdClosingType.Value;

                        HiddenField txtAdvance = (HiddenField)gr.FindControl("hdtxtAdvance");
                        dtLoan.Rows[gr.RowIndex]["NewAdvanceAmt"] = txtAdvance.Value == "" ? "0" : txtAdvance.Value;

                        dtLoan.Rows[gr.RowIndex]["AdvanceAmt"] = "0";

                        HiddenField hdtxtExcAmt = (HiddenField)gr.FindControl("hdtxtExcAmt");
                        dtLoan.Rows[gr.RowIndex]["ExcAmt"] = hdtxtExcAmt.Value == "" ? "0" : hdtxtExcAmt.Value;
                    }

                    TextBox txtChqNo = (TextBox)gr.FindControl("txtChque");
                    DropDownList ddlPyn = (DropDownList)gr.FindControl("ddlPyn");
                    dtLoan.Rows[gr.RowIndex]["ChequeNo"] = txtChqNo.Text;
                    dtLoan.Rows[gr.RowIndex]["PA"] = ddlPyn.SelectedValue;
                    Label txtLoanNo = (Label)gr.FindControl("txtLoanNo");
                    if (gr.Cells[21].Text == "" || gr.Cells[21].Text == "&nbsp;")
                    {
                        gblFuction.AjxMsgPopup("Loan A/c is not Set for Loan No. " + txtLoanNo.Text);
                        return;
                    }
                    dtLoan.Rows[gr.RowIndex]["LoanAc"] = gr.Cells[21].Text;
                    if (gr.Cells[22].Text == "" || gr.Cells[22].Text == "&nbsp;")
                    {
                        gblFuction.AjxMsgPopup("Loan Interest A/c is not Set for " + txtLoanNo.Text);
                        return;
                    }
                    dtLoan.Rows[gr.RowIndex]["InstAc"] = gr.Cells[22].Text;
                    dtLoan.Rows[gr.RowIndex]["AdvAc"] = gr.Cells[45].Text;

                    dtLoan.Rows[gr.RowIndex]["ProductID"] = gr.Cells[26].Text;
                    dtLoan.Rows[gr.RowIndex]["IntRate"] = gr.Cells[27].Text;
                    dtLoan.Rows[gr.RowIndex]["MemberNo"] = gr.Cells[28].Text;
                    dtLoan.Rows[gr.RowIndex]["PrincpalDue"] = gr.Cells[29].Text;
                    dtLoan.Rows[gr.RowIndex]["TotalDue"] = gr.Cells[30].Text;
                    // dtLoan.Rows[gr.RowIndex]["WeekDue"] = gr.Cells[31].Text;
                    dtLoan.Rows[gr.RowIndex]["OverDue"] = gr.Cells[32].Text;
                    dtLoan.Rows[gr.RowIndex]["ChequeNo"] = gr.Cells[33].Text;
                    dtLoan.Rows[gr.RowIndex]["CollType"] = gr.Cells[34].Text;
                    dtLoan.Rows[gr.RowIndex]["LWaveOffId"] = gr.Cells[35].Text;
                    dtLoan.Rows[gr.RowIndex]["IsWriteoff"] = gr.Cells[36].Text;
                    dtLoan.Rows[gr.RowIndex]["WriteOffAC"] = gr.Cells[37].Text;
                    dtLoan.Rows[gr.RowIndex]["WriteOffRecAC"] = gr.Cells[38].Text;

                    HiddenField hdDescID = (HiddenField)gr.FindControl("hdDescID");
                    HiddenField hdDeathDt = (HiddenField)gr.FindControl("hdDeathDt");
                    HiddenField hdDPerson = (HiddenField)gr.FindControl("hdDPerson");
                    HiddenField hdDeathType = (HiddenField)gr.FindControl("hdDeathType");
                    HiddenField hdPrincpalDue = (HiddenField)gr.FindControl("hdPrincpalDue");
                    HiddenField hdLWaveOffId = (HiddenField)gr.FindControl("hdLWaveOffId");
                    if (Convert.ToString(dtLoan.Rows[gr.RowIndex]["ClosingType"]) == "D")//hdClosingType
                    {
                        dtLoan.Rows[gr.RowIndex]["DeathDt"] = gblFuction.setDate(hdDeathDt.Value);
                        dtLoan.Rows[gr.RowIndex]["DPerson"] = hdDPerson.Value;
                        dtLoan.Rows[gr.RowIndex]["DeathType"] = hdDeathType.Value;

                        dtLoan.Rows[gr.RowIndex]["LWaveOffId"] = hdLWaveOffId.Value;

                    }
                    else
                    {

                        dtLoan.Rows[gr.RowIndex]["DeathDt"] = "";
                        dtLoan.Rows[gr.RowIndex]["DPerson"] = "";
                        //dtLoan.Rows[gr.RowIndex]["DeathType"] = "";
                        dtLoan.Rows[gr.RowIndex]["DeathType"] = 0;
                        dtLoan.Rows[gr.RowIndex]["LWaveOffId"] = 0;
                        vTotFrRecNo = vTotFrRecNo + Convert.ToDecimal((dtLoan.Rows[gr.RowIndex]["Total"]));
                    }
                    dtLoan.Rows[gr.RowIndex]["DescID"] = hdDescID.Value;
                    dtLoan.Rows[gr.RowIndex]["ReffId"] = gr.Cells[24].Text;
                    HiddenField hdReason = (HiddenField)gr.FindControl("hdReason");
                    dtLoan.Rows[gr.RowIndex]["Reason"] = hdReason.Value;

                    dtLoan.Rows[gr.RowIndex]["AbsentReason"] = -1;
                    dtLoan.Rows[gr.RowIndex]["PaymentStatus"] = -1;
                }
                if (dtLoan.Rows[0]["LoanAc"].ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Loan A/c is not Set for this Loan Type");
                    return;
                }
                if (dtLoan.Rows[0]["InstAc"].ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Loan Interest A/c is not Set for this Loan Type");
                    return;
                }
                if (dtLoan.Rows[0]["WriteOffRecAC"].ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Bad Debt Written Off Recovery A/c is not Set for this Loan Type");
                    return;
                }
                if (dtLoan == null) return;
                if ((vTotFrRecNo > 0) && (txtReceiptNo.Text.Trim() == ""))
                {
                    gblFuction.AjxMsgPopup("Please Enter the Receipt No");
                    return;
                }
                if (pAccDate > vLoginDt)
                {
                    gblFuction.AjxMsgPopup("Collection date should not greater than login date");
                    return;
                }

                if (pAccDate < gblFuction.setDate(hdnValueDt.Value))
                {
                    gblFuction.AjxMsgPopup("Value date should not greater than Transaction date");
                    return;
                }

                if (pAccDate < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) && pAccDate > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Recovery Date should be in financial year.");
                    return;
                }
                if (pAccDate < vFinFromDt || pAccDate > vFinToDt)
                {
                    gblFuction.AjxMsgPopup("Recovery Date should be in financial year.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtRecovryDt");
                    return;
                }

                if (dtLoan.Rows.Count == 0)
                {
                    gblFuction.AjxMsgPopup("No amount is collected");
                    return;
                }
                if (gblFuction.setDate(hdnValueDt.Value) > gblFuction.setDate(txtRecovryDt.Text))
                {
                    gblFuction.AjxMsgPopup("Value Date Should be Less than Equal To Transaction Date.");
                    return;
                }

                vUserId = Convert.ToInt32(Session[gblValue.UserId]);
                if (this.RoleId != 1 && vUserId != 13 && vUserId != 69 && vUserId != 1511 && vUserId != 505)//CENTR - 4103
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return;
                        }
                    }
                }
                oLR = new CLoanRecovery();

                //string vMsg;
                //if (cbAdvColl.Checked == true)
                //{
                //    vMsg = oLR.ChkHoliday(pBranch, gblFuction.setDate(txtAdvDate.Text));
                //    if (vMsg != "")
                //    {
                //        gblFuction.AjxMsgPopup(vMsg);
                //        return;
                //    }
                //}
                using (StringWriter oSW = new StringWriter())
                {
                    dtLoan.WriteXml(oSW);
                    sXml = oSW.ToString();
                }
                string vReceiptNo = txtReceiptNo.Text.Trim();

                vRetMsg = oLR.ChkCollectionBeforeSave(pAccDate, sXml);
                if (vRetMsg != "X")
                {
                    gblFuction.MsgPopup(vRetMsg);
                    return;
                }

                vErr = oLR.InsertCollection(pAccDate, sXml, pBranch, vActMstTbl, vActDtlTbl, vModeAC, vFinYear, vNaration, Convert.ToInt32(Session[gblValue.UserId]),
                    "I", 0, vCollMode, vEoID, gblFuction.setDate(hdnValueDt.Value), ref vReceiptNo, Convert.ToString(Session[gblValue.MultiColl]), txtReceiptNo.Text.Trim());
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg + " Receipt No is -" + vReceiptNo);
                }
                else if (vErr == 5)
                {
                    gblFuction.AjxMsgPopup("ReceiptNo already exist..");
                }
                else if (vErr == 6)
                {
                    gblFuction.AjxMsgPopup("Cash Reconciliation already done for this day..");
                }
                else if (vErr == 7)
                {
                    gblFuction.AjxMsgPopup("You do not have rights to take multiple collection..");
                }
                else if (vErr == 8)
                {
                    gblFuction.AjxMsgPopup("Collection Amount Must be Greater than Zero..");
                }
                else if (vErr == 9)
                {
                    gblFuction.AjxMsgPopup("Premature Collection is not valid. Please try again..");
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                }
                ViewState["dtDtlRst"] = null;
                ViewState["dtRst"] = null;
                ClearControls();
                chkPreClose.Checked = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLR = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="pgvName"></param>
        private void TotCalculation(DataTable dt, string pgvName)
        {
            decimal TotPrin = 0, TotInst = 0, TotAmt = 0;
            if (pgvName == "gvRecvry")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TotPrin = TotPrin + Convert.ToDecimal(dr["PrincpalPaid"]);
                    TotInst = TotInst + Convert.ToDecimal(dr["InterestPaid"]);
                    TotAmt = TotAmt + Convert.ToDecimal(dr["Total"]);
                }
                txtTotPrin.Text = string.Format("{0:N}", TotPrin);
                txtTotInt.Text = string.Format("{0:N}", TotInst);
                txtTotal.Text = TotAmt.ToString();//string.Format("{0:N}", TotAmt);
            }
            if (pgvName == "gvLed")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TotPrin = TotPrin + Convert.ToDecimal(dr["PrinCollAmt"]);
                    TotInst = TotInst + Convert.ToDecimal(dr["IntCollAmt"]);
                }
                txtPrinPaidL.Text = string.Format("{0:N}", TotPrin);
                txtInstPaidL.Text = string.Format("{0:N}", TotInst);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLedgr_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = null;
            CLoanRecovery oLR = null;
            string vLedTyp = "A";
            try
            {
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                // string pLoanID = Convert.ToString(ViewState["LoanId"]);
                string pLoanID = hdLoanId.Value;
                oLR = new CLoanRecovery();
                dtDtl = oLR.GetCollectionDtlByLoanId(pLoanID, vLedTyp, vBrCode);
                if (dtDtl.Rows.Count > 0)
                {
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                    TotCalculation(dtDtl, "gvLnDisb");
                }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = null;
            try
            {
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                if (rdbRecvry.SelectedValue != "N")
                {
                    if (e.CommandName == "cmdDel")
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("lbtnShow");
                        foreach (GridViewRow gr in gvLed.Rows)
                        {
                            LinkButton lb = (LinkButton)gr.FindControl("lbtnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                        }
                        btnShow.ForeColor = System.Drawing.Color.Red;
                        dtDtl = (DataTable)ViewState["dtDtlRst"];
                        //string vLoanID = ViewState["LoanId"].ToString();
                        Int32 vSLNo = Convert.ToInt32(e.CommandArgument);
                        Int32 vRow = dtDtl.Rows.IndexOf(dtDtl.Rows.Find(vSLNo));
                        txtPrinPay.Text = Convert.ToString(dtDtl.Rows[vRow]["PrincDue"]);
                        txtIntPay.Text = Convert.ToString(dtDtl.Rows[vRow]["InstDue"]);
                        //txtBankChrgPay.Text = Convert.ToString(dtDtl.Rows[vRow]["BankChargeDue"]);
                        txtTotAmtPay.Text = Convert.ToString(dtDtl.Rows[vRow]["TotalDue"]);

                        txtPrinPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["PrinCollAmt"]);
                        txtIntPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["IntCollAmt"]);
                        //txtBankPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["BankChargeColl"]);
                        txtTotPaid.Text = Convert.ToString(dtDtl.Rows[vRow]["TotalAmt"]);
                        ViewState["SlNo"] = vSLNo;

                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please select the modify recovery...");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            DataTable dtW = null;
            CLoanRecovery oLR = null;
            DateTime vlastCollDt;
            Int32 vSlNo = 0, vErr = 0, vMaxSLNo = 0, vErr1 = 0;
            string vReffID = "", vLedTyp = "M", vLnStatus = "";
            if (Convert.ToDecimal(txtTotPaid.Text) + Convert.ToDecimal(txtAdvPaid.Text) <= 0) return;
            try
            {
                string vAcMst = Session[gblValue.ACVouMst].ToString();
                string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                string vBranch;
                vBranch = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBranch = ddlBranch.SelectedValue.ToString();
                //}
                string vLoanID = hdLoanId.Value;// Convert.ToString(ViewState["LoanId"]);
                CCollectionRoutine oCC = new CCollectionRoutine();
                dtW = oCC.GetMaxCollDate(vLoanID, vBranch, "M");
                if (Convert.ToString(dtW.Rows[0]["IsWriteoff"]) == "Y")
                {
                    if (Convert.ToString(dtW.Rows[0]["Status"]) != "O")
                    {
                        gblFuction.AjxMsgPopup("After Bad Debt Written Off you can not delete the collection.");
                        return;
                    }
                }
                vSlNo = Convert.ToInt32(hdSlNo.Value);
                vReffID = vLoanID + "-" + vSlNo;
                dt = (DataTable)ViewState["dtDtlRst"];
                vMaxSLNo = Convert.ToInt32(dt.Compute("MAX(SLNo)", ""));
                dt.Select("SLNo=" + vMaxSLNo).ToString();
                DataRow[] dr = dt.Select("SLNo=" + vMaxSLNo);
                vlastCollDt = gblFuction.setDate(Convert.ToString(dr[0].ItemArray[1].ToString()));
                //vlastCollDt = gblFuction.setDate(Convert.ToString(dt.Compute("max(RecDate)", " ")));
                if (vSlNo != vMaxSLNo)
                {
                    gblFuction.AjxMsgPopup("You Can delete only Last Record.");
                    return;
                }

                if (vlastCollDt < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || vlastCollDt > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Recovery Delete Date should be in financial year.");
                    return;
                }
                if (this.RoleId != 1)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vlastCollDt)
                    {
                        gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                        return;
                    }
                }
                //----------------------------------------------------------------------
                oLR = new CLoanRecovery();
                vErr1 = oLR.CashReconChkFortheDay(vlastCollDt, vBranch);
                if (vErr1 > 0)
                {
                    gblFuction.AjxMsgPopup("You can not Delete,Cash Reconciliation already done..");
                    return;
                }
                //---------------------------------------------------------------------------

                // oLR = new CLoanRecovery();
                if (rdbLoan.SelectedValue == "O")
                    vLnStatus = "O";
                else if (rdbLoan.SelectedValue == "C")
                    vLnStatus = "C";
                vErr = oLR.DeleteCollection(vLoanID, vSlNo, vLnStatus, vReffID, vAcMst, vAcDtl, vBranch, Convert.ToInt32(Session[gblValue.UserId]));
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
                    dtDtl = oLR.GetCollectionDtlByLoanId(vLoanID, vLedTyp, vBranch);
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                    TotCalculation(dtDtl, "gvLnDisb");
                    txtPrinPay.Text = "0.00";
                    txtPrinPaid.Text = "0.00";
                    txtIntPay.Text = "0.00";
                    txtIntPaid.Text = "0.00";
                    txtTotAmtPay.Text = "0.00";
                    txtTotPaid.Text = "0.00";
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oLR = null;
            }
        }

        protected void btnReverse_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            DataTable dtW = null;
            CLoanRecovery oLR = null;
            DateTime vlastCollDt;
            Int32 vSlNo = 0, vErr = 0, vMaxSLNo = 0, vErr1 = 0;
            string vReffID = "", vLedTyp = "M", vLnStatus = "";
            if (Convert.ToDecimal(txtTotPaid.Text) + Convert.ToDecimal(txtAdvPaid.Text) <= 0) return;
            try
            {
                string vAcMst = Session[gblValue.ACVouMst].ToString();
                string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                string vBranch;
                vBranch = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBranch = ddlBranch.SelectedValue.ToString();
                //}
                string vLoanID = hdLoanId.Value;// Convert.ToString(ViewState["LoanId"]);
                CCollectionRoutine oCC = new CCollectionRoutine();
                dtW = oCC.GetMaxCollDate(vLoanID, vBranch, "M");
                if (Convert.ToString(dtW.Rows[0]["IsWriteoff"]) == "Y")
                {
                    if (Convert.ToString(dtW.Rows[0]["Status"]) != "O")
                    {
                        gblFuction.AjxMsgPopup("After Bad Debt Written Off you can not reverse the collection.");
                        return;
                    }
                }
                vSlNo = Convert.ToInt32(hdSlNo.Value);
                vReffID = vLoanID + "-" + vSlNo;
                dt = (DataTable)ViewState["dtDtlRst"];
                vMaxSLNo = Convert.ToInt32(dt.Compute("MAX(SLNo)", ""));
                dt.Select("SLNo=" + vMaxSLNo).ToString();
                DataRow[] dr = dt.Select("SLNo=" + vMaxSLNo);
                vlastCollDt = gblFuction.setDate(Convert.ToString(dr[0].ItemArray[1].ToString()));
                //if (vSlNo != vMaxSLNo)
                //{
                //    gblFuction.AjxMsgPopup("You Can reverse only Last Record.");
                //    return;
                //}

                if (vlastCollDt < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || vlastCollDt > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Recovery Reverse Date should be in financial year.");
                    return;
                }
                if (this.RoleId != 1)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not Reverse, Day end already done..");
                        return;
                    }

                    //----------------------------------------------------------------------
                    oLR = new CLoanRecovery();
                    vErr1 = oLR.CashReconChkFortheDay(gblFuction.setDate(txtRecovryDt.Text), vBranch);
                    if (vErr1 > 0)
                    {
                        gblFuction.AjxMsgPopup("You can not Reverse,Cash Reconciliation already done..");
                        return;
                    }
                    //---------------------------------------------------------------------------
                }
                // oLR = new CLoanRecovery();
                if (rdbLoan.SelectedValue == "O")
                    vLnStatus = "O";
                else if (rdbLoan.SelectedValue == "C")
                    vLnStatus = "C";
                oLR = new CLoanRecovery();
                vErr = oLR.ReverseCollection(vLoanID, vSlNo, vLnStatus, vReffID, vAcMst, vAcDtl, vBranch, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(txtRecovryDt.Text), Session[gblValue.ShortYear].ToString(), txtReversalReson.Text);
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup("Amount Reversed Successfully");
                    txtReversalReson.Text = "";
                    oLR = new CLoanRecovery();
                    dtDtl = oLR.GetCollectionDtlByLoanId(vLoanID, vLedTyp, vBranch);
                    gvLed.DataSource = dtDtl;
                    gvLed.DataBind();
                    dtDtl.PrimaryKey = new DataColumn[] { dtDtl.Columns["SlNo"] };
                    ViewState["dtDtlRst"] = dtDtl;
                    TotCalculation(dtDtl, "gvLnDisb");
                    txtPrinPay.Text = "0.00";
                    txtPrinPaid.Text = "0.00";
                    txtIntPay.Text = "0.00";
                    txtIntPaid.Text = "0.00";
                    txtTotAmtPay.Text = "0.00";
                    txtTotPaid.Text = "0.00";
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oLR = null;
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
        protected void gvRecvry_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtChque = (TextBox)e.Row.FindControl("txtChque");
                DropDownList ddlPyn = (DropDownList)e.Row.FindControl("ddlPyn");
                ddlPyn.SelectedIndex = ddlPyn.Items.IndexOf(ddlPyn.Items.FindByValue(e.Row.Cells[11].Text));
                if (rdbCashBank.SelectedValue == "C")
                    txtChque.Enabled = false;
                else if (rdbCashBank.SelectedValue == "B")
                    txtChque.Enabled = true;
                //10
                //if (rdbCashBank.SelectedValue == "C")
                txtChque.Enabled = false;
                //else if (rdbCashBank.SelectedValue == "B")
                //    txtChque.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtChque_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CLoanRecovery oLR = null;
            Int32 vRow = 0;
            try
            {
                string vBrCode;
                vBrCode = Session[gblValue.BrnchCode].ToString();
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{

                //}
                //else
                //{
                //    vBrCode = ddlBranch.SelectedValue.ToString();
                //}
                TextBox txtBox = (TextBox)sender;
                GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
                vRow = gvRow.RowIndex;
                TextBox txtChequeText = (TextBox)gvRow.FindControl("txtChque");
                if (txtChequeText.Text == "")
                    return;
                if (Convert.ToInt32(txtChequeText.Text.Length) != 6)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Cheque No. should be 6 Digit");
                    //gblFuction.focus("ctl00_cph_Main_tabLnDis_PnlDtl_txtChque");
                    txtChequeText.Text = "";
                    return;
                }
                else
                {
                    oLR = new CLoanRecovery();
                    //vRet = oLR.ChkduplicateChechno(txtChequeText.Text, vBrCode);
                    //if (vRet == 1)
                    //{
                    //    gblFuction.AjxMsgPopup("Cheque No is duplicate");
                    //    return;
                    //}
                    dt = (DataTable)ViewState["dtRst"];
                    dt.Rows[vRow]["ChequeNo"] = txtChequeText.Text;
                    dt.AcceptChanges();
                    gvRecvry.DataSource = dt;
                    gvRecvry.DataBind();
                    ViewState["dtRst"] = dt;
                }
            }
            catch { }
            finally
            {
                dt = null;
                oLR = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkJurnl_CheckedChanged(object sender, EventArgs e)
        {
            if (chkJurnl.Checked == false)
            {
                ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
                ddlLedger.Enabled = false;
            }
            else
            {
                ddlLedger.SelectedIndex = -1;
                ddlLedger.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void chkPreClose_CheckedChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    DataTable dtM = null;
        //    string vLoanID = "";
        //    Int32 vRow = 0;
        //    Int32 vTotAmt = 0, vErr = 0;
        //    //Decimal vIntAmt = 0.0M;
        //    dt = (DataTable)ViewState["dtRst"];
        //    if (ViewState["LoanId"] == null) return;
        //    vLoanID = (string)ViewState["LoanId"];
        //    vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));
        //    string vBrCode;
        //    vBrCode = Session[gblValue.BrnchCode].ToString();
        //    //if (Session[gblValue.BrnchCode].ToString() != "0000")
        //    //{

        //    //}
        //    //else
        //    //{
        //    //    vBrCode = ddlBranch.SelectedValue.ToString();
        //    //}
        //    CCollectionRoutine oCC = new CCollectionRoutine();
        //    dtM = oCC.GetMaxCollDate(vLoanID, vBrCode, "M");
        //    if (dtM.Rows.Count > 0)
        //    {
        //        if (Convert.ToString(dtM.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dtM.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtRecovryDt.Text))
        //        {
        //            gblFuction.AjxMsgPopup(" Last collection date is" + Convert.ToString(dtM.Rows[0]["MaxCollDt"]));
        //            chkPreClose.Checked = false;
        //            return;
        //        }
        //    }
        //    vErr = oCC.ChkPreMatClosing(vLoanID, vBrCode, gblFuction.setDate(txtRecovryDt.Text));
        //    if (vErr > 0)
        //    {
        //        gblFuction.AjxMsgPopup("Expected closing date of this loan is more than 35 Days. Premature closing is not possible");
        //        chkPreClose.Checked = false;
        //        return;
        //    }
        //    try
        //    {
        //        //vIntAmt = Convert.ToDecimal(txtIntPay.Text);
        //        if (chkPreClose.Checked == true)
        //        {
        //            txtPrinPay.Text = string.Format("{0:N}", txtPrinOS.Text);
        //            txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));

        //            vTotAmt = Convert.ToInt32(Convert.ToDecimal(txtTotAmtPay.Text));
        //            //if(vTotAmt % 5 !=0)
        //            //    vTotAmt = ((vTotAmt / 5) + 1) * 5;

        //            if ((Convert.ToDecimal(txtTotAmtPay.Text) - vTotAmt) > 0)
        //                vTotAmt = vTotAmt + 1;

        //            txtTotAmtPay.Text = Convert.ToString(vTotAmt);
        //            txtIntPay.Text = Convert.ToString(Convert.ToDecimal(txtTotAmtPay.Text) - Convert.ToDecimal(txtPrinPay.Text));

        //            txtPrinPaid.Text = txtPrinPay.Text;
        //            txtIntPaid.Text = txtIntPay.Text;
        //            txtTotPaid.Text = txtTotAmtPay.Text;
        //            chkDeathClose.Enabled = false;
        //            chkAdv.Checked = false;
        //            chkAdv.Enabled = false;
        //            ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
        //            ddlLedger.Enabled = false;
        //        }
        //        else
        //        {
        //            txtIntPay.Text = Convert.ToString(dt.Rows[vRow]["InterestDue"]);
        //            txtPrinPay.Text = Convert.ToString(dt.Rows[vRow]["PrincpalDue"]);
        //            txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));
        //            txtPrinPaid.Text = txtPrinPay.Text;
        //            txtIntPaid.Text = txtIntPay.Text;
        //            txtTotPaid.Text = txtTotAmtPay.Text;
        //            chkDeathClose.Enabled = true;
        //            chkAdv.Enabled = true;
        //            ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
        //            ddlLedger.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void chkDeathClose_CheckedChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null, dt1 = null;
        //    string vLoanID = "";
        //    Int32 vRow = 0;
        //    Int32 vTotAmt = 0;
        //    //Decimal vIntAmt = 0.0M;
        //    dt = (DataTable)ViewState["dtRst"];
        //    if (ViewState["LoanId"] == null) return;
        //    vLoanID = (string)ViewState["LoanId"];
        //    vRow = dt.Rows.IndexOf(dt.Rows.Find(vLoanID));
        //    CLoanRecovery oLR = null;
        //    oLR = new CLoanRecovery();
        //    dt1 = oLR.ChkBCLoanByLoanId(vLoanID);

        //    if (chkDeathClose.Checked == true)
        //    {
        //        chkJurnl.Checked = true;
        //        ddlLedger.Enabled = true;
        //        if (dt1.Rows[0]["ISBC"].ToString() == "Y")
        //            ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("G0962")); // RECEIVABLE FROM BC MEMBER (DEATH CASE) 
        //        else
        //            ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("G0320")); // INSURANCE CLAIMS RECEIVABLE
        //        ddlLedger.Enabled = false;
        //        chkPreClose.Enabled = false;
        //        chkAdv.Checked = false;
        //        chkAdv.Enabled = false;
        //        txtDeathDt.Enabled = true;
        //        ddlWvReason.Enabled = true;
        //        ddlWvReason.SelectedIndex = -1;
        //        ddlDPerson.Enabled = true;
        //        ddlDPerson.SelectedIndex = -1;
        //        ddlDeathType.Enabled = true;

        //        txtDeathDt.Text = "";// (string)Session[gblValue.LoginDate];

        //        txtPrinPay.Text = string.Format("{0:N}", txtPrinOS.Text);
        //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text));

        //        vTotAmt = Convert.ToInt32(Convert.ToDecimal(txtTotAmtPay.Text));
        //        //vTotAmt = ((vTotAmt / 5) + 1) * 5;
        //        txtTotAmtPay.Text = Convert.ToString(vTotAmt);
        //        txtIntPay.Text = "0.00";

        //        txtPrinPaid.Text = txtPrinPay.Text;
        //        txtIntPaid.Text = txtIntPay.Text;
        //        txtTotPaid.Text = txtTotAmtPay.Text;
        //        DateTime vCollDt = gblFuction.setDate(txtRecovryDt.Text.ToString());
        //        PopInsLedger(vCollDt);
        //    }
        //    else
        //    {
        //        chkJurnl.Checked = false;
        //        ddlLedger.Enabled = false;
        //        ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue("C0001"));
        //        chkPreClose.Enabled = true;
        //        chkAdv.Enabled = true;
        //        txtDeathDt.Enabled = false;
        //        txtDeathDt.Text = "";
        //        ddlWvReason.Enabled = false;
        //        ddlWvReason.SelectedIndex = -1;
        //        ddlDPerson.Enabled = false;
        //        ddlDPerson.SelectedIndex = -1;
        //        ddlDeathType.Enabled = false;

        //        txtIntPay.Text = Convert.ToString(dt.Rows[vRow]["InterestDue"]);
        //        txtPrinPay.Text = Convert.ToString(dt.Rows[vRow]["PrincpalDue"]);
        //        txtTotAmtPay.Text = Convert.ToString(Convert.ToDecimal(txtPrinPay.Text) + Convert.ToDecimal(txtIntPay.Text));
        //        txtPrinPaid.Text = txtPrinPay.Text;
        //        txtIntPaid.Text = txtIntPay.Text;
        //        txtTotPaid.Text = txtTotAmtPay.Text;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void chkAdv_CheckedChanged(object sender, EventArgs e)
        //{

        //    Int32 vRoleID = Convert.ToInt32(Session[gblValue.RoleId]);
        //    if (chkAdv.Checked == true)
        //    {
        //        if (Convert.ToInt32(hfAM.Value) == vRoleID)
        //        {
        //            chkDeathClose.Checked = false;
        //            chkPreClose.Checked = false;
        //            chkDeathClose.Enabled = false;
        //            chkPreClose.Enabled = false;
        //        }
        //        else
        //        {
        //            gblFuction.AjxMsgPopup("Only AM is Authorised to Enter Advance Payement"); //AM Role Id must be 2
        //            chkAdv.Checked = false;
        //            chkDeathClose.Enabled = true;
        //            if (Convert.ToInt32(Session[gblValue.RoleId]) == 2 || Convert.ToInt32(Session[gblValue.RoleId]) == 3 || Convert.ToInt32(Session[gblValue.RoleId]) == 6)
        //            {
        //                chkPreClose.Enabled = false;
        //            }
        //            else
        //            {
        //                chkPreClose.Enabled = true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        chkDeathClose.Enabled = true;
        //        if (Convert.ToInt32(Session[gblValue.RoleId]) == 2 || Convert.ToInt32(Session[gblValue.RoleId]) == 3 || Convert.ToInt32(Session[gblValue.RoleId]) == 6)
        //        {
        //            chkPreClose.Enabled = false;
        //        }
        //        else
        //        {
        //            chkPreClose.Enabled = true;
        //        }
        //    }
        //}
        private void PopBank()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CVoucher oVou = new CVoucher();
            dt = oVou.GetBank(vBrCode);
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescID";
            ddlBank.DataSource = dt;
            ddlBank.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlBank.Items.Insert(0, oItem);
        }
        protected void rdbCashBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbCashBank.SelectedValue == "C")
            {
                ClearControls();
                ddlBank.Enabled = false;
                ddlBank.SelectedIndex = -1;
            }
            else if (rdbCashBank.SelectedValue == "B")
            {
                ClearControls();
                ddlBank.Enabled = true;
                ddlBank.SelectedIndex = -1;
            }

        }
        protected void cbAdvColl_CheckeChenged(object sender, EventArgs e)
        {
            if (cbAdvColl.Checked == true)
            {
                txtAdvDate.Enabled = true;
                txtToDt.Enabled = true;
            }
            else
            {
                txtAdvDate.Text = "";
                txtToDt.Text = "";
                txtAdvDate.Enabled = false;
                txtToDt.Enabled = false;
            }
        }
    }
}
