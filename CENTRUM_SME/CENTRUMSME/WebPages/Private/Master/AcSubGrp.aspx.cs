using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class AcSubGrp : CENTRUMBAse
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
                LoadList();
                popAccGroup();
                tabSGrp.ActiveTabIndex = 0;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
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
                this.PageHeading = "AC Sub Group Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuAcctSubGrp);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "AC Sub Group Master", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtAcGrp");
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtAcGrp");
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
        private void popAccGroup()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                dt = oCb.PopComboMIS("N", "N", "AA", "ACGroupId", "ACGroup", "AcGroup", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlAcGrp.DataSource = dt;
                ddlAcGrp.DataTextField = "ACGroup";
                ddlAcGrp.DataValueField = "ACGroupId";
                ddlAcGrp.DataBind();
                ddlAcGrp.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
                oCb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtSubGrpCode.Enabled = Status;
            txtSubGrpName.Enabled = Status;
            ddlAcGrp.Enabled = Status;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtSubGrpCode.Text = "";
            txtSubGrpName.Text = "";
            ddlAcGrp.SelectedIndex = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null;
            TreeNode tnAccGrp = null;
            Int32 vAcctId = 0;
            tvAcSubGrp.Nodes.Clear();
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                dtRoot = oCb.PopComboMIS("N", "N", "AA", "ACHeadId", "ACHead", "ACHead", 0, "AA", "AA", System.DateTime.Now, "0000");
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["ACHead"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvAcSubGrp.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("AH" + dr["ACHeadId"]);
                    vAcctId = Convert.ToInt32(dr["ACHeadId"]);
                    for (int iC = 0; iC < 1; iC++)
                    {
                        tnAccGrp = new TreeNode("No Record");
                        tnAccGrp.PopulateOnDemand = false;
                        tnAccGrp.Value = "AG";
                        tnRoot.ChildNodes.Add(tnAccGrp);
                        tnRoot.CollapseAll();
                    }
                    tnAccGrp.SelectAction = TreeNodeSelectAction.None;
                }
            }
            finally
            {
                oCb = null;
                dtRoot = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvAcSubGrp_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dtAcctGrp = null;
            DataTable dtAcctSubGrp = null;
            TreeNode tnGroup = null;
            TreeNode tnSubGroup = null;
            Int32 vAHId = 0, vGID = 0;
            CGblIdGenerator oCb = null;
            try
            {

                if (e.Node.Value.Substring(0, 2) == "AH")
                {
                    oCb = new CGblIdGenerator();
                    e.Node.ChildNodes.Clear();
                    vAHId = Convert.ToInt32(e.Node.Value.Substring(2));
                    dtAcctGrp = oCb.PopComboMIS("S", "N", "AA", "ACGroupId", "ACGroup", "AcGroup", vAHId, "ACHeadId", "AA", System.DateTime.Now, "0000");
                    foreach (DataRow drBr in dtAcctGrp.Rows)
                    {
                        tnGroup = new TreeNode(Convert.ToString(drBr["AcGroup"]));
                        tnGroup.Value = Convert.ToString("AG" + drBr["ACGroupId"]);
                        e.Node.ChildNodes.Add(tnGroup);
                        tnSubGroup = new TreeNode("No Record");
                        tnGroup.PopulateOnDemand = false;
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        tnGroup.SelectAction = TreeNodeSelectAction.None;
                        tnSubGroup.Value = "0";
                        tnGroup.ChildNodes.Add(tnSubGroup);
                    }
                }
                else if (e.Node.Value.Substring(0, 2) == "AG")
                {
                    oCb = new CGblIdGenerator();
                    e.Node.ChildNodes.Clear();
                    vGID = Convert.ToInt32(e.Node.Value.Substring(2));
                    dtAcctSubGrp = oCb.PopComboMIS("S", "N", "AA", "AcSubGrpId", "SubGrp", "ACSubGrp", vGID, "AcGroupId", "AA", System.DateTime.Now, "0000");
                    if (dtAcctSubGrp.Rows.Count > 0)
                    {
                        foreach (DataRow drGrp in dtAcctSubGrp.Rows)
                        {
                            tnSubGroup = new TreeNode(drGrp["SubGrp"].ToString());
                            tnSubGroup.Value = drGrp["AcSubGrpId"].ToString();
                            tnSubGroup.PopulateOnDemand = false;
                            e.Node.SelectAction = TreeNodeSelectAction.None;
                            e.Node.ChildNodes.Add(tnSubGroup);
                        }
                    }
                    else
                    {
                        tnSubGroup = new TreeNode("No Record");
                        tnSubGroup.Value = "0";
                        tnSubGroup.PopulateOnDemand = false;
                        e.Node.SelectAction = TreeNodeSelectAction.None;
                        e.Node.ChildNodes.Add(tnSubGroup);
                    }
                }
            }
            finally
            {
                dtAcctGrp = null;
                dtAcctSubGrp = null;
                oCb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvAcSubGrp_SelectedNodeChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            Int32 pGrpId = 0;
            CAcSubGroup oAcSubGrp = null;
            try
            {
                oAcSubGrp = new CAcSubGroup();
                pGrpId = Convert.ToInt32(tvAcSubGrp.SelectedNode.Value);
                if (pGrpId <= 0) return;
                dt = oAcSubGrp.GetAcctSubGroupDtl(pGrpId);
                if (dt.Rows.Count > 0)
                {
                    ViewState["StateEdit"] = "Edit";
                    ViewState["AcSubGrpId"] = dt.Rows[0]["AcSubGrpId"].ToString(); ;
                    txtSubGrpCode.Text = dt.Rows[0]["SubCode"].ToString();
                    txtSubGrpName.Text = dt.Rows[0]["SubGrp"].ToString();
                    ddlAcGrp.SelectedIndex = ddlAcGrp.Items.IndexOf(ddlAcGrp.Items.FindByValue(dt.Rows[0]["AcGroupId"].ToString()));
                    LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                    LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                    tabSGrp.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
            }
            finally
            {
                oAcSubGrp = null;
                dt = null;
            }
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
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    gblFuction.MsgPopup("Fedaration can not delete Account sub Group...");
                    return;
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadList();
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabSGrp.ActiveTabIndex = 0;
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
                    LoadList();
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    gblFuction.MsgPopup("Fedaration can not Edit Account sub Group...");
                    return;
                }
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtSubGrpName");
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    gblFuction.MsgPopup("Fedaration can not add Account sub Group...");
                    return;
                }
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tabSGrp.ActiveTabIndex = 1;
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
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtSubGrpCode.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Sub Group Code Cannot be blank...");
                gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtSubGrpCode");
                vResult = false;
                return vResult;
            }
            if (txtSubGrpName.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Sub Group Name Cannot be blank...");
                gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtSubGrpName");
                vResult = false;
                return vResult;
            }
            if (ddlAcGrp.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Account Group Cannot be Blank...");
                gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlAcGrp");
                vResult = false;
                return vResult;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            string vSystem = "", vLedUse = "";
            Boolean vResult = false;
            CAcSubGroup oSubGrp = null;
            DataTable dt = null;
            Int32 vErr = 0;
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                Int32 vSubGrpId = Convert.ToInt32(ViewState["AcSubGrpId"]);
                Int32 vAcGrpId = Convert.ToInt32(ddlAcGrp.SelectedValue);
                if (Mode == "Save")
                {
                    if (ValidateFields() == false)
                        return false;

                    oSubGrp = new CAcSubGroup();
                    dt = oSubGrp.ChkDupAcSubGrp(vSubGrpId, txtSubGrpCode.Text.Replace("'", "''"), txtSubGrpName.Text.Replace("'", "''"), "Save");
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Sub Group Name/Code Cannot be Duplicated.");
                        gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtSubGrpName");
                        return false;
                    }
                    vErr = oSubGrp.InsertAcSubGrp(txtSubGrpName.Text.Replace("'", "''"), vAcGrpId, "N",
                            txtSubGrpCode.Text.Replace("'", "''"), this.UserID, "I", 0);
                    if (vErr == 0)
                    {
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
                    if (ValidateFields() == false)
                        return false;

                    oSubGrp = new CAcSubGroup();
                    oSubGrp.ChkDeleteAcSubGrp(vSubGrpId, ref vSystem, ref vLedUse);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Account can not be Edited.");
                        return false;
                    }
                    dt = oSubGrp.ChkDupAcSubGrp(vSubGrpId, txtSubGrpCode.Text.Replace("'", "''"), txtSubGrpName.Text.Replace("'", "''"), "Edit");
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Sub Group Name/Code Cannot be Duplicate.");
                        gblFuction.focus("ctl00_cph_Main_tabSubGrp_pnlDtl_txtSubGrpName");
                        return false;
                    }
                    vErr = oSubGrp.UpdateAcSubGroup(vSubGrpId, txtSubGrpName.Text.Replace("'", "''"), vAcGrpId, "N",
                        txtSubGrpCode.Text.Replace("'", "''"), this.UserID, "E", 0);
                    if (vErr == 0)
                    {
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
                    oSubGrp = new CAcSubGroup();
                    oSubGrp.ChkDeleteAcSubGrp(vSubGrpId, ref vSystem, ref vLedUse);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Account can not be Delete.");
                        vResult = false;
                    }
                    if (vLedUse == "Y")
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                    if (vSystem == "N" && vLedUse == "N")
                    {
                        vErr = oSubGrp.UpdateAcSubGroup(vSubGrpId, txtSubGrpName.Text.Replace("'", "''"), vAcGrpId, "N",
                                txtSubGrpCode.Text.Replace("'", "''"), this.UserID, "D", 0);
                        if (vErr == 0)
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
                }
                return vResult;
            }
            catch (Exception ex)
            {
                vResult = false;
                throw ex;
            }
        }
    }
}