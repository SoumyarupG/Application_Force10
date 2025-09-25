using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class Holiday : CENTRUMBase
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
                ViewState["State"] = null;
                popState();
                LoadGrid(0);
                tbHDay.ActiveTabIndex = 0;
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
                this.PageHeading = "Holiday List";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHoliday);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;  
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Holiday List", false);
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
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();                    
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);                    
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);                    
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
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
            try
            {
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbHDay.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                rbHT_SelectedIndexChanged(sender, e);
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
                    LoadGrid(1);
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
            try
            {                
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                rbHT_SelectedIndexChanged(sender, e);
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
            tbHDay.ActiveTabIndex = 0;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vDistId = 0, vTotRows = 0;
            string vBrCode = "";
            CHoliday oHoli = null;
            try
            {
                vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oHoli = new CHoliday();
                dt = oHoli.GetHolidayPG(vBrCode, pPgIndx, ref vTotRows);
                gvHDay.DataSource = dt;
                gvHDay.DataBind();
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
                oHoli = null;
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
            tbHDay.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvHDay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vHoliCode = 0;
            DataTable dt = null;
            CHoliday oHoli = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                vHoliCode = Convert.ToInt32(e.CommandArgument);
                ViewState["vHoliCode"] = vHoliCode;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvHDay.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oHoli = new CHoliday();
                    dt = oHoli.GetHolidayById(vBrCode,vHoliCode);
                    if (dt.Rows.Count > 0)
                    {
                        rbHT.SelectedValue=Convert.ToString(dt.Rows[0]["Type"]);
                        txtHday.Text = Convert.ToString(dt.Rows[0]["HolidayName"]).Replace("''", "'");
                        if (dt.Rows[0]["HoliDay_Date"].ToString() != "01/01/1900")
                            txtDt.Text = Convert.ToString(dt.Rows[0]["HoliDay_Date"]);
                        ddlWDay.SelectedIndex = ddlWDay.Items.IndexOf(ddlWDay.Items.FindByValue(dt.Rows[0]["DayName"].ToString()));
                        txtSkipDays.Text = Convert.ToString(dt.Rows[0]["SkipDays"]);
                        txtHDesc.Text = Convert.ToString(dt.Rows[0]["Description"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateID"].ToString()));
                        tbHDay.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oHoli = null;
            }
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
            string vBrCode = Session[gblValue.BrnchCode].ToString(), vOpt = "", vErrMsg = "";
            Int32 vHoliId = Convert.ToInt32(ViewState["vHoliCode"]);
            Int32 vErr = 0, vRec = 0;
            double vSkipDays = 0;
            CHoliday oHoli = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vOpt = rbHT.SelectedValue;
                if (rbHT.SelectedValue == "S")
                {
                    if (txtSkipDays.Text != "" && txtSkipDays.Text != "0")
                    {
                        vSkipDays = Convert.ToDouble(txtSkipDays.Text);
                    }
                    else
                    {
                        gblFuction.MsgPopup("Enter Skip Days");
                        return false;
                    }
                    //if (Convert.ToInt32(txtSkipDays.Text) % 7 != 0)
                    //{
                    //    gblFuction.MsgPopup("Enter Skip Days in Multiple of 7");
                    //    return false;
                    //}
                }
                    
                if (Mode == "Save")
                {
                    oHoli = new CHoliday();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("HolidayMst", "HolidayName", txtHday.Text.Replace("'", "''"), "BranchCode", vBrCode, "HolidayCode", vHoliId.ToString(), "Save", "HoliDay_Date", Convert.ToString(gblFuction.setDate(txtDt.Text).Year));
                    vRec = oGbl.ChkDuplicateHoli("HolidayMst", "HolidayName", txtHday.Text.Replace("'", "''"), "BranchCode", vBrCode, "HolidayCode", vHoliId.ToString(), "Save", "HoliDay_Date", Convert.ToString(gblFuction.setDate(txtDt.Text).Year), "StateID", Convert.ToInt32(ddlState.SelectedValue));
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Holiday Can not be Duplicate...");
                        return false;
                    }
                    vRec = oHoli.ChkCollectionDate(gblFuction.setDate(txtDt.Text), vBrCode, Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlWDay.SelectedValue));
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Collection Already Done...");
                        return false;
                    }

                    vErr = oHoli.InsertHoli(vHoliId, txtHday.Text.Replace("'", "''"), rbHT.SelectedValue, Convert.ToInt32(ddlWDay.SelectedValue),
                                    gblFuction.setDate(txtDt.Text), txtHDesc.Text.Replace("'", "''"), vSkipDays, vBrCode, 
                                    Convert.ToInt32(ddlState.SelectedValue), this.UserID, "Save",ref vErrMsg);
                    if (vErr > 0)
                    {
                        //ViewState["vHoliCode"] = vHoliId;
                        vResult = true;
                    }
                    else
                    {
                        if (vErrMsg.Length > 0)
                        {
                            gblFuction.MsgPopup(vErrMsg);
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                        }
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oHoli = new CHoliday();
                    oGbl = new CGblIdGenerator();
                    ///vRec = oGbl.ChkDuplicate("HolidayMst", "HolidayName", txtHday.Text.Replace("'", "''"), "", "", "HolidayCode", vHoliId.ToString(), "Edit");
                    vRec = oGbl.ChkDuplicateHoli("HolidayMst", "HolidayName", txtHday.Text.Replace("'", "''"), "BranchCode", vBrCode, "HolidayCode", vHoliId.ToString(), "Edit", "HoliDay_Date", Convert.ToString(gblFuction.setDate(txtDt.Text).Year), "StateID", Convert.ToInt32(ddlState.SelectedValue));
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Holiday Can not be Duplicate...");
                        return false;
                    }
                    vErr = oHoli.InsertHoli(vHoliId, txtHday.Text.Replace("'", "''"), rbHT.SelectedValue, Convert.ToInt32(ddlWDay.SelectedValue),
                                    gblFuction.setDate(txtDt.Text), txtHDesc.Text.Replace("'", "''"), vSkipDays, vBrCode,
                                    Convert.ToInt32(ddlState.SelectedValue), this.UserID, "Edit", ref vErrMsg);
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
                    oHoli = new CHoliday();
                    vRec = oHoli.ChkCollectionDate(gblFuction.setDate(txtDt.Text), vBrCode,Convert.ToInt32(ddlState.SelectedValue), Convert.ToInt32(ddlWDay.SelectedValue));
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Collection Already Done Holiday Cannot be delete...");
                        return false;
                    }
                    vErr = oHoli.InsertHoli(vHoliId, txtHday.Text.Replace("'", "''"), rbHT.SelectedValue, Convert.ToInt32(ddlWDay.SelectedValue),
                                    gblFuction.setDate(txtDt.Text), txtHDesc.Text.Replace("'", "''"), vSkipDays, vBrCode,
                                    Convert.ToInt32(ddlState.SelectedValue), this.UserID, "Delet",ref vErrMsg);
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
                return vResult;
            }          
            finally
            {
                oHoli = null;
                oGbl = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbHT_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (rbHT.SelectedValue == "S")
            //{
            //    txtDt.Enabled = true;
            //    ddlWDay.Enabled = false;
            //}
            //if (rbHT.SelectedValue == "W")
            //{
            //    ddlWDay.Enabled = true;
            //    ddlWDay.SelectedIndex = -1;
            //    ceDt.Dispose();
            //    txtDt.Text = "";
            //    txtDt.Enabled = false;
            //}

            if (rbHT.SelectedValue == "S")
            {
                txtSkipDays.Enabled = true;
            }
            if (rbHT.SelectedValue != "S")
            {
                txtSkipDays.Enabled = false;
                txtSkipDays.Text  = "-1";
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDt_TextChanged(object sender, EventArgs e)
        {
            if (txtDt.Text != "")
            {
                ddlWDay.Enabled = false;
                ceDt.SelectedDate = gblFuction.setDate(txtDt.Text);
                ddlWDay.SelectedIndex = ddlWDay.Items.IndexOf(ddlWDay.Items.FindByText(ceDt.SelectedDate.Value.DayOfWeek.ToString().ToUpper()));
            }
            else
            {
                ddlWDay.SelectedIndex = -1;
                ddlWDay.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            rbHT.Enabled = Status;
            txtHday.Enabled = Status;
            txtDt.Enabled = Status;
            txtSkipDays.Enabled = Status;
            ddlWDay.Enabled = Status;
            txtHDesc.Enabled = Status;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
                ddlState.Enabled = Status;
            else
                ddlState.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtHday.Text = "";
            txtDt.Text = "";            
            txtHDesc.Text = "";
            ddlWDay.SelectedIndex = -1;
            txtSkipDays.Text = "-1";
            lblDate.Text = "";
            lblUser.Text = "";
            if (Session[gblValue.BrnchCode].ToString() == "0000")
                ddlState.SelectedIndex = -1;
            else
                SetState();
        }
        /// <summary>
        /// 
        /// </summary>
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
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
        /// <summary>
        /// 
        /// </summary>
        private void SetState()
        {
            string vBCode = "";
            DataTable dt = null;
            CBranch oBr = null;
            try
            {
                vBCode = Session[gblValue.BrnchCode].ToString();
                oBr = new CBranch();
                dt = oBr.GetBranchDetails(vBCode);
                if (dt.Rows.Count > 0)
                {
                    ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateID"].ToString()));
                }
            }
            finally
            {
                dt = null;
                oBr = null;
            }
        }
    }
}