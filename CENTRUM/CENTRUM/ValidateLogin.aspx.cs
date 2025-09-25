using System;
using System.Text;
using FORCEBA;

namespace CENTRUM
{
    public partial class ValidateLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["msg"] != null)
            {
                /*BASE 64 DECODE*/
                string encmac = Request.QueryString["msg"].ToString();
                var base64EncodedBytes = System.Convert.FromBase64String(encmac);
                string decmac = Encoding.UTF8.GetString(base64EncodedBytes);
                /****************/
                CLogin cl = new CLogin();

                int status = cl.validateMAC(decmac);

                if (status == 1)
                {
                    Session["authenticate_status"] = "success";
                    Response.RedirectPermanent("Login.aspx");
                }
                else
                {
                    Response.RedirectPermanent("Unauthorized.aspx");
                }
            }
            else
            {
                //Response.Write("cannot read query string !!");
                Response.RedirectPermanent("Unauthorized.aspx");
            }
        }
    }
}