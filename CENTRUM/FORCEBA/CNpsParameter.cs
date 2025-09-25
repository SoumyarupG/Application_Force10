using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FORCEDA;
using System.Data.SqlClient;

namespace FORCEBA
{
    public class CNpsParameter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable NPS_GetParameterList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "NPS_GetParameterList";
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
        /// <param name="pID"></param>
        /// <param name="pNLAORegNo"></param>
        /// <param name="pNLAOOffice"></param>
        /// <param name="pNPSAc"></param>
        /// <returns></returns>
        public Int32 NPS_SaveParameter(ref Int32 pID, string pNLAORegNo, string pNLCCNo, string pNPSAc)
        {
            Int32 vErr = 0;
            SqlCommand oCmd = new SqlCommand();

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "NPS_SaveParameter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pID", pID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pNLAORegNo", pNLAORegNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNLAOOffice", pNLCCNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pNPSAc", pNPSAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pID = Convert.ToInt32(oCmd.Parameters["@pID"].Value);
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
