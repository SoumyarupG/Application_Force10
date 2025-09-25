using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using System.Data;
using CENTRUMBA;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Web.Services;


namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_InternalChecks : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFDocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        string vVKYCEnv = ConfigurationManager.AppSettings["VKYCEnv"];
        string vVKYCUsrNm = ConfigurationManager.AppSettings["VKYCUNm"];
        string vVKYCPasswrd = ConfigurationManager.AppSettings["VKYCPW"];
        string apiBaseUrl = ConfigurationManager.AppSettings["VkycapiBaseUrl"];
        string generateTokenEndpoint = ConfigurationManager.AppSettings["VkycTokenEndpoint"];
        string sendLinkEndpoint = ConfigurationManager.AppSettings["VkycLinkEndpoint"];
        string PosidexEncURL = ConfigurationManager.AppSettings["PosidexEncURL"];


        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                CF_GetCust360();

                hdnMaxFileSize.Value = MaxFileSize;
                if (Session[gblValue.LeadID] != null)
                {
                    GetInternalChecks();
                    StatusButton("Show");
                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbIntChk.ActiveTabIndex = 1;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Internal Checks";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFIntrChk);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Internal Checks", false);
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
            Boolean vAmlPassYn = hdnAmlPassYn.Value == "Y" ? false : Status;
            Boolean vValVKYCYN = hdnValVKYCYN.Value == "Y" ? false : Status;

            Boolean vAmlPassYnCA1 = hdnAmlPassYnCA1.Value == "Y" ? false : Status;
            Boolean vValVKYCYNCA1 = hdnValVKYCYNCA1.Value == "Y" ? false : Status;

            Boolean vAmlPassYnCA2 = hdnAmlPassYnCA2.Value == "Y" ? false : Status;
            Boolean vValVKYCYNCA2 = hdnValVKYCYNCA2.Value == "Y" ? false : Status;

            Boolean vAmlPassYnCA3 = hdnAmlPassYnCA3.Value == "Y" ? false : Status;
            Boolean vValVKYCYNCA3 = hdnValVKYCYNCA3.Value == "Y" ? false : Status;

            Boolean vAmlPassYnCA4 = hdnAmlPassYnCA4.Value == "Y" ? false : Status;
            Boolean vValVKYCYNCA4 = hdnValVKYCYNCA4.Value == "Y" ? false : Status;

            Boolean vAmlPassYnG = hdnAmlPassYnG.Value == "Y" ? false : Status;
            Boolean vValVKYCYNG = hdnValVKYCYNG.Value == "Y" ? false : Status;

            txtPanNo.Enabled = false;
            txtPanNoCA1.Enabled = false;
            txtPanNoCA2.Enabled = false;
            txtPanNoCA3.Enabled = false;
            txtPanNoCA4.Enabled = false;
            txtPanNoG.Enabled = false;
            txtVoterId.Enabled = false;
            txtVoterIdCA1.Enabled = false;
            txtVoterIdCA2.Enabled = false;
            txtVoterIdCA3.Enabled = false;
            txtVoterIdCA4.Enabled = false;
            txtVoterIdG.Enabled = false;
            ddlCustomer.Enabled = Status;

            ddlElecBillStat.Enabled = Status;
            ddlFCUStatus.Enabled = Status;

            btnCheckAml.Enabled = vAmlPassYn;
            btnCheckAmlCA1.Enabled = vAmlPassYnCA1;
            btnCheckAmlCA2.Enabled = vAmlPassYnCA2;
            btnCheckAmlCA3.Enabled = vAmlPassYnCA3;
            btnCheckAmlCA4.Enabled = vAmlPassYnCA4;
            btnCheckAmlG.Enabled = vAmlPassYnG;

            fuElecBill.Enabled = Status;
            fuElecBillCA1.Enabled = Status;
            fuElecBillCA2.Enabled = Status;
            fuElecBillCA3.Enabled = Status;
            fuElecBillCA4.Enabled = Status;
            fuElecBillG.Enabled = Status;

            fuFcuReport.Enabled = Status;
            fuFcuReportCA1.Enabled = Status;
            fuFcuReportCA2.Enabled = Status;
            fuFcuReportCA3.Enabled = Status;
            fuFcuReportCA4.Enabled = Status;
            fuFcuReportG.Enabled = Status;

            txtFCUDt.Enabled = Status;
            txtFCUDtCA1.Enabled = Status;
            txtFCUDtCA2.Enabled = Status;
            txtFCUDtCA3.Enabled = Status;
            txtFCUDtCA4.Enabled = Status;
            txtFCUDtG.Enabled = Status;

        }
        private void ClearControls()
        {
            lblBCPNum.Text = "";
            hdnLeadId.Value = "";
            hdnAppId.Value = "";
            txtPanNo.Text = "";
            lblPanStatus.Text = "";
            txtVoterId.Text = "";
            lblVoterStatus.Text = "";
            lblDdupStatus.Text = "";
            fuFcuReport.Controls.Clear();
            fuElecBill.Controls.Clear();


            hdnValVKYCYN.Value = "N";
            hdnValVKYCYNCA1.Value = "N";
            hdnValVKYCYNCA2.Value = "N";
            hdnValVKYCYNCA3.Value = "N";
            hdnValVKYCYNCA4.Value = "N";
            hdnValVKYCYNG.Value = "N";
            hdnAmlPassYn.Value = "N";
            hdnAmlPassYnCA1.Value = "N";
            hdnAmlPassYnCA2.Value = "N";
            hdnAmlPassYnCA3.Value = "N";
            hdnAmlPassYnCA4.Value = "N";
            hdnAmlPassYnG.Value = "N";


            txtPanNoCA1.Text = "";
            lblPanStatusCA1.Text = "";
            txtVoterIdCA1.Text = "";
            lblVoterStatusCA1.Text = "";
            lblDdupStatusCA1.Text = "";
            fuFcuReportCA1.Controls.Clear();
            fuElecBillCA1.Controls.Clear();
            lblCoApp1Status.Text = "";
            hdnCoApp1Status.Value = "Y";
            lblCoApp1Name.Text = "";
            lblCustNameCA1.Text = "";

            txtPanNoCA2.Text = "";
            lblPanStatusCA2.Text = "";
            txtVoterIdCA2.Text = "";
            lblVoterStatusCA2.Text = "";
            lblDdupStatusCA2.Text = "";
            fuFcuReportCA2.Controls.Clear();
            fuElecBillCA2.Controls.Clear();
            lblCoApp2Status.Text = "";
            hdnCoApp2Status.Value = "Y";
            lblCoApp2Name.Text = "";
            lblCustNameCA2.Text = "";

            txtPanNoCA3.Text = "";
            lblPanStatusCA3.Text = "";
            txtVoterIdCA3.Text = "";
            lblVoterStatusCA3.Text = "";
            lblDdupStatusCA3.Text = "";
            fuFcuReportCA3.Controls.Clear();
            fuElecBillCA3.Controls.Clear();
            lblCoApp3Status.Text = "";
            hdnCoApp3Status.Value = "Y";
            lblCoApp3Name.Text = "";
            lblCustNameCA3.Text = "";

            txtPanNoCA4.Text = "";
            lblPanStatusCA4.Text = "";
            txtVoterIdCA4.Text = "";
            lblVoterStatusCA4.Text = "";
            lblDdupStatusCA4.Text = "";
            fuFcuReportCA4.Controls.Clear();
            fuElecBillCA4.Controls.Clear();
            lblCoApp4Status.Text = "";
            hdnCoApp4Status.Value = "Y";
            lblCoApp4Name.Text = "";
            lblCustNameCA4.Text = "";

            txtPanNoG.Text = "";
            lblPanStatusG.Text = "";
            txtVoterIdG.Text = "";
            lblVoterStatusG.Text = "";
            lblDdupStatusG.Text = "";
            fuFcuReportG.Controls.Clear();
            fuElecBillG.Controls.Clear();
            lblGuarantorStatus.Text = "";
            hdnGuarantorStatus.Value = "Y";
            lblGuarantorName.Text = "";
            lblCustNameG.Text = "";

            lblElecBill.Text = "";
            lblFcuRpt.Text = "";
            lblElecBillCA1.Text = "";
            lblFcuRptCA1.Text = "";
            lblElecBillCA2.Text = "";
            lblFcuRptCA2.Text = "";
            lblElecBillCA3.Text = "";
            lblFcuRptCA3.Text = "";
            lblElecBillCA4.Text = "";
            lblFcuRptCA4.Text = "";
            lblElecBillG.Text = "";
            lblFcuRptG.Text = "";

            txtFCUDt.Text = "";
            txtFCUExDt.Text = "";
            txtFCUDtCA1.Text = "";
            txtFCUExDtCA1.Text = "";
            txtFCUDtCA2.Text = "";
            txtFCUExDtCA2.Text = "";
            txtFCUDtCA3.Text = "";
            txtFCUExDtCA3.Text = "";
            txtFCUDtCA4.Text = "";
            txtFCUExDtCA4.Text = "";
            txtFCUDtG.Text = "";
            txtFCUExDtG.Text = "";

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
                //btnSave.Enabled = true;
                btnEdit.Enabled = true;
            }
        }
        private void CF_GetCust360()
        {
            DataTable dt = new DataTable();
            CCust360 oMem = new CCust360();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetCust360(vBrCode);
                ListItem oli1 = new ListItem("<--Select-->", "-1");

                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "Member";
                ddlCustomer.DataValueField = "MemberID";
                ddlCustomer.DataBind();
                ddlCustomer.Items.Insert(0, oli1);
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
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null, dt1 = null, dt2 = null, dt3 = null;
            CCust360 oRO = null;
            DataSet ds = new DataSet();
            string vMemberID = ddlCustomer.SelectedValue;
            string vCustNm = ddlCustomer.SelectedItem.Text;
            try
            {
                oRO = new CCust360();
                ds = oRO.CF_GetCust360IntChk(vMemberID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                if (dt.Rows.Count > 0)
                {
                    hdnLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                    hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                    txtPanNo.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                    lblPanStatus.Text = Convert.ToString(dt.Rows[0]["PanVerifyPF"]);
                    txtVoterId.Text = Convert.ToString(dt.Rows[0]["VoterNo"]);
                    lblVoterStatus.Text = Convert.ToString(dt.Rows[0]["VoterVerifyPF"]);
                    lblDdupStatus.Text = Convert.ToString(dt.Rows[0]["DdupPF"]);
                    lblBCPNum.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);

                    //FOR CO-APPLICANT-1
                    if (dt1.Rows.Count > 0)
                    {
                        txtPanNoCA1.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                        lblPanStatusCA1.Text = Convert.ToString(dt1.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA1.Text = Convert.ToString(dt1.Rows[0]["VoterNo"]);
                        lblVoterStatusCA1.Text = Convert.ToString(dt1.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA1.Text = Convert.ToString(dt1.Rows[0]["DdupPF"]);
                        lblCustNameCA1.Text = vCustNm;
                        lblCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                    }
                    else
                    {
                        txtPanNoCA1.Text = "";
                        lblPanStatusCA1.Text = "";
                        txtVoterIdCA1.Text = "";
                        lblVoterStatusCA1.Text = "";
                        lblDdupStatusCA1.Text = "";
                        lblCoApp1Name.Text = "";
                    }
                    //FOR CO-APPLICANT-2
                    if (dt2.Rows.Count > 0)
                    {
                        txtPanNoCA2.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                        lblPanStatusCA2.Text = Convert.ToString(dt2.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA2.Text = Convert.ToString(dt2.Rows[0]["VoterNo"]);
                        lblVoterStatusCA2.Text = Convert.ToString(dt2.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA2.Text = Convert.ToString(dt2.Rows[0]["DdupPF"]);
                        lblCustNameCA2.Text = vCustNm;
                        lblCoApp2Name.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                    }
                    else
                    {
                        txtPanNoCA2.Text = "";
                        lblPanStatusCA2.Text = "";
                        txtVoterIdCA2.Text = "";
                        lblVoterStatusCA2.Text = "";
                        lblDdupStatusCA2.Text = "";
                        lblCoApp2Name.Text = "";
                    }
                    //FOR GUARANTOR
                    if (dt3.Rows.Count > 0)
                    {
                        txtPanNoG.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                        lblPanStatusG.Text = Convert.ToString(dt3.Rows[0]["PanVerifyPF"]);
                        txtVoterIdG.Text = Convert.ToString(dt3.Rows[0]["VoterNo"]);
                        lblVoterStatusG.Text = Convert.ToString(dt3.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusG.Text = Convert.ToString(dt3.Rows[0]["DdupPF"]);
                        lblCustNameG.Text = vCustNm;
                        lblGuarantorName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                    }
                    else
                    {
                        txtPanNoG.Text = "";
                        lblPanStatusG.Text = "";
                        txtVoterIdG.Text = "";
                        lblVoterStatusG.Text = "";
                        lblDdupStatusG.Text = "";
                        lblGuarantorName.Text = "";
                    }

                }
                else
                {
                    hdnLeadId.Value = "";
                    hdnAppId.Value = "";
                    txtPanNo.Text = "";
                    lblPanStatus.Text = "";
                    txtVoterId.Text = "";
                    lblVoterStatus.Text = "";
                    lblDdupStatus.Text = "";
                }
            }
            finally
            {
                oRO = null;
                dt = null;
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
                tbIntChk.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                ImgElecBill.Enabled = false;
                imgFcuReport.Enabled = false;
                CF_GetCust360();
                ddlAmlStatus.Enabled = false;
                ddlAmlStatusCA1.Enabled = false;
                ddlAmlStatusCA2.Enabled = false;
                ddlAmlStatusCA3.Enabled = false;
                ddlAmlStatusCA4.Enabled = false;
                ddlAmlStatusG.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
                ddlCustomer.Enabled = false;
                CoApp1StatusCheck(hdnCoApp1Status.Value);
                CoApp2StatusCheck(hdnCoApp2Status.Value);
                CoApp3StatusCheck(hdnCoApp3Status.Value);
                CoApp4StatusCheck(hdnCoApp4Status.Value);
                GStatusCheck(hdnGuarantorStatus.Value);
                ddlAmlStatus.Enabled = false;
                ddlAmlStatusCA1.Enabled = false;
                ddlAmlStatusCA2.Enabled = false;
                ddlAmlStatusCA3.Enabled = false;
                ddlAmlStatusCA4.Enabled = false;
                ddlAmlStatusG.Enabled = false;

                btnValidateVkyc.Enabled = true;
                btnValidateVkycCA1.Enabled = true;
                btnValidateVkycCA2.Enabled = true;
                btnValidateVkycCA3.Enabled = true;
                btnValidateVkycCA4.Enabled = true;
                btnValidateVkycG.Enabled = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbIntChk.ActiveTabIndex = 1;
            //  EnableControl(false);
            StatusButton("Show");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                GetInternalChecks();
                StatusButton("Show");
                ViewState["StateEdit"] = null;
                int activeTabIndex;
                if (int.TryParse(hdActiveTab.Value, out activeTabIndex))
                {
                    tbIntChk.ActiveTabIndex = activeTabIndex;
                }
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0; string vCustType = ""; string vElecBillUplPF = "F"; string vElecBillUplYN = "N"; string vElecBillFileName = ""; string vElecBillFileExt = "";
            FileUpload vElecBillFileUploader = null; FileUpload vFcuRptUploader = null; CCust360 oMem = null; string vElecBillFileStorePath = "";
            string vFcuUplYN = "N"; string vFcuFileName = ""; string vFcuFileExt = ""; string vFcuFileStorePath = ""; string vFcuStatus = "";
            string vAmlStatus = "F"; string vVkycStatus = "F"; Int32 vMaxFileSize = Convert.ToInt32(MaxFileSize);
            DateTime vFCUDt = gblFuction.setDate(txtFCUDt.Text.ToString());
            DateTime vFCUExDt = gblFuction.setDate(txtFCUExDt.Text.ToString());

            string vCustTypeCA1 = ""; string vElecBillUplPFCA1 = "F"; string vElecBillFileNameCA1 = ""; string vElecBillFileExtCA1 = "";
            FileUpload vElecBillFileUploaderCA1 = null; FileUpload vFcuRptUploaderCA1 = null; string vElecBillFileStorePathCA1 = "";
            string vFcuUplYNCA1 = "N"; string vFcuFileNameCA1 = ""; string vFcuFileExtCA1 = ""; string vFcuFileStorePathCA1 = ""; string vFcuStatusCA1 = "";
            string vAmlStatusCA1 = "F"; string vVkycStatusCA1 = "F"; string vElecBillUplYNCA1 = "N";
            DateTime vFCUDtCA1 = gblFuction.setDate(txtFCUDtCA1.Text.ToString());
            DateTime vFCUExDtCA1 = gblFuction.setDate(txtFCUExDtCA1.Text.ToString());

            string vCustTypeCA2 = ""; string vElecBillUplPFCA2 = "F"; string vElecBillFileNameCA2 = ""; string vElecBillFileExtCA2 = "";
            FileUpload vElecBillFileUploaderCA2 = null; FileUpload vFcuRptUploaderCA2 = null; string vElecBillFileStorePathCA2 = "";
            string vFcuUplYNCA2 = "N"; string vFcuFileNameCA2 = ""; string vFcuFileExtCA2 = ""; string vFcuFileStorePathCA2 = ""; string vFcuStatusCA2 = "";
            string vAmlStatusCA2 = "F"; string vVkycStatusCA2 = "F"; string vElecBillUplYNCA2 = "N";
            DateTime vFCUDtCA2 = gblFuction.setDate(txtFCUDtCA2.Text.ToString());
            DateTime vFCUExDtCA2 = gblFuction.setDate(txtFCUExDtCA2.Text.ToString());

            string vCustTypeCA3 = ""; string vElecBillUplPFCA3 = "F"; string vElecBillFileNameCA3 = ""; string vElecBillFileExtCA3 = "";
            FileUpload vElecBillFileUploaderCA3 = null; FileUpload vFcuRptUploaderCA3 = null; string vElecBillFileStorePathCA3 = "";
            string vFcuUplYNCA3 = "N"; string vFcuFileNameCA3 = ""; string vFcuFileExtCA3 = ""; string vFcuFileStorePathCA3 = ""; string vFcuStatusCA3 = "";
            string vAmlStatusCA3 = "F"; string vVkycStatusCA3 = "F"; string vElecBillUplYNCA3 = "N";
            DateTime vFCUDtCA3 = gblFuction.setDate(txtFCUDtCA3.Text.ToString());
            DateTime vFCUExDtCA3 = gblFuction.setDate(txtFCUExDtCA3.Text.ToString());

            string vCustTypeCA4 = ""; string vElecBillUplPFCA4 = "F"; string vElecBillFileNameCA4 = ""; string vElecBillFileExtCA4 = "";
            FileUpload vElecBillFileUploaderCA4 = null; FileUpload vFcuRptUploaderCA4 = null; string vElecBillFileStorePathCA4 = "";
            string vFcuUplYNCA4 = "N"; string vFcuFileNameCA4 = ""; string vFcuFileExtCA4 = ""; string vFcuFileStorePathCA4 = ""; string vFcuStatusCA4 = "";
            string vAmlStatusCA4 = "F"; string vVkycStatusCA4 = "F"; string vElecBillUplYNCA4 = "N";
            DateTime vFCUDtCA4 = gblFuction.setDate(txtFCUDtCA4.Text.ToString());
            DateTime vFCUExDtCA4 = gblFuction.setDate(txtFCUExDtCA4.Text.ToString());

            string vCustTypeG = ""; string vElecBillUplPFG = "F"; string vElecBillFileNameG = ""; string vElecBillFileExtG = "";
            FileUpload vElecBillFileUploaderG = null; FileUpload vFcuRptUploaderG = null; string vElecBillFileStorePathG = "";
            string vFcuUplYNG = "N"; string vFcuFileNameG = ""; string vFcuFileExtG = ""; string vFcuFileStorePathG = ""; string vFcuStatusG = "";
            string vAmlStatusG = "F"; string vVkycStatusG = "F"; string vElecBillUplYNG = "N";
            DateTime vFCUDtG = gblFuction.setDate(txtFCUDtG.Text.ToString());
            DateTime vFCUExDtG = gblFuction.setDate(txtFCUExDtG.Text.ToString());
            hdActiveTab.Value = "0";

            try
            {
                if (tbIntChk.ActiveTabIndex == 0) hdActiveTab.Value = "0";
                else if (tbIntChk.ActiveTabIndex == 1) hdActiveTab.Value = "1";
                else if (tbIntChk.ActiveTabIndex == 2) hdActiveTab.Value = "2";
                else if (tbIntChk.ActiveTabIndex == 3) hdActiveTab.Value = "3";
                else if (tbIntChk.ActiveTabIndex == 4) hdActiveTab.Value = "4";
                else if (tbIntChk.ActiveTabIndex == 5) hdActiveTab.Value = "5";


                #region APPLICANT
                vAmlStatus = ddlAmlStatus.SelectedValue;
                vVkycStatus = ddlVKycStatus.SelectedValue;

                vCustType = "A";


                vElecBillFileUploader = fuElecBill;
                vElecBillUplYN = fuElecBill.HasFile == true ? "Y" : "N";
                if (vElecBillUplYN == "Y")
                {
                    vElecBillFileName = fuElecBill.PostedFile.FileName.ToString();
                    vElecBillFileExt = System.IO.Path.GetExtension(fuElecBill.PostedFile.FileName);
                    vElecBillFileStorePath = BucketURL + CFDocumentBucket;

                    if ((vElecBillFileExt.ToLower() != ".pdf") && (vElecBillFileExt.ToLower() != ".xlx") && (vElecBillFileExt.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExt = ".png";
                    }

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBill.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBill.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                }
                else
                {
                    if (lblElecBill.Text == "")
                    {
                        vElecBillFileExt = "";
                        vElecBillUplYN = "N";

                    }
                    else
                    {
                        vElecBillFileExt = hdnElecBillExt.Value;
                        vElecBillUplYN = "Y";
                    }
                }


                vElecBillUplPF = ddlElecBillStat.SelectedValue;



                vFcuRptUploader = fuFcuReport;
                vFcuUplYN = fuFcuReport.HasFile == true ? "Y" : "N";
                if (vFcuUplYN == "Y")
                {
                    vFcuFileName = fuFcuReport.PostedFile.FileName.ToString();
                    vFcuFileExt = System.IO.Path.GetExtension(fuFcuReport.PostedFile.FileName);
                    vFcuFileStorePath = BucketURL + CFDocumentBucket;
                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReport.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReport.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                    if ((vFcuFileExt.ToLower() != ".pdf") && (vFcuFileExt.ToLower() != ".xlx") && (vFcuFileExt.ToLower() != ".xlsx"))
                    {
                        vFcuFileExt = ".png";
                    }
                }
                else
                {
                    if (lblFcuRpt.Text == "")
                    {
                        vFcuFileExt = "";
                        vFcuUplYN = "N";
                    }
                    else
                    {
                        vFcuFileExt = hdnFcuReportExt.Value;
                        vFcuUplYN = "Y";
                    }
                }



                vFcuStatus = ddlFCUStatus.SelectedValue;
                #endregion

                #region Co-APPLICANT 1
                vAmlStatusCA1 = ddlAmlStatusCA1.SelectedValue;
                vVkycStatusCA1 = ddlVKycStatusCA1.SelectedValue;

                vCustTypeCA1 = "CA1";


                vElecBillFileUploaderCA1 = fuElecBillCA1;
                vElecBillUplYNCA1 = fuElecBillCA1.HasFile == true ? "Y" : "N";
                if (vElecBillUplYNCA1 == "Y")
                {
                    vElecBillFileNameCA1 = fuElecBillCA1.PostedFile.FileName.ToString();
                    vElecBillFileExtCA1 = System.IO.Path.GetExtension(fuElecBillCA1.PostedFile.FileName);
                    vElecBillFileStorePathCA1 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExtCA1.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBillCA1.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBillCA1.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                    if ((vElecBillFileExtCA1.ToLower() != ".pdf") && (vElecBillFileExtCA1.ToLower() != ".xlx") && (vElecBillFileExtCA1.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExtCA1 = ".png";
                    }
                }
                else
                {
                    if (lblElecBillCA1.Text == "")
                    {
                        vElecBillFileExtCA1 = "";
                        vElecBillUplYNCA1 = "N";
                    }
                    else
                    {
                        vElecBillFileExtCA1 = hdnElecBillExtCA1.Value;
                        vElecBillUplYNCA1 = "Y";
                    }
                }



                vElecBillUplPFCA1 = ddlElecBillStatCA1.SelectedValue;



                vFcuRptUploaderCA1 = fuFcuReportCA1;
                vFcuUplYNCA1 = fuFcuReportCA1.HasFile == true ? "Y" : "N";
                if (vFcuUplYNCA1 == "Y")
                {
                    vFcuFileNameCA1 = fuFcuReportCA1.PostedFile.FileName.ToString();
                    vFcuFileExtCA1 = System.IO.Path.GetExtension(fuFcuReportCA1.PostedFile.FileName);
                    vFcuFileStorePathCA1 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExtCA1.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReportCA1.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReportCA1.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vFcuFileExtCA1.ToLower() != ".pdf") && (vFcuFileExtCA1.ToLower() != ".xlx") && (vFcuFileExtCA1.ToLower() != ".xlsx"))
                    {
                        vFcuFileExtCA1 = ".png";
                    }
                }
                else
                {
                    if (lblFcuRptCA1.Text == "")
                    {
                        vFcuFileExtCA1 = "";
                        vFcuUplYNCA1 = "N";
                    }
                    else
                    {
                        vFcuFileExtCA1 = hdnFcuReportExtCA1.Value;
                        vFcuUplYNCA1 = "Y";
                    }
                }

                vFcuStatusCA1 = ddlFCUStatusCA1.SelectedValue;
                #endregion

                #region Co-APPLICANT 2
                vAmlStatusCA2 = ddlAmlStatusCA2.SelectedValue;
                vVkycStatusCA2 = ddlVKycStatusCA2.SelectedValue;

                vCustTypeCA2 = "CA2";


                vElecBillFileUploaderCA2 = fuElecBillCA2;
                vElecBillUplYNCA2 = fuElecBillCA2.HasFile == true ? "Y" : "N";
                if (vElecBillUplYNCA2 == "Y")
                {
                    vElecBillFileNameCA2 = fuElecBillCA2.PostedFile.FileName.ToString();
                    vElecBillFileExtCA2 = System.IO.Path.GetExtension(fuElecBillCA2.PostedFile.FileName);
                    vElecBillFileStorePathCA2 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExtCA2.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBillCA2.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBillCA2.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vElecBillFileExtCA2.ToLower() != ".pdf") && (vElecBillFileExtCA2.ToLower() != ".xlx") && (vElecBillFileExtCA2.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExtCA2 = ".png";
                    }
                }
                else
                {
                    if (lblElecBillCA2.Text == "")
                    {
                        vElecBillFileExtCA2 = "";
                        vElecBillUplYNCA2 = "N";
                    }
                    else
                    {
                        vElecBillFileExtCA2 = hdnElecBillExtCA2.Value;
                        vElecBillUplYNCA2 = "Y";
                    }
                }

                vElecBillUplPFCA2 = ddlElecBillStatCA2.SelectedValue;



                vFcuRptUploaderCA2 = fuFcuReportCA2;
                vFcuUplYNCA2 = fuFcuReportCA2.HasFile == true ? "Y" : "N";
                if (vFcuUplYNCA2 == "Y")
                {
                    vFcuFileNameCA2 = fuFcuReportCA2.PostedFile.FileName.ToString();
                    vFcuFileExtCA2 = System.IO.Path.GetExtension(fuFcuReportCA2.PostedFile.FileName);
                    vFcuFileStorePathCA2 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExtCA2.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReportCA2.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReportCA2.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vFcuFileExtCA2.ToLower() != ".pdf") && (vFcuFileExtCA2.ToLower() != ".xlx") && (vFcuFileExtCA2.ToLower() != ".xlsx"))
                    {
                        vFcuFileExtCA2 = ".png";
                    }
                }
                else
                {
                    if (lblFcuRptCA2.Text == "")
                    {
                        vFcuFileExtCA2 = "";
                        vFcuUplYNCA2 = "N";
                    }
                    else
                    {
                        vFcuFileExtCA2 = hdnFcuReportExtCA2.Value;
                        vFcuUplYNCA2 = "Y";
                    }

                }

                vFcuStatusCA2 = ddlFCUStatusCA2.SelectedValue;
                #endregion

                #region Co-APPLICANT 3
                vAmlStatusCA3 = ddlAmlStatusCA3.SelectedValue;
                vVkycStatusCA3 = ddlVKycStatusCA3.SelectedValue;

                vCustTypeCA3 = "CA3";


                vElecBillFileUploaderCA3 = fuElecBillCA3;
                vElecBillUplYNCA3 = fuElecBillCA3.HasFile == true ? "Y" : "N";
                if (vElecBillUplYNCA3 == "Y")
                {
                    vElecBillFileNameCA3 = fuElecBillCA3.PostedFile.FileName.ToString();
                    vElecBillFileExtCA3 = System.IO.Path.GetExtension(fuElecBillCA3.PostedFile.FileName);
                    vElecBillFileStorePathCA3 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExtCA3.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBillCA3.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBillCA3.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vElecBillFileExtCA3.ToLower() != ".pdf") && (vElecBillFileExtCA3.ToLower() != ".xlx") && (vElecBillFileExtCA3.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExtCA3 = ".png";
                    }
                }
                else
                {
                    if (lblElecBillCA3.Text == "")
                    {
                        vElecBillFileExtCA3 = "";
                        vElecBillUplYNCA3 = "N";
                    }
                    else
                    {
                        vElecBillFileExtCA3 = hdnElecBillExtCA3.Value;
                        vElecBillUplYNCA3 = "Y";
                    }
                }

                vElecBillUplPFCA3 = ddlElecBillStatCA3.SelectedValue;



                vFcuRptUploaderCA3 = fuFcuReportCA3;
                vFcuUplYNCA3 = fuFcuReportCA3.HasFile == true ? "Y" : "N";
                if (vFcuUplYNCA3 == "Y")
                {
                    vFcuFileNameCA3 = fuFcuReportCA3.PostedFile.FileName.ToString();
                    vFcuFileExtCA3 = System.IO.Path.GetExtension(fuFcuReportCA3.PostedFile.FileName);
                    vFcuFileStorePathCA3 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExtCA3.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReportCA3.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReportCA3.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vFcuFileExtCA3.ToLower() != ".pdf") && (vFcuFileExtCA3.ToLower() != ".xlx") && (vFcuFileExtCA3.ToLower() != ".xlsx"))
                    {
                        vFcuFileExtCA3 = ".png";
                    }
                }
                else
                {
                    if (lblFcuRptCA3.Text == "")
                    {
                        vFcuFileExtCA3 = "";
                        vFcuUplYNCA3 = "N";
                    }
                    else
                    {
                        vFcuFileExtCA3 = hdnFcuReportExtCA3.Value;
                        vFcuUplYNCA3 = "Y";
                    }

                }

                vFcuStatusCA3 = ddlFCUStatusCA3.SelectedValue;
                #endregion

                #region Co-APPLICANT 4
                vAmlStatusCA4 = ddlAmlStatusCA4.SelectedValue;
                vVkycStatusCA4 = ddlVKycStatusCA4.SelectedValue;

                vCustTypeCA4 = "CA4";


                vElecBillFileUploaderCA4 = fuElecBillCA4;
                vElecBillUplYNCA4 = fuElecBillCA4.HasFile == true ? "Y" : "N";
                if (vElecBillUplYNCA4 == "Y")
                {
                    vElecBillFileNameCA4 = fuElecBillCA4.PostedFile.FileName.ToString();
                    vElecBillFileExtCA4 = System.IO.Path.GetExtension(fuElecBillCA4.PostedFile.FileName);
                    vElecBillFileStorePathCA4 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExtCA4.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBillCA4.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBillCA4.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vElecBillFileExtCA4.ToLower() != ".pdf") && (vElecBillFileExtCA4.ToLower() != ".xlx") && (vElecBillFileExtCA4.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExtCA4 = ".png";
                    }
                }
                else
                {
                    if (lblElecBillCA4.Text == "")
                    {
                        vElecBillFileExtCA4 = "";
                        vElecBillUplYNCA4 = "N";
                    }
                    else
                    {
                        vElecBillFileExtCA4 = hdnElecBillExtCA4.Value;
                        vElecBillUplYNCA4 = "Y";
                    }
                }

                vElecBillUplPFCA4 = ddlElecBillStatCA4.SelectedValue;



                vFcuRptUploaderCA4 = fuFcuReportCA4;
                vFcuUplYNCA4 = fuFcuReportCA4.HasFile == true ? "Y" : "N";
                if (vFcuUplYNCA4 == "Y")
                {
                    vFcuFileNameCA4 = fuFcuReportCA4.PostedFile.FileName.ToString();
                    vFcuFileExtCA4 = System.IO.Path.GetExtension(fuFcuReportCA4.PostedFile.FileName);
                    vFcuFileStorePathCA4 = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExtCA4.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReportCA4.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReportCA4.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vFcuFileExtCA4.ToLower() != ".pdf") && (vFcuFileExtCA4.ToLower() != ".xlx") && (vFcuFileExtCA4.ToLower() != ".xlsx"))
                    {
                        vFcuFileExtCA4 = ".png";
                    }
                }
                else
                {
                    if (lblFcuRptCA4.Text == "")
                    {
                        vFcuFileExtCA4 = "";
                        vFcuUplYNCA4 = "N";
                    }
                    else
                    {
                        vFcuFileExtCA4 = hdnFcuReportExtCA4.Value;
                        vFcuUplYNCA4 = "Y";
                    }

                }

                vFcuStatusCA4 = ddlFCUStatusCA4.SelectedValue;
                #endregion

                #region GUARANTOR
                vAmlStatusG = ddlAmlStatusG.SelectedValue;
                vVkycStatusG = ddlVKycStatusG.SelectedValue;

                vCustTypeG = "G";


                vElecBillFileUploaderG = fuElecBillG;
                vElecBillUplYNG = fuElecBillG.HasFile == true ? "Y" : "N";
                if (vElecBillUplYNG == "Y")
                {
                    vElecBillFileNameG = fuElecBillG.PostedFile.FileName.ToString();
                    vElecBillFileExtG = System.IO.Path.GetExtension(fuElecBillG.PostedFile.FileName);
                    vElecBillFileStorePathG = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vElecBillFileExtG.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuElecBillG.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuElecBillG.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vElecBillFileExtG.ToLower() != ".pdf") && (vElecBillFileExtG.ToLower() != ".xlx") && (vElecBillFileExtG.ToLower() != ".xlsx"))
                    {
                        vElecBillFileExtG = ".png";
                    }

                }
                else
                {
                    if (lblElecBillG.Text == "")
                    {
                        vElecBillFileExtG = "";
                        vElecBillUplYNG = "N";
                    }
                    else
                    {
                        vElecBillFileExtG = hdnElecBillExtG.Value;
                        vElecBillUplYNG = "Y";
                    }
                }

                vElecBillUplPFG = ddlElecBillStatG.SelectedValue;



                vFcuRptUploaderG = fuFcuReportG;
                vFcuUplYNG = fuFcuReportG.HasFile == true ? "Y" : "N";
                if (vFcuUplYNG == "Y")
                {
                    vFcuFileNameG = fuFcuReportG.PostedFile.FileName.ToString();
                    vFcuFileExtG = System.IO.Path.GetExtension(fuFcuReportG.PostedFile.FileName);
                    vFcuFileStorePathG = BucketURL + CFDocumentBucket;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFcuFileExtG.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuFcuReportG.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuFcuReportG.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------

                    if ((vFcuFileExtG.ToLower() != ".pdf") && (vFcuFileExtG.ToLower() != ".xlx") && (vFcuFileExtG.ToLower() != ".xlsx"))
                    {
                        vFcuFileExtG = ".png";
                    }
                }
                else
                {
                    if (lblFcuRptG.Text == "")
                    {
                    }
                    else
                    {
                        vFcuFileExtG = hdnFcuReportExtG.Value;
                        vFcuUplYNG = "Y";
                    }
                }

                vFcuStatusG = ddlFCUStatusG.SelectedValue;
                #endregion

                oMem = new CCust360();
                vErr = oMem.CF_UpdateCustIntChk(hdnAppId.Value, vCustType, vVkycStatus, vAmlStatus, vElecBillUplPF, vElecBillFileExt, vElecBillFileStorePath,
                    vFcuUplYN, vFcuFileExt, vFcuFileStorePath, vFcuStatus, Convert.ToInt32(Session[gblValue.UserId]), vElecBillUplYN,
                    vCustTypeCA1, vVkycStatusCA1, vAmlStatusCA1, vElecBillUplPFCA1, vElecBillFileExtCA1, vElecBillFileStorePathCA1,
                    vFcuUplYNCA1, vFcuFileExtCA1, vFcuFileStorePathCA1, vFcuStatusCA1, vElecBillUplYNCA1,

                    vCustTypeCA2, vVkycStatusCA2, vAmlStatusCA2, vElecBillUplPFCA2, vElecBillFileExtCA2, vElecBillFileStorePathCA2,
                    vFcuUplYNCA2, vFcuFileExtCA2, vFcuFileStorePathCA2, vFcuStatusCA2, vElecBillUplYNCA2,

                    vCustTypeG, vVkycStatusG, vAmlStatusG, vElecBillUplPFG, vElecBillFileExtG, vElecBillFileStorePathG,
                    vFcuUplYNG, vFcuFileExtG, vFcuFileStorePathG, vFcuStatusG, vElecBillUplYNG, vFCUDt, vFCUExDt, vFCUDtCA1, vFCUExDtCA1,
                    vFCUDtCA2, vFCUExDtCA2, vFCUDtG, vFCUExDtG,

                    vCustTypeCA3, vVkycStatusCA3, vAmlStatusCA3, vElecBillUplPFCA3, vElecBillFileExtCA3, vElecBillFileStorePathCA3,
                    vFcuUplYNCA3, vFcuFileExtCA3, vFcuFileStorePathCA3, vFcuStatusCA3, vElecBillUplYNCA3, vFCUDtCA3, vFCUExDtCA3,

                    vCustTypeCA4, vVkycStatusCA4, vAmlStatusCA4, vElecBillUplPFCA4, vElecBillFileExtCA4, vElecBillFileStorePathCA4,
                    vFcuUplYNCA4, vFcuFileExtCA4, vFcuFileStorePathCA4, vFcuStatusCA4, vElecBillUplYNCA4, vFCUDtCA4, vFCUExDtCA4);

                if (vErr > 0)
                {
                    if (vElecBillFileExt.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBill.HasFile)
                        {
                            SaveMemberImages(fuElecBill, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustType + "_" + "ElectricBill", vElecBillFileExt, "N");
                        }
                    }
                    else
                    {
                        if (fuElecBill.HasFile)
                        {
                            SaveMemberImages(fuElecBill, hdnLeadId.Value, vCustType + "_" + "ElectricBill", vElecBillFileExt, "N");
                        }
                    }

                    if (vFcuFileExt.ToLower().Contains(".pdf"))
                    {
                        if (fuFcuReport.HasFile)
                        {
                            SaveMemberImages(fuFcuReport, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustType + "_" + "FCUReport", vFcuFileExt, "N");

                        }
                    }
                    else
                    {
                        if (fuFcuReport.HasFile)
                        {
                            SaveMemberImages(fuFcuReport, hdnLeadId.Value, vCustType + "_" + "FCUReport", vFcuFileExt, "N");
                        }
                    }
                    if (vElecBillFileExtCA1.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBillCA1.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA1, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA1 + "_" + "ElectricBill", vElecBillFileExtCA1, "N");
                        }
                    }
                    else
                    {
                        if (fuElecBillCA1.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA1, hdnLeadId.Value, vCustTypeCA1 + "_" + "ElectricBill", vElecBillFileExtCA1, "N");
                        }
                    }

                    if (vElecBillFileExtCA2.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBillCA2.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA2, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA2 + "_" + "ElectricBill", vElecBillFileExtCA2, "N");
                        }
                    }
                    else
                    {
                        if (fuElecBillCA2.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA2, hdnLeadId.Value, vCustTypeCA2 + "_" + "ElectricBill", vElecBillFileExtCA2, "N");
                        }
                    }

                    if (vElecBillFileExtCA3.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBillCA3.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA3, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA3 + "_" + "ElectricBill", vElecBillFileExtCA3, "N");
                        }
                    }
                    else
                    {
                        if (fuElecBillCA3.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA3, hdnLeadId.Value, vCustTypeCA3 + "_" + "ElectricBill", vElecBillFileExtCA3, "N");
                        }
                    }
                    if (vElecBillFileExtCA4.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBillCA4.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA4, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA4 + "_" + "ElectricBill", vElecBillFileExtCA4, "N");
                        }
                    }
                    else
                    {
                        if (fuElecBillCA4.HasFile)
                        {
                            SaveMemberImages(fuElecBillCA4, hdnLeadId.Value, vCustTypeCA4 + "_" + "ElectricBill", vElecBillFileExtCA4, "N");
                        }
                    }

                    if (vElecBillFileExtG.ToLower().Contains(".pdf"))
                    {
                        if (fuElecBillG.HasFile)
                        {
                            SaveMemberImages(fuElecBillG, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeG + "_" + "ElectricBill", vElecBillFileExtG, "N");

                        }
                    }
                    else
                    {
                        if (fuElecBillG.HasFile)
                        {
                            SaveMemberImages(fuElecBillG, hdnLeadId.Value, vCustTypeG + "_" + "ElectricBill", vElecBillFileExtG, "N");
                        }
                    }

                    if (vFcuFileExtCA1.ToLower().Contains(".pdf"))
                    {
                        if (fuFcuReportCA1.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA1, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA1 + "_" + "FCUReport", vFcuFileExtCA1, "N");
                        }
                    }
                    else
                    {
                        if (fuFcuReportCA1.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA1, hdnLeadId.Value, vCustTypeCA1 + "_" + "FCUReport", vFcuFileExtCA1, "N");
                        }
                    }

                    if (vFcuFileExtCA2.ToLower().Contains(".pdf"))
                    {
                        if (fuFcuReportCA2.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA2, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA2 + "_" + "FCUReport", vFcuFileExtCA2, "N");
                        }
                    }
                    else
                    {
                        if (fuFcuReportCA2.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA2, hdnLeadId.Value, vCustTypeCA2 + "_" + "FCUReport", vFcuFileExtCA2, "N");
                        }
                    }
                    if (vFcuFileExtCA3.ToLower().Contains(".pdf"))
                    {
                        if (fuFcuReportCA3.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA3, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA3 + "_" + "FCUReport", vFcuFileExtCA3, "N");
                        }
                    }
                    else
                    {
                        if (fuFcuReportCA3.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA3, hdnLeadId.Value, vCustTypeCA3 + "_" + "FCUReport", vFcuFileExtCA3, "N");
                        }
                    }
                    if (vFcuFileExtCA4.ToLower().Contains(".pdf"))
                    {
                        if (fuFcuReportCA4.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA4, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeCA4 + "_" + "FCUReport", vFcuFileExtCA4, "N");
                        }
                    }
                    else
                    {
                        if (fuFcuReportCA4.HasFile)
                        {
                            SaveMemberImages(fuFcuReportCA4, hdnLeadId.Value, vCustTypeCA4 + "_" + "FCUReport", vFcuFileExtCA4, "N");
                        }
                    }

                    if (vFcuFileExtG.ToLower().Contains(".png"))
                    {
                        if (fuFcuReportG.HasFile)
                        {
                            SaveMemberImages(fuFcuReportG, hdnLeadId.Value, hdnLeadId.Value + "_" + vCustTypeG + "_" + "FCUReport", vFcuFileExtG, "N");
                        }
                    }
                    else
                    {
                        if (fuFcuReportG.HasFile)
                        {
                            SaveMemberImages(fuFcuReportG, hdnLeadId.Value, vCustTypeG + "_" + "FCUReport", vFcuFileExtG, "N");
                        }
                    }


                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
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
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, CFDocumentBucket, MinioUrl);
            return isImageSaved;
        }
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public string GetBase64Image(string pImageName, string pLeadId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pLeadId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }
        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion
        #region ConvertFileToByteArray
        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        #endregion
        protected void GetInternalChecks()
        {
            string pMemberId = ""; 
            string vBrCode = "";
            DataTable dt, dt1, dt2, dt3 = null, dt4 = null, dt5 = null;
            DataSet ds = new DataSet();
            ClearControls();
            if (Session[gblValue.BCPNO] != null)
            {
                lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
            }
            try
            {
                if (Session[gblValue.MemberID] != null)
                {
                    pMemberId = Convert.ToString(Session[gblValue.MemberID]);
                }
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["MemberId"] = pMemberId;

                CCust360 oC360 = new CCust360();
                ds = oC360.CF_GetCust360IntChk(pMemberId);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                dt4 = ds.Tables[4];
                dt5 = ds.Tables[5];

                if (dt.Rows.Count > 0)
                {
                    hdnLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                    hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                    txtPanNo.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                    lblPanStatus.Text = Convert.ToString(dt.Rows[0]["PanVerifyPF"]);
                    txtVoterId.Text = Convert.ToString(dt.Rows[0]["VoterNo"]);
                    lblVoterStatus.Text = Convert.ToString(dt.Rows[0]["VoterVerifyPF"]);
                    lblDdupStatus.Text = Convert.ToString(dt.Rows[0]["DdupPF"]);
                    hdnElecBillExt.Value = Convert.ToString(dt.Rows[0]["ElecBillExt"]);
                    hdnFcuReportExt.Value = Convert.ToString(dt.Rows[0]["FCURptExt"]);
                    txtFCUDt.Text = Convert.ToString(dt.Rows[0]["FCUDt"]);
                    txtFCUExDt.Text = Convert.ToString(dt.Rows[0]["FCUExDt"]);

                   
                    ddlCustomer.ClearSelection();
                    ListItem oli1 = new ListItem(Convert.ToString(dt.Rows[0]["Member"]), "-1");
                    ddlCustomer.Items.Insert(0, oli1);

                    if (Convert.ToString(dt.Rows[0]["ElecBillPF"]) == "Y")
                    {
                        lblElecBill.Text = "ElectricBill" + hdnElecBillExt.Value;
                    }
                    else
                    {
                        lblElecBill.Text = "";
                    }

                    if (Convert.ToString(dt.Rows[0]["FCURptUplYN"]) == "Y")
                    {
                        lblFcuRpt.Text = "FCUReport" + hdnFcuReportExt.Value;
                    }
                    else
                    {
                        lblFcuRpt.Text = "";
                    }

                    ddlAmlStatus.SelectedIndex = ddlAmlStatus.Items.IndexOf(ddlAmlStatus.Items.FindByText(dt.Rows[0]["AMLCheckPF"].ToString()));
                    if (Convert.ToString(dt.Rows[0]["AMLCheckPF"]) == "Pass")
                    {
                        hdnAmlPassYn.Value = "Y";
                        hdAppAMLFlag.Value = "Y";
                    }
                    else if (Convert.ToString(dt.Rows[0]["AMLCheckPF"]) == "Fail")
                    {
                        hdnAmlPassYn.Value = "Y";
                        hdAppAMLFlag.Value = "Y";
                    }
                    ddlVKycStatus.SelectedIndex = ddlVKycStatus.Items.IndexOf(ddlVKycStatus.Items.FindByText(dt.Rows[0]["VKYCVerifyYN"].ToString()));
                   
                    ddlElecBillStat.SelectedIndex = ddlElecBillStat.Items.IndexOf(ddlElecBillStat.Items.FindByText(dt.Rows[0]["ElecBillStatus"].ToString()));
                    ddlFCUStatus.SelectedIndex = ddlFCUStatus.Items.IndexOf(ddlFCUStatus.Items.FindByValue(dt.Rows[0]["FCUStatus"].ToString()));

                    if (Convert.ToString(dt.Rows[0]["VkycFlag"]) == "Y")
                    {
                        hdAppVkycYN.Value = "Y";
                    }
                    else
                    {
                        hdAppVkycYN.Value = "N";
                    }

                    StatusButton("Show");
                    tbIntChk.ActiveTabIndex = 1;
                    ImgElecBill.Enabled = true;
                    imgFcuReport.Enabled = true;
                    btnValidateVkyc.Enabled = false;

                    // FOR CO-APPLICANT 1
                    #region CO-APPLICANT 1
                    if (dt1.Rows.Count > 0)
                    {

                        txtPanNoCA1.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                        lblPanStatusCA1.Text = Convert.ToString(dt1.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA1.Text = Convert.ToString(dt1.Rows[0]["VoterNo"]);
                        lblVoterStatusCA1.Text = Convert.ToString(dt1.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA1.Text = Convert.ToString(dt1.Rows[0]["DdupPF"]);
                        hdnElecBillExtCA1.Value = Convert.ToString(dt1.Rows[0]["ElecBillExt"]);
                        hdnFcuReportExtCA1.Value = Convert.ToString(dt1.Rows[0]["FCURptExt"]);
                        lblCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                        txtFCUDtCA1.Text = Convert.ToString(dt1.Rows[0]["FCUDt"]);
                        txtFCUExDtCA1.Text = Convert.ToString(dt1.Rows[0]["FCUExDt"]);


                        lblCustNameCA1.Text = Convert.ToString(dt1.Rows[0]["Member"]);

                        if (Convert.ToString(dt1.Rows[0]["ElecBillPF"]) == "Y")
                        {
                            lblElecBillCA1.Text = "ElectricBill" + hdnElecBillExtCA1.Value;
                        }
                        else
                        {
                            lblElecBillCA1.Text = "";
                        }

                        if (Convert.ToString(dt1.Rows[0]["FCURptUplYN"]) == "Y")
                        {
                            lblFcuRptCA1.Text = "FCUReport" + hdnFcuReportExtCA1.Value;
                        }
                        else
                        {
                            lblFcuRptCA1.Text = "";
                        }

                        ddlAmlStatusCA1.SelectedIndex = ddlAmlStatusCA1.Items.IndexOf(ddlAmlStatusCA1.Items.FindByText(dt1.Rows[0]["AMLCheckPF"].ToString()));
                        if (Convert.ToString(dt1.Rows[0]["AMLCheckPF"]) == "Pass")
                        {
                            hdnAmlPassYnCA1.Value = "Y";
                            hdCoApp1AMLFlag.Value = "Y";
                        }
                        else if (Convert.ToString(dt1.Rows[0]["AMLCheckPF"]) == "Fail")
                        {
                            hdnAmlPassYnCA1.Value = "Y";
                            hdCoApp1AMLFlag.Value = "Y";
                        }
                        ddlVKycStatusCA1.SelectedIndex = ddlVKycStatusCA1.Items.IndexOf(ddlVKycStatusCA1.Items.FindByText(dt1.Rows[0]["VKYCVerifyYN"].ToString()));
                        //if (Convert.ToString(dt1.Rows[0]["VKYCVerifyYN"]) == "Pass")
                        //{
                        //    hdnValVKYCYNCA1.Value = "Y";
                        //}
                        //else
                        //{
                        //    hdnValVKYCYNCA1.Value = "N";
                        //}

                        ddlElecBillStatCA1.SelectedIndex = ddlElecBillStatCA1.Items.IndexOf(ddlElecBillStatCA1.Items.FindByText(dt1.Rows[0]["ElecBillStatus"].ToString()));

                        ddlFCUStatusCA1.SelectedIndex = ddlFCUStatusCA1.Items.IndexOf(ddlFCUStatusCA1.Items.FindByValue(dt1.Rows[0]["FCUStatus"].ToString()));

                        StatusButton("Show");
                        if (Convert.ToString(dt1.Rows[0]["VkycFlag"]) == "Y")
                        {
                            hdCoApp1VkycYN.Value = "Y";
                        }
                        else
                        {
                            hdCoApp1VkycYN.Value = "N";
                        }
                        tbIntChk.ActiveTabIndex = 1;
                        ImgElecBillCA1.Enabled = true;
                        imgFcuReportCA1.Enabled = true;

                        lblCoApp1Status.Text = "";
                        hdnCoApp1Status.Value = "Y";
                        btnValidateVkycCA1.Enabled = false;


                    }
                    else
                    {
                        lblCoApp1Status.Text = "Please validate KYC from Customer 360 Form";
                        lblCoApp1Status.ForeColor = System.Drawing.Color.Red;
                        hdnCoApp1Status.Value = "N";


                    }

                    #endregion

                    // FOR CO-APPLICANT 2
                    #region CO-APPLICANT 2
                    if (dt2.Rows.Count > 0)
                    {

                        txtPanNoCA2.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                        lblPanStatusCA2.Text = Convert.ToString(dt2.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA2.Text = Convert.ToString(dt2.Rows[0]["VoterNo"]);
                        lblVoterStatusCA2.Text = Convert.ToString(dt2.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA2.Text = Convert.ToString(dt2.Rows[0]["DdupPF"]);
                        hdnElecBillExtCA2.Value = Convert.ToString(dt2.Rows[0]["ElecBillExt"]);
                        hdnFcuReportExtCA2.Value = Convert.ToString(dt2.Rows[0]["FCURptExt"]);
                        lblCoApp2Name.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                        txtFCUDtCA2.Text = Convert.ToString(dt2.Rows[0]["FCUDt"]);
                        txtFCUExDtCA2.Text = Convert.ToString(dt2.Rows[0]["FCUExDt"]);


                        lblCustNameCA2.Text = Convert.ToString(dt2.Rows[0]["Member"]);

                        if (Convert.ToString(dt2.Rows[0]["ElecBillPF"]) == "Y")
                        {
                            lblElecBillCA2.Text = "ElectricBill" + hdnElecBillExtCA2.Value;
                        }
                        else
                        {
                            lblElecBillCA2.Text = "";
                        }

                        if (Convert.ToString(dt2.Rows[0]["FCURptUplYN"]) == "Y")
                        {
                            lblFcuRptCA2.Text = "FCUReport" + hdnFcuReportExtCA2.Value;
                        }
                        else
                        {
                            lblFcuRptCA2.Text = "";
                        }

                        ddlAmlStatusCA2.SelectedIndex = ddlAmlStatusCA2.Items.IndexOf(ddlAmlStatusCA2.Items.FindByText(dt2.Rows[0]["AMLCheckPF"].ToString()));
                        if (Convert.ToString(dt2.Rows[0]["AMLCheckPF"]) == "Pass")
                        {
                            hdnAmlPassYnCA2.Value = "Y";
                            hdCoApp2AMLFlag.Value = "Y";
                        }
                        else if (Convert.ToString(dt2.Rows[0]["AMLCheckPF"]) == "Fail")
                        {
                            hdnAmlPassYnCA2.Value = "Y";
                            hdCoApp2AMLFlag.Value = "Y";
                        }
                        ddlVKycStatusCA2.SelectedIndex = ddlVKycStatusCA2.Items.IndexOf(ddlVKycStatusCA2.Items.FindByText(dt2.Rows[0]["VKYCVerifyYN"].ToString()));
                        //if (Convert.ToString(dt2.Rows[0]["VKYCVerifyYN"]) == "Pass")
                        //{
                        //    hdnValVKYCYNCA2.Value = "Y";
                        //}
                        //else
                        //{
                        //    hdnValVKYCYNCA2.Value = "N";
                        //}

                        ddlElecBillStatCA2.SelectedIndex = ddlElecBillStatCA2.Items.IndexOf(ddlElecBillStatCA2.Items.FindByText(dt2.Rows[0]["ElecBillStatus"].ToString()));

                        ddlFCUStatusCA2.SelectedIndex = ddlFCUStatusCA2.Items.IndexOf(ddlFCUStatusCA2.Items.FindByValue(dt2.Rows[0]["FCUStatus"].ToString()));

                        if (Convert.ToString(dt2.Rows[0]["VkycFlag"]) == "Y")
                        {
                            hdCoApp2VkycYN.Value = "Y";
                        }
                        else
                        {
                            hdCoApp2VkycYN.Value = "N";
                        }

                        StatusButton("Show");
                        tbIntChk.ActiveTabIndex = 1;
                        ImgElecBillCA2.Enabled = true;
                        imgFcuReportCA2.Enabled = true;

                        lblCoApp2Status.Text = "";
                        hdnCoApp2Status.Value = "Y";
                        btnValidateVkycCA2.Enabled = false;



                    }
                    else
                    {
                        lblCoApp2Status.Text = "Please validate KYC from Customer 360 Form";
                        lblCoApp2Status.ForeColor = System.Drawing.Color.Red;
                        hdnCoApp2Status.Value = "N";

                    }
                    #endregion

                    // FOR Guarantor
                    #region Guarantor
                    if (dt3.Rows.Count > 0)
                    {

                        txtPanNoG.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                        lblPanStatusG.Text = Convert.ToString(dt3.Rows[0]["PanVerifyPF"]);
                        txtVoterIdG.Text = Convert.ToString(dt3.Rows[0]["VoterNo"]);
                        lblVoterStatusG.Text = Convert.ToString(dt3.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusG.Text = Convert.ToString(dt3.Rows[0]["DdupPF"]);
                        hdnElecBillExtG.Value = Convert.ToString(dt3.Rows[0]["ElecBillExt"]);
                        hdnFcuReportExtG.Value = Convert.ToString(dt3.Rows[0]["FCURptExt"]);
                        lblGuarantorName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                        txtFCUDtG.Text = Convert.ToString(dt3.Rows[0]["FCUDt"]);
                        txtFCUExDtG.Text = Convert.ToString(dt3.Rows[0]["FCUExDt"]);

                        lblCustNameG.Text = Convert.ToString(dt3.Rows[0]["Member"]);

                        if (Convert.ToString(dt3.Rows[0]["ElecBillPF"]) == "Y")
                        {
                            lblElecBillG.Text = "ElectricBill" + hdnElecBillExtCA2.Value;
                        }
                        else
                        {
                            lblElecBillG.Text = "";
                        }

                        if (Convert.ToString(dt3.Rows[0]["FCURptUplYN"]) == "Y")
                        {
                            lblFcuRptG.Text = "FCUReport" + hdnFcuReportExtG.Value;
                        }
                        else
                        {
                            lblFcuRptG.Text = "";
                        }

                        ddlAmlStatusG.SelectedIndex = ddlAmlStatusG.Items.IndexOf(ddlAmlStatusG.Items.FindByText(dt3.Rows[0]["AMLCheckPF"].ToString()));
                        if (Convert.ToString(dt3.Rows[0]["AMLCheckPF"]) == "Pass")
                        {
                            hdnAmlPassYnG.Value = "Y";
                            hdGuarAMLFlag.Value = "Y";
                        }
                        else if (Convert.ToString(dt3.Rows[0]["AMLCheckPF"]) == "Fail")
                        {
                            hdnAmlPassYnG.Value = "Y";
                            hdGuarAMLFlag.Value = "Y";
                        }
                        ddlVKycStatusG.SelectedIndex = ddlVKycStatusG.Items.IndexOf(ddlVKycStatusG.Items.FindByText(dt3.Rows[0]["VKYCVerifyYN"].ToString()));
                        //if (Convert.ToString(dt3.Rows[0]["VKYCVerifyYN"]) == "Pass")
                        //{
                        //    hdnValVKYCYNG.Value = "Y";
                        //}
                        //else
                        //{
                        //    hdnValVKYCYNG.Value = "N";
                        //}

                        ddlElecBillStatG.SelectedIndex = ddlElecBillStatG.Items.IndexOf(ddlElecBillStatG.Items.FindByText(dt3.Rows[0]["ElecBillStatus"].ToString()));

                        ddlFCUStatusG.SelectedIndex = ddlFCUStatusG.Items.IndexOf(ddlFCUStatusG.Items.FindByValue(dt3.Rows[0]["FCUStatus"].ToString()));

                        if (Convert.ToString(dt3.Rows[0]["VkycFlag"]) == "Y")
                        {
                            hdGVkycYN.Value = "Y";
                        }
                        else
                        {
                            hdGVkycYN.Value = "N";
                        }

                        StatusButton("Show");
                        tbIntChk.ActiveTabIndex = 1;
                        ImgElecBillG.Enabled = true;
                        imgFcuReportG.Enabled = true;
                        hdnGuarantorStatus.Value = "Y";
                        lblGuarantorStatus.Text = "";
                        btnValidateVkycG.Enabled = false;

                    }
                    else
                    {
                        lblGuarantorStatus.Text = "Please validate KYC from Customer 360 Form";
                        lblGuarantorStatus.ForeColor = System.Drawing.Color.Red;
                        hdnGuarantorStatus.Value = "N";

                    }
                    #endregion

                    // FOR CO-APPLICANT 3
                    #region CO-APPLICANT 3
                    if (dt4.Rows.Count > 0)
                    {

                        txtPanNoCA3.Text = Convert.ToString(dt4.Rows[0]["PanNo"]);
                        lblPanStatusCA3.Text = Convert.ToString(dt4.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA3.Text = Convert.ToString(dt4.Rows[0]["VoterNo"]);
                        lblVoterStatusCA3.Text = Convert.ToString(dt4.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA3.Text = Convert.ToString(dt4.Rows[0]["DdupPF"]);
                        hdnElecBillExtCA3.Value = Convert.ToString(dt4.Rows[0]["ElecBillExt"]);
                        hdnFcuReportExtCA3.Value = Convert.ToString(dt4.Rows[0]["FCURptExt"]);
                        lblCoApp3Name.Text = Convert.ToString(dt4.Rows[0]["CoAppName"]);
                        txtFCUDtCA3.Text = Convert.ToString(dt4.Rows[0]["FCUDt"]);
                        txtFCUExDtCA3.Text = Convert.ToString(dt4.Rows[0]["FCUExDt"]);


                        lblCustNameCA3.Text = Convert.ToString(dt4.Rows[0]["Member"]);

                        if (Convert.ToString(dt4.Rows[0]["ElecBillPF"]) == "Y")
                        {
                            lblElecBillCA3.Text = "ElectricBill" + hdnElecBillExtCA3.Value;
                        }
                        else
                        {
                            lblElecBillCA3.Text = "";
                        }

                        if (Convert.ToString(dt4.Rows[0]["FCURptUplYN"]) == "Y")
                        {
                            lblFcuRptCA3.Text = "FCUReport" + hdnFcuReportExtCA3.Value;
                        }
                        else
                        {
                            lblFcuRptCA3.Text = "";
                        }

                        ddlAmlStatusCA3.SelectedIndex = ddlAmlStatusCA3.Items.IndexOf(ddlAmlStatusCA3.Items.FindByText(dt4.Rows[0]["AMLCheckPF"].ToString()));
                        if (Convert.ToString(dt4.Rows[0]["AMLCheckPF"]) == "Pass")
                        {
                            hdnAmlPassYnCA3.Value = "Y";
                            hdCoApp3AMLFlag.Value = "Y";
                        }
                        else if (Convert.ToString(dt4.Rows[0]["AMLCheckPF"]) == "Fail")
                        {
                            hdnAmlPassYnCA3.Value = "Y";
                            hdCoApp3AMLFlag.Value = "Y";
                        }
                        ddlVKycStatusCA3.SelectedIndex = ddlVKycStatusCA3.Items.IndexOf(ddlVKycStatusCA3.Items.FindByText(dt4.Rows[0]["VKYCVerifyYN"].ToString()));
                        //if (Convert.ToString(dt4.Rows[0]["VKYCVerifyYN"]) == "Pass")
                        //{
                        //    hdnValVKYCYNCA3.Value = "Y";
                        //}
                        //else
                        //{
                        //    hdnValVKYCYNCA3.Value = "N";
                        //}

                        ddlElecBillStatCA3.SelectedIndex = ddlElecBillStatCA3.Items.IndexOf(ddlElecBillStatCA3.Items.FindByText(dt4.Rows[0]["ElecBillStatus"].ToString()));

                        ddlFCUStatusCA3.SelectedIndex = ddlFCUStatusCA3.Items.IndexOf(ddlFCUStatusCA3.Items.FindByValue(dt4.Rows[0]["FCUStatus"].ToString()));

                        if (Convert.ToString(dt4.Rows[0]["VkycFlag"]) == "Y")
                        {
                            hdCoApp3VkycYN.Value = "Y";
                        }
                        else
                        {
                            hdCoApp3VkycYN.Value = "N";
                        }

                        StatusButton("Show");
                        tbIntChk.ActiveTabIndex = 1;
                        ImgElecBillCA3.Enabled = true;
                        imgFcuReportCA3.Enabled = true;

                        lblCoApp3Status.Text = "";
                        hdnCoApp3Status.Value = "Y";
                        btnValidateVkycCA3.Enabled = false;



                    }
                    else
                    {
                        lblCoApp3Status.Text = "Please validate KYC from Customer 360 Form";
                        lblCoApp3Status.ForeColor = System.Drawing.Color.Red;
                        hdnCoApp3Status.Value = "N";

                    }
                    #endregion

                    // FOR CO-APPLICANT 4
                    #region CO-APPLICANT 4
                    if (dt5.Rows.Count > 0)
                    {

                        txtPanNoCA4.Text = Convert.ToString(dt5.Rows[0]["PanNo"]);
                        lblPanStatusCA4.Text = Convert.ToString(dt5.Rows[0]["PanVerifyPF"]);
                        txtVoterIdCA4.Text = Convert.ToString(dt5.Rows[0]["VoterNo"]);
                        lblVoterStatusCA4.Text = Convert.ToString(dt5.Rows[0]["VoterVerifyPF"]);
                        lblDdupStatusCA4.Text = Convert.ToString(dt5.Rows[0]["DdupPF"]);
                        hdnElecBillExtCA4.Value = Convert.ToString(dt5.Rows[0]["ElecBillExt"]);
                        hdnFcuReportExtCA4.Value = Convert.ToString(dt5.Rows[0]["FCURptExt"]);
                        lblCoApp4Name.Text = Convert.ToString(dt5.Rows[0]["CoAppName"]);
                        txtFCUDtCA4.Text = Convert.ToString(dt5.Rows[0]["FCUDt"]);
                        txtFCUExDtCA4.Text = Convert.ToString(dt5.Rows[0]["FCUExDt"]);


                        lblCustNameCA4.Text = Convert.ToString(dt5.Rows[0]["Member"]);

                        if (Convert.ToString(dt5.Rows[0]["ElecBillPF"]) == "Y")
                        {
                            lblElecBillCA4.Text = "ElectricBill" + hdnElecBillExtCA4.Value;
                        }
                        else
                        {
                            lblElecBillCA4.Text = "";
                        }

                        if (Convert.ToString(dt5.Rows[0]["FCURptUplYN"]) == "Y")
                        {
                            lblFcuRptCA4.Text = "FCUReport" + hdnFcuReportExtCA4.Value;
                        }
                        else
                        {
                            lblFcuRptCA4.Text = "";
                        }

                        ddlAmlStatusCA4.SelectedIndex = ddlAmlStatusCA4.Items.IndexOf(ddlAmlStatusCA4.Items.FindByText(dt5.Rows[0]["AMLCheckPF"].ToString()));
                        if (Convert.ToString(dt5.Rows[0]["AMLCheckPF"]) == "Pass")
                        {
                            hdnAmlPassYnCA4.Value = "Y";
                            hdCoApp4AMLFlag.Value = "Y";
                        }
                        else if (Convert.ToString(dt5.Rows[0]["AMLCheckPF"]) == "Fail")
                        {
                            hdnAmlPassYnCA4.Value = "Y";
                            hdCoApp4AMLFlag.Value = "Y";
                        }
                        ddlVKycStatusCA4.SelectedIndex = ddlVKycStatusCA4.Items.IndexOf(ddlVKycStatusCA4.Items.FindByText(dt5.Rows[0]["VKYCVerifyYN"].ToString()));
                        //if (Convert.ToString(dt5.Rows[0]["VKYCVerifyYN"]) == "Pass")
                        //{
                        //    hdnValVKYCYNCA4.Value = "Y";
                        //}
                        //else
                        //{
                        //    hdnValVKYCYNCA4.Value = "N";
                        //}

                        ddlElecBillStatCA4.SelectedIndex = ddlElecBillStatCA4.Items.IndexOf(ddlElecBillStatCA4.Items.FindByText(dt5.Rows[0]["ElecBillStatus"].ToString()));

                        ddlFCUStatusCA4.SelectedIndex = ddlFCUStatusCA4.Items.IndexOf(ddlFCUStatusCA4.Items.FindByValue(dt5.Rows[0]["FCUStatus"].ToString()));

                        if (Convert.ToString(dt5.Rows[0]["VkycFlag"]) == "Y")
                        {
                            hdCoApp4VkycYN.Value = "Y";
                        }
                        else
                        {
                            hdCoApp4VkycYN.Value = "N";
                        }

                        StatusButton("Show");
                        tbIntChk.ActiveTabIndex = 1;
                        ImgElecBillCA4.Enabled = true;
                        imgFcuReportCA4.Enabled = true;

                        lblCoApp4Status.Text = "";
                        hdnCoApp4Status.Value = "Y";
                        btnValidateVkycCA4.Enabled = false;



                    }
                    else
                    {
                        lblCoApp4Status.Text = "Please validate KYC from Customer 360 Form";
                        lblCoApp4Status.ForeColor = System.Drawing.Color.Red;
                        hdnCoApp4Status.Value = "N";

                    }
                    #endregion
                }
            }

            finally
            {
                dt = null;
            }
        }
        private void CoApp1StatusCheck(string vCoAppValue)
        {
            if (vCoAppValue == "N")
            {
                txtPanNoCA1.Enabled = false;
                btnValidateVkycCA1.Enabled = false;
                txtVoterIdCA1.Enabled = false;
                btnCheckAmlCA1.Enabled = false;
                lblDdupStatusCA1.Enabled = false;
                fuElecBillCA1.Enabled = false;
                ImgElecBillCA1.Enabled = false;
                ddlElecBillStatCA1.Enabled = false;
                fuFcuReportCA1.Enabled = false;
                imgFcuReportCA1.Enabled = false;
                ddlFCUStatusCA1.Enabled = false;
                txtFCUDtCA1.Enabled = false;
            }
            else
            {
                Boolean vAmlPassYnCA1 = hdnAmlPassYnCA1.Value == "Y" ? false : true;
                txtPanNoCA1.Enabled = true;
                btnValidateVkycCA1.Enabled = true;
                txtVoterIdCA1.Enabled = true;
                btnCheckAmlCA1.Enabled = vAmlPassYnCA1;
                lblDdupStatusCA1.Enabled = true;
                fuElecBillCA1.Enabled = true;
                ImgElecBillCA1.Enabled = true;
                ddlElecBillStatCA1.Enabled = true;
                fuFcuReportCA1.Enabled = true;
                imgFcuReportCA1.Enabled = true;
                ddlFCUStatusCA1.Enabled = true;
                txtFCUDtCA1.Enabled = true;
            }
        }
        private void CoApp4StatusCheck(string CoApp4)
        {
            if (CoApp4 == "N")
            {
                txtPanNoCA4.Enabled = false;
                btnValidateVkycCA4.Enabled = false;
                txtVoterIdCA4.Enabled = false;
                btnCheckAmlCA4.Enabled = false;
                lblDdupStatusCA4.Enabled = false;
                fuElecBillCA4.Enabled = false;
                ImgElecBillCA4.Enabled = false;
                ddlElecBillStatCA4.Enabled = false;
                fuFcuReportCA4.Enabled = false;
                imgFcuReportCA4.Enabled = false;
                ddlFCUStatusCA4.Enabled = false;
                txtFCUDtCA4.Enabled = false;
            }
            else
            {
                Boolean vAmlPassYnCA4 = hdnAmlPassYnCA4.Value == "Y" ? false : true;
                txtPanNoCA4.Enabled = true;
                btnValidateVkycCA4.Enabled = true;
                txtVoterIdCA4.Enabled = true;
                btnCheckAmlCA4.Enabled = vAmlPassYnCA4;
                lblDdupStatusCA4.Enabled = true;
                fuElecBillCA4.Enabled = true;
                ImgElecBillCA4.Enabled = true;
                ddlElecBillStatCA4.Enabled = true;
                fuFcuReportCA4.Enabled = true;
                imgFcuReportCA4.Enabled = true;
                ddlFCUStatusCA4.Enabled = true;
                txtFCUDtCA4.Enabled = true;
            }
        }
        private void CoApp2StatusCheck(string CoApp2)
        {
            if (CoApp2 == "N")
            {
                txtPanNoCA2.Enabled = false;
                btnValidateVkycCA2.Enabled = false;
                txtVoterIdCA2.Enabled = false;
                btnCheckAmlCA2.Enabled = false;
                lblDdupStatusCA2.Enabled = false;
                fuElecBillCA2.Enabled = false;
                ImgElecBillCA2.Enabled = false;
                ddlElecBillStatCA2.Enabled = false;
                fuFcuReportCA2.Enabled = false;
                imgFcuReportCA2.Enabled = false;
                ddlFCUStatusCA2.Enabled = false;
                txtFCUDtCA2.Enabled = false;
            }
            else
            {
                Boolean vAmlPassYnCA2 = hdnAmlPassYnCA2.Value == "Y" ? false : true;
                txtPanNoCA2.Enabled = true;
                btnValidateVkycCA2.Enabled = true;
                txtVoterIdCA2.Enabled = true;
                btnCheckAmlCA2.Enabled = vAmlPassYnCA2;
                lblDdupStatusCA2.Enabled = true;
                fuElecBillCA2.Enabled = true;
                ImgElecBillCA2.Enabled = true;
                ddlElecBillStatCA2.Enabled = true;
                fuFcuReportCA2.Enabled = true;
                imgFcuReportCA2.Enabled = true;
                ddlFCUStatusCA2.Enabled = true;
                txtFCUDtCA2.Enabled = true;
            }
        }
        private void CoApp3StatusCheck(string CoApp3)
        {
            if (CoApp3 == "N")
            {
                txtPanNoCA3.Enabled = false;
                btnValidateVkycCA3.Enabled = false;
                txtVoterIdCA3.Enabled = false;
                btnCheckAmlCA3.Enabled = false;
                lblDdupStatusCA3.Enabled = false;
                fuElecBillCA3.Enabled = false;
                ImgElecBillCA3.Enabled = false;
                ddlElecBillStatCA3.Enabled = false;
                fuFcuReportCA3.Enabled = false;
                imgFcuReportCA3.Enabled = false;
                ddlFCUStatusCA3.Enabled = false;
                txtFCUDtCA3.Enabled = false;
            }
            else
            {
                Boolean vAmlPassYnCA3 = hdnAmlPassYnCA3.Value == "Y" ? false : true;
                txtPanNoCA3.Enabled = true;
                btnValidateVkycCA3.Enabled = true;
                txtVoterIdCA3.Enabled = true;
                btnCheckAmlCA3.Enabled = vAmlPassYnCA3;
                lblDdupStatusCA3.Enabled = true;
                fuElecBillCA3.Enabled = true;
                ImgElecBillCA3.Enabled = true;
                ddlElecBillStatCA3.Enabled = true;
                fuFcuReportCA3.Enabled = true;
                imgFcuReportCA3.Enabled = true;
                ddlFCUStatusCA3.Enabled = true;
                txtFCUDtCA3.Enabled = true;
            }
        }
        private void GStatusCheck(string Guarantor)
        {
            if (Guarantor == "N")
            {
                txtPanNoG.Enabled = false;
                btnValidateVkycG.Enabled = false;
                txtVoterIdG.Enabled = false;
                btnCheckAmlG.Enabled = false;
                lblDdupStatusG.Enabled = false;
                fuElecBillG.Enabled = false;
                ImgElecBillG.Enabled = false;
                ddlElecBillStatG.Enabled = false;
                fuFcuReportG.Enabled = false;
                imgFcuReportG.Enabled = false;
                ddlFCUStatusG.Enabled = false;
                txtFCUDtG.Enabled = false;
            }
            else
            {
                Boolean vAmlPassYnG = hdnAmlPassYnG.Value == "Y" ? false : true;
                txtPanNoG.Enabled = true;
                btnValidateVkycG.Enabled = true;
                txtVoterIdG.Enabled = true;
                btnCheckAmlG.Enabled = vAmlPassYnG;
                lblDdupStatusG.Enabled = true;
                fuElecBillG.Enabled = true;
                ImgElecBillG.Enabled = true;
                ddlElecBillStatG.Enabled = true;
                fuFcuReportG.Enabled = true;
                imgFcuReportG.Enabled = true;
                ddlFCUStatusG.Enabled = true;
                txtFCUDtG.Enabled = true;
            }
        }
        protected void ImgElecBill_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_A_" + "ElectricBill" + hdnElecBillExt.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("A_" + "ElectricBill" + hdnElecBillExt.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "A_" + "ElectricBill" + hdnElecBillExt.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void imgFcuReport_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_A_" + "FCUReport" + hdnFcuReportExt.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("A_" + "FCUReport" + hdnFcuReportExt.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "A_" + "FCUReport" + hdnFcuReportExt.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }


        }
        protected void btnValidateVkyc_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkyc, "A", hdAppVkycYN);
        }
        protected void btnValidateVkycCA1_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkycCA1, "CA1", hdCoApp1VkycYN);
        }
        protected void btnValidateVkycCA2_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkycCA2, "CA2", hdCoApp2VkycYN);
        }
        protected void btnValidateVkycCA3_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkycCA3, "CA3", hdCoApp3VkycYN);
        }
        protected void btnValidateVkycCA4_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkycCA4, "CA4", hdCoApp4VkycYN);
        }
        protected void btnValidateVkycG_Click(object sender, EventArgs e)
        {
            ValidateVkyc(btnValidateVkycG, "G", hdGVkycYN);
        }
        protected void ValidateVkyc(Button btnVerify, string vCustType, HiddenField hdnVerifyVoterYN)
        {
            CCust360 oMem = null;
            oMem = new CCust360();

            int vErr = oMem.ChkVkyc(Convert.ToInt64(hdnLeadId.Value.Trim()), vCustType);
            if (vErr == 0)
            {
                gblFuction.AjxMsgPopup("VKYC Completed Successfully...");
                return;
            }
            else if (vErr == 1)
            {
                gblFuction.AjxMsgPopup("Two Attempts to Initiate VKYC Have Expired");
                return;
            }
            else if (vErr == 2)
            {
                gblFuction.AjxMsgPopup("One Attempt Left to Initiate VKYC");
                return;
            }
            else if (vErr == 3)
            {
                gblFuction.AjxMsgPopup("VKYC is Already Initiated...");
                return;
            }
            else if (vErr == 55)
            {
                gblFuction.AjxMsgPopup("VKYC Rejected By Auditor...");
                return;
            }
            else if (vErr == 65)
            {
                gblFuction.AjxMsgPopup("VKYC Rejected By Agent...");
                return;
            }
            else
            {
                //API URL
                string apiUrl = apiBaseUrl + generateTokenEndpoint;

                // request body
                var requestBody = new
                {
                    username = vVKYCUsrNm,
                    password = vVKYCPasswrd
                };

                string jsonRequestBody = JsonConvert.SerializeObject(requestBody);

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                    if (request == null)
                    {
                        throw new NullReferenceException("request is not a http request");
                    }
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";

                    using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                    {
                        writer.Write(jsonRequestBody);
                        writer.Close();
                    }

                    try
                    {
                        // Get the response from the server
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            // StatusCode 200, for successfull response 
                            if ((int)response.StatusCode == 200)
                            {
                                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                                {
                                    string responseContent = reader.ReadToEnd();
                                    reader.Close();

                                    JObject jsonResponse = JObject.Parse(responseContent);

                                    string token = jsonResponse["Token"].ToString();

                                    SaveTokenToDatabase(token, vCustType, hdnLeadId.Value, responseContent, 200);

                                    // Calling 2nd API with token
                                    CallSecondApi(token, vCustType, hdnLeadId.Value);
                                }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response != null)
                        {
                            using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                            {
                                int statusCode = (int)errorResponse.StatusCode;

                                // Handle Error status codes
                                if (statusCode >= 400)
                                {
                                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                                    {
                                        string errorResponseContent = reader.ReadToEnd();
                                        reader.Close();
                                        SaveTokenToDatabase("", vCustType, hdnLeadId.Value, errorResponseContent, statusCode);
                                        gblFuction.AjxMsgPopup("Error During Token Generation: Status Code " + statusCode);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {

                            gblFuction.AjxMsgPopup("Unable to connect to the server.Please Try Again...");
                            return;
                        }
                    }

                }
                catch (WebException ex)
                {
                    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        gblFuction.AjxMsgPopup("Error During Token Generation: API call failed: {errorResponse}");
                        return;
                    }
                }
            }
        }
        private void CallSecondApi(string token, string vCustType, string vLeadId)
        {
            DataSet ds = null;
            DataTable dt = null;
            string userId = "", phoneNumber = "", sendNotification = "1", linkType = "free",
                Gender = "", Dob = "", state = "", district = "", MemNm = "", PinCode = "",
                lastKycDate = "";

            string sendLinkUrl = apiBaseUrl + sendLinkEndpoint; // Full URL for send link API

            Int32 vUid = Convert.ToInt32(Session[gblValue.UserId]);
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);

            CCust360 oMem = null;
            oMem = new CCust360();

            ds = oMem.GetVKYCDetails(Convert.ToInt64(hdnLeadId.Value));
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                userId = Convert.ToString(dt.Rows[0]["LeadID"]);
                phoneNumber = Convert.ToString(dt.Rows[0]["MobNo"]);
                Gender = Convert.ToString(dt.Rows[0]["MobNo"]);
                Dob = Convert.ToString(dt.Rows[0]["DoB"]);
                state = Convert.ToString(dt.Rows[0]["StateName"]);
                district = Convert.ToString(dt.Rows[0]["DistrictName"]);
                MemNm = Convert.ToString(dt.Rows[0]["MemberName"]);
                lastKycDate = Convert.ToString(dt.Rows[0]["lastKycDate"]);
                PinCode = Convert.ToString(dt.Rows[0]["CurrPin"]);
            }

            string extras = "{\"last_kyc_date\": \"" + lastKycDate + "\", \"gender\": \"" + Gender + "\",\"dob\": \"" + Dob + "\",\"state\": \"" + state + "\",\"district\": \"" + district + "\",\"Appname\": \"" + MemNm + "\",\"pin\": \"" + PinCode + "\",\"product_code\":\"SOLARFINANCE\"}";


            string sendLinkRequestBody = "user_id=" + phoneNumber
                + "&phone_number=" + phoneNumber +
                                         "&send_notification=" + sendNotification + "&link_type=" + linkType +
                                         "&extras=" + extras;

            // Send POST request to send the link

            try
            {
                HttpWebRequest sendLinkRequest = (HttpWebRequest)WebRequest.Create(sendLinkUrl);
                sendLinkRequest.Method = "POST";
                sendLinkRequest.ContentType = "application/x-www-form-urlencoded";
                sendLinkRequest.Accept = "application/json";

                sendLinkRequest.Headers["auth"] = token;

                byte[] byteArray = Encoding.UTF8.GetBytes(sendLinkRequestBody);
                sendLinkRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = sendLinkRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response from the send link request

                try
                {
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    HttpWebResponse sendLinkResponse = (HttpWebResponse)sendLinkRequest.GetResponse();

                    if ((int)sendLinkResponse.StatusCode == 200)
                    {
                        string sendLinkResponseString = string.Empty;

                        using (StreamReader responseReader = new StreamReader(sendLinkResponse.GetResponseStream()))
                        {
                            sendLinkResponseString = responseReader.ReadToEnd();
                            SendLinkResponse sendLinkResponseObject = JsonConvert.DeserializeObject<SendLinkResponse>(sendLinkResponseString);

                            string ExpiryTime = sendLinkResponseObject.link_expiry_time;
                            string linkId = sendLinkResponseObject.link_id;
                            string linkUrl = sendLinkResponseObject.link_url;
                            string PhoneNo = sendLinkResponseObject.phone_number;
                            string SessionId = sendLinkResponseObject.session_id;
                            string UserId = sendLinkResponseObject.user_id;
                            string ValDuration = sendLinkResponseObject.validity_duration;

                            SaveVKYCResponseLog(vCustType, hdnLeadId.Value, 200, sendLinkResponseString, SessionId, ExpiryTime, sendLinkRequestBody);
                            if (vCustType == "A")
                            {
                                hdAppVkycYN.Value = "Y";
                                ddlVKycStatus.SelectedValue = "Initiated";
                            }
                            else if (vCustType == "CA1")
                            {
                                hdCoApp1VkycYN.Value = "Y";
                                ddlVKycStatusCA1.SelectedValue = "Initiated";
                            }
                            else if (vCustType == "CA2")
                            {
                                hdCoApp2VkycYN.Value = "Y";
                                ddlVKycStatusCA2.SelectedValue = "Initiated";
                            }
                            else if (vCustType == "CA3")
                            {
                                hdCoApp3VkycYN.Value = "Y";
                                ddlVKycStatusCA3.SelectedValue = "Initiated";
                            }
                            else if (vCustType == "CA4")
                            {
                                hdCoApp4VkycYN.Value = "Y";
                                ddlVKycStatusCA4.SelectedValue = "Initiated";
                            }
                            else if (vCustType == "G")
                            {
                                hdGVkycYN.Value = "Y";
                                ddlVKycStatusG.SelectedValue = "Initiated";
                            }

                            gblFuction.AjxMsgPopup("VKYC Initiated Successfully...");
                            return;
                        }
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                        {
                            int statusCode = (int)errorResponse.StatusCode;

                            // Handle Error status codes
                            if (statusCode >= 400)
                            {
                                if (vCustType == "A")
                                {
                                    hdAppVkycYN.Value = "N";
                                    ddlVKycStatus.SelectedValue = "Fail";
                                }
                                else if (vCustType == "CA1")
                                {
                                    hdCoApp1VkycYN.Value = "N";
                                    ddlVKycStatusCA1.SelectedValue = "Fail";
                                }
                                else if (vCustType == "CA2")
                                {
                                    hdCoApp2VkycYN.Value = "N";
                                    ddlVKycStatusCA2.SelectedValue = "Fail";
                                }
                                else if (vCustType == "CA3")
                                {
                                    hdCoApp3VkycYN.Value = "N";
                                    ddlVKycStatusCA3.SelectedValue = "Fail";
                                }
                                else if (vCustType == "CA4")
                                {
                                    hdCoApp4VkycYN.Value = "N";
                                    ddlVKycStatusCA4.SelectedValue = "Fail";
                                }
                                else if (vCustType == "G")
                                {
                                    hdGVkycYN.Value = "N";
                                    ddlVKycStatusG.SelectedValue = "Fail";
                                }

                                if (statusCode == 404)
                                {
                                    gblFuction.AjxMsgPopup("Error: Status Code " + statusCode + ", Session Not Found");
                                    return;
                                }
                                else
                                {
                                    using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                                    {
                                        string errorResponseContent = reader.ReadToEnd();
                                        reader.Close();
                                        SaveVKYCResponseLog(vCustType, hdnLeadId.Value, statusCode, errorResponseContent, "", "", "");
                                        gblFuction.AjxMsgPopup("Error: Status Code " + statusCode);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        gblFuction.AjxMsgPopup("Error: Unable to connect to the server.");
                        return;
                    }
                }
            }
            catch (WebException ex)
            {
                using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    string errorResponse = reader.ReadToEnd();
                    gblFuction.AjxMsgPopup("Error: API call failed: {errorResponse}");
                    return;
                }
            }
        }
        private void SaveTokenToDatabase(string vtoken, string vCusttype, string LeadID, string vStatusMsg, Int32 vStatusCode)
        {
            Int32 vUid = Convert.ToInt32(Session[gblValue.UserId]);
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);

            CCust360 oMem = null;
            oMem = new CCust360();
            oMem.SaveVKYCTokenLog(Convert.ToInt64(hdnLeadId.Value), vCusttype, vtoken, vStatusCode, vStatusMsg, vBranch, vUid);
        }

        public class SendLinkResponse
        {
            public string link_expiry_time { get; set; }
            public string link_id { get; set; }
            public string link_url { get; set; }
            public string phone_number { get; set; }
            public string session_id { get; set; }
            public string user_id { get; set; }
            public string validity_duration { get; set; }
        }


        private void SaveVKYCResponseLog(string vCustType, string LeadId, Int32 vStatusCode, string vResMsg, string vSessionId, string vExpiryTime, string vReqBody)
        {
            Int32 vUid = Convert.ToInt32(Session[gblValue.UserId]);
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);

            CCust360 oMem = null;
            oMem = new CCust360();
            oMem.SaveVKYCResponseLog(Convert.ToInt64(hdnLeadId.Value), vCustType, vStatusCode, vResMsg, vSessionId, vExpiryTime, vBranch, vUid, vReqBody);
        }
        protected void btnCheckAml_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdAppAMLFlag.Value = "Y";

            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "A");
            gblFuction.AjxMsgPopup(vMsg);
        }
        #region CO-APPLICANT 1
        protected void btnCheckAmlCA1_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdCoApp1AMLFlag.Value = "Y";
            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "CA1");
            gblFuction.AjxMsgPopup(vMsg);
        }

        protected void imgFcuReportCA1_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA1_" + "FCUReport" + hdnFcuReportExtCA1.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }


        }
        protected void ImgElecBillCA1_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA1_" + "ElectricBill" + hdnElecBillExtCA1.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        #endregion
        #region CO-APPLICANT 2
        protected void btnCheckAmlCA2_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdCoApp2AMLFlag.Value = "Y";
            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "CA2");
            gblFuction.AjxMsgPopup(vMsg);
        }

        protected void imgFcuReportCA2_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA2_" + "FCUReport" + hdnFcuReportExtCA2.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillCA2_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA2_" + "ElectricBill" + hdnElecBillExtCA2.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }
        #endregion
        #region CO-APPLICANT 3
        protected void btnCheckAmlCA3_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdCoApp2AMLFlag.Value = "Y";
            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "CA3");
            gblFuction.AjxMsgPopup(vMsg);
        }

        protected void imgFcuReportCA3_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA3_" + "FCUReport" + hdnFcuReportExtCA3.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        protected void ImgElecBillCA3_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA3_" + "ElectricBill" + hdnElecBillExtCA3.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }
        #endregion
        #region CO-APPLICANT 4
        protected void btnCheckAmlCA4_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdCoApp2AMLFlag.Value = "Y";
            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "CA4");
            gblFuction.AjxMsgPopup(vMsg);
        }

        protected void imgFcuReportCA4_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA4_" + "FCUReport" + hdnFcuReportExtCA4.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        protected void ImgElecBillCA4_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "CA4_" + "ElectricBill" + hdnElecBillExtCA4.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }
        #endregion
        #region GUARANTOR
        protected void btnCheckAmlG_Click(object sender, EventArgs e)
        {
            string vMsg = "";
            hdGuarAMLFlag.Value = "Y";
            vMsg = JocataCalling(Convert.ToInt64(hdnLeadId.Value), hdnAppId.Value, Convert.ToString(Session[gblValue.UserId]), "G");
            gblFuction.AjxMsgPopup(vMsg);
        }

        protected void imgFcuReportG_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_G_" + "FCUReport" + hdnFcuReportExtG.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("G_" + "FCUReport" + hdnFcuReportExtG.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "G_" + "FCUReport" + hdnFcuReportExtG.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void ImgElecBillG_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdnLeadId.Value + "_G_" + "ElectricBill" + hdnElecBillExtG.Value;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }

            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image("G_" + "ElectricBill" + hdnElecBillExtG.Value, hdnLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdnLeadId.Value + "G_" + "ElectricBill" + hdnElecBillExtG.Value);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        #endregion
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
        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vRampResponse = "";
            try
            {
                //-----------------------Ramp Request------------------------
                string postURL = vKarzaEnv == "PROD" ? "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request" : "https://usfbamluat.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
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
        public string JocataCalling(Int64 pLeadId, string pMemberID, string pCreatedBy, string vCustType)
        {
            string vMsg = "", vResponseXml = "", vRequestXml = "", vResponseData = "", vScreeningId = "";
            DataTable dt = null;
            CCust360 obj = null;
            try
            {
                dt = new DataTable();
                obj = new CCust360();
                dt = obj.GetJocataRequestData(pMemberID, vCustType);
                if (dt.Rows.Count > 0)
                {
                    #region JocataRequest
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
                    #endregion

                    string vRequestData = JsonConvert.SerializeObject(req); //getting Request Json from here.
                    vRequestXml = AsString(JsonConvert.DeserializeXmlNode(vRequestData, "root"));

                    vResponseData = RampRequest(req);
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);
                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vMsg = "True match member.";
                            vStatus = "N";

                        }
                        else
                        {
                            vMsg = "False match member.";
                            vStatus = "P";
                            try
                            {
                                ProsiReq pReq = new ProsiReq();
                                pReq.pMemberId = pMemberID;
                                pReq.pCreatedBy = pCreatedBy;
                                pReq.pLeadId = pLeadId.ToString();
                                //Prosidex(pReq);
                                PosidexEncryption(pReq);

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally { }
                            //hdnAmlPassYn.Value = "Y";

                        }
                        if (vCustType == "A")
                        {
                            if (vStatus == "N") ddlAmlStatus.SelectedValue = "F";
                            else if (vStatus == "P") ddlAmlStatus.SelectedValue = "P";

                            btnCheckAml.Enabled = false;
                        }
                        else if (vCustType == "CA1")
                        {
                            if (vStatus == "N") ddlAmlStatusCA1.SelectedValue = "F";
                            else if (vStatus == "P") ddlAmlStatusCA1.SelectedValue = "P";
                            btnCheckAmlCA1.Enabled = false;
                        }
                        else if (vCustType == "CA2")
                        {
                            if (vStatus == "N") ddlAmlStatusCA2.SelectedValue = "F";
                            else if (vStatus == "P") ddlAmlStatusCA2.SelectedValue = "P";
                            btnCheckAmlCA2.Enabled = false;
                        }
                        else if (vCustType == "G")
                        {
                            if (vStatus == "N") ddlAmlStatusG.SelectedValue = "F";
                            else if (vStatus == "P") ddlAmlStatusG.SelectedValue = "P";
                            btnCheckAmlG.Enabled = false;
                        }
                        obj.UpdateJocataStatus(pMemberID, vScreeningId, vStatus, Convert.ToInt32(pCreatedBy));
                    }
                    else if (vResponse.rampResponse.error == "invalid_token")
                    {
                        vMsg = vResponse.rampResponse.error_description;
                    }
                    else
                    {
                        vMsg = "Problem in Jocata Request Data.";
                    }
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    obj.SaveJocataLog(pLeadId, vCustType, vRequestXml, vResponseXml, vScreeningId);
                }
            }
            catch (WebException ex)
            {
                dt = obj.GetJocataRequestData(pMemberID, vCustType);
                if (dt.Rows.Count > 0)
                {
                    #region JocataRequest
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
                    #endregion

                    string vRequestData = JsonConvert.SerializeObject(req); //getting Request Json from here.
                    vRequestXml = AsString(JsonConvert.DeserializeXmlNode(vRequestData, "root"));
                }
                obj.SaveJocataLog(pLeadId, vCustType, vRequestXml, vResponseXml, vScreeningId);
                if (ex.Status == WebExceptionStatus.ConnectFailure)
                {
                    // Server is down or not reachable
                    vMsg = "Unable to connect to API server.";
                }
                else if (ex.Status == WebExceptionStatus.Timeout)
                {
                    // Request timed out
                    vMsg = "The request timed out.";
                }
                else if (ex.Response != null)
                {
                    // Handle specific HTTP error response (like 500, 404, etc.)
                    var httpResponse = (HttpWebResponse)ex.Response;
                    vMsg = "HTTP Error: " + (int)httpResponse.StatusCode + httpResponse.StatusDescription;
                }
                else
                {
                    // General error
                    vMsg = "Unable To Connect To The Server...";
                }

            }
            catch (Exception ex)
            {
                vMsg = "Unable To Connect To The Server...";
            }
            //catch (WebException we)
            //{
            //    //vMsg = we.Message;
            //    //vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
            //    //obj.SaveJocataLog(pLeadId, vCustType,vRequestXml, vResponseXml, vScreeningId);
            //    //obj.UpdateJocataStatus(pMemberID, vScreeningId, "U", Convert.ToInt32(pCreatedBy));
            //}
            return vMsg;
        }

        #region Posidex
        public ProsidexResponse Prosidex(ProsiReq pProsiReq)
        {
            CCust360 obj = null;
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            List<ACE> oACE = new List<ACE>();
            ProsidexResponse pResponseData = null;
            obj = new CCust360();
            dt = obj.CF_GetProsidexReqData(Convert.ToInt64(pProsiReq.pLeadId), "A", pProsiReq.pMemberId, pProsiReq.pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pLeadId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();
                //--------------------Address Part----------------------------
                oACE.Add(new ACE(RemoveSpecialCharecters(dt.Rows[0]["ADDRESS"].ToString()),
                    dt.Rows[0]["ADDRESS_TYPE_FLAG"].ToString(),
                    dt.Rows[0]["COUNTRY"].ToString(),
                    RemoveSpecialCharecters(dt.Rows[0]["City"].ToString()),
                    dt.Rows[0]["EMAIL"].ToString(),
                    dt.Rows[0]["EMAIL_TYPE"].ToString(),
                    dt.Rows[0]["PHONE"].ToString(),
                    dt.Rows[0]["PHONE_TYPE"].ToString(),
                    dt.Rows[0]["PINCODE"].ToString(),
                    dt.Rows[0]["State"].ToString()
                    ));
                pReq.ACE = oACE;
                //-------------------------------------------------------------
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            //if (pResponseData.response_code != 200 || pResponseData.response_code != 300)
            //{
            //    pResponseData = ProsidexSearchCustomer(pReqData);
            //}
            return pResponseData;
        }

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "";
            Int32 vCreatedBy = 1; Int64 vLeadId = 0;
            ProsidexResponse oProsidexResponse = null;
            CCust360 obj = null;
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                hdnMemberId.Value = prosidexRequest.Request.DG.CUST_ID;
                hdnRequestId.Value = prosidexRequest.Request.UnitySfb_RequestId;
                vLeadId = Convert.ToInt64(prosidexRequest.Request.DG.APPLICATIONID);

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer" : "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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
                string vResponseCode = res.Response.StatusInfo.ResponseCode;
                if (vResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                obj = new CCust360();
                obj.CF_SaveProsidexLog(vMemberId, vLeadId, "A", vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------                  
                return oProsidexResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                obj = new CCust360();
                obj.CF_SaveProsidexLog(vMemberId, vLeadId, "A", vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }

        public ProsidexResponse PosidexEncryption(ProsiReq pProsiReq)
        {
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            List<ACE> oACE = new List<ACE>();
            ProsidexResponse pResponseData = null;
            CCust360 obj = new CCust360();
            dt = obj.CF_GetProsidexReqData(Convert.ToInt64(pProsiReq.pLeadId), "A", pProsiReq.pMemberId, pProsiReq.pCreatedBy); 
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pLeadId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();
                //--------------------Address Part----------------------------
                oACE.Add(new ACE(dt.Rows[0]["ADDRESS"].ToString(),
                    dt.Rows[0]["ADDRESS_TYPE_FLAG"].ToString(),
                    dt.Rows[0]["COUNTRY"].ToString(),
                    dt.Rows[0]["City"].ToString(),
                    dt.Rows[0]["EMAIL"].ToString(),
                    dt.Rows[0]["EMAIL_TYPE"].ToString(),
                    dt.Rows[0]["PHONE"].ToString(),
                    dt.Rows[0]["PHONE_TYPE"].ToString(),
                    dt.Rows[0]["PINCODE"].ToString(),
                    dt.Rows[0]["State"].ToString()
                    ));
                pReq.ACE = oACE;
                //-------------------------------------------------------------
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
            }
            pResponseData = ProsidexEncryption(pReq);
            return pResponseData;
        }

        public ProsidexResponse ProsidexEncryption(Request Req)
        {
            string vRequestdata = "", vFullResponse = "", vResponse = "", vUCIC = "", vRequestId = "",
            vMemberId = "", vPotentialYN = "N", vPotenURL = "", vResponseCode = "", vRsaKey = "",
            vResponseData = "", vEncryptedMatchResponse = "";
            string vPostUrl = PosidexEncURL + "/ServicePosidex.svc/PosidexSearchCustomer";
            int vLeadId = 0, vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            DataTable dt = null; bool vViewPotenMem = false;
            CCust360 obj = null;
            //-------------------------------------------------------------
            vRequestId = Req.UnitySfb_RequestId;
            vMemberId = Req.DG.CUST_ID;
            vLeadId = Convert.ToInt32(Req.DG.APPLICATIONID);
            //------------------------------------------------------------
            vRequestdata = JsonConvert.SerializeObject(Req);
            vFullResponse = HttpRequest(vPostUrl, vRequestdata);
            //------------------------------------------------------------
            dynamic objFullResponse = JsonConvert.DeserializeObject(vFullResponse);
            vResponseData = Convert.ToString(objFullResponse.ResponseData);
            vRsaKey = Convert.ToString(objFullResponse.RsaKey);
            //----------------------------------------------------------
            dynamic vFinalResponse = JsonConvert.DeserializeObject(vResponseData);
            vResponseCode = Convert.ToString(vFinalResponse.StatusInfo.ResponseCode);
            vEncryptedMatchResponse = Convert.ToString(vFinalResponse.EncryptedMatchResponse);
            vResponse = DecryptStringAES(vEncryptedMatchResponse, vRsaKey);
            dynamic vResp = JsonConvert.DeserializeObject(vResponse);
            vUCIC = vResp.POSIDEX_GENERATED_UCIC;
            //------------------------------------------------------------          
            if (vResponseCode == "200")
            {
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                vPotentialYN = vUCIC == null ? "Y" : "N";
                vPotenURL = vUCIC == null ? Convert.ToString(vResp.CRM_URL) : "";
            }
            else
            {
                vUCIC = vUCIC == null ? "" : vUCIC;
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
            }
            //----------------------------Save Log-------------------------------------------------
            string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
            obj = new CCust360();
            obj.CF_SaveProsidexLog(vMemberId, vLeadId, "A", vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
            //----------------------------------Get UCIC Details-------------------------------------------
            try
            {
                CCust360 CC = new CCust360();
                dt = CC.CF_GetProsidexUCICData(Convert.ToInt64(hdnLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    vViewPotenMem = Convert.ToString(dt.Rows[0]["PotentialYN"]) == "Y" ? true : false;

                    btnShwPotenMem.Visible = vViewPotenMem;
                    btnUpdateUcic.Visible = vViewPotenMem;

                    hdnProtenUrl.Value = Convert.ToString(dt.Rows[0]["PotenURL"]);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
            //-----------------------------------------------------------------------------------------------
            return oProsidexResponse;
        }

        public string HttpRequest(string PostUrl, string Requestdata)
        {
            string vResponse = "";
            try
            {
                string postURL = PostUrl;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = streamReader.ReadToEnd();
                request.GetResponse().Close();
                return vResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
            }
            return vResponse;
        }

        public string DecryptStringAES(string plainText, string publicKey)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] aesKey = Convert.FromBase64String(publicKey);
                aesAlg.KeySize = 256;
                aesAlg.Key = aesKey;
                aesAlg.IV = new byte[16];
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(plainText)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
        #endregion

        public static string RemoveSpecialCharecters(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9,/ -]", "");
        }

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
            string pMemberId = hdnMemberId.Value;
           
            string pUcicId = getUcic(pMemberId, Convert.ToInt32(Session[gblValue.UserId]));
            int pErr = -1;
            CCust360 oMem = new CCust360();
            if (pUcicId != "")
            {
                pErr = oMem.UpdateUcicId(pUcicId, pMemberId);
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

        public string getUcic(string pMemberId, int pCreatedBy)
        {
            string vResponse = "", vUcic = "";
            CCust360 oMem = new CCust360();
            try
            {
                string Requestdata = "{\"cust_id\" :" + "\"" + pMemberId + "\"" + ",\"source_system_name\":\"BIJLI\"}";
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/getUcic";
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
                oMem.SaveProsidexLogUCIC(pMemberId,Convert.ToInt64(hdnLeadId.Value),hdnRequestId.Value, vResponseXml, pCreatedBy, vUcic);
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
                oMem.SaveProsidexLogUCIC(pMemberId, Convert.ToInt64(hdnLeadId.Value),hdnRequestId.Value, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }
    }

    public class RequestVOList
    {
        public string aadhar { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string concatAddress { get; set; }
        public string country { get; set; }
        public string customerId { get; set; }
        public string digitalID { get; set; }
        public string din { get; set; }
        public string dob { get; set; }
        public string docNumber { get; set; }
        public string drivingLicence { get; set; }
        public string email { get; set; }
        public string entityName { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string pan { get; set; }
        public string passport { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
        public string rationCardNo { get; set; }
        public string ssn { get; set; }
        public string state { get; set; }
        public string tin { get; set; }
        public string voterId { get; set; }
    }

    public class RequestListVO
    {
        public string businessUnit { get; set; }
        public string subBusinessUnit { get; set; }
        public string requestType { get; set; }
        public List<RequestVOList> requestVOList { get; set; }
    }

    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }

    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    public class PostRampRequest
    {
        public RampRequest rampRequest { get; set; }
    }

    public class ProsidexResponse
    {
        public string RequestId { get; set; }
        public string UCIC { get; set; }
        public int response_code { get; set; }

        public ProsidexResponse(string RequestId, string UCIC, int response_code)
        {
            this.RequestId = RequestId;
            this.UCIC = UCIC;
            this.response_code = response_code;
        }
    }

    public class ProsidexRequest
    {
        public Request Request { get; set; }
    }

    public class Request
    {
        public DG DG { get; set; }
        public List<ACE> ACE { get; set; }
        public string UnitySfb_RequestId { get; set; }
        public string CUST_TYPE { get; set; }
        public string CustomerCategory { get; set; }
        public string MatchingRuleProfile { get; set; }
        public string Req_flag { get; set; }
        public string SourceSystem { get; set; }
    }

    public class DG
    {
        public string ACCOUNT_NUMBER { get; set; }
        public string ALIAS_NAME { get; set; }
        public string APPLICATIONID { get; set; }
        public string Aadhar { get; set; }
        public string CIN { get; set; }
        public string CKYC { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string CUST_ID { get; set; }
        public string DOB { get; set; }
        public string DrivingLicense { get; set; }
        public string Father_First_Name { get; set; }
        public string Father_Last_Name { get; set; }
        public string Father_Middle_Name { get; set; }
        public string Father_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string GSTIN { get; set; }
        public string Lead_Id { get; set; }
        public string NREGA { get; set; }
        public string Pan { get; set; }
        public string PassportNo { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RationCard { get; set; }
        public string Registration_NO { get; set; }
        public string SALUTATION { get; set; }
        public string TAN { get; set; }
        public string Udyam_aadhar_number { get; set; }
        public string VoterId { get; set; }
        public string Tasc_Customer { get; set; }
    }

    public class ACE
    {
        public string ADDRESS { get; set; }
        public string ADDRESS_TYPE_FLAG { get; set; }
        public string COUNTRY { get; set; }
        public string City { get; set; }
        public string EMAIL { get; set; }
        public string EMAIL_TYPE { get; set; }
        public string PHONE { get; set; }
        public string PHONE_TYPE { get; set; }
        public string PINCODE { get; set; }
        public string State { get; set; }

        public ACE(string ADDRESS, string ADDRESS_TYPE_FLAG, string COUNTRY, string City, string EMAIL, string EMAIL_TYPE, string PHONE, string PHONE_TYPE, string PINCODE, string State)
        {
            this.ADDRESS = ADDRESS;
            this.ADDRESS_TYPE_FLAG = ADDRESS_TYPE_FLAG;
            this.COUNTRY = COUNTRY;
            this.City = City;
            this.EMAIL = EMAIL;
            this.EMAIL_TYPE = EMAIL_TYPE;
            this.PHONE = PHONE;
            this.PHONE_TYPE = PHONE_TYPE;
            this.PINCODE = PINCODE;
            this.State = State;
        }
    }

    public class ProsiReq
    {
        public string pMemberId { get; set; }
        public string pLeadId { get; set; }
        public string pCreatedBy { get; set; }
    }
}