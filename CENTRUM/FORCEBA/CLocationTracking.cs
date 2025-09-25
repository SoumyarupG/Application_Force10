using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using FORCEDA;

namespace FORCEBA
{
   public class CLocationTracking
    {
       public DataTable getMapData(string eoid, DateTime asondate, string pBrCode)
       {
           SqlCommand oCmd = new SqlCommand();
           DataTable dt = new DataTable();

           try
           {
               oCmd.CommandType = CommandType.StoredProcedure;
               oCmd.CommandText = "GetMapData";
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoId", eoid);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAsOnDate", asondate.Date);
               DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pBrCode", pBrCode);
               DBUtility.ExecuteForSelect(oCmd, dt);
           }
           catch
           {
               dt = null;
           }
           finally
           {
               oCmd.Dispose();
           }
           return dt;
       }
    }
}
