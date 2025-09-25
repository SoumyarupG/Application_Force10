using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using FORCEBA;

namespace CENTRUM
{
    public class CENTRUMBase: Page 
    {
        public CentrumMFI oMarg = null;
        protected CUser oUser = null;        
        private string vCanView = "";
        private string vCanAdd = "";
        private string vCanEdit = "";
        private string vCanDelete = "";
        private string vCanRpt = "";
        private string vCanProc = "";
        private Int32 vRoleID = 0;
        private Int32 vUserID = 0;
        private string vUserHO = "";
        private string vUserName = "";

        /// <summary>
        /// 
        /// </summary>
        public CENTRUMBase()
        {         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Arguments"></param>
        override protected void OnInit(System.EventArgs Arguments)
        {
            if (oMarg == null)
                oMarg = (CentrumMFI)(this.Master);

            if (IsPostBack == false)
                InitializeComponent();
        }

        
        
        /// <summary>
        /// 
        /// </summary>
        private void InitializeComponent()
        {
            CAuthentication oAth = null;
            try
            {
                oAth = new CAuthentication();
                if (oAth.IsAuthenticated == true)
                {
                    if (oMarg != null)
                    {
                        oMarg.ShowLoginInfo = oAth.UserLogin.ToUpper();
                    }
                }
            }
            finally
            {
                oAth = null;
            }
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_Module_Id"></param>
        protected void GetModuleByRole(Int32 pModuleId)
        {
            Int32 pRoleId = 0, vInt = 0;
            CAuthentication oAuth = null;
            try
            {
                oAuth = new CAuthentication();
                pRoleId = oAuth.UserRoleID;
                vRoleID = pRoleId;
                vUserID = oAuth.UserId;
                vUserName = oAuth.UserLogin;
                oUser = new CUser();
                vInt = oUser.GetModuleByRole(pRoleId, pModuleId, ref vCanView, ref  vCanAdd, ref vCanEdit, ref vCanDelete, ref vCanRpt, ref vCanProc);
            }
            finally
            {
                oAuth = null;
            }
        }
        
        #region Properties

        public bool Menu
        {
            set { oMarg.Menu = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Welcome
        {
            set { oMarg.Welcome = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string ShowLoginInfo
        {
            set { oMarg.ShowLoginInfo = value; }
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string PageHeading
        {
            set { oMarg.PageHeading = value; }
        }         

        /// <summary>
        /// 
        /// </summary>
        public string ShowBranchName
        {
            set { oMarg.ShowBranchName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ShowFinYear
        {
            set { oMarg.ShowFinYear = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Int32 RoleId
        {
            get { return vRoleID; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Int32 UserID
        {
            get { return vUserID; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string UserName
        {
            get { return vUserName; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string UserHO
        {
            get { return vUserHO;}
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string CanView
        {
            get { return vCanView; }
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string CanAdd
        {
            get { return vCanAdd; }
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string CanEdit
        {
            get { return vCanEdit; }
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string CanDelete
        {
            get { return vCanDelete; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected string CanReport
        {
            get { return vCanRpt; }
        }

        /// <summary>        
        /// 
        /// </summary>
        protected string CanProcess
        {
            get { return vCanProc; }
        }     

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //AutoRedirect();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AutoRedirect()
        {
            int int_MilliSecondsTimeOut = (this.Session.Timeout * 60000);
            string str_Script = @"
				var path = $(window.parent.location).attr('href');
                var i = path.lastIndexOf(""/"") + 1;
                var loc = path.substring(i, path.length);
                if(loc != 'Login.aspx')
                    window.setInterval('Redirect()'," + int_MilliSecondsTimeOut.ToString() + @"  );
                function Redirect() {
                    alert('Your session has been expired and Application redirects to login page now.!\n\n');

                    window.location.href = '/Login.aspx';
                    HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
                    Response.cookies.clear();
                    FormsAuthentication.SignOut();
                    Session.Abandon();
                }";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Redirect", str_Script, true);
        }
    }
}