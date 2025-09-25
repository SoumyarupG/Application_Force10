using System;
using System.Data;
using System.Configuration;
using System.Web;

namespace FORCECA
{

    /// <summary>
    /// Application Global Value
    /// </summary>
    public class gblValue
    {
        private gblValue()
        { }

        public const string FinYear = "FinYear";
        public const string FinYrNo = "FinYrNo";
        public const string FinFromDt = "FromDt";
        public const string FinToDt = "ToDt";
        public const string LoginDate = "LogDate";
        public const string BrName = "BrName";
        public const string BrnchCode = "BrCode";
        public const string UserName = "UserNm";
        public const string ACVouMst = "ACVouMst";
        public const string ACVouDtl = "ACVouDtl";
        public const string ShortYear = "ShortYear";
        public const string DistrictId = "DistrictId";
        public const string RoleId = "RoleId";
        public const string PrematureColl = "PrematureColl";
        public const string Demise = "Demise";
        public const string UserId = "UserId";
        public const string EndDate = "EndDate";
        public const string AgencyType = "AgencyType";
        public const string Designation = "Designation";
        public const string AreaID = "AreaID";
        public const string StateID = "StateID";
        //For Report Purpose Constant Value
        //public const string CompName = "Centrum Microcredit Limited";
        public const string CompName = "Unity Small Finance Bank Limited";
        //public const string Address1 = "Address1-to be supplied";
        //public const string Address2 = "Address2-to be supplied";
        public const string Address1 = "C.S.T Road, Vidyanagari Marg, Kalina, Santacruz (East)";
        public const string Address2 = "Mumbai - 400098, India";
        public const string ViewAAdhar = "ViewAAdhar";
        public const string MultiColl = "MultiColl";
        public const string ICICINEFTYN = "ICICINEFTYN";
        public const string ICICIUser = "ICICIUser";
        public const string AllowAdvYN = "AllowAdvYN";
        public const string LoginId = "LoginId";

        public const string JlgGroupTr = "JlgGroupTr";
        public const string JlgCenterTr = "JlgCenterTr";
        public const string JLGMemberTr = "JLGMemberTr";
        public const string JLGDeviationCtrl = "JLGDeviationCtrl";

        public const string SncApprAmt = "SncApprAmt";

        //For Constant Value of the Application
        public const string Country = "India";
        public const Int32 PageHeight = 700;

        //Loan Parameter Account
        public const string PrincipalLoanAc = "LoanAc";
        public const string LoanIntAc = "InstAc";
        public const string ProcfeesAC = "ProcAC";
        public const string ServiceAC = "ServiceTaxAC";
        public const string InsuAC = "InsureAC";
        public const string OthersFeesAC = "OthersFeesAC";
        public const string PenalAc = "PenalAC";
        public const string WriteoffAC = "WriteoffAC";
        public const Int32 PgSize1 = 20;
        public const string PAIAmountAC = "PAIAmountAC";
        public const string DisbLedger = "DisbLedger";
        public const string MedClaim = "MedClaimAc";
        public const string LoanIntAccruedAc = "IntAccruedAc";

        public const string CbType = "CbType";


        //Reporting Purpose
        public const string RptCollectionEfficiency = "RptCollectionEfficiency";
        public const string LoanDisbHO = "LoanDisbHO";
        public const string LoanStatusHO = "LoanStatusHO";
        public const string RptDayWiseDPD = "RptDayWiseDPD";
        public const string RptOCRLog = "RptOCRLog";
        public const string RptLoanSanctionLog = "RptLoanSanctionLog";
        public const string RptOnTimeCollection = "RptOnTimeCollection";
        public const string RptRBIMaturity = "RptRBIMaturity";
        public const string RptFunder = "RptFunder";
        public const string RptLoanCollection = "RptLoanCollection";
        public const string RptPAR = "RptPAR";
        public const string RptPortfolio = "RptPortfolio";
        public const string RptRunDown = "RptRunDown";
        public const string NeftAPI = "NeftAPI";
        public const string HOTrial = "HOTrial";
        public const string RptIncentive = "RptIncentive";
        public const string RptProsidexLog = "RptProsidexLog";
        public const string BCBranchYN = "BCBranchYN";
        public const string ParentBranchCode = "ParentBranchCode";
        public const string ParentBranchName = "ParentBranchName";
        public const string RptHouseVisit = "RptHouseVisit";
        public const string RptGST = "RptGST";
        public const string RptPSL = "RptPSL";
        public const string RptUdyamDownload = "RptUdyamDownload";
        public const string RptIBPSL = "RptIBPSL";
        public const string rptHospiCashClaimNew = "rptHospiCashClaimNew";
        public const string rptDiscrepancy = "rptDiscrepancy";
        public const string RptOD = "RptOD";
        public const string RptGuarScheme = "RptGuarScheme";
        public const string RptAdvAdj = "RptAdvAdj";
        public const string RptGLReport = "RptGLReport";
        public const string RptOTSMasterCheckReport = "RptOTSMasterCheckReport";
        public const string RptHOCenterWiseCustomerDtlReport = "RptHOCenterWiseCustomerDtlReport";
        public const string RptHOAccLedgerDtlReport = "RptHOAccLedgerDtlReport";
        public const string RptCreditSanction = "RptCreditSanction";
        public const string RptRBIDataIndent = "RptRBIDataIndent";
        public const string RptWOffColl = "RptWOffColl";
        //
    }

    /// <summary>
    /// Assigne Menu ID into a Constant
    /// </summary>
    public class mnuID
    {
        private mnuID()
        { }

        public const Int32 mnuHO = 1;

        /// <summary>
        /// For Menu Accounts 11 to 20
        /// </summary>
        public const Int32 mnuAcctGroup = 11; //Accounts Group
        public const Int32 mnuAcctSubGrp = 12; //Accounts Sub Group
        public const Int32 mnuGenLed = 13; //Chart Of Accounts
        public const Int32 mnuSubLed = 14;//Subsidiary Ledger
        public const Int32 mnuAccOpBal = 15; // Opening Balance

        public const Int32 mnuRecPay = 16;//Receipt/Payment
        public const Int32 mnuJournal = 17; //Journal
        public const Int32 mnuContra = 18;//Contra

        public const Int32 mnuCollChqBounc = 19;//Collection Cheque Bounce
        public const Int32 mnuBankRecon = 20;//Bank Reconciliation

        public const Int32 mnuBankerMst = 474;//Banker Master
        public const Int32 mnuBankerLoan = 475;//Banker Loan
        public const Int32 mnuBankerLoanRecovery = 476;//Banker Loan Recovery
        public const Int32 mnuBankRepaySch = 477;//Banker Loan Repayment Schedule
        public const Int32 mnuMonthlyRepaySch = 478;//Banker Loan Repayment Schedule
        public const Int32 mnuBankerLoanStatus = 479;//Banker Loan Status
        /// <summary>
        /// Geographical
        /// </summary>
        public const Int32 mnuRegionMst = 21;//Region Master
        public const Int32 mnuAreaMst = 22;//Area Master
        public const Int32 mnuDistrictMst = 23;//District Master
        public const Int32 mnuBranchMst = 24;//Branch Master
        public const Int32 mnuBlockMst = 25;//Block Master
        public const Int32 mnuGpMst = 26;//GP Master
        public const Int32 mnuVillageMst = 27;//Village Master
        public const Int32 mnuStateMst = 28;//State Master
        /// <summary>
        /// General
        /// </summary>
        public const Int32 mnuWrkAreaMst = 31;//Mohalla Master
        public const Int32 mnuLoanParam = 32;//Loan Parameter Master
        public const Int32 mnuLoanAppParam = 33;//Loan Application Parameter
        public const Int32 mnuHoliday = 34;//Holiday Master
        public const Int32 mnuJLGGeneralParameter = 111;//JLG General Parameter
        /// <summary>
        /// Common
        /// </summary>
        public const Int32 mnuOccu = 35;//Occupation Master
        public const Int32 mnuHMRel = 36;//Human Relation Master
        public const Int32 mnuSDesig = 37;//Security Designation Master
        public const Int32 mnuQualify = 38;//Security Designation Master
        public const Int32 mnuNATCAT = 75;//NAT CAT Master
        public const Int32 mnuBusTypeMst = 876;//Business Type Master
        public const Int32 mnuBusSubTypeMst = 877;//Business Sub Type
        public const Int32 mnuBusActivity = 878;//Business Activity
        public const Int32 mnuDept = 879;//Department Master
        /// <summary>
        /// Loan Related
        /// </summary>
        public const Int32 mnuLonPurpose = 39;//Loan Purpose Master
        public const Int32 mnuLonSubPur = 40;//Loan Sub Purpose Master
        public const Int32 mnuFundSource = 41;//Fund Source Master
        public const Int32 mnuLnPool = 42;//Pool Master
        public const Int32 mnuProduct = 43;//Loan Product Master
        public const Int32 mnuScheme = 44;//Loan Scheme Master
        public const Int32 mnuLnWvOff = 45;//Loan Wave Off Master
        public const Int32 mnuInsPolicyMaster = 500;//InsurancePolicy Master
        public const Int32 mnuPAIMaster = 501;//PA Insurance Collection
        public const Int32 mnuCollPointMst = 169;//Collection Point Master
        public const Int32 mnuCollDeposit = 170;//Collection Deposit
        public const Int32 mnuMisUpload = 171;//Mis Upload
        public const Int32 mnuReconRptCollPointWise = 172;//Recon Report Collection Point Wise
        public const Int32 mnuReconRptLoWise = 173;//Recon Report Lo Point Wise
        public const Int32 mnuReconMisRpt = 174;//Reconciliation MIS Report
        public const Int32 mnuCanclReasn = 175;//Cancellation Reason Master
        public const Int32 mnuCashReconRpt = 176;//Cash Reason Report
        public const Int32 mnuCBCheckForLnEligibility = 177;//CB Check for Loan Eligibility
        public const Int32 mnuRiskPremiumChart = 178;//Risk Premium Chart
        public const Int32 mnuPricingSchemeParameter = 179;//Pricing Scheme Parameter
        public const Int32 mnuPricingSchemeParameterApp = 180;//Pricing Scheme Parameter Approval

        /// <summary>
        /// Operation
        /// </summary>
        public const Int32 mnuEmpMst = 46;//Employee Master 
        public const Int32 mnuRoTransfer = 47;//Employee Transfer
        public const Int32 mnuCenterMst = 48;//Center Master
        public const Int32 mnuGrpMst = 49;//Group Master
        public const Int32 mnuMemberMst = 50;//Member Master
        public const Int32 mnuColleRoutin = 51;//Collection Routine
        public const Int32 mnuNewMemberMst = 550;//New Member Master
        public const Int32 mnuMemberRedFlag = 551;//Member Red Flag
        public const Int32 mnuInitialApproachApprove = 552;//Initial Approach Approve

        /// <summary>
        /// Transaction
        /// </summary>
        public const Int32 mnuCGT = 52;//CGT
        public const Int32 mnuGRT = 53;//GRT
        public const Int32 mnuHVisit = 54;//House Visit
        public const Int32 mnuLoanApplication = 55;//Loan Application
        public const Int32 mnuLoanSanction = 56;//Loan Sanction
        public const Int32 mnuLoanSanchold = 74;//Sanction Hold Loan
        public const Int32 mnuLoanDisbursement = 57;//Loan Disbursement
        public const Int32 mnuLoanRecovery = 58;//Loan Recovery
        public const Int32 mnuSecuRefnd = 59;//Security Refund
        public const Int32 mnuFndAlloc = 60;//Fund Allocation
        public const Int32 mnuPoolTag = 61;//Pool Tagging
        public const Int32 mnuLoanReschedul = 62;//Loan Rescheduling
        public const Int32 mnuGrMetDay = 63;//Group Meeting Day Change
        public const Int32 mnuWritOf = 64;//Write-off
        public const Int32 mnuPrtyCashReq = 65;//Petty Cash Requisition
        public const Int32 mnuImprtEqfxData = 251;//Import Equifax Data mnuGenRdyToDisb
        public const Int32 mnuGenRdyToDisb = 252;//Generate Ready To Disburse
        public const Int32 mnuEdtSorceFnd = 253;//Edit Source Of Fund
        public const Int32 mnuIntrBrFndTr = 254;//Inter Branch Fund Transfer
        public const Int32 mnuClmFndTr = 255;//Claim/Fund Transfer
        public const Int32 mnuDmndSht = 256;//Demand Sheet
        public const Int32 mnuOverdue = 257;//Overdue Report mnuBorrowDtl
        public const Int32 mnuBorrowDtl = 258;//Overdue Report 
        public const Int32 mnuInsuranceRpt = 259;//Insurance Report 
        public const Int32 mnuDailyMisRpt = 260;//Daily MIS Report 
        public const Int32 mnuMemSrchRpt = 261;//Member Search Report
        public const Int32 mnuInsAmtPayMem = 262;//Insurance Amount Pay To Member
        public const Int32 mnuBrFundTr = 263;//HO Branch Fund Transfer Maker
        public const Int32 mnuIntBrFundTr = 264;//Branch Branch Fund Transfer(Cash)
        public const Int32 mnuNEFTDisbApprvl = 265;//NEFT disb Approval
        public const Int32 mnuNEFTTransfer = 266;//NEFT Transfer
        public const Int32 mnuEdtSubPurpose = 271;//Edit Source Of Fund
        public const Int32 mnuProvisionalDD = 400;//Edit Source Of Fund
        public const Int32 mnuInsAmtRecMem = 472;//Insurance Amount Receive From Member
        public const Int32 mnuImprtIDBIData = 473;//Import BC Data
        public const Int32 mnuImportPolicyNo = 901; //Import Policy No
        public const Int32 mnuBCCollrtp = 480;//BC Report
        public const Int32 mnuStckSummVeri = 481; // Stock Summary And Verification
        public const Int32 mnuStckDamage = 482; // Stock Dmage Entry
        public const Int32 mnuDocVerify = 483;//Document Verification
        public const Int32 mnuNEFTDisbApprovalHO = 484;//NEFT Disbursement Approval
        public const Int32 mnuCancelNEFTdisb = 485;//Cancel NEFT Disbursement 
        public const Int32 mnuDDSent = 486;//Death Document Sent
        public const Int32 mnuDDHORcv = 487;//Death Document Received By HO 
        public const Int32 mnuDDInsRcvCncl = 488;//Death Document Received/Cancel By Insurance Company
        public const Int32 mnuRetExIDBI = 489;// Return To IDBI Extra Amount
        public const Int32 mnuRetExIDBIBr = 490;// Return To IDBI Extra Amount Branch
        public const Int32 mnuICMST = 900;// Insurance Comapny Master
        public const Int32 mnuBrFundTrBank = 902; //Inter Branch Fund transfer (Bank)
        public const Int32 mnuBrNEFT = 903; // Pre Disbursement Approval
        public const Int32 mnuHoNEFT = 904; // HO Disbursement Approval     
        public const Int32 mnuMedCL = 905; // MediClaim
        public const Int32 mnuRescheduleByLoanNo = 906; // Reschedule By Loan No
        public const Int32 mnuLoanRecovaryAddj = 907; // Loan Recovary Adjuatment
        public const Int32 mnuHOLoanSanchold = 908;//HO Sanction Hold Loan
        public const Int32 mnuHONEFTTransfer = 909;//HO NEFT Transfer
        public const Int32 mnubankDetailDeathMem = 910;//Bank Detail Death Member
        public const Int32 mnuInitialApp = 911;//Initial Approach
        public const Int32 mnuHoNEFTCancel = 912; // HO Disbursement Cancel  
        public const Int32 mnuLoanUtilChk = 913; // Loan Utilization Check
        public const Int32 mnuRedFlag = 914;//Member Red Flag
        public const Int32 mnuBrFundTrans = 915;//Bulk Upload
        public const Int32 mnuDisbursement = 916;//Disbursement Report
        public const Int32 mnuPARRanking = 917;//PAR Ranking Report
        public const Int32 mnuDisbursementRanking = 918;//Disbursement Ranking Report
        public const Int32 mnuClaimStatementDischarge = 919;//Disbursement Ranking Report
        public const Int32 mnuDataEntry = 920;//Data Entry Report
        public const Int32 mnuAttendanceRptMgt = 921;//Attendance Report (Management)
        public const Int32 mnuPAR = 922;//PAR Report (Management)
        public const Int32 mnuBlukUpload = 923;//Bulk Upload
        public const Int32 mnuPreCloserApp = 924;//Pre Closer Approval
        public const Int32 mnuHONEFTTransferAPI = 925;//HO NEFT Transfer API
        public const Int32 mnuHospiMst = 926;// Hospi Cash Master
        public const Int32 mnuDigiDocMnuUpload = 927;// Digital Document Manual Upload
        public const Int32 mnuBrFundTrChecker = 934;//HO Branch Fund Transfer Checker
        public const Int32 mnuKarzaRetrigger = 935;//Karza Retrigger
        public const Int32 mnuAdvIntPaytoMem = 764;//Advance Interest Pay to Member
        public const Int32 mnuBulkJournalPosting = 766;//Bulk Journal Posting
        public const Int32 mnuUdyamUploadDownload = 936;//Udyam Aadhar Upload Download
        public const Int32 mnuSFIDUpdate = 937;//Source of Fund Bulk Update - Maker
        public const Int32 mnuIRACDataRec = 938;//IRAC Duplicate Data Rectification
        public const Int32 mnuSFIDUpdateAppr = 939;//Source of Fund Bulk Update - Checker
        public const Int32 mnuDeathFlag = 940;//Death Flaging
        public const Int32 mnuDeathDeclareBulk = 941;//Bulk Death Declare
        public const Int32 mnuDeathDoccancel = 942;//Bulk Death Declare

        /// <summary>
        /// Internal Inspection
        /// </summary>
        public const Int32 mnuIntPM = 267; // Process Management
        public const Int32 mnuIntRR = 268; // Risk Rating
        public const Int32 mnuLUC = 269; // Loan Utilization
        public const Int32 mnuPDC = 270; // Pre Disbursement
        public const Int32 mnuBrnchCmpl = 361; // Branch Compliance

        /// <summary>
        /// Audit
        /// </summary>
        public const Int32 mnuAuditPlan = 66;// Internal Inspection Plan
        public const Int32 mnuAuditSubmission = 67;//Online Audit Submission

        /// <summary>
        /// HR
        /// </summary>
        public const Int32 mnuLeaveAppln = 68;//Leave Application
        public const Int32 mnuLeaveSanc = 69;//Leave Sanction        
        public const Int32 mnuKPI = 78;//Employee KPI
        public const Int32 mnuKPIQuestMst = 79;// KPI Question Master
        public const Int32 mnuSuper1KPIApp = 77;//Supervisor1 KPI Approval
        public const Int32 mnuSuper2KPIApp = 76;//Supervisor2 KPI Approval
        /// <summary>
        /// Training Module
        /// </summary>
        public const Int32 mnuTrnMstModule = 70;//Training Module Master
        public const Int32 mnuTrnSchule = 71;//Training Schedule
        public const Int32 mnuTrnModPlan = 72;//Trainer Module Plan
        public const Int32 mnuTrnModAct = 73;//Trainer Module Actual

        /// <summary>
        /// Accounts Report
        /// </summary>
        public const Int32 nmuLedgerMstRpt = 80;//Ledger Master
        public const Int32 mnuCashBookRpt = 81;//Cash Book
        public const Int32 mnuBankBookRpt = 82;//Bank Book
        public const Int32 nmuJournalRpt = 83;//Journal Book
        public const Int32 nmuLedgDtlsRpt = 84;//General Ledger Details
        public const Int32 nmuLedgSumryRpt = 85;//General Ledger Summary
        public const Int32 mnuRecPayRpt = 86;//Receipt Payment
        public const Int32 nmuProfitLossRpt = 87;//Profit & Loss
        public const Int32 nmuTrailRpt = 88;//Trail Balance
        public const Int32 nmuBalanceSheetRpt = 89;//Balance Shee
        public const Int32 nmuBankRconsilStatRpt = 90;//Bank Reconcile Statement
        public const Int32 nmuLoanWiseLedgDtlsRpt = 163;//Loan Wise Accounts Ledger Details

        //Master Listing  87 to 94
        public const Int32 nmuTehBlocVilMohRpt = 91;//Tehsil/Block/Village/Mohalla Report
        public const Int32 nmuLoanPurpseRpt = 92;//Loan Purpose Report
        public const Int32 nmuEmpMstRpt = 93;//Employee Report
        public const Int32 nmuGrMstRpt = 94;//Group Report
        public const Int32 nmuGrMstSchedulRpt = 95;//Group Meeting Schedule Report

        //Loan Related 95 to 107
        public const Int32 nmuLoanSpficRpt = 96;//Loan Sanction Report
        public const Int32 nmuLoanDisbRpt = 97;//Loan Disbursement List
        public const Int32 nmuCollDisbRpt = 98;//Collection Disbursement Report
        public const Int32 nmuPartLedgRpt = 99;//Party Ledger
        public const Int32 nmuRepayScheRpt = 100;//Repayment Schedule
        public const Int32 nmuCollectionRpt = 101;//Collection Report
        public const Int32 nmuFinlPaidRpt = 102;//Final Paid
        public const Int32 nmuAttndRegRpt = 102;//Attendence Registers
        public const Int32 nmuLoanUtiRpt = 103;//Loan Utilization
        public const Int32 nmuLoanNDMstInfoRpt = 104;//Loan & Master Information
        public const Int32 nmuLnWriteOff = 105;//Balance Shee
        public const Int32 nmuWOffCollectionRpt = 106;//Collection Report
        public const Int32 nmuSplDisbursementRpt = 107;//Special Disbursement Report
        public const Int32 nmuMemVerifyRpt = 108;//Member Verification Report
        public const Int32 nmuLoanClosureRpt = 109;//Loan Closure
        public const Int32 nmuLoanWiseTransReg = 110;//Loan Closure
        public const Int32 nmuNDCRpt = 834;// JLG No Due Certificate
        public const Int32 mnuGSTInvoiceJLG = 767;//GST Invoice JLG
        public const Int32 mnuParAnalysis = 768;//Portfolio Analysis Report
        //MIS 121 to 130
        public const Int32 mnuLoanStatusRpt = 121;//Loan Status
        public const Int32 mnuAtGlanceRpt = 122;//At a Glance
        public const Int32 mnuPortfolioActivityRpt = 123;//Portfolio Activity
        public const Int32 mnuAgeDefltSheetRpt = 124;//Age Wise Default List
        public const Int32 mnuPortflioAginRpt = 125;//Portfolio Ageing
        public const Int32 mnuFeesCollecRpt = 126;//Fees Collection
        public const Int32 nmuDemndCollRpt = 127;//Date Wise Demand And Collection
        public const Int32 mnuBrInspec = 130;//Branch Inspection
        public const Int32 mnuInspecSum = 131;//Inspection Summary
        public const Int32 mnuAuditTeam = 132;//Audit Team Report
        public const Int32 mnuAuditDtl = 133;//Audit Details Report
        public const Int32 mnuUpcmgClsLoanRpt = 134;//Upcoming Close Loan
        public const Int32 mnurptPAI = 135; //PAI Report
        public const Int32 mnuKYCDocVeri = 136;//KYCDocumentVerificationReport

        public const Int32 mnuCenterWiseCustomerRpt = 144;//Centre wise customer detail report
        public const Int32 mnuAttendanceRpt = 715;//Attendance Report
        public const Int32 mnuInitialApproachStatusRpt = 719;//Initial Approach Status

        public const Int32 mnuIntAccrualRpt = 722;//Interest Accrual Report
        public const Int32 mnuCustDtlRpt = 723;//Customer Details Report
        public const Int32 mnuLoanApplicationRpt = 724;//Loan Application Report
        public const Int32 mnuBranchListRpt = 725;//Branch List Report
        public const Int32 mnuEmployeeListRpt = 726;//Employee List Report
        public const Int32 mnuPortfolioRpt = 727;//Portfolio Report
        public const Int32 mnuPARRpt = 728;//PAR Report
        public const Int32 mnuStateOfAcc = 729;//Statement of Account Report
        public const Int32 mnuMobWorkAlloc = 730;//Mobile work Allocation

        public const Int32 mnuFunderRpt = 731;//Funder Report
        public const Int32 mnuRunDwnRpt = 732;//DropDown Report

        public const Int32 mnuCollAttRpt = 733;//Collection Attendance Report
        public const Int32 mnuOvrrideRpt = 734;//Override Report

        public const Int32 mnuLoanUtilRpt = 735;//Loan Utilazation Report
        public const Int32 mnuOnTimeCollRpt = 736;//On Time Collection Report
        public const Int32 mnuMonthEndBorrowerDetailsRpt = 737;//On Time Collection Report
        public const Int32 mnuCBEnqRpt = 738;//CB Enquery Report
        public const Int32 mnuDayWiseDPD = 761;//Day Wise DPD
        public const Int32 mnuHOInsuMstRpt = 762;//Insurance Master Report
        public const Int32 mnuRiskCateChng = 763;//Risk Category Change
        /// <summary>
        /// For Menu System  131 to 139
        /// </summary>        
        public const Int32 mnuDayendProc = 135;//Day End Process
        public const Int32 mnuYearEnd = 136;//Year End Process
        public const Int32 mnuRole = 137;//Role
        public const Int32 mnuAssigneRole = 138;//Assign Role
        public const Int32 mnuUser = 139;//User
        public const Int32 mnuChangePass = 140;//Change Password
        public const Int32 mnuTransfr = 141;//Transfer Within Branch
        public const Int32 mnuMarquee = 142;//Marquee Master
        public const Int32 mnuMac = 143;//System Information
        public const Int32 mnuDayBeginProc = 720;//Day Begin Process
        public const Int32 mnuIfsc = 721;//IFSC Master
        public const Int32 mnuInterBrTransfr = 765;//Transfer Within Branch
        /// <summary>
        /// For Utility 140 to 148
        /// </summary>
        public const Int32 mnuFullBackup = 145;//Full Backup
        public const Int32 mnuBrnchBackup = 146;//Branch Backup
        public const Int32 mnuBrnchBackupRestr = 147;//Branch Restore
        public const Int32 mnuLoanCalc = 148;//Loan Calculator
        public const Int32 mnuAltrTab = 149;//Alter Table
        public const Int32 mnuAltrSp = 150;//Alter Procedure
        public const Int32 mnuAppUpdate = 151;//Application Update
        public const Int32 mnuLoanCal = 152;//Loan Calculator


        /// <summary>
        /// For Audit 149 to 155
        /// </summary>
        public const Int32 mnuAuditPln = 155;//Audit Plan
        public const Int32 mnuHOAudit = 156;//HO Audit Part
        public const Int32 mnuBrnchAudit = 157;//Branch Audit Part
        public const Int32 mnuLoanUti = 158;//Loan Utilization
        public const Int32 mnuLoanUtiBM = 159;//Loan Utilization BM
        public const Int32 mnuLoanUtiAM = 160;//Loan Utilization AM
        public const Int32 mnuLoanUtiAudt = 161;//Loan Utilization Audit
        public const Int32 mnuBrRdyToDisb = 162;//Ready To Disbursement

        /// <summary>
        /// HO Reports
        /// </summary>
        public const Int32 mnuHOPrinOut = 301;//Principal Outstanding
        public const Int32 mnuHOActvClnt = 302;//mnuHOActvClnt
        public const Int32 mnuHOParClnt = 303;//PAR Client
        public const Int32 mnuHODfltAmt = 304;//Default Amount
        public const Int32 mnuHOParOut = 305;//PAR Outstanding
        public const Int32 mnuHONoLnDisb = 306;//No of Loan Disburse

        public const Int32 mnuHOHghMrkDataSub = 307;//HighMark Data Submission
        public const Int32 mnuHOAppNotDisb = 308;//Approved Not Disbursed
        public const Int32 mnuHOLnDisb = 309;//Loan Disbursement
        public const Int32 mnuHOFeesDtl = 310;//Fees Details
        public const Int32 mnuHOTurnArndTm = 311;//Turn Around Time
        public const Int32 mnuHOLnSts = 312;//Loan Status
        public const Int32 mnuHOColRpt = 313;//Collection Report
        public const Int32 mnuHOPrtflioAgng = 314;//Portfolio Ageing
        public const Int32 mnuHOPrtflioActvty = 315;//Portfolio Activity
        public const Int32 mnuHODtWsDmndCol = 316;//Data Wise Demand And Collection
        public const Int32 mnuHOOvrDueRpt = 317;//Overdue Report
        public const Int32 mnuHOBorrDtls = 318;//Borrower Details
        public const Int32 mnuHOComAnls = 319;//Comparative Analysis
        public const Int32 mnuHOEqfxDtSnd = 320;//Equifax Data Send
        public const Int32 mnuHORdyToDisb = 321;//Ready To Disbursement
        public const Int32 mnuHODmndColSts = 322;//Demand And Collection Status
        public const Int32 mnuHODlyMisRpt = 323;//Daily MIS Report
        public const Int32 mnuHOBrWsColDtls = 324;//Branch Wise Collection Details
        public const Int32 mnuHOBrWsDisbDtls = 325;//Branch Wise Disbursement Details
        public const Int32 mnuHOBrWsFeesSrvcTaxDtls = 326;//Branch Wise Fees And Service Tax Details
        public const Int32 mnuHOBrWsInsuDtls = 327;//Branch Wise Insurance Details
        public const Int32 mnuHOBrWsExpRpt = 328;//Branch Wise Expense Report
        public const Int32 mnuHOInsuDtlsRpt = 329;//Insurance Details Report


        public const Int32 mnuHOCashBnkClsng = 330;//Cash Bank Closing
        public const Int32 mnuHOFndTrRecon = 331;//Fund Transfer Reconcile
        public const Int32 mnuHORecptPay = 332;//Receipt Payment
        public const Int32 mnuHOPrfLs = 333;//Profit & Loss
        public const Int32 mnuHOTrialBal = 334;//Trial Balance
        public const Int32 mnuHOBalSheet = 335;//Balance Sheet
        public const Int32 mnuHOSpclAcctRpt = 336;//Special Account Report  
        public const Int32 mnuHOPool = 337;//Pool Report
        public const Int32 mnuHOFundGiven = 338;//Insurance Report
        public const Int32 mnuHOAccInt = 339;//Accrued Interest
        public const Int32 mnuHOClaimTrnIns = 340;//Claim/FT/Insurance Claim Voucher
        public const Int32 mnuHODlyMisAccRpt = 341;//Daily MIS Report
        public const Int32 mnuHOPARCategoryRpt = 342;//Daily MIS Report
        public const Int32 mnuRqFundToBr = 343;//Required Fund To be Sent to the Branch
        public const Int32 mnuHOLoanDisbProspone = 344;//Required Fund To be Sent to the Branch
        public const Int32 mnurptPDD = 401;//Required Fund To be Sent to the Branch
        public const Int32 mnurptPDDHO = 402;//Required Fund To be Sent to the Branch
        public const Int32 mnurptPPIScoreHO = 403;//CGT Score
        public const Int32 mnuHOrptMailMerge = 404;//Mail Merge Report
        public const Int32 mnuHOBCDataUploadStatus = 405;//BC Data Upload Status
        public const Int32 mnuRptHoBrWiseLnDisb = 406;// BranchWise Loan Disbursement
        public const Int32 mnuBrWiseLnDtl = 407;// Branchwise Loan Detail, JLG, Outstanding Report
        public const Int32 mnuMnWiseLnDtl = 408;//Month wise demand collection
        public const Int32 mnuRptPortPerf = 409;//Portfolio Preformance Report
        public const Int32 MemDiffStage = 410;//Member Report in different Stage
        public const Int32 mnuHORptDayEnd = 411;//DayEnd Report
        public const Int32 mnuRptCancel = 412;//Cancel Or Postpone Report
        public const Int32 mnuRptSancCanAppList = 413;//Sanction Cancel Application Report
        public const Int32 mnuHORptMudra = 414; // Mudra Report
        public const Int32 mnuHOCollRecon = 415;//HO Collection Reconciliation
        public const Int32 mnuHONeftStat = 416; // HO NEFT Status Report
        public const Int32 mnuHOPrintNEFT = 417; // NEFT Print
        public const Int32 mnuHOLnDisbOTP = 419;//Loan Disbursement OTP
        public const Int32 mnuExcessLedger = 153;//Excess Ledger Report

        public const Int32 mnuHOKPIDtl = 701; // KPI details Report
        public const Int32 mnuHORptUser = 702; // User Details Report
        public const Int32 mnuHORptDeathDoc = 705; // Death Document Status Report
        public const Int32 mnuHORptCreditFlow = 706; // Credit Flow Report For IDBI
        public const Int32 mnuHOEqFaxDataSub = 707;//EquiFax Data Submission
        public const Int32 mnuHOCIBILDataSub = 708;//CIBIL FORMAT Data Submission
        public const Int32 mnuHOExperianDataSub = 709;//Experian UNNATI TRADE Data Submission
        public const Int32 mnuInsuranceClaimUpload = 710;//Insurance Claim Upload
        public const Int32 mnuMemRegd = 711;//Membership Register
        public const Int32 mnuNEFTCancel = 712;//NEFT Cancel
        public const Int32 mnuPerformance = 713;//Performance Report
        public const Int32 mnuRptInsuAtaGlance = 714;//Insurance At a Glance Report
        public const Int32 mnuRptInsuAgeing = 716;//Insurance Ageing Report
        public const Int32 mnuHOEquiFaxStat = 718;//HO EquiFax Enquiry Report
        public const Int32 mnuHOMonthlyAttn = 742; // Monthly attendance report
        public const Int32 mnuRptAreaMapping = 743; // 	Area Mapping Report

        public const Int32 mnuRptTransferRegister = 739;//Transfer Register Report
        public const Int32 mnuInsurancePremRpt = 740;//Insurance Premium Report
        public const Int32 mnuAwaazRpt = 741;//Awaaz De Report
        public const Int32 mnuBCCollInput = 744; // 	BC Collection Input
        public const Int32 mnuBCLoanDisb = 745; // 	BC Collection Input
        public const Int32 mnuBCNewMember = 746; // 	BC New Member
        public const Int32 mnuBCSanctionMIS = 747; // 	BC Sanction MIS
        public const Int32 mnuInsuranceClaimRpt = 748; // 	Insurance Claim Report
        public const Int32 mnuHOEqFaxDataSubBC = 749;//EquiFax Data Submission BC
        public const Int32 mnuCollEffiRpt = 750;//Collection Efficiency Report
        public const Int32 mnuHospiCashClaimRpt = 751; // 	Hospi Cash Claim Report
        public const Int32 mnuReloanCBChkRpt = 752; // 	Reloan CB Check Report
        public const Int32 mnuMaturityAnl = 753; // Maturity Analysis Report
        public const Int32 mnuExgratiaData = 754; // Exgratia Data
        public const Int32 mnuRptLoanParameter = 755; // Loan Parameter Report
        public const Int32 mnuPettyCashRpt = 756; // 	Petty Cash Report
        public const Int32 mnuPettyCashBalanceRpt = 757; // 	Petty Cash Balance
        public const Int32 mnuOCRLog = 758; // 	OCR Log Report
        public const Int32 mnuRBIMaturityAnl = 759; //RBI Maturity Analysis Report
        public const Int32 mnuPettyCashCertRpt = 760; // 	Petty Cash Certificate
        /// <summary>
        /// Internal Inspection Report
        /// </summary>
        public const Int32 mnuLucAnlRpt = 345; // LUC Analysis Report
        public const Int32 mnuPdcAnlRpt = 346; // PDC Analysis Report
        public const Int32 mnuRiskRatRpt = 347; // Risk Rating Report
        public const Int32 mnuRiskRatBrRpt = 348; // Risk Rating Branchwise Report
        public const Int32 mnuRiskRatCnld = 349; // Risk Rating Consolidated Report
        public const Int32 mnuProcMgmtRpt = 350; //Process Management Report
        public const Int32 mnuBrGrdPmRpt = 351; // Branch Grading Report (PM)

        public const Int32 mnuVendMst = 352; //Vendor Master
        public const Int32 mnuItemMst = 353; //Item Master
        public const Int32 mnuOpenItem = 354; //Opening Item
        public const Int32 mnuRecHO = 355; // Item Received By HO
        public const Int32 mnuInspSch = 356;// Inspection Schedule
        public const Int32 mnuLucBrnch = 357; //LUC Branch Wise
        public const Int32 mnuPdcBrnch = 358; //PDC Branch Wise
        public const Int32 mnuInspGlance = 359; // Inspection At A Glance
        public const Int32 mnuLucGlance = 360; // LUC At A Glance
        public const Int32 mnuPdcGlance = 362; // PDC At A Glance
        public const Int32 mnuBrchCmplRpt = 363; // Branch Compliance Report
        public const Int32 mnuIrreRectRpt = 364; // Irregularities Vs Rectification

        public const Int32 mnuHoToArea = 365; // Head Office to Area
        public const Int32 mnuHoToBrch = 366; // Head Office to Branch
        public const Int32 mnuAreaToBrch = 367; // Area To Branch
        public const Int32 mnuBranchToBrch = 368; // Branch To Branch
        public const Int32 mnuBranchToStaff = 374; // HO/Branch To Staff
        public const Int32 mnuInOutLtr = 385; // Inward/Outward Letter

        public const Int32 mnuRVHoToArea = 369; // Head Office To Area(Receive)
        public const Int32 mnuRVHoToBrch = 370; // Head Office To Branch(Receive)
        public const Int32 mnuRVAreaToBrch = 371; // Area To Branch(Receive)
        public const Int32 mnuRVBrchToBrch = 372; // Branch To Branch(Receive)
        public const Int32 mnuHOCenterWisePOS = 373;//Center Wise POS

        public const Int32 mnuBrPMRpt = 378; //Process Management Report
        public const Int32 mnuBrRRRpt = 379; //Risk Rateing Report
        public const Int32 mnuBrLuc1Rpt = 380; //LUC Analysis Report
        public const Int32 mnuBrLuc2Rpt = 381; //LUC Analysis Branch Report
        public const Int32 mnuBrPdc1Rpt = 382; //PDC Analysis Report
        public const Int32 mnuBrPdc2Rpt = 383; //PDC Analysis Branch Report
        public const Int32 mnuBrComplRpt = 384; //Branch Compliance Report
        public const Int32 mnuRptFdDtls = 701; // FD Details Report
        public const Int32 mnuRptLnRepaySch = 702; // Loan Repayment Schedule
        public const Int32 mnuRptBankLnDtls = 703; // Banker Loan Details
        public const Int32 mnuRptLnRepayFutureSch = 704; // Loan Repayment Future Schedule
        /// <summary>
        /// Stock Report
        /// </summary>  
        public const Int32 mnuArBrWiseStockRpt = 375; // Branch/Area Wise Stock
        public const Int32 mnuItemWiseStockRpt = 376; // Item Wise Stock Summary
        public const Int32 mnuItemWiseStockDtlRpt = 377; // Item Wise Stock Details
        public const Int32 mnuStckVeriRpt = 391; // Stock Verification Report

        public const Int32 mnuHOSetlClnt = 399;//HO Final Paid
        /// <summary>
        /// NPS 
        /// </summary>  
        public const Int32 mnuNpsAgntMst = 201;//Agency Master
        public const Int32 mnuNpsMemMst = 202;//Member Master
        public const Int32 mnuNpsParamMst = 203;//Parameter Master
        public const Int32 mnuNpsSndToAlainkit = 204;//Send To Alainkit
        public const Int32 mnuNpsImprtPrnData = 205;//Import PRAN Data
        public const Int32 mnuNpsCol = 206;//NPS Collection
        public const Int32 mnuNpsUpldCont = 207;//Upload NPS Contribution  
        public const Int32 mnuNpsMemReport = 208; //NPS Member Report
        public const Int32 mnuNpsBankStatement = 209;//NPS Bank Statement Report
        public const Int32 mnuNpsLiteCollection = 210;//NPS Lite Collection
        public const Int32 mnuNpsForm32 = 211; //NPS Form 32 for Alainkit
        public const Int32 mnuNpsCollRemitt = 212;//NPS Collection Remittance
        public const Int32 mnuNpsRmittStatus = 213;//NPS Remittance Status
        public const Int32 mnuNpsSendPFRDA = 214;//NPS Collection Remittance
        public const Int32 mnuNpsAging = 215;//NPS Aging

        public const Int32 mnuUsrMobApp = 216;//Mobile User
        public const Int32 mnuDeviceMst = 217;//Device Master

        // BC Operation
        public const Int32 mnuSFTPUPLOAD = 9001;//Device Master
        public const Int32 mnuSFTPJLGREUPLOAD = 9002;//JLG File
        public const Int32 mnuSFTPJCONFUPLOAD = 9003;//JCONF File
        public const Int32 mnuReCheckCGTData = 9004;//Customer Reject Recheck File Upload
        public const Int32 mnuSFTPFILEDOWNLOAD = 9005;//SFTP send File Download
        public const Int32 mnuBCAccountTracking = 9006;//OverAll Status
        public const Int32 mnuBusinessActivity = 9007;//BC Business Activity
        public const Int32 mnuPortfolioOutstanding = 9008;//BC Portfolio Outstanding
        public const Int32 mnuNewBusinessSummary = 9009;//New Business Summary
        public const Int32 mnuRepaymentSummary = 9010;//Repayment Summary
        public const Int32 mnuRepaymentTracking = 9011;//Repayments Tracking
        public const Int32 mnuRepayFileTracking = 9012;//Repayments File Tracking
        public const Int32 mnuSavingApplBCRpt = 9013;//Saving Application For BC
        public const Int32 mnuBulkInsBCRpt = 9014;//Bulk Upload For BC
        public const Int32 mnuLoanAppBCRpt = 9015;//Loan Application For BC
        public const Int32 mnuDayEndRpt = 9016;//Day End Report For HO
        public const Int32 mnuDeletedData = 9018;//Deleted Data Report
        public const Int32 mnuReLoanCbCheck = 9019;//Re Loan CB Check
        public const Int32 mnuPettyCash = 9020;//Petty Cash
        public const Int32 mnuPettyReplenish = 9021;//Petty Cash Replenish//Petty Cash Approve
        public const Int32 mnuCashRecon = 9022;//Cash Reconciliation
        public const Int32 mnuHoDownloadCertificate = 9023;//HO Download All Certificate
        public const Int32 mnuDocSendRecivedInsuco = 9024;//Document Send and Received From Insurance Company
        public const Int32 mnuDeathDeclare = 9025;//Death Declaration
        public const Int32 mnuHospiCashClaim = 9026;//Hospi Cash Claim
        public const Int32 mnuCashReconCheck = 9027;//Cash Reconciliation Approval
        //public const Int32 mnuAttendanceViewRpt = 9017;//EquiFax Data Submission
        public const Int32 mnuPettyCashMst = 9028;//Petty Cash Master
        public const Int32 mnuEliAmtChange = 9029;//Eligible Amount Change
        public const Int32 mnuActivityMst = 9030;//Activity Master
        public const Int32 mnuActivityApproval = 9031;//Activity Approval
        public const Int32 mnuActivityTrackerRpt = 9032;//Activity Tracker Approval
        public const Int32 mnuPreApproveLoan = 9033;//Pre Approved Loan
        public const Int32 mnuFinacleAccData = 9034;//Finacle Accounts Data
        public const Int32 mnuLoanRecoveryBulk = 9035;//Loan Recovery Bulk
        public const Int32 mnuDeviationMatrix = 9036;//Deviation Matrix
        public const Int32 mnuDeviationMatrixApproval = 9037;//Deviation Matrix Approval
        public const Int32 mnuDeviationStatusRpt = 9038;//Deviation Status Report
        public const Int32 mnuOpenBucket = 9039;//Open Bucket
        public const Int32 mnuReportDownload = 9040;//Report Download
        public const Int32 mnuPenOpBucketRpt = 9041;//Open Bucket Report
        public const Int32 mnuDigiConsent = 9042;//Digital Consent Form
        public const Int32 mnuJocataApprove = 9043;//Jocata Approve
        public const Int32 mnuHOtoBRFundTRRpt = 9046;//HO To Branch Fund Transfer
        public const Int32 mnuFetchAadhaarDetails = 9047;//Fetch Aadhaar Details 
        public const Int32 mnuHouseVisitRpt = 9049;//House Visit Report 
        public const Int32 mnuBcDisbursementRpt = 9051;//BC Disbursement File
        public const Int32 mnuDailyAccuredInterestRpt = 9052;//Daily Accured Interest Report 
        public const Int32 mnuMonitoring = 9053;//Monitoring Module
        public const Int32 mnuMonitoringCompl = 9054;//Monitoring Compliance
        public const Int32 mnuMonitoringRpt = 9055;//Monitoring Report 
        public const Int32 mnuMonitoringComplRpt = 9056;//Monitoring Compliance Report
        public const Int32 mnuProsidexStatusRpt = 9057;//Prosidex Status
        public const Int32 mnuMonitoringComplianceRpt = 9058;//Monitoring Compliance Report
        public const Int32 mnuIncentiveRpt = 9059;//Incentive Report
        public const Int32 mnuTaxInvoice = 9060;//Tax Invoice
        public const Int32 mnuAvdIntRpt = 9061;//Interest On Advance Report
        public const Int32 mnuPincodeMst = 9062;//Pincode Master
        public const Int32 mnuAgeasDataRpt = 9063;//Ageas Data Submission Report
        public const Int32 mnuAiquHospiDataRpt = 9064;//Aiqu Hospi Data Submission Report
        public const Int32 mnuHolidaytlistRpt = 9065;//Holi Day List
        public const Int32 mnuAdvCollNtPosted = 9066;//Advance Collection Not Posted
        public const Int32 mnuInsuranceNeftApi = 9067; //Insurance Neft Api
        public const Int32 mnuRptGST = 9070;// Gst Report
        public const Int32 mnuRptHouseVisitNew = 9072;// House  Visit  Report
        public const Int32 mnuRptPSL = 9074;// PSL  Report
        public const Int32 mnuRptIBPSL = 9077;// IBPSL  Report
        public const Int32 mnurptHospiCashClaimNew = 9078;// HospiCash Claim Report New
        public const Int32 mnuHospiCashHoApproval = 9079;// HospiCash HO Approval
        public const Int32 mnuMonitoringOth = 9080;// Other Monitoring Module
        public const Int32 mnuMonitoringComplOth = 9081;//Other Compliance
        public const Int32 mnuMonitoringOD = 9082;// Overdue Monitoring
        public const Int32 mnuRptDiscrepancy = 9085;// Discrepancy Report
        public const Int32 mnuPTP = 9093;// Promise To Pay Mobile
        public const Int32 mnuTeleCalling = 9094;// Tele Calling Mobile
        public const Int32 mnuRptTeleCalling = 9096;// Tele Calling Mobile
        public const Int32 mnuVillageMasterRpt = 9100; // 	Village Master Report
        public const Int32 mnuRoleMatrix = 9101; // Role Matrix Report
        public const Int32 mnuGuaranteeSchemeReport = 9102; // Guarantee Scheme Report
        public const Int32 mnuAdvanceRescheduleReport = 9103; // Advance Reschedule Report 
        public const Int32 mnuRecoveryPoolUpload = 9104; // Recovery Pool Upload
        public const Int32 mnuAccountLedgerDtlReport = 9105; // Account Ledger Detail Report
        public const Int32 mnuAdvanceAdjustmentReport = 9106; // Advance Adjustment Report 
        public const Int32 mnuUserWiseBranchReport = 9107; // User Wise Branch Report 
        public const Int32 mnuOtsMasterReport = 9108; // Ots Master Report
        public const Int32 mnuInsuAPIRpt = 9109;//Holi Day List
        public const Int32 mnuHOCenterWiseCustomerRpt = 9110;//HO Center Wise Customer Detail Report
        public const Int32 mnuRptCreditSanction = 9111;//Credit Sanction Report
        public const Int32 mnuPdByBmMob = 9112;//PD By BM Mobile
        public const Int32 mnuLucSaral = 9113;//LUC Saral
        public const Int32 mnuMultipleUcic = 9116;//Multiple UCIC
        public const Int32 mnuMultiUpdateUcic = 9118;//Multiple Update UCIC
        public const Int32 mnuCollTimeCng = 9119;//Collection Time Change
        public const Int32 mnuRptRBIDataIndent = 9120; // Rbi Data Indent
        public const Int32 mnuBranchControlMst = 9125;//Branch Control Master
        public const Int32 mnuInitialApproachLocationInfoRpt = 9126;//Initial Approach Location Info
    }

    /// <summary>
    /// 
    /// </summary>
    public class MsgAccess
    {
        private MsgAccess()
        { }
        public const string Add = "You do not have rights for adding record.";
        public const string Edit = "You do not have rights for updating record.";
        public const string Del = "You do not have rights for deleting record.";
        public const string Proc = "You do not have rights for processing record.";
        public const string Rpt = "You do not have rights for accessing report.";
        public const string view = "You do not have rights for viewing record.";
    }

    /// <summary>
    /// 
    /// </summary>
    public class gblMarg
    {
        private gblMarg()
        { }

        /// <summary>
        ///
        /// </summary>
        public static string SaveMsg
        {
            get
            {
                return "Record Saved Successfully.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string EditMsg
        {
            get
            {
                return "Record Updated Successfully.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ConfirmDelete
        {
            get
            {
                return "Are You Sure to Delete Record?";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DeleteMsg
        {
            get
            {
                return "Record Deleted Successfully.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DuplicateMsg
        {
            get
            {
                return "This Is Duplicate Record. Please Rectify.";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string NotificationMsg
        {
            get
            {
                return "Please Select a Record.";
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public static string NotBranchUser
        {
            get
            {
                return "This Is Not A Valied User For This Branch";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ValidEmail
        {
            get
            {
                return "This Is Not A Valied Email.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string InvalidUser
        {
            get
            {
                return "Invalid UserName/Password. Please Try Again.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string InActiveUser
        {
            get
            {
                return "You are an Inactive User. Please contact System Administrator.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ValidDate
        {
            get
            {
                return "This Is Not a Valid Date. Please Rectify";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ExitMsg
        {
            get
            {
                return "You Can Not Exit, Process Is On Working...";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string RecordUseMsg
        {
            get
            {
                return "Record Is In Used. You can not delete the Record.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DBError
        {
            get
            {
                return "Data Not Saved. Data Error.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string DayEndMsg
        {
            get
            {
                return "All Trasactions are Closed for The Date.";
            }
        }
    }
}