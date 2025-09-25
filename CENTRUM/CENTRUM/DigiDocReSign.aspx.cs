using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Newtonsoft.Json;
using System.Text;

namespace CENTRUM
{
    public partial class DigiDocReSign : System.Web.UI.Page
    {
        string vpathCreateNewDoc = "";
        #region Path Details
        string path = ConfigurationManager.AppSettings["PathInitialApproach"];
        string IniPathHDrive = ConfigurationManager.AppSettings["IniPathHDrive"];
        string IniPathNetwork = ConfigurationManager.AppSettings["IniPathNetwork"];
        string IniPathGDrive = ConfigurationManager.AppSettings["IniPathGDrive"];
        string PathMinio = ConfigurationManager.AppSettings["PathMinio"];
        string InitialApproachURL = ConfigurationManager.AppSettings["InitialApproachURL"];
        string DigiDocOTPURL = ConfigurationManager.AppSettings["DigiDocOTPURL"];

        string DigiDocBucket = ConfigurationManager.AppSettings["DigiDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        string vPathImgDownload = ConfigurationManager.AppSettings["PathImgDownload"];
        string PathOTP = ConfigurationManager.AppSettings["PathOTP"];

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CDigiDoc oUsr = new CDigiDoc();
            DataTable dt = new DataTable();
            DataTable dt1 = null;

            dt = oUsr.GetDigiDocReSign();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt1 = new DataTable();
                oUsr = new CDigiDoc();
                dt1 = oUsr.GetDigiDocDtlById(Convert.ToInt64(dt.Rows[i]["DigiDocId"]));
                if (dt1.Rows.Count > 0)
                {
                    ViewState["Language"] = dt1.Rows[0]["Langu"].ToString();
                    ViewState["GroupId"] = dt1.Rows[0]["GroupId"].ToString();
                    ViewState["DateFrom"] = dt1.Rows[0]["AppDate"].ToString();
                    ViewState["DateTo"] = dt1.Rows[0]["AppDate"].ToString();
                    ViewState["LoanAppId"] = dt1.Rows[0]["LoanAppId"].ToString();
                    GetReportDocForDigitalSign(dt.Rows[i]["LoanAppId"].ToString(), Convert.ToInt64(dt.Rows[i]["DigiDocDtlsId"]));
                }
            }
        }

        #region Craete Report Doc

        private string GetReportDocForDigitalSign(string pLoanAppNo, Int64 vDigiDocDtlsId)
        {
            CReports oRpt = null;
            DataSet ds1 = new DataSet(), dsDigiDoc = new DataSet();
            DataTable dt1 = new DataTable(), dt2 = new DataTable(), dt3 = new DataTable();
            DataTable dt4 = new DataTable(), dt5 = new DataTable(), dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dtDigiDocDtls = new DataTable();

            string vRptPath = "", vImgPath = "", vLanguage = "E", vGroupId = "", vFileName = "No File Created", vDtFrom = "", vDtTo = "";
            CDigiDoc oUsr = null;
            vLanguage = Convert.ToString(ViewState["Language"]);
            vGroupId = Convert.ToString(ViewState["GroupId"]);
            vDtFrom = Convert.ToString(ViewState["DateFrom"]);
            vDtTo = Convert.ToString(ViewState["DateTo"]);
            try
            {
                vRptPath = string.Empty;
                if (vLanguage == "E") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocForm.rpt";
                if (vLanguage == "H") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormHindi.rpt";
                if (vLanguage == "G") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormGujarati.rpt";
                if (vLanguage == "O") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocFormOdia.rpt";
                if (vLanguage == "B") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintBengali.rpt";
                if (vLanguage == "K") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintKannad.rpt";
                if (vLanguage == "T") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTamil.rpt";
                if (vLanguage == "M") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintMalayalam.rpt";
                if (vLanguage == "L") vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocPrintTelugu.rpt";

                oRpt = new CReports();
                string Id = pLoanAppNo;
                ds1 = oRpt.GetDigitalDocForm(Id, gblFuction.setDate(vDtFrom), gblFuction.setDate(vDtTo), vGroupId, Convert.ToInt64(vDigiDocDtlsId), 1);
                dt1 = ds1.Tables[0];
                dt2 = ds1.Tables[1];
                dt3 = ds1.Tables[2];
                dt4 = ds1.Tables[3];
                dt5 = ds1.Tables[4];
                dt6 = ds1.Tables[5];
                dt7 = ds1.Tables[6];

                oUsr = new CDigiDoc();
                dsDigiDoc = oUsr.getDigiDocDtlsByDocId(vDigiDocDtlsId, "", "Y");
                dtDigiDocDtls = dsDigiDoc.Tables[0];
                if (dtDigiDocDtls.Rows.Count > 0)
                {
                    //-----------------------Download Images-----------------------                           
                    //string pRequestdata = "{\"pId\":\"" + Id + "\"}";
                    //CallAPI("GetDigiDocOTPImg", pRequestdata, "https://unityimage.bijliftt.com/ImageDownloadService.svc");
                    ////-------------------------------------------------------------
                    //dtDigiDocDtls.Rows[0]["PhotoImage"] = PathOTP + Id + ".jpg";

                    dtDigiDocDtls.Rows[0]["PhotoImageByte"] = GetByteImage(Id + ".jpg", Id, "O");
                    dtDigiDocDtls.AcceptChanges();
                }
                if (dt1.Rows.Count > 0)
                {
                    if (vRptPath != string.Empty)
                    {
                        //-----------------------Download Images-----------------------
                        string vEnqId = dt7.Rows[0]["EnquiryId"].ToString();
                        string pRequestdata = "{\"pEnquiryId\":\"" + vEnqId + "\"}";
                        //CallAPI("GetImage", pRequestdata, vPathImgDownload);
                        //-------------------------------------------------------------
                        string vMemberPhoto = "";
                        foreach (DataRow dr in dt1.Rows)
                        {
                            //string vMemPhoto = Convert.ToString(dr["ImgPath"]);
                            //vMemPhoto = vMemPhoto.Substring(1, vMemPhoto.Length - 1).Replace('\\', '_');
                            //vMemberPhoto = PathMinio + vMemPhoto;
                            //dr["ImgPath"] = vMemberPhoto;

                            dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                        }
                        dt1.AcceptChanges();
                        if (dt7.Rows.Count > 0)
                        {
                            vImgPath = PathMinio;
                            foreach (DataRow dr in dt7.Rows)
                            {
                                //dr["KYC1"] = vImgPath + (Convert.ToString(dr["KYC1"]).Substring(1, Convert.ToString(dr["KYC1"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC1Back"] = vImgPath + (Convert.ToString(dr["KYC1Back"]).Substring(1, Convert.ToString(dr["KYC1Back"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC2"] = vImgPath + (Convert.ToString(dr["KYC2"]).Substring(1, Convert.ToString(dr["KYC2"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC2Back"] = vImgPath + (Convert.ToString(dr["KYC2Back"]).Substring(1, Convert.ToString(dr["KYC2Back"]).Length - 1).Replace('\\', '_'));
                                //dr["KYC3"] = vImgPath + (Convert.ToString(dr["KYC3"]).Substring(1, Convert.ToString(dr["KYC3"]).Length - 1).Replace('\\', '_')); ;
                                //dr["KYC3Back"] = vImgPath + (Convert.ToString(dr["KYC3Back"]).Substring(1, Convert.ToString(dr["KYC3Back"]).Length - 1).Replace('\\', '_'));

                                dr["KYC1Byte"] = GetByteImage("IDProofImage.png", vEnqId, "I");
                                dr["KYC1BackByte"] = GetByteImage("IDProofImageBack.png", vEnqId, "I");
                                dr["KYC2Byte"] = GetByteImage("AddressProofImage.png", vEnqId, "I");
                                dr["KYC2BackByte"] = GetByteImage("AddressProofImageBack.png", vEnqId, "I");
                                dr["KYC3Byte"] = GetByteImage("AddressProofImage2.png", vEnqId, "I");
                                dr["KYC3BackByte"] = GetByteImage("AddressProofImage2Back.png", vEnqId, "I");

                            }
                        }
                        dt7.AcceptChanges();

                        if (dt3.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt3.Rows)
                            {
                                // dr["ImgPath"] = vMemberPhoto;
                                dr["ImgByte"] = GetByteImage("MemberPhoto.png", vEnqId, "I");
                            }
                            dt3.AcceptChanges();
                        }

                        vpathCreateNewDoc = ConfigurationManager.AppSettings["pathCreateNewDoc"];
                        vFileName = vpathCreateNewDoc + pLoanAppNo + ".pdf";

                        using (ReportDocument rptDoc = new ReportDocument())
                        {
                            rptDoc.Load(vRptPath);
                            rptDoc.SetDataSource(dt1);

                            if (vLanguage == "E")
                            {
                                rptDoc.Subreports["DigitalDocFormP2.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFrmP4.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "H")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Hindi.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Hindi.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Hindi.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Hindi.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Hindi.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Hindi.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "G")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Gujarati.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Gujarati.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Gujarati.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Gujarati.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Gujarati.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Gujarati.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "O")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Odia.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Odia.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Odia.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Odia.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Odia.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Odia.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "B")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "K")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Kannad.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Kannad.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Kannada.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "T")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Tamil.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Tamil.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Tamil.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "M")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Kannad"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Kannad"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Kannad"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Malayalam.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Malayalam.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Malayalam.rpt"].SetDataSource(dt7);
                            }
                            else if (vLanguage == "L")
                            {
                                rptDoc.Subreports["DigitalDocFormP2Bengali.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["DigitalDocFormP3Bengali.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["DigitalDocFormP4Bengali.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["DigitalDocFormP5Bengali.rpt"].SetDataSource(dt5);
                                rptDoc.Subreports["DigitalDocFormP6Bengali.rpt"].SetDataSource(dt6);
                                rptDoc.Subreports["DigitalDocFormP7Bengali.rpt"].SetDataSource(dt7);
                            }


                            if (dtDigiDocDtls.Rows.Count > 0)
                            {
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);
                            }
                            if (MinioYN == "Y")
                            {
                                Stream reportStream = rptDoc.ExportToStream(ExportFormatType.PortableDocFormat);
                                UploadFileMinio(strmToByte(reportStream), pLoanAppNo + ".pdf", pLoanAppNo, DigiDocBucket, MinioUrl);
                            }
                            else
                            {
                                rptDoc.ExportToDisk(ExportFormatType.PortableDocFormat, vFileName);
                            }
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
                oUsr = null;
                ds1 = null;
                dt1 = null; dt2 = null; dt3 = null;
                dt4 = null;
                dt5 = null; dt6 = null; dt7 = null;
            }
            return vFileName;
        }
        #endregion

        #region API Calling
        public string CallAPI(string pApiName, string pRequestdata, string pReportUrl)
        {
            try
            {
                string Requestdata = pRequestdata;
                string postURL = pReportUrl + "/" + pApiName;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responseReader.ReadToEnd();
                request.GetResponse().Close();
            }
            finally
            {
            }
            return "";
        }
        #endregion

        #region GetMultipartFormData
        private readonly Encoding encoding = Encoding.UTF8;

        private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;
            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                    boundary,
                    param.Key,
                    param.Value);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
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
        private bool URLExist(string pPath)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            WebRequest wR = WebRequest.Create(pPath);
            WebResponse webResponse;
            try
            {
                wR.Timeout = 3000;
                webResponse = wR.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
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

        public byte[] GetByteImage(string pImageName, string pId, string Module = "I")
        {
            string ActNetImage = "", ActNetImage1 = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = Module == "I" ? InitialApproachURL.Split(',') : DigiDocOTPURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + (Module == "I" ? pId + "_" + pImageName : pImageName);
                    ActNetImage1 = ActNetPath[j] + (Module == "I" ? pId + "/" + pImageName : pImageName);
                    if (URLExist(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        break;
                    }
                    else if (URLExist(ActNetImage1))
                    {
                        imgByte = DownloadByteImage(ActNetImage1);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return imgByte;
        }

    }
}