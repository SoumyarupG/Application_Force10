using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class ActivityMaster : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                StatusButton("View");
                PopDesig();
                LoadGrid();
                tbQly.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Activity Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " [ Login Date : " + Session[gblValue.LoginDate].ToString() + " ]";
                this.GetModuleByRole(mnuID.mnuActivityMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Activity Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
                    // btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    //btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                ViewState["StateEdit"] = "Add";
                tbQly.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

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
                    tbQly.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
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
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

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

        private void LoadGrid()
        {
            DataTable dt = null;
            CActivity oActiv = null;
            Int32 vRows = 0;
            try
            {
                oActiv = new CActivity();
                dt = oActiv.GetActivityList();
                gvActivity.DataSource = dt.DefaultView;
                gvActivity.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oActiv = null;
                dt = null;
            }
        }
        protected void gvActivity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pActivityId = 0, vRow = 0;
            DataTable dt = null;
            CActivity oActiv = null;
            try
            {
                pActivityId = Convert.ToInt32(e.CommandArgument);
                ViewState["ActivityId"] = pActivityId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvActivity.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oActiv = new CActivity();
                    dt = oActiv.GetActivityDetails(pActivityId);
                    if (dt.Rows.Count > 0)
                    {
                        txtActivity.Text = Convert.ToString(dt.Rows[vRow]["ActivityName"]);  //Make table of JobLevelMst (wid help of desigmst)
                        lblDesignation.Visible = true;
                        lblDesignation.Text = Convert.ToString(dt.Rows[vRow]["DesignationName"]);
                        hddDesig.Value = Convert.ToString(dt.Rows[vRow]["DesignationId"]);
                        ddlOperationType.SelectedIndex = ddlOperationType.Items.IndexOf(ddlOperationType.Items.FindByValue(dt.Rows[vRow]["OperationType"].ToString()));
                        //ddlPosition.SelectedIndex = ddlPosition.Items.IndexOf(ddlPosition.Items.FindByValue(dt.Rows[vRow]["Position"].ToString()));
                        //lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        //lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbQly.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                dt = null;
                oActiv = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, vActivityId = 0;
            CActivity oDesig = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ViewState["ActivityId"] != null)
                    vActivityId = Convert.ToInt32(ViewState["ActivityId"].ToString());


                if (Mode == "Save")
                {
                    oDesig = new CActivity();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("ActivityMst", "ActivityName", txtActivity.Text.Replace("'", "''"), "", "", "ActivityID", vActivityId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Activity Can not be Duplicate...");
                        return false;
                    }
                    //if (ddlDesig.SelectedValues == "")
                    //{
                    //    gblFuction.MsgPopup("Please select Designation...");
                    //    return false;
                    //}
                    vErr = oDesig.InsertActivity(ref vNewId, vActivityId, txtActivity.Text.Replace("'", "''"), ddlDesig.SelectedValues.Replace("|", ","), this.UserID, "Save",ddlOperationType.SelectedValue);
                    if (vErr > 0)
                    {
                        ViewState["ActivityId"] = vNewId;
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
                    oDesig = new CActivity();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("ActivityMst", "ActivityName", txtActivity.Text.Replace("'", "''"), "", "", "ActivityID", vActivityId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Activity Can not be Duplicate...");
                        return false;
                    }
                    string vDesig = "";
                    //if (ddlDesig.SelectedValues == "")
                    //    vDesig = hddDesig.Value;
                    //else
                    //    vDesig = ddlDesig.SelectedValues.Replace("|", ",");

                    vErr = oDesig.InsertActivity(ref vNewId, vActivityId, txtActivity.Text.Replace("'", "''"), vDesig, this.UserID, "Edit", ddlOperationType.SelectedValue);
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
                    oGbl = new CGblIdGenerator();
                    oDesig = new CActivity();
                    vErr = oDesig.InsertActivity(ref vNewId, vActivityId, txtActivity.Text.Replace("'", "''"), ddlDesig.SelectedValues.Replace("|", ","), this.UserID, "Delet", ddlOperationType.SelectedValue);
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

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oDesig = null;
                oGbl = null;
            }
            return vResult;
        }
        private void EnableControl(Boolean Status)
        {
            txtActivity.Enabled = Status;
            ddlDesig.Enabled = Status;
            ddlOperationType.Enabled = Status;
        }
        private void ClearControls()
        {
            txtActivity.Text = "";
            //ddlDesig.SelectedIndex = -1;            
            lblDate.Text = "";
            lblUser.Text = "";
            ddlOperationType.SelectedIndex = -1;
        }

        private void PopDesig()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DesignationId", "DesignationName", "DesignationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                dt.AcceptChanges();
                ddlDesig.DataSource = dt;
                ddlDesig.DataTextField = "DesignationName";
                ddlDesig.DataValueField = "DesignationId";
                ddlDesig.DataBind();
                ViewState["DsgAllow"] = dt;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
    }
}