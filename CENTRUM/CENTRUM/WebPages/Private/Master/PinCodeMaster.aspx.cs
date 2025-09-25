using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Data;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class PinCodeMaster : CENTRUMBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                StatusButton("View");
                LoadGrid("");
                tbQly.ActiveTabIndex = 0;
                popState();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Pincode Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " [ Login Date : " + Session[gblValue.LoginDate].ToString() + " ]";
                this.GetModuleByRole(mnuID.mnuPincodeMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pincode Master", false);
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
                    txtPincode.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    txtPincode.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    //btnExit.Enabled = false;
                    txtPincode.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    txtPincode.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    // btnExit.Enabled = true;
                    txtPincode.Enabled = false;
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
                    LoadGrid("");
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

        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopState();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbQly.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(txtSearch.Text);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid("");
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        private void LoadGrid(string pKeyword)
        {
            DataTable dt = null;
            CPinCode oPin = null;
            Int32 vRows = 0;
            try
            {
                oPin = new CPinCode();
                dt = oPin.GetPincodeList(pKeyword);
                gvPinCode.DataSource = dt.DefaultView;
                gvPinCode.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                oPin = null;
                dt = null;
            }
        }

        protected void gvPinCode_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pPinCode = 0, vRow = 0;
            DataTable dt = null;
            CPinCode oPin = null;
            try
            {
                pPinCode = Convert.ToInt32(e.CommandArgument);
                ViewState["PinCode"] = pPinCode;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPinCode.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPin = new CPinCode();
                    dt = oPin.GetPincodeDetails(pPinCode);
                    if (dt.Rows.Count > 0)
                    {
                        txtPincode.Text = Convert.ToString(dt.Rows[vRow]["PinCode"]);  
                        ddlAreaType.SelectedIndex = ddlAreaType.Items.IndexOf(ddlAreaType.Items.FindByValue(Convert.ToString(dt.Rows[vRow]["AreaType"])));
                        txtCity.Text = Convert.ToString(dt.Rows[vRow]["City"]).Trim();
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(Convert.ToString(dt.Rows[vRow]["StateId"])));
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
                oPin = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, pPinCode = 0;
            CPinCode oPin = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ViewState["PinCode"] != null)
                    pPinCode = Convert.ToInt32(ViewState["PinCode"].ToString());


                if (Mode == "Save")
                {
                    oPin = new CPinCode();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PincodeMst", "PinCode", txtPincode.Text.Replace("'", "''"), "", "", "", "", "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Pincode Can not be Duplicate...");
                        return false;
                    }
                    //if (ddlDesig.SelectedValues == "")
                    //{
                    //    gblFuction.MsgPopup("Please select Designation...");
                    //    return false;
                    //}
                    vErr = oPin.InsertPincode(Convert.ToInt32(txtPincode.Text), Convert.ToString(ddlAreaType.SelectedValue), this.UserID, "Save", txtCity.Text.Replace("'", "''"),Convert.ToInt32(ddlState.SelectedValue));
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
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
                    oPin = new CPinCode();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("PincodeMst", "PinCode", txtPincode.Text.Replace("'", "''"), "", "", "", "", "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("Pincode Can not be Duplicate...");
                    //    return false;
                    //}
                    //string vDesig = "";
                    //if (ddlDesig.SelectedValues == "")
                    //    vDesig = hddDesig.Value;
                    //else
                    //    vDesig = ddlDesig.SelectedValues.Replace("|", ",");

                    vErr = oPin.InsertPincode(Convert.ToInt32(txtPincode.Text), Convert.ToString(ddlAreaType.SelectedValue), this.UserID, "Edit", txtCity.Text.Replace("'", "''"), Convert.ToInt32(ddlState.SelectedValue));
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
                    oPin = new CPinCode();
                    vErr = oPin.InsertPincode(Convert.ToInt32(txtPincode.Text), Convert.ToString(ddlAreaType.SelectedValue), this.UserID, "Delet", txtCity.Text.Replace("'", "''"), Convert.ToInt32(ddlState.SelectedValue));
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
                oPin = null;
                oGbl = null;
            }
            return vResult;
        }

        private void EnableControl(Boolean Status)
        {
            ddlAreaType.Enabled = Status;
            txtCity.Enabled = Status;
            ddlState.Enabled = Status;
        }

        private void ClearControls()
        {
            txtCity.Text = "";
            txtPincode.Text = "";
            //ddlDesig.SelectedIndex = -1;            
            lblDate.Text = "";
            lblUser.Text = "";
            ddlAreaType.SelectedValue = "-1";
            ddlState.SelectedValue = "-1";
        }

    }
}