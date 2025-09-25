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
    public partial class LegalLOD : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                PopBranch();
                PendLODBrCode();
                LODBrCode();
                popCustomer();
                txtFrmDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddDays(-30));
                txtToDt.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtLODGenDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                txtVerificDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
                PendingLegLODList();
                LODList();
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
                this.PageHeading = "LEGAL LOD (LIST OF DOCUMENTS) Verification";
                //this.ShowPageHeading = true;
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                //this.ShowHOMenu = false;
                this.GetModuleByRole(mnuID.mnuLegalLOD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanReport == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "LEGAL LOD (LIST OF DOCUMENTS)", false);
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
        private void PendLODBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForLOD(Session[gblValue.BrnchCode].ToString(),"P");
            if (dt.Rows.Count > 0)
            {
                ddlPenLODLegBr.DataSource = dt;
                ddlPenLODLegBr.DataValueField = "BranchCode";
                ddlPenLODLegBr.DataTextField = "BranchName";
                ddlPenLODLegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlPenLODLegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlPenLODLegBr.DataSource = null;
                ddlPenLODLegBr.DataBind();
            }
        }
        private void LODBrCode()
        {
            DataTable dt = new DataTable();
            CDisburse oDisb = new CDisburse();
            dt = oDisb.GetBranchCodeForLOD(Session[gblValue.BrnchCode].ToString(), "A");
            if (dt.Rows.Count > 0)
            {
                ddlLODLegBr.DataSource = dt;
                ddlLODLegBr.DataValueField = "BranchCode";
                ddlLODLegBr.DataTextField = "BranchName";
                ddlLODLegBr.DataBind();
                if (Session[gblValue.BrnchCode].ToString() == "0000")
                {
                    ListItem liSel = new ListItem("ALL", "0000");
                    ddlLODLegBr.Items.Insert(0, liSel);
                }
            }
            else
            {
                ddlLODLegBr.DataSource = null;
                ddlLODLegBr.DataBind();
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
        protected void gvPendLODList_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    ds = oCG.GetPenLODBySanctId(vSanId, Convert.ToInt32(Session[gblValue.UserId]));
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
                        foreach (GridViewRow gr in gvPendLODList.Rows)
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
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtCurierName.Text = dt.Rows[0]["CourierName"].ToString();
                        txtPODDocNo.Text = dt.Rows[0]["PODDocNo"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        txtVerifiedBy.Text = dt.Rows[0]["LODVerBy"].ToString();
                        if (dt.Rows[0]["LegalLODVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        tabLODnSanc.ActiveTabIndex = 2;
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
                ds = null;
                dt = null;
                dt1 = null;
                oCG = null;
            }
        }
        protected void gvLODDone_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    ds = oCG.GetLODBySanctId(vSanId);
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
                        foreach (GridViewRow gr in gvPendLODList.Rows)
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
                        txtLnAppNo.Text = dt.Rows[0]["LoanAppID"].ToString();
                        txtCurierName.Text = dt.Rows[0]["CourierName"].ToString();
                        txtPODDocNo.Text = dt.Rows[0]["PODDocNo"].ToString();
                        hdIsDisb.Value = dt.Rows[0]["IsDisbursed"].ToString();
                        txtVerifiedBy.Text = dt.Rows[0]["LODVerBy"].ToString();
                        txtVerificDate.Text = dt.Rows[0]["LegalLODVerificationDate"].ToString();
                        if (dt.Rows[0]["LegalLODVerifyYN"].ToString() == "Y")
                            chkVerify.Checked = true;
                        else
                            chkVerify.Checked = false;
                        tabLODnSanc.ActiveTabIndex = 2;
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
                ds = null;
                dt = null;
                dt1 = null;
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
            int pLegalLODVerifyBy = 0;
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
            string pLegalLODVerifyYN = "N";
            if (chkVerify.Checked == true)
                pLegalLODVerifyYN = "Y";
            else
                pLegalLODVerifyYN = "N";
            pLoanAppNo = txtLnAppNo.Text.ToString();
            if (txtVerificDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Verification Date can not be blank ...");
                return false;
            }
            DateTime pLODVerDate = gblFuction.setDate(txtVerificDate.Text);
            pCourierName = txtCurierName.Text.ToString();
            pDocNo = txtPODDocNo.Text.ToString();
            pLegalLODVerifyBy = Convert.ToInt32(Session[gblValue.UserId]);
            CMember OMem = new CMember();
            Int32 vErr = 0;
            if (Mode == "Save")
            {
                vErr = OMem.SaveLegLOD(pSancId, pLoanAppNo, pLegalLODVerifyYN, pLODVerDate, pLegalLODVerifyBy, 0, "Save");
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
                vErr = OMem.SaveLegLOD(pSancId, pLoanAppNo, pLegalLODVerifyYN, pLODVerDate, pLegalLODVerifyBy, 0, "Delete");
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
                PendLODBrCode();
                LODBrCode();
                PendingLegLODList();
                LODList();
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
                    PendLODBrCode();
                    LODBrCode();
                    PendingLegLODList();
                    LODList();
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
        private void PendingLegLODList()
        {
            DataTable dt = null;
            CApplication oCA = null;
            string vMode = string.Empty, vBrCode = string.Empty;
            try
            {
                vBrCode = (string)Session[gblValue.BrnchCode];
                oCA = new CApplication();
                dt = oCA.GetPenLODList(ddlPenLODLegBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvPendLODList.DataSource = dt;
                    gvPendLODList.DataBind();
                }
                else
                {
                    gvPendLODList.DataSource = null;
                    gvPendLODList.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void LODList()
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
                dt = oCA.GetLODList(pFDate, pTDate, pSearch, ddlLODLegBr.SelectedValue.ToString());
                if (dt.Rows.Count > 0)
                {
                    gvLODDone.DataSource = dt;
                    gvLODDone.DataBind();
                }
                else
                {
                    gvLODDone.DataSource = null;
                    gvLODDone.DataBind();
                }
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        protected void ddlPenLODLegBr_SelectedIndexChanged(object sender, EventArgs e)
        {
            PendingLegLODList();
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LODList();
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
        protected void lbLOD_Click(object sender, EventArgs e)
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
                DateTime pLODGenDate = gblFuction.setDate(txtLODGenDate.Text);
                ds = Omem.GetLODRecordByLoanAppId(vSancId, pLODGenDate);
                if (ds.Tables.Count > 0)
                {
                    Session["LODPrint"] = ds;
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "", "window.open('LODPrint.aspx','LOD','toolbar=no,status=no,menubar=no,scrollbars=1,directories=no');", true);
                }
                else
                {
                    gblFuction.AjxMsgPopup("No Record Found For that Customer...");
                    return;

                }


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
            txtLODGenDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtVerificDate.Text = Convert.ToString(Session[gblValue.LoginDate]);
            txtVerifiedBy.Text = "";
        }
    }
}