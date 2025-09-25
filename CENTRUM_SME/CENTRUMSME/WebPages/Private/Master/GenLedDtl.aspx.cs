using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class GenLedDtl : CENTRUMBAse
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["SubLedDtl"] = null;
                popAccSubGroup();
                LoadGrid();
                StatusButton("View");
            }
        }
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Accounts Ledger Head";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGenLed);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Accounts Ledger Head", false);
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
            txtGenCode.Enabled = Status;
            txtGenDesc.Enabled = Status;
            txtShNm.Enabled = Status;
            ddlSubGrp.Enabled = Status;
            txtAddr.Enabled = Status;
            txtPhNo.Enabled = Status;
            txtEmail.Enabled = Status;
            txtBranch.Enabled = Status;
            chkSubId.Enabled = Status;
        }

        private void ClearControls()
        {
            txtGenCode.Text = "";
            txtGenDesc.Text = "";
            txtShNm.Text = "";
            ddlSubGrp.SelectedIndex = -1;
            txtAddr.Text = "";
            txtPhNo.Text = "";
            txtEmail.Text = "";
            lblUser.Text = "";
            lblDate.Text = "";
            txtBranch.Text = "";
            chkSubId.Checked = false;
        }
        private void PopSubsidiary(string pDescId)
        {

            DataTable dt = null;
            CAcGenled oCb = null;
            try
            {
                oCb = new CAcGenled();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                dt = oCb.GetAllSubsidairy(pDescId);         //vBrCode,"T" T Stands for Tag
                gvAcLed.DataSource = dt;
                gvAcLed.DataBind();
            }

            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }

        private void popAccSubGroup()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                dt = oCb.PopComboMIS("N", "N", "AA", "AcSubGrpId", "SubGrp", "AcSubGrp", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlSubGrp.DataSource = dt;
                ddlSubGrp.DataTextField = "SubGrp";
                ddlSubGrp.DataValueField = "AcSubGrpId";
                ddlSubGrp.DataBind();
                ddlSubGrp.Items.Insert(0, liSel);
            }
            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }


        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            DataTable dt = null;
            int vNoofLedger = 0;
            if (ViewState["SubLedDtl"] != null)
            {
                dt = (DataTable)ViewState["SubLedDtl"];
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow Tdr1 in dt.Rows)
                    {
                        vNoofLedger = 0;
                        foreach (DataRow Tdr2 in dt.Rows)
                        {
                            if (Tdr1["SubsidiaryId"].ToString().Trim() == Tdr2["SubsidiaryId"].ToString().Trim())
                            {
                                vNoofLedger += 1;
                            }
                        }
                        if (vNoofLedger > 1)
                        {
                            gblFuction.AjxMsgPopup("One Ledger Cannot Appear More Than Once.");
                            return false;
                        }
                    }
                }
            }

            if (txtGenCode.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Ledger Code Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtGenCode");
                vResult = false;
            }
            if (txtGenDesc.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Ledger Name Cannot be left blank...");
                gblFuction.focus("ctl00_cph_Main_txtGenDesc");
                vResult = false;
            }
            if (Convert.ToString(ddlSubGrp.SelectedItem) == "CASH IN HAND")
            {
                EnableControl(true);
                gblFuction.MsgPopup("You can not create the ledger under CASH IN HAND ...");
                gblFuction.focus("ctl00_cph_Main_ddlSubGrp");
                vResult = false;
            }
            if (ddlSubGrp.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Sub Group Cannot be left Blank...");
                gblFuction.focus("ctl00_cph_Main_ddlSubGrp");
                vResult = false;
            }
            return vResult;
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
                tabAcHd.ActiveTabIndex = 1;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                EnableControl(false);
                StatusButton("View");
                //hdEdit.Value = "-1";
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
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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


        private Boolean SaveRecords(string Mode)
        {
            Int32 vChkOpBal = 0, vChkVoucher = 0, vErr = 0, vChkSubId = 0;
            Int32 vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            string vChkDuplicate = "", vSystem = "", vActType = "", vSusidairyYN = "", vAssetTypeYN = "";
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vXmlData = "";
            Boolean vResult = false;
            CAcGenled oCAcGenled = null;
            if (ValidateFields() == false)
                return false;
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                string vGenLedId = Convert.ToString(ViewState["DescId"]);
                Int32 vddlSubGrp = Convert.ToInt32(ddlSubGrp.SelectedValue);
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                if (vddlSubGrp == 2 || vddlSubGrp == 14) // for BANK ACCOUNTS/BANK OD ACCOUNT
                    vActType = "B";
                else
                    vActType = "G";
                if (chkSubId.Checked == true)
                {
                    vSusidairyYN = "Y";
                    DataTable dt = (DataTable)ViewState["SubLedDtl"];
                    //using ( StringWriter oSW = new StringWriter())
                    //{
                    //    dt.WriteXml(oSW);
                    //    vXmlData = oSW.ToString();
                    //}
                }
                else
                {
                    vSusidairyYN = "N";
                    vXmlData = "";
                }
                if (ChkAsset.Checked == true)
                {
                    vAssetTypeYN = "Y";
                    DataTable dt = (DataTable)ViewState["SubLedDtl"];
                    //using ( StringWriter oSW = new StringWriter())
                    //{
                    //    dt.WriteXml(oSW);
                    //    vXmlData = oSW.ToString();
                    //}
                }
                else
                {
                    vAssetTypeYN = "N";
                    vXmlData = "";
                }
                if (txtBranch.Text.Trim().Length == 0 || txtBranch.Text == "")
                {
                    gblFuction.MsgPopup("Branch name can not be empty..");
                    return false;
                }
                if (Mode == "Save")
                {
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Save", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Ledger Code or Name can not be Duplicate..");
                        return false;
                    }
                    vErr = oCAcGenled.InsertAcGenled(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                        txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType,
                        txtAddr.Text.Replace("'", "''"), txtPhNo.Text.Replace("'", "''"), "N", txtEmail.Text.Replace("'", "''"),
                        txtBranch.Text, this.UserID, "I", 0, vAssetTypeYN);
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
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Edit", ref vChkDuplicate, ref vSystem);
                    if (vChkDuplicate == "Y")
                    {
                        gblFuction.MsgPopup("Ledger Code or Name can not be Duplicate..");
                        return false;
                    }
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Ledger Can not be Edited..");
                        return false;
                    }
                    else
                    {
                        vErr = oCAcGenled.UpdateAcGenled(vGenLedId, txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                              txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType, txtAddr.Text.Replace("'", "''"),
                              txtPhNo.Text, "N", txtEmail.Text.Replace("'", "''"),
                              txtBranch.Text, this.UserID, "E", 0, vAssetTypeYN);
                        if (vErr == 0)
                        {
                            ViewState["DescId"] = vGenLedId;
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                }
                else if (Mode == "Delete")
                {
                    oCAcGenled = new CAcGenled();
                    oCAcGenled.ChkDupLedger(txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"), vGenLedId, "Edit", ref vChkDuplicate, ref vSystem);
                    if (vSystem == "Y")
                    {
                        gblFuction.MsgPopup("System Ledger Can not be Deleted..");
                        return false;
                    }
                    oCAcGenled.ChkSubsideryAc(vGenLedId, ref vChkSubId);
                    if (vChkSubId> 0 )
                    {
                        gblFuction.MsgPopup("Ledger have Subsidiary Can not be Deleted..");
                        return false;
                    }
                    oCAcGenled.ChkDeleteACLedger(vGenLedId, Session[gblValue.ACVouDtl].ToString(), ref vChkVoucher, ref vChkOpBal);
                    if (vChkVoucher > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        return false;
                    }
                    if (vChkOpBal > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = true;
                    }
                    vErr = oCAcGenled.UpdateAcGenled(vGenLedId, txtGenCode.Text.Replace("'", "''"), txtGenDesc.Text.Replace("'", "''"),
                          txtShNm.Text.Replace("'", "''"), Convert.ToInt32(ddlSubGrp.SelectedValue), vSusidairyYN, vActType, txtAddr.Text.Replace("'", "''"),
                          txtPhNo.Text, "N", txtEmail.Text.Replace("'", "''"),
                          txtBranch.Text, this.UserID, "D", 0, vAssetTypeYN);
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
                return vResult;
            }
            catch (Exception ex)
            {
                vResult = false;
                throw ex;
            }
            finally
            {
                oCAcGenled = null;
            }
        }

        
        protected void gvAcLed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAcLgd = "";
            try
            {
                vAcLgd = Convert.ToString(e.CommandArgument);
                ViewState["AcGroupId"] = vAcLgd;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvAcLed.Rows)
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

                    GetAcGenLedger(vAcLgd);
                    tabAcHd.ActiveTabIndex = 1;
                }
            }
            finally
            {
                vAcLgd = "";
            }
        }
        private void GetAcGenLedger(string pGenLedId)
        {
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            CAcGenled oAcGenLed = null;
            string vSusidiary = "", vAsset = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oAcGenLed = new CAcGenled();
                if (pGenLedId == "" || pGenLedId != null)
                {
                    ds = oAcGenLed.GetGenLedSubsidairyDtl(pGenLedId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["StateEdit"] = "Edit";
                        ViewState["DescId"] = dt.Rows[0]["DescId"].ToString();
                        txtGenCode.Text = dt.Rows[0]["GenLedCode"].ToString();
                        txtGenDesc.Text = dt.Rows[0]["Desc"].ToString();
                        txtShNm.Text = dt.Rows[0]["ShortName"].ToString();
                        txtAddr.Text = dt.Rows[0]["Address1"].ToString();
                        txtPhNo.Text = dt.Rows[0]["Phone"].ToString();
                        txtEmail.Text = dt.Rows[0]["EMail"].ToString();
                        vSusidiary = dt.Rows[0]["SubSiLedYN"].ToString();
                        vAsset = dt.Rows[0]["AssetTypeYN"].ToString();
                        ddlSubGrp.SelectedIndex = ddlSubGrp.Items.IndexOf(ddlSubGrp.Items.FindByValue(dt.Rows[0]["AcSubGrpId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        txtBranch.Text = dt.Rows[0]["BranchCode"].ToString();
                        if (vSusidiary == "Y")
                        {
                            chkSubId.Checked = true;
                        }
                        else
                        {
                            chkSubId.Checked = false;
                        }
                        if (vAsset == "Y")
                        {
                            ChkAsset.Checked = true;
                        }
                        else
                        {
                            ChkAsset.Checked = false;
                        }
                        StatusButton("Show");
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvSubs.DataSource = dt1;
                        gvSubs.DataBind();
                        ViewState["SubLedDtl"] = dt1;
                    }
                    else
                    { 
                        gvSubs.DataSource = null;
                        gvSubs.DataBind();
                        ViewState["SubLedDtl"] = null;                   
                    }
                }
            }
            finally
            {
                oAcGenLed = null;
                dt = null;
                dt1 = null;
                ds = null;
            }
        }

        protected void chkSubId_CheckedChanged(object sender, EventArgs e)
        {
            CAcGenled oCAcGenled = null;
            int vChkSubId = 0;
            string vGenLedId = Convert.ToString(ViewState["DescId"]);

            if (Convert.ToString(ViewState["StateEdit"]) == "Edit")
            {
                oCAcGenled = new CAcGenled();
                oCAcGenled.ChkSubsideryAc(vGenLedId, ref vChkSubId);
                if (vChkSubId > 0)
                {
                    gblFuction.MsgPopup("Subsidiary can not be changed for this Account Ledger.");
                    chkSubId.Checked = true;
                    return;
                }
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                dt = oCb.GetAcGenLdg();
                if (dt.Rows.Count > 0)
                {
                    gvAcLed.DataSource = dt;
                    gvAcLed.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCb = null;
            }

        }
    }
}