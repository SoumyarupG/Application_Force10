using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;


namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class ROMst : CENTRUMBAse
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
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                {
                    StatusButton("View");
                }
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popBranch();
                GetSupervisor("", "A");
                popDepartment();
                popDesignation();
                LoadGrid(1);
                tbEmp.ActiveTabIndex = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Employee Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuEmpMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "RO Master", false);
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
                    btnDelete.Enabled = true;
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
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
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
        /// <param name="pEoID"></param>
        /// <param name="pMode"></param>
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
            /*
            Dictionary<string, string> oDic = new Dictionary<string, string>();
            oDic.Add("<-Select->", "0");
            oDic.Add("CRO", "LO");
            oDic.Add("BM", "BM");
            oDic.Add("AM", "AM");
            oDic.Add("ACCOUNTANT", "AC");
            oDic.Add("OTHER", "OTH");

            ddlDesig.DataSource = oDic;
            ddlDesig.DataValueField = "value";
            ddlDesig.DataTextField = "key";
            ddlDesig.DataBind();
            */
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DesignationID", "Designation", "DesignationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDesig.DataSource = dt;
                ddlDesig.DataValueField = "DesignationID";
                ddlDesig.DataTextField = "Designation";
                ddlDesig.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDesig.Items.Insert(0, oli);
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
        //private void popRelation()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    try
        //    {
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("N", "N", "AA", "RelationId", "Relation", "RelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
        //        ddlBRel.DataSource = dt;
        //        ddlBRel.DataTextField = "Relation";
        //        ddlBRel.DataValueField = "Relation";
        //        ddlBRel.DataBind();
        //        ListItem oli = new ListItem("<--Select-->", "-1");
        //        ddlBRel.Items.Insert(0, oli);
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
            ddlBranch.Enabled = false;
            LoadBranch("L", "");
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
                    ViewState["StateEdit"] = "Add";
                    tbEmp.ActiveTabIndex = 1;
                    StatusButton("Add");
                    //pnlROTrnDrp.Enabled = false;
                    ClearControls();
                    ddlBranch.Enabled = false;
                    LoadBranch("L", "");
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
            string vEoId = Convert.ToString(ViewState["EoId"]);
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");
            //GetSupervisor(vEoId, "E");
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
            string vBrCode = (string)Session[gblValue.BrnchCode];
            try
            {
                oEO = new CEO();
                dt = oEO.GetEOListPG(vBrCode, this.UserID, pPgIndx, ref vTotRows, txtSearch.Text.ToString());
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
                case "GoTo":
                    if (Int32.Parse(txtGotoPg.Text) <= Int32.Parse(lblTotPg.Text))
                        vPgNo = Int32.Parse(txtGotoPg.Text);
                    break;
            }
            LoadGrid(vPgNo);
            tbEmp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vEoId = "";
            DataTable dt = null;
            CEO oEo = null;
            try
            {
                vEoId = Convert.ToString(e.CommandArgument);
                ViewState["EoId"] = vEoId;
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
                    dt = oEo.GetEODetails(vEoId);
                    if (dt.Rows.Count > 0)
                    {
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EMPCode"]);
                        txtROName.Text = Convert.ToString(dt.Rows[0]["EOName"]);
                        txtJnDt.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        txtDoB.Text = Convert.ToString(dt.Rows[0]["DOB"]);
                        txtFHName.Text = Convert.ToString(dt.Rows[0]["FatherName"]);
                        txtMother.Text = Convert.ToString(dt.Rows[0]["MotherName"]);
                        txtAddress1.Text = Convert.ToString(dt.Rows[0]["Address1"]);
                        txtAddress2.Text = Convert.ToString(dt.Rows[0]["Address2"]);
                        txtState.Text = Convert.ToString(dt.Rows[0]["State"]);
                        txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                        txtPh1.Text = Convert.ToString(dt.Rows[0]["Mobile"]);
                        txtPh2.Text = Convert.ToString(dt.Rows[0]["Phone"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        ddlGrade.SelectedIndex = ddlGrade.Items.IndexOf(ddlGrade.Items.FindByValue(dt.Rows[0]["Grade"].ToString()));
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(Convert.ToString(dt.Rows[0]["Designation"]).Trim().ToUpper()));
                        ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByValue(dt.Rows[0]["DeptId"].ToString()));
                        ddlHo.SelectedIndex = ddlHo.Items.IndexOf(ddlHo.Items.FindByValue(dt.Rows[0]["Location"].ToString()));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        ddlSuper.SelectedIndex = ddlSuper.Items.IndexOf(ddlSuper.Items.FindByValue(dt.Rows[0]["SupervisorId"].ToString()));
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(dt.Rows[0]["RoleYN"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadBranch("Edit", vEoId);
                        tbEmp.ActiveTabIndex = 1;
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
            string vXml = "";
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vEoId = Convert.ToString(ViewState["EoId"]), vSupervisorID = "";
            string vPass = dataEncryp.EncryptText("admin");
            Int32 vErr = 0, vRec = 0, vDistrictId = 0, vStateId = 0, vDeptId = 0;
            DateTime vDOJ = gblFuction.setDate(txtJnDt.Text);
            CEO oEo = null;
            CGblIdGenerator oGbl = null;
            oEo = new CEO();

            if (ddlDept.SelectedIndex > 0) vDeptId = Convert.ToInt32(ddlDept.SelectedValue);
            if (ddlSuper.SelectedIndex > 0) vSupervisorID = ddlSuper.SelectedValue;

            try
            {
                if (ValidateField() == false)
                    return false;

                if (Mode == "Save")
                {
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                        return false;
                    }
                    if (DtBranch().Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("Branch Allocation Required...");
                        return false;
                    }
                    if (ddlHo.SelectedValue.ToString() == "H")
                    {
                        vBrCode = Session[gblValue.BrnchCode].ToString();
                    }
                    else
                    {
                        vBrCode = ddlBranch.SelectedValue.ToString();
                    }
                    vXml = DtToXml(DtBranch());
                    vErr = oEo.SaveEOMst(ref vEoId, txtEmpCode.Text.Replace("'", "''"), txtROName.Text.Replace("'", "''"), vDOJ,
                            gblFuction.setDate(txtDoB.Text), txtFHName.Text.Replace("'", "''"), txtMother.Text.Replace("'", "''"),
                            txtAddress1.Text.Replace("'","''"),txtAddress2.Text.Replace("'","''"),
                            txtState.Text.Replace("'","''"), txtPin.Text,txtPh1.Text, txtPh2.Text, txtEmail.Text, ddlGrade.SelectedValue, 
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue,ddlBranch.SelectedValue, ddlRole.SelectedValue, 
                            vSupervisorID, vBrCode, this.UserID, "Save", vPass, vXml);
                    if (vErr > 0)
                    {
                        ViewState["EoId"] = vEoId;
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
                    vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                        return false;
                    }

                    if (DtBranch().Rows.Count <= 0)
                    {
                        gblFuction.MsgPopup("Branch Allocation Required...");
                        return false;
                    }
                    vXml = DtToXml(DtBranch());
                    vErr = oEo.SaveEOMst(ref vEoId, txtEmpCode.Text.Replace("'", "''"), txtROName.Text.Replace("'", "''"), vDOJ,
                            gblFuction.setDate(txtDoB.Text), txtFHName.Text.Replace("'", "''"), txtMother.Text.Replace("'", "''"),
                            txtAddress1.Text.Replace("'", "''"), txtAddress2.Text.Replace("'", "''"),
                            txtState.Text.Replace("'", "''"), txtPin.Text, txtPh1.Text, txtPh2.Text, txtEmail.Text, ddlGrade.SelectedValue,
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue, ddlBranch.SelectedValue, ddlRole.SelectedValue,
                            vSupervisorID, vBrCode, this.UserID, "Edit", vPass, vXml);
                    vErr = 1; 
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
                    oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDelete(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has centre, you can not delete the RO.");
                    //    return false;
                    //}
                    oEo = new CEO();
                    vErr = oEo.SaveEOMst(ref vEoId, txtEmpCode.Text.Replace("'", "''"), txtROName.Text.Replace("'", "''"), vDOJ,
                            gblFuction.setDate(txtDoB.Text), txtFHName.Text.Replace("'", "''"), txtMother.Text.Replace("'", "''"),
                            txtAddress1.Text.Replace("'", "''"), txtAddress2.Text.Replace("'", "''"),
                            txtState.Text.Replace("'","''"), txtPin.Text, txtPh1.Text, txtPh2.Text, txtEmail.Text, ddlGrade.SelectedValue, 
                            ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue,ddlBranch.SelectedValue, ddlRole.SelectedValue, 
                            vSupervisorID, vBrCode, this.UserID, "Delet", vPass, vXml);
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

        private string DtToXml(DataTable dtXml)
        {
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                return vXml;
            }
            finally
            {
                dtXml = null;
            }
        }


        private DataTable DtBranch()
        {
            DataTable dt = new DataTable("EoAllocate");
            DataRow dr;

            dt.Columns.Add(new DataColumn("BranchCode"));

            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                if (chkStatus.Checked == true)
                {
                    dr = dt.NewRow();
                    dr["BranchCode"] = gr.Cells[2].Text;
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtEmpCode.Enabled = Status;
            txtJnDt.Enabled = Status;
            txtROName.Enabled = Status;
            txtDoB.Enabled = Status;
            txtFHName.Enabled = Status;
            txtMother.Enabled = Status;
            txtEmail.Enabled = Status;
            txtAddress1.Enabled = Status;
            txtAddress2.Enabled = Status;
            txtState.Enabled = Status;
            txtPin.Enabled = Status;
            txtPh1.Enabled = Status;
            txtPh2.Enabled = Status;
            txtEmail.Enabled = Status;
            ddlGrade.Enabled = Status;
            ddlDesig.Enabled = Status;
            ddlDept.Enabled = Status;
            ddlHo.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlRole.Enabled = Status;
            ddlSuper.Enabled = Status;
            gvBranch.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtEmpCode.Text = "";
            txtJnDt.Text = "";
            txtROName.Text = "";
            txtDoB.Text = "";
            txtFHName.Text = "";
            txtMother.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtPin.Text = "";
            txtPh1.Text = "";
            txtPh2.Text = "";
            txtEmail.Text = "";
            ddlGrade.SelectedIndex = -1;
            ddlDesig.SelectedIndex = -1;
            ddlDept.SelectedIndex = -1;
            ddlHo.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            ddlRole.SelectedIndex = -1;
            ddlSuper.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        /// <param name="vEoId"></param>
        private void LoadBranch(string pMode, string vEoId)
        {
            DataTable dt = null;
            CEO oEo = null;
            try
            {
                oEo = new CEO();
                dt = oEo.GetBranchForRO(pMode, vEoId);
                gvBranch.DataSource = dt;
                gvBranch.DataBind();
                ViewState["Branch"] = dt;
                foreach (GridViewRow gr in gvBranch.Rows)
                {
                    CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                    if (dt.Rows[gr.RowIndex]["Actv"].ToString() == "1")
                        chkStatus.Checked = true;
                    else
                        chkStatus.Checked = false;
                }
            }
            finally
            {
                oEo = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlHo_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            dt = (DataTable)ViewState["Branch"];

            if (ddlHo.SelectedValue == "N")
            {
                ddlBranch.Enabled = true;
            }
            else
            {
                ddlBranch.SelectedIndex = -1;
                ddlBranch.Enabled = false;
            }

            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                if (ddlHo.SelectedValue == "H")
                {
                    if (dt.Rows[gr.RowIndex]["BranchCode"].ToString() == "0000")
                        chkStatus.Checked = true;
                    else
                        chkStatus.Checked = false;
                }
                else
                    chkStatus.Checked = false;
            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            dt = (DataTable)ViewState["Branch"];
            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                if (ddlBranch.SelectedIndex > 0)
                {
                    if (dt.Rows[gr.RowIndex]["BranchCode"].ToString() == ddlBranch.SelectedValue)
                        chkStatus.Checked = true;
                    else
                        chkStatus.Checked = false;
                }
                else
                {
                    if (ddlHo.SelectedIndex > 0)
                    {
                        if (ddlHo.SelectedValue == "H")
                        {
                            if (dt.Rows[gr.RowIndex]["BranchCode"].ToString() == "0000")
                                chkStatus.Checked = true;
                            else
                                chkStatus.Checked = false;
                        }
                        else
                            chkStatus.Checked = false;
                    }
                }
            }

        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadGrid(1);
        }


    }
}