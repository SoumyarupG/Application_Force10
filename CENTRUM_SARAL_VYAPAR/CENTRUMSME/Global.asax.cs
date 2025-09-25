using System;
using System.Web;
//using System.Configuration;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using System.Web.Configuration;

namespace CENTRUM_SARALVYAPAR
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
            //foreach (string cookieKey in Request.Cookies.AllKeys)
            //{
            //    HttpCookie cookie = Request.Cookies[cookieKey];
            //    if (cookie != null)
            //    {
            //        cookie.Path = "/CookiesPath";
            //        Response.Cookies.Add(cookie);
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //if (HttpContext.Current.User != null)
            //{
            //    if (HttpContext.Current.User.Identity.IsAuthenticated)
            //    {
            //        if (HttpContext.Current.User.Identity is FormsIdentity)
            //        {
            //            FormsIdentity oFI = (FormsIdentity)HttpContext.Current.User.Identity;
            //            FormsAuthenticationTicket oFATkt = oFI.Ticket;
            //            string[] oRole = oFATkt.UserData.Split(',');
            //            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(oFI, oRole);
            //        }
            //    }
            //}

        }
        //protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        //{
        //    // only apply session cookie persistence to requests requiring session information
        //    if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
        //    {
        //        var sessionState = ConfigurationManager.GetSection("system.web/sessionState") as SessionStateSection;
        //        var cookieName = sessionState != null && !string.IsNullOrEmpty(sessionState.CookieName) ? sessionState.CookieName : "ASP.NET_SessionId";

        //        var timeout = sessionState != null ? sessionState.Timeout : TimeSpan.FromMinutes(20);

        //        // Ensure ASP.NET Session Cookies are accessible throughout the subdomains.
        //        if (Request.Cookies[cookieName] != null && Session != null && Session.SessionID != null)
        //        {
        //            Response.Cookies[cookieName].Value = Session.SessionID;
        //            Response.Cookies[cookieName].Path = "/Images";
        //            Response.Cookies[cookieName].Expires = DateTime.Now.Add(timeout);
        //        }
        //    }
        //}


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