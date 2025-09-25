using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class ClusterMaster : CENTRUMBAse
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
                if (Session[gblValue.BrnchCode].ToString() != "0000" && Session[gblValue.BrnchCode].ToString() != "999")
                    StatusButton("Exit");
                else
                    StatusButton("View");
               // PopBranch(Session[gblValue.UserName].ToString());
                popState();
                //popBlock();
                LoadGrid(1);
                tbVlg.ActiveTabIndex = 0;
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
                this.PageHeading = "Cluster Master";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
                this.GetModuleByRole(mnuID.mnuClusterMst);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Cluster Master", false);
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
                tbVlg.ActiveTabIndex = 1;
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Delete Account Group...");
                //    return;
                //}
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                //if (Session[gblValue.BrnchCode].ToString() != "0000")
                //{
                //    gblFuction.MsgPopup("Branch can not Edit Account Group...");
                //    return;
                //}
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
            tbVlg.ActiveTabIndex = 0;
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
            Int32  vTotRows = 0; ;
            string vBrCode = "";
            CVillage oVlg = null;
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oVlg = new CVillage();
                dt = oVlg.GetClusterPG(vBrCode, pPgIndx, ref vTotRows);
                gvVlg.DataSource = dt;
                gvVlg.DataBind();
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
                if (Int32.Parse(lblTotPg.Text) > 0)
                {
                    txtGotoPg.Enabled = true;
                    btnGo.Enabled = true;
                }
                else
                {
                    txtGotoPg.Enabled = false;
                    btnGo.Enabled = false;
                }

            }
            finally
            {
                oVlg = null;
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
                case "GoTo":
                    if (Int32.Parse(txtGotoPg.Text) <= Int32.Parse(lblTotPg.Text))
                        vPgNo = Int32.Parse(txtGotoPg.Text);
                    break;
            }
            LoadGrid(vPgNo);
            tbVlg.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVlg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vClusterId = "";
            DataTable dt = null;
            CVillage oVlg = null;
            try
            {
                vClusterId = Convert.ToString(e.CommandArgument);
                ViewState["ClusterId"] = vClusterId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvVlg.Rows)
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

                    oVlg = new CVillage();
                    dt = oVlg.GetClusterDetails(vClusterId);
                    if (dt.Rows.Count > 0)
                    {
                        //ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        ddlState.SelectedIndex = ddlState.Items.IndexOf(ddlState.Items.FindByValue(dt.Rows[0]["StateId"].ToString()));
                        popDistrict();
                        ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(dt.Rows[0]["DistrictId"].ToString()));
                        popVillage();
                        ddlVillage.SelectedIndex = ddlVillage.Items.IndexOf(ddlVillage.Items.FindByValue(dt.Rows[0]["VillageId"].ToString()));
                        txtCluster.Text = Convert.ToString(dt.Rows[0]["ClusterName"]);
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbVlg.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oVlg = null;
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
            string vClusterId = Convert.ToString(ViewState["ClusterId"]);
            string vNewId ="",vVillageId="";
            Int32 vErr = 0, vRec = 0, vDistrictId = 0;
            CVillage oVlg = null;
            CGblIdGenerator oGbl = null;
            try
            {
                //if (vBrCode == "" || vBrCode == "-1" || vBrCode == "0000")
                //{
                //    gblFuction.MsgPopup("No Branch Selected...");
                //    return false;
                //}
                vDistrictId = Convert.ToInt32(ddlDist.SelectedValue);
                vVillageId = ddlVillage.SelectedValue.ToString();
                if (Mode == "Save")
                {
                    oVlg = new CVillage();
                    oGbl = new CGblIdGenerator();

                    vRec = oGbl.ChkDuplicate("ClusterMst", "ClusterName", txtCluster.Text.Replace("'", "''"), "VillageId", vVillageId.ToString(), "ClusterId", vClusterId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("City/Area Can not be Duplicate...");
                        return false;
                    }
                    vErr = oVlg.SaveCluster(ref vNewId, vClusterId, txtCluster.Text.Replace("'", "''"), vVillageId, vBrCode, this.UserID, "Save");
                    if (vErr > 0)
                    {
                        ViewState["ClusterId"] = vNewId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    oVlg = new CVillage();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("ClusterMst", "ClusterName", txtCluster.Text.Replace("'", "''"), "VillageId", vVillageId.ToString(), "ClusterId", vClusterId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("City/Area Can not be Duplicate...");
                        return false;
                    }
                    vErr = oVlg.SaveCluster(ref vNewId, vClusterId, txtCluster.Text.Replace("'", "''"), vVillageId, vBrCode, this.UserID, "Edit");
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
                    oVlg = new CVillage();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vClusterId, "ClusterId", "ClusterMst");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("The City/Area Has Cluster, You Can Not Delete The Village.");
                        return false;
                    }
                    vErr = oVlg.SaveCluster(ref vNewId, vClusterId, txtCluster.Text.Replace("'", "''"), vVillageId, vBrCode, this.UserID, "Delet");
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
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oVlg = null;
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
            txtCluster.Enabled = Status;
            //ddlBranch.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlState.SelectedIndex = -1;
            ddlDist.SelectedIndex = -1;  
            txtCluster.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
           // ddlBranch.SelectedIndex = -1;  
        }


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

        protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
        {   
           if(ddlState.SelectedValue.ToString() != "-1")
           {
                popDistrict();
           }
        }
        protected void ddlDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDist.SelectedValue.ToString() != "-1")
            {
                popVillage();
            }
        }
        private void popDistrict()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            int vState = Convert.ToInt32(ddlState.SelectedValue);
            ddlDist.Items.Clear();
            try
            {
                oVlg = new CVillage();
                dt = oVlg.popDistrictBySate(vState);
                ddlDist.DataSource = dt;
                ddlDist.DataTextField = "DistrictName";
                ddlDist.DataValueField = "DistrictId";
                ddlDist.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlDist.Items.Insert(0, oli);
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }
        private void popVillage()
        {
            DataTable dt = null;
            CVillage oVlg = null;
            int vDistId = Convert.ToInt32(ddlDist.SelectedValue);
            ddlVillage.Items.Clear();
            try
            {
                oVlg = new CVillage();
                dt = oVlg.popVillageByDist(vDistId);
                ddlVillage.DataSource = dt;
                ddlVillage.DataTextField = "VillageName";
                ddlVillage.DataValueField = "VillageId";
                ddlVillage.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVillage.Items.Insert(0, oli);
            }
            finally
            {
                oVlg = null;
                dt = null;
            }
        }

        //private void PopBranch(string pUser)
        //{
        //    DataTable dt = null;
        //    CUser oUsr = null;
        //    oUsr = new CUser();
        //    dt = oUsr.GetOnlyBranchWithoutHOByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
        //    string vBrCode = (string)Session[gblValue.BrnchCode];
        //    ViewState["ID"] = null;
        //    try
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlBranch.DataSource = dt;
        //            ddlBranch.DataTextField = "BranchName";
        //            ddlBranch.DataValueField = "BranchCode";
        //            ddlBranch.DataBind();
        //            ddlBranch.Items.Insert(0, new ListItem("<--Select-->", "-1"));
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oUsr = null;
        //    }
        //}
    }
}