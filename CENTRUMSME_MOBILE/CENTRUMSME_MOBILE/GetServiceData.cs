using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;

namespace CENTRUMSME_MOBILE
{
    [DataContract]
    public class LoginLogOutData
    {
        [DataMember]
        public string LoginId { get; set; }
    }

    #region EmpData
    [DataContract]
    public class EmpData
    {
        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string UserID { get; set; }

        [DataMember]
        public string Eoid { get; set; }

        [DataMember]
        public string LogStatus { get; set; }

        [DataMember]
        public string DayEndDate { get; set; }

        [DataMember]
        public string AttStatus { get; set; }

        [DataMember]
        public string AttFlag { get; set; }

        [DataMember]
        public string AreaCat { get; set; }

        [DataMember]
        public string Designation { get; set; }

        [DataMember]
        public string LoginId { get; set; }

        [DataMember]
        public string MFAYN { get; set; }

        [DataMember]
        public string MFAOTP { get; set; }

        [DataMember]
        public string DialogToImageYN { get; set; }

        public EmpData(string EoName, string BranchCode, string UserID, string Eoid, string LogStatus, string DayEndDate, string AttStatus, string AttFlag,
            string AreaCat, string Designation, string LoginId, string MFAYN, string MFAOTP, string DialogToImageYN)
        {
            this.EoName = EoName;
            this.BranchCode = BranchCode;
            this.UserID = UserID;
            this.Eoid = Eoid;
            this.LogStatus = LogStatus;
            this.DayEndDate = DayEndDate;
            this.AttStatus = AttStatus;
            this.AttFlag = AttFlag;
            this.AreaCat = AreaCat;
            this.Designation = Designation;
            this.LoginId = LoginId;
            this.MFAYN = MFAYN;
            this.MFAOTP = MFAOTP;
            this.DialogToImageYN = DialogToImageYN;
        }
    }


    #endregion

    #region GetLeadData

    [DataContract]
    public class GetLeadData
    {
        [DataMember]
        public string LeadId { get; set; }

        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string LogInFeesCollYN { get; set; }

        [DataMember]
        public string LeadGenerationDate { get; set; }

        [DataMember]
        public string TotalLoginFees { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string MobNo { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string PropertyTypeId { get; set; }

        [DataMember]
        public string OccupationId { get; set; }


        public GetLeadData(string LeadId, string CustomerName, string LogInFeesCollYN, string LeadGenerationDate, string TotalLoginFees, string Branch,
            string MobNo, string Email, string Address, string PropertyTypeId, string OccupationId)
        {
            this.LeadId = LeadId;
            this.CustomerName = CustomerName;
            this.LogInFeesCollYN = LogInFeesCollYN;
            this.LeadGenerationDate = LeadGenerationDate;
            this.TotalLoginFees = TotalLoginFees;
            this.Branch = Branch;
            this.MobNo = MobNo;
            this.Email = Email;
            this.Address = Address;
            this.PropertyTypeId = PropertyTypeId;
            this.OccupationId = OccupationId;

        }
    }

    #endregion

    #region GetLogInFeeData

    [DataContract]
    public class GetLogInFeeData
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string TotalLoginFees { get; set; }

        [DataMember]
        public string NetLogInFees { get; set; }

        [DataMember]
        public string CGSTAmt { get; set; }

        [DataMember]
        public string SGSTAmt { get; set; }

        [DataMember]
        public string IGSTAmt { get; set; }

        public GetLogInFeeData(string Id, string TotalLoginFees, string NetLogInFees, string CGSTAmt, string SGSTAmt, string IGSTAmt)
        {
            this.Id = Id;
            this.TotalLoginFees = TotalLoginFees;
            this.NetLogInFees = NetLogInFees;
            this.CGSTAmt = CGSTAmt;
            this.SGSTAmt = SGSTAmt;
            this.IGSTAmt = IGSTAmt;
        }
    }

    #endregion

    #region GetCompanyData

    [DataContract]
    public class GetCompanyData
    {
        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string DOF { get; set; }

        [DataMember]
        public string Branch { get; set; }

        [DataMember]
        public string ApplicantName { get; set; }

        [DataMember]
        public string MemType { get; set; }


        public GetCompanyData(string CustId, string CompanyName, string DOF, string Branch, string ApplicantName, string MemType)
        {
            this.CustId = CustId;
            this.CompanyName = CompanyName;
            this.DOF = DOF;
            this.Branch = Branch;
            this.ApplicantName = ApplicantName;
            this.MemType = MemType;
        }
    }

    #endregion

    #region GetLoanAppData

    [DataContract]
    public class GetLoanAppData
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string TypeId { get; set; }

        [DataMember]
        public string TypeName { get; set; }


        public GetLoanAppData(string Type, string TypeId, string TypeName)
        {
            this.Type = Type;
            this.TypeId = TypeId;
            this.TypeName = TypeName;
        }
    }

    #endregion

    #region GetCompanyDetailsData

    [DataContract]
    public class GetCompanyDetailsData
    {
        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CustNo { get; set; }

        [DataMember]
        public string CompanyTypeID { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string DOF { get; set; }

        [DataMember]
        public string DOB { get; set; }

        [DataMember]
        public string PropertyTypeId { get; set; }

        [DataMember]
        public string OtherPropertyType { get; set; }

        [DataMember]
        public string Website { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PANNo { get; set; }

        [DataMember]
        public string AddressId { get; set; }

        [DataMember]
        public string AddressIdNo { get; set; }

        [DataMember]
        public string SectorId { get; set; }

        [DataMember]
        public string SubSectorId { get; set; }

        [DataMember]
        public string IsRegistered { get; set; }

        [DataMember]
        public string RegistrationNo { get; set; }

        [DataMember]
        public string MAddress1 { get; set; }

        [DataMember]
        public string MAddress2 { get; set; }

        [DataMember]
        public string MState { get; set; }

        [DataMember]
        public string MDistrict { get; set; }

        [DataMember]
        public string MPIN { get; set; }

        [DataMember]
        public string MMobNo { get; set; }

        [DataMember]
        public string MSTD { get; set; }

        [DataMember]
        public string MTelNo { get; set; }

        [DataMember]
        public string ApplicantName { get; set; }

        [DataMember]
        public string AppContactNo { get; set; }

        [DataMember]
        public string SameAddYN { get; set; }

        [DataMember]
        public string RAddress1 { get; set; }

        [DataMember]
        public string RAddress2 { get; set; }

        [DataMember]
        public string RState { get; set; }

        [DataMember]
        public string RDistrict { get; set; }

        [DataMember]
        public string RPIN { get; set; }

        [DataMember]
        public string RMobNo { get; set; }

        [DataMember]
        public string RSTD { get; set; }

        [DataMember]
        public string RTelNo { get; set; }

        [DataMember]
        public string GSTRegNo { get; set; }

        [DataMember]
        public string CustType { get; set; }

        [DataMember]
        public string PhotoId { get; set; }

        [DataMember]
        public string PhotoIdNo { get; set; }

        [DataMember]
        public string GenderId { get; set; }

        [DataMember]
        public string OccupationId { get; set; }

        [DataMember]
        public string BusinessTypeId { get; set; }

        [DataMember]
        public string BusinessLocation { get; set; }

        [DataMember]
        public string AnnualIncome { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string RelativeName { get; set; }

        [DataMember]
        public string QualificationId { get; set; }

        [DataMember]
        public string RligionId { get; set; }

        [DataMember]
        public string Caste { get; set; }

        [DataMember]
        public string NoOfYrInCurRes { get; set; }

        [DataMember]
        public string PhyChallangeYN { get; set; }

        [DataMember]
        public string ACHolderName { get; set; }

        [DataMember]
        public string ACNo { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string IFSCCode { get; set; }

        [DataMember]
        public string YrOfOpening { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string ContactNo { get; set; }

        [DataMember]
        public string EmpOrgName { get; set; }

        [DataMember]
        public string EmpDesig { get; set; }

        [DataMember]
        public string EmpRetiredAge { get; set; }

        [DataMember]
        public string EmpCode { get; set; }

        [DataMember]
        public string EmpCurExp { get; set; }

        [DataMember]
        public string EmpTotExp { get; set; }

        [DataMember]
        public string BusGSTAppYN { get; set; }

        [DataMember]
        public string BusGSTNo { get; set; }

        [DataMember]
        public string BusLandMark { get; set; }

        [DataMember]
        public string BusAddress1 { get; set; }

        [DataMember]
        public string BusAddress2 { get; set; }

        [DataMember]
        public string BusLocality { get; set; }

        [DataMember]
        public string BusCity { get; set; }

        [DataMember]
        public string BusPIN { get; set; }

        [DataMember]
        public string BusState { get; set; }

        [DataMember]
        public string BusMob { get; set; }

        [DataMember]
        public string BusPhone { get; set; }

        [DataMember]
        public string CommunAddress { get; set; }

        [DataMember]
        public string MaritalStatus { get; set; }

        [DataMember]
        public string ResidentialStatus { get; set; }

        [DataMember]
        public string NomineeName { get; set; }

        [DataMember]
        public string NomineeDOB { get; set; }

        [DataMember]
        public string NomineeGender { get; set; }

        [DataMember]
        public string NomineeRelation { get; set; }

        [DataMember]
        public string NomineeAddress { get; set; }

        [DataMember]
        public string NomineeState { get; set; }

        [DataMember]
        public string NomineePinCode { get; set; }


        public GetCompanyDetailsData(string CustId, string CustNo, string CompanyTypeID, string CompanyName, string DOF, string DOB, string PropertyTypeId, string OtherPropertyType, string Website, string Email,
            string PANNo, string AddressId, string AddressIdNo, string SectorId, string SubSectorId, string IsRegistered, string RegistrationNo, string MAddress1, string MAddress2, string MState, string MDistrict,
            string MPIN, string MMobNo, string MSTD, string MTelNo, string ApplicantName, string AppContactNo, string SameAddYN, string RAddress1, string RAddress2, string RState, string RDistrict, string RPIN,
            string RMobNo, string RSTD, string RTelNo, string GSTRegNo, string CustType, string PhotoId, string PhotoIdNo, string GenderId, string OccupationId, string BusinessTypeId, string BusinessLocation,
            string AnnualIncome, string Age, string RelativeName, string QualificationId, string RligionId, string Caste, string NoOfYrInCurRes, string PhyChallangeYN, string ACHolderName, string ACNo, string BankName,
            string IFSCCode, string YrOfOpening, string AccountType, string ContactNo, string EmpOrgName, string EmpDesig, string EmpRetiredAge, string EmpCode, string EmpCurExp, string EmpTotExp, string BusGSTAppYN, string BusGSTNo,
            string BusLandMark, string BusAddress1, string BusAddress2, string BusLocality, string BusCity, string BusPIN, string BusState, string BusMob, string BusPhone, string CommunAddress, string MaritalStatus,
            string ResidentialStatus, string RelationId, string NomineeName, string NomineeDOB, string NomineeGender, string NomineeRelation, string NomineeAddress
            , string NomineeState, string NomineePinCode)
        {
            this.CustId = CustId;
            this.CustNo = CustNo;
            this.CompanyTypeID = CompanyTypeID;
            this.CompanyName = CompanyName;
            this.DOF = DOF;
            this.DOB = DOB;
            this.PropertyTypeId = PropertyTypeId;
            this.OtherPropertyType = OtherPropertyType;
            this.Website = Website;
            this.Email = Email;
            this.PANNo = PANNo;
            this.AddressId = AddressId;
            this.AddressIdNo = AddressIdNo;
            this.SectorId = SectorId;
            this.SubSectorId = SubSectorId;
            this.IsRegistered = IsRegistered;
            this.RegistrationNo = RegistrationNo;
            this.MAddress1 = MAddress1;
            this.MAddress2 = MAddress2;
            this.MState = MState;
            this.MDistrict = MDistrict;
            this.MPIN = MPIN;
            this.MMobNo = MMobNo;
            this.MSTD = MSTD;
            this.MTelNo = MTelNo;
            this.ApplicantName = ApplicantName;
            this.AppContactNo = AppContactNo;
            this.SameAddYN = SameAddYN;
            this.RAddress1 = RAddress1;
            this.RAddress2 = RAddress2;
            this.RState = RState;
            this.RDistrict = RDistrict;
            this.RPIN = RPIN;
            this.RMobNo = RMobNo;
            this.RSTD = RSTD;
            this.RTelNo = RTelNo;
            this.GSTRegNo = GSTRegNo;
            this.CustType = CustType;
            this.PhotoId = PhotoId;
            this.PhotoIdNo = PhotoIdNo;
            this.GenderId = GenderId;
            this.OccupationId = OccupationId;
            this.BusinessTypeId = BusinessTypeId;
            this.BusinessLocation = BusinessLocation;
            this.AnnualIncome = AnnualIncome;
            this.Age = Age;
            this.RelativeName = RelativeName;
            this.QualificationId = QualificationId;
            this.RligionId = RligionId;
            this.Caste = Caste;
            this.NoOfYrInCurRes = NoOfYrInCurRes;
            this.PhyChallangeYN = PhyChallangeYN;
            this.ACHolderName = ACHolderName;
            this.ACNo = ACNo;
            this.BankName = BankName;
            this.IFSCCode = IFSCCode;
            this.YrOfOpening = YrOfOpening;
            this.AccountType = AccountType;
            this.ContactNo = ContactNo;
            this.EmpOrgName = EmpOrgName;
            this.EmpDesig = EmpDesig;
            this.EmpRetiredAge = EmpRetiredAge;
            this.EmpCode = EmpCode;
            this.EmpCurExp = EmpCurExp;
            this.EmpTotExp = EmpTotExp;
            this.BusGSTAppYN = BusGSTAppYN;
            this.BusGSTNo = BusGSTNo;
            this.BusLandMark = BusLandMark;
            this.BusAddress1 = BusAddress1;
            this.BusAddress2 = BusAddress2;
            this.BusLocality = BusLocality;
            this.BusCity = BusCity;
            this.BusPIN = BusPIN;
            this.BusState = BusState;
            this.BusMob = BusMob;
            this.BusPhone = BusPhone;
            this.CommunAddress = CommunAddress;
            this.MaritalStatus = MaritalStatus;
            this.ResidentialStatus = ResidentialStatus;
            this.RelationId = RelationId;
            this.NomineeName = NomineeName; 
            this.NomineeDOB = NomineeDOB; 
            this.NomineeGender = NomineeGender; 
            this.NomineeRelation = NomineeRelation; 
            this.NomineeAddress = NomineeAddress;
            this.NomineeState = NomineeState;
            this.NomineePinCode = NomineePinCode;


        }
    }

    [DataContract]
    public class GetCompanyDependent
    {
        [DataMember]
        public string SLNo { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string OccupationId { get; set; }


        public GetCompanyDependent(string SLNo, string Name, string RelationId, string Age, string OccupationId)
        {
            this.SLNo = SLNo;
            this.Name = Name;
            this.RelationId = RelationId;
            this.Age = Age;
            this.OccupationId = OccupationId;
        }
    }

    [DataContract]
    public class GetCompanyAllDetailas
    {
        [DataMember]
        public List<GetCompanyDetailsData> MemberKYCDetails { get; set; }

        [DataMember]
        public List<GetCompanyDependent> FamilyAllData { get; set; }

        public GetCompanyAllDetailas(List<GetCompanyDetailsData> MemberKYCDetails, List<GetCompanyDependent> FamilyAllData)
        {
            this.MemberKYCDetails = MemberKYCDetails;
            this.FamilyAllData = FamilyAllData;
        }
    }

    #endregion

    #region GetCompanyDtlById

    [DataContract]
    public class GetCompanyDtlById
    {
        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CustNo { get; set; }

        [DataMember]
        public string CompanyName { get; set; }


        public GetCompanyDtlById(string CustId, string CustNo, string CompanyName)
        {
            this.CustId = CustId;
            this.CustNo = CustNo;
            this.CompanyName = CompanyName;

        }
    }

    [DataContract]
    public class GetCoAppInfo
    {
        [DataMember]
        public string CoApplicantId { get; set; }

        [DataMember]
        public string CoApplicantNo { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CoAppName { get; set; }

        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string IsGuarantor { get; set; }

        [DataMember]
        public string IsPrimaryCoAppYN { get; set; }


        public GetCoAppInfo(string CoApplicantId, string CoApplicantNo, string CustId, string CoAppName, string BranchCode, string IsGuarantor, string IsPrimaryCoAppYN)
        {
            this.CoApplicantId = CoApplicantId;
            this.CoApplicantNo = CoApplicantNo;
            this.CustId = CustId;
            this.CoAppName = CoAppName;
            this.BranchCode = BranchCode;
            this.IsGuarantor = IsGuarantor;
            this.IsPrimaryCoAppYN = IsPrimaryCoAppYN;
        }
    }

    [DataContract]
    public class GetCoAppAllDetailas
    {
        [DataMember]
        public List<GetCompanyDtlById> GetCompanyDtlById { get; set; }

        [DataMember]
        public List<GetCoAppInfo> GetCoAppInfo { get; set; }

        public GetCoAppAllDetailas(List<GetCompanyDtlById> GetCompanyDtlById, List<GetCoAppInfo> GetCoAppInfo)
        {
            this.GetCompanyDtlById = GetCompanyDtlById;
            this.GetCoAppInfo = GetCoAppInfo;
        }
    }

    #endregion

    #region GetCoAppDetlById

    [DataContract]
    public class GetCoAppDetlById
    {

        [DataMember]
        public string CoApplicantId { get; set; }

        [DataMember]
        public string CoApplicantNo { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string DOA { get; set; }

        [DataMember]
        public string DOB { get; set; }

        [DataMember]
        public string CoAppName { get; set; }

        [DataMember]
        public string CastId { get; set; }

        [DataMember]
        public string ReligionId { get; set; }

        [DataMember]
        public string OccupationId { get; set; }

        [DataMember]
        public string MaritalStatus { get; set; }

        [DataMember]
        public string Gender { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string Qualification { get; set; }

        [DataMember]
        public string PhotoId { get; set; }

        [DataMember]
        public string PhotoIdNo { get; set; }

        [DataMember]
        public string AddressId { get; set; }

        [DataMember]
        public string AddressIdNo { get; set; }

        [DataMember]
        public string ContMNo { get; set; }

        [DataMember]
        public string ContTelNo { get; set; }

        [DataMember]
        public string ContFAXNo { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string YearAtRes { get; set; }

        [DataMember]
        public string YearAtBus { get; set; }

        [DataMember]
        public string PreAddress1 { get; set; }

        [DataMember]
        public string PreAddress2 { get; set; }

        [DataMember]
        public string PreState { get; set; }

        [DataMember]
        public string PreDistrict { get; set; }

        [DataMember]
        public string PrePIN { get; set; }

        [DataMember]
        public string SameAddYN { get; set; }

        [DataMember]
        public string PerAddress1 { get; set; }

        [DataMember]
        public string PerAddress2 { get; set; }

        [DataMember]
        public string PerDistrict { get; set; }

        [DataMember]
        public string PerState { get; set; }

        [DataMember]
        public string PerPIN { get; set; }

        [DataMember]
        public string IsDirector { get; set; }

        [DataMember]
        public string IsPartner { get; set; }

        [DataMember]
        public string IsPropietor { get; set; }

        [DataMember]
        public string IsSpouse { get; set; }

        [DataMember]
        public string IsSameAddAsApp { get; set; }

        [DataMember]
        public string IsGuarantor { get; set; }

        [DataMember]
        public string IsActive { get; set; }

        [DataMember]
        public string ShareHolPer { get; set; }

        [DataMember]
        public string CoAppType { get; set; }

        [DataMember]
        public string CompTypeID { get; set; }

        [DataMember]
        public string PropertyTypeId { get; set; }

        [DataMember]
        public string AppName { get; set; }

        [DataMember]
        public string IsPrimaryCoAppYN { get; set; }

        [DataMember]
        public string CustCoAppRel { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string RelativeName { get; set; }

        [DataMember]
        public string PhyChallangeYN { get; set; }

        [DataMember]
        public string ACHolderName { get; set; }

        [DataMember]
        public string ACNo { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string IFSCCode { get; set; }

        [DataMember]
        public string YrOfOpening { get; set; }

        [DataMember]
        public string AccountType { get; set; }

        [DataMember]
        public string EmpOrgName { get; set; }

        [DataMember]
        public string EmpDesig { get; set; }

        [DataMember]
        public string EmpRetiredAge { get; set; }

        [DataMember]
        public string EmpCode { get; set; }

        [DataMember]
        public string EmpCurExp { get; set; }

        [DataMember]
        public string EmpTotExp { get; set; }

        [DataMember]
        public string BusGSTAppYN { get; set; }

        [DataMember]
        public string BusGSTNo { get; set; }

        [DataMember]
        public string BusLandMark { get; set; }

        [DataMember]
        public string BusAddress1 { get; set; }

        [DataMember]
        public string BusAddress2 { get; set; }

        [DataMember]
        public string BusLocality { get; set; }

        [DataMember]
        public string BusCity { get; set; }

        [DataMember]
        public string BusPIN { get; set; }

        [DataMember]
        public string BusState { get; set; }

        [DataMember]
        public string BusMob { get; set; }

        [DataMember]
        public string BusPhone { get; set; }

        [DataMember]
        public string CommunAddress { get; set; }

        [DataMember]
        public string ResidentialStatus { get; set; }

        public GetCoAppDetlById(string CoApplicantId, string CoApplicantNo, string CustId, string DOA, string DOB,
            string CoAppName, string CastId, string ReligionId, string OccupationId, string MaritalStatus, string Gender,
            string Age, string Qualification, string PhotoId, string PhotoIdNo, string AddressId, string AddressIdNo,
            string ContMNo, string ContTelNo, string ContFAXNo, string Email, string YearAtRes, string YearAtBus,
            string PreAddress1, string PreAddress2, string PreState, string PreDistrict, string PrePIN, string SameAddYN,
            string PerAddress1, string PerAddress2, string PerDistrict, string PerState, string PerPIN, string IsDirector,
            string IsPartner, string IsPropietor, string IsSpouse, string IsSameAddAsApp, string IsGuarantor, string IsActive,
            string ShareHolPer, string CoAppType, string CompTypeID, string PropertyTypeId, string AppName, string IsPrimaryCoAppYN,
            string CustCoAppRel, string RelationId, string RelativeName, string PhyChallangeYN, string ACHolderName, string ACNo, string BankName,
            string IFSCCode, string YrOfOpening, string AccountType, string EmpOrgName, string EmpDesig, string EmpRetiredAge, string EmpCode,
            string EmpCurExp, string EmpTotExp, string BusGSTAppYN, string BusGSTNo, string BusLandMark, string BusAddress1, string BusAddress2,
            string BusLocality, string BusCity, string BusPIN, string BusState, string BusMob, string BusPhone, string CommunAddress, string ResidentialStatus)
        {
            this.CoApplicantId = CoApplicantId;
            this.CoApplicantNo = CoApplicantNo;
            this.CustId = CustId;
            this.DOA = DOA;
            this.DOB = DOB;
            this.CoAppName = CoAppName;
            this.CastId = CastId;
            this.ReligionId = ReligionId;
            this.OccupationId = OccupationId;
            this.MaritalStatus = MaritalStatus;
            this.Gender = Gender;
            this.Age = Age;
            this.Qualification = Qualification;
            this.PhotoId = PhotoId;
            this.PhotoIdNo = PhotoIdNo;
            this.AddressId = AddressId;
            this.AddressIdNo = AddressIdNo;
            this.ContMNo = ContMNo;
            this.ContTelNo = ContTelNo;
            this.ContFAXNo = ContFAXNo;
            this.Email = Email;
            this.YearAtRes = YearAtRes;
            this.YearAtBus = YearAtBus;
            this.PreAddress1 = PreAddress1;
            this.PreAddress2 = PreAddress2;
            this.PerDistrict = PerDistrict;
            this.PerState = PerState;
            this.PerPIN = PerPIN;
            this.PreState = PreState;
            this.PreDistrict = PreDistrict;
            this.PrePIN = PrePIN;
            this.SameAddYN = SameAddYN;
            this.PerAddress1 = PerAddress1;
            this.PerAddress2 = PerAddress2;
            this.IsDirector = IsDirector;
            this.IsPartner = IsPartner;
            this.IsPropietor = IsPropietor;
            this.IsSpouse = IsSpouse;
            this.IsSameAddAsApp = IsSameAddAsApp;
            this.IsGuarantor = IsGuarantor;
            this.IsActive = IsActive;
            this.ShareHolPer = ShareHolPer;
            this.CoAppType = CoAppType;
            this.CompTypeID = CompTypeID;
            this.PropertyTypeId = PropertyTypeId;
            this.AppName = AppName;
            this.IsPrimaryCoAppYN = IsPrimaryCoAppYN;
            this.CustCoAppRel = CustCoAppRel;
            this.RelationId = RelationId;
            this.RelativeName = RelativeName;
            this.PhyChallangeYN = PhyChallangeYN;
            this.ACHolderName = ACHolderName;
            this.ACNo = ACNo;
            this.BankName = BankName;
            this.IFSCCode = IFSCCode;
            this.YrOfOpening = YrOfOpening;
            this.AccountType = AccountType;
            this.EmpOrgName = EmpOrgName;
            this.EmpDesig = EmpDesig;
            this.EmpRetiredAge = EmpRetiredAge;
            this.EmpCode = EmpCode;
            this.EmpCurExp = EmpCurExp;
            this.EmpTotExp = EmpTotExp;
            this.BusGSTAppYN = BusGSTAppYN;
            this.BusGSTNo = BusGSTNo;
            this.BusLandMark = BusLandMark;
            this.BusAddress1 = BusAddress1;
            this.BusAddress2 = BusAddress2;
            this.BusLocality = BusLocality;
            this.BusCity = BusCity;
            this.BusPIN = BusPIN;
            this.BusState = BusState;
            this.BusMob = BusMob;
            this.BusPhone = BusPhone;
            this.CommunAddress = CommunAddress;
            this.ResidentialStatus = ResidentialStatus;
        }
    }

    [DataContract]
    public class GetCoAppDependentInfo
    {
        [DataMember]
        public string SLNo { get; set; }

        [DataMember]
        public string CoAppId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string OccupationId { get; set; }


        public GetCoAppDependentInfo(string SLNo, string CoAppId, string Name, string RelationId, string Age, string OccupationId)
        {
            this.SLNo = SLNo;
            this.CoAppId = CoAppId;
            this.Name = Name;
            this.RelationId = RelationId;
            this.Age = Age;
            this.OccupationId = OccupationId;

        }
    }

    [DataContract]
    public class GetCoAppAndDepAllInfo
    {
        [DataMember]
        public List<GetCoAppDetlById> GetCoAppDetlById { get; set; }

        [DataMember]
        public List<GetCoAppDependentInfo> GetCoAppDependentInfo { get; set; }

        public GetCoAppAndDepAllInfo(List<GetCoAppDetlById> GetCoAppDetlById, List<GetCoAppDependentInfo> GetCoAppDependentInfo)
        {
            this.GetCoAppDetlById = GetCoAppDetlById;
            this.GetCoAppDependentInfo = GetCoAppDependentInfo;
        }
    }

    #endregion

    #region KYCImageSave
    [DataContract]
    public class KYCImageSave
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string FileName { get; set; }

        public KYCImageSave(string Status, string FileName)
        {
            this.Status = Status;
            this.FileName = FileName;
        }
    }

    #endregion

    #region GetCompanyData

    [DataContract]
    public class ComboMisData
    {
        [DataMember]
        public string SourceID { get; set; }

        [DataMember]
        public string SourceName { get; set; }

        public ComboMisData(string SourceID, string SourceName)
        {
            this.SourceID = SourceID;
            this.SourceName = SourceName;
        }
    }

    #endregion

    #region LoanSchemeData

    [DataContract]
    public class LoanSchemeData
    {
        [DataMember]
        public string RowId { get; set; }

        [DataMember]
        public string LoanTypeId { get; set; }

        [DataMember]
        public string LoanTypeName { get; set; }

        [DataMember]
        public string LoanAmt { get; set; }

        [DataMember]
        public string New_Prod { get; set; }

        public LoanSchemeData(string RowId, string LoanTypeId, string LoanTypeName, string LoanAmt, string New_Prod)
        {
            this.RowId = RowId;
            this.LoanTypeId = LoanTypeId;
            this.LoanTypeName = LoanTypeName;
            this.LoanAmt = LoanAmt;
            this.New_Prod = New_Prod;
        }
    }

    #endregion

    #region LoanAppCoListData

    [DataContract]
    public class LoanAppCoListData
    {
        [DataMember]
        public string CoApplicantId { get; set; }

        [DataMember]
        public string CoAppName { get; set; }

        [DataMember]
        public string ScoreValue { get; set; }

        [DataMember]
        public string ReportID { get; set; }

        public LoanAppCoListData(string CoApplicantId, string CoAppName, string ScoreValue, string ReportID)
        {
            this.CoApplicantId = CoApplicantId;
            this.CoAppName = CoAppName;
            this.ScoreValue = ScoreValue;
            this.ReportID = ReportID;
        }
    }

    #endregion

    #region GetAppCustLoanAppData

    [DataContract]
    public class GetAppCustLoanAppData
    {
        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CustNo { get; set; }

        [DataMember]
        public string LeadId { get; set; }

        [DataMember]
        public string LoanAppId { get; set; }

        [DataMember]
        public string LoanAppNo { get; set; }

        [DataMember]
        public string ApplicationDt { get; set; }

        [DataMember]
        public string AppAmount { get; set; }

        [DataMember]
        public string Tenure { get; set; }

        [DataMember]
        public string PurposeID { get; set; }

        [DataMember]
        public string LoanTypeId { get; set; }

        [DataMember]
        public string CBPassYN { get; set; }

        [DataMember]
        public string CBPassDate { get; set; }

        [DataMember]
        public string SourceID { get; set; }

        [DataMember]
        public string PassorRejDate { get; set; }

        [DataMember]
        public string RejReason { get; set; }

        [DataMember]
        public string ScoreValue { get; set; }

        [DataMember]
        public string Mode { get; set; }


        public GetAppCustLoanAppData(string CustId, string CompanyName, string CustNo, string LeadId, string LoanAppId, string LoanAppNo, string ApplicationDt,
            string AppAmount, string Tenure, string PurposeID, string LoanTypeId, string CBPassYN, string CBPassDate, string SourceID, string PassorRejDate,
            string RejReason, string ScoreValue, string Mode)
        {
            this.CustId = CustId;
            this.CompanyName = CompanyName;
            this.CustNo = CustNo;
            this.LeadId = LeadId;
            this.LoanAppId = LoanAppId;
            this.LoanAppNo = LoanAppNo;
            this.ApplicationDt = ApplicationDt;
            this.AppAmount = AppAmount;
            this.Tenure = Tenure;
            this.PurposeID = PurposeID;
            this.LoanTypeId = LoanTypeId;
            this.CBPassYN = CBPassYN;
            this.CBPassDate = CBPassDate;
            this.SourceID = SourceID;
            this.PassorRejDate = PassorRejDate;
            this.RejReason = RejReason;
            this.ScoreValue = ScoreValue;
            this.Mode = Mode;
        }
    }

    #endregion

    #region PopResult
    [DataContract]
    public class PopResult
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public string Message { get; set; }
        public PopResult(string Result, string Message)
        {
            this.Result = Result;
            this.Message = Message;
        }
    }
    #endregion

    #region PendingPDBucketListData

    [DataContract]
    public class PendingPDBucketListData
    {
        [DataMember]
        public string LoanAppId { get; set; }

        [DataMember]
        public string LoanAppNo { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CustNo { get; set; }

        [DataMember]
        public string LoanTypeName { get; set; }

        [DataMember]
        public string PurposeName { get; set; }

        [DataMember]
        public string ApplicationDt { get; set; }

        [DataMember]
        public string AppAmount { get; set; }

        [DataMember]
        public string Tenure { get; set; }


        public PendingPDBucketListData(string LoanAppId, string LoanAppNo, string CompanyName, string CustNo, string LoanTypeName, string PurposeName, string ApplicationDt,
            string AppAmount, string Tenure)
        {
            this.LoanAppId = LoanAppId;
            this.LoanAppNo = LoanAppNo;
            this.CompanyName = CompanyName;
            this.CustNo = CustNo;
            this.LoanTypeName = LoanTypeName;
            this.PurposeName = PurposeName;
            this.ApplicationDt = ApplicationDt;
            this.AppAmount = AppAmount;
            this.Tenure = Tenure;
        }
    }

    #endregion

    #region PDDetails

    [DataContract]
    public class AppDetailsData
    {
        [DataMember]
        public string LoanAppId { get; set; }

        [DataMember]
        public string LoanAppNo { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string LoanTypeName { get; set; }

        [DataMember]
        public string PurposeName { get; set; }

        [DataMember]
        public string ApplicationDt { get; set; }

        [DataMember]
        public string AppAmount { get; set; }

        [DataMember]
        public string Tenure { get; set; }

        [DataMember]
        public string SanctionYN { get; set; }

        [DataMember]
        public string FinalPDStatus { get; set; }

        [DataMember]
        public string FinalPDRemarks { get; set; }

        [DataMember]
        public string FinalPDDate { get; set; }



        public AppDetailsData(string LoanAppId, string LoanAppNo, string CompanyName, string CustId, string LoanTypeName, string PurposeName,
            string ApplicationDt, string AppAmount, string Tenure, string SanctionYN, string FinalPDStatus, string FinalPDRemarks, string FinalPDDate)
        {
            this.LoanAppId = LoanAppId;
            this.LoanAppNo = LoanAppNo;
            this.CompanyName = CompanyName;
            this.CustId = CustId;
            this.LoanTypeName = LoanTypeName;
            this.PurposeName = PurposeName;
            this.ApplicationDt = ApplicationDt;
            this.AppAmount = AppAmount;
            this.Tenure = Tenure;
            this.SanctionYN = SanctionYN;
            this.FinalPDStatus = FinalPDStatus;
            this.FinalPDRemarks = FinalPDRemarks;
            this.FinalPDDate = FinalPDDate;

        }
    }

    [DataContract]
    public class CoAppDetailsData
    {
        [DataMember]
        public string CoApplicantId { get; set; }

        [DataMember]
        public string CoAppName { get; set; }


        public CoAppDetailsData(string CoApplicantId, string CoAppName)
        {
            this.CoApplicantId = CoApplicantId;
            this.CoAppName = CoAppName;
        }
    }

    [DataContract]
    public class PDDetails
    {
        [DataMember]
        public List<AppDetailsData> AppDetails { get; set; }

        [DataMember]
        public List<CoAppDetailsData> CoAppDetails { get; set; }

        public PDDetails(List<AppDetailsData> AppDetails, List<CoAppDetailsData> CoAppDetails)
        {
            this.AppDetails = AppDetails;
            this.CoAppDetails = CoAppDetails;
        }
    }

    #endregion

    #region DesigData

    [DataContract]
    public class DesigData
    {
        [DataMember]
        public string Desig { get; set; }

        public DesigData(string Desig)
        {
            this.Desig = Desig;
        }
    }

    #endregion

    #region CustRecoveryData

    [DataContract]
    public class CustRecoveryData
    {
        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string CustName { get; set; }

        public CustRecoveryData(string CustId, string CustName)
        {
            this.CustId = CustId;
            this.CustName = CustName;
        }
    }

    #endregion

    #region GetLoanNoData

    [DataContract]
    public class GetLoanNoData
    {
        [DataMember]
        public string LoanId { get; set; }

        [DataMember]
        public string LoanNo { get; set; }

        public GetLoanNoData(string LoanId, string LoanNo)
        {
            this.LoanId = LoanId;
            this.LoanNo = LoanNo;
        }
    }

    #endregion

    #region GetCollData

    [DataContract]
    public class GetCollData
    {
        [DataMember]
        public string ClosingType { get; set; }

        [DataMember]
        public string LoanId { get; set; }

        [DataMember]
        public string DisbDate { get; set; }

        [DataMember]
        public string IntRate { get; set; }

        [DataMember]
        public string DisbAmt { get; set; }

        [DataMember]
        public string PrincpalDue { get; set; }

        [DataMember]
        public string InterestDue { get; set; }

        [DataMember]
        public string PrncOutStd { get; set; }

        [DataMember]
        public string IntOutStd { get; set; }

        [DataMember]
        public string PaidPric { get; set; }

        [DataMember]
        public string PaidInt { get; set; }

        [DataMember]
        public string BounceDue { get; set; }

        [DataMember]
        public string LoanTypeId { get; set; }

        [DataMember]
        public string LoanTypeName { get; set; }

        [DataMember]
        public string FunderName { get; set; }

        [DataMember]
        public string LastTransDt { get; set; }

        [DataMember]
        public string EMIAmt { get; set; }

        [DataMember]
        public string FlDGBal { get; set; }

        [DataMember]
        public string IntDue { get; set; }

        [DataMember]
        public string CollMode { get; set; }

        [DataMember]
        public string PenaltyAmt { get; set; }
        [DataMember]
        public string PenCGST { get; set; }
        [DataMember]
        public string PenSGST { get; set; }
        [DataMember]
        public string VisitCharge { get; set; }
        [DataMember]
        public string VisitChargeCGST { get; set; }
        [DataMember]
        public string VisitChargeSGST { get; set; }
        [DataMember]
        public string GrandTotal { get; set; }

        public GetCollData(string ClosingType, string LoanId, string DisbDate, string IntRate, string DisbAmt, string PrincpalDue, string InterestDue,
            string PrncOutStd, string IntOutStd, string PaidPric, string PaidInt, string BounceDue, string LoanTypeId, string LoanTypeName, string FunderName,
            string LastTransDt, string EMIAmt, string FlDGBal, string IntDue, string CollMode, string PenaltyAmt, string PenCGST, string PenSGST,
            string VisitCharge, string VisitChargeCGST, string VisitChargeSGST, string GrandTotal)
        {
            this.ClosingType = ClosingType;
            this.LoanId = LoanId;
            this.DisbDate = DisbDate;
            this.IntRate = IntRate;
            this.DisbAmt = DisbAmt;
            this.PrincpalDue = PrincpalDue;
            this.InterestDue = InterestDue;
            this.PrncOutStd = PrncOutStd;
            this.IntOutStd = IntOutStd;
            this.PaidPric = PaidPric;
            this.PaidInt = PaidInt;
            this.BounceDue = BounceDue;
            this.LoanTypeId = LoanTypeId;
            this.LoanTypeName = LoanTypeName;
            this.FunderName = FunderName;
            this.LastTransDt = LastTransDt;
            this.EMIAmt = EMIAmt;
            this.FlDGBal = FlDGBal;
            this.IntDue = IntDue;
            this.CollMode = CollMode;

            this.PenaltyAmt = PenaltyAmt;
            this.PenCGST = PenCGST;
            this.PenSGST = PenSGST;
            this.VisitCharge = VisitCharge;
            this.VisitChargeCGST = VisitChargeCGST;
            this.VisitChargeSGST = VisitChargeSGST;
            this.GrandTotal = GrandTotal;
        }
    }

    #endregion

    #region AsonDateData

    [DataContract]
    public class AsonDateData
    {
        [DataMember]
        public string InterestDue { get; set; }

        [DataMember]
        public string PreCloseCharge { get; set; }

        [DataMember]
        public string PreCloseCGST { get; set; }

        [DataMember]
        public string PreCloseSGST { get; set; }

        public AsonDateData(string InterestDue, string PreCloseCharge, string PreCloseCGST, string PreCloseSGST)
        {
            this.InterestDue = InterestDue;
            this.PreCloseCharge = PreCloseCharge;
            this.PreCloseCGST = PreCloseCGST;
            this.PreCloseSGST = PreCloseSGST;
        }
    }

    #endregion

    #region Karza

    [DataContract]
    public class Result
    {
        public string message { get; set; }
    }

    public class KYCVoterIDResponse
    {
        public string request_id { get; set; }
        public string status_code { get; set; }
        public KYCVoterIDResponseResult result { get; set; }
    }

    public class KYCVoterIDResponseResult
    {
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string part_no { get; set; }
        public string rln_type { get; set; }
        public string section_no { get; set; }
        public string id { get; set; }
        public string epic_no { get; set; }
        public string rln_name { get; set; }
        public string district { get; set; }
        public string last_update { get; set; }
        public string state { get; set; }
        public string ac_no { get; set; }
        public string slno_inpart { get; set; }
        public string ps_name { get; set; }
        public string pc_name { get; set; }
        public string house_no { get; set; }
        public string name { get; set; }
        public string part_name { get; set; }
        public string st_code { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string ac_name { get; set; }
        public string name_v1 { get; set; }
        public string dob { get; set; }
        public string name_v3 { get; set; }
        public string name_v2 { get; set; }
    }

    [DataContract]
    public class AadhaarOTPResponse
    {
        [DataMember]
        public string requestId { get; set; }
        [DataMember]
        public Result result { get; set; }
        [DataMember]
        public int statusCode { get; set; }

        public AadhaarOTPResponse(string requestId, Result result, int statusCode)
        {
            this.requestId = requestId;
            this.result = result;
            this.statusCode = statusCode;
        }
    }

    [DataContract]
    public class MatchResponse
    {
        [DataMember]
        public double score { get; set; }
        [DataMember]
        public bool result { get; set; }
        [DataMember]
        public string requestId { get; set; }
        [DataMember]
        public string statuscode { get; set; }

        public MatchResponse(double score, bool result, string requestId, string statuscode)
        {
            this.score = score;
            this.result = result;
            this.requestId = requestId;
            this.statuscode = statuscode;
        }
    }

    #endregion

    [DataContract]
    public class CategoryVariables
    {
        [DataMember]
        public string CategoryID { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string VariableId { get; set; }
        [DataMember]
        public string Variable { get; set; }
        [DataMember]
        public string Options { get; set; }


        public CategoryVariables(string CategoryID, string Category, string VariableId, string Variable, string Options)
        {
            this.CategoryID = CategoryID;
            this.Category = Category;
            this.VariableId = VariableId;
            this.Variable = Variable;
            this.Options = Options;
        }
    }

    [DataContract]
    public class LoginData
    {
        [DataMember]
        public List<EmpData> EmpData { get; set; }

        [DataMember]
        public List<PermissionData> PermissionData { get; set; }

        public LoginData(List<EmpData> EmpData, List<PermissionData> PermissionData)
        {
            this.EmpData = EmpData;
            this.PermissionData = PermissionData;
        }
    }

    [DataContract]
    public class PermissionData
    {
        [DataMember]
        public string RoleID { get; set; }

        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public string MenuID { get; set; }

        [DataMember]
        public string MenuName { get; set; }

        [DataMember]
        public string AllowView { get; set; }

        [DataMember]
        public string AllowAdd { get; set; }

        [DataMember]
        public string AllowEdit { get; set; }

        [DataMember]
        public string AllowDel { get; set; }

        public PermissionData(string RoleID, string Role, string MenuID, string MenuName, string AllowView, string AllowAdd, string AllowEdit, string AllowDel)
        {
            this.RoleID = RoleID;
            this.Role = Role;
            this.MenuID = MenuID;
            this.MenuName = MenuName;
            this.AllowView = AllowView;
            this.AllowAdd = AllowAdd;
            this.AllowEdit = AllowEdit;
            this.AllowDel = AllowDel;
        }
    }

    #region PDQuestionAnswerMst

    [DataContract]
    public class PDQuestionAnswerMst
    {
        [DataMember]
        public string LoanAppId { get; set; }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CustType { get; set; }

        public PDQuestionAnswerMst(string LoanAppId, string ID, string Name, string CustType)
        {
            this.LoanAppId = LoanAppId;
            this.ID = ID;
            this.Name = Name;
            this.CustType = CustType;
        }
    }

    #endregion

    [DataContract]
    public class DocTypeMst
    {
        [DataMember]
        public string DocTypeId { get; set; }
        [DataMember]
        public string DocTypeName { get; set; }

        public DocTypeMst(string DocTypeId, string DocTypeName)
        {
            this.DocTypeId = DocTypeId;
            this.DocTypeName = DocTypeName;
        }
    }

    [DataContract]
    public class CustomerList
    {
        [DataMember]
        public string CustName { get; set; }
        [DataMember]
        public string LoanAppId { get; set; }

        public CustomerList(string CustName, string LoanAppId)
        {
            this.CustName = CustName;
            this.LoanAppId = LoanAppId;
        }
    }

    public class RelationMst
    {
        public string Relation { get; set; }
        public string RelationId { get; set; }
    }

    public class MaritalStatusMst
    {
        public string MaritalName { get; set; }
        public string MaritalId { get; set; }
    }

    public class PropOwnerShipMst
    {
        public string PropOwnerShip { get; set; }
        public string PropOwnerShipId { get; set; }
    }

    public class PropertypeMst
    {
        public string PropertypeName { get; set; }
        public string PropertyTypeID { get; set; }
    }

    public class BusinessTypeMst
    {
        public string BusinessType { get; set; }
        public string BusinessTypeId { get; set; }
    }

    public class SalCredModeMst
    {
        public string SalCredMode { get; set; }
        public string SalCredModeId { get; set; }
    }

    public class SalTypeMst
    {
        public string SalType { get; set; }
        public string SalTypeId { get; set; }

    }

    public class PurposeMst
    {
        public string PurposeName { get; set; }
        public string PurposeId { get; set; }
    }

    public class PenIncomeMst
    {
        public string PenIncome { get; set; }
        public string PenIncomeId { get; set; }
    }

    public class PensionFromMst
    {
        public string PensionFrom { get; set; }
        public string PensionFromId { get; set; }
    }

    public class WorkTypeMst
    {
        public string WorkType { get; set; }
        public string WorkTypeId { get; set; }
    }

    public class PropNatureMst
    {
        public string PropNature { get; set; }
        public string PropNatureId { get; set; }
    }

    public class PropJudMst
    {
        public string PropJud { get; set; }
        public string PropJudId { get; set; }
    }

    public class ChildMst
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class EraningMemberMst
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class SchoolTypMst
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class OccupationMst
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class PDMasterDetails
    {
        public List<RelationMst> RelationMst { get; set; }
        public List<MaritalStatusMst> MaritalStatusMst { get; set; }
        public List<PropOwnerShipMst> PropOwnerShipMst { get; set; }
        public List<PropertypeMst> PropertypeMst { get; set; }

        public List<BusinessTypeMst> BusinessTypeMst { get; set; }
        public List<SalCredModeMst> SalCredModeMst { get; set; }
        public List<SalTypeMst> SalTypeMst { get; set; }
        public List<PurposeMst> PurposeMst { get; set; }

        public List<PenIncomeMst> PenIncomeMst { get; set; }
        public List<PensionFromMst> PensionFromMst { get; set; }
        public List<WorkTypeMst> WorkTypeMst { get; set; }
        public List<PropNatureMst> PropNatureMst { get; set; }

        public List<PropJudMst> PropJudMst { get; set; }
        public List<ChildMst> ChildMst { get; set; }
        public List<EraningMemberMst> EraningMemberMst { get; set; }
        public List<SchoolTypMst> SchoolTypMst { get; set; }

        public List<OccupationMst> OccupationMst { get; set; }
        public List<ChildMst> BusinessSatbilityMst { get; set; }
        public List<ChildMst> AppRelationMst { get; set; }
        public List<ChildMst> BehaviourMst { get; set; }

        public List<ChildMst> AreaOfLandMst { get; set; }
        public List<ChildMst> TypeOfCropsMst { get; set; }
        public List<ChildMst> IncomeFrequencyMst { get; set; }
    }

    [DataContract]
    public class DigiDocResponse
    {
        [DataMember]
        public byte[] vPDFFile { get; set; }
        [DataMember]
        public string vNSDLResponse { get; set; }

        public DigiDocResponse(byte[] vPDFFile, string vNSDLResponse)
        {
            this.vNSDLResponse = vNSDLResponse;
            this.vPDFFile = vPDFFile;
        }
    }

    #region AllExternalImageUpload
    [DataContract]
    public class AllExternalImageUpload
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string FileName { get; set; }

        public AllExternalImageUpload(string Status, string FileName)
        {
            this.Status = Status;
            this.FileName = FileName;
        }
    }

    #endregion

    [DataContract]
    public class DownSchResponse
    {
        [DataMember]
        public byte[] vPDFFile { get; set; }
        [DataMember]
        public string vResponse { get; set; }

        public DownSchResponse(byte[] vPDFFile, string vResponse)
        {
            this.vResponse = vResponse;
            this.vPDFFile = vPDFFile;
        }
    }

    #region "NEW WORK"
    [DataContract]
    public class InitiateDigitalDocResponse
    {
        [DataMember]
        public string vSuccessCode { get; set; }
        [DataMember]
        public string vSuccessMsg { get; set; }
        [DataMember]
        public string vURL { get; set; }
        [DataMember]
        public string vToken { get; set; }

        public InitiateDigitalDocResponse(string vSuccessCode, string vSuccessMsg, string vURL, string vToken)
        {
            this.vSuccessCode = vSuccessCode;
            this.vSuccessMsg = vSuccessMsg;
            this.vURL = vURL;
            this.vToken = vToken;
        }
    }
    #endregion

    #region IFSCData
    [DataContract]
    public class IFSCData
    {
        [DataMember]
        public string BANKNAME { get; set; }

        [DataMember]
        public string BRANCHNAME { get; set; }

        public IFSCData(string BANKNAME, string BRANCHNAME)
        {
            this.BANKNAME = BANKNAME;
            this.BRANCHNAME = BRANCHNAME;
        }
    }
    #endregion

    #region DataFormResponse
    [DataContract]
    public class DataFormResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }
        [DataMember]
        public string LoanId { get; set; }

        public DataFormResponse(string StatusCode, string Status, string StatusDesc, string LoanId)
        {
            this.StatusCode = StatusCode;
            this.Status = Status;
            this.StatusDesc = StatusDesc;
            this.LoanId = LoanId;
        }
    }
    #endregion

    #region AadhaarVaultResponse
    [DataContract]
    public class AadhaarVaultResponse
    {
        [DataMember]
        public string action { get; set; }
        [DataMember]
        public int response_code { get; set; }
        [DataMember]
        public string response_message { get; set; }
        [DataMember]
        public int total_size { get; set; }
        [DataMember]
        public int total_pages { get; set; }
        [DataMember]
        public List<string> results { get; set; }

        public AadhaarVaultResponse(string action, int response_code, string response_message, int total_size, List<string> results)
        {
            this.action = action;
            this.response_code = response_code;
            this.response_message = response_message;
            this.total_size = total_size;
            this.results = results;
        }

    }
    #endregion

    #region BackAccountVerify
    [DataContract]
    public class FingPayResponse
    {
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public int StatusCode { get; set; }
        [DataMember]
        public string BeneName { get; set; }
        [DataMember]
        public string ReferrenceNo { get; set; }

        public FingPayResponse(bool Status, string StatusMessage, int StatusCode, string BeneName, string ReferrenceNo)
        {
            this.Status = Status;
            this.StatusMessage = StatusMessage;
            this.StatusCode = StatusCode;
            this.BeneName = BeneName;
            this.ReferrenceNo = ReferrenceNo;
        }
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

    [DataContract]
    public class ServerStatus
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }
    }

    [DataContract]
    public class ProsidexResponse
    {
        [DataMember]
        public string RequestId { get; set; }
        [DataMember]
        public string UCIC { get; set; }
        [DataMember]
        public int response_code { get; set; }

        public ProsidexResponse(string RequestId, string UCIC, int response_code)
        {
            this.RequestId = RequestId;
            this.UCIC = UCIC;
            this.response_code = response_code;
        }
    }

    public class DG
    {
        public string ACCOUNT_NUMBER { get; set; }
        public string ALIAS_NAME { get; set; }
        public string APPLICATIONID { get; set; }
        public string Aadhar { get; set; }
        public string CIN { get; set; }
        public string CKYC { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string CUST_ID { get; set; }
        public string DOB { get; set; }
        public string DrivingLicense { get; set; }
        public string Father_First_Name { get; set; }
        public string Father_Last_Name { get; set; }
        public string Father_Middle_Name { get; set; }
        public string Father_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string GSTIN { get; set; }
        public string Lead_Id { get; set; }
        public string NREGA { get; set; }
        public string Pan { get; set; }
        public string PassportNo { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RationCard { get; set; }
        public string Registration_NO { get; set; }
        public string SALUTATION { get; set; }
        public string TAN { get; set; }
        public string Udyam_aadhar_number { get; set; }
        public string VoterId { get; set; }
        public string Tasc_Customer { get; set; }
    }

    public class Request
    {
        public DG DG { get; set; }
        public List<object> ACE { get; set; }
        public string UnitySfb_RequestId { get; set; }
        public string CUST_TYPE { get; set; }
        public string CustomerCategory { get; set; }
        public string MatchingRuleProfile { get; set; }
        public string Req_flag { get; set; }
        public string SourceSystem { get; set; }
    }

    public class ProsidexRequest
    {
        public Request Request { get; set; }
    }

    public class ProsiReq
    {
        public string pMemberId { get; set; }
        public string pCreatedBy { get; set; }
    }

    [DataContract]
    public class IdfyAadharVerifyData
    {
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string request_id { get; set; }

        public IdfyAadharVerifyData(string url, string request_id)
        {
            this.url = url;
            this.request_id = request_id;
        }
    }

    #region ForgotPassword
    [DataContract]
    public class ForgotOTPRes
    {
        [DataMember]
        public string OTP { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }
        [DataMember]
        public string OTPId { get; set; }

        public ForgotOTPRes(string OTP, string StatusCode, string StatusDesc, string OTPId)
        {
            this.OTP = OTP;
            this.StatusCode = StatusCode;
            this.StatusDesc = StatusDesc;
            this.OTPId = OTPId;
        }
    }

    #endregion

    [DataContract]
    public class IBMAadhaarResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusMsg { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }

        public IBMAadhaarResponse(string StatusCode, string Status, string StatusMsg, string ErrorMsg)
        {
            this.StatusCode = StatusCode;
            this.Status = Status;
            this.StatusMsg = StatusMsg;
            this.ErrorMsg = ErrorMsg;
        }
    }


    [DataContract]
    public class IBMAadhaarOTPResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string txn { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusMsg { get; set; }
        [DataMember]
        public string ErrorMsg { get; set; }

        public IBMAadhaarOTPResponse(string StatusCode, string txn, string Status, string StatusMsg, string ErrorMsg)
        {
            this.StatusCode = StatusCode;
            this.txn = txn;
            this.Status = Status;
            this.StatusMsg = StatusMsg;
            this.ErrorMsg = ErrorMsg;
        }
    }

    [DataContract]
    public class GetBranchCtrlByBranchCode
    {
        [DataMember]
        public string pAdvCollMEL { get; set; }

        [DataMember]
        public string pCashCollMEL { get; set; }

        [DataMember]
        public string pDigiAuthMEL { get; set; }

        [DataMember]
        public string pManualAuthMEL { get; set; }

        [DataMember]
        public string pBioAuthMEL { get; set; }



        public GetBranchCtrlByBranchCode(string pAdvCollMEL, string pCashCollMEL, string pDigiAuthMEL, string pManualAuthMEL, string pBioAuthMEL)
        {
            this.pAdvCollMEL = pAdvCollMEL;
            this.pCashCollMEL = pCashCollMEL;
            this.pDigiAuthMEL = pDigiAuthMEL;
            this.pManualAuthMEL = pManualAuthMEL;
            this.pBioAuthMEL = pBioAuthMEL;
        }

    }
}