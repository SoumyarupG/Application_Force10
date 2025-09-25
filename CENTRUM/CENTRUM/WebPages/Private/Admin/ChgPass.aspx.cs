using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using FORCECA;
using FORCEBA;
using System.Security.Cryptography;
using System.Text;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class ChgPass : CENTRUMBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["encrypted"] = null;
                GetUserById();
                btnSave.Attributes.Add("onclick", "return GeneratePwd();");
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                //tabUsers.ActiveTabIndex = 0;
                //StatusButton("View");
                // btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Change Password";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuChangePass);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            CUser oUsr = null;
            Int32 vRec = 0;
            int vNum = 0;
            DataTable dt = new DataTable();
            try
            {
                string vPass = txtPass.Text.ToString().Trim();
                vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));
                if (txtUserNm.Text.Trim() == "")
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("User Name cannot be left blank.");
                    vResult = false;
                }
                if (txtPass.Text.Trim() == "")
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    vResult = false;
                }
                if (txtNewPass.Text.Trim() == "")
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("New Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                    vResult = false;
                }
                if (txtCPass.Text.Trim() == "")
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Confirm Password cannot be left blank.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtCPass");
                    vResult = false;
                }
                //else if (txtNewPass.Text.Trim() != "")
                //{
                //    if (txtNewPass.Text.Length < 8)
                //    {
                //        EnableControl(true);
                //        gblFuction.MsgPopup("New Password length must be minimum 8 letters.");
                //        gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                //        vResult = false;
                //    }
                //    else if (txtNewPass.Text.Length >= 8)
                //    {
                //        char[] testarr = txtNewPass.Text.ToCharArray();
                //        int count = 0, count1 = 0;

                //        for (int i = 0; i < testarr.Length; i++)
                //        {
                //            if (char.IsUpper(testarr[i]))
                //            {
                //                count++;
                //            }
                //        }
                //        if (count == 0)
                //        {
                //            EnableControl(true);
                //            gblFuction.MsgPopup("New Password  must contain at least 1 upper case letter.");
                //            gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                //            vResult = false;
                //        }

                //        for (int i = 0; i < testarr.Length; i++)
                //        {
                //            if (!Char.IsLetterOrDigit(testarr[i]))
                //            {
                //                count1++;
                //            }
                //        }
                //        if (count1 == 0)
                //        {
                //            EnableControl(true);
                //            gblFuction.MsgPopup("New Password  must contain at least 1 special character.");
                //            gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                //            vResult = false;
                //        }

                //        count1 = 0;
                //        for (int i = 0; i < testarr.Length; i++)
                //        {
                            
                //            if(Int32.TryParse(testarr[i].ToString(),out vNum)== true)
                //            {
                //                count1++;
                //            }
                //        }
                //        if (count1 == 0)
                //        {
                //            EnableControl(true);
                //            gblFuction.MsgPopup("New Password  must contain at least 1 Number character.");
                //            gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtNewPass");
                //            vResult = false;
                //        }
                //    }
                //}
                if (txtNewPass.Text.Trim() != "" && txtCPass.Text.Trim() != "")
                {
                    if (txtNewPass.Text.ToString() != txtCPass.Text.ToString())
                    {
                        EnableControl(true);
                        gblFuction.MsgPopup("New Password does not Matched with the Confirm Password.");
                        gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtCPass");
                        vResult = false;
                    }
                    else
                    {
                        oUsr = new CUser();
                        vRec = oUsr.ChkDuplicateUser(txtUserNm.Text.Trim(), vPass,"Y");
                        if (vRec == 0)
                        {
                            gblFuction.MsgPopup(gblMarg.InvalidUser);
                            EnableControl(true);
                            vResult = false;
                        }
                    }
                }
                string vNewPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(txtNewPass.Text.Trim().ToString()), GetRijndaelManaged("Force@2301***DB")));
                if (vPass == vNewPass)
                {
                    gblFuction.MsgPopup("New Password cannot be same as Current Password.");
                    //gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    vResult = false;
                }
                //************ Check Old Password ****************** 
                int i;
                dt = oUsr.ChkActiveUser(txtUserNm.Text, vPass);
                string pwd = dt.Rows[0]["OldPassword"].ToString();
                //string vNewPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(txtNewPass.Text.ToString()), GetRijndaelManaged("Force@2301***DB")));
                if (pwd != "")
                {
                    string[] arr = pwd.Split(',');
                    for (i = 0; i <= arr.Length - 1; i++)
                    {
                        if (vNewPass == arr[i])
                        {
                            gblFuction.MsgPopup("Password may not be similiar to last 3 passwords.!!");
                            return false;
                        }
                    }
                    if (arr.Length >= 3)
                    {
                        arr = arr.Skip(1).ToArray();
                    }
                    arr = arr.Concat(new string[] { vNewPass }).ToArray();
                    string oldPwd="";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            if (ViewState["UserId"] == null || ViewState["UserId"].ToString() != "1")
            {
                txtUserNm.Enabled = false;
                txtPass.Enabled = Status;
                txtNewPass.Enabled = Status;
                txtCPass.Enabled = Status;
            }
            else if (ViewState["UserId"].ToString() == "1")
            {
                txtUserNm.Enabled = false;
                txtPass.Enabled = Status;
                txtNewPass.Enabled = Status;
                txtCPass.Enabled = Status;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtUserNm.Text = "";
            txtPass.Attributes.Add("value", "");
            txtNewPass.Attributes.Add("value", "");
            txtCPass.Attributes.Add("value", "");
            lblUser.Text = "";
            lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetUserById()
        {
            CUser oUser = null;
            DataTable dt = null;
            try
            {
                oUser = new CUser();
                dt = oUser.GetUserById(this.UserID);
                if (dt.Rows.Count > 0)
                {
                    ViewState["UserId"] = Convert.ToString(dt.Rows[0]["UserId"]);
                    ViewState["encrypted"] = Convert.ToString(dt.Rows[0]["Password"]);
                    //txtPass.Attributes.Add("value", dataEncryp.DecryptText(dt.Rows[0]["Password"].ToString()));
                    txtUserNm.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                    tabUsers.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
            }
            finally
            {
                oUser = null;
                dt = null;
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
            string vBrCode = "", vPass = "";
            try
            {
                if (txtPass.Text != Convert.ToString(ViewState["encrypted"]))
                {
                    gblFuction.MsgPopup("Current Password Is Incorrect !!");
                    txtPass.Text = "";
                    txtNewPass.Text = "";
                    txtCPass.Text = "";
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    return false;
                }
                if (ValidatePassword(hdPassword.Value) == false)
                {
                    gblFuction.MsgPopup("Password Must Contain One UpperCase Letter, One Numeric Value and One Special Character and Minimum Length 8 Digit...");
                    return false;
                }
                DateTime vActivDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vClsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 vUserId = Convert.ToInt32(ViewState["UserId"]);
                //if (txtNewPass.Text.Length < 6)
                //{
                //    gblFuction.AjxMsgPopup("Password should be 6 characters");
                //    return false;
                //}
                if (Mode == "Edit")
                {
                    //if (ValidateFields() == false)
                    //    return false;
                    if (ValidateFields() == false)
                    {
                        txtPass.Text = "";
                        txtNewPass.Text = "";
                        txtCPass.Text = "";
                        return false;
                    }

                    vPass = txtNewPass.Text.ToString().Trim();
                    vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));//AES Encryption
                    oUser = new CUser();
                    vErr = oUser.ChangePassword(vUserId, vPass, this.RoleId, vBrCode, this.UserID, vLogDt, "E", 0, ViewState["olwPwd"].ToString());
                    if (vErr > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                StatusButton("View");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabUsers.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                txtPass.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tabUsers_ActiveTabChanged(object sender, EventArgs e)
        {
            if (tabUsers.ActiveTabIndex == 0)
            {
                EnableControl(false);
                StatusButton("View");
                ViewState["UserId"] = null;
                ViewState["StateEdit"] = null;
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
    }
}