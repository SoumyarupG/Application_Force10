using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
   public class CVoucher
    {
       public DataTable PopDisbBank()
       {
           DataTable dt = new DataTable();
           SqlCommand oCmd = new SqlCommand();
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "PopDisbBank";
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

       public DataTable GetAcGenLedCB(string pBranch, string pType, string pDescID)
       {
           SqlCommand oCmd = new SqlCommand();
           try
           {
               DataTable dt = new DataTable();
               oCmd.CommandType = CommandType.StoredProcedure;
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDescID", pDescID);
               oCmd.CommandText = "GetAcGenLedCB";
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
