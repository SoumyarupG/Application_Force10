using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using FORCECA;
using FORCEBA;
using System.Xml;
using System.Data;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class KarzaReTrigger : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                popMember();
                popIdentityProof();
                popAddProof();
            }
        }

        private void popMember()
        {
            DataTable dt = null;
            CNewMember oNm = null;
            try
            {
                oNm = new CNewMember();
                dt = oNm.GetReTriggerData(Convert.ToString(Session[gblValue.BrnchCode]));
                Session["dtRetrigger"] = dt;
                ddlMember.DataSource = dt;
                ddlMember.DataTextField = "MemberName";
                ddlMember.DataValueField = "MemberID";
                ddlMember.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMember.Items.Insert(0, oli);
            }
            finally
            {
                oNm = null;
                dt = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Karza Retrigger";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuKarzaRetrigger);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnVerify.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Karza Retrigger", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void ddlMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearControl();
            DataSet ds = null;
            DataTable dt = null;
            DataTable dtEnq = null;
            CMember oMem = null;
            txtCoApplName.Text = "";
            try
            {
                oMem = new CMember();
                ds = oMem.GetMemberDetails(ddlMember.SelectedValue, "", Convert.ToString(Session[gblValue.BrnchCode]), Convert.ToInt32(Session[gblValue.UserId].ToString()));
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtAdmDt.Text = Convert.ToString(dt.Rows[0]["AdmDate"]);
                    ddlAddPrf.SelectedIndex = ddlAddPrf.Items.IndexOf(ddlAddPrf.Items.FindByValue(dt.Rows[0]["M_AddProfId"].ToString()));
                    ddlIdentyProf.SelectedIndex = ddlIdentyProf.Items.IndexOf(ddlIdentyProf.Items.FindByValue(dt.Rows[0]["M_IdentyPRofId"].ToString()));
                    txtAddPrfNo.Text = Convert.ToString(dt.Rows[0]["M_AddProfNo"]);
                    txtIdentyProofNo.Text = Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]);

                    ddlAddPrf2.SelectedIndex = ddlAddPrf2.Items.IndexOf(ddlAddPrf2.Items.FindByValue(dt.Rows[0]["B_IdentyProfId"].ToString()));
                    txtIdProof3.Text = Convert.ToString(dt.Rows[0]["B_IdentyProfNo"]);
                    txtCoApplName.Text = Convert.ToString(dt.Rows[0]["B_FName"]) + " " + Convert.ToString(dt.Rows[0]["B_MName"]) + " " + Convert.ToString(dt.Rows[0]["B_LName"]);

                    if (dt.Rows[0]["B_IdentyProfId"].ToString() == "3")
                    {
                        btnCoAppVerify.Visible = true;
                    }
                    else
                    {
                        btnCoAppVerify.Visible = false;
                    }

                }

                DataTable dt1 = (DataTable)Session["dtRetrigger"];
                dtEnq = dt1.Select("MemberID = '" + ddlMember.SelectedValue + "'").CopyToDataTable();
                dtEnq.AcceptChanges();
                // dtEnq = oMem.GetLastEnqIdByMemId(ddlMember.SelectedValue);
                if (dtEnq.Rows.Count > 0)
                {
                    Session["EnquiryId"] = dtEnq.Rows[0]["EnquiryId"].ToString();
                }
            }
            finally { }
        }

        private void popAddProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("Y", "N");

                ddlAddPrf.DataSource = dt;
                ddlAddPrf.DataTextField = "IDProofName";
                ddlAddPrf.DataValueField = "IDProofId";
                ddlAddPrf.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlAddPrf.Items.Insert(0, oli1);

            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

        private void popIdentityProof()
        {
            DataTable dt = null;
            CNewMember oNM = null;
            try
            {
                oNM = new CNewMember();
                dt = oNM.popIdAddrProof("N", "Y");
                ddlIdentyProf.DataSource = dt;
                ddlIdentyProf.DataTextField = "IDProofName";
                ddlIdentyProf.DataValueField = "IDProofId";
                ddlIdentyProf.DataBind();
                ListItem oli = null;
                oli = new ListItem("<--Select-->", "-1");
                ddlIdentyProf.Items.Insert(0, oli);

                ddlAddPrf2.DataSource = dt;
                ddlAddPrf2.DataTextField = "IDProofName";
                ddlAddPrf2.DataValueField = "IDProofId";
                ddlAddPrf2.DataBind();
                oli = new ListItem("<--Select-->", "-1");
                ddlAddPrf2.Items.Insert(0, oli);
            }
            finally
            {
                oNM = null;
                dt = null;
            }
        }

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

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            CNewMember oNm = null;
            oNm = new CNewMember();
            int vCnt = oNm.GetKarzaRetryCnt(Convert.ToString(Session["EnquiryId"]));
            if (vCnt < 4)
            {
                string vEoId = hdnEoId.Value;
                string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
                oNm = new CNewMember();
                string vErrMsg = string.Empty;
                string vXml = "";
                var req = new KYCVoterRequest()
                {
                    consent = "Y",
                    epic_no = txtIdentyProofNo.Text
                };
                string Requestdata = JsonConvert.SerializeObject(req);

                //string postURL = "https://testapi.karza.in/v2/voter";
                string postURL = "https://api.karza.in/v2/voter";
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
                    request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
                    request.Host = "api.karza.in";

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
                    // responsedata = "{\"result\":{\"name\":\"POOJA ASHOK PAWAR\",\"rln_name\":\"ASHOK PAWAR\",\"rln_type\":\"H\",\"gender\":\"F\",\"district\":\"MumbaiSuburban\",\"ac_name\":\"Kalina\",\"pc_name\":\"Mumbai North-Central\",\"state\":\"Maharashtra\",\"epic_no\":\"TMF3673688\",\"dob\":\"\",\"age\":29,\"part_no\":\"42\",\"slno_inpart\":\"501\",\"ps_name\":\"Mary Immaculate High School Ground Floor Room No 3 Kalina Church Marg Santacruz (E) Mumbai -400029\",\"part_name\":\"Kalina Church Road, Santacruz (East), Mumbai 400 098.\",\"last_update\":\"08-04-2021\",\"ps_lat_long\":\"0.0,0.0\",\"rln_name_v1\":\"\\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"rln_name_v2\":\"\",\"rln_name_v3\":\"\",\"section_no\":\"11\",\"id\":\"S131750042110501\",\"name_v1\":\"\\u092a\\u0941\\u091c\\u093e \\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"name_v2\":\"\",\"name_v3\":\"\",\"ac_no\":\"175\",\"st_code\":\"S13\",\"house_no\":\"\"},\"request_id\":\"2e494b33-cf53-4dd0-a0e6-eb5f4b3a7a28\",\"status-code\":\"101\"}";

                    KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(responsedata.Replace("status-code", "status_code"));
                    try
                    {
                        responsedata = responsedata.Replace("\u0000", "");
                        responsedata = responsedata.Replace("\\u0000", "");
                        vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                        oNm.SaveKarzaVoterVerifyData(txtIdentyProofNo.Text, vXml, vBranch, vEoId);//Save Response
                    }
                    finally
                    {
                        //---
                    }

                    if (vResponseObj.status_code == "101")
                    {
                        vErrMsg = "101:Valid Authentication";
                        //dynamic res = NameMatching(ddlMember.SelectedItem.Text, vResponseObj.result.name);
                        //string vNameJson = JsonConvert.SerializeObject(res);
                        //string vNmXml = "<root>" + AsString(JsonConvert.DeserializeXmlNode(vNameJson, "NameMatchingResult")) + "</root>";
                        string vNmXml = "";
                        oNm = new CNewMember();
                        vXml = "<root>" + vXml.Replace("root", "KarzaVoterIDKYCValidationResult") + "</root>";
                        oNm.UpDateOCRData(Convert.ToString(Session["EnquiryId"]), vXml, vNmXml, "A");
                    }

                    else if (vResponseObj.status_code == "102")
                    {
                        vErrMsg = "102:Invalid ID number or combination of inputs";
                    }
                    else if (vResponseObj.status_code == "103")
                    {
                        vErrMsg = "103:No records found for the given ID or combination of inputs";
                        oNm = new CNewMember();
                        oNm.SaveRedFlag(ddlMember.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
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

                    if (vResponseObj.status_code == "102")
                    {
                        txtIdentyProofNo.Enabled = true;
                        btnVerify.Enabled = true;
                        oNm = new CNewMember();
                        oNm.SaveKarzaVerificationRetry(Convert.ToString(Session["EnquiryId"]));
                        if (vCnt == 3)
                        {
                            oNm = new CNewMember();
                            oNm.SaveRedFlag(ddlMember.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
                        }
                    }
                    else
                    {
                        btnVerify.Enabled = false;
                    }
                    gblFuction.AjxMsgPopup(vErrMsg);
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
                    vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    oNm.SaveKarzaVoterVerifyData(txtIdentyProofNo.Text, vXml, vBranch, vEoId);

                    Response.Replace("requestId", "request_id");
                    KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(Response);

                    //HttpWebResponse res = (HttpWebResponse)we.Response;
                    //KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();

                    if (Convert.ToString(vResponseObj.status_code) == "400")
                    {
                        vErrMsg = "400:Bad Request";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "401")
                    {
                        vErrMsg = "401:Unauthorized Access";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "402")
                    {
                        vErrMsg = "402:Insufficient Credits";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "500")
                    {
                        vErrMsg = "500:Internal Server Error";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "503")
                    {
                        vErrMsg = "503:Source Unavailable";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "504")
                    {
                        vErrMsg = "504:Endpoint Request Timed Out";
                    }
                    else
                    {
                        vErrMsg = Convert.ToString(vResponseObj.status_code);
                    }

                    btnVerify.Enabled = true;
                    gblFuction.AjxMsgPopup(vErrMsg);
                }
                finally
                {
                    // streamWriter = null;
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("Maximum retry Exceeded..!");
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnCoAppVerify_Click(object sender, EventArgs e)
        {
            CNewMember oNm = null;
            oNm = new CNewMember();
            int vCnt = oNm.GetKarzaRetryCnt(Convert.ToString(Session["EnquiryId"]));
            if (vCnt < 4)
            {
                string vEoId = hdnEoId.Value;
                string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
                oNm = new CNewMember();
                string vErrMsg = string.Empty;
                string vXml = "";
                var req = new KYCVoterRequest()
                {
                    consent = "Y",
                    epic_no = txtIdProof3.Text
                };
                string Requestdata = JsonConvert.SerializeObject(req);

                string postURL = "https://api.karza.in/v2/voter";
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
                    request.Headers.Add("x-karza-key", "VOht331uCcRtBgI");
                    request.Host = "api.karza.in";

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
                    // responsedata = "{\"result\":{\"name\":\"POOJA ASHOK PAWAR\",\"rln_name\":\"ASHOK PAWAR\",\"rln_type\":\"H\",\"gender\":\"F\",\"district\":\"MumbaiSuburban\",\"ac_name\":\"Kalina\",\"pc_name\":\"Mumbai North-Central\",\"state\":\"Maharashtra\",\"epic_no\":\"TMF3673688\",\"dob\":\"\",\"age\":29,\"part_no\":\"42\",\"slno_inpart\":\"501\",\"ps_name\":\"Mary Immaculate High School Ground Floor Room No 3 Kalina Church Marg Santacruz (E) Mumbai -400029\",\"part_name\":\"Kalina Church Road, Santacruz (East), Mumbai 400 098.\",\"last_update\":\"08-04-2021\",\"ps_lat_long\":\"0.0,0.0\",\"rln_name_v1\":\"\\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"rln_name_v2\":\"\",\"rln_name_v3\":\"\",\"section_no\":\"11\",\"id\":\"S131750042110501\",\"name_v1\":\"\\u092a\\u0941\\u091c\\u093e \\u0905\\u0936\\u094b\\u0915 \\u092a\\u0935\\u093e\\u0930\",\"name_v2\":\"\",\"name_v3\":\"\",\"ac_no\":\"175\",\"st_code\":\"S13\",\"house_no\":\"\"},\"request_id\":\"2e494b33-cf53-4dd0-a0e6-eb5f4b3a7a28\",\"status-code\":\"101\"}";

                    KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(responsedata.Replace("status-code", "status_code"));
                    try
                    {
                        responsedata = responsedata.Replace("\u0000", "");
                        responsedata = responsedata.Replace("\\u0000", "");
                        vXml = AsString(JsonConvert.DeserializeXmlNode(responsedata.Replace("status-code", "status_code"), "root"));
                        oNm.SaveKarzaVoterVerifyData(txtIdProof3.Text, vXml, vBranch, vEoId);//Save Response
                    }
                    finally
                    {
                        //---
                    }

                    if (vResponseObj.status_code == "101")
                    {
                        vErrMsg = "101:Valid Authentication";
                        //dynamic res = NameMatching(txtCoApplName.Text, vResponseObj.result.name);
                        //string vNameJson = JsonConvert.SerializeObject(res);
                        //string vNmXml = "<root>" + AsString(JsonConvert.DeserializeXmlNode(vNameJson, "NameMatchingResult")) + "</root>";tring vNmXml =
                        string vNmXml = "";
                        oNm = new CNewMember();
                        vXml = "<root>" + vXml.Replace("root", "KarzaVoterIDKYCValidationResult") + "</root>";
                        oNm.UpDateOCRData(Convert.ToString(Session["EnquiryId"]), vXml, vNmXml, "C");
                    }

                    else if (vResponseObj.status_code == "102")
                    {
                        vErrMsg = "102:Invalid ID number or combination of inputs";
                    }
                    else if (vResponseObj.status_code == "103")
                    {
                        vErrMsg = "103:No records found for the given ID or combination of inputs";
                        oNm = new CNewMember();
                        oNm.SaveRedFlag(ddlMember.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
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

                    if (vResponseObj.status_code == "102")
                    {
                        txtIdentyProofNo.Enabled = true;
                        btnVerify.Enabled = true;
                        oNm = new CNewMember();
                        oNm.SaveKarzaVerificationRetry(Convert.ToString(Session["EnquiryId"]));
                        if (vCnt == 3)
                        {
                            oNm = new CNewMember();
                            oNm.SaveRedFlag(ddlMember.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]));
                        }
                    }
                    else
                    {
                        btnVerify.Enabled = false;
                    }
                    gblFuction.AjxMsgPopup(vErrMsg);
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
                    vXml = AsString(JsonConvert.DeserializeXmlNode(Response, "root"));
                    oNm.SaveKarzaVoterVerifyData(txtIdentyProofNo.Text, vXml, vBranch, vEoId);

                    Response.Replace("requestId", "request_id");
                    KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();
                    vResponseObj = JsonConvert.DeserializeObject<KYCVoterIDResponse>(Response);

                    //HttpWebResponse res = (HttpWebResponse)we.Response;
                    //KYCVoterIDResponse vResponseObj = new KYCVoterIDResponse();

                    if (Convert.ToString(vResponseObj.status_code) == "400")
                    {
                        vErrMsg = "400:Bad Request";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "401")
                    {
                        vErrMsg = "401:Unauthorized Access";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "402")
                    {
                        vErrMsg = "402:Insufficient Credits";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "500")
                    {
                        vErrMsg = "500:Internal Server Error";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "503")
                    {
                        vErrMsg = "503:Source Unavailable";
                    }
                    else if (Convert.ToString(vResponseObj.status_code) == "504")
                    {
                        vErrMsg = "504:Endpoint Request Timed Out";
                    }
                    else
                    {
                        vErrMsg = Convert.ToString(vResponseObj.status_code);
                    }

                    btnVerify.Enabled = true;
                    gblFuction.AjxMsgPopup(vErrMsg);
                }
                finally
                {
                    // streamWriter = null;
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("Maximum retry Exceeded..!");
            }
        }

        private void ClearControl()
        {
            //ddlMember.SelectedIndex = -1;
            ddlAddPrf.SelectedIndex = -1;
            ddlIdentyProf.SelectedIndex = -1;
            ddlAddPrf2.SelectedIndex = -1;
            txtIdentyProofNo.Text = "";
            txtIdProof3.Text = "";
            txtAddPrfNo.Text = "";
            txtAdmDt.Text = "";
        }

        public class NameMatchRequest
        {
            public string name1 { get; set; }
            public string name2 { get; set; }
            public string type { get; set; }
            public string preset { get; set; }
        }

        public class NameMatch
        {
            public string name1 { get; set; }
            public string name2 { get; set; }
            public string pBranch { get; set; }
            public string pEoID { get; set; }
            public string pIdNo { get; set; }
        }

        #region NameMatching
        public MatchResponse NameMatching(string vName1, string vName2)
        {
            var req = new NameMatchRequest()
            {
                name1 = vName1,
                name2 = vName2,
                type = "individual",
                preset = "l"
            };

            string requestBody = JsonConvert.SerializeObject(req);
            //string postURL = "https://testapi.karza.in/v3/name";
            string postURL = "https://api.karza.in/v3/name";

            string pBranch = Convert.ToString(Session[gblValue.BrnchCode]);
            string pIdNo = txtIdentyProofNo.Text;
            string pEoID = hdnEoId.Value;
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
                // request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
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
                // string fullResponse = "{\"statusCode\":101,\"requestId\":\"80bc78a4-c7b2-464b-9180-62c319f91fc7\",\"result\":{\"score\":1.0,\"result\":true}}";
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(fullResponse);
                try
                {
                    fullResponse = fullResponse.Replace("\u0000", "");
                    fullResponse = fullResponse.Replace("\\u0000", "");
                    string vXml = AsString(JsonConvert.DeserializeXmlNode(fullResponse, "root"));
                    CNewMember oNM = new CNewMember();
                    oNM.SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);
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
                CNewMember oNM = new CNewMember();
                oNM.SaveKarzaMatchingDtl("NameMatching", vXml, pBranch, pEoID, pIdNo);

                dynamic res = JsonConvert.DeserializeObject(Response.Replace("status", "statusCode"));
                return new MatchResponse(0.00, false, Convert.ToString(res.requestId), Convert.ToString(res.statusCode));
            }
            finally
            {
                // streamWriter = null;
            }
        }
        #endregion
    }


    public class KYCVoterRequest
    {
        public string consent { get; set; }
        public string epic_no { get; set; }
    }

    public class KYCVoterIDResponse
    {
        public string request_id { get; set; }
        public string status_code { get; set; }
        public KYCVoterIDResponseResult result { get; set; }
    }

    public class KYCVoterIDResponseResult
    {
        public string ps_lat_long { get; set; }
        public string rln_name_v1 { get; set; }
        public string rln_name_v2 { get; set; }
        public string rln_name_v3 { get; set; }
        public string part_no { get; set; }
        public string rln_type { get; set; }
        public string section_no { get; set; }
        public string id { get; set; }
        public string epic_no { get; set; }
        public string rln_name { get; set; }
        public string district { get; set; }
        public string last_update { get; set; }
        public string state { get; set; }
        public string ac_no { get; set; }
        public string slno_inpart { get; set; }
        public string ps_name { get; set; }
        public string pc_name { get; set; }
        public string house_no { get; set; }
        public string name { get; set; }
        public string part_name { get; set; }
        public string st_code { get; set; }
        public string gender { get; set; }
        public string age { get; set; }
        public string ac_name { get; set; }
        public string name_v1 { get; set; }
        public string dob { get; set; }
        public string name_v3 { get; set; }
        public string name_v2 { get; set; }
    }

    public class MatchResponse
    {
        public double score { get; set; }
        public bool result { get; set; }
        public string requestId { get; set; }
        public string statuscode { get; set; }
        public MatchResponse(double score, bool result, string requestId, string statuscode)
        {
            this.score = score;
            this.result = result;
            this.requestId = requestId;
            this.statuscode = statuscode;
        }
    }
}