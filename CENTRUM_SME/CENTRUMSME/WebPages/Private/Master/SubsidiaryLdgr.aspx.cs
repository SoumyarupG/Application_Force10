using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class SubsidiaryLdgr : CENTRUMBAse
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
		if (Session[gblValue.BrnchCode].ToString() != "0000")
		    StatusButton("Exit");		    
                  
                ViewState["StateEdit"] = null;
                PopAccHead();
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    PopBranchB();
                else
                    PopBranch();
                LoadList();
                tabSubs.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<---ALL--->", "ALL");
                //dt = oCb.PopComboMIS("S", "N", "AA", "BranchCode", "BranchName", "BranchMst", Convert.ToInt32(Session[gblValue.BrnchCode].ToString()), "BranchCode", "AA", System.DateTime.Now, Session[gblValue.BrnchCode].ToString());
                dt = oCb.PopComboMIS("N", "Y", "BranchCode", "BranchCode", "BranchName", "BranchMst", Convert.ToInt32(Session[gblValue.BrnchCode].ToString()), "BranchCode", "AA", vLogDt, Session[gblValue.BrnchCode].ToString());
                ddlBranch.DataSource = dt;
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataTextField = "Name";
                ddlBranch.DataBind();
                ddlBranch.Items.Insert(0, liSel);
            }
            finally
            {
                oCb = null;
                dt.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void PopBranchB()
        {
            DataTable dt = null;
            CGblIdGenerator oCb = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCb = new CGblIdGenerator();
                ListItem liSel = new ListItem("<---ALL--->", "ALL");
                dt = oCb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", 0, "AA", "AA", vLogDt, Session[gblValue.BrnchCode].ToString());
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
                this.PageHeading = "Subsidiary Ledger";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString()+ " ( Login Date " + Session[gblValue.LoginDate].ToString()  + " )";
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
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Subsidiary Ledger", false);
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvSub_SelectedNodeChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            CSubsidary obj = null;
            String vSubId = "";
            try
            {
                vSubId = tvSub.SelectedNode.Value.Substring(2);
                obj = new CSubsidary();
                dt= obj.GetSubsidaryList(vSubId);
                ddlGenLed.SelectedIndex = ddlGenLed.Items.IndexOf(ddlGenLed.Items.FindByValue(Convert.ToString(dt.Rows[0]["DescId"])));
                ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[0]["BranchCode"])));
                txtGenCode.Text = Convert.ToString(dt.Rows[0]["SubsidiaryCode"]);
                txtSubsAc.Text = Convert.ToString(dt.Rows[0]["SubsidiaryLed"]);
                ViewState["SubsidiaryId"] = Convert.ToString(dt.Rows[0]["SubsidiaryId"]);
                tabSubs.ActiveTabIndex = 1;
                StatusButton("Show");
            }
            finally
            {
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void tvSub_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            DataTable dtAcctGrp = null;
            //DataTable dtAcctSubGrp = null;
            TreeNode tnGroup = null;
            //TreeNode tnSubGroup = null;
            string vAHId = "";//, vGID = 0;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CGblIdGenerator oCb = null;
            try
            {
                if (e.Node.Value.Substring(0, 2) == "AH")
                {
                    oCb = new CGblIdGenerator();

                    e.Node.ChildNodes.Clear();
                    vAHId = e.Node.Value.Substring(2).ToString(); ;
                    dtAcctGrp = oCb.PopComboMIS("S", "N", "AA", "SubsidiaryId", "SubsidiaryLed", "SubsidiaryMst", vAHId, "DescId", "AA", vLogDt, Session[gblValue.BrnchCode].ToString());
                    //dtAcctGrp = oCb.GetAllSubsidary();
                    foreach (DataRow drBr in dtAcctGrp.Rows)
                    {
                        tnGroup = new TreeNode(Convert.ToString(drBr["SubsidiaryLed"]));
                        tnGroup.Value = Convert.ToString("AG" + drBr["SubsidiaryId"]);
                        e.Node.ChildNodes.Add(tnGroup);
                    }
                }
               
            }
            finally
            {
                dtAcctGrp = null;
                //dtAcctSubGrp = null;
                oCb = null;
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
                if (this.CanAdd == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Add);
                    return;
                }
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
                    LoadList();
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
            tabSubs.ActiveTabIndex = 0;
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
                LoadList();
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
            txtGenCode.Enabled = Status;
            txtSubsAc.Enabled = Status;
            ddlGenLed.Enabled = Status;
            ddlBranch.Enabled = Status;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtGenCode.Text = "";
            txtSubsAc.Text = "";
            ddlGenLed.SelectedIndex = -1;
            ddlBranch.SelectedIndex = -1;
            lblUser.Text = "";
            lblDate.Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopAccHead()
        {
            DataTable dt = null;
            CSubsidary oCb = null;
            try
            {
                oCb = new CSubsidary();
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                dt = oCb.AcGenWithSub(Convert.ToString(Session[gblValue.BrnchCode]));
                
                ddlGenLed.DataSource = dt;
                ddlGenLed.DataTextField = "Desc";
                ddlGenLed.DataValueField = "DescId";
                ddlGenLed.DataBind();
                ddlGenLed.Items.Insert(0, liSel);
            }
            finally
            {
                dt = null;
                oCb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //private void LoadGrid(Int32 pPgIndx)
        //{
        //    DataTable dt = null;
        //    Int32 totalRows = 0;
        //    CSubsidary oSub = null;
        //    try
        //    {
        //        oSub = new CSubsidary();
        //        dt = oSub.GetAllSubsidary(pPgIndx, ref totalRows);
        //        gvSubs.DataSource = dt.DefaultView;
        //        gvSubs.DataBind();
        //        lblTotPg.Text = CalculateTotalPages(totalRows).ToString();
        //        lblCrPg.Text = vPgNo.ToString();
        //        if (vPgNo == 1)
        //        {
        //            btnPrev.Enabled = false;
        //            if (Int32.Parse(lblTotPg.Text) > 1)
        //                btnNext.Enabled = true;
        //            else
        //                btnNext.Enabled = false;
        //        }
        //        else
        //        {
        //            btnPrev.Enabled = true;
        //            if (vPgNo == Int32.Parse(lblTotPg.Text))
        //                btnNext.Enabled = false;
        //            else
        //                btnNext.Enabled = true;
        //        }
        //    }
        //    finally
        //    {
        //        oSub = null;
        //        dt.Dispose();
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        private void LoadList()
        {
            DataTable dtRoot = null;
            TreeNode tnRoot = null;
            //TreeNode tnAccGrp = null;
            string vAcctId = "";
            tvSub.Nodes.Clear();
            CSubsidary oCb = null;
            try
            {
                oCb = new CSubsidary();
                dtRoot = oCb.AcGenWithSub(Convert.ToString(Session[gblValue.BrnchCode]));//.PopComboMIS("N", "N", "AA", "EOID", "EOName", "EOMst", 0, "AA", "AA", System.DateTime.Now, "0000");
                foreach (DataRow dr in dtRoot.Rows)
                {
                    tnRoot = new TreeNode(dr["Desc"].ToString());
                    tnRoot.PopulateOnDemand = true;
                    tnRoot.SelectAction = TreeNodeSelectAction.None;
                    tvSub.Nodes.Add(tnRoot);
                    tnRoot.Value = Convert.ToString("AH" + dr["DescID"]);
                    vAcctId = dr["DescID"].ToString();
                    //for (int iC = 0; iC < 1; iC++)
                    //{
                    //    tnAccGrp = new TreeNode("No Record");
                    //    tnAccGrp.PopulateOnDemand = false;
                    //    tnAccGrp.Value = "AG";
                    //    tnRoot.ChildNodes.Add(tnAccGrp);
                    tnRoot.CollapseAll();
                    //}
                    //tnAccGrp.SelectAction = TreeNodeSelectAction.None;
                }
            }
            finally
            {
                oCb = null;
                dtRoot = null;
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
        //protected void ChangePage(object sender, CommandEventArgs e)
        //{
        //    switch (e.CommandName)
        //    {
        //        case "Prev":
        //            vPgNo = Int32.Parse(lblCrPg.Text) - 1;
        //            break;
        //        case "Next":
        //            vPgNo = Int32.Parse(lblCrPg.Text) + 1;
        //            break;
        //    }
        //    LoadGrid(vPgNo);
        //    tabSubs.ActiveTabIndex = 0;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void gvSubs_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    string pSubId = "";
        //    DataTable dt = null;
        //    CSubsidary oSub = null;
        //    try
        //    {
        //        pSubId = Convert.ToString(e.CommandArgument);
        //        ViewState["SubsidiaryId"] = pSubId;
        //        if (e.CommandName == "cmdShow")
        //        {
        //            GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
        //            LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
        //            foreach (GridViewRow gr in gvSubs.Rows)
        //            {
        //                LinkButton lb = (LinkButton)gr.FindControl("btnShow");
        //                lb.ForeColor = System.Drawing.Color.Black;
        //            }
        //            btnShow.ForeColor = System.Drawing.Color.Red;
        //            oSub = new CSubsidary();
        //            dt = oSub.GetSubsidaryList(pSubId);
        //            if (dt.Rows.Count > 0)
        //            {
        //                txtGenCode.Text = Convert.ToString(dt.Rows[0]["SubsidiaryCode"]);
        //                txtSubsAc.Text = Convert.ToString(dt.Rows[0]["SubsidiaryLed"]);
        //                ddlGenLed.SelectedIndex = ddlGenLed.Items.IndexOf(ddlGenLed.Items.FindByValue(dt.Rows[0]["DescId"].ToString()));
        //                lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
        //                lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
        //                tabSubs.ActiveTabIndex = 1;
        //                StatusButton("Show");
        //            }
        //        }
        //    }           
        //    finally
        //    {
        //        dt.Dispose();
        //        oSub = null;
        //    }
        //}         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vSubId = Convert.ToString(ViewState["SubsidiaryId"]);
            string vDescId ="", vNewSubId="",vBranch=""; 
            Int32 vErr = 0, vRec = 0;          
            CSubsidary oSub = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vDescId = ddlGenLed.SelectedValue.ToString();
                vBranch = ddlBranch.SelectedValue.ToString();
                if (Mode == "Save")
                {
                    oSub = new CSubsidary();
                    vRec = oSub.ChkDupSubsidery(vSubId, txtSubsAc.Text.Replace("'", "''"), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Subsidiary Account Can not be Duplicate...");
                        return false;
                    }
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                        txtSubsAc.Text.Replace("'", "''"), vDescId, this.UserID, "I", 0, "Save", vBranch);
                    if (vErr > 0)
                    {
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
                    oSub = new CSubsidary();
                    vRec = oSub.ChkDupSubsidery(vSubId, txtSubsAc.Text.Replace("'", "''"), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Subsidiary Account Can not be Duplicate...");
                        return false;
                    }
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                         txtSubsAc.Text.Replace("'", "''"), vDescId, this.UserID, "E", 0, "Edit", vBranch);
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
                    oSub = new CSubsidary();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDelete(vSubId, "SubsidiaryId", Session[gblValue.ACVouDtl].ToString());
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Member has already applied for Loan ...Can't be Deleted");
                        return false;
                    }
                    vErr = oSub.InsertSubsidiaryMst(ref vNewSubId, vSubId, txtGenCode.Text.Replace("'", "''"),
                        txtSubsAc.Text.Replace("'", "''"), vDescId, this.UserID, "D", 0, "Delet", vBranch);
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
                oSub = null;
            }
        }
    }
}