using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CGenParameter
    {
        public DataTable GetLedgerByAcHeadId(string pAcType, Int32 pAcHeadId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLedgerByAcHeadId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar,1, "@pAcType", pAcType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAcHeadId", pAcHeadId);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataTable GetParameterDetails(Int32 pLoanTypeId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetParameterDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 6, "@pLoanTypeId", pLoanTypeId);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 InsertLnParameter(Int32 pLoanTypeId, string pApplyMonatorium, string pPaySchedule, string pInstType,
        double pProcFeeAmt,
        string pLoanAC, string pInstAC, string pPenalChrgeAC, string pProcAC, string pWriteOffAC,
        Int32 pCreatedBy,
        string pPST, string pIST, int pMaxInstallNo, int pMinInstallNo, double pMaxInstRate, double pMinInstRate,
        double pMaxInstRateFlat, string pApplyBulletPaymnt, string pBounceChrgAC, string pBounceChrgWaveAC, string pPreCloseChrgAC,
        string pPreCloseWaiveAC, string pCGSTAC, string pSGSTAC, string pWriteOffRecAC,
        double pEffRedIntRate,
        string pAppLegVerProYN, string pCERSAIAC, string pExcessChargeAC,
        string pODIntRec, string pSusIntInc, string pAdvAC, string pIntAccruedAc, double pPenCharges, double pBounceCharges, string pBounceChargesGSTType,
        double pVisitCharges, string pVisitChargesGSTType, double pMinAmtReschudle, double pMaxAdvAmt,

        string pAssInsu, string pAssInsuCGSTAC, string pAssInsuSGSTAC, string pLifeInsAC, string pLifeInsCGST, string pLifeInsSGST, string pDocCh,
        string pDocCGSTCh, string pDocSGSTCh, string pLoginFee, string pLoginFeeCGST, string pLoginFeeSGST,
        string pPreEMI, string pPreEMICGST, string pPreEMISGST, string pStampDuty, string pStampDutyCGST,
        string pStampDutySGST, string pOthrCh, string pOthrChCGST, string pOthrChSGST,
        string pVisitCh, string pVisitCGST, string pVisitSGST, string pServiceTaxAC)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLnParameter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplyMonatorium", pApplyMonatorium);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pProcFeeAmt", pProcFeeAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoanAC", pLoanAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInstAC", pInstAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pProcAC", pProcAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPenalChrgeAC", pPenalChrgeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffAC ", pWriteOffAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPST", pPST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIST", pIST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMaxInstallNo", pMaxInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMinInstallNo", pMinInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxInstRate", pMaxInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMinInstRate", pMinInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxInstRateFlat", pMaxInstRateFlat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplyBulletPaymnt", pApplyBulletPaymnt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBounceChrgAC", pBounceChrgAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBounceChrgWaveAC", pBounceChrgWaveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreCloseChrgAC ", pPreCloseChrgAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreCloseChrgWaiveAC ", pPreCloseWaiveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCGSTAC", pCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSGSTAC", pSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffRecAC", pWriteOffRecAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEffRedIntRate", pEffRedIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppLegVerProYN", pAppLegVerProYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCERSAIAC", pCERSAIAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pExcessChargeAC", pExcessChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pODIntRec", pODIntRec);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSusIntInc", pSusIntInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAdvAC", pAdvAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntAccruedAc", pIntAccruedAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pPenCharges", pPenCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pBounceCharges", pBounceCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBounceChargesGSTType", pBounceChargesGSTType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pVisitCharges", pVisitCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pVisitChargesGSTType", pVisitChargesGSTType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMinAmtReschudle", pMinAmtReschudle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxAdvAmt", pMaxAdvAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsu", pAssInsu);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsuCGSTAC", pAssInsuCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsuSGSTAC", pAssInsuSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsAC", pLifeInsAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsCGST", pLifeInsCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsSGST", pLifeInsSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocCh", pDocCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocCGSTCh", pDocCGSTCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocSGSTCh", pDocSGSTCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFee", pLoginFee);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFeeCGST", pLoginFeeCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFeeSGST", pLoginFeeSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMI", pPreEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMICGST", pPreEMICGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMISGST", pPreEMISGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDuty", pStampDuty);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDutyCGST", pStampDutyCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDutySGST", pStampDutySGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrCh", pOthrCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrChCGST", pOthrChCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrChSGST", pOthrChSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitCh", pVisitCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitCGST", pVisitCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitSGST", pVisitSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pServiceTaxAC", pServiceTaxAC);


                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pFunderId = Convert.ToInt32(oCmd.Parameters["@pFunderId"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 UpdateParameter(Int32 pLoanTypeId, string pApplyMonatorium, string pPaySchedule, string pInstType,
        double pProcFeeAmt,
        string pLoanAC, string pInstAC, string pPenalChrgeAC, string pProcAC, string pWriteOffAC,
        Int32 pCreatedBy,
        string pPST, string pIST, int pMaxInstallNo, int pMinInstallNo, double pMaxInstRate, double pMinInstRate,
        double pMaxInstRateFlat, string pApplyBulletPaymnt, string pBounceChrgAC, string pBounceChrgWaveAC, string pPreCloseChrgAC,
        string pPreCloseWaiveAC, string pCGSTAC, string pSGSTAC, string pWriteOffRecAC,
        double pEffRedIntRate,
        string pAppLegVerProYN, string pCERSAIAC, string pExcessChargeAC,
        string pODIntRec, string pSusIntInc, string pAdvAC, string pIntAccruedAc, double pPenCharges, double pBounceCharges, string pBounceChargesGSTType,
        double pVisitCharges, string pVisitChargesGSTType, double pMinAmtReschudle, double pMaxAdvAmt,

        string pAssInsu, string pAssInsuCGSTAC, string pAssInsuSGSTAC, string pLifeInsAC, string pLifeInsCGST, string pLifeInsSGST, string pDocCh,
        string pDocCGSTCh, string pDocSGSTCh, string pLoginFee, string pLoginFeeCGST, string pLoginFeeSGST,
        string pPreEMI, string pPreEMICGST, string pPreEMISGST, string pStampDuty, string pStampDutyCGST,
        string pStampDutySGST, string pOthrCh, string pOthrChCGST, string pOthrChSGST,
        string pVisitCh, string pVisitCGST, string pVisitSGST, string pServiceTaxAC)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateParameter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplyMonatorium", pApplyMonatorium);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pProcFeeAmt", pProcFeeAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoanAC", pLoanAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInstAC", pInstAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pProcAC", pProcAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPenalChrgeAC", pPenalChrgeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffAC ", pWriteOffAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPST", pPST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIST", pIST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMaxInstallNo", pMaxInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMinInstallNo", pMinInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxInstRate", pMaxInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMinInstRate", pMinInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxInstRateFlat", pMaxInstRateFlat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplyBulletPaymnt", pApplyBulletPaymnt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBounceChrgAC", pBounceChrgAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBounceChrgWaveAC", pBounceChrgWaveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreCloseChrgAC ", pPreCloseChrgAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreCloseChrgWaiveAC ", pPreCloseWaiveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCGSTAC", pCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSGSTAC", pSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffRecAC", pWriteOffRecAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEffRedIntRate", pEffRedIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppLegVerProYN", pAppLegVerProYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCERSAIAC", pCERSAIAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pExcessChargeAC", pExcessChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pODIntRec", pODIntRec);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSusIntInc", pSusIntInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAdvAC", pAdvAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntAccruedAc", pIntAccruedAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pPenCharges", pPenCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pBounceCharges", pBounceCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBounceChargesGSTType", pBounceChargesGSTType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pVisitCharges", pVisitCharges);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pVisitChargesGSTType", pVisitChargesGSTType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMinAmtReschudle", pMinAmtReschudle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pMaxAdvAmt", pMaxAdvAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsu", pAssInsu);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsuCGSTAC", pAssInsuCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAssInsuSGSTAC", pAssInsuSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsAC", pLifeInsAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsCGST", pLifeInsCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLifeInsSGST", pLifeInsSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocCh", pDocCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocCGSTCh", pDocCGSTCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDocSGSTCh", pDocSGSTCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFee", pLoginFee);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFeeCGST", pLoginFeeCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLoginFeeSGST", pLoginFeeSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMI", pPreEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMICGST", pPreEMICGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreEMISGST", pPreEMISGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDuty", pStampDuty);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDutyCGST", pStampDutyCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStampDutySGST", pStampDutySGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrCh", pOthrCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrChCGST", pOthrChCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pOthrChSGST", pOthrChSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitCh", pVisitCh);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitCGST", pVisitCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitSGST", pVisitSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pServiceTaxAC", pServiceTaxAC);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }



        public Int32 DeleteParameter(Int32 pLoanTypeId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteParameter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        public Int32 ChkDelete(Int32 pID, string pField, string pTbl)
    {
        SqlCommand oCmd = new SqlCommand();
        Int32 vRec = 0;
        try
        {
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "ChkDelete";
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pID", pID);// ID Value
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pField", pField);// Field Name
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pTbl", pTbl);// Table Name
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);// ID Value
            DBUtility.Execute(oCmd);
            vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
            if (vRec == 0)
                return 0;
            else
                return 1;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            oCmd.Dispose();
        }
    }

        public DataTable GetActiveLnSchemePG()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetActiveLnSchemePG";
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataTable GetGeneralParameterList()
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGeneralParameterList";
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataTable GetGeneralParameterById(int pId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGeneralParameterById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pId", pId);
                DBUtility.ExecuteForSelect(oCmd, dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public Int32 SaveGeneralParameter(ref Int32 pNewId, Int32 pId, DateTime pEffectDate, decimal pTotalLoginFees, decimal pNeLogInFees, decimal pCGSTAmt,
         decimal pSGSTAmt, decimal pIGSTAmt, string pLogInFeesAC, string pCGSTAC, string pSGSTAC, string pIGSTAC, Int32 pCreatedBy, string pMode,
         string pPrePayAC, string pInsufFundsAC, string pLoanStatAC, string pNOCInsuAC, string pLatePayAC, string pVisitAC, decimal pPremature)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveGeneralParameter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pNewId", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pId", pId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pEffectDate", pEffectDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalLoginFees", pTotalLoginFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetLogInFees", pNeLogInFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIGSTAmt", pIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLogInFeesAC", pLogInFeesAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCGSTAC", pCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pSGSTAC", pSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pIGSTAC", pIGSTAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPrePayAC", pPrePayAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pInsufFundsAC", pInsufFundsAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLoanStatAC", pLoanStatAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pNOCInsuAC", pNOCInsuAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLatePayAC", pLatePayAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pVisitAC", pVisitAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPremature", pPremature);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                pNewId = Convert.ToInt32(oCmd.Parameters["@pNewId"].Value);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

    }
}
