using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCECA;
using FORCEBA;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CENTRUM.Private.Webpages.Admin
{
    public partial class Users : CENTRUMBase
    {
        protected int vPgNo = 1;

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
                //PopBranch();
                PopRole();
                LoadGrid(0);
                tbUsr.ActiveTabIndex = 0;
                StatusButton("View");
                ViewState["password"] = null;
                popState();
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
                this.PageHeading = "User";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuUser);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "User Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void CheckBrAll()
        {
            Int32 vRow;
            string strin = "";
            if (rblAlSel.SelectedValue == "rbAll")
            {
                lstBranch.Enabled = false;
                for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                {
                    lstBranch.Items[vRow].Selected = true;
                    if (strin == "")
                    {
                        strin = lstBranch.Items[vRow].Value;
                    }
                    else
                    {
                        strin = strin + "," + lstBranch.Items[vRow].Value + "";
                    }
                }
                ViewState["BrCode"] = strin;
            }
            else if (rblAlSel.SelectedValue == "rbSel")
            {
                ViewState["BrCode"] = null;
                lstBranch.Enabled = true;
                for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                    lstBranch.Items[vRow].Selected = false;

            }
        }

        protected void rblAlSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBrAll();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;//vDistId = 0,
            string vBrCode = "";
            CUser oUser = null;
            try
            {
                //vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oUser = new CUser();
                dt = oUser.GetUserList(vBrCode, txtSearch.Text.Trim());
                gvUser.DataSource = dt;
                gvUser.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
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
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbUsr.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        private void PopRole()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetAssignedRoles();
                ddlRole.DataSource = dt;
                ddlRole.DataTextField = "Role";
                ddlRole.DataValueField = "RoleId";
                ddlRole.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlRole.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            string vBrCode = "0000"; //for All branch    
            DataTable dt = null;
            CBranch oBranch = null;
            try
            {
                oBranch = new CBranch();
                dt = oBranch.GetBranchList(vBrCode);
                lstBranch.DataSource = dt;
                lstBranch.DataValueField = "BranchCode";
                lstBranch.DataTextField = "BranchName";
                lstBranch.DataBind();

                if (rblAlSel.SelectedValue == "rbAll")
                {
                    lstBranch.Enabled = false;
                    for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                    {
                        lstBranch.Items[vRow].Selected = true;
                        if (strin == "")
                        {
                            strin = lstBranch.Items[vRow].Value;
                        }
                        else
                        {
                            strin = strin + "," + lstBranch.Items[vRow].Value + "";
                        }
                    }
                }
                else if (rblAlSel.SelectedValue == "rbSel")
                {
                    for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                    {
                        lstBranch.Items[vRow].Selected = false;
                    }
                }
                ViewState["BrCode"] = strin;
            }
            finally
            {
                dt = null;
                oBranch = null;
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            if (ViewState["UserId"] == null || ViewState["UserId"].ToString() != "1")
            {
                txtUserNm.Enabled = Status;
                txtPass.Enabled = Status;
                ddlRole.Enabled = Status;
                chkActive.Enabled = Status;
                chkHO.Enabled = Status;
                rblAlSel.Enabled = Status;
                lstBranch.Enabled = Status;
                chkMFYN.Enabled = Status;
            }
            else if (ViewState["UserId"].ToString() == "1")
            {
                txtUserNm.Enabled = Status;
                txtPass.Enabled = Status;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtUserNm.Text = "";
            txtPass.Attributes.Add("value", "");
            ddlRole.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
            lstBranch.SelectedIndex = -1;
            chkActive.Checked = false;
            chkHO.Checked = false;
            chkMFYN.Checked = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            int vNum = 0;

            try
            {
                if (txtPass.Text.Trim() != "")
                {
                    if (txtPass.Text.Length < 8)
                    {
                        EnableControl(true);
                        gblFuction.MsgPopup("Password length must be minimum 8 letters.");
                        gblFuction.focus("ctl00_cph_Main_tbUsr_pnlDtl_txtPass");
                        vResult = false;
                    }
                    else if (txtPass.Text.Length >= 8)
                    {
                        char[] testarr = txtPass.Text.ToCharArray();
                        int count = 0, count1 = 0;

                        for (int i = 0; i < testarr.Length; i++)
                        {
                            if (char.IsUpper(testarr[i]))
                            {
                                count++;
                            }
                        }
                        if (count == 0)
                        {
                            EnableControl(true);
                            gblFuction.MsgPopup("Password  must contain at least 1 upper case letter.");
                            gblFuction.focus("ctl00_cph_Main_tbUsr_pnlDtl_txtPass");
                            vResult = false;
                        }

                        for (int i = 0; i < testarr.Length; i++)
                        {
                            if (!Char.IsLetterOrDigit(testarr[i]))
                            {
                                count1++;
                            }
                        }
                        if (count1 == 0)
                        {
                            EnableControl(true);
                            gblFuction.MsgPopup("Password  must contain at least 1 special character.");
                            gblFuction.focus("ctl00_cph_Main_tbUsr_pnlDtl_txtPass");
                            vResult = false;
                        }

                        count1 = 0;
                        for (int i = 0; i < testarr.Length; i++)
                        {

                            if (Int32.TryParse(testarr[i].ToString(), out vNum) == true)
                            {
                                count1++;
                            }
                        }
                        if (count1 == 0)
                        {
                            EnableControl(true);
                            gblFuction.MsgPopup("Password  must contain at least 1 Number character.");
                            gblFuction.focus("ctl00_cph_Main_tbUsr_pnlDtl_txtPass");
                            vResult = false;
                        }
                    }
                }

                return vResult;
            }
            finally
            {

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
            Int32 dChk = 0, vRefId = 0, vRoleId = 0;
            DataTable dt1 = null;
            CUser oUser = null;
            string vStatus = "", vUsrTyp = "", vXmlData = "", vChkDuplicate = "", vSystem = "";
            string vMFAYN = chkMFYN.Checked == true ? "Y" : "N";

            #region PASSWORD INFORMATION
            /*
                * hdPassword.Value -  CONTAINS ACTUAL PASSWORD 
                * txtPass.Text - CONTAINS THE HASHED PASSWORD
            */
            #endregion
            txtPass.Attributes.Add("value", (ViewState["password"] == null) ? txtPass.Text : ViewState["password"].ToString()); //holds the password during postback
            try
            {
                if (Mode != "Delete")
                {
                    if (txtPass.Text != "")
                    {
                        if (ValidatePassword(hdPassword.Value) == false)
                        //if (ValidatePassword(txtPass.Text) == false)
                        {
                            gblFuction.MsgPopup("Password Minimum Length 8 Digit and Must Contain One UpperCase Letter, One Numeric Value and One Special Character...");
                            return false;
                        }
                    }
                }

                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vPass = txtPass.Text;
                if (vPass != "")
                {
                    vPass = Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(vPass), GetRijndaelManaged("Force@2301***DB")));//AES
                }
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 dUserId = Convert.ToInt32(ViewState["UserId"]);
                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                if (chkActive.Checked == true)
                    vStatus = "Y";
                else
                    vStatus = "N";
                if (chkHO.Checked == true)
                    vUsrTyp = "Y";
                else
                    vUsrTyp = "N";
                if (dUserId == 1)
                    vRoleId = 1;
                else
                    vRoleId = Convert.ToInt32(ddlRole.SelectedValue);

                //if (ValidateFields() == false)
                //    return false;
                ViewState["olwPwd"] = "";
                if (Mode == "Edit")
                {
                    if (vPass != "")
                    {
                        //************ Check Old Password ****************** 
                        int i;
                        string pwd = ViewState["OldPassword"].ToString(); //
                        if (pwd != "")
                        {
                            string[] arr = pwd.Split(',');
                            for (i = 0; i <= arr.Length - 1; i++)
                            {
                                if (vPass == arr[i])
                                {
                                    gblFuction.MsgPopup("Password may not be similiar to last 3 passwords.!!");
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
                    }
                }

                if (Mode == "Save")
                {
                    oUser = new CUser();
                    dt1 = oUser.GetUserDtl(this.UserID);
                    foreach (DataRow dR in dt1.Rows)
                    {
                        if (lstBranch.Items[lstBranch.Items.IndexOf(lstBranch.Items.FindByValue(dR["BranchCode"].ToString()))].Selected == true)
                        {
                            dR["UserID"] = 1;
                            dR["Allow"] = "Y";
                            dR["CreatedBy"] = this.UserID;
                            dR["CreationDateTime"] = System.DateTime.Now.Date;
                            dR["EntType"] = "I";
                            dR["SynStatus"] = "0";
                        }
                        else
                        {
                            dR.Delete();
                        }
                    }
                    dt1.AcceptChanges();
                    dt1.TableName = "Table1";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt1.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Save", ref vChkDuplicate);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("User can not be Duplicate..");
                        return false;
                    }
                    dChk = oUser.InsertUser(0, ref dUserId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId,
                        vLogDt, vStatus, vLogDt, this.UserID, "I", 0, vUsrTyp, vStatus, vXmlData, "Save", "N",
                        "", vMFAYN);
                    if (dChk > 0)
                    {
                        ViewState["UserId"] = dUserId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oUser = new CUser();
                    dt1 = oUser.GetUserDtl(dUserId);
                    foreach (DataRow dR in dt1.Rows)
                    {
                        if (lstBranch.Items[lstBranch.Items.IndexOf(lstBranch.Items.FindByValue(dR["BranchCode"].ToString()))].Selected == true)
                        {
                            dR["UserID"] = 1;
                            dR["Allow"] = "Y";
                            dR["CreatedBy"] = this.UserID;
                            dR["CreationDateTime"] = System.DateTime.Now.Date;
                            dR["EntType"] = "I";
                            dR["SynStatus"] = "0";
                        }
                        else
                        {
                            dR.Delete();
                        }
                    }
                    dt1.AcceptChanges();
                    dt1.TableName = "Table1";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt1.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }

                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Edit", ref vChkDuplicate);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("User can not be Duplicate..");
                        return false;
                    }
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System User Can not be Edited..");
                        return false;
                    }

                    /*CHECK WHETHER PASSWORD IS CHANGED OR NOT*/
                    string isPasswordChanged = "N";
                    string newpassword = vPass;
                    string oldpassword = (ViewState["OldPassword"] == null) ? vPass : ((ViewState["OldPassword"].ToString() == "") ? vPass : ViewState["OldPassword"].ToString());

                    string[] arrOldPwd = oldpassword.Split(',');
                    if (arrOldPwd[arrOldPwd.Length - 1].ToString() != newpassword)
                    {
                        isPasswordChanged = "Y";
                    }
                    else
                    {
                        isPasswordChanged = "N";
                    }
                    /******************************************/

                    dChk = oUser.InsertUser(dUserId, ref vRefId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId,
                        vLogDt, vStatus, vLogDt, this.UserID, "I", 0, vUsrTyp, vStatus, vXmlData, "Edit", isPasswordChanged, ViewState["olwPwd"].ToString(),vMFAYN);
                    if (dUserId > 0)
                    {
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oUser = new CUser();
                    dt1 = oUser.GetUserDtl(dUserId);
                    foreach (DataRow dR in dt1.Rows)
                    {
                        if (lstBranch.Items[lstBranch.Items.IndexOf(lstBranch.Items.FindByValue(dR["BranchCode"].ToString()))].Selected == true)
                        {
                            dR["UserID"] = 1;
                            dR["Allow"] = "Y";
                            dR["CreatedBy"] = this.UserID;
                            dR["CreationDateTime"] = System.DateTime.Now.Date;
                            dR["EntType"] = "I";
                            dR["SynStatus"] = "0";
                        }
                        else
                        {
                            dR.Delete();
                        }
                    }
                    dt1.AcceptChanges();
                    dt1.TableName = "Table1";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt1.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Edit", ref vChkDuplicate);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System User Can not be Deleted..");
                        return false;
                    }
                    oUser = new CUser();
                    if ((dChk > 0 && this.UserID == dUserId) || (dChk <= 0 && this.UserID == dUserId))
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = false;
                    }
                    else
                    {
                        dChk = oUser.InsertUser(dUserId, ref vRefId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId,
                            vLogDt, vStatus, vLogDt, this.UserID, "I", 0, vUsrTyp, vStatus, vXmlData, "Delet", "N", "",vMFAYN);
                        vResult = true;
                    }
                }
                return vResult;
            }
            finally
            {
                dt1 = null;
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
                LoadGrid(0);
                StatusButton("View");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vUserId = 0;
            DataTable dt = null, dt1 = null;
            CUser oUser = null;
            string vStatus = "", vHO = "";
            txtPass.Attributes.Add("value", "");
            try
            {
                vUserId = Convert.ToInt32(e.CommandArgument);
                ViewState["UserId"] = vUserId;

                if (e.CommandName == "cmdShow")
                {
                    oUser = new CUser();
                    dt = oUser.GetUserById(vUserId);
                    if (dt.Rows.Count > 0)
                    {
                        // txtPass.Attributes.Add("value", dataEncryp.DecryptText(dt.Rows[0]["Password"].ToString()));
                        //txtPass.Attributes.Add("value", Convert.ToString(dt.Rows[0]["Password"]));
                        ViewState["password"] = Convert.ToString(dt.Rows[0]["Password"]);
                        ViewState["OldPassword"] = Convert.ToString(dt.Rows[0]["OldPassword"]); /*HOLD OLD PASSWORD TO CHECK DURING EDIT WHETHER PASSWORD IS CHANGED OR NOT*/

                        txtUserNm.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(dt.Rows[0]["RoleId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        vStatus = dt.Rows[0]["Status"].ToString();
                        vHO = dt.Rows[0]["UserType"].ToString();
                        if (vStatus == "Y")
                            chkActive.Checked = true;
                        else if (vStatus == "N")
                            chkActive.Checked = false;
                        if (vHO == "Y")
                            chkHO.Checked = true;
                        else if (vHO == "N")
                            chkHO.Checked = false;
                        if (dt.Rows[0]["MFAYN"].ToString() == "Y")
                            chkMFYN.Checked = true;
                        else
                            chkMFYN.Checked = false;

                        dt1 = oUser.GetUserDtl(vUserId);
                        lstBranch.Items.Clear();
                        lstBranch.DataSource = dt1;
                        lstBranch.DataValueField = "BranchCode";
                        lstBranch.DataTextField = "BranchName";
                        lstBranch.DataBind();
                        foreach (DataRow dr in dt1.Rows)
                        {
                            if (dr["Allow"].ToString() == "Y")
                                lstBranch.Items[lstBranch.Items.IndexOf(lstBranch.Items.FindByValue(dr["BranchCode"].ToString()))].Selected = true;
                        }
                        tbUsr.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                dt1 = null;
                oUser = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbUsr.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtUserNm");
                StatusButton("Edit");
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanDelete == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    txtPass.Enabled = true;
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    StatusButton("Delete");
                    tbUsr.ActiveTabIndex = 0;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tbUsr.ActiveTabIndex = 1;
                StatusButton("Add");
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
                if (tbUsr.ActiveTabIndex == 0)
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

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }
        //protected void btnUpdate_Click(object sender, EventArgs e)
        //{
        //    Int32 vErr = 0;
        //    CUser oUser = null;
        //    string vPass = "";
        //    Int32 vUserId = Convert.ToInt32(ViewState["UserId"]);

        //    try
        //    {
        //        vPass = txtIsoTemplate.Text;
        //        oUser = new CUser();
        //        vErr = oUser.Update_BioImg(vUserId, vPass);
        //        if (vErr == 0)
        //            gblFuction.MsgPopup("Finger Image Capture");
        //        else if (vErr == 1)
        //        {
        //            gblFuction.MsgPopup(gblMarg.DBError);

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();                
                dt = oGb.GetAllStateList();
                ddlState.DataSource = dt;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlState.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popBranchByStateId()
        {
            DataTable dt = new DataTable();
            CUser oUsr = new CUser();          
            Int32 vRow;
            string strin = "";
            ViewState["BrCode"] = null;
            string pUser = Session[gblValue.UserName].ToString();
            dt = oUsr.GetBranchByState(pUser, Convert.ToInt32(Session[gblValue.RoleId]), ddlState.SelectedValue, "ALL");
            if (dt.Rows.Count > 0)
            {
                try
                {                   
                    lstBranch.DataSource = dt;
                    lstBranch.DataValueField = "BranchCode";
                    lstBranch.DataTextField = "BranchName";
                    lstBranch.DataBind();

                    if (rblAlSel.SelectedValue == "rbAll")
                    {
                        lstBranch.Enabled = false;
                        for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                        {
                            lstBranch.Items[vRow].Selected = true;
                            if (strin == "")
                            {
                                strin = lstBranch.Items[vRow].Value;
                            }
                            else
                            {
                                strin = strin + "," + lstBranch.Items[vRow].Value + "";
                            }
                        }
                    }
                    else if (rblAlSel.SelectedValue == "rbSel")
                    {
                        for (vRow = 0; vRow < lstBranch.Items.Count; vRow++)
                        {
                            lstBranch.Items[vRow].Selected = false;
                        }
                    }
                    ViewState["BrCode"] = strin;
                }
                finally
                {
                    dt = null;
                    oUsr = null;
                }
            }
        }

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            popBranchByStateId();
        }
    }
}