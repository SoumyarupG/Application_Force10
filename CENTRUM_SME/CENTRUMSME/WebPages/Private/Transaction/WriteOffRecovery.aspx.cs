using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Drawing;
using System.IO;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class WriteOffRecovery : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtWOCollDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                hdUserID.Value = this.UserID.ToString();
                PopBranch();
                LoadGrid(); 
                StatusButton("View");
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Write-Off Recovery";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuWriteOffRecovery);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAdd.Visible = false;
                    //btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnDelete.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Write-Off Recovery", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
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
                    ClearControl();
                    break;
                case "Show":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["StateEdit"] = "Add";
                tabWritOff.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControl();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void EnableControl(Boolean Status)
        {

        }
        protected void gvWriteOffRec_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vLoanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vLoanId = Convert.ToString(e.CommandArgument);
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    Label lblSlNo = (Label)gvRow.FindControl("lblSlNo");
                    oCG = new CApplication();
                    dt = oCG.GetWOCollDtlByLoanId(vLoanId, Convert.ToInt32(lblSlNo.Text));
                    if (dt.Rows.Count > 0)
                    {
                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvWriteOffRec.Rows)
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
                        gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                        gvRow.ForeColor = System.Drawing.Color.White;
                        gvRow.Font.Bold = true;
                        btnShow.ForeColor = System.Drawing.Color.White;
                        btnShow.Font.Bold = true;
                        PopBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(Convert.ToString(dt.Rows[0]["BranchCode"])));
                        popCustForColl(dt.Rows[0]["BranchCode"].ToString());
                        txtWOCollDt.Text = dt.Rows[0]["WORecDate"].ToString();
                        ddlCust.SelectedIndex = ddlCust.Items.IndexOf(ddlCust.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustId"])));
                        POPLoanIdForView(Convert.ToString(dt.Rows[0]["CustId"]));
                        ddlLoanNo.SelectedIndex = ddlLoanNo.Items.IndexOf(ddlLoanNo.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanId"])));
                        txtLnDate.Text = dt.Rows[0]["LoanDt"].ToString();
                        txtLoanAmt.Text = dt.Rows[0]["DisbAmt"].ToString();
                        txtWODate.Text = dt.Rows[0]["WriteOffDate"].ToString();
                        txtWOAmt.Text = dt.Rows[0]["WOAmt"].ToString();
                        txtWODue.Text = dt.Rows[0]["BalanceAmt"].ToString();
                        txtColl.Text = dt.Rows[0]["CollAmt"].ToString();
                        txtCollectedBy.Text = dt.Rows[0]["CollectedBy"].ToString();
                        hdLnTypeId.Value = dt.Rows[0]["LoanTypeId"].ToString();
                        hdSlNo.Value = dt.Rows[0]["SlNo"].ToString();
                        tabWritOff.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tabWritOff.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            //string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            //if (vStateEdit == "Add" || vStateEdit == null)
            //    vStateEdit = "Save";
            string vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                LoadGrid();
                tabWritOff.ActiveTabIndex = 0;
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControl();
            }
        }
        private void LoadGrid()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetWOCollList();
                if (dt.Rows.Count > 0)
                {
                    gvWriteOffRec.DataSource = dt;
                    gvWriteOffRec.DataBind();
                }
                else
                {
                    gvWriteOffRec.DataSource = null;
                    gvWriteOffRec.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }
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
        private Boolean SaveRecords(string Mode)
        {
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            //string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBrCode = ddlBranch.SelectedValue.ToString();
            string vLnId = "", vCustId = "",vCustName = "", vDescId = "C0001",vCollMode="C",vCollectedBy="";
            decimal  vCollAmt = 0,vWODue=0;
            Int32 pLnTypeId=0,pSlNo=0;
            Int32 vErr = 0;

            string vXmlAC = string.Empty;
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            string vFYear = Session[gblValue.FinYear].ToString();
            string vNarationL = string.Empty;
            CDisburse oLD = new CDisburse();
            try
            {
                if (txtWOCollDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Write-Off Collection Date can not be left blank...");
                    return false;
                }
                DateTime vWOCollDt = gblFuction.setDate(txtWOCollDt.Text);

                if (vWOCollDt < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || vWOCollDt > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Write-Off Collection Date Should Be in Financial Year...");
                    return false;
                }
                
                if (((Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string) == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Customer ....");
                    return false;
                }
                if (((Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string) != "-1")
                {
                    vLnId = (Request[ddlLoanNo.UniqueID] as string == null) ? ddlLoanNo.SelectedValue : Request[ddlLoanNo.UniqueID] as string;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please Select Loan No ....");
                    return false;
                }
                vCustId = Convert.ToString((Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string);
                vCustName = ddlCust.SelectedItem.Text.Replace("'","");
                vCollectedBy = Convert.ToString((Request[txtCollectedBy.UniqueID] as string == null) ? txtCollectedBy.Text : Request[txtCollectedBy.UniqueID] as string).Replace("'", "");
                if (vCollectedBy == "")
                {
                    gblFuction.AjxMsgPopup("Collected By Can Not Be Blank..");
                    return false;
                }
                if (txtColl.Text == "")
                {
                    gblFuction.AjxMsgPopup("Write-Off Collection Amount can not be left blank...");
                    return false;
                }
                if (Convert.ToDouble(txtColl.Text)<=0)
                {
                    gblFuction.AjxMsgPopup("Write-Off Collection Amount zero...");
                    return false;
                }
                else
                {
                    vCollAmt = Convert.ToDecimal((Request[txtColl.UniqueID] as string == null) ? txtColl.Text : Request[txtColl.UniqueID] as string);
                }
                vWODue = Convert.ToDecimal((Request[txtWODue.UniqueID] as string == null) ? txtWODue.Text : Request[txtWODue.UniqueID] as string);
                pLnTypeId = Convert.ToInt32((Request[hdLnTypeId.UniqueID] as string == null) ? hdLnTypeId.Value : Request[hdLnTypeId.UniqueID] as string);
                pSlNo = Convert.ToInt32((Request[hdSlNo.UniqueID] as string == null) ? hdSlNo.Value : Request[hdSlNo.UniqueID] as string);
                string vWORecAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.WriteOffRecAC, pLnTypeId, vBrCode);
                if (vWORecAc == "")
                {
                    gblFuction.AjxMsgPopup("Please Set Write-Off Received A/C in Loan Parameter..");
                    return false;
                }

                DataTable dtAccount = new DataTable();
                DataRow dr;
                DataColumn dc = new DataColumn();
                dc.ColumnName = "DtlId";
                dtAccount.Columns.Add(dc);
                DataColumn dc1 = new DataColumn();
                dc1.ColumnName = "DescId";
                dtAccount.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn();
                dc2.ColumnName = "DC";
                dtAccount.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn();
                dc3.ColumnName = "Amt";
                dc3.DataType = System.Type.GetType("System.Decimal");
                dtAccount.Columns.Add(dc3);
                dtAccount.TableName = "Table1";
                if (vCollAmt > 0)
                {
                    int i = 1;
                    dr = dtAccount.NewRow();
                    dr["DescId"] = vDescId;
                    dr["DC"] = "D";
                    dr["Amt"] = vCollAmt;
                    dr["DtlId"] = i;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                    i = i + 1;
                    dr = dtAccount.NewRow();
                    dr["DescId"] = vWORecAc;
                    dr["DC"] = "C";
                    dr["Amt"] = vCollAmt;
                    dr["DtlId"] = i;
                    dtAccount.Rows.Add(dr);
                    dtAccount.AcceptChanges();

                }
                vXmlAC = DataTableTOXml(dtAccount);
                vNarationL = "Being the Amount Collected against Write-Off of Loan No- " + vLnId + " of " + vCustName;
                
                if (Mode == "Save")
                {

                    CLoanRecovery oLR = new CLoanRecovery();
                    DataTable dtMaxCollDt = new DataTable();
                    dtMaxCollDt = oLR.GetWOLastCollDate(vLnId);
                    if (dtMaxCollDt.Rows.Count > 0)
                    {
                        DateTime MaxCollDate = gblFuction.setDate(dtMaxCollDt.Rows[0]["MaxCollDt"].ToString());
                        if (vWOCollDt < MaxCollDate)
                        {
                            gblFuction.AjxMsgPopup("You can not take Write-Off collection before Last Collection Date");
                            return false;
                        }
                    }

                    vErr = oLD.SaveWriteOffColl(vWOCollDt, vLnId, pSlNo, vCustId, vCollMode, vBrCode, this.UserID, vTblMst, vTblDtl, vFinYear, vXmlAC, vNarationL, vCollectedBy, vWODue, vCollAmt,"Save");
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                if (Mode == "Delete")
                {
                    CLoanRecovery oLR = new CLoanRecovery();
                    DataTable dtMaxCollDt = new DataTable();
                    dtMaxCollDt = oLR.GetWOLastCollDate(vLnId);
                    if (dtMaxCollDt.Rows.Count > 0)
                    {
                        DateTime MaxCollDate = gblFuction.setDate(dtMaxCollDt.Rows[0]["MaxCollDt"].ToString());
                        if (vWOCollDt != MaxCollDate)
                        {
                            gblFuction.AjxMsgPopup("Only Last Collection Can Be Deleted..");
                            return false;
                        }
                    }
                    String FYear = GetCurrentFinancialYear(vWOCollDt);
                    if (vFYear != FYear)
                    {
                        gblFuction.AjxMsgPopup("Collection Details can not be deleted as Collection Date is not in same Login  Financial Year...");
                        return false;
                    }
                    vErr = oLD.SaveWriteOffColl(vWOCollDt, vLnId, pSlNo, vCustId, vCollMode, vBrCode, this.UserID, vTblMst, vTblDtl, vFinYear, vXmlAC, vNarationL, vCollectedBy, vWODue, vCollAmt, "Delete");
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
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
               oLD = null;
            }
        }
        private void ClearControl()
        {
            txtWOCollDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            ddlCust.SelectedIndex = -1;
            ddlLoanNo.SelectedIndex = -1;
            txtLnDate.Text = "";
            txtLoanAmt.Text = "";
            txtWODate.Text = "";
            txtWOAmt.Text = "";
            txtWODue.Text = "";
            txtColl.Text = "";
            txtCollectedBy.Text = "";
        }
        private void popCustomer(string pBrCode)
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetCustForWriteOffRec(pBrCode);
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CustName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        private void popCustForColl(string pBrCode)
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.PopCustForWriteOffRec(pBrCode);
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CustName";
                ddlCust.DataValueField = "CustId";
                ddlCust.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlCust.Items.Insert(0, oli);
            }
            finally
            {
                oCD = null;
                dt = null;
            }
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = "";
            if (ddlCust.SelectedValue != "-1")
            {
                pCustId = ddlCust.SelectedValue.ToString();
            }
            else
            {
                gblFuction.AjxMsgPopup("Please Select Customer To Get Loan No...");
                return;
            }
            POPLoanId(pCustId);
        }
        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                string pLoanId = ddlLoanNo.SelectedValue.ToString();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oMem = new CLoanRecovery();
                dt = oMem.GetWODtlByLoanId(pLoanId);
                if (dt.Rows.Count > 0)
                {
                    txtLnDate.Text = dt.Rows[0]["LoanDt"].ToString();
                    txtLoanAmt.Text = dt.Rows[0]["DisbAmt"].ToString();
                    txtWODate.Text = dt.Rows[0]["WriteOffDate"].ToString();
                    txtWOAmt.Text = dt.Rows[0]["WOAmt"].ToString();
                    txtWODue.Text = dt.Rows[0]["BalAmt"].ToString();
                    txtColl.Text = dt.Rows[0]["CollAmt"].ToString();
                    hdLnTypeId.Value = dt.Rows[0]["LoanTypeId"].ToString();
                    hdSlNo.Value = dt.Rows[0]["SlNo"].ToString();
                }
                else
                {
                    txtLnDate.Text = "";
                    txtLoanAmt.Text = "";
                    txtWODate.Text = "";
                    txtWOAmt.Text = "";
                    txtWODue.Text = "";
                    txtColl.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    StatusButton("Delete");
                    LoadGrid();
                    ClearControl();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void POPLoanId(string pCustId)
        {
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oMem = new CLoanRecovery();
                dt = oMem.GetLoanNoForWriteOffRec(pCustId);
                ddlLoanNo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlLoanNo.Items.Insert(0, oLi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        protected void POPLoanIdForView(string pCustId)
        {
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {

                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oMem = new CLoanRecovery();
                dt = oMem.GetWriteOffCollLoanNo(pCustId);
                ddlLoanNo.Items.Clear();
                if (dt.Rows.Count > 0)
                {
                    ddlLoanNo.DataTextField = "LoanNo";
                    ddlLoanNo.DataValueField = "LoanId";
                    ddlLoanNo.DataSource = dt;
                    ddlLoanNo.DataBind();
                    ListItem oLi = new ListItem("<--Select-->", "-1");
                    ddlLoanNo.Items.Insert(0, oLi);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        public static string GetCurrentFinancialYear(DateTime dt)
        {
            int CurrentYear = dt.Year;
            int PreviousYear = dt.Year - 1;
            int NextYear = dt.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (dt.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
        private void PopBranch()
        {
            CMember oCM = null;
            DataTable dt = null;
            oCM = new CMember();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oCM.GetBranch();
                if (dt.Rows.Count > 0)
                {
                    ddlBranch.DataTextField = "BranchName";
                    ddlBranch.DataValueField = "BranchCode";
                    ddlBranch.DataSource = dt;
                    ddlBranch.DataBind();
                    ListItem oItm = new ListItem("<--Select-->", "-1");
                    ddlBranch.Items.Insert(0, oItm);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oCM = null;
            }
        }
        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBranch.SelectedValue == "-1")
            {
                gblFuction.AjxMsgPopup("Select Branch to Load Customer");
                return;
            }
            popCustomer(ddlBranch.SelectedValue.ToString());
        }
    }
}
