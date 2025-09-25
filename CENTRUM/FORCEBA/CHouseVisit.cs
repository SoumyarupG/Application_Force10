using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FORCEDA;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Xml;
using System.Configuration;

namespace FORCEBA
{
    public class CHouseVisit
    {
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        public DataTable GetGrtMember(string pBranchcode, string pGroupid, DateTime pLoginDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGrtMember";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", pBranchcode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupid", pGroupid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
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

        public DataTable GetHVMember(string pBranchcode, string pGroupid, DateTime pLoginDt, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHVMember";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", pBranchcode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupid", pGroupid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        /// <param name="pHVId"></param>
        /// <param name="pMemberID"></param>
        /// <param name="pGRTYN"></param>
        /// <param name="pGRTDt"></param>
        /// <param name="pGRTBy"></param>
        /// <param name="pLoanCycle"></param>
        /// <param name="pLoanAppliedYN"></param>
        /// <param name="pLoanDisbYN"></param>
        /// <param name="pQ1"></param>
        /// <param name="pQ1Score"></param>
        /// <param name="pQ2"></param>
        /// <param name="pQ2Score"></param>
        /// <param name="pQ3"></param>
        /// <param name="pQ3Score"></param>
        /// <param name="pQ4"></param>
        /// <param name="pQ4Score"></param>
        /// <param name="pQ5"></param>
        /// <param name="pQ5Score"></param>
        /// <param name="pQ6"></param>
        /// <param name="pQ6Score"></param>
        /// <param name="pQ7"></param>
        /// <param name="pQ7Score"></param>
        /// <param name="pQ8"></param>
        /// <param name="pQ8Score"></param>
        /// <param name="pQ9"></param>
        /// <param name="pQ9Score"></param>
        /// <param name="pQ10"></param>
        /// <param name="pQ10Score"></param>
        /// <param name="pQ11"></param>
        /// <param name="pQ11Score"></param>
        /// <param name="pQ12"></param>
        /// <param name="pQ12Score"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pMode"></param>
        /// <returns></returns>
        /// 

        public Int32 SaveHouseStatus(Int32 pCGTId, string pMemberID, DateTime pHVDt, string pHVBy, DateTime pExpGrtDt, Int32 pQ1,
            Int32 pQ1Score, Int32 pQ2, Int32 pQ2Score, Int32 pQ3, Int32 pQ3Score, Int32 pQ4, Int32 pQ4Score,
            Int32 pQ5, Int32 pQ5Score, Int32 pQ6, Int32 pQ6Score, Int32 pQ7, Int32 pQ7Score, Int32 pQ8, Int32 pQ8Score, Int32 pQ9, Int32 pQ9Score,
            Int32 pQ10, Int32 pQ10Score, Int32 pQ11, Int32 pQ11Score, Int32 pQ12, Int32 pQ12Score, Int32 pQ13, Int32 pQ13Score, Int32 pQ14, Int32 pQ14Score,
            string pBranchCode, Int32 pCreatedBy, string pMode,
            string pQ15, string pQ15Score, string pQ16, string pQ16Score,
            string pQ15Electric, string pQ15Water, string pQ15Toilet, string pQ15Sewage, string pQ15LPG,
            string pQ16Land, string pQ16Vehicle, string pQ16Furniture, string pQ16SmartPhone, string pQ16ElectricItem, Int32 pQ14SubCat
            )
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveHouseStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 13, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pHVDt", pHVDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pHVBy", pHVBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pExpGrtDt", pExpGrtDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ1", pQ1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ1Score", pQ1Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ2", pQ2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ2Score", pQ2Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ3", pQ3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ3Score", pQ3Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ4", pQ4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ4Score", pQ4Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ5", pQ5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ5Score", pQ5Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ6", pQ6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ6Score", pQ6Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ7", pQ7);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ7Score", pQ7Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ8", pQ8);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ8Score", pQ8Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ9", pQ9);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ9Score", pQ9Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ10", pQ10);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ10Score", pQ10Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ11", pQ11);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ11Score", pQ11Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ12", pQ12);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pQ12Score", pQ12Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13", pQ13);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13Score", pQ13Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14", pQ14);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14Score", pQ14Score);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ15", pQ15);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ15Score", pQ15Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ16", pQ16);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ16Score", pQ16Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Electric", pQ15Electric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Water", pQ15Water);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Toilet", pQ15Toilet);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Sewage", pQ15Sewage);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15LPG", pQ15LPG);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Land", pQ16Land);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Vehicle", pQ16Vehicle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Furniture", pQ16Furniture);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16SmartPhone", pQ16SmartPhone);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16ElectricItem", pQ16ElectricItem);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14SubCat", pQ14SubCat);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    if (pMode == "Save")
                    {
                        JocataCalling(pMemberID, pCGTId, pCreatedBy);
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
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



        public Int32 SaveHouseStatusNew(Int32 pCGTId, string pMemberID, DateTime pHVDt, string pHVBy, DateTime pExpGrtDt, Int32 pQ1,
                    double pQ1Score, int pQ1Weighted, Int32 pQ2, double pQ2Score, int pQ2Weighted, Int32 pQ3, double pQ3Score, int pQ3Weighted, Int32 pQ4, double pQ4Score,
                    int pQ4Weighted, Int32 pQ5, double pQ5Score, int pQ5Weighted, Int32 pQ6, double pQ6Score, int pQ6Weighted, Int32 pQ7, double pQ7Score, int pQ7Weighted,
                    Int32 pQ8, double pQ8Score, int pQ8Weighted, string pQ9, double pQ9Score, int pQ9Weighted, Int32 pQ10, double pQ10Score, int pQ10Weighted,
                    Int32 pQ11, double pQ11Score, int pQ11Weighted, Int32 pQ12, double pQ12Score, int pQ12Weighted, Int32 pQ13, double pQ13Score, int pQ13Weighted,
                    Int32 pQ14, double pQ14Score, int pQ14Weighted, string pBranchCode, Int32 pCreatedBy, string pMode, Int32 pQ15, double pQ15Score, int pQ15Weighted)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string Msg = "";
            try
            {

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveHouseStatusNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 13, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pHVDt", pHVDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pHVBy", pHVBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pExpGrtDt", pExpGrtDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1", pQ1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ1Score", pQ1Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1Weighted", pQ1Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2", pQ2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ2Score", pQ2Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2Weighted", pQ2Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3", pQ3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ3Score", pQ3Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3Weighted", pQ3Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4", pQ4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ4Score", pQ4Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4Weighted", pQ4Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5", pQ5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ5Score", pQ5Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5Weighted", pQ5Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6", pQ6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ6Score", pQ6Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6Weighted", pQ6Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7", pQ7);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ7Score", pQ7Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7Weighted", pQ7Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8", pQ8);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ8Score", pQ8Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8Weighted", pQ8Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pQ9", pQ9);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ9Score", pQ9Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ9Weighted", pQ9Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10", pQ10);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ10Score", pQ10Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10Weighted", pQ10Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11", pQ11);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ11Score", pQ11Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11Weighted", pQ11Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12", pQ12);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ12Score", pQ12Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12Weighted", pQ12Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13", pQ13);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ13Score", pQ13Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13Weighted", pQ13Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14", pQ14);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ14Score", pQ14Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14Weighted", pQ14Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ15", pQ15);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ15Score", pQ15Score);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ15Weighted", pQ15Weighted);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1000, "@pMsg", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                Msg = Convert.ToString(oCmd.Parameters["@pMsg"].Value);
                if (vErr == 0)
                {
                    if (pMode == "Save")
                    {
                        JocataCalling(pMemberID, pCGTId, pCreatedBy);
                    }
                    return 1;
                }
                else
                {
                    return 0;
                }
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
        /// <param name="pFrmDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pPgIndx"></param>
        /// <param name="pMaxRow"></param>
        /// <returns></returns>
        public DataTable GetHVMasterPG(DateTime pFrmDt, DateTime pToDt, string pBranchCode, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHVMasterPG";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
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

        public int UpdateUcicId(string pUcicID, string pMemberId, int pCgtId)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateUcicId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUcicID", pUcicID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCgtId", pCgtId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                int pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return pErr;
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
        /// <param name="pTranId"></param>
        /// <returns></returns>
        public DataTable GetHouseVisitById(Int32 pHVId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHouseVisitById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pHVId", pHVId);
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

        public DataTable GetHouseVisitById_New(Int32 pHVId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHouseVisitById_New";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pHVId", pHVId);
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

        public DataTable GetHvQA()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHvQA";
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


        public DataTable GetJocataRequestData(string pMemberId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetJocataRequestData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
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

        #region Jocata Integration

        public string GetJokataToken()
        {
            string postURL = "https://aml.unitybank.co.in//ramp/webservices/createToken";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.Headers.Add("username", "BU_Bijli");
                request.Headers.Add("password", "BU_Bijli");
                request.Headers.Add("clientId", "BU_Bijli");
                request.Headers.Add("clientSecret", "BU_Bijli");
                request.Headers.Add("subBu", "Sub_BU_IB");
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(fullResponse);
                string vJokataToken = res.token;
                return vJokataToken;
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                return Response;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vMemberId = "", vRampResponse = "";
            try
            {
                //-----------------------Create Token--------------------------         
                //vJokataToken = GetJokataToken();
                //vMemberId = postRampRequest.rampRequest.listMatchingPayload.requestListVO.requestVOList[0].customerId;
                //SaveJocataToken(vMemberId, vJokataToken);
                //-----------------------Ramp Request------------------------
                string postURL = "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
                string Requestdata = JsonConvert.SerializeObject(postRampRequest);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + vJokataToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vRampResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                return vRampResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            return vRampResponse;
        }

        public Int32 SaveJocataToken(string pMemberId, string pTokenNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataToken";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTokenNo", pTokenNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string SaveJocataLog(string pMemberId, Int32 pCGTId, string pResponseData, string pScreeningID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningID", pScreeningID);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }

        public int UpdateJocataStatus(string pMemberId, Int32 pCGTId, string pScreeningID, string pStatus, Int32 pCreatedBy, string pRemarks, string pRiskCat)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            int vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateJocataStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningId", pScreeningID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 1, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pRiskCat", pRiskCat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 1, "@pErr", vErr);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return vErr;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vErr;
        }
        #endregion

        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }

        #region RejectGRT
        public string RejectGRT(DateTime pAppDate, string pMemberId, int pCGTID, string pBrCode, string pEoid, string pAppStatus, string pRejectReason, int pCreatedBy, string pScreeningID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "RejectGRT";
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDate", pAppDate);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", pMemberId);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTID", Convert.ToInt32(pCGTID));
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", pEoid);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pAppStatus", pAppStatus);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRejectReason", pRejectReason);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningID", "");

            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pMobileNo", vpMobileNo);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
            DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
            DBUtility.Execute(oCmd);
            vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
            string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);

            vpMobileNo = Convert.ToString(oCmd.Parameters["@pMobileNo"].Value);
            vDigiConcentSMS = Convert.ToString(oCmd.Parameters["@pDigiConcentSMS"].Value);
            vDigiConcentSMSTemplateId = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSTemplateId"].Value);
            vDigiConcentSMSLanguage = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSLanguage"].Value);
            if (vErr == 0)
            {
                try
                {
                    if (vDigiConcentSMS.Length > 0)
                    {
                        // vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vpMobileNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                    }
                }
                finally
                {

                }
                return "Record saved successfully";
            }
            else
            {
                return "Data Not Saved";
            }
        }

        #endregion

        #region JocataCalling
        public string JocataCalling(string pMemberID, int pCGTId, int pCreatedBy)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    List<RequestVOList> vRVL = new List<RequestVOList>();
                    vRVL.Add(new RequestVOList
                    {
                        aadhar = dt.Rows[0]["Aadhar"].ToString(),
                        address = dt.Rows[0]["ParAddress"].ToString(),
                        city = dt.Rows[0]["District"].ToString(),
                        country = dt.Rows[0]["Country"].ToString(),
                        concatAddress = dt.Rows[0]["PreAddr"].ToString(),
                        customerId = dt.Rows[0]["MemberID"].ToString(),
                        digitalID = "",
                        din = "",
                        dob = dt.Rows[0]["DOB"].ToString(),
                        docNumber = "",
                        drivingLicence = dt.Rows[0]["DL"].ToString(),
                        email = "",
                        entityName = "",
                        name = dt.Rows[0]["MemberName"].ToString(),
                        nationality = "Indian",
                        pan = dt.Rows[0]["Pan"].ToString(),
                        passport = dt.Rows[0]["Passport"].ToString(),
                        phone = dt.Rows[0]["Mobile"].ToString(),
                        pincode = dt.Rows[0]["PinCode"].ToString(),
                        rationCardNo = dt.Rows[0]["RationCard"].ToString(),
                        ssn = "",
                        state = dt.Rows[0]["State"].ToString(),
                        tin = "",
                        voterId = dt.Rows[0]["Voter"].ToString()
                    });

                    var vLV = new RequestListVO();
                    vLV.businessUnit = "BU_Bijli";
                    vLV.subBusinessUnit = "Sub_BU_IB";
                    vLV.requestType = "API";
                    vLV.requestVOList = vRVL;

                    var vLMP = new ListMatchingPayload();
                    vLMP.requestListVO = vLV;

                    var vRR = new RampRequest();
                    vRR.listMatchingPayload = vLMP;

                    var req = new PostRampRequest();
                    req.rampRequest = vRR;

                    string vResponseData = RampRequest(req);
                    //string vResponseData = "{\"rampResponse\":{\"statusCode\":\"200\",\"statusMessage\":\"Success\",\"customerId\":\"\",\"txnId\":\"28823\",\"listMatchResponse\":{\"reqStat\":\"\",\"ersk\":\"\",\"eesr\":\"\",\"statusCode\":\"\",\"statusMessage\":\"\",\"txnId\":\"\",\"responseHash\":\"fca65e0d611a6732cabe62627e8978a27b1a2adc75fd2355919c6e5a0a22d60a2ccfc8de27210a68309e3f655fc30400e19512aca0f5079de16f32b314d05daf\",\"salt\":\"828f2fbede2b74793771d21c4ac8beb2\",\"customerId\":\"\",\"ejnr\":\"\",\"matchResult\":{\"matchFlag\":true,\"searchedFor\":\"Name : Dilafruz POLVONOVA %%Customer ID : 123456 %%Passport : CT366512 \",\"searchingFor\":\"\",\"searchedDate\":1677662697267,\"responseVOList\":[{\"primaryName\":\"Dilafruz POLVONOVA\",\"listName\":\"WorldCheckSanctions\",\"alias\":\"POLVONOVA,Dilafro'z Eshmurodovna\",\"pan\":\"\",\"dob\":\"30-MAR-1984\",\"uidai\":\"\",\"passport\":\"ct366512\",\"country\":\"UZBEKISTAN\",\"entryId\":\"7335362\",\"din\":\"\",\"cin\":\"\",\"customerId\":\"\",\"voterId\":\"\",\"tinVat\":\"\",\"phoneNo\":\"\",\"gender\":\"\",\"city\":\" Urgut\",\"score\":\"100.0\",\"ruleName\":\"All Rules\",\"ruleId\":18,\"fields\":[{\"matchedField\":\"name\",\"sourceData\":\"Dilafruz POLVONOVA\",\"targetData\":\"dilafruz polvonova\"}],\"targetData\":{\"country\":\"[UZBEKISTAN]\",\"linkedTo\":\"\",\"Further Information\":\"BIOGRAPHY No known affiliation to any terrorist or militant group. IDENTIFICATION Resident of Urgut District, Samarqand Region, Uzbekistan. FUNDING To be determined. REPORTS Oct 2022 - name published on Uzbekistan Department on Combating Economic Crimes national sanctions list. Jan 2023 - no further information reported.NONCONVICTION TERROR CATEGORY NOTICE This category includes information about individuals who are reportedly being investigated for, or have been arrested or charged on suspicion of, involvement in terrorism or terror related activities. The category also includes individuals or entities identified on national or internationally recognised banning, warning or wanted lists as allegedly connected to terrorism or individuals who are reportedly connected to an organisation included on any national or international terrorism list. Inclusion in this category does not mean that an individual or entity is a terrorist or terrorist organisation or that they are involved in or connected to terrorism or terror related activity. Individuals included in this category have not been convicted of any terror related activity; you should review the content carefully and in accordance with our terms and conditions, further enquiries should be made of the report subject to investigate the outcome of any alleged investigation, arrest, charges or any reported connection to any terror related activity and whether such allegations are denied.\",\"pob\":\"samarkand samarqand region uzbekistan\",\"address\":[{\"country\":\"UZBEKISTAN\",\"city\":\" Urgut\",\"addressType\":\"\",\"pinCode\":\"\",\"state\":\"samarqand\",\"addressLine\":\"\",\"count\":21,\"minRange\":0,\"maxRange\":0,\"innerHitName\":\"\",\"index\":0,\"fullAddress\":\"urgut samarqand uzbekistan\",\"last\":false}],\"Category\":\"NONCONVICTION TERROR\",\"Companies\":\"\",\"Pep Status\":\"\",\"Source\":\"WorldCheckSanctions\",\"lastUpdatedDate\":\"31-Jan-2023\",\"Update Category\":\"\",\"nationality\":\"[UZBEKISTAN, UZBEKISTAN]\",\"passport\":\"[ct366512]\",\"dob\":\"[30-MAR-1984]\",\"OriginalSource\":\"WORLDCHECKSANCTIONS\",\"name\":\"Dilafruz POLVONOVA\",\"aliasNames\":\"[POLVONOVA,Dilafro'z Eshmurodovna, POLVONOVA,Dilafruz Eshmurodovna]\",\"CreatedDate\":\"30-Jan-2023\",\"SubCategory\":\"\",\"Pep Roles\":\"\"},\"drivingLicence\":\"\"},{\"primaryName\":\"Dilafruz POLVONOVA\",\"listName\":\"WorldCheckSanctions\",\"alias\":\"POLVONOVA,Dilafro'z Eshmurodovna\",\"pan\":\"\",\"dob\":\"30-MAR-1984\",\"uidai\":\"\",\"passport\":\"ct366512\",\"country\":\"UZBEKISTAN\",\"entryId\":\"7335362\",\"din\":\"\",\"cin\":\"\",\"customerId\":\"\",\"voterId\":\"\",\"tinVat\":\"\",\"phoneNo\":\"\",\"gender\":\"\",\"city\":\" Urgut\",\"score\":\"100.0\",\"ruleName\":\"All Rules\",\"ruleId\":18,\"fields\":[{\"matchedField\":\"passport\",\"sourceData\":\"ct366512\",\"targetData\":\"ct366512\"}],\"targetData\":{\"country\":\"[UZBEKISTAN]\",\"linkedTo\":\"\",\"Further Information\":\"BIOGRAPHY No known affiliation to any terrorist or militant group. IDENTIFICATION Resident of Urgut District, Samarqand Region, Uzbekistan. FUNDING To be determined. REPORTS Oct 2022 - name published on Uzbekistan Department on Combating Economic Crimes national sanctions list. Jan 2023 - no further information reported.NONCONVICTION TERROR CATEGORY NOTICE This category includes information about individuals who are reportedly being investigated for, or have been arrested or charged on suspicion of, involvement in terrorism or terror related activities. The category also includes individuals or entities identified on national or internationally recognised banning, warning or wanted lists as allegedly connected to terrorism or individuals who are reportedly connected to an organisation included on any national or international terrorism list. Inclusion in this category does not mean that an individual or entity is a terrorist or terrorist organisation or that they are involved in or connected to terrorism or terror related activity. Individuals included in this category have not been convicted of any terror related activity; you should review the content carefully and in accordance with our terms and conditions, further enquiries should be made of the report subject to investigate the outcome of any alleged investigation, arrest, charges or any reported connection to any terror related activity and whether such allegations are denied.\",\"pob\":\"samarkand samarqand region uzbekistan\",\"address\":[{\"country\":\"UZBEKISTAN\",\"city\":\" Urgut\",\"addressType\":\"\",\"pinCode\":\"\",\"state\":\"samarqand\",\"addressLine\":\"\",\"count\":21,\"minRange\":0,\"maxRange\":0,\"innerHitName\":\"\",\"index\":0,\"fullAddress\":\"urgut samarqand uzbekistan\",\"last\":false}],\"Category\":\"NONCONVICTION TERROR\",\"Companies\":\"\",\"Pep Status\":\"\",\"Source\":\"WorldCheckSanctions\",\"lastUpdatedDate\":\"31-Jan-2023\",\"Update Category\":\"\",\"nationality\":\"[UZBEKISTAN, UZBEKISTAN]\",\"passport\":\"[ct366512]\",\"dob\":\"[30-MAR-1984]\",\"OriginalSource\":\"WORLDCHECKSANCTIONS\",\"name\":\"Dilafruz POLVONOVA\",\"aliasNames\":\"[POLVONOVA,Dilafro'z Eshmurodovna, POLVONOVA,Dilafruz Eshmurodovna]\",\"CreatedDate\":\"30-Jan-2023\",\"SubCategory\":\"\",\"Pep Roles\":\"\"},\"drivingLicence\":\"\"}],\"txnId\":\"28823\",\"email\":\"\",\"multiRequest\":false,\"uniqueRequestId\":\"API48975230301092457267\"}}}}";
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);
                    string vScreeningId = "";
                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vStatus = "N";
                            //RejectGRT(pHVDt, pMemberID, pCGTId, pBranchCode,
                            //    pHVBy, "IR", "Jocata Matching", pCreatedBy, vScreeningId);
                        }
                        else
                        {
                            vStatus = "P";
                            try
                            {
                                ProsiReq pReq = new ProsiReq();
                                pReq.pMemberId = pMemberID;
                                pReq.pCreatedBy = Convert.ToString(pCreatedBy);
                                pReq.pCGTId = Convert.ToString(pCGTId);
                                Prosidex(pReq);
                            }
                            finally { }
                        }
                        UpdateJocataStatus(pMemberID, pCGTId, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                    }
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    SaveJocataLog(pMemberID, Convert.ToInt32(pCGTId), vResponseXml, vScreeningId);
                }
            }
            finally { }
            return "Success: Successfully saved.";
        }

        #endregion

        #region SaveProsidexLog
        public string SaveProsidexLog(string pMemberId, Int32 pCGTId, string pRequestId, string pResponseData, Int32 pCreatedBy, string pUCIC_ID, string pPotentialYN, string pPotenURL)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveProsidexLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pRequestId", pRequestId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                if (pUCIC_ID == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPotentialYN", pPotentialYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pPotenURL", pPotenURL);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }

        public string SaveProsidexLogUCIC(string pMemberId, int pCGTId, string pResponseData, Int32 pCreatedBy, string pUCIC_ID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveProsidexLogUCIC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTId", pCGTId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBUtility.Execute(oCmd);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
            return vStatusCode;
        }
        #endregion

        #region Prosidex Integration
        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "";
            Int32 vCreatedBy = 1, vCGTID = 0;
            ProsidexResponse oProsidexResponse = null;
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vCGTID = Convert.ToInt32(prosidexRequest.Request.DG.APPLICATIONID);

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();

                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.Response.StatusInfo.ResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------                  
                return oProsidexResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }

        public ProsidexResponse Prosidex(ProsiReq pProsiReq)
        {
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;
            dt = GetProsidexReqData(pProsiReq.pMemberId, pProsiReq.pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pCGTId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();
                pReq.ACE = new List<object>();
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            return pResponseData;
        }
        #endregion

        #region GetProsidexReqData
        public DataTable GetProsidexReqData(string pMemberId, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetProsidexReqData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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
        #endregion

        public DataTable GetQnAnsDtlById(int pQstnId, string pAnsId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetQnAnsDtlById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQstnId", pQstnId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAnsId", pAnsId);
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
    public class RequestListVO
    {
        public string businessUnit { get; set; }
        public string subBusinessUnit { get; set; }
        public string requestType { get; set; }
        public List<RequestVOList> requestVOList { get; set; }
    }

    public class RequestVOList
    {
        public string aadhar { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string concatAddress { get; set; }
        public string country { get; set; }
        public string customerId { get; set; }
        public string digitalID { get; set; }
        public string din { get; set; }
        public string dob { get; set; }
        public string docNumber { get; set; }
        public string drivingLicence { get; set; }
        public string email { get; set; }
        public string entityName { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string pan { get; set; }
        public string passport { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
        public string rationCardNo { get; set; }
        public string ssn { get; set; }
        public string state { get; set; }
        public string tin { get; set; }
        public string voterId { get; set; }
    }

    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }

    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    public class PostRampRequest
    {
        public RampRequest rampRequest { get; set; }
    }

    public class ProsidexResponse
    {
        public string RequestId { get; set; }
        public string UCIC { get; set; }
        public int response_code { get; set; }

        public ProsidexResponse(string RequestId, string UCIC, int response_code)
        {
            this.RequestId = RequestId;
            this.UCIC = UCIC;
            this.response_code = response_code;
        }
    }

    public class DG
    {
        public string ACCOUNT_NUMBER { get; set; }
        public string ALIAS_NAME { get; set; }
        public string APPLICATIONID { get; set; }
        public string Aadhar { get; set; }
        public string CIN { get; set; }
        public string CKYC { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string CUST_ID { get; set; }
        public string DOB { get; set; }
        public string DrivingLicense { get; set; }
        public string Father_First_Name { get; set; }
        public string Father_Last_Name { get; set; }
        public string Father_Middle_Name { get; set; }
        public string Father_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string GSTIN { get; set; }
        public string Lead_Id { get; set; }
        public string NREGA { get; set; }
        public string Pan { get; set; }
        public string PassportNo { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RationCard { get; set; }
        public string Registration_NO { get; set; }
        public string SALUTATION { get; set; }
        public string TAN { get; set; }
        public string Udyam_aadhar_number { get; set; }
        public string VoterId { get; set; }
        public string Tasc_Customer { get; set; }
    }

    public class Request
    {
        public DG DG { get; set; }
        public List<object> ACE { get; set; }
        public string UnitySfb_RequestId { get; set; }
        public string CUST_TYPE { get; set; }
        public string CustomerCategory { get; set; }
        public string MatchingRuleProfile { get; set; }
        public string Req_flag { get; set; }
        public string SourceSystem { get; set; }
    }

    public class ProsidexRequest
    {
        public Request Request { get; set; }
    }

    public class ProsiReq
    {
        public string pMemberId { get; set; }
        public string pCGTId { get; set; }
        public string pCreatedBy { get; set; }
    }

}
