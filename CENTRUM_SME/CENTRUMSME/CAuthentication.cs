using System;
using System.Web;
using CENTRUMBA;

namespace CENTRUMSME
{
    public class CAuthentication
    {
        private string vLogin = "";
        private Int32 vUserId = 0;
        private string vUsrStat="";      
        private string vBrCode = "";
        private int vLogStat = 0;
        private bool vUseAuth = false;
        private Int32 vRole = 0;

        /// <summary>
        /// 
        /// </summary>
        public CAuthentication()
        {
            AuthenticateUser();
        }

        /// <summary>
        /// 
        /// </summary>
        private void AuthenticateUser()
        {
            CUser oUser = null;
            try
            {
                vLogin = HttpContext.Current.User.Identity.Name;
                if (vLogin == "")
                {
                    vUseAuth = false;
                    vLogin = "Guest";
                }
                else
                {
                    oUser = new CUser();
                    if (oUser.GetLoginUser(vLogin) != "" || oUser.GetLoginUser(vLogin) != null)
                    {
                        vUseAuth = true;
                        vLogStat = oUser.GetLoginStatusByUser(vLogin, ref vUserId, ref vRole, ref vUsrStat, ref vBrCode);
                    }
                }
            }
            finally
            {
                oUser = null;
            }
        }      
        
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string UserLogin
        {
            get { return vLogin; }
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 UserId
        {
            get { return vUserId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserStatus
        {
            get { return vUsrStat; }
        }        

        /// <summary>
        /// 
        /// </summary>
        public int LoginStatus
        {
            get { return vLogStat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 UserRoleID
        {
            get { return vRole; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BranchCode
        {
            get { return vBrCode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAuthenticated
        {
            get { return vUseAuth; }
        }

        #endregion
    }
}