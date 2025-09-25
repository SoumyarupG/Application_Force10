using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMBA;
using CENTRUMCA;
using System.Collections;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class GeneralParameter : CENTRUMBAse
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                    StatusButton("Exit");
                else
                    StatusButton("View");
                txtEffDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                PopLiability();
                PopIncome();
                LoadGrid();
                LoadAcGenLed(Session[gblValue.BrnchCode].ToString());
                tbGenParameter.ActiveTabIndex = 0;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "General Parameter";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuGenParameterCF);
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
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "General Parameter", false);
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
        private void PopLiability()
        {
            DataTable dtLib = null;
            string vGenAcType = "G";
            Int32 vLib = 2;
            CGenParameter oGen = new CGenParameter();
            dtLib = oGen.GetLedgerByAcHeadId(vGenAcType, vLib);

            ListItem Lst5 = new ListItem();
            Lst5.Text = "<--- Select --->";
            Lst5.Value = "-1";
            ddlLogInCGSTAC.DataTextField = "Desc";
            ddlLogInCGSTAC.DataValueField = "DescId";
            ddlLogInCGSTAC.DataSource = dtLib;
            ddlLogInCGSTAC.DataBind();
            ddlLogInCGSTAC.Items.Insert(0, Lst5);

            ListItem Lst7 = new ListItem();
            Lst7.Text = "<--- Select --->";
            Lst7.Value = "-1";
            ddlLogInSGSTAC.DataTextField = "Desc";
            ddlLogInSGSTAC.DataValueField = "DescId";
            ddlLogInSGSTAC.DataSource = dtLib;
            ddlLogInSGSTAC.DataBind();
            ddlLogInSGSTAC.Items.Insert(0, Lst7);

            ddlLogInIGSTAC.DataTextField = "Desc";
            ddlLogInIGSTAC.DataValueField = "DescId";
            ddlLogInIGSTAC.DataSource = dtLib;
            ddlLogInIGSTAC.DataBind();
            ddlLogInIGSTAC.Items.Insert(0, Lst7);
        }
        private void PopIncome()
        {
            DataTable dt = null;
            string vGenAcType = "G";
            Int32 vIncome = 3;
            CGenParameter oGen = new CGenParameter();
            dt = oGen.GetLedgerByAcHeadId(vGenAcType, vIncome);

            ListItem Lst4 = new ListItem();
            Lst4.Text = "<--- Select --->";
            Lst4.Value = "-1";
            ddlLogInFeesAC.DataTextField = "Desc";
            ddlLogInFeesAC.DataValueField = "DescId";
            ddlLogInFeesAC.DataSource = dt;
            ddlLogInFeesAC.DataBind();
            ddlLogInFeesAC.Items.Insert(0, Lst4);
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
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
                tbGenParameter.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
                if (this.CanDelete == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Del);
                    return;
                }
                if (SaveRecords("Delete") == true)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    LoadGrid();
                    StatusButton("Delete");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbGenParameter.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                LoadGrid();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private void LoadGrid()
        {
            DataTable dt = null;
            string vBrCode = "";
            CGenParameter oGen = null;
            
            try
            {
                vBrCode = Session[gblValue.BrnchCode].ToString();
                oGen = new CGenParameter();
                dt = oGen.GetGeneralParameterList();
                if (dt.Rows.Count > 0)
                {
                    gvGenParameter.DataSource = dt;
                    gvGenParameter.DataBind();
                }
                else
                {
                    gvGenParameter.DataSource = null;
                    gvGenParameter.DataBind();
                }
            }
            finally
            {
                oGen = null;
                dt = null;
            }
        }
        protected void gvGenParameter_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 pId = 0, vRow = 0;
            string vBrCode = "";
            DataTable dt = null;
            try
            {
                pId = Convert.ToInt32(e.CommandArgument);
                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["Id"] = pId;
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvGenParameter.Rows)
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

                    CGenParameter oGen = new CGenParameter();
                    dt = oGen.GetGeneralParameterById(pId);
                    if (dt.Rows.Count > 0)
                    {
                        txtTotLogFees.Text = Convert.ToString(dt.Rows[0]["TotalLoginFees"]);
                        txtNetLogFees.Text = Convert.ToString(dt.Rows[0]["NetLogInFees"]);
                        txtLogFeesCGST.Text = Convert.ToString(dt.Rows[0]["CGSTAmt"]);
                        txtLogFeesSGST.Text = Convert.ToString(dt.Rows[0]["SGSTAmt"]);
                        txtLogFeesIGST.Text = Convert.ToString(dt.Rows[0]["IGSTAmt"]);
                        ddlLogInFeesAC.SelectedIndex = ddlLogInFeesAC.Items.IndexOf(ddlLogInFeesAC.Items.FindByValue(dt.Rows[0]["LogInFeesAC"].ToString()));
                        ddlLogInCGSTAC.SelectedIndex = ddlLogInCGSTAC.Items.IndexOf(ddlLogInCGSTAC.Items.FindByValue(dt.Rows[0]["CGSTAC"].ToString()));
                        ddlLogInSGSTAC.SelectedIndex = ddlLogInSGSTAC.Items.IndexOf(ddlLogInSGSTAC.Items.FindByValue(dt.Rows[0]["SGSTAC"].ToString()));
                        ddlLogInIGSTAC.SelectedIndex = ddlLogInIGSTAC.Items.IndexOf(ddlLogInIGSTAC.Items.FindByValue(dt.Rows[0]["IGSTAC"].ToString()));
                        lblUser.Text = "Last Modified By : " + dt.Rows[0]["UserName"].ToString();
                        lblDate.Text = "Last Modified Date : " + dt.Rows[0]["CreationDateTime"].ToString();
                        ddlPaymentChargeAC.SelectedIndex = ddlPaymentChargeAC.Items.IndexOf(ddlPaymentChargeAC.Items.FindByValue(dt.Rows[0]["PrePayAC"].ToString()));
                        ddlInsuffFundsAC.SelectedIndex = ddlInsuffFundsAC.Items.IndexOf(ddlInsuffFundsAC.Items.FindByValue(dt.Rows[0]["InsufFundsAC"].ToString()));
                        ddlLoanStatementAC.SelectedIndex = ddlLoanStatementAC.Items.IndexOf(ddlLoanStatementAC.Items.FindByValue(dt.Rows[0]["LoanStatAC"].ToString()));
                        ddlNOCInsuAC.SelectedIndex = ddlNOCInsuAC.Items.IndexOf(ddlNOCInsuAC.Items.FindByValue(dt.Rows[0]["NOCInsuAC"].ToString()));
                        ddlLatePaymentAC.SelectedIndex = ddlLatePaymentAC.Items.IndexOf(ddlLatePaymentAC.Items.FindByValue(dt.Rows[0]["LatePayAC"].ToString()));
                        ddlVisitAC.SelectedIndex = ddlVisitAC.Items.IndexOf(ddlVisitAC.Items.FindByValue(dt.Rows[0]["VisitAC"].ToString()));
                        txtPremature.Text = Convert.ToString(dt.Rows[0]["PrematurePerc"]);
                        tbGenParameter.ActiveTabIndex = 1;
                        StatusButton("Show");
                    }
                }
            }
            finally
            {
                dt = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vDisId = Convert.ToString(ViewState["Id"]);
            Int32 vErr = 0, vRec = 0, vStateId = 0, vId = 0, vNewId = 0;
            Decimal vTotLogFees = 0, vNetLogFees = 0, vCGSTAmt = 0, vSGSTAmt = 0, vIGSTAmt = 0, vPremature = 0; ;
            string vLogFeesAC = "", vCGSTAC = "", vSGSTAC = "", vIGSTAC = "";
            string vPrePayAC = "", vInsufFundsAC = "", vLoanStatAC = "", vNOCInsuAC = "", vLatePayAC = "", vVisitAC = "";
            CGenParameter oGen = null;
            try
            {
                if (txtEffDt.Text == "")
                {
                    gblFuction.MsgPopup("Effective Date Can Not Be Empty..");
                    return false;
                }
                DateTime pEffDt = gblFuction.setDate(txtEffDt.Text.ToString());
                if (txtTotLogFees.Text != "")
                    vTotLogFees = Convert.ToDecimal(txtTotLogFees.Text);
                if (txtNetLogFees.Text != "")
                    vNetLogFees = Convert.ToDecimal(txtNetLogFees.Text);
                if (txtLogFeesCGST.Text != "")
                    vCGSTAmt = Convert.ToDecimal(txtLogFeesCGST.Text);
                if (txtLogFeesSGST.Text != "")
                    vSGSTAmt = Convert.ToDecimal(txtLogFeesSGST.Text);
                if (txtLogFeesIGST.Text != "")
                    vIGSTAmt = Convert.ToDecimal(txtLogFeesIGST.Text);
                if (txtPremature.Text != "")
                    vPremature = Convert.ToDecimal(txtPremature.Text);

                vLogFeesAC = ddlLogInFeesAC.SelectedValue.ToString();
                vCGSTAC = ddlLogInCGSTAC.SelectedValue.ToString();
                vSGSTAC = ddlLogInSGSTAC.SelectedValue.ToString();
                vIGSTAC = ddlLogInIGSTAC.SelectedValue.ToString();
                vPrePayAC = ddlPaymentChargeAC.SelectedValue.ToString();
                vInsufFundsAC = ddlInsuffFundsAC.SelectedValue.ToString();
                vLoanStatAC = ddlLoanStatementAC.SelectedValue.ToString();
                vNOCInsuAC = ddlNOCInsuAC.SelectedValue.ToString();
                vLatePayAC = ddlLatePaymentAC.SelectedValue.ToString();
                vVisitAC = ddlVisitAC.SelectedValue.ToString();
                if (Mode == "Save")
                {
                     oGen = new CGenParameter();
                     vErr = oGen.SaveGeneralParameter(ref vNewId, vId, pEffDt, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt, vLogFeesAC, vCGSTAC,
                        vSGSTAC, vIGSTAC, this.UserID, "Save", vPrePayAC, vInsufFundsAC, vLoanStatAC, vNOCInsuAC, vLatePayAC, vVisitAC, vPremature);
                    if (vErr > 0)
                    {
                        ViewState["Id"] = vNewId;
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
                    vId = Convert.ToInt32(ViewState["Id"]);
                    oGen = new CGenParameter();
                    vErr = oGen.SaveGeneralParameter(ref vNewId, vId, pEffDt, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt, vLogFeesAC, vCGSTAC,
                        vSGSTAC, vIGSTAC, this.UserID, "Edit", vPrePayAC, vInsufFundsAC, vLoanStatAC, vNOCInsuAC, vLatePayAC, vVisitAC, vPremature);
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
                    oGen = new CGenParameter();
                    vErr = oGen.SaveGeneralParameter(ref vNewId, vId, pEffDt, vTotLogFees, vNetLogFees, vCGSTAmt, vSGSTAmt, vIGSTAmt, vLogFeesAC, vCGSTAC,
                        vSGSTAC, vIGSTAC, this.UserID, "Delete", vPrePayAC, vInsufFundsAC, vLoanStatAC, vNOCInsuAC, vLatePayAC, vVisitAC, vPremature);
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
                oGen = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtTotLogFees.Enabled = Status;
            txtNetLogFees.Enabled = Status;
            txtLogFeesCGST.Enabled = Status;
            txtLogFeesSGST.Enabled = Status;
            txtLogFeesIGST.Enabled = Status;
            ddlLogInFeesAC.Enabled = Status;
            ddlLogInCGSTAC.Enabled = Status;
            ddlLogInSGSTAC.Enabled = Status;
            ddlLogInIGSTAC.Enabled = Status;

            ddlPaymentChargeAC.Enabled = Status;
            ddlInsuffFundsAC.Enabled = Status;
            ddlLoanStatementAC.Enabled = Status;
            ddlNOCInsuAC.Enabled = Status;
            ddlLatePaymentAC.Enabled = Status;
            ddlVisitAC.Enabled = Status;
            txtPremature.Enabled = Status;
        }
        private void ClearControls()
        {
            txtTotLogFees.Text = "0.00";
            txtNetLogFees.Text = "0.00";
            txtLogFeesCGST.Text = "0.00";
            txtLogFeesSGST.Text = "0.00";
            txtLogFeesIGST.Text = "0.00";
            ddlLogInFeesAC.SelectedIndex = -1;
            ddlLogInCGSTAC.SelectedIndex = -1;
            ddlLogInSGSTAC.SelectedIndex = -1;
            ddlLogInIGSTAC.SelectedIndex = -1;
            lblDate.Text = "";
            lblUser.Text = "";
            ddlPaymentChargeAC.SelectedIndex = -1;
            ddlInsuffFundsAC.SelectedIndex = -1;
            ddlLoanStatementAC.SelectedIndex = -1;
            ddlNOCInsuAC.SelectedIndex = -1;
            ddlLatePaymentAC.SelectedIndex = -1;
            ddlVisitAC.SelectedIndex = -1;
            txtPremature.Text = "0.00";
        }
        private void LoadAcGenLed(string vBranch)
        {
            DataTable dt = null;
            Int32 I = 0;
            CVoucher oVoucher = null;
            SortedList Obj = new SortedList();
            try
            {
                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    vBranch = Session[gblValue.BrnchCode].ToString();
                }
                oVoucher = new CVoucher();
                dt = oVoucher.GetAcGenLedCB(vBranch, "V", "");
                dt.AcceptChanges();
                ddlPaymentChargeAC.DataSource = dt;
                ddlInsuffFundsAC.DataSource = dt;
                ddlLoanStatementAC.DataSource = dt;
                ddlNOCInsuAC.DataSource = dt;
                ddlLatePaymentAC.DataSource = dt;
                ddlVisitAC.DataSource = dt;

                foreach (DataRow dr in dt.Rows)
                {
                    Obj.Add(I, dr["SubSiLedYN"].ToString());
                    I = I + 1;
                }
                ddlPaymentChargeAC.DataTextField = "Desc";
                ddlPaymentChargeAC.DataValueField = "DescId";
                ddlPaymentChargeAC.DataBind();
                
                ddlInsuffFundsAC.DataTextField = "Desc";
                ddlInsuffFundsAC.DataValueField = "DescId";
                ddlInsuffFundsAC.DataBind();

                ddlLoanStatementAC.DataTextField = "Desc";
                ddlLoanStatementAC.DataValueField = "DescId";
                ddlLoanStatementAC.DataBind();

                ddlNOCInsuAC.DataTextField = "Desc";
                ddlNOCInsuAC.DataValueField = "DescId";
                ddlNOCInsuAC.DataBind();

                ddlLatePaymentAC.DataTextField = "Desc";
                ddlLatePaymentAC.DataValueField = "DescId";
                ddlLatePaymentAC.DataBind();

                ddlVisitAC.DataTextField = "Desc";
                ddlVisitAC.DataValueField = "DescId";
                ddlVisitAC.DataBind();

               
                ListItem liSel = new ListItem("<--- Select --->", "-1");
                ddlPaymentChargeAC.Items.Insert(0, liSel);
                ddlInsuffFundsAC.Items.Insert(0, liSel);
                ddlLoanStatementAC.Items.Insert(0, liSel);
                ddlNOCInsuAC.Items.Insert(0, liSel);
                ddlLatePaymentAC.Items.Insert(0, liSel);
                ddlVisitAC.Items.Insert(0, liSel);
            }
            finally
            {
                oVoucher = null;
                dt = null;
            }
        }
    }
}