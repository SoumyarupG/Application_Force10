using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class Role : CENTRUMBAse
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
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadGrid();
                popLevel();
                tabRole.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }

        private void popLevel()
        {
            DataTable dt = null, dt1 = null, dt2 = null;
            CGblIdGenerator oGb = null;

            try
            {

                oGb = new CGblIdGenerator();
                dt = oGb.PopLevel();
                dt1 = dt;
                dt2 = dt1;

                dt1.DefaultView.RowFilter = "LevelType ='O'";

                ddlOpLevel.DataSource = dt1;
                ddlOpLevel.DataTextField = "LevelName";
                ddlOpLevel.DataValueField = "LevelID";
                ddlOpLevel.DataBind();
                ListItem oli = new ListItem("Not Selected", "-1");
                ddlOpLevel.Items.Insert(0, oli);

                dt2.DefaultView.RowFilter = "LevelType ='L'";

                ddlLegLevel.DataSource = dt2;
                ddlLegLevel.DataTextField = "LevelName";
                ddlLegLevel.DataValueField = "LevelID";
                ddlLegLevel.DataBind();
                ddlLegLevel.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Role";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuRole);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Role Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid()
        {
            DataTable dt = null;
            CRole oRole = null;
            try
            {
                oRole = new CRole();
                dt = oRole.GetRoleList("");
                ViewState["Role"] = dt;
                gvRole.DataSource = dt;
                gvRole.DataBind();
            }
            finally
            {
                dt = null;
                oRole = null;
            }
        }

        /// <summary>
        /// Status Button
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
                    gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtRole");
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
                    gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtRole");
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
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
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
            if (txtRole.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Role cannot be left blank....");
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtRole");
                return false;
            }
            if (ddlRoleType.SelectedValue == "-1")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Role type cannot be left blank....");
                return false;
            }
            if (ddlOpLevel.SelectedValue != "-1")
            {
                if (ddlLegLevel.SelectedValue != "-1")
                {
                    gblFuction.MsgPopup("Set either Operation or Legal level for singale Role....");
                    return false;
                }
            }
            if (ddlLegLevel.SelectedValue != "-1")
            {
                if (ddlOpLevel.SelectedValue != "-1")
                {
                    gblFuction.MsgPopup("Set either Operation or Legal level for singale Role....");
                    return false;
                }
            }
            return vResult;
        }

        /// <summary>
        /// Enable Control
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtRole.Enabled = Status;
            ddlRoleType.Enabled = Status;
            ddlOpLevel.Enabled = Status;
            ddlLegLevel.Enabled = Status;
        }

        /// <summary>
        /// Clear Controls
        /// </summary>
        private void ClearControls()
        {
            txtRole.Text = "";
            //ddlRoleType.SelectedValue = "-1";
            ddlOpLevel.SelectedValue = "-1";
            ddlLegLevel.SelectedValue = "-1";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = 0;

            CRole oRole = null;
            string vChkDuplicate = "", vSystem = "";
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 dRoleId = Convert.ToInt32(ViewState["RoleId"]);

                if (Mode == "Save")
                {
                    if (ValidateFields() == false)
                        return false;
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Save", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Role can not be Duplicate..");
                        return false;
                    }
                    dRoleId = oRole.InsertRole(txtRole.Text.Replace("'", "''"), this.UserID, vLogDt, "I", ddlRoleType.SelectedValue, Convert.ToInt32(ddlOpLevel.SelectedValue), Convert.ToInt32(ddlLegLevel.SelectedValue));
                    if (dRoleId > 0)
                    {
                        ViewState["RoleId"] = dRoleId;
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
                    if (ValidateFields() == false)
                        return false;
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Edit", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Role can not be Duplicate..");
                        return false;
                    }
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Role Can not be Edited..");
                        return false;
                    }
                    dRoleId = oRole.UpdateRole(dRoleId, txtRole.Text.Replace("'", "''"), this.UserID, vLogDt, ddlRoleType.SelectedValue, Convert.ToInt32(ddlOpLevel.SelectedValue), Convert.ToInt32(ddlLegLevel.SelectedValue));
                    if (dRoleId > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Edit", ref vChkDuplicate, ref vSystem);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Role Can not be Deleted..");
                        return false;
                    }
                    oRole = new CRole();
                    dChk = oRole.CheckBeforeDelete(dRoleId);
                    if (dChk > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = true;
                    }
                    else
                    {
                        dRoleId = oRole.DeleteRole(dRoleId, this.UserID, vLogDt, "D");
                        if (dRoleId > 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
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
                oRole = null;
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
                    LoadGrid();
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
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRoleId = 0;
            DataTable dt = null;
            CRole oRole = null;
            try
            {
                vRoleId = Convert.ToInt32(e.CommandArgument);
                ViewState["RoleId"] = vRoleId;
                if (e.CommandName == "cmdShow")
                {
                    oRole = new CRole();
                    dt = oRole.GetRoleById(vRoleId);
                    if (dt.Rows.Count > 0)
                    {
                        txtRole.Text = Convert.ToString(dt.Rows[0]["Role"]);
                        ddlRoleType.SelectedIndex = ddlRoleType.Items.IndexOf(ddlRoleType.Items.FindByValue(dt.Rows[0]["RoleType"].ToString()));
                        ddlOpLevel.SelectedIndex = ddlOpLevel.Items.IndexOf(ddlOpLevel.Items.FindByValue(dt.Rows[0]["OperationLevel"].ToString()));
                        ddlLegLevel.SelectedIndex = ddlLegLevel.Items.IndexOf(ddlLegLevel.Items.FindByValue(dt.Rows[0]["LegalLevel"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oRole = null;
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
                tabRole.ActiveTabIndex = 0;
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
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtRole");
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
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delete");
                    tabRole.ActiveTabIndex = 0;
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
                tabRole.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}