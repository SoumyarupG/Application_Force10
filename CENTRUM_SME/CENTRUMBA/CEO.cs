using System;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;

namespace CENTRUMBA
{
    public class CEO
    {
        public DataTable GetDesignationList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDesignationList";
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
        public Int32 SaveDesignationMst(ref Int32 pNewId, Int32 pDesigId, string pDesignation, string pShortname, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDesignationMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pNewId", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDesigId", pDesigId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pDesignation", pDesignation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pShortname", pShortname.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pNewId = Convert.ToInt32(oCmd.Parameters["@pNewId"].Value);
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
        /// <param name="pNewId"></param>
        /// <param name="pEOID"></param>
        /// <param name="pEMPCode"></param>
        /// <param name="pDOJ"></param>
        /// <param name="pEOName"></param>
        /// <param name="pQualificationId"></param>
        /// <param name="pJBranch"></param>
        /// <param name="pAge"></param>
        /// <param name="pHouseNo"></param>
        /// <param name="pMohalla"></param>
        /// <param name="pVillage"></param>
        /// <param name="pPO"></param>
        /// <param name="pPIN"></param>
        /// <param name="pLandMark"></param>
        /// <param name="pDistrict"></param>
        /// <param name="pState"></param>
        /// <param name="pPhNo1"></param>
        /// <param name="pPhNo2"></param>
        /// <param name="pEmail"></param>
        /// <param name="pReferedName"></param>
        /// <param name="pContPerson"></param>
        /// <param name="pFatherName"></param>
        /// <param name="pDesignation"></param>
        /// <param name="pGender"></param>
        /// <param name="pAllocateYN"></param>
        /// <param name="pDropOutYN"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <returns></returns>
        public Int32 SaveEOMst(ref string pEOID, string pEMPCode, string pEOName, DateTime pDOJ, DateTime pDOB, string pFa_HusName, string pMother,
            string pAddress1, string pAddress2, string pState, string pPIN, string pPh1, string pPh2, string pEmail, string pGrade,
            string pDesignation, int pDeptId, string pLocation, string pBranch, string pRole, string pSupervisorId, string pBrCode,
            Int32 pCreatedBy, string pMode, string pPassword, string pXml)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveEOMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 10, "@pEOID", pEOID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEMPCode", pEMPCode.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEOName", pEOName.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOJ", pDOJ);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", pDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pFa_HusName", pFa_HusName.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMother", pMother.ToUpper());

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress1", pAddress1.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAddress2", pAddress2.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pState", pState.ToUpper());


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPIN", pPIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPh1", pPh1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPh2", pPh2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmail", pEmail);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pGrade", pGrade.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDesignation", pDesignation.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDeptId", pDeptId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLocation", pLocation.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRole", pRole);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pSupervisorId", pSupervisorId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPassword", pPassword);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length+2, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                pEOID = Convert.ToString(oCmd.Parameters["@pEOID"].Value);
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


        public Int32 SaveEOTransfer(string pEOID, DateTime pEffectiveDate, string pStatus, string pGrade, string pDesignation,
                int pDeptId, string pLocation, string pBranch, Int32 pCreatedBy, string pSuperVID, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveEOTransfer";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOID", pEOID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pEffectiveDate", pEffectiveDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", pStatus.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pGrade", pGrade.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDesignation", pDesignation.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDeptId", pDeptId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLocation", pLocation.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pSuperVID", pSuperVID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
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
        /// <param name="pPgIndx"></param>
        /// <param name="pMaxRow"></param>
        /// <returns></returns>
        public DataTable GetEOListPG(string pBranch, Int32 pUserId, Int32 pPgIndx, ref Int32 pMaxRow, string pSearch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOListPG";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar,3,"@pBranch",pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int,4,"@pUserId",pUserId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int,4,"@pPgIndx",pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int,4,"@pMaxRow",pMaxRow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar,30,"@pSearch",pSearch);
                DBUtility.ExecuteForSelect(oCmd, dt);
                pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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


        public DataTable GetEOTransferPG(Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOTransferPG";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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
        /// <param name="pEOID"></param>
        /// <returns></returns>
        public DataTable GetEODetails(String pEOID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEODetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOID", pEOID);
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
        /// <param name="pTransId"></param>
        /// <returns></returns>
        public DataTable GetEOTransByID(int pTransId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOTransByID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pTransId", pTransId);
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
        /// <param name="pEOID"></param>
        /// <returns></returns>
        public DataTable GetEOTransDetail(String pEOID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOTransDetail";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOID", pEOID);
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
        /// <param name="pMode"></param>
        /// <param name="pEoId"></param>
        /// <returns></returns>
        public DataTable GetBranchForRO(string pMode, string pEoId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchForRO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoId", pEoId);
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
        /// <param name="pEOID"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pAllocateYN"></param>
        /// <param name="pAllocateDt"></param>
        /// <param name="pDeallocateYN"></param>
        /// <param name="pDeallocateDt"></param>
        /// <param name="pDropYn"></param>
        /// <param name="pDropDt"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <returns></returns>
        public Int32 SaveEoAllocation(ref Int32 pNewId, Int32 pAlloCateID, Int32 pEOID, string pBranchCode, string pAllocateYN, DateTime pAllocateDt,
            string pDeallocateYN, DateTime pDeallocateDt, string pDropYn, DateTime pDropDt, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveEoAllocation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pNewId", pNewId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAlloCateID", pAlloCateID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEOID", pEOID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAllocateYN", pAllocateYN.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAllocateDt", pAllocateDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDeallocateYN", pDeallocateYN.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDeallocateDt", pDeallocateDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDropYn", pDropYn.ToUpper());
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDropDt", pDropDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pNewId = Convert.ToInt32(oCmd.Parameters["@pNewId"].Value);
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
        /// <param name="pEOID"></param>
        /// <returns></returns>
        public DataSet GetEOAllocationDetails(Int32 pEOID)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOAllocationDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEOID", pEOID);
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
        /// <param name="pAlloCateID"></param>
        /// <returns></returns>
        public DataTable GetEoAllDetails(Int32 pAlloCateID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEoAllDetails";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAlloCateID", pAlloCateID);
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
        /// <param name="pEoId"></param>
        /// <returns></returns>
        public DataTable GetEoAllocList(Int32 pEoId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEoAllocList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEoId", pEoId);
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
        /// <param name="pBrCode"></param>
        /// <param name="pDrpDt"></param>
        /// <returns></returns>
        public DataTable PopRO(string pBrCode, string pEOID, string pDrop, DateTime vLogDt, Int32 pUserId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopRO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOID", pEOID);//default 0
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pDrop", pDrop);//default 0
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogDt", vLogDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pUserId", pUserId);

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
        /// <param name="pEOID"></param>
        /// <param name="pMode"></param>
        /// <returns></returns>
        public DataTable PopSupervisor(string pEOID, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopSupervisor";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOID", pEOID);//default 0
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);//default 0
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
        /// <param name="pBranchcode"></param>
        /// <param name="pLoginDt"></param>
        /// <returns></returns>
        public DataTable GetAllEoNotRo(string pBranchcode, DateTime pLoginDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllEoNotRo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", pBranchcode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLoginDt", pLoginDt);//default 0
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

        public DataTable GetEoSupervisor(DateTime pEffectDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOSupervisor";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pEffectDt", pEffectDt);
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
        /// <param name="pDesignation"></param>
        /// <returns></returns>
        public DataTable GetEOByDesignation(string pDesignation)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetEOByDesignation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pDesignation", pDesignation);
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

        public DataTable GetCOPop(string pBranch, string @pDesStr)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCOPop";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesStr", @pDesStr);
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

        public DataTable getBranchStaff(string pBrCode, string pDesignation)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchStaff";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesignation", pDesignation);
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

        public DataTable GetGrdPartyLedg(string pLO, string pCenter, string pGroup, string pMember, string pLoanNo, DateTime pToDt, string pBranch, Int32 pUserId, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGrdPartyLedg";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pLO", pLO);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pCenter", pCenter);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pGroup", pGroup);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pMember", pMember);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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

        public Int32 ChkEOAllocate(string pID, string pBrachCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vRec = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkEOAllocate";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pID", pID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrachCode", pBrachCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@vRec", vRec);
                DBUtility.Execute(oCmd);
                vRec = Convert.ToInt32(oCmd.Parameters["@vRec"].Value);
                if (vRec == 0)
                    return 0;
                else
                    return 1;
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