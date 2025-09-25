using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Admin
{
    public partial class Role : CENTRUMBase
    {
        protected int vPgNo = 1;

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
                LoadGrid(0);
                tbRole.ActiveTabIndex = 0;
                StatusButton("View");
                PopBCProduct();
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
                this.PageHeading = "Role";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuRole);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Role Master", false);
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
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            string vBrCode = "";
            CRole oRole = null;
            try
            {

                vBrCode = Session[gblValue.BrnchCode].ToString();
                oRole = new CRole();
                dt = oRole.GetRoleList();
                gvRole.DataSource = dt;
                gvRole.DataBind();
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
                oRole = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            tbRole.ActiveTabIndex = 0;
        }

        /// <summary>
        /// Status Button
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
            if (txtRole.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Role cannot left blank..");
                vResult = false;
            }
            if (chkLmt.Checked == true)
            {
                if (txtRpAmt.Text.Trim() == "" || txtRpAmt.Text.Trim() == "0")
                {
                    gblFuction.MsgPopup("Receipt and Payment limit amount cannot be Zero.");
                    vResult = false;
                }
                if (txtJrAmt.Text.Trim() == "" || txtJrAmt.Text.Trim() == "0")
                {
                    gblFuction.MsgPopup("Journal limit amount cannot be Zero.");
                    vResult = false;
                }
                if (txtCnAmt.Text.Trim() == "" || txtCnAmt.Text.Trim() == "0")
                {
                    gblFuction.MsgPopup("Contra limit amount cannot be Zero.");
                    vResult = false;
                }
            }
            return vResult;
        }

        /// <summary>
        /// Enable Control
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtRole.Enabled = Status;
            chkLmt.Enabled = Status;
            txtRpAmt.Enabled = Status;
            txtJrAmt.Enabled = Status;
            txtCnAmt.Enabled = Status;
            ChkApp.Enabled = Status;
            txtAppAmt.Enabled = false;
            ChkDropMember.Enabled = Status;
            ChkPrematureCollection.Enabled = Status;
            ChkDemise.Enabled = Status;
            txtSncApprAmt.Enabled = Status;
            chkViewAadhar.Enabled = Status;
            chkMultipleColl.Enabled = Status;
            ddlProductType.Enabled = Status;
            ddlBCProduct.Enabled = Status;
            chkPotenMem.Enabled = Status;
            chkAllowAdv.Enabled = Status;
            chkPIIMaskingEnable.Enabled = Status;
            chkSARALWaiveAllow.Enabled = Status;
            chkMELWaiveAllow.Enabled = Status;
            chkJlgDelColl.Enabled = Status;
            chkJlgRevColl.Enabled = Status;
            chkJlgCenterTr.Enabled = Status;
            chkJlgMemberTr.Enabled = Status;
            chkJlgGroupTr.Enabled = Status;
            chkDeathFlag.Enabled = Status;
            chkCFWaiveAllow.Enabled = Status;
            ddlJLGDeviationCtrl.Enabled = Status;
        }

        /// <summary>
        /// Clear Controls
        /// </summary>
        private void ClearControls()
        {
            txtRole.Text = "";
            chkLmt.Checked = false;
            txtRpAmt.Text = "0";
            txtJrAmt.Text = "0";
            txtCnAmt.Text = "0";
            ChkApp.Checked = false;
            txtAppAmt.Text = "0";
            ChkDropMember.Checked = false;
            ChkPrematureCollection.Checked = false;
            ChkDemise.Checked = false;
            txtSncApprAmt.Text = "0";
            chkViewAadhar.Checked = false;
            chkMultipleColl.Checked = false;
            chkAllowAdv.Checked = false;
            ddlProductType.SelectedIndex = ddlProductType.Items.IndexOf(ddlProductType.Items.FindByValue("O"));
            ddlBCProduct.SelectedIndex = -1;
            BCPro.Visible = false;
            chkPIIMaskingEnable.Checked = false;
            chkSARALWaiveAllow.Checked = false;
            chkMELWaiveAllow.Checked = false;
            chkJlgDelColl.Checked = false;
            chkJlgRevColl.Checked = false;
            chkJlgCenterTr.Checked = false;
            chkJlgMemberTr.Checked = false;
            chkJlgGroupTr.Checked = false;
            chkDeathFlag.Checked = false;
            chkCFWaiveAllow.Checked = false;
            ddlJLGDeviationCtrl.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 dChk = 0, vBCProductID = 0;
            CRole oRole = null;
            string vChkDuplicate = "", vSystem = "", vStat = "N", vApp = "N", vDropMem = "N",
                    vPrematureColl = "N", vDemise = "N", vViewAAdhar = "N", vMultiColl = "N", vPotentMem = "N",
                    vAllowAdv = "N", vPIIMaskingEnable = "N", vSARALWaiveAllow = "N", vMELWaiveAllow = "N",
                    vJlgDelColl = "N", vJlgRevColl = "N", vJlgMemberTr = "N", vJlgCenterTr = "N", vJlgGroupTr = "N",
                    vDeathFlag = "N", vCFWaiveAllow = "N", vJLGDeviationCtrl = "NA";
            double vRpAmt = 0, vJrAmt = 0, vCnAmt = 0, vAppAmt = 0, vSncApprAmt = 0;
            try
            {
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 dRoleId = Convert.ToInt32(ViewState["RoleId"]);
                if (chkLmt.Checked == true)
                    vStat = "Y";
                if (txtRpAmt.Text != "")
                    vRpAmt = Convert.ToDouble(txtRpAmt.Text);
                if (txtJrAmt.Text != "")
                    vJrAmt = Convert.ToDouble(txtJrAmt.Text);
                if (txtCnAmt.Text != "")
                    vCnAmt = Convert.ToDouble(txtCnAmt.Text);
                if (ChkApp.Checked == true)
                    vApp = "Y";
                if (txtAppAmt.Text != "")
                    vAppAmt = Convert.ToDouble(txtAppAmt.Text);
                if (txtSncApprAmt.Text != "")
                    vSncApprAmt = Convert.ToDouble(txtSncApprAmt.Text);

                if (ChkDropMember.Checked == true)
                    vDropMem = "Y";
                if (ChkPrematureCollection.Checked == true)
                    vPrematureColl = "Y";
                vDemise = ChkDemise.Checked == true ? "Y" : "N";
                vViewAAdhar = chkViewAadhar.Checked == true ? "Y" : "N";
                vMultiColl = chkMultipleColl.Checked == true ? "Y" : "N";
                vPotentMem = chkPotenMem.Checked == true ? "Y" : "N";
                vAllowAdv = chkAllowAdv.Checked == true ? "Y" : "N";
                vPIIMaskingEnable = chkPIIMaskingEnable.Checked == true ? "Y" : "N";
                vSARALWaiveAllow = chkSARALWaiveAllow.Checked == true ? "Y" : "N";
                vMELWaiveAllow = chkMELWaiveAllow.Checked == true ? "Y" : "N";
                vJlgDelColl = chkJlgDelColl.Checked == true ? "Y" : "N";
                vJlgRevColl = chkJlgRevColl.Checked == true ? "Y" : "N";

                vJlgMemberTr = chkJlgMemberTr.Checked == true ? "Y" : "N";
                vJlgCenterTr = chkJlgCenterTr.Checked == true ? "Y" : "N";
                vJlgGroupTr = chkJlgGroupTr.Checked == true ? "Y" : "N";

                vDeathFlag = chkDeathFlag.Checked == true ? "Y" : "N";
                vCFWaiveAllow = chkCFWaiveAllow.Checked == true ? "Y" : "N";
                vJLGDeviationCtrl = ddlJLGDeviationCtrl.SelectedValue;

                if (ddlProductType.SelectedValue == "B")
                {
                    if (Convert.ToInt32(ddlBCProduct.SelectedValue) <= 0)
                    {
                        gblFuction.MsgPopup("Please Select BC Product..");
                        return false;
                    }
                }
                if (ddlProductType.SelectedValue == "B")
                {
                    vBCProductID = Convert.ToInt32(ddlBCProduct.SelectedValue);
                }

                if (Mode == "Save")
                {
                    if (ValidateFields() == false)
                        return false;
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Save", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Role can not be Duplicate..");
                        return false;
                    }
                    dRoleId = oRole.InsertRole(txtRole.Text.Replace("'", "''"), vRpAmt, vJrAmt, vCnAmt, vStat, vApp, vAppAmt, vDropMem,
                        vPrematureColl, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "I", vDemise, vSncApprAmt, vViewAAdhar, vMultiColl,
                        ddlProductType.SelectedValue, vBCProductID, vPotentMem, vAllowAdv, vPIIMaskingEnable, vSARALWaiveAllow, vMELWaiveAllow,
                        vJlgDelColl, vJlgRevColl, vJlgGroupTr, vJlgCenterTr, vJlgMemberTr, vDeathFlag, vCFWaiveAllow, vJLGDeviationCtrl);
                    if (dRoleId > 0)
                    {
                        ViewState["RoleId"] = dRoleId;
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
                    if (ValidateFields() == false)
                        return false;
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Edit", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Role can not be Duplicate..");
                        return false;
                    }
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Role Can not be Edited..");
                        return false;
                    }
                    dRoleId = oRole.UpdateRole(dRoleId, txtRole.Text.Replace("'", "''"), vRpAmt, vJrAmt, vCnAmt, vStat, vApp, vAppAmt, vDropMem,
                        vPrematureColl, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, vDemise, vSncApprAmt, vViewAAdhar, vMultiColl,
                        ddlProductType.SelectedValue, vBCProductID, vPotentMem, vAllowAdv, vPIIMaskingEnable, vSARALWaiveAllow, vMELWaiveAllow,
                        vJlgDelColl, vJlgRevColl, vJlgGroupTr, vJlgCenterTr, vJlgMemberTr, vDeathFlag, vCFWaiveAllow, vJLGDeviationCtrl);
                    if (dRoleId > 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oRole = new CRole();
                    oRole.ChkDuplicateRole(dRoleId, txtRole.Text.Replace("'", "''"), "Edit", ref vChkDuplicate, ref vSystem);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Role Can not be Deleted..");
                        return false;
                    }
                    oRole = new CRole();
                    dChk = oRole.CheckBeforeDelete(dRoleId);
                    if (dChk > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = true;
                    }
                    else
                    {
                        dRoleId = oRole.DeleteRole(dRoleId, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "D");
                        if (dRoleId > 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                }
                return vResult;
            }
            finally
            {
                oRole = null;
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
                    LoadGrid(0);
                    StatusButton("View");
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
        protected void gvRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRoleId = 0;
            DataTable dt = null;
            CRole oRole = null;
            try
            {
                vRoleId = Convert.ToInt32(e.CommandArgument);
                ViewState["RoleId"] = vRoleId;
                if (e.CommandName == "cmdShow")
                {
                    oRole = new CRole();
                    dt = oRole.GetRoleById(vRoleId);
                    if (dt.Rows.Count > 0)
                    {
                        txtRole.Text = Convert.ToString(dt.Rows[0]["Role"]);
                        txtRpAmt.Text = Convert.ToString(dt.Rows[0]["RpAmt"]);
                        txtJrAmt.Text = Convert.ToString(dt.Rows[0]["JrAmt"]);
                        txtCnAmt.Text = Convert.ToString(dt.Rows[0]["CnAmt"]);
                        txtSncApprAmt.Text = Convert.ToString(dt.Rows[0]["SncApprAmt"]);
                        //--------------------------------------------------------------------------------------
                        chkLmt.Checked = dt.Rows[0]["Stat_YN"].ToString() == "Y" ? true : false;
                        ChkApp.Checked = dt.Rows[0]["AppYN"].ToString() == "Y" ? true : false;
                        ChkDropMember.Checked = dt.Rows[0]["DropMember"].ToString() == "Y" ? true : false;
                        ChkPrematureCollection.Checked = dt.Rows[0]["PrematureColl"].ToString() == "Y" ? true : false;
                        ChkDemise.Checked = dt.Rows[0]["Demise"].ToString() == "Y" ? true : false;
                        chkViewAadhar.Checked = dt.Rows[0]["ViewAAdhar"].ToString() == "Y" ? true : false;
                        chkMultipleColl.Checked = dt.Rows[0]["MultiColl"].ToString() == "Y" ? true : false;
                        chkPotenMem.Checked = dt.Rows[0]["PotenMemYN"].ToString() == "Y" ? true : false;
                        chkAllowAdv.Checked = dt.Rows[0]["AllowAdvYN"].ToString() == "Y" ? true : false;
                        chkPIIMaskingEnable.Checked = dt.Rows[0]["PIIMaskingEnable"].ToString() == "Y" ? true : false;
                        chkSARALWaiveAllow.Checked = dt.Rows[0]["SARAL_Waive_Allow"].ToString() == "Y" ? true : false;
                        chkMELWaiveAllow.Checked = dt.Rows[0]["MEL_Waive_Allow"].ToString() == "Y" ? true : false;
                        chkJlgDelColl.Checked = dt.Rows[0]["JlgDelColl"].ToString() == "Y" ? true : false;
                        chkJlgRevColl.Checked = dt.Rows[0]["JlgRevColl"].ToString() == "Y" ? true : false;

                        chkJlgGroupTr.Checked = dt.Rows[0]["JlgGroupTr"].ToString() == "Y" ? true : false;
                        chkJlgCenterTr.Checked = dt.Rows[0]["JlgCenterTr"].ToString() == "Y" ? true : false;
                        chkJlgMemberTr.Checked = dt.Rows[0]["JLGMemberTr"].ToString() == "Y" ? true : false;
                        chkDeathFlag.Checked = dt.Rows[0]["DeathFlagDetag"].ToString() == "Y" ? true : false;
                        chkCFWaiveAllow.Checked = dt.Rows[0]["CF_Waive_Allow"].ToString() == "Y" ? true : false;

                        ddlProductType.SelectedIndex = ddlProductType.Items.IndexOf(ddlProductType.Items.FindByValue(dt.Rows[0]["ProTyp"].ToString().Trim()));
                        ddlJLGDeviationCtrl.SelectedIndex = ddlJLGDeviationCtrl.Items.IndexOf(ddlJLGDeviationCtrl.Items.FindByValue(dt.Rows[0]["JLGDeviationCtrl"].ToString().Trim()));
                        if (dt.Rows[0]["ProTyp"].ToString() == "O")
                        {
                            BCPro.Visible = false;
                        }
                        else if (dt.Rows[0]["ProTyp"].ToString() == "A")
                        {
                            BCPro.Visible = false;
                        }
                        else if (dt.Rows[0]["ProTyp"].ToString() == "B")
                        {
                            BCPro.Visible = true;
                            ddlBCProduct.SelectedIndex = ddlBCProduct.Items.IndexOf(ddlBCProduct.Items.FindByValue(dt.Rows[0]["BCProductId"].ToString().Trim()));
                        }
                        txtAppAmt.Text = Convert.ToString(dt.Rows[0]["AppAmt"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbRole.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tbRole.ActiveTabIndex = 0;
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
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabRole_pnlDtl_txtRole");
                StatusButton("Edit");
                if (ChkApp.Checked == false)
                    txtAppAmt.Enabled = false;
                else
                    txtAppAmt.Enabled = true;

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
                    LoadGrid(0);
                    StatusButton("Delete");
                    tbRole.ActiveTabIndex = 0;
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
                tbRole.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ChkApp_CheckedChanged(object sender, EventArgs e)
        {
            if (!ChkApp.Checked)
                txtAppAmt.Enabled = false;
            else
                txtAppAmt.Enabled = true;
            txtAppAmt.Text = "0";

        }

        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProductType.SelectedValue == "B")
            {
                BCPro.Visible = true;
            }
            else
            {
                BCPro.Visible = false;
            }

        }

        private void PopBCProduct()
        {
            DataTable dt = null;
            CRole oRole = null;
            try
            {
                oRole = new CRole();
                dt = oRole.GetBCProduct();
                ddlBCProduct.DataSource = dt;
                ddlBCProduct.DataTextField = "Product";
                ddlBCProduct.DataValueField = "ProductId";
                ddlBCProduct.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlBCProduct.Items.Insert(0, oli1);
            }
            finally
            {
                oRole = null;
                dt = null;
            }
        }
    }
}