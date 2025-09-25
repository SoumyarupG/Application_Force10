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
        public DataTable GetParameterByLoanID(string pLoanId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetParameterByLoanID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
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
        public DataTable GetPayScheduleById(Int32 pLoanTypeId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPayScheduleById";
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
            string pLoanAC, string pInstAC, string pPenalChrgeAC, string pIntWaveAC, string pPLFKKTaxAC, string pLPFSBTaxAC,
            string pApplChargeAC, string pProcAC, string pInsureAC, string pWriteOffAC,
            string pServiceTaxAC, string pStmpChargeAC, string pXmlMonatorium, Int32 pCreatedBy,
            string pPST, string pIST, string pInsSTaxAC, int pMaxInstallNo, int pMinInstallNo, double pMaxInstRate, double pMinInstRate,
            double pMaxInstRateFlat, string pApplyBulletPaymnt, string pBounceChrgAC, string pBounceChrgWaveAC, string pPreCloseChrgAC,
            string pPreCloseWaiveAC, string pCGSTAC, string pSGSTAC, string pFLDGAC, string pWriteOffRecAC,string pBrokenPeriodIntAC,
            string pIGSTAC, double pEffRedIntRate, string pAdminFeesAC, string pTechFeesAC, string pPropInsuAC, string pVisitChargeAC,
            string pAppLegVerProYN, string pCERSAIAC, string pIntDueAC, string pExcessChargeAC, string pInsuCGSTAC, string pInsuSGSTAC,
            string pODIntRec, string pSusIntInc, string pAdvAC, string pIntAccruedAc, double pPenCharges, double pBounceCharges, string pBounceChargesGSTType,
            double pVisitCharges, string pVisitChargesGSTType, double pMinAmtReschudle, double pMaxAdvAmt, string pPenalChargesGSTType)
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pApplChargeAC", pApplChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pProcAC", pProcAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsureAC", pInsureAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPenalChrgeAC", pPenalChrgeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntWaveAC", pIntWaveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPLFKKTaxAC", pPLFKKTaxAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLPFSBTaxAC", pLPFSBTaxAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffAC ", pWriteOffAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pServiceTaxAC ", pServiceTaxAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStmpChargeAC ", pStmpChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlMonatorium.Length + 2, "@pXmlMonatorium", pXmlMonatorium);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPST", pPST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIST", pIST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsSTaxAC", pInsSTaxAC);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pFLDGAC", pFLDGAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffRecAC", pWriteOffRecAC);
                
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBrkPrdIntAc", pBrokenPeriodIntAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIGSTAC", pIGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEffRedIntRate", pEffRedIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAdminFeesAC", pAdminFeesAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pTechFeesAC", pTechFeesAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPropInsureAC", pPropInsuAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitChargeAC", pVisitChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppLegVerProYN", pAppLegVerProYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCERSAIAC", pCERSAIAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntDueAC", pIntDueAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pExcessChargeAC", pExcessChargeAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsuCGSTAC", pInsuCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsuSGSTAC", pInsuSGSTAC);

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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPenalChargesGSTType", pPenalChargesGSTType);
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
            string pLoanAC, string pInstAC, string pPenalChrgeAC, string pIntWaveAC, string pPLFKKTaxAC, string pLPFSBTaxAC,
            string pApplChargeAC, string pProcAC, string pInsureAC, string pWriteOffAC,
            string pServiceTaxAC, string pStmpChargeAC, string pXmlMonatorium, Int32 pCreatedBy,
            string pPST, string pIST, string pInsSTaxAC, int pMaxInstallNo, int pMinInstallNo, double pMaxInstRate, double pMinInstRate,
            double pMaxInstRateFlat, string pApplyBulletPaymnt, string pBounceChrgAC, string pBounceChrgWaveAC, string pPreCloseChrgAC,
            string pPreCloseWaiveAC, string pCGSTAC, string pSGSTAC, string pFLDGAC, string pWriteOffRecAC, string pBrokenPeriodIntAC,
            string pIGSTAC, double pEffRedIntRate, string pAdminFeesAC, string pTechFeesAC, string pPropInsuAC, string pVisitChargeAC,
            string pAppLegVerProYN, string pCERSAIAC, string pIntDueAC, string pExcessChargeAC, string pInsuCGSTAC, string pInsuSGSTAC,
            string pODIntRec, string pSusIntInc, string pAdvAC, string pIntAccruedAc, double pPenCharges, double pBounceCharges, string pBounceChargesGSTType,
            double pVisitCharges, string pVisitChargesGSTType, double pMinAmtReschudle, double pMaxAdvAmt, string pPenalChargesGSTType)
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pApplChargeAC", pApplChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pProcAC", pProcAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsureAC", pInsureAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPenalChrgeAC", pPenalChrgeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntWaveAC", pIntWaveAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPLFKKTaxAC", pPLFKKTaxAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLPFSBTaxAC", pLPFSBTaxAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffAC ", pWriteOffAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pServiceTaxAC ", pServiceTaxAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pStmpChargeAC ", pStmpChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlMonatorium.Length + 2, "@pXmlMonatorium", pXmlMonatorium);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPST", pPST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIST", pIST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsSTaxAC", pInsSTaxAC);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCGSTAC ", pCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSGSTAC ", pSGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pFLDGAC", pFLDGAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pWriteOffRecAC", pWriteOffRecAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBrkPrdIntAc", pBrokenPeriodIntAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIGSTAC", pIGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEffRedIntRate", pEffRedIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pAdminFeesAC", pAdminFeesAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pTechFeesAC", pTechFeesAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPropInsureAC", pPropInsuAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pVisitChargeAC", pVisitChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppLegVerProYN", pAppLegVerProYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pCERSAIAC", pCERSAIAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIntDueAC", pIntDueAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pExcessChargeAC", pExcessChargeAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsuCGSTAC", pInsuCGSTAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pInsuSGSTAC", pInsuSGSTAC);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPenalChargesGSTType", pPenalChargesGSTType);
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
	public DataTable GetLoanAppByLoanTypeId(Int32 pLoanTypeId)
	{
		SqlCommand oCmd = new SqlCommand();
		DataTable dt = new DataTable();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetLoanAppByLoanTypeId";
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
    public DataTable GetInsuScheme()
    {
        SqlCommand oCmd = new SqlCommand();
        DataTable dt = new DataTable();
        try
        {
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "getInsuSchm";
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
    public decimal GetIRRFromFlatInterest(decimal pNoOfInst, decimal pFlatRate, decimal pLoanAmt, decimal pEMIAmt, ref decimal pReduceRate)
    {
        SqlCommand oCmd = new SqlCommand();
        try
        {
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "GetIRRFromFlatInterest";
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 4, "@nper", pNoOfInst);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 4, "@fRate", pFlatRate);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 4, "@pv", pLoanAmt);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 4, "@pInstAmt", 0);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Float, 4, "@NewRate", pReduceRate);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Float, 4, "@EMIAmt", pEMIAmt);
            DBUtility.Execute(oCmd);

            pReduceRate = Convert.ToDecimal(oCmd.Parameters["@NewRate"].Value);
            pEMIAmt = Convert.ToDecimal(oCmd.Parameters["@EMIAmt"].Value);
            return pEMIAmt;
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
