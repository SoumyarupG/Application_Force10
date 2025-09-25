using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CENTRUMCA;
using CENTRUMBA;
using System.Drawing;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using System.Net.NetworkInformation;
using System.Linq;

namespace CENTRUMSME.WebPages.Private.Agreement
{
    public partial class AgreementMaster : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string vPathQRCode = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                popCustomer();
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-30));
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PendingAgrPrintList();
                PendingAgrVerList();
                AgreementList();
                ddlCust.Enabled = false;
                ddlSancNo.Enabled = false;
                chkVerify.Enabled = false;
                txtVerificDate.Enabled = false;
                txtVerRemarks.Enabled = false;
                btnSave.Enabled = false;
                txtAgrmntDate.Enabled = false;
                ceOpDt.Enabled = false;
                txtExpDisbDate.Enabled = false;
                ceExpDisbDate.Enabled = false;
                txtNupayDate.Enabled = false;
                ceNupayDt.Enabled = false;
                txtNupayRefNo.Enabled = false;

                hdnIsDigiDoc.Value = "N";
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
        private void InitBasePage()
        {
            DataTable DtBrCntrl = null;
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Agreement Master";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuAgreement);

                if (Session["BrCntrl"] != null)
                {
                    DtBrCntrl = (DataTable)Session["BrCntrl"];
                    if (Convert.ToString(DtBrCntrl.Rows[0]["PreDBMEL"]) == "N")
                    {
                        //gblFuction.AjxMsgPopup("Pre DB is not allowed in this Branch");
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Agreement Master", false);
                    }
                }

                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Agreement Master", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
        protected void gvPendAgrmntPrint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetFinalSanctionDtlBySanctId(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendAgrmntPrint.Rows)
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
                        chkPrint.Enabled = true;
                        txtPrintDate.Enabled = true;
                        txtVerRemarks.Enabled = true;
                        btnSave.Enabled = true;
                        txtAgrmntDate.Enabled = true;
                        ceOpDt.Enabled = true;
                        chkVerify.Enabled = false;
                        txtVerificDate.Enabled = false;

                        txtExpDisbDate.Enabled = true;
                        ceExpDisbDate.Enabled = true;
                        txtNupayDate.Enabled = true;
                        ceNupayDt.Enabled = true;
                        txtNupayRefNo.Enabled = true;

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
                        txtAgrmntDate.Text = dt.Rows[0]["AgrmntDate"].ToString();

                        txtExpDisbDate.Text = dt.Rows[0]["ExpectedDt"].ToString();
                        txtNupayRefNo.Text = dt.Rows[0]["NupayRefNo"].ToString();
                        txtNupayDate.Text = dt.Rows[0]["NupayDt"].ToString();

                        if (dt.Rows[0]["IsVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        txtVerificDate.Text = dt.Rows[0]["AgrVericDate"].ToString();
                        if (dt.Rows[0]["IsPrintYN"].ToString() == "Y")
                            chkPrint.Checked = true;
                        else
                            chkPrint.Checked = false;
                        txtPrintDate.Text = dt.Rows[0]["AgrPrintDate"].ToString();
                        txtVerRemarks.Text = dt.Rows[0]["AgrVerRemarks"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        if (dt.Rows[0]["IsUpdateYN"].ToString() == "Y")
                            btnSave.Enabled = true;
                        else
                            btnSave.Enabled = false;
                        tabLoanSanc.ActiveTabIndex = 3;
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }
        }
        protected void gvPendAgrmntVer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetFinalSanctionDtlBySanctId(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendAgrmntVer.Rows)
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
                        chkPrint.Enabled = false;
                        txtPrintDate.Enabled = false;
                        txtVerRemarks.Enabled = true;
                        btnSave.Enabled = true;
                        txtAgrmntDate.Enabled = false;
                        ceOpDt.Enabled = false;
                        chkVerify.Enabled = true;
                        txtVerificDate.Enabled = true;
                        
                        txtExpDisbDate.Enabled = false;
                        ceExpDisbDate.Enabled = false;
                        txtNupayDate.Enabled = false;
                        ceNupayDt.Enabled = false;
                        txtNupayRefNo.Enabled = false;

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
                        txtAgrmntDate.Text = dt.Rows[0]["AgrmntDate"].ToString();
                        txtExpDisbDate.Text = dt.Rows[0]["ExpectedDt"].ToString();
                        txtNupayRefNo.Text = dt.Rows[0]["NupayRefNo"].ToString();
                        txtNupayDate.Text = dt.Rows[0]["NupayDt"].ToString();

                        if (dt.Rows[0]["IsVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        txtVerificDate.Text = dt.Rows[0]["AgrVericDate"].ToString();
                        if (dt.Rows[0]["IsPrintYN"].ToString() == "Y")
                            chkPrint.Checked = true;
                        else
                            chkPrint.Checked = false;
                        txtPrintDate.Text = dt.Rows[0]["AgrPrintDate"].ToString();
                        txtVerRemarks.Text = dt.Rows[0]["AgrVerRemarks"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        if (dt.Rows[0]["IsUpdateYN"].ToString() == "Y")
                            btnSave.Enabled = true;
                        else
                            btnSave.Enabled = false;
                        tabLoanSanc.ActiveTabIndex = 3;
                    }
                }
            }
            finally
            {
                dt = null;
                oCG = null;
            }

        }
        protected void gvAgrmtDone_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            String vSanId = "";
            DataTable dt = null;
            CApplication oCG = null;
            try
            {
                vSanId = Convert.ToString(e.CommandArgument);
                ViewState["LnSancId"] = vSanId;
                if (e.CommandName == "cmdShow")
                {
                    oCG = new CApplication();
                    dt = oCG.GetFinalSanctionDtlBySanctId(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendAgrmntPrint.Rows)
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
                        chkVerify.Enabled = true;
                        txtVerificDate.Enabled = true;
                        txtVerRemarks.Enabled = true;
                        btnSave.Enabled = true;
                        txtAgrmntDate.Enabled = false;
                        ceOpDt.Enabled = false;

                        txtExpDisbDate.Enabled = false;
                        ceExpDisbDate.Enabled = false;
                        txtNupayDate.Enabled = false;
                        ceNupayDt.Enabled = false;
                        txtNupayRefNo.Enabled = false;

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
                        txtAgrmntDate.Text = dt.Rows[0]["AgrmntDate"].ToString();
                        txtExpDisbDate.Text = dt.Rows[0]["ExpectedDt"].ToString();
                        txtNupayRefNo.Text = dt.Rows[0]["NupayRefNo"].ToString();
                        txtNupayDate.Text = dt.Rows[0]["NupayDt"].ToString();

                        if (dt.Rows[0]["IsVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        txtVerificDate.Text = dt.Rows[0]["AgrVericDate"].ToString();
                        if (dt.Rows[0]["IsPrintYN"].ToString() == "Y")
                            chkPrint.Checked = true;
                        else
                            chkPrint.Checked = false;
                        hdnIsDigiDoc.Value = "Y";
                        txtPrintDate.Text = dt.Rows[0]["AgrPrintDate"].ToString();
                        txtVerRemarks.Text = dt.Rows[0]["AgrVerRemarks"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        if (dt.Rows[0]["IsUpdateYN"].ToString() == "Y")
                            btnSave.Enabled = true;
                        else
                            btnSave.Enabled = false;
                        tabLoanSanc.ActiveTabIndex = 3;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string NupayRefNo = txtNupayRefNo.Text;
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string pSancId = "", pVerifyYN = "", pAgrRemarks = "", pPrintYN = "";
            if (((Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string) != "-1")
                pSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            else
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to update verification details");
                return;
            }

            if (txtAgrmntDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Agreement Date can not be blank...");
                return;
            }
            else if (gblFuction.setDate(txtAgrmntDate.Text) < vLoginDt || (gblFuction.setDate(txtAgrmntDate.Text) - vLoginDt).TotalDays > 3)
            {
                gblFuction.AjxMsgPopup("Agreement date must be current date or within 3 days in future.");
                return;
            }
            if (txtExpDisbDate.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Expceted date can not be left blank.");
                return;
            }
            else if (gblFuction.setDate(txtExpDisbDate.Text) < vLoginDt || (gblFuction.setDate(txtExpDisbDate.Text) - vLoginDt).TotalDays > 4)
            {
                gblFuction.AjxMsgPopup("Expceted date must be current date or within 4 days in future.");
                return;
            }

            if (NupayRefNo == "")
            {
                gblFuction.AjxMsgPopup("Nupay reference no can not be left blank.");
                return;
            }
            if (NupayRefNoCheck(NupayRefNo) == false || NupayRefNo.Length < 18)
            {
                gblFuction.AjxMsgPopup("You have entered an incorrect Nupay reference number.");
                return;
            }

            if (txtNupayDate.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Nupay date can not be left blank.");
                return;
            }
            else if ((vLoginDt - gblFuction.setDate(txtNupayDate.Text)).TotalDays > 10 || (vLoginDt - gblFuction.setDate(txtNupayDate.Text)).TotalDays < 0)
            {
                gblFuction.AjxMsgPopup("Nupay date must be current date or within 10 days in past from current date.");
                return;
            }
            string IsDisb = Convert.ToString((Request[hdIsDisb.UniqueID] as string == null) ? hdIsDisb.Value : Request[hdIsDisb.UniqueID] as string);
            if (IsDisb == "Y")
            {
                gblFuction.AjxMsgPopup("Disbursement Already Done, You Can Not Make Change in Agreement Section");
                return;
            }


            if (chkVerify.Checked == true)
                pVerifyYN = "Y";
            else
                pVerifyYN = "N";
            if (chkPrint.Checked == true)
                pPrintYN = "Y";
            else
                pPrintYN = "N";
            pAgrRemarks = txtVerRemarks.Text.ToString();
            if (chkPrint.Checked == true && txtPrintDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Agreement Print Date can not be blank...");
                return;
            }
            if (chkVerify.Checked == true && txtVerificDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Agreement Verification Date can not be blank...");
                return;
            }
            DateTime? pAgrPrintDate = null;
            DateTime? pAgrVerificationDt = null;
            if (txtVerificDate.Text != "")
                pAgrVerificationDt = gblFuction.setDate(txtVerificDate.Text);
            if (txtPrintDate.Text != "")
                pAgrPrintDate = gblFuction.setDate(txtPrintDate.Text);
            CMember OMem = new CMember();
            Int32 vErr = 0;
            vErr = OMem.SaveAgrVerification(pSancId, pVerifyYN, pAgrVerificationDt, pPrintYN, pAgrPrintDate, pAgrRemarks, 0);
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                PendingAgrPrintList();
                PendingAgrVerList();
                AgreementList();
                //tabLoanSanc.ActiveTabIndex = 0;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void PopSanctionNo(string pCustId)
        {
            CDisburse oMem = new CDisburse();
            DataTable dt = new DataTable();
            oMem = new CDisburse();
            dt = oMem.GetSancIdForAgreement(pCustId);
            ddlSancNo.Items.Clear();
            txtAgrmntDate.Text = "";
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
        protected void ddlSancNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string pSancNo = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            txtAgrmntDate.Text = "";
            dt = oMem.GetAgrmntDate(pSancNo);
            if (dt.Rows.Count > 0)
            {
                txtAgrmntDate.Text = dt.Rows[0]["AgrmntDate"].ToString();
            }
            else
            {
                txtAgrmntDate.Text = "";
            }
        }
        private bool ValidateDate()
        {
            bool vRst = true;
            return vRst;
        }
        private void PendingAgrPrintList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenAgrPrintList(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvPendAgrmntPrint.DataSource = dt;
                    gvPendAgrmntPrint.DataBind();
                }
                else
                {
                    gvPendAgrmntPrint.DataSource = null;
                    gvPendAgrmntPrint.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void PendingAgrVerList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenAgrVerList(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvPendAgrmntVer.DataSource = dt;
                    gvPendAgrmntVer.DataBind();
                }
                else
                {
                    gvPendAgrmntVer.DataSource = null;
                    gvPendAgrmntVer.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void AgreementList()
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
                dt = oCA.GetAgrList(pFDate, pTDate, pSearch, vBrCode);
                if (dt.Rows.Count > 0)
                {
                    gvAgrmtDone.DataSource = dt;
                    gvAgrmtDone.DataBind();
                }
                else
                {
                    gvAgrmtDone.DataSource = null;
                    gvAgrmtDone.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            AgreementList();
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
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            //switch (e.CommandName)
            //{
            //    case "Prev":
            //        vPgNo = Int32.Parse(lblCrPg.Text) - 1;
            //        break;
            //    case "Next":
            //        vPgNo = Int32.Parse(lblCrPg.Text) + 1;
            //        break;
            //}
            //PendingAgrList(vPgNo);
            //tbEmp.ActiveTabIndex = 0;
        }
        protected void lbSanctionLetter_Click(object sender, EventArgs e)
        {
            string vRptPath = "";
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetSanctionLetterDtlByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["SLDt"] = dt;
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\SanctionLetter.rpt";
                    //ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('SanctionLetter.aspx','SANCTION LETTER','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Sanction_Letter");
                        rptDoc.Dispose();
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }
            }
        }
        protected void lbSanctionLetterMAS_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetLoanAgreementByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["SLDt"] = dt;
                    //ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('SanctionLetterMAS.aspx','SANCTION LETTER','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                    string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\LoanAgreement.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Loan_Agreement");
                        rptDoc.Dispose();
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }

                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }
            }
        }
        protected void lbPowerOfAttorney_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetPOADtlBySancId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["POADt"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('PowerOfAttorney.aspx','POWER OF ATTORNEY','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }


            }

        }
        protected void lblSolePro_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetSolProDeclBySancId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["SPDDt"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('SoleProDeclaration.aspx','SOLE PROPRIETORSHIP DECLARATION','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }


            }

        }
        protected void lbWelComeLetter_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetWelComeLetterBySanctionId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["WlcmLtr"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('WelComeLetter.aspx','WELCOME LETTER','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbFinLegOpinion_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetFinLegOpinionBySancId(vSancId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Session["FLO"] = ds;  // FLO-- Final Legal Opinion
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('FinalLegalOpinion.aspx','FINAL LEGAL OPINION','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbLnApplForm_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                string vRptPath = "";
                //DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                //DataTable dt3 = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetDetailsForLAPLoanApplicationForm(vSancId, Convert.ToInt32(Session[gblValue.UserId]));
                //dt2 = Omem.GetDetailsForLAPLoanApplicationForm_Property(vSancId);
                dt2 = Omem.GetDetailsForLAPLoanApplicationForm_Obligation(vSancId);
                //ds = Omem.GetFinLegOpinionBySancId(vSancId);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    Session["FLO"] = ds;  // FLO-- Final Legal Opinion
                //    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('FinalLegalOpinion.aspx','FINAL LEGAL OPINION','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                //}
                //else
                //{
                //    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                //    return;
                //}
                using (ReportDocument rptDoc = new ReportDocument())
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ApplicationFormNew.rpt";
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt);
                    rptDoc.Subreports["ApplicationFormObligation.rpt"].SetDataSource(dt2);
                    //rptDoc.Subreports["LAPLoanApplicationForm_Obligation.rpt"].SetDataSource(dt3);
                    ////rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    ////rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    ////rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    //rptDoc.SetParameterValue("pBranch", ddlBranch.SelectedItem.Text);
                    ////rptDoc.SetParameterValue("pTitle", "LAPLoanApplicationForm");
                    ////rptDoc.SetParameterValue("pFrmDt", txtDtFrm.Text);
                    ////rptDoc.SetParameterValue("pToDt", txtToDt.Text);
                    rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Application_Form");
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
        }
        protected void lbScheduleOfCharges_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                string vRptPath = "";
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetScheduleofChargesByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\ScheduleOfCharges.rpt";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetParameterValue("pBorrower", dt.Rows[0]["Borrower"].ToString());
                        rptDoc.SetParameterValue("pCoBorrower", dt.Rows[0]["CoBorrower"].ToString());
                        rptDoc.SetParameterValue("pSanctionDate", dt.Rows[0]["SanctionDate"].ToString());
                        // rptDoc.SetDataSource(dt);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Schedule_Of_Charges");
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
        }
        protected void lbVernacular_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                string vRptPath = "";
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetScheduleofChargesByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\rptVernacular.rpt";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetParameterValue("pBorrower", dt.Rows[0]["Borrower"].ToString());
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Vernacular_Declaration");
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
        }
        protected void lbEndUser_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                string vRptPath = "";
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetScheduleofChargesByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\Annexure.rpt";
                        rptDoc.Load(vRptPath);
                        rptDoc.SetParameterValue("pName", dt.Rows[0]["Borrower"].ToString());
                        rptDoc.SetParameterValue("pDate", dt.Rows[0]["ApplicationDt"].ToString());
                        rptDoc.SetParameterValue("pLoanApplNo", dt.Rows[0]["LoanAppId"].ToString());
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Annexure");
                        Response.ClearContent();
                        Response.ClearHeaders();
                    }
                }
            }
        }
        protected void lbGuarBond_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetGuaranteeBondBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["GBDt"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('GuaranteeBond.aspx','END USE LETTER','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbRecptLnDoc_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetMasterFacilityDtlByLnAppId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["MFDt"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('AckRecptLnDoc.aspx','Acknowledgment of Receipt of Loan Documents','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbDemandProLetter_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember Omem = new CMember();
                dt = Omem.GetSanctionLetterDtlByLnAppId(vSancId);
                if (dt.Rows.Count > 0)
                {
                    Session["SLDt"] = dt;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('DemandProLetter.aspx','Demand Promissory Letter - Remplate','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbHypothecationAgree_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetHypoAgrmntBySancId(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["HADt"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('HypothecationAgreement.aspx','Agreement of Hypothecation','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void lbRepaySchedule_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No...");
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                CMember Omem = new CMember();
                ds = Omem.GetRepaySchedule(vSancId);
                if (ds.Tables.Count > 0)
                {
                    Session["RSchedule"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('RepaySchedule.aspx','Repayment Schedule','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;
                }
            }
        }
        protected void btnAgmntDtUpdate_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            string NupayRefNo = txtNupayRefNo.Text;
            string vErrMsg = "";
            if (txtAgrmntDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Agreement Date can not be blank...");
                return;
            }
            else if (gblFuction.setDate(txtAgrmntDate.Text) < vLoginDt || (gblFuction.setDate(txtAgrmntDate.Text) - vLoginDt).TotalDays > 3)
            {
                gblFuction.AjxMsgPopup("Agreement date must be current date or within 3 days in future.");
                return;
            }

            if (vSancId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to udate agreement date...");
                return;
            }
            if (vSancId == "-1")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to udate agreement date...");
                return;
            }
            if (txtExpDisbDate.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Expceted date can not be left blank.");
                return ;
            }
            else if (gblFuction.setDate(txtExpDisbDate.Text) < vLoginDt || (gblFuction.setDate(txtExpDisbDate.Text) - vLoginDt).TotalDays > 4)
            {
                gblFuction.AjxMsgPopup("Expceted date must be current date or within 4 days in future.");
                return ;
            }

            if (NupayRefNo == "")
            {
                gblFuction.AjxMsgPopup("Nupay reference no can not be left blank.");                
                return;
            }
            if (NupayRefNoCheck(NupayRefNo) == false || NupayRefNo.Length < 18)
            {
                gblFuction.AjxMsgPopup("You have entered an incorrect Nupay reference number.");               
                return;
            }

            if (txtNupayDate.Text.Trim() == "")
            {
                gblFuction.AjxMsgPopup("Nupay date can not be left blank.");
                return;
            }
            else if ((vLoginDt - gblFuction.setDate(txtNupayDate.Text)).TotalDays > 10 || (vLoginDt - gblFuction.setDate(txtNupayDate.Text)).TotalDays < 0)
            {
                gblFuction.AjxMsgPopup("Nupay date must be current date or within 10 days in past from current date.");
                return;
            }

            DateTime vAgmntDate = gblFuction.setDate(txtAgrmntDate.Text);
            Int32 vErr = 0;
            CMember OMem = new CMember();
            vErr = OMem.UpdateAgrmntDate(vSancId, vAgmntDate, gblFuction.setDate(txtExpDisbDate.Text), NupayRefNo, gblFuction.setDate(txtNupayDate.Text), 0,ref vErrMsg);
            if (vErr == 1)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
            }
            else if (vErr == 2)
            {
                gblFuction.MsgPopup(vErrMsg);
            }
            else if (vErr == 0)
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }

        public bool NupayRefNoCheck(string pNuPayRefNo)
        {
            string[] validPrefixes = { "NP138", "NP601", "NP611", "NP701", "NP711" };
            bool startsWithValidPrefix = false;
            foreach (string prefix in validPrefixes)
            {
                if (pNuPayRefNo.StartsWith(prefix))
                {
                    startsWithValidPrefix = true;
                    break;
                }
            }
            return startsWithValidPrefix;
        }

        protected void btnQRCode_Click(object sender, EventArgs e)
        {
            string Id = ddlSancNo.SelectedValue;
            string pPayee = ddlCust.SelectedItem.Text;
            string pAmount = "0";
            DataTable dt1 = new DataTable();
            CReports oRpt = new CReports();
            dt1 = oRpt.GetInstallMentAmt(Id);
            if (dt1.Rows.Count > 0)
            {
                pAmount = dt1.Rows[0]["InstallmentAmount"].ToString();
                CreateQRCode(pPayee, pAmount, Id);
            }
        }

        private void CreateQRCode(string ppayee, string pAmount, string pID)
        {
            // Hardcoded
            string merchantID = "CPL";
            string merchantName = "Centrum Microcredit Limited";
            // Hardcoded
            string payee = ppayee; //Name of the member
            string amount = pAmount;  // Installment Amount 
            string payeeAccountNo = pID;// SanctionID

            CallQRCode(merchantID, merchantName, payee, amount, payeeAccountNo);
        }

        public void CallQRCode(string pmerchantID, string pmerchantName, string ppayee, string pamount, string ppayeeAccountNo)
        {
            string pQRMsg = "Ok";
            try
            {
                string json = "";

                RootObject rootobjectreq = new RootObject();
                rootobjectreq.merchantID = pmerchantID;
                rootobjectreq.merchantName = pmerchantName;
                rootobjectreq.payee = ppayee;
                rootobjectreq.amount = pamount;
                rootobjectreq.payeeAccountNo = ppayeeAccountNo;


                json = JsonConvert.SerializeObject(rootobjectreq);

                /*******Login URL********/
                string url = "https://eft.billcloud.in:9002/service/upiservice/getQR";


                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                /*****************************/
                httpWebRequest.Proxy = null;
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", "Basic dXBpcXI6dXAhcXJAMTIz");

                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);

                //httpWebRequest.PreAuthenticate = true;
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string responsedata = string.Empty;
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    responsedata = result.ToString().Trim();
                }

                RootObjectResponseWITHQR rootobjectresponse = new RootObjectResponseWITHQR();
                rootobjectresponse = JsonConvert.DeserializeObject<RootObjectResponseWITHQR>(responsedata);

                pQRMsg = rootobjectresponse.QR;
                LoadImage(pQRMsg, ppayeeAccountNo);
                gblFuction.AjxMsgPopup("QR Code Saved Successfully.");
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Login.aspx");
                pQRMsg = ex.Message;
                gblFuction.AjxMsgPopup("Try Again!!!");

                //throw ex;
            }

        }

        public void LoadImage(string pQRImage, string ppayeeAccountNo)
        {
            string vMsg = "";
            vPathQRCode = ConfigurationManager.AppSettings["PathQRCode"];
            try
            {
                string filePath = vPathQRCode + "\\" + ppayeeAccountNo + ".png";  ///WebClient Config
                File.WriteAllBytes(filePath, Convert.FromBase64String(pQRImage));
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Login.aspx");
                vMsg = ex.Message;
                gblFuction.AjxFocus("Will be available Shortly!!!");
                //throw ex;
            }

        }

        protected void lbDigitalDocs_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            DataSet dsDigiDoc = null;
            DataTable dtAppFrm1 = null, dtAppFrm2 = null, dtSancLetter = null, dtEMISchedule = null;
            DataTable dtLoanAgr = null, dtAuthLetter = null, dtKotak = null, dtDigiDocDtls = null;

            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null
            , dt8 = null, dt9 = null, dt10 = null, dt11 = null, dt12 = null, dt13 = null, dt14 = null, dt15 = null,
            dt16 = null, dt17 = null, dt18 = null, dtScheduleOfCharges = null, dtSchedule = null;

            CReports oRpt = new CReports();
            CMember oMem = null;
            CDigiDoc oUsr = null;

            string vRptPath = "", vRptName = "";

            try
            {
                string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;


                if (vSancId == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                if (vSancId == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                if (vSancId == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No...");
                    return;
                }
                else
                {
                    vRptPath = string.Empty;
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\DigitalDocs.rpt";
                    DataTable dt = new DataTable();
                    oMem = new CMember();
                    ds = new DataSet();
                    ds = oMem.GetAgrDigitalDocs(vSancId, 0);
                    dtAppFrm1 = ds.Tables[0];
                    dtAppFrm2 = ds.Tables[1];
                    dtSancLetter = ds.Tables[2];
                    dtEMISchedule = ds.Tables[3];
                    dtLoanAgr = ds.Tables[4];
                    dtAuthLetter = ds.Tables[5];
                    dtKotak = ds.Tables[6];

                    oUsr = new CDigiDoc();
                    dsDigiDoc = oUsr.getDigiDocDtlsByDocId(0, "", "Y");
                    dtDigiDocDtls = dsDigiDoc.Tables[0];
                    string vLoanAppId = "", vCustId = "";
                    string vBrCode = (string)Session[gblValue.BrnchCode];
                    if (dtAppFrm1.Rows.Count > 0)
                    {
                        vLoanAppId = dtAppFrm1.Rows[0]["LoanAppId"].ToString();
                        vCustId = dtAppFrm1.Rows[0]["CustId"].ToString();
                    }
                    ds = oRpt.rptCAMReport(vCustId, vLoanAppId);
                    oMem = new CMember();
                    dtScheduleOfCharges = oMem.GetScheduleofChargesByLnAppId(vSancId);
                    oMem = new CMember();
                    dtSchedule = oMem.GetSanctionLetterDtlByLnAppId(vSancId);

                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        dt3 = ds.Tables[2];
                        dt4 = ds.Tables[3];
                        dt5 = ds.Tables[4];
                        dt6 = ds.Tables[5];
                        dt7 = ds.Tables[6];
                        dt8 = ds.Tables[7];
                        dt9 = ds.Tables[8];
                        dt10 = ds.Tables[9];
                        dt11 = ds.Tables[10];
                        dt12 = ds.Tables[11];
                        dt13 = ds.Tables[12];
                        dt14 = ds.Tables[13];
                        dt15 = ds.Tables[14];
                        dt16 = ds.Tables[15];
                        dt17 = ds.Tables[16];
                        dt18 = ds.Tables[17];
                    }

                    if (dtAppFrm1.Rows.Count > 0)
                    {
                        if (vRptPath != string.Empty)
                        {
                            using (ReportDocument rptDoc = new ReportDocument())
                            {
                                vRptName = "DigitalDocument";
                                rptDoc.Load(vRptPath);
                                rptDoc.SetDataSource(dtAppFrm1);
                                rptDoc.Subreports["DigitalDoc_ExistingLoanDtl.rpt"].SetDataSource(dtAppFrm2);
                                rptDoc.Subreports["DigitalDoc_SancLetter.rpt"].SetDataSource(dtSancLetter);
                                rptDoc.Subreports["DigitalDoc_EMISchedule.rpt"].SetDataSource(dtEMISchedule);
                                rptDoc.Subreports["DigitalDoc_LoanAgreement.rpt"].SetDataSource(dtLoanAgr);
                                rptDoc.Subreports["DigitalDoc_AuthLetter.rpt"].SetDataSource(dtAuthLetter);
                                rptDoc.Subreports["DigitalDoc_Kotak.rpt"].SetDataSource(dtKotak);
                                rptDoc.Subreports["DigitalDoc_eSigned.rpt"].SetDataSource(dtDigiDocDtls);

                                rptDoc.Subreports["CAM1.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["CAM2.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["App.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["CoApp.rpt"].SetDataSource(dt2);
                                rptDoc.Subreports["AppResidence.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["rptObligation.rpt"].SetDataSource(dt4);
                                rptDoc.Subreports["rptObligationCoAppl.rpt"].SetDataSource(dt17);
                                rptDoc.Subreports["CreditProfile.rpt"].SetDataSource(dt18);
                                rptDoc.Subreports["BankinDetailAppCoApp.rpt"].SetDataSource(dt8);
                                rptDoc.Subreports["rptFOIRIncomeAssesment.rpt"].SetDataSource(dt10);
                                rptDoc.Subreports["CAM6.rpt"].SetDataSource(dt1);
                                rptDoc.Subreports["DecisionSheet.rpt"].SetDataSource(dt16);
                                rptDoc.Subreports["LnSancApprove.rpt"].SetDataSource(dt3);
                                rptDoc.Subreports["SanctionLetterOwn.rpt"].SetDataSource(dtSchedule);

                                rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                                rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                                rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                                rptDoc.SetParameterValue("pBranch", vBrCode);
                                rptDoc.SetParameterValue("pTitle", "CAM Report");
                                rptDoc.SetParameterValue("pBorrower", dtScheduleOfCharges.Rows[0]["Borrower"].ToString());
                                rptDoc.SetParameterValue("pCoBorrower", dtScheduleOfCharges.Rows[0]["CoBorrower"].ToString());
                                rptDoc.SetParameterValue("pSanctionDate", dtScheduleOfCharges.Rows[0]["SanctionDate"].ToString());
                                //rptDoc.Subreports["CAM3.rpt"].SetParameterValue("pApplicantName", dtDigiDocDtls.Rows[0]["ApplicantName"].ToString());
                                //rptDoc.Subreports["CAM3.rpt"].SetParameterValue("pSMSVerifyTimeStamp", dtDigiDocDtls.Rows[0]["SMSVerifyTimeStamp"].ToString());
                                //rptDoc.Subreports["ScheduleOfCharges.rpt"].SetParameterValue("pApplicantName", dtDigiDocDtls.Rows[0]["ApplicantName"].ToString());
                                //rptDoc.Subreports["ScheduleOfCharges.rpt"].SetParameterValue("pSMSVerifyTimeStamp", dtDigiDocDtls.Rows[0]["SMSVerifyTimeStamp"].ToString());

                                rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, vSancId + "_Digital_Document");
                                rptDoc.Dispose();
                                Response.ClearContent();
                                Response.ClearHeaders();
                            }
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                ds = null;
                dtAppFrm1 = null; dtAppFrm2 = null; dtSancLetter = null;
                dtEMISchedule = null;
                dtLoanAgr = null; dtAuthLetter = null; dtKotak = null;
            }
        }

        protected void lbRepaySch_Click(object sender, EventArgs e)
        {
            GetData("PDF");
        }
        private void GetData(string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RepaySche.rpt";
            string vBranch = Session[gblValue.BrName].ToString();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            CReports oRpt = null;
            try
            {
                string vLoanNo = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
                oRpt = new CReports();
                dt = oRpt.rptRepaySchedule(vLoanNo, vBrCode, "N");
                if (pFormat == "PDF")
                {
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt);
                        rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                        rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                        rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                        rptDoc.SetParameterValue("pBranch", vBranch);
                        rptDoc.SetParameterValue("pTitle", "Repayment Schedule");
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_Repayment_Schedule");
                        Response.ClearHeaders();
                        Response.ClearContent();
                    }
                }

            }
            finally
            {
                dt = null;
                oRpt = null;
            }
        }

        protected void lbDigitalSignature_Click(object sender, EventArgs e)
        {
            if (hdnIsDigiDoc.Value == "Y")
            {
                string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
                CReports ORp = new CReports();
                string vLoanAppId = ORp.GetLoanAppId(vSancId);

                //var req = new RequestBody()
                //{
                //    vPostCredential = new PostCredential()
                //    {
                //        UserID = "CENTRUMSMEUAT",
                //        Password = "ABCD*1234",
                //        PartnerID = "PNB"
                //    },
                //    vPostDigiDocOTPDataSet = new PostDigiDocOTPDataSet()
                //    {
                //        LoanAppNo = vLoanAppId,
                //        MacID = GetMACAddress()
                //    }
                //};
                //string vRequestUrl = ConfigurationManager.AppSettings["vRequestUrl"];
                //string vRequestData = JsonConvert.SerializeObject(req);
                //string vResponseData = CallApi(vRequestData, vRequestUrl);            
                //dynamic res = JsonConvert.DeserializeObject(vResponseData);
                string vUrl = ConfigurationManager.AppSettings["vRequestUrl"];
                string vToken = "";
                Random ran = new Random();
                String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                int length = 100;
                String vRandomToken = "";
                for (int i = 0; i < length; i++)
                {
                    int a = ran.Next(b.Length); //string.Lenght gets the size of string
                    vRandomToken = vRandomToken + b.ElementAt(a);
                }

                vToken = vRandomToken;
                CMember Omem = new CMember();
                Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vUrl, vToken, "A");

                if (vErr == 0)
                {
                    string url = vUrl + "?vLoanApp=" + vLoanAppId + "&vToken=" + vToken;
                    string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("Failed: Without agreement digital sign ia not possible");
            }
        }

        protected void lbUnAssisted_Click(object sender, EventArgs e)
        {
            if (hdnIsDigiDoc.Value == "Y")
            {
                string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
                DataTable dtMob = null;
                CReports ORp = new CReports();
                CMember oMem = null;
                string vLoanAppId = ORp.GetLoanAppId(vSancId);
                string vUrl = ConfigurationManager.AppSettings["vRequestUrl"];
                string pSrtUrl = ConfigurationManager.AppSettings["vSrtUrl"];
                string vToken = "";
                Random ran = new Random();
                String b = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                int length = 100;
                String vRandomToken = "";
                for (int i = 0; i < length; i++)
                {
                    int a = ran.Next(b.Length); //string.Lenght gets the size of string
                    vRandomToken = vRandomToken + b.ElementAt(a);
                }
                vToken = vRandomToken;
                CMember Omem = new CMember();
                Int32 vErr = Omem.SaveInitiateDigitalDoc(vLoanAppId, GetMACAddress(), vUrl, vToken, "U");
                if (vErr == 0)
                {
                    dtMob = new DataTable();
                    oMem = new CMember();
                    dtMob = oMem.GetDisbSMSMemMob(vSancId, gblFuction.setDate("01-01-1900"));
                    if (dtMob.Rows.Count > 0)
                    {
                        string vMemMobNo = Convert.ToString(dtMob.Rows[0]["MobNo"]).Trim();
                        oMem = new CMember();
                        Int64 vDigiDocId = oMem.GetDigitalDocId(vLoanAppId, vToken);
                        string url = pSrtUrl + "?p=" + vToken.Substring(0, 5) + Base64Encode(Convert.ToString(vDigiDocId));
                        //string vMessageBody = "Thank you for loan application no " + vLoanAppId + " with CML. Please verify loan documents " + url + " Centrum Microcredit Ltd.";
                        string vMessageBody = "Thank you for loan application no " + vLoanAppId + " with Unity SFB. Please verify loan documents " + url;
                        string vRe = SendSMS(vMemMobNo, vMessageBody);
                        string[] arr = vRe.Split('|');
                        string vSuccessStat = arr[0];
                        if (vSuccessStat.ToString().Trim().ToLower() == "success")
                        {
                            gblFuction.AjxMsgPopup("Success:Successfully Sent.");
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Failed:Unable to deliver.");
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Failed:Mobile no not Present.");
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("Failed:Contact to system admin");
                }
            }
            else
            {
                gblFuction.AjxMsgPopup("Failed: Without agreement digital sign ia not possible.");
            }
        }

        public string CallApi(string Requestdata, string URL)
        {
            string postURL = URL;
            string responsedata = string.Empty;
            try
            {
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = "application/json";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                byte[] data = Encoding.UTF8.GetBytes(Requestdata);
                request.ContentLength = data.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                {
                    var API_Response = streamReader.ReadToEnd(); ;
                    responsedata = API_Response.ToString().Trim();
                }
            }
            finally { }
            return responsedata;
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }

        public string SendSMS(string pMobileNo, string pMsgBody)
        {
            string result = "";
            WebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                //Predefined template can not be changed.if want to change then need to be verified by Gupshup (SMS provider)
                string vMsgBody = string.Empty;
                //vMsgBody = pMsgBody;
                vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-1"));
                //vMsgBody = System.Web.HttpUtility.UrlEncode(pMsgBody, System.Text.Encoding.GetEncoding("ISO-8859-15"));
                //********************************************************************
                String sendToPhoneNumber = pMobileNo;
                String userid = "2000203137";
                String passwd = "UnitySFB@1";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                //String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007861727120133444&principalEntityId=1001301154610005078&mask=ARGUSS&v=1.1&format=text";
                // String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1007128044179320524&principalEntityId=1001301154610005078&mask=CENOTP&v=1.1&format=text";
                String url = "https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to=" + sendToPhoneNumber + "&msg=" + vMsgBody + "&msg_type=TEXT" + "&userid=" + userid + "&auth_scheme=plain" + "&password=" + passwd + "&dltTemplateId=1707163472077801532&principalEntityId=1701163041834094915&mask=UNTYBK&v=1.1&format=text";
                request = WebRequest.Create(url);
                // Send the 'HttpWebRequest' and wait for response.
                response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader reader = new System.IO.StreamReader(stream, ec);
                reader = new System.IO.StreamReader(stream);
                result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            catch (Exception exp)
            {
                result = "Error sending SMS.." + exp.ToString();
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return result;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
    public class RootObject
    {
        public string merchantID { get; set; }
        public string merchantName { get; set; }
        public string payee { get; set; }
        public string amount { get; set; }
        public string payeeAccountNo { get; set; }

    }

    public class RootObjectResponseWITHQR
    {
        public string merchantID { get; set; }
        public string merchantName { get; set; }
        public string payee { get; set; }
        public string amount { get; set; }
        public string payeeAccountNo { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string QR { get; set; }
    }

    public class RequestBody
    {
        public PostCredential vPostCredential { get; set; }
        public PostDigiDocOTPDataSet vPostDigiDocOTPDataSet { get; set; }
    }

    public class PostCredential
    {
        public string UserID { get; set; }
        public string Password { get; set; }
        public string PartnerID { get; set; }
    }

    public class PostDigiDocOTPDataSet
    {
        public string LoanAppNo { get; set; }
        public string MacID { get; set; }
    }


}