using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class DeviceMaster : CENTRUMBase
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
                popBranch();
                popBr();
                string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);

                if (vBrCode != "0000")
                {
                    ddlBr.SelectedIndex = ddlBr.Items.IndexOf(ddlBr.Items.FindByValue(vBrCode));
                    ddlBr.Enabled = false;
                    //ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(vBrCode));
                    //ddlBranch.Enabled = false;
                    LoadGrid(ddlBr.SelectedValue);
                }
                else
                {
                    LoadGrid("-1");
                }
                tbArea.ActiveTabIndex = 0;
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
                this.PageHeading = "Device Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDeviceMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Device Master", false);
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
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                string vBranchCode = Session[gblValue.BrnchCode].ToString();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), vBranchCode);
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popBr()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBr.DataSource = dt;
                ddlBr.DataTextField = "BranchName";
                ddlBr.DataValueField = "BranchCode";
                ddlBr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBr.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid(ddlBr.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtModel.Enabled = Status;
            txtIemi.Enabled = Status;
            ddlBranch.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtModel.Text = "";
            txtIemi.Text = "";
            ddlBranch.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                ViewState["StateEdit"] = "Add";
                tbArea.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                    LoadGrid("-1");
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
            tbArea.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                ddlBr.SelectedValue = "-1";
                tbArea.ActiveTabIndex = 0;
                LoadGrid("-1");
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string vBranchCode)
        {
            DataTable dt = null;
            CMob oMob = null;
            try
            {
                oMob = new CMob();
                dt = oMob.GetDeviceList(vBranchCode,txtSearchIMEI.Text);
                gvArea.DataSource = dt.DefaultView;
                gvArea.DataBind();
            }
            finally
            {
                oMob = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pID = 0, vRow = 0;
            DataTable dt = null;
            CMob oMob = null;
            try
            {
                pID = Convert.ToInt32(e.CommandArgument);
                ViewState["ID"] = pID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvArea.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oMob = new CMob();
                    dt = oMob.GetDeviceDetails(pID);
                    if (dt.Rows.Count > 0)
                    {
                        txtModel.Text = Convert.ToString(dt.Rows[vRow]["DeviceModel"]);
                        txtIemi.Text = Convert.ToString(dt.Rows[vRow]["DeviceID"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[vRow]["AllocBranch"].ToString()));                       
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbArea.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oMob = null;
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
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBranchCode = ddlBranch.SelectedValue;
            DateTime vDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Int32 vErr = 0, vRec = 0, vNewId = 0, vID = 0;
            CMob oMob = null;
            CGblIdGenerator oGbl = null;
            try
            {   
                if (ViewState["ID"] != null)
                    vID = Convert.ToInt32(ViewState["ID"]);
                if (Mode == "Save")
                {
                    oMob = new CMob();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("DeviceMst", "DeviceID", txtIemi.Text.Replace("'", "''"), "", "", "ID", vID.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("IEMI No Can not be Duplicate...");
                        return false;
                    }
                    vErr = oMob.SaveDevice(ref vNewId, txtModel.Text.Replace("'", "''"), txtIemi.Text.Replace("'", "''"), vBranchCode, vDate, Session[gblValue.BrnchCode].ToString(), this.UserID, "Save");
                    if (vErr == 0)
                    {
                        ViewState["ID"] = vNewId;
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
                    oMob = new CMob();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("DeviceMst", "DeviceID", txtIemi.Text.Replace("'", "''"), "", "", "ID", vID.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Working Area Can not be Duplicate...");
                        return false;
                    }
                    vErr = oMob.SaveDevice(ref vID, txtModel.Text.Replace("'", "''"), txtIemi.Text.Replace("'", "''"), vBranchCode, vDate, Session[gblValue.BrnchCode].ToString(), this.UserID, "Edit");
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
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
                    oMob = new CMob();
                    oGbl = new CGblIdGenerator();
                    vErr = oGbl.ChkDelete(vID, "ID", "DeviceMst");
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    vErr = oMob.SaveDevice(ref vID, txtModel.Text.Replace("'", "''"), txtIemi.Text.Replace("'", "''"), vBranchCode, vDate, Session[gblValue.BrnchCode].ToString(), this.UserID, "Delete");
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                    }
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
                oMob = null;
                oGbl = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            
                LoadGrid(ddlBr.SelectedValue);
            
        }
    }
}