using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class TrainingSchedule : CENTRUMBase
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
                
                LoadGrid(0);
                popDept();
                popModule();
                popBranch();
                popTrainer();
                popCenter();
                if(Convert.ToString(Session[gblValue.BrnchCode])=="0000")
                    StatusButton("View");
                else
                    StatusButton("ViewOnly");
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
                this.PageHeading = "Training Schedule";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTrnSchule);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Training Schedule", false);
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
                case "ViewOnly":
                    btnAdd.Enabled = false;
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlMonth.Enabled = Status;
            ddlYear.Enabled = Status;
            ddlDept.Enabled = Status;
            ddlModule.Enabled = Status;
            ddlTrainer.Enabled = Status;
            ddlVenue.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlCenter.Enabled = Status;
            txtTrainerNM.Enabled = Status;
            txtVenueNM.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlMonth.SelectedIndex = -1;
            ddlYear.SelectedIndex = -1;
            ddlDept.SelectedIndex = -1;
            ddlModule.SelectedIndex = -1;
            ddlTrainer.SelectedIndex = -1;
            ddlVenue.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            txtTrainerNM.Text = "";
            txtVenueNM.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CTraining oFS = null;
            Int32 vRows = 0;
            try
            {
                oFS = new CTraining();
                dt = oFS.GetTrainingSchedulePG(pPgIndx, ref vRows, this.UserID);
                gvTrainSchedule.DataSource = dt.DefaultView;
                gvTrainSchedule.DataBind();
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
                oFS = null;
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
            tbTrainSchedule.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTrainSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vScheduleId = "";
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            DataTable dt = null;
            CTraining oFS = null;
            try
            {
                vScheduleId = Convert.ToString(e.CommandArgument);
                ViewState["ScheduleId"] = vScheduleId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvTrainSchedule.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oFS = new CTraining();
                    dt = oFS.GetTrainingSchebyId(vScheduleId);
                    if (dt.Rows.Count > 0)
                    {
                        //txtModule.Text = Convert.ToString(dt.Rows[0]["ModuleName"]).Trim();

                        ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByText(Convert.ToString(dt.Rows[0]["Month"])));
                        ddlYear.SelectedIndex = ddlYear.Items.IndexOf(ddlYear.Items.FindByText(Convert.ToString(dt.Rows[0]["Year"])));
                        ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByValue(Convert.ToString(dt.Rows[0]["DeptId"])));
                        ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(dt.Rows[0]["ModuleId"])));
                        ddlTrainer.SelectedIndex = ddlTrainer.Items.IndexOf(ddlTrainer.Items.FindByValue(Convert.ToString(dt.Rows[0]["EOId"])));
                        popDesigGrade(ddlTrainer.SelectedValue);
                        ddlVenue.SelectedIndex = ddlVenue.Items.IndexOf(ddlVenue.Items.FindByValue(Convert.ToString(dt.Rows[0]["VenueId"])));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[0]["Branch"])));
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(Convert.ToString(dt.Rows[0]["CenterId"])));
                        txtTrainerNM.Text = Convert.ToString(dt.Rows[0]["TrainerNM"]);
                        txtVenueNM.Text = Convert.ToString(dt.Rows[0]["VenueNM"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbTrainSchedule.ActiveTabIndex = 1;
                        
                        if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                            StatusButton("Show");
                        else
                            StatusButton("ViewOnly");
                    }
                }
            }
            finally
            {
                dt = null;
                oFS = null;
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
            string vSubId = Convert.ToString(ViewState["ScheduleId"]), vScheduleId = "";
            string vBranchCode = Convert.ToString(Session[gblValue.BrnchCode]);
            Int32 vErr = 0, vRec = 0;
            CTraining oFS = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vScheduleId = Convert.ToString(ViewState["ScheduleId"]);
                if (Mode == "Save")
                {
                    oFS = new CTraining();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("TrainingMst", "ModuleName", txtModu.Text.Replace("'", "''"), "", "", "ModuleId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Module Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oFS.SaveTrainingSchedule(ref vScheduleId, ddlMonth.SelectedItem.Text, ddlYear.SelectedItem.Text, 
                           Convert.ToInt32(ddlDept.SelectedValue), ddlModule.SelectedValue,ddlTrainer.SelectedValue, 
                            txtTrainerNM.Text.Replace("'", "''"), ddlVenue.SelectedValue, ddlBranch.SelectedValue, 
                            ddlCenter.SelectedValue, txtVenueNM.Text.Replace("'", "''"), 
                            vBranchCode, this.UserID, "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["ScheduleId"] = vScheduleId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oFS = new CTraining();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("TrainingMst", "ModuleName", txtModule.Text.Replace("'", "''"), "", "", "ModuleId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Module Name Can not be Duplicate...");
                        return false;
                    }
                    vErr = oFS.SaveTrainingSchedule(ref vScheduleId, ddlMonth.SelectedItem.Text, ddlYear.SelectedItem.Text,
                           Convert.ToInt32(ddlDept.SelectedValue), ddlModule.SelectedValue, ddlTrainer.SelectedValue,
                            txtTrainerNM.Text.Replace("'", "''"), ddlVenue.SelectedValue, ddlBranch.SelectedValue,
                            ddlCenter.SelectedValue, txtVenueNM.Text.Replace("'", "''"),
                            vBranchCode, this.UserID, "E", "Edit");
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
                        oFS = new CTraining();
                        vErr = oFS.SaveTrainingSchedule(ref vScheduleId, ddlMonth.SelectedItem.Text, ddlYear.SelectedItem.Text,
                           Convert.ToInt32(ddlDept.SelectedValue), ddlModule.SelectedValue, ddlTrainer.SelectedValue,
                            txtTrainerNM.Text.Replace("'", "''"), ddlVenue.SelectedValue, ddlBranch.SelectedValue,
                            ddlCenter.SelectedValue, txtVenueNM.Text.Replace("'", "''"),
                            vBranchCode, this.UserID, "D", "Del");
                        if (vErr > 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
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
                oFS = null;
                oGbl = null;
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
            ViewState["ScheduleId"] = null;
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
                tbTrainSchedule.ActiveTabIndex = 1;
                if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                    StatusButton("Add");
                else
                    StatusButton("ViewOnly");
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
                    ClearControls();
                    tbTrainSchedule.ActiveTabIndex = 0;
                    if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                        StatusButton("Delete");
                    else
                        StatusButton("ViewOnly");
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
                if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                    StatusButton("Edit");
                else
                    StatusButton("ViewOnly");
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
            tbTrainSchedule.ActiveTabIndex = 0;
            ClearControls();
            EnableControl(false);
            LoadGrid(0);
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                StatusButton("View");
            else
                StatusButton("ViewOnly");
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
                LoadGrid(0);
                if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                    StatusButton("View");
                else
                    StatusButton("ViewOnly");
                ViewState["StateEdit"] = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popDept()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DeptID", "DeptName", "DeptMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDept.DataSource = dt;
                ddlDept.DataTextField = "DeptName";
                ddlDept.DataValueField = "DeptID";
                ddlDept.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDept.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }


        private void popModule()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ModuleId", "ModuleName", "TrainingMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlModule.DataSource = dt;
                ddlModule.DataTextField = "ModuleName";
                ddlModule.DataValueField = "ModuleId";
                ddlModule.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlModule.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popTrainer()
        {
            DataTable dt = null;
            Int32 vLast = 0;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "Eoid", "EoName", "EoMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                vLast = dt.Rows.Count+1;
                ddlTrainer.DataSource = dt;
                ddlTrainer.DataTextField = "EoName";
                ddlTrainer.DataValueField = "Eoid";
                ddlTrainer.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlTrainer.Items.Insert(0, oli);
                ListItem oli1 = new ListItem("Other", Convert.ToString(vLast));
                ddlTrainer.Items.Insert(vLast, oli1);
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
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popCenter()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "MarketID", "Market", "MarketMSt", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
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
        /// <param name="vTrainerId"></param>
        private void popDesigGrade(string vTrainerId)
        {
            DataTable dt = null;
            CTraining oTr = null;
            try
            {
                oTr = new CTraining();
                dt = oTr.GetDesigGradebyId(vTrainerId);
                if (dt.Rows.Count > 0)
                {
                    lblDesig.Text = Convert.ToString(dt.Rows[0]["Designation"]);
                    lblGrade.Text = Convert.ToString(dt.Rows[0]["Grade"]);
                }
                else
                {
                    lblDesig.Text = "";
                    lblGrade.Text = "";
                }
            }
            finally
            {
                oTr = null;
                dt = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTrainer_SelectedIndexChanged(object sender, EventArgs e)
        {
            popDesigGrade(ddlTrainer.SelectedValue);
            txtTrainerNM.Text = "";
            if (ddlTrainer.SelectedItem.Text=="Other")
                txtTrainerNM.Enabled = true;
            else
                txtTrainerNM.Enabled = false;
        }
    }
}
