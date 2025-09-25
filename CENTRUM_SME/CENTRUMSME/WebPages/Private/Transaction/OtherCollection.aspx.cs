using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class OtherCollection : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                StatusButton("View");
                txtEntryDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckFloat(event,this)");
                LoadGrid();
                GetCashBankFrom();
                trPaymentButton.Visible = false;
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Other Collection";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuOtherCollection);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Other Collection", false);
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

        private void StatusButtonApprove(String pMode)
        {
            switch (pMode)
            {
                case "Show":
                    btnPayment.Enabled = true;
                    btnSendBack.Enabled = true;
                    btnUndo.Enabled = true;
                    break;
                case "View":
                    btnPayment.Enabled = false;
                    btnSendBack.Enabled = false;
                    btnUndo.Enabled = true;
                    break;
            }
        }

        private void EnableControl(Boolean Status)
        {
            txtLoanId.Enabled = Status;
            txtEntryDt.Enabled = false;
            ddlCharges.Enabled = Status;
            txtAmount.Enabled = Status;
            txtRemarks.Enabled = Status;

            txtLoanIdApprove.Enabled = false;
            txtEntryDtApprove.Enabled = false;
            ddlChargesApprove.Enabled = false;
            txtAmountApprove.Enabled = false;
            txtRemarksApprove.Enabled = false;
            //txtSGST.Enabled = false;
            //txtSGSTApprove.Enabled = false;
            //txtCGST.Enabled = false;
            //txtCGSTApprove.Enabled = false;
        }

        private void ClearControls()
        {
            txtLoanId.Text = "";
            // txtEntryDt.Text = "";
            ddlCharges.SelectedIndex = -1;
            txtAmount.Text = "";
            txtRemarks.Text = "";
            hdnLoanId.Value = "";
            hdnOCID.Value = "";
            hdnOCIDApprove.Value = "";
            lblDate.Text = "";
            lblUser.Text = "";
            txtCGST.Text = "0";
            txtSGST.Text = "0";
            txtCGSTApprove.Text = "0";
            txtSGSTApprove.Text = "0";
            chkIncludeGST.Checked = false;
            chkIncludeGSTApprove.Checked = false;
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                tbOtherCollection.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    ClearControls();
                    tbOtherCollection.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                ViewState["StateEdit"] = "Edit";
                StatusButton("Edit");
                //LoadGrid(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbOtherCollection.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null || vStateEdit == "")
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            CMember oMem = null;
            int vErr = 0;
            Boolean vResult = false;
            string vLoanId = "", vRemarks = "", vIncludeGST = "N";
            DateTime vEntryDt;
            int vCharges = 0, vOCID = 0;
            double vAmount = 0, vCGSTAmt = 0, vSGSTAmt = 0;
            vLoanId = hdnLoanId.Value;
            vRemarks = txtRemarks.Text;
            vEntryDt = gblFuction.setDate(txtEntryDt.Text);
            vCharges = Convert.ToInt32(ddlCharges.SelectedValue);
            vAmount = Convert.ToDouble(txtAmount.Text);
            vCGSTAmt = Convert.ToDouble(txtCGST.Text);
            vSGSTAmt = Convert.ToDouble(txtSGST.Text);
            vIncludeGST = chkIncludeGST.Checked == true ? "Y" : "N";

            if (Convert.ToInt32(Session[gblValue.RoleId]) != 1) 
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vEntryDt)
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return false;
                    }
                }
            }

            
            if (Mode == "Save")
            {
                oMem = new CMember();
                vErr = oMem.SaveOtherCollection(ref vOCID, vLoanId, vCharges, vEntryDt, vAmount, vRemarks, "Save", Convert.ToInt32(Session[gblValue.UserId]), vCGSTAmt, vSGSTAmt, vIncludeGST);
                if (vErr == 1)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    LoadGrid();
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            if (Mode == "Edit")
            {
                vOCID = Convert.ToInt32(hdnOCID.Value);
                oMem = new CMember();
                vErr = oMem.SaveOtherCollection(ref vOCID, vLoanId, vCharges, vEntryDt, vAmount, vRemarks, "Edit", Convert.ToInt32(Session[gblValue.UserId]), vCGSTAmt, vSGSTAmt, vIncludeGST);
                if (vErr == 1)
                {
                    gblFuction.MsgPopup(gblPRATAM.EditMsg);
                    LoadGrid();
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            if (Mode == "Delete")
            {
                vOCID = Convert.ToInt32(hdnOCID.Value);
                oMem = new CMember();
                vErr = oMem.SaveOtherCollection(ref vOCID, vLoanId, vCharges, vEntryDt, vAmount, vRemarks, "Delete", Convert.ToInt32(Session[gblValue.UserId]), vCGSTAmt, vSGSTAmt, vIncludeGST);
                if (vErr == 1)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }

            return vResult;
        }

        private void LoadGrid()
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = oMem.GetOtherCollectionList(vBrCode);
                gvOtherCollection.DataSource = dt;
                gvOtherCollection.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }

        }

        protected void gvOtherCollection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pOCID = 0;
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            pOCID = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                string vPaidYN = Convert.ToString(gvRow.Cells[5].Text);
                foreach (GridViewRow gr in gvOtherCollection.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                dt = oMem.GetOtherCollectionById(pOCID);
                txtLoanId.Text = dt.Rows[0]["Name"].ToString();
                hdnLoanId.Value = dt.Rows[0]["LoanId"].ToString();
                txtEntryDt.Text = dt.Rows[0]["EntryDate"].ToString();
                txtAmount.Text = dt.Rows[0]["Amount"].ToString();
                txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
                hdnOCID.Value = dt.Rows[0]["OCID"].ToString();
                ddlCharges.SelectedIndex = ddlCharges.Items.IndexOf(ddlCharges.Items.FindByValue(dt.Rows[0]["Charges"].ToString()));
                txtCGST.Text = dt.Rows[0]["CGST"].ToString();
                txtSGST.Text = dt.Rows[0]["SGST"].ToString();
                chkIncludeGST.Checked = dt.Rows[0]["IncludeGST"].ToString() == "Y" ? true : false;

                tbOtherCollection.ActiveTabIndex = 1;
                if (vPaidYN == "N")
                {
                    StatusButton("Show");
                }
                else
                {
                    StatusButton("View");
                }
                EnableControl(false);
            }
            if (e.CommandName == "cmdPayment")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnPayment");
                string vPaidYN = Convert.ToString(gvRow.Cells[5].Text);
                foreach (GridViewRow gr in gvOtherCollection.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnPayment");
                    lb.ForeColor = System.Drawing.Color.Black;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;

                dt = oMem.GetOtherCollectionById(pOCID);
                txtLoanIdApprove.Text = dt.Rows[0]["Name"].ToString();
                txtEntryDtApprove.Text = dt.Rows[0]["EntryDate"].ToString();
                hdnLoanIdApprove.Value = dt.Rows[0]["LoanId"].ToString();
                txtAmountApprove.Text = dt.Rows[0]["Amount"].ToString();
                txtRemarksApprove.Text = dt.Rows[0]["Remarks"].ToString();
                hdnOCIDApprove.Value = dt.Rows[0]["OCID"].ToString();
                ddlChargesApprove.SelectedIndex = ddlChargesApprove.Items.IndexOf(ddlChargesApprove.Items.FindByValue(dt.Rows[0]["Charges"].ToString()));
                txtCGSTApprove.Text = dt.Rows[0]["CGST"].ToString();
                txtSGSTApprove.Text = dt.Rows[0]["SGST"].ToString();
                //Request.Form[txtCGSTApprove.UniqueID] = dt.Rows[0]["CGST"].ToString();
                //Request.Form[txtSGSTApprove.UniqueID] = dt.Rows[0]["SGST"].ToString();
                chkIncludeGSTApprove.Checked = dt.Rows[0]["IncludeGST"].ToString() == "Y" ? true : false;

                tbOtherCollection.ActiveTabIndex = 2;
                //StatusButton("Show");
                trButton.Visible = false;
                trPaymentButton.Visible = true;
                EnableControl(false);
                if (vPaidYN == "N")
                {
                    StatusButtonApprove("Show");
                    ddlPayTo.Enabled = true;
                    txtChqNo.Enabled = true;
                    txtChqDt.Enabled = true;
                }
                else
                {
                    if (dt.Rows[0]["CashYN"].ToString() == "N")
                    {
                        txtChqNo.Text = dt.Rows[0]["ChequeNo"].ToString();
                        txtChqDt.Text = dt.Rows[0]["ChequeDt"].ToString();
                        tblBank.Visible = true;
                        txtChqNo.Enabled = false;
                        txtChqDt.Enabled = false;
                    }
                    else
                    {
                        txtChqNo.Text = "";
                        txtChqDt.Text = "";
                        tblBank.Visible = false;
                    }
                    ddlPayTo.SelectedIndex = ddlPayTo.Items.IndexOf(ddlPayTo.Items.FindByValue(dt.Rows[0]["DebitAC"].ToString()));
                    ddlPayTo.Enabled = false;
                    StatusButtonApprove("View");
                }
            }
        }

        private void GetCashBankFrom()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBrCode = "";
            try
            {

                vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlPayTo.DataSource = dt;
                ddlPayTo.DataTextField = "Desc";
                ddlPayTo.DataValueField = "DescId";
                ddlPayTo.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlPayTo.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }

        protected void ddlPayTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPayTo.SelectedValue == "C0001")
            {
                tblBank.Visible = false;
            }
            else
            {
                tblBank.Visible = true;
            }

        }

        protected void btnUndo_Click(object sender, EventArgs e)
        {
            tbOtherCollection.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
            trButton.Visible = true;
            trPaymentButton.Visible = false;
        }

        protected void btnPayment_Click(object sender, EventArgs e)
        {
            CMember oMem = null;
            int vErr = 0;
            string vLoanId = "", vIncludeGST = "N";
            DateTime vEntryDt;
            int vCharges = 0, vOCID = 0;
            double vAmount = 0, vCGSTAmt = 0, vSGSTAmt;
            vLoanId = hdnLoanIdApprove.Value;
            vEntryDt = gblFuction.setDate(txtEntryDtApprove.Text);
            vCharges = Convert.ToInt32(ddlChargesApprove.SelectedValue);
            vAmount = Convert.ToDouble(txtAmountApprove.Text);
            vOCID = Convert.ToInt32(hdnOCIDApprove.Value);
            vCGSTAmt = Convert.ToDouble(txtCGSTApprove.Text);
            vSGSTAmt = Convert.ToDouble(txtSGSTApprove.Text);
            vIncludeGST = chkIncludeGSTApprove.Checked == true ? "Y" : "N";

            oMem = new CMember();
            vErr = oMem.ApproveOtherCollection(vOCID, vLoanId, vCharges, vEntryDt, vAmount, ddlPayTo.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]),
               Convert.ToString(Session[gblValue.BrnchCode]), txtChqNo.Text, gblFuction.setDate(txtChqDt.Text), vCGSTAmt, vSGSTAmt, vIncludeGST);
            if (vErr == 1)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButtonApprove("View");
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
                return;
            }
        }


    }
}