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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HospiCashClaim : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected int vFlag = 0;
        string ImagePath = "";
        string HospiDocBucket = ConfigurationManager.AppSettings["HospiDocBucket"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                txtDDate.Text = (string)Session[gblValue.LoginDate];
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
                this.PageHeading = "HospiCash Claim";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHospiCashClaim);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HospiCash Claim", false);
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
            //DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CDeathDocSnt();
            dt = oLD.GetHospiCashClaimList(vToDt, vBrCode, txtSearch.Text.Trim(), chkSendBack.Checked == true ? "Y" : "N");
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
                gblFuction.MsgPopup("LO Cannot be Left Blank ..");
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
            //if (fuHospiCash.HasFile == false)
            //{
            //    gblFuction.MsgPopup("Please attach HospiCash Form..");
            //    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_fuHospiCash");
            //    return vRes = false;
            //}

            return vRes;
        }

        private Boolean SaveRecords(string Mode)  //Check Account
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vLoanId = "";
            if (ValidaeField() == false)
                return false;
            CDeathDocSnt oDD = null;
            try
            {
                oDD = new CDeathDocSnt();
                DateTime vDate = gblFuction.setDate(txtDDate.Text);
                vLoanId = ddlLoan.SelectedValue;
                if (Mode == "Save")
                {
                    vErr = oDD.SaveHospiCashClaim(vLoanId, 0, Convert.ToInt32(Session[gblValue.UserId].ToString()), vDate, ddlClaimType.SelectedValue, "Save",
                        gblFuction.setDate(txtInsuValidTill.Text), ddlHospiAdminMemType.SelectedValue, gblFuction.setDate(txtHospiAdminDt.Text),
                        gblFuction.setDate(txtHospiDischDt.Text), Convert.ToInt32(txtTotalHospiDays.Text), txtIfsc.Text, txtBank.Text, txtBenfName.Text,
                        txtBenfAccNo.Text, txtInsuProdScheme.Text, ddlInsuProvider.SelectedValue, "N");
                    if (vErr == 0)
                    {

                        gblFuction.MsgPopup("Saved Successfully");
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.AjxMsgPopup("Same Day HospiCash Claim Already Saved.");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    string vDocUploadYN = "N";
                    if (hdnSBYN.Value != "Y")
                    {
                        if (fuHospiCash.HasFile)
                        {
                            string ext = System.IO.Path.GetExtension(fuHospiCash.PostedFile.FileName);
                            if (ext == ".pdf")
                            {
                                string vStatus = "N";
                                vStatus = SaveImages(fuHospiCash, hdnHID.Value, "Hospi_Cash_Claim");
                                vDocUploadYN = vStatus == "Y" ? "Y" : "N";
                            }
                            else
                            {
                                gblFuction.MsgPopup("Please Select pdf File.");
                                return false;
                            }
                        }
                        else
                        {
                            gblFuction.MsgPopup("Please Select pdf File.");
                            return false;
                        }
                    }

                    vErr = oDD.SaveHospiCashClaim(vLoanId, Convert.ToInt32(hdnHID.Value), Convert.ToInt32(Session[gblValue.UserId].ToString()), vDate, ddlClaimType.SelectedValue, "Edit",
                        gblFuction.setDate(txtInsuValidTill.Text), ddlHospiAdminMemType.SelectedValue, gblFuction.setDate(txtHospiAdminDt.Text),
                        gblFuction.setDate(txtHospiDischDt.Text), Convert.ToInt32(txtTotalHospiDays.Text), txtIfsc.Text, txtBank.Text, txtBenfName.Text,
                        txtBenfAccNo.Text, txtInsuProdScheme.Text, ddlInsuProvider.SelectedValue, vDocUploadYN);
                    if (vErr == 0)
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
                txtDDate.Enabled = false;
                ddlCo.Enabled = false;
                ddlCenter.Enabled = false;
                ddlGrp.Enabled = false;
                ddlMembr.Enabled = false;
                ddlLoan.Enabled = false;
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
                    btnSubmit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSubmit.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSubmit.Enabled = false;
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
                    btnSubmit.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSubmit.Enabled = false;
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
            ddlCenter.Enabled = Status;
            fuHospiCash.Enabled = Status;
            ddlClaimType.Enabled = Status;
            txtInsuValidTill.Enabled = false;
            //txtTotalHospiDays.Enabled = false;
            txtInsuProdScheme.Enabled = false;
            //txtBank.Enabled = false;
            ddlHospiAdminMemType.Enabled = Status;
            txtHospiAdminDt.Enabled = Status;
            txtHospiDischDt.Enabled = Status;
            txtIfsc.Enabled = Status;
            txtBenfName.Enabled = Status;
            txtBenfAccNo.Enabled = Status;
            ddlInsuProvider.Enabled = false;
        }

        private void ClearControls()
        {
            txtDDate.Text = Session[gblValue.LoginDate].ToString();
            ddlCo.SelectedIndex = -1;
            ddlGrp.SelectedIndex = -1; ;
            ddlMembr.SelectedIndex = -1;
            ddlHospiAdminMemType.SelectedIndex = -1;
            ddlClaimType.SelectedIndex = -1;
            ddlInsuProvider.SelectedIndex = -1;
            ddlCenter.Items.Clear();
            ddlGrp.Items.Clear();
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();
            txtHospiAdminDt.Text = "";
            txtHospiDischDt.Text = "";
            txtIfsc.Text = "";
            txtBenfName.Text = "";
            txtBenfAccNo.Text = "";
            txtInsuProdScheme.Text = "";
            txtInsuValidTill.Text = "";
            txtBank.Text = "";
            hdnHID.Value = "0";
            hdnSBYN.Value = "N";
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
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
                    dt = oMem.PopMemberForHospiCash(ddlMembr.SelectedValue, vBrCode, vLogDt, pMode, pLoanID);
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
                PopLoan(ddlMembr.SelectedValue, "A", "");
            }
        }

        protected void ddlLoan_SelectedIndexChanged(object sendre, EventArgs e)
        {
            CDeathDocSnt oMem = new CDeathDocSnt();
            DataTable dt = null;

            dt = oMem.PopInsuProdSchemeByLoanId(ddlLoan.SelectedValue);

            if (dt.Rows.Count > 0)
            {
                txtInsuProdScheme.Text = Convert.ToString(dt.Rows[0]["HospiName"]);
                txtInsuValidTill.Text = Convert.ToString(dt.Rows[0]["InsuValidTill"]);
                hdnDisbDt.Value = Convert.ToString(dt.Rows[0]["DisbDate"]);
                ddlInsuProvider.SelectedIndex = ddlInsuProvider.Items.IndexOf(ddlInsuProvider.Items.FindByValue(dt.Rows[0]["InsuProv"].ToString()));
            }
        }

        private void PopMember(string vGroupID)
        {
            ddlMembr.Items.Clear();
            ddlLoan.Items.Clear();

            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
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
            CDeathDocSnt oLD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString(); //gblFuction.setDate(txtFrmDt.Text);
            DateTime loginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vId = "";
            try
            {
                oLD = new CDeathDocSnt();
                vId = Convert.ToString(e.CommandArgument);
                ViewState["DDID"] = vId;
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                string vHid = (string)gvRow.Cells[8].Text;

                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                foreach (GridViewRow gr in gvLoanAppl.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }

                if (e.CommandName == "cmdShow")
                {
                    oLD = new CDeathDocSnt();
                    vId = Convert.ToString(e.CommandArgument);
                    ViewState["DDID"] = vId;
                    dt = oLD.GetHospiCashClaimDtlById(vId, Convert.ToInt32(vHid));
                    if (dt.Rows.Count > 0)
                    {
                        hdnHID.Value = vHid;
                        hdnSBYN.Value = Convert.ToString(dt.Rows[0]["SendBackYN"]);
                        btnShow.ForeColor = System.Drawing.Color.Red;
                        //Load Field
                        txtDDate.Text = Convert.ToString(dt.Rows[0]["HospiCashSendDate"]);
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
                        ddlClaimType.SelectedIndex = ddlClaimType.Items.IndexOf(ddlClaimType.Items.FindByValue(dt.Rows[0]["ClaimType"].ToString()));
                        txtInsuValidTill.Text = Convert.ToString(dt.Rows[0]["InsuValidTill"]);
                        ddlHospiAdminMemType.SelectedIndex = ddlHospiAdminMemType.Items.IndexOf(ddlHospiAdminMemType.Items.FindByValue(dt.Rows[0]["HospiAdminMemType"].ToString()));
                        txtHospiAdminDt.Text = Convert.ToString(dt.Rows[0]["HospiAdminDt"]);
                        txtHospiDischDt.Text = Convert.ToString(dt.Rows[0]["HospiDischDt"]);
                        txtTotalHospiDays.Text = Convert.ToString(dt.Rows[0]["TotalHospiDays"]);
                        txtIfsc.Text = Convert.ToString(dt.Rows[0]["IFSC"]);
                        txtBank.Text = Convert.ToString(dt.Rows[0]["Bank"]);
                        txtBenfName.Text = Convert.ToString(dt.Rows[0]["BenfName"]);
                        txtBenfAccNo.Text = Convert.ToString(dt.Rows[0]["BenfAccNo"]);
                        txtInsuProdScheme.Text = Convert.ToString(dt.Rows[0]["InsuProdScheme"]);
                        hdnDisbDt.Value = Convert.ToString(dt.Rows[0]["DisbDate"]);
                        ddlInsuProvider.SelectedIndex = ddlInsuProvider.Items.IndexOf(ddlInsuProvider.Items.FindByValue(dt.Rows[0]["InsuProvider"].ToString()));
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

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            string vDate = Session[gblValue.LoginDate].ToString();
            string vRptPath = "", vHospiCompany = "G0479", vReportName = "";
            DataTable dt = null;
            CDeathDocSnt oDD = null;
            CReports oRpt = null;
            try
            {
                oDD = new CDeathDocSnt();
                oRpt = new CReports();
                dt = oRpt.GetMediclaimRptByLoanId(ddlLoan.SelectedValue, ddlInsuProvider.SelectedValue, Convert.ToInt32(hdnHID.Value));
                if (dt.Rows.Count > 0)
                {
                }
                vHospiCompany = oDD.GetHospiCashCompany(ddlLoan.SelectedValue);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    //if (vHospiCompany == "G0297")
                    //{
                    //    if (ddlClaimType.SelectedValue == "A")
                    //    {
                    //        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\TataHospiCashAccident.rpt";
                    //        vReportName = ddlLoan.SelectedValue + "_TataHospiCash_Accident";
                    //    }
                    //    else
                    //    {
                    //        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\TataHospiCashSickness.rpt";
                    //        vReportName = ddlLoan.SelectedValue + "_TataHospiCash_Sickness";
                    //    }
                    //}
                    //else if (vHospiCompany == "G0479")
                    //{
                    //    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\KotakHospiCash.rpt";
                    //    vReportName = ddlLoan.SelectedValue + "_KotakHospiCash";
                    //}

                    if (ddlInsuProvider.SelectedValue == "A")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\AiqaClaimForm.rpt";
                        vReportName = ddlLoan.SelectedValue + "_AiqaHospiCash";
                    }
                    else if (ddlInsuProvider.SelectedValue == "T")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\TataHospiCashClaim.rpt";
                        vReportName = ddlLoan.SelectedValue + "_TataHospiCash";
                    }
                    else if (ddlInsuProvider.SelectedValue == "C")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CholaHospiCashClaim.rpt";
                        vReportName = ddlLoan.SelectedValue + "_CholaHospiCash";
                    }
                    else if (ddlInsuProvider.SelectedValue == "M")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CignaHospiCash.rpt";
                        vReportName = ddlLoan.SelectedValue + "_CignaHospiCash";
                    }

                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vReportName);
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
                //}                
            }
            finally
            {
                dt = null;
                oDD = null;
                oRpt = null;
            }
        }

        private string SaveImages(FileUpload flup, string HID, string imageName)
        {
            string isImageSaved = "N";
            try
            {
                string extension = System.IO.Path.GetExtension(flup.FileName);
                if (MinioYN == "N")
                {
                    ImagePath = ConfigurationManager.AppSettings["pathHospiMember"];
                    string folderPath = string.Format("{0}/{1}", ImagePath, ddlLoan.SelectedValue + "_" + HID);
                    System.IO.Directory.CreateDirectory(folderPath);
                    flup.SaveAs(folderPath + "_" + imageName + extension);
                    isImageSaved = "Y";
                }
                else
                {
                    string vImgName = extension.ToLower() == ".pdf" ? ddlLoan.SelectedValue + "_" + HID + "_" + imageName + extension : imageName + extension;
                    CApiCalling oAC = new CApiCalling();
                    byte[] vFileByte = ConvertFileToByteArray(flup.PostedFile);
                    isImageSaved = oAC.UploadFileMinio(vFileByte, vImgName, ddlLoan.SelectedValue, HospiDocBucket, MinioUrl);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isImageSaved;
        }

        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}