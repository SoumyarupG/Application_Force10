using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace CentrumMobService
{
    [ServiceContract]
    public interface ICentrumService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MM74y5St3biCFgs8S32HuQ",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetAppVersion(AppVersionData appVersionData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Yr5432KjCeGS1VgQtBMaZg",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LoginData> GetMobUser(UserData userData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "oZHl8f48tFCNzzp1hC3P1A",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCData> GetKYCInfo(PostKYCData postKYCData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "eyKThxpUjShxFv9WSiy74w",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<AddressData> GetAddressDtl(PostAddressData postAddressData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MosXxM8ccv3qx46HF7XTJg",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<StateData> GetStateList();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "4og97Pzv9esfHw7m9C7roA",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveGroup(PostGroupData postGroupData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveCenter",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveCenter(PostCenterData postCenterData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ceJB7qSNFr8TiVTGvD3B3g",     //SaveKYC
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKYC(PostKYCSaveData postKYCSaveData, OCRData postOCRData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Satyaj1tAttendence",     //SaveEmployeeAttendance
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveEmployeeAttendance(PostSaveEmployeeAttendance PostSaveEmployeeAttendance);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ejnnv4QbyxvNJk2OJrn5kA",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> KYCImageUpload(Stream image);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "EmpAttendanceUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<EMPImageSave> EMPImageUpload(Stream image);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GRTImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<EMPImageSave> GRTImageUpload(Stream image);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "wLiis1LpVFTJxPrk8NL5Q",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveEquifax(PostCBSaveData postCBSaveData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "qS4JwQFmRb4vRL3XKNBZkdqZUB41q4",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GroupDBData> GetGroupDashboard(PostKYCData postKYCData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "6qqEtL4UCx1ACRhhcKbs",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberCreationSubData> MemberCreationData(PostMemberData postMemberData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "aNjXeGhKsD5i0hFnb",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "NTMgZTM6MNoQ04E4wwvVPeoHSCfB0cgyka8tvu9I",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetNewMemberInfo> NewMemberCreation(PostMemberFormData postMemberFormData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "erOKwZW9jVeYvlIhEOawUmv4x8IXo",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<ExistingMemberAllData> ExistingMemberCreation(PostMemberFormData postMemberFormData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "O4BUx6NTwozUvdM3H2BtpduOBd4",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UploadMemberCreation(PostMemberSaveData postMemberSaveData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "L7sKCS4R2e7n8XII1xCCM1sAo3az",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<QNAData> GetQuestionAnswer();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetQuestionAnswerNew",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<QNADataNew> GetQuestionAnswerNew();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "RzmhPjOzDqZAryD6xpXKjlM",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberHVData> GetHouseVisitData(PostHVData postHVData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "2vglqmUyPjCk2g9g1aSnyQ",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveHouseVisit(PostHVDataSave postHVDataSave);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveHouseVisitNew",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveHouseVisitNew(PostHVDataSaveNew postHVDataSaveNew);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "2OAQQV5MtE6iqx3",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PurposeData> GetLoanPurpose();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "vF4aRJOSdQZU0ddG5cceUg",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<SchemeData> GetLoanScheme(PostSchemeData postSchemeData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "iO2AmOWEQI126jipqsEQ",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberLoanAppData> GetLoanAppData(PostLoanApp postLoanApp);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "7IKZNnadwjc4JXBcp2kEChsgp",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberLoanDtlData> LoanAppDtlByMember(PostLoanAppDtl postLoanAppDtl);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ZPiep0QFRKEFTSTa1UeKQtliadUH",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberLoanDtlByTypeData> LoanAmtByMemberScheme(PostLoanAppDtlByMember postLoanAppDtlByMember);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "C7kezqajZnyUccwyHfsQ",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveGRT(SaveGRTData saveGRTData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "qZwEyZuJaLGXLKs5GFRGve",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<MemberLoanDashboardData> GetMemberLoanDashboard(PostDashboardData postDashboardData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "XfShpa3NS61CDZHj0lVk",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetReHouseData> GetReHouseVisitData(PostReHVData postReHVData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "8qb1LM43QqRTaseI3HuAbwiJbewh9QEx7geo",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveReHouseVisit(SaveReHVData saveReHVData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "iiRo2cmV7FxThIhGAXkFZBkyQtgDWsERIaOLbfimpkk",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<CollectionData> GetCollectionData(PostCollectionData postCollectionData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "sqWfbnLKKinWyIZhAljEHLkVnLfuuwN5RheiLv6YccM",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string InsertCollection(SaveCollectionData saveCollectionData);

        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "Ib2N2KmdZO5gLuUPeuTcEVcUyQ",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetEquifaxReport(ReportData reportData);

        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "/SaveMemberImg",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Bare)]
        string SaveMemberImg(PostImage postImage);


        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "GetWorkAllocInfo",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<WorkAllocData> GetWorkAllocInfo(PostKYCData WorkAllocationData);


        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "GetCenterList",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCenterListData> GetCenterList(PostCenterList CenterListData);

        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "gfdre53dddsrweggha5",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LoanUtilizationData> GetLoanUtilityCheck(PostLoanUtilizationData PostLoanUtilizationData);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "7ggd53hhss3wwd",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateLoanUtilizationCheck(PostUpdateLoanUtilCheck postUpdateLoanUtilCheck);

        [OperationContract]
        [WebInvoke(Method = "POST",
             UriTemplate = "GetLUCAppData",
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LucCenterGroupEoList> GetLUCAppData(PostLoanApp postLoanApp);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_ChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendOTP(OTPData objOTPData);

        #region ICICI NEFT

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICBalanceFetch",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBalanceFetchResponse ICICBalanceFetch(PostBalEnqReq vPostBalEnqReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICBankTransaction",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBankTransactionResponse ICICBankTransaction(PostBankTransaction vPostBankTransaction);//

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICBankTransactionStatus",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBankTransactionStatusResponse ICICBankTransactionStatus(PostBankTransactionStatus vPostBankTransactionStatus);//


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICIDisbursement",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ICICIDisbursement(string pXml, string pUserId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICIICDisbursement",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ICICIICDisbursement(string pXml, string pUserId, string pTransDate, string pBankDescId);

        #endregion

        #region Digital Signature

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DigitalSignedDocUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<DigitalDocumentSave> DigitalSignedDocUpload(Stream DocFile);

        #endregion

        #region KARZA Integration

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "OCRPhototoData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<OCRParameterResponse> OCRPhototoData(Stream DocFile);

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
            UriTemplate = "SaveOCRData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveOCRData(OCRData vOCRData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveKarzaAadharVerifyData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseData);

        #endregion

        #region ActivityTrack
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PopROForActivity",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<EoData> PopROForActivity(PostKYCData postKYCData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PopActivityData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<ActivityData> PopActivityData(PostKYCData postKYCData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveActivityData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveActivityData(PostActivityData postActivityData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GenerateOCRLogReport",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 GenerateOCRLogReport(string pFromDt, string pToDt, string pFormat, string pUserId);

        #endregion

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ICICDeCrypt",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ICICBalanceFetchResponse ICICDeCrypt(string strAPIResponse);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetJokataToken",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetJokataToken();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "RampRequest",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string RampRequest(PostRampRequest postRampRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaAadhaarXmlOTP",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        AadhaarOTPResponse KarzaAadhaarXmlOTP(AadhaarXmlOTP aadhaarXmlOtp);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaAadhaarXml",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string KarzaAadhaarXml(AadhaarXmlDownload aadhaarXmlDownload);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaeAadhaarOTP",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        AadhaarOTPResponse KarzaeAadhaarOTP(eAadhaarOTP eAadhaarOTP);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "eAadhaarDownload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string eAadhaarDownload(eAadhaarDownload eAadhaarDownload);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendDigitalConcentSMS",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendDigitalConcentSMS(string pMobileNo, string pSMSContent, string pSMSTemplateId, string pSMSLanguage);

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "SMSAPITest",     //SMS API Test
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string SMSAPITest();
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "RampStatus",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        RampStatus RampStatus(RampStatusRequest vRampStatusRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AadhaarVault",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        AadhaarVaultResponse AadhaarVault(AadhaarVault AadhaarVault);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ProsidexSearchCustomer",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PosidexVerification",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        PosidexVerifyResponse PosidexVerification(PosidexVerificationData pProsiVerifyReq);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Prosidex",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse Prosidex(ProsiReq pProsiReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetAadhaarNoByRefId",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        AadhaarNoByRefId GetAadhaarNoByRefId(AadhaarNoReq pAadhaarNoReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AadhaarVaultRefData",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string AadhaarVaultRefData(AadhaarVault AadhaarVault);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Jocata",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string JocataCalling(string pMemberID, string pCreatedBy, string pCGTId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "JocataRiskCat",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string JocataRiskCat(RiskCatReq vRiskCatReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "getUcic",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string getUcic(ProsiReq pProsiReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "getResponse",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse getResponse(GetResponseReq req);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BankAcVerify",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        FingPayResponse BankAcVerify(FingPayRequest req);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "MonitoringQuestion",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetMonitoringQuestion> MonitoringQuestion(string pVisitType);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveMonitoring",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveMonitoring(PostSaveMonitoring postSaveMonitoring);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveMonitoringOD",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveMonitoringOD(PostSaveMonitoring postSaveMonitoring);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "TimeChk",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ServerStatus TimeChk();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetMonitoringCompliance",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetMonitoringCompliance> GetMonitoringCompliance(PostMonitoringCompliance postMonitoringCompliance);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetOtherMonitoringCompliance",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetOtherMonitoringCompliance> GetOtherMonitoringCompliance(PostMonitoringCompliance postMonitoringCompliance);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Get_Insp_Mob_OtherVisitDownload",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetOtherMonitoringData> Get_Insp_Mob_OtherVisitDownload(PostMonitoringCompliance postMonitoringCompliance);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Get_Insp_Mob_ODVisitDownload",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetOtherMonitoringData> Get_Insp_Mob_ODVisitDownload(PostMonitoringCompliance postMonitoringCompliance);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveMonitoringCompliance",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveMonitoringCompliance(PostSaveMonitoringCompliance postSaveMonitoringCompliance);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Decrypt",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string Decrypt();


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetRptIncentive_LoWise",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetIncentiveLoWise> GetRptIncentive_LoWise(PostIncentiveLoWise postIncentiveLoWise);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertNEFTTransfer",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string InsertNEFTTransfer(string pXml, string pDescId, string pCreatedby, string pLoanDt, string pEntType);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DayEndProcess",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 DayEndProcess(string pUserId, string pDayEnddt, string pXmlData, string YearNo, string FinFrom, string pFinYear);

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

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyCreateProfile",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProfileResponse IdfyCreateProfile(PostAadharData postAadharData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AesEncryptData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AesEncryptData(string pPassword);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AesDecryptData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AesDecryptData(string pPassword);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Aes256",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Aes256();

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "aes256decrypt",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string aes256decrypt(string RsaKey, string encryptedText);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PosidexEncryption",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse PosidexEncryption(ProsiReq pProsiReq);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PosidexEncrypt",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsidexResponse PosidexEncrypt(Request Req);

        [OperationContract]
        [WebInvoke(UriTemplate = "test?code={requestCode}",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ProsiReq test(string requestCode);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AadharVaultSignRequest",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string AadharVaultSignRequest(string text);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BimaPlan",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string BimaPlan();

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "GetGuote",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //string GetGuote();

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    UriTemplate = "SubmitProposal",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.Bare)]
        //string SubmitProposal();


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PosidexReCall",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string PosidexReCall();

        #region ICICI QR
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "CreateQR",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        CollQRResData CreateQR(CollQRReqData Req);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetICICIPamentStatus",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ICICIQRPaymentStatus GetICICIPamentStatus(PostICICIPaymentStat Req);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UdateICICIPaymentStatus",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ICICIQRPaymentStatus UdateICICIPaymentStatus(PostPaymentStatus Req);

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

        #region ValidateAadhaar
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ValidateAadhaar",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ValidateAadhaar(string aadhaarNumber);
        #endregion

        #region PTP
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetTeleCallingData",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PTPData> Mob_GetPTPData(PostKYCData pPTPRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "rptPartyLedger",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string rptPartyLedger(string pLoanId, string pBrCode, string pProject);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveTeleCalling",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveTeleCalling(SaveTeleCallingData TeleCallingData);

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
            UriTemplate = "ValidateOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ForgotOTPRes ValidateOTP(ValidateOTPReqData objOTPData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ForgotPassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ForgotOTPRes ForgotPassword(Post_ForgotPassword Post_ForgotPassword);

        #endregion

        #region IDFY Digital DOC
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyeSignStatus",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        IdfyeSignResponse IdfyeSignStatus(IdfySignReq response);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "IdfyeSignStatus1",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        IdfyeSignResponse IdfyeSignStatus1(IdfySign pRequest);
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

        #region InsertBulkFunderUploadApprove
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertBulkFunderUploadApprove",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertBulkFunderUploadApprove(string pFSUID, string pLoginDt, string pAppBy, string pAppRej);
        #endregion

        #region RecoveryPoolUpload
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "RecoveryPoolUpload",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        void RecoveryPoolUpload(string pLogin, string pCollXml, string pCreatedBy);
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

        #region LUC

        #region GetLoanUtilizationQsAns
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetLoanUtilizationQsAns",
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
            BodyStyle = WebMessageBodyStyle.Wrapped)]
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

        #region GetBranchCtrlByBranchCode
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetBranchCtrlByBranchCode",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetBranchCtrlByBranchCode> GetBranchCtrlByBranchCode(PostBranchCtrlByBranchCode postBranchCtrlByBranchCode);
        #endregion

        #region auaKuaAuthWithBiometric
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "auaKuaAuthWithBiometric",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string auaKuaAuthWithBiometric();
        #endregion

    }
}
