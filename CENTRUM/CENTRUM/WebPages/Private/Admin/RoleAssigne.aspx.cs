using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class RoleAssigne : CENTRUMBase
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
                LoadRoleGrid();
                LoadSSOptionGrid();
                PopRole();
                tabRole.ActiveTabIndex = 0;
                StatusButton("View");
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
                this.PageHeading = "Role Assign";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAssigneRole);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Role Assigned", false);
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
        private void PopRole()
        {
            DataTable dt = null;
            CRole oRole = null;
            try
            {
                oRole = new CRole();
                dt = oRole.GetRoleList();
                ddlRole.DataTextField = "Role";
                ddlRole.DataValueField = "RoleId";
                ddlRole.DataSource = dt;
                ddlRole.DataBind();
                ListItem liSel = new ListItem("<-- Select -->", "-1");
                ddlRole.Items.Insert(0, liSel);
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
        private void LoadRoleGrid()
        {
            DataTable dt = null;
            CSSOption oSSOption = null;
            try
            {
                oSSOption = new CSSOption();
                dt = oSSOption.GetAssignedRoleList();
                gvRole.DataSource = dt.DefaultView;
                gvRole.DataBind();
            }
            finally
            {
                dt = null;
                oSSOption = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadSSOptionGrid()
        {
            DataTable dt = null;
            CSSOption oSSOption = null;
            try
            {
                oSSOption = new CSSOption();
                dt = oSSOption.GetSSOptionList();
                ViewState["RoleAssignDt"] = dt;
                gvRoleAssign.DataSource = dt.DefaultView;
                gvRoleAssign.DataBind();
            }
            finally
            {
                dt = null;
                oSSOption = null;
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
            if (ddlRole.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Role cannot left blank....");
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_ddlRole");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlRole.Enabled = Status;
            gvRoleAssign.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRole.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CSSOptionRight oSSR = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vRoleID = Convert.ToInt32(ddlRole.SelectedValue);
            try
            {
                oSSR = new CSSOptionRight();
                dt = oSSR.GetModuleByRoleId(vRoleID, "A");
                if (dt.Rows.Count > 0)
                {
                    gblFuction.MsgPopup("This role already assign");
                    ddlRole.SelectedIndex = -1;
                    return;
                }
            }
            finally
            {
                dt = null;
                oSSR = null;
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
            string vXmlData = "";
            DataTable dt = null;
            CSSOptionRight oSSOption = null;
            Int32 vErr = 0;
            try
            {
                this.GetModuleByRole(mnuID.mnuAssigneRole);
                if (ValidateFields() == false)
                    return false;

                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vBranch = Session[gblValue.BrnchCode].ToString();
                Int32 dRoleId = Convert.ToInt32(ddlRole.SelectedValue);

                dt = (DataTable)ViewState["RoleAssignDt"];
                foreach (GridViewRow gv in gvRoleAssign.Rows)
                {
                    CheckBox chkAdd = (CheckBox)gv.FindControl("chkAdd");
                    if (chkAdd.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowAdd"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowAdd"] = "N";

                    CheckBox chkEdit = (CheckBox)gv.FindControl("chkEdit");
                    if (chkEdit.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowEdit"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowEdit"] = "N";

                    CheckBox chkDelete = (CheckBox)gv.FindControl("chkDelete");
                    if (chkDelete.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowDel"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowDel"] = "N";

                    CheckBox chkView = (CheckBox)gv.FindControl("chkView");
                    if (chkView.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowView"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowView"] = "N";

                    CheckBox chkReport = (CheckBox)gv.FindControl("chkReport");
                    if (chkReport.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowPrint"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowPrint"] = "N";

                    CheckBox chkApprv = (CheckBox)gv.FindControl("chkApprv");
                    if (chkApprv.Checked == true)
                        dt.Rows[gv.RowIndex]["AllowProc"] = "Y";
                    else
                        dt.Rows[gv.RowIndex]["AllowProc"] = "N";

                    CheckBox chkRej = (CheckBox)gv.FindControl("chkRej");
                    dt.Rows[gv.RowIndex]["AllowReject"] = chkRej.Checked == true ? "Y" : "N";


                    dt.Rows[gv.RowIndex]["RoleId"] = dRoleId.ToString();
                    dt.Rows[gv.RowIndex]["CreatedBy"] = Session[gblValue.UserId].ToString();
                    dt.Rows[gv.RowIndex]["CreationDateTime"] = System.DateTime.Now.Date;
                    dt.Rows[gv.RowIndex]["EntType"] = "I";
                    dt.Rows[gv.RowIndex]["SynStatus"] = 0.ToString();
                }
                dt.AcceptChanges();
                if (dRoleId == 1)
                {
                    gblFuction.MsgPopup("Admin Role Can Not Be Edited/Deleted.");
                    return false;
                }
                if (Mode == "Save" || Mode == "Edit")
                {
                    this.GetModuleByRole(mnuID.mnuAssigneRole);
                    oSSOption = new CSSOptionRight();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    vErr = oSSOption.InsertSSOptionRight(dRoleId, vXmlData);
                    vResult = true;
                }
                else if (Mode == "Delete")
                {
                    oSSOption = new CSSOptionRight();
                    vErr = oSSOption.ChkRoleAssignBeforeDelete(dRoleId);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = true;
                    }
                    else
                    {
                        this.GetModuleByRole(mnuID.mnuAssigneRole);
                        oSSOption.DeleteSSecureByRoleId(dRoleId, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "D");
                        vResult = true;
                    }
                }
                return vResult;
            }
            finally
            {
                dt = null;
                oSSOption = null;
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
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadRoleGrid();
                    EnableControl(false);
                    StatusButton("View");
                }
                else
                    StatusButton("Show");
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
            CSSOptionRight oSSR = null;
            try
            {
                vRoleId = Convert.ToInt32(e.CommandArgument);
                ViewState["RoleId"] = vRoleId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvRole.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oSSR = new CSSOptionRight();
                    dt = oSSR.GetModuleByRoleId(vRoleId, "E");
                    ViewState["RoleAssignDt"] = dt;
                    gvRoleAssign.DataSource = dt.DefaultView;
                    gvRoleAssign.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(Convert.ToString(dt.Rows[0]["RoleID"])));
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
                oSSR = null;
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
                StatusButton("Edit");
                ddlRole.Enabled = false;
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
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadRoleGrid();
                    StatusButton("Delete");
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
                LoadSSOptionGrid();
                tabRole.ActiveTabIndex = 1;
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
        protected void tabRole_ActiveTabChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabRole.ActiveTabIndex == 0)
                {
                    EnableControl(false);
                    StatusButton("View");
                    ViewState["RoleId"] = null;
                    ViewState["StateEdit"] = null;
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
        protected void gvRoleAssign_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CheckBox chkAdd = null;
            CheckBox chkEdit = null;
            CheckBox chkDel = null;
            CheckBox chkRpt = null;
            CheckBox chkView = null;
            CheckBox chkProc = null;
            CheckBox chkRej = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    dt = (DataTable)ViewState["RoleAssignDt"];
                    chkView = (CheckBox)e.Row.FindControl("chkView");
                    if (e.Row.Cells[2].Text == "Y")
                        chkView.Checked = true;
                    else
                        chkView.Checked = false;
                    chkAdd = (CheckBox)e.Row.FindControl("ChkAdd");
                    if (e.Row.Cells[3].Text == "Y")
                        chkAdd.Checked = true;
                    else
                        chkAdd.Checked = false;
                    chkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                    if (e.Row.Cells[4].Text == "Y")
                        chkEdit.Checked = true;
                    else
                        chkEdit.Checked = false;
                    chkDel = (CheckBox)e.Row.FindControl("chkDelete");
                    if (e.Row.Cells[5].Text == "Y")
                        chkDel.Checked = true;
                    else
                        chkDel.Checked = false;
                    chkRpt = (CheckBox)e.Row.FindControl("chkReport");
                    if (e.Row.Cells[6].Text == "Y")
                        chkRpt.Checked = true;
                    else
                        chkRpt.Checked = false;

                    chkProc = (CheckBox)e.Row.FindControl("chkApprv");
                    if (e.Row.Cells[7].Text == "Y")
                        chkProc.Checked = true;
                    else
                        chkProc.Checked = false;

                    chkRej = (CheckBox)e.Row.FindControl("chkRej");
                    chkRej.Checked = e.Row.Cells[16].Text == "Y" ? true : false;
                }
            }
            finally
            {
                dt = null;
            }
        }
    }
}