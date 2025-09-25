using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FORCEDA;

namespace FORCEBA
{
    public class CHRMst
    {
        public DataTable GetEmployeeLocation(String vBranchCode, DateTime vLoginDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEmployeeLocation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", vBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDt", vLoginDt);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }
        public DataTable GetEmployeeLocationByEmp(String vBranchCode, DateTime vLoginDt, string vEOID, string vFrmTm, string vToTm)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEmployeeLocationByEmployee";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", vBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDt", vLoginDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEOID", vEOID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFrmTm", vFrmTm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pToTm", vToTm);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }
        public DataTable GetEmployeeByBranch(String vBranchCode,DateTime vLoginDt)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
           
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetEoByBranch";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEoID", " ");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", vBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", vLoginDt);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }
    }
}
