using System;
using System.Data;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Web.UI;
using CENTRUMCF;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Configuration;
using System.Web.Hosting;

namespace CENTRUMCF
{
    public partial class InitiateDigitalDoc : CENTRUMBAse
    {
        static string _vMobileNo, _vCoAppMobNo;
        string vpathDigiDocImage = "";
        string vpathCreateNewDoc = "";

        string DigiDocOtpBucket = ConfigurationManager.AppSettings["DigiDocOtpBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        string DocumentURL = ConfigurationManager.AppSettings["DocumentURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnOTP.Value = String.Empty;
                String encURL = "";
                String _vDocStatus = "N";
                CDigiDoc oDD = null;
                Int32 _vInActiveToken;
                try
                {
                    oDD = new CDigiDoc();
                    encURL = Request.RawUrl;
                    encURL = encURL.Substring(encURL.IndexOf('?') + 1);
                    if (!encURL.Equals(""))
                    {
                        if (encURL.Contains("&"))
                        {
                            String[] queryStrParam1 = encURL.Split('&');
                            String[] queryStr1 = queryStrParam1[0].Split('=');
                            String[] queryStr2 = queryStrParam1[1].Split('=');
                            if (queryStr1.Length > 1)
                                hdnLoanAppNo.Value = queryStr1[1].ToString();
                            if (queryStr2.Length > 1)
                                hdnToken.Value = queryStr2[1].ToString();

                            // Check already Digitally Signed or not
                            _vDocStatus = oDD.DigitalSignStatus(hdnLoanAppNo.Value.Trim());
                            if (_vDocStatus.Trim().ToLower() == "y")
                            {
                                btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                                btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                txtOTP.Attributes.Add("style", "visibility:hidden;");
                                txtCOTP.Attributes.Add("style", "visibility:hidden;");
                                spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                                btnCapture.Attributes.Add("style", "visibility:hidden;");
                                btnUpload.Attributes.Add("style", "visibility:hidden;");
                                btnResetCam.Attributes.Add("style", "visibility:hidden;");
                                btnSubmit.Attributes.Add("style", "visibility:hidden;");
                                gblFuction.AjxMsgPopup("Digital Document already signed..");
                                return;
                            }

                            if (chkPageTimeStamp(hdnLoanAppNo.Value, hdnToken.Value) == true)
                            {
                                // ************ If 15 Minute Is Valid ***********//
                                btnLinkDownloadDocument.Attributes.Add("style", "visibility:Visible;");
                                btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                txtOTP.Attributes.Add("style", "visibility:hidden;");
                                txtCOTP.Attributes.Add("style", "visibility:hidden;");
                                spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                                btnCapture.Attributes.Add("style", "visibility:hidden;");
                                btnUpload.Attributes.Add("style", "visibility:hidden;");
                                btnSubmit.Attributes.Add("style", "visibility:hidden;");
                                btnResetCam.Attributes.Add("style", "visibility:hidden;");
                            }
                            else
                            {
                                // ************  Inactive The Called Token if 15 Minute Expired ***********//
                                _vInActiveToken = oDD.InActiveDigitalDoc(hdnLoanAppNo.Value, hdnToken.Value);
                                if (_vInActiveToken > 0)
                                {
                                    btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                                    btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                    txtOTP.Attributes.Add("style", "visibility:hidden;");
                                    txtCOTP.Attributes.Add("style", "visibility:hidden;");
                                    spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                                    btnCapture.Attributes.Add("style", "visibility:hidden;");
                                    btnUpload.Attributes.Add("style", "visibility:hidden;");
                                    btnResetCam.Attributes.Add("style", "visibility:hidden;");
                                    btnSubmit.Attributes.Add("style", "visibility:hidden;");
                                    gblFuction.AjxMsgPopup("Token Expired..");
                                    Page.ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/javascript'>window.close();</script>");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                            btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                            txtOTP.Attributes.Add("style", "visibility:hidden;");
                            txtCOTP.Attributes.Add("style", "visibility:hidden;");
                            spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                            btnCapture.Attributes.Add("style", "visibility:hidden;");
                            btnUpload.Attributes.Add("style", "visibility:hidden;");
                            btnResetCam.Attributes.Add("style", "visibility:hidden;");
                            btnSubmit.Attributes.Add("style", "visibility:hidden;");
                            gblFuction.AjxMsgPopup("Page not contain parameter");
                            Page.ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/javascript'>window.close();</script>");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                finally
                {
                    oDD = null;
                }
            }
        }
        #region "Checking 15 Minute Validation Of Token"
        private Boolean chkPageTimeStamp(String vLoanAppNo, String vToken)
        {
            Boolean _Valid = true;
            CDigiDoc oUsr = null;
            DataTable dtRecord = null;
            DataTable dtValid = null;
            DataSet ds = null;
            try
            {
                oUsr = new CDigiDoc();
                ds = oUsr.getDigitalDocByToken(vLoanAppNo, vToken);
                dtRecord = ds.Tables[0];
                dtValid = ds.Tables[1];
                if (dtValid.Rows.Count > 0)
                {
                    _Valid = (dtValid.Rows[0]["ValidOrNot"].ToString() == "Y") ? true : false; // true; //
                    hdnDigiDocId.Value = (_Valid == true) ? dtRecord.Rows[0]["DigiDocId"].ToString() : "0";
                    hdnCustomerName.Value = (_Valid == true) ? dtRecord.Rows[0]["CustomerName"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                oUsr = null;
            }
            return _Valid;
        }
        #endregion

        #region "Dowloan Digital Document (A)"
        protected void btnLinkDownloadDocument_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null
            , dt8 = null, dt9 = null, dt10 = null, dt11 = null, dt12 = null, dt13 = null, dt14 = null, dt15 = null,
            dt16 = null, dt17 = null, dt18 = null, dtScheduleOfCharges = null, dtSchedule = null;

            CReports oRpt = new CReports();
            CMember oMem = null;
            string vRptPath = "", vRptName = "";
            CDigiDoc oUsr = null;
            hdnOTP.Value = String.Empty;
            hdnSMSVerifyTimeStamp.Value = String.Empty;
            try
            {
                oUsr = new CDigiDoc();
                string vSancId = oUsr.GetSanctionId(hdnLoanAppNo.Value);

                if (vSancId == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                if (vSancId == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                if (vSancId == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                else
                {
                    vRptPath = string.Empty;
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocs.rpt";
                    DataTable dt = new DataTable();
                    oMem = new CMember();
                    ds = new DataSet();
                    ds = oMem.GetAgrDigitalDocs(vSancId, 0);
                    dtAppFrm1 = ds.Tables[0];
                    dtAppFrm2 = ds.Tables[1];
                    dtSancLetter = ds.Tables[2];
                    dtEMISchedule = ds.Tables[3];
                    dtLoanAgr = ds.Tables[4];
                    dtAuthLetter = ds.Tables[5];
                    dtKotak = ds.Tables[6];


                    oUsr = new CDigiDoc();
                    dsDigiDoc = oUsr.getDigiDocDtlsByDocId(0, "", "Y");
                    dtDigiDocDtls = dsDigiDoc.Tables[0];
                    string vLoanAppId = "", vCustId = "";
                    string vBrCode = (string)Session[gblValue.BrnchCode];
                    if (dtAppFrm1.Rows.Count > 0)
                    {
                        vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                        vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                    }
                    ds = oRpt.rptCAMReport(vCustId, vLoanAppId);
                    oMem = new CMember();
                    dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(vSancId);
                    oMem = new CMember();
                    dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(vSancId);

                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        dt3 = ds.Tables[2];
                        dt4 = ds.Tables[3];
                        dt5 = ds.Tables[4];
                        dt6 = ds.Tables[5];
                        dt7 = ds.Tables[6];
                        dt8 = ds.Tables[7];
                        dt9 = ds.Tables[8];
                        dt10 = ds.Tables[9];
                        dt11 = ds.Tables[10];
                        dt12 = ds.Tables[11];
                        dt13 = ds.Tables[12];
                        dt14 = ds.Tables[13];
                        dt15 = ds.Tables[14];
                        dt16 = ds.Tables[15];
                        dt17 = ds.Tables[16];
                        dt18 = ds.Tables[17];
                    }

                    oUsr = new CDigiDoc();
                    dsDigiDoc = oUsr.getDigiDocDtlsByDocId(0, "", "Y");
                    dtDigiDocDtls = dsDigiDoc.Tables[0];

                    if (dtAppFrm1.Rows.Count > 0)
                    {
                        #region GetBase64Images
                        dtAppFrm1.Rows[0]["SelfieImg"] = GetByteImage("Applicant photo.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["CoAppSelfieImg"] = GetByteImage("Co-applicant photo.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["KYC1FrontImg"] = GetByteImage("Applicant KYC ID 1 Front.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["KYC1BackImg"] = GetByteImage("Applicant KYC ID 1 Back.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["KYC2FrontImg"] = GetByteImage("Applicant KYC ID 2 Front.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["KYC2BackImg"] = GetByteImage("Applicant KYC ID 2 Back.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["CoAppKYC1FrontImg"] = GetByteImage("Co-applicant KYC ID 1 Front.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["CoAppKYC1BackImg"] = GetByteImage("Co-applicant KYC ID 1 Back.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["CoAppKYC2FrontImg"] = GetByteImage("Co-applicant KYC ID 2 Front.png", vLoanAppId, "I");
                        dtAppFrm1.Rows[0]["CoAppKYC2BackImg"] = GetByteImage("Co-applicant KYC ID 2 Back.png", vLoanAppId, "I");
                        dtAppFrm1.AcceptChanges();

                        dtDigiDocDtls.Rows[0]["PhotoImg"] = GetByteImage(vLoanAppId + ".jpg", vLoanAppId, "D");
                        dtDigiDocDtls.AcceptChanges();
                        #endregion

                        if (vRptPath != string.Empty)
                        {
                            using (ReportDocument rptDoc = new ReportDocument())
                            {
                                vRptName = "DigitalDocument";
                                rptDoc.Load(vRptPath);
                                rptDoc.SetDataSource(dtAppFrm1);
                                rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                                rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                                rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                                rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                                rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                                rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);

                                rptDoc.Subreports["CAM1.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["CAM2.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["App.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["CoApp.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["AppResidence.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["rptObligation.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["rptObligationCoAppl.rpt"].SetDataSource(dt17);
                                rptDoc.Subreports["CreditProfile.rpt"].SetDataSource(dt18);
                                rptDoc.Subreports["BankinDetailAppCoApp.rpt"].SetDataSource(dt8);
                                rptDoc.Subreports["rptFOIRIncomeAssesment.rpt"].SetDataSource(dt10);
                                rptDoc.Subreports["CAM6.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["DecisionSheet.rpt"].SetDataSource(dt16);
                                rptDoc.Subreports["LnSancApprove.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);

                                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                                rptDoc.SetParameterValue("pBranch", vBrCode);
                                rptDoc.SetParameterValue("pTitle", "CAM Report");
                                if (dtScheduleOfCharges.Rows.Count > 0)
                                {
                                    rptDoc.SetParameterValue("pBorrower", dtScheduleOfCharges.Rows[0]["Borrower"].ToString());
                                    rptDoc.SetParameterValue("pCoBorrower", dtScheduleOfCharges.Rows[0]["CoBorrower"].ToString());
                                    rptDoc.SetParameterValue("pSanctionDate", dtScheduleOfCharges.Rows[0]["SanctionDate"].ToString());
                                }
                                else
                                {
                                    rptDoc.SetParameterValue("pBorrower", "");
                                    rptDoc.SetParameterValue("pCoBorrower", "");
                                    rptDoc.SetParameterValue("pSanctionDate", "01/01/1900");
                                }

                                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Digital_Document");
                                rptDoc.Dispose();
                                Response.ClearContent();
                                Response.ClearHeaders();
                            }
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                oMem = null;
                oUsr = null;
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;
            }
        }
        #endregion

        #region "Send / Resend OTP"
        protected void btnSendResendOTP_Click(object sender, EventArgs e)
        {
            CMember oMem = null;
            CDigiDoc oUsr = null;
            DataTable dtMob = null;
            string vMessageBody = "", vSuccessStat = "", vSuccessStatCoApp = "", vMessageBodyCoApp = "";
            hdnOTP.Value = String.Empty;
            hdnSMSVerifyTimeStamp.Value = String.Empty;
            Int64 _vDigiDocDtlsID = 0;
            try
            {
                dtMob = new DataTable();
                oUsr = new CDigiDoc();
                oMem = new CMember();
                string vSancId = oUsr.GetSanctionId(hdnLoanAppNo.Value);
                dtMob = oMem.GetDisbSMSMemMob(vSancId, gblFuction.setDate("01-01-1900"));

                btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                btnSendResendOTP.Attributes.Add("style", "visibility:Visible;");
                txtOTP.Attributes.Add("style", "visibility:Visible;");
                if (hdnLoanAppNo.Value.Trim().Substring(0, 4) != "0000")
                {
                    txtCOTP.Attributes.Add("style", "visibility:Visible;");
                }
                spnOTPExpire.Attributes.Add("style", "visibility:Visible;");
                btnCapture.Attributes.Add("style", "visibility:Visible;");
                btnUpload.Attributes.Add("style", "visibility:Visible;");
                btnResetCam.Attributes.Add("style", "visibility:Visible;");
                spnlocation.Attributes.Add("style", "visibility:Visible;");

                if (dtMob.Rows.Count > 0)
                {
                    _vMobileNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                    _vCoAppMobNo = Convert.ToString(dtMob.Rows[0]["CMobNo"]).Trim();

                    Random ran = new Random();
                    String b = "0123456789";
                    int length = 6;
                    String vRandomToken = "";
                    for (int i = 0; i < length; i++)
                    {
                        int a = ran.Next(b.Length);
                        vRandomToken = vRandomToken + b.ElementAt(a);
                    }
                    hdnOTP.Value = vRandomToken.ToString().Trim();
                    //vMessageBody = "Your One Time Password: " + vRandomToken.ToString().Trim();
                    vMessageBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vRandomToken.ToString().Trim() + ".";
                    string vRe = SendSMS(_vMobileNo, vMessageBody);
                    string[] arr = vRe.Split('|');
                    vSuccessStat = arr[0];
                    // spnOTPExpire.InnerHtml = vMessageBody;
                    //  gblFuction.AjxMsgPopup(vMessageBody);
                    //----------------------------------------------Co-Applicant OTP----------------------------------------------------
                    if (hdnLoanAppNo.Value.Trim().Substring(0, 4) != "0000")
                    {
                        Random ran1 = new Random();
                        String vnum = "0123456789";
                        String vRandomOTP = "";
                        for (int i = 0; i < 6; i++)
                        {
                            int c = ran1.Next(vnum.Length);
                            vRandomOTP = vRandomOTP + vnum.ElementAt(c);
                        }
                        hdnCOTP.Value = vRandomOTP.ToString().Trim();
                        vMessageBodyCoApp = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vRandomOTP.ToString().Trim() + ".";
                        string vRe1 = SendSMS(_vCoAppMobNo, vMessageBodyCoApp);
                        string[] arr1 = vRe1.Split('|');
                        vSuccessStatCoApp = arr1[0];
                        //  gblFuction.AjxMsgPopup(vMessageBodyCoApp);
                    }

                    //------------------------------------------------------------------------------------------------------------------
                    spnlocation.InnerHtml = hdnLatLong.Value;

                    if (vSuccessStat.ToString().Trim().ToLower() == "success")
                    {
                        oUsr = new CDigiDoc();
                        Int32 _vRet = oUsr.SaveDigiDocOTPDetails(hdnLoanAppNo.Value.Trim(), Convert.ToInt64(hdnDigiDocId.Value),
                            hdnOTP.Value + (hdnCOTP.Value.Length > 0 ? "/" + hdnCOTP.Value : ""), ref _vDigiDocDtlsID);
                        //Int32 _vRet = oUsr.SaveDigiDocOTPDetails(hdnLoanAppNo.Value.Trim(), Convert.ToInt64(hdnDigiDocId.Value),
                        //    hdnOTP.Value, ref _vDigiDocDtlsID);
                        if (_vRet > 0)
                        {
                            hdnDigiDocDtlsId.Value = _vDigiDocDtlsID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                oMem = null;
                oUsr = null;
            }
        }
        //-------------------------Send SMS------------------------------------
        public string SendSMS(string pMobileNo, string pMsgBody)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)
                string vMsgBody = string.Empty;
                //vMsgBody = pMsgBody;
                vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-15"));
                //********************************************************************
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000204129";
                String passwd = "Unity@1122";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                // String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=CENOTP&v=1.1&format=text";
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472061562826&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                reader = new System.IO.StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }
        #endregion

        #region "Final Submit"
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            String _vIPAddress = "", _vBrowser = "", _vGeoLocation = "", _vPlatform = "", _vCustomerName = "", _vMobNo = "", _vDeviceDetails = "";
            hdnSMSVerifyTimeStamp.Value = "";
            CDigiDoc ocr = null;
            Int32 _result = 0;

            if (hdnOTP.Value.ToString().Trim() == "")
            {
                gblFuction.AjxMsgPopup("Click on Send/Resend OTP..");
                return;
            }
            if (txtOTP.Text.ToString().Trim() == "")
            {
                gblFuction.AjxMsgPopup("Type Applicant OTP..");
                return;
            }
            if (hdnOTP.Value.ToString().Trim() != txtOTP.Text.ToString().Trim())
            {
                gblFuction.AjxMsgPopup("Applicant OTP not Matched..");
                return;
            }
            if (hdnLoanAppNo.Value.Substring(0, 4) != "0000")
            {
                if (hdnCOTP.Value.ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Click on Send/Resend OTP..");
                    return;
                }
                if (txtCOTP.Text.ToString().Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Type CO-Applicant OTP..");
                    return;
                }
                if (hdnCOTP.Value.ToString().Trim() != txtCOTP.Text.ToString().Trim())
                {
                    gblFuction.AjxMsgPopup("CO-Applicant OTP not Matched..");
                    return;
                }
            }

            if (chkOTPTimeStamp(hdnDigiDocDtlsId.Value, hdnOTP.Value + (hdnCOTP.Value.Length > 0 ? "/" + hdnCOTP.Value : "")) == false)
            //if (chkOTPTimeStamp(hdnDigiDocDtlsId.Value, hdnOTP.Value) == false)
            {
                gblFuction.AjxMsgPopup("OTP Expired..");
                return;
            }

            try
            {
                if (SaveCapturedImage())
                {
                    ocr = new CDigiDoc();
                    _vCustomerName = hdnCustomerName.Value.Trim();
                    _vPlatform = hdnPlatform.Value.Trim();
                    _vBrowser = GetBrowserDetails();
                    _vGeoLocation = hdnLatLong.Value;
                    _vIPAddress = GetIPAddress();
                    _vMobNo = _vMobileNo;
                    _vDeviceDetails = hdnUserAgent.Value.Trim();

                    _result = ocr.UpdateDigiDocOTPDetails(Convert.ToInt64(hdnDigiDocDtlsId.Value), _vCustomerName, _vPlatform, _vBrowser, _vGeoLocation, _vIPAddress, _vMobNo, _vDeviceDetails);
                    if (_result > 0)
                    {
                        btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                        btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                        txtOTP.Attributes.Add("style", "visibility:hidden;");
                        txtCOTP.Attributes.Add("style", "visibility:hidden;");
                        spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                        btnUpload.Attributes.Add("style", "visibility:hidden;");
                        btnResetCam.Attributes.Add("style", "visibility:hidden;");
                        btnSubmit.Attributes.Add("style", "visibility:hidden;");
                        btnCapture.Attributes.Add("style", "visibility:hidden;");
                        spnlocation.Attributes.Add("style", "visibility:hidden;");

                        #region Craete Report Doc
                        GetReportDocForDigitalSign(hdnLoanAppNo.Value, _vCustomerName, Convert.ToInt64(hdnDigiDocDtlsId.Value));
                        #endregion

                        gblFuction.AjxMsgPopup("Document verified successfully..");
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/javascript'>window.close();</script>");
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No image uploaded..");
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                ocr = null;
            }
        }
        #endregion

        #region "Checking OTP Time Stamp"
        private Boolean chkOTPTimeStamp(String vDigiDocDtlsId, String vOTP)
        {
            Boolean _Valid = true;
            CDigiDoc oUsr = null;
            DataTable dtRecord = null;
            DataTable dtValid = null;
            DataSet ds = null;
            try
            {
                oUsr = new CDigiDoc();
                ds = oUsr.getDigiDocDtlsByDocId(Convert.ToInt64(vDigiDocDtlsId.ToString().Trim()), vOTP.ToString().Trim(), "N");
                dtRecord = ds.Tables[0];
                dtValid = ds.Tables[1];
                if (dtRecord.Rows.Count > 0)
                {
                    if (dtValid.Rows.Count > 0)
                    {
                        _Valid = (dtValid.Rows[0]["ValidOrNot"].ToString() == "Y") ? true : false; // true; //
                    }
                    hdnSMSVerifyTimeStamp.Value = (_Valid == true) ? dtRecord.Rows[0]["SMSVerifyTimeStamp"].ToString() : "";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                oUsr = null;
            }
            return _Valid;
        }
        #endregion

        #region "Get IP Address And Browser"
        private string GetIPAddress()
        {
            //IP Address
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }
        public string GetBrowserDetails()
        {
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            browserDetails = browser.Browser;
            return browserDetails;
        }
        #endregion

        #region Save Image
        public bool SaveCapturedImage()
        {
            string data = "";
            Boolean _Valid = false;
            try
            {
                data = hdnCapturedImage.Value;
                vpathDigiDocImage = ConfigurationManager.AppSettings["pathDigiDocImage"]; // ConfigurationManager.AppSettings["DocDownloadPath"]
                if (data != "" && data != "-1")
                {
                    string fileName = hdnLoanAppNo.Value.Trim().ToString();
                    //Convert Base64 Encoded string to Byte Array.
                    byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
                    //Save the Byte Array as Image File.
                    if (MinioYN == "Y")
                    {
                        CApiCalling oAC = new CApiCalling();
                        oAC.UploadFileMinio(imageBytes, fileName + ".jpg", "", DigiDocOtpBucket, MinioUrl);
                    }
                    else
                    {
                        string filePath = string.Format(vpathDigiDocImage + "\\{0}.jpg", fileName);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        File.WriteAllBytes(filePath, imageBytes);
                    }
                    _Valid = true;
                }
                else
                {
                    _Valid = false;
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            return _Valid;
        }
        #endregion

        #region Craete Report Doc

        private string GetReportDocForDigitalSign(string pLoanAppNo, string pMemberName, Int64 vDigiDocDtlsId)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;

            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null
            , dt8 = null, dt9 = null, dt10 = null, dt11 = null, dt12 = null, dt13 = null, dt14 = null, dt15 = null,
            dt16 = null, dt17 = null, dt18 = null, dtScheduleOfCharges = null, dtSchedule = null;


            CReports oRpt = new CReports();
            string vRptPath = "", vFileName = "No File Created";
            CDigiDoc oUsr = null;
            CMember oMem = null;

            ReportDocument rptDoc = new ReportDocument();
            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();
                string vSanctionId = oUsr.GetSanctionId(hdnLoanAppNo.Value);

                string vSancId = vSanctionId;
                vRptPath = string.Empty;
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigitalDocs.rpt"));
                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oMem.GetAgrDigitalDocs(vSancId, vDigiDocDtlsId);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];

                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];

                string vLoanAppId = "", vCustId = "";
                string vBrCode = (string)Session[gblValue.BrnchCode];
                if (dtAppFrm1.Rows.Count > 0)
                {
                    vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                    vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                }
                ds = oRpt.rptCAMReport(vCustId, vLoanAppId);
                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(vSancId, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(vSancId, vDigiDocDtlsId);

                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];
                    dt5 = ds.Tables[4];
                    dt6 = ds.Tables[5];
                    dt7 = ds.Tables[6];
                    dt8 = ds.Tables[7];
                    dt9 = ds.Tables[8];
                    dt10 = ds.Tables[9];
                    dt11 = ds.Tables[10];
                    dt12 = ds.Tables[11];
                    dt13 = ds.Tables[12];
                    dt14 = ds.Tables[13];
                    dt15 = ds.Tables[14];
                    dt16 = ds.Tables[15];
                    dt17 = ds.Tables[16];
                    dt18 = ds.Tables[17];
                }

                if (dtAppFrm1.Rows.Count > 0)
                {
                    #region GetBase64Images
                    dtAppFrm1.Rows[0]["SelfieImg"] = GetByteImage("Applicant photo.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppSelfieImg"] = GetByteImage("Co-applicant photo.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC1FrontImg"] = GetByteImage("Applicant KYC ID 1 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC1BackImg"] = GetByteImage("Applicant KYC ID 1 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC2FrontImg"] = GetByteImage("Applicant KYC ID 2 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["KYC2BackImg"] = GetByteImage("Applicant KYC ID 2 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC1FrontImg"] = GetByteImage("Co-applicant KYC ID 1 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC1BackImg"] = GetByteImage("Co-applicant KYC ID 1 Back.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC2FrontImg"] = GetByteImage("Co-applicant KYC ID 2 Front.png", vLoanAppId, "I");
                    dtAppFrm1.Rows[0]["CoAppKYC2BackImg"] = GetByteImage("Co-applicant KYC ID 2 Back.png", vLoanAppId, "I");
                    dtAppFrm1.AcceptChanges();

                    dtDigiDocDtls.Rows[0]["PhotoImg"] = GetByteImage(vLoanAppId + ".jpg", vLoanAppId, "D");
                    dtDigiDocDtls.AcceptChanges();
                    #endregion
                    if (vRptPath != string.Empty)
                    { 
                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];                       
                        vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";
                        using (rptDoc)
                        {
                            //vRptName = "DigitalDocument";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dtAppFrm1);
                            rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                            rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                            rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);

                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }

                            rptDoc.Subreports["CAM1.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["CAM2.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["App.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["CoApp.rpt"].SetDataSource(dt2);
                            rptDoc.Subreports["AppResidence.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["rptObligation.rpt"].SetDataSource(dt4);
                            rptDoc.Subreports["rptObligationCoAppl.rpt"].SetDataSource(dt17);
                            rptDoc.Subreports["CreditProfile.rpt"].SetDataSource(dt18);
                            rptDoc.Subreports["BankinDetailAppCoApp.rpt"].SetDataSource(dt8);
                            rptDoc.Subreports["rptFOIRIncomeAssesment.rpt"].SetDataSource(dt10);
                            rptDoc.Subreports["CAM6.rpt"].SetDataSource(dt1);
                            rptDoc.Subreports["DecisionSheet.rpt"].SetDataSource(dt16);
                            rptDoc.Subreports["LnSancApprove.rpt"].SetDataSource(dt3);
                            rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);

                            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                            rptDoc.SetParameterValue("pBranch", vBrCode);
                            rptDoc.SetParameterValue("pTitle", "CAM Report");
                            if (dtScheduleOfCharges.Rows.Count > 0)
                            {
                                rptDoc.SetParameterValue("pBorrower", dtScheduleOfCharges.Rows[0]["Borrower"].ToString());
                                rptDoc.SetParameterValue("pCoBorrower", dtScheduleOfCharges.Rows[0]["CoBorrower"].ToString());
                                rptDoc.SetParameterValue("pSanctionDate", dtScheduleOfCharges.Rows[0]["SanctionDate"].ToString());
                            }
                            else
                            {
                                rptDoc.SetParameterValue("pBorrower", "");
                                rptDoc.SetParameterValue("pCoBorrower", "");
                                rptDoc.SetParameterValue("pSanctionDate", "01/01/1900");
                            }
                            if (MinioYN == "Y")
                            {
                                CApiCalling oAC = new CApiCalling();
                                Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                                oAC.UploadFileMinio(strmToByte(reportStream), pLoanAppNo + ".pdf", pLoanAppNo, DigiDocBucket, MinioUrl);
                            }
                            else
                            {
                                rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                            }
                        }
                    }
                }
                else
                {
                    vFileName = "No File Created..As data Not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rptDoc.Dispose();
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;

                oUsr = null; oMem = null;
                dsDigiDoc = null;
            }
            return vFileName;
        }

        #endregion

        #region streamtoByte
        public static byte[] strmToByte(Stream vStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                vStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion        

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

        #region GetByteImage
        public byte[] GetByteImage(string ImageName, string Id, string Module)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? DocumentURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? Id + "_" + ImageName : ImageName);
                    ActNetImage1 = ActNetPath[j] + (Module == "I" ? Id + "/" + ImageName : ImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                    else if (URLExist(ActNetImage1))
                    {
                        imgByte = DownloadByteImage(ActNetImage1);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }
        #endregion

        #region URLExist
        private bool URLExist(string pPath)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pPath);
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
        #endregion

    }
}