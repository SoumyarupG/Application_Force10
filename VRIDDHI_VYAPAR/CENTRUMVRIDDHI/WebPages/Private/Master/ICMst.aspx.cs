using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Data;

namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Master
{
    public partial class ICMst : CENTRUMBAse
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
                this.PageHeading = "Insurance Company Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuInsCompMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Scheme Master", false);
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
            txtInsCompName.Enabled = Status;
            txtInsAmt.Enabled = Status;
            txtInstAmt2.Enabled = Status;
            txtInstAmtAbv2.Enabled = Status;
            txtProcFees.Enabled = Status;
            txtEffDt.Enabled = Status;
            ddlInsureLedger.Enabled = Status;
            ddlInsureLedgerPremium.Enabled = Status;
            chkCoBrwr.Enabled = Status;
            gvBranch.Enabled = Status;
            chkSelectAll.Enabled = Status;
            cbActive.Enabled = Status;
            chkExGst.Enabled = Status;
            txtFixedAmt1st.Enabled = Status;
            txtFixedAmt2nd.Enabled = Status;
            txtFixedAmtAbv2.Enabled = Status;
            ddlCalculateOn.Enabled = Status;
        }

        private void ClearControls()
        {
            txtInsCompName.Text = "";
            txtInsAmt.Text = "0.0";
            txtInstAmt2.Text = "0.0";
            txtInstAmtAbv2.Text = "0.0";
            txtProcFees.Text = "";
            txtEffDt.Text = "";
            LblUser.Text = "";
            LblDate.Text = "";
            ddlInsureLedger.SelectedIndex = -1;
            ddlInsureLedgerPremium.SelectedIndex = -1;
            chkSelectAll.Checked = false;
            chkCoBrwr.Checked = false;
            cbActive.Checked = false;
            chkExGst.Checked = false;
            txtFixedAmt1st.Text = "0.0";
            txtFixedAmt2nd.Text = "0.0";
            txtFixedAmtAbv2.Text = "0.0";
            ddlCalculateOn.SelectedIndex = 1;
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }

        private void LoadRegion(string pMode, Int32 vICId)
        {
            DataTable dt = null;
            CLoanScheme oDs = null;
            try
            {
                oDs = new CLoanScheme();
                dt = oDs.GetRegion(vICId, pMode);
                gvBranch.DataSource = dt;
                gvBranch.DataBind();

            }
            finally
            {
                dt = null;
                oDs = null;
            }
        }

        private void LoadBranch(string pMode, Int32 vICId)
        {
            DataTable dt = null;
            CLoanScheme oDs = null;
            try
            {
                oDs = new CLoanScheme();
                dt = oDs.GetICBranch(vICId, pMode);
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
                dt = oIC.GetInsuranceSchemePG(pPgIndx, ref vRows);
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
            string vICRegionID = "";
            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                if (chkStatus.Checked == true)

                    vICRegionID = vICRegionID + (gr.Cells[2].Text) + ",";
            }
            return vICRegionID;
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
            string vIcName = "", vLedgerDescId = "", vLedgerDescIdPremium = "";
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vICRegionID = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0;
            double vInsAmt = 0, vProcFees = 0, vInsAmtFor2Yr = 0, vInsAmtForAbove2Yr = 0, vFixedAmt1st = 0, vFixedAmt2nd = 0, vFixedAmtAbv2 = 0;
            string vMsg = "";
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vInsAmt = Convert.ToDouble(txtInsAmt.Text.Trim());
                vInsAmtFor2Yr = Convert.ToDouble(txtInstAmt2.Text.Trim());
                vInsAmtForAbove2Yr = Convert.ToDouble(txtInstAmtAbv2.Text.Trim());
                vProcFees = Convert.ToDouble(txtProcFees.Text == "" ? "0" : txtProcFees.Text);

                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);
                vICRegionID = TabletoString();
                vIcName = Convert.ToString(txtInsCompName.Text);
                vLedgerDescId = ddlInsureLedger.SelectedValue.ToString();
                vLedgerDescIdPremium = ddlInsureLedgerPremium.SelectedValue.ToString();
                vFixedAmt1st = Convert.ToDouble(txtFixedAmt1st.Text == "" ? "0" : txtFixedAmt1st.Text);
                vFixedAmt2nd = Convert.ToDouble(txtFixedAmt2nd.Text == "" ? "0" : txtFixedAmt2nd.Text);
                vFixedAmtAbv2 = Convert.ToDouble(txtFixedAmtAbv2.Text == "" ? "0" : txtFixedAmtAbv2.Text);
                DateTime vAppDt = gblFuction.setDate(txtEffDt.Text);

                if (ddlInsureLedgerPremium.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Insurance Ledger Premium can not be blank...");
                    return false;
                }
                if (Mode == "Save")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();

                    vRec = oGbl.ChkDuplicate("ICMst", "ICName", txtInsCompName.Text.Replace("'", "''"), "", "", "ICId", vSubId, "Save");

                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Insurance Comapny Name can not be Duplicate...");
                        return false;
                    }

                    vMsg = oLS.ChkApplicableRegion(vICRegionID, Mode, 0);
                    if (vMsg != "OK")
                    {
                        gblFuction.MsgPopup(vMsg);
                        return false;
                    }

                    //if (chkSelectAll.Checked == false)
                    //{
                    //    gblFuction.MsgPopup("Plese select atlest one branch");
                    //}
                    oLS = new CLoanScheme();
                    vErr = oLS.InsertInsuranceScheme(ref vLoanTypeId, vIcName, vAppDt, vLedgerDescId, vICRegionID, Convert.ToInt32(Session[gblValue.UserId].ToString()),
                        "I", "Save", cbActive.Checked == false ? "N" : "Y", vLedgerDescIdPremium, vInsAmt, vInsAmtFor2Yr, vInsAmtForAbove2Yr, vProcFees, chkCoBrwr.Checked == false ? "N" : "Y",
                         chkExGst.Checked == false ? "N" : "Y", vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, Convert.ToInt32(ddlCalculateOn.SelectedValue));
                    //ChkNeft.Checked == false ? "N" : "Y",
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["LoanTypeId"] = vLoanTypeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("ICMst", "ICName", txtInsCompName.Text.Replace("'", "''"), "", "", "ICId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Insurance Comapny Name can not be Duplicate...");
                        return false;
                    }

                    vMsg = oLS.ChkApplicableRegion(vICRegionID, Mode, vLoanTypeId);
                    if (vMsg != "OK")
                    {
                        gblFuction.MsgPopup(vMsg);
                        return false;
                    }
                    //if (chkSelectAll.Checked == false)
                    //{
                    //    gblFuction.MsgPopup("Plese select atlest one branch");
                    //}

                    vErr = oLS.InsertInsuranceScheme(ref vLoanTypeId, vIcName, vAppDt, vLedgerDescId, vICRegionID, Convert.ToInt32(Session[gblValue.UserId].ToString()),
                                            "I", "Edit", cbActive.Checked == false ? "N" : "Y", vLedgerDescIdPremium, vInsAmt, vInsAmtFor2Yr, vInsAmtForAbove2Yr, vProcFees, chkCoBrwr.Checked == false ? "N" : "Y",
                         chkExGst.Checked == false ? "N" : "Y", vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, Convert.ToInt32(ddlCalculateOn.SelectedValue));
                    //ChkNeft.Checked == false ? "N" : "Y",
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
                        oLS = new CLoanScheme();
                        vErr = oLS.InsertInsuranceScheme(ref vLoanTypeId, vIcName, vAppDt, vLedgerDescId, vICRegionID, Convert.ToInt32(Session[gblValue.UserId].ToString()),
                        "I", "Delete", cbActive.Checked == false ? "N" : "Y", vLedgerDescIdPremium, vInsAmt, vInsAmtFor2Yr, vInsAmtForAbove2Yr, vProcFees, chkCoBrwr.Checked == false ? "N" : "Y",
                         chkExGst.Checked == false ? "N" : "Y", vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, Convert.ToInt32(ddlCalculateOn.SelectedValue));
                        vResult = true;
                        //ChkNeft.Checked == false ? "N" : "Y",
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
                oLS = null;
                oGbl = null;
            }
        }

        protected void gvICMST_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vICId = 0;
            DataTable dt = null;
            CLoanScheme oLS = null;
            try
            {
                vICId = Convert.ToInt32(e.CommandArgument);
                ViewState["LoanTypeId"] = vICId;
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
                    dt = oLS.GetInsuranceSchemeById(vICId);
                    if (dt.Rows.Count > 0)
                    {

                        txtInsCompName.Text = Convert.ToString(dt.Rows[0]["ICName"]).Trim();
                        txtInsAmt.Text = Convert.ToString(dt.Rows[0]["ICAmt"]).Trim();
                        txtInstAmt2.Text = Convert.ToString(dt.Rows[0]["ICAmtFor2Yr"]).Trim();
                        txtInstAmtAbv2.Text = Convert.ToString(dt.Rows[0]["ICAmtForAbove2Yr"]).Trim();
                        txtProcFees.Text = Convert.ToString(dt.Rows[0]["ProcFees"]).ToString();
                        txtEffDt.Text = Convert.ToString(dt.Rows[0]["EffectiveDt"]).ToString();
                        ddlInsureLedger.SelectedIndex = ddlInsureLedger.Items.IndexOf(ddlInsureLedger.Items.FindByValue(dt.Rows[0]["LedgerDescId"].ToString().Trim()));
                        ddlInsureLedgerPremium.SelectedIndex = ddlInsureLedgerPremium.Items.IndexOf(ddlInsureLedgerPremium.Items.FindByValue(dt.Rows[0]["LedgerDescIdPremium"].ToString().Trim()));
                        ddlCalculateOn.SelectedIndex = ddlCalculateOn.Items.IndexOf(ddlCalculateOn.Items.FindByValue(dt.Rows[0]["CalcOn"].ToString().Trim()));
                     
                        chkCoBrwr.Checked = Convert.ToString(dt.Rows[0]["AppCoBrwrYN"]).Trim() == "N" ? false : true;
                        cbActive.Checked = Convert.ToString(dt.Rows[0]["Active"]).Trim() == "N" ? false : true;
                        chkExGst.Checked = Convert.ToString(dt.Rows[0]["ExcludingGST"]).Trim() == "N" ? false : true;

                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        txtFixedAmt1st.Text = Convert.ToString(dt.Rows[0]["FixedAmt1st"]).Trim();
                        txtFixedAmt2nd.Text = Convert.ToString(dt.Rows[0]["FixedAmt2nd"]).Trim();
                        txtFixedAmtAbv2.Text = Convert.ToString(dt.Rows[0]["FixedAmtAbv2"]).Trim();
                        LoadBranch("Edit", vICId);
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