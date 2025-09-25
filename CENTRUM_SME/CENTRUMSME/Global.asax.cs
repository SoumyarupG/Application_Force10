using System;
using System.Web;
//using System.Configuration;
using System.Web.Security;

namespace CENTRUMSME
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["KUDOS"] = Session.SessionID;
            var session = HttpContext.Current.Session;
            session["IPAddress"] = Request.UserHostAddress;
            session["LoginTime"] = DateTime.Now;
            session.Timeout = 30;
            // you'll need to plug in here how you're going to determine this
            //session["LoginPlace"] = "something";
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsIdentity oFI = (FormsIdentity)HttpContext.Current.User.Identity;
                        FormsAuthenticationTicket oFATkt = oFI.Ticket;
                        string[] oRole = oFATkt.UserData.Split(',');
                        HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(oFI, oRole);
                    }
                }
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError().InnerException;
            //if (ex != null)
            //{
            //    Session["ErrMsg"] = ex;
            //    Response.RedirectPermanent("~/ErrorInfo.aspx", false);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}