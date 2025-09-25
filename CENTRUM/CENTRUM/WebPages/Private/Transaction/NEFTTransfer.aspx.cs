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


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NEFTTransfer : CENTRUMBase
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                PopBank();
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "NEFT Disbursement";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuNEFTTransfer);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    //btnSave.Visible = true;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "NEFT disbursement Approva", false);
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            Int32 vRows = 0;
            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select atleast one branch...");
                return;
            }
            try
            {
                oNeft = new CNEFTTransfer();
                dt = oNeft.GetNEFTTransferPG(gblFuction.setDate(txtDt.Text), ddlBranch.SelectedValues.Replace("|", ","));
                gvDtl.DataSource = dt.DefaultView;
                gvDtl.DataBind();
                if (dt.Rows.Count > 0)
                {
                    txtTotNEFT.Text = dt.Rows[0]["NEFTFund"].ToString();
                }
                //if (dt.Rows.Count <= 0)
                //{
                //    lblTotalPages.Text = "0";
                //    lblCurrentPage.Text = "0";
                //}
                //else
                //{
                //    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                //    lblCurrentPage.Text = cPgNo.ToString();
                //}
                //if (cPgNo == 1)
                //{
                //    Btn_Previous.Enabled = false;
                //    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = true;
                //    else
                //        Btn_Next.Enabled = false;
                //}
                //else
                //{
                //    Btn_Previous.Enabled = true;
                //    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                //        Btn_Next.Enabled = false;
                //    else
                //        Btn_Next.Enabled = true;
                //}
            }
            finally
            {
                dt = null;
                oNeft = null;
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
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dt = null;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0;
            string vXml = "";
            double TotDisbAmt = 0, vClosingBal = 0; ;
            string vDescIDBank = ddlBank.SelectedValue.ToString();
            string vMsg = "";
            if (XmlFromGrid() == "")
            {
                return false;
            }
            else
            {
                vXml = XmlFromGrid();
            }
            TotDisbAmt = TotalDisbAmt();
            if (Convert.ToDouble(txtTotNEFT.Text) < Convert.ToDouble(txtTotAmt.Text))
            {
                gblFuction.AjxMsgPopup("Total NEFT Disbursement Amount cannot be greater than total amount...");
                return false;
            }
            //if (ValidateFields() == false)
            //    return false;

            CNEFTTransfer oNEFT = null;
            if (ddlBank.SelectedIndex == 0)
            {
                gblFuction.AjxMsgPopup("Select Bank");
                return false;
            }

            try
            {
                if (Mode == "Save")
                {

                    oNEFT = new CNEFTTransfer();
                    //if (TotDisbAmt == 0)
                    //{
                    //    //gblFuction.AjxMsgPopup("");
                    //    return false;

                    //}
                    vClosingBal = oNEFT.GetClosingBalBranchWise(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtDt.Text), vDescIDBank, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
                    if (vClosingBal < TotDisbAmt)
                    {
                        gblFuction.AjxMsgPopup("Insufficient Balance");
                        return false;
                    }
                    vErr = oNEFT.InsertNEFTTransfer(vXml, vDescIDBank, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(txtDt.Text), "I", ref vMsg);
                    if (vErr == 2)
                    {
                        gblFuction.AjxMsgPopup(vErr.ToString());
                        vResult = false;
                    }
                    else
                    {
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
                }
                else if (Mode == "Edit")
                {
                    //if (ValidateFields() == false)
                    //    return false;

                    //oReSchedule = new CReScheduling();
                    //dt = (DataTable)ViewState["ReSchedule"];
                    //dt = (DataTable)ViewState["ReSchedule"];
                    ////vXmlSch = DataTableTOXml(dt);

                    //vErr = oReSchedule.UpdateReScheduleMst(vRescheduleId, vLoanID, vReSchDate, vFInstNo, vAcDueDate, vNoofdays, vCurrDate, vHappDate,
                    //            vReason, txtRemark.Text, Convert.ToInt32(ddlResch.SelectedValue), Convert.ToInt32(ddlAprov.SelectedValue),
                    //            vBrCode, this.UserID, "E");
                    //if (vErr == 0)
                    //{
                    //    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    //    vResult = false;
                    //}
                }
                return vResult;
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
        private Boolean ValidateFields()
        {
            Boolean vResult = true;

            return vResult;
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
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
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
        private void ClearControls()
        {
            txtTotNEFT.Text = "0";
            //LoadGrid(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        {

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
                    gblFuction.AjxMsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = null;
                tabReSchdl.ActiveTabIndex = 1;
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabReSchdl.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
                //if (this.RoleId != 1)
                //{
                //    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                //    {
                //        gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                //        return;
                //    }
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Del);
                    return;
                }
                //if (SaveRecords("Delete") == true)
                //{
                //    gblFuction.AjxMsgPopup(gblMarg.DeleteMsg);
                //    LoadGrid(0);
                //    StatusButton("Delete");
                //}
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
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    LoadGrid(0);
                    StatusButton("View");
                    ddlBank.Enabled = false;
                    //btnSave.Enabled = false; 
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
                //if (this.RoleId != 1)
                //{
                //    if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                //    {
                //        gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
                //        return;
                //    }
                //}
                if (this.CanEdit == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Edit);
                    return;
                }
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlCo");
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

        //protected void Page_Error(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["ErrMsg"] = sender.ToString();
        //        Response.RedirectPermanent("~/ErrorInfo.aspx", false);
        //    }
        //    catch (Exception ex)
        //    { 
        //        throw new Exception();
        //    }
        //}     

        protected void gvDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            gvDtl.Visible = true;
            EventArgs Args;
            DataTable dt = null;
            CDisburse oDbr = new CDisburse();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblBranch = (Label)e.Row.FindControl("lblBranch");
                Label lblAppDate = (Label)e.Row.FindControl("lblAppDate");
                Label lblAppNo = (Label)e.Row.FindControl("lblAppNo");
                Label lblRO = (Label)e.Row.FindControl("lblRO");
                Label lblCenter = (Label)e.Row.FindControl("lblCenter");
                Label lblGroup = (Label)e.Row.FindControl("lblGroup");
                Label lblMemName = (Label)e.Row.FindControl("lblMemName");
                Label lblLnShceme = (Label)e.Row.FindControl("lblLnShceme");
                Label lblLnAmt = (Label)e.Row.FindControl("lblLnAmt");
                Label lblExpDisbDate = (Label)e.Row.FindControl("lblExpDisbDate");

                Label lblBankNm = (Label)e.Row.FindControl("lblBankNm");
                Label lblBankBranch = (Label)e.Row.FindControl("lblBankBranch");
                Label lblIFSC = (Label)e.Row.FindControl("lblIFSC");
                Label lblAcNo = (Label)e.Row.FindControl("lblAcNo");
                CheckBox chkDisb = (CheckBox)e.Row.FindControl("chkDisb");
                CheckBox chkAppFrCashDisb = (CheckBox)e.Row.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancel = (CheckBox)e.Row.FindControl("ChkCancel");
                TextBox txtRemarks = (TextBox)e.Row.FindControl("txtRemarks");
                TextBox txtCashRemark = (TextBox)e.Row.FindControl("txtCashRemark");
                TextBox txtPostRemark = (TextBox)e.Row.FindControl("txtPostRemark");
                CheckBox ChkPostpone = (CheckBox)e.Row.FindControl("ChkPostpone");
                DropDownList ddlIcMst = (DropDownList)e.Row.FindControl("ddlIcMst");
                try
                {
                    lblBranch.Text = e.Row.Cells[19].Text.Trim();
                    lblAppDate.Text = e.Row.Cells[20].Text.Trim();
                    lblAppNo.Text = e.Row.Cells[21].Text.Trim();
                    lblRO.Text = e.Row.Cells[22].Text.Trim();
                    lblCenter.Text = e.Row.Cells[23].Text.Trim();
                    lblGroup.Text = e.Row.Cells[24].Text.Trim();
                    lblMemName.Text = e.Row.Cells[25].Text.Trim();
                    lblLnShceme.Text = e.Row.Cells[26].Text.Trim();
                    //lblAppDate.Text = e.Row.Cells[13].Text.Trim();
                    lblLnAmt.Text = e.Row.Cells[27].Text.Trim();
                    lblExpDisbDate.Text = e.Row.Cells[28].Text.Trim();

                    lblBankNm.Text = e.Row.Cells[29].Text.Trim();
                    lblBankBranch.Text = e.Row.Cells[30].Text.Trim();
                    lblIFSC.Text = e.Row.Cells[31].Text.Trim();
                    lblAcNo.Text = e.Row.Cells[32].Text.Trim();
                    string ID = e.Row.Cells[33].Text.Trim();
                    if (e.Row.Cells[42].Text.Trim() != "")
                    {
                        dt = oDbr.GetInSuranceCompany(e.Row.Cells[42].Text.Trim(), gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                        ViewState["Insurance"] = dt;
                        ddlIcMst.Items.Clear();
                        ddlIcMst.DataSource = dt;
                        ddlIcMst.DataTextField = "ICName";
                        ddlIcMst.DataValueField = "ICId";
                        ddlIcMst.DataBind();
                        //ListItem oLi = new ListItem("<--Select-->", "-1");
                        //ddlIcMst.Items.Insert(0, oLi);
                    }
                    ddlIcMst.SelectedIndex = ddlIcMst.Items.IndexOf(ddlIcMst.Items.FindByValue(e.Row.Cells[44].Text.Trim()));
                    if (e.Row.Cells[34].Text == "Y")
                    {
                        chkDisb.Checked = true;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        chkDisb_CheckedChanged(chkDisb, e);
                    }
                    else
                    {
                        chkDisb.Checked = false;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        //chkDisb_CheckedChanged(chkDisb, e);
                    }
                    if (e.Row.Cells[35].Text == "Y")
                    {
                        chkAppFrCashDisb.Checked = true;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = true;
                        txtPostRemark.Enabled = false;
                        chkAppFrCashDisb_CheckedChanged(chkAppFrCashDisb, e);
                    }
                    else
                    {
                        chkAppFrCashDisb.Checked = false;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        txtPostRemark.Enabled = false;
                        //chkAppFrCashDisb_CheckedChanged(chkAppFrCashDisb, e);
                    }
                    if (e.Row.Cells[36].Text == "Y")
                    {
                        ChkCancel.Checked = true;
                        txtRemarks.Enabled = true;
                        txtCashRemark.Enabled = false;
                        txtPostRemark.Enabled = false;
                        ChkCancel_CheckedChanged(ChkCancel, e);
                    }
                    else
                    {
                        ChkCancel.Checked = false;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        txtPostRemark.Enabled = false;
                        //ChkCancel_CheckedChanged(ChkCancel, e);
                    }
                    if (e.Row.Cells[37].Text == "Y")
                    {
                        ChkPostpone.Checked = true;
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        txtPostRemark.Enabled = true;
                        ChkPostpone_CheckedChanged(ChkPostpone, e);
                    }
                    else
                    {
                        ChkPostpone.Checked = false;
                        //txtRemarks.Text = "";
                        txtRemarks.Enabled = false;
                        txtCashRemark.Enabled = false;
                        txtPostRemark.Enabled = false;
                        //ChkPostpone_CheckedChanged(ChkPostpone, e);
                    }

                }

                finally
                {
                }
            }
        }
        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("Tr");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("Loantype");
            dt.Columns.Add("LoanAppAmt");
            dt.Columns.Add("BranchName");
            dt.Columns.Add("EoName");
            dt.Columns.Add("Market");
            dt.Columns.Add("DisbYN");
            dt.Columns.Add("DisbForCashYN");
            dt.Columns.Add("CancelYN");
            dt.Columns.Add("CancelRsn");
            dt.Columns.Add("CashRemarks");
            dt.Columns.Add("PostponeYN");
            dt.Columns.Add("PostRemarks");
            dt.Columns.Add("IcId");
            foreach (GridViewRow gr in gvDtl.Rows)
            {


                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                CheckBox chkAppFrCashDisb = (CheckBox)gr.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
                CheckBox ChkPostpone = (CheckBox)gr.FindControl("ChkPostpone");
                TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
                TextBox txtCashRemark = (TextBox)gr.FindControl("txtCashRemark");
                TextBox txtPostRemark = (TextBox)gr.FindControl("txtPostRemark");
                DropDownList ddlIcMst = (DropDownList)gr.FindControl("ddlIcMst");
                string LoanAppId = gr.Cells[33].Text;
                if (chkDisb.Checked == true || chkAppFrCashDisb.Checked == true || ChkCancel.Checked == true || ChkPostpone.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanAppId"] = LoanAppId;
                    dr["Loantype"] = gr.Cells[26].Text;
                    dr["LoanAppAmt"] = gr.Cells[27].Text;
                    dr["BranchName"] = gr.Cells[19].Text;
                    dr["EoName"] = gr.Cells[22].Text;
                    dr["Market"] = gr.Cells[23].Text;

                    if (chkDisb.Checked == true)
                    {
                        dr["DisbYN"] = 'Y';
                        if (ddlIcMst.SelectedIndex < 0)
                        {
                            gblFuction.AjxMsgPopup("Please Select an insurance for Loan Application No. " + gr.Cells[21].Text);
                            return "";
                        }
                        else
                        {
                            dr["IcId"] = ddlIcMst.SelectedValue;
                        }
                    }
                    else
                        dr["DisbYN"] = 'N';
                    if (chkAppFrCashDisb.Checked == true)
                    {
                        dr["DisbForCashYN"] = 'Y';
                        if (txtCashRemark.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Give a Cash remarks for Cash selection...");
                            txtCashRemark.Focus();
                            return "";
                        }
                        else
                        {
                            dr["CashRemarks"] = txtCashRemark.Text;
                        }
                    }
                    else
                        dr["DisbForCashYN"] = 'N';
                    if (ChkCancel.Checked == true)
                    {
                        dr["CancelYN"] = 'Y';
                        if (txtRemarks.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Give a cancel remarks for cancel selection...");
                            txtRemarks.Focus();
                            return "";
                        }
                        else
                        {
                            dr["CancelRsn"] = txtRemarks.Text;
                        }

                    }
                    else
                        dr["CancelYN"] = 'N';
                    if (ChkPostpone.Checked == true)
                    {
                        dr["PostponeYN"] = 'Y';
                        if (txtPostRemark.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Give a Postpone remarks for Postpone selection...");
                            txtPostRemark.Focus();
                            return "";
                        }
                        else
                        {
                            dr["PostRemarks"] = txtPostRemark.Text;
                        }
                    }
                    else
                        dr["PostponeYN"] = 'N';
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    i++;
                }
            }
            if (dt.Rows.Count > 0)
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dt.WriteXml(oSW);
                    vXML = oSW.ToString();
                }
            }
            return vXML;

        }
        private void PopBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            //string vBrCode = "";
            //Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            oNeft = new CNEFTTransfer();
            dt = oNeft.PopBank();

            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem olist = new ListItem("<--select-->", "-1");
            ddlBank.Items.Insert(0, olist);
        }
        private double TotalDisbAmt()
        {
            double TotAmt = 0;
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                CheckBox chkAppFrCashDisb = (CheckBox)gr.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
                Label lblLnAmt = (Label)gr.FindControl("lblLnAmt");
                if (chkDisb.Checked == true || chkAppFrCashDisb.Checked == true || ChkCancel.Checked == true)
                {
                    TotAmt += Convert.ToDouble(lblLnAmt.Text.Trim());

                }

            }
            return TotAmt;
        }
        private void GenerateReport()
        {
            System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
            DataTable dt = null, dt1 = null, dt2 = null;
            string vFileNm = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vDate = Session[gblValue.LoginDate].ToString();
            DataSet ds = null;
            CAcGenled oAcGenLed = null;
            CReports oRpt = new CReports();
            string strData2 = "";
            double TotDisbAmt = 0, vClosingBal = 0;
            string vXml = "";
            string vDescIDBank = ddlBank.SelectedValue.ToString();
            CNEFTTransfer oNEFT = null;
            oNEFT = new CNEFTTransfer();

            TotDisbAmt = TotalDisbAmt();
            vXml = XmlFromGrid();
            TotDisbAmt = TotalDisbAmt();

            if (ddlBank.SelectedIndex == 0)
            {
                gblFuction.AjxMsgPopup("Select Bank");
                return;
            }

            if (TotDisbAmt == 0)
            {
                gblFuction.AjxMsgPopup("Please select disburse atleast one loan");
                return;
            }

            vClosingBal = oNEFT.GetClosingBalBranchWise(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtDt.Text), vDescIDBank, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
            if (vClosingBal < TotDisbAmt)
            {
                gblFuction.AjxMsgPopup("Insufficient Balance");
                return;
            }
            tdx.Controls.Add(DataGrid1);
            tdx.Visible = false;
            vFileNm = "attachment;filename=TRF_NEFT_LETTER.xls";
            System.IO.StringWriter sw = new System.IO.StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            htw.WriteLine("<table cellpadding='12' widht='100%'>");
            htw.WriteLine("<tr><td align=center' colspan='10'><b><font size='5'>" + gblValue.CompName + "</font></b></td></tr>");
            htw.WriteLine("<tr><td align=center' colspan='10'><b><u><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            htw.WriteLine("<tr><td align=right' colspan='10'><b><font size='3'>Date : " + vDate + "</font></b></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><b><font size='3'>To,</font></b></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><b><u><font size='3'>The Branch Manager,</font></u></b></td></tr>");
            oAcGenLed = new CAcGenled();
            ds = oAcGenLed.GetGenLedSubsidairyDtl(ddlBank.SelectedValue.ToString());
            dt2 = ds.Tables[0];
            if (dt2.Rows.Count > 0)
            {

                strData2 = dt2.Rows[0]["Address1"].ToString();
                char[] separator2 = new char[] { ',' };
                string[] strSplitArr = strData2.Split(separator2);
                foreach (string arrStr in strSplitArr)
                {
                    Response.Write(arrStr + "<br/>");
                    htw.WriteLine("<tr><td align=left' colspan='10'><b><font size='3'>" + arrStr + "</font></b></td></tr>");
                }
                htw.WriteLine("<tr><td align=center' colspan='10'><b><u><font size='3'>Re: RTGS/NEFT of Fund From our Current Account No: " + dt2.Rows[0]["Phone"].ToString() + "</font></u></b></td></tr>");
            }

            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Dear Sir,</font></td></tr>");
            ds = oRpt.NEFTTransferLetter(vXml, ddlBank.SelectedValue.ToString(), gblFuction.setDate(txtDt.Text));
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];
            htw.WriteLine("<tr><td colspan='12'><font size='3'></font></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Please transfer of Rs. " + dt.Rows[0]["TotTransAmt"].ToString() + " (Rupees " + dt.Rows[0]["NumToWord"].ToString() + ") only through NEFT RTGS </font></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>from our Current Account No:  " + dt2.Rows[0]["Phone"].ToString() + " to the below mentioned accounts of  Centrum Microcredit Pvt. Ltd.</font></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><font size='3'></font></td></tr>");
            DataGrid1.DataSource = dt1;
            DataGrid1.DataBind();
            DataGrid1.RenderControl(htw);
            //htw.WriteLine("<tr><td align=right' colspan='6'><b><u><font size='3'>Total                              " + dt.Rows[0]["TotTransAmt"].ToString() + "</font></u></b></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr><br/>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Thanking you.</font></td></tr>");
            htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr><br/>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Yours faithfully,</font></td></tr>");
            htw.WriteLine("</table>");

            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(sw.ToString());
            Response.End();

            //-----------------------

            //tdx.Controls.Add(DataGrid1);
            //tdx.Visible = false;
            //vFileNm = "attachment;filename=TRF_NEFT_LETTER.xls";
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //htw.WriteLine("<table cellpadding='12' widht='100%'>");
            //htw.WriteLine("<tr><td align=center' colspan='10'><b><font size='5'>" + gblValue.CompName + "</font></b></td></tr>");
            //htw.WriteLine("<tr><td align=center' colspan='10'><b><u><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            //htw.WriteLine("<tr><td align=right' colspan='10'><b><font size='3'>Date : " + vDate + "</font></b></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><b><font size='3'>To,</font></b></td></tr>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><b><u><font size='3'>The Branch Manager,</font></u></b></td></tr>");
            //oAcGenLed = new CAcGenled();
            //ds = oAcGenLed.GetGenLedSubsidairyDtl(ddlBank.SelectedValue.ToString());
            //dt2 = ds.Tables[0];
            //if (dt2.Rows.Count > 0)
            //{

            //    strData2 = dt2.Rows[0]["Address1"].ToString();
            //    char[] separator2 = new char[] { ',' };
            //    string[] strSplitArr = strData2.Split(separator2);
            //    foreach (string arrStr in strSplitArr)
            //    {
            //        Response.Write(arrStr + "<br/>");
            //        htw.WriteLine("<tr><td align=left' colspan='10'><b><font size='3'>" + arrStr + "</font></b></td></tr>");
            //    }
            //    htw.WriteLine("<tr><td align=center' colspan='10'><b><u><font size='3'>Re: RTGS/NEFT of Fund From our Current Account No: " + dt2.Rows[0]["Phone"].ToString() + "</font></u></b></td></tr>");
            //}

            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Dear Sir,</font></td></tr>");
            //ds = oRpt.rptNEFTLetter(ddlBank.SelectedValue.ToString(), gblFuction.setDate(vDate));
            //dt = ds.Tables[0];
            //dt1 = ds.Tables[1];
            //htw.WriteLine("<tr><td colspan='12'><font size='3'></font></td></tr>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Please transfer of Rs. " + dt.Rows[0]["TotTransAmt"].ToString() + " (Rupees " + dt.Rows[0]["NumToWord"].ToString() + ") only through NEFT RTGS </font></td></tr>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>from our Current Account No:  " + dt2.Rows[0]["Phone"].ToString() + " to the below mentioned accounts of  JAGARAN MICROFIN PVT LTD. </font></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><font size='3'></font></td></tr>");
            //DataGrid1.DataSource = dt1;
            //DataGrid1.DataBind();
            //DataGrid1.RenderControl(htw);
            ////htw.WriteLine("<tr><td align=right' colspan='6'><b><u><font size='3'>Total                              " + dt.Rows[0]["TotTransAmt"].ToString() + "</font></u></b></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr><br/>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Thanking you.</font></td></tr>");
            //htw.WriteLine("<tr><td colspan='12'><b><font size='3'></font></b></td></tr><br/>");
            //htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Yours faithfully,</font></td></tr>");
            //htw.WriteLine("</table>");

            //Response.ClearContent();
            //Response.AddHeader("content-disposition", vFileNm);
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            ////Response.ContentType = "application/vnd.oasis.opendocument.spreadsheet";
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Write(sw.ToString());
            //Response.End();
        }
        protected void chkDisb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
            CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCashCnt = vCashCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (chkDisbT.Checked == true)
                {
                    chkAppFrCashDisbT.Checked = false;
                    ChkCancelT.Checked = false;
                    chkAppFrCashDisbT.Enabled = false;
                    ChkCancelT.Enabled = false;
                    ChkPostponeT.Enabled = false;
                    ChkPostponeT.Checked = false;
                }
                else
                {
                    chkAppFrCashDisbT.Enabled = true;
                    ChkCancelT.Enabled = true;
                    ChkPostponeT.Enabled = true;
                }

            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtTotCash.Text = Convert.ToString(vTotCash);
            txtCashCnt.Text = Convert.ToString(vCashCnt);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();


        }
        protected void chkAppFrCashDisb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
            CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
                TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
                TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCashCnt = vCashCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    chkDisbT.Checked = false;
                    ChkCancelT.Checked = false;
                    chkDisbT.Enabled = false;
                    ChkCancelT.Enabled = false;
                    ChkPostponeT.Enabled = false;
                    ChkPostponeT.Checked = false;
                    txtCashRemark.Enabled = true;
                    txtRemarks.Enabled = false;
                    txtPostRemark.Enabled = false;
                }
                else
                {
                    chkDisbT.Enabled = true;
                    ChkCancelT.Enabled = true;
                    ChkPostponeT.Enabled = true;
                    txtCashRemark.Text = "";
                    txtCashRemark.Enabled = false;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtTotCash.Text = Convert.ToString(vTotCash);
            txtCashCnt.Text = Convert.ToString(vCashCnt);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();

        }
        protected void ChkCancel_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
            CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
                TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
                TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCashCnt = vCashCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    chkDisbT.Checked = false;
                    chkAppFrCashDisbT.Checked = false;
                    chkDisbT.Enabled = false;
                    chkAppFrCashDisbT.Enabled = false;
                    ChkPostponeT.Enabled = false;
                    ChkPostponeT.Checked = false;
                    txtRemarks.Enabled = true;
                    txtPostRemark.Enabled = false;
                    txtCashRemark.Enabled = false;
                }
                else
                {
                    chkDisbT.Enabled = true;
                    txtRemarks.Text = "";
                    chkAppFrCashDisbT.Enabled = true;
                    ChkPostponeT.Enabled = true;
                    txtRemarks.Enabled = false;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtTotCash.Text = Convert.ToString(vTotCash);
            txtCashCnt.Text = Convert.ToString(vCashCnt);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();

        }
        protected void btnPrn_Click(object sender, EventArgs e)
        {
            GenerateReport();
            btnSave.Enabled = true;
            ddlBank.Enabled = true;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtDt.Text.Trim() != "")
            {
                if (gblFuction.IsDate(txtDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtDt");
                }
                else
                {
                    LoadGrid(0);
                    SetTotal();
                    txtDt.Enabled = false;
                }
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
                }

            }
            finally
            {
                dt = null;
                oUsr = null;
            }
        }
        protected void ChkPostpone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
            CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            CheckBox ChkPostpone = (CheckBox)row.FindControl("ChkPostpone");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
                TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
                TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCashCnt = vCashCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    chkAppFrCashDisbT.Checked = false;
                    ChkCancelT.Checked = false;
                    chkAppFrCashDisbT.Enabled = false;
                    ChkCancelT.Enabled = false;
                    chkDisbT.Checked = false;
                    chkDisbT.Enabled = false;
                    txtPostRemark.Enabled = true;
                    txtCashRemark.Enabled = false;
                    txtRemarks.Enabled = false;
                }
                else
                {
                    chkAppFrCashDisbT.Enabled = true;
                    ChkCancelT.Enabled = true;
                    chkDisbT.Enabled = true;
                    txtPostRemark.Text = "";
                    txtPostRemark.Enabled = false;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtTotCash.Text = Convert.ToString(vTotCash);
            txtCashCnt.Text = Convert.ToString(vCashCnt);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();

        }
        public void SetTotal()
        {
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (chkAppFrCashDisbT.Checked == true)
                {
                    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCashCnt = vCashCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtTotCash.Text = Convert.ToString(vTotCash);
            txtCashCnt.Text = Convert.ToString(vCashCnt);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();

        }
        public void PopIcMst()
        {


        }
    }
}