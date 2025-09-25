using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Linq;
using FORCECA;
using FORCEBA;
using System.IO;
using System.Collections.Generic;
using CENTRUM.Service_Equifax;
using System.Xml;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net;
using System.Text;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class PettyCash_Replenish : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected string path = ConfigurationManager.AppSettings["PettyCashDocPath"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["ClickMode"] = "Load";
                ViewState["ChkCashBank"] = null;
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;

                tabPurps.ActiveTabIndex = 0;
                // tabPurps.Tabs[1].Enabled = false;
                StatusButton("View");
                //GetLedGrid(Convert.ToInt32(ddlCompanySearch.SelectedValue));
                txtFrmDt.Text = txtToDt.Text = Session[gblValue.LoginDate].ToString();
                popBranch();        //inside for different Dept                
                PopBranchRep();        // Before Add to get unReplenish Data
                LoadAcGenLedAll();
                //LoadSearchList(Convert.ToInt32(0));
                btnShowList_Click(btnShowList, null);
                btnPCFileDownload.Enabled = false;
                tabPurps.ActiveTabIndex = 1;
            }
        }

        private void LoadAcGenLed()
        {
            DataTable dt = new DataTable();
            CVoucher oVoucher = null;
            string vBranch = Convert.ToString(ddlBr.SelectedValue);
            oVoucher = new CVoucher();
            ddlCashBank.Items.Clear();
            // dt = oVoucher.GetAcGenLedCB_New("000", "CB", "", "");
            dt = oVoucher.GetAcGenLedCB(vBranch, "Y", "", "");
            if (dt.Rows.Count > 0)
            {
                ddlCashBank.DataSource = dt;
                ddlCashBank.DataTextField = "Desc";
                ddlCashBank.DataValueField = "DescId";
                ddlCashBank.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlCashBank.Items.Insert(0, liSel);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            double DrAmt = 0.00, CrAmt = 0.00;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (txtVoucherDt.Text.Trim() == "")
            {
                //EnableControl(true);
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (txtVoucherDt.Text.Trim() != "")
            {
                if (gblFuction.setDate(txtVoucherDt.Text) > vLoginDt)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                    vResult = false;
                }
                if (gblFuction.setDate(txtVoucherDt.Text) < vLoginDt)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Voucher Date should not be less than login date...");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                    vResult = false;
                }
                if (gblFuction.setDate(txtVoucherDt.Text) < vFinFromDt || gblFuction.setDate(txtVoucherDt.Text) > vFinToDt)
                {
                    //EnableControl(true);
                    gblFuction.AjxMsgPopup("Voucher Date should be within Logged In financial year.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                    vResult = false;
                }
            }
            if (ddlRecpPay.SelectedIndex == -1)
            {
                //EnableControl(true);
                gblFuction.AjxMsgPopup("Voucher Type Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlRecpPay");
                vResult = false;
            }

            //if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[2].Text == "0.00" && ddlRecpPay.SelectedValue == "P")
            //{
            //    gblFuction.AjxMsgPopup("Invalid Payment Voucher.");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
            //    vResult = false;
            //}
            //if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[1].Text == "0.00" && ddlRecpPay.SelectedValue == "R")
            //{
            //    gblFuction.AjxMsgPopup("Invalid Receipt Voucher.");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
            //    vResult = false;
            //}
            //if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[2].Text == "0" && ddlRecpPay.SelectedValue == "P") //for edit mode
            //{
            //    gblFuction.AjxMsgPopup("Invalid Payment Voucher.");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
            //    vResult = false;
            //}
            //if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[1].Text == "0" && ddlRecpPay.SelectedValue == "R") //for edit mode
            //{
            //    gblFuction.AjxMsgPopup("Invalid Receipt Voucher.");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
            //    vResult = false;
            //}

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

            //if (Math.Round(CrAmt, 2) != Math.Round(DrAmt, 2))
            //{
            //    gblFuction.AjxMsgPopup("Debit And Credit Amount Should Be Equal.");
            //    vResult = false;
            //}

            if (txtNarration.Text.Trim() == "")
            {
                //EnableControl(true);
                gblFuction.AjxMsgPopup("Narration cannot left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            if (pMode == "SendBack")
            {
                if (TxtSendBackRem.Text.Trim() == "")
                {
                    gblFuction.AjxMsgPopup("Send back remarks cannot left Blank.");
                    gblFuction.AjxFocus("ctl00_cph_Main_TxtSendBackRem");
                    vResult = false;
                }
            }
            //if (GetPIDCODE(ddlCashBank.SelectedValue) != "C" && pMode == "Save")
            //if (hbCb.Value == "B" && pMode == "Save")
            //{
            //if (txtChequeNo.Text.Trim() == "")
            //{
            //    //EnableControl(true);
            //    gblFuction.AjxMsgPopup("Please Enter the cheque No...");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeNo");
            //    vResult = false;
            //}
            //if (txtChequeDt.Text.Trim() == "")
            //{
            //    //EnableControl(true);
            //    gblFuction.AjxMsgPopup("Please Enter the cheque Date...");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
            //    vResult = false;
            //}
            //if (txtChequeDt.Text.Trim() != "")
            //{
            //    if (gblFuction.IsDate(txtChequeDt.Text) == false)
            //    {
            //        //EnableControl(true);
            //        gblFuction.AjxMsgPopup("Please Enter Valid Cheque Date.");
            //        gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
            //        vResult = false;
            //    }
            //}
            //}
            //else if (pMode == "Edit")
            //{
            //    if (txtBankName.Text.Trim() != "")
            //    {
            //if (txtChequeNo.Text.Trim() == "")
            //{
            //    //EnableControl(true);
            //    gblFuction.AjxMsgPopup("Please Enter the cheque No...");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeNo");
            //    vResult = false;
            //}
            //if (txtChequeDt.Text.Trim() == "")
            //{
            //    //EnableControl(true);
            //    gblFuction.AjxMsgPopup("Please Enter the cheque Date...");
            //    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
            //    vResult = false;
            //}
            //if (txtChequeDt.Text.Trim() != "")
            //{
            //    if (gblFuction.IsDate(txtChequeDt.Text) == false)
            //    {
            //        //EnableControl(true);
            //        gblFuction.AjxMsgPopup("Please Enter Valid Cheque Date.");
            //        gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
            //        vResult = false;
            //    }
            //}
            //}
            //}
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranchRep()
        {
            DataTable dt = null;
            CUser oUsr = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                oUsr = new CUser();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBr.DataSource = dt;
                ddlBr.DataTextField = "BranchName";
                ddlBr.DataValueField = "BranchCode";
                ddlBr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBr.Items.Insert(0, oli);

                ddlBrList.DataSource = dt;
                ddlBrList.DataTextField = "BranchName";
                ddlBrList.DataValueField = "BranchCode";
                ddlBrList.DataBind();
                ddlBrList.Items.Insert(0, oli);

                ddlOverrideBranch.DataSource = dt;
                ddlOverrideBranch.DataTextField = "BranchName";
                ddlOverrideBranch.DataValueField = "BranchCode";
                ddlOverrideBranch.DataBind();
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
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                //this.PageHeading = "Petty Cash Replenish";
                this.PageHeading = "Petty Cash Approve";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPettyReplenish);
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
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    //Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PattyCash_Replenish", false);
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "PattyCash_Approve", false);
                }

                btnAdd.Visible = false;
                btnEdit.Visible = false;
                btnDelete.Visible = false;
                if (this.CanEdit == "Y") btnSendBack.Visible = true;
                else btnSendBack.Visible = false;
                btnPCFileDownload.Enabled = false;
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
                    btnSendBack.Enabled = true;
                    //  ddlPrintOption.Enabled = false;
                    //ClearControls();
                    btnPCFileDownload.Enabled = true;
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = false;
                    //ddlPrintOption.Enabled = true;
                    btnPCFileDownload.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    //ddlPrintOption.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnSendBack.Enabled = false;
                    //ddlPrintOption.Enabled = false;
                    btnPCFileDownload.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    //ddlPrintOption.Enabled = false;
                    EnableControl(false);
                    break;
            }
        }
        private void GetLedGrid(int vCompId)
        {
            DataTable dt = null;
            CPettyReplenish oFed = null;
            try
            {
                oFed = new CPettyReplenish();
                dt = oFed.GetPettyCash_ReplenishPG(vCompId, ddlBr.SelectedValue, ddlTypeSrch.SelectedValue);
                gvReplenishList.DataSource = dt.DefaultView;
                gvReplenishList.DataBind();
            }
            finally
            {
                dt = null;
                oFed = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void gvPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string vID = "";
        //    DataSet ds = null;
        //    DataTable dt = null, dt1 = null;
        //    CPettyReplenish oPr = null;
        //    try
        //    {
        //        vID = Convert.ToString(e.CommandArgument);
        //        ViewState["RFRepId"] = vID;
        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
        //            foreach (GridViewRow gr in gvPurpose.Rows)
        //            {
        //                LinkButton lb = (LinkButton)gr.FindControl("btnShow");
        //                lb.ForeColor = System.Drawing.Color.Black;
        //            }
        //            btnShow.ForeColor = System.Drawing.Color.Red;
        //            oPr = new CPettyReplenish();
        //            ds = oPr.GetPettyCash_ReplenishById(vID, Session[gblValue.ACVouMst].ToString());
        //            dt = ds.Tables[0];
        //            dt1 = ds.Tables[1];
        //            if (dt.Rows.Count > 0)
        //            {
        //                GetVoucherRPById(dt.Rows[0]["HeadID"].ToString());
        //            }
        //            if (dt1.Rows.Count > 0)
        //            {
        //                ddlBr.SelectedIndex = ddlBr.Items.IndexOf(ddlBr.Items.FindByValue(dt1.Rows[0]["BranchCode"].ToString()));
        //                ddlBr.Enabled = false;

        //                ListItem oLi = new ListItem(dt1.Rows[0]["PettyCashNo"].ToString(), dt1.Rows[0]["PettyCashId"].ToString());
        //                ddlRVF.Items.Insert(0, oLi);
        //                ddlRVF.SelectedIndex = ddlRVF.Items.IndexOf(ddlRVF.Items.FindByValue(dt1.Rows[0]["PettyCashId"].ToString()));
        //                ddlRVF.Enabled = false;

        //                ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByText(dt1.Rows[0]["Bank"].ToString()));

        //            }
        //            tabPurps.ActiveTabIndex = 1;
        //            StatusButton("Show");
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        ds = null;
        //        oPr = null;
        //        dt1 = null;
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pHeadId"></param>
        private void GetVoucherRPById(string pHeadId)
        {
            double vTotDr = 0, vTotCr = 0;
            CVoucher oVoucher = null;
            //SortedList Obj = new SortedList();
            string vSubYN = string.Empty;
            DataTable dt = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                ViewState["HeadId"] = pHeadId;
                dt = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode);
                ViewState["VouDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    //hbCb.Value = Convert.ToString(dt.Rows[dt.Rows.Count - 1]["AcType"]);
                    txtVoucherNo.Text = Convert.ToString(dt.Rows[0]["VoucherNo"]);
                    txtVoucherDt.Text = gblFuction.getStrDate(Convert.ToString(dt.Rows[0]["VoucherDt"]));
                    ddlRecpPay.SelectedIndex = ddlRecpPay.Items.IndexOf(ddlRecpPay.Items.FindByValue(Convert.ToString(dt.Rows[0]["VoucherType"])));
                    ddlDrCr.SelectedIndex = -1;
                    //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(Convert.ToString(dt.Rows[0]["CompanyId"])));
                    if (ddlRecpPay.Text == "P".Trim())
                    {
                        ddlDrCr.SelectedIndex = 0;
                    }
                    else
                    {
                        ddlDrCr.SelectedIndex = 1;
                    }

                    //if (Convert.ToString(dt.Rows[0]["CostCBranchCode"]) != "")
                    //{
                    //    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[0]["CostCBranchCode"])));
                    //}
                    //if (Convert.ToString(dt.Rows[0]["SubLedID"]) != "")
                    //{
                    //    hdSubLed.Value = Convert.ToString(dt.Rows[0]["SubLedID"]);
                    //    txtSubLed.Text = Convert.ToString(dt.Rows[0]["SubLed"]);
                    //}

                    hdLed.Value = Convert.ToString(dt.Rows[0]["DescId"]);
                    txtLed.Text = Convert.ToString(dt.Rows[0]["Desc"]);

                    txtAmount.Text = Convert.ToString(dt.Rows[0]["Amt"]);
                    //ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByValue(Convert.ToString(dt.Rows[dt.Rows.Count - 1]["DescId"])));
                    hdCBank.Value = Convert.ToString(dt.Rows[dt.Rows.Count - 1]["DescId"]);
                    txtCBank.Text = Convert.ToString(dt.Rows[dt.Rows.Count - 1]["Desc"]);

                    if (gblFuction.getStrDate(Convert.ToString(dt.Rows[0]["ChequeDt"])) == "01/01/2000")
                        txtChequeDt.Text = "";
                    else
                        txtChequeDt.Text = gblFuction.getStrDate(Convert.ToString(dt.Rows[0]["ChequeDt"]));
                    txtChequeNo.Text = Convert.ToString(dt.Rows[0]["ChequeNo"]);
                    txtBankName.Text = Convert.ToString(dt.Rows[0]["Bank"]);
                    txtNarration.Text = Convert.ToString(dt.Rows[0]["Narration"]);
                    txtPaidTo.Text = Convert.ToString(dt.Rows[0]["PaidTo"]);
                    btnApply.Enabled = false;
                    gvVoucherDtl.DataSource = dt.DefaultView;
                    gvVoucherDtl.DataBind();
                    foreach (DataRow Tdr in dt.Rows)
                    {
                        vTotDr += Convert.ToDouble(Tdr["Debit"]);
                        vTotCr += Convert.ToDouble(Tdr["Credit"]);
                    }
                    txtDrTot.Text = vTotDr.ToString();
                    txtCrTot.Text = vTotCr.ToString();
                    StatusButton("Show");
                }
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRVF_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCashBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            hbCb.Value = "B";
            if (ddlRVF.SelectedIndex > 0)
                LoadGrid();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void ddlCompanySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopBranchRep();

        }

        protected void ddlBr_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = null;
            CPettyReplenish oCra = null;
            string vBrCode = "";
            vBrCode = Convert.ToString(ddlBr.SelectedValue);
            ddlRVF.Items.Clear();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCra = new CPettyReplenish();
                dt = oCra.GetUnReplpettyCash(vBrCode, vLoginDt);
                if (dt.Rows.Count > 0)
                {
                    ddlRVF.DataSource = dt;
                    ddlRVF.DataTextField = "PettyCashNo";
                    ddlRVF.DataValueField = "PettyCashId";
                    ddlRVF.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlRVF.Items.Insert(0, oLi);
                    ddlRVF.SelectedIndex = ddlRVF.Items.IndexOf(ddlRVF.Items.FindByValue(dt.Rows[0]["PettyCashId"].ToString()));
                }
                else
                {
                    ddlRVF.DataSource = null;
                    ddlRVF.DataBind();
                }
                LoadAcGenLed();
            }
            finally
            {
                dt = null;
                //oCra = null;
            }
        }
        private void LoadGrid()
        {
            double vTotDr = 0.0;
            double vTotCr = 0.0;
            DataTable dt = null;
            DataTable dt1 = null;
            DataSet ds = null;
            CPettyReplenish oCra = null;
            if (ddlBr.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please select a Branch");
                return;
            }
            if (ddlRVF.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please select a Revolvong Fund No.");
                return;
            }
            if (ddlCashBank.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please select a Cash / Bank");
                return;
            }

            string vBrCode = "", vRVFNo = "", vCashBank = "";
            vBrCode = Convert.ToString(ddlBr.SelectedValue);
            vRVFNo = Convert.ToString(ddlRVF.SelectedValue);
            vCashBank = Convert.ToString(ddlCashBank.SelectedValue);

            try
            {
                oCra = new CPettyReplenish();
                ds = oCra.GetLedgerGrid(vBrCode, vRVFNo, vCashBank);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    gvVoucherDtl.DataSource = dt;
                    gvVoucherDtl.DataBind();
                    ViewState["VouDtl"] = dt;
                    foreach (DataRow Tdr in dt.Rows)
                    {
                        vTotDr += Convert.ToDouble(Tdr["Debit"]);
                        vTotCr += Convert.ToDouble(Tdr["Credit"]);
                    }
                    txtDrTot.Text = vTotDr.ToString();
                    txtCrTot.Text = vTotCr.ToString();
                }
                else
                {
                    gvVoucherDtl.DataSource = null;
                    gvVoucherDtl.DataBind();
                    txtAmount.Text = "0";
                    ViewState["VouDtl"] = null;
                }
                if (dt1.Rows.Count > 0)
                {
                    txtPaidTo.Text = Convert.ToString(dt1.Rows[0]["PaidTo"]);
                    //ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dt1.Rows[0]["CompanyID"].ToString()));
                    //ddlCompany.Enabled = false;
                    txtBankName.Text = Convert.ToString(dt1.Rows[0]["BankCash"]);
                    txtNarration.Text = Convert.ToString(dt1.Rows[0]["Narration"]);
                    TxtSendBackRem.Text = Convert.ToString(dt1.Rows[0]["SendBackRem"]);
                }
            }
            finally
            {
                dt = null;
                //  oCra = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtVoucherNo.Enabled = false;
            //txtVoucherDt.Enabled = Status;
            txtBankName.Enabled = Status;
            ddlRecpPay.Enabled = Status;
            //ddlCashBank.Enabled = Status;
            txtCBank.Enabled = Status;
            ddlDrCr.Enabled = Status;
            txtLed.Enabled = Status;
            txtSubLed.Enabled = Status;
            ddlBranch.Enabled = Status;
            txtAmount.Enabled = Status;
            txtChequeNo.Enabled = Status;
            txtChequeDt.Enabled = Status;
            txtNarration.Enabled = Status;
            txtPaidTo.Enabled = Status;
            txtDrTot.Enabled = Status;
            txtCrTot.Enabled = Status;
            ddlCompany.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
            btnApply.Enabled = Status;
            chkVouPostYN.Enabled = Status;
            TxtSendBackRem.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtVoucherNo.Text = "";
            ddlRecpPay.SelectedIndex = 0;
            //ddlCashBank.SelectedIndex = 0;
            txtCBank.Text = "";
            hdCBank.Value = "-1";
            ddlDrCr.SelectedIndex = 0;
            hdSubLed.Value = "-1";
            txtSubLed.Text = "";
            ddlBranch.SelectedIndex = -1;
            hdLed.Value = "-1";
            txtLed.Text = "";
            txtAmount.Text = "0";
            txtChequeNo.Text = "";
            txtChequeDt.Text = "";
            txtNarration.Text = "";
            txtCrTot.Text = "0";
            txtDrTot.Text = "0";
            txtBankName.Text = "";
            ddlCompany.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            gvVoucherDtl.DataSource = null;
            gvVoucherDtl.DataBind();
            chkVouPostYN.Checked = false;
            TxtSendBackRem.Text = "";
        }

        protected void btnSrchShow_Click(object sender, EventArgs e)
        {
            GetLedGrid(0);
        }

        protected void btnAdd_Click_Old(object sender, EventArgs e)
        {
            try
            {
                if (ddlBr.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please select a Branch");
                    return;
                }
                if (ddlRVF.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please select a Revolvong Fund No.");
                    return;
                }
                if (ddlCashBank.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please select a Cash / Bank");
                    return;
                }
                ViewState["StateEdit"] = "Add";
                ViewState["Edit"] = "DescId";
                StatusButton("Add");
                ClearControls();
                LoadGrid();
                tabPurps.ActiveTabIndex = 1;
                gvVoucherDtl.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                ViewState["Edit"] = "DescId";
                StatusButton("Add");
                //ClearControls();
                //LoadGrid();
                tabPurps.ActiveTabIndex = 2;
                gvVoucherDtl.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                //GetLedGrid(Convert.ToInt32(0));
                LoadSearchList(Convert.ToInt32(0));
                LoadSearchPendingList(Convert.ToInt32(0));
                tabPurps.ActiveTabIndex = 0;
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ViewState["RFRepId"] = "";
            ViewState["PettyCashId"] = "";
            ViewState["BrCode"] = "";
            LoadSearchList(Convert.ToInt32(0));
            LoadSearchPendingList(Convert.ToInt32(0));
            tabPurps.ActiveTabIndex = 1;
            EnableControl(false);
            StatusButton("View");
            btnPCFileDownload.Enabled = false;
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.CanEdit == "N")
            //    {
            //        gblFuction.MsgPopup(MsgAccess.Edit);
            //        return;
            //    }
            //    ViewState["StateEdit"] = "Edit";
            //    StatusButton("Edit");
            //    gvVoucherDtl.Enabled = false;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.CanDelete == "N")
            //    {
            //        gblFuction.MsgPopup(MsgAccess.Del);
            //        return;
            //    }
            //    if (SaveRecords("Delete") == true)
            //    {
            //        gblFuction.MsgPopup(gblMarg.DeleteMsg);
            //        GetLedGrid(Convert.ToInt32(0));
            //        ClearControls();
            //        tabPurps.ActiveTabIndex = 0;
            //        StatusButton("Delete");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            ViewState["RFRepId"] = null;
            ViewState["PettyCashId"] = null;
            ViewState["BrCode"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculator_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("E:\\WebApps\\HondaLMS\\calc.exe");
            p.WaitForInputIdle();
            //NativeMethods.SetParent(p.MainWindowHandle, this.Handle);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            if (hdLed.Value == "-1" || hdLed.Value == "0")
            {
                gblFuction.AjxMsgPopup("Please Select One Ledger...");
                return;
            }
            if (hdCBank.Value == "-1" || hdCBank.Value == "0")
            {
                gblFuction.AjxMsgPopup("Please Select One Ledger...");
                return;
            }
            string EditMode = string.Empty;
            EditMode = ViewState["ClickMode"].ToString();
            string vChkCashBank = hbCb.Value;
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            DataRow dr;
            dt = (DataTable)ViewState["VouDtl"];
            string vChkNum = "";
            double Num;
            bool isNum = false;
            vChkNum = txtAmount.Text.Trim();
            isNum = double.TryParse(vChkNum, out Num);

            if (vChkCashBank == "C")
            {
                txtChequeNo.Text = "";
                txtBankName.Text = "";
                txtChequeDt.Text = "";
                txtPaidTo.Text = "";
                txtChequeNo.Enabled = false;
                txtBankName.Enabled = false;
                txtChequeDt.Enabled = false;
                txtPaidTo.Enabled = false;
            }
            if (vChkCashBank == "B")
            {
                txtChequeNo.Enabled = true;
                txtBankName.Enabled = true;
                txtChequeDt.Enabled = true;
                txtPaidTo.Enabled = true;
            }


            if (isNum == false)
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Invalid Amount...");
                gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
                return;
            }

            if (ViewState["VouDtl"] != null && hdEdit.Value == "-1")
            {
                dr = dt.NewRow();
                dr["DescId"] = hdLed.Value;// ddlLedger.SelectedValue.ToString();
                dr["Desc"] = txtLed.Text;// ddlLedger.SelectedItem.Text;

                if (hdSubLed.Value.ToString() != "-1")
                {
                    dr["SubLedID"] = hdSubLed.Value;
                    dr["SubLed"] = txtSubLed.Text;
                }
                else
                {
                    dr["SubLedID"] = "";
                    dr["SubLed"] = "";
                }
                if (ddlBranch.SelectedValue.ToString() != "-1")
                {
                    dr["CostCBranchCode"] = ddlBranch.SelectedValue.ToString();
                    dr["CostCBranch"] = ddlBranch.SelectedItem.Text;
                }
                else
                {
                    dr["CostCBranchCode"] = "";
                    dr["CostCBranch"] = "";
                }
                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["Debit"] = txtAmount.Text;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = txtAmount.Text;
                    dr["DC"] = "C";
                }
                dt.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["DescId"]) == hdCBank.Value)
                        dt.Rows.RemoveAt(i);
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
                dr = dt.NewRow();
                dr["DescId"] = hdCBank.Value;// ddlCashBank.SelectedValue.ToString();
                dr["Desc"] = txtCBank.Text;// ddlCashBank.SelectedItem.Text;

                dr["SubLedID"] = "";
                dr["SubLed"] = "";

                dr["CostCBranchCode"] = "";
                dr["CostCBranch"] = "";

                if (vDrAmt - vCrAmt < 0)
                {
                    dr["Debit"] = vCrAmt - vDrAmt;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = vDrAmt - vCrAmt;
                    dr["DC"] = "C";
                }
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value == "-1")
            {
                dt = new DataTable();
                dt.Columns.Add("DC");
                dt.Columns.Add("Debit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("Credit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add("DescId");
                dt.Columns.Add("SubLedID");
                dt.Columns.Add("SubLed");
                dt.Columns.Add("CostCBranchCode");
                dt.Columns.Add("CostCBranch");
                dt.Columns.Add("Desc");
                dt.Columns.Add("DtlId");
                dt.Columns.Add("Amt");
                dr = dt.NewRow();

                dr["DescId"] = hdLed.Value;
                dr["Desc"] = txtLed.Text;
                if (hdSubLed.Value.ToString() != "-1")
                {
                    dr["SubLedID"] = hdSubLed.Value;
                    dr["SubLed"] = txtSubLed.Text;
                }
                else
                {
                    dr["SubLedID"] = "";
                    dr["SubLed"] = "";
                }

                if (ddlBranch.SelectedValue.ToString() != "-1")
                {
                    dr["CostCBranchCode"] = ddlBranch.SelectedValue.ToString();
                    dr["CostCBranch"] = ddlBranch.SelectedItem.Text;
                }
                else
                {
                    dr["CostCBranchCode"] = "";
                    dr["CostCBranch"] = "";
                }

                if (ddlDrCr.SelectedValue == "D")
                {
                    dr["Debit"] = txtAmount.Text;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = txtAmount.Text;
                    dr["DC"] = "C";
                }
                dt.Rows.Add(dr);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["DescId"]) == hdCBank.Value)
                        dt.Rows.RemoveAt(i);
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
                dr = dt.NewRow();
                dr["DescId"] = hdCBank.Value;
                dr["Desc"] = txtCBank.Text;

                dr["SubLedID"] = "";
                dr["SubLed"] = "";

                dr["CostCBranchCode"] = "";
                dr["CostCBranch"] = "";


                if (vDrAmt - vCrAmt < 0)
                {
                    dr["Debit"] = vCrAmt - vDrAmt;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = vDrAmt - vCrAmt;
                    dr["DC"] = "C";
                }
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value != "-1" && EditMode == "G_L")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["DescId"] = hdLed.Value;
                dt.Rows[vR]["Desc"] = txtLed.Text;

                if (hdSubLed.Value.ToString() != "-1")
                {
                    dt.Rows[vR]["SubLedID"] = hdSubLed.Value;
                    dt.Rows[vR]["SubLed"] = txtSubLed.Text;
                }
                else
                {
                    dt.Rows[vR]["SubLedID"] = "";
                    dt.Rows[vR]["SubLed"] = "";
                }
                if (ddlBranch.SelectedValue.ToString() != "-1")
                {
                    dt.Rows[vR]["CostCBranchCode"] = ddlBranch.SelectedValue.ToString();
                    dt.Rows[vR]["CostCBranch"] = ddlBranch.SelectedItem.Text;
                }
                else
                {
                    dt.Rows[vR]["CostCBranchCode"] = "";
                    dt.Rows[vR]["CostCBranch"] = "";
                }


                if (ddlDrCr.SelectedValue == "D")
                {
                    dt.Rows[vR]["Debit"] = txtAmount.Text;
                    dt.Rows[vR]["Credit"] = "0";
                    dt.Rows[vR]["DC"] = "D";
                }
                else
                {
                    dt.Rows[vR]["Debit"] = "0";
                    dt.Rows[vR]["Credit"] = txtAmount.Text;
                    dt.Rows[vR]["DC"] = "C";
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["DescId"]) == hdCBank.Value)
                        dt.Rows.RemoveAt(i);
                    else
                    {
                        vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                        vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                    }
                }
                dr = dt.NewRow();
                dr["DescId"] = hdCBank.Value;
                dr["Desc"] = txtCBank.Text;
                if (vDrAmt - vCrAmt < 0)
                {
                    dr["Debit"] = vCrAmt - vDrAmt;
                    dr["Credit"] = "0";
                    dr["DC"] = "D";
                }
                else
                {
                    dr["Debit"] = "0";
                    dr["Credit"] = vDrAmt - vCrAmt;
                    dr["DC"] = "C";
                }
                dt.Rows.Add(dr);
            }
            else if (hdEdit.Value != "-1" && EditMode == "C_B")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["DescId"] = hdCBank.Value;
                dt.Rows[vR]["Desc"] = txtCBank.Text;

                dt.Rows[vR]["SubLedID"] = "";
                dt.Rows[vR]["SubLed"] = "";

                dt.Rows[vR]["CostCBranchCode"] = "";
                dt.Rows[vR]["CostCBranch"] = "";

                if (vChkCashBank == "C")
                {
                    txtChequeNo.Text = "";
                    txtBankName.Text = "";
                    txtChequeDt.Text = "";
                    txtPaidTo.Text = "";
                    txtChequeNo.Enabled = false;
                    txtBankName.Enabled = false;
                    txtChequeDt.Enabled = false;
                    txtPaidTo.Enabled = false;
                }
                if (vChkCashBank == "B")
                {
                    txtChequeNo.Enabled = true;
                    txtBankName.Enabled = true;
                    txtChequeDt.Enabled = true;
                    txtPaidTo.Enabled = true;
                }
                if (ddlDrCr.SelectedValue == "D")
                {
                    dt.Rows[vR]["Debit"] = txtAmount.Text;
                    dt.Rows[vR]["Credit"] = "0";
                    dt.Rows[vR]["DC"] = "D";
                }
                else
                {
                    dt.Rows[vR]["Debit"] = "0";
                    dt.Rows[vR]["Credit"] = txtAmount.Text;
                    dt.Rows[vR]["DC"] = "C";
                }
            }
            dt.AcceptChanges();
            ViewState["VouDtl"] = dt;
            gvVoucherDtl.DataSource = dt;
            gvVoucherDtl.DataBind();
            //string vChkCashBank = Convert.ToString(ViewState["ChkCashBank"]);
            if (hbCb.Value != "C")
                txtBankName.Text = txtCBank.Text;// ddlCashBank.SelectedItem.Text;
            else
                txtBankName.Text = "";
            foreach (DataRow Tdr in dt.Rows)
            {
                vTotDr += Convert.ToDouble(Tdr["Debit"]);
                vTotCr += Convert.ToDouble(Tdr["Credit"]);
            }
            txtDrTot.Text = vTotDr.ToString();
            txtCrTot.Text = vTotCr.ToString();
            //ddlCashBank.Enabled = false;
            txtCBank.Enabled = false;
            ddlRecpPay.Enabled = false;
            hdEdit.Value = "-1";
            gvVoucherDtl.Enabled = true;
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
        /// 
        private string GetPIDCODE(string vIdcode)
        {
            CVoucher oVoucher = null;
            oVoucher = new CVoucher();
            return "";
            // return oVoucher.GetPIDCODE(vIdcode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vRptPath = "";
            string pHeadId;
            string vCheckedBy = "";
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime VLgDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                oVoucher = new CVoucher();
                pHeadId = Convert.ToString(ViewState["HeadId"]);
                dt = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode);
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    if (dt.Rows[0]["VoucherType"].ToString() == "P")
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherPayment.rpt";
                    }
                    else
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherPayment.rpt";
                        //vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherReceipt.rpt";
                    }
                    //if (dt.Rows[0]["CompanyName"].ToString() == "HONDA MOTOR WORLD INC." || dt.Rows[0]["CompanyName"].ToString() == "HMW LENDING INVESTORS INC." || dt.Rows[0]["CompanyName"].ToString() == "MAPI LENDING INVESTORS INC.")
                    //    vCheckedBy = Convert.ToString("GVARGAS");
                    //if (dt.Rows[0]["CompanyName"].ToString() == "MOTOR ACE PHILIPPINES INC.")
                    //    vCheckedBy = Convert.ToString("PPEPITO");                
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", "");
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());

                    //rptDoc.SetParameterValue("pCheckedBy", vCheckedBy.ToString());

                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "Check_Voucher");
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            this.GetModuleByRole(mnuID.mnuLoanRecovery);
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            Boolean vResult = false;
            string vXmlData = "", vVouNo = "", vTranType = "", vHeadID = "", vChkAutoRev = "";
            Int32 vCompanyId, vErr = 0, i = 0;
            string vVoucherEdit = Convert.ToString(ViewState["VoucherEdit"]);
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string BrCode = Convert.ToString(ViewState["BrCode"]);
            //vCompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
            DataTable dtLedger = null;
            CPettyReplenish oPR = null;
            string vCashorBank = hbCb.Value;
            string vPettyCashId = Convert.ToString(ViewState["PettyCashId"]);
            string vAcVouPostYN = "N";
            CVoucher oBJV = new CVoucher();
            try
            {
                if (Mode == "Save")
                {
                    if (vCashorBank == "C")
                        vTranType = "C";
                    else
                        vTranType = "B";
                }
                if (Mode == "Edit")
                {
                    if ((vCashorBank == "C") && hdCBank.Value != "-1")
                        vTranType = "C";
                    else if ((vCashorBank != "C") && hdCBank.Value != "-1")
                        vTranType = "B";
                }
                dtLedger = (DataTable)ViewState["VouDtl"];
                if (dtLedger == null || dtLedger.Rows.Count <= 0)
                {
                    gblFuction.AjxMsgPopup("Please Enter At Least One Entry.");
                    return false;
                }
                foreach (DataRow Tdr in dtLedger.Rows)
                {
                    i = i + 1;
                    Tdr["DtlId"] = i;
                    Tdr["Amt"] = Convert.ToDouble(Tdr["Debit"]) + Convert.ToDouble(Tdr["Credit"]);
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dtLedger.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }

                vAcVouPostYN = (chkVouPostYN.Checked == true ? "Y" : "N");

                if (Mode == "Save")
                {
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1) //&& this.RoleId != 33 && this.RoleId != 34 //CENTR - 7554
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtVoucherDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You cannot save, Day end already done..");
                                return false;
                            }
                        }
                    }
                    double vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                            , vLoginDt, BrCode, Convert.ToInt32(Session[gblValue.FinYrNo]), "P0001");

                    if (Math.Round(Convert.ToDouble(txtDrTot.Text), 2) > vAmt)
                    {
                        gblFuction.AjxMsgPopup("Insufficient Balance.");
                        return false;
                    }
                    oPR = new CPettyReplenish();
                    vErr = oPR.InsertVoucherPettyCashReplenish(ref vHeadID, ref vVouNo, Session[gblValue.ACVouMst].ToString(),
                        Session[gblValue.ACVouDtl].ToString(), gblFuction.setDate(txtVoucherDt.Text),
                        ddlRecpPay.SelectedValue, "PR", "", txtNarration.Text, gblFuction.setDate(txtChequeDt.Text),
                        txtChequeNo.Text, txtBankName.Text, vTranType, vFinFromDt, vFinToDt, vXmlData, vFinYear,
                        //"I", Convert.ToString(ddlBr.SelectedValue), Convert.ToInt32(Session[gblValue.UserId]), 0, txtPaidTo.Text, Convert.ToDouble(txtDrTot.Text)
                        //"I", BrCode, Convert.ToInt32(Session[gblValue.UserId]), 0, txtPaidTo.Text, Convert.ToDouble(txtDrTot.Text),ddlRVF.SelectedValue);
                        "I", BrCode, Convert.ToInt32(Session[gblValue.UserId]), 0, txtPaidTo.Text, Convert.ToDouble(txtDrTot.Text), vPettyCashId, vAcVouPostYN);

                    if (vErr == 0)
                    {
                        ViewState["HeadId"] = vHeadID;
                        ViewState["VouDtl"] = dtLedger;
                        txtVoucherNo.Text = vVouNo;
                        vResult = true;
                        //tabPurps.ActiveTabIndex = 2;
                        //GetLedGrid(Convert.ToInt32(0));
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    //if (ValidateFields(Mode) == false)
                    //    return false;

                    //oPR = new CPettyReplenish();
                    //vErr = oPR.UpdateVoucherPettyCashReplenish(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                    //   Convert.ToString(ViewState["HeadId"]), txtVoucherNo.Text, gblFuction.setDate(txtVoucherDt.Text),
                    //   ddlRecpPay.SelectedValue, "PR", Convert.ToString(ViewState["RFRepId"]), txtNarration.Text, gblFuction.setDate(txtChequeDt.Text),
                    //   txtChequeNo.Text, txtBankName.Text, vTranType, vXmlData, "E",
                    //   //Convert.ToString(ddlBr.SelectedValue), this.UserID, 0, txtPaidTo.Text, vFinYear, Convert.ToDouble(txtDrTot.Text));
                    //   BrCode, this.UserID, 0, txtPaidTo.Text, vFinYear, Convert.ToDouble(txtDrTot.Text));
                    //if (vErr == 0)
                    //{
                    //    gblFuction.MsgPopup(gblMarg.EditMsg);
                    //    ViewState["VouDtl"] = dtLedger;
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }
                else if (Mode == "Delete")
                {
                    //oPR = new CPettyReplenish();
                    //vErr = oPR.DeleteVoucherPettyCash(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                    //    Convert.ToString(ViewState["HeadId"]), txtVoucherNo.Text, gblFuction.setDate(txtVoucherDt.Text)
                    //    //,Convert.ToString(ddlBr.SelectedValue), this.UserID, vChkAutoRev, Convert.ToString(ViewState["RFRepId"]));
                    //    ,BrCode, this.UserID, vChkAutoRev, Convert.ToString(ViewState["RFRepId"]));
                    //if (vErr == 0)
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    //    vResult = true;
                    //    ViewState["VouDtl"] = null;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }
                else if (Mode == "SendBack")
                {
                    oPR = new CPettyReplenish();
                    vErr = oPR.PettyCashReplSendBack(vPettyCashId, Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), gblFuction.setDate(txtVoucherDt.Text),
                            vFinFromDt, vFinToDt, vXmlData, vFinYear, "E", BrCode, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(Session[gblValue.LoginDate].ToString()), TxtSendBackRem.Text.Trim());
                    if (vErr == 0)
                    {
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
                dtLedger = null;
                oPR = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateEntry(string pSubYN)
        {
            bool vRst = true;
            if (txtAmount.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Amount Cannot be left blank.");
                vRst = false;
            }
            if (txtAmount.Text.Trim() == "0")
            {
                gblFuction.AjxMsgPopup("Amount Cannot be ZERO.");
                vRst = false;
            }
            if (hdLed.Value == "-1")
            {
                gblFuction.AjxMsgPopup("A/c Ledger Cannot be left blank.");
                vRst = false;
            }
            if (hdCBank.Value == "-1")
            {
                gblFuction.AjxMsgPopup("Cash/Bank Cannot be left blank.");
                vRst = false;
            }
            if (ddlDrCr.SelectedIndex < 0)
            {
                gblFuction.AjxMsgPopup("Dr/Cr Cannot be left blank.");
                vRst = false;
            }
            return vRst;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            DataRow dr;

            try
            {
                if (e.CommandName == "cmdShow")
                {
                    dt = (DataTable)ViewState["VouDtl"];
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (index < gvVoucherDtl.Rows.Count - 1)
                    {
                        gblFuction.AjxMsgPopup("You Cannot Delete These Rows");
                        return;
                    }
                    if (index == gvVoucherDtl.Rows.Count - 1)
                    {
                        if (Convert.ToString(dt.Rows[index]["DescId"]) == hdCBank.Value)
                            dt.Rows.RemoveAt(index);
                        for (int i = 0; i <= dt.Rows.Count; i++)
                        {
                            if (Convert.ToString(dt.Rows[i]["DescId"]) == hdCBank.Value)
                                dt.Rows.RemoveAt(i);
                            else
                            {
                                vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                                vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                            }
                        }
                        dr = dt.NewRow();
                        dr["DescId"] = hdCBank.Value;// ddlCashBank.SelectedValue.ToString();
                        dr["Desc"] = txtCBank.Text;// ddlCashBank.SelectedItem.Text;

                        if (vDrAmt - vCrAmt < 0)
                        {
                            dr["Debit"] = vCrAmt - vDrAmt;
                            dr["Credit"] = "0";
                            dr["DC"] = "D";
                        }
                        else
                        {
                            dr["Debit"] = "0";
                            dr["Credit"] = vDrAmt - vCrAmt;
                            dr["DC"] = "C";
                        }
                        dt.Rows.Add(dr);
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
                    if (index == gvVoucherDtl.Rows.Count - 1)
                    {
                        //ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));

                        hdCBank.Value = dt.Rows[index]["DescId"].ToString();
                        txtCBank.Text = dt.Rows[index]["DescId"].ToString();
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[index]["DC"].ToString()));
                        if (Convert.ToString(dt.Rows[index]["CostCBranchCode"]) != "")
                        {
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[index]["CostCBranchCode"])));
                        }
                        else
                        {
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue("-1"));
                        }
                        if (Convert.ToString(dt.Rows[index]["SubLedID"]) != "")
                        {
                            hdSubLed.Value = Convert.ToString(dt.Rows[index]["SubLedID"]);
                            txtSubLed.Text = Convert.ToString(dt.Rows[index]["SubLed"]);
                        }
                        else
                        {
                            hdSubLed.Value = "-1";
                            txtSubLed.Text = "";
                        }
                        txtLed.Enabled = false;
                        ddlBranch.Enabled = false;
                        txtSubLed.Enabled = false;
                        //ddlLedger.Enabled = false;
                        txtAmount.Enabled = false;
                        ViewState["ClickMode"] = "C_B";
                        //ddlCashBank.Enabled = true;
                        txtCBank.Enabled = true;
                        hdCBank.Value = "-1";
                    }
                    else
                    {
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[index]["DC"].ToString()));
                        //ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));
                        hdLed.Value = dt.Rows[index]["DescId"].ToString();
                        txtLed.Text = dt.Rows[index]["Desc"].ToString();
                        if (Convert.ToString(dt.Rows[index]["CostCBranchCode"]) != "")
                        {
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[index]["CostCBranchCode"])));
                        }
                        else
                        {
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue("-1"));
                        }
                        if (Convert.ToString(dt.Rows[index]["SubLedID"]) != "")
                        {
                            hdSubLed.Value = dt.Rows[index]["SubLedID"].ToString();
                            txtSubLed.Text = dt.Rows[index]["SubLed"].ToString();

                        }
                        else
                        {
                            hdSubLed.Value = "-1";
                            txtSubLed.Text = "";
                        }

                        //ddlCashBank.Enabled = false;
                        txtCBank.Enabled = false;
                        ViewState["ClickMode"] = "G_L";
                        //ddlLedger.Enabled = true;
                        txtLed.Enabled = true;
                        txtSubLed.Enabled = true;
                        ddlBranch.Enabled = true;
                        //hdLed.Value = "";
                        txtAmount.Enabled = true;
                    }
                    if (dt.Rows[index]["DC"].ToString() == "D")
                        txtAmount.Text = dt.Rows[index]["Debit"].ToString();
                    else
                        txtAmount.Text = dt.Rows[index]["Credit"].ToString();
                    hdEdit.Value = index.ToString();
                }
            }
            finally
            {
                dt = null;
            }
        }

        protected void btnShowList_Click(object sender, EventArgs e)
        {
            ClearViewState();
            LoadSearchList(0);
        }
        private void LoadSearchList(int vCompId)
        {
            DataTable dt = null;
            CPettyReplenish oFed = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oFed = new CPettyReplenish();
                dt = oFed.GetPettyCashReplenishList(vCompId, vBrCode, gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text));
                gvReplenishList.DataSource = dt.DefaultView;
                gvReplenishList.DataBind();
            }
            finally
            {
                dt = null;
                oFed = null;
            }
        }
        protected void btnShowPendList_Click(object sender, EventArgs e)
        {
            string vBrCode = ddlOverrideBranch.SelectedValues.Replace("|", ",");
            if (vBrCode == "")
            {
                gblFuction.MsgPopup("Please select a Branch");
                return;
            }
            //if ((ddlCashBank.SelectedValue == "-1")||(Convert.ToString(ddlCashBank.SelectedValue)==""))
            //{
            //    gblFuction.MsgPopup("Please select a Cash / Bank");
            //    return;
            //}
            ClearViewState();
            LoadSearchPendingList(0);
        }
        private void LoadSearchPendingList(int vCompId)
        {
            DataTable dt = null;
            CPettyReplenish oFed = null;
            try
            {
                oFed = new CPettyReplenish();
                string vBrCode = ddlOverrideBranch.SelectedValues.Replace("|", ",");
                dt = oFed.GetPettyCashReplenishPendingList(vCompId, vBrCode, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                gvUnReplenishList.DataSource = dt.DefaultView;
                gvUnReplenishList.DataBind();
            }
            finally
            {
                dt = null;
                oFed = null;
            }
        }
        protected void ddlOverrideBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAcGenLedAll();
        }
        private void LoadAcGenLedAll()
        {
            DataTable dt = new DataTable();
            CVoucher oVoucher = null;
            //string vBranch = Convert.ToString(ddlBr.SelectedValue);
            oVoucher = new CVoucher();
            ddlCashBank.Items.Clear();
            // dt = oVoucher.GetAcGenLedCB_New("000", "CB", "", "");
            //dt = oVoucher.GetAcGenLedCB(vBranch, "Y", "", "");
            dt = oVoucher.GetAcGenLedCB("0000", "Y", "", "");
            if (dt.Rows.Count > 0)
            {
                ddlCashBank.DataSource = dt;
                ddlCashBank.DataTextField = "Desc";
                ddlCashBank.DataValueField = "DescId";
                ddlCashBank.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlCashBank.Items.Insert(0, liSel);
            }
        }
        protected void gvReplenishList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vID = string.Empty, vPettyCashId = string.Empty;
            try
            {
                vID = Convert.ToString(e.CommandArgument);
                vPettyCashId = vID;
                if (e.CommandName == "cmdFileDownload")
                {
                    //path = ConfigurationManager.AppSettings["PettyCashDocPath"];
                    PettyCashFileDownload(vID, path);
                }
            }
            finally
            {
            }
        }
        protected void gvUnReplenishList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vID = "";
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CPettyReplenish oPr = null;
            string vBrCode = "", vPettyCashId = "", vCashBank = "";
            double vTotDr = 0.0;
            double vTotCr = 0.0;
            CPettyReplenish oCra = null;
            ClearControls();
            vCashBank = Convert.ToString(ddlCashBank.SelectedValue);
            hbCb.Value = "B";

            try
            {
                vID = Convert.ToString(e.CommandArgument);
                ViewState["BrCode"] = "";
                ViewState["PettyCashId"] = "";
                ViewState["PettyCashId"] = vID;
                vPettyCashId = vID;

                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvUnReplenishList.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    Label lblBrCode = (Label)gvRow.FindControl("lblBrCode");
                    vBrCode = lblBrCode.Text.Trim();
                    ViewState["BrCode"] = vBrCode;
                    //vBrCode = "0000";
                    oCra = new CPettyReplenish();
                    ds = oCra.GetLedgerGrid(vBrCode, vPettyCashId, vCashBank);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        gvVoucherDtl.DataSource = dt;
                        gvVoucherDtl.DataBind();
                        ViewState["VouDtl"] = dt;
                        foreach (DataRow Tdr in dt.Rows)
                        {
                            vTotDr += Convert.ToDouble(Tdr["Debit"]);
                            vTotCr += Convert.ToDouble(Tdr["Credit"]);
                        }
                        txtDrTot.Text = vTotDr.ToString();
                        txtCrTot.Text = vTotCr.ToString();
                    }
                    else
                    {
                        gvVoucherDtl.DataSource = null;
                        gvVoucherDtl.DataBind();
                        txtAmount.Text = "0";
                        ViewState["VouDtl"] = null;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        txtPaidTo.Text = Convert.ToString(dt1.Rows[0]["PaidTo"]);
                        txtBankName.Text = Convert.ToString(dt1.Rows[0]["BankCash"]);
                        txtNarration.Text = Convert.ToString(dt1.Rows[0]["Narration"]);
                        TxtSendBackRem.Text = Convert.ToString(dt1.Rows[0]["SendBackRem"]);
                    }
                    tabPurps.ActiveTabIndex = 2;
                    StatusButton("Show");
                    AddFunc();
                    btnPCFileDownload.Enabled = true;
                }
                if (e.CommandName == "cmdFileDownload")
                {
                    //path = ConfigurationManager.AppSettings["PettyCashDocPath"];
                    PettyCashFileDownload(vID, path);
                }
            }
            finally
            {
                ds = null;
                oPr = null;
                dt = null;
                dt1 = null;
                oCra = null;
            }
        }
        private void ClearViewState()
        {
            ViewState["RFRepId"] = "";
            ViewState["PettyCashId"] = "";
            ViewState["BrCode"] = "";
        }
        private void AddFunc()
        {
            ViewState["StateEdit"] = "Add";
            ViewState["Edit"] = "DescId";
            StatusButton("Add");
            tabPurps.ActiveTabIndex = 2;
            gvVoucherDtl.Enabled = false;
        }
        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            string vStateEdit = "SendBack";
            if (SaveRecords(vStateEdit) == true)
            {
                //gblFuction.MsgPopup(gblMarg.SaveMsg);
                gblFuction.MsgPopup("Record Send Back Successfully.");
                LoadSearchList(Convert.ToInt32(0));
                LoadSearchPendingList(Convert.ToInt32(0));
                tabPurps.ActiveTabIndex = 1;
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnPCFileDownload_Click(object sender, EventArgs e)
        {
            if (ViewState["PettyCashId"] != null)
            {
                string vID = Convert.ToString(ViewState["PettyCashId"]);
                PettyCashFileDownload(vID, path);
            }
        }

        protected void PettyCashFileDownload(string pId, string pPath)
        {
            try
            {
                string vDirPath = pPath;
                string vFilePath = "", vFileName = string.Empty;
                string vFileNameWithoutExt = string.Empty, vFileExt = string.Empty;
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
            }
            // }
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

    }
}
