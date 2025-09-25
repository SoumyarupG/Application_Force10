using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;
using System.Net;
using System.Xml;
using CENTRUMSME_MOBILE.Service_Equifax;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace CENTRUMSME_MOBILE
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PratamService" in code, svc and config file together. 
    public class CentrumSmeService : ICentrumSmeService
    {
        string PCSUserName = "", PCSPassword = "", CCRUserName = "", CCRPassword = "";
        private readonly Encoding encoding = Encoding.UTF8;
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly long Base = Alphabet.Length;
        string KarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string KarzaUrl = ConfigurationManager.AppSettings["KarzaUrl"];
        string vAccessTime = ConfigurationManager.AppSettings["AccessTime"];
        string vWebHookUrl = ConfigurationManager.AppSettings["WebHookUrl"];
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        string vDBName = ConfigurationManager.AppSettings["DBName"];

        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];

        string CustomerKYCImageBucket = ConfigurationManager.AppSettings["CustomerKYCImageBucket"];
        string CoApplicantKYCImageBucket = ConfigurationManager.AppSettings["CoApplicantKYCImageBucket"];
        string DocumentBucket = ConfigurationManager.AppSettings["DocumentBucket"];

        string PosidexEncURL = ConfigurationManager.AppSettings["PosidexEncURL"];
        string IBMAadhaarUrl = ConfigurationManager.AppSettings["IBMAadhaarUrl"];

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

        #region GetAppVersion
        public string GetAppVersion(string pVersion)
        {
            try
            {
                string VersionCode = ConfigurationManager.AppSettings["MobAppVersionCode"];
                if (Convert.ToInt32(VersionCode) > Convert.ToInt32(pVersion))
                {
                    return "https://centrumsmemob.bijliftt.com/centrummel.apk";
                }
                else
                {
                    return "No updates available";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region RijndaelManaged
        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        #endregion

        #region AES Decrypt
        public byte[] AesDecrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor()
                .TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        #endregion

        #region Mob_GetUser
        public List<LoginData> GetMobUser(UserData userData)
        {
            string vAttFlag = ConfigurationManager.AppSettings["AttFlag"];
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            List<EmpData> row1 = new List<EmpData>();
            List<PermissionData> row2 = new List<PermissionData>();
            List<LoginData> rowFinal = new List<LoginData>();
            CRepository oRepo = null;
            try
            {
                //userData.pEncYN = userData.pEncYN == null ? "N" : userData.pEncYN;
                //#region AES Decrypt
                //if (userData.pEncYN == "Y")
                //{
                //    String key = "Force@2301***DB";
                //    var encryptedBytes = Convert.FromBase64String(userData.pPassword);
                //    userData.pPassword = Encoding.UTF8.GetString(AesDecrypt(encryptedBytes, GetRijndaelManaged(key)));
                //}
                //#endregion

                oRepo = new CRepository();
                ds = oRepo.GetMobUser(userData);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                if (dt1.Rows.Count == 1)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row1.Add(new EmpData(rs["EoName"].ToString(), rs["BranchCode"].ToString(), rs["UserID"].ToString(), rs["Eoid"].ToString(), rs["LogStatus"].ToString(),
                            rs["DayEndDate"].ToString(), rs["AttStatus"].ToString(), vAttFlag, rs["AreaCat"].ToString(), rs["Designation"].ToString(), rs["LoginId"].ToString()
                            , rs["MFAYN"].ToString(), rs["MFAOTP"].ToString(), rs["DialogToImageYN"].ToString()));
                        if (rs["MFAYN"].ToString() == "Y")
                        {
                            SendMFAOTP(rs["MFAOTP"].ToString(), rs["MobileNo"].ToString());
                        }
                    }
                }
                else
                {
                    row1.Add(new EmpData("", "", "", "", "Login Failed", "", "", "N", "", "", "", "", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new PermissionData(rs["RoleID"].ToString(), rs["Role"].ToString(), rs["MenuID"].ToString(), rs["MenuName"].ToString(), rs["AllowView"].ToString(),
                            rs["AllowAdd"].ToString(), rs["AllowEdit"].ToString(), rs["AllowDel"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new PermissionData("No data available", "", "", "", "", "", "", ""));
                }

                rowFinal.Add(new LoginData(row1, row2));
            }
            catch (Exception ex)
            {
                rowFinal.Add(new LoginData(row1, row2));
            }
            finally
            {
                oRepo = null;
            }
            return rowFinal;
        }
        #endregion

        #region Mob_GetLeadList

        public List<GetLeadData> Mob_GetLeadList(string pBranchCode, string pEoId)
        {
            DataTable dt = new DataTable();
            List<GetLeadData> row = new List<GetLeadData>();
            CRepository oRepo = null;
            try
            {
                oRepo = new CRepository();
                dt = oRepo.Mob_GetLeadList(pBranchCode, pEoId);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLeadData(rs["LeadId"].ToString(), rs["CustomerName"].ToString(), rs["LogInFeesCollYN"].ToString(),
                            rs["LeadGenerationDate"].ToString(), rs["TotalLoginFees"].ToString(), rs["Branch"].ToString(), rs["MobNo"].ToString(),
                            rs["Email"].ToString(), rs["Address"].ToString(), rs["PropertyTypeId"].ToString(), rs["OccupationId"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetLeadData("No Data Available", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetLeadData("No Data Available", ex.Message, "", "", "", "", "", "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region Mob_GetLogInFees

        public List<GetLogInFeeData> Mob_GetLogInFees(string pDate)
        {
            DataTable dt = new DataTable();
            List<GetLogInFeeData> row = new List<GetLogInFeeData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.GetLogInFees(pDate);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLogInFeeData(rs["Id"].ToString(), rs["TotalLoginFees"].ToString(), rs["NetLogInFees"].ToString(),
                            rs["CGSTAmt"].ToString(), rs["SGSTAmt"].ToString(), rs["IGSTAmt"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetLogInFeeData("No Data Available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetLogInFeeData("No Data Available", ex.Message, "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region UpdateLead

        public string UpdateLead(string pLeadId, string pLeadGenerationDate, string pCustomerName, string pEmail, string pMobNo, string pAddress, string pPropertyTypeId,
            string pOccupationId, string pLogInFeesCollYN, string pGenParameterId, string pTotalLoginFees, string pNetLogInFees, string pCGSTAmt, string pSGSTAmt,
            string pIGSTAmt, string pBranchCode, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLead";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLeadId", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLeadGenerationDate", pLeadGenerationDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCustomerName", pCustomerName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmail", pEmail);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo", pMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddress", pAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropertyTypeId", pPropertyTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccupationId", pOccupationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLogInFeesCollYN", pLogInFeesCollYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGenParameterId", pGenParameterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalLoginFees", Convert.ToDecimal(pTotalLoginFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pNetLogInFees", Convert.ToDecimal(pNetLogInFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pCGSTAmt", Convert.ToDecimal(pCGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pSGSTAmt", Convert.ToDecimal(pSGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pIGSTAmt", Convert.ToDecimal(pIGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 7, "@pMode", "Edit");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", "M");
                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (vErr == 0)
                {
                    return "Success:Record Saved Successfully";
                }
                else
                {
                    return "Failed:Data Not Saved";
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SaveLead

        public string SaveLead(string pLeadId, string pLeadGenerationDate, string pCustomerName, string pEmail, string pMobNo, string pAddress, string pPropertyTypeId,
            string pOccupationId, string pLogInFeesCollYN, string pGenParameterId, string pTotalLoginFees, string pNetLogInFees, string pCGSTAmt, string pSGSTAmt,
            string pIGSTAmt, string pBranchCode, string pCreatedBy, string pSRC, string pAddressId, string pAddressIdNo, string pPhotoId, string pPhotoIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrMsg = "", pNewId = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLead";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLeadId", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLeadGenerationDate", pLeadGenerationDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCustomerName", pCustomerName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmail", pEmail);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo", pMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddress", pAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropertyTypeId", pPropertyTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccupationId", pOccupationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLogInFeesCollYN", pLogInFeesCollYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGenParameterId", pGenParameterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalLoginFees", Convert.ToDecimal(pTotalLoginFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pNetLogInFees", Convert.ToDecimal(pNetLogInFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pCGSTAmt", Convert.ToDecimal(pCGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pSGSTAmt", Convert.ToDecimal(pSGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pIGSTAmt", Convert.ToDecimal(pIGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", "Save");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pNewId", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", "M");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", pSRC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressId", Convert.ToInt32(pAddressId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAddressIdNo", pAddressIdNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPhotoId", Convert.ToInt32(pPhotoId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhotoIdNo", pPhotoIdNo);
                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                pNewId = Convert.ToString(oCmd.Parameters["@pNewId"].Value);
                if (vErr == 0)
                {
                    return "Success:Record Saved Successfully#" + pNewId;
                }
                else
                {
                    return "Failed:Data Not Saved.." + "(" + vErr.ToString() + ")" + pErrMsg;
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region Get Sanction

        public string GetSanctionId(string pLoanAppId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pSanctionID = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSanctionId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pSanctionID", "");
                DBUtility.Execute(oCmd);
                pSanctionID = Convert.ToString(oCmd.Parameters["@pSanctionID"].Value);
                return pSanctionID;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region Get LoanId

        public string GetLoanNoByLoanAppId(string pLoanAppId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pLoanId = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanNoByLoanAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanId", "");
                DBUtility.Execute(oCmd);
                pLoanId = Convert.ToString(oCmd.Parameters["@pLoanId"].Value);
                return pLoanId;
            }
            catch (Exception ex)
            {
                return "";
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region Get PurposeId
        public string GetPurposeIdByPurposeName(string pPurpose)
        {
            SqlCommand oCmd = new SqlCommand();
            string pPurposeId = "-1";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPurposeIdByPurposeName";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pPurpose", pPurpose);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 8, "@pPurposeId", 0);
                DBUtility.Execute(oCmd);
                pPurposeId = Convert.ToString(oCmd.Parameters["@pPurposeId"].Value);
                return pPurposeId;
            }
            catch (Exception ex)
            {
                return "-1";
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region Get Relation Id
        public string GetRelationIdByRelation(string pPurpose)
        {
            SqlCommand oCmd = new SqlCommand();
            string pPurposeId = "-1";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetRelationIdByRelation";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pRelation", pPurpose);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 8, "@pRelationId", 0);
                DBUtility.Execute(oCmd);
                pPurposeId = Convert.ToString(oCmd.Parameters["@pRelationId"].Value);
                return pPurposeId;
            }
            catch (Exception ex)
            {
                return "-1";
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region ApproveLead

        public string ApproveLead(string pLeadId, string pLeadGenerationDate, string pCustomerName, string pEmail, string pMobNo, string pAddress, string pPropertyTypeId,
            string pOccupationId, string pLogInFeesCollYN, string pGenParameterId, string pTotalLoginFees, string pNetLogInFees, string pCGSTAmt, string pSGSTAmt,
            string pIGSTAmt, string pBranchCode, string pCreatedBy, string pSRC)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrMsg = "", pMemberId = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLead";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLeadId", pLeadId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLeadGenerationDate", pLeadGenerationDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCustomerName", pCustomerName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmail", pEmail);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo", pMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddress", pAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropertyTypeId", pPropertyTypeId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccupationId", pOccupationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLogInFeesCollYN", pLogInFeesCollYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGenParameterId", pGenParameterId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalLoginFees", Convert.ToDecimal(pTotalLoginFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pNetLogInFees", Convert.ToDecimal(pNetLogInFees));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pCGSTAmt", Convert.ToDecimal(pCGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pSGSTAmt", Convert.ToDecimal(pSGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pIGSTAmt", Convert.ToDecimal(pIGSTAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 7, "@pMode", "Approve");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrMsg", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", "M");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pMemberId", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", pSRC);
                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                pMemberId = Convert.ToString(oCmd.Parameters["@pMemberId"].Value);

                if (vErr == 0)
                {
                    return "Success:" + pMemberId;
                }
                else
                {
                    return "Failed:Data Not Saved";
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }


        #endregion

        #region GetCompanyList

        public List<GetCompanyData> GetCompanyList(string pBranchCode, string pLogDt, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            List<GetCompanyData> row = new List<GetCompanyData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetCompanyList(pBranchCode, pLogDt, pCreatedBy);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetCompanyData(rs["CustId"].ToString(), rs["CompanyName"].ToString(), rs["DOF"].ToString(), rs["Branch"].ToString(), rs["ApplicantName"].ToString(), rs["MemType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetCompanyData("No Data Available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetCompanyData("No Data Available", ex.Message, "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region GetLoanAppData

        public List<GetLoanAppData> GetLoanAppData(string pBranch)
        {
            DataTable dt = new DataTable();
            List<GetLoanAppData> row = new List<GetLoanAppData>();
            CRepository oRepo = null;
            try
            {
                oRepo = new CRepository();
                dt = oRepo.Mob_GetLoanAppData(pBranch);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLoanAppData(rs["Type"].ToString(), rs["TypeID"].ToString(), rs["TypeName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetLoanAppData("No Data Available", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetLoanAppData("No Data Available", ex.Message, ""));
            }
            return row;
        }

        #endregion

        #region GetCompanyDetails

        public List<GetCompanyAllDetailas> GetCompanyDetails(string pBranchCode, string pCompanyId)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            List<GetCompanyDetailsData> row = new List<GetCompanyDetailsData>();
            List<GetCompanyDependent> row2 = new List<GetCompanyDependent>();
            List<GetCompanyAllDetailas> rowFinal = new List<GetCompanyAllDetailas>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                ds = oRepo.Mob_GetCompanyDetails(pBranchCode, pCompanyId);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row.Add(new GetCompanyDetailsData(rs["CustId"].ToString(), rs["CustNo"].ToString(), rs["CompanyTypeID"].ToString(),
                            rs["CompanyName"].ToString(), rs["DOF"].ToString(), rs["DOB"].ToString(), rs["PropertyTypeId"].ToString(), rs["OtherPropertyType"].ToString(),
                            rs["Website"].ToString(), rs["Email"].ToString(), rs["PANNo"].ToString(), rs["AddressId"].ToString(), rs["AddressIdNo"].ToString(), rs["SectorId"].ToString(),
                            rs["SubSectorId"].ToString(), rs["IsRegistered"].ToString(), rs["RegistrationNo"].ToString(),
                            rs["MAddress1"].ToString(), rs["MAddress2"].ToString(), rs["MState"].ToString(), rs["MDistrict"].ToString(), rs["MPIN"].ToString(),
                            rs["MMobNo"].ToString(), rs["MSTD"].ToString(), rs["MTelNo"].ToString(), rs["ApplicantName"].ToString(), rs["AppContactNo"].ToString(), rs["SameAddYN"].ToString(),
                            rs["RAddress1"].ToString(), rs["RAddress2"].ToString(), rs["RState"].ToString(), rs["RDistrict"].ToString(), rs["RPIN"].ToString(), rs["RMobNo"].ToString(), rs["RSTD"].ToString(),
                            rs["RTelNo"].ToString(), rs["GSTRegNo"].ToString(), rs["CustType"].ToString(), rs["PhotoId"].ToString(), rs["PhotoIdNo"].ToString(), rs["GenderId"].ToString(), rs["OccupationId"].ToString(),
                            rs["BusinessTypeId"].ToString(), rs["BusinessLocation"].ToString(), rs["AnnualIncome"].ToString(), rs["Age"].ToString(), rs["RelativeName"].ToString(), rs["QualificationId"].ToString(),
                            rs["RligionId"].ToString(), rs["Caste"].ToString(), rs["NoOfYrInCurRes"].ToString(), rs["PhyChallangeYN"].ToString(), rs["ACHolderName"].ToString(), rs["ACNo"].ToString(), rs["BankName"].ToString(),
                            rs["IFSCCode"].ToString(), rs["YrOfOpening"].ToString(), rs["AccountType"].ToString(), rs["ContactNo"].ToString(), rs["EmpOrgName"].ToString(), rs["EmpDesig"].ToString(), rs["EmpRetiredAge"].ToString(),
                            rs["EmpCode"].ToString(), rs["EmpCurExp"].ToString(), rs["EmpTotExp"].ToString(), rs["BusGSTAppYN"].ToString(), rs["BusGSTNo"].ToString(), rs["BusLandMark"].ToString(), rs["BusAddress1"].ToString(),
                            rs["BusAddress2"].ToString(), rs["BusLocality"].ToString(), rs["BusCity"].ToString(), rs["BusPIN"].ToString(), rs["BusState"].ToString(), rs["BusMob"].ToString(), rs["BusPhone"].ToString(),
                            rs["CommunAddress"].ToString(), rs["MaritalStatus"].ToString(), rs["ResidentialStatus"].ToString(), rs["RelationId"].ToString(),
                            rs["NomineeName"].ToString(), rs["NomineeDOB"].ToString(), rs["NomineeGender"].ToString(), rs["NomineeRelation"].ToString(),
                            rs["NomineeAddress"].ToString(), rs["NomineeState"].ToString(), rs["NomineePinCode"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetCompanyDetailsData("No Data Available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new GetCompanyDependent(rs["SLNo"].ToString(), rs["Name"].ToString(), rs["RelationId"].ToString(),
                            rs["Age"].ToString(), rs["OccupationId"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new GetCompanyDependent("No Data Available", "", "", "", ""));
                }

                rowFinal.Add(new GetCompanyAllDetailas(row, row2));
            }
            catch (Exception ex)
            {
                rowFinal.Add(new GetCompanyAllDetailas(row, row2));
            }
            return rowFinal;
        }

        #endregion

        #region ShowCompanyDtlById

        public List<GetCoAppAllDetailas> ShowCompanyDtlById(string pBranchCode, string pCompanyId, string pCreatedBy)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            List<GetCompanyDtlById> row = new List<GetCompanyDtlById>();
            List<GetCoAppInfo> row2 = new List<GetCoAppInfo>();
            List<GetCoAppAllDetailas> rowFinal = new List<GetCoAppAllDetailas>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                ds = oRepo.Mob_ShowCompanyDtlById(pBranchCode, pCompanyId, pCreatedBy);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row.Add(new GetCompanyDtlById(rs["CustId"].ToString(), rs["CustNo"].ToString(), rs["CompanyName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetCompanyDtlById("No Data Available", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new GetCoAppInfo(rs["CoApplicantId"].ToString(), rs["CoApplicantNo"].ToString(), rs["CustId"].ToString(),
                                                 rs["CoAppName"].ToString(), rs["BranchCode"].ToString(), rs["IsGuarantor"].ToString(), rs["IsPrimaryCoAppYN"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new GetCoAppInfo("No Data Available", "", "", "", "", "", ""));
                }

                rowFinal.Add(new GetCoAppAllDetailas(row, row2));
            }
            catch (Exception ex)
            {
                rowFinal.Add(new GetCoAppAllDetailas(row, row2));
            }
            return rowFinal;
        }

        #endregion

        #region GetCoApplicantDtl

        public List<GetCoAppAndDepAllInfo> GetCoApplicantDtl(string pBranchCode, string pCoAppId)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            List<GetCoAppDetlById> row = new List<GetCoAppDetlById>();
            List<GetCoAppDependentInfo> row2 = new List<GetCoAppDependentInfo>();
            List<GetCoAppAndDepAllInfo> rowFinal = new List<GetCoAppAndDepAllInfo>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                ds = oRepo.Mob_GetCoApplicantDtl(pBranchCode, pCoAppId);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row.Add(new GetCoAppDetlById(rs["CoApplicantId"].ToString(), rs["CoApplicantNo"].ToString(), rs["CustId"].ToString(), rs["DOA"].ToString(), rs["DOB"].ToString(),
                            rs["CoAppName"].ToString(), rs["CastId"].ToString(), rs["ReligionId"].ToString(), rs["OccupationId"].ToString(), rs["MaritalStatus"].ToString(), rs["Gender"].ToString(),
                            rs["Age"].ToString(), rs["Qualification"].ToString(), rs["PhotoId"].ToString(), rs["PhotoIdNo"].ToString(), rs["AddressId"].ToString(), rs["AddressIdNo"].ToString(),
                            rs["ContMNo"].ToString(), rs["ContTelNo"].ToString(), rs["ContFAXNo"].ToString(), rs["Email"].ToString(), rs["YearAtRes"].ToString(), rs["YearAtBus"].ToString(), rs["PreAddress1"].ToString(),
                            rs["PreAddress2"].ToString(), rs["PreState"].ToString(), rs["PreDistrict"].ToString(), rs["PrePIN"].ToString(), rs["SameAddYN"].ToString(), rs["PerAddress1"].ToString(), rs["PerAddress2"].ToString(),
                            rs["PerDistrict"].ToString(), rs["PerState"].ToString(), rs["PerPIN"].ToString(), rs["IsDirector"].ToString(), rs["IsPartner"].ToString(), rs["IsPropietor"].ToString(), rs["IsSpouse"].ToString(),
                            rs["IsSameAddAsApp"].ToString(), rs["IsGuarantor"].ToString(), rs["IsActive"].ToString(), rs["ShareHolPer"].ToString(), rs["CoAppType"].ToString(), rs["CompTypeID"].ToString(), rs["PropertyTypeId"].ToString(),
                            rs["AppName"].ToString(), rs["IsPrimaryCoAppYN"].ToString(), rs["CustCoAppRel"].ToString(), rs["RelationId"].ToString(), rs["RelativeName"].ToString(), rs["PhyChallangeYN"].ToString(),
                            rs["ACHolderName"].ToString(), rs["ACNo"].ToString(), rs["BankName"].ToString(), rs["IFSCCode"].ToString(), rs["YrOfOpening"].ToString(), rs["AccountType"].ToString(), rs["EmpOrgName"].ToString(),
                            rs["EmpDesig"].ToString(), rs["EmpRetiredAge"].ToString(), rs["EmpCode"].ToString(), rs["EmpCurExp"].ToString(), rs["EmpTotExp"].ToString(), rs["BusGSTAppYN"].ToString(), rs["BusGSTNo"].ToString(),
                            rs["BusLandMark"].ToString(), rs["BusAddress1"].ToString(), rs["BusAddress2"].ToString(), rs["BusLocality"].ToString(), rs["BusCity"].ToString(), rs["BusPIN"].ToString(), rs["BusState"].ToString(),
                            rs["BusMob"].ToString(), rs["BusPhone"].ToString(), rs["CommunAddress"].ToString(), rs["ResidentialStatus"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetCoAppDetlById("No Data Available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new GetCoAppDependentInfo(rs["SLNo"].ToString(), rs["CoAppId"].ToString(), rs["Name"].ToString(),
                                                 rs["RelationId"].ToString(), rs["Age"].ToString(), rs["OccupationId"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new GetCoAppDependentInfo("No Data Available", "", "", "", "", ""));
                }

                rowFinal.Add(new GetCoAppAndDepAllInfo(row, row2));
            }
            catch (Exception ex)
            {
                rowFinal.Add(new GetCoAppAndDepAllInfo(row, row2));
            }
            return rowFinal;
        }

        #endregion

        #region Mob_SaveCompany

        public string Mob_SaveCompany(string pCompanyId, string pCompanyNo, string pComTypeId, string pCompanyName, string pDOF, string pDOB, string pPropertyId,
            string pOtherPropertyDtl, string pWebsite, string pEmail, string pPANNo, string pAddressId, string pAddressIdNo, string pIsRegister, string pRegisNo,
            string pSectorId, string pSubSectorId, string pMAddress1, string pMAddress2, string pMState, string pMDistrict, string pMPIN, string pMMobNo, string pMSTD,
            string pMTelNo, string pSameAddYN, string pRAddress1, string pRAddress2, string pRState, string pRDistrict, string pRPIN, string pRMobNo, string pRSTD,
            string pRTelNo, string pBranchCode, string pCreatedBy, string pAppName, string pAppContNo, string pGSTRegNo, string pPhotoId, string pPhotoIdNo,
            string pOccupationId, string pBusinessTypeId, string pBusinessLocation, string pAnnualIncome, string pGenderId, string pAge, string pRelationId,
            string pRelativeName, string pQualificationId, string pRligionId, string pCaste, string pNoOfYrInCurRes, string pPhyChallangeYN, string pACHolderName,
            string pACNo, string pBankName, string pIFSCCode, string pYrOfOpening, string pAccountType, string pContactNo, string pEmpOrgName, string pEmpDesig,
            string pEmpRetiredAge, string pEmpCode, string pEmpCurExp, string pEmpTotExp, string pBusGSTAppYN, string pBusGSTNo, string pBusLandMark, string pBusAddress1,
            string pBusAddress2, string pBusLocality, string pBusCity, string pBusPIN, string pBusState, string pBusMob, string pBusPhone, string pCommunAddress,
            string pMaritalStatus, string pResidentialStatus, string pXml, string pBusinessName, string pYearsInCurrBusiness, string pAppReltvName, string pAppRelType,
            string pID1VoterResponse, string pID1AadharResponse, string pNameMatchingResponse, string pIdVerifyYN, string pIsAbledYN, string pSpeciallyAbled,
            string pNomineeName, string pNomineeDOB, string pNomineePinCode, string pNomineeAddress, string pNomineeGender, string pNomineeRelation, string pNomineeState,
            string pXmlDirector, string pMinorityYN = "N", string pSRC = "NOR")
        {
            string vErrMsg = "", xmlID1VoterResponse = "", xmlID1AadharResponse = "", xmlNameMatchingResponse = ""
                , vAadharNo = "", vMaskedAadhaar = "", vAadhaarRefNo = "", vErMsg = "";
            Int32 vErr1 = 0;
            #region Karza XML
            pID1VoterResponse = pID1VoterResponse == null ? "" : pID1VoterResponse.Replace("\u0000", "");
            pID1VoterResponse = pID1VoterResponse.Replace("\\u0000", "");
            xmlID1VoterResponse = AsString(JsonConvert.DeserializeXmlNode(pID1VoterResponse, "root"));
            pID1AadharResponse = pID1AadharResponse == null ? "" : pID1AadharResponse.Replace("\u0000", "");
            pID1AadharResponse = pID1AadharResponse.Replace("\\u0000", "");
            xmlID1AadharResponse = AsString(JsonConvert.DeserializeXmlNode(pID1AadharResponse, "root"));
            xmlNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(pNameMatchingResponse == null ? "" : pNameMatchingResponse, "root"));
            #endregion

            #region AadharVault
            if (Convert.ToInt32(pPhotoId) == 6 || Convert.ToInt32(pAddressId) == 6)
            {
                vAadharNo = Convert.ToInt32(pPhotoId) == 6 ? pPhotoIdNo : pAddressIdNo;
                vMaskedAadhaar = String.Format("{0}{1}", "********", vAadharNo.Substring(vAadharNo.Length - 4, 4));
                vAadhaarRefNo = AadhaarVaultCalling(vAadharNo, pMMobNo, pCreatedBy);
                if (vAadhaarRefNo != "")
                {
                    pPhotoIdNo = Convert.ToInt32(pPhotoId) == 6 ? vAadhaarRefNo : pPhotoIdNo;
                    pAddressIdNo = Convert.ToInt32(pAddressId) == 6 ? vAadhaarRefNo : pAddressIdNo;
                    pPhotoId = Convert.ToInt32(pPhotoId) == 6 ? "15" : pPhotoId;
                    pAddressId = Convert.ToInt32(pAddressId) == 6 ? "15" : pAddressId;
                }
                else
                {
                    return "Failed:Unable to get aadhaar ref no.";
                }
            }
            #endregion

            if (pSRC == "NOR" || pSRC == null)
            {
                vErr1 = chkDdupMEL_OWN(pAddressIdNo, pPhotoIdNo, "", pBranchCode, ref vErrMsg, pCompanyId);
            }
            if (vErr1 == 99 || vErr1 == 0 || vErr1 == 57)
            {
                if (vErr1 == 99)
                {
                    string[] arr1 = vErrMsg.Split('#');
                    pCompanyNo = arr1[1];
                }

                SqlCommand oCmd = new SqlCommand();
                Int32 vErr = 0;
                string pNewId = "";
                try
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "SaveCompany";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCompanyId", pCompanyId);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCompanyNo", pCompanyNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pComTypeId", Convert.ToInt32(pComTypeId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCompanyName ", pCompanyName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOF", pDOF);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", pDOB);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropertyId", Convert.ToInt32(pPropertyId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pOtherPropertyDtl", pOtherPropertyDtl);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pWebsite", pWebsite);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmail", pEmail);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPANNo", pPANNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressId", Convert.ToInt32(pAddressId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAddressIdNo", pAddressIdNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIsRegister", pIsRegister);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRegisNo", pRegisNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSectorId", Convert.ToInt32(pSectorId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSubSectorId", Convert.ToInt32(pSubSectorId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pMAddress1", pMAddress1);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMAddress2", pMAddress2);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMState", pMState);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMDistrict", pMDistrict);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMPIN", pMPIN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMMobNo", pMMobNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMSTD", pMSTD);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMTelNo", pMTelNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSameAddYN", pSameAddYN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pRAddress1", pRAddress1);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pRAddress2", pRAddress2);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRState", pRState);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pRDistrict", pRDistrict);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pRPIN", pRPIN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRMobNo", pRMobNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pRSTD", pRSTD);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRTelNo", pRTelNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", "Save");
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pAppName", pAppName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAppContNo", pAppContNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pGSTRegNo", pGSTRegNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustType", "I");
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPhotoId", Convert.ToInt32(pPhotoId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhotoIdNo", pPhotoIdNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccupationId", Convert.ToInt32(pOccupationId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusinessTypeId", Convert.ToInt32(pBusinessTypeId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusinessLocation", pBusinessLocation);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pAnnualIncome", Convert.ToDecimal(pAnnualIncome));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGenderId", Convert.ToInt32(pGenderId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAge", Convert.ToInt32(pAge));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(pRelationId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pRelativeName", pRelativeName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQualificationId", Convert.ToInt32(pQualificationId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRligionId", Convert.ToInt32(pRligionId));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCaste", Convert.ToInt32(pCaste));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pNoOfYrInCurRes", Convert.ToDecimal((pNoOfYrInCurRes == "" ? "0" : pNoOfYrInCurRes)));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPhyChallangeYN", pPhyChallangeYN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pACHolderName", pACHolderName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pACNo", pACNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBankName", pBankName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIFSCCode", pIFSCCode);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrOfOpening", Convert.ToInt32((pYrOfOpening == "" ? "0" : pYrOfOpening)));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAccountType", Convert.ToInt32(pAccountType));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pContactNo", pContactNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmpOrgName", pEmpOrgName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmpDesig", pEmpDesig);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpRetiredAge", Convert.ToInt32(pEmpRetiredAge));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEmpCode", pEmpCode);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpCurExp", Convert.ToInt32(pEmpCurExp));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpTotExp", Convert.ToInt32(pEmpTotExp));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusGSTAppYN", pBusGSTAppYN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBusGSTNo", pBusGSTNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusLandMark", pBusLandMark);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusAddress1", pBusAddress1);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusAddress2", pBusAddress2);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusLocality", pBusLocality);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pBusCity", pBusCity);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pBusPIN", pBusPIN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pBusState", pBusState);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBusMob", pBusMob);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pBusPhone", pBusPhone);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCommunAddress", pCommunAddress);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMaritalStatus", Convert.ToInt32(pMaritalStatus));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pResidentialStatus", pResidentialStatus);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pNewId", "");
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", vErMsg);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", "M");
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusinessName", pBusinessName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYearsInCurrBusiness", Convert.ToInt32((pYearsInCurrBusiness == "" ? "0" : pYearsInCurrBusiness)));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarNo", vMaskedAadhaar);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pMFHName", pAppReltvName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMHFRelation", Convert.ToInt32(pAppRelType));

                    if (xmlID1VoterResponse == null)
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pID1VoterResponse", DBNull.Value);
                    else
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1VoterResponse.Length + 1, "@pID1VoterResponse", xmlID1VoterResponse);

                    if (xmlID1AadharResponse == null)
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pID1AadharResponse", DBNull.Value);
                    else
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1AadharResponse.Length + 1, "@pID1AadharResponse", xmlID1AadharResponse);

                    if (xmlNameMatchingResponse == null)
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pNameMatchingResponse", DBNull.Value);
                    else
                        DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlNameMatchingResponse.Length + 1, "@pNameMatchingResponse", xmlNameMatchingResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIdVerifyYN", pIdVerifyYN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsAbledYN", pIsAbledYN);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSpeciallyAbled", Convert.ToInt32(pSpeciallyAbled));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMinorityYN", pMinorityYN);

                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pNomineeName", pNomineeName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pNomineeDOB", pNomineeDOB);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pNomineePinCode", pNomineePinCode);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pNomineeAddress.Length + 1, "@pNomineeAddress", pNomineeAddress);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNomineeGender", Convert.ToInt32(pNomineeGender));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNomineeRelation", Convert.ToInt32(pNomineeRelation));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNomineeState", Convert.ToInt32(pNomineeState));
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pXmlDirector.Length + 1, "@pXmlDirector", pXmlDirector);
                    DBUtility.Execute(oCmd);

                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    vErMsg = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                    pNewId = Convert.ToString(oCmd.Parameters["@pNewId"].Value);
                    if (vErr == 0)
                    {
                        return "Record Saved Successfully:" + pNewId;
                    }
                    else
                    {
                        if (vErMsg != "")
                        {
                            return vErMsg;
                        }
                        else
                        {
                            return "Failed:Data Not Saved";
                        }
                    }
                }
                catch (Exception ex)
                {
                    return "Failed:" + ex.Message;
                }
                finally
                {
                    oCmd.Dispose();
                }
            }
            else
            {
                return vErrMsg;
            }
        }

        public Int32 chkDdupMEL_OWN(string pPANCardNumber, string pAadhaarDocumentNumber, string pPOANumber, string pBranchCode, ref string pErrMsg, string pCustId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "chkDdupMEL_OWN";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPANCardNumber", pPANCardNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarDocumentNumber", pAadhaarDocumentNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPOANumber", pPOANumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", "OWN");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 13, "@pCustId", pCustId);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 66;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region Mob_SaveCoApplicant

        public string Mob_SaveCoApplicant(string pCoAppliantId, string pCustId, string pDOA, string pCoAppName,
            string pDOB, string pAge, string pGender, string pReligionId, string pCaste, string pOccuId, string pMaritalSt, string pQualificationId,
            string pPreAddress1, string pPreAddress2, string pPreDistrict, string pPreState, string pPrePIN, string pSameAddYN, string pPerAddress1,
            string pPerAddress2, string pPerDistrict, string pPerState, string pPerPIN, string pAddressId, string pAddressIdNo, string pPhotoId,
            string pPhotoIdNo, string pContMNo, string pContTelNo, string pEmail, string pContFAXNo, string pIsDirector, string pIsPartner,
            string pBranchCode, string pYearAtRes, string pYearAtBus, string pCreatedBy, string pGuarantor,
            string isPropietor, string IsSpouse, string SamAddAsApp, string pIsActive, string pShareHoldPer, string pCoAppType, string pComTypeID,
            string pPropertyTypeId, string pAppName, string pIsPrimaryCoAppYN, string pCustCoAppRel, string pRelationId, string pRelativeName, string pXml,
            string pPhyChallangeYN, string pACHolderName, string pACNo, string pBankName, string pIFSCCode, string pYrOfOpening, string pAccountType,
            string pEmpOrgName, string pEmpDesig, string pEmpRetiredAge, string pEmpCode, string pEmpCurExp, string pEmpTotExp, string pBusGSTAppYN,
            string pBusGSTNo, string pBusLandMark, string pBusAddress1, string pBusAddress2, string pBusLocality, string pBusCity, string pBusPIN,
            string pBusState, string pBusMob, string pBusPhone, string pCommunAddress, string pResidentialStatus, string pNominee, string pBusinessName,
            string pID1VoterResponse, string pID1AadharResponse, string pNameMatchingResponse, string pIdVerifyYN)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pNewId = "";
            string pCoApplicantNo = "", xmlID1VoterResponse = "", xmlID1AadharResponse = "", xmlNameMatchingResponse = "", vAadharNo = "", vMaskedAadhaar = "", vAadhaarRefNo = "";
            #region Karza XML
            pID1VoterResponse = pID1VoterResponse == null ? "" : pID1VoterResponse.Replace("\u0000", "");
            pID1VoterResponse = pID1VoterResponse.Replace("\\u0000", "");
            xmlID1VoterResponse = AsString(JsonConvert.DeserializeXmlNode(pID1VoterResponse, "root"));

            pID1AadharResponse = pID1AadharResponse == null ? "" : pID1AadharResponse.Replace("\u0000", "");
            pID1AadharResponse = pID1AadharResponse.Replace("\\u0000", "");
            xmlID1AadharResponse = AsString(JsonConvert.DeserializeXmlNode(pID1AadharResponse, "root"));
            xmlNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(pNameMatchingResponse == null ? "" : pNameMatchingResponse, "root"));
            #endregion

            #region AadharVault
            if (Convert.ToInt32(pPhotoId) == 6 || Convert.ToInt32(pAddressId) == 6)
            {
                vAadharNo = Convert.ToInt32(pPhotoId) == 6 ? pPhotoIdNo : pAddressIdNo;
                vMaskedAadhaar = String.Format("{0}{1}", "********", vAadharNo.Substring(vAadharNo.Length - 4, 4));
                vAadhaarRefNo = AadhaarVaultCalling(vAadharNo, pContMNo, pCreatedBy);
                if (vAadhaarRefNo != "")
                {
                    pPhotoIdNo = Convert.ToInt32(pPhotoId) == 6 ? vAadhaarRefNo : pPhotoIdNo;
                    pAddressIdNo = Convert.ToInt32(pAddressId) == 6 ? vAadhaarRefNo : pAddressIdNo;
                    pPhotoId = Convert.ToInt32(pPhotoId) == 6 ? "15" : pPhotoId;
                    pAddressId = Convert.ToInt32(pAddressId) == 6 ? "15" : pAddressId;
                }
                else
                {
                    return "Failed:Unable to get aadhaar ref no.";
                }
            }
            #endregion

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveCoApplicant";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCoAppliantId", pCoAppliantId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOA", pDOA);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 95, "@pCoAppName", pCoAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", pDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAge", Convert.ToInt32(pAge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGender", Convert.ToInt32(pGender));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pReligionId", Convert.ToInt32(pReligionId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCaste", Convert.ToInt32(pCaste));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccuId", Convert.ToInt32(pOccuId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMaritalSt", Convert.ToInt32(pMaritalSt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQualificationId", pQualificationId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPreAddress1", pPreAddress1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPreAddress2", pPreAddress2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPreDistrict", pPreDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPreState", pPreState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPrePIN", pPrePIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pSameAddYN", pSameAddYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPerAddress1", pPerAddress1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPerAddress2", pPerAddress2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPerDistrict", pPerDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPerState", pPerState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPerPIN", pPerPIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressId", Convert.ToInt32(pAddressId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAddressIdNo", pAddressIdNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPhotoId", Convert.ToInt32(pPhotoId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPhotoIdNo", pPhotoIdNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pContMNo", pContMNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pContTelNo", pContTelNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEmail", pEmail);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pContFAXNo", pContFAXNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIsDirector", pIsDirector);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pIsPartner", pIsPartner);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYearAtRes", Convert.ToInt32(pYearAtRes));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYearAtBus", Convert.ToInt32(pYearAtBus));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", "Save");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pGuarantor", pGuarantor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@isPropietor", isPropietor);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@IsSpouse", IsSpouse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@SamAddAsApp", SamAddAsApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsActive", pIsActive);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pShareHoldPer", Convert.ToDecimal(pShareHoldPer));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoAppType", pCoAppType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pComTypeID", Convert.ToInt32(pComTypeID));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropertyTypeId", Convert.ToInt32(pPropertyTypeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAppName", pAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsPrimaryCoAppYN", pIsPrimaryCoAppYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCustCoAppRel", Convert.ToInt32(pCustCoAppRel));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(pRelationId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRelativeName", pRelativeName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pXml.Length + 1, "@pXml", pXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPhyChallangeYN", pPhyChallangeYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pACHolderName", pACHolderName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pACNo", pACNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBankName", pBankName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIFSCCode ", pIFSCCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrOfOpening", Convert.ToInt32(pYrOfOpening));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAccountType", Convert.ToInt32(pAccountType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmpOrgName", pEmpOrgName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmpDesig", pEmpDesig);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpRetiredAge", Convert.ToInt32(pEmpRetiredAge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pEmpCode", pEmpCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpCurExp", Convert.ToInt32(pEmpCurExp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pEmpTotExp", Convert.ToInt32(pEmpTotExp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusGSTAppYN", pBusGSTAppYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBusGSTNo", pBusGSTNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusLandMark ", pBusLandMark);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusAddress1 ", pBusAddress1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusAddress2 ", pBusAddress2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pBusLocality ", pBusLocality);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pBusCity ", pBusCity);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pBusPIN ", pBusPIN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pBusState  ", pBusState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pBusMob  ", pBusMob);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pBusPhone  ", pBusPhone);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCommunAddress", pCommunAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pNominee", pNominee);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pResidentialStatus", pResidentialStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pNewId", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCoApplicantNo", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobStatus", "M");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusinessName", pBusinessName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarNo", vMaskedAadhaar);

                if (xmlID1VoterResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pID1VoterResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1VoterResponse.Length + 1, "@pID1VoterResponse", xmlID1VoterResponse);

                if (xmlID1AadharResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pID1AadharResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlID1AadharResponse.Length + 1, "@pID1AadharResponse", xmlID1AadharResponse);

                if (xmlNameMatchingResponse == null)
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, 8, "@pNameMatchingResponse", DBNull.Value);
                else
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, xmlNameMatchingResponse.Length + 1, "@pNameMatchingResponse", xmlNameMatchingResponse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIdVerifyYN", pIdVerifyYN);
                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pNewId = Convert.ToString(oCmd.Parameters["@pNewId"].Value);
                pCoApplicantNo = Convert.ToString(oCmd.Parameters["@pCoApplicantNo"].Value);

                if (vErr == 0)
                {
                    return "Record Saved Successfully:" + pNewId;
                }
                else
                {
                    return "Failed:Data Not Saved";
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region KYCImageUpload

        public List<KYCImageSave> KYCImageUpload(Stream image)
        {
            string vErr = "";
            List<KYCImageSave> row = new List<KYCImageSave>();
            string kycNo = "";

            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "kycNo")
                        {
                            kycNo = content.StringData;
                        }
                    }
                    else
                    {
                        byte[] binaryWriteArray = content.Data;
                        string fileName = Path.GetFileName(content.FileName);
                        string vFileTag = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            vErr = SaveMemberImages(binaryWriteArray, "CustomerKYCImage", kycNo, vFileTag);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new KYCImageSave("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new KYCImageSave("Failed", fileName));
                            }
                        }
                        else
                        {
                            row.Add(new KYCImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new KYCImageSave("Failed", "No Data Found"));
            }
            return row;
        }

        public List<KYCImageSave> CoAppImageUpload(Stream image)
        {
            string vErr = "";
            List<KYCImageSave> row = new List<KYCImageSave>();
            string kycNo = "";

            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "kycNo")
                        {
                            kycNo = content.StringData;
                        }
                    }
                    else
                    {
                        byte[] binaryWriteArray = content.Data;
                        string fileName = Path.GetFileName(content.FileName);
                        string vFileTag = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            vErr = SaveMemberImages(binaryWriteArray, "CoApplicantKYCImage", kycNo, vFileTag);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new KYCImageSave("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new KYCImageSave("Failed", fileName));
                            }
                        }
                        else
                        {
                            row.Add(new KYCImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new KYCImageSave("Failed", "No Data Found"));
            }
            return row;
        }

        public List<KYCImageSave> DocUpload(Stream image)
        {
            string vErr = "";
            List<KYCImageSave> row = new List<KYCImageSave>();
            string kycNo = "";

            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "kycNo")
                        {
                            kycNo = content.StringData;
                        }
                    }
                    else
                    {
                        byte[] binaryWriteArray = content.Data;
                        string fileName = Path.GetFileName(content.FileName);
                        string vFileTag = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            vErr = SaveMemberImages(binaryWriteArray, "Document", kycNo, vFileTag);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new KYCImageSave("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new KYCImageSave("Failed", fileName));
                            }
                        }
                        else
                        {
                            row.Add(new KYCImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new KYCImageSave("Failed", "No Data Found"));
            }
            return row;
        }


        private System.Drawing.Image LoadImage(byte[] bytes)
        {
            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }
            return image;
        }

        private string SaveMemberImages(byte[] imageBinary, string imageGroup, string folderName, string imageName)
        {
            string isImageSaved = "N";
            if (MinioYN == "N")
            {
                string folderPath = HostingEnvironment.MapPath(string.Format("~/{0}/{1}", imageGroup, folderName));
                System.IO.Directory.CreateDirectory(folderPath);
                string filePath = string.Format("{0}/{1}.png", folderPath, imageName);
                if (imageBinary != null)
                {
                    File.WriteAllBytes(filePath, imageBinary);
                    isImageSaved = "Y";
                }
            }
            else
            {
                string BucketName = imageGroup == "CustomerKYCImage" ? CustomerKYCImageBucket : imageGroup == "CoApplicantKYCImage" ? CoApplicantKYCImageBucket : DocumentBucket;
                isImageSaved = UploadFileMinio(imageBinary, imageName + ".png", folderName, BucketName, MinioUrl);
            }
            return isImageSaved;
        }

        #endregion

        #region Minio Image Upload
        public string UploadFileMinio(byte[] image, string fileName, string enqId, string bucketName, string minioUrl)
        {
            string fullResponse = "", isImageSaved = "N";
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("image", Convert.ToBase64String(image));
            postParameters.Add("KycId", enqId);
            postParameters.Add("BucketName", bucketName);
            postParameters.Add("ImageName", fileName);
            string postURL = minioUrl;
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;
            byte[] formDataforRequest = GetMultipartFormData(postParameters, formDataBoundary);
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();
                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formDataforRequest.Length;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formDataforRequest, 0, formDataforRequest.Length);
                    requestStream.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    fullResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            dynamic obj = JsonConvert.DeserializeObject(fullResponse);
            bool status = obj.status;
            if (status == true)
            {
                isImageSaved = "Y";
            }
            return isImageSaved;
        }
        #endregion

        #region PopComboMISData

        public List<ComboMisData> PopComboMISData(string pCondition, string pCodeAdd, string pCodeName, string pIDName, string pName, string pTbl, string pConID, string pConIDName,
            string pConIDDate, string pConDate, string pBranchCode)
        {
            DataTable dt = new DataTable();
            List<ComboMisData> row = new List<ComboMisData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_PopComboMISData(pCondition, pCodeAdd, pCodeName, pIDName, pName, pTbl, pConID, pConIDName, pConIDDate, pConDate, pBranchCode);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new ComboMisData(rs["SourceID"].ToString(), rs["SourceName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new ComboMisData("No Data Available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new ComboMisData("No Data Available", ex.Message));
            }
            return row;
        }

        #endregion

        #region GetActiveLnSchemePG

        public List<LoanSchemeData> GetActiveLnSchemePG(string pBranch)
        {
            DataTable dt = new DataTable();
            List<LoanSchemeData> row = new List<LoanSchemeData>();
            CRepository oRepo = null;
            try
            {
                oRepo = new CRepository();
                dt = oRepo.Mob_GetActiveLnSchemePG(pBranch);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new LoanSchemeData(rs["RowId"].ToString(), rs["LoanTypeId"].ToString(), rs["LoanTypeName"].ToString(), rs["LoanAmt"].ToString(), rs["New_Prod"].ToString()));
                    }
                }
                else
                {
                    row.Add(new LoanSchemeData("No Data Available", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new LoanSchemeData("No Data Available", ex.Message, "", "", ""));
            }
            return row;
        }

        #endregion

        #region GetCoAppByCustId

        public List<LoanAppCoListData> GetCoAppByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            List<LoanAppCoListData> row = new List<LoanAppCoListData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetCoAppByCustId(pCustId);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new LoanAppCoListData(rs["CoApplicantId"].ToString(), rs["CoAppName"].ToString(), rs["ScoreValue"].ToString(), rs["ReportID"].ToString()));
                    }
                }
                else
                {
                    row.Add(new LoanAppCoListData("No Data Available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new LoanAppCoListData("No Data Available", ex.Message, "", ""));
            }
            return row;
        }

        #endregion

        #region GetLoanAppCustForHighmarkList

        public List<GetAppCustLoanAppData> GetLoanAppCustForHighmarkList(string pMode, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            List<GetAppCustLoanAppData> row = new List<GetAppCustLoanAppData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetLoanAppCustForHighmarkList(pMode, pCreatedBy);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetAppCustLoanAppData(rs["CustId"].ToString(), rs["CompanyName"].ToString(), rs["CustNo"].ToString(), rs["LeadId"].ToString(), rs["LoanAppId"].ToString(), rs["LoanAppNo"].ToString(),
                                                            rs["ApplicationDt"].ToString(), rs["AppAmount"].ToString(), rs["Tenure"].ToString(), rs["PurposeID"].ToString(), rs["LoanTypeId"].ToString(),
                                                            rs["CBPassYN"].ToString(), rs["CBPassDate"].ToString(), rs["SourceID"].ToString(), rs["PassorRejDate"].ToString(), rs["RejReason"].ToString(),
                                                            rs["ScoreValue"].ToString(), rs["Mode"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetAppCustLoanAppData("No Data Available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetAppCustLoanAppData("No Data Available", ex.Message, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region VerifyEquifax
        public string VerifyEquifax(string pLoanAppId, string pApplicantId, string pLoanAmt, string pMode, string pBranchCode, string pLogDt, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            CRepository oRepo = null;
            string vMsg = "", pStatusDesc = "";
            string pEqXml = "";
            int vErr = 0, pStatus = 0;
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

            PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
            PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

            try
            {
                oRepo = new CRepository();
                //pMode = "A"-Customer/Applicant,"C"-CoApplicant
                dt = oRepo.GetMemberInfo(pApplicantId, "E", pMode);
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        ////*************************** For Live ***************************************************                      
                        WebServiceSoapClient eq = new WebServiceSoapClient();
                        ////************************************************GenderId 1 For MALE else Female****************************************
                        //if (dt.Rows[0]["GenderId"].ToString() == "1")
                        //{
                        //    //--------------------------------------------------LIVE--------------------------------------------------
                        //    pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                        //        , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                        //         dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                        //          dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                        //          "5750", PCSUserName, PCSPassword, "027FP27137", "9GH", " ", "PCS", "ERS", "3.1", "PRO");
                        //    //--------------------------------------------------UAT----------------------------------------------------
                        //    //pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                        //    //    , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                        //    //     dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                        //    //      dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                        //    //      "21", PCSUserName, PCSPassword, "999AA00007", "54J", " ", "PCS", "ERS", "3.1");
                        //}
                        //else
                        //{
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                             dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                              dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                               "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                        //pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                        //    , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                        //     dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                        //      dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                        //       "21", CCRUserName, CCRPassword, "028FZ00016", "FR7", "123456", "CCR", "ERS", "3.1");
                        //}
                        //*************************************************************************
                        vErr = oRepo.UpdateEquifaxInformation(pLoanAppId, Convert.ToInt32(dt.Rows[0]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), setDate(pLogDt), "P", "", ref pStatus, ref pStatusDesc);
                        if (vErr == 1)
                        {
                            return "Equifax Verification Successful" + ":" + pStatusDesc;
                        }
                        else if (vErr == 5)
                        {
                            return "Equifax Verification Failed" + ":" + pStatusDesc;
                        }
                        else
                        {
                            return "Equifax Verification Failed" + ":" + pStatusDesc;
                        }
                    }
                    catch (Exception ex)
                    {
                        vErr = oRepo.UpdateEquifaxInformation(pLoanAppId, Convert.ToInt32(dt.Rows[0]["CBID"]), pEqXml, pBranchCode, "", Convert.ToInt32(pCreatedBy), setDate(pLogDt), "P", ex.ToString(), ref pStatus, ref pStatusDesc);
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

        #region SaveLoanApplication

        public string SaveLoanApplication(string pApplicantID, string pAppDt, string pPurpId, string pLnTypeId, string pAppAmt, string pTenure,
            string pCBPassYN, string pBrCode, string pCreatedBy, string pEntType, string pYrNo, string pSourceId, string pXmlCoApp, string pPassYN,
            string pPassorRejDate, string pRejReason, string pSRC)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pLoanAppNo = "", pErrDesc = "";
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_SaveInitialApplication";

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pApplicantID", pApplicantID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDt", pAppDt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurpId", Convert.ToInt32(pPurpId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLnTypeId", Convert.ToInt32(pLnTypeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pAppAmt", Convert.ToDecimal(pAppAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCBPassYN", pCBPassYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pEntType", pEntType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", Convert.ToInt32(pYrNo));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSourceId", Convert.ToInt32(pSourceId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlCoApp.Length + 1, "@pXmlCoApp", pXmlCoApp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPassYN", pPassYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pPassorRejDate", pPassorRejDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pRejReason", pRejReason);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanAppNo", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", pSRC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1000, "@pErrDesc", pErrDesc);

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
                    if (vDigiConcentSMSLanguage.ToUpper() == "ENGLISH")
                    {
                        vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", "https://bijliftt.com/m/" + Encode(Convert.ToInt64(pLoanAppNo)));
                    }
                    else
                    {
                        vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", HttpUtility.UrlEncode("https://bijliftt.com/m/" + Encode(Convert.ToInt64(pLoanAppNo)), Encoding.GetEncoding("ISO-8859-1")));
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
                    return pErrDesc + pLoanAppNo;

                }
                else
                {
                    return pErrDesc;
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region GetPendingPDBucketList

        public List<PendingPDBucketListData> GetPendingPDBucketList(string pBrCode)
        {
            DataTable dt = new DataTable();
            List<PendingPDBucketListData> row = new List<PendingPDBucketListData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetPendingPDBucketList(pBrCode);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new PendingPDBucketListData(rs["LoanAppId"].ToString(), rs["LoanAppNo"].ToString(), rs["CompanyName"].ToString(),
                            rs["CustNo"].ToString(), rs["LoanTypeName"].ToString(), rs["PurposeName"].ToString(), rs["ApplicationDt"].ToString(),
                            rs["AppAmount"].ToString(), rs["Tenure"].ToString()));
                    }
                }
                else
                {
                    row.Add(new PendingPDBucketListData("No Data Available", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new PendingPDBucketListData("No Data Available", ex.Message, "", "", "", "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region GetLnAppDetailsForPD

        public List<PDDetails> GetLnAppDetailsForPD(string pLoanAppId)
        {
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            List<AppDetailsData> row = new List<AppDetailsData>();
            List<CoAppDetailsData> row2 = new List<CoAppDetailsData>();
            List<PDDetails> rowFinal = new List<PDDetails>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                ds = oRepo.Mob_GetLnAppDetailsForPD(pLoanAppId);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row.Add(new AppDetailsData(rs["LoanAppId"].ToString(), rs["LoanAppNo"].ToString(),
                                                    rs["CompanyName"].ToString(), rs["CustId"].ToString(),
                                                    rs["LoanTypeName"].ToString(), rs["PurposeName"].ToString(),
                                                    rs["ApplicationDt"].ToString(), rs["AppAmount"].ToString(),
                                                    rs["Tenure"].ToString(), rs["SanctionYN"].ToString(),
                                                    rs["FinalPDStatus"].ToString(), rs["FinalPDRemarks"].ToString(), rs["FinalPDDate"].ToString()));
                    }
                }
                else
                {
                    row.Add(new AppDetailsData("No Data Available", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new CoAppDetailsData(rs["CoApplicantId"].ToString(), rs["CoAppName"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new CoAppDetailsData("No Data Available", ""));
                }

                rowFinal.Add(new PDDetails(row, row2));
            }
            catch (Exception ex)
            {
                rowFinal.Add(new PDDetails(row, row2));
            }
            return rowFinal;
        }

        #endregion

        #region SavePDMst

        public string SavePDMst(string pPDId, string pLoanAppId, string pCustID, string pCoAppID, string pPDType,
            string pPDDate, string pRemarks, string pTotalSellingIncome, string pTotalCostIncome, string pTotalIncomeMargin, string pTotalExpensesMonthly,
            string pBrCode, string pCreatedBy, string pXmlIncome, string pXmlExpenses, string pSRC)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrMsg = "";

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDMst";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPDId", Convert.ToInt32(pPDId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCoAppID", pCoAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPDType", pPDType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pPDDate", pPDDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalSellingIncome", Convert.ToDecimal(pTotalSellingIncome));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalCostIncome", Convert.ToDecimal(pTotalCostIncome));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalIncomeMargin", Convert.ToDecimal(pTotalIncomeMargin));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 18, "@pTotalExpensesMonthly", Convert.ToDecimal(pTotalExpensesMonthly));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMobStatus", "M");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlIncome.Length + 1, "@pXmlIncome", pXmlIncome);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlExpenses.Length + 1, "@pXmlExpenses", pXmlExpenses);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", "Save");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pErrMsg", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", pSRC);

                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);

                if (vErr == 0)
                {
                    return "Record Saved Successfully";
                }
                else
                {
                    return "Failed:" + pErrMsg;
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region Mob_GetDesigByDesigId

        public List<DesigData> Mob_GetDesigByDesigId(string pDesigId)
        {
            DataTable dt = new DataTable();
            List<DesigData> row = new List<DesigData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.GetDesigByDesigId(pDesigId);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new DesigData(rs["Desig"].ToString()));
                    }
                }
                else
                {
                    row.Add(new DesigData("No Data Available"));
                }
            }
            catch (Exception ex)
            {
                row.Add(new DesigData(ex.Message));
            }
            return row;
        }

        #endregion

        #region GetCustForRecovery

        public List<CustRecoveryData> GetCustForRecovery(string pBrCode, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            List<CustRecoveryData> row = new List<CustRecoveryData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetCustForRecovery(pBrCode, pCreatedBy);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new CustRecoveryData(rs["CustId"].ToString(), rs["CustName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new CustRecoveryData("No Data Available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new CustRecoveryData("No Data Available", ex.Message));
            }
            return row;
        }

        #endregion

        #region GetLoannoForBounce

        public List<GetLoanNoData> GetLoannoForBounce(string CustId)
        {
            DataTable dt = new DataTable();
            List<GetLoanNoData> row = new List<GetLoanNoData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetLoannoForBounce(CustId);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLoanNoData(rs["LoanId"].ToString(), rs["LoanNo"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetLoanNoData("No Data Available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetLoanNoData("No Data Available", ex.Message));
            }
            return row;
        }

        #endregion

        #region GetCollectionByLoanId

        public List<GetCollData> GetCollectionByLoanId(string pLoanId, string pCollDt, string pBranch)
        {
            DataTable dt = new DataTable();
            List<GetCollData> row = new List<GetCollData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetCollectionByLoanIdNew(pLoanId, pCollDt, pBranch);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetCollData(rs["ClosingType"].ToString(), rs["LoanId"].ToString(), rs["DisbDate"].ToString(), rs["IntRate"].ToString(),
                            rs["DisbAmt"].ToString(), rs["PrincpalDue"].ToString(), rs["InterestDue"].ToString(), rs["PrncOutStd"].ToString(),
                            rs["IntOutStd"].ToString(), rs["PaidPric"].ToString(), rs["PaidInt"].ToString(), rs["BounceDue"].ToString(),
                            rs["LoanTypeId"].ToString(), rs["LoanTypeName"].ToString(), rs["FunderName"].ToString(), rs["LastTransDt"].ToString(), rs["EMIAmt"].ToString(),
                            rs["FlDGBal"].ToString(), rs["IntDue"].ToString(), rs["CollMode"].ToString(), rs["PenaltyAmt"].ToString(), rs["PenCGST"].ToString()
                            , rs["PenSGST"].ToString(), rs["VisitCharge"].ToString(), rs["VisitChargeCGST"].ToString(), rs["VisitChargeSGST"].ToString(), rs["GrandTotal"].ToString()
                            ));
                    }
                }
                else
                {
                    row.Add(new GetCollData("No Data Available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetCollData("No Data Available", ex.Message, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            return row;
        }

        #endregion

        #region Mob_InsertCollection

        public string Mob_InsertCollection(string pLoanId, string pAccDate, string pCustId, string pPrincColl, string pIntColl, string pPenColl,
            string pWaveIntColl, string pTotalColl, string pPrinOS, string pBrachCode, string pCreatedBy, string pBounceRecv, string pBounceWave, string pClosingType,
            string pPreCloseCharge, string pPreCloseWaive, string pBounceDue, string pIntDue, string pPreCollMode, string pPreClsCGST, string pPreClsSGST)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrDesc = "";

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_InsertCollection";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAccDate", pAccDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPrincColl", Convert.ToDecimal(pPrincColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIntColl", Convert.ToDecimal(pIntColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPenColl", Convert.ToDecimal(pPenColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pWaveIntColl", Convert.ToDecimal(pWaveIntColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalColl", Convert.ToDecimal(pTotalColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPrinOS", Convert.ToDecimal(pPrinOS));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", "C");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", "C");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pDescId", "C0001");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBankName", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChqReffNo", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrachCode", pBrachCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBounceRecv", Convert.ToDecimal(pBounceRecv));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBounceWave", Convert.ToDecimal(pBounceWave));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pClosingType", pClosingType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreCloseCharge", Convert.ToDecimal(pPreCloseCharge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreCloseWaive", Convert.ToDecimal(pPreCloseWaive));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBounceDue", Convert.ToDecimal(pBounceDue));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIntDue", Convert.ToDecimal(pIntDue));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPreCollMode", pPreCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreClsCGST", Convert.ToDecimal(pPreClsCGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreClsSGST", Convert.ToDecimal(pPreClsSGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);

                if (vErr == 0)
                {
                    return "Record Saved Successfully";
                }
                else
                {
                    return "Failed:" + pErrDesc;
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string Mob_InsertCollectionNew(string pLoanId, string pAccDate, string pCustId, string pPrincColl, string pIntColl, string pPenColl,
                string pPenCGST, string pPenSGST, string pVisitCharge, string pVisitChargeCGST, string pVisitChargeSGST, string pTotalColl, string pPrinOS,
                string pBrachCode, string pCreatedBy, string pBounceRecv, string pClosingType, string pIntDue)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrDesc = "";

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_InsertCollectionNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAccDate", pAccDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustId", pCustId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPrincColl", Convert.ToDecimal(pPrincColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIntColl", Convert.ToDecimal(pIntColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPenColl", Convert.ToDecimal(pPenColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPenCGST", Convert.ToDecimal(pPenCGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPenSGST", Convert.ToDecimal(pPenSGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pVisitCharge", Convert.ToDecimal(pVisitCharge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pVisitChargeCGST", Convert.ToDecimal(pVisitChargeCGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pVisitChargeSGST", Convert.ToDecimal(pVisitChargeSGST));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalColl", Convert.ToDecimal(pTotalColl));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPrinOS", Convert.ToDecimal(pPrinOS));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", "C");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", "C");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pDescId", "C0001");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBankName", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pChqReffNo", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pBrachCode", pBrachCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBounceRecv", Convert.ToDecimal(pBounceRecv));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pClosingType", pClosingType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIntDue", Convert.ToDecimal(pIntDue));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);

                if (vErr == 0)
                {
                    return "Record Saved Successfully";
                }
                else
                {
                    return "Failed:" + pErrDesc;
                }

            }
            catch (Exception ex)
            {
                return "Failed:" + ex.Message;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region GetAsonDateIntDue

        public List<AsonDateData> GetAsonDateIntDue(string LoanId, string AsonDate, string LastCollDt, string PrinOS, string IntRate, string LoanDt)
        {
            DataTable dt = new DataTable();
            List<AsonDateData> row = new List<AsonDateData>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.Mob_GetAsonDateIntDue(LoanId, AsonDate, LastCollDt, PrinOS, IntRate, LoanDt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new AsonDateData(rs["InterestDue"].ToString(), rs["PreCloseCharge"].ToString(), rs["PreCloseCGST"].ToString(), rs["PreCloseSGST"].ToString()));
                    }
                }
                else
                {
                    row.Add(new AsonDateData("No Data Available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new AsonDateData("No Data Available", ex.Message, "", ""));
            }
            return row;
        }

        #endregion

        #region popCategoryVariables

        public List<CategoryVariables> popCategoryVariables()
        {
            DataTable dt = new DataTable();
            List<CategoryVariables> row = new List<CategoryVariables>();
            CRepository oRepo = null;
            try
            {

                oRepo = new CRepository();
                dt = oRepo.popCategoryVariables();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new CategoryVariables(rs["CategoryID"].ToString(), rs["Category"].ToString(), rs["VariableId"].ToString(), rs["Variable"].ToString(), rs["Options"].ToString()));
                    }
                }
                else
                {
                    row.Add(new CategoryVariables("No Data Available", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new CategoryVariables("No Data Available", ex.Message, "", "", ""));
            }
            return row;
        }

        #endregion

        #region SaveLoanApplicationQA

        public string SaveLoanApplicationQA(LoanApplicationQAData loanApplicationQAData)
        {
            try
            {
                DataTable dt = new DataTable();
                string vErr = "";
                CRepository oCR = null;
                List<string> vArrQid = loanApplicationQAData.pQid.Split(new char[] { ',' }).ToList();
                List<string> vArrAnsID = loanApplicationQAData.pAnsID.Split(new char[] { ',' }).ToList();
                List<string> vArrCmt = loanApplicationQAData.pCmt.Split(new char[] { ',' }).ToList();

                dt.Columns.Add("VariableId", typeof(int));
                dt.Columns.Add("Option", typeof(string));
                dt.Columns.Add("Comment", typeof(string));
                dt.TableName = "LoanApplicationQA";
                DataRow dr = null;
                string vXmlData = "";
                for (int i = 0; i < vArrQid.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["VariableId"] = vArrQid[i];
                    dr["Option"] = vArrAnsID[i];
                    dr["Comment"] = vArrCmt[i] == "NA" ? "" : vArrCmt[i];
                    dt.Rows.Add(dr);
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                oCR = new CRepository();
                vErr = oCR.SaveLoanAppQA(loanApplicationQAData.pLoanAppId, vXmlData);
                return vErr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region GetPDQuestionAnswerMstByLnAppId
        public List<PDQuestionAnswerMst> GetPDQuestionAnswerMstByLnAppId(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<PDQuestionAnswerMst> row = new List<PDQuestionAnswerMst>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPDQuestionAnswerMstByLnAppId";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLnAppId", pLoanAppId);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new PDQuestionAnswerMst(rs["LoanAppId"].ToString(), rs["ID"].ToString(), rs["Name"].ToString(), rs["CustType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new PDQuestionAnswerMst("No Data Available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new PDQuestionAnswerMst("No Data Available", ex.Message, "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region SavePDPersonalDetail

        public string SavePDPersonalDetail(string pLoanAppId, string pPDDoneBy, string pPDDate,
         string pAppName, string pAppNameObj, string pAppNameVer, string pAppNameRemark,
         string pAppRelId, string pAppRelObj, string pAppRelVer, string pAppRelRemark,
         string pDOB, string pDOBObj, string pDOBVer, string pDOBRemark,
         string pAge, string pAgeObj, string pAgeVer, string pAgeRemark,
         string pMaritalId, string pMaritalObj, string pMaritalVer, string pMaritalRemark,
         string pNoOfChild, string pNoOfChildObj, string pNoOfChildVer, string pNoOfChildRemark,
         string pEarMem, string pEarMemObj, string pEarMemVer, string pEarMemRemark,
         string pPropOwn, string pPropOwnObj, string pPropOwnVer, string pPropOwnRemark,
         string pOwnType, string pOwnTypeObj, string pOwnTypeVer, string pOwnTypeRemark,
         string pHouseObs, string pHouseObsObj, string pHouseObsVer, string pHouseObsRemark,
         string pPerAddress, string pPerAddressObj, string pPerAddressVer, string pPerAddressRemark,
         string pRefNm, string pRefNmObj, string pRefNmVer, string pRefNmRemark,
         string pMobNo, string pMobNoObj, string pMobNoVer, string pMobNoRemark,
         string pLegalInfo, string pLegalInfoObj, string pLegalInfoVer, string pLegalInfoRemark,
         string pPolInfo, string pPolInfoObj, string pPolInfoVer, string pPolInfoRemark,
         string pPolStatus, string pPolStatusObj, string pPolStatusVer, string pPolStatusRemark,
         string pCreatedBy, string pCustID, string pCustType, string pSchool, string pSchoolObj
           , string pSchoolVer, string pSchoolRemark, string pLatitude, string pLongitude, string pAddress)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDPersonalDetail";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAppName", pAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppNameObj", pAppNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppNameVer", pAppNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppNameRemark", pAppNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAppRelId", Convert.ToInt32(pAppRelId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppRelObj", pAppRelObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppRelVer", pAppRelVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAppRelRemark", pAppRelRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDOB", setDate(pDOB));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOBObj", pDOBObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOBVer", pDOBVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOBRemark", pDOBRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAge", Convert.ToInt32(pAge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAgeObj", pAgeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAgeVer", pAgeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAgeRemark", pAgeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMaritalId", Convert.ToInt32(pMaritalId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMaritalObj", pMaritalObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMaritalVer", pMaritalVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMaritalRemark", pMaritalRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfChild", Convert.ToInt32(pNoOfChild));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfChildObj", pNoOfChildObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfChildVer", pNoOfChildVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfChildRemark", pNoOfChildRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEarMem", pEarMem);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEarMemObj", pEarMemObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEarMemVer", pEarMemVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEarMemRemark", pEarMemRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropOwn", Convert.ToInt32(pPropOwn));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropOwnObj", pPropOwnObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropOwnVer", pPropOwnVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropOwnRemark", pPropOwnRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOwnType", Convert.ToInt32(pOwnType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOwnTypeObj", pOwnTypeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOwnTypeVer", pOwnTypeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pOwnTypeRemark", pOwnTypeRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHouseObs", pHouseObs);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHouseObsObj", pHouseObsObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHouseObsVer", pHouseObsVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHouseObsRemark", pHouseObsRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPerAddress", pPerAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPerAddressObj", pPerAddressObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPerAddressVer", pPerAddressVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPerAddressRemark", pPerAddressRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRefNm", pRefNm);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRefNmObj", pRefNmObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRefNmVer", pRefNmVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRefNmRemark", pRefNmRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo", pMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNoObj", pMobNoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNoVer", pMobNoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNoRemark", pMobNoRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLegalInfo", pLegalInfo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLegalInfoObj", pLegalInfoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLegalInfoVer", pLegalInfoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLegalInfoRemark", pLegalInfoRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolInfo", pPolInfo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolInfoObj", pPolInfoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolInfoVer", pPolInfoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolInfoRemark", pPolInfoRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolStatus", pPolStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolStatusObj", pPolStatusObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolStatusVer", pPolStatusVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPolStatusRemark", pPolStatusRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchool", pSchool);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSchoolObj", pSchoolObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSchoolVer", pSchoolVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSchoolRemark", pSchoolRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLatitude", pLatitude);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLongitude", pLongitude);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLocationAddress", pAddress);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "Record Saved Successfully";
                }
                else
                {
                    return "Faild";
                }
            }
            catch (Exception ex)
            {
                // throw ex;
                return "Faild" + ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDPersonalReference
        public string SavePDPersonalReference(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
        string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDPersonalReference";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pName", pName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pContactNo", pContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRelation", pRelation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pResidence", pResidence);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOccupation", pOccupation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pKnowHimYears", Convert.ToInt32(pKnowHimYears));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully";
                else
                    return "Data not Saved";
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

        #region SavePDIncomeSourceBusiness
        public string SavePDIncomeSourceBusiness(string pLoanAppId, string pPDDoneBy, string pPDDate,
          string pTotalIncome, string pTotalIncomeObj, string pTotalIncomeVer, string pTotalIncomeRemark,
          string pBusName, string pBusNameObj, string pBusNameVer, string pBusNameRemark,
          string pBusTypeId, string pBusTypeObj, string pBusTypeVer, string pBusTypeRemark,
          string pBusStabId, string pBusStabObj, string pBusStabVer, string pBusStabRemark,
          string pBusAddress, string pBusAddressObj, string pBusAddressVer, string pBusAddressRemark,
          string pNoOfEmp, string pNoOfEmpObj, string pNoOfEmpVer, string pNoOfEmpRemark,
          string pStockSeen, string pStockSeenObj, string pStockSeenVer, string pStockSeenRemark,
          string pVendNm1, string pVendNm1Obj, string pVendNm1Ver, string pVendNm1Remark,
          string pMobNo1, string pMobNo1Obj, string pMobNo1Ver, string pMobNo1Remark,
          string pVendNm2, string pVendNm2Obj, string pVendNm2Ver, string pVendNm2Remark,
          string pMobNo2, string pMobNo2Obj, string pMobNo2Ver, string pMobNo2Remark,
          string pBusAppName, string pBusAppNameObj, string pBusAppNameVer, string pBusAppNameRemark,
          string pNoOfCust, string pNoOfCustObj, string pNoOfCustVer, string pNoOfCustRemark,
          string pCreditAmt, string pCreditAmtObj, string pCreditAmtVer, string pCreditAmtRemark,
          string pNmBoardSeen, string pNmBoardSeenObj, string pNmBoardSeenVer, string pNmBoardSeenRemark,
          string pCreatedBy, string pCustID, string pCustType, string pGrossSales, string pGrossSalesObj,
          string pGrossSalesVer, string pGrossSalesRemark, string pGrossMargin, string pGrossMarginObj,
          string pGrossMarginVer, string pGrossMarginRemark)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourceBusiness";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotalIncome", Convert.ToDecimal(pTotalIncome));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotalIncomeObj", pTotalIncomeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotalIncomeVer", pTotalIncomeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotalIncomeRemark", pTotalIncomeRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusName", pBusName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusNameObj", pBusNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusNameVer", pBusNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusNameRemark", pBusNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusTypeId", Convert.ToInt32(pBusTypeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusTypeObj", pBusTypeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusTypeVer", pBusTypeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusTypeRemark", pBusTypeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusStabId", pBusStabId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusStabObj", pBusStabObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusStabVer", pBusStabVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusStabRemark", pBusStabRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAddress", pBusAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAddressObj", pBusAddressObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAddressVer", pBusAddressVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAddressRemark", pBusAddressRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfEmp", Convert.ToInt32(pNoOfEmp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfEmpObj", pNoOfEmpObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfEmpVer", pNoOfEmpVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfEmpRemark", pNoOfEmpRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStockSeen", pStockSeen);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pStockSeenObj", pStockSeenObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pStockSeenVer", pStockSeenVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pStockSeenRemark", pStockSeenRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm1", pVendNm1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm1Obj", pVendNm1Obj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm1Ver", pVendNm1Ver);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm1Remark", pVendNm1Remark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo1", pMobNo1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo1Obj", pMobNo1Obj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo1Ver", pMobNo1Ver);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo1Remark", pMobNo1Remark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm2", pVendNm2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm2Obj", pVendNm2Obj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm2Ver", pVendNm2Ver);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVendNm2Remark", pVendNm2Remark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobNo2", pMobNo2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo2Obj", pMobNo2Obj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo2Ver", pMobNo2Ver);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMobNo2Remark", pMobNo2Remark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAppName", pBusAppName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAppNameObj", pBusAppNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAppNameVer", pBusAppNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBusAppNameRemark", pBusAppNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfCust", Convert.ToInt32(pNoOfCust));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfCustObj", pNoOfCustObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfCustVer", pNoOfCustVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfCustRemark", pNoOfCustRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCreditAmt", Convert.ToDecimal(pCreditAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCreditAmtObj", pCreditAmtObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCreditAmtVer", pCreditAmtVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCreditAmtRemark", pCreditAmtRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pNmBoardSeen", pNmBoardSeen);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNmBoardSeenObj", pNmBoardSeenObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNmBoardSeenVer", pNmBoardSeenVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNmBoardSeenRemark", pNmBoardSeenRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pGrossSales", Convert.ToDecimal(pGrossSales));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossSalesObj", pGrossSalesObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossSalesVer", pGrossSalesVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossSalesRemark", pGrossSalesRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pGrossMargin", Convert.ToDecimal(pGrossMargin));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossMarginObj", pGrossMarginObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossMarginVer", pGrossMarginVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGrossMarginRemark", pGrossMarginRemark);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully";
                else
                    return "Faild";
            }
            catch (Exception ex)
            {
                return "Faild" + ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDIncomeSourceSalary
        public string SavePDIncomeSourceSalary(string pLoanAppId, string pPDDoneBy, string pPDDate,
          string pEmpName, string pEmpNameObj, string pEmpNameVer, string pEmpNameRemark,
          string pDesig, string pDesigObj, string pDesigVer, string pDesigRemark,
          string pDOJ, string pDOJObj, string pDOJVer, string pDOJRemark,
          string pJobStb, string pJobStbObj, string pJobStbVer, string pJobStbRemark,
          string pEmpAddress, string pEmpAddressObj, string pEmpAddressVer, string pEmpAddressRemark,
          string pSalCrModeId, string pSalCrModeObj, string pSalCrModeVer, string pSalCrModeRemark,
          string pSalTypeId, string pSalTypeObj, string pSalTypeVer, string pSalTypeRemark,
          string pBankName, string pBankNameObj, string pBankNameVer, string pBankNameRemark,
          string pNetSal, string pNetSalObj, string pNetSalVer, string pNetSalRemark,
          string pHRName, string pHRNameObj, string pHRNameVer, string pHRNameRemark,
          string pHRMobNo, string pHRMobNoObj, string pHRMobNoVer, string pHRMobNoRemark,
          string pCollName, string pCollNameObj, string pCollNameVer, string pCollNameRemark,
          string pCollMobNo, string pCollMobNoObj, string pCollMobNoVer, string pCollMobNoRemark,
          string pPreEmpName, string pPreEmpNameObj, string pPreEmpNameVer, string pPreEmpNameRemark,
          string pPreEmpAddress, string pPreEmpAddressObj, string pPreEmpAddressVer, string pPreEmpAddressRemark,
          string pPreEmpDOJ, string pPreEmpDOJObj, string pPreEmpDOJVer, string pPreEmpDOJRemark,
          string pPreEmpDOL, string pPreEmpDOLObj, string pPreEmpDOLVer, string pPreEmpDOLRemark,
          string pPreWorkExp, string pPreWorkExpObj, string pPreWorkExpVer, string pPreWorkExpRemark,
          string pTotWorkExp, string pTotWorkExpObj, string pTotWorkExpVer, string pTotWorkExpRemark,
          string pCreatedBy, string pCustID, string pCustType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourceSalary";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpName", pEmpName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpNameObj", pEmpNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpNameVer", pEmpNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpNameRemark", pEmpNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesig", pDesig);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesigObj", pDesigObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesigVer", pDesigVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDesigRemark", pDesigRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 4, "@pDOJ", setDate(pDOJ));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOJObj", pDOJObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOJVer", pDOJVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDOJRemark", pDOJRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pJobStb", pJobStb);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pJobStbObj", pJobStbObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pJobStbVer", pJobStbVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pJobStbRemark", pJobStbRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpAddress", pEmpAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpAddressObj", pEmpAddressObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpAddressVer", pEmpAddressVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpAddressRemark", pEmpAddressRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSalCrModeId", Convert.ToInt32(pSalCrModeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalCrModeObj", pSalCrModeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalCrModeVer", pSalCrModeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalCrModeRemark", pSalCrModeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSalTypeId", Convert.ToInt32(pSalTypeId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalTypeObj", pSalTypeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalTypeVer", pSalTypeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSalTypeRemark", pSalTypeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBankName", pBankName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBankNameObj", pBankNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBankNameVer", pBankNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBankNameRemark", pBankNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetSal", Convert.ToDecimal(pNetSal));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNetSalObj", pNetSalObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNetSalVer", pNetSalVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNetSalRemark", pNetSalRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRName", pHRName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRNameObj", pHRNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRNameVer", pHRNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRNameRemark", pHRNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pHRMobNo", pHRMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRMobNoObj", pHRMobNoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRMobNoVer", pHRMobNoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pHRMobNoRemark", pHRMobNoRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollName", pCollName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollNameObj", pCollNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollNameVer", pCollNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollNameRemark", pCollNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollMobNo", pCollMobNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollMobNoObj", pCollMobNoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollMobNoVer", pCollMobNoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCollMobNoRemark", pCollMobNoRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpName", pPreEmpName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpNameObj", pPreEmpNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpNameVer", pPreEmpNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpNameRemark", pPreEmpNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpAddress", pPreEmpAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpAddressObj", pPreEmpAddressObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpAddressVer", pPreEmpAddressVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpAddressRemark", pPreEmpAddressRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPreEmpDOJ", setDate(pPreEmpDOJ));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOJObj", pPreEmpDOJObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOJVer", pPreEmpDOJVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOJRemark", pPreEmpDOJRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPreEmpDOL", setDate(pPreEmpDOL));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOLObj", pPreEmpDOLObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOLVer", pPreEmpDOLVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreEmpDOLRemark", pPreEmpDOLRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPreWorkExp", Convert.ToDecimal(pPreWorkExp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreWorkExpObj", pPreWorkExpObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreWorkExpVer", pPreWorkExpVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPreWorkExpRemark", pPreWorkExpRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotWorkExp", Convert.ToDecimal(pTotWorkExp));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotWorkExpObj", pTotWorkExpObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotWorkExpVer", pTotWorkExpVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotWorkExpRemark", pTotWorkExpRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region SavePDIncomeSourceRental

        public string SavePDIncomeSourceRental(string pLoanAppId, string pPDDoneBy, string pPDDate,
         string pRentalAmt, string pRentalAmtObj, string pRentalAmtVer, string pRentalAmtRemark,
         string pPropType, string pPropTypeObj, string pPropTypeVer, string pPropTypeRemark,
         string pNoOfRentRoom, string pNoOfRentRoomObj, string pNoOfRentRoomVer, string pNoOfRentRoomRemark,
         string pPropAge, string pPropAgeObj, string pPropAgeVer, string pPropAgeRemark,
         string pPropAddress, string pPropAddressObj, string pPropAddressVer, string pPropAddressRemark,
         string pTenantNOC, string pTenantNOCObj, string pTenantNOCVer, string pTenantNOCRemark,
         string pRentalDoc, string pRentalDocObj, string pRentalDocVer, string pRentalDocRemark,
         string pCreatedBy, string pCustID, string pCustType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourceRental";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pRentalAmt", Convert.ToDecimal(pRentalAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalAmtObj", pRentalAmtObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalAmtVer", pRentalAmtVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalAmtRemark", pRentalAmtRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropType", Convert.ToInt32(pPropType));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropTypeObj", pPropTypeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropTypeVer", pPropTypeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropTypeRemark", pPropTypeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfRentRoom", Convert.ToInt32(pNoOfRentRoom));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfRentRoomObj", pNoOfRentRoomObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfRentRoomVer", pNoOfRentRoomVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNoOfRentRoomRemark", pNoOfRentRoomRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPropAge", Convert.ToInt32(pPropAge));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAgeObj", pPropAgeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAgeVer", pPropAgeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAgeRemark", pPropAgeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAddress", pTenantNOC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAddressObj", pPropAddressObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAddressVer", pPropAddressVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPropAddressRemark", pPropAddressRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTenantNOC", pTenantNOC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTenantNOCObj", pTenantNOCObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTenantNOCVer", pTenantNOCVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTenantNOCRemark", pTenantNOCRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalDoc", pRentalDoc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalDocObj", pRentalDocObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalDocVer", pRentalDocVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRentalDocRemark", pRentalDocRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully.";
                else
                    return "Data Not Saved";
            }
            catch (Exception ex)
            {
                return "Falid:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDIncomeSourcePension
        public string SavePDIncomeSourcePension(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pPensionAmt, string pPensionAmtObj, string pPensionAmtVer, string pPensionAmtRemark,
        string pPenStDate, string pPenStDateObj, string pPenStDateVer, string pPenStDateRemark,
        string pPenOrder, string pPenOrderObj, string pPenOrderVer, string pPenOrderRemark,
        string pPenBankName, string pPenBankNameObj, string pPenBankNameVer, string pPenBankNameRemark,
        string pPenIncId, string pPenIncObj, string pPenIncVer, string pPenIncRemark,
        string pPenFromId, string pPenFromObj, string pPenFromVer, string pPenFromRemark,
        string pCreatedBy, string pCustID, string pCustType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourcePension";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pPensionAmt", Convert.ToDecimal(pPensionAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPensionAmtObj", pPensionAmtObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPensionAmtVer", pPensionAmtVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPensionAmtRemark", pPensionAmtRemark);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPenStDate", setDate(pPenStDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenStDateObj", pPenStDateObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenStDateVer", pPenStDateVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenStDateRemark", pPenStDateRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenOrder", pPenOrder);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenOrderObj", pPenOrderObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenOrderVer", pPenOrderVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenOrderRemark", pPenOrderRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenBankName", pPenBankName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenBankNameObj", pPenBankNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenBankNameVer", pPenBankNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenBankNameRemark", pPenBankNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPenIncId", Convert.ToInt32(pPenIncId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenIncObj", pPenIncObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenIncVer", pPenIncVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenIncRemark", pPenIncRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPenFromId", Convert.ToInt32(pPenFromId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenFromObj", pPenFromObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenFromVer", pPenFromVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPenFromRemark", pPenFromRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record saved successfully";
                else
                    return "Data not saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDIncomeSourceWages

        public string SavePDIncomeSourceWages(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pWorkType, string pWorkTypeObj, string pWorkTypeVer, string pWorkTypeRemark,
        string pWorkDoingFrom, string pWorkDoingFromObj, string pWorkDoingFromVer, string pWorkDoingFromRemark,
        string pWagesEmpName, string pWagesEmpNameObj, string pWagesEmpNameVer, string pWagesEmpNameRemark,
        string pEmpContactNo, string pEmpContactNoObj, string pEmpContactNoVer, string pEmpContactNoRemark,
        string pCreatedBy, string pCustID, string pCustType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourceWages";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkType", pWorkType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkTypeObj", pWorkTypeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkTypeVer", pWorkTypeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkTypeRemark", pWorkTypeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkDoingFrom", pWorkDoingFrom);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkDoingFromObj", pWorkDoingFromObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkDoingFromVer", pWorkDoingFromVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWorkDoingFromRemark", pWorkDoingFromRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWagesEmpName", pWagesEmpName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWagesEmpNameObj", pWagesEmpNameObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWagesEmpNameVer", pWagesEmpNameVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pWagesEmpNameRemark", pWagesEmpNameRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEmpContactNo", pEmpContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpContactNoObj", pEmpContactNoObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpContactNoVer", pEmpContactNoVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEmpContactNoRemark", pEmpContactNoRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully.";
                else
                    return "Data not Saved.";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDIncomeSourceAggriculture

        public string SavePDIncomeSourceAggriculture(string pLoanAppId, string pPDDate, string pYearlyIncome,
            string pIncomeFrequency, string pAreaOfLand, string pSelfFarmingYN, string pLeasedYN, string pTypeofcrops, string pCustType, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDIncomeSourceAggriculture";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pYearlyIncome", Convert.ToDecimal(pYearlyIncome));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIncomeFrequency", pIncomeFrequency);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAreaOfLand", Convert.ToInt32(pAreaOfLand));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSelfFarmingYN", pSelfFarmingYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLeasedYN", pLeasedYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTypeofcrops", pTypeofcrops);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDLoanRequireDetail

        public string SavePDLoanRequireDetail(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pLnPurpose, string pLnPurposeObj, string pLnPurposeVer, string pLnPurposeRemark,
        string pExpLnAmt, string pExpLnAmtObj, string pExpLnAmtVer, string pExpLnAmtRemark,
        string pExpLnTenore, string pExpLnTenoreObj, string pExpLnTenoreVer, string pExpLnTenoreRemark,
        string pEMICap, string pEMICapObj, string pEMICapVer, string pEMICapRemark,
        string pTotLnOutstanding, string pTotLnOutstandingObj, string pTotLnOutstandingVer, string pTotLnOutstandingRemark,
        string pCreatedBy, string pCustID, string pCustType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDLoanRequireDetail";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLnPurpose", Convert.ToInt32(pLnPurpose));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLnPurposeObj", pLnPurposeObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLnPurposeVer", pLnPurposeVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLnPurposeRemark", pLnPurposeRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pExpLnAmt", Convert.ToDecimal(pExpLnAmt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnAmtObj", pExpLnAmtObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnAmtVer", pExpLnAmtVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnAmtRemark", pExpLnAmtRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pExpLnTenore", Convert.ToInt32(pExpLnTenore));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnTenoreObj", pExpLnTenoreObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnTenoreVer", pExpLnTenoreVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pExpLnTenoreRemark", pExpLnTenoreRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pEMICap", Convert.ToDecimal(pEMICap));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEMICapObj", pEMICapObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEMICapVer", pEMICapVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pEMICapRemark", pEMICapRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pTotLnOutstanding", Convert.ToDecimal(pTotLnOutstanding));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotLnOutstandingObj", pTotLnOutstandingObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotLnOutstandingVer", pTotLnOutstandingVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTotLnOutstandingRemark", pTotLnOutstandingRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully.";
                else
                    return "Data not Saved.";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDInvestmentSaving

        public string SavePDInvestmentSaving(string pLoanAppId, string pPDDoneBy, string pPDDate,
        string pGold, string pGoldObj, string pGoldVer, string pGoldRemark,
        string pBondPaper, string pBondPaperObj, string pBondPaperVer, string pBondPaperRemark,
        string pDailySaving, string pDailySavingObj, string pDailySavingVer, string pDailySavingRemark,
        string pInsuPolicy, string pInsuPolicyObj, string pInsuPolicyVer, string pInsuPolicyRemark,
        string pCreatedBy, string pCustID, string pCustType, string pFixedDeposit, string pFixedDepositObj,
        string pFixedDepositVer, string pFixedDepositRemark, string pAggriLand, string pAggriLandObj,
        string pAggriLandVer, string pAggriLandRemark)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDInvestmentSaving";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pPDDoneBy", pPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGold", pGold);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGoldObj", pGoldObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGoldVer", pGoldVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGoldRemark", pGoldRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBondPaper", pBondPaper);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBondPaperObj", pBondPaperObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBondPaperVer", pBondPaperVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBondPaperRemark", pBondPaperRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDailySaving", pDailySaving);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDailySavingObj", pDailySavingObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDailySavingVer", pDailySavingVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pDailySavingRemark", pDailySavingRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pInsuPolicy", pInsuPolicy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pInsuPolicyObj", pInsuPolicyObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pInsuPolicyVer", pInsuPolicyVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pInsuPolicyRemark", pInsuPolicyRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFixedDeposit", pFixedDeposit);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFixedDepositObj", pFixedDepositObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFixedDepositVer", pFixedDepositVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFixedDepositRemark", pFixedDepositRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAggriLand", pAggriLand);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAggriLandObj", pAggriLandObj);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAggriLandVer", pAggriLandVer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pAggriLandRemark", pAggriLandRemark);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record saved successfully.";
                else
                    return "Data Not saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDBankBehaviour

        public string SavePDBankBehaviour(string pLoanAppId, string pPDDate, string pAccNo, string pAccType, string pCurrentBal, string pMonth1Bal,
        string pMonth2Bal, string pMonth3Bal, string pMonth1Tran, string pMonth2Tran, string pMonth3Tran, string pMinBal3Months, string pChqueReturns3Months,
        string pCustType, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDBankBehaviour";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccNo", pAccNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccType", pAccType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCurrentBal", Convert.ToDecimal(pCurrentBal));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth1Bal", Convert.ToDecimal(pMonth1Bal));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth2Bal", Convert.ToDecimal(pMonth2Bal));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth3Bal", Convert.ToDecimal(pMonth3Bal));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth1Tran", Convert.ToDecimal(pMonth1Tran));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth2Tran", Convert.ToDecimal(pMonth2Tran));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMonth3Tran", Convert.ToDecimal(pMonth3Tran));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pMinBal3Months", Convert.ToDecimal(pMinBal3Months));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pChqueReturns3Months", pChqueReturns3Months);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Record Saved Successfully.";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region SavePDApplicantProfile

        public string SavePDApplicantProfile(string pLoanAppId, string pPDDate, string pCoOperative, string pAccuracy, string pBusiness, string pHousehold,
        string pSavings, string pInventroy, string pPhysicalFitness, string pFamilySupport, string pCustType, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDApplicantProfile";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoOperative", pCoOperative);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAccuracy", pAccuracy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiness", pBusiness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pHousehold", pHousehold);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSavings", pSavings);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInventroy", pInventroy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPhysicalFitness", pPhysicalFitness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFamilySupport", pFamilySupport);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Save record Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region SavePDCoApplicantProfile

        public string SavePDCoApplicantProfile(string pLoanAppId, string pPDDate, string pCoOperative, string pAccuracy, string pBusiness, string pHousehold,
         string pSavings, string pInventroy, string pPhysicalFitness, string pFamilySupport, string pCustType, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDCoApplicantProfile";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCoOperative", pCoOperative);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAccuracy", pAccuracy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pBusiness", pBusiness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pHousehold", pHousehold);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSavings", pSavings);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInventroy", pInventroy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPhysicalFitness", pPhysicalFitness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFamilySupport", pFamilySupport);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Save record Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }


        #endregion

        #region SavePDBusinessReference1

        public string SavePDBusinessReference1(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
        string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy, string pPlaceOfOffice, string pPaymentIssue, string pSupplier,
             string pLatitude, string pLongitude, string pAddress)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDBusinessReference1";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pName", pName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pContactNo", pContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRelation", pRelation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pResidence", pResidence);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOccupation", pOccupation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pKnowHimYears", Convert.ToInt32(pKnowHimYears));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPlaceOfOffice", pPlaceOfOffice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaymentIssue", pPaymentIssue);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSupplier", pSupplier);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLatitude", pLatitude);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLongitude", pLongitude);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLocationAddress", pAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Save record Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region SavePDBusinessReference2

        public string SavePDBusinessReference2(string pLoanAppId, string pPDDate, string pName, string pContactNo, string pRelation, string pResidence,
        string pOccupation, string pKnowHimYears, string pCustType, string pCreatedBy, string pPlaceOfOffice, string pPaymentIssue, string pSupplier)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDBusinessReference2";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pPDDate", setDate(pPDDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pName", pName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pContactNo", pContactNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pRelation", pRelation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pResidence", pResidence);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOccupation", pOccupation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pKnowHimYears", Convert.ToInt32(pKnowHimYears));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCustType", pCustType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPlaceOfOffice", pPlaceOfOffice);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPaymentIssue", pPaymentIssue);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pSupplier", pSupplier);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Save record Successfully";
                else
                    return "Data not Saved";
            }
            catch (Exception ex)
            {
                return "Faild:" + ex.ToString();
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        #endregion

        #region PDMasterDetails

        public PDMasterDetails PDMasterDetails()
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, dt13 = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetRelationPropertyOwnerTypeOfOwnerMaritalStatusNoOfChildren";
                ds = DBUtility.GetDataSet(oCmd);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                oCmd.Dispose();
            }

            dt = ds.Tables[0];
            dt1 = ds.Tables[1];
            dt2 = ds.Tables[2];
            dt3 = ds.Tables[3];
            dt4 = ds.Tables[4];
            dt5 = ds.Tables[5];
            dt6 = ds.Tables[6];
            dt7 = ds.Tables[7];
            dt8 = ds.Tables[8];
            dt9 = ds.Tables[9];
            dt10 = ds.Tables[10];
            dt11 = ds.Tables[11];
            dt12 = ds.Tables[12];
            dt13 = ds.Tables[13];

            var list1 = dt.AsEnumerable().ToList();
            var list2 = dt1.AsEnumerable().ToList();
            var list3 = dt2.AsEnumerable().ToList();

            var list4 = dt3.AsEnumerable().ToList();
            var list5 = dt4.AsEnumerable().ToList();
            var list6 = dt5.AsEnumerable().ToList();

            var list7 = dt6.AsEnumerable().ToList();
            var list8 = dt7.AsEnumerable().ToList();
            var list9 = dt8.AsEnumerable().ToList();

            var list10 = dt9.AsEnumerable().ToList();
            var list11 = dt10.AsEnumerable().ToList();
            var list12 = dt11.AsEnumerable().ToList();

            var list13 = dt12.AsEnumerable().ToList();
            var list14 = dt13.AsEnumerable().ToList();

            var GetRelationProperty = new PDMasterDetails()
            {

                RelationMst = list1.Select(dr => new RelationMst { Relation = dr["Relation"].ToString(), RelationId = dr["RelationId"].ToString() }).ToList(),
                MaritalStatusMst = list2.Select(dr => new MaritalStatusMst { MaritalName = dr["MaritalName"].ToString(), MaritalId = dr["MaritalId"].ToString() }).ToList(),
                PropOwnerShipMst = list3.Select(dr => new PropOwnerShipMst { PropOwnerShip = dr["PropOwnerShip"].ToString(), PropOwnerShipId = dr["PropOwnerShipId"].ToString() }).ToList(),
                BusinessTypeMst = list5.Select(dr => new BusinessTypeMst { BusinessType = dr["BusinessType"].ToString(), BusinessTypeId = dr["BusinessTypeId"].ToString() }).ToList(),

                SalCredModeMst = list6.Select(dr => new SalCredModeMst { SalCredMode = dr["SalCredMode"].ToString(), SalCredModeId = dr["SalCredModeId"].ToString() }).ToList(),
                SalTypeMst = list7.Select(dr => new SalTypeMst { SalType = dr["SalType"].ToString(), SalTypeId = dr["SalTypeId"].ToString() }).ToList(),
                PropertypeMst = list8.Select(dr => new PropertypeMst { PropertypeName = dr["PropertypeName"].ToString(), PropertyTypeID = dr["PropertyTypeID"].ToString() }).ToList(),
                PenIncomeMst = list9.Select(dr => new PenIncomeMst { PenIncome = dr["PenIncome"].ToString(), PenIncomeId = dr["PenIncomeId"].ToString() }).ToList(),

                PensionFromMst = list10.Select(dr => new PensionFromMst { PensionFrom = dr["PensionFrom"].ToString(), PensionFromId = dr["PensionFromId"].ToString() }).ToList(),
                WorkTypeMst = list11.Select(dr => new WorkTypeMst { WorkType = dr["WorkType"].ToString(), WorkTypeId = dr["WorkTypeId"].ToString() }).ToList(),
                PropNatureMst = list12.Select(dr => new PropNatureMst { PropNature = dr["PropNature"].ToString(), PropNatureId = dr["PropNatureId"].ToString() }).ToList(),
                PropJudMst = list13.Select(dr => new PropJudMst { PropJud = dr["PropJud"].ToString(), PropJudId = dr["PropJudId"].ToString() }).ToList(),

                PurposeMst = list14.Select(dr => new PurposeMst { PurposeName = dr["PurposeName"].ToString(), PurposeId = dr["PurposeId"].ToString() }).ToList(),
                ChildMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="1",
                        Key="1"
                    },
                    new ChildMst{
                        Value="2",
                        Key="2"
                    },
                    new ChildMst{
                        Value="3",
                        Key="3"
                    },
                    new ChildMst{
                        Value="4",
                        Key="4"
                    },
                    new ChildMst{
                        Value="5",
                        Key="5"
                    },
                    new ChildMst{
                        Value="6",
                        Key="6"
                    },
                    new ChildMst{
                        Value="7",
                        Key="7"
                    }
                },
                EraningMemberMst = new List<EraningMemberMst>()
                {
                    new EraningMemberMst{
                        Key="Y",
                        Value="Yes"
                    },
                    new EraningMemberMst{
                        Key="N",
                        Value="No"
                    }
                },
                SchoolTypMst = new List<SchoolTypMst>(){
                    new SchoolTypMst{
                        Key="G",
                        Value="Govt School"
                    },
                    new SchoolTypMst{
                        Key="E",
                        Value="English  medium School"
                    }
                },
                OccupationMst = new List<OccupationMst>()
                {
                    new OccupationMst{
                         Key="B",
                        Value="Business"
                    },
                    new OccupationMst{
                        Key="S",
                        Value="Salaried"
                    },
                    new OccupationMst{
                        Key="N",
                        Value="Not Working"
                    }
                },
                BusinessSatbilityMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="1",
                        Key="1"
                    },
                    new ChildMst{
                        Value="2",
                        Key="2"
                    },
                    new ChildMst{
                        Value="3",
                        Key="3"
                    },
                    new ChildMst{
                        Value="4",
                        Key="4"
                    },
                    new ChildMst{
                        Value="5",
                        Key="5"
                    },
                    new ChildMst{
                        Value="6",
                        Key="6"
                    },
                    new ChildMst{
                        Value="7",
                        Key="7"
                    },
                     new ChildMst{
                        Value="8",
                        Key="8"
                    },
                    new ChildMst{
                        Value="9",
                        Key="9"
                    },
                    new ChildMst{
                        Value="10",
                        Key="10"
                    },
                    new ChildMst{
                        Value="10+",
                        Key="10+"
                    }
                },
                AppRelationMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="Friend",
                        Key="F"
                    },
                    new ChildMst{
                        Value="Relative",
                        Key="R"
                    },
                    new ChildMst{
                        Value="Naighbour",
                        Key="N"
                    }
                },
                BehaviourMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="Good",
                        Key="G"
                    },
                    new ChildMst{
                        Value="Average",
                        Key="A"
                    },
                    new ChildMst{
                        Value="Poor",
                        Key="P"
                    }
                },
                AreaOfLandMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value=" < 1 acre",
                        Key="1"
                    },
                    new ChildMst{
                        Value="1 to 5",
                        Key="2"
                    },
                    new ChildMst{
                        Value="5 to 15",
                        Key="3"
                    }
                },
                TypeOfCropsMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="Seasonal",
                        Key="S"
                    },
                    new ChildMst{
                        Value="Non seasonal",
                        Key="N"
                    },
                    new ChildMst{
                        Value="Perennial",
                        Key="P"
                    }
                },
                IncomeFrequencyMst = new List<ChildMst>()
                {
                    new ChildMst{
                        Value="Yearly",
                        Key="Y"
                    },
                    new ChildMst{
                        Value="Half yearly",
                        Key="H"
                    },
                    new ChildMst{
                        Value="Others",
                        Key="O"
                    }
                }
            };

            return GetRelationProperty;
        }
        #endregion

        #region GetDocType
        public List<DocTypeMst> GetDocType()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<DocTypeMst> row = new List<DocTypeMst>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDocType";
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new DocTypeMst(rs["DocTypeId"].ToString(), rs["DocTypeName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new DocTypeMst("No Data Available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new DocTypeMst("No Data Available", ex.Message));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region CustomerListForDocUpload
        public List<CustomerList> CustomerListForDocUpload(string pCreatedBy)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<CustomerList> row = new List<CustomerList>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "CustomerListForDocUpload";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new CustomerList(rs["CustName"].ToString(), rs["LoanAppId"].ToString()));
                    }
                }
                else
                {
                    row.Add(new CustomerList("No Data Available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new CustomerList("No Data Available", ex.Message));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region SaveDocument
        public string SaveDocument(string pLoanAppId, string pDate, string pXmlData, string pMode, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveDocument";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", setDate(pDate));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pMode", pMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Failed";
                else
                    return "Success";
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

        #region PaynearBy OTP based Digital Doc

        public List<InitiateDigitalDocResponse> InitiateDigitalDoc(PostCredential vPostCredential, PostDigiDocOTPDataSet vPostDigiDocOTPDataSet)
        {
            CRepository oRepo = new CRepository();
            List<InitiateDigitalDocResponse> row = new List<InitiateDigitalDocResponse>();
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            String vURL = "", vToken = "";
            try
            {
                string DigiDocUrl = ConfigurationManager.AppSettings["DigiDocUrl"];

                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "Credential Fields are Blank..", "", ""));
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "User Id Not found..", "", ""));
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "Password Not Matched..", "", ""));
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "Partner ID Not Matched..", "", ""));
                }
                else if (vPostDigiDocOTPDataSet.LoanAppNo.Trim() == "")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "Loan App No is Invalid..", "", ""));
                }
                else if (vPostDigiDocOTPDataSet.MacID.Trim() == "")
                {
                    row.Add(new InitiateDigitalDocResponse("101", "MacID is Invalid..", "", ""));
                }
                else
                {
                    Random ran = new Random();
                    String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                    //String sc = "!@#$%^*~";
                    int length = 100;
                    String vRandomToken = "";
                    for (int i = 0; i < length; i++)
                    {
                        int a = ran.Next(b.Length); //string.Lenght gets the size of string
                        vRandomToken = vRandomToken + b.ElementAt(a);
                    }
                    //for (int j = 0; j < 2; j++)
                    //{
                    //    int sz = ran.Next(sc.Length);
                    //    vRandomToken = vRandomToken + sc.ElementAt(sz);
                    //}

                    vURL = DigiDocUrl.ToString();
                    vToken = vRandomToken;

                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "SaveInitiateDigitalDoc";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanAppNo", vPostDigiDocOTPDataSet.LoanAppNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8000, "@pMacID", vPostDigiDocOTPDataSet.MacID);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pURL", vURL);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pToken", vToken);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                    DBUtility.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                    {
                        row.Add(new InitiateDigitalDocResponse("000", "Successful", vURL, vToken));
                    }
                    else if (vErr == 2)
                    {
                        row.Add(new InitiateDigitalDocResponse("111", "Invalid Loan Application Number", "", ""));
                    }
                    else
                    {
                        row.Add(new InitiateDigitalDocResponse("222", "Contact to system admin", "", ""));
                    }
                }
            }
            catch (Exception ex)
            {
                row.Add(new InitiateDigitalDocResponse("404", "Internal exception: " + ex.Message, "", ""));
            }
            return row;
        }

        #endregion

        #region PaynearBY FT Integration Equifax
        public string EquifaxIntegration(PostCredential vPostCredential, PostEquifaxDataSet vPostEquifaxDataSet)
        {
            CRepository oRepo = new CRepository();
            string pEqXml = "", pStatusDesc = "", pStatusDescDeDup = "";
            Int32 pStatus = 0, pStatusDeDup = 0;
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

            PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
            PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    pStatusDesc = "Error..";
                    pEqXml = "Credential Fields are Blank..";
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    pStatusDesc = "Error..";
                    pEqXml = "User Id Not found..";
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    pStatusDesc = "Error..";
                    pEqXml = "Password Not Matched..";
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    pStatusDesc = "Error..";
                    pEqXml = "Partner ID Not Matched..";
                }
                else
                {
                    /*  DeDup Checking */
                    pStatusDeDup = oRepo.DedupChaeck_Integration(vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName
                     , vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType, vPostEquifaxDataSet.AddValue, ref pStatusDescDeDup);
                    /*   End of Checking */
                    if (pStatusDeDup == 0)
                    {
                        ////*************************** For Live ***************************************************                      
                        WebServiceSoapClient eq = new WebServiceSoapClient();
                        ////************************************************GenderId 1 For MALE else Female****************************************
                        if (vPostCredential.UserID.Trim().ToUpper() == "CENTRUMSME") /*    LIVE - Production */
                        {
                            if (vPostEquifaxDataSet.GenderId == "1")
                            {
                                //--------------------------------------------------LIVE--------------------------------------------------
                                pEqXml = eq.Equifax(vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName, vPostEquifaxDataSet.DOB
                                    , vPostEquifaxDataSet.AddressType, vPostEquifaxDataSet.AddressLine1, vPostEquifaxDataSet.StateName, vPostEquifaxDataSet.PIN,
                                     vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType,
                                      vPostEquifaxDataSet.AddValue, vPostEquifaxDataSet.CoAppRel, vPostEquifaxDataSet.CoAppName,
                                       "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                                //"5750", PCSUserName, PCSPassword, "027FP27137", "9GH", " ", "PCS", "ERS", "3.1", "PRO");
                                //--------------------------------------------------UAT----------------------------------------------------
                            }
                            else
                            {
                                pEqXml = eq.Equifax(vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName, vPostEquifaxDataSet.DOB
                                    , vPostEquifaxDataSet.AddressType, vPostEquifaxDataSet.AddressLine1, vPostEquifaxDataSet.StateName, vPostEquifaxDataSet.PIN,
                                     vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType,
                                      vPostEquifaxDataSet.AddValue, vPostEquifaxDataSet.CoAppRel, vPostEquifaxDataSet.CoAppName,
                                       "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                                //--------------------------------------------------UAT----------------------------------------------------
                            }
                        }

                        else if (vPostCredential.UserID.Trim().ToUpper() == "CENTRUMSMEUAT")  /*   UAT */
                        {
                            if (vPostEquifaxDataSet.GenderId == "1")
                            {
                                ////--------------------------------------------------UAT----------------------------------------------------
                                pEqXml = eq.Equifax(vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName, vPostEquifaxDataSet.DOB
                                    , vPostEquifaxDataSet.AddressType, vPostEquifaxDataSet.AddressLine1, vPostEquifaxDataSet.StateName, vPostEquifaxDataSet.PIN,
                                     vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType,
                                      vPostEquifaxDataSet.AddValue, vPostEquifaxDataSet.CoAppRel, vPostEquifaxDataSet.CoAppName,
                                       "21", CCRUserName, CCRPassword, "028FZ00016", "FR7", "123456", "CCR", "ERS", "3.1", "UAT");
                                //"21", PCSUserName, PCSPassword, "999AA00007", "54J", " ", "PCS", "ERS", "3.1", "UAT");
                            }
                            else
                            {
                                //--------------------------------------------------UAT----------------------------------------------------
                                pEqXml = eq.Equifax(vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName, vPostEquifaxDataSet.DOB
                                    , vPostEquifaxDataSet.AddressType, vPostEquifaxDataSet.AddressLine1, vPostEquifaxDataSet.StateName, vPostEquifaxDataSet.PIN,
                                     vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType,
                                      vPostEquifaxDataSet.AddValue, vPostEquifaxDataSet.CoAppRel, vPostEquifaxDataSet.CoAppName,
                                       "21", CCRUserName, CCRPassword, "028FZ00016", "FR7", "123456", "CCR", "ERS", "3.1", "UAT");
                            }
                        }
                        //*************************************************************************
                        pStatus = oRepo.SaveEquifaxData_IntegrationLog(pEqXml, vPostEquifaxDataSet.FirstName, vPostEquifaxDataSet.MiddleName, vPostEquifaxDataSet.LastName
                            , vPostEquifaxDataSet.MobileNo, vPostEquifaxDataSet.IDType, vPostEquifaxDataSet.IDValue, vPostEquifaxDataSet.AddType, vPostEquifaxDataSet.AddValue, ref pStatusDesc);

                    }
                    else
                    {
                        pStatusDesc = "Error..";
                        pEqXml = "Dedup Failed...Reason:" + pStatusDescDeDup;
                    }
                }
                return pStatusDesc + "::" + pEqXml;

            }
            catch (Exception ex)
            {
                return "Error.." + "Equifax Verification Failed::" + ex.ToString();
            }
        }
        #endregion

        #region PaynearBY SaveDataFrom

        public string DataFromOld(RequestHeader requestHeader, RequestBody requestbody)
        {
            string pStatusDesc = "Failed:", pErrDesc = "";
            string vLeadId = "", vMemberId = "", vDate = "", vLoanAppNo = "", vMaritalStatus = "M",
                vBankAccountType = "1", vBrCode = "", vDOB = "", vPartnerID = "", vNomDOB = "", vDOF = "";
            string ACVouMst = null;
            string ACVouDtl = null;
            string vFinYr = "", pShortYear = "", vFinYrNo = "";
            int vErr2;
            decimal vNetDisbAmt = 0, vTotalCharge = 0, vProcPer = 0, vProcFee = 0, vProcFeeCGST = 0, vProcFeeSGST = 0, vProcFeeCGSTPer = 0, vProcFeeSGSTPer = 0;

            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2, dt3 = null;

            var json = new JavaScriptSerializer().Serialize(requestbody);
            PreliminaryDetails P = requestbody.PreliminaryDetails;
            ResidenceAddressDetails R = requestbody.ResidenceAddressDetails;
            KYCDetails K = requestbody.KYCDetails;
            BusinessDetails B = requestbody.BusinessDetails;
            BankDetails BNK = requestbody.BankDetails;
            CustomerLoanDetails CLD = requestbody.CustomerLoanDetails;
            CAMDetails CAM = requestbody.CAMDetails;
            OtherDetails O = requestbody.OtherDetails;
            InsuranceDetails I = requestbody.InsuranceDetails;
            BranchInformation BR = requestbody.BranchInformation;

            vMaritalStatus = P.MaritalStatus == "M" ? "1" : P.MaritalStatus == "U" ? "2" : P.MaritalStatus == "W" ? "3" : "4";
            vBankAccountType = BNK.BankAccountType.Trim() == "Saving Account" ? "1" : BNK.BankAccountType.Trim() == "Current Account" ? "2" : BNK.BankAccountType.Trim() == "Joint Saving Account" ? "3" : BNK.BankAccountType.Trim() == "Overdraft Account" ? "4" : "5";
            vBrCode = "0000";
            vDate = DateTime.Now.ToString("MM/dd/yyyy");
            vDOF = DateTime.Now.ToString("MM/dd/yyyy");
            //vDate = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST
            //vDOF = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST
            //vDOB = P.DateOfBirth;//OPEN FOR TEST
            string[] vDt = P.DateOfBirth.Split('-');
            vDOB = vDt[1] + "-" + vDt[0] + "-" + vDt[2];

            if (I.CustomerOptedForInsurance == "Y")
            {
                string[] vNBOB = I.NomineeDOB.Split('-');
                vNomDOB = vNBOB[1] + "-" + vNBOB[0] + "-" + vNBOB[2];
            }

            vPartnerID = requestHeader.PartnerID.Trim().ToUpper();

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else if (vBrCode == "")
            {
                pStatusDesc = "Failed:Branch Code Not found..";
            }
            else
            {
                try
                {
                    vErr2 = SavePayNearByDtl(P.MobileNumber, P.IsMobileNumberDifferentThenLinkedtoAadhaar, P.MobileNumberLinkedWithAadhaar, P.FullName, P.Gender, P.DateOfBirth,
                        P.EmailID, P.NumberOfDependants, K.PANCardNumber, K.POADocumentType, K.POANumber, K.AadhaarDocumentNumber, R.Address, R.Pincode, R.PincodeClassification,
                        R.CityDistrict, R.State, R.ResidenceOwnershipType, R.SinceWhenAreYouStayingHere, B.BusinessAddress, B.Pincode, B.PincodeClassification,
                        B.CityDistrict, B.State, B.BusinessPremiseOwnershipType, B.SinceWhenAreYouDoingBusinessHere, B.NoOfYearsInThisBusiness, B.StoreName,
                        B.StoreName, B.HowDoYouDoYourSaleCollectionsInBusiness, B.MonthlyTurnover, BNK.BankAccountType, BNK.BankName, BNK.AccountNumber,
                        BNK.IFSCCode, BNK.AccountOperationalScince, O.MonthlyFamilyExpense, O.AnyOtherIncome, O.PurposeOfLoan, O.ExistingLoan, O.TypeOfLoan,
                        O.ExistingCreditCard, CLD.LoanAmountRequired, CLD.LoanTenureRequired, CLD.ROIfixed, CAM.NBTScore, CAM.PNBIncomeForLast24months, CAM.PNBGTVForLast24months,
                        CAM.NBTRecommendedLoanAmount, CAM.NBTrecommendedTenure, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, CAM.ROI, CAM.EstimatedDisbursementDate,
                        CAM.EstimatedCollectionDate, CAM.EMI, "", CAM.ProcessingFees, CAM.CommentsAddedByCMLInCAM, CAM.LoanApprovedRejected, CAM.RejectedReason
                        , R.Area, B.GoogleMapAddress, B.LatLongOfTheAddress, I.CustomerOptedForInsurance, I.NomineeName, I.RelationshipWithNominee,
                        I.NomineeGender, I.NomineeDOB, vBrCode, CAM.ApprovedPaymentFrequency, vPartnerID, ref pErrDesc);
                    if (vErr2 == 0)
                    {
                        string vLead = "";
                        if (pErrDesc != "Success: Fresh member")
                        {
                            vLead = "Success:Record Saved Successfully#123";
                        }
                        else
                        {
                            //Save Lead
                            vLead = SaveLead("", vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "N", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID, "-1", "", "-1", "");
                        }
                        string[] arr1 = vLead.Split('#');
                        if (arr1[0] == "Success:Record Saved Successfully")
                        {
                            vLeadId = arr1[1];//Lead Id
                            string vLeadApproval = "";
                            if (pErrDesc != "Success: Fresh member")
                            {
                                string[] arr10 = pErrDesc.Split('#');
                                vLeadApproval = "Success:" + arr10[1];
                                vDOF = arr10[2];
                            }
                            else
                            {
                                //Approve Lead
                                vLeadApproval = ApproveLead(vLeadId, vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "Y", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID);
                            }
                            string[] arr2 = vLeadApproval.Split(':');
                            if (arr2[0] == "Success")
                            {
                                vMemberId = arr2[1];//Member Id
                                string vAge = Convert.ToString(GetAge(setDate(P.DateOfBirth)));
                                //Save Member
                                string vSaveCompany = Mob_SaveCompany(vMemberId, vMemberId, "-1", P.FullName, vDOF, vDOB, "-1", "", "", P.EmailID, K.PANCardNumber, "6", K.AadhaarDocumentNumber, "", "", "-1", "-1", R.Address, "", R.State, R.CityDistrict, R.Pincode, P.MobileNumber
                                    , "", "", "N", "", "", "", "", "", "", "", "", vBrCode, "1", P.FullName, P.MobileNumber, "", "1", K.PANCardNumber, "-1", "-1", B.BusinessAddress, "0", P.Gender == "M" ? "1" : P.Gender == "F" ? "2" : "3", vAge, "-1", "", "-1", "-1", "-1", R.SinceWhenAreYouStayingHere == "" ? "0" : R.SinceWhenAreYouStayingHere,
                                    "", P.FullName, BNK.AccountNumber, BNK.BankName, BNK.IFSCCode, BNK.AccountOperationalScince == "" ? "0" : BNK.AccountOperationalScince, vBankAccountType, "", "", "", "0", "", "0", "0", "", "", "", B.BusinessAddress, "",
                                    "", B.CityDistrict, B.Pincode, B.State, "", "", "", vMaritalStatus, R.ResidenceOwnershipType, "", B.StoreType, B.NoOfYearsInThisBusiness == "" ? "0" : B.NoOfYearsInThisBusiness, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", vPartnerID);
                                string[] arr3 = vSaveCompany.Split(':');
                                //pStatusDesc = vSaveCompany; 
                                if (arr3[0] == "Record Saved Successfully")
                                {
                                    string vErrMsg = "";
                                    if (I.CustomerOptedForInsurance == "Y")
                                    {
                                        //Save Co Applicant
                                        string vSaveCoApplicant = Mob_SaveCoApplicant("", vMemberId, vDate, I.NomineeName, vNomDOB, "0", I.NomineeGender.Trim() == "Male" ? "1" : I.NomineeGender.Trim() == "Female" ? "2" : "3", "-1", "-1", "-1", "1", "-1", "", "", "", "", "", "", "", "", "", "", "", "-1", "", "-1", "", "", "", "", "", "", "", vBrCode, "0", "0", "1", "", "", "", "", ""
                                            , "-1", "", "-1", "-1", "", "", GetRelationIdByRelation(I.RelationshipWithNominee), "-1", "", "", "N", "", "", "", "", "0", "-1", "", "", "0", "", "0", "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                                        string[] arr5 = vSaveCoApplicant.Split(':');
                                        vErrMsg = arr5[0];
                                    }
                                    else
                                    {
                                        vErrMsg = "Record Saved Successfully";
                                    }
                                    if (vErrMsg == "Record Saved Successfully")
                                    {
                                        string vPurposeID = GetPurposeIdByPurposeName(O.PurposeOfLoan);
                                        string vLoanTypeId = vPartnerID == "PNB" ? "3" : "4";
                                        //Save Loan Application
                                        string vLoanApplication = SaveLoanApplication(vMemberId, vDate, vPurposeID, vLoanTypeId, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, "N", vBrCode, "1", "I", "0", "-1", "", "Y", vDate, "", vPartnerID);
                                        string[] arr4 = vLoanApplication.Split('#');
                                        if (arr4[0] == "Success:Record Saved Successfully")
                                        {
                                            vLoanAppNo = arr4[1];//Loan Application Id
                                            //Save PD
                                            string vPd = SavePDMst("0", vLoanAppNo, vMemberId, "-1", "", vDate, "", "0", "0", "0", "0", vBrCode, "1", "", "", vPartnerID);
                                            if (vPd == "Record Saved Successfully")
                                            {
                                                int vErr = SaveIIRRatio(vLoanAppNo, 0, 0, 0, 0, 0, Convert.ToDecimal(CAM.EMI), "IIR", 60, Convert.ToInt32(CAM.CMLApprovedTenure), 0, 1);
                                                if (vErr > 0)
                                                {
                                                    Int32 vErr1 = SavePDFinalApprove(vLoanAppNo, "A", DateTime.Now, "", 1, "", "", "", "", "", "", Convert.ToDecimal(CAM.CMLApprovedLoanAmount));
                                                    if (vErr1 > 0)
                                                    {
                                                        string vSanctionId = GetSanctionId(vLoanAppNo);
                                                        vTotalCharge = (Convert.ToDecimal(CAM.InsurancePremiumAmount == "" ? "0" : CAM.InsurancePremiumAmount) + Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees));
                                                        vNetDisbAmt = Convert.ToDecimal(CAM.CMLApprovedLoanAmount) - vTotalCharge;
                                                        if (vPartnerID == "FDF")
                                                        {
                                                            vProcPer = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) / Convert.ToDecimal(CAM.CMLApprovedLoanAmount)) * 100;
                                                            vProcFee = Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees);
                                                        }
                                                        else
                                                        {
                                                            vProcFee = (Convert.ToDecimal(CAM.CMLApprovedLoanAmount) * 3) / 100;
                                                            vProcFeeCGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                            vProcFeeSGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                            vProcFeeCGSTPer = 9;
                                                            vProcFeeSGSTPer = 9;
                                                            vProcPer = 3;
                                                        }
                                                        string vRepayType = CAM.ApprovedPaymentFrequency == null ? "M" : CAM.ApprovedPaymentFrequency == "" ? "M" : CAM.ApprovedPaymentFrequency;

                                                        Int32 vErr4 = SaveSanction(ref vSanctionId, vSanctionId, vLoanAppNo, vMemberId, DateTime.Now, Convert.ToDecimal(CAM.CMLApprovedLoanAmount), Convert.ToInt32(vLoanTypeId), 0,
                                                            Convert.ToDecimal(CAM.ROI), Convert.ToInt32(CAM.CMLApprovedTenure), Convert.ToInt32(CAM.CMLApprovedTenure), "R"
                                                            , vRepayType, Convert.ToDecimal(CAM.EMI), vProcFee, 0, 0, 0,
                                                            DateTime.Now, setDate(CAM.EstimatedCollectionDate), "N", vNetDisbAmt, "N", "P", DateTime.Now, "Admin", "", vBrCode, 0, 0, 0, 0, vProcPer, 0, 0,
                                                            0, 0, 0, vTotalCharge, 1, "Edit", 0, DateTime.Now, 0, 0, "", "", "", vProcFeeCGSTPer, vProcFeeCGST, vProcFeeSGSTPer, vProcFeeSGST, 0, 0, "", "",
                                                            0, 0, 0, Convert.ToDecimal(CAM.InsurancePremiumAmount), 0, 0, 0, 0, 0, 0, "", "", "", 0, "", 0, 0, 0, 0, I.CustomerOptedForInsurance == "Y" ? 2 : 0);
                                                        if (vErr4 == 0)
                                                        {
                                                            int vErr5 = SaveFinalSanction(vSanctionId, "S", "Admin", DateTime.Now, "", 0);
                                                            if (vErr5 == 0)
                                                            {
                                                                if (vPartnerID == "FDF")
                                                                {
                                                                    string vLoanNo = "", vErrDesc = "";
                                                                    dt2 = new DataTable();
                                                                    dt3 = new DataTable();
                                                                    dt2 = GetFinYearList(vBrCode);
                                                                    if (dt2.Rows.Count > 0)
                                                                    {
                                                                        vFinYrNo = getFinYrNo(dt2.Rows[0]["YrNo"].ToString());
                                                                        vFinYr = dt2.Rows[0]["FYear"].ToString();
                                                                        ACVouMst = "ACVouMst" + vFinYrNo;
                                                                        ACVouDtl = "ACVouDtl" + vFinYrNo;
                                                                        dt3 = GetFinYearAll(vFinYr);
                                                                        pShortYear = dt3.Rows[0]["ShortYear"].ToString();
                                                                    }
                                                                    int vErr6 = InsertLoanMstNew(ref vLoanNo, vSanctionId, vLoanAppNo, vMemberId, Convert.ToInt32(vLoanTypeId), 1, setDate(CAM.EstimatedDisbursementDate), Convert.ToDecimal(CAM.CMLApprovedLoanAmount), "R",
                                                                        CAM.ApprovedPaymentFrequency, Convert.ToDecimal(CAM.ROI), 0, Convert.ToInt32(CAM.CMLApprovedTenure), 12, Convert.ToDecimal(CAM.EMI),
                                                                        Convert.ToInt32(CAM.CMLApprovedTenure), setDate(CAM.EstimatedCollectionDate), "N", 1, 1, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToDecimal(CAM.CMLApprovedLoanAmount),
                                                                        "G0674", "B", "", DateTime.Now, vBrCode, ACVouMst, ACVouDtl, pShortYear, 1, "Being the Amt of Loan Disbursed for " + P.FullName, 0, 0, "", 0, 0, "", 0, 0, 0, 0, "",
                                                                        Convert.ToDecimal(CAM.CMLApprovedLoanAmount), "", "", Convert.ToDecimal(CAM.CMLApprovedLoanAmount), "", 0, vBrCode, 0, ref vErrDesc, 0, 0, Convert.ToDecimal(CAM.InsurancePremiumAmount), 0,
                                                                        0, 0, 0, 0, 0, 0, 0, 0, 0, "N", "", 0, 0, 0); //G0674--Fund Fina Disbursement Account
                                                                    if (vErr6 == 0)
                                                                    {
                                                                        pStatusDesc = "Success";
                                                                    }
                                                                    else
                                                                    {
                                                                        pStatusDesc = "Failed:Problem in Disbursement";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    pStatusDesc = "Success";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                pStatusDesc = "Failed:Problem in HO Sanction Data Save";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            pStatusDesc = "Failed:Problem in Pre Sanction Data Save";
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                pStatusDesc = "Failed:Problem in PD Data Save";
                                            }
                                        }
                                        else
                                        {
                                            pStatusDesc = "Failed:Problem in Loan Application Data Save";
                                        }
                                    }
                                    else
                                    {
                                        pStatusDesc = "Failed:Problem in Nominee Data Update";
                                    }
                                }
                                else
                                {
                                    pStatusDesc = "Failed:Problem in Personal Data Update";
                                }
                            }
                            else
                            {
                                pStatusDesc = vLeadApproval + "Problem in Lead Approval Save";
                            }
                        }
                        else
                        {
                            pStatusDesc = vLead + " ..Problem in Lead and Personal Data Save";
                        }
                    }
                    else if (vErr2 == 2)
                    {
                        pStatusDesc = "Failed:Duplicate ID Found ... Please check PAN or Aadhaar..";
                    }
                    else if (vErr2 == 3)
                    {
                        pStatusDesc = pErrDesc;
                    }
                    else if (vErr2 == 4)
                    {
                        pStatusDesc = pErrDesc;
                    }
                    else if (vErr2 == 5)
                    {
                        pStatusDesc = "Failed:ID Found Blank ... Please check PAN or Aadhaar..";
                    }
                    else if (vErr2 == 6)
                    {
                        pStatusDesc = "Failed:Day End Done so you can not Push data for today..";
                    }
                    else if (vErr2 == 7)
                    {
                        pStatusDesc = "Failed:Day Begin not Done so you can not Push data for today..";
                    }
                    else
                    {
                        pStatusDesc = "Failed:Problem in Data Set Sent";
                    }
                }
                catch (Exception ex)
                {
                    pStatusDesc = pStatusDesc + " " + ex.Message;
                }
            }
            if (pStatusDesc == "Success")
            {
                return vLoanAppNo;
            }
            else
            {
                return pStatusDesc;
            }
        }

        #endregion

        #region PaynearBY SaveDataFromNew

        public string DataFrom(RequestHeader requestHeader, RequestBody requestbody)
        {
            string pStatusDesc = "Failed:", pErrDesc = "", pErrDesc1 = "";
            string vLeadId = "", vMemberId = "", vMemberNo = "", vDate = "", vLoanAppNo = "", vMaritalStatus = "M",
                vBankAccountType = "1", vBrCode = "", vDOB = "", vPartnerID = "", vNomDOB = "", vDOF = "";
            int vErr2 = 0, vErr3 = 0;
            decimal vNetDisbAmt = 0, vTotalCharge = 0, vProcPer = 0, vProcFee = 0, vProcFeeCGST = 0, vProcFeeSGST = 0, vProcFeeCGSTPer = 0, vProcFeeSGSTPer = 0;

            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            var json = new JavaScriptSerializer().Serialize(requestbody);
            PreliminaryDetails P = requestbody.PreliminaryDetails;
            ResidenceAddressDetails R = requestbody.ResidenceAddressDetails;
            KYCDetails K = requestbody.KYCDetails;
            BusinessDetails B = requestbody.BusinessDetails;
            BankDetails BNK = requestbody.BankDetails;
            CustomerLoanDetails CLD = requestbody.CustomerLoanDetails;
            CAMDetails CAM = requestbody.CAMDetails;
            OtherDetails O = requestbody.OtherDetails;
            InsuranceDetails I = requestbody.InsuranceDetails;
            BranchInformation BR = requestbody.BranchInformation;

            vMaritalStatus = P.MaritalStatus == null ? "0" : P.MaritalStatus == "M" ? "1" : P.MaritalStatus == "U" ? "2" : P.MaritalStatus == "W" ? "3" : "4";
            vBankAccountType = BNK.BankAccountType == null ? "0" : BNK.BankAccountType.Trim() == "Saving Account" ? "1" : BNK.BankAccountType.Trim() == "Current Account" ? "2" : BNK.BankAccountType.Trim() == "Joint Saving Account" ? "3" : BNK.BankAccountType.Trim() == "Overdraft Account" ? "4" : "5";

            //vBrCode = BR.BranchCode == "" ? "0000" : BR.BranchCode;
            vBrCode = "0000";

            vDate = DateTime.Now.ToString("MM/dd/yyyy");
            vDOF = DateTime.Now.ToString("MM/dd/yyyy");
            string[] vDt = P.DateOfBirth.Split('-');
            vDOB = vDt[1] + "-" + vDt[0] + "-" + vDt[2];

            //vDate = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST
            //vDOF = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST            
            //vDOB = P.DateOfBirth;//OPEN FOR TEST

            if (I.CustomerOptedForInsurance == "Y")
            {
                string[] vNBOB = I.NomineeDOB.Split('-');
                vNomDOB = vNBOB[1] + "-" + vNBOB[0] + "-" + vNBOB[2];
            }

            vPartnerID = requestHeader.PartnerID.Trim().ToUpper();

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else if (vBrCode == "")
            {
                pStatusDesc = "Failed:Branch Code Not found..";
            }
            else
            {
                try
                {
                    //0-->Fresh Member
                    //56-->Existing Member both with close loan (JLG) and(MEL)
                    //57-->Existing member MEL with close loan
                    //55-->Existing Member with close loan (JLG)

                    //---------------DDup Check-------------------
                    vErr3 = chkDdupMEL(K.PANCardNumber, K.AadhaarDocumentNumber, K.POANumber, vBrCode, ref pErrDesc1);
                    if (vErr3 == 0 || vErr3 == 55 || vErr3 == 56 || vErr3 == 57)
                    {
                        //------------------Save Pay Near by-------------------------------
                        vErr2 = SavePayNearByDtl(P.MobileNumber, P.IsMobileNumberDifferentThenLinkedtoAadhaar, P.MobileNumberLinkedWithAadhaar, P.FullName, P.Gender, P.DateOfBirth,
                            P.EmailID, P.NumberOfDependants, K.PANCardNumber, K.POADocumentType, K.POANumber, K.AadhaarDocumentNumber, R.Address, R.Pincode, R.PincodeClassification,
                            R.CityDistrict, R.State, R.ResidenceOwnershipType, R.SinceWhenAreYouStayingHere, B.BusinessAddress, B.Pincode, B.PincodeClassification,
                            B.CityDistrict, B.State, B.BusinessPremiseOwnershipType, B.SinceWhenAreYouDoingBusinessHere, B.NoOfYearsInThisBusiness, B.StoreName,
                            B.StoreName, B.HowDoYouDoYourSaleCollectionsInBusiness, B.MonthlyTurnover, BNK.BankAccountType, BNK.BankName, BNK.AccountNumber,
                            BNK.IFSCCode, BNK.AccountOperationalScince, O.MonthlyFamilyExpense, O.AnyOtherIncome, O.PurposeOfLoan, O.ExistingLoan, O.TypeOfLoan,
                            O.ExistingCreditCard, CLD.LoanAmountRequired, CLD.LoanTenureRequired, CLD.ROIfixed, CAM.NBTScore, CAM.PNBIncomeForLast24months, CAM.PNBGTVForLast24months,
                            CAM.NBTRecommendedLoanAmount, CAM.NBTrecommendedTenure, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, CAM.ROI, CAM.EstimatedDisbursementDate,
                            CAM.EstimatedCollectionDate, CAM.EMI, "", CAM.ProcessingFees, CAM.CommentsAddedByCMLInCAM, CAM.LoanApprovedRejected, CAM.RejectedReason
                            , R.Area, B.GoogleMapAddress, B.LatLongOfTheAddress, I.CustomerOptedForInsurance, I.NomineeName, I.RelationshipWithNominee,
                            I.NomineeGender, I.NomineeDOB, vBrCode, CAM.ApprovedPaymentFrequency, vPartnerID, ref pErrDesc);
                        if (vErr2 == 0)
                        {
                            string vLead = "";
                            if (vErr3 == 56 || vErr3 == 57)
                            {
                                vLead = "Success:Record Saved Successfully#123";
                            }
                            else
                            {
                                vLead = SaveLead("", vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "N", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID, "-1", "", "-1", "");
                            }
                            string[] arr1 = vLead.Split('#');
                            if (arr1[0] == "Success:Record Saved Successfully")
                            {
                                vLeadId = arr1[1];//Lead Id
                                string vLeadApproval = "";
                                string[] arr10 = null;

                                if (vErr3 == 56 || vErr3 == 57)
                                {
                                    arr10 = pErrDesc1.Split('#');
                                    vLeadApproval = "Success:" + arr10[1];
                                    vDOF = arr10[2];
                                    if (vErr3 == 56)
                                    {
                                        vMemberNo = arr10[3];
                                    }
                                }
                                else
                                {
                                    if (vErr3 == 55)
                                    {
                                        arr10 = pErrDesc1.Split('#');
                                        vMemberNo = arr10[1];
                                    }
                                    //Approve Lead
                                    vLeadApproval = ApproveLead(vLeadId, vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "Y", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID);
                                }
                                string[] arr2 = vLeadApproval.Split(':');
                                if (arr2[0] == "Success")
                                {
                                    vMemberId = arr2[1];//Member Id
                                    if (vErr3 != 55 || vErr3 != 56)
                                    {
                                        vMemberNo = arr2[1];//Member No
                                    }
                                    string vAge = Convert.ToString(GetAge(setDate(P.DateOfBirth)));
                                    //Save Member
                                    string vSaveCompany = Mob_SaveCompany(vMemberId, vMemberNo, "-1", P.FullName, vDOF, vDOB, "-1", "", "", P.EmailID, K.PANCardNumber, K.AadhaarDocumentNumber.Length < 4 ? "-1" : "6", K.AadhaarDocumentNumber, "", "", "-1", "-1", R.Address, "", R.State, R.CityDistrict, R.Pincode, P.MobileNumber
                                        , "", "", "N", "", "", "", "", "", "", "", "", vBrCode, "1", P.FullName, P.MobileNumber, "", "1", K.PANCardNumber, "-1", "-1", B.BusinessAddress, "0", P.Gender == "M" ? "1" : P.Gender == "F" ? "2" : "3", vAge, "-1", "", "-1", "-1", "-1", R.SinceWhenAreYouStayingHere == "" ? "0" : R.SinceWhenAreYouStayingHere,
                                        "", P.FullName, BNK.AccountNumber, BNK.BankName, BNK.IFSCCode, BNK.AccountOperationalScince == "" ? "0" : BNK.AccountOperationalScince, vBankAccountType, "", "", "", "0", "", "0", "0", "", "", "", B.BusinessAddress, "",
                                        "", B.CityDistrict, B.Pincode, B.State, "", "", "", vMaritalStatus, R.ResidenceOwnershipType, "", B.StoreType, B.NoOfYearsInThisBusiness == "" ? "0" : B.NoOfYearsInThisBusiness, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", vPartnerID);
                                    string[] arr3 = vSaveCompany.Split(':');
                                    //pStatusDesc = vSaveCompany; 
                                    if (arr3[0] == "Record Saved Successfully")
                                    {
                                        string vErrMsg = "";
                                        if (I.CustomerOptedForInsurance == "Y")
                                        {
                                            //Save Co Applicant
                                            string vSaveCoApplicant = Mob_SaveCoApplicant("", vMemberId, vDate, I.NomineeName, vNomDOB, "0", I.NomineeGender.Trim() == "Male" ? "1" : I.NomineeGender.Trim() == "Female" ? "2" : "3", "-1", "-1", "-1", "1", "-1", "", "", "", "", "", "", "", "", "", "", "",
                                                "-1", "", "-1", "", "", "", "", "", "", "", vBrCode, "0", "0", "1", "", "", "", "", ""
                                                , "-1", "", "-1", "-1", "", "", GetRelationIdByRelation(I.RelationshipWithNominee), "-1", "", "", "N", "", "", "", "", "0", "-1", "", "", "0", "", "0", "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                                            string[] arr5 = vSaveCoApplicant.Split(':');
                                            vErrMsg = arr5[0];
                                        }
                                        else
                                        {
                                            vErrMsg = "Record Saved Successfully";
                                        }
                                        if (vErrMsg == "Record Saved Successfully")
                                        {
                                            string vPurposeID = GetPurposeIdByPurposeName(O.PurposeOfLoan);
                                            string vLoanTypeId = vPartnerID == "PNB" ? "3" : "4";
                                            //Save Loan Application
                                            string vLoanApplication = SaveLoanApplication(vMemberId, vDate, vPurposeID, vLoanTypeId, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, "N", vBrCode, "1", "I", "0", "-1", "", "Y", vDate, "", vPartnerID);
                                            string[] arr4 = vLoanApplication.Split('#');
                                            if (arr4[0] == "Success:Record Saved Successfully")
                                            {
                                                vLoanAppNo = arr4[1];//Loan Application Id
                                                //Save PD
                                                string vPd = SavePDMst("0", vLoanAppNo, vMemberId, "-1", "", vDate, "", "0", "0", "0", "0", vBrCode, "1", "", "", vPartnerID);
                                                if (vPd == "Record Saved Successfully")
                                                {
                                                    int vErr = SaveIIRRatio(vLoanAppNo, 0, 0, 0, 0, 0, Convert.ToDecimal(CAM.EMI), "IIR", 60, Convert.ToInt32(CAM.CMLApprovedTenure), 0, 1);
                                                    if (vErr > 0)
                                                    {
                                                        Int32 vErr1 = SavePDFinalApprove(vLoanAppNo, "A", DateTime.Now, "", 1, "", "", "", "", "", "", Convert.ToDecimal(CAM.CMLApprovedLoanAmount));
                                                        if (vErr1 > 0)
                                                        {
                                                            string vSanctionId = GetSanctionId(vLoanAppNo);
                                                            vTotalCharge = (Convert.ToDecimal(CAM.InsurancePremiumAmount == "" ? "0" : CAM.InsurancePremiumAmount) + Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees));
                                                            vNetDisbAmt = Convert.ToDecimal(CAM.CMLApprovedLoanAmount) - vTotalCharge;
                                                            if (vPartnerID == "FDF")
                                                            {
                                                                vProcPer = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) / Convert.ToDecimal(CAM.CMLApprovedLoanAmount)) * 100;
                                                                vProcFee = Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees);
                                                            }
                                                            else
                                                            {
                                                                vProcFee = (Convert.ToDecimal(CAM.CMLApprovedLoanAmount) * 3) / 100;
                                                                vProcFeeCGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                                vProcFeeSGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                                vProcFeeCGSTPer = 9;
                                                                vProcFeeSGSTPer = 9;
                                                                vProcPer = 3;
                                                            }
                                                            string vRepayType = CAM.ApprovedPaymentFrequency == null ? "M" : CAM.ApprovedPaymentFrequency == "" ? "M" : CAM.ApprovedPaymentFrequency;

                                                            Int32 vErr4 = SaveSanction(ref vSanctionId, vSanctionId, vLoanAppNo, vMemberId, DateTime.Now, Convert.ToDecimal(CAM.CMLApprovedLoanAmount), Convert.ToInt32(vLoanTypeId), 0,
                                                                Convert.ToDecimal(CAM.ROI), Convert.ToInt32(CAM.CMLApprovedTenure), Convert.ToInt32(CAM.CMLApprovedTenure), "R"
                                                                , vRepayType, Convert.ToDecimal(CAM.EMI), vProcFee, 0, 0, 0,
                                                                DateTime.Now, setDate(CAM.EstimatedCollectionDate), "N", vNetDisbAmt, "N", "P", DateTime.Now, "Admin", "", vBrCode, 0, 0, 0, 0, vProcPer, 0, 0,
                                                                0, 0, 0, vTotalCharge, 1, "Edit", 0, DateTime.Now, 0, 0, "", "", "", vProcFeeCGSTPer, vProcFeeCGST, vProcFeeSGSTPer, vProcFeeSGST, 0, 0, "", "",
                                                                0, 0, 0, Convert.ToDecimal(CAM.InsurancePremiumAmount), 0, 0, 0, 0, 0, 0, "", "", "", 0, "", 0, 0, 0, 0, I.CustomerOptedForInsurance == "Y" ? 2 : 0);
                                                            if (vErr4 == 0)
                                                            {
                                                                int vErr5 = SaveFinalSanction(vSanctionId, "S", "Admin", DateTime.Now, "", 0);
                                                                if (vErr5 == 0)
                                                                {
                                                                    pStatusDesc = "Success";
                                                                }
                                                                else
                                                                {
                                                                    pStatusDesc = "Failed:Problem in HO Sanction Approval Data Save";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                pStatusDesc = "Failed:Problem in Pre Sanction Approval Data Save";
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    pStatusDesc = "Failed:Problem in PD Data Save";
                                                }
                                            }
                                            else
                                            {
                                                pStatusDesc = "Failed:Problem in Loan Application Data Save";
                                            }
                                        }
                                        else
                                        {
                                            pStatusDesc = "Failed:Problem in Nominee Data Update";
                                        }
                                    }
                                    else
                                    {
                                        pStatusDesc = "Failed:Problem in Personal Data Update";
                                    }
                                }
                                else
                                {
                                    pStatusDesc = vLeadApproval + "Problem in Lead Approval Save";
                                }
                            }
                            else
                            {
                                pStatusDesc = vLead + " ..Problem in Lead and Personal Data Save";
                            }
                        }
                        else if (vErr2 == 2)
                        {
                            pStatusDesc = "Failed:Duplicate ID Found ... Please check PAN or Aadhaar..";
                        }
                        else if (vErr2 == 3)
                        {
                            pStatusDesc = pErrDesc;
                        }
                        else if (vErr2 == 4)
                        {
                            pStatusDesc = pErrDesc;
                        }
                        else if (vErr2 == 5)
                        {
                            pStatusDesc = "Failed:ID Found Blank ... Please check PAN or Aadhaar..";
                        }
                        else if (vErr2 == 6)
                        {
                            pStatusDesc = "Failed:Day End Done so you can not Push data for today..";
                        }
                        else if (vErr2 == 7)
                        {
                            pStatusDesc = "Failed:Day Begin not Done so you can not Push data for today..";
                        }
                        else
                        {
                            pStatusDesc = "Failed:Problem in Data Set Sent";
                        }
                    }
                    else
                    {
                        pStatusDesc = pErrDesc1;
                    }
                }
                catch (Exception ex)
                {
                    pStatusDesc = pStatusDesc + " " + ex.Message;
                }
            }
            if (pStatusDesc == "Success")
            {
                return vLoanAppNo;
            }
            else
            {
                return pStatusDesc;
            }
        }

        public string ChkDdup(RequestHeader requestHeader, RequestBody requestbody)
        {
            string pStatusDesc = "Failed:", pErrDesc1 = "";
            string vBrCode = "0000", vPartnerID = "";
            int vErr3 = 0;

            KYCDetails K = requestbody.KYCDetails;
            vPartnerID = requestHeader.PartnerID.Trim().ToUpper();

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
            }
            else if (vBrCode == "")
            {
                pStatusDesc = "Failed:Branch Code Not found..";
            }
            else
            {
                try
                {
                    //0-->Fresh Member
                    //56-->Existing Member both with close loan (JLG) and(MEL)
                    //57-->Existing member MEL with close loan
                    //55-->Existing Member with close loan (JLG)
                    //---------------DDup Check-------------------
                    vErr3 = chkDdupMEL(K.PANCardNumber, K.AadhaarDocumentNumber, K.POANumber, vBrCode, ref pErrDesc1);
                    pStatusDesc = pErrDesc1;
                }
                catch (Exception ex)
                {
                    pStatusDesc = pStatusDesc + " " + ex.Message;
                }
            }
            return pStatusDesc;
        }

        public DataFormResponse DataFromNew(DataFormRequest DataFormRequest)
        {
            string pStatusDesc = "Failed:", pErrDesc = "", pErrDesc1 = "";
            string vLeadId = "", vMemberId = "", vMemberNo = "", vDate = "", vLoanAppNo = "", vMaritalStatus = "M",
                vBankAccountType = "1", vBrCode = "", vDOB = "", vPartnerID = "", vNomDOB = "", vDOF = "";
            int vErr2 = 0, vErr3 = 0;
            decimal vNetDisbAmt = 0, vTotalCharge = 0, vProcPer = 0, vProcFee = 0, vProcFeeCGST = 0, vProcFeeSGST = 0, vProcFeeCGSTPer = 0, vProcFeeSGSTPer = 0;
            DataFormResponse response = new DataFormResponse("500", "Fail", "Failed:Data Upload Failed.", "");

            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            var json = new JavaScriptSerializer().Serialize(DataFormRequest);
            PreliminaryDetails P = DataFormRequest.RequestBody.PreliminaryDetails;
            ResidenceAddressDetails R = DataFormRequest.RequestBody.ResidenceAddressDetails;
            KYCDetails K = DataFormRequest.RequestBody.KYCDetails;
            BusinessDetails B = DataFormRequest.RequestBody.BusinessDetails;
            BankDetails BNK = DataFormRequest.RequestBody.BankDetails;
            CustomerLoanDetails CLD = DataFormRequest.RequestBody.CustomerLoanDetails;
            CAMDetails CAM = DataFormRequest.RequestBody.CAMDetails;
            OtherDetails O = DataFormRequest.RequestBody.OtherDetails;
            InsuranceDetails I = DataFormRequest.RequestBody.InsuranceDetails;
            BranchInformation BR = DataFormRequest.RequestBody.BranchInformation;

            vMaritalStatus = P.MaritalStatus == null ? "0" : P.MaritalStatus == "M" ? "1" : P.MaritalStatus == "U" ? "2" : P.MaritalStatus == "W" ? "3" : "4";
            vBankAccountType = BNK.BankAccountType == null ? "0" : BNK.BankAccountType.Trim() == "Saving Account" ? "1" : BNK.BankAccountType.Trim() == "Current Account" ? "2" : BNK.BankAccountType.Trim() == "Joint Saving Account" ? "3" : BNK.BankAccountType.Trim() == "Overdraft Account" ? "4" : "5";

            //vBrCode = BR.BranchCode == "" ? "0000" : BR.BranchCode;
            vBrCode = "0000";

            //vDate = DateTime.Now.ToString("MM/dd/yyyy");
            //vDOF = DateTime.Now.ToString("MM/dd/yyyy");
            //string[] vDt = P.DateOfBirth.Split('-');
            //vDOB = vDt[1] + "-" + vDt[0] + "-" + vDt[2];

            vDate = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST
            vDOF = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST            
            vDOB = P.DateOfBirth;//OPEN FOR TEST

            if (I.CustomerOptedForInsurance == "Y")
            {
                string[] vNBOB = I.NomineeDOB.Split('-');
                vNomDOB = vNBOB[1] + "-" + vNBOB[0] + "-" + vNBOB[2];
            }

            vPartnerID = DataFormRequest.RequestHeader.PartnerID.Trim().ToUpper();

            if (DataFormRequest.RequestHeader.UserID.Trim() == "" || DataFormRequest.RequestHeader.Password.Trim() == "" || DataFormRequest.RequestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (DataFormRequest.RequestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && DataFormRequest.RequestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (DataFormRequest.RequestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (DataFormRequest.RequestHeader.PartnerID.Trim().ToUpper() != "PNB" && DataFormRequest.RequestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else if (vBrCode == "")
            {
                pStatusDesc = "Failed:Branch Code Not found..";
            }
            else
            {
                try
                {
                    //0-->Fresh Member
                    //56-->Existing Member both with close loan (JLG) and(MEL)
                    //57-->Existing member MEL with close loan
                    //55-->Existing Member with close loan (JLG)

                    //---------------DDup Check-------------------
                    vErr3 = chkDdupMEL(K.PANCardNumber, K.AadhaarDocumentNumber, K.POANumber, vBrCode, ref pErrDesc1);
                    if (vErr3 == 0 || vErr3 == 55 || vErr3 == 56 || vErr3 == 57)
                    {
                        //------------------Save Pay Near by-------------------------------
                        vErr2 = SavePayNearByDtl(P.MobileNumber, P.IsMobileNumberDifferentThenLinkedtoAadhaar, P.MobileNumberLinkedWithAadhaar, P.FullName, P.Gender, P.DateOfBirth,
                            P.EmailID, P.NumberOfDependants, K.PANCardNumber, K.POADocumentType, K.POANumber, K.AadhaarDocumentNumber, R.Address, R.Pincode, R.PincodeClassification,
                            R.CityDistrict, R.State, R.ResidenceOwnershipType, R.SinceWhenAreYouStayingHere, B.BusinessAddress, B.Pincode, B.PincodeClassification,
                            B.CityDistrict, B.State, B.BusinessPremiseOwnershipType, B.SinceWhenAreYouDoingBusinessHere, B.NoOfYearsInThisBusiness, B.StoreName,
                            B.StoreName, B.HowDoYouDoYourSaleCollectionsInBusiness, B.MonthlyTurnover, BNK.BankAccountType, BNK.BankName, BNK.AccountNumber,
                            BNK.IFSCCode, BNK.AccountOperationalScince, O.MonthlyFamilyExpense, O.AnyOtherIncome, O.PurposeOfLoan, O.ExistingLoan, O.TypeOfLoan,
                            O.ExistingCreditCard, CLD.LoanAmountRequired, CLD.LoanTenureRequired, CLD.ROIfixed, CAM.NBTScore, CAM.PNBIncomeForLast24months, CAM.PNBGTVForLast24months,
                            CAM.NBTRecommendedLoanAmount, CAM.NBTrecommendedTenure, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, CAM.ROI, CAM.EstimatedDisbursementDate,
                            CAM.EstimatedCollectionDate, CAM.EMI, "", CAM.ProcessingFees, CAM.CommentsAddedByCMLInCAM, CAM.LoanApprovedRejected, CAM.RejectedReason
                            , R.Area, B.GoogleMapAddress, B.LatLongOfTheAddress, I.CustomerOptedForInsurance, I.NomineeName, I.RelationshipWithNominee,
                            I.NomineeGender, I.NomineeDOB, vBrCode, CAM.ApprovedPaymentFrequency, vPartnerID, ref pErrDesc);
                        if (vErr2 == 0)
                        {
                            string vLead = "";
                            if (vErr3 == 56 || vErr3 == 57)
                            {
                                vLead = "Success:Record Saved Successfully#123";
                            }
                            else
                            {
                                vLead = SaveLead("", vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "N", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID, "-1", "", "-1", "");
                            }
                            string[] arr1 = vLead.Split('#');
                            if (arr1[0] == "Success:Record Saved Successfully")
                            {
                                vLeadId = arr1[1];//Lead Id
                                string vLeadApproval = "";
                                string[] arr10 = null;

                                if (vErr3 == 56 || vErr3 == 57)
                                {
                                    arr10 = pErrDesc1.Split('#');
                                    vLeadApproval = "Success:" + arr10[1];
                                    vDOF = arr10[2];
                                    if (vErr3 == 56)
                                    {
                                        vMemberNo = arr10[3];
                                    }
                                }
                                else
                                {
                                    if (vErr3 == 55)
                                    {
                                        arr10 = pErrDesc1.Split('#');
                                        vMemberNo = arr10[1];
                                    }
                                    //Approve Lead
                                    vLeadApproval = ApproveLead(vLeadId, vDate, P.FullName, P.EmailID, P.MobileNumber, R.Address, "-1", "-1", "Y", "0", "0", "0", "0", "0", "0", vBrCode, "1", vPartnerID);
                                }
                                string[] arr2 = vLeadApproval.Split(':');
                                if (arr2[0] == "Success")
                                {
                                    vMemberId = arr2[1];//Member Id
                                    if (vErr3 != 55 || vErr3 != 56)
                                    {
                                        vMemberNo = arr2[1];//Member No
                                    }
                                    string vAge = Convert.ToString(GetAge(setDate(P.DateOfBirth)));
                                    //Save Member
                                    string vSaveCompany = Mob_SaveCompany(vMemberId, vMemberNo, "-1", P.FullName, vDOF, vDOB, "-1", "", "", P.EmailID, K.PANCardNumber, "6", K.AadhaarDocumentNumber, "", "", "-1", "-1", R.Address, "", R.State, R.CityDistrict, R.Pincode, P.MobileNumber
                                        , "", "", "N", "", "", "", "", "", "", "", "", vBrCode, "1", P.FullName, P.MobileNumber, "", "1", K.PANCardNumber, "-1", "-1", B.BusinessAddress, "0", P.Gender == "M" ? "1" : P.Gender == "F" ? "2" : "3", vAge, "-1", "", "-1", "-1", "-1", R.SinceWhenAreYouStayingHere == "" ? "0" : R.SinceWhenAreYouStayingHere,
                                        "", P.FullName, BNK.AccountNumber, BNK.BankName, BNK.IFSCCode, BNK.AccountOperationalScince == "" ? "0" : BNK.AccountOperationalScince, vBankAccountType, "", "", "", "0", "", "0", "0", "", "", "", B.BusinessAddress, "",
                                        "", B.CityDistrict, B.Pincode, B.State, "", "", "", vMaritalStatus, R.ResidenceOwnershipType, "", B.StoreType, B.NoOfYearsInThisBusiness == "" ? "0" : B.NoOfYearsInThisBusiness, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", vPartnerID);
                                    string[] arr3 = vSaveCompany.Split(':');
                                    //pStatusDesc = vSaveCompany; 
                                    if (arr3[0] == "Record Saved Successfully")
                                    {
                                        string vErrMsg = "";
                                        if (I.CustomerOptedForInsurance == "Y")
                                        {
                                            //Save Co Applicant
                                            string vSaveCoApplicant = Mob_SaveCoApplicant("", vMemberId, vDate, I.NomineeName, vNomDOB, "0", I.NomineeGender.Trim() == "Male" ? "1" : I.NomineeGender.Trim() == "Female" ? "2" : "3", "-1", "-1", "-1", "1", "-1", "", "", "", "", "", "", "", "", "", "", "",
                                                "-1", "", "-1", "", "", "", "", "", "", "", vBrCode, "0", "0", "1", "", "", "", "", ""
                                                , "-1", "", "-1", "-1", "", "", GetRelationIdByRelation(I.RelationshipWithNominee), "-1", "", "", "N", "", "", "", "", "0", "-1", "", "", "0", "", "0", "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                                            string[] arr5 = vSaveCoApplicant.Split(':');
                                            vErrMsg = arr5[0];
                                        }
                                        else
                                        {
                                            vErrMsg = "Record Saved Successfully";
                                        }
                                        if (vErrMsg == "Record Saved Successfully")
                                        {
                                            string vPurposeID = GetPurposeIdByPurposeName(O.PurposeOfLoan);
                                            string vLoanTypeId = vPartnerID == "PNB" ? "3" : "4";
                                            //Save Loan Application
                                            string vLoanApplication = SaveLoanApplication(vMemberId, vDate, vPurposeID, vLoanTypeId, CAM.CMLApprovedLoanAmount, CAM.CMLApprovedTenure, "N", vBrCode, "1", "I", "0", "-1", "", "Y", vDate, "", vPartnerID);
                                            string[] arr4 = vLoanApplication.Split('#');
                                            if (arr4[0] == "Success:Record Saved Successfully")
                                            {
                                                vLoanAppNo = arr4[1];//Loan Application Id
                                                //Save PD
                                                string vPd = SavePDMst("0", vLoanAppNo, vMemberId, "-1", "", vDate, "", "0", "0", "0", "0", vBrCode, "1", "", "", vPartnerID);
                                                if (vPd == "Record Saved Successfully")
                                                {
                                                    int vErr = SaveIIRRatio(vLoanAppNo, 0, 0, 0, 0, 0, Convert.ToDecimal(CAM.EMI), "IIR", 60, Convert.ToInt32(CAM.CMLApprovedTenure), 0, 1);
                                                    if (vErr > 0)
                                                    {
                                                        Int32 vErr1 = SavePDFinalApprove(vLoanAppNo, "A", DateTime.Now, "", 1, "", "", "", "", "", "", Convert.ToDecimal(CAM.CMLApprovedLoanAmount));
                                                        if (vErr1 > 0)
                                                        {
                                                            string vSanctionId = GetSanctionId(vLoanAppNo);
                                                            vTotalCharge = (Convert.ToDecimal(CAM.InsurancePremiumAmount == "" ? "0" : CAM.InsurancePremiumAmount) + Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees));
                                                            vNetDisbAmt = Convert.ToDecimal(CAM.CMLApprovedLoanAmount) - vTotalCharge;
                                                            if (vPartnerID == "FDF")
                                                            {
                                                                vProcPer = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) / Convert.ToDecimal(CAM.CMLApprovedLoanAmount)) * 100;
                                                                vProcFee = Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees);
                                                            }
                                                            else
                                                            {
                                                                vProcFee = (Convert.ToDecimal(CAM.CMLApprovedLoanAmount) * 3) / 100;
                                                                vProcFeeCGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                                vProcFeeSGST = (Convert.ToDecimal(CAM.ProcessingFees == "" ? "0" : CAM.ProcessingFees) - vProcFee) / 2;
                                                                vProcFeeCGSTPer = 9;
                                                                vProcFeeSGSTPer = 9;
                                                                vProcPer = 3;
                                                            }
                                                            string vRepayType = CAM.ApprovedPaymentFrequency == null ? "M" : CAM.ApprovedPaymentFrequency == "" ? "M" : CAM.ApprovedPaymentFrequency;

                                                            Int32 vErr4 = SaveSanction(ref vSanctionId, vSanctionId, vLoanAppNo, vMemberId, DateTime.Now, Convert.ToDecimal(CAM.CMLApprovedLoanAmount), Convert.ToInt32(vLoanTypeId), 0,
                                                                Convert.ToDecimal(CAM.ROI), Convert.ToInt32(CAM.CMLApprovedTenure), Convert.ToInt32(CAM.CMLApprovedTenure), "R"
                                                                , vRepayType, Convert.ToDecimal(CAM.EMI), vProcFee, 0, 0, 0,
                                                                DateTime.Now, setDate(CAM.EstimatedCollectionDate), "N", vNetDisbAmt, "N", "P", DateTime.Now, "Admin", "", vBrCode, 0, 0, 0, 0, vProcPer, 0, 0,
                                                                0, 0, 0, vTotalCharge, 1, "Edit", 0, DateTime.Now, 0, 0, "", "", "", vProcFeeCGSTPer, vProcFeeCGST, vProcFeeSGSTPer, vProcFeeSGST, 0, 0, "", "",
                                                                0, 0, 0, Convert.ToDecimal(CAM.InsurancePremiumAmount), 0, 0, 0, 0, 0, 0, "", "", "", 0, "", 0, 0, 0, 0, I.CustomerOptedForInsurance == "Y" ? 2 : 0);
                                                            if (vErr4 == 0)
                                                            {
                                                                int vErr5 = SaveFinalSanction(vSanctionId, "S", "Admin", DateTime.Now, "", 0);
                                                                if (vErr5 == 0)
                                                                {
                                                                    pStatusDesc = "Success";
                                                                }
                                                                else
                                                                {
                                                                    pStatusDesc = "Failed:Problem in HO Sanction Approval Data Save";
                                                                }
                                                            }
                                                            else
                                                            {
                                                                pStatusDesc = "Failed:Problem in Pre Sanction Approval Data Save";
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    pStatusDesc = "Failed:Problem in PD Data Save";
                                                }
                                            }
                                            else
                                            {
                                                pStatusDesc = "Failed:Problem in Loan Application Data Save";
                                            }
                                        }
                                        else
                                        {
                                            pStatusDesc = "Failed:Problem in Nominee Data Update";
                                        }
                                    }
                                    else
                                    {
                                        pStatusDesc = "Failed:Problem in Personal Data Update";
                                    }
                                }
                                else
                                {
                                    pStatusDesc = vLeadApproval + "Problem in Lead Approval Save";
                                }
                            }
                            else
                            {
                                pStatusDesc = vLead + " ..Problem in Lead and Personal Data Save";
                            }
                        }
                        else if (vErr2 == 2)
                        {
                            pStatusDesc = "Failed:Duplicate ID Found ... Please check PAN or Aadhaar..";
                        }
                        else if (vErr2 == 3)
                        {
                            pStatusDesc = pErrDesc;
                        }
                        else if (vErr2 == 4)
                        {
                            pStatusDesc = pErrDesc;
                        }
                        else if (vErr2 == 5)
                        {
                            pStatusDesc = "Failed:ID Found Blank ... Please check PAN or Aadhaar..";
                        }
                        else if (vErr2 == 6)
                        {
                            pStatusDesc = "Failed:Day End Done so you can not Push data for today..";
                        }
                        else if (vErr2 == 7)
                        {
                            pStatusDesc = "Failed:Day Begin not Done so you can not Push data for today..";
                        }
                        else
                        {
                            pStatusDesc = "Failed:Problem in Data Set Sent";
                        }
                    }
                    else
                    {
                        pStatusDesc = pErrDesc1;
                    }
                }
                catch (Exception ex)
                {
                    pStatusDesc = pStatusDesc + " " + ex.Message;
                }
            }
            if (pStatusDesc == "Success")
            {
                response = new DataFormResponse("200", "Success", "Success:Record successfully saved.", vLoanAppNo);
                return response;
            }
            else
            {
                response = new DataFormResponse("500", "Fail", pStatusDesc, vLoanAppNo);
                return response;
            }
        }


        public string DataFrom1(RequestHeader requestHeader, RequestBody requestbody)
        {
            string pStatusDesc = "Failed:", pErrDesc1 = "";
            string vDate = "", vLoanAppNo = "00000000002", vMaritalStatus = "M", vBankAccountType = "1",
                vBrCode = "", vDOB = "", vPartnerID = "", vNomDOB = "", vDOF = "";
            int vErr3 = 0;

            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            var json = new JavaScriptSerializer().Serialize(requestbody);
            PreliminaryDetails P = requestbody.PreliminaryDetails;
            ResidenceAddressDetails R = requestbody.ResidenceAddressDetails;
            KYCDetails K = requestbody.KYCDetails;
            BusinessDetails B = requestbody.BusinessDetails;
            BankDetails BNK = requestbody.BankDetails;
            CustomerLoanDetails CLD = requestbody.CustomerLoanDetails;
            CAMDetails CAM = requestbody.CAMDetails;
            OtherDetails O = requestbody.OtherDetails;
            InsuranceDetails I = requestbody.InsuranceDetails;
            BranchInformation BR = requestbody.BranchInformation;

            vMaritalStatus = P.MaritalStatus == null ? "0" : P.MaritalStatus == "M" ? "1" : P.MaritalStatus == "U" ? "2" : P.MaritalStatus == "W" ? "3" : "4";
            vBankAccountType = BNK.BankAccountType == null ? "0" : BNK.BankAccountType.Trim() == "Saving Account" ? "1" : BNK.BankAccountType.Trim() == "Current Account" ? "2" : BNK.BankAccountType.Trim() == "Joint Saving Account" ? "3" : BNK.BankAccountType.Trim() == "Overdraft Account" ? "4" : "5";
            vBrCode = "0000";
            vDate = DateTime.Now.ToString("MM/dd/yyyy");
            vDOF = DateTime.Now.ToString("MM/dd/yyyy");
            string[] vDt = P.DateOfBirth.Split('-');
            vDOB = vDt[1] + "-" + vDt[0] + "-" + vDt[2];

            //vDate = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST
            //vDOF = DateTime.Now.ToString("dd/MM/yyyy");//OPEN FOR TEST            
            //vDOB = P.DateOfBirth;//OPEN FOR TEST

            if (I.CustomerOptedForInsurance == "Y")
            {
                string[] vNBOB = I.NomineeDOB.Split('-');
                vNomDOB = vNBOB[1] + "-" + vNBOB[0] + "-" + vNBOB[2];
            }

            vPartnerID = requestHeader.PartnerID.Trim().ToUpper();

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
            }
            else if (vBrCode == "")
            {
                pStatusDesc = "Failed:Branch Code Not found..";
            }
            else
            {
                try
                {
                    vErr3 = chkDdupMEL(K.PANCardNumber, K.AadhaarDocumentNumber, K.POANumber, vBrCode, ref pErrDesc1);
                    if (vErr3 == 0 || vErr3 == 55 || vErr3 == 56 || vErr3 == 57)
                    {
                        pStatusDesc = "Success";
                    }
                    else
                    {
                        pStatusDesc = pErrDesc1;
                    }
                }
                catch (Exception ex)
                {
                    pStatusDesc = pStatusDesc + " " + ex.Message;
                }
            }
            if (pStatusDesc == "Success")
            {
                return vLoanAppNo;
            }
            else
            {
                return pStatusDesc;
            }
        }

        #endregion

        #region PaynearBY SaveFOIR

        public Int32 SaveIIRRatio(string pLoanAppId, decimal pGrossInc, decimal pGrossExp, decimal pNetInc, decimal pObligation, decimal pIIR,
          decimal pLoanEligibility, string pDESC, decimal pIIRPer, Int32 pTenure, Int32 pErr, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIIRRatio";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pGrossInc", pGrossInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pGrossExp", pGrossExp);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetInc", pNetInc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pObligation", pObligation);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIIR", pIIR);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pLoanEligibility", pLoanEligibility);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pDESC", "IIR");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pIIRPer", pIIRPer);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pTenure", pTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
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

        #endregion

        #region PaynearBY SaveFinalPD

        public Int32 SavePDFinalApprove(string pLoanAppId, string pFinalPDStatus, DateTime pFinalPDDate, string pRemarks, Int32 pFinalPDDoneBy,
            string pLevel1, string pLevel2, string pLevel3, string pLevel4, string pLevel5, string pLevel6, decimal pSanctionAmt)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vErrMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePDFinalApprove";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFinalPDStatus", pFinalPDStatus);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 8, "@pFinalPDDate", pFinalPDDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pRemarks", pRemarks);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFinalPDDoneBy", pFinalPDDoneBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrMsg", "");

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel1", pLevel1);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel2", pLevel2);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel3", pLevel3);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel4", pLevel4);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel5", pLevel5);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLevel6", pLevel6);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pSanctionAmt", pSanctionAmt);
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

        #endregion

        #region PaynearBY Digital Document

        public DataSet GetAgrDigitalDocs(string pSancId)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetSMEAgrDigiDocs";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pSancId", pSancId);
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

        private string GetReportDocForDigitalSign(string pLoanAppNo, string pMemberName)
        {
            DataSet ds = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null;
            string vRptPath = "", vFileName = "No File Created";
            string vSanctionId = GetSanctionId(pLoanAppNo);
            ReportDocument rptDoc = new ReportDocument();
            try
            {
                string vSancId = vSanctionId;
                vRptPath = string.Empty;
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/DigitalDocs.rpt"));
                DataTable dt = new DataTable();

                ds = new DataSet();
                ds = GetAgrDigitalDocs(vSancId);
                dtAppFrm1 = ds.Tables[0];
                dtAppFrm2 = ds.Tables[1];
                dtSancLetter = ds.Tables[2];
                dtEMISchedule = ds.Tables[3];
                dtLoanAgr = ds.Tables[4];
                dtAuthLetter = ds.Tables[5];
                dtKotak = ds.Tables[6];

                if (dtAppFrm1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        vFileName = "C:\\DDHTML\\" + pLoanAppNo + '_' + "MEL" + "_" + pMemberName + ".pdf";
                        using (rptDoc)
                        {
                            //vRptName = "DigitalDocument";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dtAppFrm1);
                            rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                            rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                            rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                            rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                            rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                            rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);
                            rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                        }
                    }
                }
                else
                {
                    vFileName = "No File Created..As data Not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rptDoc.Dispose();
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;
            }
            return vFileName;
        }

        public List<DigiDocResponse> DigitalDocument(PostCredential vPostCredential, PostDigiDocDataSet vPostDigiDocDataSet)
        {
            CRepository oRepo = new CRepository();
            byte[] formData = null;
            List<DigiDocResponse> row = new List<DigiDocResponse>();

            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    row.Add(new DigiDocResponse(formData, "Credential Fields are Blank.."));
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    row.Add(new DigiDocResponse(formData, "User Id Not found.."));
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    row.Add(new DigiDocResponse(formData, "Password Not Matched.."));
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    row.Add(new DigiDocResponse(formData, "Partner ID Not Matched.."));
                }
                else
                {
                    #region Craete Report Doc

                    string Id = vPostDigiDocDataSet.LoanAppNo;
                    string pMemberName = vPostDigiDocDataSet.CustomerName;
                    string vFileName = "";

                    vFileName = GetReportDocForDigitalSign(vPostDigiDocDataSet.LoanAppNo, pMemberName);

                    #endregion
                    #region API Call for Digital Document
                    //pdf save
                    if (vFileName != "No File Created..As data Not found")
                    {
                        string vNSDLResponse = "";
                        //vFileName = "C:\\DDHTML\\FAQ.pdf";

                        formData = File.ReadAllBytes(vFileName);

                        vNSDLResponse = EquifaxDIGITALDOCUMENT("production.centrum@equifax.com", vPostDigiDocDataSet.LoanAppNo, vPostDigiDocDataSet.CustomerName, "", "MEL", "Mumbai");
                        //string vNSDLResponse = EquifaxDIGITALDOCUMENT("demo@authbridge.com", Id, Convert.ToString(dt1.Rows[0]["MemberName"]), "", vRptName, "Mumbai");
                        SavePNBDigitalDocResponse(Id, vNSDLResponse);

                        File.WriteAllText(vFileName.Replace(".pdf", ".htm"), vNSDLResponse);
                        vNSDLResponse = vNSDLResponse.Replace("\\n", "");


                        row.Add(new DigiDocResponse(formData, vNSDLResponse));
                    }
                    else
                    {
                        row.Add(new DigiDocResponse(formData, "Error.." + vFileName));
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                row.Add(new DigiDocResponse(formData, "Error.." + ex.Message));
            }
            return row;
        }

        public string EquifaxDIGITALDOCUMENT(string pUserName, string pUniqueID, string pFirstName, string pLastName, string pReason, string pLocation)
        {
            string vFileName = "";
            vFileName = "C:\\DDHTML\\" + pUniqueID + '_' + pReason + "_" + pFirstName + ".pdf";
            //vFileName = "C:\\DDHTML\\FAQ.pdf";
            // Read file data
            FileStream fs = new FileStream(vFileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();
            pUniqueID = pUniqueID + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", vFileName);
            //postParameters.Add("fileformat", "pdf");
            postParameters.Add("transID", pUniqueID);
            postParameters.Add("docType", "373");//"42" for esign ------- 373 aadhaar esign
            postParameters.Add("file", new FileParameter(data, vFileName, "application/pdf"));
            postParameters.Add("pages", "2");
            postParameters.Add("firstName", pFirstName);
            postParameters.Add("lastName", pLastName);
            postParameters.Add("reason", pReason);
            postParameters.Add("location", pLocation);
            postParameters.Add("page_no", "1");
            postParameters.Add("x_cordinate", "60");//2000
            postParameters.Add("y_cordinate", "60");
            postParameters.Add("authmode", "OTP");

            // Create request and receive response
            string postURL = "https://www.truthscreen.com/api/v2.2/aadhaaresignapi"; // "test_centrum@equifax.com";

            //  string postURL = "https://uat.truthscreen.com/truthscreen-uat/api/v2.2/aadhaaresignapi";

            string fullResponse = MultipartFormDataPost(postURL, pUserName, postParameters);
            return fullResponse;
        }


        public class FileParameter
        {
            public string transID { get; set; }             //Mandatory		Transaction ID provided by Client to make each transaction unique
            public string docType { get; set; }             //Mandatory		Document type is 42 for this.
            public byte[] file { get; set; }              //Mandatory		This indicates the type of document being uploaded for eSign. We currently support PDF, limit 3MB.
            public string pages { get; set; }               //Mandatory		1 - Last Page 2 - All Pages 3 - Pages (Indicates page no on which you want signature to be placed by the help of page_no parameter)
            public string firstName { get; set; }           //Mandatory		The first name which would be displayed in the digital signature. No name would be displayed in the signature if this is missing.
            public string lastName { get; set; }            //Optional		The last name which would be displayed in the digital signature. No name would be displayed in the signature if this is missing.
            public string reason { get; set; }              //String,Mandatory	Specify the reason for e-signing this document
            public string location { get; set; }            //String,Optional		Optionally specify the location where e-signing is being done
            public string page_no { get; set; }             //Mandatory		Indicates page numbers on which you want signatures to be placed. To be used in case of "3" in pages fields. The field should contain the page numbers based on the multiple signature requirement in a single document. For example if we need to to take 5 signs in a 3 page PDF, where 2 signs are on 1st & 2nd page and 1 sign is on last page the passed value should be 1,1,2,2,3
            public string x_cordinate { get; set; }         //Mandatory		Indicates x coordinates represent the horizontal postion of the signature on page, where you want to place the signature. To be used in case of "3" in pages fields. The number of x coordinates will should be equal to the number of times page numbers are passed in page_no filed. For eg, for above mentioned expample there should be 5 x coordinates eg. 40, 200, 40, 300, 50
            public string y_cordinate { get; set; }         //Mandatory		Indicates y coordinates represent the vertical postion of the signature on page, where you want to place the signature. To be used in case of "3" in pages fields. The number of y coordinates will should be equal to the number of times page numbers are passed in page_no filed. For eg, for above mentioned expample there should be 5 y coordinates eg. 60,60,60,60,60
            public string authmode { get; set; }            //Mandatory		Authmode value should by OTP or BIO
            //public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }

            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] pfile, string filename, string contenttype)
            {
                file = pfile;
                FileName = filename;
                ContentType = contenttype;
            }
        }

        public string MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postParameters"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.file, 0, fileToUpload.file.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="userAgent"></param>
        /// <param name="contentType"></param>
        /// <param name="formData"></param>
        /// <param name="pUniqueID"></param>
        /// <param name="pFirstName"></param>
        /// <param name="pLastName"></param>
        /// <param name="pReason"></param>
        /// <param name="pLocation"></param>
        /// <returns></returns>
        //private HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        private string PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();

                request.Headers.Add("username" + ":" + userAgent);

                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formData.Length;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }


                ///You must write ContentLength bytes to the request stream before calling [Begin]GetResponse.
                ///
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();

                //return request.GetResponse() as HttpWebResponse;

                return fullResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        #endregion

        #region DownloadSchedule

        public DataTable rptRepaySchedule(string pLoanId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptRepaySchedule";
                oCmd.CommandTimeout = 80000;
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
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

        public List<DownSchResponse> DownloadSchedule(PostCredential vPostCredential, PostDownSchDataSet vPostDownSchDataSet)
        {
            CRepository oRepo = new CRepository();
            byte[] formData = null;
            List<DownSchResponse> row = new List<DownSchResponse>();

            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    row.Add(new DownSchResponse(formData, "Credential Fields are Blank.."));
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    row.Add(new DownSchResponse(formData, "User Id Not found.."));
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    row.Add(new DownSchResponse(formData, "Password Not Matched.."));
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    row.Add(new DownSchResponse(formData, "Partner ID Not Matched.."));
                }
                else
                {
                    #region Craete Report Doc

                    string pLoanAppNo = vPostDownSchDataSet.LoanAppNo;
                    string vFileName = "";
                    vFileName = GetReportDocForSchedule(pLoanAppNo);
                    #endregion

                    if (vFileName != "No File Created..As data Not found")
                    {
                        string vNSDLResponse = "Success";
                        //vFileName = "C:\\DDHTML\\FAQ.pdf";

                        formData = File.ReadAllBytes(vFileName);
                        File.WriteAllText(vFileName.Replace(".pdf", ".htm"), vNSDLResponse);
                        row.Add(new DownSchResponse(formData, vNSDLResponse));
                    }
                    else
                    {
                        row.Add(new DownSchResponse(formData, "Error.." + vFileName));
                    }

                }
            }
            catch (Exception ex)
            {
                row.Add(new DownSchResponse(formData, "Error.." + ex.Message));
            }
            return row;
        }

        private string GetReportDocForSchedule(string pLoanAppNo)
        {

            DataTable dt = null;
            string vRptPath = "", vFileName = "No File Created";
            string vLoanId = GetLoanNoByLoanAppId(pLoanAppNo);
            ReportDocument rptDoc = new ReportDocument();
            try
            {
                vRptPath = string.Empty;
                vRptPath = HostingEnvironment.MapPath(string.Format("~/Reports/RepaySche.rpt"));
                dt = rptRepaySchedule(vLoanId, "0000");
                if (dt.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        vFileName = "C:\\DDHTML\\" + pLoanAppNo + '_' + "MEL" + ".pdf";
                        using (rptDoc)
                        {
                            //vRptName = "DigitalDocument";
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt);
                            rptDoc.SetParameterValue("pCmpName", "Unity Small Finance Bank (MEL)");
                            rptDoc.SetParameterValue("pAddress1", "");
                            rptDoc.SetParameterValue("pAddress2", "");
                            rptDoc.SetParameterValue("pBranch", "");
                            rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                            rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                        }
                    }
                }
                else
                {
                    vFileName = "No File Created..As data Not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rptDoc.Dispose();
                dt = null;
            }
            return vFileName;
        }

        #endregion

        #region PaynearBy Customer DocUpload LoanAppID as reference Number

        private string AllExternalImageUpload(byte[] imageBinary, string imageGroup, string folderName, string imageName, string Extesion)
        {
            string filePath = "";
            string isImageSaved = "N";
            string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", imageGroup, folderName));
            System.IO.Directory.CreateDirectory(folderPath);
            //if (imageGroup == "CAM")
            //{
            //    filePath = string.Format("{0}/{1}.pdf", folderPath, imageName);
            //}           
            //else
            //{
            //    filePath = string.Format("{0}/{1}.png", folderPath, imageName);
            //}
            filePath = string.Format("{0}/{1}", folderPath, imageName + Extesion);
            if (imageBinary != null)
            {
                File.WriteAllBytes(filePath, imageBinary);
                isImageSaved = "Y";
            }
            return isImageSaved;
        }

        public List<AllExternalImageUpload> AllExternalImageUpload(Stream image)
        {
            string vErr = "";
            List<AllExternalImageUpload> row = new List<AllExternalImageUpload>();
            string vReferenceNo = "";

            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "ReferenceNo")
                        {
                            vReferenceNo = content.StringData;
                        }
                        else if (content.PropertyName == "UserID")
                        {
                            if (content.StringData != "CENTRUMSMEUAT" && content.StringData != "CENTRUMSME")
                            {
                                row.Add(new AllExternalImageUpload("Failed", "Invalid User ID"));
                                return row;
                            }
                        }
                        else if (content.PropertyName == "Password")
                        {
                            if (content.StringData != "ABCD*1234")
                            {
                                row.Add(new AllExternalImageUpload("Failed", "Invalid Password"));
                                return row;
                            }
                        }
                    }
                    else
                    {
                        byte[] binaryWriteArray = content.Data;
                        string fileName = Path.GetFileName(content.FileName);
                        string vFileTag = "", vFileExt = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        vFileExt = Path.GetExtension(content.FileName);
                        if (vFileTag.Equals("CAM"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "CAM", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.ToUpper().Equals("LOANAGREE"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "LOANAGREE", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        //else
                        //{
                        //System.Drawing.Image img = LoadImage(binaryWriteArray);
                        //if (img != null)
                        //{
                        else if (vFileTag.ToUpper().Equals("SELFIE"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "SELFIE", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("PAN"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "PAN", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("POA-AadhaarF"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "POA-AadhaarF", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("POA-AadhaarB"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "POA-AadhaarB", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("POA-VoterIDF"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "POA-VoterIDF", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("POA-VoterIDB"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "POA-VoterIDB", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        else if (vFileTag.Equals("POA-DL"))
                        {
                            vErr = AllExternalImageUpload(binaryWriteArray, "POA-DL", vReferenceNo, vFileTag, vFileExt);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new AllExternalImageUpload("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new AllExternalImageUpload("Failed", fileName));
                            }
                        }
                        //}
                        //else
                        //{
                        //    row.Add(new AllExternalImageUpload("Failed", fileName));
                        //}
                        //}
                    }
                }
                return row;
            }
            else
            {
                row.Add(new AllExternalImageUpload("Failed", "No Data Found"));
            }
            return row;
        }
        #endregion

        #region PaynearBy Callback to check Digitally signed is done or not
        public string DigitalSignStatus(PostCredential vPostCredential, PostDigiDocDataSet vPostDigiDocDataSet)
        {
            string vStatus = "Not Done Succesfully";
            string vReferenceNo = vPostDigiDocDataSet.LoanAppNo;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    vStatus = "Credential Fields are Blank..";
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    vStatus = "User Id Not found..";
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    vStatus = "Password Not Matched..";
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    vStatus = "Partner ID Not Matched..";
                }
                else
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "GetDigitalSignStatus";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pReferenceNo", vReferenceNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pStatus", vStatus);
                    DBUtility.Execute(oCmd);
                    vStatus = Convert.ToString(oCmd.Parameters["@pStatus"].Value);
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
            return vStatus;
        }
        #endregion

        #region PaynearBy Callback to Update ENACH Process is done or not
        public string ENACHProcessUpdate(PostCredential vPostCredential, PostENACHDataSet vPostENACHDataSet)
        {
            string vUpdateStatus = "Successfully Not Updated";

            string vStatus = vPostENACHDataSet.StatusofProcess;
            string vReferenceNo = vPostENACHDataSet.LoanAppNo;
            Int32 vErr = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    vStatus = "Credential Fields are Blank..";
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    vStatus = "User Id Not found..";
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    vStatus = "Password Not Matched..";
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    vStatus = "Partner ID Not Matched..";
                }
                else
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "ENACHProcessUpdate";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pReferenceNo", vReferenceNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", vStatus);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                    DBUtility.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                        vUpdateStatus = "Successfully Updated";
                    else if (vErr == 2)
                        vUpdateStatus = "Invalid Reference Number";
                    else
                        vUpdateStatus = "Successfully Not Updated";
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
            return vUpdateStatus;
        }
        #endregion

        #region Age Calculation
        /// <summary>
        /// /
        /// </summary>
        /// <param name="pDOB"></param>
        /// <returns></returns>
        private Int32 GetAge(DateTime pDOB)
        {
            int vYear = DateTime.Now.Year - pDOB.Year;
            if (pDOB.Month > DateTime.Now.Month)
            {
                vYear--;
            }
            if (pDOB.Month == DateTime.Now.Month && pDOB.Day > DateTime.Now.Day)
            {
                vYear--;
            }

            return vYear;
        }
        #endregion

        #region GetSanctionDtl
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pSanctionId"></param>
        /// <returns></returns>
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
        #endregion

        #region SaveSanction
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
        #endregion

        #region SavePayNearByDtl
        public Int32 SavePayNearByDtl(string MobileNumber, string IsMobileNumberDifferentThenLinkedtoAadhaar, string MobileNumberLinkedWithAadhaar
            , string FullName, string Gender, string DateOfBirth, string EmailID, string NumberOfDependants, string PANCardNumber, string POADocumentType
            , string POANumber, string AadhaarDocumentNumber, string Address, string Pincode, string PincodeClassification, string CityDistrict
            , string State, string ResidenceOwnershipType, string SinceWhenAreYouStayingHere, string BusinessAddress, string BusiPincode
            , string BusiPincodeClassification, string BusiCityDistrict, string BusiState, string BusinessPremiseOwnershipType
            , string SinceWhenAreYouDoingBusinessHere, string NoOfYearsInThisBusiness, string StoreType, string StoreName
            , string HowDoYouDoYourSaleCollectionsInBusiness, string MonthlyTurnover, string BankAccountType, string BankName
            , string AccountNumber, string IFSCCode, string AccountOperationalScince, string MonthlyFamilyExpense
            , string AnyOtherIncome, string PurposeOfLoan, string ExistingLoan, string TypeOfLoan, string ExistingCreditCard
            , string LoanAmount, string LoanTenure, string CLDROI, string NBTScore, string PNBIncomeForLast24months
            , string PNBGTVForLast24months, string NBTRecommendedLoanAmount, string NBTrecommendedTenure, string CMLApprovedLoanAmount
            , string CMLApprovedTenure, string ROI, string EstimatedDisbursementDate, string EstimatedCollectionDate
            , string EMI, string EstimatedBPIAmount, string ProcessingFees, string CommentsAddedByCMLInCAM
            , string LoanApprovedRejected, string RejectedReason, string Area, string GoogleMapAddress, string LatLongOfTheAddress,
            string CustomerOptedForInsurance, string NomineeName, string RelationshipWithNominee, string NomineeGender,
            string NomineeDOB, string BranchCode, string pApprovedPaymentFrequency, string pSRC, ref string pErrDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePayNearByDtl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMobileNumber", MobileNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pIsMobileNumberDifferentThenLinkedtoAadhaar", IsMobileNumberDifferentThenLinkedtoAadhaar);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMobileNumberLinkedWithAadhaar", MobileNumberLinkedWithAadhaar);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pFullName", FullName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pGender", Gender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pDateOfBirth", DateOfBirth);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pEmailID", EmailID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNumberOfDependants", NumberOfDependants);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPANCardNumber", PANCardNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPOADocumentType", POADocumentType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPOANumber", POANumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAadhaarDocumentNumber", AadhaarDocumentNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pAddress", Address);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPincode", Pincode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPincodeClassification", PincodeClassification);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCityDistrict", CityDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pState", State);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pResidenceOwnershipType", ResidenceOwnershipType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSinceWhenAreYouStayingHere", SinceWhenAreYouStayingHere);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pBusinessAddress", BusinessAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusiPincode", BusiPincode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusiPincodeClassification", BusiPincodeClassification);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusiCityDistrict", BusiCityDistrict);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusiState", BusiState);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBusinessPremiseOwnershipType", BusinessPremiseOwnershipType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSinceWhenAreYouDoingBusinessHere", SinceWhenAreYouDoingBusinessHere);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNoOfYearsInThisBusiness", NoOfYearsInThisBusiness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pStoreType", StoreType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pStoreName", StoreName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pHowDoYouDoYourSaleCollectionsInBusiness", HowDoYouDoYourSaleCollectionsInBusiness);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMonthlyTurnover", MonthlyTurnover);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pBankAccountType", BankAccountType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pBankName", BankName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccountNumber", AccountNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pIFSCCode", IFSCCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccountOperationalScince", AccountOperationalScince);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMonthlyFamilyExpense", MonthlyFamilyExpense);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAnyOtherIncome", AnyOtherIncome);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pPurposeOfLoan", PurposeOfLoan);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pExistingLoan", ExistingLoan);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTypeOfLoan", TypeOfLoan);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pExistingCreditCard", ExistingCreditCard);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLoanAmount", LoanAmount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLoanTenure", LoanTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCLDROI", CLDROI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNBTScore", NBTScore);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPNBIncomeForLast24months", PNBIncomeForLast24months);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pPNBGTVForLast24months", PNBGTVForLast24months);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNBTRecommendedLoanAmount", NBTRecommendedLoanAmount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNBTrecommendedTenure", NBTrecommendedTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCMLApprovedLoanAmount", CMLApprovedLoanAmount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCMLApprovedTenure", CMLApprovedTenure);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pROI", ROI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEstimatedDisbursementDate", EstimatedDisbursementDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEstimatedCollectionDate", EstimatedCollectionDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEMI", EMI);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEstimatedBPIAmount", EstimatedBPIAmount);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pProcessingFees", ProcessingFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pCommentsAddedByCMLInCAM", CommentsAddedByCMLInCAM);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLoanApprovedRejected", LoanApprovedRejected);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pRejectedReason", RejectedReason);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pArea", Area);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGoogleMapAddress", GoogleMapAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pLatLongOfTheAddress", LatLongOfTheAddress);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pCustomerOptedForInsurance", CustomerOptedForInsurance);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNomineeName", NomineeName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRelationshipWithNominee", RelationshipWithNominee);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pNomineeGender", NomineeGender);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pNomineeDOB", NomineeDOB);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", BranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApprovedPaymentFrequency", pApprovedPaymentFrequency);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", pSRC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 150, "@pErrDesc", pErrDesc);
                DBUtility.Execute(oCmd);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region PNBRequestResponse
        public string SavePNBRequestResponse(RequestHeader requestHeader, PNBRequestResponseData requestBody)
        {
            string vUpdateStatus = "Successfully Not Updated";
            AccountInformation AC = requestBody.AccountInformation;
            PAN PAN = requestBody.PAN;
            AadharVoter AV = requestBody.AadharVoter;
            AadharDL AD = requestBody.AadharDL;
            BankAccount BA = requestBody.BankAccount;
            Insurance I = requestBody.Insurance;
            eNACH E = requestBody.eNACH;
            Int32 vErr = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
                {
                    vUpdateStatus = "Credential Fields are Blank..";
                }
                else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    vUpdateStatus = "User Id Not found..";
                }
                else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    vUpdateStatus = "Password Not Matched..";
                }
                else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB")
                {
                    vUpdateStatus = "Partner ID Not Matched..";
                }
                else
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "SavePNBRequestResponse";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanAppNo", AC.LoanAppNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, PAN.PANOCRRequest.Length, "@pPANOCRRequest", PAN.PANOCRRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, PAN.PANOCRResponse.Length, "@pPANOCRResponse", PAN.PANOCRResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, PAN.PANAuthenticationRequest.Length, "@pPANAuthenticationRequest", PAN.PANAuthenticationRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, PAN.PANAuthenticationResponse.Length, "@pPANAuthenticationResponse", PAN.PANAuthenticationResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.AadharOCRRequest.Length, "@pAVAadharOCRRequest", AV.AadharOCRRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.AadharOCRResponse.Length, "@pAVAadharOCRResponse", AV.AadharOCRResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.VoterIDOCRRequest.Length, "@pVoterIDOCRRequest", AV.VoterIDOCRRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.VoterIDOCTResponse.Length, "@pVoterIDOCTResponse", AV.VoterIDOCTResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.VoterIDAuthenticationRequest.Length, "@pVoterIDAuthenticationRequest", AV.VoterIDAuthenticationRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AV.VoterIDAuthenticationResponse.Length, "@pVoterIDAuthenticationResponse", AV.VoterIDAuthenticationResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AD.AadharOCRRequest.Length, "@pADAadharOCRRequest", AD.AadharOCRRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AD.AadharOCRResponse.Length, "@pADAadharOCRResponse", AD.AadharOCRResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AD.DLOCRRequest.Length, "@pDLOCRRequest", AD.DLOCRRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AD.DLAuthenticationRequest.Length, "@pDLAuthenticationRequest", AD.DLAuthenticationRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, AD.DLAuthenticationResponse.Length, "@pDLAuthenticationResponse", AD.DLAuthenticationResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, BA.AccountValidationRequest.Length, "@pAccountValidationRequest", BA.AccountValidationRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, BA.AccountValidationResponse.Length, "@pAccountValidationResponse", BA.AccountValidationResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, I.InsuranceRequest.Length, "@pInsuranceRequest", I.InsuranceRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, I.InsuranceResponse.Length, "@pInsuranceResponse", I.InsuranceResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, E.nuPayRequest.Length, "@pnuPayRequest", E.nuPayRequest);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, E.nuPayResponse.Length, "@pnuPayResponse", E.nuPayResponse);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                    DBUtility.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                        vUpdateStatus = "Successfully Updated";
                    else if (vErr == 2)
                        vUpdateStatus = "Invalid Loan Application Number";
                    else
                        vUpdateStatus = "Successfully Not Updated";
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
            return vUpdateStatus;
        }
        #endregion

        #region SaveFinalSanction
        public Int32 SaveFinalSanction(string pLnSancId, string pFinSancStatus, string pFinAppBy, DateTime pFinalAppDate, string pFinAppRemark, Int32 pErr)
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
        #endregion

        #region InsertLoanMstNew
        public Int32 InsertLoanMstNew(ref string pLoanNo, string pSanctionId, string pLoanAppID, string pCustID, Int32 pLoanTypeId, int pDisbSrl,
          DateTime pDishbDate, decimal pTotLnAmt, string pInstType, string pSchedule, decimal pInstRate, decimal pFInstRate, int pInstNo, decimal pInstPeriod,
          decimal pEMI, decimal pInstallSize, DateTime pStDate, string pDishbMode, Int32 pCycle, Int32 pFunderID, decimal pProcFees,
          decimal pInsuAmt, decimal pInsuSTax, decimal pInsuCGST, decimal pInsuSGST, decimal pAppCharge,
          decimal pAdvEMIPric, decimal pAdvEMIInt, decimal pStampCharge, decimal pAdvInterest,
          decimal pNetDisbAmt, string pReffLedgerAC, string pTransMode, string pReffNo, DateTime pReffDate, string pBranch,
          string pTblMst, string pTblDtl, string pFinYear, Int32 pCreatedBy, string pNarationL,
          Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pCollType, decimal pPreLnBal, string pPreLnBalLedAC, decimal pCGSTAmt,
          decimal pSGSTAmt, decimal pIGSTAmt, decimal pFLDGAmt, string pPreLnId, decimal pTotAmt,
          string pisTransDisburse, string pTrnsDisbAc, decimal pDisburseAmt, string pCollMode, decimal pBrkPrdInt, string pLogBrCode,
           decimal pPreLnInt, ref string pErrDesc, decimal pBrkPrdIntAct, decimal pBrkPrdIntWave,
           decimal vPropInsuAmt, decimal vPropInsuCGSTAmt, decimal vPropInsuSGSTAmt, decimal vAdminFees, decimal vTechFees,
           decimal vInsuIGSTAmt, decimal vPropInsuIGSTAmt,
           decimal pCERSAICharge, decimal pCERSAIChargeCGST, decimal pCERSAIChargeSGST, decimal pCERSAIChargeIGST
           , string pNetOffYN, string pNetOffLoanId, decimal pNetOffAdvanceAmt, decimal pNetOffPrincAmt, decimal pNetOffIntAmt
           )
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLoanMstNew";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppID", pLoanAppID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCustID", pCustID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", pLoanTypeId);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDisbSrl", pDisbSrl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDishbDate", pDishbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotLnAmt", pTotLnAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pInstType", pInstType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pSchedule", pSchedule);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstRate", pInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFInstRate", pFInstRate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstNo", pInstNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pInstPeriod", pInstPeriod);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMI", pEMI);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInstallSize", pInstallSize);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pStDate", pStDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pDishbMode", pDishbMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCycle", pCycle);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pFunderID", pFunderID);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pProcFees", pProcFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuAmt", pInsuAmt);



                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSTax", pInsuSTax);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuCGST", pInsuCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuSGST", pInsuSGST);



                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSTax", pLPFSTax);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFKKTax", pLPFKKTax);
                //DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pLPFSBTax", pLPFSBTax);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppCharge", pAppCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIPric", pAdvEMIPric);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvEMIInt", pAdvEMIInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pStampCharge", pStampCharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdvInterest", pAdvInterest);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pNetDisbAmt", pNetDisbAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pReffLedgerAC", pReffLedgerAC);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTransMode", pTransMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pReffNo", pReffNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pReffDate", pReffDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 4, "@pBranch", pBranch);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNarationL", pNarationL);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", pCollDay);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", pCollDayNo);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanType", pLoanType);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", pCollType);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnBal", pPreLnBal);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pPreLnBalLedAC", pPreLnBalLedAC);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCGSTAmt", pCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSGSTAmt", pSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pIGSTAmt", pIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pFLDGAmt", pFLDGAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pPreLnId", pPreLnId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTotCharge", pTotAmt);


                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pisTransDisburse", pisTransDisburse);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pTrnsDisbAc", pTrnsDisbAc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDisburseAmt", pDisburseAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", pCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBrkPrdInt", pBrkPrdInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pLogBrCode", pLogBrCode);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPreLnInt", pPreLnInt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", pErrDesc);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pBrkPrdIntAct", pBrkPrdIntAct);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pBrkPrdIntWave", pBrkPrdIntWave);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuAmt", vPropInsuAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuCGSTAmt", vPropInsuCGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuSGSTAmt", vPropInsuSGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAdminFees", vAdminFees);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pTechFees", vTechFees);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pInsuIGSTAmt", vInsuIGSTAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pPropInsuIGSTAmt", vPropInsuIGSTAmt);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAICharge", pCERSAICharge);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeCGST", pCERSAIChargeCGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeSGST", pCERSAIChargeSGST);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pCERSAIChargeIGST", pCERSAIChargeIGST);

                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pNetOffYN", pNetOffYN);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pNetOffLoanId", pNetOffLoanId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffAdvanceAmt", pNetOffAdvanceAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffPrincAmt", pNetOffPrincAmt);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 8, "@pNetOffIntAmt", pNetOffIntAmt);

                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                pLoanNo = Convert.ToString(oCmd.Parameters["@pLoanNo"].Value);
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

        #region FinYear

        internal DataTable GetFinYearList(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllFinYear";
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

        internal DataTable GetFinYearAll(string fYear)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinYearByYear";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 9, "@pYear", fYear);
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

        #region PNBDigitalDocResponse
        public Int32 SavePNBDigitalDocResponse(string pLoanAppId, string pResponseData)
        {
            SqlCommand oCmd = new SqlCommand();
            int vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SavePNBDigitalDocResponse";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", pLoanAppId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region PaynearBy Update Bank Details
        public string UpdateBankDetails(PostCredential vPostCredential, PostBankDataSet vPostBankDataSet)
        {
            string vUpdateStatus = "Successfully Not Updated";

            string vLoanAppNo = vPostBankDataSet.LoanAppNo;
            string vAcHolderName = vPostBankDataSet.AcHolderName;
            string vAccountNo = vPostBankDataSet.AccountNo;
            string vBankName = vPostBankDataSet.BankName;
            string vIfscCode = vPostBankDataSet.IfscCode;
            string vAccountType = vPostBankDataSet.AccountType.Trim();
            Int32 vAccountTypeId = vAccountType == "Saving Account" ? 1 : vAccountType == "Current Account" ? 2 : vAccountType == "Joint Saving Account" ? 3 : vAccountType == "Overdraft Account" ? 4 : 5;
            Int32 vYrOfOpening = Convert.ToInt32(vPostBankDataSet.YrOfOpening);

            Int32 vErr = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                if (vPostCredential.UserID.Trim() == "" || vPostCredential.Password.Trim() == "" || vPostCredential.PartnerID.Trim() == "")
                {
                    vUpdateStatus = "Credential Fields are Blank..";
                }
                else if (vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && vPostCredential.UserID.Trim().ToUpper() != "CENTRUMSME")
                {
                    vUpdateStatus = "User Id Not found..";
                }
                else if (vPostCredential.Password.Trim().ToUpper() != "ABCD*1234")
                {
                    vUpdateStatus = "Password Not Matched..";
                }
                else if (vPostCredential.PartnerID.Trim().ToUpper() != "PNB")
                {
                    vUpdateStatus = "Partner ID Not Matched..";
                }
                else
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "UpdateBankDetails";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanAppId", vLoanAppNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pACHolderName", vAcHolderName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pACNo", vAccountNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBankName", vBankName);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIFSCCode", vIfscCode);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAccountType", vAccountTypeId);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrOfOpening", vYrOfOpening);

                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                    DBUtility.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                        vUpdateStatus = "Successfully Updated";
                    else if (vErr == 2)
                        vUpdateStatus = "Invalid Loan Application Number";
                    else
                        vUpdateStatus = "Successfully Not Updated";
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
            return vUpdateStatus;
        }
        #endregion

        #region FundFinaDouble Loan
        public string FindFinaDoubleLoan(PostFundFinaDoubleLoan vPostFundFinaDoubleLoan)
        {
            string vPurposeOfLoan = vPostFundFinaDoubleLoan.PurposeOfLoan
            , vPartnerID = vPostFundFinaDoubleLoan.PartnerID
            , vFullName = vPostFundFinaDoubleLoan.FullName
            , vMemberId = vPostFundFinaDoubleLoan.MemberId
            , vDate = vPostFundFinaDoubleLoan.Date
            , vCMLApprovedLoanAmount = vPostFundFinaDoubleLoan.CMLApprovedLoanAmount
            , vCMLApprovedTenure = vPostFundFinaDoubleLoan.CMLApprovedTenure
            , vEMI = vPostFundFinaDoubleLoan.EMI
            , vInsurancePremiumAmount = vPostFundFinaDoubleLoan.InsurancePremiumAmount
            , vProcessingFees = vPostFundFinaDoubleLoan.ProcessingFees
            , vApprovedPaymentFrequency = vPostFundFinaDoubleLoan.ApprovedPaymentFrequency
            , vROI = vPostFundFinaDoubleLoan.ROI
            , vEstimatedCollectionDate = vPostFundFinaDoubleLoan.EstimatedCollectionDate
            , vCustomerOptedForInsurance = vPostFundFinaDoubleLoan.CustomerOptedForInsurance
            , vEstimatedDisbursementDate = vPostFundFinaDoubleLoan.EstimatedDisbursementDate;

            string ACVouMst = null;
            string ACVouDtl = null;
            string vFinYr = "", pShortYear = "", vFinYrNo = "", pStatusDesc = "";
            int vErr2;
            decimal vNetDisbAmt = 0, vTotalCharge = 0, vProcPer = 0, vProcFee = 0, vProcFeeCGST = 0, vProcFeeSGST = 0, vProcFeeCGSTPer = 0, vProcFeeSGSTPer = 0;

            DataSet ds1 = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2, dt3 = null;
            string vLoanAppNo = "", vBrCode = "0000";

            string vPurposeID = GetPurposeIdByPurposeName(vPurposeOfLoan);
            string vLoanTypeId = vPartnerID == "PNB" ? "3" : "4";
            //Save Loan Application
            string vLoanApplication = SaveLoanApplication(vMemberId, vDate, vPurposeID, vLoanTypeId, vCMLApprovedLoanAmount, vCMLApprovedTenure, "N", vBrCode, "1", "I", "0", "-1", "", "Y", vDate, "", vPartnerID);
            string[] arr4 = vLoanApplication.Split('#');
            if (arr4[0] == "Success:Record Saved Successfully")
            {
                vLoanAppNo = arr4[1];//Loan Application Id
                //Save PD
                string vPd = SavePDMst("0", vLoanAppNo, vMemberId, "-1", "", vDate, "", "0", "0", "0", "0", vBrCode, "1", "", "", vPartnerID);
                if (vPd == "Record Saved Successfully")
                {
                    int vErr = SaveIIRRatio(vLoanAppNo, 0, 0, 0, 0, 0, Convert.ToDecimal(vEMI), "IIR", 60, Convert.ToInt32(vCMLApprovedTenure), 0, 1);
                    if (vErr > 0)
                    {
                        Int32 vErr1 = SavePDFinalApprove(vLoanAppNo, "A", DateTime.Now, "", 1, "", "", "", "", "", "", Convert.ToDecimal(vCMLApprovedLoanAmount));
                        if (vErr1 > 0)
                        {
                            string vSanctionId = GetSanctionId(vLoanAppNo);
                            vTotalCharge = (Convert.ToDecimal(vInsurancePremiumAmount == "" ? "0" : vInsurancePremiumAmount) + Convert.ToDecimal(vProcessingFees == "" ? "0" : vProcessingFees));
                            vNetDisbAmt = Convert.ToDecimal(vCMLApprovedLoanAmount) - vTotalCharge;
                            if (vPartnerID == "FDF")
                            {
                                vProcPer = (Convert.ToDecimal(vProcessingFees == "" ? "0" : vProcessingFees) / Convert.ToDecimal(vCMLApprovedLoanAmount)) * 100;
                                vProcFee = Convert.ToDecimal(vProcessingFees == "" ? "0" : vProcessingFees);
                            }
                            else
                            {
                                vProcFee = (Convert.ToDecimal(vCMLApprovedLoanAmount) * 3) / 100;
                                vProcFeeCGST = (Convert.ToDecimal(vProcessingFees == "" ? "0" : vProcessingFees) - vProcFee) / 2;
                                vProcFeeSGST = (Convert.ToDecimal(vProcessingFees == "" ? "0" : vProcessingFees) - vProcFee) / 2;
                                vProcFeeCGSTPer = 9;
                                vProcFeeSGSTPer = 9;
                                vProcPer = 3;
                            }
                            string vRepayType = vApprovedPaymentFrequency == null ? "M" : vApprovedPaymentFrequency == "" ? "M" : vApprovedPaymentFrequency;

                            Int32 vErr4 = SaveSanction(ref vSanctionId, vSanctionId, vLoanAppNo, vMemberId, DateTime.Now, Convert.ToDecimal(vCMLApprovedLoanAmount), Convert.ToInt32(vLoanTypeId), 0,
                                Convert.ToDecimal(vROI), Convert.ToInt32(vCMLApprovedTenure), Convert.ToInt32(vCMLApprovedTenure), "R"
                                , vRepayType, Convert.ToDecimal(vEMI), vProcFee, 0, 0, 0,
                                DateTime.Now, setDate(vEstimatedCollectionDate), "N", vNetDisbAmt, "N", "P", DateTime.Now, "Admin", "", vBrCode, 0, 0, 0, 0, vProcPer, 0, 0,
                                0, 0, 0, vTotalCharge, 1, "Edit", 0, DateTime.Now, 0, 0, "", "", "", vProcFeeCGSTPer, vProcFeeCGST, vProcFeeSGSTPer, vProcFeeSGST, 0, 0, "", "",
                                0, 0, 0, Convert.ToDecimal(vInsurancePremiumAmount), 0, 0, 0, 0, 0, 0, "", "", "", 0, "", 0, 0, 0, 0, vCustomerOptedForInsurance == "Y" ? 2 : 0);
                            if (vErr4 == 0)
                            {
                                int vErr5 = SaveFinalSanction(vSanctionId, "S", "Admin", DateTime.Now, "", 0);
                                {
                                    if (vErr5 == 0)
                                    {
                                        if (vPartnerID == "FDF")
                                        {
                                            string vLoanNo = "", vErrDesc = "";
                                            dt2 = new DataTable();
                                            dt3 = new DataTable();
                                            dt2 = GetFinYearList(vBrCode);
                                            if (dt2.Rows.Count > 0)
                                            {
                                                vFinYrNo = getFinYrNo(dt2.Rows[0]["YrNo"].ToString());
                                                vFinYr = dt2.Rows[0]["FYear"].ToString();
                                                ACVouMst = "ACVouMst" + vFinYrNo;
                                                ACVouDtl = "ACVouDtl" + vFinYrNo;
                                                dt3 = GetFinYearAll(vFinYr);
                                                pShortYear = dt3.Rows[0]["ShortYear"].ToString();
                                            }
                                            int vErr6 = InsertLoanMstNew(ref vLoanNo, vSanctionId, vLoanAppNo, vMemberId, Convert.ToInt32(vLoanTypeId), 1, setDate(vEstimatedDisbursementDate), Convert.ToDecimal(vCMLApprovedLoanAmount), "R",
                                                vApprovedPaymentFrequency, Convert.ToDecimal(vROI), 0, Convert.ToInt32(vCMLApprovedTenure), 12, Convert.ToDecimal(vEMI),
                                                Convert.ToInt32(vCMLApprovedTenure), setDate(vEstimatedCollectionDate), "N", 1, 1, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToDecimal(vCMLApprovedLoanAmount),
                                                "G0674", "B", "", DateTime.Now, vBrCode, ACVouMst, ACVouDtl, pShortYear, 1, "Being the Amt of Loan Disbursed for " + vFullName, 0, 0, "", 0, 0, "", 0, 0, 0, 0, "",
                                                Convert.ToDecimal(vCMLApprovedLoanAmount), "", "", Convert.ToDecimal(vCMLApprovedLoanAmount), "", 0, vBrCode, 0, ref vErrDesc, 0, 0, Convert.ToDecimal(vInsurancePremiumAmount), 0,
                                                0, 0, 0, 0, 0, 0, 0, 0, 0, "N", "", 0, 0, 0); //G0674--Fund Fina Disbursement Account
                                            if (vErr6 == 0)
                                            {
                                                pStatusDesc = "Success";
                                            }
                                            else
                                            {
                                                pStatusDesc = "Failed:Problem in Disbursement";
                                            }
                                        }
                                        else
                                        {
                                            pStatusDesc = "Success";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                pStatusDesc = "Success";
                            }
                        }
                    }
                }
                else
                {
                    pStatusDesc = "Failed:Problem in PD Data Save";
                }
            }
            else
            {
                pStatusDesc = "Failed:Problem in Loan Application Data Save";
            }

            return pStatusDesc;
        }
        #endregion

        #region chkDdupMEL

        public Int32 chkDdupMEL(string pPANCardNumber, string pAadhaarDocumentNumber, string pPOANumber, string pBranchCode, ref string pErrMsg)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "chkDdupMEL";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPANCardNumber", pPANCardNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarDocumentNumber", pAadhaarDocumentNumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pPOANumber", pPOANumber);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 66;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region FundFina Loan Disbursement
        public string FundFinaLoanDisbursement(RequestHeader requestHeader, PostDisbursement requestBody)
        {
            string ACVouMst = null;
            string ACVouDtl = null;
            string vFinYr = "", pShortYear = "", vFinYrNo = "", vSanctionId = "";
            string pStatusDesc = "Failed:", vBrCode = "0000";
            DataTable dt = new DataTable();
            DataTable dt2, dt3 = null;
            int vErr6 = 1;

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else
            {
                if (string.IsNullOrEmpty(requestBody.DisburesmentDate))
                {
                    pStatusDesc = "Failed:Disbursement date should not be left blank ..";
                }
                else if (string.IsNullOrEmpty(requestBody.CollectionStartDate))
                {
                    pStatusDesc = "Failed:Collection start date should not be left blank ..";
                }
                else
                {
                    try
                    {
                        try
                        {
                            string folderPath = HostingEnvironment.MapPath(string.Format("~/{0}/{1}", "Files", "Disbursement")); ;
                            System.IO.Directory.CreateDirectory(folderPath);
                            File.WriteAllText(folderPath + "/" + requestBody.LoanAppNo + ".text", requestBody.LoanAppNo + "," + requestBody.DisburesmentDate + "," + requestBody.CollectionStartDate);
                        }
                        finally
                        {
                        }
                        string vLoanNo = "", vErrDesc = "", vLoanAppNo = "";
                        vLoanAppNo = requestBody.LoanAppNo;
                        vSanctionId = GetSanctionId(vLoanAppNo);
                        Int32 vErr = UpdateDisbDate(vSanctionId, setDate(requestBody.DisburesmentDate), setDate(requestBody.CollectionStartDate));
                        if (vErr == 0)
                        {
                            dt2 = new DataTable();
                            dt3 = new DataTable();
                            dt2 = GetFinYearList(vBrCode);
                            if (dt2.Rows.Count > 0)
                            {
                                vFinYrNo = getFinYrNo(dt2.Rows[0]["YrNo"].ToString());
                                vFinYr = dt2.Rows[0]["FYear"].ToString();
                                ACVouMst = "ACVouMst" + vFinYrNo;
                                ACVouDtl = "ACVouDtl" + vFinYrNo;
                                dt3 = GetFinYearAll(vFinYr);
                                pShortYear = dt3.Rows[0]["ShortYear"].ToString();
                            }

                            string vMemberId = "", vLoanTypeId = "", vEstimatedDisbursementDate = "",
                                vCMLApprovedLoanAmount = "", vApprovedPaymentFrequency = "",
                                vCMLApprovedTenure = "", vROI = "", vEMI = "", vEstimatedCollectionDate = "",
                                vInsurancePremiumAmount = "", vFullName = "";
                            dt = new DataTable();
                            dt = GetDisbData(vLoanAppNo);
                            if (dt.Rows.Count > 0)
                            {
                                vMemberId = dt.Rows[0]["CustId"].ToString();
                                vLoanTypeId = dt.Rows[0]["LoanTypeId"].ToString();
                                vEstimatedDisbursementDate = dt.Rows[0]["DisbDate"].ToString();
                                vCMLApprovedLoanAmount = dt.Rows[0]["SanctionAmt"].ToString();
                                vApprovedPaymentFrequency = dt.Rows[0]["RepayType"].ToString();
                                vCMLApprovedTenure = dt.Rows[0]["Tenure"].ToString();
                                vROI = dt.Rows[0]["RIntRate"].ToString();
                                vEMI = dt.Rows[0]["EMIAmt"].ToString();
                                vEstimatedCollectionDate = dt.Rows[0]["RepayStartDate"].ToString();
                                vInsurancePremiumAmount = dt.Rows[0]["PropertyInsAmt"].ToString();
                                vFullName = dt.Rows[0]["CompanyName"].ToString();

                                vErr6 = InsertLoanMstNew(ref vLoanNo, vSanctionId, vLoanAppNo, vMemberId, Convert.ToInt32(vLoanTypeId), 1, setDate(vEstimatedDisbursementDate), Convert.ToDecimal(vCMLApprovedLoanAmount), "R",
                                vApprovedPaymentFrequency, Convert.ToDecimal(vROI), 0, Convert.ToInt32(vCMLApprovedTenure), 12, Convert.ToDecimal(vEMI),
                                Convert.ToInt32(vCMLApprovedTenure), setDate(vEstimatedCollectionDate), "N", 1, 1, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToDecimal(vCMLApprovedLoanAmount),
                                "G1126", "B", "", DateTime.Now, vBrCode, ACVouMst, ACVouDtl, pShortYear, 1, "Being the Amt of Loan Disbursed for " + vFullName, 0, 0, "", 0, 0, "", 0, 0, 0, 0, "",
                                Convert.ToDecimal(vCMLApprovedLoanAmount), "", "", Convert.ToDecimal(vCMLApprovedLoanAmount), "", 0, vBrCode, 0, ref vErrDesc, 0, 0, Convert.ToDecimal(vInsurancePremiumAmount), 0,
                                0, 0, 0, 0, 0, 0, 0, 0, 0, "N", "", 0, 0, 0); //G0674--Fund Fina Disbursement Account

                                if (vErr6 == 0)
                                {
                                    pStatusDesc = "Success:Loan Successfully Disbursed.";
                                }
                                else
                                {
                                    pStatusDesc = "Failed:Problem in Disbursement.";
                                }
                            }
                            else
                            {
                                pStatusDesc = "Failed:Problem in Disbursement Data or Loan Already Disbursed.";
                            }
                        }
                        else if (vErr == 2)
                        {
                            pStatusDesc = "Failed:Invalid Loan Application Number.";
                        }
                        else if (vErr == 3)
                        {
                            pStatusDesc = "Failed:Day End Done so you can not disbursed loan for today..";
                        }
                        else if (vErr == 4)
                        {
                            pStatusDesc = "Failed:Day Begin not Done so you can not disbursed loan for today..";
                        }
                        else
                        {
                            pStatusDesc = "Failed:Loan Already Disbursed.";
                        }
                    }
                    catch (Exception e)
                    {
                        pStatusDesc = "Failed:" + e.ToString();
                    }
                }

            }
            return pStatusDesc;
        }
        #endregion

        #region DisbData

        public DataTable GetDisbData(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetDisbData";
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

        public Int32 UpdateDisbDate(string pSanctionId, DateTime pDisbDate, DateTime pCollStartDate)
        {
            Int32 pErr = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateDisbDate";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pSanctionId", pSanctionId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDisbDate", pDisbDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCollStartDate", pCollStartDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 8, "@pErr", pErr);
                DBUtility.Execute(oCmd);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return pErr;
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

        #region FundFina Cancel LoanApplication
        public string FundFinaCancelLoanApplication(RequestHeader requestHeader, PostDisbursement requestBody)
        {
            string pStatusDesc = "Failed:";

            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else
            {
                string vLoanAppNo = "";
                vLoanAppNo = requestBody.LoanAppNo;
                SqlCommand oCmd = new SqlCommand();
                try
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "CancelLoanApplication";
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanAppId", vLoanAppNo);
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pSRC", requestHeader.PartnerID.Trim().ToUpper());
                    DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                    DBUtility.Execute(oCmd);
                    pStatusDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                }
                catch (Exception ex)
                {
                    pStatusDesc = ex.ToString();
                }
                finally
                {
                    oCmd.Dispose();
                }
            }
            return pStatusDesc;
        }
        #endregion

        #region FundFinaCollection
        public Int32 InsertBulkCollectionFundFina(DateTime pAccDate, string pTblMst, string pTblDtl, string pFinYear,
        string pBankLedgr, string pCollXml, string pBrachCode, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vCollMode = pBankLedgr == "C0001" ? "C" : "B";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBulkCollectionFundFina";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", pAccDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCollXml.Length + 1, "@pXml", pCollXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBrachCode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", pTblMst);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", pTblDtl);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pModeAC", pBankLedgr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", "Being the Amt of Loan Collection from ");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", vCollMode);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@PCollectionMode", 'M');
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

        private string GetXml(DataTable dt)
        {
            string vXmlData = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlData = oSW.ToString();
            }
            return vXmlData;
        }

        public string chkFundFinaColl(DateTime pAccDate, string pCollXml)
        {
            SqlCommand oCmd = new SqlCommand();
            string vErrMsg = "";

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "chkFundFinaColl";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pAccDate);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCollXml.Length + 1, "@pXml", pCollXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 5000, "@pErrMsg", "");
                DBUtility.Execute(oCmd);
                vErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                return vErrMsg;
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

        public string FundFinaCollection(RequestHeader requestHeader, PostFindFinaCollection vPostFindFinaCollection)
        {
            string pStatusDesc = "";
            if (requestHeader.UserID.Trim() == "" || requestHeader.Password.Trim() == "" || requestHeader.PartnerID.Trim() == "")
            {
                pStatusDesc = "Failed:Credential Fields are Blank..";
                //pEqXml = "Credential Fields are Blank..";
            }
            else if (requestHeader.UserID.Trim().ToUpper() != "CENTRUMSMEUAT" && requestHeader.UserID.Trim().ToUpper() != "CENTRUMSME")
            {
                pStatusDesc = "Failed:User Id Not found..";
                //pEqXml = "User Id Not found..";
            }
            else if (requestHeader.Password.Trim().ToUpper() != "ABCD*1234")
            {
                pStatusDesc = "Failed:Password Not Matched..";
                //pEqXml = "Password Not Matched..";
            }
            else if (requestHeader.PartnerID.Trim().ToUpper() != "PNB" && requestHeader.PartnerID.Trim().ToUpper() != "FDF")
            {
                pStatusDesc = "Failed:Partner ID Not Matched..";
                //pEqXml = "Partner ID Not Matched..";
            }
            else
            {
                try
                {
                    DataTable dt1, dt2, dt3;
                    dt2 = new DataTable();
                    dt3 = new DataTable();
                    string vACVouMst = null;
                    string vACVouDtl = null;
                    string vFinYr = "", pShortYear = "", vFinYrNo = "", pXml = "";
                    string[] vDt = vPostFindFinaCollection.SettlementDate.Split('-');
                    DateTime vDate = new DateTime(Convert.ToInt32(vDt[2]), Convert.ToInt32(vDt[1]), Convert.ToInt32(vDt[0]));

                    dt2 = GetFinYearList("0000");
                    if (dt2.Rows.Count > 0)
                    {
                        vFinYrNo = getFinYrNo(dt2.Rows[0]["YrNo"].ToString());
                        vFinYr = dt2.Rows[0]["FYear"].ToString();
                        vACVouMst = "ACVouMst" + vFinYrNo;
                        vACVouDtl = "ACVouDtl" + vFinYrNo;
                        dt3 = GetFinYearAll(vFinYr);
                        pShortYear = dt3.Rows[0]["ShortYear"].ToString();
                    }

                    //----------------------------Table Creation---------------------------
                    dt1 = new DataTable();
                    dt1.Columns.Add("ReferenceNo", typeof(string));
                    dt1.Columns.Add("SettlementDate", typeof(string));
                    dt1.Columns.Add("Amount", typeof(string));
                    dt1.Columns.Add("InterestAmountRecovered", typeof(string));
                    dt1.Columns.Add("PrincipalAmountRecovered", typeof(string));
                    dt1.TableName = "Collection";
                    dt1.Rows.Add(vPostFindFinaCollection.ReferenceNo, vPostFindFinaCollection.SettlementDate,
                        vPostFindFinaCollection.Amount, vPostFindFinaCollection.InterestAmountRecovered, vPostFindFinaCollection.PrincipalAmountRecovered);
                    // -----------------------------------------------------------------------
                    pXml = GetXml(dt1);
                    string vErrMsg = chkFundFinaColl(vDate, pXml);
                    if (vErrMsg.Trim() == "")
                    {
                        int vErr = InsertBulkCollectionFundFina(vDate, vACVouMst, vACVouDtl, pShortYear, "-1", pXml, "0000", 1);
                        if (vErr == 0)
                        {
                            pStatusDesc = "Success:Collection successfully uploaded.";
                        }
                        else
                        {
                            pStatusDesc = "Failed:Collection was not uploaded successfully.";
                        }
                    }
                    else
                    {
                        pStatusDesc = "Failed:" + vErrMsg.Trim();
                    }
                }
                catch (Exception ex)
                {
                    pStatusDesc = "Failed:Collection successfully not uploaded.";
                }
            }
            return pStatusDesc;
        }
        #endregion

        #region InsertBulkCollection
        public Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy)
        {
            int vErr = 0;
            CRepository oCR = new CRepository();
            try
            {
                vErr = oCR.InsertBulkCollection(pAccDate, pTblMst, pTblDtl, pFinYear, pBankLedgr, pCollXml, pBrachCode, pCreatedBy);

            }
            finally { }
            return vErr;
        }
        #endregion

        #region SaveOtherCollectionBulk
        public Int32 SaveOtherCollectionBulk(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml, string pCreatedBy)
        {
            int vErr = 0;
            CRepository oCR = new CRepository();
            try
            {
                vErr = oCR.SaveOtherCollectionBulk(pAccDate, pTblMst, pTblDtl, pFinYear, pBankLedgr, pCollXml, pCreatedBy);
            }
            finally { }
            return vErr;
        }
        #endregion

        #region KarzaVoterIDKYCValidation
        public KYCVoterIDResponse KarzaVoterIDKYCValidation(KYCVoterIDRequest vPostVoterData)
        {
            string vVoterID = vPostVoterData.epic_no;
            string vEoId = vPostVoterData.pEoID == null ? "" : vPostVoterData.pEoID;
            string vBranch = vPostVoterData.pBranch == null ? "" : vPostVoterData.pBranch;
            var req = new KYCVoterRequest()
            {
                consent = "Y",
                epic_no = vVoterID
            };
            string Requestdata = JsonConvert.SerializeObject(req);
            //string postURL = "https://testapi.karza.in/v2/voter";
            string postURL = "https://api.karza.in/v2/voter";
            try
            {
                int vErrCount = GetKarzaVoterErrCount(vVoterID);
                if (vErrCount < 3)
                {
                    HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                    if (request == null)
                    {
                        throw new NullReferenceException("request is not a http request");
                    }
                    // Set up the request properties.
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("x-karza-key", KarzaKey);
                    //request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
                    request.Host = "api.karza.in";
                    //request.Host = "testapi.karza.in";
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    string responsedata = string.Empty;
                    byte[] data = Encoding.UTF8.GetBytes(Requestdata);
                    request.ContentLength = data.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(data, 0, data.Length);
                    requestStream.Close();
                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        var API_Response = streamReader.ReadToEnd(); ;
                        responsedata = API_Response.ToString().Trim();
                    }
                    KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(responsedata.Replace("status-code", "status_code"));
                    try
                    {
                        responsedata = responsedata.Replace("\u0000", "");
                        responsedata = responsedata.Replace("\\u0000", "");
                        string vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                        SaveKarzaVoterVerifyData(vVoterID, vXml, vBranch, vEoId);//Save Response
                    }
                    finally
                    {
                        //---
                    }
                    string vErrMsg = string.Empty;
                    if (vResponseObj.status_code == "101")
                    {
                        vErrMsg = "101:Valid Authentication";
                    }
                    else if (vResponseObj.status_code == "102")
                    {
                        SaveKarzaVoterVerifyErrLog(vVoterID, vResponseObj.status_code);
                        vErrMsg = "102:Invalid ID number or combination of inputs";
                    }
                    else if (vResponseObj.status_code == "103")
                    {
                        SaveKarzaVoterVerifyErrLog(vVoterID, vResponseObj.status_code);
                        vErrMsg = "103:No records found for the given ID or combination of inputs";
                    }
                    else if (vResponseObj.status_code == "104")
                    {
                        vErrMsg = "104:Max retries exceeded";
                    }
                    else if (vResponseObj.status_code == "105")
                    {
                        vErrMsg = "105:Missing Consent";
                    }
                    else if (vResponseObj.status_code == "106")
                    {
                        vErrMsg = "106:Multiple Records Exist";
                    }
                    else if (vResponseObj.status_code == "107")
                    {
                        vErrMsg = "107:Not Supported";
                    }
                    else
                    {
                        vErrMsg = vResponseObj.status_code;
                    }
                    vResponseObj.status_code = vErrMsg;
                    return vResponseObj;
                }
                else
                {
                    KYCVoterIDResponse vResponseObj1 = new KYCVoterIDResponse();
                    vResponseObj1.request_id = "60ffe33e-b908-4e37-bb9c-680f132cefc5";
                    vResponseObj1.status_code = "901:Maximum retry Exceeded";
                    return vResponseObj1;
                }
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                Response = Response.Replace("status", "status_code");
                string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                SaveKarzaVoterVerifyData(vVoterID, vXml, vBranch, vEoId);
                Response.Replace("requestId", "request_id");
                KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(Response);

                if (Convert.ToString(vResponseObj.status_code) == "400")
                {
                    vResponseObj.status_code = "400:Bad Request";
                }
                else if (Convert.ToString(vResponseObj.status_code) == "401")
                {
                    vResponseObj.status_code = "401:Unauthorized Access";
                }
                else if (Convert.ToString(vResponseObj.status_code) == "402")
                {
                    vResponseObj.status_code = "402:Insufficient Credits";
                }
                else if (Convert.ToString(vResponseObj.status_code) == "500")
                {
                    vResponseObj.status_code = "500:Internal Server Error";
                }
                else if (Convert.ToString(vResponseObj.status_code) == "503")
                {
                    vResponseObj.status_code = "503:Source Unavailable";
                }
                else if (Convert.ToString(vResponseObj.status_code) == "504")
                {
                    vResponseObj.status_code = "504:Endpoint Request Timed Out";
                }
                else
                {
                    vResponseObj.status_code = Convert.ToString(vResponseObj.status_code);
                }
                return vResponseObj;
            }
            finally
            {
                // streamWriter = null;
            }
        }
        #endregion

        #region AadhaarXmlOTP
        public AadhaarOTPResponse KarzaAadhaarXmlOTP(AadhaarXmlOTP aadhaarXmlOtp)
        {
            string requestBody = JsonConvert.SerializeObject(aadhaarXmlOtp);
            //string postURL = "https://testapi.karza.in/v3/aadhaar-xml/otp";
            string postURL = "https://api.karza.in/v3/aadhaar-xml/otp";
            string fullResponse = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                //request.Headers.Add("x-karza-key", "wdycvLFD27R0RuAn2guz");//Test Key
                request.Headers.Add("x-karza-key", KarzaKey);//Live Key
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                try
                {
                    //SaveKarzaAadhaarOtp("AadhaarXmlOtp", fullResponse, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
                    SaveKarzaAadhaarOtp("AadhaarXmlOtp", fullResponse, "", "", aadhaarXmlOtp.aadhaarNo);
                }
                finally { }
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(fullResponse);
                return objResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    fullResponse = reader.ReadToEnd();
                }
                fullResponse = fullResponse.Replace("status", "statusCode");
                try
                {
                    // SaveKarzaAadhaarOtp("AadhaarXmlOtp", fullResponse, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
                    SaveKarzaAadhaarOtp("AadhaarXmlOtp", fullResponse, "", "", aadhaarXmlOtp.aadhaarNo);
                }
                finally { }
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(fullResponse);
                return objResponse;
            }
            finally
            {
            }
        }
        #endregion

        #region AadhaarXmlDownload
        public string KarzaAadhaarXml(AadhaarXmlDownload aadhaarXmlDownload)
        {
            string requestBody = JsonConvert.SerializeObject(aadhaarXmlDownload);
            // string postURL = "https://testapi.karza.in/v3/aadhaar-xml/file";
            string postURL = "https://api.karza.in/v3/aadhaar-xml/file";
            string fullResponse = "";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                //request.Headers.Add("x-karza-key", "wdycvLFD27R0RuAn2guz");
                request.Headers.Add("x-karza-key", KarzaKey);//Live Key
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                //SaveKarzaAadharVerifyData(aadhaarXmlDownload.aadhaarNo, fullResponse);
                try
                {
                    //SaveKarzaAadhaarOtp("AadhaarXml", fullResponse, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
                    SaveKarzaAadhaarOtp("AadhaarXml", fullResponse, "", "", aadhaarXmlDownload.aadhaarNo);
                }
                finally { }
                return fullResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    fullResponse = reader.ReadToEnd();
                }
                //SaveKarzaAadharVerifyData(aadhaarXmlDownload.aadhaarNo, fullResponse);
                try
                {
                    //SaveKarzaAadhaarOtp("AadhaarXml", fullResponse, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
                    SaveKarzaAadhaarOtp("AadhaarXml", fullResponse, "", "", aadhaarXmlDownload.aadhaarNo);
                }
                finally { }
                return fullResponse;
            }
            finally
            {
            }
        }
        #endregion

        #region eAadhaarOTP
        public AadhaarOTPResponse KarzaeAadhaarOTP(eAadhaarOTP eAadhaarOTP)
        {
            string requestBody = JsonConvert.SerializeObject(eAadhaarOTP);
            //string postURL = "https://testapi.karza.in/v3/eaadhaar/otp";
            string postURL = "https://api.karza.in/v3/eaadhaar/otp";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                //request.Headers.Add("x-karza-key", "wdycvLFD27R0RuAn2guz");
                request.Headers.Add("x-karza-key", KarzaKey);//Live Key
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                try
                {
                    // SaveKarzaAadhaarOtp("eAadhaarOtp", fullResponse, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
                    SaveKarzaAadhaarOtp("eAadhaarOtp", fullResponse, "", "", eAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(fullResponse);
                return objResponse;
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                Response = Response.Replace("status", "statusCode");
                try
                {
                    // SaveKarzaAadhaarOtp("eAadhaarOtp", Response, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
                    SaveKarzaAadhaarOtp("eAadhaarOtp", Response, "", "", eAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(Response);
                return objResponse;
            }
            finally
            {
            }
        }
        #endregion

        #region eAadhaarDownload
        public string eAadhaarDownload(eAadhaarDownload eAadhaarDownload)
        {
            string requestBody = JsonConvert.SerializeObject(eAadhaarDownload);
            // string postURL = "https://testapi.karza.in/v3/eaadhaar/file";
            string postURL = "https://api.karza.in/v3/eaadhaar/file";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                //request.Headers.Add("x-karza-key", "wdycvLFD27R0RuAn2guz");
                request.Headers.Add("x-karza-key", KarzaKey);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                // SaveKarzaAadharVerifyData(eAadhaarDownload.aadhaarNo, fullResponse);
                try
                {
                    // SaveKarzaAadhaarOtp("eAadhaarXml", fullResponse, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
                    SaveKarzaAadhaarOtp("eAadhaarXml", fullResponse, "", "", eAadhaarDownload.aadhaarNo);
                }
                finally { }
                return fullResponse;
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                //SaveKarzaAadharVerifyData(eAadhaarDownload.aadhaarNo, Response);
                try
                {
                    //SaveKarzaAadhaarOtp("eAadhaarXml", Response, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
                    SaveKarzaAadhaarOtp("eAadhaarXml", Response, "", "", eAadhaarDownload.aadhaarNo);
                }
                finally { }
                return Response;
            }
            finally
            {
            }
        }
        #endregion

        #region NameMatching
        public MatchResponse NameMatching(NameMatch vNameMatch)
        {
            var req = new NameMatchRequest()
            {
                name1 = vNameMatch.name1,
                name2 = vNameMatch.name2,
                type = "individual",
                preset = "l"
            };

            string requestBody = JsonConvert.SerializeObject(req);
            //string postURL = "https://testapi.karza.in/v3/name";
            string postURL = "https://api.karza.in/v3/name";

            string pBranch = vNameMatch.pBranch == null ? "" : vNameMatch.pBranch;
            string pIdNo = vNameMatch.pIdNo == null ? "" : vNameMatch.pIdNo;
            string pEoID = vNameMatch.pEoID == null ? "" : vNameMatch.pEoID;
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = "application/json";
                //Set header
                request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("x-karza-key", KarzaKey);
                //request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
                //Security
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                ///You must write ContentLength bytes to the request stream before calling [Begin]GetResponse.                
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                //return request.GetResponse() as HttpWebResponse;
                dynamic res = JsonConvert.DeserializeObject(fullResponse);

                try
                {
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);
                }
                finally
                {
                }

                if (res.statusCode == "101")
                {
                    return new MatchResponse(Math.Round(Convert.ToDouble(res.result.score) * 100, 2), Convert.ToBoolean(res.result.result), Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
                }
                else
                {
                    return new MatchResponse(0.00, false, Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
                }

            }
            catch (WebException ex)
            {
                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                Response = Response.Replace("\u0000", "");
                Response = Response.Replace("\\u0000", "");
                string vXml = AsString(JsonConvert.DeserializeXmlNode(Response.Replace("status", "statusCode"), "root"));
                SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);
                dynamic res = JsonConvert.DeserializeObject(Response.Replace("status", "statusCode"));
                return new MatchResponse(0.00, false, Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
            }
            finally
            {
                // streamWriter = null;
            }
        }
        #endregion

        #region Common
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

        public string GetSerializeJson(string vJson)
        {
            if (vJson != "")
            {
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

        #region SaveKarzaAadharVerifyData
        public string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseData)
        {
            string pResponseXml = "";
            pResponseData = pResponseData.Replace("\u0000", "");
            pResponseData = pResponseData.Replace("\\u0000", "");
            pResponseXml = AsString(JsonConvert.DeserializeXmlNode(pResponseData, "root"));
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

        #region Mob_ChangePassword
        public string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword)
        {
            //postMob_ChangePassword.pEncYN = postMob_ChangePassword.pEncYN == null ? "N" : postMob_ChangePassword.pEncYN;
            //#region AES Decrypt
            //if (postMob_ChangePassword.pEncYN == "Y")
            //{
            //    String key = "Force@2301***DB";
            //    var encryptedBytes = Convert.FromBase64String(postMob_ChangePassword.pPassword);
            //    postMob_ChangePassword.pPassword = Encoding.UTF8.GetString(AesDecrypt(encryptedBytes, GetRijndaelManaged(key)));
            //    var encryptedBytes1 = Convert.FromBase64String(postMob_ChangePassword.pOldPassword);
            //    postMob_ChangePassword.pOldPassword = Encoding.UTF8.GetString(AesDecrypt(encryptedBytes1, GetRijndaelManaged(key)));
            //}
            //#endregion

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
                    return "UserName and Password doesnot match.";
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

        #region GetIFSCDtl
        public List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<IFSCData> row = new List<IFSCData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetIfscDtlByIfsc";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pIfsCode", postIFSCData.pIFSCCode);
                DBUtility.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new IFSCData(rs["BANKNAME"].ToString(), rs["BRANCHNAME"].ToString()));
                    }
                }
                else
                {
                    row.Add(new IFSCData("No data available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new IFSCData("No data available", ex.ToString()));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
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

        #region SaveKarzaAadhaarOtp
        public Int32 SaveKarzaAadhaarOtp(string pApiName, string pResponseData, string pBranch, string pEoID, string pIdNo)
        {
            string pResponseXml = "";
            pResponseData = pResponseData.Replace("\u0000", "");
            pResponseData = pResponseData.Replace("\\u0000", "");
            pResponseXml = AsString(JsonConvert.DeserializeXmlNode(pResponseData, "root"));
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

        public AadhaarVaultResponse AadhaarVault(AadhaarVault AadhaarVault)
        {
            string postURL = vDBName.ToUpper() == "CENTRUM_SME" ? "https://avault.unitybank.co.in/vault/insert" : "https://avaultuat.unitybank.co.in/vault/insert";
            //string postURL = "https://avault.unitybank.co.in/vault/insert";
            string vAadhaarNo = Convert.ToString(AadhaarVault.refData);
            string vMaskedAadhaar = String.Format("{0}{1}", "********", vAadhaarNo.Substring(vAadhaarNo.Length - 4, 4));
            try
            {
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);
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
                SaveAadhaarVaultLog(vMaskedAadhaar, Convert.ToInt32(AadhaarVault.pCreatedBy), vResponseXml, AadhaarVault.pMobileNo);
                //-------------------------------------------------------------------------------------
                AadhaarVaultResponse myDeserializedClass = JsonConvert.DeserializeObject<AadhaarVaultResponse>(fullResponse);
                return myDeserializedClass;
            }
            catch (WebException ex)
            {
                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
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
        #endregion

        #region AadhaarVault Calling
        public string AadhaarVaultCalling(string pAadharNo, string pMobileNo, string pCreatedBy)
        {
            string vAadhaarRefNo = "";
            AadhaarVaultResponse vAadharVaultResponse = null;
            AadhaarVault vAadhaarVault = new AadhaarVault();
            vAadhaarVault.refData = pAadharNo;
            vAadhaarVault.refDataType = "U";
            vAadhaarVault.pMobileNo = pMobileNo;
            vAadhaarVault.pCreatedBy = pCreatedBy;
            vAadharVaultResponse = AadhaarVault(vAadhaarVault);
            try
            {
                vAadhaarRefNo = Convert.ToString(vAadharVaultResponse.results[0]);
            }
            catch(Exception ex)
            {
                vAadhaarRefNo = "";
            }
            return vAadhaarRefNo;
        }
        #endregion

        #region BankAccountVerification

        public static byte[] SHA256HASH(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedInputBytes = null;
            using (var hash = SHA256.Create())
            {
                hashedInputBytes = hash.ComputeHash(bytes);
            }
            return hashedInputBytes;
        }

        public FingPayResponse BankAcVerify(FingPayRequest req)
        {
            string vLoginId = "unity", vPin = "81dc9bdb52d04dc20036dbd8313ed055", vResponse = "", vMemberId = "";
            int vCreatedBy = 0;
            FingPayResponse vResp = null;
            ImpsBeneDetailsRequestDataModel DM = new ImpsBeneDetailsRequestDataModel();
            DM.beneAccNo = req.beneAccNo;
            DM.beneIFSC = req.beneIFSC;
            CRepository oRP = new CRepository();
            vMemberId = req.MemberId; vCreatedBy = Convert.ToInt32(req.CreatedBy);
            BankACReqData BankACReqData = new BankACReqData();
            BankACReqData.requestId = req.MemberId;
            BankACReqData.superMerchantLoginId = vLoginId;
            BankACReqData.superMerchantPin = vPin;
            List<ImpsBeneDetailsRequestDataModel> List = new List<ImpsBeneDetailsRequestDataModel>();
            List.Add(DM);
            BankACReqData.impsBeneDetailsRequestDataModel = List;

            string vRequestData = JsonConvert.SerializeObject(BankACReqData);
            string vRequestXml = AsString(JsonConvert.DeserializeXmlNode(vRequestData, "root"));
            string hash = vRequestData + '@' + vLoginId + '@' + vPin;
            String generatedHash = Convert.ToString(Convert.ToBase64String(SHA256HASH(hash)));
            string vTimeStamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            try
            {
                //string postURL = "https://fpanalytics.tapits.in/fpaepsanalytics/api/verification/bulk/bank";
                string postURL = "https://fpcorp.tapits.in/fpbeneverification/api/verification/bulk/bank";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("trnTimestamp", vTimeStamp.Replace('-', '/'));
                request.Headers.Add("hash", generatedHash);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(vRequestData);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                // vResponse = "{\"apiStatus\":true,\"apiStatusMessage\":\"successful\",\"data\":[{\"beneAccNo\":\"37637233950\",\"beneIfscCode\":\"SBIN0013111\",\"timestamp\":\"03/07/2023 18:27:27\",\"statusCode\":\"U01\",\"rrn\":\"318411635824\",\"beneName\":\"Mr  Kushal  Koley\",\"errorResponse\":\"Transaction Successful\",\"requestId\":\"1\",\"referrenceNo\":\"GV382660150307202318270027\"}],\"apiStatusCode\":0}";
                // vResponse = "{\"apiStatus\": false,\"apiStatusMessage\": \"Hash doesn't match.\", \"data\": null,\"apiStatusCode\": 10011}";
                //-----------------------------------DB--------------------------
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                int apiStatusCode = Convert.ToInt32(res.apiStatusCode);
                bool apiStatus = res.apiStatus;
                string apiStatusMessage = "Technical failure.";
                if (apiStatusCode == 0 && apiStatus == true)
                {
                    // apiStatusMessage = Convert.ToString(res.data[0].errorResponse);
                    string statusCode = Convert.ToString(res.data[0].statusCode);
                    if (statusCode.Contains("U"))
                    {
                        statusCode = statusCode.Replace("U", "5");
                    }
                    string beneName = Convert.ToString(res.data[0].beneName == null ? "" : res.data[0].beneName);
                    string referrenceNo = Convert.ToString(res.data[0].referrenceNo == null ? "" : res.data[0].referrenceNo);
                    string vMsg = BankErrMsg(Convert.ToInt32(statusCode));
                    vResp = new FingPayResponse(apiStatus, vMsg, Convert.ToInt32(statusCode), beneName, referrenceNo);
                }
                else
                {
                    vResp = new FingPayResponse(apiStatus, apiStatusMessage, apiStatusCode, "", "");
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oRP.SaveFingPayLog(vMemberId, vRequestXml, vResponseXml, vCreatedBy);
                //-------------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                vResp = new FingPayResponse(false, "Technical failure.", 500, "", "");
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oRP.SaveFingPayLog(vMemberId, vRequestXml, vResponseXml, vCreatedBy);
                //--------------------------------------------------------------------------------------
            }
            return vResp;
        }

        #endregion

        #region BankErrMsg
        private string BankErrMsg(Int32 vErrCode)
        {
            switch (vErrCode)
            {
                case 0:
                    return "Successful IMPS transaction, where beneficiary is credited in real time.";
                case 1:
                    return "Beneficiary account number is invalid.";
                case 2:
                    return "Exceeds the maximum permissible daily value of transactions.";
                case 3:
                    return "Beneficiary account is frozen.";
                case 4:
                    return "Beneficiary account is an NRE account.";
                case 5:
                    return "Beneficiary account is closed.";
                case 6:
                    return "NPCI rejects the transaction because the configured limit is exceeded for the remitter bank in the current settlement cycle.";
                case 7:
                    return "Transaction is not permitted to the particular account type of the beneficiary. e.g. FD Account etc.";
                case 8:
                    return "Transaction limit set by the beneficiary bank is exceeded.";
                case 9:
                    return "Transaction is not allowed to a non reloadable card. ";
                case 10:
                    return "When the IFSC & Bank ID is not configured at ICICI Bank. It can be because the member bank is not yet live at NPCI or it is in a negative list of IFSC.";
                case 11:
                    return "The transaction timed out at NPCI / Beneficiary Bank.";
                case 12:
                    return "Sent by NPCI when the beneficiary bank is not live in IMPS ecosystem.";
                case 13:
                    return "Declined by ICICI Bank API if the amount entered is invalid. It can happen if amount is negative, or < Re.1, or > 5lacs, or contains more than two decimal places.";
                case 14:
                    return "Declined by ICICI Bank API if a duplicate transaction is initated by the client.";
                case 15:
                    return "Rejected by the beneficiary bank if the beneficiary is a merchant.";
                case 16:
                    return "Declined by the Remitter bank when there is an error in the format of the packet sent by the client.";
                case 17:
                    return "The error code is populated by the remitter bank when the transaction is not found.";
                case 18:
                    return "NPCI / Beneficiary bank is not connected or is down.";
                case 19:
                    return "This scenario occours when transaction was declined with an RC which is not configured at the remitter bank.";
                case 20:
                    return "Beneficiary bank has declined the transaction with an invalid response code.";
                case 21:
                    return "This happens when the orginal request got timed out between the NPCI and the beneficiary bank. On verification, beneficiary bank has declined the transaction.";
                case 22:
                    return "Transaction declined by the remitter bank, which may be due to freeze in the remitter account or if there is an account level concurrency.";
                case 24:
                    return "Transaction is declined by NPCI if the transaction amount is greater than INR 5 lacs.";
                case 29:
                    return "This response is populated by NPCI for suspected fraud basis the risk score.";
                case 30:
                    return "The transaction timed-out at NPCI.";
                case 31:
                    return "The transaction got timedout at ICICI Bank CBS";
                case 32:
                    return "Inter-bank transaction timed-out at ICICI Bank CBS.";
                case 33:
                    return "Intra-bank transaction timed-out at ICICI Bank CBS.";
                case 34:
                    return "NPCI or the beneficiary bank declines the transaction if they suspect that there is a fraud.";
                case 35:
                    return "Technical decline by beneficiary bank due to transaction processing problem.";
                case 36:
                    return "Technical decline by beneficiary bank due to transaction processing problem.";
                case 37:
                    return "Technical decline by beneficiary bank due to transaction processing problem.";
                case 38:
                    return "Exceeds the maximum permissible per transaction value.";
                case 39:
                    return "Timeout at CDCI for outward transaction.";
                case 40:
                    return "Timeout at CDCI for intra-bank transaction.";
                case 41:
                    return "If the txn date/ time mentioned in the API request is not equal to current date/time, or it is not in the format YYYYMMDDHHmmss.";
                case 49:
                    return "This response is populated by NPCI for suspected fraud basis the model risk score.";
                case 50:
                    return "This response is populated by NPCI if the limit for number of check transactions is breached.";
                case 51:
                    return "Transaction is declined by the beneficiary bank due to insufficient funds in their pool account.";
                case 52:
                    return "Beneficiary account is invalid.";
                case 60:
                    return "Transaction declined due to a technical issue..";
                case 61:
                    return "Transaction declined due to a technical issue.";
                case 62:
                    return "Undefined response code sent by NPCI.";
                case 63:
                    return "Transaction timed-out at ICICI Bank.";
                case 65:
                    return "Daily limit (count) of transactions in the beneficiary account has been breached. OR Exceeds the maximum permissible balance in the account of the beneficiary. (e.g. Jan Dhan Yojana accounts, Payments Bank etc.).";
                case 66:
                    return "Transaction timed-out at ICICI Bank.";
                case 67:
                    return "Funds returned by beneficiary bank.";
                case 68:
                    return "The transaction is rejected with this error code if there already exists a transaction with the same Settlement ID which is PENDING or is SUCCESSFUL. Applicable for clients who.";
                case 69:
                    return "The response code is populated in status check API if the funds are returned from the beneficiary bank.";
                case 70:
                    return "Technical failure at OCH.";
                case 71:
                    return "Technical failure at OCH.";
                case 72:
                    return "Technical failure at OCH.";
                case 73:
                    return "Timeout at OCH.";
                case 74:
                    return "Transactions are declined for OCH. Client need to ensure funds are available in current or nodal account.";
                case 75:
                    return "Technical failure at OCH.";
                case 76:
                    return "Balance not available in remitter's account.";
                case 77:
                    return "Technical failure at OCH.";
                case 78:
                    return "Technical failure at OCH.";
                case 80:
                    return "The error is populated by the remitter bank if the same request initiated almost instantly before the first transaction is logged in the system.";
                case 96:
                    return "Technical decline by beneficiary bank due to transaction processing problem.";
                case 101:
                    return "TIMPS Switch Not reachable.";
                case 102:
                    return "Connectivity is disconnected while the transaction is in process.";
                case 103:
                    return "TIMPS Switch Not reachable.";
                case 201:
                    return "When the beneficiary IFSC code is invalid, the transaction is rejected by NPCI.";
                case 202:
                    return "Customer transaction Limit Exceeded.";
                case 203:
                    return "Not Applicable for Composite Payouts.";
                case 204:
                    return "Decline by NPCI / Beneficiary Bank.";
                case 205:
                    return "Transaction is declined by NPCI if the payment amount is less than INR 1.00.";
                case 206:
                    return "Transaction is declined by beneficiary bank due to invalid remitter account number.";
                case 207:
                    return "General error.";
                case 208:
                    return "Not Applicable for Composite Payouts.";
                case 403:
                    return "Transaction initiated from unwhitelisted IP.";
                case 901:
                    return "Declined by remitter bank for invalid BC details.";
                case 902:
                    return "Declined by remitter bank if the passcode is incorrect.";
                case 903:
                    return "Declined by remitter bank for invalid BC retailer code.";
                case 904:
                    return "Declined by remitter bank for an internal exception";
                case 915:
                    return "Not Applicable for Composite Payouts.";
                case 916:
                    return "Not Applicable for Composite Payouts.";
                case 917:
                    return "Not Applicable for Composite Payouts.";
                case 918:
                    return "Not Applicable for Composite Payouts.";
                case 919:
                    return "Not Applicable for Composite Payouts.";
                case 920:
                    return "Not Applicable for Composite Payouts.";
                case 921:
                    return "Not Applicable for Composite Payouts.";
                case 922:
                    return "Not Applicable for Composite Payouts.";
                case 923:
                    return "Not Applicable for Composite Payouts.";
                case 924:
                    return "Not Applicable for Composite Payouts.";
                case 501:
                    return "The response code is populated by NPCI for a duplicate request.";
                case 502:
                    return "The response code is populated by NPCI if the amount cap is exceeded.";
                case 503:
                    return "The response code is populated by NPCI if the net debit cap is exceeded.";
                case 504:
                    return "The response code is populated by NPCI if the request is not found";
                case 506:
                    return "The response code is populated by NPCI if there is a mismatch in the transaction ID.";
                case 517:
                    return "The response code is populated by NPCI if the beneficiary bank is not registered.";
                case 528:
                    return "The response code is populated by NPCI if the beneficiary bank is not available.";
                case 548:
                    return "The response code is populated by NPCI if the transaction id is not present.";
                case 549:
                    return "The response code is populated by NPCI if the request message id is not present.";
                case 552:
                    return "The response code is populated by NPCI if the beneficiary orgID is not found in the request.";
                default:
                    return "Transaction declined due to a technical issue.";

            }

        }
        #endregion

        #region ServerTime
        public ServerStatus TimeChk()
        {
            ServerStatus ss = new ServerStatus();
            string vTime = DateTime.Now.ToString("HH:mm:ss");
            DateTime dt = DateTime.Parse("01/01/1900 " + vTime);
            DateTime dt1 = DateTime.Parse(vAccessTime);
            if (dt >= dt1)
            {
                ss.Status = "false";
                ss.StatusDesc = "Failed: After " + dt1.ToString("hh:mm tt") + " cannot open this page.";
            }
            else
            {
                ss.Status = "true";
                ss.StatusDesc = "Success: Successfully open.";
            }
            return ss;
        }
        #endregion

        #region Prosidex Integration
        public ProsidexResponse Prosidex(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            CRepository oDb = new CRepository();
            //ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;

            dt = oDb.GetProsidexReqData(pMemberID, pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pLoanId;
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
                //pReqData.Request = pReq;
            }
            //pResponseData = ProsidexSearchCustomer(pReqData);
            pResponseData = ProsidexEncryption(pReq);
            return pResponseData;
        }

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "", vLoanId = "";
            Int32 vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            CRepository oDb = new CRepository();
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vLoanId = prosidexRequest.Request.DG.APPLICATIONID;

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
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
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }

        public ProsidexResponse ProsidexEncryption(Request Req)
        {
            CRepository oDb = new CRepository();
            string vRequestdata = "", vFullResponse = "", vResponse = "", vUCIC = "", vRequestId = "",
                vMemberId = "", vPotentialYN = "N", vPotenURL = "", vResponseCode = "", vRsaKey = "",
                vResponseData = "", vEncryptedMatchResponse = "", vLoanId = "";
            string vPostUrl = PosidexEncURL + "/ServicePosidex.svc/PosidexSearchCustomer";
            int vCGTID = 0, vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            //-------------------------------------------------------------
            vRequestId = Req.UnitySfb_RequestId;
            vMemberId = Req.DG.CUST_ID;
            vLoanId = Req.DG.APPLICATIONID;
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
            oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
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

        #region Web Config Data
        string vKeyId = ConfigurationManager.AppSettings["KeyId"];
        string vOuId = ConfigurationManager.AppSettings["OuId"];
        string vSecret = ConfigurationManager.AppSettings["Secret"];
        string vAccountId = ConfigurationManager.AppSettings["AccountId"];
        string vApiKey = ConfigurationManager.AppSettings["ApiKey"];
        #endregion

        #region HttpWebRequest
        public string HttpWebRequest(string pUrl, string pReqData)
        {
            string vResponse = "";
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(pUrl) as HttpWebRequest;
                request.ContentType = "application/json";
                request.Method = "POST";
                request.Headers.Add("account-id", vAccountId);
                request.Headers.Add("api-key", vApiKey);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(pReqData);
                    streamWriter.Close();
                }
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = streamReader.ReadToEnd();
                request.GetResponse().Close();
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
        #endregion

        #region Task
        public string Task(string pRequestId, int pCnt, string pVerificationType)
        {
            string vResponse = "";
            string pUrl = "https://eve.idfy.com/v3/tasks?request_id=" + pRequestId;
            try
            {
                int i = pVerificationType == "A" ? 3 : 5; int vTimeOut = pVerificationType == "A" ? 1000 : 3500;
                pCnt = pCnt + 1;
                if (pCnt <= i)
                {
                    System.Threading.Thread.Sleep(vTimeOut);
                    HttpWebRequest request = WebRequest.Create(pUrl) as HttpWebRequest;
                    request.Headers.Add("account-id", vAccountId);
                    request.Headers.Add("api-key", vApiKey);
                    StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                    vResponse = streamReader.ReadToEnd();
                    request.GetResponse().Close();
                    dynamic vRes = JsonConvert.DeserializeObject(vResponse);
                    string vStatus = Convert.ToString(vRes[0].status);
                    if (vStatus == "completed")
                    {
                        string vStat = Convert.ToString(vRes[0].result.source_output.status);
                        if (vStat != (pVerificationType == "A" ? "success" : "id_found"))
                        {
                            Task(pUrl, pCnt, pVerificationType);
                        }
                    }
                    else
                    {
                        vResponse = "[{\"message\":\"Time out\",\"status\":\"failed\"}]";
                    }
                }
                else
                {
                    vResponse = "[{\"message\":\"Insufficient credits\",\"status\":\"failed\"}]";
                }
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
        #endregion

        #region NewGuid
        public string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        #region IDFY Integration
        public KYCVoterIDResponse IdfyVoterVerify(PostVoterData PostVoterData)
        {
            VoterData oVoter = new VoterData { id_number = PostVoterData.VoterId };
            VoterRequest oVoterReq = new VoterRequest(NewGuid(), NewGuid(), oVoter);
            string vReqData = JsonConvert.SerializeObject(oVoterReq);//Request Body
            string vUrl = "https://eve.idfy.com/v3/tasks/async/verify_with_source/ind_voter_id";
            string vResponse = HttpWebRequest(vUrl, vReqData);
            dynamic vResponseData = JsonConvert.DeserializeObject(vResponse);
            string vRequestID = Convert.ToString(vResponseData.request_id);
            string vRes = Task(vRequestID, 0, "V");
            //string vRes = "[{\"action\":\"verify_with_source\",\"completed_at\":\"2023-09-28T16:01:16+05:30\",\"created_at\":\"2023-09-28T16:01:16+05:30\",\"group_id\":\"407658ab-951c-4771-a55d-80d7098e2402\",\"request_id\":\"4d71e0ce-9e58-46fb-8f17-f7ac44ba3d11\",\"result\":{\"source_output\":{\"ac_no\":null,\"date_of_birth\":null,\"district\":null,\"gender\":\"M\",\"house_no\":null,\"id_number\":\"NVZ1544972\",\"last_update\":\"2023-02-25\",\"name_on_card\":\"kushal cole\",\"part_no\":null,\"ps_lat_long\":null,\"ps_name\":null,\"rln_name\":null,\"section_no\":null,\"source\":\"government_website\",\"st_code\":null,\"state\":\"WEST BENGAL\",\"status\":\"id_found\"}},\"status\":\"completed\",\"task_id\":\"9a8bdaa2-35f9-422d-834e-ab458995af0b\",\"type\":\"ind_voter_id\"}]";
            dynamic vRespVoterData = JsonConvert.DeserializeObject(vRes);
            string vStatus = vRespVoterData[0].status;
            string vRequestId = vRespVoterData[0].request_id;
            KYCVoterIDResponseResult vKR = new KYCVoterIDResponseResult();
            KYCVoterIDResponse oKVIR = new KYCVoterIDResponse();
            if (vStatus.ToLower() == "completed")
            {
                string ac_no, date_of_birth, district, gender, house_no, id_number, last_update, name_on_card, part_no, ps_lat_long,
                ps_name, rln_name, section_no, source, st_code, state, status;
                //--------------------------------------------------------------------------
                ac_no = vRespVoterData[0].result.source_output.ac_no;
                date_of_birth = vRespVoterData[0].result.source_output.date_of_birth;
                district = vRespVoterData[0].result.source_output.district;
                gender = vRespVoterData[0].result.source_output.gender;
                house_no = vRespVoterData[0].result.source_output.house_no;
                id_number = vRespVoterData[0].result.source_output.id_number;
                last_update = vRespVoterData[0].result.source_output.last_update;
                name_on_card = vRespVoterData[0].result.source_output.name_on_card;
                part_no = vRespVoterData[0].result.source_output.part_no;
                ps_lat_long = vRespVoterData[0].result.source_output.ps_lat_long;
                ps_name = vRespVoterData[0].result.source_output.ps_name;
                rln_name = vRespVoterData[0].result.source_output.rln_name;
                section_no = vRespVoterData[0].result.source_output.section_no;
                source = vRespVoterData[0].result.source_output.source;
                st_code = vRespVoterData[0].result.source_output.st_code;
                state = vRespVoterData[0].result.source_output.state;
                status = vRespVoterData[0].result.source_output.status;
                //---------------------------------------------------------------------------
                vKR.ac_no = ac_no; vKR.district = district; vKR.dob = date_of_birth; vKR.epic_no = id_number;
                vKR.gender = gender; vKR.house_no = house_no; vKR.id = id_number; vKR.last_update = last_update;
                vKR.name = name_on_card; vKR.part_no = part_no; vKR.ps_lat_long = ps_lat_long; vKR.ps_name = ps_name;
                vKR.rln_name = rln_name; vKR.st_code = st_code; vKR.state = state;
                //-----------------------------------------------------------------------------
                oKVIR.request_id = vRequestId;
                oKVIR.result = vKR;
                oKVIR.status_code = "101";
            }
            SaveIdfyVoterLog(vRequestID, vRes, PostVoterData);
            return oKVIR;
        }

        public IdfyAadharVerifyData IdfyAadharVerify(PostAadharData postAadharData)
        {
            ExtraFields oExtra = new ExtraFields();
            Data oData = new Data(NewGuid(), vKeyId, vOuId, vSecret, vWebHookUrl, "ADHAR", "xml", oExtra);
            FetchDocBody oReqBody = new FetchDocBody(NewGuid(), NewGuid(), oData);
            string vReqData = JsonConvert.SerializeObject(oReqBody); //Request Body

            string vUrl = "https://eve.idfy.com/v3/tasks/async/verify_with_source/ind_digilocker_fetch_documents";
            string vResponse = HttpWebRequest(vUrl, vReqData);
            dynamic vResponseData = JsonConvert.DeserializeObject(vResponse);
            string vRequestID = Convert.ToString(vResponseData.request_id);
            string vRes = Task(vRequestID, 0, "A");
            // string vRes = "[{\"action\":\"verify_with_source\",\"completed_at\":\"2023-09-19T17:37:35+05:30\",\"created_at\":\"2023-09-19T17:37:35+05:30\",\"group_id\":\"d4bfcd51-1a10-4753-93bb-052d6079f34c\",\"request_id\":\"a6e08b08-7d6e-47cd-b1dd-987d8d5d3a6c\",\"result\":{\"source_output\":{\"redirect_url\":\"https://capture.kyc.idfy.com/document-fetcher/digilocker/session-start/?reference_id=d1f059f8-e74d-4582-8068-9a64f538c281&ou_id=22a2e0b49d6a&key_id=4583adb3-32a7-4615-82ff-62eae2f72df0&h=c1b55097c6deb10d1f37424fac9548a97a9120e9\",\"reference_id\":\"d1f059f8-e74d-4582-8068-9a64f538c281\",\"session_exists\":false,\"status\":\"success\"}},\"status\":\"completed\",\"task_id\":\"51cb234c-3c3b-4e9b-b2c9-c1ef63350256\",\"type\":\"ind_digilocker_fetch_documents\"}]";
            dynamic vResp = JsonConvert.DeserializeObject(vRes);
            string vstatus = Convert.ToString(vResp[0].status);
            string vstatus1 = Convert.ToString(vResp[0].result.source_output.status);
            string vDigiLockerUrl = "", vRefId = "";
            if (vstatus == "completed" && vstatus1 == "success")
            {
                vDigiLockerUrl = Convert.ToString(vResp[0].result.source_output.redirect_url);
                vRefId = Convert.ToString(vResp[0].result.source_output.reference_id);
            }
            SaveIdfyAadhaarLog(vRefId, postAadharData, vRes);
            IdfyAadharVerifyData vAadharData = new IdfyAadharVerifyData(vDigiLockerUrl, vRefId);
            return vAadharData;
        }

        public string IdfyAadharVerifyData(IdfyAadharVerifyResponseData IdfyAadharData)
        {
            string vReqId = "", vStatus = "FAILED", vUid = "";
            //dynamic vResponseData = JsonConvert.DeserializeObject(pData);
            string pData = JsonConvert.SerializeObject(IdfyAadharData);
            try
            {
                vReqId = IdfyAadharData.reference_id;
                vStatus = IdfyAadharData.status;
                vUid = IdfyAadharData.parsed_details.uid;
                if (vStatus != null)
                {
                    if (vStatus.ToUpper() == "SUCCESS")
                    {
                        vUid = vUid.Substring(vUid.Length - 4);
                    }
                    else { }
                }
                UpdateIdfyAadhaarLog(vReqId, pData, vStatus, vUid);
            }
            finally { }
            return vStatus;
        }

        public string IdfyAadharVerifyJson(string pReqId)
        {
            System.Threading.Thread.Sleep(3000);
            string vStatus = GetIdfyAadhaarLog(pReqId);
            return vStatus;
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

        #region JocataCalling

        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vRampResponse = "";
            try
            {
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

        public string JocataRequest(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            string vResponseData = "";
            DataTable dt = new DataTable();
            CRepository oCR = null;
            try
            {
                oCR = new CRepository();
                dt = oCR.GetJocataRequestData(pMemberID);
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

                    vResponseData = RampRequest(req);
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
                        }
                        else
                        {
                            try
                            {
                                Prosidex(pMemberID, pLoanId, pCreatedBy);
                            }
                            finally { }
                        }
                        oCR = new CRepository();
                        oCR.UpdateJocataStatus(pLoanId, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                    }
                    oCR = new CRepository();
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    oCR.SaveJocataLog(pMemberID, pLoanId, vResponseXml, vScreeningId);
                }
            }
            catch
            {
                oCR = new CRepository();
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                oCR.SaveJocataLog(pMemberID, pLoanId, vResponseXml, "");
            }
            finally { }
            return "";
        }

        public string JocataRiskCat(RiskCatReq vRiskCatReq)
        {
            string vJokataToken = vJocataToken, vRiskCategory = "", vRampResponse = "", vResponseXml = "", pLoanId = "", pMemberID = "";
            CRepository oCR = null;
            try
            {
                //------------------------------------URL----------------------------------------------            
                string postURL = vDBName.ToUpper() == "CENTRUM_SME" ? "https://aml.unitybank.co.in/orck/on-boarding/calculate-risk"
                  : "https://jocatauat.unitybank.co.in/orck/on-boarding/calculate-risk";
                string vToken = vDBName.ToUpper() == "CENTRUM_SME" ? "611d9587-7546-8e62-1809-c8f8c193d421" : "596cf388-a04f-53b1-ce6b-e9a54082363f";
                //------------------------------------------------------------------------------------
                pMemberID = vRiskCatReq.memberId; pLoanId = vRiskCatReq.LoanNo;
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
                oCR = new CRepository();
                oCR.SaveJocataRiskCategoryLog(pMemberID, pLoanId, vResponseXml, vRiskCategory);
                return vRiskCategory;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vRampResponse, "root"));
                    oCR = new CRepository();
                    oCR.SaveJocataRiskCategoryLog(pMemberID, pLoanId, vResponseXml, vRiskCategory);
                }
            }
            finally
            {
            }
            return vRampResponse;
        }
        #endregion

        #region UpdateLogoutDtTime
        public string UpdateLogoutDt(string LoginId)
        {
            int vErr = 0;
            CRepository oCR = new CRepository();
            try
            {
                vErr = oCR.UpdateLogOutDt(Convert.ToInt32(LoginId));
            }
            catch
            {
                vErr = 1;
            }
            finally { }
            return vErr == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data.";
        }
        #endregion

        #region UpdateSessionTime
        public string UpdateSessionTime(string LoginId)
        {
            int vErr = 0;
            CRepository oCR = new CRepository();
            try
            {
                vErr = oCR.UpdateSessionTime(Convert.ToInt32(LoginId));
            }
            catch
            {
                vErr = 1;
            }
            finally { }
            return vErr == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data.";
        }
        #endregion

        #region SendMFAOTP
        public string SendMFAOTP(string pOTP, string pMobileNo)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string vOTP = pOTP;
                string vMsgBody = "Dear User, Your Forceten login OTP is " + vOTP + ". Never share OTP. Regards, USFB Bank.";
                //********************************************************************
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000204129";
                String passwd = "Unity@1122";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707166254686719830&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
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
                result = "Error sending OTP.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }
        #endregion

        #region InsertLoginDt
        public LoginLogOutData InsertLoginDt(LoginReqData Req)
        {
            string vLoginId = "0";
            LoginLogOutData oLD = new LoginLogOutData();
            CRepository oRP = new CRepository();
            vLoginId = oRP.InsertLoginDt(Req);
            oLD.LoginId = vLoginId;
            return oLD;
        }
        #endregion

        #region InsertFundSourceUpload
        public Int32 InsertFundSourceUpload(string pFunderXml, string pLoginDt, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertFSUpload";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pFunderXml.Length + 1, "@pXml", pFunderXml);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", setDate(pLoginDt));
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

        #region InsertBulkFunderUploadApprove
        public Int32 InsertBulkFunderUploadApprove(string pFSUID, string pLoginDt, string pAppBy, string pAppRej)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertBulkFunderUploadApprove";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pFSUID", Convert.ToInt32(pFSUID));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", setDate(pLoginDt));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAppBy", Convert.ToInt32(pAppBy));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppRej", pAppRej);
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

        #region SendOTP
        public string SendOTP(OTPData objOTPData)
        {
            string result = "", vOTPId = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                int vErr = SaveOTPLog(objOTPData.pMobileNo, objOTPData.pOTP, ref vOTPId, "");
                if (vErr == 0)
                {
                    string vOTP = objOTPData.pOTP;
                    //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)
                    //string vMsgBody = "Thank you for your loan application with Centrum Microcredit Ltd. Your verification OTP is " + vOTP + ".";
                    string vMsgBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vOTP + ".";
                    //********************************************************************
                    String sendToPhoneNumber = objOTPData.pMobileNo;
                    //String userid = "2000194447";
                    //String passwd = "Centrum@2020";
                    //String userid = "2000204129";
                    //String passwd = "Unity@1122";
                    String userid = "2000243134";
                    String passwd = "ZFimpPeKx";
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                    String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472061562826&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                    request = WebRequest.Create(url);
                    // Send the 'HttpWebRequest' and wait for response.
                    response = (HttpWebResponse)request.GetResponse();
                    Stream stream = response.GetResponseStream();
                    Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                    StreamReader reader = new System.IO.StreamReader(stream, ec);
                    result = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                }
                else
                {
                    result = "Please try after 5 minutes..";
                }
            }
            catch (Exception exp)
            {
                result = "Error sending OTP.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
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

        #region ForgotPassword
        public ForgotOTPRes SendForgotOTP(string pUserName)
        {
            DataTable dt = new DataTable();
            string result = "", vOTPId = "0", vOTP = "", vMobileNo = "", vUserName = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            ForgotOTPRes oFO = null;
            try
            {
                vUserName = pUserName;
                dt = ChkValidUserName(vUserName);
                if (dt.Rows.Count > 0)
                {
                    vMobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    vOTP = Convert.ToString(dt.Rows[0]["OTP"]);
                    int vErr = SaveOTPLog(vMobileNo, vOTP, ref vOTPId, vUserName);
                    if (vErr == 0)
                    {
                        string vMsgBody = vOTP + " is your OTP to reset your password. OTP valid for 5 minutes. Do not share this with anyone for security reasons. Unity Bank";
                        //********************************************************************
                        String sendToPhoneNumber = vMobileNo;
                        String userid = "2000243134";
                        String passwd = "ZFimpPeKx";
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707172985365562069&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                        request = WebRequest.Create(url);
                        response = (HttpWebResponse)request.GetResponse();
                        Stream stream = response.GetResponseStream();
                        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                        StreamReader reader = new System.IO.StreamReader(stream, ec);
                        result = reader.ReadToEnd();
                        reader.Close();
                        stream.Close();
                        oFO = new ForgotOTPRes("XX", "200", "Success:OTP has been successfully sent.", vOTPId);
                    }
                    else
                    {
                        result = "Please try after 5 minutes..";
                        oFO = new ForgotOTPRes("XX", "201", "Failed:" + result, vOTPId);
                    }
                }
                else
                {
                    result = "Invalid User Name or Password..";
                    oFO = new ForgotOTPRes("XX", "401", "Failed:" + result, vOTPId);
                }
            }
            catch (Exception exp)
            {
                result = "Error sending OTP.." + exp.ToString();
                oFO = new ForgotOTPRes("XX", "500", "Failed:" + result, vOTPId);
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return oFO;
        }

        public DataTable ChkValidUserName(string pUserName)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkValidUserName";
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pUserName", pUserName);
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

        public ForgotOTPRes ForgotPassword(string pUserName, string pPassword, string pOTPId, string pOTP)
        {
            ForgotOTPRes oFO = null;
            CRepository vResp = new CRepository();
            int vErr = 0;
            vErr = vResp.ForgotPassword(pUserName, pPassword, pOTPId, pOTP);
            if (vErr == 0)
            {
                oFO = new ForgotOTPRes("", "200", "Success:Password update successfully.", "XX");
            }
            else if (vErr == 2)
            {
                oFO = new ForgotOTPRes("", "400", "Failed:Invalid credentials.", "XX");
            }
            else
            {
                oFO = new ForgotOTPRes("", "204", "Failed:Failed to update password.", "XX");
            }
            return oFO;
        }

        public ForgotOTPRes ValidateOTP(string pOTP, string pOTPId)
        {
            DataTable dt = new DataTable();
            ForgotOTPRes oFO = null;
            CRepository oRsp = new CRepository();
            int vErr = 0;
            string pErrDesc = "";
            try
            {
                vErr = oRsp.ValidateOTP(pOTP, Convert.ToInt32(pOTPId), ref pErrDesc);
                if (vErr == 0)
                {
                    oFO = new ForgotOTPRes("XX", "200", pErrDesc, pOTPId);
                }
                else if (vErr == 1)
                {
                    oFO = new ForgotOTPRes("XX", "401", pErrDesc, pOTPId);
                }
                else
                {
                    oFO = new ForgotOTPRes("XX", "400", pErrDesc, pOTPId);
                }

            }
            catch (Exception ex)
            {
                oFO = new ForgotOTPRes("XX", "400", "Failed:Invalid OTP.", pOTPId);
            }
            return oFO;
        }
        #endregion

        #region eIBMAadhaarOTP
        public IBMAadhaarOTPResponse IBMAadhaarOTP(eIBMAadhaarOTP eIBMAadhaarOTP)
        {
            string sourceId = "FT";
            DateTime d = DateTime.Now;
            string dateString = d.ToString("yyyyMMddHHmmss");
            string vRndNo = GenerateRandomNo();
            string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"
            string txn = "AUANSDL001:" + dateString + vRndNo;
            string type = "A";
            string ch = "01";
            string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 
            IBMAadhaarOTPResponse responseObj = null;

            IBMAadhaarOTPReq pReq = new IBMAadhaarOTPReq();
            pReq.sourceId = sourceId;
            pReq.traceId = traceId;
            pReq.uid = eIBMAadhaarOTP.aadhaarNo;
            pReq.txn = txn;
            pReq.ts = ts;
            pReq.type = type;
            pReq.ch = ch;

            string requestBody = JsonConvert.SerializeObject(pReq);
            string postURL = IBMAadhaarUrl + "/aadharOtpRequest";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                try
                {
                    fullResponse = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                    fullResponse = fullResponse.Trim('"');
                    dynamic res = JsonConvert.DeserializeObject(fullResponse);

                    if (Convert.ToString(res.status) == "success")
                    {
                        responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                        responseObj.Status = Convert.ToString(res.status);
                        responseObj.txn = txn;
                        responseObj.StatusCode = "";
                        responseObj.StatusMsg = Convert.ToString(res.message);
                        responseObj.ErrorMsg = "";
                    }
                    else if (Convert.ToString(res.status) == "fail")
                    {
                        responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                        responseObj.Status = Convert.ToString(res.status);
                        responseObj.txn = "";
                        string err = Convert.ToString(res.err);
                        responseObj.StatusCode = err;
                        responseObj.StatusMsg = Convert.ToString(res.message);

                        switch (err)
                        {
                            case "110":
                                responseObj.ErrorMsg = "Aadhaar number does not have email ID.";
                                break;
                            case "111":
                                responseObj.ErrorMsg = "Aadhaar number does not have mobile number.";
                                break;
                            case "112":
                                responseObj.ErrorMsg = "Aadhaar number does not have both email ID and mobile number.";
                                break;
                            case "113":
                                responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified email ID.";
                                break;
                            case "114":
                                responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified Mobile Number.";
                                break;
                            case "115":
                                responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified email and Mobile.";
                                break;
                            case "510":
                                responseObj.ErrorMsg = "Invalid “Otp” XML format.";
                                break;
                            case "520":
                                responseObj.ErrorMsg = "Invalid device.";
                                break;
                            case "521":
                                responseObj.ErrorMsg = "Invalid mobile number.";
                                break;
                            case "522":
                                responseObj.ErrorMsg = "Invalid “type” attribute.";
                                break;
                            case "523":
                                responseObj.ErrorMsg = "Invalid “ts” attribute. Either it is not in correct format or is older than 20 min.";
                                break;
                            case "530":
                                responseObj.ErrorMsg = "Invalid AUA code.";
                                break;
                            case "540":
                                responseObj.ErrorMsg = "Invalid OTP XML version.";
                                break;
                            case "542":
                                responseObj.ErrorMsg = "AUA not authorized for ASA. This error will be returned if AUA and ASA do not have linking in the portal.";
                                break;
                            case "543":
                                responseObj.ErrorMsg = "Sub-AUA not associated with “AUA”. This error will be returned if Sub-AUA specified in “sa” attribute is not added as “Sub-AUA” in portal.";
                                break;
                            case "565":
                                responseObj.ErrorMsg = "AUA License key has expired or is invalid.";
                                break;
                            case "566":
                                responseObj.ErrorMsg = "ASA license key has expired or is invalid.";
                                break;
                            case "569":
                                responseObj.ErrorMsg = "Digital signature verification failed.";
                                break;
                            case "570":
                                responseObj.ErrorMsg = "Invalid key info in digital signature(this means that certificate used for signing the OTP request is not valid – it is either expired, or does not belong to the AUA or is not created by a CA).";
                                break;
                            case "940":
                                responseObj.ErrorMsg = "Unauthorized ASA channel. ";
                                break;
                            case "941":
                                responseObj.ErrorMsg = "Unspecified ASA channel.";
                                break;
                            case "950":
                                responseObj.ErrorMsg = "Could not generate and/or send OTP.";
                                break;
                            case "952":
                                responseObj.ErrorMsg = "OTP Flooding error.";
                                break;
                            case "999":
                                responseObj.ErrorMsg = "Unknown error.";
                                break;
                        }
                    }
                    else if (Convert.ToString(res.Status) == "Fail")
                    {
                        responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                        responseObj.Status = Convert.ToString(res.Status);
                        responseObj.txn = "";
                        string StatusCode = Convert.ToString(res.StatusCode);
                        responseObj.StatusCode = StatusCode;
                        responseObj.StatusMsg = Convert.ToString(res.ConnectionError);
                        responseObj.ErrorMsg = Convert.ToString(res.ErrorMessage);
                    }
                    else if (Convert.ToString(res.NSDL_Error_Code) != "")
                    {
                        responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                        responseObj.Status = "";
                        responseObj.txn = "";
                        string NSDL_Error_Code = Convert.ToString(res.NSDL_Error_Code);
                        responseObj.StatusCode = NSDL_Error_Code;
                        responseObj.StatusMsg = "Failure from NSDL";

                        switch (NSDL_Error_Code)
                        {
                            case "E-000":
                                responseObj.ErrorMsg = "Request received is a HTTP request";
                                break;
                            case "E-001":
                                responseObj.ErrorMsg = "Request received is a get request";
                                break;
                            case "E-100":
                                responseObj.ErrorMsg = "Auth XML not parsed properly";
                                break;
                            case "E-101":
                                responseObj.ErrorMsg = "KYC XML not parsed properly";
                                break;
                            case "E-102":
                                responseObj.ErrorMsg = "Audit logging in DB failed for request";
                                break;
                            case "E-103":
                                responseObj.ErrorMsg = "Audit logging in DB failed for response";
                                break;
                            case "E-105":
                                responseObj.ErrorMsg = "KYC XSD Validation failed";
                                break;
                            case "E-106":
                                responseObj.ErrorMsg = "KYC Request signature verification failed";
                                break;
                            case "E-107":
                                responseObj.ErrorMsg = "Auth Request signature verification failed";
                                break;
                            case "E-108":
                                responseObj.ErrorMsg = "IP verification failed for entity";
                                break;
                            case "E-109":
                                responseObj.ErrorMsg = "Blank response received from UIDAI";
                                break;
                            case "E-110":
                                responseObj.ErrorMsg = "Unable to decrypt response at KSA";
                                break;
                            case "E-111":
                                responseObj.ErrorMsg = "KYC response signature verification failed";
                                break;
                            case "E-112":
                                responseObj.ErrorMsg = "BFD XSD validation failed";
                                break;
                            case "E-113":
                                responseObj.ErrorMsg = "OTP XSD validation failed";
                                break;
                            case "E-114":
                                responseObj.ErrorMsg = "KYC response XML not parsed properly";
                                break;
                            case "E-115":
                                responseObj.ErrorMsg = "AUTH response XML not parsed properly";
                                break;
                            case "E-116":
                                responseObj.ErrorMsg = "Signed Auth XML generation Error";
                                break;
                            case "E-117":
                                responseObj.ErrorMsg = "Signed KYC XML generation Error";
                                break;
                            case "E-118":
                                responseObj.ErrorMsg = "Auth response signature verification failed";
                                break;
                            case "E-119":
                                responseObj.ErrorMsg = "ASA or KSA is unable to connect to UIDAI server";
                                break;
                            case "E-120":
                                responseObj.ErrorMsg = "Auth XSD validation failed";
                                break;
                            case "E-122":
                                responseObj.ErrorMsg = "Property file unavailable";
                                break;
                            case "E-123":
                                responseObj.ErrorMsg = "BFD request XML not parsed properly";
                                break;
                            case "E-124":
                                responseObj.ErrorMsg = "OTP request XML not parsed properly";
                                break;
                            case "E-125":
                                responseObj.ErrorMsg = "BFD request signature verification failed";
                                break;
                            case "E-126":
                                responseObj.ErrorMsg = "OTP request signature verification failed";
                                break;
                            case "E-127":
                                responseObj.ErrorMsg = "Signed BFD XML generation error";
                                break;
                            case "E-128":
                                responseObj.ErrorMsg = "Signed OTP XML generation error";
                                break;
                            case "E-129":
                                responseObj.ErrorMsg = "BFD response XML not parsed properly";
                                break;
                            case "E-130":
                                responseObj.ErrorMsg = "OTP response XML not parsed properly";
                                break;
                            case "E-131":
                                responseObj.ErrorMsg = "XML decryption error";
                                break;
                            case "E-132":
                                responseObj.ErrorMsg = "Error during KYC request signature verification";
                                break;
                            case "E-133":
                                responseObj.ErrorMsg = "Error during KYC response signature verification";
                                break;
                            case "E-134":
                                responseObj.ErrorMsg = "Error during AUTH request signature verification";
                                break;
                            case "E-135":
                                responseObj.ErrorMsg = "Error during AUTH response signature verification";
                                break;
                            case "E-136":
                                responseObj.ErrorMsg = "Error during BFD request signature verification";
                                break;
                            case "E-137":
                                responseObj.ErrorMsg = "Error during OTP request signature verification";
                                break;
                            case "E-138":
                                responseObj.ErrorMsg = "Error during KYC XSD Validation";
                                break;
                            case "E-139":
                                responseObj.ErrorMsg = "Error during AUTH XSD Validation";
                                break;
                            case "E-140":
                                responseObj.ErrorMsg = "Error during BFD XSD Validation";
                                break;
                            case "E-141":
                                responseObj.ErrorMsg = "Error during OTP XSD Validation";
                                break;
                            case "E-142":
                                responseObj.ErrorMsg = "Error during IP verification";
                                break;
                            case "E-143":
                                responseObj.ErrorMsg = "Response received is E";
                                break;
                            case "E-144":
                                responseObj.ErrorMsg = "BFD response signature verification failed";
                                break;
                            case "E-145":
                                responseObj.ErrorMsg = "OTP Signature Tag Missing in Request XML";
                                break;
                            case "E-149":
                                responseObj.ErrorMsg = "Invalid Aadhar Number";
                                break;
                            case "E-199":
                                responseObj.ErrorMsg = "KSA/ASA Internal Error";
                                break;
                            case "E-200":
                                responseObj.ErrorMsg = "One of the mandatory Sub-Aua element is null";
                                break;
                            case "E-201":
                                responseObj.ErrorMsg = "Error while validating strSaCd parameter";
                                break;
                            case "E-203":
                                responseObj.ErrorMsg = "Error while validating strAadhaar parameter";
                                break;
                            case "E-204":
                                responseObj.ErrorMsg = "Error while validating strAadhaarName parameter";
                                break;
                            case "E-205":
                                responseObj.ErrorMsg = "Error while validating strYear parameter";
                                break;
                            case "E-206":
                                responseObj.ErrorMsg = "Error while validating strGender parameter";
                                break;
                            case "E-207":
                                responseObj.ErrorMsg = "Error while validating strTransId parameter";
                                break;
                            case "E-208":
                                responseObj.ErrorMsg = "Error while validating strUDC parameter";
                                break;
                            case "E-209":
                                responseObj.ErrorMsg = "Error while validating strMV parameter";
                                break;
                            case "E-210":
                                responseObj.ErrorMsg = "Value of mvThreshold in property file should be in range 1-100";
                                break;
                            case "E-211":
                                responseObj.ErrorMsg = "Error while validating strLang: If strLname is provided then strLang is mandatory";
                                break;
                            case "E-212":
                                responseObj.ErrorMsg = "Error while validating strLmv parameter. Value should be in range 1-100(inclusive)";
                                break;
                            case "E-213":
                                responseObj.ErrorMsg = "DSC signature verification failed for Sub-AUA";
                                break;
                            case "E-214":
                                responseObj.ErrorMsg = "Error while validating strOtp parameter";
                                break;
                            case "E-215":
                                responseObj.ErrorMsg = "Error while validating strRequestType parameter";
                                break;
                            case "E-216":
                                responseObj.ErrorMsg = "Error while validating strBioType parameter";
                                break;
                            case "E-217":
                                responseObj.ErrorMsg = "Error while validating strFingerOnePos parameter";
                                break;
                            case "E-218":
                                responseObj.ErrorMsg = "Error while validating strFingerTwoPos parameter";
                                break;
                            case "E-219":
                                responseObj.ErrorMsg = "Error while validating strMS parameter";
                                break;
                            case "E-220":
                                responseObj.ErrorMsg = "Blank Signature Tag Exception";
                                break;
                            case "E-221":
                                responseObj.ErrorMsg = "Incorrect OTP Version";
                                break;
                            case "E-230":
                                responseObj.ErrorMsg = "Error during DSC signature verification for Sub-AUA";
                                break;
                            case "E-235":
                                responseObj.ErrorMsg = "Request Signature Verification Failed";
                                break;
                            case "E-236":
                                responseObj.ErrorMsg = "Error during Request Signature Verification";
                                break;
                            case "E-237":
                                responseObj.ErrorMsg = "Error during KYC Response Encryption";
                                break;
                            case "E-238":
                                responseObj.ErrorMsg = "Inernal kua not mapped against KUA code";
                                break;
                            case "E-252":
                                responseObj.ErrorMsg = "Invalid Aadhaar no. length";
                                break;
                            case "E-253":
                                responseObj.ErrorMsg = "Biometric attribute is null/missing";
                                break;
                            case "E-297":
                                responseObj.ErrorMsg = "UIDAI PID Block Encryption Certificate Expired";
                                break;
                            case "E-299":
                                responseObj.ErrorMsg = "As per UIDAI guidelines, this device is not authorized to perform transactions. Kindly procure L1 registered device";
                                break;
                            case "E-555":
                                responseObj.ErrorMsg = "Duplicate transaction id error";
                                break;
                            case "E-563":
                                responseObj.ErrorMsg = "Read timeout while connecting to UIDAI";
                                break;
                            case "E-601":
                                responseObj.ErrorMsg = "SubAua Request XML Not Parsed Properly.";
                                break;
                            case "E-602":
                                responseObj.ErrorMsg = "SubAua XSD Validation Failed.";
                                break;
                            case "E-603":
                                responseObj.ErrorMsg = "Audit Logging in DB is failed for SubAua Request.";
                                break;
                            case "E-604":
                                responseObj.ErrorMsg = "License Verfication failed for SubAua Entity.";
                                break;
                            case "E-605":
                                responseObj.ErrorMsg = "SubAua Request Signature Verification Failed.";
                                break;
                            case "E-607":
                                responseObj.ErrorMsg = "DBAudit Failed For AuditOTP Request.";
                                break;
                            case "E-608":
                                responseObj.ErrorMsg = "Purpose not mapped against entity/Purpose used is not in active state.";
                                break;
                            case "E-609":
                                responseObj.ErrorMsg = "Entity-Service Mapping Verfication failed for SubAua Entity.";
                                break;
                            case "E-610":
                                responseObj.ErrorMsg = "RDData Received Null.";
                                break;

                        }
                    }
                    SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, fullResponse, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
                return responseObj;
            }
            catch (WebException we)
            {
                string Response = "";
                string status = Convert.ToString(we.Status);
                string WebResponse = Convert.ToString(we.Response);
                HttpWebResponse HttpWebResponse = (HttpWebResponse)we.Response;
                string WebResponseMessage = Convert.ToString(we.Message);
                if (we.Response != null)
                {
                    try
                    {
                        using (var stream = we.Response.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            Response = reader.ReadToEnd();
                        }
                        Response = Response.Replace("\u0000", "");
                        Response = Response.Replace("\\u0000", "");
                        dynamic res = JsonConvert.DeserializeObject(Response);

                        if (Convert.ToString(res.Status) == "Fail")
                        {
                            responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                            responseObj.Status = Convert.ToString(res.Status);
                            responseObj.txn = "";
                            string StatusCode = Convert.ToString(res.StatusCode);
                            responseObj.StatusCode = StatusCode;
                            responseObj.StatusMsg = Convert.ToString(res.ConnectionError);
                            responseObj.ErrorMsg = Convert.ToString(res.ErrorMessage);
                        }
                        SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, Response, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
                    }
                    finally
                    {
                    }
                }
                else if (status != "")
                {
                    responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
                    responseObj.Status = status;
                    responseObj.txn = "";
                    responseObj.StatusCode = Convert.ToString(HttpStatusCode.InternalServerError);
                    responseObj.StatusMsg = WebResponse;
                    responseObj.ErrorMsg = WebResponseMessage;
                }
                return responseObj;
            }
            finally
            {
            }
        }
        #endregion

        #region eIBMAadhaar
        public IBMAadhaarFullResponse IBMAadhaarDownload(eIBMAadhaar eIBMAadhaar)
        {
            string sourceId = "FT";
            DateTime d = DateTime.Now;
            string dateString = d.ToString("yyyyMMddHHmmss");
            string vRndNo = GenerateRandomNo();
            string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"            
            string type = "A";
            string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 
            IBMAadhaarFullResponse responseObj = null;

            Kyc pKycReq = new Kyc();
            pKycReq.lr = "N";
            pKycReq.pfr = "N";

            eIBMAadhaarReq pReq = new eIBMAadhaarReq();
            pReq.sourceId = sourceId;
            pReq.traceId = traceId;
            pReq.uid = eIBMAadhaar.aadhaarNo;
            pReq.txn = "UKC:" + eIBMAadhaar.txn;
            pReq.type = type;
            pReq.ts = ts;
            pReq.otp = eIBMAadhaar.otp;
            pReq.Kyc = pKycReq;

            string requestBody = JsonConvert.SerializeObject(pReq);
            string postURL = IBMAadhaarUrl + "/aadharKYC";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                try
                {
                    fullResponse = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                    fullResponse = fullResponse.Trim('"');
                    responseObj = JsonConvert.DeserializeObject<IBMAadhaarFullResponse>(fullResponse);
                    #region comment
                    if (Convert.ToString(responseObj.status) == "success")
                    {
                        responseObj.StatusMsg = "Aadhaar Verified Successfully";

                    }
                    else if (Convert.ToString(responseObj.status) == "fail")
                    {
                        string err = Convert.ToString(responseObj.err);
                        responseObj.StatusCode = err;
                        responseObj.StatusMsg = "Aadhaar Not Verified Successfully";

                        switch (err)
                        {
                            case "K-100":
                                responseObj.ErrorMessage = "Resident authentication failed";
                                break;
                            case "K-200":
                                responseObj.ErrorMessage = "Resident data currently not available";
                                break;
                            case "K-514":
                                responseObj.ErrorMessage = "Invalid UID Token Used.";
                                break;
                            case "K-515":
                                responseObj.ErrorMessage = "Invalid VID used.";
                                break;
                            case "K-516":
                                responseObj.ErrorMessage = "Invalid ANCS Token used.";
                                break;
                            case "K-517":
                                responseObj.ErrorMessage = "VID used is expired.";
                                break;
                            case "K-519":
                                responseObj.ErrorMessage = "Invalid Authenticator Code.";
                                break;
                            case "K-540":
                                responseObj.ErrorMessage = "Invalid KYC XML";
                                break;
                            case "K-541":
                                responseObj.ErrorMessage = "Invalid e-KYC API version";
                                break;
                            case "K-542":
                                responseObj.ErrorMessage = "Invalid resident consent (“rc” attribute in “Kyc” element)";
                                break;
                            case "K-544":
                                responseObj.ErrorMessage = "Invalid resident auth type (“ra” attribute in “Kyc” element does not match what is in PID block)";
                                break;
                            case "K-545":
                                responseObj.ErrorMessage = "Resident has opted-out of this service. This feature is not implemented currently.";
                                break;
                            case "K-546":
                                responseObj.ErrorMessage = "Invalid value for “pfr” attribute";
                                break;
                            case "K-550":
                                responseObj.ErrorMessage = "Invalid Uses Attribute";
                                break;
                            case "K-551":
                                responseObj.ErrorMessage = "Invalid “Txn” namespace";
                                break;
                            case "K-552":
                                responseObj.ErrorMessage = "Invalid KUA License key";
                                break;
                            case "K-553":
                                responseObj.ErrorMessage = "KUA License key Expired.";
                                break;
                            case "K-569":
                                responseObj.ErrorMessage = "Digital signature verification failed for e-KYC XML";
                                break;
                            case "K-570":
                                responseObj.ErrorMessage = "Invalid key info in digital signature for e-KYC XML (it is either expired, or does not belongto the AUA or is not created by a well-known Certification Authority)";
                                break;
                            case "K-571":
                                responseObj.ErrorMessage = "Technical error while signing the eKYC response.";
                                break;
                            case "K-600":
                                responseObj.ErrorMessage = "AUA is invalid or not an authorized KUA";
                                break;
                            case "K-601":
                                responseObj.ErrorMessage = "ASA is invalid or not an authorized ASA";
                                break;
                            case "K-602":
                                responseObj.ErrorMessage = "KUA encryption key not available";
                                break;
                            case "K-603":
                                responseObj.ErrorMessage = "ASA encryption key not available";
                                break;
                            case "K-604":
                                responseObj.ErrorMessage = "ASA Signature not allowed";
                                break;
                            case "K-605":
                                responseObj.ErrorMessage = "Neither KUA nor ASA encryption key is available";
                                break;
                            case "K-955":
                                responseObj.ErrorMessage = "Technical Failure internal to UIDAI.";
                                break;
                            case "K-956":
                                responseObj.ErrorMessage = "Technical error while generating the PDF file.";
                                break;
                            case "K-999":
                                responseObj.ErrorMessage = "Unknown error";
                                break;
                        }
                    }
                    else if (Convert.ToString(responseObj.NSDL_Error_Code) != "")
                    {
                        responseObj.status = "fail";
                        string NSDL_Error_Code = Convert.ToString(responseObj.NSDL_Error_Code);
                        responseObj.StatusCode = NSDL_Error_Code;
                        responseObj.StatusMsg = "Failure from NSDL Scenario";

                        switch (NSDL_Error_Code)
                        {
                            case "E-000":
                                responseObj.ErrorMessage = "Request received is a HTTP request";
                                break;
                            case "E-001":
                                responseObj.ErrorMessage = "Request received is a get request";
                                break;
                            case "E-100":
                                responseObj.ErrorMessage = "Auth XML not parsed properly";
                                break;
                            case "E-101":
                                responseObj.ErrorMessage = "KYC XML not parsed properly";
                                break;
                            case "E-102":
                                responseObj.ErrorMessage = "Audit logging in DB failed for request";
                                break;
                            case "E-103":
                                responseObj.ErrorMessage = "Audit logging in DB failed for response";
                                break;
                            case "E-105":
                                responseObj.ErrorMessage = "KYC XSD Validation failed";
                                break;
                            case "E-106":
                                responseObj.ErrorMessage = "KYC Request signature verification failed";
                                break;
                            case "E-107":
                                responseObj.ErrorMessage = "Auth Request signature verification failed";
                                break;
                            case "E-108":
                                responseObj.ErrorMessage = "IP verification failed for entity";
                                break;
                            case "E-109":
                                responseObj.ErrorMessage = "Blank response received from UIDAI";
                                break;
                            case "E-110":
                                responseObj.ErrorMessage = "Unable to decrypt response at KSA";
                                break;
                            case "E-111":
                                responseObj.ErrorMessage = "KYC response signature verification failed";
                                break;
                            case "E-112":
                                responseObj.ErrorMessage = "BFD XSD validation failed";
                                break;
                            case "E-113":
                                responseObj.ErrorMessage = "OTP XSD validation failed";
                                break;
                            case "E-114":
                                responseObj.ErrorMessage = "KYC response XML not parsed properly";
                                break;
                            case "E-115":
                                responseObj.ErrorMessage = "AUTH response XML not parsed properly";
                                break;
                            case "E-116":
                                responseObj.ErrorMessage = "Signed Auth XML generation Error";
                                break;
                            case "E-117":
                                responseObj.ErrorMessage = "Signed KYC XML generation Error";
                                break;
                            case "E-118":
                                responseObj.ErrorMessage = "Auth response signature verification failed";
                                break;
                            case "E-119":
                                responseObj.ErrorMessage = "ASA or KSA is unable to connect to UIDAI server";
                                break;
                            case "E-120":
                                responseObj.ErrorMessage = "Auth XSD validation failed";
                                break;
                            case "E-122":
                                responseObj.ErrorMessage = "Property file unavailable";
                                break;
                            case "E-123":
                                responseObj.ErrorMessage = "BFD request XML not parsed properly";
                                break;
                            case "E-124":
                                responseObj.ErrorMessage = "OTP request XML not parsed properly";
                                break;
                            case "E-125":
                                responseObj.ErrorMessage = "BFD request signature verification failed";
                                break;
                            case "E-126":
                                responseObj.ErrorMessage = "OTP request signature verification failed";
                                break;
                            case "E-127":
                                responseObj.ErrorMessage = "Signed BFD XML generation error";
                                break;
                            case "E-128":
                                responseObj.ErrorMessage = "Signed OTP XML generation error";
                                break;
                            case "E-129":
                                responseObj.ErrorMessage = "BFD response XML not parsed properly";
                                break;
                            case "E-130":
                                responseObj.ErrorMessage = "OTP response XML not parsed properly";
                                break;
                            case "E-131":
                                responseObj.ErrorMessage = "XML decryption error";
                                break;
                            case "E-132":
                                responseObj.ErrorMessage = "Error during KYC request signature verification";
                                break;
                            case "E-133":
                                responseObj.ErrorMessage = "Error during KYC response signature verification";
                                break;
                            case "E-134":
                                responseObj.ErrorMessage = "Error during AUTH request signature verification";
                                break;
                            case "E-135":
                                responseObj.ErrorMessage = "Error during AUTH response signature verification";
                                break;
                            case "E-136":
                                responseObj.ErrorMessage = "Error during BFD request signature verification";
                                break;
                            case "E-137":
                                responseObj.ErrorMessage = "Error during OTP request signature verification";
                                break;
                            case "E-138":
                                responseObj.ErrorMessage = "Error during KYC XSD Validation";
                                break;
                            case "E-139":
                                responseObj.ErrorMessage = "Error during AUTH XSD Validation";
                                break;
                            case "E-140":
                                responseObj.ErrorMessage = "Error during BFD XSD Validation";
                                break;
                            case "E-141":
                                responseObj.ErrorMessage = "Error during OTP XSD Validation";
                                break;
                            case "E-142":
                                responseObj.ErrorMessage = "Error during IP verification";
                                break;
                            case "E-143":
                                responseObj.ErrorMessage = "Response received is E";
                                break;
                            case "E-144":
                                responseObj.ErrorMessage = "BFD response signature verification failed";
                                break;
                            case "E-145":
                                responseObj.ErrorMessage = "OTP Signature Tag Missing in Request XML";
                                break;
                            case "E-149":
                                responseObj.ErrorMessage = "Invalid Aadhar Number";
                                break;
                            case "E-199":
                                responseObj.ErrorMessage = "KSA/ASA Internal Error";
                                break;
                            case "E-200":
                                responseObj.ErrorMessage = "One of the mandatory Sub-Aua element is null";
                                break;
                            case "E-201":
                                responseObj.ErrorMessage = "Error while validating strSaCd parameter";
                                break;
                            case "E-203":
                                responseObj.ErrorMessage = "Error while validating strAadhaar parameter";
                                break;
                            case "E-204":
                                responseObj.ErrorMessage = "Error while validating strAadhaarName parameter";
                                break;
                            case "E-205":
                                responseObj.ErrorMessage = "Error while validating strYear parameter";
                                break;
                            case "E-206":
                                responseObj.ErrorMessage = "Error while validating strGender parameter";
                                break;
                            case "E-207":
                                responseObj.ErrorMessage = "Error while validating strTransId parameter";
                                break;
                            case "E-208":
                                responseObj.ErrorMessage = "Error while validating strUDC parameter";
                                break;
                            case "E-209":
                                responseObj.ErrorMessage = "Error while validating strMV parameter";
                                break;
                            case "E-210":
                                responseObj.ErrorMessage = "Value of mvThreshold in property file should be in range 1-100";
                                break;
                            case "E-211":
                                responseObj.ErrorMessage = "Error while validating strLang: If strLname is provided then strLang is mandatory";
                                break;
                            case "E-212":
                                responseObj.ErrorMessage = "Error while validating strLmv parameter. Value should be in range 1-100(inclusive)";
                                break;
                            case "E-213":
                                responseObj.ErrorMessage = "DSC signature verification failed for Sub-AUA";
                                break;
                            case "E-214":
                                responseObj.ErrorMessage = "Error while validating strOtp parameter";
                                break;
                            case "E-215":
                                responseObj.ErrorMessage = "Error while validating strRequestType parameter";
                                break;
                            case "E-216":
                                responseObj.ErrorMessage = "Error while validating strBioType parameter";
                                break;
                            case "E-217":
                                responseObj.ErrorMessage = "Error while validating strFingerOnePos parameter";
                                break;
                            case "E-218":
                                responseObj.ErrorMessage = "Error while validating strFingerTwoPos parameter";
                                break;
                            case "E-219":
                                responseObj.ErrorMessage = "Error while validating strMS parameter";
                                break;
                            case "E-220":
                                responseObj.ErrorMessage = "Blank Signature Tag Exception";
                                break;
                            case "E-221":
                                responseObj.ErrorMessage = "Incorrect OTP Version";
                                break;
                            case "E-230":
                                responseObj.ErrorMessage = "Error during DSC signature verification for Sub-AUA";
                                break;
                            case "E-235":
                                responseObj.ErrorMessage = "Request Signature Verification Failed";
                                break;
                            case "E-236":
                                responseObj.ErrorMessage = "Error during Request Signature Verification";
                                break;
                            case "E-237":
                                responseObj.ErrorMessage = "Error during KYC Response Encryption";
                                break;
                            case "E-238":
                                responseObj.ErrorMessage = "Inernal kua not mapped against KUA code";
                                break;
                            case "E-252":
                                responseObj.ErrorMessage = "Invalid Aadhaar no. length";
                                break;
                            case "E-253":
                                responseObj.ErrorMessage = "Biometric attribute is null/missing";
                                break;
                            case "E-297":
                                responseObj.ErrorMessage = "UIDAI PID Block Encryption Certificate Expired";
                                break;
                            case "E-299":
                                responseObj.ErrorMessage = "As per UIDAI guidelines, this device is not authorized to perform transactions. Kindly procure L1 registered device";
                                break;
                            case "E-555":
                                responseObj.ErrorMessage = "Duplicate transaction id error";
                                break;
                            case "E-563":
                                responseObj.ErrorMessage = "Read timeout while connecting to UIDAI";
                                break;
                            case "E-601":
                                responseObj.ErrorMessage = "SubAua Request XML Not Parsed Properly.";
                                break;
                            case "E-602":
                                responseObj.ErrorMessage = "SubAua XSD Validation Failed.";
                                break;
                            case "E-603":
                                responseObj.ErrorMessage = "Audit Logging in DB is failed for SubAua Request.";
                                break;
                            case "E-604":
                                responseObj.ErrorMessage = "License Verfication failed for SubAua Entity.";
                                break;
                            case "E-605":
                                responseObj.ErrorMessage = "SubAua Request Signature Verification Failed.";
                                break;
                            case "E-607":
                                responseObj.ErrorMessage = "DBAudit Failed For AuditOTP Request.";
                                break;
                            case "E-608":
                                responseObj.ErrorMessage = "Purpose not mapped against entity/Purpose used is not in active state.";
                                break;
                            case "E-609":
                                responseObj.ErrorMessage = "Entity-Service Mapping Verfication failed for SubAua Entity.";
                                break;
                            case "E-610":
                                responseObj.ErrorMessage = "RDData Received Null.";
                                break;

                        }
                    }
                    #endregion
                    SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, fullResponse, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
                }
                finally
                {
                }
                return responseObj;
            }

            catch (WebException we)
            {
                string Response = "";
                string status = Convert.ToString(we.Status);
                string WebResponse = Convert.ToString(we.Response);
                string WebResponseMessage = Convert.ToString(we.Message);
                if (we.Response != null)
                {
                    using (var stream = we.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        Response = reader.ReadToEnd();
                    }
                    try
                    {
                        Response = Response.Replace("\u0000", "");
                        Response = Response.Replace("\\u0000", "");
                        dynamic res = JsonConvert.DeserializeObject(Response);

                        if (Convert.ToString(res.Status) == "Fail")
                        {
                            responseObj.status = "fail";
                            responseObj.StatusCode = Convert.ToString(res.StatusCode);
                            responseObj.ErrorReason = Convert.ToString(res.ErrorReason);
                            responseObj.FailedAt = Convert.ToString(res.FailedAt);
                            responseObj.ErrorName = Convert.ToString(res.ErrorName);
                            responseObj.ErrorCode = Convert.ToString(res.ErrorCode);
                            responseObj.ErrorMessage = Convert.ToString(res.ErrorMessage);
                        }
                        SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, Response, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
                    }
                    finally
                    {
                    }
                }
                else if (status != "")
                {
                    responseObj.status = WebResponse;
                    responseObj.StatusCode = status;
                    responseObj.ErrorMessage = WebResponseMessage;
                }
                return responseObj;
            }
            finally
            {
            }
        }
        #endregion

        #region SaveIBMAadhaarOtp
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
        #endregion

        #region GenerateRandomNo
        public string GenerateRandomNo()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new Random();

            return rdm.Next(min, max).ToString();
        }
        #endregion

        #region GetBranchCtrlByBranchCode
        public List<GetBranchCtrlByBranchCode> GetBranchCtrlByBranchCode(PostBranchCtrlByBranchCode postBranchCtrlByBranchCode)
        {
            CRepository oCR = new CRepository();
            DataTable dt = new DataTable();
            List<GetBranchCtrlByBranchCode> row = new List<GetBranchCtrlByBranchCode>();

            try
            {
                dt = oCR.GetBranchCtrlByBranchCode(postBranchCtrlByBranchCode.pBranchCode, postBranchCtrlByBranchCode.pEffectiveDate);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetBranchCtrlByBranchCode(rs["AdvCollMEL"].ToString(), rs["CashCollMEL"].ToString(), rs["DigiAuthMEL"].ToString(), rs["ManualAuthMEL"].ToString(), rs["BioAuthMEL"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetBranchCtrlByBranchCode("No data available", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetBranchCtrlByBranchCode("No data available", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }



        #endregion
    }
}
