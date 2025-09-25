using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Web.Script.Serialization;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Collections.Generic;
using CENTRUM.Service_Equifax;
//using CENTRUM.Service_Equifax_Test;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;
using System.Text;
using CENTRUM.Service_Equifax_CCR;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class EquifaxChking : CENTRUMBase
    {
        string CCRUserName = "", CCRPassword = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopBranch();
                txtFrmDt.Text = txtToDt.Text = txtDtTo.Text = txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                    // pnlOverride.Visible = false;
                    //pnlOverride.Enabled = false;
                    btnDone.Visible = false;
                    btnClose.Visible = false;
                }
                popRO();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Credit Bureau Operation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuHOEquiFaxStat);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = false;
                    btnClose.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = true;
                    btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDone.Visible = true;
                    btnClose.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnDone.Visible = true;
                    btnClose.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Credit Bureau Operation", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private void PopBranch()
        {
            //CGblIdGenerator oGb = null;
            CUser oUsr = null;
            DataTable dt = null;
            try
            {
                // oGb = new CGblIdGenerator();
                //dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), Session[gblValue.BrnchCode].ToString());
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]), "R");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (Convert.ToString(dr["BranchCode"]) != Session[gblValue.BrnchCode].ToString())
                        {
                            dr.Delete();
                            dt.AcceptChanges();
                        }
                    }
                }
                ddlOverrideBranch.DataSource = dt;
                ddlOverrideBranch.DataTextField = "BranchName";
                ddlOverrideBranch.DataValueField = "BranchCode";
                ddlOverrideBranch.DataBind();
            }
            finally
            {
                // oGb = null;
                oUsr = null;
                dt = null;
            }
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = ddlBranch.SelectedValue.ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            ddlCenter.Items.Clear();
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlLo.DataSource = dt;
                ddlLo.DataTextField = "EoName";
                ddlLo.DataValueField = "EoId";
                ddlLo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRO();
            //GetGrpByEo(ddlLo.SelectedValue);
        }

        protected void ddlLo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCenter(ddlLo.SelectedValue);
            //GetGrpByEo(ddlLo.SelectedValue);           
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0)
            {
                PopGrp(ddlCenter.SelectedValue);
            }
        }

        private void PopGrp(string vCenterID)
        {
            DataTable dt = null;
            CLoanRecovery oCL = null;
            try
            {
                ddlGroup.Items.Clear();

                oCL = new CLoanRecovery();
                dt = oCL.GetGroups(vCenterID, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                dt.AcceptChanges();
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "Groupid";
                ddlGroup.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CLoanRecovery oCL = null;
            try
            {
                ddlCenter.Items.Clear();
                ddlGroup.Items.Clear();
                string vBrCode;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oCL = new CLoanRecovery();
                dtGr = oCL.PopCenterWithCollDay(vCOID, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode, "W");
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oCL = null;
            }
        }

        private void GetGrpByEo(string pEoId)
        {
            DataTable dt = null;
            CCollectionRoutine oGb = null;
            try
            {
                oGb = new CCollectionRoutine();
                dt = oGb.GetGrpByEo(pEoId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlGroup.DataSource = dt;
                    ddlGroup.DataTextField = "GroupName";
                    ddlGroup.DataValueField = "Groupid";
                    ddlGroup.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                }
                else
                {
                    ddlGroup.Items.Clear();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGroup.Items.Insert(0, oli);
                }
            }
            finally
            {
                dt = null;
                oGb = null;
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            string vBrchCode = null, vEoId = null, vGroupId = null, vMarketId = null;
            CGblIdGenerator oGbl = null;
            DataTable dt = null;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrchCode = ddlBranch.SelectedValue.ToString();
                vEoId = ddlLo.SelectedIndex > 0 ? Convert.ToString(ddlLo.SelectedValue) : "";
                vMarketId = ddlCenter.SelectedIndex > 0 ? Convert.ToString(ddlCenter.SelectedValue) : "";
                vGroupId = ddlGroup.SelectedIndex > 0 ? Convert.ToString(ddlGroup.SelectedValue) : "";

                if ((vGroupId == "") && (hdGrpId.Value != "-1"))
                    vGroupId = hdGrpId.Value;

                if (vBrchCode == "-1" || vBrchCode == "")
                {
                    gblFuction.AjxMsgPopup("No Branch Selected...");
                    return;
                }
                oGbl = new CGblIdGenerator();
                dt = oGbl.GetEquiFaxSrchForAppr(vDate, vBrchCode, vEoId, vMarketId, vGroupId, txtMemberName.Text, gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text));
                gvhighmarAppr.DataSource = dt;
                gvhighmarAppr.DataBind();
            }
            finally
            {
                oGbl = null;
                dt = null;
            }
        }

        private string GetStateSrtCode(string vStateName)
        {
            switch (vStateName)
            {
                case "ANDAMAN AND NICOBAR ISLANDS": return "AN";
                case "ANDHRA PRADESH": return "AP";
                case "ARUNACHAL PRADESH": return "AR";
                case "ASSAM": return "AS";
                case "BIHAR": return "BR";
                case "CHANDIGARH": return "CH";
                case "CHHATTISGARH": return "CT";
                case "DADRA AND NAGAR HAVELI": return "DN";
                case "DAMAN AND DIU": return "DD";
                case "DELHI": return "DL";
                case "GOA": return "GA";
                case "GUJARAT": return "GJ";
                case "HARYANA": return "HR";
                case "HIMACHAL PRADESH": return "HP";
                case "JAMMU AND KASHMIR": return "JK";
                case "JHARKHAND": return "JH";
                case "KARNATAKA": return "KA";
                case "KERALA": return "KL";
                case "LAKSHADWEEP": return "LD";
                case "MADHYA PRADESH": return "MP";
                case "MAHARASHTRA": return "MH";
                case "MANIPUR": return "MN";
                case "MEGHALAYA": return "ML";
                case "MIZORAM": return "MZ";
                case "NAGALAND": return "NL";
                case "ORISSA": return "OR";
                case "PONDICHERRY": return "PY";
                case "PUNJAB": return "PB";
                case "RAJASTHAN": return "RJ";
                case "SIKKIM": return "SK";
                case "TAMIL NADU": return "TN";
                case "TRIPURA": return "TR";
                case "UTTAR PRADESH": return "UP";
                case "UTTARAKHAND": return "UK";
                case "WEST BENGAL": return "WB";
                default: return "";
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            Button btnEqVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
            TextBox txtApprvDt = (TextBox)gR.FindControl("txtApprvDt");
            Label lblStatus = (Label)gR.FindControl("lblStatus");
            DataTable dt = null;
            CApplication oCAp = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vErr = 0;
            double vLoanAppAmt = 0;
            string vMsg = "";
            string vEnqId = gR.Cells[11].Text.Trim();
            Int32 vCbId = Convert.ToInt32(gR.Cells[12].Text.Trim());
            string vEoid = gR.Cells[13].Text.Trim();
            string vMemCategory = gR.Cells[16].Text.Trim();
            Int32 vSlNo = Convert.ToInt32(gR.Cells[17].Text.Trim());
            string vBranch = ddlBranch.SelectedValue.ToString();
            DateTime vLoginDt = gblFuction.setDate(txtApprvDt.Text);
            string pErrorMsg = "";
            int pStatus = 0;
            string pStatusDesc = "";
            string pCbType = Convert.ToString(Session[gblValue.CbType]);
            Int32 vErr2 = 0;
            oCAp = new CApplication();
            CEquiFaxDataSubmission oEqui = new CEquiFaxDataSubmission();

            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

            if (vMemCategory == "Member")
            {
                dt = oCAp.GetMemberInfo(vEnqId, pCbType);
            }
            else if (vMemCategory == "CoAppMem")
            {
                dt = oCAp.GetInitialApprOthMemDtlForCB(vEnqId, vCbId, vMemCategory, vSlNo);
            }
            else if (vMemCategory == "OtherMem")
            {
                dt = oCAp.GetInitialApprOthMemDtlForCB(vEnqId, vCbId, vMemCategory, vSlNo);
            }

            if (dt.Rows.Count > 0)
            {
                if (pCbType == "E")
                {
                    string pEqXml = "";
                    try
                    {
                        //*************************** For Live ***************************************************                      
                        //WSEquifaxMCRSoapClient eq = new WSEquifaxMCRSoapClient();
                        //pEqXml = eq.EquifaxVerification(dt.Rows[0]["MemberName"].ToString(), dt.Rows[0]["FamilyPersonName"].ToString(), dt.Rows[0]["HF_YN"].ToString(),
                        //                dt.Rows[0]["MemAdd"].ToString(), GetStateSrtCode(dt.Rows[0]["StateName"].ToString().ToUpper()), dt.Rows[0]["PIN"].ToString(),
                        //                dt.Rows[0]["DOB"].ToString(), dt.Rows[0]["VoterId"].ToString(), dt.Rows[0]["UID"].ToString(),
                        //                5750, "STSCENTRUM", "hg*uy56GF", "027FZ01546", "KQ7", "MCR", "1.0", "1234", "Yes", "MFI", dt.Rows[0]["MobileNo"].ToString());

                        WebServiceSoapClient eq = new WebServiceSoapClient();

                        if (vMemCategory == "Member")
                        {
                            pEqXml = eq.Equifax(
                            dt.Rows[0]["MemFName"].ToString(), dt.Rows[0]["MemMName"].ToString(), dt.Rows[0]["MemLName"].ToString()
                            , dt.Rows[0]["MemDOB"].ToString(), dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["MemAddress"].ToString()
                            , dt.Rows[0]["MemState"].ToString(), dt.Rows[0]["MemPinCode"].ToString(), dt.Rows[0]["MemMobile"].ToString()
                            , "Voter ID", dt.Rows[0]["MemVoterId"].ToString(), "Aadhaar", dt.Rows[0]["MemUID"].ToString()
                            , dt.Rows[0]["GuardianRelation"].ToString(), dt.Rows[0]["GuardianName"].ToString()
                            , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                            //*************************************************************************

                            oCAp = new CApplication();
                            vErr = oCAp.UpdateEquifaxInformation(vEnqId, vCbId, pEqXml, vBranch, vEoid, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                            if (vErr == 1)
                            {
                                string[] arr = pStatusDesc.Split(',');
                                string[] arr2 = arr[4].Split('=');
                                string vAcceptYN = arr2[1].Trim();
                                if (vAcceptYN == "Y")
                                {
                                    lblStatus.Text = "Approved";
                                    txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                                    btnEqVerify.Text = "Verified";
                                    btnEqVerify.Enabled = false;
                                }
                                else
                                {
                                    lblStatus.Text = "Cancel";
                                    txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                                    btnEqVerify.Text = "Cancel";
                                    btnEqVerify.Enabled = false;
                                }
                                gblFuction.MsgPopup(pStatusDesc);
                            }
                            if (vErr == 5)
                            {
                                gblFuction.MsgPopup("Data Not Saved, Data Error...");
                            }
                        }
                        else if ((vMemCategory == "CoAppMem") || (vMemCategory == "OtherMem"))
                        {
                            pEqXml = eq.Equifax(
                            dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(),
                            dt.Rows[0]["DOB"].ToString(), dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["Address"].ToString(),
                            dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PinCode"].ToString(), dt.Rows[0]["MobileNo"].ToString(),
                            dt.Rows[0]["IDType1"].ToString(), dt.Rows[0]["IDNo1"].ToString(), dt.Rows[0]["IDType2"].ToString(),
                            dt.Rows[0]["IDNo2"].ToString(), dt.Rows[0]["RelType"].ToString(), dt.Rows[0]["RelName"].ToString(),
                            "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                            if (pEqXml.Equals(""))
                            {
                            }
                            else
                            {
                                oCAp = new CApplication();
                                vErr2 = oCAp.UpdateEquifaxInformationOtherMem(vEnqId, vCbId, pEqXml, vBranch, vEoid, Convert.ToInt32(Session[gblValue.UserId]),
                                        vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc, vMemCategory, vSlNo);
                                if (vErr2 == 1)
                                {
                                    lblStatus.Text = "Verified";
                                    txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                                    btnEqVerify.Text = "Verified";
                                    btnEqVerify.Enabled = false;
                                    gblFuction.MsgPopup(pStatusDesc);
                                }
                                if (vErr2 == 5)
                                {
                                    gblFuction.MsgPopup("Data Not Saved, Data Error...");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        pErrorMsg = ex.ToString();
                    }
                }
                else
                {
                    try
                    {
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        string responsedata = string.Empty;
                        string userId = "in_cpuuat_cenmfi3";
                        string password = "9DDB522C741D8F3192D53B13D9FB590B02723024";
                        string mbrid = "MFI0000228";
                        string productType = "INDV";
                        string productVersion = "1.0";
                        string reqVolType = "INDV";
                        string SUB_MBR_ID = "CENTRUM MICROCREDIT PRIVATE LIMITED";

                        string request_datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                        string vAppDate = Session[gblValue.LoginDate].ToString();
                        string vAadhar = "", vVoterId = "";
                        vVoterId = Convert.ToString(dt.Rows[0]["VoterId"]);
                        vAadhar = Convert.ToString(dt.Rows[0]["UID"]);

                        string HEADER_SEGMENT = "<HEADER-SEGMENT>"
                                                  + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                                  + "<INQ-DT-TM>" + request_datetime + "</INQ-DT-TM>"
                                                  + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                                                  + "<REQ-ACTN-TYP>SUBMIT</REQ-ACTN-TYP>"
                                                  + "<TEST-FLG>N</TEST-FLG>"
                                                  + "<AUTH-FLG>Y</AUTH-FLG>"
                                                  + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                                  + "<RES-FRMT>XML/HTML</RES-FRMT>"
                                                  + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                                                  + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                                                  + "<MFI>"
                                                  + "<INDV>TRUE</INDV>"
                                                  + "<SCORE>FALSE</SCORE>"
                                                  + "<GROUP>TRUE</GROUP>"
                                                  + "</MFI>"
                                                  + "<CONSUMER>"
                                                  + "<INDV>TRUE</INDV>"
                                                  + "<SCORE>TRUE</SCORE>"
                                                  + "</CONSUMER>"
                                                  + "<IOI>TRUE</IOI>"
                                                  + "</HEADER-SEGMENT>";

                        string APPLICANT_SEGMENT = "<APPLICANT-SEGMENT>"
                                                    + "<APPLICANT-NAME><NAME1>" + dt.Rows[0]["MemberName"] + "</NAME1>"
                                                    + "<NAME2></NAME2><NAME3></NAME3><NAME4></NAME4><NAME5></NAME5></APPLICANT-NAME>"
                                                    + "<DOB><DOB-DATE>" + dt.Rows[0]["DOB"] + "</DOB-DATE>"
                                                    + "<AGE>" + dt.Rows[0]["Age"] + "</AGE>"
                                                    + "<AGE-AS-ON>" + dt.Rows[0]["AsOnDate"] + "</AGE-AS-ON></DOB>"
                                                    + "<IDS>"
                                                    + "<ID><TYPE>ID03</TYPE><VALUE>" + vAadhar + "</VALUE></ID>" //aadhaar no                                                               
                                                    + ((vVoterId == "") ? "" : "<ID><TYPE>ID02</TYPE><VALUE>" + vVoterId + "</VALUE></ID>") //voter id (if available)                                                              
                                                    + "</IDS>"
                                                    + "<RELATIONS><RELATION><NAME>" + dt.Rows[0]["RelativeName"] + "</NAME><TYPE>" + dt.Rows[0]["RelationCode"] + "</TYPE>"
                                                    + "</RELATION></RELATIONS><PHONES><PHONE><TELE-NO>" + dt.Rows[0]["MobileNo"] + "</TELE-NO>"
                                                    + "<TELE-NO-TYPE>P03</TELE-NO-TYPE></PHONE></PHONES>"
                                                    + "</APPLICANT-SEGMENT>";

                        string ADDRESS_SEGMENT = "<ADDRESS-SEGMENT><ADDRESS><TYPE>D01</TYPE><ADDRESS-1>" + dt.Rows[0]["MemAdd"] + "</ADDRESS-1>"
                                                    + "<CITY>" + dt.Rows[0]["District"] + "</CITY><STATE>" + dt.Rows[0]["StateCode"] + "</STATE><PIN>"
                                                    + dt.Rows[0]["PIN"] + "</PIN></ADDRESS></ADDRESS-SEGMENT>";

                        string APPLICATION_SEGMENT = "<APPLICATION-SEGMENT><INQUIRY-UNIQUE-REF-NO>" + vEnqId + "/" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "</INQUIRY-UNIQUE-REF-NO>"
                                                   + "<CREDT-INQ-PURPS-TYP>ACCT-ORIG</CREDT-INQ-PURPS-TYP><CREDIT-INQUIRY-STAGE>PRE-SCREEN</CREDIT-INQUIRY-STAGE>"
                                                   + "<CREDT-REQ-TYP>INDV</CREDT-REQ-TYP><BRANCH-ID>" + vBranch + "</BRANCH-ID>"
                                                   + "<LOS-APP-ID></LOS-APP-ID>"
                                                   + "<LOAN-AMOUNT></LOAN-AMOUNT></APPLICATION-SEGMENT>";

                        string INQUIRY = "<INQUIRY>" + APPLICANT_SEGMENT + ADDRESS_SEGMENT + APPLICATION_SEGMENT + "</INQUIRY>";

                        string requestXML = "<REQUEST-REQUEST-FILE>" + HEADER_SEGMENT + INQUIRY + "</REQUEST-REQUEST-FILE>";

                        var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                        // var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live

                        httpWebRequest.ContentType = "application/xml; charset=utf-8";
                        httpWebRequest.Accept = "application/xml";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.PreAuthenticate = true;
                        httpWebRequest.Headers.Add("requestXML", requestXML);
                        httpWebRequest.Headers.Add("userId", userId);
                        httpWebRequest.Headers.Add("password", password);
                        httpWebRequest.Headers.Add("mbrid", mbrid);
                        httpWebRequest.Headers.Add("productType", productType);
                        httpWebRequest.Headers.Add("productVersion", productVersion);
                        httpWebRequest.Headers.Add("reqVolType", reqVolType);

                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            var highmarkresult = streamReader.ReadToEnd();
                            responsedata = highmarkresult.ToString().Trim();
                        }

                        string vErrDescResponse1 = "";
                        int vResponse1 = 0;
                        vResponse1 = oEqui.SaveHighmarkResponse(vEnqId, responsedata, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBranch, Convert.ToInt32(Session[gblValue.UserId].ToString()), ref vErrDescResponse1, 0);
                        if (vResponse1 > 0)
                        {
                            gblFuction.AjxMsgPopup("Alert! : " + vErrDescResponse1 + ". Highmark Response Type-1 Error...");
                            return;
                        }

                        System.Threading.Thread.Sleep(10000);

                        //Report Part
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(responsedata);

                        XmlNodeList elemList = xd.GetElementsByTagName("INQUIRY-UNIQUE-REF-NO");
                        string INQUIRY_UNIQUE_REF_NO = elemList[0].InnerText;

                        elemList = xd.GetElementsByTagName("REPORT-ID");
                        string REPORT_ID = elemList[0].InnerText;

                        elemList = xd.GetElementsByTagName("RESPONSE-DT-TM");
                        string RESPONSE_DT_TM = elemList[0].InnerText;

                        elemList = xd.GetElementsByTagName("RESPONSE-TYPE");
                        string RESPONSE_TYPE = elemList[0].InnerText;

                        string REQUEST_REQUEST_FILE = "<REQUEST-REQUEST-FILE>"
                                                + "<HEADER-SEGMENT>"
                                                + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                                + "<INQ-DT-TM>" + RESPONSE_DT_TM + "</INQ-DT-TM>"
                                                + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                                                + "<REQ-ACTN-TYP>ISSUE</REQ-ACTN-TYP>"
                                                + "<TEST-FLG>N</TEST-FLG>"
                                                + "<AUTH-FLG>Y</AUTH-FLG>"
                                                + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                                + "<RES-FRMT>XML/HTML</RES-FRMT>"
                                                + "<MEMBER-PRE-OVERRIDE>Y</MEMBER-PRE-OVERRIDE>"
                                                + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                                                + "</HEADER-SEGMENT>"
                                                + "<INQUIRY>"
                                                + "<INQUIRY-UNIQUE-REF-NO>" + INQUIRY_UNIQUE_REF_NO + "</INQUIRY-UNIQUE-REF-NO>"
                                                + "<REQUEST-DT-TM>" + RESPONSE_DT_TM + "</REQUEST-DT-TM>"
                                                + "<REPORT-ID>" + REPORT_ID + "</REPORT-ID>"
                                                + "</INQUIRY>"
                                                + "</REQUEST-REQUEST-FILE>";

                        string finalXml = REQUEST_REQUEST_FILE;

                        var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                        //var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live                                                
                        httpWebRequestFinal.ContentType = "application/xml; charset=utf-8";
                        httpWebRequestFinal.Method = "POST";
                        httpWebRequestFinal.PreAuthenticate = true;
                        httpWebRequestFinal.Headers.Add("requestXML", finalXml);
                        httpWebRequestFinal.Headers.Add("userId", userId);
                        httpWebRequestFinal.Headers.Add("password", password);
                        httpWebRequestFinal.Headers.Add("mbrid", mbrid);
                        httpWebRequestFinal.Headers.Add("productType", productType);
                        httpWebRequestFinal.Headers.Add("productVersion", productVersion);
                        httpWebRequestFinal.Headers.Add("reqVolType", reqVolType);

                        var httpResponseFinal = (HttpWebResponse)httpWebRequestFinal.GetResponse();
                        string responsedataFinal = string.Empty;
                        using (var streamReader = new StreamReader(httpResponseFinal.GetResponseStream(), Encoding.UTF8))
                        {
                            var highmarkresult = streamReader.ReadToEnd();
                            responsedataFinal = highmarkresult.ToString().Trim();
                        }

                        oEqui = new CEquiFaxDataSubmission();
                        vResponse1 = oEqui.SaveDetailsHighmarkResponse(vEnqId, INQUIRY_UNIQUE_REF_NO, responsedataFinal, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBranch, Convert.ToInt32(Session[gblValue.UserId].ToString()), ref vErrDescResponse1);
                        if (vResponse1 > 0)
                        {
                            gblFuction.AjxMsgPopup("Alert! : " + vErrDescResponse1 + ". Highmark Response Type-2 Error...");
                            return;
                        }
                        else
                        {
                            pStatusDesc = vErrDescResponse1;
                        }
                        //------------------------------------------------------------------------------------------------------------------------------                  
                        //*************************************************************************

                        string[] arr1 = pStatusDesc.Split(',');
                        string[] arr3 = arr1[4].Split('=');
                        string vAcptYN = arr3[1].Trim();
                        if (vAcptYN == "Yes")
                        {
                            lblStatus.Text = "Approved";
                            txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                            btnEqVerify.Text = "Verified";
                            btnEqVerify.Enabled = false;
                        }
                        else
                        {
                            lblStatus.Text = "Cancel";
                            txtApprvDt.Text = Session[gblValue.LoginDate].ToString();
                            btnEqVerify.Text = "Cancel";
                            btnEqVerify.Enabled = false;
                        }
                        gblFuction.MsgPopup(pStatusDesc);

                    }
                    catch (Exception ex)
                    {
                        gblFuction.AjxMsgPopup("Highmark Response Error..." + ex.ToString());
                    }

                }

            }

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            Button btnDownload = (Button)sender;
            GridViewRow gR = (GridViewRow)btnDownload.NamingContainer;
            string vEnqId = gR.Cells[11].Text.Trim();
            Int32 vCbId = Convert.ToInt32(gR.Cells[12].Text.Trim());
            string vType = Convert.ToString(gR.Cells[15].Text.Trim());
            string vMemCategory = gR.Cells[16].Text.Trim();
            Int32 vSlNo = Convert.ToInt32(gR.Cells[17].Text.Trim());
            TextBox txtApprvDt = (TextBox)gR.FindControl("txtApprvDt");

            if (vType == "E")
            {
                //SetParameterForRptData(vEnqId, vCbId, "PDF", vMemCategory, vSlNo, gblFuction.setDate(txtApprvDt.Text));
                SetParameterForRptDataNew(vEnqId, vCbId, "PDF", vMemCategory, vSlNo, gblFuction.setDate(txtApprvDt.Text));
            }
            else
            {
                string url = "PopUpReportWindow.aspx?EnqId=" + vEnqId + "&CBID=" + vCbId;
                ClientScript.RegisterStartupScript(this.Page.GetType(), "script", "window.open('" + url + "');", true);
            }
        }

        private void SetParameterForRptData(string pEnquiryId, int pCbAppId, string pType, string pMemCategory, Int32 pSlNo, DateTime ReportDate)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CReports oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                string enqstatusmsg = "";

                if (ReportDate <= gblFuction.setDate("01/07/2022"))
                {
                    ds = oRpt.Equifax_Report(pEnquiryId, pCbAppId, ref  enqstatusmsg, Convert.ToInt32(Session[gblValue.UserId]));
                }
                else
                {
                    ds = oRpt.Equifax_Report_CCR(pEnquiryId, pCbAppId, ref enqstatusmsg, pMemCategory, pSlNo, Convert.ToInt32(Session[gblValue.UserId]));
                }
                if (!String.IsNullOrEmpty(enqstatusmsg))
                {
                    gblFuction.MsgPopup("Equifax Error : " + enqstatusmsg);
                    return;
                }
                else
                {
                    if (ds.Tables.Count == 2 && ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                            gblFuction.AjxMsgPopup("New User");
                            return;
                        }
                    }
                }
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                //-----------------------------------------------------------------------
                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt2.Rows) // search whole table
                    {
                        if (Convert.ToInt32(dr["IdentyPRofId"].ToString()) == 1)
                        {
                            dr["VoterId"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["VoterId"]).Substring(Convert.ToString(dt2.Rows[0]["VoterId"]).Length - 4, 4)); ; //change the name                           
                        }
                        if (Convert.ToInt32(dr["AddProfId"].ToString()) == 1)
                        {
                            dr["UID"] = String.Format("{0}{1}", "********", Convert.ToString(dt2.Rows[0]["UID"]).Substring(Convert.ToString(dt2.Rows[0]["UID"]).Length - 4, 4)); ; //change the name                           
                        }
                    }
                }
                //-------------------------------------------------------------------------

                dt1.TableName = "CBPortDtl";
                dt2.TableName = "CBPortMst";
                dt3.TableName = "CBHistoryMonth";
                if (pType == "PDF")
                {
                    if (ReportDate <= gblFuction.setDate("01/07/2022"))
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                    }
                    else
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_CCR.rpt";
                    }
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");
                        Response.ClearHeaders();
                        Response.ClearContent();
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
                dt2 = null;
                oRpt = null;
            }
        }

        private void SetParameterForRptDataNew(string pEnquiryId, int pCbAppId, string pType, string pMemCategory, Int32 pSlNo, DateTime ReportDate)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null, dt8 = null;
            CReports oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                string enqstatusmsg = "";

                if (ReportDate <= gblFuction.setDate("01/07/2022"))
                {
                    ds = oRpt.Equifax_Report(pEnquiryId, pCbAppId, ref enqstatusmsg,Convert.ToInt32(Session[gblValue.UserId].ToString()));
                }
                else
                {
                    ds = oRpt.Equifax_Report_CCR_NEW(pEnquiryId, pCbAppId, ref enqstatusmsg, pMemCategory, pSlNo);
                }
                if (!String.IsNullOrEmpty(enqstatusmsg))
                {
                    gblFuction.MsgPopup("Equifax Error : " + enqstatusmsg);
                    return;
                }
                else
                {
                    if (ds.Tables.Count == 2 && ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                            gblFuction.AjxMsgPopup("New User");
                            return;
                        }
                    }
                }
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                if (ReportDate > gblFuction.setDate("01/07/2022"))
                {
                    dt4 = ds.Tables[3];
                    dt5 = ds.Tables[4];
                    dt6 = ds.Tables[5];
                    dt7 = ds.Tables[6];
                    dt8 = ds.Tables[7];


                    //-------------------------------------------------------------------------

                    dt4.TableName = "dtRepeatCustTrack";
                    dt5.TableName = "dtRepeatCustTrackTimeline";
                    dt6.TableName = "dtIndLoanDtl";
                    dt7.TableName = "dtRePayTimeline";
                }
                else
                {
                    dt1.TableName = "CBPortDtl";
                    dt2.TableName = "CBPortMst";
                    dt3.TableName = "CBHistoryMonth";
                }

                if (pType == "PDF")
                {
                    if (ReportDate <= gblFuction.setDate("01/07/2022"))
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                    }
                    else
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptEquifaxCCR.rpt";
                    }
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        if (ReportDate <= gblFuction.setDate("01/07/2022"))
                        {
                            rptDoc.SetDataSource(ds);
                        }
                        else
                        {
                            rptDoc.SetDataSource(dt1);
                            rptDoc.Subreports["rptActiveLoan"].SetDataSource(dt2);
                            rptDoc.Subreports["RptCloseLoan"].SetDataSource(dt3);
                            rptDoc.Subreports["rptRepeatCustTrack"].SetDataSource(ds);
                            rptDoc.Subreports["RptIndivLoanDtl"].SetDataSource(ds);
                            rptDoc.Subreports["RptEnquiries"].SetDataSource(dt8);
                        }
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");
                        Response.ClearHeaders();
                        Response.ClearContent();
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
                dt2 = null;
                oRpt = null;
            }
        }

        protected void gvhighmarAppr_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Button btnVerify = (Button)e.Row.FindControl("btnVerify");
                if (e.Row.Cells[7].Text.Trim() == "Y")
                {
                    lblStatus.Text = "Approved";
                    btnVerify.Enabled = false;
                    btnVerify.Text = "Verified";
                }
                else if (e.Row.Cells[7].Text.Trim() == "C")
                {
                    lblStatus.Text = "Cancel";
                    btnVerify.Enabled = false;
                    btnVerify.Text = "Cancel";
                }
                else
                {
                    lblStatus.Text = "New";
                    btnVerify.Enabled = true;
                }
            }
        }

        //************************************Override******************************
        protected void btnShow_Click(object sender, EventArgs e)
        {
            try
            {
                string vBrCode = ddlOverrideBranch.SelectedValues.Replace("|", ",");
                if (rdbOpt.SelectedValue == "N")
                {
                    LoadGrid("N", vBrCode);
                    btnRedFlag.Visible = false;
                }
                else if (rdbOpt.SelectedValue == "A")
                {
                    LoadGrid("A", vBrCode);
                    btnRedFlag.Visible = false;
                }
                else if (rdbOpt.SelectedValue == "R")
                {
                    LoadGrid("R", vBrCode);
                    if (gvCredit.Rows.Count > 0)
                    {
                        CheckBox chkboxSelAll = (CheckBox)gvCredit.HeaderRow.FindControl("chkboxSelAll");
                        chkboxSelAll.Checked = true;
                        if (Session[gblValue.BrnchCode].ToString() != "0000")
                        {
                            btnRedFlag.Visible = false;
                        }
                        else
                        {
                            btnRedFlag.Visible = true;
                        }
                    }
                    else
                    {
                        btnRedFlag.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void LoadGrid(string pAppMode, string pBranch)
        {
            DataTable dt = null;
            CcreditbureauApproveReject oCB = null;
            try
            {
                string vBrCode = pBranch;
                oCB = new CcreditbureauApproveReject();
                dt = oCB.GetCreditbureauAppRej(gblFuction.setDate(txtDtFrm.Text), gblFuction.setDate(txtDtTo.Text), pAppMode, pBranch, txtOverrideMember.Text);
                ViewState["Sanc"] = dt;
                gvCredit.DataSource = dt;
                gvCredit.DataBind();
            }
            finally
            {
                dt = null;
                oCB = null;
            }
        }

        protected void gvCredit_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkApprove = (CheckBox)e.Row.FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)e.Row.FindControl("chkReject");
                    TextBox txtAppReason = (TextBox)e.Row.FindControl("txtAppReason");
                    TextBox txtRejectReason = (TextBox)e.Row.FindControl("txtRejectReason");
                    if (e.Row.Cells[10].Text == "Y")
                    {
                        chkApprove.Checked = true;
                        txtAppReason.Enabled = true;
                        chkReject.Checked = false;
                        chkReject.Enabled = false;
                        txtRejectReason.Enabled = false;
                    }
                    else if (e.Row.Cells[10].Text == "N")
                    {
                        chkApprove.Checked = false;
                        chkReject.Enabled = true;

                    }

                    if (e.Row.Cells[13].Text == "Y")
                    {
                        chkReject.Checked = true;
                        chkApprove.Enabled = false;
                        txtRejectReason.Enabled = true;
                    }
                    else if (e.Row.Cells[13].Text == "N")
                    {
                        chkReject.Checked = false;
                        chkReject.Enabled = false;
                        chkApprove.Enabled = true;
                    }
                    if (txtRejectReason.Text.Trim() == "UnTag Red Flag")
                    {
                        txtRejectReason.Enabled = false;
                        chkReject.Enabled = false;
                        chkApprove.Enabled = false;
                        txtAppReason.Enabled = false;
                    }

                }
            }
            finally
            {

            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            CcreditbureauApproveReject obA = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            string vBrCode = ddlOverrideBranch.SelectedValues.Replace("|", ",");
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                if (dt == null) return;
                int cnt = dt.Rows.Count;

                for (int r = 0; r < cnt; r++)
                {
                    CheckBox chkApprove = (CheckBox)gvCredit.Rows[r].FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)gvCredit.Rows[r].FindControl("chkReject");

                    if (chkApprove.Checked == false && chkReject.Checked == false)
                    {
                        string vMember = gvCredit.Rows[r].Cells[4].Text;
                        gblFuction.AjxMsgPopup("For member " + vMember + " Approved or Reject both Should not be Unchecked.");
                        return;
                    }
                    TextBox txtAmountApplied = (TextBox)gvCredit.Rows[r].FindControl("txtAmountApplied");
                    TextBox txtAppReason = (TextBox)gvCredit.Rows[r].FindControl("txtAppReason");
                    TextBox txtRejectReason = (TextBox)gvCredit.Rows[r].FindControl("txtRejectReason");

                    if (chkApprove.Checked)
                    {
                        if (txtAppReason.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Approved reason cannot be left blank.");
                            return;
                        }
                        dt.Rows[r]["IsApproved"] = "Y";
                        if (txtAmountApplied.Text != string.Empty && Convert.ToDouble(txtAmountApplied.Text) <= 60000.00)
                        {
                            dt.Rows[r]["AmountApplied"] = txtAmountApplied.Text;
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Applied Amount  cannot be greater than 60000.");
                            return;
                        }
                        dt.Rows[r]["ApprovalReason"] = txtAppReason.Text;
                        dt.Rows[r]["IsReject"] = "N";
                    }
                    else if (chkReject.Checked)
                    {
                        if (txtRejectReason.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Reject reason cannot be left blank.");
                            return;
                        }
                        dt.Rows[r]["IsReject"] = "Y";
                        dt.Rows[r]["RejectReason"] = txtRejectReason.Text;
                        dt.Rows[r]["IsApproved"] = "N";
                    }
                    else
                    {
                        dt.Rows[r]["IsReject"] = null;
                    }
                }

                dt.AcceptChanges();
                obA = new CcreditbureauApproveReject();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                string vMessage = obA.chkPreApproveLoan(vXmlData);
                if (vMessage.Length > 0)
                {
                    gblFuction.AjxMsgPopup(vMessage);
                    return;
                }

                //-----------XML Save----------
                vErr = obA.SaveCreditbureauApprove(vXmlData, Convert.ToInt32(Session[gblValue.UserId]));
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid("N", vBrCode);
                    rdbOpt.SelectedValue = "N";
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            finally
            {
                obA = null;
                dt = null;
            }

        }

        protected void btnRedFlag_Click(object sender, EventArgs e)
        {
            CcreditbureauApproveReject obA = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            string vBrCode = ddlOverrideBranch.SelectedValues.Replace("|", ",");
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                if (dt == null) return;
                int cnt = dt.Rows.Count;
                for (int r = 0; r < cnt; r++)
                {
                    CheckBox chkApprove = (CheckBox)gvCredit.Rows[r].FindControl("chkApprove");
                    CheckBox chkReject = (CheckBox)gvCredit.Rows[r].FindControl("chkReject");
                    TextBox txtAppReason = (TextBox)gvCredit.Rows[r].FindControl("txtAppReason");
                    TextBox txtRejectReason = (TextBox)gvCredit.Rows[r].FindControl("txtRejectReason");

                    if (chkApprove.Checked)
                    {
                        dt.Rows[r]["IsApproved"] = "Y";
                        dt.Rows[r]["ApprovalReason"] = txtAppReason.Text;
                        dt.Rows[r]["IsReject"] = "N";
                    }
                    else if (chkReject.Checked)
                    {
                        if (txtRejectReason.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Reject reason cannot be left blank.");
                            return;
                        }
                        dt.Rows[r]["IsReject"] = "Y";
                        dt.Rows[r]["RejectReason"] = txtRejectReason.Text;
                        dt.Rows[r]["IsApproved"] = "N";
                    }
                    else
                    {
                        dt.Rows[r]["IsReject"] = null;
                    }
                }

                dt.AcceptChanges();
                obA = new CcreditbureauApproveReject();
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                //-----------XML Save----------
                vErr = obA.SaveRedFlagFrmOverride(vXmlData, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid("N", vBrCode);
                    rdbOpt.SelectedValue = "N";
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                }
            }
            finally
            {
                obA = null;
                dt = null;
            }

        }

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApprove");
                CheckBox chkReject = (CheckBox)row.FindControl("chkReject");
                TextBox txtAppReason = (TextBox)row.FindControl("txtAppReason");
                TextBox txtRejectReason = (TextBox)row.FindControl("txtRejectReason");
                TextBox txtAmountApplied = (TextBox)row.FindControl("txtAmountApplied");

                if (chkApp.Checked == true)
                {
                    row.Cells[10].Text = "Y";
                    txtAppReason.Text = "";
                    txtAppReason.Enabled = true;
                    chkReject.Checked = false;
                    chkReject.Enabled = false;
                    txtRejectReason.Text = "";
                    txtRejectReason.Enabled = false;
                }
                else
                {
                    row.Cells[10].Text = "N";
                    txtAppReason.Text = "";
                    txtAppReason.Enabled = false;
                    chkReject.Checked = false;
                    chkReject.Enabled = true;
                    txtRejectReason.Text = "";
                    txtRejectReason.Enabled = false;
                }

            }
            finally
            {
            }
        }

        protected void chkReject_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApprove");
                CheckBox chkReject = (CheckBox)row.FindControl("chkReject");
                TextBox txtAppReason = (TextBox)row.FindControl("txtAppReason");
                TextBox txtRejectReason = (TextBox)row.FindControl("txtRejectReason");
                if (chkReject.Checked == true)
                {
                    row.Cells[13].Text = "Y";
                    chkReject.Checked = true;
                    txtAppReason.Text = "";
                    txtAppReason.Enabled = false;
                    chkApp.Checked = false;
                    chkApp.Enabled = false;
                    txtRejectReason.Enabled = true;
                }
                else
                {
                    row.Cells[13].Text = "N";
                    txtRejectReason.Text = "";
                    txtRejectReason.Enabled = false;
                    chkReject.Checked = false;
                    chkReject.Enabled = true;
                    chkApp.Enabled = true;
                }

            }
            finally
            {
            }
        }

    }
}