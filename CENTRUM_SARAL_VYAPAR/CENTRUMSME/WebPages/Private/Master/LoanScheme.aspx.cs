using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUM_SARALVYAPAR.WebPages.Private.Master
{
    public partial class LoanScheme : CENTRUMBAse
    {
        protected int cPgNo = 1;

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
                StatusButton("View");
                LoadGrid(0);
                tbSchm.ActiveTabIndex = 0;
                popLoanProduct();
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
                this.PageHeading = "Loan Scheme";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuScheme);
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
        private void ClearControls()
        {
            txtLnScheme.Text = "";
            ddlLnProd.SelectedIndex = -1;
            txtLnAmt.Text = "0.0";
            txtMinLnAmt.Text = "0.0";
            txtPrvLnAmt.Text = "0.0";
            txtLnCycle.Text = "0";
            txtMoratoriumP.Text = "0";
            txtIntCalDays.Text = "0";
            lblUser.Text = "";
            lblDate.Text = "";
            chkBank.Checked = false;
            chkHO.Checked = false;
            //ChkNeft.Checked = false;
            chkSelectAll.Checked = false;
            chkOSApp.Checked = false;
            chkCondiTopUp.Checked = false;
            txtPOSAmt.Text = "0";
            chkSelectAll2.Checked = false;
            chkTrRed.Checked = false;
            chkManFileUpload.Checked = false;
            chkMemVerification.Checked = false;
            chkEmiEligYN.Checked = false;
            chkSaralSchmYN.Checked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {
            txtLnScheme.Enabled = Status;
            ddlLnProd.Enabled = Status;
            txtLnAmt.Enabled = Status;
            txtMinLnAmt.Enabled = Status;
            txtPrvLnAmt.Enabled = Status;
            txtLnCycle.Enabled = Status;
            txtIntCalDays.Enabled = Status;
            txtMoratoriumP.Enabled = Status;
            chkBank.Enabled = Status;
            chkHO.Enabled = Status;
            //ChkNeft.Enabled = Status;
            gvBranch.Enabled = Status;
            chkSelectAll.Enabled = Status;
            chkOSApp.Enabled = Status;
            chkCondiTopUp.Enabled = Status;
            txtPOSAmt.Enabled = Status;
            gvNEFTBranch.Enabled = Status;
            chkTrRed.Enabled = Status;
            chkManFileUpload.Enabled = Status;
            chkMemVerification.Enabled = Status;
            chkEmiEligYN.Enabled = Status;
            chkSaralSchmYN.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void popLoanProduct()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ProductId", "Product", "LoanProductMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlLnProd.DataSource = dt;
                ddlLnProd.DataTextField = "Product";
                ddlLnProd.DataValueField = "ProductId";
                ddlLnProd.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLnProd.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CLoanScheme();
                dt = oIC.GetLoanSchemePG(pPgIndx, ref vRows);
                gvSchm.DataSource = dt.DefaultView;
                gvSchm.DataBind();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
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
            tbSchm.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSchm_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    foreach (GridViewRow gr in gvSchm.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLS = new CLoanScheme();
                    dt = oLS.GetLoanSchemeById(vLoanTypeId);
                    if (dt.Rows.Count > 0)
                    {
                        txtLnScheme.Text = Convert.ToString(dt.Rows[0]["LoanType"]).Trim();
                        ddlLnProd.SelectedIndex = ddlLnProd.Items.IndexOf(ddlLnProd.Items.FindByValue(dt.Rows[0]["ProductId"].ToString().Trim()));
                        txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]).Trim();
                        txtMinLnAmt.Text = Convert.ToString(dt.Rows[0]["MinLoanAmt"]).Trim();
                        txtPrvLnAmt.Text = Convert.ToString(dt.Rows[0]["PrvLnAmt"]).Trim();
                        txtLnCycle.Text = Convert.ToString(dt.Rows[0]["LoanCycle"]).Trim();
                        txtIntCalDays.Text = Convert.ToString(dt.Rows[0]["Denominator"]).Trim();
                        txtMoratoriumP.Text = Convert.ToString(dt.Rows[0]["FirstDueDayGap"]).Trim();
                        chkBank.Checked = Convert.ToString(dt.Rows[0]["IsBank"]).Trim() == "N" ? false : true;
                        chkHO.Checked = Convert.ToString(dt.Rows[0]["IsHO"]).Trim() == "N" ? false : true;
                        // ChkNeft.Checked = Convert.ToString(dt.Rows[0]["IsNEFT"]).Trim() == "N" ? false : true;
                        chkOSApp.Checked = Convert.ToString(dt.Rows[0]["chkOSApp"]).Trim() == "N" ? false : true;
                        chkCondiTopUp.Checked = Convert.ToString(dt.Rows[0]["chkCondiTopUp"]).Trim() == "N" ? false : true;
                        chkTrRed.Checked = Convert.ToString(dt.Rows[0]["TrueRedYN"]).Trim() == "N" ? false : true;
                        chkManFileUpload.Checked = Convert.ToString(dt.Rows[0]["ManualFileUploadYN"]).Trim() == "N" ? false : true;
                        chkMemVerification.Checked = Convert.ToString(dt.Rows[0]["MemVerificationYN"]).Trim() == "N" ? false : true;
                        chkEmiEligYN.Checked = Convert.ToString(dt.Rows[0]["EmiEligibilityChkYN"]).Trim() == "N" ? false : true;
                        chkSaralSchmYN.Checked = Convert.ToString(dt.Rows[0]["SaralTopUpSchmYN"]).Trim() == "N" ? false : true;
                        txtPOSAmt.Text = Convert.ToString(dt.Rows[0]["POSAmt"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadBranch("Edit", vLoanTypeId);
                        LoadNEFTBranch("Edit", vLoanTypeId);
                        tbSchm.ActiveTabIndex = 1;
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
        private string TabletoNEFTString()
        {
            string vNEFTBrCode = "";
            foreach (GridViewRow gr in gvNEFTBranch.Rows)
            {
                CheckBox chkNEFTStatus = (CheckBox)gr.FindControl("chkNEFTStatus");
                if (chkNEFTStatus.Checked == true)
                    vNEFTBrCode = vNEFTBrCode + (gr.Cells[2].Text) + ",";
            }
            return vNEFTBrCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dtXml = TabletoXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vSchBrCode = "", vNEFTBrCode = "", vTrueRedYn = "N", vManualFileUpldYn = "N";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0, vLnCycle = 0;
            double vLnAmt = 0, vPrvLnAmt = 0, vMinLnAmt = 0;
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            try
            {

                vLnAmt = Convert.ToDouble(txtLnAmt.Text.Trim());
                vMinLnAmt = Convert.ToDouble(txtMinLnAmt.Text.Trim());
                vPrvLnAmt = Convert.ToDouble(txtPrvLnAmt.Text.Trim());
                vLnCycle = Convert.ToInt32(txtLnCycle.Text.Trim());
                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);
                vSchBrCode = TabletoString();
                vNEFTBrCode = TabletoNEFTString();
                if (vLnAmt < vMinLnAmt)
                {
                    gblFuction.MsgPopup("Maximum Loan Amount should be more than Minimum Loan Amount...");
                    return false;
                }

                if (Mode == "Save")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanTypeMst", "LoanType", txtLnScheme.Text.Replace("'", "''"), "", "", "LoanTypeId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Loan Scheme Can not be Duplicate...");
                        return false;
                    }
                    vErr = oLS.InsertLoanScheme(ref vLoanTypeId, txtLnScheme.Text.Replace("'", "''"), Convert.ToInt32(ddlLnProd.SelectedValue), vMinLnAmt,
                        vLnAmt, Convert.ToInt32(txtIntCalDays.Text.ToString()), Convert.ToInt32(txtMoratoriumP.Text.ToString()), vPrvLnAmt, vLnCycle,
                        chkBank.Checked == false ? "N" : "Y", chkHO.Checked == false ? "N" : "Y", Convert.ToInt32(Session[gblValue.UserId]), "I", "Save",
                        vSchBrCode, chkOSApp.Checked == false ? "N" : "Y", chkCondiTopUp.Checked == false ? "N" : "Y", Convert.ToDouble(txtPOSAmt.Text),
                        vNEFTBrCode, chkTrRed.Checked == true ? "Y" : "N", chkManFileUpload.Checked == true ? "Y" : "N", chkMemVerification.Checked == true ? "Y" : "N",
                        chkSaralSchmYN.Checked == true ? "Y" : "N", chkEmiEligYN.Checked == true ? "Y" : "N");
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
                    vRec = oGbl.ChkDuplicate("LoanTypeMst", "LoanType", txtLnScheme.Text.Replace("'", "''"), "", "", "LoanTypeId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Loan Scheme Can not be Duplicate...");
                        return false;
                    }
                    vErr = oLS.InsertLoanScheme(ref vLoanTypeId, txtLnScheme.Text.Replace("'", "''"), Convert.ToInt32(ddlLnProd.SelectedValue), vMinLnAmt, vLnAmt,
                        Convert.ToInt32(txtIntCalDays.Text.ToString()), Convert.ToInt32(txtMoratoriumP.Text.ToString()), vPrvLnAmt, vLnCycle,
                        chkBank.Checked == false ? "N" : "Y", chkHO.Checked == false ? "N" : "Y", Convert.ToInt32(Session[gblValue.UserId]), "E", "Edit", vSchBrCode,
                        chkOSApp.Checked == false ? "N" : "Y", chkCondiTopUp.Checked == false ? "N" : "Y", Convert.ToDouble(txtPOSAmt.Text), vNEFTBrCode,
                         chkTrRed.Checked == true ? "Y" : "N", chkManFileUpload.Checked == true ? "Y" : "N", chkMemVerification.Checked == true ? "Y" : "N",
                         chkSaralSchmYN.Checked == true ? "Y" : "N", chkEmiEligYN.Checked == true ? "Y" : "N");
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
                        oLS.InsertLoanScheme(ref vLoanTypeId, txtLnScheme.Text.Replace("'", "''"), Convert.ToInt32(ddlLnProd.SelectedValue), vMinLnAmt, vLnAmt, Convert.ToInt32(txtIntCalDays.Text.ToString()), Convert.ToInt32(txtMoratoriumP.Text.ToString()), vPrvLnAmt, vLnCycle, chkBank.Checked == false ? "N" : "Y", chkHO.Checked == false ? "N" : "Y",
                             Convert.ToInt32(Session[gblValue.UserId]), "D", "Del", vSchBrCode, chkOSApp.Checked == false ? "N" : "Y", chkCondiTopUp.Checked == false ? "N" : "Y", Convert.ToDouble(txtPOSAmt.Text), vNEFTBrCode,
                             chkTrRed.Checked == true ? "Y" : "N", chkManFileUpload.Checked == true ? "Y" : "N", chkMemVerification.Checked == true ? "Y" : "N", chkSaralSchmYN.Checked == true ? "Y" : "N", chkEmiEligYN.Checked == true ? "Y" : "N");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable TabletoXml()
        {
            DataTable dt = new DataTable("Alloc");
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["LoanTypeId"] = null;
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
                ViewState["StateEdit"] = "Add";
                StatusButton("Add");
                ClearControls();
                tbSchm.ActiveTabIndex = 1;
                LoadBranch("Add", 0);
                LoadNEFTBranch("Add", 0);
            }
            catch (Exception ex)
            {
                throw ex;
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
        private void LoadNEFTBranch(string pMode, Int32 vLnTypId)
        {
            DataTable dt = null;
            CLoanScheme oDs = null;
            try
            {
                oDs = new CLoanScheme();
                dt = oDs.GetNEFTBranchForLoanType(vLnTypId, pMode);
                gvNEFTBranch.DataSource = dt;
                gvNEFTBranch.DataBind();

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
        protected void gvNEFTBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkNEFTStatus = (CheckBox)e.Row.FindControl("chkNEFTStatus");
                if (e.Row.Cells[4].Text.Trim() == "1")
                {
                    chkNEFTStatus.Checked = true;
                }
                else
                {
                    chkNEFTStatus.Checked = false;
                }
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    tbSchm.ActiveTabIndex = 0;
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
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                if (chkOSApp.Checked == true)
                {

                    chkCondiTopUp.Enabled = true;
                    txtPOSAmt.Enabled = true;

                }
                if (chkOSApp.Checked == false)
                {
                    chkCondiTopUp.Enabled = false;
                    txtPOSAmt.Enabled = false;

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
            EnableControl(false);
            LoadGrid(0);
            tbSchm.ActiveTabIndex = 0;
            ClearControls();
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }

        protected void ChkOSApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOSApp.Checked == true)
            {

                chkCondiTopUp.Enabled = true;
                txtPOSAmt.Enabled = true;
                chkCondiTopUp.Checked = false;
                txtPOSAmt.Text = "0";
            }
            if (chkOSApp.Checked == false)
            {
                chkCondiTopUp.Enabled = false;
                txtPOSAmt.Enabled = false;
                chkCondiTopUp.Checked = false;
                txtPOSAmt.Text = "0";
            }
        }
    }
}