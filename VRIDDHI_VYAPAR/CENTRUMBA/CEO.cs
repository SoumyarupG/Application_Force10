using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
   public class CEO
    {
       public DataTable PopRO(string pBrCode, string pEOID, string pDrop, DateTime vLogDt, Int32 pUserId)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "PopRO_KPI";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogDt", vLogDt);
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
       public DataTable PopAmCmByBranch(string pBrCode, DateTime vLogDt, string PDesig)
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "PopAmCmByBranch";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBrCode);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogDt", vLogDt);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, PDesig.Length + 1, "@pDesig", PDesig);
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
