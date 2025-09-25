using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CentrumSaralMobService.Service_Equifax;
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CentrumSaralMobService
{
    public class CRepository
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly long Base = Alphabet.Length;
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        string vDBName = ConfigurationManager.AppSettings["DBName"];
        string PosidexEncURL = ConfigurationManager.AppSettings["PosidexEncURL"];
        string ProdYN = ConfigurationManager.AppSettings["ProdYN"];

        #region DATE_SET
        public DateTime setDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;
            DateTime dDate = System.DateTime.Now;
            if (pDate == "")
                dDate = Convert.ToDateTime("01/01/1900");
            else
            {
                if (pDate.Length == 9)
                    pDate = pDate.Insert(0, "0");

                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);
                dDate = Convert.ToDateTime(strDate);
            }
            return dDate;
        }
        public string ConMobDate(string pDate)
        {
            string nDate = "";
            if (pDate.Length > 0)
            {
                nDate = pDate.Substring(8, 2) + "/" + pDate.Substring(5, 2) + "/" + pDate.Substring(0, 4);
                return nDate;
            }
            else
            {
                nDate = "01/01/1900";
                return nDate;
            }
        }
        public DateTime setTime(string pTime)
        {
            DateTime vDate = Convert.ToDateTime("01/01/1900" + " " + pTime);
            return vDate;
        }

        public static string getFinYrNo(string pYear)
        {
            string vYrNo = "";
            if (pYear.Length == 1)
                vYrNo = pYear.Insert(0, "0");
            return (vYrNo);
        }
        #endregion

        #region GetMobUser
        public DataSet GetMobUser(UserData userData)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetUserDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUser", userData.pUserName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, userData.pPassword.Length + 1, "@pPass", userData.pPassword);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", userData.pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", userData.pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pImeiNo1", userData.pImeiNo1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pImeiNo2", userData.pImeiNo2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVersionCode", userData.pVersionCode);
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
        #endregion

        #region GetKarzaVoterErrCount
        public Int32 GetKarzaVoterErrCount(string pVoterId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetKarzaVoterErrCount";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pVoterId", pVoterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 3, "@pCount", vErr);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pCount"].Value);
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
        #endregion

        #region SaveKarzaVoterVerifyData
        public Int32 SaveKarzaVoterVerifyData(string pVoterId, string pResponseXml, string pBranchCode, string pEoid)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaVoterVerifyData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pVoterId", pVoterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
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
        #endregion

        #region SaveKarzaVoterVerifyErrLog
        public Int32 SaveKarzaVoterVerifyErrLog(string pVoterId, string pErrorCode)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaVoterVerifyErrLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pVoterId", pVoterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pErrorCode", pErrorCode);
                DBUtility.Execute(oCmd);
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
        #endregion

        #region SaveOCRLog
        public string SaveOCRLog(string vMobNo, string vEoid, string vBranchCode, string vResponseData)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveOCRLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMobileNo", vMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", vEoid);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", vBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, vResponseData.Length + 1, "@pResponseData", vResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pStatusCode", vStatusCode);
                DBUtility.Execute(oCmd);
                vStatusCode = Convert.ToString(oCmd.Parameters["@pStatusCode"].Value);
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

        #region SaveKarzaAadharToken
        public Int32 SaveKarzaAadharToken(string pAadharNo, string pTokenNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaAadharToken";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pAadharNo", pAadharNo);
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
        #endregion

        #region SaveKarzaMatchingDtl
        public Int32 SaveKarzaMatchingDtl(string pApiName, string pResponseXml, string pBranch, string pEoID, string pIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaMatchingDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
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
        #endregion

        #region SaveInitialApproach
        public string SaveInitialApproach(PostKYCSaveData postKYCSaveData)
        {
            SqlCommand oCmd1 = new SqlCommand();
            Int32 vErr1 = 0, pCbId = 0, pCoAppCBId = 0;
            string pErrDesc = "", pEnqId = "XX", vMemDob = string.Empty;
            vMemDob = postKYCSaveData.pDob.Substring(5, 2) + "/" + postKYCSaveData.pDob.Substring(8, 2) + "/" + postKYCSaveData.pDob.Substring(0, 4);
            try
            {
                oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd1.CommandText = "SaveInitialApproach";
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 16, "@pEnquiryId", postKYCSaveData.pEnquiryId);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCBId", Convert.ToInt32(postKYCSaveData.pCbId));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCoAppCBId", Convert.ToInt32(postKYCSaveData.pCoAppCBId));
                //DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanAmount", Convert.ToDouble(postKYCSaveData.pLoanAmount));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pFirstName", postKYCSaveData.pFirstName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMiddleName", postKYCSaveData.pMiddleName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLastName", postKYCSaveData.pLastName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", postKYCSaveData.pDob);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAGE", Convert.ToInt32(postKYCSaveData.pAge));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRelativeName", postKYCSaveData.pGuardianName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(postKYCSaveData.pGuardianRelation));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIdProofType", Convert.ToInt32(postKYCSaveData.pId1Type));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pIdProofNo", postKYCSaveData.pId1No);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType", Convert.ToInt32(postKYCSaveData.pId2Type));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo", postKYCSaveData.pId2No);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType1", Convert.ToInt32(postKYCSaveData.pAddressType1));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo1", postKYCSaveData.pHouseNo1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet1", postKYCSaveData.pStreet1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea1", postKYCSaveData.pArea1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pVillage1", postKYCSaveData.pVillage1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict1", postKYCSaveData.pSubDistrict1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pDistrict1", postKYCSaveData.pDistrict1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pState1", postKYCSaveData.pState1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark1", postKYCSaveData.pLandMark1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice1", postKYCSaveData.pPostOffice1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode1", postKYCSaveData.pPinCode1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo1", postKYCSaveData.pMobileNo1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmailId1", postKYCSaveData.pEmailId1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType2", Convert.ToInt32(postKYCSaveData.pAddressType2));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo2", postKYCSaveData.pHouseNo2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet2", postKYCSaveData.pStreet2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea2", postKYCSaveData.pArea2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVillageId2", Convert.ToInt32(postKYCSaveData.pVillageId2));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict2", postKYCSaveData.pSubDistrict2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark2", postKYCSaveData.pLandMark2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice2", postKYCSaveData.pPostOffice2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode2", postKYCSaveData.pPinCode2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo2", postKYCSaveData.pMobileNo2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postKYCSaveData.pBranchCode);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCSaveData.pEoId);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAadhaarScan", postKYCSaveData.pAadhaarScan);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", postKYCSaveData.pMode);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplicationType", postKYCSaveData.pApplicationType);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEnqType", postKYCSaveData.EnqType);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", postKYCSaveData.pOTP);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTimeStamp", postKYCSaveData.pTimeStamp);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOCRApproveYN", postKYCSaveData.pOCRApproveYN);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pKarzaVerifyYN", postKYCSaveData.pKarzaVerifyYN);
                DBUtility.Execute(oCmd1);
                vErr1 = Convert.ToInt32(oCmd1.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd1.Parameters["@pErrDesc"].Value);
                pEnqId = Convert.ToString(oCmd1.Parameters["@pEnquiryId"].Value);
                pCbId = Convert.ToInt32(oCmd1.Parameters["@pCBId"].Value);
                pCoAppCBId = Convert.ToInt32(oCmd1.Parameters["@pCoAppCBId"].Value);

                if (vErr1 == 0)
                {
                    return pErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
                if (vErr1 == 2)
                {
                    return pErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
                else
                {
                    return pErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
            }
            catch (Exception ex)
            {
                return "SUCCESS:Initial Approach for this member uploaded successfully." + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
            }
            finally
            {
                oCmd1.Dispose();
            }
        }
        #endregion

        #region UpdateEquifaxInformation
        public int UpdateEquifaxInformation(string pEnqId, int pCbId, string pEqXml, string pBranchCode, string pEoId, int pCreatedBy, string pDate, ref string pEquifaxResponse)
        {
            SqlCommand oCmd1 = new SqlCommand();
            int vErr = 0;
            oCmd1.CommandType = CommandType.StoredProcedure;
            oCmd1.CommandText = "UpdateEquifaxInformation";
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", pEoId);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", pDate);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
            DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 255, "@pStatusDesc", "");
            DBUtility.Execute(oCmd1);
            vErr = Convert.ToInt32(oCmd1.Parameters["@pStatus"].Value);
            pEquifaxResponse = Convert.ToString(oCmd1.Parameters["@pStatusDesc"].Value);
            return vErr;
        }
        #endregion

        #region SaveKarzaAadharVerifyData
        public string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseXml)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaAadharVerifyData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAadharNo", pAadharNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "Success:Record successfully saved.";
                }
                else
                {
                    return "Failed:Data not saved.";
                }
            }
            catch (Exception ex)
            {
                return "Failed:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region AadhaarVault Calling
        public AadhaarVaultResponse AadhaarVaultCalling(string pAadharNo, string pMobileNo, string pCreatedBy, out string pRequest, out string pResponse)
        {
            string vAadhaarRefNo = "";
            AadhaarVaultResponse vAadharVaultResponse = null;
            AadhaarVault vAadhaarVault = new AadhaarVault();
            vAadhaarVault.refData = pAadharNo;
            vAadhaarVault.refDataType = "U";
            vAadhaarVault.pMobileNo = pMobileNo;
            vAadhaarVault.pCreatedBy = pCreatedBy;
            vAadharVaultResponse = AadhaarVault(vAadhaarVault, out pRequest, out pResponse);
            //vAadhaarRefNo = Convert.ToString(vAadharVaultResponse.results[0]);
            return vAadharVaultResponse;
        }
        #endregion

        #region SaveKYC
        public string SaveKYC(PostKYCSaveData postKYCSaveData, PostOCRData postOCRData, PostEMOCRData postEMOCRData)
        {
            string pRequest = ""; string pResponse = "";
            SqlCommand oCmd1 = new SqlCommand();
            SqlCommand oCmd2 = new SqlCommand();
            Int32 vErr1 = 0, pCbId = 0, pCoAppCBId = 0;
            string pErrDesc = "", pEnqId = "XX", vMemDob = string.Empty, pCBErrDesc = "", pIsCBYN = "Y";
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "",
            vApplAadharNo = "", vApplAadhaarRefNo = "", vCoApplAadharNo = "", vCoApplAadhaarRefNo = "", vApplMaskedAadhaar = "", vCoApplMaskedAadhaar = "";

            if (postKYCSaveData.pXmlDirector == null)
            {
                postKYCSaveData.pXmlDirector = "";
            }

            #region AadharVault
            if (Convert.ToInt32(postKYCSaveData.pId1Type) == 1 || Convert.ToInt32(postKYCSaveData.pId2Type) == 1)
            {
                AadhaarVaultResponse vAadharVaultResponse = null;
                vApplAadharNo = Convert.ToInt32(postKYCSaveData.pId1Type) == 1 ? postKYCSaveData.pId1No : postKYCSaveData.pId2No;
                vApplMaskedAadhaar = String.Format("{0}{1}", "********", vApplAadharNo.Substring(vApplAadharNo.Length - 4, 4));
                vAadharVaultResponse = AadhaarVaultCalling(vApplAadharNo, postKYCSaveData.pMobileNo1, postKYCSaveData.pCreatedBy, out pRequest, out pResponse);
                vApplAadhaarRefNo = Convert.ToString(vAadharVaultResponse.results[0]);
                if (Convert.ToInt32(Convert.ToString(vAadharVaultResponse.response_code)) == 1)
                {

                    if (vApplAadhaarRefNo != "")
                    {
                        postKYCSaveData.pId1No = Convert.ToInt32(postKYCSaveData.pId1Type) == 1 ? vApplAadhaarRefNo : postKYCSaveData.pId1No;
                        postKYCSaveData.pId2No = Convert.ToInt32(postKYCSaveData.pId2Type) == 1 ? vApplAadhaarRefNo : postKYCSaveData.pId2No;
                        postKYCSaveData.pId1Type = Convert.ToInt32(postKYCSaveData.pId1Type) == 1 ? "13" : postKYCSaveData.pId1Type;
                        postKYCSaveData.pId2Type = Convert.ToInt32(postKYCSaveData.pId2Type) == 1 ? "13" : postKYCSaveData.pId2Type;
                    }
                    else
                    {
                        return "Failed:Unable to get aadhaar ref no." + ":" + pEnqId + ":" + pCbId;
                    }
                }
                else
                {
                    return ("Failed:Aadhaar Vault API Returns Error-" + "Request-" + pRequest + "Response-" + pResponse);
                }
            }

            if (Convert.ToInt32(postKYCSaveData.pCoAppIdentyPRofId) == 1)
            {
                AadhaarVaultResponse vAadharVaultResponse1 = null;
                vCoApplAadharNo = Convert.ToString(postKYCSaveData.pCoAppIdentyProfNo);
                vCoApplMaskedAadhaar = String.Format("{0}{1}", "********", vCoApplAadharNo.Substring(vCoApplAadharNo.Length - 4, 4));
                vAadharVaultResponse1 = AadhaarVaultCalling(vCoApplAadharNo, postKYCSaveData.pCoAppMobileNo, postKYCSaveData.pCreatedBy, out pRequest, out pResponse);
                vCoApplAadhaarRefNo = Convert.ToString(vAadharVaultResponse1.results[0]);
                if (Convert.ToInt32(Convert.ToString(vAadharVaultResponse1.response_code)) == 1)
                {
                    if (vCoApplAadhaarRefNo != "")
                    {
                        postKYCSaveData.pCoAppIdentyPRofId = "13";
                        postKYCSaveData.pCoAppIdentyProfNo = vCoApplAadhaarRefNo;
                    }
                    else
                    {
                        return "Failed:Unable to get aadhaar ref no." + ":" + pEnqId + ":" + pCbId;
                    }
                }
                else
                {

                    return ("Failed:Aadhaar Vault API Returns Error-" + "Request-" + pRequest + "Response-" + pResponse);

                }
            }
            #endregion

            try
            {
                oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd1.CommandText = "SaveInitialApproach";
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 16, "@pEnquiryId", postKYCSaveData.pEnquiryId);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCBId", Convert.ToInt32(postKYCSaveData.pCbId));
                //DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanAmount", Convert.ToDouble(postKYCSaveData.pLoanAmount));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pFirstName", postKYCSaveData.pFirstName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMiddleName", postKYCSaveData.pMiddleName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLastName", postKYCSaveData.pLastName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", postKYCSaveData.pDob);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAGE", Convert.ToInt32(postKYCSaveData.pAge));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRelativeName", postKYCSaveData.pGuardianName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(postKYCSaveData.pGuardianRelation));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIdProofType", Convert.ToInt32(postKYCSaveData.pId1Type));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pIdProofNo", postKYCSaveData.pId1No);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType", Convert.ToInt32(postKYCSaveData.pId2Type));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo", postKYCSaveData.pId2No);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType1", Convert.ToInt32(postKYCSaveData.pAddressType1));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo1", postKYCSaveData.pHouseNo1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet1", postKYCSaveData.pStreet1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea1", postKYCSaveData.pArea1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pVillage1", postKYCSaveData.pVillage1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict1", postKYCSaveData.pSubDistrict1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pDistrict1", postKYCSaveData.pDistrict1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pState1", postKYCSaveData.pState1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark1", postKYCSaveData.pLandMark1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice1", postKYCSaveData.pPostOffice1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode1", postKYCSaveData.pPinCode1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo1", postKYCSaveData.pMobileNo1);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType2", Convert.ToInt32(postKYCSaveData.pAddressType2));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo2", postKYCSaveData.pHouseNo2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet2", postKYCSaveData.pStreet2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea2", postKYCSaveData.pArea2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVillageId2", Convert.ToInt32(postKYCSaveData.pVillageId2));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict2", postKYCSaveData.pSubDistrict2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark2", postKYCSaveData.pLandMark2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice2", postKYCSaveData.pPostOffice2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode2", postKYCSaveData.pPinCode2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo2", postKYCSaveData.pMobileNo2);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postKYCSaveData.pBranchCode);

                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoAppName", postKYCSaveData.pCoAppName);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCoAppDOB", postKYCSaveData.pCoAppDOB);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppRelationId", Convert.ToInt32(postKYCSaveData.pCoAppRelationId));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pCoAppAddress", postKYCSaveData.pCoAppAddress);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCoAppState", postKYCSaveData.pCoAppState);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppPinCode", postKYCSaveData.pCoAppPinCode);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCoAppMobileNo", postKYCSaveData.pCoAppMobileNo);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppIdentyPRofId", Convert.ToInt32(postKYCSaveData.pCoAppIdentyPRofId));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCoAppIdentyProfNo", postKYCSaveData.pCoAppIdentyProfNo);


                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAadhaarScan", postKYCSaveData.pAadhaarScan);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", postKYCSaveData.pMode);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplicationType", postKYCSaveData.pApplicationType);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEnqType", postKYCSaveData.EnqType);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", postKYCSaveData.pOTP);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTimeStamp", postKYCSaveData.pTimeStamp);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOCRApproveYN", postKYCSaveData.pOCRApproveYN);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pKarzaVerifyYN", postKYCSaveData.pKarzaVerifyYN);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCoAppCBId", Convert.ToInt32(postKYCSaveData.pCoAppCBId));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postKYCSaveData.pEarningMemberXml.Length + 1, "@pEarningMemberXml", postKYCSaveData.pEarningMemberXml);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", Convert.ToInt32(postKYCSaveData.pLoanSchemeId));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 14, "@pEoId", postKYCSaveData.pEoId);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", postKYCSaveData.pMemberId);

                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pApplAadhaarNo", vApplMaskedAadhaar);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCoApplAadhaarNo", vCoApplMaskedAadhaar);

                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);

                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSelFeLatitude", postKYCSaveData.pSelFeLatitude);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSelFeLongitude", postKYCSaveData.pSelFeLongitude);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pIsCBYN", pIsCBYN);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postKYCSaveData.pXmlDirector.Length + 1, "@pXmlDirector", postKYCSaveData.pXmlDirector);

                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsAbledYN", postKYCSaveData.pIsAbledYN);
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSpeciallyAbled", Convert.ToInt32(postKYCSaveData.pSpeciallyAbled));
                DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTempId", postKYCSaveData.pTempId);
                DBUtility.Execute(oCmd1);
                vErr1 = Convert.ToInt32(oCmd1.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd1.Parameters["@pErrDesc"].Value);
                pEnqId = Convert.ToString(oCmd1.Parameters["@pEnquiryId"].Value);
                pCbId = Convert.ToInt32(oCmd1.Parameters["@pCBId"].Value);
                pCoAppCBId = Convert.ToInt32(oCmd1.Parameters["@pCoAppCBId"].Value);
                pIsCBYN = Convert.ToString(oCmd1.Parameters["@pIsCBYN"].Value);

                vDigiConcentSMS = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMS"].Value);
                vDigiConcentSMSTemplateId = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSTemplateId"].Value);
                vDigiConcentSMSLanguage = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSLanguage"].Value);

                if (vErr1 == 0)
                {
                    if ((postKYCSaveData.pOCRApproveYN).Trim() == "Y" && (postKYCSaveData.pKarzaVerifyYN).Trim() == "Y" && pIsCBYN.ToUpper() == "Y")
                    {
                        VerifyEquifax(pEnqId, "C", postKYCSaveData.pBranchCode, postKYCSaveData.pDate, postKYCSaveData.pCreatedBy, vCoApplAadharNo);
                        VerifyEquifaxEM(pEnqId, "E", postKYCSaveData.pBranchCode, postKYCSaveData.pDate, postKYCSaveData.pCreatedBy);
                        pCBErrDesc = VerifyEquifax(pEnqId, "A", postKYCSaveData.pBranchCode, postKYCSaveData.pDate, postKYCSaveData.pCreatedBy, vApplAadharNo);
                    }
                    else if (pIsCBYN.ToUpper() == "N")
                    {
                        pCBErrDesc = "Equifax Verification Successful;Equifax Success=Y, Approved = Y, Cancel = N";
                    }
                    else
                    {
                        pCBErrDesc = "CB verification hold for Karza verification.";
                    }

                    postOCRData.EnquiryId = pEnqId;
                    postEMOCRData.EnquiryId = pEnqId;
                    try
                    {
                        SaveOCRData(postOCRData);
                        SaveEMOCRData(postEMOCRData);
                    }
                    finally
                    {
                    }

                    try
                    {
                        if (vDigiConcentSMSLanguage.ToUpper() == "ENGLISH")
                        {
                            vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", "https://bijliftt.com/s/" + Encode(Convert.ToInt64(pEnqId)));
                        }
                        else
                        {
                            vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", HttpUtility.UrlEncode("https://bijliftt.com/s/" + Encode(Convert.ToInt64(pEnqId)), Encoding.GetEncoding("ISO-8859-1")));
                        }

                        if (vDigiConcentSMS.Length > 0)
                        {
                            vResultSendDigitalConcentSMS = SendDigitalConcentSMS(postKYCSaveData.pMobileNo1, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                        }
                    }
                    finally { }
                    return pErrDesc + ' ' + pCBErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
                if (vErr1 == 2)
                {
                    return pErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
                else
                {
                    return pErrDesc + ":" + pEnqId + ":" + pCbId + ":" + pCoAppCBId;
                }
            }
            catch (Exception ex)
            {
                return "FAIL:Initial Approach for this member upload Failed." + ":" + pEnqId + ":" + ":" + pCbId + ":" + pCoAppCBId;
            }
            finally
            {
                oCmd1.Dispose();
                oCmd2.Dispose();
            }
        }
        #endregion

        #region GetMemberInfo
        public DataTable GetMemberInfo(string pCustId, string pType, string pCustType)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberEquifaxInfo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnqId", pCustId);
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
        #endregion

        #region VerifyEquifax
        public string 
            VerifyEquifax(string pEnqId, string pCustType, string pBranchCode, string pLogDt, string pCreatedBy, string pAadhaarNo)
        {
            DataTable dt = new DataTable();
            string vMsg = "", pStatusDesc = "";
            string pEqXml = "", CCRUserName = "", CCRPassword = "";
            int vErr = 0, pStatus = 0;
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];
            try
            {
                //pMode = "A"-Customer/Applicant,"C"-CoApplicant,"E"- Earning Member
                dt = GetMemberInfo(pEnqId, "E", pCustType);
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        ////*************************** For Live ***************************************************                      
                        WebServiceSoapClient eq = new WebServiceSoapClient();
                        ////************************************************GenderId 1 For MALE else Female****************************************                     
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                            dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(),
                            dt.Rows[0]["IDType"].ToString() == "AADHAAR" ? pAadhaarNo : dt.Rows[0]["IDValue"].ToString(),
                            dt.Rows[0]["AddType"].ToString(), dt.Rows[0]["AddType"].ToString() == "AADHAAR" ? pAadhaarNo : dt.Rows[0]["AddValue"].ToString(),
                            dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(), "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7",
                            "123456", "CCR", "ERS", "3.1", "PRO");
                        //*************************************************************************
                        vErr = UpdateEquifaxInformation(pEnqId, Convert.ToInt32(dt.Rows[0]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), pLogDt, ref pStatusDesc);
                        if (vErr == 1)
                        {
                            return "Equifax Verification Successful" + ";" + pStatusDesc;
                        }
                        else if (vErr == 5)
                        {
                            return "Equifax Verification Failed" + ";" + pStatusDesc;
                        }
                        else
                        {
                            return "Equifax Verification Failed" + ";" + pStatusDesc;
                        }
                    }
                    catch (Exception ex)
                    {
                        vErr = UpdateEquifaxInformation(pEnqId, Convert.ToInt32(dt.Rows[0]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), pLogDt, ref pStatusDesc);
                        return "Equifax Verification Failed" + ":" + ex.ToString();
                    }
                }
                else
                {
                    vMsg = "No Data Found..!!";
                }
            }
            catch (Exception ex)
            {
                vMsg = ex.Message;
            }
            return vMsg;
        }
        #endregion

        #region VerifyEquifaxEM
        public string VerifyEquifaxEM(string pEnqId, string pCustType, string pBranchCode, string pLogDt, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            string vMsg = "", pStatusDesc = "", vErrDesc = "";
            string pEqXml = "", CCRUserName = "", CCRPassword = "";
            int vErr = 0;
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];
            try
            {
                //pMode = "A"-Customer/Applicant,"C"-CoApplicant,"E"- Earning Member
                dt = GetMemberInfo(pEnqId, "E", pCustType);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        try
                        {
                            WebServiceSoapClient eq = new WebServiceSoapClient();
                            pEqXml = eq.Equifax(dt.Rows[i]["FirstName"].ToString(), dt.Rows[i]["MiddleName"].ToString(), dt.Rows[i]["LastName"].ToString(), dt.Rows[i]["DOB"].ToString()
                                , dt.Rows[i]["AddressType"].ToString(), dt.Rows[i]["AddressLine1"].ToString(), dt.Rows[i]["StateName"].ToString(), dt.Rows[i]["PIN"].ToString(),
                                 dt.Rows[i]["MobileNo"].ToString(), dt.Rows[i]["IDType"].ToString(), dt.Rows[i]["IDValue"].ToString(), dt.Rows[i]["AddType"].ToString(),
                                  dt.Rows[i]["AddValue"].ToString(), dt.Rows[i]["CoAppRel"].ToString(), dt.Rows[i]["CoAppName"].ToString(),
                                   "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                            vErr = UpdateEquifaxInformation(pEnqId, Convert.ToInt32(dt.Rows[i]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), pLogDt, ref pStatusDesc);
                            if (vErr == 1)
                            {
                                vErrDesc = vErrDesc + "Equifax Verification Successful" + ":" + pStatusDesc;
                            }
                            else if (vErr == 5)
                            {
                                vErrDesc = vErrDesc + "Equifax Verification Failed" + ":" + pStatusDesc;
                            }
                            else
                            {
                                vErrDesc = vErrDesc + "Equifax Verification Failed" + ":" + pStatusDesc;
                            }
                        }
                        catch (Exception ex)
                        {
                            vErr = UpdateEquifaxInformation(pEnqId, Convert.ToInt32(dt.Rows[i]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), pLogDt, ref pStatusDesc);
                            vErrDesc = vErrDesc + "Equifax Verification Failed" + ":" + ex.ToString();
                        }
                    }
                    return vErrDesc;
                }
                else
                {
                    vMsg = "No Data Found..!!";
                }
            }
            catch (Exception ex)
            {
                vMsg = ex.Message;
            }
            return vMsg;
        }
        #endregion

        #region GetKYCInfo
        public DataTable GetKYCInfo(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<KYCData> row = new List<KYCData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetListItem";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postKYCData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", postKYCData.pDate);
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

        #region GetAddressDtl
        public DataTable GetAddressDtl(PostAddressData postAddressData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetAddressDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", postAddressData.pBranch);
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

        #region GetStateList
        public DataTable GetStateList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetStateList";
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

        #region GetMemberDtlById
        public DataSet GetMemberDtlById(string pMemberId)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetMemberDtlById";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
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
        #endregion

        #region SaveOCRData
        public string SaveOCRData(PostOCRData vOCRData)
        {
            string xmlID1AadharFront, xmlID1AadharBack, xmlID1VoterFront, xmlID1VoterBack, xmlID1AadharResponse, xmlID1VoterResponse,
                xmlID2AadharFront, xmlID2AadharBack, xmlID2VoterFront, xmlID2VoterBack, xmlID2AadharResponse, xmlID2VoterResponse,
                xmlNameMatchingResponse, xmlAddressMatchingResponse, xmlFaceMatchingResponse, xmlCoApplIDAadharFront, xmlCoApplIDAadharBack,
                xmlCoApplIDVoterFront, xmlCoApplIDVoterBack, xmlCoApplID1AadharResponse, xmlCoApplID1VoterResponse, xmlCoApplNameMatchingResponse,
                xmlCoApplAddressMatchingResponse, xmlCoApplFaceMatchingResponse;

            string vID1AadharFront, vID1AadharBack, vID1VoterFront, vID1VoterBack, vID1AadharResponse, vID1VoterResponse,
                vID2AadharFront, vID2AadharBack, vID2VoterFront, vID2VoterBack, vID2AadharResponse, vID2VoterResponse,
                vCoApplIDAadharFront, vCoApplIDAadharBack, vCoApplIDVoterFront, vCoApplIDVoterBack,
                vCoApplID1AadharResponse, vCoApplID1VoterResponse;

            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                vID1AadharFront = vOCRData.ID1AadharFront == null ? "" : vOCRData.ID1AadharFront.Replace("\u0000", "");
                vID1AadharFront = vID1AadharFront.Replace("\\u0000", "");
                xmlID1AadharFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID1AadharFront), "root"));

                vID1AadharBack = vOCRData.ID1AadharBack == null ? "" : vOCRData.ID1AadharBack.Replace("\u0000", "");
                vID1AadharBack = vID1AadharBack.Replace("\\u0000", "");
                xmlID1AadharBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID1AadharBack), "root"));


                vID1VoterFront = vOCRData.ID1VoterFront == null ? "" : vOCRData.ID1VoterFront.Replace("\u0000", "");
                vID1VoterFront = vID1VoterFront.Replace("\\u0000", "");
                xmlID1VoterFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID1VoterFront), "root"));

                vID1VoterBack = vOCRData.ID1VoterBack == null ? "" : vOCRData.ID1VoterBack.Replace("\u0000", "");
                vID1VoterBack = vID1VoterBack.Replace("\\u0000", "");
                xmlID1VoterBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID1VoterBack), "root"));


                vID1AadharResponse = vOCRData.ID1AadharResponse == null ? "" : vOCRData.ID1AadharResponse.Replace("\u0000", "");
                vID1AadharResponse = vID1AadharResponse.Replace("\\u0000", "");
                xmlID1AadharResponse = AsString(JsonConvert.DeserializeXmlNode(vID1AadharResponse, "root"));

                vID1VoterResponse = vOCRData.ID1VoterResponse == null ? "" : vOCRData.ID1VoterResponse.Replace("\u0000", "");
                vID1VoterResponse = vID1VoterResponse.Replace("\\u0000", "");
                xmlID1VoterResponse = AsString(JsonConvert.DeserializeXmlNode(vID1VoterResponse, "root"));

                vID2AadharFront = vOCRData.ID2AadharFront == null ? "" : vOCRData.ID2AadharFront.Replace("\u0000", "");
                vID2AadharFront = vID2AadharFront.Replace("\\u0000", "");
                xmlID2AadharFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID2AadharFront), "root"));

                vID2AadharBack = vOCRData.ID2AadharBack == null ? "" : vOCRData.ID2AadharBack.Replace("\u0000", "");
                vID2AadharBack = vID2AadharBack.Replace("\\u0000", "");
                xmlID2AadharBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID2AadharBack), "root"));

                vID2VoterFront = vOCRData.ID2VoterFront == null ? "" : vOCRData.ID2VoterFront.Replace("\u0000", "");
                vID2VoterFront = vID2VoterFront.Replace("\\u0000", "");
                xmlID2VoterFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID2VoterFront), "root"));

                vID2VoterBack = vOCRData.ID2VoterBack == null ? "" : vOCRData.ID2VoterBack.Replace("\u0000", "");
                vID2VoterBack = vID2VoterBack.Replace("\\u0000", "");
                xmlID2VoterBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vID2VoterBack), "root"));

                vID2AadharResponse = vOCRData.ID2AadharResponse == null ? "" : vOCRData.ID2AadharResponse.Replace("\u0000", "");
                vID2AadharResponse = vID2AadharResponse.Replace("\\u0000", "");
                xmlID2AadharResponse = AsString(JsonConvert.DeserializeXmlNode(vID2AadharResponse, "root"));

                vID2VoterResponse = vOCRData.ID2VoterResponse == null ? "" : vOCRData.ID2VoterResponse.Replace("\u0000", "");
                vID2VoterResponse = vID2VoterResponse.Replace("\\u0000", "");
                xmlID2VoterResponse = AsString(JsonConvert.DeserializeXmlNode(vID2VoterResponse, "root"));

                vCoApplIDAadharFront = vOCRData.CoApplIDAadharFront == null ? "" : vOCRData.CoApplIDAadharFront.Replace("\u0000", "");
                vCoApplIDAadharFront = vCoApplIDAadharFront.Replace("\\u0000", "");
                xmlCoApplIDAadharFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoApplIDAadharFront), "root"));

                vCoApplIDAadharBack = vOCRData.CoApplIDAadharBack == null ? "" : vOCRData.CoApplIDAadharBack.Replace("\u0000", "");
                vCoApplIDAadharBack = vCoApplIDAadharBack.Replace("\\u0000", "");
                xmlCoApplIDAadharBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoApplIDAadharBack), "root"));

                vCoApplIDVoterFront = vOCRData.CoApplIDVoterFront == null ? "" : vOCRData.CoApplIDVoterFront.Replace("\u0000", "");
                vCoApplIDVoterFront = vCoApplIDVoterFront.Replace("\\u0000", "");
                xmlCoApplIDVoterFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoApplIDVoterFront), "root"));


                vCoApplIDVoterBack = vOCRData.CoApplIDVoterBack == null ? "" : vOCRData.CoApplIDVoterBack.Replace("\u0000", "");
                vCoApplIDVoterBack = vCoApplIDVoterBack.Replace("\\u0000", "");
                xmlCoApplIDVoterBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoApplIDVoterBack), "root"));

                vCoApplID1AadharResponse = vOCRData.CoAppID1AadharResponse == null ? "" : vOCRData.CoAppID1AadharResponse.Replace("\u0000", "");
                vCoApplID1AadharResponse = vCoApplID1AadharResponse.Replace("\\u0000", "");
                xmlCoApplID1AadharResponse = AsString(JsonConvert.DeserializeXmlNode(vCoApplID1AadharResponse, "root"));

                vCoApplID1VoterResponse = vOCRData.CoAppID1VoterResponse == null ? "" : vOCRData.CoAppID1VoterResponse.Replace("\u0000", "");
                vCoApplID1VoterResponse = vCoApplID1VoterResponse.Replace("\\u0000", "");
                xmlCoApplID1VoterResponse = AsString(JsonConvert.DeserializeXmlNode(vCoApplID1VoterResponse, "root"));


                xmlNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.NameMatchingResponse == null ? "" : vOCRData.NameMatchingResponse, "root"));
                xmlAddressMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.AddressMatchingResponse == null ? "" : vOCRData.AddressMatchingResponse, "root"));
                xmlFaceMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.FaceMatchingResponse == null ? "" : vOCRData.FaceMatchingResponse, "root"));


                xmlCoApplNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.CoAppNameMatchingResponse == null ? "" : vOCRData.CoAppNameMatchingResponse, "root"));
                xmlCoApplAddressMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.CoAppAddressMatchingResponse == null ? "" : vOCRData.CoAppAddressMatchingResponse, "root"));
                xmlCoApplFaceMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.CoAppFaceMatchingResponse == null ? "" : vOCRData.CoAppFaceMatchingResponse, "root"));
                //------------------------------------------------------------------------
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveOCRData";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", vOCRData.EnquiryId == null ? "" : vOCRData.EnquiryId);
                if (xmlID1AadharFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1AadharFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1AadharFront.Length + 1, "@pID1AadharFront", xmlID1AadharFront);

                if (xmlID1AadharBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1AadharBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1AadharBack.Length + 1, "@pID1AadharBack", xmlID1AadharBack);

                if (xmlID1VoterFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1VoterFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1VoterFront.Length + 1, "@pID1VoterFront", xmlID1VoterFront);

                if (xmlID1VoterBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1VoterBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1VoterBack.Length + 1, "@pID1VoterBack", xmlID1VoterBack);

                if (xmlID1AadharResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1AadharResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1AadharResponse.Length + 1, "@pID1AadharResponse", xmlID1AadharResponse);
                if (xmlID1VoterResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID1VoterResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1VoterResponse.Length + 1, "@pID1VoterResponse", xmlID1VoterResponse);

                if (xmlID2AadharFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2AadharFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2AadharFront.Length + 1, "@pID2AadharFront", xmlID2AadharFront);

                if (xmlID2AadharBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2AadharBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2AadharBack.Length + 1, "@pID2AadharBack", xmlID2AadharBack);
                if (xmlID2VoterFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2VoterFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2VoterFront.Length + 1, "@pID2VoterFront", xmlID2VoterFront);

                if (xmlID2VoterBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2VoterBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2VoterBack.Length + 1, "@pID2VoterBack", xmlID2VoterBack);

                if (xmlID2AadharResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2AadharResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2AadharResponse.Length + 1, "@pID2AadharResponse", xmlID2AadharResponse);

                if (xmlID2VoterResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pID2VoterResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID2VoterResponse.Length + 1, "@pID2VoterResponse", xmlID2VoterResponse);

                if (xmlNameMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pNameMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlNameMatchingResponse.Length + 1, "@pNameMatchingResponse", xmlNameMatchingResponse);

                if (xmlAddressMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pAddressMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlAddressMatchingResponse.Length + 1, "@pAddressMatchingResponse", xmlAddressMatchingResponse);

                if (xmlFaceMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pFaceMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlFaceMatchingResponse.Length + 1, "@pFaceMatchingResponse", xmlFaceMatchingResponse);

                if (xmlCoApplIDAadharFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplIDAadharFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplIDAadharFront.Length + 1, "@pCoApplIDAadharFront", xmlCoApplIDAadharFront);

                if (xmlCoApplIDAadharBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplIDAadharBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplIDAadharBack.Length + 1, "@pCoApplIDAadharBack", xmlCoApplIDAadharBack);

                if (xmlCoApplIDVoterFront == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplIDVoterFront", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplIDVoterFront.Length + 1, "@pCoApplIDVoterFront", xmlCoApplIDVoterFront);

                if (xmlCoApplIDVoterBack == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplIDVoterBack", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplIDVoterBack.Length + 1, "@pCoApplIDVoterBack", xmlCoApplIDVoterBack);


                if (xmlCoApplID1AadharResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplID1AadharResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplID1AadharResponse.Length + 1, "@pCoApplID1AadharResponse", xmlCoApplID1AadharResponse);
                if (xmlCoApplID1VoterResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplID1VoterResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplID1VoterResponse.Length + 1, "@pCoApplID1VoterResponse", xmlCoApplID1VoterResponse);

                if (xmlCoApplNameMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplNameMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplNameMatchingResponse.Length + 1, "@pCoApplNameMatchingResponse", xmlCoApplNameMatchingResponse);

                if (xmlCoApplAddressMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplAddressMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplAddressMatchingResponse.Length + 1, "@pCoApplAddressMatchingResponse", xmlCoApplAddressMatchingResponse);

                if (xmlCoApplFaceMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 6, "@pCoApplFaceMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlCoApplFaceMatchingResponse.Length + 1, "@pCoApplFaceMatchingResponse", xmlCoApplFaceMatchingResponse);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "Record saved successfully";
                }
                else
                {
                    return "Data Not Saved";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

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

        public string GetSerializeJson(string pJson)
        {
            if (pJson != "")
            {
                string vJson = (pJson.Replace("\u0000", "")).Replace("\\u0000", "");
                dynamic op = JsonConvert.DeserializeObject(vJson);
                var data = JsonConvert.DeserializeObject(op.OCRPhototoDataResult[0].ResponseString.Value);
                var status = op.OCRPhototoDataResult[0].Responsestatus;
                object obj = new { ResponseString = data, Responsestatus = status };
                return JsonConvert.SerializeObject(obj);
            }
            else
            {
                return "";
            }
        }

        #region SaveEMOCRData
        public string SaveEMOCRData(PostEMOCRData postEMOCRData)
        {
            string vErrMsg = "";
            var vEmOCR = JsonConvert.DeserializeObject<List<EMOCRData>>(postEMOCRData.EMOCRData);
            string vEnqId = postEMOCRData.EnquiryId == null ? "" : postEMOCRData.EnquiryId;
            foreach (var data in vEmOCR)
            {
                string vEmId = data.EMID == null ? "0" : data.EMID;
                string vIDFront = data.IDFront == null ? "" : data.IDFront;
                string vIDBack = data.IDBack == null ? "" : data.IDBack;
                string xmlIDFront, xmlIDBack;
                xmlIDFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vIDFront), "root"));
                xmlIDBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vIDBack), "root"));
                SqlCommand oCmd = new SqlCommand();
                Int32 vErr = 0;
                try
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "SaveEMOCRData";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", vEnqId);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pEMID", Convert.ToInt32(vEmId));
                    if (xmlIDFront == null)
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pIdFront", DBNull.Value);
                    else
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlIDFront.Length + 1, "@pIdFront", xmlIDFront);
                    if (xmlIDBack == null)
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pIdBack", DBNull.Value);
                    else
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlIDBack.Length + 1, "@pIdBack", xmlIDBack);

                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBUtility.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                    {
                        vErrMsg = vErrMsg + "Success:" + vEmId;
                    }
                    else
                    {
                        vErrMsg = vErrMsg + "Failed:" + vEmId;
                    }
                }
                catch (Exception ex)
                {
                    vErrMsg = vErrMsg + "Failed:" + vEmId;
                }
                finally
                {
                    oCmd.Dispose();
                }
            }
            return vErrMsg;
        }
        #endregion

        #endregion

        #region GetLoanType
        public DataTable GetLoanType(PostSchemeData postSchemeData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanTypeForApp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postSchemeData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsTopup", postSchemeData.pIsTopup);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", postSchemeData.pUserId);
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

        #region MemberCreationData
        public DataTable GetMemberCreationData(PostMemberData postMemberData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetMember";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMemberData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postMemberData.pEoId);
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

        #region GetIniApprMemDtlByEnqId
        public DataTable GetIniApprMemDtlByEnqId(PostMemberFormData postMemberFormData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetIniApprMemDtlByEnqId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", postMemberFormData.pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", postMemberFormData.pUserId);
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

        #region Save PD
        public string SavePdBySO(PostPdBySo postPdBySO)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vPDId = "", pErrDesc = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePdBySO";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", postPdBySO.pEnquiryId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 12, "@pMemberId", postPdBySO.pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", Convert.ToInt32(postPdBySO.pPurposeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pExpLoanAmt", Convert.ToDouble(postPdBySO.pExpLoanAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pExpLoanTenure", Convert.ToInt32(postPdBySO.pExpLoanTenure));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmiPayingCapacity", Convert.ToInt32(postPdBySO.pEmiPayingCapacity));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pExistingLoanNo", Convert.ToInt32(postPdBySO.pExistingLoanNo));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnOS", Convert.ToDouble(postPdBySO.pTotLnOS));

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplTitle", postPdBySO.pApplTitle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplFName", postPdBySO.pApplFName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplMName", postPdBySO.pApplMName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplLName", postPdBySO.pApplLName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplGender", postPdBySO.pApplGender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pApplMaritalStatus", postPdBySO.pApplMaritalStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplEduId", Convert.ToInt32(postPdBySO.pApplEduId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplReliStat", Convert.ToInt32(postPdBySO.pApplReliStat));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplReligion", Convert.ToInt32(postPdBySO.pApplReligion));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplCaste", Convert.ToInt32(postPdBySO.pApplCaste));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPerAddrType", Convert.ToInt32(postPdBySO.pApplPerAddrType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pApplPerHouseNo", postPdBySO.pApplPerHouseNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerStreet", postPdBySO.pApplPerStreet);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerLandmark", postPdBySO.pApplPerLandmark);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerArea", postPdBySO.pApplPerArea);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerVillage", postPdBySO.pApplPerVillage);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerSubDist", postPdBySO.pApplPerSubDist);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPerPostOffice", postPdBySO.pApplPerPostOffice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pApplPerPIN", postPdBySO.pApplPerPIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pApplPerDist", postPdBySO.pApplPerDist);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPerStateId", Convert.ToInt32(postPdBySO.pApplPerStateId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pApplPerContactNo", postPdBySO.pApplPerContactNo);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPreAddrType", postPdBySO.pApplPreAddrType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pApplPreHouseNo", postPdBySO.pApplPreHouseNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPreStreet", postPdBySO.pApplPreStreet);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPreLandmark", postPdBySO.pApplPreLandmark);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPreArea", postPdBySO.pApplPreArea);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPreVillageId", Convert.ToInt32(postPdBySO.pApplPreVillageId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPreSubDist", postPdBySO.pApplPreSubDist);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPrePostOffice", postPdBySO.pApplPrePostOffice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pApplPrePIN", postPdBySO.pApplPrePIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplPreDistId", postPdBySO.pApplPreDistId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPreStateId", Convert.ToInt32(postPdBySO.pApplPreStateId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplMobileNo", postPdBySO.pApplMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pApplAddiContactNo", postPdBySO.pApplAddiContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pApplPhyFitness", Convert.ToInt32(postPdBySO.pApplPhyFitness));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoApplTitle", postPdBySO.pCoApplTitle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoApplName", postPdBySO.pCoApplName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCoApplDOB", postPdBySO.pCoApplDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoApplAge", Convert.ToInt32(postPdBySO.pCoApplAge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pCoApplGender", postPdBySO.pCoApplGender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pCoApplMaritalStatus", postPdBySO.pCoApplMaritalStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoApplRelation", Convert.ToInt32(postPdBySO.pCoApplRelation));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoApplEduId", Convert.ToInt32(postPdBySO.pCoApplEduId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 350, "@pCoApplPerAddr", postPdBySO.pCoApplPerAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoApplPerStateId", Convert.ToInt32(postPdBySO.pCoApplPerStateId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoApplPerPIN", postPdBySO.pCoApplPerPIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCoApplMobileNo", postPdBySO.pCoApplMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCoApplAddiContactNo", postPdBySO.pCoApplAddiContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoApplPhyFitness", Convert.ToInt32(postPdBySO.pCoApplPhyFitness));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pACHolderName", postPdBySO.pACHolderName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pMFHName", postPdBySO.pFamilyPersonName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMFHRelation", postPdBySO.pRelationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccNo", postPdBySO.pAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIfscCode", postPdBySO.pIfscCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pAccType", postPdBySO.pAccType);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoAppIncYN", postPdBySO.pCoAppIncYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pTypeOfInc", Convert.ToInt32(postPdBySO.pTypeOfInc));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pAgeOfKeyIncEar", Convert.ToInt32(postPdBySO.pAgeOfKeyIncEar));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pAnnulInc", Convert.ToInt32(postPdBySO.pAnnulInc));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pHouseStability", Convert.ToInt32(postPdBySO.pHouseStability));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pTypeOfOwnerShip", Convert.ToInt32(postPdBySO.pTypeOfOwnerShip));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pTypeOfResi", Convert.ToInt32(postPdBySO.pTypeOfResi));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pResiCategory", postPdBySO.pResiCategory);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pTotalFamMember", Convert.ToInt32(postPdBySO.pTotalFamMember));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pNoOfChild", Convert.ToInt32(postPdBySO.pNoOfChild));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pNoOfDependent", Convert.ToInt32(postPdBySO.pNoOfDependent));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pNoOfFamEarMember", Convert.ToInt32(postPdBySO.pNoOfFamEarMember));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFamilyAsset", postPdBySO.pFamilyAsset);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOtherAsset", postPdBySO.pOtherAsset);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLandHolding", Convert.ToInt32(postPdBySO.pLandHolding));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBankingHabit", Convert.ToInt32(postPdBySO.pBankingHabit));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOtherSavings", postPdBySO.pOtherSavings);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPersonalRef", postPdBySO.pPersonalRef);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 350, "@pAddr", postPdBySO.pAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMobileNo", postPdBySO.pMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pValidatedYN", postPdBySO.pValidatedYN);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMobilePhone", postPdBySO.pMobilePhone);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pRefrigerator", postPdBySO.pRefrigerator);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTwoWheeler", postPdBySO.pTwoWheeler);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pThreeWheeler", postPdBySO.pThreeWheeler);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pFourWheeler", postPdBySO.pFourWheeler);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAirConditioner", postPdBySO.pAirConditioner);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pWashingMachine", postPdBySO.pWashingMachine);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pEmailId", postPdBySO.pEmailId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pPAN", postPdBySO.pPAN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pGSTno", postPdBySO.pGSTno);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pITR", postPdBySO.pITR);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pWhatsapp", postPdBySO.pWhatsapp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pFacebookAc", postPdBySO.pFacebookAc);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pBusinessName", postPdBySO.pBusinessName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pBusiNameOnBoard", postPdBySO.pBusiNameOnBoard);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPrimaryBusiType", Convert.ToInt32(postPdBySO.pPrimaryBusiType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPrimaryBusiSeaso", postPdBySO.pPrimaryBusiSeaso);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPrimaryBusiSubType", Convert.ToInt32(postPdBySO.pPrimaryBusiSubType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPrimaryBusiActivity", Convert.ToInt32(postPdBySO.pPrimaryBusiActivity));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pWorkingDays", Convert.ToInt32(postPdBySO.pWorkingDays));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMonthlyTrunOver", Convert.ToInt32(postPdBySO.pMonthlyTrunOver));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLocalityArea", Convert.ToInt32(postPdBySO.pLocalityArea));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pBusiEstdDt", postPdBySO.pBusiEstdDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pBusinessAddr", postPdBySO.pBusinessAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pOtherBusinessProof", postPdBySO.pOtherBusinessProof);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessVintage", Convert.ToInt32(postPdBySO.pBusinessVintage));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pBusiOwnerType", postPdBySO.pBusiOwnerType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pBusiHndlPerson", postPdBySO.pBusiHndlPerson);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pPartnerYN", postPdBySO.pPartnerYN);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfEmp", Convert.ToInt32(postPdBySO.pNoOfEmp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pValueOfStock", Convert.ToInt32(postPdBySO.pValueOfStock));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pValueOfMachinery", Convert.ToInt32(postPdBySO.pValueOfMachinery));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pBusiHours", postPdBySO.pBusiHours);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pAppName", postPdBySO.pAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pVPAID", postPdBySO.pVPAID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiTranProofType", postPdBySO.pBusiTranProofType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCashInHand", Convert.ToDouble(postPdBySO.pCashInHand));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusiRef", postPdBySO.pBusiRef);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pBusiAddr", postPdBySO.pBusiAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBusiMobileNo", postPdBySO.pBusiMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pValidateYN", postPdBySO.pValidateYN);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSecondaryBusiYN", postPdBySO.pSecondaryBusiYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfSecBusi", Convert.ToInt32(postPdBySO.pNoOfSecBusi));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiType1", Convert.ToInt32(postPdBySO.pSecBusiType1));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSecBusiSeaso1", postPdBySO.pSecBusiSeaso1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiSubType1", Convert.ToInt32(postPdBySO.pSecBusiSubType1));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiActivity1", Convert.ToInt32(postPdBySO.pSecBusiActivity1));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiType2", Convert.ToInt32(postPdBySO.pSecBusiType2));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pSecBusiSeaso2", postPdBySO.pSecBusiSeaso2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiSubType2", Convert.ToInt32(postPdBySO.pSecBusiSubType2));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSecBusiActivity2", Convert.ToInt32(postPdBySO.pSecBusiActivity2));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiIncYN", postPdBySO.pBusiIncYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppCoAppSameBusiYN", postPdBySO.pAppCoAppSameBusiYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoAppBusinessName", postPdBySO.pCoAppBusinessName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppPrimaryBusiType", Convert.ToInt32(postPdBySO.pCoAppPrimaryBusiType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoAppPrimaryBusiSeaso", postPdBySO.pCoAppPrimaryBusiSeaso);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppPrimaryBusiSubType", Convert.ToInt32(postPdBySO.pCoAppPrimaryBusiSubType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppPrimaryBusiActivity", postPdBySO.pCoAppPrimaryBusiActivity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppMonthlyTrunOver", Convert.ToInt32(postPdBySO.pCoAppMonthlyTrunOver));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pCoAppBusinessAddr", postPdBySO.pCoAppBusinessAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoAppOtherBusinessProof", postPdBySO.pCoAppOtherBusinessProof);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppBusinessVintage", Convert.ToInt32(postPdBySO.pCoAppBusinessVintage));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCoAppBusiOwnerType", postPdBySO.pCoAppBusiOwnerType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppValueOfStock", Convert.ToInt32(postPdBySO.pCoAppValueOfStock));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postPdBySO.pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEntType", postPdBySO.pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", postPdBySO.pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pPDId", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPdDate", postPdBySO.pPdDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranchCode", postPdBySO.pBranchCode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 150, "@pBankName", postPdBySO.pBankName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCoAppPerAddrType", Convert.ToInt32(postPdBySO.pCoAppPerAddrType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCoApplPreAddrType", Convert.ToInt32(postPdBySO.pCoApplPreAddrType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 350, "@pCoApplPreAddr", postPdBySO.pCoApplPreAddr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCoApplPreStateId", Convert.ToInt32(postPdBySO.pCoApplPreStateId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 10, "@pCoApplPrePIN", postPdBySO.pCoApplPrePIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pApplDOB", postPdBySO.pApplDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 2, "@pApplAge", Convert.ToInt32(postPdBySO.pApplAge));

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplHouseImgLat", postPdBySO.pApplHouseImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplHouseImgLong", postPdBySO.pApplHouseImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi1ImgLat", postPdBySO.pApplBusi1ImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi1ImgLong", postPdBySO.pApplBusi1ImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi2ImgLat", postPdBySO.pApplBusi2ImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi2ImgLong", postPdBySO.pApplBusi2ImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi3ImgLat", postPdBySO.pApplBusi3ImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusi3ImgLong", postPdBySO.pApplBusi3ImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusiIdproofImgLat", postPdBySO.pApplBusiIdproofImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pApplBusiIdproofImgLong", postPdBySO.pApplBusiIdproofImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplHouseImgLat", postPdBySO.pCoApplHouseImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplHouseImgLong", postPdBySO.pCoApplHouseImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplBusi1ImgLat", postPdBySO.pCoApplBusi1ImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplBusi1ImgLong", postPdBySO.pCoApplBusi1ImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplBusi2ImgLat", postPdBySO.pCoApplBusi2ImgLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCoApplBusi2ImgLong", postPdBySO.pCoApplBusi2ImgLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMinorityYN", postPdBySO.pMinority);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsAbledYN", postPdBySO.pIsAbledYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSpeciallyAbled", postPdBySO.pSpeciallyAbled);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", pErrDesc);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vPDId = Convert.ToString(oCmd.Parameters["@pPDId"].Value);
                string vMemberId = Convert.ToString(oCmd.Parameters["@pMemberId"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                if (vErr == 0)
                {
                    JocataRequest(vMemberId, Convert.ToInt32(vPDId), Convert.ToInt32(postPdBySO.pCreatedBy));
                    return "Success:PD Saved Successfully.:" + vPDId;
                }
                else
                {
                    return pErrDesc + ":" + vPDId;
                }
            }
            catch (Exception ex)
            {
                return "Failed:PD Upload Failed.:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string SavePdByBM(PostPdByBM postPdByBM)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePdByBM";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", Convert.ToInt32(postPdByBM.pPDId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ1", postPdByBM.pPreScrQ1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ2", postPdByBM.pPreScrQ2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ3", postPdByBM.pPreScrQ3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ4", postPdByBM.pPreScrQ4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ5", postPdByBM.pPreScrQ5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ6", postPdByBM.pPreScrQ6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreScrQ7", postPdByBM.pPreScrQ7);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanReqDtlCorrectYN", postPdByBM.pLoanReqDtlCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplDtlCorrectYN", postPdByBM.pApplDtlCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoApplDtlcorrectYN", postPdByBM.pCoApplDtlcorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOthrInfocorrectYN", postPdByBM.pOthrInfocorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pProxyInfoCorrectYN", postPdByBM.pProxyInfoCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBankInfoCorrectYN", postPdByBM.pBankInfoCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplResiRelCorrectYN", postPdByBM.pApplResiRelCorrectYN);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplBusInfoCorrectYN", postPdByBM.pApplBusInfoCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoApplBusInfoCorrectYN", postPdByBM.pCoApplBusInfoCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusinessPhotoCorrectYN", postPdByBM.pBusinessPhotoCorrectYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPdByBMDate", postPdByBM.pPdByBMDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postPdByBM.pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Edit");//postPdByBM.pMode
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pUpdateRequired", postPdByBM.pUpdateRequired);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBmSelfieLat", postPdByBM.pBmSelfieLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBmSelfieLong", postPdByBM.pBmSelfieLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBmSelfie2Lat", postPdByBM.pBmSelfie2Lat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBmSelfie2Long", postPdByBM.pBmSelfie2Long);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "Success:PD Saved Successfully.";
                }
                else if (vErr == 2)
                {
                    return "PD Upload Failed.Dayend already done.";
                }
                else if (vErr == 3)
                {
                    return "PD Upload Failed.PD by SO Date and PD by BM date should not be same day.";
                }
                else
                {
                    return "Failed:PD Upload Failed.";
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
        #endregion

        #region PopBusinessType
        public DataTable PopBusinessType()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopBusinessType";
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

        #region PopBusiSubTypeByTypeId
        public DataTable PopBusiSubTypeByTypeId(int pBusinessTypeId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopBusiSubTypeByTypeId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessTypeId", pBusinessTypeId);
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

        #region PopBusiActivityBySubTypeId
        public DataTable PopBusiActivityBySubTypeId(int pBusinessSubTypeId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopBusiActivityBySubTypeId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessSubTypeId", pBusinessSubTypeId);
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

        #region GetIFSCDtl
        public DataTable GetIFSCDtl(PostIFSCData postIFSCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetIfscDtlByIfsc";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pIfsCode", postIFSCData.pIFSCCode);
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

        #region PdByBM
        public DataTable GetPdByBMData(PostPdByBMData postPdByBMData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetPdByBMData";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postPdByBMData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", postPdByBMData.pEoId);
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

        public DataSet GetPdDtlByPdId(Int32 pPdId)
        {
            DataSet ds = null;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPdDtlByPdId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPdId", pPdId);
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

        #endregion

        #region ChangePassword
        public string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_ChangePass";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUserName", postMob_ChangePassword.pUserName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postMob_ChangePassword.pPassword.Length + 1, "@pPassword", postMob_ChangePassword.pPassword);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postMob_ChangePassword.pOldPassword.Length + 1, "@pOldPassword", postMob_ChangePassword.pOldPassword);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "User update successfully.";
                }
                else if (vErr == 2)
                {
                    return "Invalid UserName Name.";
                }
                else if (vErr == 3)
                {
                    return "UserName or Password doesnot match.";
                }
                else if (vErr == 4)
                {
                    return "Password may not be similiar to last 3 passwords.";
                }
                else
                {
                    return "Data Not Saved";
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
        #endregion

        #region ICICIDisbursememt

        public Int32 SaveIciciBankLog(string pReferenceNo, string pRequestData, string pResponseData, string pEndPoint, string pRequestJson, string pResponseJson)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIciciBankLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pReferenceNo", pReferenceNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pRequestData.Length + 1, "@pRequestData", pRequestData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pEnd_Point", pEndPoint);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pRequestJson.Length + 1, "@pRequestJson", pRequestJson);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseJson.Length + 1, "@pResponseJson", pResponseJson);
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

        public Int32 InsertNEFTTransferAPI(string pXml, Int32 pCreatedby)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertNEFTTransferAPI";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedby);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
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

        #endregion

        #region UpdateNupayStatus

        public Int32 UpdateNupayStatus(string pId, string pAccptd, string pAccpt_Ref_No, string pReason_Code, string pReason_Desc,
            string pReject_By, string pNpci_Ref, string pCredit_DateTime, string pUmrn, string pAuth_Type)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateNupayStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pId", pId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pAccptd", pAccptd);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAccptRefNo", pAccpt_Ref_No);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pReasonCode", pReason_Code);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pReasonDesc", pReason_Desc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pRejectBy", pReject_By);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNpciRef", pNpci_Ref);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCreditDateTime", pCredit_DateTime);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pUMRN", pUmrn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAuthType", pAuth_Type);
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

        #endregion

        //public string SMSAPITest()
        //{
        //    string vDigiConcentSMS = string.Empty, vDigiConcentSMSTemplateId = string.Empty, vDigiConcentSMSLanguage = string.Empty; ;
        //    string vMobNo = "7044064914";
        //    string vResultSendDigitalConcentSMS = "";
        //    SqlCommand oCmd1 = new SqlCommand();
        //    oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
        //    oCmd1.CommandText = "GetSMSContent";
        //    DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
        //    DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
        //    DBUtility.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
        //    DBUtility.Execute(oCmd1);
        //    vDigiConcentSMS = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMS"].Value);
        //    vDigiConcentSMSTemplateId = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSTemplateId"].Value);
        //    vDigiConcentSMSLanguage = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSLanguage"].Value);
        //    vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vMobNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
        //    return "";
        //}

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

        #region GetJocataRequestData
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
        #endregion

        #region Jocata Integration

        public string GetJokataToken()
        {
            string postURL = ProdYN == "Y" ? "https://aml.unitybank.co.in/ramp/webservices/createToken" : "https://jocatauat.unitybank.co.in/ramp/webservices/createToken";
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
                string postURL = ProdYN == "Y" ? "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request" : "https://usfbamluat.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
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

        public string SaveJocataLog(string pMemberId, Int32 pPdId, string pResponseData, string pScreeningID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pPdId", pPdId);
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

        public int UpdateJocataStatus(string pMemberId, Int32 pPdId, string pScreeningID, string pStatus, Int32 pCreatedBy, string pRemarks, string pRiskCat)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            int vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateJocataStatus";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pPdId", pPdId);
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

        #region JocataCalling
        public string JocataRequest(string pMemberID, Int32 pPdID, Int32 pCreatedBy)
        {
            string vResponseData = "", vScreeningId = "", vResponseXml = "", vMsg = "", vStatus = "P";
            DataTable dt = new DataTable();
            try
            {
                dt = GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    #region RequestData
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
                    #endregion

                    vResponseData = RampRequest(req);
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);

                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;

                        if (vMatchFlag == true)
                        {
                            vStatus = "N";
                            vMsg = "True Match Member";
                        }
                        else
                        {
                            vStatus = "P";
                            vMsg = "False Match Member";
                            UpdateJocataStatus(pMemberID, pPdID, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                            try
                            {
                                ProsiReq pReq = new ProsiReq();
                                pReq.pMemberId = pMemberID;
                                pReq.pCreatedBy = Convert.ToString(pCreatedBy);
                                pReq.pPDId = Convert.ToString(pPdID);
                                // Prosidex(pReq);
                                PosidexEncryption(pReq);
                            }
                            finally { }
                        }
                    }
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    SaveJocataLog(pMemberID, Convert.ToInt32(pPdID), vResponseXml, vScreeningId);
                }
            }
            catch
            {
                vMsg = "Unable to connect Jocata API.";
                vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                SaveJocataLog(pMemberID, Convert.ToInt32(pPdID), vResponseXml, vScreeningId);
                UpdateJocataStatus(pMemberID, pPdID, vScreeningId, "U", pCreatedBy, "", "LOW"); ;
            }
            return vMsg;
        }
        #endregion

        #region SaveKarzaAadhaarOtp
        public Int32 SaveKarzaAadhaarOtp(string pApiName, string pResponseXml, string pBranch, string pEoID, string pIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaAadhaarOtp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
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
        #endregion

        #region SaveAadhaarVaultLog
        public string SaveAadhaarVaultLog(string pAadhaarNo, Int32 pCreatedBy, string pResponseData, string pMobileNo)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveAadhaarVaultLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarNo", pAadhaarNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMobileNo", pMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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

        #region AadhaarVault

        private string signRequest(string text)
        {
            string dataString = text;

            byte[] originalData = System.Text.Encoding.Default.GetBytes(dataString);
            byte[] signedData;
            var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/adharvault_unity.pfx");
            X509Certificate2 certificate = new X509Certificate2(vStrMycertPub, "3652145879",
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
                );

            RSACryptoServiceProvider RSAalg = certificate.PrivateKey as RSACryptoServiceProvider;

            RSAParameters Key = RSAalg.ExportParameters(true);

            signedData = HashAndSignBytes(originalData, Key);

            if (VerifySignedHash(originalData, signedData, Key))
            {
                Console.WriteLine("The data was verified.");
            }
            else
            {
                Console.WriteLine("The data does not match the signature.");
            }
            return Convert.ToBase64String(signedData);
        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                return RSAalg.SignData(DataToSign, SHA256.Create());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                return RSAalg.VerifyData(DataToVerify, SHA256.Create(), SignedData);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        public AadhaarVaultResponse AadhaarVault(AadhaarVault AadhaarVault, out string pRequest, out string pResponse)
        {
            string postURL = ProdYN == "Y" ? "https://avault.unitybank.co.in/vault/insert" : "https://avaultuat.unitybank.co.in/vault/insert";
            string vAadhaarNo = Convert.ToString(AadhaarVault.refData);
            string vMaskedAadhaar = String.Format("{0}{1}", "********", vAadhaarNo.Substring(vAadhaarNo.Length - 4, 4));
            try
            {
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);
                pRequest = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));
                var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/adharvault_unity.pfx");
                string password = "3652145879";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                X509Certificate2Collection certificates = new X509Certificate2Collection();
                certificates.Import(vStrMycertPub, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.AllowAutoRedirect = true;
                request.ClientCertificates = certificates;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("signed-data", signRequest(requestBody));
                request.Headers.Add("x-key", "9653214879");

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                pResponse = vResponseXml;
                SaveAadhaarVaultLog(vMaskedAadhaar, Convert.ToInt32(AadhaarVault.pCreatedBy), vResponseXml, AadhaarVault.pMobileNo);
                //-------------------------------------------------------------------------------------
                AadhaarVaultResponse myDeserializedClass = JsonConvert.DeserializeObject<AadhaarVaultResponse>(fullResponse);
                return myDeserializedClass;
            }
            catch (WebException ex)
            {
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);
                pRequest = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));

                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                pResponse = vResponseXml;
                SaveAadhaarVaultLog(vMaskedAadhaar, Convert.ToInt32(AadhaarVault.pCreatedBy), vResponseXml, AadhaarVault.pMobileNo);
                //-------------------------------------------------------------------------------------
                AadhaarVaultResponse myDeserializedClass = null;
                try
                {
                    myDeserializedClass = JsonConvert.DeserializeObject<AadhaarVaultResponse>(Response);
                }
                catch
                {
                    myDeserializedClass = new AadhaarVaultResponse("", 0, "", 0, null);
                }
                return myDeserializedClass;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public AadhaarNoByRefId GetAadhaarNoByRefId(AadhaarNoReq pAadhaarNoReq)
        {
            string responsedata = "";
            try
            {
                string vPostData = "refId=" + pAadhaarNoReq.refId;
                string postURL = ProdYN == "Y" ? "https://avault.unitybank.co.in/vault/get-by-refid?" + vPostData : "https://avaultuat.unitybank.co.in/vault/get-by-refid?" + vPostData;
                var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/adharvault_unity.pfx");
                string password = "3652145879";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                X509Certificate2Collection certificates = new X509Certificate2Collection();
                certificates.Import(vStrMycertPub, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postURL);
                request.AllowAutoRedirect = true;
                request.ClientCertificates = certificates;
                request.ContentType = "application/json";
                request.Headers.Add("x-key", "9653214879");
                request.Headers.Add("signed-data", signRequest(vPostData));

                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responsedata = responseReader.ReadToEnd();
                request.GetResponse().Close();
                AadhaarNoByRefId myDeserializedClass = JsonConvert.DeserializeObject<AadhaarNoByRefId>(responsedata);
                return myDeserializedClass;
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    responsedata = reader.ReadToEnd();
                }
                AadhaarNoByRefId myDeserializedClass = JsonConvert.DeserializeObject<AadhaarNoByRefId>(responsedata);
                return myDeserializedClass;
            }
        }
        #endregion

        #region Prosidex Integration
        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "";
            Int32 vCreatedBy = 1, vPDID = 0;
            ProsidexResponse oProsidexResponse = null;
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vPDID = Convert.ToInt32(prosidexRequest.Request.DG.APPLICATIONID);
                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                string postURL = ProdYN == "Y" ? "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer" : "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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
                SaveProsidexLog(vMemberId, vPDID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------  
                //  oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 200);
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
                SaveProsidexLog(vMemberId, vPDID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
            }
            finally
            {
            }
            oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            return oProsidexResponse;
        }

        public ProsidexResponse Prosidex(ProsiReq pProsiReq)
        {
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            List<ACE> oACE = new List<ACE>();
            ProsidexResponse pResponseData = null;
            dt = GetProsidexReqData(pProsiReq.pMemberId, pProsiReq.pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pPDId;
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

                oACE.Add(new ACE(dt.Rows[0]["ADDRESS"].ToString(),
                    dt.Rows[0]["ADDRESS_TYPE_FLAG"].ToString(),
                    dt.Rows[0]["COUNTRY"].ToString(),
                    dt.Rows[0]["City"].ToString(),
                    dt.Rows[0]["EMAIL"].ToString(),
                    dt.Rows[0]["EMAIL_TYPE"].ToString(),
                    dt.Rows[0]["PHONE"].ToString(),
                    dt.Rows[0]["PHONE_TYPE"].ToString(),
                    dt.Rows[0]["PINCODE"].ToString(),
                    dt.Rows[0]["State"].ToString()
                    ));

                pReq.ACE = oACE;
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

        public ProsidexResponse PosidexEncryption(ProsiReq pProsiReq)
        {
            DataTable dt = new DataTable();
            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            List<ACE> oACE = new List<ACE>();
            ProsidexResponse pResponseData = null;
            dt = GetProsidexReqData(pProsiReq.pMemberId, pProsiReq.pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pProsiReq.pPDId;
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
                //--------------------Address Part----------------------------
                oACE.Add(new ACE(dt.Rows[0]["ADDRESS"].ToString(),
                    dt.Rows[0]["ADDRESS_TYPE_FLAG"].ToString(),
                    dt.Rows[0]["COUNTRY"].ToString(),
                    dt.Rows[0]["City"].ToString(),
                    dt.Rows[0]["EMAIL"].ToString(),
                    dt.Rows[0]["EMAIL_TYPE"].ToString(),
                    dt.Rows[0]["PHONE"].ToString(),
                    dt.Rows[0]["PHONE_TYPE"].ToString(),
                    dt.Rows[0]["PINCODE"].ToString(),
                    dt.Rows[0]["State"].ToString()
                    ));
                pReq.ACE = oACE;
                //-------------------------------------------------------------
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
            }
            pResponseData = ProsidexEncryption(pReq);
            return pResponseData;
        }

        public ProsidexResponse ProsidexEncryption(Request Req)
        {
            string vRequestdata = "", vFullResponse = "", vResponse = "", vUCIC = "", vRequestId = "",
            vMemberId = "", vPotentialYN = "N", vPotenURL = "", vResponseCode = "", vRsaKey = "",
            vResponseData = "", vEncryptedMatchResponse = "";
            string vPostUrl = PosidexEncURL + "/ServicePosidex.svc/PosidexSearchCustomer";
            int vPDID = 0, vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            //-------------------------------------------------------------
            vRequestId = Req.UnitySfb_RequestId;
            vMemberId = Req.DG.CUST_ID;
            vPDID = Convert.ToInt32(Req.DG.APPLICATIONID);
            //------------------------------------------------------------
            vRequestdata = JsonConvert.SerializeObject(Req);
            vFullResponse = HttpRequest(vPostUrl, vRequestdata);
            //------------------------------------------------------------
            dynamic objFullResponse = JsonConvert.DeserializeObject(vFullResponse);
            vResponseData = Convert.ToString(objFullResponse.ResponseData);
            vRsaKey = Convert.ToString(objFullResponse.RsaKey);
            //----------------------------------------------------------
            dynamic vFinalResponse = JsonConvert.DeserializeObject(vResponseData);
            vResponseCode = Convert.ToString(vFinalResponse.StatusInfo.ResponseCode);
            vEncryptedMatchResponse = Convert.ToString(vFinalResponse.EncryptedMatchResponse);
            vResponse = DecryptStringAES(vEncryptedMatchResponse, vRsaKey);
            dynamic vResp = JsonConvert.DeserializeObject(vResponse);
            vUCIC = vResp.POSIDEX_GENERATED_UCIC;
            //------------------------------------------------------------          
            if (vResponseCode == "200")
            {
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                vPotentialYN = vUCIC == null ? "Y" : "N";
                vPotenURL = vUCIC == null ? Convert.ToString(vResp.CRM_URL) : "";
            }
            else
            {
                vUCIC = vUCIC == null ? "" : vUCIC;
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
            }
            //----------------------------Save Log-------------------------------------------------
            string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
            SaveProsidexLog(vMemberId, vPDID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
            //--------------------------------------------------------------------------------------         
            return oProsidexResponse;
        }

        public string HttpRequest(string PostUrl, string Requestdata)
        {
            string vResponse = "";
            try
            {
                string postURL = PostUrl;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = streamReader.ReadToEnd();
                request.GetResponse().Close();
                return vResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
            }
            return vResponse;
        }

        public string DecryptStringAES(string plainText, string publicKey)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] aesKey = Convert.FromBase64String(publicKey);
                aesAlg.KeySize = 256;
                aesAlg.Key = aesKey;
                aesAlg.IV = new byte[16];
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(plainText)))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
        #endregion

        #region SaveProsidexLog
        public string SaveProsidexLog(string pMemberId, Int32 pPDId, string pRequestId, string pResponseData, Int32 pCreatedBy, string pUCIC_ID, string pPotentialYN, string pPotenURL)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveProsidexLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", pPDId);
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

        #region SaveFingPayLog
        public string SaveFingPayLog(string pMemberId, Int32 pPDId, string pRequestData, string pResponseData, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveFingPayLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pPDId", pPDId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pRequestData.Length + 1, "@pRequestXml", pRequestData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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

        #region BulkCollection
        public Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy, string pBCProductId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vCollMode = pBankLedgr == "C0001" ? "C" : "B";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBulkCollection";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", setDate(pAccDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCollXml.Length + 1, "@pXml", pCollXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBrachCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pModeAC", pBankLedgr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", "Being the Amt of Loan Collection from ");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", vCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@PCollectionMode", 'W');
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBCProductID", Convert.ToInt32(pBCProductId));
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
        #endregion

        #region SaveOtherCollectionBulk
        public Int32 SaveOtherCollectionBulk(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vCollMode = pBankLedgr == "C0001" ? "C" : "B";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveOtherCollectionBulk";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", setDate(pAccDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCollXml.Length + 1, "@pXml", pCollXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pModeAC", pBankLedgr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", "Being the Amt of Other Collection from ");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
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
        #endregion

        #region Risk Category
        public string JocataRiskCat(RiskCatReq vRiskCatReq)
        {
            string vJokataToken = vJocataToken, vRiskCategory = "", vRampResponse = "", vResponseXml = "", pPDId = "", pMemberID = "";
            try
            {
                //------------------------------------URI----------------------------------------------
                string postURL = ProdYN == "Y" ? "https://aml.unitybank.co.in/orck/on-boarding/calculate-risk"
                   : "https://jocatauat.unitybank.co.in/orck/on-boarding/calculate-risk";
                string vToken = ProdYN == "Y" ? "611d9587-7546-8e62-1809-c8f8c193d421" : "596cf388-a04f-53b1-ce6b-e9a54082363f";
                //------------------------------------------------------------------------------------
                pMemberID = vRiskCatReq.memberId; pPDId = vRiskCatReq.pdId;
                //------------------------------------------------------------------------------------
                string Requestdata = JsonConvert.SerializeObject(vRiskCatReq);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("token", vToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vRampResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic vResponse = JsonConvert.DeserializeObject(vRampResponse);
                if (vResponse.statusCode == "200")
                {
                    vRiskCategory = vResponse.riskResponse.customerRiskLevel;
                    vRiskCategory = vRiskCategory.ToUpper();
                }

                vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vRampResponse, "root"));
                SaveJocataRiskCategoryLog(pMemberID, Convert.ToInt32(pPDId), vResponseXml, vRiskCategory);
                return vRiskCategory;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vRampResponse, "root"));
                    SaveJocataRiskCategoryLog(pMemberID, Convert.ToInt32(pPDId), vResponseXml, vRiskCategory);
                }
            }
            finally
            {
            }
            return vRampResponse;
        }
        #endregion

        #region SaveJocataRiskCategoryLog
        public string SaveJocataRiskCategoryLog(string pMemberId, Int32 pPdId, string pResponseData, string pRiskCategory)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataRiskCategoryLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pPdId", pPdId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRiskCategory", pRiskCategory);
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

        #region Idfy Log Sp
        public void SaveIdfyVoterLog(string vRequestID, string vRes, PostVoterData PostVoterData)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIdfyVoterLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pReqId", vRequestID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVerificationStatus", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, vRes.Length + 1, "@pResponseData", vRes);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", PostVoterData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoid", PostVoterData.pEOId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pVoterNo", PostVoterData.VoterId);
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

        public void SaveIdfyAadhaarLog(string pRequestID, PostAadharData postAadharData, string pResponseData)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStarS = "********";
            string vReqAadhaarNo = vStarS + postAadharData.AadharNo.Substring(postAadharData.AadharNo.Length - 4);
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIdfyAadhaarLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", pRequestID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postAadharData.pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoid", postAadharData.pEOId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pReqAadhaarNo", vReqAadhaarNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
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

        public void UpdateIdfyAadhaarLog(string vReqId, string vResponseData, string vStatus, string vUid)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStarS = "********";
            vUid = vUid + vStarS + vUid;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateIdfyAadhaarLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", vReqId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVerificationStatus", vStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, vResponseData.Length + 1, "@pResponseData", vResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRespAadhaarNo", vUid);
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

        public string GetIdfyAadhaarLog(string vReqId)
        {
            string pResponseData = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetIdfyAadhaarLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", vReqId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.NVarChar, -1, "@pResponseData", pResponseData);
                DBUtility.Execute(oCmd);
                pResponseData = Convert.ToString(oCmd.Parameters["@pResponseData"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return pResponseData;
        }

        #endregion

        #region UpdateLogOutDt
        public Int32 UpdateLogOutDt(int pLoginId)
        {
            Int32 vRst = 1;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLogOutDt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoginId", pLoginId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRst", vRst);
                DBUtility.Execute(oCmd);
                vRst = Convert.ToInt32(oCmd.Parameters["@pRst"].Value);
                return vRst;
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

        #region UpdateSessionTime
        public Int32 UpdateSessionTime(int pLoginId)
        {
            Int32 vRst = 1;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateSessionTime";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoginId", pLoginId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRst", vRst);
                DBUtility.Execute(oCmd);
                vRst = Convert.ToInt32(oCmd.Parameters["@pRst"].Value);
                return vRst;
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

        #region InsertLoginDt
        public string InsertLoginDt(LoginReqData Req)
        {
            string vLoginId = "0";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLoginDt";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", Convert.ToInt32(Req.pUserId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pLoginId", vLoginId);
                DBUtility.Execute(oCmd);
                vLoginId = Convert.ToString(oCmd.Parameters["@pLoginId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vLoginId;
        }
        #endregion


        public DataTable GetCollectionByLoanId(string pLoanId, string pCollDt, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetCollectionByLoanId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCollDt", pCollDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }

        public DataTable Mob_GetLoanByMemByLo(string pLoId, string pAsOnDt, string pBranch, string pPTPYN)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetLoanByMemByLo";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoId", pLoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDt", pAsOnDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPTPYN", pPTPYN);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }

        #region Mob_Srv_SaveCollection
        public GetSaveCollection Mob_Srv_SaveCollection(PostSaveCollection postSaveCollection)
        {
            string vLoginId = "0";
            SqlCommand oCmd = new SqlCommand();
            GetSaveCollection obj = new GetSaveCollection();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_Srv_SaveCollection";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", Convert.ToString(postSaveCollection.pLoanId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", postSaveCollection.pAccDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", Convert.ToString(postSaveCollection.pBranch));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pPaymentMode", Convert.ToString(postSaveCollection.pPaymentMode));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pCollAmt", Convert.ToDecimal(postSaveCollection.pCollAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPTPYN", Convert.ToString(postSaveCollection.pPTPYN));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPTPDate", postSaveCollection.pPTPDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pPTPAmt", Convert.ToDecimal(postSaveCollection.pPTPAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pReasonId", Convert.ToInt32(postSaveCollection.pReasonId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postSaveCollection.pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEntType", Convert.ToString(postSaveCollection.pEntType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", Convert.ToInt32(postSaveCollection.pSynStatus));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEoID", Convert.ToString(postSaveCollection.pEoID));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLat", Convert.ToString(postSaveCollection.pLatitude));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLong", Convert.ToString(postSaveCollection.pLongitude));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1000, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pReceiptNo", "");
                DBUtility.Execute(oCmd);
                obj.pErr = Convert.ToString(oCmd.Parameters["@pErr"].Value);
                obj.pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                obj.pReceiptNo = Convert.ToString(oCmd.Parameters["@pReceiptNo"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return obj;
        }
        #endregion


        #region InsertInitialOTPLog
        public int InsertInitialOTPLog(string pMobileNo, string pOTP)
        {
            int vErr = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertInitialOTPLog";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobileNo", pMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", pOTP);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
            }
            catch (Exception ex)
            {
                return 1;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vErr;
        }

        public int SaveOTPLog(string pMobileNo, string pOTP, ref string pOTPId, string pUserName)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveOTP";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobileNo", pMobileNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", pOTP);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUserName", pUserName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pOTPId", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pOTPId = Convert.ToString(oCmd.Parameters["@pOTPId"].Value);
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
        #endregion


        #region LoanUtilizationQsAns
        public DataTable GetLoanUtilizationQsAns()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanUtilizationQsAns";
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }
        #endregion

        #region GetLUCPendingDataList
        public DataTable GetLUCPendingDataList(string pEoId, string pDate, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLUCPendingDataList";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoId", pEoId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }
        #endregion

        #region SaveLoanUtilization
        public string SaveLoanUtilization(PostLoanUtilization postLoanUtilization)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanUtilization";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", postLoanUtilization.pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUtlType", postLoanUtilization.pLoanUtlType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanUTLBy", Convert.ToInt32(postLoanUtilization.pLoanUTLBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLoanUTLRemarks", postLoanUtilization.pLoanUTLRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUTLVia", postLoanUtilization.pLoanUTLVia);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pLoanUTLAmt", Convert.ToDecimal(postLoanUtilization.pLoanUTLAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanUTL", postLoanUtilization.pLoanUTL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanUTLDt", postLoanUtilization.pLoanUTLDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsSamePurpose", postLoanUtilization.pIsSamePurpose);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLat", postLoanUtilization.pLat);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLong", postLoanUtilization.pLong);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsPhotoUploaded", postLoanUtilization.pIsPhotoUploaded);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, postLoanUtilization.pCollXml.Length + 1, "@pXml", postLoanUtilization.pCollXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Data Saved Successfully!!!";
                else if (vErr == 2)
                    return "Loan Utilization Already Done";
                else
                    return "Failed To Save Data!!!";
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

        public Int32 SaveIBMAadhaarOtp(string pApiName, string pTxnCode, string pRequestJsn, string pResponseJsn, string pBranch, string pEoID, string pIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIBMAadhaarOtp";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTxnCode", pTxnCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pRequestJsn.Length + 1, "@pRequestJsn", pRequestJsn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseJsn.Length + 1, "@pResponseJsn", pResponseJsn);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
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

        public DataTable GetBranchCtrlByBranchCode(string pBranchCode, string pEffectiveDate)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCtrlByBranchCode";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pEffectiveDate", pEffectiveDate);

                DBUtility.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return dt;
        }

    }
}



