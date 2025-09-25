using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;

namespace UnityVriddhiMobService
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
        public string AllowManualEntry { get; set; }

        [DataMember]
        public string AllowQRScan { get; set; }

        [DataMember]
        public string LoginId { get; set; }

        [DataMember]
        public string MFAYN { get; set; }

        [DataMember]
        public string MFAOTP { get; set; }

        public EmpData(string EoName, string BranchCode, string UserID, string Eoid, string LogStatus, string DayEndDate, string AttStatus, string AttFlag,
            string AreaCat, string Designation, string AllowManualEntry, string AllowQRScan, string LoginId, string MFAYN, string MFAOTP)
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
            this.AllowManualEntry = AllowManualEntry;
            this.AllowQRScan = AllowQRScan;
            this.LoginId = LoginId;
            this.MFAYN = MFAYN;
            this.MFAOTP = MFAOTP;
        }
    }
    #endregion

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
    public class OCRParameterResponse
    {
        [DataMember]
        public string Responsestatus { get; set; }
        [DataMember]
        public string ResponseString { get; set; }

        public OCRParameterResponse(string Responsestatus, string ResponseString)
        {
            this.Responsestatus = Responsestatus;
            this.ResponseString = ResponseString;
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

    [DataContract]
    public class KYCData
    {
        [DataMember]
        public string ItemID { get; set; }

        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public string ItemType { get; set; }

        public KYCData(string ItemID, string ItemName, string ItemType)
        {
            this.ItemID = ItemID;
            this.ItemName = ItemName;
            this.ItemType = ItemType;
        }
    }

    [DataContract]
    public class AddressData
    {
        [DataMember]
        public string VillageId { get; set; }

        [DataMember]
        public string VillageName { get; set; }

        [DataMember]
        public string GPId { get; set; }

        [DataMember]
        public string GPName { get; set; }

        [DataMember]
        public string BlockId { get; set; }

        [DataMember]
        public string BlockName { get; set; }

        [DataMember]
        public string DistrictId { get; set; }

        [DataMember]
        public string DistrictName { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        public AddressData(string VillageId, string VillageName, string GPId, string GPName, string BlockId, string BlockName, string DistrictId, string DistrictName,
            string StateId, string StateName, string ShortName)
        {
            this.VillageId = VillageId;
            this.VillageName = VillageName;
            this.GPId = GPId;
            this.GPName = GPName;
            this.BlockId = BlockId;
            this.BlockName = BlockName;
            this.DistrictId = DistrictId;
            this.DistrictName = DistrictName;
            this.StateId = StateId;
            this.StateName = StateName;
            this.ShortName = ShortName;
        }
    }


    [DataContract]
    public class StateData
    {
        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        public StateData(string StateId, string StateName, string ShortName)
        {
            this.StateId = StateId;
            this.StateName = StateName;
            this.ShortName = ShortName;
        }
    }

    [DataContract]
    public class MemberData
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Dob { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string GuardianName { get; set; }

        [DataMember]
        public string GuardianRelation { get; set; }

        [DataMember]
        public string Id1Type { get; set; }

        [DataMember]
        public string Id1No { get; set; }

        [DataMember]
        public string Id2Type { get; set; }

        [DataMember]
        public string Id2No { get; set; }

        [DataMember]
        public string AddressType1 { get; set; }

        [DataMember]
        public string HouseNo1 { get; set; }

        [DataMember]
        public string Street1 { get; set; }

        [DataMember]
        public string Area1 { get; set; }

        [DataMember]
        public string Village1 { get; set; }

        [DataMember]
        public string SubDistrict1 { get; set; }

        [DataMember]
        public string District1 { get; set; }

        [DataMember]
        public string State1 { get; set; }

        [DataMember]
        public string StateShortName { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string LandMark1 { get; set; }

        [DataMember]
        public string PostOffice1 { get; set; }

        [DataMember]
        public string PinCode1 { get; set; }

        [DataMember]
        public string MobileNo1 { get; set; }

        [DataMember]
        public string AddressType2 { get; set; }

        [DataMember]
        public string HouseNo2 { get; set; }

        [DataMember]
        public string Street2 { get; set; }

        [DataMember]
        public string Area2 { get; set; }

        [DataMember]
        public string VillageId2 { get; set; }

        [DataMember]
        public string SubDistrict2 { get; set; }

        [DataMember]
        public string LandMark2 { get; set; }

        [DataMember]
        public string PostOffice2 { get; set; }

        [DataMember]
        public string PinCode2 { get; set; }

        [DataMember]
        public string MobileNo2 { get; set; }

        [DataMember]
        public string BranchCode { get; set; }

        [DataMember]
        public string CoAppName { get; set; }

        [DataMember]
        public string CoAppDOB { get; set; }

        [DataMember]
        public string CoAppRelationId { get; set; }

        [DataMember]
        public string CoAppAddress { get; set; }

        [DataMember]
        public string CoAppState { get; set; }

        [DataMember]
        public string CoAppPinCode { get; set; }

        [DataMember]
        public string CoAppMobileNo { get; set; }

        [DataMember]
        public string CoAppIdentyPRofId { get; set; }

        [DataMember]
        public string CoAppIdentyProfNo { get; set; }

        [DataMember]
        public List<EarningMemberKYCData> EarningMemberKYCData { get; set; }


        public MemberData(string FirstName, string MiddleName, string LastName, string FullName, string Dob,
            string Age, string GuardianName, string GuardianRelation, string Id1Type, string Id1No, string Id2Type, string Id2No,
            string AddressType1, string HouseNo1, string Street1, string Area1, string Village1, string SubDistrict1,
            string District1, string State1, string StateShortName, string Address, string LandMark1, string PostOffice1,
            string PinCode1, string MobileNo1, string AddressType2, string HouseNo2, string Street2,
            string Area2, string VillageId2, string SubDistrict2, string LandMark2, string PostOffice2, string PinCode2,
            string MobileNo2, string BranchCode, string CoAppName, string CoAppDOB, string CoAppRelationId, string CoAppAddress,
            string CoAppState, string CoAppPinCode, string CoAppMobileNo, string CoAppIdentyPRofId, string CoAppIdentyProfNo,
            List<EarningMemberKYCData> EarningMemberKYCData)
        {
            this.FirstName = FirstName;
            this.MiddleName = MiddleName;
            this.LastName = LastName;
            this.FullName = FullName;
            this.Dob = Dob;
            this.Age = Age;
            this.GuardianName = GuardianName;
            this.GuardianRelation = GuardianRelation;

            this.Id1Type = Id1Type;
            this.Id1No = Id1No;
            this.Id2Type = Id2Type;
            this.Id2No = Id2No;
            this.AddressType1 = AddressType1;
            this.HouseNo1 = HouseNo1;
            this.Street1 = Street1;
            this.Area1 = Area1;

            this.Village1 = Village1;
            this.SubDistrict1 = SubDistrict1;
            this.District1 = District1;
            this.State1 = State1;
            this.StateShortName = StateShortName;
            this.Address = Address;
            this.LandMark1 = LandMark1;
            this.PostOffice1 = PostOffice1;

            this.PinCode1 = PinCode1;
            this.MobileNo1 = MobileNo1;
            this.AddressType2 = AddressType2;
            this.HouseNo2 = HouseNo2;
            this.Street2 = Street2;
            this.Area2 = Area2;
            this.VillageId2 = VillageId2;
            this.SubDistrict2 = SubDistrict2;
            this.LandMark2 = LandMark2;
            this.VillageId2 = VillageId2;
            this.PostOffice2 = PostOffice2;
            this.PinCode2 = PinCode2;

            this.MobileNo2 = MobileNo2;
            this.BranchCode = BranchCode;
            this.CoAppName = CoAppName;
            this.CoAppDOB = CoAppDOB;
            this.CoAppRelationId = CoAppRelationId;
            this.CoAppAddress = CoAppAddress;
            this.CoAppState = CoAppState;
            this.CoAppPinCode = CoAppPinCode;
            this.VillageId2 = VillageId2;
            this.PostOffice2 = PostOffice2;
            this.CoAppMobileNo = CoAppMobileNo;

            this.CoAppIdentyPRofId = CoAppIdentyPRofId;
            this.CoAppIdentyProfNo = CoAppIdentyProfNo;

            this.EarningMemberKYCData = EarningMemberKYCData;
        }
    }

    [DataContract]
    public class EarningMemberKYCData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DOB { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string PinCode { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public string IdentyPRofId { get; set; }

        [DataMember]
        public string IdentyProfNo { get; set; }


        public EarningMemberKYCData(string Name, string DOB, string RelationId, string Address, string State,
            string PinCode, string MobileNo, string IdentyPRofId, string IdentyProfNo)
        {
            this.Name = Name;
            this.DOB = DOB;
            this.RelationId = RelationId;
            this.Address = Address;
            this.State = State;
            this.PinCode = PinCode;
            this.MobileNo = MobileNo;
            this.IdentyPRofId = IdentyPRofId;
            this.IdentyProfNo = IdentyProfNo;
        }
    }

    [DataContract]
    public class SchemeData
    {
        [DataMember]
        public string LoanTypeId { get; set; }

        [DataMember]
        public string LoanType { get; set; }

        [DataMember]
        public string IsTopup { get; set; }

        public SchemeData(string LoanTypeId, string LoanType, string IsTopup)
        {
            this.LoanTypeId = LoanTypeId;
            this.LoanType = LoanType;
            this.IsTopup = IsTopup;
        }
    }

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

    [DataContract]
    public class MemberCreationSubData
    {
        [DataMember]
        public string EoId { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string MemberId { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string MemType { get; set; }

        public MemberCreationSubData(string EoId, string EoName, string MemberId, string MemberName,
            string MemType)
        {
            this.EoId = EoId;
            this.EoName = EoName;
            this.MemberId = MemberId;
            this.MemberName = MemberName;
            this.MemType = MemType;
        }
    }


    [DataContract]
    public class GetNewMemberInfo
    {
        [DataMember]
        public string MF_Name { get; set; }

        [DataMember]
        public string MM_Name { get; set; }

        [DataMember]
        public string ML_Name { get; set; }

        [DataMember]
        public string MDOB { get; set; }

        [DataMember]
        public string Age { get; set; }

        [DataMember]
        public string FamilyPersonName { get; set; }

        [DataMember]
        public string HumanRelationId { get; set; }

        [DataMember]
        public string IdentyPRofId { get; set; }

        [DataMember]
        public string IdentyProfNo { get; set; }

        [DataMember]
        public string AddProfId { get; set; }

        [DataMember]
        public string AddProfNo { get; set; }

        [DataMember]
        public string CoAppIdentyPRofId { get; set; }

        [DataMember]
        public string CoAppIdentyProfNo { get; set; }

        [DataMember]
        public string AddrType { get; set; }

        [DataMember]
        public string HouseNo { get; set; }

        [DataMember]
        public string Street { get; set; }

        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string Village { get; set; }

        [DataMember]
        public string SubDistrict { get; set; }

        [DataMember]
        public string District { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string Landmark { get; set; }

        [DataMember]
        public string PostOff { get; set; }

        [DataMember]
        public string PIN { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public string AddrType_p { get; set; }

        [DataMember]
        public string HouseNo_p { get; set; }

        [DataMember]
        public string Street_p { get; set; }

        [DataMember]
        public string Area_p { get; set; }

        [DataMember]
        public string VillageId_p { get; set; }

        [DataMember]
        public string SubDistrict_p { get; set; }

        [DataMember]
        public string Landmark_p { get; set; }

        [DataMember]
        public string PostOff_p { get; set; }

        [DataMember]
        public string PIN_p { get; set; }

        [DataMember]
        public string MobileNo_p { get; set; }

        [DataMember]
        public string MemStatus { get; set; }

        [DataMember]
        public string CoAppName { get; set; }

        [DataMember]
        public string CoApplDOB { get; set; }

        [DataMember]
        public string CoAppMobileNo { get; set; }

        [DataMember]
        public string CoAppRelationId { get; set; }

        [DataMember]
        public string CoAppAddress { get; set; }

        [DataMember]
        public string CoAppState { get; set; }

        [DataMember]
        public string CoAppPinCode { get; set; }

        [DataMember]
        public string LoanTypeId { get; set; }

        [DataMember]
        public string TotalOS { get; set; }

        [DataMember]
        public string NoOfOpenLoan { get; set; }

        [DataMember]
        public string AmountEligible { get; set; }

        [DataMember]
        public string CoAppAddPRofId { get; set; }

        [DataMember]
        public string CoAppAddProfNo { get; set; }

        [DataMember]
        public string IsAbledYN { get; set; }

        [DataMember]
        public string SpeciallyAbled { get; set; }

        public GetNewMemberInfo(string MF_Name, string MM_Name, string ML_Name, string MDOB, string Age, string FamilyPersonName, string HumanRelationId, 
            string IdentyPRofId, string IdentyProfNo, string AddProfId, string AddProfNo, string CoAppIdentyPRofId, string CoAppIdentyProfNo, string AddrType, 
            string HouseNo, string Street, string Area,string Village, string SubDistrict, string District, string State, string Landmark, string PostOff, string PIN,
            string MobileNo, string AddrType_p,string HouseNo_p, string Street_p, string Area_p, string VillageId_p, string SubDistrict_p, string Landmark_p, 
            string PostOff_p, string PIN_p, string MobileNo_p, string MemStatus, string CoAppName, string CoApplDOB, string CoAppMobileNo, string CoAppRelationId,
            string CoAppAddress, string CoAppState,string CoAppPinCode, string LoanTypeId, string TotalOS, string NoOfOpenLoan, string AmountEligible,
            string CoAppAddPRofId, string CoAppAddProfNo, string IsAbledYN, string SpeciallyAbled )
        {
            this.MF_Name = MF_Name;
            this.MM_Name = MM_Name;
            this.ML_Name = ML_Name;
            this.MDOB = MDOB;
            this.Age = Age;
            this.FamilyPersonName = FamilyPersonName;
            this.HumanRelationId = HumanRelationId;
            this.IdentyPRofId = IdentyPRofId;
            this.IdentyProfNo = IdentyProfNo;
            this.AddProfId = AddProfId;
            this.AddProfNo = AddProfNo;
            this.CoAppIdentyPRofId = CoAppIdentyPRofId;
            this.CoAppIdentyProfNo = CoAppIdentyProfNo;
            this.AddrType = AddrType;
            this.HouseNo = HouseNo;
            this.Street = Street;
            this.Area = Area;
            this.Village = Village;
            this.SubDistrict = SubDistrict;
            this.District = District;
            this.State = State;
            this.Landmark = Landmark;
            this.PostOff = PostOff;
            this.PIN = PIN;
            this.MobileNo = MobileNo;
            this.AddrType_p = AddrType_p;
            this.HouseNo_p = HouseNo_p;
            this.Street_p = Street_p;
            this.Area_p = Area_p;
            this.VillageId_p = VillageId_p;
            this.SubDistrict_p = SubDistrict_p;
            this.Landmark_p = Landmark_p;
            this.PostOff_p = PostOff_p;
            this.PIN_p = PIN_p;
            this.MobileNo_p = MobileNo_p;
            this.MemStatus = MemStatus;
            this.CoAppName = CoAppName;
            this.CoApplDOB = CoApplDOB;
            this.CoAppMobileNo = CoAppMobileNo;
            this.CoAppRelationId = CoAppRelationId;
            this.CoAppAddress = CoAppAddress;
            this.CoAppState = CoAppState;
            this.CoAppPinCode = CoAppPinCode;
            this.LoanTypeId = LoanTypeId;
            this.NoOfOpenLoan = NoOfOpenLoan;
            this.TotalOS = TotalOS;
            this.AmountEligible = AmountEligible;
            this.CoAppAddPRofId = CoAppAddPRofId;
            this.CoAppAddProfNo = CoAppAddProfNo;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class BusinessTypeData
    {
        [DataMember]
        public string BusinessTypeId { get; set; }

        [DataMember]
        public string BusinessTypeName { get; set; }

        public BusinessTypeData(string BusinessTypeId, string BusinessTypeName)
        {
            this.BusinessTypeId = BusinessTypeId;
            this.BusinessTypeName = BusinessTypeName;
        }
    }


    [DataContract]
    public class BusinessSubTypeData
    {
        [DataMember]
        public string BusinessSubTypeID { get; set; }

        [DataMember]
        public string BusinessSubType { get; set; }

        public BusinessSubTypeData(string BusinessSubTypeID, string BusinessSubType)
        {
            this.BusinessSubTypeID = BusinessSubTypeID;
            this.BusinessSubType = BusinessSubType;
        }
    }

    [DataContract]
    public class BusinessActivityData
    {
        [DataMember]
        public string BusinessActivityID { get; set; }

        [DataMember]
        public string BusinessActivity { get; set; }

        public BusinessActivityData(string BusinessActivityID, string BusinessActivity)
        {
            this.BusinessActivityID = BusinessActivityID;
            this.BusinessActivity = BusinessActivity;
        }
    }

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

    [DataContract]
    public class PDByBMData
    {
        [DataMember]
        public string EoId { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string MemberId { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string PDId { get; set; }

        public PDByBMData(string EoId, string EoName, string MemberId, string MemberName, string PDId)
        {
            this.EoId = EoId;
            this.EoName = EoName;
            this.MemberId = MemberId;
            this.MemberName = MemberName;
            this.PDId = PDId;
        }
    }

    [DataContract]
    public class PDByBMDtl
    {
        [DataMember]
        public string PurposeId { get; set; }

        [DataMember]
        public string ExpLoanAmt { get; set; }

        [DataMember]
        public string ExpLoanTenure { get; set; }

        [DataMember]
        public string EmiPayingCapacity { get; set; }

        [DataMember]
        public string ExistingLoanNo { get; set; }

        [DataMember]
        public string TotLnOS { get; set; }

        [DataMember]
        public string ApplTitle { get; set; }

        [DataMember]
        public string ApplFName { get; set; }

        [DataMember]
        public string ApplMName { get; set; }

        [DataMember]
        public string ApplLName { get; set; }

        [DataMember]
        public string ApplGender { get; set; }

        [DataMember]
        public string ApplMaritalStatus { get; set; }

        [DataMember]
        public string ApplEduId { get; set; }

        [DataMember]
        public string ApplReliStat { get; set; }

        [DataMember]
        public string ApplReligion { get; set; }

        [DataMember]
        public string ApplCaste { get; set; }

        [DataMember]
        public string ApplPerAddrType { get; set; }

        [DataMember]
        public string ApplPerHouseNo { get; set; }

        [DataMember]
        public string ApplPerStreet { get; set; }

        [DataMember]
        public string ApplPerSubDist { get; set; }

        [DataMember]
        public string ApplPerPostOffice { get; set; }

        [DataMember]
        public string ApplPerLandmark { get; set; }

        [DataMember]
        public string ApplPerArea { get; set; }

        [DataMember]
        public string ApplPerPIN { get; set; }

        [DataMember]
        public string ApplPerDist { get; set; }

        [DataMember]
        public string ApplPerVillage { get; set; }

        [DataMember]
        public string ApplPerContactNo { get; set; }

        [DataMember]
        public string ApplPerStateId { get; set; }

        [DataMember]
        public string ApplPreAddrType { get; set; }

        [DataMember]
        public string ApplPreHouseNo { get; set; }

        [DataMember]
        public string ApplPreStreet { get; set; }

        [DataMember]
        public string ApplPreVillageId { get; set; }

        [DataMember]
        public string ApplPreSubDist { get; set; }

        [DataMember]
        public string ApplPrePostOffice { get; set; }

        [DataMember]
        public string ApplPrePIN { get; set; }

        [DataMember]
        public string ApplPreLandmark { get; set; }

        [DataMember]
        public string ApplPreArea { get; set; }

        [DataMember]
        public string ApplPhyFitness { get; set; }

        [DataMember]
        public string CoApplTitle { get; set; }

        [DataMember]
        public string CoApplName { get; set; }

        [DataMember]
        public string CoApplDOB { get; set; }

        [DataMember]
        public string CoApplAge { get; set; }

        [DataMember]
        public string CoApplGender { get; set; }

        [DataMember]
        public string CoApplMaritalStatus { get; set; }

        [DataMember]
        public string CoApplRelation { get; set; }

        [DataMember]
        public string CoApplEduId { get; set; }

        [DataMember]
        public string CoApplPerAddr { get; set; }

        [DataMember]
        public string CoApplPerStateId { get; set; }

        [DataMember]
        public string CoApplPerPIN { get; set; }

        [DataMember]
        public string CoApplMobileNo { get; set; }

        [DataMember]
        public string CoApplAddiContactNo { get; set; }

        [DataMember]
        public string CoApplPhyFitness { get; set; }

        [DataMember]
        public string CoAppIncYN { get; set; }

        [DataMember]
        public string TypeOfInc { get; set; }

        [DataMember]
        public string AgeOfKeyIncEar { get; set; }

        [DataMember]
        public string AnnulInc { get; set; }

        [DataMember]
        public string HouseStability { get; set; }

        [DataMember]
        public string TypeOfOwnerShip { get; set; }

        [DataMember]
        public string TypeOfResi { get; set; }

        [DataMember]
        public string ResiCategory { get; set; }

        [DataMember]
        public string TotalFamMember { get; set; }

        [DataMember]
        public string NoOfChild { get; set; }

        [DataMember]
        public string NoOfDependent { get; set; }

        [DataMember]
        public string NoOfFamEarMember { get; set; }

        [DataMember]
        public string LandHolding { get; set; }

        [DataMember]
        public string BankingHabit { get; set; }

        [DataMember]
        public string PersonalRef { get; set; }

        [DataMember]
        public string Addr { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public string ValidatedYN { get; set; }

        [DataMember]
        public string MobilePhone { get; set; }

        [DataMember]
        public string Refrigerator { get; set; }

        [DataMember]
        public string TwoWheeler { get; set; }

        [DataMember]
        public string ThreeWheeler { get; set; }

        [DataMember]
        public string FourWheeler { get; set; }

        [DataMember]
        public string AirConditioner { get; set; }

        [DataMember]
        public string WashingMachine { get; set; }

        [DataMember]
        public string EmailId { get; set; }

        [DataMember]
        public string PAN { get; set; }

        [DataMember]
        public string GSTno { get; set; }

        [DataMember]
        public string ITR { get; set; }

        [DataMember]
        public string Whatsapp { get; set; }

        [DataMember]
        public string FacebookAc { get; set; }

        [DataMember]
        public string ACHolderName { get; set; }

        [DataMember]
        public string AccNo { get; set; }

        [DataMember]
        public string IfscCode { get; set; }

        [DataMember]
        public string AccType { get; set; }

        [DataMember]
        public string BusinessName { get; set; }

        [DataMember]
        public string BusiNameOnBoard { get; set; }

        [DataMember]
        public string PrimaryBusiType { get; set; }

        [DataMember]
        public string PrimaryBusiSeaso { get; set; }

        [DataMember]
        public string PrimaryBusiSubType { get; set; }

        [DataMember]
        public string PrimaryBusiActivity { get; set; }

        [DataMember]
        public string WorkingDays { get; set; }

        [DataMember]
        public string MonthlyTrunOver { get; set; }

        [DataMember]
        public string LocalityArea { get; set; }

        [DataMember]
        public string BusiEstdDt { get; set; }

        [DataMember]
        public string BusinessAddr { get; set; }

        [DataMember]
        public string BusinessVintage { get; set; }

        [DataMember]
        public string BusiOwnerType { get; set; }

        [DataMember]
        public string BusiHndlPerson { get; set; }

        [DataMember]
        public string PartnerYN { get; set; }

        [DataMember]
        public string NoOfEmp { get; set; }

        [DataMember]
        public string ValueOfStock { get; set; }

        [DataMember]
        public string ValueOfMachinery { get; set; }

        [DataMember]
        public string BusiHours { get; set; }

        [DataMember]
        public string AppName { get; set; }

        [DataMember]
        public string VPAID { get; set; }

        [DataMember]
        public string BusiTranProofType { get; set; }

        [DataMember]
        public string CashInHand { get; set; }

        [DataMember]
        public string BusiRef { get; set; }

        [DataMember]
        public string BusiAddr { get; set; }

        [DataMember]
        public string BusiMobileNo { get; set; }

        [DataMember]
        public string BusiValidateYN { get; set; }

        [DataMember]
        public string SecondaryBusiYN { get; set; }

        [DataMember]
        public string NoOfSecBusi { get; set; }

        [DataMember]
        public string SecBusiType1 { get; set; }

        [DataMember]
        public string SecBusiSeaso1 { get; set; }

        [DataMember]
        public string SecBusiSubType1 { get; set; }

        [DataMember]
        public string SecBusiActivity1 { get; set; }

        [DataMember]
        public string SecBusiType2 { get; set; }

        [DataMember]
        public string SecBusiSeaso2 { get; set; }

        [DataMember]
        public string SecBusiSubType2 { get; set; }

        [DataMember]
        public string SecBusiActivity2 { get; set; }

        [DataMember]
        public string BusiIncYN { get; set; }

        [DataMember]
        public string AppCoAppSameBusiYN { get; set; }

        [DataMember]
        public string CoAppBusiName { get; set; }

        [DataMember]
        public string CoApplPrimaryBusiType { get; set; }

        [DataMember]
        public string CoApplPrimaryBusiSeaso { get; set; }

        [DataMember]
        public string CoApplPrimaryBusiSubType { get; set; }

        [DataMember]
        public string CoApplPrimaryBusiActivity { get; set; }

        [DataMember]
        public string CoApplMonthlyTrunOver { get; set; }

        [DataMember]
        public string CoApplBusinessAddr { get; set; }

        [DataMember]
        public string CoApplBusinessVintage { get; set; }

        [DataMember]
        public string CoApplBusiOwnerType { get; set; }

        [DataMember]
        public string CoApplValueOfStock { get; set; }

        [DataMember]
        public string ApplAge { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string CoAppPerAddrType { get; set; }

        [DataMember]
        public string CoApplPreAddrType { get; set; }

        [DataMember]
        public string CoApplPreAddr { get; set; }

        [DataMember]
        public string CoApplPreStateId { get; set; }

        [DataMember]
        public string CoApplPrePIN { get; set; }

        [DataMember]
        public string ApplDOB { get; set; }

        [DataMember]
        public string FamilyAsset { get; set; }

        [DataMember]
        public string OtherAsset { get; set; }

        [DataMember]
        public string OtherSavings { get; set; }

        [DataMember]
        public string OtherBusinessProof { get; set; }

        [DataMember]
        public string ApplMobileNo { get; set; }

        [DataMember]
        public string ApplAddProfId { get; set; }
        [DataMember]
        public string ApplAddProfNo { get; set; }
        [DataMember]
        public string ApplIdentyPRofId { get; set; }
        [DataMember]
        public string ApplIdentyProfNo { get; set; }
        [DataMember]
        public string CoApplIdentyPRofId { get; set; }
        [DataMember]
        public string CoApplIdentyProfNo { get; set; }

        [DataMember]
        public string CoApplAddPRofId { get; set; }
        [DataMember]
        public string CoApplAddProfNo { get; set; }

        [DataMember]
        public string UdyamAadhaarRegistNo { get; set; }

        [DataMember]
        public string FamilyPersonName { get; set; }
        [DataMember]
        public string HumanRelationId { get; set; }

        public string IsAbledYN { get; set; }
        [DataMember]
        public string SpeciallyAbled { get; set; }


        public PDByBMDtl(string PurposeId, string ExpLoanAmt, string ExpLoanTenure, string EmiPayingCapacity, string ExistingLoanNo, string TotLnOS,
         string ApplTitle, string ApplFName, string ApplMName, string ApplLName, string ApplGender, string ApplMaritalStatus, string ApplEduId,
         string ApplReliStat, string ApplReligion, string ApplCaste, string ApplPerAddrType, string ApplPerHouseNo, string ApplPerStreet,
         string ApplPerSubDist, string ApplPerPostOffice, string ApplPerLandmark, string ApplPerArea, string ApplPerPIN, string ApplPerDist,
         string ApplPerVillage, string ApplPerContactNo, string ApplPerStateId, string ApplPreAddrType, string ApplPreHouseNo, string ApplPreStreet,
         string ApplPreVillageId, string ApplPreSubDist, string ApplPrePostOffice, string ApplPrePIN, string ApplPreLandmark, string ApplPreArea,
         string ApplPhyFitness, string CoApplTitle, string CoApplName, string CoApplDOB, string CoApplAge, string CoApplGender, string CoApplMaritalStatus,
         string CoApplRelation, string CoApplEduId, string CoApplPerAddr, string CoApplPerStateId, string CoApplPerPIN, string CoApplMobileNo,
         string CoApplAddiContactNo, string CoApplPhyFitness, string CoAppIncYN, string TypeOfInc, string AgeOfKeyIncEar, string AnnulInc,
         string HouseStability, string TypeOfOwnerShip, string TypeOfResi, string ResiCategory, string TotalFamMember, string NoOfChild,
         string NoOfDependent, string NoOfFamEarMember, string LandHolding, string BankingHabit, string PersonalRef, string Addr,
         string MobileNo, string ValidatedYN, string MobilePhone, string Refrigerator, string TwoWheeler, string ThreeWheeler, string FourWheeler,
         string AirConditioner, string WashingMachine, string EmailId, string PAN, string GSTno, string ITR, string Whatsapp, string FacebookAc,
         string ACHolderName, string AccNo, string IfscCode, string AccType, string BusinessName, string BusiNameOnBoard, string PrimaryBusiType,
         string PrimaryBusiSeaso, string PrimaryBusiSubType, string PrimaryBusiActivity, string WorkingDays, string MonthlyTrunOver,
         string LocalityArea, string BusiEstdDt, string BusinessAddr, string BusinessVintage, string BusiOwnerType, string BusiHndlPerson,
         string PartnerYN, string NoOfEmp, string ValueOfStock, string ValueOfMachinery, string BusiHours, string AppName,
         string VPAID, string BusiTranProofType, string CashInHand, string BusiRef, string BusiAddr, string BusiMobileNo,
         string BusiValidateYN, string SecondaryBusiYN, string NoOfSecBusi, string SecBusiType1, string SecBusiSeaso1,
         string SecBusiSubType1, string SecBusiActivity1, string SecBusiType2, string SecBusiSeaso2, string SecBusiSubType2,
         string SecBusiActivity2, string BusiIncYN, string AppCoAppSameBusiYN, string CoAppBusiName, string CoApplPrimaryBusiType, string CoApplPrimaryBusiSeaso,
         string CoApplPrimaryBusiSubType, string CoApplPrimaryBusiActivity, string CoApplMonthlyTrunOver, string CoApplBusinessAddr, string CoApplBusinessVintage,
         string CoApplBusiOwnerType, string CoApplValueOfStock, string ApplAge, string BankName, string CoAppPerAddrType, string CoApplPreAddrType,
         string CoApplPreAddr, string CoApplPreStateId, string CoApplPrePIN, string ApplDOB, string FamilyAsset, string OtherAsset, string OtherSavings,
         string OtherBusinessProof, string ApplMobileNo, string ApplAddProfId, string ApplAddProfNo, string ApplIdentyPRofId,
         string ApplIdentyProfNo, string CoApplIdentyPRofId, string CoApplIdentyProfNo, string CoApplAddPRofId, string CoApplAddProfNo,
         string UdyamAadhaarRegistNo, string FamilyPersonName, string HumanRelationId, string IsAbledYN, string SpeciallyAbled)
        {
            this.PurposeId = PurposeId;
            this.ExpLoanAmt = ExpLoanAmt;
            this.ExpLoanTenure = ExpLoanTenure;
            this.EmiPayingCapacity = EmiPayingCapacity;
            this.ExistingLoanNo = ExistingLoanNo;
            this.TotLnOS = TotLnOS;
            this.ApplTitle = ApplTitle;
            this.ApplFName = ApplFName;
            this.ApplMName = ApplMName;
            this.ApplLName = ApplLName;
            this.ApplGender = ApplGender;
            this.ApplMaritalStatus = ApplMaritalStatus;
            this.ApplEduId = ApplEduId;
            this.ApplReliStat = ApplReliStat;
            this.ApplReligion = ApplReligion;
            this.ApplCaste = ApplCaste;
            this.ApplPerAddrType = ApplPerAddrType;
            this.ApplPerHouseNo = ApplPerHouseNo;
            this.ApplPerStreet = ApplPerStreet;
            this.ApplPerSubDist = ApplPerSubDist;
            this.ApplPerPostOffice = ApplPerPostOffice;
            this.ApplPerLandmark = ApplPerLandmark;
            this.ApplPerArea = ApplPerArea;
            this.ApplPerPIN = ApplPerPIN;
            this.ApplPerDist = ApplPerDist;
            this.ApplPerVillage = ApplPerVillage;
            this.ApplPerContactNo = ApplPerContactNo;
            this.ApplPerStateId = ApplPerStateId;
            this.ApplPreAddrType = ApplPreAddrType;
            this.ApplPreHouseNo = ApplPreHouseNo;
            this.ApplPreStreet = ApplPreStreet;
            this.ApplPreVillageId = ApplPreVillageId;
            this.ApplPreSubDist = ApplPreSubDist;
            this.ApplPrePostOffice = ApplPrePostOffice;
            this.ApplPrePIN = ApplPrePIN;
            this.ApplPreLandmark = ApplPreLandmark;
            this.ApplPreArea = ApplPreArea;
            this.ApplPhyFitness = ApplPhyFitness;
            this.CoApplTitle = CoApplTitle;
            this.CoApplName = CoApplName;
            this.CoApplDOB = CoApplDOB;
            this.CoApplAge = CoApplAge;
            this.CoApplGender = CoApplGender;
            this.CoApplMaritalStatus = CoApplMaritalStatus;
            this.CoApplRelation = CoApplRelation;
            this.CoApplEduId = CoApplEduId;
            this.CoApplPerAddr = CoApplPerAddr;
            this.CoApplPerStateId = CoApplPerStateId;
            this.CoApplPerPIN = CoApplPerPIN;
            this.CoApplMobileNo = CoApplMobileNo;
            this.CoApplAddiContactNo = CoApplAddiContactNo;
            this.CoApplPhyFitness = CoApplPhyFitness;
            this.CoAppIncYN = CoAppIncYN;
            this.TypeOfInc = TypeOfInc;
            this.AgeOfKeyIncEar = AgeOfKeyIncEar;
            this.AnnulInc = AnnulInc;
            this.HouseStability = HouseStability;
            this.TypeOfOwnerShip = TypeOfOwnerShip;
            this.TypeOfResi = TypeOfResi;
            this.ResiCategory = ResiCategory;
            this.TotalFamMember = TotalFamMember;
            this.NoOfChild = NoOfChild;
            this.NoOfDependent = NoOfDependent;
            this.NoOfFamEarMember = NoOfFamEarMember;
            this.LandHolding = LandHolding;
            this.BankingHabit = BankingHabit;
            this.PersonalRef = PersonalRef;
            this.Addr = Addr;
            this.MobileNo = MobileNo;
            this.ValidatedYN = ValidatedYN;
            this.MobilePhone = MobilePhone;
            this.Refrigerator = Refrigerator;
            this.TwoWheeler = TwoWheeler;
            this.ThreeWheeler = ThreeWheeler;
            this.FourWheeler = FourWheeler;
            this.AirConditioner = AirConditioner;
            this.WashingMachine = WashingMachine;
            this.EmailId = EmailId;
            this.PAN = PAN;
            this.GSTno = GSTno;
            this.ITR = ITR;
            this.Whatsapp = Whatsapp;
            this.FacebookAc = FacebookAc;
            this.ACHolderName = ACHolderName;
            this.AccNo = AccNo;
            this.IfscCode = IfscCode;
            this.AccType = AccType;
            this.BusinessName = BusinessName;
            this.BusiNameOnBoard = BusiNameOnBoard;
            this.PrimaryBusiType = PrimaryBusiType;
            this.PrimaryBusiSeaso = PrimaryBusiSeaso;
            this.PrimaryBusiSubType = PrimaryBusiSubType;
            this.PrimaryBusiActivity = PrimaryBusiActivity;
            this.WorkingDays = WorkingDays;
            this.MonthlyTrunOver = MonthlyTrunOver;
            this.LocalityArea = LocalityArea;
            this.BusiEstdDt = BusiEstdDt;
            this.BusinessAddr = BusinessAddr;
            this.BusinessVintage = BusinessVintage;
            this.BusiOwnerType = BusiOwnerType;
            this.BusiHndlPerson = BusiHndlPerson;
            this.PartnerYN = PartnerYN;
            this.NoOfEmp = NoOfEmp;
            this.ValueOfStock = ValueOfStock;
            this.ValueOfMachinery = ValueOfMachinery;
            this.BusiHours = BusiHours;
            this.AppName = AppName;
            this.VPAID = VPAID;
            this.BusiTranProofType = BusiTranProofType;
            this.CashInHand = CashInHand;
            this.BusiRef = BusiRef;
            this.BusiAddr = BusiAddr;
            this.BusiMobileNo = BusiMobileNo;
            this.BusiValidateYN = BusiValidateYN;
            this.SecondaryBusiYN = SecondaryBusiYN;
            this.NoOfSecBusi = NoOfSecBusi;
            this.SecBusiType1 = SecBusiType1;
            this.SecBusiSeaso1 = SecBusiSeaso1;
            this.SecBusiSubType1 = SecBusiSubType1;
            this.SecBusiActivity1 = SecBusiActivity1;
            this.SecBusiType2 = SecBusiType2;
            this.SecBusiSeaso2 = SecBusiSeaso2;
            this.SecBusiSubType2 = SecBusiSubType2;
            this.SecBusiActivity2 = SecBusiActivity2;
            this.BusiIncYN = BusiIncYN;
            this.AppCoAppSameBusiYN = AppCoAppSameBusiYN;
            this.CoAppBusiName = CoAppBusiName;
            this.CoApplPrimaryBusiType = CoApplPrimaryBusiType;
            this.CoApplPrimaryBusiSeaso = CoApplPrimaryBusiSeaso;
            this.CoApplPrimaryBusiSubType = CoApplPrimaryBusiSubType;
            this.CoApplPrimaryBusiActivity = CoApplPrimaryBusiActivity;
            this.CoApplMonthlyTrunOver = CoApplMonthlyTrunOver;
            this.CoApplBusinessAddr = CoApplBusinessAddr;
            this.CoApplBusinessVintage = CoApplBusinessVintage;
            this.CoApplBusiOwnerType = CoApplBusiOwnerType;
            this.CoApplValueOfStock = CoApplValueOfStock;
            this.ApplAge = ApplAge;
            this.BankName = BankName;
            this.CoAppPerAddrType = CoAppPerAddrType;
            this.CoApplPreAddrType = CoApplPreAddrType;
            this.CoApplPreAddr = CoApplPreAddr;
            this.CoApplPreStateId = CoApplPreStateId;
            this.CoApplPrePIN = CoApplPrePIN;
            this.ApplDOB = ApplDOB;
            this.FamilyAsset = FamilyAsset;
            this.OtherAsset = OtherAsset;
            this.OtherSavings = OtherSavings;
            this.OtherBusinessProof = OtherBusinessProof;
            this.ApplMobileNo = ApplMobileNo;
            this.ApplAddProfId = ApplAddProfId;
            this.ApplAddProfNo = ApplAddProfNo;
            this.ApplIdentyPRofId = ApplIdentyPRofId;
            this.ApplIdentyProfNo = ApplIdentyProfNo;
            this.CoApplIdentyPRofId = CoApplIdentyPRofId;
            this.CoApplIdentyProfNo = CoApplIdentyProfNo;
            this.CoApplAddPRofId = CoApplAddPRofId;
            this.CoApplAddProfNo = CoApplAddProfNo;
            this.UdyamAadhaarRegistNo = UdyamAadhaarRegistNo;
            this.FamilyPersonName = FamilyPersonName;
            this.HumanRelationId = HumanRelationId;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class ICICBalanceFetchResponse
    {
        [DataMember]
        public string AGGR_ID { get; set; }
        [DataMember]
        public string CORP_ID { get; set; }
        [DataMember]
        public string USER_ID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string ACCOUNTNO { get; set; }
        [DataMember]
        public string DATE { get; set; }
        [DataMember]
        public string EFFECTIVEBAL { get; set; }
        [DataMember]
        public string CURRENCY { get; set; }
        [DataMember]
        public string MESSAGE { get; set; }

        public ICICBalanceFetchResponse(string AGGR_ID, string CORP_ID, string USER_ID, string URN, string Response, string ACCOUNTNO, string DATE, string EFFECTIVEBAL, string CURRENCY, string MESSAGE)
        {
            this.AGGR_ID = AGGR_ID;
            this.CORP_ID = CORP_ID;
            this.USER_ID = USER_ID;
            this.URN = URN;
            this.Response = Response;
            this.ACCOUNTNO = ACCOUNTNO;
            this.DATE = DATE;
            this.EFFECTIVEBAL = EFFECTIVEBAL;
            this.CURRENCY = CURRENCY;
            this.MESSAGE = MESSAGE;
        }

    }

    [DataContract]
    public class ICICBankTransactionResponse
    {
        [DataMember]
        public string AGGR_ID { get; set; }
        [DataMember]
        public string AGGR_NAME { get; set; }
        [DataMember]
        public string CORP_ID { get; set; }
        [DataMember]
        public string USER_ID { get; set; }
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string UTRNUMBER { get; set; }
        [DataMember]
        public string REQID { get; set; }
        [DataMember]
        public string STATUS { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string message { get; set; }

        public ICICBankTransactionResponse(string AGGR_ID, string AGGR_NAME, string CORP_ID, string USER_ID, string URN, string UNIQUEID, string UTRNUMBER
            , string REQID, string STATUS, string Response, string message)
        {
            this.AGGR_ID = AGGR_ID;
            this.AGGR_NAME = AGGR_NAME;
            this.CORP_ID = CORP_ID;
            this.USER_ID = USER_ID;
            this.URN = URN;
            this.UNIQUEID = UNIQUEID;
            this.UTRNUMBER = UTRNUMBER;
            this.REQID = REQID;
            this.STATUS = STATUS;
            this.Response = Response;
            this.message = message;
        }

    }

    [DataContract]
    public class NupayStatus
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }
        public NupayStatus(string StatusCode, string StatusDesc)
        {
            this.StatusCode = StatusCode;
            this.StatusDesc = StatusDesc;
        }
    }

    [DataContract]
    public class Result
    {
        [DataMember]
        public string message { get; set; }
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

    [DataContract]
    public class ServerStatus
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }
    }

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
    public class RefIdResult
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string refId { get; set; }
        [DataMember]
        public string refData { get; set; }
        [DataMember]
        public string refDataType { get; set; }
        [DataMember]
        public string tokenId { get; set; }
        [DataMember]
        public int isActive { get; set; }
    }

    [DataContract]
    public class AadhaarNoByRefId
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
        public List<RefIdResult> results { get; set; }

        public AadhaarNoByRefId(string action, int response_code, string response_message, int total_size, int total_pages, List<RefIdResult> results)
        {
            this.action = action;
            this.response_code = response_code;
            this.response_message = response_message;
            this.total_size = total_size;
            this.total_pages = total_pages;
            this.results = results;
        }
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

    public class ClientData
    {
        public string caseId { get; set; }
    }

    public class PANResult
    {
        public string name { get; set; }
    }

    public class KarzaPANResponse
    {
        public string status_code { get; set; }
        public string request_id { get; set; }
        public PANResult result { get; set; }
        public ClientData clientData { get; set; }
    }
}