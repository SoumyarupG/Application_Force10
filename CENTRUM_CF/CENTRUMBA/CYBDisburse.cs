using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CYBDisburse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanAmt"></param>
        /// <param name="pInterest"></param>
        /// <param name="pInstallNo"></param>
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
        /// <param name="pFrDueday"></param>
        /// <param name="vInstTyp"></param>
        /// <returns></returns>
        public DataTable GetSchedule(decimal pLoanAmt, decimal pInterest, double pInstallNo, double pInstPeriod,
                               DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                               string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string vInstTyp)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYBSchedule";
                oCmd.CommandTimeout = 4800;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLoanAmt", pLoanAmt);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@vInstTyp", vInstTyp);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanInt", pLoanInt);                
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
        /// <param name="pShgID"></param>
        /// <param name="pShgName"></param>
        /// <param name="pRepaysch"></param>
        /// <param name="pIntType"></param>
        /// <param name="pLoanAmt"></param>
        /// <param name="pIntRate"></param>
        /// <param name="pIntsumAmt"></param>
        /// <param name="pIntPeriod"></param>
        /// <param name="pInstallNo"></param>
        /// <param name="InstallAmt"></param>
        /// <param name="pDisDT"></param>
        /// <param name="pFstPaydt"></param>
        /// <param name="pXml"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <param name="pBrCode"></param>
        /// <param name="pYbLoanId"></param>
        /// <returns></returns>
        public Int32 InsertYBLoanMst(ref string pLoanNo, string pShgID, string pShgName, string pRepaysch, string pIntType, double pLoanAmt, double pIntRate, double pIntsumAmt,
                                                 double pIntPeriod, double pInstallNo, double InstallAmt, DateTime pDisDT, DateTime pFstPaydt,
                                                 string pXml, Int32 pCreatedBy, string pMode, string pBrCode, string pYbLoanId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertYBLoan";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 10, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pShgID", pShgID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pShgName", pShgName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pRepaysch", pRepaysch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIntType", pIntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pIntRate", pIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pIntsumAmt", pIntsumAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pIntPeriod", pIntPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@pInstallNo", pInstallNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 15, "@InstallAmt", InstallAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDisDT", pDisDT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFstPaydt", pFstPaydt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pYbLoanId", pYbLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pLoanNo = oCmd.Parameters["@pLoanNo"].Value.ToString();

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
        /// <param name="pYbLoanId"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataSet GetYbLoanById(string pYbLoanId, string pBranchCode)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pYbLoanId", pYbLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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
        /// <param name="pCentreId"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable GetYbLoanBalance(string pGroupId, string pBranchCode, string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanBalance";
                oCmd.CommandTimeout = 4800;

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", pGroupId);
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



        public DataTable GetYbLoanSearch(string pBranch, string pSearch, string pCondSearch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanSearch";
                oCmd.CommandTimeout = 4800;

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pCondSearch", pCondSearch);

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
        /// <param name="pBranchCode"></param>
        /// <param name="pDtFrom"></param>
        /// <param name="pDtTo"></param>
        /// <param name="pShgId"></param>
        /// <returns></returns>
        public DataTable GetYbLoanByShg(string pBranchCode, DateTime pDtFrom, DateTime pDtTo, string pShgId)
        {

            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetYbLoanByShg";
                oCmd.CommandTimeout = 4800;

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDtFrom", pDtFrom);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDtTo", pDtTo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pShgId", pShgId);


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
    }
}
