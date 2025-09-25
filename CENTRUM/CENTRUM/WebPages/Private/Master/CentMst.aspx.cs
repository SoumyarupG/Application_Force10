using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class CentMst : CENTRUMBase
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
                StatusButton("View");
                txtDtFrm.Enabled = false;
                txtDtFrm.Text = Session[gblValue.LoginDate].ToString();
                txtLgDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                PopRO();
                popVilg();
                popGP();
                popBlk();
                popDist();
                popState();
                LoadList();
                tbCnt.ActiveTabIndex = 0;
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
                this.PageHeading = "Center";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCenterMst);
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
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Center Master", false);
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
                    txtDtCl.Enabled = false;
                    ddlDrp.Enabled = false;
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
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "Eoid";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);

                ddlCFBy.DataSource = dt;
                ddlCFBy.DataTextField = "EoName";
                ddlCFBy.DataValueField = "Eoid";
                ddlCFBy.DataBind();
                ListItem ol1 = new ListItem("<--Select-->", "-1");
                ddlCFBy.Items.Insert(0, ol1);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popVilg()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            String vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oVlg = new CVillage();
                dt = oVlg.PopVillage(vBrCode);
                ddlVilg.DataSource = dt;
                ddlVilg.DataTextField = "VillageName";
                ddlVilg.DataValueField = "VillageId";
                ddlVilg.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVilg.Items.Insert(0, oli);
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popGP()
        {
            DataTable dt = null;
            CGblIdGenerator oGP = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oGP = new CGblIdGenerator();
                dt = oGP.PopComboMIS("N", "N", "NN", "GPId", "GPName", "GPMst", "0", "AA", "AA", gblFuction.setDate(""), "0000");
                ddlGp.DataSource = dt;
                ddlGp.DataTextField = "GPName";
                ddlGp.DataValueField = "GPId";
                ddlGp.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGp.Items.Insert(0, oli);
            }
            finally
            {
                oGP = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popBlk()
        {
            DataTable dt = null;
            CGblIdGenerator oBlk = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oBlk = new CGblIdGenerator();
                dt = oBlk.PopComboMIS("N", "N", "NN", "BlockId", "BlockName", "BlockMst", "0", "AA", "AA", gblFuction.setDate(""), "0000");
                ddlBlk.DataSource = dt;
                ddlBlk.DataTextField = "BlockName";
                ddlBlk.DataValueField = "BlockId";
                ddlBlk.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBlk.Items.Insert(0, oli);
            }
            finally
            {
                oBlk = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popDist()
        {
            DataTable dt = null;
            CGblIdGenerator oDist = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oDist = new CGblIdGenerator();
                dt = oDist.PopComboMIS("N", "N", "NN", "DistrictId", "DistrictName", "DistrictMst", "0", "AA", "AA", gblFuction.setDate(""), "0000");
                ddlDist.DataSource = dt;
                ddlDist.DataTextField = "DistrictName";
                ddlDist.DataValueField = "DistrictId";
                ddlDist.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDist.Items.Insert(0, oli);
            }
            finally
            {
                oDist = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oSta = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oSta = new CGblIdGenerator();
                dt = oSta.PopComboMIS("N", "N", "NN", "StateId", "StateName", "StateMst", "0", "AA", "AA", gblFuction.setDate(""), "0000");
                ddlStat.DataSource = dt;
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlStat.Items.Insert(0, oli);
            }
            finally
            {
                oSta = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDrp.SelectedValue != "-1")
            {
                txtDtCl.Enabled = true;
                clDtCl.Enabled = true;
                txtRem.Enabled = true;
            }
            else
            {
                txtDtCl.Text = "";
                txtRem.Text = ""; 
                txtDtCl.Enabled = false;
                clDtCl.Enabled = false;
                txtRem.Enabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlVilg_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVillage oVlg = null;
            string vVlgId = ddlVilg.SelectedValue;
            try
            {
                oVlg = new CVillage();
                dt = oVlg.GetGpBlkDistStateList(vVlgId);
                ddlGp.DataSource = ddlBlk.DataSource = ddlDist.DataSource = ddlStat.DataSource = dt;
                ddlGp.DataTextField = "GPName";
                ddlGp.DataValueField = "GPId";
                ddlGp.DataBind();
                ddlBlk.DataTextField = "BlockName";
                ddlBlk.DataValueField = "BlockId";
                ddlBlk.DataBind();
                ddlDist.DataTextField = "DistrictName";
                ddlDist.DataValueField = "DistrictId";
                ddlDist.DataBind();
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ddlGp.Enabled = false;
                ddlBlk.Enabled = false;
                ddlDist.Enabled = false;
                ddlStat.Enabled = false;
            }
            finally
            {
                oVlg = null;
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
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbCnt.ActiveTabIndex = 1;
            StatusButton("Add");
            txtDtCl.Enabled = false;
            txtRem.Enabled = false;
            ClearControls();
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
                    LoadList();
                    ClearControls();
                    tbCnt.ActiveTabIndex = 0;
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
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
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
                LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
	    DateTime vEndDt = gblFuction.setDate(Session[gblValue.EndDate].ToString());
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vNewId = "";
            Int32 vCentId = Convert.ToInt32(ViewState["CentId"]);
            Int32 vErr = 0, vEoId = 0, vRec = 0;
            DateTime vFrmDt = gblFuction.setDate(txtDtFrm.Text);
            DateTime vClDt = gblFuction.setDate(txtDtCl.Text);
            CGblIdGenerator oGbl = null;
            oGbl = new CGblIdGenerator();
            if (vFrmDt > vEndDt.AddDays(1))
	        {
		        gblFuction.MsgPopup("Market Formation Date must be Less than or equal with End Date...");
		        return false;
	        }
            if (ddlDrp.SelectedValue != "-1")
            {
                if (vFrmDt >= vClDt)
                {
                    gblFuction.MsgPopup("Invalid Dropout Date....");
                    return false;
                }
                //vErr = oGbl.ChkDeleteString(vNewId, "Marketid", "GroupMSt");
                //if (vErr > 0)
                //{
                //    gblFuction.MsgPopup("This Center has Active Group, the system will not allow to Drop.");
                //    vResult = false;
                //}
            }
            CCenter oCent = null;
            
            try
            {
                vEoId = Convert.ToInt32(ddlRO.SelectedValue);
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("MarketMst", "Market", txtCrNm.Text.Replace("'", "''"), "EOID", ddlRO.SelectedValue, "MarketID", vCentId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Center Name can not be Duplicate within Same RO...");
                        return false;
                    }

                    oCent = new CCenter();
                    vErr = oCent.SaveCenter(ref vNewId, ddlRO.SelectedValue, vFrmDt, txtCrNm.Text.Replace("'", "''"),
                            txtAdd1.Text.Replace("'", "''"), txtAdd2.Text.Replace("'", "''"),
                            txtAdd3.Text.Replace("'", "''"), txtPh1.Text.Replace("'", "''"), Convert.ToInt32(ddlVilg.SelectedValue),
                            txtCrLed.Text.Replace("'", "''"), txtMPlc.Text.Replace("'", "''"), Convert.ToInt32(txtBDist.Text), 
                            ddlCFBy.SelectedValue, gblFuction.setDate(txtRoDt.Text), ddlDrp.SelectedValue, gblFuction.setDate(txtDtCl.Text),
                            txtRem.Text.Replace("'", "''"), vBrCode, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["CentId"] = vNewId;
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
                    oCent = new CCenter();
                    vNewId = Convert.ToString(ViewState["CentId"]);
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("MarketMst", "Market", txtCrNm.Text.Replace("'", "''"), "EOID", ddlRO.SelectedValue, "MarketID", vNewId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Center Name can not be Duplicate within Same LO...");
                        return false;
                    }
                    if (ddlDrp.SelectedValue != "-1")
                    {
                        vErr = oGbl.ChkDeleteString(vNewId, "Marketid", "GroupMSt");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("This Center has Active Group, the system will not allow to Drop.");
                            return false;
                        }
                    }
                    vErr = oCent.SaveCenter(ref vNewId, ddlRO.SelectedValue, vFrmDt, txtCrNm.Text.Replace("'", "''"),
                            txtAdd1.Text.Replace("'", "''"), txtAdd2.Text.Replace("'", "''"),
                            txtAdd3.Text.Replace("'", "''"), txtPh1.Text.Replace("'", "''"), Convert.ToInt32(ddlVilg.SelectedValue),
                            txtCrLed.Text.Replace("'", "''"), txtMPlc.Text.Replace("'", "''"), Convert.ToInt32(txtBDist.Text),
                            ddlCFBy.SelectedValue, gblFuction.setDate(txtRoDt.Text), ddlDrp.SelectedValue, gblFuction.setDate(txtDtCl.Text),
                            txtRem.Text.Replace("'", "''"), vBrCode, this.UserID, "Edit");
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
                    vNewId = Convert.ToString(ViewState["CentId"]);
                    vErr = oGbl.ChkDeleteString(vNewId, "Marketid", "GroupMSt");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("This Center has active Group, the system will not allow to delete.");
                        vResult = false;
                    }
                    oCent = new CCenter();
                    
                    vErr = oCent.SaveCenter(ref vNewId, ddlRO.SelectedValue, vFrmDt, txtCrNm.Text.Replace("'", "''"),
                            txtAdd1.Text.Replace("'", "''"), txtAdd2.Text.Replace("'", "''"),
                            txtAdd3.Text.Replace("'", "''"), txtPh1.Text.Replace("'", "''"), Convert.ToInt32(ddlVilg.SelectedValue),
                            txtCrLed.Text.Replace("'", "''"), txtMPlc.Text.Replace("'", "''"), Convert.ToInt32(txtBDist.Text), 
                            ddlCFBy.SelectedValue, gblFuction.setDate(txtRoDt.Text), ddlDrp.SelectedValue, gblFuction.setDate(txtDtCl.Text),
                            txtRem.Text.Replace("'", "''"), vBrCode, this.UserID, "Delet");
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
                oCent = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null, tnGrp = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Int32 vEoId = 0;
            tvCnt.Nodes.Clear();
            CEO oRO = null;
            try
            {
                oRO = new CEO();
                dtRoot = oRO.PopRO(Session[gblValue.BrnchCode].ToString(), "0", "0",vLogDt, this.UserID);
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["EoName"].ToString());
                    tnRoot.PopulateOnDemand = false;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvCnt.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("EO" + dr["Eoid"]);
                    vEoId = Convert.ToInt32(dr["Eoid"]);
                    tnGrp = new TreeNode("No Record");
                    tnGrp.Value = "EO";
                    tnRoot.ChildNodes.Add(tnGrp);
                    tnRoot.CollapseAll();
                }
            }
            finally
            {
                dtRoot = null;
                oRO = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvCnt_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dt = null;
            TreeNode tnGrp = null;
            CGblIdGenerator oCb = null;
            string vEoId = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (e.Node.Value.Substring(0, 2) == "EO")
                {
                    oCb = new CGblIdGenerator();
                    e.Node.ChildNodes.Clear();
                    vEoId = e.Node.Value.Substring(2);
                    dt = oCb.PopComboMIS("S", "N", "N", "MarketID", "Market", "MarketMSt", vEoId, "EOID", "AA", gblFuction.setDate(""), vBrCode);
                    foreach (DataRow drBr in dt.Rows)
                    {
                        tnGrp = new TreeNode(Convert.ToString(drBr["Market"]));
                        tnGrp.Value = Convert.ToString(drBr["MarketID"]);
                        tnGrp.PopulateOnDemand = false;
                        e.Node.ChildNodes.Add(tnGrp);
                    }
                }
            }
            finally
            {
                oCb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvCnt_SelectedNodeChanged(object sender, EventArgs e)
        {
           string vCentId = "";
           try
           {
               vCentId = Convert.ToString(tvCnt.SelectedNode.Value);
               fillCenterDtl(vCentId);
           }
           finally
           {
           }
        }

        private void fillCenterDtl(string vCentId)
        {
            CCenter oCent = null;
            DataTable dt = null;            
            try
            {             
                ViewState["CentId"] = vCentId;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oCent = new CCenter();
                dt = oCent.GetCentrDetails(vCentId, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    txtDtFrm.Text = dt.Rows[0]["Formationdt"].ToString();
                    txtCrNm.Text = dt.Rows[0]["Market"].ToString();
                    txtAdd1.Text = dt.Rows[0]["Add1"].ToString();
                    txtAdd2.Text = dt.Rows[0]["Add2"].ToString();
                    txtAdd3.Text = dt.Rows[0]["Add3"].ToString();
                    txtPh1.Text = dt.Rows[0]["ContactNo"].ToString();
                    txtCrLed.Text = dt.Rows[0]["MarketLeader"].ToString();
                    txtMPlc.Text = dt.Rows[0]["MeetPlace"].ToString();
                    txtBDist.Text = dt.Rows[0]["DistfrmBr"].ToString();
                    if (dt.Rows[0]["RointChDt"].ToString() != "01/01/1900")
                        txtRoDt.Text = dt.Rows[0]["RointChDt"].ToString();
                    if (dt.Rows[0]["Tra_DropDate"].ToString() != "01/01/1900")
                        txtDtCl.Text = dt.Rows[0]["Tra_DropDate"].ToString();
                    txtRem.Text = dt.Rows[0]["DropReason"].ToString();
                    ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["Eoid"].ToString()));
                    ddlVilg.SelectedIndex = ddlVilg.Items.IndexOf(ddlVilg.Items.FindByValue(dt.Rows[0]["VillageId"].ToString()));
                    ddlGp.SelectedIndex = ddlGp.Items.IndexOf(ddlGp.Items.FindByValue(dt.Rows[0]["GPId"].ToString()));
                    ddlBlk.SelectedIndex = ddlBlk.Items.IndexOf(ddlBlk.Items.FindByValue(dt.Rows[0]["BlockId"].ToString()));
                    ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(dt.Rows[0]["DistrictId"].ToString()));
                    ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByValue(dt.Rows[0]["StateId"].ToString()));
                    ddlCFBy.SelectedIndex = ddlCFBy.Items.IndexOf(ddlCFBy.Items.FindByValue(dt.Rows[0]["CenterfrBy"].ToString()));
                    ddlDrp.SelectedIndex = ddlDrp.Items.IndexOf(ddlDrp.Items.FindByValue(dt.Rows[0]["Tra_Drop"].ToString()));
                    if (dt.Rows[0]["Tra_Drop"].ToString() != "-")
                    {
                        txtDtCl.Enabled = true;
                        clDtCl.Enabled = true;
                        txtRem.Enabled = true;
                    }
                    else
                    {
                        txtDtCl.Enabled = false;
                        clDtCl.Enabled = false;
                        txtRem.Enabled = false;
                    }
                    lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                    lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                    tbCnt.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
                else
                { gblFuction.MsgPopup("No Records found."); }

            }
            finally
            {
                oCent = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlRO.Enabled = Status;
            txtCrNm.Enabled = Status;
            txtAdd1.Enabled = Status;
            txtAdd2.Enabled = Status;
            txtAdd3.Enabled = Status;
            txtPh1.Enabled = Status;
            ddlVilg.Enabled = Status;
            ddlGp.Enabled = Status;
            ddlBlk.Enabled = Status;
            ddlDist.Enabled = Status;
            ddlStat.Enabled = Status;
            txtCrLed.Enabled = Status;
            txtMPlc.Enabled = Status;
            txtBDist.Enabled = Status;
            ddlCFBy.Enabled = Status;
            txtRoDt.Enabled = Status;
            ddlDrp.Enabled = Status;
            txtDtCl.Enabled = Status;
            txtRem.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlRO.SelectedIndex = -1;
            txtCrNm.Text = "";
            txtAdd1.Text = "";
            txtAdd2.Text = "";
            txtAdd3.Text = "";
            txtPh1.Text = "";
            ddlVilg.SelectedIndex = -1;
            ddlGp.SelectedIndex = -1;
            ddlBlk.SelectedIndex = -1;
            ddlDist.SelectedIndex = -1;
            ddlStat.SelectedIndex = -1;
            txtCrLed.Text = "";
            txtMPlc.Text = "";
            txtBDist.Text = "";
            ddlCFBy.SelectedIndex = -1;
            txtRoDt.Text = "";
            ddlDrp.SelectedIndex = -1;
            txtDtCl.Text = "";
            txtRem.Text = "";
        }

        protected void txtRoDt_TextChanged(object sender, EventArgs e)
        {
            if (gblFuction.IsDate(txtRoDt.Text) == true)
            {
                DateTime vIntDate = gblFuction.setDate(txtRoDt.Text);
                if (vIntDate.Day != 1)
                {
                    gblFuction.AjxMsgPopup("Interchange Date should be first day of month");
                    txtRoDt.Text = "";
                }
                else
                {
                    if (vIntDate.Month != 4 && vIntDate.Month != 9)
                    {
                        gblFuction.AjxMsgPopup("Interchange Month should be Either April OR September");
                        txtRoDt.Text = "";
                    }
                }
            }
        }

        protected void btnShow1_Click(object sender, EventArgs e)
        {
            CCenter oCent = new CCenter();
            DataTable dt = new DataTable();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            dt = oCent.SearchCenter(vBrCode, txtSearch.Text, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
            gvCenter.DataSource = dt;
            gvCenter.DataBind();

        }

        protected void gvCenter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vCentID = "";
            vCentID = Convert.ToString(e.CommandArgument);          
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvCenter.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }              
                btnShow.ForeColor = System.Drawing.Color.Red;
            }
            fillCenterDtl(vCentID);
        }
    }
}