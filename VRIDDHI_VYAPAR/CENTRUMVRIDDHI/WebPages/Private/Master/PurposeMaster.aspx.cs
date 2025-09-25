using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;


namespace CENTRUM_VRIDDHIVYAPAR.WebPages.Private.Master
{
    public partial class PurposeMaster : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                ViewState["StateEdit"] = null;
                tabPurps.ActiveTabIndex = 0;
                popPurpose();
                LoadGrid(0);
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
                this.PageHeading = "Loan Sub-Purpose Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLonPurpose);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Sub-Purpose Master", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_ddlLoanSector");
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlLnPurpose.Enabled = Status;
            txtLoanPurpose.Enabled = Status;
        }
        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlLnPurpose.SelectedIndex = -1;
            txtLoanPurpose.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CPurpose oFed = null;
            Int32 vRows = 0;
            try
            {
                oFed = new CPurpose();
                dt = oFed.GetPurposePG(pPgIndx, ref vRows);
                gvPurpose.DataSource = dt.DefaultView;
                gvPurpose.DataBind();
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
                oFed = null;
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
            tabPurps.ActiveTabIndex = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPurposeId = 0, vRow = 0;
            DataTable dt = null;
            CPurpose oPr = null;
            try
            {
                vPurposeId = Convert.ToInt32(e.CommandArgument);
                ViewState["PurposeId"] = vPurposeId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvPurpose.Rows)
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

                    oPr = new CPurpose();
                    dt = oPr.GetPurposeById(vPurposeId);
                    if (dt.Rows.Count > 0)
                    {
                        ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(dt.Rows[0]["SectorId"].ToString().Trim()));
                        txtLoanPurpose.Text = Convert.ToString(dt.Rows[vRow]["PurposeName"]).Trim();
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tabPurps.ActiveTabIndex = 1;
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
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["PurposeId"]);
            int vSectorID = Convert.ToInt32(ddlLnPurpose.SelectedValue);
            Int32 vErr = 0, vRec = 0, vPurpsId = 0;
            CPurpose oPr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vPurpsId = Convert.ToInt32(ViewState["PurposeId"]);
                if (Mode == "Save")
                {
                    oPr = new CPurpose();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PurposeMst", "PurposeName", txtLoanPurpose.Text.Replace("'", "''"), "", "", "PurposeId", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Purpose Can not be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertPurpose(ref vPurpsId, txtLoanPurpose.Text.Replace("'", "''"), vSectorID, Convert.ToInt32(Session[gblValue.UserId]), "I", "Save");
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["PurposeId"] = vPurpsId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oPr = new CPurpose();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("PurposeMst", "PurposeName", txtLoanPurpose.Text.Replace("'", "''"), "", "", "PurposeId", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Purpose Can not be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertPurpose(ref vPurpsId, txtLoanPurpose.Text.Replace("'", "''"), vSectorID, Convert.ToInt32(Session[gblValue.UserId]), "E", "Edit");
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
                    vRec = oGbl.ChkDelete(vPurpsId, "PurposeId", "LoanApplication");
                    if (vRec <= 0)
                    {
                        oPr = new CPurpose();
                        vErr = oPr.InsertPurpose(ref vPurpsId, txtLoanPurpose.Text.Replace("'", "''"), vSectorID, Convert.ToInt32(Session[gblValue.UserId]), "D", "Del");
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
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
                oPr = null;
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
            ViewState["PurposeId"] = null;
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
                tabPurps.ActiveTabIndex = 1;
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
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid(0);
                    ClearControls();
                    tabPurps.ActiveTabIndex = 0;
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
            tabPurps.ActiveTabIndex = 0;
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
                LoadGrid(0);
                StatusButton("Show");
                ViewState["StateEdit"] = null;
            }
        }

        private void popPurpose()
        {
            DataTable dt = null;
            CPurpose oGb = null;
            try
            {
                oGb = new CPurpose();
                dt = oGb.PopJLGPurpose(gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate])));
                ddlLnPurpose.DataSource = dt;
                ddlLnPurpose.DataTextField = "Purpose";
                ddlLnPurpose.DataValueField = "PurposeID";
                ddlLnPurpose.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLnPurpose.Items.Insert(0, oli);
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
    }
}
