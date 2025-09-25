using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using FORCEBA;
using FORCECA;

namespace CENTRUM
{
    /// <summary>
    /// Summary description for GetKYCVerify
    /// </summary>
    public class GetKYCVerify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            DataTable dt = new DataTable();
            string vslno = Convert.ToString(context.Request.QueryString["Id"]);
            string vType = Convert.ToString(context.Request.QueryString["vType"]);
            Int32 vDocId = Convert.ToInt32(vType);
            CApplication oMem = new CApplication();
            dt = oMem.GetKYCDocImageByIdVerify(vslno, vDocId);

            //Session[gblValue.DocVerify] = Convert.ToString(dt.Rows[0]["DocVerfied"]);
            if (dt.Rows.Count > 0)
            {

                byte[] imgBy = (byte[])dt.Rows[0]["Doc_Image"];
                context.Response.Clear();
                context.Response.ContentType = Convert.ToString(dt.Rows[0]["File_name"]).Trim();
                context.Response.BinaryWrite((byte[])imgBy);

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