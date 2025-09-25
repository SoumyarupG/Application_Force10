using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class VoucherJDtl : CENTRUMBase
    {
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
                ViewState["ClickMode"] = "Load";
                txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                LoadAcGenLed();
                chkVouEntryLimit();
                if (Request.QueryString["sr"] != null)
                    GetVoucherRPById(Convert.ToString(Server.UrlDecode(Request.QueryString["sr"])));
                else if (Request.QueryString["aId"] != null)
                    GetVoucherRPById(Convert.ToString(Server.UrlDecode(Request.QueryString["aId"])));
                else
                    StatusButton("Add");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pHeadId"></param>
        private void GetVoucherRPById(string pHeadId)
        {
            double vTotDr = 0, vTotCr = 0;
            CVoucher oVoucher = null;
            SortedList Obj = new SortedList();
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
                    txtVoucherNo.Text = Convert.ToString(dt.Rows[0]["VoucherNo"]);
                    txtVoucherDt.Text = gblFuction.getStrDate(Convert.ToString(dt.Rows[0]["VoucherDt"]));
                    ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(Convert.ToString(dt.Rows[0]["DC"]))); ;
                    ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(Convert.ToString(dt.Rows[0]["DescId"])));
                    Obj = ddlLedger.ExtraField;
                    vSubYN = Obj.GetByIndex((ddlLedger.SelectedIndex) - 1).ToString();
                    if (vSubYN == "Y")
                    {
                        ddlSubLedger.Enabled = true;
                        LoadSubAcGenLed();
                        ddlSubLedger.SelectedIndex = ddlSubLedger.Items.IndexOf(ddlSubLedger.Items.FindByValue(dt.Rows[0]["SubsidiaryId"].ToString()));
                        ddlSubLedger.Enabled = true;
                    }
                    else
                    {
                        ddlSubLedger.Enabled = false;
                        ddlSubLedger.ClearSelection();
                    }
                    txtAmount.Text = Convert.ToString(dt.Rows[0]["Amt"]);
                    txtNarration.Text = Convert.ToString(dt.Rows[0]["Narration"]);
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
        private void LoadAcGenLed()
        {
            DataTable dt = null;
            Int32 I = 0;
            CVoucher oVoucher = null;
            SortedList Obj = new SortedList();
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBranch, "V", "");
                dt.AcceptChanges();
                ddlLedger.DataSource = dt;
                foreach (DataRow dr in dt.Rows)
                {
                    Obj.Add(I, dr["SubSiLedYN"].ToString());  //,dr["DescId"].ToString()
                    I = I + 1;
                }
                ddlLedger.DataTextField = "Desc";
                ddlLedger.DataValueField = "DescId";
                ddlLedger.ExtraField = Obj;
                ddlLedger.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlLedger.Items.Insert(0, liSel);
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
        private void LoadSubAcGenLed()
        {
            DataTable dt = null;
            //Int32 I = 0;
            CVoucher oVoucher = null;
            SortedList Obj = new SortedList();
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBranch, "S", ddlLedger.SelectedValue.ToString());
                ddlSubLedger.DataSource = dt;
                ddlSubLedger.DataTextField = "SubsidiaryLed";
                ddlSubLedger.DataValueField = "SubsidiaryId";
                ddlSubLedger.ExtraField = Obj;
                ddlSubLedger.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlSubLedger.Items.Insert(0, liSel);
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
        protected void ddlLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vSubYN = string.Empty;
            SortedList Obj = new SortedList();
            Obj = ddlLedger.ExtraField;

            vSubYN = Obj.GetByIndex((ddlLedger.SelectedIndex) - 1).ToString();
            txtAmount.Text = "0";
            if (vSubYN == "Y")
            {
                ddlSubLedger.Enabled = true;
                LoadSubAcGenLed();
            }
            else
            {
                ddlSubLedger.Enabled = false;
                ddlSubLedger.ClearSelection();
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSubLedger_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmount.Text = "0";
            gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
        }


        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Journal";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuJournal);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Journal Voucher", false);
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
        /// <returns></returns>
        private bool ValidateEntry(string pSubYN)
        {
            bool vRst = true;
            //Int32 vRec = 0;
            //DataTable dt = null;
            if (txtAmount.Text.Trim() == "")
            {
                EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Amount Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (txtAmount.Text.Trim() == "0")
            {
                EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Amount Cannot be ZERO.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (ddlLedger.SelectedIndex <= 0)
            {
                EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlLedger");
                vRst = false;
            }
            if (ddlSubLedger.SelectedIndex <= 0 && pSubYN == "Y")
            {
                EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Subsidiary A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlSubLedger");
                vRst = false;
            }
            if (ddlDrCr.SelectedIndex < 0)
            {
                EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Dr/Cr Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                vRst = false;
            }
            return vRst;
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
                    EnableControl(true, "");
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = false;
                    ClearControls();
                    gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnPrint.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false, "");
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = false;
                    EnableControl(true, "");
                    gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false, "");
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnPrint.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false, "");
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status, string pSubYN)
        {
            txtVoucherNo.Enabled = false;
            txtVoucherDt.Enabled = Status;
            ddlDrCr.Enabled = Status;
            ddlLedger.Enabled = Status;
            ddlSubLedger.Enabled = Status & (pSubYN == "Y");
            txtAmount.Enabled = Status;
            txtNarration.Enabled = Status;
            txtDrTot.Enabled = Status;
            txtCrTot.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtVoucherNo.Text = "";
            ddlDrCr.SelectedIndex = 0;
            ddlLedger.SelectedIndex = -1;
            ddlSubLedger.SelectedIndex = -1;
            txtAmount.Text = "0";
            txtNarration.Text = "";
            txtDrTot.Text = "0";
            txtCrTot.Text = "0";
            hdEdit.Value = "-1";
            gvVoucherDtl.DataSource = null;
            gvVoucherDtl.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            EnableControl(false, "");
            hdEdit.Value = "-1";
            StatusButton("View");
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
            ReportDocument rptDoc = new ReportDocument();
            string pHeadId;
            double vAllToTal = 0.0;
            //try
            //{
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            pHeadId = Convert.ToString(ViewState["HeadId"]);
            dt = oVoucher.GetVoucherDtl(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode);
            foreach (DataRow dr in dt.Rows)
            {
                vAllToTal = vAllToTal + Convert.ToDouble(dr["Debit"].ToString());
            }
            //dt.DefaultView.Sort = "DC ASC";
            DataView dv = dt.DefaultView;
            //dv.Sort = "DC ASC";
            dv.Sort = "DC DESC";
            DataTable sortedDT = dv.ToTable();
            //if (dt.Rows[0]["VoucherType"].ToString() == "P")
            //{
            //    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherPayment.rpt";
            //}
            //else
            //{
            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherJournal.rpt";
            //}
            rptDoc.Load(vRptPath);
            rptDoc.SetDataSource(sortedDT);
            rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
            rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
            rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
            rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
            if (dt.Rows[0]["VoucherType"].ToString() == "R")
                rptDoc.SetParameterValue("pTitle", "Receipt Voucher");
            else if (dt.Rows[0]["VoucherType"].ToString() == "P")
                rptDoc.SetParameterValue("pTitle", "Payment Voucher");
            else if (dt.Rows[0]["VoucherType"].ToString() == "J")
                rptDoc.SetParameterValue("pTitle", "Journal Voucher");
            else
                rptDoc.SetParameterValue("pTitle", "Contra Voucher");
            rptDoc.SetParameterValue("pAllTotal", vAllToTal);
            //rptDoc.SetParameterValue("pFrmDt", txtDtFrm.Text);
            //rptDoc.SetParameterValue("pToDt", txtToDt.Text);

            rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Journal_Voucher");
            rptDoc.Close();
            rptDoc.Dispose();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Private/Transaction/VoucherJ.aspx");
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
                ViewState["VouDtl"] = null;
                ViewState["HeadId"] = null;
                StatusButton("Add");
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
                if (this.RoleId != 1)
                {
                    //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                    //{
                    //    gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                    //    return;
                    //}
                }
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["VoucherEdit"] = "Edit";
                StatusButton("Edit");
                txtVoucherDt.Enabled = false;
                
                btnApply.Enabled = true;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vVoucherEdit = Convert.ToString(ViewState["VoucherEdit"]);
            if (vVoucherEdit == "" || vVoucherEdit == null)
                vVoucherEdit = "Save";
            if (SaveRecords(vVoucherEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                EnableControl(false, "");
                StatusButton("Show");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApply_Click(object sender, EventArgs e)
        {
            string EditMode = string.Empty;
            string vSubYN = string.Empty;
            EditMode = ViewState["ClickMode"].ToString();
            SortedList Obj = new SortedList();
            Obj = ddlLedger.ExtraField;
            if (ddlLedger.SelectedIndex != 0)
                vSubYN = Obj.GetByIndex(ddlLedger.SelectedIndex - 1).ToString();
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
                EnableControl(true, vSubYN);
                gblFuction.AjxMsgPopup("Invalid Amount...");
                gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
                return;
            }
            if (ViewState["VouDtl"] != null && hdEdit.Value == "-1")
            {
                dr = dt.NewRow();
                dr["DescId"] = ddlLedger.SelectedValue.ToString();
                if (vSubYN == "Y")
                {
                    dr["SubsidiaryId"] = ddlSubLedger.SelectedValue.ToString();
                    dr["Desc"] = ddlLedger.SelectedItem.Text;
                    dr["SubDesc"] = ddlSubLedger.SelectedItem.Text;
                }
                else
                {
                    dr["SubsidiaryId"] = "N";
                    dr["Desc"] = ddlLedger.SelectedItem.Text;
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
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
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

                dr["DescId"] = ddlLedger.SelectedValue.ToString();
                if (vSubYN == "Y")
                {
                    dr["SubsidiaryId"] = ddlSubLedger.SelectedValue.ToString();
                    dr["Desc"] = ddlLedger.SelectedItem.Text;
                    dr["SubDesc"] = ddlSubLedger.SelectedItem.Text;
                }
                else
                {
                    dr["SubsidiaryId"] = "N";
                    dr["Desc"] = ddlLedger.SelectedItem.Text;
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
                    vDrAmt = vDrAmt + Convert.ToDouble(dt.Rows[i]["Debit"]);
                    vCrAmt = vCrAmt + Convert.ToDouble(dt.Rows[i]["Credit"]);
                }
            }
            else if (hdEdit.Value != "-1" && EditMode == "G_L")
            {
                Int32 vR = Convert.ToInt32(hdEdit.Value);
                dt.Rows[vR]["DescId"] = ddlLedger.SelectedValue.ToString();
                if (vSubYN == "Y")
                {
                    dt.Rows[vR]["SubsidiaryId"] = ddlSubLedger.SelectedValue.ToString();
                    dt.Rows[vR]["Desc"] = ddlLedger.SelectedItem.Text;
                    dt.Rows[vR]["SubDesc"] = ddlSubLedger.SelectedItem.Text;
                }
                else
                {
                    dt.Rows[vR]["SubsidiaryId"] = "N";
                    dt.Rows[vR]["Desc"] = ddlLedger.SelectedItem.Text;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            string vSubYN = string.Empty;
            SortedList Obj = new SortedList();
            //DataRow dr;
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
                    ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(dt.Rows[index]["DescId"].ToString()));
                    Obj = ddlLedger.ExtraField;
                    vSubYN = Obj.GetByIndex((ddlLedger.SelectedIndex) - 1).ToString();

                    if (vSubYN == "Y")
                    {
                        ddlSubLedger.Enabled = true;
                        LoadSubAcGenLed();
                        ddlSubLedger.SelectedIndex = ddlSubLedger.Items.IndexOf(ddlSubLedger.Items.FindByValue(dt.Rows[index]["SubsidiaryId"].ToString()));
                        ddlSubLedger.Enabled = true;

                    }
                    else
                    {
                        ddlSubLedger.Enabled = false;
                        ddlSubLedger.ClearSelection();
                    }

                    ViewState["ClickMode"] = "G_L";
                    ddlLedger.Enabled = true;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RoleId != 1)
                {
                    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtVoucherDt.Text))
                    {
                        gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                        return;
                    }
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
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
        /// <param name="Mode"></param>
        /// <returns></returns>
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
            int vNoofLedger;
            try
            {

                vTranType = "J";
                dtLedger = (DataTable)ViewState["VouDtl"];
                if (dtLedger == null || dtLedger.Rows.Count <= 0)
                {
                    gblFuction.AjxMsgPopup("Please Enter At Least One Entry.");
                    return false;
                }
                foreach (DataRow Tdr1 in dtLedger.Rows)
                {
                    vNoofLedger = 0;
                    foreach (DataRow Tdr2 in dtLedger.Rows)
                    {
                        if (Tdr1["DescID"].ToString().Trim() == Tdr2["DescID"].ToString().Trim() && Tdr1["SubsidiaryID"].ToString().Trim() == Tdr2["SubsidiaryID"].ToString().Trim())
                        {
                            vNoofLedger += 1;
                        }
                    }
                    if (vNoofLedger > 1)
                    {
                        gblFuction.AjxMsgPopup("One Ledger Cannot Appear More Than Once.");
                        return false;
                    }
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
                if (this.RoleId != 1) 
                //&& Convert.ToInt32(Session[gblValue.UserId]) != 69
                //    && Convert.ToInt32(Session[gblValue.UserId]) != 70 && Convert.ToInt32(Session[gblValue.UserId]) != 1001
                //     && Convert.ToInt32(Session[gblValue.UserId]) != 1511 && Convert.ToInt32(Session[gblValue.UserId]) != 505
                //     && Convert.ToInt32(Session[gblValue.RoleId]) != 12 && Convert.ToInt32(Session[gblValue.UserId]) != 2458
                //    && Convert.ToInt32(Session[gblValue.RoleId]) != 5 && Convert.ToInt32(Session[gblValue.RoleId]) != 9)
                //&& Convert.ToInt32(Session[gblValue.UserId]) != 744 && Convert.ToInt32(Session[gblValue.UserId]) != 1565)//CENTR - 7554,CENTR - 2160
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtVoucherDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }
                if (Mode == "Save")
                {
                    if (ValidateFields(Mode) == false)
                        return false;
                    if (this.RoleId != 1)
                    {
                        //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtVoucherDt.Text))
                        //{
                        //    gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        //    return false;
                        //}
                    }
                    oVoucher = new CVoucher();
                    vErr = oVoucher.InsertVoucher(ref vHeadID, ref vVouNo, Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                        gblFuction.setDate(txtVoucherDt.Text), "J", "", "", txtNarration.Text.Replace("'", "''"), gblFuction.setDate(""),
                        "", "", vTranType, vFinFromDt, vFinToDt, vXmlData, vFinYear, "I", Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId]), 0);

                    if (vErr == 0)
                    {
                        ViewState["HeadId"] = vHeadID;
                        ViewState["VouDtl"] = dtLedger;
                        txtVoucherNo.Text = vVouNo;
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

                    oVoucher = new CVoucher();
                    vErr = oVoucher.UpdateVoucher(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                        Convert.ToString(ViewState["HeadId"]), txtVoucherNo.Text, gblFuction.setDate(txtVoucherDt.Text),
                       "J", "", "", txtNarration.Text.Replace("'", "''"), gblFuction.setDate(""), "", "", vTranType, vXmlData, "E",
                       Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId]), 0);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        ViewState["VouDtl"] = dtLedger;
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
                    oVoucher = new CVoucher();
                    vErr = oVoucher.DeleteVoucher(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(),
                        Convert.ToString(ViewState["HeadId"]), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId]));
                    if (vErr == 0)
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
                return vResult;
            }
            finally
            {
                dtLedger = null;
                oVoucher = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields(string pMode)
        {
            Boolean vResult = true;
            double DrAmt = 0, CrAmt = 0;
            DateTime vFinFromDt = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinToDt = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (txtVoucherDt.Text.Trim() == "")
            {
                EnableControl(true, "");
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (txtVoucherDt.Text.Trim() != "")
            {
                if (gblFuction.setDate(txtVoucherDt.Text) > vLoginDt)
                {
                    EnableControl(true, "");
                    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                    gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                    vResult = false;
                }
                if (gblFuction.setDate(txtVoucherDt.Text) < vFinFromDt || gblFuction.setDate(txtVoucherDt.Text) > vFinToDt)
                {
                    EnableControl(true, "");
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
                foreach (DataRow dR in dt.Rows)
                {
                    DrAmt = DrAmt + Convert.ToDouble(dR["Debit"].ToString());
                    CrAmt = CrAmt + Convert.ToDouble(dR["Credit"].ToString());
                }
            }
            if (DrAmt == 0 && CrAmt == 0)
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Cannot Be ZERO.");
                vResult = false;
            }
            if (Convert.ToDecimal(DrAmt) != Convert.ToDecimal(CrAmt))
            {
                gblFuction.AjxMsgPopup("Debit And Credit Amount Should Be Equal.");
                vResult = false;
            }
            if (hdStatYN.Value == "Y" && DrAmt == CrAmt)
            {
                if (Convert.ToDouble(hdJrAmt.Value) > 0)
                {
                    if (DrAmt > Convert.ToDouble(hdJrAmt.Value))
                    {
                        gblFuction.AjxMsgPopup("Authorised Amount limit has been exceed...");
                        vResult = false;
                    }
                }
            }
            if (hdStatYN.Value == "Y" && DrAmt == CrAmt)
            {
                if (DrAmt > Convert.ToDouble(hdJrAmt.Value))
                {
                    gblFuction.AjxMsgPopup("Authorised Amount limit cannot be over....");
                    vResult = false;
                }
            }
            if (txtNarration.Text.Trim() == "")
            {
                EnableControl(true, "");
                gblFuction.AjxMsgPopup("Narration can not left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            return vResult;
        }

        /// <summary>
        /// 
        /// </summary>
        private void chkVouEntryLimit()
        {
            DataTable dt = null;
            CRole oRl = null;
            Int32 vRoleId = Convert.ToInt32(Session[gblValue.RoleId].ToString());
            try
            {
                oRl = new CRole();
                dt = oRl.GetRoleById(vRoleId);
                if (dt.Rows.Count > 0)
                {
                    hdStatYN.Value = Convert.ToString(dt.Rows[0]["Stat_YN"]);
                    hdJrAmt.Value = Convert.ToString(dt.Rows[0]["JrAmt"]);
                }
            }
            finally
            {
                oRl = null;
                dt = null;
            }
        }



        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            Double vAmt = 0.0;
            CVoucher oBJV = null;
            oBJV = new CVoucher();
            DataTable dt = null;
            string pHeadId;
            double VouAmt = 0;

            if (ddlSubLedger.Enabled == true)
            {
                if (ddlSubLedger.SelectedValue == "C0001")
                {
                    if (Convert.ToString(ViewState["VoucherEdit"]) == "Edit")
                    {
                        string vBrCode = Session[gblValue.BrnchCode].ToString();
                        pHeadId = Convert.ToString(ViewState["HeadId"]);
                        dt = oBJV.GetVoucherDtlAmt(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode, "C0001");
                        if (dt.Rows.Count > 0)
                        {
                            VouAmt = Convert.ToDouble(dt.Rows[0]["Amt"]);
                        }
                        vAmt = VouAmt + oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                    }
                    else
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                    }
                }
                else
                {
                    if (Convert.ToString(ViewState["VoucherEdit"]) == "Edit")
                    {
                        string vBrCode = Session[gblValue.BrnchCode].ToString();
                        pHeadId = Convert.ToString(ViewState["HeadId"]);
                        dt = oBJV.GetVoucherDtlAmt(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode, ddlSubLedger.SelectedValue);
                        if (dt.Rows.Count > 0)
                        {
                            VouAmt = Convert.ToDouble(dt.Rows[0]["Amt"]);
                        }
                        vAmt = VouAmt + oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                  , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), ddlSubLedger.SelectedValue);
                    }
                    else
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                  , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), ddlSubLedger.SelectedValue);
                    }
                }
            }
            else
            {
                if (ddlLedger.SelectedValue == "C0001")
                {
                    if (Convert.ToString(ViewState["VoucherEdit"]) == "Edit")
                    {
                        string vBrCode = Session[gblValue.BrnchCode].ToString();
                        pHeadId = Convert.ToString(ViewState["HeadId"]);
                        dt = oBJV.GetVoucherDtlAmt(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode, "C0001");
                        if (dt.Rows.Count > 0)
                        {
                            VouAmt = Convert.ToDouble(dt.Rows[0]["Amt"]);
                        }
                        vAmt = VouAmt + oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                    }
                    else
                    {
                        vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), "C0001");
                    }
                }
                else
                {
                    if (ddlLedger.SelectedValue != "G0156" && ddlLedger.SelectedValue != "G0123")
                    {
                        if (Convert.ToString(ViewState["VoucherEdit"]) == "Edit")
                        {
                            string vBrCode = Session[gblValue.BrnchCode].ToString();
                            pHeadId = Convert.ToString(ViewState["HeadId"]);
                            dt = oBJV.GetVoucherDtlAmt(Session[gblValue.ACVouMst].ToString(), Session[gblValue.ACVouDtl].ToString(), pHeadId, vBrCode, ddlLedger.SelectedValue);
                            if (dt.Rows.Count > 0)
                            {
                                VouAmt = Convert.ToDouble(dt.Rows[0]["Amt"]);
                            }
                            vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                      , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), ddlLedger.SelectedValue);
                        }
                        else
                        {
                            vAmt = oBJV.GetClosingCashBank(gblFuction.setDate(Session[gblValue.FinFromDt].ToString())
                                      , gblFuction.setDate(txtVoucherDt.Text), Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.FinYrNo]), ddlLedger.SelectedValue);
                        }
                    }
                }
            }

            if (ddlDrCr.SelectedValue.ToString() == "D")
            {
                if (ddlLedger.SelectedValue != "G0156" && ddlLedger.SelectedValue != "G0123")
                {
                    if (Math.Round(Convert.ToDouble(txtAmount.Text), 2) > vAmt)
                    {
                        gblFuction.AjxMsgPopup("Insufficient Balance.");
                        if (ddlSubLedger.Enabled == true)
                        {
                            ddlSubLedger.SelectedIndex = -1;
                            txtAmount.Text = "";
                        }
                        else
                        {
                            ddlLedger.SelectedIndex = -1;
                            txtAmount.Text = "";
                        }
                        return;
                    }
                }
            }
        }
    }
}