using System;
using System.Data;
using System.Data.SqlClient;   
using System.Configuration;
using System.Web;
using FORCEDA;

namespace FORCEBA
{
    public class CRole
    {    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRole"></param>
        public Int32 InsertRole(string pRole, double ppRpAmt, double pJrAmt, double pCnAmt, string pStat_YN, string pApp, double pAppAmt, string pDropMem, string pPrematureColl,
            Int32 pCreatedBy, DateTime pLogDt, string pEntType, string pDemise, double pSncApprAmt, string pViewAAdhar, string pMultiColl, string pProTyp, Int32 pBCProductId,
            string pPotenMemYN, string pAllowAdvYN, string pPIIMaskingEnable, string pSARALWaiveAllow, string pMELWaiveAllow, string pJlgDelColl, string pJlgRevColl,
           string pJlgGroupTr, string pJlgCenterTr, string pJLGMemberTr, string pDeathFlag, string pCFWaiveAllow, string pJLGDeviationCtrl)
        {           
            SqlCommand oCmd = new SqlCommand();            
            Int32 vErr = 0;
            try
            {                         
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertRole";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pRole", pRole.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pRpAmt", ppRpAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pJrAmt", pJrAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCnAmt", pCnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStat_YN", pStat_YN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApp", pApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppAmt", pAppAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDropMem", pDropMem);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPrematureColl", pPrematureColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCreationDateTime", pLogDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDemise", pDemise);       
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSncApprAmt", pSncApprAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pViewAAdhar", pViewAAdhar);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pMultiColl", pMultiColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pProTyp", pProTyp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBCProductId", pBCProductId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPotenMemYN", pPotenMemYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pAllowAdvYN", pAllowAdvYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPIIMaskingEnable", pPIIMaskingEnable);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSARALWaiveAllow", pSARALWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pMELWaiveAllow", pMELWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgDelColl", pJlgDelColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgRevColl", pJlgRevColl);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgGroupTr", pJlgGroupTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgCenterTr", pJlgCenterTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJLGMemberTr", pJLGMemberTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDeathFlag", pDeathFlag);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCFWaiveAllow", pCFWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pJLGDeviationCtrl", pJLGDeviationCtrl);
                DBUtility.Execute(oCmd);              
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr > 0)
                    return vErr;
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
        /// <param name="pRoleId"></param>
        /// <param name="pRole"></param>
        public Int32 UpdateRole(Int32 pRoleId, string pRole, double pRpAmt, double pJrAmt, double pCnAmt, string pStat_YN, string pApp, double pAppAmt, string pDropMember, string pPrematureColl,
            Int32 pCreatedBy, DateTime pLogDt, string pDemise, double pSncApprAmt, string pViewAAdhar, string pMultiColl, string pProTyp, Int32 pBCProductId, string pPotenMemYN,
            string pAllowAdvYN, string pPIIMaskingEnable, string pSARALWaiveAllow, string pMELWaiveAllow,string pJlgDelColl, string pJlgRevColl,
            string pJlgGroupTr, string pJlgCenterTr, string pJLGMemberTr, string pDeathFlag, string pCFWaiveAllow, string pJLGDeviationCtrl)
        {
            SqlCommand oCmd = new SqlCommand();           
            Int32 vErr = 0;
            try
            {              
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateRole";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pRole", pRole.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pRpAmt", pRpAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pJrAmt", pJrAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCnAmt", pCnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStat_YN", pStat_YN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApp", pApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppAmt", pAppAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDropMember", pDropMember);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPrematureColl", pPrematureColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCreationDateTime", pLogDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "E");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDemise", pDemise);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSncApprAmt", pSncApprAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pViewAAdhar", pViewAAdhar);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pMultiColl", pMultiColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pProTyp", pProTyp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBCProductId", pBCProductId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPotenMemYN", pPotenMemYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pAllowAdvYN", pAllowAdvYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pPIIMaskingEnable", pPIIMaskingEnable);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSARALWaiveAllow", pSARALWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pMELWaiveAllow", pMELWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgDelColl", pJlgDelColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgRevColl", pJlgRevColl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgGroupTr", pJlgGroupTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJlgCenterTr", pJlgCenterTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pJLGMemberTr", pJLGMemberTr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDeathFlag", pDeathFlag);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCFWaiveAllow", pCFWaiveAllow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pJLGDeviationCtrl", pJLGDeviationCtrl);
                DBUtility.Execute(oCmd);               
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return pRoleId;
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
        /// <returns></returns>
        public Int32 CheckBeforeDelete(Int32 pRoleId)
        {
            Int32 dRst = 0;
            SqlCommand oCmd = new SqlCommand();           
            try
            {                
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkRoleBeforeDelete";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleID", pRoleId);
                //dRst = DBUtility.ExecDblScaler(oCmd);               
                return dRst;
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
        public Int32 DeleteRole(Int32 pRoleId, Int32 pUserId, DateTime vLogDt, string vEntType)
        {
            Int32 vErr = 0;
            SqlCommand oCmd = new SqlCommand();          
            try
            {                
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DeleteRole";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pUserId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCreationDateTime", vLogDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", vEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return pRoleId;
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
        /// <returns></returns>
        public DataTable GetRoleList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();           
            try
            {               
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllRoles";
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
        public DataTable GetRoleById(double pRoleId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {              
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetRoleById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pRoleId", pRoleId);
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
        /// <param name="pRoleID"></param>
        /// <param name="pRole"></param>
        /// <param name="pMode"></param>
        /// <returns></returns>
        public void ChkDuplicateRole(double pRoleID, string pRole, string pMode, ref string pChkDup, ref string pChkSystem)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDuplicateRole";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoleID", pRoleID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pRole", pRole);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pChkDup", pChkDup);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pChkSystem", pChkSystem);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pBrCode", pBrCode);
                DBUtility.Execute(oCmd);
                if (Convert.ToString(oCmd.Parameters["@pChkSystem"].Value) == "N")
                    pChkSystem = "N";
                else
                    pChkSystem = "Y";
                if (Convert.IsDBNull(oCmd.Parameters["@pChkDup"].Value) == true)
                    pChkDup = "N";
                else
                    pChkDup = "Y";
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

        public DataTable GetBCProduct()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBCProduct";
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