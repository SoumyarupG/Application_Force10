using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class AcGroup : CENTRUMBase
    {
        protected int currentPageNumber = 1;
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
                PopAccHead();
                LoadGrid(1);
                tabAcGr.ActiveTabIndex = 0;
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
                this.PageHeading = "Accounts Group";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAcctGroup);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "AC Group Master", false);
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
                    gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
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
                    gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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
                //if (this.CanAdd == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Add);
                //    return;
                //}
                ViewState["StateEdit"] = "Add";
                tabAcGr.ActiveTabIndex = 1;
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
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                    return;
                }
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    gblFuction.MsgPopup("Branch can not Edit Account Group...");
                    return;
                }
                //if (this.CanEdit == "N")
                //{
                //    gblFuction.MsgPopup(MsgAccess.Edit);
                //    return;
                //}
                ViewState["StateEdit"] = "Edit";
                gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
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
            tabAcGr.ActiveTabIndex = 0;
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
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
            txtAcGrp.Enabled = Status;
            ddlAcHd.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtAcGrp.Text = "";
            ddlAcHd.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopAccHead()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            oCb = new CGblIdGenerator();
            ListItem liSel = new ListItem();
            liSel.Text = "<--- Select --->";
            liSel.Value = "-1";
            dt = oCb.PopComboMIS("N", "N", "AA", "ACHeadId", "ACHead", "ACHead", 0, "AA", "AA", System.DateTime.Now, "0000");
            ddlAcHd.DataSource = dt;
            ddlAcHd.DataTextField = "ACHead";
            ddlAcHd.DataValueField = "ACHeadId";
            ddlAcHd.DataBind();
            ddlAcHd.Items.Insert(0, liSel);

        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 totalRows = 0;
            CAcGroup oAcGrp = null;
            try
            {
                oAcGrp = new CAcGroup();
                dt = oAcGrp.GetAcGroupsPG(pPgIndx, ref totalRows);
                gvAcGrp.DataSource = dt.DefaultView;
                gvAcGrp.DataBind();
                lblTotalPages.Text = CalculateTotalPages(totalRows).ToString();
                lblCurrentPage.Text = currentPageNumber.ToString();
                if (currentPageNumber == 1)
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
                    if (currentPageNumber == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }

            finally
            {
                oAcGrp = null;
                dt.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalRows"></param>
        /// <returns></returns>
        private int CalculateTotalPages(double totalRows)
        {
            int totalPages = (int)Math.Ceiling(totalRows / gblValue.PgSize1);
            return totalPages;
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
                    currentPageNumber = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;

                case "Next":
                    currentPageNumber = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(currentPageNumber);
            tabAcGr.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAcGrp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vAcGrpId = 0;
            DataTable dt = null;
            CAcGroup oAcGrp = null;
            try
            {
                vAcGrpId = Convert.ToInt32(e.CommandArgument);
                ViewState["AcGroupId"] = vAcGrpId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvAcGrp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oAcGrp = new CAcGroup();
                    dt = oAcGrp.GetAcGroupById(vAcGrpId);
                    if (dt.Rows.Count > 0)
                    {
                        txtAcGrp.Text = Convert.ToString(dt.Rows[0]["AcGroup"]);
                        ddlAcHd.SelectedIndex = ddlAcHd.Items.IndexOf(ddlAcHd.Items.FindByValue(dt.Rows[0]["ACHeadId"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabAcGr.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
                oAcGrp = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Boolean ValidateFields()
        {
            Boolean vResult = true;
            if (txtAcGrp.Text.Trim() == "")
            {
                EnableControl(true);
                gblFuction.MsgPopup("Account Group Can not be left blank...");
                gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
                vResult = false;
                return vResult;
            }
            if (ddlAcHd.SelectedIndex <= 0)
            {
                EnableControl(true);
                gblFuction.MsgPopup("Account Head Can not be left blank...");
                gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_ddlAcHd");
                vResult = false;
                return vResult;
            }
            return vResult;
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
            Int32 vAcGrpId = Convert.ToInt32(ViewState["AcGroupId"]);
            Int32 vAcHeadId = Convert.ToInt32(ddlAcHd.SelectedValue);
            Int32 vErr = 0, vRec = 0;
            DataTable dt = null;
            CAcGroup oAcGrp = null;
            CGblIdGenerator oGlb = null;
            try
            {
                if (ValidateFields() == false)
                    return false;

                if (Mode == "Save")
                {
                    oAcGrp = new CAcGroup();
                    dt = oAcGrp.ChkDupAcGroupName(txtAcGrp.Text.Replace("'", "''"), 0, "S");
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Account Group Can not be Duplicate...");
                        gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_txtAcGrp");
                        return false;
                    }
                    vErr = oAcGrp.InsertAcGroup(txtAcGrp.Text.Replace("'", "''"), vAcHeadId, "N", this.UserID, "I", 0);
                    if (vErr == 0)
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
                    oAcGrp = new CAcGroup();
                    dt = oAcGrp.ChkDupAcGroupName(txtAcGrp.Text.Replace("'", "''"), vAcGrpId, "E");
                    if (dt.Rows.Count > 0)
                    {
                        gblFuction.MsgPopup("Account Group Can not be Duplicated.");
                        gblFuction.focus("ctl00_cph_Main_tabAcGr_pnlDtl_ddlAcHd");
                        return false;
                    }
                    dt = oAcGrp.GetAcGroupById(vAcGrpId);
                    if (Convert.ToString(dt.Rows[0]["System"]) == "Y")
                    {
                        gblFuction.MsgPopup("You Can Not Edit the System Record");
                        return false;
                    }
                    vErr = oAcGrp.UpdateAcGroup(vAcGrpId, txtAcGrp.Text.Replace("'", "''"), vAcHeadId, "N", this.UserID, "E", 0);
                    if (vErr == 0)
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
                    oAcGrp = new CAcGroup();
                    oGlb = new CGblIdGenerator();
                    vRec = oGlb.ChkDelete(vAcGrpId, "AcGroupId", "AcSubGrp");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup(gblMarg.RecordUseMsg);
                        return false;
                    }


                    dt = oAcGrp.GetAcGroupById(vAcGrpId);
                    if (Convert.ToString(dt.Rows[0]["System"]) == "Y")
                    {
                        gblFuction.MsgPopup("You Can not Delete the System Record");
                        return false;
                    }
                    vErr = oAcGrp.UpdateAcGroup(vAcGrpId, txtAcGrp.Text.Replace("'", "''"), vAcHeadId, "N", this.UserID, "D", 0);
                    if (vErr == 0)
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
        }
    }
}