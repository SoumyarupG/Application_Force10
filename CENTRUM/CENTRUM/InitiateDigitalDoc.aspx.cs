using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Hosting;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;


namespace CENTRUM
{
    public partial class InitiateDigitalDoc : CENTRUMBase
    {
        string _vMobileNo = "";
        string vpathDigiDocImage = "";
        string vpathCreateNewDoc = "";

        #region Path Set
        string path = ConfigurationManager.AppSettings["PathInitialApproach"];
        string IniPathHDrive = ConfigurationManager.AppSettings["IniPathHDrive"];
        string IniPathNetwork = ConfigurationManager.AppSettings["IniPathNetwork"];
        string IniPathNetwork2 = ConfigurationManager.AppSettings["IniPathNetwork2"];
        string IniPathNetwork3 = ConfigurationManager.AppSettings["IniPathNetwork3"];
        string IniPathGDrive = ConfigurationManager.AppSettings["IniPathGDrive"];
        string PathMinio = ConfigurationManager.AppSettings["PathMinio"];
        string PathOTP = ConfigurationManager.AppSettings["PathOTP"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string AadhaarURL = ConfigurationManager.AppSettings["AadhaarURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        string DigiDocOtpBucket = ConfigurationManager.AppSettings["DigiDocOtpBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {               
                ViewState["GroupId"] = null;
                ViewState["Language"] = null;
                ViewState["DateFrom"] = null;
                ViewState["DateTo"] = null;
                ViewState["MobNo"] = null;
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
                            String[] queryStr3 = queryStrParam1[2].Split('=');
                            String[] queryStr4 = queryStrParam1[3].Split('=');
                            String[] queryStr5 = queryStrParam1[4].Split('=');
                            String[] queryStr6 = queryStrParam1[5].Split('=');
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

                            // Check already Digitally Signed or not
                            _vDocStatus = oDD.DigitalSignStatus(hdnLoanAppNo.Value.Trim());
                            if (_vDocStatus.Trim().ToLower() == "y")
                            {
                                btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
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
                                btnLinkDownloadDocument.Attributes.Add("style", "visibility:Visible;");
                                btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                                txtOTP.Attributes.Add("style", "visibility:hidden;");
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
                            btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
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
                    oDD = null;
                }
            }
        }
        #endregion

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
            CReports oRpt = null;
            DataSet ds1 = new DataSet(), dsDigiDoc = new DataSet();
            DataTable dt1 = new DataTable(), dt2 = new DataTable(), dt3 = new DataTable();
            DataTable dt4 = new DataTable(), dt5 = new DataTable(), dt6 = new DataTable();
            DataTable dt7 = new DataTable(), dtDigiDocDtls = new DataTable();

            string vRptPath = "", vRptName = "", vImgPath = "", vLanguage = "E", vGroupId = "", vDtFrom = "", vDtTo = "";
            CDigiDoc oUsr = null;
            hdnOTP.Value = String.Empty;
            hdnSMSVerifyTimeStamp.Value = String.Empty;
            vLanguage = Convert.ToString(ViewState["Language"]);
            vGroupId = Convert.ToString(ViewState["GroupId"]);
            vDtFrom = Convert.ToString(ViewState["DateFrom"]);
            vDtTo = Convert.ToString(ViewState["DateTo"]);
            try
            {
                oUsr = new CDigiDoc();
                if (hdnLoanAppNo.Value == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Application No...");
                    return;
                }
                if (vGroupId == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Group..");
                    return;
                }
                if (vLanguage == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Language...");
                    return;
                }
                else
                {
                    vRptPath = string.Empty;
                    if (vLanguage == "E") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocForm.rpt";
                    if (vLanguage == "H") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormHindi.rpt";
                    if (vLanguage == "G") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormGujarati.rpt";
                    if (vLanguage == "O") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormOdia.rpt";
                    if (vLanguage == "B") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintBengali.rpt";
                    if (vLanguage == "K") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintKannad.rpt";
                    if (vLanguage == "T") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTamil.rpt";
                    if (vLanguage == "M") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintMalayalam.rpt";
                    if (vLanguage == "L") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTelugu.rpt";

                    oRpt = new CReports();
                    string Id = hdnLoanAppNo.Value;
                    ds1 = oRpt.GetDigitalDocForm(Id, gblFuction.setDate(vDtFrom), gblFuction.setDate(vDtTo), vGroupId, 0, 1);
                    dt1 = ds1.Tables[0];
                    dt2 = ds1.Tables[1];
                    dt3 = ds1.Tables[2];
                    dt4 = ds1.Tables[3];
                    dt5 = ds1.Tables[4];
                    dt6 = ds1.Tables[5];
                    dt7 = ds1.Tables[6];

                    oUsr = new CDigiDoc();
                    dsDigiDoc = oUsr.getDigiDocDtlsByDocId(0, "", "Y");
                    dtDigiDocDtls = dsDigiDoc.Tables[0];
                    string vPath = "";

                    if (dt1.Rows.Count > 0)
                    {
                        if (vRptPath != string.Empty)
                        {
                            //-----------------------Download Images-----------------------
                            string vEnqId = dt7.Rows[0]["EnquiryId"].ToString();
                            //string pRequestdata = "{\"pEnquiryId\":\"" + vEnqId + "\"}";
                            //CallAPI("GetImage", pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
                            //-------------------------------------------------------------                           
                            foreach (DataRow dr in dt1.Rows)
                            {
                                //string vMemPhoto = Convert.ToString(dr["ImgPath"]);
                                //vMemPhoto = vMemPhoto.Substring(1, vMemPhoto.Length - 1).Replace('\\', '_');
                                //vMemberPhoto = PathMinio + vMemPhoto;
                                //dr["ImgPath"] = vMemberPhoto;

                                dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                            }
                            dt1.AcceptChanges();
                            if (dt7.Rows.Count > 0)
                            {
                                vImgPath = PathMinio;
                                string M_IdentyPRofId = dt7.Rows[0]["M_IdentyPRofId"].ToString();
                                string M_AddProfId = dt7.Rows[0]["M_AddProfId"].ToString();
                                string B_IdentyProfId = dt7.Rows[0]["B_IdentyProfId"].ToString();
                                foreach (DataRow dr in dt7.Rows)
                                {
                                    //dr["KYC1"] = vImgPath + (Convert.ToString(dr["KYC1"]).Substring(1, Convert.ToString(dr["KYC1"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC1Back"] = vImgPath + (Convert.ToString(dr["KYC1Back"]).Substring(1, Convert.ToString(dr["KYC1Back"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC2"] = vImgPath + (Convert.ToString(dr["KYC2"]).Substring(1, Convert.ToString(dr["KYC2"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC2Back"] = vImgPath + (Convert.ToString(dr["KYC2Back"]).Substring(1, Convert.ToString(dr["KYC2Back"]).Length - 1).Replace('\\', '_'));
                                    //dr["KYC3"] = vImgPath + (Convert.ToString(dr["KYC3"]).Substring(1, Convert.ToString(dr["KYC3"]).Length - 1).Replace('\\', '_')); ;
                                    //dr["KYC3Back"] = vImgPath + (Convert.ToString(dr["KYC3Back"]).Substring(1, Convert.ToString(dr["KYC3Back"]).Length - 1).Replace('\\', '_'));

                                    dr["KYC1Byte"] = M_IdentyPRofId == "13" ? GetAadhaarByteImage("IDProofImage.png", vEnqId, "I") : GetByteImage("IDProofImage.png", vEnqId, "I");
                                    dr["KYC1BackByte"] = M_IdentyPRofId == "13" ? GetAadhaarByteImage("IDProofImageBack.png", vEnqId, "I") : GetByteImage("IDProofImageBack.png", vEnqId, "I");
                                    dr["KYC2Byte"] = M_AddProfId == "13" ? GetAadhaarByteImage("AddressProofImage.png", vEnqId, "I") : GetByteImage("AddressProofImage.png", vEnqId, "I");
                                    dr["KYC2BackByte"] = M_AddProfId == "13" ? GetAadhaarByteImage("AddressProofImageBack.png", vEnqId, "I") : GetByteImage("AddressProofImageBack.png", vEnqId, "I");
                                    dr["KYC3Byte"] = B_IdentyProfId == "13" ? GetAadhaarByteImage("AddressProofImage2.png", vEnqId, "I") : GetByteImage("AddressProofImage2.png", vEnqId, "I");
                                    dr["KYC3BackByte"] = B_IdentyProfId == "13" ? GetAadhaarByteImage("AddressProofImage2Back.png", vEnqId, "I") : GetByteImage("AddressProofImage2Back.png", vEnqId, "I");
                                }
                            }
                            dt7.AcceptChanges();

                            if (dt3.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt3.Rows)
                                {
                                    // dr["ImgPath"] = vMemberPhoto;
                                    dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                                }
                                dt3.AcceptChanges();
                            }

                            using (ReportDocument rptDoc = new ReportDocument())
                            {
                                vRptName = "Application_Form";
                                rptDoc.Load(vRptPath);
                                rptDoc.SetDataSource(dt1);

                                if (vLanguage == "E")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFrmP4.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "H")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Hindi.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Hindi.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Hindi.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Hindi.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Hindi.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Hindi.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "G")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Gujarati.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Gujarati.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Gujarati.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Gujarati.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Gujarati.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Gujarati.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "O")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Odia.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Odia.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Odia.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Odia.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Odia.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Odia.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "B")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "K")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Kannad.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Kannad.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Kannada.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "T")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Tamil.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Tamil.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Tamil.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "M")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Malayalam.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Malayalam.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Malayalam.rpt"].SetDataSource(dt7);
                                }
                                else if (vLanguage == "L")
                                {
                                    rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                    rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                    rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                    rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                    rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                    rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                                }
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Id + '_' + vRptName);
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
                oUsr = null;
                ds1 = null;
                dt1 = null; dt2 = null; dt3 = null;
                dt4 = null;
                dt5 = null; dt6 = null; dt7 = null;
            }
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
                dtMob = oMem.GetDisbSMSMemMob(vLoanAppId);

                btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                btnSendResendOTP.Attributes.Add("style", "visibility:Visible;");
                txtOTP.Attributes.Add("style", "visibility:Visible;");
                spnOTPExpire.Attributes.Add("style", "visibility:Visible;");
                btnCapture.Attributes.Add("style", "visibility:Visible;");
                btnUpload.Attributes.Add("style", "visibility:Visible;");
                btnResetCam.Attributes.Add("style", "visibility:Visible;");
                spnlocation.Attributes.Add("style", "visibility:Visible;");

                if (dtMob.Rows.Count > 0)
                {
                    _vMobileNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                    hndMobileNo.Value = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                    ViewState["MobNo"] = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
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
                    // vMessageBody = "Thank you for your loan application with Centrum Microcredit Ltd. Your verification OTP is " + vRandomToken.ToString().Trim() + " Thanking you, Centrum Microcredit Ltd.";
                    vMessageBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vRandomToken.ToString().Trim() + ".";
                    string vRe = SendSMS(_vMobileNo, vMessageBody);
                    string[] arr = vRe.Split('|');
                    vSuccessStat = arr[0];//"success";
                    // spnOTPExpire.InnerHtml = vMessageBody;

                    spnlocation.InnerHtml = hdnLatLong.Value;

                    if (vSuccessStat.ToString().Trim().ToLower() == "success")
                    {
                        oUsr = new CDigiDoc();
                        Int32 _vRet = oUsr.SaveDigiDocOTPDetails(hdnLoanAppNo.Value.Trim(), Convert.ToInt64(hdnDigiDocId.Value), hdnOTP.Value, ref _vDigiDocDtlsID);
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
                //String userid = "2000194447";
                //String passwd = "Centrum@2020";
                String userid = "2000204129";
                String passwd = "Unity@1122";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=CENOTP&v=1.1&format=text";
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
                    ocr = new CDigiDoc();
                    _vCustomerName = hdnCustomerName.Value.Trim();
                    _vPlatform = hdnPlatform.Value.Trim();
                    _vBrowser = GetBrowserDetails();
                    _vGeoLocation = hdnLatLong.Value;
                    _vIPAddress = GetIPAddress();
                    //_vMobNo = hndMobileNo.Value.Trim();
                    _vMobNo = Convert.ToString(ViewState["MobNo"]);
                    _vDeviceDetails = hdnUserAgent.Value.Trim();

                    _result = ocr.UpdateDigiDocOTPDetails(Convert.ToInt64(hdnDigiDocDtlsId.Value), _vCustomerName, _vPlatform, _vBrowser, _vGeoLocation, _vIPAddress, _vMobNo, _vDeviceDetails);
                    if (_result > 0)
                    {

                        btnLinkDownloadDocument.Attributes.Add("style", "visibility:hidden;");
                        btnSendResendOTP.Attributes.Add("style", "visibility:hidden;");
                        txtOTP.Attributes.Add("style", "visibility:hidden;");
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

        #region Save Doc Image
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

        #region URLExist
        private bool URLExist(string pPath)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            WebRequest wR = WebRequest.Create(pPath);
            WebResponse webResponse;
            try
            {
                wR.Timeout = 3000;
                webResponse = wR.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
        #region Craete Report Doc

        private string GetReportDocForDigitalSign(string pLoanAppNo, string pMemberName, Int64 vDigiDocDtlsId)
        {
            CReports oRpt = null;
            DataSet ds1 = new DataSet(), dsDigiDoc = new DataSet();
            DataTable dt1 = new DataTable(), dt2 = new DataTable(), dt3 = new DataTable();
            DataTable dt4 = new DataTable(), dt5 = new DataTable(), dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dtDigiDocDtls = new DataTable();

            string vRptPath = "", vImgPath = "", vLanguage = "E", vGroupId = "", vFileName = "No File Created", vDtFrom = "", vDtTo = "";
            CDigiDoc oUsr = null;
            hdnOTP.Value = String.Empty;
            hdnSMSVerifyTimeStamp.Value = String.Empty;
            vLanguage = Convert.ToString(ViewState["Language"]);
            vGroupId = Convert.ToString(ViewState["GroupId"]);
            vDtFrom = Convert.ToString(ViewState["DateFrom"]);
            vDtTo = Convert.ToString(ViewState["DateTo"]);
            try
            {
                vRptPath = string.Empty;
                if (vLanguage == "E") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocForm.rpt";
                if (vLanguage == "H") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormHindi.rpt";
                if (vLanguage == "G") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormGujarati.rpt";
                if (vLanguage == "O") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormOdia.rpt";
                if (vLanguage == "B") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintBengali.rpt";
                if (vLanguage == "K") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintKannad.rpt";
                if (vLanguage == "T") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTamil.rpt";
                if (vLanguage == "M") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintMalayalam.rpt";
                if (vLanguage == "L") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTelugu.rpt";

                oRpt = new CReports();
                string Id = hdnLoanAppNo.Value;
                ds1 = oRpt.GetDigitalDocForm(Id, gblFuction.setDate(vDtFrom), gblFuction.setDate(vDtTo), vGroupId, Convert.ToInt64(hdnDigiDocDtlsId.Value), 1);
                dt1 = ds1.Tables[0];
                dt2 = ds1.Tables[1];
                dt3 = ds1.Tables[2];
                dt4 = ds1.Tables[3];
                dt5 = ds1.Tables[4];
                dt6 = ds1.Tables[5];
                dt7 = ds1.Tables[6];

                oUsr = new CDigiDoc();
                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];
                if (dtDigiDocDtls.Rows.Count > 0)
                {
                    //-----------------------Download Images-----------------------                           
                    //string pRequestdata = "{\"pId\":\"" + Id + "\"}";
                    //CallAPI("GetDigiDocOTPImg", pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
                    ////-------------------------------------------------------------
                    //dtDigiDocDtls.Rows[0]["PhotoImage"] = PathOTP + Id + ".jpg";

                    dtDigiDocDtls.Rows[0]["PhotoImageByte"] = GetByteImage(Id + ".jpg", Id, "O");
                    dtDigiDocDtls.AcceptChanges();
                }
                string vPath = "";
                if (dt1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        string vEnqId = dt7.Rows[0]["EnquiryId"].ToString();
                        string vMemberPhoto = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            //string vMemPhoto = Convert.ToString(dr["ImgPath"]);
                            //vMemPhoto = vMemPhoto.Substring(1, vMemPhoto.Length - 1).Replace('\\', '_');
                            //vMemberPhoto = PathMinio + vMemPhoto;
                            //dr["ImgPath"] = vMemberPhoto;

                            dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                        }
                        dt1.AcceptChanges();
                        if (dt7.Rows.Count > 0)
                        {
                            vImgPath = PathMinio;
                            foreach (DataRow dr in dt7.Rows)
                            {
                                //dr["KYC1"] = vImgPath + (Convert.ToString(dr["KYC1"]).Substring(1, Convert.ToString(dr["KYC1"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC1Back"] = vImgPath + (Convert.ToString(dr["KYC1Back"]).Substring(1, Convert.ToString(dr["KYC1Back"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC2"] = vImgPath + (Convert.ToString(dr["KYC2"]).Substring(1, Convert.ToString(dr["KYC2"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC2Back"] = vImgPath + (Convert.ToString(dr["KYC2Back"]).Substring(1, Convert.ToString(dr["KYC2Back"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC3"] = vImgPath + (Convert.ToString(dr["KYC3"]).Substring(1, Convert.ToString(dr["KYC3"]).Length - 1).Replace('\\', '_')); ;
                                //dr["KYC3Back"] = vImgPath + (Convert.ToString(dr["KYC3Back"]).Substring(1, Convert.ToString(dr["KYC3Back"]).Length - 1).Replace('\\', '_'));

                                dr["KYC1Byte"] = GetByteImage("IDProofImage.png", vEnqId, "I");
                                dr["KYC1BackByte"] = GetByteImage("IDProofImageBack.png", vEnqId, "I");
                                dr["KYC2Byte"] = GetByteImage("AddressProofImage.png", vEnqId, "I");
                                dr["KYC2BackByte"] = GetByteImage("AddressProofImageBack.png", vEnqId, "I");
                                dr["KYC3Byte"] = GetByteImage("AddressProofImage2.png", vEnqId, "I");
                                dr["KYC3BackByte"] = GetByteImage("AddressProofImage2Back.png", vEnqId, "I");

                            }
                        }
                        dt7.AcceptChanges();

                        if (dt3.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt3.Rows)
                            {
                                // dr["ImgPath"] = vMemberPhoto;
                                dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                            }
                            dt3.AcceptChanges();
                        }

                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
                        vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";

                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);

                            if (vLanguage == "E")
                            {
                                rptDoc.Subreports["DigitalDocFormP2.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFrmP4.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "H")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Hindi.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Hindi.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Hindi.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Hindi.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Hindi.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Hindi.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "G")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Gujarati.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Gujarati.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Gujarati.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Gujarati.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Gujarati.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Gujarati.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "O")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Odia.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Odia.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Odia.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Odia.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Odia.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Odia.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "B")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "K")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Kannad.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Kannad.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Kannada.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "T")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Tamil.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Tamil.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Tamil.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "M")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Malayalam.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Malayalam.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Malayalam.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "L")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                            }

                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }
                            if (MinioYN == "Y")
                            {
                                Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                                UploadFileMinio(strmToByte(reportStream), pLoanAppNo + ".pdf", pLoanAppNo, DigiDocBucket, MinioUrl);
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
                oUsr = null;
                ds1 = null;
                dt1 = null; dt2 = null; dt3 = null;
                dt4 = null;
                dt5 = null; dt6 = null; dt7 = null;
            }
            return vFileName;
        }

        #endregion

        #region API Calling
        public string CallAPI(string pApiName, string pRequestdata, string pReportUrl)
        {
            try
            {
                string Requestdata = pRequestdata;
                string postURL = pReportUrl + "/" + pApiName;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            finally
            {
            }
            return "";
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
            return imgByte;
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
            return imgByte;
        }
        #endregion
    }
}