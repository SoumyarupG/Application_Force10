using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FORCEDA;

namespace FORCEBA
{
    public class CAcGenled
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAccSubGrpId"></param>
        /// <param name="pGenDesc"></param>
        /// <param name="pBranch"></param>
        /// <returns></returns>
        public DataTable GetGenLedByAcSubGrpId(Int32 pAccSubGrpId, string pGenDesc, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGenLedByAcSubGrpId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAccSubGrpId", pAccSubGrpId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pGenDesc", pGenDesc);
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
        /// <param name="pGenLedId"></param>
        /// <returns></returns>
        public DataTable GetGenLedDtl(string pGenLedId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGenLedDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pGenLedId", pGenLedId);
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
        /// <param name="pGenLedId"></param>
        /// <returns></returns>
        public DataTable GetAllSubsidairy(string pBrCode, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllSubsidary";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                dt = DBUtility.GetDataTable(oCmd);
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
        /// <param name="pGenLedId"></param>
        /// <returns></returns>
        public DataSet GetGenLedSubsidairyDtl(string pGenLedId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGenLedDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pGenLedId", pGenLedId);
                ds = DBUtility.GetDataSet(oCmd);
                return ds;
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
        /// <param name="pGenLedCode"></param>
        /// <param name="pGenLedName"></param>
        /// <param name="pGenLedId"></param>
        /// <param name="pChkDup"></param>
        /// <returns></returns>
        public void ChkDupLedger(string pGenLedCode, string pGenLedName, string pGenLedId, string pMode, ref string pChkDup, ref string pChkSystem)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDupLedger";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedCode", pGenLedCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pGenLedName", pGenLedName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedId", pGenLedId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pChkDup", pChkDup);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pChkSystem", pChkSystem);
                DBUtility.Execute(oCmd);
                if (Convert.ToString(oCmd.Parameters["@pChkSystem"].Value) == "N")
                    pChkSystem = "N";
                else
                    pChkSystem = "Y";
                if (Convert.ToString(oCmd.Parameters["@pChkDup"].Value) == "")
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
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pGenLedId"></param>
        ///// <param name="pTblDtl"></param>
        ///// <param name="pChkVoucher"></param>
        ///// <param name="pChkOpBal"></param>
        //public void ChkDeleteACLedger(string pGenLedId, string pTblDtl, ref Int32 pChkVoucher, ref Int32 pChkOpBal)
        //{
        //    SqlCommand oCmd = new SqlCommand();
        //    try
        //    {
        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.CommandText = "ChkDeleteACLedger";
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedId", pGenLedId);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTblDtl", pTblDtl);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pChkVoucher", pChkVoucher);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pChkOpBal", pChkOpBal);
        //        DBUtility.Execute(oCmd);
        //        //if (Convert.IsDBNull(oCmd.Parameters["@pChkDup"].Value) == true)
        //        //    pChkDup = "N";
        //        //else
        //        //    pChkDup = "Y";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oCmd.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pGenCode"></param>
        ///// <param name="pDesc"></param>
        ///// <param name="pShortName"></param>
        ///// <param name="pAcSubGrpId"></param>
        ///// <param name="pAcType"></param>
        ///// <param name="pAddress1"></param>
        ///// <param name="pPhone"></param>
        ///// <param name="pSystem"></param>
        ///// <param name="pEMail"></param>
        ///// <param name="pBranch"></param>
        ///// <param name="pCreatedBy"></param>
        ///// <param name="pEntType"></param>
        ///// <param name="pSynStatus"></param>
        ///// <returns></returns>
        //public Int32 InsertAcGenled(string pGenCode, string pDesc, string pShortName, int pAcSubGrpId, string pAcType, string pAddress1, 
        //    string pPhone, string pSystem, string pEMail, string pBranch, Int32 pCreatedBy, string pEntType, Int32 pSynStatus)
        //{
        //    SqlCommand oCmd = new SqlCommand();
        //    Int32 vErr = 0;
        //    try
        //    {
        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.CommandText = "InsertACGenled";
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedCode", pGenCode.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesc", pDesc.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pShortName", pShortName.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pAcSubGrpId", pAcSubGrpId);
        //        //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSubSiLedYN", pSubSiLedYN);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAcType", pAcType);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress1", pAddress1.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhone", pPhone);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSystem", pSystem);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEMail", pEMail.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pBranch", pBranch);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
        //        DBUtility.Execute(oCmd);
        //        vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
        //        if (vErr == 0)
        //            return 0;
        //        else
        //            return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oCmd.Dispose();
        //    }
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="pDescId"></param>
        ///// <param name="pGenCode"></param>
        ///// <param name="pDesc"></param>
        ///// <param name="pShortName"></param>
        ///// <param name="pAcType"></param>
        ///// <param name="pAddress1"></param>
        ///// <param name="pPhone"></param>
        ///// <param name="pSystem"></param>
        ///// <param name="pEMail"></param>
        ///// <param name="pAcSubGrpId"></param>
        ///// <param name="pEntType"></param>
        ///// <param name="pBranch"></param>
        ///// <param name="pCreatedBy"></param>
        ///// <param name="pSynStatus"></param>
        ///// <returns></returns>
        //public Int32 UpdateAcGenled(string pDescId, string pGenCode, string pDesc, string pShortName, Int32 pAcSubGrpId,  
        //    string pAcType, string pAddress1, string pPhone, string pSystem, string pEMail, string pBranch, Int32 pCreatedBy, string pEntType, Int32 pSynStatus)
        //{
        //    SqlCommand oCmd = new SqlCommand();
        //    Int32 vErr = 0;
        //    try
        //    {
        //        oCmd.CommandType = CommandType.StoredProcedure;
        //        oCmd.CommandText = "UpdateACGenled";
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDescId", pDescId);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedCode", pGenCode.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesc", pDesc.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pShortName", pShortName.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pAcSubGrpId", pAcSubGrpId);
        //        //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSubSiLedYN", pSubSiLedYN);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAcType", pAcType);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress1", pAddress1.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhone", pPhone);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSystem", pSystem);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEMail", pEMail.ToUpper());
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pBranch", pBranch);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
        //        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
        //        DBUtility.Execute(oCmd);
        //        vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
        //        if (vErr == 0)
        //            return 0;
        //        else
        //            return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oCmd.Dispose();
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGenLedId"></param>
        /// <param name="pTblDtl"></param>
        /// <param name="pChkVoucher"></param>
        /// <param name="pChkOpBal"></param>
        public void ChkDeleteACLedger(string pGenLedId, string pTblDtl, ref Int32 pChkVoucher, ref Int32 pChkOpBal)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDeleteACLedger";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedId", pGenLedId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pChkVoucher", pChkVoucher);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pChkOpBal", pChkOpBal);
                DBUtility.Execute(oCmd);
                //if (Convert.IsDBNull(oCmd.Parameters["@pChkDup"].Value) == true)
                //    pChkDup = "N";
                //else
                //    pChkDup = "Y";
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
        /// <param name="pGenCode"></param>
        /// <param name="pDesc"></param>
        /// <param name="pShortName"></param>
        /// <param name="pAcSubGrpId"></param>
        /// <param name="pAcType"></param>
        /// <param name="pAddress1"></param>
        /// <param name="pPhone"></param>
        /// <param name="pSystem"></param>
        /// <param name="pEMail"></param>
        /// <param name="pBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pEntType"></param>
        /// <param name="pSynStatus"></param>
        /// <returns></returns>
        public Int32 InsertAcGenled(string pGenCode, string pDesc, string pShortName, int pAcSubGrpId, string pSubSiLedYN, string pAcType, string pAddress1,
            string pPhone, string pSystem, string pEMail, string pBranch, Int32 pCreatedBy, string pEntType, Int32 pSynStatus, string pXml, string vchkSVE,
            string pAccNo, string pDisburseAC, string pGlshCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertACGenled";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedCode", pGenCode.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesc", pDesc.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pShortName", pShortName.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pAcSubGrpId", pAcSubGrpId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSubSiLedYN", pSubSiLedYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAcType", pAcType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress1", pAddress1.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhone", pPhone);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSystem", pSystem);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEMail", pEMail.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8000, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pchkSVE", vchkSVE);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAccNo", pAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDisburseAC", pDisburseAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGlshCode", pGlshCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //if (vErr == 0)
                //    return 0;
                //else
                //return 1;
                return vErr;
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
        /// <param name="pDescId"></param>
        /// <param name="pGenCode"></param>
        /// <param name="pDesc"></param>
        /// <param name="pShortName"></param>
        /// <param name="pAcType"></param>
        /// <param name="pAddress1"></param>
        /// <param name="pPhone"></param>
        /// <param name="pSystem"></param>
        /// <param name="pEMail"></param>
        /// <param name="pAcSubGrpId"></param>
        /// <param name="pEntType"></param>
        /// <param name="pBranch"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pSynStatus"></param>
        /// <returns></returns>
        public Int32 UpdateAcGenled(string pDescId, string pGenCode, string pDesc, string pShortName, Int32 pAcSubGrpId, string pSubSiLedYN,
            string pAcType, string pAddress1, string pPhone, string pSystem, string pEMail, string pBranch, Int32 pCreatedBy, string pEntType,
            Int32 pSynStatus, string pXml, Int32 pYearNo, string vchkSVE, string pAccNo, string pDisburseAC, string pGlshCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateACGenled";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pDescId", pDescId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGenLedCode", pGenCode.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesc", pDesc.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pShortName", pShortName.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pAcSubGrpId", pAcSubGrpId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSubSiLedYN", pSubSiLedYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAcType", pAcType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress1", pAddress1.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhone", pPhone);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSystem", pSystem);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEMail", pEMail.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8000, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYearNo", pYearNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pchkSVE", vchkSVE);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAccNo", pAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDisburseAC", pDisburseAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pGlshCode", pGlshCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //if (vErr == 0)
                //    return 0;
                //else
                //    return 1;
                return vErr;
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


        public DataTable GetLedgerList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLedgerList";
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

        public DataTable GetParentChildGLDesc(string pGLType, string pParentGL)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetParentChildGLDesc";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pGLType", pGLType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pParentGL.Length + 1, "@pParentGL", pParentGL);
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
