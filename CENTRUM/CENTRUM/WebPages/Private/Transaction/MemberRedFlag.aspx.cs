using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;


namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class MemberRedFlag : CENTRUMBase
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
                // txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                ViewState["Mode"] = "N";

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
                this.PageHeading = "Member Red Flag";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuRedFlag);
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
                    if (rdbOpt.SelectedValue == "N")
                    {
                        LoadGrid("N", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    }
                    else if (rdbOpt.SelectedValue == "R")
                    {
                        LoadGrid("R", txtFrmDt.Text, txtToDt.Text, vBrCode, 1);
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                ViewState["Mode"] = rdbOpt.SelectedValue;
                rdbOpt.Enabled = false;
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
            //if (txtAppDt.Text.Trim() != "" || txtAppDt.Text.Trim() == "")
            //{
            //    if (gblFuction.IsDate(txtAppDt.Text) == false)
            //    {
            //        gblFuction.MsgPopup(gblMarg.ValidDate);
            //        gblFuction.focus("ctl00_cph_Main_txtSancDt");
            //        vResult = false;
            //    }
            //}
            return vResult;
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            CApplication oApp = null;
            DataTable dt = new DataTable();
            Int32 vErr = 0;
            string vXmlData = "";
            DateTime vSanDt = gblFuction.setDate(txtFrmDt.Text);
            if (gvSanc.Rows.Count == 0)
            {
                gblFuction.AjxMsgPopup("No Records to Update.");
                return;
            }
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["Sanc"];
                    if (dt == null) return;
                    if (ValidateFields() == false) return;

                    foreach (GridViewRow gr in gvSanc.Rows)
                    {
                        CheckBox chkCan = (CheckBox)gr.FindControl("chkCan");
                        TextBox txtCanReason = (TextBox)gr.FindControl("txtCanReason");
                        if (chkCan.Checked == true && txtCanReason.Text == "")
                        {
                            gblFuction.AjxMsgPopup("Reject Reason Cannot be left blank.");
                            txtCanReason.Focus();
                            return;
                        }
                    }

                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }
                    string vMode = ViewState["Mode"].ToString();
                    //-----------XML Save----------
                    vErr = oApp.UpdatetRedFlag(vXmlData, this.UserID, vBrCode, "E", 0, vSanDt, vMode);
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
                dt = oLS.GetRedFlag(vFromDt, vToDt, pAppMode, vBrCode, vLoanProduct, pPgIndx, ref vRows, txtSearch.Text, Convert.ToInt32(Session[gblValue.UserId]));
                if (Convert.ToString(Session[gblValue.ViewAAdhar]) == "N")
                {
                    foreach (DataRow dr in dt.Rows) // search whole table
                    {
                        if (Convert.ToInt32(dr["M_AddProfId"].ToString()) == 1)
                        {
                            dr["M_AddProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_AddProfNo"]).Length - 4, 4)); ; //change the name                           
                        }
                        if (Convert.ToInt32(dr["M_IdentyPRofId"].ToString()) == 1)
                        {
                            dr["M_IdentyProfNo"] = String.Format("{0}{1}", "********", Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Substring(Convert.ToString(dt.Rows[0]["M_IdentyProfNo"]).Length - 4, 4)); ; //change the name                           
                        }
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

                if (chkApp.Checked)
                {
                    chkCan.Checked = false;
                    chkCan.Enabled = true;
                    row.Cells[5].Text = "Y";
                    dt.Rows[row.RowIndex]["Untag"] = "Y";
                    row.Cells[7].Text = "N";
                    dt.Rows[row.RowIndex]["Reject"] = "N";
                    chkCan.Enabled = false;
                }
                else
                {
                    chkCan.Enabled = true;
                    row.Cells[5].Text = "N";
                    dt.Rows[row.RowIndex]["Untag"] = "N";
                    row.Cells[7].Text = "N";
                    dt.Rows[row.RowIndex]["Reject"] = "Y";
                    if (rdbOpt.SelectedValue == "N")
                        chkCan.Enabled = true;
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
            GridViewRow row = (GridViewRow)checkbox.NamingContainer;
            CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
            CheckBox chkCan = (CheckBox)row.FindControl("chkCan");
            TextBox txtCanReason = (TextBox)row.FindControl("txtCanReason");

            dt = (DataTable)ViewState["Sanc"];
            if (checkbox.Checked == true)
            {
                txtCanReason.Enabled = true;
                chkApp.Checked = false;
                chkApp.Enabled = false;
                row.Cells[7].Text = "Y";
                dt.Rows[row.RowIndex]["Reject"] = "Y";
                dt.Rows[row.RowIndex]["Untag"] = "N";
                row.Cells[5].Text = "N";
                upSanc.Update();
                dt.AcceptChanges();
            }
            else
            {
                chkApp.Enabled = true;
                txtCanReason.Text = "";
                row.Cells[7].Text = "N";
                dt.Rows[row.RowIndex]["Reject"] = "N";
                dt.Rows[row.RowIndex]["Untag"] = "N";
                row.Cells[5].Text = "N";

                txtCanReason.Enabled = false;
                if (rdbOpt.SelectedValue == "R")
                    chkApp.Enabled = true;
            }
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
                    CheckBox chkApp = (CheckBox)e.Row.FindControl("chkApp");
                    CheckBox chkCan = (CheckBox)e.Row.FindControl("chkCan");
                    //if (rdbOpt.SelectedValue == "N")
                    //{
                    //    chkApp.Enabled = false;
                    //    chkCan.Enabled = true;
                    //}
                    //else
                    //{
                    //    chkApp.Enabled = true;
                    //    chkCan.Enabled = false;
                    //}
                    if (e.Row.Cells[5].Text == "Y" && e.Row.Cells[7].Text == "Y")
                    {
                        chkApp.Checked = true;
                        chkCan.Enabled = false;
                    }
                    else if (e.Row.Cells[5].Text == "N" && e.Row.Cells[7].Text == "Y")
                    {
                        chkCan.Checked = true;
                        chkApp.Enabled = false;
                    }  
                    else if (e.Row.Cells[5].Text == "N" && e.Row.Cells[7].Text == "N")
                    {                        
                        chkApp.Enabled = false;
                    }                    
                }
            }
            finally
            {

            }
        }

        protected void txtCanReason_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCanReason = (TextBox)gvRow.FindControl("txtCanReason");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["Sanc"];
            dt.Rows[gvRow.RowIndex]["RejReason"] = txtCanReason.Text;
            dt.AcceptChanges();
            ViewState["Sanc"] = dt;
            upSanc.Update();
        }

    }
}