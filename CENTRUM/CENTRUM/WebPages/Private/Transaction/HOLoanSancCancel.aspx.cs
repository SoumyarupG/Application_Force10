using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class HOLoanSancCancel : CENTRUMBase
    {
        protected int cPgNo = 1;
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
                ViewState["StateEdit"] = null;
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                PopBranch(Session[gblValue.UserName].ToString());

            }
        }

        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]), "R");
                Session[gblValue.AreaID] = dt.Rows[0]["AreaID"].ToString();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
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
                this.PageHeading = "HO Sanction Hold Loan";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuHOLoanSanchold);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "HO Sanction Cancel Loan", false);
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ValidDate() == true)
            {
                try
                {
                    //string vBrCode = Session[gblValue.BrnchCode].ToString();
                    string vBrCode = ddlBranch.SelectedValue.ToString();
                    LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDone_Click(object sender, EventArgs e)
        {
            CApplication oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    if (ValidateFields() == false) return;

                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["SanDate"].ToString() == "")
                        {
                            dr["SanDate"] = "01/01/1900";
                        }
                        if (dr["CancelRsn"].ToString() == "")
                        {
                            dr["CancelRsn"] = "Equifax Cancel but Sanctioned";
                        }
                        if (dr["SancCanRsn"].ToString() == "")
                        {
                            dr["SancCanRsn"] = "EQUFAX CANCEL MEMBER";
                        }
                    }

                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    //-----------XML Save----------
                    vErr = oApp.UpdateCancelLoan(vXmlData, this.UserID, vBrCode, "E", 0, vSanDt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                TextBox txtSancCanRsn = (TextBox)gvSanc.Rows[vRow].FindControl("txtSancCanRsn");

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


                    if (chkApp.Checked == true && gblFuction.IsDate(txtSanDt.Text) == false)
                    {
                        gblFuction.MsgPopup("Please enter Proper Sanction Date..");
                        vResult = false;
                    }

                    if (chkApp.Checked == true && txtSancCanRsn.Text.Trim() == "")
                    {
                        gblFuction.MsgPopup("Please enter Sanction Reason..");
                        vResult = false;
                    }
                }
                if (chkApp.Checked == false && chkCan.Checked == false)
                {

                }
            }
            return vResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pAppMode"></param>
        /// <param name="pFromDt"></param>
        /// <param name="pToDt"></param>
        /// <param name="pBranch"></param>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 totalRows = 0;
            Int32 vRows = 0;
            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetSanctionList(vFromDt, vToDt, pAppMode, vBrCode, 0, pPgIndx, ref totalRows, Convert.ToInt32(Session[gblValue.UserId].ToString()));
                ViewState["Sanc"] = dt;
                gvSanc.DataSource = dt;
                gvSanc.DataBind();
                lblTotalPages.Text = CalTotPgs(totalRows).ToString();
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
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, cPgNo);

        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            //Int32 vRst = 0;
            //CApplication oApp = null;
            DateTime vAppDate;
            string vLnAppDt = "", vBranch = "", vMsg = "";//, vAppId = "";
            DataTable dt = null;
            Int32 vDisbDateCount = Convert.ToInt32(ViewState["DCount"]);
            CApplication oCG = null;
            //DataTable dtimp
            try
            {
                dt = (DataTable)ViewState["Sanc"];
                //dtImp = dt.Clone();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr.ItemArray[0].ToString() != "")
                //        dtImp.ImportRow(dr);
                //}


                //vBranch = Session[gblValue.BrnchCode].ToString();
                vBranch = ddlBranch.SelectedValue.ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
                TextBox txtSanAmt = (TextBox)row.FindControl("txtSanAmt");
                TextBox txtSanDt = (TextBox)row.FindControl("txtSanDt");
                TextBox txtReason = (TextBox)row.FindControl("txtReason");
                TextBox txtSancCanRsn = (TextBox)row.FindControl("txtSancCanRsn");
                //if (txtExDisbDt.Text == "" || gblFuction.IsDate(txtExDisbDt.Text) == false)
                //{
                //    gblFuction.AjxMsgPopup("Expected Disbursement date cannot be blank");
                //    chkApp.Checked = false;
                //    return;
                //}


                //vAppId = row.Cells[13].Text;
                if (txtSanDt.Text == "" || gblFuction.IsDate(txtSanDt.Text) == false)
                    vAppDate = gblFuction.setDate(txtAppDt.Text);
                else
                    vAppDate = gblFuction.setDate(row.Cells[12].Text);
                txtSancCanRsn.Text = "";
                //oApp = new CApplication();
                //vRst = oApp.ChkEditSanction(vAppId, vBranch);
                //if (vRst == 1)
                //{
                //    gblFuction.AjxMsgPopup("more than 15 loan cannot have the same Exp Disb Date");
                //    chkApp.Checked = true;
                //    return;
                //}
                if (this.RoleId != 1)
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= vAppDate)
                        {
                            gblFuction.AjxMsgPopup("Day End already done...");
                            if (chkApp.Checked == true)
                                chkApp.Checked = false;
                            else
                                chkApp.Checked = true;
                            return;
                        }
                    }
                }
                oCG = new CApplication();
                vMsg = oCG.ChkLoanAppOther(row.Cells[13].Text);
                if (vMsg != "")
                {
                    gblFuction.MsgPopup(vMsg);
                    if (chkApp.Checked == true)
                        chkApp.Checked = false;
                    else
                        chkApp.Checked = true;
                    return;
                }
                if (chkApp.Checked == true && chkCan.Checked == false)
                {
                    vLnAppDt = row.Cells[1].Text;
                    if (gblFuction.setDate(vLnAppDt) > gblFuction.setDate(txtAppDt.Text))
                    {
                        gblFuction.AjxMsgPopup("Sanction Date can not be less than the Application Date.");
                        row.Cells[8].Text = "N";
                        txtSanAmt.Text = "";
                        txtSanDt.Text = "";
                        chkApp.Checked = false;
                        chkCan.Checked = false;
                        dt.Rows[row.RowIndex]["Approved"] = "N";
                        dt.Rows[row.RowIndex]["SanDate"] = "";
                        dt.AcceptChanges();
                        upSanc.Update();
                        return;
                    }
                    else
                    {
                        row.Cells[8].Text = "Y";
                        txtSanAmt.Text = row.Cells[6].Text;
                        txtSanDt.Text = txtAppDt.Text;
                        dt.Rows[row.RowIndex]["Approved"] = "Y";
                        dt.Rows[row.RowIndex]["ApprovedAmt"] = txtSanAmt.Text;
                        dt.Rows[row.RowIndex]["SanDate"] = txtAppDt.Text;
                        dt.AcceptChanges();
                    }
                    txtReason.Enabled = false;
                    txtSancCanRsn.Enabled = true;
                    chkCan.Enabled = false;
                    txtSanAmt.Enabled = false;
                    txtSanDt.Enabled = true;
                }
                else if (chkApp.Checked == true && chkCan.Checked == true)
                {
                    gblFuction.AjxMsgPopup("Sanction and Cancel can not be Selected.");
                    chkApp.Checked = false;
                    chkCan.Checked = false;
                    row.Cells[8].Text = "N";
                    row.Cells[10].Text = "N";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.AcceptChanges();
                    upSanc.Update();
                    return;
                }
                else
                {
                    txtReason.Enabled = false;
                    txtSancCanRsn.Enabled = false;
                    chkCan.Enabled = true;
                    txtSanAmt.Enabled = false;
                    row.Cells[8].Text = "N";
                    txtSanAmt.Text = "";
                    txtSanDt.Text = "";
                    dt.Rows[row.RowIndex]["Approved"] = "N";
                    dt.Rows[row.RowIndex]["ApprovedAmt"] = "0";
                    dt.Rows[row.RowIndex]["SanDate"] = "";
                    dt.AcceptChanges();
                }

                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            finally
            {
                //oApp = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            TextBox txtSanAmt = (TextBox)row.FindControl("txtSanAmt");
            TextBox txtSanDt = (TextBox)row.FindControl("txtSanDt");
            TextBox txtReason = (TextBox)row.FindControl("txtReason");
            TextBox txtSancCanRsn = (TextBox)row.FindControl("txtSancCanRsn");
            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                if (row.Cells[8].Text == "Y")
                {
                    checkbox2.Checked = false;
                    gblFuction.AjxMsgPopup("Please remove Sanction before make Cancel.");
                    row.Cells[10].Text = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.AcceptChanges();
                    upSanc.Update();
                    return;
                }
                txtReason.Enabled = false;
                txtSancCanRsn.Enabled = false;
                //txtReason.Text = "";
                txtSanAmt.Enabled = false;
                chkApp.Enabled = false;
                row.Cells[10].Text = "Y";
                txtSanDt.Enabled = false;
                txtSanDt.Text = "";
                dt.Rows[row.RowIndex]["Cancel"] = "Y";
                dt.Rows[row.RowIndex]["SanDate"] = "";
                dt.AcceptChanges();
            }
            else
            {
                txtReason.Enabled = false;
                txtSancCanRsn.Enabled = true;
                //txtReason.Text = "";
                chkApp.Enabled = true;
                row.Cells[10].Text = "N";
                dt.Rows[row.RowIndex]["Cancel"] = "N";
                dt.AcceptChanges();
            }
            txtSanDt.Text = "";
            dt.Rows[row.RowIndex]["SanDate"] = "";
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSanAmt_TextChanged(object sender, EventArgs e)
        {
            decimal vAmount = 0;
            Int32 vRst = 0;
            DataTable dt = null;
            //string vBranch = Session[gblValue.BrnchCode].ToString();
            string vBranch = ddlBranch.SelectedValue.ToString();
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            string vAppId = gvRow.Cells[1].Text;
            TextBox txtAmt = (TextBox)gvRow.FindControl("txtSanAmt");
            CheckBox chkApp = (CheckBox)gvRow.FindControl("chkApp");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["Sanc"];
            vAmount = Convert.ToDecimal(gvRow.Cells[6].Text);
            if (txtAmt.Text == "" || txtAmt.Text == "0")
            {
                gblFuction.AjxMsgPopup("Sanction amount can not be zero...");
                txtAmt.Text = vAmount.ToString();
                dt.Rows[gvRow.RowIndex]["ApprovedAmt"] = txtAmt.Text;
                dt.AcceptChanges();
                return;
            }
            if (vAmount < Convert.ToDecimal(txtAmt.Text))
            {
                gblFuction.AjxMsgPopup("Sanction Can not Exceed Loan Amount.");
                txtAmt.Text = vAmount.ToString();
                dt.Rows[gvRow.RowIndex]["ApprovedAmt"] = txtAmt.Text;
                dt.AcceptChanges();
                return;
            }
            vRst = oApp.ChkEditSanction(vAppId, vBranch);
            if (vRst == 1)
            {
                gblFuction.AjxMsgPopup("Selected Loan has been disbursed.");
                txtAmt.Text = Convert.ToString(vAmount);
                dt.Rows[gvRow.RowIndex]["ApprovedAmt"] = txtAmt.Text;
                dt.AcceptChanges();
                return;
            }
            dt.Rows[gvRow.RowIndex]["ApprovedAmt"] = txtAmt.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtSanDt_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtSanDt = (TextBox)gvRow.FindControl("txtSanDt");
            dt = (DataTable)ViewState["Sanc"];
            DateTime vAppDate = gblFuction.setDate(gvRow.Cells[1].Text);
            if (txtSanDt.Text == "" || gblFuction.IsDate(txtSanDt.Text) == false)
            {
                gblFuction.AjxMsgPopup("Sanction Date Invalid...");
                txtSanDt.Text = txtAppDt.Text;
                dt.Rows[gvRow.RowIndex]["SanDate"] = txtSanDt.Text;
                dt.AcceptChanges();
                return;
            }
            DateTime vSanDate = gblFuction.setDate(txtSanDt.Text);
            if (vSanDate < vAppDate)
            {
                gblFuction.AjxMsgPopup("Sanction Can not be Less Than Application Date");
                txtSanDt.Text = gvRow.Cells[1].Text;
                dt.Rows[gvRow.RowIndex]["SanDate"] = txtSanDt.Text;
                dt.AcceptChanges();
                return;
            }
            else
            {
                dt.Rows[gvRow.RowIndex]["SanDate"] = txtSanDt.Text;
                dt.AcceptChanges();
            }

            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");
                    TextBox txtReason = (TextBox)e.Row.FindControl("txtReason");
                    TextBox txtSancCanRsn = (TextBox)e.Row.FindControl("txtSancCanRsn");
                    TextBox txtAmt = (TextBox)e.Row.FindControl("txtSanAmt");
                    TextBox txtSanDt = (TextBox)e.Row.FindControl("txtSanDt");
                    if (txtSancCanRsn.Text.Trim() == "")
                    {
                        txtSancCanRsn.Text = "EQUFAX CANCEL MEMBER";
                    }
                    if (e.Row.Cells[8].Text == "Y")
                    {
                        chkApp.Checked = true;
                        chkCan.Enabled = false;
                        txtReason.Enabled = false;
                        txtSancCanRsn.Enabled = true;
                        txtSanDt.Enabled = true;
                        txtAmt.Enabled = true;
                    }
                    else if (e.Row.Cells[8].Text == "N")
                    {
                        chkApp.Checked = false;
                        chkCan.Enabled = true;
                        txtReason.Enabled = false;
                        txtSancCanRsn.Enabled = false;
                        txtSanDt.Enabled = false;
                        txtAmt.Enabled = false;
                    }

                    //Bind Calcel

                    if (e.Row.Cells[10].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkApp.Enabled = false;
                        txtReason.Enabled = false;
                        txtSancCanRsn.Enabled = false;
                    }
                    else if (e.Row.Cells[10].Text == "N")
                    {
                        chkCan.Checked = false;
                        chkApp.Enabled = true;
                        txtReason.Enabled = false;
                        txtSancCanRsn.Enabled = true;
                    }

                }
            }
            finally
            {

            }
        }


        protected void txtReason_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtReason = (TextBox)gvRow.FindControl("txtReason");
            dt = (DataTable)ViewState["Sanc"];

            if (txtReason.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["CancelRsn"] = txtReason.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void txtSancCanRsn_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtSancCanRsn = (TextBox)gvRow.FindControl("txtSancCanRsn");
            dt = (DataTable)ViewState["Sanc"];

            if (txtSancCanRsn.Text != "")
            {
                dt.Rows[gvRow.RowIndex]["SancCanRsn"] = txtSancCanRsn.Text;
                dt.AcceptChanges();
                return;
            }
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        //protected void txtExDisbDt_TextChanged(object sender, EventArgs e)
        //{
        //    DataTable dt = null;
        //    CApplication oApp = null;
        //    try
        //    {
        //        if (gblFuction.IsDate(txtExDisbDt.Text) == true)
        //        {
        //            oApp = new CApplication();
        //            dt = oApp.CountExpDisbDate(gblFuction.setDate(txtExDisbDt.Text));
        //            if (dt.Rows.Count > 0)
        //                ViewState["DCount"] = dt.Rows.Count;
        //            else
        //                ViewState["DCount"] = 0;

        //            btnShow_Click(sender, e);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oApp = null;
        //    }
        //}
    }
}