using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DeathDeclare : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                txtDDate.Text = (string)Session[gblValue.LoginDate];
                PopWaveofReason();
                PopLedger();
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLoanDisb.ActiveTabIndex = 0;
                ClearControls();
                EnableControl(false);
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Death Declaration";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuDeathDeclare);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Death Declaration", false);
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
            dt = oLD.GetDeathDeclareList(vFrmDt, vToDt, vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vRows);
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

                StatusButton("Add");
                ClearControls();
                popRO();
                txtDDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
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
            DateTime vLogDt;
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

            if (gblFuction.IsDate(txtDDate.Text.Trim()) == false)
            {
                gblFuction.MsgPopup("Loan Date Should be in DD/MM/YYYY Format ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtDDate");
                return vRes = false;
            }
            return vRes;
        }

        private Boolean SaveRecords(string Mode)  //Check Account
        {
            Boolean vResult = false;
            Int32 vErr = 0, vNewId = 0, vID = 0;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanId = "";
            if (ValidaeField() == false)
                return false;
            CDeathDocSnt oDD = null;
            try
            {
                oDD = new CDeathDocSnt();
                DateTime vDate = gblFuction.setDate(txtDDate.Text);
                DateTime vDeathDate = gblFuction.setDate(txtDeathDt.Text);
                vLoanId = ddlLoan.SelectedValue;
                if (Mode == "Save")
                {
                    vErr = oDD.SaveDeathDeclare(ref vNewId, vID, vDate, vBrCode, ddlCo.SelectedValue, ddlGrp.SelectedValue, ddlMembr.SelectedValue, vLoanId,
                            vDeathDate, ddlDPerson.SelectedValue, ddlLedger.SelectedValue, Convert.ToInt32(ddlWvReason.SelectedValue), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Saved Successfully");
                        ViewState["DDID"] = vNewId;
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
                oDD = null;
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
                    gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
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
            ddlCo.Enabled = Status;
            ddlGrp.Enabled = Status;
            ddlMembr.Enabled = Status;
            ddlLoan.Enabled = Status;
            txtDDate.Enabled = Status;
            ddlWvReason.Enabled = Status;
            txtDeathDt.Enabled = Status;
            ddlDPerson.Enabled = Status;
            ddlLedger.Enabled = false;
            ddlCenter.Enabled = Status;
        }

        private void ClearControls()
        {
            txtDDate.Text = Session[gblValue.LoginDate].ToString();
            ddlCo.SelectedIndex = -1;
            ddlGrp.SelectedIndex = -1; ;
            ddlMembr.SelectedIndex = -1;
            txtDeathDt.Text = "";
            ddlDPerson.SelectedIndex = -1;
            ddlLedger.SelectedIndex = -1;
            ddlWvReason.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlGrp.Items.Clear();
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();
            txtLoanDt.Text = "";
            txtLoanAmt.Text = "";
            txtLoanScheme.Text = "";
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(txtDDate.Text);
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

        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        private void PopGroup(string vCenterID)
        {            
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLogDt = gblFuction.setDate(txtDDate.Text);
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

        private void PopCenter(string vEOID)
        {
            ddlGrp.Items.Clear();
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
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

        private void PopLoan(string vMemberID, string pMode, string pLoanID)
        {
            ddlLoan.Items.Clear();
            DataTable dt = null;
            try
            {
                string vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vLogDt = gblFuction.setDate(txtDDate.Text);
                CDeathDocSnt oMem = new CDeathDocSnt();
                if (Convert.ToString(ddlMembr.SelectedValue) != "-1")
                {
                    dt = oMem.PopMemberForDD(ddlMembr.SelectedValue, vBrCode, vLogDt, pMode, pLoanID);
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

        private void GetLoanDtl()
        {
            DataTable dt = null, dt1 = null;
            CDisburse oDis = null;
            try
            {
                string vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vLogDt = gblFuction.setDate(txtDDate.Text);

                oDis = new CDisburse();
                if (Convert.ToString(ddlLoan.SelectedValue) != "-1")
                {
                    dt = oDis.GetInSuranceCompanyCollection(vBrCode, vLogDt, ddlLoan.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[0]["DescId"].ToString()));
                    }
                    dt1 = oDis.GetAllLoanByLoanId(ddlLoan.SelectedValue);
                    if (dt.Rows.Count > 0)
                    {
                        txtLoanDt.Text = dt1.Rows[0]["LoanDt"].ToString();
                        txtLoanAmt.Text = dt1.Rows[0]["LoanAmt"].ToString();
                        txtLoanScheme.Text = dt1.Rows[0]["LoanType"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDis = null;
                dt = null;
                dt1 = null;
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
                PopLoan(ddlMembr.SelectedValue, "A", "");
            }
        }

        protected void ddlLoan_SelectedIndexChanged(object sendre, EventArgs e)
        {
            if (ddlLoan.SelectedIndex > 0)
            {
                GetLoanDtl();
            }
        }

        private void PopMember(string vGroupID)
        {
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLoginDt = gblFuction.setDate(txtDDate.Text);
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

        private void PopWaveofReason()
        {
            Int32 vRows = 0;
            DataTable dt = null;
            CWaveOff oWv = new CWaveOff();
            dt = oWv.GetWavePG(1, ref vRows);
            ddlWvReason.DataTextField = "LoanWaveReason";
            ddlWvReason.DataValueField = "LoanWaveoffId";
            ddlWvReason.DataSource = dt;
            ddlWvReason.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlWvReason.Items.Insert(0, oItem);
        }

        private void PopLedger()
        {
            DataTable dt = null;
            string vBrCode;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            CVoucher oVou = new CVoucher();
            dt = oVou.GetAcGenLedCB(vBrCode, "S", "");
            ddlLedger.DataTextField = "Desc";
            ddlLedger.DataValueField = "DescID";
            ddlLedger.DataSource = dt;
            ddlLedger.DataBind();
            ListItem oItem = new ListItem();
            oItem.Text = "<--- Select --->";
            oItem.Value = "-1";
            ddlLedger.Items.Insert(0, oItem);
        }

        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
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
                    ViewState["DDID"] = vId;
                    dt = oLD.GetDeathDeclareById(vId, loginDt);
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
                        txtDDate.Text = Convert.ToString(dt.Rows[0]["DDDate"]);
                        popRO();
                        ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(ddlCo.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        PopGroup(ddlCenter.SelectedValue);
                        ddlGrp.SelectedIndex = ddlGrp.Items.IndexOf(ddlGrp.Items.FindByValue(dt.Rows[0]["Groupid"].ToString()));
                        PopMember(ddlCo.SelectedValue);
                        ddlMembr.SelectedIndex = ddlMembr.Items.IndexOf(ddlMembr.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        PopLoan(ddlMembr.SelectedValue, "E", dt.Rows[0]["LoanID"].ToString());
                        ddlLoan.SelectedIndex = ddlLoan.Items.IndexOf(ddlLoan.Items.FindByValue(dt.Rows[0]["LoanId"].ToString()));
                        txtLoanDt.Text = Convert.ToString(dt.Rows[0]["LoanDt"]);
                        txtLoanAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                        txtLoanScheme.Text = Convert.ToString(dt.Rows[0]["LoanType"]);
                        txtDeathDt.Text = Convert.ToString(dt.Rows[0]["DeathDate"]);
                        ddlDPerson.SelectedIndex = ddlDPerson.Items.IndexOf(ddlDPerson.Items.FindByValue(dt.Rows[0]["DeathType"].ToString()));
                        ddlWvReason.SelectedIndex = ddlWvReason.Items.IndexOf(ddlWvReason.Items.FindByValue(dt.Rows[0]["DeathReasonID"].ToString()));
                       // ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[0]["DescID"].ToString()));
                    }
                    tabLoanDisb.ActiveTabIndex = 1;
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