using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Configuration;
using System.IO;

namespace SendSms
{
   
    public sealed class AuthSms
    {
        private string vSmsUrl = "http://api.msg91.com/api/sendhttp.php?";
        private string vUserName = "audut@kudosfinance.in";
        private string vPassword = "kudos@2018";
        private string vAuthKey = "234062AmchPLaLUZgR5b84e49e";
        /// </summary>
        /// <param name="pMobileNo"></param>
        /// <param name="pMessage"></param>
        /// <returns></returns>
        public string SendSms(string pMobileNo, string pMessage)
        {                                
            string vRetGuid = string.Empty;
            try
            {
                // http://trans.masssms.tk/api.php?username=CENTRUMSME&password=717544&sender=CENTRUMSME&sendto=918308097840&message=hello
                string vData = "http://trans.masssms.tk/api.php?username=CENTRUMSME&password=717544&sender=CENTRUMSME&sendto=91" + pMobileNo + "&message=" + pMessage;
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(vData);
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                //  vRetGuid = respStreamReader.ReadToEnd().Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\n<title>Untitled Document</title>\n</head>\n\n<body>\n</body>\n</html>\n\n\n\n\n\n\n\n\n", "").Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\n<title>universal SMS API</title>\n</head>\n\n<body>\n</body>\n</html>\n", "");
                vRetGuid = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
                return vRetGuid;                    
            }
            catch (ApplicationException ex)
            {
                return vRetGuid="";            
            }
        }

        public string SendCIBIL(string pXML)
        {
            string vRetGuid = string.Empty;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072; // For Security Purpose on Live Server...
                //string vData = "https://cibilservice.bijliftt.com/CibilService.asmx/CibilProduction?pxml=" + pXML;
                //string vData = "http://208.50.229.54:3011/CibilService.asmx/CibilProduction?pxml=" + pXML;//For Service and Programe dll in different server..
                string vData = "http://localhost:3011/CibilService.asmx/CibilProduction?pxml=" + pXML;  // For Service and Programe dll in same server..
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(vData);
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                vRetGuid = respStreamReader.ReadToEnd();
                respStreamReader.Close();
                myResp.Close();
                return vRetGuid;
            }
            catch (ApplicationException ex)
            {
                return vRetGuid = "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vGuId"></param>
        /// <returns></returns>
        public string GetGuidStatus(ref string vGuId, ref string pMsgStat, ref string pReason)
        {
            string vData = string.Empty;
            string vRtnVal = string.Empty;
            try
            {               
                if (vGuId != null || vGuId != "")
                {
                    //"http://api.msg91.com/api/sendhttp.php?country=91&sender=KudosF&route=4&mobiles=" + pMobileNo + "&authkey=234062AmchPLaLUZgR5b84e49e&message=" + pMessage;  
                    vData = vSmsUrl + "country=91&sender=KudosF&route=4&authkey=234062AmchPLaLUZgR5b84e49e&msgguid=" + vGuId;
                    HttpWebRequest vReq = (HttpWebRequest)WebRequest.Create(vData);
                    HttpWebResponse vRsp = (HttpWebResponse)vReq.GetResponse();
                    StreamReader vStrRead = new StreamReader(vRsp.GetResponseStream());
                    //vRtnVal = vStrRead.ReadToEnd().Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\n<title>Untitled Document</title>\n</head>\n\n<body>\n</body>\n</html>\n\n\n\n\n\n\n\n\n", "").Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />\n<title>universal SMS API</title>\n</head>\n\n<body>\n</body>\n</html>\n", "");
                    vRtnVal = vStrRead.ReadToEnd();
                    vGuId = vRtnVal;
                    vStrRead.Close();
                    vRsp.Close();
                    //GetSmsStatus(vRtnVal, ref pMsgStat, ref pReason);
                    GetSmsStatus(vRtnVal, ref pReason, ref pMsgStat);
                   
                }
                return vRtnVal;
            }
            catch (ApplicationException ex)
            {
                return vRtnVal = "";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRetGuid"></param>
        /// <param name="pStatus"></param>
        /// <returns></returns>
        private void GetSmsStatus(string pRetGuid, ref string pStatus, ref string vReson)
        {
            switch (pRetGuid)
            {
                case "2019":
                    vReson = "Invalid user name or password";
                    pStatus = "Invalid user name or password.";
                    break;
                case "2020":
                    vReson = "Message delivered successfully";
                    pStatus = "Message Delivered.";
                    break;
                case "2021":
                    vReson = "Message delivered successfully";
                    pStatus = "Message Delivered.";
                    break;
                case "2022":
                    vReson = "Mobile number is blank";
                    pStatus = "Mobile number is blank.";
                    break;
                case "2023":
                    vReson = "Low balance or credit";
                    pStatus = "Low balance or credit.";
                    break;
                case "8448":
                    vReson = "Message delivered successfully";
                    pStatus = "Message Delivered.";
                    break;
                default:
                    vReson = "Unknown Error";
                    pStatus = "Unknown Error.";
                    break;
            }
        }
    }
}
