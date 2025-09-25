using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using FORCEBA;
using FORCECA;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class Woffdecl : CENTRUMBase
    {
        protected int cPgNo = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtFrmDt.Text = Session[gblValue.LoginDate].ToString();
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                txtWofDt.Text = Session[gblValue.LoginDate].ToString();
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();

                PopBranch(Session[gblValue.UserName].ToString());
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Session[gblValue.BrnchCode].ToString()));
                    ddlBranch.Enabled = false;
                }
                else
                {
                    ddlBranch.Enabled = true;
                }
                popRO();
                //PopGroup("");
                //PopReason();
                LoadGrid(0);
                tabWoff.ActiveTabIndex = 0;
                StatusButton("View");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUser"></param>
        /// 
        private void PopBranch(string pUser)
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(pUser, Convert.ToInt32(Session[gblValue.RoleId]));
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataBind();
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
                else
                {
                    ListItem liSel = new ListItem("<--- Select --->", "-1");
                    ddlBranch.Items.Insert(0, liSel);
                }
            }
            finally
            {
                dt = null;
                oUsr = null;
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
                this.PageHeading = "Bad Debt Written Off";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString()+" - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuWritOf);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnEdit.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Write-off", false);
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
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControls();
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    //gblFuction.focus("ctl00_cph_Main_tabLnScheme_pnlDtl_txtLnScheme");
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    ClearControls();
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
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
        private void ClearControls()
        {
            ddlBranch.SelectedIndex = -1;
            ddlCo.SelectedIndex = -1;
            ddlCenter.SelectedIndex = -1;
            txtPrinOs.Text = "0";
            ddlGrp.SelectedIndex = -1;
            txtWAmt.Text = "0";
            ddlMem.SelectedIndex = -1;
            txtWofDt.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            ddlLoan.SelectedIndex = -1;
            txtReason.Text = "";
            lblUser.Text = "";
            lblDate.Text = "";
        }

        
        private void EnableControl(bool Status)
        {
            ddlBranch.Enabled = Status;
            ddlCo.Enabled = Status;
            //txtPrinOs.Enabled=Status;
            ddlGrp.Enabled = Status;
            //txtWAmt.Enabled=Status;
            ddlMem.Enabled = Status;
            txtWofDt.Enabled = Status;
            ddlLoan.Enabled = Status;
            txtReason.Enabled = Status;
            ddlCenter.Enabled = Status;
        }

        private void popRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode;
            if (Session[gblValue.BrnchCode].ToString() != "0000")
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
            }
            else
            {
                vBrCode = ddlBranch.SelectedValue.ToString();
            }

            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);
                ddlCo.DataSource = dt;
                ddlCo.DataTextField = "EoName";
                ddlCo.DataValueField = "EoId";
                ddlCo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void PopCenter(string vEOID)
        {
            ddlMem.Items.Clear();
            ddlGrp.Items.Clear();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            string vBrCode = "";
            Int32 vBrId = 0;
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                vBrId = Convert.ToInt32(vBrCode);
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("D", "N", "AA", "MarketID", "Market", "MarketMSt", vEOID, "EOID", "Tra_DropDate", vLogDt, vBrCode);
                ddlCenter.DataSource = dt;
                ddlCenter.DataTextField = "Market";
                ddlCenter.DataValueField = "MarketID";
                ddlCenter.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCenter.Items.Insert(0, oli);
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
        /// <param name="vCOID"></param>
        private void PopGroup(string vMarketID)
        {
            DataTable dtGr = null;
            CGblIdGenerator oGbl = null;
            try
            {
                ddlGrp.Items.Clear();
                string vBrCode="";
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oGbl = new CGblIdGenerator();
                dtGr = oGbl.PopComboMIS("S", "N", "AA", "GroupID", "GroupName", "GroupMst", vMarketID, "MarketID", "DropoutDt", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vBrCode);
                if (dtGr.Rows.Count > 0)
                {
                    ddlGrp.DataSource = dtGr;
                    ddlGrp.DataTextField = "GroupName";
                    ddlGrp.DataValueField = "GroupID";
                    ddlGrp.DataBind();
                }
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlGrp.Items.Insert(0, oLi);
            }
            finally
            {
                dtGr = null;
                oGbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlGrp_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopMember();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PopMember()
        {
            ddlMem.Items.Clear();
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DataTable dt = null;
            try
            {
                CMember oMem = new CMember();
                if (Convert.ToString(ddlCo.SelectedValue) != "-1")
                {
                    string vBrCode = "";
                    if (Session[gblValue.BrnchCode].ToString() != "0000")
                    {
                        vBrCode = Session[gblValue.BrnchCode].ToString();
                    }
                    else
                    {
                        vBrCode = ddlBranch.SelectedValue.ToString();
                    }
                    dt = oMem.PopGrpMember(Convert.ToString(ddlGrp.SelectedValue), vBrCode, vLoginDt, "M");
                    ddlMem.DataTextField = "MemberName";
                    ddlMem.DataValueField = "MemberId";
                    ddlMem.DataSource = dt;
                    ddlMem.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlMem.Items.Insert(0, oItm);
                }
                ddlMem.Focus();
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
        protected void ddlMem_SelectedIndexChanged(object sender, EventArgs e)
        {
            popLoan();
        }

        private void popLoan()
        {
            DataTable dt = null;
            CWriteOff oCG = null;
            string vMemId = "";
            try
            {
                vMemId = ddlMem.SelectedValue;
                string vBrCode = "";
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oCG = new CWriteOff();
                dt = oCG.GetActvLoanMemWise(vMemId, vBrCode, gblFuction.setDate(txtWofDt.Text));
                ddlLoan.DataSource = dt;
                ddlLoan.DataTextField = "LoanNo";
                ddlLoan.DataValueField = "LoanId";
                ddlLoan.DataBind();
                ListItem oLi = new ListItem("<--Select-->", "-1");
                ddlLoan.Items.Insert(0, oLi);
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCo_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopCenter(ddlCo.SelectedValue);
        }

        protected void ddlCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex > 0) PopGroup(ddlCenter.SelectedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLoan_SelectedIndexChanged(object sender, EventArgs e)
        {
            CWriteOff oWF = null;
            DataTable dt = null;
            string vLoanId;
            string vBrCode = "";
            try
            {
                vLoanId = Convert.ToString(ddlLoan.SelectedValue);
                
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                oWF = new CWriteOff();
                dt = oWF.GetWOffAmtByLoanId(vLoanId, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    txtPrinOs.Text = dt.Rows[0]["PrincDue"].ToString();
                    txtWAmt.Text = dt.Rows[0]["PrincDue"].ToString();
                    hdOff.Value = dt.Rows[0]["IsWriteoff"].ToString();
                }
                else
                {
                    txtPrinOs.Text = "0";
                    txtWAmt.Text = "0";
                    hdOff.Value = "Y";
                }
            }
            finally
            {
                oWF = null;
                dt = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedIndex > 0) popRO();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pPgIndx"></param>
        private void LoadGrid(Int32 pPgIndx)
        {
            DataTable dt = null;
            CWriteOff oWF = null;
            Int32 vRows = 0;
            string vBrCode = string.Empty;
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                DateTime vFrmDt = gblFuction.setDate(txtFrmDt.Text);
                DateTime vToDt = gblFuction.setDate(txtToDt.Text);
                oWF = new CWriteOff();
                dt = oWF.GetWriteOffList(vFrmDt, vToDt, pPgIndx, ref vRows);
                gvWoff.DataSource = dt.DefaultView;
                gvWoff.DataBind();
                lblTotalPages.Text = CalTotPgs(vRows).ToString();
                lblCurrentPage.Text = cPgNo.ToString();
                if (cPgNo == 0)
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
                    if (cPgNo == Int32.Parse(lblTotalPages.Text))
                        Btn_Next.Enabled = false;
                    else
                        Btn_Next.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oWF = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRows"></param>
        /// <returns></returns>
        private int CalTotPgs(double pRows)
        {
            //int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            int totPg = (int)Math.Ceiling(pRows / 15);
            return totPg;
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
                    cPgNo = Int32.Parse(lblTotalPages.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadGrid(cPgNo);
            tabWoff.ActiveTabIndex = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvWoff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vAppId = 0;
            DataTable dt = null;
            CWriteOff oWF = null;
            try
            {
                vAppId = Convert.ToInt32(e.CommandArgument);
                ViewState["AppId"] = vAppId;
                if (e.CommandName == "cmdShow")
                {
                    oWF = new CWriteOff();
                    dt = oWF.GetWriteOffDetails(vAppId);
                    if (dt.Rows.Count > 0)
                    {
                        txtPrinOs.Text = dt.Rows[0]["PrinAmt"].ToString();
                        txtWAmt.Text = dt.Rows[0]["PrinAmt"].ToString();
                        txtWofDt.Text = dt.Rows[0]["WriteOffDate"].ToString();
                        txtAppDt.Text = dt.Rows[0]["AppvlDt"].ToString();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        popRO();
                        ddlCo.SelectedIndex = ddlCo.Items.IndexOf(ddlCo.Items.FindByValue(dt.Rows[0]["EoID"].ToString()));
                        PopCenter(ddlCo.SelectedValue);
                        ddlCenter.SelectedIndex = ddlCenter.Items.IndexOf(ddlCenter.Items.FindByValue(dt.Rows[0]["MarketID"].ToString()));
                        PopGroup(ddlCenter.SelectedValue);
                        ddlGrp.SelectedIndex = ddlGrp.Items.IndexOf(ddlGrp.Items.FindByValue(dt.Rows[0]["GroupID"].ToString()));
                        PopMember();
                        ddlMem.SelectedIndex = ddlMem.Items.IndexOf(ddlMem.Items.FindByValue(dt.Rows[0]["MemberID"].ToString()));
                        popLoan();
                        ddlLoan.SelectedIndex = ddlLoan.Items.IndexOf(ddlLoan.Items.FindByValue(dt.Rows[0]["LoanId"].ToString()));
                        txtReason.Text = dt.Rows[0]["Reason"].ToString();
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        tabWoff.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oWF = null;
            }
        }       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string DataTableTOXml(DataTable dt)
        {
            string sXml = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                sXml = oSW.ToString();
            }
            return sXml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Mode"></param>
        /// <returns></returns>
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            DataTable dt = null;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty;
            string vXmlLoan = string.Empty, vXmlAC = string.Empty, vDescID = string.Empty;
            Int32  vAppId = 0, vErr = 0, vYrNo = 0;
            double vLnAmt = 0, vPrinAmt = 0, vIntAmt = 0;
            string vLoanId = "", vLoanAc = "", vWriteOffAC = "", vTblMst = "", vTblDtl = "", vFinYear = "";
            DateTime vAppDt = gblFuction.setDate(txtWofDt.Text);
            DateTime vAppvlDt = gblFuction.setDate(txtAppDt.Text);
            string vNaration = "Being the Amt of Loan Write-off ";
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            CWriteOff oWF = null;
            CDisburse oLD = null;
            Int32 vLnTypId = 0;
            CGenParameter oGP = null;
            try
            {
                if (hdOff.Value == "Y")
                {
                    gblFuction.MsgPopup("The Loan has been Bad Debt Written Off.");
                    return false;
                }
                if (Convert.ToDouble(txtWAmt.Text) <= 0)
                {
                    gblFuction.MsgPopup("Bad Debt Written Off Amt should grater than zero...");
                    return false;
                }
                if (gblFuction.setDate(txtWofDt.Text) > vLoginDt)
                {
                    gblFuction.MsgPopup("Bad Debt Written Off date should not grater than login date...");
                    txtWofDt.Text = Session[gblValue.LoginDate].ToString();
                    return false;
                }
                oLD = new CDisburse();
                vLoanId = ddlLoan.SelectedValue;
                dt = oLD.GetAllLoanByLoanId(vLoanId);
                if (dt.Rows.Count > 0)
                {
                    vLnTypId = Convert.ToInt32(dt.Rows[0]["LoanTypeId"].ToString());
                }
                
                oGP = new CGenParameter();
                dt = oGP.GetParameterDetails(vLnTypId, gblFuction.setDate(Session[gblValue.LoginDate].ToString()));
                if (dt.Rows.Count > 0)
                {
                    vLoanAc = dt.Rows[0]["LoanAC"].ToString().Trim();
                    vWriteOffAC = dt.Rows[0]["WriteOffAC"].ToString().Trim();
                }
                if (vLoanAc == "")
                {
                    gblFuction.MsgPopup("Loan A/c is not Set for this Loan Type");
                    return false;
                }
                if (vWriteOffAC == "")
                {
                    gblFuction.MsgPopup("Bad Debt Written Off A/c is not Set for this Loan Type");
                    return false;
                }
                if (vAppDt < vFinFrom || vAppDt > vFinTo)
                {
                    gblFuction.MsgPopup("Write Off Date should be Financial Year.");
                    return false;
                }
                //vReasonId = Convert.ToInt32(ddlReason.SelectedValue);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
                
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBrCode = Session[gblValue.BrnchCode].ToString();
                }
                else
                {
                    vBrCode = ddlBranch.SelectedValue.ToString();
                }
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = Convert.ToInt32(ViewState["AppId"]);
                vPrinAmt = Convert.ToDouble(txtPrinOs.Text);
                vLnAmt = Convert.ToDouble(txtWAmt.Text);
                vTblMst = Session[gblValue.ACVouMst].ToString();
                vTblDtl = Session[gblValue.ACVouDtl].ToString();
                vFinYear = Session[gblValue.ShortYear].ToString();

                //------Create Account table(Bank)----
                DataTable dtAccount = new DataTable();
                DataRow dr;
                DataColumn dc = new DataColumn("DC", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc);
                DataColumn dc1 = new DataColumn("Amt", System.Type.GetType("System.Decimal"));
                dtAccount.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn("DescId", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn("DtlId", System.Type.GetType("System.String"));
                dtAccount.Columns.Add(dc3);
                dtAccount.TableName = "Table1";
                if (vLnAmt > 0)
                {
                    dr = dtAccount.NewRow();
                    dr["DescId"] = vWriteOffAC;
                    dr["DC"] = "D";
                    dr["Amt"] = vLnAmt;
                    dr["DtlId"] = 1;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                    dr = dtAccount.NewRow();
                    dr["DescId"] = vLoanAc;
                    dr["DC"] = "C";
                    dr["Amt"] = vLnAmt;
                    dr["DtlId"] = 2;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();
                }
                vXmlAC = DataTableTOXml(dtAccount);

                if (Mode == "Save")
                {
                    if (this.RoleId != 1)
                    {
                        //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                        //{
                        //    gblFuction.AjxMsgPopup("You can not save, Day end already done..");
                        //    return false;
                        //}
                    }
                    oWF = new CWriteOff();
                    CCollectionRoutine oCC = new CCollectionRoutine();
                    dt = oCC.GetMaxCollDate(vLoanId, vBrCode, "M");
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToString(dt.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dt.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtWofDt.Text))
                        {
                            gblFuction.MsgPopup(" Last collection date is" + Convert.ToString(dt.Rows[0]["MaxCollDt"]));
                            return false;
                        }
                    }
                    vErr = oWF.InsertWriteOffMst(vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                        gblFuction.setDate(""), txtReason.Text.Replace("'","''"), vBrCode, this.UserID, "I", 0, vTblMst,
                        vTblDtl, vFinYear, vXmlAC,vAppvlDt, vNaration,"Save");
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
                //else if (Mode == "Edit")
                //{
                //    oWF = new CWriteOff();
                //    CCollectionRoutine oCC = new CCollectionRoutine();
                //    dt = oCC.GetMaxCollDate(vLoanId, vBrCode, "M");
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (Convert.ToString(dt.Rows[0]["MaxCollDt"]) != "01/01/1900" && gblFuction.setDate(Convert.ToString(dt.Rows[0]["MaxCollDt"]).ToString()) >= gblFuction.setDate(txtWofDt.Text))
                //        {
                //            gblFuction.MsgPopup(" Last collection date is" + Convert.ToString(dt.Rows[0]["MaxCollDt"]));
                //            return false;
                //        }
                //    }
                //    vErr = oWF.UpdateWriteOffMst(vAppId, vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                //        gblFuction.setDate(""), txtReason.Text.Replace("'", "''"), vBrCode, this.UserID, "E", 0, vTblMst,
                //        vTblDtl, vFinYear, vDescID, vXmlAC, "Edit", vNaration);
                //    if (vErr == 0)
                //        vResult = true;
                //    else
                //    {
                //        gblFuction.MsgPopup(gblMarg.DBError);
                //        vResult = false;
                //    }
                //}
                else if (Mode == "Delete")
                {
                    oWF = new CWriteOff();
                    //vErr = oWF.UpdateWriteOffMst(vAppId, vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                    //    gblFuction.setDate(""), txtReason.Text.Replace("'", "''"), vBrCode, this.UserID, "D", 0, vTblMst,
                    //    vTblDtl, vFinYear, vXmlAC, "Delet", vNaration);
                    vErr = oWF.InsertWriteOffMst(vLoanId, vAppDt, vPrinAmt, vIntAmt, vLnAmt, "O",
                        gblFuction.setDate(""), txtReason.Text.Replace("'", "''"), vBrCode, this.UserID, "I", 0, vTblMst,
                        vTblDtl, vFinYear, vXmlAC,vAppvlDt, vNaration, "Del");
                    if (vErr == 0)
                        vResult = true;
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
                oWF = null;
                oLD = null;
                oGP = null;
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
                ViewState["StateEdit"] = null;
                tabWoff.ActiveTabIndex = 1;
                StatusButton("Add");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                tabWoff.ActiveTabIndex = 0;
                EnableControl(false);
                StatusButton("View");
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
                if (this.RoleId != 1)
                {
                    //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
                    //{
                    //    gblFuction.AjxMsgPopup("You can not Delete, Day end already done..");
                    //    return;
                    //}
                }
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblMarg.DeleteMsg);
                    LoadGrid(0);
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                if (vStateEdit == "" || vStateEdit == null)
                    vStateEdit = "Save";
                if (SaveRecords(vStateEdit) == true)
                {
                    gblFuction.MsgPopup(gblMarg.SaveMsg);
                    LoadGrid(0);
                    StatusButton("View");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this.RoleId != 1)
        //        {
        //            //if (gblFuction.setDate(Session[gblValue.EndDate].ToString()) > gblFuction.setDate(txtWofDt.Text))
        //            //{
        //            //    gblFuction.AjxMsgPopup("You can not edit, Day end already done..");
        //            //    return;
        //            //}
        //        }
        //        if (this.CanEdit == "N")
        //        {
        //            gblFuction.MsgPopup(MsgAccess.Edit);
        //            return;
        //        }
        //        ViewState["StateEdit"] = "Edit";
        //        gblFuction.focus("ctl00_cph_Main_tabApp_pnlDtl_ddlCo");
        //        StatusButton("Edit");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


       
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
    }
}
