using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CENTRUMDA;
using System.Data;

namespace CENTRUMBA
{
   public class CNach
    {
       public Int32 SaveNachBank(string pXml, string pSignType)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "SaveNachBank";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+1, "@pXml", pXml);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pSignType", pSignType);              
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

       public Int32 SaveNachCategory(string pXml)
       {
           SqlCommand oCmd = new SqlCommand();
           Int32 vErr = 0;
           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "SaveNachCategory";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);               
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
    }
}
