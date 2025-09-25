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
using CentrumMobService.Service_Equifax_UAT;
using System.Net;
using System.Xml;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Security;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web;
using CentrumMobService.Service_Equifax;
using System.ServiceModel.Web;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Net.Mail;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Xml.Serialization;

namespace CentrumMobService
{
    public class CentrumService : ICentrumService
    {
        public static readonly string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
        public static readonly long Base = Alphabet.Length;
        private static readonly string CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        #region WebConfig Variable
        string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        string vAccessTime = ConfigurationManager.AppSettings["AccessTime"];
        string vCompMailID = ConfigurationManager.AppSettings["CompMailID"];
        string vPswd = ConfigurationManager.AppSettings["Pswd"];
        string vToMailId = ConfigurationManager.AppSettings["ToMailId"];

        string InitialBucket = ConfigurationManager.AppSettings["InitialBucket"];
        string MemberBucket = ConfigurationManager.AppSettings["MemberBucket"];
        string GroupBucket = ConfigurationManager.AppSettings["GroupBucket"];
        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string LUCBucket = ConfigurationManager.AppSettings["LUCBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string GroupImgPath = ConfigurationManager.AppSettings["GroupImgPath"];
        string InitialImgPath = ConfigurationManager.AppSettings["InitialImgPath"];
        string ServerIP = ConfigurationManager.AppSettings["ServerIP"];
        string IdfyWebHook = ConfigurationManager.AppSettings["IdfyWebHook"];

        string API_key = ConfigurationManager.AppSettings["API_key"];
        string Account_ID = ConfigurationManager.AppSettings["Account_ID"];
        string user_key = ConfigurationManager.AppSettings["user_key"];
        string PosidexEncURL = ConfigurationManager.AppSettings["PosidexEncURL"];
        string DynamicQRUrl = ConfigurationManager.AppSettings["DynamicQRUrl"];


        string X_IBM_Client_Id = ConfigurationManager.AppSettings["X_IBM_Client_Id"];
        string X_IBM_Client_Secret = ConfigurationManager.AppSettings["X_IBM_Client_Secret"];
        string X_Client_IP = ConfigurationManager.AppSettings["X_Client_IP"];

        string IBMAadhaarUrl = ConfigurationManager.AppSettings["IBMAadhaarUrl"];

        #endregion

        #region AppVersion
        public string GetAppVersion(AppVersionData appVersionData)
        {
            try
            {
                string VersionCode = ConfigurationManager.AppSettings["MobAppVersionCode"];

                if (Convert.ToInt32(VersionCode) > Convert.ToInt32(appVersionData.pVersion))
                {
                    return "https://centrummob.bijliftt.com/Unity.apk";
                }
                else
                {
                    return "No updates available";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region AES Encrypt
        public byte[] AesEncrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMobileNo", pMobileNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", pOTP);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUserName", pUserName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pOTPId", 0);
                DBConnect.Execute(oCmd);
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

        #region MobUser
        public List<LoginData> GetMobUser(UserData userData)
        {
            string vAttFlag = ConfigurationManager.AppSettings["AttFlag"];

            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<EmpData> row1 = new List<EmpData>();
            List<PermissionData> row2 = new List<PermissionData>();
            List<LoginData> rowFinal = new List<LoginData>();

            userData.pEncYN = userData.pEncYN == null ? "N" : userData.pEncYN;
            // string vX_Key = RsaDecrypt(userData.pKey);

            //-----------08.07.2025---------------
            string vX_Key = RsaDecrypt(userData.pKey);
            byte[] xKey = Convert.FromBase64String(vX_Key);
            userData.pPassword = Decrypt(userData.pPassword, xKey);
            //-----------------------------------

            //#region AES Decrypt
            //if (userData.pEncYN == "Y")
            //{
            //    String key = "Force@2301***DB";
            //    var encryptedBytes = Convert.FromBase64String(userData.pPassword);
            //    userData.pPassword = Encoding.UTF8.GetString(AesDecrypt(encryptedBytes, GetRijndaelManaged(key)));
            //}
            //#endregion

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetUserDtl";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUser", userData.pUserName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, userData.pPassword.Length + 1, "@pPass", userData.pPassword);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", userData.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", userData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pImeiNo1", userData.pImeiNo1);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pImeiNo2", userData.pImeiNo2);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVersionCode", userData.pVersionCode);
                ds = DBConnect.GetDataSet(oCmd);

                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                if (dt1.Rows.Count == 1)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        row1.Add(new EmpData(rs["EoName"].ToString(), rs["BranchCode"].ToString(), rs["UserID"].ToString(), rs["Eoid"].ToString(), rs["LogStatus"].ToString(),
                            rs["DayEndDate"].ToString(), rs["AttStatus"].ToString(), vAttFlag, rs["AreaCat"].ToString(), rs["Designation"].ToString(), rs["AllowGalleryAccess"].ToString(),
                            rs["ActivateApkAccess"].ToString(), rs["AllowManualEntry"].ToString(), rs["AllowQRScan"].ToString(), rs["AreaCategoryId"].ToString(),
                            rs["BCBranchYN"].ToString(), rs["BranchName"].ToString(), rs["ThirdWeekNotAllow"].ToString(), rs["LoginId"].ToString(),
                            rs["AllowAdvYN"].ToString(), rs["MFAYN"].ToString(), rs["MFAOTP"].ToString(), rs["ImgMaskingYN"].ToString(),
                            rs["DialogToImageYN"].ToString()));
                        if (rs["MFAYN"].ToString() == "Y")
                        {
                            SendMFAOTP(rs["MFAOTP"].ToString(), rs["MobileNo"].ToString());
                        }
                    }
                }
                else
                {
                    row1.Add(new EmpData("", "", "", "", "Login Failed", "", "", "N", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
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
                oCmd.Dispose();
            }
            return rowFinal;
        }
        #endregion

        #region KYCInfo
        public List<KYCData> GetKYCInfo(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<KYCData> row = new List<KYCData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetListItem";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postKYCData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", postKYCData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pType", postKYCData.pType);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new KYCData(rs["ItemID"].ToString(), rs["ItemName"].ToString(), rs["ItemType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new KYCData("No data available", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new KYCData("No data available", ex.ToString(), ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region WorkAllocInfo
        public List<WorkAllocData> GetWorkAllocInfo(PostKYCData WorkAllocationData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<WorkAllocData> row = new List<WorkAllocData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetWorkAlloc";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", WorkAllocationData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", WorkAllocationData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", WorkAllocationData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new WorkAllocData(rs["EOID"].ToString(), rs["EoName"].ToString(), rs["UserID"].ToString()));
                    }
                }
                else
                {
                    row.Add(new WorkAllocData("No data available", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new WorkAllocData("No data available", ex.ToString(), ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region AddressDtl
        public List<AddressData> GetAddressDtl(PostAddressData postAddressData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<AddressData> row = new List<AddressData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetAddressDtl";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", postAddressData.pBranch);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new AddressData(rs["VillageId"].ToString(), rs["VillageName"].ToString(), rs["GPId"].ToString(), rs["GPName"].ToString(),
                            rs["BlockId"].ToString(), rs["BlockName"].ToString(), rs["DistrictId"].ToString(), rs["DistrictName"].ToString(),
                            rs["StateId"].ToString(), rs["StateName"].ToString(), rs["ShortName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new AddressData("No data available", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new AddressData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region GetStateList
        public List<StateData> GetStateList()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<StateData> row = new List<StateData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetStateList";
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new StateData(rs["StateId"].ToString(), rs["StateName"].ToString(), rs["ShortName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new StateData("No data available", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new StateData("No data available", ex.ToString(), ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region SaveGroup
        public string SaveGroup(PostGroupData postGroupData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveGroup";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 16, "@pGroupid", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMarketID", postGroupData.pMktId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 12, "@pDOF", postGroupData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pGroupName", postGroupData.pGroupName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10, "@pGroupCode", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pGroupLeader", postGroupData.pLeaderName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pPhNo1", postGroupData.pLeaderContact);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pPhNo2", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVillageID", Convert.ToInt32(postGroupData.pVillageId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pPIN", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 12, "@pClosingDt", "1900-01-01");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pClType", "-");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRemarks", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postGroupData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pFormedBy", postGroupData.pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postGroupData.pUserId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Save");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pMobId", postGroupData.pMobId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                DBConnect.Execute(oCmd);

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
        #endregion

        #region SaveKYC
        public string SaveKYC_Old(PostKYCSaveData postKYCSaveData)
        {
            SqlCommand oCmd1 = new SqlCommand();
            SqlCommand oCmd2 = new SqlCommand();
            Int32 vErr1 = 0, vErr2 = 0, pCbId = 0;
            string pErrDesc = "", pEnqId = "XX", vMemDob = string.Empty, aadhaarId = "", voterId = "", relationCode = "", pEqXml = "", equifaxResponse = "", vErrMsg = "";
            EquifaxService.WSEquifaxMCRSoapClient eq = new EquifaxService.WSEquifaxMCRSoapClient();
            vMemDob = postKYCSaveData.pDob.Substring(5, 2) + "/" + postKYCSaveData.pDob.Substring(8, 2) + "/" + postKYCSaveData.pDob.Substring(0, 4);

            int vErr3 = ChkDdupMEL(postKYCSaveData.pId1No, postKYCSaveData.pId2No, postKYCSaveData.pId3No, ref vErrMsg);
            if (vErr3 == 0)
            {
                try
                {
                    oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd1.CommandText = "SaveInitialApproach";
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 16, "@pEnquiryId", postKYCSaveData.pEnquiryId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCBId", Convert.ToInt32(postKYCSaveData.pCbId));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pGroupID", postKYCSaveData.pGroupId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanAmount", Convert.ToDouble(postKYCSaveData.pLoanAmount));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pFirstName", postKYCSaveData.pFirstName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMiddleName", postKYCSaveData.pMiddleName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLastName", postKYCSaveData.pLastName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", postKYCSaveData.pDob);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAGE", Convert.ToInt32(postKYCSaveData.pAge));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRelativeName", postKYCSaveData.pGuardianName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(postKYCSaveData.pGuardianRelation));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIdProofType", Convert.ToInt32(postKYCSaveData.pId1Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pIdProofNo", postKYCSaveData.pId1No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType", Convert.ToInt32(postKYCSaveData.pId2Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo", postKYCSaveData.pId2No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType2", Convert.ToInt32(postKYCSaveData.pId3Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo2", postKYCSaveData.pId3No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType1", Convert.ToInt32(postKYCSaveData.pAddressType1));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo1", postKYCSaveData.pHouseNo1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet1", postKYCSaveData.pStreet1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea1", postKYCSaveData.pArea1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pVillage1", postKYCSaveData.pVillage1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict1", postKYCSaveData.pSubDistrict1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pDistrict1", postKYCSaveData.pDistrict1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pState1", postKYCSaveData.pState1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark1", postKYCSaveData.pLandMark1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice1", postKYCSaveData.pPostOffice1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode1", postKYCSaveData.pPinCode1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo1", postKYCSaveData.pMobileNo1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmailId1", postKYCSaveData.pEmailId1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType2", Convert.ToInt32(postKYCSaveData.pAddressType2));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo2", postKYCSaveData.pHouseNo2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet2", postKYCSaveData.pStreet2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea2", postKYCSaveData.pArea2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVillageId2", Convert.ToInt32(postKYCSaveData.pVillageId2));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict2", postKYCSaveData.pSubDistrict2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark2", postKYCSaveData.pLandMark2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice2", postKYCSaveData.pPostOffice2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode2", postKYCSaveData.pPinCode2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo2", postKYCSaveData.pMobileNo2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmailId2", postKYCSaveData.pEmailId2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postKYCSaveData.pBranchCode);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCSaveData.pEoId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAadhaarScan", postKYCSaveData.pAadhaarScan);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", postKYCSaveData.pMode);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pISGroupSame", postKYCSaveData.pISGroupSame);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplicationType", postKYCSaveData.pApplicationType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMktId", postKYCSaveData.pMktId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEnqType", postKYCSaveData.EnqType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", postKYCSaveData.pOTP);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTimeStamp", postKYCSaveData.pTimeStamp);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOCRApproveYN", postKYCSaveData.pOCRApproveYN);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pKarzaVerifyYN", postKYCSaveData.pKarzaVerifyYN);
                    DBConnect.Execute(oCmd1);
                    vErr1 = Convert.ToInt32(oCmd1.Parameters["@pErr"].Value);
                    pErrDesc = Convert.ToString(oCmd1.Parameters["@pErrDesc"].Value);
                    pEnqId = Convert.ToString(oCmd1.Parameters["@pEnquiryId"].Value);
                    pCbId = Convert.ToInt32(oCmd1.Parameters["@pCBId"].Value);

                    if (vErr1 == 0)
                    {
                        if (postKYCSaveData.pId1Type.Equals("1"))
                        {
                            aadhaarId = postKYCSaveData.pId1No;
                        }
                        else if (postKYCSaveData.pId2Type.Equals("1"))
                        {
                            aadhaarId = postKYCSaveData.pId2No;
                        }
                        else
                        {
                            aadhaarId = "";
                        }

                        if (postKYCSaveData.pId1Type.Equals("3"))
                        {
                            voterId = postKYCSaveData.pId1No;
                        }
                        else if (postKYCSaveData.pId2Type.Equals("3"))
                        {
                            voterId = postKYCSaveData.pId2No;
                        }
                        else
                        {
                            voterId = postKYCSaveData.pId2No;
                        }

                        relationCode = GetRelationCode(postKYCSaveData.pGuardianRelation);

                        try
                        {
                            if (postKYCSaveData.pTyp == "E")
                            {
                                if (postKYCSaveData.pOCRApproveYN == "Y" && postKYCSaveData.pKarzaVerifyYN == "Y")
                                // if (postKYCSaveData.pOCRApproveYN == "Y")
                                {
                                    //-------------------------------------------LIVE---------------------------------------------
                                    pEqXml = eq.EquifaxVerification(postKYCSaveData.pFullName, postKYCSaveData.pGuardianName, relationCode, postKYCSaveData.pAddress,
                                        postKYCSaveData.pStateShortName, postKYCSaveData.pPinCode1, vMemDob, voterId, aadhaarId,
                                        5750, "STSCENTRUM", "hg*uy56GF", "027FZ01546", "KQ7", "MCR", "1.0", "1234", "Yes", "MFI", postKYCSaveData.pMobileNo1);
                                    //------------------------------------------TEST-----------------------------------------------
                                    //    //wsEquiFaxSoapClient eq1 = new wsEquiFaxSoapClient();
                                    //    //pEqXml = eq1.EquifaxVerification(postKYCSaveData.pFullName, postKYCSaveData.pGuardianName, relationCode, postKYCSaveData.pAddress,
                                    //    //    postKYCSaveData.pStateShortName, postKYCSaveData.pPinCode1, vMemDob, voterId, aadhaarId,
                                    //    //           21, "UAT_CENT", "7Vf7Xa*3", "007FZ00016", "YT9", "MCR", "1.0", "1234", "Yes", "MFI");
                                    //---------------------------------------------------------------------------------------------
                                    if (pEqXml.Equals(""))
                                    {
                                        oCmd2.CommandType = CommandType.StoredProcedure;
                                        oCmd2.CommandText = "UpdateEquifaxInformation";
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "Equifax File Not Found");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                                        DBConnect.Execute(oCmd2);
                                        vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                                        equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);

                                        if (vErr2 == 1)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else if (vErr2 == 5)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                    }
                                    else
                                    {
                                        pEqXml = pEqXml.Replace("<?xml version=\"1.0\"?>", "").Trim();
                                        pEqXml = pEqXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                                        pEqXml = pEqXml.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");

                                        oCmd2.CommandType = CommandType.StoredProcedure;
                                        oCmd2.CommandText = "UpdateEquifaxInformation";
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                                        DBConnect.Execute(oCmd2);
                                        vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                                        equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);

                                        if (vErr2 == 1)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Successful" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else if (vErr2 == 5)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                    }
                                }
                                else
                                {
                                    return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Hold for OCR Data mismatch " + ":" + equifaxResponse + ":" + pCbId;
                                }
                            }
                            else
                            {
                                //    DataTable dt = new DataTable();
                                //    SqlCommand oCmd = new SqlCommand();
                                //    oCmd.CommandType = CommandType.StoredProcedure;
                                //    oCmd.CommandText = "GetMemberEquifaxInfo";
                                //    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnqId", pEnqId);
                                //    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pType", "H");
                                //    DBConnect.ExecuteForSelect(oCmd, dt);
                                //    if (dt.Rows.Count > 0)
                                //    {
                                //        try
                                //        {
                                //            ServicePointManager.Expect100Continue = true;
                                //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                                //            string responsedata = string.Empty;
                                //            string userId = "in_cpuuat_cenmfi3";
                                //            string password = "9DDB522C741D8F3192D53B13D9FB590B02723024";
                                //            string mbrid = "MFI0000228";
                                //            string productType = "INDV";
                                //            string productVersion = "1.0";
                                //            string reqVolType = "INDV";
                                //            string SUB_MBR_ID = "CENTRUM MICROCREDIT PRIVATE LIMITED";

                                //            string request_datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                                //            string vAppDate = postKYCSaveData.pDate;
                                //            string vAadhar = "", vVoterId = "";
                                //            vVoterId = Convert.ToString(dt.Rows[0]["VoterId"]);
                                //            vAadhar = Convert.ToString(dt.Rows[0]["UID"]);

                                //            string HEADER_SEGMENT = "<HEADER-SEGMENT>"
                                //                                      + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                //                                      + "<INQ-DT-TM>" + request_datetime + "</INQ-DT-TM>"
                                //                                      + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                                //                                      + "<REQ-ACTN-TYP>SUBMIT</REQ-ACTN-TYP>"
                                //                                      + "<TEST-FLG>N</TEST-FLG>"
                                //                                      + "<AUTH-FLG>Y</AUTH-FLG>"
                                //                                      + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                //                                      + "<RES-FRMT>XML/HTML</RES-FRMT>"
                                //                                      + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                                //                                      + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                                //                                      + "<MFI>"
                                //                                      + "<INDV>TRUE</INDV>"
                                //                                      + "<SCORE>FALSE</SCORE>"
                                //                                      + "<GROUP>TRUE</GROUP>"
                                //                                      + "</MFI>"
                                //                                      + "<CONSUMER>"
                                //                                      + "<INDV>TRUE</INDV>"
                                //                                      + "<SCORE>TRUE</SCORE>"
                                //                                      + "</CONSUMER>"
                                //                                      + "<IOI>TRUE</IOI>"
                                //                                      + "</HEADER-SEGMENT>";

                                //            string APPLICANT_SEGMENT = "<APPLICANT-SEGMENT>"
                                //                                        + "<APPLICANT-NAME><NAME1>" + dt.Rows[0]["MemberName"] + "</NAME1>"
                                //                                        + "<NAME2></NAME2><NAME3></NAME3><NAME4></NAME4><NAME5></NAME5></APPLICANT-NAME>"
                                //                                        + "<DOB><DOB-DATE>" + dt.Rows[0]["DOB"] + "</DOB-DATE>"
                                //                                        + "<AGE>" + dt.Rows[0]["Age"] + "</AGE>"
                                //                                        + "<AGE-AS-ON>" + dt.Rows[0]["AsOnDate"] + "</AGE-AS-ON></DOB>"
                                //                                        + "<IDS>"
                                //                                        + "<ID><TYPE>ID03</TYPE><VALUE>" + vAadhar + "</VALUE></ID>" //aadhaar no                                                               
                                //                                        + ((vVoterId == "") ? "" : "<ID><TYPE>ID02</TYPE><VALUE>" + vVoterId + "</VALUE></ID>") //voter id (if available)                                                              
                                //                                        + "</IDS>"
                                //                                        + "<RELATIONS><RELATION><NAME>" + dt.Rows[0]["RelativeName"] + "</NAME><TYPE>" + dt.Rows[0]["RelationCode"] + "</TYPE>"
                                //                                        + "</RELATION></RELATIONS><PHONES><PHONE><TELE-NO>" + dt.Rows[0]["MobileNo"] + "</TELE-NO>"
                                //                                        + "<TELE-NO-TYPE>P03</TELE-NO-TYPE></PHONE></PHONES>"
                                //                                        + "</APPLICANT-SEGMENT>";

                                //            string ADDRESS_SEGMENT = "<ADDRESS-SEGMENT><ADDRESS><TYPE>D01</TYPE><ADDRESS-1>" + dt.Rows[0]["MemAdd"] + "</ADDRESS-1>"
                                //                                        + "<CITY>" + dt.Rows[0]["District"] + "</CITY><STATE>" + dt.Rows[0]["StateCode"] + "</STATE><PIN>"
                                //                                        + dt.Rows[0]["PIN"] + "</PIN></ADDRESS></ADDRESS-SEGMENT>";

                                //            string APPLICATION_SEGMENT = "<APPLICATION-SEGMENT><INQUIRY-UNIQUE-REF-NO>" + pEnqId + "/" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "</INQUIRY-UNIQUE-REF-NO>"
                                //                                       + "<CREDT-INQ-PURPS-TYP>ACCT-ORIG</CREDT-INQ-PURPS-TYP><CREDIT-INQUIRY-STAGE>PRE-SCREEN</CREDIT-INQUIRY-STAGE>"
                                //                                       + "<CREDT-REQ-TYP>INDV</CREDT-REQ-TYP><BRANCH-ID>" + postKYCSaveData.pBranchCode + "</BRANCH-ID>"
                                //                                       + "<LOS-APP-ID></LOS-APP-ID>"
                                //                                       + "<LOAN-AMOUNT></LOAN-AMOUNT></APPLICATION-SEGMENT>";

                                //            string INQUIRY = "<INQUIRY>" + APPLICANT_SEGMENT + ADDRESS_SEGMENT + APPLICATION_SEGMENT + "</INQUIRY>";

                                //            string requestXML = "<REQUEST-REQUEST-FILE>" + HEADER_SEGMENT + INQUIRY + "</REQUEST-REQUEST-FILE>";

                                //            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                                //            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live

                                //            httpWebRequest.ContentType = "application/xml; charset=utf-8";
                                //            httpWebRequest.Accept = "application/xml";
                                //            httpWebRequest.Method = "POST";
                                //            httpWebRequest.PreAuthenticate = true;
                                //            httpWebRequest.Headers.Add("requestXML", requestXML);
                                //            httpWebRequest.Headers.Add("userId", userId);
                                //            httpWebRequest.Headers.Add("password", password);
                                //            httpWebRequest.Headers.Add("mbrid", mbrid);
                                //            httpWebRequest.Headers.Add("productType", productType);
                                //            httpWebRequest.Headers.Add("productVersion", productVersion);
                                //            httpWebRequest.Headers.Add("reqVolType", reqVolType);

                                //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                                //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                                //            {
                                //                var highmarkresult = streamReader.ReadToEnd();
                                //                responsedata = highmarkresult.ToString().Trim();
                                //            }

                                //            string vErrDescResponse1 = "";
                                //            int vResponse1 = 0;
                                //            oCmd = new SqlCommand();

                                //            oCmd.CommandType = CommandType.StoredProcedure;
                                //            oCmd.CommandText = "SaveHighmarkResponse";
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pHighMarkId", pEnqId);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, responsedata.Length + 1, "@pResponseData", responsedata);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", postKYCSaveData.pDate);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppliedAmt", Convert.ToDouble(postKYCSaveData.pLoanAmount));
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", "");
                                //            DBConnect.Execute(oCmd);
                                //            vResponse1 = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                                //            vErrDescResponse1 = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);

                                //            if (vResponse1 > 0)
                                //            {
                                //                return pErrDesc + ":" + pEnqId + ". Highmark Response Type-1 Error..." + ":" + vErrDescResponse1 + ":" + pCbId;
                                //            }

                                //            System.Threading.Thread.Sleep(10000);

                                //            //Report Part
                                //            XmlDocument xd = new XmlDocument();
                                //            xd.LoadXml(responsedata);

                                //            XmlNodeList elemList = xd.GetElementsByTagName("INQUIRY-UNIQUE-REF-NO");
                                //            string INQUIRY_UNIQUE_REF_NO = elemList[0].InnerText;

                                //            elemList = xd.GetElementsByTagName("REPORT-ID");
                                //            string REPORT_ID = elemList[0].InnerText;

                                //            elemList = xd.GetElementsByTagName("RESPONSE-DT-TM");
                                //            string RESPONSE_DT_TM = elemList[0].InnerText;

                                //            elemList = xd.GetElementsByTagName("RESPONSE-TYPE");
                                //            string RESPONSE_TYPE = elemList[0].InnerText;

                                //            string REQUEST_REQUEST_FILE = "<REQUEST-REQUEST-FILE>"
                                //                                    + "<HEADER-SEGMENT>"
                                //                                    + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                //                                    + "<INQ-DT-TM>" + RESPONSE_DT_TM + "</INQ-DT-TM>"
                                //                                    + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                                //                                    + "<REQ-ACTN-TYP>ISSUE</REQ-ACTN-TYP>"
                                //                                    + "<TEST-FLG>N</TEST-FLG>"
                                //                                    + "<AUTH-FLG>Y</AUTH-FLG>"
                                //                                    + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                //                                    + "<RES-FRMT>XML/HTML</RES-FRMT>"
                                //                                    + "<MEMBER-PRE-OVERRIDE>Y</MEMBER-PRE-OVERRIDE>"
                                //                                    + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                                //                                    + "</HEADER-SEGMENT>"
                                //                                    + "<INQUIRY>"
                                //                                    + "<INQUIRY-UNIQUE-REF-NO>" + INQUIRY_UNIQUE_REF_NO + "</INQUIRY-UNIQUE-REF-NO>"
                                //                                    + "<REQUEST-DT-TM>" + RESPONSE_DT_TM + "</REQUEST-DT-TM>"
                                //                                    + "<REPORT-ID>" + REPORT_ID + "</REPORT-ID>"
                                //                                    + "</INQUIRY>"
                                //                                    + "</REQUEST-REQUEST-FILE>";

                                //            string finalXml = REQUEST_REQUEST_FILE;

                                //            var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                                //            //var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live                                                
                                //            httpWebRequestFinal.ContentType = "application/xml; charset=utf-8";
                                //            httpWebRequestFinal.Method = "POST";
                                //            httpWebRequestFinal.PreAuthenticate = true;
                                //            httpWebRequestFinal.Headers.Add("requestXML", finalXml);
                                //            httpWebRequestFinal.Headers.Add("userId", userId);
                                //            httpWebRequestFinal.Headers.Add("password", password);
                                //            httpWebRequestFinal.Headers.Add("mbrid", mbrid);
                                //            httpWebRequestFinal.Headers.Add("productType", productType);
                                //            httpWebRequestFinal.Headers.Add("productVersion", productVersion);
                                //            httpWebRequestFinal.Headers.Add("reqVolType", reqVolType);

                                //            var httpResponseFinal = (HttpWebResponse)httpWebRequestFinal.GetResponse();
                                //            string responsedataFinal = string.Empty;
                                //            using (var streamReader = new StreamReader(httpResponseFinal.GetResponseStream(), Encoding.UTF8))
                                //            {
                                //                var highmarkresult = streamReader.ReadToEnd();
                                //                responsedataFinal = highmarkresult.ToString().Trim();
                                //            }

                                //            oCmd = new SqlCommand();
                                //            Int32 vErr = 0;
                                //            oCmd.CommandType = CommandType.StoredProcedure;
                                //            oCmd.CommandText = "SaveDetailsHighmarkResponse";
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pHighMarkId", pEnqId);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pInqUniqueRefNo", INQUIRY_UNIQUE_REF_NO);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, responsedataFinal.Length + 1, "@pResponseData", responsedataFinal);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", postKYCSaveData.pDate);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", "");
                                //            DBConnect.Execute(oCmd);
                                //            vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                                //            vErrDescResponse1 = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);
                                //            oCmd.Dispose();

                                //            if (vErr > 0)
                                //            {
                                //                return pErrDesc + ":" + pEnqId + ". Highmark Response Type-2 Error..." + ":" + vErrDescResponse1 + ":" + pCbId;
                                //            }
                                //            else
                                //            {
                                //                return pErrDesc + ":" + pEnqId + ":" + "Highmark Verification Successful" + ":" + vErrDescResponse1 + ":" + pCbId;
                                //            }
                                //        }
                                //        catch (Exception ex)
                                //        {
                                //            return pErrDesc + ":" + pEnqId + ":" + "Highmark Response Error.." + ex + ":" + "" + ":" + pCbId;
                                //        }
                                //    }
                            }

                        }
                        catch (Exception ex)
                        {
                            oCmd2.CommandType = CommandType.StoredProcedure;
                            oCmd2.CommandText = "UpdateEquifaxInformation";
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", ex.ToString());
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                            DBConnect.Execute(oCmd2);
                            vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                            equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);

                            if (vErr2 == 1)
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                            else if (vErr2 == 5)
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                            else
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                        }
                    }
                    if (vErr1 == 2)
                    {
                        return pErrDesc + ":" + pEnqId + ":" + pCbId;
                    }
                    else
                    {
                        return pErrDesc + ":" + pEnqId + ":" + pCbId;
                    }
                }
                catch (Exception ex)
                {
                    return "SUCCESS:Initial Approach for this member uploaded successfully." + ":" + pEnqId + ":" + "Connection could not be established." + ": " + ":" + pCbId;
                }
                finally
                {
                    oCmd1.Dispose();
                    oCmd2.Dispose();
                }
            }
            else
            {
                return vErrMsg + ":" + pEnqId + ":" + pCbId;
            }
        }

        public string SaveKYC(PostKYCSaveData postKYCSaveData, OCRData postOCRData)
        {
            string pRequest = ""; string pResponse = "";
            SqlCommand oCmd1 = new SqlCommand();
            SqlCommand oCmd2 = new SqlCommand();
            Int32 vErr1 = 0, vErr2 = 0, pCbId = 0;
            string pErrDesc = "", pEnqId = "XX", vMemDob = string.Empty, aadhaarId = "", voterId = "", relationCode = "", pEqXml = "", equifaxResponse = "", vErrMsg = "";
            string CCRUserName = "STS_CENCCR", CCRPassword = "V2*PdhbrB";
            string vAddressType = "H", vAddress1 = string.Empty;
            WebServiceSoapClient eq = new WebServiceSoapClient();
            vAddress1 = Convert.ToString(postKYCSaveData.pAddress);
            vMemDob = postKYCSaveData.pDob.Substring(5, 2) + "/" + postKYCSaveData.pDob.Substring(8, 2) + "/" + postKYCSaveData.pDob.Substring(0, 4);
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "",
                vApplAadharNo = "", vApplAadhaarRefNo = "", vCoApplAadharNo = "", vCoApplAadhaarRefNo = "", vApplMaskedAadhaar = "", vCoApplMaskedAadhaar = "";
            //try
            //{
            //    string vResponseData = JsonConvert.SerializeObject(postKYCSaveData);
            //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "IniReqData", postKYCSaveData.pMobileNo1));
            //    System.IO.Directory.CreateDirectory(folderPath);
            //    Guid guid = Guid.NewGuid();
            //    File.WriteAllText(folderPath + "/" + Convert.ToString(guid) + ".text", vResponseData);
            //}
            //finally { }

            #region AadharVault
            if (Convert.ToInt32(postKYCSaveData.pId1Type) == 1 || Convert.ToInt32(postKYCSaveData.pId2Type) == 1)
            {
                AadhaarVaultResponse vAadharVaultResponse = null;

                vApplAadharNo = Convert.ToInt32(postKYCSaveData.pId1Type) == 1 ? postKYCSaveData.pId1No : postKYCSaveData.pId2No;
                vApplMaskedAadhaar = String.Format("{0}{1}", "********", vApplAadharNo.Substring(vApplAadharNo.Length - 4, 4));
                AadhaarVault vAadhaarVault = new AadhaarVault();
                vAadhaarVault.refData = vApplAadharNo;
                vAadhaarVault.refDataType = "U";
                vAadhaarVault.pMobileNo = postKYCSaveData.pMobileNo1;
                vAadhaarVault.pCreatedBy = postKYCSaveData.pCreatedBy;
                vAadharVaultResponse = AadhaarVault(vAadhaarVault);

                pRequest = AsString(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(vAadhaarVault), "root"));
                pResponse = AsString(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(vAadharVaultResponse), "root"));

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
                        // return "Failed:Unable to get aadhaar ref no." + ":" + pEnqId + ":" + pCbId;
                        return "Failed:Unable to get aadhaar ref no." + ":" + pEnqId + ":" + "Unable to get aadhaar ref no." + ":" + "" + ":" + pCbId;
                    }
                }
                else
                {
                    return ("Failed:Aadhaar Vault API Returns Error-" + "Request-" + pRequest + "Response-" + pResponse);
                }
            }
            if (Convert.ToInt32(postKYCSaveData.pCoAppIdType) == 1)
            {
                AadhaarVaultResponse vAadharVaultResponse1 = null;
                vCoApplAadharNo = Convert.ToString(postKYCSaveData.pCoAppIdNo);
                vCoApplMaskedAadhaar = String.Format("{0}{1}", "********", vCoApplAadharNo.Substring(vCoApplAadharNo.Length - 4, 4));
                AadhaarVault vAVault = new AadhaarVault();
                vAVault.refData = vCoApplAadharNo;
                vAVault.refDataType = "U";
                vAVault.pMobileNo = postKYCSaveData.pCoAppMobile;
                vAVault.pCreatedBy = postKYCSaveData.pCreatedBy;
                vAadharVaultResponse1 = AadhaarVault(vAVault);
                pRequest = AsString(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(vAVault), "root"));
                pResponse = AsString(JsonConvert.DeserializeXmlNode(JsonConvert.SerializeObject(vAadharVaultResponse1), "root"));
                vCoApplAadhaarRefNo = Convert.ToString(vAadharVaultResponse1.results[0]);
                if (Convert.ToInt32(Convert.ToString(vAadharVaultResponse1.response_code)) == 1)
                {
                    if (vCoApplAadhaarRefNo != "")
                    {
                        postKYCSaveData.pCoAppIdType = "13";
                        postKYCSaveData.pCoAppIdNo = vCoApplAadhaarRefNo;
                    }
                    else
                    {
                        return "Failed:Unable to get aadhaar ref no." + ":" + pEnqId + ":" + "Unable to get aadhaar ref no." + ":" + "" + ":" + pCbId;
                    }
                }
                else
                {
                    return ("Failed:Aadhaar Vault API Returns Error-" + "Request-" + pRequest + "Response-" + pResponse);
                }
            }
            #endregion

            int vErr3 = ChkDdupMEL(postKYCSaveData.pId1No, postKYCSaveData.pId2No, postKYCSaveData.pId3No, ref vErrMsg);
            if (vErr3 == 0)
            {
                try
                {
                    oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    oCmd1.CommandText = "SaveInitialApproach";
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.VarChar, 16, "@pEnquiryId", postKYCSaveData.pEnquiryId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 4, "@pCBId", Convert.ToInt32(postKYCSaveData.pCbId));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pGroupID", postKYCSaveData.pGroupId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanAmount", Convert.ToDouble(postKYCSaveData.pLoanAmount));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pFirstName", postKYCSaveData.pFirstName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMiddleName", postKYCSaveData.pMiddleName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLastName", postKYCSaveData.pLastName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDOB", postKYCSaveData.pDob);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAGE", Convert.ToInt32(postKYCSaveData.pAge));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRelativeName", postKYCSaveData.pGuardianName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRelationId", Convert.ToInt32(postKYCSaveData.pGuardianRelation));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIdProofType", Convert.ToInt32(postKYCSaveData.pId1Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pIdProofNo", postKYCSaveData.pId1No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType", Convert.ToInt32(postKYCSaveData.pId2Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo", postKYCSaveData.pId2No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddressProofType2", Convert.ToInt32(postKYCSaveData.pId3Type));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo2", postKYCSaveData.pId3No);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType1", Convert.ToInt32(postKYCSaveData.pAddressType1));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo1", postKYCSaveData.pHouseNo1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet1", postKYCSaveData.pStreet1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea1", postKYCSaveData.pArea1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pVillage1", postKYCSaveData.pVillage1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict1", postKYCSaveData.pSubDistrict1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pDistrict1", postKYCSaveData.pDistrict1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pState1", postKYCSaveData.pState1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark1", postKYCSaveData.pLandMark1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice1", postKYCSaveData.pPostOffice1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode1", postKYCSaveData.pPinCode1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo1", postKYCSaveData.pMobileNo1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmailId1", postKYCSaveData.pEmailId1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAddType2", Convert.ToInt32(postKYCSaveData.pAddressType2));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pHouseNo2", postKYCSaveData.pHouseNo2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pStreet2", postKYCSaveData.pStreet2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pArea2", postKYCSaveData.pArea2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pVillageId2", Convert.ToInt32(postKYCSaveData.pVillageId2));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pSubDistrict2", postKYCSaveData.pSubDistrict2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLandMark2", postKYCSaveData.pLandMark2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pPostOffice2", postKYCSaveData.pPostOffice2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pPinCode2", postKYCSaveData.pPinCode2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMobileNo2", postKYCSaveData.pMobileNo2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pEmailId2", postKYCSaveData.pEmailId2);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postKYCSaveData.pBranchCode);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCSaveData.pEoId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAadhaarScan", postKYCSaveData.pAadhaarScan);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", postKYCSaveData.pMode);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pISGroupSame", postKYCSaveData.pISGroupSame);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pApplicationType", postKYCSaveData.pApplicationType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMktId", postKYCSaveData.pMktId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEnqType", postKYCSaveData.EnqType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pOTP", postKYCSaveData.pOTP);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTimeStamp", postKYCSaveData.pTimeStamp);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pOCRApproveYN", postKYCSaveData.pOCRApproveYN);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pKarzaVerifyYN", postKYCSaveData.pKarzaVerifyYN);

                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pGender", postKYCSaveData.pGender);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusTypeId", postKYCSaveData.pBussType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOccupationId", postKYCSaveData.pOccupation);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Float, 18, "@pDeclIncome", postKYCSaveData.pDeclaredInc);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIncFrequency", postKYCSaveData.pIncFrequency);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pCoAppName", postKYCSaveData.pCoAppFullName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppFName", postKYCSaveData.pCoAppFName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppMName", postKYCSaveData.pCoAppMName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppLName", postKYCSaveData.pCoAppLName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCoAppDOB", postKYCSaveData.pCoAppDOB);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppAge", postKYCSaveData.pCoAppAge);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppRelationId", postKYCSaveData.pCoAppRelation);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pCoAppGender", postKYCSaveData.pCoAppGender);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoAppMobileNo", postKYCSaveData.pCoAppMobile);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 300, "@pCoAppAddress", postKYCSaveData.pCoAppAddress);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCoAppPinCode", postKYCSaveData.pCoAppPinCode);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppState", postKYCSaveData.pCoAppState);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppOccupationId", postKYCSaveData.pCoAppOccupation);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppBusTypeId", postKYCSaveData.pCoApppBussType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoAppDeclIncome", postKYCSaveData.pCoAppDeclaredInc);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppIncFrequency", postKYCSaveData.pCoAppIncFrequency);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppIdentyProfId", postKYCSaveData.pCoAppIdType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pCoAppIdentyProfNo", postKYCSaveData.pCoAppIdNo);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Xml, postKYCSaveData.pXmlEarningMemDtl.Length + 1, "@pXmlEarningMemDtl", postKYCSaveData.pXmlEarningMemDtl);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSelFeLatitude", postKYCSaveData.pSelFeLatitude);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pSelFeLongitude", postKYCSaveData.pSelFeLongitude);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusActvId", postKYCSaveData.pBussActvId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppBusActvId", postKYCSaveData.pCoAppBussActvId);

                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pApplAadhaarNo", vApplMaskedAadhaar);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pCoAppplAadhaarNo", vCoApplMaskedAadhaar);

                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);

                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppHouseNo", postKYCSaveData.pCoHouseNo);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppStreet", postKYCSaveData.pCoStreet);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppArea", postKYCSaveData.pCoArea);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppVillageId", postKYCSaveData.pCoVillage);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppSubDistrict", postKYCSaveData.pCoSubDistrict);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppDistrict", postKYCSaveData.pCoDistrict);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppLandmark", postKYCSaveData.pCoLandmark);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCoAppPostOffice", postKYCSaveData.pCoPostOffice);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppStateId", postKYCSaveData.pCoAppStateId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postKYCSaveData.pXmlDirector.Length + 1, "@pXmlDirector", postKYCSaveData.pXmlDirector);

                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsAbledYN", postKYCSaveData.pIsAbledYN);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSpeciallyAbled", postKYCSaveData.pSpeciallyAbled);
                    if (postKYCSaveData.pTempId == null)
                        DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTempId", DBNull.Value);
                    else
                        DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTempId", postKYCSaveData.pTempId);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFaceMatchRes", postKYCSaveData.pFaceMatchRes);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFaceMatchScr", postKYCSaveData.pFaceMatchScr);

                    DBConnect.Execute(oCmd1);
                    vErr1 = Convert.ToInt32(oCmd1.Parameters["@pErr"].Value);
                    pErrDesc = Convert.ToString(oCmd1.Parameters["@pErrDesc"].Value);
                    pEnqId = Convert.ToString(oCmd1.Parameters["@pEnquiryId"].Value);
                    pCbId = Convert.ToInt32(oCmd1.Parameters["@pCBId"].Value);
                    postOCRData.EnquiryId = pEnqId;

                    if (vErr1 == 0)
                    {
                        vDigiConcentSMS = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMS"].Value);
                        vDigiConcentSMSTemplateId = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSTemplateId"].Value);
                        vDigiConcentSMSLanguage = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSLanguage"].Value);
                        //-------------Send Digital Concent-----------------
                        vDigiConcentSMS = vDigiConcentSMS.Replace("{#var#}", vDigiConcentSMSLanguage.ToUpper() == "ENGLISH" ? "https://bijliftt.com/n/" + Encode(Convert.ToInt64(pEnqId)) : HttpUtility.UrlEncode("https://bijliftt.com/n/" + Encode(Convert.ToInt64(pEnqId)), Encoding.GetEncoding("ISO-8859-1")));
                        //-------------------------Save OCR Data-----------------------------
                        try
                        {
                            string vOCRMsg = SaveOCRData(postOCRData);
                        }
                        finally { }
                        //-------------------------send concent sms-----------------------------
                        try
                        {
                            if (vDigiConcentSMS.Length > 0)
                            {
                                vResultSendDigitalConcentSMS = SendDigitalConcentSMS(postKYCSaveData.pMobileNo1, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
                            }
                        }
                        finally { }

                        //-------------------------------------------------------------------
                        if (postKYCSaveData.pId1Type.Equals("13"))
                        {
                            //aadhaarId = postKYCSaveData.pId1No;
                            //aadhaarId = vApplMaskedAadhaar;
                            aadhaarId = vApplAadharNo;
                        }
                        else if (postKYCSaveData.pId2Type.Equals("13"))
                        {
                            //aadhaarId = postKYCSaveData.pId2No;
                            // aadhaarId = vApplMaskedAadhaar;
                            aadhaarId = vApplAadharNo;
                        }
                        else
                        {
                            aadhaarId = "";
                        }

                        if (postKYCSaveData.pId1Type.Equals("3"))
                        {
                            voterId = postKYCSaveData.pId1No;
                        }
                        else if (postKYCSaveData.pId2Type.Equals("3"))
                        {
                            voterId = postKYCSaveData.pId2No;
                        }
                        else
                        {
                            voterId = postKYCSaveData.pId2No;
                        }

                        relationCode = GetRelationCode(postKYCSaveData.pGuardianRelation);

                        try
                        {
                            if (postKYCSaveData.pTyp == "E")
                            {
                                if (postKYCSaveData.pOCRApproveYN == "Y" && postKYCSaveData.pKarzaVerifyYN == "Y")
                                {
                                    //-------------------------------------------LIVE----------------------------------------------
                                    pEqXml = eq.Equifax(
                                     postKYCSaveData.pFirstName, postKYCSaveData.pMiddleName, postKYCSaveData.pLastName
                                    , Convert.ToDateTime(postKYCSaveData.pDob).ToString("yyyy-MM-dd")
                                    , vAddressType, vAddress1, postKYCSaveData.pState1, postKYCSaveData.pPinCode1, postKYCSaveData.pMobileNo1
                                    , "Voter ID", voterId, "Aadhaar", aadhaarId, postKYCSaveData.pGuardianRelation, postKYCSaveData.pGuardianName
                                    , "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                                    if (pEqXml.Equals(""))
                                    {
                                        oCmd2 = new SqlCommand();
                                        oCmd2.CommandType = CommandType.StoredProcedure;
                                        oCmd2.CommandText = "UpdateEquifaxInformation";
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "Equifax File Not Found");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                                        DBConnect.Execute(oCmd2);
                                        vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                                        equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);

                                        if (vErr2 == 1)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else if (vErr2 == 5)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                    }
                                    else
                                    {
                                        pEqXml = pEqXml.Replace("<?xml version=\"1.0\"?>", "").Trim();
                                        pEqXml = pEqXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                                        pEqXml = pEqXml.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");

                                        string vOthMemCBResponse = ProcessOtherMemCB(pEnqId, pCbId, postKYCSaveData.pBranchCode, Convert.ToInt32(postKYCSaveData.pCreatedBy),
                                                postKYCSaveData.pDate, CCRUserName, CCRPassword, postKYCSaveData.pEoId, vCoApplAadharNo);

                                        oCmd2 = new SqlCommand();
                                        oCmd2.CommandType = CommandType.StoredProcedure;
                                        oCmd2.CommandText = "UpdateEquifaxInformation";
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                                        DBConnect.Execute(oCmd2);
                                        vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                                        equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);
                                        if (vErr2 == 1)
                                        {
                                            if (vOthMemCBResponse == "Successful")
                                            {
                                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Successful" + ":" + equifaxResponse + ":" + pCbId;
                                            }
                                            else
                                            {
                                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Successful" + ":" + equifaxResponse + ":" + pCbId + " And Equifax Failed for " + vOthMemCBResponse + "";
                                            }
                                        }
                                        else if (vErr2 == 5)
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                        else
                                        {
                                            return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                                        }
                                    }
                                }
                                else
                                {
                                    return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Hold for OCR Data mismatch " + ":" + equifaxResponse + ":" + pCbId;
                                }
                            }
                            else
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            oCmd2 = new SqlCommand();
                            oCmd2.CommandType = CommandType.StoredProcedure;
                            oCmd2.CommandText = "UpdateEquifaxInformation";
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCSaveData.pBranchCode);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postKYCSaveData.pEoId);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postKYCSaveData.pCreatedBy));
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postKYCSaveData.pDate);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", ex.ToString());
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                            DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                            DBConnect.Execute(oCmd2);
                            vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                            equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);

                            if (vErr2 == 1)
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                            else if (vErr2 == 5)
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                            else
                            {
                                return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed" + ":" + equifaxResponse + ":" + pCbId;
                            }
                        }
                    }
                    if (vErr1 == 2)
                    {
                        return pErrDesc + ":" + pEnqId + ":" + pCbId;
                    }
                    else
                    {
                        return pErrDesc + ":" + pEnqId + ":" + pCbId;
                    }
                }
                catch (Exception ex)
                {
                    return "SUCCESS:Initial Approach for this member uploaded successfully." + ":" + pEnqId + ":" + "Connection could not be established." + ": " + ":" + pCbId;
                }
                finally
                {
                    oCmd1.Dispose();
                    oCmd2.Dispose();
                }
            }
            else
            {
                return vErrMsg + ":" + pEnqId + ":" + pCbId;
            }
        }
        #endregion

        //public string SMSAPITest()
        //{
        //    string vDigiConcentSMS = string.Empty, vDigiConcentSMSTemplateId = string.Empty, vDigiConcentSMSLanguage = string.Empty; ;
        //    string vMobNo = "7044064914";
        //    string vResultSendDigitalConcentSMS="";
        //    SqlCommand oCmd1 = new SqlCommand();
        //    oCmd1.CommandType = System.Data.CommandType.StoredProcedure;
        //    oCmd1.CommandText = "GetSMSContent";
        //    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
        //    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
        //    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
        //    DBConnect.Execute(oCmd1);            
        //    vDigiConcentSMS = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMS"].Value);
        //    vDigiConcentSMSTemplateId = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSTemplateId"].Value);
        //    vDigiConcentSMSLanguage = Convert.ToString(oCmd1.Parameters["@pDigiConcentSMSLanguage"].Value);
        //    vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vMobNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
        //    return "";
        //}

        #region GetRelationCode
        private string GetRelationCode(string vRelation)
        {
            switch (vRelation)
            {
                case "6": return "H";
                case "4": return "F";
                default: return "F";
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
                            if (vFileTag.Equals("GroupPhoto"))
                            {
                                if (MinioYN == "N")
                                {
                                    vErr = SaveMemberImages(binaryWriteArray, "Group", kycNo, vFileTag);
                                }
                                else
                                {
                                    vErr = UploadFileMinio(binaryWriteArray, fileName, kycNo, GroupBucket, MinioUrl);
                                }
                                if (vErr.Equals("Y"))
                                {
                                    row.Add(new KYCImageSave("Successful", fileName));
                                }
                                else
                                {
                                    row.Add(new KYCImageSave("Failed", fileName));
                                }
                            }
                            else if (vFileTag.Equals("PassbookImage"))
                            {
                                if (MinioYN == "N")
                                {
                                    vErr = SaveMemberImages(binaryWriteArray, "Member", kycNo, vFileTag);
                                }
                                else
                                {
                                    vErr = UploadFileMinio(binaryWriteArray, fileName, kycNo, MemberBucket, MinioUrl);
                                }
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

                                if (MinioYN == "N")
                                {
                                    vErr = SaveMemberImages(binaryWriteArray, "InitialApproach", kycNo, vFileTag);
                                }
                                else
                                {
                                    vErr = UploadFileMinio(binaryWriteArray, fileName, kycNo, InitialBucket, MinioUrl);
                                }
                                if (vErr.Equals("Y"))
                                {
                                    row.Add(new KYCImageSave("Successful", fileName));
                                }
                                else
                                {
                                    row.Add(new KYCImageSave("Failed", fileName));
                                }
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

        public List<EMPImageSave> EMPImageUpload(Stream image)
        {
            string vErr = "";
            List<EMPImageSave> row = new List<EMPImageSave>();
            string EmpCode = "";
            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "EmpCode")
                        {
                            EmpCode = content.StringData;
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
                            //if (vFileTag.Equals("EmpPhoto"))
                            //{
                            vErr = SaveEmpImages(binaryWriteArray, "Attendance", EmpCode, vFileTag);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new EMPImageSave("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new EMPImageSave("Failed", fileName));
                            }
                            //}
                        }
                        else
                        {
                            row.Add(new EMPImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new EMPImageSave("Failed", "No Data Found"));
            }
            return row;
        }

        public List<EMPImageSave> GRTImageUpload(Stream image)
        {
            string vErr = "";
            List<EMPImageSave> row = new List<EMPImageSave>();
            string GroupCode = "";
            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "GroupCode")
                        {
                            GroupCode = content.StringData;
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
                            vErr = SaveMemberImages(binaryWriteArray, "Individual", GroupCode, vFileTag);
                            if (vErr.Equals("Y"))
                            {
                                row.Add(new EMPImageSave("Successful", fileName));
                            }
                            else
                            {
                                row.Add(new EMPImageSave("Failed", fileName));
                            }
                            //}
                        }
                        else
                        {
                            row.Add(new EMPImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new EMPImageSave("Failed", "No Data Found"));
            }
            return row;
        }

        public string SaveMemberImg(PostImage postImage)
        {
            string isImageSaved = "N";
            string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", postImage.imageGroup, postImage.folderName));
            System.IO.Directory.CreateDirectory(folderPath);
            string filePath = string.Format("{0}/{1}.png", folderPath, postImage.imageName);
            if (postImage.imageBinary != null)
            {
                File.WriteAllBytes(filePath, postImage.imageBinary);
                isImageSaved = "Y";
            }
            return isImageSaved;
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
            string folderPath = "", vImgName = "";
            if (imageGroup == "InitialApproach")
            {
                folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", imageGroup, folderName));
                vImgName = imageName;
            }
            else
            {
                folderPath = HostingEnvironment.MapPath("~/Files/" + imageGroup);
                vImgName = folderName + "_" + imageName;
            }
            System.IO.Directory.CreateDirectory(folderPath);
            string filePath = string.Format("{0}/{1}.png", folderPath, vImgName);
            if (imageBinary != null)
            {
                File.WriteAllBytes(filePath, imageBinary);
                isImageSaved = "Y";
            }
            return isImageSaved;
        }

        private string SaveEmpImages(byte[] imageBinary, string imageGroup, string folderName, string imageName)
        {
            string isImageSaved = "N";
            //string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", imageGroup, folderName));
            string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}", imageGroup));
            System.IO.Directory.CreateDirectory(folderPath);
            //string filePath = string.Format("{0}/{1}.png", folderPath, imageName);
            string filePath = string.Format("{0}/{1}.png", folderPath, imageName);
            if (imageBinary != null)
            {
                File.WriteAllBytes(filePath, imageBinary);
                isImageSaved = "Y";
            }
            return isImageSaved;
        }
        #endregion

        #region SaveEquifax
        public string SaveEquifax(PostCBSaveData postCBSaveData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vMemDob = string.Empty, aadhaarId = "", voterId = "", relationCode = "", pEqXml = "", equifaxResponse = "";
            EquifaxService.WSEquifaxMCRSoapClient eq = new EquifaxService.WSEquifaxMCRSoapClient();
            vMemDob = postCBSaveData.pDOB.Substring(5, 2) + "/" + postCBSaveData.pDOB.Substring(8, 2) + "/" + postCBSaveData.pDOB.Substring(0, 4);
            try
            {
                //if (postCBSaveData.pTyp == "E")
                //{
                if (postCBSaveData.pIdProofType.Equals("1"))
                {
                    aadhaarId = postCBSaveData.pIdProofNo;
                }
                else if (postCBSaveData.pAddressProofType.Equals("1"))
                {
                    aadhaarId = postCBSaveData.pAddressProofNo;
                }
                else
                {
                    aadhaarId = "";
                }

                if (postCBSaveData.pIdProofType.Equals("3"))
                {
                    voterId = postCBSaveData.pIdProofNo;
                }
                else if (postCBSaveData.pAddressProofType.Equals("3"))
                {
                    voterId = postCBSaveData.pAddressProofNo;
                }
                else
                {
                    voterId = "";
                }

                relationCode = GetRelationCode(postCBSaveData.pRelationId);

                try
                {
                    pEqXml = eq.EquifaxVerification(postCBSaveData.pFullName, postCBSaveData.pRelativeName, relationCode, postCBSaveData.pAddress,
                        postCBSaveData.pStateShortName, postCBSaveData.pPinCode, vMemDob, voterId, aadhaarId,
                        5750, "STSCENTRUM", "hg*uy56GF", "027FZ01546", "KQ7", "MCR", "1.0", "1234", "Yes", "MFI", postCBSaveData.pMobileNo1);

                    if (pEqXml.Equals(""))
                    {
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandText = "UpdateEquifaxInformation";
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", postCBSaveData.pEnquiryId);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", Convert.ToInt32(postCBSaveData.pCBId));
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postCBSaveData.pBranchCode);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postCBSaveData.pEoId);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCBSaveData.pCreatedBy));
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postCBSaveData.pDate);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "Equifax File Not Found");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                        DBConnect.Execute(oCmd);
                        vErr = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                        equifaxResponse = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);

                        if (vErr == 1)
                        {
                            return "Equifax Verification Failed" + ":" + equifaxResponse;
                        }
                        else if (vErr == 5)
                        {
                            return "Equifax Verification Failed" + ":" + equifaxResponse;
                        }
                        else
                        {
                            return "Equifax Verification Failed" + ":" + equifaxResponse;
                        }
                    }
                    else
                    {
                        pEqXml = pEqXml.Replace("<?xml version=\"1.0\"?>", "").Trim();
                        pEqXml = pEqXml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                        pEqXml = pEqXml.Replace("xmlns=\"http://services.equifax.com/eport/ws/schemas/1.0\"", "");

                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandText = "UpdateEquifaxInformation";
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", postCBSaveData.pEnquiryId);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", Convert.ToInt32(postCBSaveData.pCBId));
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postCBSaveData.pBranchCode);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postCBSaveData.pEoId);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCBSaveData.pCreatedBy));
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postCBSaveData.pDate);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                        DBConnect.Execute(oCmd);
                        vErr = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                        equifaxResponse = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);

                        if (vErr == 1)
                        {
                            return "Equifax Verification Successful" + ":" + equifaxResponse;
                        }
                        else if (vErr == 5)
                        {
                            return "Equifax Verification Failed" + ":" + equifaxResponse;
                        }
                        else
                        {
                            return "Equifax Verification Failed" + ":" + equifaxResponse;
                        }
                    }
                }
                catch (Exception ex)
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "UpdateEquifaxInformation";
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", postCBSaveData.pEnquiryId);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", Convert.ToInt32(postCBSaveData.pCBId));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postCBSaveData.pBranchCode);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postCBSaveData.pEoId);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCBSaveData.pCreatedBy));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postCBSaveData.pDate);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "F");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", ex.ToString());
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pStatusDesc", "");
                    DBConnect.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pStatus"].Value);
                    equifaxResponse = Convert.ToString(oCmd.Parameters["@pStatusDesc"].Value);

                    if (vErr == 1)
                    {
                        return "Equifax Verification Failed" + ":" + equifaxResponse;
                    }
                    else if (vErr == 5)
                    {
                        return "Equifax Verification Failed" + ":" + equifaxResponse;
                    }
                    else
                    {
                        return "Equifax Verification Failed" + ":" + equifaxResponse;
                    }
                }
                //}
                //else
                //{
                //    DataTable dt = new DataTable();
                //    oCmd = new SqlCommand();
                //    oCmd.CommandType = CommandType.StoredProcedure;
                //    oCmd.CommandText = "GetMemberEquifaxInfo";
                //    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnqId", postCBSaveData.pEnquiryId);
                //    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pType", "H");
                //    DBConnect.ExecuteForSelect(oCmd, dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        try
                //        {
                //            ServicePointManager.Expect100Continue = true;
                //            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //            string responsedata = string.Empty;
                //            string userId = "in_cpuuat_cenmfi3";
                //            string password = "9DDB522C741D8F3192D53B13D9FB590B02723024";
                //            string mbrid = "MFI0000228";
                //            string productType = "INDV";
                //            string productVersion = "1.0";
                //            string reqVolType = "INDV";
                //            string SUB_MBR_ID = "CENTRUM MICROCREDIT PRIVATE LIMITED";

                //            string request_datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                //            string vAadhar = "", vVoterId = "";
                //            vVoterId = Convert.ToString(dt.Rows[0]["VoterId"]);
                //            vAadhar = Convert.ToString(dt.Rows[0]["UID"]);

                //            string HEADER_SEGMENT = "<HEADER-SEGMENT>"
                //                                      + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                //                                      + "<INQ-DT-TM>" + request_datetime + "</INQ-DT-TM>"
                //                                      + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                //                                      + "<REQ-ACTN-TYP>SUBMIT</REQ-ACTN-TYP>"
                //                                      + "<TEST-FLG>N</TEST-FLG>"
                //                                      + "<AUTH-FLG>Y</AUTH-FLG>"
                //                                      + "<AUTH-TITLE>USER</AUTH-TITLE>"
                //                                      + "<RES-FRMT>XML/HTML</RES-FRMT>"
                //                                      + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                //                                      + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                //                                      + "<MFI>"
                //                                      + "<INDV>TRUE</INDV>"
                //                                      + "<SCORE>FALSE</SCORE>"
                //                                      + "<GROUP>TRUE</GROUP>"
                //                                      + "</MFI>"
                //                                      + "<CONSUMER>"
                //                                      + "<INDV>TRUE</INDV>"
                //                                      + "<SCORE>TRUE</SCORE>"
                //                                      + "</CONSUMER>"
                //                                      + "<IOI>TRUE</IOI>"
                //                                      + "</HEADER-SEGMENT>";

                //            string APPLICANT_SEGMENT = "<APPLICANT-SEGMENT>"
                //                                        + "<APPLICANT-NAME><NAME1>" + dt.Rows[0]["MemberName"] + "</NAME1>"
                //                                        + "<NAME2></NAME2><NAME3></NAME3><NAME4></NAME4><NAME5></NAME5></APPLICANT-NAME>"
                //                                        + "<DOB><DOB-DATE>" + dt.Rows[0]["DOB"] + "</DOB-DATE>"
                //                                        + "<AGE>" + dt.Rows[0]["Age"] + "</AGE>"
                //                                        + "<AGE-AS-ON>" + dt.Rows[0]["AsOnDate"] + "</AGE-AS-ON></DOB>"
                //                                        + "<IDS>"
                //                                        + "<ID><TYPE>ID03</TYPE><VALUE>" + vAadhar + "</VALUE></ID>" //aadhaar no                                                               
                //                                        + ((vVoterId == "") ? "" : "<ID><TYPE>ID02</TYPE><VALUE>" + vVoterId + "</VALUE></ID>") //voter id (if available)                                                              
                //                                        + "</IDS>"
                //                                        + "<RELATIONS><RELATION><NAME>" + dt.Rows[0]["RelativeName"] + "</NAME><TYPE>" + dt.Rows[0]["RelationCode"] + "</TYPE>"
                //                                        + "</RELATION></RELATIONS><PHONES><PHONE><TELE-NO>" + dt.Rows[0]["MobileNo"] + "</TELE-NO>"
                //                                        + "<TELE-NO-TYPE>P03</TELE-NO-TYPE></PHONE></PHONES>"
                //                                        + "</APPLICANT-SEGMENT>";

                //            string ADDRESS_SEGMENT = "<ADDRESS-SEGMENT><ADDRESS><TYPE>D01</TYPE><ADDRESS-1>" + dt.Rows[0]["MemAdd"] + "</ADDRESS-1>"
                //                                        + "<CITY>" + dt.Rows[0]["District"] + "</CITY><STATE>" + dt.Rows[0]["StateCode"] + "</STATE><PIN>"
                //                                        + dt.Rows[0]["PIN"] + "</PIN></ADDRESS></ADDRESS-SEGMENT>";

                //            string APPLICATION_SEGMENT = "<APPLICATION-SEGMENT><INQUIRY-UNIQUE-REF-NO>" + postCBSaveData.pEnquiryId + "/" + DateTime.Now.ToString("ddMMyyyyHHmmss") + "</INQUIRY-UNIQUE-REF-NO>"
                //                                       + "<CREDT-INQ-PURPS-TYP>ACCT-ORIG</CREDT-INQ-PURPS-TYP><CREDIT-INQUIRY-STAGE>PRE-SCREEN</CREDIT-INQUIRY-STAGE>"
                //                                       + "<CREDT-REQ-TYP>INDV</CREDT-REQ-TYP><BRANCH-ID>" + postCBSaveData.pBranchCode + "</BRANCH-ID>"
                //                                       + "<LOS-APP-ID></LOS-APP-ID>"
                //                                       + "<LOAN-AMOUNT></LOAN-AMOUNT></APPLICATION-SEGMENT>";

                //            string INQUIRY = "<INQUIRY>" + APPLICANT_SEGMENT + ADDRESS_SEGMENT + APPLICATION_SEGMENT + "</INQUIRY>";

                //            string requestXML = "<REQUEST-REQUEST-FILE>" + HEADER_SEGMENT + INQUIRY + "</REQUEST-REQUEST-FILE>";

                //            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                //            //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live

                //            httpWebRequest.ContentType = "application/xml; charset=utf-8";
                //            httpWebRequest.Accept = "application/xml";
                //            httpWebRequest.Method = "POST";
                //            httpWebRequest.PreAuthenticate = true;
                //            httpWebRequest.Headers.Add("requestXML", requestXML);
                //            httpWebRequest.Headers.Add("userId", userId);
                //            httpWebRequest.Headers.Add("password", password);
                //            httpWebRequest.Headers.Add("mbrid", mbrid);
                //            httpWebRequest.Headers.Add("productType", productType);
                //            httpWebRequest.Headers.Add("productVersion", productVersion);
                //            httpWebRequest.Headers.Add("reqVolType", reqVolType);

                //            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                //            using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                //            {
                //                var highmarkresult = streamReader.ReadToEnd();
                //                responsedata = highmarkresult.ToString().Trim();
                //            }

                //            string vErrDescResponse1 = "";
                //            int vResponse1 = 0;
                //            oCmd = new SqlCommand();

                //            oCmd.CommandType = CommandType.StoredProcedure;
                //            oCmd.CommandText = "SaveHighmarkResponse";
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pHighMarkId", postCBSaveData.pEnquiryId);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, responsedata.Length + 1, "@pResponseData", responsedata);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", postCBSaveData.pDate);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postCBSaveData.pBranchCode);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCBSaveData.pCreatedBy));
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAppliedAmt", 0);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", "");
                //            DBConnect.Execute(oCmd);
                //            vResponse1 = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //            vErrDescResponse1 = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);

                //            if (vResponse1 > 0)
                //            {
                //                return "Highmark Response Type-1 Error..." + ":" + vErrDescResponse1;
                //            }

                //            System.Threading.Thread.Sleep(10000);

                //            //Report Part
                //            XmlDocument xd = new XmlDocument();
                //            xd.LoadXml(responsedata);

                //            XmlNodeList elemList = xd.GetElementsByTagName("INQUIRY-UNIQUE-REF-NO");
                //            string INQUIRY_UNIQUE_REF_NO = elemList[0].InnerText;

                //            elemList = xd.GetElementsByTagName("REPORT-ID");
                //            string REPORT_ID = elemList[0].InnerText;

                //            elemList = xd.GetElementsByTagName("RESPONSE-DT-TM");
                //            string RESPONSE_DT_TM = elemList[0].InnerText;

                //            elemList = xd.GetElementsByTagName("RESPONSE-TYPE");
                //            string RESPONSE_TYPE = elemList[0].InnerText;

                //            string REQUEST_REQUEST_FILE = "<REQUEST-REQUEST-FILE>"
                //                                    + "<HEADER-SEGMENT>"
                //                                    + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                //                                    + "<INQ-DT-TM>" + RESPONSE_DT_TM + "</INQ-DT-TM>"
                //                                    + "<REQ-VOL-TYP>C01</REQ-VOL-TYP>"
                //                                    + "<REQ-ACTN-TYP>ISSUE</REQ-ACTN-TYP>"
                //                                    + "<TEST-FLG>N</TEST-FLG>"
                //                                    + "<AUTH-FLG>Y</AUTH-FLG>"
                //                                    + "<AUTH-TITLE>USER</AUTH-TITLE>"
                //                                    + "<RES-FRMT>XML/HTML</RES-FRMT>"
                //                                    + "<MEMBER-PRE-OVERRIDE>Y</MEMBER-PRE-OVERRIDE>"
                //                                    + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                //                                    + "</HEADER-SEGMENT>"
                //                                    + "<INQUIRY>"
                //                                    + "<INQUIRY-UNIQUE-REF-NO>" + INQUIRY_UNIQUE_REF_NO + "</INQUIRY-UNIQUE-REF-NO>"
                //                                    + "<REQUEST-DT-TM>" + RESPONSE_DT_TM + "</REQUEST-DT-TM>"
                //                                    + "<REPORT-ID>" + REPORT_ID + "</REPORT-ID>"
                //                                    + "</INQUIRY>"
                //                                    + "</REQUEST-REQUEST-FILE>";

                //            string finalXml = REQUEST_REQUEST_FILE;

                //            var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Test
                //            //var httpWebRequestFinal = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/doGet.service/requestResponse"); //Live                                                
                //            httpWebRequestFinal.ContentType = "application/xml; charset=utf-8";
                //            httpWebRequestFinal.Method = "POST";
                //            httpWebRequestFinal.PreAuthenticate = true;
                //            httpWebRequestFinal.Headers.Add("requestXML", finalXml);
                //            httpWebRequestFinal.Headers.Add("userId", userId);
                //            httpWebRequestFinal.Headers.Add("password", password);
                //            httpWebRequestFinal.Headers.Add("mbrid", mbrid);
                //            httpWebRequestFinal.Headers.Add("productType", productType);
                //            httpWebRequestFinal.Headers.Add("productVersion", productVersion);
                //            httpWebRequestFinal.Headers.Add("reqVolType", reqVolType);

                //            var httpResponseFinal = (HttpWebResponse)httpWebRequestFinal.GetResponse();
                //            string responsedataFinal = string.Empty;
                //            using (var streamReader = new StreamReader(httpResponseFinal.GetResponseStream(), Encoding.UTF8))
                //            {
                //                var highmarkresult = streamReader.ReadToEnd();
                //                responsedataFinal = highmarkresult.ToString().Trim();
                //            }

                //            oCmd = new SqlCommand();
                //            vErr = 0;
                //            oCmd.CommandType = CommandType.StoredProcedure;
                //            oCmd.CommandText = "SaveDetailsHighmarkResponse";
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pHighMarkId", postCBSaveData.pEnquiryId);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pInqUniqueRefNo", INQUIRY_UNIQUE_REF_NO);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, responsedataFinal.Length + 1, "@pResponseData", responsedataFinal);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", postCBSaveData.pDate);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postCBSaveData.pBranchCode);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCBSaveData.pCreatedBy));
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                //            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 3000, "@pErrDescResponse", "");
                //            DBConnect.Execute(oCmd);
                //            vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                //            vErrDescResponse1 = Convert.ToString(oCmd.Parameters["@pErrDescResponse"].Value);
                //            oCmd.Dispose();

                //            if (vErr > 0)
                //            {
                //                return "Highmark Response Type-2 Error..." + ":" + vErrDescResponse1;
                //            }
                //            else
                //            {
                //                return "Highmark Verification Successful" + ":" + vErrDescResponse1;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return "Highmark Response Error.." + ":" + ex.ToString();
                //        }
                //    }
                //    else
                //    {
                //        return "No Records Found" + ":" + "";
                //    }
                //}
            }
            catch (Exception ex)
            {
                return "Equifax Verification Failed" + ":" + "Connection could not be established..!";
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region GetGroupDashboard
        public List<GroupDBData> GetGroupDashboard(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<GroupDBData> row = new List<GroupDBData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetFormedGroupDB";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postKYCData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", postKYCData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GroupDBData(rs["MobGroup"].ToString(), rs["WebGroup"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GroupDBData("No data available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GroupDBData("No data available", ex.ToString()));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region IFSCDtl
        public List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<IFSCData> row = new List<IFSCData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetIfscDtlByIfsc";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pIfsCode", postIFSCData.pIFSCCode);
                DBConnect.ExecuteForSelect(oCmd, dt);

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

        #region Member Creation Data
        public List<MemberCreationSubData> MemberCreationData(PostMemberData postMemberData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberCreationSubData> row = new List<MemberCreationSubData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetMember";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMemberData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postMemberData.pEoId);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new MemberCreationSubData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["Marketid"].ToString(), rs["MarketName"].ToString(), rs["Groupid"].ToString(),
                            rs["GroupName"].ToString(), rs["CollDay"].ToString(), rs["EnquiryId"].ToString(), rs["Name"].ToString(), rs["MemType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberCreationSubData("No data available", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberCreationSubData("No data available", ex.ToString(), "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<GetNewMemberInfo> NewMemberCreation(PostMemberFormData postMemberFormData)
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            List<GetNewMemberInfo> row = new List<GetNewMemberInfo>();
            List<EarningMemberDtl> row1 = new List<EarningMemberDtl>();
            string vImgPath = "", vEnqId = postMemberFormData.pEnquiryId;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                //oCmd.CommandText = "GetInitialApproachById";
                oCmd.CommandText = "Mob_GetInitialApproachById";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@vEnqId", vEnqId);
                //DBConnect.ExecuteForSelect(oCmd, dt);
                ds = DBConnect.GetDataSet(oCmd);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs1 in dt1.Rows)
                    {
                        row1.Add(new EarningMemberDtl(rs1["SlNo"].ToString(), rs1["Name"].ToString(), rs1["DOB"].ToString(), rs1["Relation"].ToString(),
                            rs1["Address1"].ToString(), rs1["StateId"].ToString(), rs1["PinCode"].ToString(), rs1["MobileNo"].ToString(),
                            rs1["KYCType"].ToString(), rs1["KYCNo"].ToString(), rs1["BusinessTypeId"].ToString(),
                            rs1["OccupationId"].ToString(), rs1["DeclaredIncome"].ToString(), rs1["IncomeFrequencyId"].ToString(),
                            rs1["BusinessActvId"].ToString()));
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        //string pRequestdata = "{\"pEnquiryId\":\"" + vEnqId + "\"}";
                        //HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetImage", pRequestdata);
                        vImgPath = "https://unityimage.bijliftt.com/DownloadImage/";

                        row.Add(new GetNewMemberInfo(rs["MF_Name"].ToString(), rs["MM_Name"].ToString(), rs["ML_Name"].ToString(), rs["MDOB"].ToString(), rs["Age"].ToString(),
                            rs["FamilyPersonName"].ToString(), rs["HumanRelationId"].ToString(), rs["IdentyPRofId"].ToString(), rs["IdentyProfNo"].ToString(), rs["AddProfId"].ToString(),
                            rs["AddProfNo"].ToString(), rs["AddProfId2"].ToString(), rs["AddProfNo2"].ToString(), rs["AddrType"].ToString(), rs["HouseNo"].ToString(), rs["Street"].ToString(),
                            rs["Area"].ToString(), rs["Village"].ToString(), rs["WardNo"].ToString(), rs["District"].ToString(), rs["State"].ToString(), rs["Landmark"].ToString(),
                            rs["PostOff"].ToString(), rs["PIN"].ToString(), rs["MobileNo"].ToString(), rs["EmailId"].ToString(), rs["AddrType_p"].ToString(), rs["HouseNo_p"].ToString(),
                            rs["Street_p"].ToString(), rs["Area_p"].ToString(), rs["VillageId_p"].ToString(), rs["WardNo_p"].ToString(), rs["Landmark_p"].ToString(), rs["PostOff_p"].ToString(),
                            rs["PIN_p"].ToString(), rs["MobileNo_p"].ToString(), rs["EmailId_p"].ToString(), rs["MemStatus"].ToString(),
                            rs["Gender"].ToString(), rs["BusinessTypeId"].ToString(), rs["OccupationId"].ToString(), rs["DeclaredInc"].ToString(), rs["IncFrequency"].ToString(),
                            rs["CoAppName"].ToString(), rs["CoAppFName"].ToString(), rs["CoAppMName"].ToString(), rs["CoAppLName"].ToString(),
                            rs["CoAppDOB"].ToString(), rs["CoAppAge"].ToString(), rs["CoAppGender"].ToString(), rs["CoAppRelationId"].ToString(), rs["CoAppMobileNo"].ToString(),
                            rs["CoAppAddress"].ToString(), rs["CoAppState"].ToString(), rs["CoAppPinCode"].ToString(),
                            rs["CoAppIdentyProfId"].ToString(), rs["CoAppIdentyProfNo"].ToString(), rs["CoAppBusinessTypeId"].ToString(),
                            rs["CoAppOccupationId"].ToString(), rs["CoAppDeclaredInc"].ToString(), rs["CoAppIncFrequency"].ToString(),
                            rs["CoApplicantDOB"].ToString(), row1, rs["BusinessActvId"].ToString(), rs["CoAppBusinessActvId"].ToString(),
                            rs["EMI_Obligation"].ToString(), rs["EligibleEMI"].ToString(), vImgPath, rs["EnqDate"].ToString(),
                            rs["CoAppHouseNo"].ToString(), rs["CoAppStreet"].ToString(), rs["CoAppArea"].ToString(),
                            rs["CoAppVillageId"].ToString(), rs["CoAppSubDistrict"].ToString(), rs["CoAppDistrict"].ToString(),
                            rs["CoAppLandmark"].ToString(), rs["CoAppPostOffice"].ToString(), rs["CoAppStateId"].ToString(),
                            rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString())
                            );
                    }
                }
                else
                {
                    row.Add(new GetNewMemberInfo("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", row1, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetNewMemberInfo("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", row1, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<ExistingMemberAllData> ExistingMemberCreation(PostMemberFormData postMemberFormData)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<ExstMemberData> row1 = new List<ExstMemberData>();
            List<IncomeData> row2 = new List<IncomeData>();
            List<AssetData> row3 = new List<AssetData>();
            List<ExistingMemberAllData> rowFinal = new List<ExistingMemberAllData>();
            List<EarningMemberDtl> row4 = new List<EarningMemberDtl>();
            string vImgPath = "", vEnqId = postMemberFormData.pEnquiryId;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetExistMemberData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", vEnqId);
                ds = DBConnect.GetDataSet(oCmd);

                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt1.Rows)
                    {
                        //string pRequestdata = "{\"pEnquiryId\":\"" + vEnqId + "\"}";
                        //HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetImage", pRequestdata);
                        vImgPath = "https://unityimage.bijliftt.com/DownloadImage/";

                        row1.Add(new ExstMemberData(rs["MemberId"].ToString(), rs["Distance_frm_Branch"].ToString(), rs["Distance_frm_Coll_Center"].ToString(),
                            rs["MF_Name"].ToString(), rs["MM_Name"].ToString(), rs["ML_Name"].ToString(), rs["MDOB"].ToString(), rs["Age"].ToString(), rs["M_Gender"].ToString(),
                            rs["MM_Status"].ToString(), rs["M_RelgId"].ToString(), rs["M_Caste"].ToString(), rs["M_QualificationId"].ToString(), rs["M_OccupationId"].ToString(),
                            rs["FamilyPersonName"].ToString(), rs["HumanRelationId"].ToString(), rs["MaidenNmF"].ToString(), rs["MaidenNmM"].ToString(), rs["MaidenNmL"].ToString(),
                            rs["IdentyPRofId"].ToString(), rs["IdentyProfNo"].ToString(), rs["AddProfId"].ToString(), rs["AddProfNo"].ToString(), rs["AddProfId2"].ToString(),
                            rs["AddProfNo2"].ToString(), rs["AddrType"].ToString(), rs["HouseNo"].ToString(), rs["Street"].ToString(), rs["Area"].ToString(),
                            rs["Village"].ToString(), rs["WardNo"].ToString(), rs["District"].ToString(), rs["State"].ToString(), rs["Landmark"].ToString(), rs["PostOff"].ToString(),
                            rs["PIN"].ToString(), rs["MobileNo"].ToString(), rs["EmailId"].ToString(), rs["AddrType_p"].ToString(), rs["HouseNo_p"].ToString(), rs["Street_p"].ToString(),
                            rs["Area_p"].ToString(), rs["VillageId_p"].ToString(), rs["WardNo_p"].ToString(), rs["Landmark_p"].ToString(), rs["PostOff_p"].ToString(),
                            rs["PIN_p"].ToString(), rs["MobileNo_p"].ToString(), rs["EmailId_p"].ToString(), rs["Area_Category"].ToString(), rs["YearsOfStay"].ToString(),
                            rs["MemNamePBook"].ToString(), rs["AccNo"].ToString(), rs["IFSCCode"].ToString(), rs["Acc_Type"].ToString(), rs["B_FName"].ToString(),
                            rs["B_MName"].ToString(), rs["B_LName"].ToString(), rs["DOB"].ToString(), rs["B_Age"].ToString(), rs["B_Gender"].ToString(),
                            rs["B_HumanRelationId"].ToString(), rs["B_RelgId"].ToString(), rs["B_Caste"].ToString(), rs["B_QualificationId"].ToString(),
                            rs["B_OccupationId"].ToString(), rs["B_IdentyProfId"].ToString(), rs["B_IdentyProfNo"].ToString(), rs["B_AddProfId"].ToString(),
                            rs["B_AddProfNo"].ToString(), rs["B_HouseNo"].ToString(), rs["B_Street"].ToString(), rs["B_Area"].ToString(), rs["B_VillageID"].ToString(),
                            rs["B_WardNo"].ToString(), rs["B_Landmark"].ToString(), rs["B_PostOff"].ToString(), rs["B_PIN"].ToString(), rs["B_Mobile"].ToString(),
                            rs["B_Email"].ToString(), rs["GuarFName"].ToString(), rs["GuarLName"].ToString(), rs["GuarDOB"].ToString(), rs["GuarAge"].ToString(),
                            rs["GuarGen"].ToString(), rs["GuarRel"].ToString(), rs["No_of_House_Member"].ToString(), rs["No_of_Children"].ToString(),
                            rs["NoOfDependants"].ToString(), rs["MemStatus"].ToString(),
                            rs["MemBusinessTypeId"].ToString(), rs["DeclaredInc"].ToString(), rs["IncFrequency"].ToString(),
                            rs["CoAppBusinessTypeId"].ToString(), rs["CoAppDeclaredInc"].ToString(), rs["CoAppIncFrequency"].ToString(),
                            rs["MemEmailId"].ToString(), rs["CoAppMaritalStat"].ToString(),
                            rs["BusinessActvId"].ToString(), rs["CoAppBusinessActvId"].ToString(), rs["EMI_Obligation"].ToString(),
                            rs["EligibleEMI"].ToString(), vImgPath, rs["EnqDate"].ToString(), rs["CoAppHouseNo"].ToString(), rs["CoAppStreet"].ToString(),
                            rs["CoAppArea"].ToString(), rs["CoAppVillageId"].ToString(), rs["CoAppSubDistrict"].ToString(), rs["CoAppDistrict"].ToString(),
                            rs["CoAppLandmark"].ToString(), rs["CoAppPostOffice"].ToString(), rs["CoAppStateId"].ToString(), rs["CoAppPinCode"].ToString(),
                            rs["CoAppState"].ToString(), rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString()
                         ));
                    }
                }
                else
                {
                    row1.Add(new ExstMemberData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt2.Rows)
                    {
                        row2.Add(new IncomeData(rs["FamilyIncome"].ToString(), rs["SlefIncome"].ToString(), rs["OtherIncome"].ToString(), rs["TotIncome"].ToString(),
                            rs["ExHsRntAmt"].ToString(), rs["ExpFdAmt"].ToString(), rs["ExpEduAmt"].ToString(), rs["ExpMedAmt"].ToString(), rs["ExpLnInsAmt"].ToString(),
                            rs["ExpFuelAmt"].ToString(), rs["ExpElectricAmt"].ToString(), rs["ExpTransAmt"].ToString(), rs["ExpOtherAmt"].ToString(), rs["TotalExp"].ToString(),
                            rs["Surplus"].ToString(), rs["OtherIncomeSrcId"].ToString()));
                    }
                }
                else
                {
                    row2.Add(new IncomeData("0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "-1"));
                }

                if (dt3.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt3.Rows)
                    {
                        row3.Add(new AssetData(rs["AssetName"].ToString(), rs["AssetQty"].ToString(), rs["AssetAmt"].ToString()));
                    }
                }
                else
                {
                    row3.Add(new AssetData("No data available", "", ""));
                }

                if (dt4.Rows.Count > 0)
                {
                    foreach (DataRow rs1 in dt4.Rows)
                    {
                        row4.Add(new EarningMemberDtl(rs1["SlNo"].ToString(), rs1["Name"].ToString(), rs1["DOB"].ToString(), rs1["Relation"].ToString(),
                        rs1["Address1"].ToString(), rs1["StateId"].ToString(), rs1["PinCode"].ToString(), rs1["MobileNo"].ToString(),
                        rs1["KYCType"].ToString(), rs1["KYCNo"].ToString(), rs1["BusinessTypeId"].ToString(),
                        rs1["OccupationId"].ToString(), rs1["DeclaredIncome"].ToString(), rs1["IncomeFrequencyId"].ToString(),
                        rs1["BusinessActvId"].ToString()));
                    }
                }
                else
                {
                    row4.Add(new EarningMemberDtl("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

                rowFinal.Add(new ExistingMemberAllData(row1, row2, row3, row4));
            }
            catch (Exception)
            {
                rowFinal.Add(new ExistingMemberAllData(row1, row2, row3, row4));
            }
            finally
            {
                oCmd.Dispose();
            }
            return rowFinal;
        }

        public string UploadMemberCreation(PostMemberSaveData postMemberSaveData)
        {
            SqlCommand oCmd1 = new SqlCommand();
            SqlCommand oCmd2 = new SqlCommand();
            Int32 vErr1 = 0, vErr2 = 0;
            string memberId = "", errDesc = "";
            try
            {
                oCmd1.CommandType = CommandType.StoredProcedure;
                oCmd1.CommandText = "chkIfscExistOrNot";
                DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pIfsc", postMemberSaveData.pIFSC);
                DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pExist", 0);
                DBConnect.Execute(oCmd1);
                vErr1 = Convert.ToInt32(oCmd1.Parameters["@pExist"].Value);

                if (vErr1 <= 0)
                {
                    return "Invalid IFSC Code";
                }
                else
                {
                    oCmd2.CommandType = CommandType.StoredProcedure;
                    oCmd2.CommandText = "SaveNewMember";
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", postMemberSaveData.pEnqId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 16, "@pMemberID", postMemberSaveData.pMemberID);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMeetingDay", postMemberSaveData.pMeetingDay);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pPjMeetDt", "1900-01-01");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pDistFrmBranch", Convert.ToDouble(postMemberSaveData.pDistFrmBranch));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pDistFrmCollCenter", Convert.ToDouble(postMemberSaveData.pDistFrmCollCenter));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMF_Name", postMemberSaveData.pMF_Name);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pMM_Name", postMemberSaveData.pMM_Name);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pML_Name", postMemberSaveData.pML_Name);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pM_DOB", postMemberSaveData.pM_DOB);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_Age", Convert.ToInt32(postMemberSaveData.pM_Age));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pM_Gender", postMemberSaveData.pM_Gender);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMM_Status", postMemberSaveData.pMM_Status);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pM_RelgId", postMemberSaveData.pM_RelgId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pM_Caste", postMemberSaveData.pM_Caste);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_QualificationId", Convert.ToInt32(postMemberSaveData.pM_QualificationId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_OccupationId", Convert.ToInt32(postMemberSaveData.pM_OccupationId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMHF_Name", postMemberSaveData.pMHF_Name);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pFather_YN", postMemberSaveData.pFather_YN);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMaidenNmF", postMemberSaveData.pMaidenNmF);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMaidenNmM", postMemberSaveData.pMaidenNmM);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pMaidenNmL", postMemberSaveData.pMaidenNmL);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_AddrType", Convert.ToInt32(postMemberSaveData.pM_AddrType));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_HouseNo", postMemberSaveData.pM_HouseNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_Street", postMemberSaveData.pM_Street);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_Landmark", postMemberSaveData.pM_Landmark);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_Area", postMemberSaveData.pM_Area);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_Village", postMemberSaveData.pM_Village);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_WardNo", postMemberSaveData.pM_WardNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_District", postMemberSaveData.pM_District);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_State", postMemberSaveData.pM_State);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_PostOff", postMemberSaveData.pM_PostOff);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pM_PIN", postMemberSaveData.pM_PIN);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pMemAddr", postMemberSaveData.pMemAddr);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_Mobile", postMemberSaveData.pM_Mobile);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_Phone", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_Email", postMemberSaveData.pM_Email);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_CommAddrType", Convert.ToInt32(postMemberSaveData.pM_CommAddrType));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_CommHouseNo", postMemberSaveData.pM_CommHouseNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_CommStreet", postMemberSaveData.pM_CommStreet);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_CommLandmark", postMemberSaveData.pM_CommLandmark);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pM_CommArea", postMemberSaveData.pM_CommArea);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_CommVillageId", Convert.ToInt32(postMemberSaveData.pM_CommVillageId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_CommWardNo", postMemberSaveData.pM_CommWardNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_CommPostOff", postMemberSaveData.pM_CommPostOff);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pM_CommPIN", postMemberSaveData.pM_CommPIN);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_CommMobile", postMemberSaveData.pM_CommMobile);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_CommPhone", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pM_CommEmail", postMemberSaveData.pM_CommEmail);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_AreaCatagory", Convert.ToInt32(postMemberSaveData.pM_AreaCatagory));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pM_YearsOfStay", Convert.ToDouble(postMemberSaveData.pM_YearsOfStay));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_IdentyPRofId", Convert.ToInt32(postMemberSaveData.pM_IdentyPRofId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_IdentyProfNo", postMemberSaveData.pM_IdentyProfNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_AddProfId", Convert.ToInt32(postMemberSaveData.pM_AddProfId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_AddProfNo", postMemberSaveData.pM_AddProfNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pM_AddProfId2", Convert.ToInt32(postMemberSaveData.pM_AddProfId2));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pM_AddProfNo2", postMemberSaveData.pM_AddProfNo2);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pMemNamePBook", postMemberSaveData.pMemNamePBook);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccNo", postMemberSaveData.pAccNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 11, "@pIFSC", postMemberSaveData.pIFSC);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAccType", Convert.ToInt32(postMemberSaveData.pAccType));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pB_FName", postMemberSaveData.pB_FName);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pB_MName", postMemberSaveData.pB_MName);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pB_LName", postMemberSaveData.pB_LName);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pB_DOB", postMemberSaveData.pB_DOB);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_Age", Convert.ToInt32(postMemberSaveData.pB_Age));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pB_Gender", postMemberSaveData.pB_Gender);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_HumanRelationId", Convert.ToInt32(postMemberSaveData.pB_HumanRelationId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pB_RelgId", postMemberSaveData.pB_RelgId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pB_Caste", postMemberSaveData.pB_Caste);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_QualificationId", Convert.ToInt32(postMemberSaveData.pB_QualificationId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_OccupationId", Convert.ToInt32(postMemberSaveData.pB_OccupationId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_HouseNo", postMemberSaveData.pB_HouseNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_Street", postMemberSaveData.pB_Street);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pB_Landmark", postMemberSaveData.pB_Landmark);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pB_Area", postMemberSaveData.pB_Area);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_VillageID", Convert.ToInt32(postMemberSaveData.pB_VillageID));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_WardNo", postMemberSaveData.pB_WardNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_PostOff", postMemberSaveData.pB_PostOff);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pB_PIN", postMemberSaveData.pB_PIN);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pCoBrwrAddr", postMemberSaveData.pCoBrwrAddr);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pB_Mobile", postMemberSaveData.pB_Mobile);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pB_Phone", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pB_Email", postMemberSaveData.pB_Email);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_IdentyProfId", Convert.ToInt32(postMemberSaveData.pB_IdentyProfId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_IdentyProfNo", postMemberSaveData.pB_IdentyProfNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_AddProfId", Convert.ToInt32(postMemberSaveData.pB_AddProfId));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pB_AddProfNo", postMemberSaveData.pB_AddProfNo);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGuarFName", postMemberSaveData.pGuarFName);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pGuarLName", postMemberSaveData.pGuarLName);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pGuarDOB", postMemberSaveData.pGuarDOB);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGuarAge", Convert.ToInt32(postMemberSaveData.pGuarAge));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pGuarGen", postMemberSaveData.pGuarGen);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pGuarRel", Convert.ToInt32(postMemberSaveData.pGuarRel));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfDpndnt", Convert.ToInt32(postMemberSaveData.pNoOfDpndnt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfHouseMem", Convert.ToInt32(postMemberSaveData.pNoOfHouseMem));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNoOfChildren", Convert.ToInt32(postMemberSaveData.pNoOfChildren));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pFamilyIncome", Convert.ToDouble(postMemberSaveData.pFamilyIncome));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pSelfIncome", Convert.ToDouble(postMemberSaveData.pSelfIncome));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pOtherIncome", Convert.ToDouble(postMemberSaveData.pOtherIncome));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pTotIncome", Convert.ToDouble(postMemberSaveData.pTotIncome));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@ExHsRntAmt", Convert.ToDouble(postMemberSaveData.ExHsRntAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pFdAmt", Convert.ToDouble(postMemberSaveData.pFdAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@ExEduAmt", Convert.ToDouble(postMemberSaveData.ExEduAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExMedAmt", Convert.ToDouble(postMemberSaveData.pExMedAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExLnInsAmt", Convert.ToDouble(postMemberSaveData.pExLnInsAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExpFuelAmt", Convert.ToDouble(postMemberSaveData.pExpFuelAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExpElectricAmt", Convert.ToDouble(postMemberSaveData.pExpElectricAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExpTransAmt", Convert.ToDouble(postMemberSaveData.pExpTransAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pExpOtherAmt", Convert.ToDouble(postMemberSaveData.pExpOtherAmt));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pTotExp", Convert.ToDouble(postMemberSaveData.pTotExp));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pSurplus", Convert.ToDouble(postMemberSaveData.pSurplus));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, postMemberSaveData.pXmlAsset.Length + 1, "@pXmlAsset", postMemberSaveData.pXmlAsset);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", postMemberSaveData.pBranchcode);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAdmDate", postMemberSaveData.pAdmDate);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLogDt", postMemberSaveData.pLogDt);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 10, "@pCreatedBy", Convert.ToInt32(postMemberSaveData.pCreatedBy));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMarketId", postMemberSaveData.pMarketId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pGroupId", postMemberSaveData.pGroupId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Save");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "pMobWeb", "M");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pIDBICustId", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pIDBISavingsAcNo", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pTra_Drop", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pTra_DropDate", "1900-01-01");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCGTYN", "");
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRemarks", "");

                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMemBusTypeId", postMemberSaveData.pMemBusTypeId);
                    if (postMemberSaveData.pMemEMailId == null)
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMemEMailId", DBNull.Value);
                    else
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMemEMailId", postMemberSaveData.pMemEMailId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pCoAppMaritalStat", postMemberSaveData.pCoAppMaritalStat);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOtherIncomeSrcId", postMemberSaveData.pOtherIncomeSrcId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, postMemberSaveData.pXmlEarningMemDtl.Length + 1, "@pXmlEarningMemDtl", postMemberSaveData.pXmlEarningMemDtl);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pDeclIncome", postMemberSaveData.pDeclIncome);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pIncFrequency", postMemberSaveData.pIncFrequency);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppBusTypeId", postMemberSaveData.pCoAppBusTypeId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCoAppDeclIncome", postMemberSaveData.pCoAppDeclIncome);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppIncFrequency", postMemberSaveData.pCoAppIncFrequency);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBusActvId", postMemberSaveData.pMemBusActvId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCoAppBusActvId", postMemberSaveData.pCoAppBusActvId);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pMHF_Relation", Convert.ToInt32(postMemberSaveData.pMHF_Relation));
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMinority", postMemberSaveData.pMinority);

                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pB_Village", postMemberSaveData.pB_Village);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pB_District", postMemberSaveData.pB_District);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pB_StateId", postMemberSaveData.pB_StateId);

                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsAbledYN", postMemberSaveData.pIsAbledYN);
                    DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSpeciallyAbled", Convert.ToInt32(postMemberSaveData.pSpeciallyAbled));

                    DBConnect.Execute(oCmd2);
                    vErr2 = Convert.ToInt32(oCmd2.Parameters["@pErr"].Value);
                    memberId = Convert.ToString(oCmd2.Parameters["@pMemberID"].Value);
                    errDesc = Convert.ToString(oCmd2.Parameters["@pErrDesc"].Value);

                    if (vErr2 == 0)
                    {
                        return errDesc + ":" + memberId;
                    }
                    else
                    {
                        return errDesc;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                oCmd1.Dispose();
                oCmd2.Dispose();
            }
        }
        #endregion

        #region House Visit
        public List<QNAData> GetQuestionAnswer()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<QNAData> row = new List<QNAData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHVQNA";
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new QNAData(rs["Qno"].ToString(), rs["Question"].ToString(), rs["AnsNo"].ToString(), rs["Ans"].ToString(), rs["Score"].ToString()));
                    }
                }
                else
                {
                    row.Add(new QNAData("No data available", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new QNAData("No data available", ex.ToString(), "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<QNADataNew> GetQuestionAnswerNew()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<QNADataNew> row = new List<QNADataNew>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetHVQNA_New";
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new QNADataNew(rs["Qno"].ToString(), rs["Question"].ToString(), rs["AnsNo"].ToString(), rs["Ans"].ToString(), rs["Score"].ToString(),
                             rs["Weighted"].ToString()));
                    }
                }
                else
                {
                    row.Add(new QNADataNew("No data available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new QNADataNew("No data available", ex.ToString(), "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<MemberHVData> GetHouseVisitData(PostHVData postHVData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberHVData> row = new List<MemberHVData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetHVMember";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchcode", postHVData.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postHVData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDt", postHVData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new MemberHVData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["Groupid"].ToString(), rs["GroupName"].ToString(),
                            rs["MemberID"].ToString(), rs["CGTId"].ToString(), rs["Member"].ToString(), rs["marketId"].ToString(), rs["marketName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberHVData("No data available", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberHVData("No data available", ex.ToString(), "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string SaveHouseVisit(PostHVDataSave postHVDataSave)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveHouseStatus";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTId", Convert.ToInt32(postHVDataSave.pCGTId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberID", postHVDataSave.pMemberID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pHVDt", postHVDataSave.pHVDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pHVBy", postHVDataSave.pHVBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pExpGrtDt", postHVDataSave.pExpGrtDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1", Convert.ToInt32(postHVDataSave.pQ1));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1Score", Convert.ToInt32(postHVDataSave.pQ1Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2", Convert.ToInt32(postHVDataSave.pQ2));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2Score", Convert.ToInt32(postHVDataSave.pQ2Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3", Convert.ToInt32(postHVDataSave.pQ3));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3Score", Convert.ToInt32(postHVDataSave.pQ3Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4", Convert.ToInt32(postHVDataSave.pQ4));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4Score", Convert.ToInt32(postHVDataSave.pQ4Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5", Convert.ToInt32(postHVDataSave.pQ5));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5Score", Convert.ToInt32(postHVDataSave.pQ5Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6", Convert.ToInt32(postHVDataSave.pQ6));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6Score", Convert.ToInt32(postHVDataSave.pQ6Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7", Convert.ToInt32(postHVDataSave.pQ7));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7Score", Convert.ToInt32(postHVDataSave.pQ7Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8", Convert.ToInt32(postHVDataSave.pQ8));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8Score", Convert.ToInt32(postHVDataSave.pQ8Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ9", Convert.ToInt32(postHVDataSave.pQ9));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ9Score", Convert.ToInt32(postHVDataSave.pQ9Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10", Convert.ToInt32(postHVDataSave.pQ10));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10Score", Convert.ToInt32(postHVDataSave.pQ10Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11", Convert.ToInt32(postHVDataSave.pQ11));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11Score", Convert.ToInt32(postHVDataSave.pQ11Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12", Convert.ToInt32(postHVDataSave.pQ12));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12Score", Convert.ToInt32(postHVDataSave.pQ12Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13", Convert.ToInt32(postHVDataSave.pQ13));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13Score", Convert.ToInt32(postHVDataSave.pQ13Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postHVDataSave.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postHVDataSave.pCreatedBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Save");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobWeb", "M");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14", Convert.ToInt32(postHVDataSave.pQ14));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14Score", Convert.ToInt32(postHVDataSave.pQ14Score));

                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ15", Convert.ToString(postHVDataSave.pQ15));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ15Score", Convert.ToString(postHVDataSave.pQ15Score));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ16", Convert.ToString(postHVDataSave.pQ16));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pQ16Score", Convert.ToString(postHVDataSave.pQ16Score));

                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Electric", Convert.ToString(postHVDataSave.pQ15ElectricYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Water", Convert.ToString(postHVDataSave.pQ15WaterYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Toilet", Convert.ToString(postHVDataSave.pQ15ToiletYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15Sewage", Convert.ToString(postHVDataSave.pQ15SewageYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ15LPG", Convert.ToString(postHVDataSave.pQ15LPGYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Land", Convert.ToString(postHVDataSave.pQ16LandYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Vehicle", Convert.ToString(postHVDataSave.pQ16VehicleYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16Furniture", Convert.ToString(postHVDataSave.pQ16FurnitureYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16SmartPhone", Convert.ToString(postHVDataSave.pQ16SmartPhoneYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pQ16ElectricItem", Convert.ToString(postHVDataSave.pQ16ElectricItemYN));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14SubCat", Convert.ToInt32(postHVDataSave.pQ14SubCat));
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    try
                    {
                        JocataCalling(Convert.ToString(postHVDataSave.pMemberID), Convert.ToString(postHVDataSave.pCreatedBy), Convert.ToString(postHVDataSave.pCGTId));
                    }
                    finally { }
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

        public string SaveHouseVisitNew(PostHVDataSaveNew postHVDataSaveNew)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            double vTotalScore = 0;
            vTotalScore = Convert.ToDouble(postHVDataSaveNew.pQ1Score) + Convert.ToDouble(postHVDataSaveNew.pQ2Score) + Convert.ToDouble(postHVDataSaveNew.pQ3Score)
                + Convert.ToDouble(postHVDataSaveNew.pQ4Score) + Convert.ToDouble(postHVDataSaveNew.pQ5Score) + Convert.ToDouble(postHVDataSaveNew.pQ6Score)
                + Convert.ToDouble(postHVDataSaveNew.pQ7Score) + Convert.ToDouble(postHVDataSaveNew.pQ8Score) + Convert.ToDouble(postHVDataSaveNew.pQ9Score)
                + Convert.ToDouble(postHVDataSaveNew.pQ10Score) + Convert.ToDouble(postHVDataSaveNew.pQ11Score) + Convert.ToDouble(postHVDataSaveNew.pQ12Score)
                + Convert.ToDouble(postHVDataSaveNew.pQ13Score) + Convert.ToDouble(postHVDataSaveNew.pQ14Score) + Convert.ToDouble(postHVDataSaveNew.pQ15Score);
            try
            {
                if (vTotalScore >= 50)
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "SaveHouseStatusNew";
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 13, "@pCGTId", postHVDataSaveNew.pCGTId);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMemberID", postHVDataSaveNew.pMemberID);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pHVDt", postHVDataSaveNew.pHVDt);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pHVBy", postHVDataSaveNew.pHVBy);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pExpGrtDt", postHVDataSaveNew.pExpGrtDt);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1", Convert.ToInt32(postHVDataSaveNew.pQ1));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ1Score", Convert.ToDouble(postHVDataSaveNew.pQ1Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ1Weighted", Convert.ToInt32(postHVDataSaveNew.pQ1Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2", Convert.ToInt32(postHVDataSaveNew.pQ2));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ2Score", Convert.ToDouble(postHVDataSaveNew.pQ2Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ2Weighted", Convert.ToInt32(postHVDataSaveNew.pQ2Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3", Convert.ToInt32(postHVDataSaveNew.pQ3));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ3Score", Convert.ToDouble(postHVDataSaveNew.pQ3Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ3Weighted", Convert.ToInt32(postHVDataSaveNew.pQ3Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4", Convert.ToInt32(postHVDataSaveNew.pQ4));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ4Score", Convert.ToDouble(postHVDataSaveNew.pQ4Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ4Weighted", Convert.ToInt32(postHVDataSaveNew.pQ4Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5", Convert.ToInt32(postHVDataSaveNew.pQ5));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ5Score", Convert.ToDouble(postHVDataSaveNew.pQ5Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ5Weighted", Convert.ToInt32(postHVDataSaveNew.pQ5Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6", Convert.ToInt32(postHVDataSaveNew.pQ6));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ6Score", Convert.ToDouble(postHVDataSaveNew.pQ6Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ6Weighted", Convert.ToInt32(postHVDataSaveNew.pQ6Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7", Convert.ToInt32(postHVDataSaveNew.pQ7));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ7Score", Convert.ToDouble(postHVDataSaveNew.pQ7Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ7Weighted", Convert.ToInt32(postHVDataSaveNew.pQ7Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8", Convert.ToInt32(postHVDataSaveNew.pQ8));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ8Score", Convert.ToDouble(postHVDataSaveNew.pQ8Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ8Weighted", Convert.ToInt32(postHVDataSaveNew.pQ8Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pQ9", postHVDataSaveNew.pQ9);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ9Score", Convert.ToDouble(postHVDataSaveNew.pQ9Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ9Weighted", Convert.ToDouble(postHVDataSaveNew.pQ9Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10", Convert.ToInt32(postHVDataSaveNew.pQ10));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ10Score", Convert.ToDouble(postHVDataSaveNew.pQ10Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ10Weighted", Convert.ToInt32(postHVDataSaveNew.pQ10Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11", Convert.ToInt32(postHVDataSaveNew.pQ11));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ11Score", Convert.ToDouble(postHVDataSaveNew.pQ11Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ11Weighted", Convert.ToInt32(postHVDataSaveNew.pQ11Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12", Convert.ToInt32(postHVDataSaveNew.pQ12));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ12Score", Convert.ToDouble(postHVDataSaveNew.pQ12Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ12Weighted", Convert.ToInt32(postHVDataSaveNew.pQ12Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13", Convert.ToInt32(postHVDataSaveNew.pQ13));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ13Score", Convert.ToDouble(postHVDataSaveNew.pQ13Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ13Weighted", Convert.ToInt32(postHVDataSaveNew.pQ13Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14", Convert.ToInt32(postHVDataSaveNew.pQ14));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ14Score", Convert.ToDouble(postHVDataSaveNew.pQ14Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ14Weighted", Convert.ToInt32(postHVDataSaveNew.pQ14Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ15", Convert.ToInt32(postHVDataSaveNew.pQ15));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pQ15Score", Convert.ToDouble(postHVDataSaveNew.pQ15Score));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pQ15Weighted", Convert.ToInt32(postHVDataSaveNew.pQ15Weighted));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postHVDataSaveNew.pBranchCode);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", postHVDataSaveNew.pCreatedBy);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLatitude", postHVDataSaveNew.pLatitude);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLongitude", postHVDataSaveNew.pLongitude);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Save");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1000, "@pMsg", "");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobWeb", "M");

                    DBConnect.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    if (vErr == 0)
                    {
                        try
                        {
                            JocataCalling(Convert.ToString(postHVDataSaveNew.pMemberID), Convert.ToString(postHVDataSaveNew.pCreatedBy), Convert.ToString(postHVDataSaveNew.pCGTId));
                        }
                        finally { }
                        return "Record saved successfully";
                    }
                    else
                    {
                        return "Data Not Saved";
                    }
                }
                else
                {
                    return "Total Score Cannot be Lesser than 50";
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

        public List<GetReHouseData> GetReHouseVisitData(PostReHVData postReHVData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<GetReHouseData> row = new List<GetReHouseData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetSanctionList";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postReHVData.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postReHVData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@peoId", postReHVData.peoId);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetReHouseData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["Groupid"].ToString(), rs["GroupName"].ToString(), rs["MemberID"].ToString(),
                            rs["MemberName"].ToString(), rs["LoanAppId"].ToString(), rs["LoanAppAmt"].ToString(), rs["EnquiryId"].ToString(), rs["EnqDate"].ToString(),
                            rs["CGTID"].ToString(), rs["ColDay"].ToString(), rs["MemAddr"].ToString(), rs["M_Mobile"].ToString(), rs["EmailId"].ToString(), rs["M_IdentyPRofId"].ToString(),
                            rs["M_IdentyProfNo"].ToString(), rs["M_AddProfId"].ToString(), rs["M_AddProfNo"].ToString(), rs["AddProfId2"].ToString(), rs["AddProfNo2"].ToString(),
                            rs["CoBrwrName"].ToString(), rs["CoBrwrAge"].ToString(), rs["CoBrwrRelation"].ToString(), rs["CoBrwrAddr"].ToString(), rs["GuarName"].ToString(),
                            rs["GuarAge"].ToString(), rs["GuarRel"].ToString(), rs["FamilyIncome"].ToString(), rs["SlefIncome"].ToString(), rs["OtherIncome"].ToString(),
                            rs["TotIncome"].ToString(), rs["ExHsRntAmt"].ToString(), rs["ExpFdAmt"].ToString(), rs["ExpEduAmt"].ToString(), rs["ExpMedAmt"].ToString(),
                            rs["ExpLnInsAmt"].ToString(), rs["ExpFuelAmt"].ToString(), rs["ExpElectricAmt"].ToString(), rs["ExpTransAmt"].ToString(), rs["ExpOtherAmt"].ToString(),
                            rs["TotalExp"].ToString(), rs["Surplus"].ToString(), rs["AssetsDetails"].ToString(), rs["Q1A"].ToString(), rs["Q2A"].ToString(), rs["Q3A"].ToString(),
                            rs["Q4A"].ToString(), rs["Q5A"].ToString(), rs["Q6A"].ToString(), rs["Q7A"].ToString(), rs["Q8A"].ToString(), rs["Q9A"].ToString(), rs["Q10A"].ToString(),
                            rs["Q11A"].ToString(), rs["Q12A"].ToString(), rs["Q13A"].ToString(), rs["marketId"].ToString(), rs["marketName"].ToString(), rs["Q14A"].ToString(), rs["Q15A"].ToString()
                            , rs["Q15ElectricYN"].ToString(), rs["Q15WaterYN"].ToString(), rs["Q15ToiletYN"].ToString(), rs["Q15SewageYN"].ToString(), rs["Q15LPGYN"].ToString()
                            , rs["Q16LandYN"].ToString(), rs["Q16VehicleYN"].ToString(), rs["Q16FurnitureYN"].ToString(), rs["Q16SmartPhoneYN"].ToString(), rs["Q16ElectricItemYN"].ToString(),
                            rs["Q14SubCat"].ToString(), rs["EnqqDate"].ToString()
                          ));
                    }
                }
                else
                {
                    row.Add(new GetReHouseData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                        , "", "", "", "", "", "", "", "", "", "", "", "", ""
                        ));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetReHouseData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                    , "", "", "", "", "", "", "", "", "", "", "", "", ""
                    ));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string SaveReHouseVisit(SaveReHVData saveReHVData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateSanction";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, saveReHVData.pXml.Length + 1, "@pXmlData", saveReHVData.pXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(saveReHVData.pCreatedBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", saveReHVData.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pSanDt", saveReHVData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
        #endregion

        #region Loan Purpose
        public List<PurposeData> GetLoanPurpose()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<PurposeData> row = new List<PurposeData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetPurpose";
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new PurposeData(rs["PurposeID"].ToString(), rs["Purpose"].ToString(), rs["SubPurposeID"].ToString(), rs["SubPurpose"].ToString()));
                    }
                }
                else
                {
                    row.Add(new PurposeData("No data available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new PurposeData("No data available", ex.ToString(), "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region Loan Scheme
        public List<SchemeData> GetLoanScheme(PostSchemeData postSchemeData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<SchemeData> row = new List<SchemeData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanTypeForApp";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postSchemeData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsTopup", "N");
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new SchemeData(rs["LoanTypeId"].ToString(), rs["LoanType"].ToString(), rs["AssetType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new SchemeData("No data available", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new SchemeData("No data available", ex.ToString(), ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region Loan Application Details

        private bool ColumnEqual(object A, object B)
        {
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value  
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is BNull.Value  
                return false;
            return (A.Equals(B)); // value type standard comparison  
        }
        public DataTable SelectDistinct(DataTable SourceTable, string FieldName)
        {

            DataTable dt = new DataTable(SourceTable.TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);
            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }

        public List<MemberLoanAppData> GetLoanAppData(PostLoanApp postLoanApp)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberLoanAppData> row = new List<MemberLoanAppData>();
            string vImgPath = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetMemListForLoanApp";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postLoanApp.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postLoanApp.pBranch);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    //DataTable dt1 = new DataTable();
                    //dt1 = SelectDistinct(dt, "Groupid");
                    //foreach (DataRow rs1 in dt1.Rows)
                    //{
                    //    string pRequestdata = "{\"pId\":\"" + rs1["Groupid"].ToString() + "\"}";
                    //    HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetGRTImage", pRequestdata);
                    //}
                    foreach (DataRow rs in dt.Rows)
                    {
                        vImgPath = "https://unityimage.bijliftt.com/GroupImage/" + rs["Groupid"].ToString() + "_GroupPhoto.png";
                        row.Add(new MemberLoanAppData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["Groupid"].ToString(), rs["GroupName"].ToString(),
                            rs["CGTId"].ToString(), rs["MemberID"].ToString(), rs["MemberName"].ToString(), rs["marketId"].ToString(), rs["marketName"].ToString(),
                            rs["GroupType"].ToString(), vImgPath, rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberLoanAppData("No data available", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberLoanAppData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<MemberLoanDtlData> LoanAppDtlByMember(PostLoanAppDtl postLoanAppDtl)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberLoanDtlData> row = new List<MemberLoanDtlData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanDtlByMember";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@MemberID", postLoanAppDtl.pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@AppDt", postLoanAppDtl.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        //string pReqdata = "{\"pId\":\"" + postLoanAppDtl.pMemberId + "\"}";
                        //HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetPassBookImage", pReqdata);

                        //string pRequestdata = "{\"pEnquiryId\":\"" + rs["EnquiryId"].ToString() + "\"}";
                        //HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetImage", pRequestdata);

                        row.Add(new MemberLoanDtlData(rs["EnquiryId"].ToString(), rs["AmountApplied"].ToString(), rs["MemAddr"].ToString(), rs["M_Mobile"].ToString(),
                            rs["EmailId"].ToString(), rs["M_IdentyPRofId"].ToString(), rs["M_IdentyProfNo"].ToString(), rs["M_AddProfId"].ToString(), rs["M_AddProfNo"].ToString(),
                            rs["AddProfId2"].ToString(), rs["AddProfNo2"].ToString(), rs["CoBrwrName"].ToString(), rs["CoBrwrAge"].ToString(), rs["CoBrwrRelation"].ToString(),
                            rs["CoBrwrAddr"].ToString(), rs["GuarName"].ToString(), rs["GuarAge"].ToString(), rs["GuarRel"].ToString(), rs["FamilyIncome"].ToString(),
                            rs["SlefIncome"].ToString(), rs["OtherIncome"].ToString(), rs["TotIncome"].ToString(), rs["ExHsRntAmt"].ToString(), rs["ExpFdAmt"].ToString(),
                            rs["ExpEduAmt"].ToString(), rs["ExpMedAmt"].ToString(), rs["ExpLnInsAmt"].ToString(), rs["ExpFuelAmt"].ToString(), rs["ExpElectricAmt"].ToString(),
                            rs["ExpTransAmt"].ToString(), rs["ExpOtherAmt"].ToString(), rs["TotalExp"].ToString(), rs["Surplus"].ToString(), rs["AssetsDetails"].ToString(),
                            rs["Q1A"].ToString(), rs["Q2A"].ToString(), rs["Q3A"].ToString(), rs["Q4A"].ToString(), rs["Q5A"].ToString(), rs["Q6A"].ToString(),
                            rs["Q7A"].ToString(), rs["Q8A"].ToString(), rs["Q9A"].ToString(), rs["Q10A"].ToString(), rs["Q11A"].ToString(), rs["Q12A"].ToString(),
                            rs["Q13A"].ToString(), rs["AAdharNo"].ToString(), rs["bnk_acc"].ToString(), rs["ifsc_num"].ToString(), rs["Q14A"].ToString()
                            , rs["Q15ElectricYN"].ToString(), rs["Q15WaterYN"].ToString(), rs["Q15ToiletYN"].ToString(), rs["Q15SewageYN"].ToString(), rs["Q15LPGYN"].ToString(), rs["Q15A"].ToString()
                            , rs["Q16LandYN"].ToString(), rs["Q16VehicleYN"].ToString(), rs["Q16FurnitureYN"].ToString(), rs["Q16SmartPhoneYN"].ToString(), rs["Q16ElectricItemYN"].ToString()
                            , rs["EligibleEMI"].ToString(), rs["EMIEligibleAmt"].ToString(), rs["AmountApplied24M"].ToString(), rs["MaxLoanAmt12M"].ToString(), rs["MaxLoanAmt24M"].ToString()
                            , rs["AssetType"].ToString(), rs["Q14SubCat"].ToString(), rs["ApplAadhaarNo"].ToString(), "https://unityimage.bijliftt.com/PassBook/", "https://unityimage.bijliftt.com/DownloadImage/"
                            , rs["EnqDate"].ToString(), rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberLoanDtlData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                        , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                        ));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberLoanDtlData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                    , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                    ));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region LoanAmtByMemberScheme
        public List<MemberLoanDtlByTypeData> LoanAmtByMemberScheme(PostLoanAppDtlByMember postLoanAppDtlByMember)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberLoanDtlByTypeData> row = new List<MemberLoanDtlByTypeData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAmtAndCycleByLTypeID";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanTypeId", Convert.ToInt32(postLoanAppDtlByMember.pLoanAppId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", postLoanAppDtlByMember.pMemberId);

                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new MemberLoanDtlByTypeData(rs["TRN"].ToString(), rs["LoanAmt"].ToString(), rs["LoanCycle"].ToString(), rs["ApLoanCycle"].ToString(),
                            rs["PrvLoanYN"].ToString(), rs["AppliedAmt"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberLoanDtlByTypeData("No data available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberLoanDtlByTypeData("No data available", ex.ToString(), "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region Common Function
        internal DataTable GetFinYearList(string pBrCode)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllFinYear";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        internal string getFinYrNo(string pYear)
        {
            string vYrNo = "";
            if (pYear.Length == 1)
                vYrNo = pYear.Insert(0, "0");
            return (vYrNo);
        }

        internal DataTable GetFinYearAll(string fYear)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetFinYearByYear";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 9, "@pYear", fYear);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        public static string RemoveSpecialCharecters(string text)
        {
            return Regex.Replace(text, "[^a-zA-Z0-9,/ -]", "");
        }

        #endregion

        #region SaveGRT
        public string SaveGRT(SaveGRTData saveGRTData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vFinYrNo = "";
            string vErrDesc = "";
            try
            {
                if (saveGRTData.pAppStatus.ToString() == "IR")
                {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "RejectGRT";
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDate", saveGRTData.pAppDate);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", saveGRTData.pMemberId);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTID", Convert.ToInt32(saveGRTData.pCGTID));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", saveGRTData.pBrCode);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", saveGRTData.pEoid);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pAppStatus", saveGRTData.pAppStatus);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRejectReason", saveGRTData.pRejectReason);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(saveGRTData.pCreatedBy));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                    DBConnect.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                    vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                    if (vErr == 0)
                    {
                        return "Record saved successfully";
                    }
                    else
                    {
                        return "Data Not Saved";
                    }
                }

                dt = GetFinYearList(saveGRTData.pBrCode);
                if (dt.Rows.Count > 0)
                {
                    vFinYrNo = dt.Rows[0]["YrNo"].ToString();
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = "InsertApplication";
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 16, "@pLoanAppNo", "");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDate", saveGRTData.pAppDate);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", saveGRTData.pMemberId);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@ploantypeid", Convert.ToInt32(saveGRTData.ploantypeid));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 4, "@pLoanAppAmt", Convert.ToDouble(saveGRTData.pLoanAppAmt));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pPurposeId", Convert.ToInt32(saveGRTData.pPurposeId));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSubPurposeId", Convert.ToInt32(saveGRTData.pSubPurposeId));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanCycle", Convert.ToInt32(saveGRTData.pLoanCycle));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTID", Convert.ToInt32(saveGRTData.pCGTID));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", saveGRTData.pBrCode);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(saveGRTData.pCreatedBy));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pYrNo", Convert.ToInt32(vFinYrNo));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pNomName", saveGRTData.pNomName);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNomAddress", "");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNomAge", Convert.ToInt32(saveGRTData.pNomAge));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pNomRel", Convert.ToInt32(saveGRTData.pNomRel));
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobWeb", "M");
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", saveGRTData.pEoid);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pAppStatus", saveGRTData.pAppStatus);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRejectReason", saveGRTData.pRejectReason);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEligibleAmt", saveGRTData.pEligibleAmt);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pRefNo", saveGRTData.pRefNo);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pSurplus", saveGRTData.pSurplus);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEligibleEMI", saveGRTData.pEligibleEMI);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEMIEligibleAmt", saveGRTData.pEMIEligibleAmt);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pEligibleLoanAmt", saveGRTData.pEligibleLoanAmt);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEligibleLoanAmtText", saveGRTData.pEligibleLoanAmtText);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLatitude", saveGRTData.pLatitude);
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLongitude", saveGRTData.pLongitude);
                    DBConnect.Execute(oCmd);
                    vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);

                    if (vErr == 0)
                    {
                        return "Record saved successfully";
                    }
                    else if (vErr == 3)
                    {
                        return "Already One Loan is Active..";
                    }
                    else if (vErr == 4)
                    {
                        return "One Approved Loan  Application is Pending for Disburse..";
                    }
                    else if (vErr == 5)
                    {
                        return "Invalid Loan Scheme Selected..";
                    }
                    else if (vErr == 6)
                    {
                        return "Applied Amount is Greater than CB Amount so you have to Recheck CB..";
                    }
                    else if (vErr == 7)
                    {
                        return "CB Pending for Approval..";
                    }
                    else if (vErr == 8)
                    {
                        return "Applied Amount should be within the Range..";
                    }
                    else if (vErr == 9)
                    {
                        return "Collection Routine of Center and Scheme Payment Schedule Should be Matched..";
                    }
                    else if (vErr == 10)
                    {
                        return "Village is Missing in Group Master..";
                    }
                    else if (vErr == 11)
                    {
                        return "Please select proper Loan Scheme..";
                    }
                    else if (vErr == 12)
                    {
                        return "Member creation date and GRT date should not be same..";
                    }
                    else if (vErr == 13)
                    {
                        return "GRT is not possible. Either Initial approach or member creation or House visit of this member is done by you..";
                    }
                    else if (vErr == 14)
                    {
                        return "Applied EMI Amount is Greater than Eligible EMI Amount..";
                    }
                    else if (vErr == 15)
                    {
                        return "Selected Loan Eligible Amount not Matching with Loan Scheme..";
                    }
                    else if (vErr == 16)
                    {
                        return "Loan Applied amount should not be more than Selected Loan Eligible Amount..";
                    }
                    else if (vErr == 17)
                    {
                        return "Day End Already Done..";
                    }
                    else
                    {
                        return "Data Not Saved";
                    }

                }
                else
                {
                    return "Financial Year Not Found";
                }

                if (saveGRTData.pAppStatus.ToString() == "R")
                {
                    dt = GetFinYearList(saveGRTData.pBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        vFinYrNo = dt.Rows[0]["YrNo"].ToString();
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandText = "InsertApplication";
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 16, "@pLoanAppNo", "");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDate", saveGRTData.pAppDate);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", saveGRTData.pMemberId);
                        DBConnect.Execute(oCmd);

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
                    else
                    {
                        return "Financial Year Not Found";
                    }
                }
                else
                {
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
        #endregion

        #region GetMemberLoanDashboard
        public List<MemberLoanDashboardData> GetMemberLoanDashboard(PostDashboardData postDashboardData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<MemberLoanDashboardData> row = new List<MemberLoanDashboardData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetMemberLoanDashboard";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", Convert.ToInt32(postDashboardData.pUserId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postDashboardData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pDate", postDashboardData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", postDashboardData.pEoId);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new MemberLoanDashboardData(rs["TotalMember"].ToString(), rs["TotalApplication"].ToString(), rs["TotalMemberCurrent"].ToString(),
                            rs["TotalLoanCurrent"].ToString()));
                    }
                }
                else
                {
                    row.Add(new MemberLoanDashboardData("No data available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new MemberLoanDashboardData("No data available", ex.ToString(), "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region Collection
        public List<CollectionData> GetCollectionData(PostCollectionData postCollectionData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<CollectionData> row = new List<CollectionData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetAllLoanCollection";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLnStatus", "O");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pRoutine", Convert.ToInt32(postCollectionData.pRoutin));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRecvDt", postCollectionData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEOId", postCollectionData.pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMktID", "-1");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pGroupId", "-1");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postCollectionData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberID", "-1");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsHolidayColl", postCollectionData.pIsHoliday);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pHolidayFrmDt", postCollectionData.pFromDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pHolidayToDt", postCollectionData.pToDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPTPYN", postCollectionData.pPTPYN);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new CollectionData(rs["GroupId"].ToString(), rs["GroupName"].ToString(), rs["MemberId"].ToString(), rs["MemberName"].ToString(),
                            rs["MemberNo"].ToString(), rs["ProductID"].ToString(), rs["IntRate"].ToString(), rs["LoanId"].ToString(), rs["LoanNo"].ToString(),
                            rs["PrincpalDue"].ToString(), rs["PrincpalPaid"].ToString(), rs["InterestDue"].ToString(), rs["InterestPaid"].ToString(), rs["TotalDue"].ToString(),
                            rs["Total"].ToString(), rs["PrincOS"].ToString(), rs["ClosingType"].ToString(), rs["DescID"].ToString(), rs["AdvanceAmt"].ToString(),
                            rs["OverDue"].ToString(), rs["SlNo"].ToString(), rs["ReffId"].ToString(), rs["ChequeNo"].ToString(), rs["CollType"].ToString(),
                            rs["Noofinst"].ToString(), rs["PA"].ToString(), rs["LoanAc"].ToString(), rs["InstAc"].ToString(), rs["AdvAc"].ToString(), rs["Reason"].ToString(),
                            rs["ActionTaken"].ToString(), rs["DeathDt"].ToString(), rs["LWaveOffId"].ToString(), rs["DPerson"].ToString(), rs["IsWriteoff"].ToString(),
                            rs["WriteOffAC"].ToString(), rs["WriteOffRecAC"].ToString(), rs["DeathType"].ToString(), rs["Duedt"].ToString(), rs["OverDueGroupYn"].ToString(),
                            rs["GCollDay"].ToString(), rs["GColltime"].ToString(), rs["ExcAmt"].ToString(), rs["NewAdvanceAmt"].ToString(), rs["PreMatInt"].ToString(),
                            rs["ProvDeathDec"].ToString(), rs["InstallmentAmt"].ToString(), rs["MarketID"].ToString(), rs["MarketName"].ToString(), rs["IntOS"].ToString(),
                            rs["SecMoNo"].ToString(), rs["DeathFlag"].ToString()));
                    }
                }
                else
                {
                    row.Add(new CollectionData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new CollectionData("No data available", "0x80131904", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string InsertCollection(SaveCollectionData saveCollectionData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string ACVouMst = null;
            string ACVouDtl = null;
            string vFinYrNo = "";
            string vFinYr = "";
            string pShortYear = "";
            string vReceiptNo = "";
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                dt1 = GetFinYearList(saveCollectionData.pBranch);
                if (dt1.Rows.Count > 0)
                {
                    vFinYrNo = getFinYrNo(dt1.Rows[0]["YrNo"].ToString());
                    vFinYr = dt1.Rows[0]["FYear"].ToString();
                    ACVouMst = "ACVouMst" + vFinYrNo;
                    ACVouDtl = "ACVouDtl" + vFinYrNo;

                    dt2 = GetFinYearAll(vFinYr);
                    if (dt2.Rows.Count == 0)
                    {
                        return "Data not saved";
                    }
                    else
                    {
                        pShortYear = dt2.Rows[0]["ShortYear"].ToString();
                        oCmd.CommandType = CommandType.StoredProcedure;
                        oCmd.CommandText = "Mob_Srv_SaveCollection";
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAccDate", saveCollectionData.pDate);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, saveCollectionData.pXml.Length + 1, "@pXml", saveCollectionData.pXml);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", saveCollectionData.pBranch);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblMst", ACVouMst);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pTblDtl", ACVouDtl);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pModeAC", "C0001");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pShortYear);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pNaration", "Being the Amt of Loan Collection from ");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(saveCollectionData.pUserId));
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSynStatus", 0);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pCollMode", "C");
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEoID", saveCollectionData.pEoId);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@PCollectionMode", 'M');
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pValueDate", saveCollectionData.pValueDate);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pReceiptNo", saveCollectionData.pReceiptNo);
                        DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pManualReceiptNo", saveCollectionData.pReceiptNo);
                        DBConnect.Execute(oCmd);
                        vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                        vReceiptNo = Convert.ToString(oCmd.Parameters["@pReceiptNo"].Value);

                        if (vErr == 0)
                        {
                            return "Record saved successfully" + "#" + vReceiptNo;
                        }
                        else if (vErr == 1)
                        {
                            return "Collection amount should not be more than outstanding " + "#XX";
                        }
                        else if (vErr == 5)
                        {
                            return "ReceiptNo already exist" + "#XX";
                        }
                        else if (vErr == 6)
                        {
                            return "Cash reconciliation already done" + "#XX";
                        }
                        else if (vErr == 7)
                        {
                            return "Multiple collection against single loan not allowed for this role" + "#XX";
                        }
                        else
                        {
                            return "Data Not Saved" + "#XX";
                        }
                    }
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

        #region SaveEmployeeAttendance
        public string SaveEmployeeAttendance(PostSaveEmployeeAttendance PostSaveEmployeeAttendance)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vAttDate = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_Save_EmpAttendance";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEmpCode", PostSaveEmployeeAttendance.pEmpCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pAttType", PostSaveEmployeeAttendance.pAttType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 20, "@pLatitute", PostSaveEmployeeAttendance.pLatitute);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Decimal, 20, "@pLongitute", PostSaveEmployeeAttendance.pLongitute);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pIMEI1", PostSaveEmployeeAttendance.pIMEI1);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pIMEI2", PostSaveEmployeeAttendance.pIMEI2);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pAddress", PostSaveEmployeeAttendance.pAddress);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10, "@pAttDate", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                vAttDate = Convert.ToString(oCmd.Parameters["@pAttDate"].Value);

                return vAttDate;


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

        #region GetEquifaxReport
        public Stream GetEquifaxReport(ReportData reportData)
        {
            DataSet ds = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            var stream = new MemoryStream();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "RptEquifaxSinglePager";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnquiryId", reportData.pEnquiryId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCBID", Convert.ToInt32(reportData.pCbId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@LoginDate", reportData.pDate);
                ds = DBConnect.GetDataSet(oCmd);

                StringBuilder sbHtml = new StringBuilder();

                if (ds.Tables.Count == 0)
                {
                    sbHtml.Append("No Record Found !!");
                    var writer = new StreamWriter(stream);
                    writer.Write(sbHtml.ToString());
                    writer.Flush();
                    stream.Position = 0;
                }
                else
                {
                    DataTable dtConsumerBasicInformation = null, dtInputEnquiry = null, dtPersonalInformation = null, dtConsumerAddress = null, dtSummary = null,
                        dtMfiDetails = null, dtEnquiries = null;
                    sbHtml.Append("<Html>");
                    sbHtml.Append("<head>");
                    #region Style Sheet
                    sbHtml.Append(@"<style type='text/css'>
        body
        {
            margin-top: 0px;
            margin-left: 0px;
        }
        
        #page_1
        {
            position: relative;
            overflow: hidden;
            margin: 59px 0px 45px 53px;
            padding: 0px;
            border: none;
            width: 958px;
        }
        
        #page_1 #p1dimg1
        {
            position: absolute;
            top: 0px;
            left: 0px;
            z-index: -1;
            width: 99%;
            height: 813px;
        }
        #page_1 #p1dimg1 #p1img1
        {
            width: 99%;
            height: 813px;
        }
        
        
        
        
        #page_2
        {
            position: relative;
            overflow: hidden;
            margin: 59px 0px 43px 53px;
            padding: 0px;
            border: none;
            width: 958px;
        }
        
        #page_2 #p2dimg1
        {
            position: absolute;
            top: 0px;
            left: 0px;
            z-index: -1;
            width: 99%;
            height: 134px;
        }
        #page_2 #p2dimg1 #p2img1
        {
            width: 99%;
            height: 134px;
        }
        
        
        
        
        .dclr
        {
            clear: both;
            float: none;
            height: 1px;
            margin: 0px;
            padding: 0px;
            overflow: hidden;
        }
        
        .ft0
        {
            font: bold 16px 'Courier New';
            line-height: 18px;
        }
        .ft1
        {
            font: 11px 'Courier New';
            color: #800000;
            line-height: 14px;
        }
        .ft2
        {
            font: 11px 'Courier New';
            line-height: 14px;
        }
        .ft3
        {
            font: 10px 'Courier New';
            line-height: 12px;
        }
        .ft4
        {
            font: 9px 'Courier New';
            line-height: 13px;
        }
        .ft5
        {
            font: 10px 'Courier New';
            color: #800000;
            line-height: 12px;
        }
        .ft6
        {
            font: bold 11px 'Courier New';
            line-height: 14px;
        }
        .ft7
        {
            font: bold 9px 'Courier New';
            color: #ffffff;
            line-height: 12px;
        }
        .ft8
        {
            font: 9px 'Courier New';
            line-height: 12px;
        }
        .ft9
        {
            font: 9px 'Courier New';
            line-height: 9px;
        }
        .ft10
        {
            font: 1px 'Courier New';
            line-height: 9px;
        }
        .ft11
        {
            font: 1px 'Courier New';
            line-height: 10px;
        }
        .ft12
        {
            font: 9px 'Courier New';
            line-height: 10px;
        }
        .ft13
        {
            font: bold 11px 'Courier New';
            line-height: 12px;
        }
        .ft14
        {
            font: bold 11px 'Courier New';
            line-height: 11px;
        }
        .ft15
        {
            font: 7px 'Courier New';
            color: #ffffff;
            line-height: 8px;
        }
        .ft16
        {
            font: 1px 'Courier New';
            line-height: 7px;
        }
        .ft17
        {
            font: 7px 'Courier New';
            color: #ffffff;
            line-height: 7px;
        }
        .ft18
        {
            font: 8px 'Courier New';
            line-height: 8px;
        }
        .ft19
        {
            font: 9px 'Courier New';
            line-height: 11px;
        }
        .ft20
        {
            font: 8px 'Courier New';
            color: #808080;
            line-height: 8px;
        }
        .ft21
        {
            font: 7px 'Courier New';
            color: #808080;
            line-height: 8px;
        }
        
        .p0
        {
            text-align: left;
            padding-left: 193px;
            margin-top: 7px;
            margin-bottom: 0px;
        }
        .p1
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p2
        {
            text-align: left;
            padding-left: 9px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p3
        {
            text-align: left;
            padding-left: 20px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p4
        {
            text-align: left;
            padding-left: 4px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p5
        {
            text-align: left;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p6
        {
            text-align: left;
            padding-left: 14px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p7
        {
            text-align: left;
            padding-left: 68px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p8
        {
            text-align: left;
            padding-left: 61px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p9
        {
            text-align: left;
            padding-left: 2px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p10
        {
            text-align: left;
            margin-top: 0px;
            margin-bottom: 0px;
        }
        .p11
        {
            text-align: left;
            padding-left: 18px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p12
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 4px;
            margin-bottom: 0px;
        }
        .p13
        {
            text-align: left;
            padding-left: 19px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p14
        {
            text-align: left;
            padding-left: 17px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p15
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 11px;
            margin-bottom: 0px;
        }
        .p16
        {
            text-align: left;
            padding-left: 83px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p17
        {
            text-align: right;
            padding-right: 23px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p18
        {
            text-align: left;
            padding-left: 24px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p19
        {
            text-align: left;
            padding-left: 12px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p20
        {
            text-align: left;
            padding-left: 10px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p21
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 5px;
            margin-bottom: 0px;
        }
        .p22
        {
            text-align: left;
            padding-left: 21px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p23
        {
            text-align: left;
            padding-left: 16px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p24
        {
            text-align: left;
            padding-left: 15px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p25
        {
            text-align: left;
            padding-left: 11px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p26
        {
            text-align: center;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p27
        {
            text-align: center;
            padding-right: 1px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p28
        {
            text-align: center;
            padding-left: 2px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p29
        {
            text-align: left;
            padding-left: 76px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p30
        {
            text-align: left;
            padding-left: 44px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p31
        {
            text-align: left;
            padding-left: 36px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p32
        {
            text-align: left;
            padding-left: 40px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p33
        {
            text-align: center;
            padding-left: 1px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p34
        {
            text-align: right;
            padding-right: 30px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p35
        {
            text-align: right;
            padding-right: 43px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p36
        {
            text-align: right;
            padding-right: 83px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p37
        {
            text-align: left;
            padding-left: 6px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p38
        {
            text-align: right;
            padding-right: 3px;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        .p39
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 0px;
            margin-bottom: 0px;
        }
        .p40
        {
            text-align: left;
            padding-left: 3px;
            margin-top: 2px;
            margin-bottom: 0px;
        }
        .p41
        {
            text-align: left;
            padding-left: 3px;
            padding-right: 74px;
            margin-top: 0px;
            margin-bottom: 0px;
        }
        .p42
        {
            text-align: right;
            margin-top: 0px;
            margin-bottom: 0px;
            white-space: nowrap;
        }
        
        .td0
        {
            padding: 0px;
            margin: 0px;
            width: 114px;
            vertical-align: bottom;
        }
        .td1
        {
            padding: 0px;
            margin: 0px;
            width: 136px;
            vertical-align: bottom;
        }
        .td2
        {
            padding: 0px;
            margin: 0px;
            width: 82px;
            vertical-align: bottom;
        }
        .td3
        {
            padding: 0px;
            margin: 0px;
            width: 79px;
            vertical-align: bottom;
        }
        .td4
        {
            padding: 0px;
            margin: 0px;
            width: 107px;
            vertical-align: bottom;
        }
        .td5
        {
            padding: 0px;
            margin: 0px;
            width: 150px;
            vertical-align: bottom;
        }
        .td6
        {
            padding: 0px;
            margin: 0px;
            width: 257px;
            vertical-align: bottom;
        }
        .td7
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 114px;
            vertical-align: bottom;
        }
        .td8
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 136px;
            vertical-align: bottom;
        }
        .td9
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 82px;
            vertical-align: bottom;
        }
        .td10
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 79px;
            vertical-align: bottom;
        }
        .td11
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 107px;
            vertical-align: bottom;
        }
        .td12
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 150px;
            vertical-align: bottom;
        }
        .td13
        {
            border-left: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 249px;
            vertical-align: bottom;
            background: #800000;
        }
        .td14
        {
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 161px;
            vertical-align: bottom;
            background: #800000;
        }
        .td15
        {
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 107px;
            vertical-align: bottom;
            background: #800000;
        }
        .td16
        {
            border-right: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 149px;
            vertical-align: bottom;
            background: #800000;
        }
        .td17
        {
            border-left: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td18
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 149px;
            vertical-align: bottom;
        }
        .td19
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 256px;
            vertical-align: bottom;
        }
        .td20
        {
            border-left: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td21
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 149px;
            vertical-align: bottom;
        }
        .td22
        {
            border-left: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 69px;
            vertical-align: bottom;
            background: #800000;
        }
        .td23
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 366px;
            vertical-align: bottom;
            background: #800000;
        }
        .td24
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 48px;
            vertical-align: bottom;
            background: #800000;
        }
        .td25
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 69px;
            vertical-align: bottom;
            background: #800000;
        }
        .td26
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 114px;
            vertical-align: bottom;
            background: #800000;
        }
        .td27
        {
            border-left: #000000 1px solid;
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 69px;
            vertical-align: bottom;
        }
        .td28
        {
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 366px;
            vertical-align: bottom;
        }
        .td29
        {
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 48px;
            vertical-align: bottom;
        }
        .td30
        {
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 69px;
            vertical-align: bottom;
        }
        .td31
        {
            border-right: #000000 1px solid;
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 114px;
            vertical-align: bottom;
        }
        .td32
        {
            border-left: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 41px;
            vertical-align: bottom;
            background: #800000;
        }
        .td33
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 117px;
            vertical-align: bottom;
            background: #800000;
        }
        .td34
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 33px;
            vertical-align: bottom;
            background: #800000;
        }
        .td35
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 251px;
            vertical-align: bottom;
            background: #800000;
        }
        .td36
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 34px;
            vertical-align: bottom;
            background: #800000;
        }
        .td37
        {
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 116px;
            vertical-align: bottom;
            background: #800000;
        }
        .td38
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #800000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 74px;
            vertical-align: bottom;
            background: #800000;
        }
        .td39
        {
            border-left: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 41px;
            vertical-align: bottom;
        }
        .td40
        {
            padding: 0px;
            margin: 0px;
            width: 117px;
            vertical-align: bottom;
        }
        .td41
        {
            padding: 0px;
            margin: 0px;
            width: 33px;
            vertical-align: bottom;
        }
        .td42
        {
            padding: 0px;
            margin: 0px;
            width: 159px;
            vertical-align: bottom;
        }
        .td43
        {
            padding: 0px;
            margin: 0px;
            width: 92px;
            vertical-align: bottom;
        }
        .td44
        {
            padding: 0px;
            margin: 0px;
            width: 34px;
            vertical-align: bottom;
        }
        .td45
        {
            padding: 0px;
            margin: 0px;
            width: 116px;
            vertical-align: bottom;
        }
        .td46
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 74px;
            vertical-align: bottom;
        }
        .td47
        {
            border-left: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 41px;
            vertical-align: bottom;
        }
        .td48
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 117px;
            vertical-align: bottom;
        }
        .td49
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 33px;
            vertical-align: bottom;
        }
        .td50
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 159px;
            vertical-align: bottom;
        }
        .td51
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 92px;
            vertical-align: bottom;
        }
        .td52
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 34px;
            vertical-align: bottom;
        }
        .td53
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 116px;
            vertical-align: bottom;
        }
        .td54
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 74px;
            vertical-align: bottom;
        }
        .td55
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 132px;
            vertical-align: bottom;
            background: #800000;
        }
        .td56
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 66px;
            vertical-align: bottom;
            background: #800000;
        }
        .td57
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 52px;
            vertical-align: bottom;
            background: #800000;
        }
        .td58
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 46px;
            vertical-align: bottom;
            background: #800000;
        }
        .td59
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 53px;
            vertical-align: bottom;
            background: #800000;
        }
        .td60
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 132px;
            vertical-align: bottom;
            background: #800000;
        }
        .td61
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 66px;
            vertical-align: bottom;
            background: #800000;
        }
        .td62
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 52px;
            vertical-align: bottom;
            background: #800000;
        }
        .td63
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 46px;
            vertical-align: bottom;
            background: #800000;
        }
        .td64
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 53px;
            vertical-align: bottom;
            background: #800000;
        }
        .td65
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 132px;
            vertical-align: bottom;
            background: #800000;
        }
        .td66
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 66px;
            vertical-align: bottom;
            background: #800000;
        }
        .td67
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 52px;
            vertical-align: bottom;
            background: #800000;
        }
        .td68
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 46px;
            vertical-align: bottom;
            background: #800000;
        }
        .td69
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 53px;
            vertical-align: bottom;
            background: #800000;
        }
        .td70
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 132px;
            vertical-align: bottom;
        }
        .td71
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 66px;
            vertical-align: bottom;
        }
        .td72
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 52px;
            vertical-align: bottom;
        }
        .td73
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 46px;
            vertical-align: bottom;
        }
        .td74
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 53px;
            vertical-align: bottom;
        }
        .td75
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 132px;
            vertical-align: bottom;
        }
        .td76
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 66px;
            vertical-align: bottom;
        }
        .td77
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 52px;
            vertical-align: bottom;
        }
        .td78
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 46px;
            vertical-align: bottom;
        }
        .td79
        {
            border-right: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 53px;
            vertical-align: bottom;
        }
        .td80
        {
            border-left: #000000 1px solid;
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 212px;
            vertical-align: bottom;
            background: #800000;
        }
        .td81
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
            background: #800000;
        }
        .td82
        {
            border-right: #000000 1px solid;
            border-top: #000000 1px solid;
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 112px;
            vertical-align: bottom;
            background: #800000;
        }
        .td83
        {
            border-left: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 213px;
            vertical-align: bottom;
        }
        .td84
        {
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td85
        {
            border-right: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td86
        {
            border-left: #000000 1px solid;
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 213px;
            vertical-align: bottom;
        }
        .td87
        {
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 114px;
            vertical-align: bottom;
        }
        .td88
        {
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td89
        {
            border-right: #000000 1px solid;
            border-bottom: #c0c0c0 1px solid;
            padding: 0px;
            margin: 0px;
            width: 113px;
            vertical-align: bottom;
        }
        .td90
        {
            padding: 0px;
            margin: 0px;
            width: 214px;
            vertical-align: bottom;
        }
        .td91
        {
            padding: 0px;
            margin: 0px;
            width: 328px;
            vertical-align: bottom;
        }
        .td92
        {
            padding: 0px;
            margin: 0px;
            width: 153px;
            vertical-align: bottom;
        }
        .td93
        {
            padding: 0px;
            margin: 0px;
            width: 62px;
            vertical-align: bottom;
        }
        .td94
        {
            padding: 0px;
            margin: 0px;
            width: 67px;
            vertical-align: bottom;
        }
        .td95
        {
            padding: 0px;
            margin: 0px;
            width: 168px;
            vertical-align: bottom;
        }
        .td96
        {
            padding: 0px;
            margin: 0px;
            width: 100px;
            vertical-align: bottom;
        }
        .td97
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 153px;
            vertical-align: bottom;
        }
        .td98
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 62px;
            vertical-align: bottom;
        }
        .td99
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 67px;
            vertical-align: bottom;
        }
        .td100
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 168px;
            vertical-align: bottom;
        }
        .td101
        {
            border-bottom: #000000 1px solid;
            padding: 0px;
            margin: 0px;
            width: 100px;
            vertical-align: bottom;
        }
        .td102
        {
            padding: 0px;
            margin: 0px;
            width: 117px;
            vertical-align: bottom;
            background: #800000;
        }
        .td103
        {
            padding: 0px;
            margin: 0px;
            width: 153px;
            vertical-align: bottom;
            background: #800000;
        }
        .td104
        {
            padding: 0px;
            margin: 0px;
            width: 62px;
            vertical-align: bottom;
            background: #800000;
        }
        .td105
        {
            padding: 0px;
            margin: 0px;
            width: 67px;
            vertical-align: bottom;
            background: #800000;
        }
        .td106
        {
            padding: 0px;
            margin: 0px;
            width: 168px;
            vertical-align: bottom;
            background: #800000;
        }
        .td107
        {
            padding: 0px;
            margin: 0px;
            width: 100px;
            vertical-align: bottom;
            background: #800000;
        }
        .td108
        {
            padding: 0px;
            margin: 0px;
            width: 309px;
            vertical-align: bottom;
        }
        .td109
        {
            padding: 0px;
            margin: 0px;
            width: 49px;
            vertical-align: bottom;
        }
        
        .tr0
        {
            height: 16px;
        }
        .tr1
        {
            height: 14px;
        }
        .tr2
        {
            height: 27px;
        }
        .tr3
        {
            height: 9px;
        }
        .tr4
        {
            height: 10px;
        }
        .tr5
        {
            height: 15px;
        }
        .tr6
        {
            height: 13px;
        }
        .tr7
        {
            height: 7px;
        }
        .tr8
        {
            height: 19px;
        }
        .tr9
        {
            height: 43px;
        }
        
        .t0
        {
            width:  99%;
            margin-top: 6px;
            font: 9px 'Courier New';
        }
        .t1
        {
            width: 99%;
            margin-top: 4px;
            font: 9px 'Courier New';
            border:1px solid black;
        }
        .t2
        {
            width: 99%;
            margin-top: 3px;
            font: 9px 'Courier New';
             border:1px solid black;
        }
        .t3
        {
            width: 99%;
            margin-top: 6px;
            font: 11px 'Courier New';
              border:1px solid black;
        }
        .t4
        {
            width: 99%;
            margin-top: 10px;
            font: 8px 'Courier New';
              border:1px solid black;
        }
    </style>");
                    #endregion
                    #region Header
                    sbHtml.Append(@"<div id='page_1'>
        <div id='p1dimg1'><img src='data:image/jpg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCAMtApsDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDuvFt9beG9Q02wtbGfUbu+fYsQuliKnKheqEckn06flUutaj0S08/xJod/p7SMFt4YLuG5eXg7jgAAAfL353irfi/wdquu+MrbVEtbO4sLaFE8iW4KGYqS2D8pwMtj6D34r6h4S1WK50fUbHQLCNNPuXmbTbW8wZHOza4dlA/g5GP4RjOeG69W+51wweBcIppX66+um+nToKfE3huHUbvT72HVrK6toHnaO4EQLBV37RgnkryM4HbOeKkufEPhm18N2GuSf2n5F87LFEPL8z5SQSRnGMj17isC8aLWPF2q6B4m06LTdQ1dYzZ3iYYwEKNi7yQHBMYHy4JO5e/y66+Ede1u58PWWqWFpZ6doYEbu0wn+2KoQEhQBgNsHXpu79KPb1e43l+DjZyjbq9elnt3108iy+veHo4tbkMOrbdGlWG4O2P5mMhjGznnkE844pIdd0R7Ce/n03xBa2cVv54uLi2VUlBKhVQjILEsMdsc5wKx7j4ceJLnSb1JJLAX9/fm6upVuHCkKDsAG3n5pJCc+i/ht6l8PLmfV7ZdJvI9M021VZF8wvdNJKMgZjkbaAqk4PXk8HjB7er3JlgsAu33vpb83e3/AACqPFPhxtEOrpaa09otwbeRlEOY22hhuG7oc8H1HOOM6enX+garrNtpll/aE001ot2zrsKQoRkBz2bleAD94Vzmn/DjxLp9vq2kR3OnnTtTkjWS5YfOkaMTuWMDarHPTPGBgjqOl+H/AIV1TwjLqNlcC1lsZZPMinjOHJHHK46EYPXjHfJNCr1e4VcFgIxk4JN9Nemn4rt1Ny20GyubdJgbyMMPuSAKw+oIqX/hGrL/AJ6z/wDfQ/wrZoq/bVO55/1el/KjG/4Rqy/56z/99D/Cj/hGrL/nrP8A99D/AArZoo9tU7h9XpfymN/wjVl/z1n/AO+h/hR/wjVl/wA9Z/8Avof4Vs0Ue2qdw+r0v5TG/wCEasv+es//AH0P8KP+Easv+es//fQ/wrZoo9tU7h9XpfymN/wjVl/z1n/76H+FH/CNWX/PWf8A76H+FbNFHtqncPq9L+Uxv+Easv8AnrP/AN9D/Cj/AIRqy/56z/8AfQ/wrZoo9tU7h9XpfymN/wAI1Zf89Z/++h/hR/wjVl/z1n/76H+FbNFHtqncPq9L+Uxv+Easv+es/wD30P8ACj/hGrL/AJ6z/wDfQ/wrZoo9tU7h9XpfymN/wjVl/wA9Z/8Avof4Uf8ACNWX/PWf/vof4Vs0Ue2qdw+r0v5TG/4Rqy/56z/99D/Cj/hGrL/nrP8A99D/AArZoo9tU7h9XpfymN/wjVl/z1n/AO+h/hR/wjVl/wA9Z/8Avof4Vs0Ue2qdw+r0v5TG/wCEasv+es//AH0P8KP+Easv+es//fQ/wrZoo9tU7h9XpfymN/wjVl/z1n/76H+FH/CNWX/PWf8A76H+FbNFHtqncPq9L+Uxv+Easv8AnrP/AN9D/Cj/AIRqy/56z/8AfQ/wrZoo9tU7h9XpfymN/wAI1Zf89Z/++h/hR/wjVl/z1n/76H+FbNFHtqncPq9L+Uxv+Easv+es/wD30P8ACj/hGrL/AJ6z/wDfQ/wrZoo9tU7h9XpfymN/wjVl/wA9Z/8Avof4Uf8ACNWX/PWf/vof4Vs0Ue2qdw+r0v5TG/4Rqy/56z/99D/Cj/hGrL/nrP8A99D/AArZoo9tU7h9XpfymN/wjVl/z1n/AO+h/hR/wjVl/wA9Z/8Avof4Vs0Ue2qdw+r0v5TG/wCEasv+es//AH0P8KP+Easv+es//fQ/wrZoo9tU7h9XpfymN/wjVl/z1n/76H+FH/CNWX/PWf8A76H+FbNFHtqncPq9L+Uxv+Easv8AnrP/AN9D/Cj/AIRqy/56z/8AfQ/wrZoo9tU7h9XpfymN/wAI1Zf89Z/++h/hR/wjVl/z1n/76H+FbNFHtqncPq9L+Uxv+Easv+es/wD30P8ACj/hGrL/AJ6z/wDfQ/wrZoo9tU7h9XpfymN/wjVl/wA9Z/8Avof4Uf8ACNWX/PWf/vof4Vs0Ue2qdw+r0v5TG/4Rqy/56z/99D/Cj/hGrL/nrP8A99D/AArZoo9tU7h9XpfymN/wjVl/z1n/AO+h/hR/wjVl/wA9Z/8Avof4Vs0Ue2qdw+r0v5TG/wCEasv+es//AH0P8KP+Easv+es//fQ/wrZoo9tU7h9XpfymN/wjVl/z1n/76H+FH/CNWX/PWf8A76H+FbNFHtqncPq9L+Uxv+Easv8AnrP/AN9D/Cj/AIRqy/56z/8AfQ/wrZoo9tU7h9XpfymN/wAI1Zf89Z/++h/hR/wjVl/z1n/76H+FbNFHtqncPq9L+Uxv+Easv+es/wD30P8ACj/hGrL/AJ6z/wDfQ/wrZoo9tU7h9XpfymN/wjVl/wA9Z/8Avof4Uf8ACNWX/PWf/vof4Vs0Ue2qdw+r0v5TG/4Rqy/56z/99D/Cj/hGrL/nrP8A99D/AArZoo9tU7h9XpfymN/wjVl/z1n/AO+h/hR/wjVl/wA9Z/8Avof4Vs0Ue2qdw+r0v5TG/wCEasv+es//AH0P8KP+Easv+es//fQ/wrZoo9tU7h9XpfymN/wjVl/z1n/76H+FH/CNWX/PWf8A76H+FbNFHtqncPq9L+Uxv+Easv8AnrP/AN9D/Cj/AIRqy/56z/8AfQ/wrZoo9tU7h9XpfyhRWN/bN7/0CJ/1/wDiaP7Zvf8AoET/AK//ABNHspf00H1in/Sf+Rcv9J07VDGb/T7W7MeQhnhV9ucZxkcZwPyFXaxv7Zvf+gRP+v8A8TR/bN7/ANAif9f/AImj2Mv6aD6zDa7+5/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRWN/bN7/wBAif8AX/4mj+2b3/oET/r/APE0eyl/TQfWKf8ASf8AkbNFY39s3v8A0CJ/1/8AiaP7Zvf+gRP+v/xNHspf00H1in/Sf+Rs0Vjf2ze/9Aif9f8A4mj+2b3/AKBE/wCv/wATR7KX9NB9Yp/0n/kbNFY39s3v/QIn/X/4mj+2b3/oET/r/wDE0eyl/TQfWKf9J/5GzRRRWZsFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB//2Q=='
                id='p1img1'></div>
            <div class='dclr'></div>
            <p class='p0 ft0'>
            MICROFINANCE CONSUMER CREDIT SUMMARY</p>");
                    #endregion
                    #region dtConsumerBasicInformation
                    if (ds.Tables.Count >= 1)
                    {
                        dtConsumerBasicInformation = ds.Tables[0];
                        if (dtConsumerBasicInformation.Rows.Count > 0)
                        {

                            sbHtml.Append(@"<table cellpadding='0' cellspacing='0' class='t0'>
            <tr>
                <td class='tr0 td0'>
                    <p class='p1 ft1'>
                        CLIENT ID:</p>
                </td>
                <td class='tr0 td1'>
                    <p class='p2 ft2'>
                       " + dtConsumerBasicInformation.Rows[0]["ClientID"].ToString() + @"</p>
                </td>
                <td class='tr0 td2'>
                    <p class='p3 ft3'>
                        BranchID:</p>
                </td>
                <td class='tr0 td3'>
                    <p class='p4 ft2'>
                </td>
                <td class='tr0 td4'>
                    <p class='p5 ft4'>
                        &nbsp; " + dtConsumerBasicInformation.Rows[0]["BranchId"].ToString() + @"</p>
                </td>
                <td class='tr0 td5'>
                    <p class='p6 ft2'>
                        <span class='ft1'>DATE: </span>
                        <nobr>  " + dtConsumerBasicInformation.Rows[0]["Date"].ToString() + @"</nobr>
                    </p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td0'>
                    <p class='p1 ft5'>
                        REPORT ORDER NO:</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p2 ft2'>
                          " + dtConsumerBasicInformation.Rows[0]["ReportOrderNO"].ToString() + @"</p>
                </td>
                <td class='tr1 td2'>
                    <p class='p3 ft3'>
                        KendraID:</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p4 ft2'>
                       " + dtConsumerBasicInformation.Rows[0]["KedraId"].ToString() + @"</p>
                </td>
                <td class='tr1 td4'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr1 td5'>
                    <p class='p6 ft2'>
                        <span class='ft1'>TIME: </span>  " + dtConsumerBasicInformation.Rows[0]["Time"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr0 td0'>
                    <p class='p1 ft5'>
                        REFERENCE NUMBER:</p>
                </td>
                <td class='tr0 td1'>
                    <p class='p2 ft2'>
                        <nobr>  " + dtConsumerBasicInformation.Rows[0]["CustRefField"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr0 td2'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td3'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td colspan='2' class='tr0 td6'>
                    <p class='p5 ft1'>
                        ADDITIONAL SEARCH FIELD: <span class='ft2'>  " + dtConsumerBasicInformation.Rows[0]["AdditionalSearchField"].ToString() + @"</span></p>
                </td>
            </tr>
            <tr>
                <td class='tr0 td0'>
                    <p class='p1 ft2'>
                        Consumer Name:</p>
                </td>
                <td class='tr0 td1'>
                    <p class='p2 ft6'>
                         " + dtConsumerBasicInformation.Rows[0]["FullName"].ToString() + @"</p>
                </td>
                <td class='tr0 td2'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td3'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td4'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td5'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>");
                        }
                    }
                    #endregion
                    #region dtInputEnquiry


                    if (ds.Tables.Count >= 2)
                    {
                        dtInputEnquiry = ds.Tables[1];
                        if (dtInputEnquiry.Rows.Count > 0)
                        {

                            sbHtml.Append(@"<tr>
                <td class='tr2 td7'>
                    <p class='p1 ft6'>
                        Input Enquiry:</p>
                </td>
                <td class='tr2 td8'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr2 td9'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr2 td10'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr2 td11'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr2 td12'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td colspan='2' class='tr1 td13'>
                    <p class='p7 ft7'>
                        Personal Information</p>
                </td>
                <td colspan='2' class='tr1 td14'>
                    <p class='p8 ft7'>
                        Identification</p>
                </td>
                <td class='tr1 td15'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr1 td16'>
                    <p class='p5 ft7'>
                        Contact Details</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                        Consumer's</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p9 ft8'>
                        " + dtInputEnquiry.Rows[0]["FullName"].ToString() + @"</p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        PAN:</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p5 ft4'>
                       " + dtInputEnquiry.Rows[0]["PANId"].ToString() + @"</p>
                </td>
                <td class='tr1 td4'>
                    <p class='p10 ft8'>
                        AddrLine1:</p>
                </td>
                <td class='tr1 td18'>
                    <p class='p10 ft8'>
                      " + dtInputEnquiry.Rows[0]["AddrLine1"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr3 td17'>
                    <p class='p1 ft9'>
                        FullName:</p>
                </td>
                <td class='tr3 td1'>
                    <p class='p5 ft10'></p>
                </td>
                <td class='tr3 td2'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td3'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td4'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td18'>
                    <p class='p10 ft9'>
                        </p>
                </td>
            </tr>
            <tr>
                <td class='tr4 td17'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td1'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td2'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td3'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td4'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td18'>
                    <p class='p10 ft12'>
                       </p>
                </td>
            </tr>
            <tr>
                <td class='tr3 td17'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td1'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td2'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td3'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td4'>
                    <p class='p5 ft10'>
                        &nbsp;</p>
                </td>
                <td class='tr3 td18'>
                    <p class='p10 ft9'>
                        </p>
                </td>
            </tr>
            <tr>
                <td class='tr4 td17'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td1'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td2'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td3'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td4'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td18'>
                    <p class='p10 ft12'>
                        </p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                         H:
                </td>
                <td class='tr1 td1'>
                    <p class='p9 ft8'>
                      " + dtInputEnquiry.Rows[0]["H"].ToString() + @"</p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        Voter ID:" + dtInputEnquiry.Rows[0]["VoterID"].ToString() + @"</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p5 ft4'>
                         </p>
                </td>
                <td class='tr1 td4'>
                    <p class='p10 ft8'>
                        State: " + dtInputEnquiry.Rows[0]["State"].ToString() + @"</p>
                </td>
                <td class='tr1 td18'>
                    <p class='p10 ft8'>
                      </p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td17'>
                    <p class='p5 ft4'>
                </td>
                <td class='tr5 td1'>
                    <p class='p5 ft4'>
                </td>
                <td class='tr5 td2'>
                    <p class='p5 ft8'>
                        Passport ID:" + dtInputEnquiry.Rows[0]["PassportID"].ToString() + @"</p>
                </td>
                <td class='tr5 td3'>
                    <p class='p5 ft4'>
                         </p>
                </td>
                <td class='tr5 td4'>
                    <p class='p10 ft8'>
                        Postal:
                        " + dtInputEnquiry.Rows[0]["Postal"].ToString() + @"</p>
</p>
                </td>
                <td class='tr5 td18'>
                    <p class='p10 ft8'>
                        " + dtInputEnquiry.Rows[0]["PassportID"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td17'>
                    <p class='p1 ft8'>
                        DOB:</p>
                </td>
                <td class='tr5 td1'>
                    <p class='p9 ft8'>
                        <nobr>" + dtInputEnquiry.Rows[0]["DOB"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr5 td2'>
                    <p class='p5 ft8'>
                        UID:" + dtInputEnquiry.Rows[0]["NationalIdCard"].ToString() + @"</p>
                </td>
                <td class='tr5 td3'>
                    <p class='p5 ft4' style='margin-left:0px;'>
                        </p>
                </td>
                <td colspan='2' class='tr5 td19'>
                    <p class='p10 ft8'>
                        Ration Card:" + dtInputEnquiry.Rows[0]["RationCard"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                        Gender:</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p9 ft8'>
                         </p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        Driver's " + dtInputEnquiry.Rows[0]["DriverLicence"].ToString() + @"</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p5 ft4'>
                        </p>
                </td>
                <td class='tr1 td4'>
                    <p class='p10 ft8'>
                        Home:  " + dtInputEnquiry.Rows[0]["HomePhone"].ToString() + @"</p>
                </td>
                <td class='tr1 td18'>
                    <p class='p5 ft4'>
                      </p>
                </td>
            </tr>
            <tr>
                <td class='tr4 td17'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td1'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td2'>
                    <p class='p5 ft12'>
                        Licence:</p>
                </td>
                <td class='tr4 td3'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td4'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td18'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                        Inquiry/Request</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p9 ft8'>
                        " + dtInputEnquiry.Rows[0]["InquiryPurpose"].ToString() + @"</p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        AdditionalId1:</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p5 ft4'>
                       " + dtInputEnquiry.Rows[0]["AdditionalId1"].ToString() + @"</p>
                </td>
                <td class='tr1 td4'>
                    <p class='p10 ft8'>
                        Mobile:</p>
                </td>
                <td class='tr1 td18'>
                    <p class='p10 ft8'>
                        " + dtInputEnquiry.Rows[0]["MobilePhone"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr4 td17'>
                    <p class='p1 ft12'>
                        Purpose:</p>
                </td>
                <td class='tr4 td1'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td2'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td3'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td4'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
                <td class='tr4 td18'>
                    <p class='p5 ft11'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td20'>
                    <p class='p1 ft8'>
                        Transaction Amount:</p>
                </td>
                <td class='tr5 td8'>
                    <p class='p9 ft8'>
                        " + dtInputEnquiry.Rows[0]["TransactionAmount"].ToString() + @"</p>
                </td>
                <td class='tr5 td9'>
                    <p class='p5 ft8'>
                        AdditionalId2:</p>
                </td>
                <td class='tr5 td10'>
                    <p class='p5 ft4' style='margin-left:0px;'>
                        " + dtInputEnquiry.Rows[0]["AdditionalId2"].ToString() + @"</p>
                </td>
                <td class='tr5 td11'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td21'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td7'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td8'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td9'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td10'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td11'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td12'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>");

                        }
                    }


                    if (ds.Tables.Count >= 3)
                    {
                        dtPersonalInformation = ds.Tables[2];
                        if (dtPersonalInformation.Rows.Count > 0)
                        {
                            sbHtml.Append(@"<tr>
                <td colspan='2' class='tr1 td13'>
                    <p class='p7 ft7'>
                        Personal Information</p>
                </td>
                <td colspan='2' class='tr1 td14'>
                    <p class='p8 ft7'>
                        Identification</p>
                </td>
                <td class='tr1 td15'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr1 td16'>
                    <p class='p1 ft7'>
                        Family Details</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                        Previous Name:</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p5 ft4'>
                       " + dtPersonalInformation.Rows[0]["PreviousName"].ToString() + @"</p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        Voter ID:</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p11 ft8'>
                        " + dtPersonalInformation.Rows[0]["VoterId"].ToString() + @"</p>
                </td>
                <td colspan='2' class='tr1 td19'>
                    <p class='p10 ft8' style='margin-left: 45px;'>
                        No. of Dependents: " + dtPersonalInformation.Rows[0]["NoOfDependents"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td17'>
                    <p class='p1 ft8'>
                        Alias Name:</p>
                </td>
                <td class='tr5 td1'>
                    <p class='p5 ft4'>
                         " + dtPersonalInformation.Rows[0]["AliasName"].ToString() + @"</p>
                </td>
                <td class='tr5 td2'>
                    <p class='p5 ft8'>
                        Ration Card:</p>
                </td>
                <td class='tr5 td3'>
                    <p class='p11 ft8'>
                       " + dtPersonalInformation.Rows[0]["RationCard"].ToString() + @"</p>
                </td>
                <td class='tr5 td4'>
                    <p class='p10 ft8'>
                        Husband:</p>
                </td>
                <td class='tr5 td18'>
                    <p class='p10 ft8'>
                         " + dtPersonalInformation.Rows[0]["Husband"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td17'>
                    <p class='p1 ft8'>
                        DOB:</p>
                </td>
                <td class='tr1 td1'>
                    <p class='p9 ft8'>
                        <nobr>" + dtPersonalInformation.Rows[0]["DateOfBirth"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr1 td2'>
                    <p class='p5 ft8'>
                        UID:</p>
                </td>
                <td class='tr1 td3'>
                    <p class='p5 ft4'>
                        " + dtPersonalInformation.Rows[0]["NationalIDCard"].ToString() + @"</p>
                </td>
                <td class='tr1 td4'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr1 td18'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td17'>
                    <p class='p1 ft8'>
                        Age:</p>
                </td>
                <td class='tr5 td1'>
                    <p class='p9 ft8'>
                       " + dtPersonalInformation.Rows[0]["Age"].ToString() + @"</p>
                </td>
                <td class='tr5 td2'>
                    <p class='p5 ft8'>
                        PAN:</p>
                </td>
                <td class='tr5 td3'>
                    <p class='p5 ft4'>
                         " + dtPersonalInformation.Rows[0]["PANId"].ToString() + @"</p>
                </td>
                <td class='tr5 td4'>
                    <p class='p10 ft8'>
                        Other ID:</p>
                </td>
                <td class='tr5 td18'>
                    <p class='p10 ft8'>
                       " + dtPersonalInformation.Rows[0]["IDOther"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td17'>
                    <p class='p1 ft8'>
                        Gender:</p>
                </td>
                <td class='tr5 td1'>
                    <p class='p9 ft8'>
                       " + dtPersonalInformation.Rows[0]["Gender"].ToString() + @"</p>
                </td>
                <td class='tr5 td2'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td3'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td4'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td18'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td20'>
                    <p class='p1 ft8'>
                        Marital Status:</p>
                </td>
                <td class='tr5 td8'>
                    <p class='p9 ft8'>
                        " + dtPersonalInformation.Rows[0]["MaritalStatus"].ToString() + @"</p>
                </td>
                <td class='tr5 td9'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td10'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td11'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td21'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
        </table>");
                        }
                    }

                    #endregion
                    #region dtConsumerAddress
                    if (ds.Tables.Count >= 4)
                    {
                        dtConsumerAddress = ds.Tables[3];
                        sbHtml.Append(@"<p class='p12 ft13'>
            Consumer Address:</p>
        <table cellpadding='0' cellspacing='0' class='t1'>
            <tr>
                <td class='tr6 td22'>
                    <p class='p1 ft7' style='padding-left: 50%;'>
                        Type</p>
                </td>
                <td class='tr6 td23'>
                    <p class='p5 ft7'>
                        Address</p>
                </td>
                <td class='tr6 td24'>
                    <p class='p5 ft7'>
                        State</p>
                </td>
                <td class='tr6 td25'>
                    <p class='p13 ft7'>
                        Postal</p>
                </td>
                <td class='tr6 td26'>
                    <p class='p14 ft7'>
                        Date Reported</p>
                </td>
            </tr>");
                        foreach (DataRow row in dtConsumerAddress.Rows)
                        {
                            sbHtml.Append(@"
            <tr>
                <td class='tr1 td27'>
                    <p class='p1 ft8'>
                       " + row["Type"].ToString() + @"</p>
                </td>
                <td class='tr1 td28'>
                    <p class='p5 ft8'>
                        " + row["Address"].ToString() + @"</p>
                </td>
                <td class='tr1 td29'>
                    <p class='p5 ft8'>
                      " + row["State"].ToString() + @"</p>
                </td>
                <td class='tr1 td30'>
                    <p class='p13 ft8'>
                        " + row["Postal"].ToString() + @"</p>
                </td>
                <td class='tr1 td31'>
                    <p class='p14 ft8'>
                        <nobr> " + row["DateReported"].ToString() + @"</nobr>
                    </p>
                </td>
            </tr>");
                        }
                        sbHtml.Append("</table>");
                    }
                    #endregion
                    #region dtSummary
                    if (ds.Tables.Count >= 5)
                    {
                        dtSummary = ds.Tables[4];
                        if (dtSummary.Rows.Count > 0)
                        {
                            sbHtml.Append(@"  <p class='p15 ft13'>
            Summary:</p>
        <table cellpadding='0' cellspacing='0' class='t1'>
            <tr>
                <td class='tr6 td32'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr6 td33'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr6 td34'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td colspan='2' class='tr6 td35'>
                    <p class='p16 ft7'>
                        Credit Report Summary</p>
                </td>
                <td class='tr6 td36'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr6 td37'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr6 td38'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td39'>
                    <p class='p1 ft8'>
                        Number</p>
                </td>
                <td class='tr1 td40'>
                    <p class='p5 ft8'>
                        of Open Accounts:</p>
                </td>
                <td class='tr1 td41'>
                    <p class='p17 ft8'>
                        " + dtSummary.Rows[0]["NoOfActiveAccounts"].ToString() + @"</p>
                </td>
                <td class='tr1 td42'>
                    <p class='p18 ft8'>
                        Total Balance Amount:</p>
                </td>
                <td class='tr1 td43'>
                    <p class='p19 ft8'>
                           " + dtSummary.Rows[0]["TotalBalanceAmount"].ToString() + @"</p>
                </td>
                <td class='tr1 td44'>
                    <p class='p5 ft8'>
                        Total</p>
                </td>
                <td class='tr1 td45'>
                    <p class='p5 ft8'>
                        Monthly Instmt Amt:</p>
                </td>
                <td class='tr1 td46'>
                    <p class='p20 ft8'>
                         " + dtSummary.Rows[0]["TotalMonthlyPaymentAmount"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr5 td47'>
                    <p class='p1 ft8'>
                        Number</p>
                </td>
                <td class='tr5 td48'>
                    <p class='p5 ft8'>
                        of PastDue Accounts:</p>
                </td>
                <td class='tr5 td49'>
                    <p class='p17 ft8'>
                        " + dtSummary.Rows[0]["NoOfPastDueAccounts"].ToString() + @"</p>
                </td>
                <td class='tr5 td50'>
                    <p class='p18 ft8'>
 Total Past Due Amount:
                </td>
                <td class='tr5 td51'>
                    <p class='p19 ft8'>
                        " + dtSummary.Rows[0]["TotalPastDue"].ToString() + @"</p>
                </td>
                <td class='tr5 td52'>
                    <p class='p5 ft8'>
                        Total</p>
                </td>
                <td class='tr5 td53'>
                    <p class='p5 ft8'>
                        Written Off Amount:</p>
                </td>
                <td class='tr5 td54'>
                    <p class='p20 ft8'>
                       " + dtSummary.Rows[0]["TotalWrittenOffAmount"].ToString() + @"</p>
                </td>
            </tr>
        </table>");
                        }
                    }
                    #endregion
                    #region dtMfiDetails
                    if (ds.Tables.Count >= 5)
                    {
                        dtMfiDetails = ds.Tables[5];
                        sbHtml.Append(@"<p class='p21 ft14'>
            MFI Details:</p>
        <table cellpadding='0' cellspacing='0' class='t1'>
            <tr>
                <td class='tr3 td55'>
                    <p class='p22 ft15'>
                        Name of MFI Institution</p>
                </td>
                <td class='tr3 td56'>
                    <p class='p1 ft15'>
                        Latest Reported</p>
                </td>
                <td class='tr3 td57'>
                    <p class='p23 ft15'>
                        Total</p>
                </td>
                <td class='tr3 td58'>
                    <p class='p24 ft15'>
                        Open</p>
                </td>
                <td class='tr3 td58'>
                    <p class='p25 ft15'>
                        Closed</p>
                </td>
                <td class='tr3 td57'>
                    <p class='p20 ft15'>
                        Past Due</p>
                </td>
                <td class='tr3 td57'>
                    <p class='p6 ft15'>
                        Sum of</p>
                </td>
                <td class='tr3 td59'>
                    <p class='p6 ft15'>
                        Sum of</p>
                </td>
                <td class='tr3 td57'>
                    <p class='p6 ft15'>
                        Sum of</p>
                </td>
                <td class='tr3 td57'>
                    <p class='p4 ft15'>
                        Sum of Over</p>
                </td>
                <td class='tr3 td59'>
                    <p class='p6 ft15'>
                        Sum of</p>
                </td>
            </tr>  <tr>
                <td class='tr7 td60'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td61'>
                    <p class='p26 ft17'>
                        Date</p>
                </td>
                <td class='tr7 td62'>
                    <p class='p26 ft17'>
                        Accounts</p>
                </td>
                <td class='tr7 td63'>
                    <p class='p26 ft17'>
                        Accounts</p>
                </td>
                <td class='tr7 td63'>
                    <p class='p26 ft17'>
                        Accounts</p>
                </td>
                <td class='tr7 td62'>
                    <p class='p26 ft17'>
                        Accounts</p>
                </td>
                <td class='tr7 td62'>
                    <p class='p26 ft17'>
                        Current</p>
                </td>
                <td class='tr7 td64'>
                    <p class='p27 ft17'>
                        Disbursed</p>
                </td>
                <td class='tr7 td62'>
                    <p class='p26 ft17'>
                        Installment</p>
                </td>
                <td class='tr7 td62'>
                    <p class='p26 ft17'>
                        Due Amounts</p>
                </td>
                <td class='tr7 td64'>
                    <p class='p27 ft17'>
                        <nobr>Written-off</nobr>
                    </p>
                </td>
            </tr>");
                        sbHtml.Append(@"<tr>
                <td class='tr7 td65'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td66'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td67'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td68'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td68'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td67'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td67'>
                    <p class='p26 ft17'>
                        Balance</p>
                </td>
                <td class='tr7 td69'>
                    <p class='p27 ft17'>
                        Amount</p>
                </td>
                <td class='tr7 td67'>
                    <p class='p26 ft17'>
                        Amounts</p>
                </td>
                <td class='tr7 td67'>
                    <p class='p5 ft16'>
                        &nbsp;</p>
                </td>
                <td class='tr7 td69'>
                    <p class='p27 ft17'>
                        Amounts</p>
                </td>
            </tr>");
                        foreach (DataRow row in dtMfiDetails.Rows)
                        {

                            sbHtml.Append(@"<tr>
                <td class='tr6 td70'>
                    <p class='p28 ft8'>
                       " + row["Institution"].ToString() + @"</p>
                </td>
                <td class='tr6 td71'>
                    <p class='p26 ft8'>
                        <nobr> " + row["LatestDateReported"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr6 td72'>
                    <p class='p26 ft8'>
                        " + row["TotalAccounts"].ToString() + @"</p>
                </td>
                <td class='tr6 td73'>
                    <p class='p26 ft8'>
                        " + row["OpenAccounts"].ToString() + @"</p>
                </td>
                <td class='tr6 td73'>
                    <p class='p27 ft8'>
                        " + row["ClosedAccounts"].ToString() + @"</p>
                </td>
                <td class='tr6 td72'>
                    <p class='p27 ft8'>
                        " + row["PastDueAccounts"].ToString() + @"</p>
                </td>
                <td class='tr6 td72'>
                    <p class='p26 ft8'>
                        " + row["CurrentBalance"].ToString() + @"</p>
                </td>
                <td class='tr6 td74'>
                    <p class='p26 ft8'>
                       " + row["DisbursedAmount"].ToString() + @"</p>
                </td>
                <td class='tr6 td72'>
                    <p class='p27 ft8'>
                        " + row["InstallmentAmount"].ToString() + @"</p>
                </td>
                <td class='tr6 td72'>
                    <p class='p26 ft8'>
                       " + row["PastDueAmount"].ToString() + @"</p>
                </td>
                <td class='tr6 td74'>
                    <p class='p26 ft8'>
                       " + row["WriteOffAmount"].ToString() + @"</p>
                </td>
            </tr>");
                        }
                    }
                    sbHtml.Append("</table>");
                    #endregion
                    #region dtEnquiries

                    if (ds.Tables.Count >= 6)
                    {
                        dtEnquiries = ds.Tables[6];
                        sbHtml.Append(@"<p class='p21 ft14'>
            Enquiries:</p>
        <table cellpadding='0' cellspacing='0' class='t2'>
            <tr>
                <td class='tr1 td80'>
                    <p class='p29 ft7'>
                        Institution</p>
                </td>
                <td class='tr1 td81'>
                    <p class='p10 ft7'>
                        Date</p>
                </td>
                <td class='tr1 td82'>
                    <p class='p30 ft7'>
                        Time</p>
                </td>
                <td class='tr1 td82'>
                    <p class='p31 ft7'>
                        Purpose</p>
                </td>
                <td class='tr1 td81'>
                    <p class='p32 ft7'>
                        Amount</p>
                </td>
            </tr>");
                        foreach (DataRow row in dtEnquiries.Rows)
                        {
                            sbHtml.Append(@"<tr>
                <td class='tr1 td83'>
                    <p class='p33 ft8'>
                        " + row["Institution"].ToString() + @"</p>
                </td>
                <td class='tr1 td0'>
                    <p class='p34 ft8'>
                        <nobr>  " + row["Date"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr1 td84'>
                    <p class='p35 ft8'>
                      " + row["Time"].ToString() + @"</p>
                </td>
                <td class='tr1 td84'>
                    <p class='p27 ft8'>
                         " + row["RequestPurpose"].ToString() + @"</p>
                </td>
                <td class='tr1 td85'>
                    <p class='p5 ft4'>
                         " + row["Amount"].ToString() + @"</p>
                </td>
            </tr>");



                        }

                        //sbHtml.Append("</table>");
                    }
                    #endregion
                    #region Compliance Remarks
                    sbHtml.Append(@"<tr>
                <td class='tr8 td90'>
                    <p class='p1 ft6'>
                        Compliance Remarks:</p>
                </td>
                <td class='tr8 td0'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr8 td84'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr8 td84'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr8 td0'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
<tr>
                <td colspan='2' class='tr9 td91'>
                    <p class='p5 ft18'>
                        Copyright© 2016 Equifax Credit Information Services Pvt Ltd</p>
                </td>
                <td class='tr9 td84'>
                    <p class='p36 ft18'>
                        - 1 -</p>
                </td>
                <td class='tr9 td84'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr9 td0'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
        </table>
</div>");

                    #endregion
                    #region MICROFINANCE CONSUMER CREDIT SUMMARY
                    sbHtml.Append(@"<div id='page_2'>
        <div id='p2dimg1'>
            <img src='data:image/jpg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCACGApsDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDuvFut6h4b1DTbC1ln1G7vn2LELjyipyoXrkckn06flUuvE2q6Jaef4ktr/T2kYLbwwXKXLy8HccBgAB8vfneKt+L/AAdquu+MrbVEtbO4sLaFE8iW4KGYqS2D8pwMtj6D34r6h4S1WK50fUbHQLCNNPuXmbTbW8wZHOza4dlA/g5GP4RjOeG61S//AAF/kdcMHg3CKe/X3n56b6dOgp8ZWkOo3en3t7q1ldW0DztHcIoLBV37RiQ8leRnA7ZzxUlz4usrXw3Ya5JqWp+RfOyxRBB5nykgkjzMYyPXuKwLxotY8XaroHibTotN1DV1jNneJhjAQo2LvJAcExgfLgk7l7/Lrr4R17W7nw9ZapYWlnp2hgRu7TCf7YqhASFAGA2wdem7v0o9vU/pL/IbwGEjZyTXV+89rPbXXXTyLL+KreOLW5De6tt0aVYbg+WPmYyGMbP3nPIJ5xxSQ+KVewnv5z4gtbOK388XFxaFUlBKhVQhyCxLDHbHOcCse4+HHiS50m9SSSwF/f35urqVbhwpCg7ABt5+aSQnPov4bepfDy5n1e2XSbyPTNNtVWRfML3TSSjIGY5G2gKpOD15PB4we2qf0kTLBYFdf/Jn0t+bvb/gFUeNbVtEOrpca09otwbeRlRMxttDDcPN6HPB9RzjjOnp2tw6rrNtpllqeoTTTWi3bOq5SFCMgOd/DcrwAfvCuc0/4ceJdPt9W0iO5086dqckayXLD50jRidyxgbVY56Z4wMEdR0vw/8ACuqeEZdRsrgWstjLJ5kU8Zw5I45XHQjB68Y75JoVap/SQVcFgoxk4O76e89tPxXbqbltpt5c26TDVLyMMPuSIysPqC1S/wBjXv8A0F5/1/8Aiq2aKv2sv6SPP+r0/wCm/wDMxv7Gvf8AoLz/AK//ABVH9jXv/QXn/X/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v/AMVR/Y17/wBBef8AX/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v8A8VR/Y17/ANBef9f/AIqtmij2sv6SD6vT/pv/ADMb+xr3/oLz/r/8VR/Y17/0F5/1/wDiq2aKPay/pIPq9P8Apv8AzMb+xr3/AKC8/wCv/wAVR/Y17/0F5/1/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/wDFUf2Ne/8AQXn/AF/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/APFUf2Ne/wDQXn/X/wCKrZoo9rL+kg+r0/6b/wAzG/sa9/6C8/6//FUf2Ne/9Bef9f8A4qtmij2sv6SD6vT/AKb/AMzG/sa9/wCgvP8Ar/8AFUf2Ne/9Bef9f/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/8AxVH9jXv/AEF5/wBf/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/wDxVH9jXv8A0F5/1/8Aiq2aKPay/pIPq9P+m/8AMxv7Gvf+gvP+v/xVH9jXv/QXn/X/AOKrZoo9rL+kg+r0/wCm/wDMxv7Gvf8AoLz/AK//ABVH9jXv/QXn/X/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v/AMVR/Y17/wBBef8AX/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v8A8VR/Y17/ANBef9f/AIqtmij2sv6SD6vT/pv/ADMb+xr3/oLz/r/8VR/Y17/0F5/1/wDiq2aKPay/pIPq9P8Apv8AzMb+xr3/AKC8/wCv/wAVR/Y17/0F5/1/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/wDFUf2Ne/8AQXn/AF/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/APFUf2Ne/wDQXn/X/wCKrZoo9rL+kg+r0/6b/wAzG/sa9/6C8/6//FUf2Ne/9Bef9f8A4qtmij2sv6SD6vT/AKb/AMzG/sa9/wCgvP8Ar/8AFUf2Ne/9Bef9f/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/8AxVH9jXv/AEF5/wBf/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/wDxVH9jXv8A0F5/1/8Aiq2aKPay/pIPq9P+m/8AMxv7Gvf+gvP+v/xVH9jXv/QXn/X/AOKrZoo9rL+kg+r0/wCm/wDMxv7Gvf8AoLz/AK//ABVH9jXv/QXn/X/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v/AMVR/Y17/wBBef8AX/4qtmij2sv6SD6vT/pv/Mxv7Gvf+gvP+v8A8VR/Y17/ANBef9f/AIqtmij2sv6SD6vT/pv/ADMb+xr3/oLz/r/8VR/Y17/0F5/1/wDiq2aKPay/pIPq9P8Apv8AzMb+xr3/AKC8/wCv/wAVR/Y17/0F5/1/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/wDFUf2Ne/8AQXn/AF/+KrZoo9rL+kg+r0/6b/zMb+xr3/oLz/r/APFUf2Ne/wDQXn/X/wCKrZoo9rL+kg+r0/6b/wAzG/sa9/6C8/6//FUf2Ne/9Bef9f8A4qtmij2sv6SD6vT/AKb/AMzG/sa9/wCgvP8Ar/8AFUf2Ne/9Bef9f/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/8AxVH9jXv/AEF5/wBf/iq2aKPay/pIPq9P+m/8zG/sa9/6C8/6/wDxVH9jXv8A0F5/1/8Aiq2aKPay/pIPq9P+m/8AMxv7Gvf+gvP+v/xVH9jXv/QXn/X/AOKrZoo9rL+kg+r0/wCm/wDMxv7Gvf8AoLz/AK//ABVH9jXv/QXn/X/4qtmij2sv6SD6vT/pv/MKKxv+Elsv+eU//fI/xo/4SWy/55T/APfI/wAaPY1OwfWKX8xcv9J07VDGb/T7W7MeQhnhV9ucZxkcZwPyFXaxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jPsH1mltzGzRWN/wAJLZf88p/++R/jR/wktl/zyn/75H+NHsanYPrFL+Y2aKxv+Elsv+eU/wD3yP8AGj/hJbL/AJ5T/wDfI/xo9jU7B9YpfzGzRWN/wktl/wA8p/8Avkf40f8ACS2X/PKf/vkf40exqdg+sUv5jZorG/4SWy/55T/98j/Gj/hJbL/nlP8A98j/ABo9jU7B9YpfzGzRWN/wktl/zyn/AO+R/jR/wktl/wA8p/8Avkf40exqdg+sUv5jZorG/wCElsv+eU//AHyP8aP+Elsv+eU//fI/xo9jU7B9YpfzGzRWN/wktl/zyn/75H+NH/CS2X/PKf8A75H+NHsanYPrFL+Y2aKxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jU7B9YpfzGzRWN/wAJLZf88p/++R/jR/wktl/zyn/75H+NHsanYPrFL+Y2aKxv+Elsv+eU/wD3yP8AGj/hJbL/AJ5T/wDfI/xo9jU7B9YpfzGzRWN/wktl/wA8p/8Avkf40f8ACS2X/PKf/vkf40exqdg+sUv5jZorG/4SWy/55T/98j/Gj/hJbL/nlP8A98j/ABo9jU7B9YpfzGzRWN/wktl/zyn/AO+R/jR/wktl/wA8p/8Avkf40exqdg+sUv5jZorG/wCElsv+eU//AHyP8aP+Elsv+eU//fI/xo9jU7B9YpfzGzRWN/wktl/zyn/75H+NH/CS2X/PKf8A75H+NHsanYPrFL+Y2aKxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jU7B9YpfzGzRWN/wAJLZf88p/++R/jR/wktl/zyn/75H+NHsanYPrFL+Y2aKxv+Elsv+eU/wD3yP8AGj/hJbL/AJ5T/wDfI/xo9jU7B9YpfzGzRWN/wktl/wA8p/8Avkf40f8ACS2X/PKf/vkf40exqdg+sUv5jZorG/4SWy/55T/98j/Gj/hJbL/nlP8A98j/ABo9jU7B9YpfzGzRWN/wktl/zyn/AO+R/jR/wktl/wA8p/8Avkf40exqdg+sUv5jZorG/wCElsv+eU//AHyP8aP+Elsv+eU//fI/xo9jU7B9YpfzGzRWN/wktl/zyn/75H+NH/CS2X/PKf8A75H+NHsanYPrFL+Y2aKxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jU7B9YpfzGzRWN/wAJLZf88p/++R/jR/wktl/zyn/75H+NHsanYPrFL+Y2aKxv+Elsv+eU/wD3yP8AGj/hJbL/AJ5T/wDfI/xo9jU7B9YpfzGzRWN/wktl/wA8p/8Avkf40f8ACS2X/PKf/vkf40exqdg+sUv5jZorG/4SWy/55T/98j/Gj/hJbL/nlP8A98j/ABo9jU7B9YpfzGzRWN/wktl/zyn/AO+R/jR/wktl/wA8p/8Avkf40exqdg+sUv5jZorG/wCElsv+eU//AHyP8aP+Elsv+eU//fI/xo9jU7B9YpfzGzRWN/wktl/zyn/75H+NH/CS2X/PKf8A75H+NHsanYPrFL+Y2aKxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jU7B9YpfzGzRWN/wAJLZf88p/++R/jR/wktl/zyn/75H+NHsanYPrFL+Y2aKxv+Elsv+eU/wD3yP8AGj/hJbL/AJ5T/wDfI/xo9jU7B9YpfzGzRWN/wktl/wA8p/8Avkf40f8ACS2X/PKf/vkf40exqdg+sUv5jZorG/4SWy/55T/98j/Gj/hJbL/nlP8A98j/ABo9jU7B9YpfzGzRWN/wktl/zyn/AO+R/jR/wktl/wA8p/8Avkf40exqdg+sUv5jZorG/wCElsv+eU//AHyP8aP+Elsv+eU//fI/xo9jU7B9YpfzGzRWN/wktl/zyn/75H+NH/CS2X/PKf8A75H+NHsanYPrFL+Y2aKxv+Elsv8AnlP/AN8j/Gj/AISWy/55T/8AfI/xo9jU7B9YpfzGzRRRWZsFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB//2Q=='
                id='p2img1'></div>
        <div class='dclr'>
        </div>
        <p class='p0 ft0'>
            MICROFINANCE CONSUMER CREDIT SUMMARY</p>");
                    if (dtConsumerBasicInformation.Rows.Count > 0)
                    {
                        sbHtml.Append(@"<table cellpadding='0' cellspacing='0' class='t3'>
            <tr>
                <td class='tr0 td40'>
                    <p class='p1 ft1'>
                        CLIENT ID:</p>
                </td>
                <td class='tr0 td92'>
                    <p class='p37 ft2'>
                        " + dtConsumerBasicInformation.Rows[0]["ClientID"].ToString() + @" </p>
                </td>
                <td class='tr0 td93'>
                    <p class='p5 ft3'>
                        BranchID:</p>
                </td>
                <td class='tr0 td94'>
                    <p class='p4 ft2'>
                         " + dtConsumerBasicInformation.Rows[0]["BranchId"].ToString() + @" </p>
                </td>
                <td class='tr0 td95'>
                    <p class='p38 ft1'>
                        DATE:</p>
                </td>
                <td class='tr0 td96'>
                    <p class='p1 ft2'>
                        <nobr>" + dtConsumerBasicInformation.Rows[0]["Date"].ToString() + @"</nobr>
                    </p>
                </td>
            </tr>
            <tr>
                <td class='tr1 td40'>
                    <p class='p1 ft1'>
                        REPORT ORDER NO:</p>
                </td>
                <td class='tr1 td92'>
                    <p class='p37 ft2'>
                        " + dtConsumerBasicInformation.Rows[0]["ReportOrderNO"].ToString() + @"</p>
                </td>
                <td class='tr1 td93'>
                    <p class='p5 ft3'>
                        KendraID:</p>
                </td>
                <td class='tr1 td94'>
                    <p class='p4 ft2'>
                       " + dtConsumerBasicInformation.Rows[0]["KedraId"].ToString() + @"</p>
                </td>
                <td class='tr1 td95'>
                    <p class='p38 ft1'>
                        TIME:</p>
                </td>
                <td class='tr1 td96'>
                    <p class='p1 ft2'>
                        " + dtConsumerBasicInformation.Rows[0]["Time"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr0 td40'>
                    <p class='p1 ft5'>
                        REFERENCE NUMBER:</p>
                </td>
                <td class='tr0 td92'>
                    <p class='p37 ft2'>
                        <nobr>" + dtConsumerBasicInformation.Rows[0]["CustRefField"].ToString() + @"</nobr>
                    </p>
                </td>
                <td class='tr0 td93'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td94'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td95'>
                    <p class='p38 ft5'>
                        ADDITIONAL SEARCH FIELD:</p>
                </td>
                <td class='tr0 td96'>
                    <p class='p1 ft2'>
                        " + dtConsumerBasicInformation.Rows[0]["AdditionalSearchField"].ToString() + @"</p>
                </td>
            </tr>
            <tr>
                <td class='tr0 td48'>
                    <p class='p1 ft2'>
                        Consumer Name:</p>
                </td>
                <td class='tr0 td97'>
                    <p class='p37 ft6'>
                        " + dtConsumerBasicInformation.Rows[0]["FullName"].ToString() + @"</p>
                </td>
                <td class='tr0 td98'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td99'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td100'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr0 td101'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>");
                    }
                    #endregion
                    #region Remarks
                    sbHtml.Append(@"<tr>
                <td class='tr5 td102'>
                    <p class='p1 ft7'>
                        Remarks</p>
                </td>
                <td class='tr5 td103'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td104'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td105'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td106'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
                <td class='tr5 td107'>
                    <p class='p5 ft4'>
                        &nbsp;</p>
                </td>
            </tr>
        </table>");
                    #endregion
                    #region Footer
                    sbHtml.Append(@"<p class='p41 ft21'>
            This report is to be used subject to and in compliance with the Membership agreement
            entered into between the Member/Specified User and Equifax Credit Information Services
            Private Limited ('ECIS'), in the case of Members/Specified Users of ECIS. In other
            cases, the use of this report is governed by the terms and conditions of ECIS, contained
            in the Application form submitted by the customer/user. The information contained
            in this report is derived from various Members/sources which are not controlled
            by ECIS. ECIS provides this report on a best effort basis and does not guarantee
            the timeliness, correctness or completeness of the information contained therein,
            except as explicitly stated in the Membership agreement/terms and conditions of
            ECIS, as the case may be.</p>
        <table cellpadding='0' cellspacing='0' class='t4'>
            <tr>
                <td class='tr4 td108'>
                    <p class='p5 ft18'>
                        Copyright© 2016 Equifax Credit Information Services Pvt Ltd</p>
                </td>
                <td class='tr4 td109'>
                    <p class='p42 ft18'>
                        - 2 -</p>
                </td>
            </tr>
        </table>
    </div>");
                    #endregion
                    #region EndHtmlTags
                    sbHtml.Append(@"</head>
            </html>");
                    #endregion

                    var writer = new StreamWriter(stream);
                    writer.Write(sbHtml.ToString());
                    writer.Flush();
                    stream.Position = 0;
                }
            }
            catch
            {
                StringBuilder sbHtml = new StringBuilder();
                sbHtml.Append("No Record Found !!");
                var writer = new StreamWriter(stream);
                writer.Write(sbHtml.ToString());
                writer.Flush();
                stream.Position = 0;
            }
            return stream;
        }
        #endregion

        #region GetCenterList
        public List<GetCenterListData> GetCenterList(PostCenterList CenterListData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<GetCenterListData> row = new List<GetCenterListData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetCenterList";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", CenterListData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", CenterListData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pEoId", CenterListData.pEoId);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetCenterListData(rs["MarketId"].ToString(), rs["MarketName"].ToString(), rs["Groupid"].ToString(), rs["GroupName"].ToString(), rs["MarketType"].ToString(), rs["GroupType"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetCenterListData("No data available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetCenterListData("No data available", ex.ToString(), "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region SaveCenter
        public string SaveCenter(PostCenterData postCenterData)
        {
            SqlCommand oCmd = new SqlCommand();
            SqlCommand oCmd1 = new SqlCommand();
            string msg = "", vMsg = "";
            Int32 vErr = 0;
            string pNewId;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDupColltime_Name";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoId", postCenterData.pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pMarketId", "-1");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", postCenterData.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pCollTime", postCenterData.pColltime);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", postCenterData.pCollDay);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", postCenterData.pCollDayNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", postCenterData.pCollType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pMode", "Save");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pMsg", vMsg);
                DBConnect.Execute(oCmd);
                vMsg = Convert.ToString(oCmd.Parameters["@pMsg"].Value);

                if (vMsg != "")
                {
                    msg = vMsg;
                    return msg;
                }
                else
                {
                    oCmd1.CommandType = CommandType.StoredProcedure;
                    oCmd1.CommandText = "SaveCenter";
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 10, "@pNewId", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pRO", postCenterData.pEoID);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pFrmDt", postCenterData.pDate);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMktName", postCenterData.pCenterName.ToUpper());
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAdd1", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAdd2", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAdd3", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pContNo", postCenterData.pLeaderContact);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 5, "@pVilg", Convert.ToInt32(postCenterData.pVillageId));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pCrLed", postCenterData.pLeaderName);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pMetPlc", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pDistFrmBr", -1);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pCntFrmBy", postCenterData.pEoID);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pRoIntDt", "1900-01-01");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pDrpOut", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDrpDt", "1900-01-01");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pRem", "");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postCenterData.pBranch);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(postCenterData.pUserId));
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Save");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDay", postCenterData.pCollDay);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollDayNo", postCenterData.pCollDayNo);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCollType", postCenterData.pCollType);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.Char, 1, "@pCollSche", postCenterData.pCollSche);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pColltime", postCenterData.pColltime);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLatitude", postCenterData.pLatitude);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLongitude", postCenterData.pLongitude);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobTag", "M");
                    DBConnect.AddParameter(oCmd1.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pGeoAddress", postCenterData.pGeoAddress);
                    DBConnect.Execute(oCmd1);
                    vErr = Convert.ToInt32(oCmd1.Parameters["@pErr"].Value);
                    pNewId = Convert.ToString(oCmd1.Parameters["@pNewId"].Value);

                    if (vErr == 0)
                    {
                        return "Record saved successfully : " + postCenterData.pCenterName + "#" + pNewId;
                    }
                    else if (vErr == 3)
                    {
                        return "Bi weekly collection routine not allowed for this branch";
                    }
                    else
                    {
                        return "Data Not Saved";
                    }
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

        #region GetLoanUtilityCheck
        public List<LoanUtilizationData> GetLoanUtilityCheck(PostLoanUtilizationData postLoanUtilizationData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<LoanUtilizationData> row = new List<LoanUtilizationData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanUtilityCheck";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postLoanUtilizationData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", postLoanUtilizationData.pGroupId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoggedDate", postLoanUtilizationData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new LoanUtilizationData(rs["LoanId"].ToString(), rs["LoanNo"].ToString(), rs["MemberName"].ToString(), rs["dose"].ToString(), rs["LoanDate"].ToString(),
                            rs["LoanAmt"].ToString(), rs["Purpose"].ToString(), rs["SubPurpose"].ToString(), rs["GroupId"].ToString(), rs["LoanUTLType"].ToString(),
                            rs["LoanUTLRemarks"].ToString(), rs["LoanUTLVia"].ToString(), rs["LoanUTLAmt"].ToString(), rs["VerifiedBy"].ToString(), rs["VerificationDate"].ToString(), rs["isSamePurpose"].ToString()));
                    }
                }
                else
                {
                    row.Add(new LoanUtilizationData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new LoanUtilizationData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region UpdateLoanUtilizationCheck
        public string UpdateLoanUtilizationCheck(PostUpdateLoanUtilCheck postUpdateLoanUtilCheck)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLoanUtilizationCheck";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", postUpdateLoanUtilCheck.pLoanId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUTLType", postUpdateLoanUtilCheck.pLoanUTLType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pLoanUTLBy", Convert.ToInt32(postUpdateLoanUtilCheck.pLoanUTLBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLoanUTLRemarks", postUpdateLoanUtilCheck.pLoanUTLRemarks);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUTLVia", postUpdateLoanUtilCheck.pLoanUTLVia);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pLoanUTLAmt", Convert.ToInt32(postUpdateLoanUtilCheck.pLoanUTLAmt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoanUTLDt", postUpdateLoanUtilCheck.pLoanUTLDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 8, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Bit, 1, "@pIsSamePurpose", Convert.ToBoolean(postUpdateLoanUtilCheck.IsSamePurpose));
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                {
                    return "Loan Utilization update successfully.";
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

        #region ChangePassword
        public string Mob_ChangePassword(PostMob_ChangePassword postMob_ChangePassword)
        {
            postMob_ChangePassword.pEncYN = postMob_ChangePassword.pEncYN == null ? "N" : postMob_ChangePassword.pEncYN;
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
            //-----------08.07.2025---------------
            string vX_Key = RsaDecrypt(postMob_ChangePassword.pKey);
            byte[] xKey = Convert.FromBase64String(vX_Key);
            postMob_ChangePassword.pPassword = Decrypt(postMob_ChangePassword.pPassword, xKey);
            postMob_ChangePassword.pOldPassword = Decrypt(postMob_ChangePassword.pOldPassword, xKey);
            //-----------------------------------

            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_ChangePass";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUserName", postMob_ChangePassword.pUserName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postMob_ChangePassword.pPassword.Length + 1, "@pPassword", postMob_ChangePassword.pPassword);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postMob_ChangePassword.pOldPassword.Length + 1, "@pOldPassword", postMob_ChangePassword.pOldPassword);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region ForgotPassword
        public ForgotOTPRes SendForgotOTP(ForgotOTPData objOTPData)
        {
            DataTable dt = new DataTable();
            string result = "", vOTPId = "0", vOTP = "", vMobileNo = "", vUserName = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            ForgotOTPRes oFO = null;
            try
            {
                vUserName = objOTPData.pUserName;
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pUserName", pUserName);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        public ForgotOTPRes ForgotPassword(Post_ForgotPassword Post_ForgotPassword)
        {
            //-----------08.07.2025---------------
            string vX_Key = RsaDecrypt(Post_ForgotPassword.pKey);
            byte[] xKey = Convert.FromBase64String(vX_Key);
            Post_ForgotPassword.pPassword = Decrypt(Post_ForgotPassword.pPassword, xKey);
            //-----------------------------------
            ForgotOTPRes oFO = null;
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ForgotPassword";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUserName", Post_ForgotPassword.pUserName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, Post_ForgotPassword.pPassword.Length + 1, "@pPassword", Post_ForgotPassword.pPassword);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOTPId", Convert.ToInt32(Post_ForgotPassword.pOTPId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pOTP", Post_ForgotPassword.pOTP);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobYN", "M");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return oFO;
        }

        public ForgotOTPRes ValidateOTP(ValidateOTPReqData objOTPData)
        {
            DataTable dt = new DataTable();
            ForgotOTPRes oFO = null;
            int vErr = 0;
            string pErrDesc = "";
            try
            {
                vErr = ValidateOTP(objOTPData.pOTP, Convert.ToInt32(objOTPData.pOTPId), ref pErrDesc);
                if (vErr == 0)
                {
                    oFO = new ForgotOTPRes("XX", "200", pErrDesc, objOTPData.pOTPId);
                }
                else if (vErr == 1)
                {
                    oFO = new ForgotOTPRes("XX", "401", pErrDesc, objOTPData.pOTPId);
                }
                else
                {
                    oFO = new ForgotOTPRes("XX", "400", pErrDesc, objOTPData.pOTPId);
                }

            }
            catch (Exception ex)
            {
                oFO = new ForgotOTPRes("XX", "400", "Failed:Invalid OTP.", objOTPData.pOTPId);
            }
            return oFO;
        }

        public Int32 ValidateOTP(string pOTP, int pOTPId, ref string pErrDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            int pErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ValidateOTP";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOTPId", pOTPId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pOTP", pOTP);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBConnect.Execute(oCmd);
                pErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
            }
            catch (Exception ex)
            {
                return 5;
            }
            finally
            {
                oCmd.Dispose();
            }
            return pErr;
        }

        #endregion

        #region GetLUCAppData
        public List<LucCenterGroupEoList> GetLUCAppData(PostLoanApp postLoanApp)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<LucCenterGroupEoList> row = new List<LucCenterGroupEoList>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "LucCenterGroupEoList";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postLoanApp.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postLoanApp.pBranch);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new LucCenterGroupEoList(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["Groupid"].ToString(), rs["GroupName"].ToString(),
                             rs["MarketID"].ToString(), rs["Market"].ToString()));
                    }
                }
                else
                {
                    row.Add(new LucCenterGroupEoList("No data available", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new LucCenterGroupEoList("No data available", ex.ToString(), "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region ICICI Transaction
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vPostBankTransactionStatus"></param>
        /// <returns></returns>
        public ICICBankTransactionStatusResponse ICICBankTransactionStatus(PostBankTransactionStatus vPostBankTransactionStatus)
        {
            string pIPAddress = string.Empty;
            string strEncryptedData = string.Empty;
            string strDeryptedData = string.Empty;
            string strAPIResponse = string.Empty;
            ICICBankTransactionStatusResponse objResponse = new ICICBankTransactionStatusResponse("", "", "", "", "", "");
            try
            {
                /*         
                 * ********************************************Testing Data
                    "AGGRID":"OTOE0027"
                    "CORPID":"PRACHICIB1"
                    "USERID":"USER3"
                    "URN":"SR191962059"
                    "UNIQUEID": "111100236523"
                  
                 * strAPIResponse = "";
                 * **********************************
                 */
                string strJsonDatatoEncrypt = string.Empty;
                strJsonDatatoEncrypt = JsonConvert.SerializeObject(vPostBankTransactionStatus);

                //Call for encryption
                strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                //Call For Balance Fetch


                // For UAT Call *******************************#############################
                //pIPAddress = "bijliserver54.bijliftt.com"; //need to change for Production
                //strAPIResponse = ExecuteAPIRequest(strEncryptedData, "https://apigwuat.icicibank.com:8443/api/Corporate/CIB/v1/TransactionInquiry", pIPAddress);


                // For Production Call *******************************#############################
                pIPAddress = ServerIP; //"45.248.57.81";// "bijliserver57.bijliftt.com"; //need to change for Production
                // pIPAddress = "12.203.162.57";// "bijliserver57.bijliftt.com"; //need to change for Production
                strAPIResponse = ExecuteAPIRequest(strEncryptedData, "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/TransactionInquiry", pIPAddress);
                //Call for Decryption
                strDeryptedData = DataDeCrypt(strAPIResponse);

                objResponse = JsonConvert.DeserializeObject<ICICBankTransactionStatusResponse>(strDeryptedData);

            }
            catch (Exception ex)
            {
                ICICBankTransactionStatusResponse objResponseError = new ICICBankTransactionStatusResponse("Error", ex.ToString(), pIPAddress, strEncryptedData, strDeryptedData, strAPIResponse);
                return objResponseError;
            }
            finally
            {

            }
            return objResponse;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vPostBankTransaction"></param>
        /// <returns></returns>
        public ICICBankTransactionResponse ICICBankTransaction(PostBankTransaction vPostBankTransaction)
        {
            string pIPAddress = string.Empty;
            string strEncryptedData = string.Empty;
            string strDeryptedData = string.Empty;
            string strAPIResponse = string.Empty;
            string pURL = string.Empty;
            //List<ICICBankTransactionResponse> row = new List<ICICBankTransactionResponse>();
            ICICBankTransactionResponse objResponse = new ICICBankTransactionResponse("", "", "", "", "", "", "", "", "", "", "");
            try
            {
                /*         
                 * ********************************************Testing Data
                    "AGGRID":"OTOE0027"
                    "AGGRNAME":"CENTRUM"
                    "CORPID":"PRACHICIB1"
                    "USERID":"USER3"
                    "URN":"SR191962059"
                    "DEBITACC":"000451000301"
                    "CREDITACC":"000405002777"
                    "IFSC":"ICIC0000011"
                    "AMOUNT":"1"
                    "CURRENCY":"INR",
                    "TXNTYPE":"TPA",
                    "PAYEENAME" : "",//it should be any payee name
                    "UNIQUEID" : "",//it should be unique for each transaction
                    "REMARKS" : "testing"
                 * strAPIResponse = "a4bjGus8IIvS8Rd8/0oGTlwFXchO+VCh4J0s2ungsjtBPGWx0Mashp5hNPAJ6wtYDa05b00i4Ukl\r\ng6t1e22TIWa5our80jwyqhNJe7pRGPBUsLdtmXRAB0qCq5eS
                 * Gn6gru6kP7PQKJGoRkubXgQkitpn\r\n0NTIoqYh4KTW4nBR/cwyR5X38hikCO2n7PCDbiT8PTijWe8NJ4HB69/JsFM/8ddWMWYItPJsOWsV\r\nUus4JL6xwswL1kNdE73uKeBmd6MzLsCEAc
                 * YeT2RyPgvvHR4b/7fs+Rks7gGuAdwSxZ3PR80Tmw+f\r\nDmYD2TTadhVuOHRtN8GZN0geFIROyJEGCUENlg==";
                 * **********************************
                 */
                string strJsonDatatoEncrypt = string.Empty;
                strJsonDatatoEncrypt = JsonConvert.SerializeObject(vPostBankTransaction);
                //Call for encryption
                strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                //Save LOG
                string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "ICICI", vPostBankTransaction.UNIQUEID));
                try
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                    File.WriteAllText(folderPath + "/" + "Request.text", strJsonDatatoEncrypt);
                }
                finally { }

                // For UAT Call *******************************#############################
                //pIPAddress = "bijliserver54.bijliftt.com"; //need to change for Production
                //strAPIResponse = ExecuteAPIRequest(strEncryptedData, "https://apigwuat.icicibank.com:8443/api/Corporate/CIB/v1/Transaction", pIPAddress);


                // For Production Call *******************************#############################
                pIPAddress = ServerIP;//"45.248.57.81";
                //pIPAddress = "12.203.162.57";// "bijliserver57.bijliftt.com"; //need to change for Production     
                pURL = "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/Transaction";

                strAPIResponse = ExecuteAPIRequest(strEncryptedData, pURL, pIPAddress);
                //Call for Decryption
                strDeryptedData = DataDeCrypt(strAPIResponse);
                //------------------------Save LOG --------------------------------
                try
                {
                    SaveIciciBankLog(vPostBankTransaction.UNIQUEID, strEncryptedData, strAPIResponse, pURL, strJsonDatatoEncrypt, strDeryptedData);
                    File.WriteAllText(folderPath + "/" + "Response.text", strDeryptedData);
                }
                finally { }
                //---------------------------------------------------------------               
                objResponse = JsonConvert.DeserializeObject<ICICBankTransactionResponse>(strDeryptedData);

                //row.Add(new ICICBankTransactionResponse(objResponse.AGGR_ID, objResponse.AGGR_NAME, objResponse.CORP_ID, objResponse.USER_ID, objResponse.URN
                //  , objResponse.UNIQUEID, objResponse.UTRNUMBER, objResponse.REQID, objResponse.STATUS, objResponse.Response, objResponse.message));

            }
            catch (Exception ex)
            {
                ICICBankTransactionResponse objResponseError = new ICICBankTransactionResponse("Error", ex.ToString(), "", "", "", "", "", pIPAddress, strEncryptedData, strDeryptedData, strAPIResponse);
                return objResponseError;
            }
            finally
            {

            }
            return objResponse;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vPostBalEnqReq"></param>
        /// <returns></returns>
        public ICICBalanceFetchResponse ICICBalanceFetch(PostBalEnqReq vPostBalEnqReq)
        {
            string strEncryptedData = string.Empty;
            string strDeryptedData = string.Empty;
            string strAPIResponse = string.Empty;
            string pIPAddress = string.Empty;
            string pUrl = string.Empty;
            ICICBalanceFetchResponse objResponse = new ICICBalanceFetchResponse("", "", "", "", "", "", "", "", "", "");
            try
            {
                /*
                 * ********************************************Testing Data
                 objBalEnqReq.AGGRID = "OTOE0027";
                 objBalEnqReq.CORPID = "PRACHICIB1";
                 objBalEnqReq.USERID = "USER3";
                 objBalEnqReq.URN = "SR191962059";
                 objBalEnqReq.ACCOUNTNO = "000451000301";
                 strAPIResponse = "XhUzFLkaIsRVsYpgI3O/ylfnMYnqn9uusGK5AsL/FxztaKUGqH0edKrebOdoUYQ94FKF55X7WJjP\r\nhn3Xc3irT3BOkhRFiQNejNJ2j52h7+1eCeLaA8Xx
                 TsorHZEIjc7rW5Fr7VmRRhTSkbRE/preMgnF\r\nL4OLjDileXSnz+7CCRKsu4TjYDAxbLYgTeGNN1te23cVtwi4LsgpLFHIIKvR8XgYSNpu4VCxIP57\r\nWLM+o9ijzSaSwmL0LFtUnafcxGfDBy
                 WWLWvkCDaJQX6H3YtCSlo9IToWxG2lx5+rTfry7TqYjr63\r\nbhFJnkktQDDTXFD9DyERZoETxLiCWuha0GKMhQ==";
                 * **********************************
                 */
                string strJsonDatatoEncrypt = string.Empty;
                strJsonDatatoEncrypt = JsonConvert.SerializeObject(vPostBalEnqReq);

                //Call for encryption
                strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                //Call For Balance Fetch

                // For UAT Call *******************************#############################
                // pIPAddress = "bijliserver54.bijliftt.com"; //need to change for Production
                //strAPIResponse = ExecuteAPIRequest(strEncryptedData, "https://apigwuat.icicibank.com:8443/api/Corporate/CIB/v1/BalanceInquiry", pIPAddress);

                // For Production Call *******************************#############################
                pIPAddress = ServerIP;//"45.248.57.81";// "bijliserver57.bijliftt.com"; //need to change for Production
                // pIPAddress = "12.203.162.57";// "bijliserver57.bijliftt.com"; //need to change for Production
                pUrl = "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/BalanceInquiry";
                strAPIResponse = ExecuteAPIRequest(strEncryptedData, pUrl, pIPAddress);

                //Call for Decryption
                try
                {
                    strDeryptedData = DataDeCrypt(strAPIResponse);
                }
                catch
                {
                    strDeryptedData = "";
                }
                //*************** Save LOG ******************
                try
                {
                    SaveIciciBankLog(vPostBalEnqReq.ACCOUNTNO, strEncryptedData, strAPIResponse, pUrl, strJsonDatatoEncrypt, strDeryptedData);
                }
                finally
                {
                }

                objResponse = JsonConvert.DeserializeObject<ICICBalanceFetchResponse>(strDeryptedData);
            }
            catch (Exception ex)
            {
                ICICBalanceFetchResponse objResponseError = new ICICBalanceFetchResponse("Error", ex.ToString(), "", "", "", "", pIPAddress, strEncryptedData, strDeryptedData, strAPIResponse);
                return objResponseError;
            }
            finally
            {

            }
            return objResponse;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requeststr"></param>
        /// <returns></returns>
        public string DataEncrypt(string requeststr)
        {
            string EncryptedAESKeyBase64 = string.Empty;
            string EncryptedIVKeyBase64 = string.Empty;
            string EncryptedStrBase64 = "";
            var publicKey = string.Empty;
            byte[] AesKey, AesIV;

            /************************************************************************************************************/
            try
            {
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    //////////////////////// Public Key of ICICI required for Encryption 
                    /////////////////////// ACTUAL CODING FOR ICICI ////////////////////////////////////////////////
                    string vCretificatePath = "~/Certificate/ICICI_Pub.cer";
                    var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath(vCretificatePath);
                    //var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/ICICI_FTT_PublicKey.cer");
                    publicKey = File.ReadAllText(vStrMycertPub);
                    var utf8PublicKey = Encoding.UTF8.GetBytes(publicKey);

                    EncryptedStrBase64 = RSACryptoService.EncryptRsa(Encoding.UTF8.GetBytes(requeststr), utf8PublicKey);

                    /////////////////////////////////////////////////////////////////////////////////////////////////
                    /////////////////////////////////////////////////////////////////////////////////////////////////

                    //2. Generate a 128 -bits AES Key (Symmetric  Key Generator can be used ).
                    //3. This Generated AES key should be unique for each request.
                    //4. Generate a 16 byte IV - Initiation Vector ( RandomNo Generator can be used ).
                    rijAlg.BlockSize = 128;
                    rijAlg.KeySize = 128;
                    rijAlg.Mode = CipherMode.ECB;
                    rijAlg.Padding = PaddingMode.None;
                    rijAlg.GenerateKey();
                    rijAlg.GenerateIV();
                    //5. Use this AES key and IV values which will be used  to  decrypt the response Payload.
                    AesKey = rijAlg.Key;
                    AesIV = rijAlg.IV;
                    //9.  Encrypt the generated AES key with shared RSA public Key using the RSA algorithm RSA/ECB/PKCS1Padding.
                    //10. Encode the encrypted AES key value as Base64.
                    EncryptedAESKeyBase64 = RSACryptoService.EncryptRsa(rijAlg.Key, utf8PublicKey);
                    //11.  Encrypt  the generated IV with shared  RSA public Key using the RSA algorithm RSA/ECB/PKCS1Padding.
                    //12.  Encode the encrypted IV value as Base64.
                    EncryptedIVKeyBase64 = RSACryptoService.EncryptRsa(rijAlg.IV, utf8PublicKey);
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    ///////////////////////////////////////////////////////////////////////////////////////////////
                    ///////////////////////////////////////////////////////////////////////////////////////////////
                    // Create the streams used for encryption.
                    //using (MemoryStream msEncrypt = new MemoryStream())
                    //{
                    //    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    //    {
                    //        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    //        {

                    //            //Write all data to the stream.
                    //            swEncrypt.Write(requeststr);
                    //        }
                    //        encrypted = msEncrypt.ToArray();
                    //    }
                    //}
                }


                return EncryptedStrBase64;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="APIresponsestr"></param>
        /// <returns></returns>
        public string DataDeCrypt(string APIresponsestr)
        {
            byte[] decodeAPIResponse = Convert.FromBase64String(APIresponsestr);
            var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/BijliPrivateKey.pfx");
            string vActualResposetext = RSACryptoService.decryptRsa(decodeAPIResponse, vStrMycertPub);
            return vActualResposetext;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonPayload"></param>
        /// <param name="vURL"></param>
        /// <returns></returns>
        public string ExecuteAPIRequest(string jsonPayload, string vURL, string vIPAddress)
        {
            string responsedata = string.Empty;
            try
            {
                var vBalFetchURLString = vURL;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(vBalFetchURLString);
                httpWebRequest.ContentType = "text/plain";
                httpWebRequest.ContentLength = 684;
                httpWebRequest.Method = "POST";
                //For UAT ***********************************************************************************************
                //httpWebRequest.Headers.Add("apikey", "cd9a1e0e-1c0a-4b63-9df6-b6c50695528f");
                //httpWebRequest.Host = "apigwuat.icicibank.com:8443";
                //For Production ***********************************************************************************************
                //httpWebRequest.Headers.Add("apikey", "ae8d1913-c2a9-4dcc-8310-a05412a49c1b");
                httpWebRequest.Headers.Add("apikey", "ObPxZx0jrA0oTrmsUapErTKqVAGwY4AY");
                httpWebRequest.Host = "apibankingone.icicibank.com";
                httpWebRequest.Headers.Add("x-forwarded-for", vIPAddress);
                httpWebRequest.Accept = "*/*";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                byte[] data = Encoding.UTF8.GetBytes(jsonPayload);
                httpWebRequest.ContentLength = data.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var API_Response = streamReader.ReadToEnd(); ;
                    responsedata = API_Response.ToString().Trim();
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
                return Response;
            }
            finally
            {
            }
            return responsedata;
        }

        public string ICICIDisbursement(string pXml, string pUserId)
        {
            string responsedata = string.Empty;
            string vXML = "";
            DataTable dt = new DataTable();
            int vUserId = Convert.ToInt32(pUserId);
            try
            {
                #region CREATE DATA TABLE
                DataTable dtdata = new DataTable("Table1");
                dtdata.Columns.Add("LoanId");
                dtdata.Columns.Add("Response");
                dtdata.Columns.Add("Message");
                dtdata.Columns.Add("UTRNUMBER");
                #endregion

                dt = CreateDataTableXML(pXml);
                foreach (DataRow dr in dt.Rows)
                {
                    var vOBJ = new PostBankTransaction()
                    {
                        AGGRID = dr["AGGRID"].ToString(),
                        AGGRNAME = dr["AGGRNAME"].ToString(),
                        CORPID = dr["CORPID"].ToString(),
                        USERID = dr["USERID"].ToString(),
                        URN = dr["URN"].ToString(),
                        DEBITACC = dr["DEBITACC"].ToString(),
                        CREDITACC = dr["CREDITACC"].ToString(),
                        IFSC = dr["IFSC"].ToString(),
                        AMOUNT = dr["AMOUNT"].ToString(),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        TXNTYPE = dr["TXNTYPE"].ToString(),
                        PAYEENAME = dr["PAYEENAME"].ToString(),
                        UNIQUEID = dr["UNIQUEID"].ToString(), ///"1333333311111"
                        REMARKS = dr["REMARKS"].ToString()
                    };
                    try
                    {
                        string pIPAddress = string.Empty;
                        string strEncryptedData = string.Empty;
                        string strDeryptedData = string.Empty;
                        string strAPIResponse = string.Empty;
                        string pURL = string.Empty;
                        ICICBankTransactionResponse objResponse = new ICICBankTransactionResponse("", "", "", "", "", "", "", "", "", "", "");
                        try
                        {
                            string strJsonDatatoEncrypt = string.Empty;
                            strJsonDatatoEncrypt = JsonConvert.SerializeObject(vOBJ);
                            strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                            pIPAddress = ServerIP;//"45.248.57.81";
                            pURL = "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/Transaction";
                            strAPIResponse = ExecuteAPIRequest(strEncryptedData, pURL, pIPAddress);
                            //Call for Decryption
                            strDeryptedData = DataDeCrypt(strAPIResponse);
                            //------------------------Save LOG --------------------------------
                            try
                            {
                                SaveIciciBankLog(vOBJ.UNIQUEID, strEncryptedData, strAPIResponse, pURL, strJsonDatatoEncrypt, strDeryptedData);
                            }
                            finally { }
                            //---------------------------------------------------------------  
                            //strDeryptedData = "{\"CORP_ID\":\"581109799\",\"USER_ID\":\"AMOLBHAR\",\"AGGR_ID\":\"OTOE0027\",\"AGGR_NAME\":\"CENTRUM\",\"REQID\":\"1084131665\",\"STATUS\":\"SUCCESS\",\"UNIQUEID\":\"11150771953\",\"URN\":\"SR191962059\",\"UTRNUMBER\":\"031235410641\",\"RESPONSE\":\"SUCCESS\"}";
                            objResponse = JsonConvert.DeserializeObject<ICICBankTransactionResponse>(strDeryptedData);
                        }
                        catch (Exception ex)
                        {
                            ICICBankTransactionResponse objResponseError = new ICICBankTransactionResponse("Error", ex.ToString(), "", "", "", "", "", pIPAddress, strEncryptedData, strDeryptedData, strAPIResponse);
                        }
                        finally
                        {

                        }
                        if (objResponse != null)
                        {
                            #region INSERT DATA INTO DATATABLE
                            DataRow dr1 = dtdata.NewRow();
                            dr1["LoanId"] = (objResponse.Response.ToUpper() == "SUCCESS" ? objResponse.UNIQUEID.ToUpper() : dr["UNIQUEID"].ToString());
                            dr1["Response"] = objResponse.Response.ToUpper();
                            if (objResponse.Response.ToUpper() != "SUCCESS")
                            {
                                dr1["Message"] = objResponse.message.ToUpper();
                            }
                            else
                            {
                                dr1["Message"] = objResponse.STATUS.ToUpper();
                            }
                            if (objResponse.Response.ToUpper() == "SUCCESS")
                            {
                                dr1["UTRNUMBER"] = objResponse.UTRNUMBER.ToUpper();
                            }
                            else
                            {
                                dr1["UTRNUMBER"] = "";
                            }
                            dtdata.Rows.Add(dr1);
                            dtdata.AcceptChanges();
                            if (dtdata.Rows.Count > 0)
                            {
                                using (StringWriter oSW = new StringWriter())
                                {
                                    dtdata.WriteXml(oSW);
                                    vXML = oSW.ToString();
                                }
                                InsertNEFTTransferAPI(vXML, vUserId);
                            }
                            dtdata.Clear();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                    finally
                    {
                    }
                }
            }
            finally
            {
            }
            return responsedata;
        }

        public Int32 InsertNEFTTransferAPI(string pXml, Int32 pCreatedby)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertNEFTTransferAPI";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedby);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 3;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public string ICICIICDisbursement(string pXml, string pUserId, string pTransDate, string pBankDescId)
        {
            string responsedata = string.Empty;
            string vXML = "";
            DataTable dt = new DataTable();
            int vUserId = Convert.ToInt32(pUserId);
            try
            {
                #region CREATE DATA TABLE
                DataTable dtdata = new DataTable("Table1");
                dtdata.Columns.Add("LoanId");
                dtdata.Columns.Add("AMOUNT");
                dtdata.Columns.Add("Response");
                dtdata.Columns.Add("Message");
                dtdata.Columns.Add("UTRNUMBER");
                #endregion

                dt = CreateDataTableXML(pXml);
                foreach (DataRow dr in dt.Rows)
                {
                    var vOBJ = new PostBankTransaction()
                    {
                        AGGRID = dr["AGGRID"].ToString(),
                        AGGRNAME = dr["AGGRNAME"].ToString(),
                        CORPID = dr["CORPID"].ToString(),
                        USERID = dr["USERID"].ToString(),
                        URN = dr["URN"].ToString(),
                        DEBITACC = dr["DEBITACC"].ToString(),
                        CREDITACC = dr["CREDITACC"].ToString(),
                        IFSC = dr["IFSC"].ToString(),
                        AMOUNT = dr["AMOUNT"].ToString(),
                        CURRENCY = dr["CURRENCY"].ToString(),
                        TXNTYPE = dr["TXNTYPE"].ToString(),
                        PAYEENAME = dr["PAYEENAME"].ToString(),
                        UNIQUEID = dr["UNIQUEID"].ToString(), ///"1333333311111"
                        REMARKS = dr["REMARKS"].ToString()
                    };
                    try
                    {
                        string pIPAddress = string.Empty;
                        string strEncryptedData = string.Empty;
                        string strDeryptedData = string.Empty;
                        string strAPIResponse = string.Empty;
                        string pURL = string.Empty;
                        ICICBankTransactionResponse objResponse = new ICICBankTransactionResponse("", "", "", "", "", "", "", "", "", "", "");
                        try
                        {
                            string strJsonDatatoEncrypt = string.Empty;
                            strJsonDatatoEncrypt = JsonConvert.SerializeObject(vOBJ);
                            strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                            pIPAddress = ServerIP;// "45.248.57.81";
                            pURL = "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/Transaction";
                            strAPIResponse = ExecuteAPIRequest(strEncryptedData, pURL, pIPAddress);
                            //strAPIResponse = "fAQJyTlkBErZBqowtNTZFIGxyraaPZ1Z0U8fgIHosQUgCPN4aPg5L1IurofE1X0u09LvMzTC++3UOSmON8oLwJBcJ+bActsste4WFhRWv3Ae9fRhtkdVfLF7DcrFMp5MQSsba86ihyqlkoZzwG+y5MHFPHPI3IPBiaWtKk1SxDGkMEDp6V4trqNT0zAxCebOQltlO+4n3TLuF53L7l0qJTuguUgQAyTLx75Joie1uG9bDSUsopj5sbuCkVLHS2BJxcaaDhxWAGT1s63PqbVe+xPyZ2MsKn0f+VKhK0Nx6bm2GuCyY7SJnhGHWXxaA/FkYLk8PTCDncELEebjVVzlUM4m9cfvIKH7ezOoHDC450OCRy1CQa3FG6h9R7nxKXLvAM0FmndBgcA3CrYcFon4V0aG14Fsmufrwu3e/yWLLAxzzUnoLxf/SHQ2NAQIgSECKUMa6geJhEL2cvn61kfaLDN23/ipnLjg6rps1dlcaBX5yznodhFqG9gWliLpe6wExAxl2VpDmb65guW2NOpgCmNXFlzWaZcolbu6Rl3GFy3QnaaDr4zkfyajpqPqksjzF4NhVMic/E0v03mr7t7+7pvVoEX22Otna9uNtdPqnlvrjhx0PrP9ne21KUhPUM6BJDedrkRty7konzop0Nrl5qmRzer3AyMVOzev47i7iKg=";
                            //Call for Decryption
                            strDeryptedData = DataDeCrypt(strAPIResponse);
                            //------------------------Save LOG --------------------------------
                            try
                            {
                                SaveIciciBankLog(vOBJ.UNIQUEID, strEncryptedData, strAPIResponse, pURL, strJsonDatatoEncrypt, strDeryptedData);
                            }
                            catch (Exception ex) { }
                            finally { }
                            //---------------------------------------------------------------                              
                            objResponse = JsonConvert.DeserializeObject<ICICBankTransactionResponse>(strDeryptedData);
                        }
                        catch (Exception ex)
                        {
                            ICICBankTransactionResponse objResponseError = new ICICBankTransactionResponse("Error", ex.ToString(), "", "", "", "", "", pIPAddress, strEncryptedData, strDeryptedData, strAPIResponse);
                        }
                        finally
                        {
                        }
                        if (objResponse != null)
                        {
                            #region INSERT DATA INTO DATATABLE
                            DataRow dr1 = dtdata.NewRow();
                            dr1["LoanId"] = (objResponse.Response.ToUpper() == "SUCCESS" ? objResponse.UNIQUEID.ToUpper() : dr["UNIQUEID"].ToString());
                            dr1["Response"] = objResponse.Response.ToUpper();
                            if (objResponse.Response.ToUpper() != "SUCCESS")
                            {
                                dr1["Message"] = objResponse.message.ToUpper();
                            }
                            else
                            {
                                dr1["Message"] = objResponse.STATUS.ToUpper();
                            }
                            if (objResponse.Response.ToUpper() == "SUCCESS")
                            {
                                dr1["UTRNUMBER"] = objResponse.UTRNUMBER.ToUpper();
                            }
                            else
                            {
                                dr1["UTRNUMBER"] = "";
                            }
                            dr1["AMOUNT"] = dr["AMOUNT"].ToString();

                            dtdata.Rows.Add(dr1);
                            dtdata.AcceptChanges();
                            if (dtdata.Rows.Count > 0)
                            {
                                using (StringWriter oSW = new StringWriter())
                                {
                                    dtdata.WriteXml(oSW);
                                    vXML = oSW.ToString();
                                }
                                InsertNEFTTransferICAPI(vXML, vUserId, setDate(pTransDate), pBankDescId);
                            }
                            dtdata.Clear();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                    finally
                    {
                    }
                }
            }
            finally
            {
            }
            return responsedata;
        }

        public Int32 InsertNEFTTransferICAPI(string pXml, Int32 pCreatedby, DateTime pTransDate, string pBankDescId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertNEFTTransferICAPI";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", pCreatedby);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pTransDate", pTransDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBankDescId", pBankDescId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                return vErr;
            }
            catch (Exception ex)
            {
                return 3;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region DigitalDoc Upload

        public List<DigitalDocumentSave> DigitalSignedDocUpload(Stream DocFile)
        {
            string vErr = "", vResponse = "NotDone";
            string fileName = "";
            string vFileTag = "";

            byte[] binaryWriteArray = null;
            List<DigitalDocumentSave> row = new List<DigitalDocumentSave>();
            string vLoanType = "";
            MultipartParser parser = new MultipartParser(DocFile);

            //string vPC =  parser.ContentType;
            //string vPFilename =  parser.Filename;

            List<StreamContent> obj = parser.StreamContents;

            string a = obj[0].PropertyName;
            string b = obj[0].StringData;
            string c = ((!obj[0].IsFile) ? "N" : "Y");
            string d = obj[0].FileName;


            string a1 = obj[1].PropertyName;
            string b1 = obj[1].StringData;
            string c1 = ((!obj[1].IsFile) ? "N" : "Y");
            string d1 = obj[1].FileName;

            string path = @"C:\Log.txt";
            if (!File.Exists(path))
            {
                File.Create(path);
                TextWriter tw = new StreamWriter(path);
                tw.WriteLine(a + ".." + b + ".." + c + ".." + d + ".." + a1 + ".." + b1 + ".." + c1 + ".." + d1); ;
                tw.Close();
            }
            else if (File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine(a + ".." + b + ".." + c + ".." + d + ".." + a1 + ".." + b1 + ".." + c1 + ".." + d1); ;
                }
            }
            // Console.WriteLine(a + ".." + b + ".." + c + ".." + d + ".." + a1 + ".." + b1 + ".." + c1 + ".." + d1);


            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "LoanType")
                        {
                            vLoanType = content.StringData;
                        }
                    }
                    else
                    {
                        binaryWriteArray = content.Data;
                        fileName = Path.GetFileName(content.FileName);
                        vFileTag = "";
                        //vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        vFileTag = fileName.Substring(0, 11);

                    }
                }

                if (vLoanType == "")
                    vLoanType = "JLG";

                vResponse = UpdateDigitalDocYN(vFileTag, vLoanType);

                if (vResponse == "Done" && vLoanType != "")
                {
                    vErr = SaveDigitalDoc(binaryWriteArray, "DigitalDoc", vLoanType, vFileTag);
                    if (vErr.Equals("Y"))
                    {
                        if (File.Exists(path))
                        {
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine("Successful" + ".." + fileName); ;
                            }
                        }
                        row.Add(new DigitalDocumentSave("Successful", fileName));
                    }
                    else
                    {
                        if (File.Exists(path))
                        {
                            using (var tw = new StreamWriter(path, true))
                            {
                                tw.WriteLine("Failed" + ".." + fileName); ;
                            }
                        }
                        row.Add(new DigitalDocumentSave("Failed", fileName));
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        using (var tw = new StreamWriter(path, true))
                        {
                            tw.WriteLine("Failed" + ".." + fileName); ;
                        }
                    }
                    row.Add(new DigitalDocumentSave("Failed", fileName));
                }

                return row;
            }
            else
            {
                row.Add(new DigitalDocumentSave("Failed", "No Data Found"));
            }
            return row;
        }

        private string SaveDigitalDoc(byte[] imageBinary, string imageGroup, string LoanType, string imageName)
        {
            string vSMEDigitalSignServerPath = ConfigurationManager.AppSettings["SMEDigitalSignServerPath"];
            string isImageSaved = "N";
            string folderPath = "", filePath = "";
            if (LoanType == "JLG")
            {
                //string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", imageGroup, folderName));
                folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}", imageGroup));
            }
            else
            {
                //string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", imageGroup, folderName));
                folderPath = string.Format(vSMEDigitalSignServerPath + "\\Files\\{0}", imageGroup);
            }

            System.IO.Directory.CreateDirectory(folderPath);
            filePath = string.Format("{0}/{1}.pdf", folderPath, imageName);
            if (imageBinary != null)
            {
                File.WriteAllBytes(filePath, imageBinary);
                isImageSaved = "Y";
            }

            return isImageSaved;
        }
        #endregion

        #region UpdateDigitalDocYN

        public string UpdateDigitalDocYN(string pLoanAppId, string pCallFrom)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateDigitalDocYN";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanAppId", pLoanAppId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pCallFrom", pCallFrom);
                DBConnect.Execute(oCmd);
                return "Done";
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

        #region KARZA Integration

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private readonly Encoding encoding = Encoding.UTF8;

        #region GetMultipartFormData
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

                if (param.Value is OCRParameter)
                {
                    OCRParameter fileToUpload = (OCRParameter)param.Value;

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
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v2/voter" : "https://testapi.karza.in/v2/voter";
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
                    request.Headers.Add("x-karza-key", vKarzaKey);
                    request.Host = vKarzaEnv == "PROD" ? "api.karza.in" : "testapi.karza.in";

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
                    //responsedata = "{\"request_id\":\"930a2b3d-bf1b-429c-9c64-13cf2d143ea0\",\"result\":{\"ac_name\":\"Mahagama\",\"ac_no\":\"18\",\"age\":\"49\",\"district\":\"Godda\",\"dob\":\"\",\"epic_no\":\"GCG1994440\",\"gender\":\"M\",\"house_no\":\"\",\"id\":\"\",\"last_update\":\"\",\"name\":\"shankar sah\",\"name_v1\":\"शंकर साह\",\"name_v2\":\"\",\"name_v3\":\"\",\"part_name\":\"Anganbari Kendra Rajpur\",\"part_no\":\"400\",\"pc_name\":\"Godda\",\"ps_lat_long\":\"0.0,0.0\",\"ps_name\":\"Anganbari Kendra Rajpur\",\"rln_name\":\"ram dev sah\",\"rln_name_v1\":\"राम देव साह\",\"rln_name_v2\":\"\",\"rln_name_v3\":\"\",\"rln_type\":\"F\",\"section_no\":\"3\",\"slno_inpart\":\"725\",\"st_code\":\"S27\",\"state\":\"Jharkhand\"},\"status_code\":\"101:Valid Authentication\"}";
                    // responsedata =RemoveSpecialCharacters("{\"KarzaVoterIDKYCValidationResult\":{\"request_id\":\"dedb5bd1-7e48-452a-b518-b9732d6d2160\",\"result\":{\"ac_name\":\"Godda\",\"ac_no\":\"17\",\"age\":\"62\",\"district\":\"Godda\",\"dob\":\"\",\"epic_no\":\"BWB3380303\",\"gender\":\"F\",\"house_no\":\"\",\"id\":\"\",\"last_update\":\"\",\"name\":\"kari devi\",\"name_v1\":\"कारी देवी\",\"name_v2\":\"\",\"name_v3\":\"\",\"part_name\":\"Shri Nandan Yamuna Memorial +2 High School Lukluki (South Part)\",\"part_no\":\"234\",\"pc_name\":\"Godda\",\"ps_lat_long\":\"24.85527800,87.30561100\",\"ps_name\":\"Shri Nandan Yamuna Memorial +2 High School Lukluki\",\"rln_name\":\"kam dev mandal\",\"rln_name_v1\":\"काम देव मण्डल\",\"rln_name_v2\":\"\",\"rln_name_v3\":\"\",\"rln_type\":\"H\",\"section_no\":\"1\",\"slno_inpart\":\"334\",\"st_code\":\"S27\",\"state\":\"Jharkhand\"},\"status_code\":\"101:Valid Authentication\"}}");
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

                //HttpWebResponse res = (HttpWebResponse)we.Response;
                //KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();

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

        #region OCRPhototoData
        public List<OCRParameterResponse> OCRPhototoData(Stream DocFile)
        {
            string vStringResponse = "", vXml = "", vStatusCode = "";
            List<OCRParameterResponse> row = new List<OCRParameterResponse>();
            string vMobileNo = "", vEoid = "", vBranchCode = "";
            try
            {
                MultipartParser parser = new MultipartParser(DocFile);
                if (parser != null && parser.Success)
                {
                    foreach (var content in parser.StreamContents)
                    {
                        if (!content.IsFile)
                        {
                            if (content.PropertyName == "IdData")
                            {
                                string[] arr = content.StringData.Split('#');
                                vMobileNo = arr[0].ToString();
                                vEoid = arr[2].ToString();
                                vBranchCode = arr[1].ToString();
                            }
                        }
                        else
                        {
                            byte[] binaryWriteArray = content.Data;
                            string fileName = Path.GetFileName(content.FileName);
                            string vFileTag = "";
                            vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                            vStringResponse = OCRPhototoDataInternal(binaryWriteArray);
                            // vStringResponse = "{'requestId':'7365f177-4e20-49ec-acfb-7105008c7329','result':[{'type':'Voterid Front','details':{'voterid':{'value':'OR/02/015/034131'},'name':{'value':'SUKANTI SAHU'},'gender':{'value':'FEMALE'},'relation':{'value':'H  RABINDRA SAHU'},'dob':{'value':''},'doc':{'value':'1.1.1995'},'age':{'value':'25'}}}],'statusCode':101}";
                            // vStringResponse = "{'requestId':'68c60c69-5244-49bb-be94-9bd1c5377f58','result':[],'statusCode':102}";
                            //Save Response data                                                     
                            // row.Add(new OCRParameterResponse("Success", vStringResponse));
                        }
                    }
                    string vResponseData = vStringResponse;
                    //try
                    //{
                    //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "OCR", vMobileNo));
                    //    System.IO.Directory.CreateDirectory(folderPath);
                    //    Guid guid = Guid.NewGuid();
                    //    File.WriteAllText(folderPath + "/" + Convert.ToString(guid) + ".text", vResponseData);
                    //}
                    //finally { }
                    vResponseData = vResponseData.Replace("\u0000", "");
                    vResponseData = vResponseData.Replace("\\u0000", "");
                    vXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));//Convert Json to Xml  
                    vStatusCode = SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
                    if (vStatusCode == "101")
                    {
                        row.Add(new OCRParameterResponse("Success", vStringResponse));
                    }
                    else
                    {
                        row.Add(new OCRParameterResponse("Failure", vStringResponse));
                    }
                }
                else
                {
                    row.Add(new OCRParameterResponse("Failure", "{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":504}"));
                    vXml = AsString(JsonConvert.DeserializeXmlNode("{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":700}", "root"));//Convert Json to Xml  
                    SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
                }
            }
            catch (Exception ex)
            {
                row.Add(new OCRParameterResponse("Failure", "{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":504}"));
                vXml = AsString(JsonConvert.DeserializeXmlNode("{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":701}", "root"));//Convert Json to Xml  
                SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
                // row.Add(new OCRParameterResponse("Failure", ex.ToString()));
            }
            return row;
        }
        ///OCR technology
        ///
        public string OCRPhototoDataInternal(byte[] formData) //Stream DocFile
        {
            // Create request and receive response
            //DocFile.Position = 0;
            //byte[] formData = new byte[DocFile.Length];
            //DocFile.Read(formData, 0, formData.Length);
            //DocFile.Close();
            // byte[] formData = ReadToEnd(DocFile);
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("file", new OCRParameter(formData));
            postParameters.Add("maskAadhaar", "false");
            postParameters.Add("hideAadhaar", "false");
            postParameters.Add("conf", "true");
            postParameters.Add("docType", "false");

            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formDataforRequest = GetMultipartFormData(postParameters, formDataBoundary);
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/ocr/kyc" : "https://testapi.karza.in/v3/ocr/kyc";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.CookieContainer = new CookieContainer();
                request.Headers.Add("cache-control", "no-cache");
                request.Headers.Add("x-karza-key", vKarzaKey);
                request.Host = vKarzaEnv == "PROD" ? "api.karza.in" : "testapi.karza.in";
                request.KeepAlive = false;
                request.Timeout = System.Threading.Timeout.Infinite;
                request.ContentLength = formDataforRequest.Length;

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formDataforRequest, 0, formDataforRequest.Length);
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
            catch (WebException ex)
            {
                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                return Response.Replace("status", "statusCode");
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public class OCRParameter
        {
            public byte[] file { get; set; }
            public string maskAadhaar { get; set; }
            public string hideAadhaar { get; set; }
            public string conf { get; set; }
            public string docType { get; set; }

            public string FileName { get; set; }
            public string ContentType { get; set; }

            public OCRParameter(byte[] file) : this(file, null) { }
            public OCRParameter(byte[] file, string filename) : this(file, filename, null) { }
            public OCRParameter(byte[] pfile, string filename, string contenttype)
            {
                file = pfile;
                FileName = filename;
                ContentType = contenttype;
            }
        }
        #endregion

        #region GetKarzaToken
        public string GetKarzaToken(string vAadharNo)
        {
            string requestBody = @"{""productId"": [""aadhaar_xml""]}";
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/get-jwt" : "https://testapi.karza.in/v3/get-jwt";
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                string vKarzaToken = res.result.data.karzaToken;
                try
                {
                    SaveKarzaAadharToken(vAadharNo, vKarzaToken);
                }
                finally
                {
                }
                return vKarzaToken;
            }
            catch (Exception ex)
            {
                return "Token not Found.";
            }
            finally
            {
                // streamWriter = null;
            }
        }
        #endregion

        #region NameMatchRequest
        public class NameMatchRequest
        {
            public string name1 { get; set; }
            public string name2 { get; set; }
            public string type { get; set; }
            public string preset { get; set; }
        }
        #endregion

        #region AddressMatchRequest
        public class AddressMatchRequest
        {
            public string address1 { get; set; }
            public string address2 { get; set; }
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
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/name" : "https://testapi.karza.in/v3/name";
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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

        #region AddressMatching
        public MatchResponse AddressMatching(AddressMatch vAddressMatch)
        {
            var req = new AddressMatchRequest()
            {
                address1 = vAddressMatch.address1,
                address2 = vAddressMatch.address2,
            };
            string requestBody = JsonConvert.SerializeObject(req);
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v2/address" : "https://testapi.karza.in/v2/address";
            string pBranch = vAddressMatch.pBranch == null ? "" : vAddressMatch.pBranch;
            string pIdNo = vAddressMatch.pIdNo == null ? "" : vAddressMatch.pIdNo;
            string pEoID = vAddressMatch.pEoID == null ? "" : vAddressMatch.pEoID;
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                dynamic res = JsonConvert.DeserializeObject(fullResponse.Replace("status-code", "statusCode").Replace("match", "result"));
                try
                {
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse.Replace("status-code", "statusCode"), "root"));
                    SaveKarzaMatchingDtl("AddresssMatching", vXml, pBranch, pEoID, pIdNo);
                }
                finally
                {
                }

                if (res.statusCode == "101")
                {
                    return new MatchResponse(Math.Round(Convert.ToDouble(res.result.score) * 100, 2), Convert.ToBoolean(res.result.result), Convert.ToString(res.request_id), Convert.ToString(res.statusCode));
                }
                else
                {
                    return new MatchResponse(0.00, false, Convert.ToString(res.request_id), Convert.ToString(res.statusCode));
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
                SaveKarzaMatchingDtl("AddresssMatching", vXml, pBranch, pEoID, pIdNo);
                dynamic res = JsonConvert.DeserializeObject(Response.Replace("status-code", "statusCode").Replace("match", "result"));
                return new MatchResponse(0.00, false, Convert.ToString(res.request_id), Convert.ToString(res.statusCode));
            }
            finally
            {
                // streamWriter = null;
            }
        }
        #endregion

        #region FaceMatching
        public MatchResponse FaceMatching(FaceMatch vFaceMatch)
        {
            var req = new FaceMatch()
            {
                image1B64 = vFaceMatch.image1B64,
                image2B64 = vFaceMatch.image2B64
            };
            string requestBody = JsonConvert.SerializeObject(req);
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/facesimilarity" : "https://testapi.karza.in/v3/facesimilarity";
            string pBranch = vFaceMatch.pBranch == null ? "" : vFaceMatch.pBranch;
            string pIdNo = vFaceMatch.pIdNo == null ? "" : vFaceMatch.pIdNo;
            string pEoID = vFaceMatch.pEoID == null ? "" : vFaceMatch.pEoID;
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                    SaveKarzaMatchingDtl("FaceMatching", vXml, pBranch, pEoID, pIdNo);
                }
                finally
                {
                }

                if (res.statusCode == "101")
                {
                    return new MatchResponse(Math.Round(Convert.ToDouble(res.result.matchScore), 2), Convert.ToString(res.result.match) == "yes" ? true : false, Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
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
                SaveKarzaMatchingDtl("FaceMatching", vXml, pBranch, pEoID, pIdNo);
                dynamic res = JsonConvert.DeserializeObject(Response.Replace("status", "statusCode"));
                return new MatchResponse(0.00, false, Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
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
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/aadhaar-xml/otp" : "https://testapi.karza.in/v3/aadhaar-xml/otp";
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveKarzaAadhaarOtp("AadhaarXmlOtp", vXml, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
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
                    Response = Response.Replace("\u0000", "");
                    Response = Response.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    SaveKarzaAadhaarOtp("AadhaarXmlOtp", vXml, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
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

        #region AadhaarXmlDownload
        public string KarzaAadhaarXml(AadhaarXmlDownload aadhaarXmlDownload)
        {
            string requestBody = JsonConvert.SerializeObject(aadhaarXmlDownload);
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/aadhaar-xml/file" : "https://testapi.karza.in/v3/aadhaar-xml/file";
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
                request.Headers.Add("x-karza-key", vKarzaKey);//wdycvLFD27R0RuAn2guz               
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
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveKarzaAadhaarOtp("AadhaarXml", vXml, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
                }
                finally
                {
                }
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
                try
                {
                    Response = Response.Replace("\u0000", "");
                    Response = Response.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    SaveKarzaAadhaarOtp("AadhaarXml", vXml, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
                }
                finally
                {
                }
                return Response;
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
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/eaadhaar/otp" : "https://testapi.karza.in/v3/eaadhaar/otp";
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveKarzaAadhaarOtp("eAadhaarOtp", vXml, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
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
                    Response = Response.Replace("\u0000", "");
                    Response = Response.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    SaveKarzaAadhaarOtp("eAadhaarOtp", vXml, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
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
            string postURL = vKarzaEnv == "PROD" ? "https://api.karza.in/v3/eaadhaar/file" : "https://testapi.karza.in/v3/eaadhaar/file";
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
                request.Headers.Add("x-karza-key", vKarzaKey);
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
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveKarzaAadhaarOtp("eAadhaarXml", vXml, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
                }
                finally
                {
                }
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
                try
                {
                    Response = Response.Replace("\u0000", "");
                    Response = Response.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    SaveKarzaAadhaarOtp("eAadhaarXml", vXml, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
                }
                finally
                {
                }
                return Response;
            }
            finally
            {
            }
        }
        #endregion

        #endregion

        #region SaveOCRData
        public string SaveOCRData(OCRData vOCRData)
        {
            string xmlID1AadharFront, xmlID1AadharBack, xmlID1VoterFront, xmlID1VoterBack, xmlID1AadharResponse, xmlID1VoterResponse,
                xmlID2AadharFront, xmlID2AadharBack, xmlID2VoterFront, xmlID2VoterBack, xmlID2AadharResponse, xmlID2VoterResponse,
                xmlNameMatchingResponse, xmlAddressMatchingResponse, xmlFaceMatchingResponse, xmlNameMatchingResponseID2,
                xmlAddressMatchingResponseID2, xmlFaceMatchingResponseID2;

            string vID1AadharFront, vID1AadharBack, vID1VoterFront, vID1VoterBack, vID1AadharResponse, vID1VoterResponse,
                vID2AadharFront, vID2AadharBack, vID2VoterFront, vID2VoterBack, vID2AadharResponse, vID2VoterResponse;

            string vCoAppID1AadharFront, vCoAppID1AadharBack, vCoAppID1VoterFront, vCoAppID1VoterBack, vCoAppID1AadharResponse, vCoAppID1VoterResponse,
                xmlCoAppID1AadharFront, xmlCoAppID1AadharBack, xmlCoAppID1VoterFront, xmlCoAppID1VoterBack, xmlCoAppID1AadharResponse, xmlCoAppID1VoterResponse,
                xmlCoAppNameMatchingResponse, xmlCoAppAddressMatchingResponse;

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

                xmlNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.NameMatchingResponse == null ? "" : vOCRData.NameMatchingResponse, "root"));
                xmlAddressMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.AddressMatchingResponse == null ? "" : vOCRData.AddressMatchingResponse, "root"));
                xmlFaceMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.FaceMatchingResponse == null ? "" : vOCRData.FaceMatchingResponse, "root"));
                xmlNameMatchingResponseID2 = AsString(JsonConvert.DeserializeXmlNode(vOCRData.NameMatchingResponseID2 == null ? "" : vOCRData.NameMatchingResponseID2, "root"));
                xmlAddressMatchingResponseID2 = AsString(JsonConvert.DeserializeXmlNode(vOCRData.AddressMatchingResponseID2 == null ? "" : vOCRData.AddressMatchingResponseID2, "root"));
                xmlFaceMatchingResponseID2 = AsString(JsonConvert.DeserializeXmlNode(vOCRData.FaceMatchingResponseID2 == null ? "" : vOCRData.FaceMatchingResponseID2, "root"));


                vCoAppID1AadharFront = vOCRData.CoAppID1AadharFront == null ? "" : vOCRData.CoAppID1AadharFront.Replace("\u0000", "");
                vCoAppID1AadharFront = vCoAppID1AadharFront.Replace("\\u0000", "");
                xmlCoAppID1AadharFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoAppID1AadharFront), "root"));

                vCoAppID1AadharBack = vOCRData.CoAppID1AadharBack == null ? "" : vOCRData.CoAppID1AadharBack.Replace("\u0000", "");
                vCoAppID1AadharBack = vCoAppID1AadharBack.Replace("\\u0000", "");
                xmlCoAppID1AadharBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoAppID1AadharBack), "root"));

                vCoAppID1VoterFront = vOCRData.CoAppID1VoterFront == null ? "" : vOCRData.CoAppID1VoterFront.Replace("\u0000", "");
                vCoAppID1VoterFront = vCoAppID1VoterFront.Replace("\\u0000", "");
                xmlCoAppID1VoterFront = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoAppID1VoterFront), "root"));

                vCoAppID1VoterBack = vOCRData.CoAppID1VoterBack == null ? "" : vOCRData.CoAppID1VoterBack.Replace("\u0000", "");
                vCoAppID1VoterBack = vCoAppID1VoterBack.Replace("\\u0000", "");
                xmlCoAppID1VoterBack = AsString(JsonConvert.DeserializeXmlNode(GetSerializeJson(vCoAppID1VoterBack), "root"));

                vCoAppID1AadharResponse = vOCRData.CoAppID1AadharResponse == null ? "" : vOCRData.CoAppID1AadharResponse.Replace("\u0000", "");
                vCoAppID1AadharResponse = vCoAppID1AadharResponse.Replace("\\u0000", "");
                xmlCoAppID1AadharResponse = AsString(JsonConvert.DeserializeXmlNode(vCoAppID1AadharResponse, "root"));

                vCoAppID1VoterResponse = vOCRData.CoAppID1VoterResponse == null ? "" : vOCRData.CoAppID1VoterResponse.Replace("\u0000", "");
                vCoAppID1VoterResponse = vCoAppID1VoterResponse.Replace("\\u0000", "");
                xmlCoAppID1VoterResponse = AsString(JsonConvert.DeserializeXmlNode(vCoAppID1VoterResponse, "root"));

                xmlCoAppNameMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.CoAppNameMatchingResponse == null ? "" : vOCRData.CoAppNameMatchingResponse, "root"));
                xmlCoAppAddressMatchingResponse = AsString(JsonConvert.DeserializeXmlNode(vOCRData.CoAppAddressMatchingResponse == null ? "" : vOCRData.CoAppAddressMatchingResponse, "root"));
                ///---------------------------------------------------------------------------------------------------------------------------------------------------
                //try
                //{
                //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "OCR", vOCRData.EnquiryId));
                //    System.IO.Directory.CreateDirectory(folderPath);

                //    File.WriteAllText(folderPath + "/ID1AadharFront.xml", xmlID1AadharFront);
                //    File.WriteAllText(folderPath + "/ID1AadharBack.xml", xmlID1AadharBack);
                //    File.WriteAllText(folderPath + "/ID1AadharResponse.xml", xmlID1AadharResponse);

                //    File.WriteAllText(folderPath + "/ID1VoterFront.xml", xmlID1VoterFront);
                //    File.WriteAllText(folderPath + "/ID1VoterBack.xml", xmlID1VoterBack);
                //    File.WriteAllText(folderPath + "/ID1VoterResponse.xml", xmlID1VoterResponse);

                //    File.WriteAllText(folderPath + "/ID2AadharFront.xml", xmlID2AadharFront);
                //    File.WriteAllText(folderPath + "/ID2AadharBack.xml", xmlID2AadharBack);
                //    File.WriteAllText(folderPath + "/ID2AadharResponse.xml", xmlID2AadharResponse);

                //    File.WriteAllText(folderPath + "/ID2VoterFront.xml", xmlID2VoterFront);
                //    File.WriteAllText(folderPath + "/ID2VoterBack.xml", xmlID2VoterBack);
                //    File.WriteAllText(folderPath + "/ID2VoterResponse.xml", xmlID2VoterResponse);

                //    File.WriteAllText(folderPath + "/NameMatchingResponse.xml", xmlNameMatchingResponse);
                //    File.WriteAllText(folderPath + "/FaceMatchingResponse.xml", xmlFaceMatchingResponse);
                //    File.WriteAllText(folderPath + "/AddressMatchingResponse.xml", xmlAddressMatchingResponse);

                //    File.WriteAllText(folderPath + "/CoAppID1AadharFront.xml", xmlCoAppID1AadharFront);
                //    File.WriteAllText(folderPath + "/CoAppID1AadharBack.xml", xmlCoAppID1AadharBack);
                //    File.WriteAllText(folderPath + "/CoAppID1AadharResponse.xml", xmlCoAppID1AadharResponse);

                //    File.WriteAllText(folderPath + "/CoAppID1VoterFront.xml", xmlCoAppID1VoterFront);
                //    File.WriteAllText(folderPath + "/CoAppID1VoterBack.xml", xmlCoAppID1VoterBack);
                //    File.WriteAllText(folderPath + "/CoAppID1VoterResponse.xml", xmlCoAppID1VoterResponse);

                //    File.WriteAllText(folderPath + "/CoAppNameMatchingResponse.xml", xmlCoAppNameMatchingResponse);
                //    File.WriteAllText(folderPath + "/CoAppAddressMatchingResponse.xml", xmlCoAppAddressMatchingResponse);

                //}
                //finally
                //{
                //}
                //------------------------------------------------------------------------
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveOCRData";

                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEnquiryId", vOCRData.EnquiryId == null ? "" : vOCRData.EnquiryId);
                if (xmlID1AadharFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1AadharFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1AadharFront.Length + 1, "@pID1AadharFront", xmlID1AadharFront);

                if (xmlID1AadharBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1AadharBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1AadharBack.Length + 1, "@pID1AadharBack", xmlID1AadharBack);

                if (xmlID1VoterFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1VoterFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1VoterFront.Length + 1, "@pID1VoterFront", xmlID1VoterFront);

                if (xmlID1VoterBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1VoterBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1VoterBack.Length + 1, "@pID1VoterBack", xmlID1VoterBack);

                if (xmlID1AadharResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1AadharResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1AadharResponse.Length + 1, "@pID1AadharResponse", xmlID1AadharResponse);

                if (xmlID1VoterResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID1VoterResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID1VoterResponse.Length + 1, "@pID1VoterResponse", xmlID1VoterResponse);

                if (xmlID2AadharFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2AadharFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2AadharFront.Length + 1, "@pID2AadharFront", xmlID2AadharFront);

                if (xmlID2AadharBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2AadharBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2AadharBack.Length + 1, "@pID2AadharBack", xmlID2AadharBack);
                if (xmlID2VoterFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2VoterFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2VoterFront.Length + 1, "@pID2VoterFront", xmlID2VoterFront);

                if (xmlID2VoterBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2VoterBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2VoterBack.Length + 1, "@pID2VoterBack", xmlID2VoterBack);

                if (xmlID2AadharResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2AadharResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2AadharResponse.Length + 1, "@pID2AadharResponse", xmlID2AadharResponse);

                if (xmlID2VoterResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pID2VoterResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlID2VoterResponse.Length + 1, "@pID2VoterResponse", xmlID2VoterResponse);

                if (xmlNameMatchingResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pNameMatchingResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlNameMatchingResponse.Length + 1, "@pNameMatchingResponse", xmlNameMatchingResponse);

                if (xmlAddressMatchingResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pAddressMatchingResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlAddressMatchingResponse.Length + 1, "@pAddressMatchingResponse", xmlAddressMatchingResponse);

                if (xmlFaceMatchingResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pFaceMatchingResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlFaceMatchingResponse.Length + 1, "@pFaceMatchingResponse", xmlFaceMatchingResponse);

                if (xmlNameMatchingResponseID2 == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pNameMatchingResponseID2", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlNameMatchingResponseID2.Length + 1, "@pNameMatchingResponseID2", xmlNameMatchingResponseID2);

                if (xmlAddressMatchingResponseID2 == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pAddressMatchingResponseID2", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlAddressMatchingResponseID2.Length + 1, "@pAddressMatchingResponseID2", xmlAddressMatchingResponseID2);

                if (xmlFaceMatchingResponseID2 == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pFaceMatchingResponseID2", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlFaceMatchingResponseID2.Length + 1, "@pFaceMatchingResponseID2", xmlFaceMatchingResponseID2);

                //---------------------------------------------------------------------Co-Applicant-----------------------------------------------------------
                if (xmlCoAppID1AadharFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1AadharFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1AadharFront.Length + 1, "@pCoAppID1AadharFront", xmlCoAppID1AadharFront);

                if (xmlCoAppID1AadharBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1AadharBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1AadharBack.Length + 1, "@pCoAppID1AadharBack", xmlCoAppID1AadharBack);

                if (xmlCoAppID1VoterFront == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1VoterFront", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1VoterFront.Length + 1, "@pCoAppID1VoterFront", xmlCoAppID1VoterFront);

                if (xmlCoAppID1VoterBack == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1VoterBack", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1VoterBack.Length + 1, "@pCoAppID1VoterBack", xmlCoAppID1VoterBack);

                if (xmlCoAppID1AadharResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1AadharResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1AadharResponse.Length + 1, "@pCoAppID1AadharResponse", xmlCoAppID1AadharResponse);

                if (xmlCoAppID1VoterResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppID1VoterResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppID1VoterResponse.Length + 1, "@pCoAppID1VoterResponse", xmlCoAppID1VoterResponse);


                if (xmlCoAppNameMatchingResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppNameMatchingResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppNameMatchingResponse.Length + 1, "@pCoAppNameMatchingResponse", xmlCoAppNameMatchingResponse);

                if (xmlCoAppAddressMatchingResponse == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pCoAppAddressMatchingResponse", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, xmlCoAppAddressMatchingResponse.Length + 1, "@pCoAppAddressMatchingResponse", xmlCoAppAddressMatchingResponse);
                //-----------------------------------------------------------------------------------------------------------------------------------------

                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        private DataTable CreateDataTableXML(string xmlFile)
        {
            DataTable dt = new DataTable();
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlFile);
                XmlNode nodoEstructura = xml.DocumentElement.ChildNodes.Cast<XmlNode>().ToList()[0];
                foreach (XmlNode columna in nodoEstructura.ChildNodes)
                {
                    dt.Columns.Add(columna.Name, typeof(String));
                }
                XmlNode filas = xml.DocumentElement;
                foreach (XmlNode fila in filas.ChildNodes)
                {
                    dt.Rows.Add(fila["AGGRID"].InnerText, fila["AGGRNAME"].InnerText, fila["CORPID"].InnerText, fila["USERID"].InnerText,
                        fila["URN"].InnerText, fila["DEBITACC"].InnerText, fila["CREDITACC"].InnerText, fila["IFSC"].InnerText,
                        fila["AMOUNT"].InnerText, fila["CURRENCY"].InnerText, fila["TXNTYPE"].InnerText, fila["PAYEENAME"].InnerText,
                        fila["UNIQUEID"].InnerText, fila["REMARKS"].InnerText);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMobileNo", vMobNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", vEoid);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", vBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, vResponseData.Length + 1, "@pResponseData", vResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pStatusCode", vStatusCode);
                DBConnect.Execute(oCmd);
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

        #region ActivityTrack
        public List<EoData> PopROForActivity(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<EoData> row = new List<EoData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopROForActivity";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pLogDt", postKYCData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new EoData(rs["Eoid"].ToString(), rs["EOName"].ToString()));
                    }
                }
                else
                {
                    row.Add(new EoData("No data available", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new EoData("No data available", ex.ToString()));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<ActivityData> PopActivityData(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<ActivityData> row = new List<ActivityData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "PopActivityData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoId", postKYCData.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postKYCData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Date, 10, "@pLogDt", postKYCData.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new ActivityData(rs["Eoid"].ToString(), rs["EOName"].ToString(), rs["MarketID"].ToString(), rs["Market"].ToString(), rs["Groupid"].ToString()
                            , rs["GroupName"].ToString(), rs["MemberID"].ToString(), rs["MemberName"].ToString(), rs["NomName"].ToString(), rs["LoanNo"].ToString(), rs["LoanAmt"].ToString(),
                            rs["Outstanding"].ToString(), rs["PrinCollAmt"].ToString(), rs["PrinODAmt"].ToString(), rs["NOD"].ToString(), rs["DeathDate"].ToString(), rs["WriteOffAmt"].ToString()));
                    }
                }
                else
                {
                    row.Add(new ActivityData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new ActivityData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string SaveActivityData(PostActivityData postActivityData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pErrDesc;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_SaveActivityData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", postActivityData.Eoid);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pActivityID", postActivityData.ActivityID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 12, "@pActivityDate", postActivityData.ActivityDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 12, "@pActivityTime", postActivityData.ActivityTime);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLongitude", postActivityData.Longitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pLattitude", postActivityData.Lattitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pIMEINo", postActivityData.IMEINo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, postActivityData.LocationAddress.Length + 1, "@pLocationAddress", postActivityData.LocationAddress);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", postActivityData.MemberID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRemarks", postActivityData.Remarks);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", postActivityData.BranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "M");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pGroupId", postActivityData.GroupID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMarketID", postActivityData.CenterID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pOpType", postActivityData.ActivityType);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }
        #endregion

        #region Check D-Dup MEL
        public Int32 ChkDdupMEL(string pIdProofNo, string pAddressProofNo, string pAddressProofNo2, ref string pErrDesc)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkDdupMEL";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pIdProofNo", pIdProofNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo", pAddressProofNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pAddressProofNo2", pAddressProofNo2);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 60, "@pErrDesc", pErrDesc);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);

                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
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
        #endregion,

        #region SaveKarzaVoterVerifyData
        public Int32 SaveKarzaVoterVerifyData(string pVoterId, string pResponseXml, string pBranchCode, string pEoid)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaVoterVerifyData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pVoterId", pVoterId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pEoid);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region SaveKarzaAadharToken
        public Int32 SaveKarzaAadharToken(string pAadharNo, string pTokenNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaAadharToken";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pAadharNo", pAadharNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTokenNo", pTokenNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
            //try
            //{
            //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "OCR", pAadharNo)); ;
            //    System.IO.Directory.CreateDirectory(folderPath);
            //    File.WriteAllText(folderPath + "/AadharResponse.txt", pResponseData);
            //}
            //finally
            //{
            //}
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pAadharNo", pAadharNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region SaveKarzaMatchingDtl
        public Int32 SaveKarzaMatchingDtl(string pApiName, string pResponseXml, string pBranch, string pEoID, string pIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaMatchingDtl";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region SaveKarzaAadhaarOtp
        public Int32 SaveKarzaAadhaarOtp(string pApiName, string pResponseXml, string pBranch, string pEoID, string pIdNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveKarzaAadhaarOtp";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseXml.Length + 1, "@pResponseXml", pResponseXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pVoterId", pVoterId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 3, "@pErrorCode", pErrorCode);
                DBConnect.Execute(oCmd);
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

        #region GetKarzaVoterErrCount
        public Int32 GetKarzaVoterErrCount(string pVoterId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetKarzaVoterErrCount";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 25, "@pVoterId", pVoterId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 3, "@pCount", vErr);
                DBConnect.Execute(oCmd);
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

        #region SaveIciciBankLog
        public Int32 SaveIciciBankLog(string pReferenceNo, string pRequestData, string pResponseData, string pEndPoint, string pRequestJson, string pResponseJson)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveIciciBankLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pReferenceNo", pReferenceNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pRequestData.Length + 1, "@pRequestData", pRequestData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pEnd_Point", pEndPoint);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pRequestJson.Length + 1, "@pRequestJson", pRequestJson);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseJson.Length + 1, "@pResponseJson", pResponseJson);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region SaveJocataLog
        public string SaveJocataLog(string pMemberId, Int32 pCGTId, string pResponseData, string pScreeningID)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningID", pScreeningID);
                DBConnect.Execute(oCmd);
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

        #region SaveJocataRiskCategoryLog
        public string SaveJocataRiskCategoryLog(string pMemberId, Int32 pCGTId, string pResponseData, string pRiskCategory)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataRiskCategoryLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRiskCategory", pRiskCategory);
                DBConnect.Execute(oCmd);
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

        #region RejectGRT
        public string RejectGRT(string pAppDate, string pMemberId, string pCGTID, string pBrCode, string pEoid, string pAppStatus, string pRejectReason, string pCreatedBy, string pScreeningID)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string vDigiConcentSMS = "", vDigiConcentSMSTemplateId = "", vDigiConcentSMSLanguage = "", vResultSendDigitalConcentSMS = "", vpMobileNo = "";
            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "RejectGRT";
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pAppDate", pAppDate);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", pMemberId);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTID", Convert.ToInt32(pCGTID));
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", pEoid);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pAppStatus", pAppStatus);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRejectReason", pRejectReason);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningID", "");

            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pMobileNo", vpMobileNo);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.NVarChar, 1000, "@pDigiConcentSMS", vDigiConcentSMS);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSTemplateId", vDigiConcentSMSTemplateId);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 100, "@pDigiConcentSMSLanguage", vDigiConcentSMSLanguage);
            DBConnect.Execute(oCmd);
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
                        vResultSendDigitalConcentSMS = SendDigitalConcentSMS(vpMobileNo, vDigiConcentSMS, vDigiConcentSMSTemplateId, vDigiConcentSMSLanguage);
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

        #region UpdateJocataStatus
        public string UpdateJocataStatus(string pMemberId, Int32 pCGTId, string pScreeningID, string pStatus, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateJocataStatus";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pScreeningId", pScreeningID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pStatus", pStatus);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 1, "@pCreatedBy", pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.Int, 1, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region GenerateOCRLogReport
        public Int32 GenerateOCRLogReport(string pFromDt, string pToDt, string pFormat, string pUserId)
        {
            string vFromDt, vToDt = "";
            //vFromDt = pFromDt.Substring(3, 2) + "/" + pFromDt.Substring(0, 2) + "/" + pFromDt.Substring(6, 4);
            //vToDt = pToDt.Substring(3, 2) + "/" + pToDt.Substring(0, 2) + "/" + pToDt.Substring(6, 4);
            vFromDt = pFromDt;
            vToDt = pToDt;
            SqlCommand oCmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "RptOCRLog";
                oCmd.CommandTimeout = 80000;
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pFromDt", vFromDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pToDt", vToDt);
                DBConnect.ExecuteForSelect(oCmd, dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            string vFolderPath = "C:\\BijliReport\\" + pUserId;
            if (!System.IO.Directory.Exists(vFolderPath))
            {
                Directory.CreateDirectory(vFolderPath);
            }

            if (pFormat == "Excel")
            {
                Excel(dt, vFolderPath);
            }
            else
            {
                CSV(dt, vFolderPath);
            }
            return 1;
        }
        #endregion

        #region CSV&Excel
        private void CSV(DataTable dt, string vFolderPath)
        {

            string vFileNm = "";
            vFileNm = vFolderPath + "\\OCR_Log_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";

            try
            {
                StreamWriter sw = new StreamWriter(vFileNm, false);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sw.Write(dt.Columns[i]);
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dt.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
                // Write(dt, vFileNm);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        private void Excel(DataTable dt, string vFolderPath)
        {
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
            string vFileNm = "";
            vFileNm = vFolderPath + "\\OCR_Log_Report_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table border='0' cellpadding='5' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='5'>" + "" + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>" + "" + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><u><font size='3'>OCR Report " + "" + "</font></u></b></td></tr>");
            DataGrid1.RenderControl(htw);
            htw.WriteLine("</td></tr>");
            htw.WriteLine("<tr><td colspan='3'><b><u><font size='3'></font></u></b></td></tr>");
            htw.WriteLine("</table>");
            File.WriteAllText(vFileNm, sw.ToString());

        }
        #endregion

        #region ICICDeCrypt
        public ICICBalanceFetchResponse ICICDeCrypt(string strAPIResponse)
        {

            string strDeryptedData = string.Empty;
            ICICBalanceFetchResponse objResponse = new ICICBalanceFetchResponse("", "", "", "", "", "", "", "", "", "");
            try
            {
                strDeryptedData = DataDeCrypt(strAPIResponse);
                objResponse = JsonConvert.DeserializeObject<ICICBalanceFetchResponse>(strDeryptedData);
            }
            catch (Exception ex)
            {
                ICICBalanceFetchResponse objResponseError = new ICICBalanceFetchResponse("Error", ex.ToString(), "", "", "", "", "", "", strDeryptedData, strAPIResponse);
                return objResponseError;
            }
            finally
            {

            }
            return objResponse;
        }
        #endregion

        #region Other Member CB
        private string ProcessOtherMemCB(string pEnqId, Int32 pCbId, string pBranchCode, Int32 pCreatedBy, string pDate
            , string CCRUserName, string CCRPassword, string pEoId, string pAadhaarNo)
        {
            WebServiceSoapClient eq = new WebServiceSoapClient();
            string pEqXml = string.Empty, equifaxResponse = string.Empty, pErrDesc = string.Empty;
            string pMemCategory = string.Empty, vMemTag = string.Empty;
            Int32 vErr2 = 0, vCbIdOtherMem = 0;
            Int32 pSlNo = 0;
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            SqlCommand oCmd2 = new SqlCommand();

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Mob_GetInitialApprOthMemDtlForCB";
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
            DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
            DBConnect.ExecuteForSelect(oCmd, dt);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow rs in dt.Rows)
                {
                    pMemCategory = rs["MemCategory"].ToString();
                    pCbId = Convert.ToInt32(rs["CBID"].ToString());
                    pSlNo = Convert.ToInt32(rs["SlNo"].ToString());

                    if (pMemCategory == "CoAppMem") vMemTag = "Co-Applicant";
                    if (pMemCategory == "OtherMem") vMemTag = "Other Earning Member";

                    pEqXml = eq.Equifax(
                    rs["FirstName"].ToString(), rs["MiddleName"].ToString(), rs["LastName"].ToString(), rs["DOB"].ToString()
                    , rs["AddressType"].ToString(), rs["Address"].ToString(), rs["StateName"].ToString(), rs["PinCode"].ToString(), rs["MobileNo"].ToString()
                    , rs["IDType1"].ToString(), rs["IDType1"].ToString() == "AADHAAR" && pMemCategory == "CoAppMem" ? pAadhaarNo : rs["IDNo1"].ToString()
                    , rs["IDType2"].ToString(), rs["IDType2"].ToString() == "AADHAAR" && pMemCategory == "CoAppMem" ? pAadhaarNo : rs["IDNo2"].ToString()
                    , rs["RelType"].ToString(), rs["RelName"].ToString(), "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

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
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEnqId", pEnqId);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCbId", pCbId);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Xml, pEqXml.Length + 1, "@pEquifaxXML", pEqXml);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBranchCode);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pEoid", pEoId);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", pDate);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", "P");
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1000, "@pErrorMsg", "");
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pStatus", 0);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 255, "@pStatusDesc", "");
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@MemCategory", pMemCategory);
                        DBConnect.AddParameter(oCmd2.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pSlNo", pSlNo);
                        DBConnect.Execute(oCmd2);
                        vErr2 = Convert.ToInt32(oCmd2.Parameters["@pStatus"].Value);
                        equifaxResponse = Convert.ToString(oCmd2.Parameters["@pStatusDesc"].Value);
                        //if (vErr2 == 1)
                        //{
                        //    //return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Successful" + ":" + equifaxResponse + ":" + pCbId;
                        //    return "Successful";
                        //}
                        //else if (vErr2 == 5)
                        //{
                        //    return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed for "+ vMemTag + " " + ":" + equifaxResponse + ":" + pCbId;
                        //}
                        //else
                        //{
                        //    return pErrDesc + ":" + pEnqId + ":" + "Equifax Verification Failed for "+ vMemTag + " " + ":" + equifaxResponse + ":" + pCbId;
                        //}
                    }
                }
                return "Successful";
            }
            else
            {
                return "Successful";
            }
            //return "Successful";
        }
        #endregion

        #region Jocata Integration

        public DataTable GetJocataRequestData(string pMemberId)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetJocataRequestData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        public string GetJokataToken()
        {
            string postURL = vKarzaEnv == "PROD" ? "https://aml.unitybank.co.in/ramp/webservices/createToken" : "https://usfbamluat.unitybank.co.in/ramp/webservices/createToken";
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
            string vJokataToken = vJocataToken, vRampResponse = "";
            try
            {
                //-----------------------Ramp Request------------------------
                string postURL = vKarzaEnv == "PROD" ? "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request" : "https://usfbamluat.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
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

        public RampStatus RampStatus(RampStatusRequest vRampStatusRequest)
        {
            string vScreeningId = "", vBuCode = "", vSubBuCode = "", vScreeningStatus = "", vRemarks = "";
            RampStatus vRampStatus = new RampStatus("500", "Fail", "Status not Updated Successfully.", vScreeningId);
            if (vRampStatusRequest != null)
            {
                vScreeningId = vRampStatusRequest.screeningId;
                vBuCode = vRampStatusRequest.buCode;
                vSubBuCode = vRampStatusRequest.subBuCode;
                vScreeningStatus = vRampStatusRequest.screeningStatus;
                vRemarks = vRampStatusRequest.remarks;

                IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
                WebHeaderCollection headers = request.Headers;
                string vUserName = "", vPassword = "";
                foreach (string headerName in headers.AllKeys)
                {
                    if (headerName == "UserID")
                    {
                        vUserName = headers[headerName];
                    }
                    if (headerName == "Password")
                    {
                        vPassword = headers[headerName];
                    }
                }
                if (vUserName == "" || vUserName != "JOCATA")
                {
                    vRampStatus = new RampStatus("500", "Fail", "Status not Updated Successfully.", vScreeningId);
                }
                else if (vPassword == "" || vPassword != "Jocata#12345!")
                {
                    vRampStatus = new RampStatus("500", "Fail", "Status not Updated Successfully.", vScreeningId);
                }
                else
                {
                    int vErr = SaveJocataStatus(vBuCode, vSubBuCode, vScreeningId, vScreeningStatus, vRemarks, "");
                    if (vErr == 0)
                    {
                        vRampStatus = new RampStatus("200", "Success", "Status Updated Successfully.", vScreeningId);
                    }
                    else
                    {
                        vRampStatus = new RampStatus("500", "Fail", "Status not Updated Successfully.", vScreeningId);
                    }
                }
            }
            return vRampStatus;
        }

        public Int32 SaveJocataStatus(string pBuCode, string pSubBuCode, string pScreeningId, string pScreeningStatus, string pRemarks, string pUniqueId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataStatus";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pBuCode", pBuCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pSubBuCode", pSubBuCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pScreeningId", pScreeningId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pScreeningStatus", pScreeningStatus);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pRemarks", pRemarks);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 150, "@pUniqueId", pUniqueId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        public string JocataCalling(string pMemberID, string pCreatedBy, string pCGTId)
        {
            string vMsg = "", vResponseXml = "", vResponseData = "", vScreeningId = "";
            DataTable dt = null;
            try
            {
                dt = new DataTable();
                dt = GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    #region JocataRequest
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
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vMsg = "True match member.";
                            vStatus = "N";
                        }
                        else
                        {
                            vMsg = "False match member.";
                            vStatus = "P";
                            UpdateJocataStatus(pMemberID, Convert.ToInt32(pCGTId), vScreeningId, vStatus, Convert.ToInt32(pCreatedBy));
                            try
                            {
                                ProsiReq pReq = new ProsiReq();
                                pReq.pMemberId = pMemberID;
                                pReq.pCreatedBy = pCreatedBy;
                                pReq.pCGTId = pCGTId;
                                //Prosidex(pReq);
                                PosidexEncryption(pReq);
                            }
                            finally { }
                        }
                    }
                    else
                    {
                        vMsg = "Problem in Jocata Request Data.";
                    }
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    SaveJocataLog(pMemberID, Convert.ToInt32(pCGTId), vResponseXml, vScreeningId);
                }
            }
            catch
            {
                vMsg = "Unable to connect Jocata API.";
                vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                SaveJocataLog(pMemberID, Convert.ToInt32(pCGTId), vResponseXml, vScreeningId);
                UpdateJocataStatus(pMemberID, Convert.ToInt32(pCGTId), vScreeningId, "U", Convert.ToInt32(pCreatedBy));
            }
            return vMsg;
        }

        public string JocataRiskCat(RiskCatReq vRiskCatReq)
        {
            string vJokataToken = vJocataToken, vRiskCategory = "", vRampResponse = "", vResponseXml = "", pCGTId = "", pMemberID = "";
            try
            {
                //------------------------------------URI----------------------------------------------
                string postURL = vKarzaEnv == "PROD" ? "https://aml.unitybank.co.in/orck/on-boarding/calculate-risk"
                    : "https://jocatauat.unitybank.co.in/orck/on-boarding/calculate-risk";
                string vToken = vKarzaEnv == "PROD" ? "611d9587-7546-8e62-1809-c8f8c193d421" : "596cf388-a04f-53b1-ce6b-e9a54082363f";
                //------------------------------------------------------------------------------------
                pMemberID = vRiskCatReq.memberId; pCGTId = vRiskCatReq.cgtId;
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
                SaveJocataRiskCategoryLog(pMemberID, Convert.ToInt32(pCGTId), vResponseXml, vRiskCategory);
                return vRiskCategory;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                    vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vRampResponse, "root"));
                    SaveJocataRiskCategoryLog(pMemberID, Convert.ToInt32(pCGTId), vResponseXml, vRiskCategory);
                }
            }
            finally
            {
            }
            return vRampResponse;
        }

        #endregion

        #region SaveJocataToken
        public Int32 SaveJocataToken(string pMemberId, string pTokenNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveJocataToken";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pTokenNo", pTokenNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region  Send SMS
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

        #region AadhaarVault

        public string AadharVaultSignRequest(string text)
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
            string postURL = vKarzaEnv == "PROD" ? "https://avault.unitybank.co.in/vault/insert" : "https://avaultuat.unitybank.co.in/vault/insert";
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
                string requestBody = JsonConvert.SerializeObject(AadhaarVault);


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

        public AadhaarNoByRefId GetAadhaarNoByRefId(AadhaarNoReq pAadhaarNoReq)
        {
            string responsedata = "";
            try
            {
                string vPostData = "refId=" + pAadhaarNoReq.refId;
                string postURL = vKarzaEnv == "PROD" ? "https://avault.unitybank.co.in/vault/get-by-refid?" + vPostData : "https://avaultuat.unitybank.co.in/vault/get-by-refid?" + vPostData;
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
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(responsedata, "root"));
                SaveAadhaarVaultLog("", 1, vResponseXml, "");
                //-------------------------------------------------------------------------------------
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

                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(responsedata, "root"));
                SaveAadhaarVaultLog("", 1, vResponseXml, "");
                //-------------------------------------------------------------------------------------
                AadhaarNoByRefId myDeserializedClass = JsonConvert.DeserializeObject<AadhaarNoByRefId>(responsedata);
                return myDeserializedClass;
            }
        }

        public string AadhaarVaultRefData(AadhaarVault AadhaarVault)
        {
            string postURL = vKarzaEnv == "PROD" ? "https://avault.unitybank.co.in/vault/get-by-refData" : "https://avaultuat.unitybank.co.in/vault/get-by-refData";
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

                return fullResponse;
            }
            catch (WebException ex)
            {
                string Response = "";
                using (var stream = ex.Response.GetResponseStream())
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pAadhaarNo", pAadhaarNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMobileNo", pMobileNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBConnect.Execute(oCmd);
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
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer" : "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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

                //try
                //{
                //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "Prosidex", prosidexRequest.Request.DG.APPLICATIONID));
                //    System.IO.Directory.CreateDirectory(folderPath);
                //    Guid guid = Guid.NewGuid();
                //    File.WriteAllText(folderPath + "/" + Convert.ToString(guid) + ".text", vResponse);
                //}
                //finally { }
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                string vResponseCode = res.Response.StatusInfo.ResponseCode;
                if (vResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
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
            List<ACE> oACE = new List<ACE>();
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
                //--------------------Address Part----------------------------
                oACE.Add(new ACE(RemoveSpecialCharecters(dt.Rows[0]["ADDRESS"].ToString()),
                    dt.Rows[0]["ADDRESS_TYPE_FLAG"].ToString(),
                    dt.Rows[0]["COUNTRY"].ToString(),
                    RemoveSpecialCharecters(dt.Rows[0]["City"].ToString()),
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
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            //if (pResponseData.response_code != 200 || pResponseData.response_code != 300)
            //{
            //    pResponseData = ProsidexSearchCustomer(pReqData);
            //}
            return pResponseData;
        }

        public string getUcic(ProsiReq pProsiReq)
        {
            string vResponse = "", vUcic = "";
            try
            {
                string Requestdata = "{\"cust_id\" :" + "\"" + pProsiReq.pMemberId + "\"" + ",\"source_system_name\":\"BIJLI\"}";
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic" : "http://144.24.116.182:9002/UnitySfbWS/getUcic";
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
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                SaveProsidexLogUCIC(pProsiReq.pMemberId, Convert.ToInt32(pProsiReq.pCGTId), vResponseXml, Convert.ToInt32(pProsiReq.pCreatedBy), vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                SaveProsidexLogUCIC(pProsiReq.pMemberId, Convert.ToInt32(pProsiReq.pCGTId), vResponseXml, Convert.ToInt32(pProsiReq.pCreatedBy), vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }

        public ProsidexResponse getResponse(GetResponseReq req)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "";
            Int32 vCreatedBy = 1, vCGTID = 0;
            ProsidexResponse oProsidexResponse = null;
            try
            {
                vRequestId = req.pRequestId;
                vMemberId = req.pMemberId;
                vCGTID = Convert.ToInt32(req.pCGTId);
                string Requestdata = "{\"UnitySfb_RequestId\" :" + "\"" + req.pRequestId + "\"}";
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9002/UnitySfbWS/getResponse" : "http://144.24.116.182:9002/UnitySfbWS/getResponse";
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
                string vResponseCode = res.Response.StatusInfo.ResponseCode;
                if (vResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
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

        public PosidexVerifyResponse PosidexVerification(PosidexVerificationData pProsiVerifyReq)
        {
            PosidexVerifyResponse oPr = new PosidexVerifyResponse(null);
            if (pProsiVerifyReq != null)
            {
                string vRequestXml = ToXML(pProsiVerifyReq).Replace("<?xml version=\"1.0\"?>", "");
                List<Response> row = new List<Response>();
                DataTable dt = new DataTable();
                dt = UpdatePosidex(vRequestXml);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new Response(rs["UnitySfb_RequestId"].ToString(), rs["CUSTOMER_ID"].ToString(), rs["SourceSystem"].ToString(), rs["Stat"].ToString(), rs["Msg"].ToString(), rs["UCIC"].ToString()));
                    }
                }

                //row.Add(new Response(pProsiVerifyReq.Metadata[0].UnitySfb_RequestId, "1", "1", "1", "1", "1"));
                //row.Add(new Response(pProsiVerifyReq.Metadata[1].UnitySfb_RequestId, "2", "2", "2", "2", "2"));

                oPr.Response = row;
            }
            return oPr;
        }

        public string ToXML(Object oObject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(oObject.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject, ns);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pRequestId", pRequestId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                if (pUCIC_ID == null)
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", DBNull.Value);
                else
                    DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pPotentialYN", pPotentialYN);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 500, "@pPotenURL", pPotenURL);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pUCIC_ID", pUCIC_ID);
                DBConnect.Execute(oCmd);
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

        public DataTable UpdatePosidex(string pXmlData)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdatePosidex";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        #region GetProsidexReqData
        public DataTable GetProsidexReqData(string pMemberId, string pCreatedBy)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetProsidexReqData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberID", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        #region BankAccountVerification

        public Int32 ChkAccNo(string pMemberId, string pAccNo)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "ChkAccNo";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pAccNo", pAccNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
            string vLoginId = "unity", vPin = "81dc9bdb52d04dc20036dbd8313ed055", vResponse = "", vMemberId = "", vAccNo = "";
            int vCGTID = 0, vCreatedBy = 0, vErr = 0;
            //-----------------------------------------
            vCGTID = Convert.ToInt32(req.CGTId);
            vMemberId = req.MemberId;
            vCreatedBy = Convert.ToInt32(req.CreatedBy);
            vAccNo = req.beneAccNo;
            //-----------------------------------------
            FingPayResponse vResp = null;
            vErr = ChkAccNo(vMemberId, vAccNo);
            if (vErr > 0)
            {
                vResp = new FingPayResponse(false, "Account no already exist with another member.", 500, "", "");
            }
            else
            {
                ImpsBeneDetailsRequestDataModel DM = new ImpsBeneDetailsRequestDataModel();
                DM.beneAccNo = vAccNo;
                DM.beneIFSC = req.beneIFSC;
                BankACReqData BankACReqData = new BankACReqData();
                BankACReqData.requestId = req.CGTId;
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
                    request.Timeout = 300000;
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(vRequestData);
                        streamWriter.Close();
                    }
                    StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                    vResponse = responseReader.ReadToEnd();
                    request.GetResponse().Close();
                    //-----------------------------------DB--------------------------
                    //vResponse = "{\"apiStatus\":\"true\",\"apiStatusMessage\":\"successful\",\"data\":[{\"beneAccNo\":\"059601000071602\",\"beneIfscCode\":\"IOBA0000596\",\"timestamp\":\"19/12/2024 11:52:27\",\"statusCode\":\"30\",\"rrn\":\"435411589408\",\"beneName\":\"\",\"errorResponse\":\"No transaction response from NPCI\",\"requestId\":\"4951754\",\"referrenceNo\":\"GV29114171912202411520027\"}],\"apiStatusCode\":\"0\"}";
                    dynamic res = JsonConvert.DeserializeObject(vResponse);
                    int apiStatusCode = Convert.ToInt32(res.apiStatusCode);
                    bool apiStatus = res.apiStatus;
                    string apiStatusMessage = "Technical failure.";
                    apiStatusMessage = Convert.ToString(res.data[0].errorResponse == null ? "Technical failure." : res.data[0].errorResponse);
                    if (apiStatusCode == 0 && apiStatus == true)
                    {
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
                    SaveFingPayLog(vMemberId, vCGTID, vRequestXml, vResponseXml, vCreatedBy);
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
                    SaveFingPayLog(vMemberId, vCGTID, vRequestXml, vResponseXml, vCreatedBy);
                    //--------------------------------------------------------------------------------------
                }
            }
            return vResp;
        }

        #endregion

        #region SaveFingPayLog
        public string SaveFingPayLog(string pMemberId, Int32 pCGTId, string pRequestData, string pResponseData, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            string vStatusCode = "Save Successfully.";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveFingPayLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pMemberId", pMemberId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCGTId", pCGTId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pRequestData.Length + 1, "@pRequestXml", pRequestData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseData.Length + 1, "@pResponseXml", pResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", pCreatedBy);
                DBConnect.Execute(oCmd);
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

        #region MonitoringTools
        public List<GetMonitoringQuestion> MonitoringQuestion(string pVisitType)
        {
            DataSet ds = null;
            List<GetMonitoringQuestion> row = new List<GetMonitoringQuestion>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_GetInspectionList";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 2, "@pInspType", "MN");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pVisitType", pVisitType);
                ds = DBConnect.GetDataSet(oCmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow rs in ds.Tables[0].Rows)
                    {
                        row.Add(new GetMonitoringQuestion(
                            Convert.ToString(rs["ItemId"]), Convert.ToString(rs["ItemName"]),
                            Convert.ToString(rs["ItemTypeId"]), Convert.ToString(rs["ItemTypeName"]),
                            Convert.ToString(rs["QID"]), Convert.ToString(rs["Question"]),
                            Convert.ToString(rs["Block"])));
                    }
                }
                else
                {
                    row.Add(new GetMonitoringQuestion("No data available", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetMonitoringQuestion("No data available", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string SaveMonitoring(PostSaveMonitoring postSaveMonitoring)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0, pInspID = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_SaveMonitor";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.BigInt, 4, "@pInspID", pInspID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pInspType", postSaveMonitoring.pInspType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pSubDt", postSaveMonitoring.pSubDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCrdFrmDt", postSaveMonitoring.pCrdFrmDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCrdToDt", postSaveMonitoring.pCrdToDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postSaveMonitoring.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLat", postSaveMonitoring.pLatitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLong", postSaveMonitoring.pLongitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pGeoAddress", postSaveMonitoring.pGeoAddress);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, postSaveMonitoring.pXmlData.Length + 1, "@pXmlData", postSaveMonitoring.pXmlData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", postSaveMonitoring.pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", postSaveMonitoring.pMode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pVisitType", postSaveMonitoring.pVisitType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOid", postSaveMonitoring.pEOid);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pCenterid", postSaveMonitoring.pCenterid);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pMemberId", postSaveMonitoring.pMemberId);
                DBConnect.Execute(oCmd);
                pInspID = Convert.ToInt32(oCmd.Parameters["@pInspID"].Value);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                if (vErr == 0)
                    return "Record Saved Succesfully";
                else
                    return "Data Not Saved.";
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

        public string SaveMonitoringOD(PostSaveMonitoring postSaveMonitoring)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0, pInspID = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_SaveMonitorOD";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.BigInt, 4, "@pInspID", pInspID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pInspType", postSaveMonitoring.pInspType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pSubDt", postSaveMonitoring.pSubDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCrdFrmDt", postSaveMonitoring.pCrdFrmDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCrdToDt", postSaveMonitoring.pCrdToDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postSaveMonitoring.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLat", postSaveMonitoring.pLatitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pLong", postSaveMonitoring.pLongitude);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 250, "@pGeoAddress", postSaveMonitoring.pGeoAddress);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, postSaveMonitoring.pXmlData.Length + 1, "@pXmlData", postSaveMonitoring.pXmlData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", postSaveMonitoring.pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", postSaveMonitoring.pMode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pVisitType", postSaveMonitoring.pVisitType);
                DBConnect.Execute(oCmd);
                pInspID = Convert.ToInt32(oCmd.Parameters["@pInspID"].Value);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                if (vErr == 0)
                    return "Record Saved Succesfully";
                else
                    return "Data Not Saved.";
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

        #region MonitoringComplianceTools

        public List<GetMonitoringCompliance> GetMonitoringCompliance(PostMonitoringCompliance postMonitoringCompliance)
        {
            DataTable dt = new DataTable();
            List<GetMonitoringCompliance> row = new List<GetMonitoringCompliance>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_GetMonitorCmplDetailsById";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMonitoringCompliance.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postMonitoringCompliance.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetMonitoringCompliance(Convert.ToString(rs["MonitoringId"]), Convert.ToString(rs["FromDt"]), Convert.ToString(rs["ToDt"]),
                            Convert.ToString(rs["ItemID"]), Convert.ToString(rs["ItemName"]),
                            Convert.ToString(rs["ItemTypeID"]), Convert.ToString(rs["ItemTypeName"]),
                            Convert.ToString(rs["QID"]), Convert.ToString(rs["Question"]),
                            Convert.ToString(rs["Block"]), Convert.ToString(rs["MonitoringYN"]), Convert.ToString(rs["MonitoringRemarks"])));
                    }
                }
                else
                {
                    row.Add(new GetMonitoringCompliance("No data available", "", "", "", "", "", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetMonitoringCompliance("No data available", ex.Message, "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<GetOtherMonitoringCompliance> GetOtherMonitoringCompliance(PostMonitoringCompliance postMonitoringCompliance)
        {
            DataTable dt = new DataTable();
            List<GetOtherMonitoringCompliance> row = new List<GetOtherMonitoringCompliance>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_GetOtherMonitorCmplDetailsById";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMonitoringCompliance.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDate", postMonitoringCompliance.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetOtherMonitoringCompliance(Convert.ToString(rs["MonitoringId"]), Convert.ToString(rs["FromDt"]), Convert.ToString(rs["ToDt"]),
                            Convert.ToString(rs["ItemID"]), Convert.ToString(rs["ItemName"]),
                            Convert.ToString(rs["ItemTypeID"]), Convert.ToString(rs["ItemTypeName"]),
                            Convert.ToString(rs["QID"]), Convert.ToString(rs["Question"]),
                            Convert.ToString(rs["Block"]), Convert.ToString(rs["MonitoringYN"]), Convert.ToString(rs["MonitoringRemarks"]), Convert.ToString(rs["VisitType"])
                            , Convert.ToString(rs["EoName"]), Convert.ToString(rs["Eoid"]), Convert.ToString(rs["Marketid"])
                            , Convert.ToString(rs["Market"]), Convert.ToString(rs["MemberID"]), Convert.ToString(rs["MemberName"])));
                    }
                }
                else
                {
                    row.Add(new GetOtherMonitoringCompliance("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetOtherMonitoringCompliance("No data available", ex.Message, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public List<GetOtherMonitoringData> Get_Insp_Mob_OtherVisitDownload(PostMonitoringCompliance postMonitoringCompliance)
        {
            DataTable dt = new DataTable();
            List<GetOtherMonitoringData> row = new List<GetOtherMonitoringData>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_Mob_OtherVisitDownload";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMonitoringCompliance.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDate", postMonitoringCompliance.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetOtherMonitoringData(Convert.ToString(rs["EoName"]), Convert.ToString(rs["Eoid"]), Convert.ToString(rs["Marketid"]),
                            Convert.ToString(rs["Market"]), Convert.ToString(rs["MemberID"]),
                            Convert.ToString(rs["MemberName"]), Convert.ToString(rs["VisitType"])));
                    }
                }
                else
                {
                    row.Add(new GetOtherMonitoringData("No data available", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetOtherMonitoringData("No data available", ex.Message, "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }


        public List<GetOtherMonitoringData> Get_Insp_Mob_ODVisitDownload(PostMonitoringCompliance postMonitoringCompliance)
        {
            DataTable dt = new DataTable();
            List<GetOtherMonitoringData> row = new List<GetOtherMonitoringData>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_Mob_ODVisitDownload";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postMonitoringCompliance.pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pLoginDate", postMonitoringCompliance.pDate);
                DBConnect.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetOtherMonitoringData(Convert.ToString(rs["EoName"]), Convert.ToString(rs["Eoid"]), Convert.ToString(rs["Marketid"]),
                            Convert.ToString(rs["Market"]), Convert.ToString(rs["MemberID"]),
                            Convert.ToString(rs["MemberName"]), ""));
                    }
                }
                else
                {
                    row.Add(new GetOtherMonitoringData("No data available", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetOtherMonitoringData("No data available", ex.Message, "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string SaveMonitoringCompliance(PostSaveMonitoringCompliance postSaveMonitoringCompliance)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0, pInspID = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Insp_SaveMonitorCompliance";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 4, "@pInspID", postSaveMonitoringCompliance.pInspID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pCmplDt", postSaveMonitoringCompliance.pCmplDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, postSaveMonitoringCompliance.pXmlData.Length + 1, "@pXmlData", postSaveMonitoringCompliance.pXmlData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", postSaveMonitoringCompliance.pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 5, "@pMode", "Edit");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 500, "@pErrDesc", "");
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                string vErrDesc = Convert.ToString(oCmd.Parameters["@pErrDesc"].Value);
                if (vErr == 0)
                    return "Record Saved Succesfully";
                else
                    return "Data Not Saved.";
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

        #region Decrypt
        public string Decrypt()
        {
            string vResponse = "d2KabQ+sZk1In0WKimtQKZ3Wtv0cQGK3AmpEYrXHWwEXSGjhF4Xf+6LYgZC28i0mSCUPcT6tzgKNdKkOgVE+C0qgcoHqYWt1BKvHHHJKc/0D0BWE2Vwn+CMCOjqO1k+tfjrmQgamoBtVULBgYQ7omfk4Eis5qAuuwZ5HwyfKYfRpKibc3wtxYxhapRBjOUddVzfiM4IrJdQ6AGTWLv8SUY6CyPboZpVsx+sc0Ao8dbI72nmFetdzyZnRibikzaMl07IEfyqFob3f7o7Rj7FlUGfRy/CtegQAcY17inYwyT/+w3ZdfUg4vEJoF8BMxkhuo331tvkAgAGHU98RuC3RW5qtWyv6eCBnQ4UqH93ICc6jErSPjZGmiYie6OTFNKSu2sXnOnIGiH0RxS+kO13i0GbJBNeNZwtdgzkOrtDi6ZidlNFQ5v01+wpNPfYeSLghE76Ax0NEWquIaEjPwK6KiJCvSWKuKwCKWbWnBDL8YYZBlQ+cZ2NPQO6V35Ssaf0qWtACpQ3AKWag+r1RwyX7xkleK+qZHJ12+NBun1DzxYJO5XJoarwo7YiK98+I4T8ZerS50TyolMhs5aDBEogtT6DUa1djiEep/mrxSmAWfFMq4+6Ni9IrPqzJ4G0HCjbsdYnA4agWHRTMtdr7hWJHuk9LlzyxFPrqR0CjOMUuzIw=";
            return DataDeCrypt(vResponse);
        }
        #endregion

        #region Incentive
        public List<GetIncentiveLoWise> GetRptIncentive_LoWise(PostIncentiveLoWise postIncentiveLoWise)
        {
            DataSet ds = null;
            List<GetIncentiveLoWise> row = new List<GetIncentiveLoWise>();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "RptIncentive_LoWise";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 10, "@pDtTo", postIncentiveLoWise.pDtTo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", postIncentiveLoWise.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEOId", postIncentiveLoWise.pEOId);

                ds = DBConnect.GetDataSet(oCmd);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow rs in ds.Tables[0].Rows)
                    {
                        row.Add(new GetIncentiveLoWise(
                            Convert.ToString(rs["FinalIncentiveAmount"])));
                    }
                }
                else
                {
                    row.Add(new GetIncentiveLoWise("No data available"));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetIncentiveLoWise(ex.Message.ToString()));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }
        #endregion

        #region Ho Neft Transfer
        public string InsertNEFTTransfer(string pXml, string pDescId, string pCreatedby, string pLoanDt, string pEntType)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            string pMsg = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertNEFTTransfer";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXml.Length + 1, "@pXml", pXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 8, "@pBankDescId", pDescId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedby", Convert.ToInt32(pCreatedby));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", pEntType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanDt", setDate(pLoanDt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 30000, "@pMsg", pMsg);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pMsg = Convert.ToString(oCmd.Parameters["@pMsg"].Value);
                if (vErr == 0)
                {
                    SendAutomail("Disbursement Proccess Successfully Done", "Loan Disbursement Process");
                    return "Success";
                }
                else
                {
                    if (vErr == 2)
                    {
                        return "Fail";
                    }
                    else
                    {
                        return "Fail";
                    }
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

        #region SetDate
        public static DateTime setDate(string pDate)
        {
            string StrDD, StrMM, StrYYYY, strDate;

            string pattern = "MM/dd/yyyy"; //date pattern

            var dDate = DateTime.Now;

            if (pDate == "")
            {
                dDate = Convert.ToDateTime("01/01/1900");
            }
            else
            {
                if (pDate.Length == 9)
                {
                    pDate = pDate.Insert(0, "0");
                }
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                strDate = (StrMM + "/" + StrDD + "/" + StrYYYY);

                dDate = DateTime.ParseExact(strDate, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            return dDate;
        }
        #endregion

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

        #region DayEndProcess
        public Int32 DayEndProcess(string pUserId, string pDayEnddt, string pXmlData, string YearNo, string FinFrom, string pFinYear)
        {
            string pErrMsg = "";
            Int32 vRet = 0;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "DayEndProcess";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", Convert.ToInt32(pUserId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDayEnddt", setDate(pDayEnddt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pXmlData.Length + 1, "@pXmlData", pXmlData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@YearNo", Convert.ToInt32(YearNo));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@FinFrom", FinFrom);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 10, "@pFinYear", pFinYear);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 30000, "@pErrMsg", pErrMsg);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vRet);
                DBConnect.Execute(oCmd);
                vRet = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pErrMsg = Convert.ToString(oCmd.Parameters["@pErrMsg"].Value);
                if (pErrMsg != "")
                {
                    SendAutomail(pErrMsg, "Day End Process");
                }
                return vRet;
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

        #region WebHook
        public string WebHook(Stream Req)
        {
            string v = new StreamReader(System.Web.HttpContext.Current.Request.InputStream).ReadToEnd();
            var stream = OperationContext.Current.RequestContext.RequestMessage.GetBody<Stream>();
            var sr = new StreamReader(stream);
            string body = "";
            try
            {
                string folderPath = HostingEnvironment.MapPath("~/Files/" + "WebHook"); ;
                System.IO.Directory.CreateDirectory(folderPath);
                File.WriteAllText(folderPath + "/" + "WebHook.txt", body);
            }
            catch
            {
            }
            return body;
        }
        #endregion

        #region sendmail

        public void SendAutomail(string MailBody, string MailSubject)
        {
            MailMessage oM = new MailMessage();
            oM.To.Add(vToMailId.ToString());
            oM.From = new MailAddress(vCompMailID);
            // oM.Subject = "Day End Process";
            oM.Subject = MailSubject;
            oM.Body = MailBody;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Host = "smtp.gmail.com";
            smtp.Credentials = new System.Net.NetworkCredential(vCompMailID, vPswd);
            smtp.EnableSsl = true;
            smtp.Timeout = 360000;
            smtp.Send(oM);
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
                    //else
                    //{
                    //vResponse = "[{\"message\":\"Time out\",\"status\":\"failed\"}]";
                    //}
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

        #region Voter ID Verification
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
        #endregion

        #region Aadhar Verification
        public IdfyAadharVerifyData IdfyAadharVerify(PostAadharData postAadharData)
        {
            ExtraFields oExtra = new ExtraFields();
            Data oData = new Data(NewGuid(), vKeyId, vOuId, vSecret, IdfyWebHook, "ADHAR", "xml", oExtra);
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
                if (vStatus != null)
                {
                    if (vStatus.ToUpper() == "SUCCESS")
                    {
                        vUid = IdfyAadharData.parsed_details.uid;
                        vUid = vUid.Substring(vUid.Length - 4);
                    }
                }
                UpdateIdfyAadhaarLog(vReqId, pData, vStatus, vUid);
            }
            finally { }
            return vStatus;
        }

        public string IdfyAadharVerifyJson(string pReqId)
        {
            string vStatus = "";
            System.Threading.Thread.Sleep(30000);
            for (int i = 0; i <= 24; i++)
            {
                vStatus = GetIdfyAadhaarLog(pReqId);
                if (vStatus != "")
                {
                    break;
                }
                System.Threading.Thread.Sleep(10000);
            }
            return vStatus;
        }
        #endregion

        #region CreateProfile
        public ProfileResponse IdfyCreateProfile(PostAadharData postAadharData)
        {
            ProfileRequest oPR = new ProfileRequest();
            Config oC = new Config();
            ProfileData oPD = new ProfileData();
            oC.id = NewGuid();
            oPR.config = oC;
            oPR.reference_id = postAadharData.AadharNo;
            oPR.data = oPD;
            ProfileResponse oResponse = new ProfileResponse(null, "", "");
            string vReqData = JsonConvert.SerializeObject(oPR);//Request Body
            string vUrl = "https://api.kyc.idfy.com/sync/profiles";
            string vResponse = HttpWebRequest(vUrl, vReqData);
            oResponse = JsonConvert.DeserializeObject<ProfileResponse>(vResponse);
            return oResponse;
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pReqId", vRequestID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVerificationStatus", "");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, vRes.Length + 1, "@pResponseData", vRes);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", PostVoterData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoid", PostVoterData.pEOId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pVoterNo", PostVoterData.VoterId);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", pRequestID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", postAadharData.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pEoid", postAadharData.pEOId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pReqAadhaarNo", vReqAadhaarNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseData.Length + 1, "@pResponseData", pResponseData);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", vReqId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pVerificationStatus", vStatus);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, vResponseData.Length + 1, "@pResponseData", vResponseData);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pRespAadhaarNo", vUid);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRefId", vReqId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.InputOutput, SqlDbType.NVarChar, -1, "@pResponseData", pResponseData);
                DBConnect.Execute(oCmd);
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

        #region AesEncryptData
        public string AesEncryptData(string pPassword)
        {
            string vPass = "";
            vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(pPassword), GetRijndaelManaged("Force@2301***DB")));
            return vPass;
        }
        #endregion

        #region AesDecryptData
        public string AesDecryptData(string pPassword)
        {
            string vPass = "";
            String key = "Force@2301***DB";
            var encryptedBytes = Convert.FromBase64String(pPassword);
            vPass = Encoding.UTF8.GetString(AesDecrypt(encryptedBytes, GetRijndaelManaged(key)));
            return vPass;
        }
        #endregion

        #region Experiment
        public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes;
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
                        csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
                return encryptedBytes;
            }
        }

        public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                AesManaged aes = new AesManaged();
                aes.Padding = PaddingMode.None;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes;
                using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var msPlain = new System.IO.MemoryStream())
                        {
                            csDecrypt.CopyTo(msPlain);
                            decryptedBytes = msPlain.ToArray();
                        }
                    }
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }

        public string Aes256()
        {
            string plaintext = "Hello, World!";
            Console.WriteLine(plaintext);
            // Generate a random key and IV
            byte[] key = new byte[32]; // 256-bit key
            byte[] iv = new byte[16]; // 128-bit IV
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }
            string vAesKey = Convert.ToBase64String(key);
            byte[] ciphertext = Encrypt(plaintext, key, iv);
            string encryptedText = Convert.ToBase64String(ciphertext);
            return encryptedText;
        }

        public string aes256decrypt(string RsaKey, string encryptedText)
        {
            byte[] key = Convert.FromBase64String(RsaKey);
            byte[] iv = new byte[16]; // 128-bit IV
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }
            byte[] bytes = Convert.FromBase64String(encryptedText);
            string decryptedText = Decrypt(bytes, key, iv);
            return decryptedText;
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
                //pReqData.Request = pReq;
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
            int vCGTID = 0, vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            //-------------------------------------------------------------
            vRequestId = Req.UnitySfb_RequestId;
            vMemberId = Req.DG.CUST_ID;
            vCGTID = Convert.ToInt32(Req.DG.APPLICATIONID);
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
            SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
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

        public ProsidexResponse ProsidexSearchCustomerEncryptipn(ProsidexRequest prosidexRequest)
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
                //// Generate a random key and IV
                //byte[] key = new byte[32]; // 256-bit key
                //byte[] iv = new byte[16]; // 128-bit IV
                //using (var rng = new RNGCryptoServiceProvider())
                //{
                //    rng.GetBytes(key);
                //    rng.GetBytes(iv);
                //}
                //string vAesKey = Convert.ToBase64String(key);
                //byte[] ciphertext = Encrypt(Requestdata, key, iv);
                //string encryptedText = Convert.ToBase64String(ciphertext);
                string encryptedText = "", key = "", EncryptedKey = "";

                encryptedText = EncryptStringAES(Requestdata, ref key);
                EncryptedKey = EncryptKey(key);
                //------------------------Token Generate--------------
                string vBearerToken = ProsidexGenerateToken();
                //----------------------------------------------------
                string vReq = "{\"Request\":{\"EncryptedRequest\":\"" + encryptedText + "\"}}";
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9004/UnitySfbWS/searchCustomer" : "http://144.24.116.182:9004/UnitySfbWS/searchCustomer";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + vBearerToken);
                request.Headers.Add("RsaKey", EncryptedKey);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(vReq);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();

                dynamic res = JsonConvert.DeserializeObject(vResponse);
                string vResponseCode = res.StatusInfo.ResponseCode;
                if (vResponseCode == "200")
                {
                    vUCIC = "";
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, Convert.ToInt32(vResponseCode == "" ? "500" : vResponseCode));
                }

                //string vAllData = "Plain Text :" + Requestdata + ",Encrypted Request :" + vReq + ",Sceret Key :" + key + ",RSA Key :" + EncryptedKey;

                //try
                //{
                //    string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "Prosidex", prosidexRequest.Request.DG.APPLICATIONID));
                //    System.IO.Directory.CreateDirectory(folderPath);
                //    Guid guid = Guid.NewGuid();
                //    File.WriteAllText(folderPath + "/" + Convert.ToString(guid) + ".text", vAllData);
                //}
                //finally { }


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

        public string ProsidexGenerateToken()
        {
            string vUserName = "Posidex_UAT", vPassword = "rV1hj/eBiet5LTa2bqNyDw==", vResponse = "", vBearerToken = "";
            try
            {
                string vReq = "{\"USER_NAME\":\"" + vUserName + "\",\"PASSWORD\":\"" + vPassword + "\"}";
                string postURL = vKarzaEnv == "PROD" ? "https://ucic.unitybank.co.in:9004/UnitySfbWS/token/generate-token" : "http://144.24.116.182:9004/UnitySfbWS/token/generate-token";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(vReq);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                vBearerToken = Convert.ToString(res.TOKEN);
            }
            catch
            {
                vBearerToken = "";
            }
            return vBearerToken;
        }

        public string EncryptKey(string plainText)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string k = "<RSAKeyValue><Modulus>eoHletbW31f3BW0rsYXVX3mpQH+716oDRL3BYePAiG0K3xSyKg8Lkf90PB/ya2Adqo5+HunsiiodwVXL1szAHjBxtr8zIeC0rL0xhkHLrpO+ceElAh6h+quRvdnua+Z6GVxBpENxC97T0Ce/r6laRgeQrR+TCTnHUUzoo8375fXqqwarAm1Tqcq0eWCtDlC1LU24vLmE9P0en3RPrUXJf6hF/08LcYUw4uUNi9+d63IcV0HcM7TYH6eBuNtCfECw7PVPc0onaeQ+BXWp9DfwsG8EietEJ3znIpZ83ZtmLwD8jkJ4STHlFjqphpH+wfZ+xNSgOVvnUSfP31toBGOuMw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
                rsa.FromXmlString(k);
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] ciphertext = rsa.Encrypt(plaintextBytes, true);
                return Convert.ToBase64String(ciphertext);
            }
        }

        public string EncryptStringAES(string plainText, ref string publicKey)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                // Set the key and IV
                //aesAlg.Key = Encoding.UTF8.GetBytes(key);
                byte[] aesKey = GenerateRandomKey(32);
                aesAlg.KeySize = 256;
                aesAlg.Key = aesKey;
                publicKey = Convert.ToBase64String(aesKey);

                aesAlg.IV = new byte[16]; // You may want to generate a random IV for each encryption

                // Set the cipher mode and padding
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream
                            swEncrypt.Write(plainText);
                        }
                    }

                    // Convert the encrypted stream to a byte array
                    byte[] encryptedBytes = msEncrypt.ToArray();

                    // Return the Base64-encoded result
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
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

        public byte[] GenerateRandomKey(int length)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[length];
                rng.GetBytes(key);
                return key;
            }
        }

        public ProsidexResponse PosidexEncrypt(Request Req)
        {
            ProsidexResponse oProsidexResponse = null;
            string vRequestdata = "", vFullResponse = "", vResponse = "", vUCIC = "", vRequestId = "",
              vMemberId = "", vPotentialYN = "N", vPotenURL = "", vResponseCode = "", vRsaKey = "",
              vResponseData = "", vEncryptedMatchResponse = "";
            int vCGTID = 0, vCreatedBy = 1;
            //-------------------------------------------------------------
            vRequestId = Req.UnitySfb_RequestId;
            vMemberId = Req.DG.CUST_ID;
            vCGTID = Convert.ToInt32(Req.DG.APPLICATIONID);
            //--------------------------------------------------------------
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            string vUserName = "", vPassword = "";
            foreach (string headerName in headers.AllKeys)
            {
                if (headerName == "UserID")
                {
                    vUserName = headers[headerName];
                }
                if (headerName == "Password")
                {
                    vPassword = headers[headerName];
                }
            }
            if (vUserName == "" || vUserName != "Posidex")
            {
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 401);
            }
            else if (vPassword == "" || vPassword != "qZa#9804!")
            {
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 401);
            }
            else
            {
                //------------------------------------------------------------
                vRequestdata = JsonConvert.SerializeObject(Req);
                vFullResponse = HttpRequest("http://bijliserver54.bijliftt.com:3007/ServicePosidex.svc/PosidexSearchCustomer", vRequestdata);
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
                vUCIC = Convert.ToString(vResp.POSIDEX_GENERATED_UCIC);
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
                SaveProsidexLog(vMemberId, vCGTID, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------     
            }
            return oProsidexResponse;
        }

        #endregion

        public ProsiReq test(string requestCode)
        {
            ProsiReq p = new ProsiReq();
            p.pMemberId = requestCode;
            return p;
        }

        #region Bima Plan
        public string BimaPlan()
        {
            DataTable dt = new DataTable();
            dt = GetBimaPlanData();
            string vResponse = SubmitProposal(dt);
            return vResponse;
        }

        public DataTable GetBimaPlanData()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBimaPlanData";
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        public string fnHttpWebRequest(string PostUrl, string Requestdata, string ApiKey)
        {
            string vResponse = "";
            try
            {
                string postURL = PostUrl;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "apiKey " + ApiKey);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    vResponse = sr.ReadToEnd();
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

        public string fnBimaplanHttpWebRequest(string PostUrl, string Requestdata)
        {
            string vResponse = "";//unity-api-key:unity-secret-key
            string ClientSignature = vKarzaEnv == "PROD" ? "aKTWQPEK4SRWW1205ALoCRA233are3tMsxkUEKDsQK=:4ERhwuRfBA7Ut19prwy+m6Db8YGvKvplGverk7C3V8n=" : "unity-api-key:unity-secret-key";
            try
            {
                string postURL = PostUrl;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("client-signature", ClientSignature);
                request.Headers.Add("third-party-code", "UNIBNK");
                request.Headers.Add("user-type", "ORGANIZATION");

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    vResponse = sr.ReadToEnd();
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

        public string GetGuote(string pLoanNo, string pProductCode, string pDOB, double pLoanAmt, int pLoanTenure,
            int pPolicyTenure, double pSumAssured, int pNumberOfLives, int pCreatedBy)
        {
            string vTransactionId = "";
            string vUrl = vKarzaEnv == "PROD" ? "https://production.bimaplan.co/quote" : "https://uat.bimaplan.co/quote";
            //---------------Obj-------------------
            QuoteRequest oQR = new QuoteRequest();
            QuoteData oData = new QuoteData();
            CreditLifeDetails oCD = new CreditLifeDetails();
            QuoteApplicantDetails oAD = new QuoteApplicantDetails();
            AdditionalFields oAF = new AdditionalFields();
            //------------------------------------
            oAD.dateOfBirth = pDOB;
            oCD.loanAmount = pLoanAmt;
            oCD.loanTenure = pLoanTenure;
            oCD.policyTenure = pPolicyTenure;
            oCD.sumAssured = pSumAssured;
            oCD.numberOfLives = pNumberOfLives;
            //------------------------------------
            oData.applicantDetails = oAD;
            oData.creditLifeDetails = oCD;
            //----------Additional Fields----------
            oAF.dummyField1 = "";
            oAF.dummyField2 = "";
            //------------------------------------
            oQR.additionalFields = oAF;
            oQR.data = oData;
            oQR.productCode = pProductCode;
            //------------------------------------
            string RequestJson = JsonConvert.SerializeObject(oQR);
            try
            {
                string ResponseJson = fnBimaplanHttpWebRequest(vUrl, RequestJson);
                //string ResponseJson = "{\"message\":\"Successfully fetched premium\",\"data\":{\"transactionId\":\"ce674b58-49b2-4441-adf7-3588262ec14e\",\"premiumDetails\":{\"gst\":171.34,\"net\":951.86,\"total\":1123.2,\"currency\":\"INR\"},\"premiumBreakup\":[{\"providerName\":\"AFLI\",\"productName\":\"CREDIT_LIFE\",\"planName\":\"AFLI Credit Life\",\"premiumDetails\":{\"total\":1123.2,\"gst\":171.34,\"net\":951.86,\"currency\":\"INR\",\"quoteInsurerSpecificData\":null}}]},\"additionalFields\":{\"dummyField1\":\"\",\"dummyField2\":\"\"},\"timestamp\":1715855692777}";
                dynamic obj = JsonConvert.DeserializeObject(ResponseJson);
                string vMsg = Convert.ToString(obj.message);
                if (vMsg.Contains("Successfully fetched premium"))
                {
                    vTransactionId = Convert.ToString(obj.data.transactionId);
                }
                SaveBimaPlanLog(pLoanNo, RequestJson, ResponseJson, vTransactionId, "Q", pCreatedBy);
            }
            catch (Exception ex)
            {
            }
            finally { }
            return vTransactionId;
        }

        public string SubmitProposal(DataTable dt)
        {
            string vUrl = vKarzaEnv == "PROD" ? "https://production.bimaplan.co/proposal" : "https://uat.bimaplan.co/proposal";
            string RequestJson = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string vTransanctionId = "";
                vTransanctionId = GetGuote(Convert.ToString(dt.Rows[i]["LoanAccountNumber"]), Convert.ToString(dt.Rows[i]["ProductCode"]),
                   Convert.ToString(dt.Rows[i]["DOB"]), Convert.ToDouble(dt.Rows[i]["LoanAmount"]), Convert.ToInt32(dt.Rows[i]["LoanTenure"]),
                   Convert.ToInt32(dt.Rows[i]["PolicyTerm"]), Convert.ToDouble(dt.Rows[i]["SumAssured"]), 2, 1);
                if (vTransanctionId != "")
                {
                    ProRequestData oPR = new ProRequestData();
                    ProData oPD = new ProData();
                    ProCreditLifeDetails oCLD = new ProCreditLifeDetails();
                    List<ProInsuredDetail> oLID = new List<ProInsuredDetail>();
                    ProApplicantDetails oAD = new ProApplicantDetails();
                    List<ProNomineeDetail> oLND = new List<ProNomineeDetail>();
                    AdditionalFields oAF = new AdditionalFields();
                    //-------------------------------------
                    oAF.dummyField1 = "";
                    oAF.dummyField2 = "";
                    //-----------------------------------
                    oCLD.loanAccountNumber = Convert.ToString(dt.Rows[i]["LoanAccountNumber"]);
                    oCLD.loanAmount = Convert.ToDouble(dt.Rows[i]["LoanAmount"]);
                    oCLD.loanTenure = Convert.ToInt32(dt.Rows[i]["LoanTenure"]);
                    oCLD.policyTenure = Convert.ToInt32(dt.Rows[i]["PolicyTerm"]);
                    oCLD.loanDisbursementDate = Convert.ToString(dt.Rows[i]["LoanDisbursementDate"]);
                    oCLD.sumAssured = Convert.ToDouble(dt.Rows[i]["SumAssured"]);
                    oCLD.premiumFunding = Convert.ToString(dt.Rows[i]["PremiumFunding"]);
                    oCLD.loanSanctionDate = Convert.ToString(dt.Rows[i]["LoanSanctionDate"]);
                    oCLD.medicalQuestions = "Yes";
                    oCLD.coverType = "Level";
                    oCLD.numberOfLives = 2;
                    //------------------------------------
                    oAD.referenceId = Convert.ToString(dt.Rows[i]["MemberNo"]);
                    oAD.country = "India";
                    oAD.firstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    oAD.lastName = Convert.ToString(dt.Rows[i]["LastName"]);
                    oAD.dateOfBirth = Convert.ToString(dt.Rows[i]["DOB"]);
                    oAD.gender = Convert.ToString(dt.Rows[i]["Gender"]);
                    oAD.education = Convert.ToString(dt.Rows[i]["Education"]);
                    oAD.maritalStatus = Convert.ToString(dt.Rows[i]["MaritalStatus"]);
                    // oAD.email = Convert.ToString(dt.Rows[i]["Email"]);
                    oAD.mobile = Convert.ToString(dt.Rows[i]["Mobile"]);
                    oAD.addressLine1 = Convert.ToString(dt.Rows[i]["AddressLine1"]);
                    oAD.addressLine2 = Convert.ToString(dt.Rows[i]["AddressLine2"]);
                    oAD.pincode = Convert.ToString(dt.Rows[i]["PinCode"]);
                    oAD.city = Convert.ToString(dt.Rows[i]["City"]);
                    oAD.state = Convert.ToString(dt.Rows[i]["StateName"]);
                    oAD.occupation = Convert.ToString(dt.Rows[i]["OccupationName"]);
                    oAD.documentType = Convert.ToString(dt.Rows[i]["GovtDocumentType"]);
                    oAD.documentNumber = Convert.ToString(dt.Rows[i]["DocumentNumber"]);
                    oAD.isApplicantInGoodHealth = "Yes";
                    //-----------------------------------
                    oLND.Add(new ProNomineeDetail(Convert.ToString(dt.Rows[i]["NomineeFName"]), Convert.ToString(dt.Rows[i]["NomineeLName"]), Convert.ToString(dt.Rows[i]["NomineeRelation"])
                        , Convert.ToString(dt.Rows[i]["NomineeDOB"]), Convert.ToString(dt.Rows[i]["NomineeGender"]), Convert.ToString(dt.Rows[i]["NomineeMobile"]),
                        Convert.ToString(dt.Rows[i]["NomineeAddressLine1"]), Convert.ToString(dt.Rows[i]["NomineeAddressLine2"]), Convert.ToString(dt.Rows[i]["NomineePincode"]),
                        Convert.ToString(dt.Rows[i]["PercentageShare"]), Convert.ToString(dt.Rows[i]["NomineeCity"]), Convert.ToString(dt.Rows[i]["NomineeState"]),
                        Convert.ToString(dt.Rows[i]["NomineeOccupation"])));
                    //------------------------------------
                    oLID.Add(new ProInsuredDetail("borrower", Convert.ToString(dt.Rows[i]["ExternalReferenceNumber"]), oAD, oLND));
                    //------------------------------------
                    oAD = new ProApplicantDetails();
                    oLND = new List<ProNomineeDetail>();
                    //------------------------------------
                    oAD.referenceId = Convert.ToString(dt.Rows[i]["MemberNo"]) + "-C";
                    oAD.country = "India";
                    oAD.firstName = Convert.ToString(dt.Rows[i]["NomineeFName"]);
                    oAD.lastName = Convert.ToString(dt.Rows[i]["NomineeLName"]);
                    oAD.dateOfBirth = Convert.ToString(dt.Rows[i]["NomineeDOB"]);
                    oAD.gender = Convert.ToString(dt.Rows[i]["NomineeGender"]);
                    oAD.education = Convert.ToString(dt.Rows[i]["NomineeEducation"]);
                    oAD.maritalStatus = Convert.ToString(dt.Rows[i]["NomineeMaritalStatus"]);
                    //oAD.email = Convert.ToString(dt.Rows[i]["Email"]);
                    oAD.mobile = Convert.ToString(dt.Rows[i]["NomineeMobile"]);
                    oAD.addressLine1 = Convert.ToString(dt.Rows[i]["NomineeAddressLine1"]);
                    oAD.addressLine2 = Convert.ToString(dt.Rows[i]["NomineeAddressLine2"]);
                    oAD.pincode = Convert.ToString(dt.Rows[i]["NomineePincode"]);
                    oAD.city = Convert.ToString(dt.Rows[i]["NomineeCity"]);
                    oAD.state = Convert.ToString(dt.Rows[i]["NomineeState"]);
                    oAD.occupation = Convert.ToString(dt.Rows[i]["NomineeOccupation"]);
                    oAD.documentType = Convert.ToString(dt.Rows[i]["NomineeIdType"]);
                    oAD.documentNumber = Convert.ToString(dt.Rows[i]["NomineeIdNo"]);
                    oAD.relationshipWithBorrower = Convert.ToString(dt.Rows[i]["NomineeRelation"]);
                    oAD.isApplicantInGoodHealth = "Yes";
                    //-----------------------------------
                    oLND.Add(new ProNomineeDetail(Convert.ToString(dt.Rows[i]["FirstName"]), Convert.ToString(dt.Rows[i]["LastName"]), Convert.ToString(dt.Rows[i]["MemberRelation"]),
                        Convert.ToString(dt.Rows[i]["DOB"]), Convert.ToString(dt.Rows[i]["Gender"]), Convert.ToString(dt.Rows[i]["Mobile"]),
                        Convert.ToString(dt.Rows[i]["AddressLine1"]), Convert.ToString(dt.Rows[i]["AddressLine2"]), Convert.ToString(dt.Rows[i]["PinCode"]),
                        Convert.ToString(dt.Rows[i]["PercentageShare"]), Convert.ToString(dt.Rows[i]["City"]), Convert.ToString(dt.Rows[i]["StateName"]),
                        Convert.ToString(dt.Rows[i]["OccupationName"])));
                    //-------------------------------------
                    oLID.Add(new ProInsuredDetail("coborrower", Convert.ToString(dt.Rows[i]["ExternalReferenceNumber"]), oAD, oLND));
                    //------------------------------------
                    oPD.insuredDetails = oLID;
                    oPD.additionalFields = oAF;
                    oPD.creditLifeDetails = oCLD;
                    //------------------------------------
                    oPR.productCode = Convert.ToString(dt.Rows[i]["ProductCode"]);
                    oPR.transactionId = vTransanctionId;
                    oPR.data = oPD;
                    //------------------------------------
                    RequestJson = JsonConvert.SerializeObject(oPR);
                    string ResponseData = fnBimaplanHttpWebRequest(vUrl, RequestJson);
                    SaveBimaPlanLog(Convert.ToString(dt.Rows[i]["LoanAccountNumber"]), RequestJson, ResponseData, vTransanctionId, "P", 1);
                }
            }
            return "Success";
        }

        #endregion

        #region SaveBimaPlanLog
        public Int32 SaveBimaPlanLog(string pLoanNo, string pRequestJson, string pResponseJson, string pTransactionId, string pType, Int32 pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveBimaPlanLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanNo", pLoanNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pRequestJson.Length + 1, "@pRequestJson", pRequestJson);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.NVarChar, pResponseJson.Length + 1, "@pResponseJson", pResponseJson);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTransactionId", pTransactionId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pType", pType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 8, "@pCreatedBy", pCreatedBy);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region Minio Image Upload
        public string UploadFileMinio(byte[] image, string fileName, string enqId, string bucketName, string minioUrl)
        {
            string fullResponse = "", isImageSaved = "N";
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("image", Convert.ToBase64String(image));
            postParameters.Add("KycId", enqId);
            postParameters.Add("BucketName", bucketName);
            postParameters.Add("ImageName", fileName);
            // Create request and receive response
            //  string postURL = "https://ocr.bijliftt.com/KYCFileUploadBase64";
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

        public static byte[] strmToByte(Stream vStream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                vStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion

        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region PosidexReCall
        public string PosidexReCall()
        {
            DataTable dt = new DataTable();
            dt = GetPosidexReCallData();
            foreach (DataRow dR in dt.Rows)
            {
                ProsiReq pReq = new ProsiReq();
                pReq.pMemberId = dR["MemberId"].ToString();
                pReq.pCreatedBy = "200324";
                pReq.pCGTId = dR["CGTId"].ToString();
                string vRequestdata = JsonConvert.SerializeObject(pReq);
                string vFullResponse = HttpRequest("https://centrummob.bijliftt.com/CentrumService.svc/Prosidex", vRequestdata);
            }
            return "Complete";
        }

        public DataTable GetPosidexReCallData()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetPosidexReCallData";
                DBConnect.ExecuteForSelect(oCmd, dt);
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

        #region UpdateLogoutDtTime
        public string UpdateLogoutDt(string LoginId)
        {
            int vErr = 1;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateLogOutDt";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoginId", Convert.ToInt32(LoginId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRst", vErr);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pRst"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vErr == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data.";
        }
        #endregion

        #region UpdateSessionTime
        public string UpdateSessionTime(string LoginId)
        {
            Int32 vRst = 1;
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateSessionTime";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoginId", LoginId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pRst", vRst);
                DBConnect.Execute(oCmd);
                vRst = Convert.ToInt32(oCmd.Parameters["@pRst"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vRst == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data.";
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
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertLoginDt";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pUserId", Convert.ToInt32(Req.pUserId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pLoginId", vLoginId);
                DBConnect.Execute(oCmd);
                vLoginId = Convert.ToString(oCmd.Parameters["@pLoginId"].Value);
                oLD.LoginId = vLoginId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return oLD;
        }
        #endregion

        #region AadhaarNoValidate

        // The multiplication table (D5 table)
        private static readonly int[,] d = new int[,]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 },
            { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 },
            { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 },
            { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 },
            { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 },
            { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 },
            { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 },
            { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 },
            { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }
        };
        // The permutation table
        private static readonly int[,] p = new int[,]
        {
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 },
            { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2 },
            { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7 },
            { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0 },
            { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1 },
            { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5 },
            { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8 }
        };

        // The inverse table
        private static readonly int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

        public static bool Validate(string num)
        {
            int c = 0;
            int[] myArray = StringToReversedIntArray(num);
            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c, p[(i % 8), myArray[i]]];
            }
            return c == 0;
        }


        public static string generateVerhoeff(string num)
        {
            int c = 0;
            int[] myArray = StringToReversedIntArray(num);
            for (int i = 0; i < myArray.Length; i++)
            {
                c = d[c, p[((i + 1) % 8), myArray[i]]];
            }
            return inv[c].ToString();
        }


        private static int[] StringToReversedIntArray(string num)
        {
            int[] myArray = new int[num.Length];
            for (int i = 0; i < num.Length; i++)
            {
                myArray[i] = int.Parse(num.Substring(i, 1));
            }
            Array.Reverse(myArray);
            return myArray;
        }

        public string ValidateAadhaar(string aadhaarNumber)
        {
            bool isValid = Validate(aadhaarNumber);
            if (isValid == false)
            {
                return "Aadhaar number is Invalid";
            }
            else
            {
                return "Aadhaar number is valid";
            }
        }

        #endregion

        #region TeleCalling

        public List<PTPData> Mob_GetPTPData(PostKYCData pPTPRequest)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            List<PTPData> row = new List<PTPData>();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Mob_GetPTPData";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pAsOnDt", pPTPRequest.pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pPTPRequest.pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoid", pPTPRequest.pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pSearch", pPTPRequest.pSearch);
                DBConnect.ExecuteForSelect(oCmd, dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new PTPData(rs["BranchCode"].ToString(), rs["BranchName"].ToString(), rs["Eoid"].ToString(), rs["EoName"].ToString(),
                            rs["MarketID"].ToString(), rs["Market"].ToString(), rs["GroupNo"].ToString(), rs["GroupName"].ToString(), rs["MemberNo"].ToString(),
                            rs["Membername"].ToString(), rs["M_Mobile"].ToString(), rs["SecMobileNo"].ToString(), rs["CoAppName"].ToString(),
                            rs["LoanId"].ToString(), rs["LoanNo"].ToString(), rs["LoanAmt"].ToString(), rs["LoanDt"].ToString(), rs["POS"].ToString(),
                            rs["IOS"].ToString(), rs["PAR"].ToString(), rs["Prject"].ToString()));
                    }
                }
                else
                {
                    row.Add(new PTPData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new PTPData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
                oCmd.Dispose();
            }
            return row;
        }

        public string rptPartyLedger(string pLoanId, string pBrCode, string pProject)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            DataTable dt = null, dt1 = null;
            string vBase64String = "", vRptPath = "";
            string vMemName = "", vMemNo = "", vLnProduct = "";
            string vLnNo = "", vDisbDt = "", vSpouseNm = "";
            string vFundSource = "", vPurpose = "", vGroupName = "", vMarket = "", vEO = "";
            double vLoanAmt = 0, vIntAmt = 0, vTopupAmt = 0, vIntRate = 0, vOSAmt = 0;
            try
            {
                if (pProject.ToUpper() == "J")
                {
                    vRptPath = HostingEnvironment.MapPath("~/Reports/rptPartyLedger.rpt");
                    ds = rptPartyLedgerJLG(pLoanId, pBrCode);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0 && dt1.Rows.Count > 0)
                    {
                        vOSAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                        foreach (DataRow dr in dt1.Rows)
                        {
                            if (Convert.IsDBNull(dr["Principal_"]) == false)
                            {
                                vOSAmt = vOSAmt - Convert.ToDouble(dr["Principal_"].ToString());
                                dr["Out_Standing"] = Math.Round(vOSAmt, 2);
                            }
                        }
                        dt1.AcceptChanges();
                        vMemNo = dt.Rows[0]["MemberNo"].ToString();
                        vMemName = dt.Rows[0]["MemberName"].ToString();
                        vSpouseNm = dt.Rows[0]["Spouce"].ToString();
                        vFundSource = dt.Rows[0]["FundSource"].ToString();
                        vPurpose = dt.Rows[0]["Purpose"].ToString();
                        vLnNo = dt.Rows[0]["LoanNo"].ToString();
                        vLoanAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);
                        vIntAmt = Convert.ToDouble(dt.Rows[0]["IntAmt"]);
                        vDisbDt = dt.Rows[0]["LoanDt"].ToString();
                        vLnProduct = dt.Rows[0]["LoanType"].ToString();
                        vGroupName = dt.Rows[0]["Groupname"].ToString();
                        vMarket = dt.Rows[0]["Market"].ToString();
                        vEO = dt.Rows[0]["EoName"].ToString();
                        vTopupAmt = Convert.ToDouble(dt.Rows[0]["TopupAmt"]);
                        vIntRate = Convert.ToDouble(dt.Rows[0]["IntRate"]);
                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);
                            rptDoc.SetParameterValue("pCmpName", "Unity Small Finance Bank Limited");
                            rptDoc.SetParameterValue("pMemNo", vMemNo);
                            rptDoc.SetParameterValue("pMemName", vMemName);
                            rptDoc.SetParameterValue("pRoName", vEO);
                            rptDoc.SetParameterValue("pSpouceName", vSpouseNm);
                            rptDoc.SetParameterValue("pFndSource", vFundSource);
                            rptDoc.SetParameterValue("pGroupName", vGroupName);
                            rptDoc.SetParameterValue("pLoanNo", vLnNo);
                            rptDoc.SetParameterValue("pPurpose", vPurpose);
                            rptDoc.SetParameterValue("pLoanAmt", vLoanAmt);
                            rptDoc.SetParameterValue("pIntAmt", vIntAmt);
                            rptDoc.SetParameterValue("pDisbDate", vDisbDt);
                            rptDoc.SetParameterValue("pLoanSchm", vLnProduct);
                            rptDoc.SetParameterValue("pTopupAmt", vTopupAmt);
                            rptDoc.SetParameterValue("pIntRate", vIntRate);
                            Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                            vBase64String = Convert.ToBase64String(strmToByte(reportStream));
                            rptDoc.Dispose();
                        }
                    }
                }
                else if (pProject.ToUpper() == "S")
                {
                    vRptPath = HostingEnvironment.MapPath("~/Reports/PartyLedger.rpt");
                    ds = rptPartyLedgerSARAL(pLoanId, pBrCode);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.Subreports["OtherCollection.rpt"].SetDataSource(dt1);
                        rptDoc.SetParameterValue("pCmpName", "Unity Small Finance Bank(MEL-Saral Vyapar)");
                        rptDoc.SetParameterValue("pAddress1", "617 , Floor 6th, Neelkanth Corporate IT Park, Plot Number 240,240/01 , Kirol Road , Vidyavihar(West), Mumbai 400086");
                        rptDoc.SetParameterValue("pAddress2", "");
                        rptDoc.SetParameterValue("pBranch", pBrCode);
                        rptDoc.SetParameterValue("pTitle", "Party Ledger");
                        Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                        vBase64String = Convert.ToBase64String(strmToByte(reportStream));
                        rptDoc.Dispose();
                    }
                }
                else if (pProject.ToUpper() == "M")
                {
                    vRptPath = HostingEnvironment.MapPath("~/Reports/PartyLedgerMEL.rpt");
                    ds = rptPartyLedgerPDF(pLoanId, pBrCode);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.Subreports["OtherCollection.rpt"].SetDataSource(dt1);
                        rptDoc.SetParameterValue("pCmpName", "Unity Small Finance Bank(MEL)");
                        rptDoc.SetParameterValue("pAddress1", "617 , Floor 6th, Neelkanth Corporate IT Park, Plot Number 240,240/01 , Kirol Road , Vidyavihar(West), Mumbai 400086");
                        rptDoc.SetParameterValue("pAddress2", "");
                        rptDoc.SetParameterValue("pBranch", pBrCode);
                        rptDoc.SetParameterValue("pTitle", "Party Ledger");
                        Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                        vBase64String = Convert.ToBase64String(strmToByte(reportStream));
                        rptDoc.Dispose();
                    }

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
            return vBase64String;
        }

        public string SaveTeleCalling(SaveTeleCallingData TeleCallingData)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveTeleCalling";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, TeleCallingData.pXml.Length + 1, "@pXml", TeleCallingData.pXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(TeleCallingData.pUserId));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pEntType", "I");
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
        }

        public DataSet rptPartyLedgerSARAL(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedgerSaral";
                oCmd.CommandTimeout = 80000;
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 13, "@pLoanId", pSancId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pBCProductID", -1);
                ds = DBConnect.GetDataSet(oCmd);
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

        public DataSet rptPartyLedgerJLG(string pLoanId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedger";
                oCmd.CommandTimeout = 80000;
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pLoanId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pReportType", "P");
                ds = DBConnect.GetDataSet(oCmd);
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

        public DataSet rptPartyLedgerPDF(string pSancId, string pBrCode)
        {
            SqlCommand oCmd = new SqlCommand();
            DataSet ds = new DataSet();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "rptPartyLedgerMEL";
                oCmd.CommandTimeout = 80000;
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pLoanId", pSancId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBrCode", pBrCode);
                ds = DBConnect.GetDataSet(oCmd);
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

        #region SaveIDFYDigitalDOC
        public IdfyeSignResponse IdfyeSignStatus(IdfySignReq idfyRequest)
        {
            string vSignedUrl = "", vLoanAppId = "", vRequestData = "", vDocumentId = "", vProject = "J";
            IdfyeSignResponse vIdfyResp = null;
            try
            {
                vRequestData = JsonConvert.SerializeObject(idfyRequest);
                vDocumentId = idfyRequest.documentId;
                UpdateIdfyDigiDocLog("", "", vRequestData, "R", vDocumentId);
                if (idfyRequest.request.action.Contains("Signed"))
                {
                    vSignedUrl = GetIdfyDocByDocId(vDocumentId);
                    if (vSignedUrl != "")
                    {
                        byte[] SignedPdf = DownloadPdf(vSignedUrl);
                        vLoanAppId = GetLoanAppIdByeSignDocId(vDocumentId, ref vProject);
                        if (vLoanAppId != "")
                        {
                            if (MinioYN == "N")
                            {
                                string folderPath = HostingEnvironment.MapPath("~/Files/DigitalDoc");
                                string filePath = folderPath + "/" + vLoanAppId + ".pdf";
                                File.WriteAllBytes(filePath, SignedPdf);
                            }
                            else
                            {
                                if (vProject == "J")
                                {
                                    UploadFileMinio(SignedPdf, vLoanAppId + ".pdf", vLoanAppId, DigiDocBucket, MinioUrl);
                                }
                                else if (vProject == "S")
                                {
                                    UploadFileMinio(SignedPdf, vLoanAppId + ".pdf", vLoanAppId, "saraldigitaldoc", MinioUrl);
                                }
                            }
                        }
                        else
                        {
                            vIdfyResp = new IdfyeSignResponse("404", "Failure:No data available.");
                        }
                    }
                }
                vIdfyResp = new IdfyeSignResponse("200", "Success:Record saved successfully.");
            }
            catch
            {
                vIdfyResp = new IdfyeSignResponse("400", "Failure:Bad request.");
            }
            return vIdfyResp;
        }

        public string GetIdfyDocByDocId(string eSignDocId)
        {
            string vResponse = "", vDocUrl = "";
            Guid id = Guid.NewGuid();
            string vTaskID = Convert.ToString(id);
            try
            {
                IdfyDocData oDD = new IdfyDocData();
                oDD.esign_doc_id = eSignDocId;
                oDD.user_key = user_key;
                IdfyDocRequest oDR = new IdfyDocRequest();
                oDR.data = oDD;
                oDR.group_id = vTaskID;
                oDR.task_id = vTaskID;
                string vRequest = JsonConvert.SerializeObject(oDR);
                string postURL = "https://eve.idfy.com/v3/tasks/sync/generate/esign_retrieve";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Account-ID", Account_ID);
                request.Headers.Add("api-key", API_key);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(vRequest);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic objResponse = JsonConvert.DeserializeObject(vResponse);
                vDocUrl = objResponse.result.source_output.file_details.esign_file[0];
                UpdateIdfyDigiDocLog("", "", vResponse, "F", eSignDocId);
                return vDocUrl;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                UpdateIdfyDigiDocLog("", "", vResponse, "F", eSignDocId);
            }
            return vDocUrl;
        }

        public string GetLoanAppIdByReqId(string pRequestId)
        {
            string vLoanAppId = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAppIdByReqId";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRequestId", pRequestId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanAppId", vLoanAppId);
                DBConnect.Execute(oCmd);
                vLoanAppId = Convert.ToString(oCmd.Parameters["@pLoanAppId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vLoanAppId;
        }

        public string GetLoanAppIdByeSignDocId(string pEsignDocId, ref string pProject)
        {
            string vLoanAppId = "";
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanAppIdByeSignDocId";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEsignDocId", pEsignDocId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 12, "@pLoanAppId", vLoanAppId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 1, "@pProject", pProject);
                DBConnect.Execute(oCmd);
                vLoanAppId = Convert.ToString(oCmd.Parameters["@pLoanAppId"].Value);
                pProject = Convert.ToString(oCmd.Parameters["@pProject"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCmd.Dispose();
            }
            return vLoanAppId;
        }

        public Int32 UpdateIdfyDigiDocLog(string pTaskID, string pRequestId, string pResponseJson, string pMode, string pEsignDocId)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "UpdateIdfyDigiDocLog";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pTaskID", pTaskID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseJson.Length + 1, "@pResponseJson", pResponseJson);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pRequestId", pRequestId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pEsignDocId", pEsignDocId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMode", pMode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", vErr);
                DBConnect.Execute(oCmd);
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

        public byte[] DownloadPdf(string pUrl)
        {
            byte[] pdfBytes = null;
            using (WebClient wc = new WebClient())
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                string base64String = wc.DownloadString(pUrl);
                pdfBytes = Convert.FromBase64String(base64String.Replace("\"", ""));
            }
            return pdfBytes;
        }

        public IdfyeSignResponse IdfyeSignStatus1(IdfySign pRequest)
        {
            string vSignedUrl = "", vLoanAppId = "", vDocumentId = "", vProject = "J";
            IdfyeSignResponse vIdfyResp = null;
            try
            {
                vSignedUrl = pRequest.pUrl;
                vDocumentId = pRequest.pDocumentId;
                byte[] SignedPdf = DownloadPdf(vSignedUrl);
                vLoanAppId = GetLoanAppIdByeSignDocId(vDocumentId, ref vProject);
                if (vLoanAppId != "")
                {
                    if (MinioYN == "N")
                    {
                        string folderPath = HostingEnvironment.MapPath("~/Files/DigitalDoc");
                        string filePath = folderPath + "/" + vLoanAppId + ".pdf";
                        File.WriteAllBytes(filePath, SignedPdf);
                    }
                    else
                    {
                        if (vProject == "J")
                        {
                            UploadFileMinio(SignedPdf, vLoanAppId + ".pdf", vLoanAppId, DigiDocBucket, MinioUrl);
                        }
                        else if (vProject == "S")
                        {
                            UploadFileMinio(SignedPdf, vLoanAppId + ".pdf", vLoanAppId, "saraldigitaldoc", MinioUrl);
                        }
                    }
                }
                else
                {
                    vIdfyResp = new IdfyeSignResponse("404", "Failure:No data available.");
                }
                vIdfyResp = new IdfyeSignResponse("200", "Success:Record saved successfully.");
            }
            catch
            {
                vIdfyResp = new IdfyeSignResponse("400", "Failure:Bad request.");
            }
            return vIdfyResp;
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pFunderXml.Length + 1, "@pXml", pFunderXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", setDate(pLoginDt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.BigInt, 8, "@pFSUID", Convert.ToInt32(pFSUID));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoginDt", setDate(pLoginDt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pAppBy", Convert.ToInt32(pAppBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pAppRej", pAppRej);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region RecoveryPoolUpload
        public void RecoveryPoolUpload(string pLogin, string pCollXml, string pCreatedBy)
        {
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "InsertRecoveryPool";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLogin", setDate(pLogin));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, pCollXml.Length + 1, "@pXml", pCollXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pCreatedBy", Convert.ToInt32(pCreatedBy));
                DBConnect.Execute(oCmd);
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

        #region ICICI QR
        public CollQRResData CreateQR(CollQRReqData Req)
        {
            CollQRResData oCQ = new CollQRResData();
            string vResponse = "";
            try
            {
                string vUniqueNo = API_GetLoanDetails_ICICI(Req.pLoanId, Req.pAmt);
                if (vUniqueNo != "")
                {
                    var oDQR = new DynamicQRReqData()
                    {
                        amount = Req.pAmt,
                        ICICIMarchantName = "Unity Small Finance Bank",
                        ICICIMarchantVPA = "",
                        ICICIMccCode = "",
                        merchantId = "",
                        merchantTranId = vUniqueNo,
                        terminalId = "",
                        UniqueNo = vUniqueNo
                    };

                    string vReqData = JsonConvert.SerializeObject(oDQR);
                    //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    //HttpWebRequest request = WebRequest.Create(DynamicQRUrl) as HttpWebRequest;
                    //request.ContentType = "application/json";
                    //request.Method = "POST";
                    //using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    //{
                    //    streamWriter.Write(vReqData);
                    //    streamWriter.Close();
                    //}
                    //StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                    //vResponse = streamReader.ReadToEnd();
                    //request.GetResponse().Close();

                    vResponse = "{\"CreateDynamicQRResult\":\"upi:\\/\\/pay?pa=kushalkoley@ybl&pn=Unity Small Finance Bank&tr=" + vUniqueNo + "&am=" + Req.pAmt + "&cu=INR&mc=5411\"}";
                    dynamic obj = JsonConvert.DeserializeObject(vResponse);
                    oCQ.QRString = obj.CreateDynamicQRResult;
                    oCQ.BillerBillID = vUniqueNo;
                }
                else
                {
                    oCQ.QRString = "";
                    oCQ.BillerBillID = "";
                }
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
            }
            return oCQ;
        }

        public string API_GetLoanDetails_ICICI(string pLoanId, string pCollAmt)
        {
            SqlCommand oCmd = new SqlCommand();
            string pBillerBillID = "";
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "API_GetLoanDetails_ICICI";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", pLoanId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pCollAmt", Convert.ToDouble(pCollAmt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 30, "@pBillerBillID", pBillerBillID);
                DBConnect.Execute(oCmd);
                pBillerBillID = Convert.ToString(oCmd.Parameters["@pBillerBillID"].Value);
                return pBillerBillID;
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

        public ICICIQRPaymentStatus UdateICICIPaymentStatus(PostPaymentStatus Req)
        {
            ICICIQRPaymentStatus oQRP = new ICICIQRPaymentStatus("500", "Internal Server Error.");
            string pTransactionID = "";
            int vErr = API_PostLoanPaymentUpdateDetails_BBPS("ICICI", Req.merchantTranId, Req.PayerAmount, "", Req.BankRRN, ref pTransactionID, "");
            if (vErr > 0)
            {
                oQRP = new ICICIQRPaymentStatus("200", "Success.");
            }
            return oQRP;
        }

        public Int32 API_PostLoanPaymentUpdateDetails_BBPS(string pPlatformBillID, string pBillerBillID, string pAmount,
          string pPlatformTransactionRefID, string pUniquePaymentRefID, ref string pReceiptNumber, string pTransactionID)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "API_PostLoanPaymentUpdateDetails_BBPS";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pPlatformBillID", pPlatformBillID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 16, "@pbillerBillID", pBillerBillID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 8, "@pAmount", Convert.ToDouble(pAmount));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pPlatformTransactionRefID", pPlatformTransactionRefID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pUniquePaymentRefID", pUniquePaymentRefID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 100, "@pTransactionID", pTransactionID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 50, "@pReceiptNumber", 0);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.DateTime, 8, "@pTransDatetime", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                pReceiptNumber = Convert.ToString(oCmd.Parameters["@pReceiptNumber"].Value);
                //pTransDatetime = Convert.ToDateTime(oCmd.Parameters["@pTransDatetime"].Value);
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

        public ICICIQRPaymentStatus GetICICIPamentStatus(PostICICIPaymentStat Req)
        {
            ICICIQRPaymentStatus oQRP = new ICICIQRPaymentStatus("", "");
            int vCnt = GetICICIQrPaymentStatus(Req.BillerBillID);
            if (vCnt > 0)
            {
                oQRP = new ICICIQRPaymentStatus("200", "Success");
            }
            else
            {
                oQRP = new ICICIQRPaymentStatus("402", "Failed");
            }
            return oQRP;
        }

        public int GetICICIQrPaymentStatus(string pBillerBillID)
        {
            SqlCommand oCmd = new SqlCommand();
            int pCnt = 0;
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetICICIQrPaymentStatus";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pBillerBillID", pBillerBillID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pCnt", pCnt);
                DBConnect.Execute(oCmd);
                pCnt = Convert.ToInt32(oCmd.Parameters["@pCnt"].Value);
                return pCnt;
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

        #region eIBMAadhaarOTP
        //public IBMAadhaarOTPResponse IBMAadhaarOTP(eIBMAadhaarOTP eIBMAadhaarOTP)
        //{
        //    string sourceId = "MB";
        //    DateTime d = DateTime.Now;
        //    string dateString = d.ToString("yyyyMMddHHmmss");
        //    string vRndNo = GenerateRandomNo();
        //    string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"
        //    string txn = "AUANSDL001:" + dateString + vRndNo;
        //    string type = "A";
        //    string ch = "01";
        //    string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 
        //    IBMAadhaarOTPResponse responseObj = null;

        //    IBMAadhaarOTPReq pReq = new IBMAadhaarOTPReq();
        //    pReq.sourceId = sourceId;
        //    pReq.traceId = traceId;
        //    pReq.uid = eIBMAadhaarOTP.aadhaarNo;
        //    pReq.txn = txn;
        //    pReq.ts = ts;
        //    pReq.type = type;
        //    pReq.ch = ch;

        //    string requestBody = JsonConvert.SerializeObject(pReq);
        //    //string postURL = vKarzaEnv == "PROD" ? "https://baseurl/usfb/v1/uidai/KYCandVerification/aadharOtpRequest" : "https://baseurl/usfb/v1/uidai/KYCandVerification/aadharOtpRequest";
        //    string postURL = "https://connect-nonprod.unitybank.co.in/preprodunity/uatportal/usfb/v1/uidai/KYCandVerification/aadharOtpRequest";
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        if (request == null)
        //        {
        //            throw new NullReferenceException("request is not a http request");
        //        }
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Headers.Add("X-IBM-Client-Id", X_IBM_Client_Id);
        //        request.Headers.Add("X-IBM-Client-Secret", X_IBM_Client_Secret);
        //        request.Headers.Add("X-Client-IP", X_Client_IP);

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //        {
        //            streamWriter.Write(requestBody);
        //            streamWriter.Close();
        //        }
        //        StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
        //        string fullResponse = responseReader.ReadToEnd();
        //        request.GetResponse().Close();
        //        try
        //        {
        //            fullResponse = fullResponse.Replace("\u0000", "");
        //            fullResponse = fullResponse.Replace("\\u0000", "");

        //            dynamic res = JsonConvert.DeserializeObject(fullResponse);

        //            if (Convert.ToString(res.status) == "success")
        //            {
        //                responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //                responseObj.Status = Convert.ToString(res.status);
        //                responseObj.txn = txn;
        //                responseObj.StatusCode = "";
        //                responseObj.StatusMsg = Convert.ToString(res.message);
        //                responseObj.ErrorMsg = "";
        //            }
        //            else if (Convert.ToString(res.status) == "fail")
        //            {
        //                responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //                responseObj.Status = Convert.ToString(res.status);
        //                responseObj.txn = "";
        //                string err = Convert.ToString(res.err);
        //                responseObj.StatusCode = err;
        //                responseObj.StatusMsg = Convert.ToString(res.message);

        //                switch (err)
        //                {
        //                    case "110":
        //                        responseObj.ErrorMsg = "Aadhaar number does not have email ID.";
        //                        break;
        //                    case "111":
        //                        responseObj.ErrorMsg = "Aadhaar number does not have mobile number.";
        //                        break;
        //                    case "112":
        //                        responseObj.ErrorMsg = "Aadhaar number does not have both email ID and mobile number.";
        //                        break;
        //                    case "113":
        //                        responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified email ID.";
        //                        break;
        //                    case "114":
        //                        responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified Mobile Number.";
        //                        break;
        //                    case "115":
        //                        responseObj.ErrorMsg = "Aadhaar Number doesn’t have verified email and Mobile.";
        //                        break;
        //                    case "510":
        //                        responseObj.ErrorMsg = "Invalid “Otp” XML format.";
        //                        break;
        //                    case "520":
        //                        responseObj.ErrorMsg = "Invalid device.";
        //                        break;
        //                    case "521":
        //                        responseObj.ErrorMsg = "Invalid mobile number.";
        //                        break;
        //                    case "522":
        //                        responseObj.ErrorMsg = "Invalid “type” attribute.";
        //                        break;
        //                    case "523":
        //                        responseObj.ErrorMsg = "Invalid “ts” attribute. Either it is not in correct format or is older than 20 min.";
        //                        break;
        //                    case "530":
        //                        responseObj.ErrorMsg = "Invalid AUA code.";
        //                        break;
        //                    case "540":
        //                        responseObj.ErrorMsg = "Invalid OTP XML version.";
        //                        break;
        //                    case "542":
        //                        responseObj.ErrorMsg = "AUA not authorized for ASA. This error will be returned if AUA and ASA do not have linking in the portal.";
        //                        break;
        //                    case "543":
        //                        responseObj.ErrorMsg = "Sub-AUA not associated with “AUA”. This error will be returned if Sub-AUA specified in “sa” attribute is not added as “Sub-AUA” in portal.";
        //                        break;
        //                    case "565":
        //                        responseObj.ErrorMsg = "AUA License key has expired or is invalid.";
        //                        break;
        //                    case "566":
        //                        responseObj.ErrorMsg = "ASA license key has expired or is invalid.";
        //                        break;
        //                    case "569":
        //                        responseObj.ErrorMsg = "Digital signature verification failed.";
        //                        break;
        //                    case "570":
        //                        responseObj.ErrorMsg = "Invalid key info in digital signature(this means that certificate used for signing the OTP request is not valid – it is either expired, or does not belong to the AUA or is not created by a CA).";
        //                        break;
        //                    case "940":
        //                        responseObj.ErrorMsg = "Unauthorized ASA channel. ";
        //                        break;
        //                    case "941":
        //                        responseObj.ErrorMsg = "Unspecified ASA channel.";
        //                        break;
        //                    case "950":
        //                        responseObj.ErrorMsg = "Could not generate and/or send OTP.";
        //                        break;
        //                    case "952":
        //                        responseObj.ErrorMsg = "OTP Flooding error.";
        //                        break;
        //                    case "999":
        //                        responseObj.ErrorMsg = "Unknown error.";
        //                        break;
        //                }
        //            }
        //            else if (Convert.ToString(res.Status) == "Fail")
        //            {
        //                responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //                responseObj.Status = Convert.ToString(res.Status);
        //                responseObj.txn = "";
        //                string StatusCode = Convert.ToString(res.StatusCode);
        //                responseObj.StatusCode = StatusCode;
        //                responseObj.StatusMsg = Convert.ToString(res.ConnectionError);
        //                responseObj.ErrorMsg = Convert.ToString(res.ErrorMessage);
        //            }
        //            else if (Convert.ToString(res.NSDL_Error_Code) != "")
        //            {
        //                responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //                responseObj.Status = "";
        //                responseObj.txn = "";
        //                string NSDL_Error_Code = Convert.ToString(res.NSDL_Error_Code);
        //                responseObj.StatusCode = NSDL_Error_Code;
        //                responseObj.StatusMsg = "Failure from NSDL";

        //                switch (NSDL_Error_Code)
        //                {
        //                    case "E-000":
        //                        responseObj.ErrorMsg = "Request received is a HTTP request";
        //                        break;
        //                    case "E-001":
        //                        responseObj.ErrorMsg = "Request received is a get request";
        //                        break;
        //                    case "E-100":
        //                        responseObj.ErrorMsg = "Auth XML not parsed properly";
        //                        break;
        //                    case "E-101":
        //                        responseObj.ErrorMsg = "KYC XML not parsed properly";
        //                        break;
        //                    case "E-102":
        //                        responseObj.ErrorMsg = "Audit logging in DB failed for request";
        //                        break;
        //                    case "E-103":
        //                        responseObj.ErrorMsg = "Audit logging in DB failed for response";
        //                        break;
        //                    case "E-105":
        //                        responseObj.ErrorMsg = "KYC XSD Validation failed";
        //                        break;
        //                    case "E-106":
        //                        responseObj.ErrorMsg = "KYC Request signature verification failed";
        //                        break;
        //                    case "E-107":
        //                        responseObj.ErrorMsg = "Auth Request signature verification failed";
        //                        break;
        //                    case "E-108":
        //                        responseObj.ErrorMsg = "IP verification failed for entity";
        //                        break;
        //                    case "E-109":
        //                        responseObj.ErrorMsg = "Blank response received from UIDAI";
        //                        break;
        //                    case "E-110":
        //                        responseObj.ErrorMsg = "Unable to decrypt response at KSA";
        //                        break;
        //                    case "E-111":
        //                        responseObj.ErrorMsg = "KYC response signature verification failed";
        //                        break;
        //                    case "E-112":
        //                        responseObj.ErrorMsg = "BFD XSD validation failed";
        //                        break;
        //                    case "E-113":
        //                        responseObj.ErrorMsg = "OTP XSD validation failed";
        //                        break;
        //                    case "E-114":
        //                        responseObj.ErrorMsg = "KYC response XML not parsed properly";
        //                        break;
        //                    case "E-115":
        //                        responseObj.ErrorMsg = "AUTH response XML not parsed properly";
        //                        break;
        //                    case "E-116":
        //                        responseObj.ErrorMsg = "Signed Auth XML generation Error";
        //                        break;
        //                    case "E-117":
        //                        responseObj.ErrorMsg = "Signed KYC XML generation Error";
        //                        break;
        //                    case "E-118":
        //                        responseObj.ErrorMsg = "Auth response signature verification failed";
        //                        break;
        //                    case "E-119":
        //                        responseObj.ErrorMsg = "ASA or KSA is unable to connect to UIDAI server";
        //                        break;
        //                    case "E-120":
        //                        responseObj.ErrorMsg = "Auth XSD validation failed";
        //                        break;
        //                    case "E-122":
        //                        responseObj.ErrorMsg = "Property file unavailable";
        //                        break;
        //                    case "E-123":
        //                        responseObj.ErrorMsg = "BFD request XML not parsed properly";
        //                        break;
        //                    case "E-124":
        //                        responseObj.ErrorMsg = "OTP request XML not parsed properly";
        //                        break;
        //                    case "E-125":
        //                        responseObj.ErrorMsg = "BFD request signature verification failed";
        //                        break;
        //                    case "E-126":
        //                        responseObj.ErrorMsg = "OTP request signature verification failed";
        //                        break;
        //                    case "E-127":
        //                        responseObj.ErrorMsg = "Signed BFD XML generation error";
        //                        break;
        //                    case "E-128":
        //                        responseObj.ErrorMsg = "Signed OTP XML generation error";
        //                        break;
        //                    case "E-129":
        //                        responseObj.ErrorMsg = "BFD response XML not parsed properly";
        //                        break;
        //                    case "E-130":
        //                        responseObj.ErrorMsg = "OTP response XML not parsed properly";
        //                        break;
        //                    case "E-131":
        //                        responseObj.ErrorMsg = "XML decryption error";
        //                        break;
        //                    case "E-132":
        //                        responseObj.ErrorMsg = "Error during KYC request signature verification";
        //                        break;
        //                    case "E-133":
        //                        responseObj.ErrorMsg = "Error during KYC response signature verification";
        //                        break;
        //                    case "E-134":
        //                        responseObj.ErrorMsg = "Error during AUTH request signature verification";
        //                        break;
        //                    case "E-135":
        //                        responseObj.ErrorMsg = "Error during AUTH response signature verification";
        //                        break;
        //                    case "E-136":
        //                        responseObj.ErrorMsg = "Error during BFD request signature verification";
        //                        break;
        //                    case "E-137":
        //                        responseObj.ErrorMsg = "Error during OTP request signature verification";
        //                        break;
        //                    case "E-138":
        //                        responseObj.ErrorMsg = "Error during KYC XSD Validation";
        //                        break;
        //                    case "E-139":
        //                        responseObj.ErrorMsg = "Error during AUTH XSD Validation";
        //                        break;
        //                    case "E-140":
        //                        responseObj.ErrorMsg = "Error during BFD XSD Validation";
        //                        break;
        //                    case "E-141":
        //                        responseObj.ErrorMsg = "Error during OTP XSD Validation";
        //                        break;
        //                    case "E-142":
        //                        responseObj.ErrorMsg = "Error during IP verification";
        //                        break;
        //                    case "E-143":
        //                        responseObj.ErrorMsg = "Response received is E";
        //                        break;
        //                    case "E-144":
        //                        responseObj.ErrorMsg = "BFD response signature verification failed";
        //                        break;
        //                    case "E-145":
        //                        responseObj.ErrorMsg = "OTP Signature Tag Missing in Request XML";
        //                        break;
        //                    case "E-149":
        //                        responseObj.ErrorMsg = "Invalid Aadhar Number";
        //                        break;
        //                    case "E-199":
        //                        responseObj.ErrorMsg = "KSA/ASA Internal Error";
        //                        break;
        //                    case "E-200":
        //                        responseObj.ErrorMsg = "One of the mandatory Sub-Aua element is null";
        //                        break;
        //                    case "E-201":
        //                        responseObj.ErrorMsg = "Error while validating strSaCd parameter";
        //                        break;
        //                    case "E-203":
        //                        responseObj.ErrorMsg = "Error while validating strAadhaar parameter";
        //                        break;
        //                    case "E-204":
        //                        responseObj.ErrorMsg = "Error while validating strAadhaarName parameter";
        //                        break;
        //                    case "E-205":
        //                        responseObj.ErrorMsg = "Error while validating strYear parameter";
        //                        break;
        //                    case "E-206":
        //                        responseObj.ErrorMsg = "Error while validating strGender parameter";
        //                        break;
        //                    case "E-207":
        //                        responseObj.ErrorMsg = "Error while validating strTransId parameter";
        //                        break;
        //                    case "E-208":
        //                        responseObj.ErrorMsg = "Error while validating strUDC parameter";
        //                        break;
        //                    case "E-209":
        //                        responseObj.ErrorMsg = "Error while validating strMV parameter";
        //                        break;
        //                    case "E-210":
        //                        responseObj.ErrorMsg = "Value of mvThreshold in property file should be in range 1-100";
        //                        break;
        //                    case "E-211":
        //                        responseObj.ErrorMsg = "Error while validating strLang: If strLname is provided then strLang is mandatory";
        //                        break;
        //                    case "E-212":
        //                        responseObj.ErrorMsg = "Error while validating strLmv parameter. Value should be in range 1-100(inclusive)";
        //                        break;
        //                    case "E-213":
        //                        responseObj.ErrorMsg = "DSC signature verification failed for Sub-AUA";
        //                        break;
        //                    case "E-214":
        //                        responseObj.ErrorMsg = "Error while validating strOtp parameter";
        //                        break;
        //                    case "E-215":
        //                        responseObj.ErrorMsg = "Error while validating strRequestType parameter";
        //                        break;
        //                    case "E-216":
        //                        responseObj.ErrorMsg = "Error while validating strBioType parameter";
        //                        break;
        //                    case "E-217":
        //                        responseObj.ErrorMsg = "Error while validating strFingerOnePos parameter";
        //                        break;
        //                    case "E-218":
        //                        responseObj.ErrorMsg = "Error while validating strFingerTwoPos parameter";
        //                        break;
        //                    case "E-219":
        //                        responseObj.ErrorMsg = "Error while validating strMS parameter";
        //                        break;
        //                    case "E-220":
        //                        responseObj.ErrorMsg = "Blank Signature Tag Exception";
        //                        break;
        //                    case "E-221":
        //                        responseObj.ErrorMsg = "Incorrect OTP Version";
        //                        break;
        //                    case "E-230":
        //                        responseObj.ErrorMsg = "Error during DSC signature verification for Sub-AUA";
        //                        break;
        //                    case "E-235":
        //                        responseObj.ErrorMsg = "Request Signature Verification Failed";
        //                        break;
        //                    case "E-236":
        //                        responseObj.ErrorMsg = "Error during Request Signature Verification";
        //                        break;
        //                    case "E-237":
        //                        responseObj.ErrorMsg = "Error during KYC Response Encryption";
        //                        break;
        //                    case "E-238":
        //                        responseObj.ErrorMsg = "Inernal kua not mapped against KUA code";
        //                        break;
        //                    case "E-252":
        //                        responseObj.ErrorMsg = "Invalid Aadhaar no. length";
        //                        break;
        //                    case "E-253":
        //                        responseObj.ErrorMsg = "Biometric attribute is null/missing";
        //                        break;
        //                    case "E-297":
        //                        responseObj.ErrorMsg = "UIDAI PID Block Encryption Certificate Expired";
        //                        break;
        //                    case "E-299":
        //                        responseObj.ErrorMsg = "As per UIDAI guidelines, this device is not authorized to perform transactions. Kindly procure L1 registered device";
        //                        break;
        //                    case "E-555":
        //                        responseObj.ErrorMsg = "Duplicate transaction id error";
        //                        break;
        //                    case "E-563":
        //                        responseObj.ErrorMsg = "Read timeout while connecting to UIDAI";
        //                        break;
        //                    case "E-601":
        //                        responseObj.ErrorMsg = "SubAua Request XML Not Parsed Properly.";
        //                        break;
        //                    case "E-602":
        //                        responseObj.ErrorMsg = "SubAua XSD Validation Failed.";
        //                        break;
        //                    case "E-603":
        //                        responseObj.ErrorMsg = "Audit Logging in DB is failed for SubAua Request.";
        //                        break;
        //                    case "E-604":
        //                        responseObj.ErrorMsg = "License Verfication failed for SubAua Entity.";
        //                        break;
        //                    case "E-605":
        //                        responseObj.ErrorMsg = "SubAua Request Signature Verification Failed.";
        //                        break;
        //                    case "E-607":
        //                        responseObj.ErrorMsg = "DBAudit Failed For AuditOTP Request.";
        //                        break;
        //                    case "E-608":
        //                        responseObj.ErrorMsg = "Purpose not mapped against entity/Purpose used is not in active state.";
        //                        break;
        //                    case "E-609":
        //                        responseObj.ErrorMsg = "Entity-Service Mapping Verfication failed for SubAua Entity.";
        //                        break;
        //                    case "E-610":
        //                        responseObj.ErrorMsg = "RDData Received Null.";
        //                        break;

        //                }
        //            }
        //            //string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
        //            SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, fullResponse, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
        //        }
        //        finally
        //        {
        //        }
        //        //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(fullResponse);
        //        return responseObj;
        //    }
        //    catch (WebException we)
        //    {
        //        string Response = "";
        //        string status = Convert.ToString(we.Status);
        //        string WebResponse = Convert.ToString(we.Response);
        //        HttpWebResponse HttpWebResponse = (HttpWebResponse)we.Response;
        //        //HttpStatusCode HttpStatusCode = HttpWebResponse.StatusCode;
        //        string WebResponseMessage = Convert.ToString(we.Message);
        //        if (we.Response != null)
        //        {
        //            try
        //            {
        //                using (var stream = we.Response.GetResponseStream())
        //                using (var reader = new StreamReader(stream))
        //                {
        //                    Response = reader.ReadToEnd();
        //                }
        //                //Response = Response.Replace("status", "statusCode");
        //                Response = Response.Replace("\u0000", "");
        //                Response = Response.Replace("\\u0000", "");
        //                dynamic res = JsonConvert.DeserializeObject(Response);

        //                if (Convert.ToString(res.Status) == "Fail")
        //                {
        //                    responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //                    responseObj.Status = Convert.ToString(res.Status);
        //                    responseObj.txn = "";
        //                    string StatusCode = Convert.ToString(res.StatusCode);
        //                    responseObj.StatusCode = StatusCode;
        //                    responseObj.StatusMsg = Convert.ToString(res.ConnectionError);
        //                    responseObj.ErrorMsg = Convert.ToString(res.ErrorMessage);
        //                }
        //                //string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
        //                SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, Response, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
        //            }
        //            finally
        //            {
        //            }
        //        }
        //        else if (status != "")
        //        {
        //            responseObj = new IBMAadhaarOTPResponse("", "", "", "", "");
        //            responseObj.Status = status;
        //            responseObj.txn = "";
        //            responseObj.StatusCode = Convert.ToString(HttpStatusCode.InternalServerError);
        //            responseObj.StatusMsg = WebResponse;
        //            responseObj.ErrorMsg = WebResponseMessage;
        //        }

        //        //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(Response);
        //        return responseObj;
        //    }

        //    //catch (WebException ex)
        //    //{
        //    //    string Response = "";
        //    //    if (ex.Status == WebExceptionStatus.ProtocolError)
        //    //    {
        //    //        var response = ex.Response as HttpWebResponse;
        //    //        if (response != null)
        //    //        {
        //    //            //Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
        //    //            Response = Response.Replace("\u0000", "");
        //    //            Response = Response.Replace("\\u0000", "");
        //    //            dynamic res = JsonConvert.DeserializeObject(Response);

        //    //            if (Convert.ToString(res.Status) == "Fail")
        //    //            {
        //    //                responseObj.Status = Convert.ToString(res.Status);
        //    //                responseObj.txn = "";
        //    //                string StatusCode = Convert.ToString(res.StatusCode);
        //    //                responseObj.StatusCode = StatusCode;
        //    //                responseObj.StatusMsg = Convert.ToString(res.ConnectionError);
        //    //                responseObj.ErrorMsg = Convert.ToString(res.ErrorMessage);
        //    //            }
        //    //            //string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
        //    //            SaveIBMAadhaarOtp("eAadhaarOtp", txn, Response, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
        //    //        }
        //    //        else
        //    //        {
        //    //            // no http status code available
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        // no http status code available
        //    //    }
        //    //    return responseObj;
        //    //}
        //    finally
        //    {
        //    }
        //}

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
                //string fullResponse = "\"{\\\"status\\\":\\\"success\\\",\\\"message\\\":\\\"OTP has been sent to mobile number *******7773\\\",\\\"code\\\":\\\"97ff0775f6e84344bbc7df4eab766d2e\\\",\\\"txn\\\":\\\"AUANSDL001:202506241337051658\\\",\\\"ts\\\":\\\"2025-06-24T13:38:11.139+05:30\\\"}\"";
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
                    //string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, fullResponse, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
                //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(fullResponse);
                return responseObj;
            }
            catch (WebException we)
            {
                string Response = "";
                string status = Convert.ToString(we.Status);
                string WebResponse = Convert.ToString(we.Response);
                HttpWebResponse HttpWebResponse = (HttpWebResponse)we.Response;
                //HttpStatusCode HttpStatusCode = HttpWebResponse.StatusCode;
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
                        //Response = Response.Replace("status", "statusCode");
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
                        //string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
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

                //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(Response);
                return responseObj;
            }
            finally
            {
            }
        }
        #endregion

        #region eIBMAadhaar
        //public IBMAadhaarFullResponse IBMAadhaarDownload(eIBMAadhaar eIBMAadhaar)
        //{
        //    string sourceId = "FT";
        //    DateTime d = DateTime.Now;
        //    string dateString = d.ToString("yyyyMMddHHmmss");
        //    string vRndNo = GenerateRandomNo();
        //    string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"
        //    //string txn = "AUANSDL001:" + dateString + vRndNo;
        //    string type = "A";
        //    string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 
        //    IBMAadhaarFullResponse responseObj = null;
        //    //responseObj = new IBMAadhaarResponse("", "", "", "");

        //    Kyc pKycReq = new Kyc();
        //    pKycReq.lr = "N";
        //    pKycReq.pfr = "N";

        //    eIBMAadhaarReq pReq = new eIBMAadhaarReq();
        //    pReq.sourceId = sourceId;
        //    pReq.traceId = traceId;
        //    pReq.uid = eIBMAadhaar.aadhaarNo;
        //    pReq.txn = "UKC:" + eIBMAadhaar.txn;
        //    pReq.type = type;
        //    pReq.ts = ts;
        //    pReq.otp = eIBMAadhaar.otp;
        //    pReq.Kyc = pKycReq;

        //    string requestBody = JsonConvert.SerializeObject(pReq);
        //    //string postURL = vKarzaEnv == "PROD" ? "https://baseurl/usfb/v1/uidai/KYCandVerification/aadharKYC" : "https://baseurl/usfb/v1/uidai/KYCandVerification/aadharKYC";
        //    string postURL = "https://connect-nonprod.unitybank.co.in/preprodunity/uatportal/usfb/v1/uidai/KYCandVerification/aadharKYC";
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        if (request == null)
        //        {
        //            throw new NullReferenceException("request is not a http request");
        //        }
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.Headers.Add("X-IBM-Client-Id", X_IBM_Client_Id);
        //        request.Headers.Add("X-IBM-Client-Secret", X_IBM_Client_Secret);
        //        request.Headers.Add("X-Client-IP", X_Client_IP);

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
        //        using (var streamWriter = new StreamWriter(request.GetRequestStream()))
        //        {
        //            streamWriter.Write(requestBody);
        //            streamWriter.Close();
        //        }
        //        StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
        //        string fullResponse = responseReader.ReadToEnd();
        //        request.GetResponse().Close();
        //        try
        //        {
        //            fullResponse = fullResponse.Replace("\u0000", "");
        //            fullResponse = fullResponse.Replace("\\u0000", "");

        //            dynamic res = JsonConvert.DeserializeObject(fullResponse);

        //            responseObj = JsonConvert.DeserializeObject<IBMAadhaarFullResponse>(fullResponse);
        //            #region comment
        //            if (Convert.ToString(responseObj.status) == "success")
        //            {
        //                responseObj.StatusMsg = "Aadhaar Verified Successfully";

        //            }
        //            else if (Convert.ToString(responseObj.status) == "fail")
        //            {
        //                string err = Convert.ToString(responseObj.err);
        //                responseObj.StatusCode = err;
        //                responseObj.StatusMsg = "Aadhaar Not Verified Successfully";

        //                switch (err)
        //                {
        //                    case "K-100":
        //                        responseObj.ErrorMessage = "Resident authentication failed";
        //                        break;
        //                    case "K-200":
        //                        responseObj.ErrorMessage = "Resident data currently not available";
        //                        break;
        //                    case "K-514":
        //                        responseObj.ErrorMessage = "Invalid UID Token Used.";
        //                        break;
        //                    case "K-515":
        //                        responseObj.ErrorMessage = "Invalid VID used.";
        //                        break;
        //                    case "K-516":
        //                        responseObj.ErrorMessage = "Invalid ANCS Token used.";
        //                        break;
        //                    case "K-517":
        //                        responseObj.ErrorMessage = "VID used is expired.";
        //                        break;
        //                    case "K-519":
        //                        responseObj.ErrorMessage = "Invalid Authenticator Code.";
        //                        break;
        //                    case "K-540":
        //                        responseObj.ErrorMessage = "Invalid KYC XML";
        //                        break;
        //                    case "K-541":
        //                        responseObj.ErrorMessage = "Invalid e-KYC API version";
        //                        break;
        //                    case "K-542":
        //                        responseObj.ErrorMessage = "Invalid resident consent (“rc” attribute in “Kyc” element)";
        //                        break;
        //                    case "K-544":
        //                        responseObj.ErrorMessage = "Invalid resident auth type (“ra” attribute in “Kyc” element does not match what is in PID block)";
        //                        break;
        //                    case "K-545":
        //                        responseObj.ErrorMessage = "Resident has opted-out of this service. This feature is not implemented currently.";
        //                        break;
        //                    case "K-546":
        //                        responseObj.ErrorMessage = "Invalid value for “pfr” attribute";
        //                        break;
        //                    case "K-550":
        //                        responseObj.ErrorMessage = "Invalid Uses Attribute";
        //                        break;
        //                    case "K-551":
        //                        responseObj.ErrorMessage = "Invalid “Txn” namespace";
        //                        break;
        //                    case "K-552":
        //                        responseObj.ErrorMessage = "Invalid KUA License key";
        //                        break;
        //                    case "K-553":
        //                        responseObj.ErrorMessage = "KUA License key Expired.";
        //                        break;
        //                    case "K-569":
        //                        responseObj.ErrorMessage = "Digital signature verification failed for e-KYC XML";
        //                        break;
        //                    case "K-570":
        //                        responseObj.ErrorMessage = "Invalid key info in digital signature for e-KYC XML (it is either expired, or does not belongto the AUA or is not created by a well-known Certification Authority)";
        //                        break;
        //                    case "K-571":
        //                        responseObj.ErrorMessage = "Technical error while signing the eKYC response.";
        //                        break;
        //                    case "K-600":
        //                        responseObj.ErrorMessage = "AUA is invalid or not an authorized KUA";
        //                        break;
        //                    case "K-601":
        //                        responseObj.ErrorMessage = "ASA is invalid or not an authorized ASA";
        //                        break;
        //                    case "K-602":
        //                        responseObj.ErrorMessage = "KUA encryption key not available";
        //                        break;
        //                    case "K-603":
        //                        responseObj.ErrorMessage = "ASA encryption key not available";
        //                        break;
        //                    case "K-604":
        //                        responseObj.ErrorMessage = "ASA Signature not allowed";
        //                        break;
        //                    case "K-605":
        //                        responseObj.ErrorMessage = "Neither KUA nor ASA encryption key is available";
        //                        break;
        //                    case "K-955":
        //                        responseObj.ErrorMessage = "Technical Failure internal to UIDAI.";
        //                        break;
        //                    case "K-956":
        //                        responseObj.ErrorMessage = "Technical error while generating the PDF file.";
        //                        break;
        //                    case "K-999":
        //                        responseObj.ErrorMessage = "Unknown error";
        //                        break;
        //                }
        //            }
        //            else if (Convert.ToString(responseObj.NSDL_Error_Code) != "")
        //            {
        //                responseObj.status = "fail";
        //                string NSDL_Error_Code = Convert.ToString(responseObj.NSDL_Error_Code);
        //                responseObj.StatusCode = NSDL_Error_Code;
        //                responseObj.StatusMsg = "Failure from NSDL Scenario";

        //                switch (NSDL_Error_Code)
        //                {
        //                    case "E-000":
        //                        responseObj.ErrorMessage = "Request received is a HTTP request";
        //                        break;
        //                    case "E-001":
        //                        responseObj.ErrorMessage = "Request received is a get request";
        //                        break;
        //                    case "E-100":
        //                        responseObj.ErrorMessage = "Auth XML not parsed properly";
        //                        break;
        //                    case "E-101":
        //                        responseObj.ErrorMessage = "KYC XML not parsed properly";
        //                        break;
        //                    case "E-102":
        //                        responseObj.ErrorMessage = "Audit logging in DB failed for request";
        //                        break;
        //                    case "E-103":
        //                        responseObj.ErrorMessage = "Audit logging in DB failed for response";
        //                        break;
        //                    case "E-105":
        //                        responseObj.ErrorMessage = "KYC XSD Validation failed";
        //                        break;
        //                    case "E-106":
        //                        responseObj.ErrorMessage = "KYC Request signature verification failed";
        //                        break;
        //                    case "E-107":
        //                        responseObj.ErrorMessage = "Auth Request signature verification failed";
        //                        break;
        //                    case "E-108":
        //                        responseObj.ErrorMessage = "IP verification failed for entity";
        //                        break;
        //                    case "E-109":
        //                        responseObj.ErrorMessage = "Blank response received from UIDAI";
        //                        break;
        //                    case "E-110":
        //                        responseObj.ErrorMessage = "Unable to decrypt response at KSA";
        //                        break;
        //                    case "E-111":
        //                        responseObj.ErrorMessage = "KYC response signature verification failed";
        //                        break;
        //                    case "E-112":
        //                        responseObj.ErrorMessage = "BFD XSD validation failed";
        //                        break;
        //                    case "E-113":
        //                        responseObj.ErrorMessage = "OTP XSD validation failed";
        //                        break;
        //                    case "E-114":
        //                        responseObj.ErrorMessage = "KYC response XML not parsed properly";
        //                        break;
        //                    case "E-115":
        //                        responseObj.ErrorMessage = "AUTH response XML not parsed properly";
        //                        break;
        //                    case "E-116":
        //                        responseObj.ErrorMessage = "Signed Auth XML generation Error";
        //                        break;
        //                    case "E-117":
        //                        responseObj.ErrorMessage = "Signed KYC XML generation Error";
        //                        break;
        //                    case "E-118":
        //                        responseObj.ErrorMessage = "Auth response signature verification failed";
        //                        break;
        //                    case "E-119":
        //                        responseObj.ErrorMessage = "ASA or KSA is unable to connect to UIDAI server";
        //                        break;
        //                    case "E-120":
        //                        responseObj.ErrorMessage = "Auth XSD validation failed";
        //                        break;
        //                    case "E-122":
        //                        responseObj.ErrorMessage = "Property file unavailable";
        //                        break;
        //                    case "E-123":
        //                        responseObj.ErrorMessage = "BFD request XML not parsed properly";
        //                        break;
        //                    case "E-124":
        //                        responseObj.ErrorMessage = "OTP request XML not parsed properly";
        //                        break;
        //                    case "E-125":
        //                        responseObj.ErrorMessage = "BFD request signature verification failed";
        //                        break;
        //                    case "E-126":
        //                        responseObj.ErrorMessage = "OTP request signature verification failed";
        //                        break;
        //                    case "E-127":
        //                        responseObj.ErrorMessage = "Signed BFD XML generation error";
        //                        break;
        //                    case "E-128":
        //                        responseObj.ErrorMessage = "Signed OTP XML generation error";
        //                        break;
        //                    case "E-129":
        //                        responseObj.ErrorMessage = "BFD response XML not parsed properly";
        //                        break;
        //                    case "E-130":
        //                        responseObj.ErrorMessage = "OTP response XML not parsed properly";
        //                        break;
        //                    case "E-131":
        //                        responseObj.ErrorMessage = "XML decryption error";
        //                        break;
        //                    case "E-132":
        //                        responseObj.ErrorMessage = "Error during KYC request signature verification";
        //                        break;
        //                    case "E-133":
        //                        responseObj.ErrorMessage = "Error during KYC response signature verification";
        //                        break;
        //                    case "E-134":
        //                        responseObj.ErrorMessage = "Error during AUTH request signature verification";
        //                        break;
        //                    case "E-135":
        //                        responseObj.ErrorMessage = "Error during AUTH response signature verification";
        //                        break;
        //                    case "E-136":
        //                        responseObj.ErrorMessage = "Error during BFD request signature verification";
        //                        break;
        //                    case "E-137":
        //                        responseObj.ErrorMessage = "Error during OTP request signature verification";
        //                        break;
        //                    case "E-138":
        //                        responseObj.ErrorMessage = "Error during KYC XSD Validation";
        //                        break;
        //                    case "E-139":
        //                        responseObj.ErrorMessage = "Error during AUTH XSD Validation";
        //                        break;
        //                    case "E-140":
        //                        responseObj.ErrorMessage = "Error during BFD XSD Validation";
        //                        break;
        //                    case "E-141":
        //                        responseObj.ErrorMessage = "Error during OTP XSD Validation";
        //                        break;
        //                    case "E-142":
        //                        responseObj.ErrorMessage = "Error during IP verification";
        //                        break;
        //                    case "E-143":
        //                        responseObj.ErrorMessage = "Response received is E";
        //                        break;
        //                    case "E-144":
        //                        responseObj.ErrorMessage = "BFD response signature verification failed";
        //                        break;
        //                    case "E-145":
        //                        responseObj.ErrorMessage = "OTP Signature Tag Missing in Request XML";
        //                        break;
        //                    case "E-149":
        //                        responseObj.ErrorMessage = "Invalid Aadhar Number";
        //                        break;
        //                    case "E-199":
        //                        responseObj.ErrorMessage = "KSA/ASA Internal Error";
        //                        break;
        //                    case "E-200":
        //                        responseObj.ErrorMessage = "One of the mandatory Sub-Aua element is null";
        //                        break;
        //                    case "E-201":
        //                        responseObj.ErrorMessage = "Error while validating strSaCd parameter";
        //                        break;
        //                    case "E-203":
        //                        responseObj.ErrorMessage = "Error while validating strAadhaar parameter";
        //                        break;
        //                    case "E-204":
        //                        responseObj.ErrorMessage = "Error while validating strAadhaarName parameter";
        //                        break;
        //                    case "E-205":
        //                        responseObj.ErrorMessage = "Error while validating strYear parameter";
        //                        break;
        //                    case "E-206":
        //                        responseObj.ErrorMessage = "Error while validating strGender parameter";
        //                        break;
        //                    case "E-207":
        //                        responseObj.ErrorMessage = "Error while validating strTransId parameter";
        //                        break;
        //                    case "E-208":
        //                        responseObj.ErrorMessage = "Error while validating strUDC parameter";
        //                        break;
        //                    case "E-209":
        //                        responseObj.ErrorMessage = "Error while validating strMV parameter";
        //                        break;
        //                    case "E-210":
        //                        responseObj.ErrorMessage = "Value of mvThreshold in property file should be in range 1-100";
        //                        break;
        //                    case "E-211":
        //                        responseObj.ErrorMessage = "Error while validating strLang: If strLname is provided then strLang is mandatory";
        //                        break;
        //                    case "E-212":
        //                        responseObj.ErrorMessage = "Error while validating strLmv parameter. Value should be in range 1-100(inclusive)";
        //                        break;
        //                    case "E-213":
        //                        responseObj.ErrorMessage = "DSC signature verification failed for Sub-AUA";
        //                        break;
        //                    case "E-214":
        //                        responseObj.ErrorMessage = "Error while validating strOtp parameter";
        //                        break;
        //                    case "E-215":
        //                        responseObj.ErrorMessage = "Error while validating strRequestType parameter";
        //                        break;
        //                    case "E-216":
        //                        responseObj.ErrorMessage = "Error while validating strBioType parameter";
        //                        break;
        //                    case "E-217":
        //                        responseObj.ErrorMessage = "Error while validating strFingerOnePos parameter";
        //                        break;
        //                    case "E-218":
        //                        responseObj.ErrorMessage = "Error while validating strFingerTwoPos parameter";
        //                        break;
        //                    case "E-219":
        //                        responseObj.ErrorMessage = "Error while validating strMS parameter";
        //                        break;
        //                    case "E-220":
        //                        responseObj.ErrorMessage = "Blank Signature Tag Exception";
        //                        break;
        //                    case "E-221":
        //                        responseObj.ErrorMessage = "Incorrect OTP Version";
        //                        break;
        //                    case "E-230":
        //                        responseObj.ErrorMessage = "Error during DSC signature verification for Sub-AUA";
        //                        break;
        //                    case "E-235":
        //                        responseObj.ErrorMessage = "Request Signature Verification Failed";
        //                        break;
        //                    case "E-236":
        //                        responseObj.ErrorMessage = "Error during Request Signature Verification";
        //                        break;
        //                    case "E-237":
        //                        responseObj.ErrorMessage = "Error during KYC Response Encryption";
        //                        break;
        //                    case "E-238":
        //                        responseObj.ErrorMessage = "Inernal kua not mapped against KUA code";
        //                        break;
        //                    case "E-252":
        //                        responseObj.ErrorMessage = "Invalid Aadhaar no. length";
        //                        break;
        //                    case "E-253":
        //                        responseObj.ErrorMessage = "Biometric attribute is null/missing";
        //                        break;
        //                    case "E-297":
        //                        responseObj.ErrorMessage = "UIDAI PID Block Encryption Certificate Expired";
        //                        break;
        //                    case "E-299":
        //                        responseObj.ErrorMessage = "As per UIDAI guidelines, this device is not authorized to perform transactions. Kindly procure L1 registered device";
        //                        break;
        //                    case "E-555":
        //                        responseObj.ErrorMessage = "Duplicate transaction id error";
        //                        break;
        //                    case "E-563":
        //                        responseObj.ErrorMessage = "Read timeout while connecting to UIDAI";
        //                        break;
        //                    case "E-601":
        //                        responseObj.ErrorMessage = "SubAua Request XML Not Parsed Properly.";
        //                        break;
        //                    case "E-602":
        //                        responseObj.ErrorMessage = "SubAua XSD Validation Failed.";
        //                        break;
        //                    case "E-603":
        //                        responseObj.ErrorMessage = "Audit Logging in DB is failed for SubAua Request.";
        //                        break;
        //                    case "E-604":
        //                        responseObj.ErrorMessage = "License Verfication failed for SubAua Entity.";
        //                        break;
        //                    case "E-605":
        //                        responseObj.ErrorMessage = "SubAua Request Signature Verification Failed.";
        //                        break;
        //                    case "E-607":
        //                        responseObj.ErrorMessage = "DBAudit Failed For AuditOTP Request.";
        //                        break;
        //                    case "E-608":
        //                        responseObj.ErrorMessage = "Purpose not mapped against entity/Purpose used is not in active state.";
        //                        break;
        //                    case "E-609":
        //                        responseObj.ErrorMessage = "Entity-Service Mapping Verfication failed for SubAua Entity.";
        //                        break;
        //                    case "E-610":
        //                        responseObj.ErrorMessage = "RDData Received Null.";
        //                        break;

        //                }
        //            }
        //            #endregion

        //            //string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
        //            SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, fullResponse, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
        //        }
        //        finally
        //        {
        //        }
        //        //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(fullResponse);
        //        return responseObj;
        //    }

        //    catch (WebException we)
        //    {
        //        string Response = "";
        //        string status = Convert.ToString(we.Status);
        //        string WebResponse = Convert.ToString(we.Response);
        //        string WebResponseMessage = Convert.ToString(we.Message);
        //        if (we.Response != null)
        //        {
        //            using (var stream = we.Response.GetResponseStream())
        //            using (var reader = new StreamReader(stream))
        //            {
        //                Response = reader.ReadToEnd();
        //            }
        //            //Response = Response.Replace("status", "statusCode");
        //            try
        //            {
        //                Response = Response.Replace("\u0000", "");
        //                Response = Response.Replace("\\u0000", "");
        //                dynamic res = JsonConvert.DeserializeObject(Response);

        //                if (Convert.ToString(res.Status) == "Fail")
        //                {
        //                    responseObj.status = "fail";
        //                    responseObj.StatusCode = Convert.ToString(res.StatusCode);
        //                    responseObj.ErrorReason = Convert.ToString(res.ErrorReason);
        //                    responseObj.FailedAt = Convert.ToString(res.FailedAt);
        //                    responseObj.ErrorName = Convert.ToString(res.ErrorName);
        //                    responseObj.ErrorCode = Convert.ToString(res.ErrorCode);
        //                    responseObj.ErrorMessage = Convert.ToString(res.ErrorMessage);
        //                }
        //                //string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
        //                SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, Response, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
        //            }
        //            finally
        //            {
        //            }
        //        }
        //        else if (status != "")
        //        {
        //            responseObj.status = WebResponse;
        //            responseObj.StatusCode = status;
        //            responseObj.ErrorMessage = WebResponseMessage;
        //        }
        //        return responseObj;
        //    }
        //    finally
        //    {
        //    }
        //}

        public IBMAadhaarFullResponse IBMAadhaarDownload(eIBMAadhaar eIBMAadhaar)
        {
            string sourceId = "FT";
            DateTime d = DateTime.Now;
            string dateString = d.ToString("yyyyMMddHHmmss");
            string vRndNo = GenerateRandomNo();
            string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"
            //string txn = "AUANSDL001:" + dateString + vRndNo;
            string type = "A";
            string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 
            IBMAadhaarFullResponse responseObj = null;
            //responseObj = new IBMAadhaarResponse("", "", "", "");

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
                    string vFulResp = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                    vFulResp = vFulResp.Replace("\\\"", "");
                    vFulResp = vFulResp.Trim('"').Replace("\\", "");
                    try
                    {
                        responseObj = JsonConvert.DeserializeObject<IBMAadhaarFullResponse>(vFulResp);
                    }
                    catch
                    {
                        vFulResp = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                        vFulResp = vFulResp.Trim('"').Replace("\\", "");
                        responseObj = JsonConvert.DeserializeObject<IBMAadhaarFullResponse>(vFulResp);
                    }

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

                    //string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, vFulResp, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
                }
                finally
                {
                }
                //IBMAadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<IBMAadhaarOTPResponse>(fullResponse);
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
                    //Response = Response.Replace("status", "statusCode");
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
                        //string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
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
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 30, "@pApiName", pApiName);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pTxnCode", pTxnCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pRequestJsn.Length + 1, "@pRequestJsn", pRequestJsn);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, pResponseJsn.Length + 1, "@pResponseJsn", pResponseJsn);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoID", pEoID);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pIdNo", pIdNo);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
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

        #region LUC
        #region GetLoanUtilizationQsAns
        public List<GetLoanUtilizationQsAns> GetLoanUtilizationQsAns()
        {
            DataTable dt = new DataTable();
            List<GetLoanUtilizationQsAns> row = new List<GetLoanUtilizationQsAns>();

            try
            {
                dt = GetLoanUtilizationQAns();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLoanUtilizationQsAns(rs["PurposeID"].ToString(), rs["SubPurposeID"].ToString(), rs["QNo"].ToString(), rs["Question"].ToString(),
                           rs["AnswerType"].ToString(), rs["Answer"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetLoanUtilizationQsAns("No data available", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetLoanUtilizationQsAns("No data available", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }

        public DataTable GetLoanUtilizationQAns()
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLoanUtilizationQsAns";
                DBConnect.ExecuteForSelect(oCmd, dt);
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
        public List<GetLUCPendingDataList> GetLUCPendingDataList(PostLUCPendingDataList postLUCPendingDataList)
        {
            DataTable dt = new DataTable();
            List<GetLUCPendingDataList> row = new List<GetLUCPendingDataList>();

            try
            {
                dt = GetLUCPendingData(postLUCPendingDataList.pEoId, postLUCPendingDataList.pDate, postLUCPendingDataList.pBranch);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLUCPendingDataList(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["LoanId"].ToString(), rs["MemberId"].ToString(),
                           rs["MemberName"].ToString(), rs["LoanAccountId"].ToString(), rs["Cycle"].ToString(), rs["DisbDate"].ToString()
                           , rs["LoanAmt"].ToString(), rs["PurposeID"].ToString(), rs["PurposeNm"].ToString(), rs["SubPurposeId"].ToString()
                           , rs["SubPurposeNm"].ToString(), rs["CenterID"].ToString(), rs["CenterNm"].ToString(), rs["GroupID"].ToString()
                           , rs["GroupNm"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetLUCPendingDataList("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetLUCPendingDataList("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }


        public DataTable GetLUCPendingData(string pEoId, string pDate, string pBranch)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetLUCPendingDataList";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 12, "@pEoId", pEoId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pDate", pDate);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranch", pBranch);
                DBConnect.ExecuteForSelect(oCmd, dt);
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
            string vErrMsg = "";

            try
            {
                vErrMsg = SaveLoanUtil(postLoanUtilization);
            }
            finally { }
            return vErrMsg;
        }

        public string SaveLoanUtil(PostLoanUtilization postLoanUtilization)
        {
            SqlCommand oCmd = new SqlCommand();
            Int32 vErr = 0;

            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "SaveLoanUtilization";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 15, "@pLoanId", postLoanUtilization.pLoanId);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUtlType", postLoanUtilization.pLoanUtlType);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pLoanUTLBy", Convert.ToInt32(postLoanUtilization.pLoanUTLBy));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 200, "@pLoanUTLRemarks", postLoanUtilization.pLoanUTLRemarks);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 20, "@pLoanUTLVia", postLoanUtilization.pLoanUTLVia);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Float, 10, "@pLoanUTLAmt", Convert.ToDecimal(postLoanUtilization.pLoanUTLAmt));
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pLoanUTL", postLoanUtilization.pLoanUTL);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pLoanUTLDt", postLoanUtilization.pLoanUTLDt);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsSamePurpose", postLoanUtilization.pIsSamePurpose);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLat", postLoanUtilization.pLat);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 50, "@pLong", postLoanUtilization.pLong);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pIsPhotoUploaded", postLoanUtilization.pIsPhotoUploaded);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Xml, postLoanUtilization.pCollXml.Length + 1, "@pXml", postLoanUtilization.pCollXml);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBConnect.Execute(oCmd);
                vErr = Convert.ToInt32(oCmd.Parameters["@pErr"].Value);
                if (vErr == 0)
                    return "Details Saved Successfully!!!";
                else if (vErr == 2)
                    return "Loan Utilization Already Done";
                else
                    return "Failed To save Data!!!";
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


        public List<LUCImageSave> LUCImageUpload(Stream image)
        {
            string vErr = "";
            List<LUCImageSave> row = new List<LUCImageSave>();
            string MemNo = "";

            MultipartParser parser = new MultipartParser(image);
            if (parser != null && parser.Success)
            {
                foreach (var content in parser.StreamContents)
                {
                    if (!content.IsFile)
                    {
                        if (content.PropertyName == "MemberNo")
                        {
                            MemNo = content.StringData;
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
                            if (vFileTag.Equals("LUCPhoto"))
                            {
                                if (MinioYN == "N")
                                {
                                    vErr = SaveMemberImages(binaryWriteArray, "LUC", MemNo, vFileTag);
                                }
                                else
                                {
                                    vErr = UploadFileMinio(binaryWriteArray, fileName, MemNo, LUCBucket, MinioUrl);
                                }
                                if (vErr.Equals("Y"))
                                {
                                    row.Add(new LUCImageSave("Successful", fileName));
                                }
                                else
                                {
                                    row.Add(new LUCImageSave("Failed", fileName));
                                }
                            }

                        }
                        else
                        {
                            row.Add(new LUCImageSave("Failed", fileName));
                        }
                    }
                }
                return row;
            }
            else
            {
                row.Add(new LUCImageSave("Failed", "No Data Found"));
            }
            return row;
        }


        #endregion

        #region IBM Aadhaar Encryption and Decryption
        public static string Decrypt(string strToDecrypt, byte[] x_key)
        {
            try
            {
                byte[] keyBytes = new byte[32];
                byte[] ivBytes = new byte[16];
                Array.Copy(x_key, 0, keyBytes, 0, 32);
                Array.Copy(x_key, 0, ivBytes, 0, 16); // or use 16–32 if needed
                //Console.WriteLine("Key: " + Convert.ToBase64String(keyBytes));
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    byte[] encryptedBytes = Convert.FromBase64String(strToDecrypt);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during decryption: " + e.Message);
                return null;
            }
        }

        public static string Encrypt(string strToEncrypt, byte[] x_key)
        {
            try
            {
                byte[] keyBytes = new byte[32];
                byte[] ivBytes = new byte[16];
                Array.Copy(x_key, 0, keyBytes, 0, 32);
                Array.Copy(x_key, 0, ivBytes, 0, 16); // or use 16–32 if you've split them
                Console.WriteLine("Key: " + Convert.ToBase64String(keyBytes));
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(strToEncrypt);
                        byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                        return Convert.ToBase64String(encryptedBytes);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during encryption: " + e.Message);
                return null;
            }
        }

        public static string SessionKeyEncryption(byte[] x_key)
        {
            try
            {
                Console.WriteLine("Key rsa: " + Convert.ToBase64String(x_key));
                // Load certificate with public key (no private key needed for encryption)
                string vCretificatePath = "~/Certificate/Unity_Cert.cer";
                var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath(vCretificatePath);
                X509Certificate2 cert = new X509Certificate2(vStrMycertPub);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
                // Use default padding (PKCS#1 v1.5)
                byte[] encryptedKey = rsa.Encrypt(x_key, false);
                return Convert.ToBase64String(encryptedKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine("RSA encryption error: " + ex.Message);
                return null;
            }
        }

        public static string GenerateRandomString()
        {
            var sb = new StringBuilder();
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomByte = new byte[1];
                for (int i = 0; i < 32; i++)
                {
                    do
                    {
                        rng.GetBytes(randomByte);
                    } while (randomByte[0] >= CHARSET.Length * (256 / CHARSET.Length));

                    sb.Append(CHARSET[randomByte[0] % CHARSET.Length]);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region RSADecrypt
        public string RsaDecrypt(string pData)
        {
            // Load the certificate with private key
            string certPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/BijliPrivateKey_Kushal.pfx");
            string certPassword = "ftt@123";
            X509Certificate2 cert = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet);
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
            string encryptedBase64 = pData;
            byte[] encryptedData = Convert.FromBase64String(encryptedBase64);

            byte[] decryptedData = rsa.Decrypt(encryptedData, false); // 'false' = PKCS#1 v1.5
            string decryptedText = Convert.ToBase64String(decryptedData);
            return decryptedText;
        }
        #endregion

        #region GetBranchCtrlByBranchCode
        public List<GetBranchCtrlByBranchCode> GetBranchCtrlByBranchCode(PostBranchCtrlByBranchCode postBranchCtrlByBranchCode)
        {
            DataTable dt = new DataTable();
            List<GetBranchCtrlByBranchCode> row = new List<GetBranchCtrlByBranchCode>();

            try
            {
                dt = GetBranchCtrlByBranchCode(postBranchCtrlByBranchCode.pBranchCode, postBranchCtrlByBranchCode.pEffectiveDate);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetBranchCtrlByBranchCode(rs["InitialAppJLG"].ToString(), rs["AdvCollJLG"].ToString(), rs["CashCollJLG"].ToString(), rs["DigiAuthJLG"].ToString(), rs["ManualAuthJLG"].ToString(), rs["BioAuthJLG"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetBranchCtrlByBranchCode("No data available", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetBranchCtrlByBranchCode("No data available", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }

        public DataTable GetBranchCtrlByBranchCode(string pBranchCode, string pEffectiveDate)
        {
            DataTable dt = new DataTable();
            SqlCommand oCmd = new SqlCommand();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "GetBranchCtrlByBranchCode";
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 4, "@pBranchCode", pBranchCode);
                DBConnect.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.DateTime, 8, "@pEffectiveDate", pEffectiveDate);

                DBConnect.ExecuteForSelect(oCmd, dt);
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

        #region IBMAuaKua
        public string auaKuaAuthWithBiometric()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?><PidData><Resp errCode=\"0\" errInfo=\"\" fCount=\"1\" fType=\"2\" iType=\"0\" iCount=\"0\" pCount=\"0\" nmPoints=\"36\" qScore=\"86\"/><Data type=\"X\">MjAyNS0wOC0yN1QxMjoxMzoyMKvLkuUC7ytD1uluitz1xCje76oQoaB3FkHiIziCKOS9tqxa97GPBwdGuf2LOk2xdINemDRdpou6INTMSf8WL/laqHukivQZPx1mpvWAnoDwGvjffIWZVIaUWeWY8uvg83yGIdFNmyaV7COOrdaHh3O+24wm6FjP0UhLNJjnKB0Gj7ojz+H8ly4CdTwDndGcfiZdd50fQVnGrVptB8B/gzzpH+4Dqj7/bsWxuizkza+qJRjrrQ/M+NamsuxzNJsJ4C8zn6s9hixiXnaUBdQiCNFmtCS4XO7ZFU95Exu/poHo4/y07tMe9K+USJHYDEwZ6hF6J1l57aBojacbVJCk1LmFalBZ/w8yJcxectB48OCU2Fp5zd65HF+n6cyNR276O9qM1BD9q+4eoMpTJmKl2eqN/NUuHNZisjT/FU2l0l4/t1+4sj6biSZO0ZMTY7z0LfDVIAnEOh+OvUFohobchpfinEpm5PIWuff0wD0nzpV+i18GVNcKvG1ndDN+b3e22UECm5CgWQ+YSP6pLShWRtI/XKA1eqvITZmK9r8C3cXsIpmv/FeEVXzJK5cqHlxPRSw+5ZgDLtjgHmac0qDpMyyAFfwo7lezaWaV4ANu0Lifg8IgPxo1jT5PCOaZ/cw4FhLqWrOU57DdFA7qQmATOebtrzqETt9/NYx+gNkziCGx8P74K6sMsqbAttDdcMpxaJZGMvjyraeUvEH5PqJKNHRvCm/0kmsJ91Av7rN/KJqTM5locUe51VpzMNCutE1WDfkJ5hB3neA/ETHvwJID2Se1IIVhXihNrd9fGbw3TtyhaPMFXZVFLdX8zeU2muNvuE7OBDf54dRoLVbD0y6+2vcAZKquSsf5JiEXKe5ThUuD6Ndx7nn4r04ine+HB/GyMiiOqcdHZklxMKjo+19kd4F2FyWlQX5I9n4D+3IQMoBYOEg7dv7GT+7pZrxIQfoVPZv6WSVmdMBtNI/clO/67xlSslIIhlMg55+K/u8BwPcbSGnvG8cVPsfJH7mlsqKEtKPJjMTl6qILB2rVtjV6U1qRLT1daVFGDXyT2w6kNAR/l4PAseEa1v12kkO5+KJANcnJ34xkeB7BaE7fzueNIoLMPVUMkwWeyxaTc29igY2SsQSjLg28tnyD8DnynSDGcGLsF0UF1VO1dI69MiOZTyPJzSTXuK0OAe4Tw0Xd6e+L0Gc+mfSJnKMFA0EJOXVb5vUGMrOFdk+IuACS8vQNu9q55Zs6qL9+nC9MeR6obWpnjurKVJSj4ivs56A7pKCuEm2xG8v2pti40gTARsYSXEeG18MnetqlPwT5NiFXU+pnaDdXmDIYg2QDTicifOH6fQZ3Zcx9ZlRCI7UMiDQECR+MrJlh+Wr1+pOZHKmcqqF+mOos2aeZmeGYn9rQutllxxtssYpnL0HWWW8GhwSXlQhgE57WTB8nopR9qRcdUlf/+B+pASQxfz+/GDaqoKYaA7d1ZsvdwXNvP4hC1Zde4iPhZ/atBsW8gai7tam8uaUuqY6I7hxfs5D8/5r0GmiUEgmXkhRAggX/ByQCxLFd/edSXRwbjgEQYBWnyOeuoPGaz/dB7SdoLGK3QKGbOxFDd9uG8E0g58zF6TZAjqtdl5J7HpnUhc6UrpKOIGQlJZxenMl9iIaeSSg6kRgczf+Z6TOs06Ez0F5trXtBb2qI08aUQqtxT9w96oK8XrbHAANm6xEZwpNEZ7KfWfWq18crTikXQvgKB9bB7NkJ6HTCx8xedHZ/EOCPTkx7/ssoLIkUXHxolDY33WsaC4BXx8er92Gg6IOFdAx5adwrcHUnIdSChifNmHHm56QVBi7zKg2hxR6v7BD3FfGsLmKembOB4lRfR+RBZZJG8ULeL5/V4R4FsR3M1oK/A3cil5h4gIE1lwhqmIrwY6JhJP7Xb3hGTFrSKVAavR1mJutE3nmVXiskUTJXT6Pxyh9lZfqwennuyG8zIC93MoAqRmaQ06gHfIW8zYM8PLaHny85hn2TS6ZK4YPnlNboZq3T2ZnyfnH5B3tJAfZfK//RMKVGUBfRqS/YGES3v0rRe0cA99DAwZrDal1VV1DW70ersi6vsjpb+JJBE9dj+lRTMkMOV62atEKuf5v7jWqG9UWLUMQtBaOBcDlaQ7W1gKIWcMAvBRlyGsGwXvVjzxaDWkPlVi+ud+kNzWc/qOrWh6NinpLLwewB6KFlnj0y9zA1eY9y8PWcY5JaJMvv6gPs8uoiokJpM6YWsd0j1Qe6gFSjCZXxcrDUCipX6aQYYhCI2GMujalDynB13QBXrDEjbkLR1sdD3jFn5OpXxz/Jsw6IsWNLSp8vaO1KZ9w8gLlT909jcbaXKM/X2/6Lw52CXXUhayEDSrzaRNBfeLEV3jbx3Dol9xNPKU8MNuWWJJqxtDKrJcUEB5q1IMkHR3C5Lib6LfqGFW3GX8XFgF30Hc5qjeDW/FwLnoYrGmpKxlmxoYwYQOWwdAlC+Ycd5BaDv5ilJG/zuI3bs60ROJwc8X2mpT7nUY91NdsIoNNdoefq6AQMJowvbI1QFsT7fDFdkovSFn1K7RY550GeH1y8vFpTgQHzsGZXvsGvF8lMJq8W14kF/PiHCrAT+vrUKglPy/DkduQxLHAxK4bgeP0jnb7HcwPV/22IsaI8gogqyPFw1OHQvyNJIfYq90JXZFm+8nxO8FkVHHFrzXNhdS0iKJukWXCWuAUoggIndW50ERtfKeNIg5Gjg2heBUeLdAW0LogM/pNkXjiJvc9IeQMfi7NixN97rVZypVO575yQG+u2/+4GC/brK/NAaow7igwK9XxrY8VoOgvueZgVcy1UG4HDmGXvklAxEVslgIJIdAVtGXNLhi9/cu5CX6qXgeW8XjyKzTxa42kv++2sRXQLb8fsA9WMot/p/PzrHOq2fgYZcNube/ghNb2Etso/s27D/9lYNeIvWIGo5xmvWJU/SYo7KUvTlXX7tY6gDYo8O7wCqUjHocyRXtvCHbSAmsD+Vl7EXPc6yD+XYiRN6XGx9rIfrVNYlrYkb0I1irRTba067Bv6OCF8N+UJEldMVsLGc4gvkdFLGFYrkB6XKJU6WztoxzfdTcSiH/wwqrjd9f7voFX4Ra0Pygz+jPUObuzAXWs9P9enZ91pl2Wn88k8F/kOXKF/3wFK8I7l8TqIXk8ys5KgEGm0E0zZIXqUa6T0XbixGyZaxiXDMXZDxyf5rwZcPQwR8ePVwxBZRgp00M633+Nm3WycXQ9h1PQKMZ/yajA1clr8Yo05wmRGTa0iIONnLBMfYxFbm/xgkRDSb/pEsOe3iyBvnwTsThG5td6qA47O0pTLTiuHBefs5/Ow990B2cS6x94ireh3WEhg/Z6CG8nUpXGpi+NPcOJwfb04fpurvb6D8l+98CmmeV1+y1KrQIzH0D6HqQqFg3tSn++pCL9+sSrsdR03/wckRTWR/rRqt2NRJley/sqnDP8KZd7Wh2zPuYJFqVTQ7CyPiX3OnnfG5puquTzRCk9xINp+k3sE8KRBVAEPZem3Z/dw+RRF9/EfkL1ONk/WfppWiBz3KAJxqk+14uvBIdVSLxGkrIwxFKsDtelJsdI8Dqbe5CMVK74MUZHSaSX1d1w2zmN4LAgeuqZxqwSHi3afDB/fUI3s3dho5IkiyTuJ4J7OSmWjB6ewHpQDKVKk7uyrBlOLVAV9o05o+1hc+ABPLw8A2Shcwwheai5Aw+DcqCLXlgMxB+e2r9Dw3F6twQM/Ny2RsgeFGTjpT2t6dPG8On3aHlZt4A3hWGNh1aMs3Yb33FoYGsiY2LNxU3qjyLEjzpROeSgk27XWuC7shFlnP4IQ54es5kwN5plxcdZFq6zZhaVrMCAoMbau3RffTqBwBCYhKWXw96+hODy3N6IS23pcMW+j4KOSJM0SFs/a6sOfyHbU8ktI0TzzKmmR2FxOqyqst27JyoEpE0cSPubY9LmVFYM/Aar24BRzKHA2QnY2QUcU2tGSQtljF9EcAs5iDru8270/8XryZzHdAehgkzYtdEwySqWEKeSbyCINT1AP+r/R7c3h1UoieevNJ1FgL4z8Y8TqIhnGXhsc71ghV1sAURREDYEj4xyDB9mWwD5+NmVqohFDiM/0t8DLk8KoTdl+fFXtr9WGnVr65Fp2clucmwn1nHbjhy0EWfLdV9r6qhUug4NshFrvWiRYNcaKWzFEY6l1inzlfhlR+GmGCd5uSBIREYKtLSddOCniWk9TdFJIiK2GhBab0lZvvvrqMbSWnt9vgC8SCj+6p0pODpsS5DpZGOBTVNTAEaroMHXed7oKWVYKZFtdPBAgp+7bQvCTRCSOr7q5SzFH6XvmEmx/mxWGKb2HO5qSTfA578y45qb2SyDJTWDr0akCuGdX2bjYIgK7Ua61XCYreAt3nm7TlVjuTaI2ZypT1Gb5HITPwod8/84aasyDs08U6EwkZb43IYAUTTRQpWufzWkewValwi4l6QaeEDqPGVKWFgk6q2zll+x+68OZY5RCbizvKoV6N/lhhqpmgSR1x1XESqV6vJpl+w9IQxlqsO2Ee8eebXt3RR8XOz7G2PbXxGSy0XkL4kTnO4Zcwtw2OFKI1aDyuSxWUhrWIbYrCu5kk7u4q92/LvIYugofTDqE99PgOtTyOYTcZv26N6fODEN4XpF0EbSHGm1t9qyENo5Y0vR5GApZLVPcfnM1xl9wczpfrjNkxNzewwVWmwfJLLWCrDPOYjd6eMTGkXgKzu336pG60rd+U8EUEi/GYX91BWQzuutrfF9iJmhnS2nMWdbU56Mvytmu9Nhe17hxFhRf41P4AlE80fwAq9ib8vsMDSmF8b+HZfBcmEQe6f77/ew72xovPRFuAOZF3dektjbPsG6ntZ9MtRPqMIoMX7Edou+fztXiXOe9g6J/Z6xcoouNX15G8bbwyDHJsTdUWc67Vh3oJodov7n6S6V1bfdiTb64no3MahkkjfH2zuvI7x4AZ62KenYg3dVGXibtEum1iVeEefZX9yVxkxwYah9DhAKqSGyuhj35tClgNUUuVk24QXAJfmXN7+0rLLfAssN0NY1HmCB8z93RjXdcWPHI48zAO7Nm3mujWnuBLWplnR9HKNto7tE9/B7g9njoRIoGUMl2e7KKlLB8tb/sR2evyHmNnenq85eNM80+voxRcMC36W20ZrAY5+6uUJWby1dy2LRHDbrT+q5eq14cSO+mf3UZqli7u7zZdIoQgI56n2+Y+zsTfZ5J2rorkKZZVYkhQ5PpOUbT/Gy4qPMPkHdaeBLjdLGSzcdxy8n74fpY33QWlQTvokBveNBCXNAm+hsIfZrvxjP9RCQ1REmm2LCbyFclIH0wYRqeRfLDvSYwmdxjsXxD6C1X0CgFPKzeBuFn0zHp8UB1b8+SouF9pgd+7EjLxV9HCOH2NmoGuT4oBJetxcwPU1rF6MPY96JZPYulHYU7ATPi1BK/0REXOiORHWA/Cmwz/6sst/b+5J7qamcuOnhHmbIh+XiiRV3nMtc72Rx9d8Q2mJZf725N3npCYQxdykSvX1rV2Y3wzW3FhLpI4K7O5mOcZZfz/RsW51hnleY6FMKez6CWZ+8uE8z6T1VBsnMZhnEC6u+jiY//J74OdVmqHpuwxGckM7N9jpVVHYHwTEurjGsPLt1nsflqDFmm7yzPfItpjRA3R3exSxF20Tn3hft0WdhsZJbu1/qNhHE/jFo1a0JieYREwWxUInJC8LgFFuzxH+K+5y9xxWfMCLDQ9cI5Ydz3HvcOk/IYAx7sy9/AGI/ZxgfKEJB8wicvNkBN4vuszBJC4lwS5aOSzPZWgjiAngYnWwCDI5biuGiyiaettrVUIX3IQNqm2PSSitsxFXSZAp39HCwTlPz4cjx2dZzXyR32Y68MVvXa3AHT3ZWZZmahW4a0wf26b+LLaHHMhufzBqtaVEDN9dZDDm9J+s7kTWWeCb3MwLuKfs/ucGydqK7d5d4gHEGR6dfCxNWY0KiFsIYo9PANdTEdNdxl/x3CMB1/O1JLNjNhsDXQtJU9QMzJd7Ft1jzCiu0Q4la8vMxTaPoZpi6wsutrzqUbabdYBkhHcfNLvxHQGA27jtM4ZjwPjfTY0tTJkuQ2pMLAOjYx4LFChQneUk/UFImMF8Q5ONQrOo8pjM0QuxmkAzQ2YMVdpdRQFE490oT9G4VWOzhZycpxK2aBbz/EHeIzM+PwAVGzl+s7j+2jbZuwuLl6ggevrHIfe0SVwcmGjz5C5x3K0dFZtgCYjYf8xWW5PEd0iWVZpT2ZeD1YH2tyZFWPdP95f5NqUQYGdu8jjeeA2wOc2exE0twQVD7sfFfOUnS5n9QKEqe+QZJVYcMDpEq0hc15SbdDOeMxvuU0pJsRxUyUtS1lMgEbbgKSIKmkyVeemZKyJIvYX5xzkbKEK67/nDxQ25Dgd2cbVedNovA6g5A2ZohGv1h84z1RsZMv2zoBykvQHzYm4CJXCa81DF+PQb5zDAJPl1voBQY9IATUCc591zAszWfrn4tmz71aoizN6hCUec5zK+zeWitoqpkR4DhTETadGIk7KL8VYVok2YyoUb18JDzkJCSPXIItqRevaLPrnsYvnbRVQY26zkRjKDAdJGON0z+J60J0fg+Tz+fldPcL/CZktSXuSiO17EE8iZzbI7OnRIDf5kxsYT1Y4FHcj5AcZP0sjEslF/Wj/RB6+JxV9PVl9Oa3BUa1tDutt+jSDXvw/D5kH5lBx4jSU067N9jzKbhoSXhaL27yW6WgSiA/QKYS/jBux4ATD/AVy0ZqzIpwVtQOm28sZ1S8MsL3sYz6Aav6Ri5bap+NDhWUsNSf/frJYV2epzOacMEL4KOcUnnBfPTgrqPmS4CD6/y+gN9ztMjjDe8j+i5np6nlwXhRIE5MBZTGCiEGtw8w8+28I9ZhR9dBpxkTY6lqG98s9fOJGRIl0MNNrOJJK5NKYdzLSLymMvoZsP1mSrWNOtG4cJ6TZGQJ1ZgBHaucY/HkansNa8NBiUWbuPodQREri2c5pnV0ov/1Djn1/6kPxbdeBKpccFZNA7nye7QGId3zQr5cFp30JBfpTsBbXhtXMukjSKIyCyxRqHYF2y7y4M4Ay1dHaFfXAzXj+zISpNKz8gxvnevIdNymSuwz281SnI0VaCQJpI+MCYOXoXLVtUmsGTgF0s/7oH0AtuGmOd/trrFA6gxEFw6bCCQnVxN9WctoWwoxo5eNt5aHTqauoZUUBIls6Us+LMAydJYJ/WofefUA83d7QdaanD5UPdhys2aJIrbwccCS4c99XdcR5TTNYQwD7q9m76s2BYo2rMJ40xXC5ZKwgpKXrJM7Y0lEMfI+25k8kSTxguTnVO8mXIIzGds1ywNSu8fVT+EtRYmf1rMF45ALXTM4FpS228BwL6uU/0fwUz51+DpXPUTfvObNip3Qr+T3Iqu78Ay+38NUAxWYkItmr4BFivRd2Z1AOM8sYKWJ0poq1mT9jMKRoi7sIVUxu5U+Hw9JrWlQhyabVi37Xsjxj+N2WVZ6ixWh5+lla44D1LbUXcGTD+xPZv9FSgxu8Jf8Y5iOyOWe1mrGFNECuHu0DsE/QSCpmmi4m2N8W/+rz5RtPZW/Fd/I/b6xrzQVpsEKy+i/fu2lGem6458Rr46LBAfn0PJ4svWDXWw7ZxI3pQJH/3pwfZUjzPg05BmnhRqNiXa/WQtK3DbQ1MkRhGGchXggWlTwaeD9woJ77KMRuHUFqjTDvDMcmHEhj/5wmgC2UVIvVYkdfQFNRgRR7QvwWfKg1o3cbbzQMBdbWdzs7SzhTqR2nrd6xti1/ZAmRdF8ULHsqk19uj6ihnRgdxljiUXBENX1xiiDAlRhvmmGw//QJnLrZAZHRK1wbK7Mj2e8VFuNmkjRayjL/2USHjzFPZ1zs6JG8akWHIFUD91SVMzx6wLg+XySjoU7qcrggX+5fG6zpBRL750OR1U0ZS9JLTfFO0NCyAUyPliXFNRBEwLRushN1PDJr8nAQPFtkXvLoFAnKWhhfKjjnjlzP33RM/Yt3JFbAI4UoeoFd6Et94P/gg9sWVW0st16Bz7CRDlw4yPczZqMm3V0PMbJOpdzS3F7aonF1KoLUBeKepNPmV6IIsecVrQyTjpWZ+RMTqjiSYv0UlrknDGBXiD9mrX+5AtCk4EK2Jz8TGJcVq1uG2RMfPaU3xbrdkuzb/kS1uVlbQ3D0jrzheqPqKE2w7xi1TgGsE1GdQEnKSmvfb+2Hj46zUPPWzT8uahAdFH0S18flFnhWwUGyB8q5lD/NtaLnDo7ptDG32JPVDH0/6Cvaav4r5L1P+rAw55RwUfMVjXgatQFz91Nt6HztfChQUqM4YLHPgxmWUli6xCrP/Zmg50r2FFqeb2YU1WRlmooHulSVJ3ef4a+jxwz/SOuNfF4ZHX6yOcEsEZpDLMi5DNIRWxmhdzovVAVaBviR+u3qwvVJl8yjYvSnxuW4f1cwbdFNNe+rHGqJ7I9YqadDthpFklvHMsutJrcK74HAkZi4lpbdW1qDcSgisxq5kaoAEYzlL5wZLzQS0phBcDLfYnOXBcCjOVratsw/+w9nKc+xokNEVzO4BzkHuLBSNo+4KrF6+nqxLQ5xZkc975DXePIS+mff5L3D6B6nsA5SbSVHnE/47AekHv86TLJS9az7KybVIqArbMr8aNjVGX7DfqAPpIpyx6tAJvy/QeAS6UA9eNei/cIrxzVuXPDHX7K1vHxwaECp01CpVT73eLI/CYML8W9RRESIvc2SDpkIZ72fPimIbPbizNUTubuvOKG+FU39x/tJ1faZibcQcVy1kMW2fWIKl51v1XQ5efT5l5eeUE9Lz4kX6ZlCpfueh40RtF8PqJ/9u3LFZxpLl0XIVdxiZ5aP+stUv6cado6Mqudv83r4k+kxbajeUYYvoIzUeZNPW73wKEDx9fzPWhblCbQWyeeenMu3/xicK37lyzMmaMyQxzcKIWerRw5bnsmO985yaBAbDv5BZXrmcOChAgqxK0p7BONcMucsxO24ZWiJ6aT2szLKIaPUITvL4sb86GU6eFK8s01fzKTp9po9hYFJBmpBVQnptvTB4n1vJhJ0O43h2yPml1fpb3D9uHlCbtELs+gMO1lyvqJF23xeiDktT8YKelN1kRaVtd+4Pw6qP0w9w53WbTvZuvZP6HvCVox3EsGkTfUtPMlv5HKmqoIWjRTzFCkcnkzo98C46KXRbt1oTAS+5PDmOthrYLk9NroM38YZ4d8vbownA7KQVWII/LD/rgUfPK2pKkpmdDast1VD0HTZRKYdszHRkEL0uMXAsJluObxvsxj7B7wtHYbXO+oCE4WGTDHubGJtCfLi7sYMbN6J8LkPscxjSUJaV7w1+lXY0MaeKEWWreoxs4FC4TNVDJA77jr2+ztVQ9iW7KIec8V15uTkuN99iBGOJNRF4FFlQN9baujP0vFEqAZ1dPsntb0qNef5UBT3mQKxx4bs6A9Da6aDO4+iQy/WbfNeCzDKSoUQjjEA3zlf36E7BZK1OFoq7Ii0odQY/e5L7KgWQrpYCiIxcWxiwM5CWGRXEef/h+OthzTIeu8EkyzMhh+kG4iKseTp8/phMj2X4d+ZdZInIlbLfxEoN79g5tq2Q+wlaGNiryUWBU1JX7g/C1WqAU6NstBd+f0ixIiQgkKFsw2mBt9qZ8q73nyLbRWvMUtVWYmc7XqFRSQ6VfPCYJ+CYCVJ7jrfQcYvS7ijW8GVJ6V5BumwVS4jh/5ABSdthTT6VYQqOvXWd86RL2PiRlAcx/psWeb2W6G0vVysXWXIsMNeMnT/Ri3dvBEzUGOjG/m163RstzBvKOFB0+dyjauyQM62cO1Wup8QvjE2WtyKa1Uf9xbrctXsan9eIRA3VR0BccLLGm2sxKretfeepuZO2LG7IOcpOfY4p2AwyAedLL0UvgoAK1Lza8lMQ26qFOjUXVKq5npgBXbmV/vmrgEayTDaKflGIPiRr0/hxD5JyWEshXbAbTMuT5UidRtyiJTX1SYwSrdr+Tz/qCxaTTY++e1RWOczqDRyq+8LIzMaF6h1cnZP1PlSEvjRE6ZAQbEpAHuCgzzau4lKg4ZED6cGMcJpERdy+2J0bxdtmBjeZWW4WsNRfYpoqLK9gvL/muiYnxul18KhuPAjHjHOZ7IsUd9UNC+MjtAFJesPx6sxIt7ceot4+rGKoUYkFIDcfni3G5fKKdSAGQcXkzdskJWbRaLJ/sXoUgrctkgbKSQyXV0qU/DiOZELnEM5Z6FGmi5853IU03TIqcb6dyaJcVReoNDCjvYe37X21CWQQ4CcbtqCmJ6itwLWb5l9uAATlbmLr7et8yO42k92ZEL1cqmD0ii89vdeQvDFIf2BWNgh8U25owO04B8r6zDB55UhkNdfyFQfBPWAwBDHfhuAvAHsBfD0y8/kPftZ35GGsZoJg6cy6hidabMzrZxQsC+E3rjNtcIvu/9g06YEndDhf0TS8njyKdOP/JMamJInMLR4EMt5bvGfW0BdI8JtD6fp+/Exsb0mx6In2qUEiG1tNRvosYGyh7BwTODe15kB6wzanfvuFaM62g5SMxsZPj8kJV0sxC/KfCXjuxKidSHSk+/0stJUHYoNxp5MxU9wcqM2jW/PNPnn8VGu9YwKsmWeGL8DuH+xWwrTujg/pDs1zFvBba8xiBVLvi4GymBV3aAgQC72Hyko4R6v1oI9FWEXVxhYJ/MKqng10wBkg+4R793moYcLAxYxlMtE6gumox9v+4SNnyp95JgsXW0yzjND9Cl8dVUG5aDJ9mE3M0C3gBywEzQ29/gQ9QG9918wJ4c4rgOt74NpUyPPpJBundHg5WO32DrVkqkvhAsj/SnkKgHuQpNo7GRLLhFoBtxvNHyE6HemgbflmOOjQo09mQiMTd1iE/unzFqAwNpSMPyQQbN4SICzIL7SnILhrUqaB+kixIIVgo6lQAcl/UH/vwNNGblBPR1/3W+pDfrtxf/xqepEDBKJZ63qgtdIJo0i3sLQTcwGpvk1ZG7SVJ+TqDXvoHkwlLlycgdIOC1sWYcG/g3gyJwQ8wdTCL68mI1LIUv2mEMAwHygk9pF2bfymgdk0SNBipe5S4jOqcd/YXuShksXg0qufU2Tun+1IsKLMX4S6QsOch/5CJXKAQ0dfPCWNmcNtzF/55ZXmTL1vbvGhD0iWIWvGCqy/hEGDRnHj6FTV7JGUGsOCIrWcnjBSQu9kIA00Tcwkuwq9eto56hmnU075hv4x2kiNA1FEjx6RSFLWgYloP9twFDiztBA15x3RMCDsqMC+u75oNVRWpbZwZIjhwN5rZaIuzAR3ALMXNExQIkuPOa4pyQ7ls8K6C5aEw28X7RMm8vb91NbsNahooVvkspxY3QefIUfBH83EfmwgpAJ2nNeBkAti7kx7jPdqBZzgDartESm4Z2D9HPuo0aR7XeVbe7kBb1NXuH+HmjOkxShSF3OlbOvQSKCRo/akTEu5Jj94JvkQgCprL6YqLzB11l039KJJnj+w76qK/bZXV7Mku+HAshnQwxiXkeUfiNpJbJAiO2+oU2YzlJy5eWEtYJUmYv2V+ypwV3XQOevR3SU+cIEU76meKg5mgEAB69KSpo3dst55LzU8G2aKK6uC0na7RFT21+NbG6OeaDglyW3DQPUcBKG2H1kTOR6qIY1G4XuTwPGd3gpRuTA51N5L9M5dvqDezDao7hNIxCR0oWeMSpm2OFKgnSwU4qTuowrfQsWo/RTPXXMXOodsYt0ID6nBoyr5LC/AOulPLmfOoHnP6ek+MEVuKsXqiFf1OxQTb86RSC7DFsmV59MxhuXmbR7mPG1KwZhYQoP+T8zPdnZFbHOfUjOWS9BZ2g9UIpYD8W23bbBwge+wecmqMcH6h0UWyZCoIYDX4wJwh/F3bVH+zSsmx8YNc4SE7hN4MJUm1GGRwaS8yA38WqAfnp7LdjJBI2kDtjkhVH6cbeNfLDofMdb8Uizum9xxSJKJN2b/CB02Zni+S3/+pE89dnluICt6+MMYTVCCiMOQ7tkXZ4/iRtNsBe2dpvWY6ZKBxznkign796lZ6Umii8uH3ibtxVXA88/YH1i2GfZI2VTqS88OqJqr2MLVeeqf2Mvuch5Frb0xZ7CUFviC53QP2RGTA7omyhDWB7PPKpVNwtsmUO+xKBiHiyN0gcp4jhFR1oYFUBkXyai6c2cyGFnhtYX7rBtSZcsL7Uh1YloM5yatEgGLtAw0wXSVw5Skw4YJhiPBlNGMBe4jy4FYa/bdWPMiwkEK6IfXS2xiZq5S3dsq/C3HMix0FLgfI8kPf2WI8AoC/ifW5psP28goFGMu6765ZLjZTZI++v6mrqen02S/h8xlzEqFDlImT0IxvMxNZHxwUVRwxtZU0Q/G06UvPTZnkcpvLFI/HeW5P3gDstIUuHcpU17nvMyY00fOo5Tjtz+7urEwarCkBR/SHuXQ3CT9i6llnSg3LRpL3o3dKutGhYBDmAmeDuc9NFOow47P/8ILr5lKHqMqVhNOLW8TFZ0WM4cfSqzFRT2ZORkm73sEj2yq10SXxBsrsofLoyfaoNsgWOfHQ2+w5/BJeW9f/IvvaXKSxq384wK+lvurjL6zKgZVHWjhqd+4sIrIL6EHSyKii+3yL86xKTdmjO6wXZcKMlOsfGvwkkfbB57Ckb/Qh57UJDEmQ4e76N/WjhwOJDoIMYU9o8+pqSaOxFHnAhVc3yIisFq1/ZMQLrkto0JLZ+RhmhQ0Ow4oj/S9hWYNTcd2rE+J8SvoUufd1ItW2lVMVTqRFms1PjmqXx/ev0NjwX6nOyeMjIzswNzSQ+iDjP69HacElLzLAs+F3dEpfOQMoeJog2rWhI83KhPVV2a/iQmY5Hd+tCPPAFanhA0S4/1o60UD8kwHS3uIV245fcWP74rwqhCsymRZVOLEFNRCd94HuiJnLkG3yOxwrQDnGu06D4MqgxbNO0oaTSEdWneNJR97OYgYK8qaYK+amXAQhqDOD5WLq0HzGXaX02J/RWGvSBlxCif5pSmYM7an4FiUWW+TJ5c3KhKzM/vQz1Gj7XwcqTFhHAPSUGwqi1vRZGdi9kh2uR5iYxFnZpgLcCLtedEcrdnhuVwqj4oPArnKPu+q7IW4o5K0MpoCVODu8/XUsOTBfvunYfCUel2OA3ZSw7ddcNGl6f83yFsB7reFlsJjfLFSIKRhA3FZv0z6QGnEJBBoUunsGOU+G2OqMcMv2AcTd2Vrqt6s8T4ehM8cPKr/HUfUo9p48dFDCBbmzoV5wbphlWMYHFKi/z2BCjvm6ZA7w781g7OlxNVHNKcjxOH6/idLnutSX1cOoRDXjaBwfFGqgHI3c5sbQUHpd3byj3yfWxKh+ssgPSBV7ZLxDnB1u8RzECeu3bsN2EdOaOQXj1i6SN4FkSHaLbRZWGCncDcx6vSqKcJ3lNx5dAPdQpJzywzpWKZ/ADp0RPL8v2ERG1JK2vIxXK4a6o6zlIy4bGFOaKHVO/K35XQ2+54fP8uDsYnNJANEbP9woe/af4xkEprMtQuf4OH11sEUL8sGJlfqMOuahrgKMDKMpQjT3IN4Lqc0ESL4MsMLPLRRFzufIp50Vly2mGhe+SHtuHZhOdP5Ee7FN63fkXCudjr1DwLqhgiodCoOy1rUZxqYa9qsfmMundE0gv/gadxjXNC+8yLy5qv0lkcnIW19sfyEhwzAQUc30+d6U/8RYT9suq+c30O04TzG9e5KmASOxPtSCIzQV8/mH6Dm3VgyJetp6tcuVIbxjqcQXDG9sCdiGGw4bWQuTagL7VEFQTNH2i/RDVOlUUwf6odzanSJ/69bVyuCWiHuRZDsPPlLfz2/EHHlFeUSNWZEjS3NpyaxL2AOe75GguBffjNZX/xpuNIeZJPl78DzsRuPmFVST/d5+picdV1lcmTWxtgUMMWtsJKXFdtjvll3XVOmlYhbqrgKqcBmedq5mWEhfImLBGL42rL5IoCKSx9m53+BBwEPA8+Wgx4eaEPDWDWvgMDGJ7d5wjHUqNDH/4ysb2opThNhjM8M1d9F5jQtw/ibD4Bc/+JUKIbemEtWXA2VZN9WK6mDZiIuqGi7u/yvLDvAWWitE346QJ/GTlmK9mn79ZwJzHIAzp+nuxq+DOQGcoDwK9WC51xh1ETsGiipKG3tD0cAUSOV6L5FLaQgbxwrmyWMByBT5PwFi1kaelRVG9syWl2Rydty+lKjUFAqQEOMpTY9G4cwrRVVwXFB5oxfhlN4x3lwA9MQQ+ct4bh7zAyKjEjTifTomiBi5VwXB/0hYPvSC6UPR3dxs09e7HPmd1bvmLq7aJWA3r2CteYRIp2ntykZpogXkA8DdTOcsWqaCRP1Gl+Yu8AZoPkx5MyQeOW0cSyD9IImiYrfp4uBahu4CFZAEziXUWepriNmOS+CkL1N1VrGbqHkV+Sssz84+8nQzeTTL7FywFILUdqTtE+CpHRSa6rEXkCeC6YKlyaicR3xpKjmcEVSfd0r/LBZHTdqyl4QkBlG4j1icNF6M2VTq3sFNJEdDlc8JO6Dzl4Obry71LJXzQ4P7lyuCJWM/YBbgs0Qi4kl5gHRzuxitynghN3KadRAjHxzHjP7h5MOSi7mNS8jAwDCM018Fpyo508ABP0ajl7rJsxXgwk9aHwaZFZmEKhrCeGOCXPEE9DSERuA83OwNOPQITEEnpjzTlm/L22ebPsm/egPSdmRqS9ay7jVWrz5zyydMqhEK02Y7ZyC0xT12KyGAjlgRlLygtPSXEaXJ22ws1MnULQ1Ey6u7XAwhds5oytVAkxhEDwaa70Sw22fs9l5HYKyaWnvgH0MyQP8reDuQVtajgB0UQwhDaMxIxpK8G6QRVGw+9tPGh1dYKy9YYNooCLRXws9WxrG34uOckPS1b8U3/ZTXQkVQ7i8Yy/6ryeNwSBEbevjrSA2VFwvvTGrW2JKwph5Mh5g0h58KEOgp3hj/+LXSBUQrD17G9U0CHAn/RK6tYuq8G0wfTjiikIKWX5TjoSnCpeQPocpLid0UNInzzNIsSmuBOuwVc/zmPrefk6GKpBRTzxznbWyiKXFT9/w21owssh2r8Gda7EDIR4hNrriwcXEUU+1Um8VzTHPJLS4kVZASaZStr+tVivU8YV8PUuJ2ZDMMCQ1xDiMKbCtG8XbhI0gMP8VSlZNAfOn52zblHY6aWaTB2MEHgBGNe24HUy8wwzchil80=</Data><Skey ci=\"20250923\">Uaqb8C5jDTw967CCDourHZ0abalsrKUxOiDogWV+z/oGcp386BJ+UZXOOgq2QGzzueAvJt7+RoX3xbI1YOkmvWNqVwlwzGxsGwISzcuMfeCSENtOj/D0iK2HhFtqZuX8CtfV40H8wKsMbIJUsP8TbkPCepkN2li9+z8AKLSL2ebZmU9R1GCVFmdAlsu1cDBKypVLGQpD/tsBHksIyTNNeWajfk+6reY5GE4MM8QB5GTRZHsqMXuHSKmR/1UWjBPUD5XDItsmvKeiMOcWC5CXxA2Cfhs6vgriR/CV2F3mGyKPb0t1pNlQ0v5yNsWEjmVERQsaPNqa8B151A/+a6ueeg==</Skey><Hmac>HlVZ3i+sOSzUxuLOhYxe3Q3FXKGJhyGKQN2q1Siro07Qkn4McJTKr14QPZ/WH1P6</Hmac><DeviceInfo dpId=\"STARTEK.ACPL\" rdsId=\"RENESAS.ACPL.001\" rdsVer=\"1.3.2\" dc=\"b07f9057 - 5981 - 4564 - a028 - 03a1f42e79ac\" mi=\"FM220U L1\" mc=\"MIIDfzCCAmegAwIBAgIEBK + LMDANBgkqhkiG9w0BAQsFADB0MR0wGwYDVQQDExRCSUpBWSBBTUFSTkFUSCBTSU5IQTEQMA4GA1UECBMHR1VKQVJBVDERMA8GA1UECxMIRElSRUNUT1IxITAfBgNVBAoTGEFDQ0VTUyBDT01QVVRFQ0ggUFZUIExURDELMAkGA1UEBhMCSU4wHhcNMjUwODI2MTIwODQ0WhcNMjUwOTI1MTIwODQ0WjBRMQswCQYDVQQGEwJJTjEQMA4GA1UECAwHR3VqYXJhdDENMAsGA1UECgwEVVNFUjENMAsGA1UECwwEVVNFUjESMBAGA1UEAwwJUFJPRF9VU0VSMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAqyrXh7yBs9l9jkK3TqMsYSA6HHW8h9m6kf / XhrBU9d20COnRkc7D7pmg / Zy339BOHY4FQMCD2pue5SbfLqKl3xlX0N1Hxz2gJ36yrtvElv + KxxIYm5Xpfg4NY5yaww82shB3GXrUGMKh6SmYMSnhQeZE5OOh8UDaBqJzmTq49wI4IGjGfHX2f61nPK2IwSNmpA02wo5RxfNFSv72gJn4nIoAUYavzg65tllWlNvEostGxuU4Ucd9LRA34l9sNt5y2LcOOAtKtxp3QG9uQLNdnWfCZm1PbmsJv62B34tACOD4pCKzUzKX92m9MZYVefpmcV6ZyUk6eBv4e8bSuyDbGQIDAQABozwwOjAMBgNVHRMEBTADAQH / MAsGA1UdDwQEAwIBhjAdBgNVHQ4EFgQU7yyaXA2gPDWT1SkCpqazyjE709gwDQYJKoZIhvcNAQELBQADggEBAG02oR + ORcR5NPmljssEZT9 + U6gzZX / uoswVpUYudQFcTtL0WnfOcNIhFx3NvBmkoBOP8yndRb7iQ / 0jXw + Iiu5YuR5GYNhESCQCPDsk / INPDukMl1Be3eeVqV1Qq7PqHCOykcKKQQTw3ZYZSregtrRXPFr2Tj71I424oMiGbj8WOpUpSR4ZHTN5VnxbvKM / zJ5oDrXbrVajKpsuzDQN2hgt2 / YdXJGGZ / +C3EI7ztSozKCbN0ukdn4gleLhFMKnDN9xoNdPqdt + i6dof1LHzxwAvNN3PAkor2Pw22jAgvGJw6niW1iJF7T5uVw + aMrjz7c6SzY + x5DHhhtKf3 + xSvk = \"><Additional_Info><Info name=\"modality_type\" value=\"Finger\"/><Info name=\"device_type\" value=\"L1\"/><Info name=\"srno\" value=\"M240361160\"/><Info name=\"Customer_Bound\" value=\"Open\"/><Info name=\"sysid\" value=\"b1f5502b7b09cbbfc7c820e86f829137\"/><Info name=\"SubscriptionUpto\" value=\"22 - Nov - 2025\"/><Info name=\"ts\" value=\"2025 - 08 - 27T12: 13:20 + 05:30\"/></Additional_Info><additional_info><Param name=\"modality_type\" value=\"Finger\"/><Param name=\"device_type\" value=\"L1\"/><Param name=\"srno\" value=\"M240361160\"/><Param name=\"DeviceType\" value=\"Open\"/><Param name=\"sysid\" value=\"b1f5502b7b09cbbfc7c820e86f829137\"/><Param name=\"SubscriptionUpto\" value=\"22 - Nov - 2025\"/><Param name=\"ts\" value=\"2025 - 08 - 27T12: 13:20 + 05:30\"/></additional_info></DeviceInfo></PidData>";
            string vReplaceString = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"yes\"?>";
            string vPidData = xml.Replace(vReplaceString, "");
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(vPidData);
            string vBase64PidData = Convert.ToBase64String(plainTextBytes);

            string sourceId = "FT";
            DateTime d = DateTime.Now;
            string dateString = d.ToString("yyyyMMddHHmmss");
            string vRndNo = GenerateRandomNo();
            string traceId = sourceId + dateString + vRndNo; // "MB202408251320160123"
            string txn = "AUANSDL001:" + dateString + vRndNo;
            string ts = d.ToString("yyyy-MM-dd") + "T" + d.ToString("HH:mm:ss"); //"2024-09-01T09:58:15" 

            auaKuaAuthReq pReq = new auaKuaAuthReq();
            pReq.sourceId = sourceId;
            pReq.traceId = traceId;
            pReq.LAT = "22.577152";
            pReq.LONG = "88.4310016";
            pReq.DEVMACID = "76:de:45:ed:4e:4e";
            pReq.DEVID = "public";
            pReq.CONSENT = "Y";
            pReq.SHRC = "Y";
            pReq.SERTYPE = "24";
            pReq.UDC = "1234567890";
            pReq.AADHAARNO = "736542221947";
            pReq.RRN = "1234567890";
            pReq.REF = "ABCDEFGHIJKL";
            pReq.PIDDATA = vBase64PidData;
            pReq.PFR = "N";
            pReq.LANG = "N";

            string requestBody = JsonConvert.SerializeObject(pReq);
            string postURL = "http://192.168.204.58:3010/IBMAadhaarService.svc" + "/auaKuaAuthWithBiometric";
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
                //string fullResponse = "\"{\\\"status\\\":\\\"success\\\",\\\"message\\\":\\\"OTP has been sent to mobile number *******7773\\\",\\\"code\\\":\\\"97ff0775f6e84344bbc7df4eab766d2e\\\",\\\"txn\\\":\\\"AUANSDL001:202506241337051658\\\",\\\"ts\\\":\\\"2025-06-24T13:38:11.139+05:30\\\"}\"";
                try
                {
                    fullResponse = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                    fullResponse = fullResponse.Trim('"');
                    dynamic res = JsonConvert.DeserializeObject(fullResponse);

                }
                finally { }
            }
            finally { }
            return "";
        }
        #endregion

    }
}

