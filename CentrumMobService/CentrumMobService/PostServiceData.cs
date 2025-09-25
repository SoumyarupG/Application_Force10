using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CentrumMobService
{
    [DataContract]
    public class AppVersionData
    {
        [DataMember]
        public string pVersion { get; set; }
    }
    [DataContract]
    public class OTPData
    {
        [DataMember]
        public string pMobileNo { get; set; }
        [DataMember]
        public string pOTP { get; set; }
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
    public class KYCVoterRequest
    {
        [DataMember]
        public string consent { get; set; }
        [DataMember]
        public string epic_no { get; set; }
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
    public class OCRData
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
        public string NameMatchingResponseID2 { get; set; }
        [DataMember]
        public string AddressMatchingResponseID2 { get; set; }
        [DataMember]
        public string FaceMatchingResponseID2 { get; set; }

        [DataMember]
        public string CoAppID1AadharFront { get; set; }
        [DataMember]
        public string CoAppID1AadharBack { get; set; }
        [DataMember]
        public string CoAppID1VoterFront { get; set; }
        [DataMember]
        public string CoAppID1VoterBack { get; set; }
        [DataMember]
        public string CoAppID1AadharResponse { get; set; }
        [DataMember]
        public string CoAppID1VoterResponse { get; set; }

        [DataMember]
        public string CoAppNameMatchingResponse { get; set; }
        [DataMember]
        public string CoAppAddressMatchingResponse { get; set; }

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
    public class PostKYCData
    {
        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pType { get; set; }

        [DataMember]
        public string pSearch { get; set; }
    }


    [DataContract]
    public class PostAddressData
    {
        [DataMember]
        public string pBranch { get; set; }
    }

    [DataContract]
    public class PostGroupData
    {
        [DataMember]
        public string pEoID { get; set; }

        [DataMember]
        public string pUserId { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pMktId { get; set; }

        [DataMember]
        public string pGroupName { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pMobId { get; set; }

        [DataMember]
        public string pLeaderName { get; set; }

        [DataMember]
        public string pLeaderContact { get; set; }

        [DataMember]
        public string pVillageId { get; set; }


    }

    [DataContract]
    public class PostCenterData
    {
        [DataMember]
        public string pEoID { get; set; }

        [DataMember]
        public string pUserId { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pCenterName { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pCollDay { get; set; }

        [DataMember]
        public string pCollDayNo { get; set; }

        [DataMember]
        public string pCollType { get; set; }

        [DataMember]
        public string pCollSche { get; set; }

        [DataMember]
        public string pColltime { get; set; }

        [DataMember]
        public string pLatitude { get; set; }

        [DataMember]
        public string pLongitude { get; set; }

        [DataMember]
        public string pLeaderName { get; set; }

        [DataMember]
        public string pLeaderContact { get; set; }

        [DataMember]
        public string pVillageId { get; set; }

        [DataMember]
        public string pGeoAddress { get; set; }

    }

    [DataContract]
    public class PostKYCSaveData
    {
        [DataMember]
        public string pEnquiryId { get; set; }

        [DataMember]
        public string pGroupId { get; set; }

        [DataMember]
        public string pLoanAmount { get; set; }

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
        public string pId3Type { get; set; }

        [DataMember]
        public string pId3No { get; set; }

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
        public string pEmailId2 { get; set; }

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
        public string pISGroupSame { get; set; }

        [DataMember]
        public string pApplicationType { get; set; }

        [DataMember]
        public string pMktId { get; set; }

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
        public string pGender { get; set; }

        [DataMember]
        public string pBussType { get; set; }

        [DataMember]
        public string pOccupation { get; set; }

        [DataMember]
        public string pDeclaredInc { get; set; }

        [DataMember]
        public string pIncFrequency { get; set; }

        [DataMember]
        public string pCoAppFullName { get; set; }

        [DataMember]
        public string pCoAppFName { get; set; }

        [DataMember]
        public string pCoAppMName { get; set; }

        [DataMember]
        public string pCoAppLName { get; set; }

        [DataMember]
        public string pCoAppGender { get; set; }

        [DataMember]
        public string pCoApppBussType { get; set; }

        [DataMember]
        public string pCoAppDeclaredInc { get; set; }

        [DataMember]
        public string pCoAppIncFrequency { get; set; }

        [DataMember]
        public string pCoAppDOB { get; set; }

        [DataMember]
        public string pCoAppAge { get; set; }

        [DataMember]
        public string pCoAppAddress { get; set; }

        [DataMember]
        public string pCoAppPinCode { get; set; }

        [DataMember]
        public string pCoAppState { get; set; }

        [DataMember]
        public string pCoAppIdType { get; set; }

        [DataMember]
        public string pCoAppIdNo { get; set; }

        [DataMember]
        public string pCoAppMobile { get; set; }

        [DataMember]
        public string pCoAppRelation { get; set; }

        [DataMember]
        public string pCoAppOccupation { get; set; }

        [DataMember]
        public string pXmlEarningMemDtl { get; set; }

        [DataMember]
        public string pSelFeLatitude { get; set; }

        [DataMember]
        public string pSelFeLongitude { get; set; }

        [DataMember]
        public string pBussActvId { get; set; }

        [DataMember]
        public string pCoAppBussActvId { get; set; }

        [DataMember]
        public string pApplDigiLockerYN { get; set; }

        [DataMember]
        public string pCoApplDigiLockerYN { get; set; }

        [DataMember]
        public string pCoHouseNo { get; set; }

        [DataMember]
        public string pCoStreet { get; set; }

        [DataMember]
        public string pCoArea { get; set; }

        [DataMember]
        public string pCoVillage { get; set; }

        [DataMember]
        public string pCoSubDistrict { get; set; }

        [DataMember]
        public string pCoDistrict { get; set; }

        [DataMember]
        public string pCoLandmark { get; set; }

        [DataMember]
        public string pCoPostOffice { get; set; }

        [DataMember]
        public string pCoAppStateId { get; set; }

        [DataMember]
        public string pXmlDirector { get; set; }

        [DataMember]
        public string pIsAbledYN { get; set; }

        [DataMember]
        public string pSpeciallyAbled { get; set; }

        [DataMember]
        public string pTempId { get; set; }

        [DataMember]
        public string pFaceMatchRes { get; set; }

        [DataMember]
        public string pFaceMatchScr { get; set; }

    }

    [DataContract]
    public class PostSaveEmployeeAttendance
    {
        [DataMember]
        public string pEmpCode { get; set; }
        [DataMember]
        public string pAttType { get; set; }
        [DataMember]
        public string pLatitute { get; set; }
        [DataMember]
        public string pLongitute { get; set; }
        [DataMember]
        public string pIMEI1 { get; set; }
        [DataMember]
        public string pIMEI2 { get; set; }
        [DataMember]
        public string pAddress { get; set; }
    }

    [DataContract]
    public class PostCBSaveData
    {
        [DataMember]
        public string pEnquiryId { get; set; }

        [DataMember]
        public string pCBId { get; set; }

        [DataMember]
        public string pFullName { get; set; }

        [DataMember]
        public string pDOB { get; set; }

        [DataMember]
        public string pRelativeName { get; set; }

        [DataMember]
        public string pRelationId { get; set; }

        [DataMember]
        public string pPinCode { get; set; }

        [DataMember]
        public string pAddress { get; set; }

        [DataMember]
        public string pStateShortName { get; set; }

        [DataMember]
        public string pIdProofType { get; set; }

        [DataMember]
        public string pIdProofNo { get; set; }

        [DataMember]
        public string pAddressProofType { get; set; }

        [DataMember]
        public string pAddressProofNo { get; set; }

        [DataMember]
        public string pMobileNo1 { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pCreatedBy { get; set; }

        [DataMember]
        public string pTyp { get; set; }
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
    public class PostIFSCData
    {
        [DataMember]
        public string pIFSCCode { get; set; }
    }

    [DataContract]
    public class PostMemberFormData
    {
        [DataMember]
        public string pEnquiryId { get; set; }
    }

    [DataContract]
    public class PostMemberSaveData
    {
        [DataMember]
        public string pEnqId { get; set; }

        [DataMember]
        public string pMemberID { get; set; }

        [DataMember]
        public string pMeetingDay { get; set; }

        [DataMember]
        public string pDistFrmBranch { get; set; }

        [DataMember]
        public string pDistFrmCollCenter { get; set; }

        [DataMember]
        public string pMF_Name { get; set; }

        [DataMember]
        public string pMM_Name { get; set; }

        [DataMember]
        public string pML_Name { get; set; }

        [DataMember]
        public string pM_DOB { get; set; }

        [DataMember]
        public string pM_Age { get; set; }

        [DataMember]
        public string pM_Gender { get; set; }

        [DataMember]
        public string pMM_Status { get; set; }

        [DataMember]
        public string pM_RelgId { get; set; }

        [DataMember]
        public string pM_Caste { get; set; }

        [DataMember]
        public string pM_QualificationId { get; set; }

        [DataMember]
        public string pM_OccupationId { get; set; }

        [DataMember]
        public string pMHF_Name { get; set; }

        [DataMember]
        public string pFather_YN { get; set; }

        [DataMember]
        public string pMHF_Relation { get; set; }

        [DataMember]
        public string pMaidenNmF { get; set; }

        [DataMember]
        public string pMaidenNmM { get; set; }

        [DataMember]
        public string pMaidenNmL { get; set; }

        [DataMember]
        public string pM_AddrType { get; set; }

        [DataMember]
        public string pM_HouseNo { get; set; }

        [DataMember]
        public string pM_Street { get; set; }

        [DataMember]
        public string pM_Landmark { get; set; }

        [DataMember]
        public string pM_Area { get; set; }

        [DataMember]
        public string pM_Village { get; set; }

        [DataMember]
        public string pM_WardNo { get; set; }

        [DataMember]
        public string pM_District { get; set; }

        [DataMember]
        public string pM_State { get; set; }

        [DataMember]
        public string pM_PostOff { get; set; }

        [DataMember]
        public string pM_PIN { get; set; }

        [DataMember]
        public string pMemAddr { get; set; }

        [DataMember]
        public string pM_Mobile { get; set; }

        [DataMember]
        public string pM_Email { get; set; }

        [DataMember]
        public string pM_CommAddrType { get; set; }

        [DataMember]
        public string pM_CommHouseNo { get; set; }

        [DataMember]
        public string pM_CommStreet { get; set; }

        [DataMember]
        public string pM_CommLandmark { get; set; }

        [DataMember]
        public string pM_CommArea { get; set; }

        [DataMember]
        public string pM_CommVillageId { get; set; }

        [DataMember]
        public string pM_CommWardNo { get; set; }

        [DataMember]
        public string pM_CommPostOff { get; set; }

        [DataMember]
        public string pM_CommPIN { get; set; }

        [DataMember]
        public string pM_CommMobile { get; set; }

        [DataMember]
        public string pM_CommEmail { get; set; }

        [DataMember]
        public string pM_AreaCatagory { get; set; }

        [DataMember]
        public string pM_YearsOfStay { get; set; }

        [DataMember]
        public string pM_IdentyPRofId { get; set; }

        [DataMember]
        public string pM_IdentyProfNo { get; set; }

        [DataMember]
        public string pM_AddProfId { get; set; }

        [DataMember]
        public string pM_AddProfNo { get; set; }

        [DataMember]
        public string pM_AddProfId2 { get; set; }

        [DataMember]
        public string pM_AddProfNo2 { get; set; }

        [DataMember]
        public string pMemNamePBook { get; set; }

        [DataMember]
        public string pAccNo { get; set; }

        [DataMember]
        public string pIFSC { get; set; }

        [DataMember]
        public string pAccType { get; set; }

        [DataMember]
        public string pB_FName { get; set; }

        [DataMember]
        public string pB_MName { get; set; }

        [DataMember]
        public string pB_LName { get; set; }

        [DataMember]
        public string pB_DOB { get; set; }

        [DataMember]
        public string pB_Age { get; set; }

        [DataMember]
        public string pB_Gender { get; set; }

        [DataMember]
        public string pB_HumanRelationId { get; set; }

        [DataMember]
        public string pB_RelgId { get; set; }

        [DataMember]
        public string pB_Caste { get; set; }

        [DataMember]
        public string pB_QualificationId { get; set; }

        [DataMember]
        public string pB_OccupationId { get; set; }

        [DataMember]
        public string pB_HouseNo { get; set; }

        [DataMember]
        public string pB_Street { get; set; }

        [DataMember]
        public string pB_Landmark { get; set; }

        [DataMember]
        public string pB_Area { get; set; }

        [DataMember]
        public string pB_VillageID { get; set; }

        [DataMember]
        public string pB_WardNo { get; set; }

        [DataMember]
        public string pB_PostOff { get; set; }

        [DataMember]
        public string pB_PIN { get; set; }

        [DataMember]
        public string pCoBrwrAddr { get; set; }

        [DataMember]
        public string pB_Mobile { get; set; }

        [DataMember]
        public string pB_Email { get; set; }

        [DataMember]
        public string pB_IdentyProfId { get; set; }

        [DataMember]
        public string pB_IdentyProfNo { get; set; }

        [DataMember]
        public string pB_AddProfId { get; set; }

        [DataMember]
        public string pB_AddProfNo { get; set; }

        [DataMember]
        public string pGuarFName { get; set; }

        [DataMember]
        public string pGuarLName { get; set; }

        [DataMember]
        public string pGuarDOB { get; set; }

        [DataMember]
        public string pGuarAge { get; set; }

        [DataMember]
        public string pGuarGen { get; set; }

        [DataMember]
        public string pGuarRel { get; set; }

        [DataMember]
        public string pNoOfDpndnt { get; set; }

        [DataMember]
        public string pNoOfHouseMem { get; set; }

        [DataMember]
        public string pNoOfChildren { get; set; }

        [DataMember]
        public string pFamilyIncome { get; set; }

        [DataMember]
        public string pSelfIncome { get; set; }

        [DataMember]
        public string pOtherIncome { get; set; }

        [DataMember]
        public string pTotIncome { get; set; }

        [DataMember]
        public string ExHsRntAmt { get; set; }

        [DataMember]
        public string pFdAmt { get; set; }

        [DataMember]
        public string ExEduAmt { get; set; }

        [DataMember]
        public string pExMedAmt { get; set; }

        [DataMember]
        public string pExLnInsAmt { get; set; }

        [DataMember]
        public string pExpFuelAmt { get; set; }

        [DataMember]
        public string pExpElectricAmt { get; set; }

        [DataMember]
        public string pExpTransAmt { get; set; }

        [DataMember]
        public string pExpOtherAmt { get; set; }

        [DataMember]
        public string pTotExp { get; set; }

        [DataMember]
        public string pSurplus { get; set; }

        [DataMember]
        public string pXmlAsset { get; set; }

        [DataMember]
        public string pBranchcode { get; set; }

        [DataMember]
        public string pAdmDate { get; set; }

        [DataMember]
        public string pLogDt { get; set; }

        [DataMember]
        public string pCreatedBy { get; set; }

        [DataMember]
        public string pMarketId { get; set; }

        [DataMember]
        public string pGroupId { get; set; }

        [DataMember]
        public string pMemBusTypeId { get; set; }
        [DataMember]
        public string pMemEMailId { get; set; }
        [DataMember]
        public string pDeclIncome { get; set; }
        [DataMember]
        public string pIncFrequency { get; set; }
        [DataMember]
        public string pCoAppMaritalStat { get; set; }
        [DataMember]
        public string pCoAppBusTypeId { get; set; }
        [DataMember]
        public string pCoAppDeclIncome { get; set; }
        [DataMember]
        public string pCoAppIncFrequency { get; set; }
        [DataMember]
        public string pOtherIncomeSrcId { get; set; }
        [DataMember]
        public string pXmlEarningMemDtl { get; set; }
        [DataMember]
        public string pMemBusActvId { get; set; }
        [DataMember]
        public string pCoAppBusActvId { get; set; }
        [DataMember]
        public string pMinority { get; set; }

        [DataMember]
        public string pB_Village { get; set; }
        [DataMember]
        public string pB_District { get; set; }
        [DataMember]
        public string pB_StateId { get; set; }

        [DataMember]
        public string pIsAbledYN { get; set; }
        [DataMember]
        public string pSpeciallyAbled { get; set; }

    }

    [DataContract]
    public class PostHVData
    {
        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostHVDataSave
    {
        [DataMember]
        public string pCGTId { get; set; }
        [DataMember]
        public string pMemberID { get; set; }
        [DataMember]
        public string pHVDt { get; set; }
        [DataMember]
        public string pHVBy { get; set; }
        [DataMember]
        public string pExpGrtDt { get; set; }
        [DataMember]
        public string pQ1 { get; set; }
        [DataMember]
        public string pQ1Score { get; set; }
        [DataMember]
        public string pQ2 { get; set; }
        [DataMember]
        public string pQ2Score { get; set; }
        [DataMember]
        public string pQ3 { get; set; }
        [DataMember]
        public string pQ3Score { get; set; }
        [DataMember]
        public string pQ4 { get; set; }
        [DataMember]
        public string pQ4Score { get; set; }
        [DataMember]
        public string pQ5 { get; set; }
        [DataMember]
        public string pQ5Score { get; set; }
        [DataMember]
        public string pQ6 { get; set; }
        [DataMember]
        public string pQ6Score { get; set; }
        [DataMember]
        public string pQ7 { get; set; }
        [DataMember]
        public string pQ7Score { get; set; }
        [DataMember]
        public string pQ8 { get; set; }
        [DataMember]
        public string pQ8Score { get; set; }
        [DataMember]
        public string pQ9 { get; set; }
        [DataMember]
        public string pQ9Score { get; set; }
        [DataMember]
        public string pQ10 { get; set; }
        [DataMember]
        public string pQ10Score { get; set; }
        [DataMember]
        public string pQ11 { get; set; }
        [DataMember]
        public string pQ11Score { get; set; }
        [DataMember]
        public string pQ12 { get; set; }
        [DataMember]
        public string pQ12Score { get; set; }
        [DataMember]
        public string pQ13 { get; set; }
        [DataMember]
        public string pQ13Score { get; set; }
        [DataMember]
        public string pBranchCode { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
        [DataMember]
        public string pQ14 { get; set; }
        [DataMember]
        public string pQ14Score { get; set; }
        [DataMember]
        public string pQ15 { get; set; }
        [DataMember]
        public string pQ15Score { get; set; }
        [DataMember]
        public string pQ16 { get; set; }
        [DataMember]
        public string pQ16Score { get; set; }
        [DataMember]
        public string pQ15ElectricYN { get; set; }
        [DataMember]
        public string pQ15WaterYN { get; set; }
        [DataMember]
        public string pQ15ToiletYN { get; set; }
        [DataMember]
        public string pQ15SewageYN { get; set; }
        [DataMember]
        public string pQ15LPGYN { get; set; }
        [DataMember]
        public string pQ16LandYN { get; set; }
        [DataMember]
        public string pQ16VehicleYN { get; set; }
        [DataMember]
        public string pQ16FurnitureYN { get; set; }
        [DataMember]
        public string pQ16SmartPhoneYN { get; set; }
        [DataMember]
        public string pQ16ElectricItemYN { get; set; }
        [DataMember]
        public string pQ14SubCat { get; set; }
    }

    [DataContract]
    public class PostHVDataSaveNew
    {
        [DataMember]
        public string pCGTId { get; set; }
        [DataMember]
        public string pMemberID { get; set; }
        [DataMember]
        public string pHVDt { get; set; }
        [DataMember]
        public string pHVBy { get; set; }
        [DataMember]
        public string pExpGrtDt { get; set; }
        [DataMember]
        public string pQ1 { get; set; }
        [DataMember]
        public string pQ1Score { get; set; }
        [DataMember]
        public string pQ1Weighted { get; set; }

        [DataMember]
        public string pQ2 { get; set; }
        [DataMember]
        public string pQ2Score { get; set; }
        [DataMember]
        public string pQ2Weighted { get; set; }

        [DataMember]
        public string pQ3 { get; set; }
        [DataMember]
        public string pQ3Score { get; set; }
        [DataMember]
        public string pQ3Weighted { get; set; }

        [DataMember]
        public string pQ4 { get; set; }
        [DataMember]
        public string pQ4Score { get; set; }
        [DataMember]
        public string pQ4Weighted { get; set; }

        [DataMember]
        public string pQ5 { get; set; }
        [DataMember]
        public string pQ5Score { get; set; }
        [DataMember]
        public string pQ5Weighted { get; set; }

        [DataMember]
        public string pQ6 { get; set; }
        [DataMember]
        public string pQ6Score { get; set; }
        [DataMember]
        public string pQ6Weighted { get; set; }

        [DataMember]
        public string pQ7 { get; set; }
        [DataMember]
        public string pQ7Score { get; set; }
        [DataMember]
        public string pQ7Weighted { get; set; }

        [DataMember]
        public string pQ8 { get; set; }
        [DataMember]
        public string pQ8Score { get; set; }
        [DataMember]
        public string pQ8Weighted { get; set; }

        [DataMember]
        public string pQ9 { get; set; }
        [DataMember]
        public string pQ9Score { get; set; }
        [DataMember]
        public string pQ9Weighted { get; set; }

        [DataMember]
        public string pQ10 { get; set; }
        [DataMember]
        public string pQ10Score { get; set; }
        [DataMember]
        public string pQ10Weighted { get; set; }

        [DataMember]
        public string pQ11 { get; set; }
        [DataMember]
        public string pQ11Score { get; set; }
        [DataMember]
        public string pQ11Weighted { get; set; }

        [DataMember]
        public string pQ12 { get; set; }
        [DataMember]
        public string pQ12Score { get; set; }
        [DataMember]
        public string pQ12Weighted { get; set; }

        [DataMember]
        public string pQ13 { get; set; }
        [DataMember]
        public string pQ13Score { get; set; }
        [DataMember]
        public string pQ13Weighted { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
        [DataMember]
        public string pQ14 { get; set; }
        [DataMember]
        public string pQ14Score { get; set; }
        [DataMember]
        public string pQ14Weighted { get; set; }

        [DataMember]
        public string pQ15 { get; set; }
        [DataMember]
        public string pQ15Score { get; set; }
        [DataMember]
        public string pQ15Weighted { get; set; }

        [DataMember]
        public string pLatitude { get; set; }
        [DataMember]
        public string pLongitude { get; set; }

    }

    [DataContract]
    public class PostSchemeData
    {
        [DataMember]
        public string pBranch { get; set; }
    }

    [DataContract]
    public class PostLoanApp
    {
        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostLoanAppDtl
    {
        [DataMember]
        public string pMemberId { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostLoanAppDtlByMember
    {
        [DataMember]
        public string pMemberId { get; set; }

        [DataMember]
        public string pLoanAppId { get; set; }

    }

    [DataContract]
    public class SaveGRTData
    {
        [DataMember]
        public string pAppDate { get; set; }

        [DataMember]
        public string pMemberId { get; set; }

        [DataMember]
        public string ploantypeid { get; set; }

        [DataMember]
        public string pLoanAppAmt { get; set; }

        [DataMember]
        public string pPurposeId { get; set; }

        [DataMember]
        public string pSubPurposeId { get; set; }

        [DataMember]
        public string pLoanCycle { get; set; }

        [DataMember]
        public string pCGTID { get; set; }

        [DataMember]
        public string pBrCode { get; set; }

        [DataMember]
        public string pCreatedBy { get; set; }

        [DataMember]
        public string pNomName { get; set; }

        [DataMember]
        public string pNomAge { get; set; }

        [DataMember]
        public string pNomRel { get; set; }

        [DataMember]
        public string pEoid { get; set; }

        [DataMember]
        public string pAppStatus { get; set; }

        [DataMember]
        public string pRejectReason { get; set; }

        [DataMember]
        public string pEligibleAmt { get; set; }

        [DataMember]
        public string pRefNo { get; set; }

        [DataMember]
        public string pSurplus { get; set; }

        [DataMember]
        public string pEligibleEMI { get; set; }

        [DataMember]
        public string pEMIEligibleAmt { get; set; }

        [DataMember]
        public string pEligibleLoanAmt { get; set; }

        [DataMember]
        public string pEligibleLoanAmtText { get; set; }

        [DataMember]
        public string pLatitude { get; set; }

        [DataMember]
        public string pLongitude { get; set; }

    }

    [DataContract]
    public class PostDashboardData
    {
        [DataMember]
        public string pUserId { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pEoId { get; set; }
    }

    [DataContract]
    public class PostReHVData
    {
        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string peoId { get; set; }
    }

    [DataContract]
    public class SaveReHVData
    {
        [DataMember]
        public string pXml { get; set; }

        [DataMember]
        public string pCreatedBy { get; set; }

        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostCollectionData
    {
        [DataMember]
        public string pEoID { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pRoutin { get; set; }

        [DataMember]
        public string pIsHoliday { get; set; }

        [DataMember]
        public string pFromDate { get; set; }

        [DataMember]
        public string pToDate { get; set; }

        [DataMember]
        public string pPTPYN { get; set; }
    }

    [DataContract]
    public class SaveCollectionData
    {
        [DataMember]
        public string pXml { get; set; }

        [DataMember]
        public string pUserId { get; set; }

        [DataMember]
        public string pEoId { get; set; }

        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pValueDate { get; set; }

        [DataMember]
        public string pReceiptNo { get; set; }
    }

    [DataContract]
    public class ReportData
    {
        [DataMember]
        public string pEnquiryId { get; set; }

        [DataMember]
        public string pCbId { get; set; }

        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostImage
    {
        [DataMember]
        public byte[] imageBinary { get; set; }

        [DataMember]
        public string imageGroup { get; set; }

        [DataMember]
        public string folderName { get; set; }

        [DataMember]
        public string imageName { get; set; }

    }

    [DataContract]
    public class PostCenterList
    {
        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pEoId { get; set; }

    }
    [DataContract]
    public class PostLoanUtilizationData
    {
        [DataMember]
        public string pBranch { get; set; }

        [DataMember]
        public string pDate { get; set; }

        [DataMember]
        public string pGroupId { get; set; }
    }

    [DataContract]
    public class PostUpdateLoanUtilCheck
    {
        [DataMember]
        public string pLoanId { get; set; }

        [DataMember]
        public string pLoanUTLType { get; set; }

        [DataMember]
        public string pLoanUTLBy { get; set; }

        [DataMember]
        public string pLoanUTLRemarks { get; set; }

        [DataMember]
        public string pLoanUTLVia { get; set; }

        [DataMember]
        public string pLoanUTLAmt { get; set; }

        [DataMember]
        public string pLoanUTLDt { get; set; }

        [DataMember]
        public string IsSamePurpose { get; set; }

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
        [DataMember]
        public string pKey { get; set; }
    }

    [DataContract]
    public class Post_ForgotPassword
    {
        [DataMember]
        public string pUserName { get; set; }
        [DataMember]
        public string pPassword { get; set; }
        [DataMember]
        public string pOTPId { get; set; }
        [DataMember]
        public string pOTP { get; set; }
        [DataMember]
        public string pKey { get; set; }
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
    public class PostBankTransactionStatus
    {
        [DataMember]
        public string AGGRID { get; set; }
        [DataMember]
        public string CORPID { get; set; }
        [DataMember]
        public string USERID { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string URN { get; set; }
    }

    [DataContract]
    public class PostActivityData
    {
        [DataMember]
        public string Eoid { get; set; }
        [DataMember]
        public string ActivityID { get; set; }
        [DataMember]
        public string ActivityDate { get; set; }
        [DataMember]
        public string ActivityTime { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Lattitude { get; set; }
        [DataMember]
        public string IMEINo { get; set; }
        [DataMember]
        public string LocationAddress { get; set; }
        [DataMember]
        public string MemberID { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string BranchCode { get; set; }
        [DataMember]
        public string GroupID { get; set; }
        [DataMember]
        public string CenterID { get; set; }
        [DataMember]
        public string ActivityType { get; set; }
    }


    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }

    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    public class RequestListVO
    {
        public string businessUnit { get; set; }
        public string subBusinessUnit { get; set; }
        public string requestType { get; set; }
        public List<RequestVOList> requestVOList { get; set; }
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

    public class PostRampRequest
    {
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

    [DataContract]
    public class RampStatusRequest
    {
        [DataMember]
        public string buCode { get; set; }
        [DataMember]
        public string subBuCode { get; set; }
        [DataMember]
        public string screeningId { get; set; }
        [DataMember]
        public string screeningStatus { get; set; }
        [DataMember]
        public string remarks { get; set; }
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
        public string pCGTId { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
    }
    [DataContract]
    public class GetResponseReq
    {
        [DataMember]
        public string pRequestId { get; set; }
        [DataMember]
        public string pMemberId { get; set; }
        [DataMember]
        public string pCGTId { get; set; }
    }

    [DataContract]
    public class Metadata
    {
        [DataMember]
        public string UnitySfb_RequestId { get; set; }
        [DataMember]
        public string CUSTOMER_ID { get; set; }
        [DataMember]
        public string SourceSystem { get; set; }
        [DataMember]
        public string UCIC { get; set; }
    }

    [DataContract]
    public class PosidexVerificationData
    {
        [DataMember]
        public List<Metadata> Metadata { get; set; }
    }


    #endregion

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
        public string CGTId { get; set; }
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

    [DataContract]
    public class PostSaveMonitoring
    {
        [DataMember]
        public string pInspType { get; set; }
        [DataMember]
        public string pSubDt { get; set; }
        [DataMember]
        public string pCrdFrmDt { get; set; }
        [DataMember]
        public string pCrdToDt { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pLatitude { get; set; }
        [DataMember]
        public string pLongitude { get; set; }
        [DataMember]
        public string pGeoAddress { get; set; }
        [DataMember]
        public string pXmlData { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
        [DataMember]
        public string pMode { get; set; }
        [DataMember]
        public string pVisitType { get; set; }
        [DataMember]
        public string pEOid { get; set; }
        [DataMember]
        public string pCenterid { get; set; }
        [DataMember]
        public string pMemberId { get; set; }
    }



    [DataContract]
    public class PostMonitoringCompliance
    {
        [DataMember]
        public string pBranchCode { get; set; }
        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class PostSaveMonitoringCompliance
    {
        [DataMember]
        public string pInspID { get; set; }
        [DataMember]
        public string pCmplDt { get; set; }
        [DataMember]
        public string pXmlData { get; set; }
        [DataMember]
        public string pCreatedBy { get; set; }
    }

    [DataContract]
    public class PostIncentiveLoWise
    {
        [DataMember]
        public string pDtTo { get; set; }
        [DataMember]
        public string pBranch { get; set; }
        [DataMember]
        public string pEOId { get; set; }
    }

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

    public class Config
    {
        public string id { get; set; }
    }

    public class ProfileData
    {
    }

    public class ProfileRequest
    {
        public string reference_id { get; set; }
        public Config config { get; set; }
        public ProfileData data { get; set; }
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
        public string cgtId { get; set; }
        public string requestId { get; set; }
        public string isEntity { get; set; }
        public RiskParameter riskParameter { get; set; }
    }
    #endregion

    #region IdfyAadharVerifyData
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

    #region Proposal Request
    public class AdditionalFields
    {
        public string dummyField1 { get; set; }
        public string dummyField2 { get; set; }
    }

    public class ApplicantDetails
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pinCode { get; set; }
        public string occupation { get; set; }
        public string govtDocumentType { get; set; }
        public string documentNumber { get; set; }
        public string isApplicantInGoodHealth { get; set; }
    }

    public class LoanDetails
    {
        public string loanAccountNumber { get; set; }
        public string loanAmount { get; set; }
        public string interestRate { get; set; }
        public string loanTenure { get; set; }
        public string premiumFunding { get; set; }
        public string loanSanctionDate { get; set; }
        public string loanDisbursementDate { get; set; }
        public string policyTerm { get; set; }
    }

    public class NomineeDetail
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string relationship { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string nomineeAddressLine1 { get; set; }
        public string nomineeAddressLine2 { get; set; }
        public string nomineeCity { get; set; }
        public string nomineeState { get; set; }
        public string nomineePincode { get; set; }
        public string percentageShare { get; set; }

        public NomineeDetail(string firstName, string lastName, string relationship, string dateOfBirth, string gender, string mobile, string nomineeAddressLine1,
            string nomineeAddressLine2, string nomineeCity, string nomineeState, string nomineePincode, string percentageShare)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.relationship = relationship;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
            this.mobile = mobile;
            this.nomineeAddressLine1 = nomineeAddressLine1;
            this.nomineeAddressLine2 = nomineeAddressLine2;
            this.nomineeCity = nomineeCity;
            this.nomineeState = nomineeState;
            this.nomineePincode = nomineePincode;
            this.percentageShare = percentageShare;
        }
    }

    public class PlanSpecifics
    {
        public LoanDetails loanDetails { get; set; }
        public string sumAssured { get; set; }
        public string benefitOptions { get; set; }
    }

    public class ProposalRequest
    {
        public string partnerCode { get; set; }
        public string productCode { get; set; }
        public string externalReferenceNumber { get; set; }
        public ApplicantDetails applicantDetails { get; set; }
        public PlanSpecifics planSpecifics { get; set; }
        public List<NomineeDetail> nomineeDetails { get; set; }
        public AdditionalFields additionalFields { get; set; }
    }

    #endregion
    #region Quote Request
    public class QuoteApplicantDetails
    {
        public string dateOfBirth { get; set; }
    }

    public class CreditLifeDetails
    {
        public int loanTenure { get; set; }
        public int policyTenure { get; set; }
        public double loanAmount { get; set; }
        public double sumAssured { get; set; }
        public int numberOfLives { get; set; }
    }

    public class QuoteData
    {
        public CreditLifeDetails creditLifeDetails { get; set; }
        public QuoteApplicantDetails applicantDetails { get; set; }
    }

    public class QuoteRequest
    {
        public string productCode { get; set; }
        public QuoteData data { get; set; }
        public AdditionalFields additionalFields { get; set; }
    }

    #endregion

    #region Proposal Request

    public class ProApplicantDetails
    {
        public string referenceId { get; set; }
        public string country { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string education { get; set; }
        public string maritalStatus { get; set; }
        //public string email { get; set; }
        public string mobile { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string pincode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string occupation { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string isApplicantInGoodHealth { get; set; }
        public string relationshipWithBorrower { get; set; }
    }

    public class ProCreditLifeDetails
    {
        public string loanAccountNumber { get; set; }
        public double loanAmount { get; set; }
        public int loanTenure { get; set; }
        public int policyTenure { get; set; }
        public string loanDisbursementDate { get; set; }
        public double sumAssured { get; set; }
        public string premiumFunding { get; set; }
        public string loanSanctionDate { get; set; }
        public string medicalQuestions { get; set; }
        public string coverType { get; set; }
        public int numberOfLives { get; set; }
    }

    public class ProData
    {
        public ProCreditLifeDetails creditLifeDetails { get; set; }
        public List<ProInsuredDetail> insuredDetails { get; set; }
        public AdditionalFields additionalFields { get; set; }
    }

    public class ProInsuredDetail
    {
        public string customerType { get; set; }
        public string externalReferenceNumber { get; set; }
        public ProApplicantDetails applicantDetails { get; set; }
        public List<ProNomineeDetail> nomineeDetails { get; set; }
        public ProInsuredDetail(string customerType, string externalReferenceNumber, ProApplicantDetails applicantDetails, List<ProNomineeDetail> nomineeDetails)
        {
            this.customerType = customerType;
            this.externalReferenceNumber = externalReferenceNumber;
            this.applicantDetails = applicantDetails;
            this.nomineeDetails = nomineeDetails;
        }
    }

    public class ProNomineeDetail
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string relationshipWithApplicant { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string mobile { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string pincode { get; set; }
        public string percentageShare { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string occupation { get; set; }
        public ProNomineeDetail(string firstName, string lastName, string relationshipWithApplicant, string dateOfBirth,
            string gender, string mobile, string addressLine1, string addressLine2, string pincode, string percentageShare,
            string city, string state, string occupation)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.relationshipWithApplicant = relationshipWithApplicant;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
            this.mobile = mobile;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.pincode = pincode;
            this.percentageShare = percentageShare;
            this.city = city;
            this.state = state;
            this.occupation = occupation;
        }

    }

    public class ProRequestData
    {
        public string productCode { get; set; }
        public string transactionId { get; set; }
        public ProData data { get; set; }
    }
    #endregion


    public class LoginReqData
    {
        public string pUserId { get; set; }
    }

    [DataContract]
    public class SaveTeleCallingData
    {
        [DataMember]
        public string pXml { get; set; }

        [DataMember]
        public string pUserId { get; set; }

    }

    [DataContract]
    public class ForgotOTPData
    {
        [DataMember]
        public string pUserName { get; set; }
    }

    [DataContract]
    public class ValidateOTPReqData
    {
        [DataMember]
        public string pOTP { get; set; }
        [DataMember]
        public string pOTPId { get; set; }
    }

    #region DigitalSign Request Class
    //public class Esigner
    //{
    //    public string esigner_name { get; set; }
    //    public string esigner_pincode { get; set; }
    //    public string esigner_state { get; set; }
    //    public string esigner_title { get; set; }
    //}

    //public class FileDetails
    //{
    //    public string audit_file { get; set; }
    //    public List<string> esign_file { get; set; }
    //}

    //public class RequestDetail
    //{
    //    public string esign_type { get; set; }
    //    public string esign_url { get; set; }
    //    public string esigner_email { get; set; }
    //    public string esigner_name { get; set; }
    //    public string esigner_phone { get; set; }
    //    public object expiry_date { get; set; }
    //    public bool is_active { get; set; }
    //    public bool is_expired { get; set; }
    //    public bool is_rejected { get; set; }
    //    public bool is_signed { get; set; }
    //}

    //public class IDFYResult
    //{
    //    public SourceOutput source_output { get; set; }
    //}

    //public class IdfyeSignResponse
    //{
    //    public string action { get; set; }
    //    public string completed_at { get; set; }
    //    public string created_at { get; set; }
    //    public string group_id { get; set; }
    //    public string request_id { get; set; }
    //    public IDFYResult result { get; set; }
    //    public string status { get; set; }
    //    public string task_id { get; set; }
    //    public string type { get; set; }
    //}

    //public class SourceOutput
    //{
    //    public string esign_doc_id { get; set; }
    //    public object esign_folder { get; set; }
    //    public object esign_irn { get; set; }
    //    public List<Esigner> esigners { get; set; }
    //    public FileDetails file_details { get; set; }
    //    public List<RequestDetail> request_details { get; set; }
    //    public string status { get; set; }
    //}

    public class IdfySignRequest
    {
        public string action { get; set; }
        public bool active { get; set; }
        public string email { get; set; }
        public object error { get; set; }
        public bool expired { get; set; }
        public string expiryDate { get; set; }
        public string invitationUrl { get; set; }
        public string inviteeType { get; set; }
        public string name { get; set; }
        public object phone { get; set; }
        public object rejectionMessage { get; set; }
        public string signType { get; set; }
    }

    public class IdfySignReq
    {
        public IdfySignRequest request { get; set; }
        public object irn { get; set; }
        public List<object> messages { get; set; }
        public string documentId { get; set; }
        public string documentStatus { get; set; }
        public string mac { get; set; }
        public IdfySignVerification verification { get; set; }
        public string webhookType { get; set; }
    }

    public class IdfySignVerification
    {
        public object gender { get; set; }
        public object name { get; set; }
        public object pincode { get; set; }
        public object smartName { get; set; }
        public object state { get; set; }
        public object title { get; set; }
        public object yob { get; set; }
    }



    public class IdfyDocData
    {
        public string user_key { get; set; }
        public string esign_doc_id { get; set; }
    }

    public class IdfyDocRequest
    {
        public string task_id { get; set; }
        public string group_id { get; set; }
        public IdfyDocData data { get; set; }
    }

    #endregion


    [DataContract]
    public class CollQRReqData
    {
        [DataMember]
        public string pLoanId { get; set; }
        [DataMember]
        public string pNoOfInst { get; set; }
        [DataMember]
        public string pAmt { get; set; }
        [DataMember]
        public string pDate { get; set; }
    }

    [DataContract]
    public class DynamicQRReqData
    {
        [DataMember]
        public string merchantId { get; set; }
        [DataMember]
        public string terminalId { get; set; }
        [DataMember]
        public string amount { get; set; }
        [DataMember]
        public string merchantTranId { get; set; }
        [DataMember]
        public string ICICIMarchantVPA { get; set; }
        [DataMember]
        public string ICICIMarchantName { get; set; }
        [DataMember]
        public string UniqueNo { get; set; }
        [DataMember]
        public string ICICIMccCode { get; set; }
    }

    public class eIBMAadhaarOTP
    {
        public string aadhaarNo { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }

    public class IBMAadhaarOTPReq
    {
        public string sourceId { get; set; }
        public string traceId { get; set; }
        public string uid { get; set; }
        public string txn { get; set; }
        public string ts { get; set; }
        public string type { get; set; }
        public string ch { get; set; }

    }

    public class eIBMAadhaar
    {
        public string aadhaarNo { get; set; }
        public string txn { get; set; }
        public string otp { get; set; }
        public string pBranch { get; set; }
        public string pEoID { get; set; }
    }


    public class eIBMAadhaarReq
    {
        public string sourceId { get; set; }
        public string traceId { get; set; }
        public string uid { get; set; }
        public string txn { get; set; }
        public string type { get; set; }
        public string ts { get; set; }
        public string otp { get; set; }
        public Kyc Kyc { get; set; }
    }

    public class Kyc
    {
        public string lr { get; set; }
        public string pfr { get; set; }
    }

    public class PostPaymentStatus
    {
        public string merchantId { get; set; }
        public string subMerchantId { get; set; }
        public string terminalId { get; set; }
        public string BankRRN { get; set; }
        public string merchantTranId { get; set; }
        public string PayerName { get; set; }
        public string PayerMobile { get; set; }
        public string PayerVA { get; set; }
        public string PayerAmount { get; set; }
        public string TxnStatus { get; set; }
        public string TxnInitDate { get; set; }
        public string TxnCompletionDate { get; set; }
        public string ResponseCode { get; set; }
        public string RespCodeDescription { get; set; }
        public string PayeeVPA { get; set; }
        public string PayerAccountType { get; set; }
    }

    [DataContract]
    public class PostICICIPaymentStat
    {
        [DataMember]
        public string BillerBillID { get; set; }
    }

    public class KycRes
    {
        public string code { get; set; }
        public string ret { get; set; }
        public string ts { get; set; }
        public string ttl { get; set; }
        public string txn { get; set; }
        public UidData UidData { get; set; }
    }

    public class LData
    {
        public int lang { get; set; }
        public string name { get; set; }
        public string co { get; set; }
        public string house { get; set; }
        public string street { get; set; }
        public string lm { get; set; }
        public string loc { get; set; }
        public string vtc { get; set; }
        public string subdist { get; set; }
        public string dist { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pc { get; set; }
        public string po { get; set; }
    }

    public class Pht
    {
        public string base64image { get; set; }
    }

    public class Poa
    {
        public string co { get; set; }
        public string house { get; set; }
        public string street { get; set; }
        public string lm { get; set; }
        public string loc { get; set; }
        public string vtc { get; set; }
        public string subdist { get; set; }
        public string dist { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string pc { get; set; }
        public string po { get; set; }
    }

    public class Poi
    {
        public string dob { get; set; }
        public string gender { get; set; }
        public string name { get; set; }
    }

    public class Prn
    {
        public string type { get; set; }
        public string base64pdf { get; set; }
    }

    public class IBMAadhaarFullResponse
    {
        public string status { get; set; }
        public string err { get; set; }
        public string StatusMsg { get; set; }
        public KycRes KycRes { get; set; }
        public string NSDL_Error_Code { get; set; }
        public string StatusCode { get; set; }
        public string ErrorReason { get; set; }
        public string FailedAt { get; set; }
        public string ErrorName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class UidData
    {
        public string tkn { get; set; }
        public string uid { get; set; }
        public Poi Poi { get; set; }
        public Poa Poa { get; set; }
        public LData LData { get; set; }
        public Pht Pht { get; set; }
        public Prn Prn { get; set; }
    }

    [DataContract]
    public class PostLUCPendingDataList
    {
        [DataMember]
        public string pEoId { get; set; }
        [DataMember]
        public string pDate { get; set; }
        [DataMember]
        public string pBranch { get; set; }
    }


    [DataContract]
    public class PostLoanUtilization
    {
        [DataMember]
        public string pLoanId { get; set; }
        [DataMember]
        public string pLoanUtlType { get; set; }
        [DataMember]
        public string pLoanUTLBy { get; set; }
        [DataMember]
        public string pLoanUTLRemarks { get; set; }
        [DataMember]
        public string pLoanUTLVia { get; set; }
        [DataMember]
        public string pLoanUTLAmt { get; set; }
        [DataMember]
        public string pLoanUTL { get; set; }
        [DataMember]
        public string pLoanUTLDt { get; set; }
        [DataMember]
        public string pIsSamePurpose { get; set; }
        [DataMember]
        public string pLat { get; set; }
        [DataMember]
        public string pLong { get; set; }
        [DataMember]
        public string pIsPhotoUploaded { get; set; }
        [DataMember]
        public string pCollXml { get; set; }

    }

    [DataContract]
    public class PostBranchCtrlByBranchCode
    {
        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pEffectiveDate { get; set; }
    }

    public class IdfySign
    {
        public string pUrl { get; set; }
        public string pDocumentId { get; set; }
    }

}

