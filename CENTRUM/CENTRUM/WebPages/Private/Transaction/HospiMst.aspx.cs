using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HospiMst : CENTRUMBase
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
                PopLiability();
                tabLoanAppl.ActiveTabIndex = 0;
                //popLoanProduct();
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Hospi Cash Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHospiMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Hospi Cash Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopLiability()
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
                ddlHospiLedger.DataTextField = "Desc";
                ddlHospiLedger.DataValueField = "DescId";
                ddlHospiLedger.DataSource = dt;
                ddlHospiLedger.DataBind();
                ddlHospiLedger.Items.Insert(0, Lst1);

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
            txtHospiCashName.Enabled = Status;
            txtHospiAmt.Enabled = Status;
            txtHospiAmt15M.Enabled = Status;
            txtHospiAmt2.Enabled = Status;
            txtHospiAmtAbv2.Enabled = Status;
            txtEffDt.Enabled = Status;
            ddlHospiLedger.Enabled = Status;
            cbActive.Enabled = Status;
            txtFixedAmt1st.Enabled = Status;
            txtFixedAmt15M.Enabled = Status;
            txtFixedAmt2nd.Enabled = Status;
            txtFixedAmtAbv2.Enabled = Status;
            gvBranch.Enabled = Status;
            chkSelectAll.Enabled = Status;

            txtHospiAmt18M.Enabled = Status;
            txtFixedAmt18M.Enabled = Status;
        }

        private void ClearControls()
        {
            txtHospiCashName.Text = "";
            txtHospiAmt.Text = "0.0";
            txtHospiAmt15M.Text = "0.0";
            txtHospiAmt2.Text = "0.0";
            txtHospiAmtAbv2.Text = "0.0";
            txtEffDt.Text = "";
            LblUser.Text = "";
            LblDate.Text = "";
            ddlHospiLedger.SelectedIndex = -1;
            cbActive.Checked = false;
            txtFixedAmt1st.Text = "0.0";
            txtFixedAmt2nd.Text = "0.0";
            txtFixedAmtAbv2.Text = "0.0";
            txtFixedAmt15M.Text = "0.0";
            txtHospiAmt18M.Text = "0.0";
            txtFixedAmt18M.Text = "0.0";
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

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CLoanScheme();
                dt = oIC.GetHospiCashPG(pPgIndx, ref vRows);
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
            ViewState["HospiId"] = null;
        }

        private Boolean SaveRecords(string Mode)
        {
            //DataTable dtXml = TabletoXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vHospiName = "", vLedgerDescId = "";
            string vSubId = Convert.ToString(ViewState["HospiId"]), vSchBrCode = "";
            Int32 vErr = 0, vRec = 0, vHospiId = 0;
            double vHospiAmt = 0, vHospiAmtFor2Yr = 0, vHospiAmtForAbove2Yr = 0, vHospiAmtFor15M = 0, vHospiAmtFor18M = 0,
            vFixedAmt1st = 0, vFixedAmt2nd = 0, vFixedAmtAbv2 = 0, vFixedAmt15M = 0, vFixedAmt18M = 0;
            string vMsg = "";
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            try
            {

                vHospiAmt = Convert.ToDouble(txtHospiAmt.Text.Trim());
                vHospiAmtFor15M = Convert.ToDouble(txtHospiAmt15M.Text.Trim());
                vHospiAmtFor18M = Convert.ToDouble(txtHospiAmt18M.Text.Trim());
                vHospiAmtFor2Yr = Convert.ToDouble(txtHospiAmt2.Text.Trim());
                vHospiAmtForAbove2Yr = Convert.ToDouble(txtHospiAmtAbv2.Text.Trim());
                vHospiId = Convert.ToInt32(ViewState["HospiId"]);
                vHospiName = Convert.ToString(txtHospiCashName.Text);
                vLedgerDescId = ddlHospiLedger.SelectedValue.ToString();
                vSchBrCode = TabletoString();
                DateTime vAppDt = gblFuction.setDate(txtEffDt.Text);
                vFixedAmt1st = Convert.ToDouble(txtFixedAmt1st.Text == "" ? "0" : txtFixedAmt1st.Text);
                vFixedAmt2nd = Convert.ToDouble(txtFixedAmt2nd.Text == "" ? "0" : txtFixedAmt2nd.Text);
                vFixedAmtAbv2 = Convert.ToDouble(txtFixedAmtAbv2.Text == "" ? "0" : txtFixedAmtAbv2.Text);
                vFixedAmt15M = Convert.ToDouble(txtFixedAmt15M.Text == "" ? "0" : txtFixedAmt15M.Text);
                vFixedAmt18M = Convert.ToDouble(txtFixedAmt18M.Text == "" ? "0" : txtFixedAmt18M.Text);

                if (Mode == "Save")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("HospiMst", "HospiName", txtHospiCashName.Text.Replace("'", "''"), "", "", "HospiId", vSubId, "Save");

                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Hospi Cash can not be Duplicate...");
                        return false;
                    }

                    if (cbActive.Checked == true)
                    {
                        oLS = new CLoanScheme();
                        vMsg = oLS.ChkHospiApplicableBranch(vSchBrCode, Mode, 0);
                        if (vMsg != "OK")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }

                    oLS = new CLoanScheme();
                    vErr = oLS.InsertHospiCash(ref vHospiId, vHospiName, vHospiAmt, vHospiAmtFor2Yr, vHospiAmtForAbove2Yr, vAppDt, vLedgerDescId,
                         Convert.ToInt32(Session[gblValue.UserId].ToString()), "I", "Save", cbActive.Checked == false ? "N" : "Y", "N",
                         vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, vSchBrCode, vHospiAmtFor15M, vFixedAmt15M, vHospiAmtFor18M, vFixedAmt18M);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["HospiId"] = vHospiId;
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
                    vRec = oGbl.ChkDuplicate("HospiMst", "HospiName", txtHospiCashName.Text.Replace("'", "''"), "", "", "HospiId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Hospi Cash Name can not be Duplicate...");
                        return false;
                    }
                    if (cbActive.Checked == true)
                    {
                        oLS = new CLoanScheme();
                        vMsg = oLS.ChkHospiApplicableBranch(vSchBrCode, Mode, vHospiId);
                        if (vMsg != "OK")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }

                    vErr = oLS.InsertHospiCash(ref vHospiId, vHospiName, vHospiAmt, vHospiAmtFor2Yr, vHospiAmtForAbove2Yr, vAppDt, vLedgerDescId,
                         Convert.ToInt32(Session[gblValue.UserId].ToString()), "E", "Edit", cbActive.Checked == false ? "N" : "Y", "N",
                         vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, vSchBrCode, vHospiAmtFor15M, vFixedAmt15M, vHospiAmtFor18M, vFixedAmt18M);

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
                        oLS.InsertHospiCash(ref vHospiId, vHospiName, vHospiAmt, vHospiAmtFor2Yr, vHospiAmtForAbove2Yr, vAppDt, vLedgerDescId,
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "D", "Del", cbActive.Checked == false ? "N" : "Y", "N",
                        vFixedAmt1st, vFixedAmt2nd, vFixedAmtAbv2, vSchBrCode, vHospiAmtFor15M, vFixedAmt15M, vHospiAmtFor18M, vFixedAmt18M);
                        vResult = true;
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
            Int32 vHospiId = 0;
            DataTable dt = null;
            CLoanScheme oLS = null;
            try
            {
                vHospiId = Convert.ToInt32(e.CommandArgument);
                ViewState["HospiId"] = vHospiId;
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
                    dt = oLS.GetHospiCashById(vHospiId);
                    if (dt.Rows.Count > 0)
                    {
                        txtHospiCashName.Text = Convert.ToString(dt.Rows[0]["HospiName"]).Trim();
                        txtHospiAmt.Text = Convert.ToString(dt.Rows[0]["HospiAmt"]).Trim();
                        txtHospiAmt2.Text = Convert.ToString(dt.Rows[0]["HospiAmtFor2Yr"]).Trim();
                        txtHospiAmtAbv2.Text = Convert.ToString(dt.Rows[0]["HospiAmtForAbove2Yr"]).Trim();
                        txtEffDt.Text = Convert.ToString(dt.Rows[0]["EffectiveDt"]).ToString();
                        ddlHospiLedger.SelectedIndex = ddlHospiLedger.Items.IndexOf(ddlHospiLedger.Items.FindByValue(dt.Rows[0]["LedgerDescId"].ToString().Trim()));

                        cbActive.Checked = Convert.ToString(dt.Rows[0]["Active"]).Trim() == "N" ? false : true;
                        txtFixedAmt1st.Text = Convert.ToString(dt.Rows[0]["FixedAmt1st"]).Trim();
                        txtFixedAmt2nd.Text = Convert.ToString(dt.Rows[0]["FixedAmt2nd"]).Trim();
                        txtFixedAmtAbv2.Text = Convert.ToString(dt.Rows[0]["FixedAmtAbv2"]).Trim();

                        txtHospiAmt15M.Text = Convert.ToString(dt.Rows[0]["HospiAmt15M"]).Trim();
                        txtFixedAmt15M.Text = Convert.ToString(dt.Rows[0]["FixedAmt15M"]).Trim();

                        txtHospiAmt18M.Text = Convert.ToString(dt.Rows[0]["HospiAmt18M"]).Trim();
                        txtFixedAmt18M.Text = Convert.ToString(dt.Rows[0]["FixedAmt18M"]).Trim();

                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadBranch("HEdit", vHospiId);
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
    }
}