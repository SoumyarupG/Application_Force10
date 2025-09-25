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
    public partial class WriteOffDeclare : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                txtWODeclDt.Text = txtFromDt.Text = txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                //txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                hdUserID.Value = this.UserID.ToString();
                LoadGrid();
                PopBranch();
                // popCustomer();

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
                this.PageHeading = "Write-Off Declaration";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuWriteOffDecalre);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Write Off Declaration", false);
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
                    ClearControls();
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
                ClearControls();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtWODeclDt.Enabled = Status;
            ddlBranch.Enabled = Status;
            ddlCust.Enabled = Status;
            ddlLoanNo.Enabled = Status;
            txtLnDate.Enabled = Status;
            txtPOSAmt.Enabled = Status;
            txtLoanAmt.Enabled = Status;
            txtWOAmt.Enabled = Status;
        }
        private void ClearControls()
        {
            txtWODeclDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
            ddlBranch.SelectedIndex = -1;
            ddlCust.SelectedIndex = -1;
            ddlLoanNo.SelectedIndex = -1;
            txtLnDate.Text = "";
            txtPOSAmt.Text = "0";
            txtLoanAmt.Text = "0";
            txtWOAmt.Text = "0";
        }
        protected void gvWriteOff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vLoanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vLoanId = Convert.ToString(e.CommandArgument);
                ViewState["LnId"] = vLoanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetWriteOffDtlByLoanId(vLoanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvWriteOff.Rows)
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
                        popCustomer(dt.Rows[0]["BranchCode"].ToString());
                        ddlCust.SelectedIndex = ddlCust.Items.IndexOf(ddlCust.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustId"])));
                        POPLoanId(Convert.ToString(dt.Rows[0]["CustId"]));
                        ddlLoanNo.SelectedIndex = ddlLoanNo.Items.IndexOf(ddlLoanNo.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanId"])));
                        txtWODeclDt.Text = dt.Rows[0]["WODecDate"].ToString();
                        txtLnDate.Text = dt.Rows[0]["DisbDate"].ToString();
                        txtLoanAmt.Text = dt.Rows[0]["DisbAmt"].ToString();
                        txtPOSAmt.Text = dt.Rows[0]["WOAmt"].ToString();
                        txtWOAmt.Text = dt.Rows[0]["WOAmt"].ToString();
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
                ClearControls();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
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
                dt = oCA.GetWriteOffDeclList(gblFuction.setDate(txtFromDt.Text),gblFuction.setDate(txtToDt.Text),txtLoanNoSearch.Text);
                if (dt.Rows.Count > 0)
                {
                    gvWriteOff.DataSource = dt;
                    gvWriteOff.DataBind();
                }
                else
                {
                    gvWriteOff.DataSource = null;
                    gvWriteOff.DataBind();
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
            string vLnId = "", vWOLedger = "", vCustomernme = "";
            decimal vWOAmt = 0;
            Int32 pErr = 0, pLnTypeId = 0;
            Int32 vErr = 0;
            string vXmlAC = string.Empty;
            string vTblMst = Session[gblValue.ACVouMst].ToString();
            string vTblDtl = Session[gblValue.ACVouDtl].ToString();
            string vFinYear = Session[gblValue.ShortYear].ToString();
            string vNarationL = string.Empty;
            CDisburse oLD = new CDisburse();
            try
            {
                if (txtWODeclDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Write-Off Decalration Date can not be left blank...");
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
                if (txtWOAmt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Write-Off Amount can not be left blank...");
                    return false;
                }
                else
                {
                    vWOAmt = Convert.ToDecimal(txtWOAmt.Text);
                }
                if (((Request[lblLnTypeId.UniqueID] as string == null) ? lblLnTypeId.Text : Request[lblLnTypeId.UniqueID] as string) != "")
                {
                    pLnTypeId = Convert.ToInt32(((Request[lblLnTypeId.UniqueID] as string == null) ? lblLnTypeId.Text : Request[lblLnTypeId.UniqueID] as string));
                }
                DateTime vWODeclDt = gblFuction.setDate(txtWODeclDt.Text);
                if (vWODeclDt < gblFuction.setDate(Session[gblValue.FinFromDt].ToString()) || vWODeclDt > gblFuction.setDate(Session[gblValue.FinToDt].ToString()))
                {
                    gblFuction.AjxMsgPopup("Write-Off Date Should Be in Financial Year...");
                    return false;
                }

                //string vLoanAc = CGblIdGenerator.ChkLoanParameterByLoanTypId(gblValue.PrincipalLoanAc, pLnTypeId, vBrCode);
                vCustomernme = ddlCust.SelectedItem.Text.ToString();

                //DataTable dtAccount = new DataTable();
                //DataRow dr;
                //DataColumn dc = new DataColumn();
                //dc.ColumnName = "DtlId";
                //dtAccount.Columns.Add(dc);
                //DataColumn dc1 = new DataColumn();
                //dc1.ColumnName = "DescId";
                //dtAccount.Columns.Add(dc1);
                //DataColumn dc2 = new DataColumn();
                //dc2.ColumnName = "DC";
                //dtAccount.Columns.Add(dc2);
                //DataColumn dc3 = new DataColumn();
                //dc3.ColumnName = "Amt";
                //dc3.DataType = System.Type.GetType("System.Decimal");
                //dtAccount.Columns.Add(dc3);
                //dtAccount.TableName = "Table1";
                //if (Convert.ToDecimal((Request[txtWOAmt.UniqueID] as string == null) ? txtWOAmt.Text : Request[txtWOAmt.UniqueID] as string) > 0)
                //{
                //    int i = 1;
                //    dr = dtAccount.NewRow();
                //    dr["DescId"] = vLoanAc;
                //    dr["DC"] = "C";
                //    dr["Amt"] = Convert.ToDecimal((Request[txtWOAmt.UniqueID] as string == null) ? txtWOAmt.Text : Request[txtWOAmt.UniqueID] as string);
                //    dr["DtlId"] = i;
                //    dtAccount.Rows.Add(dr);
                //    dtAccount.AcceptChanges();

                //    i = i + 1;
                //    dr = dtAccount.NewRow();
                //    dr["DescId"] = vWOLedger;
                //    dr["DC"] = "D";
                //    dr["Amt"] = Convert.ToDecimal((Request[txtWOAmt.UniqueID] as string == null) ? txtWOAmt.Text : Request[txtWOAmt.UniqueID] as string);
                //    dr["DtlId"] = i;
                //    dtAccount.Rows.Add(dr);
                //    dtAccount.AcceptChanges();

                //}
                //vXmlAC = DataTableTOXml(dtAccount);
                vNarationL = "Being the Amount adjust against Write-Off of Loan No- " + vLnId + " of " + vCustomernme;
                if (Mode == "Save")
                {
                    vErr = oLD.SaveWriteOffDeclare(vLnId, vWODeclDt, vWOAmt, vTblMst, vTblDtl, vFinYear, vBrCode, vNarationL, this.UserID, pErr, "Save");
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
                    gblFuction.AjxMsgPopup("Record Can Not be Deleted...");
                    //CCollectionRoutine oColl = new CCollectionRoutine();
                    //DataTable dt = new DataTable();
                    //dt = oColl.ChkWOCollForDel(vLnId);
                    //if (Convert.ToInt32(dt.Rows[0]["TotColl"]) > 0)
                    //{
                    //    gblFuction.AjxMsgPopup("Write-Off Collection  already exist against this Loan... Record Can Not be Deleted...");
                    //    return false;
                    //}
                    //vErr = oLD.SaveWriteOffDeclare(vLnId, vWODeclDt, vWOAmt, vXmlAC, vTblMst, vTblDtl, vFinYear, vBrCode, vNarationL, this.UserID, pErr, "Delete", vWOLedger);
                    //if (vErr == 0)
                    //{
                    //    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    //    vResult = true;
                    //}
                    //else
                    //{
                    //    gblFuction.MsgPopup(gblPRATAM.DBError);
                    //    vResult = false;
                    //}
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
        private void popCustomer(string pBrCode)
        {
            DataTable dt = null;
            CLoanRecovery oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());

            try
            {
                oCD = new CLoanRecovery();
                dt = oCD.GetCustNameForWODecl(pBrCode);
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
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                if (txtWODeclDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Write Off Declaration Date can not be left blank...");
                    return;
                }
                DateTime pWODate = gblFuction.setDate(txtWODeclDt.Text);
                string pCustId = ddlCust.SelectedValue.ToString();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                oMem = new CLoanRecovery();
                dt = oMem.GetWOLoanIDByCustId(pCustId, pWODate);
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
        protected void ddlLoanNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtWODeclDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Write Off Declaration Date can not be left blank...");
                return;
            }
            DataTable dt = null;
            CLoanRecovery oMem = null;
            try
            {
                string pLoanId = ddlLoanNo.SelectedValue.ToString();
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                DateTime pWODate = gblFuction.setDate(txtWODeclDt.Text);
                oMem = new CLoanRecovery();
                dt = oMem.GetWODtlbyLoanNo(pLoanId, pWODate);
                if (dt.Rows.Count > 0)
                {
                    txtPOSAmt.Text = dt.Rows[0]["POSAmt"].ToString();
                    txtWOAmt.Text = dt.Rows[0]["POSAmt"].ToString();
                    txtLoanAmt.Text = dt.Rows[0]["DisbAmt"].ToString();
                    txtLnDate.Text = dt.Rows[0]["DisbDate"].ToString();
                    lblLnTypeId.Text = dt.Rows[0]["LoanTypeId"].ToString();
                }
                else
                {
                    txtPOSAmt.Text = "";
                    txtWOAmt.Text = "";
                    txtLoanAmt.Text = "";
                    txtLnDate.Text = "";
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
                    ClearControls();
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
                dt = oMem.GetLoanNoByCustId(pCustId);
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
        private void PopBranch()
        {
            CUser oUsr = null;
            DataTable dt = null;
            oUsr = new CUser();
            try
            {
                string vBrCode = Session[gblValue.BrnchCode].ToString();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
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
                oUsr = null;
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
