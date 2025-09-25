using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CDisburse
    {
        public DataTable GetCustForNOC(string pBranch, DateTime pLoginDt)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustForNOC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
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
        public int UpdateUcicId(string pUcicID, string pCustId, string pLoanId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateUcicId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUcicID", pUcicID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                int pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return pErr;
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
        public DataTable GetCustForPreClose(string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustForPreClose";
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
            }
        }
        public DataTable GetLoanNoForNOC(string pBranch, string pCustId, DateTime pLoginDt)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanNoForNOC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
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
        public DataTable GetLoanNoForPreClose(string pBranch, string pCustId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanNoForPreClose";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
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
        public DataTable PrintNOC(DateTime pLoginDt, string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PrintNOC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
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
        public DataTable RptForeCloseLtr(DateTime pLoginDt, string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "RptForeCloseLtr";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogInDate", pLoginDt);
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
        public DataTable GetLoanAppByMemberId(string pMemberId, string pIsDisburse, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAppByMemberId";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
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
        public DataTable GetCustForDisbursh()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameForDisburse";
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
        public DataTable GetCustForDisbLoan()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameFromLnMst";
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
        public DataTable GetCustNameForReSch(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameForReSch";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBrCode);
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
        public DataTable GetCustNameForLegAgreement(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameForLegAgreement";
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
        public DataTable GetCustNameForAgreement()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameForAgreement";
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
        public DataTable GetComments()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetComments";
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
        public DataTable GetSancIdByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSancIdByCustId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        public DataTable GetSancIdForDisb(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSancIdForDisb";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        public DataTable GetLnSancIdByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnSancIdByCustId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        public DataTable GetLnNoByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnNoByCustId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        public DataTable GetLnNoByCustIdForReSch(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnNoByCustIdForReSch";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        public DataTable GetSancIdForAgreement(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSancIdForAgreement";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@CustId", pCustId);
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
        /// <param name="pLoanAppID"></param>
        /// <param name="pBranch"></param>
        /// <returns></returns>
        public DataTable GetLoanAppdtlGD(string pLoanAppID, string pBranch, string vLoanDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAppdtlGD";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppID", pLoanAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 12, "@pLoanDisbDt", vLoanDt);
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
        public DataTable GetSancDtlBySancId(string pSancId, string pCustId, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSancDtlBySancId";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pSancId", pSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranch", pBranch);
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
        /// pInstallNo & pInstPeriod changed to double from int32 by Prasenjit
        /// </summary>
        /// <param name="pLoanTypeID"></param>
        /// <param name="pLoanAmt"></param>
        /// <param name="pInterest"></param>
        /// <param name="pInstallNot"></param>
        /// <param name="pInstPeriod"></param>
        /// <param name="pStatrDt"></param>
        /// <param name="pType"></param>
        /// <param name="pLoanID"></param>
        /// <param name="pIsDisburse"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPaySchedule"></param>
        /// <param name="pBank"></param>
        /// <param name="pChequeNo"></param>
        /// <param name="pCollDay"></param>
        /// <param name="pCollDayNo"></param>
        /// <param name="pLoanType"></param>
        /// <returns></returns>
        public DataTable GetSchedule(Int32 pLoanTypeID, decimal pLoanAmt, decimal pFInterest, decimal pInterest, double pInstallNo, double pInstPeriod,
                                DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                                string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType, Int32 pCollType, DateTime pLoanDt, string pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetScheduleKudos";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFInterest", pFInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInterest", pInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallNo", pInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStatrDt", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@vFirstDuedays", pFrDueday);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPEType", pPEType);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanInt", pLoanInt); 
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pSanctionId", pSanctionId);
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
        public DataTable GetScheduleR(Int32 pLoanTypeID, decimal pLoanAmt, decimal pFInterest, decimal pInterest, double pInstallNo, double pInstPeriod,
                        DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                        string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType, Int32 pCollType, DateTime pLoanDt, string pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetScheduleKudosR";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFInterest", pFInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInterest", pInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallNo", pInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStatrDt", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisburse", pIsDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@vFirstDuedays", pFrDueday);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPEType", pPEType);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanInt", pLoanInt); 
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pSanctionId", pSanctionId);
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
        /// <param name="pLoanNo"></param>
        /// <param name="pLoanAppID"></param>
        /// <param name="pMemberID"></param>
        /// <param name="pEoID"></param>
        /// <param name="pLoanType"></param>
        /// <param name="pLoanDt"></param>
        /// <param name="pTotLnAmt"></param>
        /// <param name="pPeriod"></param>
        /// <param name="vInstType"></param>
        /// <param name="pInstNo"></param>
        /// <param name="pInstRate"></param>
        /// <param name="pSchedule"></param>
        /// <param name="pInstallSize"></param>
        /// <param name="pStDate"></param>
        /// <param name="pCycle"></param>
        /// <param name="pLoanTypeID"></param>
        /// <param name="pPurposeID"></param>
        /// <param name="pFunderID"></param>
        /// <param name="pCashOrChqOrJour"></param>
        /// <param name="pCheckNo"></param>
        /// <param name="pChqDt"></param>
        /// <param name="pChechis"></param>
        /// <param name="pFrame"></param>
        /// <param name="pLicense"></param>
        /// <param name="pAssetType"></param>
        /// <param name="pLedgerAC"></param>
        /// <param name="pProcFees"></param>
        /// <param name="pInsuAmt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pXmlAC"></param>
        /// <param name="pXmlFees"></param>
        /// <param name="pTblMst"></param>
        /// <param name="pTblDtl"></param>
        /// <param name="pFinYear"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pNarationL"></param>
        /// <param name="pNarationF"></param>
        /// <param name="pPDCBank"></param>
        /// <param name="pPDCBranch"></param>
        /// <param name="pPDCChequeNo"></param>
        /// <param name="pCollDay"></param>
        /// <param name="pCollDayNo"></param>
        /// <param name="pPEType"></param>
        /// <param name="pAssemblerId"></param>
        /// <param name="pYbLoanId"></param>
        /// <returns></returns>


        public Int32 SaveLoanRestructure(string pLoanNo, string pCustID, Int32 pLoanTypeId, int pDisbSrl,
       DateTime pDishbDate, decimal pTotLnAmt, string pInstType, string pSchedule, decimal pInstRate, decimal pFInstRate, int pInstNo, decimal pInstPeriod,
       decimal pEMI, decimal pInstallSize, DateTime pStDate, string pDishbMode, Int32 pCycle, Int32 pFunderID, decimal pProcFees, decimal pLPFSTax,
       decimal pLPFKKTax, decimal pLPFSBTax, decimal pInsuAmt, decimal pInsuSTax, decimal pInsuKKTax, decimal pInsuSBTax, decimal pAppCharge,
       decimal pAdvEMIPric, decimal pAdvEMIInt, decimal pStampCharge, decimal pAdvInterest,
       decimal pNetDisbAmt, string pReffLedgerAC, string pTransMode, string pReffNo, DateTime pReffDate, string pBranch, string pXmlPreAC, string pXmlAc,
       string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL, string pNarationR,
       Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pCollType, decimal pPreLnBal, string pPreLnBalLedAC, decimal pCGSTAmt,
       decimal pSGSTAmt, decimal pFLDGAmt, decimal pPreLnOS, string pBrCode, string pRestrAC, decimal pIntDue, decimal pIntWaive,
       decimal pBounceRecv, decimal pBounceWaive)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanRestructure";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPreLoanID", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDisbSrl", pDisbSrl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDishbDate", pDishbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFInstRate", pFInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMI", pEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDishbMode", pDishbMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSTax", pLPFSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFKKTax", pLPFKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSBTax", pLPFSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSTax", pInsuSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuKKTax", pInsuKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSBTax", pInsuSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppCharge", pAppCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIPric", pAdvEMIPric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIInt", pAdvEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pStampCharge", pStampCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvInterest", pAdvInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetDisbAmt", pNetDisbAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pReffLedgerAC", pReffLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", pTransMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffNo", pReffNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReffDate", pReffDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlPreAC.Length + 2, "@pXmlPreAC", pXmlPreAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlAc.Length + 2, "@pXmlAc", pXmlAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationR", pNarationR);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPreLnBalLedAC", pPreLnBalLedAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnOS", pPreLnOS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pRestrAC", pRestrAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIntDue", pIntDue);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIntWaive", pIntWaive);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBounceRecv", pBounceRecv);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBounceWaive", pBounceWaive);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                // pLoanNo = Convert.ToString(oCmd.Parameters["@pLoanNo"].Value);
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
        public Int32 SaveWriteOffDeclare(string pLoanNo, DateTime pWODate, decimal pWOAmt,
          string pTblMst, string pTblDtl, string pFinYear, string pBranch, string pNarationL, Int32 pCreatedBy, Int32 pErr, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveWriteOffDeclare";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pWODate", pWODate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pWOAmt", pWOAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pLoanNo = Convert.ToString(oCmd.Parameters["@pLoanNo"].Value);
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

        public Int32 SaveWriteOffColl(DateTime pCollDate, string pLoanId, Int32 pSlNo, string pCustId, string pCollMode, string pBranch, int pCreatedBy, string pTblMst,
            string pTblDtl, string pFinYear, string pXmlAC, string pNaration, string pCollectedBy, decimal pWODue, decimal pCollAmt, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveWriteOffColl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCollDate", pCollDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", pCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlAC.Length + 1, "@pXmlAC", pXmlAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", pNaration);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 400, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCollectedBy", pCollectedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pWODue", pWODue);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCollAmt", pCollAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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

        public Int32 InsertLoanMst(ref string pLoanNo, string pSanctionId, string pLoanAppID, string pCustID, Int32 pLoanTypeId, int pDisbSrl,
            DateTime pDishbDate, decimal pTotLnAmt, string pInstType, string pSchedule, decimal pInstRate, decimal pFInstRate, int pInstNo, decimal pInstPeriod,
            decimal pEMI, decimal pInstallSize, DateTime pStDate, string pDishbMode, Int32 pCycle, Int32 pFunderID, decimal pProcFees, decimal pLPFSTax,
            decimal pLPFKKTax, decimal pLPFSBTax, decimal pInsuAmt, decimal pInsuSTax, decimal pInsuKKTax, decimal pInsuSBTax, decimal pAppCharge,
            decimal pAdvEMIPric, decimal pAdvEMIInt, decimal pStampCharge, decimal pAdvInterest,
            decimal pNetDisbAmt, string pReffLedgerAC, string pTransMode, string pReffNo, DateTime pReffDate, string pBranch, string pXmlAC, string pXmlFees,
            string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL, string pNarationF,
            Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pCollType, decimal pPreLnBal, string pPreLnBalLedAC, decimal pCGSTAmt,
            decimal pSGSTAmt, decimal pFLDGAmt, string pPreLnId, decimal pTotAmt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppID", pLoanAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDisbSrl", pDisbSrl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDishbDate", pDishbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFInstRate", pFInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMI", pEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDishbMode", pDishbMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSTax", pLPFSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFKKTax", pLPFKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSBTax", pLPFSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSTax", pInsuSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuKKTax", pInsuKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSBTax", pInsuSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppCharge", pAppCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIPric", pAdvEMIPric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIInt", pAdvEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pStampCharge", pStampCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvInterest", pAdvInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetDisbAmt", pNetDisbAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pReffLedgerAC", pReffLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", pTransMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffNo", pReffNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReffDate", pReffDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlAC.Length + 2, "@pXmlAC", pXmlAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlFees.Length + 2, "@pXmlFees", pXmlFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationF", pNarationF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPreLnBalLedAC", pPreLnBalLedAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPreLnId", pPreLnId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotCharge", pTotAmt);
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

        public Int32 InsertLoanMstNew(ref string pLoanNo, string pSanctionId, string pLoanAppID, string pCustID, Int32 pLoanTypeId, int pDisbSrl,
           DateTime pDishbDate, decimal pTotLnAmt, string pInstType, string pSchedule, decimal pInstRate, decimal pFInstRate, int pInstNo, decimal pInstPeriod,
           decimal pEMI, decimal pInstallSize, DateTime pStDate, string pDishbMode, Int32 pCycle, Int32 pFunderID, decimal pProcFees,
           decimal pInsuAmt, decimal pInsuSTax, decimal pInsuCGST, decimal pInsuSGST, decimal pAppCharge,
           decimal pAdvEMIPric, decimal pAdvEMIInt, decimal pStampCharge, decimal pAdvInterest,
           decimal pNetDisbAmt, string pReffLedgerAC, string pTransMode, string pReffNo, DateTime pReffDate, string pBranch,
           string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL,
           Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pCollType, decimal pPreLnBal, string pPreLnBalLedAC, decimal pCGSTAmt,
           decimal pSGSTAmt, decimal pIGSTAmt, decimal pFLDGAmt, string pPreLnId, decimal pTotAmt,
           string pisTransDisburse, string pTrnsDisbAc, decimal pDisburseAmt, string pCollMode, decimal pBrkPrdInt, string pLogBrCode,
            decimal pPreLnInt, ref string pErrDesc, decimal pBrkPrdIntAct, decimal pBrkPrdIntWave,
            decimal vPropInsuAmt, decimal vPropInsuCGSTAmt, decimal vPropInsuSGSTAmt, decimal vAdminFees, decimal vTechFees,
            decimal vInsuIGSTAmt, decimal vPropInsuIGSTAmt,
            decimal pCERSAICharge, decimal pCERSAIChargeCGST, decimal pCERSAIChargeSGST, decimal pCERSAIChargeIGST
            , string pNetOffYN, string pNetOffLoanId, decimal pNetOffAdvanceAmt, decimal pNetOffPrincAmt, decimal pNetOffIntAmt
            )
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLoanMstNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppID", pLoanAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDisbSrl", pDisbSrl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDishbDate", pDishbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFInstRate", pFInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMI", pEMI);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDishbMode", pDishbMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);



                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSTax", pInsuSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuCGST", pInsuCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSGST", pInsuSGST);



                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSTax", pLPFSTax);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFKKTax", pLPFKKTax);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSBTax", pLPFSBTax);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppCharge", pAppCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIPric", pAdvEMIPric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIInt", pAdvEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pStampCharge", pStampCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvInterest", pAdvInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetDisbAmt", pNetDisbAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pReffLedgerAC", pReffLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", pTransMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffNo", pReffNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReffDate", pReffDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPreLnBalLedAC", pPreLnBalLedAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIGSTAmt", pIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPreLnId", pPreLnId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotCharge", pTotAmt);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pisTransDisburse", pisTransDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pTrnsDisbAc", pTrnsDisbAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDisburseAmt", pDisburseAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", pCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBrkPrdInt", pBrkPrdInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pLogBrCode", pLogBrCode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnInt", pPreLnInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", pErrDesc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBrkPrdIntAct", pBrkPrdIntAct);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBrkPrdIntWave", pBrkPrdIntWave);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuAmt", vPropInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuCGSTAmt", vPropInsuCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuSGSTAmt", vPropInsuSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdminFees", vAdminFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTechFees", vTechFees);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuIGSTAmt", vInsuIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuIGSTAmt", vPropInsuIGSTAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAICharge", pCERSAICharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeCGST", pCERSAIChargeCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeSGST", pCERSAIChargeSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeIGST", pCERSAIChargeIGST);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pNetOffYN", pNetOffYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pNetOffLoanId", pNetOffLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffAdvanceAmt", pNetOffAdvanceAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffPrincAmt", pNetOffPrincAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffIntAmt", pNetOffIntAmt);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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

        public Int32 DeleteLoanMst(string pLoanId, string pSanctionId, string pLoanAppID, string pCustID, Int32 pLoanTypeId, int pDisbSrl,
         DateTime pDishbDate, decimal pTotLnAmt, string pInstType, string pSchedule, decimal pInstRate, decimal pFInstRate, int pInstNo, decimal pInstPeriod,
         decimal pEMI, decimal pInstallSize, DateTime pStDate, string pDishbMode, Int32 pCycle, Int32 pFunderID, decimal pProcFees, decimal pLPFSTax,
         decimal pLPFKKTax, decimal pLPFSBTax, decimal pInsuAmt, decimal pInsuSTax, decimal pInsuKKTax, decimal pInsuSBTax, decimal pAppCharge,
         decimal pAdvEMIPric, decimal pAdvEMIInt, decimal pStampCharge, decimal pAdvInterest,
         decimal pNetDisbAmt, string pReffLedgerAC, string pTransMode, string pReffNo, DateTime pReffDate, string pBranch,
         string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL, string pNarationF,
         Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pCollType, decimal pPreLnBal, string pPreLnBalLedAC, decimal pCGSTAmt,
         decimal pSGSTAmt, decimal pFLDGAmt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppID", pLoanAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDisbSrl", pDisbSrl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDishbDate", pDishbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFInstRate", pFInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMI", pEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDishbMode", pDishbMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSTax", pLPFSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFKKTax", pLPFKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSBTax", pLPFSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSTax", pInsuSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuKKTax", pInsuKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSBTax", pInsuSBTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppCharge", pAppCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIPric", pAdvEMIPric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIInt", pAdvEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pStampCharge", pStampCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvInterest", pAdvInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetDisbAmt", pNetDisbAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pReffLedgerAC", pReffLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", pTransMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffNo", pReffNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReffDate", pReffDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationF", pNarationF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPreLnBalLedAC", pPreLnBalLedAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);

                DBUtility.Execute(oCmd);

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
        public Int32 SaveLoanDisbursementBulk(string pMarketID, string pBranch, string pEoID, DateTime pLoanDt, DateTime pStDate,
                                            string pLedgerAC, string pCB, string pBank, string pXml, string pTblMst, string pTblDtl,
                                            string pFinYear, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanDisbursementBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketID", pMarketID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLedgerAC", pLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCB", pCB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 1, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                if (vErr == 0)
                {
                    return 1;
                }
                else
                {
                    if (vErr == 2)
                    {
                        return 2;
                    }
                    else
                    {
                        return 0;
                    }
                }
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
        /// <param name="pLoanId"></param>
        /// <param name="pLoanDt"></param>
        /// <param name="pLoanType"></param>
        /// <param name="pTotLnAmt"></param>
        /// <param name="pPeriod"></param>
        /// <param name="vInstType"></param>
        /// <param name="pInstNo"></param>
        /// <param name="pInstRate"></param>
        /// <param name="pSchedule"></param>
        /// <param name="pInstallSize"></param>
        /// <param name="pStDate"></param>
        /// <param name="pCycle"></param>
        /// <param name="pLoanTypeID"></param>
        /// <param name="pPurposeID"></param>
        /// <param name="pFunderID"></param>
        /// <param name="pCashOrChqOrJour"></param>
        /// <param name="pCheckNo"></param>
        /// <param name="pChqDt"></param>
        /// <param name="pChechis"></param>
        /// <param name="pFrame"></param>
        /// <param name="pLicense"></param>
        /// <param name="pAssetType"></param>
        /// <param name="pLedgerAC"></param>
        /// <param name="pProcFees"></param>
        /// <param name="pInsuAmt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pXmlAC"></param>
        /// <param name="pXmlFees"></param>
        /// <param name="pTblMst"></param>
        /// <param name="pTblDtl"></param>
        /// <param name="pFinYear"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pNarationL"></param>
        /// <param name="pNarationF"></param>
        /// <param name="pPDCBank"></param>
        /// <param name="pPDCBranch"></param>
        /// <param name="pPDCChequeNo"></param>
        /// <param name="pCollDay"></param>
        /// <param name="pCollDayNo"></param>
        /// <param name="pPEType"></param>
        /// <param name="pAssemblerId"></param>
        /// <param name="pYbLoanId"></param>
        /// <returns></returns>
        public Int32 UpdateLoanMst(string pLoanId, DateTime pLoanDt, string pLoanType, decimal pTotLnAmt, Int32 pPeriod, string vInstType, Int32 pInstNo,
            decimal pInstRate, string pSchedule, decimal pInstallSize, DateTime pStDate, Int32 pCycle, Int32 pLoanTypeID, Int32 pPurposeID,
            Int32 pFunderID, string pCashOrChqOrJour, string pCheckNo, DateTime pChqDt, string pChechis, string pFrame, string pLicense, string pAssetType,
            string pLedgerAC, decimal pProcFees, decimal pInsuAmt, string pBranch, string pXmlAC,
            string pXmlFees, string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL, string pNarationF,
            string pPDCBank, string pPDCBranch, string pPDCChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pPEType, string pAssemblerId, string pYbLoanId, Int32 pCollType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLoanMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPeriod", pPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@vInstType", vInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeID", pPurposeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCashOrChqOrJour", pCashOrChqOrJour);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCheckNo", pCheckNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChqDt", pChqDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pChechis", pChechis);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFrame", pFrame);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLicense", pLicense);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAssetType", pAssetType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLedgerAC", pLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlAC.Length + 2, "@pXmlAC", pXmlAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlFees.Length + 2, "@pXmlFees", pXmlFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationF", pNarationF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPDCBank", pPDCBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPDCBranch", pPDCBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPDCChequeNo", pPDCChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPEType", pPEType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAssemblerId", pAssemblerId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pYbLoanId", pYbLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
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

        public DataTable GetDisburseList(string pSearch, DateTime pFromDt, DateTime pTodate, string pBranch, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDisburseList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pTodate", pTodate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (Convert.IsDBNull(oCmd.Parameters["@pMaxRow"].Value) == true)
                    pMaxRow = 0;
                else
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

        public DataTable GetDisburseListForCancel(string pSearch, DateTime pFromDt, DateTime pTodate, string pBranch, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDisburseListForCancel";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pTodate", pTodate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (Convert.IsDBNull(oCmd.Parameters["@pMaxRow"].Value) == true)
                    pMaxRow = 0;
                else
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
        /// <param name="pLoanId"></param>
        /// <param name="pBranch"></param>
        /// <returns></returns>
        public DataTable GetDisbDtlbyLoanId(string pLoanId, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDisbDtlbyLoanId";
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

        public DataTable GetBankLoanByLoanId(string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBankLoanByLoanId";
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

        public DataTable GetBnkLnSchedule(string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBnkLnSchedule";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMemberId"></param>
        /// <param name="pLoanTypeId"></param>
        /// <returns></returns>
        public DataTable GetLoanbyTypeMemId(string pMemberId, Int32 pLoanTypeId, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanbyTypeMemId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pLoanTypeId", pLoanTypeId);
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
            }
        }

        public Int32 GetMemLastModifiedDt(string pMemberId, DateTime pLoanDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vNoOfDays = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemLastModifiedDt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoanDt", pLoanDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pNoOfDays", 0);
                DBUtility.Execute(oCmd);
                vNoOfDays = Convert.ToInt32(oCmd.Parameters["@pNoOfDays"].Value);
                if (vNoOfDays == 0)
                    return 0;
                else
                    return vNoOfDays;
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
        /// <param name="pChechis"></param>
        /// <param name="pFrame"></param>
        /// <param name="pLicense"></param>
        /// <returns></returns>
        public Int32 ChkAssetInformation(string pChechis, string pFrame, string pLicense)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkAssetInformation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pChechis", pChechis);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFrame", pFrame);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLicense", pLicense);
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

        public DataTable GetLnAppDtlByGroup(string pGroupId, DateTime pLogDate, string pBranchCode, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnAppDtlByGroup";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", pGroupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogDate", pLogDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetReScheduleList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReScheduleList";
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

        public DataTable GetReSchedlDtlbyID(Int32 pReSchId, string pLoanNo)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReSchedlDtlbyID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pReSchId", pReSchId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
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





        public DataTable GetReSchedule(Int32 pLoanTypeID, decimal pPOSAmt, decimal pRInterest, DateTime pDueDt, DateTime pDefaultDt, DateTime pStatrDt,
         string pLoanID, string pBranch, string pPaySchedule, string pChequeNo, string pBank, Decimal pEMIAmt, Int32 pLastInstNo, decimal pRPAmt, decimal pRAAmt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ReScheduleKudos";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPOSAmt", pPOSAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRInterest", pRInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDueDt", pDueDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDefaultDt", pDefaultDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStartDate", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEMIAmt", pEMIAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLstInstNo", pLastInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", "L");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRPAmt", pRPAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRAAmt", pRAAmt);
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

        public Int32 InsertReScheduleMst(Int32 pLoanTypeID, decimal pPOSAmt, decimal pFInterest, decimal pRInterest, DateTime pDueDt, DateTime pDefaultDt, DateTime pStatrDt,
                               string pLoanID, string pBranch, string pPaySchedule, string pChequeNo, string pBank, Decimal pEMIAmt, Int32 pLastInstNo,
                               DateTime pReSecheduleDt, DateTime pLnDt, decimal pLnAmt, DateTime pLastRecDt, decimal pPreIntDue, decimal pPreEMIAmt,
                               string pRemarks, Int32 pPreTotInstNo, Int32 pCreatedby, string pEntType, Int32 pErr, decimal pRPAmt, decimal pRAAmt
                             )
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                Int32 vErr = 0;
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertReScheduleMst";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPOSAmt", pPOSAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFInterest", pFInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRInterest", pRInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDueDt", pDueDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDefaultDt", pDefaultDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStartDate", pStatrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPaySchedule", pPaySchedule);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChequeNo", pChequeNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBank", pBank);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEMIAmt", pEMIAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLstInstNo", pLastInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReSecheduleDt", pReSecheduleDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLnDt", pLnDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLnAmt", pLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLastRecDt", pLastRecDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreIntDue", pPreIntDue);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreEMIAmt", pPreEMIAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPreTotInstNo", pPreTotInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedby);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRPAmt", pRPAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRAAmt", pRAAmt);
                DBUtility.ExecuteForSelect(oCmd, dt);
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

        #region Trans Disbursement
        public DataTable GetTransDisbList(DateTime pFrmDt, DateTime pToDt, string pLoanNo, string pBranchCode, int pBusinessId, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTransDisbList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessId", pBusinessId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pMaxRow", pMaxRow);
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

        public DataTable GetTransLoanNo(string pBranch, int pBusinessId, DateTime pLoginDt)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTransLoanNo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessId", pBusinessId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
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
        public DataTable GetCustForTrans(string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustForTrans";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranch);
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
        public DataTable GetLoanNoForTrans(string pCustId, string pBranch)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanNoForTrans";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranch);
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
        public DataTable GetLoanDtlForTrans(string pLoanId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanDtlForTrans";
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
        public DataTable GetTransDisbBranchCode()
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTransDisbBranchCode";
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
        public DataTable GetBranchForHOLegal(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForHOLegal";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetBranchForPreOpinion(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForPreOpinion";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetBranchCodeForLOA(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForLOA";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchCodeForLOD(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForLOD";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchCodeForDocSendToCustody(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForDocSendToCustody";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBrForPreCloseDocRecByHO(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBrForPreCloseDocRecByHO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBrForPreCloseDocHandOver(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBrForPreCloseDocHandOver";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchCodeForDocSend(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForDocSend";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchCodeForDocRec(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForDocRec";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchCodeForMODTD(string pBranchCode, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCodeForMODTD";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetBranchForLegResolve(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForLegResolve";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetBranchForLegResponse(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForLegResolve";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetBranchForFinalLegal(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForFinalLegal";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataSet GetTransDisDtlByLoanId(string pLoanId, string pTransId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataSet dS = new DataSet();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTransDisDtlByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pTransId", pTransId);
                dS = DBUtility.GetDataSet(oCmd);
                return dS;
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

        public DataTable GetRemCompAmtCoLnAmt(string pLoanID, string pBankType)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                DataTable dt = new DataTable();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetRemCompAmtCoLnAmt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanID", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBankType", pBankType);
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

        public Int32 SaveTransDisb(string pLoanId, double pCurDisAmt, string pCheckNo, DateTime pChqDt, string pLedgerAC, string pBranch, Int32 pCreatedBy, string pTblMst,
                string pTblDtl, string pFinYear, string pMode, string pBankType, DateTime pVoucherDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveTransDisb";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDisburseAmt", pCurDisAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCheckNo", pCheckNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pChqDt", pChqDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pLedgerAC", pLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pBankType", pBankType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pVoucherDt", pVoucherDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string message = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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

        public DataTable GetTrnsDtls(string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTrnsDtls";
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
        #endregion
        public DataTable CalCulateBrokenPrdInt(DateTime pLnDate, DateTime pFDueDate, decimal pLnAmt, double pRIntRate)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CalCulateBrokenPrdInt";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 15, "@pLnDate", pLnDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 4, "@pFirstDueDt", pFDueDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLnAmt", pLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pRIntRate", pRIntRate);
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

        public DataTable GetNetOffLoanByLoanAppId(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetNetOffLoanByLoanAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanAppId", pLoanAppId);
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

        public DataTable GetNetOffPIAbyLoanId(string pLoanId, DateTime pLoanDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetNetOffPIAbyLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 8, "@pLoanDt", pLoanDt);
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

        public DataTable GetDeathIntCal(string pLoanId, DateTime pCollDt, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDeathIntCal";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 8, "@pCollDt", pCollDt);
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
            }
        }

        public DataTable GetPreMatureAmtByLoanId(string pLoanId, DateTime pLoanDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreMatureAmtByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 8, "@pLoanDt", pLoanDt);
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

        public Int32 UpdateNachNo(string pLoanId, string pNachNo, int pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateNachNo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNachNo", pNachNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;

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

        public Int32 InsertCancelNEFT(string pXml, Int32 pCreatedby, string pEntType, DateTime pCloseDt, ref String pMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertCancelNEFT";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedby);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCloseDt", pCloseDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pMsg", pMsg);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pMsg = Convert.ToString(oCmd.Parameters["@pMsg"].Value);
                if (vErr == 0)
                {
                    return 0;
                }
                else
                {
                    if (vErr == 2)
                    {
                        return 2;
                    }
                    else if (vErr == 3)
                    {
                        return 3;
                    }
                    else
                    {
                        return 1;
                    }
                }
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

        #region Jocata_Integration

        public DataTable GetJocataRequestData(string pMemberId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetJocataRequestData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
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

        public Int32 SaveJocataToken(string pMemberId, string pTokenNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataToken";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTokenNo", pTokenNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string SaveJocataLog(string pMemberId, string pLoanId, string pResponseData, string pScreeningID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningID", pScreeningID);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }

        public Int32 UpdateJocataStatus(string pLoanId, string pScreeningID, string pStatus, Int32 pCreatedBy, string pRemarks, string pRiskCat)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            int vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateJocataStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningId", pScreeningID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 1, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pRiskCat", pRiskCat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 1, "@pErr", vErr);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return vErr;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vErr;
        }

        public DataTable GetJocataDtlByScrningID(string pScreeningId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetJocataDtlByScrningID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningId", pScreeningId);
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
        #endregion

        #region Prosidex_Integration

        public DataTable GetProsidexReqData(string pMemberId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetProsidexReqData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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

        public string SaveProsidexLog(string pMemberId, string pLoanId, string pRequestId, string pResponseData, Int32 pCreatedBy, string pUCIC_ID, string pPotentialYN, string pPotenURL)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveProsidexLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pRequestId", pRequestId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                if (pUCIC_ID == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPotentialYN", pPotentialYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pPotenURL", pPotenURL);

                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }
        public string SaveProsidexLogUCIC(string pMemberId, string pLoanId, string pResponseData, Int32 pCreatedBy, string pUCIC_ID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveProsidexLogUCIC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }
        #endregion
    }
}
