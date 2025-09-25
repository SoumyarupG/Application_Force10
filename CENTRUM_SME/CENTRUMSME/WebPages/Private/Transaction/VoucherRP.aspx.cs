using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class VoucherRP : CENTRUMBAse
    {
        protected int vPgNo = 1;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                ViewState["VoucherEdit"] = null;
                ViewState["VouDtl"] = null;
                ViewState["ClickMode"] = "Load";
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtChequeNo.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadGrid(1);
                tabAcHd.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Receipt & Payment Voucher";                
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuRecPay);
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
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnDelete.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Receipt & Payment Voucher", false);
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
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    gblFuction.focus("ctl00_cph_Main_ddlRecpPay");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnPrint.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtFromDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("From Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtFromDt");
                vResult = false;
            }
            if (txtFromDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtFromDt.Text) == false)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtFromDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("To Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
            }
            if (txtToDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    EnableControl(true);
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            return vResult;
        }
        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            double DrAmt = 0.00, CrAmt = 0.00;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string vBankID = hdCBank.Value;
            if (txtVoucherDt.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (txtVoucherDt.Text.Trim() != "")
            {
                //if (gblFuction.setDate(txtVoucherDt.Text) > vLoginDt)
                //{
                //    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                //    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                //    vResult = false;
                //}
                if (gblFuction.setDate(txtVoucherDt.Text) < vFinFromDt || gblFuction.setDate(txtVoucherDt.Text) > vFinToDt)
                {
                    gblFuction.AjxMsgPopup("Voucher Date should be within Logged In financial year.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                    vResult = false;
                }
            }
            if (ddlRecpPay.SelectedIndex == -1)
            {
                gblFuction.AjxMsgPopup("Voucher Type Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlRecpPay");
                vResult = false;
            }
            if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[3].Text == "0.00" && ddlRecpPay.SelectedValue == "P")
            {
                gblFuction.AjxMsgPopup("Invalid Payment Voucher.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vResult = false;
            }
            if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[2].Text == "0.00" && ddlRecpPay.SelectedValue == "R")
            {
                gblFuction.AjxMsgPopup("Invalid Receipt Voucher.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vResult = false;
            }
            if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[3].Text == "0" && ddlRecpPay.SelectedValue == "P") //for edit mode
            {
                gblFuction.AjxMsgPopup("Invalid Payment Voucher.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vResult = false;
            }
            if (gvVoucherDtl.Rows[gvVoucherDtl.Rows.Count - 1].Cells[2].Text == "0" && ddlRecpPay.SelectedValue == "R") //for edit mode
            {
                gblFuction.AjxMsgPopup("Invalid Receipt Voucher.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vResult = false;
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
                    DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvVoucherDtl.Rows[i].Cells[2].Text), 2);
                    CrAmt = Math.Round(CrAmt, 2) + Math.Round(Convert.ToDouble(gvVoucherDtl.Rows[i].Cells[3].Text), 2);
                }
            }
            if (DrAmt == 0 && CrAmt == 0)
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Cannot Be ZERO.");
                vResult = false;
            }
            if (Math.Round(CrAmt, 2) != Math.Round(DrAmt, 2))
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Should Be Equal.");
                vResult = false;
            }
            if (txtNarration.Text.Trim() == "")
            {

                gblFuction.AjxMsgPopup("Narration can not left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            if (vBankID != "C0001" && pMode == "Save")
            {
                if (txtChequeNo.Text.Trim() == "")
                {

                    gblFuction.AjxMsgPopup("Please Enter the cheque No...");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeNo");
                    vResult = false;
                }
                if (txtChequeDt.Text.Trim() == "")
                {

                    gblFuction.AjxMsgPopup("Please Enter the cheque Date...");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
                    vResult = false;
                }

                if (txtChequeDt.Text.Trim() != "")
                {
                    if (gblFuction.IsDate(txtChequeDt.Text) == false)
                    {

                        gblFuction.AjxMsgPopup("Please Enter Valid Cheque Date.");
                        gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
                        vResult = false;
                    }
                }
            }
            else if (pMode == "Edit")
            {
                if (txtBankName.Text.Trim() != "")
                {
                    if (txtChequeNo.Text.Trim() == "")
                    {

                        gblFuction.AjxMsgPopup("Please Enter the cheque No...");
                        gblFuction.AjxFocus("ctl00_cph_Main_txtChequeNo");
                        vResult = false;
                    }
                    if (txtChequeDt.Text.Trim() == "")
                    {

                        gblFuction.AjxMsgPopup("Please Enter the cheque Date...");
                        gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
                        vResult = false;
                    }

                    if (txtChequeDt.Text.Trim() != "")
                    {
                        if (gblFuction.IsDate(txtChequeDt.Text) == false)
                        {

                            gblFuction.AjxMsgPopup("Please Enter Valid Cheque Date.");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtChequeDt");
                            vResult = false;
                        }
                    }
                }
            }

            return vResult;
        }
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vR = 0;
            CVoucher oVoucher = null;
            try
            {
                if (ValidateFields() == true)
                {
                    string vAcMst = Session[gblValue.ACVouMst].ToString();
                    string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    string vDtFrom = gblFuction.setStrDate(txtFromDt.Text);
                    string vDtTo = gblFuction.setStrDate(txtToDt.Text);
                    string vSearch = txtSearch.Text;
                    oVoucher = new CVoucher();
                    dt = oVoucher.GetVoucherlist(vAcMst, vAcDtl, vBrCode, vDtFrom, vDtTo, vSearch, "A", pPgIndx, ref vR); //VoucherTyp=R/P 
                    gvVouvher.DataSource = dt.DefaultView;
                    gvVouvher.DataBind();
                    if (dt.Rows.Count <= 0)
                    {
                        lblTotPg.Text = "0";
                        lblCrPg.Text = "0";
                    }
                    else
                    {
                        lblTotPg.Text = getTotPages(vR).ToString();
                        lblCrPg.Text = vPgNo.ToString();
                    }
                    if (vPgNo == 1)
                    {
                        btnPrv.Enabled = false;
                        if (Int32.Parse(lblTotPg.Text) > 0 && vPgNo != Int32.Parse(lblTotPg.Text))
                            btnNxt.Enabled = true;
                        else
                            btnNxt.Enabled = false;
                    }
                    else
                    {
                        btnPrv.Enabled = true;
                        if (vPgNo == Int32.Parse(lblTotPg.Text))
                            btnNxt.Enabled = false;
                        else
                            btnNxt.Enabled = true;
                    }
                }
            }            
            finally
            {
                oVoucher = null;
                dt.Dispose();
            }
        }
        private int getTotPages(double pRows)
        {
            int vTPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vTPg;
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
        }
        protected void gvVouvher_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            double vTotDr = 0, vTotCr = 0;
            string vHeadId = "";
            CVoucher oVoucher = null;
            DataTable dtAc = null;
            try
            {              

                if (e.CommandName == "cmdDtl")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnDtl = (LinkButton)gvRow.FindControl("btnDtl");
                    //LinkButton btnPrint = (LinkButton)gvRow.FindControl("btnPrint");

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvVouvher.Rows)
                    {
                        if ((gr.RowIndex) % 2 == 0)
                        {
                            gr.BackColor = backColor;
                            gr.ForeColor = foreColor;
                        }
                        else
                        {
                            gr.BackColor = System.Drawing.Color.White;
                            gr.ForeColor = foreColor;
                        }
                        gr.Font.Bold = false;
                    }
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnDtl.ForeColor = System.Drawing.Color.White;
                    btnDtl.Font.Bold = true;
                    //btnPrint.ForeColor = System.Drawing.Color.White;
                    //btnPrint.Font.Bold = true;

                    vHeadId = Convert.ToString(e.CommandArgument);
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    ViewState["HeadId"] = vHeadId;

                    oVoucher = new CVoucher();
                    ViewState["VouDtl"] = null;
                    dtAc = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), vHeadId, vBrCode);
                    ViewState["VouDtl"] = dtAc;
                    if (dtAc.Rows.Count > 0)
                    {
                        txtVoucherNo.Text = Convert.ToString(dtAc.Rows[0]["VoucherNo"]);
                        txtVoucherDt.Text = gblFuction.getStrDate(Convert.ToString(dtAc.Rows[0]["VoucherDt"]));
                        ddlRecpPay.SelectedIndex = ddlRecpPay.Items.IndexOf(ddlRecpPay.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["VoucherType"])));
                        ddlDrCr.SelectedIndex = -1;
                        if (ddlRecpPay.Text == "P".Trim())
                        {
                            ddlDrCr.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlDrCr.SelectedIndex = 1;
                        }
                        //ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["DescId"])));
                        hdLed.Value = Convert.ToString(dtAc.Rows[0]["DescId"]);
                        txtLed.Text = Convert.ToString(dtAc.Rows[0]["Desc"]);


                        txtAmount.Text = Convert.ToString(dtAc.Rows[0]["Amt"]);
                        //ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByValue(Convert.ToString(dtAc.Rows[dtAc.Rows.Count - 1]["DescId"])));
                        hdCBank.Value = Convert.ToString(dtAc.Rows[dtAc.Rows.Count - 1]["DescId"]);
                        txtCBank.Text = Convert.ToString(dtAc.Rows[dtAc.Rows.Count - 1]["Desc"]);

                        if (hdCBank.Value == "C0001")
                        {
                            txtBankName.Enabled = false;
                            txtChequeNo.Enabled = false;
                            txtChequeDt.Enabled = false;
                        }
                        else
                        {
                            txtBankName.Enabled = true;
                            txtChequeNo.Enabled = true;
                            txtChequeDt.Enabled = true;
                        }
                        if (gblFuction.getStrDate(Convert.ToString(dtAc.Rows[0]["ChequeDt"])) == "01/01/2000")
                            txtChequeDt.Text = "";
                        else
                            txtChequeDt.Text = gblFuction.getStrDate(Convert.ToString(dtAc.Rows[0]["ChequeDt"]));
                        txtChequeNo.Text = Convert.ToString(dtAc.Rows[0]["ChequeNo"]);
                        txtBankName.Text = Convert.ToString(dtAc.Rows[0]["Bank"]);
                        txtNarration.Text = Convert.ToString(dtAc.Rows[0]["Narration"]);
                        btnApply.Enabled = false;
                        gvVoucherDtl.DataSource = dtAc.DefaultView;
                        gvVoucherDtl.DataBind();
                        foreach (DataRow Tdr in dtAc.Rows)
                        {
                            vTotDr += Convert.ToDouble(Tdr["Debit"]);
                            vTotCr += Convert.ToDouble(Tdr["Credit"]);
                        }
                        txtDrTot.Text = vTotDr.ToString();
                        txtCrTot.Text = vTotCr.ToString();
                        StatusButton("Show");
                        tabAcHd.ActiveTabIndex = 1;
                    }
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oVoucher = null;
                dtAc = null;
            }
        }
        protected void btnShow_Click(object sender, System.EventArgs e)
        {
            LoadGrid(1);
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx", false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
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
                String vBankID = hdCBank.Value;
                string EditMode = string.Empty;
                string vSubYN = string.Empty;
                EditMode = ViewState["ClickMode"].ToString();
                if (txtSubLedger.Text.ToString().Length > 0)
                    vSubYN = "Y";
                else
                    vSubYN = "N";
                if (ValidateEntry(vSubYN) == false) return;

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

                    gblFuction.AjxMsgPopup("Invalid Amount...");
                    gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
                    return;
                }
                if (ViewState["VouDtl"] != null && hdEdit.Value == "-1")
                {
                    dr = dt.NewRow();

                    dr["DescId"] = hdLed.Value;
                    if (vSubYN == "Y")
                    {
                        dr["SubsidiaryId"] = hdSubLedger.Value;
                        dr["Desc"] = txtLed.Text;
                        dr["SubDesc"] = txtSubLedger.Text;
                    }
                    else
                    {
                        dr["SubsidiaryId"] = "-1";
                        dr["Desc"] = txtLed.Text;
                        dr["SubDesc"] = "";
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
                        if (Convert.ToString(dt.Rows[i]["DescId"]) == vBankID)
                            dt.Rows.RemoveAt(i);
                        vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                        vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                    }
                    dr = dt.NewRow();
                    dr["DescId"] = vBankID;

                    dr["Desc"] = txtCBank.Text.Trim();
                    dr["SubsidiaryId"] = "-1";
                    dr["SubDesc"] = "";
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
                    DataColumn dc = new DataColumn();
                    dc.ColumnName = "DC";
                    dt.Columns.Add(dc);
                    DataColumn dc1 = new DataColumn();
                    dc1.ColumnName = "Debit";
                    dc1.DataType = System.Type.GetType("System.Decimal");
                    dt.Columns.Add(dc1);
                    DataColumn dc2 = new DataColumn();
                    dc2.ColumnName = "Credit";
                    dc2.DataType = System.Type.GetType("System.Decimal");
                    dt.Columns.Add(dc2);
                    DataColumn dc3 = new DataColumn();
                    dc3.ColumnName = "DescId";
                    dt.Columns.Add(dc3);
                    DataColumn dc4 = new DataColumn();
                    dc4.ColumnName = "Desc";
                    dt.Columns.Add(dc4);
                    dr = dt.NewRow();
                    DataColumn dc5 = new DataColumn();
                    dc5.ColumnName = "DtlId";
                    dt.Columns.Add(dc5);
                    DataColumn dc13 = new DataColumn();
                    dc13.ColumnName = "SubsidiaryId";
                    dt.Columns.Add(dc13);
                    DataColumn dc14 = new DataColumn();
                    dc14.ColumnName = "SubDesc";
                    dt.Columns.Add(dc14);
                    dr = dt.NewRow();
                    DataColumn dc6 = new DataColumn();
                    dc6.ColumnName = "Amt";
                    dt.Columns.Add(dc6);
                    dr = dt.NewRow();

                    dr["DescId"] = hdLed.Value;
                    if (hdSubLedger.Value.ToString().Trim() != "") //;vSubYN == "Y"
                    {
                        dr["SubsidiaryId"] = hdSubLedger.Value;
                        dr["Desc"] = txtLed.Text;
                        dr["SubDesc"] = txtSubLedger.Text;
                    }
                    else
                    {
                        dr["SubsidiaryId"] = "-1";
                        dr["Desc"] = txtLed.Text.Trim();
                        dr["SubDesc"] = "";
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
                    dr["Desc"] = txtCBank.Text.Trim();
                    dr["SubsidiaryId"] = "-1";
                    dr["SubDesc"] = "";
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
                    if (vSubYN == "Y")
                    {
                        dt.Rows[vR]["SubsidiaryId"] = hdSubLedger.Value;
                        dt.Rows[vR]["Desc"] = txtLed.Text;
                        dt.Rows[vR]["SubDesc"] = txtSubLedger.Text;
                    }
                    else
                    {
                        dt.Rows[vR]["SubsidiaryId"] = "-1";
                        dt.Rows[vR]["Desc"] = txtLed.Text;
                        dt.Rows[vR]["SubDesc"] = "";
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
                    dr["Desc"] = txtCBank.Text.Trim();
                    dr["SubsidiaryId"] = "-1";
                    dr["SubDesc"] = "";

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
                    dt.Rows[vR]["Desc"] = txtCBank.Text.Trim();
                    dt.Rows[vR]["SubsidiaryId"] = "N";
                    if (hdCBank.Value == "C0001")
                    {
                        txtChequeNo.Text = "";
                        txtBankName.Text = "";
                        txtChequeDt.Text = "";
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
                if (hdCBank.Value != "C0001")
                    txtBankName.Text = txtCB.Text;
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
            }
            finally
            {
            }
        }
        private bool ValidateEntry(string pSubYN)
        {
            bool vRst = true;

            if (txtAmount.Text.Trim() == "")
            {

                gblFuction.AjxMsgPopup("Amount Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (txtAmount.Text.Trim() == "0")
            {

                gblFuction.AjxMsgPopup("Amount Cannot be ZERO.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (hdLed.Value == "-1")
            {

                gblFuction.AjxMsgPopup("A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlLedger");
                vRst = false;
            }
            if (hdSubLedger.Value == "-1" && pSubYN == "Y")
            {

                gblFuction.AjxMsgPopup("Subsidiary A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlSubLedger");
                vRst = false;
            }
            if (hdCBank.Value == "-1")
            {

                gblFuction.AjxMsgPopup("Cash/Bank Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlCashBank");
                vRst = false;
            }
            if (ddlDrCr.SelectedIndex < 0)
            {

                gblFuction.AjxMsgPopup("Dr/Cr Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                vRst = false;
            }

            return vRst;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanAdd == "N")
                {
                    ViewState["VouDtl"] = null;
                    ViewState["HeadId"] = null;
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                tabAcHd.ActiveTabIndex = 1;
                ViewState["VouDtl"] = null;
                ViewState["HeadId"] = null;
                StatusButton("Add");
                ClearControls();
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
                //if (this.RoleId != 1 && this.RoleId != 2 && this.RoleId != 4 && this.RoleId != 16)
                //{
                //    if (Session[gblValue.EndDate] != null)
                //    {
                //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                //        {
                //            gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                //            return;
                //        }
                //    }
                //}

                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }

                ViewState["VoucherEdit"] = "Edit";
                StatusButton("Edit");
                txtVoucherDt.Enabled = true;

                btnApply.Enabled = true;
                if (hdCBank.Value == "C0001")
                {
                    txtBankName.Enabled = false;
                    txtChequeNo.Enabled = false;
                    txtChequeDt.Enabled = false;
                }
                else
                {
                    txtBankName.Enabled = true;
                    txtChequeNo.Enabled = true;
                    txtChequeDt.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vVoucherEdit = Convert.ToString(ViewState["VoucherEdit"]);
            if (vVoucherEdit == "" || vVoucherEdit == null)
                vVoucherEdit = "Save";
            if (SaveRecords(vVoucherEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("View");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.RoleId != 1 && this.RoleId != 2 && this.RoleId != 16)
                //{
                //    if (Session[gblValue.EndDate] != null)
                //    {
                //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                //        {
                //            gblFuction.AjxMsgPopup("You can not delete, Day end already done..");
                //            return;
                //        }
                //    }
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    StatusButton("Delete");
                    ClearControls();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            if (Convert.ToInt32(Session[gblValue.RoleId]) != 1) //&& this.RoleId != 2
            {
                if (Session[gblValue.EndDate] != null)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text.ToString()))
                    {
                        gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        return false;
                    }
                }
            }
            string vFinYear = Session[gblValue.ShortYear].ToString();
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            Boolean vResult = false;
            string vXmlData = "", vVouNo = "", vTranType = "", vHeadID = "";
            Int32 vErr = 0, i = 0;
            string vVoucherEdit = Convert.ToString(ViewState["VoucherEdit"]);
            Int32 vFinYr = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dtLedger = null;
            CVoucher oVoucher = null;
            string vBankID = hdCBank.Value;
            try
            {
                if (Mode == "Save")
                {
                    if (vBankID == "C0001")
                        vTranType = "C";
                    else if (vBankID != "C0001" && vBankID != "-1")
                        vTranType = "B";
                }
                if (Mode == "Edit")
                {
                    if (txtBankName.Text.Trim() == "")
                        vTranType = "C";
                    else if (vBankID != "C0001" && vBankID != "-1")
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
                using (System.IO.StringWriter oSW = new System.IO.StringWriter())
                {
                    dtLedger.WriteXml(oSW);
                    vXmlData = oSW.ToString();
                }
                if (Mode == "Save")
                {
                    if (ValidateFields(Mode) == false)
                        return false;
                    //if (this.RoleId != 1 && this.RoleId != 2 && this.RoleId != 4 && this.RoleId != 16)
                    //{
                    //    if (Session[gblValue.EndDate] != null)
                    //    {
                    //        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                    //        {
                    //            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                    //            return false;
                    //        }
                    //    }
                    //}

                    oVoucher = new CVoucher();
                    vErr = oVoucher.InsertVoucher(ref vHeadID, ref vVouNo, Session[gblValue.ACVouMst].ToString(),
                        Session[gblValue.ACVouDtl].ToString(), gblFuction.setDate(txtVoucherDt.Text),
                        ddlRecpPay.SelectedValue, "", "", txtNarration.Text, gblFuction.setDate(txtChequeDt.Text),
                        txtChequeNo.Text, txtBankName.Text, vTranType, vFinFromDt, vFinToDt, vXmlData, vFinYear,
                        "I", Session[gblValue.BrnchCode].ToString(), this.UserID, 0);

                    if (vErr == 0)
                    {
                        ViewState["HeadId"] = vHeadID;
                        ViewState["VouDtl"] = dtLedger;
                        txtVoucherNo.Text = vVouNo;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    if (ValidateFields(Mode) == false)
                        return false;

                    oVoucher = new CVoucher();
                    vErr = oVoucher.UpdateVoucher(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                       Convert.ToString(ViewState["HeadId"]), txtVoucherNo.Text, gblFuction.setDate(txtVoucherDt.Text),
                       ddlRecpPay.SelectedValue, "", "", txtNarration.Text, gblFuction.setDate(txtChequeDt.Text),
                       txtChequeNo.Text, txtBankName.Text, vTranType, vXmlData, "E",
                       Session[gblValue.BrnchCode].ToString(), this.UserID, 0);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        ViewState["VouDtl"] = dtLedger;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }

                }
                else if (Mode == "Delete")
                {
                    oVoucher = new CVoucher();
                    vErr = oVoucher.DeleteVoucher(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                        Convert.ToString(ViewState["HeadId"]), Session[gblValue.BrnchCode].ToString(), this.UserID);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        vResult = true;
                        ViewState["VouDtl"] = null;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                LoadGrid(vPgNo);
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtLedger = null;
                oVoucher = null;
            }
        }
        private void LoadAcGenLed()
        {
        }
        private void LoadSubAcGenLed()
        {
        }
        private void GetCashBank()
        {
            //DataTable dt = null;
            //CVoucher oVoucher = null;
            //try
            //{
            //    string vBrCode = Session[gblValue.BrnchCode].ToString();
            //    oVoucher = new CVoucher();
            //    dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
            //    ddlCashBank.DataSource = dt;
            //    ddlCashBank.DataTextField = "Desc";
            //    ddlCashBank.DataValueField = "DescId";
            //    ddlCashBank.DataBind();
            //    ListItem Li = new ListItem("<-- Select -->", "-1");
            //    ddlCashBank.Items.Insert(0, Li);
            //}
            //finally
            //{
            //    oVoucher = null;
            //    dt.Dispose();
            //}
        }
        private void EnableControl(Boolean Status)
        {
            txtVoucherNo.Enabled = false;
            txtVoucherDt.Enabled = Status;
            txtBankName.Enabled = Status;
            ddlRecpPay.Enabled = Status;
            //ddlCashBank.Enabled = Status;
            txtCBank.Enabled = Status;
            ddlDrCr.Enabled = Status;
            //ddlLedger.Enabled = Status;
            txtLed.Enabled = Status;
            //ddlSubLedger.Enabled = Status;
            txtSubLedger.Enabled = Status;
            txtAmount.Enabled = Status;
            txtChequeNo.Enabled = Status;
            txtChequeDt.Enabled = Status;
            txtNarration.Enabled = Status;
            txtDrTot.Enabled = Status;
            txtCrTot.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
            btnApply.Enabled = Status;
        }
        private void ClearControls()
        {
            txtVoucherNo.Text = "";
            ddlRecpPay.SelectedIndex = 0;
            //ddlCashBank.SelectedIndex = 0;
            txtCBank.Text = "";
            hdCBank.Value = "-1";
            ddlDrCr.SelectedIndex = 0;
            //ddlLedger.SelectedIndex = -1;
            hdLed.Value = "-1";
            txtLed.Text = "";
            txtAmount.Text = "0";
            txtChequeNo.Text = "";
            txtChequeDt.Text = "";
            txtNarration.Text = "";
            txtCrTot.Text = "0";
            txtDrTot.Text = "0";
            gvVoucherDtl.DataSource = null;
            gvVoucherDtl.DataBind();
        }
        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            string vSubYN = string.Empty;
            DataRow dr;
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    dt = (DataTable)ViewState["VouDtl"];
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (index == gvVoucherDtl.Rows.Count - 1)
                    {
                        gblFuction.AjxMsgPopup("You Cannot Delete Cash/Bank....Only Chnage is Allowed");
                        return;
                    }
                    if (index != gvVoucherDtl.Rows.Count)
                    {
                        if (Convert.ToString(dt.Rows[index]["DescId"]) != hdCBank.Value)
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
                        dr["DescId"] = hdCBank.Value;
                        dr["Desc"] = txtCBank.Text.Trim();
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
                        //ddlLedger.Enabled = false;
                        txtLed.Enabled = false;
                        //ddlSubLedger.Enabled = false;
                        txtSubLedger.Enabled = false;
                        txtAmount.Enabled = false;
                        ViewState["ClickMode"] = "C_B";
                        //ddlCashBank.Enabled = true;
                        txtCBank.Enabled = true;
                    }
                    else
                    {
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(dt.Rows[index]["DC"].ToString()));
                        //ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));
                        hdLed.Value = dt.Rows[index]["DescId"].ToString();


                        //ddlCashBank.Enabled = false;
                        txtCBank.Enabled = false;
                        ViewState["ClickMode"] = "G_L";
                        //ddlLedger.Enabled = true;
                        txtLed.Enabled = true;

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
                dt.Dispose();
            }
        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vRptPath = "";

            string pHeadId;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
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
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherReceipt.rpt";
                    }
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Payment_Voucher");
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                oVoucher = null;
            }
        }
    }
}



