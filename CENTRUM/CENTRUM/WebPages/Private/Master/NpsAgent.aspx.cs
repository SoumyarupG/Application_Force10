using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
namespace CENTRUM.WebPages.Private.Master
{
    public partial class NpsAgent : CENTRUMBase
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
                StatusButton("View");
                LoadGrid();
                tbArea.ActiveTabIndex = 0;
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
                this.PageHeading = "Agency";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNpsAgntMst);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Agency Master", false);
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtName.Enabled = Status;
            txtAdd.Enabled = Status;
            txtPhone.Enabled = Status;
            txtCntPrsn.Enabled = Status;
            txtCode.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtAdd.Text = "";
            txtPhone.Text = "";
            txtCntPrsn.Text = "";
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
                    LoadGrid();
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
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid()
        {
            DataTable dt = null;
            CNpsAgent oAgt = null;
            try
            {
                oAgt = new CNpsAgent();
                dt = oAgt.NPS_GetAgentList();
                gvArea.DataSource = dt.DefaultView;
                gvArea.DataBind();
            }
            finally
            {
                oAgt = null;
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
            Int32 pAgentId = 0, vRow = 0;
            DataTable dt = null;
            CNpsAgent oAgt = null;
            try
            {
                pAgentId = Convert.ToInt32(e.CommandArgument);
                ViewState["AgntID"] = pAgentId;
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
                    oAgt = new CNpsAgent();
                    dt = oAgt.NPS_GetAgencyById(pAgentId);
                    if (dt.Rows.Count > 0)
                    {
                        txtCode.Text = Convert.ToString(dt.Rows[vRow]["Code"]);
                        txtName.Text = Convert.ToString(dt.Rows[vRow]["Name"]);
                        txtAdd.Text = Convert.ToString(dt.Rows[vRow]["Address"]);
                        txtPhone.Text = Convert.ToString(dt.Rows[vRow]["Phone"]);
                        txtCntPrsn.Text = Convert.ToString(dt.Rows[vRow]["ContPerson"]);
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
                oAgt = null;
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
            Int32 vErr = 0, vRec = 0, vNewId = 0, vAgntId = 0;
            CNpsAgent oAgt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ViewState["AgntID"] != null)
                    vAgntId = Convert.ToInt32(ViewState["AgntID"]);
                if (Mode == "Save")
                {
                    oAgt = new CNpsAgent();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("NPS_AgencyMst", "AgCode", txtCode.Text.Replace("'", "''"), "", "", "AgntID", vAgntId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Working Agency Code Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("NPS_AgencyMst", "Name", txtName.Text.Replace("'", "''"), "", "", "AgntID", vAgntId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Working Agency Can not be Duplicate...");
                        return false;
                    }
                    vErr = oAgt.NPS_SaveAgencyMst(ref vNewId,txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"), txtAdd.Text.Replace("'","''"), 
                        txtPhone.Text, txtCntPrsn.Text.Replace("'","''"), this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["AgntID"] = vAgntId;
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
                    oAgt = new CNpsAgent();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("NPS_AgencyMst", "AgCode", txtCode.Text.Replace("'", "''"), "", "", "AgntID", vAgntId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Working Agency Code Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("NPS_AgencyMst", "Name", txtName.Text.Replace("'", "''"), "", "", "AgntID", vAgntId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Working Agency Can not be Duplicate...");
                        return false;
                    }
                    vErr = oAgt.NPS_SaveAgencyMst(ref vAgntId, txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"), txtAdd.Text.Replace("'", "''"),
                        txtPhone.Text, txtCntPrsn.Text.Replace("'", "''"), this.UserID, "Edit");
                    if (vErr > 0)
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
                    oAgt = new CNpsAgent();
                    oGbl = new CGblIdGenerator();
                    vErr = oGbl.ChkDelete(vAgntId, "AgentId", "NPS_MemberMst");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }
                    vErr = oAgt.NPS_SaveAgencyMst(ref vAgntId, txtCode.Text.Replace("'", "''"), txtName.Text.Replace("'", "''"), txtAdd.Text.Replace("'", "''"),
                         txtPhone.Text, txtCntPrsn.Text.Replace("'", "''"), this.UserID, "Del");
                    if (vErr > 0)
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
                oAgt = null;
                oGbl = null;
            }
        }
    }
}
