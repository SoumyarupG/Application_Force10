using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BankDetailsOfDeathMember : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                
                popBank();
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                txtLnDt.Text = (string)Session[gblValue.LoginDate];
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLoanDisb.ActiveTabIndex = 0;
                ClearControls();
                EnableControl(false);
                //gblFuction.AjxMsgPopup(((int)Convert.ToDateTime(txtLnDt.Text).DayOfWeek).ToString());

            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Bank Details of Death Member";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnubankDetailDeathMem);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (this.UserID == 1) return;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Bank Details of Death Member", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CDeathDocSnt oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = (string)Session[gblValue.BrnchCode];
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CDeathDocSnt();

            dt = oLD.GetAccListAfterDeath(vFrmDt, vToDt, vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vRows);

            gvLoanAppl.DataSource = dt.DefaultView;
            gvLoanAppl.DataBind();
            lblTotalPages.Text = CalTotPgs(vRows).ToString();
            lblCurrentPage.Text = cPgNo.ToString();
            if (cPgNo == 1)
            {
                Btn_Previous.Enabled = false;
                if (Int32.Parse(lblTotalPages.Text) > 1)
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

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
        }

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
                tabLoanDisb.ActiveTabIndex = 1;
                //tabLoanDisb.Tabs[0].Enabled = false;
                //tabLoanDisb.Tabs[1].Enabled = true;
                //tabLoanDisb.Tabs[2].Enabled = false;
                StatusButton("Add");
                ClearControls();
                popRO();
                txtLnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
            //tabLoanDisb.Tabs[0].Enabled = true;
            //tabLoanDisb.Tabs[1].Enabled = false;
            //tabLoanDisb.Tabs[2].Enabled = false;
            EnableControl(false);
            StatusButton("View");
        }
        
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
                    //LoadGrid(1);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private bool ValidaeField()
        {
            bool vRes = true;
            DateTime vLnDt, vLogDt;
            string vMsg = "";
            CDisburse oLD = null;

            vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vUserId = Session[gblValue.UserId].ToString();
            Int32 vUsrId = Convert.ToInt32(vUserId);

            if (ddlCo.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("CO Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCo");
                return vRes = false;
            }
            
            if (ddlGrp.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Group Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlGrp");
                return vRes = false;
            }

            if (ddlMembr.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Member Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlMembr");
                return vRes = false;
            }

            if (gblFuction.IsDate(txtLnDt.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Loan Date Should be in DD/MM/YYYY Format ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtLnDt");
                return vRes = false;
            }
            return vRes;
        }

        private Boolean SaveRecords(string Mode)  //Check Account
        {
            Boolean vResult = false;
            Int32 vErr = 0, vNewId = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vID = Convert.ToString(ViewState["DeathMemInfoID"]);
            if (ValidaeField() == false)
                return false;
            string vBank = "", vGroupId = "", vEoId = "", vMember = "", vBankBranch= "", vIFSC="", vAccNo="", vBenefName="", vLoanId="";
            CCollectionRoutine oCR = null;
            CGblIdGenerator oGbl = null;
            CDeathDocSnt oDD = null;
            try
            {
                oDD = new CDeathDocSnt();
                vEoId = ddlCo.SelectedValue;
                vGroupId = ddlGrp.SelectedValue;
                vMember = ddlMembr.SelectedValue;
                vBank = ddlBankName.SelectedValue;
                //DateTime vDate = gblFuction.setDate(txtLnDt.Text);
                DateTime vDate = gblFuction.setDate(txtLnDt.Text);
                vBankBranch = txtBranch.Text.ToString();
                vIFSC = txtIFSC.Text.ToString();
                vAccNo = txtAccNo.Text.ToString();
                vBenefName = txtBeneficiaryName.Text.ToString();
                vLoanId = ddlLoan.SelectedValue;

                if (Convert.ToInt32(vLoanId) != -1)
                {
                    if (Mode == "Save")
                    {
                        vErr = oDD.ChkDuplicateDeathMember(vMember, vLoanId, vBrCode);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("Already an account exists for this member after death..");
                            return false;
                        }

                        vErr = oDD.SaveBankInfoDeathMember(ref vNewId, vID, vBenefName, vEoId, vGroupId, vMember, vLoanId, vBank, vDate, vBankBranch, vIFSC, vAccNo, vBrCode, this.UserID, "Save");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("Information Saved Successfully");
                            ViewState["DeathMemInfoID"] = vNewId;
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
                        //vErr = oDD.ChkDuplicateDeathMember(vMember, vLoanId, vBrCode);
                        //if (vErr > 0)
                        //{
                        //    gblFuction.MsgPopup("Already an account exists for this member after death..");
                        //    return false;
                        //}

                        vErr = oDD.SaveBankInfoDeathMember(ref vNewId, vID, vBenefName, vEoId, vGroupId, vMember, vLoanId, vBank, vDate, vBankBranch, vIFSC, vAccNo, vBrCode, this.UserID, "Edit");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("Information Updated Successfully");
                            ViewState["DeathMemInfoID"] = vNewId;
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
                        vErr = oDD.SaveBankInfoDeathMember(ref vNewId, vID, vBenefName, vEoId, vGroupId, vMember, vLoanId, vBank, vDate, vBankBranch, vIFSC, vAccNo, vBrCode, this.UserID, "Delete");
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
                }
                else
                {
                    gblFuction.MsgPopup("Please select one loan No.");
                    vResult = false;
                }
                return vResult;
            }
            finally
            {
                oCR = null;
                oGbl = null;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ddlCo.Enabled = false;
                ddlGrp.Enabled = false;
                ddlMembr.Enabled = false;
                ddlLoan.Enabled = false;
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            ViewState["DeathMemInfoID"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";

            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                //LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
                    //btnCanDisb.Enabled = true;
                    //btnPostpond.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    //btnCanDisb.Enabled = false;
                    //btnPostpond.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    //btnCanDisb.Enabled = false;
                    //btnPostpond.Enabled = false;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    //btnCanDisb.Enabled = false;
                    //btnPostpond.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    //btnCanDisb.Enabled = false;
                    //btnPostpond.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void EnableControl(Boolean Status)
        {
            ddlCo.Enabled = Status;
            ddlGrp.Enabled = Status;
            ddlMembr.Enabled = Status;
            ddlLoan.Enabled = Status;
            txtLnDt.Enabled = Status;
            ddlBankName.Enabled = Status;
            txtBranch.Enabled = Status;
            txtAccNo.Enabled = Status;
            txtIFSC.Enabled = Status;
            txtBeneficiaryName.Enabled = Status;
        }

        private void ClearControls()
        {            
            ddlCo.SelectedIndex = -1;
            ddlGrp.SelectedIndex = -1;;
            ddlMembr.SelectedIndex = -1;
            txtLnDt.Text = Session[gblValue.LoginDate].ToString();
            //txtNEFTDt.Text = "";
            //txtNeftNo.Text = "";            
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
            try
            {
                ddlCo.Items.Clear();
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popBank()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BankId", "BankName", "BankMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBankName.DataSource = dt;
                ddlBankName.DataTextField = "BankName";
                ddlBankName.DataValueField = "BankId";
                ddlBankName.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBankName.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopGroup(ddlCo.SelectedValue);
        }

        private void PopGroup(string vCenterID)
        {
            ddlGrp.Items.Clear();
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLogDt = gblFuction.setDate(txtLnDt.Text);
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopTransferMIS("Y", "GroupMst", vCenterID, vLogDt, vBrCode);
                //dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);

                ddlGrp.DataSource = dt;
                ddlGrp.DataTextField = "GroupName";
                ddlGrp.DataValueField = "GroupID";
                ddlGrp.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGrp.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        private void PopLoan(string vMemberID)
        {
            ddlLoan.Items.Clear();

            DataTable dt = null;
            try
            {
                CDeathDocSnt oMem = new CDeathDocSnt();
                if (Convert.ToString(ddlMembr.SelectedValue) != "-1")
                {
                    dt = oMem.PopDeathMemberLoan(ddlMembr.SelectedValue, Session[gblValue.BrnchCode].ToString());

                    ddlLoan.DataTextField = "LoanNo";
                    ddlLoan.DataValueField = "LoanId";
                    ddlLoan.DataSource = dt;
                    ddlLoan.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlLoan.Items.Insert(0, oItm);
                }
                ddlLoan.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGrp.SelectedIndex > 0)
            {
                PopMember(ddlGrp.SelectedValue);
            }
        }

        protected void ddlMembr_SelectedIndexChanged(object sendre, EventArgs e)
        {
            if (ddlMembr.SelectedIndex > 0)
            {
                PopLoan(ddlMembr.SelectedValue);
            }
        }
        
        private void PopMember(string vGroupID)
        {
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLoginDt = gblFuction.setDate(txtLnDt.Text);
            DataTable dt = null;
            try
            {
                CMember oMem = new CMember();
                if (Convert.ToString(ddlCo.SelectedValue) != "-1")
                {
                    dt = oMem.PopGrpMember(ddlGrp.SelectedValue, Session[gblValue.BrnchCode].ToString(), vLoginDt, "M");

                    ddlMembr.DataTextField = "MemberName";
                    ddlMembr.DataValueField = "MemberId";
                    ddlMembr.DataSource = dt;
                    ddlMembr.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlMembr.Items.Insert(0, oItm);
                }
                ddlMembr.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            //DataTable dtDtl = null;
            CDeathDocSnt oLD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString(); //gblFuction.setDate(txtFrmDt.Text);
            DateTime loginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vId = "";
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    oLD = new CDeathDocSnt();
                    vId = Convert.ToString(e.CommandArgument);
                    ViewState["DeathMemInfoID"] = vId;

                    dt = oLD.GetInfoById(vId, loginDt);

                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                        foreach (GridViewRow gr in gvLoanAppl.Rows)
                        {
                            LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                        }
                        btnShow.ForeColor = System.Drawing.Color.Red;
                        //Load Field
                        txtAccNo.Text = Convert.ToString(dt.Rows[0]["AccNo"]);
                        txtBeneficiaryName.Text = Convert.ToString(dt.Rows[0]["BeneficiaryName"]);
                        txtLnDt.Text = Convert.ToString(dt.Rows[0]["Date"]);
                        txtIFSC.Text = Convert.ToString(dt.Rows[0]["IFSCCode"]);
                        txtBranch.Text = Convert.ToString(dt.Rows[0]["BankBranch"]);
                        //popBank();
                        ddlBankName.SelectedIndex = ddlBankName.Items.IndexOf(ddlBankName.Items.FindByValue(dt.Rows[0]["bankid"].ToString()));
                        popRO();
                        ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopGroup(ddlCo.SelectedValue);
                        ddlGrp.SelectedIndex = ddlGrp.Items.IndexOf(ddlGrp.Items.FindByValue(dt.Rows[0]["Groupid"].ToString()));
                        PopMember(ddlCo.SelectedValue);
                        ddlMembr.SelectedIndex = ddlMembr.Items.IndexOf(ddlMembr.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        PopLoan(ddlMembr.SelectedValue);
                        ddlLoan.SelectedIndex = ddlLoan.Items.IndexOf(ddlLoan.Items.FindByValue(dt.Rows[0]["LoanId"].ToString()));
                    }
                    tabLoanDisb.ActiveTabIndex = 1;
                    //tabLoanDisb.Tabs[0].Enabled = false;
                    //tabLoanDisb.Tabs[1].Enabled = true;               
                    //tabLoanDisb.Tabs[2].Enabled = false;
                    StatusButton("Show");
                    EnableControl(false);
                }
            }
            finally
            {
                dt = null;
                oLD = null;
            }
        }
    }
}