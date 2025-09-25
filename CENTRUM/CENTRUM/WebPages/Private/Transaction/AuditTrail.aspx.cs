using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class AuditTrail : CENTRUMBase
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");               
                popAGM();
                popBranch();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
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
                this.PageHeading = "Internal Inspection Audit Plan";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAuditPlan);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Internal Inspection Audit Plan", false);
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
            }
        }


        private void EnableControl(Boolean Status)
        {
            txtPlanDt.Enabled = Status;
            ddlAGM.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlBranch.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtPlanDt.Text = "";
            ddlAGM.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            SetInitialRow();
        }

        /// <summary>
        /// 
        /// </summary>
        private void popAGM()
        {
            DataTable dt = null;
            CEO oRO = null;
            try
            {
                oRO = new CEO();
                dt = oRO.GetEOByDesignation("AGM");
                ddlAGM.DataSource = dt;
                ddlAGM.DataTextField = "EoName";
                ddlAGM.DataValueField = "EmpCode";
                ddlAGM.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAGM.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CAudit oAd = null;
            try
            {
                oAd = new CAudit();
                dt = oAd.GetAuditPlanListPG(pPgIndx, ref vTotRows);
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
                oAd = null;
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


        private void SetInitialRow()
        {
            DataTable dt=null;
            DataTable dtEo = null;
            CEO oEo = null;
            try
            {
                dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
                dt.Columns.Add(new DataColumn("EoID", typeof(string)));
                dt.Columns.Add(new DataColumn("StartDt", typeof(string)));
                dt.Columns.Add(new DataColumn("EndDt", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["EoID"] = "";
                dr["StartDt"] = "";
                dr["EndDt"] = "";
                dt.Rows.Add(dr);
                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;
                gvBranch.DataSource = dt;
                gvBranch.DataBind();

                oEo = new CEO();
                dtEo = oEo.GetEOByDesignation("RO");
                DropDownList ddlInspector = (DropDownList)gvBranch.Rows[0].FindControl("ddlInspector");

                ddlInspector.DataSource = dtEo;
                ddlInspector.DataTextField = "EoName";
                ddlInspector.DataValueField = "EoId";
                ddlInspector.DataBind();

                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlInspector.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtEo = null;
                oEo = null;
            }
        }


        public void AddNewRowToGrid()
        {
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        drCurrentRow = dtCurrentTable.NewRow();
                        drCurrentRow["SlNo"] = dtCurrentTable.Rows.Count + 1;
                        dtCurrentTable.Rows.Add(drCurrentRow);
                        //Store the current data to ViewState for future reference
                        ViewState["CurrentTable"] = dtCurrentTable;
                        for (int i = 0; i < dtCurrentTable.Rows.Count - 1; i++)
                        {
                            TextBox txtStDt = (TextBox)gvBranch.Rows[i].FindControl("txtStDt");
                            TextBox txtEndDt = (TextBox)gvBranch.Rows[i].FindControl("txtEndDt");
                            //extract the DropDownList Selected Items
                            DropDownList ddlInspector = (DropDownList)gvBranch.Rows[i].FindControl("ddlInspector");
                            dtCurrentTable.Rows[i]["StartDt"] = txtStDt.Text;
                            dtCurrentTable.Rows[i]["EndDt"] = txtEndDt.Text;
                            // Update the DataRow with the DDL Selected Items
                            dtCurrentTable.Rows[i]["EoId"] = ddlInspector.SelectedValue;
                        }
                        //Rebind the Grid with the current data to reflect changes
                        gvBranch.DataSource = dtCurrentTable;
                        gvBranch.DataBind();
                    }
                }

                //Set Previous Data on Postbacks
                SetPreviousData();
            }
            catch
            {
            }
            finally
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public void SetPreviousData()
        {
            int rowIndex = 0;
            CEO oEo = null;
            DataTable dtEo = null;
            try
            {
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox txtStDt = (TextBox)gvBranch.Rows[i].FindControl("txtStDt");
                            TextBox txtEndDt = (TextBox)gvBranch.Rows[i].FindControl("txtEndDt");
                            DropDownList ddlInspector = (DropDownList)gvBranch.Rows[i].FindControl("ddlInspector");
                            oEo = new CEO();
                            dtEo = oEo.GetEOByDesignation("RO");
                            DataRow dr = null;
                            dr = dtEo.NewRow();
                            dr["EoId"] = 0;
                            dr["EoName"] = string.Empty;
                            dtEo.Rows.InsertAt(dr, dtEo.Rows.Count + 1);
                            dtEo.Rows[dtEo.Rows.Count - 1]["EoId"] = dtEo.Rows[0]["EoId"];
                            dtEo.Rows[dtEo.Rows.Count - 1]["EoName"] = dtEo.Rows[0]["EoName"];
                            dtEo.Rows[0]["EoId"] = "-1";
                            dtEo.Rows[0]["EoName"] = "<--Select-->";
                            ddlInspector.DataSource = dtEo;
                            ddlInspector.DataTextField = "EoName";
                            ddlInspector.DataValueField = "EoId";
                            ddlInspector.DataBind();

                            if (i < dt.Rows.Count - 1)
                            {
                                //Set the Previous Selected Items on Each DropDownList  on Postbacks
                                ddlInspector.ClearSelection();
                                ddlInspector.SelectedIndex = ddlInspector.Items.IndexOf(ddlInspector.Items.FindByValue(dt.Rows[i]["EoId"].ToString()));//.Selected = true;
                                txtStDt.Text = dt.Rows[i]["StartDt"].ToString();
                                txtEndDt.Text = dt.Rows[i]["EndDt"].ToString();
                            }
                            rowIndex++;
                        }
                    }
                    ViewState["CurrentTable"] = dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vAdPlanId = "";
            DataTable dt = null;
            DataTable dtDtl = null;
            CAudit oAu = null;
            DataRow dr;
            try
            {
                vAdPlanId = Convert.ToString(e.CommandArgument);
                ViewState["AdPlanId"] = vAdPlanId;
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
                    oAu = new CAudit();
                    dt = oAu.GetAuditPlanByID(vAdPlanId);
                    dtDtl = oAu.GetAuditDtlById(vAdPlanId);
                    if (dt.Rows.Count > 0)
                    {
                        txtPlanDt.Text = Convert.ToString(dt.Rows[0]["AdPlanDt"]);
                        ddlAGM.SelectedIndex = ddlAGM.Items.IndexOf(ddlAGM.Items.FindByValue(dt.Rows[0]["AgmCode"].ToString()));
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }

                    if (dtDtl.Rows.Count > 0)
                    {
                        dr = dtDtl.NewRow();
                        dr["EoID"] = "";
                        dr["StartDt"] = "";
                        dr["EndDt"] = "";
                        dr["SlNo"] = Convert.ToInt32(dtDtl.Rows[dtDtl.Rows.Count - 1]["SlNo"]) + 1;
                        dtDtl.Rows.InsertAt(dr, dtDtl.Rows.Count + 1);
                        ViewState["CurrentTable"] = dtDtl;
                        gvBranch.DataSource = dtDtl;
                        gvBranch.DataBind();
                        SetPreviousData();
                    }
   
                }
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }


        protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRow = 0, vMaxRow = 0;
            GridViewRow Row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            vRow = Row.RowIndex;
            vMaxRow = gvBranch.Rows.Count;
            DataTable dtDtl = null;
            dtDtl = (DataTable)ViewState["CurrentTable"];
            if (vRow != vMaxRow - 1)
            {
                if (e.CommandName == "cmdDelRec")
                {
                    dtDtl.Rows.RemoveAt(vRow);
                    dtDtl.AcceptChanges();
                    ViewState["CurrentTable"] = dtDtl;
                    gvBranch.DataSource = dtDtl;
                    gvBranch.DataBind();
                }
                SetPreviousData();
            }
        }


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
            ClearControls();
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
            //string vEoId = Convert.ToString(ViewState["EoId"]);
            //if (this.CanEdit == "N")
            //{
            //    gblFuction.MsgPopup(MsgAccess.Edit);
            //    return;
            //}
            //tbEmp.ActiveTabIndex = 1;
            //ViewState["StateEdit"] = "Edit";
            //StatusButton("Edit");
            ////GetSupervisor(vEoId, "E");
            //if (ddlHo.SelectedIndex > 0 && ddlHo.SelectedValue == "H") ddlBranch.Enabled = false;
            //if (ddlHo.SelectedIndex > 0 && ddlHo.SelectedValue == "N") ddlBranch.Enabled = true;
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
            if (ddlBranch.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Branch cannot be empty");
                vResult = false;
                return vResult;
            }
            
            return vResult;
        }


        private Boolean SaveRecords(string Mode)
        {
            string vXml = "";
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vAdPlanID = Convert.ToString(ViewState["AdPlanId"]);
            Int32 vErr = 0;
            DateTime vPlanDt = gblFuction.setDate(txtPlanDt.Text);
            CAudit oAu = null;
 
            try
            {
                if (ValidateField() == false)
                    return false;

                if (Mode == "Save")
                {
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EmpCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Save");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code can not be Duplicate...");
                    //    return false;
                    //}
                    vXml = DtToXml(DtBranch());
                    vErr = oAu.InsertAuditPlan(ref vAdPlanID, ddlBranch.SelectedValue, vPlanDt, ddlAGM.SelectedValue, this.UserID, "Save", vXml);
                    if (vErr > 0)
                    {
                        ViewState["AdPlanId"] = vAdPlanID;
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
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDuplicate("EOMst", "EMPCode", txtEmpCode.Text.Replace("'", "''"), "", "", "EOID", vEoId.ToString(), "Edit");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("EMP Code Can not be Duplicate...");
                    //    return false;
                    //}

                    vXml = DtToXml(DtBranch());
                    vErr = oAu.InsertAuditPlan(ref vAdPlanID, ddlBranch.SelectedValue, vPlanDt, ddlAGM.SelectedValue, this.UserID, "Edit", vXml);
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
                    oAu = new CAudit();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    //oEo = new CEO();
                    vErr = oAu.InsertAuditPlan(ref vAdPlanID, ddlBranch.SelectedValue, vPlanDt, ddlAGM.SelectedValue, this.UserID, "Delet", vXml);
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
                oAu = null;
                //oGbl = null;
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable DtBranch()
        {
            DataTable dt = new DataTable("AuditPlanDtl");
            DataRow dr;
            int vSlNo = 1;
            dt.Columns.Add(new DataColumn("SlNo"));
            dt.Columns.Add(new DataColumn("InspectorId"));
            dt.Columns.Add(new DataColumn("StartDt"));
            dt.Columns.Add(new DataColumn("EndDt"));

            foreach (GridViewRow gr in gvBranch.Rows)
            {
                DropDownList ddlInspector = (DropDownList)gr.FindControl("ddlInspector");
                TextBox txtStDt = (TextBox)gr.FindControl("txtStDt");
                TextBox txtEndDt = (TextBox)gr.FindControl("txtEndDt");
                if (ddlInspector.SelectedIndex >0 && txtStDt.Text !="" && txtEndDt.Text !="")
                {
                    dr = dt.NewRow();
                    dr["SlNo"] = vSlNo;
                    dr["InspectorId"] = ddlInspector.SelectedValue;
                    dr["StartDt"] =gblFuction.setDate(txtStDt.Text);
                    dr["EndDt"] = gblFuction.setDate(txtEndDt.Text);
                    dt.Rows.Add(dr);
                    vSlNo = vSlNo + 1;
                }
            }
            return dt;
        }


        protected void txtEndDt_TextChanged(object sender, EventArgs e)
        {
            int rowindex = 0, vMaxRow = 0;
            TextBox txtVal = (TextBox)sender;
            GridViewRow gvr = (GridViewRow)txtVal.NamingContainer;
            rowindex = gvr.RowIndex;
            vMaxRow = gvBranch.Rows.Count;

            TextBox txtEndDt = (TextBox)gvBranch.Rows[rowindex].FindControl("txtEndDt");
            TextBox txtStDt = (TextBox)gvBranch.Rows[rowindex].FindControl("txtStDt");
            DropDownList ddlInspector = (DropDownList)gvBranch.Rows[rowindex].FindControl("ddlInspector");

            if (gblFuction.IsDate(txtStDt.Text) == false)
                gblFuction.AjxMsgPopup("Start Date is invalid");

            if (gblFuction.IsDate(txtEndDt.Text) == false)
                gblFuction.AjxMsgPopup("End Date is invalid");


            if (gblFuction.setDate(txtEndDt.Text) < gblFuction.setDate(txtStDt.Text))
            {
                gblFuction.AjxMsgPopup("end date cannot be greater than start date");
                return;
            }

            if (rowindex == vMaxRow - 1)
            {
                if (txtEndDt.Text.Trim() != "" && txtStDt.Text.Trim() != "" && ddlInspector.SelectedIndex > 0)
                {
                    AddNewRowToGrid();
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please feed all values in Material Details");
                    txtEndDt.Text = "";
                }
            }

            //up1.Update();
        }


    }
}
