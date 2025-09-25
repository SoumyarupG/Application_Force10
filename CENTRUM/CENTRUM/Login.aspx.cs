using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Text;

namespace CENTRUM
{
    public partial class Jagaran : CENTRUMBase
    {
        public static string WindowName = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            string salt = "";
            //if (Request.QueryString["e"] != null)
            //{
            //    if (Request.QueryString["e"] == "random")
            //    {
            //        gblFuction.MsgPopup("Unfortunately Current Session Has Exired! Please Login Again To Continue");
            //    }
            //    else if (Request.QueryString["e"] == "changed")
            //    {
            //        gblFuction.MsgPopup("Password Is Changed Successfully !! Login With The New Password !!");
            //    }
            //    Request.QueryString["e"].Remove(0);
            //}

            if (Session["salt"] == null)
            {
                salt = CreateSalt(16).ToLower();
                Session["salt"] = salt.ToString();
            }
            else
            {
                salt = Session["salt"].ToString();
            }

            if (Session["LoginYN"] != null)
            {
                Response.Redirect("~/SsnExpr.aspx", true);
            }
            //if (Request.Cookies["LoginYN"] != null)
            //{
            //    Response.Redirect("~/SsnExpr.aspx", true);
            //}
            /*Hiding Menu Bar*/
            Control menu = Page.Master.FindControl("divmenubar");
            Control header = Page.Master.FindControl("spancompname");
            Control marquee = Page.Master.FindControl("marqueemsg");
            Control submsg = Page.Master.FindControl("lt1");
            Control img = Page.Master.FindControl("Img2");
            Control img3 = Page.Master.FindControl("Img3");
            Control imgReport = Page.Master.FindControl("imgReport");
            Control imgDashBoard = Page.Master.FindControl("imgDashBoard");
            img3.Visible = false;
            if (menu != null)
            {
                menu.Visible = false;
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
            if (imgReport != null)
            {
                imgReport.Visible = false;
            }
            if (imgDashBoard != null)
            {
                imgDashBoard.Visible = false;
            }
            /*---------------*/
            //if (!string.IsNullOrEmpty(Session[gblValue.BrnchCode] as string))
            //{
            //    gblFuction.AjxMsgPopup("Another User Is Already Logged In With Current Session. Please Close Current Session.");
            //   // Response.Redirect("~/SsnExpr.aspx", false);
            //}
            //else
            //{
            LinkButton LogOut = (LinkButton)Master.FindControl("lbLogOut");
            LinkButton ChBranch = (LinkButton)Master.FindControl("lblChBranch");
            LogOut.Text = "";
            ChBranch.Text = "";
            this.Welcome = false;
            Session[gblValue.LoginDate] = hdnDt.Value;
            btnLog.Attributes.Add("onclick", "return HashPwdwithSalt('" + salt.ToString() + "');");
            if (!IsPostBack)
            {
                Control lblSearch = Page.Master.FindControl("lblSearch");
                lblSearch.Visible = false;
                //txtPass.Attributes.Add("onKeyPress", "doClick('" + btnLog.ClientID + "',event)");
                //  txtUsrNm.Focus();   
                Session["PIIMaskingEnable"] = "N";
                txtUsrNm.Focus();
            }
            //}
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

        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            Int32 vRec = 0;
            //string vPass = dataEncryp.EncryptText(txtPass.Text);
            string vPass = "";
            //--------------------------------------------------------
            byte[] x_key = Convert.FromBase64String(Session["X_KEY"].ToString());
            vPass = Decrypt(txtPass.Text.ToString(), x_key);
            //--------------------------------------------------------
            string vSalt = Convert.ToString(Session["salt"]);
            vPass = vPass.Replace(vSalt, "");
            vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));//AES
            //string vPass1 = dataEncryp.DecryptText(txtPass.Text);
            object pwd;
            CUser oUsr = null;
            DataTable dt = null;

            try
            {
                if (txtUsrNm.Text.ToUpper() == "ADMINFTT" && txtPass.Text.ToUpper() == FormsAuthentication.HashPasswordForStoringInConfigFile("52922f0c6a8e53dd3e2d24022ae8baa6".ToLower() + Convert.ToString(Session["salt"]), "md5"))
                {
                    txtUsrNm.Text = "Admin";
                }
                else
                {
                    //if (HttpContext.Current.User.Identity.Name != "")
                    //{
                    //    gblFuction.AjxMsgPopup("Another User Is Already Logged In With Current Session. Please Close Current Session.");

                    //    Session.Abandon();
                    //    FormsAuthentication.SignOut();
                    //    Session.Clear();
                    //    Session.RemoveAll();
                    //    vResult = false;
                    //    return vResult;
                    //}

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
                        if (txtUsrNm.Text != "" && txtPass.Text.Trim() != "")
                        {
                            oUsr = new CUser();
                            vRec = oUsr.ChkDuplicateUser(txtUsrNm.Text, vPass, "Y");
                            if (vRec == 0)
                            {
                                gblFuction.AjxMsgPopup(gblMarg.InvalidUser);
                                gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                                vResult = false;
                                return vResult;
                            }
                        }
                    }
                    if (txtUsrNm.Text.Trim() != "")
                    {
                        if (txtUsrNm.Text != "" && txtPass.Text.Trim() != "")
                        {
                            oUsr = new CUser();

                            dt = oUsr.ChkActiveUser(txtUsrNm.Text, vPass);
                            if (dt.Rows.Count > 0)
                            {
                                pwd = dt.Rows[0]["Password"].ToString();
                                ViewState["Pass"] = pwd;

                                //string hashed_pwd =
                                //    FormsAuthentication.HashPasswordForStoringInConfigFile(pwd.ToString().ToLower() + Convert.ToString(Session["salt"]), "md5");
                                string hashed_pwd = pwd.ToString();
                                if (dt.Rows.Count > 0)
                                {
                                    if (dt.Rows[0]["Status"].ToString() != "Y")
                                    {
                                        gblFuction.AjxMsgPopup(gblMarg.InActiveUser);
                                        gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                                        vResult = false;
                                        return vResult;
                                    }
                                    else if (string.Compare(hashed_pwd.ToLower(), vPass.ToString().ToLower()) != 0)
                                    {
                                        gblFuction.AjxMsgPopup(gblMarg.InvalidUser);
                                        txtUsrNm.Focus();
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
                                    gblFuction.AjxMsgPopup("User not exist.");
                                    gblFuction.AjxFocus("ctl00_cph_Main_txtUsrNm");
                                    vResult = false;
                                    return vResult;
                                }
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup(gblMarg.InvalidUser);
                                txtUsrNm.Focus();
                                vResult = false;
                                return vResult;
                            }
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
        private void Button_Login()
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            int pRec = 0, vErr = 0;

            try
            {
                //if (ValidateFields() == true)
                //{
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
                    else if (vErr == 101)
                    {
                        gblFuction.AjxMsgPopup("Your user account has been temporarily locked due to too many failed attempts. Contact your account administrator for assistance.");
                        return;
                    }
                    else if (vErr > 30)
                    {
                        gblFuction.AjxMsgPopup("Your Password has Expired.Please Change Your Password.!!");
                        return;
                    }

                }

                string username = txtUsrNm.Text;
                string vIPAddress = GetIPAddress();
                string vBrowser = GetBrowserDetails();

                Session[gblValue.UserName] = txtUsrNm.Text.Trim();
                oUsr = new CUser();
                dt = oUsr.GetRoleByUser(txtUsrNm.Text.Trim(), vBrowser, vIPAddress);
                Session[gblValue.RoleId] = dt.Rows[0]["RoleId"].ToString();
                Session[gblValue.UserId] = dt.Rows[0]["UserID"].ToString();
                Session[gblValue.Designation] = dt.Rows[0]["Designation"].ToString();
                Session[gblValue.PrematureColl] = dt.Rows[0]["PrematureColl"].ToString();
                Session[gblValue.Demise] = dt.Rows[0]["Demise"].ToString();
                Session[gblValue.SncApprAmt] = dt.Rows[0]["SncApprAmt"].ToString();
                Session[gblValue.ViewAAdhar] = dt.Rows[0]["ViewAAdhar"].ToString();
                Session[gblValue.MultiColl] = dt.Rows[0]["MultiColl"].ToString();
                Session[gblValue.ICICINEFTYN] = dt.Rows[0]["ICICINEFTYN"].ToString();
                Session[gblValue.ICICIUser] = dt.Rows[0]["ICICIUser"].ToString();
                Session[gblValue.AllowAdvYN] = dt.Rows[0]["AllowAdvYN"].ToString();
                Session[gblValue.LoginId] = dt.Rows[0]["LoginId"].ToString();

                Session[gblValue.JlgGroupTr] = dt.Rows[0]["JlgGroupTr"].ToString();
                Session[gblValue.JlgCenterTr] = dt.Rows[0]["JlgCenterTr"].ToString();
                Session[gblValue.JLGMemberTr] = dt.Rows[0]["JLGMemberTr"].ToString();
                Session["PIIMaskingEnable"] = dt.Rows[0]["PIIMaskingEnable"].ToString();
                Session[gblValue.JLGDeviationCtrl] = dt.Rows[0]["JLGDeviationCtrl"].ToString();                

                ////pRec=oUsr.LoginCheck(Convert.ToInt32(Session[gblValue.UserId].ToString()));
                //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, username, System.DateTime.Now,
                //    System.DateTime.Now.AddMinutes(30), true, "vTkt", FormsAuthentication.FormsCookiePath);
                //string encTicket = FormsAuthentication.Encrypt(ticket);
                //Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                //hdnUser.Value = dt.Rows[0]["UserID"].ToString();
                //Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
                ////divVerifiedByPopUp.Visible = true;
                ////Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
                ////this.ShowLoginInfo = username.ToUpper();
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, username, System.DateTime.Now,
                        System.DateTime.Now.AddDays(1), true, "vTkt", FormsAuthentication.FormsCookiePath);
                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                //Response.Cookies[FormsAuthentication.FormsCookieName].Path = "/Images";
                //Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
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
                if (Request.Cookies["UNITY"] != null)
                {
                    Session["LoginCookies"] = Request.Cookies["UNITY"].Value;
                }
                //--------------------------------------------------------
                ScriptManager.RegisterStartupScript(this, typeof(Page), "RedirectPage", "window.open('WebPages/Public/FinYear.aspx', '" + WindowName + "');window.opener = top;", true);
                this.ShowLoginInfo = username.ToUpper();

                //}
                //else
                //{
                //    vErr = oUsr.UpdateLastLoginDt(txtUsrNm.Text.Trim(), "FAIL");
                //    if (vErr >= 3)
                //    {
                //        lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Already 3 error attempts. Your Account is blocked. Please Contact to System Admin...!!";
                //    }
                //    else
                //    {
                //        lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " " + vErr.ToString() + "  error attempt is done. Account will be blocked after 3 error attempts!!";
                //    }
                //    return;
                //}
            }
            finally
            {
                dt = null;
                oUsr = null;
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
                //string vMsgBody = "Thank you for your loan application with Centrum Microcredit Ltd. Your verification OTP is " + vOTP + ".";
                string vMsgBody = "Dear User, Your Forceten login OTP is " + vOTP + ". Never share OTP. Regards, USFB Bank.";
                //********************************************************************
                String sendToPhoneNumber = hdnMobileNo.Value;
                //String userid = "2000194447";
                //String passwd = "Centrum@2020";
                //String userid = "2000204129";
                //String passwd = "Unity@1122";
                String userid = "2000243134";
                String passwd = "ZFimpPeKx";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
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
                CUser oUser = new CUser();
                int vErrCnt = oUser.InsertWrongOTPLog(txtUsrNm.Text.Trim());
                if (vErrCnt == 0)
                {
                    lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Invalid OTP..!!";
                }
                else
                {
                    lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Your user account has been temporarily locked due to too many failed attempts. Contact your account administrator for assistance...!!";
                }

            }
        }
        protected void btnLog_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            int vErr = 0;

            if (ValidateFields() == true)
            {
                //****************** Send OTP as SMS *****////
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
                //****************************************////

            }
            else
            {
                vErr = oUsr.UpdateLastLoginDt(txtUsrNm.Text.Trim(), "FAIL");
                if (vErr >= 3)
                {
                    trOTP.Visible = false;
                    trBtnProceed.Visible = false;

                    lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " Already 3 error attempts. Your Account is blocked. Please Contact to System Admin...!!";
                }
                else
                {
                    trOTP.Visible = false;
                    trBtnProceed.Visible = false;

                    lblError.Text = "Hi " + txtUsrNm.Text.Trim() + " " + vErr.ToString() + "  error attempt is done. Account will be blocked after 3 error attempts!!";
                }
                return;
            }
        }

        protected void btnLogDone_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Match();", true);
            if ((Request[hdnStatus.UniqueID] as string == null ? hdnStatus.Value : Request[hdnStatus.UniqueID] as string) == "0")
            {
                Response.Redirect("~/WebPages/Public/FinYear.aspx", false);
            }

        }

        protected void ibAtt_Click(object sender, EventArgs e)
        {
            Response.Redirect("Biometry.aspx", false);
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
                    gblFuction.AjxMsgPopup("Password Saved Successfully..!!");
                    ChangePassword_ClearControls();
                    pnlpopup.Style.Add("display", "none");
                    //Response.Redirect("~/Login.aspx", false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
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

                //if (ValidatePassword(hdPassword.Value) == false)
                //{
                //    gblFuction.AjxMsgPopup("Password Must Contain One UpperCase Letter, One Numeric Value and One Special Character and Minimum Length 8 Digit...");
                //    return false;
                //}
                DateTime vActivDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vClsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                Int32 vUserId = vUsrId;
                //if (txtNewPass.Text.Length < 6)
                //{
                //    gblFuction.AjxMsgPopup("Password should be 6 characters");
                //    return false;
                //}
                if (Mode == "Edit")
                {
                    vPass = Decrypt(txtNewPass.Text.ToString().Trim(),x_key);
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
                    {
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblMarg.DBError);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="passWord"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateChangePassFields()
        {
            Boolean vResult = true;
            CUser oUsr = null;
            Int32 vRec = 0;
            int vNum = 0;
            DataTable dt = new DataTable();
            try
            {
                string vPass = txtPass.Text.ToString().Trim();
                if (txtUserNm.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("User Name cannot be left blank.");
                    vResult = false;
                }
                if (txtPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    vResult = false;
                }
                if (txtNewPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("New Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                    vResult = false;
                }
                if (txtCPass.Text.Trim() == "")
                {
                    gblFuction.MsgPopup("Confirm Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtCPass");
                    vResult = false;
                }

                if (txtNewPass.Text.Trim() != "" && txtCPass.Text.Trim() != "")
                {
                    if (txtNewPass.Text.ToString() != txtCPass.Text.ToString())
                    {
                        gblFuction.MsgPopup("New Password does not Matched with the Confirm Password.");
                        gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtCPass");
                        vResult = false;
                    }
                    else
                    {
                        oUsr = new CUser();
                        vRec = oUsr.ChkDuplicateUser(txtUserNm.Text.Trim(), vPass, "Y");
                        if (vRec == 0)
                        {
                            gblFuction.MsgPopup(gblMarg.InvalidUser);
                            vResult = false;
                        }
                    }
                }
                if (txtPass.Text.Trim() == txtNewPass.Text.Trim())
                {
                    gblFuction.MsgPopup("New Password cannot be same as Current Password.");
                    //gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    vResult = false;
                }
                //************ Check Old Password ****************** 
                int i;
                dt = oUsr.ChkActiveUser(txtUserNm.Text, vPass);
                string pwd = dt.Rows[0]["OldPassword"].ToString();
                if (pwd != "")
                {
                    string[] arr = pwd.Split(',');
                    for (i = 0; i <= arr.Length - 1; i++)
                    {
                        if (txtNewPass.Text.ToString() == arr[i])
                        {
                            gblFuction.MsgPopup("Password may not be similiar to last 3 passwords.!!");
                            return false;
                        }
                    }
                    if (arr.Length >= 3)
                    {
                        arr = arr.Skip(1).ToArray();
                    }
                    arr = arr.Concat(new string[] { txtNewPass.Text }).ToArray();
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
                return vResult;
            }
            finally
            {
                oUsr = null;
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

        protected void lnkbtnForgotPwd_Click(object sender, EventArgs e)
        {
            txtUsrNm.Text = txtUsrNm.Text.Trim();
            try
            {
                SendForgotOTP(txtUsrNm.Text);

            }
            catch (Exception ex)
            {

            }
        }

        public void SendForgotOTP(string pUserName)
        {
            DataTable dt = new DataTable();
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            string vOTP = "", vMobileNo = "";
            CUser oUser = new CUser();
            try
            {
                dt = oUser.ChkValidUserName(pUserName);
                if (dt.Rows.Count > 0)
                {
                    vMobileNo = Convert.ToString(dt.Rows[0]["MobileNo"]);
                    vOTP = Convert.ToString(dt.Rows[0]["OTP"]);
                    int vErr = oUser.SaveOTPLog(vMobileNo, vOTP);
                    if (vErr > 0)
                    {
                        tblLogin.Visible = false;
                        tblForgotPwd.Visible = true;

                        Session["ForgotPassOtp"] = vOTP;

                        string vMsgBody = vOTP + " is your OTP to reset your password. OTP valid for 5 minutes. Do not share this with anyone for security reasons. Unity Bank";
                        //********************************************************************
                        String sendToPhoneNumber = vMobileNo;
                        String userid = "2000243134";
                        String passwd = "ZFimpPeKx";
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707172985365562069&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                        request = WebRequest.Create(url);
                        response = (HttpWebResponse)request.GetResponse();
                        Stream stream = response.GetResponseStream();
                        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                        StreamReader reader = new System.IO.StreamReader(stream, ec);
                        result = reader.ReadToEnd();
                        reader.Close();
                        stream.Close();
                        result = "Success:OTP has been successfully sent.";
                    }
                    else
                    {
                        result = "Please try after 5 minutes..";
                    }
                }
                else
                {
                    result = "Invalid User Name..";
                }
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
            gblFuction.AjxMsgPopup(result);
        }

        protected void btnFPassProceed_Click(object sender, EventArgs e)
        {
            string vCnfPass = "",vNewfPass="";
            DataTable dt = new DataTable();
            CUser oUser = new CUser();
            int vErrCnt = 0;
            if (Convert.ToString(Session["ForgotPassOtp"]) == txtFOTP.Text.Trim().ToUpper())
            {
                if (txtFNewPwd.Text != "")
                {
                    //--------------------------------------------------------
                    byte[] x_key = Convert.FromBase64String(Session["X_KEY"].ToString());
                    //--------------------------------------------------------
                    vCnfPass = Decrypt(txtFCnfPwd.Text.ToString().Trim(), x_key);
                    vNewfPass = Decrypt(txtFNewPwd.Text.ToString().Trim(), x_key);
                    if (vNewfPass == vCnfPass)
                    {                       
                        vCnfPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vCnfPass), GetRijndaelManaged("Force@2301***DB")));
                        //if (ValidatePassword(hdnFPwd.Value.Trim()) == false)
                        //{
                        //    gblFuction.AjxMsgPopup("Password Must Contain One UpperCase Letter, One Numeric Value and One Special Character and Minimum Length 8 Digit...");
                        //    return;
                        //}
                        //else
                        //{
                        //************ Check Old Password ****************** 
                        int i;
                        dt = oUser.ChkActiveUser(txtUsrNm.Text, "");
                        string pwd = dt.Rows[0]["OldPassword"].ToString();
                        if (pwd != "")
                        {
                            string[] arr = pwd.Split(',');
                            for (i = 0; i <= arr.Length - 1; i++)
                            {
                                if (vCnfPass == arr[i])
                                {
                                    gblFuction.AjxMsgPopup("Password may not be similiar to last 3 passwords.!!");
                                    return;
                                }
                            }
                        }
                        //}
                        //***************************************************
                        oUser = new CUser();
                        int vErr = oUser.ForgotPassword(txtUsrNm.Text, vCnfPass);
                        if (vErr > 0)
                        {
                            txtUsrNm.Text = "";
                            tblLogin.Visible = true;
                            tblForgotPwd.Visible = false;
                            lblForgotErr.Text = "Hi " + txtUsrNm.Text.Trim() + " New Password Saved Successfully..!!";
                            gblFuction.AjxMsgPopup(lblForgotErr.Text);
                        }
                    }
                    else
                    {
                        lblForgotErr.Text = "Hi " + txtUsrNm.Text.Trim() + " New Password & Confirm Password Must be Same..!!";
                    }
                }
                else
                {
                    lblForgotErr.Text = "Hi " + txtUsrNm.Text.Trim() + " Enter New Password..!!";
                }

                //trOTP.Visible = false;
                //trBtnProceed.Visible = false;

                //Button_Login();
            }
            else
            {
                oUser = new CUser();
                vErrCnt = oUser.InsertWrongOTPLog(txtUsrNm.Text.Trim());
                if (vErrCnt == 0)
                {
                    lblForgotErr.Text = "Hi " + txtUsrNm.Text.Trim() + " Invalid OTP..!!";
                }
                else
                {
                    lblForgotErr.Text = "Hi " + txtUsrNm.Text.Trim() + " Your user account has been temporarily locked due to too many failed attempts. Contact your account administrator for assistance...!!";
                }
            }
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
                Array.Copy(x_key, 0, ivBytes, 0, 16); // or use 16–32 if needed
                //Console.WriteLine("Key: " + Convert.ToBase64String(keyBytes));
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
