using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class PurposeMst : CENTRUMBase
    {
        protected int cPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                StatusButton("View");
                tbPurp.ActiveTabIndex = 0;               
                LoadGrid(0);
            }
        }
        
        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Loan Purpose";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnDelete.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Purpose Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        
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
            }
        }
        
        private void EnableControl(Boolean Status)
        {
            txtPurp.Enabled = Status;
            txtToDt.Enabled = false;
            chkActive.Enabled = Status;
        }

        private void ClearControls()
        {
            txtPurp.Text = "";
            txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            chkActive.Checked = false;
            lblDate.Text = "";
            lblUser.Text = "";
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CPurpose oFed = null;
            Int32 vRows = 0;
            try
            {
                oFed = new CPurpose();
                dt = oFed.GetPurposePG(pPgIndx, txtSearch.Text, ref vRows);
                gvPurp.DataSource = dt.DefaultView;
                gvPurp.DataBind();
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
                oFed = null;
            }
        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblTotPg.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tbPurp.ActiveTabIndex = 0;
        }

        protected void chkActive_CheckedChanged(object sender, EventArgs e)
        {
            if (chkActive.Checked == true)
            {
                txtToDt.Enabled = true;
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            else
            {
                txtToDt.Enabled = false;
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            }
            upRO.Update();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid(0);
        }

        protected void gvPurpose_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPurposeId = 0, vRow = 0;
            DataTable dt = null;
            CPurpose oPr= null;
            try
            {
                vPurposeId = Convert.ToInt32(e.CommandArgument);
                ViewState["PurposeId"] = vPurposeId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvPurp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oPr = new CPurpose();
                    dt = oPr.GetPurposeById(vPurposeId);
                    if (dt.Rows.Count > 0)
                    {
                        txtPurp.Text = Convert.ToString(dt.Rows[vRow]["Purpose"]).Trim();
                        txtToDt.Text = Convert.ToString(dt.Rows[vRow]["DeActiveDt"]).Trim();
                        if (dt.Rows[vRow]["DeActiveYN"].ToString() == "Y")
                        {
                            chkActive.Checked = true;
                        }
                        else chkActive.Checked = false;
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbPurp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oPr = null;
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["PurposeId"]);
            string vActive = "N";
            DateTime vActiveDt = gblFuction.setDate(txtToDt.Text);
            Int32 vErr = 0, vRec = 0, vPurpsId = 0;
            CPurpose oPr = null;
            CGblIdGenerator oGbl = null;

            try
            {               
                vPurpsId = Convert.ToInt32(ViewState["PurposeId"]);

                if (chkActive.Checked == true)
                {
                    vActive = "Y";
                }

                if (Mode == "Save")
                {
                    oPr = new CPurpose();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanPurposeMst", "Purpose", txtPurp.Text.Replace("'", "''"), "", "",
                                             "PurposeID", vSubId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Purpose of Loan Cannot be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertPurpose(ref vPurpsId, txtPurp.Text.Replace("'", "''"), this.UserID, "I", "Save",
                                             vActive, vActiveDt);
                    if (vErr > 0)
                    {
                        vResult = true;
                        ViewState["PurposeId"] = vPurpsId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oPr = new CPurpose();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("LoanPurposeMst", "Purpose", txtPurp.Text.Replace("'", "''"), "", "",
                                             "PurposeID", vSubId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Purpose of Loan Cannot be Duplicate...");
                        return false;
                    }
                    vErr = oPr.InsertPurpose(ref vPurpsId, txtPurp.Text.Replace("'", "''"), this.UserID, "E", "Edit",
                                             vActive, vActiveDt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.EditMsg);
                        vResult = true;
                        ViewState["PurposeId"] = vPurpsId;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oGbl = new CGblIdGenerator();
                    vRec = 0;                    
                    if (vRec <= 0)
                    {
                        oPr = new CPurpose();
                        vErr = oPr.InsertPurpose(ref vPurpsId, txtPurp.Text.Replace("'", "''"), this.UserID, "D", "Del",
                                                 vActive, vActiveDt);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblMarg.DeleteMsg);
                            vResult = true;
                            ViewState["PurposeId"] = vPurpsId;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblMarg.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        vResult = false;
                    }
                }
                return vResult;
            }            
            finally
            {
                oPr = null;
                oGbl = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
            ViewState["PurposeId"] = null;
        }

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
                tbPurp.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    tbPurp.ActiveTabIndex = 0;
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbPurp.ActiveTabIndex = 0;
            LoadGrid(0);
            EnableControl(false);
            StatusButton("View");
        }

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
    }
}