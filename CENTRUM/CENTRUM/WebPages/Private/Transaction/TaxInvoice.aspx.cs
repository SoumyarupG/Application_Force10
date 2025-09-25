using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class TaxInvoice : CENTRUMBase
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                    StatusButton("View");
                else
                    StatusButton("Exit");
                LoadGrid(0);
                txtInvoiceDt.Text = Session[gblValue.LoginDate].ToString();
                popState();
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
                this.PageHeading = "Tax Invoice";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuTaxInvoice);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Tax Invoice", false);
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
        /// <param name="pMode"></param>
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                   
                    //gblFuction.focus("ctl00_cph_Main_tabGp_pnlDtl_txtBlock");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabGp_pnlDtl_txtBlock");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CTaxInvoice oTax = null;
            Int32 vRows = 0;
            try
            {
                oTax = new CTaxInvoice();
                dt = oTax.GetTaxInvoicePG(pPgIndx, txtSearch.Text, ref vRows);
                gvTaxInvoiceList.DataSource = dt.DefaultView;
                gvTaxInvoiceList.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotPg.Text = "0";
                    lblCrPg.Text = "0";
                }
                else
                {
                    lblTotPg.Text = CalTotPgs(vRows).ToString();
                    lblCrPg.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 0 && cPgNo != Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (cPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                dt = null;
                oTax = null;
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }


        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlSupplyPlace.DataSource = dt;
                ddlSupplyPlace.DataTextField = "StateName";
                ddlSupplyPlace.DataValueField = "StateId";
                ddlSupplyPlace.DataBind();

                ddlRState.DataSource = dt;
                ddlRState.DataTextField = "StateName";
                ddlRState.DataValueField = "StateId";
                ddlRState.DataBind();

                ddlSState.DataSource = dt;
                ddlSState.DataTextField = "StateName";
                ddlSState.DataValueField = "StateId";
                ddlSState.DataBind();

                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSupplyPlace.Items.Insert(0, oli);
                ddlRState.Items.Insert(0, oli);
                ddlSState.Items.Insert(0, oli);

            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvTaxInvoiceList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vTaxId = 0, vRow = 0;
            DataTable dt = null;
            CTaxInvoice oTax = null;
            try
            {
                vTaxId = Convert.ToInt32(e.CommandArgument);
                ViewState["vTaxId"] = vTaxId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvTaxInvoiceList.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oTax = new CTaxInvoice();
                    dt = oTax.GetTaxInvoiceDetailsById(vTaxId);
                    if (dt.Rows.Count > 0)
                    {

                        txtInvoiceNo.Text = Convert.ToString(dt.Rows[vRow]["InvoiceNo"]).Trim();
                        txtInvoiceDt.Text =  Convert.ToString(dt.Rows[vRow]["InvoiceDate"]).Trim();
                        ddlSupplyPlace.SelectedValue  = Convert.ToString(dt.Rows[vRow]["InvoiceState"]).Trim();

                        txtRName.Text = Convert.ToString(dt.Rows[vRow]["RecieverName"]);
                        txtRAddress.Text = Convert.ToString(dt.Rows[vRow]["RecieverAddress"]);
                        txtRetailerGstin.Text = Convert.ToString(dt.Rows[vRow]["RecieverGstin"]).Trim();
                        ddlRState.SelectedValue = Convert.ToString(dt.Rows[vRow]["RecieverState"]).Trim();

                        txtSAddress.Text = Convert.ToString(dt.Rows[vRow]["SupplierAddress"]);
                        txtSGstin.Text = Convert.ToString(dt.Rows[vRow]["SupplierGstin"]).Trim();
                        ddlSState.SelectedValue = Convert.ToString(dt.Rows[vRow]["SupplierState"]).Trim();

                        txtPremiumCollected.Text = Convert.ToString(dt.Rows[vRow]["PremiumCollected"]) ;
                        txtROI.Text = Convert.ToString(dt.Rows[vRow]["Roi"]) ;
                        txtProductDesc.Text = Convert.ToString(dt.Rows[vRow]["ProductDesc"]).Trim();

                        if (Convert.ToString(dt.Rows[0]["NotEditableYN"]) == "Y")
                            chkNotEditableYN.Checked = true;
                        else
                            chkNotEditableYN.Checked = false;

                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbOcup.ActiveTabIndex = 1;
                        if (Session[gblValue.BrnchCode].ToString() == "0000")
                            StatusButton("Show");
                        else
                            StatusButton("Exit");
                    }
                }
            }
            finally
            {
                dt = null;
                oTax = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vTaxId = Convert.ToInt32(ViewState["vTaxId"]);
            string invoiceNo = "", vNotEditableYN= "N";
            DateTime vInvoiceDt = gblFuction.setDate(txtInvoiceDt.Text);
            Int32 vErr = 0, vRec = 0, vBnkId = 0,vYrNo =Convert.ToInt32(Session[gblValue.FinYrNo]);
            
            CTaxInvoice oTax = null;
            try
            {
                if (chkNotEditableYN.Checked == true) vNotEditableYN = "Y";

                if (Mode == "Save")
                {
                    oTax = new CTaxInvoice();
                    vErr = oTax.InsertUpdateInvoice(ref vTaxId,  ref invoiceNo, vInvoiceDt,vYrNo,Convert.ToInt32(ddlSupplyPlace.SelectedValue),txtRName.Text,txtRAddress.Text,txtRetailerGstin.Text
                        ,Convert.ToInt32(ddlRState.SelectedValue),txtSAddress.Text,txtSGstin.Text,Convert.ToInt32(ddlSState.SelectedValue),Convert.ToDouble(txtPremiumCollected.Text),Convert.ToDouble(txtROI.Text)
                        , this.UserID, "Save", txtProductDesc.Text.Trim(), vNotEditableYN);
                    if (vErr > 0)
                    {
                        ViewState["vTaxId"] = vTaxId;
                        txtInvoiceNo.Text = invoiceNo;
                        gblFuction.MsgPopup(gblMarg.SaveMsg);
                        vResult = true;                        
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oTax = new CTaxInvoice();
                    vErr = oTax.InsertUpdateInvoice(ref vTaxId,ref   invoiceNo, vInvoiceDt, vYrNo, Convert.ToInt32(ddlSupplyPlace.SelectedValue), txtRName.Text, txtRAddress.Text, txtRetailerGstin.Text
                        , Convert.ToInt32(ddlRState.SelectedValue), txtSAddress.Text, txtSGstin.Text, Convert.ToInt32(ddlSState.SelectedValue), Convert.ToDouble(txtPremiumCollected.Text), Convert.ToDouble(txtROI.Text)
                        , this.UserID, "Edit", txtProductDesc.Text.Trim(), vNotEditableYN); 
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                        oTax = new CTaxInvoice();
                        vErr = oTax.InsertUpdateInvoice(ref vTaxId, ref  invoiceNo, vInvoiceDt, vYrNo, Convert.ToInt32(ddlSupplyPlace.SelectedValue), txtRName.Text, txtRAddress.Text, txtRetailerGstin.Text
                        , Convert.ToInt32(ddlRState.SelectedValue), txtSAddress.Text, txtSGstin.Text, Convert.ToInt32(ddlSState.SelectedValue), Convert.ToDouble(txtPremiumCollected.Text), Convert.ToDouble(txtROI.Text)
                        , this.UserID, "Delete", txtProductDesc.Text.Trim(), vNotEditableYN); 
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                }
                return vResult;
            }
            finally
            {
                oTax = null;
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
            ViewState["PurposeId"] = null;
        }

        protected void ImgBtnPrint_Click(object sender, EventArgs e)
        {

            ImageButton ImgBtnVerify = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)ImgBtnVerify.NamingContainer;
            LinkButton btnShow = (LinkButton)gR.FindControl("btnShow");
            
            string vRptPath = "";
            DataTable dt = null;
            CReports oRpt = null;

            vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\TaxInvoice.rpt";
            

            try
            {
                oRpt = new CReports();
                dt = oRpt.rptTaxInvoice(Convert.ToInt32(btnShow.CommandArgument));
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_TaxInvoice");
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Data not found");
                }
            }
            finally
            {
                dt = null;
                oRpt = null;
            }
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
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                LoadGrid(0);
                ViewState["StateEdit"] = "Add";
                tbOcup.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tbOcup.ActiveTabIndex = 0;
                    StatusButton("Delete");
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
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                if (chkNotEditableYN.Checked == true)
                {
                    gblFuction.MsgPopup("Edit not possible..");
                    return;
                }

                ViewState["StateEdit"] = "Edit";
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
            tbOcup.ActiveTabIndex = 0;
            EnableControl(false);
            ClearControls();
            StatusButton("View");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(0);
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlSupplyPlace.Enabled = Status;
            txtRName.Enabled = Status;
            txtRAddress.Enabled = Status;
            txtRetailerGstin.Enabled = Status;
            ddlRState.Enabled = Status;
            txtSAddress.Enabled = Status;
            txtSGstin.Enabled = Status;
            ddlSState.Enabled = Status;
            txtPremiumCollected.Enabled = Status; ;
            txtROI.Enabled = Status;
            txtProductDesc.Enabled = Status;
            chkNotEditableYN.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            //txtInvoiceNo.Text = "";
            //txtInvoiceDt.Text = "";
            ddlSupplyPlace.SelectedValue = "-1";
            txtRName.Text = "";
            txtRAddress.Text = "";
            txtRetailerGstin.Text = "";
            ddlRState.SelectedValue = "-1";
            txtSAddress.Text = "";
            txtSGstin.Text = "";
            ddlSState.SelectedValue = "-1";
            txtPremiumCollected.Text = "0";
            txtROI.Text = "0";
            txtProductDesc.Text = "";
            chkNotEditableYN.Checked = false;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1; //lblCrPg
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1; //lblTotPg
                    break;
            }
            LoadGrid(cPgNo);
            tbOcup.ActiveTabIndex = 0;
        }
    }
}