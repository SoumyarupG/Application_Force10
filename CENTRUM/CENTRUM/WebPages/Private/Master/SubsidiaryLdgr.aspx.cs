using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class SubsidiaryLdgr : CENTRUMBase 
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                //Permission("btnAdd", "btnEdit", "btnDelete", "btnCancel", "btnSave", "Purpose Master");
                ViewState["StateEdit"] = null;
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    StatusButton("View");
                    PopBranchB();
                }
                else
                {
                    StatusButton("Exit");
                    PopBranch();
                }
                ViewState["StateEdit"] = null;
                PopAccHead();
                LoadGrid(1);
                tabSubs.ActiveTabIndex = 0;
            }
        }


        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Subsidiary Ledger";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuSubLed);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "AC Sub Group Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }


        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.CanAdd == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Add);
                //    return;
                //}
                ViewState["StateEdit"] = "Add";
                tabSubs.ActiveTabIndex = 1;
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                //    return;
                //}
                //if (this.CanDelete == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Del);
                //    return;
                //}
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(1);
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Edit Account Group...");
                //    return;
                //}
                //if (this.CanEdit == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Edit);
                //    return;
                //}
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
            tabSubs.ActiveTabIndex = 0;
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
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtAcGrp");
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
                    gblFuction.focus("ctl00_cph_Main_tabSGrp_pnlDtl_txtAcGrp");
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

        private void EnableControl(Boolean Status)
        {
            txtGenCode.Enabled = Status;
            txtSubsAc.Enabled = Status;
            ddlBranch.Enabled = Status;
            //ddlGenLed.Enabled = Status;
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtGenCode.Text = "";
            txtSubsAc.Text = "";
            // ddlGenLed.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
        }

        private void PopAccHead()
        {
            //DataTable dt = null;
            //CSubsidary oCb = null;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            //try
            //{
            //    oCb = new CSubsidary();
            //    ListItem liSel = new ListItem("<--- Select --->", "-1");
            //    //dt = oCb.PopComboMIS("S", "N", "AA", "DescId", "[Desc]", "ACGenLed", 1, "SubSiLedYN", "AA", System.DateTime.Now, "0000");
            //    dt = oCb.GetGenLedSub(vBrCode);
            //    ddlGenLed.DataSource = dt;
            //    ddlGenLed.DataTextField = "Desc";
            //    ddlGenLed.DataValueField = "DescId";
            //    ddlGenLed.DataBind();
            //    ddlGenLed.Items.Insert(0, liSel);
            //}
            //finally
            //{
            //    dt = null;
            //    oCb = null;
            //}
        }

        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 totalRows = 0;
            CSubsidary oSub = null;
            try
            {
                oSub = new CSubsidary();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oSub.GetAllSubsidary(pPgIndx, ref totalRows, vBrCode, "S"); // to Show
                gvSubs.DataSource = dt.DefaultView;
                gvSubs.DataBind();
                lblTotPg.Text = CalculateTotalPages(totalRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
                }
            }
            finally
            {
                oSub = null;
                dt.Dispose();
            }
        }

        private int CalculateTotalPages(double totalRows)
        {
            int totalPages = (int)Math.Ceiling(totalRows / gblValue.PgSize1);
            return totalPages;
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tabSubs.ActiveTabIndex = 0;
        }


        protected void gvSubs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string pSubId = "";
            DataTable dt = null;
            CSubsidary oSub = null;
            try
            {
                pSubId = Convert.ToString(e.CommandArgument);
                ViewState["SubsidiaryId"] = pSubId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvSubs.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oSub = new CSubsidary();
                    dt = oSub.GetSubsidaryList(pSubId);
                    if (dt.Rows.Count > 0)
                    {
                        txtGenCode.Text = Convert.ToString(dt.Rows[0]["SubsidiaryCode"]);
                        txtSubsAc.Text = Convert.ToString(dt.Rows[0]["SubsidiaryLed"]);
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        //ddlGenLed.SelectedIndex = ddlGenLed.Items.IndexOf(ddlGenLed.Items.FindByValue(dt.Rows[0]["DescId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabSubs.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt.Dispose();
                oSub = null;
            }
        }

        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<---ALL--->", "ALL");
                dt = oCb.PopComboMIS("S", "N", "AA", "BranchCode", "BranchName", "BranchMst", Convert.ToInt32(Session[gblValue.BrnchCode].ToString()), "BranchCode", "AA", System.DateTime.Now, Session[gblValue.BrnchCode].ToString());
                ddlBranch.DataSource = dt;
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, liSel);
            }
            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }

        private void PopBranchB()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<---ALL--->", "ALL");
                dt = oCb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", System.DateTime.Now, Session[gblValue.BrnchCode].ToString());
                ddlBranch.DataSource = dt;
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, liSel);
            }
            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["SubsidiaryId"]);
            string vNewSubId = "", vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vErr = 0, vRec = 0;
            CSubsidary oSub = null;
            try
            {
                //vDescId = ddlGenLed.SelectedValue.ToString();
                if (Mode == "Save")
                {
                    oSub = new CSubsidary();
                    vRec = oSub.ChkDupSubsidery(vSubId, txtSubsAc.Text.Replace("'", "''"), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Subsidiary Account Can not be Duplicate...");
                        return false;
                    }
                    //vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                    //    txtSubsAc.Text.Replace("'", "''"), vDescId, this.UserID, "I", 0, "Save");
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                        txtSubsAc.Text.Replace("'", "''"),
                        ddlBranch.SelectedValue, this.UserID, "I", 0, "Save");
                    if (vErr > 0)
                    {
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
                    oSub = new CSubsidary();
                    vRec = oSub.ChkDupSubsidery(vSubId, txtSubsAc.Text.Replace("'", "''"), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Subsidiary Account Can not be Duplicate...");
                        return false;
                    }
                    //vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                    //     txtSubsAc.Text.Replace("'", "''"), vDescId, this.UserID, "E", 0, "Edit");
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                       txtSubsAc.Text.Replace("'", "''"),
                        ddlBranch.SelectedValue, this.UserID, "E", 0, "Edit");
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
                    oSub = new CSubsidary();
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                        txtSubsAc.Text.Replace("'", "''"),
                        ddlBranch.SelectedValue, this.UserID, "D", 0, "Delet");//, vDescId
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oSub = null;
            }
        }
    }
}
