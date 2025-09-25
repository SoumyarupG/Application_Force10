using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace CentrumSaralMobService
{
    [ServiceContract]
    public interface ICentrumSaralService
    {
        #region GetAppVersion

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetAppVersion",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetAppVersion(string pVersion);

        #endregion

        #region Mob_GetUser

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetMobUser",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LoginData> GetMobUser(UserData userData);

        #endregion

        #region SendOTP

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendOTP(OTPData objOTPData);

        #endregion

        #region SaveKYC

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveKYC",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKYC(PostKYCSaveData postKYCSaveData, PostOCRData postOCRData, PostEMOCRData postEMOCRData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveOCRData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveOCRData(PostOCRData postOCRData);

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
        List<KYCData> GetKYCInfo(PostKYCData postKYCData);
        #endregion

        #region GetAddressDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetAddressDtl",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<AddressData> GetAddressDtl(PostAddressData postAddressData);
        #endregion

        #region GetStateList
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetStateList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<StateData> GetStateList();
        #endregion

        #region GetBusinessTypeList
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessTypeList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BusinessTypeData> GetBusinessTypeList();
        #endregion

        #region GetBusinessSubType
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessSubType",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BusinessSubTypeData> GetBusinessSubType(string pBusiTypeId);
        #endregion

        #region GetBusinessActivity
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBusinessActivity",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<BusinessActivityData> GetBusinessActivity(string pBusiSubTypeId);
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
        KYCVoterIDResponse KarzaVoterIDKYCValidation(KYCVoterIDRequest vPostVoterData);

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
        List<MemberData> GetMemberDtl(string pMemberId);
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
        List<SchemeData> GetLoanScheme(PostSchemeData postSchemeData);
        #endregion

        #region MemberCreationData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MemberCreationList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberCreationSubData> MemberCreationList(PostMemberData postMemberData);
        #endregion

        #region MemberCreationData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "NewMemberCreationData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetNewMemberInfo> NewMemberCreationData(PostMemberFormData postMemberFormData);
        #endregion

        #region SavePdBySO
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePdBySO_XX",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePdBySO(PostPdBySo postPdBySo);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePdBySO",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        SavePdBySORes SavePdBySO_XX(postPdBySoXX postPdBySo);
        #endregion

        #region SavePdByBM
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePdByBM",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePdByBM(PostPdByBM postPdByBM);
        #endregion

        #region GetIFSCDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetIFSCDtl",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData);
        #endregion

        #region GetPdByBMData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetPdByBMData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PDByBMData> GetPdByBMData(PostPdByBMData postPdByBMData);
        #endregion

        #region GetPdByBMData
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetPdDtlByPdId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PDByBMDtl> GetPdDtlByPdId(PostPdDtl postPdDtl);
        #endregion

        #region ChangePassword
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_ChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword);

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
            UriTemplate = "PosidexEncryption",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse PosidexEncryption(ProsiReq pProsiReq);

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
            BodyStyle = WebMessageBodyStyle.Bare)]
        ServerStatus TimeChk();
        #endregion

        #region Bank Ac Verify
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BankAcVerify",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        FingPayResponse BankAcVerify(FingPayRequest req);
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
        string UpdateLogoutDt(string LoginId);
        #endregion

        #region UpdateSessionTime
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UpdateSessionTime",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateSessionTime(string LoginId);
        #endregion

        #region InsertLoginDt
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertLoginDt",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        LoginLogOutData InsertLoginDt(LoginReqData Req);
        #endregion

        #region GetCollectionByLoanId
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetCollectionByLoanId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCollectionByLoanId> GetCollectionByLoanId(PostCollectionByLoanId postCollectionByLoanId);
        #endregion

        #region Mob_GetLoanByMemByLo
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLoanByMemByLo",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLoanByMemByLo> Mob_GetLoanByMemByLo(PostLoanByMemByLo postLoanByMemByLo);
        #endregion

        #region Mob_Srv_SaveCollection
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_Srv_SaveCollection",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        GetSaveCollection Mob_Srv_SaveCollection(PostSaveCollection postSaveCollection);
        #endregion

        #region ForgotPassword
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendForgotOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ForgotOTPRes SendForgotOTP(ForgotOTPData objOTPData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ForgotPassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ForgotOTPRes ForgotPassword(Post_ForgotPassword Post_ForgotPassword);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ValidateOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ForgotOTPRes ValidateOTP(ValidateOTPReqData objOTPData);

        #endregion

        #region InsertFundSourceUpload
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertFundSourceUpload",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertFundSourceUpload(string pFunderXml, string pLoginDt, string pCreatedBy);
        #endregion

        #region LUC

        #region GetLoanUtilizationQAns
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetLoanUtilizationQAns",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLoanUtilizationQsAns> GetLoanUtilizationQsAns();
        #endregion


        #region GetLUCPendingDataList
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetLUCPendingDataList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedResponse)]
        List<GetLUCPendingDataList> GetLUCPendingDataList(PostLUCPendingDataList postLUCPendingDataList);
        #endregion

        #region SaveLoanUtilization
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveLoanUtilization",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveLoanUtilization(PostLoanUtilization postLoanUtilization);
        #endregion


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "LUCImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LUCImageSave> LUCImageUpload(Stream image);

        #endregion

        #region IBMAadhaarOTP

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IBMAadhaarOTP",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IBMAadhaarOTPResponse IBMAadhaarOTP(eIBMAadhaarOTP eIBMAadhaarOTP);
        #endregion

        #region IBMAadhaarDownload

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IBMAadhaarDownload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        IBMAadhaarFullResponse IBMAadhaarDownload(eIBMAadhaar eIBMAadhaar);
        #endregion

        #region InsertBulkFunderUploadApprove
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertBulkFunderUploadApprove",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertBulkFunderUploadApprove(string pFSUID, string pLoginDt, string pAppBy, string pAppRej);
        #endregion

        #region GetBranchCtrlByBranchCode
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBranchCtrlByBranchCode",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetBranchCtrlByBranchCode> GetBranchCtrlByBranchCode(PostBranchCtrlByBranchCode postBranchCtrlByBranchCode);
        #endregion
    }
}
