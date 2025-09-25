using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;


namespace CENTRUMBA
{
    public class CFinYear
    {           
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetFinYearList(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();          
            try
            {               
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllFinYear";
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
        public DataTable GetFYrByYrNo(int pYearNo, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFYrByYrNo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYearNo", pYearNo);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pYear"></param>
        /// <returns></returns>
        public DataTable GetFinYearByYear(string @pYear)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();           
            try
            {               
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinYearByYear";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 9, "@pYear", @pYear);
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
        /// <param name="pDate"></param>
        /// <param name="pYrNo"></param>
        /// <returns></returns>
        public Int32 ChkFinancialYear(DateTime pDate, Int32 pYrNo, string pBrCode)
        {
            Int32 dResult = 0;           
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkFinancialYear";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRet", dResult);
                DBUtility.Execute(oCmd);
                dResult = Convert.ToInt32(oCmd.Parameters["@pRet"].Value);
                return dResult;
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