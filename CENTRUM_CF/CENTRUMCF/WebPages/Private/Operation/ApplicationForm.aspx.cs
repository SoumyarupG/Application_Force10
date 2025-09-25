using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using CENTRUMCA;

namespace CENTRUMCF.WebPages.Private.Operation
{
    public partial class ApplicationForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadRecord();
        }

        private void LoadRecord()
        {
            string vChannel = "", vSubSource = "", vEPC = "";

            string vApplicationDt = "", vBranchOffice = "", vApplicationType = "", vLoanType = "", vLEINo = "", vLoanAmt = "", vRepayOptn = "", vTenor = "",
            vBankNm = "", vBranch = "", vAcHolderNm = "", vAcNo = "", vIFSC = "", vAcType = "";

            string vApplName = "", vResiLandmark = "", vResiCity = "", vResiPin = "", vResiResidenceType = "", vResiYrCurrAdd = "", vOfcLandmark = "", vOfcCity = "", vOfcPin = "",
            vPerLandmark = "", vPerCity = "", vPerPin = "", vPerResiType = "", vPerYrCurrAdd = "", vSTD = "", vLandLine1 = "", vLandLine2 = "", vMobile1 = "", vMobile2 = "", vEmailAddr = "",
            vDOB = "", vGender = "", vMaritalStatus = "", vHighestEdu = "", vPanNo = "", vReligion = "", vCaste = "", vPersonWithDisability = "", vConstitution = "", vSalaried = "",
            vContactPersonNm = "", vSelfEmployed = "", vNatureOfActivity = "", vSelfIncome = "", vFamilyIncome = "", vWorkingMembersInFamily = "", vPEP = "", vBankBranch = "", vLoanAmtReq = "";

            string vCoAppNm1 = "", vCoApp1 = "N", vResiLandmark1 = "", vResiCity1 = "", vResiPin1 = "", vResiResidenceType1 = "", vResiYrCurrAdd1 = "", vOfcLandmark1 = "", vOfcCity1 = "", vOfcPin1 = "",
            vPerLandmark1 = "", vPerCity1 = "", vPerPin1 = "", vPerResiType1 = "", vPerYrCurrAdd1 = "", vSTD1 = "", vLandLine1_1 = "", vLandLine2_1 = "", vMobile1_1 = "", vMobile2_1 = "", vEmailAddr1 = "",
            vDOB1 = "", vGender1 = "", vMaritalStatus1 = "", vHighestEdu1 = "", vPanNo1 = "", vReligion1 = "", vCaste1 = "", vSalaried1 = "",
            vContactPersonNm1 = "", vSelfEmployed1 = "", vNatureOfActivity1 = "", vPEP1 = "", vAnnualTurnOver1 = "", vRelWithApp1 = "";

            string vCoAppNm2 = "", vCoApp2 = "N", vResiLandmark2 = "", vResiCity2 = "", vResiPin2 = "", vResiResidenceType2 = "", vResiYrCurrAdd2 = "", vOfcLandmark2 = "", vOfcCity2 = "", vOfcPin2 = "",
            vPerLandmark2 = "", vPerCity2 = "", vPerPin2 = "", vPerResiType2 = "", vPerYrCurrAdd2 = "", vSTD2 = "", vLandLine1_2 = "", vLandLine2_2 = "", vMobile1_2 = "", vMobile2_2 = "", vEmailAddr2 = "",
            vDOB2 = "", vGender2 = "", vMaritalStatus2 = "", vHighestEdu2 = "", vPanNo2 = "", vReligion2 = "", vCaste2 = "", vSalaried2 = "",
            vContactPersonNm2 = "", vSelfEmployed2 = "", vNatureOfActivity2 = "",  vPEP2 = "", vAnnualTurnOver2 = "", vRelWithApp2 = "";

            string vCoAppNm3 = "", vCoApp3 = "N", vResiLandmark3 = "", vResiCity3 = "", vResiPin3 = "", vResiResidenceType3 = "", vResiYrCurrAdd3 = "", vOfcLandmark3 = "", vOfcCity3 = "", vOfcPin3 = "",
            vPerLandmark3 = "", vPerCity3 = "", vPerPin3 = "", vPerResiType3 = "", vPerYrCurrAdd3 = "", vSTD3 = "", vLandLine1_3 = "", vLandLine2_3 = "", vMobile1_3 = "", vMobile2_3 = "", vEmailAddr3 = "",
            vDOB3 = "", vGender3 = "", vMaritalStatus3 = "", vHighestEdu3 = "", vPanNo3 = "", vReligion3 = "", vCaste3 = "", vSalaried3 = "",
            vContactPersonNm3 = "", vSelfEmployed3 = "", vNatureOfActivity3 = "",  vPEP3 = "", vAnnualTurnOver3 = "", vRelWithApp3 = "";

            string vCoAppNm4 = "", vCoApp4 = "Y", vResiLandmark4 = "", vResiCity4 = "", vResiPin4 = "", vResiResidenceType4 = "", vResiYrCurrAdd4 = "", vOfcLandmark4 = "", vOfcCity4 = "", vOfcPin4 = "",
            vPerLandmark4 = "", vPerCity4 = "", vPerPin4 = "", vPerResiType4 = "", vPerYrCurrAdd4 = "", vSTD4 = "", vLandLine1_4 = "", vLandLine2_4 = "", vMobile1_4 = "", vMobile2_4 = "", vEmailAddr4 = "",
            vDOB4 = "", vGender4 = "", vMaritalStatus4 = "", vHighestEdu4 = "", vPanNo4 = "", vReligion4 = "", vCaste4 = "",  vSalaried4 = "",
            vContactPersonNm4 = "", vSelfEmployed4 = "", vNatureOfActivity4 = "",  vPEP4 = "", vAnnualTurnOver4 = "", vRelWithApp4 = "";

            string vCoAppNmG = "", vCoAppG = "N", vResiLandmarkG = "", vResiCityG = "", vResiPinG = "", vResiResidenceTypeG = "", vResiYrCurrAddG = "", vOfcLandmarkG = "", vOfcCityG = "", vOfcPinG = "",
            vPerLandmarkG = "", vPerCityG = "", vPerPinG = "", vPerResiTypeG = "", vPerYrCurrAddG = "", vSTDG = "", vLandLine1_G = "", vLandLine2_G = "", vMobile1_G = "", vMobile2_G = "", vEmailAddrG = "",
            vDOBG = "", vGenderG = "", vMaritalStatusG = "", vHighestEduG = "", vPanNoG = "", vReligionG = "", vCasteG = "", vSalariedG = "",
            vContactPersonNmG = "", vSelfEmployedG = "", vNatureOfActivityG = "",  vPEPG = "", vAnnualTurnOverG = "", vRelWithAppG = "";

            string vBussDesc = "", vYrInBuss = "", vNoOfEmp = "", vInvestmentAmt = "", vGSTRegYN = "", vGSTNo = "", vLenderNm = "", vROI = "";

            string vSecurityType = "", vOwnerNm = "", vOwnershipType = "", vCollateralCity = "", vCollateralState = "", vCollateralPin = "", vMarketableValue = "";

            string vIdentityProof = "", vResidenceProof = "", vIncomeProof = "", vOtherDocument = "";

            string vRelatedToBank1 = "", vRelatedToBank2 = "";

            string vApplFormNo = "", vApplDt = "", vProduct = "", vLoanTerm;

            DataSet ds = (DataSet)Session["SLDt"];
            DataTable dtApplDtl = ds.Tables[0];
            DataTable dtLoan = ds.Tables[1];
            DataTable dtSecurity = ds.Tables[2];
            DataTable dtKyc = ds.Tables[3];
            DataTable dtBankRel = ds.Tables[4];
            DataTable dtFamily = ds.Tables[5];
            DataTable dtDirectr = ds.Tables[6];
            DataTable dtCoApp1 = ds.Tables[7];
            DataTable dtCoApp2 = ds.Tables[8];
            DataTable dtCoApp3 = ds.Tables[9];
            DataTable dtCoApp4 = ds.Tables[10];
            DataTable dtCoAppG = ds.Tables[11];
            DataTable dtFlag = ds.Tables[12];

            StringBuilder sb = new StringBuilder();



            if (dtApplDtl.Rows.Count > 0)
            {

                try
                {


                    #region VARIABLE_DECLARATION

                    #region SOURCING_DETAILS

                    vChannel = dtApplDtl.Rows[0]["Channel"].ToString();
                    vSubSource = dtApplDtl.Rows[0]["SubSource"].ToString();
                    vEPC = dtApplDtl.Rows[0]["EpcName"].ToString();

                    #endregion

                    #region LOAN_PURPOSE_DETAILS

                    vApplicationDt = dtApplDtl.Rows[0]["AppDate"].ToString();
                    vBranchOffice = dtApplDtl.Rows[0]["BranchName"].ToString();
                    vApplicationType = dtApplDtl.Rows[0]["ApplicantType"].ToString();
                    vLoanTerm = dtApplDtl.Rows[0]["LoanTerm"].ToString();
                    vLoanType = dtApplDtl.Rows[0]["LoanTypeName"].ToString();
                    vLEINo = dtApplDtl.Rows[0]["LEINo"].ToString();
                    vLoanAmtReq = dtApplDtl.Rows[0]["LoanAmtRequerd"].ToString();
                    vLoanAmt = dtApplDtl.Rows[0]["LoanAmt"].ToString();
                    vRepayOptn = dtApplDtl.Rows[0]["RepaymentOption"].ToString();
                    vTenor = dtApplDtl.Rows[0]["Tenure"].ToString();
                    vBankNm = dtApplDtl.Rows[0]["BankName"].ToString();
                    vBankBranch = dtApplDtl.Rows[0]["BankBranch"].ToString();
                    vAcHolderNm = dtApplDtl.Rows[0]["AcHoldName"].ToString();
                    vAcNo = dtApplDtl.Rows[0]["AccNo"].ToString();
                    vIFSC = dtApplDtl.Rows[0]["IFSCCode"].ToString();
                    vAcType = dtApplDtl.Rows[0]["AcType"].ToString();

                    //--  EpcName, AppNo,AppDate , BranchName ,ApplicantType, RepaymentOption,OBD.AcHoldName,OBD.BankName,OBD.AccNo,OBD.AcType,OBD.IFSCCode
                    //-- MemberName ,CurrPin,CurrAdd,CurrLandmark,ResiStabYrs,OwnShipStatus, PerPin,PerAdd , Email, MobNo, DOB, Gender, MaritalStatus,Education,PanNo
                    //-- Caste, Religion

                    #endregion

                    #region PERSONAL_DETAILS

                    vApplName = dtApplDtl.Rows[0]["MemberName"].ToString();
                    vResiLandmark = dtApplDtl.Rows[0]["CurrLandmark"].ToString(); // Current Landmark
                    vResiCity = dtApplDtl.Rows[0]["CurCity"].ToString();
                    vResiPin = dtApplDtl.Rows[0]["CurrPin"].ToString();
                    vResiResidenceType = dtApplDtl.Rows[0]["OwnShipStatus"].ToString();
                    vResiYrCurrAdd = dtApplDtl.Rows[0]["ResiStabYrs"].ToString();
                    vOfcLandmark = dtApplDtl.Rows[0]["OfcLandmark"].ToString();
                    vOfcCity = dtApplDtl.Rows[0]["OfcCity"].ToString();
                    vOfcPin = dtApplDtl.Rows[0]["OfcPin"].ToString();
                    vPerLandmark = dtApplDtl.Rows[0]["PerLandMark"].ToString();
                    vPerCity = dtApplDtl.Rows[0]["PerCity"].ToString();
                    vPerPin = dtApplDtl.Rows[0]["PerPin"].ToString();
                    vPerResiType = dtApplDtl.Rows[0]["PerResiType"].ToString();
                    vPerYrCurrAdd = dtApplDtl.Rows[0]["PerResiYr"].ToString();
                    vSTD = dtApplDtl.Rows[0]["STDCODE"].ToString();
                    vLandLine1 = dtApplDtl.Rows[0]["TelePh1"].ToString();
                    vLandLine2 = dtApplDtl.Rows[0]["TelePh2"].ToString();
                    vMobile1 = dtApplDtl.Rows[0]["MobNo"].ToString();
                    vMobile2 = dtApplDtl.Rows[0]["MobNo2"].ToString();
                    vEmailAddr = dtApplDtl.Rows[0]["Email"].ToString();
                    vDOB = dtApplDtl.Rows[0]["DOB"].ToString();
                    vGender = dtApplDtl.Rows[0]["Gender"].ToString();
                    vMaritalStatus = dtApplDtl.Rows[0]["MaritalStatus"].ToString();
                    vHighestEdu = dtApplDtl.Rows[0]["Education"].ToString();
                    vPanNo = dtApplDtl.Rows[0]["PanNo"].ToString();
                    vReligion = dtApplDtl.Rows[0]["Religion"].ToString();
                    vCaste = dtApplDtl.Rows[0]["Caste"].ToString();
                    vPersonWithDisability = dtApplDtl.Rows[0]["PersonWithDisability"].ToString();
                    vConstitution = dtApplDtl.Rows[0]["Constitution"].ToString();
                    vSalaried = dtApplDtl.Rows[0]["Salaried"].ToString();
                    vContactPersonNm = dtApplDtl.Rows[0]["ContactPersonNm"].ToString();
                    vSelfEmployed = dtApplDtl.Rows[0]["SelfEmployed"].ToString();
                    vNatureOfActivity = dtApplDtl.Rows[0]["NatureOfActivity"].ToString();
                    vSelfIncome = dtApplDtl.Rows[0]["SelfIncome"].ToString();
                    vFamilyIncome = dtApplDtl.Rows[0]["FamilyIncome"].ToString();
                    vWorkingMembersInFamily = dtApplDtl.Rows[0]["WorkingMembersInFamily"].ToString();
                    vApplFormNo = dtApplDtl.Rows[0]["AppNo"].ToString();
                    vApplDt = dtApplDtl.Rows[0]["AppDate"].ToString();
                    vProduct = dtApplDtl.Rows[0]["Product"].ToString();
                    vPEP = dtApplDtl.Rows[0]["PEP"].ToString();

                    #endregion

                    #region DETAILS_OF_CO_APPLICANT_1

                    if (dtCoApp1.Rows.Count > 0)
                    {
                        vCoAppNm1 = dtCoApp1.Rows[0]["CoAppName"].ToString();
                        vRelWithApp1 = dtCoApp1.Rows[0]["RelWithApp"].ToString();
                        vResiLandmark1 = dtCoApp1.Rows[0]["CurrLandmark"].ToString();
                        vResiCity1 = dtCoApp1.Rows[0]["CurrAdd"].ToString();
                        vResiPin1 = dtCoApp1.Rows[0]["CurrPin"].ToString();
                        vResiResidenceType1 = dtCoApp1.Rows[0]["OwnShipStatus"].ToString();
                        vResiYrCurrAdd1 = dtCoApp1.Rows[0]["ResiStabYrs"].ToString();
                        vOfcLandmark1 = dtCoApp1.Rows[0]["OfcLandmark"].ToString();
                        vOfcCity1 = dtCoApp1.Rows[0]["OfcCity"].ToString();
                        vOfcPin1 = dtCoApp1.Rows[0]["OfcPin"].ToString();
                        vPerLandmark1 = dtCoApp1.Rows[0]["PerLandMark"].ToString();
                        vPerCity1 = dtCoApp1.Rows[0]["PerAdd"].ToString();
                        vPerPin1 = dtCoApp1.Rows[0]["PerPin"].ToString();
                        vPerResiType1 = dtCoApp1.Rows[0]["PerResiType"].ToString();
                        vPerYrCurrAdd1 = dtCoApp1.Rows[0]["PerResiYr"].ToString();
                        vSTD1 = dtCoApp1.Rows[0]["STDCODE"].ToString();
                        vLandLine1_1 = dtCoApp1.Rows[0]["TelePh1"].ToString();
                        vLandLine2_1 = dtCoApp1.Rows[0]["TelePh2"].ToString();
                        vMobile1_1 = dtCoApp1.Rows[0]["MobNo"].ToString();
                        vMobile2_1 = dtCoApp1.Rows[0]["MobNo2"].ToString();
                        vEmailAddr1 = dtCoApp1.Rows[0]["Email"].ToString();
                        vDOB1 = dtCoApp1.Rows[0]["DOB"].ToString();
                        vGender1 = dtCoApp1.Rows[0]["Gender"].ToString();
                        vMaritalStatus1 = dtCoApp1.Rows[0]["MaritalStatus"].ToString();
                        vHighestEdu1 = dtCoApp1.Rows[0]["Education"].ToString();
                        vPanNo1 = dtCoApp1.Rows[0]["PanNo"].ToString();
                        vReligion1 = dtCoApp1.Rows[0]["Religion"].ToString();
                        vCaste1 = dtCoApp1.Rows[0]["Caste"].ToString();                       
                        vSalaried1 = dtCoApp1.Rows[0]["Salaried"].ToString();
                        vContactPersonNm1 = dtCoApp1.Rows[0]["ContactPersonNm"].ToString();
                        vSelfEmployed1 = dtCoApp1.Rows[0]["SelfEmployed"].ToString();
                        vNatureOfActivity1 = dtCoApp1.Rows[0]["NatureOfActivity"].ToString();                        
                        vAnnualTurnOver1 = dtCoApp1.Rows[0]["AnnualTurnOver"].ToString();
                        vPEP1 = dtCoApp1.Rows[0]["PEP"].ToString();
                    }

                    #endregion

                    #region DETAILS_OF_CO_APPLICANT_2

                    if (dtCoApp2.Rows.Count > 0)
                    {
                        vCoAppNm2 = dtCoApp2.Rows[0]["CoAppName"].ToString();
                        vRelWithApp2 = dtCoApp2.Rows[0]["RelWithApp"].ToString();
                        vResiLandmark2 = dtCoApp2.Rows[0]["CurrLandmark"].ToString();
                        vResiCity2 = dtCoApp2.Rows[0]["CurrAdd"].ToString();
                        vResiPin2 = dtCoApp2.Rows[0]["CurrPin"].ToString();
                        vResiResidenceType2 = dtCoApp2.Rows[0]["OwnShipStatus"].ToString();
                        vResiYrCurrAdd2 = dtCoApp2.Rows[0]["ResiStabYrs"].ToString();
                        vOfcLandmark2 = dtCoApp2.Rows[0]["OfcLandmark"].ToString();
                        vOfcCity2 = dtCoApp2.Rows[0]["OfcCity"].ToString();
                        vOfcPin2 = dtCoApp2.Rows[0]["OfcPin"].ToString();
                        vPerLandmark2 = dtCoApp2.Rows[0]["PerLandMark"].ToString();
                        vPerCity2 = dtCoApp2.Rows[0]["PerAdd"].ToString();
                        vPerPin2 = dtCoApp2.Rows[0]["PerPin"].ToString();
                        vPerResiType2 = dtCoApp2.Rows[0]["PerResiType"].ToString();
                        vPerYrCurrAdd2 = dtCoApp2.Rows[0]["PerResiYr"].ToString();
                        vSTD2 = dtCoApp2.Rows[0]["STDCODE"].ToString();
                        vLandLine1_2 = dtCoApp2.Rows[0]["TelePh1"].ToString();
                        vLandLine2_2 = dtCoApp2.Rows[0]["TelePh2"].ToString();
                        vMobile1_2 = dtCoApp2.Rows[0]["MobNo"].ToString();
                        vMobile2_2 = dtCoApp2.Rows[0]["MobNo2"].ToString();
                        vEmailAddr2 = dtCoApp2.Rows[0]["Email"].ToString();
                        vDOB2 = dtCoApp2.Rows[0]["DOB"].ToString();
                        vGender2 = dtCoApp2.Rows[0]["Gender"].ToString();
                        vMaritalStatus2 = dtCoApp2.Rows[0]["MaritalStatus"].ToString();
                        vHighestEdu2 = dtCoApp2.Rows[0]["Education"].ToString();
                        vPanNo2 = dtCoApp2.Rows[0]["PanNo"].ToString();
                        vReligion2 = dtCoApp2.Rows[0]["Religion"].ToString();
                        vCaste2 = dtCoApp2.Rows[0]["Caste"].ToString();
                        vSalaried2 = dtCoApp2.Rows[0]["Salaried"].ToString();
                        vContactPersonNm2 = dtCoApp2.Rows[0]["ContactPersonNm"].ToString();
                        vSelfEmployed2 = dtCoApp2.Rows[0]["SelfEmployed"].ToString();
                        vNatureOfActivity2 = dtCoApp2.Rows[0]["NatureOfActivity"].ToString();
                        vAnnualTurnOver2 = dtCoApp2.Rows[0]["AnnualTurnOver"].ToString();
                        vPEP2 = dtCoApp2.Rows[0]["PEP"].ToString();
                    }

                    #endregion

                    #region DETAILS_OF_CO_APPLICANT_3

                    if (dtCoApp3.Rows.Count > 0)
                    {
                        vCoAppNm3 = dtCoApp3.Rows[0]["CoAppName"].ToString();
                        vRelWithApp3 = dtCoApp3.Rows[0]["RelWithApp"].ToString();
                        vResiLandmark3 = dtCoApp3.Rows[0]["CurrLandmark"].ToString();
                        vResiCity3 = dtCoApp3.Rows[0]["CurrAdd"].ToString();
                        vResiPin3 = dtCoApp3.Rows[0]["CurrPin"].ToString();
                        vResiResidenceType3 = dtCoApp3.Rows[0]["OwnShipStatus"].ToString();
                        vResiYrCurrAdd3 = dtCoApp3.Rows[0]["ResiStabYrs"].ToString();
                        vOfcLandmark3 = dtCoApp3.Rows[0]["OfcLandmark"].ToString();
                        vOfcCity3 = dtCoApp3.Rows[0]["OfcCity"].ToString();
                        vOfcPin3 = dtCoApp3.Rows[0]["OfcPin"].ToString();
                        vPerLandmark3 = dtCoApp3.Rows[0]["PerLandMark"].ToString();
                        vPerCity3 = dtCoApp3.Rows[0]["PerAdd"].ToString();
                        vPerPin3 = dtCoApp3.Rows[0]["PerPin"].ToString();
                        vPerResiType3 = dtCoApp3.Rows[0]["PerResiType"].ToString();
                        vPerYrCurrAdd3 = dtCoApp3.Rows[0]["PerResiYr"].ToString();
                        vSTD3 = dtCoApp3.Rows[0]["STDCODE"].ToString();
                        vLandLine1_3 = dtCoApp3.Rows[0]["TelePh1"].ToString();
                        vLandLine2_3 = dtCoApp3.Rows[0]["TelePh2"].ToString();
                        vMobile1_3 = dtCoApp3.Rows[0]["MobNo"].ToString();
                        vMobile2_3 = dtCoApp3.Rows[0]["MobNo2"].ToString();
                        vEmailAddr3 = dtCoApp3.Rows[0]["Email"].ToString();
                        vDOB3 = dtCoApp3.Rows[0]["DOB"].ToString();
                        vGender3 = dtCoApp3.Rows[0]["Gender"].ToString();
                        vMaritalStatus3 = dtCoApp3.Rows[0]["MaritalStatus"].ToString();
                        vHighestEdu3 = dtCoApp3.Rows[0]["Education"].ToString();
                        vPanNo3 = dtCoApp3.Rows[0]["PanNo"].ToString();
                        vReligion3 = dtCoApp3.Rows[0]["Religion"].ToString();
                        vCaste3 = dtCoApp3.Rows[0]["Caste"].ToString();
                        vSalaried3 = dtCoApp3.Rows[0]["Salaried"].ToString();
                        vContactPersonNm3 = dtCoApp3.Rows[0]["ContactPersonNm"].ToString();
                        vSelfEmployed3 = dtCoApp3.Rows[0]["SelfEmployed"].ToString();
                        vNatureOfActivity3 = dtCoApp3.Rows[0]["NatureOfActivity"].ToString();
                        vAnnualTurnOver3 = dtCoApp3.Rows[0]["AnnualTurnOver"].ToString();
                        vPEP3 = dtCoApp3.Rows[0]["PEP"].ToString();
                    }

                    #endregion

                    #region DETAILS_OF_CO_APPLICANT_4
                    if (dtCoApp4.Rows.Count > 0)
                    {
                        vCoAppNm4 = dtCoApp4.Rows[0]["CoAppName"].ToString();
                        vRelWithApp4 = dtCoApp4.Rows[0]["RelWithApp"].ToString();
                        vResiLandmark4 = dtCoApp4.Rows[0]["CurrLandmark"].ToString();
                        vResiCity4 = dtCoApp4.Rows[0]["CurrAdd"].ToString();
                        vResiPin4 = dtCoApp4.Rows[0]["CurrPin"].ToString();
                        vResiResidenceType4 = dtCoApp4.Rows[0]["OwnShipStatus"].ToString();
                        vResiYrCurrAdd4 = dtCoApp4.Rows[0]["ResiStabYrs"].ToString();
                        vOfcLandmark4 = dtCoApp4.Rows[0]["OfcLandmark"].ToString();
                        vOfcCity4 = dtCoApp4.Rows[0]["OfcCity"].ToString();
                        vOfcPin4 = dtCoApp4.Rows[0]["OfcPin"].ToString();
                        vPerLandmark4 = dtCoApp4.Rows[0]["PerLandMark"].ToString();
                        vPerCity4 = dtCoApp4.Rows[0]["PerAdd"].ToString();
                        vPerPin4 = dtCoApp4.Rows[0]["PerPin"].ToString();
                        vPerResiType4 = dtCoApp4.Rows[0]["PerResiType"].ToString();
                        vPerYrCurrAdd4 = dtCoApp4.Rows[0]["PerResiYr"].ToString();
                        vSTD4 = dtCoApp4.Rows[0]["STDCODE"].ToString();
                        vLandLine1_4 = dtCoApp4.Rows[0]["TelePh1"].ToString();
                        vLandLine2_4 = dtCoApp4.Rows[0]["TelePh2"].ToString();
                        vMobile1_4 = dtCoApp4.Rows[0]["MobNo"].ToString();
                        vMobile2_4 = dtCoApp4.Rows[0]["MobNo2"].ToString();
                        vEmailAddr4 = dtCoApp4.Rows[0]["Email"].ToString();
                        vDOB4 = dtCoApp4.Rows[0]["DOB"].ToString();
                        vGender4 = dtCoApp4.Rows[0]["Gender"].ToString();
                        vMaritalStatus4 = dtCoApp4.Rows[0]["MaritalStatus"].ToString();
                        vHighestEdu4 = dtCoApp4.Rows[0]["Education"].ToString();
                        vPanNo4 = dtCoApp4.Rows[0]["PanNo"].ToString();
                        vReligion4 = dtCoApp4.Rows[0]["Religion"].ToString();
                        vCaste4 = dtCoApp4.Rows[0]["Caste"].ToString();
                        vSalaried4 = dtCoApp4.Rows[0]["Salaried"].ToString();
                        vContactPersonNm4 = dtCoApp4.Rows[0]["ContactPersonNm"].ToString();
                        vSelfEmployed4 = dtCoApp4.Rows[0]["SelfEmployed"].ToString();
                        vNatureOfActivity4 = dtCoApp4.Rows[0]["NatureOfActivity"].ToString();
                        vAnnualTurnOver4 = dtCoApp4.Rows[0]["AnnualTurnOver"].ToString();
                        vPEP4 = dtCoApp4.Rows[0]["PEP"].ToString();
                    }
                    #endregion

                    #region DETAILS_OF_Guarantor
                    if (dtCoAppG.Rows.Count > 0)
                    {
                        vCoAppNmG = dtCoAppG.Rows[0]["CoAppName"].ToString();
                        vRelWithAppG = dtCoAppG.Rows[0]["RelWithApp"].ToString();
                        vResiLandmarkG = dtCoAppG.Rows[0]["CurrLandmark"].ToString();
                        vResiCityG = dtCoAppG.Rows[0]["CurrAdd"].ToString();
                        vResiPinG = dtCoAppG.Rows[0]["CurrPin"].ToString();
                        vResiResidenceTypeG = dtCoAppG.Rows[0]["OwnShipStatus"].ToString();
                        vResiYrCurrAddG = dtCoAppG.Rows[0]["ResiStabYrs"].ToString();
                        vOfcLandmarkG = dtCoAppG.Rows[0]["OfcLandmark"].ToString();
                        vOfcCityG = dtCoAppG.Rows[0]["OfcCity"].ToString();
                        vOfcPinG = dtCoAppG.Rows[0]["OfcPin"].ToString();
                        vPerLandmarkG = dtCoAppG.Rows[0]["PerLandMark"].ToString();
                        vPerCityG = dtCoAppG.Rows[0]["PerAdd"].ToString();
                        vPerPinG = dtCoAppG.Rows[0]["PerPin"].ToString();
                        vPerResiTypeG = dtCoAppG.Rows[0]["PerResiType"].ToString();
                        vPerYrCurrAddG = dtCoAppG.Rows[0]["PerResiYr"].ToString();
                        vSTDG = dtCoAppG.Rows[0]["STDCODE"].ToString();
                        vLandLine1_G = dtCoAppG.Rows[0]["TelePh1"].ToString();
                        vLandLine2_G = dtCoAppG.Rows[0]["TelePh2"].ToString();
                        vMobile1_G = dtCoAppG.Rows[0]["MobNo"].ToString();
                        vMobile2_G = dtCoAppG.Rows[0]["MobNo2"].ToString();
                        vEmailAddrG = dtCoAppG.Rows[0]["Email"].ToString();
                        vDOBG = dtCoAppG.Rows[0]["DOB"].ToString();
                        vGenderG = dtCoAppG.Rows[0]["Gender"].ToString();
                        vMaritalStatusG = dtCoAppG.Rows[0]["MaritalStatus"].ToString();
                        vHighestEduG = dtCoAppG.Rows[0]["Education"].ToString();
                        vPanNoG = dtCoAppG.Rows[0]["PanNo"].ToString();
                        vReligionG = dtCoAppG.Rows[0]["Religion"].ToString();
                        vCasteG = dtCoAppG.Rows[0]["Caste"].ToString();
                        vSalariedG = dtCoAppG.Rows[0]["Salaried"].ToString();
                        vContactPersonNmG = dtCoAppG.Rows[0]["ContactPersonNm"].ToString();
                        vSelfEmployedG = dtCoAppG.Rows[0]["SelfEmployed"].ToString();
                        vNatureOfActivityG = dtCoAppG.Rows[0]["NatureOfActivity"].ToString();
                        vAnnualTurnOverG = dtCoAppG.Rows[0]["AnnualTurnOver"].ToString();
                        vPEPG = dtCoAppG.Rows[0]["PEP"].ToString();
                    }
                    #endregion

                    #region PURPOSE_OF_LOAN
                    if (dtLoan.Rows.Count > 0)
                    {
                        vBussDesc = dtLoan.Rows[0]["BussDesc"].ToString();
                        vYrInBuss = dtLoan.Rows[0]["YrInBuss"].ToString();
                        vNoOfEmp = dtLoan.Rows[0]["NoOfEmp"].ToString();
                        vInvestmentAmt = dtLoan.Rows[0]["InvestmentAmt"].ToString();
                        vGSTRegYN = dtLoan.Rows[0]["GSTRegYN"].ToString();
                        vGSTNo = dtLoan.Rows[0]["GSTNo"].ToString();
                        vLenderNm = dtLoan.Rows[0]["LenderNm"].ToString();
                        vROI = dtLoan.Rows[0]["ROI"].ToString();
                    }
                    #endregion

                    #region SECURITY
                    if (dtSecurity.Rows.Count > 0)
                    {
                        vSecurityType = dtSecurity.Rows[0]["SecurityType"].ToString();
                        vOwnerNm = dtSecurity.Rows[0]["OwnerNm"].ToString();
                        vOwnershipType = dtSecurity.Rows[0]["OwnershipType"].ToString();
                        vCollateralCity = dtSecurity.Rows[0]["CollateralCity"].ToString();
                        vCollateralState = dtSecurity.Rows[0]["CollateralState"].ToString();
                        vCollateralPin = dtSecurity.Rows[0]["CollateralPin"].ToString();
                        vMarketableValue = dtSecurity.Rows[0]["MarketableValue"].ToString();
                    }
                    #endregion

                    #region KYC
                    if (dtKyc.Rows.Count > 0)
                    {
                        vIdentityProof = dtKyc.Rows[0]["IdentityProof"].ToString();
                        vResidenceProof = dtKyc.Rows[0]["ResidenceProof"].ToString();
                        vIncomeProof = dtKyc.Rows[0]["IncomeProof"].ToString();
                        vOtherDocument = dtKyc.Rows[0]["OtherDocument"].ToString();
                    }
                    #endregion

                    #region Relation_With_bank
                    if (dtBankRel.Rows.Count > 0)
                    {
                        vRelatedToBank1 = dtBankRel.Rows[0]["RelatedToBank1"].ToString();
                        vRelatedToBank2 = dtBankRel.Rows[0]["RelatedToBank2"].ToString();
                    }
                    #endregion

                    #region Temp_DT
                    // START --- Creating a table temporarily to Complete the Design.. This will come from DataBase later
                    //DataTable dtFamily = new DataTable();

                    //dtFamily.Columns.Add("SrNo", typeof(string));
                    //dtFamily.Columns.Add("Name", typeof(string));
                    //dtFamily.Columns.Add("Relationship", typeof(string));
                    //dtFamily.Columns.Add("LoanAmount", typeof(string));
                    //dtFamily.Columns.Add("LoanAccountNumber", typeof(string));
                    //dtFamily.Columns.Add("Remarks", typeof(string));

                    //dtFamily.Rows.Add("1", "Jhon Desai", "Brother", "50,000", "12345", "Active A/c");
                    // END
                    #endregion

                    #region Temp_DT_dtDirectr
                    // START --- Creating a table temporarily to Complete the Design.. This will come from DataBase later
                    //DataTable dtDirectr = new DataTable();

                    //dtDirectr.Columns.Add("SrNo", typeof(string));
                    //dtDirectr.Columns.Add("DirectorNm", typeof(string));
                    //dtDirectr.Columns.Add("DirectorDesig", typeof(string));
                    //dtDirectr.Columns.Add("DirectorRelatn", typeof(string));

                    //dtDirectr.Rows.Add("1", "Tarun", "MD", "Brother");
                    // END
                    #endregion

                    #region CoApp_Flag
                    if (dtFlag.Rows.Count > 0)
                    {
                        vCoApp1 = dtFlag.Rows[0]["CA1Flag"].ToString();
                        vCoApp2 = dtFlag.Rows[0]["CA2Flag"].ToString();
                        vCoApp3 = dtFlag.Rows[0]["CA3Flag"].ToString();
                        vCoApp4 = dtFlag.Rows[0]["CA4Flag"].ToString();
                        vCoAppG = dtFlag.Rows[0]["GFlag"].ToString(); 
                    }
                    #endregion



                    #endregion

                    vCoApp1 = "Y"; vCoApp2 = "Y"; vCoApp3 = "Y"; vCoApp4 = "Y"; vCoAppG = "Y";

                    sb.Append("<table style='width:100%; font-family:Arial, sans-serif; font-size:14px; border-collapse:collapse;'>");

                    // Title
                    sb.Append("<tr><td colspan='2' style='text-align:center; font-weight:bold; font-size:16px; text-decoration: underline;'>CLIMATE FINANCE ROOFTOP SOLAR SYSTEM – APPLICATION FORM</td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:center; font-weight:bold; font-size:16px; padding-bottom:20px;'>Customer Declaration</td></tr>");

                    #region Paragraph
                    // Paragraph
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<ol>");
                    sb.Append("<li>I/we am/are a citizen and tax resident of India.</li>");
                    sb.Append("<li>I/we are a company established in accordance with the laws of India.</li>");
                    sb.Append("<li>I/We have not submitted any application for insolvency, been declared insolvent nor has any insolvency/bankruptcy proceeding been initiated against me/us.</li>");
                    sb.Append("<li>I/We have not defaulted on any repayment of any other loan repayment or any other amount to any lender or creditor and my/our name does not appear on the Reserve Bank of India's (RBI) list of defaulters and Export Credit Guarantee Corporation's (ECGC) caution list or any list/notifications issued by the RBI with respect to anti money laundering or combating financing of terrorism or any sanctions lists published by the United Nations Security Council with respect to terrorist related activities.</li>");
                    sb.Append("<li>I am the true and lawful owner of the assets provided as security.</li>");
                    sb.Append("<li>I/We further undertake to provide any further information/documents that Bank and/or its group companies and/or its agents may require. </li>");
                    sb.Append("<li>I/We understand that the Bank shall have the discretion to change the rate of interest prospectively (in line with any regulatory requirement, and/ or as permitted by any applicable laws and levy charges as applicable to the Loan.</li>");
                    sb.Append("<li>I/We hereby give my/our consent to and agree and authorise the Bank to fetch my personal details from UIDAI. I/We hereby state that I/We have no objection in authenticating myself with Aadhaar based authentication system and I/We voluntarily consent to providing my/our Aadhaar number / VID number, Biometric Information and/or One Time Pin (OTP) data (and/or any similar authentication data) for the purpose of this Loan Application. I/We understand that the Biometric and/or OTP and/or any other authentication data I/We may provide for authentication shall be used only for authenticating my/our identity through the Aadhaar authentication system for the specific transaction or as per requirement of law and for no other purposes (if applicable).</li>");
                    sb.Append("<li>I/We confirm that I/We have been informed about the alternatives to submission of identity information and I/We have agreed to authenticate myself through Aadhaar based authentication system with full understanding of alternatives to submission of identity information.</li>");
                    sb.Append("<li>I/We understand that Unity Small Finance Bank shall ensure security and confidentiality of my/our personal identity data provided for the purpose of Aadhaar based authentication. I/We authorise Unity Small Finance Bank to verify and authenticate my/our Aadhaar during processing of my Loan Application.</li>");
                    sb.Append("<li>I/We further authorise the Bank to share my/our Aadhaar related details/information with regulatory/statutory bodies as and when required (if applicable). </li>");
                    sb.Append("<li>I/We hereby expressly consent to and authorise the Bank (whether acting by itself or through any of the service providers and whether in an automated manner or otherwise to collect, store and process my/our application details, personal data and sensitive information about me/us, information, papers and data relating to know your customer (KYC), credit information, and any other information about me/us /pertaining to me/us or not as may be deemed relevant by Bank ( collectively “Information”) and I/We hereby also expressly consent to and authorise the Bank to download KYC details from the CKYC Registry using my/our CKYC ID for the purpose of climate finance rooftop solar system.</li>");
                    sb.Append("<li>I/We expressly consent Bank to share and disclose the Information to service providers, consultants, credit information companies, information utilities, other banks and financial institutions, affiliates, group companies, subsidiaries, regulators, investigating agencies, judicial, quasi-judicial and statutory authorities, or to other persons/institutions/entities as may be necessary in connection with the contractual or legal requirements or in the legitimate interests of the Bank.</li>");
                    sb.Append("<li>I/We hereby consent the Bank, its service providers, agents and/or affiliatesto process the information including by way of storing, structuring, organising, reproducing, copying, using, profiling, etc as may be deemed fit by the Bank and for the purposes of credit appraisal, fraud detection, anti-money laundering obligations, for entering into contract, for direct marketing, for cross selling, promotion, for developing credit scoring models and business strategies, for monitoring, for evaluation and improving the quality of services and products or for any purposes as the Bank may deem fit.</li>");
                    sb.Append("<li>The Bank shall not discriminate on grounds of sex, caste, and religion in the matter of lending. However, the Applicant(s) understands that this shall not preclude the Bank from participating in credit-linked schemes framed for weaker sections of the society.</li>");
                    sb.Append("<li>I/We declare that all the particulars and information given in the application form are true, correct and complete and no material information has been withheld/suppressed.</li>");
                    sb.Append("<li>I/We shall advise Unity Small Finance Bank Limited (Bank). / Bank in writing of any change in my/ our residential or employment/ business address or any such change which may affect my credit worthiness.</li>");
                    sb.Append("<li>I/We confirm that the funds shall be used for the stated purpose and will not be used for speculative or antisocial purpose.</li>");
                    sb.Append("<li>I/We understand that Bank reserves the right to retain the photographs and documents submitted with this application and will not return the same to me/us.</li>");
                    sb.Append("<li>I/We understand that the sanction of this loan is at the sole discretion of Bank and upon my/our executing necessary security(ies) and other formalities as required by Bank.</li>");

                    // Nested <li>
                    sb.Append("<li>I/We hereby confirm that");
                    sb.Append("<ol type='a'>");
                    sb.Append("<li>where the Borrower is an individual /proprietor(s) none of the Directors of the Bank or their relatives or the relatives of the senior officer of the Bank, is his / her business partner or guarantor or relative;</li>");
                    sb.Append("<li>where the Borrower is a partnership firm or limited liability partnership none of the Directors of the Bank or their relatives or the relatives of the senior officer of the Bank, is interested in the firm as partner, manager, employee or guarantor;</li>");
                    sb.Append("<li>where the Borrower is a company or a corporation, none of the Directors of the Bank or their relatives or the relatives of the senior officer of the Bank, is interested in the company/corporation or in its Subsidiary or Holding Company as director, managing agent, manager, employee or guarantor or holder of substantial interest;</li>");
                    sb.Append("</ol>");
                    sb.Append("</li>");
                    // END Nested <li>

                    // Nested <li>
                    sb.Append("<li>I/we hereby confirm that");
                    sb.Append("<ol type='a'>");
                    sb.Append("<li>where the Borrower is an individual/ proprietor(s) he/she is not a Director of other banks or relative of the director of other banks;</li>");
                    sb.Append("<li>where the Borrower is a partnership firm or limited liability partnership, none of the Directors of other banks or relative of a director of other banks is interested in the firm as partner or guarantor;</li>");
                    sb.Append("<li>where the Borrower is a company or a corporation, none of the directors of other banks or relative of a director of other banks, is interested in the company / corporation as director or guarantor or holder of substantial interest.</li>");
                    sb.Append("</ol>");
                    sb.Append("</li>");
                    // END Nested <li>
                    sb.Append("<li>I/We agree and accept that Bank may in its sole discretion, by itself or through authorised persons, advocate, agencies, credit bureau, etc. verify any information given, check credit references, employment details and obtain credit reports to determine creditworthiness from time to time.</li>");
                    sb.Append("<li>I/We have no objection in sharing my/our Aadhaar details and in authenticating me/us with Aadhaar based authentication system and hereby give my voluntary consent as required under the Aadhaar Act, 2016 and regulations framed thereunder to provide my identity information (Aadhaar number, biometric information & demographic information) for Aadhaar based authentication for the purpose of availing loan from Bank.</li>");
                    sb.Append("<li>I hereby consent to receiving information from Central KYC Registry through SMS/ Email on the above registered number / email address.</li>");
                    sb.Append("<li>I/ We waive the privilege of privacy and privity of contract.</li>");
                    sb.Append("<li>I/ We understand that the tenure / repayment /interest/other terms and conditions of the loan are subject to changes as a consequence to any changes in the money market conditions or on account of any other statutory or regulatory requirements or at Bank’s discretion. Bank reserves that right to review and amend the terms of the loan to such extent as it may deem fit.</li>");
                    sb.Append("<li>I/We understand that the purchase of any insurance products is purely voluntary, and is not linked to availing of any other facility from the Bank.</li>");
                    sb.Append("<li>I/We hereby expressly authorize Bank to send to me/us communications regarding loans, insurance and other products from Bank, its group companies and / or third parties through telephone calls / SMSs / emails / post etc. including but not limited to promotional, transactional communications. I/We confirm that I/We shall not challenge receipt of such communications by me as unsolicited communication, defined under TRAI Regulations on Unsolicited Commercial Communications.</li>");
                    sb.Append("<li>In case any of the above information is found to be false or untrue or misleading or misrepresenting, I am aware that I may be held liable for it.</li>");
                    sb.Append("<li>I/We hereby confirm that the above contents and the terms and conditions that shall be applicable to the Facility have been explained to me in the language understood by me.</li>");
                    sb.Append("<li>I/ We confirm that I/we have read and understood the above Declaration, and that the details provided by me/ us are correct.</li>");
                    sb.Append("<li>I/We confirm that the above Declaration shall be executed by us either through wet signature or mobile OTP mechanism. I/We shall be solely responsible to ensure that the OTP is not compromised or shared with any unauthorized users. All the records of USFB arising out of the use of the OTP shall be binding on the Borrower.</li>");
                    sb.Append("<li>I/We understand and acknowledge that Bank shall have absolute discretion without assigning any reason (unless required by applicable law) to reject my / our application and that Bank shall not be responsible / liable in any manner whatsoever to me / us for such rejection or any delay in notifying me / us of such rejection.</li>");
                    sb.Append("<li>I/We are neither related to any directors of Bank nor I/We are his/her relatives as defined under Companies Act, 2013.</li>");

                    sb.Append("</ol>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/><br/><br/></td></tr>");
                    #endregion

                    #region All_Boxes
                    // All boxes for Applicant n Co-Applicant
                    sb.Append("<tr><td colspan='2' cellspacing='10px' style='padding-top:10px;padding-left:15px;'>");
                    sb.Append("<table style='border-collapse: seperate; width: 100%;'>");
                    sb.Append("<tr>");

                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Applicant");
                    sb.Append("</td>");
                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Co-Applicant 1");
                    sb.Append("</td>");
                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Co-Applicant 2");
                    sb.Append("</td>");
                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Co-Applicant 3");
                    sb.Append("</td>");
                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Co-Applicant 4");
                    sb.Append("</td>");
                    sb.Append("<td style='width: 70px; height: 70px; border: 1px solid black; box-sizing: border-box; overflow: hidden; white-space: nowrap; text-align: center; vertical-align: middle;'>");
                    sb.Append("Guarantor");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/></td></tr>");
                    // Application Form
                    //sb.Append("<tr><td colspan='2' style='width: 70px; height: 70px; border: 1px solid black; >Application Form No</td></tr>");                
                    sb.Append("<tr><td colspan='2'><h3>FILL ALL FIELDS IN CAPITAL LETTERS ONLY</h3></td></tr>");
                    #endregion

                    #region SOURCING_DETAILS
                    // SOURCING DETAILS
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>SOURCING DETAILS</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>Channel</td>");
                    sb.Append("<td style='padding:5px; width:70%;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    //sb.Append("<li>Direct Walkin / Branch Sourcing / Business Correspondents / Inclusive Banking / Marketing Campaign / Call Centre / Customer Referral / Inhouse Calling/ Direct Sales Team</li>");
                    sb.AppendFormat("<li>{0}</li>", vChannel);
                    sb.AppendFormat("<li>Sub Source: {0}</li>", vSubSource);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>EPC</td>");
                    sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vEPC);
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    #endregion

                    #region LOAN_DETAILS
                    // Loan DETAILS
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>LOAN DETAILS</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>Application Date</td>");
                    sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vApplicationDt);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Branch Name</td><td style='padding:5px;'>{0}</td></tr>", vBranchOffice);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Applicant Type</td><td style='padding:5px;'>{0}</td></tr>", vApplicationType);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Loan Type</td><td style='padding:5px;'>{0}</td></tr>", vLoanTerm);
                    sb.AppendFormat("<tr><td style='padding:5px;'>LEI Number</td><td style='padding:5px;'>{0}</td></tr>", vLEINo);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Loan Amount Required</td><td style='padding:5px;'>{0}</td></tr>", vLoanAmtReq);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Repayment Option</td><td style='padding:5px;'>{0}</td></tr>", vRepayOptn);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Loan Tenor</td><td style='padding:5px;'>{0}</td></tr>", vTenor);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Bank Account Details for Disbursal</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Bank Name: {0}</li>", vBankNm);
                    sb.AppendFormat("<li>Branch: {0}</li>", vBankBranch);
                    sb.AppendFormat("<li>Account Holder Name: {0}</li>", vAcHolderNm);
                    sb.AppendFormat("<li>Account Number: {0}</li>", vAcNo);
                    sb.AppendFormat("<li>IFSC Code: {0}</li>", vIFSC);
                    sb.AppendFormat("<li>Account Type: {0}</li>", vAcType);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Bank Account Details for Disbursal
                    sb.Append("</tr>"); // FOR-- Bank Account Details for Disbursal              
                    sb.Append("</table>");
                    sb.Append("</td></tr>"); // -- Created Table within this
                    #endregion

                    #region PERSONAL_DETAILS
                    // PERSONAL DETAILS
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>PERSONAL DETAILS</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>Applicant Name</td>");
                    sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vApplName);

                    sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmark);
                    sb.AppendFormat("<li>City: {0}</li>", vResiCity);
                    sb.AppendFormat("<li>Pin: {0}</li>", vResiPin);
                    sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceType);
                    sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAdd);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Residence Address
                    sb.Append("</tr>"); // FOR-- Residence Address

                    sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmark);
                    sb.AppendFormat("<li>City: {0}</li>", vOfcCity);
                    sb.AppendFormat("<li>Pin: {0}</li>", vOfcPin);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Office Address
                    sb.Append("</tr>"); // FOR-- Office Address

                    sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmark);
                    sb.AppendFormat("<li>City: {0}</li>", vPerCity);
                    sb.AppendFormat("<li>Pin: {0}</li>", vPerPin);
                    sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiType);
                    sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAdd);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Permanent Address
                    sb.Append("</tr>"); // FOR-- Permanent Address

                    sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>STD Code: {0}</li>", vSTD);
                    sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1);
                    sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Telephone No
                    sb.Append("</tr>"); // FOR-- Telephone No

                    sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1);
                    sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Mobile No
                    sb.Append("</tr>"); // FOR-- Mobile No

                    sb.AppendFormat("<tr><td style='padding:5px;'>Email Address</td><td style='padding:5px;'>{0}</td></tr>", vEmailAddr);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOB);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGender);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatus);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEdu);
                    sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNo);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligion);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCaste);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Person with Disability</td><td style='padding:5px;'>{0}</td></tr>", vPersonWithDisability);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Constitution</td><td style='padding:5px;'>{0}</td></tr>", vConstitution);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalaried);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNm);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployed);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivity);

                    sb.AppendFormat("<tr><td style='padding:5px;'>Annual Income/Turnover</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Self: {0}</li>", vSelfIncome);
                    sb.AppendFormat("<li>Family: {0}</li>", vFamilyIncome);
                    sb.AppendFormat("<li>Working Members in Family: {0}</li>", vWorkingMembersInFamily);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Annual Income/Turnover
                    sb.Append("</tr>"); // FOR-- Annual Income/Turnover

                    sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEP);

                    sb.Append("</table>");
                    sb.Append("</td></tr>"); // -- Created Table within this

                    #endregion

                    if (vCoApp1 == "Y")
                    {
                        #region DETAILS_OF_CO_APPLICANT_1
                        // DETAILS OF CO_APPLICANT 1
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>CO-APPLICANT 1 DETAILS (If Any)</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:30%;'>Application Name</td>");
                        sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vCoAppNm1);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>Relationship with the Applicant </td>");
                        sb.AppendFormat("<td style='padding:5px;'>{0}</td>", vRelWithApp1);
                        sb.Append("</tr>");
                        sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmark1);
                        sb.AppendFormat("<li>City: {0}</li>", vResiCity1);
                        sb.AppendFormat("<li>Pin: {0}</li>", vResiPin1);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceType1);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAdd1);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                      
                        sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmark1);
                        sb.AppendFormat("<li>City: {0}</li>", vOfcCity1);
                        sb.AppendFormat("<li>Pin: {0}</li>", vOfcPin1);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmark1);
                        sb.AppendFormat("<li>City: {0}</li>", vPerCity1);
                        sb.AppendFormat("<li>Pin: {0}</li>", vPerPin1);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiType1);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAdd1);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>STD Code: {0}</li>", vSTD1);
                        sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1_1);
                        sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2_1);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1_1);
                        sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2_1);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        
                  
                        sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOB1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGender1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatus1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEdu1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNo1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligion1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCaste1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalaried1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNm1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployed1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivity1);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vAnnualTurnOver1);                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEP1);

                        sb.Append("</table>");
                        sb.Append("</td></tr>"); // -- Created Table within this
                        #endregion
                    }
                    if (vCoApp2 == "Y")
                    {
                        #region DETAILS_OF_CO_APPLICANT_2
                        // DETAILS OF CO_APPLICANT 2
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>CO-APPLICANT 1 DETAILS (If Any)</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:30%;'>Application Name</td>");
                        sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vCoAppNm2);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>Relationship with the Applicant </td>");
                        sb.AppendFormat("<td style='padding:5px;'>{0}</td>", vRelWithApp2);
                        sb.Append("</tr>");
                        sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmark2);
                        sb.AppendFormat("<li>City: {0}</li>", vResiCity2);
                        sb.AppendFormat("<li>Pin: {0}</li>", vResiPin2);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceType2);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAdd2);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmark2);
                        sb.AppendFormat("<li>City: {0}</li>", vOfcCity2);
                        sb.AppendFormat("<li>Pin: {0}</li>", vOfcPin2);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        
                        sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmark2);
                        sb.AppendFormat("<li>City: {0}</li>", vPerCity2);
                        sb.AppendFormat("<li>Pin: {0}</li>", vPerPin2);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiType2);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAdd2);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                        
                        sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>STD Code: {0}</li>", vSTD2);
                        sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1_2);
                        sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2_2);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1_2);
                        sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2_2);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");
                       
                        sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOB2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGender2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatus2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEdu2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNo2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligion2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCaste2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalaried2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNm2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployed2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivity2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vAnnualTurnOver2);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEP2);

                        sb.Append("</table>");
                        sb.Append("</td></tr>"); // -- Created Table within this
                        #endregion
                    }
                    if (vCoApp3 == "Y")
                    {
                        #region DETAILS_OF_CO_APPLICANT_3
                        // DETAILS OF CO_APPLICANT 3
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>CO-APPLICANT 1 DETAILS (If Any)</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:30%;'>Application Name</td>");
                        sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vCoAppNm3);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>Relationship with the Applicant </td>");
                        sb.AppendFormat("<td style='padding:5px;'>{0}</td>", vRelWithApp3);
                        sb.Append("</tr>");
                        sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmark3);
                        sb.AppendFormat("<li>City: {0}</li>", vResiCity3);
                        sb.AppendFormat("<li>Pin: {0}</li>", vResiPin3);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceType3);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAdd3);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmark3);
                        sb.AppendFormat("<li>City: {0}</li>", vOfcCity3);
                        sb.AppendFormat("<li>Pin: {0}</li>", vOfcPin3);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmark3);
                        sb.AppendFormat("<li>City: {0}</li>", vPerCity3);
                        sb.AppendFormat("<li>Pin: {0}</li>", vPerPin3);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiType3);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAdd3);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>STD Code: {0}</li>", vSTD3);
                        sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1_3);
                        sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2_3);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1_3);
                        sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2_3);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOB3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGender3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatus3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEdu3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNo3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligion3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCaste3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalaried3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNm3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployed3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivity3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vAnnualTurnOver3);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEP3);

                        sb.Append("</table>");
                        sb.Append("</td></tr>"); // -- Created Table within this
                        #endregion
                    }
                    if (vCoApp4 == "Y")
                    {
                        #region DETAILS_OF_CO_APPLICANT_4
                        // DETAILS OF CO_APPLICANT 4
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>CO-APPLICANT 1 DETAILS (If Any)</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:30%;'>Application Name</td>");
                        sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vCoAppNm4);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>Relationship with the Applicant </td>");
                        sb.AppendFormat("<td style='padding:5px;'>{0}</td>", vRelWithApp4);
                        sb.Append("</tr>");
                        sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmark4);
                        sb.AppendFormat("<li>City: {0}</li>", vResiCity4);
                        sb.AppendFormat("<li>Pin: {0}</li>", vResiPin4);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceType4);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAdd4);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmark4);
                        sb.AppendFormat("<li>City: {0}</li>", vOfcCity4);
                        sb.AppendFormat("<li>Pin: {0}</li>", vOfcPin4);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmark4);
                        sb.AppendFormat("<li>City: {0}</li>", vPerCity4);
                        sb.AppendFormat("<li>Pin: {0}</li>", vPerPin4);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiType4);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAdd4);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>STD Code: {0}</li>", vSTD4);
                        sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1_4);
                        sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2_4);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1_4);
                        sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2_4);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOB4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGender4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatus4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEdu4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNo4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligion4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCaste4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalaried4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNm4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployed4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivity4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vAnnualTurnOver4);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEP4);

                        sb.Append("</table>");
                        sb.Append("</td></tr>"); // -- Created Table within this
                        #endregion
                    }
                    if (vCoAppG == "Y")
                    {
                        #region DETAILS_OF_Guarantor
                        // DETAILS OF CO_APPLICANT G
                        sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>CO-APPLICANT 1 DETAILS (If Any)</strong></td></tr>");
                        sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                        sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px; width:30%;'>Application Name</td>");
                        sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vCoAppNmG);
                        sb.Append("</tr>");
                        sb.Append("<tr>");
                        sb.Append("<td style='padding:5px;'>Relationship with the Applicant </td>");
                        sb.AppendFormat("<td style='padding:5px;'>{0}</td>", vRelWithAppG);
                        sb.Append("</tr>");
                        sb.AppendFormat("<tr><td style='padding:5px;'>Residence Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vResiLandmarkG);
                        sb.AppendFormat("<li>City: {0}</li>", vResiCityG);
                        sb.AppendFormat("<li>Pin: {0}</li>", vResiPinG);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vResiResidenceTypeG);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vResiYrCurrAddG);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Office Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vOfcLandmarkG);
                        sb.AppendFormat("<li>City: {0}</li>", vOfcCityG);
                        sb.AppendFormat("<li>Pin: {0}</li>", vOfcPinG);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Permanent Address</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Landmark: {0}</li>", vPerLandmarkG);
                        sb.AppendFormat("<li>City: {0}</li>", vPerCityG);
                        sb.AppendFormat("<li>Pin: {0}</li>", vPerPinG);
                        sb.AppendFormat("<li>Residence Type: {0}</li>", vPerResiTypeG);
                        sb.AppendFormat("<li>Years at Current Residence: {0}</li>", vPerYrCurrAddG);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Telephone No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>STD Code: {0}</li>", vSTDG);
                        sb.AppendFormat("<li>Landline 1: {0}</li>", vLandLine1_G);
                        sb.AppendFormat("<li>Landline 2: {0}</li>", vLandLine2_G);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Mobile No</td>");
                        sb.Append("<td style='padding:5px;'>");
                        sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                        sb.AppendFormat("<li>Mobile 1: {0}</li>", vMobile1_G);
                        sb.AppendFormat("<li>Mobile 2: {0}</li>", vMobile2_G);
                        sb.Append("</ul>");
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        sb.AppendFormat("<tr><td style='padding:5px;'>Date of Birth/Date of incorporation</td><td style='padding:5px;'>{0}</td></tr>", vDOBG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Gender</td><td style='padding:5px;'>{0}</td></tr>", vGenderG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Marital Status</td><td style='padding:5px;'>{0}</td></tr>", vMaritalStatusG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Highest Education Qualification</td><td style='padding:5px;'>{0}</td></tr>", vHighestEduG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>PAN No</td><td style='padding:5px;'>{0}</td></tr>", vPanNoG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Religion</td><td style='padding:5px;'>{0}</td></tr>", vReligionG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Caste</td><td style='padding:5px;'>{0}</td></tr>", vCasteG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Salaried</td><td style='padding:5px;'>{0}</td></tr>", vSalariedG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Contact person name</td><td style='padding:5px;'>{0}</td></tr>", vContactPersonNmG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Self Employed</td><td style='padding:5px;'>{0}</td></tr>", vSelfEmployedG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vNatureOfActivityG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Nature of activity</td><td style='padding:5px;'>{0}</td></tr>", vAnnualTurnOverG);
                        sb.AppendFormat("<tr><td style='padding:5px;'>Politically Exposed Person(PEP)</td><td style='padding:5px;'>{0}</td></tr>", vPEPG);

                        sb.Append("</table>");
                        sb.Append("</td></tr>"); // -- Created Table within this
                        #endregion
                    }

                    #region EXISTING_UNITY_LOAN_DETAILS_SELF_FAMILY
                    // EXISTING UNITY LOAN DETAILS – SELF & FAMILY
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>EXISTING UNITY LOAN DETAILS – SELF & FAMILY</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    // Start HTML table
                    sb.Append("<table style='border-collapse: collapse; width: 100%; text-align: left;' border='1' >");
                    // Add header row
                    sb.Append("<tr>");
                    sb.Append("<th style='padding: 8px;'>Sr No</th>");
                    sb.Append("<th style='padding: 8px;'>Name</th>");
                    sb.Append("<th style='padding: 8px;'>Relationship</th>");
                    sb.Append("<th style='padding: 8px;'>Loan Amount (in INR)</th>");
                    sb.Append("<th style='padding: 8px;'>Loan Account Number</th>");
                    sb.Append("<th style='padding: 8px;'>Remarks</th>");
                    sb.Append("</tr>");

                    // Fill rows from DataTable
                    foreach (DataRow row in dtFamily.Rows)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["SrNo"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["Name"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["Relationship"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["LoanAmount"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["LoanAccountNumber"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["Remarks"]);
                        sb.Append("</tr>");
                    }
                    // End table
                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/><br/></td></tr>");
                    // Close main table
                    #endregion

                    #region PURPOSE_OF_LOAN
                    // PURPOSE_OF_LOAN
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>PURPOSE OF LOAN</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.AppendFormat("<tr><td style='padding:5px;width:30%;'>To Purchase and Install a Solar Power System</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Kindly provide Business Description: {0}</li>", vBussDesc);
                    sb.AppendFormat("<li>No of Years in Business: {0}</li>", vYrInBuss);
                    sb.AppendFormat("<li>No of Employees: {0}</li>", vNoOfEmp);
                    sb.AppendFormat("<li>Original Investment Amount in Plant & Machinery/Equipment (Rs in Lacs): {0}</li>", vInvestmentAmt);
                    sb.AppendFormat("<li>GST Registered: {0}</li>", vGSTRegYN);
                    sb.AppendFormat("<li>GSTIN Number: {0}</li>", vGSTNo);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.AppendFormat("<tr><td style='padding:5px;width:30%;'>Repay Loans to Informal Sector</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Name of the Lender: {0}</li>", vLenderNm);
                    sb.AppendFormat("<li>Rate of Interest of Loan: {0}</li>", vROI);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- PURPOSE_OF_LOAN
                    sb.Append("</tr>"); // FOR-- PURPOSE_OF_LOAN              
                    sb.Append("</table>");
                    sb.Append("</td></tr>"); // -- Created Table within this
                    #endregion

                    #region SECURITY
                    // SECURITY
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>SECURITY</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>Type of Security/ Collateral</td>");
                    sb.AppendFormat("<td style='padding:5px; width:70%;'>{0}</td>", vSecurityType);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Owner Name</td><td style='padding:5px;'>{0}</td></tr>", vOwnerNm);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Ownership Type</td><td style='padding:5px;'>{0}</td></tr>", vOwnershipType);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Address of Collateral</td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>City: {0}</li>", vCollateralCity);
                    sb.AppendFormat("<li>State: {0}</li>", vCollateralState);
                    sb.AppendFormat("<li>Pin: {0}</li>", vCollateralPin);
                    sb.AppendFormat("<tr><td style='padding:5px;'>Marketable Value</td><td style='padding:5px;'>{0}</td></tr>", vMarketableValue);
                    sb.Append("</ul>");
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    sb.Append("</td>"); // FOR-- Bank Account Details for Disbursal
                    sb.Append("</tr>"); // FOR-- Bank Account Details for Disbursal              
                    sb.Append("</table>");
                    sb.Append("</td></tr>"); // -- Created Table within this
                    #endregion

                    #region DETAILS_OF_THE_KNOW_YOUR_CUSTOMER_(KYC)_DOCUMENTS_SUBMITTED
                    // DETAILS_OF_THE_KNOW_YOUR_CUSTOMER_(KYC)_DOCUMENTS_SUBMITTED
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>DETAILS OF THE KNOW YOUR CUSTOMER (KYC) DOCUMENTS SUBMITTED</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    // Start HTML table
                    sb.Append("<table style='border-collapse: collapse; width: 100%; text-align: left;' border='1' >");
                    // Add header row
                    sb.Append("<tr>");
                    sb.Append("<th style='padding: 8px;'>TYPE OF PROOF</th>");
                    sb.Append("<th style='padding: 8px;'>DOCUMENT THAT CAN BE SUBMITTED (Any one in each category)</th>");
                    sb.Append("<th style='padding: 8px;'>DOCUMENT SUBMITTED</th>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;'>Identity Proof</td>");
                    sb.Append("<td style='padding:8px;'>Identity Proof & Address Proof of the Borrower and all the Co-Borrowers in line with the KYC policy</td>");
                    sb.AppendFormat("<td style='padding:8px;'>{0}</td>", vIdentityProof);
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;'>Property (residence or office) ownership proof (if applicable)</td>");
                    sb.Append("<td style='padding:8px;'>Identity Proof & Address Proof of the Borrower and all the Co-Borrowers in line with the KYC policy</td>");
                    sb.AppendFormat("<td style='padding:8px;'>{0}</td>", vResidenceProof);
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;'>Income Proof (Latest documents)</td>");
                    sb.Append("<td style='padding:8px;'>Last 6 months bank statement of main operative business account (if applicable)</td>");
                    sb.AppendFormat("<td style='padding:8px;'>{0}</td>", vIncomeProof);
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;'>Other Documents</td>");
                    sb.Append("<td style='padding:8px;'>Identity Proof & Address Proof of the Borrower and all the Co-Borrowers in line with the KYC policy</td>");
                    sb.AppendFormat("<td style='padding:8px;'>{0}</td>", vOtherDocument);
                    sb.Append("</tr>");


                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/><br/></td></tr>");
                    // Close main table
                    #endregion

                    #region END_USE_DECLARATION
                    // END USE DECLARATION
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>END USE DECLARATION</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    sb.Append("<ol>");
                    sb.Append("<li>I/We declare that I/we shall use the Loan only for the purpose stated in this form and not for any other purpose.</li>");
                    sb.Append("<li>I/We shall indemnify the Bank in the event of any loss or damage incurred by it, arising out of or in connection with false /incorrect declaration /misrepresentation of information by me.</li>");
                    sb.Append("<li>I/We declare that I/We will not utilise the borrowed money for acquisition of small savings instruments, investment in capital markets instruments or in another scheme of mutual fund or for speculative purposes in silver, bullion, essential commodities, property rate arbitrage, etc. In case it is observed that the borrowed money is utilised for any other purpose other than the purpose for which the money has been availed for, the Bank at its sole discretion would recall the loan and take appropriate action to close the loan and initiate legal proceedings against me/us.</li>");
                    sb.Append("</ol>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/></td></tr>");
                    #endregion

                    #region APPLICANT(S)_DECLARATION
                    // APPLICANT(S) DECLARATION 
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>APPLICANT(S) DECLARATION IN RESPECT OF RELATIONSHIP WITH DIRECTOR/SENIOR MANAGEMENT OF THE BANK/ OFFICER OF THE BANK / ANY OTHER BANK</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    // Start HTML table
                    sb.Append("<table style='border-collapse: collapse; width: 100%; text-align: left;' border='1' >");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;width:15%;'>I</td>");
                    sb.Append("<td style='padding:8px;width:60%;'>Are you a Director/Senior Officer or related to a Director/Senior Officer of the Bank as defined by the extant guidelines of RBI from time to time, of Unity Small Finance Bank or any other Bank.</td>");
                    sb.AppendFormat("<td style='padding:8px;width:25%;'>{0}</td>", vRelatedToBank1);
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:8px;'>II</td>");
                    sb.Append("<td style='padding:8px;'>Are you a Director of Subsidiaries/Trustee of Mutual Funds/Venture Capital Fund setup by any bank.</td>");
                    sb.AppendFormat("<td style='padding:8px;'>{0}</td>", vRelatedToBank2);
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/><br/></td></tr>");
                    // Close main table
                    #endregion

                    #region RELATED_TO_BANK
                    // ERELATED_TO_BANK
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>I/We declare(s) that the I/We am/are related to the Director’(s) and / or Senior Officer(s) of Unity Small Finance Bank or of any other banks as specified hereto:</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>");
                    // Start HTML table
                    sb.Append("<table style='border-collapse: collapse; width: 100%; text-align: left;' border='1'>");
                    // Add header row
                    sb.Append("<tr>");
                    sb.Append("<th style='padding: 8px;'>Sr No</th>");
                    sb.Append("<th style='padding: 8px;'>Name of Director(s) / Senior Officers</th>");
                    sb.Append("<th style='padding: 8px;'>Designation</th>");
                    sb.Append("<th style='padding: 8px;'>Relationship</th>");
                    sb.Append("</tr>");

                    // Fill rows from DataTable
                    foreach (DataRow row in dtDirectr.Rows)
                    {
                        sb.Append("<tr>");
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["SrNo"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["DirectorNm"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["DirectorDesig"]);
                        sb.AppendFormat("<td style='padding:8px;'>{0}</td>", row["DirectorRelatn"]);
                        sb.Append("</tr>");
                    }
                    // End table
                    sb.Append("</table>");
                    sb.Append("</td></tr>");
                    sb.Append("<tr><td><br/><br/></td></tr>");
                    // Close main table
                    #endregion

                    #region PARAGRAPH
                    // END USE DECLARATION
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>In case if any of the above stated declarations are breached during the tenor of the facility, the Applicant(s) shall inform the Bank immediately. In case of non-compliance with the undertaking or giving wrong undertaking in relation to the provisions connected lending/section 20 of the Banking Regulation Act, 1949, at any time during the currency of the loan, the Bank reserves the right to recall the loan immediately. I/We acknowledge that the Bank shall have no liability for any consequences arising out of any erroneous details provided by me for which I shall be solely liable. I/We authorise the Bank to carry out such credit checks and at such time as it may deem necessary.</td></tr>");
                    sb.Append("<tr><td><br/><br/><br/>");
                    #endregion

                    #region SCHEDULE_OF_CHARGES_PAYABLE_BY_THE_APPLICANT
                    // SCHEDULE OF CHARGES PAYABLE BY THE APPLICANT
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><strong>SCHEDULE OF CHARGES PAYABLE BY THE APPLICANT:</strong></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:10px;'>"); // OPEN TR TD--1
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>"); //OPEN TABLE--1
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px; width:30%;'>Description</td>");
                    sb.Append("<td style='padding:5px; width:70%;'>Charges</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>"); // 00
                    sb.Append("<td style='padding:5px; width:30%;'>Login Fees</td>");
                    sb.Append("<td style='padding:5px; width:70%;'>"); // OPEN TD -- 2
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>"); // OPEN TABLE - 2
                    sb.Append("<tr>");
                    sb.Append("<td style='width: 50%; vertical-align: middle;'>Loan Amount upto Rs. 50 lakhs</td>");
                    sb.Append("<td style='width: 50%; vertical-align: middle;'>Rs. 5000/- + Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width: 50%; vertical-align: middle;'>Loan Amount Rs. 50 lakhs and above</td>");
                    sb.Append("<td style='width: 50%; vertical-align: middle;'>Rs. 8000/- + Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");  // close TABLE - 2
                    sb.Append("<ol>");
                    sb.AppendFormat("<li>For additional property – Rs. 4000/- + Taxes (per property)</li>");
                    sb.AppendFormat("<li>If the actual value exceeds, in that case, the differential to be collected prior to sanction</li>");
                    sb.Append("</ol>");
                    sb.Append("</td>"); // close TD -- 2
                    sb.Append("</tr>"); // 00
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Processing Fees</td>");
                    sb.Append("<td style='padding:5px;'>Upto 5 % + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Cheque Bounce Charges</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 500 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Cheque Swap Charges</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 1500 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Property Swap Charges (If applicable)</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 15000 +Applicable Tax + Legal and Valuation charges as per actuals.</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Escrow account opening charges</td>");
                    sb.Append("<td style='padding:5px;'>As per Actual</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ol>");
                    sb.AppendFormat("<li>Legal, Repossession & Incidental charges.</li>");
                    sb.AppendFormat("<li>Legal Documentation Charges</li>");
                    sb.Append("</ol>");
                    sb.Append("</td>");
                    sb.Append("<td style='padding:5px;'>As per Actual</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>ROC Charges</td>");
                    sb.Append("<td style='padding:5px;'>As per Actual</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Repossession & Storage charges of Assets Financed</td>");
                    sb.Append("<td style='padding:5px;'>As per Actual</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Documentation Charges(if applicable)</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 1500 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Stamp Duty Charges</td>");
                    sb.Append("<td style='padding:5px;'>Payable as per actuals by the customer</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Stamp Duty Charges</td>");
                    sb.Append("<td style='padding:5px;'>Payable as per actuals by the customer</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Penal Charges</td>");
                    sb.Append("<td style='padding:5px;'>For default or delay in the payment of dues including principal, interest, cost, charges, taxes, expenses, payable to the Bank - 36% per annum + taxes (3% per month + taxes)</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Loan reschedule charges in terms of EMI/Tenure/ROI</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 2000 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Statement of Account Charges</td>");
                    sb.Append("<td>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Once in a year will not be chargeable.</li>");
                    sb.AppendFormat("<li>Rs. 500 + Applicable Tax in case if an extra copy required.</li>");
                    sb.Append("</ul>");
                    sb.Append("</td>");

                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Duplicate List of Documents Charges</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 1500 + Applicable Taxr</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>Document Retrieval Charges (If applicable)</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 1000 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>CERSAI Charges</td>");
                    sb.Append("<td style='padding:5px;'>Rs. 500 + Applicable Tax</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<ul style='list-style: none; padding: 0; margin: 0;'>");
                    sb.AppendFormat("<li>Part Prepayment and Foreclosure charges:</li>");
                    sb.AppendFormat("<li>Note: - For Mortgages Part prepayment Charge will not be payable where borrower is an Individual and ROI is Floating.</li>");
                    sb.Append("</ul>");
                    sb.Append("</td>");

                    sb.Append("<td style='padding:5px;'>");

                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>"); // OPEN TABLE - 2
                    sb.Append("<tr>");
                    sb.Append("<td style='width: 50%;vertical-align: top;'>Months on Board (MOB)</td>");
                    sb.Append("<td style='width: 50%;vertical-align: middle;'>Charges (excluding taxes)");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width: 50%;vertical-align: middle;'>If Closed using own funds</td>");
                    sb.Append("<td style='width: 50%;vertical-align: middle;'>In case of BT to other financer</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");

                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td style='padding:5px;'></td>");
                    sb.Append("<td style='padding:5px;'>");
                    sb.Append("<table style='width:100%; border-collapse:collapse;' border='1'>");
                    sb.Append("<tr>");
                    sb.Append("<td style='width: 35%;vertical-align: top;'>0-12 Months</td>");
                    sb.Append("<td style='width: 50%;vertical-align: top;'>6% of the POS</td>");
                    sb.Append("<td style='width: 35%;vertical-align: top;'>No Foreclosure allowed</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align: top;'>Post 12 months till 36 Months</td>");
                    sb.Append("<td style='vertical-align: top;'>5% of the POS</td>");
                    sb.Append("<td style='vertical-align: top;'>6% of the POS</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr>");
                    sb.Append("<td style='vertical-align: top;'>Post 36 Months</td>");
                    sb.Append("<td style='vertical-align: top;'>4% of the POS</td>");
                    sb.Append("<td style='vertical-align: top;'>5% of the POS</td>");
                    sb.Append("</tr>");
                    sb.Append("</table>");
                    sb.Append("</td>");
                    sb.Append("</tr>");


                    sb.Append("</table>"); //CLOSE TABLE--1
                    sb.Append("</td></tr>"); // CLOSE TR TD--1
                    #endregion

                    #region Conclusion
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><i>All the above mentioned charges are Non-Refundable.</i></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><i>Taxes as applicable</i></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><br /><br /><br /><br /></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Applicant Signature:</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Date:</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Place:</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><br /></td></tr>");
                    sb.Append("<tr><td colspan='2' style='text-align:center; font-weight:bold; font-size:16px; padding-bottom:20px;'>Acknowledgement for receipt of Application Form</td></tr>");
                    sb.AppendFormat("<tr><td style='padding-top:5px;'>Application Form No:</td><td style='padding-top:5px;'>{0}</td></tr>", vApplFormNo);
                    sb.AppendFormat("<tr><td style='padding-top:5px;'>Date of Application:</td><td style='padding-top:5px;'>{0}</td></tr>", vApplDt);
                    sb.AppendFormat("<tr><td style='padding-top:5px;'>Loan Amount:</td><td style='padding-top:5px;text-align:left;'>{0}</td></tr>", vLoanAmt);
                    sb.AppendFormat("<tr><td style='padding-top:5px;'>Product:</td><td style='padding-top:5px;'>{0}</td></tr>", vProduct);
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><br /><br /><br /></td></tr>");


                    sb.AppendFormat("<tr><td colspan='2' style='padding-top:20px;'>We acknowledge the receipt of your loan application/request in the name of{0}</td></tr>", vApplName);
                    sb.AppendFormat("<tr><td colspan='2' style='padding-top:20px;'>We confirm having received upfront login fee (Non-Refundable) of, favouring 'Unity Small Finance Bank Limited' via Online transfer no. drawn on from Mr/Mrs./M/s.{0}</td></tr>", vApplName);
                    sb.AppendFormat("<tr><td colspan='2' style='padding-top:20px;'>We shall process the loan request within 21 working days (14 working days in case of loan amount less than 25 lacs) subsequent to and subject to receiving of all required information and at the sole discretion of the Bank.</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Terms and conditions apply.</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'><br /><br /></td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Applicant Signature:</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Place:</td></tr>");
                    sb.Append("<tr><td colspan='2' style='padding-top:20px;'>Date:</td></tr>");

                    #endregion

                    sb.Append("</table>"); // CLOSE MAIN TABLE

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                }
            }
            Literal1.Text = sb.ToString();
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "mywindow", "printform();", true);
        }
    }
}