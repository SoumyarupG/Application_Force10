using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;
using EO.Web;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class LoanSancn : CENTRUMBase
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

                if (vBrCode == "0000")
                {
                    PopBranch(Session[gblValue.UserName].ToString());
                    ddlBranch.Visible = true;
                    lblBr.Visible = true;
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
                        ddlBranch.DataSource = dt;
                        ddlBranch.DataTextField = "BranchName";
                        ddlBranch.DataValueField = "BranchCode";
                        ddlBranch.DataBind();
                    }
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Sanction";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLoanSanction);
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
                    string vBrCode = Session[gblValue.BrnchCode].ToString() == "0000" ? ddlBranch.SelectedValues.Replace("|", ",") : Session[gblValue.BrnchCode].ToString();
                    if (rdbOpt.SelectedValue == "N")
                    {
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    }
                    else if (rdbOpt.SelectedValue == "A")
                    {
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    }
                    else if (rdbOpt.SelectedValue == "C")
                    {
                        LoadGrid("C", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
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
            Int32 vErr = 0;
            string vXmlData = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            if (gvSanc.Rows.Count == 0)
            {
                gblFuction.AjxMsgPopup("No Records to Update.");
                return;
            }
            else
            {
                for (int i = 1; i <= gvSanc.Rows.Count; i++)
                {
                    CheckBox chkCan = (CheckBox)gvSanc.Rows[i - 1].FindControl("chkCan");
                    TextBox txtCanReason = (TextBox)gvSanc.Rows[i - 1].FindControl("txtCanReason");

                    if (chkCan.Checked == true && rdbOpt.SelectedValue != "C")
                    {
                        if (Convert.ToString(txtCanReason.Text) == "")
                        {
                            gblFuction.MsgPopup("Cancel Reason Cannot be Blank...");
                            txtCanReason.Focus();
                            return;
                        }
                        else if (Convert.ToString(txtCanReason.Text).Length < 10)
                        {
                            gblFuction.MsgPopup("Cancel Reason Must be of 10 Characters...");
                            txtCanReason.Focus();
                            return;
                        }
                    }
                }
            }

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
                            dr["CancelRsn"] = "XX";
                        }
                        if (dr["EQIFAXNo"].ToString() == "")
                        {
                            dr["EQIFAXNo"] = "XX";
                        }
                        if (dr["EQIFAXdt"].ToString() == "")
                        {
                            dr["EQIFAXdt"] = "01/01/1900";
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
                    vErr = oApp.UpdateSanction(vXmlData, this.UserID, vBrCode, "E", 0, vSanDt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        LoadGrid("A", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                        rdbOpt.SelectedValue = "A";
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

        private Boolean ValidateFields()//To Check
        {
            Boolean vResult = true;
            return vResult;
        }

        private void LoadGrid(string pAppMode, string pFromDt, string pToDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;
            Int32 vLoanProduct = 0;

            //  Int32.TryParse(ddlLnProd.SelectedValue, out vLoanProduct);

            try
            {
                string vBrCode = pBranch;
                DateTime vFromDt = gblFuction.setDate(pFromDt);
                DateTime vToDt = gblFuction.setDate(pToDt);
                oLS = new CApplication();
                dt = oLS.GetSanctionList(vFromDt, vToDt, pAppMode, vBrCode, vLoanProduct, pPgIndx, ref vRows, Convert.ToInt32(Session[gblValue.UserId]));
                foreach (DataRow dr in dt.Rows) // search whole table
                {
                    if (dr["M_IdentyProfNo"].ToString().Length == 12)
                    {
                        dr["M_IdentyProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_IdentyProfNo"]).Substring(Convert.ToString(dr["M_IdentyProfNo"]).Length - 4, 4));
                    }
                    if (dr["M_AddProfNo"].ToString().Length == 12)
                    {
                        dr["M_AddProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dr["M_AddProfNo"]).Substring(Convert.ToString(dr["M_AddProfNo"]).Length - 4, 4));
                    }
                }
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

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            string vBranch = "";
            DataTable dt = null;
            Int32 vDisbDateCount = Convert.ToInt32(ViewState["DCount"]);
            try
            {
                dt = (DataTable)ViewState["Sanc"];

                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
                CheckBox chkCan = (CheckBox)row.FindControl("chkCan");

                if (this.RoleId != 1)     // 1 for Admin  4 for BM
                {
                    if (Session[gblValue.EndDate] != null)
                    {
                        if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) >= gblFuction.setDate((txtAppDt.Text.ToString())))
                        {
                            gblFuction.AjxMsgPopup("Day End already done...");
                            if (chkApp.Checked == true)
                            {
                                chkApp.Checked = false;
                            }
                            else
                            {
                                chkApp.Checked = true;
                            }
                            return;
                        }
                    }
                    if (Convert.ToDouble(row.Cells[8].Text) > Convert.ToDouble(Session[gblValue.SncApprAmt].ToString()))
                    {
                        gblFuction.AjxMsgPopup("Approval Power Denied..!!");
                        chkApp.Checked = false;
                        return;
                    }
                }

                if (chkApp.Checked)
                {
                    row.Cells[10].Text = "Y";
                    dt.Rows[row.RowIndex]["Approved"] = "Y";
                    dt.Rows[row.RowIndex]["ApprovedAmt"] = row.Cells[8].Text;
                    dt.Rows[row.RowIndex]["SanDate"] = gblFuction.setDate(txtAppDt.Text);
                    chkCan.Enabled = false;
                }
                else
                {
                    CApplication OAp = new CApplication();
                    int count = OAp.chkSancAppRej(row.Cells[21].Text, "Approved", row.Cells[15].Text);
                    if (count > 0)
                    {
                        gblFuction.AjxMsgPopup("You can not Edit this application because this application has been approved by branch..!!");
                        chkApp.Checked = true;
                        return;
                    }
                    else
                    {
                        row.Cells[10].Text = "N";
                        dt.Rows[row.RowIndex]["Approved"] = "N";
                        dt.Rows[row.RowIndex]["ApprovedAmt"] = 0.00;
                        dt.Rows[row.RowIndex]["SanDate"] = "";
                        chkCan.Enabled = true;
                    }
                }

                dt.AcceptChanges();
                ViewState["Sanc"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        protected void chkCan_CheckedChanged(object sender, EventArgs e)
        {
            int User = 0;
            User = Convert.ToInt32(Session[gblValue.UserId].ToString());
            DataTable dt = null;
            CheckBox checkbox = (CheckBox)sender;
            CheckBox checkbox2 = (CheckBox)sender;
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            GridViewRow row1 = (GridViewRow)checkbox2.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            CheckBox chkCan = (CheckBox)row.FindControl("chkCan");

            TextBox txtCanReason = (TextBox)row.FindControl("txtCanReason");
            TextBox txtAddrProof = (TextBox)row.FindControl("txtAddrProof");
            TextBox txtIdProof = (TextBox)row.FindControl("txtIdProof");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                txtCanReason.Enabled = true;
                txtAddrProof.Enabled = true;
                txtIdProof.Enabled = true;
                if (row.Cells[10].Text == "Y")
                {
                    checkbox2.Checked = false;
                    gblFuction.AjxMsgPopup("Please remove Sanction before make Cancel.");
                    row.Cells[12].Text = "N";
                    dt.Rows[row.RowIndex]["Cancel"] = "N";
                    dt.AcceptChanges();
                    upSanc.Update();
                    return;
                }
                chkApp.Enabled = false;
                row.Cells[12].Text = "Y";
                dt.Rows[row.RowIndex]["Cancel"] = "Y";
                dt.Rows[row.RowIndex]["SanDate"] = "";
                dt.Rows[row.RowIndex]["CanReason"] = txtCanReason.Text.ToString();
                dt.Rows[row.RowIndex]["M_IdentyProfNo"] = txtIdProof.Text.ToString();
                dt.Rows[row.RowIndex]["M_AddProfNo"] = txtAddrProof.Text.ToString();
                dt.AcceptChanges();
            }
            else
            {
                if (User != 1)
                {
                    if (row.Cells[11].Text == "")
                    {
                        checkbox.Checked = true;
                        chkCan.Checked = true;
                        chkCan.Enabled = false;
                        gblFuction.AjxMsgPopup("Cannot Uncheck This..");
                        upSanc.Update();
                        return;
                    }
                }
                CApplication OAp = new CApplication();
                int count = OAp.chkSancAppRej(row.Cells[21].Text, "Cancel", row.Cells[15].Text);
                if (count > 0)
                {
                    gblFuction.AjxMsgPopup("You can not Edit this application because new application found against this member..!!");
                    chkApp.Checked = true;
                    return;
                }
                dt.Rows[row.RowIndex]["CanReason"] = "";
                dt.Rows[row.RowIndex]["M_IdentyProfNo"] = row.Cells[13].Text;
                dt.Rows[row.RowIndex]["M_AddProfNo"] = row.Cells[14].Text;
                txtAddrProof.Text = row.Cells[14].Text;
                txtAddrProof.Enabled = false;
                txtIdProof.Text = row.Cells[13].Text;
                txtIdProof.Enabled = false;
                txtCanReason.Text = "";
                txtCanReason.Enabled = false;

                chkApp.Enabled = true;
                row.Cells[10].Text = "N";
                dt.Rows[row.RowIndex]["Cancel"] = "N";
                dt.AcceptChanges();
            }
            dt.Rows[row.RowIndex]["SanDate"] = "";
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void gvSanc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bind Approval
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");

                    if (e.Row.Cells[10].Text == "Y")
                    {
                        chkApp.Checked = true;
                        chkApp.Enabled = false;
                        chkCan.Enabled = false;
                    }
                    else if (e.Row.Cells[10].Text == "N")
                    {
                        chkApp.Checked = false;
                        chkCan.Enabled = true;
                    }

                    //Bind Calcel

                    if (e.Row.Cells[12].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkCan.Enabled = false;
                        chkApp.Enabled = false;
                    }
                    else if (e.Row.Cells[12].Text == "N")
                    {
                        chkCan.Checked = false;
                        chkApp.Enabled = true;
                    }

                }
            }
            finally
            {

            }
        }

        protected void gvSanc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vMemId = "";
            string vBranchCode = "";
            DataTable dt = null;
            CApplication oApp = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    vBranchCode = gvRow.Cells[22].Text;
                    vMemId = Convert.ToString(e.CommandArgument);
                    if (vMemId.Length > 0)
                    {
                        oApp = new CApplication();
                        dt = oApp.GetLoanDtlByMemberID(vMemId, vBranchCode);
                        gvLnDtl.DataSource = dt;
                        gvLnDtl.DataBind();
                    }
                    else
                    {
                        gvLnDtl.DataSource = null;
                        gvLnDtl.DataBind();
                    }
                }
            }
            finally
            {
                dt = null;
                oApp = null;
            }
        }

        protected void txtIdProof_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtIdProof = (TextBox)gvRow.FindControl("txtIdProof");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["Sanc"];
            dt.Rows[gvRow.RowIndex]["M_IdentyProfNo"] = txtIdProof.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
        protected void txtAddrProof_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtAddrProof = (TextBox)gvRow.FindControl("txtAddrProof");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["Sanc"];
            dt.Rows[gvRow.RowIndex]["M_AddProfNo"] = txtAddrProof.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }
        protected void txtCanReason_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCanReason = (TextBox)gvRow.FindControl("txtCanReason");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["Sanc"];
            dt.Rows[gvRow.RowIndex]["CanReason"] = txtCanReason.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

    }
}