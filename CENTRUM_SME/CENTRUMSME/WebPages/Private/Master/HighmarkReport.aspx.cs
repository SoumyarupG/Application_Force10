using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class HighmarkReport : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ei"] != null)
            {
                SetParameterForRptData(Request.QueryString["ei"].ToString(), Request.QueryString["em"].ToString());
            }
            else
            {
                gblFuction.MsgPopup("Enquiry Id Not Found. Unable To Extract HighMark Report");
            }
        }

        private void SetParameterForRptData(string enquiryid, string enquirymode)
        {
            int flag = 0;
            CMember ck = new CMember();

            try
            {
                string xml = ck.rptHighMark_CreditReport(enquiryid, enquirymode);

                string status = xml.Substring(0, xml.IndexOf(':'));

                if (status == "SUCCESS")
                {
                    #region CREDENTIAL PART

                    //PRODUCTION CREDENTIALS
                    string userId = "cpu_prod@jarofinance.com";
                    string password = "2E5EBCBC0758EE9D45A4262BD0D9DF58F0C61ED6";
                    string mbrid = "NBF0001293";
                    //string productType = "INDV";
                    //string productVersion = "1.0";
                    //string reqVolType = "INDV";
                    string SUB_MBR_ID = "JARO FINCAP PRIVATE LIMITED";
                    #endregion

                    #region DATA RETRIEVAL PART

                    xml = xml.Substring(xml.IndexOf(':') + 1);
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xml);

                    XmlNodeList elemList = xd.GetElementsByTagName("INQUIRY-UNIQUE-REF-NO");
                    string INQUIRY_UNIQUE_REF_NO = elemList[0].InnerText;

                    elemList = xd.GetElementsByTagName("REPORT-ID");
                    string REPORT_ID = elemList[0].InnerText;

                    elemList = xd.GetElementsByTagName("RESPONSE-DT-TM");
                    string RESPONSE_DT_TM = elemList[0].InnerText;

                    elemList = xd.GetElementsByTagName("RESPONSE-TYPE");
                    string RESPONSE_TYPE = elemList[0].InnerText;

                    #endregion

                    #region DATA MAKE PART

                    string REQUEST_REQUEST_FILE = "<REQUEST-REQUEST-FILE>"
                                                + "<HEADER-SEGMENT>"
                                                + "<PRODUCT-TYP>BASE_PLUS_REPORT</PRODUCT-TYP>"
                                                + "<PRODUCT-VER>2.0</PRODUCT-VER>"
                                                + "<REQ-MBR>" + mbrid + "</REQ-MBR>"
                                                + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                                + "<INQ-DT-TM>" + RESPONSE_DT_TM + "</INQ-DT-TM>"
                                                + "<REQ-VOL-TYP>INDV</REQ-VOL-TYP>"
                                                + "<REQ-ACTN-TYP>ISSUE</REQ-ACTN-TYP>"
                                                + "<TEST-FLG>N</TEST-FLG>"
                                                + "<USER-ID>" + userId + "</USER-ID>"
                                                + "<PWD>" + password + "</PWD>"
                                                + "<AUTH-FLG>Y</AUTH-FLG>"
                                                + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                                + "<RES-FRMT>XML/HTML</RES-FRMT>"
                                                + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                                                + "<RES-FRMT-EMBD>Y</RES-FRMT-EMBD>"
                                                + "</HEADER-SEGMENT>"
                                                + "<INQUIRY>"
                                                + "<INQUIRY-UNIQUE-REF-NO>" + INQUIRY_UNIQUE_REF_NO + "</INQUIRY-UNIQUE-REF-NO>"
                                                + "<REQUEST-DT-TM>" + RESPONSE_DT_TM + "</REQUEST-DT-TM>"
                                                + "<REPORT-ID>" + REPORT_ID + "</REPORT-ID>"
                                                + "</INQUIRY>"
                                                + "</REQUEST-REQUEST-FILE>";

                    #endregion

                    #region REQUEST RESPONSE PART
                    /*UAT URL*/
                    //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse");
                    /*********/

                    /*PRODUCTION URL*/
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/Inquiry/CPUAction.action");
                    /****************/

                    httpWebRequest.ContentType = "application/xml; charset=utf-8";
                    //httpWebRequest.Accept = "application/xml";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.PreAuthenticate = true;
                    //httpWebRequest.Timeout = 1000; //1 minute time out

                    httpWebRequest.Headers.Add("inquiryXML", REQUEST_REQUEST_FILE);
                    httpWebRequest.Headers.Add("userId", userId);
                    httpWebRequest.Headers.Add("password", password);

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    string responsedata = string.Empty;

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        var highmarkresult = streamReader.ReadToEnd();
                        responsedata = highmarkresult.ToString().Trim();
                    }
                    #endregion

                    xd = new XmlDocument();
                    xd.LoadXml(responsedata);

                    /*CHECK WHETHER THERE IS ERROR OR NOT*/
                    elemList = xd.GetElementsByTagName("ERROR");

                    if (elemList.Count > 0)
                    {
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            if (elemList[i].InnerText != "")
                            {
                                string error = "ERROR FROM RESPONSE:" + elemList[i].ChildNodes[1].InnerText;
                                gblFuction.MsgPopup(error);
                                flag = 1;
                                break;
                            }
                        }
                    }
                    /*************************************/
                    if (flag == 0)
                    {
                        elemList = xd.GetElementsByTagName("PRINTABLE-REPORT");

                        Response.Write(elemList[0].ChildNodes[2].InnerXml.Replace("]]>", ""));

                        //for (int i = 0; i < elemList.Count; i++)
                        //{
                        //    Response.Write(elemList[i].ChildNodes[2].InnerXml.Replace("]]>", ""));
                        //}
                    }
                }
                else
                {
                    gblFuction.MsgPopup(xml);
                }
            }
            catch (Exception ex)
            {
                gblFuction.MsgPopup("INTERNAL ERROR FROM PAGE:" + ex.Message);
            }
        }
    }
}