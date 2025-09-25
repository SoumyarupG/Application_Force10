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
    public partial class LegalLOA : CENTRUMBAse
    {
        protected int vPgNo = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                PendLOABrCode();
                LOABrCode();
                popCustomer();
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-30));
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtLOAGenDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PendingLegLOAList();
                LOAList();
                ddlCust.Enabled = false;
                ddlSancNo.Enabled = false;
                btnSave.Enabled = false;
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
        private void PendLOABrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForLOA(Session[gblValue.BrnchCode].ToString(),"P");
            if (dt.Rows.Count > 0)
            {
                ddlPenLOALegBr.DataSource = dt;
                ddlPenLOALegBr.DataValueField = "BranchCode";
                ddlPenLOALegBr.DataTextField = "BranchName";
                ddlPenLOALegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlPenLOALegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlPenLOALegBr.DataSource = null;
                ddlPenLOALegBr.DataBind();
            }
        }
        private void LOABrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForLOA(Session[gblValue.BrnchCode].ToString(), "A");
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
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "")
                    Response.Redirect("~/Login.aspx", false);

                this.Menu = false;
                this.PageHeading = "LEGAL LOA (Letter Of Acknowledgement)";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLegalMODTD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "LEGAL LOA (Letter Of Acknowledgement)", false);
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
        protected void gvPendLOAList_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    dt = oCG.GetLOADtlBySanctId(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendLOAList.Rows)
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
                       // chkVerify.Enabled = false;
                       // txtVerificDate.Enabled = false;



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
                        if (dt.Rows[0]["LegLOAVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        if(dt.Rows[0]["LegLOAVerificationDate"].ToString()=="")
                            txtVerificDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                        else
                            txtVerificDate.Text = dt.Rows[0]["LegLOAVerificationDate"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
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
        protected void gvLOADone_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    dt = oCG.GetLOADtlBySanctId(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvPendLOAList.Rows)
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
                       // chkVerify.Enabled = true;
                        txtVerificDate.Enabled = true;
                        btnSave.Enabled = true;
                      

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

                        if (dt.Rows[0]["LegLOAVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        if (dt.Rows[0]["LegLOAVerificationDate"].ToString() == "")
                            txtVerificDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                        else
                            txtVerificDate.Text = dt.Rows[0]["LegLOAVerificationDate"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
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
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string pSancId = "", pLOAVerifyYN = "";
            if (((Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string) != "-1")
                pSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
            else
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to update verification details");
                return;
            }
            
            if (chkVerify.Checked == true)
                pLOAVerifyYN = "Y";
            else
                pLOAVerifyYN = "N";
            if (chkVerify.Checked == true && txtVerificDate.Text == "")
            {
                gblFuction.AjxMsgPopup("LOA Verification Date can not be blank...");
                return;
            }
            DateTime pLOAVerificationDt = gblFuction.setDate(txtVerificDate.Text);
            CMember OMem = new CMember();
            Int32 vErr = 0;
            vErr = OMem.SaveLOAVerification(pSancId, pLOAVerifyYN, pLOAVerificationDt,0);
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                PendLOABrCode();
                LOABrCode();
                PendingLegLOAList();
                LOAList();
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
        private void PendingLegLOAList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                // vMode = rdbSel.SelectedValue;
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenLOAList(ddlPenLOALegBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvPendLOAList.DataSource = dt;
                    gvPendLOAList.DataBind();
                }
                else
                {
                    gvPendLOAList.DataSource = null;
                    gvPendLOAList.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void LOAList()
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
                dt = oCA.GetLOAList(pFDate, pTDate, pSearch,ddlLOALegBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvLOADone.DataSource = dt;
                    gvLOADone.DataBind();
                }
                else
                {
                    gvLOADone.DataSource = null;
                    gvLOADone.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void ddlPenLOALegBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            PendingLegLOAList();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LOAList();
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
        protected void lbLOA_Click(object sender, EventArgs e)
        {
            string vSancId = (Request[ddlSancNo.UniqueID] as string == null) ? ddlSancNo.SelectedValue : Request[ddlSancNo.UniqueID] as string;
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
                DateTime pLOAGenDate = gblFuction.setDate(txtLOAGenDate.Text);
                ds = Omem.GetLOARecordByLoanAppId(vSancId, pLOAGenDate);
                if (ds.Tables.Count > 0)
                {
                    Session["LOAPrint"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('LOAPrint.aspx','LOA','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }


            }

        }
    }
}