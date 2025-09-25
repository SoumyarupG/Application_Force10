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
using Newtonsoft.Json;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HONEFTDisbursement : CENTRUMBase
    {
        string vMobService = ConfigurationManager.AppSettings["MobService"];
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Enabled = false;
                PopDisbBank();
                PopBranch(Session[gblValue.UserName].ToString());
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
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

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "HO NEFT Disbursement";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHONEFTTransfer);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO NEFT disbursement Approve", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

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
                gvDtl.DataSource = dt;
                gvDtl.DataBind();
                //if (dt.Rows.Count > 0)
                //{
                //    txtTotNEFT.Text = dt.Rows[0]["NEFTFund"].ToString();
                //}
            }
            finally
            {
                dt = null;
                oNeft = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        private Boolean SaveRecords(string Mode)
        {
            DataTable dt = null;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0;
            string vXml = "";
            double TotDisbAmt = 0;
            string vDescIDBank = ddlBank.SelectedValue.ToString();
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                DropDownList ddlBrBank = (DropDownList)gr.FindControl("ddlBrBank");
                if (chkDisb.Checked == true)
                {
                    if (ddlBrBank.SelectedIndex == 0)
                    {
                        gblFuction.MsgPopup("Please select a Bank..!!");
                        return false;
                    }
                }
            }


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

            CNEFTTransfer oNEFT = null;
            //if (ddlBank.SelectedIndex == 0)
            //{
            //    gblFuction.AjxMsgPopup("Select Bank");
            //    return false;
            //}
            oNEFT = new CNEFTTransfer();
            int err = 0;
            err = oNEFT.chkGroupAndLoanAmt(vXml, ref vMsg);
            if (err == 1)
            {
                gblFuction.AjxMsgPopup(vMsg.ToString());
                return false;
            }
            //------------------------------
            CApplication oApp = null;
            oApp = new CApplication();
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
            ds = oApp.ChkActiveMemberinaGroup(gblFuction.setDate(txtDt.Text), vXml, "F");
            DtGrp = ds.Tables[0];
            //dtLoanScheme = ds.Tables[1];
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
                return false;
            }
            //if (dtLoanScheme.Rows.Count > 0)
            //{
            //    for (int j = 0; j <= dtLoanScheme.Rows.Count - 1; j++)
            //    {
            //        vMesg = vMesg + "Against Group " + dtLoanScheme.Rows[j]["GroupName"].ToString() + " Loan Scheme Should be Same. ";
            //    }
            //    gblFuction.MsgPopup(vMesg);
            //    return false;
            //}
            //if (dtApplAmt.Rows.Count > 0)
            //{
            //    for (int j = 0; j <= dtApplAmt.Rows.Count - 1; j++)
            //    {
            //        vMesg = vMesg + "Against Group " + dtApplAmt.Rows[j]["GroupName"].ToString() + " Application Amount Should be Same. ";
            //    }
            //    gblFuction.MsgPopup(vMesg);
            //    return false;
            //}
            if (dtInstNo.Rows.Count > 0)
            {
                for (int j = 0; j <= dtInstNo.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against Group " + dtInstNo.Rows[j]["GroupName"].ToString() + " Loan Tenure Should be Same. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }
            ////-------------CENTR - 3000-------------------
            if (dtWeeklyCenterMemCnt.Rows.Count > 0)
            {
                for (int j = 0; j <= dtWeeklyCenterMemCnt.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against Center " + dtWeeklyCenterMemCnt.Rows[j]["Market"].ToString() + " Must be Disburse With 5 Customer. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }

            //if (dtMonthlyMemCnt.Rows.Count > 0)
            //{
            //    for (int j = 0; j <= dtMonthlyMemCnt.Rows.Count - 1; j++)
            //    {
            //        vMesg = vMesg + "Against Group " + dtMonthlyMemCnt.Rows[j]["GroupName"].ToString() + " Must be Disburse With 2 Customer. ";
            //    }
            //    gblFuction.MsgPopup(vMesg);
            //    return false;
            //}
            if (dtActMemInACenter.Rows.Count > 0)
            {
                for (int j = 0; j <= dtActMemInACenter.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against center " + dtActMemInACenter.Rows[j]["MarketName"].ToString() + " maximum 36 customer can be disburse. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }

            if (dtActMemInACenterLimit.Rows.Count > 0)
            {
                for (int j = 0; j <= dtActMemInACenterLimit.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against center " + dtActMemInACenterLimit.Rows[j]["MarketName"].ToString() + " mininum 1 and maximum 36 customer can be have active loan. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }

            if (dtMonthlyGrActMemChk.Rows.Count > 0)
            {
                for (int j = 0; j <= dtMonthlyGrActMemChk.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against Monthly Group " + dtMonthlyGrActMemChk.Rows[j]["GroupNameM"].ToString() + " No of Active member should not be more than 12. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }

            if (dtWeeklyGrActMemChk.Rows.Count > 0)
            {
                for (int j = 0; j <= dtWeeklyGrActMemChk.Rows.Count - 1; j++)
                {
                    vMesg = vMesg + "Against Weekly Group " + dtWeeklyGrActMemChk.Rows[j]["GroupNameW"].ToString() + " No of Active member should not be more than 5. ";
                }
                gblFuction.MsgPopup(vMesg);
                return false;
            }
            ////---------------End CENTR - 1780--------------------

            //------------------------------------
            try
            {
                if (Mode == "Save")
                {
                    //vClosingBal = oNEFT.GetClosingBalBranchWise(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtDt.Text), vDescIDBank, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
                    //if (vClosingBal < TotDisbAmt)
                    //{
                    //    gblFuction.AjxMsgPopup("Insufficient Balance");
                    //    return false;
                    //}
                    vErr = oNEFT.InsertNEFTTransfer_Checkforbackend(vXml, vDescIDBank, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(txtDt.Text), "I", ref vMsg);
                    if (vErr == 0)
                    {                       
                        var req = new HoDisbursement()
                        {
                            pXml = vXml,
                            pDescId=vDescIDBank,
                            pEntType="I",
                            pLoanDt=txtDt.Text,
                            pCreatedby = Convert.ToString(Session[gblValue.UserId])                         
                        };
                        string Requestdata = JsonConvert.SerializeObject(req);
                        CallService("InsertNEFTTransfer", Requestdata, vMobService);
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(vMsg.ToString());
                        vResult = false;
                    }

                    #region InsertNEFTTransfer
                    //    vErr = oNEFT.InsertNEFTTransfer(vXml, vDescIDBank, Convert.ToInt32(Session[gblValue.UserId]), gblFuction.setDate(txtDt.Text), "I", ref vMsg);
                    //    if (vErr == 2)
                    //    {
                    //        gblFuction.AjxMsgPopup(vMsg.ToString());
                    //        vResult = false;
                    //    }
                    //    else
                    //    {
                    //        if (vErr == 0)
                    //        {
                    //            gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    //            vResult = true;
                    //        }
                    //        else
                    //        {
                    //            gblFuction.AjxMsgPopup(gblMarg.DBError);
                    //            vResult = false;
                    //        }
                    //    }
                    //}
                    //else if (Mode == "Edit")
                    //{
                    //    //if (ValidateFields() == false)
                    //    //    return false;

                    //    //oReSchedule = new CReScheduling();
                    //    //dt = (DataTable)ViewState["ReSchedule"];
                    //    //dt = (DataTable)ViewState["ReSchedule"];
                    //    ////vXmlSch = DataTableTOXml(dt);

                    //    //vErr = oReSchedule.UpdateReScheduleMst(vRescheduleId, vLoanID, vReSchDate, vFInstNo, vAcDueDate, vNoofdays, vCurrDate, vHappDate,
                    //    //            vReason, txtRemark.Text, Convert.ToInt32(ddlResch.SelectedValue), Convert.ToInt32(ddlAprov.SelectedValue),
                    //    //            vBrCode, this.UserID, "E");
                    //    //if (vErr == 0)
                    //    //{
                    //    //    gblFuction.AjxMsgPopup(gblMarg.SaveMsg);
                    //    //    vResult = true;
                    //    //}
                    //    //else
                    //    //{
                    //    //    gblFuction.AjxMsgPopup(gblMarg.DBError);
                    //    //    vResult = false;
                    //    //}
                    //}
                    #endregion
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            return vResult;
        }

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

        private void ClearControls()
        {
            //txtTotNEFT.Text = "0";
            //LoadGrid(0);
        }

        private void EnableControl(bool Status)
        {

        }

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
            CVoucher oVoucher = null;
            DataTable dtBrBank = null;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlBrBank = (DropDownList)e.Row.FindControl("ddlBrBank");
                try
                {
                    oVoucher = new CVoucher();
                    ddlBrBank.Items.Clear();
                    dtBrBank = oVoucher.GetAcGenLedCB(e.Row.Cells[20].Text.Trim(), "D", "");
                    if (dtBrBank.Rows.Count > 0)
                    {
                        ddlBrBank.DataSource = dtBrBank;
                        ddlBrBank.DataTextField = "Desc";
                        ddlBrBank.DataValueField = "DescId";
                        ddlBrBank.DataBind();
                    }
                    ListItem oLc = new ListItem("<--Select-->", "-1");
                    ddlBrBank.Items.Insert(0, oLc);
                    ddlBrBank.SelectedIndex = ddlBrBank.Items.IndexOf(ddlBrBank.Items.FindByValue(e.Row.Cells[22].Text.Trim()));

                }
                finally
                {
                    oVoucher = null;
                    dtBrBank = null;
                }
            }
        }

        private String XmlFromGrid()
        {
            Int32 i = 0;
            String vXML = "";
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SlNo");
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("Loantype");
            dt.Columns.Add("LoanAmt");
            dt.Columns.Add("BranchName");
            dt.Columns.Add("EoName");
            dt.Columns.Add("DisbYN");
            dt.Columns.Add("ICID");
            dt.Columns.Add("BankDescId");
            dt.Columns.Add("GroupId");
            dt.Columns.Add("GroupName");
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
                if (chkDisb.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanAppId"] = gr.Cells[15].Text;
                    Label lblLnShceme = (Label)gr.FindControl("lblLnShceme");
                    dr["Loantype"] = lblLnShceme.Text;
                    Label lblBranch = (Label)gr.FindControl("lblBranch");
                    dr["BranchName"] = lblBranch.Text;
                    Label lblRO = (Label)gr.FindControl("lblRO");
                    dr["EoName"] = lblRO.Text;
                    Label lblLnAmt = (Label)gr.FindControl("lblLnAmt");
                    dr["LoanAmt"] = lblLnAmt.Text;
                    if (chkDisb.Checked == true)
                        dr["DisbYN"] = 'Y';
                    else
                        dr["DisbYN"] = 'N';
                    dr["ICID"] = gr.Cells[18].Text;
                    DropDownList ddlBrBank = (DropDownList)gr.FindControl("ddlBrBank");
                    dr["BankDescId"] = ddlBrBank.SelectedValue;
                    dr["GroupId"] = gr.Cells[21].Text;
                    Label lblGroup = (Label)gr.FindControl("lblGroup");
                    dr["GroupName"] = lblGroup.Text;
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

        private void PopDisbBank()
        {
            DataTable dt = null;
            CNEFTTransfer oNeft = null;
            oNeft = new CNEFTTransfer();
            dt = oNeft.PopDisbBank();
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
                if (chkDisb.Checked == true)
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
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>from our Current Account No:  " + dt2.Rows[0]["Phone"].ToString() + " to the below mentioned accounts of Unity SFB.</font></td></tr>");
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
            //CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
            //CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
                Label lblLnAmt = (Label)gR.FindControl("lblLnAmt");
                //CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
                //CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                //CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(lblLnAmt.Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                //if (chkAppFrCashDisbT.Checked == true)
                //{
                //    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vCashCnt = vCashCnt + 1;
                //}
                //if (ChkCancelT.Checked == true)
                //{
                //    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vCanCnt = vCanCnt + 1;
                //}
                //if (ChkPostponeT.Checked == true)
                //{
                //    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vPostCnt = vPostCnt + 1;
                //}
                //if (chkDisbT.Checked == true)
                //{
                //    chkAppFrCashDisbT.Checked = false;
                //    ChkCancelT.Checked = false;
                //    chkAppFrCashDisbT.Enabled = false;
                //    ChkCancelT.Enabled = false;
                //    ChkPostponeT.Enabled = false;
                //    ChkPostponeT.Checked = false;
                //}
                //else
                //{
                //    chkAppFrCashDisbT.Enabled = true;
                //    ChkCancelT.Enabled = true;
                //    ChkPostponeT.Enabled = true;
                //}

            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            //txtTotCash.Text = Convert.ToString(vTotCash);
            //txtCashCnt.Text = Convert.ToString(vCashCnt);
            //txtCanTot.Text = Convert.ToString(vTotCan);
            //txtCanCnt.Text = Convert.ToString(vCanCnt);
            //txtPostTot.Text = Convert.ToString(VTotPost);
            //txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            //UpCount.Update();



        }
        //protected void chkAppFrCashDisb_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox checkbox = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)checkbox.NamingContainer;
        //    CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
        //    CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
        //    CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
        //    double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
        //    Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
        //    foreach (GridViewRow gR in gvDtl.Rows)
        //    {
        //        CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
        //        CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
        //        CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
        //        CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
        //        TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
        //        TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
        //        TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
        //        if (chkDisbT.Checked == true)
        //        {
        //            vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vTotCount = vTotCount + 1;
        //        }
        //        if (chkAppFrCashDisbT.Checked == true)
        //        {
        //            vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCashCnt = vCashCnt + 1;
        //        }
        //        if (ChkCancelT.Checked == true)
        //        {
        //            vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCanCnt = vCanCnt + 1;
        //        }
        //        if (ChkPostponeT.Checked == true)
        //        {
        //            VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vPostCnt = vPostCnt + 1;
        //        }
        //        if (chkAppFrCashDisbT.Checked == true)
        //        {
        //            chkDisbT.Checked = false;
        //            ChkCancelT.Checked = false;
        //            chkDisbT.Enabled = false;
        //            ChkCancelT.Enabled = false;
        //            ChkPostponeT.Enabled = false;
        //            ChkPostponeT.Checked = false;
        //            txtCashRemark.Enabled = true;
        //            txtRemarks.Enabled = false;
        //            txtPostRemark.Enabled = false;
        //        }
        //        else
        //        {
        //            chkDisbT.Enabled = true;
        //            ChkCancelT.Enabled = true;
        //            ChkPostponeT.Enabled = true;
        //            txtCashRemark.Text = "";
        //            txtCashRemark.Enabled = false;
        //        }
        //    }
        //    //txtTotAmt.Text = Convert.ToString(vTotalAmt);
        //    //txtTotCount.Text = Convert.ToString(vTotCount);
        //    //txtTotCash.Text = Convert.ToString(vTotCash);
        //    //txtCashCnt.Text = Convert.ToString(vCashCnt);
        //    //txtCanTot.Text = Convert.ToString(vTotCan);
        //    //txtCanCnt.Text = Convert.ToString(vCanCnt);
        //    //txtPostTot.Text = Convert.ToString(VTotPost);
        //    //txtPostCnt.Text = Convert.ToString(vPostCnt);
        //    //UpTot.Update();
        //    //UpCount.Update();

        //}
        //protected void ChkCancel_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox checkbox = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)checkbox.NamingContainer;
        //    CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
        //    CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
        //    CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
        //    double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
        //    Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
        //    foreach (GridViewRow gR in gvDtl.Rows)
        //    {
        //        CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
        //        CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
        //        CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
        //        CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
        //        TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
        //        TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
        //        TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
        //        if (chkDisbT.Checked == true)
        //        {
        //            vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vTotCount = vTotCount + 1;
        //        }
        //        if (chkAppFrCashDisbT.Checked == true)
        //        {
        //            vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCashCnt = vCashCnt + 1;
        //        }
        //        if (ChkCancelT.Checked == true)
        //        {
        //            vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCanCnt = vCanCnt + 1;
        //        }
        //        if (ChkPostponeT.Checked == true)
        //        {
        //            VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vPostCnt = vPostCnt + 1;
        //        }
        //        if (ChkCancelT.Checked == true)
        //        {
        //            chkDisbT.Checked = false;
        //            chkAppFrCashDisbT.Checked = false;
        //            chkDisbT.Enabled = false;
        //            chkAppFrCashDisbT.Enabled = false;
        //            ChkPostponeT.Enabled = false;
        //            ChkPostponeT.Checked = false;
        //            txtRemarks.Enabled = true;
        //            txtPostRemark.Enabled = false;
        //            txtCashRemark.Enabled = false;
        //        }
        //        else
        //        {
        //            chkDisbT.Enabled = true;
        //            txtRemarks.Text = "";
        //            chkAppFrCashDisbT.Enabled = true;
        //            ChkPostponeT.Enabled = true;
        //            txtRemarks.Enabled = false;
        //        }
        //    }
        //    //txtTotAmt.Text = Convert.ToString(vTotalAmt);
        //    //txtTotCount.Text = Convert.ToString(vTotCount);
        //    //txtTotCash.Text = Convert.ToString(vTotCash);
        //    //txtCashCnt.Text = Convert.ToString(vCashCnt);
        //    //txtCanTot.Text = Convert.ToString(vTotCan);
        //    //txtCanCnt.Text = Convert.ToString(vCanCnt);
        //    //txtPostTot.Text = Convert.ToString(VTotPost);
        //    //txtPostCnt.Text = Convert.ToString(vPostCnt);
        //    //UpTot.Update();
        //    //UpCount.Update();

        //}

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

        //protected void ChkPostpone_CheckedChanged(object sender, EventArgs e)
        //{
        //    CheckBox checkbox = (CheckBox)sender;
        //    GridViewRow row = (GridViewRow)checkbox.NamingContainer;
        //    CheckBox chkDisb = (CheckBox)row.FindControl("chkDisb");
        //    CheckBox chkAppFrCashDisb = (CheckBox)row.FindControl("chkAppFrCashDisb");
        //    CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
        //    CheckBox ChkPostpone = (CheckBox)row.FindControl("ChkPostpone");
        //    double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
        //    Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
        //    foreach (GridViewRow gR in gvDtl.Rows)
        //    {
        //        CheckBox chkDisbT = (CheckBox)gR.FindControl("chkDisb");
        //        CheckBox chkAppFrCashDisbT = (CheckBox)gR.FindControl("chkAppFrCashDisb");
        //        CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
        //        CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
        //        TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
        //        TextBox txtCashRemark = (TextBox)gR.FindControl("txtCashRemark");
        //        TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
        //        if (chkDisbT.Checked == true)
        //        {
        //            vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vTotCount = vTotCount + 1;
        //        }
        //        if (chkAppFrCashDisbT.Checked == true)
        //        {
        //            vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCashCnt = vCashCnt + 1;
        //        }
        //        if (ChkCancelT.Checked == true)
        //        {
        //            vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vCanCnt = vCanCnt + 1;
        //        }
        //        if (ChkPostponeT.Checked == true)
        //        {
        //            VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
        //            vPostCnt = vPostCnt + 1;
        //        }
        //        if (ChkPostponeT.Checked == true)
        //        {
        //            chkAppFrCashDisbT.Checked = false;
        //            ChkCancelT.Checked = false;
        //            chkAppFrCashDisbT.Enabled = false;
        //            ChkCancelT.Enabled = false;
        //            chkDisbT.Checked = false;
        //            chkDisbT.Enabled = false;
        //            txtPostRemark.Enabled = true;
        //            txtCashRemark.Enabled = false;
        //            txtRemarks.Enabled = false;
        //        }
        //        else
        //        {
        //            chkAppFrCashDisbT.Enabled = true;
        //            ChkCancelT.Enabled = true;
        //            chkDisbT.Enabled = true;
        //            txtPostRemark.Text = "";
        //            txtPostRemark.Enabled = false;
        //        }
        //    }
        //    //txtTotAmt.Text = Convert.ToString(vTotalAmt);
        //    //txtTotCount.Text = Convert.ToString(vTotCount);
        //    //txtTotCash.Text = Convert.ToString(vTotCash);
        //    //txtCashCnt.Text = Convert.ToString(vCashCnt);
        //    //txtCanTot.Text = Convert.ToString(vTotCan);
        //    //txtCanCnt.Text = Convert.ToString(vCanCnt);
        //    //txtPostTot.Text = Convert.ToString(VTotPost);
        //    //txtPostCnt.Text = Convert.ToString(vPostCnt);
        //    //UpTot.Update();
        //    //UpCount.Update();

        //}

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
                //if (chkAppFrCashDisbT.Checked == true)
                //{
                //    vTotCash = vTotCash + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vCashCnt = vCashCnt + 1;
                //}
                //if (ChkCancelT.Checked == true)
                //{
                //    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vCanCnt = vCanCnt + 1;
                //}
                //if (ChkPostponeT.Checked == true)
                //{
                //    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                //    vPostCnt = vPostCnt + 1;
                //}
            }
            //txtTotAmt.Text = Convert.ToString(vTotalAmt);
            //txtTotCount.Text = Convert.ToString(vTotCount);
            //txtTotCash.Text = Convert.ToString(vTotCash);
            //txtCashCnt.Text = Convert.ToString(vCashCnt);
            //txtCanTot.Text = Convert.ToString(vTotCan);
            //txtCanCnt.Text = Convert.ToString(vCanCnt);
            //txtPostTot.Text = Convert.ToString(VTotPost);
            //txtPostCnt.Text = Convert.ToString(vPostCnt);
            //UpTot.Update();
            //UpCount.Update();

        }

        private void CallService(string pApiName, string pRequestdata, string pReportUrl)
        {
            string vMsg = "";
            CApiCalling oAPI = new CApiCalling();
            try
            {
                vMsg = oAPI.GenerateReport(pApiName, pRequestdata, pReportUrl);
            }
            finally
            {
                gblFuction.AjxMsgPopup("Success...HO NEFT Disbursement Job is running at Back Ground ...Please wait for 15/20 mins.");
            }
        }
    }

    public class HoDisbursement
    {
        public string pXml { get; set; }
        public string pDescId { get; set; }
        public string pCreatedby { get; set; }
        public string pLoanDt { get; set; }
        public string pEntType { get; set; }
    }
}