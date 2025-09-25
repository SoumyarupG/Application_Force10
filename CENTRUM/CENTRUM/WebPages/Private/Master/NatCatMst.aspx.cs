using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class NatCatMst : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    StatusButton("Exit");

                }
                else
                {
                    StatusButton("View");
                }
                ViewState["StateEdit"] = null;
                LoadGrid(0);
                PopAssets();
                PopLib();
                tabLoanAppl.ActiveTabIndex = 0;
                //popLoanProduct();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NAT CAT Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNATCAT);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NAT CAT Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopAssets()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vAssets = 1;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
                ListItem Lst1 = new ListItem("<--- Select --->", "-1");
                ddlInsureLedger.DataTextField = "Desc";
                ddlInsureLedger.DataValueField = "DescId";
                ddlInsureLedger.DataSource = dt;
                ddlInsureLedger.DataBind();
                ddlInsureLedger.Items.Insert(0, Lst1);

            }
            finally
            {
                dt = null;
                oGen = null;
            }
        }

        private void PopLib()
        {
            DataTable dt = null;
            CGenParameter oGen = null;
            string vGenAcType = "G";
            Int32 vLib = 4;
            try
            {
                oGen = new CGenParameter();
                dt = oGen.GetLedgerByAcHeadId(vGenAcType, vLib);
                ListItem Lst1 = new ListItem("<--- Select --->", "-1");
                ddlInsureLedgerPremium.DataTextField = "Desc";
                ddlInsureLedgerPremium.DataValueField = "DescId";
                ddlInsureLedgerPremium.DataSource = dt;
                ddlInsureLedgerPremium.DataBind();
                ddlInsureLedgerPremium.Items.Insert(0, Lst1);

            }
            finally
            {
                dt = null;
                oGen = null;
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
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    //  gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnDelete.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(bool Status)
        {
            txtNatCatIncName.Enabled = Status;
            
            txtEffDt.Enabled = Status;
            //ChkNeft.Enabled = Status;
            //chkBank.Checked = false;
            ddlInsureLedger.Enabled = Status;
            ddlInsureLedgerPremium.Enabled = Status;
            
            gvBranch.Enabled = Status;
            chkSelectAll.Enabled = Status;
            cbActive.Enabled = Status;
            
            txtFixedAmt1Yr.Enabled = Status;
            txtFixedAmt2Yr.Enabled = Status;
            
        }

        private void ClearControls()
        {
            txtNatCatIncName.Text = "";
            
            txtEffDt.Text = "";
            LblUser.Text = "";
            LblDate.Text = "";
            //ChkNeft.Checked = false;
            ddlInsureLedger.SelectedIndex = -1;
            ddlInsureLedgerPremium.SelectedIndex = -1;
            chkSelectAll.Checked = false;
            
            cbActive.Checked = false;
            
            txtFixedAmt1Yr.Text = "0.0";
            txtFixedAmt2Yr.Text = "0.0";
            
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
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
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanAppl.ActiveTabIndex = 0;
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
                StatusButton("Add");
                ClearControls();
                //tbSchm.ActiveTabIndex = 1;
                tabLoanAppl.ActiveTabIndex = 1;
                LoadBranch("Add", 0);

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
            EnableControl(false);
            LoadGrid(0);
            tabLoanAppl.ActiveTabIndex = 0;
            ClearControls();
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
                StatusButton("View");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }


        private void LoadBranch(string pMode, Int32 vLnTypId)
        {
            DataTable dt = null;
            CLoanScheme oDs = null;
            try
            {
                oDs = new CLoanScheme();
                dt = oDs.GetBranchForLoanType(vLnTypId, pMode);
                gvBranch.DataSource = dt;
                gvBranch.DataBind();

            }
            finally
            {
                dt = null;
                oDs = null;
            }
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CLoanScheme();
                dt = oIC.GetNATCATSchemePG(pPgIndx, ref vRows);
                gvICMST.DataSource = dt.DefaultView;
                gvICMST.DataBind();
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
                oIC = null;
            }
        }



        protected void gvBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkStatus = (CheckBox)e.Row.FindControl("chkStatus");
                if (e.Row.Cells[4].Text.Trim() == "1")
                {
                    chkStatus.Checked = true;
                }
                else
                {
                    chkStatus.Checked = false;
                }
            }
        }

        private string TabletoString()
        {
            string vSchBrCode = "";
            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                if (chkStatus.Checked == true)

                    vSchBrCode = vSchBrCode + (gr.Cells[2].Text) + ",";
            }
            return vSchBrCode;
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
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    //tbSchm.ActiveTabIndex = 0;
                    tabLoanAppl.ActiveTabIndex = 0;
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
            ViewState["LoanTypeId"] = null;
        }

        private Boolean SaveRecords(string Mode)
        {
            //DataTable dtXml = TabletoXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vNatCatIcName = "", vLedgerDescId = "", vLedgerDescIdPremium = "";
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vSchBrCode = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0, vLnCycle = 0;
            double vFixedAmt1st = 0, vFixedAmt2nd = 0;
            string vMsg = "";
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);
                vSchBrCode = TabletoString();
                vNatCatIcName = Convert.ToString(txtNatCatIncName.Text);
                vLedgerDescId = ddlInsureLedger.SelectedValue.ToString();
                vLedgerDescIdPremium = ddlInsureLedgerPremium.SelectedValue.ToString();
                DateTime vAppDt = gblFuction.setDate(txtEffDt.Text);
                vFixedAmt1st = Convert.ToDouble(txtFixedAmt1Yr.Text == "" ? "0" : txtFixedAmt1Yr.Text);
                vFixedAmt2nd = Convert.ToDouble(txtFixedAmt2Yr.Text == "" ? "0" : txtFixedAmt2Yr.Text);
                
                if (ddlInsureLedgerPremium.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Insurance Ledger Premium can not be blank...");
                    return false;
                }
                if (Mode == "Save")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();

                    vRec = oGbl.ChkDuplicate("NatCatIncMst", "NatCatIncName", txtNatCatIncName.Text.Replace("'", "''"), "", "", "NatCatId", vSubId, "Save");

                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Product Name can not be Duplicate...");
                        return false;
                    }
                    if (cbActive.Checked == true)
                    {
                        vMsg = oLS.ChkNATCATAppBranch(vSchBrCode, Mode, 0);
                        if (vMsg != "OK")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }

                    //if (chkSelectAll.Checked == false)
                    //{
                    //    gblFuction.MsgPopup("Plese select atlest one branch");
                    //}
                    oLS = new CLoanScheme();
                    vErr = oLS.InsertNATCAT(ref vLoanTypeId, vNatCatIcName, vAppDt, vLedgerDescId, 
                        vSchBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "I", "Save", cbActive.Checked == false ? "N" : "Y", 
                        vLedgerDescIdPremium, vFixedAmt1st, vFixedAmt2nd);
                    //ChkNeft.Checked == false ? "N" : "Y",
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["LoanTypeId"] = vLoanTypeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("NatCatIncMst", "NatCatIncName", txtNatCatIncName.Text.Replace("'", "''"), "", "", "NatCatId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Insurance Comapny Name can not be Duplicate...");
                        return false;
                    }

                    if (cbActive.Checked == true)
                    {
                        vMsg = oLS.ChkNATCATAppBranch(vSchBrCode, Mode, vLoanTypeId);
                        if (vMsg != "OK")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }
                    //if (chkSelectAll.Checked == false)
                    //{
                    //    gblFuction.MsgPopup("Plese select atlest one branch");
                    //}

                    vErr = oLS.InsertNATCAT(ref  vLoanTypeId, vNatCatIcName, vAppDt, vLedgerDescId,
                        vSchBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "E", "Edit", cbActive.Checked == false ? "N" : "Y",
                        vLedgerDescIdPremium, vFixedAmt1st, vFixedAmt2nd);
                    //ChkNeft.Checked == false ? "N" : "Y",
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
                    vRec = 0;
                    if (vRec <= 0)
                    {
                        oLS = new CLoanScheme();
                        oLS.InsertNATCAT(ref vLoanTypeId, vNatCatIcName, vAppDt, vLedgerDescId,
                        vSchBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "D", "Del", cbActive.Checked == false ? "N" : "Y",
                        vLedgerDescIdPremium, vFixedAmt1st, vFixedAmt2nd);
                        vResult = true;
                        //ChkNeft.Checked == false ? "N" : "Y",
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oLS = null;
                oGbl = null;
            }
        }

        protected void gvICMST_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vLoanTypeId = 0;
            DataTable dt = null;
            CLoanScheme oLS = null;
            try
            {
                vLoanTypeId = Convert.ToInt32(e.CommandArgument);
                ViewState["LoanTypeId"] = vLoanTypeId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvICMST.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLS = new CLoanScheme();
                    dt = oLS.GetNATCATById(vLoanTypeId);
                    if (dt.Rows.Count > 0)
                    {

                        txtNatCatIncName.Text = Convert.ToString(dt.Rows[0]["NatCatIncName"]).Trim();
                        
                        txtEffDt.Text = Convert.ToString(dt.Rows[0]["EffectiveDt"]).ToString();
                        ddlInsureLedger.SelectedIndex = ddlInsureLedger.Items.IndexOf(ddlInsureLedger.Items.FindByValue(dt.Rows[0]["LedgerDescIdClaim"].ToString().Trim()));
                        ddlInsureLedgerPremium.SelectedIndex = ddlInsureLedgerPremium.Items.IndexOf(ddlInsureLedgerPremium.Items.FindByValue(dt.Rows[0]["LedgerDescIdPremium"].ToString().Trim()));
                        
                        cbActive.Checked = Convert.ToString(dt.Rows[0]["Active"]).Trim() == "N" ? false : true;
                        txtFixedAmt1Yr.Text = Convert.ToString(dt.Rows[0]["FixedAmt1Yr"]).Trim();
                        txtFixedAmt2Yr.Text = Convert.ToString(dt.Rows[0]["FixedAmt2Yr"]).Trim();
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadBranch("NEdit", vLoanTypeId);
                        //LoadNEFTBranch("Edit", vLoanTypeId);
                        //tbSchm.ActiveTabIndex = 1;
                        tabLoanAppl.ActiveTabIndex = 1;
                        if (Session[gblValue.BrnchCode].ToString() != "0000")
                            StatusButton("Exit");
                        else
                            StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oLS = null;
            }

        }

    }
}