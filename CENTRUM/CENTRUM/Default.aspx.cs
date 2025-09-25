using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace CENTRUM
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        //    byte[] vimg;
        //    using (WebClient webclient = new WebClient())
        //    {
        //        vimg = webclient.DownloadData("https://centrummob.bijliftt.com/Files/InitialApproach/11180683939/IDProofImage.png");
        //    }
          
        //    Dictionary<string, object> postParameters = new Dictionary<string, object>();
        //    postParameters.Add("IdData", "8600196812#1118#111861192");
        //    postParameters.Add("IMAGE", new FormUpload.FileParameter(vimg, "Id.png", "image/png"));

        //    // Create request and receive response
        //   // string postURL = "https://centrummob.bijliftt.com/CentrumService.svc/OCRPhototoData";
        //    string postURL = "http://localhost:8419/CentrumService.svc/OCRPhototoData";
        //    string userAgent = "Someone";
        //    HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(postURL, userAgent, postParameters);

        //    // Process response
        //    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
        //    string fullResponse = responseReader.ReadToEnd();
        //    webResponse.Close();
        //    Response.Write(fullResponse);
        }


    }
}
