using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using CENTRUMBA;
using CENTRUMCA;
using System.Web.Hosting;


namespace CENTRUM_SARALVYAPAR
{
    public partial class CoAppDigitalDoc : CENTRUMBAse
    {
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        string _vMobileNo = "", vpathDigiDocImage = "", vpathCreateNewDoc = "";

        string PathMinio = ConfigurationManager.AppSettings["PathMinio"];
        string PathOTP = ConfigurationManager.AppSettings["PathOTP"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string AadhaarURL = ConfigurationManager.AppSettings["AadhaarURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];
        string DigiDocOtpBucket = ConfigurationManager.AppSettings["DigiDocOtpBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //GetReportDocForDigitalSign("11300000962", 0);               
                // btnSubmit_Click(this, EventArgs.Empty);
                ViewState["GroupId"] = null;
                ViewState["Language"] = null;
                ViewState["DateFrom"] = null;
                ViewState["DateTo"] = null;
                ViewState["MobNo"] = null;
                ViewState["SignOrg"] = null;
                hdnOTP.Value = String.Empty;
                String encURL = "";
                String _vDocStatus = "N";

                CDigiDoc oUsr = null;
                Int32 _vInActiveToken;
                try
                {
                    oUsr = new CDigiDoc();
                    encURL = Request.RawUrl;
                    encURL = encURL.Substring(encURL.IndexOf('?') + 1);
                    if (!encURL.Equals(""))
                    {
                        if (encURL.Contains("&"))
                        {
                            String[] queryStrParam1 = encURL.Split('&');
                            String[] queryStr1 = queryStrParam1[0].Split('=');
                            String[] queryStr2 = queryStrParam1[1].Split('=');
                            String[] queryStr3 = queryStrParam1[2].Split('=');
                            String[] queryStr4 = queryStrParam1[3].Split('=');
                            String[] queryStr5 = queryStrParam1[4].Split('=');
                            String[] queryStr6 = queryStrParam1[5].Split('=');
                            String[] queryStr7 = queryStrParam1[6].Split('=');

                            if (queryStr1.Length > 1)
                                hdnLoanAppNo.Value = queryStr1[1].ToString();
                            if (queryStr2.Length > 1)
                                hdnToken.Value = queryStr2[1].ToString();
                            if (queryStr3.Length > 1)
                                ViewState["GroupId"] = queryStr3[1].ToString();
                            if (queryStr4.Length > 1)
                                ViewState["Language"] = queryStr4[1].ToString();
                            if (queryStr5.Length > 1)
                                ViewState["DateFrom"] = queryStr5[1].ToString();
                            if (queryStr6.Length > 1)
                                ViewState["DateTo"] = queryStr6[1].ToString();
                            if (queryStr7.Length > 1)
                                ViewState["SignOrg"] = queryStr7[1].ToString();

                            // Check already Digitally Signed or not
                            _vDocStatus = oUsr.DigitalSignStatus(hdnLoanAppNo.Value.Trim());
                            if (_vDocStatus.Trim().ToLower() == "y")
                            {
                                btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:hidden;");
                                btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                txtOTP.Attributes.Add("style", "visibility:hidden;");
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
                                btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:Visible;");
                                btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                txtOTP.Attributes.Add("style", "visibility:hidden;");
                                spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                                btnCapture.Attributes.Add("style", "visibility:hidden;");
                                btnUpload.Attributes.Add("style", "visibility:hidden;");
                                btnResetCam.Attributes.Add("style", "visibility:hidden;");
                                btnSubmit.Attributes.Add("style", "visibility:hidden;");
                            }
                            else
                            {
                                // ************  Inactive The Called Token if 15 Minute Expired ***********//
                                _vInActiveToken = oUsr.InActiveDigitalDoc(hdnLoanAppNo.Value, hdnToken.Value);
                                if (_vInActiveToken > 0)
                                {
                                    btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:hidden;");
                                    btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                    txtOTP.Attributes.Add("style", "visibility:hidden;");
                                    spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                                    btnCapture.Attributes.Add("style", "visibility:hidden;");
                                    btnUpload.Attributes.Add("style", "visibility:hidden;");
                                    btnResetCam.Attributes.Add("style", "visibility:hidden;");
                                    btnSubmit.Attributes.Add("style", "visibility:hidden;");
                                    gblFuction.AjxMsgPopup("Token Expired..");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:hidden;");
                            btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                            txtOTP.Attributes.Add("style", "visibility:hidden;");
                            spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                            btnCapture.Attributes.Add("style", "visibility:hidden;");
                            btnUpload.Attributes.Add("style", "visibility:hidden;");
                            btnResetCam.Attributes.Add("style", "visibility:hidden;");
                            btnSubmit.Attributes.Add("style", "visibility:hidden;");
                            gblFuction.AjxMsgPopup("Page not contain parameter");
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
                    oUsr = null;
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
                ds = oUsr.getDigitalDocByToken(vLoanAppNo, vToken, "C");
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

        #region "Send / Resend OTP"
        protected void btnSendResendOTP_Click(object sender, EventArgs e)
        {
            CMember oMem = null;
            CDigiDoc oUsr = null;
            DataTable dtMob = null;
            string vMessageBody = "", vSuccessStat = "";
            hdnOTP.Value = String.Empty;
            hdnSMSVerifyTimeStamp.Value = String.Empty;
            Int64 _vDigiDocDtlsID = 0;

            try
            {
                dtMob = new DataTable();
                oUsr = new CDigiDoc();
                oMem = new CMember();
                string vLoanAppId = hdnLoanAppNo.Value;
                DateTime DisbDt = DateTime.Now;
                dtMob = oMem.GetDisbSMSMemMob(vLoanAppId, DisbDt);
                btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:hidden;");
                btnSendResendOTP.Attributes.Add("style", "visibility:Visible;");
                txtOTP.Attributes.Add("style", "visibility:Visible;");
                spnOTPExpire.Attributes.Add("style", "visibility:Visible;");
                spnlocation.Attributes.Add("style", "visibility:Visible;");
                btnCapture.Attributes.Add("style", "visibility:Visible;");
                btnUpload.Attributes.Add("style", "visibility:Visible;");
                btnResetCam.Attributes.Add("style", "visibility:Visible;");

                if (dtMob.Rows.Count > 0)
                {
                    _vMobileNo = Convert.ToString(dtMob.Rows[0]["CoAppMob"]).Trim();
                    hndMobileNo.Value = Convert.ToString(dtMob.Rows[0]["CoAppMob"]).Trim();
                    ViewState["MobNo"] = Convert.ToString(dtMob.Rows[0]["CoAppMob"]).Trim();
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
                    vMessageBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vRandomToken.ToString().Trim() + ".";
                    string vRe = SendSMS(_vMobileNo, vMessageBody);
                    string[] arr = vRe.Split('|');
                    vSuccessStat = arr[0];//"success";  
                    spnlocation.InnerHtml = hdnLatLong.Value;
                    if (vSuccessStat.ToString().Trim().ToLower() == "success")
                    {
                        oUsr = new CDigiDoc();
                        Int32 _vRet = oUsr.SaveDigiDocOTPDetails(hdnLoanAppNo.Value.Trim(), Convert.ToInt64(hdnDigiDocId.Value), hdnOTP.Value, ref _vDigiDocDtlsID);
                        if (_vRet > 0)
                        {
                            hdnDigiDocDtlsId.Value = _vDigiDocDtlsID.ToString();
                        }
                        btnSubmit.Attributes.Add("style", "visibility:Visible;");
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
                vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //********************************************************************
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000243134";
                String passwd = "ZFimpPeKx";
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472061562826&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
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
            Int32 _result = 0;
            string vLoanAppId = hdnLoanAppNo.Value;
            string vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
            CDigiDoc oDd = new CDigiDoc();
            string vCoAppMobileNo = "";
            string vCoAppName = oDd.GetCoAppName(vLoanAppId, ref vCoAppMobileNo);
            hdnSMSVerifyTimeStamp.Value = "";
            if (hdnOTP.Value.ToString().Trim() == "")
            {
                gblFuction.AjxMsgPopup("Click on Send/Resend OTP..");
                return;
            }
            if (txtOTP.Text.ToString().Trim() == "")
            {
                gblFuction.AjxMsgPopup("Type OTP..");
                return;
            }
            if (hdnOTP.Value.ToString().Trim() != txtOTP.Text.ToString().Trim())
            {
                gblFuction.AjxMsgPopup("OTP not Matched..");
                return;
            }
            if (chkOTPTimeStamp(hdnDigiDocDtlsId.Value, hdnOTP.Value) == false)
            {
                gblFuction.AjxMsgPopup("OTP Expired..");
                return;
            }
            try
            {
                if (SaveCapturedImage())
                {
                    oDd = new CDigiDoc();
                    _vCustomerName = hdnCustomerName.Value.Trim();
                    _vPlatform = hdnPlatform.Value.Trim();
                    _vBrowser = GetBrowserDetails();
                    _vGeoLocation = hdnLatLong.Value;
                    _vIPAddress = GetIPAddress();
                    _vMobNo = Convert.ToString(ViewState["MobNo"]);
                    _vDeviceDetails = hdnUserAgent.Value.Trim();
                    _result = oDd.UpdateDigiDocOTPDetails(Convert.ToInt64(hdnDigiDocDtlsId.Value), _vCustomerName, _vPlatform, _vBrowser, _vGeoLocation, _vIPAddress, _vMobNo, _vDeviceDetails);
                    if (_result > 0)
                    {
                        btnLinkWithOutDownDoc.Attributes.Add("style", "visibility:hidden;");
                        btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                        txtOTP.Attributes.Add("style", "visibility:hidden;");
                        spnOTPExpire.Attributes.Add("style", "visibility:hidden;");
                        btnSubmit.Attributes.Add("style", "visibility:hidden;");
                        btnUpload.Attributes.Add("style", "visibility:hidden;");
                        btnResetCam.Attributes.Add("style", "visibility:hidden;");
                        btnCapture.Attributes.Add("style", "visibility:hidden;");
                        spnlocation.Attributes.Add("style", "visibility:hidden;");

                        #region Create Report Doc
                        GetReportDocForDigitalSign(hdnLoanAppNo.Value, Convert.ToInt64(hdnDigiDocDtlsId.Value));
                        // Path to the existing PDF and the output PDF file
                        string inputPdfPath = vpathCreateNewDoc + vLoanAppId + "_CoAppSign.pdf";
                        DigitalSign(inputPdfPath, vLoanAppId, vCoAppName, vCoAppMobileNo);
                        File.Delete(inputPdfPath);
                        oDd = new CDigiDoc();
                        oDd.UpdateCoAppDigitalSign(vLoanAppId, Convert.ToString(ViewState["SignOrg"]));
                        #endregion
                        gblFuction.AjxMsgPopup("Document verified successfully..");
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "CloseWindowScript", "window.close();", true);
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
                gblFuction.AjxMsgPopup(ex.Message.ToString());
            }
            finally
            {

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

        #region Save Doc Image
        public bool SaveCapturedImage()
        {
            string data = "";
            Boolean _Valid = false;
            try
            {
                data = hdnCapturedImage.Value;
                vpathDigiDocImage = ConfigurationManager.AppSettings["pathDigiDocImage"];
                if (data != "" && data != "-1")
                {
                    string fileName = hdnLoanAppNo.Value.Trim().ToString();
                    byte[] imageBytes = Convert.FromBase64String(data.Split(',')[1]);
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

        #region URLExist
        private bool URLExist(string pPath)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            WebRequest wR = WebRequest.Create(pPath);
            wR.Timeout = 3000;
            try
            {
                using (WebResponse webResponse = wR.GetResponse())
                {
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Create Report Doc
        private string GetReportDocForDigitalSign(string pLoanAppNo, Int64 vDigiDocDtlsId)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;
            DataTable dtScheduleOfCharges = null, dtSchedule = null, dtScheduleOwn = null;

            CReports oRpt = new CReports();
            string vRptPath = "", vFileName = "No File Created", vLanguage = "";
            CDigiDoc oUsr = null;
            CMember oMem = null;
            // ReportDocument rptDoc = new ReportDocument();
            CApiCalling oAC = new CApiCalling();
            vLanguage = Convert.ToString(ViewState["Language"]);
            try
            {
                oUsr = new CDigiDoc();
                oMem = new CMember();
                vRptPath = string.Empty;
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/" + (vLanguage == "H" ? "DigitalDoc_Hindi.rpt" : "DigitalDocs.rpt")));
                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = oRpt.GetDigitalDocs(pLoanAppNo, 0, 1);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];

                string vEnqId = dtAppFrm1.Rows[0]["EnquiryId"].ToString();
                string vBranchAddress = dtAppFrm1.Rows[0]["BranchAddress"].ToString();
                string vBrCode = dtAppFrm1.Rows[0]["BranchCode"].ToString();
                string vBranch = dtAppFrm1.Rows[0]["BranchName"].ToString();

                dtAppFrm1.Rows[0]["MemImg"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemId1FImg"] = GetByteImage("IDProofImage.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemId1BImg"] = GetByteImage("IDProofImageBack.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemAdd1FImg"] = GetByteImage("AddressProofImage.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["MemAdd1BImg"] = GetByteImage("AddressProofImageBack.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["CoMemId1FImg"] = GetByteImage("AddressProofImage2.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["CoMemId1BImg"] = GetByteImage("AddressProofImage2Back.png", vEnqId, "I");
                dtAppFrm1.Rows[0]["SignPageYN"] = "Y";
                dtAppFrm1.Rows[0]["Language"] = vLanguage;
                dtAppFrm1.AcceptChanges();

                oUsr = new CDigiDoc();
                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];

                string vLoanAppId = "", vCustId = "";
                if (dtAppFrm1.Rows.Count > 0)
                {
                    vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                    vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                }
                dtDigiDocDtls.Rows[0]["ImgPhoto"] = GetByteImage(vLoanAppId + ".jpg", vLoanAppId, "O");
                dtDigiDocDtls.AcceptChanges();
                oMem = new CMember();
                dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(pLoanAppNo, vDigiDocDtlsId);
                oMem = new CMember();
                dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(pLoanAppNo, vDigiDocDtlsId);
                oRpt = new CReports();
                dtScheduleOwn = oRpt.rptRepaySchedule(pLoanAppNo, vBrCode, "N", 0);

                if (dtAppFrm1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
                        vFileName = vpathCreateNewDoc + pLoanAppNo + "_CoAppSign.pdf";

                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            //vRptName = "DigitalDocument";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dtAppFrm1);
                            rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                            rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            if (vLanguage != "H")
                            {
                                rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                                rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);
                                rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);
                            }
                            if (vLanguage == "H")
                            {
                                rptDoc.Subreports["RepaySche_Hindi.rpt"].SetDataSource(dtScheduleOwn);
                                rptDoc.Subreports["RepaymentSche.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "G")
                            {
                                rptDoc.Subreports["RepaySche_Gujrati.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "B")
                            {
                                rptDoc.Subreports["RepaySche_Beng.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "O")
                            {
                                rptDoc.Subreports["RepaySche_Odia.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "T")
                            {
                                rptDoc.Subreports["RepaySche_Tamil.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "L")
                            {
                                rptDoc.Subreports["RepaySche_Telegu.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else if (vLanguage == "M")
                            {
                                rptDoc.Subreports["RepaySche_Malayalam.rpt"].SetDataSource(dtScheduleOwn);
                            }
                            else
                            {
                                rptDoc.Subreports["RepaySche_Kannad.rpt"].SetDataSource(dtScheduleOwn);
                            }

                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }
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
                            //if (MinioYN == "Y")
                            //{
                            //    Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                            //    oAC.UploadFileMinio(strmToByte(reportStream), pLoanAppNo + ".pdf", pLoanAppNo, DigiDocBucket, MinioUrl);
                            //}
                            //else
                            //{
                            rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                            //}

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
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;

                oUsr = null; oMem = null;
                dsDigiDoc = null;
            }
            return vFileName;
        }

        public void DigitalSign(string pFilePath, string pFileName, string pName, string pMobileNo)
        {
            string vSignOrg = Convert.ToString(ViewState["SignOrg"]);
            string base_folder_path = Path.GetDirectoryName(pFilePath);
            string outputPdfPath = base_folder_path + "\\" + pFileName + ".pdf"; // Path to save the new PDF         
            PdfReader pdfReader = new PdfReader(pFilePath);
            {
                using (FileStream fs = new FileStream(outputPdfPath, FileMode.Create))
                {
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, fs);
                    {
                        int pageCount = pdfReader.NumberOfPages;
                        for (int pageNumber = 1; pageNumber <= pageCount; pageNumber++)
                        {
                            PdfContentByte canvas = pdfStamper.GetOverContent(pageNumber);
                            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL);
                            float a = vSignOrg == "D" ? 60 : 430, height = 60, b = vSignOrg == "D" ? 40 : 7, width = 200;
                            //float a = 60, height = 60, b = 40, width = 200;//Protean
                            //float a = 430, height = 60, b = 7, width = 200;//IDFY
                            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
                            canvas.BeginText();
                            canvas.SetFontAndSize(baseFont, 8);
                            ColumnText ct = new ColumnText(canvas);
                            ct.SetSimpleColumn(a, b, a + width, b + height); // Add multiline text 
                            Paragraph p = new Paragraph();
                            p.SetLeading(0, (float)1.2);
                            p.Add(new Chunk("Digitally Signed by:\n", font));
                            p.Add(new Chunk("Name:" + pName + "\n", font));
                            p.Add(new Chunk("Reason:SARAL" + "\n", font));
                            p.Add(new Chunk("Date:" + indianTime.ToString("R") + " +5:30" + "\n", font)); // Add the paragraph to the column text 
                            p.Add(new Chunk("Mobile No:" + pMobileNo, font));
                            ct.AddElement(p); // Render the text 
                            ct.Go();
                            canvas.EndText();
                        }
                        pdfStamper.Close();
                    }
                }
                pdfReader.Close();
            }
        }

        #endregion

        #region GetMultipartFormData
        private readonly Encoding encoding = Encoding.UTF8;

        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();
            return formData;
        }
        #endregion

        #region Minio Image Upload
        public string UploadFileMinio(byte[] image, string fileName, string enqId, string bucketName, string minioUrl)
        {
            string fullResponse = "", isImageSaved = "N";
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("image", Convert.ToBase64String(image));
            postParameters.Add("KycId", enqId);
            postParameters.Add("BucketName", bucketName);
            postParameters.Add("ImageName", fileName);
            // Create request and receive response
            //  string postURL = "https://ocr.bijliftt.com/KYCFileUploadBase64";
            string postURL = minioUrl;
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formDataforRequest = GetMultipartFormData(postParameters, formDataBoundary);
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();
                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formDataforRequest.Length;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formDataforRequest, 0, formDataforRequest.Length);
                    requestStream.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    fullResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            dynamic obj = JsonConvert.DeserializeObject(fullResponse);
            bool status = obj.status;
            if (status == true)
            {
                isImageSaved = "Y";
            }
            return isImageSaved;
        }

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

        #region Get Byte Image
        public byte[] GetByteImage(string pImageName, string pId, string Module)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? InitialApproachURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    ActNetImage1 = ActNetPath[j] + (Module == "I" ? pId + "/" + pImageName : pImageName);
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
            return CompressImage(imgByte);
        }

        public byte[] GetAadhaarByteImage(string pImageName, string pId, string Module)
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? AadhaarURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    ActNetImage1 = ActNetPath[j] + (Module == "I" ? pId + "/" + pImageName : pImageName);
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
            return CompressImage(imgByte);
        }
        #endregion

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        #region ResizeImage
        public byte[] CompressImage(byte[] imageBytes)
        {
            int height = 0, width = 0;
            byte[] img = null;
            if (imageBytes != null)
            {
                using (var inStream = new MemoryStream(imageBytes))
                {
                    using (var image = System.Drawing.Image.FromStream(inStream))
                    {
                        height = image.Height;
                        width = image.Width;
                    }
                }
                if (width > height)
                {
                    img = ResizeImage(imageBytes, 450, 300);
                }
                else
                {
                    img = ResizeImage(imageBytes, 300, 450);
                }
            }
            return img;
        }

        public byte[] ResizeImage(byte[] imageBytes, int width, int height)
        {
            using (var inStream = new MemoryStream(imageBytes))
            {
                using (var image = System.Drawing.Image.FromStream(inStream))
                {
                    var resizedImage = new Bitmap(width, height);
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        graphics.DrawImage(image, 0, 0, width, height);
                    }
                    using (var outStream = new MemoryStream())
                    {
                        resizedImage.Save(outStream, image.RawFormat);
                        return outStream.ToArray();
                    }
                }
            }
        }
        #endregion
    }
}