using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class Users : CENTRUMBAse 
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
                PopBranch();
                PopRole("W");
                GetDesigForUser();         
                LoadGrid();
                tabUsers.ActiveTabIndex = 0;
                StatusButton("View");
             
                txtPass.Attributes["type"] = "password";
                btnSave.Attributes.Add("onclick", "return GeneratePwd();");
                txtPass.Attributes.Add("autocomplete", "off");

            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "User";                
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuUser);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
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
        private void LoadGrid()
        {
            DataTable dt = null;
            CUser oUser = null;
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oUser = new CUser();
                dt = oUser.GetUserList(vBranch);                
                gvUser.DataSource = dt.DefaultView;
                gvUser.DataBind();
            }
            finally
            {
                dt = null;
                oUser = null;
            }
        }
        private void PopBranch()
        {
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
            }
            finally
            {
                dt = null;
                oBranch = null;
            }
        }
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
        private void PopRole(string pRoleType)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                ddlRole.Items.Clear();
                ddlRole.DataSource = null;
                ddlRole.DataBind();

                oUsr = new CUser();
                dt = oUsr.GetAssignedRoles(pRoleType);
                if (dt.Rows.Count > 0)
                {
                    ddlRole.DataSource = dt;
                    ddlRole.DataTextField = "Role";
                    ddlRole.DataValueField = "RoleId";
                    ddlRole.DataBind();
                }
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlRole.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private void GetDesigForUser()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                ddlDesig.Items.Clear();
                ddlDesig.DataSource = null;
                ddlDesig.DataBind();

                oUsr = new CUser();
                dt = oUsr.GetDesigForUser();
                if (dt.Rows.Count > 0)
                {
                    ddlDesig.DataSource = dt;
                    ddlDesig.DataTextField = "Designation";
                    ddlDesig.DataValueField = "DesignationID";
                    ddlDesig.DataBind();
                }
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlDesig.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            string vPass = dataEncryp.EncryptText(txtPass.Text);
            if (txtUserNm.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("User cannot be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtUserNm");
                vResult = false;
            }
            if (txtPass.Attributes.ToString() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Password cannot be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_txtPass");
                vResult = false;
            }           
            if (ddlRole.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Role cannot be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabUsers_pnlDtl_ddlRole");
                vResult = false;
            }          
            return vResult;
        }
        private void EnableControl(Boolean Status)
        {
            if (Session[gblValue.UserId].ToString() == null || Session[gblValue.UserId].ToString() != "1")
            {
                txtFullName.Enabled = Status;
                txtUserNm.Enabled = Status;
                txtPass.Enabled = Status;
                ddlRole.Enabled = Status;             
                chkActive.Enabled = Status;
                chkHO.Enabled = Status;
                lstBranch.Enabled = Status;
                chkWebAccess.Enabled = Status;
                chkMobAccess.Enabled = Status;
                ddlDesig.Enabled = Status;
            }
            else if (Session[gblValue.UserId].ToString() == "1")
            {
                txtFullName.Enabled = Status;
                txtUserNm.Enabled = Status;
                txtPass.Enabled = Status;
            }
        }
        private void ClearControls()
        {
            txtUserNm.Text = "";
            txtFullName.Text = "";
            txtPass.Attributes.Add("value","");
            hdPassword.Value = "";
            ddlRole.SelectedIndex = -1;
            ddlDesig.SelectedIndex = -1;   
            lblUser.Text = "";
            lblDate.Text = "";
            lstBranch.SelectedIndex = -1;           
            chkActive.Checked = false;
            chkHO.Checked = false;
            chkWebAccess.Checked = true;
            chkMobAccess.Checked = false;
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = 0, vRefId = 0, vRoleId = 0;
            DataTable dt1 = null;
            CUser oUser = null;            
            string vStatus = "", vUsrTyp="", vXmlData="", vChkDuplicate = "", vSystem = "",vMobAcc="N",vWebAccess="Y",vFullName="";            
            
            try
            {
                if (ValidatePassword(hdPassword.Value) == false)
                //if (ValidatePassword(txtPass.Text) == false) 
                {
                    gblFuction.MsgPopup("Password Minimum Length 8 Digit and Must Contain One UpperCase Letter, One Numeric Value and One Special Character...");
                    return false;
                } 
                
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vPass = txtPass.Text.ToString();
                string vOrgPass = "";
                vOrgPass = (Request[hdPassword.UniqueID] as string == null) ? hdPassword.Value : Request[hdPassword.UniqueID] as string;
                //string vPass = dataEncryp.EncryptText(txtPass.Text);
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
                //if (ddlDesig.SelectedValue.ToString() == "-1")
                //{
                //    gblFuction.MsgPopup("Please Select Designation...");
                //    return false;
                //}
                Int32 vDesigId = Convert.ToInt32(ddlDesig.SelectedValue);
                if (chkMobAccess.Checked == true)
                    vMobAcc = "Y";
                else
                    vMobAcc = "N";
                if (chkWebAccess.Checked == true)
                    vWebAccess = "Y";
                else
                    vWebAccess = "N";
                vFullName = txtFullName.Text.ToString();
                if (Mode == "Save")
                {
                    //if (ValidateFields() == false)
                    //    return false;
                    if (chkActive.Checked == false) 
                    {
                        gblFuction.MsgPopup("User Should be Active.");
                        return false;
                    }

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
                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Save",rdbType.SelectedValue.ToString() ,ref vChkDuplicate);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("User can not be Duplicate..");
                        return false;
                    }
                    dChk = oUser.InsertUser(0, ref dUserId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId, vLogDt, vStatus, vLogDt, this.UserID,
                        "I", 0, vUsrTyp, vStatus, vXmlData, "Save", vOrgPass, rdbType.SelectedValue.ToString(), vMobAcc, vWebAccess, vDesigId, vFullName);
                    if (dChk > 0)
                    {
                        ViewState["UserId"] = dUserId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //if (ValidateFields() == false)
                    //    return false;
                    
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
                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Edit", rdbType.SelectedValue.ToString(), ref vChkDuplicate);
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
                    dChk = oUser.InsertUser(dUserId, ref vRefId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId, vLogDt, vStatus, vLogDt, this.UserID,
                        "I", 0, vUsrTyp, vStatus, vXmlData, "Edit", vOrgPass, rdbType.SelectedValue.ToString(), vMobAcc, vWebAccess, vDesigId, vFullName);
                    if (dUserId > 0)
                    {
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {                    
                    //oUser = new CUser();
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
                    oUser.CheckDuplicateUser(dUserId, txtUserNm.Text.Replace("'", "''"), "Edit", rdbType.SelectedValue.ToString(), ref vChkDuplicate);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System User Can not be Deleted..");
                        return false;
                    }
                    oUser = new CUser();                   
                    if ((dChk > 0 && this.UserID == dUserId) || (dChk <= 0 && this.UserID == dUserId))
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                    else
                    {
                        dChk = oUser.InsertUser(dUserId, ref vRefId, txtUserNm.Text.Replace("'", "''"), vPass, vRoleId, vLogDt, vStatus, vLogDt, this.UserID,
                            "I", 0, vUsrTyp, vStatus, vXmlData, "Delet", vOrgPass, rdbType.SelectedValue.ToString(), vMobAcc, vWebAccess, vDesigId, vFullName);
                        vResult = true;
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
                dt1 = null;
                oUser = null;    
            }
        }
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
                    LoadGrid();
                    StatusButton("View");
                }                     
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vUserId = 0;
            DataTable dt = null, dt1 = null;           
            CUser oUser = null;
            string vStatus = "", vHO="";
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
                        //txtPass.Attributes.Add("value", dataEncryp.DecryptText(dt.Rows[0]["Password"].ToString()));
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvUser.Rows)
                        {
                            if ((gr.RowIndex) % 2 == 0)
                            {
                                gr.BackColor = backColor;
                                gr.ForeColor = foreColor;
                            }
                            else
                            {
                                gr.BackColor = System.Drawing.Color.White;
                                gr.ForeColor = foreColor;
                            }
                            gr.Font.Bold = false;
                            LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                            lb.Font.Bold = false;
                        }
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;

                        hdPassword.Value = Convert.ToString(dt.Rows[0]["MD5Password"]);
                        txtPass.Text = Convert.ToString(dt.Rows[0]["Password"]);
                        txtUserNm.Text = Convert.ToString(dt.Rows[0]["UserName"]);
                        txtFullName.Text = Convert.ToString(dt.Rows[0]["FullName"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        vStatus = dt.Rows[0]["Status"].ToString();
                        vHO = dt.Rows[0]["UserType"].ToString();
                        rdbType.SelectedValue = dt.Rows[0]["AssignType"].ToString();
                        PopRole(dt.Rows[0]["AssignType"].ToString());
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(dt.Rows[0]["RoleId"].ToString()));
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(dt.Rows[0]["DesignationID"].ToString()));
                        if (vStatus == "Y")
                            chkActive.Checked = true;
                        else if (vStatus == "N")
                            chkActive.Checked = false;
                        if (vHO == "Y")
                            chkHO.Checked = true;
                        else if (vHO == "N")
                            chkHO.Checked = false;

                        if (dt.Rows[0]["IsMobAccess"].ToString() == "Y")
                            chkMobAccess.Checked = true;
                        else
                            chkMobAccess.Checked = false;
                        if (dt.Rows[0]["IsWebAccess"].ToString() == "Y")
                            chkWebAccess.Checked = true;
                        else
                            chkWebAccess.Checked = false;


                        dt1 = oUser.GetUserDtl(vUserId);
                        lstBranch.Items.Clear();
                        lstBranch.DataSource = dt1;
                        lstBranch.DataBind();
                        foreach (DataRow dr in dt1.Rows)
                        {
                            if (dr["Allow"].ToString() == "Y")
                                lstBranch.Items[lstBranch.Items.IndexOf(lstBranch.Items.FindByValue(dr["BranchCode"].ToString()))].Selected = true;
                        }           
                        tabUsers.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oUser = null;
            }
        }       
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delete");
                    tabUsers.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                tabUsers.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
                char[] special = {'@','#','$','%','^','&','+','=','!','*','(',')','|','_' ,'-','\\','/','<','>'}; // or whatever
                if (passWord.IndexOfAny(special) == -1) return false;
            }
            return true;
        }
        protected void rdbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopRole(rdbType.SelectedValue);
        }
    }
}