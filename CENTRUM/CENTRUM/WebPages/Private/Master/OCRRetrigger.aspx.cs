using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Text;
using System.Data;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class OCRRetrigger : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "OCR Retrigger";            
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuOCRLog);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd=="Y")
                {

                }
                else
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "OCR Retrigger", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnReTigger_Click(object sender, EventArgs e)
        {
            string vEnqId = txtEnqId.Text;
            byte[] ID1Front, ID1Back, ID2Front, ID2Back = null;
            string ID1FrontResponse = "", ID1BackResponse = "", ID2FrontResponse = "", ID2BackResponse = "", vID1VoterResponse = "";

            DataTable dt = new DataTable();
            CNewMember oNm = new CNewMember();
            dt = oNm.GetOCRRetriggerData(vEnqId);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["IdentyPRofId"].ToString() == "1")
                {
                    gblFuction.AjxMsgPopup("Aadhar Retrigger not available.");
                    return;
                }
                using (WebClient webclient = new WebClient())
                {
                    ID1Front = webclient.DownloadData("https://centrummob.bijliftt.com/Files/InitialApproach/" + vEnqId + "/IDProofImage.png");
                    ID1Back = webclient.DownloadData("https://centrummob.bijliftt.com/Files/InitialApproach/" + vEnqId + "/IDProofImageBack.png");
                    ID2Front = webclient.DownloadData("https://centrummob.bijliftt.com/Files/InitialApproach/" + vEnqId + "/AddressProofImage.png");
                    ID2Back = webclient.DownloadData("https://centrummob.bijliftt.com/Files/InitialApproach/" + vEnqId + "/AddressProofImageBack.png");
                }
                if ((ID1Front != null && ID1Front.Length > 0) && (ID1Back != null && ID1Back.Length > 0) &&
                    (ID2Front != null && ID2Front.Length > 0) && (ID2Back != null && ID2Back.Length > 0))
                {
                    string vIdData = dt.Rows[0]["MobileNo"].ToString() + '#' + dt.Rows[0]["BranchCode"].ToString() + '#' + dt.Rows[0]["EoId"].ToString();

                    ID1FrontResponse = CallMultiPartData(ID1Front, vIdData);
                    ID1BackResponse = CallMultiPartData(ID1Back, vIdData);
                    ID2FrontResponse = CallMultiPartData(ID2Front, vIdData);
                    ID2BackResponse = CallMultiPartData(ID2Back, vIdData);


                    //ID1FrontResponse = "{\'OCRPhototoDataResult\':[{\'ResponseString\':\'{\\\'requestId\\\':\\\'90c3cdd4-cda6-44ad-9873-454de2449200\\\',\\\'result\\\':[{\\\'details\\\':{\\\'voterid\\\':{\\\'value\\\':\\\'LVK1211572\\\'},\\\'name\\\':{\\\'value\\\':\\\'MANASI ANANT SODAYE\\\'},\\\'gender\\\':{\\\'value\\\':\\\'FEMALE\\\'},\\\'relation\\\':{\\\'value\\\':\\\'ANANT SODAYE\\\'},\\\'dob\\\':{\\\'value\\\':\\\'\\\'},\\\'doc\\\':{\\\'value\\\':\\\'1\\/1\\/2008\\\'},\\\'age\\\':{\\\'value\\\':\\\'21\\\'}},\\\'type\\\':\\\'Voterid Front\\\'}],\\\'statusCode\\\':101}\',\'Responsestatus\':\'Success\'}]}";
                    //ID2FrontResponse = "{\'OCRPhototoDataResult\':[{\'ResponseString\':'{\\\'requestId\\\':\\\'96d80d1c-934d-47b1-a442-eb6d73b03c1b\\\',\\\'result\\\':[{\\\'details\\\':{\\\'aadhaar\\\':{\\\'value\\\':\\\'811145518302\\\',\\\'isMasked\\\':\\\'no\\\'},\\\'dob\\\':{\\\'value\\\':\\\'15\\/09\\/1990\'},\\\'father\\\':{\\\'value\\\':\\\'\\\'},\\\'gender\\\':{\\\'value\\\':\\\'FEMALE\\\'},\\\'mother\\\':{\'value\\\':\\\'\\\'},\\\'name\\\':{\\\'value\\\':\\\'Mansi Anant Sodaye\\\'},\\\'yob\\\':{\\\'value\\\':\\\'\\\'},\\\'imageUrl\\\':{\\\'value\\\':\\\'\\\'},\\\'qr\\\':{\\\'value\\\':\\\'\\\'}},\\\'type\\\':\\\'Aadhaar Front Bottom\\\'}],\\\'statusCode\\\':101}',\'Responsestatus\':\'Success\'}]}";
                    //ID2BackResponse = "{\'OCRPhototoDataResult\':[{\'ResponseString\':'{\\\'requestId\\\':\\\'f62b8488-aa55-4b05-8685-d8ceeef3002f\\\',\\\'result\\\':[{\\\'details\\\':{\\\'aadhaar\\\':{\\\'value\\\':\\\'811145518302\\\',\\\'isMasked\\\':\\\'no\\\'},\\\'address\\\':{\\\'value\\\':\\\' W\\/O : Anant Sodaye, At Post Devache Gothane, Sodayewadi, Near Rautwadi, Taluka Rajapur, Dist Ratnagiri, Devache Gothana,f.t oiri Devache Gothane, Maharashtra, 416\\/v2 416702\\\'},\\\'pin\\\':{\\\'value\\\':\\\'416702\\\'},\\\'imageUrl\\\':{\\\'value\\\':\\\'\\\'},\\\'qr\\\':{\\\'value\\\':\\\'\\\'},\\\'father\\\':{\\\'value\\\':\\\'\\\'},\\\'husband\\\':{\\\'value\\\':\\\': Anant Sodaye\\\'},\\\'addressSplit\\\':{\'careOf\\\':\'W\\/O : Anant Sodaye\\\',\\\'houseNumber\\\':\\\'\\\',\\\'city\\\':\\\'\\\',\\\'line1\\\':\\\'\\\',\\\'line2\\\':\\\'At Post Devache Gothane Sodayewadi Taluka Rajapur Dist ratnagiri, Devache Gothana,f.t oiri Devache Gothane 416 \\/ v2\\\',\\\'street\\\':\\\'\\\',\\\'locality\\\':\\\'At Post Devache Gothane Sodayewadi Taluka Rajapur\\\',\\\'landmark\\\':\\\'Near Rautwadi\\\',\\\'district\\\':\\\'ratnagiri\\\',\\\'state\\\':\\\'maharashtra\\\',\\\'pin\\\':\\\'416702\\\'}},\\\'type\\\':\\\'Aadhaar Back\\\'}],\\\'statusCode\\\':101}',\'Responsestatus\':\'Success\'}]}";  

                    dynamic op = JsonConvert.DeserializeObject(ID1FrontResponse);
                  //  var data = JsonConvert.DeserializeObject(op.OCRPhototoDataResult[0].ResponseString.Value);
                   // var status = op.OCRPhototoDataResult[0].Responsestatus;

                    string IdNo = "", Name = "", ID1ResponseName = "", ID1ResponseId = "";
                    if (dt.Rows[0]["IdentyPRofId"].ToString() == "3")
                    {
                       // IdNo = data.result[0].details.voterid.value;
                        var req = new PostVoterData()
                        {
                            vPostVoterData = new vPostVoterData()
                            {
                                epic_no = IdNo,
                                consent = "Y"
                            }
                        };
                        vID1VoterResponse = CallApi(JsonConvert.SerializeObject(req), "https://centrummobtest.bijliftt.com/CentrumService.svc/KarzaVoterIDKYCValidation");

                        //vID1VoterResponse = "{'KarzaVoterIDKYCValidationResult':{'request_id':'ea5a6500-bdd6-42d3-aaeb-1b4533769a66','result':{'ac_name':'Rajapur','ac_no':'267','age':null,'district':'RATNAGIRI','dob':'','epic_no':'LVK1211572','gender':'F','house_no':'640','id':'S132670283010020','last_update':'11-10-2019','name':'Mansi Anant Sodathe','name_v1':'मानसी अनंत','name_v2':'','name_v3':'','part_name':'Davachegothane Rautwadi-Sodyewadi','part_no':'283','pc_name':'Ratnagiri-Sindhudurg','ps_lat_long':'0.0,0.0','ps_name':'Zilla Parishad Purn Prathamik Shala Daivache gothane (Sodyewadi)','rln_name':'Anant Sodathe','rln_name_v1':'अनंत','rln_name_v2':'','rln_name_v3':'','rln_type':'H','section_no':'1','slno_inpart':'20','st_code':'S13','state':'MAHARASHTRA'},'status_code':'101:Valid Authentication'}}";
                        dynamic op1 = JsonConvert.DeserializeObject(vID1VoterResponse);
                       // ID1ResponseName = op1.KarzaVoterIDKYCValidationResult.result.name;
                       // ID1ResponseId = op1.KarzaVoterIDKYCValidationResult.result.epic_no;
                    }
                    else
                    {
                        //IdNo = data.result[0].details.aadhaar.value;
                    }
                  //  Name = data.result[0].details.name.value;

                    var req1 = new NameMatch()
                    {
                        vNameMatch = new vNameMatch()
                        {
                            name1 = Name,
                            name2 = ID1ResponseName,
                            pEoID = dt.Rows[0]["EoId"].ToString(),
                            pBranch = dt.Rows[0]["BranchCode"].ToString(),
                            pIdNo = IdNo
                        }
                    };
                    string vNameMatchResponse = CallApi(JsonConvert.SerializeObject(req1), "https://centrummobtest.bijliftt.com/CentrumService.svc/NameMatching");
                    //string vNameMatchResponse = "{\'NameMatchingResult\':{'\result\':true,\'score\':82.25}}";
                    //var req2 = new AddressMatch()
                    //{
                    //    vAddressMatch = new vAddressMatch()
                    //    {
                    //        address1 = Name,
                    //        address2 = ID1ResponseName
                    //    }
                    //};
                    //string vAddressMatchResponse = CallApi(JsonConvert.SerializeObject(req1), "https://centrummobtest.bijliftt.com/CentrumService.svc/AddressMatching");


                    var req3 = new OCRData()
                    {
                        vOCRData = new vOCRData()
                        {
                            EnquiryId = vEnqId,
                            ID1AadharBack = "",
                            ID1AadharFront = "",
                            ID1VoterFront = ID1FrontResponse,
                            ID1VoterBack = ID1BackResponse,
                            ID1VoterResponse = vID1VoterResponse,
                            ID1AadharResponse = "",
                            ID2AadharFront = ID2FrontResponse,
                            ID2AadharBack = ID2BackResponse,
                            ID2AadharResponse = "",
                            ID2VoterBack = "",
                            ID2VoterFront = "",
                            ID2VoterResponse = "",
                            NameMatchingResponse = vNameMatchResponse,
                            AddressMatchingResponse = "",
                            FaceMatchingResponse = "",
                            NameMatchingResponseID2 = "",
                            FaceMatchingResponseID2 = "",
                            AddressMatchingResponseID2 = ""

                        }
                    };

                    string vOCRResponse = CallApi(JsonConvert.SerializeObject(req3), "https://centrummob.bijliftt.com/CentrumService.svc/SaveOCRData");
                    Console.Write(vOCRResponse);
                }
                else
                {

                }
            }
        }

        public string CallMultiPartData(byte[] vImg, string vIdData)
        {
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("IdData", vIdData);
            postParameters.Add("IMAGE", new FormUpload.FileParameter(vImg, "Id.png", "image/png"));

            string postURL = "https://centrummob.bijliftt.com/CentrumService.svc/OCRPhototoData";
            string userAgent = "Someone";
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            //webResponse.Close();
            return fullResponse;
        }

        public string CallApi(string Requestdata, string URL)
        {
            string postURL = URL;
            string responsedata = string.Empty;
            // string Requestdata = JsonConvert.SerializeObject(req);
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
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);


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
            }
            finally { }
            return responsedata;
        }
    }

    public class vPostVoterData
    {
        public string consent { get; set; }
        public string epic_no { get; set; }
    }
    public class PostVoterData
    {
        public vPostVoterData vPostVoterData { get; set; }
    }
    public class vNameMatch
    {
        public string name1 { get; set; }
        public string name2 { get; set; }

        public string pBranch { get; set; }
        public string pEoID { get; set; }
        public string pIdNo { get; set; }
    }
    public class NameMatch
    {
        public vNameMatch vNameMatch { get; set; }
    }

    public class vAddressMatch
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
    }
    public class AddressMatch
    {
        public vAddressMatch vAddressMatch { get; set; }
    }

    public class vOCRData
    {
        public string EnquiryId { get; set; }
        public string ID1AadharFront { get; set; }
        public string ID1AadharBack { get; set; }
        public string ID1VoterFront { get; set; }
        public string ID1VoterBack { get; set; }
        public string ID1AadharResponse { get; set; }
        public string ID1VoterResponse { get; set; }
        public string ID2AadharFront { get; set; }
        public string ID2AadharBack { get; set; }
        public string ID2VoterFront { get; set; }
        public string ID2VoterBack { get; set; }
        public string ID2AadharResponse { get; set; }
        public string ID2VoterResponse { get; set; }
        public string NameMatchingResponse { get; set; }
        public string AddressMatchingResponse { get; set; }
        public string FaceMatchingResponse { get; set; }
        public string NameMatchingResponseID2 { get; set; }
        public string AddressMatchingResponseID2 { get; set; }
        public string FaceMatchingResponseID2 { get; set; }
    }
    public class OCRData
    {
        public vOCRData vOCRData { get; set; }
    }
}