using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class BranchMst : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");

                txtOpDt.Text = Session[gblValue.LoginDate].ToString();
                txtLogDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popDistrict();
                LoadGrid(1);
                popState();
                PopAssets();
                tbBrnh.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Branch Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBranchMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Branch Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtBrCode.Enabled = Status;
            txtBrName.Enabled = Status;
            txtOpDt.Enabled = Status;

            txtAddr1.Enabled = Status;
            txtAddr2.Enabled = Status;
            txtAddr3.Enabled = Status;
            ddlDist.Enabled = Status;
            txtPin.Enabled = Status;
            txtFax.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtEmail.Enabled = Status;
            txtOfficePhNo.Enabled = Status;
            ddlState.Enabled = Status;
            ddlImpAC.Enabled = Status;
        }
        private void ClearControls()
        {
            txtBrCode.Text = "";
            txtBrName.Text = "";
            txtAddr1.Text = "";
            txtAddr2.Text = "";
            txtAddr3.Text = "";
            ddlDist.SelectedIndex = -1;
            txtPin.Text = "";
            txtFax.Text = "";
            txtPhNo.Text = "";
            txtEmail.Text = "";
            txtOfficePhNo.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
            ddlState.SelectedIndex = -1;
            ddlImpAC.SelectedIndex = -1;
        }
        private void PopAssets()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vAssets = 1;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";
            ddlImpAC.DataTextField = "Desc";
            ddlImpAC.DataValueField = "DescId";
            ddlImpAC.DataSource = dt;
            ddlImpAC.DataBind();
            ddlImpAC.Items.Insert(0, Lst1);
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
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0; ;
            string vBrCode = "";
            CBranch oBr = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CBranch();
                dt = oBr.GetBranchPG(vBrCode, pPgIndx, ref vTotRows);
                gvBrch.DataSource = dt;
                gvBrch.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oBr = null;
                dt.Dispose();
            }
        }
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;

                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbBrnh.ActiveTabIndex = 0;
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDisId = Convert.ToString(ViewState["BCode"]);
            Int32 vErr = 0, vRec = 0, vDistId = 0;
            DateTime vOpDt = gblFuction.setDate(txtOpDt.Text);
            CBranch oBrch = null;
            CGblIdGenerator oGbl = null;
            string vBrCode = txtBrCode.Text.Replace("'", "''");
            string vImpAC = ddlImpAC.SelectedValue.ToString();
            if (vBrCode != "0000")
            {
                if (vImpAC == "-1")
                {
                    gblFuction.AjxMsgPopup("Please select Imprest Account ...");
                    return false;
                }
            }
            try
            {
                vDistId = Convert.ToInt32(ddlDist.SelectedValue);
                if (Mode == "Save")
                {
                    oBrch = new CBranch();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BranchMst", "BranchCode", txtBrCode.Text.Replace("'", "''"), "", "", "", vDisId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Branch Can not be Duplicate...");
                        return false;
                    }
                    vRec = oGbl.ChkDuplicate("BranchMst", "BranchName", txtBrName.Text.Replace("'", "''"), "", "", "", vDisId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Branch Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), vOpDt,
                        txtAddr1.Text.Replace("'", "''"), txtAddr2.Text.Replace("'", "''"),
                        txtAddr3.Text.Replace("'", "''"), vDistId, txtPin.Text, txtFax.Text, txtPhNo.Text, txtEmail.Text,
                         this.UserID, "Save", txtOfficePhNo.Text.Trim(), vImpAC,ddlCuster.SelectedValue.ToString());
                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                        ViewState["BCode"] = txtBrCode.Text;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oBrch = new CBranch();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BranchMst", "BranchCode", txtBrCode.Text.Replace("'", "''"), "", "", "BranchCode", vDisId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Branch Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), vOpDt,
                        txtAddr1.Text.Replace("'", "''"), txtAddr2.Text.Replace("'", "''"),
                        txtAddr3.Text.Replace("'", "''"), vDistId, txtPin.Text, txtFax.Text, txtPhNo.Text, txtEmail.Text,
                         this.UserID, "Edit", txtOfficePhNo.Text.Trim(), vImpAC, ddlCuster.SelectedValue.ToString());

                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oBrch = new CBranch();
                    vErr = oBrch.SaveBranch(txtBrCode.Text.Replace("'", "''"), txtBrName.Text.Replace("'", "''"), vOpDt,
                        txtAddr1.Text.Replace("'", "''"), txtAddr2.Text.Replace("'", "''"),
                        txtAddr3.Text.Replace("'", "''"), vDistId, txtPin.Text, txtFax.Text, txtPhNo.Text, txtEmail.Text,
                         this.UserID, "Delet", txtOfficePhNo.Text.Trim(), vImpAC, ddlCuster.SelectedValue.ToString());
                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
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
                oBrch = null;
                oGbl = null;
            }
        }
        private void popDistrict()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DistrictId", "DistrictName", "DistrictMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDist.DataSource = dt;
                ddlDist.DataTextField = "DistrictName";
                ddlDist.DataValueField = "DistrictId";
                ddlDist.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDist.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            ddlState.Items.Clear();
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
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
        protected void gvBrch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBCode = "";
            DataTable dt = null;
            CBranch oBr = null;
            try
            {
                vBCode = Convert.ToString(e.CommandArgument);
                ViewState["BCode"] = vBCode;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvBrch.Rows)
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

                    oBr = new CBranch();
                    dt = oBr.GetBranchDetails(vBCode);
                    if (dt.Rows.Count > 0)
                    {
                        txtBrCode.Text = Convert.ToString(dt.Rows[0]["BranchCode"]);
                        txtBrName.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                        txtOpDt.Text = Convert.ToString(dt.Rows[0]["OpeningDt"]);
                        txtAddr1.Text = Convert.ToString(dt.Rows[0]["Address1"]);
                        txtAddr2.Text = Convert.ToString(dt.Rows[0]["Address2"]);
                        txtAddr3.Text = Convert.ToString(dt.Rows[0]["Address3"]);
                        ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(dt.Rows[0]["DistrictId"].ToString()));
                        popVillage();
                        ddlVillage.SelectedIndex = ddlVillage.Items.IndexOf(ddlVillage.Items.FindByValue(dt.Rows[0]["VillageId"].ToString()));
                        popCluster();
                        ddlCuster.SelectedIndex = ddlCuster.Items.IndexOf(ddlCuster.Items.FindByValue(dt.Rows[0]["ClusterId"].ToString()));
                        txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                        txtFax.Text = Convert.ToString(dt.Rows[0]["Fax"]);
                        txtPhNo.Text = Convert.ToString(dt.Rows[0]["PhNo"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtOfficePhNo.Text = Convert.ToString(dt.Rows[0]["OffPhNo"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateId"].ToString()));
                        ddlImpAC.SelectedIndex = ddlImpAC.Items.IndexOf(ddlImpAC.Items.FindByValue(dt.Rows[0]["ImprestAC"].ToString()));
                        tbBrnh.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oBr = null;
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
                tbBrnh.ActiveTabIndex = 1;
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                //    return;
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid(1);
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Edit Account Group...");
                //    return;
                //}
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                txtBrCode.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBrnh.ActiveTabIndex = 0;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private void popVillage()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            int vDistId = Convert.ToInt32(ddlDist.SelectedValue);
            ddlVillage.Items.Clear();
            try
            {
                oVlg = new CVillage();
                dt = oVlg.popVillageByDist(vDistId);
                ddlVillage.DataSource = dt;
                ddlVillage.DataTextField = "VillageName";
                ddlVillage.DataValueField = "VillageId";
                ddlVillage.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVillage.Items.Insert(0, oli);
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }
        private void popCluster()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVillageid = ddlVillage.SelectedValue.ToString();
            ddlCuster.Items.Clear();
            try
            {
                oVlg = new CVillage();
                dt = oVlg.popClusterByVillageid(vVillageid);
                ddlCuster.DataSource = dt;
                ddlCuster.DataTextField = "ClusterName";
                ddlCuster.DataValueField = "ClusterId";
                ddlCuster.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCuster.Items.Insert(0, oli);
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }
        protected void ddlDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDist.SelectedValue.ToString() != "-1")
            {
                popVillage();
            }
        }
        protected void ddlVillage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVillage.SelectedValue.ToString() != "-1")
            {
                popCluster();
            }
        }
    }
}