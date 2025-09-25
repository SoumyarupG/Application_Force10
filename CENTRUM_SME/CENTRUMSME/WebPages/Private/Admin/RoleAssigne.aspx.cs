using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.Private.Webpages.Admin
{
    public partial class RoleAssigne : CENTRUMBAse 
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

		if (Session[gblValue.RoleId].ToString() != "1")
			StatusButton("Exit");
		else
		        StatusButton("View");		    

                ViewState["StateEdit"] = null;
                LoadRoleGrid();
                //LoadSSOptionGrid();
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
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Assign Role";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuAssigneRole);
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
                dt = oRole.GetRoleList("");
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
        private void LoadSSOptionGrid(string pMenuType)
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
            ddlType.Enabled = Status;
            gvRoleAssign.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRole.SelectedIndex = -1;
            ddlType.SelectedIndex = -1;
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vRoleID = Convert.ToInt32(ddlRole.SelectedValue);
            CSSOptionRight oSSR = null;
            oSSR = new CSSOptionRight();
            string vMnuType = ddlType.SelectedValue == "-1" ? "" : ddlType.SelectedValue.ToString();

            dt = oSSR.GetModuleByRoleId(vRoleID, vMnuType);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["RoleID"].ToString() != "0")
                {
                    gblFuction.MsgPopup("This role already assign");
                    //ddlRole.SelectedIndex = -1;
                    ViewState["RoleAssignDt"] = null;
                    gvRoleAssign.DataSource = null;
                    gvRoleAssign.DataBind();
                    return;
                }
                else
                {
                    ViewState["RoleAssignDt"] = dt;
                    gvRoleAssign.DataSource = dt.DefaultView;
                    gvRoleAssign.DataBind();
                }
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
                if (ValidateFields() == false)
                    return false;

                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vBranch = Session[gblValue.BrnchCode].ToString();
                Int32 dRoleId = Convert.ToInt32(ddlRole.SelectedValue);
                dt = (DataTable)ViewState["RoleAssignDt"];
                foreach (DataRow vdr in dt.Rows)
                {
                    vdr["RoleId"] = dRoleId.ToString();
                    vdr["CreatedBy"] = this.UserID.ToString();
                    vdr["CreationDateTime"] = System.DateTime.Now.Date;   //vLogDt.ToString();  
                    //vdr["CreationDateTime"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                    vdr["EntType"] = "I";
                    vdr["SynStatus"] = 0.ToString();
                }
                dt.AcceptChanges();
                if (dRoleId == 1)
                {
                    gblFuction.MsgPopup("Admin Role Can Not Be Edited/Deleted.");
                    return false;
                }
                if (Mode == "Save" || Mode == "Edit")
                {

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
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = true;
                    }
                    else
                    {
                        oSSOption.DeleteSSecureByRoleId(dRoleId, this.UserID, vLogDt, "D");
                        vResult = true;
                    }
                }
                return vResult;
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
                    //foreach (GridViewRow gr in gvRole.Rows)
                    //{
                    //    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    //    lb.ForeColor = System.Drawing.Color.Black;
                    //}
                    //btnShow.ForeColor = System.Drawing.Color.Red;

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvRole.Rows)
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
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnShow.ForeColor = System.Drawing.Color.White;
                    btnShow.Font.Bold = true;

                    oSSR = new CSSOptionRight();
                    dt = oSSR.GetModuleByRoleId(vRoleId,"");
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                //LoadSSOptionGrid();
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
            
	        if (e.Row.RowType == DataControlRowType.DataRow)
	        {
                if(e.Row.RowIndex == 0)
                    e.Row.Style.Add("height","50px");
            }	
		
	        DataTable dt = null;
            CheckBox chkAdd = null;
            CheckBox chkEdit = null;
            CheckBox chkDel = null;
            CheckBox chkRpt = null;
            CheckBox chkView = null;
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRoleAssign_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["RoleAssignDt"];
            ViewState["Page"] = e.NewPageIndex;
            gvRoleAssign.PageIndex = e.NewPageIndex;
            gvRoleAssign.DataSource = dt.DefaultView;
            gvRoleAssign.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkEdit_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 iR = 0;
            try
            {
                Int32 iPN = Convert.ToInt32(ViewState["Page"]);
                dt = (DataTable)ViewState["RoleAssignDt"];
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                iR = row.RowIndex;
                if (checkbox.Checked == true)
                {
                    row.Cells[4].Text = "Y";
                    if (iPN <= 0)
                        dt.Rows[iR]["AllowEdit"] = "Y";
                }
                else
                {
                    row.Cells[4].Text = "N";
                    if (iPN <= 0)
                        dt.Rows[iR]["AllowEdit"] = "N";
                }
                dt.AcceptChanges();
                ViewState["RoleAssignDt"] = dt;
            }
            finally
            {
                //dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkDelete_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 iR = 0;
            Int32 iPN = Convert.ToInt32(ViewState["Page"]);
            dt = (DataTable)ViewState["RoleAssignDt"];
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            iR = row.RowIndex;
            if (checkbox.Checked == true)
            {
                row.Cells[5].Text = "Y";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowDel"] = "Y";
            }
            else
            {
                row.Cells[5].Text = "N";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowDel"] = "N";
            }
            dt.AcceptChanges();
            ViewState["RoleAssignDt"] = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkReport_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 iR = 0;
            Int32 iPN = Convert.ToInt32(ViewState["Page"]);
            dt = (DataTable)ViewState["RoleAssignDt"];
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            iR = row.RowIndex;
            if (checkbox.Checked == true)
            {
                row.Cells[6].Text = "Y";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowPrint"] = "Y";
            }
            else
            {
                row.Cells[6].Text = "N";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowPrint"] = "N";
            }
            dt.AcceptChanges();
            ViewState["RoleAssignDt"] = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkView_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 iR = 0;
            Int32 iPN = Convert.ToInt32(ViewState["Page"]);
            dt = (DataTable)ViewState["RoleAssignDt"];
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            iR = row.RowIndex;
            if (checkbox.Checked == true)
            {
                row.Cells[2].Text = "Y";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowView"] = "Y";
            }
            else
            {
                row.Cells[2].Text = "N";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowView"] = "N";
            }
            dt.AcceptChanges();
            ViewState["RoleAssignDt"] = dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkAdd_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 iR = 0;
            Int32 iPN = Convert.ToInt32(ViewState["Page"]);
            dt = (DataTable)ViewState["RoleAssignDt"];
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            iR = row.RowIndex;
            if (checkbox.Checked == true)
            {
                row.Cells[3].Text = "Y";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowAdd"] = "Y";
            }
            else
            {
                row.Cells[3].Text = "N";
                if (iPN <= 0)
                    dt.Rows[iR]["AllowAdd"] = "N";
            }
            dt.AcceptChanges();
            ViewState["RoleAssignDt"] = dt;
        }


        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CSSOptionRight oSSR = null;
            if (ddlType.SelectedValue.ToString() != "-1")
            {
                if (Convert.ToString(ViewState["StateEdit"]) == "Edit" || Convert.ToInt32(ViewState["VieStat"]) == 0)
                {
                    oSSR = new CSSOptionRight();
                    dt = oSSR.GetModuleByRoleId(Convert.ToInt32(ViewState["RoleId"]), ddlType.SelectedValue.ToString());
                    gvRoleAssign.DataSource = dt.DefaultView;
                    gvRoleAssign.DataBind();
                    ViewState["RoleAssignDt"] = dt;
                }
                else
                    LoadSSOptionGrid(ddlType.SelectedValue);
            }

        }
    }
}