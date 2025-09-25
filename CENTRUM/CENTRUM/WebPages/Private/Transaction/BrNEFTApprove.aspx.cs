using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Net.Sockets;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class BrNEFTApprove : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected string vValidationStatus = "Not Match";

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
                LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                popLO();
                PopCenter();
                popGroup();
            }
        }

        private void InitBasePage()
        {
            DataTable DtBrCntrl = null;
            try
            {
                this.Menu = false;
                this.PageHeading = "Pre Disbursement Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBrNEFT);
                //if (this.UserID == 1) return;


                if (Session["BrCntrl"] != null)
                {
                    DtBrCntrl = (DataTable)Session["BrCntrl"];
                    if (Convert.ToString(DtBrCntrl.Rows[0]["PreDBJLG"]) == "N")
                    {
                        //gblFuction.AjxMsgPopup("Pre DB is not allowed in this Branch");
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre Disbursement Approval", false);
                    }
                }

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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre Disbursement Approval", false);
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
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "T")
                        LoadGrid("T", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "C")
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
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
            Int32 vErr = 0;
            string vXmlData = "", vXml = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            Int32 vRow = 0;
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            CHoliday oHoli = null;
            DataTable dt2 = new DataTable();

            if (ddlOperation.SelectedValue == "A")
            {
                //**********************Group Date Check***********************
                for (vRow = 0; vRow < gvSanc.Rows.Count; vRow++)
                {
                    CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkNEFT");
                    TextBox txtExpDisbDate = (TextBox)gvSanc.Rows[vRow].FindControl("txtExpDisbDate");
                    string DisbDt = txtExpDisbDate.Text;
                    string Group = gvSanc.Rows[vRow].Cells[22].Text;
                    string vAssetType = gvSanc.Rows[vRow].Cells[36].Text;
                    TextBox txtSelfIncome = (TextBox)gvSanc.Rows[vRow].FindControl("txtSelfIncome");
                    TextBox txtFamilyIncome = (TextBox)gvSanc.Rows[vRow].FindControl("txtFamilyIncome");
                    DateTime vCBDate = gblFuction.setDate(gvSanc.Rows[vRow].Cells[27].Text);
                    TextBox txtIfsc = (TextBox)gvSanc.Rows[vRow].FindControl("txtIfsc");
                    CApplication oCG = new CApplication();
                    if (chkApp.Checked == true)
                    {
                        if (gvSanc.Rows[vRow].Cells[40].Text == "N" && txtIfsc.Text.Trim().Substring(0, 4) != "UCBA")
                        {
                            gblFuction.AjxMsgPopup("Please verify bank account details.");
                            return;
                        }
                        if (txtExpDisbDate.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Expected disbursement date can not be empty.");
                            return;
                        }
                        if (rdbOpt.SelectedValue == "N")
                        {
                            double vTotDays = (vSanDt - vCBDate).TotalDays;
                            if (vTotDays > 15)
                            {
                                gblFuction.AjxMsgPopup("CB Enquiry date is more than 15 days.");
                                return;
                            }
                        }
                        if (vAssetType == "N")
                        {
                            if (txtSelfIncome.Text == "0" || txtSelfIncome.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please fill Self Income.");
                                return;
                            }

                            if (txtFamilyIncome.Text == "0" || txtFamilyIncome.Text == "")
                            {
                                gblFuction.AjxMsgPopup("Please fill Self Income.");
                                return;
                            }
                            if (Convert.ToDouble(txtSelfIncome.Text) + Convert.ToDouble(txtFamilyIncome.Text) < 26000)
                            {
                                gblFuction.AjxMsgPopup("Total Income should not be less than 26000.");
                                return;
                            }

                            if (Convert.ToDouble(txtSelfIncome.Text) + Convert.ToDouble(txtFamilyIncome.Text) > 40000)
                            {
                                gblFuction.AjxMsgPopup("Total Income should not be more than 40000.");
                                return;
                            }
                        }
                        double vTotalAmt = Convert.ToDouble(txtSelfIncome.Text == "" ? "0" : txtSelfIncome.Text)
                                + Convert.ToDouble(txtFamilyIncome.Text == "" ? "0" : txtFamilyIncome.Text);
                        string vPassYN = oCG.chkFOIR(gvSanc.Rows[vRow].Cells[19].Text, vTotalAmt);
                        if (vPassYN == "N" && ddlOperation.SelectedValue == "A")
                        {
                            gblFuction.AjxMsgPopup("FOIR Breached!");
                            return;
                        }

                        for (int i = 0; i < gvSanc.Rows.Count; i++)
                        {
                            CheckBox chkAppi = (CheckBox)gvSanc.Rows[i].FindControl("chkNEFT");
                            TextBox txtDisbDate = (TextBox)gvSanc.Rows[i].FindControl("txtExpDisbDate");
                            Label txtMem = (Label)gvSanc.Rows[i].FindControl("txtMem");
                            if (chkAppi.Checked == true)
                            {
                                if (Group == gvSanc.Rows[i].Cells[22].Text)
                                {
                                    if (DisbDt != txtDisbDate.Text)
                                    {
                                        gblFuction.AjxMsgPopup("Expected disbursement date should not be different in one group.");
                                        return;
                                    }
                                }
                                oHoli = new CHoliday();
                                dt1 = oHoli.IsHolidaySP(Session[gblValue.BrnchCode].ToString(), gblFuction.setDate(txtDisbDate.Text));
                                if (dt1.Rows.Count > 0)
                                {
                                    if (Convert.ToString(dt1.Rows[0]["Holiday"]) == "Y")
                                    {
                                        gblFuction.AjxMsgPopup("Expected disbursement date should not be Holiday for Member " + txtMem.Text);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                //*************************************************************
            }
            if (ValidDate() == true)
            {
                try
                {
                    GetData();
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    //if (ValidateFields() == false) return; 

                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    if (ddlOperation.SelectedValue == "A")
                    {
                        CNEFTTransfer oNEFT = null;
                        string vMsg = "";
                        oNEFT = new CNEFTTransfer();
                        int vEr = oNEFT.ChkSurplusAmt(vXmlData, vSanDt, ref vMsg, "Branch");
                        if (vEr > 0)
                        {
                            gblFuction.AjxMsgPopup(vMsg.ToString());
                            return;
                        }

                        int vCollstdtEr = oNEFT.ChkCollStDate(vXmlData, ref vMsg);
                        if (vCollstdtEr > 0)
                        {
                            gblFuction.AjxMsgPopup(vMsg.ToString());
                            return;
                        }
                        //------------------------------------------                    
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
                        ds = oApp.ChkActiveMemberinaGroup(vSanDt, vXmlData, "P");
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
                                vMesg = vMesg + "Against Center " + dtWeeklyCenterMemCnt.Rows[j]["Market"].ToString() + " Must be Disburse With 2 Customer. ";
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
                                vMesg = vMesg + "Against center " + dtActMemInACenterLimit.Rows[j]["MarketName"].ToString() + " mininum 1 and maximum 36 customer can be have active loan including new disbursement. ";
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
                    }
                    //-------------------------------
                    oApp = new CApplication();
                    //-----------XML Save----------
                    vErr = oApp.InsertNEFTBrApprove(vXmlData, Convert.ToInt32(Session[gblValue.UserId]), vBrCode, "E", 0, vSanDt, ddlOperation.SelectedValue);
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
                CheckBox chkApp = (CheckBox)gvSanc.Rows[vRow].FindControl("chkApp");
                CheckBox chkCan = (CheckBox)gvSanc.Rows[vRow].FindControl("chkCan");
                TextBox txtSanAmt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSanAmt");
                TextBox txtSanDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtSanDt");
                TextBox txtReason = (TextBox)gvSanc.Rows[vRow].FindControl("txtReason");
                TextBox txtEQFDt = (TextBox)gvSanc.Rows[vRow].FindControl("txtEQFDt");
                TextBox txtNoMfi = (TextBox)gvSanc.Rows[vRow].FindControl("txtNoMfi");
                TextBox txtTOTOS = (TextBox)gvSanc.Rows[vRow].FindControl("txtTOTOS");
                if (chkApp.Checked == true || chkCan.Checked == true)
                {
                    if (chkApp.Checked == true && txtSanDt.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please Enter Sanction Date..");
                        vResult = false;
                    }

                    if (chkApp.Checked == true && txtSanAmt.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please Enter Sanction Amount..");
                        vResult = false;
                    }

                    if (chkCan.Checked == true && txtReason.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please select the Reason for Cancel loan..");
                        vResult = false;
                    }

                    if (chkCan.Checked == true && txtReason.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please select the Reason for Cancel loan..");
                        vResult = false;
                    }

                    if (chkApp.Checked == true && gblFuction.IsDate(txtSanDt.Text) == false)
                    {
                        gblFuction.MsgPopup("Please enter Proper Sanction Date..");
                        vResult = false;
                    }

                    if (txtEQFDt.Text.Trim() != "" && gblFuction.IsDate(txtEQFDt.Text) == false)
                    {
                        gblFuction.MsgPopup("Please enter Proper Eqi Fax Date..");
                        vResult = false;
                    }
                    if (txtEQFDt.Text.Trim() != "" && gblFuction.IsDate(txtEQFDt.Text) == false)
                    {
                        gblFuction.MsgPopup("Please enter Proper Eqi Fax Date..");
                        vResult = false;
                    }

                    if (txtNoMfi.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("No of MFI Should Not be Blank..");
                        vResult = false;
                    }

                    if (txtTOTOS.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Total Outstanding Should Not be Blank..");
                        vResult = false;
                    }
                }
                if (chkApp.Checked == false && chkCan.Checked == false)
                {

                }
            }
            return vResult;
        }

        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;
            Int32 vLoanProduct = 0;
            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                DateTime vLogDt = gblFuction.setDate(txtAppDt.Text);
                string vGroupId = ddlGroup.SelectedValue;
                string vMarketId = ddlCentr.SelectedValue;
                string vLoId = ddlLO.SelectedValue;
                oLS = new CApplication();
                dt = oLS.GetNEFTBRApprList(vFromDt, vToDt, pAppMode, vBrCode, pPgIndx, ref vRows, this.RoleId, vLogDt, vGroupId, vMarketId, vLoId);
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                gvSanc.Enabled = pAppMode == "N" ? true : false;
                hdnOpt.Value = pAppMode;


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
                gvSanc.Enabled = pAppMode == "N" ? true : false;
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
                CheckBox chkNEFT = (CheckBox)gr.FindControl("chkNEFT");
                TextBox txtExpDisbDate = (TextBox)gr.FindControl("txtExpDisbDate");
                TextBox txtSelfIncome = (TextBox)gr.FindControl("txtSelfIncome");
                TextBox txtFamilyIncome = (TextBox)gr.FindControl("txtFamilyIncome");
                TextBox txtCollStDate = (TextBox)gr.FindControl("txtCollStDate");
                if (chkNEFT.Checked == true)
                {
                    dt.Rows[gr.RowIndex]["ExpDate"] = gblFuction.setDate(txtExpDisbDate.Text);
                    dt.Rows[gr.RowIndex]["SlefIncome"] = txtSelfIncome.Text;
                    dt.Rows[gr.RowIndex]["FamilyIncome"] = txtFamilyIncome.Text;
                    dt.Rows[gr.RowIndex]["CollStDate"] = gblFuction.setDate(txtCollStDate.Text);
                }
            }
        }

        protected void chkCash_CheckChanged(object sender, EventArgs e)
        {

            DateTime vAppDate;
            string vLnAppDt = "", vBranch = "", vMsg = "";//, vAppId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkCash = (CheckBox)row.FindControl("chkCash");
                CheckBox chkNEFT = (CheckBox)row.FindControl("chkNEFT");
                oCG = new CApplication();
                if (checkbox.Checked == true)
                {
                    vMsg = oCG.ChkNeftCash(row.Cells[19].Text, vBranch);
                    if (row.Cells[20].Text == "Y")
                    {
                        gblFuction.AjxMsgPopup("This application is already approved by HO");
                        checkbox.Checked = false;
                        return;
                    }
                    else
                    {
                        if (vMsg == "NEFT")
                        {
                            gblFuction.AjxMsgPopup("This loan scheme is only for NEFT disbursement");
                            chkCash.Checked = false;
                            return;
                        }
                        else
                        {
                            dt.Rows[row.RowIndex]["CashApproveYN"] = "Y";
                            dt.Rows[row.RowIndex]["NEFTApproveYN"] = "N";
                            dt.Rows[row.RowIndex]["HOApproveYN"] = "Y";
                            chkNEFT.Enabled = false;
                        }
                    }
                }
                else
                {

                    if (row.Cells[20].Text != "N")
                    {
                        gblFuction.AjxMsgPopup("This application is already disbursed");
                        checkbox.Checked = true;
                        return;
                    }
                    else
                    {
                        dt.Rows[row.RowIndex]["CashApproveYN"] = "N";
                        dt.Rows[row.RowIndex]["HOApproveYN"] = "N";
                        chkNEFT.Enabled = true;
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

        protected void chkNEFT_CheckChanged(object sender, EventArgs e)
        {
            string vDoneYN = "", vMsg = "";
            DataTable dt = null;
            CApplication oCG = null;

            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkCash = (CheckBox)row.FindControl("chkCash");
            TextBox txtIfsc = (TextBox)row.FindControl("txtIfsc");
            TextBox txtSelfIncome = (TextBox)row.FindControl("txtSelfIncome");
            TextBox txtFamilyIncome = (TextBox)row.FindControl("txtFamilyIncome");
            string LoanAppID = Convert.ToString(row.Cells[19].Text);
            string vAssetType = Convert.ToString(row.Cells[36].Text);
            CMember cMem = new CMember();
            dt = (DataTable)ViewState["Sanc"];
            oCG = new CApplication();
            if (checkbox.Checked == true)
            {
                //vMsg = oCG.ChkNeftCash(row.Cells[12].Text, Session[gblValue.BrnchCode].ToString());
                vDoneYN = cMem.ChkDiscrepancy(LoanAppID, Session[gblValue.BrnchCode].ToString());
                if (vDoneYN == "N" && ddlOperation.SelectedValue == "A")
                {
                    gblFuction.AjxMsgPopup("Do the Branch Level Discrepancy before Approval .");
                    checkbox.Checked = false;
                    return;
                }
                //else if (vDoneYN == "N" && ddlOperation.SelectedValue == "C")
                //{
                //    gblFuction.AjxMsgPopup("Without Discrepancy Cannot Sendback.");
                //    checkbox.Checked = false;
                //    return;
                //}
                else
                {
                    checkbox.Checked = true;
                }

                if (row.Cells[40].Text == "N" && ddlOperation.SelectedValue == "A" && txtIfsc.Text.Trim().Substring(0, 4) != "UCBA")
                {
                    gblFuction.AjxMsgPopup("Please verify bank account details.");
                    checkbox.Checked = false;
                    return;
                }                
                if (row.Cells[20].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already disbursed");
                    checkbox.Checked = false;
                    return;
                }
                else
                {
                    if (ddlOperation.SelectedValue == "A")
                    {
                        vMsg = oCG.ChkNEFTData(row.Cells[19].Text, Session[gblValue.BrnchCode].ToString());
                    }
                    if (vMsg != "")
                    {
                        gblFuction.AjxMsgPopup(vMsg);
                        checkbox.Checked = false;
                        return;
                    }
                    //else if (Convert.ToDecimal(row.Cells[34].Text) >= 70)
                    //{
                    //    gblFuction.AjxMsgPopup("FOIR is more than 70 Percent");
                    //    checkbox.Checked = false;
                    //    return;
                    //}
                    else
                    {
                        if (ddlOperation.SelectedValue == "A")
                        {
                            vMsg = oCG.ChkCollRoutine(row.Cells[19].Text); //Previous Loan Over Due checking incorporated
                        }                      
                        
                        if (vMsg != "")
                        {
                            gblFuction.AjxMsgPopup(vMsg);
                            checkbox.Checked = false;
                            return;
                        }
                        else
                        {
                            double vTotalAmt = Convert.ToDouble(txtSelfIncome.Text == "" ? "0" : txtSelfIncome.Text)
                                + Convert.ToDouble(txtFamilyIncome.Text == "" ? "0" : txtFamilyIncome.Text);
                            string vPassYN = oCG.chkFOIR(row.Cells[19].Text, vTotalAmt);
                            if (vPassYN == "N" && ddlOperation.SelectedValue == "A")
                            {
                                gblFuction.AjxMsgPopup("FOIR Breached!");
                                checkbox.Checked = false;
                                return;
                            }
                            if (vAssetType != "Q")
                            {
                                txtSelfIncome.Enabled = false;
                                txtFamilyIncome.Enabled = false;
                            }

                            dt.Rows[row.RowIndex]["CashApproveYN"] = "N";
                            dt.Rows[row.RowIndex]["NEFTApproveYN"] = "Y";
                            dt.Rows[row.RowIndex]["HOApproveYN"] = "N";
                            chkCash.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (row.Cells[19].Text == "Y")
                {
                    gblFuction.AjxMsgPopup("This application is already approved by HO");
                    checkbox.Checked = true;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["NEFTApproveYN"] = "N";
                    chkCash.Enabled = true;
                    if (vAssetType != "Q")
                    {
                        txtSelfIncome.Enabled = true;
                        txtFamilyIncome.Enabled = true;
                    }
                }
            }
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void cbSendBack_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)((CheckBox)sender).NamingContainer;
            CheckBox cbSendBack = (CheckBox)gvRow.FindControl("cbSendBack");
            string LoanAppID = Convert.ToString(gvRow.Cells[19].Text);
            CMember cMem = new CMember();
            string vDoneYN = "";
            if (cbSendBack.Checked == true)
            {
                vDoneYN = cMem.ChkDiscrepancy(LoanAppID, Session[gblValue.BrnchCode].ToString());
                if (vDoneYN == "N")
                {
                    gblFuction.AjxMsgPopup("Do the Branch Level Discrepancy before Sendback .");
                    cbSendBack.Checked = false;
                }
                else
                {
                    cbSendBack.Checked = true;
                }
            }
        }

        protected void chkNATCAT_CheckChanged(object sender, EventArgs e)
        {
            // string vMsg = "";
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CApplication oCG = null;
            dt = (DataTable)ViewState["Sanc"];
            oCG = new CApplication();
            if (checkbox.Checked == true)
            {
                //vMsg = oCG.ChkNeftCash(row.Cells[12].Text, Session[gblValue.BrnchCode].ToString());
                // vMsg = oCG.ChkNEFTData(row.Cells[19].Text, Session[gblValue.BrnchCode].ToString());
                if (row.Cells[20].Text != "N")
                {
                    gblFuction.AjxMsgPopup("This application is already disbursed");
                    if (checkbox.Checked == true)
                    {
                        checkbox.Checked = false;
                    }
                    else
                    {
                        checkbox.Checked = true;
                    }
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["NatCatIncYN"] = "Y";

                }
            }
            else
            {
                if (row.Cells[20].Text == "Y")
                {
                    gblFuction.AjxMsgPopup("This application is already approved by HO");
                    checkbox.Checked = true;
                    return;
                }
                else
                {
                    dt.Rows[row.RowIndex]["NatCatIncYN"] = "N";

                }
            }
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void chkMedi_CheckChanged(object sender, EventArgs e)
        {

            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;

            dt = (DataTable)ViewState["Sanc"];

            if (checkbox.Checked == true)
            {
                dt.Rows[row.RowIndex]["MediclaimYN"] = "Y";
            }
            else
            {
                dt.Rows[row.RowIndex]["MediclaimYN"] = "N";
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
        //protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
        //    string vM_AddProfId, vM_AddProfNo, vM_IdentyPRofId, vM_IdentyProfNo;

        //    vM_AddProfId = gvRow.Cells[24].Text;
        //    vM_AddProfNo = gvRow.Cells[25].Text;
        //    vM_IdentyPRofId = gvRow.Cells[26].Text;
        //    vM_IdentyProfNo = gvRow.Cells[27].Text;

        //    vM_AddProfId = "2";
        //    vM_AddProfNo = "SCG3252855";
        //    vM_IdentyPRofId = "1";
        //    vM_IdentyProfNo = "878391650941";

        //    Label txtMem = (Label)gvRow.FindControl("txtMem");
        //    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

        //    if (e.CommandName == "cmdKYCvalidation")
        //    {
        //        try
        //        {
        //            if (vM_AddProfId == "1" || vM_IdentyPRofId == "1") // If Any one is AADHAR Then AADHAR Validation
        //            {
        //                KarzaAADHARKYCValidation(btnShow.Text, txtMem.Text, vM_AddProfId, vM_AddProfNo, vM_IdentyPRofId, vM_IdentyProfNo);
        //            }
        //            if (vValidationStatus == "Not Match" || vM_AddProfId == "2" || vM_IdentyPRofId == "2") // If Any one is Voter ID Then Voter ID Validation
        //            {
        //                KarzaVoterIDKYCValidation(btnShow.Text, txtMem.Text, vM_AddProfId, vM_AddProfNo, vM_IdentyPRofId, vM_IdentyProfNo);
        //            }
        //        }
        //        finally
        //        {
        //        }
        //    }
        //}

        //public static string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("No network adapters with an IPv4 address in the system!");
        //}

        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            DateTime vEndDate = DateTime.Now;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkCash = (CheckBox)e.Row.FindControl("chkCash");
                    CheckBox chkNEFT = (CheckBox)e.Row.FindControl("chkNEFT");
                    // CheckBox chkMedi = (CheckBox)e.Row.FindControl("chkMedi");
                    TextBox txtExpDisbDate = (TextBox)e.Row.FindControl("txtExpDisbDate");
                    DateTime vCBDate = gblFuction.setDate(e.Row.Cells[27].Text);
                    DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
                    AjaxControlToolkit.CalendarExtender ceExpDisbDate = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("ceExpDisbDate");
                    ceExpDisbDate.StartDate = gblFuction.setDate(e.Row.Cells[1].Text);
                    vEndDate = gblFuction.setDate(e.Row.Cells[1].Text);
                    ceExpDisbDate.EndDate = vEndDate.AddDays(10);
                    string vAssetType = Convert.ToString(e.Row.Cells[36].Text);
                    TextBox txtSelfIncome = (TextBox)e.Row.FindControl("txtSelfIncome");
                    TextBox txtFamilyIncome = (TextBox)e.Row.FindControl("txtFamilyIncome");
                    ImageButton ImgBtnView = (ImageButton)e.Row.FindControl("ImgBtnView");
                    ImageButton ImgBtnVerify = (ImageButton)e.Row.FindControl("ImgBtnVerify");
                    TextBox txtAccNo = (TextBox)e.Row.FindControl("txtAccNo");
                    TextBox txtIfsc = (TextBox)e.Row.FindControl("txtIfsc");
                    CheckBox chkNATCAT = (CheckBox)e.Row.FindControl("chkNATCAT");
                    chkNATCAT.Checked = Convert.ToString(e.Row.Cells[35].Text) == "Y" ? true : false;
                    string vPaySchedule = Convert.ToString(e.Row.Cells[43].Text);
                    string vLoanAppId = Convert.ToString(e.Row.Cells[19].Text);
                    chkNATCAT.Visible = Convert.ToString(e.Row.Cells[46].Text) == "N" ? false : true;
                    if (Convert.ToString(e.Row.Cells[40].Text) == "Y")
                    {
                        ImgBtnView.ImageUrl = "https://unity.bijliftt.com/Images/Verify.png";
                        ImgBtnVerify.Enabled = false;
                        txtAccNo.Enabled = false;
                        txtIfsc.Enabled = false;
                    }
                    else
                    {
                        ImgBtnView.ImageUrl = "https://unity.bijliftt.com/Images/Pending.png";
                        ImgBtnVerify.Enabled = true;
                        txtAccNo.Enabled = true;
                        txtIfsc.Enabled = true;
                    }
                    //---------------------------------------------------------------------------
                    DropDownList ddlHospiCash = (DropDownList)e.Row.FindControl("ddlHospiCash");
                    ddlHospiCash.Items.Clear();
                    oGb = new CGblIdGenerator();
                    //dt = oGb.PopComboMIS("N", "N", "AA", "HospiId", "HospiName", "HospiMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    dt = oGb.popHospiCash(vSanDt, Convert.ToString(Session[gblValue.BrnchCode]), Convert.ToInt32(e.Row.Cells[28].Text.Trim()));
                    if (dt.Rows.Count > 0)
                    {
                        ddlHospiCash.DataSource = dt;
                        ddlHospiCash.DataTextField = "HospiName";
                        ddlHospiCash.DataValueField = "HospiId";
                        ddlHospiCash.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlHospiCash.Items.Insert(0, oLc);
                    ddlHospiCash.SelectedIndex = ddlHospiCash.Items.IndexOf(ddlHospiCash.Items.FindByValue(e.Row.Cells[28].Text.Trim()));
                    //---------------------------------------------------------------------------

                    if (e.Row.Cells[16].Text == "Y")
                    {
                        chkCash.Checked = true;
                        chkNEFT.Enabled = false;
                    }
                    else if (e.Row.Cells[16].Text == "N")
                    {
                        chkCash.Checked = false;
                        chkNEFT.Enabled = true;
                    }

                    //Bind Calcel

                    if (e.Row.Cells[18].Text == "Y")
                    {
                        chkNEFT.Checked = true;
                        chkCash.Enabled = false;
                    }
                    else if (e.Row.Cells[18].Text == "N")
                    {
                        chkNEFT.Checked = false;
                        chkCash.Enabled = true;
                    }

                    if (e.Row.Cells[18].Text == "Y")
                    {
                        txtExpDisbDate.Enabled = false;
                    }

                    if (e.Row.Cells[26].Text == "Y")
                    {
                        // chkMedi.Checked = true;
                    }
                    else if (e.Row.Cells[26].Text == "N")
                    {
                        //chkMedi.Checked = false;
                    }

                    if (vAssetType == "Q")
                    {
                        txtSelfIncome.Enabled = false;
                        txtFamilyIncome.Enabled = false;
                    }
                    else
                    {
                        txtSelfIncome.Enabled = true;
                        txtFamilyIncome.Enabled = true;
                    }

                    if (rdbOpt.SelectedValue == "N")
                    {
                        double vTotDays = (vSanDt - vCBDate).TotalDays;
                        if (vTotDays > 15)
                        {
                            e.Row.BackColor = System.Drawing.Color.PeachPuff;
                        }
                        string vDoneYN = Convert.ToString(e.Row.Cells[45].Text);
                        if (vDoneYN == "Y")
                        {
                            e.Row.BackColor = System.Drawing.Color.Thistle;
                        }
                    }

                    if (vPaySchedule != "B")
                    {
                        gvSanc.Columns[44].Visible = false;
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popLO()
        {
            //DataTable dt = null;
            //CGblIdGenerator oGb = null;
            //string vBrCode = "";
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //try
            //{
            //    vBrCode = (string)Session[gblValue.BrnchCode];
            //    oGb = new CGblIdGenerator();
            //    dt = oGb.PopTransferMIS("Y", "MarketMst", "", vLogDt, vBrCode);
            //    ddlLO.DataSource = dt;
            //    ddlLO.DataTextField = "Market";
            //    ddlLO.DataValueField = "MarketID";
            //    ddlLO.DataBind();
            //    ListItem oli = new ListItem("<--Select-->", "-1");
            //    ddlLO.Items.Insert(0, oli);
            //}
            //finally
            //{
            //    oGb = null;
            //    dt = null;
            //}
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlLO.DataSource = dt;
                ddlLO.DataTextField = "EoName";
                ddlLO.DataValueField = "EoId";
                ddlLO.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLO.Items.Insert(0, oli);

            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void popGroup()
        {
            //ddlGroup.Items.Clear();
            //DataTable dt = null;
            //CUser oCEO = null;
            //DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            //oCEO = new CUser();
            //dt = oCEO.popGroupByEoid(ddlLO.SelectedValue, vLogDt);
            //if (dt.Rows.Count > 0)
            //{
            //    ddlGroup.DataSource = dt;
            //    ddlGroup.DataTextField = "GroupName";
            //    ddlGroup.DataValueField = "Groupid";
            //    ddlGroup.DataBind();
            //}
            //ListItem Li = new ListItem("<-- Select -->", "-1");
            //ddlGroup.Items.Insert(0, Li);
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "Groupid", "GroupName", "GroupMSt", ddlCentr.SelectedValue, "MarketID", "Tra_DropDate", vLogDt, vBrCode);
                ddlGroup.DataSource = dt;
                ddlGroup.DataTextField = "GroupName";
                ddlGroup.DataValueField = "Groupid";
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

        protected void ddlLO_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCenter();
        }

        private void PopCenter()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("S", "N", "AA", "MarketID", "Market", "MarketMSt", ddlLO.SelectedValue, "EoId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                //if (dt.Rows.Count > 0)
                //{
                ddlCentr.DataSource = dt;
                ddlCentr.DataTextField = "Market";
                ddlCentr.DataValueField = "MarketID";
                ddlCentr.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCentr.Items.Insert(0, oli);
                //}

            }
            finally
            { }
        }

        protected void ddlCentr_SelectedIndexChanged(object sender, EventArgs e)
        {
            popGroup();
        }

        protected void ddlHospiCash_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            DropDownList Dropdown = (DropDownList)sender;
            GridViewRow row = (GridViewRow)Dropdown.NamingContainer;

            dt = (DataTable)ViewState["Sanc"];

            if (Dropdown.SelectedValue != "-1")
            {
                dt.Rows[row.RowIndex]["MediclaimYN"] = "Y";
                dt.Rows[row.RowIndex]["HospiId"] = Dropdown.SelectedValue;
            }
            else
            {
                dt.Rows[row.RowIndex]["MediclaimYN"] = "N";
                dt.Rows[row.RowIndex]["HospiId"] = Dropdown.SelectedValue;
            }
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    if (rdbOpt.SelectedValue == "N")
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "T")
                        LoadGrid("T", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    else if (rdbOpt.SelectedValue == "C")
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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
                CheckBox chkNEFT = (CheckBox)gr.FindControl("chkNEFT");
                DataRow dr = dt.NewRow();
                dr["SlNo"] = i;
                dr["LoanAppId"] = gr.Cells[19].Text;
                dr["LoanAmt"] = gr.Cells[9].Text;
                dr["DisbYN"] = chkNEFT.Checked == true ? "Y" : "N";
                dr["GroupId"] = gr.Cells[22].Text;
                dr["GroupName"] = gr.Cells[3].Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                i = i + 1;
            }
            return dt;
        }

        #region KARZA Related Class
        public class clientData
        {
            public string caseId { get; set; }
        }


        public class KYCConsentOTPParameter
        {
            public string consent { get; set; }
            public string aadhaarNo { get; set; }
            public string accessKey { get; set; }
            public clientData clientData { get; set; }

        }
        public class KYCConsentParameter
        {
            public string lattitude { get; set; }
            public string longitude { get; set; }
            public string ipAddress { get; set; }
            public string userAgent { get; set; }
            public string deviceId { get; set; }
            public string deviceInfo { get; set; }
            public string consent { get; set; }
            public string name { get; set; }
            public string consentTime { get; set; }
            public string consentText { get; set; }
            public clientData clientData { get; set; }
        }
        public class result
        {
            public string accessKeyValidity { get; set; }
            public string message { get; set; }
            public string accessKey { get; set; }

        }
        public class KYCConsentResponse
        {
            public result result { get; set; }
            public string statusCode { get; set; }
            public string requestId { get; set; }
            public clientData clientData { get; set; }
        }
        #endregion

        //#region KARZA API call

        //private void KarzaAADHARKYCValidation(string vLoanAppNo, string vMemberName, string pM_AddProfId, string pM_AddProfNo, string pM_IdentyPRofId, string pM_IdentyProfNo)
        //{

        //    /*

        //     pM_AddProfId = 2 means VOTERID , 1 Means AADHAAR            pM_AddProfNo
        //     pM_IdentyPRofId = 2 means VOTERID , 1 Means AADHAAR             pM_IdentyProfNo

        //     */
        //    TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
        //    int secondsSinceEpoch = (int)t.TotalSeconds;
        //    string vIPAddress = GetLocalIPAddress();

        //    var req = new KYCConsentParameter()
        //    {
        //        lattitude = "",
        //        longitude = "",
        //        ipAddress = vIPAddress,
        //        userAgent = Request.Headers["User-Agent"].ToString(),
        //        deviceId = "",
        //        deviceInfo = "",
        //        consent = "Y",
        //        name = vMemberName,
        //        consentTime = Convert.ToString(secondsSinceEpoch),          //epoch in C#
        //        consentText = "I hereby give my consent",
        //        clientData =
        //                new clientData
        //                {
        //                    caseId = vLoanAppNo
        //                }
        //    };
        //    string Requestdata = JsonConvert.SerializeObject(req);
        //    string postURL = "https://testapi.karza.in/v3/aadhaar-consent";
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        if (request == null)
        //        {
        //            throw new NullReferenceException("request is not a http request");
        //        }

        //        // Set up the request properties.
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        //request.CookieContainer = new CookieContainer();

        //        //request.Headers.Add("cache-control", "no-cache");
        //        request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
        //        request.Host = "testapi.karza.in";

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

        //        string responsedata = string.Empty;

        //        byte[] data = Encoding.UTF8.GetBytes(Requestdata);
        //        request.ContentLength = data.Length;
        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(data, 0, data.Length);
        //        requestStream.Close();
        //        var httpResponse = (HttpWebResponse)request.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
        //        {
        //            var API_Response = streamReader.ReadToEnd(); ;
        //            responsedata = API_Response.ToString().Trim();
        //        }

        //        KYCConsentResponse vResponseObj = new KYCConsentResponse();
        //        vResponseObj = JsonConvert.DeserializeObject<KYCConsentResponse>(responsedata);
        //        string vAPIOTPAccessKey = vResponseObj.result.accessKey;
        //        if (vResponseObj.statusCode == "101")
        //        {
        //            string vAADHAAR = "";
        //            if (pM_IdentyPRofId == "1")
        //            {
        //                vAADHAAR = pM_IdentyProfNo;
        //            }
        //            else if (pM_AddProfId == "1")
        //            {
        //                vAADHAAR = pM_AddProfNo;
        //            }
        //            KarzaKYCConsentOTP(vLoanAppNo, vAADHAAR, vAPIOTPAccessKey);
        //        }
        //        else
        //        {
        //            vValidationStatus = "Not Match";
        //            gblFuction.AjxMsgPopup("Karza returns ..." + vResponseObj.statusCode + ". ......" + vResponseObj.result.message);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // streamWriter = null;
        //    }

        //}

        //private void KarzaKYCConsentOTP(string pLoanAppNo, string pAADHAAR, string pAPIOTPAccessKey)
        //{
        //    var req = new KYCConsentOTPParameter()
        //    {
        //        consent = "Y",
        //        aadhaarNo = pAADHAAR,
        //        accessKey = pAPIOTPAccessKey,
        //        clientData =
        //                new clientData
        //                {
        //                    caseId = pLoanAppNo
        //                }
        //    };
        //    string Requestdata = JsonConvert.SerializeObject(req);
        //    string postURL = "https://testapi.karza.in/v3/get-aadhaar-otp";
        //    try
        //    {
        //        HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
        //        if (request == null)
        //        {
        //            throw new NullReferenceException("request is not a http request");
        //        }

        //        // Set up the request properties.
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        //request.CookieContainer = new CookieContainer();

        //        //request.Headers.Add("cache-control", "no-cache");
        //        request.Headers.Add("x-karza-key", "1ky3RiYtz54WQdGe");
        //        request.Host = "testapi.karza.in";

        //        ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

        //        string responsedata = string.Empty;

        //        byte[] data = Encoding.UTF8.GetBytes(Requestdata);
        //        request.ContentLength = data.Length;
        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(data, 0, data.Length);
        //        requestStream.Close();
        //        var httpResponse = (HttpWebResponse)request.GetResponse();
        //        using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
        //        {
        //            var API_Response = streamReader.ReadToEnd(); ;
        //            responsedata = API_Response.ToString().Trim();
        //        }

        //        KYCConsentResponse vResponseObj = new KYCConsentResponse();
        //        vResponseObj = JsonConvert.DeserializeObject<KYCConsentResponse>(responsedata);
        //        string vAPIOTPAccessKey = vResponseObj.result.accessKey;
        //        if (vResponseObj.statusCode == "101")
        //        {
        //            //KarzaKYC Next Step
        //        }
        //        else
        //        {
        //            vValidationStatus = "Not Match";
        //            gblFuction.AjxMsgPopup("Karza returns ..." + vResponseObj.statusCode + "... That means..." + vResponseObj.result.message);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        // streamWriter = null;
        //    }
        //}

        //#endregion

        #region Bank Account Verification
        protected void ImgBtnVerify_Click(object sender, EventArgs e)
        {
            ImageButton ImgBtnVerify = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImgBtnVerify.NamingContainer;
            string vMemberID = gR.Cells[41].Text;
            string vCGTId = gR.Cells[42].Text;
            TextBox txtAccNo = (TextBox)gR.FindControl("txtAccNo");
            TextBox txtIfsc = (TextBox)gR.FindControl("txtIfsc");
            TextBox txtBenName = (TextBox)gR.FindControl("txtBenName");
            ImageButton ImgBtnView = (ImageButton)gR.FindControl("ImgBtnView");
            CApplication oApp = new CApplication();

            if (txtAccNo.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Bank Account can not be left blank.");
                return;
            }
            if (txtIfsc.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("IFSC Code can not be left blank.");
                return;
            }
            int vErr = oApp.ChkAccNo(vMemberID, txtAccNo.Text.Trim(), txtIfsc.Text.Trim());
            if (vErr == 9999)
            {
                gblFuction.AjxMsgPopup("Invalid IFSC Code.");
                return;
            }
            else if (vErr > 0)
            {
                gblFuction.AjxMsgPopup("Account no already exist with another member.");
                return;
            }

            string vResponse = "";
            FingPayRequest fpr = new FingPayRequest();
            fpr.beneAccNo = txtAccNo.Text;
            fpr.beneIFSC = txtIfsc.Text.ToUpper();
            fpr.CGTId = vCGTId;
            fpr.MemberId = vMemberID;
            fpr.CreatedBy = Session[gblValue.UserId].ToString();
            string vRequestData = JsonConvert.SerializeObject(fpr);
            try
            {
                string postURL = "https://centrummob.bijliftt.com/CentrumService.svc/BankAcVerify";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("cache-control", "no-cache");
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(vRequestData);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                vResponse = responseReader.ReadToEnd();
                request.GetResponse().Close();
                dynamic res = JsonConvert.DeserializeObject(vResponse);
                bool vStatus = res.Status;
                string vStatusMessage = Convert.ToString(res.StatusMessage);
                if (vStatus == true && res.StatusCode == 0)
                {
                    ImgBtnView.ImageUrl = "https://unity.bijliftt.com/Images/Verify.png";
                    txtBenName.Text = Convert.ToString(res.BeneName);
                    gR.Cells[40].Text = "Y";
                    txtAccNo.Enabled = false;
                    txtIfsc.Enabled = false;
                    ImgBtnVerify.Enabled = false;
                }
                gblFuction.AjxMsgPopup(vStatusMessage);
            }
            catch (WebException we)
            {
                using (var stream = we.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    vResponse = reader.ReadToEnd();
                }
            }

            gblFuction.AjxMsgPopup(vResponse);
        }
        #endregion

        protected void txtExpDisbDate_TextChanged(object sender, EventArgs e)
        {

            DataTable dt = null;
            CApplication oLS = null;

            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtExpDisbDate = (TextBox)gvRow.FindControl("txtExpDisbDate");
            string vAppId = Convert.ToString(gvRow.Cells[19].Text);
            string vPaySchedule = Convert.ToString(gvRow.Cells[43].Text);
            TextBox txtCollStDate = (TextBox)gvRow.FindControl("txtCollStDate");

            if (vPaySchedule == "B")
            {
                oLS = new CApplication();
                dt = oLS.GetBiWeeklyCollStDt(vAppId, gblFuction.setDate(txtExpDisbDate.Text));
                if (dt.Rows.Count > 0)
                {
                    txtCollStDate.Text = Convert.ToString(dt.Rows[0]["CollStartDt"]).Trim();
                }

            }


        }
    }
}