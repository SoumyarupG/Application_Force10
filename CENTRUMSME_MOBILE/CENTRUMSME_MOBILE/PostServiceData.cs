using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;

namespace CENTRUMSME_MOBILE
{

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
    }

    [DataContract]
    public class LoanApplicationQAData
    {
        [DataMember]
        public string pQid { get; set; }

        [DataMember]
        public string pAnsID { get; set; }

        [DataMember]
        public string pCmt { get; set; }

        [DataMember]
        public string pLoanAppId { get; set; }

    }
    [DataContract]
    public class PostFundFinaDoubleLoan
    {

        [DataMember]
        public string PurposeOfLoan { get; set; }
        [DataMember]
        public string PartnerID { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string MemberId { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string CMLApprovedLoanAmount { get; set; }
        [DataMember]
        public string CMLApprovedTenure { get; set; }
        [DataMember]
        public string EMI { get; set; }
        [DataMember]
        public string InsurancePremiumAmount { get; set; }
        [DataMember]
        public string ProcessingFees { get; set; }
        [DataMember]
        public string ApprovedPaymentFrequency { get; set; }
        [DataMember]
        public string ROI { get; set; }
        [DataMember]
        public string EstimatedCollectionDate { get; set; }
        [DataMember]
        public string CustomerOptedForInsurance { get; set; }
        [DataMember]
        public string EstimatedDisbursementDate { get; set; }
    }
    #region PaynearBy FT Integration

    [DataContract]
    public class PostDigiDocDataSet
    {
        [DataMember]
        public string LoanAppNo { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
    }

    [DataContract]
    public class PostENACHDataSet
    {
        [DataMember]
        public string LoanAppNo { get; set; }
        [DataMember]
        public string StatusofProcess { get; set; }
    }

    [DataContract]
    public class PostEquifaxDataSet
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public string AddressType { get; set; }
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string StateName { get; set; }
        [DataMember]
        public string PIN { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string IDType { get; set; }
        [DataMember]
        public string IDValue { get; set; }
        [DataMember]
        public string AddType { get; set; }
        [DataMember]
        public string AddValue { get; set; }
        [DataMember]
        public string CoAppRel { get; set; }
        [DataMember]
        public string CoAppName { get; set; }
        [DataMember]
        public string GenderId { get; set; }

    }

    [DataContract]
    public class PostCredential
    {
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string PartnerID { get; set; }
    }

    [DataContract]
    public class PostDigiDocOTPDataSet
    {
        [DataMember]
        public string LoanAppNo { get; set; }
        [DataMember]
        public string MacID { get; set; }
    }

    [DataContract]
    public class PostBankDataSet
    {
        [DataMember]
        public string LoanAppNo { get; set; }
        [DataMember]
        public string AcHolderName { get; set; }
        [DataMember]
        public string AccountNo { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string IfscCode { get; set; }
        [DataMember]
        public string AccountType { get; set; }
        [DataMember]
        public string YrOfOpening { get; set; }
    }

    #endregion

    #region PaynearBy Request

    [DataContract]
    public class RequestHeader
    {
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string PartnerID { get; set; }
    }

    [DataContract]
    public class RequestBody
    {
        [DataMember]
        public PreliminaryDetails PreliminaryDetails { get; set; }
        [DataMember]
        public KYCDetails KYCDetails { get; set; }
        [DataMember]
        public ResidenceAddressDetails ResidenceAddressDetails { get; set; }
        [DataMember]
        public BusinessDetails BusinessDetails { get; set; }
        [DataMember]
        public BankDetails BankDetails { get; set; }
        [DataMember]
        public OtherDetails OtherDetails { get; set; }
        [DataMember]
        public CustomerLoanDetails CustomerLoanDetails { get; set; }
        [DataMember]
        public InsuranceDetails InsuranceDetails { get; set; }
        [DataMember]
        public CAMDetails CAMDetails { get; set; }
        [DataMember]
        public BranchInformation BranchInformation { get; set; }
    }

    [DataContract]
    public class DataFormRequest
    {
        [DataMember]
        public RequestHeader RequestHeader { get; set; }
        [DataMember]
        public RequestBody RequestBody { get; set; }
    }

    [DataContract]
    public class PreliminaryDetails
    {
        [DataMember]
        public string MobileNumber { get; set; }
        [DataMember]
        public string IsMobileNumberDifferentThenLinkedtoAadhaar { get; set; }
        [DataMember]
        public string MobileNumberLinkedWithAadhaar { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string DateOfBirth { get; set; }
        [DataMember]
        public string MaritalStatus { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public string NumberOfDependants { get; set; }
    }

    [DataContract]
    public class KYCDetails
    {
        [DataMember]
        public string PANCardNumber { get; set; }
        [DataMember]
        public string POADocumentType { get; set; }
        [DataMember]
        public string POANumber { get; set; }
        [DataMember]
        public string AadhaarDocumentNumber { get; set; }
    }

    [DataContract]
    public class ResidenceAddressDetails
    {
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Pincode { get; set; }
        [DataMember]
        public string PincodeClassification { get; set; }
        [DataMember]
        public string Area { get; set; }
        [DataMember]
        public string CityDistrict { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ResidenceOwnershipType { get; set; }
        [DataMember]
        public string SinceWhenAreYouStayingHere { get; set; }
    }

    [DataContract]
    public class BusinessDetails
    {
        [DataMember]
        public string BusinessAddress { get; set; }
        [DataMember]
        public string Pincode { get; set; }
        [DataMember]
        public string PincodeClassification { get; set; }
        [DataMember]
        public string CityDistrict { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string BusinessPremiseOwnershipType { get; set; }
        [DataMember]
        public string SinceWhenAreYouDoingBusinessHere { get; set; }
        [DataMember]
        public string NoOfYearsInThisBusiness { get; set; }
        [DataMember]
        public string StoreType { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string HowDoYouDoYourSaleCollectionsInBusiness { get; set; }
        [DataMember]
        public string MonthlyTurnover { get; set; }
        [DataMember]
        public string GoogleMapAddress { get; set; }
        [DataMember]
        public string LatLongOfTheAddress { get; set; }
    }

    [DataContract]
    public class BankDetails
    {
        [DataMember]
        public string BankAccountType { get; set; }
        [DataMember]
        public string BankName { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string IFSCCode { get; set; }
        [DataMember]
        public string AccountOperationalScince { get; set; }
    }

    [DataContract]
    public class OtherDetails
    {
        [DataMember]
        public string MonthlyFamilyExpense { get; set; }
        [DataMember]
        public string AnyOtherIncome { get; set; }
        [DataMember]
        public string PurposeOfLoan { get; set; }
        [DataMember]
        public string ExistingLoan { get; set; }
        [DataMember]
        public string TypeOfLoan { get; set; }
        [DataMember]
        public string ExistingCreditCard { get; set; }
    }

    [DataContract]
    public class CustomerLoanDetails
    {
        [DataMember]
        public string LoanAmountRequired { get; set; }
        [DataMember]
        public string LoanTenureRequired { get; set; }
        [DataMember]
        public string ROIfixed { get; set; }
    }

    [DataContract]
    public class InsuranceDetails
    {
        [DataMember]
        public string CustomerOptedForInsurance { get; set; }
        [DataMember]
        public string NomineeName { get; set; }
        [DataMember]
        public string RelationshipWithNominee { get; set; }
        [DataMember]
        public string NomineeGender { get; set; }
        [DataMember]
        public string NomineeDOB { get; set; }
    }

    [DataContract]
    public class CAMDetails
    {
        [DataMember]
        public string NBTScore { get; set; }
        [DataMember]
        public string PNBIncomeForLast24months { get; set; }
        [DataMember]
        public string PNBGTVForLast24months { get; set; }
        [DataMember]
        public string NBTRecommendedLoanAmount { get; set; }
        [DataMember]
        public string NBTrecommendedTenure { get; set; }
        [DataMember]
        public string CMLApprovedLoanAmount { get; set; }
        [DataMember]
        public string CMLApprovedTenure { get; set; }
        [DataMember]
        public string ROI { get; set; }
        [DataMember]
        public string EstimatedDisbursementDate { get; set; }
        [DataMember]
        public string EstimatedCollectionDate { get; set; }
        [DataMember]
        public string ProcessingFees { get; set; }
        [DataMember]
        public string InsurancePremiumAmount { get; set; }
        [DataMember]
        public string InsuranceJobID { get; set; }
        [DataMember]
        public string EMI { get; set; }
        [DataMember]
        public string FinalDisbursableAmount { get; set; }
        [DataMember]
        public string CommentsAddedByCMLInCAM { get; set; }
        [DataMember]
        public string LoanApprovedRejected { get; set; }
        [DataMember]
        public string RejectedReason { get; set; }
        [DataMember]
        public string ApprovedPaymentFrequency { get; set; }
    }

    [DataContract]
    public class BranchInformation
    {
        [DataMember]
        public string BranchCode { get; set; }
    }

    #endregion

    [DataContract]
    public class PostDisbursement
    {
        [DataMember]
        public string LoanAppNo { get; set; }
        [DataMember]
        public string DisburesmentDate { get; set; }
        [DataMember]
        public string CollectionStartDate { get; set; }
    }

    [DataContract]
    public class PNBRequestResponseData
    {
        [DataMember]
        public AccountInformation AccountInformation { get; set; }
        [DataMember]
        public PAN PAN { get; set; }
        [DataMember]
        public AadharVoter AadharVoter { get; set; }
        [DataMember]
        public AadharDL AadharDL { get; set; }
        [DataMember]
        public BankAccount BankAccount { get; set; }
        [DataMember]
        public Insurance Insurance { get; set; }
        [DataMember]
        public eNACH eNACH { get; set; }

    }

    [DataContract]
    public class PAN
    {
        [DataMember]
        public string PANOCRRequest { get; set; }
        [DataMember]
        public string PANOCRResponse { get; set; }
        [DataMember]
        public string PANAuthenticationRequest { get; set; }
        [DataMember]
        public string PANAuthenticationResponse { get; set; }
    }

    [DataContract]
    public class AadharVoter
    {
        [DataMember]
        public string AadharOCRRequest { get; set; }
        [DataMember]
        public string AadharOCRResponse { get; set; }
        [DataMember]
        public string VoterIDOCRRequest { get; set; }
        [DataMember]
        public string VoterIDOCTResponse { get; set; }
        [DataMember]
        public string VoterIDAuthenticationRequest { get; set; }
        [DataMember]
        public string VoterIDAuthenticationResponse { get; set; }
    }

    [DataContract]
    public class AadharDL
    {
        [DataMember]
        public string AadharOCRRequest { get; set; }
        [DataMember]
        public string AadharOCRResponse { get; set; }
        [DataMember]
        public string DLOCRRequest { get; set; }
        [DataMember]
        public string DLAuthenticationRequest { get; set; }
        [DataMember]
        public string DLAuthenticationResponse { get; set; }
    }

    [DataContract]
    public class BankAccount
    {
        [DataMember]
        public string AccountValidationRequest { get; set; }
        [DataMember]
        public string AccountValidationResponse { get; set; }

    }

    [DataContract]
    public class Insurance
    {
        [DataMember]
        public string InsuranceRequest { get; set; }
        [DataMember]
        public string InsuranceResponse { get; set; }
    }

    [DataContract]
    public class eNACH
    {
        [DataMember]
        public string nuPayRequest { get; set; }
        [DataMember]
        public string nuPayResponse { get; set; }
    }

    [DataContract]
    public class AccountInformation
    {
        [DataMember]
        public string LoanAppNo { get; set; }
    }

    [DataContract]
    public class PostDownSchDataSet
    {
        [DataMember]
        public string LoanAppNo { get; set; }

    }

    [DataContract]
    public class PostFindFinaCollection
    {
        [DataMember]
        public string ReferenceNo { get; set; }
        [DataMember]
        public string SettlementDate { get; set; }
        [DataMember]
        public string Amount { get; set; }
        [DataMember]
        public string PrincipalAmountRecovered { get; set; }
        [DataMember]
        public string InterestAmountRecovered { get; set; }
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
    public class AadhaarXmlOTP
    {
        [DataMember]
        public string aadhaarNo { get; set; }
        [DataMember]
        public string consent { get; set; }
        //[DataMember]
        //public string pBranch { get; set; }
        //[DataMember]
        //public string pEoID { get; set; }
    }

    [DataContract]
    public class AadhaarXmlDownload
    {
        [DataMember]
        public string otp { get; set; }
        [DataMember]
        public string aadhaarNo { get; set; }
        [DataMember]
        public string requestId { get; set; }
        [DataMember]
        public string consent { get; set; }
        //[DataMember]
        //public string pBranch { get; set; }
        //[DataMember]
        //public string pEoID { get; set; }
    }

    [DataContract]
    public class eAadhaarOTP
    {
        [DataMember]
        public string aadhaarNo { get; set; }
        [DataMember]
        public string consent { get; set; }
        //[DataMember]
        //public string pBranch { get; set; }
        //[DataMember]
        //public string pEoID { get; set; }
    }

    [DataContract]
    public class eAadhaarDownload
    {
        [DataMember]
        public string otp { get; set; }
        [DataMember]
        public string aadhaarNo { get; set; }
        [DataMember]
        public string accessKey { get; set; }
        [DataMember]
        public string consent { get; set; }
        //[DataMember]
        //public string pBranch { get; set; }
        //[DataMember]
        //public string pEoID { get; set; }
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
    public class NameMatchRequest
    {
        [DataMember]
        public string name1 { get; set; }
        [DataMember]
        public string name2 { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string preset { get; set; }
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
    public class PostIFSCData
    {
        [DataMember]
        public string pIFSCCode { get; set; }
    }

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
    public class FingPayRequest
    {
        [DataMember]
        public string beneAccNo { get; set; }
        [DataMember]
        public string beneIFSC { get; set; }
        [DataMember]
        public string MemberId { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
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

    #region Jocata_Class

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

    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }

    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    public class PostRampRequest
    {
        public RampRequest rampRequest { get; set; }
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
        public string LoanNo { get; set; }
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
    public class OTPData
    {
        [DataMember]
        public string pMobileNo { get; set; }
        [DataMember]
        public string pOTP { get; set; }
    }

    #region IBM
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

    #endregion

    [DataContract]
    public class PostBranchCtrlByBranchCode
    {
        [DataMember]
        public string pBranchCode { get; set; }

        [DataMember]
        public string pEffectiveDate { get; set; }
    }
}