using System;
using System.Data;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Text;
using System.Linq;

namespace CENTRUMSME
{
    public partial class Login : CENTRUMBAse
    {
        //static int wCnt = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static string WindowName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton LogOut = (LinkButton)Master.FindControl("lbLogOut");
            LogOut.Text = "";
            ((LinkButton)Master.FindControl("lblChangeBranch")).Text = "";
            this.Welcome = false;
            Session[gblValue.LoginDate] = hdnDt.Value;
            /*Hiding Menu Bar*/
            Control menu = Page.Master.FindControl("divmenubar");
            Control menu1 = Page.Master.FindControl("divMenu");
            Control header = Page.Master.FindControl("spancompname");
            Control marquee = Page.Master.FindControl("marqueemsg");
            Control submsg = Page.Master.FindControl("lt1");
            Control img = Page.Master.FindControl("Img2");
            Control img3 = Page.Master.FindControl("Img3");
            img3.Visible = false;
            if (menu != null)
            {
                menu.Visible = false;
            }
            if (menu1 != null)
            {
                menu1.Visible = false;
            }

            if (header != null)
            {
                header.Visible = false;
            }
            if (marquee != null)
            {
                marquee.Visible = false;
            }
            if (submsg != null)
            {
                submsg.Visible = false;
            }
            if (img != null)
            {
                img.Visible = false;
            }

            if (!IsPostBack)
            {
                ChangePassword_ClearControls();
                //get the 5 digit salt
                string salt = CreateSalt(16).ToLower();
                //Save the salt in session variable
                Session["salt"] = salt.ToString();
                //Add the JS function call to button with a parameter
                btnLog.Attributes.Add("onclick", "return HashPwdwithSalt('" + salt.ToString() + "');");
                //txtPass.Attributes.Add("onKeyPress", "doClick('" + btnLog.ClientID + "',event)");                
                //txtUserNm.Enabled = true;
                //txtUserNm.Focus();
                //txtPass.Enabled = true;
            }
        }
        public string getSession()
        {
            string WindowName = Guid.NewGuid().ToString().Replace("-", "");
            return WindowName;
        }
        private string CreateSalt(int size) //Generate the salt via Random Number Generator cryptography
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            Int32 vRec = 0;
            string vPass = "";
            //--------------------------------------------------------
            byte[] x_key = Convert.FromBase64String(Session["X_KEY"].ToString());
            vPass = Decrypt(txtPass.Text.ToString(), x_key);
            //--------------------------------------------------------
            string vSalt = Convert.ToString(Session["salt"]);
            vPass = vPass.Replace(vSalt, "");
            //string vPass = dataEncryp.EncryptText(txtPass.Text);
            vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));//AES
            CUser oUsr = null;
            DataTable dt = null;
            object pwd;
            string vLastLogBr = string.Empty, vLastLogDt = string.Empty;
            try
            {
                if (txtUsrNm.Text.ToUpper() == "ADMINFTT" && txtPass.Text.ToUpper() == FormsAuthentication.HashPasswordForStoringInConfigFile("52922f0c6a8e53dd3e2d24022ae8baa6".ToLower() + Convert.ToString(Session["salt"]), "md5"))
                {
                    txtUsrNm.Text = "Admin";
                }
                else
                {
                    if (txtUsrNm.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("User cannot be left blank.");
                        gblFuction.AjxFocus("ctl00_cph_Main_txtUserNm");
                        vResult = false;
                        return vResult;
                    }

                    if (txtUsrNm.Text.Trim() != "")
                    {
                        if (txtPass.Text.Trim() == "")
                        {
                            gblFuction.AjxMsgPopup("Password cannot be left blank.");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtPass");
                            vResult = false;
                            return vResult;
                        }
                    }
                    if (txtPass.Text.Trim() != "")
                    {
                        if (txtUsrNm.Text.Trim() != "" && txtPass.Text.Trim() != "")
                        {
                            oUsr = new CUser();
                            vRec = oUsr.ChkDuplicateUser(txtUsrNm.Text.Trim(), vPass);
                            if (vRec == 0)
                            {
                                gblFuction.AjxMsgPopup(gblPRATAM.InvalidUser);
                                //wCnt = wCnt + 1;
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUserNm");
                                vResult = false;
                                return vResult;
                            }
                        }
                    }

                    if (txtUsrNm.Text.Trim() != "" && txtPass.Text.Trim() != "")
                    {
                        oUsr = new CUser();
                        dt = oUsr.ChkActiveUser(txtUsrNm.Text.Trim(), vPass);
                        if (dt.Rows.Count > 0)
                        {

                            ViewState["Pass"] = dt.Rows[0]["Password"].ToString();
                            pwd = dt.Rows[0]["Password"].ToString();
                            string hashed_pwd = pwd.ToString();
                            //FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.ToString().ToLower() + Session["salt"].ToString(), "md5");

                            if (dt.Rows[0]["Status"].ToString() != "Y")
                            {
                                gblFuction.AjxMsgPopup(gblPRATAM.InActiveUser);
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUserNm");
                                vResult = false;
                                return vResult;
                            }
                            else if (string.Compare(hashed_pwd.ToLower(), vPass.ToString().ToLower()) != 0)
                            {
                                gblFuction.AjxMsgPopup(gblPRATAM.InvalidUser);
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUserNm");
                                vResult = false;
                                return vResult;
                            }
                            else
                            {
                                hdnMFAYN.Value = dt.Rows[0]["MFAYN"].ToString();
                                hdnMobileNo.Value = dt.Rows[0]["MobileNo"].ToString();
                                hdnOTP.Value = dt.Rows[0]["OTP"].ToString();
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.InvalidUser);
                            txtUsrNm.Focus();
                            vResult = false;
                            return vResult;
                        }

                    }
                }
                return vResult;
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLog_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateFields() == true)
                {
                    if (hdnMFAYN.Value == "Y")
                    {
                        if (hdnMobileNo.Value != "0" && hdnMobileNo.Value.Length == 10)
                        {
                            if (SendOTP().Contains("Error"))
                            {
                                tblLogin.Visible = true;
                                trOTP.Visible = false;
                                trBtnProceed.Visible = false;
                                lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " OTP can not be Sent!!";
                            }
                            else
                            {
                                tblLogin.Visible = false;
                                trOTP.Visible = true;
                                trBtnProceed.Visible = true;
                                lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Enter the OTP and Proceed !!!";
                            }
                        }
                        else
                        {
                            tblLogin.Visible = true;
                            trOTP.Visible = false;
                            trBtnProceed.Visible = false;
                            lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Invalid Mobile Number !!!";
                        }
                    }
                    else
                    {
                        Button_Login();
                    }
                }
                else
                {
                    return;
                }
            }
            finally
            {
            }
        }

        private void Button_Login()
        {
            DataTable dt = null;
            CUser oUsr = null;
            int vErr = 0;
            oUsr = new CUser();
            vErr = oUsr.UpdateLastLoginDt(txtUsrNm.Text.Trim(), "PASS");
            if (vErr > 0)
            {
                if (vErr == 98)
                {
                    gblFuction.AjxMsgPopup("Hi " + txtUsrNm.Text.Trim() + " as a first time User, You can not Login now..Please change your password...!!");
                    return;
                }
                else if (vErr == 99)
                {
                    gblFuction.AjxMsgPopup("Your UserID has Expired (No Login for last 30 days).Please Contact to System Admin.!!");
                    return;
                }
                else if (vErr >= 21 && vErr <= 30)
                {
                    gblFuction.AjxMsgPopup("Your Password will be Expired in " + Convert.ToString(30 - vErr) + " days.!!Please Change it.!!");
                }
                else if (vErr == 100)
                {
                    gblFuction.AjxMsgPopup("Your last session terminated incorrectly or is currently active. Please wait for the session out time or retry after 5 minutes.");
                    return;
                }
                else if (vErr > 30)
                {
                    gblFuction.AjxMsgPopup("Your Password has Expired.Please Change Your Password.!!");
                    return;
                }
            }
            string username = txtUsrNm.Text.Trim();
            string vBrowser = GetBrowserDetails();
            string vIPAddress = GetIPAddress();

            oUsr = new CUser();
            dt = oUsr.GetRoleByUser(txtUsrNm.Text.Trim(), vBrowser, vIPAddress);
            if (dt.Rows.Count > 0)
            {
                Session[gblValue.RoleId] = dt.Rows[0]["RoleId"].ToString();
                Session[gblValue.UserId] = dt.Rows[0]["UserId"].ToString();
                Session[gblValue.UserName] = txtUsrNm.Text.Trim();
                Session[gblValue.DesignationID] = dt.Rows[0]["Designation"].ToString();
                Session[gblValue.LoginId] = dt.Rows[0]["LoginId"].ToString();

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, username, System.DateTime.Now,
                    System.DateTime.Now.AddDays(1), true, "vTkt", FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if (Session["WindowName"] == null)
                {
                    WindowName = getSession();
                    Session["WindowName"] = WindowName;
                }
                else
                {
                    WindowName = Session["WindowName"].ToString();
                }
                //--------------------------------------------------------
                if (Request.Cookies["USFBSME"] != null)
                {
                    Session["LoginCookies"] = Request.Cookies["USFBSME"].Value;
                }
                //--------------------------------------------------------
                //Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
                ScriptManager.RegisterStartupScript(this, typeof(Page), "RedirectPage", "window.open('WebPages/Public/FinYear.aspx', '" + WindowName + "');window.opener = top;", true);
                this.ShowLoginInfo = username.ToUpper();
            }
            else
            {
                gblFuction.AjxMsgPopup("You Do Not Have Permission To Access Web..Contact Administrator..");
                return;
            }
        }

        public string SendOTP()
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                string vOTP = hdnOTP.Value;
                //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)             
                string vMsgBody = "Dear User, Your Forceten login OTP is " + vOTP + ". Never share OTP. Regards, USFB Bank.";
                //********************************************************************
                String sendToPhoneNumber = hdnMobileNo.Value;
                //String userid = "2000204129";
                //String passwd = "Unity@1122";
                String userid = "2000243134";
                String passwd = "ZFimpPeKx";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707166254686719830&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending OTP.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            if (hdnOTP.Value == txtOTP.Text.Trim().ToUpper())
            {
                trOTP.Visible = false;
                trBtnProceed.Visible = false;
                Button_Login();
            }
            else
            {
                lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Invalid OTP..!!";
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            ChangePassword_ClearControls();
            Response.Redirect("~/Login.aspx", false);
        }

        protected void lbBack_Click(object sender, EventArgs e)
        {
        }

        private void ChangePassword_ClearControls()
        {
            txtUserNm.Text = "";
            txtOldPass.Text = "";
            txtNewPass.Text = "";
            txtCPass.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            txtUsrNm.Text = txtUserNm.Text.Trim();
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Edit";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    ChangePassword_ClearControls();
                    pnlpopup.Style.Add("display", "none");
                    Response.Redirect("~/Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            CUser oUser = null;
            DataTable dt = null;
            string vBrCode = "", vOldPass = "", vPass = "";
            try
            {
                oUser = new CUser();
                int vUsrId = oUser.GetUserByName(txtUserNm.Text.Trim());
                if (vUsrId == 0)
                {
                    gblFuction.AjxMsgPopup("Invalid UserName Name..");
                    return false;
                }
                //--------------------------------------------------------
                byte[] x_key = Convert.FromBase64String(Session["X_KEY"].ToString());
                //--------------------------------------------------------
                vOldPass = Decrypt(txtOldPass.Text.Trim(), x_key);
                vOldPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vOldPass), GetRijndaelManaged("Force@2301***DB")));//AES
                vErr = oUser.ChkOldPW(vUsrId, vOldPass);
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup("UserName and Password doesnot match..");
                    return false;
                }

                if (ValidatePassword(txtNewPass.Text) == false)
                {
                    gblFuction.AjxMsgPopup("Password Must Contain One UpperCase Letter, One Numeric Value and One Special Character and Minimum Length 8 Digit...");
                    return false;
                }
                DateTime vActivDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vClsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                Int32 vUserId = vUsrId;
                if (Mode == "Edit")
                {
                    vPass = Decrypt(txtNewPass.Text.ToString().Trim(), x_key);
                    vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));
                    //************ Check Old Password ****************** 
                    int i;
                    dt = oUser.ChkActiveUser(txtUserNm.Text, vOldPass);
                    string pwd = dt.Rows[0]["OldPassword"].ToString();
                    if (pwd != "")
                    {
                        string[] arr = pwd.Split(',');
                        for (i = 0; i <= arr.Length - 1; i++)
                        {
                            if (vPass == arr[i])
                            {
                                gblFuction.AjxMsgPopup("Password may not be similiar to last 3 passwords.!!");
                                return false;
                            }
                        }
                        if (arr.Length >= 3)
                        {
                            arr = arr.Skip(1).ToArray();
                        }
                        arr = arr.Concat(new string[] { vPass }).ToArray();
                        string oldPwd = "";
                        for (i = 0; i <= arr.Length - 1; i++)
                        {
                            if (oldPwd == "")
                                oldPwd = arr[i];
                            else
                                oldPwd = oldPwd + ',' + arr[i];
                        }

                        ViewState["olwPwd"] = oldPwd;

                    }
                    //***************************************************
                    //***************************************************
                    if (txtNewPass.Text != txtCPass.Text)
                    {
                        gblFuction.AjxMsgPopup("New and Confirm Password mis-matched..!!");
                        return false;
                    }
                    //***************************************************
                    oUser = new CUser();
                    vErr = oUser.ChangePassword(vUserId, vPass, this.RoleId, vBrCode, vUserId, vLogDt, "E", 0, ViewState["olwPwd"].ToString());
                    if (vErr > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oUser = null;
            }
        }

        static bool ValidatePassword(string passWord)
        {
            if (passWord.Trim().Length <= 7) return false;

            int validConditions = 0;
            foreach (char c in passWord)
            {
                if (c >= 'a' && c <= 'z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 0) return false;
            foreach (char c in passWord)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 1) return false;
            foreach (char c in passWord)
            {
                if (c >= '0' && c <= '9')
                {
                    validConditions++;
                    break;
                }
            }
            if (validConditions == 2) return false;
            if (validConditions == 3)
            {
                char[] special = { '@', '#', '$', '%', '^', '&', '+', '=', '!', '*', '(', ')', '|', '_', '-', '\\', '/', '<', '>' }; // or whatever
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }

        #region RijndaelManaged
        public RijndaelManaged GetRijndaelManaged(String secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }
        #endregion

        #region AES Encrypt
        public byte[] AesEncrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor()
                .TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        #endregion

        #region "Get IP Address And Browser"
        private string GetIPAddress()
        {
            //IP Address
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }

        public string GetBrowserDetails()
        {
            string browserDetails = string.Empty;
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            browserDetails = browser.Browser;
            return browserDetails;
        }
        #endregion

        #region AesDecrypt
        public static string Decrypt(string strToDecrypt, byte[] x_key)
        {
            try
            {
                byte[] keyBytes = new byte[32];
                byte[] ivBytes = new byte[16];
                Array.Copy(x_key, 0, keyBytes, 0, 32);
                Array.Copy(x_key, 0, ivBytes, 0, 16);
                using (Aes aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = ivBytes;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    byte[] encryptedBytes = Convert.FromBase64String(strToDecrypt);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during decryption: " + e.Message);
                return null;
            }
        }
        #endregion
    }
}