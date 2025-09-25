using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CSSOptionRight
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRoleId"></param>
        /// <param name="pMenuName"></param>
        /// <param name="pMnCaption"></param>
        /// <param name="pMenuType"></param>
        /// <param name="pAllowView"></param>
        /// <param name="pAllowAdd"></param>
        /// <param name="pAllowEdit"></param>
        /// <param name="pAllowDel"></param>
        /// <param name="pAllowPrint"></param>
        /// <param name="pAllowProc"></param>
        /// <param name="pSystem"></param>
        /// <param name="pCompCode"></param>
        /// <param name="pBranch"></param>
        public Int32 InsertSSOptionRight(Int32 pRoleId, string pXmlData)
        {
            SqlCommand oCmd = new SqlCommand();            
            Int32 vErr = 0;
            try
            {                  
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertSSSecureMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length+1, "@pXmlData", pXmlData);
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

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRoleId"></param>
        /// <param name="pMenuType"></param>
        public void DeleteSSecureByRoleId(int pRoleId, int pCreatedBy, DateTime pCreationDateTime, string pEntType)
        {
            SqlCommand oCmd = new SqlCommand();           
            try
            {               
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteSSecureByRoleId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCreationDateTime", pCreationDateTime);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.Execute(oCmd);               
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
        /// <param name="pRoleId"></param>
        /// <returns></returns>
        public DataTable GetModuleByRoleId(double pRoleId, string pMenuType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();           
            try
            {                
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetModuleByRoleId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMenuType", pMenuType);

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
        /// <param name="pRoleId"></param>
        /// <returns></returns>
        public Int32 ChkRoleAssignBeforeDelete(double pRoleId)
        {
            Int32 vRest = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkRoleAssignBeforeDelete";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pRec", vRest);
                DBUtility.Execute(oCmd);
                vRest = Convert.ToInt32(oCmd.Parameters["@pRec"].Value);
                return vRest;
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

        public DataTable GetMenuByRollID(double pRoleId, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMenuByRollID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
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
        /// <param name="pSearch"></param>
        /// <param name="pBranch"></param>
        /// <param name="pMenu"></param>
        /// <returns></returns>
        public DataTable GetSearchRecord(string pSearch, string pBranch, string pMenu, Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSearchRecord";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2000, "@pMenu", pMenu);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
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