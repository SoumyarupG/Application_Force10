using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class TrainerModuleActual : CENTRUMBase
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
                popModule();
                popBranch();
                LoadBorrower();
                LoadEmployee();
                popTrainer();
                //ddlTrainedBy.SelectedIndex = ddlTrainedBy.Items.IndexOf(ddlTrainedBy.Items.FindByValue(Convert.ToString(Session[gblValue.UserName])));
                txtTrainingDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
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
                this.PageHeading = "Trainer Module Actual";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTrnModAct);
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Trainer Module Actual", false);
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
            ddlTraineType.Enabled = Status;
            ddlModule.Enabled = Status;
            ddlVenue.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlTrainedBy.Enabled = Status;
            txtTrainEndTm.Enabled = Status;
            txtTrainStTm.Enabled = Status;
            txtTrainingDt.Enabled = Status;
            gvBorrow.Enabled = Status;
            gvEmp.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlTraineType.SelectedIndex = -1;
            ddlModule.SelectedIndex = -1;
            ddlVenue.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            ddlTrainedBy.SelectedIndex = -1;
            txtTrainEndTm.Text = "";
            txtTrainStTm.Text = "";
            txtTrainingDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            gvBorrow.DataSource = null;
            gvBorrow.DataBind();
            gvEmp.DataSource = null;
            gvEmp.DataBind();
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
                dt = oFS.GetTrainingModuleActualPG(pPgIndx, ref vRows, this.UserID);
                gvTrainActual.DataSource = dt.DefaultView;
                gvTrainActual.DataBind();
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
            tbTrainActual.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTrainActual_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vTrainerActualId = "";
            string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            DataSet ds = null;
            DataTable dt = null, dtBorrower = null, dtEMP = null;
            CTraining oFS = null;
            try
            {
                vTrainerActualId = Convert.ToString(e.CommandArgument);
                ViewState["TrainerActualId"] = vTrainerActualId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvTrainActual.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oFS = new CTraining();
                    ds = oFS.GetTrainModuleActualbyId(vTrainerActualId);
                    dt = ds.Tables[0];
                    dtBorrower = ds.Tables[1];
                    dtEMP = ds.Tables[2];

                    if (dt.Rows.Count > 0)
                    {
                        ddlTraineType.SelectedIndex = ddlTraineType.Items.IndexOf(ddlTraineType.Items.FindByText(Convert.ToString(dt.Rows[0]["TraineeType"])));
                        ddlModule.SelectedIndex = ddlModule.Items.IndexOf(ddlModule.Items.FindByValue(Convert.ToString(dt.Rows[0]["ModuleId"])));
                        popTrainer();
                        ddlTrainedBy.SelectedIndex = ddlTrainedBy.Items.IndexOf(ddlTrainedBy.Items.FindByValue(Convert.ToString(dt.Rows[0]["EOId"])));
                        txtTrainStTm.Text = Convert.ToString(dt.Rows[0]["TrainStTime"]);
                        txtTrainEndTm.Text = Convert.ToString(dt.Rows[0]["TrainEndTime"]);
                        ddlVenue.SelectedIndex = ddlVenue.Items.IndexOf(ddlVenue.Items.FindByValue(Convert.ToString(dt.Rows[0]["VenueId"])));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[0]["BranchCode"])));
                        txtTrainingDt.Text = Convert.ToString(dt.Rows[0]["TrainingDt"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        

                        if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                            StatusButton("Show");
                        else
                            StatusButton("ViewOnly");
                    }
                    if (dtBorrower !=null)
                    {
                        gvBorrow.DataSource = null;
                        gvBorrow.DataBind();
                        ((DataTable)ViewState["Borrower"]).Rows.Clear();
                        for (int k = 0; gvBorrow.Rows.Count != dtBorrower.Rows.Count;)
                        {
                            //if (gvBorrow.Rows.Count < dtBorrower.Rows.Count)
                            //{
                            
                            NewBorrower(k);
                            k++;
                            //}
                        }
                        ViewState["Borrower"] = dtBorrower;
                        SetData("B");
                    }

                    if (dtEMP != null)
                    {
                        gvEmp.DataSource = null;
                        gvEmp.DataBind();
                        ((DataTable)ViewState["Employee"]).Rows.Clear();
                        for (int k = 0; gvEmp.Rows.Count != dtEMP.Rows.Count; )
                        {
                            //if (gvBorrow.Rows.Count < dtBorrower.Rows.Count)
                            //{
                            
                            NewEmployee(k);
                            k++;
                            //}
                        }
                        ViewState["Employee"] = dtEMP;
                        SetData("E");
                    }
                    tbTrainActual.ActiveTabIndex = 1;
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
            string vSubId = Convert.ToString(ViewState["TrainerActualId"]), vTrainerActualId = "";
            string vUserBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
            DataTable dtBorrow = null, dtEmp = null;
            string vXmlBorrower = "", vXmlEmployee = "";
            Int32 vErr = 0, vRec = 0;
            CTraining oFS = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vTrainerActualId = Convert.ToString(ViewState["TrainerActualId"]);

                GetData("B");
                GetData("E");

                dtBorrow = (DataTable)ViewState["Borrower"];
                using (StringWriter oSW = new StringWriter())
                {
                    dtBorrow.WriteXml(oSW);
                    vXmlBorrower = oSW.ToString();
                }


                dtEmp = (DataTable)ViewState["Employee"];
                using (StringWriter oSW = new StringWriter())
                {
                    dtEmp.WriteXml(oSW);
                    vXmlEmployee = oSW.ToString();
                }

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
                    vErr = oFS.SaveTrainerModuleActual(ref vTrainerActualId, ddlTraineType.SelectedItem.Text, ddlModule.SelectedValue,
                           gblFuction.setDate(txtTrainingDt.Text), txtTrainStTm.Text.Replace("'", "''"), txtTrainEndTm.Text.Replace("'", "''"),
                           ddlVenue.SelectedValue, ddlBranch.SelectedValue, vUserBrCode, vXmlBorrower, vXmlEmployee, this.UserID, "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["TrainerActualId"] = vTrainerActualId;
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
                    vErr = oFS.SaveTrainerModuleActual(ref vTrainerActualId, ddlTraineType.SelectedItem.Text, ddlModule.SelectedValue,
                           gblFuction.setDate(txtTrainingDt.Text), txtTrainStTm.Text.Replace("'", "''"), txtTrainEndTm.Text.Replace("'", "''"),
                           ddlVenue.SelectedValue, ddlBranch.SelectedValue, vUserBrCode, vXmlBorrower, vXmlEmployee, this.UserID, "I", "Edit");
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
                        vErr = oFS.SaveTrainerModuleActual(ref vTrainerActualId, ddlTraineType.SelectedItem.Text, ddlModule.SelectedValue,
                           gblFuction.setDate(txtTrainingDt.Text), txtTrainStTm.Text.Replace("'", "''"), txtTrainEndTm.Text.Replace("'", "''"),
                           ddlVenue.SelectedValue, ddlBranch.SelectedValue, vUserBrCode, vXmlBorrower, vXmlEmployee, this.UserID, "I", "Del");
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
            ViewState["TrainerActualId"] = null;
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
                tbTrainActual.ActiveTabIndex = 1;
                if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
                    StatusButton("Add");
                else
                    StatusButton("ViewOnly");
                ClearControls();
                LoadBorrower();
                LoadEmployee();
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
                    tbTrainActual.ActiveTabIndex = 0;
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
            tbTrainActual.ActiveTabIndex = 0;
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


        /// <summary>
        /// 
        /// </summary>
        private void LoadBorrower()
        {
            ViewState["Borrower"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("CenterId");
            dt.Columns.Add("BorrowId");
            dt.Columns.Add("chkYN");
            dt.AcceptChanges();
            ViewState["Borrower"] = dt;
            NewBorrower(0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vRow"></param>
        private void NewBorrower(Int32 vRow)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["Borrower"];
                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.Add();
                //dt.Rows[gvBorrow.Rows.Count]["RowId"] = gvBorrow.Rows.Count + 1;
                dt.Rows[vRow]["BranchCode"] = -1;
                dt.Rows[vRow]["CenterId"] = -1;
                dt.Rows[vRow]["BorrowId"] = -1;
                dt.Rows[vRow]["chkYN"] = "N";
                dt.AcceptChanges();

                ViewState["Borrower"] = dt;
                gvBorrow.DataSource = dt;
                gvBorrow.DataBind();
                SetData("B");
                //DropDownList ddlBrBorrow = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBrBorrow");
                //popBrBorrow(ddlBrBorrow);
                //upColl.Update();
                    
            }
            finally
            {
                //oTr = null;
                //dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddlBrBorrow"></param>
        private void popBr(DropDownList ddlBr,string vMode)//vMode={A=ALL except HO,L=ALL include HO}
        {
            //DropDownList ddlBrBorrow1 = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBrBorrow");

            CTraining oTr = null;
            DataTable dtDenom = null;

            ddlBr.Items.Clear();
            oTr = new CTraining();
            dtDenom = oTr.GetBranchForRO(vMode, "");
            ddlBr.DataSource = dtDenom;
            ddlBr.DataTextField = "BranchName";
            ddlBr.DataValueField = "BranchCode";
            ddlBr.DataBind();
            ListItem oLi = new ListItem("<--Select-->", "-1");
            ddlBr.Items.Insert(0, oLi);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBrBorrow_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBrBorrow = (DropDownList)sender;
            GridViewRow gr = (GridViewRow)ddlBrBorrow.Parent.Parent;
            Int32 vRow = gr.RowIndex;
            popCenter(ddlBrBorrow.SelectedValue,vRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBrEmp_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBrEmp = (DropDownList)sender;
            GridViewRow gr = (GridViewRow)ddlBrEmp.Parent.Parent;
            Int32 vRow = gr.RowIndex;
            popEmployee(ddlBrEmp.SelectedValue, vRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vBranchCode"></param>
        /// <param name="vRow"></param>
        private void popCenter(string vBranchCode, Int32 vRow)
        {
            DataTable dt = null;
            DropDownList ddlCenter = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlCenter");
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", vBranchCode, "BranchCode", "AA", gblFuction.setDate("01/01/1900"), "0000");
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
        /// <param name="vBranchCode"></param>
        /// <param name="vRow"></param>
        private void popEmployee(string vBranchCode, Int32 vRow)
        {
            DataTable dt = null;
            DropDownList ddlEmpName = (DropDownList)gvEmp.Rows[vRow].FindControl("ddlEmpName");
            CTraining oTr = null;
            try
            {
                oTr = new CTraining();
                dt = oTr.GetEmployeeByBr(vBranchCode);
                ddlEmpName.DataSource = dt;
                ddlEmpName.DataTextField = "EoName";
                ddlEmpName.DataValueField = "Eoid";
                ddlEmpName.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEmpName.Items.Insert(0, oli);
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
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCenter = (DropDownList)sender;
            GridViewRow gr = (GridViewRow)ddlCenter.Parent.Parent;
            Int32 vRow = gr.RowIndex;
            popBorrower(ddlCenter.SelectedValue, vRow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vBranchCode"></param>
        /// <param name="vRow"></param>
        private void popBorrower(string vCenterId, Int32 vRow)
        {
            DataTable dt = null;
            DropDownList ddlBorrow = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBorrow");
            CTraining oTr = null;
            try
            {
                oTr = new CTraining();
                dt = oTr.GetBorrower(vCenterId,gblFuction.setDate(txtTrainingDt.Text));
                ddlBorrow.DataSource = dt;
                ddlBorrow.DataTextField = "MemName";
                ddlBorrow.DataValueField = "MemberID";
                ddlBorrow.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBorrow.Items.Insert(0, oli);
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
        protected void ImDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            GridView gv = (GridView)btnDel.Parent.Parent.NamingContainer;
            //GridViewRow PrevGr = gv.Rows[gR.RowIndex - 1];
            if (btnDel.AlternateText == "B")
            {
                dt = (DataTable)ViewState["Borrower"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["Borrower"] = dt;
                    gvBorrow.DataSource = dt;
                    gvBorrow.DataBind();
                    SetData("B");
                    //PrevGr.Enabled = true;
                    //TotalPaid();
                }
                else if (dt.Rows.Count == 1)
                {
                    //LoadDenom();
                    //TotalPaid();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            if (btnDel.AlternateText == "E")
            {
                dt = (DataTable)ViewState["Employee"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["Employee"] = dt;
                    gvEmp.DataSource = dt;
                    gvEmp.DataBind();
                    SetData("E");
                    //PrevGr.Enabled = true;
                    //TotalPaid();
                }
                else if (dt.Rows.Count == 1)
                {
                    //LoadDenom();
                    //TotalPaid();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBorrow_Click(object sender, EventArgs e)
        {
            //DataTable dt = GetData();
            int curRow = 0, maxRow = 0;
            Button btnAddBorrow = (Button)sender;
            GridViewRow gr = (GridViewRow)btnAddBorrow.NamingContainer;
            curRow = gr.RowIndex;
            maxRow = gvBorrow.Rows.Count;
            GetData("B");
            if (curRow == maxRow - 1)
            {
                NewBorrower(gvBorrow.Rows.Count);
                gr.Enabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddEmp_Click(object sender, EventArgs e)
        {
            //DataTable dt = GetData();
            int curRow = 0, maxRow = 0;
            Button btnAddEmp = (Button)sender;
            GridViewRow gr = (GridViewRow)btnAddEmp.NamingContainer;
            curRow = gr.RowIndex;
            maxRow = gvEmp.Rows.Count;
            GetData("E");
            if (curRow == maxRow - 1)
            {
                NewEmployee(gvEmp.Rows.Count);
                gr.Enabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void GetData(string vMode)
        {
            DataTable dt = null;
            if (vMode == "B")
            {
                dt = (DataTable)ViewState["Borrower"];
                foreach (GridViewRow gr in gvBorrow.Rows)
                {
                    DropDownList ddlBrBorrow = (DropDownList)gr.FindControl("ddlBrBorrow");
                    DropDownList ddlCenter = (DropDownList)gr.FindControl("ddlCenter");
                    DropDownList ddlBorrow = (DropDownList)gr.FindControl("ddlBorrow");
                    CheckBox cbSel = (CheckBox)gr.FindControl("cbSel");
                    //TextBox txtStCh = (TextBox)gr.FindControl("txtStCh");



                    dt.Rows[gr.RowIndex]["BranchCode"] = ddlBrBorrow.SelectedValue;
                    dt.Rows[gr.RowIndex]["CenterId"] = ddlCenter.SelectedValue;
                    dt.Rows[gr.RowIndex]["BorrowId"] = ddlBorrow.SelectedValue;
                    dt.Rows[gr.RowIndex]["chkYN"] = cbSel.Checked ? "Y" : "N";
                    //dt.Rows[gr.RowIndex]["StCh"] = txtStCh.Text;

                    dt.AcceptChanges();
                    ViewState["Borrower"] = dt;
                }
            }
            if (vMode == "E")
            {
                dt = (DataTable)ViewState["Employee"];
                foreach (GridViewRow gr in gvEmp.Rows)
                {
                    DropDownList ddlBrEmp = (DropDownList)gr.FindControl("ddlBrEmp");
                    DropDownList ddlEmpName = (DropDownList)gr.FindControl("ddlEmpName");
                    DropDownList ddlScoreModule = (DropDownList)gr.FindControl("ddlScoreModule");
                    DropDownList ddlRetrain = (DropDownList)gr.FindControl("ddlRetrain");
                    TextBox txtScore = (TextBox)gr.FindControl("txtScore");
                    TextBox txtMrksObtain = (TextBox)gr.FindControl("txtMrksObtain");
                    TextBox txtScrPercent = (TextBox)gr.FindControl("txtScrPercent");


                    dt.Rows[gr.RowIndex]["BranchCode"] = ddlBrEmp.SelectedValue;
                    dt.Rows[gr.RowIndex]["EOID"] = ddlEmpName.SelectedValue;
                    dt.Rows[gr.RowIndex]["ScoreModuleYN"] = ddlScoreModule.SelectedValue;
                    dt.Rows[gr.RowIndex]["FullScore"] = txtScore.Text == "" ? "0" : txtScore.Text;
                    dt.Rows[gr.RowIndex]["MarksRcvd"] = txtMrksObtain.Text == "" ? "0" : txtMrksObtain.Text;
                    dt.Rows[gr.RowIndex]["ScorePercent"] = txtScrPercent.Text == "" ? "0" : txtScrPercent.Text;
                    dt.Rows[gr.RowIndex]["RetrainYN"] = ddlRetrain.SelectedValue;
                    dt.AcceptChanges();
                    ViewState["Employee"] = dt;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetData(string vMode)
        {
            DataTable dt = null;
            int i=0;
            if (vMode == "B")//B=Borrower
            {
                i = 0;
                dt = (DataTable)ViewState["Borrower"];
                foreach (DataRow gr in dt.Rows)
                {
                    //if (dt.Rows.Count - 1 != gr.RowIndex)
                    //{
                    DropDownList ddlBrBorrow = (DropDownList)gvBorrow.Rows[i].FindControl("ddlBrBorrow");
                    DropDownList ddlCenter = (DropDownList)gvBorrow.Rows[i].FindControl("ddlCenter");
                    DropDownList ddlBorrow = (DropDownList)gvBorrow.Rows[i].FindControl("ddlBorrow");
                    CheckBox cbSel = (CheckBox)gvBorrow.Rows[i].FindControl("cbSel");
                    popBr(ddlBrBorrow,"A");
                    ddlBrBorrow.SelectedIndex = ddlBrBorrow.Items.IndexOf(ddlBrBorrow.Items.FindByValue(Convert.ToString(dt.Rows[i]["BranchCode"])));
                    popCenter(ddlBrBorrow.SelectedValue, i);
                    ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(Convert.ToString(dt.Rows[i]["CenterId"])));
                    popBorrower(ddlCenter.SelectedValue, i);
                    ddlBorrow.SelectedIndex = ddlBorrow.Items.IndexOf(ddlBorrow.Items.FindByValue(Convert.ToString(dt.Rows[i]["BorrowId"])));
                    cbSel.Checked = Convert.ToString(dt.Rows[i]["chkYN"]) == "Y" ? true : false;
                    //}
                    i++;
                }
            }
            if (vMode == "E")//E=Employee
            {
                i = 0;
                dt = (DataTable)ViewState["Employee"];
                foreach (DataRow gr in dt.Rows)
                {
                    //if (dt.Rows.Count - 1 != gr.RowIndex)
                    //{
                    DropDownList ddlBrEmp = (DropDownList)gvEmp.Rows[i].FindControl("ddlBrEmp");
                    DropDownList ddlEmpName = (DropDownList)gvEmp.Rows[i].FindControl("ddlEmpName");
                    DropDownList ddlScoreModule = (DropDownList)gvEmp.Rows[i].FindControl("ddlScoreModule");
                    DropDownList ddlRetrain = (DropDownList)gvEmp.Rows[i].FindControl("ddlRetrain");
                    TextBox txtScore = (TextBox)gvEmp.Rows[i].FindControl("txtScore");
                    TextBox txtMrksObtain = (TextBox)gvEmp.Rows[i].FindControl("txtMrksObtain");
                    TextBox txtScrPercent = (TextBox)gvEmp.Rows[i].FindControl("txtScrPercent");
                    popBr(ddlBrEmp,"L");
                    ddlBrEmp.SelectedIndex = ddlBrEmp.Items.IndexOf(ddlBrEmp.Items.FindByValue(Convert.ToString(dt.Rows[i]["BranchCode"])));
                    popEmployee(ddlBrEmp.SelectedValue, i);
                    ddlEmpName.SelectedIndex = ddlEmpName.Items.IndexOf(ddlEmpName.Items.FindByValue(Convert.ToString(dt.Rows[i]["EOID"])));
                    ddlScoreModule.SelectedIndex = ddlScoreModule.Items.IndexOf(ddlScoreModule.Items.FindByValue(Convert.ToString(dt.Rows[i]["ScoreModuleYN"])));
                    txtScore.Text = Convert.ToString(dt.Rows[i]["FullScore"]);
                    txtMrksObtain.Text = Convert.ToString(dt.Rows[i]["MarksRcvd"]);
                    txtScrPercent.Text = Convert.ToString(dt.Rows[i]["ScorePercent"]);
                    ddlRetrain.SelectedIndex = ddlRetrain.Items.IndexOf(ddlRetrain.Items.FindByValue(Convert.ToString(dt.Rows[i]["RetrainYN"])));
                    //}
                    i++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadEmployee()
        {
            ViewState["Employee"] = null;
            DataTable dt = new DataTable("Table2");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("EOID");
            dt.Columns.Add("ScoreModuleYN");
            dt.Columns.Add("FullScore");
            dt.Columns.Add("MarksRcvd");
            dt.Columns.Add("ScorePercent");
            dt.Columns.Add("RetrainYN");
            dt.AcceptChanges();
            ViewState["Employee"] = dt;
            NewEmployee(0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vRow"></param>
        private void NewEmployee(Int32 vRow)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["Employee"];
                DataRow dr;
                dr = dt.NewRow();
                dt.Rows.Add();
                //dt.Rows[gvBorrow.Rows.Count]["RowId"] = gvBorrow.Rows.Count + 1;
                dt.Rows[vRow]["BranchCode"] = -1;
                dt.Rows[vRow]["EOID"] = -1;
                dt.Rows[vRow]["ScoreModuleYN"] = "Y";
                dt.Rows[vRow]["FullScore"] = 0;
                dt.Rows[vRow]["MarksRcvd"] = 0;
                dt.Rows[vRow]["ScorePercent"] = 0;
                dt.Rows[vRow]["RetrainYN"] = "N";
                dt.AcceptChanges();

                ViewState["Employee"] = dt;
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
                SetData("E");
                //DropDownList ddlBrBorrow = (DropDownList)gvBorrow.Rows[vRow].FindControl("ddlBrBorrow");
                //popBrBorrow(ddlBrBorrow);
                //upColl.Update();

            }
            finally
            {
                //oTr = null;
                //dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popTrainer()
        {
            DataTable dt = null;
            Int32 vLast = 0;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                //if(vMode=="A")
                //    dt = oGb.PopComboMIS("N", "N", "AA", "Eoid", "EoName", "EoMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                //else
                dt = oGb.PopComboMIS("S", "N", "AA", "Eoid", "EoName", "EoMst", Convert.ToString(Session[gblValue.UserName]), "EmpCode", "AA", gblFuction.setDate("01/01/1900"), "0000");
                vLast = dt.Rows.Count;
                ddlTrainedBy.DataSource = dt;
                ddlTrainedBy.DataTextField = "EoName";
                ddlTrainedBy.DataValueField = "Eoid";
                ddlTrainedBy.DataBind();
                //ListItem oli = new ListItem("<--Select-->", "-1");
                //ddlTrainedBy.Items.Insert(0, oli);
                ListItem oli1 = new ListItem("Other", Convert.ToString(vLast));
                ddlTrainedBy.Items.Insert(vLast, oli1);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

    }
}
