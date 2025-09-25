using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FORCEDA;
using System.Data;
using System.Data.SqlClient;


namespace FORCEBA
{
    public class CReLoanCB
    {
        public DataTable GetReLoanCBData(string pBranch, Int32 pTenure, DateTime pDate, string pMemberId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReLoanCBData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pBranch.Length + 1, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMemberId", pMemberId);
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

        public DataTable GetReLoanMemberInfo(string pMemberId, Int32 pUserId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReLoanMemberInfo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", pUserId);
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

        public int SaveReLoanCbData(string pMemberId, string pEquifaxXML, string pEoid, Int32 pCreatedBy, DateTime pDate, string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveReLoanCbData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", @pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
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

        public int SaveReLoanCbData_CCR(string pMemberId, string pEquifaxXML, string pEoid, Int32 pCreatedBy, DateTime pDate, string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveReLoanCbData_CCR";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", @pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
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
         
        public DataTable GetReLoanCbRecord(DateTime pFromDate, DateTime pToDate, string pBranchCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetReLoanCbRecord";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", pFromDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", pToDate);
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

        public DataSet Equifax_Report_ReLoan(string pEnquiryId, Int32 pCBAppId, ref string pEnqStatusMsg)
        {

            SqlCommand oCmd = new SqlCommand();
            DataSet ds = null;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Equifax_Report_ReLoan";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 3, "@pCbId", pCBAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 500, "@pEnqStatusMsg", pEnqStatusMsg);
                ds = DBUtility.GetDataSet(oCmd);
                if (String.IsNullOrEmpty(oCmd.Parameters["@pEnqStatusMsg"].Value.ToString()))
                    pEnqStatusMsg = "";
                else
                    pEnqStatusMsg = oCmd.Parameters["@pEnqStatusMsg"].Value.ToString();
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

        public int SavePreApproveLoan(string pMemberId, string pEquifaxXML, string pEoid, Int32 pCreatedBy, DateTime pDate,
            string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc, string pBranchCode, int pLoanTypeId, int pTenure,
            double pLoanAmt, int pPurposeId, int pSubPurposeId, int pYrNo)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
            Int32 pErr;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreApproveLoan";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", @pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", pPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSubPurposeId", pSubPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pErrorMsg", pErrorMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 500, "@pStatusDesc", pStatusDesc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.Execute(oCmd);
                pStatus = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pStatusDesc = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);
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

        public int SavePreApproveLoan_CCR(string pMemberId, string pEquifaxXML, string pEoid, Int32 pCreatedBy, DateTime pDate,
            string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc, string pBranchCode, int pLoanTypeId, int pTenure,
            double pLoanAmt, int pPurposeId, int pSubPurposeId, int pYrNo)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
            Int32 pErr;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreApproveLoan_CCR";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", @pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", pPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSubPurposeId", pSubPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pErrorMsg", pErrorMsg);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pStatus", pStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 500, "@pStatusDesc", pStatusDesc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.Execute(oCmd);
                pStatus = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pStatusDesc = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);
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

        public string GetLastDayEndByBrCode(string pBranchCode)
        {
            SqlCommand oCmd = new SqlCommand();
            string pDate;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLastDayEndByBrCode";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pDate", "");
                DBUtility.Execute(oCmd);
                pDate = Convert.ToString(oCmd.Parameters["@pDate"].Value);
                return pDate;
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


        public int SavePreApprIniApp(string pMemberId,string pEoid, Int32 pCreatedBy, DateTime pDate,
         string pErrorMsg, string pBranchCode, int pLoanTypeId, int pTenure,
         double pLoanAmt, int pPurposeId, int pSubPurposeId, int pYrNo,ref int pCBId,ref string pEnquiryId)
        {
            SqlCommand oCmd = new SqlCommand();          
            Int32 pErr;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePreApprIniApp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", @pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLoanAmt", pLoanAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", pPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSubPurposeId", pSubPurposeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3000, "@pErrorMsg", pErrorMsg);               
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 8, "@pCBId", pCBId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 15, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", pYrNo);
                DBUtility.Execute(oCmd);
                pEnquiryId = Convert.ToString(oCmd.Parameters["@pEnquiryId"].Value);
                pCBId = Convert.ToInt32(oCmd.Parameters["@pCBId"].Value);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);              
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


        public int SaveCBCheckforLoanEligibility(string pLoanAccNo, string	pCustID, string	pAppName,DateTime pAppDOB, string pAppMob, string pAppGender, 
            string pApp1stKYCType,	string pApp1stKYCNo, string	pApp2ndKYCType, string	pApp2ndKYCNo,string	pAppAdd, string pAppCity, string pAppDistrict,
            string pAppVillage, string pAppState, string pAppPincode, string pCoAppName, DateTime pCoAppDOB, string pCoApp1stKYCType, string pCoApp1stKYCNo,
            string pCoAppAdd, string pCoAppCity, string pCoAppDistrict, string pCoAppVillage, string pCoAppState, string pCoAppPincode, ref int pCBId,
            ref string pEnquiryId, Int32 pCreatedBy, DateTime pDate)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 pErr;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveCBCheckforLoanEligibility";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLoanAccNo", pLoanAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppName", pAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAppDOB", pAppDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pAppMob", pAppMob);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pAppGender", pAppGender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pApp1stKYCType", pApp1stKYCType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pApp1stKYCNo", pApp1stKYCNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pApp2ndKYCType", pApp2ndKYCType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pApp2ndKYCNo", pApp2ndKYCNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pAppAdd", pAppAdd);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppCity", pAppCity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppDistrict", pAppDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppVillage", pAppVillage);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppState", pAppState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pAppPincode", pAppPincode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoAppName", pCoAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCoAppDOB", pCoAppDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCoApp1stKYCType", pCoApp1stKYCType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCoApp1stKYCNo", pCoApp1stKYCNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pCoAppAdd", pCoAppAdd);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoAppCity", pCoAppCity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoAppDistrict", pCoAppDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoAppVillage", pCoAppVillage);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoAppState", pCoAppState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoAppPincode", pCoAppPincode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 8, "@pCBId", pCBId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 15, "@pEnquiryId", pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.Execute(oCmd);
                pEnquiryId = Convert.ToString(oCmd.Parameters["@pEnquiryId"].Value);
                pCBId = Convert.ToInt32(oCmd.Parameters["@pCBId"].Value);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
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


        public DataTable GetInitialApprOthMemDtlForCB(string pEnqId, Int32 pCbId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetInitialApprOthMemDtlForCB";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
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

        public int UpdateEquifaxInformationOtherMem(string pEnqId, Int32 pCbId, string pEqXml, int pSlNo, 
            string pBranchCode, Int32 pCreatedBy, DateTime pDate, string pEoId, string pMemCategory)
        {
            int vErr2 = 0;
            SqlCommand oCmd2 = new SqlCommand();
            if (pEqXml.Equals(""))
            {
            }
            else
            {
                pEqXml = pEqXml.Replace("<?xml version=\"1.0\"?>", "").Trim();
                pEqXml = pEqXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                pEqXml = pEqXml.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
                oCmd2 = new SqlCommand();
                oCmd2.CommandType = CommandType.StoredProcedure;
                oCmd2.CommandText = "UpdateEquifaxInformationOtherMem";
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", pEoId);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", pDate);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 255, "@pStatusDesc", "");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@MemCategory", pMemCategory);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
                DBUtility.Execute(oCmd2);
                vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);

            }
            return vErr2;
        }

        public int UpdateEquifaxInforCoApp_CBCheck(string pEnqId, Int32 pCbId, string pEqXml, int pSlNo,
            string pBranchCode, Int32 pCreatedBy, DateTime pDate, string pMemCategory)
        {
            int vErr2 = 0;
            SqlCommand oCmd2 = new SqlCommand();
            if (pEqXml.Equals(""))
            {
            }
            else
            {
                pEqXml = pEqXml.Replace("<?xml version=\"1.0\"?>", "").Trim();
                pEqXml = pEqXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                pEqXml = pEqXml.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");
                oCmd2 = new SqlCommand();
                oCmd2.CommandType = CommandType.StoredProcedure;
                oCmd2.CommandText = "UpdateEquifaxInforCoApp_CBCheck";
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", pDate);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 255, "@pStatusDesc", "");
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@MemCategory", pMemCategory);
                DBUtility.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
                DBUtility.Execute(oCmd2);
                vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);

            }
            return vErr2;
        }

        public int UpdateEquifaxInformation_CBCheck(string pEnqId, Int32 pCbId, string pEquifaxXML, string pBrCode, Int32 pCreatedBy, DateTime pDate, string pMode, string pErrorMsg, ref Int32 pStatus, ref string pStatusDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            pEquifaxXML = pEquifaxXML.Replace("<?xml version=\"1.0\"?>", "").Trim();
            pEquifaxXML = pEquifaxXML.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
            pEquifaxXML = pEquifaxXML.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateEquifaxInformation_CBCheck";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnqId", pEnqId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 3, "@pCbId", pCbId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEquifaxXML.Length + 1, "@pEquifaxXML", pEquifaxXML);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
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

    }
}
