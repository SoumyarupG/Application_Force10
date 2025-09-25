using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Xml;
using System.Web.Hosting;
using System.Security.Cryptography;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Web;

namespace UnityVriddhiMobService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UnityVriddhiService" in code, svc and config file together.
    public class UnityVriddhiService : IUnityVriddhiService
    {
        private static readonly string CHARSET = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        #region WebConfig Variables
        string vDBName = ConfigurationManager.AppSettings["DBName"];
        string vAccessTime = ConfigurationManager.AppSettings["AccessTime"];
        string vWebHookUrl = ConfigurationManager.AppSettings["WebHookUrl"];

        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        string InitialBucket = ConfigurationManager.AppSettings["InitialBucket"];
        string MemberBucket = ConfigurationManager.AppSettings["MemberBucket"];
        string PDBucket = ConfigurationManager.AppSettings["PDBucket"];

        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string PDURL = ConfigurationManager.AppSettings["PDURL"];
        string ServerIP = ConfigurationManager.AppSettings["ServerIP"];

        string vKeyId = ConfigurationManager.AppSettings["KeyId"];
        string vOuId = ConfigurationManager.AppSettings["OuId"];
        string vSecret = ConfigurationManager.AppSettings["Secret"];
        string vAccountId = ConfigurationManager.AppSettings["AccountId"];
        string vApiKey = ConfigurationManager.AppSettings["ApiKey"];
        #endregion

        #region SendOTP
        public string SendOTP(string EncryptedRequest)
        {
            string result = "";
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                return "Unauthorized access.";
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                OTPData objOTPData = JsonConvert.DeserializeObject<OTPData>(vRequestData);
                //-----------------------------------                

                WebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    string vOTP = objOTPData.pOTP;
                    string vMsgBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vOTP + ".";
                    //********************************************************************
                    String sendToPhoneNumber = objOTPData.pMobileNo;
                    String userid = "2000204129";
                    String passwd = "Unity@1122";
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472061562826&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
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
                return Encrypt(result, xKey);
            }
        }
        #endregion

        #region GetAppVersion
        public string GetAppVersion(string EncryptedRequest)
        {
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                return "Unauthorized access.";
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic vObj = JsonConvert.DeserializeObject(vRequestData);
                string pVersion = vObj.pVersion;
                try
                {
                    string VersionCode = ConfigurationManager.AppSettings["MobAppVersionCode"];
                    if (Convert.ToInt32(VersionCode) > Convert.ToInt32(pVersion))
                    {
                        return Encrypt("https://centrumsaralmob.bijliftt.com/SaralVyapar.apk", xKey);
                    }
                    else
                    {
                        return Encrypt("No updates available", xKey);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
        public string GetMobUser(string EncryptedRequest)
        {
            string vAttFlag = ConfigurationManager.AppSettings["AttFlag"];
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            byte[] xKey = null;

            List<EmpData> row1 = new List<EmpData>();
            List<PermissionData> row2 = new List<PermissionData>();
            List<LoginData> rowFinal = new List<LoginData>();
            //------------------------------------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            //-------------------------------------------------------------
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                row1.Add(new EmpData("", "", "", "", "Login Failed", "", "", "N", "", "", "", "", "", "", ""));
                row2.Add(new PermissionData("Unauthorized access", "", "", "", "", "", "", ""));
                rowFinal.Add(new LoginData(row1, row2));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                UserData userData = JsonConvert.DeserializeObject<UserData>(vRequestData);
                //-----------------------------------           
                CRepository oRepo = null;
                try
                {
                    //-----------08.07.2025---------------  
                    string vX_Key_Pwd = RsaDecrypt(userData.pKey);
                    byte[] xKey_Pwd = Convert.FromBase64String(vX_Key_Pwd);
                    userData.pPassword = Decrypt(userData.pPassword, xKey_Pwd);
                    //-----------------------------------
                    oRepo = new CRepository();
                    ds = oRepo.GetMobUser(userData);
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];

                    if (dt1.Rows.Count == 1)
                    {
                        foreach (DataRow rs in dt1.Rows)
                        {
                            row1.Add(new EmpData(rs["EoName"].ToString(), rs["BranchCode"].ToString(), rs["UserID"].ToString(), rs["Eoid"].ToString(), rs["LogStatus"].ToString(),
                                rs["DayEndDate"].ToString(), rs["AttStatus"].ToString(), vAttFlag, rs["AreaCat"].ToString(), rs["Designation"].ToString(),
                                rs["AllowManualEntry"].ToString(), rs["AllowQRScan"].ToString(), rs["LoginId"].ToString(), rs["MFAYN"].ToString(), rs["MFAOTP"].ToString()));
                            if (rs["MFAYN"].ToString() == "Y")
                            {
                                SendMFAOTP(rs["MFAOTP"].ToString(), rs["MobileNo"].ToString());
                            }
                        }
                    }
                    else
                    {
                        row1.Add(new EmpData("", "", "", "", "Login Failed", "", "", "N", "", "", "", "", "", "", ""));
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
            }
            return Encrypt(JsonConvert.SerializeObject(rowFinal), xKey);
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

        public string KarzaVoterIDKYCValidation(string EncryptedRequest)
        {
            KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
            byte[] xKey = null;
            //------------------------------------------------------------
            IncomingWebRequestContext req1 = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req1.Headers;
            string pKey = headers["X-Session-Key"];
            //-------------------------------------------------------------
            if (pKey == null || pKey == "")
            {
                vResponseObj = new KYCVoterIDResponse();
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                vResponseObj.request_id = "60ffe33e-b908-4e37-bb9c-680f132cefc5";
                vResponseObj.status_code = "401:Unauthorized access.";
                return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                KYCVoterIDRequest vPostVoterData = JsonConvert.DeserializeObject<KYCVoterIDRequest>(vRequestData);
                //----------------------------------- 
                string vVoterID = vPostVoterData.epic_no;
                string vEoId = vPostVoterData.pEoID == null ? "" : vPostVoterData.pEoID;
                string vBranch = vPostVoterData.pBranch == null ? "" : vPostVoterData.pBranch;
                CRepository oCR = null;
                var req = new KYCVoterRequest()
                {
                    consent = "Y",
                    epic_no = vVoterID
                };
                string Requestdata = JsonConvert.SerializeObject(req);
                string postURL = "https://testapi.karza.in/v2/voter";
                //string postURL = "https://api.karza.in/v2/voter";
                try
                {
                    oCR = new CRepository();
                    int vErrCount = oCR.GetKarzaVoterErrCount(vVoterID);
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
                        request.Headers.Add("x-karza-key", "wdycvLFD27R0RuAn2guz");
                        //request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
                        //request.Host = "api.karza.in";
                        request.Host = "testapi.karza.in";

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
                        vResponseObj = new KYCVoterIDResponse();
                        vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(responsedata.Replace("status-code", "status_code"));
                        try
                        {
                            responsedata = responsedata.Replace("\u0000", "");
                            responsedata = responsedata.Replace("\\u0000", "");
                            string vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                            oCR = new CRepository();
                            oCR.SaveKarzaVoterVerifyData(vVoterID, vXml, vBranch, vEoId);//Save Response
                        }
                        finally
                        {
                            //---
                        }
                        string vErrMsg = string.Empty;
                        oCR = new CRepository();
                        if (vResponseObj.status_code == "101")
                        {
                            vErrMsg = "101:Valid Authentication";
                        }
                        else if (vResponseObj.status_code == "102")
                        {

                            oCR.SaveKarzaVoterVerifyErrLog(vVoterID, vResponseObj.status_code);
                            vErrMsg = "102:Invalid ID number or combination of inputs";
                        }
                        else if (vResponseObj.status_code == "103")
                        {
                            oCR.SaveKarzaVoterVerifyErrLog(vVoterID, vResponseObj.status_code);
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
                        return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
                        //return vResponseObj;
                    }
                    else
                    {
                        vResponseObj = new KYCVoterIDResponse();
                        vResponseObj.request_id = "60ffe33e-b908-4e37-bb9c-680f132cefc5";
                        vResponseObj.status_code = "901:Maximum retry Exceeded";
                        //return vResponseObj1;
                        return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaVoterVerifyData(vVoterID, vXml, vBranch, vEoId);

                    Response.Replace("requestId", "request_id");
                    vResponseObj = new KYCVoterIDResponse();
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
                    //return vResponseObj;
                    return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
                }
                finally
                {
                    // streamWriter = null;
                }
            }
        }

        public List<OCRParameterResponse> OCRPhototoData(Stream DocFile)
        {
            CRepository oCR = null;
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
                        }
                    }
                    string vResponseData = vStringResponse;
                    try
                    {
                        string folderPath = HostingEnvironment.MapPath(string.Format("~/Files/{0}/{1}", "OCR", vMobileNo));
                        System.IO.Directory.CreateDirectory(folderPath);
                        Guid guid = Guid.NewGuid();
                        File.WriteAllText(folderPath + "/" + Convert.ToString(guid) + ".text", vResponseData);
                    }
                    finally { }
                    vResponseData = vResponseData.Replace("\u0000", "");
                    vResponseData = vResponseData.Replace("\\u0000", "");
                    vXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));//Convert Json to Xml  
                    oCR = new CRepository();
                    vStatusCode = oCR.SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
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
                    oCR = new CRepository();
                    oCR.SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
                }
            }
            catch (Exception ex)
            {
                row.Add(new OCRParameterResponse("Failure", "{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":504}"));
                vXml = AsString(JsonConvert.DeserializeXmlNode("{\"requestId\":\"2a62d0a5-be3a-4cdc-9a1c-990f74d01297\",\"result\":[],\"statusCode\":701}", "root"));//Convert Json to Xml  
                oCR = new CRepository();
                oCR.SaveOCRLog(vMobileNo, vEoid, vBranchCode, vXml);
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

            //string postURL = "https://testapi.karza.in/v3/ocr/kyc";
            string postURL = "https://api.karza.in/v3/ocr/kyc";
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
                request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");//Live
                //request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe"); //test
                request.Host = "api.karza.in";
                //request.Host = "testapi.karza.in";

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

        public string GetKarzaToken(string vAadharNo)
        {
            string requestBody = @"{""productId"": [""aadhaar_xml""]}";
            //string postURL = "https://testapi.karza.in/v2/get-jwt";
            string postURL = "https://api.karza.in/v2/get-jwt";
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
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
                string vKarzaToken = res.result.karzaToken;
                try
                {
                    oCR = new CRepository();
                    oCR.SaveKarzaAadharToken(vAadharNo, vKarzaToken);
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

        public class NameMatchRequest
        {
            public string name1 { get; set; }
            public string name2 { get; set; }
            public string type { get; set; }
            public string preset { get; set; }
        }

        public class AddressMatchRequest
        {
            public string address1 { get; set; }
            public string address2 { get; set; }
        }

        public string SaveKarzaAadharVerifyData(string pAadharNo, string pResponseData)
        {
            string pResponseXml = "";
            pResponseData = pResponseData.Replace("\u0000", "");
            pResponseData = pResponseData.Replace("\\u0000", "");
            pResponseXml = AsString(JsonConvert.DeserializeXmlNode(pResponseData, "root"));
            CRepository oCR = new CRepository();
            return oCR.SaveKarzaAadharVerifyData(pAadharNo, pResponseXml);
        }

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
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
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
                    oCR = new CRepository();
                    oCR.SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);
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
                oCR = new CRepository();
                oCR.SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);
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
            CRepository oCR = null;
            string requestBody = JsonConvert.SerializeObject(req);
            //string postURL = "https://testapi.karza.in/v2/address";
            string postURL = "https://api.karza.in/v2/address";

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
                request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");//Test KEY-- QqNBCOWSaESvPH3z
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
                dynamic res = JsonConvert.DeserializeObject(fullResponse.Replace("status-code", "statusCode").Replace("match", "result"));
                try
                {
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse.Replace("status-code", "statusCode"), "root"));
                    oCR = new CRepository();
                    oCR.SaveKarzaMatchingDtl("AddresssMatching", vXml, pBranch, pEoID, pIdNo);
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
                oCR = new CRepository();
                oCR.SaveKarzaMatchingDtl("AddresssMatching", vXml, pBranch, pEoID, pIdNo);
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
            CRepository oCR = null;
            string requestBody = JsonConvert.SerializeObject(req);
            //string postURL = "https://testapi.karza.in/v3/facesimilarity";
            string postURL = "https://api.karza.in/v3/facesimilarity";

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
                request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
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
                    oCR = new CRepository();
                    oCR.SaveKarzaMatchingDtl("FaceMatching", vXml, pBranch, pEoID, pIdNo);
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
                oCR = new CRepository();
                oCR.SaveKarzaMatchingDtl("FaceMatching", vXml, pBranch, pEoID, pIdNo);
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
            string postURL = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "https://testapi.karza.in/v3/aadhaar-xml/otp" : "https://api.karza.in/v3/aadhaar-xml/otp";
            string xKarzaKey = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", xKarzaKey);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("AadhaarXmlOtp", vXml, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("AadhaarXmlOtp", vXml, aadhaarXmlOtp.pBranch, aadhaarXmlOtp.pEoID, aadhaarXmlOtp.aadhaarNo);
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
            string postURL = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "https://testapi.karza.in/v3/aadhaar-xml/file" : "https://api.karza.in/v3/aadhaar-xml/file";
            string xKarzaKey = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", xKarzaKey);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("AadhaarXml", vXml, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("AadhaarXml", vXml, aadhaarXmlDownload.pBranch, aadhaarXmlDownload.pEoID, aadhaarXmlDownload.aadhaarNo);
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
            string postURL = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "https://testapi.karza.in/v3/eaadhaar/otp" : "https://api.karza.in/v3/eaadhaar/otp";
            string xKarzaKey = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", xKarzaKey);
                // request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(fullResponse);
                try
                {
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("eAadhaarOtp", vXml, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
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
                AadhaarOTPResponse objResponse = JsonConvert.DeserializeObject<AadhaarOTPResponse>(Response);
                try
                {
                    Response = Response.Replace("\u0000", "");
                    Response = Response.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("eAadhaarOtp", vXml, eAadhaarOTP.pBranch, eAadhaarOTP.pEoID, eAadhaarOTP.aadhaarNo);
                }
                finally
                {
                }
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
            string postURL = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "https://testapi.karza.in/v3/eaadhaar/file" : "https://api.karza.in/v3/eaadhaar/file";
            string xKarzaKey = vDBName.ToUpper() == "CENTRUM_SARAL_UAT" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
            CRepository oCR = null;
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
                request.Headers.Add("x-karza-key", xKarzaKey);
                //request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("eAadhaarXml", vXml, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaAadhaarOtp("eAadhaarXml", vXml, eAadhaarDownload.pBranch, eAadhaarDownload.pEoID, eAadhaarDownload.aadhaarNo);
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

        #region PAN Authentication
        public string KarzaPANValidation(string EncryptedRequest)
        {
            KarzaPANResponse vResponseObj = new KarzaPANResponse();
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                vResponseObj = new KarzaPANResponse();
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                vResponseObj.status_code = "401:Unauthorized access.";
                return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                KYCPANRequest vPostPanData = JsonConvert.DeserializeObject<KYCPANRequest>(vRequestData);
                //-----------------------------------
                string vPAN = vPostPanData.pan;
                string vEoId = vPostPanData.pEoID == null ? "" : vPostPanData.pEoID;
                string vBranch = vPostPanData.pBranch == null ? "" : vPostPanData.pBranch;
                CRepository oCR = null;
                string Requestdata = JsonConvert.SerializeObject(vPostPanData);
                string postURL = vDBName.ToUpper() == "CENTRUM_Vriddhi" ? "https://api.karza.in/v2/pan" : "https://testapi.karza.in/v2/pan";
                try
                {
                    HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                    if (request == null)
                    {
                        throw new NullReferenceException("request is not a http request");
                    }
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Headers.Add("x-karza-key", vDBName.ToUpper() == "CENTRUM_Vriddhi" ? "VOht331uCcRtBgI" : "wdycvLFD27R0RuAn2guz");
                    request.Host = vDBName.ToUpper() == "CENTRUM_Vriddhi" ? "api.karza.in" : "testapi.karza.in";
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(Requestdata);
                        streamWriter.Close();
                    }
                    StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                    string responsedata = responseReader.ReadToEnd();
                    request.GetResponse().Close();
                    vResponseObj = new KarzaPANResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KarzaPANResponse>(responsedata.Replace("status-code", "status_code"));
                    try
                    {
                        responsedata = responsedata.Replace("\u0000", "");
                        responsedata = responsedata.Replace("\\u0000", "");
                        string vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                        oCR = new CRepository();
                        oCR.SaveKarzaPanVerifyData(vPAN, vXml, vBranch, vEoId);//Save Response
                    }
                    finally
                    {
                    }
                    string vErrMsg = string.Empty;
                    int vStatusCode = Convert.ToInt32(vResponseObj.status_code);
                    if (vStatusCode == 101)
                    {
                        vErrMsg = "101:Valid Authentication";
                    }
                    else
                    {
                        vErrMsg = vStatusCode.ToString();
                    }
                    vResponseObj.status_code = vErrMsg;
                    return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
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
                    oCR = new CRepository();
                    oCR.SaveKarzaPanVerifyData(vPAN, vXml, vBranch, vEoId);

                    Response.Replace("requestId", "request_id");
                    vResponseObj = new KarzaPANResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KarzaPANResponse>(Response);

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
                    return Encrypt(JsonConvert.SerializeObject(vResponseObj), xKey);
                }
                finally
                {
                    // streamWriter = null;
                }
            }
        }

        #endregion

        #endregion

        #region CommonString
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
        #endregion

        #region SerializeJson
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

        #region GetKYCInfo
        public string GetKYCInfo(string EncryptedRequest)
        {
            PostKYCData postKYCData = new PostKYCData();
            DataTable dt = new DataTable();
            List<KYCData> row = new List<KYCData>();
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                row.Add(new KYCData("Unauthirized Access.", "", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postKYCData = JsonConvert.DeserializeObject<PostKYCData>(vRequestData);
                //-----------------------------------
                try
                {
                    CRepository oRs = new CRepository();
                    dt = oRs.GetKYCInfo(postKYCData);
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

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region SaveKYC
        public string SaveKYC(string EncryptedRequest)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                return Encrypt("FAIL:Unauthorized access." + ":XX:0:0", xKey);
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                PostIAData postKYCData = JsonConvert.DeserializeObject<PostIAData>(vRequestData);
                PostKYCSaveData postKYCSaveData = new PostKYCSaveData();
                PostVerifyData postVerifyData = new PostVerifyData();
                PostEMOCRData postEMOCRData = new PostEMOCRData();
                postKYCSaveData = postKYCData.postKYCSaveData;
                postVerifyData = postKYCData.postVerifyData;
                postEMOCRData = postKYCData.postEMOCRData;
                //-------------------------------------
                vErrDesc = oCR.SaveKYC(postKYCSaveData, postVerifyData, postEMOCRData);
            }
            return Encrypt(vErrDesc, xKey);
        }

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
                        string vFileTag = "", vFileExt = ".png";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        if (fileName.Contains(".pdf"))
                        {
                            vFileExt = ".pdf";
                        }
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            if (vFileTag.Equals("GroupPhoto"))
                            {
                                vErr = SaveMemberImages(binaryWriteArray, "Group", kycNo, vFileTag, vFileExt);
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
                                vErr = SaveMemberImages(binaryWriteArray, "Member", kycNo, vFileTag, vFileExt);
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
                                vErr = SaveMemberImages(binaryWriteArray, "InitialApproach", kycNo, vFileTag, vFileExt);
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

        private System.Drawing.Image LoadImage(byte[] bytes)
        {
            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms);
            }
            return image;
        }

        private string SaveMemberImages(byte[] imageBinary, string imageGroup, string folderName, string imageName, string fileExt)
        {
            string isImageSaved = "N";
            if (MinioYN == "N")
            {
                string folderPath = HostingEnvironment.MapPath("~/Files/" + imageGroup);
                System.IO.Directory.CreateDirectory(folderPath);
                string filePath = string.Format("{0}/{1}", folderPath, folderName + "_" + imageName + fileExt);
                if (imageBinary != null)
                {
                    File.WriteAllBytes(filePath, imageBinary);
                    isImageSaved = "Y";
                }
            }
            else
            {
                string BucketName = imageGroup == "Member" ? MemberBucket : imageGroup == "PD" ? PDBucket : InitialBucket;
                isImageSaved = UploadFileMinio(imageBinary, imageName + fileExt, folderName, BucketName, MinioUrl);
            }
            return isImageSaved;
        }

        public List<KYCImageSave> PdImageUpload(Stream image)
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
                        string vFileTag = "", vFileExt = ".png";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        if (fileName.Contains(".pdf"))
                        {
                            vFileExt = ".pdf";
                            vErr = SaveMemberImages(binaryWriteArray, "PD", kycNo, vFileTag, vFileExt);
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
                            System.Drawing.Image img = LoadImage(binaryWriteArray);
                            if (img != null)
                            {
                                vErr = SaveMemberImages(binaryWriteArray, "PD", kycNo, vFileTag, vFileExt);
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
                }
                return row;
            }
            else
            {
                row.Add(new KYCImageSave("Failed", "No Data Found"));
            }
            return row;
        }

        #endregion

        #region SaveOCRData
        public string SaveOCRData(PostOCRData postOCRData)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SaveOCRData(postOCRData);
            return vErrDesc;
        }
        #endregion

        #region SaveVerifyData
        public string SaveVerifyData(PostVerifyData postVerifyData)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SaveVerifyData(postVerifyData);
            return vErrDesc;
        }
        #endregion

        #region GetAddressDtl
        public string GetAddressDtl(string EncryptedRequest)
        {
            DataTable dt = new DataTable();
            List<AddressData> row = new List<AddressData>();
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row = new List<AddressData>();
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                row.Add(new AddressData("Unauthorized access.", "", "", "", "", "", "", "", "", "", ""));
                return Encrypt(JsonConvert.SerializeObject(row), xKey);
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                PostAddressData postAddressData = JsonConvert.DeserializeObject<PostAddressData>(vRequestData);
                //-----------------------------------
                try
                {
                    CRepository oCR = new CRepository();
                    dt = oCR.GetAddressDtl(postAddressData);
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
                }
                return Encrypt(JsonConvert.SerializeObject(row), xKey);
            }
        }
        #endregion

        #region GetStateList
        public string GetStateList()
        {
            List<StateData> row = new List<StateData>();
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row = row = new List<StateData>(); ;
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                row.Add(new StateData("Unauthorized access", "", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                //-----------------------------------
                DataTable dt = new DataTable();
                row = new List<StateData>();
                CRepository oCR = new CRepository();
                try
                {
                    dt = oCR.GetStateList();
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

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetMemberDtl
        public string GetMemberDtl(string EncryptedRequest)
        {
            string pMemberId = "";
            byte[] xKey = null;
            DataTable dt, dt1 = new DataTable();
            DataSet ds = new DataSet();
            List<MemberData> row = new List<MemberData>();
            List<EarningMemberKYCData> row1 = new List<EarningMemberKYCData>();
            CRepository oCR = new CRepository();
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new MemberData("Unauthorized access", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                          "", "", "", "", "", "", "", "", "", "", "", row1));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic vObj = JsonConvert.DeserializeObject(vRequestData);
                pMemberId = vObj.pMemberId;
                try
                {
                    ds = oCR.GetMemberDtlById(pMemberId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];

                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow rs1 in dt1.Rows)
                        {
                            row1.Add(new EarningMemberKYCData(rs1["Name"].ToString(), rs1["DOB"].ToString(), rs1["Relation"].ToString(), rs1["Address1"].ToString(),
                               rs1["StateId"].ToString(), rs1["PinCode"].ToString(), rs1["MobileNo"].ToString(), rs1["KYCType"].ToString(), rs1["KYCNo"].ToString()));
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new MemberData(rs["MF_Name"].ToString(), rs["MM_Name"].ToString(), rs["ML_Name"].ToString(), rs["FullName"].ToString(),
                               rs["M_DOB"].ToString(), rs["M_Age"].ToString(), rs["GuarName"].ToString(), rs["GuarRel"].ToString(), rs["M_IdentyPRofId"].ToString(),
                               rs["M_IdentyProfNo"].ToString(), rs["M_AddProfId"].ToString(), rs["M_AddProfNo"].ToString(), rs["AddrType"].ToString(),
                               rs["M_HouseNo"].ToString(), rs["M_Street"].ToString(), rs["Area"].ToString(), rs["Village"].ToString(), rs["M_WardNo"].ToString(),
                               "", rs["State"].ToString(), "", rs["MemAddr"].ToString(), rs["Landmark"].ToString(), rs["M_PostOff"].ToString(), rs["M_PIN"].ToString(),
                               rs["M_Mobile"].ToString(), rs["AddrType_p"].ToString(), rs["HouseNo_p"].ToString(), rs["Street_p"].ToString(), rs["Area_p"].ToString(),
                               rs["VillageId_p"].ToString(), rs["WardNo_p"].ToString(), rs["Landmark_p"].ToString(), rs["PostOff_p"].ToString(), rs["PIN_p"].ToString(),
                               rs["MobileNo_p"].ToString(), rs["Branchcode"].ToString(), rs["CoAppName"].ToString(), rs["CoAppDOB"].ToString(),
                                rs["B_HumanRelationId"].ToString(), rs["B_Address"].ToString(), "", rs["B_PIN"].ToString(), rs["B_Mobile"].ToString(), "", "", row1
                               ));
                        }
                    }
                    else
                    {
                        row.Add(new MemberData("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", row1));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new MemberData("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", row1));
                }
                finally
                {

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetLoanScheme
        public string GetLoanScheme(string EncryptedRequest)
        {
            DataTable dt = new DataTable();
            List<SchemeData> row = new List<SchemeData>();
            PostSchemeData postSchemeData = null;
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new SchemeData("Unauthorized access", "", ""));
            }
            else
            {
                postSchemeData = new PostSchemeData();
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postSchemeData = JsonConvert.DeserializeObject<PostSchemeData>(vRequestData);
                //-----------------------------------
                try
                {
                    CRepository oCR = new CRepository();
                    dt = oCR.GetLoanType(postSchemeData);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new SchemeData(rs["LoanTypeId"].ToString(), rs["LoanType"].ToString(), rs["IsTopup"].ToString()));
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
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region SaveEMOCRData
        public string SaveEMOCRData(PostEMOCRData postEMOCRData)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SaveEMOCRData(postEMOCRData);
            return vErrDesc;
        }
        #endregion

        #region MemberCreationList
        public string MemberCreationList(string EncryptedRequest)
        {
            PostMemberData postMemberData = new PostMemberData();
            byte[] xKey = null;
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<MemberCreationSubData> row = new List<MemberCreationSubData>();
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new MemberCreationSubData("Unauthorized access", "", "", "", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postMemberData = JsonConvert.DeserializeObject<PostMemberData>(vRequestData);
                try
                {
                    dt = oCR.GetMemberCreationData(postMemberData);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new MemberCreationSubData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["EnquiryId"].ToString(), rs["Name"].ToString(), rs["MemType"].ToString()));
                        }
                    }
                    else
                    {
                        row.Add(new MemberCreationSubData("No data available", "", "", "", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new MemberCreationSubData("No data available", ex.ToString(), "", "", ""));
                }
                finally
                {
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region NewMemberCreationData
        public string NewMemberCreationData(string EncryptedRequest)
        {
            PostMemberFormData postMemberFormData = new PostMemberFormData();
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<GetNewMemberInfo> row = new List<GetNewMemberInfo>();
            byte[] xKey = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new GetNewMemberInfo("Unauthorized access", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                       "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postMemberFormData = JsonConvert.DeserializeObject<PostMemberFormData>(vRequestData);
                try
                {
                    dt = oCR.GetIniApprMemDtlByEnqId(postMemberFormData);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            //string pRequestdata = "{\"pEnquiryId\":\"" + postMemberFormData.pEnquiryId + "\"}";
                            // HttpWebRequest("https://unityimage.bijliftt.com/ImageDownloadService.svc/GetImage", pRequestdata);
                            // vImgPath = "https://unityimage.bijliftt.com/DownloadImage/";
                            row.Add(new GetNewMemberInfo(rs["MF_Name"].ToString(), rs["MM_Name"].ToString(), rs["ML_Name"].ToString(), rs["MDOB"].ToString(), rs["Age"].ToString(),
                                rs["FamilyPersonName"].ToString(), rs["HumanRelationId"].ToString(), rs["IdentyPRofId"].ToString(), rs["IdentyProfNo"].ToString(), rs["AddProfId"].ToString(),
                                rs["AddProfNo"].ToString(), rs["CoAppIdentyPRofId"].ToString(), rs["CoAppIdentyProfNo"].ToString(), rs["AddrType"].ToString(),
                                rs["HouseNo"].ToString(), rs["Street"].ToString(),
                                rs["Area"].ToString(), rs["Village"].ToString(), rs["WardNo"].ToString(), rs["District"].ToString(), rs["State"].ToString(), rs["Landmark"].ToString(),
                                rs["PostOff"].ToString(), rs["PIN"].ToString(), rs["MobileNo"].ToString(), rs["AddrType_p"].ToString(), rs["HouseNo_p"].ToString(),
                                rs["Street_p"].ToString(), rs["Area_p"].ToString(), rs["VillageId_p"].ToString(), rs["WardNo_p"].ToString(), rs["Landmark_p"].ToString(), rs["PostOff_p"].ToString(),
                                rs["PIN_p"].ToString(), rs["ContactNo"].ToString(), rs["MemStatus"].ToString(),
                                rs["CoAppName"].ToString(), rs["CoApplDOB"].ToString(), rs["CoAppMobileNo"].ToString(), rs["CoAppRelationId"].ToString(),
                                rs["CoAppAddress"].ToString(), rs["CoAppState"].ToString(), rs["CoAppPinCode"].ToString(), rs["LoanTypeId"].ToString(),
                                rs["TotalOS"].ToString(), rs["NoOfOpenLoan"].ToString(), rs["AmountEligible"].ToString(),
                                rs["CoAppAddressPRofId"].ToString(), rs["CoAppAddressProfNo"].ToString(), rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString()
                                ));
                        }
                    }
                    else
                    {
                        row.Add(new GetNewMemberInfo("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new GetNewMemberInfo("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
                finally
                {
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region SavePdBySO
        public string SavePdBySO(string EncryptedRequest)
        {
            PostPdBySo postPdBySo = null;
            string vErrDesc = "";
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                vErrDesc = "Failed:Unauthorized access.:0";
            }
            else
            {
                postPdBySo = new PostPdBySo();
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postPdBySo = JsonConvert.DeserializeObject<PostPdBySo>(vRequestData);
                CRepository oCR = null;
                oCR = new CRepository();
                vErrDesc = oCR.SavePdBySO(postPdBySo);
            }
            return Encrypt(vErrDesc, xKey);
        }
        #endregion

        #region SavePdByBM
        public string SavePdByBM(string EncryptedRequest)
        {
            PostPdByBM postPdByBM = null;
            CRepository oCR = null;
            string vErrDesc = "";
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                vErrDesc = "Failed:Unauthorized access.:0";
            }
            else
            {
                postPdByBM = new PostPdByBM();
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postPdByBM = JsonConvert.DeserializeObject<PostPdByBM>(vRequestData);
                oCR = new CRepository();
                vErrDesc = oCR.SavePdByBM(postPdByBM);
            }
            return Encrypt(vErrDesc, xKey);
        }
        #endregion

        #region GetBusinessTypeList
        public string GetBusinessTypeList()
        {
            DataTable dt = new DataTable();
            List<BusinessTypeData> row = new List<BusinessTypeData>();
            CRepository oCR = new CRepository();
            byte[] xKey = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new BusinessTypeData("Unauthorized access", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                //------------------------------------
                try
                {
                    dt = oCR.PopBusinessType();
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new BusinessTypeData(rs["BusinessTypeId"].ToString(), rs["BusinessTypeName"].ToString()));
                        }
                    }
                    else
                    {
                        row.Add(new BusinessTypeData("No data available", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new BusinessTypeData("No data available", ex.ToString()));
                }
                finally
                {

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetBusiSubTypeByTypeId
        public string GetBusinessSubType(string EncryptedRequest)
        {
            DataTable dt = new DataTable();
            List<BusinessSubTypeData> row = new List<BusinessSubTypeData>();
            CRepository oCR = new CRepository();
            byte[] xKey = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new BusinessSubTypeData("Unauthorized access", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic vObj = JsonConvert.DeserializeObject(vRequestData);
                string pBusiTypeId = vObj.pBusiTypeId;
                //-----------------------------------
                try
                {
                    dt = oCR.PopBusiSubTypeByTypeId(Convert.ToInt32(pBusiTypeId));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new BusinessSubTypeData(rs["BusinessSubTypeID"].ToString(), rs["BusinessSubType"].ToString()));
                        }
                    }
                    else
                    {
                        row.Add(new BusinessSubTypeData("No data available", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new BusinessSubTypeData("No data available", ex.ToString()));
                }
                finally
                {

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetBusinessActivity
        public string GetBusinessActivity(string EncryptedRequest)
        {
            DataTable dt = new DataTable();
            List<BusinessActivityData> row = new List<BusinessActivityData>();
            CRepository oCR = new CRepository();
            string pBusiSubTypeId = "";
            byte[] xKey = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new BusinessActivityData("Unauthorized access", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic vObj = JsonConvert.DeserializeObject(vRequestData);
                pBusiSubTypeId = vObj.pBusiSubTypeId;
                //-----------------------------------
                try
                {
                    dt = oCR.PopBusiActivityBySubTypeId(Convert.ToInt32(pBusiSubTypeId));
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new BusinessActivityData(rs["BusinessActivityID"].ToString(), rs["BusinessActivity"].ToString()));
                        }
                    }
                    else
                    {
                        row.Add(new BusinessActivityData("No data available", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new BusinessActivityData("No data available", ex.ToString()));
                }
                finally
                {

                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetIFSCDtl
        public string GetIFSCDtl(string EncryptedRequest)
        {
            PostIFSCData postIFSCData = new PostIFSCData();
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<IFSCData> row = new List<IFSCData>();
            byte[] xKey = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                row.Add(new IFSCData("Unauthorized access", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postIFSCData = JsonConvert.DeserializeObject<PostIFSCData>(vRequestData);
                try
                {
                    dt = oCR.GetIFSCDtl(postIFSCData);
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
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetPdByBMData
        public string GetPdByBMData(string EncryptedRequest)
        {
            byte[] xKey = null;
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<PDByBMData> row = new List<PDByBMData>();
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                row.Add(new PDByBMData("Unauthorized access", "", "", "", ""));
            }
            else
            {
                PostPdByBMData postPdByBMData = null;
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postPdByBMData = JsonConvert.DeserializeObject<PostPdByBMData>(vRequestData);
                //------------------------------------           
                try
                {
                    dt = oCR.GetPdByBMData(postPdByBMData);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow rs in dt.Rows)
                        {
                            row.Add(new PDByBMData(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["EnquiryId"].ToString(), rs["Name"].ToString(), rs["PDId"].ToString()));
                        }
                    }
                    else
                    {
                        row.Add(new PDByBMData("No data available", "", "", "", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new PDByBMData("No data available", ex.ToString(), "", "", ""));
                }
                finally
                {
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region GetPdDtlByPdId
        public string GetPdDtlByPdId(string EncryptedRequest)
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2, dt3, dt4 = null;
            CRepository oCR = new CRepository();
            List<PDByBMDtl> row = new List<PDByBMDtl>();
            byte[] xKey = null;
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                row.Add(new PDByBMDtl("Unauthorized access", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                        , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", ""));
            }
            else
            {
                PostPdDtl postPdDtl = new PostPdDtl();
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postPdDtl = JsonConvert.DeserializeObject<PostPdDtl>(vRequestData);
                //------------------------------------
                try
                {
                    ds = new DataSet();
                    dt = new DataTable();
                    dt1 = new DataTable();
                    dt2 = new DataTable();
                    dt3 = new DataTable();
                    dt4 = new DataTable();

                    ds = oCR.GetPdDtlByPdId(Convert.ToInt32(postPdDtl.pPdId));
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];
                    dt4 = ds.Tables[4];

                    if (dt.Rows.Count > 0)
                    {
                        row.Add(new PDByBMDtl(
                           dt.Rows[0]["PurposeId"].ToString(), dt.Rows[0]["ExpLoanAmt"].ToString(), dt.Rows[0]["ExpLoanTenure"].ToString(), dt.Rows[0]["EmiPayingCapacity"].ToString(), dt.Rows[0]["ExistingLoanNo"].ToString(),
                           dt.Rows[0]["TotLnOS"].ToString(), dt.Rows[0]["ApplTitle"].ToString(), dt.Rows[0]["ApplFName"].ToString(), dt.Rows[0]["ApplMName"].ToString(), dt.Rows[0]["ApplLName"].ToString(),
                           dt.Rows[0]["ApplGender"].ToString(), dt.Rows[0]["ApplMaritalStatus"].ToString(), dt.Rows[0]["ApplEduId"].ToString(), dt.Rows[0]["ApplReliStat"].ToString(),
                           dt.Rows[0]["ApplReligion"].ToString(), dt.Rows[0]["ApplCaste"].ToString(), dt.Rows[0]["ApplPerAddrType"].ToString(), dt.Rows[0]["ApplPerHouseNo"].ToString(),
                           dt.Rows[0]["ApplPerStreet"].ToString(), dt.Rows[0]["ApplPerSubDist"].ToString(), dt.Rows[0]["ApplPerPostOffice"].ToString(), dt.Rows[0]["ApplPerLandmark"].ToString(),
                           dt.Rows[0]["ApplPerArea"].ToString(), dt.Rows[0]["ApplPerPIN"].ToString(), dt.Rows[0]["ApplPerDist"].ToString(), dt.Rows[0]["ApplPerVillage"].ToString(),
                           dt.Rows[0]["ApplPerContactNo"].ToString(), dt.Rows[0]["ApplPerStateId"].ToString(), dt.Rows[0]["ApplPreAddrType"].ToString(), dt.Rows[0]["ApplPreHouseNo"].ToString(),
                           dt.Rows[0]["ApplPreStreet"].ToString(), dt.Rows[0]["ApplPreVillageId"].ToString(), dt.Rows[0]["ApplPreSubDist"].ToString(), dt.Rows[0]["ApplPrePostOffice"].ToString(),
                           dt.Rows[0]["ApplPrePIN"].ToString(), dt.Rows[0]["ApplPreLandmark"].ToString(), dt.Rows[0]["ApplPreArea"].ToString(), dt.Rows[0]["ApplPhyFitness"].ToString(),
                           dt.Rows[0]["CoApplTitle"].ToString(), dt.Rows[0]["CoApplName"].ToString(), dt.Rows[0]["CoApplDOB"].ToString(), dt.Rows[0]["CoApplAge"].ToString(),
                           dt.Rows[0]["CoApplGender"].ToString(), dt.Rows[0]["CoApplMaritalStatus"].ToString(), dt.Rows[0]["CoApplRelation"].ToString(), dt.Rows[0]["CoApplEduId"].ToString(),
                           dt.Rows[0]["CoApplPerAddr"].ToString(), dt.Rows[0]["CoApplPerStateId"].ToString(), dt.Rows[0]["CoApplPerPIN"].ToString(), dt.Rows[0]["CoApplMobileNo"].ToString(),
                           dt.Rows[0]["CoApplAddiContactNo"].ToString(), dt.Rows[0]["CoApplPhyFitness"].ToString(),
                           dt1.Rows[0]["CoAppIncYN"].ToString(), dt1.Rows[0]["TypeOfInc"].ToString(),
                           dt1.Rows[0]["AgeOfKeyIncEar"].ToString(), dt1.Rows[0]["AnnulInc"].ToString(), dt1.Rows[0]["HouseStability"].ToString(), dt1.Rows[0]["TypeOfOwnerShip"].ToString(),
                           dt1.Rows[0]["TypeOfResi"].ToString(), dt1.Rows[0]["ResiCategory"].ToString(), dt1.Rows[0]["TotalFamMember"].ToString(), dt1.Rows[0]["NoOfChild"].ToString(),
                           dt1.Rows[0]["NoOfDependent"].ToString(), dt1.Rows[0]["NoOfFamEarMember"].ToString(), dt1.Rows[0]["LandHolding"].ToString(), dt1.Rows[0]["BankingHabit"].ToString(),
                           dt1.Rows[0]["PersonalRef"].ToString(), dt1.Rows[0]["Addr"].ToString(), dt1.Rows[0]["MobileNo"].ToString(), dt1.Rows[0]["ValidatedYN"].ToString(),
                           dt2.Rows[0]["MobilePhone"].ToString(), dt2.Rows[0]["Refrigerator"].ToString(), dt2.Rows[0]["TwoWheeler"].ToString(), dt2.Rows[0]["ThreeWheeler"].ToString(),
                           dt2.Rows[0]["FourWheeler"].ToString(), dt2.Rows[0]["AirConditioner"].ToString(), dt2.Rows[0]["WashingMachine"].ToString(), dt2.Rows[0]["EmailId"].ToString(),
                           dt2.Rows[0]["PAN"].ToString(), dt2.Rows[0]["GSTno"].ToString(), dt2.Rows[0]["ITR"].ToString(), dt2.Rows[0]["Whatsapp"].ToString(), dt2.Rows[0]["FacebookAc"].ToString(),
                           dt.Rows[0]["ACHolderName"].ToString(), dt.Rows[0]["AccNo"].ToString(), dt.Rows[0]["IfscCode"].ToString(), dt.Rows[0]["AccType"].ToString(),
                           dt3.Rows[0]["BusinessName"].ToString(), dt3.Rows[0]["BusiNameOnBoard"].ToString(), dt3.Rows[0]["PrimaryBusiType"].ToString(), dt3.Rows[0]["PrimaryBusiSeaso"].ToString(),
                           dt3.Rows[0]["PrimaryBusiSubType"].ToString(), dt3.Rows[0]["PrimaryBusiActivity"].ToString(), dt3.Rows[0]["WorkingDays"].ToString(),
                           dt3.Rows[0]["MonthlyTrunOver"].ToString(), dt3.Rows[0]["LocalityArea"].ToString(), dt3.Rows[0]["BusiEstdDt"].ToString(),
                           dt3.Rows[0]["BusinessAddr"].ToString(), dt3.Rows[0]["BusinessVintage"].ToString(), dt3.Rows[0]["BusiOwnerType"].ToString(), dt3.Rows[0]["BusiHndlPerson"].ToString(),
                           dt3.Rows[0]["PartnerYN"].ToString(), dt3.Rows[0]["NoOfEmp"].ToString(),
                           dt3.Rows[0]["ValueOfStock"].ToString(), dt3.Rows[0]["ValueOfMachinery"].ToString(), dt3.Rows[0]["BusiHours"].ToString(), dt3.Rows[0]["AppName"].ToString(),
                           dt3.Rows[0]["VPAID"].ToString(), dt3.Rows[0]["BusiTranProofType"].ToString(), dt3.Rows[0]["CashInHand"].ToString(), dt3.Rows[0]["BusiRef"].ToString(),
                           dt3.Rows[0]["Addr"].ToString(), dt3.Rows[0]["BusiMobileNo"].ToString(), dt3.Rows[0]["ValidateYN"].ToString(), dt3.Rows[0]["SecondaryBusiYN"].ToString(),
                           dt3.Rows[0]["NoOfSecBusi"].ToString(), dt3.Rows[0]["SecBusiType1"].ToString(), dt3.Rows[0]["SecBusiSeaso1"].ToString(), dt3.Rows[0]["SecBusiSubType1"].ToString(),
                           dt3.Rows[0]["SecBusiActivity1"].ToString(), dt3.Rows[0]["SecBusiType2"].ToString(), dt3.Rows[0]["SecBusiSeaso2"].ToString(), dt3.Rows[0]["SecBusiSubType2"].ToString(),
                           dt3.Rows[0]["SecBusiActivity2"].ToString(), dt4.Rows[0]["BusiIncYN"].ToString(), dt4.Rows[0]["AppCoAppSameBusiYN"].ToString(), dt4.Rows[0]["BusinessName"].ToString(),
                           dt4.Rows[0]["PrimaryBusiType"].ToString(), dt4.Rows[0]["PrimaryBusiSeaso"].ToString(), dt4.Rows[0]["PrimaryBusiSubType"].ToString(), dt4.Rows[0]["PrimaryBusiActivity"].ToString(),
                           dt4.Rows[0]["MonthlyTrunOver"].ToString(), dt4.Rows[0]["BusinessAddr"].ToString(), dt4.Rows[0]["BusinessVintage"].ToString(), dt4.Rows[0]["BusiOwnerType"].ToString(),
                           dt4.Rows[0]["ValueOfStock"].ToString(), dt.Rows[0]["ApplAge"].ToString(), dt.Rows[0]["BankName"].ToString(), dt.Rows[0]["CoAppPerAddrType"].ToString(),
                           dt.Rows[0]["CoApplPreAddrType"].ToString(), dt.Rows[0]["CoApplPreAddr"].ToString(), dt.Rows[0]["CoApplPreStateId"].ToString(),
                           dt.Rows[0]["CoApplPrePIN"].ToString(), dt.Rows[0]["ApplDOB"].ToString(), dt1.Rows[0]["FamilyAsset"].ToString(), dt1.Rows[0]["OtherAsset"].ToString(),
                           dt1.Rows[0]["OtherSavings"].ToString(), dt3.Rows[0]["OtherBusinessProof"].ToString(), dt.Rows[0]["ApplMobileNo"].ToString()
                           , dt.Rows[0]["ApplAddProfId"].ToString(), dt.Rows[0]["ApplAddProfNo"].ToString(), dt.Rows[0]["ApplIdentyPRofId"].ToString(),
                           dt.Rows[0]["ApplIdentyProfNo"].ToString(), dt.Rows[0]["CoApplIdentyPRofId"].ToString(), dt.Rows[0]["CoApplIdentyProfNo"].ToString(),
                           dt.Rows[0]["CoAppAddPRofId"].ToString(), dt.Rows[0]["CoAppAddPRofNo"].ToString(), dt.Rows[0]["UdyamAadhaarRegistNo"].ToString(),
                           dt.Rows[0]["FamilyPersonName"].ToString(), dt.Rows[0]["HumanRelationId"].ToString(),
                           dt.Rows[0]["IsAbledYN"].ToString(), dt.Rows[0]["SpeciallyAbled"].ToString()
                           ));
                    }
                    else
                    {
                        row.Add(new PDByBMDtl("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                            , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                            "", "", "", "", "", "", "", "", "", ""));
                    }
                }
                catch (Exception ex)
                {
                    row.Add(new PDByBMDtl("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                          , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                          "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                          "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                          "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                          "", "", "", "", "", "", "", "", "", "", ""));
                }
                finally
                {
                }
            }
            return Encrypt(JsonConvert.SerializeObject(row), xKey);
        }
        #endregion

        #region ChangePassword
        public string Mob_ChangePassword(string EncryptedRequest)
        {

            PostMob_ChangePassword postMob_ChangePassword = new PostMob_ChangePassword();
            byte[] xKey = null;
            string vMsg = "";
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                vMsg = "Unauthorized access";
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                postMob_ChangePassword = JsonConvert.DeserializeObject<PostMob_ChangePassword>(vRequestData);
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
                CRepository oCR = new CRepository();
                vMsg = oCR.Mob_ChangePassword(postMob_ChangePassword);
            }
            return Encrypt(vMsg, xKey);
        }
        #endregion

        #region ICICIDisbursement
        public ICICBalanceFetchResponse ICICBalanceFetch(PostBalEnqReq vPostBalEnqReq)
        {
            string strEncryptedData = string.Empty;
            string strDeryptedData = string.Empty;
            string strAPIResponse = string.Empty;
            string pIPAddress = string.Empty;
            string pUrl = string.Empty;
            CRepository oCR = null;
            ICICBalanceFetchResponse objResponse = new ICICBalanceFetchResponse("", "", "", "", "", "", "", "", "", "");
            try
            {
                string strJsonDatatoEncrypt = string.Empty;
                strJsonDatatoEncrypt = JsonConvert.SerializeObject(vPostBalEnqReq);
                //Call for encryption
                strEncryptedData = DataEncrypt(strJsonDatatoEncrypt);
                //Call For Balance Fetch
                // For Production Call *******************************#############################
                pIPAddress = ServerIP;//"45.248.57.81";// "bijliserver57.bijliftt.com"; //need to change for Production               
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
                    oCR = new CRepository();
                    oCR.SaveIciciBankLog(vPostBalEnqReq.ACCOUNTNO, strEncryptedData, strAPIResponse, pUrl, strJsonDatatoEncrypt, strDeryptedData);
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

        public string ICICIDisbursement(string pXml, string pUserId)
        {
            string responsedata = string.Empty;
            string vXML = "";
            DataTable dt = new DataTable();
            CRepository oCR = null;
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
                            pIPAddress = ServerIP;// "45.248.57.81";
                            pURL = "https://apibankingone.icicibank.com/api/Corporate/CIB/v1/Transaction";
                            strAPIResponse = ExecuteAPIRequest(strEncryptedData, pURL, pIPAddress);
                            //Call for Decryption
                            strDeryptedData = DataDeCrypt(strAPIResponse);
                            //------------------------Save LOG --------------------------------
                            try
                            {
                                oCR = new CRepository();
                                oCR.SaveIciciBankLog(vOBJ.UNIQUEID, strEncryptedData, strAPIResponse, pURL, strJsonDatatoEncrypt, strDeryptedData);
                            }
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
                            dtdata.Rows.Add(dr1);
                            dtdata.AcceptChanges();
                            if (dtdata.Rows.Count > 0)
                            {
                                using (StringWriter oSW = new StringWriter())
                                {
                                    dtdata.WriteXml(oSW);
                                    vXML = oSW.ToString();
                                }
                                oCR = new CRepository();
                                oCR.InsertNEFTTransferAPI(vXML, Convert.ToInt32(pUserId));
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

                    var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/ICICI_Pub.cer");
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

        public string DataDeCrypt(string APIresponsestr)
        {
            byte[] decodeAPIResponse = Convert.FromBase64String(APIresponsestr);
            var vStrMycertPub = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/BijliPrivateKey.pfx");
            string vActualResposetext = RSACryptoService.decryptRsa(decodeAPIResponse, vStrMycertPub);
            return vActualResposetext;
        }

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
        #endregion

        #region NupayCallBackURL

        public NupayStatus UpdateNupayStatus(string id, string accptd, string accpt_ref_no, string reason_code, string reason_desc,
            string reject_by, string npci_ref, string credit_datetime, string umrn, string auth_type)
        {
            DataTable dt = new DataTable();
            CRepository oCR = null;
            NupayStatus res = new NupayStatus("", "");
            oCR = new CRepository();
            int vErr = oCR.UpdateNupayStatus(id, accptd, accpt_ref_no, reason_code, reason_desc, reject_by, npci_ref, credit_datetime, umrn, auth_type);
            if (vErr == 0)
            {
                res = new NupayStatus("0000", "Data submitted successfully.");
            }
            else
            {
                res = new NupayStatus("0001", "Failed to submit data.");
            }
            return res;
        }

        #endregion

        //public string SMSAPITest()
        //{            
        //    CRepository oCR = null;         
        //    oCR = new CRepository();
        //    string vErr = oCR.SMSAPITest();           
        //    return "";
        //}

        #region WebHook
        public AadhaarOTPResponse WebHook()
        {
            var stream = OperationContext.Current.RequestContext.RequestMessage.GetBody<Stream>();
            var sr = new StreamReader(stream);
            string body = sr.ReadToEnd();
            //dynamic a = JsonConvert.DeserializeObject(body);
            AadhaarOTPResponse a = JsonConvert.DeserializeObject<AadhaarOTPResponse>(body);
            return a;
        }
        #endregion

        #region Prosidex
        public ProsidexResponse Prosidex(ProsiReq pProsiReq)
        {
            CRepository oCR = new CRepository();
            ProsidexResponse pResponseData = null;
            pResponseData = oCR.Prosidex(pProsiReq);
            return pResponseData;
        }

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            CRepository oCR = new CRepository();
            ProsidexResponse pResponseData = null;
            pResponseData = oCR.ProsidexSearchCustomer(prosidexRequest);
            return pResponseData;
        }

        public string ProsidexSearchCustomerTest(ProsidexRequest prosidexRequest)
        {
            string vResponse = "";
            try
            {
                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                string postURL = "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
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
                return vResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                return vResponse;
            }
            finally
            {
            }
        }
        #endregion

        #region Jocata
        public string JocataRequest(string pMemberID, Int32 pPdID, Int32 pCreatedBy)
        {
            CRepository oCR = new CRepository();
            string pResponseData = oCR.JocataRequest(pMemberID, pPdID, pCreatedBy);
            return pResponseData;
        }
        #endregion

        #region ServerTime
        public string TimeChk()
        {
            byte[] xKey = null;
            ServerStatus ss = new ServerStatus();
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {                
                ss.Status = "false";
                ss.StatusDesc = "Failed: Unauthorized access.";
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                //-------------------------------               
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
            }
            return Encrypt(JsonConvert.SerializeObject(ss), xKey);
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

        public string BankAcVerify(string EncryptedRequest)
        {
            FingPayRequest req = new FingPayRequest();
            CRepository oCR = new CRepository();
            byte[] xKey = null;
            FingPayResponse vResp = null;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req1 = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req1.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                vResp = new FingPayResponse(false, "Unauthorized access", 500, "", "");
            }
            else
            {
                //---------------------------05.08.2025------------------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData1 = Decrypt(EncryptedRequest, xKey);
                req = JsonConvert.DeserializeObject<FingPayRequest>(vRequestData1);
                //-------------------------------------------------------------
                string vLoginId = "unity", vPin = "81dc9bdb52d04dc20036dbd8313ed055", vResponse = "", vMemberId = "";
                int vPdId = 0, vCreatedBy = 0;
                ImpsBeneDetailsRequestDataModel DM = new ImpsBeneDetailsRequestDataModel();
                DM.beneAccNo = req.beneAccNo;
                DM.beneIFSC = req.beneIFSC;

                vPdId = Convert.ToInt32(req.PDId); vMemberId = req.MemberId; vCreatedBy = Convert.ToInt32(req.CreatedBy);
                BankACReqData BankACReqData = new BankACReqData();
                BankACReqData.requestId = req.PDId;
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
                    string postURL = "https://fpanalytics.tapits.in/fpaepsanalytics/api/verification/bulk/bank";
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
                    //vResponse = "{\"apiStatus\":true,\"apiStatusMessage\":\"successful\",\"data\":[{\"beneAccNo\":\"37637233950\",\"beneIfscCode\":\"SBIN0013111\",\"timestamp\":\"03/07/2023 18:27:27\",\"statusCode\":\"0\",\"rrn\":\"318411635824\",\"beneName\":\"Mr  Kushal  Koley\",\"errorResponse\":\"Transaction Successful\",\"requestId\":\"1\",\"referrenceNo\":\"GV382660150307202318270027\"}],\"apiStatusCode\":0}";
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
                    oCR.SaveFingPayLog(vMemberId, vPdId, vRequestXml, vResponseXml, vCreatedBy);
                    //-------------------------------------------------------------------------------------
                }
                catch (WebException we)
                {
                    using (var stream = we.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        vResponse = reader.ReadToEnd();
                    }
                    vResp = new FingPayResponse(false, "failed", 500, "", "");
                    //----------------------------Save Log-------------------------------------------------
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                    oCR.SaveFingPayLog(vMemberId, vPdId, vRequestXml, vResponseXml, vCreatedBy);
                    //--------------------------------------------------------------------------------------
                }
            }
            return Encrypt(JsonConvert.SerializeObject(vResp), xKey);
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

        #region Decrypt
        public string Decrypt()
        {
            string vResponse = "d2KabQ+sZk1In0WKimtQKZ3Wtv0cQGK3AmpEYrXHWwEXSGjhF4Xf+6LYgZC28i0mSCUPcT6tzgKNdKkOgVE+C0qgcoHqYWt1BKvHHHJKc/0D0BWE2Vwn+CMCOjqO1k+tfjrmQgamoBtVULBgYQ7omfk4Eis5qAuuwZ5HwyfKYfRpKibc3wtxYxhapRBjOUddVzfiM4IrJdQ6AGTWLv8SUY6CyPboZpVsx+sc0Ao8dbI72nmFetdzyZnRibikzaMl07IEfyqFob3f7o7Rj7FlUGfRy/CtegQAcY17inYwyT/+w3ZdfUg4vEJoF8BMxkhuo331tvkAgAGHU98RuC3RW5qtWyv6eCBnQ4UqH93ICc6jErSPjZGmiYie6OTFNKSu2sXnOnIGiH0RxS+kO13i0GbJBNeNZwtdgzkOrtDi6ZidlNFQ5v01+wpNPfYeSLghE76Ax0NEWquIaEjPwK6KiJCvSWKuKwCKWbWnBDL8YYZBlQ+cZ2NPQO6V35Ssaf0qWtACpQ3AKWag+r1RwyX7xkleK+qZHJ12+NBun1DzxYJO5XJoarwo7YiK98+I4T8ZerS50TyolMhs5aDBEogtT6DUa1djiEep/mrxSmAWfFMq4+6Ni9IrPqzJ4G0HCjbsdYnA4agWHRTMtdr7hWJHuk9LlzyxFPrqR0CjOMUuzIw=";
            return DataDeCrypt(vResponse);
        }
        #endregion

        #region InsertBulkCollection
        public Int32 InsertBulkCollection(string pAccDate, string pTblMst, string pTblDtl, string pFinYear, string pBankLedgr, string pCollXml,
        string pBrachCode, string pCreatedBy, string pBCProductId)
        {
            int vErr = 0;
            CRepository oCR = new CRepository();
            try
            {
                vErr = oCR.InsertBulkCollection(pAccDate, pTblMst, pTblDtl, pFinYear, pBankLedgr, pCollXml, pBrachCode, pCreatedBy, pBCProductId);
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

        #region AadhaarVault
        public AadhaarNoByRefId GetAadhaarNoByRefId(AadhaarNoReq pAadhaarNoReq)
        {
            AadhaarNoByRefId oAN = new AadhaarNoByRefId("", 0, "", 0, 0, null);
            CRepository oRep = new CRepository();
            oAN = oRep.GetAadhaarNoByRefId(pAadhaarNoReq);
            return oAN;
        }
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

        #region IDFY Voter ID Verification
        public KYCVoterIDResponse IdfyVoterVerify(PostVoterData PostVoterData)
        {
            CRepository oCR = new CRepository();
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
            oCR.SaveIdfyVoterLog(vRequestID, vRes, PostVoterData);
            return oKVIR;
        }
        #endregion

        #region IDFY Aadhar Verification
        public IdfyAadharVerifyData IdfyAadharVerify(PostAadharData postAadharData)
        {
            CRepository oCR = new CRepository();
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
            oCR.SaveIdfyAadhaarLog(vRefId, postAadharData, vRes);
            IdfyAadharVerifyData vAadharData = new IdfyAadharVerifyData(vDigiLockerUrl, vRefId);
            return vAadharData;
        }

        public string IdfyAadharVerifyData(IdfyAadharVerifyResponseData IdfyAadharData)
        {
            CRepository oCR = new CRepository();
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
                oCR.UpdateIdfyAadhaarLog(vReqId, pData, vStatus, vUid);
            }
            finally { }
            return vStatus;
        }

        public string IdfyAadharVerifyJson(string pReqId)
        {
            CRepository oCR = null;
            string vStatus = "";
            System.Threading.Thread.Sleep(30000);
            for (int i = 0; i <= 24; i++)
            {
                oCR = new CRepository();
                vStatus = oCR.GetIdfyAadhaarLog(pReqId);
                if (vStatus != "")
                {
                    break;
                }
                System.Threading.Thread.Sleep(10000);
            }
            return vStatus;
        }
        #endregion

        #region JocataRiskCat
        public string JocataRiskCat(RiskCatReq vRiskCatReq)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.JocataRiskCat(vRiskCatReq);
            return vErrDesc;
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

        #region GetBase64Image
        public string GetBase64Img(string pId, string pModule, string pImageName)
        {
            string base64image = "";
            if (pModule == "I")
            {
                base64image = ViewImage(pImageName, pId);
            }
            else
            {
                base64image = ViewPDImage(pImageName, pId);
            }
            return base64image;
        }

        public string ViewImage(string pImageName, string pEnquiryId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = InitialApproachURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pEnquiryId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }
        public string ViewPDImage(string pImageName, string pPdId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = PDURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pPdId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
        }

        #endregion

        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
        }
        #endregion

        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
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

        #region UpdateLogoutDtTime
        public string UpdateLogoutDt(string EncryptedRequest)
        {
            int vErr = 0;
            byte[] xKey = null;
            CRepository oCR = new CRepository();
            //------------------------------Get Header Request-------------------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                //row.Add(new PDByBMData("Unauthorized access", "", "", "", ""));
                vErr = 100;
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic obj = JsonConvert.DeserializeObject(vRequestData);
                string LoginId = obj.LoginId;
                try
                {
                    vErr = oCR.UpdateLogOutDt(Convert.ToInt32(LoginId));
                }
                catch
                {
                    vErr = 1;
                }
                finally { }
            }
            return Encrypt((vErr == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data."), xKey);
        }
        #endregion

        #region UpdateSessionTime
        public string UpdateSessionTime(string EncryptedRequest)
        {
            byte[] xKey = null;
            Int32 vRst = 0;
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                vRst = 100;
            }
            else
            {
                string LoginId = null;
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                dynamic vObj = JsonConvert.DeserializeObject(vRequestData);
                LoginId = vObj.LoginId;
                //---------------------------------               
                CRepository oCR = new CRepository();
                try
                {
                    vRst = oCR.UpdateSessionTime(Convert.ToInt32(LoginId));
                }
                catch
                {
                    vRst = 1;
                }
                finally { }
            }
            return Encrypt(vRst == 0 ? "Success: Successfully Updated." : "Failed: Failed to Update Data.", xKey);
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
        public string InsertLoginDt(string EncryptedRequest)
        {
            byte[] xKey = null;
            LoginReqData Req = new LoginReqData();
            LoginLogOutData oLD = new LoginLogOutData();
            //-----------------------Get Header Request--------------------
            IncomingWebRequestContext req = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = req.Headers;
            //-------------------------------------------------------------
            string pKey = headers["X-Session-Key"];
            if (pKey == null || pKey == "")
            {
                //row.Add(new MemberCreationSubData("Unauthorized access", "", "", "", ""));
            }
            else
            {
                //-----------05.08.2025---------------
                string vX_Key = RsaDecrypt(pKey);
                xKey = Convert.FromBase64String(vX_Key);
                string vRequestData = Decrypt(EncryptedRequest, xKey);
                Req = JsonConvert.DeserializeObject<LoginReqData>(vRequestData);
                //--------------------------------
                string vLoginId = "0";

                CRepository oRP = new CRepository();
                vLoginId = oRP.InsertLoginDt(Req);
                oLD.LoginId = vLoginId;
            }
            return Encrypt(JsonConvert.SerializeObject(oLD), xKey);
        }
        #endregion

        #region HighMarkProcess
        public string HighMarkProcess(string pEnqId, string pMemberName, string pDOB, string pAge, string pAsOnDate,
         string pAadhaar, string pPanId, string pVoterId, string pRationCard, string pRelativeName, string pRelationCode,
         string pContactNo, string pAddress, string pDistrictName, string pStateCode, string pPinCode)
        {
            string vXml = "";
            CRepository oRP = new CRepository();
            vXml = oRP.HighMarkProcess(pEnqId, pMemberName, pDOB, pAge, pAsOnDate, pAadhaar, pPanId, pVoterId, pRationCard, pRelativeName, pRelationCode,
                pContactNo, pAddress, pDistrictName, pStateCode, pPinCode);
            return vXml;
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

    }
}
