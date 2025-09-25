using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Data;
using System.Web.Security;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanRecovaryAdjastment : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtTranDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                GetCashBank();
                LoadGrid();
                PopMember();
                tbMem.ActiveTabIndex = 0;
            }
        }

        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode] == null)
                {
                    Session.Abandon();
                    FormsAuthentication.SignOut();
                    Session.RemoveAll();
                    Response.Redirect("~/Login.aspx?e=random");
                }

                this.Menu = false;
                this.PageHeading = "Loan Recovery Adjustment";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1)
                {
                    return;
                }

                this.GetModuleByRole(mnuID.mnuLoanRecovaryAddj);
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;
                    btnDelete.Visible = false;
                    btnAdd.Visible = false;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    btnAdd.Visible = true;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;
                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                    btnSave.Visible = true;
                    btnDelete.Visible = true;
                    btnAdd.Visible = true;
                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Recovery Adjustment", true);
                }

            }
            catch
            {
                gblFuction.MsgPopup("Set Proper Role Permission From Assign Role Module.");
            }
            finally
            {
                //nothing to do here
            }
        }

        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    clearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    clearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void clearControls()
        {
            try
            {
                foreach (var c in tbMem.Controls[0].Controls[0].Controls) /*CLEAR LIST TAB CONTROLS*/
                {
                    var textbox = c as TextBox;
                    if (textbox != null)
                    {
                        textbox.Text = string.Empty;
                    }
                    var dropDownList = c as DropDownList;
                    if (dropDownList != null)
                    {
                        if (dropDownList.Items.Count > 0)
                        {
                            dropDownList.SelectedIndex = -1;
                        }
                    }
                    var checkbox = c as CheckBox;
                    if (checkbox != null)
                    {
                        checkbox.Checked = false;
                    }
                    var radiobutton = c as RadioButton;
                    if (radiobutton != null)
                    {
                        radiobutton.Checked = false;
                    }
                    var rblist = c as RadioButtonList;
                    if (rblist != null)
                    {
                        rblist.ClearSelection();
                    }
                }

                foreach (var c in tbMem.Controls[1].Controls[0].Controls) /*CLEAR DETAILS TAB CONTROLS*/
                {
                    var textbox = c as TextBox;
                    if (textbox != null)
                    {
                        textbox.Text = string.Empty;
                    }
                    var dropDownList = c as DropDownList;
                    if (dropDownList != null)
                    {
                        if (dropDownList.Items.Count > 0)
                        {
                            dropDownList.SelectedIndex = -1;
                        }
                    }
                    var checkbox = c as CheckBox;
                    if (checkbox != null)
                    {
                        checkbox.Checked = false;
                    }
                    var radiobutton = c as RadioButton;
                    if (radiobutton != null)
                    {
                        radiobutton.Checked = false;
                    }
                    var rblist = c as RadioButtonList;
                    if (rblist != null)
                    {
                        rblist.ClearSelection();
                    }
                }
                ddlLoanNo.Items.Clear();
                txtLoanDt.Text = "";
                txtLoanAmt.Text = "";
                txtPOS.Text = "";
                txtIOS.Text = "";
                txtTOS.Text = "";
                txtPrnAmt.Text = "";
                txtInsAmt.Text = "";
                txtTotAdj.Text = "";
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtTranDt.Text = Session[gblValue.LoginDate].ToString();
                gvWoff.DataSource = null;
                gvWoff.DataBind();
                gvSchdl.DataSource = null;
                gvSchdl.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } //clear all similar type of controls at the same time

        private void EnableControl(bool Status)
        {
            //txtCustomerName.Enabled = Status;
            ddlMem.Enabled = Status;
            txtPrnAmt.Enabled = Status;
            ddlLedgerP.Enabled = Status;
            txtTotAdj.Enabled = false;
            txtInsAmt.Enabled = Status;
            ddlLoanNo.Enabled = Status;
            ddlRO.Enabled = Status;
            ddlCenter.Enabled = Status;
            ddlMem.Enabled = Status;
            ddlGroup.Enabled = Status;
            ddlLedgerP.Enabled = Status;
        }

        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(txtTranDt.Text);
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlRO.DataSource = dt;
                ddlRO.DataTextField = "EoName";
                ddlRO.DataValueField = "EoId";
                ddlRO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlRO.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        protected void ddlRO_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vRoId = "";
            vRoId = Convert.ToString(ddlRO.SelectedValue);
            //PopCenter(vRoId);
            //PopGroup(vCentId);
        }

        //private void PopCenter(string vCOID)
        //{
        //    DataTable dtGr = null;
        //    CLoanRecovery oCL = null;
        //    try
        //    {
        //        ddlCenter.Items.Clear();
        //        ddlGroup.Items.Clear();
        //        ddlMem.Items.Clear();
        //        ddlLoanNo.Items.Clear();
        //        string vBrCode;
        //        vBrCode = Session[gblValue.BrnchCode].ToString();

        //        oCL = new CLoanRecovery();
        //        dtGr = oCL.PopCenterWithCollDay(vCOID, gblFuction.setDate(txtTranDt.Text), vBrCode, "W"); //With CollDay
        //        dtGr.AcceptChanges();
        //        ddlCenter.DataSource = dtGr;
        //        ddlCenter.DataTextField = "Market";
        //        ddlCenter.DataValueField = "MarketID";
        //        ddlCenter.DataBind();
        //        ListItem oLi = new ListItem("<--Select-->", "-1");
        //        ddlCenter.Items.Insert(0, oLi);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        dtGr = null;
        //        oCL = null;
        //    }
        //}

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vCentId = "";
            vCentId = Convert.ToString(ddlCenter.SelectedValue);
            PopGroup(vCentId);
        }

        private void PopGroup(string vCenterID)
        {
            ddlGroup.Items.Clear();
            ddlMem.Items.Clear();
            ddlLoanNo.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "GroupID", "GroupName", "GroupMst", vCenterID, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "GroupID";
                ddlGroup.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlGroup.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        protected void ddlGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vGropId = "";
            vGropId = ddlGroup.SelectedValue.ToString();
            PopMember();
        }

        private void PopMember()
        {
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CMember oMem = null;
            string vBrCode = "";

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oMem = new CMember();
                dt = oMem.GetMemListByBranchCode(vBrCode);
                ddlMem.DataSource = dt;
                ddlMem.DataTextField = "MemberName";
                ddlMem.DataValueField = "MemberID";
                ddlMem.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlMem.Items.Insert(0, oli);
            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }

        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string vMemId = ddlMem.SelectedValue;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CMember oMem = null;
            ddlLoanNo.Items.Clear();
            try
            {
                oMem = new CMember();
                dt = oMem.GetMemberDtlByMemberNo(vMemId, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    PopLoan(vMemId);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void PopLoan(string pMemberID)
        {
            ddlLoanNo.Items.Clear();
            DataTable dt = null;
            CLoan oLn = null;
            string vBrCode = "";

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime logindate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                oLn = new CLoan();
                dt = oLn.GetLoanByMemId(pMemberID, vBrCode, logindate);
                ddlLoanNo.DataSource = dt;
                ddlLoanNo.DataTextField = "LoanNo";
                ddlLoanNo.DataValueField = "LoanId";
                ddlLoanNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoanNo.Items.Insert(0, oli);
            }
            finally
            {
                oLn = null;
                dt = null;
            }
        }

        private void GetCashBank()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;

            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oVoucher = new CVoucher();

                dt = oVoucher.GetAcGenLedCB(vBrCode, "G", "");

                if (dt.Rows.Count > 0)
                {
                    ddlLedgerP.DataSource = dt;
                    ddlLedgerP.DataTextField = "Desc";
                    ddlLedgerP.DataValueField = "DescId";
                    ddlLedgerP.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlLedgerP.Items.Insert(0, liSel);
                    ddlLedgerP.SelectedIndex = 0;
                }
                else
                {
                    gblFuction.AjxMsgPopup("No General Ledger Found for Entry");
                }
            }
            finally
            {
                oVoucher = null;
                dt.Dispose();
            }
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CLoan oMem = null;
            string vBrCode = string.Empty;

            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oMem = new CLoan();
                dt = oMem.GetMemberLoanCollAdjList(vFrmDt, vToDt, vBrCode, txtSearch.Text.Replace("'", "''"));
                gvWoff.DataSource = dt;
                gvWoff.DataBind();
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }

        private void LoadDetails(string vLoanId)
        {
            CLoan oApp = null;
            DataSet ds = null;
            DataTable dt, dt1;

            try
            {
                oApp = new CLoan();

                ds = oApp.GetLoanDtlByMemberIDLA(vLoanId, "LA1", gblFuction.setDate(txtTranDt.Text)); // LO - Loan Offsetting

                dt = ds.Tables[0];
                dt1 = ds.Tables[1];

                if (dt.Rows.Count > 0)
                {
                    ViewState["LoanId"] = dt.Rows[0]["LoanId"].ToString();
                    txtLoanNo.Text = dt.Rows[0]["LoanNo"].ToString();
                    txtLoanAmt.Text = dt.Rows[0]["LoanAmt"].ToString();
                    txtLoanDt.Text = dt.Rows[0]["LoanDt"].ToString();
                    ViewState["LoanTypeId"] = dt.Rows[0]["LoanTypeId"].ToString();
                    txtPOS.Text = dt.Rows[0]["POS"].ToString();
                    txtIOS.Text = dt.Rows[0]["IOS"].ToString();
                    txtTOS.Text = Convert.ToString(Convert.ToDouble(txtPOS.Text) + Convert.ToDouble(txtIOS.Text));
                    txtPrnAmt.Text = "0";
                    txtInsAmt.Text = "0";
                }
                else
                {
                    ViewState["LoanId"] = null;
                    txtLoanNo.Text = "0";
                    txtLoanAmt.Text = "0";
                    txtLoanDt.Text = "0";
                    txtPOS.Text = "0";
                    txtIOS.Text = "0";
                    txtTOS.Text = "0";
                    txtPrnAmt.Text = "0";
                    txtInsAmt.Text = "0";
                }
                if (dt1.Rows.Count > 0)
                {
                    gvSchdl.DataSource = dt1;
                    gvSchdl.DataBind();
                }
                else
                {
                    gvSchdl.DataSource = null;
                    gvSchdl.DataBind();
                }

            }
            finally
            {
                oApp = null;
                ds = null;
                dt = null;
                dt1 = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = string.Empty, vOffSetId = string.Empty, vMemberId = "", vLoanid = "", vLedgerAc = "", vIntAc = "", vLoanAc = "";
            Int32 vErr = 0;
            Int32 vAcType = 1;
            DateTime vTranDt = gblFuction.setDate(txtTranDt.Text);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            CLoan oLn = null;
            Double vPOffSet = 0.0, vIOffSet = 0.0;

            try
            {
                if (Convert.ToInt32(Session[gblValue.UserId]) != 1)//&& Convert.ToInt32(Session[gblValue.UserId]) != 505
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate(txtTranDt.Text))
                        {
                            gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                            return false;
                        }
                    }
                }

                vMemberId = Convert.ToString(ddlMem.SelectedValue);

                vLoanid = Convert.ToString(ViewState["LoanId"]);

                if (vLoanid == "")
                {
                    gblFuction.AjxMsgPopup("No Loan Selected...");
                    return false;
                }

                vPOffSet = Convert.ToDouble(txtPrnAmt.Text);
                vIOffSet = Convert.ToDouble(txtInsAmt.Text);

                CGblIdGenerator oGbl = null;
                oGbl = new CGblIdGenerator();

                vLedgerAc = ddlLedgerP.SelectedValue;
                vLoanAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, Convert.ToInt32(ViewState["LoanTypeId"]), vBrCode);
                vIntAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.LoanIntAccruedAc, Convert.ToInt32(ViewState["LoanTypeId"]), vBrCode);


                if (Mode == "Save")
                {
                    oLn = new CLoan();
                    vErr = oLn.SaveMemberLoanCollectionAdj(vLoanid, "LA", vMemberId, vLedgerAc, vLoanAc, vIntAc, vPOffSet, vIOffSet, vTblMst, vTblDtl, vFinYear, vTranDt, vBrCode, Convert.ToInt32(Session[gblValue.UserId]), "Save", vAcType);

                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else if (vErr == 2)
                    {
                        gblFuction.MsgPopup("Transaction Date Should be Greater equal to than Last Collection Date.");
                        vResult = false;
                    }
                    else if (vErr == 3)
                    {
                        gblFuction.MsgPopup("For Closed Loan Adjustment Not Possible.");
                        vResult = false;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oLn = new CLoan();
                    vOffSetId = Convert.ToString(ViewState["OffSetId"]); //need to check

                    vErr = oLn.SaveMemberLoanCollectionAdj(vLoanid, vOffSetId, vMemberId, vLedgerAc, vLoanAc, vIntAc, vPOffSet, vIOffSet, vTblMst, vTblDtl, vFinYear, vTranDt, vBrCode, Convert.ToInt32(Session[gblValue.UserId]), "Delet", vAcType);

                    if (vErr > 0)
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
            finally
            {
                oLn = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        //protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlMem.SelectedIndex > 0)
        //    {
        //        PopLoan(ddlMem.SelectedValue);
        //    }
        //}

        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetails(ddlLoanNo.SelectedValue.ToString());
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtTranDt.Text = Session[gblValue.LoginDate].ToString();
                PopRO();
                ViewState["StateEdit"] = null;
                tbMem.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    LoadGrid();
                    StatusButton("Show");
                    tbMem.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gvWoff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vOffSetId = "";
            DataSet ds = null;
            DataTable dt, dt1, dt2;
            CLoan oLn = null;
            try
            {
                vOffSetId = Convert.ToString(e.CommandArgument);
                ViewState["OffSetId"] = vOffSetId;

                if (e.CommandName == "cmdShow")
                {
                    oLn = new CLoan();
                    ds = oLn.GetMemberLoanCollAdjDetails(vOffSetId);
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    //dt2 = ds.Tables[2];
                    if (dt.Rows.Count > 0)
                    {
                        //hdnCustomerId.Value = dt.Rows[0]["CustomerId"].ToString();
                        //txtCustomerName.Text = dt.Rows[0]["CustName"].ToString();
                        txtTranDt.Text = dt1.Rows[0]["AccDate"].ToString();
                        // PopRO();
                        // ddlRO.SelectedIndex = ddlRO.Items.IndexOf(ddlRO.Items.FindByValue(dt.Rows[0]["EoId"].ToString()));
                        // PopCenter(dt.Rows[0]["EoId"].ToString());
                        // ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        //PopGroup(dt.Rows[0]["MarketID"].ToString());
                        //ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(dt.Rows[0]["GroupId"].ToString()));
                        PopMember();
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(dt.Rows[0]["MemberId"].ToString()));
                        PopLoan(ddlMem.SelectedValue);
                        ddlLoanNo.SelectedIndex = ddlLoanNo.Items.IndexOf(ddlLoanNo.Items.FindByValue(dt.Rows[0]["LoanId"].ToString()));
                        ddlLedgerP.SelectedIndex = ddlLedgerP.Items.IndexOf(ddlLedgerP.Items.FindByValue(dt.Rows[0]["DescId"].ToString()));
                        ViewState["LoanId"] = dt.Rows[0]["LoanId"].ToString();
                        txtLoanNo.Text = dt.Rows[0]["LoanNo"].ToString();
                        txtLoanAmt.Text = dt.Rows[0]["LoanAmt"].ToString();
                        txtLoanDt.Text = dt.Rows[0]["LoanDt"].ToString();
                        ViewState["LoanTypeId"] = dt.Rows[0]["LoanTypeId"].ToString();
                        txtPOS.Text = dt.Rows[0]["POS"].ToString();
                        txtIOS.Text = dt.Rows[0]["IOS"].ToString();
                        txtTOS.Text = Convert.ToString(Convert.ToDouble(txtPOS.Text) + Convert.ToDouble(txtIOS.Text));
                        txtPrnAmt.Text = dt.Rows[0]["POffSet"].ToString();
                        txtInsAmt.Text = dt.Rows[0]["IOffSet"].ToString();
                        txtTotAdj.Text = Convert.ToString(Convert.ToDouble(txtPrnAmt.Text) + Convert.ToDouble(txtInsAmt.Text));
                        tbMem.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvSchdl.DataSource = dt1;
                        gvSchdl.DataBind();
                    }
                    else
                    {
                        gvSchdl.DataSource = dt1;
                        gvSchdl.DataBind();
                    }
                }
            }
            finally
            {
                dt = null;
                oLn = null;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //nothing to do
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                EnableControl(false);
                StatusButton("View");
                tbMem.ActiveTabIndex = 0;
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
                if ((Convert.ToInt32(Session[gblValue.UserId]) != 1) && (Convert.ToBoolean((this.CanDelete == "") ? "false" : this.CanDelete == "Y" ? "true" : "false") == false))
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    LoadGrid();
                    clearControls();
                    StatusButton("Delete");
                    tbMem.ActiveTabIndex = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}