using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using CENTRUMDA;
using System.Net;
using System.IO;
using System.Text;
using System.Net.NetworkInformation;
using System.Configuration;
using System.Web;

namespace CENTRUMBA
{
    public class CApplication
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly long Base = Alphabet.Length;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pFrmDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pPgIndx"></param>
        /// <param name="pMaxRow"></param>
        /// <returns></returns>
        public DataTable GetSanctionList(string pMode, string pBranchCode, DateTime pFrmDt, DateTime pToDt, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                //oCmd.CommandText = "GetSanctionList";
                oCmd.CommandText = "GetSanctionListNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.ExecuteForSelect(oCmd, dt);
                //pMaxRow = Convert.ToInt32(oCmd.Parameters["@pMaxRow"].Value);
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

        public DataTable GetAllInitialLoanApp(string pCustId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllInitialLoanApp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCustId", pCustId);
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
        public Int32 SaveHighmarkResponse(string pCustomerId, string pResponseData, DateTime pLoginDt, string pBrCode, Int32 pCreatedBy, string pCustomerType, ref string pErrDescResponse)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveHighmarkResponse";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustomerId", pCustomerId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustomerType", pCustomerType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", pErrDescResponse);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDescResponse = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);
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
        public Int32 SaveDetailsHighmarkResponse(string pCustomerId, string pInqUniqueRefNo, string pResponseData, DateTime pLoginDt, string pBrCode,
            int pCreatedBy, string pCustomerType, ref string pErrDescResponse)
        {

            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDetailsHighmarkResponse";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustomerId ", pCustomerId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pInqUniqueRefNo", pInqUniqueRefNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", pLoginDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustomerType", pCustomerType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", pErrDescResponse);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDescResponse = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);
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
        public DataSet GetLnAppList(string pCustId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnAppList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCustId", pCustId);
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
        public DataTable GetCustomer(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllCustomer";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBranchCode);
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
        public DataSet GetInitLoanDtlByLoanId(string pLoanId, string pBranchCode)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                //oCmd.CommandText = "GetInLoanAppByLoanId";
                oCmd.CommandText = "GetInLoanAppByLoanIdNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBranchCode);
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
        public DataTable GetLoanAppDtlLoanId(string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAppDtlLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
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

        public DataTable GetCustForHighmark(string pCustId, string pType = "A")
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustForHighmark";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
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
        /// <param name="pMemberId"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable GetCycleByMemberId(string pMemberId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCycleByMemberId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        /// <param name="pLoanTypeID"></param>
        /// <returns></returns>
        public DataTable GetLnAmtByLoanTypeId(Int32 pLoanTypeID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnAmtByLoanTypeId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeID", pLoanTypeID);
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
        /// <param name="pMemberID"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public string ChkSaveApplication(string pMemberID, string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            string pRet = "X";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkSaveApplication";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pRet", pRet);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToString(oCmd.Parameters["@pRet"].Value);
                return pRet;
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
        /// <param name="pMemberID"></param>
        /// <param name="pGroupID"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pDOA"></param>
        /// <param name="pCycle"></param>
        /// <param name="pAppAmt"></param>
        /// <returns></returns>
        public string ChkLoanApplication_Size(string pMemberID, string pGroupID, string pBranchCode, DateTime pDOA, Int32 pCycle, Double pAppAmt)
        {
            SqlCommand oCmd = new SqlCommand();
            string pRet = "X";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkLoanApplication_Size";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupID", pGroupID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOA", pDOA);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 12, "@pAppAmt", pAppAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pRet", pRet);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToString(oCmd.Parameters["@pRet"].Value);
                return pRet;
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
        /// <param name="pLoanAppId"></param>
        /// <param name="pMemberID"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public Int32 ChkEditApplication(string pLoanAppId, string pMemberID, string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkEditApplication";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRet", 0);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToInt32(oCmd.Parameters["@pRet"].Value);
                return pRet;
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
        /// <param name="pLoanAppNo"></param>
        /// <param name="pMemberID"></param>
        /// <param name="vAppDt"></param>
        /// <param name="vLnTypeId"></param>
        /// <param name="vPurpId"></param>
        /// <param name="vLnAmt"></param>
        /// <param name="vCycle"></param>
        /// <param name="vExPectedDt"></param>
        /// <param name="vPPI1"></param>
        /// <param name="vPPI2"></param>
        /// <param name="vPPI3"></param>
        /// <param name="vPPI4"></param>
        /// <param name="vPPI5"></param>
        /// <param name="vPPI6"></param>
        /// <param name="vPPI7"></param>
        /// <param name="vPPI8"></param>
        /// <param name="vPPI9"></param>
        /// <param name="vPPI10"></param>
        /// <param name="vPPI11"></param>
        /// <param name="vPPI12"></param>
        /// <param name="vPPI13"></param>
        /// <param name="vPPI14"></param>
        /// <param name="vPPI15"></param>
        /// <param name="vPPI16"></param>
        /// <param name="vPPI17"></param>
        /// <param name="vPPI18"></param>
        /// <param name="vBrCode"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pEntType"></param>
        /// <param name="pYrNo"></param>
        /// <returns></returns>
        public Int32 SaveInitialApplication(ref string pLoanAppNo, string pApplicantID, DateTime pAppDt, Int32 pPurpId, decimal pAppliedAmt, int pTenure,
             string pCBPassYN, string pBrCode, Int32 pCreatedBy, string pEntType, Int32 pYrNo, Int32 pLnTypeId, string pMachDtl, int pSourceId,
            string vXmlCoApp, string vXmlAsset, string pPassYN, DateTime pPassorRejDate, string pRejReason, string pAddTerms, ref string pErrDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                //oCmd.CommandText = "SaveInitialApplication";
                oCmd.CommandText = "SaveInitialApplicationNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanAppNo", pLoanAppNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicantID", pApplicantID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAppDt", pAppDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurpId", pPurpId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLnTypeId", pLnTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppAmt", pAppliedAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCBPassYN", pCBPassYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pMacDtl", pMachDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSourceId", pSourceId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, vXmlCoApp.Length + 1, "@pXmlCoApp", vXmlCoApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, vXmlAsset.Length + 1, "@pXmlAsset", vXmlAsset);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPassYN", pPassYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPassorRejDate", pPassorRejDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pRejReason", pRejReason);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 800, "@pAddTerms", pAddTerms);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDesc", pErrDesc);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pMobileNo", vpMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pLoanAppNo = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);

                vpMobileNo = Convert.ToString(oCmd.Parameters["@pMobileNo"].Value);
                vDigiConcentSMS = Convert.ToString(oCmd.Parameters["@pDigiConcentSMS"].Value);
                vDigiConcentSMSTemplateId = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSTemplateId"].Value);
                vDigiConcentSMSLanguage = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSLanguage"].Value);

                if (vErr == 0)
                {
                    try
                    {
                        if (vDigiConcentSMSLanguage.ToUpper() == "ENGLISH")
                        {
                            vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", "https://bijliftt.com/m/" + Encode(Convert.ToInt64(pLoanAppNo)));
                        }
                        else
                        {
                            vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", System.Web.HttpUtility.UrlEncode("https://bijliftt.com/m/" + Encode(Convert.ToInt64(pLoanAppNo)), Encoding.GetEncoding("ISO-8859-1")));
                        }

                        if (vDigiConcentSMS.Length > 0)
                        {
                            vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vpMobileNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                        }
                    }
                    finally
                    {

                    }
                    return 0;

                }
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

        public Int32 UpdateApplication(string pLoanAppId, string pApplicantID, DateTime pAppDt, Int32 pPurpId, decimal pLnAmt, Int32 pTenure,
                                        string pBrCode, Int32 pCreatedBy, string pMode, Int32 pLnTypeId, string pMachDtl, int pSourceId,
                                        string vXmlCoApp, string vXmlAsset, string pPassYN, DateTime pPassorRejDate, string pRejReason,
            string pAddTerms, ref string pErrDesc)//, string pType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                // oCmd.CommandText = "UpdateApplication";
                oCmd.CommandText = "UpdateApplicationNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicantIdID", pApplicantID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAppDt", pAppDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurpId", pPurpId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLnTypeId", pLnTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLnAmt", pLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 4000, "@pErrDesc", pErrDesc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pMachDtl", pMachDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSourceId", pSourceId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, vXmlCoApp.Length + 1, "@pXmlCoApp", vXmlCoApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, vXmlAsset.Length + 1, "@pXmlAsset", vXmlAsset);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPassYN", pPassYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPassorRejDate", pPassorRejDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pRejReason", pRejReason);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 800, "@pAddTerms", pAddTerms);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pMobileNo", vpMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);

                vpMobileNo = Convert.ToString(oCmd.Parameters["@pMobileNo"].Value);
                vDigiConcentSMS = Convert.ToString(oCmd.Parameters["@pDigiConcentSMS"].Value);
                vDigiConcentSMSTemplateId = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSTemplateId"].Value);
                vDigiConcentSMSLanguage = Convert.ToString(oCmd.Parameters["@pDigiConcentSMSLanguage"].Value);
                if (vErr == 0)
                {
                    if (vDigiConcentSMSLanguage.ToUpper() == "ENGLISH")
                    {
                        vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", "https://bijliftt.com/m/" + Encode(Convert.ToInt64(pApplicantID)));
                    }
                    else
                    {
                        vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", HttpUtility.UrlEncode("https://bijliftt.com/m/" + Encode(Convert.ToInt64(pApplicantID)), Encoding.GetEncoding("ISO-8859-1")));
                    }
                    try
                    {
                        if (vDigiConcentSMS.Length > 0)
                        {
                            vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vpMobileNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                        }
                    }
                    finally
                    {

                    }
                    return 0;
                }
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

        public Int32 SaveFinalSanction(string pLnSancId, string pFinSancStatus, string pFinAppBy, DateTime pFinalAppDate, string pFinAppRemark, Int32 pErr, int pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveFinalSanctionDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSancId", pLnSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFinSancStatus", pFinSancStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pFinAppBy", pFinAppBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinalAppDate", pFinalAppDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFinAppRemark", pFinAppRemark);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pNewId = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                if (vErr == 0)
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

        public Int32 UpdateFinalSanctionDtl(string pLnSancId, string pFinSancStatus, string pFinAppBy, DateTime pFinalAppDate, string pFinAppRemark, Int32 pErr)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateFinalSanctionDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSancId", pLnSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFinSancStatus", pFinSancStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pFinAppBy", pFinAppBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinalAppDate", pFinalAppDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pFinAppRemark", pFinAppRemark);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pNewId = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                if (vErr == 0)
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
        
        public Int32 FinalSanctionSendBack(string pLnSancId, string pStatus, Int32 pCreatedBy, Int32 pErr)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "FinalSanctionSendBack";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSancId", pLnSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pNewId = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                if (vErr == 0)
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
        
        public Int32 SaveSanction(ref string pNewId, string pLnSancId, string pLnAppId, string pCustId, DateTime pSancDate, decimal pSancAmt,
            int pLnTypeId, decimal pFIntRate, decimal pRIntRate, Int32 pNoOfInstal, Int32 pTenure, string pIntType, string pRepayType, decimal pEMIAm,
            decimal pLPFAmt, decimal pLPFSTAmt, decimal pInsAmt, decimal pInsCGSTAmt,
            DateTime pFinalSancDate, DateTime pRepayStartDate, string pIsDisbursed, decimal pNetDisbAmt, string pAdvEMIRcvYS, string pSanctionStatus,
            DateTime pFinalApprovedDt, string pFinalApprovedBy, string pRemarks, string pBranchCode, decimal pLPFKKTax, decimal pLPFSBTax, decimal pInsuSGSTAmt,
            decimal pInsuSBTax, decimal pLPFPer, decimal pLPFSTxrate, decimal pLPFKKRate, decimal pLPFSBRate, decimal pAppChrge, decimal pStampChrge, decimal TotChrge,
            int pCreatedBy, string pMode, Int32 pErr, DateTime pDisbDate, decimal pPreEMIInt, decimal pPreLnBal, string pSecurityChk1, string pSecurityChk2, string pSecurityChk3,
            decimal pLPFCGSTRate, decimal pLPFCGSTAmt, decimal pLPFSGSTRate, decimal pLPFSGSTAmt, decimal pFLDGRate, decimal pFLDGAmt,
            string pPreLnIdTopUp, string pPreLnAc,
            decimal pBrkPrdIntAct, decimal pBrkPrdIntWave, decimal pBrkPrdInt,
            decimal pPropertyInsAmt, decimal pPropertyInsCGSTAmt, decimal pPropertyInsSGSTAmt, decimal pAdminFees, decimal pTechFees,
            decimal pIGSTAmt, decimal pInsuIGSTAmt, string pIGSTAppLPF, string pIGSTAppInsu, string pIGSTAppPropInsu, decimal pPropInsuIGST,
            string pIGSTAppOnCERSAICharge, decimal pCERSAICharge, decimal pCERSAIChargeCGST, decimal pCERSAIChargeSGST, decimal pCERSAIChargeIGST, Int32 pICID)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveSanctionDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pNewId", pNewId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnSancId", pLnSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSancDate", pSancDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pSancAmt", pSancAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLnTypeId", pLnTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFIntRate", pFIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRIntRate", pRIntRate);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfInstal", pNoOfInstal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIntType", pIntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRepayType", pRepayType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEMIAmt", pEMIAm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFAmt", pLPFAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFSTAmt", pLPFSTAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInsAmt", pInsAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInsSTAmt", pInsCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInsuKKTax", pInsuSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInsuSBTax", 0);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinalSancDate", pFinalSancDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pRepayStartDate", pRepayStartDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsDisbursed", pIsDisbursed);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetDisbAmt", pNetDisbAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAdvEMIRcvYN", pAdvEMIRcvYS);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSanctionStatus", pSanctionStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFinalApprovedDt", pFinalApprovedDt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pFinalApprovedBy", pFinalApprovedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFKKTax", pLPFKKTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFSBTax", pLPFSBTax);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@LPFPer", pLPFPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@LPFSTxrate", pLPFSTxrate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFKKRate", pLPFKKRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@LPFSBRate", pLPFSBRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pAppChrge", pAppChrge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStampChrge", pStampChrge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@TotChrge", TotChrge);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDisbDate", pDisbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreEMIInt", pPreEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSecurityChk1", pSecurityChk1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSecurityChk2", pSecurityChk2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSecurityChk3", pSecurityChk3);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFCGSTRate", pLPFCGSTRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFCGSTAmt", pLPFCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFSGSTRate", pLPFSGSTRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLPFSGSTAmt", pLPFSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFLDGRate", pFLDGRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPreLnIdTopUp", pPreLnIdTopUp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPreLnAc", pPreLnAc);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBrkPrdIntAct", pBrkPrdIntAct);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBrkPrdIntWave", pBrkPrdIntWave);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBrkPrdInt", pBrkPrdInt);

                //decimal pPropertyInsAmt, decimal pPropertyInsCGSTAmt, decimal pPropertyInsSGSTAmt, decimal pAdminFees, decimal pTechFees
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPropertyInsAmt", pPropertyInsAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPropertyInsCGSTAmt", pPropertyInsCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPropertyInsSGSTAmt", pPropertyInsSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pAdminFees", pAdminFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTechFees", pTechFees);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIGSTAmt", pIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInsuIGSTAmt", pInsuIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIGSTAppLPF", pIGSTAppLPF);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIGSTAppInsu", pIGSTAppInsu);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIGSTAppPropInsu", pIGSTAppPropInsu);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPropInsuIGST", pPropInsuIGST);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIGSTAppOnCERSAICharge", pIGSTAppOnCERSAICharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAICharge", pCERSAICharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeCGST", pCERSAIChargeCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeSGST", pCERSAIChargeSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeIGST", pCERSAIChargeIGST);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pICID", pICID);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pNewId = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                if (vErr == 0)
                    return 0;
                else if (vErr == 2)
                    return 2;
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

        public Int32 SaveCAM(Int32 pNewId, int pCAMId, string pLnAppId, int pPurposeId, int pLoanTypeId, decimal pAppLnAmt, decimal pProLnAmt,
            decimal pProIntRate, Int32 pAPPTenure, Int32 pProTenure, Int32 pNoOfInstal, decimal pTermLn, decimal pCCOD, decimal pCIBILLoanOS, decimal pKFIExistExpos,
            decimal pProExpos, decimal pTotalDebt, decimal pExistEMI, decimal pProEMI, decimal pCCIntSave, decimal pTotMonLia,
            decimal pTotalAMScore, string pRemarks, int pPartId1, int pPartId2, int pPartId3, int pPartId4, int pPartId5, int pPartId6, int pPartId7, int pPartId8,
            int pPartId9, int pPartId10, int pPartId11, int pPartId12, int pPartId13, int pPartId14, int pPartId15, int pPartId16, int pPartId17, int pPartId18,
            int pAMValueId1, int pAMValueId2, int pAMValueId3, int pAMValueId4, int pAMValueId5, int pAMValueId6, int pAMValueId7, int pAMValueId8,
            int pAMValueId9, int pAMValueId10, int pAMValueId11, int pAMValueId12, int pAMValueId13, int pAMValueId14, int pAMValueId15, int pAMValueId16,
            int pAMValueId17, int pAMValueId18, decimal pStr1, decimal pStr2, decimal pStr3, decimal pStr4, decimal pStr5, decimal pStr6, decimal pStr7,
            decimal pStr8, decimal pStr9, decimal pStr10, decimal pStr11, decimal pStr12, decimal pStr13, decimal pStr14, decimal pStr15, decimal pStr16,
            decimal pStr17, decimal pStr18, string pCustId, string pNewSancId, string pBranchCode, int pCreatedBy, string pMode, Int32 pErr,
            decimal pTotalAMScorePercen, decimal pTotalEquity, decimal pDERatio, string pTotDebtComments, decimal pPBDIT, decimal pDSCR, string ptxtTotEquityComments
            )
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveCAM";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNewId", pNewId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 12, "@pCAMId", pCAMId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", pPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pAppLnAmt", pAppLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pProLnAmt", pProLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pProIntRate", pProIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAPPTenure", pAPPTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pProTenure", pProTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfInstal", pNoOfInstal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTermLn", pTermLn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCCOD", pCCOD);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCIBILLoanOS", pCIBILLoanOS);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pKFIExistExpos", pKFIExistExpos);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pProExpos", pProExpos);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalDebt", pTotalDebt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pExistEMI", pExistEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pProEMI", pProEMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCCIntSave", pCCIntSave);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotMonLia", pTotMonLia);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalAMScore", pTotalAMScore);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId1", pPartId1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId2", pPartId2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId3", pPartId3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId4", pPartId4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId5", pPartId5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId6", pPartId6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId7", pPartId7);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId8", pPartId8);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId9", pPartId9);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId10", pPartId10);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId11", pPartId11);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId12", pPartId12);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId13", pPartId13);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId14", pPartId14);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId15", pPartId15);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId16", pPartId16);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId17", pPartId17);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPartId18", pPartId18);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId1", pAMValueId1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId2", pAMValueId2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId3", pAMValueId3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId4", pAMValueId4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId5", pAMValueId5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId6", pAMValueId6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId7", pAMValueId7);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId8", pAMValueId8);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId9", pAMValueId9);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId10", pAMValueId10);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId11", pAMValueId11);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId12", pAMValueId12);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId13", pAMValueId13);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId14", pAMValueId14);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId15", pAMValueId15);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId16", pAMValueId16);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId17", pAMValueId17);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAMValueId18", pAMValueId18);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr1", pStr1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr2", pStr2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr3", pStr3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr4", pStr4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr5", pStr5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr6", pStr6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr7", pStr7);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr8", pStr8);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr9", pStr9);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr10", pStr10);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr11", pStr11);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr12", pStr12);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr13", pStr13);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr14", pStr14);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr15", pStr15);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr16", pStr16);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr17", pStr17);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pStr18", pStr18);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pNewSancID", pNewSancId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalAMScorePercen", pTotalAMScorePercen);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalEquity", pTotalEquity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pDERatio", pDERatio);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTotDebtComments", pTotDebtComments);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPBDIT", pPBDIT);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pDSCR", pDSCR);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTotEquityComments", ptxtTotEquityComments);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //pNewId = Convert.ToString(oCmd.Parameters["@pLoanAppNo"].Value);
                if (vErr == 0)
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



        public Int32 SaveLoanAppliBulk(string pMarketID, DateTime pAppDt, int pYrNo, string pBrCode, string pXml, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanAppliBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketID", pMarketID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAppDt", pAppDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
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
        public Int32 SaveBranchLegal(string pLnAppId, DateTime pDate, Int32 pCreatedBy, string pMode, string pBrCode, string pFileNm)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveBranchLegal";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pLegBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pFileNm.Length + 1, "@pFileName", pFileNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
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
        /// <param name="pLoanAppId"></param>
        /// <param name="pMemberID"></param>
        /// <param name="pAppDt"></param>
        /// <param name="pLnTypeId"></param>
        /// <param name="pPurpId"></param>
        /// <param name="pLnAmt"></param>
        /// <param name="pCycle"></param>
        /// <param name="pExPectedDt"></param>
        /// <param name="pPPI1"></param>
        /// <param name="pPPI2"></param>
        /// <param name="pPPI3"></param>
        /// <param name="pPPI4"></param>
        /// <param name="pPPI5"></param>
        /// <param name="pPPI6"></param>
        /// <param name="pPPI7"></param>
        /// <param name="pPPI8"></param>
        /// <param name="pPPI9"></param>
        /// <param name="pPPI10"></param>
        /// <param name="pPPI11"></param>
        /// <param name="pPPI12"></param>
        /// <param name="pPPI13"></param>
        /// <param name="pPPI14"></param>
        /// <param name="pPPI15"></param>
        /// <param name="pPPI16"></param>
        /// <param name="pPPI17"></param>
        /// <param name="pPPI18"></param>
        /// <param name="pBrCode"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pEntType"></param>
        /// <param name="pMode"></param>
        /// <param name="pType"></param>
        /// <returns></returns>


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pLoanAppId"></param>
        /// <returns></returns>
        public DataTable GetApplicationDtl(String pLoanAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetApplicationDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
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
        public DataTable TotDueOnSancdate(String pLoanAppId, DateTime pSancDate)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "TotDueOnSancdate";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSancDate", pSancDate);
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
        public DataSet GetSanctionDtl(String pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSanctionDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanSancId", pSanctionId);
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
        public DataTable GetFinalSanctionDtl(String pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinalSanctionDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanSancId", pSanctionId);
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
        public DataTable GetFinalSanctionDtlBySanctId(String pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinalSanctionDtlBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanSancId", pSanctionId);
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
        public DataTable GetMODTDtlBySanctId(String pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMODTDtlBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanSancId", pSanctionId);
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
        public DataTable GetLOADtlBySanctId(String pSanctionId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLOADtlBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanSancId", pSanctionId);
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
        public DataSet GetPenDocSendBySanctId(string pSanctionId, Int32 pUserId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenDocSendBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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
        public DataSet GetPenDocRecBySanctId(string pSanctionId, Int32 pUserId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenDocRecBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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
        public DataSet GetPenLODBySanctId(string pSanctionId, Int32 pUserId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLODBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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
        public DataSet GetPenDocSendToCustodyBySanctId(string pSanctionId, Int32 pUserId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenDocSendToCustodyBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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
        public DataSet GetLODBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLODBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetDocSendToCustodyBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocSendToCustodyBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetPendPreCloseDocRecBySanctId(string pSanctionId, Int32 pUserId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPendPreCloseDocRecBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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
        public DataSet GetPreCloseDocRecBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreCloseDocRecBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetPreCloseDocHandOverBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreCloseDocHandOverBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetPenPreCloseDocHandOverBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenPreCloseDocHandOverBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetDocSendBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocSendBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataSet GetDocRecBySanctId(string pSanctionId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocRecBySanctId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanSancId", pSanctionId);
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
        public DataTable GetPenLegDtlByAppId(String pLnAppId, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLegDtlByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetQueryResponseByAppId(String pLnAppId, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetQueryResponseByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetQueryResolveByAppId(String pLnAppId, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetQueryResolveByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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

        public DataSet GetCustKYCByAppId(String pLnAppId)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustKYCByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetLegalQuariesByAppId(String pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLegalQuariesByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetLegalPassHistoryByAppId(String pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLegalPassHistoryByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetLnAppDetailsForFinalLeg(String pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLnAppDetailsForFinalLeg";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLnAppId);
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
        public DataTable GetPropertSchByAppId(String pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPropertSchByAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetCustNameByLnAppId(String pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCustNameByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetTaxRate()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetTaxRate";
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
        public DataTable GetDocType()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocType";
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
        public DataTable GetDocListByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocListByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetFinalLegDocListByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinalLegDocListByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetDocListFinalLegalByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocListFinalLegalByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetReceivedDocListByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReceivedDocListByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLnAppId);
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
        public DataTable CalculateEMI(decimal pLnAmt, decimal pRIntRate, decimal pInstNo, int pLoanTypeId, string pRepayType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CalculateEMI";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@LoanAmt", pLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@RIntRate", pRIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@InstallNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@LoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRepayType", pRepayType);
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
        public DataTable CalculateEMIRestructure(decimal pLnAmt, decimal pRIntRate, decimal pInstNo, int pLoanTypeId, string pPaySchedule)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CalculateEMI";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@LoanAmt", pLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pInterest", pRIntRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@InstallNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@LoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRepayType", pPaySchedule);
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
        /// <param name="pXmlData"></param>
        /// <param name="pCreatedBy"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pEntType"></param>
        /// <param name="pSynStatus"></param>
        /// <param name="pSanDt"></param>
        /// <returns></returns>
        public Int32 UpdateSanction(string pXmlData, Int32 pCreatedBy, string pBranchCode, string pEntType, Int32 pSynStatus, DateTime pSanDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateSanction";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", pSynStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSanDt", pSanDt);
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
        /// <param name="pFrmDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pMode"></param>
        /// <param name="pBranchCode"></param>
        /// <param name="pPgIndx"></param>
        /// <param name="pMaxRow"></param>
        /// <returns></returns>
        public DataTable GetSanctionList(DateTime pFrmDt, DateTime pToDt, string pMode, string pBranchCode, Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSanctionList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
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
        public DataTable GetFinalSanctionList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinalSanctionList";
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

        public DataTable GetFinalSancNotDisbList(Int32 pPgIndx, ref Int32 pMaxRow)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinalSancNotDisbList";
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

        public DataTable GetPenAgrVerList(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenAgrListNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetPenAgrPrintList(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenAgrPrintListNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetPenMODTList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenMODTList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenLOAList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLOAList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenLODList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLODList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenDocSendToCustodyList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenDocSendToCustodyList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenPreCloseDocRecByHOList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenPreCloseDocRecByHOList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenPreCloseDocHandOverList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenPreCloseDocHandOverList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenOrgDocSendList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenOrgDocSendList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenOrgDocRecList(string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenOrgDocRecList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetMODTList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMODTList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetLOAList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLOAList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetLODList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLODList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetDocSendToCustodyList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocSendToCustodyList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPreCloseDocRecByHOList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreCloseDocRecByHOList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetPreCloseDocHandOverList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPreCloseDocHandOverList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetOrgDocSendList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetOrgDocSendList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetOrgDocRecList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetOrgDocRecList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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
        public DataTable GetAgrList(DateTime pFrDt, DateTime pToDt, string pSearch, string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAgrList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrDate", pFrDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetPendingDisbList(int pPgIndx, ref int pMaxRow, DateTime pFromDt, DateTime pToDt, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenDisbList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pPgIndx", pPgIndx);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.BigInt, 4, "@pMaxRow", pMaxRow);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranchCode.Length +1, "@pBranchCode", pBranchCode);
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
        public DataTable GetPenLegList(string pMode, string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLegList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetPenResponseList(string pMode, string pBrCode, string pSearch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenResponseList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
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
        public DataTable GetPenResolveListNew(string pMode, string pBrCode, string pSearch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenResolveListNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pSearch", pSearch);
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
        public DataTable GetLegList(string pMode, string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPenLegList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetHOLegalList(string pBrCode, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHOLegalList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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
        public DataTable GetWriteOffDeclList(DateTime pFromDt, DateTime pToDate, string pLoanId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWriteOffDeclList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDate", pToDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
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
        public DataTable GetWOCollList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWOCollList";
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
        public DataTable GetLegalCust(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLegalCust";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetLegalCoApp(string pLnAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLegalCoApp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLnAppId);
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
        public DataTable GetWriteOffDtlByLoanId(string pLoanID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWriteOffDtlByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanID);
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
        public DataTable GetWOCollDtlByLoanId(string pLoanID, Int32 pSlNo)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetWOCollDtlByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
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
        public Int32 ChkEditSanction(string pLoanAppId, string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkEditSanction";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRet", 0);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToInt32(oCmd.Parameters["@pRet"].Value);
                return pRet;
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
        /// <param name="pMemberId"></param>
        /// <param name="pBranchCode"></param>
        /// <returns></returns>
        public DataTable GetLoansByMemberID(string pMemberId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoansByMemberID";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
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

        public Int32 ChkFormNo(string pAppFormNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkAppFormNo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pAppFormNo", pAppFormNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRet", 0);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToInt32(oCmd.Parameters["@pRet"].Value);
                return pRet;
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

        public DataTable GetSanctionList(DateTime pSanctionDt, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSanctionsByDate";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSanctionDt", pSanctionDt);
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

        public DataTable GetMemForBulkAppl(string pGroupId, DateTime pLogDate, string pBranchCode, string pMode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemForBulkAppl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", pGroupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogDate", pLogDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
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

        public int SaveExcellLnSanctionXML(string pXml, DateTime pCBDate, DateTime pLoginDate, string pBranchCode, int pCreatedBy, ref string pErrDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "HMXMLPurser";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDate", pLoginDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCBDate", pCBDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", pErrDesc);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                if (vErr == 0)
                {
                    return 0;
                }
                if (vErr == 9999)
                {
                    return 1;
                }
                if (vErr == 99999)
                {
                    return 3;
                }
                else
                {
                    return 2;
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

        public DataTable GetLnMinMax(string pAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMinLoanApp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@LoanAppId", pAppId);
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

        public int SaveTrnferToCBXml(string pXmlData, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveTrnfrCB";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXml", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
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

        public DataTable GetGroupByCenter(string pMarketId, string pBranchCode, DateTime pLogDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetGroupByCenter";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketId", pMarketId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLogDt", pLogDt);
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

        public DataTable GetMemberByGroup(string pGroupId, string pBranchCode, DateTime pLogDt)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberByGroup";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", pGroupId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLogDt", pLogDt);
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

        public DataTable CheckDisbBySancId(string pSancId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDisb";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSancId", pSancId);
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

        public Int32 SavePropertySchedule(Int32 pPcheduleId, string pLoanAppId, DateTime pDate, string pRemarks, string pNBoundary,
                                string pSBoundary, string pEBoundary, string pWBoundary, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePropertySchedule";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPcheduleId", pPcheduleId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pRemarks.Length + 1, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pNBoundary", pNBoundary);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pSBoundary", pSBoundary);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pEBoundary", pEBoundary);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pWBoundary", pWBoundary);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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
        public Int32 SaveLegalQuaries(Int32 pLegQuaryId, string pLoanAppId, DateTime pQueryGenDate, string pQ1, string pQ2,
                            string pQ3, string pQ4, string pQ5, Int32 pCreatedBy, string pMode, string pLegalQueryAppYN)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLegalQuaries";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLegQuaryId", pLegQuaryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pQueryGenDate", pQueryGenDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pQ1", pQ1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pQ2", pQ2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pQ3", pQ3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pQ4", pQ4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pQ5", pQ5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLegalQueryAppYN", pLegalQueryAppYN);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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
        public Int32 SaveLegalResponse(Int32 pLegQuaryId, string pLoanAppId, DateTime pResponseDate, string pA1, string pA2,
                       string pA3, string pA4, string pA5, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLegalBranchResponse";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLegQuaryId", pLegQuaryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pResponseDate", pResponseDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pA1", pA1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pA2", pA2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pA3", pA3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pA4", pA4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pA5", pA5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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
        public Int32 SaveLegalResolve(Int32 pLegQuaryId, string pLoanAppId, DateTime pResolveDate, string pQ1ResolveYN, string pQ2ResolveYN,
                      string pQ3ResolveYN, string pQ4ResolveYN, string pQ5ResolveYN, string pLegalFinalQueryResolve, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLegalResolve";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLegQuaryId", pLegQuaryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pResolveDate", pResolveDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ1ResolveYN", pQ1ResolveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ2ResolveYN", pQ2ResolveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ3ResolveYN", pQ3ResolveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ4ResolveYN", pQ4ResolveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ5ResolveYN", pQ5ResolveYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLegalFinalQueryResolve", pLegalFinalQueryResolve);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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
        public Int32 SaveDocListBulk(string pApplicationId, string pXml, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDocListBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
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
        public Int32 SaveDocListFinalLegalBulk(string pApplicationId, string pXml, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDocListFinalLegalBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
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
        public Int32 SaveDocReceiveListBulk(string pApplicationId, string pXml, Int32 pCreatedBy, string pMode,
            DateTime pLegOriginalDocRecDate, string pIsConfirmByLegalForDisb)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDocReceiveListBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLegOriginalDocRecDate", pLegOriginalDocRecDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsConfirmByLegalForDisb", pIsConfirmByLegalForDisb);
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
        public Int32 SavePreCloseDocRecBulk(string pSanctionId, string pApplicationId, string pXml, Int32 pCreatedBy, string pMode,
           DateTime pPreCloseDocRecDate)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreCloseDocRecBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPreCloseDocRecDate", pPreCloseDocRecDate);
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
        public Int32 SavePreCloseDocHandOver(string pSanctionId, string pApplicationId, string pXml, Int32 pCreatedBy, string pMode,
          DateTime pPreCloseDocHandOverDate)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreCloseDocHandOver";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPreCloseDocHandOverDate", pPreCloseDocHandOverDate);
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
        public Int32 SavePropertyDtlBulk(string pApplicationId, string pXml, string pMobStatus, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePropertyDtlBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicationId", pApplicationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXmlPropertyDtl", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", pMobStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
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
        public Int32 SaveLegalKYC(string pApplicantID, string pCustId, string pXmlCoApp, string pXmlApp, int pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLegalKYC";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicantID", pApplicantID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlCoApp.Length + 1, "@pXmlCoApp", pXmlCoApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlApp.Length + 1, "@pXmlApp", pXmlApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
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
        public Int32 SaveLegalPassHistory(Int32 pPassingHisId, string pLoanAppId, DateTime pEntryDate, string pPassHis1, string pPassHis2,
                         string pPassHis3, string pPassHis4, string pPassHis5, string pLegalNote1, string pLegalNote2, string pLegalNote3,
                         string pLegReport, string pPropOwnerNm, Int32 pCreatedBy, string pMode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLegalPassHistory";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPassingHisId", pPassingHisId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pEntryDate", pEntryDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pPassHis1", pPassHis1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pPassHis2", pPassHis2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pPassHis3", pPassHis3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pPassHis4", pPassHis4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pPassHis5", pPassHis5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pLegalNote1", pLegalNote1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pLegalNote2", pLegalNote2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pLegalNote3", pLegalNote3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 800, "@pLegalReport", pLegReport);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPropertyOwnerNm", pPropOwnerNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
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

        public DataTable GetMemberInfo(string pEnqId, string pType, string pCustType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberEquifaxInfo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pEnqId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pType", pType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCustType", pCustType);
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

        public int UpdateEquifaxInformation(string pLoanAppId, Int32 pCbId, string pEquifaxXML, string pBrCode, string pEoid, Int32 pCreatedBy, DateTime pDate, string pMode, string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateEquifaxInformation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 3, "@pCbId", pCbId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pErrorMsg", pErrorMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 500, "@pStatusDesc", pStatusDesc);
                DBUtility.Execute(oCmd);
                pStatus = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                pStatusDesc = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);
                return pStatus;
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

        public Int32 SaveLoanAppQA(string vLoanAppId, string vXml)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pRet = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanAppQA";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", vLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, vXml.Length + 1, "@pXml", vXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                pRet = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return pRet;
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

        public DataTable PopLoanApplication(string pLoanAppId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopLoanApplication";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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

        public DataTable GetMemberByLoanId(string pLoanAppId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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

        public DataTable GetMemberByMemberId(string pCustId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberByMemberId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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

        public DataTable GetInsuAmt(string pLoanAppId, Int32 pICID, decimal pLoanAmt, Int32 pInstNo)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetInsuAmt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pICID", pICID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 11, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pInstNo", pInstNo);
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

        public DataTable GetDeathMemberByLoanId(string pLoanId, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDeathMemberByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
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

        public DataTable GetDocDownloadData(string pBranchCode, DateTime pFrmDt, DateTime pToDt, string pSearch, string pType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocDownloadData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFrmDt", pFrmDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSearch", pSearch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
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

        public Int32 SaveDocsDownApprv(string pXmlData, int pCreatedBy, DateTime pDate)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDocsDownApprv";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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

        public Int32 SendBackHoDisb(string pSanctionId, int pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SendBackHoDisb";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 14, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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

        public Int32 DigiDocSendback(string pXmlData, Int32 pCreatedBy, DateTime pSendBackDt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DigiDocSendback";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pSendBackDt", pSendBackDt);
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

        #region  Send SMS DigitalConcent
        public string SendDigitalConcentSMS(string pMobileNo, string pSMSContent, string pSMSTemplateId, string pSMSLanguage)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            String url = "";
            try
            {
                string vMsgBody = pSMSContent;
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000204129";
                String passwd = "Unity@1122";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                if (pSMSLanguage.ToUpper() == "ENGLISH")
                {
                    vMsgBody = System.Web.HttpUtility.UrlEncode(vMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                    url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=" + pSMSTemplateId + "&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                }
                else
                {
                    url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=Unicode_text" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=" + pSMSTemplateId + "&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                }
                request = WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }
        #endregion

        #region Encode
        public string Encode(long i)
        {
            if (i == 0) return Alphabet[0].ToString();
            var s = string.Empty;
            while (i > 0)
            {
                var index = i % Base;
                s += Alphabet[(int)index];
                i = i / Base;
            }
            return string.Join(string.Empty, s.Reverse());
        }
        #endregion
    }
}
