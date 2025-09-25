using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Timers;
using CENTRUM.WebSrvcs;
using System.Web.Routing;
using System.Web.Configuration;
using System.Configuration;

namespace CENTRUM
{
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            //RegisterRoutes(RouteTable.Routes);
            System.Timers.Timer oTm = new System.Timers.Timer();
            oTm.Start();
            oTm.Interval = (60000 * 1 * 1);
            oTm.Elapsed += Timer_Elapsed;
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SMSService sms = null;
            try
            {
                sms = new SMSService();
                System.Threading.Thread.Sleep(7000);
                sms.SendAllSMS();
                // sms.UpdateDelivStatus();
                System.Threading.Thread.Sleep(7000);
            }
            finally
            {
                sms = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["ErrMsg"] = null;
            // Session["CENTRUM"] = Session.SessionID;           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //var sessionCookie = Request.Cookies["ASP.NET_SessionId"];
            //if (sessionCookie != null)
            //{
            //    sessionCookie.Path = "/Images";
            //    Response.Cookies.Add(sessionCookie);
            //}

              //  MoveCookiesToAdminPath();
        }
        private void MoveCookiesToAdminPath()
        {
            HttpContext context = HttpContext.Current;
            foreach (string cookieName in context.Request.Cookies.AllKeys)
            {
                HttpCookie expiredCookie = new HttpCookie(cookieName);
                expiredCookie.Expires = DateTime.Now.AddDays(-1);
                expiredCookie.Path = "/";
                context.Response.Cookies.Add(expiredCookie);
                HttpCookie oldCookie = context.Request.Cookies[cookieName];
                if (oldCookie != null)
                {
                    HttpCookie newCookie = new HttpCookie(cookieName, oldCookie.Value);                  
                    newCookie.Path = "/Admin";
                    context.Response.Cookies.Add(newCookie);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError().InnerException;
            if (ex != null)
            {
                Session["ErrMsg"] = ex;
                Response.RedirectPermanent("~/ErrorInfo.aspx", false);
            }
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

        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    routes.Ignore("{resource}.axd/{*pathInfo}");  // This line ensures WebResource.axd is not processed by the routing module[_{{{CITATION{{{_1{How to fix WebResource.axd JavaScript errors appearing when using asp ...](https://irishdotnet.dev/how-to-fix-webresource-axd-javascript-errors-appearing-when-using-asp-net-routing/)
        //    // Add your other routes here
        //    routes.MapPageRoute("Default", "{*url}", "~/Login.aspx");
        //}
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
        //            //Response.Cookies[cookieName].Expires = DateTime.Now.Add(timeout);
        //        }
        //    }
        //}
    }
}