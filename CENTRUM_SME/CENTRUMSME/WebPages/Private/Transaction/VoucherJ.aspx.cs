using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class VoucherJ : CENTRUMBAse
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
                if (txtFromDt.Text == "" || txtToDt.Text == "")
                {
                    txtFromDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                    txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                }
                ViewState["VoucherEdit"] = null;
                ViewState["VouDtl"] = null;
                ViewState["ClickMode"] = "Load";
                StatusButton("View");

                txtVoucherDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtAmount.Attributes.Add("onKeyPress", "javascript:return CheckNumbers(event,this)");
                LoadAcGenLed();
                LoadGrid(1);
                tabAcHd.ActiveTabIndex = 0;
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

            }
            
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
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Journal Voucher";                
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuJournal);
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
                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
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
                    dt = oVoucher.GetVoucherlist(vAcMst, vAcDtl, vBrCode, vDtFrom, vDtTo, vSearch, "J", pPgIndx, ref vR); //VoucherTyp=R/P 
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
		if (dt != null)
		   dt.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        private int getTotPages(double pRows)
        {
            int vTPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vTPg;
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
                    EnableControl(true,"");
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
                    EnableControl(false,"");
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPrint.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true,"");
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
                    EnableControl(false,"");
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
                    EnableControl(false,"");
                    break;
            }
        }
        private void EnableControl(Boolean Status, string pSubYN)
        {
            txtVoucherNo.Enabled = false;
            txtVoucherDt.Enabled = true;
            ddlDrCr.Enabled = Status;
            ddlLedger.Enabled = Status;
            ddlSubLedger.Enabled = Status & (pSubYN == "Y");
            txtAmount.Enabled = Status;
            txtNarration.Enabled = Status;
            txtDrTot.Enabled = Status;
            txtCrTot.Enabled = Status;
            gvVoucherDtl.Enabled = Status;
            btnApply.Enabled = Status;
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
                EnableControl(true, "");
                //txtVoucherDt.Enabled = true;
                //ceVDt.Enabled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// 
        protected void btnApply_Click(object sender, EventArgs e)
        {
            SortedList Obj = null; DataTable dt = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            try
            {
                string EditMode = string.Empty;
                string vSubYN = string.Empty;
                EditMode = ViewState["ClickMode"].ToString();
                Obj = new SortedList();

                if (Convert.ToInt32(ddlSubLedger.SelectedIndex) > 0)
                    //vSubYN = Obj.GetByIndex(ddlLedger.SelectedIndex - 1).ToString();
                    vSubYN = "Y";
                else
                    vSubYN = "N";
                if (ValidateEntry(vSubYN) == false) return;


                DataRow dr;
                dt = (DataTable)ViewState["VouDtl"];
                string vChkNum = "";
                double Num;
                bool isNum = false;
                vChkNum = txtAmount.Text.Trim();
                isNum = double.TryParse(vChkNum, out Num);
                if (isNum == false)
                {
                    //EnableControl(true, vSubYN);
                    gblFuction.AjxMsgPopup("Invalid Amount...");
                    //gblFuction.focus("ctl00_cph_Main_tabGenLed_pnlDtl_txtAmount");
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
                    DataColumn dc5 = new DataColumn();
                    dc5.ColumnName = "DtlId";
                    dt.Columns.Add(dc5);
                    DataColumn dc13 = new DataColumn();
                    dc13.ColumnName = "SubsidiaryId";
                    dt.Columns.Add(dc13);
                    DataColumn dc14 = new DataColumn();
                    dc14.ColumnName = "SubDesc";
                    dt.Columns.Add(dc14);
                    DataColumn dc6 = new DataColumn();
                    dc6.ColumnName = "Amt";
                    dt.Columns.Add(dc6);
                    dr = dt.NewRow();

                    dr["DescId"] = ddlLedger.SelectedValue.ToString();
                    if (ddlSubLedger.SelectedIndex != 0)
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
            finally
            {
                Obj = null;
                dt = null;
            }
        }
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControls();
            EnableControl(false,"");
            StatusButton("View");
            hdEdit.Value = "-1";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtFromDt.Text.Trim() == "")
            {
                EnableControl(true,"");
                gblFuction.MsgPopup("From Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtFromDt");
                vResult = false;
            }
            if (txtFromDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtFromDt.Text) == false)
                {
                    EnableControl(true,"");
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtFromDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() == "")
            {
                EnableControl(true,"");
                gblFuction.MsgPopup("To Date Cannot be left blank.");
                gblFuction.focus("ctl00_cph_Main_txtToDt");
                vResult = false;
            }
            if (txtToDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    EnableControl(true,"");
                    gblFuction.MsgPopup("Please Enter Valid Date.");
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            return vResult;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.RoleId != 1 && this.RoleId != 2)
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

        
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Webpages/Public/Main.aspx", false);
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
                btnApply.Enabled = true;
               //txtVoucherDt.Enabled = true;
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
            try
            {

                vTranType = "J";
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
                        "J", "", "", txtNarration.Text, gblFuction.setDate(""),
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
                       "J", "", "", txtNarration.Text, gblFuction.setDate(""),
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
        
        protected void gvVouvher_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vHeadId = "";            
            string vSubYN = string.Empty;
            CVoucher oVoucher = null;
            DataTable dtAc = null;
            double vTotDr = 0, vTotCr = 0;

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
                        ddlDrCr.SelectedIndex = ddlDrCr.Items.IndexOf(ddlDrCr.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["DC"]))); ;
                        ddlLedger.SelectedIndex = ddlLedger.Items.IndexOf(ddlLedger.Items.FindByValue(Convert.ToString(dtAc.Rows[0]["DescId"])));

                        vSubYN = "N";
                        if (vSubYN == "Y")
                        {
                            ddlSubLedger.Enabled = true;
                            LoadSubAcGenLed();
                            ddlSubLedger.SelectedIndex = ddlSubLedger.Items.IndexOf(ddlSubLedger.Items.FindByValue(dtAc.Rows[0]["SubsidiaryId"].ToString()));
                            ddlSubLedger.Enabled = true;
                        }
                        else
                        {
                            ddlSubLedger.Enabled = false;
                            ddlSubLedger.ClearSelection();
                        }
                        txtAmount.Text = Convert.ToString(dtAc.Rows[0]["Amt"]);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, System.EventArgs e)
        {
            LoadGrid(1);
        }

        private void LoadAcGenLed()
        {
            DataTable dt = null;
            Int32 I = 0;
            CVoucher oVoucher = null;
            SortedList Obj = null;
            try
            {
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                Obj = new SortedList();
                dt = oVoucher.GetAcGenLedCB(vBranch, "G", "");
                dt.AcceptChanges();
                ddlLedger.DataSource = dt;
                foreach (DataRow dr in dt.Rows)
                {

                    Obj.Add(I, dr["SubSiLedYN"].ToString());  //,dr["DescId"].ToString()
                    I = I + 1;
                }
                ddlLedger.DataTextField = "Desc";
                ddlLedger.DataValueField = "DescId";
                ddlLedger.DataBind();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlLedger.Items.Insert(0, liSel);
            }
            finally
            {
                oVoucher = null;
                Obj = null;
                dt = null;
            }
        }


        private void LoadSubAcGenLed()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            SortedList Obj = null;
            try
            {
                Obj = new SortedList();
                string vBranch = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBranch, "S", ddlLedger.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    ddlSubLedger.DataSource = dt;
                    ddlSubLedger.DataTextField = "SubsidiaryLed";
                    ddlSubLedger.DataValueField = "SubsidiaryId";
                    ddlSubLedger.DataBind();
                }
                else
                {
                    ddlSubLedger.Enabled = false;
                    ddlSubLedger.ClearSelection();
                    ddlSubLedger.Items.Clear();
                    gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                }
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlSubLedger.Items.Insert(0, liSel);
                ddlSubLedger.Enabled = true;
            }
            finally
            {
                oVoucher = null;
                dt = null;
                Obj = null;
            }
        }


        private bool ValidateEntry(string pSubYN)
        {
            bool vRst = true;

            if (txtAmount.Text.Trim() == "")
            {
                //EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Amount Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (txtAmount.Text.Trim() == "0")
            {
                //EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Amount Cannot be ZERO.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
                vRst = false;
            }
            if (ddlLedger.SelectedIndex <= 0)
            {
                //EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlLedger");
                vRst = false;
            }
            if (ddlSubLedger.SelectedIndex <= 0 && pSubYN == "Y")
            {
                //EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Subsidiary A/c Ledger Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlSubLedger");
                vRst = false;
            }
            if (ddlDrCr.SelectedIndex < 0)
            {
                //EnableControl(true, pSubYN);
                gblFuction.AjxMsgPopup("Dr/Cr Cannot be left blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_ddlDrCr");
                vRst = false;
            }

            return vRst;
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
                //EnableControl(true, "");
                gblFuction.AjxMsgPopup("Voucher Date Cannot be left blank.");
                //gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                vResult = false;
            }
            if (txtVoucherDt.Text.Trim() != "")
            {
                //if (gblFuction.setDate(txtVoucherDt.Text) > vLoginDt)
                //{
                //    //EnableControl(true, "");
                //    gblFuction.AjxMsgPopup("Voucher Date should not be grater than login date...");
                //    //gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
                //    vResult = false;
                //}
                if (gblFuction.setDate(txtVoucherDt.Text) < vFinFromDt || gblFuction.setDate(txtVoucherDt.Text) > vFinToDt)
                {
                    //EnableControl(true, "");
                    gblFuction.AjxMsgPopup("Voucher Date should be within Logged In financial year.");
                    //gblFuction.AjxFocus("ctl00_cph_Main_txtVoucherDt");
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
                //EnableControl(true, "");
                gblFuction.AjxMsgPopup("Narration can not left Blank.");
                gblFuction.AjxFocus("ctl00_cph_Main_txtNarration");
                vResult = false;
            }
            return vResult;
        }

        protected void gvVoucherDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            SortedList Obj = null;
            double vDrAmt = 0, vCrAmt = 0, vTotDr = 0, vTotCr = 0;
            string vSubYN = string.Empty;

            try
            {
                Obj = new SortedList();
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
                    //Obj = ddlLedger.ExtraField;
                    //vSubYN = Obj.GetByIndex((ddlLedger.SelectedIndex) - 1).ToString();
                    vSubYN = "N";
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
                Obj = null;
                dt = null;
            }
        }


        protected void ddlSubLedger_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmount.Text = "0";
            gblFuction.AjxFocus("ctl00_cph_Main_txtAmount");
        }

        protected void ddlLedger_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vSubYN = string.Empty;
            SortedList Obj = null;

            try
            {
                Obj = new SortedList();
                //Obj = ddlLedger.ExtraField;
                //vSubYN = Obj.GetByIndex((ddlLedger.SelectedIndex) - 1).ToString();
                txtAmount.Text = "0";
                LoadSubAcGenLed();
            }
            finally
            {
                Obj = null;
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
                EnableControl(false, "");
                StatusButton("View");
                ClearControls();
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
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
                DataTable sortedDT = dv.ToTable();

                using (ReportDocument rptDoc = new ReportDocument())
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptPrintVoucherJournal.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(sortedDT);
                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", Session[gblValue.BrName].ToString());
                    rptDoc.SetParameterValue("pTitle", "Journal Voucher");
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
            }
        }
    }
}