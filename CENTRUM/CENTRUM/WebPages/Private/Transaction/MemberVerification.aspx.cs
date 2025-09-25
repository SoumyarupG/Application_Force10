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
    public partial class MemberVerification : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch(Session[gblValue.UserName].ToString());
                //popRO();
                PopLoanType();
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLoanDisb.ActiveTabIndex = 0;
            }
        }
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oLi);
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = (string)Session[gblValue.BrnchCode];
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CDisburse();
            dt = oLD.GetVerificationList(vFrmDt, vToDt, vBrCode, txtSearch.Text.Trim(), pPgIndx, ref vRows);
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
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopLoanType()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlLoanType.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "LoanTypeID", "LoanType", "LoanTypeMst", "0", "AA", "AA", System.DateTime.Now, "0000");
                ddlLoanType.DataSource = dt;
                ddlLoanType.DataTextField = "LoanType";
                ddlLoanType.DataValueField = "LoanTypeId";
                ddlLoanType.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoanType.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
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
                this.PageHeading = "Member Verification";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanDisbursement);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Member Verification", false);
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
                tabLoanDisb.ActiveTabIndex = 1;
                StatusButton("Add");
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                    //LoadGrid(1);
                    StatusButton("Delete");
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
        /// <returns></returns>
        private bool ValidaeField(string vMode)
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
            if (ddlCenter.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Center Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCenter");
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
            if (ddlAppNo.SelectedIndex <= 0)
            {
                gblFuction.MsgPopup("Application No Cannot be Left Blank ..");
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlAppNo");
                return vRes = false;
            }

            return vRes;
        }


        private Boolean SaveRecords(string Mode)  //Check Account
        {
            Boolean vResult = false;
            CVoucher oBJV = null;
            oBJV = new CVoucher();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vMVDate = gblFuction.setDate(txtMVDt.Text);
            //if (ValidaeField(Mode) == false)
            //    return false;

            DataTable dt = null;
            string vLoanAppID = "";
            Int32 vMVID=0, vErr = 0;

            CDisburse oLD = null;
            vLoanAppID = ddlAppNo.SelectedValue;

            oLD = new CDisburse();

            if (Mode == "Save")
            {

                vErr = oLD.SaveMemVerification(ref vMVID, vMVDate, vLoanAppID, ddlResponse1.SelectedValue, ddlResponse2.SelectedValue, ddlResponse3.SelectedValue,
                                            ddlResponse4.SelectedValue, ddlResponse5.SelectedValue, ddlResponse6.SelectedValue, ddlResponse7.SelectedValue, 
                                            ddlResponse8.SelectedValue, ddlResponse9.SelectedValue, ddlResponse10.SelectedValue, ddlResponse11.SelectedValue, 
                                            txtRem.Text,vBrCode, Convert.ToInt32(Session[gblValue.UserId].ToString()), "I");
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
                if (hdAppYN.Value != "")
                {
                    gblFuction.AjxMsgPopup("HO Approved Or Cancelled");
                    EnableControl(false);
                    return true;
                }
                vMVID= Convert.ToInt32(ViewState["MVID"]);
                vErr = oLD.SaveMemVerification(ref vMVID, vMVDate, vLoanAppID, ddlResponse1.SelectedValue, ddlResponse2.SelectedValue, ddlResponse3.SelectedValue,
                        ddlResponse4.SelectedValue, ddlResponse5.SelectedValue, ddlResponse6.SelectedValue, ddlResponse7.SelectedValue, ddlResponse8.SelectedValue,
                        ddlResponse9.SelectedValue, ddlResponse10.SelectedValue, ddlResponse11.SelectedValue, txtRem.Text, vBrCode, 
                        Convert.ToInt32(Session[gblValue.UserId].ToString()), "E");
                if (vErr == 0)
                {
                    gblFuction.AjxMsgPopup(gblMarg.EditMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            LoadGrid(0);
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
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                //if (hdAppYN.Value != "")
                //{
                //    gblFuction.AjxMsgPopup("HO Approved Or Cancelled");
                //    EnableControl(false);
                //    return;
                //}
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtMVDt.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlCo.Enabled = Status;
            ddlGrp.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMembr.Enabled = Status;
            ddlAppNo.Enabled = Status;
            ddlResponse1.Enabled = Status;
            ddlResponse2.Enabled = Status;
            ddlResponse3.Enabled = Status;
            ddlResponse4.Enabled = Status;
            ddlResponse5.Enabled = Status;
            ddlResponse6.Enabled = Status;
            ddlResponse7.Enabled = Status;
            ddlResponse8.Enabled = Status;
            ddlResponse9.Enabled = Status;
            ddlResponse10.Enabled = Status;
            ddlResponse11.Enabled = Status;
            txtRem.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtMVDt.Text = (string)Session[gblValue.LoginDate];
            ddlBranch.SelectedIndex = -1;
            ddlCo.SelectedIndex = -1;
            ddlAppNo.SelectedIndex = -1;
            ddlGrp.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            ddlMembr.SelectedIndex = -1;
            ddlLoanType.SelectedIndex = -1;
            txtLnAmt.Text = "0";
            txtIntRate.Text = "0";
            ddlResponse1.SelectedIndex = -1;
            ddlResponse2.SelectedIndex = -1;
            ddlResponse3.SelectedIndex = -1;
            ddlResponse4.SelectedIndex = -1;
            ddlResponse5.SelectedIndex = -1;
            ddlResponse6.SelectedIndex = -1;
            ddlResponse7.SelectedIndex = -1;
            ddlResponse8.SelectedIndex = -1;
            ddlResponse9.SelectedIndex = -1;
            ddlResponse10.SelectedIndex = -1;
            ddlResponse11.SelectedIndex = -1;
            txtRem.Text = "";
            hdAppYN.Value = "";
            ViewState["MVID"] = "";
        }
        /// <summary>
        /// 
        /// </summary>
        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = ddlBranch.SelectedValue;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ddlCo.Items.Clear();
                ddlCenter.Items.Clear();
                ddlCenter.SelectedIndex = -1;
                ddlGrp.Items.Clear();
                ddlGrp.SelectedIndex = -1;
                ddlMembr.Items.Clear();
                ddlMembr.SelectedIndex = -1;
                ddlAppNo.Items.Clear();
                ddlAppNo.SelectedIndex = -1;
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

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            popRO();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCo.SelectedIndex > 0) PopCenter(ddlCo.SelectedValue);
            ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(ddlCo.SelectedValue));
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCOID"></param>
        private void PopCenter(string vCOID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlCenter.Items.Clear();
                ddlCenter.SelectedIndex = -1;
                ddlGrp.Items.Clear();
                ddlGrp.SelectedIndex = -1;
                ddlMembr.Items.Clear();
                ddlMembr.SelectedIndex = -1;
                ddlAppNo.Items.Clear();
                ddlAppNo.SelectedIndex = -1;
                string vBrCode = ddlBranch.SelectedValue;
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "", "MarketID", "Market", "MarketMst", vCOID, "EOID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                dtGr.AcceptChanges();
                ddlCenter.DataSource = dtGr;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oLi);
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
        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vCenterID"></param>
        private void PopGroup(string vCenterID)
        {
            ddlGrp.Items.Clear();
            ddlGrp.SelectedIndex = -1;
            ddlMembr.Items.Clear();
            ddlMembr.SelectedIndex = -1;
            ddlAppNo.Items.Clear();
            ddlAppNo.SelectedIndex = -1;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                vBrCode = ddlBranch.SelectedValue;
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMembr.SelectedIndex = -1;
            PopMember(ddlGrp.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vGroupID"></param>
        private void PopMember(string vGroupID)
        {
            ddlMembr.Items.Clear();
            ddlMembr.SelectedIndex = -1;
            ddlAppNo.Items.Clear();
            ddlAppNo.SelectedIndex = -1;
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            try
            {
                CMember oMem = new CMember();
                if (Convert.ToString(ddlCo.SelectedValue) != "-1")
                {
                    dt = oMem.PopGrpMember(ddlGrp.SelectedValue, ddlBranch.SelectedValue, vLoginDt, "M");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMembr_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ddlAppNo.Items.Clear();
            ddlAppNo.SelectedIndex = -1;
            txtAdd.Text = "";
            txtAge.Text = "";
            txtMob.Text = "";
            txtBank.Text = "";
            txtNominee.Text = "";
            DataTable dtApp = null, dt = null;
            CDisburse oLD = null;
            string vMemberId = ddlMembr.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {

                oLD = new CDisburse();
                ds = oLD.GetVarifyLoanAppByMemberId(vMemberId, "N", gblFuction.setDate(txtMVDt.Text));
                dt = ds.Tables[0];
                dtApp = ds.Tables[1];

                if (dt.Rows.Count > 0)
                {
                    txtAdd.Text = Convert.ToString(dt.Rows[0]["MemAddr"]);
                    txtAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);
                    txtMob.Text = Convert.ToString(dt.Rows[0]["M_Mobile"]);
                    txtBank.Text = Convert.ToString(dt.Rows[0]["BankName"]);
                    txtNominee.Text = Convert.ToString(dt.Rows[0]["NomName"]);
                }

                if (dtApp.Rows.Count > 0)
                {
                    ddlAppNo.DataTextField = "LoanAppNo";
                    ddlAppNo.DataValueField = "LoanAppId";
                    ddlAppNo.DataSource = dtApp;
                    ddlAppNo.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlAppNo.Items.Insert(0, oItm);
                }
                else
                {
                    ddlAppNo.Items.Clear();
                    ddlAppNo.Items.Add("No Matching Records.");
                }

                //fillLnDtl(vMemberId, gblFuction.setDate(txtLnDt.Text));

            }
            finally
            {
                ds = null;
                dt = null;
                dtApp = null;
                oLD = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vMemberID"></param>
        /// <param name="vIsDisburse"></param>
        private void PopLoanNo(string vMemberID, string vIsDisburse)
        {
            DataSet ds = new DataSet();
            ddlAppNo.Items.Clear();
            DataTable dtApp = null, dt = null;
            CDisburse oLD = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            txtAdd.Text = "";
            txtAge.Text = "";
            txtMob.Text = "";
            txtBank.Text = "";
            txtNominee.Text = "";
            try
            {
                oLD = new CDisburse();
                ds = oLD.GetVarifyLoanAppByMemberId(vMemberID, vIsDisburse, gblFuction.setDate(txtMVDt.Text));
                dt = ds.Tables[0];
                dtApp = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    txtAdd.Text = Convert.ToString(dt.Rows[0]["MemAddr"]);
                    txtAge.Text = Convert.ToString(dt.Rows[0]["M_Age"]);
                    txtMob.Text = Convert.ToString(dt.Rows[0]["M_Mobile"]);
                    txtBank.Text = Convert.ToString(dt.Rows[0]["BankName"]);
                    txtNominee.Text = Convert.ToString(dt.Rows[0]["NomName"]);
                }

                if (dtApp.Rows.Count > 0)
                {
                    ddlAppNo.DataTextField = "LoanAppNo";
                    ddlAppNo.DataValueField = "LoanAppId";
                    ddlAppNo.DataSource = dtApp;
                    ddlAppNo.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlAppNo.Items.Insert(0, oItm);
                }
                else
                {
                    ddlAppNo.Items.Clear();
                    ddlAppNo.Items.Add("No Matching Records.");
                }

            }
            finally
            {
                ds = null;
                dt = null;
                dtApp = null;
                oLD = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlAppNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vBrCode = ddlBranch.SelectedValue;
            string vGrpId = ddlGrp.SelectedValue;
            string vLoanAppID = ddlAppNo.SelectedValue;
            DataTable dt = null;
            CDisburse oLD = null;
            try
            {
                oLD = new CDisburse();
                dt = oLD.GetLoanAppdtlGD(vLoanAppID, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
                    txtLnAmt.Text = Convert.ToString(dt.Rows[0]["ApprovedAmt"]);
                    txtIntRate.Text = Convert.ToString(dt.Rows[0]["InstRate"]);
                }
            }
            finally
            {
                dt = null;
                oLD = null;
            }
        }

        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vMVID = 0;
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    oLD = new CDisburse();
                    vMVID = Convert.ToInt32(e.CommandArgument);
                    dt = oLD.GetMemberVerificationByID(vMVID);
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
                        hdAppYN.Value = Convert.ToString(dt.Rows[0]["HOApproveYN"]);
                        txtMVDt.Text = Convert.ToString(dt.Rows[0]["MVDate"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        popRO();
                        ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));
                        PopCenter(ddlCo.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        PopGroup(ddlCenter.SelectedValue);
                        ddlGrp.SelectedIndex = ddlGrp.Items.IndexOf(ddlGrp.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
                        PopMember(dt.Rows[0]["GroupId"].ToString());
                        ddlMembr.SelectedIndex = ddlMembr.Items.IndexOf(ddlMembr.Items.FindByValue(dt.Rows[0]["MemberId"].ToString()));
                        PopLoanNo(ddlMembr.SelectedValue, "Y");
                        ddlAppNo.SelectedIndex = ddlAppNo.Items.IndexOf(ddlAppNo.Items.FindByValue(dt.Rows[0]["LoanAppId"].ToString()));
                        ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
                        txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAppAmt"]);
                        txtIntRate.Text = Convert.ToString(dt.Rows[0]["InstRate"]);
                        txtRem.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
                        ddlResponse1.SelectedIndex = ddlResponse1.Items.IndexOf(ddlResponse1.Items.FindByValue(dt.Rows[0]["Q1Response"].ToString()));
                        ddlResponse2.SelectedIndex = ddlResponse2.Items.IndexOf(ddlResponse2.Items.FindByValue(dt.Rows[0]["Q2Response"].ToString()));
                        ddlResponse3.SelectedIndex = ddlResponse3.Items.IndexOf(ddlResponse3.Items.FindByValue(dt.Rows[0]["Q3Response"].ToString()));
                        ddlResponse4.SelectedIndex = ddlResponse4.Items.IndexOf(ddlResponse4.Items.FindByValue(dt.Rows[0]["Q4Response"].ToString()));
                        ddlResponse5.SelectedIndex = ddlResponse5.Items.IndexOf(ddlResponse5.Items.FindByValue(dt.Rows[0]["Q5Response"].ToString()));
                        ddlResponse6.SelectedIndex = ddlResponse6.Items.IndexOf(ddlResponse6.Items.FindByValue(dt.Rows[0]["Q6Response"].ToString()));
                        ddlResponse7.SelectedIndex = ddlResponse7.Items.IndexOf(ddlResponse7.Items.FindByValue(dt.Rows[0]["Q7Response"].ToString()));
                        ddlResponse8.SelectedIndex = ddlResponse8.Items.IndexOf(ddlResponse8.Items.FindByValue(dt.Rows[0]["Q8Response"].ToString()));
                        ddlResponse9.SelectedIndex = ddlResponse9.Items.IndexOf(ddlResponse9.Items.FindByValue(dt.Rows[0]["Q9Response"].ToString()));
                        ddlResponse10.SelectedIndex = ddlResponse10.Items.IndexOf(ddlResponse10.Items.FindByValue(dt.Rows[0]["Q10Response"].ToString()));
                        ddlResponse11.SelectedIndex = ddlResponse11.Items.IndexOf(ddlResponse11.Items.FindByValue(dt.Rows[0]["Q11Response"].ToString()));
                        //fillLnDtl(dt.Rows[0]["MemberId"].ToString(), vAppDt);

                        LblUser.Text = "Last Modified By : " + Convert.ToString(dt.Rows[0]["UserName"]);
                        LblDate.Text = "Last Modified Date : " + Convert.ToString(dt.Rows[0]["CreationDateTime"]);

                        ViewState["MVID"] = Convert.ToString(dt.Rows[0]["MVID"]);
                        //hdPSNName.Value = Convert.ToString(dt.Rows[0]["SName"]);
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