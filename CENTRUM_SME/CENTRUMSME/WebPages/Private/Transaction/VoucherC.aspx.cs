using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class VoucherC : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                InitBasePage();
                txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                if (txtFromDt.Text == "" || txtToDt.Text == "")
                {
                    txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                    txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                }
                ViewState["VoucherEdit"] = null;
                ViewState["VouDtl"] = null;
                ViewState["ClickMode"] = "Load";
                StatusButton("View");
                GetCashBank();
                LoadGrid(1);
                tabAcHd.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Contra Voucher";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuContra);
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
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnEdit.Visible = true;
                    btnDelete.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Contra Voucher", false);
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
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
                    gblFuction.focus("ctl00_cph_Main_ddlRecpPay");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            EnableControl(false);
            hdEdit.Value = "-1";
            StatusButton("View");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RoleId != 1 && this.RoleId != 2)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
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
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RoleId != 1 && this.RoleId != 2 && this.RoleId != 4 && this.RoleId != 16)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                            return;
                        }
                    }
                }
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["VoucherEdit"] = "Edit";
                StatusButton("Edit");
                txtVoucherDt.Enabled = true;
                btnApply.Enabled = true;
                //ddlCashBank.Enabled = false;
                if (ddlCashBank.SelectedValue == "C0001")
                {
                }
                else
                {
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
                EnableControl(false);
                StatusButton("View");
                ClearControls();
            }
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

                        ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["DescId"])));
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["DC"]))); ;
                        txtAmount.Text = Convert.ToString(dtAc.Rows[0]["Amt"]);
                        if (ddlCashBank.SelectedValue == "C0001")
                        { }
                        else
                        { }

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
                ceDateC.Enabled = true;
                EnableControl(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int getTotPages(double pRows)
        {
            int vTPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vTPg;
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
                    dt = oVoucher.GetVoucherlist(vAcMst, vAcDtl, vBrCode, vDtFrom, vDtTo, vSearch, "N", pPgIndx, ref vR); //VoucherTyp=R/P 
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
                dt = null;
            }
        }
        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            double DrAmt = 0, CrAmt = 0;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (txtVoucherDt.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (txtVoucherDt.Text.Trim() != "")
            {
                //if (gblFuction.setDate(txtVoucherDt.Text) > vLoginDt)
                //{
                //    EnableControl(true);
                //    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                //    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                //    vResult = false;
                //}
                if (gblFuction.setDate(txtVoucherDt.Text) < vFinFromDt || gblFuction.setDate(txtVoucherDt.Text) > vFinToDt)
                {
                    EnableControl(true);
                    gblFuction.AjxMsgPopup("Voucher Date should be within Logged In financial year.");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
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
            if (Math.Round(CrAmt, 2) != Math.Round(DrAmt, 2))
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Should Be Equal.");
                vResult = false;
            }
            if (txtNarration.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Narration can not left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            // To check for Withdrawl chq Amount
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["DescId"].ToString() != "C0001" && Convert.ToDouble(dr["Credit"].ToString()) > 0)
                    {
                        if (txtChqNo.Text.Trim() == "")
                        {
                            EnableControl(true);
                            gblFuction.AjxMsgPopup("Please Enter the cheque No...");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtChqNo");
                            vResult = false;
                        }
                        if (txtchqdt.Text.Trim() == "")
                        {
                            EnableControl(true);
                            gblFuction.AjxMsgPopup("Please Enter the cheque Date...");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtChqdt");
                            vResult = false;
                        }
                        if (gblFuction.setDate(txtchqdt.Text) < gblFuction.setDate(txtVoucherDt.Text))
                        {
                            EnableControl(true);
                            gblFuction.AjxMsgPopup("cheque Date can not be less than voucher date...");
                            gblFuction.AjxFocus("ctl00_cph_Main_txtchqdt");
                            vResult = false;
                        }
                        if (txtchqdt.Text.Trim() != "")
                        {
                            if (gblFuction.IsDate(txtchqdt.Text) == false)
                            {
                                EnableControl(true);
                                gblFuction.AjxMsgPopup("Please Enter Valid Cheque Date.");
                                gblFuction.AjxFocus("ctl00_cph_Main_txtchqdt");
                                vResult = false;
                            }
                        }
                    }
                }
            }

            return vResult;
        }
        private void GetCashBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBrCode, "A", "");
                ddlCashBank.DataSource = dt;
                ddlCashBank.DataTextField = "Desc";
                ddlCashBank.DataValueField = "DescId";
                ddlCashBank.DataBind();
                ListItem Li = new ListItem("<-- Select -->", "-1");
                ddlCashBank.Items.Insert(0, Li);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtVoucherNo.Enabled = false;
            txtVoucherDt.Enabled = Status;
            ddlDrCr.Enabled = Status;
            ddlCashBank.Enabled = Status;
            txtAmount.Enabled = Status;
            txtNarration.Enabled = Status;
            txtDrTot.Enabled = Status;
            txtCrTot.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
            btnApply.Enabled = Status;
        }
        private void ClearControls()
        {
            ViewState["VouDtl"] = null;
            txtVoucherNo.Text = "";
            ddlDrCr.SelectedIndex = 0;
            ddlCashBank.SelectedIndex = -1;
            txtAmount.Text = "0";
            txtNarration.Text = "";
            txtDrTot.Text = "0";
            txtCrTot.Text = "0";
            hdEdit.Value = "-1";
            gvVoucherDtl.DataSource = null;
            gvVoucherDtl.DataBind();
        }
        protected void btnApply_Click(object sender, EventArgs e)
        {
            string EditMode = string.Empty;
            EditMode = ViewState["ClickMode"].ToString();

            if (ValidateEntry() == false) return;

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
                EnableControl(true);
                gblFuction.AjxMsgPopup("Invalid Amount...");
                gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
                return;
            }
            if (ViewState["VouDtl"] != null && hdEdit.Value == "-1")
            {
                dr = dt.NewRow();
                dr["DescId"] = ddlCashBank.SelectedValue.ToString();
                dr["Desc"] = ddlCashBank.SelectedItem.Text;
                dr["SubsidiaryId"] = "N";
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
                dt.AcceptChanges();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
            }
            else if (hdEdit.Value == "-1")
            {
                dt = new DataTable();
                DataColumn dc = new DataColumn("DC");
                dt.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("Debit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn("Credit", System.Type.GetType("System.Decimal"));
                dt.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn("DescId");
                dt.Columns.Add(dc3);
                DataColumn dc4 = new DataColumn("Desc");
                dt.Columns.Add(dc4);
                DataColumn dc13 = new DataColumn("SubsidiaryId");
                dt.Columns.Add(dc13);
                //dr = dt.NewRow();
                DataColumn dc5 = new DataColumn("DtlId");
                dt.Columns.Add(dc5);
                //dr = dt.NewRow();
                DataColumn dc6 = new DataColumn("Amt");
                dt.Columns.Add(dc6);
                dr = dt.NewRow();

                dr["DescId"] = ddlCashBank.SelectedValue.ToString();
                dr["Desc"] = ddlCashBank.SelectedItem.Text;
                dr["SubsidiaryId"] = "N";
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
                dt.AcceptChanges();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
            }
            else if (hdEdit.Value != "-1" && EditMode == "C_B")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["DescId"] = ddlCashBank.SelectedValue.ToString();
                dt.Rows[vR]["Desc"] = ddlCashBank.SelectedItem.Text;
                dt.Rows[vR]["SubsidiaryId"] = "N";
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
            foreach (DataRow Tdr in dt.Rows)
            {
                vTotDr += Convert.ToDouble(Tdr["Debit"]);
                vTotCr += Convert.ToDouble(Tdr["Credit"]);
            }
            txtDrTot.Text = vTotDr.ToString();
            txtCrTot.Text = vTotCr.ToString();
            hdEdit.Value = "-1";
        }
        private Boolean SaveRecords(string Mode)
        {
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
            try
            {

                vTranType = "N";
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
                    if (this.RoleId != 1 && this.RoleId != 2 && this.RoleId != 4 && this.RoleId != 16)
                    {
                        if (Session[gblValue.EndDate] != null)
                        {
                            if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                            {
                                gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                                return false;
                            }
                        }
                    }

                    oVoucher = new CVoucher();
                    vErr = oVoucher.InsertVoucher(ref vHeadID, ref vVouNo, Session[gblValue.ACVouMst].ToString(),
                        Session[gblValue.ACVouDtl].ToString(), gblFuction.setDate(txtVoucherDt.Text),
                        "N", "", "", txtNarration.Text, gblFuction.setDate(""),
                        //txtChqNo.Text, txtBankName.Text, vTranType, vFinFromDt, vFinToDt, vXmlData, vFinYear,
                        "", "", vTranType, vFinFromDt, vFinToDt, vXmlData, vFinYear,
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
                       "N", "", "", txtNarration.Text, gblFuction.setDate(""),
                        //txtChqNo.Text, txtBankName.Text, vTranType, vXmlData, "E",
                       "", "", vTranType, vXmlData, "E",
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
        private bool ValidateEntry()
        {
            bool vRst = true;

            if (txtAmount.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Amount Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (txtAmount.Text.Trim() == "0")
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Amount Cannot be ZERO.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (ddlCashBank.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Cash/Bank Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlCashBank");
                vRst = false;
            }
            if (ddlDrCr.SelectedIndex < 0)
            {
                EnableControl(true);
                gblFuction.AjxMsgPopup("Dr/Cr Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                vRst = false;
            }
            return vRst;
        }
        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    dt = (DataTable)ViewState["VouDtl"];
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (index != gvVoucherDtl.Rows.Count)
                    {
                        dt.Rows.RemoveAt(index);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                            vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
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
                    hdEdit.Value = "-1";
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
                    ddlCashBank.SelectedIndex = ddlCashBank.Items.IndexOf(ddlCashBank.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));
                    ViewState["ClickMode"] = "C_B";
                    ddlCashBank.Enabled = true;
                    txtAmount.Enabled = true;
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
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            DataTable sortedDT = null;
            string vRptPath = "";

            string pHeadId;
            double vAllToTal = 0.0;

            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                pHeadId = Convert.ToString(ViewState["HeadId"]);
                dt = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode);
                foreach (DataRow dr in dt.Rows)
                {
                    vAllToTal = vAllToTal + Convert.ToDouble(dr["Debit"].ToString());
                }

                DataView dv = dt.DefaultView;
                dv.Sort = "DC ASC";
                sortedDT = dv.ToTable();
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherJournal.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(sortedDT);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
                    if (dt.Rows[0]["VoucherType"].ToString() == "J")
                        rptDoc.SetParameterValue("pTitle", "Journal Voucher");
                    else
                        rptDoc.SetParameterValue("pTitle", "Contra Voucher");
                    rptDoc.SetParameterValue("pAllTotal", vAllToTal);
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Journal_Voucher");
                    Response.ClearHeaders();
                    Response.ClearContent();
                }
            }
            finally
            {
                dt = null;
                oVoucher = null;
                sortedDT = null;
            }
        }
    }
}
