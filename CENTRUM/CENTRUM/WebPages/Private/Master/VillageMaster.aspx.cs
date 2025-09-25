using System;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;
using System.Web;

namespace CENTRUM.WebPages.Private.Master
{
    public partial class VillageMaster : CENTRUMBase
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
                LoadGrid(1);
                StatusButton("View");
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
                this.Menu = false;
                this.PageHeading = "Village";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuVillageMst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Village Master", false);
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
                    btnPrint.Enabled = false;
                    ClearControls();                   
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnPrint.Enabled = false;
                    EnableControl(true);                    
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
                    EnableControl(false);                   
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnPrint.Enabled = true;
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
                ddlStat.DataSource = dt;
                ddlStat.DataTextField = "StateName";
                ddlStat.DataValueField = "StateId";
                ddlStat.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlStat.Items.Insert(0, oli);
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
        /// <param name="pDistrictId"></param>
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
        /// <param name="pBlkId"></param>
        private void PopGP(Int32 pBlkId)
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string pBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (pBlkId > 0)
                {
                    oGb = new CGblIdGenerator();
                    dt = oGb.PopComboMIS("S", "N", "AA", "GPId", "GPName", "GPMst", pBlkId, "BlockId", "AA", gblFuction.setDate("01/01/1900"), pBrCode);
                    ddlGp.DataSource = dt;
                    ddlGp.DataTextField = "GPName";
                    ddlGp.DataValueField = "GPId";
                    ddlGp.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlGp.Items.Insert(0, oli);
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
        protected void ddlStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vStateId = Convert.ToInt32(ddlStat.SelectedItem.Value);
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
        protected void ddlBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 vBlkId = Convert.ToInt32(ddlBlock.SelectedItem.Value);
            PopGP(vBlkId);
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
                txtClsDt.Enabled = false;
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
        protected void chkVilCls_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVilCls.Checked == true)
                txtClsDt.Enabled = true;
            else
                txtClsDt.Enabled = false;
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
                    LoadGrid(1);
                    ClearControls();
                    tbVlg.ActiveTabIndex = 0;
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
                txtClsDt.Enabled = false;
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
            Int32 vTotRows = 0; ;           
            CVillage oVlg = null;
            string vBrCode = "";
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oVlg = new CVillage();
                dt = oVlg.GetVillagePG(pPgIndx, ref vTotRows, txtSearch.Text.Trim(), vBrCode);
                gvVlg.DataSource = dt;
                gvVlg.DataBind();
                ViewState["Village"] = dt;
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
                oVlg = null;
                dt=null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
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
            tbVlg.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvVlg_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vVlgId = 0, vRow=0;
            DataTable dt = null;           
            try
            {
                vVlgId = Convert.ToInt32(e.CommandArgument);
                ViewState["VlgId"] = vVlgId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvVlg.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    dt = (DataTable)ViewState["Village"];
                    dt.PrimaryKey = new DataColumn[] { dt.Columns["VillageId"] };
                    vRow = dt.Rows.IndexOf(dt.Rows.Find(vVlgId));
                    Int32 pStatId = Convert.ToInt32(dt.Rows[vRow]["StateId"].ToString());
                    Int32 pDistId = Convert.ToInt32(dt.Rows[vRow]["DistrictId"].ToString());
                    Int32 pBlkId = Convert.ToInt32(dt.Rows[vRow]["BlockId"].ToString());
                    Int32 pGPId = Convert.ToInt32(dt.Rows[vRow]["GPId"].ToString());
                    PopDist(pStatId);
                    PopBlk(pDistId);
                    PopGP(pBlkId);                   
                    if (dt.Rows.Count > 0)
                    {
                        ddlStat.SelectedIndex = ddlStat.Items.IndexOf(ddlStat.Items.FindByValue(dt.Rows[vRow]["StateId"].ToString()));
                        ddlDist.SelectedIndex = ddlDist.Items.IndexOf(ddlDist.Items.FindByValue(dt.Rows[vRow]["DistrictId"].ToString()));
                        ddlBlock.SelectedIndex = ddlBlock.Items.IndexOf(ddlBlock.Items.FindByValue(dt.Rows[vRow]["BlockId"].ToString()));
                        ddlGp.SelectedIndex = ddlGp.Items.IndexOf(ddlGp.Items.FindByValue(dt.Rows[vRow]["GPId"].ToString()));
                        txtVillage.Text = Convert.ToString(dt.Rows[vRow]["VillageName"]);
                        txtDisBr.Text = Convert.ToString(dt.Rows[vRow]["Dist_from_Br"]);
                        txtDisPS.Text = Convert.ToString(dt.Rows[vRow]["Dist_from_PS"]);
                        txtTotPop.Text = Convert.ToString(dt.Rows[vRow]["Population"]);
                        txtActDt.Text = Convert.ToString(dt.Rows[vRow]["DOF"]);
                        string ClsStats = Convert.ToString(dt.Rows[vRow]["CloseStatus"]);
                        if (ClsStats == "Y")
                        {
                            chkVilCls.Enabled = false;
                            txtClsDt.Enabled = false;
                            chkVilCls.Checked=true;
                            txtClsDt.Text = Convert.ToString(dt.Rows[vRow]["CloseDate"]);
                        }
                        else
                        {
                            chkVilCls.Checked=false;
                            txtClsDt.Text = "";
                            chkVilCls.Enabled = false;
                            txtClsDt.Enabled = false;
                        }
                        lblUser.Text = "Last Modified By : " + dt.Rows[vRow]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[vRow]["CreationDateTime"].ToString();
                        tbVlg.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
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
            Int32 vVillageId = Convert.ToInt32(ViewState["VlgId"]);
            Int32 vErr = 0, vRec = 0, vNewId = 0, vBlkId = 0;
            Int32 vTotPopu = 0, vDisBr = 0, vDisPS = 0;
            CVillage oVlg = null;
            CGblIdGenerator oGbl = null;
            string vClosStats="";
            if(txtTotPop.Text=="")
                vTotPopu=0;
            else
                vTotPopu=Convert.ToInt32(txtTotPop.Text);
            if(txtDisBr.Text=="")
                vDisBr=0;
            else
                vDisBr=Convert.ToInt32(txtDisBr.Text);
            if(txtDisPS.Text=="")
                vDisPS=0;
            else
                vDisPS=Convert.ToInt32(txtDisPS.Text);
            if(chkVilCls.Checked==true)
                vClosStats="Y";
            else
                vClosStats="N";
            try
            {
                vBlkId = Convert.ToInt32(ddlBlock.SelectedValue);
                if (Mode == "Save")
                {
                    oVlg = new CVillage();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("VillageMst", "VillageName", txtVillage.Text.Replace("'", "''"), "GPId", ddlGp.SelectedValue, "VillageId", vVillageId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Village Can not be Duplicate...");
                        return false;
                    }
                    vErr = oVlg.SaveVillage(ref vNewId, vVillageId, txtVillage.Text.Replace("'", "''"), Convert.ToInt32(ddlGp.SelectedValue), vTotPopu, vDisBr, vDisPS, gblFuction.setDate(txtActDt.Text), vClosStats, gblFuction.setDate(txtClsDt.Text), this.UserID, "Save", vBrCode);
                    if (vErr > 0)
                    {
                        ViewState["VlgId"] = vNewId;
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
                    oVlg = new CVillage();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("VillageMst", "VillageName", txtVillage.Text.Replace("'", "''"), "GPId", ddlGp.SelectedValue, "VillageId", vVillageId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Village Can not be Duplicate...");
                        return false;
                    }
                    vErr = oVlg.SaveVillage(ref vNewId, vVillageId, txtVillage.Text.Replace("'", "''"), Convert.ToInt32(ddlGp.SelectedValue), vTotPopu, vDisBr, vDisPS, gblFuction.setDate(txtActDt.Text), vClosStats, gblFuction.setDate(txtClsDt.Text), this.UserID, "Edit", vBrCode);
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
                    oVlg = new CVillage();
                    vErr = oVlg.SaveVillage(ref vNewId, vVillageId, txtVillage.Text.Replace("'", "''"), Convert.ToInt32(ddlGp.SelectedValue), vTotPopu, vDisBr, vDisPS, gblFuction.setDate(txtActDt.Text), vClosStats, gblFuction.setDate(txtClsDt.Text), this.UserID, "Delet", vBrCode);
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
            ddlStat.Enabled = Status;
            ddlDist.Enabled = Status;
            ddlBlock.Enabled = Status;
            ddlGp.Enabled = Status;
            txtVillage.Enabled = Status;
            txtDisBr.Enabled = Status;
            txtDisPS.Enabled = Status;
            txtTotPop.Enabled = Status;
            txtActDt.Enabled = Status;
            chkVilCls.Enabled = Status;
            txtClsDt.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            ddlStat.SelectedIndex = -1;
            ddlDist.SelectedIndex = -1;
            ddlBlock.SelectedIndex = -1;
            ddlGp.SelectedIndex = -1;
            txtVillage.Text = "";
            txtDisBr.Text = "";
            txtDisPS.Text = "";
            txtTotPop.Text = "";
            txtActDt.Text = "";
            chkVilCls.Checked = false;
            txtClsDt.Text = "";
            lblDate.Text = "";
            lblUser.Text = "";
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            CReports oRpt = new CReports();
            dt = oRpt.RptVillageMst(Session[gblValue.BrnchCode].ToString());

            string vFileNm = "attachment;filename=District_Master_Details.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", vFileNm);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write("<style>  .txt " + "\r\n" + " {mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
            Response.Write("<table border='1' cellpadding='5' widht='120%'>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><font size='3'>" + CGblIdGenerator.GetBranchAddress1(Session[gblValue.BrnchCode].ToString()) + "</font></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>District List</font></b></td></tr>");
            //Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'>As on Date " + txtAsOnDt.Text + "</font></b></td></tr>");
            Response.Write("<tr><td align=center' colspan='" + dt.Columns.Count + "'><b><font size='3'></font></b></td></tr>");
            string tab = string.Empty;
            Response.Write("<tr>");
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write("<td><b>" + dtcol.ColumnName + "<b></td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dtrow in dt.Rows)
            {
                Response.Write("<tr style='height:20px;'>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToString() == "RBI_State_Code" || dt.Columns[j].ColumnName.ToString() == "RBI_District_Code" || dt.Columns[j].ColumnName.ToString() == "BranchCode")
                    {
                        Response.Write("<td &nbsp nowrap class='txt'>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                    else
                    {

                        Response.Write("<td nowrap>" + Convert.ToString(dtrow[j]) + "</td>");
                    }
                }
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.End();

        }
    }
}