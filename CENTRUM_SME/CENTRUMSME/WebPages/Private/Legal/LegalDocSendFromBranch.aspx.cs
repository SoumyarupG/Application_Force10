using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;

namespace CENTRUMSME.WebPages.Private.Legal
{
    public partial class LegalDocSendFromBranch : CENTRUMBAse
    {
        protected int vPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                PendDocSendBrCode();
                DocSendBrCode();
                popCustomer();
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-30));
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtDocSendDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PendingLegDocSendList();
                LegDocSendList();
                ddlCust.Enabled = false;
                ddlSancNo.Enabled = false;
                btnSave.Enabled = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "LEGAL DOCUMENTS SEND";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLegalDocSend);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "LEGAL DOCUMENTS SEND", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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
        private void PendDocSendBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForDocSend(Session[gblValue.BrnchCode].ToString(), "P");
            if (dt.Rows.Count > 0)
            {
                ddlPenLegDocSendBr.DataSource = dt;
                ddlPenLegDocSendBr.DataValueField = "BranchCode";
                ddlPenLegDocSendBr.DataTextField = "BranchName";
                ddlPenLegDocSendBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlPenLegDocSendBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlPenLegDocSendBr.DataSource = null;
                ddlPenLegDocSendBr.DataBind();
            }
        }
        private void DocSendBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForDocSend(Session[gblValue.BrnchCode].ToString(), "A");
            if (dt.Rows.Count > 0)
            {
                ddlLOALegBr.DataSource = dt;
                ddlLOALegBr.DataValueField = "BranchCode";
                ddlLOALegBr.DataTextField = "BranchName";
                ddlLOALegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlLOALegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlLOALegBr.DataSource = null;
                ddlLOALegBr.DataBind();
            }
        }
        private void popCustomer()
        {
            DataTable dt = null;
            CDisburse oCD = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oCD = new CDisburse();
                dt = oCD.GetCustNameForAgreement();
                ddlCust.DataSource = dt;
                ddlCust.DataTextField = "CompanyName";
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
        protected void gvPendDocSendList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable ();
            DataTable dt1 = new DataTable();
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    ds = oCG.GetPenDocSendBySanctId(vSanId, Convert.ToInt32(Session[gblValue.UserId]));
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendDocSendList.Rows)
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

                        ddlCust.Enabled = true;
                        ddlSancNo.Enabled = true;
                        btnSave.Enabled = true;
                        btnDelete.Enabled = false;

                        ddlCust.Items.Clear();
                        ddlCust.DataSource = null;
                        ddlCust.DataBind();
                        ListItem liSel = new ListItem();
                        liSel.Text = dt.Rows[0]["CustName"].ToString();
                        liSel.Value = dt.Rows[0]["CustID"].ToString();
                        ddlCust.Items.Insert(0, liSel);
                        PopBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        PopSanctionNo(dt.Rows[0]["CustID"].ToString());
                        ddlSancNo.SelectedIndex = ddlSancNo.Items.IndexOf(ddlSancNo.Items.FindByValue(dt.Rows[0]["SanctionID"].ToString()));
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtDocSendBy.Text = dt.Rows[0]["UserName"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        tabDocSend.ActiveTabIndex = 2;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvDocList.DataSource = dt1;
                        gvDocList.DataBind();
                    }
                    else
                    {
                        gvDocList.DataSource = null;
                        gvDocList.DataBind();
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void gvDocSendList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    ds = oCG.GetDocSendBySanctId(vSanId);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                    }
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendDocSendList.Rows)
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


                        ddlCust.Enabled = true;
                        ddlSancNo.Enabled = true;

                        btnSave.Enabled = false;
                        btnDelete.Enabled = true;

                        ddlCust.Items.Clear();
                        ddlCust.DataSource = null;
                        ddlCust.DataBind();
                        ListItem liSel = new ListItem();
                        liSel.Text = dt.Rows[0]["CustName"].ToString();
                        liSel.Value = dt.Rows[0]["CustID"].ToString();
                        ddlCust.Items.Insert(0, liSel);
                        PopBranch();
                        ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                        PopSanctionNo(dt.Rows[0]["CustID"].ToString());
                        ddlSancNo.SelectedIndex = ddlSancNo.Items.IndexOf(ddlSancNo.Items.FindByValue(dt.Rows[0]["SanctionID"].ToString()));
                        txtDocSendDate.Text = dt.Rows[0]["SendDate"].ToString();
                        txtDocSendBy.Text = dt.Rows[0]["UserName"].ToString();
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtCurierName.Text = dt.Rows[0]["CourierName"].ToString();
                        txtPODDocNo.Text = dt.Rows[0]["PODDocNo"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        hdIsRec.Value = dt.Rows[0]["LegOrgDocRecYN"].ToString();
                        tabDocSend.ActiveTabIndex = 2;
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvDocList.DataSource = dt1;
                        gvDocList.DataBind();
                    }
                    else
                    {
                        gvDocList.DataSource = null;
                        gvDocList.DataBind();
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void ddlCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pCustId = (Request[ddlCust.UniqueID] as string == null) ? ddlCust.SelectedValue : Request[ddlCust.UniqueID] as string;
            if (pCustId != "-1")
                PopSanctionNo(pCustId);
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;

            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string pSancId = "", pLoanAppNo = "", pCourierName = "", pDocNo = "";
            int pDocSendBy = 0;
            if (((Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string) != "-1")
                pSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            else
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return false;
            }
            if (txtLnAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank...");
                return false;
            }
            pLoanAppNo = txtLnAppNo.Text.ToString();
            if (txtDocSendDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Document Send Date can not be blank...");
                return false;
            }
            DateTime pDocSendDate = gblFuction.setDate(txtDocSendDate.Text);
            pCourierName = txtCurierName.Text.ToString();
            pDocNo = txtPODDocNo.Text.ToString();
            pDocSendBy = Convert.ToInt32(Session[gblValue.UserId]);
            CMember OMem = new CMember();
            Int32 vErr = 0;
            if (Mode == "Save")
            {
                vErr = OMem.SaveLegOrgDocSend(pSancId, pLoanAppNo, pCourierName, pDocNo, pDocSendDate, pDocSendBy, 0,"Save");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);

                    vResult = true;
                    //tabDocSend.ActiveTabIndex = 0;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            }
            else if (Mode == "Delete")
            {
                if (hdIsRec.Value.ToString() == "Y")
                {
                    gblFuction.AjxMsgPopup("File Already Received By HO. You can not delete Send Details..");
                    return false;
                }
                vErr = OMem.SaveLegOrgDocSend(pSancId, pLoanAppNo, pCourierName, pDocNo, pDocSendDate, pDocSendBy, 0, "Delete");
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                    vResult = true;
                    //tabDocSend.ActiveTabIndex = 0;
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                    vResult = false;
                }
            
            }
            return vResult;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (SaveRecords("Save") == true)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                ClearControl();
                PendDocSendBrCode();
                DocSendBrCode();
                PendingLegDocSendList();
                LegDocSendList();
                ViewState["StateEdit"] = null;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
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
                    ClearControl();
                    PendDocSendBrCode();
                    DocSendBrCode();
                    PendingLegDocSendList();
                    LegDocSendList();
                    ViewState["StateEdit"] = null;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void PopSanctionNo(string pCustId)
        {
            CDisburse oMem = new CDisburse();
            DataTable dt = new DataTable();
            oMem = new CDisburse();
            dt = oMem.GetSancIdForAgreement(pCustId);
            ddlSancNo.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                ddlSancNo.DataSource = dt;
                ddlSancNo.DataTextField = "SanctionNo";
                ddlSancNo.DataValueField = "SanctionID";
                ddlSancNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSancNo.Items.Insert(0, oli);
            }
            else
            {
                gblFuction.AjxMsgPopup("Final Sanction is not complete for that Customer....");
                ddlSancNo.DataSource = null;
                ddlSancNo.DataBind();
                return;
            }
        }
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }
        private void PendingLegDocSendList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenOrgDocSendList(ddlPenLegDocSendBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvPendDocSendList.DataSource = dt;
                    gvPendDocSendList.DataBind();
                }
                else
                {
                    gvPendDocSendList.DataSource = null;
                    gvPendDocSendList.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void LegDocSendList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            DateTime pFDate = gblFuction.setDate(txtFrmDt.Text.ToString());
            DateTime pTDate = gblFuction.setDate(txtToDt.Text.ToString());
            string pSearch = txtSearch.Text.Trim();
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetOrgDocSendList(pFDate, pTDate, pSearch, ddlLOALegBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvDocSendList.DataSource = dt;
                    gvDocSendList.DataBind();
                }
                else
                {
                    gvDocSendList.DataSource = null;
                    gvDocSendList.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void ClearControl()
        {
            gvDocList.DataSource = null;
            gvDocList.DataBind();
            ddlBranch.SelectedIndex = -1;
            txtLnAppNo.Text = "";
            txtCurierName.Text = "";
            txtPODDocNo.Text = "";
            txtDocSendDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtDocSendBy.Text = "";
        }
        protected void ddlPenLOALegBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            PendingLegDocSendList();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LegDocSendList();
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/WebPages/Public/Main.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}