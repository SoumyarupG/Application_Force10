using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class LeadGeneration : CENTRUMBAse
    {
        protected int cPgNo = 1; 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var x = hdGenParameterId.Value;

            InitBasePage();
            if (!IsPostBack)
            {
                //if (Session[gblValue.BrnchCode].ToString() == "0000")
                //    StatusButton("Exit");
                //else
                //    StatusButton("View");
                txtLeadGenDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                //PopPropertyType();
                PopOccupation();
                LoadLeadList(1);
                tbLeadGen.ActiveTabIndex = 0;
                StatusButton("View");
                pnlTeleCalling.Visible = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Lead Generation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLeadGeneration);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "General Parameter", false);
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
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = true;
                    btnReject.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                    EnableControl(false);
                    break;
            }
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
                tbLeadGen.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                pnlTeleCalling.Visible = false;
            }
            catch (Exception ex)
            {
                throw ex;
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
                    LoadLeadList(1);
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
                if (hdnLogInFeesCollYN.Value == "Y")
                {
                    txtAddress.Enabled = false;
                    txtCustName.Enabled = false;
                    txtEmail.Enabled = false;
                    txtLeadGenDt.Enabled = false;
                    txtMobNo.Enabled = false;
                    txtEarningMember.Enabled = false;
                    ddlOccuType.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbLeadGen.ActiveTabIndex = 0;
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
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                LoadLeadList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControls();
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadLeadList(1);
        }
        private void LoadLeadList(int vPgNo)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetLeadList(vBrCode, txtSearch.Text.Trim());
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLead.DataSource = dt1;
                        gvLead.DataBind();
                    }
                    else
                    {
                        gvLead.DataSource = null;
                        gvLead.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                oMem = null;
            }

        }
        protected void gvLead_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string pLeadId = "";
            string vBrCode = "";
            DataTable dt,dt1 = null;
            DataSet ds = new DataSet();
            ClearControls();
            try
            {
                pLeadId = Convert.ToString(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["LeadId"] = pLeadId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvLead.Rows)
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
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnShow.ForeColor = System.Drawing.Color.White;
                    btnShow.Font.Bold = true;

                    CDistrict oDist = new CDistrict();
                    ds=oDist.GetLeadDtlByLeadId(pLeadId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

                    if (dt.Rows.Count > 0)
                    {
                        hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadId"]);
                        txtLeadID.Text = Convert.ToString(dt.Rows[0]["LeadId"]);
                        txtLeadGenDt.Text = Convert.ToString(dt.Rows[0]["LeadGenerationDate"]);
                        txtCustName.Text = Convert.ToString(dt.Rows[0]["CustomerName"]);
                        txtAddress.Text = Convert.ToString(dt.Rows[0]["Address"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtMobNo.Text = Convert.ToString(dt.Rows[0]["MobNo"]);                       
                        if (dt.Rows[0]["LogInFeesCollYN"].ToString() == "Y")
                            ChkLogInFees.Checked = true;
                        else
                            ChkLogInFees.Checked = false;
                        txtTotLogFees.Text = Convert.ToString(dt.Rows[0]["TotalLoginFees"]);
                        txtNetLogFees.Text = Convert.ToString(dt.Rows[0]["NetLogInFees"]);
                        txtLogFeesCGST.Text = Convert.ToString(dt.Rows[0]["CGSTAmt"]);
                        txtLogFeesSGST.Text = Convert.ToString(dt.Rows[0]["SGSTAmt"]);
                        txtLogFeesIGST.Text = Convert.ToString(dt.Rows[0]["IGSTAmt"]);
                        ddlOccuType.SelectedIndex = ddlOccuType.Items.IndexOf(ddlOccuType.Items.FindByValue(dt.Rows[0]["OccupationId"].ToString()));
                       // ddlPropertyType.SelectedIndex = ddlPropertyType.Items.IndexOf(ddlPropertyType.Items.FindByValue(dt.Rows[0]["PropertyTypeId"].ToString()));
                        txtEarningMember.Text = Convert.ToString(dt.Rows[0]["PropertyTypeId"]);
                        hdGenParameterId.Value = Convert.ToString(dt.Rows[0]["GenParameterId"]);

                        if (dt1.Rows.Count > 0)
                        {     
                            chkAadhar.Checked = dt1.Rows[0]["KycDoc"].ToString() == "Y" ? true : false;
                            chkPan.Checked = dt1.Rows[0]["KycDocPan"].ToString() == "Y" ? true : false;
                            ddlNob.SelectedIndex = ddlNob.Items.IndexOf(ddlNob.Items.FindByValue(dt1.Rows[0]["NatOfBusiness"].ToString()));
                            ddlBp.SelectedIndex = ddlBp.Items.IndexOf(ddlBp.Items.FindByValue(dt1.Rows[0]["BusinessProof"].ToString()));
                            ddlNoYSame.SelectedIndex = ddlNoYSame.Items.IndexOf(ddlNoYSame.Items.FindByValue(dt1.Rows[0]["Nysp"].ToString()));
                            ddlBpo.SelectedIndex = ddlBpo.Items.IndexOf(ddlBpo.Items.FindByValue(dt1.Rows[0]["Bpo"].ToString()));
                            ddlNod.SelectedIndex = ddlNod.Items.IndexOf(ddlNod.Items.FindByValue(dt1.Rows[0]["Nod"].ToString()));
                            ddlBankAc.SelectedIndex = ddlBankAc.Items.IndexOf(ddlBankAc.Items.FindByValue(dt1.Rows[0]["BankAc"].ToString()));
                            ddlResiOwner.SelectedIndex = ddlResiOwner.Items.IndexOf(ddlResiOwner.Items.FindByValue(dt1.Rows[0]["ResiOwner"].ToString()));
                            txtTotalIncome.Text = Convert.ToString(dt1.Rows[0]["TotalInc"]);
                            txtTotExp.Text = Convert.ToString(dt1.Rows[0]["TotalExp"]);
                            txtTotLoanObl.Text = Convert.ToString(dt1.Rows[0]["TotLoanOblig"]);
                            txtExpLoanAmt.Text = Convert.ToString(dt1.Rows[0]["ExpLoanAmt"]);
                           // txtAge.Text = Convert.ToString(dt1.Rows[0]["Age"]);
                            hdnTeleCallingYN.Value = "Y";
                        }
                        else
                        {
                            hdnTeleCallingYN.Value = "N";
                        }
                        tbLeadGen.ActiveTabIndex = 1;                       
                        pnlTeleCalling.Visible = true;
                        hdnLogInFeesCollYN.Value = dt.Rows[0]["LogInFeesCollYN"].ToString();
                        if (dt.Rows[0]["LogInFeesCollYN"].ToString() == "Y" || dt.Rows[0]["RejectYN"].ToString() == "Y")
                        {
                            StatusButton("View");
                            btnApprove.Enabled = false;
                            btnReject.Enabled = false;
                        }
                        else
                        {
                            StatusButton("Show");
                        }
                        
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("New Lead Generation/Update Existing Lead Information Can be Done From Branch.Please Log In To Branch");
                return false;
            }
            Int32 vErr = 0, vProTypeId = 0, vOccTypeId = 0, vGenParId = 0;
            Decimal vTotLogFees = 0, vNetLogFees = 0, vCGSTAmt = 0, vSGSTAmt = 0, vIGSTAmt = 0;
            string vNewId = "", vLeadId = "", vCustomerName = "", vEmail = "", vMobNo = "", vAddress = "",
                vLogInFeesCollYN = "N", vBrCode = "", vErrMsg = "";
            CMember oMem = null;
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = hdLeadId.Value;
                }
                vCustomerName = txtCustName.Text.ToString();
                vEmail = txtEmail.Text.ToString();
                vMobNo = txtMobNo.Text.ToString();
                vAddress = txtAddress.Text.ToString();
                if (ChkLogInFees.Checked == true)
                    vLogInFeesCollYN = "Y";
                else
                    vLogInFeesCollYN = "N";
                vBrCode = Session[gblValue.BrnchCode].ToString();

                if (((Request[hdGenParameterId.UniqueID] as string == null) ? hdGenParameterId.Value : Request[hdGenParameterId.UniqueID] as string) != "")
                {
                    vGenParId = Convert.ToInt32((Request[hdGenParameterId.UniqueID] as string == null) ? hdGenParameterId.Value : Request[hdGenParameterId.UniqueID] as string);
                }
                else
                {
                    vGenParId = 0;
                }
                if (txtLeadGenDt.Text == "")
                {
                    gblFuction.MsgPopup("Lead Generation Date Can Not Be Empty..");
                    return false;
                }
                DateTime pLeadGenDt = gblFuction.setDate(txtLeadGenDt.Text.ToString());
                if (((Request[txtTotLogFees.UniqueID] as string == null) ? txtTotLogFees.Text : Request[txtTotLogFees.UniqueID] as string) != "")
                    vTotLogFees = Convert.ToDecimal(((Request[txtTotLogFees.UniqueID] as string == null) ? txtTotLogFees.Text : Request[txtTotLogFees.UniqueID] as string));
                if (((Request[txtNetLogFees.UniqueID] as string == null) ? txtNetLogFees.Text : Request[txtNetLogFees.UniqueID] as string) != "")
                    vNetLogFees = Convert.ToDecimal((Request[txtNetLogFees.UniqueID] as string == null) ? txtNetLogFees.Text : Request[txtNetLogFees.UniqueID] as string);
                if (((Request[txtLogFeesCGST.UniqueID] as string == null) ? txtLogFeesCGST.Text : Request[txtLogFeesCGST.UniqueID] as string) != "")
                    vCGSTAmt = Convert.ToDecimal((Request[txtLogFeesCGST.UniqueID] as string == null) ? txtLogFeesCGST.Text : Request[txtLogFeesCGST.UniqueID] as string);
                if (((Request[txtLogFeesSGST.UniqueID] as string == null) ? txtLogFeesSGST.Text : Request[txtLogFeesSGST.UniqueID] as string) != "")
                    vSGSTAmt = Convert.ToDecimal((Request[txtLogFeesSGST.UniqueID] as string == null) ? txtLogFeesSGST.Text : Request[txtLogFeesSGST.UniqueID] as string);
                if (((Request[txtLogFeesIGST.UniqueID] as string == null) ? txtLogFeesIGST.Text : Request[txtLogFeesIGST.UniqueID] as string) != "")
                    vIGSTAmt = Convert.ToDecimal((Request[txtLogFeesIGST.UniqueID] as string == null) ? txtLogFeesIGST.Text : Request[txtLogFeesIGST.UniqueID] as string);

                //if (((Request[ddlPropertyType.UniqueID] as string == null) ? ddlPropertyType.SelectedValue : Request[ddlPropertyType.UniqueID] as string) != "-1")
                //    vProTypeId = Convert.ToInt32(((Request[ddlPropertyType.UniqueID] as string == null) ? ddlPropertyType.SelectedValue : Request[ddlPropertyType.UniqueID] as string));

                vProTypeId =Convert.ToInt32(txtEarningMember.Text);
                if (((Request[ddlOccuType.UniqueID] as string == null) ? ddlOccuType.SelectedValue : Request[ddlOccuType.UniqueID] as string) != "-1")
                    vOccTypeId = Convert.ToInt32(((Request[ddlOccuType.UniqueID] as string == null) ? ddlOccuType.SelectedValue : Request[ddlOccuType.UniqueID] as string));

                //if (vLogInFeesCollYN == "Y" && vTotLogFees <= 0)
                //{
                //    gblFuction.MsgPopup("LogIn Fees Amount Can Be Zero");
                //    return false;
                //}
                if (Mode == "Save")
                {
                    oMem = new CMember();
                    vErr = oMem.SaveLead(ref vNewId, vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                       vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                       vBrCode, this.UserID, "Save", 0, ref vErrMsg);
                    if (vErr == 0)
                    {
                        hdLeadId.Value = vNewId.Trim();
                        ViewState["LeadId"] = vNewId;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;

                        //// SMS Trigger---
                        //DataTable dt_Sms = new DataTable();
                        //CSMS oSms = null;
                        //AuthSms oAuth = null;
                        //string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                        //oSms = new CSMS();
                        //oAuth = new AuthSms();

                        //// For Applicant  (FC----> Fees Collection)
                        //dt_Sms = oSms.Get_ToSend_SMS(vNewId, pLeadGenDt, "FC");
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
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oMem = new CMember();
                    vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                       vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                       vBrCode, this.UserID, "Edit", 0, ref vErrMsg,chkAadhar.Checked==true?"Y":"N",ddlNob.SelectedValue,ddlBp.SelectedValue
                       ,ddlNoYSame.SelectedValue,ddlBpo.SelectedValue,ddlNod.SelectedValue,Convert.ToDouble(txtTotalIncome.Text == "" ? "0" :txtTotalIncome.Text),
                       Convert.ToDouble(txtTotExp.Text == "" ? "0" : txtTotExp.Text ), Convert.ToDouble(txtTotLoanObl.Text  == "" ? "0" : txtTotLoanObl.Text ),
                       Convert.ToDouble(txtExpLoanAmt.Text == "" ? "0" : txtExpLoanAmt.Text), ddlBankAc.SelectedValue, ddlResiOwner.SelectedValue, chkPan.Checked == true ? "Y" : "N");

                    if (vErr == 0)
                    {
                        hdLeadId.Value = vLeadId;
                        gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;

                        //// SMS Trigger---
                        //DataTable dt_Sms = new DataTable();
                        //CSMS oSms = null;
                        //AuthSms oAuth = null;
                        //string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                        //oSms = new CSMS();
                        //oAuth = new AuthSms();

                        //// For Applicant  (FC----> Fees Collection)
                        //dt_Sms = oSms.Get_ToSend_SMS(vLeadId, pLeadGenDt, "FC");
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
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oMem = new CMember();
                    vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                       vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                       vBrCode, this.UserID, "Delete", 0, ref vErrMsg, chkAadhar.Checked == true ? "Y" : "N", ddlNob.SelectedValue, ddlBp.SelectedValue
                       , ddlNoYSame.SelectedValue, ddlBpo.SelectedValue, ddlNod.SelectedValue, Convert.ToDouble(txtTotalIncome.Text == "" ? "0" : txtTotalIncome.Text),
                       Convert.ToDouble(txtTotExp.Text == "" ? "0" : txtTotExp.Text), Convert.ToDouble(txtTotLoanObl.Text == "" ? "0" : txtTotLoanObl.Text),
                       Convert.ToDouble(txtExpLoanAmt.Text == "" ? "0" : txtExpLoanAmt.Text), ddlBankAc.SelectedValue, ddlResiOwner.SelectedValue, chkPan.Checked == true ? "Y" : "N");
                    if (vErr == 0)
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
                else if (Mode == "Approve")
                {
                    oMem = new CMember();
                    vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                       vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                       vBrCode, this.UserID, "Approve", 0, ref vErrMsg, chkAadhar.Checked == true ? "Y" : "N", ddlNob.SelectedValue, ddlBp.SelectedValue
                       , ddlNoYSame.SelectedValue, ddlBpo.SelectedValue, ddlNod.SelectedValue, Convert.ToDouble(txtTotalIncome.Text == "" ? "0" : txtTotalIncome.Text),
                       Convert.ToDouble(txtTotExp.Text == "" ? "0" : txtTotExp.Text), Convert.ToDouble(txtTotLoanObl.Text == "" ? "0" : txtTotLoanObl.Text),
                       Convert.ToDouble(txtExpLoanAmt.Text == "" ? "0" : txtExpLoanAmt.Text), ddlBankAc.SelectedValue, ddlResiOwner.SelectedValue, chkPan.Checked == true ? "Y" : "N");
                    if (vErr == 0)
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
                else if (Mode == "Reject")
                {
                    oMem = new CMember();
                    vErr = oMem.UpdateLead(vLeadId, pLeadGenDt, vCustomerName, vEmail, vMobNo, vAddress, vProTypeId, vOccTypeId, vLogInFeesCollYN,
                       vGenParId, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt,
                       vBrCode, this.UserID, "Reject", 0, ref vErrMsg, chkAadhar.Checked == true ? "Y" : "N", ddlNob.SelectedValue, ddlBp.SelectedValue
                       , ddlNoYSame.SelectedValue, ddlBpo.SelectedValue, ddlNod.SelectedValue, Convert.ToDouble(txtTotalIncome.Text == "" ? "0" : txtTotalIncome.Text),
                       Convert.ToDouble(txtTotExp.Text == "" ? "0" : txtTotExp.Text), Convert.ToDouble(txtTotLoanObl.Text == "" ? "0" : txtTotLoanObl.Text),
                       Convert.ToDouble(txtExpLoanAmt.Text == "" ? "0" : txtExpLoanAmt.Text), ddlBankAc.SelectedValue, ddlResiOwner.SelectedValue, chkPan.Checked == true ? "Y" : "N");
                    if (vErr == 0)
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
        private void EnableControl(Boolean Status)
        {
            txtAddress.Enabled = Status;
            txtCustName.Enabled = Status;
            txtEmail.Enabled = Status;
            txtLeadGenDt.Enabled = Status;
            txtMobNo.Enabled = Status;
            //txtTotLogFees.Enabled = Status;
            //txtNetLogFees.Enabled = Status;
            //txtLogFeesCGST.Enabled = Status;
            //txtLogFeesSGST.Enabled = Status;
            //txtLogFeesIGST.Enabled = Status;
            //ddlPropertyType.Enabled = Status;
            txtEarningMember.Enabled = Status;
            ddlOccuType.Enabled = Status;
            //ChkLogInFees.Enabled = Status;
            //ddlKycDoc.Enabled = Status;;
            ddlNob.Enabled = Status;;
            ddlBp.Enabled = Status;;
            ddlNoYSame.Enabled = Status;
            ddlBpo.Enabled = Status;
            ddlNod.Enabled = Status;
            ddlBankAc.Enabled = Status;
            //chkAadhar.Checked = Status;
            //chkPan.Checked = Status;
            txtTotalIncome.Enabled = Status;
            txtTotExp.Enabled = Status;
            txtTotLoanObl.Enabled = Status;
            txtExpLoanAmt.Enabled = Status;
           // txtAge.Enabled = Status;
            ddlResiOwner.Enabled = Status;
        }
        private void ClearControls()
        {
            txtAddress.Text = "";
            txtCustName.Text = "";
            txtEmail.Text = "";
            txtMobNo.Text = "";
            txtTotLogFees.Text = "0.00";
            txtNetLogFees.Text = "0.00";
            txtLogFeesCGST.Text = "0.00";
            txtLogFeesSGST.Text = "0.00";
            txtLogFeesIGST.Text = "0.00";
            //ddlPropertyType.SelectedIndex = -1;
            txtEarningMember.Text = "";
            ddlOccuType.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            ChkLogInFees.Checked = false;
            txtLeadID.Text = "";
            //ddlKycDoc.SelectedIndex = -1;
            chkPan.Checked = false;
            chkAadhar.Checked = false;
            ddlNob.SelectedIndex = -1;
            ddlBp.SelectedIndex = -1;
            ddlNoYSame.SelectedIndex = -1;
            ddlBpo.SelectedIndex = -1;
            ddlNod.SelectedIndex = -1;
            ddlBankAc.SelectedIndex = -1;
            txtTotalIncome.Text = "";
            txtTotExp.Text = "";
            txtTotLoanObl.Text = "";
            txtExpLoanAmt.Text = "";
           // txtAge.Text = "";
            ddlResiOwner.SelectedIndex = -1;
            pnlTeleCalling.Visible = false;

        }
        //private void PopPropertyType()
        //{
        //    CMember oMem = new CMember();
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = oMem.GetCompanyAndPropertyType(2);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlPropertyType.DataSource = dt;
        //            ddlPropertyType.DataTextField = "PropertypeName";
        //            ddlPropertyType.DataValueField = "PropertyTypeID";
        //            ddlPropertyType.DataBind();
        //            ListItem oli = new ListItem("<--Select-->", "-1");
        //            ddlPropertyType.Items.Insert(0, oli);
        //        }
        //        else
        //        {
        //            ddlPropertyType.DataSource = null;
        //            ddlPropertyType.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oMem = null;
        //    }
        //}
        private void PopOccupation()
        {
            ddlOccuType.Items.Clear();
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetOccupation();
                if (dt.Rows.Count > 0)
                {
                    ddlOccuType.DataSource = dt;
                    ddlOccuType.DataTextField = "Occupation";
                    ddlOccuType.DataValueField = "OccupationId";
                }
                else
                {
                    ddlOccuType.DataSource = null;
                }
                ddlOccuType.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlOccuType.Items.Insert(0, oli1);
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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanProcess == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                

                if (SaveRecords("Approve") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadLeadList(1);
                    StatusButton("View");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanProcess == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Reject") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadLeadList(1);
                    StatusButton("View");                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadLeadList(cPgNo);
        }
    }
}