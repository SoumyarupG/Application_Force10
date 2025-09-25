using System;

namespace CENTRUMCA
{

    /// <summary>
    /// Application Global Value
    /// </summary>
    public class gblValue
    {
        private gblValue()
        { }
        public const string HOBrnchCode = "HOBrCode";
        public const string FinYear = "FinYear";
        public const string FinYrNo = "FinYrNo";
        public const string FinFromDt = "FromDt";
        public const string FinToDt = "ToDt";
        public const string LoginDate = "LogDate";
        public const string BrName = "BrName";
        public const string BrnchCode = "BrCode";
        public const string UserName = "UserNm";
        public const string DesignationID = "DesignationID";
        public const string LoginId = "LoginId";
        public const string ACVouMst = "ACVouMst";
        public const string ACVouDtl = "ACVouDtl";
        public const string ShortYear = "ShortYear";
        public const string DistrictId = "DistrictId";
        public const string RoleId = "RoleId";
        public const string UserId = "UserId";
        public const string EndDate = "EndDate";
        public const string BrType = "BrType";
        public const string RegId = "RegId";
        public const string CBDB = "CB";
        //For Report Purpose Constant Value
        public const string CompName = "Unity Small Finance Bank - Climate Finance";
        public const string Address1 = "Address 11";
        public const string Address2 = "Address 11";
        public const string CIN = "";

        //For Constant Value of the Application
        public const string Country = "India";
        public const Int32 PageHeight = 700;

        //Loan Parameter Account
        public const string PrincipalLoanAc = "LoanAc";
        public const string LoanIntAc = "InstAc";
        public const string IntWaveAC = "IntWaveAC";
        public const string ProcfeesAC = "ProcAC";
        public const string ServiceAC = "ServiceTaxAC";
        public const string InsuAC = "InsureAC";
        public const string PenalAc = "PenalAC";
        public const string WriteoffAC = "WriteoffAC";
        public const string WriteOffRecAC = "WriteOffRecAC";
        public const string InsServTaxAC = "InsServTaxAC";
        public const string ApplicationCargeAC = "ApplicationCargeAC";
        public const string StampChargeAC = "StampChargeAC";
        public const string LPFKKTaxAC = "LPFKKTaxAC";
        public const string LPFSBTaxAC = "LPFSBTaxAC";
        public const string CGSTAC = "CGSTAC";
        public const string SGSTAC = "SGSTAC";
        public const string FLDGAC = "FLDGAC";
        public const string ReStructureAC = "ReStructureAC";
        public const string BankChargeAC = "BankChargeAC";
        public const string BounceChrgAC = "BounceChrgAC";
        public const Int32 PgSize1 = 20;
        public const string ViewAAdhar = "ViewAAdhar";
        public const string LoanIntAccruedAc = "IntAccruedAc";
        public const string LeadID = "LeadID";
        public const string MemberID = "MemberID";
        public const string BCPNO = "BCPNO";
        public const string ApplNm = "ApplNm";
        public const string EmpStatus = "EmpStatus";
        public const string AssMtdId = "AssMtdId";
        public const string IncomeStatus = "IncomeStatus";
        public const string AssMtdTypId = "AssMtdTypId";
        public const string InsuStatus = "InsuStatus";
        public const string MobNo = "MobNo";
        public const string ICICINEFTYN = "ICICINEFTYN";
        public const string ICICIUser = "ICICIUser";
        public const string NeftAPI = "NeftAPI";
    }

    /// <summary>
    /// Assign Menu ID into a Constant
    /// </summary>
    public class mnuID
    {
        private mnuID()
        { }

        public const Int32 mnuHO = 1;

        /// <summary>
        /// For Menu Accounts 101 to 200
        /// Master
        /// </summary>
        public const Int32 mnuAcctGroup = 101; //Account Group
        public const Int32 mnuAcctSubGrp = 102; //Account Sub-Group
        public const Int32 mnuGenLed = 103; //Chart Of Account
        public const Int32 mnuSubLed = 104;//Subsidiary Ledger
        public const Int32 mnuAccOpBal = 105; // Opening Balance

        /// Voucher Entry
        public const Int32 mnuRecPay = 106;//Receipt and Payment
        public const Int32 mnuJournal = 107; //Journal
        public const Int32 mnuContra = 108;//Contra

        /// Others
        public const Int32 mnuBankRecon = 109;//Bank Reconciliation


        /// <summary>
        /// For Menu Master 201 to 300
        /// Geographical
        public const Int32 mnuStateMst = 201;//State Master
        public const Int32 mnuRegionMst = 202;//Region Master
        public const Int32 mnuDistrictMst = 203;//District Master
        public const Int32 mnuDivisionMst = 204;//Division Master
        public const Int32 mnuAreaMst = 205;//Area Master
        public const Int32 mnuBranchMst = 206;//Branch Master
        public const Int32 mnuTehsilMst = 207;//Tehsil/Taluka Master
        public const Int32 mnuBlockMst = 208;//Block Master
        public const Int32 mnuVillageMst = 209;//Village/Ward Master
        public const Int32 mnuMohallaMst = 210;//Mohalla Master
        public const Int32 mnuClusterMst = 2009;//Village/Ward Master


        /// General
        public const Int32 mnuDesignationMst = 226;//Designation Master
        public const Int32 mnuLonPurpose = 211;//Loan Purpose Master
        //public const Int32 mnuLonSubPur = 212;//Loan Sub Purpose Master
        public const Int32 mnuFundSource = 213;// Source of Fund Master
        public const Int32 mnuInsCompMst = 214;//Insurance Company Master
        public const Int32 mnuInsSchemeMst = 215;//Insurance Scheme Master
        public const Int32 mnuSecrvTx = 5004;//Service Tax Master
        public const Int32 mnuCredtBure = 217;//Credit Bureau Master
        public const Int32 mnuCanclReasn = 218;//Cancel Reason Master
        public const Int32 mnuOccu = 219;//Occupation Master
        public const Int32 mnuProduct = 220;//Loan Product Master
        public const Int32 mnuScheme = 221;//Loan Scheme Master
        public const Int32 mnuLoanParam = 5003;//Loan Parameter Master
        public const Int32 mnuMarquee = 223;//Marquee Master
        public const Int32 mnuLoanApplicationSource = 5002;//Source of Loan Application
        public const Int32 mnuLevalRangeMst = 225;//Level Range Master

        public const Int32 mnuDocUpload = 5008;//Document Upload
        public const Int32 mnuDigiConsentSME = 5018;//Digital Consent Form
        public const Int32 mnuOtherCollection = 5015;//Other Collection
        public const Int32 mnuLoanUtilChk = 5010;//Loan Utilization Check
        public const Int32 mnuPDC = 5011;//PDC
        public const Int32 mnuPreMatchColl = 5012;//Premature Collection
        public const Int32 mnuDeathDeclare = 5013;//Death Declare
        public const Int32 mnuCollDelRev = 5034;//Collection Delete Reverse
        /// <summary>
        /// For Menu Operation 301 to 400
        /// </summary>
        ///
        public const Int32 mnuGeneralParameter = 5001;//General Parameter
        public const Int32 mnuLeadGeneration = 200;//Lead Generation
        public const Int32 mnuEmpMst = 301;//Employee Master 
        public const Int32 mnuRoTransfer = 302;//Employee Transfer
        public const Int32 mnuMohallaSurvy = 303;//Mahalla Survey
        public const Int32 mnuCenterMst = 304;//Cluster Formation
        public const Int32 mnuColleRoutin = 305;//Collection Routine
        public const Int32 mnuHgMrkDtCllec = 306; // HighMark Data Collection
        public const Int32 mnuBulkHighDEntry = 307; // Bulk HighMark Data Collection
        public const Int32 mnuCBChecking = 308; // Transfer To Credit Bureau Checking
        public const Int32 mnuHighMAppr = 309; // HighMark Data Approve
        public const Int32 mnuHighMrkDAppr = 310; // Bulk HighMark Data Approve
        public const Int32 mnuCGT = 311;//CGT
        public const Int32 mnuGrpMst = 312;//Group Formation
        public const Int32 mnuMemberMst = 313;//Member Allocation
        public const Int32 mnuLnAppBucket = 5009;//Loan Application Bucket
        public const Int32 mnuPreCAMBucket = 316;//Pre CAM Bucket
        public const Int32 mnuCAMBucket = 317;//CAM Bucket
        public const Int32 mnuCustomermst = 314;//Master Bucket
        /// <summary>
        /// For Menu Transaction 401 to 500
        /// Transaction
        /// </summary>
        public const Int32 mnuAccrued = 399;//Accrued Posting
        public const Int32 mnuAccruedLend = 400;//Lender Accrued Posting
        public const Int32 mnuLoanApplication = 401;//Loan Application
        public const Int32 mnuLoanApplicationBlk = 402;//Loan Application Bulk
        public const Int32 mnuLoanApprisal = 403;//Loan Appraisal
        public const Int32 mnuLoanSanction = 404;//Loan Sanction
        public const Int32 mnuLoanDisbursement = 405;//Loan Disbursement
        public const Int32 mnuTransDisb = 999;//Loan Tranche Disbursement
        public const Int32 mnuLoanDisbursementBlk = 406;//Loan Disbursement Bulk
        public const Int32 mnuLoanRecoveryInd = 601;//Loan Recovery
        public const Int32 mnuChequeBounce = 421;//Cheque  Bounce
        public const Int32 mnuLoanUtilisation = 408;//Loan Utilization
        public const Int32 mnuEdtSorceFnd = 409;//Edit Source of Fund
        public const Int32 mnuLoanReschedul = 410; //Individual Loan Rescheduling
        public const Int32 mnuGrMetDay = 411;// Meeting Day/Date Change
        public const Int32 mnuWritOf = 412;//Write-off
        public const Int32 mnuWriteoffCol = 413;//Write-off Collection
        public const Int32 mnuAccruedIntPost = 414;//Accrued Interest Posting
        public const Int32 mnuFinalLoanSanction = 5010;//Loan Final Sanction
        public const Int32 mnuWriteOffDecalre = 426;//Write-Off Declaration
        public const Int32 mnuWriteOffRecovery = 427;//Write-Off Recovery
        public const Int32 mnuFinalSancNotDisb = 428;//Loan Final Sanction But Not Disburse //
        public const Int32 mnuLoanRecoveryBulk = 1021;//Loan Recovery
        public const Int32 mnuLoanRecoveryDeleteBulk = 1022;//Loan Recovery
        public const Int32 mnuCIBILDataSubmissionRpt = 429;//CIBIL Data Submission Report
        public const Int32 mnuLeadInformation = 430;//Lead Information
        public const Int32 mnuCollectionUpload = 4007;//Loan Recovery
        public const Int32 mnuOverride = 4008;//Override
        public const Int32 mnuProvisionalDD = 431;//Provisional Death Declare
        public const Int32 mnuOtherCollectionBulk = 4020;//Other Collection Bulk

      //  public const Int32 mnuSvDep = 415; //Optional Savings Deposit/Withdrawal
        public const Int32 mnuBlkSvDep = 416; //Savings Deposit Bulk Collection
        public const Int32 mnuIntWith = 417; // Interest Withheld
        public const Int32 mnuAgreement = 418; // Agreement
        public const Int32 mnuOffVerification = 419; // Office Verification
        public const Int32 mnuResVerification = 420; // Residence Verification
        public const Int32 mnuBulkLenderEntry = 422; // Bulk Lender Entry
        public const Int32 mnuLnRescheduling = 423; // Loan Rescheduling
        public const Int32 mnuLnReStructure = 424; // Loan ReStructure
        public const Int32 mnuDashboard = 425; // Dashboard
        public const Int32 mnuDocsDownload = 432; // Document Verification and Download
        public const Int32 mnuRiskCatChngMEL = 4022;//Risk Category Change(MEL)
        /// <summary>
        /// For Menu Reports 501 to 600
        /// Reports
        /// </summary>
        public const Int32 nmuLedgerBook = 499;//Ledger Master
        public const Int32 nmuDayBookRpt = 500;//Trail Balance
        public const Int32 nmuLedgerMstRpt = 501;//Ledger Master
        public const Int32 mnuCashBookRpt = 502;//Cash Book
        public const Int32 mnuBankBookRpt = 503;//Bank Book
        public const Int32 nmuJournalRpt = 504;//Journal Book
        public const Int32 nmuLedgDtlsRpt = 505;//General Ledger Details
        public const Int32 nmuLedgSumryRpt = 506;//General Ledger Summary
        public const Int32 mnuRecPayRpt = 507;//Receipt Payment
        public const Int32 nmuProfitLossRpt = 508;//Profit & Loss
        public const Int32 nmuTrailRpt = 509;//Trail Balance
        public const Int32 nmuBalanceSheetRpt = 510;//Balance Sheet
        public const Int32 nmuBankRconsilStatRpt = 511;//Bank Reconcile Statement

        //Master Listing 
        public const Int32 nmuTehBlocVilMohRpt = 512;//Taluka/Block/Village/Mohalla Report
        public const Int32 nmuLoanPurpseRpt = 513;//Loan Purpose Report
        public const Int32 nmuEmpMstRpt = 514;//Employee Report

        //Loan Related
        public const Int32 nmuCGTRpt = 515;//CGT Report
        public const Int32 nmuLoanSpficRpt = 9050;//Loan Sanction Report
        public const Int32 nmuLoanDisbRpt = 517;//Loan Disbursement List
        public const Int32 nmuCollDisbRpt = 518;//Collection Disbursement Sheet
        public const Int32 nmuPartLedgRpt = 519;//Party Ledger
        public const Int32 nmuRepayScheRpt = 520;//Repayment Schedule
        public const Int32 nmuCollectionRpt = 521;//Collection Report
        public const Int32 nmuFinlPaidRpt = 522;//Settled Clients
        public const Int32 nmuLoanUtiRpt = 523;//Loan Utilization
        public const Int32 mnuLoanPreCloserView = 541;//Member's Loan Pre-Closer View
        public const Int32 mnuCAMRpt = 542;//CAM Report
        public const Int32 mnuBankStatmntRpt = 543;//Bank Statement Report
        public const Int32 mnuFinStatmntRpt = 544;//Financial Statement Report
        public const Int32 mnuSettleCustRpt = 545;//Settle Customer Report
        public const Int32 mnuLenderRpt = 546;//Lender Report
        public const Int32 mnuBounceListRpt = 547;//Bounce Report
        public const Int32 mnuNonBounceListRpt = 548;//Non Bounce List
        public const Int32 mnuPortfolioBreakupRpt = 549;//Portfolio Break Up
        public const Int32 mnuMOMDisbRpt = 550;//Month On Month Disb
        public const Int32 mnuPortfolioCutsbRpt = 551;//Portfolio Cuts
        public const Int32 mnuDisbCutsbRpt = 552;//Disbursement Cuts
        public const Int32 mnuWriteOffDeclareRPt = 553;//Write-Off Declaration Report
        public const Int32 mnuLoginMISRpt = 554;//Bounce List
        public const Int32 mnuWriteOffCollRPt = 555;//Write-Off Collection Report
        public const Int32 mnuWriteOffStatusRPt = 556;//Write-Off Status Report
        public const Int32 mnuFutureDemandRPt = 557;//Future Demand Report
        public const Int32 mnuFuturePaymentLendRPt = 558;//Future Payment Lender Report
        public const Int32 nmuLendRepayScheRpt = 559;//Repayment Schedule
        public const Int32 mnuLoginMISSummaryRpt = 560;//Login MIS Summary
        public const Int32 mnuSMSStatusRpt = 561;//SMS Status Report
        public const Int32 nmuCBCheckRpt = 562;//CB Check Report
        public const Int32 nmuLogInMisRpt = 563;//Log In MIS Report
        public const Int32 mnuCBOverrideReport = 564;//CB Override Report
        public const Int32 mnuOthCollRpt = 565;//Other Collection Report
        public const Int32 nmuNDC = 835;// MEL No Due Certificate

        //Monitoring Reports
        public const Int32 mnuDemndCollRpt = 524;//Demand and Collection
        public const Int32 mnuCOPerformRpt = 525;//CRO Performance Report
        public const Int32 mnuBrnchPerformRpt = 526;//Branch Performance Report

        //MIS Reports
        public const Int32 mnuLoanStatusCombineRpt = 527;//Loan Status(Combine)
        public const Int32 mnuLoanStatusRpt = 528;//Loan Status
        public const Int32 mnuAtGlanceRpt = 529;//At a Glance
        public const Int32 mnuPortfolioActivityRpt = 530;//Portfolio Activity
        public const Int32 mnuAgeDefltSheetRpt = 531;//Age Wise Default List
        public const Int32 mnuPortflioAginRpt = 532;//Portfolio Ageing
        public const Int32 mnuFeesCollecRpt = 533;//Fees Collection
        public const Int32 mnuInsuranceRpt = 534;// Insurance Report
        public const Int32 mnuEOCgtGrtRpt = 535;// CRO wise CGT GRT Report

        //Write-Off Reports
        public const Int32 mnuWriteOffDecRpt = 536;//  Write Off Declaration Report
        public const Int32 mnuWriteOffDemdRpt = 537;//  Write Off Demand Report
        public const Int32 mnuWriteOffCollRpt = 538;//  Write Off Collection Report
        public const Int32 mnuWriteOffStatusRpt = 539;//  Write Off Status Report


        //Deposit
        public const Int32 mnuSavDetList = 540;// SavingsDetails list


        /// <summary>
        /// For Menu System  601 to 700
        /// </summary>        
        public const Int32 mnuDayendProc = 601;//Day End Process
        public const Int32 mnuYearEnd = 602;//Year End Process
        public const Int32 mnuRole = 603;//Role Creation
        public const Int32 mnuAssigneRole = 604;//Assign Role
        public const Int32 mnuUser = 605;//User Creation
        public const Int32 mnuChangePass = 606;//Change Password
        public const Int32 mnuMobUser = 607;//Change Password
        public const Int32 mnuSMSReminder = 608;//SMS Reminder
        /// <summary>
        /// For Utility 701 to 800
        /// </summary>
        public const Int32 mnuTransfrWithinBr = 701;//Transfer Within Branch
        public const Int32 mnuIntrBrTransfr = 9071;//Inter Branch Transfer

        /// <summary>
        /// For Consolidated Reports 801 to 900
        /// </summary>

        // Dashboard
        public const Int32 mnuDSBPrinOSRpt = 801;//Dashboard Principal Outstanding
        public const Int32 mnuDSBActvClientRpt = 802;//Dashboard Active Client
        public const Int32 mnuDSBODClientRpt = 803;//Dashboard OD Client
        public const Int32 mnuDSBODAmtRpt = 804;//Dashboard OD Amount
        public const Int32 mnuDSBODOSRpt = 805;//Dashboard OD Outstanding
        public const Int32 mnuDSBDisbNoRpt = 806;//Dashboard No Of Loan
        public const Int32 mnuDSBDisbAmtRpt = 807;//Dashboard Disburse Amount

        // MIS Reports
        public const Int32 mnuHOHghMrkDataSub = 808; // HO Highmark Data Submission
        public const Int32 mnuHghMrkAnlys = 809; // HO HighMark Data Analysis
        public const Int32 mnuHOCleintProfile = 810; // HO Client Profile Report
        public const Int32 mnuHOLoanSanction = 811; // HO Loan Sanction Report
        public const Int32 mnuHOLoanDisb = 812; // HO Loan Disbursement Report
        public const Int32 mnuHODemandSheet = 813; // HO Loan Demand Sheet Report
        public const Int32 mnuHOLoanCollection = 814; // HO Loan Collection Report
        public const Int32 mnuLoanStatusBucket = 815; // HO Loan Status Bucket Report
        public const Int32 mnuHOComAnalysisPos = 816; // Comparative Analysis Report
        public const Int32 mnuHOPortAgeing = 817; // HO Portfolio Ageing Report
        public const Int32 mnuHOPortActivity = 818; // HO Portfolio Activity Report
        public const Int32 mnuHOAtGlanceRpt = 819; // HO At A Glance Report
        public const Int32 mnuHOODClientList = 820; // HO Overdue Client Report
        public const Int32 mnuHOLoanUtilisationRpt = 821; // HO Loan Utilization Report
        public const Int32 mnuHOFeesCollecRpt = 822;//HO Fees collection report
        public const Int32 mnuHOSettledClientRpt = 823;//HO Settle Client Report
        public const Int32 mnuHOPARCategoryRpt = 824; // HO PAR Category";
        public const Int32 mnuHOAccIntPostRpt = 831;//Accrued Interest Posting Report
        public const Int32 mnuBrHMEnqRpt = 832; //HO Highmark Enquiry Report (Branch Wise)
        public const Int32 mnuHOSavDetList = 833; //HO Savings Detail List
        public const Int32 mnuHOInsuranceRpt = 9068;//Insurance Data

        // HO Accounts Reports
        public const Int32 nmuHOLedgDtlsRpt = 825; // HO Account Ledger Details";
        public const Int32 mnuHORecPayRpt = 826; // HO Receipt Payment Report";
        public const Int32 mnuHOTrial = 827; // HO Trial Balance Report";
        public const Int32 mnuHOProfitLoss = 828; // HO Profit And Loss Report";
        public const Int32 nmuHOBalanceSheetRpt = 829; // HO Balance Sheet Report";
        public const Int32 mnuHOBankReconRpt = 830; // HO Bank Reconciliation Report";


        

       


        /// <summary>
        /// Unknown Menu items 901 to 1000 
        /// </summary>
        public const Int32 mnuYBPaymentSchedule = 901;//Yes Bank Payment Schedule
        public const Int32 nmuGRTRpt = 902;//GRT Report
        public const Int32 mnuChequePrint = 903;//Cheque Printing
        public const Int32 mnuDPN = 904;//Demand Promissory Note
        public const Int32 mnuSavLedger = 905;//Savings Ledger
        public const Int32 mnuHOHMEnqRpt = 906; //Consolidated High Mark Data Enquiry
        public const Int32 mnuDocUpLoad = 907;//Documents UpLoad
        public const Int32 mnuHVisit = 908;//House Visit (GRT Operation)
        public const Int32 mnuRedFlag = 910;//Red Flagging


        //// View Menu Item 
        public const Int32 mnuApplicant = 920;//Applicant
        public const Int32 mnuAppLnApplication = 921;//Loan Application
        public const Int32 mnuCoApplicant = 922;//Co Applicant
        public const Int32 mnuCompany = 923;//Company

        public const Int32 mnuCB = 924;//Credit breaue
        public const Int32 mnuBankAC = 925;//Applicant Bank Account
        public const Int32 mnuBSStatement = 926;//Balance Sheet Statement
        public const Int32 mnuPLStatement = 927;//Profit Loass Statement
        public const Int32 mnuReference = 928;//Reference 
        public const Int32 mnuCAM = 929;//CAM
        public const Int32 mnuComBackground = 930;//Company Background
        public const Int32 mnuPromoBackground = 931;//Promoter Background
        public const Int32 mnuCheckList = 932;//Check List
        public const Int32 mnuGrpComBackInfo = 933;//Information of Group Companies 

        //BONFLEET 950
        public const Int32 mnuBonCustReg = 950;//Bonfleet Customer Registration Report
        public const Int32 mnuBonLoanDisb = 951;//Bonfleet Loan Disbursement Report
        public const Int32 mnuBonLoanColl = 952;//Bonfleet Loan Collection Report
        public const Int32 mnuBonCustRegistration = 953;//Bonfleet  Customer Registration
        public const Int32 nmuBonPartLedgRpt = 954;//Bonfleet  Party Ledger
        public const Int32 mnuBonLoanStatusRpt = 955;//Bonfleet  Loan Status Report
        public const Int32 mnuBonSettleCustRpt = 956;//Bonfleet  Settled Customer

        //Personal Discussion 1001
        public const Int32 mnuPD = 1001;//Personal Discussion
        public const Int32 mnuPDBM = 1002;//Personal Discussion By BM
        public const Int32 mnuPDCM = 1003;//Personal Discussion By CM
        public const Int32 mnuPDRisk = 1004;//Personal Discussion By Risk

        // Legal
        public const Int32 mnuBrLegal = 1010;//Branch Legal Process
        public const Int32 mnuHOLegal = 1011;//Branch Legal Process
        public const Int32 mnuPreOpinion = 1012;//Branch Legal Process
        public const Int32 mnuBranchQueryResponse = 1013;//Branch Response Against Query
        public const Int32 mnuFinalLegalOpinion = 1014;//Final Legal Opinion
        public const Int32 mnuLegalMODTD = 1024;//LEGAL MODTD
        public const Int32 mnuLegalResolveQuery = 1025;//LEGAL MODTD
        public const Int32 mnuLegalDocReceived = 1026;//LEGAL Document Receive
        public const Int32 mnuLegalLOA = 1027;//LOA
        public const Int32 mnuLegalDocSend = 1028;//Original LEGAL Document Send From Branch
        public const Int32 mnuLegalDocRecByHO = 1029;//Original LEGAL Document Rec By HO
        public const Int32 mnuLegalLOD = 1030;//Legal List Of Documents
        public const Int32 mnuLegalDocSendToCustody = 1031;//DOCUMENTS SEND TO SAFE CUSTODY
        public const Int32 mnuLegalPreCloseDocRecByHO = 1032;//PRE-CLOSER DOCUMENTS RECEIVE
        public const Int32 mnuLegalPreCloseDochandOver = 1033;//PRE-CLOSER DOCUMENTS HAND OVER
        public const Int32 mnuLegalDocPrint = 1034;//PRE-CLOSER DOCUMENTS HAND OVER


        public const Int32 mnuIncomeItemMst = 5006;//Income Item Master
        public const Int32 mnuExpenseItemMst = 5007;//Expense Item Master
        public const Int32 mnuOccupationSubTypeMst = 5005;//Occupation Sub Type Master
        public const Int32 mnuNocLetter = 1018;//Loan Sanction
        public const Int32 mnuCollBounceBulk = 1019;//Loan Sanction   
        public const Int32 mnuLoanDisbursementCancel = 1020;//Loan Disbursement Cancel
        public const Int32 mnuLoanRecovaryAddj = 5014; //Loan Recovery Adjustment
        public const Int32 mnuTransfr = 5016; //Member Transfer
        public const Int32 mnuFetchAadhaarDetails = 5019; //Fetch Aadhaar Details
        public const Int32 mnuCBEnqRpt = 5036;//CB Enquiry Report 
        public const Int32 mnuOtherChargesDemandSheet = 9087; //Other Charges Demand Sheet(Mel)


        // CF Menu Start

        public const Int32 mnuCFNewLeadGen = 20000;// New Lead Generation(CF)
        public const Int32 mnuCFBasicDet = 20001;// Basic Details(CF)
        public const Int32 mnuCFBCvendorRep = 20002;// BC Vendor Reports(CF)
        public const Int32 mnuCFBusEmpDet = 20003;// Employment/Business Details(CF)
        public const Int32 mnuCFDocUpload = 20004;// Document Upload(CF)
        public const Int32 mnuCFExtrChk = 20005;// External Checks(CF)
        public const Int32 mnuCFEPCMst = 20006;// EPC Master(CF)
        public const Int32 mnuCFIntrChk = 20007;// Internal Checks(CF)
        public const Int32 mnuCFOthrRep = 20008;// Other Reports(CF)
        public const Int32 mnuCFCust360 = 20009;// Customer 360(CF)
        public const Int32 mnuCFIncDtl = 20010;// Income Detail(CF)
        public const Int32 mnuCFElecConAnlys = 20011;// Electric Consumption Analysis(CF)

        public const Int32 mnuCFObligationDtl = 20012;// Obligation Details(CF)
        public const Int32 mnuCFBankingDtl = 20013;// Banking Details(CF)

        public const Int32 mnuCFSolarDtl = 20014;// Solar Details(CF)
        public const Int32 mnuCFRocommendation = 20015;// Recommendation (CF)
        public const Int32 mnuCFLTVComputation = 20016;// LTV Computation (CF)
        public const Int32 mnuCFInsurence = 20017;// Insurence Charges (CF)
        public const Int32 mnuCFSanction = 20018;// Sanction Condition (CF)
        public const Int32 mnuCFDeviation = 20019;// Deviation (CF)
        public const Int32 mnuCFFinalDec = 20020;// Final Decision (CF)   
        public const Int32 mnuCFLoanApp = 20021;// Loan Application (CF)  

        public const Int32 mnuCFBCM = 20022;// BC-BCM (CF)
        public const Int32 mnuCFHO = 20023;// HO-CREDIT (CF)
        public const Int32 mnuCFBOE = 20024;// BC-BOE/BM (CF)
        public const Int32 mnuCFLAS = 20025;// Loan Application Status (CF)
        public const Int32 mnuCFLDD = 20026;// Loan Disbursement Details (CF)

        public const Int32 mnuCFTRN = 20027;// Loan Tranche Disbursement (CF)
        public const Int32 mnuCFUMRN = 20028;// UMRN Update (CF)
        public const Int32 mnuCFLoanApp1 = 20029; // Loan Application Central OPS-1 Stage (CF)  
        public const Int32 mnuCFLoanApp2 = 20030; // Loan Application Central OPS-2 Stage (CF) 

        public const Int32 mnuPreMatColl = 20031; // Premature Collection(CF)
        public const Int32 mnuCFProvDD = 20032; // Provisional Death Declare(CF)
        public const Int32 mnuCFDeathDeclare = 20033; // Death Declare(CF 
        public const Int32 mnuCFDDCloseLoan = 20034; // Death Declaration Close Loan(CF) 
        public const Int32 mnuCFDDocUpLd = 20035; // Death Document Upload(CF) 
        public const Int32 mnuCFDownloadCertificate = 20036; // Download and Verification Document(CF) 
        public const Int32 mnuLoanRecoverySngl = 20037;//Loan Recovery Single
        public const Int32 mnuLoanRecoveryBlk = 20038;//Loan Recovery Bulk
        public const Int32 nmuCollectionRptCF = 20039;//Cllection Report(CF)
        public const Int32 mnuDeathFlagCF = 20040;//Death Flaging(CF)
        public const Int32 mnuHoDwnldCrtCF = 20041;//Download and Verification Death Document(CF)
        public const Int32 mnuGenParameterCF = 20042;//General Parameter(CF)
        public const Int32 mnuCollDelRevCF = 20043;//Collection Delete Reverse(CF)

        public const Int32 mnuPreDBRptCF = 20044;//Pre-DB Report(CF)
        public const Int32 mnuDeathTgRptCF = 20045;//Death Tagging Report(CF)

        public const Int32 mnuBranchMstRptCF = 20046;//Branch Master Report(CF)       
        public const Int32 mnuUserMstRptCF = 20047;//User Master Report(CF)
        public const Int32 mnuAllEPCRptCF = 20048;//All EPC Report(CF)
        public const Int32 mnuEPCBankingRptCF = 20049;//EPC Banking Report(CF)
        public const Int32 mnuHypothecationRptCF = 20050;//Hypothecation Report(CF)
        public const Int32 mnuCustomerDtlRptCF = 20051;//Customer Detail Report(CF)
        public const Int32 mnuSanctionCondRptCF = 20052;//Sanction Condition Report(CF)
        public const Int32 mnuCustomerBankingRptCF = 20053;//Customer Banking Report(CF)
        public const Int32 mnuCustChargesRptCF = 20054;//Customer Charges Report(CF)
        public const Int32 mnuDeathSettlRptCF = 20055;//Death Settlement Report(CF)
        public const Int32 mnuPremCollRptCF = 20056;//Premature Collection Report(CF)
        public const Int32 mnuHONEFTTransferAPICF = 20057;//Neft Transfer(CF)
        public const Int32 mnuDeathDoccancelCF = 20058; // Death Doc cancel After Verification
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
    public class gblPRATAM
    {
        //private gblKUDOS()
        //{ }

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

        public static string CheckMsg
        {
            get
            {
                return "Select CheckBox!!";
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

        public static string NotDeleteMsg
        {
            get
            {
                return "Nothing To Delete!";
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
                return "This Is Not A Valid User For This Branch";
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ValidEmail
        {
            get
            {
                return "This Is Not A Valid Email.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string InvalidUser
        {
            get
            {
                return "Invalid User Name/Password. Please Try Again.";
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