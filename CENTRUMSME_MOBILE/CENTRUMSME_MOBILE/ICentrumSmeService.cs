using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace CENTRUMSME_MOBILE
{
    [ServiceContract]
    public interface ICentrumSmeService
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

        #region Mob_GetLeadList

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLeadList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLeadData> Mob_GetLeadList(string pBranchCode, string pEoId);

        #endregion

        #region Mob_GetLogInFees

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLogInFees",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLogInFeeData> Mob_GetLogInFees(string pDate);

        #endregion

        #region UpdateLead

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_UpdateLead",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateLead(string pLeadId, string pLeadGenerationDate, string pCustomerName, string pEmail, string pMobNo, string pAddress, string pPropertyTypeId,
            string pOccupationId, string pLogInFeesCollYN, string pGenParameterId, string pTotalLoginFees, string pNetLogInFees, string pCGSTAmt, string pSGSTAmt,
            string pIGSTAmt, string pBranchCode, string pCreatedBy);

        #endregion

        #region SaveLead

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_SaveLead",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveLead(string pLeadId, string pLeadGenerationDate, string pCustomerName, string pEmail, string pMobNo, string pAddress, string pPropertyTypeId,
            string pOccupationId, string pLogInFeesCollYN, string pGenParameterId, string pTotalLoginFees, string pNetLogInFees, string pCGSTAmt, string pSGSTAmt,
            string pIGSTAmt, string pBranchCode, string pCreatedBy, string pSRC, string pAddressId, string pAddressIdNo, string pPhotoId, string pPhotoIdNo);

        #endregion

        #region Mob_GetCompanyList

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCompanyList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCompanyData> GetCompanyList(string pBranchCode, string pLogDt, string pCreatedBy);

        #endregion

        #region Mob_GetLoanAppData

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLoanAppData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLoanAppData> GetLoanAppData(string pBranch);

        #endregion

        #region Mob_GetCompanyDetails
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCompanyDetails",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCompanyAllDetailas> GetCompanyDetails(string pBranchCode, string pCompanyId);
        #endregion

        #region Mob_ShowCompanyDtlById
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_ShowCompanyDtlById",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCoAppAllDetailas> ShowCompanyDtlById(string pBranchCode, string pCompanyId, string pCreatedBy);
        #endregion

        #region Mob_GetCoApplicantDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCoApplicantDtl",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCoAppAndDepAllInfo> GetCoApplicantDtl(string pBranchCode, string pCoAppId);
        #endregion

        #region Mob_SaveCompany

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_SaveCompany",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_SaveCompany(string pCompanyId, string pCompanyNo, string pComTypeId, string pCompanyName, string pDOF, string pDOB, string pPropertyId,
            string pOtherPropertyDtl, string pWebsite, string pEmail, string pPANNo, string pAddressId, string pAddressIdNo, string pIsRegister, string pRegisNo,
            string pSectorId, string pSubSectorId, string pMAddress1, string pMAddress2, string pMState, string pMDistrict, string pMPIN, string pMMobNo, string pMSTD,
            string pMTelNo, string pSameAddYN, string pRAddress1, string pRAddress2, string pRState, string pRDistrict, string pRPIN, string pRMobNo, string pRSTD,
            string pRTelNo, string pBranchCode, string pCreatedBy, string pAppName, string pAppContNo, string pGSTRegNo, string pPhotoId, string pPhotoIdNo,
            string pOccupationId, string pBusinessTypeId, string pBusinessLocation, string pAnnualIncome, string pGenderId, string pAge, string pRelationId,
            string pRelativeName, string pQualificationId, string pRligionId, string pCaste, string pNoOfYrInCurRes, string pPhyChallangeYN, string pACHolderName,
            string pACNo, string pBankName, string pIFSCCode, string pYrOfOpening, string pAccountType, string pContactNo, string pEmpOrgName, string pEmpDesig,
            string pEmpRetiredAge, string pEmpCode, string pEmpCurExp, string pEmpTotExp, string pBusGSTAppYN, string pBusGSTNo, string pBusLandMark, string pBusAddress1,
            string pBusAddress2, string pBusLocality, string pBusCity, string pBusPIN, string pBusState, string pBusMob, string pBusPhone, string pCommunAddress,
            string pMaritalStatus, string pResidentialStatus, string pXml, string pBusinessName, string pYearsInCurrBusiness, string pAppReltvName, string pAppRelType, string pID1VoterResponse,
            string pID1AadharResponse, string pNameMatchingResponse, string pIdVerifyYN, string pIsAbledYN, string pSpeciallyAbled,
            string pNomineeName, string pNomineeDOB, string pNomineePinCode, string pNomineeAddress, string pNomineeGender, string pNomineeRelation, string pNomineeState,
            string pXmlDirector,
            string pMinorityYN = "N",
            string pSRC = "NOR");

        #endregion

        #region Mob_SaveCoApplicant

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_SaveCoApplicant",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_SaveCoApplicant(string pCoAppliantId, string pCustId, string pDOA, string pCoAppName,
            string pDOB, string pAge, string pGender, string pReligionId, string pCaste, string pOccuId, string pMaritalSt, string pQualificationId,
            string pPreAddress1, string pPreAddress2, string pPreDistrict, string pPreState, string pPrePIN, string pSameAddYN, string pPerAddress1,
            string pPerAddress2, string pPerDistrict, string pPerState, string pPerPIN, string pAddressId, string pAddressIdNo, string pPhotoId,
            string pPhotoIdNo, string pContMNo, string pContTelNo, string pEmail, string pContFAXNo, string pIsDirector, string pIsPartner,
            string pBranchCode, string pYearAtRes, string pYearAtBus, string pCreatedBy, string pGuarantor,
            string isPropietor, string IsSpouse, string SamAddAsApp, string pIsActive, string pShareHoldPer, string pCoAppType, string pComTypeID,
            string pPropertyTypeId, string pAppName, string pIsPrimaryCoAppYN, string pCustCoAppRel, string pRelationId, string pRelativeName, string pXml,
            string pPhyChallangeYN, string pACHolderName, string pACNo, string pBankName, string pIFSCCode, string pYrOfOpening, string pAccountType,
            string pEmpOrgName, string pEmpDesig, string pEmpRetiredAge, string pEmpCode, string pEmpCurExp, string pEmpTotExp, string pBusGSTAppYN,
            string pBusGSTNo, string pBusLandMark, string pBusAddress1, string pBusAddress2, string pBusLocality, string pBusCity, string pBusPIN,
            string pBusState, string pBusMob, string pBusPhone, string pCommunAddress, string pResidentialStatus, string pNominee, string pBusinessName,
            string pID1VoterResponse, string pID1AadharResponse, string pNameMatchingResponse, string pIdVerifyYN);

        #endregion

        #region KYCImageUpload

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KYCImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> KYCImageUpload(Stream image);

        #endregion

        #region CoAppImageUpload

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "CoAppImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> CoAppImageUpload(Stream image);

        #endregion

        #region Mob_PopComboMISData

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_PopComboMISData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<ComboMisData> PopComboMISData(string pCondition, string pCodeAdd, string pCodeName, string pIDName, string pName, string pTbl, string pConID, string pConIDName,
            string pConIDDate, string pConDate, string pBranchCode);

        #endregion

        #region Mob_GetActiveLnSchemePG

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetActiveLnSchemePG",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LoanSchemeData> GetActiveLnSchemePG(string pBranch);

        #endregion

        #region Mob_GetCoAppByCustId

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCoAppByCustId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<LoanAppCoListData> GetCoAppByCustId(string pCustId);

        #endregion

        #region Mob_GetLoanAppCustForHighmarkList

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLoanAppCustForHighmarkList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetAppCustLoanAppData> GetLoanAppCustForHighmarkList(string pMode, string pCreatedBy);

        #endregion

        #region VerifyEquifax
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "VerifyEquifax",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string VerifyEquifax(string pLoanAppId, string pApplicantId, string pLoanAmt, string pMode, string pBranchCode, string pLogDt, string pCreatedBy);
        #endregion

        #region Mob_SaveLoanApplication

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_SaveLoanApplication",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveLoanApplication(string pApplicantID, string pAppDt, string pPurpId, string pLnTypeId, string pAppAmt, string pTenure,
            string pCBPassYN, string pBrCode, string pCreatedBy, string pEntType, string pYrNo, string pSourceId, string pXmlCoApp, string pPassYN,
            string pPassorRejDate, string pRejReason, string pSRC);

        #endregion

        #region Mob_GetPendingPDBucketList

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetPendingPDBucketList",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PendingPDBucketListData> GetPendingPDBucketList(string pBrCode);

        #endregion

        #region Mob_GetLnAppDetailsForPD
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLnAppDetailsForPD",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PDDetails> GetLnAppDetailsForPD(string pLoanAppId);
        #endregion

        #region Mob_SavePDMst

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_SavePDMst",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDMst(string pPDId, string pLoanAppId, string pCustID, string pCoAppID, string pPDType,
            string pPDDate, string pRemarks, string pTotalSellingIncome, string pTotalCostIncome, string pTotalIncomeMargin, string pTotalExpensesMonthly,
            string pBrCode, string pCreatedBy, string pXmlIncome, string pXmlExpenses, string pSRC);

        #endregion

        #region Mob_GetLogInFees

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetDesigByDesigId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<DesigData> Mob_GetDesigByDesigId(string pDesigId);

        #endregion

        #region Mob_GetCustForRecovery

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCustForRecovery",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<CustRecoveryData> GetCustForRecovery(string pBrCode, string pCreatedBy);

        #endregion

        #region Mob_GetLoannoForBounce

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetLoannoForBounce",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetLoanNoData> GetLoannoForBounce(string CustId);

        #endregion

        #region Mob_GetCollectionByLoanId

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetCollectionByLoanId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<GetCollData> GetCollectionByLoanId(string pLoanId, string pCollDt, string pBranch);

        #endregion

        #region Mob_InsertCollection

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_InsertCollectionNew",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        //string Mob_InsertCollection(string pLoanId, string pAccDate, string pCustId, string pPrincColl, string pIntColl, string pPenColl,
        //    string pWaveIntColl, string pTotalColl, string pPrinOS, string pBrachCode, string pCreatedBy, string pBounceRecv, string pBounceWave, string pClosingType,
        //    string pPreCloseCharge, string pPreCloseWaive, string pBounceDue, string pIntDue, string pPreCollMode, string pPreClsCGST, string pPreClsSGST);

        string Mob_InsertCollectionNew(string pLoanId, string pAccDate, string pCustId, string pPrincColl, string pIntColl, string pPenColl,
                string pPenCGST, string pPenSGST, string pVisitCharge, string pVisitChargeCGST, string pVisitChargeSGST, string pTotalColl, string pPrinOS,
                string pBrachCode, string pCreatedBy, string pBounceRecv, string pClosingType, string pIntDue);

        #endregion

        #region Mob_GetAsonDateIntDue

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_GetAsonDateIntDue",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<AsonDateData> GetAsonDateIntDue(string LoanId, string AsonDate, string LastCollDt, string PrinOS, string IntRate, string LoanDt);

        #endregion

        #region popCategoryVariables

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "popCategoryVariables",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<CategoryVariables> popCategoryVariables();
        #endregion

        #region SaveLoanApplicationQA
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveLoanApplicationQA",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveLoanApplicationQA(LoanApplicationQAData loanApplicationQAData);

        #endregion

        #region GetPDQuestionAnswerMstByLnAppId

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetPDQuestionAnswerMstByLnAppId",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<PDQuestionAnswerMst> GetPDQuestionAnswerMstByLnAppId(string pLoanAppId);

        #endregion

        #region GetPDQuestionAnswerMstByLnAppId
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDPersonalDetail",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDPersonalDetail(string pLoanAppId, string pPDDoneBy, string pPDDate,
         string pAppName, string pAppNameObj, string pAppNameVer, string pAppNameRemark,
         string pAppRelId, string pAppRelObj, string pAppRelVer, string pAppRelRemark,
         string pDOB, string pDOBObj, string pDOBVer, string pDOBRemark,
         string pAge, string pAgeObj, string pAgeVer, string pAgeRemark,
         string pMaritalId, string pMaritalObj, string pMaritalVer, string pMaritalRemark,
         string pNoOfChild, string pNoOfChildObj, string pNoOfChildVer, string pNoOfChildRemark,
         string pEarMem, string pEarMemObj, string pEarMemVer, string pEarMemRemark,
         string pPropOwn, string pPropOwnObj, string pPropOwnVer, string pPropOwnRemark,
         string pOwnType, string pOwnTypeObj, string pOwnTypeVer, string pOwnTypeRemark,
         string pHouseObs, string pHouseObsObj, string pHouseObsVer, string pHouseObsRemark,
         string pPerAddress, string pPerAddressObj, string pPerAddressVer, string pPerAddressRemark,
         string pRefNm, string pRefNmObj, string pRefNmVer, string pRefNmRemark,
         string pMobNo, string pMobNoObj, string pMobNoVer, string pMobNoRemark,
         string pLegalInfo, string pLegalInfoObj, string pLegalInfoVer, string pLegalInfoRemark,
         string pPolInfo, string pPolInfoObj, string pPolInfoVer, string pPolInfoRemark,
         string pPolStatus, string pPolStatusObj, string pPolStatusVer, string pPolStatusRemark,
         string pCreatedBy, string pCustID, string pCustType, string pSchool, string pSchoolObj,
         string pSchoolVer, string pSchoolRemark, string pLatitude, string pLongitude, string pAddress);

        #endregion

        #region SavePDPersonalReference
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDPersonalReference",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDPersonalReference(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
        string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy);
        #endregion

        #region SavePDIncomeSourceBusiness
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourceBusiness",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourceBusiness(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pTotalIncome, string pTotalIncomeObj, string pTotalIncomeVer, string pTotalIncomeRemark,
        string pBusName, string pBusNameObj, string pBusNameVer, string pBusNameRemark,
        string pBusTypeId, string pBusTypeObj, string pBusTypeVer, string pBusTypeRemark,
        string pBusStabId, string pBusStabObj, string pBusStabVer, string pBusStabRemark,
        string pBusAddress, string pBusAddressObj, string pBusAddressVer, string pBusAddressRemark,
        string pNoOfEmp, string pNoOfEmpObj, string pNoOfEmpVer, string pNoOfEmpRemark,
        string pStockSeen, string pStockSeenObj, string pStockSeenVer, string pStockSeenRemark,
        string pVendNm1, string pVendNm1Obj, string pVendNm1Ver, string pVendNm1Remark,
        string pMobNo1, string pMobNo1Obj, string pMobNo1Ver, string pMobNo1Remark,
        string pVendNm2, string pVendNm2Obj, string pVendNm2Ver, string pVendNm2Remark,
        string pMobNo2, string pMobNo2Obj, string pMobNo2Ver, string pMobNo2Remark,
        string pBusAppName, string pBusAppNameObj, string pBusAppNameVer, string pBusAppNameRemark,
        string pNoOfCust, string pNoOfCustObj, string pNoOfCustVer, string pNoOfCustRemark,
        string pCreditAmt, string pCreditAmtObj, string pCreditAmtVer, string pCreditAmtRemark,
        string pNmBoardSeen, string pNmBoardSeenObj, string pNmBoardSeenVer, string pNmBoardSeenRemark,
        string pCreatedBy, string pCustID, string pCustType, string pGrossSales, string pGrossSalesObj,
        string pGrossSalesVer, string pGrossSalesRemark, string pGrossMargin, string pGrossMarginObj,
        string pGrossMarginVer, string pGrossMarginRemark);
        #endregion

        #region SavePDIncomeSourceSalary
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourceSalary",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourceSalary(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pEmpName, string pEmpNameObj, string pEmpNameVer, string pEmpNameRemark,
        string pDesig, string pDesigObj, string pDesigVer, string pDesigRemark,
        string pDOJ, string pDOJObj, string pDOJVer, string pDOJRemark,
        string pJobStb, string pJobStbObj, string pJobStbVer, string pJobStbRemark,
        string pEmpAddress, string pEmpAddressObj, string pEmpAddressVer, string pEmpAddressRemark,
        string pSalCrModeId, string pSalCrModeObj, string pSalCrModeVer, string pSalCrModeRemark,
        string pSalTypeId, string pSalTypeObj, string pSalTypeVer, string pSalTypeRemark,
        string pBankName, string pBankNameObj, string pBankNameVer, string pBankNameRemark,
        string pNetSal, string pNetSalObj, string pNetSalVer, string pNetSalRemark,
        string pHRName, string pHRNameObj, string pHRNameVer, string pHRNameRemark,
        string pHRMobNo, string pHRMobNoObj, string pHRMobNoVer, string pHRMobNoRemark,
        string pCollName, string pCollNameObj, string pCollNameVer, string pCollNameRemark,
        string pCollMobNo, string pCollMobNoObj, string pCollMobNoVer, string pCollMobNoRemark,
        string pPreEmpName, string pPreEmpNameObj, string pPreEmpNameVer, string pPreEmpNameRemark,
        string pPreEmpAddress, string pPreEmpAddressObj, string pPreEmpAddressVer, string pPreEmpAddressRemark,
        string pPreEmpDOJ, string pPreEmpDOJObj, string pPreEmpDOJVer, string pPreEmpDOJRemark,
        string pPreEmpDOL, string pPreEmpDOLObj, string pPreEmpDOLVer, string pPreEmpDOLRemark,
        string pPreWorkExp, string pPreWorkExpObj, string pPreWorkExpVer, string pPreWorkExpRemark,
        string pTotWorkExp, string pTotWorkExpObj, string pTotWorkExpVer, string pTotWorkExpRemark,
        string pCreatedBy, string pCustID, string pCustType);

        #endregion

        #region SavePDIncomeSourceRental
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourceRental",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourceRental(string pLoanAppId, string pPDDoneBy, string pPDDate,
         string pRentalAmt, string pRentalAmtObj, string pRentalAmtVer, string pRentalAmtRemark,
         string pPropType, string pPropTypeObj, string pPropTypeVer, string pPropTypeRemark,
         string pNoOfRentRoom, string pNoOfRentRoomObj, string pNoOfRentRoomVer, string pNoOfRentRoomRemark,
         string pPropAge, string pPropAgeObj, string pPropAgeVer, string pPropAgeRemark,
         string pPropAddress, string pPropAddressObj, string pPropAddressVer, string pPropAddressRemark,
         string pTenantNOC, string pTenantNOCObj, string pTenantNOCVer, string pTenantNOCRemark,
         string pRentalDoc, string pRentalDocObj, string pRentalDocVer, string pRentalDocRemark,
         string pCreatedBy, string pCustID, string pCustType);

        #endregion

        #region SavePDIncomeSourcePension
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourcePension",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourcePension(string pLoanAppId, string pPDDoneBy, string pPDDate,
         string pPensionAmt, string pPensionAmtObj, string pPensionAmtVer, string pPensionAmtRemark,
         string pPenStDate, string pPenStDateObj, string pPenStDateVer, string pPenStDateRemark,
         string pPenOrder, string pPenOrderObj, string pPenOrderVer, string pPenOrderRemark,
         string pPenBankName, string pPenBankNameObj, string pPenBankNameVer, string pPenBankNameRemark,
         string pPenIncId, string pPenIncObj, string pPenIncVer, string pPenIncRemark,
         string pPenFromId, string pPenFromObj, string pPenFromVer, string pPenFromRemark,
         string pCreatedBy, string pCustID, string pCustType);

        #endregion

        #region SavePDIncomeSourceWages
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourceWages",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourceWages(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pWorkType, string pWorkTypeObj, string pWorkTypeVer, string pWorkTypeRemark,
        string pWorkDoingFrom, string pWorkDoingFromObj, string pWorkDoingFromVer, string pWorkDoingFromRemark,
        string pWagesEmpName, string pWagesEmpNameObj, string pWagesEmpNameVer, string pWagesEmpNameRemark,
        string pEmpContactNo, string pEmpContactNoObj, string pEmpContactNoVer, string pEmpContactNoRemark,
        string pCreatedBy, string pCustID, string pCustType);

        #endregion

        #region SavePDIncomeSourceAggriculture
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDIncomeSourceAggriculture",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDIncomeSourceAggriculture(string pLoanAppId, string pPDDate, string pYearlyIncome,
        string pIncomeFrequency, string pAreaOfLand, string pSelfFarmingYN, string pLeasedYN,
        string pTypeofcrops, string pCustType, string pCreatedBy);

        #endregion

        #region SavePDLoanRequireDetail
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDLoanRequireDetail",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDLoanRequireDetail(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pLnPurpose, string pLnPurposeObj, string pLnPurposeVer, string pLnPurposeRemark,
        string pExpLnAmt, string pExpLnAmtObj, string pExpLnAmtVer, string pExpLnAmtRemark,
        string pExpLnTenore, string pExpLnTenoreObj, string pExpLnTenoreVer, string pExpLnTenoreRemark,
        string pEMICap, string pEMICapObj, string pEMICapVer, string pEMICapRemark,
        string pTotLnOutstanding, string pTotLnOutstandingObj, string pTotLnOutstandingVer, string pTotLnOutstandingRemark,
        string pCreatedBy, string pCustID, string pCustType);

        #endregion

        #region SavePDInvestmentSaving

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDInvestmentSaving",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDInvestmentSaving(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pGold, string pGoldObj, string pGoldVer, string pGoldRemark,
        string pBondPaper, string pBondPaperObj, string pBondPaperVer, string pBondPaperRemark,
        string pDailySaving, string pDailySavingObj, string pDailySavingVer, string pDailySavingRemark,
        string pInsuPolicy, string pInsuPolicyObj, string pInsuPolicyVer, string pInsuPolicyRemark,
        string pCreatedBy, string pCustID, string pCustType, string pFixedDeposit, string pFixedDepositObj,
        string pFixedDepositVer, string pFixedDepositRemark, string pAggriLand, string pAggriLandObj,
        string pAggriLandVer, string pAggriLandRemark);

        #endregion

        #region SavePDBankBehaviour

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDBankBehaviour",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDBankBehaviour(string pLoanAppId, string pPDDate, string pAccNo, string pAccType, string pCurrentBal, string pMonth1Bal,
        string pMonth2Bal, string pMonth3Bal, string pMonth1Tran, string pMonth2Tran, string pMonth3Tran, string pMinBal3Months, string pChqueReturns3Months,
        string pCustType, string pCreatedBy);

        #endregion

        #region SavePDApplicantProfile

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDApplicantProfile",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDApplicantProfile(string pLoanAppId, string pPDDate, string pCoOperative, string pAccuracy, string pBusiness, string pHousehold,
        string pSavings, string pInventroy, string pPhysicalFitness, string pFamilySupport, string pCustType, string pCreatedBy);

        #endregion

        #region SavePDCoApplicantProfile

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDCoApplicantProfile",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDCoApplicantProfile(string pLoanAppId, string pPDDate, string pCoOperative, string pAccuracy, string pBusiness, string pHousehold,
        string pSavings, string pInventroy, string pPhysicalFitness, string pFamilySupport, string pCustType, string pCreatedBy);

        #endregion

        #region SavePDBusinessReference1

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDBusinessReference1",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDBusinessReference1(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
        string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy, string pPlaceOfOffice, string pPaymentIssue, string pSupplier
            , string pLatitude, string pLongitude, string pAddress);

        #endregion

        #region SavePDBusinessReference2

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SavePDBusinessReference2",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePDBusinessReference2(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
         string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy, string pPlaceOfOffice, string pPaymentIssue, string pSupplier);

        #endregion

        #region GetRelationPropertyOwnerTypeOfOwner

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetRelationPropertyOwnerTypeOfOwner",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        PDMasterDetails PDMasterDetails();

        #endregion

        #region GetDocType

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "GetDocType",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<DocTypeMst> GetDocType();
        #endregion

        #region CustomerListForDocUpload
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "CustomerListForDocUpload",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<CustomerList> CustomerListForDocUpload(string pCreatedBy);
        #endregion

        #region Upload Document
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SaveDocument",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveDocument(string pLoanAppId, string pDate, string pXmlData, string pMode, string pCreatedBy);


        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DocUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<KYCImageSave> DocUpload(Stream image);
        #endregion

        #region PayNearby FT Integration

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AllExternalImageUpload",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<AllExternalImageUpload> AllExternalImageUpload(Stream image);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "EquifaxIntegration",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string EquifaxIntegration(PostCredential vPostCredential, PostEquifaxDataSet vPostEquifaxDataSet);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DataFrom",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string DataFrom1(RequestHeader RequestHeader, RequestBody RequestBody);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ChkDdup",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ChkDdup(RequestHeader RequestHeader, RequestBody RequestBody);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DataFromOld",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string DataFromOld(RequestHeader RequestHeader, RequestBody RequestBody);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DataFromNew",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        DataFormResponse DataFromNew(DataFormRequest DataFormRequest);



        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DigitalSignStatus",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string DigitalSignStatus(PostCredential vPostCredential, PostDigiDocDataSet vPostDigiDocDataSet);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DigitalDocument",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<DigiDocResponse> DigitalDocument(PostCredential vPostCredential, PostDigiDocDataSet vPostDigiDocDataSet);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ENACHProcessUpdate",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string ENACHProcessUpdate(PostCredential vPostCredential, PostENACHDataSet vPostENACHDataSet);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "PNBRequestResponseData",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SavePNBRequestResponse(RequestHeader RequestHeader, PNBRequestResponseData RequestBody);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "UpdateBankDetails",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string UpdateBankDetails(PostCredential vPostCredential, PostBankDataSet vPostBankDataSet);

        #endregion

        #region FindFinaDoubleLoan
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "FindFinaDoubleLoan",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string FindFinaDoubleLoan(PostFundFinaDoubleLoan vPostFundFinaDoubleLoan);
        #endregion

        #region DownloadSchedule
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "DownloadSchedule",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<DownSchResponse> DownloadSchedule(PostCredential vPostCredential, PostDownSchDataSet vPostDownSchDataSet);
        #endregion

        #region FundFinaLoanDisbursement
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "FundFinaLoanDisbursement",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string FundFinaLoanDisbursement(RequestHeader RequestHeader, PostDisbursement RequestBody);
        #endregion

        #region InitiateDigitalDoc
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InitiateDigitalDoc",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<InitiateDigitalDocResponse> InitiateDigitalDoc(PostCredential vPostCredential, PostDigiDocOTPDataSet vPostDigiDocOTPDataSet);
        #endregion

        #region FundFinaCancelLoanApplication
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "FundFinaCancelLoanApplication",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string FundFinaCancelLoanApplication(RequestHeader RequestHeader, PostDisbursement RequestBody);
        #endregion

        #region FundFinaCollection
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "FundFinaCollection",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string FundFinaCollection(RequestHeader RequestHeader, PostFindFinaCollection RequestBody);
        #endregion

        #region InsertBulkCollection
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "InsertBulkCollection",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy);
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

        #region Karza
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "KarzaVoterIDKYCValidation",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        KYCVoterIDResponse KarzaVoterIDKYCValidation(KYCVoterIDRequest vPostVoterData);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "NameMatching",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        MatchResponse NameMatching(NameMatch vNameMatch);

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
            UriTemplate = "SaveKarzaAadharVerifyData",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseData);

        #endregion

        #region Mob_ChangePassword
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Mob_ChangePassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword);
        #endregion

        #region GetIFSCDtl
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "aNjXeGhKsD5i0hFnb",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData);
        #endregion

        #region BankAcVerify
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "BankAcVerify",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        FingPayResponse BankAcVerify(FingPayRequest req);
        #endregion

        #region TimeChk
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "TimeChk",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        ServerStatus TimeChk();
        #endregion

        #region AadhaarVault
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "AadhaarVault",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        AadhaarVaultResponse AadhaarVault(AadhaarVault AadhaarVault);
        #endregion

        #region Prosidex
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "Prosidex",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ProsidexResponse Prosidex(string pMemberID, string pLoanId, Int32 pCreatedBy);
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

        #region Jocata
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "JocataRequest",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string JocataRequest(string pMemberID, string pLoanId, Int32 pCreatedBy);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "JocataRiskCat",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        string JocataRiskCat(RiskCatReq vRiskCatReq);
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

        #region SendOTP
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        string SendOTP(OTPData objOTPData);
        #endregion

        #region ForgotPassword
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "SendForgotOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ForgotOTPRes SendForgotOTP(string pUserName);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ValidateOTP",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ForgotOTPRes ValidateOTP(string pOTP, string pOTPId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "ForgotPassword",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        ForgotOTPRes ForgotPassword(string pUserName, string pPassword, string pOTPId, string pOTP);
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
