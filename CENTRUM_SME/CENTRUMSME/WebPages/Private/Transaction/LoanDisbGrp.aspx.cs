using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using SendSms;
using System.Web.UI;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using System.Collections.Generic;
using System.Configuration;

namespace CENTRUMSME.WebPages.Private.Transaction
{

    public partial class LoanDisbGrp : CENTRUMBAse
    {
        string vPathImage = "", vPathHDrive = "", vPathNetworkDrive1 = "", vPathNetworkDrive2 = "", vPathNetworkDrive = "";
        protected int cPgNo = 1;
        protected int vFlag = 0;
        string vJocataToken = ConfigurationManager.AppSettings["JocataToken"];
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                Session["CheckRefresh"] = Server.UrlDecode(System.DateTime.Now.ToString());
                //////hdUserID.Value = this.UserID.ToString();
                //////hdAdmsnDt.Value = Session[gblValue.LoginDate].ToString();
                if (vBrCode == "0000")
                {
                    PopBranch();
                    ddlBranch.Visible = true;

                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("BranchName", typeof(string));
                    dt.Columns.Add("BranchCode", typeof(string));
                    dt.Rows.Add();
                    dt.Rows[0]["BranchName"] = Session[gblValue.BrName].ToString();
                    dt.Rows[0]["BranchCode"] = Session[gblValue.BrnchCode].ToString();
                    dt.AcceptChanges();

                    if (dt.Rows.Count > 0)
                    {
                        ddlPendingBranch.DataSource = dt;
                        ddlPendingBranch.DataTextField = "BranchName";
                        ddlPendingBranch.DataValueField = "BranchCode";
                        ddlPendingBranch.DataBind();
                    }
                }
                popCustomer();
                PopPreLnBalLed();
                LoadBankLedger();
                LoadAcGenLed();
                PopLoanType();
                PopPurpose();
                PopFunder();
                txtFrmDt.Text = (string)Session[gblValue.LoginDate];
                txtToDt.Text = (string)Session[gblValue.LoginDate];
                txtPendingFrmDt.Text = txtPendingToDt.Text = (string)Session[gblValue.LoginDate];
                CalendarExtender3.EndDate = gblFuction.setDate((string)Session[gblValue.LoginDate]);
                // txtLnDt.Text = (string)Session[gblValue.LoginDate];
                LoadPendingDisbList(1);
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tabLoanDisb.ActiveTabIndex = 0;
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                //string pMemberID = "", pLoanId = "";
                // Prosidex(pMemberID, pLoanId, 1);
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Disbursement";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanDisbursement);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Disbursement", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ViewState["CheckRefresh"] = Session["CheckRefresh"];
        }
        private void LoadAcGenLed()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            // dt = oVoucher.GetAcGenLedCB(vBranch, "G", "");
            dt = oVoucher.PopDisbBank();
            ddlLedgr.DataSource = dt;
            ddlLedgr.DataTextField = "Desc";
            ddlLedgr.DataValueField = "DescId";
            ddlLedgr.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlLedgr.Items.Insert(0, liSel);
        }
        private void PopPreLnBalLed()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vAssets = 1;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vAssets);
            ListItem Lst1 = new ListItem();
            Lst1.Text = "<--- Select --->";
            Lst1.Value = "-1";
            //ddlLnBalLed.DataTextField = "Desc";
            //ddlLnBalLed.DataValueField = "DescId";
            //ddlLnBalLed.DataSource = dt;
            //ddlLnBalLed.DataBind();
            //ddlLnBalLed.Items.Insert(0, Lst1);
        }
        protected void gvLoanSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    if (gvRow.Cells[9].Text.Trim() == "Y")
                    {
                        clearNetOff();
                        oCG = new CApplication();
                        dt = oCG.GetFinalSanctionDtlBySanctId(vSanId);
                        if (dt.Rows.Count > 0)
                        {
                            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                            System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                            System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                            foreach (GridViewRow gr in gvLoanSanc.Rows)
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
                                LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                                lb.ForeColor = System.Drawing.Color.Black;
                                lb.Font.Bold = false;
                            }
                            gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                            gvRow.ForeColor = System.Drawing.Color.White;
                            gvRow.Font.Bold = true;
                            btnShow.ForeColor = System.Drawing.Color.White;
                            btnShow.Font.Bold = true;

                            ViewState["StateEdit"] = "Add";
                            StatusButton("Add");
                            ClearControls();
                            PopFunder();
                            popCustomer();
                            PopBranch();
                            ddlBranch.Items.Clear();
                            ddlBranch.DataSource = null;
                            ddlBranch.DataBind();
                            ListItem liSel = new ListItem();
                            liSel.Text = dt.Rows[0]["BranchName"].ToString();
                            liSel.Value = dt.Rows[0]["BranchCode"].ToString();
                            ddlBranch.Items.Insert(0, liSel);
                            ddlCust.SelectedIndex = ddlCust.Items.IndexOf(ddlCust.Items.FindByValue(dt.Rows[0]["CustID"].ToString()));
                            popSanctionNoForDisb(dt.Rows[0]["CustID"].ToString());
                            tabLoanDisb.ActiveTabIndex = 2;
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("You can not disbursed this loan because Digital signature is pending.");
                        return;
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPendingDisbList(1);
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            vBrCode = (string)Session[gblValue.BrnchCode];
            string vSearch = txtSearch.Text.Trim();
            DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
            DateTime vToDt = gblFuction.setDate(txtToDt.Text);
            oLD = new CDisburse();
            dt = oLD.GetDisburseList(vSearch, vFrmDt, vToDt, vBrCode, pPgIndx, ref vRows);
            gvLoanAppl.DataSource = dt.DefaultView;
            gvLoanAppl.DataBind();
            lblTotalPages.Text = CalTotPgs(vRows).ToString();
            lblCurrentPage.Text = cPgNo.ToString();
            if (cPgNo == 0)
            {
                Btn_Previous.Enabled = false;
                if (Int32.Parse(lblTotalPages.Text) > 1)
                    Btn_Next.Enabled = true;
                else
                    Btn_Next.Enabled = false;
            }
            else
            {
                Btn_Previous.Enabled = true;
                if (cPgNo == Int32.Parse(lblTotalPages.Text))
                    Btn_Next.Enabled = false;
                else
                    Btn_Next.Enabled = true;
            }
            CalculateTotal();
        }
        private void LoadPendingDisbList(int cPgNo)
        {
            DataTable dt = null;
            CApplication oCA = null;
            Int32 vRows = 0;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = Session[gblValue.BrnchCode].ToString() == "0000" ? ddlPendingBranch.SelectedValues.Replace("|", ",") : Session[gblValue.BrnchCode].ToString();

                oCA = new CApplication();
                dt = oCA.GetPendingDisbList(cPgNo, ref vRows, gblFuction.setDate(txtPendingFrmDt.Text), gblFuction.setDate(txtPendingToDt.Text), vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvLoanSanc.DataSource = dt;
                    gvLoanSanc.DataBind();

                    lblTotalPage.Text = CalTotPgs(vRows).ToString();
                    lblCurrPage.Text = cPgNo.ToString();
                    if (cPgNo == 0)
                    {
                        BtnPrevious.Enabled = false;
                        if (Int32.Parse(lblTotalPage.Text) > 1)
                            BtnNext.Enabled = true;
                        else
                            BtnNext.Enabled = false;
                    }
                    else
                    {
                        BtnPrevious.Enabled = true;
                        if (cPgNo == Int32.Parse(lblTotalPage.Text))
                            BtnNext.Enabled = false;
                        else
                            BtnNext.Enabled = true;
                    }
                }
                else
                {
                    gvLoanSanc.DataSource = null;
                    gvLoanSanc.DataBind();

                    lblTotalPage.Text = CalTotPgs(vRows).ToString();
                    lblCurrPage.Text = cPgNo.ToString();
                    if (cPgNo == 0)
                    {
                        BtnPrevious.Enabled = false;
                        if (Int32.Parse(lblTotalPage.Text) > 1)
                            BtnNext.Enabled = true;
                        else
                            BtnNext.Enabled = false;
                    }
                    else
                    {
                        BtnPrevious.Enabled = true;
                        if (cPgNo == Int32.Parse(lblTotalPage.Text))
                            BtnNext.Enabled = false;
                        else
                            BtnNext.Enabled = true;
                    }
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void CalculateTotal()
        {
            double DrAmt = 0.00;
            for (Int32 i = 0; i < gvLoanAppl.Rows.Count; i++)
            {
                DrAmt = Math.Round(DrAmt, 2) + Math.Round(Convert.ToDouble(gvLoanAppl.Rows[i].Cells[4].Text), 2);
            }
            txtDisAmt.Text = Convert.ToString(DrAmt);

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
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
                case "GoTo":
                    if (Int32.Parse(txtGotoPg.Text) <= Int32.Parse(lblCurrentPage.Text))
                        cPgNo = Int32.Parse(txtGotoPg.Text);
                    break;
            }
            LoadGrid(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
        }
        private void LoadBankLedger()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            dt = oVoucher.GetAcGenLedCB(vBranch, "B", "");
            ddlBank.DataSource = dt;
            ddlBank.DataTextField = "Desc";
            ddlBank.DataValueField = "DescId";
            ddlBank.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlBank.Items.Insert(0, liSel);
        }
        private void PopPurpose()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlPurps.Items.Clear();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "PurposeID", "PurposeName", "PurposeMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlPurps.DataSource = dt;
                ddlPurps.DataTextField = "PurposeName";
                ddlPurps.DataValueField = "PurposeId";
                ddlPurps.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlPurps.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        private void PopFunder()
        {
            DataTable dt = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlSrcFund.Items.Clear();
                ddlSrcFund.DataSource = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                oGbl = new CGblIdGenerator();
                dt = oGbl.PopComboMIS("N", "N", "AA", "FunderId", "FunderName", "FunderMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                ddlSrcFund.DataSource = dt;
                ddlSrcFund.DataTextField = "FunderName";
                ddlSrcFund.DataValueField = "FunderId";
                ddlSrcFund.DataBind();
                ddlSrcFund.Items.Insert(0, oLi);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oGbl = null;
            }
        }
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            CLoanScheme oLS = new CLoanScheme();
            dt = oLS.GetLoanTypePop(vBrCode, vLogDate, "");
            ddlLoanType.DataTextField = "LoanTypeName";
            ddlLoanType.DataValueField = "LoanTypeId";
            ddlLoanType.DataSource = dt;
            ddlLoanType.DataBind();
            ListItem oItm = new ListItem();
            oItm.Text = "<--- Select --->";
            oItm.Value = "-1";
            ddlLoanType.Items.Insert(0, oItm);
        }

        //protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //////string vMarketId = ddlCenter.SelectedItem.Value;
        //    //////PopGroup(vMarketId);
        //    ViewState["LoanId"] = null;
        //    string vLoanId = "";
        //    PopYbLoanBal(vLoanId);
        //}

        //private void PopGroup(string vMarketId)
        //{
        //    DataTable dt = null;
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    DateTime vAdmDt = gblFuction.setDate(txtLnDt.Text);
        //    CCenter oCent = null;
        //    try
        //    {
        //        oCent = new CCenter();
        //        dt = oCent.GetMeetingDayByCenterId(vMarketId, vBrCode, vAdmDt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlGrp.DataSource = dt;
        //            ddlGrp.DataTextField = "GroupName";
        //            ddlGrp.DataValueField = "Groupid";
        //            ddlGrp.DataBind();
        //            ListItem oli = new ListItem("<--Select-->", "-1");
        //            ddlGrp.Items.Insert(0, oli);
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oCent = null;
        //    }
        //}
        protected void btbSchedul_Click(object sender, EventArgs e)
        {
            //string vMsg = ValidateField();
            //if (vMsg != "")
            //{
            //    gblFuction.AjxMsgPopup(vMsg);
            //    return;
            //}
            Int32 vCollDay = 0, vCollDayNo = 0, vCollType = 0;
            CCollectionRoutine oCR = null;
            DataTable dt = null;
            if (Request[ddlSancNo.UniqueID] as string == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select the Sanction No...");
                return;
            }

            //txtFlatIntRate
            Int32 vLoanTypeID = Convert.ToInt32((Request[ddlLoanType.UniqueID] as string == null) ? ddlLoanType.SelectedValue : Request[ddlLoanType.UniqueID] as string);
            decimal vLoanAmt = Convert.ToDecimal(Request[txtLnAmt.UniqueID] as string);
            decimal vInstRate = Convert.ToDecimal(Request[txtIntRate.UniqueID] as string);

            decimal vFInstRate = Convert.ToDecimal(Request[txtFlatIntRate.UniqueID] as string);

            Int32 vInstallNo = Convert.ToInt32(Request[txtInstNo.UniqueID] as string);
            Int32 vInstPeriod = Convert.ToInt32(Request[txtIntPeriod.UniqueID] as string);
            DateTime vStartDt = gblFuction.setDate(Request[txtStDt.UniqueID] as string);
            DateTime vLoanDt = gblFuction.setDate(Request[txtLnDt.UniqueID] as string);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vPaySchedule = (Request[ddlRpSchdle.UniqueID] as string == null) ? ddlRpSchdle.SelectedValue : Request[ddlRpSchdle.UniqueID] as string;
            string vSanctionId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            Int32 vFrDueday = Convert.ToInt32((gblFuction.setDate(Request[txtStDt.UniqueID] as string) - gblFuction.setDate(Request[txtLnDt.UniqueID] as string)).TotalDays);

            GetSchedule(vLoanTypeID, vLoanAmt, vFInstRate, vInstRate, vInstallNo, vInstPeriod, vStartDt, "L", "", "N", vBrCode, vPaySchedule, "", "", vCollDay, vCollDayNo, "G", vFrDueday, "", vCollType, vLoanDt, vSanctionId);
            tabLoanDisb.ActiveTabIndex = 2;

        }
        private void GetSchedule(Int32 pLoanTypeID, decimal pLoanAmt, decimal pFInterest, decimal pInterest, Int32 pInstallNo, Int32 pInstPeriod,
                                 DateTime pStatrDt, string pType, string pLoanID, string pIsDisburse, string pBranch, string pPaySchedule,
                                 string pBank, string pChequeNo, Int32 pCollDay, Int32 pCollDayNo, string pLoanType, Int32 pFrDueday, string pPEType, Int32 vCollType, DateTime vLoanDt, string vSanctionId)
        {
            DataTable dt = null;
            CDisburse oLD = null;
            double VLoanInt = 0.0;
            oLD = new CDisburse();
            dt = oLD.GetSchedule(pLoanTypeID, pLoanAmt, pFInterest, pInterest, pInstallNo, pInstPeriod, pStatrDt, pType, pLoanID, pIsDisburse, pBranch, pPaySchedule, pBank, pChequeNo, pCollDay, pCollDayNo, pLoanType, pFrDueday, pPEType, vCollType, vLoanDt, vSanctionId);
            if (dt.Rows.Count > 0)
            {
                gvSchdl.DataSource = dt;
                gvSchdl.DataBind();
                VLoanInt = Convert.ToDouble(dt.Compute("Sum(InstAmt)", ""));
                txtIntAmt.Text = VLoanInt.ToString();
            }
            tabLoanDisb.ActiveTabIndex = 2;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //        if (this.CanAdd == "N")
                //        {
                //            gblFuction.MsgPopup(MsgAccess.Add);
                //            return;
                //        }
                //        ddlRpSchdle.Enabled = true;
                //        ddlLoanType.Enabled = true;
                //        ddlInstType.Enabled = true;
                ViewState["StateEdit"] = "Add";
                tabLoanDisb.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
                PopFunder();
                //        txtLnDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabLoanDisb.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToString(ViewState["IsMig"]) == "Y")
                {
                    gblFuction.MsgPopup("Migrated Loan Cannot be Deleted...");
                    return;
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecordsNew("Delete") == true)
                {
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
                if (Convert.ToString(ViewState["IsMig"]) == "Y")
                {
                    gblFuction.MsgPopup("Migrated Loan Cannot be Modified");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("You Can Not Edit Disbursed Loan... To update Disbursement Record kindly delete disburshment  and redisburse it... ");
                    return;
                }
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
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecordsNew(vStateEdit) == true)
            {
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadPendingDisbList(1);
                tabLoanDisb.ActiveTabIndex = 0;
            }
            else
            {
                ClearControls();
            }

        }
        private string ValidateField()
        {
            string vMsg = "";
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            if (gblFuction.IsDate((Request[txtLnDt.UniqueID] as string == null) ? txtLnDt.Text : Request[txtLnDt.UniqueID] as string) == false)
            {
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtLnDt");
                vMsg = "Loan Date Should be in DD/MM/YYYY Format ..";
            }
            DateTime vLnDate = gblFuction.setDate((Request[txtLnDt.UniqueID] as string == null) ? txtLnDt.Text : Request[txtLnDt.UniqueID] as string);
            if (vLnDate < vFinFrom || vLnDate > vFinTo)
            {
                EnableControl(true);
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtl_txtAppDt");
                vMsg = "Loan Disburse Date should be Financial Year.";
            }
            if (vLnDate > vLoginDt)
            {
                EnableControl(true);
                vMsg = "Loan Disburse Date should not be greater than login date.";
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtl_txtAppDt");
            }
            if (rdbPayMode.SelectedValue == "B")
            {
                if (gblFuction.IsDate(txtChqDt.Text) == false)
                {
                    vMsg = "Cheque Date Should be in DD/MM/YYYY Format ..";
                    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtChqDt");
                }

                if (ddlBank.SelectedValue == "-1")
                {
                    vMsg = "Please select the bank account.";
                    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlBank");
                }
            }
            if (rdbPayMode.SelectedValue == "J")
            {
                if (txtRefNo.Text == "")
                {
                    vMsg = "Please Input Reff No";
                    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtRefNo");
                }

                if (ddlLedgr.SelectedValue == "-1")
                {
                    vMsg = "Please select Ledger Account.";
                    gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlLedgr");
                }
            }
            if (ddlCust.SelectedIndex == -1) //ddlLoanType.SelectedValue == "-1"
            {
                vMsg = "Please select Customer Name";
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlCust");
            }
            if (ddlSancNo.SelectedIndex == -1) //ddlLoanType.SelectedValue == "-1"
            {
                vMsg = "Please select Sanction No";
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_ddlSancNo");
            }
            if (txtLnCycle.Text == "" || txtLnCycle.Text == "0") //ddlLoanType.SelectedValue == "-1"
            {
                vMsg = "Please input Loan Cycle No";
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtLnCycle");
            }
            if (txtStDt.Text == "")
            {
                vMsg = "Repayment Start Date Can Not Be Empty";
                gblFuction.focus("ctl00_cph_Main_tabLoanDisb_pnlDtls_txtStDt");
            }
            return vMsg;
        }
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }

        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            CMember cMem = null;
            string vSanctionID = "";
            CApplication oCG = null;
            foreach (GridViewRow gr in gvLoanSanc.Rows)
            {
                CheckBox chkSendBack = (CheckBox)gr.FindControl("chkSendBack");
                if (chkSendBack.Checked)
                {
                    vSanctionID = gr.Cells[14].Text;
                    cMem = new CMember();

                    oCG = new CApplication();
                    int vErr = oCG.SendBackHoDisb(vSanctionID, Convert.ToInt32(Session[gblValue.UserId]));
                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                        LoadPendingDisbList(1);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                    }
                }
            }
        }

        private Boolean SaveRecordsNew(string Mode)  //Check Account
        {
            DateTime pDishbDate = gblFuction.setDate((Request[txtLnDt.UniqueID] as string == null) ? txtLnDt.Text : Request[txtLnDt.UniqueID] as string);
            DateTime pStDate = gblFuction.setDate((Request[txtStDt.UniqueID] as string == null) ? txtStDt.Text : Request[txtStDt.UniqueID] as string);
            if (pDishbDate > pStDate)
            {
                gblFuction.AjxMsgPopup("Loan Disbursement Date Can Not Be Greater Than First Installment Date...");
                return false;
            }
            if (pDishbDate < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || pDishbDate > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
            {
                gblFuction.AjxMsgPopup("Loan Disbursement Date must be with in Login Financial Year");
                return false;
            }
            if (ddlSrcFund.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Fund Source can not be left blank.");
                return false;
            }
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //// string vMsg = ValidateField();
            // if (vMsg != "")
            // {
            //     gblFuction.MsgPopup(vMsg);
            //     return false;
            // }
            Boolean vResult = false;
            DataTable dt = null;


            Int32 vCollDay = 0, vCollDayNo = 0, vCollType = 0;
            string vTranscMode = "", vDishbMode = "", vReffNo = "", vReffDt = "", vReffLedgerAc = "", vLoanNo = "", vLoanId = "";
            Int32 vFunderID = 0, vErr = 0, vLoanTypeID = 0, pDisbSrl = 0, vInstallSize = 0, vCycle = 0, vInstalNo = 0;
            string pSchedule = "", vInstType = "", vCustID = "", vLoanAppID = "", vSanctionId = "", vPreLnBalLed = "", vPreLoanId = "",
                isTransDisburse = "N", vTrnsDisbAc = "", vNetOffYN = "N", VNetOffLoanId = "";
            Decimal TotLnAmt = 0, InstPeriod = 0, LPFStax = 0, LPFKKTax = 0, LPFSBTax = 0, InsuStax = 0, InsuCGST = 0, InsuSGST = 0, InsuIGST = 0,
                AppCharge = 0, StampCharge = 0, NetDisb = 0,
                EMI = 0, AdvEMI = 0, AdvEMIPric = 0, AdvEMIInt = 0, ProceFees = 0, InsuAmt = 0, IntRate = 0, FIntRate = 0, AdvInt = 0, PreLnBal = 0,
                CGSTAmt = 0, SGSTAmt = 0, FLDGAmt = 0, vTotCharge = 0,
                 vBrkPrdInt = 0, PreLnInt = 0, vBrkPrdIntAct = 0, vBrkPrdIntWave = 0, vDisburseAmt = 0,
                 vPropInsuAmt = 0, vPropInsuCGSTAmt = 0, vPropInsuSGSTAmt = 0, vPropInsuIGSTAmt = 0, vAdminFees = 0, vTechFees = 0,
                 vCERSAIAmt = 0, vCERSAIAmtCGST = 0, vCERSAIAmtSGST = 0, vCERSAIAmtIGST = 0, VNetOffPrincAmt = 0, VNetOffIntAmt = 0, VNetOffAdvAmt = 0;
            // string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = (Request[ddlBranch.UniqueID] as string == null) ? ddlBranch.SelectedValue : Request[ddlBranch.UniqueID] as string;
            string vXmlAC = string.Empty, vXmlFees = string.Empty;
            string vNarationL = string.Empty;
            string vNarationF = string.Empty;

            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            //hfDisbSrl.Value
            vCycle = Convert.ToInt32((Request[txtLnCycle.UniqueID] as string == null) ? txtLnCycle.Text : Request[txtLnCycle.UniqueID] as string);
            //if (((Request[hfDisbSrl.UniqueID] as string == null) ? hfDisbSrl.Value : Request[hfDisbSrl.UniqueID] as string) != "")
            //{
            //    pDisbSrl = Convert.ToInt32((Request[hfDisbSrl.UniqueID] as string == null) ? hfDisbSrl.Value : Request[hfDisbSrl.UniqueID] as string);
            //}
            pDisbSrl = 1;

            InstPeriod = Convert.ToDecimal((Request[txtIntPeriod.UniqueID] as string == null) ? txtIntPeriod.Text : Request[txtIntPeriod.UniqueID] as string);

            vInstalNo = Convert.ToInt32((Request[txtInstNo.UniqueID] as string == null) ? txtInstNo.Text : Request[txtInstNo.UniqueID] as string);

            ProceFees = Convert.ToDecimal((Request[txtProsFee.UniqueID] as string == null) ? txtProsFee.Text : Request[txtProsFee.UniqueID] as string);
            InsuAmt = Convert.ToDecimal((Request[txtInsuAmt.UniqueID] as string == null) ? txtInsuAmt.Text : Request[txtInsuAmt.UniqueID] as string);

            LPFStax = Convert.ToDecimal((Request[txtServiceTax.UniqueID] as string == null) ? txtServiceTax.Text : Request[txtServiceTax.UniqueID] as string);
            LPFKKTax = Convert.ToDecimal((Request[txtLPFKKTax.UniqueID] as string == null) ? txtLPFKKTax.Text : Request[txtLPFKKTax.UniqueID] as string);
            LPFSBTax = Convert.ToDecimal((Request[txtLPFSBTax.UniqueID] as string == null) ? txtLPFSBTax.Text : Request[txtLPFSBTax.UniqueID] as string);

            InsuStax = Convert.ToDecimal((Request[txtInsSerTax.UniqueID] as string == null) ? txtInsSerTax.Text : Request[txtInsSerTax.UniqueID] as string);
            InsuCGST = Convert.ToDecimal((Request[txtInsuCGSTAmt.UniqueID] as string == null) ? txtInsuCGSTAmt.Text : Request[txtInsuCGSTAmt.UniqueID] as string);
            InsuSGST = Convert.ToDecimal((Request[txtInsuSGSTAmt.UniqueID] as string == null) ? txtInsuSGSTAmt.Text : Request[txtInsuSGSTAmt.UniqueID] as string);
            InsuIGST = Convert.ToDecimal((Request[txtInsuIGSTAmt.UniqueID] as string == null) ? txtInsuIGSTAmt.Text : Request[txtInsuIGSTAmt.UniqueID] as string);


            AppCharge = Convert.ToDecimal((Request[txtAppCharge.UniqueID] as string == null) ? txtAppCharge.Text : Request[txtAppCharge.UniqueID] as string);
            StampCharge = Convert.ToDecimal((Request[txtStmpChrg.UniqueID] as string == null) ? txtStmpChrg.Text : Request[txtStmpChrg.UniqueID] as string);
            NetDisb = Convert.ToDecimal((Request[txtNetDisb.UniqueID] as string == null) ? txtNetDisb.Text : Request[txtNetDisb.UniqueID] as string);
            EMI = Convert.ToDecimal((Request[txtEMIAmt.UniqueID] as string == null) ? txtEMIAmt.Text : Request[txtEMIAmt.UniqueID] as string);
            AdvEMI = Convert.ToDecimal((Request[txtAdvEMI.UniqueID] as string == null) ? txtAdvEMI.Text : Request[txtAdvEMI.UniqueID] as string);
            AdvEMIPric = Convert.ToDecimal((Request[txtAdvEMIPrin.UniqueID] as string == null) ? txtAdvEMIPrin.Text : Request[txtAdvEMIPrin.UniqueID] as string);
            AdvEMIInt = Convert.ToDecimal((Request[txtAdvEMIInst.UniqueID] as string == null) ? txtAdvEMIInst.Text : Request[txtAdvEMIInst.UniqueID] as string);
            AdvInt = Convert.ToDecimal((Request[txtAdvInt.UniqueID] as string == null) ? txtAdvInt.Text : Request[txtAdvInt.UniqueID] as string);
            IntRate = Convert.ToDecimal((Request[txtIntRate.UniqueID] as string == null) ? txtIntRate.Text : Request[txtIntRate.UniqueID] as string);

            //FIntRate = Convert.ToDecimal((Request[txtFlatIntRate.UniqueID] as string == null) ? txtFlatIntRate.Text : Request[txtFlatIntRate.UniqueID] as string);
            TotLnAmt = Convert.ToDecimal((Request[txtLnAmt.UniqueID] as string == null) ? txtLnAmt.Text : Request[txtLnAmt.UniqueID] as string);
            PreLnBal = Convert.ToDecimal((Request[txtPreLnBal.UniqueID] as string == null) ? txtPreLnBal.Text : Request[txtPreLnBal.UniqueID] as string);
            decimal pPreLnInt = 0;
            CGSTAmt = Convert.ToDecimal((Request[txtLPFCGST.UniqueID] as string == null) ? txtLPFCGST.Text : Request[txtLPFCGST.UniqueID] as string);
            SGSTAmt = Convert.ToDecimal((Request[txtLPFSGST.UniqueID] as string == null) ? txtLPFSGST.Text : Request[txtLPFSGST.UniqueID] as string);
            decimal IGSTAmt = Convert.ToDecimal((Request[txtLPFIGST.UniqueID] as string == null) ? txtLPFIGST.Text : Request[txtLPFIGST.UniqueID] as string);
            FLDGAmt = Convert.ToDecimal((Request[txtFLDGAmt.UniqueID] as string == null) ? txtFLDGAmt.Text : Request[txtFLDGAmt.UniqueID] as string);
            CDisburse oLD = null;
            vLoanAppID = (Request[hfLoanAppId.UniqueID] as string == null) ? hfLoanAppId.Value : Request[hfLoanAppId.UniqueID] as string;
            vCustID = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            vSanctionId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;

            vInstType = (Request[ddlInstType.UniqueID] as string == null) ? ddlInstType.SelectedValue : Request[ddlInstType.UniqueID] as string;
            pSchedule = (Request[ddlRpSchdle.UniqueID] as string == null) ? ddlRpSchdle.SelectedValue : Request[ddlRpSchdle.UniqueID] as string;


            vBrkPrdInt = Convert.ToDecimal((Request[txtBrkPrdInt.UniqueID] as string == null) ? txtBrkPrdInt.Text : Request[txtBrkPrdInt.UniqueID] as string);
            vBrkPrdIntAct = Convert.ToDecimal((Request[txtBrkPrdIntAct.UniqueID] as string == null) ? txtBrkPrdIntAct.Text : Request[txtBrkPrdIntAct.UniqueID] as string);
            vBrkPrdIntWave = Convert.ToDecimal((Request[txtBrkPrdIntWave.UniqueID] as string == null) ? txtBrkPrdIntWave.Text : Request[txtBrkPrdIntWave.UniqueID] as string);

            vPropInsuAmt = Convert.ToDecimal((Request[txtPropInsuAmt.UniqueID] as string == null) ? txtPropInsuAmt.Text : Request[txtPropInsuAmt.UniqueID] as string);
            vPropInsuCGSTAmt = Convert.ToDecimal((Request[txtPropInsuCGST.UniqueID] as string == null) ? txtPropInsuCGST.Text : Request[txtPropInsuCGST.UniqueID] as string);
            vPropInsuSGSTAmt = Convert.ToDecimal((Request[txtPropInsuSGST.UniqueID] as string == null) ? txtPropInsuSGST.Text : Request[txtPropInsuSGST.UniqueID] as string);
            vPropInsuIGSTAmt = Convert.ToDecimal((Request[txtPropInsuIGST.UniqueID] as string == null) ? txtPropInsuIGST.Text : Request[txtPropInsuIGST.UniqueID] as string);


            vCERSAIAmt = Convert.ToDecimal((Request[txtCERSAICharge.UniqueID] as string == null) ? txtCERSAICharge.Text : Request[txtCERSAICharge.UniqueID] as string);
            vCERSAIAmtCGST = Convert.ToDecimal((Request[txtCERSAIChargeCGST.UniqueID] as string == null) ? txtCERSAIChargeCGST.Text : Request[txtCERSAIChargeCGST.UniqueID] as string);
            vCERSAIAmtSGST = Convert.ToDecimal((Request[txtCERSAIChargeSGST.UniqueID] as string == null) ? txtCERSAIChargeSGST.Text : Request[txtCERSAIChargeSGST.UniqueID] as string);
            vCERSAIAmtIGST = Convert.ToDecimal((Request[txtCERSAIChargeIGST.UniqueID] as string == null) ? txtCERSAIChargeIGST.Text : Request[txtCERSAIChargeIGST.UniqueID] as string);

            vAdminFees = Convert.ToDecimal((Request[txtAdminFees.UniqueID] as string == null) ? txtAdminFees.Text : Request[txtAdminFees.UniqueID] as string);
            vTechFees = Convert.ToDecimal((Request[txtTechFees.UniqueID] as string == null) ? txtTechFees.Text : Request[txtTechFees.UniqueID] as string);


            vLoanTypeID = Convert.ToInt32((Request[ddlLoanType.UniqueID] as string == null) ? ddlLoanType.SelectedValue : Request[ddlLoanType.UniqueID] as string);

            if (((Request[ddlSrcFund.UniqueID] as string == null) ? ddlSrcFund.SelectedValue : Request[ddlSrcFund.UniqueID] as string) != "-1")
            {
                vFunderID = Convert.ToInt32((Request[ddlSrcFund.UniqueID] as string == null) ? ddlSrcFund.SelectedValue : Request[ddlSrcFund.UniqueID] as string);
            }
            if (((Request[txtTotCharge.UniqueID] as string == null) ? txtTotCharge.Text : Request[txtTotCharge.UniqueID] as string) != "")
            {
                vTotCharge = Convert.ToDecimal((Request[txtTotCharge.UniqueID] as string == null) ? txtTotCharge.Text : Request[txtTotCharge.UniqueID] as string);
            }
            vPreLnBalLed = Convert.ToString((Request[hdPreLnAc.UniqueID] as string == null) ? hdPreLnAc.Value : Request[hdPreLnAc.UniqueID] as string);
            vPreLoanId = Convert.ToString((Request[hdPreLnIdTopUp.UniqueID] as string == null) ? hdPreLnIdTopUp.Value : Request[hdPreLnIdTopUp.UniqueID] as string);

            VNetOffLoanId = (Request[ddlNefOffLoanNo.UniqueID] as string == null) ? ddlNefOffLoanNo.SelectedValue : Request[ddlNefOffLoanNo.UniqueID] as string;
            VNetOffPrincAmt = Convert.ToDecimal((Request[txtNetOffPrinc.UniqueID] as string == null) ? txtNetOffPrinc.Text : Request[txtNetOffPrinc.UniqueID] as string);
            VNetOffIntAmt = Convert.ToDecimal((Request[txtNetOffInt.UniqueID] as string == null) ? txtNetOffInt.Text : Request[txtNetOffInt.UniqueID] as string);
            VNetOffAdvAmt = Convert.ToDecimal((Request[txtNetOffAdv.UniqueID] as string == null) ? txtNetOffAdv.Text : Request[txtNetOffAdv.UniqueID] as string);

            //if (PreLnBal > 0)
            //{
            //    if (vPreLnBalLed == "")
            //    {
            //        gblFuction.AjxMsgPopup("Prevoius Loan Balance Ledger has not been set...");
            //        return false;
            //    }
            //    if (vPreLoanId == "")
            //    {
            //        gblFuction.AjxMsgPopup("Prevoius Loan Id has not been set...");
            //        return false;
            //    }
            //}



            #region Trans Disbursement
            if (chkIsTrans.Checked == true)
            {
                if (!string.IsNullOrEmpty(this.txtTransDisburseAmt.Text.Trim()))
                {
                    vDisburseAmt = Convert.ToDecimal(txtTransDisburseAmt.Text);
                    isTransDisburse = "Y";
                    CGblIdGenerator oCG = new CGblIdGenerator();
                    DataTable dtTrans = oCG.GetTransDisbAc();
                    vTrnsDisbAc = dtTrans.Rows[0]["ToBeDisburseAC"].ToString();
                    if (vTrnsDisbAc == "")
                    {
                        gblFuction.MsgPopup("Please Set Tranche Disbursement Pending Account");
                        return false;
                    }
                }
                else
                {
                    isTransDisburse = "N";
                    gblFuction.AjxMsgPopup("Please set disburse amount.");
                    return false;
                }
            }
            else
            {
                vDisburseAmt = 0;
                isTransDisburse = "N";
                vTrnsDisbAc = "";
            }
            #endregion
            string pCollMode = "";
            string pLogBrCode = ddlBranch.SelectedValue.ToString();
            string pErrDesc = "";
            // isTransDisburse,vTrnsDisbAc,vDisburseAmt,pCollMode,vBrkPrdInt,pLogBrCode,pPreLnInt,pErrDesc,pBrkPrdIntAct,vBrkPrdIntWave

            //if (hdPreLnAc.Value != "")
            //    vPreLnBalLed = hdPreLnAc.Value.ToString();
            //if (hdPreLnIdTopUp.Value != "")
            //    vPreLoanId = hdPreLnIdTopUp.Value.ToString();

            // vPreLnBalLed = (Request[ddlLnBalLed.UniqueID] as string == null) ? ddlLnBalLed.SelectedValue : Request[ddlLnBalLed.UniqueID] as string;

            string vLoanAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, vLoanTypeID, vBrCode);
            string vLoanIncAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.LoanIntAc, vLoanTypeID, vBrCode);
            string vProcFeecAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ProcfeesAC, vLoanTypeID, vBrCode);
            string vIncAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.InsuAC, vLoanTypeID, vBrCode);
            string vServiceTaxAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ServiceAC, vLoanTypeID, vBrCode);
            string vStampChargeAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.StampChargeAC, vLoanTypeID, vBrCode);
            string vApplicationAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.ApplicationCargeAC, vLoanTypeID, vBrCode);

            string vLPFKKTaxAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.LPFKKTaxAC, vLoanTypeID, vBrCode);
            string vLPFSBTaxAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.LPFSBTaxAC, vLoanTypeID, vBrCode);
            string vLPFCGSTAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.CGSTAC, vLoanTypeID, vBrCode);
            string vLPFSGSTAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.SGSTAC, vLoanTypeID, vBrCode);
            string vFLDGAC = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.FLDGAC, vLoanTypeID, vBrCode);

            //if (PreLnBal > 0 && vPreLnBalLed == "-1")
            //{
            //    gblFuction.AjxMsgPopup("Please Select Ledger For Previous Loan balance..");
            //    return false;
            //}
            if (vCycle == 0)
            {
                gblFuction.AjxMsgPopup("Loan Cycle can not be zero..");
                return false;
            }
            // string vInsServTax = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.InsServTaxAC, vLoanTypeID, vBrCode);

            //if (vLoanTypeID <= 0)
            //{
            //    gblFuction.AjxMsgPopup("You can not save, Loan Type Not Selected...");
            //    return false;
            //}

            //vLoanId = (string)ViewState["LoanId"];
            if (Mode == "Save")
            {
                if (rdbPayMode.SelectedValue == "C")
                {
                    vDishbMode = "C";
                    vTranscMode = "C";
                    vReffNo = "";
                    vReffDt = "";
                    vReffLedgerAc = "C0001";
                }
                else if (rdbPayMode.SelectedValue == "B")
                {
                    if (ddlBank.SelectedValue.ToString() == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Select Bank Ledger..");
                        return false;
                    }
                    vDishbMode = "B";
                    vTranscMode = "B";
                    vReffNo = txtChqNo.Text;
                    vReffDt = txtChqDt.Text;
                    vReffLedgerAc = Convert.ToString(ddlBank.SelectedValue);
                }
                else if (rdbPayMode.SelectedValue == "N")
                {
                    if (ddlLedgr.SelectedValue.ToString() == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Select Ledger for NEFT/RTGS..");
                        return false;
                    }
                    vDishbMode = "N";
                    vTranscMode = "B";
                    vReffNo = txtRefNo.Text;
                    vReffDt = txtLnDt.Text;
                    vReffLedgerAc = Convert.ToString(ddlLedgr.SelectedValue);
                    //vAssetType = "0";
                }
            }
            string vCustName = ddlCust.SelectedItem.Text.ToString().Trim().Replace("'", ""), vErrDesc = "";
            vNarationL = "Being the Amt of Loan Disbursed for  " + vCustName;
            vNarationF = "Being the Amt of Fees For Loan Disbursed for " + vCustName;
            // vXmlAC = DataTableTOXml(dtAccount);
            int vUserId = Convert.ToInt32(Session[gblValue.UserId].ToString());

            oLD = new CDisburse();
            if (Mode == "Save")
            {
                vErr = oLD.InsertLoanMstNew(ref vLoanNo, vSanctionId, vLoanAppID, vCustID, vLoanTypeID, pDisbSrl, pDishbDate, TotLnAmt, vInstType, pSchedule,
                  IntRate, FIntRate, vInstalNo, InstPeriod, EMI, vInstallSize, pStDate, vDishbMode, vCycle, vFunderID, ProceFees,
                  InsuAmt, InsuStax, InsuCGST, InsuSGST, AppCharge, AdvEMIPric, AdvEMIInt, StampCharge, AdvInt, NetDisb, vReffLedgerAc, vTranscMode, vReffNo,
                  gblFuction.setDate(vReffDt), vBrCode, vTblMst, vTblDtl, vFinYear, vUserId, vNarationL, vCollDay,
                  vCollDayNo, "", vCollType, PreLnBal, vPreLnBalLed, CGSTAmt, SGSTAmt, IGSTAmt, FLDGAmt, vPreLoanId, vTotCharge,
                  isTransDisburse, vTrnsDisbAc, vDisburseAmt, pCollMode, vBrkPrdInt, pLogBrCode, pPreLnInt, ref vErrDesc, vBrkPrdIntAct, vBrkPrdIntWave,
                  vPropInsuAmt, vPropInsuCGSTAmt, vPropInsuSGSTAmt, vAdminFees, vTechFees, InsuIGST, vPropInsuIGSTAmt,
                  vCERSAIAmt, vCERSAIAmtCGST, vCERSAIAmtSGST, vCERSAIAmtIGST, chkNetOff.Checked == true ? "Y" : "N", VNetOffLoanId,
                  VNetOffAdvAmt, VNetOffPrincAmt, VNetOffIntAmt);

                if (vErr == 0)
                {
                    //-----------------------Jocata---------------------------
                    //try
                    //{
                    //    JocataRequest(vCustID, vLoanNo, vUserId);
                    //}
                    //finally
                    //{
                    //}
                    //--------------------------------------------------------
                    //// SMS //
                    string vMemMobNo = "", vDisbLoanId = "", vSuccessStat = "", vGuid = "";
                    string vEncode = "", vQstring = "", vMessageBody = "", vLinkBody = "";
                    Random oRend = null;
                    CSMS oDisbSms = null;
                    CMember oMem = null;
                    DataTable dtMob = new DataTable();
                    oMem = new CMember();
                    dtMob = oMem.GetDisbSMSMemMob(vSanctionId, pDishbDate);
                    if (dtMob.Rows.Count > 0)
                    {
                        vMemMobNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                        /////--------------------------------------Message Body-----------------------------------------
                        oRend = new Random();
                        vEncode = Convert.ToString(oRend.Next(1000, 9999)) + vSanctionId + Convert.ToString(oRend.Next(1000, 9999));
                        vQstring = vEncode;
                        vLinkBody = "https://centrumsme.bijliftt.com/sdn.aspx?pm=" + vQstring; //Live
                        //vLinkBody = "http://bijliserver54.bijliftt.com:3078/sdn.aspx?pm=" + vQstring; //UAT
                        //vMessageBody = "Thank you. Your loan no " + vLoanNo + " from CML has been disbursed. The link to view your loan documents is " + vLinkBody + " Centrum Microcredit Ltd.";

                        vMessageBody = "Thank you. Your loan no " + vLoanNo + " from Unity SFB has been disbursed. The link to view your loan documents is " + vLinkBody + ".";
                        /////-------------------------------------------------------------------------------------------
                        //string vRe = SendSMS(vMemMobNo, "Hi");
                        string vRe = SendSMS(vMemMobNo, vMessageBody);
                        string[] arr = vRe.Split('|');
                        vSuccessStat = arr[0];
                        vGuid = arr[2];
                        vDisbLoanId = vLoanNo;
                        oDisbSms = new CSMS();
                        int vErrSMS = oDisbSms.SaveDisbDigiDocSMS(vDisbLoanId, vCustID, vMemMobNo, vSuccessStat, vGuid, "I", this.UserID);
                    }
                    oMem = null;
                    oDisbSms = null;
                    dtMob = null;
                    oRend = null;
                    //// SMS //

                    txtLnNo.Text = vLoanNo;
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;

                    //// Trigger SMS For Disb
                    //DataTable dt_Sms = new DataTable();
                    //CSMS oSms = null;
                    //AuthSms oAuth = null;
                    //string vRtnGuid = string.Empty, vStatusCode = string.Empty, vStatusDesc = string.Empty;
                    //oSms = new CSMS();
                    //oAuth = new AuthSms();

                    //// For Applicant  (LD----> Loan Disbursement)
                    //dt_Sms = oSms.Get_ToSend_SMS(vSanctionId, pDishbDate, "LD");
                    //if (dt_Sms.Rows.Count > 0)
                    //{
                    //    foreach (DataRow drSMS in dt_Sms.Rows)
                    //    {
                    //        if (drSMS["MobNo"].ToString().Length >= 10)
                    //        {
                    //            vRtnGuid = oAuth.SendSms(drSMS["MobNo"].ToString(), drSMS["Msg"].ToString());
                    //            System.Threading.Thread.Sleep(500);
                    //            if (!string.IsNullOrEmpty(vRtnGuid))
                    //            {
                    //                vRtnGuid = vRtnGuid.Remove(0, 7);
                    //                if (vRtnGuid != "")
                    //                {
                    //                    vStatusDesc = "Message Delivered";
                    //                    vStatusCode = "Message Delivered Successfully";
                    //                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //                }
                    //                else
                    //                {
                    //                    vStatusDesc = "Unknown Error";
                    //                    vStatusCode = "Unknown Error";
                    //                    oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                vStatusDesc = "Unknown Error";
                    //                vStatusCode = "Unknown Error";
                    //                oSms.UpdateSingle_SMS_Status(Convert.ToInt32(drSMS["SMSId"].ToString()), vRtnGuid, vStatusDesc, vStatusCode);
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if (vErrDesc == "")
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrDesc);
                    }
                    vResult = false;
                }

            }
            else if (Mode == "Edit")
            {
                vLoanId = (string)ViewState["LoanId"];
                gblFuction.AjxMsgPopup("You Can Not Edit Disbursed Loan... To update Disbursement Record kindly delete disburshment  and redisburse it... ");
                return false;
            }
            else if (Mode == "Delete")
            {
                string pLoanId = (Request[txtLnNo.UniqueID] as string == null) ? txtLnNo.Text : Request[txtLnNo.UniqueID] as string;
                CCollectionRoutine oColl = new CCollectionRoutine();
                dt = oColl.ChkCollForDel(pLoanId, vBrCode, pDishbDate);
                if (Convert.ToInt32(dt.Rows[0]["TotColl"]) > 0)
                {
                    gblFuction.AjxMsgPopup("Collection  already exist against this Loan... Loan Can Not be Deleted...");
                    return false;
                }
                String FYear = GetCurrentFinancialYear(pDishbDate);
                string vFullFinYear = Session[gblValue.FinYear].ToString();
                if (vFullFinYear != FYear)
                {
                    gblFuction.AjxMsgPopup("Disbursement  can not be deleted as Disbursement Date is not in same Login  Financial Year...");
                    return false;
                }
                vErr = oLD.DeleteLoanMst(pLoanId, vSanctionId, vLoanAppID, vCustID, vLoanTypeID, pDisbSrl, pDishbDate, TotLnAmt, vInstType, pSchedule,
                   IntRate, FIntRate, vInstalNo, InstPeriod, EMI, vInstallSize, pStDate, vDishbMode, vCycle, vFunderID, ProceFees, LPFStax, LPFKKTax, LPFSBTax,
                  InsuAmt, InsuStax, InsuCGST, InsuSGST, AppCharge, AdvEMIPric, AdvEMIInt, StampCharge, AdvInt, NetDisb, vReffLedgerAc, vTranscMode, vReffNo,
                  gblFuction.setDate(vReffDt), vBrCode, vTblMst, vTblDtl, vFinYear, vUserId, vNarationL, vNarationF, vCollDay,
                  vCollDayNo, "", vCollType, PreLnBal, vPreLnBalLed, CGSTAmt, SGSTAmt, FLDGAmt);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    vResult = true;
                    tabLoanDisb.ActiveTabIndex = 0;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            LoadGrid(0);
            return vResult;
        }
        public static string GetCurrentFinancialYear(DateTime dt)
        {
            int CurrentYear = dt.Year;
            int PreviousYear = dt.Year - 1;
            int NextYear = dt.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (dt.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
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
                    gblFuction.focus("ctl00_cph_Main_tabLnDisb_pnlDtl_ddlCo");
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
        private void EnableControl(Boolean Status)
        {
            /*******************/
            ddlBranch.Enabled = Status;
            ddlLoanType.Enabled = Status;
            ddlRpSchdle.Enabled = Status;
            ddlInstType.Enabled = Status;
            ddlCust.Enabled = Status;
            ddlSancNo.Enabled = Status;
            txtLnDt.Enabled = Status;
            ddlSrcFund.Enabled = Status;
            rdbPayMode.Enabled = Status;
            txtChqNo.Enabled = Status;
            txtChqDt.Enabled = Status;
            //txtStDt.Enabled = false;
            ddlBank.Enabled = Status;
            ddlLedgr.Enabled = Status;
            txtRefNo.Enabled = Status;
            txtInsSerTax.Enabled = Status;
            txtSancAmt.Enabled = Status;
            txtSancDt.Enabled = Status;
            txtLnCycle.Enabled = Status;
            /*************************/
        }
        private void ClearControls()
        {
            /******/
            txtLnNo.Text = "";
            ddlSancNo.SelectedIndex = -1;
            ddlRpSchdle.SelectedIndex = -1;
            ddlCust.SelectedIndex = -1;
            txtFlatIntRate.Text = "";
            txtIntRate.Text = "";
            txtDisAmt.Text = "";
            ddlRpSchdle.SelectedIndex = -1;
            ddlInstType.SelectedIndex = -1;
            ddlLoanType.SelectedIndex = -1;
            ddlInstType.SelectedIndex = -1;
            txtLnAmt.Text = "0";
            txtIntRate.Text = "0";
            txtLnCycle.Text = "0";
            txtIntAmt.Text = "0";
            txtIntPeriod.Text = "0";
            txtInstNo.Text = "0";
            //txtLnDt.Text = "";
            //txtStDt.Text = "";
            //Session[gblValue.LoginDate].ToString()
            txtStDt.Text = "";
            txtEMIAmt.Text = "";
            txtAdvEMI.Text = "";
            txtProsFee.Text = "0";
            txtInsuAmt.Text = "0";
            txtServiceTax.Text = "0";
            ddlSrcFund.SelectedIndex = -1;
            ddlPurps.SelectedIndex = -1;
            rdbPayMode.SelectedValue = "N";
            txtChqNo.Text = "";
            txtChqDt.Text = "";
            ddlBank.SelectedIndex = -1;
            ddlLedgr.SelectedIndex = -1;
            txtRefNo.Text = "";
            ddlBank.SelectedIndex = -1;
            txtAppCharge.Text = "0";
            txtStmpChrg.Text = "0";
            txtPreLnBal.Text = "0";
            txtAdvInt.Text = "0";
            // ddlLnBalLed.SelectedIndex = -1;
            txtNetDisb.Text = "0";
            txtTotCharge.Text = "0";

        }
        private void PopBranch()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranch();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oItm);

                    //ddlPendingBranch.DataTextField = "BranchName";
                    //ddlPendingBranch.DataValueField = "BranchCode";
                    //ddlPendingBranch.DataSource = dt;
                    //ddlPendingBranch.DataBind();
                    //ListItem oItm1 = new ListItem("<--Select-->", "-1");
                    //ddlPendingBranch.Items.Insert(0, oItm1);

                    ddlPendingBranch.DataTextField = "BranchName";
                    ddlPendingBranch.DataValueField = "BranchCode";
                    ddlPendingBranch.DataSource = dt;
                    ddlPendingBranch.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        private void popCustomer()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustForDisbursh();
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        private void popCustomerForDisbLoan()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustForDisbLoan();
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        private void popSanctionNo(string pCustId)
        {

            DataTable dt = null;
            CDisburse oMem = null;
            try
            {
                oMem = new CDisburse();
                dt = oMem.GetSancIdByCustId(pCustId);
                if (dt.Rows.Count > 0)
                {
                    ddlSancNo.Items.Clear();
                    ddlSancNo.DataTextField = "SanctionNo";
                    ddlSancNo.DataValueField = "SanctionID";
                    ddlSancNo.DataSource = dt;
                    ddlSancNo.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlSancNo.Items.Insert(0, oli);

                }
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
        private void popSanctionNoForDisb(string pCustId)
        {

            DataTable dt = null;
            CDisburse oMem = null;
            try
            {
                oMem = new CDisburse();
                dt = oMem.GetSancIdForDisb(pCustId);
                if (dt.Rows.Count > 0)
                {
                    ddlSancNo.Items.Clear();
                    ddlSancNo.DataTextField = "SanctionNo";
                    ddlSancNo.DataValueField = "SanctionID";
                    ddlSancNo.DataSource = dt;
                    ddlSancNo.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlSancNo.Items.Insert(0, oli);

                }
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
        private void popLnSanctionNo(string pCustId)
        {

            DataTable dt = null;
            CDisburse oMem = null;
            try
            {
                oMem = new CDisburse();
                dt = oMem.GetLnSancIdByCustId(pCustId);
                if (dt.Rows.Count > 0)
                {
                    ddlSancNo.Items.Clear();
                    ddlSancNo.DataTextField = "SanctionNo";
                    ddlSancNo.DataValueField = "SanctionID";
                    ddlSancNo.DataSource = dt;
                    ddlSancNo.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlSancNo.Items.Insert(0, oli);

                }
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

        protected void btnDownloadDigiDoc_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["DocDownloadPath"];
            // vPathHDrive= ConfigurationManager.AppSettings["DocPathHDrive"];
            vPathNetworkDrive1 = ConfigurationManager.AppSettings["PathNetworkDrive1"];
            vPathNetworkDrive2 = ConfigurationManager.AppSettings["PathNetworkDrive2"];
            vPathNetworkDrive = ConfigurationManager.AppSettings["PathNetworkDrive"];

            ImageButton btn = sender as ImageButton;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lblLoanAppId = (Label)gvrow.FindControl("lblLoanAppId");
            string vLoanAppId = lblLoanAppId.Text;
            if (vLoanAppId != "")
            {
                string folderPath = string.Format("{0}/{1}", vPathImage, "DigitalDoc");
                string filePath1 = string.Format("{0}/{1}", folderPath, vLoanAppId + ".pdf");
                if (File.Exists(filePath1))
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment;filename=\"" + vLoanAppId + ".pdf" + "\"");
                    Response.TransmitFile(filePath1);
                    Response.End();
                    return;
                }
                else
                {
                    string[] arrPathNetwork = vPathNetworkDrive.Split(',');
                    int i;
                    string vPathDigiDoc = "";
                    for (i = 0; i <= arrPathNetwork.Length - 1; i++)
                    {
                        if (ValidUrlChk(arrPathNetwork[i] + "DigitalDoc/" + vLoanAppId + ".pdf"))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + "DigitalDoc/" + vLoanAppId + ".pdf";
                            break;
                        }
                        if (ValidUrlChk(arrPathNetwork[i] + "smedigitaldoc/" + vLoanAppId + ".pdf"))
                        {
                            vPathDigiDoc = arrPathNetwork[i] + "smedigitaldoc/" + vLoanAppId + ".pdf";
                            break;
                        }
                    }
                    if (vPathDigiDoc != "")
                    {
                        WebClient cln = null;
                        byte[] vDoc = null;
                        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                        cln = new WebClient();
                        vDoc = cln.DownloadData(vPathDigiDoc);
                        Response.AddHeader("Content-Type", "Application/octet-stream");
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("File not Exists..");
                    }

                    //WebClient cln = null;
                    //byte[] vDoc = null;
                    //ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    //if (ValidUrlChk(vPathNetworkDrive1 + "DigitalDoc/" + vLoanAppId + ".pdf"))
                    //{
                    //    cln = new WebClient();
                    //    vDoc = cln.DownloadData(vPathNetworkDrive1 + "DigitalDoc/" + vLoanAppId + ".pdf");
                    //    Response.AddHeader("Content-Type", "Application/octet-stream");
                    //    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                    //    Response.BinaryWrite(vDoc);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //else if (ValidUrlChk(vPathNetworkDrive2 + "DigitalDoc/" + vLoanAppId + ".pdf"))
                    //{
                    //    cln = new WebClient();
                    //    vDoc = cln.DownloadData(vPathNetworkDrive2 + "DigitalDoc/" + vLoanAppId + ".pdf");
                    //    Response.AddHeader("Content-Type", "Application/octet-stream");
                    //    Response.AddHeader("Content-Disposition", "attachment;   filename=" + vLoanAppId + ".pdf");
                    //    Response.BinaryWrite(vDoc);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //else
                    //{
                    //    gblFuction.AjxMsgPopup("File not Exists");
                    //}

                    return;
                }
            }
        }

        public bool ValidUrlChk(string pUrl)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
            WebRequest wR = WebRequest.Create(pUrl);
            WebResponse webResponse;
            try
            {
                wR.Timeout = 10000;
                webResponse = wR.GetResponse();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void clearNetOff()
        {
            chkNetOff.Checked = false;
            chkNetOff.Enabled = true;
            ddlNefOffLoanNo.Items.Clear();
            txtNetOffPrinc.Text = "0.00";
            txtNetOffInt.Text = "0.00";
            txtNetOffAdv.Text = "0.00";
        }

        protected void gvLoanAppl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dt = null;
            DataTable dtDtl = null;
            CDisburse oLD = null;
            Int32 vCollType = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vLoanId = "";
            clearNetOff();
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    oLD = new CDisburse();
                    vLoanId = Convert.ToString(e.CommandArgument);
                    dt = oLD.GetDisbDtlbyLoanId(vLoanId, vBrCode);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvLoanAppl.Rows)
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
                            LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                            lb.ForeColor = System.Drawing.Color.Black;
                            lb.Font.Bold = false;
                        }
                        //gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;

                        PopBranch();
                        popCustomerForDisbLoan();
                        LoadBankLedger();
                        LoadAcGenLed();
                        PopLoanType();
                        PopPurpose();
                        PopFunder();
                        /***************************/
                        ddlBranch.Items.Clear();
                        ddlBranch.DataSource = null;
                        ddlBranch.DataBind();
                        ListItem liSel = new ListItem();
                        liSel.Text = dt.Rows[0]["BranchName"].ToString();
                        liSel.Value = dt.Rows[0]["BranchCode"].ToString();
                        ddlBranch.Items.Insert(0, liSel);
                        ViewState["IsMig"] = Convert.ToString(dt.Rows[0]["IsMig"]);
                        txtLnNo.Text = Convert.ToString(dt.Rows[0]["LoanId"]);
                        ViewState["LoanId"] = Convert.ToString(dt.Rows[0]["LoanId"]);
                        ViewState["CustNo"] = Convert.ToString(dt.Rows[0]["CustNo"]);

                        ddlCust.SelectedIndex = ddlCust.Items.IndexOf(ddlCust.Items.FindByValue(dt.Rows[0]["CustId"].ToString()));
                        hfDisbSrl.Value = dt.Rows[0]["DisbSrl"].ToString();
                        popLnSanctionNo(dt.Rows[0]["CustId"].ToString());
                        txtSancDt.Text = Convert.ToString(dt.Rows[0]["FinalApprovedDt"]);
                        // ddlSancNo
                        //FinalApprovedDt
                        ddlSancNo.SelectedIndex = ddlSancNo.Items.IndexOf(ddlSancNo.Items.FindByValue(dt.Rows[0]["SanctionID"].ToString()));
                        txtSancAmt.Text = Convert.ToString(dt.Rows[0]["SanctionAmt"]);
                        ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
                        ddlRpSchdle.SelectedIndex = ddlRpSchdle.Items.IndexOf(ddlRpSchdle.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString()));
                        ddlInstType.SelectedIndex = ddlInstType.Items.IndexOf(ddlInstType.Items.FindByValue(dt.Rows[0]["InstType"].ToString()));
                        txtIntRate.Text = Convert.ToString(dt.Rows[0]["IntRate"]);
                        txtFlatIntRate.Text = Convert.ToString(dt.Rows[0]["FIntRate"]);
                        txtIntAmt.Text = Convert.ToString(dt.Rows[0]["InterestAmt"]);
                        txtLnDt.Text = Convert.ToString(dt.Rows[0]["DisbDate"]);
                        txtStDt.Text = Convert.ToString(dt.Rows[0]["RepayStartDt"]);
                        txtEMIAmt.Text = Convert.ToString(dt.Rows[0]["EMIAmt"]);
                        txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["NoOfInst"]);
                        txtInstNo.Text = Convert.ToString(dt.Rows[0]["NoOfInst"]);
                        txtLnCycle.Text = Convert.ToString(dt.Rows[0]["Cycle"]);
                        txtLnAmt.Text = Convert.ToString(dt.Rows[0]["DisbAmt"]);

                        txtAppCharge.Text = Convert.ToString(dt.Rows[0]["AppCharge"]);
                        txtProsFee.Text = Convert.ToString(dt.Rows[0]["ProcFees"]);
                        txtLPFCGST.Text = Convert.ToString(dt.Rows[0]["CGSTAmt"]);
                        txtLPFSGST.Text = Convert.ToString(dt.Rows[0]["SGSTAmt"]);
                        txtLPFIGST.Text = Convert.ToString(dt.Rows[0]["IGSTAmt"]);

                        txtInsuAmt.Text = Convert.ToString(dt.Rows[0]["InsuAmt"]);
                        txtInsuCGSTAmt.Text = Convert.ToString(dt.Rows[0]["InsuSTax"]);
                        txtInsuSGSTAmt.Text = Convert.ToString(dt.Rows[0]["InsuKKTax"]);
                        txtInsuIGSTAmt.Text = Convert.ToString(dt.Rows[0]["InsuSBTax"]);


                        txtServiceTax.Text = Convert.ToString(dt.Rows[0]["LPFSTax"]);
                        txtInsSerTax.Text = Convert.ToString(dt.Rows[0]["InsuSTax"]);
                        txtLPFKKTax.Text = Convert.ToString(dt.Rows[0]["LPFKKTax"]);
                        txtLPFSBTax.Text = Convert.ToString(dt.Rows[0]["LPFSBTax"]);

                        txtAdvEMIPrin.Text = Convert.ToString(dt.Rows[0]["AdvEMIPric"]);
                        txtAdvEMIInst.Text = Convert.ToString(dt.Rows[0]["AdvEMIInt"]);
                        txtAdvEMI.Text = Convert.ToString(dt.Rows[0]["AdvanceEMI"]);
                        txtStmpChrg.Text = Convert.ToString(dt.Rows[0]["StampCharge"]);
                        txtAdvInt.Text = Convert.ToString(dt.Rows[0]["AdvInterest"]);
                        txtPreLnBal.Text = Convert.ToString(dt.Rows[0]["PreLnBal"]);


                        txtFLDGAmt.Text = Convert.ToString(dt.Rows[0]["FLDGAmt"]);
                        hdPreLnIdTopUp.Value = Convert.ToString(dt.Rows[0]["PreLoanId"]);
                        hdPreLnAc.Value = Convert.ToString(dt.Rows[0]["PreLnBalLedAC"]);
                        //if (dt.Rows[0]["PreLnBalLedAC"].ToString() != "")
                        //{
                        //    ddlLnBalLed.SelectedIndex = ddlLnBalLed.Items.IndexOf(ddlLnBalLed.Items.FindByValue(Convert.ToString(dt.Rows[0]["PreLnBalLedAC"])));
                        //}
                        txtTotCharge.Text = Convert.ToString(dt.Rows[0]["TotalCharge"]);
                        txtNetDisb.Text = Convert.ToString(dt.Rows[0]["NetDisbAmt"]);
                        ddlSrcFund.SelectedIndex = ddlSrcFund.Items.IndexOf(ddlSrcFund.Items.FindByValue(Convert.ToString(dt.Rows[0]["FunderId"])));
                        ddlPurps.SelectedIndex = ddlPurps.Items.IndexOf(ddlPurps.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeId"])));
                        bool vViewPotenMem = false;
                        CRole oRl = new CRole();
                        DataTable dt1 = new DataTable();
                        dt1 = oRl.GetRoleById(Convert.ToInt32(Session[gblValue.RoleId].ToString()));
                        if (dt1.Rows.Count > 0)
                        {
                            vViewPotenMem = Convert.ToString(dt.Rows[0]["ShowPotential"]) == "Y" && Convert.ToString(dt1.Rows[0]["PotenMemYN"]) == "Y" ? true : false;
                        }
                        btnShwPotenMem.Visible = vViewPotenMem;
                        btnUpdateUcic.Visible = vViewPotenMem;
                        hdnProtenUrl.Value = (Convert.ToString(dt.Rows[0]["PotenURL"]));

                        if (Convert.ToString(dt.Rows[0]["DisbMode"]) == "C")
                        {
                            rdbPayMode.SelectedValue = "C";
                        }
                        else if (Convert.ToString(dt.Rows[0]["DisbMode"]) == "B")
                        {
                            rdbPayMode.SelectedValue = "B";
                            txtChqNo.Text = Convert.ToString(dt.Rows[0]["ReffNo"]);
                            txtChqDt.Text = Convert.ToString(dt.Rows[0]["ReffDate"]);
                            ddlBank.SelectedIndex = ddlBank.Items.IndexOf(ddlBank.Items.FindByValue(Convert.ToString(dt.Rows[0]["RefLedgerAC"])));
                        }
                        else if (Convert.ToString(dt.Rows[0]["DisbMode"]) == "N")
                        {
                            rdbPayMode.SelectedValue = "N";
                            ddlLedgr.SelectedIndex = ddlLedgr.Items.IndexOf(ddlLedgr.Items.FindByValue(Convert.ToString(dt.Rows[0]["RefLedgerAC"])));
                            txtRefNo.Text = Convert.ToString(dt.Rows[0]["ReffNo"]);
                        }
                        if (Convert.ToString(dt.Rows[0]["IsTransDisb"]) == "N")
                        {
                            chkIsTrans.Checked = false;

                        }
                        else

                            chkIsTrans.Checked = true;

                        txtTransDisburseAmt.Text = Convert.ToString(dt.Rows[0]["TransAmt"]);
                        txtBrkPrdIntAct.Text = Convert.ToString(dt.Rows[0]["BrkPrdIntAct"]);
                        txtBrkPrdIntWave.Text = Convert.ToString(dt.Rows[0]["BrkPrdIntWave"]);
                        txtBrkPrdInt.Text = Convert.ToString(dt.Rows[0]["BrkPrdInt"]);

                        txtPropInsuAmt.Text = Convert.ToString(dt.Rows[0]["PropertyInsAmt"]);
                        txtPropInsuCGST.Text = Convert.ToString(dt.Rows[0]["PropertyInsCGSTAmt"]);
                        txtPropInsuSGST.Text = Convert.ToString(dt.Rows[0]["PropertyInsSGSTAmt"]);
                        txtPropInsuIGST.Text = Convert.ToString(dt.Rows[0]["PropertyInsIGSTAmt"]);

                        txtAdminFees.Text = Convert.ToString(dt.Rows[0]["AdminFees"]);
                        txtTechFees.Text = Convert.ToString(dt.Rows[0]["TechFees"]);

                        chkNetOff.Checked = Convert.ToString(dt.Rows[0]["NetOffYN"]) == "Y" ? true : false;
                        txtNetOffPrinc.Text = Convert.ToString(dt.Rows[0]["NetOffPrinAmt"]);
                        txtNetOffAdv.Text = Convert.ToString(dt.Rows[0]["NetOffAdvAmt"]);
                        txtNetOffInt.Text = Convert.ToString(dt.Rows[0]["NetOffIntAmt"]);
                        if (Convert.ToString(dt.Rows[0]["NetOffYN"]) == "Y")
                        {
                            ddlNefOffLoanNo.DataSource = dt;
                            ddlNefOffLoanNo.DataTextField = "MemberName";
                            ddlNefOffLoanNo.DataValueField = "NetOffLoanId";
                            ddlNefOffLoanNo.DataBind();
                            chkNetOff.Enabled = false;
                        }

                        ddlNefOffLoanNo.SelectedIndex = ddlNefOffLoanNo.Items.IndexOf(ddlNefOffLoanNo.Items.FindByValue(Convert.ToString(dt.Rows[0]["NetOffLoanId"])));
                        ViewState["LoanId"] = Convert.ToString(dt.Rows[0]["LoanId"]);
                    }
                    tabLoanDisb.ActiveTabIndex = 2;
                    StatusButton("Show");
                    EnableControl(false);
                }
            }
            finally
            {
                dt = null;
                dtDtl = null;
                oLD = null;
            }
        }

        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = "";
            if (((Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string) != "-1")
            {
                pCustId = ((Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string);
                popSanctionNoForDisb(pCustId);
            }
        }
        //protected void ddlAppNo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string vBrCode = Session[gblValue.BrnchCode].ToString();
        //    string vLoanAppID = Convert.ToString(ddlAppNo.SelectedValue);
        //    string vCenterId = Convert.ToString(ddlCenter.SelectedValue);
        //    DateTime vLoanDt = gblFuction.setDate(txtLnDt.Text);
        //    Int32 vCollDay = 0, vCollDayNo = 0;
        //    DataTable dt = null, dtDay = null, dtSize = null;
        //    CDisburse oLD = null;
        //    CCollectionRoutine oCR = null;
        //    try
        //    {
        //        oLD = new CDisburse();
        //        dt = oLD.GetLoanAppdtlGD(vLoanAppID, vBrCode);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlRpSchdle.SelectedIndex = ddlRpSchdle.Items.IndexOf(ddlRpSchdle.Items.FindByValue(dt.Rows[0]["PaySchedule"].ToString().Trim()));
        //            ddlLoanType.SelectedIndex = ddlLoanType.Items.IndexOf(ddlLoanType.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
        //            ddlInstType.SelectedIndex = ddlInstType.Items.IndexOf(ddlInstType.Items.FindByValue(dt.Rows[0]["InstType"].ToString()));
        //            ddlPurps.SelectedIndex = ddlPurps.Items.IndexOf(ddlPurps.Items.FindByValue(dt.Rows[0]["PurposeID"].ToString()));
        //            txtLnAmt.Text = Convert.ToString(dt.Rows[0]["ApprovalAmt"]);
        //            txtIntRate.Text = Convert.ToString(dt.Rows[0]["InstRate"]);
        //            txtLnCycle.Text = Convert.ToString(dt.Rows[0]["Cycle"]);
        //            txtIntPeriod.Text = Convert.ToString(dt.Rows[0]["InstPeriod"]);
        //            txtInstNo.Text = Convert.ToString(dt.Rows[0]["InstallNo"]);
        //            txtProsFee.Text = Convert.ToString(dt.Rows[0]["ProcFeeAmt"]);
        //            txtInsuAmt.Text = Convert.ToString(dt.Rows[0]["IncFeesAmt"]);
        //            txtBDSAmt.Text = Convert.ToString(dt.Rows[0]["OthersAmt"]);
        //            txtServiceTax.Text = Convert.ToString(dt.Rows[0]["ServiceTaxAmt"]);
        //            if (Convert.ToString(dt.Rows[0]["AssType"]) == "Y")
        //            {
        //                rdbPayMode.SelectedIndex = 2;
        //                tblBank.Visible = false;
        //                tblJurnl.Visible = true;
        //            }
        //            oCR = new CCollectionRoutine();
        //            dtDay = oCR.GetCollDay(vCenterId);
        //            if (dtDay.Rows.Count > 0)
        //            {
        //                vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
        //                if (Convert.IsDBNull(dtDay.Rows[0]["CollDayNo"]) == true)
        //                {
        //                    vCollDayNo = 0;
        //                }
        //                else
        //                {
        //                    vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
        //                }
        //                txtStDt.Text = gblFuction.GetStartDate(vLoanDt, vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), vCenterId);
        //                txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
        //            }
        //            else
        //            {
        //                gblFuction.AjxMsgPopup("Please Set the collection Routine of the Group...");
        //                return;
        //            }
        //            dtSize = gblFuction.GetInstallSize(Convert.ToInt32(ddlLoanType.SelectedValue), Convert.ToDecimal(txtLnAmt.Text), Convert.ToDecimal(txtIntRate.Text), Convert.ToInt32(txtIntPeriod.Text),
        //                                            Convert.ToInt32(txtInstNo.Text), Convert.ToString(ddlRpSchdle.SelectedValue));
        //            txtInstSize.Text = Convert.ToString(dtSize.Rows[0]["InstallAmt"]);
        //            txtIntAmt.Text = Convert.ToString(dtSize.Rows[0]["TotInst"]);


        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oLD = null;
        //    }
        //}


        //protected void rdbPayMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (rdbPayMode.SelectedValue == "C")
        //    {
        //        tblBank.Visible = false;
        //        tblJurnl.Visible = false;
        //    }
        //    else if (rdbPayMode.SelectedValue == "B")
        //    {
        //        tblBank.Visible = true;
        //        tblJurnl.Visible = false;
        //    }
        //    else if (rdbPayMode.SelectedValue == "J")
        //    {
        //        tblBank.Visible = false;
        //        tblJurnl.Visible = true;
        //    }
        //}

        //protected void txtLnDt_TextChanged(object sender, EventArgs e)
        //{
        //    Int32 vCollDay = 0, vCollDayNo = 0, vCollType=0;
        //    DataTable dtDay = null;
        //    CCollectionRoutine oCR = null;
        //    if (ddlAppNo.SelectedIndex <= 0)
        //    {
        //        gblFuction.AjxMsgPopup("Please Select the application No...");
        //        return;
        //    }
        //    if (txtLnDt.Text != "" && gblFuction.IsDate(txtLnDt.Text) == true)
        //    {
        //        oCR = new CCollectionRoutine();
        //        dtDay = oCR.GetCollDay(Convert.ToString(ddlGrp.SelectedValue));
        //        if (dtDay.Rows.Count > 0)
        //        {
        //            vCollType = Convert.ToInt32(dtDay.Rows[0]["CollType"]);
        //            vCollDay = Convert.ToInt32(dtDay.Rows[0]["CollDay"]);
        //            vCollDayNo = Convert.ToInt32(dtDay.Rows[0]["CollDayNo"]);
        //            txtStDt.Text = gblFuction.GetStartDate(gblFuction.setDate(txtLnDt.Text), vCollDay, vCollDayNo, Convert.ToString(ddlRpSchdle.SelectedValue), Convert.ToString(ddlGrp.SelectedValue), vCollType);
        //            txtDay.Text = Convert.ToString(gblFuction.setDate(txtStDt.Text).DayOfWeek);
        //        }
        //    }
        //    else
        //    {
        //        gblFuction.AjxMsgPopup("Invalid Loan Date...");
        //        return;
        //    }
        //}

        //protected void ddlYBLoan_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string YbLoanNo = ddlYBLoan.SelectedItem.Text.ToString();
        //    Int32 intSrChar = YbLoanNo.IndexOf(":");
        //    txtYbBalAmt.Text = "";

        //    if (intSrChar > 0)
        //        txtYbBalAmt.Text = Convert.ToString(YbLoanNo.Substring(intSrChar + 1, (YbLoanNo.Length - (intSrChar + 1))).ToString());
        //}

        //private void PopYbLoanBal(string vLoanId)
        //{
        //    /*****************
        //    ddlYBLoan.Items.Clear();
        //    CYBDisburse oYbLoan = null;
        //    DataTable dt = null;
        //    try
        //    {
        //        oYbLoan = new CYBDisburse();
        //        dt = oYbLoan.GetYbLoanBalance(ddlCenter.SelectedValue, Session[gblValue.BrnchCode].ToString(), vLoanId);
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlYBLoan.DataSource = dt;
        //            ddlYBLoan.DataTextField = "YBLOANNO";
        //            ddlYBLoan.DataValueField = "YBLOANID";
        //            ddlYBLoan.DataBind();

        //        }
        //        ListItem oLi = new ListItem("<--Select-->", "-1");
        //        ddlYBLoan.Items.Insert(0, oLi);
        //    }
        //    finally
        //    {
        //        oYbLoan = null;
        //        dt = null;
        //    }
        //    ***************/
        //}

        protected void btnShwPotenMem_Click(object sender, EventArgs e)
        {
            string vUrl = hdnProtenUrl.Value;
            string url = vUrl + "BIJLI";
            string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            return;
        }

        protected void btnUpdateUcic_Click(object sender, EventArgs e)
        {
            string pCustId = Convert.ToString(ViewState["CustNo"]);
            string pLoanId = Convert.ToString(ViewState["LoanId"]);
            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            string pUcicId = getUcic(pCustId, pCreatedBy, pLoanId);
            int pErr = -1;
            CDisburse hv = new CDisburse();
            if (pUcicId != "")
            {
                pErr = hv.UpdateUcicId(pUcicId, pCustId, pLoanId);
                if (pErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.EditMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            else
            {
                gblFuction.MsgPopup("Respose Error");
            }
        }

        public string getUcic(string pCustId, int pCreatedBy, string pLoanId)
        {
            string vResponse = "", vUcic = "";
            CDisburse oDb = new CDisburse();
            try
            {
                string Requestdata = "{\"cust_id\" :\"" + pCustId + "\",\"source_system_name\":\"BIJLI\"}";
                // string postURL = "http://144.24.116.182:9002/UnitySfbWS/getUcic";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/getUcic";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                if (res.ResponseCode == "200")
                {
                    vUcic = res.Ucic;
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLogUCIC(pCustId, pLoanId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log---------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLogUCIC(pCustId, pLoanId, vResponseXml, pCreatedBy, vUcic);
                //---------------------------------------------------------------------------------
            }
            return vUcic;
        }

        protected void btnUpdateNachNo_Click(object sender, EventArgs e)
        {
            Button btnUpdateNachNo = (Button)sender;
            GridViewRow gR = (GridViewRow)btnUpdateNachNo.NamingContainer;
            LinkButton btnShow = (LinkButton)gR.FindControl("btnShow");
            TextBox txtNachNo = (TextBox)gR.FindControl("txtNachNo");
            if (txtNachNo.Text == "")
            {
                gblFuction.AjxMsgPopup("UMRN can not be left blank.");
                return;
            }

            //-----------------------
            CDisburse oLD = new CDisburse();
            int vErr = 0;
            vErr = oLD.UpdateNachNo(btnShow.Text, txtNachNo.Text, Convert.ToInt32(Session[gblValue.UserId].ToString()));
            if (vErr == 0)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.EditMsg);
                LoadGrid(1);
                return;
            }
            else if (vErr == 2)
            {
                gblFuction.AjxMsgPopup("You have already entered that UMRN.");
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }
            //-------------------------------------------------------------------            
        }

        //-------------------------Send SMS------------------------------------
        public string SendSMS(string pMobileNo, string pMsgBody)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)
                string vMsgBody = string.Empty;
                //vMsgBody = pMsgBody;
                vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-15"));
                //********************************************************************
                String sendToPhoneNumber = pMobileNo;
                //String userid = "2000194447";
                //String passwd = "Centrum@2020";
                String userid = "2000203137";
                String passwd = "UnitySFB@1";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007825554454171385&principalEntityId=1001301154610005078&mask=CENMEL&v=1.1&format=text";
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472090473581&principalEntityId=1001301154610005078&mask=MFIDOC&v=1.1&format=text";
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472090473581&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                reader = new System.IO.StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        //--------------------Base64 Encode----------------------------------
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        //-------------------------------------------------------------------

        /*
        protected void btnSMS_Click(object sender, EventArgs e)
        {
            // Test SMS button                       
            string vSanctionId = "00000000029";
            string vLoanNo = "00000000029";
            string vCustID = "00000000029";
            DateTime pDishbDate = gblFuction.setDate(txtFrmDt.Text);

            string vMemMobNo = "", vDisbLoanId = "", vSuccessStat = "", vGuid = "";
            string vEncode = "", vQstring = "", vMessageBody = "", vLinkBody = "";
            Random oRend = null;
            CSMS oDisbSms = null;
            CMember oMem = null;
            DataTable dtMob = new DataTable();
            oMem = new CMember();
            dtMob = oMem.GetDisbSMSMemMob(vSanctionId, pDishbDate);
            if (dtMob.Rows.Count > 0)
            {
                vMemMobNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                /////------------------------------------Message Body---------------------------------------
                //vEncode = Base64Encode(vSanctionId);
                //vQstring = ("Vm9zIGZhY3R1cmVzIGltcGF577" + vEncode + "WVOVEovDkqwhCF8N");   
                oRend = new Random();
                vEncode = Convert.ToString(oRend.Next(1000, 9999)) + vSanctionId + Convert.ToString(oRend.Next(1000, 9999));
                vQstring = vEncode;
                vMessageBody = "";
                //vLinkBody = "https://centrum.bijliftt.com/SMSDocDNLD.aspx?LoanAppId=" + vQstring;
                //vLinkBody = "https://centrum.bijliftt.com/SMSDocDNLD.aspx?param=" + vQstring;
                //vLinkBody = "https://centrumsme.bijliftt.com/sdn.aspx?pm=" + vQstring; //Live
                vLinkBody = "http://bijliserver54.bijliftt.com:3078/sdn.aspx?pm=" + vQstring; //UAT
                vMessageBody = "Thank you. Your loan no " + vLoanNo + " from CML has been disbursed. The link to view your loan documents is " + vLinkBody + " Centrum Microcredit Ltd.";
                /////-------------------------------------------------------------------------------------------
                //string vRe = SendSMS(vMemMobNo, "Hi");
                string vRe = SendSMS(vMemMobNo, vMessageBody);                                
                string[] arr = vRe.Split('|');
                vSuccessStat = arr[0];
                vGuid = arr[2];
                vDisbLoanId = vLoanNo;
                oDisbSms = new CSMS();
                int vErrSMS = oDisbSms.SaveDisbDigiDocSMS(vDisbLoanId, vCustID, vMemMobNo, vSuccessStat, vGuid, "I", this.UserID);
            }
            oMem = null;
            oDisbSms = null;
            dtMob = null;
            oRend = null;
        }       
        */

        protected void Change_Page(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrPage.Text) + 1;
                    break;
            }
            LoadPendingDisbList(cPgNo);
            tabLoanDisb.ActiveTabIndex = 0;
        }

        #region JocataCalling

        public string GetJokataToken()
        {
            string postURL = "https://aml.unitybank.co.in/ramp/webservices/createToken";
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "text/plain";
                request.Headers.Add("username", "BU_Bijli");
                request.Headers.Add("password", "BU_Bijli");
                request.Headers.Add("clientId", "BU_Bijli");
                request.Headers.Add("clientSecret", "BU_Bijli");
                request.Headers.Add("subBu", "Sub_BU_IB");
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(fullResponse);
                string vJokataToken = res.token;
                return vJokataToken;
            }
            catch (WebException we)
            {
                string Response = "";
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    Response = reader.ReadToEnd();
                }
                return Response;
            }
            finally
            {
                // streamWriter = null;
            }
        }

        public string RampRequest(PostRampRequest postRampRequest)
        {
            string vJokataToken = vJocataToken, vMemberId = "", vRampResponse = "";
            try
            {
                //-----------------------Create Token--------------------------         
                //vJokataToken = GetJokataToken();
                //vMemberId = postRampRequest.rampRequest.listMatchingPayload.requestListVO.requestVOList[0].customerId;
                //CDisburse oDb = new CDisburse();
                //oDb.SaveJocataToken(vMemberId, vJokataToken);
                //-----------------------Ramp Request------------------------
                string postURL = "https://aml.unitybank.co.in/ramp/webservices/request/handle-ramp-request";
                string Requestdata = JsonConvert.SerializeObject(postRampRequest);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + vJokataToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vRampResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                return vRampResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vRampResponse = reader.ReadToEnd();
                }
            }
            finally
            {
            }
            return vRampResponse;
        }

        public string JocataRequest(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            string vResponseData = "";
            DataTable dt = new DataTable();
            CDisburse oDb = null;
            try
            {
                oDb = new CDisburse();
                dt = oDb.GetJocataRequestData(pMemberID);
                if (dt.Rows.Count > 0)
                {
                    List<RequestVOList> vRVL = new List<RequestVOList>();
                    vRVL.Add(new RequestVOList
                    {
                        aadhar = dt.Rows[0]["Aadhar"].ToString(),
                        address = dt.Rows[0]["ParAddress"].ToString(),
                        city = dt.Rows[0]["District"].ToString(),
                        country = dt.Rows[0]["Country"].ToString(),
                        concatAddress = dt.Rows[0]["PreAddr"].ToString(),
                        customerId = dt.Rows[0]["MemberID"].ToString(),
                        digitalID = "",
                        din = "",
                        dob = dt.Rows[0]["DOB"].ToString(),
                        docNumber = "",
                        drivingLicence = dt.Rows[0]["DL"].ToString(),
                        email = "",
                        entityName = "",
                        name = dt.Rows[0]["MemberName"].ToString(),
                        nationality = "Indian",
                        pan = dt.Rows[0]["Pan"].ToString(),
                        passport = dt.Rows[0]["Passport"].ToString(),
                        phone = dt.Rows[0]["Mobile"].ToString(),
                        pincode = dt.Rows[0]["PinCode"].ToString(),
                        rationCardNo = dt.Rows[0]["RationCard"].ToString(),
                        ssn = "",
                        state = dt.Rows[0]["State"].ToString(),
                        tin = "",
                        voterId = dt.Rows[0]["Voter"].ToString()
                    });

                    var vLV = new RequestListVO();
                    vLV.businessUnit = "BU_Bijli";
                    vLV.subBusinessUnit = "Sub_BU_IB";
                    vLV.requestType = "API";
                    vLV.requestVOList = vRVL;

                    var vLMP = new ListMatchingPayload();
                    vLMP.requestListVO = vLV;

                    var vRR = new RampRequest();
                    vRR.listMatchingPayload = vLMP;

                    var req = new PostRampRequest();
                    req.rampRequest = vRR;

                    vResponseData = RampRequest(req);
                    dynamic vResponse = JsonConvert.DeserializeObject(vResponseData);
                    string vScreeningId = "";
                    if (vResponse.rampResponse.statusCode == "200")
                    {
                        Boolean vMatchFlag = vResponse.rampResponse.listMatchResponse.matchResult.matchFlag;
                        vScreeningId = vResponse.rampResponse.listMatchResponse.matchResult.uniqueRequestId;
                        string vStatus = "P";
                        if (vMatchFlag == true)
                        {
                            vStatus = "N";
                        }
                        else
                        {
                            try
                            {
                                Prosidex(pMemberID, pLoanId, pCreatedBy);
                            }
                            finally { }
                        }
                        oDb = new CDisburse();
                        oDb.UpdateJocataStatus(pLoanId, vScreeningId, vStatus, pCreatedBy, "", "LOW");
                    }
                    oDb = new CDisburse();
                    string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                    oDb.SaveJocataLog(pMemberID, pLoanId, vResponseXml, vScreeningId);
                }
            }
            catch
            {
                oDb = new CDisburse();
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponseData, "root"));
                oDb.SaveJocataLog(pMemberID, pLoanId, vResponseXml, "");
            }
            finally { }
            return "";
        }
        #endregion

        #region Prosidex Integration
        public ProsidexResponse Prosidex(string pMemberID, string pLoanId, Int32 pCreatedBy)
        {
            DataTable dt = new DataTable();
            CDisburse oDb = new CDisburse();

            ProsidexRequest pReqData = new ProsidexRequest();
            Request pReq = new Request();
            DG pDg = new DG();
            ProsidexResponse pResponseData = null;

            dt = oDb.GetProsidexReqData(pMemberID, pCreatedBy);
            if (dt.Rows.Count > 0)
            {
                pDg.ACCOUNT_NUMBER = dt.Rows[0]["ACCOUNT_NUMBER"].ToString();
                pDg.ALIAS_NAME = dt.Rows[0]["ALIAS_NAME"].ToString();
                pDg.APPLICATIONID = pLoanId;
                pDg.Aadhar = dt.Rows[0]["Aadhar"].ToString();
                pDg.CIN = dt.Rows[0]["CIN"].ToString();
                pDg.CKYC = dt.Rows[0]["CKYC"].ToString();
                pDg.CUSTOMER_STATUS = dt.Rows[0]["CUSTOMER_STATUS"].ToString();
                pDg.CUST_ID = dt.Rows[0]["CUST_ID"].ToString();
                pDg.DOB = dt.Rows[0]["DOB"].ToString();
                pDg.DrivingLicense = dt.Rows[0]["DrivingLicense"].ToString();
                pDg.Father_First_Name = dt.Rows[0]["Father_First_Name"].ToString();
                pDg.Father_Last_Name = dt.Rows[0]["Father_Last_Name"].ToString();
                pDg.Father_Middle_Name = dt.Rows[0]["Father_Middle_Name"].ToString();
                pDg.Father_Name = dt.Rows[0]["Father_Name"].ToString();
                pDg.First_Name = dt.Rows[0]["First_Name"].ToString();
                pDg.Middle_Name = dt.Rows[0]["Middle_Name"].ToString();
                pDg.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                pDg.Gender = dt.Rows[0]["Gender"].ToString();
                pDg.GSTIN = dt.Rows[0]["GSTIN"].ToString();
                pDg.Lead_Id = dt.Rows[0]["Lead_Id"].ToString();
                pDg.NREGA = dt.Rows[0]["NREGA"].ToString();
                pDg.Pan = dt.Rows[0]["Pan"].ToString();
                pDg.PassportNo = dt.Rows[0]["PassportNo"].ToString();
                pDg.RELATION_TYPE = dt.Rows[0]["RELATION_TYPE"].ToString();
                pDg.RationCard = dt.Rows[0]["RationCard"].ToString();
                pDg.Registration_NO = dt.Rows[0]["Registration_NO"].ToString();
                pDg.SALUTATION = dt.Rows[0]["SALUTATION"].ToString();
                pDg.TAN = dt.Rows[0]["TAN"].ToString();
                pDg.Udyam_aadhar_number = dt.Rows[0]["Udyam_aadhar_number"].ToString();
                pDg.VoterId = dt.Rows[0]["VoterId"].ToString();
                pDg.Tasc_Customer = dt.Rows[0]["Tasc_Customer"].ToString();

                pReq.ACE = new List<object>();
                pReq.UnitySfb_RequestId = dt.Rows[0]["RequestId"].ToString();
                pReq.CUST_TYPE = "I";
                pReq.CustomerCategory = "I";
                pReq.MatchingRuleProfile = "1";
                pReq.Req_flag = "QA";
                pReq.SourceSystem = "BIJLI";
                pReq.DG = pDg;
                pReqData.Request = pReq;
            }
            pResponseData = ProsidexSearchCustomer(pReqData);
            return pResponseData;
        }

        public ProsidexResponse ProsidexSearchCustomer(ProsidexRequest prosidexRequest)
        {
            string vResponse = "", vUCIC = "", vRequestId = "", vMemberId = "", vPotentialYN = "N", vPotenURL = "", vLoanId = "";
            Int32 vCreatedBy = 1;
            ProsidexResponse oProsidexResponse = null;
            CDisburse oDb = new CDisburse();
            try
            {
                vRequestId = prosidexRequest.Request.UnitySfb_RequestId;
                vMemberId = prosidexRequest.Request.DG.CUST_ID;
                vLoanId = prosidexRequest.Request.DG.APPLICATIONID;

                string Requestdata = JsonConvert.SerializeObject(prosidexRequest);
                //string postURL = "http://144.24.116.182:9002/UnitySfbWS/searchCustomer";
                string postURL = "https://ucic.unitybank.co.in:9002/UnitySfbWS/searchCustomer";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(Requestdata);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);

                if (res.Response.StatusInfo.ResponseCode == "200")
                {
                    vUCIC = res.Response.StatusInfo.POSIDEX_GENERATED_UCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, vUCIC == null ? 300 : 200);
                    vPotentialYN = vUCIC == null ? "Y" : "N";
                    vPotenURL = vUCIC == null ? res.Response.StatusInfo.CRM_URL : "";
                }
                else
                {
                    vUCIC = vUCIC == null ? "" : vUCIC;
                    oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------                
                return oProsidexResponse;
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
                //----------------------------Save Log-------------------------------------------------
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(vResponse, "root"));
                oDb.SaveProsidexLog(vMemberId, vLoanId, vRequestId, vResponseXml, vCreatedBy, vUCIC, vPotentialYN, vPotenURL);
                //--------------------------------------------------------------------------------------
                oProsidexResponse = new ProsidexResponse(vRequestId, vUCIC, 500);
            }
            finally
            {
            }
            return oProsidexResponse;
        }
        #endregion

        #region Common
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
        #endregion
    }

    #region Jocata_Class

    public class RequestListVO
    {
        public string businessUnit { get; set; }
        public string subBusinessUnit { get; set; }
        public string requestType { get; set; }
        public List<RequestVOList> requestVOList { get; set; }
    }

    public class RequestVOList
    {
        public string aadhar { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string concatAddress { get; set; }
        public string country { get; set; }
        public string customerId { get; set; }
        public string digitalID { get; set; }
        public string din { get; set; }
        public string dob { get; set; }
        public string docNumber { get; set; }
        public string drivingLicence { get; set; }
        public string email { get; set; }
        public string entityName { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string pan { get; set; }
        public string passport { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
        public string rationCardNo { get; set; }
        public string ssn { get; set; }
        public string state { get; set; }
        public string tin { get; set; }
        public string voterId { get; set; }
    }

    public class ListMatchingPayload
    {
        public RequestListVO requestListVO { get; set; }
    }

    public class RampRequest
    {
        public ListMatchingPayload listMatchingPayload { get; set; }
    }

    public class PostRampRequest
    {
        public RampRequest rampRequest { get; set; }
    }

    #endregion

    #region Prosidex_Class

    public class DG
    {
        public string ACCOUNT_NUMBER { get; set; }
        public string ALIAS_NAME { get; set; }
        public string APPLICATIONID { get; set; }
        public string Aadhar { get; set; }
        public string CIN { get; set; }
        public string CKYC { get; set; }
        public string CUSTOMER_STATUS { get; set; }
        public string CUST_ID { get; set; }
        public string DOB { get; set; }
        public string DrivingLicense { get; set; }
        public string Father_First_Name { get; set; }
        public string Father_Last_Name { get; set; }
        public string Father_Middle_Name { get; set; }
        public string Father_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Gender { get; set; }
        public string GSTIN { get; set; }
        public string Lead_Id { get; set; }
        public string NREGA { get; set; }
        public string Pan { get; set; }
        public string PassportNo { get; set; }
        public string RELATION_TYPE { get; set; }
        public string RationCard { get; set; }
        public string Registration_NO { get; set; }
        public string SALUTATION { get; set; }
        public string TAN { get; set; }
        public string Udyam_aadhar_number { get; set; }
        public string VoterId { get; set; }
        public string Tasc_Customer { get; set; }
    }

    public class Request
    {
        public DG DG { get; set; }
        public List<object> ACE { get; set; }
        public string UnitySfb_RequestId { get; set; }
        public string CUST_TYPE { get; set; }
        public string CustomerCategory { get; set; }
        public string MatchingRuleProfile { get; set; }
        public string Req_flag { get; set; }
        public string SourceSystem { get; set; }
    }

    public class ProsidexRequest
    {
        public Request Request { get; set; }
    }

    public class ProsiReq
    {
        public string pMemberId { get; set; }
        public string pCreatedBy { get; set; }
    }

    public class ProsidexResponse
    {
        public string RequestId { get; set; }
        public string UCIC { get; set; }
        public int response_code { get; set; }
        public ProsidexResponse(string RequestId, string UCIC, int response_code)
        {
            this.RequestId = RequestId;
            this.UCIC = UCIC;
            this.response_code = response_code;
        }
    }

    #endregion
}
