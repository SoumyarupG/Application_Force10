using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using System.Web;

namespace FORCECA
{
    public class CApiCalling
    {
        public string GenerateReport(string pApiName, string pRequestdata, string pReportUrl)
        {
            try
            {
                string Requestdata = pRequestdata;
                string postURL = pReportUrl + "/" + pApiName;
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 10000;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                byte[] data = Encoding.UTF8.GetBytes(Requestdata);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
            }
            finally
            {
            }
            return "After few minutes you can download your report from report dashboard.";
        }

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
            // Create request and receive response           
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
        
    }   
}
