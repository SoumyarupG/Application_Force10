using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class RoTransfer : CENTRUMBAse
    {
        protected int vPgNo = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                {
                    StatusButton("View");
                }
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                PopBranch(Session[gblValue.UserName].ToString());
                popSupervisor();
                popEo();
                popDepartment();
                popDesignation();
                LoadGrid(1);
                tbEmp.ActiveTabIndex = 0;

            }
        }


        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Employee Transfer";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRoTransfer);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "RO Transfer", false);
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
        private void popEo()
        {
            DataTable dt = null;
            CCM oGb = null;
            string vBr = ddlBr.SelectedValue.ToString();
            try
            {
                oGb = new CCM();
                dt = oGb.GetCmByBranch(vBr, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ddlEo.DataSource = dt;
                ddlEo.DataTextField = "EoName";
                ddlEo.DataValueField = "EoId";
                ddlEo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEo.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void popSupervisor()
        {

            DataTable dt = null;
            CEO oEO = null;
            try
            {
                oEO = new CEO();
                dt = oEO.GetEoSupervisor(gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                ddlSuper.DataSource = dt;
                ddlSuper.DataTextField = "EoName";
                ddlSuper.DataValueField = "EoId";
                ddlSuper.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSuper.Items.Insert(0, oli);
            }
            finally
            {
                oEO = null;
                dt = null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        //private void popBranch()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    try
        //    {
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
        //        ddlBranch.DataSource = dt;
        //        ddlBranch.DataTextField = "BranchName";
        //        ddlBranch.DataValueField = "BranchCode";
        //        ddlBranch.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlBranch.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        private void popDepartment()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DeptID", "DeptName", "DeptMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
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

        /// <summary>
        /// 
        /// </summary>
        private void popDesignation()
        {
            Dictionary<string, string> oDic = new Dictionary<string, string>();
            oDic.Add("<-Select->", "0");
            oDic.Add("CRO", "LO");
            oDic.Add("BM", "BM");
            oDic.Add("AM", "AM");
            ddlDesig.DataSource = oDic;
            ddlDesig.DataValueField = "value";
            ddlDesig.DataTextField = "key";
            ddlDesig.DataBind();
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            //pnlROTrnDrp.Enabled = false;
            ClearControls();
            //ddlBranch.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            if (ddlHo.SelectedIndex > 0 && ddlHo.SelectedValue == "H") ddlBranch.Enabled = false;
            if (ddlHo.SelectedIndex > 0 && ddlHo.SelectedValue == "N") ddlBranch.Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
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
            if (ViewState["Tra_Drop"].ToString() == "D")
            {
                gblFuction.MsgPopup("Terminate Employee cannot be save!!");
                return;
            }
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateField()
        {
            Boolean vResult = true;
            if (ddlHo.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Location cannot be empty");
                vResult = false;
                return vResult;
            }
            if (ddlHo.SelectedValue == "N")
            {
                if (ddlBranch.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("if Non Ho Then Branch Selection Mandatory");
                    vResult = false;
                    return vResult;
                }
            }
            if (txtLstTr.Text == "" || gblFuction.IsDate(txtLstTr.Text) == false)
            {
                gblFuction.MsgPopup("Last Transfer Date Invalid");
                vResult = false;
                return vResult;
            }
            if (txtEff.Text == "" || gblFuction.IsDate(txtEff.Text) == false)
            {
                gblFuction.MsgPopup("Effective Date Invalid");
                vResult = false;
                return vResult;
            }

            if (gblFuction.setDate(txtLstTr.Text) >= gblFuction.setDate(txtEff.Text))
            {
                gblFuction.MsgPopup("Effective Date should be greater than Last Transfer Date");
                vResult = false;
                return vResult;
            }

            if (ddlStatus.SelectedValue == "T" || ddlStatus.SelectedValue == "D" || ddlStatus.SelectedValue == "S" )
            {
                DataTable dt = null;
                CCenter oCL = null;
                try
                {

                    oCL = new CCenter();

                    Int32 intSrChar = txtBranch.Text.IndexOf("-");
                    string vBrCode = "";

                    if (intSrChar > 0)
                    {
                        vBrCode = Convert.ToString(txtBranch.Text.Substring(0, (intSrChar - 1)));
                    }

                    dt = oCL.PopCenterWithCollDay(ddlEo.SelectedValue, gblFuction.setDate(txtEff.Text), vBrCode, "N"); //With No CollDay
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Cluster Are Found Against This C.R.O.! Transfer Cluster First.");
                        vResult = false;
                        return vResult;
                    }
                }
                finally
                {
                    dt = null;
                    oCL = null;
                }

            }


            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CEO oEO = null;
            try
            {
                oEO = new CEO();
                dt = oEO.GetEOTransferPG(pPgIndx, ref vTotRows);
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
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
                oEO = null;
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
            tbEmp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int vTransId = 0;
            //string vEoID = "";
            DataTable dt = null;
            CEO oEo = null;
            ViewState["Tra_Drop"] = null;

            try
            {
                vTransId = Convert.ToInt32(e.CommandArgument);
                ViewState["TransId"] = vTransId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    /**
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    **/

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvEmp.Rows)
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
                    
                    oEo = new CEO();
                    dt = oEo.GetEOTransByID(vTransId);
                    if (dt.Rows.Count > 0)
                    {
                        ddlEo.SelectedIndex = ddlEo.Items.IndexOf(ddlEo.Items.FindByValue(dt.Rows[0]["Eoid"].ToString()));
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EMPCode"]);
                        txtJnDt.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        txtLstTr.Text = Convert.ToString(dt.Rows[0]["LstTrDt"]);
                        txtGrade.Text = Convert.ToString(dt.Rows[0]["PGrade"]);
                        txtDesig.Text = Convert.ToString(dt.Rows[0]["PDesignation"]);
                        txtDept.Text = Convert.ToString(dt.Rows[0]["DeptName"]);
                        txtLocation.Text = Convert.ToString(dt.Rows[0]["PLocation"]);
                        txtBranch.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                        txtEff.Text = Convert.ToString(dt.Rows[0]["EffectiveDate"]);
                        ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(dt.Rows[0]["Status"].ToString()));
                        ddlGrade.SelectedIndex = ddlGrade.Items.IndexOf(ddlGrade.Items.FindByValue(dt.Rows[0]["Grade"].ToString()));
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(Convert.ToString(dt.Rows[0]["Designation"])));
                        ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByValue(dt.Rows[0]["DeptId"].ToString()));
                        ddlHo.SelectedIndex = ddlHo.Items.IndexOf(ddlHo.Items.FindByValue(dt.Rows[0]["Location"].ToString()));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        popSupervisor();
                        ddlSuper.SelectedIndex = ddlSuper.Items.IndexOf(ddlSuper.Items.FindByValue(dt.Rows[0]["SupervisorID"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        //DataTable DEO = oEo.CheckForTerminateEO(dt.Rows[0]["Eoid"].ToString());
                        ViewState["Tra_Drop"] = dt.Rows[0]["Tra_Drop"].ToString();
                        StatusButton("Show");
                        EnableControl(false);
                    }
                }
            }
            finally
            {
                dt = null;
                oEo = null;
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vEoId = ddlEo.SelectedValue.ToString(), vChkDlk = "N";
            Int32 vErr = 0, vRec = 0, vDeptId = 0;
            CEO oEo = null;
            oEo = new CEO();
            string vEffDate = gblFuction.setStrDate(txtEff.Text);
            CGblIdGenerator oGbl = null;
            CLoanRecovery oLn = null;
            DataTable dt = null;

            if (ddlDept.SelectedIndex > 0) vDeptId = Convert.ToInt32(ddlDept.SelectedValue);


            try
            {
                if (ValidateField() == false)
                    return false;

                if (Mode == "Save")
                {
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    oLn = new CLoanRecovery();

                    //dt = oLn.CheckMaxCollectionDate(ddlBranch.SelectedValue.ToString());
                    //if (dt.Rows.Count > 0)
                    //{
                    //    if (gblFuction.setDate(txtEff.Text) < gblFuction.setDate(dt.Rows[0]["LastCollDt"].ToString()))
                    //    {
                    //        gblFuction.AjxMsgPopup("Collection Already taken after that Date, Cannot Transfer...");
                    //        return false;
                    //    }
                    //}

                    //vRec = oGbl.ChkReferenceTable(vEoId, ddlBranch.SelectedValue, "EOMST", "MARKETMST", "EOID", "EOID");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The Branch have Cluster under the CRO, hence it can not be change current status..");
                    //    return false;
                    //}

                    vRec = oGbl.ChkDuplicate("EOTransfer", "EffectiveDate", vEffDate, "EoID", vEoId, "EOID", vEoId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Transfer Date cannot be Duplicate...");
                        return false;
                    }

                    vErr = oEo.SaveEOTransfer(vEoId, gblFuction.setDate(txtEff.Text), ddlStatus.SelectedValue, ddlGrade.SelectedValue,
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue, ddlBranch.SelectedValue, this.UserID, ddlSuper.SelectedValue, "Save");
                    if (vErr > 0)
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
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDelete(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    vRec = oGbl.ChkDuplicate("EOTransfer", "EffectiveDate", vEffDate, "EoID", vEoId, "EOID", vEoId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                        return false;
                    }

                    vErr = oEo.SaveEOTransfer(vEoId, gblFuction.setDate(txtEff.Text), ddlStatus.SelectedValue, ddlGrade.SelectedValue,
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue, ddlBranch.SelectedValue, this.UserID, ddlSuper.SelectedValue, "Edit");
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
                    oEo = new CEO();


                    vErr = oEo.SaveEOTransfer(vEoId, gblFuction.setDate(txtEff.Text), ddlStatus.SelectedValue, ddlGrade.SelectedValue,
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue, ddlBranch.SelectedValue, this.UserID, ddlSuper.SelectedValue, "Delet");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
            finally
            {
                oEo = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrevDt"></param>
        /// <param name="NxtDt"></param>
        /// <returns></returns>
        protected Boolean Datechk(string PrevDt, string NxtDt)
        {
            if (gblFuction.setDate(PrevDt) < gblFuction.setDate(NxtDt))
                return true;
            else
                return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtEff.Enabled = Status;
            ddlEo.Enabled = Status;
            ddlStatus.Enabled = Status;
            ddlGrade.Enabled = Status;
            ddlDesig.Enabled = Status;
            ddlDept.Enabled = Status;
            ddlHo.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlSuper.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlEo.SelectedIndex = -1;
            txtEmpCode.Text = "";
            txtJnDt.Text = "";
            txtDesig.Text = "";
            txtGrade.Text = "";
            txtDept.Text = "";
            txtLocation.Text = "";
            txtBranch.Text = "";
            //txtSuper.Text = "";
            txtEff.Text = "";
            ddlStatus.SelectedIndex = -1;
            ddlGrade.SelectedIndex = -1;
            ddlDesig.SelectedIndex = -1;
            ddlDept.SelectedIndex = -1;
            ddlHo.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            ddlSuper.DataSource = null;
            ddlSuper.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlHo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHo.SelectedValue == "N")
            {
                ddlBranch.Enabled = true;
            }
            else
            {
                ddlBranch.SelectedIndex = -1;
                ddlBranch.Enabled = false;
            }
        }

        protected void ddlEo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CEO oEo = null;
            ViewState["Tra_Drop"] = null;

            try
            {
                if (ddlEo.SelectedIndex > 0)
                {
                    oEo = new CEO();
                    dt = oEo.GetEOTransDetail(ddlEo.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EmpCode"]);
                        txtJnDt.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        txtLstTr.Text = Convert.ToString(dt.Rows[0]["EffectiveDate"]);                        
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(dt.Rows[0]["Designation"].ToString()));
                        txtDesig.Text = ddlDesig.SelectedItem.ToString();   //Convert.ToString(dt.Rows[0]["Designation"]);
                        txtGrade.Text = Convert.ToString(dt.Rows[0]["Grade"]);
                        txtDept.Text = Convert.ToString(dt.Rows[0]["DeptName"]);
                        txtLocation.Text = Convert.ToString(dt.Rows[0]["Location"]);
                        txtBranch.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                        txtSuper.Text = Convert.ToString(dt.Rows[0]["Supervisor"]);
                        ViewState["Tra_Drop"] = dt.Rows[0]["Tra_Drop"].ToString();
                        popSupervisor();
                        ddlDesig.SelectedIndex = -1;
                    }
                }
                else
                {
                    txtEmpCode.Text = "";
                    txtJnDt.Text = "";
                    txtLstTr.Text = "";
                    txtDesig.Text = "";
                    txtGrade.Text = "";
                    txtDept.Text = "";
                    txtLocation.Text = "";
                    txtBranch.Text = "";
                    txtSuper.Text = "";
                }
            }
            finally
            {
            }
        }

        private void GetSupervisor(string pEoID, string pMode)
        {
            DataTable dt = null;
            CEO oEo = null;
            try
            {
                oEo = new CEO();
                dt = oEo.PopSupervisor(pEoID, pMode);
                ddlSuper.DataSource = dt;
                ddlSuper.DataTextField = "EoName";
                ddlSuper.DataValueField = "EoID";
                ddlSuper.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSuper.Items.Insert(0, oli);
                if (dt.Rows.Count <= 0)
                {
                    ListItem Self = new ListItem("SELF", "0");
                    ddlSuper.Items.Insert(1, Self);
                }
            }
            finally
            {
                oEo = null;
                dt = null;
            }
        }

        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBr.SelectedIndex > 0) popEo();
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            oUsr = new CUser();
            dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
            ViewState["ID"] = null;
            try
            {

                if (dt.Rows.Count > 0)
                {

                    ddlBr.DataSource = dt;
                    ddlBr.DataTextField = "BranchName";
                    ddlBr.DataValueField = "BranchCode";
                    ddlBr.DataBind();
                    ddlBr.Items.Insert(0, new ListItem("<--Select-->", "-1"));
                    
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ddlBranch.Items.Insert(0, new ListItem("<--Select-->", "-1"));

                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
    }
}
