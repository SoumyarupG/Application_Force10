using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using FORCEBA;
using FORCECA;
using System.Web.Services;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class ROMst : CENTRUMBase
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
                popBranch();
                popState();
                // GetSupervisor("", "A");
                popDistrict();
                popRelation();
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
                this.Menu = false;
                this.PageHeading = "Employee";
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


        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateID", "StateName", "StateMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateID";
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


        private void popDistrict()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DistrictID", "DistrictName", "DistrictMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDistrict.DataSource = dt;
                ddlDistrict.DataTextField = "DistrictName";
                ddlDistrict.DataValueField = "DistrictID";
                ddlDistrict.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDistrict.Items.Insert(0, oli);
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
        //private void GetSupervisor(string pEoID, string pMode)
        //{
        //    //DataTable dt = null;
        //    //CEO oEo = null;
        //    //try
        //    //{
        //    //    oEo = new CEO();
        //    //dt = oEo.PopSupervisor(pEoID, pMode);
        //    //    ddlSuper.DataSource = dt;
        //    //    ddlSuper.DataTextField = "EoName";
        //    //    ddlSuper.DataValueField = "EoID";
        //    //    ddlSuper.DataBind();
        //    //    ListItem oli = new ListItem("<--Select-->", "-1");
        //    //    ddlSuper.Items.Insert(0, oli);
        //    //}
        //    //finally
        //    //{
        //    //    oEo = null;
        //    //    dt = null;
        //    //}
        //}

        [WebMethod]
        public static List<Super> GetSupervisor(string Name)
        {
            DataTable dt = new DataTable();
            CEO oEo = new CEO();
            List<Super> empResult = new List<Super>();
            dt = oEo.PopSupervisorByName(Name);
            foreach (DataRow dR in dt.Rows)
            {
                Super oSL = new Super
                {
                    SuperName = Convert.ToString(dR["EoName"]),
                    SuperId = Convert.ToString(dR["Eoid"])
                };
                empResult.Add(oSL);

            }
            return empResult;
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
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "DesigCode", "DesignationName", "DesignationMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlDesig.DataSource = dt;
                ddlDesig.DataTextField = "DesignationName";
                ddlDesig.DataValueField = "DesigCode";
                ddlDesig.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDesig.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
            //Dictionary<string, string> oDic = new Dictionary<string, string>();
            //oDic.Add("<-Select->", "0");
            //oDic.Add("RO", "RO");
            //oDic.Add("BM", "BM");
            //oDic.Add("AM", "AM");
            //oDic.Add("AGM", "AGM");
            //oDic.Add("JRO", "JRO");
            //oDic.Add("SRO", "SRO");
            //oDic.Add("ABM ", "ABM");
            //oDic.Add("SBM ", "SBM");
            //oDic.Add("AAM ", "AAM");
            //oDic.Add("ARM ", "ARM");
            //oDic.Add("RM ", "RM");
            //oDic.Add("DGM-PROCESS COMPLIANCE", "DGM");
            //oDic.Add("CFO", "CFO");
            //oDic.Add("ASSISTANT MANAGER-HR", "AMH");
            //oDic.Add("EXECUTIVE-HR", "EHR");
            //oDic.Add("MANAGER-IT", "MIT");
            //oDic.Add("ASSISTANT MANAGER-IT", "AIT");
            //oDic.Add("EXECUTIVE-IT", "EIT");
            //oDic.Add("ASSISTANT MANAGER-ACCOUNTS", "AMA");
            //oDic.Add("MANAGER-NPS", "MNP");
            //oDic.Add("AGM-OPERATION", "AOP");
            //oDic.Add("AGM-NPS", "ANP");
            //oDic.Add("EXECUTIVE", "EXE");
            //oDic.Add("INSPECTION OFFICER", "IO");
            //oDic.Add("INSPECTION ASSISTANT", "IA");
            //ddlDesig.DataSource = oDic;
            //ddlDesig.DataValueField = "value";
            //ddlDesig.DataTextField = "key";
            //ddlDesig.DataBind();
        }

        private void popRelation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "HumanRelationId", "HumanRelationName", "HumanRelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBRel.DataSource = dt;
                ddlBRel.DataTextField = "HumanRelationName";
                ddlBRel.DataValueField = "HumanRelationName";
                ddlBRel.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBRel.Items.Insert(0, oli);
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
            if (ddlHo.SelectedIndex > 0 && ddlHo.SelectedValue == "N") ddlBranch.Enabled = false;
            ddlHo.Enabled = false;
            if(this.UserID != 1)
            {
                txtEmpCode.Enabled = false;
                txtROName.Enabled = false;
            }
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
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
                dt = oEO.GetEOListPG(vBrCode, this.UserID, txtSearch.Text.Trim(), pPgIndx, ref vTotRows);
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
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oEo = new CEO();
                    dt = oEo.GetEODetails(vEoId);
                    if (dt.Rows.Count > 0)
                    {
                        txtEmpCode.Text = Convert.ToString(dt.Rows[0]["EMPCode"]);
                        txtROName.Text = Convert.ToString(dt.Rows[0]["EOName"]);
                        txtJnDt.Text = Convert.ToString(dt.Rows[0]["DOJ"]);
                        txtDoB.Text = Convert.ToString(dt.Rows[0]["DOB"]);
                        txtFHName.Text = Convert.ToString(dt.Rows[0]["Fa_HusName"]);
                        txtMother.Text = Convert.ToString(dt.Rows[0]["Mother"]);

                        txtBuildNo.Text = Convert.ToString(dt.Rows[0]["BuildNo"]);
                        txtBlock.Text = Convert.ToString(dt.Rows[0]["Block"]);
                        txtVilge.Text = Convert.ToString(dt.Rows[0]["Village"]);
                        txtStreet.Text = Convert.ToString(dt.Rows[0]["Street"]);
                        txtGP.Text = Convert.ToString(dt.Rows[0]["GP"]);
                        txtPO.Text = Convert.ToString(dt.Rows[0]["PO"]);
                        txtPS.Text = Convert.ToString(dt.Rows[0]["PS"]);
                        ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByValue(dt.Rows[0]["DistrictId"].ToString()));
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateId"].ToString()));
                        ddlBRel.SelectedIndex = ddlBRel.Items.IndexOf(ddlBRel.Items.FindByValue(dt.Rows[0]["contactPRela"].ToString()));
                        txtCountry.Text = Convert.ToString(dt.Rows[0]["Country"]);
                        txtPin.Text = Convert.ToString(dt.Rows[0]["PIN"]);
                        txtPh1.Text = Convert.ToString(dt.Rows[0]["Ph1"]);
                        txtPh2.Text = Convert.ToString(dt.Rows[0]["Ph2"]);
                        txtEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                        txtCPNam.Text = Convert.ToString(dt.Rows[0]["ContactP"]);
                        txtCPPh.Text = Convert.ToString(dt.Rows[0]["ContactPPh"]);

                        ddlGrade.SelectedIndex = ddlGrade.Items.IndexOf(ddlGrade.Items.FindByValue(dt.Rows[0]["Grade"].ToString()));
                        ddlDesig.SelectedIndex = ddlDesig.Items.IndexOf(ddlDesig.Items.FindByValue(Convert.ToString(dt.Rows[0]["Designation"]).Trim().ToUpper()));
                        ddlDept.SelectedIndex = ddlDept.Items.IndexOf(ddlDept.Items.FindByValue(dt.Rows[0]["DeptId"].ToString()));
                        ddlHo.SelectedIndex = ddlHo.Items.IndexOf(ddlHo.Items.FindByValue(dt.Rows[0]["Location"].ToString()));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        //ddlSuper.SelectedIndex = ddlSuper.Items.IndexOf(ddlSuper.Items.FindByValue(dt.Rows[0]["SupervisorId"].ToString()));
                        txtSuper.Text = Convert.ToString(dt.Rows[0]["SupervisorName"]);
                        hdSuperID.Value = Convert.ToString(dt.Rows[0]["SupervisorId"]);
                        ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(dt.Rows[0]["RoleYN"].ToString()));
                        //ddlJDesig.SelectedIndex = ddlJDesig.Items.IndexOf(ddlJDesig.Items.FindByValue(Convert.ToString(dt.Rows[0]["Designation"])));
                        //ddlQuali.SelectedIndex = ddlQuali.Items.IndexOf(ddlQuali.Items.FindByValue(Convert.ToString(dt.Rows[0]["QualificationId"])));
                        //ddlMrySts.SelectedIndex = ddlMrySts.Items.IndexOf(ddlMrySts.Items.FindByValue(Convert.ToString(dt.Rows[0]["MaritalStatus"])));
                        //ddlGendr.SelectedIndex = ddlGendr.Items.IndexOf(ddlGendr.Items.FindByValue(Convert.ToString(dt.Rows[0]["Gender"])));
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
            //string vPass = dataEncryp.EncryptText("admin");
            string vPass = "21232f297a57a5a743894a0e4a801fc3";
            Int32 vErr = 0, vRec = 0, vDistrictId = 0, vStateId = 0, vDeptId = 0;
            DateTime vDOJ = gblFuction.setDate(txtJnDt.Text);
            CEO oEo = null;
            CGblIdGenerator oGbl = null;
            oEo = new CEO();

            if (ddlDistrict.SelectedIndex > 0) vDistrictId = Convert.ToInt32(ddlDistrict.SelectedValue);
            if (ddlState.SelectedIndex > 0) vStateId = Convert.ToInt32(ddlState.SelectedValue);
            if (ddlDept.SelectedIndex > 0) vDeptId = Convert.ToInt32(ddlDept.SelectedValue);
            // if (ddlSuper.SelectedIndex > 0) vSupervisorID = ddlSuper.SelectedValue;
            vSupervisorID = hdSuperID.Value;
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
                    vRec = oGbl.ChkDuplicate("EOMst", "Ph1", txtPh1.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Mobile No can not be Duplicate...");
                        return false;
                    }

                    vRec = oGbl.ChkDupMobNoForEO( txtPh1.Text.Replace("'", "''"));
                    if (vRec > 0)
                    {
                        string MobUser = "";
                        MobUser = oGbl.getMobileUserAllProject(txtPh1.Text.ToString());
                        gblFuction.MsgPopup(MobUser.ToString() + "Already Access this Mobile Number");
                        return false;

                        //gblFuction.MsgPopup("EMP Mobile No can not be Duplicate...");
                        //return false;
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
                            txtBuildNo.Text.Replace("'", "''"), txtBlock.Text.Replace("'", "''"), txtVilge.Text.Replace("'", "''"),
                            txtStreet.Text.Replace("'", "''"), txtGP.Text.Replace("'", "''"), txtPO.Text.Replace("'", "''"),
                            txtPS.Text.Replace("'", "''"), vDistrictId, vStateId, txtCountry.Text.Replace("'", "''"), txtPin.Text,
                            txtPh1.Text, txtPh2.Text, txtEmail.Text, txtCPNam.Text.Replace("'", "''"), ddlBRel.SelectedValue,
                            txtCPPh.Text.Replace("'", "''"), ddlGrade.SelectedValue, ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue,
                            ddlBranch.SelectedValue, ddlRole.SelectedValue, vSupervisorID, vBrCode, this.UserID, "Save", vPass, vXml);
                    if (vErr > 0)
                    {
                        ViewState["EoId"] = vEoId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
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
                    vRec = oGbl.ChkDuplicate("EOMst", "Ph1", txtPh1.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("EMP Mobile No can not be Duplicate...");
                        return false;
                    }

                    vRec = oGbl.ChkDupMobNoForEO(txtPh1.Text.Replace("'", "''"));
                    if (vRec > 0)
                    {
                        string MobUser = "";
                        MobUser = oGbl.getMobileUserAllProject(txtPh1.Text.ToString());
                        gblFuction.MsgPopup(MobUser.ToString() + "Already Access this Mobile Number");
                        return false;

                        //gblFuction.MsgPopup("EMP Mobile No can not be Duplicate...");
                        //return false;
                    }

                    vXml = DtToXml(DtBranch());
                    vErr = oEo.SaveEOMst(ref vEoId, txtEmpCode.Text.Replace("'", "''"), txtROName.Text.Replace("'", "''"), vDOJ,
                            gblFuction.setDate(txtDoB.Text), txtFHName.Text.Replace("'", "''"), txtMother.Text.Replace("'", "''"),
                            txtBuildNo.Text.Replace("'", "''"), txtBlock.Text.Replace("'", "''"), txtVilge.Text.Replace("'", "''"),
                            txtStreet.Text.Replace("'", "''"), txtGP.Text.Replace("'", "''"), txtPO.Text.Replace("'", "''"),
                            txtPS.Text.Replace("'", "''"), vDistrictId, vStateId, txtCountry.Text.Replace("'", "''"), txtPin.Text,
                            txtPh1.Text, txtPh2.Text, txtEmail.Text, txtCPNam.Text.Replace("'", "''"), ddlBRel.SelectedValue,
                            txtCPPh.Text.Replace("'", "''"), ddlGrade.SelectedValue, ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue,
                            ddlBranch.SelectedValue, ddlRole.SelectedValue, vSupervisorID, vBrCode, this.UserID, "Edit", vPass, vXml);
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
                    oEo = new CEO();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("The RO has Center, you can not delete the RO.");
                        return false;
                    }
                    oEo = new CEO();
                    vErr = oEo.SaveEOMst(ref vEoId, txtEmpCode.Text.Replace("'", "''"), txtROName.Text.Replace("'", "''"), vDOJ,
                            gblFuction.setDate(txtDoB.Text), txtFHName.Text.Replace("'", "''"), txtMother.Text.Replace("'", "''"),
                            txtBuildNo.Text.Replace("'", "''"), txtBlock.Text.Replace("'", "''"), txtVilge.Text.Replace("'", "''"),
                            txtStreet.Text.Replace("'", "''"), txtGP.Text.Replace("'", "''"), txtPO.Text.Replace("'", "''"),
                            txtPS.Text.Replace("'", "''"), vDistrictId, vStateId, txtCountry.Text.Replace("'", "''"), txtPin.Text,
                            txtPh1.Text, txtPh2.Text, txtEmail.Text, txtCPNam.Text.Replace("'", "''"), ddlBRel.SelectedValue,
                            txtCPPh.Text.Replace("'", "''"), ddlGrade.SelectedValue, ddlDesig.SelectedValue, vDeptId, ddlHo.SelectedValue,
                            ddlBranch.SelectedValue, ddlRole.SelectedValue, vSupervisorID, vBrCode, this.UserID, "Delet", vPass, vXml);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
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
            txtVilge.Enabled = Status;
            txtPO.Enabled = Status;
            txtPS.Enabled = Status;
            txtBuildNo.Enabled = Status;
            txtBlock.Enabled = Status;
            txtVilge.Enabled = Status;
            txtStreet.Enabled = Status;
            txtGP.Enabled = Status;
            txtPO.Enabled = Status;
            txtPS.Enabled = Status;
            ddlDistrict.Enabled = Status;
            ddlState.Enabled = Status;
            txtCountry.Enabled = Status;
            txtPin.Enabled = Status;
            txtPh1.Enabled = Status;
            txtPh2.Enabled = Status;
            txtEmail.Enabled = Status;
            txtCPNam.Enabled = Status;
            txtCPPh.Enabled = Status;
            ddlBRel.Enabled = Status;
            ddlGrade.Enabled = Status;
            ddlDesig.Enabled = Status;
            ddlDept.Enabled = Status;
            ddlHo.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlRole.Enabled = Status;
            //ddlSuper.Enabled = Status;
            txtSuper.Enabled = Status;
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
            txtBuildNo.Text = "";
            txtBlock.Text = "";
            txtVilge.Text = "";
            txtStreet.Text = "";
            txtGP.Text = "";
            txtPO.Text = "";
            txtPS.Text = "";
            ddlDistrict.SelectedIndex = -1;
            ddlState.SelectedIndex = -1;
            txtCountry.Text = "INDIA";
            txtPin.Text = "";
            txtPh1.Text = "";
            txtPh2.Text = "";
            txtEmail.Text = "";
            txtCPNam.Text = "";
            txtCPPh.Text = "";
            ddlBRel.SelectedIndex = -1;
            ddlGrade.SelectedIndex = -1;
            ddlDesig.SelectedIndex = -1;
            ddlDept.SelectedIndex = -1;
            ddlHo.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            ddlRole.SelectedIndex = -1;
            // ddlSuper.SelectedIndex = -1;
            txtSuper.Text = "";
            hdSuperID.Value = "";
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
            DataTable dt1 = new DataTable();
            CEO oEo = new CEO();

            dt = (DataTable)ViewState["Branch"];
            dt1 = oEo.AutoPopBranchDtls(ddlBranch.SelectedValue);
            if (dt1.Rows.Count > 0)
            {
                ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByValue(dt1.Rows[0]["District"].ToString()));
                ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt1.Rows[0]["StateID"].ToString()));
                txtPin.Text = dt1.Rows[0]["PIN"].ToString();
            }
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
                        {
                            chkStatus.Checked = false;
                        }
                    }
                }
            }

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string vEoId = Convert.ToString(ViewState["EoId"]);
            RegisterBioAtt(vEoId);
        }

        public void RegisterBioAtt(string pEoId)
        {
            Int32 vErr = 0;
            CUser oUser = null;
            string vPassL = "", vPassR = "";
            Int32 vUserId = Convert.ToInt32(Session[gblValue.UserId].ToString());
            try
            {
                vPassL = hdnL.Value;
                vPassR = hdnR.Value;
                oUser = new CUser();
                vErr = oUser.Update_BioImg(pEoId, vPassL, vPassR);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup("Finger Image Capture");
                    Response.Redirect("~/WebPages/Public/Main.aspx", false);
                }
                else if (vErr == 1)
                {
                    gblFuction.MsgPopup(gblMarg.DBError);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class Super
    {
        public string SuperName { get; set; }
        public string SuperId { get; set; }
    }
}