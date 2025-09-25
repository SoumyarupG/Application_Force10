using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class FetchAadhaarDetails : CENTRUMBase
    {

        string pMobService = ConfigurationManager.AppSettings["MobService"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Fetch Aadhaar Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuFetchAadhaarDetails);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Fetch Aadhaar Details", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            string vRequestJson = "{\"refId\": \"" + txtAadhRefNo.Text + "\"}";
            string vFullResponse = HttpRequest(pMobService + "/GetAadhaarNoByRefId", vRequestJson);
            dynamic objFullResponse = JsonConvert.DeserializeObject(vFullResponse);
            string vAadhaarNo = objFullResponse.results[0].refData;
            txtAadhNo.Text = vAadhaarNo;
        }
        public string HttpRequest(string PostUrl, string Requestdata)
        {
            string vResponse = "";
            try
            {
                string postURL = PostUrl;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = streamReader.ReadToEnd();
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
            }
            return vResponse;
        }

    }

}
