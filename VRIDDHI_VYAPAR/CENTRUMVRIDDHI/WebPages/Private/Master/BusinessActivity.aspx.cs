using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;


namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Master
{
    public partial class BusinessActivity : CENTRUMBAse
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabBusiAct.ActiveTabIndex = 0;
                popBusinessType();
                LoadGrid(0);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Business Activity";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBusiActivityMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Business Activity", false);
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
                    btnExit.Enabled = false;
                    ClearControls();
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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

        private void EnableControl(Boolean Status)
        {
            txtBusinessActivity.Enabled = Status;
            ddlBusiType.Enabled = Status;
            ddlBusiSubType.Enabled = Status;
            txtMarginWithOutShop.Enabled = Status;
            txtMarginWithShop.Enabled = Status;
        }

        private void ClearControls()
        {
            txtBusinessActivity.Text = "";
            ddlBusiType.SelectedIndex = -1;
            ddlBusiSubType.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            txtMarginWithOutShop.Text = "0";
            txtMarginWithShop.Text = "0";
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBusinessType oFed = null;
            Int32 vRows = 0;
            try
            {
                oFed = new CBusinessType();
                dt = oFed.GetBusinessActivityPG(pPgIndx, ref vRows);
                gvBusiAct.DataSource = dt.DefaultView;
                gvBusiAct.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oFed = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabBusiAct.ActiveTabIndex = 0;
        }

        private void popBusinessType()
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusinessType();
                ddlBusiType.DataSource = dt;
                ddlBusiType.DataTextField = "BusinessTypeName";
                ddlBusiType.DataValueField = "BusinessTypeId";
                ddlBusiType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusiType.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popBusinessSubType(int pBusinessTypeId)
        {
            DataTable dt = null;
            CBusinessType oGb = null;
            try
            {
                oGb = new CBusinessType();
                dt = oGb.PopBusiSubTypeByTypeId(pBusinessTypeId);
                ddlBusiSubType.DataSource = dt;
                ddlBusiSubType.DataTextField = "BusinessSubType";
                ddlBusiSubType.DataValueField = "BusinessSubTypeID";
                ddlBusiSubType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBusiSubType.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void gvBusiAct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vBusinessActivityID = 0, vRow = 0;
            DataTable dt = null;
            CBusinessType oPr = null;
            try
            {
                vBusinessActivityID = Convert.ToInt32(e.CommandArgument);
                ViewState["BusinessActivityID"] = vBusinessActivityID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvBusiAct.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPr = new CBusinessType();
                    dt = oPr.GetBusinessActivityById(vBusinessActivityID);
                    if (dt.Rows.Count > 0)
                    {
                        txtBusinessActivity.Text = Convert.ToString(dt.Rows[vRow]["BusinessActivity"]).Trim();
                        ddlBusiType.SelectedIndex = ddlBusiType.Items.IndexOf(ddlBusiType.Items.FindByValue(dt.Rows[0]["BusinessTypeId"].ToString().Trim()));
                        popBusinessSubType(Convert.ToInt32(dt.Rows[0]["BusinessTypeId"]));
                        ddlBusiSubType.SelectedIndex = ddlBusiSubType.Items.IndexOf(ddlBusiSubType.Items.FindByValue(dt.Rows[0]["BusinessSubTypeId"].ToString().Trim()));
                        txtMarginWithShop.Text = Convert.ToString(dt.Rows[vRow]["MarginWithShop"]).Trim();
                        txtMarginWithOutShop.Text = Convert.ToString(dt.Rows[vRow]["MarginWithOutShop"]).Trim();

                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tabBusiAct.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPr = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vBusinessActivityID = 0;
            CBusinessType oPr = null;
            CGblIdGenerator oGbl = null;

            try
            {
                vBusinessActivityID = Convert.ToInt32(ViewState["BusinessActivityID"]);
                if (Mode == "Save")
                {
                    oPr = new CBusinessType();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BusinessActivityMst", "BusinessActivity", txtBusinessActivity.Text.Replace("'", "''"), "", "",
                                             "BusinessActivityID", Convert.ToString(ViewState["BusinessActivityID"]), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Business Activity Cannot be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertBusinessActivity(ref vBusinessActivityID, txtBusinessActivity.Text.Replace("'", "''"),Convert.ToInt32(ddlBusiType.SelectedValue),
                        Convert.ToInt32(ddlBusiSubType.SelectedValue), this.UserID, "I", "Save",Convert.ToDouble(txtMarginWithOutShop.Text),Convert.ToDouble(txtMarginWithShop.Text));
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["BusinessActivityID"] = vBusinessActivityID;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oPr = new CBusinessType();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BusinessActivityMst", "BusinessActivity", txtBusinessActivity.Text.Replace("'", "''"), "", "",
                                             "BusinessActivityID", Convert.ToString(ViewState["BusinessActivityID"]), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Business Activity Cannot be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertBusinessActivity(ref vBusinessActivityID, txtBusinessActivity.Text.Replace("'", "''"),
                           Convert.ToInt32(ddlBusiType.SelectedValue), Convert.ToInt32(ddlBusiSubType.SelectedValue), this.UserID, "E", "Edit",
                           Convert.ToDouble(txtMarginWithOutShop.Text), Convert.ToDouble(txtMarginWithShop.Text));
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
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
                    oGbl = new CGblIdGenerator();
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oPr = new CBusinessType();
                        vErr = oPr.InsertBusinessActivity(ref vBusinessActivityID, txtBusinessActivity.Text.Replace("'", "''"),
                                                    Convert.ToInt32(ddlBusiType.SelectedValue), Convert.ToInt32(ddlBusiSubType.SelectedValue), this.UserID, "D", "Del"
                                                    , Convert.ToDouble(txtMarginWithOutShop.Text), Convert.ToDouble(txtMarginWithShop.Text));
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
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oPr = null;
                oGbl = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["PurposeId"] = null;
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
                tabBusiAct.ActiveTabIndex = 1;
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
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tabBusiAct.ActiveTabIndex = 0;
                    StatusButton("Delete");
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
            tabBusiAct.ActiveTabIndex = 0;
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
                LoadGrid(0);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        protected void ddlBusiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            popBusinessSubType(Convert.ToInt32(ddlBusiType.SelectedValue));
        }
       
    }
}