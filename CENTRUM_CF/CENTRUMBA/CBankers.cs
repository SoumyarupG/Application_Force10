using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CBankers
    {
        public Int32 InsertBankers(ref Int32 pICId, string pICName, string pAddress, string pContPerson1, string pContNo1, string pContPerson2,
                                                string pContNo2, Int32 pCreatedBy, string pDescid, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBankers";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pICId", pICId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pICName", pICName.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddress", pAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pContPerson1", pContPerson1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pContNo1", pContNo1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pContPerson2", pContPerson2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pContNo2", pContNo2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 312, "@pDescid", pDescid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pICId = Convert.ToInt32(oCmd.Parameters["@pICId"].Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetBankersPG(Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankersPG";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPurposeId"></param>
        /// <returns></returns>
        public DataTable GetBankersById(Int32 pICId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankersById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pICId", pICId);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFunderId"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable GetAllBankers(string pFunderId, string pBranchCode, DateTime pFrmDt, DateTime pToDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllBankers";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pFunderId", pFunderId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pXmlFundr"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public Int32 InsertBankers(string pXmlIns, Int32 pCreatedBy, string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBankers";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlIns.Length+2, "@pXmlIns", pXmlIns);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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


        public DataTable GetBankerLoanPG(DateTime pFromDt, DateTime pTodate, string pBranch, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankerLoanPG";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pTodate", pTodate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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




        public Int32 InsertBankLoanMst(ref string pLoanNo, string pBankerId, string pSchedule, string pInstType, decimal pLoanAmt, decimal pInstRate,
                                       Int32 pCycle, Int32 pPeriod, Int32 pInstNo, DateTime pLoanDt, DateTime pStDate, string pChequeNo,
                                       DateTime pChqDt, string pDescId, string pBranch, string pTblMst, string pTblDtl, string pFinYear,
                                       Int32 pCreatedBy, string pNarationL, string pNarationF, decimal pIntAmt, decimal pInstAmt, decimal pFLDGAmt, 
                                       string pFLDGBy, decimal pFLDGPer, decimal pFLDGINT, DateTime pFLDGMatDT, decimal pFLDGMarMoney,
                                       decimal pFLDGProPer, decimal pFLDGProAmt, decimal pFLDGOtherPer, decimal pFLDGOtherAmt, Int32 pPrintMora, Int32 pIntMora, 
                                       string pInsAmtPeriod, string pIsRickshaw, DateTime pSanctionDate, decimal pSanctionAmt, DateTime pFLDGDate, Int32 pFLDGPeriod,
                                       decimal pPenalIntRate, string pContEmail )
        {                       
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBankLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBankerId", pBankerId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPeriod", pPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChqDt", pChqDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDescId", pDescId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationF", pNarationF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIntAmt", pIntAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstAmt", pInstAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGPer", pFLDGPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGINT", pFLDGINT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFLDGMatDT", pFLDGMatDT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGMarMoney", pFLDGMarMoney);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGProPer", pFLDGProPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGProAmt", pFLDGProAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGOtherPer", pFLDGOtherPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGOtherAmt", pFLDGOtherAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPrintMora", pPrintMora);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIntMora", pIntMora);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInsAmtPeriod", pInsAmtPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFLDGBy", pFLDGBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsRickshaw", pIsRickshaw);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSanctionDate", pSanctionDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSanctionAmt", pSanctionAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFLDGDate", pFLDGDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFLDGPeriod", pFLDGPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPenalIntRate", pPenalIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pContEmail", pContEmail);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pLoanNo = Convert.ToString(oCmd.Parameters["@pLoanNo"].Value);
                if (vErr == 0)
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


        public Int32 UpdateBankLoanMst(string pLoanNo, string pBankerId, string pSchedule, string pInstType, decimal pLoanAmt, decimal pInstRate,
                Int32 pCycle, Int32 pPeriod, Int32 pInstNo, DateTime pLoanDt, DateTime pStDate, string pChequeNo,
                DateTime pChqDt, string pDescId, string pBranch, string pTblMst, string pTblDtl, string pFinYear,
                Int32 pCreatedBy, string pNarationL, string pNarationF, decimal pIntAmt, decimal pInstAmt, decimal pFLDGAmt, 
                string pFLDGBy, decimal pFLDGPer, decimal pFLDGINT, DateTime pFLDGMatDT, decimal pFLDGMarMoney,
                decimal pFLDGProPer, decimal pFLDGProAmt, decimal pFLDGOtherPer, decimal pFLDGOtherAmt, Int32 pPrintMora, Int32 pIntMora, string pInsAmtPeriod,
                string pIsRickshaw, DateTime pSanctionDate, decimal pSanctionAmt, DateTime pFLDGDate, Int32 pFLDGPeriod, decimal pPenalIntRate, string pContEmail)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateBankLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBankerId", pBankerId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPeriod", pPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChqDt", pChqDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDescId", pDescId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationF", pNarationF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIntAmt", pIntAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstAmt", pInstAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFLDGBy", pFLDGBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGPer", pFLDGPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGINT", pFLDGINT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFLDGMatDT", pFLDGMatDT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGMarMoney", pFLDGMarMoney);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGProPer", pFLDGProPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGProAmt", pFLDGProAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGOtherPer", pFLDGOtherPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGOtherAmt", pFLDGOtherAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPrintMora", pPrintMora);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIntMora", pIntMora);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInsAmtPeriod", pInsAmtPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsRickshaw", pIsRickshaw);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSanctionDate", pSanctionDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSanctionAmt", pSanctionAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFLDGDate", pFLDGDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFLDGPeriod", pFLDGPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPenalIntRate", pPenalIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pContEmail", pContEmail);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pLoanNo = Convert.ToString(oCmd.Parameters["@pLoanNo"].Value);
                if (vErr == 0)
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



        public Int32 DeleteBankLoanMst(string pLoanNo, DateTime pLoanDt, Int32 pCreatedBy, string pBrCode, string pTblMst, string pTblDtl)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteBankLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
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




        public DataTable GetBankSchedule(string pInstType, decimal pLoanAmt, decimal pInterest, int pInstallNo, int pInstPeriod,
                        DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                        string pChequeNo, string pBank, Int32 pPMoraAmt, DateTime pLoanDisbDt, string pInsAmtPeriod)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankSchedule";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInterest", pInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pInstallNo", pInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pStatrDt", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType); //   char(1),--S=Generate the Schedule,L=get schedule from BankLoanDtl Table
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pPMoraAmt", pPMoraAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoanDisbDt", pLoanDisbDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInsAmtPeriod", pInsAmtPeriod);


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


        public DataTable GetBankScheduleEqualEMI(string pInstType, decimal pLoanAmt, decimal pInterest, int pInstallNo, int pInstPeriod,
                      DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                      string pChequeNo, string pBank, Int32 pPMoraAmt, DateTime pLoanDisbDt, string pInsAmtPeriod)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankScheduleEqualEMI";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInterest", pInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pInstallNo", pInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pStatrDt", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType); //   char(1),--S=Generate the Schedule,L=get schedule from BankLoanDtl Table
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pPMoraAmt", pPMoraAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoanDisbDt", pLoanDisbDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInsAmtPeriod", pInsAmtPeriod);
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



	public DataTable GetBankLoanPayment(string pLnStatus, Int32 pRoutine, DateTime pRecvDt, string pLoanId, string pBranch)
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetBankLoanPayment";
			oCmd.CommandTimeout = 4800;
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLnStatus", pLnStatus);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoutine", pRoutine);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPayDt", pRecvDt);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanId);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
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
			dt = null;
		}
	}



	public DataTable GetRepaymentByLoanId(string pLoanId, DateTime pCollDt, string pBranch)
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetRepaymentByLoanId";
			oCmd.CommandTimeout = 4800;
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCollDt", pCollDt);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
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
			dt = null;
		}
	}




	public DataTable GetBankPaymentDtlByLoanId(string pLoanId, string pLedTyp, string pBrCode)
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetBankPaymentDtlByLoanId";
			oCmd.CommandTimeout = 4800;
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLedTyp", pLedTyp);
			// DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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




	public DataTable GetBnkInstallAllocation(string pLoanId, string pBrCode)
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetBnkInstallAllocation";
			oCmd.CommandTimeout = 4800;
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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


	public Int32 InsertRepayment(DateTime pAccDate, string pXml, string pBranch, string pTblMst,
		   string pTblDtl, string pModeAC, string pFinYear, string pNaration, Int32 pCreatedBy, string pEntType, Int32 pSynStatus, string pCollMode)
	{
		SqlCommand oCmd = new SqlCommand();
		Int32 vErr = 0;
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "InsertRepayment";
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", pAccDate);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+2, "@pXml", pXml);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pModeAC", pModeAC);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", pNaration);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", pCollMode);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
			DBUtility.Execute(oCmd);
			vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
			if (vErr == 0)
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


        public Int32 DeleteRepayment(string pLoanID, Int32 pSlNo, string pLnStatus, string pReffID, string pTblMst, string pTblDtl, string pBranch, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
		oCmd.CommandText = "DeleteRepayment";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLnStatus", pLnStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffID", pReffID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
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



	public DataTable GetInterestHead()
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetInterestHead";
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


	public DataTable GetBankerAcHead(Int32 pBankerId)
	{
		DataTable dt = new DataTable();
		SqlCommand oCmd = new SqlCommand();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "GetBankerAcHead";
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBankerId", pBankerId);
			dt = DBUtility.GetDataTable(oCmd);
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
    }
}
