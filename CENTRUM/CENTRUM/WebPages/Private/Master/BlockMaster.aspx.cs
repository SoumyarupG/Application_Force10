using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class BlockMaster :CENTRUMBase
    {
        protected int vPgNo = 1;

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
                ViewState["State"] = null;
                popState();
                LoadGrid(0);
                tbBlk.ActiveTabIndex = 0;
                ClearControls();
                StatusButton("View");
                EnableControl(false);
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
                this.PageHeading = "Block/PS";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuBlockMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Block/PS Master", false);
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void popState()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "StateId", "StateName", "StateMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlState.DataSource = dt;
                ddlState.DataTextField = "StateName";
                ddlState.DataValueField = "StateId";
                ddlState.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlState.Items.Insert(0, oli);
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
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vStatId = Convert.ToInt32(ddlState.SelectedValue);
            FillDist(vStatId,0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pStateId"></param>
        private void FillDist(Int32 pStateId, Int32 pDistrictId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                if (pStateId > 0)
                {
                    oGb = new CGblIdGenerator();
                    dt = oGb.PopComboMIS("S", "N", "AA", "DistrictId", "DistrictName", "DistrictMst", pStateId, "StateId", "AA", gblFuction.setDate("01/01/1900"), "0000");
                    ddlDist.DataSource = dt;
                    ddlDist.DataTextField = "DistrictName";
                    ddlDist.DataValueField = "DistrictId";
                    ddlDist.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlDist.Items.Insert(0, oli);
                    if (pDistrictId > 0)
                        ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(pDistrictId.ToString()));
                }
            }
            finally
            {
                dt = null;
                oGb = null;
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
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
                ViewState["StateEdit"] = "Add";
                tbBlk.ActiveTabIndex = 1;
                StatusButton("Add");                
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
                    tbBlk.ActiveTabIndex = 0;
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
            tbBlk.ActiveTabIndex = 0;
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32  vTotRows = 0; //vDistId = 0,        
            CBlock oBlk = null;
            string pBrCode = "";
            try
            {
                //vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());   
                 pBrCode = Session[gblValue.BrnchCode].ToString();
                oBlk = new CBlock();
                dt = oBlk.GetBlockPG(pPgIndx, ref vTotRows, pBrCode);
                gvBlk.DataSource = dt;
                gvBlk.DataBind();
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
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
                oBlk = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
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
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadGrid(vPgNo);
            tbBlk.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBlk_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vBlkId = 0;
            DataTable dt = null;
            CBlock oBlk = null;
            try
            {
                vBlkId = Convert.ToInt32(e.CommandArgument);
                ViewState["BlkId"] = vBlkId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvBlk.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oBlk = new CBlock();
                    dt = oBlk.GetBlockDetails(vBlkId);
                    if (dt.Rows.Count > 0)
                    {                         
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateId"].ToString()));
                        FillDist(Convert.ToInt32(dt.Rows[0]["StateId"].ToString()), Convert.ToInt32(dt.Rows[0]["DistrictId"].ToString()));                      
                        txtBlock.Text = Convert.ToString(dt.Rows[0]["BlockName"]);                        
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbBlk.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oBlk = null;
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vBlkId = Convert.ToInt32(ViewState["BlkId"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, vDistId = 0;
            CBlock oBlk = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vDistId = Convert.ToInt32(ddlDist.SelectedValue);
                if (Mode == "Save")
                {
                    oBlk = new CBlock();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BlockMst", "BlockName", txtBlock.Text.Replace("'", "''"), "BranchCode", vBrCode, "BlockId", vBlkId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Block Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBlk.SaveBlock(ref vNewId, vBlkId, txtBlock.Text.Replace("'", "''"), vDistId, this.UserID, "Save", vBrCode);
                    if (vErr > 0)
                    {
                        ViewState["BlkId"] = vNewId;
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
                    oBlk = new CBlock();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("BlockMst", "BlockName", txtBlock.Text.Replace("'", "''"), "BranchCode",vBrCode, "BlockId", vBlkId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Block Can not be Duplicate...");
                        return false;
                    }
                    vErr = oBlk.SaveBlock(ref vNewId, vBlkId, txtBlock.Text.Replace("'", "''"), vDistId, this.UserID, "Edit", vBrCode);
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
                    oBlk = new CBlock();
                    vErr = oBlk.SaveBlock(ref vNewId, vBlkId, txtBlock.Text.Replace("'", "''"), vDistId, this.UserID, "Delet", vBrCode);
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
                oBlk = null;
                oGbl = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtBlock.Enabled = Status;
            ddlState.Enabled = Status;
            ddlDist.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtBlock.Text = "";
            ddlState.SelectedIndex = -1;
            ddlDist.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
    }
}