using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCECA;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class GP : CENTRUMBase
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
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                popState();             
                LoadGrid(0);
                tbGp.ActiveTabIndex = 0;
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
                this.PageHeading = "GP/Ward";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGpMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "GP/Ward Master", false);
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
        /// <param name="pStateId"></param>
        private void PopDist(Int32 pStateId)
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
        /// <param name="pStateId"></param>
        private void PopBlk(Int32 pDistrictId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string pBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (pDistrictId > 0)
                {
                    oGb = new CGblIdGenerator();
                    dt = oGb.PopComboMIS("S", "N", "AA", "BlockId", "BlockName", "BlockMst", pDistrictId, "DistrictId", "AA", gblFuction.setDate("01/01/1900"), pBrCode);
                    ddlBlock.DataSource = dt;
                    ddlBlock.DataTextField = "BlockName";
                    ddlBlock.DataValueField = "BlockId";
                    ddlBlock.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlBlock.Items.Insert(0, oli);                  
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
        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vStateId = Convert.ToInt32(ddlState.SelectedItem.Value);
            PopDist(vStateId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vDistId = Convert.ToInt32(ddlDist.SelectedItem.Value);
            PopBlk(vDistId);
            
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
                tbGp.ActiveTabIndex = 1;
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
                    tbGp.ActiveTabIndex = 0;
                    ClearControls();
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
            tbGp.ActiveTabIndex = 0;
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
            ViewState["BlockId"] = ddlBlock.SelectedValue;
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
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32  vTotRows = 0;    // vDistId = 0,      
            CGP oGP = null;
            string pBrCode = "";
            try
            {
                //vDistId = Convert.ToInt32(Session[gblValue.DistrictId].ToString());
                pBrCode = Session[gblValue.BrnchCode].ToString();
                oGP = new CGP();
                dt = oGP.GetGPPG(pPgIndx, ref vTotRows, pBrCode);
                gvGp.DataSource = dt;
                gvGp.DataBind();
                ViewState["GP"] = dt;
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
                dt = null;
                oGP = null;
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
            tbGp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvGp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vGPId = 0, vRow = 0, vStatId = 0, vDistId=0;
            DataTable dt = null;
            try
            {
                vGPId = Convert.ToInt32(e.CommandArgument);
                ViewState["GPId"] = vGPId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvGp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    dt = (DataTable)ViewState["GP"];
                    dt.PrimaryKey=new DataColumn[]{dt.Columns["GPId"]};
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(vGPId));
                    vStatId = Convert.ToInt32(dt.Rows[vRow]["StateId"].ToString());
                    vDistId = Convert.ToInt32(dt.Rows[vRow]["DistrictId"].ToString());
                    PopDist(vStatId);
                    PopBlk(vDistId);
                    if (dt.Rows.Count > 0)
                    {
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[vRow]["StateId"].ToString()));
                        ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(dt.Rows[vRow]["DistrictId"].ToString()));
                        ddlBlock.SelectedIndex = ddlBlock.Items.IndexOf(ddlBlock.Items.FindByValue(dt.Rows[vRow]["BlockId"].ToString()));
                        txtGP.Text = dt.Rows[vRow]["GPName"].ToString();
                        txtPopu.Text = dt.Rows[vRow]["Population"].ToString();
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbGp.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt=null;
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
            string vGPId = Convert.ToString(ViewState["GPId"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, vBlkId = 0, vGpId=0,vPop=0;
            CGP oGP = null;
            CGblIdGenerator oGbl = null;
            string pBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (txtPopu.Text.Trim()!="")
                    vPop=Convert.ToInt32(txtPopu.Text);
                 
                vBlkId = Convert.ToInt32(ViewState["BlockId"]);
                if (Mode == "Save")
                {
                    oGP = new CGP();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("GPMst", "GPName", txtGP.Text.Replace("'", "''"), "", "", "GPId", vGPId, "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("GP Can not be Duplicate...");
                        return false;
                    }
                    vErr = oGP.SaveGP(ref vNewId, vGpId, txtGP.Text.Replace("'", "''"), vBlkId, vPop, this.UserID, "Save", pBrCode);
                    if (vErr > 0)
                    {
                        ViewState["GPId"] = vNewId;
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
                    oGP = new CGP();
                    oGbl = new CGblIdGenerator();
                    vGpId = Convert.ToInt32(vGPId);
                    vRec = oGbl.ChkDuplicate("GPMst", "GPName", txtGP.Text.Replace("'", "''"), "", "", "GPId", vGPId, "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("GP Can not be Duplicate...");
                        return false;
                    }
                    vErr = oGP.SaveGP(ref vNewId, vGpId, txtGP.Text.Replace("'", "''"), vBlkId, vPop, this.UserID, "Edit", pBrCode);
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
                    oGP = new CGP();
                    vGpId = Convert.ToInt32(vGPId);
                    vErr = oGP.SaveGP(ref vNewId, vGpId, txtGP.Text.Replace("'", "''"), vBlkId, Convert.ToInt32(txtPopu.Text), this.UserID, "Delet", pBrCode);
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
                oGP = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            ddlState.Enabled = Status;
            ddlDist.Enabled = Status;
            ddlBlock.Enabled = Status;
            txtGP.Enabled = Status;
            txtPopu.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlState.SelectedIndex = -1;
            ddlDist.SelectedIndex = -1;
            ddlBlock.SelectedIndex = -1;
            txtGP.Text = "";
            txtPopu.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }
    }     
}