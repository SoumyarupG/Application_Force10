using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Web.Hosting;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Net.Security;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web;
using System.Globalization;
using System.Net.Mail;
using System.Xml.Serialization;
using System.Xml;



namespace CentrumCF_MobService
{
    public class CentrumCFService : ICentrumCF_MobService
    {
        private static string ApiKey = ConfigurationManager.AppSettings["ApiKey"];
        private static string MemberRefId = ConfigurationManager.AppSettings["MemberRefId"];
        private static string EnqMemUsrId = ConfigurationManager.AppSettings["EnqMemUsrId"];
        private static string EnqMemUPwd = ConfigurationManager.AppSettings["EnqMemUPwd"];
        private static string CustRefId = ConfigurationManager.AppSettings["CustRefId"];
        private static string vSSLPWD = ConfigurationManager.AppSettings["SSLPWD"];
        private static string ClientToken = ConfigurationManager.AppSettings["ClientToken"];
        private static string vCibilCertificatePath = "";
        private static string vCerName = ConfigurationManager.AppSettings["CerName"];

        private static string vCibilApiUrl = ConfigurationManager.AppSettings["CibilApiUrl"];
        private static string vCibilApiHostUrl = ConfigurationManager.AppSettings["CibilApiHostUrl"];
        private static string vServiceCode = ConfigurationManager.AppSettings["ServiceCode"];
        string ServerIP = ConfigurationManager.AppSettings["ServerIP"];

        #region VerifyCIBIL
        public string VerifyCIBIL(string pLeadId, string pApplicanType)
        {
            string fullResponse = "", Requestdata = "", Cust_Ref_Id = "";
            CRepository oRepo = null;
            DataTable dt = new DataTable();
            oRepo = new CRepository();
            string vShortDate = DateTime.Now.ToString("ddMMyyyy");
            dt = oRepo.GetDataForCibilEnq(Convert.ToInt64(pLeadId), pApplicanType);
            if (dt.Rows.Count > 0)
            {
                Cust_Ref_Id = Convert.ToString(dt.Rows[0]["CustRefId"]);
                var tuefHeaderReq = new TuefHeader()
                {
                    headerType = "TUEF",
                    version = "12",
                    memberRefNo = MemberRefId,
                    gstStateCode = "01",
                    enquiryMemberUserId = EnqMemUsrId,
                    enquiryPassword = EnqMemUPwd,
                    enquiryPurpose = dt.Rows[0]["CIBILEnquiryPurpose"].ToString(),
                    enquiryAmount = dt.Rows[0]["CIBILEnquiryAmount"].ToString(),   ///dt.Rows[0]["CIBILEnquiryAmount"].ToString(),
                    scoreType = "08",
                    outputFormat = "01",
                    responseSize = "1",
                    ioMedia = "CC",
                    authenticationMethod = "L"
                };

                List<Name> namesReq = new List<Name>();
                namesReq.Add(new Name
                {
                    index = "N01",
                    firstName = Convert.ToString(dt.Rows[0]["FirstName"]),
                    middleName = Convert.ToString(dt.Rows[0]["MiddleName"]),
                    lastName = Convert.ToString(dt.Rows[0]["LastName"]),
                    birthDate = Convert.ToString(dt.Rows[0]["DOB"]),
                    gender = Convert.ToString(dt.Rows[0]["GenderId"])
                });

                List<Telephone> telephoneReq = new List<Telephone>();
                telephoneReq.Add(new Telephone
                {
                    index = "T01",
                    telephoneNumber = Convert.ToString(dt.Rows[0]["ContactNo"]),
                    telephoneType = "01"
                });

                List<Id> idReq = new List<Id>();
                idReq.Add(new Id
                {
                    index = "I01",
                    idNumber = Convert.ToString(dt.Rows[0]["IdNo"]),
                    idType = Convert.ToString(dt.Rows[0]["IdProofCibilCode"])
                });

                List<CIBILAddress> addressesReq = new List<CIBILAddress>();
                addressesReq.Add(new CIBILAddress
                {
                    index = "A01",
                    line1 = Convert.ToString(dt.Rows[0]["Addressline1"]),
                    line2 = Convert.ToString(dt.Rows[0]["Addressline2"]),
                    line3 = Convert.ToString(dt.Rows[0]["Addressline3"]),
                    stateCode = Convert.ToString(dt.Rows[0]["CibilStateCode"]),
                    pinCode = Convert.ToString(dt.Rows[0]["PinCode"]),
                    addressCategory = "01",
                    residenceCode = "01"
                });
                List<EnquiryAccount> enquiryAccountsReq = new List<EnquiryAccount>();
                enquiryAccountsReq.Add(new EnquiryAccount
                {
                    index = "I01",
                    accountNumber = ""
                });

                var consumerInputSubjectReq = new ConsumerInputSubject()
                {
                    tuefHeader = tuefHeaderReq,
                    names = namesReq,
                    telephones = telephoneReq,
                    ids = idReq,
                    addresses = addressesReq,
                    enquiryAccounts = enquiryAccountsReq
                };

                var req = new Root()
                {
                    serviceCode = vServiceCode,
                    monitoringDate = vShortDate,
                    consumerInputSubject = consumerInputSubjectReq
                };
                Requestdata = JsonConvert.SerializeObject(req);

                string certificateFilePath = HostingEnvironment.MapPath("~/Certificate/UnityBank.pfx");
                string certificatePassword = "Unity#123$";
                X509Certificate2 certificate = new X509Certificate2(certificateFilePath, certificatePassword, X509KeyStorageFlags.DefaultKeySet);
                HttpWebRequest request = WebRequest.Create(vCibilApiUrl) as HttpWebRequest;
                request.ClientCertificates.Add(certificate);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("apikey", ApiKey);
                request.Headers.Add("member-ref-id", MemberRefId);
                request.Headers.Add("cust-bu-no", "GST0001");
                request.Headers.Add("sub-bu-code", "A01");
                request.Headers.Add("cust-ref-id", Cust_Ref_Id);

                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                try
                {
                    StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                    fullResponse = responseReader.ReadToEnd();

                    request.GetResponse().Close();
                    if (fullResponse == "")
                    {
                        fullResponse = "{\"controlData\":\"No Response\"}";
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        fullResponse = reader.ReadToEnd();
                    }
                }
            }
            else
            {
                fullResponse = "{\"controlData\":\"No Data Found\"}";
            }
            return fullResponse;
        }
        #endregion

        #region VkycFinalApproval
        public string FinalApproval(AgentResponse vAgentRes)
        {
            
            string vAuditorStatus = "", vJsonData = "", vSessionId = "",
            vUserId = "", vAgentStatus = "", vAuditorResult = ""; int vErr = 0;

            //SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-1");
            try
            {
                 vJsonData = JsonConvert.SerializeObject(vAgentRes);
                // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-2");
                 if (vAgentRes != null)
                 {
                     if (vAgentRes.session_id != null)
                     {
                         //SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-3");
                         vSessionId = vAgentRes.session_id;
                         //SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-4");
                     }
                    // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-5");
                     if (vAgentRes.user_id != null)
                     {
                       //  SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-6");
                         vUserId = vAgentRes.user_id;
                        // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-7");
                     }
                   //  SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-8");
                     if (vAgentRes.session_status != null)
                     {
                       //  SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-9");
                         vAgentStatus = vAgentRes.session_status;
                        // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-10");
                     }
                     // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-11");
                     if (vAgentRes.audit_result != null)
                     {
                        // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-12");
                         vAuditorStatus = vAgentRes.audit_result;
                        // SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-13");
                     }
                 }
              
                switch (vAuditorStatus)
                {
                    case "1":
                        vAuditorResult = "Approved";
                        break;
                    case "1.0":
                        vAuditorResult = "Approved";
                        break;
                    case "Approved":
                        vAuditorResult = "Approved";
                        break;
                    case "0":
                        vAuditorResult = "Rejected";
                        break;
                    case "Rejected":
                        vAuditorResult = "Rejected";
                        break;
                }

               vErr = SavetoDB(vJsonData, vUserId, vSessionId, vAgentStatus.Trim(), vAuditorResult.Trim(), "Stage-Final");
               if (vErr == 1)
               {
                   return "Response Receivecd Successfully...";
               }
               else
               {
                   return "Invalid Response...";
               }
               
            }
            catch (Exception ex)
            {
                
                SavetoDB("","" ,"","", ex.Message,"Stage-catch");
                return "Error during Receiving the Response";
            }
        }

        private Int32 SavetoDB(String vJsonData, String vUserId, String vSessionId, string vAgentStatus, string vAuditorStatus, string Stage)
        {
            int pErr = 0;
            CRepository oRepo = null;
            DataTable dt = new DataTable();
            oRepo = new CRepository();

            pErr = oRepo.SaveVkycFinalLog(vJsonData, vUserId, vSessionId, vAgentStatus, vAuditorStatus, Stage);
            return pErr;
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
       
    }
}
