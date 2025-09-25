using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Globalization;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class GroupMeetDay : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                PopRO();
                LoadGrid(1);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

                tabGrpMeet.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCO.DataSource = dt;
                ddlCO.DataTextField = "EoName";
                ddlCO.DataValueField = "Eoid";
                ddlCO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCO.Items.Insert(0, oli);
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = " Group Rescheduling";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGrMetDay);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                   // btnCancel.Visible = false;
                   // btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Group Meeting Day Change", false);
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
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
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
            txtDayForwrd.Text = "0";
            ddlReson.SelectedIndex = -1;
            ddlReschBy.SelectedIndex = -1;
            txtAsOnDt.Text = "";
            ddlCollDay.SelectedIndex = -1;
            ddlCO.SelectedIndex = -1;
            chkSelect.Checked = false;
            gvDtl.DataSource = null;
            gvDtl.DataBind();
            //txtToDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            //txtFrmDt.Text = System.DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            //lblUser.Text = "";
            //lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {
            ddlCO.Enabled = Status;
            ddlCollDay.Enabled = Status;
            txtAsOnDt.Enabled = Status;
            txtDayForwrd.Enabled = Status;
            ddlReson.Enabled = Status;
            ddlReschBy.Enabled = Status;
            chkSelect.Enabled = Status;
            gvDtl.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CReScheduling oRsh = null;
            Int32 vRows = 0;
            try
            {
                oRsh = new CReScheduling();
                dt = oRsh.GetGroupMeetingList(pPgIndx, ref vRows, Session[gblValue.BrnchCode].ToString(), gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text));
                gvGrpMeet.DataSource = dt.DefaultView;
                gvGrpMeet.DataBind();
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
                oRsh = null;
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
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
                    break;
            }
            LoadGrid(cPgNo);
            tabGrpMeet.ActiveTabIndex = 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGrpMeet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            Int32 vReSchID = 0;
            vReSchID = Convert.ToInt32(e.CommandArgument);
            string vBranch = Session[gblValue.BrnchCode].ToString();
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvGrpMeet.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                CReScheduling oGR = new CReScheduling();
                dt = oGR.GetGroupReschById(vReSchID, vBranch);
                if (dt.Rows.Count > 0)
                {
                    ddlCO.SelectedIndex = ddlCO.Items.IndexOf(ddlCO.Items.FindByValue(Convert.ToString(dt.Rows[0]["EOId"]).Trim()));
                    txtAsOnDt.Text = Convert.ToString(dt.Rows[0]["ModDate"]);
                    txtDayForwrd.Text = Convert.ToString(dt.Rows[0]["DaysForward"]);
                    ddlReschBy.SelectedIndex = ddlReschBy.Items.IndexOf(ddlReschBy.Items.FindByValue(Convert.ToString(dt.Rows[0]["ReSchdlBy"]).Trim()));
                    ddlReson.SelectedIndex = ddlReson.Items.IndexOf(ddlReson.Items.FindByValue(Convert.ToString(dt.Rows[0]["ReasonId"]).Trim()));
                    //lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                    //lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                    GetReschGroupList(Convert.ToInt32(dt.Rows[0]["ReSchdlId"]), vBranch);
                    tabGrpMeet.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
                else
                {
                    //ddlCO.SelectedIndex = -1;
                    //txtDmndDt.Text = "";
                    //txtResDt.Text = "";
                    //txtDayNo.Text = "";
                    //ddlresn.SelectedIndex = -1;
                    //ddlResBy.SelectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pReSchdlId"></param>
        private void GetReschGroupList(Int32 pReSchdlId, string pBranch)
        {
            DataTable dt = null;
            CReScheduling oGR = new CReScheduling();
            dt = oGR.GetGroupScheduleDtl(pBranch, pReSchdlId);
            gvDtl.DataSource = dt;
            gvDtl.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ViewState["ReGroupForSch"] = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            DateTime vDemandDt = gblFuction.setDate(txtAsOnDt.Text);
            DateTime vLogDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CReScheduling oRS = new CReScheduling();
            if (vLogDate > vDemandDt)
            {
                gblFuction.AjxMsgPopup("Demand Date should be geter than login Date.");
                return;
            }
            if (ddlCO.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Select RO.");
                return;
            }

            Int32 vOWkNo = 0;
            CultureInfo ciCurr = CultureInfo.CurrentCulture;

            vOWkNo = Convert.ToInt32(ciCurr.Calendar.GetDayOfWeek(vDemandDt));
            if (Convert.ToInt32(ddlCollDay.SelectedValue) != vOWkNo)
            {
                gblFuction.AjxMsgPopup("Demand Date should Match with Collection Day.");
                return;
            }

            dt = oRS.GetGroupForSch(ddlCO.SelectedValue.ToString(),vDemandDt, vBranch);
            ViewState["ReGroupForSch"] = dt;
            gvDtl.DataSource = dt;
            gvDtl.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRes = (CheckBox)e.Row.FindControl("chkRes");
                if (e.Row.Cells[3].Text == "Y")
                    chkRes.Checked = true;
                else
                    chkRes.Checked = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["ReGroupForSch"];
            DataTable dtColl = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            DateTime vLastDate;
            CReScheduling oRS = new CReScheduling();
            if (Convert.ToInt32(txtDayForwrd.Text) == 0)
            {
                gblFuction.AjxMsgPopup("Please Enter the No. of Days Forward/Backward");
                chkSelect.Checked = false;
                return;
            }
            if (chkSelect.Checked == true)
            {
                foreach (GridViewRow gR in gvDtl.Rows)
                {
                    CheckBox chkRes = (CheckBox)gR.FindControl("chkRes");
                    chkRes.Checked = true;
                    dt.Rows[gR.RowIndex]["ReshdlYes"] = "Y";
                    vLastDate = gblFuction.setDate(txtAsOnDt.Text).AddDays(Convert.ToInt32(txtDayForwrd.Text));
                    dtColl = oRS.GetMaxCollDateReSh(Convert.ToString(dt.Rows[gR.RowIndex]["GroupID"]), vBranch);
                    if (dtColl.Rows[0]["AccDate"] == null || Convert.ToString(dtColl.Rows[0]["AccDate"]) == "")
                    {
                        gblFuction.AjxMsgPopup("Reschedule only possible after atleast one collection");
                        chkRes.Checked = false;
                        dt.Rows[gR.RowIndex]["ReshdlYes"] = "N";
                        return;
                    }
                    else
                    {
                        if (gblFuction.setDate(Convert.ToString(dtColl.Rows[0]["AccDate"])) >= vLastDate)
                        {
                            gblFuction.AjxMsgPopup("Reschedule not possible before last collection");
                            chkRes.Checked = false;
                            dt.Rows[gR.RowIndex]["ReshdlYes"] = "N";
                            return;
                        }
                    }


                }
                dt.AcceptChanges();
                ViewState["ReGroupForSch"] = dt;
            }
            else
            {
                foreach (GridViewRow gR in gvDtl.Rows)
                {
                    CheckBox chkRes = (CheckBox)gR.FindControl("chkRes");
                    chkRes.Checked = false;
                    dt.Rows[gR.RowIndex]["ReshdlYes"] = "N";
                }
            }
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        protected void chkRes_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtColl = null;
            Int32 vRow = 0;
            DateTime vLastDate;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            vRow = row.RowIndex;
            CheckBox chkReshdl = (CheckBox)row.FindControl("chkRes");
            DataTable dt = (DataTable)ViewState["ReGroupForSch"];
            CReScheduling oRS = new CReScheduling();
            if (Convert.ToInt32(txtDayForwrd.Text) == 0)
            {
                gblFuction.AjxMsgPopup("Please enter the No. of Days Forward/Backward");
                chkReshdl.Checked = false;
                return;
            }
            vLastDate = gblFuction.setDate(txtAsOnDt.Text).AddDays(Convert.ToInt32(txtDayForwrd.Text));
            dtColl = oRS.GetMaxCollDateReSh(Convert.ToString(dt.Rows[vRow]["GroupID"]), vBranch);
            if (dtColl.Rows[0]["AccDate"] == null || Convert.ToString(dtColl.Rows[0]["AccDate"]) == "")
            {
                gblFuction.AjxMsgPopup("Reschedule only possible after atleast one collection ");
                chkReshdl.Checked = false;
                return;
            }
            else
            {
                if (gblFuction.setDate(Convert.ToString(dtColl.Rows[0]["AccDate"])) >= vLastDate)
                {
                    gblFuction.AjxMsgPopup("Reschedule not possible before last collection ");
                    chkReshdl.Checked = false;
                    return;
                }
            }

            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vLastDate.AddDays(-1))
            {
                gblFuction.AjxMsgPopup("Reschedule not possible because day end already done on " + Session[gblValue.EndDate].ToString());
                chkReshdl.Checked = false;
                return;
            }

            if (chkReshdl.Checked == true)
                dt.Rows[vRow]["ReshdlYes"] = "Y";
            else
                dt.Rows[vRow]["ReshdlYes"] = "N";

            dt.AcceptChanges();
            ViewState["ReGroupForSch"] = dt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtDayNo_TextChanged(object sender, EventArgs e)
        {
            Int32 vOWkNo = 0, vMWkNo = 0;
            CultureInfo ciCurr = CultureInfo.CurrentCulture;


            if (txtDayForwrd.Text.Trim() != "" && Convert.ToInt32(txtDayForwrd.Text) != 0)
                chkSelect.Enabled = true;
            DateTime vDemandDt = gblFuction.setDate(txtAsOnDt.Text);
            DateTime vModifyDt = vDemandDt.AddDays(Convert.ToInt32(txtDayForwrd.Text));
            vOWkNo = ciCurr.Calendar.GetWeekOfYear(vDemandDt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            vMWkNo = ciCurr.Calendar.GetWeekOfYear(vModifyDt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            //txtModiDt.Text = Convert.ToString(vModifyDt.ToString("D"));
            //ViewState["ModifyDt"] = vModifyDt;
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
                    gblFuction.AjxMsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tabGrpMeet.ActiveTabIndex = 1;
                StatusButton("Add");
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
            try
            {
                tabGrpMeet.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
                    gblFuction.AjxMsgPopup(MsgAccess.Del);
                    return;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    //LoadGrid(0);
                    StatusButton("View");
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
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dtSchd = null;
            //DataTable dtLSchd = null;
            //DataTable dtGroup = null;
            Boolean vResult = false;
            string vXmlSch = string.Empty;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vReason = Convert.ToInt32(ddlReson.SelectedValue);
            Int32 vScheduleBy = Convert.ToInt32(ddlReschBy.SelectedValue);
            Int32 vNoofdays = 0, vErr = 0;
            DateTime vReSchDate = gblFuction.setDate(txtAsOnDt.Text);
            

            CReScheduling oRS = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            dtSchd = (DataTable)ViewState["ReGroupForSch"];
            foreach (DataRow row in dtSchd.Rows)
            {
                if (Convert.ToString(row["ReshdlYes"]) == "N")
                    row.Delete();
            }
            dtSchd.AcceptChanges();
            if (dtSchd == null)
            {
                gblFuction.AjxMsgPopup("Please click show button");
                return false;
            }
            if (txtDayForwrd.Text == "")
                vNoofdays = 0;
            else
                vNoofdays = Convert.ToInt32(txtDayForwrd.Text);

            try
            {
                if (Mode == "Save")
                {
                    if (ValidateFields() == false)
                        return false;
                    oRS = new CReScheduling();

                   


                    vXmlSch = DataTableTOXml(dtSchd);
                    vErr = oRS.InsertReSchdlGroup(ddlCO.SelectedValue.ToString(), vNoofdays, vReSchDate, vReason, vScheduleBy,
                                vBrCode, this.UserID, "I", vXmlSch);
                    if (vErr == 0)
                    {
                        gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    gblFuction.AjxMsgPopup("Edit not possible.");
                    return false;
                }
                else if (Mode == "Delete")
                {
                    gblFuction.AjxMsgPopup("Delete not possible.");
                    return false;
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            Int32 vNoofGroup = 0;
            DataTable dt = (DataTable)ViewState["ReGroupForSch"];
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToString(dr["ReshdlYes"]) == "Y")
                    vNoofGroup = vNoofGroup + 1;
            }
            if (vNoofGroup == 0)
            {
                gblFuction.AjxMsgPopup("Please select atleast one Group");
                vResult = false;
                return vResult;
            }
            if (txtDayForwrd.Text.Trim() == "" || txtDayForwrd.Text.Trim() == "0")
            {
                gblFuction.AjxMsgPopup("No of Day Extend should not be zero");
                vResult = false;
                return vResult;
            }
           

            if (Convert.ToInt32(ddlReson.SelectedValue) <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select the Reson");
                vResult = false;
                return vResult;
            }
            if (Convert.ToInt32(ddlReschBy.SelectedValue) <= 0)
            {
                gblFuction.AjxMsgPopup("Please Select the reschedule by");
                vResult = false;
                return vResult;
            }
            if (txtAsOnDt.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Please Select the Demand Date");
                vResult = false;
                return vResult;
            }
            return vResult;
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
                    gblFuction.AjxMsgPopup(MsgAccess.Edit);
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShowLst_Click(object sender, EventArgs e)
        {
            LoadGrid(Int32.Parse(lblCurrentPage.Text));
        }
    }
}

