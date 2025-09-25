using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Web.Services;
using System.Web;

namespace CENTRUM
{
    /// <summary>
    /// Summary description for getImage
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class getImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string vGid = Convert.ToString(context.Request.QueryString["Gid"]);
            string vbid = Convert.ToString(context.Request.QueryString["bid"]);
            CPopImage oPi = new CPopImage();

            dt = oPi.GetImageByID(vGid);
            if (dt.Rows.Count > 0)
            {
                byte[] imgBy = (byte[])dt.Rows[0]["ImageData"];
                context.Response.Clear();
                context.Response.ContentType = Convert.ToString(dt.Rows[0]["ImageType"]).Trim();
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