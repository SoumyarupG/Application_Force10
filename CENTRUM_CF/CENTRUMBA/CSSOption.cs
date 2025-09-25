using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CSSOption
    {     
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable GetSSOptionList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();          
            try
            {              
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllSSOption";
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
        /// <returns></returns>
        public DataTable GetAssignedRoleList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {               
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAssignedRoleList";
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