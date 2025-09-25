using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;
using System.Drawing;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class IntInsPlan : CENTRUMBase
    {
        private int vPgNo = 1;

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
                ViewState["Branch"] = null;
                ViewState["SubInsp"] = null;
                tbEmp.ActiveTabIndex = 0;
                StatusButton("View");
                popInspector();
                PopSubInspector();
                PopBranch();
                LoadGrid(1);
                btnSave.Enabled = false;
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
                this.PageHeading = "Internal Inspection Plan";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuAuditPlan);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Internal Inspection Plan", false);
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
                    btnSave.Enabled = false;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
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
        /// <param name="Status"></param>
        private void EnableControl(Boolean Status)
        {
            txtPlanDt.Enabled = Status;
            ddlInsp.Enabled = Status;
            gvBranch.Enabled = Status;          
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtPlanDt.Text = Session[gblValue.LoginDate].ToString();
            ddlInsp.SelectedIndex = -1;           
            lblDate.Text = "";
            lblUser.Text = "";
            txtFrmDt.Text = "";
            txtToDt.Text = "";
            SetInitialRow();
        }

        /// <summary>
        /// 
        /// </summary>
        private void popInspector()
        {
            DataTable dt = null;
            CEO oRO = null;
            try
            {
                oRO = new CEO();
                dt = oRO.GetEOByIOAO();
                ddlInsp.DataSource = dt;
                ddlInsp.DataTextField = "EoName";
                ddlInsp.DataValueField = "EoID";
                ddlInsp.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlInsp.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopSubInspector()
        {
            DataTable dtEo = null;
            CEO oEo = null;
            try
            {
                oEo = new CEO();
                dtEo = oEo.GetEOByDesignation("IA");
                ViewState["SubInsp"] = dtEo;
            }
            finally
            {
                dtEo = null;
                oEo = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopBranch()
        {
            DataTable dtBr = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dtBr = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");            
                ViewState["Branch"] = dtBr;
            }
            finally
            {
                dtBr = null;
                oGb = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnShow_Click(object sender, EventArgs e)
        { 
            LoadGrid(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            Int32 vTotRows = 0;
            CInsPlan oAd = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oAd = new CInsPlan();
                dt = oAd.GetInsPlanListPG(pPgIndx, vFrmDt, vToDt, ref vTotRows);
                gvEmp.DataSource = dt;
                gvEmp.DataBind();
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
                oAd = null;
                dt = null;
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
            tbEmp.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetInitialRow()
        {
            DataTable dt = null;           
            try
            {
                dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add(new DataColumn("SlNo", typeof(int)));
                dt.Columns.Add(new DataColumn("Branch", typeof(string))); 
                dt.Columns.Add(new DataColumn("EOID", typeof(string)));
                dt.Columns.Add(new DataColumn("InspTyp", typeof(string)));
                dt.Columns.Add(new DataColumn("StartDt", typeof(string)));
                dt.Columns.Add(new DataColumn("EndDt", typeof(string)));
                dt.Columns.Add(new DataColumn("CmpDueDt", typeof(string)));
                dt.Columns.Add(new DataColumn("ActCmpDt", typeof(string)));
                dt.Columns.Add(new DataColumn("InsOff", typeof(string)));
                dt.Columns.Add(new DataColumn("StartDt1", typeof(string)));
                dt.Columns.Add(new DataColumn("EndDt1", typeof(string)));
                dr = dt.NewRow();
                dr["SlNo"] = 1;
                dr["Branch"] = "";
                dr["EOID"] = "";
                dr["InspTyp"] = "";
                dr["StartDt"] = "";
                dr["EndDt"] = "";
                dr["CmpDueDt"] = "";
                dr["ActCmpDt"] = "";              
                dr["StartDt1"] = "";
                dr["EndDt1"] = "";
                dt.Rows.Add(dr);               
                ViewState["DataTbl"] = dt;
                gvBranch.DataSource = dt;
                gvBranch.DataBind();                 
            }          
            finally
            {
                dt = null;               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddNewRowToGrid()
        {
            DataRow drCrRow = null;
            DataTable dtCuTbl = null;
            string vLgDt = Convert.ToString(Session[gblValue.LoginDate]);
            try
            {
                if (ViewState["DataTbl"] != null)
                {
                    dtCuTbl = (DataTable)ViewState["DataTbl"];                   
                    if (dtCuTbl.Rows.Count > 0)
                    {
                        drCrRow = dtCuTbl.NewRow();
                        drCrRow["SlNo"] = dtCuTbl.Rows.Count + 1;
                        dtCuTbl.Rows.Add(drCrRow);                      
                        ViewState["DataTbl"] = dtCuTbl;
                        for (int i = 0; i < dtCuTbl.Rows.Count - 1; i++)
                        {
                            TextBox txtStDt = (TextBox)gvBranch.Rows[i].FindControl("txtStDt");
                            TextBox txtEndDt = (TextBox)gvBranch.Rows[i].FindControl("txtEndDt");
                            dtCuTbl.Rows[i]["StartDt"] = txtStDt.Text;
                            dtCuTbl.Rows[i]["EndDt"] = txtEndDt.Text;
                            TextBox txtCmplDt = (TextBox)gvBranch.Rows[i].FindControl("txtCmplDt");
                            dtCuTbl.Rows[i]["CmpDueDt"] = txtCmplDt.Text;
                            TextBox txtACmplDt = (TextBox)gvBranch.Rows[i].FindControl("txtACmplDt");
                            dtCuTbl.Rows[i]["ActCmpDt"] = txtACmplDt.Text;                           
                            DropDownList ddlInspector = (DropDownList)gvBranch.Rows[i].FindControl("ddlSubInsp");  
                            dtCuTbl.Rows[i]["EOId"] = ddlInspector.SelectedValue;                           
                            DropDownList ddlBranch = (DropDownList)gvBranch.Rows[i].FindControl("ddlBranch");
                            dtCuTbl.Rows[i]["Branch"] = ddlBranch.SelectedValue;                          
                            DropDownList ddlInspTyp = (DropDownList)gvBranch.Rows[i].FindControl("ddlInspTyp");
                            dtCuTbl.Rows[i]["InspTyp"] = ddlInspTyp.SelectedValue;                         
                            dtCuTbl.Rows[i]["StartDt1"] = txtStDt.Text;
                            dtCuTbl.Rows[i]["EndDt1"] = txtEndDt.Text;

                            DateTime vLogDt = gblFuction.setDate(vLgDt);
                            DateTime vStDt = gblFuction.setDate(txtStDt.Text);
                            DateTime vEndDt = gblFuction.setDate(txtEndDt.Text);
                            DateTime vCmpDt = gblFuction.setDate(txtCmplDt.Text);
                            if (vLogDt >= vStDt)
                            {
                                gblFuction.MsgPopup("Start Date should be after the Login Date. At row - " +  (i+1).ToString());                                 
                                dtCuTbl.Rows[i].Delete();
                                dtCuTbl.AcceptChanges();
                                return;
                            }
                            if (vStDt >= vEndDt)
                            {
                                gblFuction.MsgPopup("End Date should be after the Start Date. At row - " + (i+1).ToString());
                                dtCuTbl.Rows[i].Delete();
                                dtCuTbl.AcceptChanges();
                                return;
                            }                           
                        }                                           
                        gvBranch.DataSource = dtCuTbl;
                        gvBranch.DataBind();
                    }
                }              
            }            
            finally
            {
                dtCuTbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnProc_Click(object sender, EventArgs e)
        {
            DataTable dtDtl = null;
            DataTable dt = null;
            try
            {
                dtDtl = (DataTable)ViewState["DataTbl"];
                dt = new DataTable();
                dt = dtDtl.Clone();
                foreach (GridViewRow gr in gvBranch.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["SlNo"] = 1;
                    dr["Branch"] = gr.Cells[10].Text;
                    dr["EOID"] = gr.Cells[11].Text;
                    dr["InspTyp"] = gr.Cells[12].Text;
                    TextBox txtStDt = (TextBox)gr.FindControl("txtStDt");
                    dr["StartDt"] = txtStDt.Text;
                    dr["StartDt1"] = txtStDt.Text;
                    TextBox txtEndDt = (TextBox)gr.FindControl("txtEndDt");
                    dr["EndDt"] = txtEndDt.Text;
                    dr["EndDt1"] = txtEndDt.Text;
                    TextBox txtCmplDt = (TextBox)gr.FindControl("txtCmplDt");
                    dr["CmpDueDt"] = txtCmplDt.Text;
                    TextBox txtACmplDt = (TextBox)gr.FindControl("txtACmplDt");
                    dr["ActCmpDt"] = txtACmplDt.Text;
                    DropDownList ddlSubInsp = (DropDownList)gr.FindControl("ddlSubInsp");
                    dr["EOId"] = ddlSubInsp.SelectedValue;
                    DropDownList ddlBranch = (DropDownList)gr.FindControl("ddlBranch");
                    dr["Branch"] = ddlBranch.SelectedValue;
                    DropDownList ddlInspTyp = (DropDownList)gr.FindControl("ddlInspTyp");
                    dr["InspTyp"] = ddlInspTyp.SelectedValue;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                ViewState["DataTbl"] = dt;
                gvBranch.DataSource = dt;
                gvBranch.DataBind();
                System.Threading.Thread.Sleep(1000);
                HighlightDuplicate(gvBranch);
            }
            finally
            {
                dtDtl = null;
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridview"></param>
        public void HighlightDuplicate(GridView gridview)
        {
            if (gridview.Rows.Count > 1)
            {
                for (int currentRow = 0; currentRow <= gridview.Rows.Count; currentRow++)
                {
                    for (int otherRow = currentRow + 1; otherRow <= gridview.Rows.Count - 1; otherRow++)
                    {
                        bool duplicateRow = false;
                        if (((gridview.Rows[currentRow].Cells[10].Text) == (gridview.Rows[otherRow].Cells[10].Text) &&
                            (gridview.Rows[currentRow].Cells[11].Text) == (gridview.Rows[otherRow].Cells[11].Text) &&
                            (gridview.Rows[currentRow].Cells[12].Text) == (gridview.Rows[otherRow].Cells[12].Text)) &&
                            ((gridview.Rows[currentRow].Cells[13].Text) == (gridview.Rows[otherRow].Cells[13].Text) &&
                            (gridview.Rows[currentRow].Cells[14].Text) == (gridview.Rows[otherRow].Cells[14].Text))  
                            )
                        {
                            duplicateRow = true;
                            btnSave.Enabled = false;
                            DropDownList ddlBranch = (DropDownList)gridview.Rows[otherRow].FindControl("ddlBranch");
                            ddlBranch.ForeColor = Color.Red;
                            ddlBranch.BackColor = Color.Yellow;
                            DropDownList ddlSubInsp = (DropDownList)gridview.Rows[otherRow].FindControl("ddlSubInsp");
                            ddlSubInsp.ForeColor = Color.Red;
                            ddlSubInsp.BackColor = Color.Yellow;
                            DropDownList ddlInspTyp = (DropDownList)gridview.Rows[otherRow].FindControl("ddlInspTyp");
                            ddlInspTyp.ForeColor = Color.Red;
                            ddlInspTyp.BackColor = Color.Yellow;
                            TextBox txtStDt = (TextBox)gridview.Rows[otherRow].FindControl("txtStDt");
                            txtStDt.ForeColor = Color.Red;
                            txtStDt.BackColor = Color.Yellow;
                            TextBox txtEndDt = (TextBox)gridview.Rows[otherRow].FindControl("txtEndDt");
                            txtEndDt.ForeColor = Color.Red;
                            txtEndDt.BackColor = Color.Yellow;
                            TextBox txtCmplDt = (TextBox)gridview.Rows[otherRow].FindControl("txtCmplDt");
                            txtCmplDt.ForeColor = Color.Red;
                            txtCmplDt.BackColor = Color.Yellow;
                            gridview.Rows[otherRow].BackColor = Color.Yellow;
                            gridview.Rows[otherRow].ForeColor = Color.Red;
                        }
                        else
                        {
                            btnSave.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vPlanId = 0;
            DataTable dt = null;
            DataTable dtDtl = null;
            DataSet ds = null;
            CInsPlan oAu = null;
            DataRow dr;
            try
            {
                vPlanId = Convert.ToInt32(e.CommandArgument);
                ViewState["AdPlanId"] = vPlanId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    foreach (GridViewRow gr in gvEmp.Rows)
                    {
                        LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                        lb.ForeColor = System.Drawing.Color.Black;
                    }
                    btnShow.ForeColor = System.Drawing.Color.Red;
                    oAu = new CInsPlan();
                    ds = oAu.GetInsPlanByID(vPlanId);
                    dt = ds.Tables[0];
                    dtDtl = ds.Tables[1];
                    ViewState["DataTbl"] = dtDtl; 
                    if (dt.Rows.Count > 0)
                    {
                        txtPlanDt.Text = Convert.ToString(dt.Rows[0]["PlanDt"]);
                        ddlInsp.SelectedIndex = ddlInsp.Items.IndexOf(ddlInsp.Items.FindByValue(dt.Rows[0]["EOID"].ToString()));                       
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                    if (dtDtl.Rows.Count > 0)
                    {
                        dr = dtDtl.NewRow();
                        dr["SlNo"] = Convert.ToInt32(dtDtl.Rows[dtDtl.Rows.Count - 1]["SlNo"]) + 1;
                        dr["Branch"] = "";
                        dr["EOID"] = "";
                        dr["InspTyp"] = "";
                        dr["StartDt"] = "";
                        dr["EndDt"] = "";
                        dr["CmpDueDt"] = "";
                        dr["ActCmpDt"] = "";
                        dr["StartDt1"] = "";
                        dr["EndDt1"] = "";                       
                        dtDtl.Rows.InsertAt(dr, dtDtl.Rows.Count + 1);
                        ViewState["DataTbl"] = dtDtl;
                        gvBranch.DataSource = dtDtl;
                        gvBranch.DataBind();                       
                    }
                }
            }
            finally
            {
                dt = null;
                dtDtl = null;
                ds = null;
                oAu = null;
            }
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {           
            DataTable dtBr = null;            
            DataTable dtEo = null;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Bidn Branch
                    dtBr = (DataTable)ViewState["Branch"];
                    DropDownList ddlBrnch = (DropDownList)e.Row.FindControl("ddlBranch");
                    ddlBrnch.DataSource = dtBr;
                    ddlBrnch.DataTextField = "BranchName";
                    ddlBrnch.DataValueField = "BranchCode";
                    ddlBrnch.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlBrnch.Items.Insert(0, oli);
                    ddlBrnch.SelectedIndex = ddlBrnch.Items.IndexOf(ddlBrnch.Items.FindByValue(e.Row.Cells[10].Text.Trim()));

                    //Bind Sub Inspector
                    dtEo = (DataTable)ViewState["SubInsp"];
                    DropDownList ddlInspector = (DropDownList)e.Row.FindControl("ddlSubInsp");
                    ddlInspector.DataSource = dtEo;
                    ddlInspector.DataTextField = "EoName";
                    ddlInspector.DataValueField = "EoId";
                    ddlInspector.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlInspector.Items.Insert(0, oLi);
                    ddlInspector.SelectedIndex = ddlInspector.Items.IndexOf(ddlInspector.Items.FindByValue(e.Row.Cells[11].Text.Trim()));

                    //Inspection Type
                    DropDownList ddlInspTyp = (DropDownList)e.Row.FindControl("ddlInspTyp");
                    ddlInspTyp.SelectedIndex = ddlInspTyp.Items.IndexOf(ddlInspTyp.Items.FindByValue(e.Row.Cells[12].Text.Trim()));                                       
                }
            }
            finally
            {
                dtBr = null;
                dtEo = null;
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vRow = 0, vMaxRow = 0;
            GridViewRow Row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            vRow = Row.RowIndex;
            vMaxRow = gvBranch.Rows.Count;
            DataTable dtDtl = null;
            dtDtl = (DataTable)ViewState["DataTbl"];            
            if (e.CommandName == "cmdNewRec")
            {                   
                AddNewRowToGrid();                    
            }
            if (e.CommandName == "cmdDelRec")
            {
                dtDtl.Rows.RemoveAt(vRow);
                dtDtl.AcceptChanges();
                ViewState["DataTbl"] = dtDtl;
                gvBranch.DataSource = dtDtl;
                gvBranch.DataBind();
            }                    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExit_Click(object sender, EventArgs e)
        {
            ViewState["StateEdit"] = null;
            ViewState["State"] = null;
            ViewState["Branch"] = null;
            ViewState["SubInsp"] = null;
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (this.CanAdd == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Add);
                return;
            }
            ViewState["StateEdit"] = "Add";
            tbEmp.ActiveTabIndex = 1;
            StatusButton("Add");
            ClearControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string vEoId = Convert.ToString(ViewState["EoId"]);
            if (this.CanEdit == "N")
            {
                gblFuction.MsgPopup(MsgAccess.Edit);
                return;
            }
            tbEmp.ActiveTabIndex = 1;
            ViewState["StateEdit"] = "Edit";
            StatusButton("Edit");           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbEmp.ActiveTabIndex = 0;
            ClearControls();
            LoadGrid(0);
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
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                EnableControl(false);
            }
        }
         
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            string vXml = string.Empty;
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Int32 vAdPlanID = Convert.ToInt32(ViewState["AdPlanId"]);
            Int32 vErr = 0, vUserID=0;
            DateTime vPlanDt = gblFuction.setDate(txtPlanDt.Text);
            CInsPlan oPln = null;           
            DataTable dt = null;
            try
            {
                dt = (DataTable)ViewState["DataTbl"];
                if (dt.Rows.Count <= 0)
                {
                    gblFuction.MsgPopup("Please enter valid data.");
                    return false;
                }
                dt.TableName = "Table1";
                vXml = DtToXml(dt);
                vUserID = Int32.Parse(Session[gblValue.UserId].ToString()); 
                if (Mode == "Save")
                {
                    oPln = new CInsPlan();
                    string vRest = oPln.ChkDuoInsPlan(vXml);
                    if (vRest != "")
                    {
                        gblFuction.MsgPopup("Inspection Plan of " + vRest + " have been found.");
                        return false;
                    }
                    vErr = oPln.SaveInsPlan(ref vAdPlanID, vPlanDt, ddlInsp.SelectedValue, vUserID, vXml, "Save");
                    if (vErr > 0)
                    {
                        ViewState["AdPlanId"] = vAdPlanID;
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
                    oPln = new CInsPlan();                   
                    vErr = oPln.SaveInsPlan(ref vAdPlanID, vPlanDt, ddlInsp.SelectedValue, vUserID, vXml, "Edit");
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
                    oPln = new CInsPlan();
                    vErr = oPln.SaveInsPlan(ref vAdPlanID, vPlanDt, ddlInsp.SelectedValue, vUserID, vXml, "Delete");
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
                oPln = null;             
                dt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtXml"></param>
        /// <returns></returns>
        private string DtToXml(DataTable dtXml)
        {
            string vXml = string.Empty;
            try
            {
                foreach (DataRow dr in dtXml.Rows)
                {
                    dr["StartDt"] = gblFuction.setDate(dr["StartDt"].ToString());
                    dr["EndDt"] = gblFuction.setDate(dr["EndDt"].ToString());
                    dr["CmpDueDt"] = gblFuction.setDate(dr["CmpDueDt"].ToString());
                    dr["ActCmpDt"] = gblFuction.setDate(dr["ActCmpDt"].ToString());
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                return vXml;
            }
            finally
            {
                dtXml = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtEndDt_TextChanged(object sender, EventArgs e)
        {
            GridViewRow cR = (GridViewRow)((TextBox)sender).Parent.Parent;
            TextBox txtEndDt = (TextBox)cR.FindControl("txtEndDt");            
            TextBox txtCmplDt = (TextBox)cR.FindControl("txtCmplDt");
            DateTime vEDt = gblFuction.setDate(txtEndDt.Text);
            DateTime vCmpDt =vEDt.AddDays(10);
            
            string vDayNm = vCmpDt.DayOfWeek.ToString();
            if (vDayNm == "Sunday")
                vCmpDt = vEDt.AddDays(11);
            
            txtCmplDt.Text = gblFuction.putStrDate(vCmpDt);
        }
    }
}