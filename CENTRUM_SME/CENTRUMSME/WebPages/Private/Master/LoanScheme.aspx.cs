using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class LoanScheme : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                LoadGrid(0);
                popLoanProduct();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);
                
                this.Menu = false;
                this.PageHeading = "Loan Scheme Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuScheme);
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Scheme Master", false);
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
                    ClearControls();
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
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
                    gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
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
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
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
            txtLnScheme.Text="";
            txtSrtNm.Text="";
            ddlLnProd.SelectedIndex=-1;
            txtIntroDt.Text=System.DateTime.Now.ToString("dd/MM/yyyy");
            chkActv.Checked = false;
            txtLnAmt.Text="0.0";
            lblUser.Text = "";
            lblDate.Text = "";
            txtminLnAmt.Text = "0.0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(bool Status)
        { 
            txtLnScheme.Enabled=Status;
            txtSrtNm.Enabled=Status;
            ddlLnProd.Enabled=Status;
            txtIntroDt.Enabled=Status;
            chkActv.Enabled=Status;
            txtLnAmt.Enabled = Status;
            txtminLnAmt.Enabled = Status;
            gvBranch.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void popLoanProduct()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "ProductId", "ProductName", "ProductMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlLnProd.DataSource = dt;
                ddlLnProd.DataTextField = "ProductName";
                ddlLnProd.DataValueField = "ProductId";
                ddlLnProd.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLnProd.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private DataTable TabletoXml()
        {
            DataTable dt = new DataTable("Alloc");
            dt.Columns.Add("BranchCode", typeof(string));
            foreach (GridViewRow gr in gvBranch.Rows)
            {
                CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                Label lblbranchcode = (Label)gr.FindControl("lblBranchCode");
                string branchcode = lblbranchcode.Text;
                if (chkStatus.Checked == true)
                    dt.Rows.Add(branchcode);
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pMode"></param>
        private void LoadBranch(string pMode, Int32 vLnTypId)
        {
            DataTable dt = null;
            CLoanScheme oLS = null;
            try
            {
                oLS = new CLoanScheme();
                dt = oLS.GetBranchForScheme(pMode, vLnTypId);
                gvBranch.DataSource = dt;
                gvBranch.DataBind();
                foreach (GridViewRow gr in gvBranch.Rows)
                {
                    CheckBox chkStatus = (CheckBox)gr.FindControl("chkStatus");
                    if (dt.Rows[gr.RowIndex]["Actv"].ToString() == "1")
                        chkStatus.Checked = true;
                    else
                        chkStatus.Checked = false;
                }
            }
            finally
            {
                oLS = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CLoanScheme oIC = null;
            Int32 vRows = 0;
            try
            {
                oIC = new CLoanScheme();
                dt = oIC.GetLoanSchemePG(pPgIndx, ref vRows, txtSearch.Text.ToString());
                gvLnScheme.DataSource = dt.DefaultView;
                gvLnScheme.DataBind();
                if (dt.Rows.Count <= 0)
                {
                    lblTotalPages.Text = "0";
                    lblCurrentPage.Text = "0";
                }
                else
                {
                    lblTotalPages.Text = CalTotPgs(vRows).ToString();
                    lblCurrentPage.Text = cPgNo.ToString();
                }
                if (cPgNo == 1)
                {
                    Btn_Previous.Enabled = false;
                    if (Int32.Parse(lblTotalPages.Text) > 0 && cPgNo != Int32.Parse(lblTotalPages.Text))
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
                oIC = null;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1; //lblCurrentPage
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1; //lblTotalPages
                    break;
                case "GoTo":
                    if (Int32.Parse(txtGotoPg.Text) <= Int32.Parse(lblTotalPages.Text))
                        cPgNo = Int32.Parse(txtGotoPg.Text);
                    break;
            }
            LoadGrid(cPgNo);
            tabLnScheme.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvLnScheme_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vLoanTypeId = 0;
            DataTable dt = null;
            CLoanScheme oLS = null;
            try
            {
                vLoanTypeId = Convert.ToInt32(e.CommandArgument);
                ViewState["LoanTypeId"] = vLoanTypeId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvLnScheme.Rows)
                    {
                        if ((gr.RowIndex) % 2 == 0)
                        {
                            gr.BackColor = backColor;
                            gr.ForeColor = foreColor;
                        }
                        else
                        {
                            gr.BackColor = System.Drawing.Color.White;
                            gr.ForeColor = foreColor;
                        }
                        gr.Font.Bold = false;
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                        lb.Font.Bold = false;
                    }
                    gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                    gvRow.ForeColor = System.Drawing.Color.White;
                    gvRow.Font.Bold = true;
                    btnShow.ForeColor = System.Drawing.Color.White;
                    btnShow.Font.Bold = true;
                    
                    oLS = new CLoanScheme();
                    dt = oLS.GetLoanSchemeById(vLoanTypeId);
                    if (dt.Rows.Count > 0)
                    {
                        txtLnScheme.Text = Convert.ToString(dt.Rows[0]["LoanTypeName"]).Trim();
                        txtSrtNm.Text = Convert.ToString(dt.Rows[0]["ShotName"]).Trim();
                        ddlLnProd.SelectedIndex = ddlLnProd.Items.IndexOf(ddlLnProd.Items.FindByValue(dt.Rows[0]["ProductId"].ToString().Trim()));
                        txtIntroDt.Text = Convert.ToString(dt.Rows[0]["IntroDate"]).Trim();
                        if (dt.Rows[0]["ActiveYN"].ToString().Trim() == "Y")
                            chkActv.Checked = true;
                        else
                            chkActv.Checked = false;
                        txtLnAmt.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]).Trim();
                        txtminLnAmt.Text = Convert.ToString(dt.Rows[0]["MinLoanAmt"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        LoadBranch("Edit", vLoanTypeId);
                        tabLnScheme.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool ValidateField()
        {
            bool vRes = true;
            if (gblFuction.IsDate(txtIntroDt.Text) == false)
            {
                gblFuction.MsgPopup("Introduce Date should be in DD/MM/YYYY Format");
                gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtIntroDt");
                return vRes = false;
            }
          

            return vRes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            DataTable dtXml = TabletoXml();
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["LoanTypeId"]), vActv = "", vXml = "";
            Int32 vErr = 0, vRec = 0, vLoanTypeId = 0;
            double vLnAmt=0,vMinLnAmt=0;
            CLoanScheme oLS = null;
            CGblIdGenerator oGbl = null;
            try
            {
                if (ValidateField() == false)
                    return false;
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                if (txtminLnAmt.Text.Trim() != "")
                    vMinLnAmt = Convert.ToDouble(txtminLnAmt.Text.Trim());
                else
                    vMinLnAmt = 0.0;
                if (txtLnAmt.Text.Trim() != "")
                    vLnAmt = Convert.ToDouble(txtLnAmt.Text.Trim());
                else
                    vLnAmt = 0.0;

                if (chkActv.Checked == true)
                    vActv = "Y";
                else
                    vActv = "N";

                vLoanTypeId = Convert.ToInt32(ViewState["LoanTypeId"]);
                if (Mode == "Save")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanTypeMst", "LoanTypeName", txtLnScheme.Text.Replace("'", "''"), "", "", "LoanTypeId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Loan Scheme Can not be Duplicate...");
                        return false;
                    }
                    vErr = oLS.InsertLoanScheme(ref vLoanTypeId, txtLnScheme.Text.Replace("'", "''"),Convert.ToInt32(ddlLnProd.SelectedValue),
                                            txtSrtNm.Text.Replace("'", "''"), gblFuction.setDate(txtIntroDt.Text), vActv, vLnAmt, vMinLnAmt,this.UserID, vXml);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["LoanTypeId"] = vLoanTypeId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oLS = new CLoanScheme();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanTypeMst", "LoanTypeName", txtLnScheme.Text.Replace("'", "''"), "", "", "LoanTypeId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Loan Scheme Can not be Duplicate...");
                        return false;
                    }
                    vErr = oLS.UpdateLoanScheme(vLoanTypeId, txtLnScheme.Text.Replace("'", "''"), Convert.ToInt32(ddlLnProd.SelectedValue),
                                            txtSrtNm.Text.Replace("'", "''"), gblFuction.setDate(txtIntroDt.Text), vActv, vLnAmt,vMinLnAmt, this.UserID, vXml);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oGbl = new CGblIdGenerator();
                  //  vRec = oGbl.ChkDelete(vLoanTypeId, "LoanTypeId", "LoanApplication");
                    if (vRec <= 0)
                    {
                        oLS = new CLoanScheme();
                        oLS.DeleteLoanScheme(vLoanTypeId);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oLS = null;
                oGbl = null;
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
            ViewState["LoanTypeId"] = null;
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
                ViewState["StateEdit"] = "Add";
                StatusButton("Add");
                LoadBranch("Add", 0);
                ClearControls();
                tabLnScheme.ActiveTabIndex = 1;
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    ClearControls();
                    StatusButton("Delete");
                    LoadGrid(0);
                    tabLnScheme.ActiveTabIndex = 0;
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
            EnableControl(false);
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
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                StatusButton("Show");
                LoadGrid(0);
                ViewState["StateEdit"] = null;
            }
        }


        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadGrid(0);
        }
    }
}
