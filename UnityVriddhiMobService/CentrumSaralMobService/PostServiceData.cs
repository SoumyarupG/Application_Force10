using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;


namespace UnityVriddhiMobService
{
    [DataContract]
    public class OTPData
    {
        [DataMember]
        public string pMobileNo { get; set; }
        [DataMember]
        public string pOTP { get; set; }
    }

    [DataContract]
    public class UserData
    {
        [DataMember]
        public string pUserName { get; set; }

        [DataMember]
        public string pPassword { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pImeiNo1 { get; set; }

        [DataMember]
        public string pImeiNo2 { get; set; }

        [DataMember]
        public string pVersionCode { get; set; }

        [DataMember]
        public string pEncYN { get; set; }

        [DataMember]
        public string pKey { get; set; }
    }

    [DataContract]
    public class KYCVoterIDRequest
    {
        [DataMember]
        public string consent { get; set; }
        [DataMember]
        public string epic_no { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEoID { get; set; }
    }

    [DataContract]
    public class KYCVoterRequest
    {
        [DataMember]
        public string consent { get; set; }
        [DataMember]
        public string epic_no { get; set; }
    }

    [DataContract]
    public class KYCPANRequest
    {
        [DataMember]
        public string consent { get; set; }
        [DataMember]
        public string pan { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEoID { get; set; }
    }

    [DataContract]
    public class NameMatch
    {
        [DataMember]
        public string name1 { get; set; }
        [DataMember]
        public string name2 { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEoID { get; set; }
        [DataMember]
        public string pIdNo { get; set; }

    }

    [DataContract]
    public class AddressMatch
    {
        [DataMember]
        public string address1 { get; set; }
        [DataMember]
        public string address2 { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEoID { get; set; }
        [DataMember]
        public string pIdNo { get; set; }
    }

    [DataContract]
    public class FaceMatch
    {
        [DataMember]
        public string image1B64 { get; set; }
        [DataMember]
        public string image2B64 { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEoID { get; set; }
        [DataMember]
        public string pIdNo { get; set; }
    }

    [DataContract]
    public class PostKYCSaveData
    {
        [DataMember]
        public string pEnquiryId { get; set; }

        [DataMember]
        public string pMemberId { get; set; }

        [DataMember]
        public string pFirstName { get; set; }

        [DataMember]
        public string pMiddleName { get; set; }

        [DataMember]
        public string pLastName { get; set; }

        [DataMember]
        public string pFullName { get; set; }

        [DataMember]
        public string pDob { get; set; }

        [DataMember]
        public string pAge { get; set; }

        [DataMember]
        public string pGuardianName { get; set; }

        [DataMember]
        public string pGuardianRelation { get; set; }

        [DataMember]
        public string pId1Type { get; set; }

        [DataMember]
        public string pId1No { get; set; }

        [DataMember]
        public string pId2Type { get; set; }

        [DataMember]
        public string pId2No { get; set; }

        [DataMember]
        public string pAddressType1 { get; set; }

        [DataMember]
        public string pHouseNo1 { get; set; }

        [DataMember]
        public string pStreet1 { get; set; }

        [DataMember]
        public string pArea1 { get; set; }

        [DataMember]
        public string pVillage1 { get; set; }

        [DataMember]
        public string pSubDistrict1 { get; set; }

        [DataMember]
        public string pDistrict1 { get; set; }

        [DataMember]
        public string pState1 { get; set; }

        [DataMember]
        public string pStateShortName { get; set; }

        [DataMember]
        public string pAddress { get; set; }

        [DataMember]
        public string pLandMark1 { get; set; }

        [DataMember]
        public string pPostOffice1 { get; set; }

        [DataMember]
        public string pPinCode1 { get; set; }

        [DataMember]
        public string pMobileNo1 { get; set; }

        [DataMember]
        public string pEmailId1 { get; set; }

        [DataMember]
        public string pAddressType2 { get; set; }

        [DataMember]
        public string pHouseNo2 { get; set; }

        [DataMember]
        public string pStreet2 { get; set; }

        [DataMember]
        public string pArea2 { get; set; }

        [DataMember]
        public string pVillageId2 { get; set; }

        [DataMember]
        public string pSubDistrict2 { get; set; }

        [DataMember]
        public string pLandMark2 { get; set; }

        [DataMember]
        public string pPostOffice2 { get; set; }

        [DataMember]
        public string pPinCode2 { get; set; }

        [DataMember]
        public string pMobileNo2 { get; set; }

        [DataMember]
        public string pAadhaarScan { get; set; }

        [DataMember]
        public string pCbId { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pCreatedBy { get; set; }

        [DataMember]
        public string pMode { get; set; }

        [DataMember]
        public string pApplicationType { get; set; }

        [DataMember]
        public string pTyp { get; set; }

        [DataMember]
        public string EnqType { get; set; }

        [DataMember]
        public string pOTP { get; set; }

        [DataMember]
        public string pTimeStamp { get; set; }

        [DataMember]
        public string pOCRApproveYN { get; set; }

        [DataMember]
        public string pKarzaVerifyYN { get; set; }

        [DataMember]
        public string pCoAppCBId { get; set; }

        [DataMember]
        public string pCoAppName { get; set; }

        [DataMember]
        public string pCoAppDOB { get; set; }

        [DataMember]
        public string pCoAppRelationId { get; set; }

        [DataMember]
        public string pCoAppAddress { get; set; }

        [DataMember]
        public string pCoAppState { get; set; }

        [DataMember]
        public string pCoAppPinCode { get; set; }

        [DataMember]
        public string pCoAppMobileNo { get; set; }

        [DataMember]
        public string pCoAppIdentyPRofId { get; set; }

        [DataMember]
        public string pCoAppIdentyProfNo { get; set; }

        [DataMember]
        public string pCoAppAddressPRofId { get; set; }

        [DataMember]
        public string pCoAppAddressProfNo { get; set; }

        [DataMember]
        public string pEarningMemberXml { get; set; }

        [DataMember]
        public string pLoanSchemeId { get; set; }

        [DataMember]
        public string pIsAbledYN { get; set; }

        [DataMember]
        public string pSpeciallyAbled { get; set; }

    }
       

    [DataContract]
    public class PostKYCData
    {
        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostAddressData
    {
        [DataMember]
        public string pBranch { get; set; }
    }

    [DataContract]
    public class PostOCRData
    {
        [DataMember]
        public string EnquiryId { get; set; }
        [DataMember]
        public string ID1AadharFront { get; set; }
        [DataMember]
        public string ID1AadharBack { get; set; }
        [DataMember]
        public string ID1VoterFront { get; set; }
        [DataMember]
        public string ID1VoterBack { get; set; }
        [DataMember]
        public string ID1AadharResponse { get; set; }
        [DataMember]
        public string ID1VoterResponse { get; set; }
        [DataMember]
        public string ID2AadharFront { get; set; }
        [DataMember]
        public string ID2AadharBack { get; set; }
        [DataMember]
        public string ID2VoterFront { get; set; }
        [DataMember]
        public string ID2VoterBack { get; set; }
        [DataMember]
        public string ID2AadharResponse { get; set; }
        [DataMember]
        public string ID2VoterResponse { get; set; }
        [DataMember]
        public string NameMatchingResponse { get; set; }
        [DataMember]
        public string AddressMatchingResponse { get; set; }
        [DataMember]
        public string FaceMatchingResponse { get; set; }
        [DataMember]
        public string CoApplIDAadharFront { get; set; }
        [DataMember]
        public string CoApplIDAadharBack { get; set; }
        [DataMember]
        public string CoApplIDVoterFront { get; set; }
        [DataMember]
        public string CoApplIDVoterBack { get; set; }

        [DataMember]
        public string CoAppID1AadharResponse { get; set; }
        [DataMember]
        public string CoAppID1VoterResponse { get; set; }
        [DataMember]
        public string CoAppNameMatchingResponse { get; set; }
        [DataMember]
        public string CoAppAddressMatchingResponse { get; set; }
        [DataMember]
        public string CoAppFaceMatchingResponse { get; set; }
    }

    [DataContract]
    public class PostEMOCRData
    {
        [DataMember]
        public string EnquiryId { get; set; }
        [DataMember]
        public string EMOCRData { get; set; }
    }

    [DataContract]
    public class EMOCRData
    {
        [DataMember]
        public string EMID { get; set; }
        [DataMember]
        public string IDFront { get; set; }
        [DataMember]
        public string IDBack { get; set; }
    }

    [DataContract]
    public class PostSchemeData
    {
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pIsTopup { get; set; }
        [DataMember]
        public string pUserId { get; set; }
    }

    [DataContract]
    public class PostMemberData
    {
        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pBranch { get; set; }
    }

    [DataContract]
    public class PostMemberFormData
    {
        [DataMember]
        public string pEnquiryId { get; set; }
        [DataMember]
        public string pUserId { get; set; }
    }

    [DataContract]
    public class PostPdBySo
    {
        [DataMember]
        public string pEnquiryId { get; set; }
        [DataMember]
        public string pMemberId { get; set; }
        [DataMember]
        public string pPurposeId { get; set; }
        [DataMember]
        public string pExpLoanAmt { get; set; }
        [DataMember]
        public string pExpLoanTenure { get; set; }
        [DataMember]
        public string pEmiPayingCapacity { get; set; }
        [DataMember]
        public string pExistingLoanNo { get; set; }
        [DataMember]
        public string pTotLnOS { get; set; }
        [DataMember]
        public string pApplTitle { get; set; }
        [DataMember]
        public string pApplFName { get; set; }
        [DataMember]
        public string pApplMName { get; set; }
        [DataMember]
        public string pApplLName { get; set; }
        [DataMember]
        public string pApplGender { get; set; }
        [DataMember]
        public string pApplMaritalStatus { get; set; }
        [DataMember]
        public string pApplEduId { get; set; }
        [DataMember]
        public string pApplReliStat { get; set; }
        [DataMember]
        public string pApplReligion { get; set; }
        [DataMember]
        public string pApplCaste { get; set; }
        [DataMember]
        public string pApplPerAddrType { get; set; }
        [DataMember]
        public string pApplPerHouseNo { get; set; }
        [DataMember]
        public string pApplPerStreet { get; set; }
        [DataMember]
        public string pApplPerLandmark { get; set; }
        [DataMember]
        public string pApplPerArea { get; set; }
        [DataMember]
        public string pApplPerVillage { get; set; }
        [DataMember]
        public string pApplPerSubDist { get; set; }
        [DataMember]
        public string pApplPerPostOffice { get; set; }
        [DataMember]
        public string pApplPerPIN { get; set; }
        [DataMember]
        public string pApplPerDist { get; set; }
        [DataMember]
        public string pApplPerStateId { get; set; }
        [DataMember]
        public string pApplPerContactNo { get; set; }
        [DataMember]
        public string pApplPreAddrType { get; set; }
        [DataMember]
        public string pApplPreHouseNo { get; set; }
        [DataMember]
        public string pApplPreStreet { get; set; }
        [DataMember]
        public string pApplPreLandmark { get; set; }
        [DataMember]
        public string pApplPreArea { get; set; }
        [DataMember]
        public string pApplPreVillageId { get; set; }
        [DataMember]
        public string pApplPreSubDist { get; set; }
        [DataMember]
        public string pApplPrePostOffice { get; set; }
        [DataMember]
        public string pApplPrePIN { get; set; }
        [DataMember]
        public string pApplPreDistId { get; set; }
        [DataMember]
        public string pApplPreStateId { get; set; }
        [DataMember]
        public string pApplMobileNo { get; set; }
        [DataMember]
        public string pApplAddiContactNo { get; set; }
        [DataMember]
        public string pApplPhyFitness { get; set; }
        [DataMember]
        public string pCoApplTitle { get; set; }
        [DataMember]
        public string pCoApplName { get; set; }
        [DataMember]
        public string pCoApplDOB { get; set; }
        [DataMember]
        public string pCoApplAge { get; set; }
        [DataMember]
        public string pCoApplGender { get; set; }
        [DataMember]
        public string pCoApplMaritalStatus { get; set; }
        [DataMember]
        public string pCoApplRelation { get; set; }
        [DataMember]
        public string pCoApplEduId { get; set; }
        [DataMember]
        public string pCoApplPerAddr { get; set; }
        [DataMember]
        public string pCoApplPerStateId { get; set; }
        [DataMember]
        public string pCoApplPerPIN { get; set; }
        [DataMember]
        public string pCoApplMobileNo { get; set; }
        [DataMember]
        public string pCoApplAddiContactNo { get; set; }
        [DataMember]
        public string pCoApplPhyFitness { get; set; }
        [DataMember]
        public string pACHolderName { get; set; }
        [DataMember]
        public string pFamilyPersonName { get; set; }
        [DataMember]
        public string pRelationId { get; set; }
        [DataMember]
        public string pAccNo { get; set; }
        [DataMember]
        public string pIfscCode { get; set; }
        [DataMember]
        public string pAccType { get; set; }
        [DataMember]
        public string pCoAppIncYN { get; set; }
        [DataMember]
        public string pTypeOfInc { get; set; }
        [DataMember]
        public string pAgeOfKeyIncEar { get; set; }
        [DataMember]
        public string pAnnulInc { get; set; }
        [DataMember]
        public string pHouseStability { get; set; }
        [DataMember]
        public string pTypeOfOwnerShip { get; set; }
        [DataMember]
        public string pTypeOfResi { get; set; }
        [DataMember]
        public string pResiCategory { get; set; }
        [DataMember]
        public string pTotalFamMember { get; set; }
        [DataMember]
        public string pNoOfChild { get; set; }
        [DataMember]
        public string pNoOfDependent { get; set; }
        [DataMember]
        public string pNoOfFamEarMember { get; set; }
        [DataMember]
        public string pFamilyAsset { get; set; }
        [DataMember]
        public string pOtherAsset { get; set; }
        [DataMember]
        public string pLandHolding { get; set; }
        [DataMember]
        public string pBankingHabit { get; set; }
        [DataMember]
        public string pOtherSavings { get; set; }
        [DataMember]
        public string pPersonalRef { get; set; }
        [DataMember]
        public string pAddr { get; set; }
        [DataMember]
        public string pMobileNo { get; set; }
        [DataMember]
        public string pValidatedYN { get; set; }
        [DataMember]
        public string pMobilePhone { get; set; }
        [DataMember]
        public string pRefrigerator { get; set; }
        [DataMember]
        public string pTwoWheeler { get; set; }
        [DataMember]
        public string pThreeWheeler { get; set; }
        [DataMember]
        public string pFourWheeler { get; set; }
        [DataMember]
        public string pAirConditioner { get; set; }
        [DataMember]
        public string pWashingMachine { get; set; }
        [DataMember]
        public string pEmailId { get; set; }
        [DataMember]
        public string pPAN { get; set; }
        [DataMember]
        public string pGSTno { get; set; }
        [DataMember]
        public string pITR { get; set; }
        [DataMember]
        public string pWhatsapp { get; set; }
        [DataMember]
        public string pFacebookAc { get; set; }
        [DataMember]
        public string pBusinessName { get; set; }
        [DataMember]
        public string pBusiNameOnBoard { get; set; }
        [DataMember]
        public string pPrimaryBusiType { get; set; }
        [DataMember]
        public string pPrimaryBusiSeaso { get; set; }
        [DataMember]
        public string pPrimaryBusiSubType { get; set; }
        [DataMember]
        public string pPrimaryBusiActivity { get; set; }
        [DataMember]
        public string pWorkingDays { get; set; }
        [DataMember]
        public string pMonthlyTrunOver { get; set; }
        [DataMember]
        public string pLocalityArea { get; set; }
        [DataMember]
        public string pBusiEstdDt { get; set; }
        [DataMember]
        public string pBusinessAddr { get; set; }
        [DataMember]
        public string pOtherBusinessProof { get; set; }
        [DataMember]
        public string pBusinessVintage { get; set; }
        [DataMember]
        public string pBusiOwnerType { get; set; }
        [DataMember]
        public string pBusiHndlPerson { get; set; }
        [DataMember]
        public string pPartnerYN { get; set; }
        [DataMember]
        public string pNoOfEmp { get; set; }
        [DataMember]
        public string pValueOfStock { get; set; }
        [DataMember]
        public string pValueOfMachinery { get; set; }
        [DataMember]
        public string pBusiHours { get; set; }
        [DataMember]
        public string pAppName { get; set; }
        [DataMember]
        public string pVPAID { get; set; }
        [DataMember]
        public string pBusiTranProofType { get; set; }
        [DataMember]
        public string pCashInHand { get; set; }
        [DataMember]
        public string pBusiRef { get; set; }
        [DataMember]
        public string pBusiAddr { get; set; }
        [DataMember]
        public string pBusiMobileNo { get; set; }
        [DataMember]
        public string pValidateYN { get; set; }
        [DataMember]
        public string pSecondaryBusiYN { get; set; }
        [DataMember]
        public string pNoOfSecBusi { get; set; }
        [DataMember]
        public string pSecBusiType1 { get; set; }
        [DataMember]
        public string pSecBusiSeaso1 { get; set; }
        [DataMember]
        public string pSecBusiSubType1 { get; set; }
        [DataMember]
        public string pSecBusiActivity1 { get; set; }
        [DataMember]
        public string pSecBusiType2 { get; set; }
        [DataMember]
        public string pSecBusiSeaso2 { get; set; }
        [DataMember]
        public string pSecBusiSubType2 { get; set; }
        [DataMember]
        public string pSecBusiActivity2 { get; set; }
        [DataMember]
        public string pBusiIncYN { get; set; }
        [DataMember]
        public string pAppCoAppSameBusiYN { get; set; }
        [DataMember]
        public string pCoAppBusinessName { get; set; }
        [DataMember]
        public string pCoAppPrimaryBusiType { get; set; }
        [DataMember]
        public string pCoAppPrimaryBusiSeaso { get; set; }
        [DataMember]
        public string pCoAppPrimaryBusiSubType { get; set; }
        [DataMember]
        public string pCoAppPrimaryBusiActivity { get; set; }
        [DataMember]
        public string pCoAppMonthlyTrunOver { get; set; }
        [DataMember]
        public string pCoAppBusinessAddr { get; set; }
        [DataMember]
        public string pCoAppOtherBusinessProof { get; set; }
        [DataMember]
        public string pCoAppBusinessVintage { get; set; }
        [DataMember]
        public string pCoAppBusiOwnerType { get; set; }
        [DataMember]
        public string pCoAppValueOfStock { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
        [DataMember]
        public string pEntType { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string pPdDate { get; set; }
        [DataMember]
        public string pBranchCode { get; set; }
        [DataMember]
        public string pBankName { get; set; }
        [DataMember]
        public string pCoAppPerAddrType { get; set; }
        [DataMember]
        public string pCoApplPreAddrType { get; set; }
        [DataMember]
        public string pCoApplPreAddr { get; set; }
        [DataMember]
        public string pCoApplPreStateId { get; set; }
        [DataMember]
        public string pCoApplPrePIN { get; set; }
        [DataMember]
        public string pApplDOB { get; set; }
        [DataMember]
        public string pApplAge { get; set; }
        [DataMember]
        public string pApplHouseImgLat { get; set; }
        [DataMember]
        public string pApplHouseImgLong { get; set; }
        [DataMember]
        public string pApplBusi1ImgLat { get; set; }
        [DataMember]
        public string pApplBusi1ImgLong { get; set; }
        [DataMember]
        public string pApplBusi2ImgLat { get; set; }
        [DataMember]
        public string pApplBusi2ImgLong { get; set; }
        [DataMember]
        public string pApplBusi3ImgLat { get; set; }
        [DataMember]
        public string pApplBusi3ImgLong { get; set; }
        [DataMember]
        public string pApplBusiIdproofImgLat { get; set; }
        [DataMember]
        public string pApplBusiIdproofImgLong { get; set; }
        [DataMember]
        public string pCoApplHouseImgLat { get; set; }
        [DataMember]
        public string pCoApplHouseImgLong { get; set; }
        [DataMember]
        public string pCoApplBusi1ImgLat { get; set; }
        [DataMember]
        public string pCoApplBusi1ImgLong { get; set; }
        [DataMember]
        public string pCoApplBusi2ImgLat { get; set; }
        [DataMember]
        public string pCoApplBusi2ImgLong { get; set; }
        [DataMember]
        public string pUdyamAadhaarRegNo { get; set; }
        [DataMember]
        public string pMinority { get; set; }
        [DataMember]
        public string pIsAbledYN { get; set; }
        [DataMember]
        public string pSpeciallyAbled { get; set; }

    }

    [DataContract]
    public class PostPdByBM
    {
        [DataMember]
        public string pPDId { get; set; }
        [DataMember]
        public string pPreScrQ1 { get; set; }
        [DataMember]
        public string pPreScrQ2 { get; set; }
        [DataMember]
        public string pPreScrQ3 { get; set; }
        [DataMember]
        public string pPreScrQ4 { get; set; }
        [DataMember]
        public string pPreScrQ5 { get; set; }
        [DataMember]
        public string pPreScrQ6 { get; set; }
        [DataMember]
        public string pPreScrQ7 { get; set; }
        [DataMember]
        public string pLoanReqDtlCorrectYN { get; set; }
        [DataMember]
        public string pApplDtlCorrectYN { get; set; }
        [DataMember]
        public string pCoApplDtlcorrectYN { get; set; }
        [DataMember]
        public string pOthrInfocorrectYN { get; set; }
        [DataMember]
        public string pProxyInfoCorrectYN { get; set; }
        [DataMember]
        public string pBankInfoCorrectYN { get; set; }
        [DataMember]
        public string pApplResiRelCorrectYN { get; set; }
        [DataMember]
        public string pApplBusInfoCorrectYN { get; set; }
        [DataMember]
        public string pCoApplBusInfoCorrectYN { get; set; }
        [DataMember]
        public string pBusinessPhotoCorrectYN { get; set; }
        [DataMember]
        public string pPdByBMDate { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string pUpdateRequired { get; set; }

        [DataMember]
        public string pBmSelfieLat { get; set; }
        [DataMember]
        public string pBmSelfieLong { get; set; }
        [DataMember]
        public string pBmSelfie2Lat { get; set; }
        [DataMember]
        public string pBmSelfie2Long { get; set; }
    }

    [DataContract]
    public class PostIFSCData
    {
        [DataMember]
        public string pIFSCCode { get; set; }
    }

    [DataContract]
    public class PostPdByBMData
    {
        [DataMember]
        public string pBranch { get; set; }
    }

    [DataContract]
    public class PostPdDtl
    {
        [DataMember]
        public string pPdId { get; set; }
    }

    [DataContract]
    public class PostMob_ChangePassword
    {
        [DataMember]
        public string pUserName { get; set; }

        [DataMember]
        public string pPassword { get; set; }

        [DataMember]
        public string pOldPassword { get; set; }

        [DataMember]
        public string pEncYN { get; set; }
    }

    [DataContract]
    public class PostBankTransaction
    {
        [DataMember]
        public string AGGRID { get; set; }
        [DataMember]
        public string AGGRNAME { get; set; }
        [DataMember]
        public string CORPID { get; set; }
        [DataMember]
        public string USERID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string DEBITACC { get; set; }
        [DataMember]
        public string CREDITACC { get; set; }
        [DataMember]
        public string IFSC { get; set; }
        [DataMember]
        public string AMOUNT { get; set; }
        [DataMember]
        public string CURRENCY { get; set; }
        [DataMember]
        public string TXNTYPE { get; set; }
        [DataMember]
        public string PAYEENAME { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string REMARKS { get; set; }
    }

    [DataContract]
    public class PostBalEnqReq
    {
        [DataMember]
        public string AGGRID { get; set; }
        [DataMember]
        public string CORPID { get; set; }
        [DataMember]
        public string USERID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string ACCOUNTNO { get; set; }
    }

    [DataContract]
    public class RequestListVO
    {
        [DataMember]
        public string businessUnit { get; set; }
        [DataMember]
        public string subBusinessUnit { get; set; }
        [DataMember]
        public string requestType { get; set; }
        [DataMember]
        public List<RequestVOList> requestVOList { get; set; }
    }

    [DataContract]
    public class RequestVOList
    {
        [DataMember]
        public string aadhar { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string concatAddress { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string customerId { get; set; }
        [DataMember]
        public string digitalID { get; set; }
        [DataMember]
        public string din { get; set; }
        [DataMember]
        public string dob { get; set; }
        [DataMember]
        public string docNumber { get; set; }
        [DataMember]
        public string drivingLicence { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string entityName { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string nationality { get; set; }
        [DataMember]
        public string pan { get; set; }
        [DataMember]
        public string passport { get; set; }
        [DataMember]
        public string phone { get; set; }
        [DataMember]
        public string pincode { get; set; }
        [DataMember]
        public string rationCardNo { get; set; }
        [DataMember]
        public string ssn { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public string tin { get; set; }
        [DataMember]
        public string voterId { get; set; }
    }

    [DataContract]
    public class ListMatchingPayload
    {
        [DataMember]
        public RequestListVO requestListVO { get; set; }
    }

    [DataContract]
    public class RampRequest
    {
        [DataMember]
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    [DataContract]
    public class PostRampRequest
    {
        [DataMember]
        public RampRequest rampRequest { get; set; }
    }

    public class AadhaarXmlOTP
    {
        public string aadhaarNo { get; set; }
        public string consent { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }

    public class AadhaarXmlDownload
    {
        public string otp { get; set; }
        public string aadhaarNo { get; set; }
        public string requestId { get; set; }
        public string consent { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }

    public class eAadhaarOTP
    {
        public string aadhaarNo { get; set; }
        public string consent { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }

    public class eAadhaarDownload
    {
        public string otp { get; set; }
        public string aadhaarNo { get; set; }
        public string accessKey { get; set; }
        public string consent { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }

    #region AadharVault
    [DataContract]
    public class AadhaarVault
    {
        [DataMember]
        public string refData { get; set; }
        [DataMember]
        public string refDataType { get; set; }
        [DataMember]
        public string pMobileNo { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
    }

    [DataContract]
    public class AadhaarNoReq
    {
        [DataMember]
        public string refId { get; set; }
    }
    #endregion

    #region ProsidexRequest
    [DataContract]
    public class ACE
    {
        [DataMember]
        public string ADDRESS { get; set; }
        [DataMember]
        public string ADDRESS_TYPE_FLAG { get; set; }
        [DataMember]
        public string COUNTRY { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string EMAIL { get; set; }
        [DataMember]
        public string EMAIL_TYPE { get; set; }
        [DataMember]
        public string PHONE { get; set; }
        [DataMember]
        public string PHONE_TYPE { get; set; }
        [DataMember]
        public string PINCODE { get; set; }
        [DataMember]
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


    [DataContract]
    public class DG
    {
        [DataMember]
        public string ACCOUNT_NUMBER { get; set; }
        [DataMember]
        public string ALIAS_NAME { get; set; }
        [DataMember]
        public string APPLICATIONID { get; set; }
        [DataMember]
        public string Aadhar { get; set; }
        [DataMember]
        public string CIN { get; set; }
        [DataMember]
        public string CKYC { get; set; }
        [DataMember]
        public string CUSTOMER_STATUS { get; set; }
        [DataMember]
        public string CUST_ID { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public string DrivingLicense { get; set; }
        [DataMember]
        public string Father_First_Name { get; set; }
        [DataMember]
        public string Father_Last_Name { get; set; }
        [DataMember]
        public string Father_Middle_Name { get; set; }
        [DataMember]
        public string Father_Name { get; set; }
        [DataMember]
        public string First_Name { get; set; }
        [DataMember]
        public string Middle_Name { get; set; }
        [DataMember]
        public string Last_Name { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string GSTIN { get; set; }
        [DataMember]
        public string Lead_Id { get; set; }
        [DataMember]
        public string NREGA { get; set; }
        [DataMember]
        public string Pan { get; set; }
        [DataMember]
        public string PassportNo { get; set; }
        [DataMember]
        public string RELATION_TYPE { get; set; }
        [DataMember]
        public string RationCard { get; set; }
        [DataMember]
        public string Registration_NO { get; set; }
        [DataMember]
        public string SALUTATION { get; set; }
        [DataMember]
        public string TAN { get; set; }
        [DataMember]
        public string Udyam_aadhar_number { get; set; }
        [DataMember]
        public string VoterId { get; set; }
        [DataMember]
        public string Tasc_Customer { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember]
        public DG DG { get; set; }
        [DataMember]
        public List<ACE> ACE { get; set; }
        [DataMember]
        public string UnitySfb_RequestId { get; set; }
        [DataMember]
        public string CUST_TYPE { get; set; }
        [DataMember]
        public string CustomerCategory { get; set; }
        [DataMember]
        public string MatchingRuleProfile { get; set; }
        [DataMember]
        public string Req_flag { get; set; }
        [DataMember]
        public string SourceSystem { get; set; }
    }

    [DataContract]
    public class ProsidexRequest
    {
        [DataMember]
        public Request Request { get; set; }
    }

    [DataContract]
    public class ProsiReq
    {
        [DataMember]
        public string pMemberId { get; set; }
        [DataMember]
        public string pPDId { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
    }

    [DataContract]
    public class FingPayRequest
    {
        [DataMember]
        public string beneAccNo { get; set; }
        [DataMember]
        public string beneIFSC { get; set; }
        [DataMember]
        public string MemberId { get; set; }
        [DataMember]
        public string PDId { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
    }

    [DataContract]
    public class ImpsBeneDetailsRequestDataModel
    {
        [DataMember]
        public string beneAccNo { get; set; }
        [DataMember]
        public string beneIFSC { get; set; }
    }
    [DataContract]
    public class BankACReqData
    {
        [DataMember]
        public string superMerchantLoginId { get; set; }
        [DataMember]
        public string superMerchantPin { get; set; }
        [DataMember]
        public string requestId { get; set; }
        [DataMember]
        public List<ImpsBeneDetailsRequestDataModel> impsBeneDetailsRequestDataModel { get; set; }
    }

    #endregion

    #region IDFY
    [DataContract]
    public class PostVoterData
    {
        [DataMember]
        public string VoterId { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEOId { get; set; }
    }

    [DataContract]
    public class VoterData
    {
        [DataMember]
        public string id_number { get; set; }
    }

    [DataContract]
    public class VoterRequest
    {
        [DataMember]
        public string task_id { get; set; }
        [DataMember]
        public string group_id { get; set; }
        [DataMember]
        public VoterData data { get; set; }

        public VoterRequest(string task_id, string group_id, VoterData data)
        {
            this.task_id = task_id;
            this.group_id = group_id;
            this.data = data;
        }
    }

    [DataContract]
    public class Data
    {
        [DataMember]
        public string reference_id { get; set; }
        [DataMember]
        public string key_id { get; set; }
        [DataMember]
        public string ou_id { get; set; }
        [DataMember]
        public string secret { get; set; }
        [DataMember]
        public string callback_url { get; set; }
        [DataMember]
        public string doc_type { get; set; }
        [DataMember]
        public string file_format { get; set; }
        [DataMember]
        public ExtraFields extra_fields { get; set; }

        public Data(string reference_id, string key_id, string ou_id, string secret, string callback_url, string doc_type, string file_format, ExtraFields extra_fields)
        {
            this.reference_id = reference_id;
            this.key_id = key_id;
            this.ou_id = ou_id;
            this.secret = secret;
            this.callback_url = callback_url;
            this.doc_type = doc_type;
            this.file_format = file_format;
            this.extra_fields = extra_fields;
        }
    }

    [DataContract]
    public class ExtraFields
    {
    }
    [DataContract]
    public class FetchDocBody
    {
        [DataMember]
        public string task_id { get; set; }
        [DataMember]
        public string group_id { get; set; }
        [DataMember]
        public Data data { get; set; }

        public FetchDocBody(string task_id, string group_id, Data data)
        {
            this.task_id = task_id;
            this.group_id = group_id;
            this.data = data;
        }
    }

    [DataContract]
    public class PostAadharData
    {
        [DataMember]
        public string AadharNo { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEOId { get; set; }
    }

    public class ParsedDetails
    {
        public string co { get; set; }
        public string country { get; set; }
        public string dist { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string generated_at { get; set; }
        public string house { get; set; }
        public string lang { get; set; }
        public string lm { get; set; }
        public string loc { get; set; }
        public string name { get; set; }
        public string pc { get; set; }
        public string pht { get; set; }
        public string state { get; set; }
        public string street { get; set; }
        public string uid { get; set; }
        public string vtc { get; set; }
    }

    public class IdfyAadharVerifyResponseData
    {
        public string doc_type { get; set; }
        public string download_url { get; set; }
        public string file_format { get; set; }
        public string h { get; set; }
        public ParsedDetails parsed_details { get; set; }
        public string reference_id { get; set; }
        public string status { get; set; }
    }
    #endregion

    #region RiskCatReq
    public class RiskParameter
    {
        public string occupation { get; set; }
        public string custCategory { get; set; }
        public string annualIncomeTurnOver { get; set; }
        public string pep { get; set; }
        public string nationality { get; set; }
        public string countryOfIncorporation { get; set; }
        public string correspondAddrCountry { get; set; }
        public string prodSubType { get; set; }
        public string industry { get; set; }
    }

    public class RiskCatReq
    {
        public string memberId { get; set; }
        public string pdId { get; set; }
        public string requestId { get; set; }
        public string isEntity { get; set; }
        public RiskParameter riskParameter { get; set; }
    }
    #endregion

    public class LoginReqData
    {
        public string pUserId { get; set; }
    }

    [DataContract]
    public class PostVerifyData
    {
        [DataMember]
        public string EnquiryId { get; set; }
        [DataMember]
        public string ApplID1Id { get; set; }
        [DataMember]
        public string ApplID1Response { get; set; }
        [DataMember]
        public string ApplID2Id { get; set; }
        [DataMember]
        public string ApplID2Response { get; set; }
        [DataMember]
        public string CoApplID1Id { get; set; }
        [DataMember]
        public string CoApplID1Response { get; set; }
        [DataMember]
        public string CoApplID2Id { get; set; }
        [DataMember]
        public string CoApplID2Response { get; set; }

    }
    public class userDataReq
    {
        public UserData userData { get; set; }
    }

    [DataContract]
    public class PostIAData
    {
        [DataMember]
        public PostKYCSaveData postKYCSaveData { get; set; }

        [DataMember]
        public PostVerifyData postVerifyData { get; set; }

        [DataMember]
        public PostEMOCRData postEMOCRData { get; set; }
    }

}