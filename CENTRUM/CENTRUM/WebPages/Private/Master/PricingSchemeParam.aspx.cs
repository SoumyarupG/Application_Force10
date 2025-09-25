using System;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;


namespace CENTRUM.WebPages.Private.Master
{
    public partial class PricingSchemeParam : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                LoadGrid(0);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {

                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Pricing Scheme Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPricingSchemeParameter);
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
                    btnAdd.Visible = true;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnAdd.Visible = true;
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pricing Scheme Parameter", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtComponentName");
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtComponentName");
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

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {

            txtEffectiveDate.Text = Session[gblValue.LoginDate].ToString();
            txtComponentName.Text = "";
            txtCostofDeposits.Text = "0.00";
            txtCRRNegativeCarry.Text = "0.00";
            txtOperatingCost.Text = "0.00";
            txtCostofFunds.Text = "0.00";
            txtCreditCost.Text = "0.00";
            txtBusinessMargin.Text = "0.00";
            txtTotalBaseInterest.Text = "0.00";

            lblUser.Text = "";
            lblDate.Text = "";

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {

            txtEffectiveDate.Enabled = Status;
            txtComponentName.Enabled = Status;
            txtEffectiveDate.Enabled = Status;
            txtCostofDeposits.Enabled = Status;
            txtCRRNegativeCarry.Enabled = Status;
            txtOperatingCost.Enabled = Status;
            txtCreditCost.Enabled = Status;
            txtBusinessMargin.Enabled = Status;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CRiskPremium oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CRiskPremium();
                dt = oIC.GetPricingSchemePG(pPgIndx, ref vRows);
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotalPages.Text = "0";
                    lblCurrentPage.Text = "0";
                }
                else
                {
                    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                    lblCurrentPage.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = true;
                    else
                        Btn_Next.Enabled = false;
                }
                else
                {
                    Btn_Previous.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
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
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
                    break;
            }
            LoadGrid(cPgNo);
            tabLnScheme.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnScheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPricingSchemeID = 0;
            DataTable dt = null;
            CRiskPremium oLS = null;
            try
            {
                vPricingSchemeID = Convert.ToInt32(e.CommandArgument);
                ViewState["PricingSchemeId"] = vPricingSchemeID;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvLnScheme.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oLS = new CRiskPremium();
                    dt = oLS.GetPricingSchemeById(vPricingSchemeID);
                    if (dt.Rows.Count > 0)
                    {

                        txtEffectiveDate.Text = Convert.ToString(dt.Rows[0]["PricingSchemeEffectiveDt"]).Trim();
                        txtComponentName.Text = Convert.ToString(dt.Rows[0]["ComponentName"]).Trim();
                        txtCostofDeposits.Text = Convert.ToString(dt.Rows[0]["CostofDeposits"]).Trim();
                        txtCRRNegativeCarry.Text = Convert.ToString(dt.Rows[0]["CRRNegativeCarry"]).Trim();
                        txtOperatingCost.Text = Convert.ToString(dt.Rows[0]["OperatingCost"]).Trim();
                        txtCostofFunds.Text = Convert.ToString(dt.Rows[0]["CostofFunds"]).Trim();
                        txtCreditCost.Text = Convert.ToString(dt.Rows[0]["CreditCost"]).Trim();
                        txtBusinessMargin.Text = Convert.ToString(dt.Rows[0]["BusinessMargin"]).Trim();
                        txtTotalBaseInterest.Text = Convert.ToString(dt.Rows[0]["TotalBaseInterest"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabLnScheme.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField()
        {
            bool vRes = true;

            if (gblFuction.IsDate(txtEffectiveDate.Text) == false)
            {
                gblFuction.MsgPopup("EffectiveDate Date should be in DD/MM/YYYY Format");
                gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtIntroDt");
                return vRes = false;
            }


            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["PricingSchemeId"]), vErrorMsg = "";
            Int32 vErr = 0, vPricingSchemeID = 0;
            
            CRiskPremium oLS = null;

            try
            {
                if (ValidateField() == false)
                    return false;
                


                vPricingSchemeID = Convert.ToInt32(ViewState["PricingSchemeId"]);

                if (Mode == "Save")
                {
                    oLS = new CRiskPremium();
                    vErr = oLS.InsertPricingScheme(ref vPricingSchemeID, txtComponentName.Text.Trim(), gblFuction.setDate(txtEffectiveDate.Text),
                        Convert.ToDouble(txtCostofDeposits.Text), Convert.ToDouble(txtCRRNegativeCarry.Text),Convert.ToDouble(txtOperatingCost.Text), 
                        Convert.ToDouble(txtCostofFunds.Text), Convert.ToDouble(txtCreditCost.Text),Convert.ToDouble(txtBusinessMargin.Text),
                        Convert.ToDouble(txtTotalBaseInterest.Text), Mode, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate("01/01/1900"), ref vErrorMsg);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["PricingSchemeId"] = vPricingSchemeID;
                    }

                    else
                    {
                        gblFuction.MsgPopup(vErrorMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oLS = new CRiskPremium();

                    vErr = oLS.InsertPricingScheme(ref vPricingSchemeID, txtComponentName.Text.Trim(), gblFuction.setDate(txtEffectiveDate.Text),
                        Convert.ToDouble(txtCostofDeposits.Text), Convert.ToDouble(txtCRRNegativeCarry.Text), Convert.ToDouble(txtOperatingCost.Text),
                        Convert.ToDouble(txtCostofFunds.Text), Convert.ToDouble(txtCreditCost.Text), Convert.ToDouble(txtBusinessMargin.Text),
                        Convert.ToDouble(txtTotalBaseInterest.Text), Mode, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate("01/01/1900"), ref vErrorMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrorMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oLS = new CRiskPremium();

                    vErr = oLS.InsertPricingScheme(ref vPricingSchemeID, txtComponentName.Text.Trim(), gblFuction.setDate(txtEffectiveDate.Text),
                        Convert.ToDouble(txtCostofDeposits.Text), Convert.ToDouble(txtCRRNegativeCarry.Text), Convert.ToDouble(txtOperatingCost.Text),
                        Convert.ToDouble(txtCostofFunds.Text), Convert.ToDouble(txtCreditCost.Text), Convert.ToDouble(txtBusinessMargin.Text),
                        Convert.ToDouble(txtTotalBaseInterest.Text), Mode, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate("01/01/1900"), ref vErrorMsg);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrorMsg);
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
                oLS = null;

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
            ViewState["PricingSchemeId"] = null;
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
                tabLnScheme.ActiveTabIndex = 1;
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
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    tabLnScheme.ActiveTabIndex = 0;
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
                StatusButton("Show");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }

    }
}

