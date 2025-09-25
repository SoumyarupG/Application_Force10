using System;
using System.Data;
using System.Web;
using System.Web.Services;
using CENTRUMBA;

namespace CENTRUM_SARALVYAPAR
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class getImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataTable dt = new DataTable();
            string vId = Convert.ToString(context.Request.QueryString["Id"]);
            CMember oMem = new CMember();
            dt = oMem.GetMemberPicById(vId);
            if (dt.Rows.Count>0)
            {
                byte[] imgBy = (byte[])dt.Rows[0]["PicAtt"];
                context.Response.Clear();
                context.Response.ContentType = Convert.ToString(dt.Rows[0]["PicAttType"]).Trim();
                context.Response.BinaryWrite(imgBy);
            }  
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
