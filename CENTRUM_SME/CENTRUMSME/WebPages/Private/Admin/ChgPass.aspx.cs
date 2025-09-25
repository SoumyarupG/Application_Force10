using System;
using System.Data;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Admin
{
    public partial class ChgPass : CENTRUMBAse
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
                GetUserById();
                tabUsers.ActiveTabIndex = 0;
                StatusButton("View");
                btnSave.Attributes.Add("onclick", "return GeneratePwd();");        
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
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
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
            try
            {
                string vPass = txtPass.Text.ToString().Trim();
                if (txtUserNm.Text.Trim() == "")
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("User Name cannot be left blank.");
                    vResult = false;
                }
                if (txtPass.Text.Trim()  == "")
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
                if (txtPass.Text.Trim() == hdPassword.Value.ToString())
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("New Password cannot be same as New Password.");
                    gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                    vResult = false;
                }
                if (txtNewPass.Text.Trim() != "" && txtCPass.Text.Trim() != "")
                {
                    if (hdPassword.Value.ToString() != hdCPassword.Value.ToString())
                    //if (txtNewPass.Text.ToString() != txtCPass.Text.ToString())
                    {
                        EnableControl(true);
                        gblFuction.MsgPopup("New Password does not Matched with the Confirm Password.");
                        gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtCPass");
                        vResult = false;
                    }
                    else
                    {
                        oUsr = new CUser();
                        vRec = oUsr.ChkDuplicateUser(txtUserNm.Text.Trim(), vPass);
                        if (vRec == 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.InvalidUser);
                            EnableControl(true);
                            vResult = false;
                        }
                    }
                }               
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
            hdPassword.Value = "";
            hdCPassword.Value = "";
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
                    txtPass.Text = dt.Rows[0]["Password"].ToString();
                    //txtPass.Attributes.Add("value", dataEncryp.DecryptText(dt.Rows[0]["Password"].ToString()));
                    txtUserNm.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                    tabUsers.ActiveTabIndex = 1;
                    StatusButton("Show");
                    btnSave.Attributes.Add("onclick", "return GeneratePwd();");  
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
            string vBrCode = "";
            try
            {

                if (ValidatePassword(hdPassword.Value) == false)
                {
                    gblFuction.MsgPopup("Password Must Contain One UpperCase Letter, One Numeric Value and One Special Character and Minimum Length 8 Digit...");
                    return false;
                }
                
                DateTime vActivDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vClsDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();    
                string vPass = "", vOrgPass="";
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 vUserId = Convert.ToInt32(ViewState["UserId"]);
                if (Mode == "Edit")
                {
                    if (ValidateFields() == false)
                    {
                        txtPass.Text = "";
                        txtNewPass.Text = "";
                        txtCPass.Text = "";
                        return false;
                    }
                    vPass = txtNewPass.Text.ToString().Trim();
                    vOrgPass = (Request[hdPassword.UniqueID] as string == null) ? hdPassword.Value : Request[hdPassword.UniqueID] as string;
                   // vOrgPass = hdPassword.Value.ToString();
                    //vPass = dataEncryp.EncryptText(txtNewPass.Text);
                    oUser = new CUser();
                    vErr = oUser.ChangePassword(vUserId, vPass,vOrgPass);
                    if (vErr > 0)
                    {
                        txtPass.Text = "";
                        txtNewPass.Text = "";
                        txtCPass.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
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
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);                 
                    StatusButton("View");
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
            try
            {
                if (tabUsers.ActiveTabIndex == 0)
                {
                    EnableControl(false);
                    StatusButton("View");
                    ViewState["UserId"] = null;
                    ViewState["StateEdit"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
    }
}