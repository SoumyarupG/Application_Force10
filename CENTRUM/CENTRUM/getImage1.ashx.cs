using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using FORCEBA;
namespace CENTRUM
{
    /// <summary>
    /// Summary description for getImage1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class getImage1 : IHttpHandler {

        public void ProcessRequest (HttpContext context) {
            DataTable dt = new DataTable();
            string vId = Convert.ToString(context.Request.QueryString["Id"]);
            string vSubId = Convert.ToString(context.Request.QueryString["SubId"]);
            Int32 vSlNo = 0;
            vSlNo = Convert.ToInt32(vSubId);
            CApplication oMem = new CApplication();
            dt = oMem.GetKYCDocImageById(vId, vSlNo);
            if (dt.Rows.Count > 0)
            {
               
                    byte[] imgBy = (byte[])dt.Rows[0]["PicAtt"];
                    context.Response.Clear();
                    context.Response.ContentType = Convert.ToString(dt.Rows[0]["PicAttType"]).Trim();
                    context.Response.BinaryWrite((byte[])imgBy);
                
            }
        }
     
        public bool IsReusable {
            get {
                return false;
            }
        }
    }
}