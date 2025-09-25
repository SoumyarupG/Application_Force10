using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Configuration;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DigiDocViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string vLoanAppId = Request.QueryString["id"];
                //string vLoanAppId = "";
                ViewDigiDoc(vLoanAppId);
            }
        }

        public void ViewDigiDoc(string pLoanAppId)
        {
            try
            {
                string vPathDigiDoc = GetDigiDocPath(pLoanAppId);
                if (vPathDigiDoc != "")
                {
                    using (WebClient cln = new WebClient())
                    {
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        vDoc = cln.DownloadData(vPathDigiDoc);
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.Buffer = true;
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.BinaryWrite(vDoc);
                        Response.End();
                        Response.Close();
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Data Found..");
                }
            }
            finally
            {
            }
        }

        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
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

        public string GetDigiDocPath(string Id)
        {
            string pathNetwork = ConfigurationManager.AppSettings["pathKycNetwork"];
            string[] arrPathNetwork = pathNetwork.Split(',');
            int i;
            string vPathDigiDoc = "";
            for (i = 0; i <= arrPathNetwork.Length - 1; i++)
            {
                if (ValidUrlChk(arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + "DigitalDoc/" + Id + ".pdf";
                    break;
                }
                else if (ValidUrlChk(arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + "jlgdigitaldocs/" + Id + ".pdf";
                    break;
                }
                else if (ValidUrlChk(arrPathNetwork[i] + Id + ".pdf"))
                {
                    vPathDigiDoc = arrPathNetwork[i] + Id + ".pdf";
                    break;
                }
            }
            return vPathDigiDoc;
        }
    }
}