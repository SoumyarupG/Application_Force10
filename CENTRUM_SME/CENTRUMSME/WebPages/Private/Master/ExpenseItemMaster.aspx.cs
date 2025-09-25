using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class ExpenseItemMaster : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                GetOccupationType();
                LoadGrid();
                tbExpenseItem.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Expense Item  Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuExpenseItemMst);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Expense Item  Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void GetOccupationType()
        {
            DataTable dt = new DataTable();
            CProduct oProduct = new CProduct();
            dt = oProduct.GetOccupationType();
            if (dt.Rows.Count > 0)
            {
                ddlOccType.DataSource = dt;
                ddlOccType.DataTextField = "OccupationType";
                ddlOccType.DataValueField = "OccupationTypeId";
                ddlOccType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlOccType.Items.Insert(0, oli);
            }
            else
            {
                ddlOccType.DataSource = null;
                ddlOccType.DataBind();
            }
        }
        protected void ddlOccType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vOccTypeId = 0;
            if (ddlOccType.SelectedValue != "-1")
            {
                vOccTypeId = Convert.ToInt32((Request[ddlOccType.UniqueID] as string == null) ? ddlOccType.SelectedValue : Request[ddlOccType.UniqueID] as string);
            }
            if (vOccTypeId != 0)
            {
                GetOccSubType(vOccTypeId);
            }
        }
        private void GetOccSubType(int pOccTypeId)
        {
            DataTable dt = new DataTable();
            CProduct oProduct = new CProduct();
            try
            {
                dt = oProduct.GetOccSubTypeByTypeId(pOccTypeId);
                ddlOccSubType.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlOccSubType.DataSource = dt;
                    ddlOccSubType.DataTextField = "OccupationSubType";
                    ddlOccSubType.DataValueField = "OccupationSubTypeId";
                    ddlOccSubType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlOccSubType.Items.Insert(0, oli);
                }
                else
                {
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlOccSubType.Items.Insert(0, oli);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oProduct = null;
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
                tbExpenseItem.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delet") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delet");
                }
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
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbExpenseItem.ActiveTabIndex = 0;
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CDistrict oDist = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oDist = new CDistrict();
                dt = oDist.GetExpenseItemList();
                if (dt.Rows.Count > 0)
                {
                    gvDist.DataSource = dt.DefaultView;
                    gvDist.DataBind();
                }
                else
                {
                    gvDist.DataSource = null;
                    gvDist.DataBind();
                }
            }
            finally
            {
                oDist = null;
                dt = null;
            }
        }
        protected void gvDist_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pId = 0;
            string vBrCode = "";
            DataTable dt = null;
            try
            {
                pId = Convert.ToInt32(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["DistId"] = pId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvDist.Rows)
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

                    CDistrict oDist = new CDistrict();
                    dt = oDist.GetExpenseItemById(pId);
                    if (dt.Rows.Count > 0)
                    {
                        hfItemId.Value = Convert.ToString(dt.Rows[0]["Id"]);
                        txtExpenseItem.Text = Convert.ToString(dt.Rows[0]["Product"]);
                        GetOccupationType();
                        ddlOccType.SelectedIndex = ddlOccType.Items.IndexOf(ddlOccType.Items.FindByValue(dt.Rows[0]["OccupationTypeId"].ToString()));
                        if (dt.Rows[0]["OccupationTypeId"].ToString() != "")
                            GetOccSubType(Convert.ToInt32(dt.Rows[0]["OccupationTypeId"].ToString()));
                        ddlOccSubType.SelectedIndex = ddlOccSubType.Items.IndexOf(ddlOccSubType.Items.FindByValue(dt.Rows[0]["OccupationSubTypeId"].ToString()));
                        tbExpenseItem.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            //string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDisId = Convert.ToString(ViewState["Id"]);
            Int32 vErr = 0, vItemId = 0, vNewId = 0, vOccSubTypeId = 0;
            CDistrict oDist = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (hfItemId.Value.ToString() != "")
                {
                    vItemId = Convert.ToInt32(hfItemId.Value.ToString());
                }
                if (ddlOccSubType.SelectedValue.ToString() == "-1")
                {
                    gblFuction.MsgPopup("Please Select Occupation Sub Type...");
                    return false;
                }
                vOccSubTypeId = Convert.ToInt32(ddlOccSubType.SelectedValue);
                if (Mode == "Save")
                {
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();

                    DataTable dtCount = new DataTable();
                    dtCount = oGbl.ChkeExpenseItemDuplicate(txtExpenseItem.Text.Replace("'", "''"), vItemId, "Save");
                    if (dtCount.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dtCount.Rows[0]["NoOfRecord"].ToString()) > 0)
                        {
                            gblFuction.MsgPopup("Item Name Can not be Duplicate...");
                            return false;
                        }
                    }
                    vErr = oDist.SaveExpenseItem(ref vNewId, vItemId, vOccSubTypeId, txtExpenseItem.Text.Replace("'", "''"), this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["Id"] = vNewId;
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
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();
                    DataTable dtCount = new DataTable();
                    dtCount = oGbl.ChkeExpenseItemDuplicate(txtExpenseItem.Text.Replace("'", "''"), vItemId, "Edit");
                    if (dtCount.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dtCount.Rows[0]["NoOfRecord"].ToString()) > 0)
                        {
                            gblFuction.MsgPopup("Item Name Can not be Duplicate...");
                            return false;
                        }
                    }
                    vErr = oDist.SaveExpenseItem(ref vNewId, vItemId, vOccSubTypeId, txtExpenseItem.Text.Replace("'", "''"), this.UserID, "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        ViewState["Id"] = vNewId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delet")
                {
                    oDist = new CDistrict();
                    oGbl = new CGblIdGenerator();
                    DataTable dtCount = new DataTable();
                    dtCount = oGbl.ChkExpenseItemDelete(vItemId);
                    if (dtCount.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dtCount.Rows[0]["CountProduct"].ToString()) > 0)
                        {
                            gblFuction.MsgPopup("Expense Item  cannot be deleted, It is used in PD..");
                            return false;
                        }
                    }
                    vErr = oDist.SaveExpenseItem(ref vNewId, vItemId, vOccSubTypeId, txtExpenseItem.Text.Replace("'", "''"), this.UserID, "Delet");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                oDist = null;
                oGbl = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtExpenseItem.Enabled = Status;
            ddlOccType.Enabled = Status;
            ddlOccSubType.Enabled = Status;
        }
        private void ClearControls()
        {
            txtExpenseItem.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ddlOccType.SelectedIndex = -1;
            ddlOccSubType.SelectedIndex = -1;
        }
    }
}