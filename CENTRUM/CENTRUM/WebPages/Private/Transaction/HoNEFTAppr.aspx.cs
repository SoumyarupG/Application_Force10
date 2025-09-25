using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.Data;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HoNEFTAppr : CENTRUMBase
    {

        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                PopBranch(Session[gblValue.UserName].ToString());
                //LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);

            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HO Disbursement Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHoNEFT);
                //if (this.UserID == 1) return;
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                    btnDone.Visible = false;
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    //if (rdbOpt.SelectedValue == "N")
                    //    LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    //else if (rdbOpt.SelectedValue == "A")
                    //    LoadGrid("A", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool ValidDate()
        {
            Boolean vResult = true;
            if (txtFrmDt.Text.Trim() != "" || txtFrmDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtFrmDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtFrDt");
                    vResult = false;
                }
            }
            if (txtToDt.Text.Trim() != "" || txtToDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtToDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtToDt");
                    vResult = false;
                }
            }
            if (txtAppDt.Text.Trim() != "" || txtAppDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAppDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtSancDt");
                    vResult = false;
                }
            }
            return vResult;
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            CApplication oApp = null;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = null;
            Int32 vErr = 0;
            string vXmlData = "", vXml = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            CHoliday oHoli = null;
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    //if (ValidateFields() == false) return;
                    Int32 vRow = 0;
                    for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
                    {
                        CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkAppr");
                        CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCash");
                        // CheckBox chkMedi = (CheckBox)gvSanc.Rows[vRow].FindControl("chkMedi");
                        TextBox txtExpDisbDate = (TextBox)gvSanc.Rows[vRow].FindControl("txtExpDisbDate");
                        DropDownList ddlHospiCash = (DropDownList)gvSanc.Rows[vRow].FindControl("ddlHospiCash");
                        DateTime vCBDate = gblFuction.setDate(gvSanc.Rows[vRow].Cells[31].Text);
                        Label txtMem = (Label)gvSanc.Rows[vRow].FindControl("txtMem");
                        //**********************Group Date Check***********************                      
                        if (chkApp.Checked == true)
                        {
                            string DisbDt = txtExpDisbDate.Text;
                            string Group = gvSanc.Rows[vRow].Cells[30].Text;
                            for (int i = 0; i < gvSanc.Rows.Count; i++)
                            {
                                CheckBox chkAppi = (CheckBox)gvSanc.Rows[i].FindControl("chkAppr");
                                TextBox txtDisbDate = (TextBox)gvSanc.Rows[i].FindControl("txtExpDisbDate");
                                Label txtMem1 = (Label)gvSanc.Rows[i].FindControl("txtMem");
                                if (chkAppi.Checked == true)
                                {
                                    if (Group == gvSanc.Rows[i].Cells[30].Text)
                                    {
                                        if (DisbDt != txtDisbDate.Text)
                                        {
                                            gblFuction.AjxMsgPopup("Expected disbursement date should not be different in one group.");
                                            return;
                                        }
                                    }
                                }

                                oHoli = new CHoliday();
                                dt1 = oHoli.IsHolidaySP(gvSanc.Rows[i].Cells[35].Text, gblFuction.setDate(txtDisbDate.Text));
                                if (dt1.Rows.Count > 0)
                                {
                                    if (Convert.ToString(dt1.Rows[0]["Holiday"]) == "Y")
                                    {
                                        gblFuction.AjxMsgPopup("Expected disbursement date should not be Holiday for Member " + txtMem1.Text);
                                        return;
                                    }
                                }
                            }
                            double vTotDays = (vSanDt - vCBDate).TotalDays;
                            if (vTotDays > 15)
                            {
                                gblFuction.AjxMsgPopup("Warning! You cannot Approve the loan of this member (" + txtMem.Text + ") due to CB Approval date is more than 15days");
                                return;
                            }

                        }
                        //*************************************************************
                        TextBox txtAccountNo = (TextBox)gvSanc.Rows[vRow].FindControl("txtAccountNo");
                        TextBox txtIFSC = (TextBox)gvSanc.Rows[vRow].FindControl("txtIfsc");
                        CheckBox cbSendBack = (CheckBox)gvSanc.Rows[vRow].FindControl("cbSendBack");

                        dt.Rows[vRow]["AccNo"] = txtAccountNo.Text;
                        dt.Rows[vRow]["IFSCCode"] = txtIFSC.Text;
                        if ((dt.Rows[vRow]["CashApproveYN"].ToString() == "N") && (dt.Rows[vRow]["HOApproveYN"].ToString() == "Y"))
                        {
                            dt.Rows[vRow]["HOApproveYN"] = "Y";
                            dt.Rows[vRow]["ExpDate"] = gblFuction.setDate(txtExpDisbDate.Text);
                        }
                        //else
                        //{
                        //    dt.Rows[vRow]["ExpDate"] = gblFuction.setDate(txtExpDisbDate.Text);
                        //}

                        if ((dt.Rows[vRow]["CashApproveYN"].ToString() == "Y") && (dt.Rows[vRow]["HOApproveYN"].ToString() == "N"))
                        {
                            dt.Rows[vRow]["HOApproveYN"] = "Y";
                            dt.Rows[vRow]["CashApproveYN"] = "Y";
                            dt.Rows[vRow]["NEFTApproveYN"] = "N";
                        }
                        if ((dt.Rows[vRow]["HOApproveYN"].ToString() == "N") && (dt.Rows[vRow]["CashApproveYN"].ToString() == "N"))
                        {
                            dt.Rows[vRow]["HOApproveYN"] = "N";
                            dt.Rows[vRow]["CashApproveYN"] = "N";
                            dt.Rows[vRow]["NEFTApproveYN"] = "Y";
                        }
                        if (ddlHospiCash.SelectedValue != "-1")
                        {
                            dt.Rows[vRow]["MediclaimYN"] = "Y";
                            dt.Rows[vRow]["HospiId"] = ddlHospiCash.SelectedValue;
                        }
                        else
                        {
                            dt.Rows[vRow]["MediclaimYN"] = "N";
                            dt.Rows[vRow]["HospiId"] = ddlHospiCash.SelectedValue;
                        }
                        if (cbSendBack.Checked == true)
                        {
                            dt.Rows[vRow]["SendBackYN"] = "Y";
                        }
                        else
                        {
                            dt.Rows[vRow]["SendBackYN"] = "N";
                        }

                    }

                    dt.AcceptChanges();
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["AccNo"].ToString() == "")
                            {
                                gblFuction.MsgPopup("Please enter Account Number..");
                                return;
                            }
                            if (dt.Rows[i]["IFSCCode"].ToString() == "")
                            {
                                gblFuction.MsgPopup("Please enter IFSC Code..");
                                return;
                            }

                        }
                        CNEFTTransfer oNEFT = new CNEFTTransfer();
                        string vMsg = "";
                        int vEr = oNEFT.ChkSurplusAmt(vXmlData, vSanDt, ref vMsg, "HO");
                        if (vEr > 0)
                        {
                            gblFuction.AjxMsgPopup(vMsg.ToString());
                            return;
                        }
                        //------------------------------------------  
                        dt2 = new DataTable();
                        dt2 = Xml();
                        using (StringWriter oSW = new StringWriter())
                        {
                            dt2.WriteXml(oSW);
                            vXml = oSW.ToString();
                        }
                        oNEFT = new CNEFTTransfer();
                        int err = 0;
                        err = oNEFT.chkGroupAndLoanAmt(vXml, ref vMsg);
                        if (err == 1)
                        {
                            gblFuction.AjxMsgPopup(vMsg.ToString());
                            return;
                        }
                        //----------------------------------
                        DataSet ds = new DataSet();
                        DataTable DtGrp = new DataTable();
                        //DataTable dtLoanScheme = new DataTable();
                        //DataTable dtApplAmt = new DataTable();
                        DataTable dtInstNo = new DataTable();
                        DataTable dtWeeklyCenterMemCnt = new DataTable();
                        DataTable dtMonthlyMemCnt = new DataTable();
                        DataTable dtActMemInACenter = new DataTable();
                        DataTable dtActMemInACenterLimit = new DataTable();
                        DataTable dtMonthlyGrActMemChk = new DataTable();
                        DataTable dtWeeklyGrActMemChk = new DataTable();

                        string vMesg = "";
                        ds = oApp.ChkActiveMemberinaGroup(vSanDt, vXmlData, "H");
                        DtGrp = ds.Tables[0];
                        //dtLoanScheme = ds.Tables[1];
                        //dtApplAmt = ds.Tables[2];
                        dtInstNo = ds.Tables[1];
                        dtWeeklyCenterMemCnt = ds.Tables[2];
                        dtMonthlyMemCnt = ds.Tables[3];
                        dtActMemInACenter = ds.Tables[4];
                        dtActMemInACenterLimit = ds.Tables[5];
                        dtMonthlyGrActMemChk = ds.Tables[6];
                        dtWeeklyGrActMemChk = ds.Tables[7];
                        ////-------------CENTR - 1780-------------------
                        if (DtGrp.Rows.Count > 0)
                        {
                            for (int j = 0; j <= DtGrp.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against Group " + DtGrp.Rows[j]["GroupName"].ToString() + " Disbursed member count is " + DtGrp.Rows[j]["XmlMemberCount"].ToString() + " out of " + DtGrp.Rows[j]["OriginalMemberCount"].ToString() + " ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }
                        //if (dtLoanScheme.Rows.Count > 0)
                        //{
                        //    for (int j = 0; j <= dtLoanScheme.Rows.Count - 1; j++)
                        //    {
                        //        vMesg = vMesg + "Against Group " + dtLoanScheme.Rows[j]["GroupName"].ToString() + " Loan Scheme Should be Same. ";
                        //    }
                        //    gblFuction.MsgPopup(vMesg);
                        //    return;
                        //}
                        //if (dtApplAmt.Rows.Count > 0)
                        //{
                        //    for (int j = 0; j <= dtApplAmt.Rows.Count - 1; j++)
                        //    {
                        //        vMesg = vMesg + "Against Group " + dtApplAmt.Rows[j]["GroupName"].ToString() + " Application Amount Should be Same. ";
                        //    }
                        //    gblFuction.MsgPopup(vMesg);
                        //    return;
                        //}
                        if (dtInstNo.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtInstNo.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against Group " + dtInstNo.Rows[j]["GroupName"].ToString() + " Loan Tenure Should be Same. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }
                        ////-------------CENTR - 3000-------------------
                        if (dtWeeklyCenterMemCnt.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtWeeklyCenterMemCnt.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against Center " + dtWeeklyCenterMemCnt.Rows[j]["Market"].ToString() + " Must be Disburse With 5 Customer. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }
                        //if (dtMonthlyMemCnt.Rows.Count > 0)
                        //{
                        //    for (int j = 0; j <= dtMonthlyMemCnt.Rows.Count - 1; j++)
                        //    {
                        //        vMesg = vMesg + "Against Group " + dtMonthlyMemCnt.Rows[j]["GroupName"].ToString() + " Must be Disburse With 2 Customer. ";
                        //    }
                        //    gblFuction.MsgPopup(vMesg);
                        //    return;
                        //}
                        if (dtActMemInACenter.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtActMemInACenter.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against center " + dtActMemInACenter.Rows[j]["MarketName"].ToString() + " maximum 36 customer can be disburse. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }

                        if (dtActMemInACenterLimit.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtActMemInACenterLimit.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against center " + dtActMemInACenterLimit.Rows[j]["MarketName"].ToString() + " mininum 1 and maximum 36 customer can be have active loan. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }

                        if (dtMonthlyGrActMemChk.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtMonthlyGrActMemChk.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against Monthly Group " + dtMonthlyGrActMemChk.Rows[j]["GroupNameM"].ToString() + " No of Active member should not be more than 12. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }

                        if (dtWeeklyGrActMemChk.Rows.Count > 0)
                        {
                            for (int j = 0; j <= dtWeeklyGrActMemChk.Rows.Count - 1; j++)
                            {
                                vMesg = vMesg + "Against Weekly Group " + dtWeeklyGrActMemChk.Rows[j]["GroupNameW"].ToString() + " No of Active member should not be more than 5. ";
                            }
                            gblFuction.MsgPopup(vMesg);
                            return;
                        }

                        ////---------------End CENTR - 1780--------------------

                        vErr = oApp.InsertNEFTHOApprove(vXmlData, Convert.ToInt32(Session[gblValue.UserId]), vBrCode, "E", 0, vSanDt);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.SaveMsg);
                            LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                            rdbOpt.SelectedValue = "N";
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please select atleast One row...");
                    }
                }
                finally
                {
                    oApp = null;
                    dt = null;
                }
            }
        }

        protected void txtAppDt_TextChanged(object sender, EventArgs e)
        {
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            if (vAppDt > vLoginDt)
            {
                gblFuction.MsgPopup("Approved date should not grater than login date..");
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();  //Convert.ToString(vLoginDt);
                return;
            }
        }

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            Int32 vRow = 0;
            for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
            {
                CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkAppr");
                CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCash");
                TextBox txtAccountNo = (TextBox)gvSanc.Rows[vRow].FindControl("txtAccountNo");
                TextBox txtIFSC = (TextBox)gvSanc.Rows[vRow].FindControl("txtIfsc");
                if (chkApp.Checked == true || chkCan.Checked == true)
                {
                    if (txtAccountNo.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please enter Account Number..");
                        vResult = false;
                    }
                    if (txtIFSC.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please enter IFSC Code..");
                        vResult = false;
                    }
                }
                if (chkApp.Checked == false && chkCan.Checked == false)
                {

                }
            }
            return vResult;
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

        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;
            string vGroupId, vMarketId = null;

            if (ddlBranch.SelectedValues == "")
            {
                gblFuction.AjxMsgPopup("Please Select branch...");
                return;
            }

            if (hdGrpId.Value == "-1" || hdGrpId.Value == "" || txtGroup.Text.Trim() == "")
            {
                vGroupId = "";
            }
            else
            {
                vGroupId = hdGrpId.Value;
            }
            if (hdCntrId.Value == "-1" || hdCntrId.Value == "" || txtCenter.Text.Trim() == "")
            {
                vMarketId = "";
            }
            else
            {
                vMarketId = hdCntrId.Value;
            }

            try
            {

                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetNEFTHoApprList(vFromDt, vToDt, pAppMode, ddlBranch.SelectedValues.Replace("|", ","), pPgIndx, ref vRows, vGroupId, vMarketId);
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
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
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }

        public void GetData()
        {
            DataTable dt = (DataTable)ViewState["Sanc"];
            foreach (GridViewRow gr in gvSanc.Rows)
            {
                CheckBox chkCash = (CheckBox)gr.FindControl("chkCash");
                CheckBox chkNEFT = (CheckBox)gr.FindControl("chkNEFT");
                if (chkCash.Checked == true)
                {
                    dt.Rows[gr.RowIndex]["CashApproveYN"] = "Y";
                    dt.Rows[gr.RowIndex]["NEFTApproveYN"] = "N";
                    dt.Rows[gr.RowIndex]["HOApproveYN"] = "Y";
                }
            }
        }

        protected void chkAppr_CheckChanged(object sender, EventArgs e)
        {
            string vBranch = "",vMsg = "";//, vAppId = "";
            CApplication oCG = null;
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["Sanc"];

                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkAppr = (CheckBox)row.FindControl("chkAppr");
                CheckBox chkCash = (CheckBox)row.FindControl("chkCash");
                CheckBox cbSendBack = (CheckBox)row.FindControl("cbSendBack");
                string LoanAppID = Convert.ToString(row.Cells[14].Text);
                CMember cMem = new CMember();
                string vDoneYN = "";
                if (checkbox.Checked == true)
                {
                    vDoneYN = cMem.ChkDiscrepancy(LoanAppID, "0000");
                    if (vDoneYN == "N")
                    {
                        gblFuction.AjxMsgPopup("With Discrepancy Cannot Approve .");
                        checkbox.Checked = false;
                        checkbox.Enabled = true;
                        cbSendBack.Checked = false;
                        cbSendBack.Enabled = true;
                        return;
                    }
                    else
                    {

                        cbSendBack.Checked = false;
                        cbSendBack.Enabled = false;
                        checkbox.Checked = true;
                        checkbox.Enabled = true;

                    }

                    oCG = new CApplication();
                    vMsg = oCG.ChkCollRoutine(LoanAppID); //Previous Loan Over Due checking incorporated
                    if (vMsg != "")
                    {
                        gblFuction.AjxMsgPopup(vMsg);
                        checkbox.Checked = false;
                        return;
                    }

                    if (gblFuction.setDate(row.Cells[13].Text) > gblFuction.setDate(txtAppDt.Text))
                    {
                        gblFuction.AjxMsgPopup("Branch Approval Date Cannot be Greater Than HO approve");
                        checkbox.Checked = false;
                        return;
                    }
                    if (row.Cells[15].Text != "N")
                    {
                        gblFuction.AjxMsgPopup("This application is already disbursed");
                        checkbox.Checked = false;
                        return;
                    }
                    //if (chkAppr.Checked == true)
                    //{
                    //    dt.Rows[row.RowIndex]["HOApproveYN"] = "Y";
                    //}
                    //if (chkCash.Checked == true)
                    //{
                    //    dt.Rows[row.RowIndex]["HOApproveYN"] = "Y";
                    //    dt.Rows[row.RowIndex]["CashApproveYN"] = "Y";
                    //    dt.Rows[row.RowIndex]["NEFTApproveYN"] = "N";
                    //}
                    else
                    {
                        dt.Rows[row.RowIndex]["HOApproveYN"] = "Y";
                        chkCash.Enabled = false;
                    }
                }
                else
                {

                    if (row.Cells[15].Text != "N")
                    {
                        gblFuction.AjxMsgPopup("This application is already disbursed");
                        checkbox.Checked = true;
                        return;
                    }
                    else
                    {
                        dt.Rows[row.RowIndex]["HOApproveYN"] = "N";
                        //dt.Rows[row.RowIndex]["NEFTApproveYN"] = "Y";
                        chkCash.Enabled = true;
                    }
                }
                dt.AcceptChanges();
                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            finally
            {
                //oApp = null;
                dt = null;
            }
        }

        protected void cbSendBack_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)((CheckBox)sender).NamingContainer;
            CheckBox cbSendBack = (CheckBox)gvRow.FindControl("cbSendBack");
            CheckBox chkAppr = (CheckBox)gvRow.FindControl("chkAppr");
            string LoanAppID = Convert.ToString(gvRow.Cells[14].Text);
            CMember cMem = new CMember();
            string vDoneYN = "";
            if (cbSendBack.Checked == true)
            {
                vDoneYN = cMem.ChkDiscrepancy(LoanAppID, "0000");
                if (vDoneYN == "N")
                {
                    cbSendBack.Checked = true;
                    chkAppr.Enabled = false;
                    //chkAppr.Checked = false;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Without Discrepancy Cannot Sendback .");
                    cbSendBack.Checked = false;
                    //chkAppr.Checked = true;
                    chkAppr.Enabled = true;

                }
            }
            else
            {
                chkAppr.Enabled = true;

            }
        }

        //protected void chkMedi_CheckChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = (DataTable)ViewState["Sanc"];
        //        CheckBox checkbox = (CheckBox)sender;
        //        GridViewRow row = (GridViewRow)checkbox.NamingContainer;
        //        if (checkbox.Checked == true)
        //        {
        //            dt.Rows[row.RowIndex]["MediclaimYN"] = "Y";
        //        }
        //        else
        //        {
        //            dt.Rows[row.RowIndex]["MediclaimYN"] = "N";
        //        }
        //        dt.AcceptChanges();
        //        ViewState["Sanc"] = dt;
        //        upSanc.Update();
        //    }
        //    finally
        //    {
        //        //oApp = null;
        //        dt = null;
        //    }
        //}

        protected void txtAccountNo_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtAccountNo = (TextBox)gvRow.FindControl("txtAccountNo");
            dt = (DataTable)ViewState["Sanc"];

            if (txtAccountNo.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["AccNo"] = txtAccountNo.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void txtIfsc_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtIfsc = (TextBox)gvRow.FindControl("txtIfsc");
            dt = (DataTable)ViewState["Sanc"];

            if (txtIfsc.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["IFSCCode"] = txtIfsc.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void chkCash_CheckChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkAppr = (CheckBox)row.FindControl("chkAppr");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (row.Cells[15].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already disbursed");
                    checkbox.Checked = false;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["CashApproveYN"] = "Y";
                    //dt.Rows[row.RowIndex]["NEFTApproveYN"] = "N";
                    //dt.Rows[row.RowIndex]["HOApproveYN"] = "N";
                    chkAppr.Enabled = false;
                }
            }
            else
            {
                if (row.Cells[15].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already approved by HO");
                    checkbox.Checked = true;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["CashApproveYN"] = "N";
                    //dt.Rows[row.RowIndex]["NEFTApproveYN"] = "Y";
                    //dt.Rows[row.RowIndex]["HOApproveYN"] = "N";
                    chkAppr.Enabled = true;
                }
            }
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }

            LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);
            //tabLoanAppl.ActiveTabIndex = 0;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkAppr = (CheckBox)e.Row.FindControl("chkAppr");
                    CheckBox chkCash = (CheckBox)e.Row.FindControl("chkCash");
                    //CheckBox chkMedi = (CheckBox)e.Row.FindControl("chkMedi");
                    DropDownList ddlHospiCash = (DropDownList)e.Row.FindControl("ddlHospiCash");
                    TextBox txtAccountNo = (TextBox)e.Row.FindControl("txtAccountNo");
                    TextBox txtIfsc = (TextBox)e.Row.FindControl("txtIfsc");
                    TextBox txtExpDisbDate = (TextBox)e.Row.FindControl("txtExpDisbDate");
                    DateTime vCBDate = gblFuction.setDate(e.Row.Cells[31].Text);
                    if (e.Row.Cells[11].Text == "Y")
                    {
                        chkAppr.Checked = true;
                        txtExpDisbDate.Enabled = false;
                        txtIfsc.Enabled = false;
                        txtAccountNo.Enabled = false;
                    }
                    else if (e.Row.Cells[11].Text == "N")
                    {
                        chkAppr.Checked = false;

                    }
                    if (rdbOpt.SelectedValue == "N" || rdbOpt.SelectedValue == "S")
                    {
                        double vTotDays = (vSanDt - vCBDate).TotalDays;
                        if (vTotDays > 15)
                        {
                            e.Row.BackColor = System.Drawing.Color.PeachPuff;
                        }
                    }

                    //if (rdbOpt.SelectedValue != "S")
                    //{
                    //    gvSanc.Columns[38].Visible = false;
                    //}
                    //else
                    //{
                    //    gvSanc.Columns[38].Visible = true;
                    //}

                    if (e.Row.Cells[34].Text == "Y")
                    {
                        //chkMedi.Checked = true;
                    }
                    else
                    {
                        //chkMedi.Checked = false;
                    }
                    //Bind Calcel

                    //-------------------------------------------------------------------------
                    ddlHospiCash.Items.Clear();
                    oGb = new CGblIdGenerator();
                    //dt = oGb.PopComboMIS("N", "N", "AA", "HospiId", "HospiName", "HospiMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");

                    dt = oGb.popHospiCash(vSanDt, e.Row.Cells[35].Text.Trim(), Convert.ToInt32(e.Row.Cells[36].Text.Trim()));
                    if (dt.Rows.Count > 0)
                    {
                        ddlHospiCash.DataSource = dt;
                        ddlHospiCash.DataTextField = "HospiName";
                        ddlHospiCash.DataValueField = "HospiId";
                        ddlHospiCash.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlHospiCash.Items.Insert(0, oLc);
                    ddlHospiCash.SelectedIndex = ddlHospiCash.Items.IndexOf(ddlHospiCash.Items.FindByValue(e.Row.Cells[36].Text.Trim()));
                    //---------------------------------------------------------------------------
                }
            }
            finally
            {

            }
        }

        private DataTable Xml()
        {
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("LoanAmt");
            dt.Columns.Add("DisbYN");
            dt.Columns.Add("GroupId");
            dt.Columns.Add("GroupName");
            int i = 0;
            foreach (GridViewRow gr in gvSanc.Rows)
            {
                CheckBox chkNEFT = (CheckBox)gr.FindControl("chkAppr");
                DataRow dr = dt.NewRow();
                dr["SlNo"] = i;
                dr["LoanAppId"] = gr.Cells[14].Text;
                dr["LoanAmt"] = gr.Cells[9].Text;
                dr["DisbYN"] = chkNEFT.Checked == true ? "Y" : "N";
                dr["GroupId"] = gr.Cells[30].Text;
                dr["GroupName"] = gr.Cells[3].Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                i = i + 1;
            }
            return dt;
        }

        protected void rdbOpt_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();           
            LoadGrid(rdbOpt.SelectedValue, txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
        }
    }
}