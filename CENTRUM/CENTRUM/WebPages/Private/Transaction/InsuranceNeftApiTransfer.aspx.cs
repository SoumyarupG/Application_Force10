using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Net;
using CENTRUM.CentrumMobUAT;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class InsuranceNeftApiTransfer : CENTRUMBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Enabled = false;
                //popState();
                PopBranch(Session[gblValue.UserName].ToString());
                popIC();
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
                PopDisbBank();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Insurance NEFT API";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuInsuranceNeftApi);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Insurance NEFT API", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            Int32 vRows = 0;
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select atleast one branch...");
                return;
            }
            try
            {
                oNeft = new CNEFTTransfer();
                dt = oNeft.GetNEFTTransferICAPI(ddlBank.SelectedValue, gblFuction.setDate(txtDt.Text), ddlBranch.SelectedValues.Replace("|", ","), "N", Convert.ToInt32(ddlProduct.SelectedValue)); //N means NEFT           
                gvDtl.DataSource = dt;
                gvDtl.DataBind();

            }
            finally
            {
                dt = null;
                oNeft = null;
            }
        }

        public void SetTotal()
        {
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().ToUpper() != "")
            {
                if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                {
                    gblFuction.AjxMsgPopup("Invalid User ID...");
                    return;
                }
                if (txtDt.Text.Trim() != "")
                {
                    if (gblFuction.IsDate(txtDt.Text) == false)
                    {
                        gblFuction.MsgPopup(gblMarg.ValidDate);
                        gblFuction.focus("ctl00_cph_Main_txtDt");
                    }
                    else if (ddlBank.SelectedValue == "-1")
                    {
                        gblFuction.MsgPopup("please Select a Bank...");
                    }
                    else
                    {
                        LoadGrid(0);
                        SetTotal();
                        txtDt.Enabled = false;
                    }
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("User name can not be Blank");
                return;
            }
        }

        private void PopDisbBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            oNeft = new CNEFTTransfer();
            dt = oNeft.PopDisbBank();
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem olist = new ListItem("<--select-->", "-1");
            ddlBank.Items.Insert(0, olist);
        }

        private void popIC()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            oGbl = new CGblIdGenerator();
            dt = oGbl.PopComboMIS("N", "N", "AA", "ICId", "ICName", "ICMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
            ddlProduct.DataSource = dt;
            ddlProduct.DataTextField = "ICName";
            ddlProduct.DataValueField = "ICId";
            ddlProduct.DataBind();
            ListItem oL1 = new ListItem("<-- Select -->", "-1");
            ddlProduct.Items.Insert(0, oL1);
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                }
                //dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValue);
                //if (dt.Rows.Count > 0)
                //{
                //    ddlBranch.DataSource = dt;
                //    ddlBranch.DataTextField = "BranchName";
                //    ddlBranch.DataValueField = "BranchCode";
                //    ddlBranch.DataBind();                    
                //}

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(bool Status)
        {

        }

        private void ClearControls()
        {
            //txtTotNEFT.Text = "0";
            //LoadGrid(0);
        }

        protected void ProgressBar1_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {

        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabReSchdl.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveInitiate()
        {
            try
            {
                if (txtUser.Text.Trim().ToUpper() != "")
                {
                    if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                    {
                        gblFuction.AjxMsgPopup("Invalid User ID...");
                        return;
                    }
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return;
                    }
                    if (Session[gblValue.ICICINEFTYN].ToString() == "Y")
                    {
                        Int32 vNeftAPI = Convert.ToInt32(Session[gblValue.NeftAPI].ToString());
                        if (vNeftAPI != 0)
                        {
                            Int32 unixTicks = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                            if (unixTicks - vNeftAPI > 600)
                            {
                                Session[gblValue.NeftAPI] = ((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                                if (vStateEdit == "" || vStateEdit == null)
                                    vStateEdit = "Save";
                                if (SaveRecords(vStateEdit) == true)
                                {                                   
                                    gblFuction.AjxMsgPopup("Disbusrement process is running on background.Please wait..");
                                    LoadGrid(0);
                                    StatusButton("View");
                                    ddlBank.Enabled = false;                                    
                                }
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("NEFT Request Is Executing ...Please Wait For 10 Mins..And Re Execute..");
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("You do not have access...");
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("User ID Can not be Blank...");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveInitiate();
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = true;
            string vNEFTAcc = "", vBeneficiaryAcc = "", vBeneficiaryIFSC = "", vNEFTAmount = "", vLoanid = "", vPayeeName = "", responsedata = "", vXML = "", vXmlData = "";
            CNEFTTransfer oNEFT = new CNEFTTransfer();
            int vErr = 0;
            int vCnt = 0;

            #region CREATE DATA TABLE
            DataTable dtdata = new DataTable("Table1");
            dtdata.Columns.Add("LoanId");
            dtdata.Columns.Add("Response");
            dtdata.Columns.Add("Message");
            dtdata.Columns.Add("UTRNUMBER");

            DataTable dtNeft = new DataTable("Table1");
            dtNeft.Columns.Add("AGGRID");
            dtNeft.Columns.Add("AGGRNAME");
            dtNeft.Columns.Add("CORPID");
            dtNeft.Columns.Add("USERID");
            dtNeft.Columns.Add("URN");
            dtNeft.Columns.Add("DEBITACC");
            dtNeft.Columns.Add("CREDITACC");
            dtNeft.Columns.Add("IFSC");
            dtNeft.Columns.Add("AMOUNT");
            dtNeft.Columns.Add("CURRENCY");
            dtNeft.Columns.Add("TXNTYPE");
            dtNeft.Columns.Add("PAYEENAME");
            dtNeft.Columns.Add("UNIQUEID");
            dtNeft.Columns.Add("REMARKS");
            #endregion

            try
            {
                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    responsedata = "";
                    CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                    #region IF Loan selected for NEFT
                    if (chkDisb.Checked == true)
                    {
                        vNEFTAcc = gr.Cells[23].Text;
                        vBeneficiaryAcc = gr.Cells[24].Text;
                        vBeneficiaryIFSC = gr.Cells[25].Text;
                        vNEFTAmount = gr.Cells[26].Text;
                        vLoanid = gr.Cells[27].Text;
                        vPayeeName = gr.Cells[28].Text;

                        DataRow drNeft = dtNeft.NewRow();
                        drNeft["AGGRID"] = "OTOE0027";
                        drNeft["AGGRNAME"] = "CENTRUM";
                        drNeft["CORPID"] = "581109799";
                        drNeft["USERID"] = txtUser.Text.Trim().ToUpper();
                        drNeft["URN"] = "SR191962059";
                        drNeft["DEBITACC"] = vNEFTAcc;
                        drNeft["CREDITACC"] = vBeneficiaryAcc;
                        drNeft["IFSC"] = vBeneficiaryIFSC;
                        drNeft["AMOUNT"] = vNEFTAmount;
                        drNeft["CURRENCY"] = "INR";
                        drNeft["TXNTYPE"] = "RGS";
                        drNeft["PAYEENAME"] = vPayeeName;
                        drNeft["UNIQUEID"] = vLoanid;
                        drNeft["REMARKS"] = "Disbursement from Unity Small Finance Bank Ltd.";
                        dtNeft.Rows.Add(drNeft);
                        dtNeft.AcceptChanges();                        
                        vCnt = vCnt + 1;
                    }
                    #endregion
                }
                if (vCnt == 0)
                {
                    gblFuction.AjxMsgPopup("No Loan Selected for NEFT");
                    vResult = false;
                }

                using (StringWriter oSW = new StringWriter())
                {
                    dtNeft.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                var req = new ICICIDisbursement()
                {
                    pXml = vXmlData,
                    pUserId = Convert.ToString(Session[gblValue.UserId]),
                    pTransDate=txtDt.Text,
                    pBankDescId=ddlBank.SelectedValue
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                CallAPI("ICICIICDisbursement", Requestdata, "https://centrummob.bijliftt.com/CentrumService.svc");
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ScriptManager.RegisterStartupScript(this, typeof(Page), "CallPage", "pageCalling();", true);
            }
        }

        private void CallAPI(string pApiName, string pRequestdata, string pReportUrl)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, pReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup("Disbusrement process is running on background.");
            }
        }

        protected void chkDisb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");           
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                Label lblLnAmt = (Label)gR.FindControl("lblLnAmt");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(lblLnAmt.Text.Trim());
                    vTotCount = vTotCount + 1;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            UpTot.Update();
        }

        protected void btnBalance_Click(object sender, EventArgs e)
        {
            if (txtUser.Text.Trim().ToUpper() != "")
            {
                if (txtUser.Text.Trim().ToUpper() != Session[gblValue.ICICIUser].ToString().Trim().ToUpper())
                {
                    gblFuction.AjxMsgPopup("Invalid User ID...");
                    return;
                }
                if (Session[gblValue.ICICINEFTYN].ToString() == "Y")
                {
                    string responsedata = "";
                    string vAccountNumber = ddlBank.SelectedItem.Text.Trim().Substring(ddlBank.SelectedItem.Text.IndexOf("#") + 1);
                    var vObj = new PostBalEnqReq()
                    {
                        AGGRID = "OTOE0027",
                        //CORPID = "CENTRUMM29112017",
                        CORPID = "581109799",
                        USERID = txtUser.Text.Trim().ToUpper(),
                        URN = "SR191962059",
                        ACCOUNTNO = vAccountNumber
                    };
                    string json = "{\"vPostBalEnqReq\":" + JsonConvert.SerializeObject(vObj) + "}";
                    try
                    {
                        var vURLString = "https://centrummob.bijliftt.com/CentrumService.svc/ICICBalanceFetch";
                        //Centrum MOB and Centrum Web hosted in same server
                        //var vURLString = "http://localhost:3008/CentrumService.svc/ICICBalanceFetch";
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(vURLString);
                        httpWebRequest.Method = "POST";
                        httpWebRequest.Host = "centrummob.bijliftt.com";
                        httpWebRequest.ContentType = "application/json";
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        byte[] data = Encoding.UTF8.GetBytes(json);
                        httpWebRequest.ContentLength = data.Length;
                        Stream requestStream = httpWebRequest.GetRequestStream();
                        requestStream.Write(data, 0, data.Length);
                        requestStream.Close();
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            var API_Response = streamReader.ReadToEnd(); ;
                            responsedata = API_Response.ToString().Trim();
                        }

                        responsedata = responsedata.Replace("{\"ICICBalanceFetchResult\":", "").Replace("}}", "}");

                        ICICBalanceFetchResponse vResponseObj = new ICICBalanceFetchResponse();

                        vResponseObj = JsonConvert.DeserializeObject<ICICBalanceFetchResponse>(responsedata);
                        if ((vResponseObj.EFFECTIVEBAL) == null)
                        {
                            gblFuction.AjxMsgPopup("Bank API returns:- " + vResponseObj.MESSAGE);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Account Effective Balance is : " + vResponseObj.EFFECTIVEBAL);
                        }
                        return;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("You do not have Access ");
                    return;
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("User name can not be Blank");
                return;
            }
        }
    }
}