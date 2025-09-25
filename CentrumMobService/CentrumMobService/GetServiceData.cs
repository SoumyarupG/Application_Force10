using System.Runtime.Serialization;
using System.Data;
using System.Collections.Generic;

namespace CentrumMobService
{
    [DataContract]
    public class LoginLogOutData
    {
        [DataMember]
        public string LoginId { get; set; }
    }

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
        public string AllowGalleryAccess { get; set; }

        [DataMember]
        public string ActivateApkAccess { get; set; }

        [DataMember]
        public string AllowManualEntry { get; set; }

        [DataMember]
        public string AllowQRScan { get; set; }

        [DataMember]
        public string AreaCategoryId { get; set; }

        [DataMember]
        public string BCBranchYN { get; set; }

        [DataMember]
        public string BranchName { get; set; }

        [DataMember]
        public string ThirdWeekNotAllow { get; set; }

        [DataMember]
        public string LoginId { get; set; }

        [DataMember]
        public string AllowAdvYN { get; set; }

        [DataMember]
        public string MFAYN { get; set; }

        [DataMember]
        public string MFAOTP { get; set; }

        [DataMember]
        public string ImgMaskingYN { get; set; }

        [DataMember]
        public string DialogToImageYN { get; set; }

        public EmpData(string EoName, string BranchCode, string UserID, string Eoid, string LogStatus, string DayEndDate, string AttStatus, string AttFlag
            , string AreaCat, string Designation, string AllowGalleryAccess, string ActivateApkAccess, string AllowManualEntry, string AllowQRScan
            , string AreaCategoryId, string BCBranchYN, string BranchName, string ThirdWeekNotAllow, string LoginId, string AllowAdvYN, string MFAYN,
             string MFAOTP, string ImgMaskingYN, string DialogToImageYN)
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
            this.AllowGalleryAccess = AllowGalleryAccess;
            this.ActivateApkAccess = ActivateApkAccess;
            this.AllowManualEntry = AllowManualEntry;
            this.AllowQRScan = AllowQRScan;
            this.BCBranchYN = BCBranchYN;
            this.BranchName = BranchName;
            this.ThirdWeekNotAllow = ThirdWeekNotAllow;
            this.LoginId = LoginId;
            this.AllowAdvYN = AllowAdvYN;
            this.MFAYN = MFAYN;
            this.MFAOTP = MFAOTP;
            this.ImgMaskingYN = ImgMaskingYN;
            this.DialogToImageYN = DialogToImageYN;
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
    public class WorkAllocData
    {
        [DataMember]
        public string EoID { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string UserID { get; set; }

        public WorkAllocData(string EoID, string EoName, string UserID)
        {
            this.EoID = EoID;
            this.EoName = EoName;
            this.UserID = UserID;
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
    public class EMPImageSave
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string FileName { get; set; }
        public EMPImageSave(string Status, string FileName)
        {
            this.Status = Status;
            this.FileName = FileName;
        }
    }

    [DataContract]
    public class GroupDBData
    {
        [DataMember]
        public string MobGroup { get; set; }

        [DataMember]
        public string WebGroup { get; set; }

        public GroupDBData(string MobGroup, string WebGroup)
        {
            this.MobGroup = MobGroup;
            this.WebGroup = WebGroup;
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
        public string Marketid { get; set; }

        [DataMember]
        public string MarketName { get; set; }

        [DataMember]
        public string GroupId { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string CollDay { get; set; }

        [DataMember]
        public string MemberId { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string MemType { get; set; }

        public MemberCreationSubData(string EoId, string EoName, string Marketid, string MarketName, string GroupId, string GroupName, string CollDay, string MemberId, string MemberName,
            string MemType)
        {
            this.EoId = EoId;
            this.EoName = EoName;
            this.Marketid = Marketid;
            this.MarketName = MarketName;
            this.GroupId = GroupId;
            this.GroupName = GroupName;
            this.CollDay = CollDay;
            this.MemberId = MemberId;
            this.MemberName = MemberName;
            this.MemType = MemType;
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
        public string AddProfId2 { get; set; }

        [DataMember]
        public string AddProfNo2 { get; set; }

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
        public string EmailId { get; set; }

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
        public string EmailId_p { get; set; }

        [DataMember]
        public string MemStatus { get; set; }

        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string BusinessTypeId { get; set; }
        [DataMember]
        public string OccupationId { get; set; }
        [DataMember]
        public string DeclaredInc { get; set; }
        [DataMember]
        public string IncFrequency { get; set; }
        [DataMember]
        public string CoAppName { get; set; }
        [DataMember]
        public string CoAppFName { get; set; }
        [DataMember]
        public string CoAppMName { get; set; }
        [DataMember]
        public string CoAppLName { get; set; }
        [DataMember]
        public string CoAppDOB { get; set; }
        [DataMember]
        public string CoAppAge { get; set; }
        [DataMember]
        public string CoAppGender { get; set; }
        [DataMember]
        public string CoAppRelationId { get; set; }
        [DataMember]
        public string CoAppMobileNo { get; set; }
        [DataMember]
        public string CoAppAddress { get; set; }
        [DataMember]
        public string CoAppState { get; set; }
        [DataMember]
        public string CoAppPinCode { get; set; }
        [DataMember]
        public string CoAppIdentyProfId { get; set; }
        [DataMember]
        public string CoAppIdentyProfNo { get; set; }
        [DataMember]
        public string CoAppBusinessTypeId { get; set; }
        [DataMember]
        public string CoAppOccupationId { get; set; }
        [DataMember]
        public string CoAppDeclaredInc { get; set; }
        [DataMember]
        public string CoAppIncFrequency { get; set; }
        [DataMember]
        public string CoApplicantDOB { get; set; }
        [DataMember]
        public List<EarningMemberDtl> EarningMemberDtl { get; set; }
        [DataMember]
        public string BusinessActvId { get; set; }
        [DataMember]
        public string CoAppBusinessActvId { get; set; }
        [DataMember]
        public string EMI_Obligation { get; set; }
        [DataMember]
        public string EligibleEMI { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string EnqDate { get; set; }

        [DataMember]
        public string CoAppHouseNo { get; set; }

        [DataMember]
        public string CoAppStreet { get; set; }

        [DataMember]
        public string CoAppArea { get; set; }

        [DataMember]
        public string CoAppVillageId { get; set; }

        [DataMember]
        public string CoAppSubDistrict { get; set; }

        [DataMember]
        public string CoAppDistrict { get; set; }

        [DataMember]
        public string CoAppLandmark { get; set; }

        [DataMember]
        public string CoAppPostOffice { get; set; }

        [DataMember]
        public string CoAppStateId { get; set; }

        [DataMember]
        public string IsAbledYN { get; set; }

        [DataMember]
        public string SpeciallyAbled { get; set; }

        public GetNewMemberInfo(string MF_Name, string MM_Name, string ML_Name, string MDOB, string Age, string FamilyPersonName, string HumanRelationId, string IdentyPRofId,
            string IdentyProfNo, string AddProfId, string AddProfNo, string AddProfId2, string AddProfNo2, string AddrType, string HouseNo, string Street, string Area,
            string Village, string SubDistrict, string District, string State, string Landmark, string PostOff, string PIN, string MobileNo, string EmailId, string AddrType_p,
            string HouseNo_p, string Street_p, string Area_p, string VillageId_p, string SubDistrict_p, string Landmark_p, string PostOff_p, string PIN_p, string MobileNo_p,
            string EmailId_p, string MemStatus,
            string Gender, string BusinessTypeId, string OccupationId, string DeclaredInc, string IncFrequency,
            string CoAppName, string CoAppFName, string CoAppMName, string CoAppLName, string CoAppDOB, string CoAppAge, string CoAppGender,
            string CoAppRelationId, string CoAppMobileNo, string CoAppAddress, string CoAppState, string CoAppPinCode, string CoAppIdentyProfId, string CoAppIdentyProfNo,
            string CoAppBusinessTypeId, string CoAppOccupationId, string CoAppDeclaredInc, string CoAppIncFrequency, string CoApplicantDOB,
            List<EarningMemberDtl> EarningMemberDtl, string BusinessActvId, string CoAppBusinessActvId, string EMI_Obligation, string EligibleEMI,
            string ImageUrl, string EnqDate, string CoAppHouseNo, string CoAppStreet, string CoAppArea, string CoAppVillageId, string CoAppSubDistrict,
            string CoAppDistrict, string CoAppLandmark, string CoAppPostOffice, string CoAppStateId, string IsAbledYN, string SpeciallyAbled)
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
            this.AddProfId2 = AddProfId2;
            this.AddProfNo2 = AddProfNo2;
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
            this.EmailId = EmailId;
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
            this.EmailId_p = EmailId_p;
            this.MemStatus = MemStatus;

            this.Gender = Gender;
            this.BusinessTypeId = BusinessTypeId;
            this.OccupationId = OccupationId;
            this.DeclaredInc = DeclaredInc;
            this.IncFrequency = IncFrequency;
            this.CoAppName = CoAppName;
            this.CoAppFName = CoAppFName;
            this.CoAppMName = CoAppMName;
            this.CoAppLName = CoAppLName;
            this.CoAppDOB = CoAppDOB;
            this.CoAppAge = CoAppAge;
            this.CoAppGender = CoAppGender;
            this.CoAppRelationId = CoAppRelationId;
            this.CoAppMobileNo = CoAppMobileNo;
            this.CoAppAddress = CoAppAddress;
            this.CoAppState = CoAppState;
            this.CoAppPinCode = CoAppPinCode;
            this.CoAppIdentyProfId = CoAppIdentyProfId;
            this.CoAppIdentyProfNo = CoAppIdentyProfNo;
            this.CoAppBusinessTypeId = CoAppBusinessTypeId;
            this.CoAppOccupationId = CoAppOccupationId;
            this.CoAppDeclaredInc = CoAppDeclaredInc;
            this.CoAppIncFrequency = CoAppIncFrequency;
            this.CoApplicantDOB = CoApplicantDOB;
            this.EarningMemberDtl = EarningMemberDtl;
            this.BusinessActvId = BusinessActvId;
            this.CoAppBusinessActvId = CoAppBusinessActvId;
            this.EMI_Obligation = EMI_Obligation;
            this.EligibleEMI = EligibleEMI;
            this.ImageUrl = ImageUrl;
            this.EnqDate = EnqDate;
            this.CoAppHouseNo = CoAppHouseNo;
            this.CoAppStreet = CoAppStreet;
            this.CoAppArea = CoAppArea;
            this.CoAppVillageId = CoAppVillageId;
            this.CoAppSubDistrict = CoAppSubDistrict;
            this.CoAppDistrict = CoAppDistrict;
            this.CoAppLandmark = CoAppLandmark;
            this.CoAppPostOffice = CoAppPostOffice;
            this.CoAppStateId = CoAppStateId;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class ExstMemberData
    {
        [DataMember]
        public string MemberId { get; set; }

        [DataMember]
        public string Distance_frm_Branch { get; set; }

        [DataMember]
        public string Distance_frm_Coll_Center { get; set; }

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
        public string M_Gender { get; set; }

        [DataMember]
        public string MM_Status { get; set; }

        [DataMember]
        public string M_RelgId { get; set; }

        [DataMember]
        public string M_Caste { get; set; }

        [DataMember]
        public string M_QualificationId { get; set; }

        [DataMember]
        public string M_OccupationId { get; set; }

        [DataMember]
        public string FamilyPersonName { get; set; }

        [DataMember]
        public string HumanRelationId { get; set; }

        [DataMember]
        public string MaidenNmF { get; set; }

        [DataMember]
        public string MaidenNmM { get; set; }

        [DataMember]
        public string MaidenNmL { get; set; }

        [DataMember]
        public string IdentyPRofId { get; set; }

        [DataMember]
        public string IdentyProfNo { get; set; }

        [DataMember]
        public string AddProfId { get; set; }

        [DataMember]
        public string AddProfNo { get; set; }

        [DataMember]
        public string AddProfId2 { get; set; }

        [DataMember]
        public string AddProfNo2 { get; set; }

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
        public string EmailId { get; set; }

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
        public string EmailId_p { get; set; }

        [DataMember]
        public string Area_Category { get; set; }

        [DataMember]
        public string YearsOfStay { get; set; }

        [DataMember]
        public string MemNamePBook { get; set; }

        [DataMember]
        public string AccNo { get; set; }

        [DataMember]
        public string IFSCCode { get; set; }

        [DataMember]
        public string Acc_Type { get; set; }

        [DataMember]
        public string B_FName { get; set; }

        [DataMember]
        public string B_MName { get; set; }

        [DataMember]
        public string B_LName { get; set; }

        [DataMember]
        public string DOB { get; set; }

        [DataMember]
        public string B_Age { get; set; }

        [DataMember]
        public string B_Gender { get; set; }

        [DataMember]
        public string B_HumanRelationId { get; set; }

        [DataMember]
        public string B_RelgId { get; set; }

        [DataMember]
        public string B_Caste { get; set; }

        [DataMember]
        public string B_QualificationId { get; set; }

        [DataMember]
        public string B_OccupationId { get; set; }

        [DataMember]
        public string B_IdentyProfId { get; set; }

        [DataMember]
        public string B_IdentyProfNo { get; set; }

        [DataMember]
        public string B_AddProfId { get; set; }

        [DataMember]
        public string B_AddProfNo { get; set; }

        [DataMember]
        public string B_HouseNo { get; set; }

        [DataMember]
        public string B_Street { get; set; }

        [DataMember]
        public string B_Area { get; set; }

        [DataMember]
        public string B_VillageID { get; set; }

        [DataMember]
        public string B_WardNo { get; set; }

        [DataMember]
        public string B_Landmark { get; set; }

        [DataMember]
        public string B_PostOff { get; set; }

        [DataMember]
        public string B_PIN { get; set; }

        [DataMember]
        public string B_Mobile { get; set; }

        [DataMember]
        public string B_Email { get; set; }

        [DataMember]
        public string GuarFName { get; set; }

        [DataMember]
        public string GuarLName { get; set; }

        [DataMember]
        public string GuarDOB { get; set; }

        [DataMember]
        public string GuarAge { get; set; }

        [DataMember]
        public string GuarGen { get; set; }

        [DataMember]
        public string GuarRel { get; set; }

        [DataMember]
        public string No_of_House_Member { get; set; }

        [DataMember]
        public string No_of_Children { get; set; }

        [DataMember]
        public string NoOfDependants { get; set; }

        [DataMember]
        public string MemStatus { get; set; }

        [DataMember]
        public string BusinessTypeId { get; set; }
        [DataMember]
        public string DeclaredInc { get; set; }
        [DataMember]
        public string IncFrequency { get; set; }
        [DataMember]
        public string CoAppBusinessTypeId { get; set; }
        [DataMember]
        public string CoAppDeclaredInc { get; set; }
        [DataMember]
        public string CoAppIncFrequency { get; set; }
        [DataMember]
        public string MemEmailId { get; set; }
        [DataMember]
        public string CoAppMaritalStat { get; set; }
        [DataMember]
        public string BusinessActvId { get; set; }
        [DataMember]
        public string CoAppBusinessActvId { get; set; }
        [DataMember]
        public string EMI_Obligation { get; set; }
        [DataMember]
        public string EligibleEMI { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string EnqDate { get; set; }

        [DataMember]
        public string CoAppHouseNo { get; set; }

        [DataMember]
        public string CoAppStreet { get; set; }

        [DataMember]
        public string CoAppArea { get; set; }

        [DataMember]
        public string CoAppVillageId { get; set; }

        [DataMember]
        public string CoAppSubDistrict { get; set; }

        [DataMember]
        public string CoAppDistrict { get; set; }

        [DataMember]
        public string CoAppLandmark { get; set; }

        [DataMember]
        public string CoAppPostOffice { get; set; }

        [DataMember]
        public string CoAppStateId { get; set; }

        [DataMember]
        public string CoAppPinCode { get; set; }

        [DataMember]
        public string CoAppState { get; set; }

        [DataMember]
        public string IsAbledYN { get; set; }

        [DataMember]
        public string SpeciallyAbled { get; set; }


        public ExstMemberData(string MemberId, string Distance_frm_Branch, string Distance_frm_Coll_Center, string MF_Name, string MM_Name, string ML_Name, string MDOB,
            string Age, string M_Gender, string MM_Status, string M_RelgId, string M_Caste, string M_QualificationId, string M_OccupationId, string FamilyPersonName,
            string HumanRelationId, string MaidenNmF, string MaidenNmM, string MaidenNmL, string IdentyPRofId, string IdentyProfNo, string AddProfId, string AddProfNo,
            string AddProfId2, string AddProfNo2, string AddrType, string HouseNo, string Street, string Area, string Village, string SubDistrict, string District,
            string State, string Landmark, string PostOff, string PIN, string MobileNo, string EmailId, string AddrType_p, string HouseNo_p, string Street_p, string Area_p,
            string VillageId_p, string SubDistrict_p, string Landmark_p, string PostOff_p, string PIN_p, string MobileNo_p, string EmailId_p, string Area_Category,
            string YearsOfStay, string MemNamePBook, string AccNo, string IFSCCode, string Acc_Type, string B_FName, string B_MName, string B_LName, string DOB, string B_Age,
            string B_Gender, string B_HumanRelationId, string B_RelgId, string B_Caste, string B_QualificationId, string B_OccupationId, string B_IdentyProfId, string B_IdentyProfNo,
            string B_AddProfId, string B_AddProfNo, string B_HouseNo, string B_Street, string B_Area, string B_VillageID, string B_WardNo, string B_Landmark, string B_PostOff,
            string B_PIN, string B_Mobile, string B_Email, string GuarFName, string GuarLName, string GuarDOB, string GuarAge, string GuarGen, string GuarRel,
            string No_of_House_Member, string No_of_Children, string NoOfDependants, string MemStatus,
            string BusinessTypeId, string DeclaredInc, string IncFrequency, string CoAppBusinessTypeId, string CoAppDeclaredInc,
            string CoAppIncFrequency, string MemEmailId, string CoAppMaritalStat, string BusinessActvId, string CoAppBusinessActvId, string EMI_Obligation,
            string EligibleEMI, string ImageUrl, string EnqDate, string CoAppHouseNo, string CoAppStreet, string CoAppArea, string CoAppVillageId, string CoAppSubDistrict,
            string CoAppDistrict, string CoAppLandmark, string CoAppPostOffice, string CoAppStateId, string CoAppPinCode, string CoAppState,
            string IsAbledYN, string SpeciallyAbled)
        {
            this.MemberId = MemberId;
            this.Distance_frm_Branch = Distance_frm_Branch;
            this.Distance_frm_Coll_Center = Distance_frm_Coll_Center;
            this.MF_Name = MF_Name;
            this.MM_Name = MM_Name;
            this.ML_Name = ML_Name;
            this.MDOB = MDOB;
            this.Age = Age;
            this.M_Gender = M_Gender;
            this.MM_Status = MM_Status;
            this.M_RelgId = M_RelgId;
            this.M_Caste = M_Caste;
            this.M_QualificationId = M_QualificationId;
            this.M_OccupationId = M_OccupationId;
            this.FamilyPersonName = FamilyPersonName;
            this.HumanRelationId = HumanRelationId;
            this.MaidenNmF = MaidenNmF;
            this.MaidenNmM = MaidenNmM;
            this.MaidenNmL = MaidenNmL;
            this.IdentyPRofId = IdentyPRofId;
            this.IdentyProfNo = IdentyProfNo;
            this.AddProfId = AddProfId;
            this.AddProfNo = AddProfNo;
            this.AddProfId2 = AddProfId2;
            this.AddProfNo2 = AddProfNo2;
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
            this.EmailId = EmailId;
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
            this.EmailId_p = EmailId_p;
            this.Area_Category = Area_Category;
            this.YearsOfStay = YearsOfStay;
            this.MemNamePBook = MemNamePBook;
            this.AccNo = AccNo;
            this.IFSCCode = IFSCCode;
            this.Acc_Type = Acc_Type;
            this.B_FName = B_FName;
            this.B_MName = B_MName;
            this.B_LName = B_LName;
            this.DOB = DOB;
            this.B_Age = B_Age;
            this.B_Gender = B_Gender;
            this.B_HumanRelationId = B_HumanRelationId;
            this.B_RelgId = B_RelgId;
            this.B_Caste = B_Caste;
            this.B_QualificationId = B_QualificationId;
            this.B_OccupationId = B_OccupationId;
            this.B_IdentyProfId = B_IdentyProfId;
            this.B_IdentyProfNo = B_IdentyProfNo;
            this.B_AddProfId = B_AddProfId;
            this.B_AddProfNo = B_AddProfNo;
            this.B_HouseNo = B_HouseNo;
            this.B_Street = B_Street;
            this.B_Area = B_Area;
            this.B_VillageID = B_VillageID;
            this.B_WardNo = B_WardNo;
            this.B_Landmark = B_Landmark;
            this.B_PostOff = B_PostOff;
            this.B_PIN = B_PIN;
            this.B_Mobile = B_Mobile;
            this.B_Email = B_Email;
            this.GuarFName = GuarFName;
            this.GuarLName = GuarLName;
            this.GuarDOB = GuarDOB;
            this.GuarAge = GuarAge;
            this.GuarGen = GuarGen;
            this.GuarRel = GuarRel;
            this.No_of_House_Member = No_of_House_Member;
            this.No_of_Children = No_of_Children;
            this.NoOfDependants = NoOfDependants;
            this.MemStatus = MemStatus;

            this.BusinessTypeId = BusinessTypeId;
            this.DeclaredInc = DeclaredInc;
            this.IncFrequency = IncFrequency;
            this.CoAppBusinessTypeId = CoAppBusinessTypeId;
            this.CoAppDeclaredInc = CoAppDeclaredInc;
            this.CoAppIncFrequency = CoAppIncFrequency;
            this.MemEmailId = MemEmailId;
            this.CoAppMaritalStat = CoAppMaritalStat;
            this.BusinessActvId = BusinessActvId;
            this.CoAppBusinessActvId = CoAppBusinessActvId;
            this.EMI_Obligation = EMI_Obligation;
            this.EligibleEMI = EligibleEMI;
            this.ImageUrl = ImageUrl;
            this.EnqDate = EnqDate;

            this.CoAppHouseNo = CoAppHouseNo;
            this.CoAppStreet = CoAppStreet;
            this.CoAppArea = CoAppArea;
            this.CoAppVillageId = CoAppVillageId;
            this.CoAppSubDistrict = CoAppSubDistrict;
            this.CoAppDistrict = CoAppDistrict;
            this.CoAppLandmark = CoAppLandmark;
            this.CoAppPostOffice = CoAppPostOffice;
            this.CoAppStateId = CoAppStateId;
            this.CoAppPinCode = CoAppPinCode;
            this.CoAppState = CoAppState;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class IncomeData
    {
        [DataMember]
        public string FamilyIncome { get; set; }

        [DataMember]
        public string SlefIncome { get; set; }

        [DataMember]
        public string OtherIncome { get; set; }

        [DataMember]
        public string TotIncome { get; set; }

        [DataMember]
        public string ExHsRntAmt { get; set; }

        [DataMember]
        public string ExpFdAmt { get; set; }

        [DataMember]
        public string ExpEduAmt { get; set; }

        [DataMember]
        public string ExpMedAmt { get; set; }

        [DataMember]
        public string ExpLnInsAmt { get; set; }

        [DataMember]
        public string ExpFuelAmt { get; set; }

        [DataMember]
        public string ExpElectricAmt { get; set; }

        [DataMember]
        public string ExpTransAmt { get; set; }

        [DataMember]
        public string ExpOtherAmt { get; set; }

        [DataMember]
        public string TotalExp { get; set; }

        [DataMember]
        public string Surplus { get; set; }

        [DataMember]
        public string OtherIncomeSrcId { get; set; }

        public IncomeData(string FamilyIncome, string SlefIncome, string OtherIncome, string TotIncome, string ExHsRntAmt, string ExpFdAmt, string ExpEduAmt, string ExpMedAmt,
            string ExpLnInsAmt, string ExpFuelAmt, string ExpElectricAmt, string ExpTransAmt, string ExpOtherAmt, string TotalExp, string Surplus,
            string OtherIncomeSrcId)
        {
            this.FamilyIncome = FamilyIncome;
            this.SlefIncome = SlefIncome;
            this.OtherIncome = OtherIncome;
            this.TotIncome = TotIncome;
            this.ExHsRntAmt = ExHsRntAmt;
            this.ExpFdAmt = ExpFdAmt;
            this.ExpEduAmt = ExpEduAmt;
            this.ExpMedAmt = ExpMedAmt;
            this.ExpLnInsAmt = ExpLnInsAmt;
            this.ExpFuelAmt = ExpFuelAmt;
            this.ExpElectricAmt = ExpElectricAmt;
            this.ExpTransAmt = ExpTransAmt;
            this.ExpOtherAmt = ExpOtherAmt;
            this.TotalExp = TotalExp;
            this.Surplus = Surplus;
            this.OtherIncomeSrcId = OtherIncomeSrcId;
        }
    }

    [DataContract]
    public class AssetData
    {
        [DataMember]
        public string AssetName { get; set; }

        [DataMember]
        public string AssetQty { get; set; }

        [DataMember]
        public string AssetAmt { get; set; }

        public AssetData(string AssetName, string AssetQty, string AssetAmt)
        {
            this.AssetName = AssetName;
            this.AssetQty = AssetQty;
            this.AssetAmt = AssetAmt;
        }
    }

    [DataContract]
    public class ExistingMemberAllData
    {
        [DataMember]
        public List<ExstMemberData> ExstMemberData { get; set; }

        [DataMember]
        public List<IncomeData> IncomeData { get; set; }

        [DataMember]
        public List<AssetData> AssetData { get; set; }

        [DataMember]
        public List<EarningMemberDtl> EarningMemDtlData { get; set; }

        public ExistingMemberAllData(List<ExstMemberData> ExstMemberData, List<IncomeData> IncomeData, List<AssetData> AssetData,
            List<EarningMemberDtl> EarningMemDtlData)
        {
            this.ExstMemberData = ExstMemberData;
            this.IncomeData = IncomeData;
            this.AssetData = AssetData;
            this.EarningMemDtlData = EarningMemDtlData;
        }
    }

    [DataContract]
    public class QNAData
    {
        [DataMember]
        public string Qno { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public string AnsNo { get; set; }

        [DataMember]
        public string Ans { get; set; }

        [DataMember]
        public string Score { get; set; }

        public QNAData(string Qno, string Question, string AnsNo, string Ans, string Score)
        {
            this.Qno = Qno;
            this.Question = Question;
            this.AnsNo = AnsNo;
            this.Ans = Ans;
            this.Score = Score;
        }
    }

    [DataContract]
    public class QNADataNew
    {
        [DataMember]
        public string Qno { get; set; }

        [DataMember]
        public string Question { get; set; }

        [DataMember]
        public string AnsNo { get; set; }

        [DataMember]
        public string Ans { get; set; }

        [DataMember]
        public string Score { get; set; }

        [DataMember]
        public string Weighted { get; set; }

        public QNADataNew(string Qno, string Question, string AnsNo, string Ans, string Score, string Weighted)
        {
            this.Qno = Qno;
            this.Question = Question;
            this.AnsNo = AnsNo;
            this.Ans = Ans;
            this.Score = Score;
            this.Weighted = Weighted;
        }
    }



    [DataContract]
    public class MemberHVData
    {
        [DataMember]
        public string Eoid { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string CGTId { get; set; }

        [DataMember]
        public string Member { get; set; }

        [DataMember]
        public string marketId { get; set; }

        [DataMember]
        public string marketName { get; set; }

        public MemberHVData(string Eoid, string EoName, string Groupid, string GroupName, string MemberID, string CGTId, string Member, string marketId, string marketName)
        {
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.Groupid = Groupid;
            this.GroupName = GroupName;
            this.MemberID = MemberID;
            this.CGTId = CGTId;
            this.Member = Member;
            this.marketId = marketId;
            this.marketName = marketName;
        }
    }

    [DataContract]
    public class PurposeData
    {
        [DataMember]
        public string PurposeID { get; set; }

        [DataMember]
        public string Purpose { get; set; }

        [DataMember]
        public string SubPurposeID { get; set; }

        [DataMember]
        public string SubPurpose { get; set; }

        public PurposeData(string PurposeID, string Purpose, string SubPurposeID, string SubPurpose)
        {
            this.PurposeID = PurposeID;
            this.Purpose = Purpose;
            this.SubPurposeID = SubPurposeID;
            this.SubPurpose = SubPurpose;
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
        public string AssetType { get; set; }

        public SchemeData(string LoanTypeId, string LoanType, string AssetType)
        {
            this.LoanTypeId = LoanTypeId;
            this.LoanType = LoanType;
            this.AssetType = AssetType;
        }
    }

    [DataContract]
    public class MemberLoanAppData
    {
        [DataMember]
        public string Eoid { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string CGTId { get; set; }

        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string marketId { get; set; }

        [DataMember]
        public string marketName { get; set; }

        [DataMember]
        public string GroupType { get; set; }

        [DataMember]
        public string GroupImg { get; set; }

        [DataMember]
        public string IsAbledYN { get; set; }

        [DataMember]
        public string SpeciallyAbled { get; set; }


        public MemberLoanAppData(string Eoid, string EoName, string Groupid, string GroupName, string CGTId, string MemberID, string MemberName,
            string marketId, string marketName, string GroupType, string GroupImg, string IsAbledYN, string SpeciallyAbled)
        {
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.Groupid = Groupid;
            this.GroupName = GroupName;
            this.CGTId = CGTId;
            this.MemberID = MemberID;
            this.MemberName = MemberName;
            this.marketId = marketId;
            this.marketName = marketName;
            this.GroupType = GroupType;
            this.GroupImg = GroupImg;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class MemberLoanDtlData
    {
        [DataMember]
        public string EnquiryId { get; set; }

        [DataMember]
        public string AmountApplied { get; set; }

        [DataMember]
        public string MemAddr { get; set; }

        [DataMember]
        public string M_Mobile { get; set; }

        [DataMember]
        public string EmailId { get; set; }

        [DataMember]
        public string M_IdentyPRofId { get; set; }

        [DataMember]
        public string M_IdentyProfNo { get; set; }

        [DataMember]
        public string M_AddProfId { get; set; }

        [DataMember]
        public string M_AddProfNo { get; set; }

        [DataMember]
        public string AddProfId2 { get; set; }

        [DataMember]
        public string AddProfNo2 { get; set; }

        [DataMember]
        public string CoBrwrName { get; set; }

        [DataMember]
        public string CoBrwrAge { get; set; }

        [DataMember]
        public string CoBrwrRelation { get; set; }

        [DataMember]
        public string CoBrwrAddr { get; set; }

        [DataMember]
        public string GuarName { get; set; }

        [DataMember]
        public string GuarAge { get; set; }

        [DataMember]
        public string GuarRel { get; set; }

        [DataMember]
        public string FamilyIncome { get; set; }

        [DataMember]
        public string SlefIncome { get; set; }

        [DataMember]
        public string OtherIncome { get; set; }

        [DataMember]
        public string TotIncome { get; set; }

        [DataMember]
        public string ExHsRntAmt { get; set; }

        [DataMember]
        public string ExpFdAmt { get; set; }

        [DataMember]
        public string ExpEduAmt { get; set; }

        [DataMember]
        public string ExpMedAmt { get; set; }

        [DataMember]
        public string ExpLnInsAmt { get; set; }

        [DataMember]
        public string ExpFuelAmt { get; set; }

        [DataMember]
        public string ExpElectricAmt { get; set; }

        [DataMember]
        public string ExpTransAmt { get; set; }

        [DataMember]
        public string ExpOtherAmt { get; set; }

        [DataMember]
        public string TotalExp { get; set; }

        [DataMember]
        public string Surplus { get; set; }

        [DataMember]
        public string AssetsDetails { get; set; }

        [DataMember]
        public string Q1A { get; set; }

        [DataMember]
        public string Q2A { get; set; }

        [DataMember]
        public string Q3A { get; set; }

        [DataMember]
        public string Q4A { get; set; }

        [DataMember]
        public string Q5A { get; set; }

        [DataMember]
        public string Q6A { get; set; }

        [DataMember]
        public string Q7A { get; set; }

        [DataMember]
        public string Q8A { get; set; }

        [DataMember]
        public string Q9A { get; set; }

        [DataMember]
        public string Q10A { get; set; }

        [DataMember]
        public string Q11A { get; set; }

        [DataMember]
        public string Q12A { get; set; }

        [DataMember]
        public string Q13A { get; set; }

        [DataMember]
        public string AAdharNo { get; set; }

        [DataMember]
        public string bnk_acc { get; set; }

        [DataMember]
        public string ifsc_num { get; set; }

        [DataMember]
        public string Q14A { get; set; }

        [DataMember]
        public string Q15ElectricYN { get; set; }
        [DataMember]
        public string Q15WaterYN { get; set; }
        [DataMember]
        public string Q15ToiletYN { get; set; }
        [DataMember]
        public string Q15SewageYN { get; set; }
        [DataMember]
        public string Q15LPGYN { get; set; }
        [DataMember]
        public string Q15A { get; set; }
        [DataMember]
        public string Q16LandYN { get; set; }
        [DataMember]
        public string Q16VehicleYN { get; set; }
        [DataMember]
        public string Q16FurnitureYN { get; set; }
        [DataMember]
        public string Q16SmartPhoneYN { get; set; }
        [DataMember]
        public string Q16ElectricItemYN { get; set; }
        [DataMember]
        public string EligibleEMI { get; set; }
        [DataMember]
        public string EMIEligibleAmt { get; set; }
        [DataMember]
        public string AmountApplied24M { get; set; }
        [DataMember]
        public string MaxLoanAmt12M { get; set; }
        [DataMember]
        public string MaxLoanAmt24M { get; set; }
        [DataMember]
        public string AssetType { get; set; }
        [DataMember]
        public string Q14SubCat { get; set; }
        [DataMember]
        public string ApplAadhaarNo { get; set; }
        [DataMember]
        public string PassbookImgUrl { get; set; }
        [DataMember]
        public string KycImgUrl { get; set; }
        [DataMember]
        public string EnqDate { get; set; }

        [DataMember]
        public string IsAbledYN { get; set; }

        [DataMember]
        public string SpeciallyAbled { get; set; }

        public MemberLoanDtlData(string EnquiryId, string AmountApplied, string MemAddr, string M_Mobile, string EmailId, string M_IdentyPRofId, string M_IdentyProfNo,
            string M_AddProfId, string M_AddProfNo, string AddProfId2, string AddProfNo2, string CoBrwrName, string CoBrwrAge, string CoBrwrRelation, string CoBrwrAddr,
            string GuarName, string GuarAge, string GuarRel, string FamilyIncome, string SlefIncome, string OtherIncome, string TotIncome, string ExHsRntAmt, string ExpFdAmt,
            string ExpEduAmt, string ExpMedAmt, string ExpLnInsAmt, string ExpFuelAmt, string ExpElectricAmt, string ExpTransAmt, string ExpOtherAmt, string TotalExp,
            string Surplus, string AssetsDetails, string Q1A, string Q2A, string Q3A, string Q4A, string Q5A, string Q6A, string Q7A, string Q8A, string Q9A, string Q10A,
            string Q11A, string Q12A, string Q13A, string AAdharNo, string bnk_acc, string ifsc_num, string Q14A
            , string Q15ElectricYN, string Q15WaterYN, string Q15ToiletYN, string Q15SewageYN, string Q15LPGYN, string Q15A
            , string Q16LandYN, string Q16VehicleYN, string Q16FurnitureYN, string Q16SmartPhoneYN, string Q16ElectricItemYN, string EligibleEMI,
            string EMIEligibleAmt, string AmountApplied24M, string MaxLoanAmt12M, string MaxLoanAmt24M, string AssetType, string Q14SubCat, string ApplAadhaarNo,
            string PassbookImgUrl, string KycImgUrl, string EnqDate, string IsAbledYN, string SpeciallyAbled
            )
        {
            this.EnquiryId = EnquiryId;
            this.AmountApplied = AmountApplied;
            this.MemAddr = MemAddr;
            this.M_Mobile = M_Mobile;
            this.EmailId = EmailId;
            this.M_IdentyPRofId = M_IdentyPRofId;
            this.M_IdentyProfNo = M_IdentyProfNo;
            this.M_AddProfId = M_AddProfId;
            this.M_AddProfNo = M_AddProfNo;
            this.AddProfId2 = AddProfId2;
            this.AddProfNo2 = AddProfNo2;
            this.CoBrwrName = CoBrwrName;
            this.CoBrwrAge = CoBrwrAge;
            this.CoBrwrRelation = CoBrwrRelation;
            this.CoBrwrAddr = CoBrwrAddr;
            this.GuarName = GuarName;
            this.GuarAge = GuarAge;
            this.GuarRel = GuarRel;
            this.FamilyIncome = FamilyIncome;
            this.SlefIncome = SlefIncome;
            this.OtherIncome = OtherIncome;
            this.TotIncome = TotIncome;
            this.ExHsRntAmt = ExHsRntAmt;
            this.ExpFdAmt = ExpFdAmt;
            this.ExpEduAmt = ExpEduAmt;
            this.ExpMedAmt = ExpMedAmt;
            this.ExpLnInsAmt = ExpLnInsAmt;
            this.ExpFuelAmt = ExpFuelAmt;
            this.ExpElectricAmt = ExpElectricAmt;
            this.ExpTransAmt = ExpTransAmt;
            this.ExpOtherAmt = ExpOtherAmt;
            this.TotalExp = TotalExp;
            this.Surplus = Surplus;
            this.AssetsDetails = AssetsDetails;
            this.Q1A = Q1A;
            this.Q2A = Q2A;
            this.Q3A = Q3A;
            this.Q4A = Q4A;
            this.Q5A = Q5A;
            this.Q6A = Q6A;
            this.Q7A = Q7A;
            this.Q8A = Q8A;
            this.Q9A = Q9A;
            this.Q10A = Q10A;
            this.Q11A = Q11A;
            this.Q12A = Q12A;
            this.Q13A = Q13A;
            this.AAdharNo = AAdharNo;
            this.bnk_acc = bnk_acc;
            this.ifsc_num = ifsc_num;
            this.Q14A = Q14A;

            this.Q15ElectricYN = Q15ElectricYN;
            this.Q15WaterYN = Q15WaterYN;
            this.Q15ToiletYN = Q15ToiletYN;
            this.Q15SewageYN = Q15SewageYN;
            this.Q15LPGYN = Q15LPGYN;
            this.Q15A = Q15A;
            this.Q16LandYN = Q16LandYN;
            this.Q16VehicleYN = Q16VehicleYN;
            this.Q16FurnitureYN = Q16FurnitureYN;
            this.Q16SmartPhoneYN = Q16SmartPhoneYN;
            this.Q16ElectricItemYN = Q16ElectricItemYN;

            this.EligibleEMI = EligibleEMI;
            this.EMIEligibleAmt = EMIEligibleAmt;
            this.AmountApplied24M = AmountApplied24M;
            this.MaxLoanAmt12M = MaxLoanAmt12M;
            this.MaxLoanAmt24M = MaxLoanAmt24M;
            this.AssetType = AssetType;
            this.Q14SubCat = Q14SubCat;
            this.ApplAadhaarNo = ApplAadhaarNo;
            this.PassbookImgUrl = PassbookImgUrl;
            this.KycImgUrl = KycImgUrl;
            this.EnqDate = EnqDate;
            this.IsAbledYN = IsAbledYN;
            this.SpeciallyAbled = SpeciallyAbled;
        }
    }

    [DataContract]
    public class MemberLoanDtlByTypeData
    {
        [DataMember]
        public string TRN { get; set; }

        [DataMember]
        public string LoanAmt { get; set; }

        [DataMember]
        public string LoanCycle { get; set; }

        [DataMember]
        public string ApLoanCycle { get; set; }

        [DataMember]
        public string PrvLoanYN { get; set; }

        [DataMember]
        public string AppliedAmt { get; set; }

        public MemberLoanDtlByTypeData(string TRN, string LoanAmt, string LoanCycle, string ApLoanCycle, string PrvLoanYN, string AppliedAmt)
        {
            this.TRN = TRN;
            this.LoanAmt = LoanAmt;
            this.LoanCycle = LoanCycle;
            this.ApLoanCycle = ApLoanCycle;
            this.PrvLoanYN = PrvLoanYN;
            this.AppliedAmt = AppliedAmt;
        }
    }

    [DataContract]
    public class MemberLoanDashboardData
    {
        [DataMember]
        public string TotalMember { get; set; }

        [DataMember]
        public string LoanApplication { get; set; }

        [DataMember]
        public string TotalMemberCurrent { get; set; }

        [DataMember]
        public string TotalLoanCurrent { get; set; }

        public MemberLoanDashboardData(string TotalMember, string LoanApplication, string TotalMemberCurrent, string TotalLoanCurrent)
        {
            this.TotalMember = TotalMember;
            this.LoanApplication = LoanApplication;
            this.TotalMemberCurrent = TotalMemberCurrent;
            this.TotalLoanCurrent = TotalLoanCurrent;
        }
    }

    [DataContract]
    public class GetReHouseData
    {
        [DataMember]
        public string Eoid { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string LoanAppId { get; set; }

        [DataMember]
        public string LoanAppAmt { get; set; }

        [DataMember]
        public string EnquiryId { get; set; }

        [DataMember]
        public string EnqDate { get; set; }

        [DataMember]
        public string CGTID { get; set; }

        [DataMember]
        public string ColDay { get; set; }

        [DataMember]
        public string MemAddr { get; set; }

        [DataMember]
        public string M_Mobile { get; set; }

        [DataMember]
        public string EmailId { get; set; }

        [DataMember]
        public string M_IdentyPRofId { get; set; }

        [DataMember]
        public string M_IdentyProfNo { get; set; }

        [DataMember]
        public string M_AddProfId { get; set; }

        [DataMember]
        public string M_AddProfNo { get; set; }

        [DataMember]
        public string AddProfId2 { get; set; }

        [DataMember]
        public string AddProfNo2 { get; set; }

        [DataMember]
        public string CoBrwrName { get; set; }

        [DataMember]
        public string CoBrwrAge { get; set; }

        [DataMember]
        public string CoBrwrRelation { get; set; }

        [DataMember]
        public string CoBrwrAddr { get; set; }

        [DataMember]
        public string GuarName { get; set; }

        [DataMember]
        public string GuarAge { get; set; }

        [DataMember]
        public string GuarRel { get; set; }

        [DataMember]
        public string FamilyIncome { get; set; }

        [DataMember]
        public string SlefIncome { get; set; }

        [DataMember]
        public string OtherIncome { get; set; }

        [DataMember]
        public string TotIncome { get; set; }

        [DataMember]
        public string ExHsRntAmt { get; set; }

        [DataMember]
        public string ExpFdAmt { get; set; }

        [DataMember]
        public string ExpEduAmt { get; set; }

        [DataMember]
        public string ExpMedAmt { get; set; }

        [DataMember]
        public string ExpLnInsAmt { get; set; }

        [DataMember]
        public string ExpFuelAmt { get; set; }

        [DataMember]
        public string ExpElectricAmt { get; set; }

        [DataMember]
        public string ExpTransAmt { get; set; }

        [DataMember]
        public string ExpOtherAmt { get; set; }

        [DataMember]
        public string TotalExp { get; set; }

        [DataMember]
        public string Surplus { get; set; }

        [DataMember]
        public string AssetsDetails { get; set; }

        [DataMember]
        public string Q1A { get; set; }

        [DataMember]
        public string Q2A { get; set; }

        [DataMember]
        public string Q3A { get; set; }

        [DataMember]
        public string Q4A { get; set; }

        [DataMember]
        public string Q5A { get; set; }

        [DataMember]
        public string Q6A { get; set; }

        [DataMember]
        public string Q7A { get; set; }

        [DataMember]
        public string Q8A { get; set; }

        [DataMember]
        public string Q9A { get; set; }

        [DataMember]
        public string Q10A { get; set; }

        [DataMember]
        public string Q11A { get; set; }

        [DataMember]
        public string Q12A { get; set; }

        [DataMember]
        public string Q13A { get; set; }

        [DataMember]
        public string marketId { get; set; }

        [DataMember]
        public string marketName { get; set; }

        [DataMember]
        public string Q14A { get; set; }
        [DataMember]
        public string Q15A { get; set; }

        [DataMember]
        public string Q15ElectricYN { get; set; }
        [DataMember]
        public string Q15WaterYN { get; set; }
        [DataMember]
        public string Q15ToiletYN { get; set; }
        [DataMember]
        public string Q15SewageYN { get; set; }
        [DataMember]
        public string Q15LPGYN { get; set; }
        [DataMember]
        public string Q16LandYN { get; set; }
        [DataMember]
        public string Q16VehicleYN { get; set; }
        [DataMember]
        public string Q16FurnitureYN { get; set; }
        [DataMember]
        public string Q16SmartPhoneYN { get; set; }
        [DataMember]
        public string Q16ElectricItemYN { get; set; }
        [DataMember]
        public string Q14SubCat { get; set; }
        [DataMember]
        public string EnqqDate { get; set; }

        public GetReHouseData(string Eoid, string EoName, string Groupid, string GroupName, string MemberID, string MemberName, string LoanAppId, string LoanAppAmt, string EnquiryId,
            string EnqDate, string CGTID, string ColDay, string MemAddr, string M_Mobile, string EmailId, string M_IdentyPRofId, string M_IdentyProfNo, string M_AddProfId, string M_AddProfNo,
            string AddProfId2, string AddProfNo2, string CoBrwrName, string CoBrwrAge, string CoBrwrRelation, string CoBrwrAddr, string GuarName, string GuarAge, string GuarRel,
            string FamilyIncome, string SlefIncome, string OtherIncome, string TotIncome, string ExHsRntAmt, string ExpFdAmt, string ExpEduAmt, string ExpMedAmt, string ExpLnInsAmt,
            string ExpFuelAmt, string ExpElectricAmt, string ExpTransAmt, string ExpOtherAmt, string TotalExp, string Surplus, string AssetsDetails, string Q1A, string Q2A,
            string Q3A, string Q4A, string Q5A, string Q6A, string Q7A, string Q8A, string Q9A, string Q10A, string Q11A, string Q12A, string Q13A, string marketId, string marketName,
            string Q14A, string Q15A, string Q15ElectricYN, string Q15WaterYN, string Q15ToiletYN, string Q15SewageYN, string Q15LPGYN
            , string Q16LandYN, string Q16VehicleYN, string Q16FurnitureYN, string Q16SmartPhoneYN, string Q16ElectricItemYN, string Q14SubCat, string EnqqDate
            )
        {
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.Groupid = Groupid;
            this.GroupName = GroupName;
            this.MemberID = MemberID;
            this.MemberName = MemberName;
            this.LoanAppId = LoanAppId;
            this.LoanAppAmt = LoanAppAmt;
            this.EnquiryId = EnquiryId;
            this.EnqDate = EnqDate;
            this.CGTID = CGTID;
            this.ColDay = ColDay;
            this.MemAddr = MemAddr;
            this.M_Mobile = M_Mobile;
            this.EmailId = EmailId;
            this.M_IdentyPRofId = M_IdentyPRofId;
            this.M_IdentyProfNo = M_IdentyProfNo;
            this.M_AddProfId = M_AddProfId;
            this.M_AddProfNo = M_AddProfNo;
            this.AddProfId2 = AddProfId2;
            this.AddProfNo2 = AddProfNo2;
            this.CoBrwrName = CoBrwrName;
            this.CoBrwrAge = CoBrwrAge;
            this.CoBrwrRelation = CoBrwrRelation;
            this.CoBrwrAddr = CoBrwrAddr;
            this.GuarName = GuarName;
            this.GuarAge = GuarAge;
            this.GuarRel = GuarRel;
            this.FamilyIncome = FamilyIncome;
            this.SlefIncome = SlefIncome;
            this.OtherIncome = OtherIncome;
            this.TotIncome = TotIncome;
            this.ExHsRntAmt = ExHsRntAmt;
            this.ExpFdAmt = ExpFdAmt;
            this.ExpEduAmt = ExpEduAmt;
            this.ExpMedAmt = ExpMedAmt;
            this.ExpLnInsAmt = ExpLnInsAmt;
            this.ExpFuelAmt = ExpFuelAmt;
            this.ExpElectricAmt = ExpElectricAmt;
            this.ExpTransAmt = ExpTransAmt;
            this.ExpOtherAmt = ExpOtherAmt;
            this.TotalExp = TotalExp;
            this.Surplus = Surplus;
            this.AssetsDetails = AssetsDetails;
            this.Q1A = Q1A;
            this.Q2A = Q2A;
            this.Q3A = Q3A;
            this.Q4A = Q4A;
            this.Q5A = Q5A;
            this.Q6A = Q6A;
            this.Q7A = Q7A;
            this.Q8A = Q8A;
            this.Q9A = Q9A;
            this.Q10A = Q10A;
            this.Q11A = Q11A;
            this.Q12A = Q12A;
            this.Q13A = Q13A;
            this.marketId = marketId;
            this.marketName = marketName;
            this.Q14A = Q14A;
            this.Q15A = Q15A;
            this.Q15ElectricYN = Q15ElectricYN;
            this.Q15WaterYN = Q15WaterYN;
            this.Q15ToiletYN = Q15ToiletYN;
            this.Q15SewageYN = Q15SewageYN;
            this.Q15LPGYN = Q15LPGYN;
            this.Q16LandYN = Q16LandYN;
            this.Q16VehicleYN = Q16VehicleYN;
            this.Q16FurnitureYN = Q16FurnitureYN;
            this.Q16SmartPhoneYN = Q16SmartPhoneYN;
            this.Q16ElectricItemYN = Q16ElectricItemYN;
            this.Q14SubCat = Q14SubCat;
            this.EnqqDate = EnqqDate;
        }
    }

    [DataContract]
    public class CollectionData
    {
        [DataMember]
        public string GroupId { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string MemberId { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string MemberNo { get; set; }

        [DataMember]
        public string ProductID { get; set; }

        [DataMember]
        public string IntRate { get; set; }

        [DataMember]
        public string LoanId { get; set; }

        [DataMember]
        public string LoanNo { get; set; }

        [DataMember]
        public string PrincpalDue { get; set; }

        [DataMember]
        public string PrincpalPaid { get; set; }

        [DataMember]
        public string InterestDue { get; set; }

        [DataMember]
        public string InterestPaid { get; set; }

        [DataMember]
        public string TotalDue { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string PrincOS { get; set; }

        [DataMember]
        public string ClosingType { get; set; }

        [DataMember]
        public string DescID { get; set; }

        [DataMember]
        public string AdvanceAmt { get; set; }

        [DataMember]
        public string OverDue { get; set; }

        [DataMember]
        public string SlNo { get; set; }

        [DataMember]
        public string ReffId { get; set; }

        [DataMember]
        public string ChequeNo { get; set; }

        [DataMember]
        public string CollType { get; set; }

        [DataMember]
        public string Noofinst { get; set; }

        [DataMember]
        public string PA { get; set; }

        [DataMember]
        public string LoanAc { get; set; }

        [DataMember]
        public string InstAc { get; set; }

        [DataMember]
        public string AdvAc { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public string ActionTaken { get; set; }

        [DataMember]
        public string DeathDt { get; set; }

        [DataMember]
        public string LWaveOffId { get; set; }

        [DataMember]
        public string DPerson { get; set; }

        [DataMember]
        public string IsWriteoff { get; set; }

        [DataMember]
        public string WriteOffAC { get; set; }

        [DataMember]
        public string WriteOffRecAC { get; set; }

        [DataMember]
        public string DeathType { get; set; }

        [DataMember]
        public string Duedt { get; set; }

        [DataMember]
        public string OverDueGroupYn { get; set; }

        [DataMember]
        public string GCollDay { get; set; }

        [DataMember]
        public string GColltime { get; set; }

        [DataMember]
        public string ExcAmt { get; set; }

        [DataMember]
        public string NewAdvanceAmt { get; set; }

        [DataMember]
        public string PrematureInterest { get; set; }

        [DataMember]
        public string ProvDeathDec { get; set; }

        [DataMember]
        public string InstallmentAmt { get; set; }

        [DataMember]
        public string MarketID { get; set; }

        [DataMember]
        public string MarketName { get; set; }

        [DataMember]
        public string IntOS { get; set; }

        [DataMember]
        public string SecMobNo { get; set; }

        [DataMember]
        public string DeathFlag { get; set; }

        public CollectionData(string GroupId, string GroupName, string MemberId, string MemberName, string MemberNo, string ProductID, string IntRate, string LoanId, string LoanNo,
            string PrincpalDue, string PrincpalPaid, string InterestDue, string InterestPaid, string TotalDue, string Total, string PrincOS, string ClosingType, string DescID,
            string AdvanceAmt, string OverDue, string SlNo, string ReffId, string ChequeNo, string CollType, string Noofinst, string PA, string LoanAc, string InstAc, string AdvAc,
            string Reason, string ActionTaken, string DeathDt, string LWaveOffId, string DPerson, string IsWriteoff, string WriteOffAC, string WriteOffRecAC, string DeathType,
            string Duedt, string OverDueGroupYn, string GCollDay, string GColltime, string ExcAmt, string NewAdvanceAmt, string PrematureInterest, string ProvDeathDec,
            string InstallmentAmt, string MarketID, string MarketName, string IntOS, string SecMobNo, string DeathFlag)
        {
            this.GroupId = GroupId;
            this.GroupName = GroupName;
            this.MemberId = MemberId;
            this.MemberName = MemberName;
            this.MemberNo = MemberNo;
            this.ProductID = ProductID;
            this.IntRate = IntRate;
            this.LoanId = LoanId;
            this.LoanNo = LoanNo;
            this.PrincpalDue = PrincpalDue;
            this.PrincpalPaid = PrincpalPaid;
            this.InterestDue = InterestDue;
            this.InterestPaid = InterestPaid;
            this.TotalDue = TotalDue;
            this.Total = Total;
            this.PrincOS = PrincOS;
            this.ClosingType = ClosingType;
            this.DescID = DescID;
            this.AdvanceAmt = AdvanceAmt;
            this.OverDue = OverDue;
            this.SlNo = SlNo;
            this.ReffId = ReffId;
            this.ChequeNo = ChequeNo;
            this.CollType = CollType;
            this.Noofinst = Noofinst;
            this.PA = PA;
            this.LoanAc = LoanAc;
            this.InstAc = InstAc;
            this.AdvAc = AdvAc;
            this.Reason = Reason;
            this.ActionTaken = ActionTaken;
            this.DeathDt = DeathDt;
            this.LWaveOffId = LWaveOffId;
            this.DPerson = DPerson;
            this.IsWriteoff = IsWriteoff;
            this.WriteOffAC = WriteOffAC;
            this.WriteOffRecAC = WriteOffRecAC;
            this.DeathType = DeathType;
            this.Duedt = Duedt;
            this.OverDueGroupYn = OverDueGroupYn;
            this.GCollDay = GCollDay;
            this.GColltime = GColltime;
            this.ExcAmt = ExcAmt;
            this.NewAdvanceAmt = NewAdvanceAmt;
            this.PrematureInterest = PrematureInterest;
            this.ProvDeathDec = ProvDeathDec;
            this.InstallmentAmt = InstallmentAmt;
            this.MarketID = MarketID;
            this.MarketName = MarketName;
            this.IntOS = IntOS;
            this.SecMobNo = SecMobNo;
            this.DeathFlag = DeathFlag;
        }
    }

    [DataContract]
    public class GetCenterListData
    {
        [DataMember]
        public string MarketId { get; set; }

        [DataMember]
        public string MarketName { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string GroupType { get; set; }

        [DataMember]
        public string MarketType { get; set; }


        public GetCenterListData(string MarketId, string MarketName, string Groupid, string GroupName, string GroupType, string MarketType)
        {
            this.MarketId = MarketId;
            this.MarketName = MarketName;
            this.Groupid = Groupid;
            this.GroupName = GroupName;
            this.GroupType = GroupType;
            this.MarketType = MarketType;
        }
    }

    [DataContract]
    public class LoanUtilizationData
    {
        [DataMember]
        public string LoanId { get; set; }

        [DataMember]
        public string LoanNo { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string dose { get; set; }

        [DataMember]
        public string LoanDate { get; set; }

        [DataMember]
        public string LoanAmt { get; set; }

        [DataMember]
        public string Purpose { get; set; }

        [DataMember]
        public string SubPurpose { get; set; }

        [DataMember]
        public string GroupId { get; set; }

        [DataMember]
        public string LoanUTLType { get; set; }

        [DataMember]
        public string LoanUTLRemarks { get; set; }

        [DataMember]
        public string LoanUTLVia { get; set; }

        [DataMember]
        public string LoanUTLAmt { get; set; }

        [DataMember]
        public string VerifiedBy { get; set; }

        [DataMember]
        public string VerificationDate { get; set; }

        [DataMember]
        public string isSamePurpose { get; set; }


        public LoanUtilizationData(string LoanId, string LoanNo, string MemberName, string dose, string LoanDate, string LoanAmt, string Purpose, string SubPurpose,
            string GroupId, string LoanUTLType, string LoanUTLRemarks, string LoanUTLVia, string LoanUTLAmt, string VerifiedBy, string VerificationDate, string isSamePurpose)
        {
            this.LoanId = LoanId;
            this.LoanNo = LoanNo;
            this.MemberName = MemberName;
            this.dose = dose;
            this.LoanDate = LoanDate;
            this.LoanAmt = LoanAmt;
            this.Purpose = Purpose;
            this.SubPurpose = SubPurpose;
            this.GroupId = GroupId;
            this.LoanUTLType = LoanUTLType;
            this.LoanUTLRemarks = LoanUTLRemarks;
            this.LoanUTLVia = LoanUTLVia;
            this.LoanUTLAmt = LoanUTLAmt;
            this.VerifiedBy = VerifiedBy;
            this.VerificationDate = VerificationDate;
            this.isSamePurpose = isSamePurpose;
        }


    }

    [DataContract]
    public class LucCenterGroupEoList
    {
        [DataMember]
        public string Eoid { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string Groupid { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string marketId { get; set; }

        [DataMember]
        public string marketName { get; set; }

        public LucCenterGroupEoList(string Eoid, string EoName, string Groupid, string GroupName, string marketId, string marketName)
        {
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.Groupid = Groupid;
            this.GroupName = GroupName;
            this.marketId = marketId;
            this.marketName = marketName;
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
    public class ICICBankTransactionStatusResponse
    {
        [DataMember]
        public string URN { get; set; }
        [DataMember]
        public string UNIQUEID { get; set; }
        [DataMember]
        public string UTRNUMBER { get; set; }
        [DataMember]
        public string STATUS { get; set; }
        [DataMember]
        public string Response { get; set; }
        [DataMember]
        public string message { get; set; }

        public ICICBankTransactionStatusResponse(string URN, string UNIQUEID, string UTRNUMBER
            , string STATUS, string Response, string message)
        {
            this.URN = URN;
            this.UNIQUEID = UNIQUEID;
            this.UTRNUMBER = UTRNUMBER;
            this.STATUS = STATUS;
            this.Response = Response;
            this.message = message;
        }

    }

    [DataContract]
    public class DigitalDocumentSave
    {
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string FileName { get; set; }
        public DigitalDocumentSave(string Status, string FileName)
        {
            this.Status = Status;
            this.FileName = FileName;
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
    public class EoData
    {
        [DataMember]
        public string EoID { get; set; }

        [DataMember]
        public string EoName { get; set; }

        public EoData(string EoID, string EoName)
        {
            this.EoID = EoID;
            this.EoName = EoName;
        }
    }

    [DataContract]
    public class ActivityData
    {
        [DataMember]
        public string EoID { get; set; }

        [DataMember]
        public string EoName { get; set; }

        [DataMember]
        public string CenterID { get; set; }

        [DataMember]
        public string CenterName { get; set; }

        [DataMember]
        public string GroupID { get; set; }

        [DataMember]
        public string GroupName { get; set; }

        [DataMember]
        public string MemberID { get; set; }

        [DataMember]
        public string MemberName { get; set; }

        [DataMember]
        public string RelativeName { get; set; }

        [DataMember]
        public string LoanNo { get; set; }

        [DataMember]
        public string LoanAmount { get; set; }

        [DataMember]
        public string Outstanding { get; set; }

        [DataMember]
        public string PrincipalCollectionAmount { get; set; }

        [DataMember]
        public string PrincipalODAmount { get; set; }

        [DataMember]
        public string NoOfDays { get; set; }

        [DataMember]
        public string DeathDate { get; set; }

        [DataMember]
        public string WriteOffAmount { get; set; }

        public ActivityData(string EoID, string EoName, string CenterID, string CenterName, string GroupID, string GroupName, string MemberID, string MemberName
            , string RelativeName, string LoanNo, string LoanAmount, string Outstanding, string PrincipalCollectionAmount, string PrincipalODAmount, string NoOfDays
            , string DeathDate, string WriteOffAmount
            )
        {
            this.EoID = EoID;
            this.EoName = EoName;
            this.CenterID = CenterID;
            this.CenterName = CenterName;
            this.GroupID = GroupID;
            this.GroupName = GroupName;
            this.MemberID = MemberID;
            this.MemberName = MemberName;
            this.RelativeName = RelativeName;
            this.LoanNo = LoanNo;
            this.LoanAmount = LoanAmount;
            this.Outstanding = Outstanding;
            this.PrincipalCollectionAmount = PrincipalCollectionAmount;
            this.PrincipalODAmount = PrincipalODAmount;
            this.NoOfDays = NoOfDays;
            this.DeathDate = DeathDate;
            this.WriteOffAmount = WriteOffAmount;
        }
    }

    [DataContract]
    public class EarningMemberDtl
    {
        [DataMember]
        public string SlNo { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DOB { get; set; }

        [DataMember]
        public string RelationId { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string PinCode { get; set; }

        [DataMember]
        public string MobileNo { get; set; }

        [DataMember]
        public string IdentyPRofId { get; set; }

        [DataMember]
        public string IdentyProfNo { get; set; }

        [DataMember]
        public string BusinessTypeId { get; set; }

        [DataMember]
        public string OccupationId { get; set; }

        [DataMember]
        public string DeclaredIncome { get; set; }

        [DataMember]
        public string IncomeFrequencyId { get; set; }

        [DataMember]
        public string BusinessActvId { get; set; }

        public EarningMemberDtl(string SlNo, string Name, string DOB, string RelationId, string Address, string StateId,
            string PinCode, string MobileNo, string IdentyPRofId, string IdentyProfNo, string BusinessTypeId, string OccupationId,
            string DeclaredIncome, string IncomeFrequencyId, string BusinessActvId)
        {
            this.SlNo = SlNo;
            this.Name = Name;
            this.DOB = DOB;
            this.RelationId = RelationId;
            this.Address = Address;
            this.StateId = StateId;
            this.PinCode = PinCode;
            this.MobileNo = MobileNo;
            this.IdentyPRofId = IdentyPRofId;
            this.IdentyProfNo = IdentyProfNo;
            this.BusinessTypeId = BusinessTypeId;
            this.OccupationId = OccupationId;
            this.DeclaredIncome = DeclaredIncome;
            this.IncomeFrequencyId = IncomeFrequencyId;
            this.BusinessActvId = BusinessActvId;
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
    public class RampStatus
    {
        [DataMember]
        public string statusCode { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string statusDesc { get; set; }
        [DataMember]
        public string screeningId { get; set; }

        public RampStatus(string statusCode, string status, string statusDesc, string screeningId)
        {
            this.statusCode = statusCode;
            this.status = status;
            this.statusDesc = statusDesc;
            this.screeningId = screeningId;
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
    public class Result1
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
        public object tokenId { get; set; }
        [DataMember]
        public int isActive { get; set; }
    }

    [DataContract]
    public class AadhaarNoResult
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
        public List<Result1> results { get; set; }

        public AadhaarNoResult(string action, int response_code, string response_message, int total_size, int total_pages, List<Result1> results)
        {
            this.action = action;
            this.response_code = response_code;
            this.response_message = response_message;
            this.total_size = total_size;
            this.total_pages = total_pages;
            this.results = results;
        }

    }

    #region ProsidexResponse

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

    #endregion

    #region PosidexVerificationResponse

    [DataContract]
    public class Response
    {
        [DataMember]
        public string UnitySfb_RequestId { get; set; }
        [DataMember]
        public string CUSTOMER_ID { get; set; }
        [DataMember]
        public string SourceSystem { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string UCIC { get; set; }

        public Response(string UnitySfb_RequestId, string CUSTOMER_ID, string SourceSystem, string Status, string Message, string UCIC)
        {
            this.UnitySfb_RequestId = UnitySfb_RequestId;
            this.CUSTOMER_ID = CUSTOMER_ID;
            this.SourceSystem = SourceSystem;
            this.Status = Status;
            this.Message = Message;
            this.UCIC = UCIC;
        }
    }

    [DataContract]
    public class PosidexVerifyResponse
    {
        [DataMember]
        public List<Response> Response { get; set; }
        public PosidexVerifyResponse(List<Response> Response)
        {
            this.Response = Response;
        }
    }

    #endregion

    [DataContract]
    public class GetMonitoringQuestion
    {
        [DataMember]
        public string ItemId { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string ItemTypeId { get; set; }
        [DataMember]
        public string ItemTypeName { get; set; }
        [DataMember]
        public string QID { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Block { get; set; }

        public GetMonitoringQuestion(string ItemId, string ItemName, string ItemTypeId, string ItemTypeName, string QID, string Question, string Block)
        {
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.ItemTypeId = ItemTypeId;
            this.ItemTypeName = ItemTypeName;
            this.QID = QID;
            this.Question = Question;
            this.Block = Block;
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
    public class GetMonitoringCompliance
    {
        [DataMember]
        public string MonitoringId { get; set; }
        [DataMember]
        public string FromDt { get; set; }
        [DataMember]
        public string ToDt { get; set; }

        [DataMember]
        public string ItemId { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string ItemTypeId { get; set; }
        [DataMember]
        public string ItemTypeName { get; set; }
        [DataMember]
        public string QID { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Block { get; set; }
        [DataMember]
        public string MonitoringYN { get; set; }
        [DataMember]
        public string MonitoringRemarks { get; set; }

        public GetMonitoringCompliance(string MonitoringId, string FromDt, string ToDt, string ItemId, string ItemName, string ItemTypeId, string ItemTypeName, string QID, string Question, string Block, string MonitoringYN, string MonitoringRemarks)
        {
            this.MonitoringId = MonitoringId;
            this.FromDt = FromDt;
            this.ToDt = ToDt;
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.ItemTypeId = ItemTypeId;
            this.ItemTypeName = ItemTypeName;
            this.QID = QID;
            this.Question = Question;
            this.Block = Block;
            this.MonitoringYN = MonitoringYN;
            this.MonitoringRemarks = MonitoringRemarks;

        }
    }

    [DataContract]
    public class GetOtherMonitoringCompliance
    {
        [DataMember]
        public string MonitoringId { get; set; }
        [DataMember]
        public string FromDt { get; set; }
        [DataMember]
        public string ToDt { get; set; }

        [DataMember]
        public string ItemId { get; set; }
        [DataMember]
        public string ItemName { get; set; }
        [DataMember]
        public string ItemTypeId { get; set; }
        [DataMember]
        public string ItemTypeName { get; set; }
        [DataMember]
        public string QID { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string Block { get; set; }
        [DataMember]
        public string MonitoringYN { get; set; }
        [DataMember]
        public string MonitoringRemarks { get; set; }
        [DataMember]
        public string VisitType { get; set; }
        [DataMember]
        public string EoName { get; set; }
        [DataMember]
        public string Eoid { get; set; }
        [DataMember]
        public string Marketid { get; set; }
        [DataMember]
        public string Market { get; set; }
        [DataMember]
        public string MemberID { get; set; }
        [DataMember]
        public string MemberName { get; set; }

        public GetOtherMonitoringCompliance(string MonitoringId, string FromDt, string ToDt, string ItemId, string ItemName, string ItemTypeId, string ItemTypeName, string QID, string Question, string Block, string MonitoringYN, string MonitoringRemarks, string VisitType, string EoName, string Eoid, string Marketid, string Market, string MemberID, string MemberName)
        {
            this.MonitoringId = MonitoringId;
            this.FromDt = FromDt;
            this.ToDt = ToDt;
            this.ItemId = ItemId;
            this.ItemName = ItemName;
            this.ItemTypeId = ItemTypeId;
            this.ItemTypeName = ItemTypeName;
            this.QID = QID;
            this.Question = Question;
            this.Block = Block;
            this.MonitoringYN = MonitoringYN;
            this.MonitoringRemarks = MonitoringRemarks;
            this.VisitType = VisitType;
            this.EoName = EoName;
            this.Eoid = Eoid;
            this.Marketid = Marketid;
            this.Market = Market;
            this.MemberID = MemberID;
            this.MemberName = MemberName;
        }
    }

    [DataContract]
    public class GetOtherMonitoringData
    {
        [DataMember]
        public string EoName { get; set; }
        [DataMember]
        public string Eoid { get; set; }
        [DataMember]
        public string Marketid { get; set; }

        [DataMember]
        public string Market { get; set; }
        [DataMember]
        public string MemberID { get; set; }
        [DataMember]
        public string MemberName { get; set; }
        [DataMember]
        public string VisitType { get; set; }


        public GetOtherMonitoringData(string EoName, string Eoid, string Marketid, string Market, string MemberID, string MemberName, string VisitType)
        {
            this.EoName = EoName;
            this.Eoid = Eoid;
            this.Marketid = Marketid;
            this.Market = Market;
            this.MemberID = MemberID;
            this.MemberName = MemberName;
            this.VisitType = VisitType;

        }
    }

    [DataContract]
    public class GetIncentiveLoWise
    {
        [DataMember]
        public string FinalIncentiveAmount { get; set; }

        public GetIncentiveLoWise(string FinalIncentiveAmount)
        {
            this.FinalIncentiveAmount = FinalIncentiveAmount;
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
    public class ProfileResponse
    {
        [DataMember]
        public object capture_expires_at { get; set; }
        [DataMember]
        public string capture_link { get; set; }
        [DataMember]
        public string profile_id { get; set; }

        public ProfileResponse(object capture_expires_at, string capture_link, string profile_id)
        {
            this.capture_expires_at = capture_expires_at;
            this.capture_link = capture_link;
            this.profile_id = profile_id;
        }
    }

    [DataContract]
    public class PTPData
    {
        [DataMember]
        public string BranchCode { get; set; }
        [DataMember]
        public string BranchName { get; set; }
        [DataMember]
        public string Eoid { get; set; }
        [DataMember]
        public string EoName { get; set; }
        [DataMember]
        public string MarketID { get; set; }
        [DataMember]
        public string Market { get; set; }
        [DataMember]
        public string GroupNo { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string MemberNo { get; set; }
        [DataMember]
        public string SecMobileNo { get; set; }
        [DataMember]
        public string Membername { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string CoAppName { get; set; }
        [DataMember]
        public string LoanId { get; set; }
        [DataMember]
        public string LoanNo { get; set; }
        [DataMember]
        public string LoanAmt { get; set; }
        [DataMember]
        public string LoanDt { get; set; }
        [DataMember]
        public string POS { get; set; }
        [DataMember]
        public string IOS { get; set; }
        [DataMember]
        public string PAR { get; set; }
        [DataMember]
        public string Project { get; set; }

        public PTPData(string BranchCode, string BranchName, string Eoid, string EoName, string MarketID, string Market, string GroupNo, string GroupName, string MemberNo
                       , string Membername, string MobileNo, string SecMobileNo, string CoAppName, string LoanId, string LoanNo, string LoanAmt, string LoanDt, string POS,
            string IOS, string PAR, string Project)
        {
            this.BranchCode = BranchCode;
            this.BranchName = BranchName;
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.MarketID = MarketID;
            this.Market = Market;
            this.GroupNo = GroupNo;
            this.GroupName = GroupName;
            this.MemberNo = MemberNo;
            this.SecMobileNo = SecMobileNo;
            this.Membername = Membername;
            this.MobileNo = MobileNo;
            this.CoAppName = CoAppName;
            this.LoanId = LoanId;
            this.LoanNo = LoanNo;
            this.LoanAmt = LoanAmt;
            this.LoanDt = LoanDt;
            this.POS = POS;
            this.IOS = IOS;
            this.PAR = PAR;
            this.Project = Project;
        }
    }

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

    [DataContract]
    public class IdfyeSignResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusDesc { get; set; }

        public IdfyeSignResponse(string StatusCode, string StatusDesc)
        {
            this.StatusCode = StatusCode;
            this.StatusDesc = StatusDesc;
        }
    }

    [DataContract]
    public class CollQRResData
    {
        [DataMember]
        public string QRString { get; set; }
        [DataMember]
        public string BillerBillID { get; set; }
    }


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
    public class ICICIQRPaymentStatus
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string Status { get; set; }

        public ICICIQRPaymentStatus(string StatusCode, string Status)
        {
            this.StatusCode = StatusCode;
            this.Status = Status;
        }
    }

    [DataContract]
    public class GetLoanUtilizationQsAns
    {
        [DataMember]
        public string PurposeID { get; set; }
        [DataMember]
        public string SubPurposeID { get; set; }
        [DataMember]
        public string QNo { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public string AnswerType { get; set; }
        [DataMember]
        public string Answer { get; set; }

        public GetLoanUtilizationQsAns(string PurposeID, string SubPurposeID, string QNo, string Question, string AnswerType, string Answer)
        {
            this.PurposeID = PurposeID;
            this.SubPurposeID = SubPurposeID;
            this.QNo = QNo;
            this.Question = Question;
            this.AnswerType = AnswerType;
            this.Answer = Answer;

        }

    }



    [DataContract]
    public class GetLUCPendingDataList
    {
        [DataMember]
        public string Eoid { get; set; }
        [DataMember]
        public string EoName { get; set; }
        [DataMember]
        public string LoanId { get; set; }
        [DataMember]
        public string MemberId { get; set; }
        [DataMember]
        public string MemberName { get; set; }
        [DataMember]
        public string LoanAccountId { get; set; }
        [DataMember]
        public string Cycle { get; set; }
        [DataMember]
        public string DisbDate { get; set; }
        [DataMember]
        public string LoanAmt { get; set; }
        [DataMember]
        public string PurposeID { get; set; }
        [DataMember]
        public string PurposeNm { get; set; }
        [DataMember]
        public string SubPurposeId { get; set; }
        [DataMember]
        public string SubPurposeNm { get; set; }
        [DataMember]
        public string CenterID { get; set; }
        [DataMember]
        public string CenterNm { get; set; }
        [DataMember]
        public string GroupID { get; set; }
        [DataMember]
        public string GroupNm { get; set; }


        public GetLUCPendingDataList(string Eoid, string EoName, string LoanId, string MemberId, string MemberName, string LoanAccountId, string Cycle,
            string DisbDate, string LoanAmt, string PurposeID, string PurposeNm, string SubPurposeId, string SubPurposeNm, string CenterID, string CenterNm
            , string GroupID, string GroupNm)
        {
            this.Eoid = Eoid;
            this.EoName = EoName;
            this.LoanId = LoanId;
            this.MemberId = MemberId;
            this.MemberName = MemberName;
            this.LoanAccountId = LoanAccountId;
            this.Cycle = Cycle;
            this.DisbDate = DisbDate;
            this.LoanAmt = LoanAmt;
            this.PurposeID = PurposeID;
            this.PurposeNm = PurposeNm;
            this.SubPurposeId = SubPurposeId;
            this.SubPurposeNm = SubPurposeNm;
            this.CenterID = CenterID;
            this.CenterNm = CenterNm;
            this.GroupID = GroupID;
            this.GroupNm = GroupNm;
        }

    }


    [DataContract]
    public class LUCImageSave
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string FileName { get; set; }

        public LUCImageSave(string Status, string FileName)
        {
            this.Status = Status;
            this.FileName = FileName;
        }
    }

    [DataContract]
    public class GetBranchCtrlByBranchCode
    {
        [DataMember]
        public string pInitialAppJLG { get; set; }

        [DataMember]
        public string pAdvCollJLG { get; set; }

        [DataMember]
        public string pCashCollJLG { get; set; }

        [DataMember]
        public string pDigiAuthJLG { get; set; }

        [DataMember]
        public string pManualAuthJLG { get; set; }

        [DataMember]
        public string pBioAuthJLG { get; set; }

        public GetBranchCtrlByBranchCode(string pInitialAppJLG, string pAdvCollJLG, string pCashCollJLG, string pDigiAuthJLG, string pManualAuthJLG, string pBioAuthJLG)
        {
            this.pInitialAppJLG = pInitialAppJLG;
            this.pAdvCollJLG = pAdvCollJLG;
            this.pCashCollJLG = pCashCollJLG;
            this.pDigiAuthJLG = pDigiAuthJLG;
            this.pManualAuthJLG = pManualAuthJLG;
            this.pBioAuthJLG = pBioAuthJLG;
        }

    }

    public class auaKuaAuthReq
    {
        public string sourceId { get; set; }
        public string traceId { get; set; }
        public string LAT { get; set; }
        public string LONG { get; set; }
        public string DEVMACID { get; set; }
        public string DEVID { get; set; }
        public string CONSENT { get; set; }
        public string SHRC { get; set; }
        public string SERTYPE { get; set; }
        public string UDC { get; set; }
        public string AADHAARNO { get; set; }
        public string RRN { get; set; }
        public string REF { get; set; }
        public string PIDDATA { get; set; }
        public string PFR { get; set; }
        public string LANG { get; set; }
    }
}
