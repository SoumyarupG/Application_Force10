using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class NEFTDisbApprvl : CENTRUMBase
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //txtFrmDt.Text = Convert.ToString(Session[gblValue.LoginDate]);

                //PopCM();
                LoadGrid(0);
                tabReSchdl.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
           // LoadGrid(Int32.Parse(lblCurrentPage.Text));
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Pre NEFT disbursement Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.GetModuleByRole(mnuID.mnuLoanReschedul)
                this.GetModuleByRole(mnuID.mnuNEFTDisbApprvl);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Pre NEFT disbursement Approval", false);
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
            CNEFTDisbApprvl oNeft = null;
            Int32 vRows = 0;
            try
            {
                oNeft = new CNEFTDisbApprvl();
                dt = oNeft.GetNEFTDisbListPG(Session[gblValue.BrnchCode].ToString(), gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                gvDtl.DataSource = dt.DefaultView;
                gvDtl.DataBind();
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
            //Int32 vRescheduleId = Convert.ToInt32(ViewState["RescheduleId"]);
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0;
            string vXml = "";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
                vXml=XmlFromGrid();
                
                //if (ValidateFields() == false)
                //    return false;
                
                CNEFTDisbApprvl oNEFT = null;

                
                try
                {
                    if (Mode == "Save")
                    {

                        oNEFT = new CNEFTDisbApprvl();

                        vErr = oNEFT.InsertNEFTDisbAppvl(vXml, vBrCode, Convert.ToInt32(Session[gblValue.UserId]), "I");

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
            
            LoadGrid(0);
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
                    //LoadGrid(0);
                    StatusButton("View");
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
                Label lblAppDate = (Label)e.Row.FindControl("lblAppDate");
                Label lblAppNo = (Label)e.Row.FindControl("lblAppNo");
                Label lblRO = (Label)e.Row.FindControl("lblRO");
                Label lblCenter = (Label)e.Row.FindControl("lblCenter");
                Label lblGroup = (Label)e.Row.FindControl("lblGroup");
                Label lblMemName = (Label)e.Row.FindControl("lblMemName");
                Label lblLnShceme = (Label)e.Row.FindControl("lblLnShceme");
                Label lblLnAmt = (Label)e.Row.FindControl("lblLnAmt");
                Label lblExpDisbDate = (Label)e.Row.FindControl("lblExpDisbDate");
                CheckBox ChkAppv = (CheckBox)e.Row.FindControl("ChkAppv");
                CheckBox ChkPostpone = (CheckBox)e.Row.FindControl("ChkPostpone");
                CheckBox ChkCancel = (CheckBox)e.Row.FindControl("ChkCancel");
                try
                {
                    lblAppDate.Text = e.Row.Cells[14].Text.Trim();
                    lblAppNo.Text = e.Row.Cells[15].Text.Trim();
                    lblRO.Text = e.Row.Cells[16].Text.Trim();
                    lblCenter.Text = e.Row.Cells[17].Text.Trim();
                    lblGroup.Text = e.Row.Cells[18].Text.Trim();
                    lblMemName.Text = e.Row.Cells[19].Text.Trim();
                    lblLnShceme.Text = e.Row.Cells[20].Text.Trim();
                    //lblAppDate.Text = e.Row.Cells[13].Text.Trim();
                    lblLnAmt.Text = e.Row.Cells[21].Text.Trim();
                    lblExpDisbDate.Text = e.Row.Cells[22].Text.Trim();
                    string ID = e.Row.Cells[23].Text.Trim();
                    if (e.Row.Cells[24].Text == "1")
                        ChkAppv.Checked = true;
                    else
                        ChkAppv.Checked = false;
                    if (e.Row.Cells[25].Text == "1")
                        ChkPostpone.Checked = true;
                    else
                        ChkPostpone.Checked = false;
                    if (e.Row.Cells[26].Text == "1")
                        ChkCancel.Checked = true;
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
            dt.Columns.Add("LoanAppId");
            dt.Columns.Add("AppvYN");
            dt.Columns.Add("PostPoneYN");
            dt.Columns.Add("CancelYN");
            dt.Columns.Add("PostRemarks");
            dt.Columns.Add("Remarks");
            foreach (GridViewRow gr in gvDtl.Rows)
            {


                CheckBox ChkAppv = (CheckBox)gr.FindControl("ChkAppv");
                 CheckBox ChkPostpone = (CheckBox)gr.FindControl("ChkPostpone");
                 CheckBox ChkCancel = (CheckBox)gr.FindControl("ChkCancel");
                 TextBox txtPostRemark = (TextBox)gr.FindControl("txtPostRemark");
                 TextBox txtCanRemark = (TextBox)gr.FindControl("txtCanRemark");
                string LoanAppId = gr.Cells[23].Text;
                if (ChkAppv.Checked == true || ChkPostpone.Checked==true || ChkCancel.Checked==true)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = Convert.ToString(i);
                    dr["LoanAppId"] = LoanAppId;
                    if (ChkAppv.Checked == true)
                    {
                        dr["AppvYN"] = 'Y';
                    }
                    else
                        dr["AppvYN"] = 'N';
                    if (ChkPostpone.Checked == true)
                    {
                        dr["PostPoneYN"] = 'Y';
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
                        dr["PostPoneYN"] = 'N';
                    if (ChkCancel.Checked == true)
                    {
                        dr["CancelYN"] = 'Y';
                        if (txtCanRemark.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Please Give a cancel remarks for cancel selection...");
                            txtCanRemark.Focus();
                            return "";
                        }
                        else
                        {
                            dr["Remarks"] = txtCanRemark.Text;
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
        protected void ChkAppv_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox ChkAppv = (CheckBox)row.FindControl("ChkAppv");
            CheckBox ChkPostpone = (CheckBox)row.FindControl("ChkPostpone");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");

            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("ChkAppv");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[27].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (chkDisbT.Checked == true)
                {
                    ChkCancelT.Checked = false;
                    ChkCancelT.Enabled = false;
                    ChkPostponeT.Checked = false;
                    ChkPostponeT.Enabled = false;

                }
                else
                {
                    ChkCancelT.Enabled = true;
                    ChkPostponeT.Enabled = true;
                }

            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();
            
        }
        protected void ChkPostpone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox ChkAppv = (CheckBox)row.FindControl("ChkAppv");
            CheckBox ChkPostpone = (CheckBox)row.FindControl("ChkPostpone");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("ChkAppv");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                TextBox txtPostRemark = (TextBox)gR.FindControl("txtPostRemark");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
                
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    
                    ChkCancelT.Checked = false;
                    
                    ChkCancelT.Enabled = false;
                    chkDisbT.Checked = false;
                    chkDisbT.Enabled = false;
                    txtPostRemark.Enabled = true;
                }
                else
                {
                    txtPostRemark.Text = "";
                    txtPostRemark.Enabled = false;
                    ChkCancelT.Enabled = true;
                    chkDisbT.Enabled = true;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
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
            CheckBox ChkAppv = (CheckBox)row.FindControl("ChkAppv");
            CheckBox ChkPostpone = (CheckBox)row.FindControl("ChkPostpone");
            CheckBox ChkCancel = (CheckBox)row.FindControl("ChkCancel");
            double vTotalAmt = 0, vTotCash = 0, vTotCan = 0, VTotPost = 0;
            Int32 vTotCount = 0, vCashCnt = 0, vCanCnt = 0, vPostCnt = 0;
            foreach (GridViewRow gR in gvDtl.Rows)
            {
                CheckBox chkDisbT = (CheckBox)gR.FindControl("ChkAppv");
                CheckBox ChkCancelT = (CheckBox)gR.FindControl("ChkCancel");
                CheckBox ChkPostponeT = (CheckBox)gR.FindControl("ChkPostpone");
                TextBox txtCanRemark = (TextBox)gR.FindControl("txtCanRemark");
                if (chkDisbT.Checked == true)
                {
                    vTotalAmt = vTotalAmt + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vTotCount = vTotCount + 1;
                }
               
                if (ChkCancelT.Checked == true)
                {
                    vTotCan = vTotCan + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vCanCnt = vCanCnt + 1;
                }
                if (ChkPostponeT.Checked == true)
                {
                    VTotPost = VTotPost + Convert.ToDouble(gR.Cells[21].Text.Trim());
                    vPostCnt = vPostCnt + 1;
                }
                if (ChkCancelT.Checked == true)
                {
                    chkDisbT.Checked = false;
                   
                    chkDisbT.Enabled = false;
                    
                    txtCanRemark.Enabled = true;
                    ChkPostponeT.Enabled = false;
                    ChkPostponeT.Checked = false;
                }
                else
                {
                    chkDisbT.Enabled = true;
                    txtCanRemark.Text = "";
                    txtCanRemark.Enabled = false;
                    ChkPostponeT.Enabled = true;
                }
            }
            txtTotAmt.Text = Convert.ToString(vTotalAmt);
            txtTotCount.Text = Convert.ToString(vTotCount);
            
            txtCanTot.Text = Convert.ToString(vTotCan);
            txtCanCnt.Text = Convert.ToString(vCanCnt);
            txtPostTot.Text = Convert.ToString(VTotPost);
            txtPostCnt.Text = Convert.ToString(vPostCnt);
            UpTot.Update();
            UpCount.Update();
            
        }
    }
}