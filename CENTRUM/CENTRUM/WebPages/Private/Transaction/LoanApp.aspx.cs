using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LoanApp : CENTRUMBase
    {
        private int cPgNo = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["CGTID"] = null;
                txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.Enabled = true;
                }
                LoadGrid(1);
                StatusButton("View");
                tabLoanAppl.ActiveTabIndex = 0;
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
                this.PageHeading = "Loan Application";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanApplication);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
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
            txtAppDt.Enabled = Status;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
                ddlBranch.Enabled = Status;

            txtRo.Enabled = Status;
            txtMarket.Enabled = Status;
            txtGroup.Enabled = Status;
            txtMember.Enabled = Status;
            txtLnSchem.Enabled = Status;
            txtPurpose.Enabled = Status;
            txtSubPurp.Enabled = Status;
            txtProcFee.Enabled = Status;
            txtLnAmt.Enabled = Status;
            txtSrvTax.Enabled = Status;
            txtDisbDt.Enabled = Status;
            chkIsTopUp.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtAppNo.Text = "";
            txtAppDt.Text = Session[gblValue.LoginDate].ToString();

            txtRo.Text = "";
            hdEoId.Value = "-1";
            txtMarket.Text = "";
            hdMarketId.Value = "-1";
            txtGroup.Text = "";
            hdGrpId.Value = "-1";
            txtMember.Text = "";
            hdMemberId.Value = "-1";
            txtLnSchem.Text = "";
            hdLnSchID.Value = "-1";
            txtPurpose.Text = "";
            hdPurposeId.Value = "-1";
            txtSubPurp.Text = "";
            hdSubPurp.Value = "-1";

            txtProcFee.Text = "0";
            txtLnAmt.Text = "0";
            txtSrvTax.Text = "0";
            txtLnCycle.Text = "1";
            DateTime vDisbDt = gblFuction.setDate(txtAppDt.Text).AddDays(10);
            txtDisbDt.Text = gblFuction.putStrDate(vDisbDt);
            chkIsTopUp.Checked = false;
            LblDate.Text = "";
            LblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oCG = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oCG = new CApplication();
                dt = oCG.GetApplicationList(vMode, vBrCode, vFrmDt, vToDt, txtSearch.Text.Trim(), pPgIndx, ref vRows);
                gvLoanAppl.DataSource = dt.DefaultView;
                gvLoanAppl.DataBind();
                lbTotPg.Text = CalTotPgs(vRows).ToString();
                lbCrPg.Text = cPgNo.ToString();
                if (cPgNo == 0)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lbTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lbTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
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
        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAppId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vAppId = Convert.ToString(e.CommandArgument);
                ViewState["AppId"] = vAppId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetApplicationDtl_NEW(vAppId);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["IsTopup"].ToString().Trim() == "Y")
                            chkIsTopUp.Checked = true;
                        else
                            chkIsTopUp.Checked = false;
                        txtAppNo.Text = dt.Rows[0]["LoanAppNo"].ToString();
                        txtAppDt.Text = dt.Rows[0]["AppDate"].ToString();

                        txtRo.Text = dt.Rows[0]["EoName"].ToString();
                        hdEoId.Value = dt.Rows[0]["EOID"].ToString();
                        txtMarket.Text = dt.Rows[0]["Market"].ToString();
                        hdMarketId.Value = dt.Rows[0]["MarketID"].ToString();
                        txtGroup.Text = dt.Rows[0]["GroupName"].ToString();
                        hdGrpId.Value = dt.Rows[0]["GroupID"].ToString();
                        txtMember.Text = dt.Rows[0]["MemberCode"].ToString();
                        hdMemberId.Value = dt.Rows[0]["MemberID"].ToString();
                        txtLnSchem.Text = dt.Rows[0]["LoanType"].ToString();
                        hdLnSchID.Value = dt.Rows[0]["LoanTypeId"].ToString();
                        txtPurpose.Text = dt.Rows[0]["Purpose"].ToString();
                        hdPurposeId.Value = dt.Rows[0]["PurposeID"].ToString();
                        txtSubPurp.Text = dt.Rows[0]["SubPurpose"].ToString();
                        hdSubPurp.Value = dt.Rows[0]["SubPurposeId"].ToString();

                        txtLnAmt.Text = dt.Rows[0]["LoanAppAmt"].ToString();
                        txtLnCycle.Text = dt.Rows[0]["LoanCycle"].ToString();
                        txtDisbDt.Text = dt.Rows[0]["ExpDate"].ToString();
                        txtProcFee.Text = dt.Rows[0]["LnApplnPFees"].ToString();
                        txtSrvTax.Text = dt.Rows[0]["LnApplnSTax"].ToString();
                        LblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        LblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabLoanAppl.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
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
                tabLoanAppl.ActiveTabIndex = 1;
                StatusButton("Add");
                txtAppDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
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
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanAppl.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
                    cPgNo = Int32.Parse(lbTotPg.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lbCrPg.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanAppl.ActiveTabIndex = 0;
        }
        private bool SetUploads(FileUpload vFu, ref byte[] vFile, ref string vType)
        {
            if (vFu.HasFile == false)
            {
                //gblFuction.MsgPopup("Please Attach a File");
                gblFuction.focus(vFu.UniqueID.Replace("$", "_"));
                vFile = null;
                vType = "";
                return false;
            }
            if (vFu.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return false;
            }
            if (vFu.HasFile)
            {
                vFile = new byte[vFu.PostedFile.InputStream.Length + 1];
                vFu.PostedFile.InputStream.Read(vFile, 0, vFile.Length);
                vType = System.IO.Path.GetExtension(vFu.FileName).ToLower();
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            this.GetModuleByRole(mnuID.mnuLoanApplication);
            Boolean vResult = false;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            Int32 vErr = 0, vPurpId = 0, vSubPurpId = 0, vLnTypeId = 0, vCycle = 0, vCGTID = 0, vYrNo = 0, vErr1 = 0;
            string vAppId = "", vMemId = "", vTopup = "";
            double vLnAmt = 0, vProcFees = 0, vSrvTax = 0;
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            DateTime vExPectedDt = gblFuction.setDate(txtDisbDt.Text);
            CApplication oCG = null;
            byte[] vPsBk = null;
            byte[] vLstBnkCpy = null;
            byte[] vMIdProof = null;
            byte[] vAdd = null;
            byte[] vCoIDProof = null;
            byte[] vCoAdProof = null;
            string vPsBkType = "", vLstBnkCpyType = "", vMIdProofType = "", vAddType = "", vCoIDProofType = "", vCoAdProofType = "";
            SetUploads(PsBk, ref vPsBk, ref vPsBkType);
            SetUploads(LstBnkCpy, ref vLstBnkCpy, ref vLstBnkCpyType);
            SetUploads(MIdProof, ref vMIdProof, ref vMIdProofType);
            SetUploads(fuAddProof, ref vAdd, ref vAddType);
            SetUploads(CoIDProof, ref vCoIDProof, ref vCoIDProofType);
            SetUploads(CoAdProof, ref vCoAdProof, ref vCoAdProofType);
            try
            {
                if (ValidateFields() == false) return false;

                vCGTID = Convert.ToInt32(ViewState["CGTID"]);
                vMemId = hdMemberId.Value;// ddlMemNo.SelectedValue;
                vLnTypeId = Int32.Parse(hdLnSchID.Value);  //Convert.ToInt32(ddlLnSchem.SelectedValue);
                vPurpId = Int32.Parse(hdPurposeId.Value); // Convert.ToInt32(ddlLnPurps.SelectedValue);
                vSubPurpId = Int32.Parse(hdSubPurp.Value); // Convert.ToInt32(ddlSubPur.SelectedValue);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
                vCycle = Convert.ToInt32(txtLnCycle.Text);
                if (txtLnAmt.Text == "")
                {
                    gblFuction.MsgPopup("Application amount can not blank...");
                    return false;
                }
                vLnAmt = Convert.ToDouble(txtLnAmt.Text);

                if (txtProcFee.Text != "")
                    vProcFees = Convert.ToDouble(txtProcFee.Text);

                if (txtSrvTax.Text != "")
                    vSrvTax = Convert.ToDouble(txtSrvTax.Text);

                if (Session[gblValue.BrnchCode].ToString() == "0000")
                    vBrCode = ddlBranch.SelectedValue;
                else
                    vBrCode = Session[gblValue.BrnchCode].ToString();

                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToString(ViewState["AppId"]);

                if (chkIsTopUp.Checked == true)
                    vTopup = "Y";
                else
                    vTopup = "N";

                if (Mode == "Save")
                {
                    if (this.RoleId != 1)  // && this.RoleId != 4     1 for Admin   4 for BM discussed with Prodip as on 2nd Sep/2014
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtAppDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }

                    oCG = new CApplication();
                    vErr = oCG.ChkSaveApplication(vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("An Application pending for disbursement.");
                        return false;
                    }
                    vErr = oCG.ChkAppMember(vMemId, vAppDt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Invalid Age for Spouse.OR Borrower..Can not save");
                        return false;
                    }
                    if (vTopup == "N")
                    {
                        vErr = oCG.ChkMemberOpenLoan(vMemId, vAppDt, "");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup("Loan Exists. Can not save");
                            return false;
                        }
                    }
                    vErr1 = oCG.chkLoanAppNo(vBrCode, vAppDt);
                    if (vErr1 > 0)
                    {
                        gblFuction.MsgPopup("Invalid no of Loan Application");
                        return false;
                    }
                    //vErr = oCG.InsertApplication(ref vAppNo, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                    //            vExPectedDt, vCycle, vProcFees, vSrvTax, vCGTID, vBrCode, this.UserID, "I", vYrNo, vTopup,
                    //             vPsBk, vPsBkType, vLstBnkCpy, vLstBnkCpyType, vMIdProof, vMIdProofType, vAdd, vAddType, vCoIDProof, vCoIDProofType,
                    //            vCoAdProof, vCoAdProofType);
                    vErr = 0;
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
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
                    if (ValidateFields() == false) return false;

                    oCG = new CApplication();
                    vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Approved or Cancelled Application cannot be Edited.");
                        return false;
                    }
                    //vErr = oCG.UpdateApplication(vAppId, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                    //            vExPectedDt, vCycle, vProcFees, vSrvTax, vBrCode, vTopup, this.UserID, "Edit",
                    //             vPsBk, vPsBkType, vLstBnkCpy, vLstBnkCpyType, vMIdProof, vMIdProofType, vAdd, vAddType, vCoIDProof, vCoIDProofType,
                    //            vCoAdProof, vCoAdProofType);//, "A");
                    vErr = 0;
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oCG = new CApplication();
                    vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Approved or Cancelled Application cannot be Deleted.");
                        return false;
                    }
                    else
                    {
                        //vErr = oCG.UpdateApplication(vAppId, vAppDt, vMemId, vLnTypeId, vLnAmt, vPurpId, vSubPurpId,
                        //        vExPectedDt, vCycle, vProcFees, vSrvTax, vBrCode, vTopup, this.UserID, "Delet",
                        //         vPsBk, vPsBkType, vLstBnkCpy, vLstBnkCpyType, vMIdProof, vMIdProofType, vAdd, vAddType, vCoIDProof, vCoIDProofType,
                        //        vCoAdProof, vCoAdProofType);//, "A");
                        vErr = 0;
                        if (vErr == 0)
                            vResult = true;
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                }
                return vResult;
            }
            finally
            {
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDate = gblFuction.setDate(txtAppDt.Text);
            DateTime vExpDisbDt = gblFuction.setDate(txtDisbDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vUserId = Session[gblValue.UserId].ToString();
            Int32 vUsrId = Convert.ToInt32(vUserId);
            string vLnCycle = txtLnCycle.Text;
            Int32 vLoanCycle = Convert.ToInt32(vLnCycle);
            CApplication oCG = null;
            CDisburse oLD = null;
            DataTable dt = null;
            string vMsg = "";
            try
            {

                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    gblFuction.MsgPopup("Loan Application Date should be Financial Year.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
                if (vAppDate < vLoginDt)
                {
                    gblFuction.MsgPopup("Loan Application Date should not be greater than login date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtAppDt");
                    vResult = false;
                }
                if (vExpDisbDt <= vAppDate)
                {
                    gblFuction.MsgPopup("Expected Disbursement Date should not be Lesser than Loan Application Date.");
                    gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_txtDisbDt");
                    vResult = false;
                }
                if (hdMemberId.Value != "-1" && hdGrpId.Value != "-1")
                {
                    oCG = new CApplication();
                    dt = oCG.GetCGTMember(hdGrpId.Value, hdMemberId.Value, vBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["CGTID"] = dt.Rows[0]["CGTID"].ToString();
                        if (gblFuction.setDate(txtAppDt.Text).Year - gblFuction.setDate(dt.Rows[0]["M_DOB"].ToString()).Year > 55)
                        {
                            gblFuction.MsgPopup("Member Age Exceed 55 Years..Can not Apply for Loan");
                            return false;
                        }
                    }
                }

                if (hdMemberId.Value != "-1" && hdLnSchID.Value != "-1")
                {
                    oCG = new CApplication();
                    if (oCG.ChkMemBankInf(hdMemberId.Value, Convert.ToInt32(hdLnSchID.Value)) == 1)
                    {
                        gblFuction.MsgPopup("Member Bank Information required for this scheme");
                        vResult = false;
                    }
                }

                if (hdMemberId.Value != "-1")
                {
                    oLD = new CDisburse();
                    vMsg = oLD.chkMembersInfo("", hdMemberId.Value, "LA", vUsrId, vLoanCycle);
                    if (vMsg != "")
                    {
                        gblFuction.MsgPopup(vMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            finally
            {
                oCG = null;
                oLD = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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



#region OldCode



/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void txtAppDt_TextChanged(object sender, EventArgs e)
//{
//    //DateTime vDisbDt = gblFuction.setDate(txtAppDt.Text).AddDays(10);
//    //txtDisbDt.Text = gblFuction.putStrDate(vDisbDt);
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void ddlLnPurps_SelectedIndexChanged(object sender, EventArgs e)
//{
//    //if (ddlLnPurps.SelectedIndex > 0) PopSubPurpose(Convert.ToInt32(ddlLnPurps.SelectedValue));
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void chkIsTopUp_CheckedChanged(object sender, EventArgs e)
//{
//    DataTable dt = null;
//    CApplication oApp = null;
//    try
//    {
//        oApp = new CApplication();
//        if (chkIsTopUp.Checked == true)
//        {
//            dt = oApp.GetLoanTypeForApp("Y", Session[gblValue.BrnchCode].ToString());
//            //ddlLnSchem.DataSource = dt;
//            //ddlLnSchem.DataTextField = "LoanType";
//            //ddlLnSchem.DataValueField = "LoanTypeId";
//            //ddlLnSchem.DataBind();
//        }
//        else
//        {
//            dt = oApp.GetLoanTypeForApp("N", Session[gblValue.BrnchCode].ToString());
//            //ddlLnSchem.DataSource = dt;
//            //ddlLnSchem.DataTextField = "LoanType";
//            //ddlLnSchem.DataValueField = "LoanTypeId";
//            //ddlLnSchem.DataBind();
//        }
//        ListItem oLi = new ListItem("<--Select-->", "-1");
//        //ddlLnSchem.Items.Insert(0, oLi);
//    }
//    finally
//    {
//        dt = null;
//        oApp = null;
//    }
//}

/// <summary>
/// 
/// </summary>
/// <param name="vPurposeID"></param>
//private void PopSubPurpose(Int32 vPurposeID)
//{
//    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
//    DataTable dt = null;
//    CSubPurpose oGb = null;
//    string vBrCode = "";
//    try
//    {
//        vBrCode = (string)Session[gblValue.BrnchCode];
//        oGb = new CSubPurpose();
//        dt = oGb.PopSubPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])), vPurposeID);
//        //ddlSubPur.DataSource = dt;
//        //ddlSubPur.DataTextField = "SubPurpose";
//        //ddlSubPur.DataValueField = "SubPurposeID";
//        //ddlSubPur.DataBind();
//        //ListItem oli = new ListItem("<--Select-->", "-1");
//        //ddlSubPur.Items.Insert(0, oli);
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
//protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
//{
//    //if (ddlRO.SelectedIndex > 0) PopCenter(ddlRO.SelectedValue);
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
//{
//    //if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
//{
//    //ddlMemNo.SelectedIndex = -1;
//    //PopMember(ddlGroup.SelectedValue, "");
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void ddlMemNo_SelectedIndexChanged(object sender, EventArgs e)
//{            
//    DataTable dt = null;
//    CApplication oCG = null;
//    string vBrCode = (string)Session[gblValue.BrnchCode];
//    DateTime vLogDt = gblFuction.setDate("");
//    try
//    {
//        //if (ddlMemNo.SelectedIndex > 0 && ddlGroup.SelectedIndex > 0)
//        //{
//        //    oCG = new CApplication();
//        //    dt = oCG.GetCGTMember(ddlGroup.SelectedValue, ddlMemNo.SelectedValue, vBrCode);
//        //    if (dt.Rows.Count > 0)
//        //    {
//        //        ViewState["CGTID"] = dt.Rows[0]["CGTID"].ToString();
//        //        if (gblFuction.setDate(txtAppDt.Text).Year - gblFuction.setDate(dt.Rows[0]["M_DOB"].ToString()).Year > 55)
//        //        {
//        //            gblFuction.AjxMsgPopup("Member Age Exceed 55 Years..Can not Apply for Loan");
//        //            ddlMemNo.SelectedIndex = 0;
//        //        }
//        //    }
//        //}
//    }
//    finally
//    {
//        dt = null;
//        oCG = null;
//    }
//}

/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
//protected void ddlLnSchem_SelectedIndexChanged(object sender, EventArgs e)
//{
//    DataTable dt = null;
//    CApplication oCG = null;
//    Int32 vLoanTypeID = 0;
//    Int32 vApLoanCycle = 0;
//    //if (ddlMemNo.SelectedIndex <= 0)
//    //{
//    //    gblFuction.AjxMsgPopup("Select Member");
//    //    ddlLnSchem.SelectedIndex = -1;
//    //}
//    try
//    {
//        //PopPurpose();
//        //vLoanTypeID = Convert.ToInt32(ddlLnSchem.SelectedValue);
//        oCG = new CApplication();
//        //dt = oCG.GetLoanAmtAndCycleByLTypeID(vLoanTypeID, ddlMemNo.SelectedValue);
//        if (dt.Rows.Count > 0)
//        {
//            if (dt.Rows[0]["PrvLoanYN"].ToString() == "N")
//            {
//                gblFuction.AjxMsgPopup("The Member can not avail this Loan Scheme ");
//                //ddlLnSchem.SelectedIndex = -1;
//                return;
//            }
//            else
//            {
//                txtLnAmt.Text = dt.Rows[0]["LoanAmt"].ToString();
//                txtLnCycle.Text = dt.Rows[0]["LoanCycle"].ToString();
//                vApLoanCycle = Convert.ToInt32(dt.Rows[0]["ApLoanCycle"].ToString());
//                if (Convert.ToInt32(txtLnCycle.Text) < vApLoanCycle)
//                {
//                    gblFuction.AjxMsgPopup("Applied Amount is applicable for" + vApLoanCycle.ToString() + "and Above");
//                    //ddlLnSchem.SelectedIndex = -1;
//                }
//            }
//        }
//        else
//        {
//            txtLnAmt.Text = "";
//            txtLnCycle.Text = "";
//        }
//    }
//    finally
//    {
//        dt = null;
//        oCG = null;
//    }
//}



/// <summary>
/// 
/// </summary>
/// <param name="vEOID"></param>
//private void PopCenter(string vEOID)
//{
//    //ddlMemNo.Items.Clear();
//    //ddlGroup.Items.Clear();
//    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
//    DataTable dt = null;
//    CGblIdGenerator oGb = null;
//    string vBrCode = "";
//    Int32 vBrId = 0;
//    try
//    {
//        vBrCode = (string)Session[gblValue.BrnchCode];
//        vBrId = Convert.ToInt32(vBrCode);
//        oGb = new CGblIdGenerator();
//        dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
//        //ddlCenter.DataSource = dt;
//        //ddlCenter.DataTextField = "Market";
//        //ddlCenter.DataValueField = "MarketID";
//        //ddlCenter.DataBind();
//        ListItem oli = new ListItem("<--Select-->", "-1");
//        //ddlCenter.Items.Insert(0, oli);
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
/// <param name="vCenterID"></param>
//private void PopGroup(string vCenterID)
//{
//    //ddlGroup.Items.Clear();
//    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
//    DataTable dt = null;
//    CGblIdGenerator oGb = null;
//    string vBrCode = "";
//    Int32 vBrId = 0;
//    try
//    {
//        vBrCode = (string)Session[gblValue.BrnchCode];
//        vBrId = Convert.ToInt32(vBrCode);
//        oGb = new CGblIdGenerator();
//        dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
//        //ddlGroup.DataSource = dt;
//        //ddlGroup.DataTextField = "GroupName";
//        //ddlGroup.DataValueField = "GroupID";
//        //ddlGroup.DataBind();
//        //ListItem oli = new ListItem("<--Select-->", "-1");
//        //ddlGroup.Items.Insert(0, oli);
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
/// <param name="vGroupID"></param>
//private void PopMember(string vGroupID, string vMemId)
//{
//    DataTable dt = null;
//    CMember oMem = null;
//    CApplication oApp = null;
//    string vBrCode = "";

//    try
//    {
//        if (Session[gblValue.BrnchCode].ToString() == "0000")                
//            vBrCode = ddlBranch.SelectedValue;
//        else                
//            vBrCode = Session[gblValue.BrnchCode].ToString();                

//        if (chkIsTopUp.Checked == true)
//        {
//            oMem = new CMember();
//            dt = oMem.GetMemListByGroupId(vGroupID, vBrCode);
//            //ddlMemNo.DataSource = dt;
//            //ddlMemNo.DataTextField = "MemberCode";
//            //ddlMemNo.DataValueField = "MemberId";
//            //ddlMemNo.DataBind();
//        }
//        else
//        {
//            oApp = new CApplication();
//            dt = oApp.GetCGTMember(vGroupID, vMemId, vBrCode);
//            //ddlMemNo.DataTextField = "Member";
//            //ddlMemNo.DataValueField = "MemberId";
//            //ddlMemNo.DataSource = dt;
//            //ddlMemNo.DataBind();
//        }
//        //ListItem oItm = new ListItem("<--- Select --->", "-1");          
//        //ddlMemNo.Items.Insert(0, oItm);
//        //ddlMemNo.Focus();
//    }
//    finally
//    {
//        dt = null;
//        oMem = null;
//        oApp = null;
//    }
//}

/// <summary>
/// 
/// </summary>
//private void PopLoanType()
//{
//    DataTable dt = null;
//    CApplication oApp = null;
//    try
//    {
//        oApp = new CApplication();
//        dt = oApp.GetLoanTypeForApp("N", Session[gblValue.BrnchCode].ToString());
//        //ddlLnSchem.DataSource = dt;
//        //ddlLnSchem.DataTextField = "LoanType";
//        //ddlLnSchem.DataValueField = "LoanTypeId";
//        //ddlLnSchem.DataBind();
//        //ListItem oLi = new ListItem("<--Select-->", "-1");
//        //ddlLnSchem.Items.Insert(0, oLi);
//    }
//    finally
//    {
//        dt = null;
//        oApp = null;
//    }
//}

        /// <summary>
        /// 
        /// </summary>
//private void PopPurpose()
//{
//    DataTable dt = null;
//    CSubPurpose oGb = null;
//    try
//    {
//        //if (ddlLnSchem.SelectedValue == "9" || ddlLnSchem.SelectedValue == "10")
//        //{
//        //    oGb = new CSubPurpose();
//        //    dt = oGb.PopPurposeEdu(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
//        //    ddlLnPurps.DataSource = dt;
//        //    ddlLnPurps.DataTextField = "Purpose";
//        //    ddlLnPurps.DataValueField = "PurposeID";
//        //    ddlLnPurps.DataBind();
//        //    ListItem oli = new ListItem("<--Select-->", "-1");
//        //    ddlLnPurps.Items.Insert(0, oli);
//        //}
//        //else
//        //{
//        //    oGb = new CSubPurpose();
//        //    dt = oGb.PopPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
//        //    ddlLnPurps.DataSource = dt;
//        //    ddlLnPurps.DataTextField = "Purpose";
//        //    ddlLnPurps.DataValueField = "PurposeID";
//        //    ddlLnPurps.DataBind();
//        //    ListItem oli = new ListItem("<--Select-->", "-1");
//        //    ddlLnPurps.Items.Insert(0, oli);
//        //}
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
        //private void popRO()
        //{
        //    DataTable dt = null;
        //    CEO oRO = null;
        //    string vBrCode;
        //    if (Session[gblValue.BrnchCode].ToString() != "0000")          
        //        vBrCode = Session[gblValue.BrnchCode].ToString();            
        //    else            
        //        vBrCode = ddlBranch.SelectedValue.ToString();            

        //    DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
        //    try
        //    {
        //        oRO = new CEO();
        //        dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
        //        //ddlRO.DataSource = dt;
        //        //ddlRO.DataTextField = "EoName";
        //        //ddlRO.DataValueField = "EoId";
        //        //ddlRO.DataBind();
        //        //ListItem oli = new ListItem("<--Select-->", "-1");
        //        //ddlRO.Items.Insert(0, oli);
        //    }
        //    finally
        //    {
        //        oRO = null;
        //        dt = null;
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlBranch.SelectedIndex > 0)
        //        popRO();
        //}

//$("#<%=txtMember.ClientID %>").blur(function () {
 //           $.ajax({
 //               type: "POST",
 //               url: "../../../WebSrvcs/wsLnAppl.asmx/CheckMemberAge",
 //               data: "{pMemberId: '" + $("#<%=hdMemberId.ClientID %>").val() + "', pGroupId: '" + $("#<%=hdGrpId.ClientID %>").val() + "', pApplnDt: '" + $("#<%=txtAppDt.ClientID %>").val() + "'}",
 //               contentType: "application/json; charset=utf-8",
 //               dataType: "json",
 //               async: true,
 //               success: function (data) {
 //                   $("#<%=txtLnAmt.ClientID %>").val(data.LoanAmt);
 //               },
 //               failure: function (msg) {
 //                   alert("Ajax error...." + msg);
 //               }
 //           });
 //       });

 //$("#<%=txtLnSchem.ClientID %>").blur(function () {
 //           $.ajax({
 //               type: "POST",
 //               url: "../../../WebSrvcs/wsLnAppl.asmx/GetLoanAmtByLTypeID",
 //               data: "{pLoanTypeID: '" + $("#<%=hdLnSchID.ClientID %>").val() + "', pMemberId: '" + $("#<%=hdMemberId.ClientID %>").val() + "' }",
 //               contentType: "application/json; charset=utf-8",
 //               dataType: "json",
 //               async: true,
 //               success: function (response) {
 //                   if (response.PrvLoanYN == 'Y') {                       
 //                       $("#<%=txtLnAmt.ClientID %>").val(response.LoanAmt);
 //                       $("#<%=txtLnCycle.ClientID %>").val(response.LoanCycle);
 //                       var vPrvLoanYN = response.PrvLoanYN;
 //                       var vApLoanCycle = response.ApLoanCycle;
 //                   }
 //                   else {
 //                       alert('This Product ID is not valid. Try again!');
 //                   }
 //               },
 //               error: function (msg) {
 //                   alert("Ajax error ..." + msg);
 //               }
 //           });
 //       });

#endregion