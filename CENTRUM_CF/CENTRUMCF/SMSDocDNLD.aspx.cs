using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace CENTRUMCF
{
    public partial class SMSDocDNLD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["param"] != null && Request.QueryString["param"] != string.Empty)
                {
                    string vQStr = Convert.ToString(Request.QueryString["param"]);                    
                    string vId = Base64Decode(vQStr.Replace("Vm9zIGZhY3R1cmVzIGltcGF577", "").Replace("WVOVEovDkqwhCF8N", ""));
                    DocDownload(vId);
                    Page.ClientScript.RegisterStartupScript((typeof(Page)), "closePage", "<script type='text/JavaScript'>window.close();</script>");
                }
            }
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        protected void DocDownload(string pId)
        {
            try
            {
                string vServerFilePath = ConfigurationManager.AppSettings["DocDownloadPath"] +"\\DigitalDoc\\";
                string vFileName = pId + ".pdf";
                string vFilePath = vServerFilePath + vFileName;
                if (File.Exists(vFilePath))
                {
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment;filename=" + vFilePath);
                    Response.WriteFile(vFilePath);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript((typeof(Page)), "closePage", "<script type='text/JavaScript'>window.close();</script>");
                }
            }
            finally
            {
            }
        }
    }
}
