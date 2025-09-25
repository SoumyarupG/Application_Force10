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
    public partial class CancelNEFTDisbursement : CENTRUMBase
    {
        protected int cPgNo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PopBranch(Session[gblValue.UserName].ToString());
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
                TxtDtFrm.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDt.Text = Convert.ToString(Session[gblValue.LoginDate]); 
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
                this.PageHeading = "Cancel NEFT Disbursement";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCancelNEFTdisb);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cancel NEFT Disbursement", false);
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
       protected void ChkCancel_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0;
            Int32 vTotCount = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                TextBox txtRemark = (TextBox)gR.FindControl("txtRemarks");
                if (ChkCancelT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[19].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                else
                {
                    txtRemark.Text = "";
                }

            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            UpTot.Update();
            
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
                dt = oNeft.GetCancelNEFT(gblFuction.setDate(TxtDtFrm.Text), gblFuction.setDate(txtDt.Text), ddlBranch.SelectedValues.Replace("|", ","), txtSearch.Text);
                gvDtl.DataSource = dt.DefaultView;
                gvDtl.DataBind();
              
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
            string vXml = "", pMsg = "";
            double TotDisbAmt = 0, vClosingBal = 0;
           
            string vdate = "";
            vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vclosedt = gblFuction.setDate(vdate);
            vXml = XmlFromGrid();
            TotDisbAmt = TotalDisbAmt();
            //if (ValidateFields() == false)
            //    return false;

            CNEFTTransfer oNEFT = null;
           

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
                    //vClosingBal = oNEFT.GetClosingBalBranchWise(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtDt.Text), vDescIDBank, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
                    //if (vClosingBal < TotDisbAmt)
                    //{
                    //    gblFuction.AjxMsgPopup("Insufficient Balance");
                    //    return false;
                    //}
                    vErr = oNEFT.InsertCancelNEFT(vXml, this.UserID, "I", vclosedt, ref pMsg);
                    if (vErr == 2 || vErr == 3 || vErr == 4)
                    {
                        gblFuction.AjxMsgPopup(pMsg);
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
                   
                    btnSave.Enabled = false; 
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblBranch = (Label)e.Row.FindControl("lblBranch");
                Label lblLoanDate = (Label)e.Row.FindControl("lblLoanDate");
                Label lblLoanNo = (Label)e.Row.FindControl("lblLoanNo");
                Label lblRO = (Label)e.Row.FindControl("lblRO");
                Label lblGroup = (Label)e.Row.FindControl("lblGroup");
                Label lblMemName = (Label)e.Row.FindControl("lblMemName");
                Label lblLnShceme = (Label)e.Row.FindControl("lblLnShceme");
                Label lblLnAmt = (Label)e.Row.FindControl("lblLnAmt");         
                Label lblBankNm = (Label)e.Row.FindControl("lblBankNm");               
                CheckBox ChkCancel = (CheckBox)e.Row.FindControl("ChkCancel");
                
                try
                {
                    lblBranch.Text = e.Row.Cells[11].Text.Trim();
                    lblLoanDate.Text = e.Row.Cells[12].Text.Trim();
                    lblLoanNo.Text = e.Row.Cells[13].Text.Trim();
                    lblRO.Text = e.Row.Cells[14].Text.Trim();
                    lblGroup.Text = e.Row.Cells[16].Text.Trim();
                    lblMemName.Text = e.Row.Cells[17].Text.Trim();
                    lblLnShceme.Text = e.Row.Cells[18].Text.Trim();                   
                    lblLnAmt.Text = e.Row.Cells[19].Text.Trim();                  
                    lblBankNm.Text = e.Row.Cells[20].Text.Trim();
                    string ID = e.Row.Cells[22].Text.Trim();
                    if (e.Row.Cells[21].Text == "Y")
                    {
                        ChkCancel.Checked = true;                       
                    }
                    else                    
                        ChkCancel.Checked = false;                      
                    
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
            dt.Columns.Add("LoanId");           
            dt.Columns.Add("CancelYN");
            dt.Columns.Add("Remarks");   
            foreach (GridViewRow gr in gvDtl.Rows)
            {               
                CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
                TextBox txtLoanId = (TextBox)gr.FindControl("txtLoanId");
                TextBox txtRemarks = (TextBox)gr.FindControl("txtRemarks");
                string LoanId = txtLoanId.Text;
                if (ChkCancel.Checked == true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanId"] = LoanId;                  
                   
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
                            dr["Remarks"] = txtRemarks.Text;
                        }
                    }
                    else
                        dr["CancelYN"] = 'N';
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                    i++;
                }
            }
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXML = oSW.ToString();
            }
            return vXML;

        }

        private double TotalDisbAmt()
        {
            double TotAmt = 0;
            //foreach (GridViewRow gr in gvDtl.Rows)
            //{
            //    CheckBox chkDisb = (CheckBox)gr.FindControl("chkDisb");
            //    CheckBox chkAppFrCashDisb = (CheckBox)gr.FindControl("chkAppFrCashDisb");
            //    CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
            //    Label lblLnAmt = (Label)gr.FindControl("lblLnAmt");
            //    if (chkDisb.Checked == true || chkAppFrCashDisb.Checked == true || ChkCancel.Checked == true)
            //    {
            //        TotAmt += Convert.ToDouble(lblLnAmt.Text.Trim());

            //    }

            //}
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
            //string vDescIDBank = ddlBank.SelectedValue.ToString();
            CNEFTTransfer oNEFT = null;
            oNEFT = new CNEFTTransfer();
            
            TotDisbAmt = TotalDisbAmt();
            vXml = XmlFromGrid();
            TotDisbAmt = TotalDisbAmt();

            //if (ddlBank.SelectedIndex == 0)
            //{
            //    gblFuction.AjxMsgPopup("Select Bank");
            //    return;
            //}

            if (TotDisbAmt == 0)
            {
                gblFuction.AjxMsgPopup("Please select disburse atleast one loan");
                return;
            }

            //vClosingBal = oNEFT.GetClosingBalBranchWise(gblFuction.setDate(Session[gblValue.FinFromDt].ToString()), gblFuction.setDate(txtDt.Text), vDescIDBank, Convert.ToInt32(Session[gblValue.FinYrNo].ToString()));
            //if (vClosingBal < TotDisbAmt)
            //{
            //    gblFuction.AjxMsgPopup("Insufficient Balance");
            //    return;
            //}
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
            //ds = oAcGenLed.GetGenLedSubsidairyDtl(ddlBank.SelectedValue.ToString());
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
            //ds = oRpt.NEFTTransferLetter(vXml,ddlBank.SelectedValue.ToString(), gblFuction.setDate(txtDt.Text));
            dt = ds.Tables[0];
            dt1 = ds.Tables[1];
            htw.WriteLine("<tr><td colspan='12'><font size='3'></font></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>Please transfer of Rs. " + dt.Rows[0]["TotTransAmt"].ToString() + " (Rupees " + dt.Rows[0]["NumToWord"].ToString() + ") only through NEFT RTGS </font></td></tr>");
            htw.WriteLine("<tr><td align=left' colspan='12'><font size='3'>from our Current Account No:  " + dt2.Rows[0]["Phone"].ToString() + " to the below mentioned accounts of Unity Small Finance Bank Ltd. </font></td></tr>");
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
       
       
      
        protected void btnPrn_Click(object sender, EventArgs e)
        {
            GenerateReport();
            btnSave.Enabled = true;
            //ddlBank.Enabled = true;
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
    }
}
  