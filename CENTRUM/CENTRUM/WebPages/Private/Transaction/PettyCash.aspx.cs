using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using FORCEBA;
using FORCECA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using System.Net;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PattyCash : CENTRUMBase
    {
        protected int cPgNo = 1;
        string PettyCashBucket = ConfigurationManager.AppSettings["PettyCashBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        protected string path = ConfigurationManager.AppSettings["PettyCashDocPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["ClickMode"] = "Load";
                ViewState["StateEdit"] = null;
                ViewState["Schedule"] = null;
                ViewState["SubmitYN"] = null;
                ViewState["PattyCashId"] = null;
                Session["PCDoc"] = null;
                txtFromDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                tabPattyCash.ActiveTabIndex = 0;
                //PopCompany();
                PopExpenseHead();
                PopBranch(Session[gblValue.UserName].ToString());
                ddlBranch.Enabled = true;
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                hdnBrnch.Value = Session[gblValue.BrnchCode].ToString();
                ddlBranch.Enabled = false;
                //}
                ddlBrch.Enabled = false;
                PettyCashLoadgrid(0);
                StatusButton("View");
                //fuFileUpload.Attributes["onchange"] = "UploadFileCheck(this)";
            }
            if (fuFileUpload.HasFile)
            {
                Session["PCDoc"] = fuFileUpload;
                lblFileName.Text = fuFileUpload.FileName;
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
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);

                    ddlBrch.DataSource = dt;
                    ddlBrch.DataTextField = "BranchName";
                    ddlBrch.DataValueField = "BranchCode";
                    ddlBrch.DataBind();
                    ddlBrch.SelectedIndex = ddlBrch.Items.IndexOf(ddlBrch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));

                }
            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Petty Cash";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPettyCash);
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
                    btnAdd.Visible = true;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                //else
                //    if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete == "Y")
                //    {
                //        // btnAdd.Visible = false;
                //        btnEdit.Visible = false;
                //        btnDelete.Visible = true;
                //        btnCancel.Visible = false;
                //        btnSave.Visible = false;
                //    }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Petty Cash", false);
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
                    btnPrint.Enabled = false;
                    btnExcelPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSubmit.Enabled = false;
                    fuFileUpload.Enabled = true;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnPrint.Enabled = true;
                    btnExcelPrint.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSubmit.Enabled = true;
                    fuFileUpload.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExcelPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnSubmit.Enabled = false;
                    fuFileUpload.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExcelPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSubmit.Enabled = false;
                    fuFileUpload.Enabled = false;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExcelPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSubmit.Enabled = false;
                    fuFileUpload.Enabled = false;
                    EnableControl(false);
                    break;
            }

            //if (Session[gblValue.BrnchCode].ToString() == "0000")
            //{
            //    btnAdd.Enabled = false;
            //    btnSave.Enabled = false;
            //    btnEdit.Enabled = false;
            //    btnDelete.Enabled = false;
            //    btnCancel.Enabled = false;
            //    btnSubmit.Enabled = false;
            //}
        }

        private void ClearControls()
        {
            txtVoucherNo.Text = "";
            txtPaidTo.Text = "";
            ddlDrCr.SelectedIndex = -1;
            ddlType.SelectedIndex = -1;
            txtNarration.Text = "";
            txtCrTot.Text = "0";
            txtDrTot.Text = "0";
            ViewState["VouDtl"] = null;
            txtReffId.Text = "";
            gvVoucherDtl.DataSource = null;
            gvVoucherDtl.DataBind();
            ddlExpenseHead.SelectedIndex = -1;
            fuFileUpload.Dispose();
            Session["PCDoc"] = null;
            lblFileName.Text = "";
            txtPettyCashBalance.Text = "0";
            TxtSendBackRem.Text = "";
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (hdLed.Value == "-1" || hdLed.Value == "0" || hdLed.Value == "")
            {
                gblFuction.AjxMsgPopup("Please Select One Ledger...");
                return;
            }
            //if (hdCBank.Value == "-1" || hdCBank.Value == "0")
            //{
            //    gblFuction.AjxMsgPopup("Please Select One Ledger...");
            //    return;
            //}
            string EditMode = string.Empty;
            EditMode = ViewState["ClickMode"].ToString();
            //string vChkCashBank = hbCb.Value;
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            DataRow dr;
            dt = (DataTable)ViewState["VouDtl"];
            string vChkNum = "";
            double Num;
            bool isNum = false;
            vChkNum = txtAmount.Text.Trim();
            isNum = double.TryParse(vChkNum, out Num);

            if (isNum == false)
            {
                // EnableControl(true);
                gblFuction.AjxMsgPopup("Invalid Amount...");
                gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
                return;
            }

            if (ViewState["VouDtl"] != null && hdEdit.Value == "-1")
            {
                dr = dt.NewRow();
                dr["DescId"] = hdLed.Value;// ddlLedger.SelectedValue.ToString();
                dr["Desc"] = txtLed.Text;// ddlLedger.SelectedItem.Text;
                //dr["SubLedID"] = hdSubLed.Value;
                //dr["SubLed"] = txtSubLed.Text;
                //dr["PCV"] = txtPCVNo.Text;
                //dr["PCVDt"] = txtPCVDate.Text;
                //dr["PCVPayee"] = txtPCCVPayee.Text;
                //dr["PCVRemarks"] = txtPCVRemarks.Text;

                //if (hdLed.Value == "14010" || hdLed.Value == "21005" || hdLed.Value == "21006" || hdLed.Value == "21007" || hdLed.Value == "21008" ||
                //hdLed.Value == "21009" || hdLed.Value == "21010" || hdLed.Value == "21011" || hdLed.Value == "21012" || hdLed.Value == "21013" || hdLed.Value == "21015")
                //{
                //    dr["BranchCode"] = "0000";
                //    dr["Branch"] = "Head-Office";
                //}
                //else
                //{
                //    dr["BranchCode"] = ddlBrch.SelectedValue;
                //    dr["Branch"] = ddlBrch.SelectedItem.Text;
                //}

                dr["BranchCode"] = ddlBrch.SelectedValue;
                dr["Branch"] = ddlBrch.SelectedItem.Text;

                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["Debit"] = txtAmount.Text;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                    dr["Amt"] = txtAmount.Text;
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = txtAmount.Text;
                    dr["DC"] = "C";
                    dr["Amt"] = txtAmount.Text;
                }
                dr["ExpHeadId"] = Convert.ToString(ddlExpenseHead.SelectedValue);
                dr["ExpenseHead"] = Convert.ToString(ddlExpenseHead.SelectedItem);
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value == "-1")
            {
                dt = new DataTable();
                dt.Columns.Add("SlNo");
                dt.Columns.Add("DC");
                dt.Columns.Add("Debit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("Credit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("DescId");
                dt.Columns.Add("Desc");
                dt.Columns.Add("SubLedID");
                dt.Columns.Add("SubLed");
                dt.Columns.Add("DtlId");
                dt.Columns.Add("Amt");
                dt.Columns.Add("BranchCode");
                dt.Columns.Add("Branch");
                dt.Columns.Add("PCV"); dt.Columns.Add("PCVDt"); dt.Columns.Add("PCVPayee"); dt.Columns.Add("PCVRemarks");
                dt.Columns.Add("ExpHeadId");
                dt.Columns.Add("ExpenseHead");

                dr = dt.NewRow();
                dr["DescId"] = hdLed.Value;
                dr["Desc"] = txtLed.Text;
                //dr["SubLedID"] = hdSubLed.Value;
                //dr["SubLed"] = txtSubLed.Text;
                //dr["PCV"] = txtPCVNo.Text; dr["PCVDt"] = txtPCVDate.Text; dr["PCVPayee"] = txtPCCVPayee.Text; dr["PCVRemarks"] = txtPCVRemarks.Text;

                //if (hdLed.Value == "14010" || hdLed.Value == "21005" || hdLed.Value == "21006" || hdLed.Value == "21007" || hdLed.Value == "21008" ||
                //hdLed.Value == "21009" || hdLed.Value == "21010" || hdLed.Value == "21011" || hdLed.Value == "21012" || hdLed.Value == "21013" || hdLed.Value == "21015")
                //{
                //    dr["BranchCode"] = "0000";
                //    dr["Branch"] = "Head-Office";
                //}
                //else
                //{
                //    dr["BranchCode"] = ddlBrch.SelectedValue;
                //    dr["Branch"] = ddlBrch.SelectedItem.Text;
                //}

                dr["BranchCode"] = ddlBrch.SelectedValue;
                dr["Branch"] = ddlBrch.SelectedItem.Text;

                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["Debit"] = txtAmount.Text;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                    dr["Amt"] = txtAmount.Text;
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = txtAmount.Text;
                    dr["DC"] = "C";
                    dr["Amt"] = txtAmount.Text;
                }
                dr["ExpHeadId"] = Convert.ToString(ddlExpenseHead.SelectedValue);
                dr["ExpenseHead"] = Convert.ToString(ddlExpenseHead.SelectedItem);
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value != "-1" && EditMode == "G_L")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["DescId"] = hdLed.Value;
                dt.Rows[vR]["Desc"] = txtLed.Text;

                dt.Rows[vR]["ExpHeadId"] = Convert.ToString(ddlExpenseHead.SelectedValue);
                dt.Rows[vR]["ExpenseHead"] = Convert.ToString(ddlExpenseHead.SelectedItem);

                //dt.Rows[vR]["SubLedID"] = hdSubLed.Value;
                //dt.Rows[vR]["SubLed"] = txtSubLed.Text;
                //dt.Rows[vR]["PCV"] = txtPCVNo.Text;
                //dt.Rows[vR]["PCVDt"] = txtPCVDate.Text;
                //dt.Rows[vR]["PCVPayee"] = txtPCCVPayee.Text;
                //dt.Rows[vR]["PCVRemarks"] = txtPCVRemarks.Text;

                //if (hdLed.Value == "14010" || hdLed.Value == "21005" || hdLed.Value == "21006" || hdLed.Value == "21007" || hdLed.Value == "21008" ||
                //hdLed.Value == "21009" || hdLed.Value == "21010" || hdLed.Value == "21011" || hdLed.Value == "21012" || hdLed.Value == "21013" || hdLed.Value == "21015")
                //{
                //    dt.Rows[vR]["BranchCode"] = "0000";
                //    dt.Rows[vR]["Branch"] = "Head-Office";
                //}
                //else
                //{
                //    dt.Rows[vR]["BranchCode"] = ddlBrch.SelectedValue;
                //    dt.Rows[vR]["Branch"] = ddlBrch.SelectedItem.Text;
                //}

                dt.Rows[vR]["BranchCode"] = ddlBrch.SelectedValue;
                dt.Rows[vR]["Branch"] = ddlBrch.SelectedItem.Text;

                if (ddlDrCr.SelectedValue == "D")
                {
                    dt.Rows[vR]["Debit"] = txtAmount.Text;
                    dt.Rows[vR]["Credit"] = "0";
                    dt.Rows[vR]["DC"] = "D";
                    dt.Rows[vR]["Amt"] = txtAmount.Text;
                }
                else
                {
                    dt.Rows[vR]["Debit"] = "0";
                    dt.Rows[vR]["Credit"] = txtAmount.Text;
                    dt.Rows[vR]["DC"] = "C";
                    dt.Rows[vR]["Amt"] = txtAmount.Text;
                }
            }
            else if (hdEdit.Value != "-1" && EditMode == "C_B")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);

                if (ddlDrCr.SelectedValue == "D")
                {
                    dt.Rows[vR]["Debit"] = txtAmount.Text;
                    dt.Rows[vR]["Credit"] = "0";
                    dt.Rows[vR]["DC"] = "D";
                    dt.Rows[vR]["Amt"] = txtAmount.Text;
                }
                else
                {
                    dt.Rows[vR]["Debit"] = "0";
                    dt.Rows[vR]["Credit"] = txtAmount.Text;
                    dt.Rows[vR]["DC"] = "C";
                    dt.Rows[vR]["Amt"] = txtAmount.Text;
                }
            }
            dt.AcceptChanges();
            ViewState["VouDtl"] = dt;
            gvVoucherDtl.DataSource = dt;
            gvVoucherDtl.DataBind();

            foreach (DataRow Tdr in dt.Rows)
            {
                vTotDr += Convert.ToDouble(Tdr["Debit"]);
                vTotCr += Convert.ToDouble(Tdr["Credit"]);
            }
            txtDrTot.Text = vTotDr.ToString();
            txtCrTot.Text = vTotCr.ToString();
            txtAmount.Text = "";
            txtLed.Text = "";
            hdEdit.Value = "-1";
            hdLed.Value = "";
            if (ddlExpenseHead.SelectedIndex > 0)
            {
                if (txtNarration.Text.Trim() == "")
                    txtNarration.Text = "BEING THE AMOUNT " + Convert.ToString(ddlExpenseHead.SelectedItem).Trim();
                else
                    txtNarration.Text = txtNarration.Text.Trim() + ", " + Convert.ToString(ddlExpenseHead.SelectedItem).Trim();
            }
            //txtSubLed.Text = "";
            //hdSubLed.Value = "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtVoucherNo.Enabled = Status;
            txtDt.Enabled = Status;
            txtPaidTo.Enabled = Status;
            txtLed.Enabled = Status;
            txtAmount.Enabled = Status;
            //ddlDrCr.Enabled = Status;
            ddlDrCr.Enabled = false;
            txtNarration.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
            //ddlCompany.Enabled = Status;
            //txtSubLed.Enabled = Status;
            ddlType.Enabled = Status;
            ddlExpenseHead.Enabled = Status;
            txtPettyCashBalance.Enabled = Status;
            TxtSendBackRem.Enabled = false;
        }

        protected void btnShow_Click(object sender, System.EventArgs e)
        {
            PettyCashLoadgrid(0);
        }

        private void PettyCashLoadgrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            Int32 vPattyCashId = 0;
            CPettyCash cPc = null;
            string vBrCode = ddlBranch.SelectedValue;
            DateTime vDtFrom = gblFuction.setDate(txtFromDt.Text);
            DateTime vDtTo = gblFuction.setDate(txtToDt.Text);
            string vLoginBranch = Session[gblValue.BrnchCode].ToString();
            try
            {
                cPc = new CPettyCash();
                dt = cPc.PettyCashLoadgrid(vPattyCashId, pPgIndx, ref vTotRows, vBrCode, vDtFrom, vDtTo, ddlTypeSrch.SelectedValue, vLoginBranch);
                gvPetty.DataSource = dt;
                gvPetty.DataBind();
                lblTotPg.Text = CalTotPgs(vTotRows).ToString();
                lblCrPg.Text = cPgNo.ToString();
                if (cPgNo == 1)
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
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                cPc = null;
                dt = null;
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
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            PettyCashLoadgrid(cPgNo);
            tabPattyCash.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //if (Session[gblValue.BrnchCode].ToString() == "0000")
            //{
            //    if (SaveRecords("Submit") == true)
            //    {
            //        gblFuction.MsgPopup("Record Submitted for further transaction");
            //        EnableControl(false);
            //        StatusButton("View");
            //        PettyCashLoadgrid(0);
            //        tabPattyCash.ActiveTabIndex = 0;
            //    }
            //}
            //else
            //{
            //    gblFuction.MsgPopup("Branch Can not use This...");
            //    return;
            //}

            //if (Session[gblValue.BrnchCode].ToString() != "0000")
            //{
            if (Convert.ToString(ViewState["SubmitYN"]) == "Y")
            {
                gblFuction.MsgPopup("Already Submitted or replenished....Edit Not allowed");
                return;
            }
            if (SaveRecords("Submit") == true)
            {
                gblFuction.MsgPopup("Record Submitted for further transaction");
                EnableControl(false);
                StatusButton("View");
                PettyCashLoadgrid(0);
                tabPattyCash.ActiveTabIndex = 0;
            }
            //}
            //else
            //{
            //    gblFuction.MsgPopup("HO Can not use This...");
            //    return;
            //}
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vVoucherEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vVoucherEdit == "Add" || vVoucherEdit == null)
                vVoucherEdit = "Save";

            if (SaveRecords(vVoucherEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                EnableControl(false);
                StatusButton("Show");
                PettyCashLoadgrid(0);
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
                //if (Convert.ToString(ViewState["SubmitYN"]) == "Y" && (ddlBranch.SelectedValue == Convert.ToString(Session[gblValue.BrnchCode])))
                //{
                //    gblFuction.MsgPopup("Already Submitted or replenished....Edit Not allowed");
                //    return;
                //}
                if (Convert.ToString(ViewState["SubmitYN"]) == "Y")
                {
                    gblFuction.MsgPopup("Already Submitted or replenished....Edit Not allowed");
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Int32 vRoleId = 0;
            try
            {
                // this.GetModuleByRole(mnuID.mnuPettyCash);
                vRoleId = Convert.ToInt32(Session[gblValue.RoleId].ToString());
                if (vRoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not delete, Day end already done..");
                            return;
                        }
                    }
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (Convert.ToString(ViewState["SubmitYN"]) == "Y")
                {
                    gblFuction.MsgPopup("Already Submitted or replenished....Edit Not allowed");
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    StatusButton("Show");
                    PettyCashLoadgrid(0);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            CPettyCash oCP = null;
            DataTable dt = null;
            try
            {
                ViewState["StateEdit"] = "Add";
                tabPattyCash.ActiveTabIndex = 1;
                StatusButton("Add");
                oCP = new CPettyCash();
                dt = new DataTable();
                DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
                DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
                DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo]);
                dt = oCP.GetPettyCashCloseBal(vFinFromDt, vLoginDt, Session[gblValue.BrnchCode].ToString(), vFinYr, "P0001");
                if (dt.Rows.Count > 0)
                {
                    txtPettyCashBalance.Text = Convert.ToString(dt.Rows[0]["Bal"]);
                }
                else
                {
                    txtPettyCashBalance.Text = "0";
                }
            }
            finally
            {
                oCP = null;
                dt = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabPattyCash.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

        protected void gvPetty_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vId = "";
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null;
            CPettyCash cPc = null;

            try
            {
                vId = Convert.ToString(e.CommandArgument);
                ViewState["PattyCashId"] = vId;
                fuFileUpload.Dispose();
                Session["PCDoc"] = null;
                lblFileName.Text = "";
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPetty.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    cPc = new CPettyCash();
                    ds = cPc.ShowPettyCashDetails(vId);
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        txtVoucherNo.Text = Convert.ToString(dt1.Rows[0]["PettyCashNo"]);
                        txtDt.Text = Convert.ToString(dt1.Rows[0]["TrDate"]);
                        txtPaidTo.Text = Convert.ToString(dt1.Rows[0]["PaidTo"]);
                        txtNarration.Text = Convert.ToString(dt1.Rows[0]["Narration"]);
                        TxtSendBackRem.Text = Convert.ToString(dt1.Rows[0]["SendBackRem"]);
                        txtReffId.Text = Convert.ToString(dt1.Rows[0]["ReffId"]);
                        //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dt1.Rows[0]["CompanyID"].ToString()));
                        ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(dt1.Rows[0]["PType"].ToString()));
                        ViewState["SubmitYN"] = Convert.ToString(dt1.Rows[0]["SubmitYN"]);
                        //if (Session[gblValue.BrnchCode].ToString() == "0000")
                        //{
                        //    ddlBrch.SelectedIndex = ddlBrch.Items.IndexOf(ddlBrch.Items.FindByValue(dt1.Rows[0]["BranchCode"].ToString()));
                        //    hdnBrnch.Value = Convert.ToString(dt1.Rows[0]["BranchCode"]);
                        //}
                        txtPettyCashBalance.Text = Convert.ToString(dt1.Rows[0]["PettyCashCloseBal"]);
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        txtCrTot.Text = dt2.Compute("Sum(Credit)", "").ToString();
                        txtDrTot.Text = dt2.Compute("Sum(Debit)", "").ToString();
                        //txtPCVNo.Text = Convert.ToString(dt2.Rows[0]["PCV"]);
                        //txtPCVDate.Text = Convert.ToString(dt2.Rows[0]["PCVDt"]);
                        //txtPCCVPayee.Text = Convert.ToString(dt2.Rows[0]["PCVPayee"]);
                        //txtPCVRemarks.Text = Convert.ToString(dt2.Rows[0]["PCVRemarks"]);
                        gvVoucherDtl.DataSource = dt2;
                        gvVoucherDtl.DataBind();
                        ViewState["VouDtl"] = dt2;
                    }
                    tabPattyCash.ActiveTabIndex = 1;
                    StatusButton("Show");
                }
                if (e.CommandName == "cmdFileDownload")
                {
                    // path = ConfigurationManager.AppSettings["PettyCashDocPath"];
                    PettyCashFileDownload(vId, path);
                }
            }

            finally
            {
                ds = null;
                dt1 = null;
                dt2 = null;
                cPc = null;
            }
        }

        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            double DrAmt = 0.00, CrAmt = 0.00;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (txtDt.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (pMode == "Edit" || pMode == "Delete")
            {
                if (txtReffId.Text.Length > 0)
                {
                    gblFuction.AjxMsgPopup("This transaction is already replenished. You can not edit or delete this record...");
                    vResult = false;
                }
            }
            if (txtDt.Text.Trim() != "")
            {
                //if (gblFuction.setDate(txtDt.Text) > vLoginDt)
                //{
                //    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                //    gblFuction.AjxFocus("ctl00_cph_Main_txtDt");
                //    vResult = false;
                //}
                //if (gblFuction.setDate(txtDt.Text) < vLoginDt)
                //{
                //    gblFuction.AjxMsgPopup("Voucher Date should not be less than login date...");
                //    gblFuction.AjxFocus("ctl00_cph_Main_txtDt");
                //    vResult = false;
                //}
                if (gblFuction.setDate(txtDt.Text) < vFinFromDt || gblFuction.setDate(txtDt.Text) > vFinToDt)
                {
                    gblFuction.AjxMsgPopup("Voucher Date should be within Logged In financial year.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtDt");
                    vResult = false;
                }
            }
            DataTable dt = null;
            dt = (DataTable)ViewState["VouDtl"];
            if (ViewState["VouDtl"] != null)
            {
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.AjxMsgPopup("Please Enter At Least One Entry.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                    vResult = false;
                }
            }
            if (dt.Rows.Count > 0)
            {
                for (Int32 i = 0; i < gvVoucherDtl.Rows.Count; i++)
                {
                    DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvVoucherDtl.Rows[i].Cells[1].Text), 2);
                    CrAmt = Math.Round(CrAmt, 2) + Math.Round(Convert.ToDouble(gvVoucherDtl.Rows[i].Cells[2].Text), 2);
                }
            }
            if (DrAmt == 0 && CrAmt == 0)
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Cannot Be ZERO.");
                vResult = false;
            }
            if (txtNarration.Text.Trim() == "")
            {
                //EnableControl(true);
                gblFuction.AjxMsgPopup("Narration can not left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            if (pMode == "Submit")
            {
                if (gblFuction.setDate(txtDt.Text) > vLoginDt)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Submit Date should not be before than Entry date...");
                    vResult = false;
                }

                if (vLoginDt < vFinFromDt || vLoginDt > vFinToDt)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Login Date should be within financial year.");
                    vResult = false;
                }
            }
            if (pMode == "Save")
            {
                if ((Convert.ToDouble(txtPettyCashBalance.Text)-Convert.ToDouble(txtDrTot.Text))<0)
                {
                    gblFuction.AjxMsgPopup("Insufficient Petty cash Balance.");
                    vResult = false;
                }
            }

            return vResult;
        }

        //Convert.ToDouble(txtAmount.Text.Replace("'", "''") == "" ? "0" : txtAmount.Text.Replace("'", "''"));
        private Boolean SaveRecords(string Mode)
        {
            //this.GetModuleByRole(mnuID.mnuPettyCash);
            string vVoucherEdit = Convert.ToString(ViewState["StateEdit"]);
            Boolean vResult = false;
            Int32 vErr = 0, i = 0;
            string vXmlData = "", vVouNo = "", vTranType = "", vHeadID = "", vId = "", vFileExt = "";
            string vBranchCode = Session[gblValue.BrnchCode].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime vDt = gblFuction.setDate(txtDt.Text);
            double vAmount = 0, vPettyCashBal = 0, vAmtLimitChk = 1000;
            path = ConfigurationManager.AppSettings["PettyCashDocPath"];

            //if (txtVoucherNo.Text == "")
            //{
            //    gblFuction.AjxMsgPopup("Please Enter Voucher No.");
            //    return false;
            //}

            if (!fuFileUpload.HasFile)
            {
                if (Session["PCDoc"] != null)
                {
                    fuFileUpload = (FileUpload)Session["PCDoc"];
                }
            }

            if (fuFileUpload.HasFile)
            {
                if (fuFileUpload.PostedFile.ContentLength > 1048576)
                {
                    gblFuction.MsgPopup("File size should not exceeded 1 MB");
                    fuFileUpload.Dispose();
                    Session["PCDoc"] = null;
                    lblFileName.Text = "";
                    return false;
                }
            }

            if (txtPaidTo.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Please Enter Paid To.");
                return false;
            }

            if (txtDrTot.Text == "" || txtDrTot.Text == "0")
                vAmount = Convert.ToDouble(txtCrTot.Text);
            else
                vAmount = Convert.ToDouble(txtDrTot.Text);

            if ((txtPettyCashBalance.Text.Trim() != "") || (txtPettyCashBalance.Text.Trim() != "0"))
            {
                vPettyCashBal = Convert.ToDouble(txtPettyCashBalance.Text.Trim());
            }
            else
            {
                vPettyCashBal = Convert.ToDouble("0");
            }

            string vNarration = Convert.ToString(txtNarration.Text).Trim().ToUpper();
            DataTable dtLedger = null;
            CPettyCash cPC = null;
            try
            {
                //dtLedger = (DataTable)ViewState["VouDtl"];

                vId = Convert.ToString(ViewState["PattyCashId"]);
                vVouNo = txtVoucherNo.Text;
                dtLedger = (DataTable)ViewState["VouDtl"];
                if (dtLedger == null || dtLedger.Rows.Count <= 0)
                {
                    gblFuction.AjxMsgPopup("Please Enter At Least One Entry.");
                    return false;
                }

                foreach (DataRow Tdr in dtLedger.Rows)
                {
                    i = i + 1;
                    Tdr["SlNo"] = i;
                    Tdr["Amt"] = Convert.ToDouble(Tdr["Debit"]) + Convert.ToDouble(Tdr["Credit"]);
                }

                using (StringWriter oSW = new StringWriter())
                {
                    dtLedger.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                if (Mode == "Save")
                {
                    if (vAmount > vAmtLimitChk)
                    {
                        if (fuFileUpload.HasFile == false)
                        {
                            gblFuction.AjxMsgPopup("Please Select File for Upload");
                            return false;
                        }
                    }
                }
                if (Mode == "Edit")
                {
                    if (vAmount > vAmtLimitChk)
                    {
                        if (fuFileUpload.HasFile == false)
                        {
                            DataTable dtChk = new DataTable();
                            cPC = new CPettyCash();
                            dtChk = cPC.GetPCDocUploadStat(vId, vBranchCode);
                            if (dtChk.Rows.Count > 0)
                            {
                                if (Convert.ToString(dtChk.Rows[0]["FileStat"]) == "Y")
                                {
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Please Select File for Upload");
                                    return false;
                                }
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Please Select File for Upload");
                                return false;
                            }
                            dtChk = null;
                            cPC = null;
                        }
                    }
                }

                if (Mode == "Save")
                {
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    vVouNo = txtVoucherNo.Text;
                    cPC = new CPettyCash();
                    vErr = cPC.InsertPettyCash(ref vId, ref vVouNo, txtPaidTo.Text.Trim().Replace("'", "''").ToUpper(), vDt, vAmount,
                        vNarration, vBranchCode, vFinYear, vXmlData, Convert.ToInt32(Session[gblValue.UserId]), "Save", ddlType.SelectedValue,
                        Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vPettyCashBal);
                    if (vErr > 0)
                    {
                        ViewState["PattyCashId"] = vId;
                        ViewState["VouDtl"] = dtLedger;
                        txtVoucherNo.Text = vVouNo;
                        if (fuFileUpload.HasFile)
                        {
                            vFileExt = System.IO.Path.GetExtension(fuFileUpload.FileName);
                            PettyCashFileUpload(vId, path, vFileExt);
                            Session["PCDoc"] = null;
                            Int32 vFileUploadStat = UpdatePCDocUploadStat(vId, vBranchCode);
                        }
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
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    cPC = new CPettyCash();
                    vErr = cPC.InsertPettyCash(ref vId, ref vVouNo, txtPaidTo.Text.Trim().Replace("'", "''").ToUpper(), vDt, vAmount
                        , vNarration, vBranchCode, vFinYear, vXmlData, Convert.ToInt32(Session[gblValue.UserId]), "Edit", ddlType.SelectedValue,
                        Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vPettyCashBal);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        ViewState["VouDtl"] = dtLedger;
                        if (fuFileUpload.HasFile)
                        {
                            vFileExt = System.IO.Path.GetExtension(fuFileUpload.FileName);
                            PettyCashFileUpload(vId, path, vFileExt);
                            Session["PCDoc"] = null;
                            Int32 vFileUploadStat = UpdatePCDocUploadStat(vId, vBranchCode);
                        }
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
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    cPC = new CPettyCash();
                    vErr = cPC.InsertPettyCash(ref vId, ref vVouNo, txtPaidTo.Text.Trim().Replace("'", "''").ToUpper(), vDt,
                        vAmount, vNarration, vBranchCode, vFinYear, vXmlData, Convert.ToInt32(Session[gblValue.UserId]), "Delete", ddlType.SelectedValue,
                        Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vPettyCashBal);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.DeleteMsg);
                        vResult = true;
                        ViewState["VouDtl"] = null;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Submit")
                {
                    Double vAmt = 0.0;
                    CVoucher oBJV = null;
                    oBJV = new CVoucher();
                    DataTable dt = null;
                    DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                    if (ViewState["PattyCashId"] == null)
                    {
                        gblFuction.MsgPopup("Nothing to Submit");
                        return false;
                    }
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vLoginDt)
                            {
                                gblFuction.AjxMsgPopup("You cannot save, Day end already done..");
                                return false;
                            }
                        }
                    }

                    vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                            , vLoginDt, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), "P0001");

                    if (Math.Round(vAmount, 2) > vAmt)
                    {
                        gblFuction.AjxMsgPopup("Insufficient Balance.");
                        return false;
                    }

                    cPC = new CPettyCash();
                    vErr = cPC.InsertPettyCash(ref vId, ref vVouNo, txtPaidTo.Text.Trim().Replace("'", "''").ToUpper(), vLoginDt,
                        vAmount, vNarration, vBranchCode, vFinYear, vXmlData, Convert.ToInt32(Session[gblValue.UserId]), "Submit", ddlType.SelectedValue,
                        Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vPettyCashBal);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["VouDtl"] = null;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cPC = null;
                dtLedger = null;
            }

        }

        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            double vDrAmt = 0, vCrAmt = 0;
            DataTable dt = null;
            try
            {
                if (e.CommandName == "cmdDel")
                {
                    dt = (DataTable)ViewState["VouDtl"];
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    gvVoucherDtl.DataSource = dt;
                    gvVoucherDtl.DataBind();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["DC"].ToString() == "D")
                            vDrAmt += Convert.ToDouble(dr["Amt"]);
                        else if (dr["DC"].ToString() == "C")
                            vCrAmt += Convert.ToDouble(dr["Amt"]);
                    }
                    txtDrTot.Text = vDrAmt.ToString();
                    txtCrTot.Text = vCrAmt.ToString();
                }
                else if (e.CommandName == "cmdEdit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)row.FindControl("btnEdit");
                    int index = row.RowIndex;
                    dt = (DataTable)ViewState["VouDtl"];
                    foreach (GridViewRow gr in gvVoucherDtl.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnEdit");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[index]["DC"].ToString()));
                    //ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));
                    txtLed.Text = dt.Rows[index]["Desc"].ToString();
                    hdLed.Value = dt.Rows[index]["DescId"].ToString();
                    //txtSubLed.Text = dt.Rows[index]["SubLed"].ToString();
                    //hdSubLed.Value = dt.Rows[index]["SubLedID"].ToString();
                    ddlBrch.SelectedIndex = ddlBrch.Items.IndexOf(ddlBrch.Items.FindByValue(dt.Rows[index]["BranchCode"].ToString()));
                    //txtPCVNo.Text = Convert.ToString(dt.Rows[index]["PCV"].ToString());
                    //txtPCVDate.Text = Convert.ToString(dt.Rows[index]["PCVDt"].ToString());
                    //txtPCCVPayee.Text = Convert.ToString(dt.Rows[index]["PCVPayee"].ToString());
                    //txtPCVRemarks.Text = Convert.ToString(dt.Rows[index]["PCVRemarks"].ToString());
                    ddlExpenseHead.SelectedIndex = ddlExpenseHead.Items.IndexOf(ddlExpenseHead.Items.FindByValue(dt.Rows[index]["ExpHeadId"].ToString()));

                    ViewState["ClickMode"] = "G_L";
                    txtAmount.Enabled = true;
                    if (dt.Rows[index]["DC"].ToString() == "D")
                        txtAmount.Text = dt.Rows[index]["Debit"].ToString();
                    else
                        txtAmount.Text = dt.Rows[index]["Credit"].ToString();
                    hdEdit.Value = index.ToString();
                }
                ViewState["VouDtl"] = dt;
            }
            finally
            {

            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Session["PCDoc"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vRptPath = "";
            string pPattyCashId;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime VLgDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                oVoucher = new CVoucher();
                pPattyCashId = Convert.ToString(ViewState["PattyCashId"]);
                dt = oVoucher.rptPettyCashPrint(pPattyCashId);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPettyCashPrint.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", dt.Rows[0]["CompanyName"].ToString());
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
                    rptDoc.SetParameterValue("pDTotal", Convert.ToDouble(txtDrTot.Text.Trim()));
                    rptDoc.SetParameterValue("pCTotal", Convert.ToDouble(txtCrTot.Text.Trim()));

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Petty_Cash_Report");
                    rptDoc.Close(); rptDoc.Dispose();

                }
                Response.ClearContent();
                Response.ClearHeaders();
            }
            finally
            {
                dt = null;
            }
        }

        protected void btnExcelPrint_Click(object sender, EventArgs e)
        {
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTitle = "", vFileNm = "";
            DataTable dt = null, dt1 = null;
            DataSet ds = null;
            CReports oRpt = null;
            CVoucher oVoucher = null;
            string pPattyCashId;
            try
            {

                vTitle = "Petty Cash Excel Report";
                oVoucher = new CVoucher();
                oRpt = new CReports();
                pPattyCashId = Convert.ToString(ViewState["PattyCashId"]);
                dt1 = oVoucher.rptPettyCashPrint(pPattyCashId);

                using (ReportDocument rptDoc = new ReportDocument())
                {
                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();

                    String vCompanyName = "", vPaidTo = "", vVoucherNo = "", vVoucherDt = "", vNarration = "", vBranchName = "";

                    vPaidTo = dt1.Rows[0]["PaidTo"].ToString();
                    vVoucherNo = dt1.Rows[0]["VoucherNo"].ToString();
                    vVoucherDt = dt1.Rows[0]["VoucherDt"].ToString();
                    vNarration = dt1.Rows[0]["Narration"].ToString();
                    vBranchName = dt1.Rows[0]["BranchName"].ToString();

                    dt1.Columns.Remove("BranchName");
                    dt1.Columns.Remove("PaidTo");
                    dt1.Columns.Remove("VoucherNo");
                    dt1.Columns.Remove("VoucherDt");
                    dt1.Columns.Remove("Narration");
                    dt1.Columns["Desc"].ColumnName = "GeneralLedger";

                    dt1.AcceptChanges();

                    DataGrid1.DataSource = dt1;
                    DataGrid1.DataBind();

                    tdx.Controls.Add(DataGrid1);
                    tdx.Visible = false;
                    //vOSAmt = Convert.ToDouble(dt.Rows[0]["LoanAmt"]);

                    //dt1.AcceptChanges();
                    vFileNm = "attachment;filename=PettyCashExcelReport" + ".xls"; ;
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");
                    htw.WriteLine("<tr><td align='left' colspan='" + dt1.Columns.Count + "'><b><u><font size='3'>" + vTitle + " </font></u></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Run Date/Time: " + DateTime.Now.AddMinutes(150) + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td></td><td></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Branch : " + vBranchName + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Company : " + vCompanyName + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Paid To: " + vPaidTo + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Revolving Fund No: " + vVoucherNo + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Date: " + vVoucherDt + "</font></b></td></tr>");

                    htw.WriteLine("</table>");
                    DataGrid1.RenderControl(htw);
                    htw.WriteLine("<table border='0' cellpadding='5' width='100%'>");

                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Narration: " + vNarration + "</font></b></td></tr>");

                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Prepared By: " + Session[gblValue.UserName].ToString() + "</font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Checked By: </font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Approved By: </font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Received By: </font></b></td></tr>");
                    htw.WriteLine("<tr><td align=left' colspan= '" + dt1.Columns.Count + "'><b><font size='3'>Date: </font></b></td></tr>");

                    htw.WriteLine("</table>");

                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //if (Session[gblValue.BrnchCode].ToString() != "0000")
                    //    Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
                    //else
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Write(sw.ToString());
                    Response.End();

                }
                Response.ClearContent();
                Response.ClearHeaders();
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        private void PopExpenseHead()
        {
            DataTable dt = null;
            CPettyCash oPetty = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oPetty = new CPettyCash();
                dt = oPetty.PopPettyCashExpenseHead(vBrCode, vLoginDt);
                if (dt.Rows.Count > 0)
                {
                    ddlExpenseHead.DataSource = dt;
                    ddlExpenseHead.DataTextField = "ExpenseHead";
                    ddlExpenseHead.DataValueField = "PettyMstId";
                    ddlExpenseHead.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlExpenseHead.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oPetty = null;
            }
        }

        protected void ddlExpenseHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlExpenseHead.SelectedIndex > 0) PopPettyCashExpHeadLedger(ddlExpenseHead.SelectedValue);
        }

        private void PopPettyCashExpHeadLedger(string pExpHeadId)
        {
            DataTable dt = null;
            CPettyCash oPetty = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oPetty = new CPettyCash();
                dt = oPetty.PopPettyCashExpHeadLedger(pExpHeadId, vBrCode, vLoginDt);
                if (dt.Rows.Count > 0)
                {
                    txtLed.Text = Convert.ToString(dt.Rows[0]["LedgerDesc"]);
                    hdLed.Value = Convert.ToString(dt.Rows[0]["DescId"]);
                }
                else
                {
                    txtLed.Text = "";
                    hdLed.Value = "-1";
                }
            }
            finally
            {
                dt = null;
                oPetty = null;
            }
        }

        private void PettyCashFileUpload(string pId, string pPath, string pFileExt)
        {
            string vFileName = pId;
            string[] files = System.IO.Directory.GetFiles(pPath, vFileName + ".*");
            if (MinioYN == "Y")
            {
                CApiCalling oAC = new CApiCalling();
                byte[] vFileByte = ConvertFileToByteArray(fuFileUpload.PostedFile);
                oAC.UploadFileMinio(vFileByte, pId + pFileExt, "", PettyCashBucket, MinioUrl);
            }
            else
            {
                foreach (string vFiles in files)
                {
                    File.Delete(vFiles);
                }
                string vFilePath = pPath + pId + pFileExt;
                fuFileUpload.SaveAs(vFilePath);
            }
        }

        protected void PettyCashFileDownload(string pId, string pPath)
        {
            try
            {
                string vDirPath = pPath;
                string vFilePath = "", vFileName = string.Empty;
                string vFileNameWithoutExt = string.Empty, vFileExt = string.Empty, vPathArchive = string.Empty;
                //foreach (var file in Directory.GetFiles(vDirPath))
                //{
                //    vFileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                //    vFileExt = Path.GetExtension(file);
                //    if (pId == vFileNameWithoutExt)
                //    {
                //        vFilePath = pPath + pId + vFileExt;
                //        break;
                //    }
                //}
                //if (File.Exists(vFilePath))
                //{
                //    Response.Clear();
                //    vFileName = vFileNameWithoutExt + vFileExt;
                //    Response.AddHeader("content-disposition", "attachment;filename=" + vFileName);
                //    Response.WriteFile(vFilePath);
                //    Response.End();
                //}
                //else
                //{
                string pathPettyCash = ConfigurationManager.AppSettings["pathPettyCash"];
                string[] arrPathNetwork = pathPettyCash.Split(',');
                string vPathPettyCashDoc = "";
                for (int i = 0; i <= arrPathNetwork.Length - 1; i++)
                {
                    string[] Ext = (".pdf,.png,.jpg,.jpeg").Split(',');
                    for (int j = 0; j < Ext.Length; j++)
                    {
                        vFileName = pId + Ext[j];
                        if (ValidUrlChk(arrPathNetwork[i] + vFileName))
                        {
                            vPathPettyCashDoc = arrPathNetwork[i] + vFileName;
                            break;
                        }
                    }
                    break;
                }
                if (vPathPettyCashDoc != "")
                {
                    WebClient cln = null;
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPathPettyCashDoc);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vFileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
                else
                {
                    gblFuction.MsgPopup("No Data Found..");
                }
                //vPathArchive = ConfigurationManager.AppSettings["PettyCashDocPathArchive"];
                //foreach (var file in Directory.GetFiles(vPathArchive))
                //{
                //    vFileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                //    vFileExt = Path.GetExtension(file);
                //    if (pId == vFileNameWithoutExt)
                //    {
                //        vFilePath = vPathArchive + pId + vFileExt;
                //        break;
                //    }
                //}

                //if (File.Exists(vFilePath))
                //{
                //    Response.Clear();
                //    vFileName = vFileNameWithoutExt + vFileExt;
                //    Response.AddHeader("content-disposition", "attachment;filename=" + vFileName);
                //    Response.WriteFile(vFilePath);
                //    Response.End();
                //}
                //else
                //{
                //    gblFuction.MsgPopup("No Data Found..");
                //}
                //}
            }
            finally
            {
            }
        }
        #region URLExist
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        private Int32 UpdatePCDocUploadStat(string pId, string pBranchCode)
        {
            Int32 vStat = 0;
            CPettyCash oPC = new CPettyCash();
            vStat = oPC.UpdatePCDocUploadStat(pId, pBranchCode);
            return vStat;
        }

        //protected void UploadFileCheck(object sender, EventArgs e)
        //{
        //    if (fuFileUpload.PostedFile.ContentLength > 1048576)
        //    {
        //        gblFuction.MsgPopup("File size should not exceeded 1 MB");
        //        fuFileUpload.Dispose();
        //        return;
        //    }
        //}
        #region ConvertFileToByteArray
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
        #endregion
    }
}
