using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace UnityVriddhiMobService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUnityVriddhiService" in both code and config file together.
    [ServiceContract]
    public interface IUnityVriddhiService
    {
        #region GetAppVersion

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetAppVersion",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetAppVersion(string EncryptedRequest);

        #endregion

        #region Mob_GetUser

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetMobUser",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetMobUser(string EncryptedRequest);

        #endregion

        #region SendOTP

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendOTP(string EncryptedRequest);

        #endregion

        #region SaveKYC

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveKYC",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKYC(string EncryptedRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveOCRData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveOCRData(PostOCRData postOCRData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveVerifyData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveVerifyData(PostVerifyData postVerifyData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KYCImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> KYCImageUpload(Stream image);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PdImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> PdImageUpload(Stream image);

        #endregion

        #region GetKYCInfo
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetKYCInfo",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetKYCInfo(string EncryptedRequest);
        #endregion

        #region GetAddressDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetAddressDtl",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetAddressDtl(string EncryptedRequest);
        #endregion

        #region GetStateList
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetStateList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetStateList();
        #endregion

        #region GetBusinessTypeList
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessTypeList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetBusinessTypeList();
        #endregion

        #region GetBusinessSubType
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessSubType",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetBusinessSubType(string EncryptedRequest);
        #endregion

        #region GetBusinessActivity
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessActivity",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetBusinessActivity(string EncryptedRequest);
        #endregion

        #region KARZA Integration

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "OCRPhototoData",
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped)]
        //List<OCRParameterResponse> OCRPhototoData(Stream DocFile);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetKarzaToken",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetKarzaToken(string vAadharNo);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "NameMatching",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        MatchResponse NameMatching(NameMatch vNameMatch);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AddressMatching",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        MatchResponse AddressMatching(AddressMatch vAddressMatch);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaVoterIDKYCValidation",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string KarzaVoterIDKYCValidation(string EncryptedRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaPANValidation",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string KarzaPANValidation(string EncryptedRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "FaceMatching",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        MatchResponse FaceMatching(FaceMatch vFaceMatch);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveKarzaAadharVerifyData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseData);

        #endregion

        #region GetMemberDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetMemberDtl",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetMemberDtl(string EncryptedRequest);
        #endregion

        #region SaveEMOCRData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveEMOCRData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveEMOCRData(PostEMOCRData postEMOCRData);
        #endregion

        #region GetLoanScheme
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetLoanScheme",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetLoanScheme(string EncryptedRequest);
        #endregion

        #region MemberCreationData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MemberCreationList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string MemberCreationList(string EncryptedRequest);
        #endregion

        #region MemberCreationData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "NewMemberCreationData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string NewMemberCreationData(string EncryptedRequest);
        #endregion

        #region SavePdBySO
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePdBySO",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePdBySO(string EncryptedRequest);
        #endregion

        #region SavePdByBM
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePdByBM",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePdByBM(string EncryptedRequest);
        #endregion

        #region GetIFSCDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetIFSCDtl",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetIFSCDtl(string EncryptedRequest);
        #endregion

        #region GetPdByBMData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetPdByBMData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetPdByBMData(string EncryptedRequest);
        #endregion

        #region GetPdByBMData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetPdDtlByPdId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetPdDtlByPdId(string EncryptedRequest);
        #endregion

        #region ChangePassword
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_ChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_ChangePassword(string EncryptedRequest);

        #endregion

        #region ICICIDisbursement
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICBalanceFetch",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBalanceFetchResponse ICICBalanceFetch(PostBalEnqReq vPostBalEnqReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICIDisbursement",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ICICIDisbursement(string pXml, string pUserId);

        #endregion

        #region UpdateNupayStatus
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UpdateNupayStatus",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        NupayStatus UpdateNupayStatus(string id, string accptd, string accpt_ref_no, string reason_code, string reason_desc,
             string reject_by, string npci_ref, string credit_datetime, string umrn, string auth_type);
        #endregion

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "SMSAPITest",     //SMS API Test
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string SMSAPITest();

        #region KarzaAadhaarXmlOTP
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaAadhaarXmlOTP",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        AadhaarOTPResponse KarzaAadhaarXmlOTP(AadhaarXmlOTP aadhaarXmlOtp);
        #endregion

        #region KarzaAadhaarXml
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaAadhaarXml",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string KarzaAadhaarXml(AadhaarXmlDownload aadhaarXmlDownload);
        #endregion

        #region KarzaeAadhaarOTP
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaeAadhaarOTP",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        AadhaarOTPResponse KarzaeAadhaarOTP(eAadhaarOTP eAadhaarOTP);
        #endregion

        #region eAadhaarDownload
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "eAadhaarDownload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string eAadhaarDownload(eAadhaarDownload eAadhaarDownload);
        #endregion

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        AadhaarOTPResponse WebHook();

        #region Prosidex
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Prosidex",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse Prosidex(ProsiReq pProsiReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ProsidexSearchCustomer",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ProsidexSearchCustomerTest",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string ProsidexSearchCustomerTest(ProsidexRequest prosidexRequest);
        #endregion

        #region Jocata
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "JocataRequest",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string JocataRequest(string pMemberID, Int32 pPdID, Int32 pCreatedBy);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "JocataRiskCat",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string JocataRiskCat(RiskCatReq vRiskCatReq);

        #endregion

        #region Time Check
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "TimeChk",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string TimeChk();
        #endregion

        #region Bank Ac Verify
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BankAcVerify",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string BankAcVerify(string EncryptedRequest);
        #endregion

        #region Decrypt
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Decrypt",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string Decrypt();
        #endregion

        #region InsertBulkCollection
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertBulkCollection",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy, string pBCProductId);
        #endregion

        #region SaveOtherCollectionBulk
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveOtherCollectionBulk",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 SaveOtherCollectionBulk(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml, string pCreatedBy);
        #endregion

        #region IDFY
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyVoterVerify",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        KYCVoterIDResponse IdfyVoterVerify(PostVoterData PostVoterData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyAadharVerify",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        IdfyAadharVerifyData IdfyAadharVerify(PostAadharData postAadharData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyAadharVerifyData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string IdfyAadharVerifyData(IdfyAadharVerifyResponseData pData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyAadharVerifyJson",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string IdfyAadharVerifyJson(string pReqId);
        #endregion

        #region GetBase64Img
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBase64Img",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetBase64Img(string pId, string pModule, string pImageName);
        #endregion

        #region UpdateLogoutDt
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UpdateLogoutDt",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateLogoutDt(string EncryptedRequest);
        #endregion

        #region UpdateSessionTime
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UpdateSessionTime",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateSessionTime(string EncryptedRequest);
        #endregion

        #region InsertLoginDt
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertLoginDt",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string InsertLoginDt(string EncryptedRequest);
        #endregion

        #region HighMarkProcess
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "HighMarkProcess",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string HighMarkProcess(string pEnqId, string pMemberName, string pDOB, string pAge, string pAsOnDate,
             string pAadhaar, string pPanId, string pVoterId, string pRationCard, string pRelativeName, string pRelationCode,
             string pContactNo, string pAddress, string pDistrictName, string pStateCode, string pPinCode);
        #endregion
        
    }
}
