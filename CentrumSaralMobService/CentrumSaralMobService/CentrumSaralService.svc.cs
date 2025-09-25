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
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace CentrumSaralMobService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CentrumSaralService" in code, svc and config file together.
    public class CentrumSaralService : ICentrumSaralService
    {
        #region WebConfig Variables
        string vDBName = ConfigurationManager.AppSettings["DBName"];
        string vAccessTime = ConfigurationManager.AppSettings["AccessTime"];
        string vWebHookUrl = ConfigurationManager.AppSettings["WebHookUrl"];

        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        string InitialBucket = ConfigurationManager.AppSettings["InitialBucket"];
        string MemberBucket = ConfigurationManager.AppSettings["MemberBucket"];
        string PDBucket = ConfigurationManager.AppSettings["PDBucket"];
        string LUCBucket = ConfigurationManager.AppSettings["LUCBucket"];

        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string PDURL = ConfigurationManager.AppSettings["PDURL"];
        string ServerIP = ConfigurationManager.AppSettings["ServerIP"];

        string vKeyId = ConfigurationManager.AppSettings["KeyId"];
        string vOuId = ConfigurationManager.AppSettings["OuId"];
        string vSecret = ConfigurationManager.AppSettings["Secret"];
        string vAccountId = ConfigurationManager.AppSettings["AccountId"];
        string vApiKey = ConfigurationManager.AppSettings["ApiKey"];

        string X_IBM_Client_Id = ConfigurationManager.AppSettings["X_IBM_Client_Id"];
        string X_IBM_Client_Secret = ConfigurationManager.AppSettings["X_IBM_Client_Secret"];
        string X_Client_IP = ConfigurationManager.AppSettings["X_Client_IP"];
        string ProdYN = ConfigurationManager.AppSettings["ProdYN"];

        string IBMAadhaarUrl = ConfigurationManager.AppSettings["IBMAadhaarUrl"];

        #endregion

        #region SendOTP
        public string SendOTP(OTPData objOTPData)
        {
            CRepository OCR = new CRepository();
            string result = "";
            Int32 vErr = 0;
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string vOTP = objOTPData.pOTP;
                string vMsgBody = "Thank you for your loan application with Unity SFB. Your verification OTP is " + vOTP + ".";
                //********************************************************************
                String sendToPhoneNumber = objOTPData.pMobileNo;
                vErr = OCR.InsertInitialOTPLog(sendToPhoneNumber, vOTP);
                if (vErr == 0)
                {
                    String userid = "2000243134";
                    String passwd = "ZFimpPeKx";
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
                else if (vErr == 6)
                {
                    result = "Failed : Maximum OTP limit exceeded..";
                }
                else
                {
                    result = "Error sending OTP..";
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
        #endregion

        #region GetAppVersion
        public string GetAppVersion(string pVersion)
        {
            try
            {
                string VersionCode = ConfigurationManager.AppSettings["MobAppVersionCode"];
                if (Convert.ToInt32(VersionCode) > Convert.ToInt32(pVersion))
                {
                    return "https://centrumsaralmob.bijliftt.com/SaralVyapar.apk";
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
                //-----------08.07.2025---------------
                string vX_Key = RsaDecrypt(userData.pKey);
                byte[] xKey = Convert.FromBase64String(vX_Key);
                userData.pPassword = Decrypt(userData.pPassword, xKey);
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
                            rs["DayEndDate"].ToString(), rs["AttStatus"].ToString(), vAttFlag, rs["AreaCat"].ToString(), rs["Designation"].ToString(), rs["AllowManualEntry"].ToString(),
                            rs["AllowQRScan"].ToString(), rs["LoginId"].ToString(), rs["MFAYN"].ToString(), rs["MFAOTP"].ToString(), rs["MobileNo"].ToString(),
                            rs["AllowAdvYN"].ToString(), rs["ImgMaskingYN"].ToString(), rs["DialogToImageYN"].ToString()));
                        if (rs["MFAYN"].ToString() == "Y")
                        {
                            SendMFAOTP(rs["MFAOTP"].ToString(), rs["MobileNo"].ToString());
                        }
                    }
                }
                else
                {
                    row1.Add(new EmpData("", "", "", "", "Login Failed", "", "", "N", "", "", "", "", "", "", "", "", "", "", ""));
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

        public KYCVoterIDResponse KarzaVoterIDKYCValidation(KYCVoterIDRequest vPostVoterData)
        {
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

            //string postURL = "https://testapi.karza.in/v2/voter";
            string postURL = "https://api.karza.in/v2/voter";
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
                    //request.CookieContainer = new CookieContainer();
                    //
                    //request.Headers.Add("cache-control", "no-cache");
                    request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
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
                oCR = new CRepository();
                oCR.SaveKarzaVoterVerifyData(vVoterID, vXml, vBranch, vEoId);

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
            string postURL = ProdYN == "N" ? "https://testapi.karza.in/v3/aadhaar-xml/otp" : "https://api.karza.in/v3/aadhaar-xml/otp";
            string xKarzaKey = ProdYN == "N" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
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
            string postURL = ProdYN == "N" ? "https://testapi.karza.in/v3/aadhaar-xml/file" : "https://api.karza.in/v3/aadhaar-xml/file";
            string xKarzaKey = ProdYN == "N" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
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
            string postURL = ProdYN == "N" ? "https://testapi.karza.in/v3/eaadhaar/otp" : "https://api.karza.in/v3/eaadhaar/otp";
            string xKarzaKey = ProdYN == "N" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
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
            string postURL = ProdYN == "N" ? "https://testapi.karza.in/v3/eaadhaar/file" : "https://api.karza.in/v3/eaadhaar/file";
            string xKarzaKey = ProdYN == "N" ? "wdycvLFD27R0RuAn2guz" : "VOht331uCcRtBgI";
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
        public List<KYCData> GetKYCInfo(PostKYCData postKYCData)
        {
            DataTable dt = new DataTable();
            List<KYCData> row = new List<KYCData>();
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
            return row;
        }
        #endregion

        #region SaveKYC
        public string SaveKYC(PostKYCSaveData postKYCSaveData, PostOCRData postOCRData, PostEMOCRData postEMOCRData)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SaveKYC(postKYCSaveData, postOCRData, postEMOCRData);
            return vErrDesc;
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
                        string vFileTag = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            if (vFileTag.Equals("GroupPhoto"))
                            {
                                vErr = SaveMemberImages(binaryWriteArray, "Group", kycNo, vFileTag);
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
                                vErr = SaveMemberImages(binaryWriteArray, "Member", kycNo, vFileTag);
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
                                vErr = SaveMemberImages(binaryWriteArray, "InitialApproach", kycNo, vFileTag);
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

        private string SaveMemberImages(byte[] imageBinary, string imageGroup, string folderName, string imageName)
        {
            string isImageSaved = "N";
            if (MinioYN == "N")
            {
                string folderPath = HostingEnvironment.MapPath("~/Files/" + imageGroup);
                System.IO.Directory.CreateDirectory(folderPath);
                string filePath = string.Format("{0}/{1}.png", folderPath, folderName + "_" + imageName);
                if (imageBinary != null)
                {
                    File.WriteAllBytes(filePath, imageBinary);
                    isImageSaved = "Y";
                }
            }
            else
            {
                string BucketName = imageGroup == "Member" ? MemberBucket : imageGroup == "PD" ? PDBucket : imageGroup == "LUC" ? LUCBucket : InitialBucket;
                isImageSaved = UploadFileMinio(imageBinary, imageName + ".png", folderName, BucketName, MinioUrl);
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
                        string vFileTag = "";
                        vFileTag = fileName.Substring(0, fileName.IndexOf('.'));
                        System.Drawing.Image img = LoadImage(binaryWriteArray);
                        if (img != null)
                        {
                            vErr = SaveMemberImages(binaryWriteArray, "PD", kycNo, vFileTag);
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

        #region GetAddressDtl
        public List<AddressData> GetAddressDtl(PostAddressData postAddressData)
        {
            DataTable dt = new DataTable();
            List<AddressData> row = new List<AddressData>();
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
            return row;
        }
        #endregion

        #region GetStateList
        public List<StateData> GetStateList()
        {
            DataTable dt = new DataTable();
            List<StateData> row = new List<StateData>();
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
            return row;
        }
        #endregion

        #region GetMemberDtl
        public List<MemberData> GetMemberDtl(string pMemberId)
        {
            DataTable dt, dt1 = new DataTable();
            DataSet ds = new DataSet();
            List<MemberData> row = new List<MemberData>();
            List<EarningMemberKYCData> row1 = new List<EarningMemberKYCData>();
            CRepository oCR = new CRepository();
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
            return row;
        }
        #endregion

        #region GetLoanScheme
        public List<SchemeData> GetLoanScheme(PostSchemeData postSchemeData)
        {
            DataTable dt = new DataTable();
            List<SchemeData> row = new List<SchemeData>();
            try
            {
                CRepository oCR = new CRepository();
                dt = oCR.GetLoanType(postSchemeData);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new SchemeData(rs["LoanTypeId"].ToString(), rs["LoanType"].ToString(), rs["IsTopup"].ToString(), rs["IsCBYN"].ToString()));
                    }
                }
                else
                {
                    row.Add(new SchemeData("No data available", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new SchemeData("No data available", ex.ToString(), "", ""));
            }
            finally
            {
            }
            return row;
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
        public List<MemberCreationSubData> MemberCreationList(PostMemberData postMemberData)
        {
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<MemberCreationSubData> row = new List<MemberCreationSubData>();
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
            return row;
        }
        #endregion

        #region NewMemberCreationData
        public List<GetNewMemberInfo> NewMemberCreationData(PostMemberFormData postMemberFormData)
        {
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<GetNewMemberInfo> row = new List<GetNewMemberInfo>();
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
                            rs["AddProfNo"].ToString(), rs["CoAppIdentyPRofId"].ToString(), rs["CoAppIdentyProfNo"].ToString(), rs["AddrType"].ToString(), rs["HouseNo"].ToString(),
                            rs["Street"].ToString(), rs["Area"].ToString(), rs["Village"].ToString(), rs["WardNo"].ToString(), rs["District"].ToString(), rs["State"].ToString(),
                            rs["Landmark"].ToString(), rs["PostOff"].ToString(), rs["PIN"].ToString(), rs["MobileNo"].ToString(), rs["AddrType_p"].ToString(), rs["HouseNo_p"].ToString(),
                            rs["Street_p"].ToString(), rs["Area_p"].ToString(), rs["VillageId_p"].ToString(), rs["WardNo_p"].ToString(), rs["Landmark_p"].ToString(),
                            rs["PostOff_p"].ToString(), rs["PIN_p"].ToString(), rs["ContactNo"].ToString(), rs["MemStatus"].ToString(), rs["CoAppName"].ToString(),
                            rs["CoApplDOB"].ToString(), rs["CoAppMobileNo"].ToString(), rs["CoAppRelationId"].ToString(), rs["CoAppAddress"].ToString(),
                            rs["CoAppState"].ToString(), rs["CoAppPinCode"].ToString(), rs["LoanTypeId"].ToString(), rs["TotalOS"].ToString(), rs["NoOfOpenLoan"].ToString(),
                            rs["AmountEligible"].ToString(), rs["MinLoanAmt"].ToString(), rs["MaxLoanAmt"].ToString(), rs["ImgValidationYN"].ToString(),
                            rs["ApplReligion"].ToString(), rs["ApplCaste"].ToString(), rs["IsAbledYN"].ToString(), rs["SpeciallyAbled"].ToString(),
                            rs["PreApproveLnYN"].ToString()
                            ));
                    }
                }
                else
                {
                    row.Add(new GetNewMemberInfo("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetNewMemberInfo("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                    "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
            }
            return row;
        }
        #endregion

        #region SavePdBySO
        public string SavePdBySO(PostPdBySo postPdBySo)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SavePdBySO(postPdBySo);
            return vErrDesc;
        }

        public SavePdBySORes SavePdBySO_XX(postPdBySoXX postPdBySo)
        {
            PostPdBySo objPdBySo = new PostPdBySo();
            objPdBySo = postPdBySo.postPdBySo;
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SavePdBySO(objPdBySo);
            SavePdBySORes res = new SavePdBySORes();
            res.SavePdBySOResult = vErrDesc;
            return res;
        }
        #endregion

        #region SavePdByBM
        public string SavePdByBM(PostPdByBM postPdByBM)
        {
            CRepository oCR = null;
            string vErrDesc = "";
            oCR = new CRepository();
            vErrDesc = oCR.SavePdByBM(postPdByBM);
            return vErrDesc;
        }
        #endregion

        #region GetBusinessTypeList
        public List<BusinessTypeData> GetBusinessTypeList()
        {
            DataTable dt = new DataTable();
            List<BusinessTypeData> row = new List<BusinessTypeData>();
            CRepository oCR = new CRepository();
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
            return row;
        }
        #endregion

        #region GetBusiSubTypeByTypeId
        public List<BusinessSubTypeData> GetBusinessSubType(string pBusiTypeId)
        {
            DataTable dt = new DataTable();
            List<BusinessSubTypeData> row = new List<BusinessSubTypeData>();
            CRepository oCR = new CRepository();
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
            return row;
        }
        #endregion

        #region GetBusinessActivity
        public List<BusinessActivityData> GetBusinessActivity(string pBusiSubTypeId)
        {
            DataTable dt = new DataTable();
            List<BusinessActivityData> row = new List<BusinessActivityData>();
            CRepository oCR = new CRepository();
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
            return row;
        }
        #endregion

        #region GetIFSCDtl
        public List<IFSCData> GetIFSCDtl(PostIFSCData postIFSCData)
        {
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<IFSCData> row = new List<IFSCData>();
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
            return row;
        }
        #endregion

        #region GetPdByBMData
        public List<PDByBMData> GetPdByBMData(PostPdByBMData postPdByBMData)
        {
            DataTable dt = new DataTable();
            CRepository oCR = new CRepository();
            List<PDByBMData> row = new List<PDByBMData>();
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
            return row;

        }
        #endregion

        #region GetPdDtlByPdId
        public List<PDByBMDtl> GetPdDtlByPdId(PostPdDtl postPdDtl)
        {
            DataSet ds = null;
            DataTable dt, dt1, dt2, dt3, dt4 = null;
            CRepository oCR = new CRepository();
            List<PDByBMDtl> row = new List<PDByBMDtl>();
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
                       dt.Rows[0]["FamilyPersonName"].ToString(), dt.Rows[0]["HumanRelationId"].ToString(), dt.Rows[0]["ImgValidationYN"].ToString(),
                       dt.Rows[0]["IsAbledYN"].ToString(), dt.Rows[0]["SpeciallyAbled"].ToString(), dt.Rows[0]["PreApproveLnYN"].ToString()));
                }
                else
                {
                    row.Add(new PDByBMDtl("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                        , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new PDByBMDtl("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""
                      , "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                      "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                      "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {
            }
            return row;

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
            CRepository oCR = new CRepository();
            string vMsg = oCR.Mob_ChangePassword(postMob_ChangePassword);
            return vMsg;
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

        public ProsidexResponse PosidexEncryption(ProsiReq pProsiReq)
        {
            CRepository oCR = new CRepository();
            ProsidexResponse pResponseData = null;
            pResponseData = oCR.PosidexEncryption(pProsiReq);
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
            CRepository oCR = new CRepository();
            string vLoginId = "unity", vPin = "81dc9bdb52d04dc20036dbd8313ed055", vResponse = "", vMemberId = "";
            int vPdId = 0, vCreatedBy = 0;
            FingPayResponse vResp = null;
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
                string postURL = "https://fpcorp.tapits.in/fpbeneverification/api/verification/bulk/bank";
                //string postURL = "https://fpanalytics.tapits.in/fpaepsanalytics/api/verification/bulk/bank";
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
            Int32 vRst = 0;
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
            CRepository oRP = new CRepository();
            vLoginId = oRP.InsertLoginDt(Req);
            oLD.LoginId = vLoginId;
            return oLD;
        }
        #endregion

        #region GetCollectionByLoanId
        public List<GetCollectionByLoanId> GetCollectionByLoanId(PostCollectionByLoanId postCollectionByLoanId)
        {
            DataTable dt = new DataTable();
            List<GetCollectionByLoanId> row = new List<GetCollectionByLoanId>();
            CRepository oCR = new CRepository();
            try
            {
                dt = oCR.GetCollectionByLoanId(postCollectionByLoanId.pLoanId, (postCollectionByLoanId.pCollDt), postCollectionByLoanId.pBranch);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetCollectionByLoanId(rs["ClosingType"].ToString(), rs["LoanId"].ToString(), rs["DisbDate"].ToString(), rs["IntRate"].ToString(),
                           rs["DisbAmt"].ToString(), rs["PrincpalDue"].ToString(), rs["InterestDue"].ToString(), rs["PrncOutStd"].ToString(), rs["IntOutStd"].ToString(),
                           rs["PaidPric"].ToString(), rs["PaidInt"].ToString(), rs["AdvanceAmt"].ToString(), rs["LoanTypeId"].ToString(),
                           rs["LoanTypeName"].ToString(), rs["FunderName"].ToString(), rs["LastTransDt"].ToString(), rs["EMIAmt"].ToString(), rs["FlDGBal"].ToString(),
                           rs["IntDue"].ToString(), rs["CollMode"].ToString(), rs["ODAmount"].ToString(), rs["NoofDays"].ToString(), rs["PenDuDate"].ToString(), rs["PrePenDue"].ToString(), rs["PenaltyAmt"].ToString(),
                           rs["PenCGST"].ToString(), rs["PenSGST"].ToString(), rs["VisitCharge"].ToString(), rs["VisitChargeCGST"].ToString(), rs["VisitChargeSGST"].ToString(),
                           rs["VisitChargeDue"].ToString(), rs["BounceCharge"].ToString(), rs["BounceChargeCGST"].ToString(), rs["BounceChargeSGST"].ToString(), rs["BounceChargeDue"].ToString(),
                           rs["IsNPA"].ToString(), rs["IsPDD"].ToString(), rs["MemberName"].ToString(), rs["MemberNo"].ToString(),
                            rs["loanNo"].ToString(), rs["EMISequence"].ToString(), rs["TotPreMatAmt"].ToString(), rs["ExcessAmt"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetCollectionByLoanId("No data available", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetCollectionByLoanId("No data available", ex.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "",
                        "", "", "", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }
        #endregion

        #region Mob_GetLoanByMemByLo
        public List<GetLoanByMemByLo> Mob_GetLoanByMemByLo(PostLoanByMemByLo postLoanByMemByLo)
        {
            DataTable dt = new DataTable();
            List<GetLoanByMemByLo> row = new List<GetLoanByMemByLo>();
            CRepository oCR = new CRepository();
            try
            {
                dt = oCR.Mob_GetLoanByMemByLo(postLoanByMemByLo.pLoId, (postLoanByMemByLo.pAsOnDt), postLoanByMemByLo.pBranchCode, postLoanByMemByLo.pPTPYN);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLoanByMemByLo(rs["LoanNo"].ToString(), rs["LoanId"].ToString(), rs["MemberNo"].ToString(), rs["MemberId"].ToString(),
                           rs["MemberName"].ToString(), rs["ApplPerAddr"].ToString(), rs["ApplPreAddr"].ToString(), rs["BranchCode"].ToString()));
                    }
                }
                else
                {
                    row.Add(new GetLoanByMemByLo("No data available", "", "", "", "", "", "", ""));
                }
            }
            catch (Exception ex)
            {
                row.Add(new GetLoanByMemByLo("No data available", ex.ToString(), "", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }
        #endregion

        #region Mob_Srv_SaveCollection
        public GetSaveCollection Mob_Srv_SaveCollection(PostSaveCollection postSaveCollection)
        {
            GetSaveCollection oSC = new GetSaveCollection();
            CRepository oRP = new CRepository();
            oSC = oRP.Mob_Srv_SaveCollection(postSaveCollection);
            return oSC;
        }
        #endregion

        #region ForgotPassword
        public ForgotOTPRes SendForgotOTP(ForgotOTPData objOTPData)
        {
            DataTable dt = new DataTable();
            WebRequest request = null;
            HttpWebResponse response = null;
            string result = "", vOTPId = "0", vOTP = "", vMobileNo = "", vUserName = "";
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
                        oFO = new ForgotOTPRes(vOTP, "200", "Success:OTP has been successfully sent.", vOTPId);
                    }
                    else
                    {
                        result = "Please try after 5 minutes..";
                        oFO = new ForgotOTPRes(vOTP, "201", "Failed:" + result, vOTPId);
                    }
                }
                else
                {
                    result = "Invalid User Name or Password..";
                    oFO = new ForgotOTPRes(vOTP, "401", "Failed:" + result, vOTPId);
                }
            }
            catch (Exception exp)
            {
                result = "Error sending OTP.." + exp.ToString();
                oFO = new ForgotOTPRes(vOTP, "500", "Failed:" + result, vOTPId);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 60, "@pUserName", Post_ForgotPassword.pUserName);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, Post_ForgotPassword.pPassword.Length + 1, "@pPassword", Post_ForgotPassword.pPassword);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOTPId", Convert.ToInt32(Post_ForgotPassword.pOTPId));
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pOTP", Post_ForgotPassword.pOTP);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 1, "@pMobYN", "M");
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", 0);
                DBUtility.Execute(oCmd);
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
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.Int, 4, "@pOTPId", pOTPId);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Input, SqlDbType.VarChar, 6, "@pOTP", pOTP);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.Int, 4, "@pErr", pErr);
                DBUtility.AddParameter(oCmd.Parameters, ParameterDirection.Output, SqlDbType.VarChar, 200, "@pErrDesc", "");
                DBUtility.Execute(oCmd);
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

        #region LUC
        #region GetLoanUtilizationQsAns
        public List<GetLoanUtilizationQsAns> GetLoanUtilizationQsAns()
        {
            DataTable dt = new DataTable();
            List<GetLoanUtilizationQsAns> row = new List<GetLoanUtilizationQsAns>();
            CRepository oCR = new CRepository();
            try
            {
                dt = oCR.GetLoanUtilizationQsAns();

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
        #endregion

        #region GetLUCPendingDataList
        public List<GetLUCPendingDataList> GetLUCPendingDataList(PostLUCPendingDataList postLUCPendingDataList)
        {
            DataTable dt = new DataTable();
            List<GetLUCPendingDataList> row = new List<GetLUCPendingDataList>();
            CRepository oCR = new CRepository();
            try
            {
                dt = oCR.GetLUCPendingDataList(postLUCPendingDataList.pEoId, postLUCPendingDataList.pDate, postLUCPendingDataList.pBranch);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow rs in dt.Rows)
                    {
                        row.Add(new GetLUCPendingDataList(rs["Eoid"].ToString(), rs["EoName"].ToString(), rs["LoanId"].ToString(), rs["MemberId"].ToString(),
                           rs["MemberName"].ToString(), rs["LoanAccountId"].ToString(), rs["Cycle"].ToString(), rs["DisbDate"].ToString()
                           , rs["LoanAmt"].ToString(), rs["PurposeID"].ToString(), rs["PurposeNm"].ToString(), rs["SubPurposeId"].ToString()
                           , rs["SubPurposeNm"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetLUCPendingDataList("No data available", "", "", "", "", "", "", "", "", "", "", "", ""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetLUCPendingDataList("No data available", "", "", "", "", "", "", "", "", "", "", "", ""));
            }
            finally
            {

            }
            return row;
        }
        #endregion

        #region SaveLoanUtilization
        public string SaveLoanUtilization(PostLoanUtilization postLoanUtilization)
        {
            int vErr = 0; string VErrMsg = "";
            CRepository oCR = new CRepository();
            try
            {
                VErrMsg = oCR.SaveLoanUtilization(postLoanUtilization);
            }
            finally { }
            return VErrMsg;
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
                                vErr = SaveMemberImages(binaryWriteArray, "LUC", MemNo, vFileTag);
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

        #region eIBMAadhaarOTP
        //public IBMAadhaarOTPResponse IBMAadhaarOTP(eIBMAadhaarOTP eIBMAadhaarOTP)
        //{
        //    CRepository oCR = new CRepository();
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
        //            oCR.SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, fullResponse, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
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
        //                oCR.SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, Response, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
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

        //    finally
        //    {
        //    }
        //}

        public IBMAadhaarOTPResponse IBMAadhaarOTP(eIBMAadhaarOTP eIBMAadhaarOTP)
        {
            CRepository oCR = new CRepository();
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
                    //string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    oCR.SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, fullResponse, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
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
                        oCR.SaveIBMAadhaarOtp("eAadhaarOtp", txn, requestBody, Response, eIBMAadhaarOTP.pBranch, eIBMAadhaarOTP.pEoID, eIBMAadhaarOTP.aadhaarNo);
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
        //    CRepository oCR = new CRepository();
        //    string sourceId = "MB";
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

        //            oCR.SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, fullResponse, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
        //        }
        //        finally
        //        {
        //        }
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
        //                oCR.SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, Response, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
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
            CRepository oCR = new CRepository();
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
                //string fullResponse = "";
                //fullResponse = "{\"KycRes\":{\"code\":\"ba1b6a0a02ff4d67ada251599581ee17\",\"ret\":\"Y\",\"ts\":\"2025-07-02T17:04:42.956+05:30\",\"ttl\":\"2026-07-02T17:04:42\",\"txn\":\"UKC:AUANSDL001:202507021704354175\",\"UidData\":{\"tkn\":\"01001006cazCZzszbCb78MF7r68csuOei593kWOKNu/SL0VFbd8nfNId8qaHO4T2IkfFuj7e\",\"uid\":\"********3355\",\"Poi\":{\"dob\":\"27-03-1979\",\"gender\":\"F\",\"name\":\"TAHERJAN GAYEN\"},\"Poa\":{\"country\":\"India\",\"dist\":\"Bankura\",\"house\":\"\\\\\",\"lm\":\"Hetagora\",\"pc\":\"722164\",\"po\":\"Bankadaha\",\"state\":\"West Bengal\",\"street\":\"\\\\\",\"subdist\":\"Bishnupur\",\"vtc\":\"Hetagara\"},\"Pht\":{\"base64image\":\"/9j/4AAQSkZJRgABAgAAAQABAAD/2wBDAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDL/2wBDAQkJCQwLDBgNDRgyIRwhMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjL/wAARCADIAKADASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDWUZIqbbUKsTKBVnFdcThYzFJgipMUY4qhEVJzUuKaRSsAzFJipCKaRRYBmKQg0/FJjmiwDCOKbjtUmKQikBGRSEU8imkUWAYRSYp9JiiwDDmk9sU/FIQKLAMpDT8d6TGaVgL/AJQEm4VNS7aTFUlYBKKXFJimAhpMU7FNxmgBD6U007BxTW49qQDaSlNRvNFGDudVx6mgdh9NJqD7dbE489M/WpQ6N0INK6YNNAaaTTunFGKAsMozS0hoAb1pDTiKQ9KYhpPWjOaCOaSkBsd6KKPeqGJjFJkUZpOtAgIpKXNJRcBDWbqerW+nL85DSEcIDzis3xN4qt9DhaFGV7th8q9l9zXk15rt9fzu7TN8xyTnk1lKbekTaEFvI73UfFEkhIadYEH8KHmuYm8TfvPl3N/tHrXOO7MeWLMepqMZJ/xrL2d/idzb2lvgVjq4vEybvmdgCOcitO31+2fCxz7D7NiuGjUOSD1qFx5ZOc1HsY300L9rLqj1+x1yQY3v5q9wev4V0MFxHcx742yOhHpXg1vqd1buGinYEdjzXaeGvFLSTqkmEmxjr8r1cXOG+qM5RhPbRnpFJimRSrNGsinKkU810p32OZprQSkzSmm9aYg4pKPWkz+dIRsYoxS0UwG7aTGBTj60mc0DGVieJ9fi8P6U9wdrXDcRRk9T6/QVutj/APVXhnjfWm1bxBP5cmbeJjHHg8YB/rwaym+iNKcb6swru7n1C6kuLhy7udxLHqadb20sn3E69zU9hY+fIAeR3rrrLTAAAAAAKynU5dEdNOnzas5qLSnwCTzU39mccnNdU2n4pn2Eg9K5nVkdSoxOXFgE6CoJ7E88ZrrmscDpVWWzBPSpVR3K9krHDzQNG3I4oidlZShwynKkV0t5pu5CRXPSwmKRlFdcJ33OSpTtsep+Dta+32Iic/vFHOfWurHNeReCbvZrKQFsb2yo98EV61GeorSm7PlOeqtpD6TFOpK1MRpFNIp5PFMHqaANk0UtGKoBppp64p+OaQipsBQ1S9+w6ZdXC8vFDJIo91Un+lfOmc8nk5r3fxFK0ukans6JAwH5c/1rw61tzNOgx7muWNRSnLyOtQ5YrzOi0iPEKnvXS27EEGsmwhWNFBOABWms0Ct98cVhO7OumklqacfOCRVyOBJF6YrOW8iVAQwJpTqJCZBwBWGnU2s2tC1dQJGMDrWVcgc4HStDzdwy7frVC6ZGY7XGMUbsFoZcpHOawNUgG4uOK3pACfvCsvU4S0ZxW1NtPUzqxTRhWF5JYajDdxH5o2DD8K96cBJcV4FbwGa7SDozsF/M4r3qdwbj0ro5rVY+ZxSV4PyJgaDTEORTq6jlGdTQTTqYaQG1mlpBS1YgNQTyeWo5wSamNZt87ETNzjaEXHv/APrFc2Jq+zhc2ow5pGRLMkttd2rg+ZLaSMw9MDn9TXkmlR5kJx7V6lEpa+1aRDuZbKWMD0wp/rn8q8/0a1AXdzzXFRuos7nq0OYy7sltqjgU2ZrdYiTOdwYKfYkEj+RrTm0tZBuJOR0FVJdLRpjKwIJIOB0JFWprqU4PoVEE8MhBdsr1Brd09pJ0GF4xWeI5bmd3kGfU1binktotoGAKyqWZrTuhuoXUkQK9DWI9xck/6zA9zWhdSmZskc5zVUxCZJIiOWGAfSimrBVbEVJgRvkzkZpXZ8bWzjsaEtGRnLZLFFjUjsFUKP0AqaC2mA/eHcPWtXJdGZRTMqzhH/CU6cpHElzEMeuXFezXH+t3jGSa8rht8eItIlxnbeRA/wDfYr1GZQu45J3ybue3AFHM3VgZTVoyJY2qQmoIulSnpXonAITSE0e1IaVwsb1JRQSB1qxEUzbQqjqxrLnclymfvydPYDH86uxHzrwyE8KPlH+frWNexmESz7mCxrK4xz33V5OMqc0+U9GhDkjqVPCUiXGrxPJgfakk3IR1z8x/SuJ0sNFaoHUiReGBHQ9667RnFpq9gypuKyO28/wr9z/0Fs1l6taCx8TanAR8pnMyHHZ/n49gWI/Ci6TsVDVkkREsYIXnvUUtozNyKhgma3lPGRWnDcAqxYfMKiVjqWxUis8ssQHzMcAVRWJrsEoOMVYvJ5lkV4m+YZ/DPFU4Jbq1ctkFe2O1V0J6lZ4vs853j5T+lTC2zhlXg9DUFw9xLMGOAnfPU1o6fOqptkGR6UnsNEOwrjIzSttVcnircksQyRjFZNzMZT8o49aIFSWhNpyLca3ZLnG2beOf7qlv6V6Bcjb5Qzycn+n9K4bwvAs+toSfmhRpF59tv8mrtrmTdcAf3FC1tSV6y8jiru0WSxA4FSjFQxHpUvQ16B54h6+lIetBOaQmgZuVn6ldtEgii5kf9BVyeZIIXkfgKKwor4SuzuhLH0om2ldG+HpOcrvZGjYl1jdpMZGP61maoWi0qVgu44C49ckA/wA601ceXjplsfl/k1k6reK+jSYGC0uwD1KSf/YV4lT3qp3yKMaCGMStyBKeB1YZJIHvhcVH4pi+029hqwfdKN1ncFRxvQnn6bg/1yKkjvlXCPGWUF3HPAc8Y/HOenrV+GL+1tM1DS5I9hmw8Dt0dgiMGHtuOO/SrmramUHZnGrLt4IprXux8E4quJd6gkFWHBB6g+lMlVZuvGKaR03sWGu42PzOKYL6HkA5qFYIlHIyfWlEcKryB0q9O5KuMmu4nxzihZ1UZDAio54omTKgYqs0YT7pNOya3Btpl8yiVOTUbttXbio0YKgUdabIXlljt4RmaQ7VFJLUqUtDqvB1uESe9kGAx2LnuBzkfjx+Fbgy0mSeScmobW3WzsILaM5VFxn19/xqxEvNdWFho59zzcRK75S1GOKeTSIOKU11WOcaaaaU80hpAJqV2tzepbbiIlPzEdzUluIFTEeD25rHtI3uJSV5weST61ppaLEQS3I9O9RiWlpc9mFNQjymhGnmbCeMEkfqKx9fhMGnxgHB+0lxz/e3n+tbUCMs8QB+URHj3+X/AOvWFr9wZNMsIyCZJojJ6Z2quf8A0KvHjrUIm9GYinzCpQnAkDNu75PX6VqvcO1zaSWMbD7JuiT/AG41Kc/jn8eKx/OWVsx5CnEYIHf/AD/OtbTrqNRAiIfOtgWkAGcBcDHT+Lb+ldM0c6OZ8VQ/2Zr5mCkW98vnoewY/eH58/8AAqoJOp712Xiewh1rwvvtcu1qBNEc8lMYI/Ec/UCvMd0sB9V7VENUdCZ0SAN3oeAORgVkQ6iQAGBqyuoqO5puPkaRdyzJbhRkmqjkLxTJb/cMA5qsJXnlEcal3POBVRiEmkWt4UZ6n0FbvhqxCLLrN3wFUiIH9TVXTdFupryK2G0mWMSySD+AZIx9eK19fuY49lhb4KIBnHr6U+V1JKnHr+REFzysti1ZaxFOv7w7G3EfrW5BtblSDXCwxBYgCeRzVq21eSyuVEZ3A9RmvVVJRjZE1sEneUXqd1nFNJrll8RXLAnA56e1bdhfpd2okZgrA4IzUuLRwzw84K7Lh4Bpp6daQMGzhh+FBx61JiZNmZFI8vOSe1aVublseZg+mfrUenSxpahNv3Sea0rYwzTqoJ5YfzrnxFR3eh7jkaCSRpdSAnDRRq30BJ/+JrC8QRpGLRF+9BHJxjsdoH/oNbF9Z+Yl88ZO6WARceoDY/8AQhWF4mQ/20f3mA1qnB/335rzKTTmc1TYwoZfOtI1eMRkTI+P7xG1v1YY/GtO0u5oNQci2BjkYMGHJz2/9CYVnIvmWqFmVfmSRWPYhv8AJrVEkou1fgZQkD/gWa6JswJNJmUWpi2HYoAQnGXVfkbOPQqPzrhNd03+ztUlgCjyziSP02nt+HI/Cu0tZ2fVITC/mRhiCvTHmNyOnqS1VfF+nD+zxc4Jkt3w3+4x/ocfnWDlyy9Topu5wccCk9B9KsCyB7ChIs8g05nkjGAavmZryoqT24Q7QoLHgCtfS9Oe2itpIot9zcggf7JWQjH0wAc+9S2GmTqI7ySMuWRsrjlcghSPfOD/AJ563TrGPSbI3l18szjdtP8AyzXA4+uc1Up8sTF+9KyIYIYvDekGMndO/LMe59PpXJvIZ7ssxJwcknuas6rqkt9MzEnBOEU/zqrGnlx89SK9LB4dwXPPdnfRp8qC4m2jr9KggQl/Mbqae8TSBOOe9SqNpAxwo6V2mlhZH8scfhTYror8qyEH64qjcXO+Qheg/Woog0koPpUyd9DCbWx0VpqVzEd4ckehrVtdc3SlZ8AZ4YVy8blCOeKsvhmBXoehrGSd9TkqUlLdHeWUNu1rHmUE7FJHua07OKJbpTnkk/1rmrWzuG6AoBgc1vaVC6TDfIXznr9MVwV4JX942krLc0LgsYnKNwbmLp/d3ID/AFrmfEZH9sOVy7qixsPYKW/9m/WuggfCxRnnddSqD9C5/pXK68D/AG3qDccbDuPQARp/+uuOjH3jnqbGfFGtxZrDKcDC59xn/CrEl+BdJI/KJG5IHrwR/wCg4qqXVIYEX5giqoOeqgY/pT1G07io8oIfrgjr+mK6ZRuY2NY20Nlp17cRTKs7tuXJ+4dv3voME10GqafBeQz28jACXdExHYFRz+WK4k4ntbmEHaZ42iQk8RqSAT+C5rvJY9skzMeDKrDJ6fKq1yVk1Z3NKZ4f5k0BKSKVkQlXX0YcEVo6dbPczEzIwG0lVI+8ccfritDULKOw8XX8hXdFHOZsH1ZRIf1augs7OLVriF0j2R4mFy4bthNvXoTlj9Aa1Vlq9jSU3si14fjm+yfbr1FVIRtQf3jjaW+nJx9TWBr2tPfSsiN+5U8D+9V3XNbDq2n2ZxAvylh3+lYa24X94/PoK7cJh+Z+0mvQ6qFKyuyGGDI82T7x6A+lTBN1ShSRkjipEUY4Feo9DqsRRpx0HtVbUyYbcEZDScCteNI1TzpSFhTqT0z6Vy95eyX9yZ2GB0RR/CK5+f3+VGU522IUiJwKuKgiXgcmkhjwMnj2qbGXArVIx8xig4561bt228EfKevtUYUc469alC7YuvJptJktXOottXmyd/zAgE5rf0idprhB5brliPmHtWFpk1kY0LoFfZtYnocV0FhJGLpHUDl/x5rzMRy6qwT22LVxB5MkLAnEc8kpz/tb/wD4quZ1e3e71e5twwVHYE543YjXj6dT+FdTqWXtJ9owfOiUH1BZAf5muP1eKVr67lEpjAdfLkPBVyqoB9DnFcFJ6nNPYomCOEJDjcFUMm3oAR0/Mk/jUMqEutyWJl788beOPpg0+ZDayWtuTubzsSEdWYIefpgAflTYrMmfLyFogC0UZPIx1/L+tdafmZCzReaj2wOBJES56YUjpnt1r0a4hE58stzuRx6ghsj9VrzSRZ00+W5RszQeYUdkBDbeCPocfp7V6kqKt7JIP+eaDn/ebH865cQ1ZGlPc4LxBaS3Pi2Kyt4wfNtxJI5HAyzKSfoqriotT1OHT7Y6Xp7EvnEsg6k4x+da3jS7fT5oPs0eJrmJot4HIC7eP/H65O3gEeWk+ZzyTXXhaHtEpS2R10qafvMZFbEDzJBz6U7cSMH8qllfONpwKhHPXpXrxR2IUSfw44FOMigZZgAOvtUbsEB6VnvL55Iydv8AOiSQNi6pqH2uOK2iysK/MQf4j61Ut4tzliPlX2rbbRV+w/aOfMCbmHpWbAMREjuawpOEr8pzuzHY5Uepp2B5x9BTSf3i9qVn+cAfjW4i9plsLidi33EXJ96bdr8xKjCZOMfpWpo8Q+xFsEeZOFLDuAMj9TUesNELoRrjKqAQO3+eK5FUk67S2I3lYn0+xllijcH5Xycg/wCfSt/T7Y206Euz/MpxnHI60UVzV6suZxFKT2N++aRLX9zjc08Oc8/L5iBv0zXDam0c9zeQSGXKnLlRnaV2MPqT8vH1+tFFefS+I5pbFWB7ZIljXzN5UttkB34B3dTznjH4H6VLYwxw3sjZZ/3cpbL7cK7B/l/ICiiul7Gdi+4N/NbqhMcMtxGCFHAViCBj3/mK7a4LEhQ207kZcDklWLEfkDRRXPX0sjSCOS8eORdaWR/Cswwf+2dcu8xPXFFFexgUvYo9Cj8CIsl29qHZYlLFv1oortvY2M6aZpz3CfzqS3jDSICAcEMR6gHpRRWNVtRbM5M6uKFjYCGQAF1KsB2z2/WuT4WJQO9FFcOBd5SMI9SKRvnqMtmWiivRGzoLK+NrppK43hjt/Edf0rHuLggHLEu3c0UVlCKi211GlZn/2Q==\"}}},\"status\":\"success\"}";
                //fullResponse = File.ReadAllText("C:/Users/kushal.k/Desktop/ibm.txt");
                try
                {
                    string vFulResp = System.Text.RegularExpressions.Regex.Unescape(fullResponse);
                    vFulResp = vFulResp.Replace("\\\"", "");
                    vFulResp = vFulResp.Trim('"').Replace("\\", "");
                    //  dynamic res = JsonConvert.DeserializeObject(vFulResp);
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

                    oCR.SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, vFulResp, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
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
                        oCR.SaveIBMAadhaarOtp("eAadhaarDownload", eIBMAadhaar.txn, requestBody, Response, eIBMAadhaar.pBranch, eIBMAadhaar.pEoID, eIBMAadhaar.aadhaarNo);
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

        #region GenerateRandomNo
        public string GenerateRandomNo()
        {
            int min = 1000;
            int max = 9999;
            Random rdm = new Random();

            return rdm.Next(min, max).ToString();
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

        #region AesDecrypt
        public static string Decrypt(string strToDecrypt, byte[] x_key)
        {
            try
            {
                byte[] keyBytes = new byte[32];
                byte[] ivBytes = new byte[16];
                Array.Copy(x_key, 0, keyBytes, 0, 32);
                Array.Copy(x_key, 0, ivBytes, 0, 16); // or use 16–32 if needed
                // Console.WriteLine("Key: " + Convert.ToBase64String(keyBytes));
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
        #endregion

        #region RSADecrypt
        public string RsaDecrypt(string pData)
        {
            string decryptedText = "";
            try
            {
                // Load the certificate with private key
                string certPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Certificate/BijliPrivateKey_Kushal.pfx");
                string certPassword = "ftt@123";
                X509Certificate2 cert = new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
                RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
                string encryptedBase64 = pData;
                byte[] encryptedData = Convert.FromBase64String(encryptedBase64);

                byte[] decryptedData = rsa.Decrypt(encryptedData, false); // 'false' = PKCS#1 v1.5
                decryptedText = Convert.ToBase64String(decryptedData);
            }
            catch (Exception x)
            {
            }
            return decryptedText;
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
                        row.Add(new GetBranchCtrlByBranchCode(rs["InitialAppSARAL"].ToString(), rs["AdvCollSARAL"].ToString(), rs["CashCollSARAL"].ToString(), rs["DigiAuthSARAL"].ToString(), rs["ManualAuthSARAL"].ToString(), rs["BioAuthSARAL"].ToString()));
                    }

                }
                else
                {
                    row.Add(new GetBranchCtrlByBranchCode("No data available", "", "","","",""));
                }

            }
            catch (Exception ex)
            {
                row.Add(new GetBranchCtrlByBranchCode("No data available", "", "","","",""));
            }
            finally
            {

            }
            return row;
        }

        

        #endregion
    }
}
