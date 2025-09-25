using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class BusinessTypeMaster : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    StatusButton("Exit");
                }
                else
                {
                    StatusButton("View");
                }
                ViewState["StateEdit"] = null;
                tbBuss.ActiveTabIndex = 0;
                LoadGrid(0);
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                this.Menu = false;
                this.PageHeading = "Business Type Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBusTypeMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Business Type Master", false);
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
                    ClearControls();
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
        private void EnableControl(Boolean Status)
        {
            txtBusinessType.Enabled = Status;
        }
        private void ClearControls()
        {
            txtBusinessType.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
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
                tbBuss.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                    LoadGrid(0);
                    ClearControls();
                    tbBuss.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBuss.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CBusinessType oFed = null;
            Int32 vRows = 0;
            try
            {
                oFed = new CBusinessType();
                dt = oFed.GetBusinessTypePG(pPgIndx, ref vRows);
                gvBusinessType.DataSource = dt.DefaultView;
                gvBusinessType.DataBind();
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
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["BusinessTypeId"]);
            Int32 vErr = 0, vRec = 0, vBusinessTypeId = 0;
            CBusinessType oBt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vBusinessTypeId = Convert.ToInt32(ViewState["BusinessTypeId"]);
                if (Mode == "Save")
                {
                    oBt = new CBusinessType();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BusinessTypeMst", "BusinessTypeName", txtBusinessType.Text.Replace("'", "''"), "", "", "BusinessTypeId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Business Type Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBt.InsertBusinessType(ref vBusinessTypeId, txtBusinessType.Text.Replace("'", "''"),
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["BusinessTypeId"] = vBusinessTypeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oBt = new CBusinessType();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BusinessTypeMst", "BusinessTypeName", txtBusinessType.Text.Replace("'", "''"), "", "", "BusinessTypeId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Business Type Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBt.InsertBusinessType(ref vBusinessTypeId, txtBusinessType.Text.Replace("'", "''"),
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "E", "Edit");
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
                    vRec = oGbl.ChkDelete(vBusinessTypeId, "BusinessTypeId", "BusinessTypeMst");
                    if (vRec <= 0)
                    {
                        oBt = new CBusinessType();
                        vErr = oBt.InsertBusinessType(ref vBusinessTypeId, txtBusinessType.Text.Replace("'", "''"),
                            Convert.ToInt32(Session[gblValue.UserId].ToString()), "D", "Del");
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
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
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
                oBt = null;
                oGbl = null;
            }
        }
        protected void gvBusinessType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vBusinessTypeId = 0, vRow = 0;
            DataTable dt = null;
            CBusinessType oPr = null;
            try
            {
                vBusinessTypeId = Convert.ToInt32(e.CommandArgument);
                ViewState["BusinessTypeId"] = vBusinessTypeId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvBusinessType.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                   
                    oPr = new CBusinessType();
                    dt = oPr.GetBusinessTypeById(vBusinessTypeId);
                    if (dt.Rows.Count > 0)
                    {
                        txtBusinessType.Text = Convert.ToString(dt.Rows[vRow]["BusinessTypeName"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbBuss.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
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
            tbBuss.ActiveTabIndex = 0;
        }
    }
}
