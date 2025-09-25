using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Configuration;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web.Hosting;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUM_SARALVYAPAR
{
    public partial class IDFYDigitalSign : CENTRUMBAse
    {
        string Account_ID = ConfigurationManager.AppSettings["Account_ID"];
        string API_key = ConfigurationManager.AppSettings["API_key"];
        string user_key = ConfigurationManager.AppSettings["user_key"];
        string esign_profile_id = ConfigurationManager.AppSettings["esign_profile_id"];

        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string AadhaarURL = ConfigurationManager.AppSettings["AadhaarURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["GroupId"] = null;
                ViewState["Language"] = null;
                ViewState["DateFrom"] = null;
                ViewState["DateTo"] = null;
                ViewState["MobNo"] = null;
                ViewState["LoanAppId"] = null;
                ViewState["MobNo"] = null;
                String encURL = "";
                String _vDocStatus = "N";
                CDigiDoc oDD = null;
                try
                {
                    encURL = Request.RawUrl;
                    encURL = encURL.Substring(encURL.IndexOf('?') + 1);
                    if (!encURL.Equals(""))
                    {
                        if (encURL.Contains("&"))
                        {
                            String[] queryStrParam1 = encURL.Split('&');
                            String[] queryStr1 = queryStrParam1[0].Split('=');
                            String[] queryStr2 = queryStrParam1[1].Split('=');
                            String[] queryStr3 = queryStrParam1[2].Split('=');
                            String[] queryStr4 = queryStrParam1[3].Split('=');
                            String[] queryStr5 = queryStrParam1[4].Split('=');
                            String[] queryStr6 = queryStrParam1[5].Split('=');
                            if (queryStr1.Length > 1)
                                ViewState["LoanAppId"] = queryStr1[1].ToString();
                            if (queryStr3.Length > 1)
                                ViewState["GroupId"] = queryStr3[1].ToString();
                            if (queryStr4.Length > 1)
                                ViewState["Language"] = queryStr4[1].ToString();
                            if (queryStr5.Length > 1)
                                ViewState["DateFrom"] = queryStr5[1].ToString();
                            if (queryStr6.Length > 1)
                                ViewState["DateTo"] = queryStr6[1].ToString();

                            oDD = new CDigiDoc();
                            _vDocStatus = oDD.DigitalSignStatus(Convert.ToString(ViewState["LoanAppId"]));
                            if (_vDocStatus.Trim().ToLower() == "y")
                            {
                                btnSignInit.Visible = false;
                                gblFuction.AjxMsgPopup("Digital Document already signed..");
                                return;
                            }
                            else
                            {
                                btnSignInit.Visible = true;
                            }
                        }
                        else
                        {
                            btnSignInit.Visible = false;
                            gblFuction.AjxMsgPopup("Page not contain parameter");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
                finally
                {

                }
            }
        }

        public void IDFYDigitalSignature(string pName, string pMobileNo, string pEmailId, string pFileName, string pBase64File, string pLoanAppId)
        {
            CDigiDoc oUsr = null;
            string pDigiSignReqData = "";
            //--------------------------------------
            //--------------------------------------
            List<EsignInvitee> row = new List<EsignInvitee>();
            row.Add(new EsignInvitee(pMobileNo, pName, pEmailId));
            //--------------------------------------
            EsignFileDetails objFileDtl = new EsignFileDetails();
            objFileDtl.file_name = pFileName;//File Name
            objFileDtl.esign_profile_id = esign_profile_id;//Profileid
            objFileDtl.esign_file = pBase64File;//Base 64 File
            //--------------------------------------
            EsignStampDetails objStampDtl = new EsignStampDetails();
            //--------------------------------------
            Data objData = new Data();
            objData.verify_aadhaar_details = false;
            objData.user_key = user_key;//user key
            objData.flow_type = "PDF";
            objData.esign_file_details = objFileDtl;
            objData.esign_stamp_details = objStampDtl;
            objData.esign_invitees = row;
            //--------------------------------------
            Guid id = Guid.NewGuid();
            string vTaskID = Convert.ToString(id);
            DigiSignReq req = new DigiSignReq();
            req.task_id = vTaskID;
            req.group_id = vTaskID;
            req.data = objData;
            //--------------------------------------   
            oUsr = new CDigiDoc();
            oUsr.SaveIdfyDigiDocLog(pLoanAppId, vTaskID);
            pDigiSignReqData = JsonConvert.SerializeObject(req);
            //string folderPath = HostingEnvironment.MapPath("~/Files");
            //System.IO.Directory.CreateDirectory(folderPath);
            //File.WriteAllText(folderPath + "/" + pLoanAppId + "_Request.txt", pDigiSignReqData);

            string resData = "";
            string url = "https://eve.idfy.com/v3/tasks/sync/generate/esign_document";  // URL to send the request to              
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Account-ID", Account_ID);
            request.Headers.Add("API-key", API_key);
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(pDigiSignReqData);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                resData = responseReader.ReadToEnd();
                request.GetResponse().Close();
                //File.WriteAllText(folderPath + "/" + pLoanAppId + "_Response.txt", resData);
                //resData = "{\"action\":\"generate\",\"completed_at\":\"2024-12-13T17:27:27+05:30\",\"created_at\":\"2024-12-13T17:27:26+05:30\",\"group_id\":\"523764d9-e142-4844-a1a4-cc63230a19b1\",\"request_id\":\"4539489d-0f7d-4f8c-a3b2-967912049f9d\",\"result\":{\"source_output\":{\"esign_details\":[{\"esign_expiry\":\"2024-12-23T18:29:59Z\",\"esign_url\":\"https://app1.leegality.com/sign/a952d83f-c941-4eba-8674-ec844dd41c4f\",\"esigner_email\":\"tarun.bhojwani@unitybank.co.in\",\"esigner_name\":\"GAUTAM GANPAT RANDIVE\",\"esigner_phone\":\"8698588972\",\"url_status\":true},{\"esign_expiry\":null,\"esign_url\":null,\"esigner_email\":null,\"esigner_name\":null,\"esigner_phone\":null,\"url_status\":false}],\"esign_doc_id\":\"ko6tKf3\",\"esigner_details\":null,\"status\":\"Success\"}},\"status\":\"completed\",\"task_id\":\"523764d9-e142-4844-a1a4-cc63230a19b1\",\"type\":\"esign_document\"}";
                dynamic objResponse = JsonConvert.DeserializeObject(resData);
                string vRequestId = objResponse.request_id;
                string vEsignDocId = objResponse.result.source_output.esign_doc_id;
                string vUrl = objResponse.result.source_output.esign_details[0].esign_url;
                try
                {
                    oUsr = new CDigiDoc();
                    oUsr.UpdateIdfyDigiDocLog(vTaskID, vRequestId, resData, "I", vEsignDocId);
                }
                catch { }
                Response.Redirect(vUrl);
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    resData = reader.ReadToEnd();
                    gblFuction.AjxMsgPopup(resData);
                    //oCR = new CReports();
                    //oCR.UpdateIdfyDigiDocLog(vTaskID, vRequestId, resData, "I", vEsignDocId);
                    // File.WriteAllText(folderPath + "/" + pLoanAppId + "_ErrResponse.txt", resData);
                }
            }
        }

        protected void btnSignInit_Click(object sender, EventArgs e)
        {
            //GetReportDocForDigitalSign();           
            CDigiDoc oUsr = null;
            DataTable dtRecord = null;
            DataSet ds = null;
            string vMemberName = "", vMobileNo = "";
            string vLoanAppId = Convert.ToString(ViewState["LoanAppId"]);
            string vFileName = vLoanAppId + ".pdf";
            string vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
            string PdfFilePath = vpathCreateNewDoc + vLoanAppId + ".pdf";
            try
            {
                oUsr = new CDigiDoc();
                ds = oUsr.getDigitalDocByToken(vLoanAppId, "", "A");
                dtRecord = ds.Tables[0];
                vMemberName = dtRecord.Rows[0]["CustomerName"].ToString();
                vMobileNo = dtRecord.Rows[0]["MobileNo"].ToString();
                if (File.Exists(PdfFilePath))
                {
                    try
                    {
                        // byte[] pdfByte = File.ReadAllBytes(PdfFilePath);
                        byte[] pdfByte = null;
                        using (FileStream fs = new FileStream(PdfFilePath, FileMode.Open, FileAccess.Read))
                        {
                            pdfByte = new byte[fs.Length]; fs.Read(pdfByte, 0, pdfByte.Length);
                        }
                        string vBase64Pdf = Convert.ToBase64String(pdfByte);
                        IDFYDigitalSignature(vMemberName, vMobileNo, "idfydigitalsign@unitybank.co.in", vFileName, vBase64Pdf, vLoanAppId);//IDFY Digital Sign
                    }
                    catch (Exception ex)
                    {
                        gblFuction.AjxMsgPopup(ex.Message.ToString());
                        return;
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("File not found.");
                    return;
                }
            }
            catch (Exception ex)
            {
                gblFuction.AjxMsgPopup(ex.Message.ToString());
            }
            finally
            {
                oUsr = null;
            }
        }

        public string ConvertStreamToBase64(Stream stream)
        {
            // Convert Stream to byte array
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                byte[] byteArray = ms.ToArray();
                // Convert byte array to Base64 string
                return Convert.ToBase64String(byteArray);
            }
        }
    }
    #region RequestBody
    public class Data
    {
        public bool verify_aadhaar_details { get; set; }
        public string user_key { get; set; }
        public string flow_type { get; set; }
        public EsignStampDetails esign_stamp_details { get; set; }
        public List<EsignInvitee> esign_invitees { get; set; }
        public EsignFileDetails esign_file_details { get; set; }
    }

    public class EsignFileDetails
    {
        public string file_name { get; set; }
        public string esign_profile_id { get; set; }
        public string esign_file { get; set; }
    }

    public class EsignInvitee
    {
        public string esigner_phone { get; set; }
        public string esigner_name { get; set; }
        public string esigner_email { get; set; }

        public EsignInvitee(string esigner_phone, string esigner_name, string esigner_email)
        {
            this.esigner_phone = esigner_phone;
            this.esigner_name = esigner_name;
            this.esigner_email = esigner_email;
        }
    }

    public class EsignStampDetails
    {
        public object esign_stamp_value { get; set; }
        public object esign_stamp_series { get; set; }
        public object esign_series_group { get; set; }
    }

    public class DigiSignReq
    {
        public string task_id { get; set; }
        public string group_id { get; set; }
        public Data data { get; set; }
    }
    #endregion
}