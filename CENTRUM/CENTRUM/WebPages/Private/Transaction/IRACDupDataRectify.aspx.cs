using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;
using System.IO;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class IRACDupDataRectify : CENTRUMBase
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
                txtAsOnDt.Text = Session[gblValue.LoginDate].ToString();

                LoadGrid("N", txtAsOnDt.Text, vBrCode, 1);

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
                this.PageHeading = "IRAC Duplicate Data Rectification";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuIRACDataRec);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "IRAC Duplicate Data Rectification", false);
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
                        LoadGrid("N", txtAsOnDt.Text, vBrCode, 1);
                    }
                    
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
            if (txtAsOnDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAsOnDt.Text) == false)
                {
                    gblFuction.MsgPopup(gblMarg.ValidDate);
                    gblFuction.focus("ctl00_cph_Main_txtAsOnDt");
                    vResult = false;
                }
            }
            if (txtAsOnDt.Text.Trim() != "" || txtAsOnDt.Text.Trim() == "")
            {
                if (gblFuction.IsDate(txtAsOnDt.Text) == false)
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
            string vXmlData = "", ValidationCheck = "";
            //DateTime vSanDt = gblFuction.setDate(txtExDisbDt.Text);
            DateTime vSanDt = gblFuction.setDate(txtAppDt.Text);
            string vActMstTbl = Session[gblValue.ACVouMst].ToString(),
               vActDtlTbl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            Int32 vCreatedBy = 0;
            if (gvSanc.Rows.Count == 0)
            {
                gblFuction.AjxMsgPopup("No Records to Update.");
                return;
            }
            if (ValidDate() == true)
            {
                try
                {
                    dt = (DataTable)ViewState["IRACData"];
                    if (dt == null) return;
                    if (ValidateFields() == false) return;

                    

                    string vBrCode = Session[gblValue.BrnchCode].ToString();
                    oApp = new CApplication();
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlData = oSW.ToString();
                    }

                    ValidationCheck = oApp.ChkBeforeIRACDupDataRectify(vXmlData, vSanDt);

                    if (ValidationCheck.ToString().Trim() != "")
                    {
                        gblFuction.AjxMsgPopup(ValidationCheck);
                        return;
                    }
                    else
                    {
                        //-----------XML Save----------
                        vCreatedBy = Convert.ToInt32(Session[gblValue.UserId]);
                        vErr = oApp.UpdateIRACDupDataRectify(vXmlData, vCreatedBy, vSanDt, vActMstTbl, vActDtlTbl, vFinYear);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.SaveMsg);
                            LoadGrid("A", txtAsOnDt.Text, vBrCode, 1);
                            rdbOpt.SelectedValue = "A";
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                        }
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
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            Boolean vResult = true;

            if (vFinFrom > gblFuction.setDate(txtAppDt.Text) || gblFuction.setDate(txtAppDt.Text) > vFinTo)
            {
                gblFuction.MsgPopup("Date should be within this financial year.");
                vResult=false;
            }

            return vResult;
        }

        private void LoadGrid(string pAppMode, string pAsOnDt, string pBranch, Int32 pPgIndx)
        {
            DataTable dt = null;
            CApplication oLS = null;
            Int32 vRows = 0;
            
            //  Int32.TryParse(ddlLnProd.SelectedValue, out vLoanProduct);

            try
            {
                string vBrCode = pBranch;
                DateTime vAsOnDt = gblFuction.setDate(pAsOnDt);
                oLS = new CApplication();
                dt = oLS.GetIRACUnApprovedDupDataList(vAsOnDt, pAppMode, pPgIndx, ref vRows);
                
                ViewState["IRACData"] = dt;
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

            LoadGrid(rdbOpt.SelectedValue, txtAsOnDt.Text, vBrCode, cPgNo);
            //tabLoanAppl.ActiveTabIndex = 0;
        }

        protected void chkApp_CheckedChanged(object sender, EventArgs e)
        {
            string vBranch = "";
            DataTable dt = null;
            Int32 vDisbDateCount = Convert.ToInt32(ViewState["DCount"]);
            try
            {
                dt = (DataTable)ViewState["IRACData"];

                vBranch = Session[gblValue.BrnchCode].ToString();
                CheckBox checkbox = (CheckBox)sender;
                GridViewRow row = (GridViewRow)checkbox.NamingContainer;
                CheckBox chkApp = (CheckBox)row.FindControl("chkApp");
                TextBox txtCUSTOMER_NAME = (TextBox)row.FindControl("txtCUSTOMER_NAME");
                TextBox txtPREVASSETCLASS = (TextBox)row.FindControl("txtPREVASSETCLASS");
                TextBox txtCURRASSETCLASS = (TextBox)row.FindControl("txtCURRASSETCLASS");
                TextBox txtPREVPROVAMT = (TextBox)row.FindControl("txtPREVPROVAMT");
                TextBox txtCURRPROVAMT = (TextBox)row.FindControl("txtCURRPROVAMT");
                TextBox txtPREV_INT_ACCRUED_AMT = (TextBox)row.FindControl("txtPREV_INT_ACCRUED_AMT");
                TextBox txtCURR_INT_ACCRUED_AMT = (TextBox)row.FindControl("txtCURR_INT_ACCRUED_AMT");

                
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
                
                

                if (chkApp.Checked)
                {
                    if (txtCUSTOMER_NAME.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Customer name should not be blank...");
                        chkApp.Checked = false;
                    }
                    else if (txtPREVASSETCLASS.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Previous asset class should not be blank...");
                        chkApp.Checked = false;
                    }
                    else if (txtCURRASSETCLASS.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Current asset class should not be blank...");
                        chkApp.Checked = false;
                    }
                    else if (txtPREVPROVAMT.Text.Trim() == "0" || txtPREVPROVAMT.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Previous Provitioning Amount should not be blank or zero...");
                        chkApp.Checked = false;
                    }
                    else if (txtCURRPROVAMT.Text.Trim() == "0" || txtCURRPROVAMT.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Current Provitioning Amount should not be blank or zero...");
                        chkApp.Checked = false;
                    }
                    else if (txtPREV_INT_ACCRUED_AMT.Text.Trim() == "0" || txtPREV_INT_ACCRUED_AMT.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Previous Interest Accrued Amount should not be blank or zero...");
                        chkApp.Checked = false;
                    }
                    else if (txtCURR_INT_ACCRUED_AMT.Text.Trim() == "0" || txtCURR_INT_ACCRUED_AMT.Text.Trim() == "")
                    {
                        gblFuction.AjxMsgPopup("Current Interest Accrued Amount should not be blank or zero...");
                        chkApp.Checked = false;
                    }
                    else
                    {
                        row.Cells[19].Text = "Y";
                        dt.Rows[row.RowIndex]["Approved"] = "Y";
                        
                    }
                }
                

                dt.AcceptChanges();
                ViewState["IRACData"] = dt;
                upSanc.Update();
            }
            finally
            {
                dt = null;
            }
        }

        

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }





        protected void txtCUSTOMER_NAME_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCUSTOMER_NAME = (TextBox)gvRow.FindControl("txtCUSTOMER_NAME");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CUSTOMER_NAME"] = txtCUSTOMER_NAME.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtPREVASSETCLASS_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtPREVASSETCLASS = (TextBox)gvRow.FindControl("txtPREVASSETCLASS");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["PREV_ASSET_CLASS"] = txtPREVASSETCLASS.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtCURRASSETCLASS_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCURRASSETCLASS = (TextBox)gvRow.FindControl("txtCURRASSETCLASS");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CURR_ASSET_CLASS"] = txtCURRASSETCLASS.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtPREVNPADATE_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtPREVNPADATE = (TextBox)gvRow.FindControl("txtPREVNPADATE");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["PREV_NPA_DATE"] = txtPREVNPADATE.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtCURRNPADATE_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCURRNPADATE = (TextBox)gvRow.FindControl("txtCURRNPADATE");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CURR_NPA_DATE"] = txtCURRNPADATE.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtNPATAGDATE_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtNPATAGDATE = (TextBox)gvRow.FindControl("txtNPATAGDATE");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["NPA_TAG_DATE"] = txtNPATAGDATE.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtPREVPROVAMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtPREVPROVAMT = (TextBox)gvRow.FindControl("txtPREVPROVAMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["PREV_PROV_AMT"] = txtPREVPROVAMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtCURRPROVAMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCURRPROVAMT = (TextBox)gvRow.FindControl("txtCURRPROVAMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CURR_PROV_AMT"] = txtCURRPROVAMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtPREV_INT_OS_AMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtPREV_INT_OS_AMT = (TextBox)gvRow.FindControl("txtPREV_INT_OS_AMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["PREV_INT_OS_AMT"] = txtPREV_INT_OS_AMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtCURR_INT_OS_AMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCURR_INT_OS_AMT = (TextBox)gvRow.FindControl("txtCURR_INT_OS_AMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CURR_INT_OS_AMT"] = txtCURR_INT_OS_AMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtPREV_INT_ACCRUED_AMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtPREV_INT_ACCRUED_AMT = (TextBox)gvRow.FindControl("txtPREV_INT_ACCRUED_AMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["PREV_INT_ACCRUED_AMT"] = txtPREV_INT_ACCRUED_AMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtCURR_INT_ACCRUED_AMT_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtCURR_INT_ACCRUED_AMT = (TextBox)gvRow.FindControl("txtCURR_INT_ACCRUED_AMT");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["CURR_INT_ACCRUED_AMT"] = txtCURR_INT_ACCRUED_AMT.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtSMA_FLAG_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtSMA_FLAG = (TextBox)gvRow.FindControl("txtSMA_FLAG");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["SMA_FLAG"] = txtSMA_FLAG.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
        protected void txtMAX_DPD_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            TextBox txtBox = (TextBox)sender;
            GridViewRow gvRow = (GridViewRow)txtBox.NamingContainer;
            TextBox txtMAX_DPD = (TextBox)gvRow.FindControl("txtMAX_DPD");
            CApplication oApp = new CApplication();
            dt = (DataTable)ViewState["IRACData"];
            dt.Rows[gvRow.RowIndex]["MAX_DPD"] = txtMAX_DPD.Text.Trim();
            dt.AcceptChanges();
            ViewState["IRACData"] = dt;
            upSanc.Update();
        }
    }
}