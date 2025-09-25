using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LnReschedule : CENTRUMBase
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

                PopCM();
                LoadGrid(0);
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(Int32.Parse(lblCurrentPage.Text));
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Rescheduling";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanReschedul);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Individual Loan Rescheduling", false);
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CReScheduling oRsh = null;
            Int32 vRows = 0;
            try
            {
                oRsh = new CReScheduling();
                dt = oRsh.GetReSchedulingPG(pPgIndx, ref vRows, Session[gblValue.BrnchCode].ToString(), gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text));
                gvReSchdl.DataSource = dt.DefaultView;
                gvReSchdl.DataBind();
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
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dt = null;
            Boolean vResult = false;
            Int32 vRescheduleId = Convert.ToInt32(ViewState["RescheduleId"]);
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);

            Int32 vFInstNo = 0, vNoofdays = 0, vErr = 0;
            string vXmlSch = string.Empty;
            if (Convert.ToInt32(ddlCO.SelectedValue) == -1)
            {
                gblFuction.MsgPopup("Please select the RO ");
                return false;
            }
            if (Convert.ToInt32(ddlMem.SelectedValue) == -1)
            {
                gblFuction.MsgPopup("Please select the Member which you want to re-scheduling");
                return false;
            }
            if (Convert.ToInt32(ddlLoanNo.SelectedValue) == -1)
            {
                gblFuction.MsgPopup("Please select the loan which you want to re-scheduling");
                return false;
            }
            if (ValidateFields() == false)
                return false;
            string vLoanID = ddlLoanNo.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vReason = Convert.ToInt32(ddlResn.SelectedValue);
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vReSchDate = gblFuction.setDate(txtResDt.Text);
            DateTime vHappDate = gblFuction.setDate(txtHapenDt.Text);
            DateTime vAcDueDate = gblFuction.setDate(ViewState["FromDueDt"].ToString());
            DateTime vCurrDate = gblFuction.setDate(txtCurDueDt.Text);
            CReScheduling oReSchedule = null;

            if (txtInstNo.Text == "")
                vFInstNo = 0;
            else
                vFInstNo = Convert.ToInt32(txtInstNo.Text);

            if (txtExtnd.Text == "")
                vNoofdays = 0;
            else
                vNoofdays = Convert.ToInt32(txtExtnd.Text);
            try
            {
                if (Mode == "Save")
                {
                    //if (ValidateFields() == false)
                    //    return false;

                    oReSchedule = new CReScheduling();
                    dt = (DataTable)ViewState["ReSchedule"];
                    //vXmlSch = DataTableTOXml(dt);

                    vErr = oReSchedule.InsertReScheduleMst(vLoanID, vReSchDate, vFInstNo, vAcDueDate, vNoofdays, vCurrDate, vHappDate,
                                vReason, txtRemark.Text, Convert.ToInt32(ddlResch.SelectedValue), Convert.ToInt32(ddlAprov.SelectedValue),
                                vBrCode, this.UserID, "I");

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
                    if (ValidateFields() == false)
                        return false;

                    oReSchedule = new CReScheduling();
                    dt = (DataTable)ViewState["ReSchedule"];
                    dt = (DataTable)ViewState["ReSchedule"];
                    //vXmlSch = DataTableTOXml(dt);

                    vErr = oReSchedule.UpdateReScheduleMst(vRescheduleId, vLoanID, vReSchDate, vFInstNo, vAcDueDate, vNoofdays, vCurrDate, vHappDate,
                                vReason, txtRemark.Text, Convert.ToInt32(ddlResch.SelectedValue), Convert.ToInt32(ddlAprov.SelectedValue),
                                vBrCode, this.UserID, "E");
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
            if (ddlLoanNo.SelectedIndex == -1)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Loan No.Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlLoanNo");
                vResult = false;
            }
            if (txtResDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Re-Scheduling Date Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtResDt");
                vResult = false;
            }
            if (txtHapenDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Happaning Date Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtHapenDt");
                vResult = false;
            }
            if (ddlResn.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Reason Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlResn");
                vResult = false;
            }
            if (txtInstNo.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("From Installment No. Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtInstNo");
                vResult = false;
            }
            if (txtAcDueDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Actual Due Date Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtAcDueDt");
                vResult = false;
            }
            if (txtExtnd.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("No of Days Extend Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtExtnd");
                vResult = false;
            }
            if (gblFuction.setDate(txtHapenDt.Text.Trim()) > gblFuction.setDate(txtResDt.Text.Trim()))
            {
                EnableControl(true);
                gblFuction.MsgPopup("Reschedule date should be less than happening date...");
                //gblFuction.focus("ct100_cph_Main_tbSch_PnlDtl_txtRsDate");
                vResult = false;
            }
            if (txtResDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtResDt.Text) == false)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    //gblFuction.focus("ct100_cph_Main_tbSch_PnlDtl_txtRsDate");
                    vResult = false;
                }
            }
            if (txtHapenDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtHapenDt.Text) == false)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtHapenDt");
                    vResult = false;
                }
            }
            if (ddlResch.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Reschedule by  Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlResch");
                vResult = false;
            }
            if (ddlAprov.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Approved by Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_ddlAprov");
                vResult = false;
            }
            if (txtRemark.Text == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Remarks Cannot be left blank...");
                gblFuction.focus("ct100_cph_Main_tabReSchdl_pnlDtls_txtRemark");
                vResult = false;
            }
            return vResult;
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
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
            ddlCO.SelectedIndex=-1;
            ddlMarket.SelectedIndex = -1;
            ddlGrp.SelectedIndex=-1;
            ddlMem.SelectedIndex=-1;
            ddlLoanNo.SelectedIndex=-1;
            txtResDt.Text = "";
            txtHapenDt.Text = Session[gblValue.LoginDate].ToString();
            ddlResn.SelectedIndex=-1;
            txtInstNo.Text = "";
            txtAcDueDt.Text = "";
            txtExtnd.Text = "";
            txtCurDueDt.Text = "";
            ddlAprov.SelectedIndex=-1;
            ddlResch.SelectedIndex=-1;
            txtRemark.Text = "";
            lblUser.Text = "";
            lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        { 
            ddlCO.Enabled=Status;
            ddlGrp.Enabled=Status;
            ddlMem.Enabled=Status;
            ddlLoanNo.Enabled=Status;
            txtResDt.Enabled=Status;
            txtHapenDt.Enabled=Status;
            ddlResn.Enabled=Status;
            txtInstNo.Enabled=Status;
            txtAcDueDt.Enabled=Status;
            txtExtnd.Enabled=Status;
            txtCurDueDt.Enabled=Status;
            ddlAprov.Enabled=Status;
            ddlResch.Enabled=Status;
            txtRemark.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopCM()
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
                ddlCO.DataValueField = "EoId";
                ddlCO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCO.Items.Insert(0, oli);

                ddlAprov.DataSource = dt;
                ddlAprov.DataTextField = "EoName";
                ddlAprov.DataValueField = "EoId";
                ddlAprov.DataBind();
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ddlAprov.Items.Insert(0, oli1);

                ddlResch.DataSource = dt;
                ddlResch.DataTextField = "EoName";
                ddlResch.DataValueField = "EoId";
                ddlResch.DataBind();
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ddlResch.Items.Insert(0, oli11);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCO_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopMarket(ddlCO.SelectedValue);
            ddlMarket.SelectedIndex = ddlMarket.Items.IndexOf(ddlMarket.Items.FindByValue(ddlCO.SelectedValue));
            ddlGrp.Items.Clear();
            PopGroup(ddlMarket.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMarket_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlGrp.Items.Clear();
            PopGroup(ddlMarket.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCOID"></param>
        private void PopGroup(string vMarketID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlGrp.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "AA", "GroupID", "GroupName", "GroupMst", vMarketID, "MarketID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                if (dtGr.Rows.Count > 0)
                {
                    ddlGrp.DataSource = dtGr;
                    ddlGrp.DataTextField = "GroupName";
                    ddlGrp.DataValueField = "GroupID";
                    ddlGrp.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlGrp.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCOID"></param>
        private void PopMarket(string vCOID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlMarket.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMst", vCOID, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                if (dtGr.Rows.Count > 0)
                {
                    ddlMarket.DataSource = dtGr;
                    ddlMarket.DataTextField = "Market";
                    ddlMarket.DataValueField = "MarketID";
                    ddlMarket.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlMarket.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMem.Items.Clear();
            PopMember();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopMember()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vBrCode = "", vGrpId="";            
            DateTime vLogDt = gblFuction.setDate("");
            try
            {
                ddlMem.Items.Clear();
                vGrpId = ddlGrp.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("D", "Y", "MemberNo", "MemberID", "MF_Name+' '+ MM_Name+ ' '+ML_Name", "MemberMst", vGrpId, "GroupID", "Tra_DropDate", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlMem.DataSource = dt;
                    ddlMem.DataTextField = "Name";
                    ddlMem.DataValueField = "MemberID";
                    ddlMem.DataBind();
                }
                ListItem oL1 = new ListItem("<-- Select -->", "-1");
                ddlMem.Items.Insert(0, oL1);
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlLoanNo.Items.Clear();
            popLoan();
        }

        /// <summary>
        /// 
        /// </summary>
        private void popLoan()
        {
            DataTable dt = null;
            CGblIdGenerator oCG = null;
            string vMemId = "";
            try
            {
                vMemId = ddlMem.SelectedValue;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oCG = new CGblIdGenerator();
                dt = oCG.PopComboMIS("S", "N", "AA", "LoanId", "LoanNo", "LoanMst", vMemId, "MemberId", "AA", System.DateTime.Now, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetRescheduleList(string pLoanNo)
        {
            ViewState["ReSchedule"] = null;
            DataTable dt = null;
            CReScheduling oSchedule = new CReScheduling();
            dt = oSchedule.GetLoanSchedule(Session[gblValue.BrnchCode].ToString(), pLoanNo);
            ViewState["ReSchedule"] = dt;
            gvDtl.DataSource = dt;
            gvDtl.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vInstNo = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            vInstNo = Convert.ToInt32(e.CommandArgument);
	    ViewState["InstNo"] = vInstNo;
            if (e.CommandName == "cmdShow")
            {
                DataTable dt = null;
                DataTable dtCol = null;
                String vFromDutDt;
                DateTime vLoginDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                txtInstNo.Text = "";
                txtAcDueDt.Text = "";
                foreach (GridViewRow gr in gvDtl.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                CReScheduling oReSch = new CReScheduling();
                CCollectionRoutine oColl = new CCollectionRoutine();
                //Need to Cheacking for Collection Details
                dt = oReSch.GetLoanScheduleByInstNo(vInstNo, ddlLoanNo.SelectedValue);
                if (vLoginDate > gblFuction.setDate(Convert.ToString(dt.Rows[0]["DueDtOriginal"]).ToString()))
                {
                    gblFuction.AjxMsgPopup("You Can not Reschedule before login Date.");
                    return;
                }
                dtCol = oColl.GetMaxCollDate(ddlLoanNo.SelectedValue, vBrCode, "M");
                if (dtCol.Rows.Count > 0)
                {
                    if (Convert.ToString(dtCol.Rows[0]["MaxCollDt"]) != "")
                    {
                        if (gblFuction.setDate(Convert.ToString(dtCol.Rows[0]["MaxCollDt"]).ToString()) > gblFuction.setDate(Convert.ToString(dt.Rows[0]["DueDtOriginal"]).ToString()))
                        {
                            gblFuction.AjxMsgPopup("You Can not Reschedule before Collection Date. Last Collection Date is" + Convert.ToString(dtCol.Rows[0]["MaxCollDt"]));
                            return;
                        }
                    }
                }
                ViewState["dtCol"] = dtCol;
                if (dt.Rows.Count > 0)
                {
                    txtInstNo.Text = Convert.ToString(dt.Rows[0]["InstNo"]);
                    txtAcDueDt.Text = Convert.ToDateTime(dt.Rows[0]["DueDt"]).ToString("dd/MM/yyyy");
                    vFromDutDt = dt.Rows[0]["DueDtOriginal"].ToString();
                    tabReSchdl.ActiveTabIndex = 1;
                    ViewState["FromDueDt"] = vFromDutDt;
		    txtResDt.Text = Session[gblValue.LoginDate].ToString();
		    txtHapenDt.Text = Session[gblValue.LoginDate].ToString();
                    txtExtnd.Text = "";
                    txtCurDueDt.Text = "";
                }
                else
                {
                    txtInstNo.Text = "";
                    txtAcDueDt.Text = "";
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtExtnd_TextChanged(object sender, EventArgs e)
        {
            String vFromDueDt;
            DateTime vCDueDate;
            DataTable dtCol = null;
            vFromDueDt = (String)ViewState["FromDueDt"];

	    if (vFromDueDt.Trim().Length <= 0)
	    {
	       vFromDueDt = txtAcDueDt.Text;	
	    }

            if (txtExtnd.Text != "" && txtExtnd.Text != "-")
            {
                Int32 vDayExtd = Convert.ToInt32(txtExtnd.Text);
                //if (vDayExtd % 7 == 0)
                //{
                    vCDueDate = gblFuction.setDate(vFromDueDt).AddDays(vDayExtd);
                    txtCurDueDt.Text = gblFuction.putStrDate(vCDueDate);
                    dtCol = (DataTable)ViewState["dtCol"];
                    if (dtCol.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtCol.Rows[0]["MaxCollDt"]) != "")
                        {
                            if (gblFuction.setDate(Convert.ToString(dtCol.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtCurDueDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You Can not Reschedule before Collection Date. Last Collection Date is" + Convert.ToString(dtCol.Rows[0]["MaxCollDt"]));
                                txtCurDueDt.Text = "";
                                txtExtnd.Text = "";
                                return;
                            }
                        }
                    }
                //}
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Enter No of Days ...");
                return;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStDate"></param>
        /// <param name="pColType"></param>
        /// <param name="pCount"></param>
        /// <returns></returns>
        private string getDueDate(DateTime pStDate, Int32 pCount)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vRtDt = "";


                pStDate = pStDate.AddDays(pCount);
                vRtDt = gblFuction.putStrDate(pStDate);
         
            return vRtDt;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vLoanNo = ddlLoanNo.SelectedValue;
            GetRescheduleList(vLoanNo);
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
                tabReSchdl.ActiveTabIndex = 1;
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
                tabReSchdl.ActiveTabIndex = 0;
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
                //if (this.RoleId != 1)
                //{
                //    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                //    {
                //        gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                //        return;
                //    }
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(0);
                //    StatusButton("Delete");
                //}
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.RoleId != 1)
                //{
                //    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                //    {
                //        gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                //        return;
                //    }
                //}
                if (this.CanEdit == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlCo");
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

        //protected void Page_Error(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["ErrMsg"] = sender.ToString();
        //        Response.RedirectPermanent("~/ErrorInfo.aspx", false);
        //    }
        //    catch (Exception ex)
        //    { 
        //        throw new Exception();
        //    }
        //}         
    }
}
