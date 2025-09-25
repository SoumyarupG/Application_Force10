using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class IntInspLUC : CENTRUMBase
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
                StatusButton("View");
                popBranch();                   
                LoadGrid(1);
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                tbEmp.ActiveTabIndex = 0;
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
                this.PageHeading = "Loan Utilization Check";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLUC);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Utilization Check", false);
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
        private void popBranch()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "BranchCode", "BranchName", "BranchMst", "", "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBranch.Items.Insert(0, oli);
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
                    ddlQ26.Enabled = false;
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
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    if (ddlQ25.SelectedIndex > 0)
                    {
                        if (ddlQ25.SelectedValue == "2") ddlQ26.Enabled = true;
                        else ddlQ26.Enabled = false;
                    }
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
            txtLucDt.Enabled = Status;
            txtLoanNo.Enabled = Status;         

            ddlQ1.Enabled = Status;             
            ddlQ2.Enabled = Status;             
            ddlQ3.Enabled = Status;             
            ddlQ4.Enabled = Status;            
            ddlQ5.Enabled = Status;            
            ddlQ6.Enabled = Status;           
            ddlQ7.Enabled = Status;             
            ddlQ8.Enabled = Status;           
            ddlQ9.Enabled = Status;            
            ddlQ10.Enabled = Status;            
            ddlQ11.Enabled = Status;            
            ddlQ12.Enabled = Status;            
            ddlQ13.Enabled = Status;            
            ddlQ14.Enabled = Status;             
            ddlQ15.Enabled = Status;            
            ddlQ16.Enabled = Status;            
            ddlQ17.Enabled = Status;             
            ddlQ18.Enabled = Status;            
            ddlQ19.Enabled = Status;            
            ddlQ20.Enabled = Status;            
            ddlQ21.Enabled = Status;            
            ddlQ22.Enabled = Status;            
            ddlQ23.Enabled = Status;             
            ddlQ24.Enabled = Status;            
            ddlQ25.Enabled = Status;             
            ddlQ26.Enabled = Status;             
            ddlQ27.Enabled = Status;
            ddlQ28.Enabled = Status;
            ddlQ29.Enabled = Status;
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearControls()
        {
            txtLucDt.Text = "";
            txtSubDt.Text = Session[gblValue.LoginDate].ToString();
            txtLoanNo.Text = "";
            lblBrnch.Text = "";
            lblRO.Text = "";
            lblCent.Text = "";
            lblGrp.Text = "";
            lblMem.Text = "";
            hdLoanId.Value = "0";
            hdMemId.Value = "0";
            hdBrCode.Value = "0";
            hdCentId.Value = "0";
            hdGrpId.Value = "0";
            hdMemId.Value = "0";
            ddlQ1.SelectedIndex = -1;            
            ddlQ2.SelectedIndex = -1;            
            ddlQ3.SelectedIndex = -1;            
            ddlQ4.SelectedIndex = -1;            
            ddlQ5.SelectedIndex = -1;            
            ddlQ6.SelectedIndex = -1;            
            ddlQ7.SelectedIndex = -1;            
            ddlQ8.SelectedIndex = -1;            
            ddlQ9.SelectedIndex = -1;            
            ddlQ10.SelectedIndex = -1;            
            ddlQ11.SelectedIndex = -1;            
            ddlQ12.SelectedIndex = -1;            
            ddlQ13.SelectedIndex = -1;            
            ddlQ14.SelectedIndex = -1;            
            ddlQ15.SelectedIndex = -1;            
            ddlQ16.SelectedIndex = -1;            
            ddlQ17.SelectedIndex = -1;            
            ddlQ18.SelectedIndex = -1;            
            ddlQ19.SelectedIndex = -1;            
            ddlQ20.SelectedIndex = -1;            
            ddlQ21.SelectedIndex = -1;            
            ddlQ22.SelectedIndex = -1;            
            ddlQ23.SelectedIndex = -1;            
            ddlQ24.SelectedIndex = -1;            
            ddlQ25.SelectedIndex = -1;            
            ddlQ26.SelectedIndex = -1;            
            ddlQ27.SelectedIndex = -1;
            ddlQ28.SelectedIndex = -1;
            ddlQ29.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
        }
                      

        /// <summary>
        /// 
        /// </summary>
        private void PopFilterMem()
        {
            ddlQ27.Items.Clear();
            ddlQ27.SelectedIndex = -1;

            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                if (Convert.ToString(hdGrpId.Value) != "0")
                {
                    dt = oMem.PopFilterMember(hdGrpId.Value, hdMemId.Value, hdBrCode.Value, vLoginDt);
                    ddlQ27.DataTextField = "MemberName";
                    ddlQ27.DataValueField = "MemberId";
                    ddlQ27.DataSource = dt;
                    ddlQ27.DataBind();
                    ListItem oItm = new ListItem("<--- Select --->", "-1");
                    ddlQ27.Items.Insert(0, oItm);
                }
                ddlQ27.Focus();
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ25_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQ25.SelectedValue == "2")
            {
                ddlQ26.Enabled = true;
            }
            else
            {
                ddlQ26.Enabled = false;
                ddlQ27.SelectedIndex = -1;
                ddlQ27.Items.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlQ26_SelectedIndexChanged(object sender, EventArgs e)
        {            
            PopFilterMem();            
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
            CIntIspLUC oAd = null;
            try
            {
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                string vBrCode = ddlBranch.SelectedValue; 

                oAd = new CIntIspLUC();
                dt = oAd.GetIntInspLUCPG(pPgIndx, vFrmDt, vToDt, vBrCode, ref vTotRows);
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
        protected void gvEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pInspID = 0;
            CIntIspLUC oAu = null;
            DataTable dt = null;

            try
            {
                pInspID = Convert.ToInt32(e.CommandArgument);
                ViewState["InsPecId"] = pInspID;
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
                    oAu = new CIntIspLUC();
                    dt = oAu.GetIntInspLUCById(pInspID);
                    if (dt.Rows.Count > 0)
                    {
                        txtSubDt.Text = Convert.ToString(dt.Rows[0]["SubDt"]);
                        txtLucDt.Text = Convert.ToString(dt.Rows[0]["LucPdcDt"]);    
                        txtLoanNo.Text = Convert.ToString(dt.Rows[0]["LoanNo"]);
                        lblBrnch.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                        lblRO.Text = Convert.ToString(dt.Rows[0]["EoName"]);
                        lblCent.Text = Convert.ToString(dt.Rows[0]["Market"]);
                        lblGrp.Text = Convert.ToString(dt.Rows[0]["GroupName"]);
                        lblMem.Text = Convert.ToString(dt.Rows[0]["MemName"]) + " " + Convert.ToString(dt.Rows[0]["MemberNo"]);
                        hdLoanId.Value = Convert.ToString(dt.Rows[0]["LoanId"]);
                        hdBrCode.Value = Convert.ToString(dt.Rows[0]["BranchCode"]);
                        hdRoId.Value = Convert.ToString(dt.Rows[0]["ROId"]);
                        hdCentId.Value = Convert.ToString(dt.Rows[0]["CenterId"]);
                        hdGrpId.Value = Convert.ToString(dt.Rows[0]["Groupid"]);
                        hdMemId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                        
                        ddlQ1.SelectedIndex = ddlQ1.Items.IndexOf(ddlQ1.Items.FindByValue(dt.Rows[0]["A1"].ToString()));
                        ddlQ2.SelectedIndex = ddlQ2.Items.IndexOf(ddlQ2.Items.FindByValue(dt.Rows[0]["A2"].ToString()));
                        ddlQ3.SelectedIndex = ddlQ3.Items.IndexOf(ddlQ3.Items.FindByValue(dt.Rows[0]["A3"].ToString()));
                        ddlQ4.SelectedIndex = ddlQ4.Items.IndexOf(ddlQ4.Items.FindByValue(dt.Rows[0]["A4"].ToString()));
                        ddlQ5.SelectedIndex = ddlQ5.Items.IndexOf(ddlQ5.Items.FindByValue(dt.Rows[0]["A5"].ToString()));
                        ddlQ6.SelectedIndex = ddlQ6.Items.IndexOf(ddlQ6.Items.FindByValue(dt.Rows[0]["A6"].ToString()));
                        ddlQ7.SelectedIndex = ddlQ7.Items.IndexOf(ddlQ7.Items.FindByValue(dt.Rows[0]["A7"].ToString()));
                        ddlQ8.SelectedIndex = ddlQ8.Items.IndexOf(ddlQ8.Items.FindByValue(dt.Rows[0]["A8"].ToString()));
                        ddlQ9.SelectedIndex = ddlQ9.Items.IndexOf(ddlQ9.Items.FindByValue(dt.Rows[0]["A9"].ToString()));
                        ddlQ10.SelectedIndex = ddlQ10.Items.IndexOf(ddlQ10.Items.FindByValue(dt.Rows[0]["A10"].ToString()));
                        ddlQ11.SelectedIndex = ddlQ11.Items.IndexOf(ddlQ11.Items.FindByValue(dt.Rows[0]["A11"].ToString()));
                        ddlQ12.SelectedIndex = ddlQ12.Items.IndexOf(ddlQ12.Items.FindByValue(dt.Rows[0]["A12"].ToString()));
                        ddlQ13.SelectedIndex = ddlQ13.Items.IndexOf(ddlQ13.Items.FindByValue(dt.Rows[0]["A13"].ToString()));
                        ddlQ14.SelectedIndex = ddlQ14.Items.IndexOf(ddlQ14.Items.FindByValue(dt.Rows[0]["A14"].ToString()));
                        ddlQ15.SelectedIndex = ddlQ15.Items.IndexOf(ddlQ15.Items.FindByValue(dt.Rows[0]["A15"].ToString()));
                        ddlQ16.SelectedIndex = ddlQ16.Items.IndexOf(ddlQ16.Items.FindByValue(dt.Rows[0]["A16"].ToString()));
                        ddlQ17.SelectedIndex = ddlQ17.Items.IndexOf(ddlQ17.Items.FindByValue(dt.Rows[0]["A17"].ToString()));
                        ddlQ18.SelectedIndex = ddlQ18.Items.IndexOf(ddlQ18.Items.FindByValue(dt.Rows[0]["A18"].ToString()));
                        ddlQ19.SelectedIndex = ddlQ19.Items.IndexOf(ddlQ19.Items.FindByValue(dt.Rows[0]["A19"].ToString()));
                        ddlQ20.SelectedIndex = ddlQ20.Items.IndexOf(ddlQ20.Items.FindByValue(dt.Rows[0]["A20"].ToString()));
                        ddlQ21.SelectedIndex = ddlQ21.Items.IndexOf(ddlQ21.Items.FindByValue(dt.Rows[0]["A21"].ToString()));
                        ddlQ22.SelectedIndex = ddlQ22.Items.IndexOf(ddlQ22.Items.FindByValue(dt.Rows[0]["A22"].ToString()));
                        ddlQ23.SelectedIndex = ddlQ23.Items.IndexOf(ddlQ23.Items.FindByValue(dt.Rows[0]["A23"].ToString()));
                        ddlQ24.SelectedIndex = ddlQ24.Items.IndexOf(ddlQ24.Items.FindByValue(dt.Rows[0]["A24"].ToString()));
                        ddlQ25.SelectedIndex = ddlQ25.Items.IndexOf(ddlQ25.Items.FindByValue(dt.Rows[0]["A25"].ToString()));
                        ddlQ26.SelectedIndex = ddlQ26.Items.IndexOf(ddlQ26.Items.FindByValue(dt.Rows[0]["A26"].ToString()));
                        PopFilterMem();
                        ddlQ27.SelectedIndex = ddlQ27.Items.IndexOf(ddlQ27.Items.FindByValue(dt.Rows[0]["A27"].ToString()));
                        ddlQ28.SelectedIndex = ddlQ28.Items.IndexOf(ddlQ28.Items.FindByValue(dt.Rows[0]["A28"].ToString()));
                        ddlQ29.SelectedIndex = ddlQ29.Items.IndexOf(ddlQ29.Items.FindByValue(dt.Rows[0]["A29"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tbEmp.ActiveTabIndex = 1;
                        StatusButton("Show");
                        EnableControl(false);
                    }
                }
            }
            finally
            {
                dt = null;
                oAu = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSerch_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            CIntIspLUC oLuc = null;
            try
            {
                oLuc = new CIntIspLUC();
                dt = oLuc.GetRecForLUCByLoanNo(txtLoanNo.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    txtLoanNo.Text = Convert.ToString(dt.Rows[0]["LoanNo"]);
                    lblBrnch.Text = Convert.ToString(dt.Rows[0]["BranchName"]);
                    lblRO.Text = Convert.ToString(dt.Rows[0]["EoName"]);
                    lblCent.Text = Convert.ToString(dt.Rows[0]["Market"]);
                    lblGrp.Text = Convert.ToString(dt.Rows[0]["GroupName"]);
                    lblMem.Text = Convert.ToString(dt.Rows[0]["MemName"]) + " " + Convert.ToString(dt.Rows[0]["MemberNo"]);
                    hdLoanId.Value = Convert.ToString(dt.Rows[0]["LoanId"]);
                    hdBrCode.Value = Convert.ToString(dt.Rows[0]["BranchCode"]);
                    hdRoId.Value = Convert.ToString(dt.Rows[0]["eoid"]);
                    hdCentId.Value = Convert.ToString(dt.Rows[0]["Marketid"]);
                    hdGrpId.Value = Convert.ToString(dt.Rows[0]["Groupid"]);
                    hdMemId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                }
                else
                {
                    txtLoanNo.Text = "No Record found.. ";
                    lblBrnch.Text = "";
                    lblRO.Text = "";
                    lblCent.Text = "";
                    lblGrp.Text = "";
                    lblMem.Text = "";
                    hdLoanId.Value = "0";
                    hdMemId.Value = "0";
                    hdBrCode.Value = "0";
                    hdCentId.Value = "0";
                    hdGrpId.Value = "0";
                    hdMemId.Value = "0";
                }
            }
            finally
            {
                dt = null;
                oLuc = null;
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
            string vBrCode = hdBrCode.Value, vROID = hdRoId.Value, vLoanId= hdLoanId.Value;
            string vCentID = hdCentId.Value, vGrpID = hdGrpId.Value, vMemID = hdMemId.Value, vQ27 = "";
            Int32 vInsPecId = 0, vErr = 0, vRec = 0, vQ26 = 0, vUserID=0;
            DateTime vSubDt = gblFuction.setDate(txtSubDt.Text);
            DateTime vLucDt = gblFuction.setDate(txtLucDt.Text);
            CIntIspLUC oAu = null;
            CGblIdGenerator oGbl = null;
            try
            {
                vUserID = Int32.Parse(Session[gblValue.UserId].ToString()); 
                if (vLucDt > vSubDt)
                {
                    gblFuction.MsgPopup("LUC Date should be earlier date of Submission Date.");
                    return false;                
                }

                if (ViewState["InsPecId"] != null) vInsPecId = Convert.ToInt32(Convert.ToString(ViewState["InsPecId"]));
                
                if (ddlQ26.SelectedIndex > 0) vQ26 = Convert.ToInt32(ddlQ26.SelectedValue);
                if (ddlQ27.SelectedIndex > 0) vQ27 = ddlQ27.SelectedValue;

                if (Mode == "Save")
                {
                    oAu = new CIntIspLUC();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("IntInspLoanMst", "LoanID", vLoanId, "InspType", "LUC", "InspID", vInsPecId.ToString(), "Save");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Duplicate Inspection...");
                        return false;
                    }

                    vErr = oAu.SaveInspLUC(ref vInsPecId, "LUC", vSubDt, vBrCode, vROID, vCentID, vGrpID, vMemID, vLoanId, vUserID, "Save", 
                        Convert.ToInt32(ddlQ1.SelectedValue), Convert.ToInt32(ddlQ2.SelectedValue), Convert.ToInt32(ddlQ3.SelectedValue), 
                        Convert.ToInt32(ddlQ4.SelectedValue), Convert.ToInt32(ddlQ5.SelectedValue), Convert.ToInt32(ddlQ6.SelectedValue), 
                        Convert.ToInt32(ddlQ7.SelectedValue), Convert.ToInt32(ddlQ8.SelectedValue), Convert.ToInt32(ddlQ9.SelectedValue), 
                        Convert.ToInt32(ddlQ10.SelectedValue), Convert.ToInt32(ddlQ11.SelectedValue), Convert.ToInt32(ddlQ12.SelectedValue), 
                        Convert.ToInt32(ddlQ13.SelectedValue), Convert.ToInt32(ddlQ14.SelectedValue), Convert.ToInt32(ddlQ15.SelectedValue), 
                        Convert.ToInt32(ddlQ16.SelectedValue), Convert.ToInt32(ddlQ17.SelectedValue), Convert.ToInt32(ddlQ18.SelectedValue), 
                        Convert.ToInt32(ddlQ19.SelectedValue), Convert.ToInt32(ddlQ20.SelectedValue), Convert.ToInt32(ddlQ21.SelectedValue), 
                        Convert.ToInt32(ddlQ22.SelectedValue), Convert.ToInt32(ddlQ23.SelectedValue), Convert.ToInt32(ddlQ24.SelectedValue), 
                        Convert.ToInt32(ddlQ25.SelectedValue), vQ26, vQ27, Convert.ToInt32(ddlQ28.SelectedValue), vLucDt,
                        Convert.ToInt32(ddlQ29.SelectedValue));
                    if (vErr > 0)
                    {
                        ViewState["InsPecId"] = vInsPecId;
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
                    oAu = new CIntIspLUC();
                    oGbl = new CGblIdGenerator();
                    vRec = oGbl.ChkDuplicate("IntInspLoanMst", "MemberId", vMemID, "InspType", "LUC", "InspID", vInsPecId.ToString(), "Edit");
                    if (vRec > 0)
                    {
                        gblFuction.MsgPopup("Duplicate Inspection...");
                        return false;
                    }

                    vErr = oAu.SaveInspLUC(ref vInsPecId, "LUC", vSubDt, vBrCode, vROID, vCentID, vGrpID, vMemID, vLoanId, vUserID, "Edit",
                        Convert.ToInt32(ddlQ1.SelectedValue), Convert.ToInt32(ddlQ2.SelectedValue), Convert.ToInt32(ddlQ3.SelectedValue),
                        Convert.ToInt32(ddlQ4.SelectedValue), Convert.ToInt32(ddlQ5.SelectedValue), Convert.ToInt32(ddlQ6.SelectedValue),
                        Convert.ToInt32(ddlQ7.SelectedValue), Convert.ToInt32(ddlQ8.SelectedValue), Convert.ToInt32(ddlQ9.SelectedValue),
                        Convert.ToInt32(ddlQ10.SelectedValue), Convert.ToInt32(ddlQ11.SelectedValue), Convert.ToInt32(ddlQ12.SelectedValue),
                        Convert.ToInt32(ddlQ13.SelectedValue), Convert.ToInt32(ddlQ14.SelectedValue), Convert.ToInt32(ddlQ15.SelectedValue),
                        Convert.ToInt32(ddlQ16.SelectedValue), Convert.ToInt32(ddlQ17.SelectedValue), Convert.ToInt32(ddlQ18.SelectedValue),
                        Convert.ToInt32(ddlQ19.SelectedValue), Convert.ToInt32(ddlQ20.SelectedValue), Convert.ToInt32(ddlQ21.SelectedValue),
                        Convert.ToInt32(ddlQ22.SelectedValue), Convert.ToInt32(ddlQ23.SelectedValue), Convert.ToInt32(ddlQ24.SelectedValue),
                        Convert.ToInt32(ddlQ25.SelectedValue), vQ26, vQ27, Convert.ToInt32(ddlQ28.SelectedValue), vLucDt,
                        Convert.ToInt32(ddlQ29.SelectedValue));
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
                    oAu = new CIntIspLUC();
                    //oGbl = new CGblIdGenerator();
                    //vRec = oGbl.ChkDeleteString(vEoId, "EOID", "MarketMSt");
                    //if (vRec > 0)
                    //{
                    //    gblFuction.MsgPopup("The RO has group, you can not delete the RO.");
                    //    return false;
                    //}
                    vErr = oAu.SaveInspLUC(ref vInsPecId, "", vSubDt, "", "", "", "", "", "", vUserID, "Del", 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, vLucDt, 0);
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
                oAu = null;
            }
        }  
    }
}