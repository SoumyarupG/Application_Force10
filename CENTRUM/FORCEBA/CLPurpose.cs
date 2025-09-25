using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using FORCEDA;

namespace FORCEBA
{
    public class CLPurpose
    {
        public DataTable rptLPurpose(string pa,string pSecId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptLPurpose";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 10, "@pa", pa);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pSecId", pSecId);
                oCmd.CommandTimeout = 36000;
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



	public DataTable rptLoanPurpose(DateTime pFrmDt, DateTime pToDt, string pBranch, string pPurposeId)
	{
		SqlCommand oCmd = new SqlCommand();
		DataTable dt = new DataTable();
		try
		{
			oCmd.CommandType = CommandType.StoredProcedure;
			oCmd.CommandText = "rptLoanPurpose";
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pBranch", pBranch);
			DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pPurposeId", pPurposeId);
			oCmd.CommandTimeout = 36000;
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
