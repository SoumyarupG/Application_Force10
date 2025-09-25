using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class Accrued : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                txtAccruedDt.Text = (string)Session[gblValue.LoginDate];
                txtACPostDate.Text = (string)Session[gblValue.LoginDate];
                ViewState["StateEdit"] = null;
                PopBranch();
                LoadAcLed();
                // LoadBounceList();
                tabAccIntPost.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {

            try
            {
                this.Menu = false;
                this.PageHeading = "Accrued Posting";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAccrued);
                if (this.UserID == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = false;

                    return;
                }
                if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnSave.Visible = true;

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

                    return;
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Collection Bounce", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabAccIntPost.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void LoadAcLed()
        {
            DataTable dt = null;
            CVoucher oVoucher = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oVoucher = new CVoucher();
            // dt = oVoucher.GetAcGenLedCB(vBranch, "G", "");
            dt = oVoucher.GetAccruedLed();
            ddlLed.DataSource = dt;
            ddlLed.DataTextField = "Desc";
            ddlLed.DataValueField = "DescId";
            ddlLed.DataBind();
            ListItem liSel = new ListItem("<--- Select --->", "-1");
            ddlLed.Items.Insert(0, liSel);
        }
        private void LoadAccPosList(DateTime pDate,string pBranchCode)
        {
            DataTable dt = null;
            CLoanRecovery oCL = null;
            string vBranch = Session[gblValue.BrnchCode].ToString();
            oCL = new CLoanRecovery();
            dt = oCL.GetAccInstLisByDate(pDate, pBranchCode);
            if (dt.Rows.Count > 0)
            {
                gvACList.DataSource = dt;
                gvACList.DataBind();
            }
            else
            {
                gvACList.DataSource = null;
                gvACList.DataBind();
            }
        }
        //protected void txtACPostDate_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtACPostDate.Text != "")
        //    {
        //        DateTime vDate = gblFuction.setDate(txtACPostDate.Text);
        //        LoadAccPosList(vDate);
        //    }
        //}
        private void LoadAccruedDetail(DateTime pCurDate, DateTime pNextDate, string pBranchCode)
        {
            CLoanRecovery ORecovery = new CLoanRecovery();
            DataTable dt = new DataTable();
            try
            {
                dt = ORecovery.GetLoanIdForAccrued(pCurDate, pNextDate, pBranchCode);
                if (dt.Rows.Count > 0)
                {
                    gvAccInt.DataSource = dt;
                    gvAccInt.DataBind();
                }
                else
                {
                    gvAccInt.DataSource = null;
                    gvAccInt.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ORecovery = null;
                dt = null;
            }
        }
        private void PopBranch()
        {
            ddlBranch.Items.Clear();
            CMember oCM = new CMember();
            DataTable dt = new DataTable(); ;
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();

                dt = oCM.GetBranchByBrCode(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    if (vBrCode == "0000")
                    {
                        ListItem oItm = new ListItem("All", "A");
                        ddlBranch.Items.Insert(0, oItm);
                    }

                    ddlBr.DataTextField = "BranchName";
                    ddlBr.DataValueField = "BranchCode";
                    ddlBr.DataSource = dt;
                    ddlBr.DataBind();
                    if (vBrCode == "0000")
                    {
                        ListItem oItm = new ListItem("All", "A");
                        ddlBr.Items.Insert(0, oItm);
                    }
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
        protected void cbApprvSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk;
            string vLogDt = Session[gblValue.LoginDate].ToString();
            foreach (GridViewRow rowItem in gvAccInt.Rows)
            {
                chk = (CheckBox)(rowItem.Cells[6].FindControl("cbApprv"));
                chk.Checked = ((CheckBox)sender).Checked;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
            }
        }
        private void ClearControls()
        {
            gvAccInt.DataSource = null;
            gvAccInt.DataBind();
            ddlLed.SelectedIndex = -1;
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabAccIntPost.ActiveTabIndex = 0;
            StatusButton("View");
        }
        //private void GetCollDtl(string LoanNo, DateTime pBounceDate)
        //{
        //    CLoanRecovery ORecovery = new CLoanRecovery();
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = ORecovery.GetLastCollDtlByLoanNo(LoanNo, pBounceDate);
        //        if (dt.Rows.Count > 0)
        //        {
        //            //hdAccDate.Value = dt.Rows[0]["AccDate"].ToString();
        //            hdOutStand.Value = dt.Rows[0]["Outstanding"].ToString();
        //            txtPrinCollAmt.Text = dt.Rows[0]["PrinCollAmt"].ToString();
        //            txtIntCollAmt.Text = dt.Rows[0]["IntCollAmt"].ToString();
        //            txtPenAmt.Text = dt.Rows[0]["PenCollAmt"].ToString();
        //            txtWaveIntAmt.Text = dt.Rows[0]["IntWaveAmt"].ToString();
        //            txtTotCollAmt.Text = dt.Rows[0]["CollAmt"].ToString();
        //            hdBalAmt.Value = dt.Rows[0]["BalanceAmt"].ToString();
        //            txtCollMode.Text = dt.Rows[0]["CollectionMode"].ToString();
        //            txtChqRefNo.Text = dt.Rows[0]["ChequeReffNo"].ToString();
        //            hdCollMode.Value = dt.Rows[0]["CollMode"].ToString();
        //            hdTransMode.Value = dt.Rows[0]["TransMode"].ToString();
        //            hdDescId.Value = dt.Rows[0]["DescId"].ToString();
        //            hdBankName.Value = dt.Rows[0]["BankName"].ToString();

        //        }
        //        else
        //        {
        //            txtPrinCollAmt.Text = "0";
        //            txtIntCollAmt.Text = "0";
        //            txtPenAmt.Text = "0";
        //            txtWaveIntAmt.Text = "0";
        //            txtTotCollAmt.Text = "0";
        //            txtCollMode.Text = "";
        //            txtPrinCollAmt.Text = "0";
        //            txtChqRefNo.Text = "";
        //            hdCollMode.Value = "";
        //            hdTransMode.Value = "";
        //            hdDescId.Value = "";
        //            hdBankName.Value = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ORecovery = null;
        //        dt = null;
        //    }
        //}
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAccruedDt.Text == "")
            {
                gblFuction.MsgPopup("Date Can Not Be Empty");
                return;
            }
            if (ddlLed.SelectedValue == "-1")
            {
                gblFuction.MsgPopup("Please Select Ledger");
                return;
            }
            CLoanRecovery OLC = new CLoanRecovery();
            try
            {
                Boolean vResult = false;
                Int32 vErr = 0;
                string vBranch = Session[gblValue.BrnchCode].ToString();
                string vActMstTbl = Session[gblValue.ACVouMst].ToString();
                string vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
                string vFinYear = Session[gblValue.ShortYear].ToString();
                string vAccLed = ddlLed.SelectedValue.ToString();
                DateTime CurDate = gblFuction.setDate(txtAccruedDt.Text);
                DateTime NextDate = CurDate.AddDays(-CurDate.Day + 1).AddMonths(1);
                string vYrNo = Session[gblValue.FinYrNo].ToString();

                // If Accrued Posing done on 31 march of any Financial year(i.e End Date of any Financial Year)
                string StrDD, StrMM, StrYYYY, strDate;
                string pDate = txtAccruedDt.Text;
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                string vNewAcVouMst = "";
                string vNewAcVouDtl = "";
                string vNewShortYr = "";

                if (Convert.ToInt32(StrMM) == 3 && Convert.ToInt32(StrDD) == 31)
                {
                    Int32 vNewYrNo = Convert.ToInt32(vYrNo) + 1;
                    // Check Whether Account Master and Accounts Detail Table has been created or Not For New Financial Year
                    // That is whether user run Year End Process before doing accrued on 31 march 
                    CLoanRecovery CRec = new CLoanRecovery();
                    DataTable dt = new DataTable();
                    dt = CRec.CheckNewFinYear(vNewYrNo, vBranch);
                    if (Convert.ToInt32(dt.Rows[0][0]) == 0)
                    {
                        gblFuction.MsgPopup("Please Run Year End Process First then Do Accrued Posting on 31 March");
                        return;
                    }
                    else
                    {
                        DataTable dtFinyYr = new DataTable();
                        CFinYear oFinYr = new CFinYear();
                        dtFinyYr = oFinYr.GetFYrByYrNo(vNewYrNo, vBranch);
                        vNewShortYr = Convert.ToString(dtFinyYr.Rows[0]["ShortYear"]);
                        vNewAcVouMst = gblValue.ACVouMst + gblFuction.getFinYrNo(vNewYrNo.ToString());
                        vNewAcVouDtl = gblValue.ACVouDtl + gblFuction.getFinYrNo(vNewYrNo.ToString());
                    }
                }
                else
                {
                    vNewAcVouMst = Session[gblValue.ACVouMst].ToString();
                    vNewAcVouDtl = Session[gblValue.ACVouDtl].ToString();
                    vNewShortYr = Session[gblValue.ShortYear].ToString();
                }

                DataRow dr = null;
                string vXml = "";
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add("LoanId", typeof(string));
                dtXml.Columns.Add("AccruedInt", typeof(float));
                dtXml.Columns.Add("AccruedInstNo", typeof(int));
                dtXml.Columns.Add("InstAC", typeof(string));
                dtXml.Columns.Add("DaysGap", typeof(int));
                foreach (GridViewRow gr in gvAccInt.Rows)
                {
                    if (((CheckBox)gr.FindControl("cbApprv")).Checked == true)
                    {
                        dr = dtXml.NewRow();
                        dr["LoanId"] = ((Label)gr.FindControl("lblLoanId")).Text;
                        dr["AccruedInt"] = ((Label)gr.FindControl("lblAccInt")).Text;
                        dr["AccruedInstNo"] = ((Label)gr.FindControl("lblInstNo")).Text;
                        dr["InstAC"] = ((Label)gr.FindControl("lblInstAC")).Text;
                        dr["DaysGap"] = ((Label)gr.FindControl("lblDays")).Text;
                        dtXml.Rows.Add(dr);
                        dtXml.AcceptChanges();
                    }
                }
                dtXml.TableName = "Table1";
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
               // vErr = OLC.SaveAccruedPosting(CurDate, NextDate, vXml, vActMstTbl, vActDtlTbl, vFinYear, vBranch, "J", vAccLed, this.UserID, "Save", 0);
                vErr = OLC.SaveAccruedPostingNew(CurDate, NextDate, vXml, vActMstTbl, vActDtlTbl, vFinYear, vBranch, "J", vAccLed,
                    Convert.ToInt32(Session[gblValue.UserId]), "Save", 0, vNewAcVouMst, vNewAcVouDtl, vNewShortYr);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                OLC = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void gvACList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataTable dtDtl = new DataTable();
            CLoanRecovery oLR = new CLoanRecovery();
            String DelPer = "";
            try
            {
                if (Convert.ToInt32(Session[gblValue.UserId]) != 1)
                {
                    if (this.CanDelete == "Y")
                        DelPer = "Y";
                    else
                        DelPer = "N";
                }
                else
                {
                    DelPer = "Y";
                }
                if (DelPer == "Y")
                {
                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    string vAcMst = Session[gblValue.ACVouMst].ToString();
                    string vAcDtl = Session[gblValue.ACVouDtl].ToString();
                    string vFinYear = Session[gblValue.FinYear].ToString();
                    string vReffID = "";
                    Int32 vErr = 0;

                    if (e.CommandName == "cmdDelete")
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnSlct");
                        Int32 RowIndex = Convert.ToInt32(e.CommandArgument);
                        int totalrow = gvACList.Rows.Count;
                        DateTime pDate = gblFuction.setDate(txtACPostDate.Text);


                        string vLoanId = ((Label)gvACList.Rows[RowIndex].FindControl("lblLoanNo")).Text.ToString();
                        string vBranchCode = ((Label)gvACList.Rows[RowIndex].FindControl("lblBranch")).Text.ToString();
                        Int32 vSlNo = Convert.ToInt32(((Label)gvACList.Rows[RowIndex].FindControl("lblAccruedInstNo")).Text);
                        vReffID = vLoanId + "-" + vSlNo;
                        DateTime vAccDate = gblFuction.setDate(((Label)gvACList.Rows[RowIndex].FindControl("lblPostDate")).Text);
                        string vYrNo = Session[gblValue.FinYrNo].ToString();

                        // If Accrued Posing done on 31 march of any Financial year(i.e End Date of any Financial Year)
                        string StrDD, StrMM, StrYYYY, strDate;
                        string vDate = txtACPostDate.Text;
                        StrDD = vDate.Substring(0, 2);
                        StrMM = vDate.Substring(3, 2);
                        StrYYYY = vDate.Substring(6, 4);
                        string vNewAcVouMst = "";
                        string vNewAcVouDtl = "";
                        string vNewShortYr = "";

                        // If Accrued Posting Done On 31 March of any Financial Year , then we have to delete account posting 
                        // from current Financial Year and Next Financial Year
                        if (Convert.ToInt32(StrMM) == 3 && Convert.ToInt32(StrDD) == 31)
                        {
                            Int32 vNewYrNo = Convert.ToInt32(vYrNo) + 1;
                            DataTable dtFinyYr = new DataTable();
                            CFinYear oFinYr = new CFinYear();
                            dtFinyYr = oFinYr.GetFYrByYrNo(vNewYrNo, vBranchCode);
                            vNewShortYr = Convert.ToString(dtFinyYr.Rows[0]["ShortYear"]);
                            vNewAcVouMst = gblValue.ACVouMst + gblFuction.getFinYrNo(vNewYrNo.ToString());
                            vNewAcVouDtl = gblValue.ACVouDtl + gblFuction.getFinYrNo(vNewYrNo.ToString());
                        }
                        else
                        {
                            vNewAcVouMst = Session[gblValue.ACVouMst].ToString();
                            vNewAcVouDtl = Session[gblValue.ACVouDtl].ToString();
                            vNewShortYr = Session[gblValue.ShortYear].ToString();
                        }

                        String FYear = GetCurrentFinancialYear(vAccDate);
                        if (vFinYear != FYear)
                        {
                            gblFuction.AjxMsgPopup("Accrued Details can not be deleted as Accrued Posting Date is not in same Login  Financial Year...");
                            return;
                        }
                        vErr = oLR.DeleteAccruedPostingNew(vLoanId, vSlNo, vReffID, vAcMst, vAcDtl, vBranchCode, Convert.ToInt32(Session[gblValue.UserId]),
                                                       vAccDate, vNewAcVouMst, vNewAcVouDtl);
                        if (vErr == 0)
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DeleteMsg);
                            dtDtl = oLR.GetAccInstLisByDate(pDate,ddlBr.SelectedValue.ToString());
                            if (dtDtl.Rows.Count > 0)
                            {
                                gvACList.DataSource = dtDtl;
                                gvACList.DataBind();
                            }
                            else
                            {
                                gvACList.DataSource = null;
                                gvACList.DataBind();
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                        }
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("You do not have permission to delete Accrued Posting Record.. Contact your Administrator..");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtDtl = null;
            }
        }
        //protected void gvACList_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    DataTable dtDtl = new DataTable();
        //    CLoanRecovery oLR = new CLoanRecovery();
        //    String DelPer = "";
        //    try
        //    {
        //        if (this.UserID != 1)
        //        {
        //            if (this.CanDelete == "Y")
        //                DelPer = "Y";
        //            else
        //                DelPer = "N";
        //        }
        //        else
        //        {
        //            DelPer = "Y";
        //        }
        //        if (DelPer == "Y")
        //        {
        //            string vBrCode = Session[gblValue.BrnchCode].ToString();
        //            string vAcMst = Session[gblValue.ACVouMst].ToString();
        //            string vAcDtl = Session[gblValue.ACVouDtl].ToString();
        //            string vFinYear = Session[gblValue.FinYear].ToString();
        //            string vReffID = "";
        //            Int32 vErr = 0;

        //            if (e.CommandName == "cmdDelete")
        //            {
        //                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnSlct");
        //                Int32 RowIndex = Convert.ToInt32(e.CommandArgument);
        //                int totalrow = gvACList.Rows.Count;
        //                DateTime pDate = gblFuction.setDate(txtACPostDate.Text);


        //                string vLoanId = ((Label)gvACList.Rows[RowIndex].FindControl("lblLoanNo")).Text.ToString();
        //                Int32 vSlNo = Convert.ToInt32(((Label)gvACList.Rows[RowIndex].FindControl("lblAccruedInstNo")).Text);
        //                vReffID = vLoanId + "-" + vSlNo;
        //                DateTime vAccDate = gblFuction.setDate(((Label)gvACList.Rows[RowIndex].FindControl("lblPostDate")).Text);
        //                String FYear = GetCurrentFinancialYear(vAccDate);
        //                if (vFinYear != FYear)
        //                {
        //                    gblFuction.AjxMsgPopup("Accrued Details can not be deleted as Accrued Posting Date is not in same Login  Financial Year...");
        //                    return;
        //                }
        //                vErr = oLR.DeleteAccruedPosting(vLoanId, vSlNo, vReffID, vAcMst, vAcDtl, vBrCode, this.UserID, vAccDate);
        //                if (vErr == 0)
        //                {
        //                    gblFuction.AjxMsgPopup(gblKUDOS.DeleteMsg);
        //                    dtDtl = oLR.GetAccInstLisByDate(pDate);
        //                    if (dtDtl.Rows.Count > 0)
        //                    {
        //                        gvACList.DataSource = dtDtl;
        //                        gvACList.DataBind();
        //                    }
        //                    else
        //                    {
        //                        gvACList.DataSource = null;
        //                        gvACList.DataBind();
        //                    }
        //                }
        //                else
        //                {
        //                    gblFuction.AjxMsgPopup(gblKUDOS.DBError);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            gblFuction.AjxMsgPopup("You do not have permission to delete Accrued Posting Record.. Contact your Administrator..");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dtDtl = null;
        //    }
        //}
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (txtAccruedDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Accrued Posting Date can not be left blank...");
                return;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pBrCode = ddlBranch.SelectedValue.ToString();
                string pDate = txtAccruedDt.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                Int32 lastday = DateTime.DaysInMonth(Convert.ToInt32(StrYYYY), Convert.ToInt32(StrMM));
                if (lastday != Convert.ToInt32(StrDD))
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be last date of month...");
                    return;
                }
                DateTime CurrDate = gblFuction.setDate(txtAccruedDt.Text);
                DateTime NextDate = CurrDate.AddMonths(1);
                LoadAccruedDetail(CurrDate, NextDate, pBrCode);
            }
        }
        protected void btnShowList_Click(object sender, EventArgs e)
        {
            if (txtACPostDate.Text != "")
            {
                DateTime vDate = gblFuction.setDate(txtACPostDate.Text);
                string pBrCode = ddlBr.SelectedValue.ToString();
                LoadAccPosList(vDate, pBrCode);
            }
        }
    }
}