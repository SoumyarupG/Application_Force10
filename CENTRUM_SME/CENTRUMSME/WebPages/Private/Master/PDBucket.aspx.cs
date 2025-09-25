using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class PDBucket : CENTRUMBAse
    {
        protected int vPgNo = 1;
        protected int cPgNo = 1; 

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                // txtAdmDtApp.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                ViewState["CustID"] = null;
                ViewState["CustType"] = null;
                hdUserID.Value = this.UserID.ToString();
                mView.ActiveViewIndex = 0;
                LoadPendingPDBucket(1);
                StatusButton("View");
                txtFinalPDDate.Text = Session[gblValue.LoginDate].ToString();
                //txttext.Attributes.Add("onkeypress", "return numericOnly(this);");
                pnlCMPD.Visible = false;
                pnlRMPD.Visible = false;
                //if (Session[gblValue.DesignationID].ToString() == "1")//BM
                //{
                //    pnlBMPD.Visible = true;
                   
                //}
                //else if (Session[gblValue.DesignationID].ToString() == "4")//CM
                //{
                //    pnlBMPD.Visible = true;
                   
                //}
                //else if (Session[gblValue.DesignationID].ToString() == "14")//RM
                //{
                //    pnlBMPD.Visible = true;
                   
                //}
                //else if (Session[gblValue.DesignationID].ToString() == "15")//BM & CAO
                //{
                //    pnlBMPD.Visible = true;                   
                //}
            }

        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Personal Discussion";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuPD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = true;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Personal Discussion", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        private void ViewAcess()
        {
            if (mView.ActiveViewIndex == 1)
            {
                this.Menu = false;
                this.PageHeading = "Loan Application";
                this.GetModuleByRole(mnuID.mnuAppLnApplication);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 2)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion";
                this.GetModuleByRole(mnuID.mnuPDBM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 3)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By CM";
                this.GetModuleByRole(mnuID.mnuPDCM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 4)
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion By Risk";
                this.GetModuleByRole(mnuID.mnuPDRisk);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else
            {
                this.Menu = false;
                this.PageHeading = "Personal Discussion";
                this.GetModuleByRole(mnuID.mnuPD);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
        }
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = false;
                    // ClearControls();
                    break;
                case "Show":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnAppAdd.Enabled = false;
                    //btnAppEdit.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAppAdd.Enabled = true;
                    //btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    //btnAppAdd.Visible = false;
                    //btnAppEdit.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void LoadPendingPDBucket(int cPgNo)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            Int32 vRows = 0;

            try
            {
                ds = oMem.GetPendingPDBucketList(txtSearch.Text.Trim(), vBrCode,rblPDList.SelectedValue,cPgNo,ref vRows);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLoanApp.DataSource = dt1;
                        gvLoanApp.DataBind();

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
                    else
                    {
                        gvLoanApp.DataSource = null;
                        gvLoanApp.DataBind();

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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                oMem = null;
            }

        }

        private int CalTotPgs(double pRows)
        {
            int totPg = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return totPg;
        }

        // Population of All Tab's Data on Click of Show Information in Loan Application
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {
                // get PD By BM
                LoadPDByBM(pLnAppId);
                // get PD By BM From Question Answer
                GetTotalIncomeFromQuestionAnswerBM(pLnAppId, "BM");
                // get PD By CM
                LoadPDByCM(pLnAppId);
                // get PD By CM From Question Answer
                GetTotalIncomeFromQuestionAnswerCM(pLnAppId, "CM");
                // get PD By Risk
                LoadPDByRisk(pLnAppId);
                // get PD By RM From Question Answer
                GetTotalIncomeFromQuestionAnswerRM(pLnAppId, "RM");
                // get Property Details
                GetPropertyDtlByLnAppId(pLnAppId);
                // Get Level Approval Range
                GetLevelApprovalRange();
                // get PD Final Approve
                LoadPDFinalApprove(pLnAppId);
                // get Data For IIR
                GetRecForIIR(pLnAppId);
                // get Data For Obligation and FOIR
                GetRecForObligation(pLnAppId);
                // Get Record For LTV
                GetLTVValByLnAppId(pLnAppId);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void ChangePage(object sender, CommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            switch (e.CommandName)
            {
                case "Previous":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) - 1;
                    break;
                case "Next":
                    cPgNo = Int32.Parse(lblCurrentPage.Text) + 1;
                    break;
            }
            LoadPendingPDBucket(cPgNo);            
        }

        #region Loan Application
        protected void gvLoanApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vApplicantionID = "";
            vApplicantionID = Convert.ToString(e.CommandArgument);
            ViewState["LoanAppId"] = vApplicantionID;
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
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
                ShowInitialLoanAppData(vApplicantionID, vBrCode);
            }
            else if (e.CommandName == "cmdShowInfo")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowLoanRelationDetails(vApplicantionID);
            }
        }
        protected void btnBackApplication_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 0;
            ViewAcess();
        }
        protected Control GetControlThatCausedPostBack(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button || c is System.Web.UI.WebControls.ImageButton)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control;

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadPendingPDBucket(cPgNo);
        }
        private void ShowAllInitialLoanApp(string vCustId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataTable dt = new DataTable();
            dt = ca.GetAllInitialLoanApp(vCustId);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    gvLoanApp.DataSource = dt;
                    gvLoanApp.DataBind();
                }
                else
                {
                    gvLoanApp.DataSource = null;
                    gvLoanApp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;
            }

        }
        private void ShowInitialLoanAppData(string pLnAppId, string vBrCode)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            try
            {
                ds = ca.GetInitLoanDtlByLoanId(pLnAppId, vBrCode);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];

                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 1;
                    // ViewAcess();
                    hdfApplicationId.Value = pLnAppId;
                    //btSaveApplication.Enabled = false;
                    //btnUpdateApplication.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppNo.Text = Convert.ToString(dt.Rows[0]["LoanAppNo"]).Trim();
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["ApplicationDt"]).Trim();
                    PopApplicant();
                    ddlLoanApplicantname.SelectedIndex = ddlLoanApplicantname.Items.IndexOf(ddlLoanApplicantname.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustID"])));
                    PopPurpose();
                    PopLoanType();
                    ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                    ddlLnScheme.SelectedIndex = ddlLnScheme.Items.IndexOf(ddlLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
                    EnableMachinDtl(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
                    txtAppLnAmt.Text = Convert.ToString(dt.Rows[0]["AppAmount"]).Trim();
                    txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]).Trim();
                    txtLnPurposeDetails.Text = dt.Rows[0]["MachDtl"].ToString();
                    popSourceName();
                    ddlSourceName.SelectedIndex = ddlSourceName.Items.IndexOf(ddlSourceName.Items.FindByValue(Convert.ToString(dt.Rows[0]["SourceID"])));
                    ddlLnAppStatus.SelectedIndex = ddlLnAppStatus.Items.IndexOf(ddlLnAppStatus.Items.FindByValue(Convert.ToString(dt.Rows[0]["PassYN"].ToString())));
                    //if (dt.Rows[0]["PassYN"].ToString() == "Y")
                    //    chkLnAppPass.Checked = true;
                    //else
                    //    chkLnAppPass.Checked = false;
                    txtLnAppPassDt.Text = dt.Rows[0]["PassorRejDate"].ToString();
                    txtLnAppRejReason.Text = dt.Rows[0]["RejReason"].ToString();
                    txtAddTerms.Text = dt.Rows[0]["AddTerms"].ToString();
                }
                if (dt1.Rows.Count > 0)
                {
                    gvCoAppDtl.DataSource = dt1;
                    gvCoAppDtl.DataBind();
                }
                else
                {
                    gvCoAppDtl.DataSource = null;
                    gvCoAppDtl.DataBind();
                }
                if (dt2.Rows.Count > 0)
                {
                    ViewState["MLAsset"] = dt2;
                    gvMLAsset.DataSource = dt2;
                    gvMLAsset.DataBind();
                }
                else
                {
                    popMLAsset();
                }
                if (dt3.Rows.Count > 0)
                {
                    gvApp.DataSource = dt3;
                    gvApp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ca = null;
                dt = null;

            }

        }
        protected void btnUpdateApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearApplication();
                LoadPendingPDBucket(1);
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDeleteApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                else if (vStateEdit == "Delete")
                    lblMsg.Text = gblPRATAM.DeleteMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearApplication();
                LoadPendingPDBucket(1);
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveLoanAppRecords(string Mode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty, vApplicantId = "", vAppId = "", vMachDtl = "", vAddTerms = "", vErrDesc = "";
            Int32 vErr = 0, vPurpId = 0, vTenure = 0, vYrNo = 0, vLnTypeId = 0, vSourceId = 0;
            decimal vLnAmt = 0;

            CApplication oCG = new CApplication();
            string vXml = "", vXmlAsset = "", vPassYN = "", vRejReason = "";
            try
            {
                if (txtAppDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Application Date");
                    txtAppDt.Focus();
                    return false;
                }
                else if (txtLnAppPassDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Loan Application Pass/Rejection Date Can Not Be Blank...");
                    txtLnAppPassDt.Focus();
                    return false;
                }
                else if (ddlLoanApplicantname.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Applicant Name...");
                    ddlLoanApplicantname.Focus();
                    return false;
                }
                else if (ddlLnPurpose.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Purpose...");
                    ddlLnPurpose.Focus();
                    return false;
                }
                else if (ddlLnScheme.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Loan Type...");
                    ddlLnScheme.Focus();
                    return false;
                }
                else if (txtTenure.Text == "")
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be Empty...");
                    txtTenure.Focus();
                    return false;
                }
                else if (txtAppLnAmt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Loan Amount Can Not Be Empty...");
                    txtAppLnAmt.Focus();
                    return false;
                }
                else
                {

                }
                DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
                DateTime vPassorRejDate = gblFuction.setDate(txtLnAppPassDt.Text);
                vLnTypeId = Convert.ToInt32(Request[ddlLnScheme.UniqueID] as string);

                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add(new DataColumn("CoApplicantId"));
                dtXml.Columns.Add(new DataColumn("CoApplicantName"));
                dtXml.Columns.Add(new DataColumn("ReportID"));
                dtXml.Columns.Add(new DataColumn("ScoreValue"));
                dtXml.Columns.Add(new DataColumn("IsActive"));
                foreach (GridViewRow gr in gvCoAppDtl.Rows)
                {
                    //if (((CheckBox)gr.FindControl("chkCoApp")).Checked == true)
                    //{
                    dr = dtXml.NewRow();
                    dr["CoApplicantId"] = ((Label)gr.FindControl("lblCoApplicantId")).Text;
                    dr["CoApplicantName"] = ((Label)gr.FindControl("lblCoAppName")).Text;
                    dr["ReportID"] = ((Label)gr.FindControl("lblReportID")).Text;
                    dr["ScoreValue"] = ((Label)gr.FindControl("lblScoreValue")).Text;
                    if (((CheckBox)gr.FindControl("chkCoApp")).Checked == true)
                        dr["IsActive"] = "Y";
                    else
                        dr["IsActive"] = "N";
                    dtXml.Rows.Add(dr);
                    dtXml.AcceptChanges();
                    //}
                }
                dtXml.TableName = "Table1";
                // In Case of Machinary Loan ,Machine Asset Details will be inserted .... 
                if (vLnTypeId == 2)
                {
                    DataRow drAsset = null;
                    DataTable dtXmlAsset = new DataTable();
                    dtXmlAsset.Columns.Add("SlNo", typeof(int));
                    dtXmlAsset.Columns.Add("MachDesc", typeof(string));
                    dtXmlAsset.Columns.Add("MachSupp", typeof(string));
                    dtXmlAsset.Columns.Add("Place", typeof(string));
                    dtXmlAsset.Columns.Add("Make", typeof(string));
                    dtXmlAsset.Columns.Add("Model", typeof(string));
                    dtXmlAsset.Columns.Add("Amount", typeof(decimal));

                    foreach (GridViewRow gr in gvMLAsset.Rows)
                    {
                        if (((TextBox)gr.FindControl("txtMachDesc")).Text != "" && ((TextBox)gr.FindControl("txtAmount")).Text != "")
                        {
                            drAsset = dtXmlAsset.NewRow();
                            drAsset["SlNo"] = ((Label)gr.FindControl("lblSLNoMLAsset")).Text;
                            drAsset["MachDesc"] = ((TextBox)gr.FindControl("txtMachDesc")).Text;
                            drAsset["MachSupp"] = ((TextBox)gr.FindControl("txtMachSupp")).Text;
                            drAsset["Place"] = ((TextBox)gr.FindControl("txtPlace")).Text;
                            drAsset["Make"] = ((TextBox)gr.FindControl("txtMake")).Text;
                            drAsset["Model"] = ((TextBox)gr.FindControl("txtModel")).Text;
                            drAsset["Amount"] = ((TextBox)gr.FindControl("txtAmount")).Text;
                            dtXmlAsset.Rows.Add(drAsset);
                            dtXmlAsset.AcceptChanges();
                        }
                    }
                    dtXmlAsset.TableName = "Table2";

                    using (StringWriter oSW = new StringWriter())
                    {
                        dtXmlAsset.WriteXml(oSW);
                        vXmlAsset = oSW.ToString().Replace("12:00:00AM", "").Trim();
                    }
                }


                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00AM", "").Trim();
                }

                vApplicantId = (Request[ddlLoanApplicantname.UniqueID] as string == null) ? ddlLoanApplicantname.SelectedValue : Request[ddlLoanApplicantname.UniqueID] as string;
                vPurpId = Convert.ToInt32(Request[ddlLnPurpose.UniqueID] as string);
                //if (chkLnAppPass.Checked == true)
                //    vPassYN = "Y";
                //else
                //    vPassYN = "N";
                vPassYN = (Request[ddlLnAppStatus.UniqueID] as string == null) ? ddlLnAppStatus.SelectedValue : Request[ddlLnAppStatus.UniqueID] as string;
                vRejReason = (Request[txtLnAppRejReason.UniqueID] as string == null) ? txtLnAppRejReason.Text : Request[txtLnAppRejReason.UniqueID] as string;
                vAddTerms = (Request[txtAddTerms.UniqueID] as string == null) ? txtAddTerms.Text : Request[txtAddTerms.UniqueID] as string;
                vSourceId = Convert.ToInt32(Request[ddlSourceName.UniqueID] as string);
                vYrNo = Convert.ToInt32(Session[gblValue.FinYrNo].ToString());
                if (Request[txtTenure.UniqueID] as string != "")
                    vTenure = Convert.ToInt32(Request[txtTenure.UniqueID] as string);
                if (vTenure == 0)
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be zero...");
                    txtTenure.Focus();
                    return false;
                }
                if (txtAppLnAmt.Text.ToString() != "")
                    decimal.TryParse(txtAppLnAmt.Text.ToString(), out vLnAmt);
                if (vLnAmt == 0)
                {
                    gblFuction.AjxMsgPopup("Loan Amount Can Not Be zero...");
                    txtAppLnAmt.Focus();
                    return false;
                }
                vBrCode = (string)Session[gblValue.BrnchCode];
                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = hdfApplicationId.Value;
                vMachDtl = txtLnPurposeDetails.Text.ToString().Trim();


                if (Mode == "Save")
                {
                    //if (ValidateFieldsForLnApp() == false) return false;

                    vErr = oCG.SaveInitialApplication(ref vAppNo, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure, "N",
                      vBrCode, Convert.ToInt32(hdUserID.Value), "I", vYrNo, vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                      vRejReason, vAddTerms,ref vErrDesc);
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
                        ViewState["LoanAppId"] = vAppNo;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
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
                    // if (ValidateFieldsForLnApp() == false) return false;

                    oCG = new CApplication();
                    vErr = oCG.UpdateApplication(vAppId, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure,
                         vBrCode, Convert.ToInt32(hdUserID.Value), "Edit", vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                         vRejReason, vAddTerms, ref vErrDesc);
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
                else if (Mode == "Delete")
                {
                    //oCG = new CApplication();
                    //vErr = oCG.ChkEditApplication(vAppId, vMemId, vBrCode);
                    //if (vErr > 0)
                    //{
                    //    gblFuction.MsgPopup("Approved or Cancelled Application cannot be Deleted.");
                    //    return false;
                    //}
                    //else
                    //{
                    vErr = oCG.UpdateApplication(vAppId, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure,
                        vBrCode, Convert.ToInt32(hdUserID.Value), "Delete", vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                        vRejReason, vAddTerms, ref vErrDesc);
                    if (vErr == 0)
                        vResult = true;
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
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
                oCG = null;
            }
        }
        private void EnableControl(Boolean Status)
        {
        }
        private void popSourceName()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "SourceID", "SourceName", "SourceMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                ddlSourceName.DataSource = dt;
                if (dt.Rows.Count > 0)
                {
                    ddlSourceName.DataTextField = "SourceName";
                    ddlSourceName.DataValueField = "SourceID";
                    ddlSourceName.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlSourceName.Items.Insert(0, oli1);
                }
                else
                {
                    ddlSourceName.DataSource = null;
                    ddlSourceName.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oGb = null;
                dt = null;
            }
        }
        private void PopApplicant()
        {
            string vBrCode = (string)Session[gblValue.BrnchCode];
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetApplicantList(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlLoanApplicantname.DataSource = dt;
                    ddlLoanApplicantname.DataTextField = "CompanyName";
                    ddlLoanApplicantname.DataValueField = "CustId";
                    ddlLoanApplicantname.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLoanApplicantname.Items.Insert(0, oli);
                }
                else
                {
                    ddlLoanApplicantname.DataSource = null;
                    ddlLoanApplicantname.DataBind();
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
        private void PopPurpose()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetLoanPurposeList();
                if (dt.Rows.Count > 0)
                {
                    ddlLnPurpose.DataSource = dt;
                    ddlLnPurpose.DataTextField = "PurposeName";
                    ddlLnPurpose.DataValueField = "PurposeId";
                    ddlLnPurpose.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlLnPurpose.Items.Insert(0, oli);
                }
                else
                {
                    ddlLnPurpose.DataSource = null;
                    ddlLnPurpose.DataBind();
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
        private void PopLoanType()
        {
            DataTable dt = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDate = gblFuction.setDate(Convert.ToString(Session[gblValue.LoginDate]));
            CLoanScheme oLS = new CLoanScheme();
            try
            {
                dt = oLS.GetActiveLnSchemePG();
                if (dt.Rows.Count > 0)
                {
                    ddlLnScheme.DataTextField = "LoanTypeName";
                    ddlLnScheme.DataValueField = "LoanTypeId";
                    ddlLnScheme.DataSource = dt;
                    ddlLnScheme.DataBind();
                    ListItem oItm = new ListItem();
                    oItm.Text = "<--- Select --->";
                    oItm.Value = "-1";
                    ddlLnScheme.Items.Insert(0, oItm);
                }
                else
                {
                    ddlLnScheme.DataSource = null;
                    ddlLnScheme.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oLS = null;
            }
        }
        private Boolean ValidateFieldsForLnApp()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CApplication oCG = oCG = new CApplication();
            DateTime vAppDate = vLoginDt;

            if (txtAppDt.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Loan Application Date cannot be empty.");
                vResult = false;
            }
            else
            {
                vAppDate = gblFuction.setDate(txtAppDt.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    gblFuction.MsgPopup("Loan Application Date should be Financial Year.");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    gblFuction.MsgPopup("Loan Application Date should not be greater than login date.");
                    vResult = false;
                }
            }
            if (Request[ddlLoanApplicantname.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Applicant...");
                vResult = false;
            }

            return vResult;
        }
        private void ClearApplication()
        {
            txtAppNo.Text = "";
            txtAppDt.Text = "";
            ddlLoanApplicantname.SelectedIndex = -1;
            ddlLnPurpose.SelectedIndex = -1;
            ddlLnScheme.SelectedIndex = -1;
            ddlSourceName.SelectedIndex = -1;
            txtAppLnAmt.Text = "";
            txtTenure.Text = "";
            txtLnPurposeDetails.Text = "";
            txtAddTerms.Text = "";
        }
        protected void ddlLnScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLnScheme.SelectedValue != "-1")
            {
                int LnType = Convert.ToInt32(ddlLnScheme.SelectedValue);
                EnableMachinDtl(LnType);
            }
        }
        protected void ddlLoanApplicantname_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            string vCustId = ddlLoanApplicantname.SelectedValue.ToString();
            try
            {
                dt = OMem.GetCoAppByCustId(vCustId);
                if (dt.Rows.Count > 0)
                {
                    gvCoAppDtl.DataSource = dt;
                    gvCoAppDtl.DataBind();
                }
                else
                {
                    gvCoAppDtl.DataSource = null;
                    gvCoAppDtl.DataBind();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                OMem = null;
            }
        }
        protected void EnableMachinDtl(int LnType)
        {
            try
            {
                // 1 for BL,2 for ML, 3 for STL
                if (LnType == 2)
                {
                    gvMLAsset.Enabled = true;
                }
                else
                {
                    gvMLAsset.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddAsset_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["MLAsset"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvMLAsset.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLNoMLAsset = (Label)gvMLAsset.Rows[curRow].FindControl("lblSLNoMLAsset");
                TextBox txtMachDesc = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMachDesc");
                TextBox txtMachSupp = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMachSupp");
                TextBox txtMake = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtMake");
                TextBox txtPlace = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtPlace");
                TextBox txtModel = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtModel");
                TextBox txtAmount = (TextBox)gvMLAsset.Rows[curRow].FindControl("txtAmount");

                dt.Rows[curRow][0] = lblSLNoMLAsset.Text;
                if (txtMachDesc.Text == "")
                {
                    gblFuction.AjxMsgPopup("Machine Description Can Not Be Empty");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtMachDesc.Text);
                }
                dt.Rows[curRow][2] = (txtMachSupp.Text);
                dt.Rows[curRow][3] = (txtPlace.Text);
                dt.Rows[curRow][4] = (txtMake.Text);
                dt.Rows[curRow][5] = (txtModel.Text);
                if (txtAmount.Text == "")
                {
                    gblFuction.AjxMsgPopup("Amount Can Not Be Empty");
                    return;
                }
                else
                {
                    dt.Rows[curRow][6] = (txtAmount.Text);
                }

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvMLAsset.Rows[vRow].FindControl("lblSLNoMLAsset")).Text) + 1;
                dt.AcceptChanges();

                ViewState["MLAsset"] = dt;
                gvMLAsset.DataSource = dt;
                gvMLAsset.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ImDelAsset_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["MLAsset"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["MLAsset"] = dt;
                    gvMLAsset.DataSource = dt;
                    gvMLAsset.DataBind();
                }
                else
                {
                    popMLAsset();
                    gblFuction.MsgPopup("First Row can not be deleted.");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void popMLAsset()
        {
            DataTable dt = null;
            try
            {
                dt = GetMachAsset();
                ViewState["MLAsset"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvMLAsset.DataSource = dt;
                    gvMLAsset.DataBind();
                }
                else
                {
                    gvMLAsset.DataSource = null;
                    gvMLAsset.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private DataTable GetMachAsset()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("MachDesc", typeof(string));
            dt.Columns.Add("MachSupp", typeof(string));
            dt.Columns.Add("Place", typeof(string));
            dt.Columns.Add("Make", typeof(string));
            dt.Columns.Add("Model", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;
        }
        protected void gvCoAppDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Button btnVerify = (Button)e.Row.FindControl("btnVerify");
                //ImageButton btnDownloadCoAppEnq = (ImageButton)e.Row.FindControl("btnDownloadCoAppEnq");
                Label lblIsActive = (Label)e.Row.FindControl("lblIsActive");
                CheckBox chkCoApp = (CheckBox)e.Row.FindControl("chkCoApp");
                if (lblIsActive.Text == "N")
                {
                    chkCoApp.Checked = false;
                }
                else
                {
                    chkCoApp.Checked = true;
                }
            }
        }

        #endregion

        #region PD BY BM

        #region Income

        protected void btnBMPDIncome_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            double vTotalSellingPrice = 0, vTotalCostPrice = 0;
            DataRow dr;
            if (ViewState["BMPDIncome"] == null)
            {
                SetBMPDIncome();
            }
            dt = (DataTable)ViewState["BMPDIncome"];
            //txtBMPDDIncome.Text = Convert.ToString((Request[txtBMPDDIncome.UniqueID] as string == null) ? txtBMPDDIncome.Text : Request[txtBMPDDIncome.UniqueID] as string);
            //txtBMPDMIncome.Text = Convert.ToString((Request[txtBMPDMIncome.UniqueID] as string == null) ? txtBMPDMIncome.Text : Request[txtBMPDMIncome.UniqueID] as string);

            foreach (GridViewRow gr in gvBMPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlBMPDIncome.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlBMPDIncome.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlBMPDIncome.SelectedItem.Text;
            dr["ID"] = ddlBMPDIncome.SelectedValue;
            dr["Sold"] = txtBMPDIncomeSold.Text == "" ? "0" : txtBMPDIncomeSold.Text;
            dr["Price"] = txtBMPDIncomePrice.Text == "" ? "0" : txtBMPDIncomePrice.Text;
            dr["Cost"] = txtBMPDCostPrice.Text == "" ? "0" : txtBMPDCostPrice.Text;

            vTotalSellingPrice = Convert.ToDouble(txtBMPDIncomeSold.Text == "" ? "0" : txtBMPDIncomeSold.Text) * Convert.ToDouble(txtBMPDIncomePrice.Text == "" ? "0" : txtBMPDIncomePrice.Text);
            vTotalCostPrice = Convert.ToDouble(txtBMPDIncomeSold.Text == "" ? "0" : txtBMPDIncomeSold.Text) * Convert.ToDouble(txtBMPDCostPrice.Text == "" ? "0" : txtBMPDCostPrice.Text);
            dr["TotalSell"] = vTotalSellingPrice.ToString();
            dr["TotalCost"] = vTotalCostPrice.ToString();
            dr["Margin"] = Convert.ToString(vTotalSellingPrice - vTotalCostPrice);

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["BMPDIncome"] = dt;

            gvBMPDIncome.DataSource = dt;
            gvBMPDIncome.DataBind();

            TotalBMPDIncome();

            ddlBMPDIncome.SelectedIndex = -1;
            txtBMPDIncomeSold.Text = "";
            txtBMPDIncomePrice.Text = "";
            txtBMPDCostPrice.Text = "";
            //txtBMPDDIncome.Text = "";
            //txtBMPDMIncome.Text = "";
            ddlBMPDIncome.Focus();

        }

        private void SetBMPDIncome()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Sold");
            dt.Columns.Add("Price");
            dt.Columns.Add("Cost");
            dt.Columns.Add("TotalSell");
            dt.Columns.Add("TotalCost");
            dt.Columns.Add("Margin");
            dt.AcceptChanges();
            ViewState["BMPDIncome"] = dt;
        }

        protected void btnDelBMPDIncome_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["BMPDIncome"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["BMPDIncome"] = dt;
                    gvBMPDIncome.DataSource = dt;
                    gvBMPDIncome.DataBind();

                    TotalBMPDIncome();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        #region Expenses

        protected void btnBMPDExpenses_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            DataRow dr;
            if (ViewState["BMPDExpenses"] == null)
            {
                SetBMPDExpenses();
            }
            dt = (DataTable)ViewState["BMPDExpenses"];
            txtBMPDMExpenses.Text = Convert.ToString((Request[txtBMPDMExpenses.UniqueID] as string == null) ? txtBMPDMExpenses.Text : Request[txtBMPDMExpenses.UniqueID] as string);

            foreach (GridViewRow gr in gvBMPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlBMPDExpenses.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlBMPDExpenses.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlBMPDExpenses.SelectedItem.Text;
            dr["ID"] = ddlBMPDExpenses.SelectedValue;
            dr["Used"] = txtBMPDExpensesUsed.Text == "" ? "0" : txtBMPDExpensesUsed.Text;
            dr["Expenses"] = txtBMPDExpensesExp.Text == "" ? "0" : txtBMPDExpensesExp.Text;
            dr["MExpenses"] = txtBMPDMExpenses.Text == "" ? "0" : txtBMPDMExpenses.Text;

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["BMPDExpenses"] = dt;

            gvBMPDExpenses.DataSource = dt;
            gvBMPDExpenses.DataBind();

            TotalBMPDExpenses();

            ddlBMPDExpenses.SelectedIndex = -1;
            txtBMPDExpensesUsed.Text = "";
            txtBMPDExpensesExp.Text = "";
            txtBMPDMExpenses.Text = "";

            ddlBMPDExpenses.Focus();

        }

        private void SetBMPDExpenses()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Used");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MExpenses");
            dt.AcceptChanges();
            ViewState["BMPDExpenses"] = dt;
        }

        protected void btnDelBMPDExpenses_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["BMPDExpenses"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["BMPDExpenses"] = dt;

                    gvBMPDExpenses.DataSource = dt;
                    gvBMPDExpenses.DataBind();

                    TotalBMPDExpenses();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        protected void btnAddBMPD_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("B");
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Personal Discussion..");
                    return;
                }
                else
                {
                    ds = oMem.GetLnAppDetailsForPD(ViewState["LoanAppId"].ToString());
                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        if (dt1.Rows.Count > 0)
                        {
                            txtBMPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                            txtBMPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                            hdBMPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                            txtBMPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                            txtBMPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                            txtBMPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                            txtBMPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                            txtBMPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                            hdBMLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                            hdBMFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                            txtBMPDRemarks.Text = "";
                            hdBMPDId.Value = "";
                            chkBMPDCoApp.Checked = false;
                            ddlBMPDCoApplicant.Enabled = false;
                        }
                        else
                        {
                            txtBMPDLnApp.Text = "";
                            txtBMPDCustName.Text = "";
                            hdBMPDCustId.Value = "";
                            txtBMPDAppDt.Text = "";
                            txtBMPDAppAmt.Text = "";
                            txtBMPDAppTenure.Text = "";
                            txtBMPDAppType.Text = "";
                            txtBMPDLnPurpose.Text = "";
                            txtBMPDRemarks.Text = "";
                            hdBMPDId.Value = "";
                            hdBMLNSanction.Value = "";
                            hdBMFinalPDStatus.Value = "";
                            chkBMPDCoApp.Checked = false;
                            ddlBMPDCoApplicant.Enabled = false;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ddlBMPDCoApplicant.Items.Clear();
                            ddlBMPDCoApplicant.DataTextField = "CoAppName";
                            ddlBMPDCoApplicant.DataValueField = "CoApplicantId";
                            ddlBMPDCoApplicant.DataSource = dt2;
                            ddlBMPDCoApplicant.DataBind();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlBMPDCoApplicant.Items.Insert(0, oItm);
                        }
                        else
                        {
                            ddlBMPDCoApplicant.Items.Clear();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlBMPDCoApplicant.Items.Insert(0, oItm);
                        }


                        mView.ActiveViewIndex = 2;
                        ViewAcess();
                        txtBMPDDate.Text = Session[gblValue.LoginDate].ToString();
                        btnPDBMSave.Enabled = true;
                        btnPDBMUpdate.Enabled = false;
                        btnPDBMDelete.Enabled = false;
                        btnPDBMBack.Enabled = true;


                    }
                    else
                    {
                        btnPDBMSave.Enabled = false;
                        btnPDBMUpdate.Enabled = false;
                        btnPDBMDelete.Enabled = false;
                        btnPDBMBack.Enabled = true;
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
            }
        }
        private void BindAssment(string pType)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                ds = oMem.GetIncExpAssessment();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (pType == "B")
                    {
                        ddlBMPDIncome.DataTextField = "Product";
                        ddlBMPDIncome.DataValueField = "ID";
                        ddlBMPDIncome.DataSource = dt1;
                        ddlBMPDIncome.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlBMPDIncome.Items.Insert(0, oItm);

                        gvBMPDIncome.DataSource = null;
                        gvBMPDIncome.DataBind();

                        ViewState["BMPDIncome"] = null;

                        //gvBMPDIncome.DataSource = dt1;
                        //gvBMPDIncome.DataBind();

                        ddlBMPDExpenses.DataTextField = "Product";
                        ddlBMPDExpenses.DataValueField = "ID";
                        ddlBMPDExpenses.DataSource = dt2;
                        ddlBMPDExpenses.DataBind();
                        ListItem oItm1 = new ListItem();
                        oItm1.Text = "<--- Select --->";
                        oItm1.Value = "-1";
                        ddlBMPDExpenses.Items.Insert(0, oItm1);

                        gvBMPDExpenses.DataSource = null;
                        gvBMPDExpenses.DataBind();

                        ViewState["BMPDExpenses"] = null;

                        //gvBMPDExpenses.DataSource = dt2;
                        //gvBMPDExpenses.DataBind();
                    }
                    if (pType == "C")
                    {
                        ddlCMPDIncome.DataTextField = "Product";
                        ddlCMPDIncome.DataValueField = "ID";
                        ddlCMPDIncome.DataSource = dt1;
                        ddlCMPDIncome.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlCMPDIncome.Items.Insert(0, oItm);

                        gvCMPDIncome.DataSource = null;
                        gvCMPDIncome.DataBind();

                        //gvCMPDIncome.DataSource = dt1;
                        //gvCMPDIncome.DataBind();

                        ViewState["CMPDIncome"] = null;

                        ddlCMPDExpenses.DataTextField = "Product";
                        ddlCMPDExpenses.DataValueField = "ID";
                        ddlCMPDExpenses.DataSource = dt2;
                        ddlCMPDExpenses.DataBind();
                        ListItem oItm1 = new ListItem();
                        oItm1.Text = "<--- Select --->";
                        oItm1.Value = "-1";
                        ddlCMPDExpenses.Items.Insert(0, oItm1);

                        gvCMPDExpenses.DataSource = null;
                        gvCMPDExpenses.DataBind();

                        ViewState["CMPDExpenses"] = null;

                        //gvCMPDExpenses.DataSource = null;
                        //gvCMPDExpenses.DataBind();
                    }
                    if (pType == "R")
                    {

                        ddlRMPDIncome.DataTextField = "Product";
                        ddlRMPDIncome.DataValueField = "ID";
                        ddlRMPDIncome.DataSource = dt1;
                        ddlRMPDIncome.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlRMPDIncome.Items.Insert(0, oItm);

                        gvRiskPDIncome.DataSource = null;
                        gvRiskPDIncome.DataBind();

                        //gvRiskPDIncome.DataSource = dt1;
                        //gvRiskPDIncome.DataBind();

                        ViewState["RMPDIncome"] = null;



                        gvRiskPDExpenses.DataSource = null;
                        gvRiskPDExpenses.DataBind();

                        ddlRMPDExpenses.DataTextField = "Product";
                        ddlRMPDExpenses.DataValueField = "ID";
                        ddlRMPDExpenses.DataSource = dt2;
                        ddlRMPDExpenses.DataBind();
                        ListItem oItm1 = new ListItem();
                        oItm1.Text = "<--- Select --->";
                        oItm1.Value = "-1";
                        ddlRMPDExpenses.Items.Insert(0, oItm1);


                        ViewState["RMPDExpenses"] = null;

                        //gvRiskPDExpenses.DataSource = dt2;
                        //gvRiskPDExpenses.DataBind();
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
            }
        }
        private void LoadPDByBM(string pLnAppId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetPDListForLoanApplication(pLnAppId, "B");
                if (dt.Rows.Count > 0)
                {
                    gvPDByBM.DataSource = dt;
                    gvPDByBM.DataBind();
                }
                else
                {
                    gvPDByBM.DataSource = null;
                    gvPDByBM.DataBind();
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
        private void GetTotalIncomeFromQuestionAnswerBM(string pLnAppId, string pPDDoneBy)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetTotalIncome(pLnAppId, pPDDoneBy);
                if (dt.Rows.Count > 0)
                {
                    gvBMQuesAns.DataSource = dt;
                    gvBMQuesAns.DataBind();
                }
                else
                {
                    gvBMQuesAns.DataSource = null;
                    gvBMQuesAns.DataBind();
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
        decimal pIncBM = 0, pNetIncBM = 0, pExpBM = 0;
        decimal pIncCM = 0, pNetIncCM = 0, pExpCM = 0;
        decimal pIncRM = 0, pNetIncRM = 0, pExpRM = 0;
        protected void gvBMQuesAns_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGrossIncBM = (Label)e.Row.FindControl("lblGrossIncBM");
                Label lblGrossExpBM = (Label)e.Row.FindControl("lblGrossExpBM");
                Label lblNetIncBM = (Label)e.Row.FindControl("lblNetIncBM");
                if (lblGrossIncBM.Text != "")
                    pIncBM = pIncBM + Convert.ToDecimal(lblGrossIncBM.Text);
                if (lblGrossExpBM.Text != "")
                    pExpBM = pExpBM + Convert.ToDecimal(lblGrossExpBM.Text);
                if (lblNetIncBM.Text != "")
                    pNetIncBM = pNetIncBM + Convert.ToDecimal(lblNetIncBM.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotGrosIncBM = (Label)e.Row.FindControl("lblTotGrosIncBM");
                lblTotGrosIncBM.Text = Math.Round(pIncBM, 0).ToString();
                Label lblTotGrosExpBM = (Label)e.Row.FindControl("lblTotGrosExpBM");
                lblTotGrosExpBM.Text = Math.Round(pExpBM, 0).ToString();
                Label lblTotNetIncBM = (Label)e.Row.FindControl("lblTotNetIncBM");
                lblTotNetIncBM.Text = Math.Round(pNetIncBM, 0).ToString();
            }
        }
        private void GetTotalIncomeFromQuestionAnswerCM(string pLnAppId, string pPDDoneBy)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetTotalIncome(pLnAppId, pPDDoneBy);
                if (dt.Rows.Count > 0)
                {
                    gvCMQuesAns.DataSource = dt;
                    gvCMQuesAns.DataBind();
                }
                else
                {
                    gvCMQuesAns.DataSource = null;
                    gvCMQuesAns.DataBind();
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
        protected void gvCMQuesAns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGrossIncCM = (Label)e.Row.FindControl("lblGrossIncCM");
                Label lblGrossExpCM = (Label)e.Row.FindControl("lblGrossExpCM");
                Label lblNetIncCM = (Label)e.Row.FindControl("lblNetIncCM");
                if (lblGrossIncCM.Text != "")
                    pIncCM = pIncCM + Convert.ToDecimal(lblGrossIncCM.Text);
                if (lblGrossExpCM.Text != "")
                    pExpCM = pExpCM + Convert.ToDecimal(lblGrossExpCM.Text);
                if (lblNetIncCM.Text != "")
                    pNetIncCM = pNetIncCM + Convert.ToDecimal(lblNetIncCM.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotGrosIncCM = (Label)e.Row.FindControl("lblTotGrosIncCM");
                lblTotGrosIncCM.Text = Math.Round(pIncCM, 0).ToString();
                Label lblTotGrosExpCM = (Label)e.Row.FindControl("lblTotGrosExpCM");
                lblTotGrosExpCM.Text = Math.Round(pExpCM, 0).ToString();
                Label lblTotNetIncCM = (Label)e.Row.FindControl("lblTotNetIncCM");
                lblTotNetIncCM.Text = Math.Round(pNetIncCM, 0).ToString();
            }
        }
        private void GetTotalIncomeFromQuestionAnswerRM(string pLnAppId, string pPDDoneBy)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetTotalIncome(pLnAppId, pPDDoneBy);
                if (dt.Rows.Count > 0)
                {
                    gvRMQuesAns.DataSource = dt;
                    gvRMQuesAns.DataBind();
                }
                else
                {
                    gvRMQuesAns.DataSource = null;
                    gvRMQuesAns.DataBind();
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
        protected void gvRMQuesAns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGrossIncRM = (Label)e.Row.FindControl("lblGrossIncRM");
                Label lblGrossExpRM = (Label)e.Row.FindControl("lblGrossExpRM");
                Label lblNetIncRM = (Label)e.Row.FindControl("lblNetIncRM");
                if (lblGrossIncRM.Text != "")
                    pIncRM = pIncCM + Convert.ToDecimal(lblGrossIncRM.Text);
                if (lblGrossExpRM.Text != "")
                    pExpRM = pExpRM + Convert.ToDecimal(lblGrossExpRM.Text);
                if (lblNetIncRM.Text != "")
                    pNetIncRM = pNetIncCM + Convert.ToDecimal(lblNetIncRM.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotGrosIncRM = (Label)e.Row.FindControl("lblTotGrosIncRM");
                lblTotGrosIncRM.Text = Math.Round(pIncRM, 0).ToString();
                Label lblTotGrosExpRM = (Label)e.Row.FindControl("lblTotGrosExpRM");
                lblTotGrosExpRM.Text = Math.Round(pExpRM, 0).ToString();
                Label lblTotNetIncRM = (Label)e.Row.FindControl("lblTotNetIncRM");
                lblTotNetIncRM.Text = Math.Round(pNetIncRM, 0).ToString();
            }
        }
        protected void gvCRQuesAns_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblGrossIncRM = (Label)e.Row.FindControl("lblGrossIncRM");
                Label lblNetIncRM = (Label)e.Row.FindControl("lblNetIncRM");
                if (lblGrossIncRM.Text != "")
                    pIncRM = pIncCM + Convert.ToDecimal(lblGrossIncRM.Text);
                if (lblNetIncRM.Text != "")
                    pNetIncRM = pNetIncRM + Convert.ToDecimal(lblNetIncRM.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotGrosIncRM = (Label)e.Row.FindControl("lblTotGrosIncRM");
                lblTotGrosIncRM.Text = pIncRM.ToString();

                Label lblTotNetIncRM = (Label)e.Row.FindControl("lblTotNetIncRM");
                lblTotNetIncRM.Text = Math.Round(pNetIncRM, 0).ToString();
            }
        }
        protected void chkBMPDCoApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBMPDCoApp.Checked == true)
            {
                ddlBMPDCoApplicant.Enabled = true;
                ddlBMPDCoApplicant.SelectedIndex = -1;
            }
            else
            {
                ddlBMPDCoApplicant.Enabled = false;
                ddlBMPDCoApplicant.SelectedIndex = -1;

            }
        }
        protected void gvPDByBM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            string vReportID = "";
            vReportID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByBM.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindBMAssmentView(Convert.ToInt32(vReportID));

            }
            else if (e.CommandName == "cmdEdit")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnEditInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByBM.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnEditInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindBMPDDeatils(Convert.ToInt32(vReportID));
                gvBMPDIncomeView.DataSource = null;
                gvBMPDIncomeView.DataBind();
                gvBMPDExpensesView.DataSource = null;
                gvBMPDExpensesView.DataBind();
            }
        }
        private void BindBMPDDeatils(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("B");
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt1.Rows.Count > 0)
                    {
                        txtBMPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                        txtBMPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                        hdBMPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                        txtBMPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                        txtBMPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                        txtBMPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                        txtBMPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                        txtBMPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                        txtBMPDDate.Text = dt1.Rows[0]["PDDate"].ToString();
                        txtBMPDRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                        hdBMPDId.Value = dt1.Rows[0]["PDId"].ToString();
                        hdBMLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                        hdBMFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                        chkBMPDCoApp.Checked = false;
                        ddlBMPDCoApplicant.Enabled = false;
                    }
                    else
                    {
                        txtBMPDLnApp.Text = "";
                        txtBMPDCustName.Text = "";
                        hdBMPDCustId.Value = "";
                        txtBMPDAppDt.Text = "";
                        txtBMPDAppAmt.Text = "";
                        txtBMPDAppTenure.Text = "";
                        txtBMPDAppType.Text = "";
                        txtBMPDLnPurpose.Text = "";
                        txtBMPDDate.Text = "";
                        txtBMPDRemarks.Text = "";
                        hdBMPDId.Value = "";
                        hdBMLNSanction.Value = "";
                        hdBMFinalPDStatus.Value = "";
                        chkBMPDCoApp.Checked = false;
                        ddlBMPDCoApplicant.Enabled = false;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        ddlBMPDCoApplicant.Items.Clear();
                        ddlBMPDCoApplicant.DataTextField = "CoAppName";
                        ddlBMPDCoApplicant.DataValueField = "CoApplicantId";
                        ddlBMPDCoApplicant.DataSource = dt2;
                        ddlBMPDCoApplicant.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlBMPDCoApplicant.Items.Insert(0, oItm);
                    }
                    else
                    {
                        ddlBMPDCoApplicant.Items.Clear();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlBMPDCoApplicant.Items.Insert(0, oItm);
                    }



                    if (dt3.Rows.Count > 0)
                    {
                        gvBMPDIncome.DataSource = dt3;
                        gvBMPDIncome.DataBind();

                        ViewState["BMPDIncome"] = dt3;
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvBMPDExpenses.DataSource = dt4;
                        gvBMPDExpenses.DataBind();

                        ViewState["BMPDExpenses"] = dt4;
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");
                    TextBox txtTotalMarginPer = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtTotalMarginPer");


                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();
                        txtTotalMarginPer.Text = Convert.ToString((Convert.ToDecimal(txtSumTotalMargin.Text) / Convert.ToDecimal(txtSumTotSellPrice.Text)) * 100);
                        if (dt1.Rows[0]["CoAppID"].ToString() != "")
                        {
                            chkBMPDCoApp.Checked = true;
                            ddlBMPDCoApplicant.Enabled = true;
                            ddlBMPDCoApplicant.SelectedIndex = ddlBMPDCoApplicant.Items.IndexOf(ddlBMPDCoApplicant.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CoAppID"])));

                        }
                    }

                    btnPDBMSave.Enabled = false;
                    btnPDBMUpdate.Enabled = true;
                    btnPDBMDelete.Enabled = true;
                    btnPDBMBack.Enabled = true;
                    mView.ActiveViewIndex = 2;
                    ViewAcess();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oMem = null;
            }
        }
        private void BindBMAssmentView(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt3.Rows.Count > 0)
                    {
                        gvBMPDIncomeView.DataSource = dt3;
                        gvBMPDIncomeView.DataBind();
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvBMPDExpensesView.DataSource = dt4;
                        gvBMPDExpensesView.DataBind();
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncomeView.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncomeView.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncomeView.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpensesView.FooterRow.FindControl("txtTotalMExpenses");

                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();
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
            }
        }
        private void GetRelationPropertyOwnerTypeOfOwner()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();
            DataTable dt6 = new DataTable();
            DataTable dt7 = new DataTable();
            DataTable dt8 = new DataTable();
            DataTable dt9 = new DataTable();
            DataTable dt10 = new DataTable();
            DataTable dt11 = new DataTable();
            DataTable dt12 = new DataTable();
            DataTable dt13 = new DataTable();
            DataTable dt14 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                ds = oMem.GetRelationPropertyOwnerTypeOfOwner();
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
                    if (dt1.Rows.Count > 0)
                    {
                        ddlQAAppRelation.DataSource = dt1;
                        ddlQAAppRelation.DataTextField = "Relation";
                        ddlQAAppRelation.DataValueField = "RelationId";
                        ddlQAAppRelation.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAAppRelation.Items.Insert(0, oli);

                    }
                    if (dt2.Rows.Count > 0)
                    {
                        ddlQAMStatus.DataSource = dt2;
                        ddlQAMStatus.DataTextField = "MaritalName";
                        ddlQAMStatus.DataValueField = "MaritalId";
                        ddlQAMStatus.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAMStatus.Items.Insert(0, oli);
                    }
                    if (dt3.Rows.Count > 0)
                    {
                        ddlQAPO.DataSource = dt3;
                        ddlQAPO.DataTextField = "PropOwnerShip";
                        ddlQAPO.DataValueField = "PropOwnerShipId";
                        ddlQAPO.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAPO.Items.Insert(0, oli);
                    }
                    if (dt8.Rows.Count > 0)
                    {
                        ddlQAOT.DataSource = dt8;
                        ddlQAOT.DataTextField = "PropertypeName";
                        ddlQAOT.DataValueField = "PropertyTypeID";
                        ddlQAOT.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAOT.Items.Insert(0, oli);
                    }
                    if (dt5.Rows.Count > 0)
                    {
                        ddlQABT.DataSource = dt5;
                        ddlQABT.DataTextField = "BusinessType";
                        ddlQABT.DataValueField = "BusinessTypeId";
                        ddlQABT.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQABT.Items.Insert(0, oli);
                    }
                    if (dt6.Rows.Count > 0)
                    {
                        ddlQASalMode.DataSource = dt6;
                        ddlQASalMode.DataTextField = "SalCredMode";
                        ddlQASalMode.DataValueField = "SalCredModeId";
                        ddlQASalMode.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQASalMode.Items.Insert(0, oli);
                    }
                    if (dt7.Rows.Count > 0)
                    {
                        ddlQASalType.DataSource = dt7;
                        ddlQASalType.DataTextField = "SalType";
                        ddlQASalType.DataValueField = "SalTypeId";
                        ddlQASalType.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQASalType.Items.Insert(0, oli);
                    }
                    if (dt8.Rows.Count > 0)
                    {
                        ddlQAPropType.DataSource = dt8;
                        ddlQAPropType.DataTextField = "PropertypeName";
                        ddlQAPropType.DataValueField = "PropertyTypeID";
                        ddlQAPropType.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAPropType.Items.Insert(0, oli);

                        ddlQAProType.DataSource = dt8;
                        ddlQAProType.DataTextField = "PropertypeName";
                        ddlQAProType.DataValueField = "PropertyTypeID";
                        ddlQAProType.DataBind();
                        ListItem oli2 = new ListItem("<--Select-->", "-1");
                        ddlQAProType.Items.Insert(0, oli2);
                    }
                    if (dt9.Rows.Count > 0)
                    {
                        ddlQAPenInc.DataSource = dt9;
                        ddlQAPenInc.DataTextField = "PenIncome";
                        ddlQAPenInc.DataValueField = "PenIncomeId";
                        ddlQAPenInc.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAPenInc.Items.Insert(0, oli);
                    }
                    if (dt10.Rows.Count > 0)
                    {
                        ddlQAPenFrom.DataSource = dt10;
                        ddlQAPenFrom.DataTextField = "PensionFrom";
                        ddlQAPenFrom.DataValueField = "PensionFromId";
                        ddlQAPenFrom.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAPenFrom.Items.Insert(0, oli);
                    }
                    if (dt11.Rows.Count > 0)
                    {
                        ddlQAWorkType.DataSource = dt11;
                        ddlQAWorkType.DataTextField = "WorkType";
                        ddlQAWorkType.DataValueField = "WorkTypeId";
                        ddlQAWorkType.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAWorkType.Items.Insert(0, oli);
                    }
                    if (dt12.Rows.Count > 0)
                    {
                        ddlQAProNature.DataSource = dt12;
                        ddlQAProNature.DataTextField = "PropNature";
                        ddlQAProNature.DataValueField = "PropNatureId";
                        ddlQAProNature.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAProNature.Items.Insert(0, oli);
                    }
                    if (dt13.Rows.Count > 0)
                    {
                        ddlQAProJud.DataSource = dt13;
                        ddlQAProJud.DataTextField = "PropJud";
                        ddlQAProJud.DataValueField = "PropJudId";
                        ddlQAProJud.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQAProJud.Items.Insert(0, oli);
                    }
                    if (dt14.Rows.Count > 0)
                    {
                        ddlQALnPurpose.DataSource = dt14;
                        ddlQALnPurpose.DataTextField = "PurposeName";
                        ddlQALnPurpose.DataValueField = "PurposeId";
                        ddlQALnPurpose.DataBind();
                        ListItem oli = new ListItem("<--Select-->", "-1");
                        ddlQALnPurpose.Items.Insert(0, oli);
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
            }
        }
        protected void txtSoldBMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtPrice.Focus();
            TotalBMPDIncome();
        }
        protected void txtPriceBMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDays.Focus();
            TotalBMPDIncome();
        }
        protected void txtDaysBMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvBMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDIncome.Focus();
            TotalBMPDIncome();
        }
        private void TotalBMPDIncome()
        {
            decimal vSell = 0, vCost = 0, vMargin = 0,vMarginPer=0;
            foreach (GridViewRow gr in gvBMPDIncome.Rows)
            {
                TextBox txtTotSellPrice = (TextBox)gr.FindControl("txtTotSellPrice");
                TextBox txtTotalCost = (TextBox)gr.FindControl("txtTotalCost");
                TextBox txtMargin = (TextBox)gr.FindControl("txtMargin");                
                if (!string.IsNullOrEmpty(txtTotSellPrice.Text))
                {
                    vSell = vSell + Convert.ToDecimal(txtTotSellPrice.Text);
                }
                if (!string.IsNullOrEmpty(txtTotalCost.Text))
                {
                    vCost = vCost + Convert.ToDecimal(txtTotalCost.Text);
                }
                if (!string.IsNullOrEmpty(txtMargin.Text))
                {
                    vMargin = vMargin + Convert.ToDecimal(txtMargin.Text);
                }            

            }
            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMarginPer = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtTotalMarginPer");

            txtSumTotSellPrice.Text = vSell.ToString("0.00");
            txtSumTotalCost.Text = vCost.ToString("0.00");
            txtSumTotalMargin.Text = vMargin.ToString("0.00");
            txtTotalMarginPer.Text = ((vMargin / vSell) * 100).ToString("0.00");
        }
        protected void txtUsedBMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtExpenses.Focus();
            TotalBMPDExpenses();
        }
        protected void txtExpensesBMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvBMPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtMExpenses.Focus();
            TotalBMPDExpenses();
        }
        private void TotalBMPDExpenses()
        {
            decimal vMExpenses = 0;
            foreach (GridViewRow gr in gvBMPDExpenses.Rows)
            {
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");
                if (!string.IsNullOrEmpty(txtMExpenses.Text))
                {
                    vMExpenses = vMExpenses + Convert.ToDecimal(txtMExpenses.Text);
                }
            }
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");
            txtTotalMExpenses.Text = vMExpenses.ToString("0.00");
        }
        protected void btnPDBMSave_Click(object sender, EventArgs e)
        {

            if (hdBMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Add Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Add Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Add Personal Discussion..");
                return;
            }

            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();

            dtIncome = DtBMPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtBMPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }
            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation

            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtBMPDAppDt.Text) > gblFuction.setDate(txtBMPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkBMPDCoApp.Checked == true)
            {
                if (ddlBMPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion
            if (chkBMPDCoApp.Checked == true)
            {
                vCoAppID = ddlBMPDCoApplicant.SelectedValue;
            }
            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdBMPDCustId.Value, vCoAppID, "B", gblFuction.setDate(txtBMPDDate.Text), txtBMPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                LoadPDByBM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["BMPDExpenses"] = null;
                ViewState["BMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDBMUpdate_Click(object sender, EventArgs e)
        {
            if (hdBMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Update Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Update Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Update Personal Discussion..");
                return;
            }
            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtBMPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtBMPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            if (chkBMPDCoApp.Checked == true)
            {
                vCoAppID = ddlBMPDCoApplicant.SelectedValue;
            }
            if (hdBMPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdBMPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }
            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation


            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtBMPDAppDt.Text) > gblFuction.setDate(txtBMPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkBMPDCoApp.Checked == true)
            {
                if (ddlBMPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion

            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdBMPDCustId.Value, vCoAppID, "B", gblFuction.setDate(txtBMPDDate.Text), txtBMPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                LoadPDByBM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["BMPDExpenses"] = null;
                ViewState["BMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDBMDelete_Click(object sender, EventArgs e)
        {

            if (hdBMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Delete Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Delete Personal Discussion..");
                return;
            }
            if (hdBMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Delete Personal Discussion..");
                return;
            }

            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtBMPDIncome();
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtBMPDExpenses();
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            if (chkBMPDCoApp.Checked == true)
            {
                vCoAppID = ddlBMPDCoApplicant.SelectedValue;
            }
            if (hdBMPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdBMPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }


            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdBMPDCustId.Value, vCoAppID, "B", gblFuction.setDate(txtBMPDDate.Text), txtBMPDRemarks.Text
                                , 0, 0, 0, 0
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                LoadPDByBM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["BMPDExpenses"] = null;
                ViewState["BMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }
        }
        protected void btnPDBMBack_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            ViewAcess();


            gvBMPDExpensesView.DataSource = null;
            gvBMPDExpensesView.DataBind();
            gvBMPDIncomeView.DataSource = null;
            gvBMPDIncomeView.DataBind();
            ViewState["BMPDExpenses"] = null;
            ViewState["BMPDIncome"] = null;
        }
        private DataTable DtBMPDIncome()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;

            dt.Columns.Add("ProductID");
            dt.Columns.Add("SoldItem");
            dt.Columns.Add("SellingPrice");
            dt.Columns.Add("CostPrice");
            dt.Columns.Add("TotalSell");
            dt.Columns.Add("TotalCost");
            dt.Columns.Add("Margin");

            dt.AcceptChanges();

            foreach (GridViewRow gr in gvBMPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtSold = (TextBox)gr.FindControl("txtSold");
                TextBox txtSellingPrice = (TextBox)gr.FindControl("txtSellingPrice");
                TextBox txtCostPrice = (TextBox)gr.FindControl("txtCostPrice");
                TextBox txtTotSellPrice = (TextBox)gr.FindControl("txtTotSellPrice");
                TextBox txtTotalCost = (TextBox)gr.FindControl("txtTotalCost");
                TextBox txtMargin = (TextBox)gr.FindControl("txtMargin");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["SoldItem"] = txtSold.Text == "" ? "0" : txtSold.Text;
                dr["SellingPrice"] = txtSellingPrice.Text == "" ? "0" : txtSellingPrice.Text;
                dr["CostPrice"] = txtCostPrice.Text == "" ? "0" : txtCostPrice.Text;
                dr["TotalSell"] = txtTotSellPrice.Text == "" ? "0" : txtTotSellPrice.Text;
                dr["TotalCost"] = txtTotalCost.Text == "" ? "0" : txtTotalCost.Text;
                dr["Margin"] = txtMargin.Text == "" ? "0" : txtMargin.Text;

                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }


            return dt;
        }
        private DataTable DtBMPDExpenses()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;
            dt.Columns.Add("ProductID");
            dt.Columns.Add("UsedItem");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MonthlyExpenses");
            dt.AcceptChanges();

            foreach (GridViewRow gr in gvBMPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtUsed = (TextBox)gr.FindControl("txtUsed");
                TextBox txtExpenses = (TextBox)gr.FindControl("txtExpenses");
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["UsedItem"] = txtUsed.Text == "" ? "0" : txtUsed.Text;
                dr["Expenses"] = txtExpenses.Text == "" ? "0" : txtExpenses.Text;
                dr["MonthlyExpenses"] = txtMExpenses.Text == "" ? "0" : txtMExpenses.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }

            return dt;
        }
        #endregion

        #region PD BY CM

        #region Income

        protected void btnCMPDIncome_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            DataRow dr;
            if (ViewState["CMPDIncome"] == null)
            {
                SetCMPDIncome();
            }
            dt = (DataTable)ViewState["CMPDIncome"];
            txtCMPDDIncome.Text = Convert.ToString((Request[txtCMPDDIncome.UniqueID] as string == null) ? txtCMPDDIncome.Text : Request[txtCMPDDIncome.UniqueID] as string);
            txtCMPDMIncome.Text = Convert.ToString((Request[txtCMPDMIncome.UniqueID] as string == null) ? txtCMPDMIncome.Text : Request[txtCMPDMIncome.UniqueID] as string);

            foreach (GridViewRow gr in gvCMPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlCMPDIncome.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlBMPDIncome.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlCMPDIncome.SelectedItem.Text;
            dr["ID"] = ddlCMPDIncome.SelectedValue;
            dr["Sold"] = txtCMPDIncomeSold.Text == "" ? "0" : txtCMPDIncomeSold.Text;
            dr["Price"] = txtCMPDIncomePrice.Text == "" ? "0" : txtCMPDIncomePrice.Text;
            dr["NODays"] = txtCMPDIncomeDays.Text == "" ? "0" : txtCMPDIncomeDays.Text;
            dr["DIncome"] = txtCMPDDIncome.Text == "" ? "0" : txtCMPDDIncome.Text;
            dr["MIncome"] = txtCMPDMIncome.Text == "" ? "0" : txtCMPDMIncome.Text;
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["CMPDIncome"] = dt;

            gvCMPDIncome.DataSource = dt;
            gvCMPDIncome.DataBind();

            TotalCMPDIncome();

            ddlCMPDIncome.SelectedIndex = -1;
            txtCMPDIncomeSold.Text = "";
            txtCMPDIncomePrice.Text = "";
            txtCMPDIncomeDays.Text = "";
            txtCMPDDIncome.Text = "";
            txtCMPDMIncome.Text = "";
            ddlCMPDIncome.Focus();

        }

        private void SetCMPDIncome()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Sold");
            dt.Columns.Add("Price");
            dt.Columns.Add("NODays");
            dt.Columns.Add("DIncome");
            dt.Columns.Add("MIncome");
            dt.AcceptChanges();
            ViewState["CMPDIncome"] = dt;
        }


        protected void btnDelCMPDIncome_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["CMPDIncome"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["CMPDIncome"] = dt;
                    gvCMPDIncome.DataSource = dt;
                    gvCMPDIncome.DataBind();

                    TotalCMPDIncome();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion


        #region Expenses

        protected void btnCMPDExpenses_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            DataRow dr;
            if (ViewState["CMPDExpenses"] == null)
            {
                SetCMPDExpenses();
            }
            dt = (DataTable)ViewState["CMPDExpenses"];
            txtCMPDMExpenses.Text = Convert.ToString((Request[txtCMPDMExpenses.UniqueID] as string == null) ? txtCMPDMExpenses.Text : Request[txtCMPDMExpenses.UniqueID] as string);

            foreach (GridViewRow gr in gvCMPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlCMPDExpenses.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlCMPDExpenses.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlCMPDExpenses.SelectedItem.Text;
            dr["ID"] = ddlCMPDExpenses.SelectedValue;
            dr["Used"] = txtCMPDExpensesUsed.Text == "" ? "0" : txtCMPDExpensesUsed.Text;
            dr["Expenses"] = txtCMPDExpensesExp.Text == "" ? "0" : txtCMPDExpensesExp.Text;
            dr["MExpenses"] = txtCMPDMExpenses.Text == "" ? "0" : txtCMPDMExpenses.Text;

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["CMPDExpenses"] = dt;

            gvCMPDExpenses.DataSource = dt;
            gvCMPDExpenses.DataBind();

            TotalCMPDExpenses();

            ddlCMPDExpenses.SelectedIndex = -1;
            txtCMPDExpensesUsed.Text = "";
            txtCMPDExpensesExp.Text = "";
            txtCMPDMExpenses.Text = "";

            ddlCMPDExpenses.Focus();

        }

        private void SetCMPDExpenses()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Used");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MExpenses");
            dt.AcceptChanges();
            ViewState["CMPDExpenses"] = dt;
        }

        protected void btnDelCMPDExpenses_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["CMPDExpenses"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["CMPDExpenses"] = dt;

                    gvCMPDExpenses.DataSource = dt;
                    gvCMPDExpenses.DataBind();

                    TotalCMPDExpenses();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        private void LoadPDByCM(string pLnAppId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetPDListForLoanApplication(pLnAppId, "C");
                if (dt.Rows.Count > 0)
                {
                    gvPDByCM.DataSource = dt;
                    gvPDByCM.DataBind();
                }
                else
                {
                    gvPDByCM.DataSource = null;
                    gvPDByCM.DataBind();
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
        protected void btnAddCMPD_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("C");
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Personal Discussion..");
                    return;
                }
                else
                {
                    ds = oMem.GetLnAppDetailsForPD(ViewState["LoanAppId"].ToString());
                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        if (dt1.Rows.Count > 0)
                        {
                            txtCMPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                            txtCMPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                            hdCMPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                            txtCMPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                            txtCMPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                            txtCMPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                            txtCMPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                            txtCMPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                            hdCMLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                            hdCMFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                            txtCMPDRemarks.Text = "";
                            hdCMPDId.Value = "";
                            chkCMPDCoApp.Checked = false;
                            ddlCMPDCoApplicant.Enabled = false;
                        }
                        else
                        {
                            txtCMPDLnApp.Text = "";
                            txtCMPDCustName.Text = "";
                            hdCMPDCustId.Value = "";
                            txtCMPDAppDt.Text = "";
                            txtCMPDAppAmt.Text = "";
                            txtCMPDAppTenure.Text = "";
                            txtCMPDAppType.Text = "";
                            txtCMPDLnPurpose.Text = "";
                            txtCMPDRemarks.Text = "";
                            hdCMPDId.Value = "";
                            hdCMLNSanction.Value = "";
                            hdCMFinalPDStatus.Value = "";
                            chkCMPDCoApp.Checked = false;
                            ddlCMPDCoApplicant.Enabled = false;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ddlCMPDCoApplicant.Items.Clear();
                            ddlCMPDCoApplicant.DataTextField = "CoAppName";
                            ddlCMPDCoApplicant.DataValueField = "CoApplicantId";
                            ddlCMPDCoApplicant.DataSource = dt2;
                            ddlCMPDCoApplicant.DataBind();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlCMPDCoApplicant.Items.Insert(0, oItm);
                        }
                        else
                        {
                            ddlCMPDCoApplicant.Items.Clear();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlCMPDCoApplicant.Items.Insert(0, oItm);
                        }


                        mView.ActiveViewIndex = 3;
                        ViewAcess();
                        txtCMPDDate.Text = Session[gblValue.LoginDate].ToString();
                        btnPDCMSave.Enabled = true;
                        btnPDCMUpdate.Enabled = false;
                        btnPDCMDelete.Enabled = false;
                        btnPDCMBack.Enabled = true;


                    }
                    else
                    {
                        btnPDCMSave.Enabled = false;
                        btnPDCMUpdate.Enabled = false;
                        btnPDCMDelete.Enabled = false;
                        btnPDCMBack.Enabled = true;
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
            }
        }




        protected void chkCMPDCoApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCMPDCoApp.Checked == true)
            {
                ddlCMPDCoApplicant.Enabled = true;
                ddlCMPDCoApplicant.SelectedIndex = -1;
            }
            else
            {
                ddlCMPDCoApplicant.Enabled = false;
                ddlCMPDCoApplicant.SelectedIndex = -1;

            }
        }

        protected void gvPDByCM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            string vReportID = "";
            vReportID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByCM.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindCMAssmentView(Convert.ToInt32(vReportID));

            }
            else if (e.CommandName == "cmdEdit")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnEditInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByCM.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnEditInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindCMPDDeatils(Convert.ToInt32(vReportID));
                gvCMPDIncomeView.DataSource = null;
                gvCMPDIncomeView.DataBind();
                gvCMPDExpensesView.DataSource = null;
                gvCMPDExpensesView.DataBind();
            }
        }

        private void BindCMPDDeatils(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("C");
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt1.Rows.Count > 0)
                    {
                        txtCMPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                        txtCMPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                        hdCMPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                        txtCMPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                        txtCMPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                        txtCMPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                        txtCMPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                        txtCMPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                        txtCMPDDate.Text = dt1.Rows[0]["PDDate"].ToString();
                        txtCMPDRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                        hdCMPDId.Value = dt1.Rows[0]["PDId"].ToString();
                        hdCMLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                        hdCMFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                        chkCMPDCoApp.Checked = false;
                        ddlCMPDCoApplicant.Enabled = false;
                    }
                    else
                    {
                        txtCMPDLnApp.Text = "";
                        txtCMPDCustName.Text = "";
                        hdCMPDCustId.Value = "";
                        txtCMPDAppDt.Text = "";
                        txtCMPDAppAmt.Text = "";
                        txtCMPDAppTenure.Text = "";
                        txtCMPDAppType.Text = "";
                        txtCMPDLnPurpose.Text = "";
                        txtCMPDDate.Text = "";
                        txtCMPDRemarks.Text = "";
                        hdCMPDId.Value = "";
                        hdCMLNSanction.Value = "";
                        hdCMFinalPDStatus.Value = "";
                        chkCMPDCoApp.Checked = false;
                        ddlCMPDCoApplicant.Enabled = false;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        ddlCMPDCoApplicant.Items.Clear();
                        ddlCMPDCoApplicant.DataTextField = "CoAppName";
                        ddlCMPDCoApplicant.DataValueField = "CoApplicantId";
                        ddlCMPDCoApplicant.DataSource = dt2;
                        ddlCMPDCoApplicant.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlCMPDCoApplicant.Items.Insert(0, oItm);
                    }
                    else
                    {
                        ddlCMPDCoApplicant.Items.Clear();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlCMPDCoApplicant.Items.Insert(0, oItm);
                    }



                    if (dt3.Rows.Count > 0)
                    {
                        gvCMPDIncome.DataSource = dt3;
                        gvCMPDIncome.DataBind();

                        ViewState["CMPDIncome"] = dt3;
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvCMPDExpenses.DataSource = dt4;
                        gvCMPDExpenses.DataBind();
                        ViewState["CMPDExpenses"] = dt4;
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");


                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();
                        if (dt1.Rows[0]["CoAppID"].ToString() != "")
                        {
                            chkCMPDCoApp.Checked = true;
                            ddlCMPDCoApplicant.Enabled = true;
                            ddlCMPDCoApplicant.SelectedIndex = ddlCMPDCoApplicant.Items.IndexOf(ddlCMPDCoApplicant.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CoAppID"])));

                        }
                    }

                    btnPDCMSave.Enabled = false;
                    btnPDCMUpdate.Enabled = true;
                    btnPDCMDelete.Enabled = true;
                    btnPDCMBack.Enabled = true;
                    mView.ActiveViewIndex = 3;
                    ViewAcess();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oMem = null;
            }
        }

        private void BindCMAssmentView(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt3.Rows.Count > 0)
                    {
                        gvCMPDIncomeView.DataSource = dt3;
                        gvCMPDIncomeView.DataBind();
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvCMPDExpensesView.DataSource = dt4;
                        gvCMPDExpensesView.DataBind();
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();
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
            }
        }


        protected void txtSoldCMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtPrice.Focus();
            TotalCMPDIncome();
        }


        protected void txtPriceCMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDays.Focus();
            TotalCMPDIncome();
        }

        protected void txtDaysCMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvCMPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDIncome.Focus();
            TotalCMPDIncome();
        }


        private void TotalCMPDIncome()
        {
            decimal vDIncome = 0, vMIncome = 0;
            foreach (GridViewRow gr in gvCMPDIncome.Rows)
            {
                TextBox txtDIncome = (TextBox)gr.FindControl("txtDIncome");
                TextBox txtMIncome = (TextBox)gr.FindControl("txtMIncome");
                if (!string.IsNullOrEmpty(txtDIncome.Text))
                {
                    vDIncome = vDIncome + Convert.ToDecimal(txtDIncome.Text);
                }
                if (!string.IsNullOrEmpty(txtMIncome.Text))
                {
                    vMIncome = vMIncome + Convert.ToDecimal(txtMIncome.Text);
                }
            }
            TextBox txtTotalDIncome = (TextBox)gvCMPDIncome.FooterRow.FindControl("txtTotalDIncome");
            TextBox txtTotalMIncome = (TextBox)gvCMPDIncome.FooterRow.FindControl("txtTotalMIncome");
            txtTotalDIncome.Text = vDIncome.ToString("0.00");
            txtTotalMIncome.Text = vMIncome.ToString("0.00");
        }



        protected void txtUsedCMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtExpenses.Focus();
            TotalCMPDExpenses();
        }

        protected void txtExpensesCMPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvCMPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtMExpenses.Focus();
            TotalCMPDExpenses();
        }

        private void TotalCMPDExpenses()
        {
            decimal vMExpenses = 0;
            foreach (GridViewRow gr in gvCMPDExpenses.Rows)
            {
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");
                if (!string.IsNullOrEmpty(txtMExpenses.Text))
                {
                    vMExpenses = vMExpenses + Convert.ToDecimal(txtMExpenses.Text);
                }
            }
            TextBox txtTotalMExpenses = (TextBox)gvCMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");
            txtTotalMExpenses.Text = vMExpenses.ToString("0.00");
        }

        protected void btnPDCMSave_Click(object sender, EventArgs e)
        {
            if (hdCMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Add Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Add Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Add Personal Discussion..");
                return;
            }

            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();

            dtIncome = DtCMPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtCMPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation


            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtCMPDAppDt.Text) > gblFuction.setDate(txtCMPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkCMPDCoApp.Checked == true)
            {
                if (ddlCMPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion
            if (chkCMPDCoApp.Checked == true)
            {
                vCoAppID = ddlCMPDCoApplicant.SelectedValue;
            }
            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdCMPDCustId.Value, vCoAppID, "C", gblFuction.setDate(txtCMPDDate.Text), txtCMPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                LoadPDByCM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["CMPDExpenses"] = null;
                ViewState["CMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDCMUpdate_Click(object sender, EventArgs e)
        {
            if (hdCMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Update Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Update Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Update Personal Discussion..");
                return;
            }
            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtCMPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtCMPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation


            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtCMPDAppDt.Text) > gblFuction.setDate(txtCMPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkCMPDCoApp.Checked == true)
            {
                if (ddlCMPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion

            if (chkCMPDCoApp.Checked == true)
            {
                vCoAppID = ddlCMPDCoApplicant.SelectedValue;
            }
            if (hdCMPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdCMPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }


            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdCMPDCustId.Value, vCoAppID, "C", gblFuction.setDate(txtCMPDDate.Text), txtCMPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                LoadPDByCM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["CMPDExpenses"] = null;
                ViewState["CMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDCMDelete_Click(object sender, EventArgs e)
        {

            if (hdCMLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Delete Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Delete Personal Discussion..");
                return;
            }
            if (hdCMFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Delete Personal Discussion..");
                return;
            }


            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtCMPDIncome();
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtCMPDExpenses();
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            if (chkCMPDCoApp.Checked == true)
            {
                vCoAppID = ddlCMPDCoApplicant.SelectedValue;
            }
            if (hdCMPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdCMPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }


            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdCMPDCustId.Value, vCoAppID, "C", gblFuction.setDate(txtCMPDDate.Text), txtCMPDRemarks.Text
                                , 0, 0, 0, 0
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                LoadPDByCM(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["CMPDExpenses"] = null;
                ViewState["CMPDIncome"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }
        }
        protected void btnPDCMBack_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 2;
            ViewAcess();

            gvCMPDExpensesView.DataSource = null;
            gvCMPDExpensesView.DataBind();
            gvCMPDIncomeView.DataSource = null;
            gvCMPDIncomeView.DataBind();
        }

        private DataTable DtCMPDIncome()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;
            dt.Columns.Add("ProductID");
            dt.Columns.Add("SoldItem");
            dt.Columns.Add("SellingPrice");
            dt.Columns.Add("Days");
            dt.Columns.Add("DailyIncome");
            dt.Columns.Add("MonthlyIncome");
            dt.AcceptChanges();

            foreach (GridViewRow gr in gvCMPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtSold = (TextBox)gr.FindControl("txtSold");
                TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
                TextBox txtDays = (TextBox)gr.FindControl("txtDays");
                TextBox txtDIncome = (TextBox)gr.FindControl("txtDIncome");
                TextBox txtMIncome = (TextBox)gr.FindControl("txtMIncome");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["SoldItem"] = txtSold.Text == "" ? "0" : txtSold.Text;
                dr["SellingPrice"] = txtPrice.Text == "" ? "0" : txtPrice.Text;
                dr["Days"] = txtDays.Text == "" ? "0" : txtDays.Text;
                dr["DailyIncome"] = txtDIncome.Text == "" ? "0" : txtDIncome.Text;
                dr["MonthlyIncome"] = txtMIncome.Text == "" ? "0" : txtMIncome.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }


            return dt;
        }

        private DataTable DtCMPDExpenses()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;
            dt.Columns.Add("ProductID");
            dt.Columns.Add("UsedItem");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MonthlyExpenses");
            dt.AcceptChanges();

            foreach (GridViewRow gr in gvCMPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtUsed = (TextBox)gr.FindControl("txtUsed");
                TextBox txtExpenses = (TextBox)gr.FindControl("txtExpenses");
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["UsedItem"] = txtUsed.Text == "" ? "0" : txtUsed.Text;
                dr["Expenses"] = txtExpenses.Text == "" ? "0" : txtExpenses.Text;
                dr["MonthlyExpenses"] = txtMExpenses.Text == "" ? "0" : txtMExpenses.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }

            return dt;
        }

        #endregion

        #region PD BY Risk

        #region Income

        protected void btnRMPDIncome_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            DataRow dr;
            if (ViewState["RMPDIncome"] == null)
            {
                SetRMPDIncome();
            }
            dt = (DataTable)ViewState["RMPDIncome"];
            txtRMPDDIncome.Text = Convert.ToString((Request[txtRMPDDIncome.UniqueID] as string == null) ? txtRMPDDIncome.Text : Request[txtRMPDDIncome.UniqueID] as string);
            txtRMPDMIncome.Text = Convert.ToString((Request[txtRMPDMIncome.UniqueID] as string == null) ? txtRMPDMIncome.Text : Request[txtRMPDMIncome.UniqueID] as string);

            foreach (GridViewRow gr in gvRiskPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlRMPDIncome.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlRMPDIncome.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlRMPDIncome.SelectedItem.Text;
            dr["ID"] = ddlRMPDIncome.SelectedValue;
            dr["Sold"] = txtRMPDIncomeSold.Text == "" ? "0" : txtRMPDIncomeSold.Text;
            dr["Price"] = txtRMPDIncomePrice.Text == "" ? "0" : txtRMPDIncomePrice.Text;
            dr["NODays"] = txtRMPDIncomeDays.Text == "" ? "0" : txtRMPDIncomeDays.Text;
            dr["DIncome"] = txtRMPDDIncome.Text == "" ? "0" : txtRMPDDIncome.Text;
            dr["MIncome"] = txtRMPDMIncome.Text == "" ? "0" : txtRMPDMIncome.Text;
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["RMPDIncome"] = dt;

            gvRiskPDIncome.DataSource = dt;
            gvRiskPDIncome.DataBind();

            TotalRiskPDIncome();

            ddlRMPDIncome.SelectedIndex = -1;
            txtRMPDIncomeSold.Text = "";
            txtRMPDIncomePrice.Text = "";
            txtRMPDIncomeDays.Text = "";
            txtRMPDDIncome.Text = "";
            txtRMPDMIncome.Text = "";
            ddlRMPDIncome.Focus();

        }

        private void SetRMPDIncome()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Sold");
            dt.Columns.Add("Price");
            dt.Columns.Add("NODays");
            dt.Columns.Add("DIncome");
            dt.Columns.Add("MIncome");
            dt.AcceptChanges();
            ViewState["RMPDIncome"] = dt;
        }


        protected void btnDelRMPDIncome_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["RMPDIncome"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["RMPDIncome"] = dt;
                    gvRiskPDIncome.DataSource = dt;
                    gvRiskPDIncome.DataBind();

                    TotalRiskPDIncome();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        #region Expenses

        protected void btnRMPDExpenses_Click(object sender, EventArgs e)
        {

            DataTable dt = null;
            DataRow dr;
            if (ViewState["RMPDExpenses"] == null)
            {
                SetRMPDExpenses();
            }
            dt = (DataTable)ViewState["RMPDExpenses"];
            txtRMPDMExpenses.Text = Convert.ToString((Request[txtRMPDMExpenses.UniqueID] as string == null) ? txtRMPDMExpenses.Text : Request[txtRMPDMExpenses.UniqueID] as string);

            foreach (GridViewRow gr in gvRiskPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");

                if (ddlRMPDExpenses.SelectedValue == lblProductID.Text)
                {
                    gblFuction.MsgPopup("Duplicate Product Entry not allow..!!");
                    ddlCMPDExpenses.Focus();
                    return;
                }
            }

            dr = dt.NewRow();
            dr["SlNo"] = dt.Rows.Count + 1;
            dr["Product"] = ddlRMPDExpenses.SelectedItem.Text;
            dr["ID"] = ddlRMPDExpenses.SelectedValue;
            dr["Used"] = txtRMPDExpensesUsed.Text == "" ? "0" : txtRMPDExpensesUsed.Text;
            dr["Expenses"] = txtRMPDExpensesExp.Text == "" ? "0" : txtRMPDExpensesExp.Text;
            dr["MExpenses"] = txtRMPDMExpenses.Text == "" ? "0" : txtRMPDMExpenses.Text;

            dt.Rows.Add(dr);
            dt.AcceptChanges();
            ViewState["RMPDExpenses"] = dt;

            gvRiskPDExpenses.DataSource = dt;
            gvRiskPDExpenses.DataBind();

            TotalRiskPDExpenses();

            ddlRMPDExpenses.SelectedIndex = -1;
            txtRMPDExpensesUsed.Text = "";
            txtRMPDExpensesExp.Text = "";
            txtRMPDMExpenses.Text = "";

            ddlRMPDExpenses.Focus();

        }

        private void SetRMPDExpenses()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SlNo");
            dt.Columns.Add("Product");
            dt.Columns.Add("ID");
            dt.Columns.Add("Used");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MExpenses");
            dt.AcceptChanges();
            ViewState["RMPDExpenses"] = dt;
        }

        protected void btnDelRMPDExpenses_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["RMPDExpenses"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();

                    ViewState["RMPDExpenses"] = dt;

                    gvRiskPDExpenses.DataSource = dt;
                    gvRiskPDExpenses.DataBind();

                    TotalCMPDExpenses();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }

        #endregion

        private void LoadPDByRisk(string pLnAppId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.GetPDListForLoanApplication(pLnAppId, "R");
                if (dt.Rows.Count > 0)
                {
                    gvPDByRisk.DataSource = dt;
                    gvPDByRisk.DataBind();
                }
                else
                {
                    gvPDByRisk.DataSource = null;
                    gvPDByRisk.DataBind();
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
        protected void btnAddRiskPD_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("R");
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Personal Discussion..");
                    return;
                }
                else
                {
                    string vMsg = "";
                    vMsg = oMem.GetPDCondForRisk(ViewState["LoanAppId"].ToString());
                    if (vMsg != "")
                    {
                        gblFuction.MsgPopup(vMsg);
                        return;
                    }


                    ds = oMem.GetLnAppDetailsForPD(ViewState["LoanAppId"].ToString());
                    if (ds.Tables.Count > 0)
                    {
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        if (dt1.Rows.Count > 0)
                        {
                            txtRiskPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                            txtRiskPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                            hdRiskPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                            txtRiskPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                            txtRiskPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                            txtRiskPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                            txtRiskPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                            txtRiskPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                            hdRiskLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                            hdRiskFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                            txtRiskPDRemarks.Text = "";
                            hdRiskPDId.Value = "";
                            chkRiskPDCoApp.Checked = false;
                            ddlRiskPDCoApplicant.Enabled = false;
                        }
                        else
                        {
                            txtRiskPDLnApp.Text = "";
                            txtRiskPDCustName.Text = "";
                            hdRiskPDCustId.Value = "";
                            txtRiskPDAppDt.Text = "";
                            txtRiskPDAppAmt.Text = "";
                            txtRiskPDAppTenure.Text = "";
                            txtRiskPDAppType.Text = "";
                            txtRiskPDLnPurpose.Text = "";
                            txtRiskPDRemarks.Text = "";
                            hdRiskPDId.Value = "";
                            hdRiskLNSanction.Value = "";
                            hdRiskFinalPDStatus.Value = "";
                            chkRiskPDCoApp.Checked = false;
                            ddlRiskPDCoApplicant.Enabled = false;
                        }

                        if (dt2.Rows.Count > 0)
                        {
                            ddlRiskPDCoApplicant.Items.Clear();
                            ddlRiskPDCoApplicant.DataTextField = "CoAppName";
                            ddlRiskPDCoApplicant.DataValueField = "CoApplicantId";
                            ddlRiskPDCoApplicant.DataSource = dt2;
                            ddlRiskPDCoApplicant.DataBind();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlRiskPDCoApplicant.Items.Insert(0, oItm);
                        }
                        else
                        {
                            ddlRiskPDCoApplicant.Items.Clear();
                            ListItem oItm = new ListItem();
                            oItm.Text = "<--- Select --->";
                            oItm.Value = "-1";
                            ddlRiskPDCoApplicant.Items.Insert(0, oItm);
                        }


                        mView.ActiveViewIndex = 4;
                        ViewAcess();
                        txtRiskPDDate.Text = Session[gblValue.LoginDate].ToString();
                        btnPDRiskSave.Enabled = true;
                        btnPDRiskUpdate.Enabled = false;
                        btnPDRiskDelete.Enabled = false;
                        btnPDRiskBack.Enabled = true;


                    }
                    else
                    {
                        btnPDRiskSave.Enabled = false;
                        btnPDRiskUpdate.Enabled = false;
                        btnPDRiskDelete.Enabled = false;
                        btnPDRiskBack.Enabled = true;
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
            }
        }
        protected void chkRiskPDCoApp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRiskPDCoApp.Checked == true)
            {
                ddlRiskPDCoApplicant.Enabled = true;
                ddlRiskPDCoApplicant.SelectedIndex = -1;
            }
            else
            {
                ddlRiskPDCoApplicant.Enabled = false;
                ddlRiskPDCoApplicant.SelectedIndex = -1;

            }
        }
        protected void gvPDByRisk_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            string vReportID = "";
            vReportID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "RiskdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByRisk.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindRiskAssmentView(Convert.ToInt32(vReportID));

            }
            else if (e.CommandName == "RiskdEdit")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnEditInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPDByRisk.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnEditInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;

                BindRiskPDDeatils(Convert.ToInt32(vReportID));
                gvRiskPDIncomeView.DataSource = null;
                gvRiskPDIncomeView.DataBind();
                gvRiskPDExpensesView.DataSource = null;
                gvRiskPDExpensesView.DataBind();
            }
        }
        private void BindRiskPDDeatils(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                BindAssment("R");
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt1.Rows.Count > 0)
                    {
                        txtRiskPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                        txtRiskPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                        hdRiskPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                        txtRiskPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                        txtRiskPDAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                        txtRiskPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                        txtRiskPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                        txtRiskPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                        txtRiskPDDate.Text = dt1.Rows[0]["PDDate"].ToString();
                        txtRiskPDRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                        hdRiskPDId.Value = dt1.Rows[0]["PDId"].ToString();
                        hdRiskLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                        hdRiskFinalPDStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                        chkRiskPDCoApp.Checked = false;
                        ddlRiskPDCoApplicant.Enabled = false;
                    }
                    else
                    {
                        txtRiskPDLnApp.Text = "";
                        txtRiskPDCustName.Text = "";
                        hdRiskPDCustId.Value = "";
                        txtRiskPDAppDt.Text = "";
                        txtRiskPDAppAmt.Text = "";
                        txtRiskPDAppTenure.Text = "";
                        txtRiskPDAppType.Text = "";
                        txtRiskPDLnPurpose.Text = "";
                        txtRiskPDDate.Text = "";
                        txtRiskPDRemarks.Text = "";
                        hdRiskPDId.Value = "";
                        hdRiskLNSanction.Value = "";
                        hdRiskFinalPDStatus.Value = "";
                        chkRiskPDCoApp.Checked = false;
                        ddlRiskPDCoApplicant.Enabled = false;
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        ddlRiskPDCoApplicant.Items.Clear();
                        ddlRiskPDCoApplicant.DataTextField = "CoAppName";
                        ddlRiskPDCoApplicant.DataValueField = "CoApplicantId";
                        ddlRiskPDCoApplicant.DataSource = dt2;
                        ddlRiskPDCoApplicant.DataBind();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlRiskPDCoApplicant.Items.Insert(0, oItm);
                    }
                    else
                    {
                        ddlRiskPDCoApplicant.Items.Clear();
                        ListItem oItm = new ListItem();
                        oItm.Text = "<--- Select --->";
                        oItm.Value = "-1";
                        ddlRiskPDCoApplicant.Items.Insert(0, oItm);
                    }



                    if (dt3.Rows.Count > 0)
                    {
                        gvRiskPDIncome.DataSource = dt3;
                        gvRiskPDIncome.DataBind();

                        ViewState["RMPDIncome"] = dt3;
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvRiskPDExpenses.DataSource = dt4;
                        gvRiskPDExpenses.DataBind();

                        ViewState["RMPDExpenses"] = dt4;
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");



                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();

                        if (dt1.Rows[0]["CoAppID"].ToString() != "")
                        {
                            chkRiskPDCoApp.Checked = true;
                            ddlRiskPDCoApplicant.Enabled = true;
                            ddlRiskPDCoApplicant.SelectedIndex = ddlRiskPDCoApplicant.Items.IndexOf(ddlRiskPDCoApplicant.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CoAppID"])));

                        }
                    }

                    btnPDRiskSave.Enabled = false;
                    btnPDRiskUpdate.Enabled = true;
                    btnPDRiskDelete.Enabled = true;
                    btnPDRiskBack.Enabled = true;
                    mView.ActiveViewIndex = 4;
                    ViewAcess();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                oMem = null;
            }
        }
        private void BindRiskAssmentView(Int32 vPDId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            CMember oMem = new CMember();
            try
            {
                ds = oMem.GetPDDetailsByPDID(vPDId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    dt3 = ds.Tables[2];
                    dt4 = ds.Tables[3];

                    if (dt3.Rows.Count > 0)
                    {
                        gvRiskPDIncomeView.DataSource = dt3;
                        gvRiskPDIncomeView.DataBind();
                    }
                    if (dt4.Rows.Count > 0)
                    {
                        gvRiskPDExpensesView.DataSource = dt4;
                        gvRiskPDExpensesView.DataBind();
                    }

                    TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
                    TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
                    TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
                    TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

                    if (dt1.Rows.Count > 0)
                    {
                        txtSumTotSellPrice.Text = dt1.Rows[0]["TotalSellingIncome"].ToString();
                        txtSumTotalCost.Text = dt1.Rows[0]["TotalCostIncome"].ToString();
                        txtSumTotalMargin.Text = dt1.Rows[0]["TotalMarginIncome"].ToString();
                        txtTotalMExpenses.Text = dt1.Rows[0]["TotalExpensesMonthly"].ToString();
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
            }
        }
        protected void txtSoldRiskPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtPrice.Focus();
            TotalRiskPDIncome();
        }
        protected void txtPriceRiskPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDays.Focus();
            TotalRiskPDIncome();
        }
        protected void txtDaysRiskPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vSold = 0, vPrice = 0, vDays = 0, vDIncome = 0, vMIncome = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtSold = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtSold");
            TextBox txtPrice = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtPrice");
            TextBox txtDays = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDays");
            TextBox txtDIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtDIncome");
            TextBox txtMIncome = (TextBox)gvRiskPDIncome.Rows[curRow].FindControl("txtMIncome");
            if (!string.IsNullOrEmpty(txtSold.Text))
            {
                vSold = Convert.ToDecimal(txtSold.Text);
            }
            if (!string.IsNullOrEmpty(txtPrice.Text))
            {
                vPrice = Convert.ToDecimal(txtPrice.Text);
            }
            if (!string.IsNullOrEmpty(txtDays.Text))
            {
                vDays = Convert.ToDecimal(txtDays.Text);
            }
            vDIncome = vSold * vPrice;
            vMIncome = vDIncome * vDays;
            txtDIncome.Text = vDIncome.ToString("0.00");
            txtMIncome.Text = vMIncome.ToString("0.00");
            txtDIncome.Focus();
            TotalRiskPDIncome();
        }
        private void TotalRiskPDIncome()
        {
            decimal vDIncome = 0, vMIncome = 0;
            foreach (GridViewRow gr in gvRiskPDIncome.Rows)
            {
                TextBox txtDIncome = (TextBox)gr.FindControl("txtDIncome");
                TextBox txtMIncome = (TextBox)gr.FindControl("txtMIncome");
                if (!string.IsNullOrEmpty(txtDIncome.Text))
                {
                    vDIncome = vDIncome + Convert.ToDecimal(txtDIncome.Text);
                }
                if (!string.IsNullOrEmpty(txtMIncome.Text))
                {
                    vMIncome = vMIncome + Convert.ToDecimal(txtMIncome.Text);
                }
            }
            TextBox txtTotalDIncome = (TextBox)gvRiskPDIncome.FooterRow.FindControl("txtTotalDIncome");
            TextBox txtTotalMIncome = (TextBox)gvRiskPDIncome.FooterRow.FindControl("txtTotalMIncome");
            txtTotalDIncome.Text = vDIncome.ToString("0.00");
            txtTotalMIncome.Text = vMIncome.ToString("0.00");
        }
        protected void txtUsedRiskPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtExpenses.Focus();
            TotalRiskPDExpenses();
        }
        protected void txtExpensesRiskPD_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0;
            decimal vUsed = 0, vExpenses = 0, vMExpenses = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtUsed = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtUsed");
            TextBox txtExpenses = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtExpenses");
            TextBox txtMExpenses = (TextBox)gvRiskPDExpenses.Rows[curRow].FindControl("txtMExpenses");
            if (!string.IsNullOrEmpty(txtUsed.Text))
            {
                vUsed = Convert.ToDecimal(txtUsed.Text);
            }
            if (!string.IsNullOrEmpty(txtExpenses.Text))
            {
                vExpenses = Convert.ToDecimal(txtExpenses.Text);
            }
            vMExpenses = vUsed * vExpenses;
            txtMExpenses.Text = vMExpenses.ToString("0.00");
            txtMExpenses.Focus();
            TotalRiskPDExpenses();
        }
        private void TotalRiskPDExpenses()
        {
            decimal vMExpenses = 0;
            foreach (GridViewRow gr in gvRiskPDExpenses.Rows)
            {
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");
                if (!string.IsNullOrEmpty(txtMExpenses.Text))
                {
                    vMExpenses = vMExpenses + Convert.ToDecimal(txtMExpenses.Text);
                }
            }
            TextBox txtTotalMExpenses = (TextBox)gvRiskPDExpenses.FooterRow.FindControl("txtTotalMExpenses");
            txtTotalMExpenses.Text = vMExpenses.ToString("0.00");
        }

        protected void btnPDRiskSave_Click(object sender, EventArgs e)
        {
            if (hdRiskLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Add Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Add Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Add Personal Discussion..");
                return;
            }
            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();

            dtIncome = DtRiskPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtRiskPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }
            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation



            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtRiskPDAppDt.Text) > gblFuction.setDate(txtRiskPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkRiskPDCoApp.Checked == true)
            {
                if (ddlRiskPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion
            if (chkRiskPDCoApp.Checked == true)
            {
                vCoAppID = ddlRiskPDCoApplicant.SelectedValue;
            }
            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdRiskPDCustId.Value, vCoAppID, "R", gblFuction.setDate(txtRiskPDDate.Text), txtRiskPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 3;
                LoadPDByRisk(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["RMPDIncome"] = null;
                ViewState["RMPDExpenses"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDRiskUpdate_Click(object sender, EventArgs e)
        {
            if (hdRiskLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Update Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Update Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Update Personal Discussion..");
                return;
            }
            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtRiskPDIncome();
            if (dtIncome.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Income Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtRiskPDExpenses();
            if (dtExpenses.Rows.Count == 0)
            {
                gblFuction.MsgPopup("Please Add Expenses Deatils..");
                return;
            }
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }
            TextBox txtSumTotSellPrice = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotSellPrice");
            TextBox txtSumTotalCost = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalCost");
            TextBox txtSumTotalMargin = (TextBox)gvBMPDIncome.FooterRow.FindControl("txtSumTotalMargin");
            TextBox txtTotalMExpenses = (TextBox)gvBMPDExpenses.FooterRow.FindControl("txtTotalMExpenses");

            #region Validation

            if (string.IsNullOrEmpty(txtSumTotSellPrice.Text))
            {
                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalCost.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtSumTotalMargin.Text))
            {

                gblFuction.MsgPopup("Please Enter Income Assessment First..");
                return;
            }
            if (string.IsNullOrEmpty(txtTotalMExpenses.Text))
            {

                gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
                return;
            }

            //if (Convert.ToDecimal(txtTotalDIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMIncome.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Income Assessment First..");
            //    return;
            //}
            //if (Convert.ToDecimal(txtTotalMExpenses.Text) == 0)
            //{
            //    gblFuction.MsgPopup("Please Enter Expenses Assessment First..");
            //    return;
            //}

            if (gblFuction.setDate(txtRiskPDAppDt.Text) > gblFuction.setDate(txtRiskPDDate.Text))
            {
                gblFuction.MsgPopup("PD Date must be Greater Or Equal to Application date..");
                return;
            }
            if (chkRiskPDCoApp.Checked == true)
            {
                if (ddlRiskPDCoApplicant.SelectedIndex == 0)
                {
                    gblFuction.MsgPopup("Please Select  Co-Applicant..");
                    return;
                }
            }
            #endregion
            if (chkRiskPDCoApp.Checked == true)
            {
                vCoAppID = ddlRiskPDCoApplicant.SelectedValue;
            }
            if (hdRiskPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdRiskPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }


            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdRiskPDCustId.Value, vCoAppID, "R", gblFuction.setDate(txtRiskPDDate.Text), txtRiskPDRemarks.Text
                                , Convert.ToDecimal(txtSumTotSellPrice.Text), Convert.ToDecimal(txtSumTotalCost.Text), Convert.ToDecimal(txtSumTotalMargin.Text), Convert.ToDecimal(txtTotalMExpenses.Text)
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Edit");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 3;
                LoadPDByRisk(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["RMPDIncome"] = null;
                ViewState["RMPDExpenses"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }

        }
        protected void btnPDRiskDelete_Click(object sender, EventArgs e)
        {

            if (hdRiskLNSanction.Value == "Y")
            {
                gblFuction.MsgPopup("After Sanction not allow to Delete Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "R")
            {
                gblFuction.MsgPopup("After Final Reject PD not allow to Delete Personal Discussion..");
                return;
            }
            if (hdRiskFinalPDStatus.Value == "A")
            {
                gblFuction.MsgPopup("After Final Approve PD not allow to Delete Personal Discussion..");
                return;
            }
            DataTable dtIncome = null, dtExpenses = null;
            string vXmlIncome = "", vXmlExpenses = "", vCoAppID = "";
            Int32 vErr = 0, vPDId = 0;
            CMember oMem = new CMember();
            dtIncome = DtRiskPDIncome();
            using (StringWriter oSW = new StringWriter())
            {
                dtIncome.WriteXml(oSW);
                vXmlIncome = oSW.ToString();
            }

            dtExpenses = DtRiskPDExpenses();
            using (StringWriter oSW = new StringWriter())
            {
                dtExpenses.WriteXml(oSW);
                vXmlExpenses = oSW.ToString();
            }

            if (chkRiskPDCoApp.Checked == true)
            {
                vCoAppID = ddlRiskPDCoApplicant.SelectedValue;
            }
            if (hdRiskPDId.Value != "")
            {
                vPDId = Convert.ToInt32(hdRiskPDId.Value);
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;
            }


            vErr = oMem.SavePDMst(vPDId, ViewState["LoanAppId"].ToString(), hdRiskPDCustId.Value, vCoAppID, "R", gblFuction.setDate(txtRiskPDDate.Text), txtRiskPDRemarks.Text
                                , 0, 0, 0, 0
                                , vXmlIncome, vXmlExpenses, Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
            if (vErr > 0)
            {
                gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 3;
                LoadPDByRisk(ViewState["LoanAppId"].ToString());
                ViewAcess();
                ViewState["RMPDIncome"] = null;
                ViewState["RMPDExpenses"] = null;
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup(gblPRATAM.DBError);
                return;

            }
        }
        protected void btnPDRiskBack_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 3;
            ViewAcess();

            gvRiskPDExpensesView.DataSource = null;
            gvRiskPDExpensesView.DataBind();
            gvRiskPDIncomeView.DataSource = null;
            gvRiskPDIncomeView.DataBind();
            ViewState["RMPDIncome"] = null;
            ViewState["RMPDExpenses"] = null;
        }

        private DataTable DtRiskPDIncome()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;
            dt.Columns.Add("ProductID");
            dt.Columns.Add("SoldItem");
            dt.Columns.Add("SellingPrice");
            dt.Columns.Add("Days");
            dt.Columns.Add("DailyIncome");
            dt.Columns.Add("MonthlyIncome");
            dt.AcceptChanges();

            foreach (GridViewRow gr in gvRiskPDIncome.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtSold = (TextBox)gr.FindControl("txtSold");
                TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
                TextBox txtDays = (TextBox)gr.FindControl("txtDays");
                TextBox txtDIncome = (TextBox)gr.FindControl("txtDIncome");
                TextBox txtMIncome = (TextBox)gr.FindControl("txtMIncome");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["SoldItem"] = txtSold.Text == "" ? "0" : txtSold.Text;
                dr["SellingPrice"] = txtPrice.Text == "" ? "0" : txtPrice.Text;
                dr["Days"] = txtDays.Text == "" ? "0" : txtDays.Text;
                dr["DailyIncome"] = txtDIncome.Text == "" ? "0" : txtDIncome.Text;
                dr["MonthlyIncome"] = txtMIncome.Text == "" ? "0" : txtMIncome.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }


            return dt;
        }

        private DataTable DtRiskPDExpenses()
        {
            DataTable dt = new DataTable("Table1");
            DataRow dr;
            dt.Columns.Add("ProductID");
            dt.Columns.Add("UsedItem");
            dt.Columns.Add("Expenses");
            dt.Columns.Add("MonthlyExpenses");
            dt.AcceptChanges();

            foreach (GridViewRow gr in gvRiskPDExpenses.Rows)
            {
                Label lblProductID = (Label)gr.FindControl("lblProductID");
                TextBox txtUsed = (TextBox)gr.FindControl("txtUsed");
                TextBox txtExpenses = (TextBox)gr.FindControl("txtExpenses");
                TextBox txtMExpenses = (TextBox)gr.FindControl("txtMExpenses");

                dr = dt.NewRow();
                dr["ProductID"] = lblProductID.Text;
                dr["UsedItem"] = txtUsed.Text == "" ? "0" : txtUsed.Text;
                dr["Expenses"] = txtExpenses.Text == "" ? "0" : txtExpenses.Text;
                dr["MonthlyExpenses"] = txtMExpenses.Text == "" ? "0" : txtMExpenses.Text;
                dt.Rows.Add(dr);
                dt.AcceptChanges();
            }

            return dt;
        }

        #endregion

        #region PropertyDetails
        protected void btnAddPropRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["PropDtls"];

                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvProperty.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblPropSLNo = (Label)gvProperty.Rows[curRow].FindControl("lblPropSLNo");
                TextBox txtCollateralId = (TextBox)gvProperty.Rows[curRow].FindControl("txtCollateralId");
                DropDownList ddlSecurityType = (DropDownList)gvProperty.Rows[curRow].FindControl("ddlSecurityType");
                Label lblSecurityType = (Label)gvProperty.Rows[curRow].FindControl("lblSecurityType");
                TextBox txtAddress = (TextBox)gvProperty.Rows[curRow].FindControl("txtAddress");
                TextBox txtPropType = (TextBox)gvProperty.Rows[curRow].FindControl("txtPropType");
                TextBox txtProUsage = (TextBox)gvProperty.Rows[curRow].FindControl("txtProUsage");
                TextBox txtPropCost = (TextBox)gvProperty.Rows[curRow].FindControl("txtPropCost");
                TextBox txtTechValue1 = (TextBox)gvProperty.Rows[curRow].FindControl("txtTechValue1");
                TextBox txtTechValue2 = (TextBox)gvProperty.Rows[curRow].FindControl("txtTechValue2");
                TextBox txtTechValueCons = (TextBox)gvProperty.Rows[curRow].FindControl("txtTechValueCons");
                TextBox txtValuationDt = (TextBox)gvProperty.Rows[curRow].FindControl("txtValuationDt");
                TextBox txtPropertySize = (TextBox)gvProperty.Rows[curRow].FindControl("txtPropertySize");
                TextBox txtPropertyAge = (TextBox)gvProperty.Rows[curRow].FindControl("txtPropertyAge");
                TextBox txtOwnedBy = (TextBox)gvProperty.Rows[curRow].FindControl("txtOwnedBy");
                Label lblRelation = (Label)gvProperty.Rows[curRow].FindControl("lblRelation");
                DropDownList ddlRel = (DropDownList)gvProperty.Rows[curRow].FindControl("ddlRel");

                dt.Rows[curRow]["SlNo"] = lblPropSLNo.Text;
                if (ddlSecurityType.SelectedValue == "-1")
                {
                    lblSecurityType.Text = "-1";
                    gblFuction.AjxMsgPopup("Please Seclect Nature Of Property");
                    return;
                }
                else
                    lblSecurityType.Text = ddlSecurityType.SelectedValue.ToString();
                dt.Rows[curRow]["CollateralID"] = txtCollateralId.Text;
                dt.Rows[curRow]["PropertyNature"] = Convert.ToInt32(lblSecurityType.Text);
                dt.Rows[curRow]["Address"] = txtAddress.Text;
                dt.Rows[curRow]["PropertyType"] = txtPropType.Text;
                dt.Rows[curRow]["PropertyUsage"] = txtProUsage.Text;
                if (txtPropCost.Text != "")
                    dt.Rows[curRow]["PropertyCost"] = txtPropCost.Text;
                else
                    dt.Rows[curRow]["PropertyCost"] = 0;
                if (txtTechValue1.Text != "")
                    dt.Rows[curRow]["TechValue1"] = txtTechValue1.Text;
                else
                    dt.Rows[curRow]["TechValue1"] = 0;
                if (txtTechValue2.Text != "")
                    dt.Rows[curRow]["TechValue2"] = txtTechValue2.Text;
                else
                    dt.Rows[curRow]["TechValue2"] = 0;
                if (txtTechValueCons.Text != "")
                    dt.Rows[curRow]["TechValueCons"] = txtTechValueCons.Text;
                else
                    dt.Rows[curRow]["TechValueCons"] = 0;
                if (txtValuationDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("Valuation Date Can Not Be Empty");
                    return;
                }
                dt.Rows[curRow]["ValuationDt"] = txtValuationDt.Text;
                dt.Rows[curRow]["Area"] = txtPropertySize.Text;
                dt.Rows[curRow]["Age"] = txtPropertyAge.Text;
                dt.Rows[curRow]["OwnedBy"] = txtOwnedBy.Text;
                if (ddlRel.SelectedIndex == -1)
                {
                    lblRelation.Text = "-1";
                }
                else
                    lblRelation.Text = ddlRel.SelectedValue.ToString();
                dt.Rows[curRow]["RelationId"] = Convert.ToInt32(lblRelation.Text);
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(lblPropSLNo.Text) + 1;
                dt.Rows[vRow + 1]["ValuationDt"] = Session[gblValue.LoginDate].ToString();
                dt.AcceptChanges();

                ViewState["PropDtls"] = dt;
                gvProperty.DataSource = dt;
                gvProperty.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ImDel_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            dt = (DataTable)ViewState["PropDtls"];
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted ");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["PropDtls"] = dt;
                    gvProperty.DataSource = dt;
                    gvProperty.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void GetPropertyDtl()
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow(); ;
            dt.Columns.Add("SlNo", typeof(Int32));
            dt.Columns.Add("CollateralID", typeof(string));
            dt.Columns.Add("PropertyNature", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("PropertyType", typeof(string));
            dt.Columns.Add("PropertyUsage", typeof(string));
            dt.Columns.Add("PropertyCost", typeof(decimal));
            dt.Columns.Add("TechValue1", typeof(decimal));
            dt.Columns.Add("TechValue2", typeof(decimal));
            dt.Columns.Add("TechValueCons", typeof(decimal));
            dt.Columns.Add("ValuationDt", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Age", typeof(decimal));
            dt.Columns.Add("OwnedBy", typeof(string));
            dt.Columns.Add("RelationId", typeof(Int32));

            dr["SlNo"] = 1;
            dr["CollateralID"] = "";
            dr["PropertyNature"] = "-1";
            dr["Address"] = "";
            dr["PropertyType"] = "";
            dr["PropertyUsage"] = "";
            dr["PropertyCost"] = 0;
            dr["TechValue1"] = 0;
            dr["TechValue2"] = 0;
            dr["TechValueCons"] = 0;
            dr["ValuationDt"] = Session[gblValue.LoginDate].ToString();
            dr["Area"] = "";
            dr["Age"] = 0;
            dr["OwnedBy"] = "";
            dr["RelationId"] = 0;
            dt.Rows.Add(dr);

            ViewState["PropDtls"] = dt;

            gvProperty.DataSource = dt;
            gvProperty.DataBind();
        }
        private void GetPropertyDtlByLnAppId(string pLoanAppId)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetPropertyDtlByLoanAppId(pLoanAppId);
            txtPropDtlsAppId.Text = pLoanAppId;
            if (dt.Rows.Count > 0)
            {
                ViewState["PropDtls"] = dt;
                gvProperty.DataSource = dt;
                gvProperty.DataBind();
            }
            else
            {

                ViewState["PropDtls"] = null;
                GetPropertyDtl();
            }
        }
        protected void btnPropDtls_Click(object sender, EventArgs e)
        {
            //string xmlPropDtls = "";
            //DataTable dtPropDtls = new DataTable();
            //Int32 vErr = 0;
            //dtPropDtls = GetPropDtls();
            //dtPropDtls.TableName = "PropDtls";

            //using (StringWriter oSW1 = new StringWriter())
            //{
            //    dtPropDtls.WriteXml(oSW1);
            //    xmlPropDtls = oSW1.ToString().Trim();
            //}

            //CMember oMem = new CMember();
            //vErr = oMem.SavePropDtls(lblExtLnDtlsAppId.Text, xmlPropDtls);

            //if (vErr == 0)
            //{
            //    gblFuction.AjxMsgPopup("Property Details Saved Successfully......");
            //    return;
            //}
            //else
            //{
            //    gblFuction.MsgPopup(gblFORCE.DBError);
            //}
        }
        protected void GetPropDtls(string pLoanAppId)
        {
            //DataTable dt = new DataTable();
            //CMember oMem = new CMember();
            //lblPropDtlsAppId.Text = pLoanAppId;
            //dt = oMem.GetPropDtls(pLoanAppId);

            //if (dt.Rows.Count > 0)
            //{
            //    ViewState["PropDtls"] = dt;
            //    gvProperty.DataSource = dt;
            //    gvProperty.DataBind();
            //}
            //else
            //{
            //    ViewState["PropDtls"] = null;
            //    AddNewRowPropDtls();
            //}
        }
        protected void gvProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSecurityType = (Label)e.Row.FindControl("lblSecurityType");
                Label lblRelation = (Label)e.Row.FindControl("lblRelation");

                DropDownList ddlRel = (DropDownList)e.Row.FindControl("ddlRel");
                DropDownList ddlSecurityType = (DropDownList)e.Row.FindControl("ddlSecurityType");
                DataTable dtDesig1 = null, dtDesig2 = null, dtValue = new DataTable();
                DataTable dt = (DataTable)ViewState["gvProperty"];
                string vBrCode = Convert.ToString(Session[gblValue.BrnchCode]);
                CGblIdGenerator oAg1 = null, oAg2 = null;

                oAg1 = new CGblIdGenerator();
                dtDesig1 = oAg1.PopComboMIS("N", "N", "AA", "RelationId", "Relation", "RelationMst", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlRel.DataSource = dtDesig1;
                ddlRel.DataTextField = "Relation";
                ddlRel.DataValueField = "RelationId";
                ddlRel.DataBind();
                ListItem oli1 = new ListItem("<-Select->", "-1");
                ddlRel.Items.Insert(0, oli1);
                if (lblRelation.Text != "")
                    ddlRel.SelectedIndex = ddlRel.Items.IndexOf(ddlRel.Items.FindByValue(lblRelation.Text.Trim()));

                oAg2 = new CGblIdGenerator();
                dtDesig2 = oAg2.PopComboMIS("N", "N", "AA", "PropertyTypeID", "PropertypeName", "PropertyTypeMst1", 0, "AA", "AA", gblFuction.setDate("01/01/1900"), "0000");
                ddlSecurityType.DataSource = dtDesig2;
                ddlSecurityType.DataTextField = "PropertypeName";
                ddlSecurityType.DataValueField = "PropertyTypeID";
                ddlSecurityType.DataBind();
                ddlSecurityType.Items.Insert(0, oli1);
                if (lblSecurityType.Text != "")
                    ddlSecurityType.SelectedIndex = ddlSecurityType.Items.IndexOf(ddlSecurityType.Items.FindByValue(lblSecurityType.Text.Trim()));
                //*****************End********************
            }
        }
        private void SavePropertyDetails(string pMode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");

            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string pLnAppId = txtPropDtlsAppId.Text.ToString();
            if (pLnAppId == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            DataTable dt = new DataTable("PropertyList");
            dt.Columns.Add("SlNo", typeof(Int32));
            dt.Columns.Add("CollateralID", typeof(string));
            dt.Columns.Add("PropertyNature", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("PropertyType", typeof(string));
            dt.Columns.Add("PropertyUsage", typeof(string));
            dt.Columns.Add("PropertyCost", typeof(decimal));
            dt.Columns.Add("TechValue1", typeof(decimal));
            dt.Columns.Add("TechValue2", typeof(decimal));
            dt.Columns.Add("TechValueCons", typeof(decimal));
            dt.Columns.Add("ValuationDt", typeof(string));
            dt.Columns.Add("Area", typeof(string));
            dt.Columns.Add("Age", typeof(decimal));
            dt.Columns.Add("OwnedBy", typeof(string));
            dt.Columns.Add("RelationId", typeof(Int32));
            DataRow dr;
            foreach (GridViewRow gr in gvProperty.Rows)
            {

                Label lblPropSLNo = (Label)gr.FindControl("lblPropSLNo");
                TextBox txtCollateralId = (TextBox)gr.FindControl("txtCollateralId");
                DropDownList ddlSecurityType = (DropDownList)gr.FindControl("ddlSecurityType");
                Label lblSecurityType = (Label)gr.FindControl("lblSecurityType");
                TextBox txtAddress = (TextBox)gr.FindControl("txtAddress");
                TextBox txtPropType = (TextBox)gr.FindControl("txtPropType");
                TextBox txtProUsage = (TextBox)gr.FindControl("txtProUsage");
                TextBox txtPropCost = (TextBox)gr.FindControl("txtPropCost");
                TextBox txtTechValue1 = (TextBox)gr.FindControl("txtTechValue1");
                TextBox txtTechValue2 = (TextBox)gr.FindControl("txtTechValue2");
                TextBox txtTechValueCons = (TextBox)gr.FindControl("txtTechValueCons");
                TextBox txtValuationDt = (TextBox)gr.FindControl("txtValuationDt");
                TextBox txtPropertySize = (TextBox)gr.FindControl("txtPropertySize");
                TextBox txtPropertyAge = (TextBox)gr.FindControl("txtPropertyAge");
                TextBox txtOwnedBy = (TextBox)gr.FindControl("txtOwnedBy");
                Label lblRelation = (Label)gr.FindControl("lblRelation");
                DropDownList ddlRel = (DropDownList)gr.FindControl("ddlRel");

                if (txtValuationDt.Text != "")
                {
                    dr = dt.NewRow();
                    dr["SlNo"] = lblPropSLNo.Text;
                    if (ddlSecurityType.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Plese Select Nature Of Property,As it can not be blank..");
                        lblSecurityType.Text = "-1";
                        return;
                    }
                    else
                        lblSecurityType.Text = ddlSecurityType.SelectedValue.ToString();
                    dr["CollateralID"] = txtCollateralId.Text;
                    dr["PropertyNature"] = Convert.ToInt32(lblSecurityType.Text);
                    dr["Address"] = txtAddress.Text;
                    dr["PropertyType"] = txtPropType.Text;
                    dr["PropertyUsage"] = txtProUsage.Text;

                    if (txtPropCost.Text != "")
                        dr["PropertyCost"] = txtPropCost.Text;
                    else
                        dr["PropertyCost"] = 0;
                    if (txtTechValue1.Text != "")
                        dr["TechValue1"] = txtTechValue1.Text;
                    else
                        dr["TechValue1"] = 0;
                    if (txtTechValue2.Text != "")
                        dr["TechValue2"] = txtTechValue2.Text;
                    else
                        dr["TechValue2"] = 0;
                    if (txtTechValueCons.Text != "")
                        dr["TechValueCons"] = txtTechValueCons.Text;
                    else
                        dr["TechValueCons"] = 0;

                    if (txtValuationDt.Text != "")
                        dr["ValuationDt"] = gblFuction.setDate(txtValuationDt.Text);
                    else
                        dr["ValuationDt"] = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                    dr["Area"] = txtPropertySize.Text;
                    if (txtPropertyAge.Text != "")
                        dr["Age"] = txtPropertyAge.Text;
                    else
                        dr["Age"] = 0;
                    dr["OwnedBy"] = txtOwnedBy.Text;
                    if (ddlRel.SelectedIndex == -1)
                    {
                        lblRelation.Text = "-1";
                    }
                    else
                        lblRelation.Text = ddlRel.SelectedValue.ToString();
                    dr["RelationId"] = Convert.ToInt32(lblRelation.Text);
                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
            }


            CApplication oFS = null;
            DataTable dtXml = dt;
            int vErr = 0;
            string vXml = "";
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
                }
                oFS = new CApplication();
                if (pMode == "Save")
                {
                    vErr = oFS.SavePropertyDtlBulk(pLnAppId, vXml, "W", Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Save Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Edit")
                {
                    vErr = oFS.SavePropertyDtlBulk(pLnAppId, vXml, "W", Convert.ToInt32(Session[gblValue.UserId].ToString()), "Save");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Updated Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
                        return;
                    }
                }
                if (pMode == "Delete")
                {
                    vErr = oFS.SavePropertyDtlBulk(pLnAppId, vXml, "W", Convert.ToInt32(Session[gblValue.UserId].ToString()), "Delete");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Record Deleted Successfully");
                        return;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Error: Data not Saved");
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
                dtXml = null;
                oFS = null;
            }
        }
        protected void btnPropDtlsSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Save";
            SavePropertyDetails(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        protected void btnPropDtlsDelete_Click(object sender, EventArgs e)
        {
            string vStateEdit = "Delete";
            SavePropertyDetails(vStateEdit);
            ShowLoanRelationDetails(ViewState["LoanAppId"].ToString());
        }
        #endregion

        #region IIR
        private void ClearIIR()
        {
            txtIIRTenure.Text = "0";
            txtIIRGrossInc.Text = "0";
            txtIIRGrossExp.Text = "0";
            txtIIRNetInc.Text = "0";
            txtIIRObligation.Text = "0";
            txtIIRAmount.Text = "0";
            txtIIRLnligibility.Text = "0";
            txtIIRPer.Text = "0";
        }
        private void GetRecForIIR(string pLoanAppId)
        {
            ClearIIR();
            int tenure = 0;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            CMember OMem = new CMember();
            ds = OMem.GetExistIIRRecByLnAppId(pLoanAppId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtIIRLnAppNo.Text = pLoanAppId.ToString();
                txtIIRTenure.Text = ds.Tables[0].Rows[0]["Tenure"].ToString();
                txtIIRGrossInc.Text = ds.Tables[0].Rows[0]["GrossInc"].ToString();
                txtIIRGrossExp.Text = ds.Tables[0].Rows[0]["GrossExp"].ToString();
                txtIIRNetInc.Text = ds.Tables[0].Rows[0]["NetInc"].ToString();
                txtIIRAmount.Text = ds.Tables[0].Rows[0]["IIR"].ToString();
                txtIIRPer.Text = ds.Tables[0].Rows[0]["IIRPer"].ToString();
                txtIIRObligation.Text = ds.Tables[0].Rows[0]["Obligation"].ToString();
                if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Tenure"].ToString()) == false)
                    tenure = Convert.ToInt32(ds.Tables[0].Rows[0]["Tenure"]);
                txtIIRLnligibility.Text = ds.Tables[0].Rows[0]["LoanEligibility"].ToString();
            }
            else
            {
                ds1 = OMem.GetIIRRecByLnAppId(pLoanAppId);
                txtIIRLnAppNo.Text = pLoanAppId;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    txtIIRTenure.Text = ds1.Tables[0].Rows[0]["Tenure"].ToString();
                    txtIIRGrossInc.Text = ds1.Tables[0].Rows[0]["TotInc"].ToString();
                    txtIIRGrossExp.Text = ds1.Tables[0].Rows[0]["TotExp"].ToString();
                    txtIIRNetInc.Text = ds1.Tables[0].Rows[0]["NetInc"].ToString();
                    txtIIRAmount.Text = ds1.Tables[0].Rows[0]["IIR"].ToString();
                    txtIIRPer.Text = ds1.Tables[0].Rows[0]["IIRPer"].ToString();
                    txtIIRObligation.Text = ds1.Tables[0].Rows[0]["TotalEMIAmt"].ToString();
                    if (string.IsNullOrEmpty(ds1.Tables[0].Rows[0]["Tenure"].ToString()) == false)
                        tenure = Convert.ToInt32(ds1.Tables[0].Rows[0]["Tenure"]);
                    txtIIRLnligibility.Text = ds1.Tables[0].Rows[0]["LoanEligibility"].ToString();

                }
            }
        }
        protected void txtIIRPer_TextChanged(object sender, EventArgs e)
        {
            int vTenure = 0;
            decimal vNetIncome = 0, vIIRPer = 0, vIIRAmount = 0, vLnAmtEligibility = 0;
            if (txtIIRTenure.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Tenure Can Not Be Empty");
                return;
            }
            if (txtIIRPer.Text == "")
            {
                gblFuction.AjxMsgPopup("IIR Percentage Can Not Be Empty");
                return;
            }
            if (txtIIRNetInc.Text == "")
            {
                gblFuction.AjxMsgPopup("Net Income Can Not Be Empty");
                return;
            }
            vTenure = Convert.ToInt32(txtIIRTenure.Text);
            vIIRPer = Convert.ToDecimal(txtIIRPer.Text);
            vNetIncome = Convert.ToDecimal(txtIIRNetInc.Text);
            vIIRAmount = Math.Round((vNetIncome * vIIRPer) / 100, 0);
            txtIIRAmount.Text = vIIRAmount.ToString();
            txtIIRLnligibility.Text = "0";
            if (vTenure == 36)
            {
                vLnAmtEligibility = Convert.ToDecimal(Math.Round(((vIIRAmount / 4083) * 100000), 4));
                txtIIRLnligibility.Text = vLnAmtEligibility.ToString();
            }
            else if (vTenure == 48)
            {
                vLnAmtEligibility = Convert.ToDecimal(Math.Round((vIIRAmount / 3428 * 100000), 4));
                txtIIRLnligibility.Text = vLnAmtEligibility.ToString();
            }
            else if (vTenure == 12)
            {
                vLnAmtEligibility = Convert.ToDecimal(Math.Round((vIIRAmount / 9602 * 100000), 4));
                txtIIRLnligibility.Text = vLnAmtEligibility.ToString();
            }
            else if (vTenure == 18)
            {
                vLnAmtEligibility = Convert.ToDecimal(Math.Round((vIIRAmount / 6818 * 100000), 4));
                txtIIRLnligibility.Text = vLnAmtEligibility.ToString();
            }
            else if (vTenure == 24)
            {
                vLnAmtEligibility = Convert.ToDecimal(Math.Round((vIIRAmount / 5438 * 100000), 4));
                txtIIRLnligibility.Text = vLnAmtEligibility.ToString();
            }
            else
            {
                gblFuction.AjxMsgPopup("Tenure must be either 36,48,12,18,24.As Loan Eligible amount are calculating on this tenure only...");
                return;
            }

        }
        protected void btnSaveIIR_Click(object sender, EventArgs e)
        {
            string pLnAppId = "";
            int vTenure = 0;
            decimal vGrossInc = 0, vExpense = 0, vNetIncome = 0, vObligation = 0, vIIRPer = 0, vIIRAmt = 0, vLnEligibility = 0;
            if (txtIIRLnAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank,Please click on Show Informatiom");
                return;
            }
            else
                pLnAppId = txtIIRLnAppNo.Text.ToString().Trim();
            if (txtIIRTenure.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Tenure Can Not Be Empty");
                return;
            }
            if (txtIIRPer.Text == "")
            {
                gblFuction.AjxMsgPopup("IIR Percentage Can Not Be Empty");
                return;
            }
            if (txtIIRNetInc.Text == "")
            {
                gblFuction.AjxMsgPopup("Net Income Can Not Be Empty");
                return;
            }
            vTenure = Convert.ToInt32(txtIIRTenure.Text);
            vIIRPer = Convert.ToDecimal(txtIIRPer.Text);
            vGrossInc = Convert.ToDecimal(txtIIRGrossInc.Text);
            vExpense = Convert.ToDecimal(txtIIRGrossExp.Text);
            vNetIncome = Convert.ToDecimal(txtIIRNetInc.Text);
            vObligation = Convert.ToDecimal(txtIIRObligation.Text);
            vIIRAmt = Convert.ToDecimal(txtIIRAmount.Text);
            if (txtIIRLnligibility.Text != "")
                vLnEligibility = Convert.ToDecimal(txtIIRLnligibility.Text);

            Int32 vErr = 0;
            CMember oMem = new CMember();
            try
            {
                vErr = oMem.SaveIIRRatio(pLnAppId, vGrossInc, vExpense, vNetIncome, vObligation, vIIRAmt, vLnEligibility, "IIR", vIIRPer, vTenure, 0, Convert.ToInt32(Session[gblValue.UserId]));
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        protected void btnDeleteIIR_Click(object sender, EventArgs e)
        {
            string pLnAppId = "";
            if (txtIIRLnAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank,Please click on Show Informatiom");
                return;
            }
            else
                pLnAppId = txtIIRLnAppNo.Text.ToString().Trim();
            string vErrDesc = "";
            Int32 vErr = 0;
            CMember oMem = new CMember();
            try
            {
                vErr = oMem.DeleteIIRRatio(pLnAppId, "IIR", ref vErrDesc, 0);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    ClearIIR();
                }
                else
                {
                    if (vErrDesc == "")
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrDesc);
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
            }
        }
        #endregion

        #region FOIR
        private void GetRecForObligation(string pLoanAppId)
        {
            ClearFOIR();
            int tenure = 0;
            txtFOIRLnApp.Text = pLoanAppId;
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            CMember OMem = new CMember();
            // First Check Existing Record From Obligation
            ds = OMem.GetExistObligationByLnAppId(pLoanAppId);
            ds1 = OMem.GetRecForObligation(pLoanAppId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvObligationNew.DataSource = ds.Tables[0];
                gvObligationNew.DataBind();
                btnSaveObligation.Visible = true;
                ViewState["Obligation"] = ds.Tables[0];
            }
            else
            {
                // Get Record From HighmarkDataDetail (For New Record)

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    gvObligationNew.DataSource = ds1.Tables[0];
                    gvObligationNew.DataBind();
                    btnSaveObligation.Visible = true;
                    ViewState["Obligation"] = ds1.Tables[0];
                }
                else
                {
                    GetObligationList();
                }
            }
            // For Existing Rcord
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    txtFOIRTenure.Text = ds.Tables[1].Rows[0]["Tenure"].ToString();
                    txtFOIRGrossInc.Text = ds.Tables[1].Rows[0]["TotInc"].ToString();
                    txtFOIRGrossExp.Text = ds.Tables[1].Rows[0]["TotExp"].ToString();
                    txtFOIRNetInc.Text = ds.Tables[1].Rows[0]["NetInc"].ToString();
                    txtFOIRAmount.Text = ds.Tables[1].Rows[0]["IIR"].ToString();
                    txtFOIRObligation.Text = ds.Tables[1].Rows[0]["TotalEMIAmt"].ToString();
                    txtFOIRPer.Text = ds.Tables[1].Rows[0]["IIRPer"].ToString();
                    if (string.IsNullOrEmpty(ds.Tables[1].Rows[0]["Tenure"].ToString()) == false)
                        tenure = Convert.ToInt32(ds.Tables[1].Rows[0]["Tenure"]);
                    txtFOIRLnEligibility.Text = ds.Tables[1].Rows[0]["LoanAmtElg"].ToString();
                }
                else // For New Record
                {
                    if (ds1.Tables.Count > 1)
                    {
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            txtFOIRTenure.Text = ds1.Tables[1].Rows[0]["Tenure"].ToString();
                            txtFOIRGrossInc.Text = ds1.Tables[1].Rows[0]["TotInc"].ToString();
                            txtFOIRGrossExp.Text = ds1.Tables[1].Rows[0]["TotExp"].ToString();
                            txtFOIRNetInc.Text = ds1.Tables[1].Rows[0]["NetInc"].ToString();
                            txtFOIRAmount.Text = ds1.Tables[1].Rows[0]["IIR"].ToString();
                            txtFOIRObligation.Text = ds1.Tables[1].Rows[0]["TotalEMIAmt"].ToString();
                            txtFOIRPer.Text = ds1.Tables[1].Rows[0]["IIRPer"].ToString();
                            if (string.IsNullOrEmpty(ds1.Tables[1].Rows[0]["Tenure"].ToString()) == false)
                                tenure = Convert.ToInt32(ds1.Tables[1].Rows[0]["Tenure"]);
                            txtFOIRLnEligibility.Text = ds1.Tables[1].Rows[0]["LoanAmtElg"].ToString();
                        }
                    }
                }
            }

        }
        protected void txtFOIRPer_TextChanged(object sender, EventArgs e)
        {
            CalculateObligationData();
        }

        private void ClearFOIR()
        {
            gvObligationNew.DataSource = null;
            gvObligationNew.DataBind();
            txtFOIRTenure.Text = "0";
            txtFOIRGrossInc.Text = "0";
            txtFOIRGrossExp.Text = "0";
            txtFOIRNetInc.Text = "0";

            txtFOIRObligation.Text = "0";
            txtFOIRAmount.Text = "0";
            txtFOIRLnEligibility.Text = "0";
        }
        decimal EMIAmt = 0, TotalEMIAmt = 0, Balance = 0, TotBalance = 0;
        protected void gvObligationNew_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtObjEMIAmount = (TextBox)e.Row.FindControl("txtObjEMIAmount");
                TextBox txtCurrentBalance = (TextBox)e.Row.FindControl("txtCurrentBalance");
                Label lblOblChk = (Label)e.Row.FindControl("lblOblChk");
                CheckBox chkObl = (CheckBox)e.Row.FindControl("chkObl");
                if (lblOblChk.Text == "N")
                {
                    chkObl.Checked = false;
                }
                else
                {
                    chkObl.Checked = true;
                    if (txtObjEMIAmount.Text != "")
                        EMIAmt = Convert.ToDecimal(txtObjEMIAmount.Text);
                    if (txtCurrentBalance.Text != "")
                        Balance = Convert.ToDecimal(txtCurrentBalance.Text);
                }
                TotalEMIAmt = TotalEMIAmt + EMIAmt;
                TotBalance = TotBalance + Balance;
                EMIAmt = 0; Balance = 0;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblFOIRTotalEMIAmount = (Label)e.Row.FindControl("lblFOIRTotalEMIAmount");
                lblFOIRTotalEMIAmount.Text = TotalEMIAmt.ToString();

                Label lblFOIRTotalBalPric = (Label)e.Row.FindControl("lblFOIRTotalBalPric");
                lblFOIRTotalBalPric.Text = TotBalance.ToString();
            }           
        }
        private void CalculateObligationData()
        {
            decimal TotEMI = 0, GrossInc = 0, GrossExp = 0, NetInc = 0, Obligation = 0, IIR = 0, LoanEligibility = 0, IIRPer = 0;
            string TotalEMI = "";
            Int32 tenure = 0;
            if (gvObligationNew.Rows.Count > 0)
            {
                Label lblFOIRTotalEMIAmount = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalEMIAmount") as Label);
                TotalEMI = lblFOIRTotalEMIAmount.Text;
            }
            if (txtFOIRPer.Text == "")
            {
                gblFuction.AjxMsgPopup("FOIR Percentage Can Not Be Empty..");
                return;
            }


            IIRPer = Convert.ToDecimal(txtFOIRPer.Text);
            if (TotalEMI != "")
                TotEMI = Convert.ToDecimal(TotalEMI);
            txtFOIRObligation.Text = TotEMI.ToString();
            txtFOIRLnEligibility.Text = (Convert.ToDouble(txtFOIRAmount.Text) - Convert.ToDouble(txtFOIRObligation.Text)).ToString();
            //if (txtFOIRTenure.Text != "")
            //    tenure = Convert.ToInt32(txtFOIRTenure.Text);
            //if (txtFOIRGrossInc.Text != "")
            //    GrossInc = Convert.ToDecimal(txtFOIRGrossInc.Text);
            //if (txtFOIRGrossExp.Text != "")
            //    GrossExp = Convert.ToDecimal(txtFOIRGrossExp.Text);
            //if (txtFOIRNetInc.Text != "")
            //    NetInc = Convert.ToDecimal(txtFOIRNetInc.Text);
            //if (txtFOIRObligation.Text != "")
            //    Obligation = Convert.ToDecimal(txtFOIRObligation.Text);

            ////if (tenure == 36)
            ////    LoanEligibility = Convert.ToDecimal(txtFOIRLnElig36.Text);
            ////else if (tenure == 48)
            ////    LoanEligibility = Convert.ToDecimal(txtFOIRLnElig48.Text);
            ////else
            ////    LoanEligibility = 0;
            //if ((NetInc) > 0)
            //    //txtFOIRAmount.Text = Math.Round(((NetInc - TotEMI) * 40 / 100), 2).ToString();
            //    txtFOIRAmount.Text = Math.Round(((NetInc) * IIRPer / 100), 2).ToString();
            //else
            //    txtFOIRAmount.Text = "0";

            //if (txtFOIRAmount.Text != "")
            //    IIR = Convert.ToDecimal(Math.Round(((NetInc) * IIRPer / 100), 2));
            //txtFOIRLnEligibility.Text = "0";
            //if (tenure == 36)
            //{
            //    LoanEligibility = Math.Round((((IIR - Obligation) / 4083) * 100000), 2);
            //    txtFOIRLnEligibility.Text = LoanEligibility.ToString();
            //}
            //else if (tenure == 48)
            //{
            //    LoanEligibility = Math.Round((((IIR - Obligation) / 3428) * 100000), 2);
            //    txtFOIRLnEligibility.Text = LoanEligibility.ToString();
            //}
            //else if (tenure == 12)
            //{
            //    LoanEligibility = Math.Round((((IIR - Obligation) / 9602) * 100000), 2);
            //    txtFOIRLnEligibility.Text = LoanEligibility.ToString();
            //}
            //else if (tenure == 18)
            //{
            //    LoanEligibility = Math.Round((((IIR - Obligation) / 6818) * 100000), 2);
            //    txtFOIRLnEligibility.Text = LoanEligibility.ToString();
            //}
            //else if (tenure == 24)
            //{
            //    LoanEligibility = Math.Round((((IIR - Obligation) / 5438) * 100000), 2);
            //    txtFOIRLnEligibility.Text = LoanEligibility.ToString();
            //}
            //else
            //    txtFOIRLnEligibility.Text = "0";

        }
        protected void btnSaveObligation_Click(object sender, EventArgs e)
        {
            string pLnAppId = "", TotalEMI = "";
            if (txtFOIRLnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank,Please click on Show Informatiom");
                return;
            }
            else
                pLnAppId = txtFOIRLnApp.Text.ToString().Trim();
            //if (gvObligationNew.Rows.Count <= 0)
            //{
            //    gblFuction.AjxMsgPopup("No Record Found For Save");
            //    return;
            //}
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("CompanyName", typeof(string));
            dtXml.Columns.Add("AccType", typeof(string));
            dtXml.Columns.Add("EMIFromHighMark", typeof(decimal));
            dtXml.Columns.Add("Term", typeof(int));
            dtXml.Columns.Add("EMIAmount", typeof(decimal));
            dtXml.Columns.Add("BalancePrinc", typeof(decimal));
            dtXml.Columns.Add("Frequency", typeof(string));
            foreach (GridViewRow gr in gvObligationNew.Rows)
            {
                dr = dtXml.NewRow();
                dr["CompanyName"] = ((TextBox)gr.FindControl("txtCompanyName")).Text;
                dr["AccType"] = ((TextBox)gr.FindControl("txtAccType")).Text;
                if (((TextBox)gr.FindControl("txtObjEMI")).Text != "")
                    dr["EMIFromHighMark"] = Convert.ToDecimal(((TextBox)gr.FindControl("txtObjEMI")).Text);
                else
                    dr["EMIFromHighMark"] = 0;
                if (((TextBox)gr.FindControl("txtTerms")).Text != "")
                    dr["Term"] = Convert.ToInt32(((TextBox)gr.FindControl("txtTerms")).Text);
                else
                    dr["Term"] = 0;
                if (((TextBox)gr.FindControl("txtObjEMIAmount")).Text != "")
                    dr["EMIAmount"] = Convert.ToDecimal(((TextBox)gr.FindControl("txtObjEMIAmount")).Text);
                else
                    dr["EMIAmount"] = 0;
                if (((TextBox)gr.FindControl("txtCurrentBalance")).Text != "")
                    dr["BalancePrinc"] = Convert.ToDecimal(((TextBox)gr.FindControl("txtCurrentBalance")).Text);
                else
                    dr["BalancePrinc"] = 0;
                dr["Frequency"] = ((TextBox)gr.FindControl("txtObjFreq")).Text;
                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";
            string vXml = "";
            CMember oMem = new CMember();
            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString();
                }
                if (gvObligationNew.Rows.Count > 0)
                {
                    TotalEMI = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalEMIAmount") as Label).Text;
                }
                Int32 tenure = 0;
                decimal TotEMI = 0, GrossInc = 0, GrossExp = 0, NetInc = 0, Obligation = 0, IIR = 0, LoanEligibility = 0, IIRPer = 0;
                if (TotalEMI != "")
                    TotEMI = Convert.ToDecimal(TotalEMI);

                if (txtFOIRPer.Text == "")
                {
                    gblFuction.AjxMsgPopup("FOIR Percentage Can Not Be Empty...");
                    return;
                }
                if (txtFOIRTenure.Text == "")
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be Empty...");
                    return;
                }
                IIRPer = Convert.ToDecimal(txtFOIRPer.Text);

                if (txtFOIRTenure.Text != "")
                    tenure = Convert.ToInt32(txtFOIRTenure.Text);
                if (txtFOIRGrossInc.Text != "")
                    GrossInc = Convert.ToDecimal(txtFOIRGrossInc.Text);
                if (txtFOIRGrossExp.Text != "")
                    GrossExp = Convert.ToDecimal(txtFOIRGrossExp.Text);
                if (txtFOIRNetInc.Text != "")
                    NetInc = Convert.ToDecimal(txtFOIRNetInc.Text);
                if (txtFOIRObligation.Text != "")
                    Obligation = Convert.ToDecimal(txtFOIRObligation.Text);
                if (txtFOIRAmount.Text != "")
                    IIR = Convert.ToDecimal(txtFOIRAmount.Text);
                if (txtFOIRLnEligibility.Text != "")
                    LoanEligibility = Convert.ToDecimal(txtFOIRLnEligibility.Text);

                Int32 vErr = 0;
                vErr = oMem.SaveObligationBulk(pLnAppId, TotEMI, vXml, Convert.ToInt32(Session[gblValue.UserId]), "Save",
                    GrossInc, GrossExp, NetInc, Obligation, IIR, LoanEligibility, 0, IIRPer, tenure);
                if (vErr > 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                }
                else
                {
                    gblFuction.MsgPopup(gblPRATAM.DBError);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
            }
        }
        protected void BubtnDeleteObligationtton_Click(object sender, EventArgs e)
        {
            string pLnAppId = "";
            if (txtIIRLnAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank,Please click on Show Informatiom");
                return;
            }
            else
                pLnAppId = txtIIRLnAppNo.Text.ToString().Trim();
            string vErrDesc = "";
            Int32 vErr = 0;
            CMember oMem = new CMember();
            try
            {
                vErr = oMem.DeleteIIRRatio(pLnAppId, "FOIR", ref vErrDesc, 0);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    ClearFOIR();
                }
                else
                {
                    if (vErrDesc == "")
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrDesc);
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
            }
        }
        protected void btnAddDocRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["Obligation"];

                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvObligationNew.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblPropSLNo = (Label)gvObligationNew.Rows[curRow].FindControl("lblPropSLNo");
                TextBox txtCompanyName = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtCompanyName");
                TextBox txtAccType = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtAccType");
                TextBox txtObjEMI = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtObjEMI");
                TextBox txtTerms = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtTerms");
                TextBox txtObjEMIAmount = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtObjEMIAmount");
                TextBox txtCurrentBalance = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtCurrentBalance");
                TextBox txtObjFreq = (TextBox)gvObligationNew.Rows[curRow].FindControl("txtObjFreq");


                dt.Rows[curRow][0] = lblPropSLNo.Text;
                dt.Rows[curRow][1] = txtCompanyName.Text;
                dt.Rows[curRow][2] = txtAccType.Text;
                if (txtObjEMI.Text != "")
                    dt.Rows[curRow][3] = Convert.ToDecimal(txtObjEMI.Text);
                else
                    dt.Rows[curRow][3] = 0;
                if (txtTerms.Text != "")
                    dt.Rows[curRow][4] = Convert.ToDecimal(txtTerms.Text);
                else
                    dt.Rows[curRow][4] = 0;

                if (txtObjEMIAmount.Text != "")
                {
                    if (Convert.ToDecimal(txtObjEMIAmount.Text) == 0)
                    {
                        gblFuction.AjxMsgPopup("EMI Amount Can Not Be Zero");
                        return;
                    }
                    dt.Rows[curRow][5] = Convert.ToDecimal(txtObjEMIAmount.Text);
                }
                else
                {
                    gblFuction.AjxMsgPopup("EMI Amount Can Not Be Empty");
                    return;
                }
                if (txtCurrentBalance.Text != "")
                    dt.Rows[curRow][6] = Convert.ToDecimal(txtCurrentBalance.Text);
                else
                    dt.Rows[curRow][6] = 0;
                dt.Rows[curRow][7] = txtObjFreq.Text;

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvObligationNew.Rows[vRow].FindControl("lblPropSLNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["Obligation"] = dt;
                gvObligationNew.DataSource = dt;
                gvObligationNew.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void ImDelDoc_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["Obligation"];
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted ");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["Obligation"] = dt;
                    gvObligationNew.DataSource = dt;
                    gvObligationNew.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
            }
        }
        private void GetObligationList()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("CompanyName", typeof(string));
            dt.Columns.Add("AccType", typeof(string));
            dt.Columns.Add("EMI", typeof(decimal));
            dt.Columns.Add("Term", typeof(int));
            dt.Columns.Add("EMIAmount", typeof(decimal));
            dt.Columns.Add("CurrentBalance", typeof(decimal));
            dt.Columns.Add("Frequency", typeof(string));
            dt.Columns.Add("IsActive", typeof(string));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dt.Rows[0]["CompanyName"] = "";
            dt.Rows[0]["AccType"] = "";
            dt.Rows[0]["EMI"] = 0;
            dt.Rows[0]["Term"] = 0;
            dt.Rows[0]["EMIAmount"] = 0;
            dt.Rows[0]["CurrentBalance"] = 0;
            dt.Rows[0]["Frequency"] = "";
            dt.Rows[0]["IsActive"] = "Y";

            gvObligationNew.DataSource = dt;
            gvObligationNew.DataBind();

            ViewState["Obligation"] = dt;

        }
        protected void txtObjEMIAmount_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            TextBox txtEMIAmt = (TextBox)row.FindControl("txtObjEMIAmount");
            CheckBox chkObl = (CheckBox)row.FindControl("chkObl");
            double TotEMI = 0, TotBal = 0;
            if (gvObligationNew.Rows.Count > 0)
            {
                for (int i = 0; i < gvObligationNew.Rows.Count; i++)
                {
                    string txtObjEMIAmount = ((TextBox)gvObligationNew.Rows[i].FindControl("txtObjEMIAmount")).Text.Trim();
                    string txtCurrentBalance = ((TextBox)gvObligationNew.Rows[i].FindControl("txtCurrentBalance")).Text.Trim();
                    CheckBox ChkActive = (CheckBox)gvObligationNew.Rows[i].FindControl("chkObl");
                    if (ChkActive.Checked == true && txtObjEMIAmount != "")
                        TotEMI = TotEMI + Convert.ToDouble(txtObjEMIAmount);
                    if (ChkActive.Checked == true && txtCurrentBalance != "")
                        TotBal = TotBal + Convert.ToDouble(txtCurrentBalance);
                }
            }
            Label lblFOIRTotalEMIAmount = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalEMIAmount") as Label);
            Label lblFOIRTotalBalPric = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalBalPric") as Label);
            lblFOIRTotalEMIAmount.Text = TotEMI.ToString();
            lblFOIRTotalBalPric.Text = TotBal.ToString();
            CalculateObligationData();
        }
        protected void txtCurrentBalance_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).NamingContainer);
            CheckBox chkObl = (CheckBox)row.FindControl("chkObl");
            double TotEMI = 0, TotBal = 0;
            if (gvObligationNew.Rows.Count > 0)
            {
                for (int i = 0; i < gvObligationNew.Rows.Count; i++)
                {
                    string txtObjEMIAmount = ((TextBox)gvObligationNew.Rows[i].FindControl("txtObjEMIAmount")).Text.Trim();
                    string txtCurrentBalance = ((TextBox)gvObligationNew.Rows[i].FindControl("txtCurrentBalance")).Text.Trim();
                    CheckBox ChkActive = (CheckBox)gvObligationNew.Rows[i].FindControl("chkObl");
                    if (ChkActive.Checked == true && txtObjEMIAmount != "")
                        TotEMI = TotEMI + Convert.ToDouble(txtObjEMIAmount);
                    if (ChkActive.Checked == true && txtCurrentBalance != "")
                        TotBal = TotBal + Convert.ToDouble(txtCurrentBalance);
                }
            }
            Label lblFOIRTotalEMIAmount = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalEMIAmount") as Label);
            Label lblFOIRTotalBalPric = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalBalPric") as Label);
            lblFOIRTotalEMIAmount.Text = TotEMI.ToString();
            lblFOIRTotalBalPric.Text = TotBal.ToString();
            CalculateObligationData();
        }
        protected void chkObl_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
            TextBox txtEMIAmt = (TextBox)row.FindControl("txtObjEMIAmount");
            CheckBox chkObl = (CheckBox)row.FindControl("chkObl");
            double TotEMI = 0, TotBal = 0;
            if (gvObligationNew.Rows.Count > 0)
            {
                for (int i = 0; i < gvObligationNew.Rows.Count; i++)
                {
                    string txtObjEMIAmount = ((TextBox)gvObligationNew.Rows[i].FindControl("txtObjEMIAmount")).Text.Trim();
                    string txtCurrentBalance = ((TextBox)gvObligationNew.Rows[i].FindControl("txtCurrentBalance")).Text.Trim();
                    CheckBox ChkActive = (CheckBox)gvObligationNew.Rows[i].FindControl("chkObl");
                    if (ChkActive.Checked == true && txtObjEMIAmount != "")
                        TotEMI = TotEMI + Convert.ToDouble(txtObjEMIAmount);
                    if (ChkActive.Checked == true && txtCurrentBalance != "")
                        TotBal = TotBal + Convert.ToDouble(txtCurrentBalance);
                }
            }
            Label lblFOIRTotalEMIAmount = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalEMIAmount") as Label);
            Label lblFOIRTotalBalPric = (gvObligationNew.FooterRow.FindControl("lblFOIRTotalBalPric") as Label);
            lblFOIRTotalEMIAmount.Text = TotEMI.ToString();
            lblFOIRTotalBalPric.Text = TotBal.ToString();
            CalculateObligationData();
        }
        #endregion

        #region LTV
        private void GetLTVValByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetLTVValueByLnAppId(pLnAppId);
            ClearLTV();
            if (dt.Rows.Count > 0)
            {
                txtLTVLnAppNo.Text = dt.Rows[0]["LnAppId"].ToString();
                txtLand.Text = dt.Rows[0]["LandInSqFt"].ToString();
                txtTotLandSqFt.Text = dt.Rows[0]["LandInSqFt"].ToString();
                txtGovtValPerSqFt.Text = dt.Rows[0]["GovtValPerSqFt"].ToString();
                txtGovVlaueSqFt.Text = dt.Rows[0]["GovtValInSqFt"].ToString();
                txtGovVlaue.Text = dt.Rows[0]["TotGovtVal"].ToString();
                txtGV.Text = dt.Rows[0]["GVVal"].ToString();
                txtMarkValPerSqFt.Text = dt.Rows[0]["MarketValPerSqFt"].ToString();
                txtMV.Text = dt.Rows[0]["MVVal"].ToString();
                txtLV.Text = dt.Rows[0]["LVVal"].ToString();
                txtBuildAreasSqft.Text = dt.Rows[0]["BuildingAreInSqFt"].ToString();
                txtTotBuilAreaSqft.Text = dt.Rows[0]["BuildingAreInSqFt"].ToString();
                GetYrOfBuilding();
                ddlYrOfBuild.SelectedIndex = ddlYrOfBuild.Items.IndexOf(ddlYrOfBuild.Items.FindByValue(Convert.ToString(dt.Rows[0]["YrOfBuilding"])));
                txtDepre.Text = dt.Rows[0]["Depreciation"].ToString();
                txtStConsCost.Text = dt.Rows[0]["StdConsCost"].ToString();
                txtBuildVal.Text = dt.Rows[0]["BuildingVal"].ToString();
                txtTotPropValue.Text = dt.Rows[0]["TotPropVal"].ToString();
                txtLTVPer.Text = dt.Rows[0]["LTVPer"].ToString();
                txtFinLTVVal.Text = dt.Rows[0]["LTVVal"].ToString();
            }
            else
            {
                txtLTVLnAppNo.Text = pLnAppId;
                GetYrOfBuilding();
            }
        }
        private void ClearLTV()
        {
            txtLand.Text = "0";
            txtTotLandSqFt.Text = "0";
            txtGovtValPerSqFt.Text = "0";
            txtGovVlaueSqFt.Text = "0";
            txtGovVlaue.Text = "0";
            txtGV.Text = "0";
            txtMarkValPerSqFt.Text = "0";
            txtMV.Text = "0";
            txtLV.Text = "0";
            txtBuildAreasSqft.Text = "0";
            txtTotBuilAreaSqft.Text = "0";
            //ddlYrOfBuild.Text = dt.
            txtDepre.Text = "0";
            txtStConsCost.Text = "0";
            txtBuildVal.Text = "0";
            txtTotPropValue.Text = "0";
            txtLTVPer.Text = "50";
            txtFinLTVVal.Text = "0";

        }
        private void GetYrOfBuilding()
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetYrOfBuilding();

            if (dt.Rows.Count > 0)
            {
                ddlYrOfBuild.DataSource = dt;
                ddlYrOfBuild.DataTextField = "NoOfYr";
                ddlYrOfBuild.DataValueField = "NoOfYr";
                ddlYrOfBuild.DataBind();
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ddlYrOfBuild.Items.Insert(0, oli2);
            }
            else
            {
                ddlYrOfBuild.DataSource = null;
                ddlYrOfBuild.DataBind();
            }
        }
        protected void ddlYrOfBuild_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlYrOfBuild.SelectedValue.ToString()) == true)
            {
                gblFuction.AjxMsgPopup("Pease Select Year Of Building Years");
                return;
            }
            int Yr = Convert.ToInt32(ddlYrOfBuild.SelectedValue.ToString());
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetDeprByNoOfYr(Yr);
            if (dt.Rows.Count > 0)
            {
                txtDepre.Text = dt.Rows[0]["Depreciation"].ToString();
            }
            GetBuildingValue();
        }
        protected void txtStConsCost_TextChanged(object sender, EventArgs e)
        {
            GetBuildingValue();
            GetLTVValue();
        }
        private void GetBuildingValue()
        {
            decimal StdCost = 0, Depre = 0, BuildUpAreaSqft = 0, BuildingVal = 0, TotPropertyval = 0, LVValue = 0;
            if (txtBuildAreasSqft.Text != "")
                BuildUpAreaSqft = Convert.ToDecimal(txtBuildAreasSqft.Text);
            if (txtStConsCost.Text != "")
                StdCost = Convert.ToDecimal(txtStConsCost.Text);
            if (txtDepre.Text != "")
                Depre = Convert.ToDecimal(txtDepre.Text);
            BuildingVal = ((BuildUpAreaSqft * StdCost) * (100 - Depre)) / 100;
            txtBuildVal.Text = BuildingVal.ToString();
            if (txtLV.Text != "")
                LVValue = Convert.ToDecimal(txtLV.Text);
            TotPropertyval = (BuildingVal + LVValue);
            txtTotPropValue.Text = TotPropertyval.ToString();
        }
        private void GetGovtValue()
        {
            decimal TotallandSqFt = 0, GovtValuePerSqFt = 0, SqFt = 0, TotalGovValue = 0, GVValue = 0, MVValue = 0;
            if (txtLand.Text != "")
                TotallandSqFt = Convert.ToDecimal(txtLand.Text);
            if (txtGovtValPerSqFt.Text != "")
                GovtValuePerSqFt = Convert.ToDecimal(txtGovtValPerSqFt.Text);
            if (txtGovVlaueSqFt.Text != "")
                SqFt = Convert.ToDecimal(txtGovVlaueSqFt.Text);
            TotalGovValue = (GovtValuePerSqFt * SqFt);
            txtGovVlaue.Text = TotalGovValue.ToString();
            GVValue = (TotallandSqFt * TotalGovValue);
            txtGV.Text = GVValue.ToString();
            if (txtMV.Text != "")
                MVValue = Convert.ToDecimal(txtMV.Text);
            if (GVValue < MVValue)
                txtLV.Text = GVValue.ToString();
            else
                txtLV.Text = MVValue.ToString();
        }
        private void GetMarketValue()
        {
            decimal TotallandSqFt = 0, MarktValuePerSqFt = 0, MVValue = 0, GVValue = 0;
            if (txtLand.Text != "")
                TotallandSqFt = Convert.ToDecimal(txtLand.Text);
            if (txtMarkValPerSqFt.Text != "")
                MarktValuePerSqFt = Convert.ToDecimal(txtMarkValPerSqFt.Text);
            MVValue = (TotallandSqFt * MarktValuePerSqFt);
            txtMV.Text = MVValue.ToString();
            if (txtGV.Text != "")
                GVValue = Convert.ToDecimal(txtGV.Text);
            if (GVValue < MVValue)
                txtLV.Text = GVValue.ToString();
            else
                txtLV.Text = MVValue.ToString();
        }
        private void GetLTVValue()
        {
            decimal TotPropertyval = 0, LTVPer = 0, LTVValue = 0;
            if (txtTotPropValue.Text != "")
                TotPropertyval = Convert.ToDecimal(txtTotPropValue.Text);
            if (txtLTVPer.Text != "")
                LTVPer = Convert.ToDecimal(txtLTVPer.Text);
            LTVValue = (TotPropertyval * LTVPer) / 100;
            txtFinLTVVal.Text = Math.Round(LTVValue, 0).ToString();
        }
        protected void txtBuildAreasSqft_TextChanged(object sender, EventArgs e)
        {
            txtTotBuilAreaSqft.Text = txtBuildAreasSqft.Text;
            GetBuildingValue();
        }
        protected void txtLand_TextChanged(object sender, EventArgs e)
        {
            txtTotLandSqFt.Text = txtLand.Text;
            GetGovtValue();
        }
        protected void txtGovVlaueSqFt_TextChanged(object sender, EventArgs e)
        {
            GetGovtValue();
        }
        protected void txtGovtValPerSqFt_TextChanged(object sender, EventArgs e)
        {
            GetGovtValue();
        }
        protected void txtMarkValPerSqFt_TextChanged(object sender, EventArgs e)
        {
            GetMarketValue();
        }
        protected void txtLTVPer_TextChanged(object sender, EventArgs e)
        {
            GetLTVValue();
        }
        protected void btnSavePropVal_Click(object sender, EventArgs e)
        {
            string pLnAppId = "";
            if (txtLTVLnAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank,Please click on Show Informatiom");
                return;
            }
            else
                pLnAppId = txtLTVLnAppNo.Text.ToString().Trim();
            Int32 vErr = 0;
            decimal pLandInSqFt = 0, pGovtValPerSqFt = 0, pGovtValInSqFt = 0, pTotGovtVal = 0, pGVVal = 0, pMarketValPerSqFt = 0, pMVVal = 0, pLVVal = 0,
                   pBuildingAreInSqFt = 0, pYrOfBuilding = 0, pDepreciation = 0, pStdConsCost = 0, pBuildingVal = 0, pTotPropVal = 0, pLTVPer = 0, pLTVVal = 0;

            pLandInSqFt = Convert.ToDecimal(txtLand.Text);
            pGovtValPerSqFt = Convert.ToDecimal(txtGovtValPerSqFt.Text);
            pGovtValInSqFt = Convert.ToDecimal(txtGovVlaueSqFt.Text);
            pTotGovtVal = Convert.ToDecimal(txtGovVlaue.Text);
            pGVVal = Convert.ToDecimal(txtGV.Text);

            pMarketValPerSqFt = Convert.ToDecimal(txtMarkValPerSqFt.Text);
            pMVVal = Convert.ToDecimal(txtMV.Text);
            pLVVal = Convert.ToDecimal(txtLV.Text);
            pBuildingAreInSqFt = Convert.ToDecimal(txtBuildAreasSqft.Text);
            pYrOfBuilding = Convert.ToDecimal(ddlYrOfBuild.SelectedValue.ToString());

            pDepreciation = Convert.ToDecimal(txtDepre.Text);
            pStdConsCost = Convert.ToDecimal(txtStConsCost.Text);
            pBuildingVal = Convert.ToDecimal(txtBuildVal.Text);
            pTotPropVal = Convert.ToDecimal(txtTotPropValue.Text);
            pLTVPer = Convert.ToDecimal(txtLTVPer.Text);
            pLTVVal = Convert.ToDecimal(txtFinLTVVal.Text);
            CMember OMem = new CMember();


            // public Int32 SaveLTVValue(string pLnAppId, decimal pLandInSqFt, decimal pGovtValPerSqFt, decimal pGovtValInSqFt, decimal pTotGovtVal, 
            // decimal pGVVal, decimal pMarketValPerSqFt,decimal pMVVal, decimal pLVVal, decimal pBuildingAreInSqFt, decimal pYrOfBuilding,
            //decimal pDepreciation, decimal pStdConsCost, decimal pBuildingVal, decimal pTotPropVal, decimal pLTVPer, decimal pLTVVal,
            // Int32 pCreatedBy, Int32 pErr)

            vErr = OMem.SaveLTVValue(pLnAppId, pLandInSqFt, pGovtValPerSqFt, pGovtValInSqFt, pTotGovtVal, pGVVal, pMarketValPerSqFt, pMVVal, pLVVal, pBuildingAreInSqFt,
                pYrOfBuilding, pDepreciation, pStdConsCost, pBuildingVal, pTotPropVal, pLTVPer, pLTVVal, Convert.ToInt32(Session[gblValue.UserId]), 0);
            if (vErr > 0)
            {
                gblFuction.MsgPopup("Property Valuation Saved Successfully");
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        #endregion

        #region PD Final Approve
        private Int32 ApprovalLevel(Int32 RoleId, string Mode)
        {
            int Output = 0;
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetLevelApprovalByRoleId(RoleId, Mode);
            if (dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["ApprovalLevel"].ToString()) == false)
                {
                    Output = Convert.ToInt32(dt.Rows[0]["ApprovalLevel"].ToString());
                }
            }
            return Output;
        }
        private bool CheckApproval()
        {
            bool vResult = true;
            int x = ApprovalLevel(Convert.ToInt32(Session[gblValue.RoleId].ToString()), "O");
            if (chkOpLevel1.Checked == true)
            {
                if (x == 1)
                    vResult = true;
                else if (x > 1)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkOpLevel2.Checked == true)
            {
                if (x == 2)
                    vResult = true;
                else if (x > 2)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkOpLevel3.Checked == true)
            {
                if (x == 3)
                    vResult = true;
                else if (x > 3)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkOpLevel4.Checked == true)
            {
                if (x == 4)
                    vResult = true;
                else if (x > 4)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkOpLevel5.Checked == true)
            {
                if (x == 5)
                    vResult = true;
                else if (x > 5)
                    vResult = true;
                else
                    vResult = false;
            }
            if (chkOpLevel6.Checked == true)
            {
                if (x == 6)
                    vResult = true;
                else
                    vResult = false;
            }
            return vResult;
        }
        private void GetLevelApprovalRange()
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            DateTime pDate = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            dt = OMem.GetOpLevelRange(pDate);

            if (dt.Rows.Count > 0)
            {
                lblLevel1Amt.Text = dt.Rows[0]["OpLevel1"].ToString();
                lblLevel2Amt.Text = dt.Rows[0]["OpLevel2"].ToString();
                lblLevel3Amt.Text = dt.Rows[0]["OpLevel3"].ToString();
                lblLevel4Amt.Text = dt.Rows[0]["OpLevel4"].ToString();
                lblLevel5Amt.Text = dt.Rows[0]["OpLevel5"].ToString();
                lblLevel6Amt.Text = dt.Rows[0]["OpLevel6"].ToString();
            }
            else
            {
                lblLevel1Amt.Text = "0";
                lblLevel2Amt.Text = "0";
                lblLevel3Amt.Text = "0";
                lblLevel4Amt.Text = "0";
                lblLevel5Amt.Text = "0";
                lblLevel6Amt.Text = "0";
            }
        }
        private void LoadPDFinalApprove(string pLnAppId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            CMember oMem = new CMember();
            try
            {

                ds = oMem.GetLnAppDetailsForFinalPD(ViewState["LoanAppId"].ToString());
                if (ds.Tables.Count > 0)
                {
                    ClearPDFinalApprove();
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        txtFinalPDLnApp.Text = dt1.Rows[0]["LoanAppNo"].ToString();
                        txtFinalPDCustName.Text = dt1.Rows[0]["CompanyName"].ToString();
                        hdFinalPDCustId.Value = dt1.Rows[0]["CustId"].ToString();
                        txtFinalPDAppDt.Text = dt1.Rows[0]["ApplicationDt"].ToString();
                        txtLnAppAmt.Text = dt1.Rows[0]["AppAmount"].ToString();
                        txtFinalPDAppTenure.Text = dt1.Rows[0]["Tenure"].ToString();
                        txtFinalPDAppType.Text = dt1.Rows[0]["LoanTypeName"].ToString();
                        txtFinalPDLnPurpose.Text = dt1.Rows[0]["PurposeName"].ToString();
                        hdFinalLNSanction.Value = dt1.Rows[0]["SanctionYN"].ToString();
                        if (string.IsNullOrEmpty(dt1.Rows[0]["FinalPDDate"].ToString()))
                        {
                            txtFinalPDDate.Text = Session[gblValue.LoginDate].ToString();
                        }
                        else
                        {
                            txtFinalPDDate.Text = dt1.Rows[0]["FinalPDDate"].ToString();
                        }

                        txtFinalRemarks.Text = dt1.Rows[0]["FinalPDRemarks"].ToString();
                        ddlApproveType.SelectedIndex = ddlApproveType.Items.IndexOf(ddlApproveType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["FinalPDStatus"])));
                        hdFinalPDAppStatus.Value = dt1.Rows[0]["FinalPDStatus"].ToString();
                        hdFinalPDId.Value = "";
                        if (dt1.Rows[0]["OpLevel1"].ToString() == "Y")
                            chkOpLevel1.Checked = true;
                        else
                            chkOpLevel1.Checked = false;
                        if (dt1.Rows[0]["OpLevel2"].ToString() == "Y")
                            chkOpLevel2.Checked = true;
                        else
                            chkOpLevel2.Checked = false;
                        if (dt1.Rows[0]["OpLevel3"].ToString() == "Y")
                            chkOpLevel3.Checked = true;
                        else
                            chkOpLevel3.Checked = false;
                        if (dt1.Rows[0]["OpLevel4"].ToString() == "Y")
                            chkOpLevel4.Checked = true;
                        else
                            chkOpLevel4.Checked = false;
                        if (dt1.Rows[0]["OpLevel5"].ToString() == "Y")
                            chkOpLevel5.Checked = true;
                        else
                            chkOpLevel5.Checked = false;
                        if (dt1.Rows[0]["OpLevel6"].ToString() == "Y")
                            chkOpLevel6.Checked = true;
                        else
                            chkOpLevel6.Checked = false;
                    }
                    else
                    {
                        txtFinalPDLnApp.Text = "";
                        txtFinalPDCustName.Text = "";
                        hdFinalPDCustId.Value = "";
                        txtFinalPDDate.Text = Session[gblValue.LoginDate].ToString();
                        txtFinalPDAppAmt.Text = "";
                        txtFinalPDAppTenure.Text = "";
                        txtFinalPDAppType.Text = "";
                        txtFinalPDLnPurpose.Text = "";
                        hdFinalLNSanction.Value = "";
                        txtFinalPDDate.Text = "";
                        txtFinalRemarks.Text = "";
                        ddlApproveType.SelectedIndex = ddlApproveType.Items.IndexOf(ddlApproveType.Items.FindByValue("N"));
                        hdFinalPDId.Value = "";
                        chkOpLevel1.Checked = false;
                        chkOpLevel2.Checked = false;
                        chkOpLevel3.Checked = false;
                        chkOpLevel4.Checked = false;
                        chkOpLevel5.Checked = false;
                        chkOpLevel6.Checked = false;
                        hdFinalPDAppStatus.Value = "";
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        txtIIRVal.Text = dt2.Rows[0]["IIRVal"].ToString();
                        txtFOIRVal.Text = dt2.Rows[0]["FOIRVal"].ToString();
                        txtLTVVal.Text = dt2.Rows[0]["LTVVal"].ToString();
                        txtFinalPDAppAmt.Text = dt2.Rows[0]["SanctionAmt"].ToString();
                    }
                    else
                    {
                        txtIIRVal.Text = "0";
                        txtFOIRVal.Text = "0";
                        txtLTVVal.Text = "0";
                        txtFinalPDAppAmt.Text = "0";
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
            }
        }
        private void ClearPDFinalApprove()
        {
            txtFinalPDLnApp.Text = "";
            txtFinalPDCustName.Text = "";
            hdFinalPDCustId.Value = "";
            txtFinalPDDate.Text = Session[gblValue.LoginDate].ToString();
            txtLnAppAmt.Text = "";

            txtFinalPDAppTenure.Text = "";
            txtFinalPDAppType.Text = "";
            txtFinalPDLnPurpose.Text = "";
            hdFinalLNSanction.Value = "";
            txtFinalPDDate.Text = "";
            txtFinalRemarks.Text = "";
            ddlApproveType.SelectedIndex = ddlApproveType.Items.IndexOf(ddlApproveType.Items.FindByValue("N"));
            hdFinalPDId.Value = "";
            chkOpLevel1.Checked = false;
            chkOpLevel2.Checked = false;
            chkOpLevel3.Checked = false;
            chkOpLevel4.Checked = false;
            chkOpLevel5.Checked = false;
            chkOpLevel6.Checked = false;
            hdFinalPDAppStatus.Value = "";
            txtIIRVal.Text = "";
            txtFOIRVal.Text = "";
            txtLTVVal.Text = "";
            txtFinalPDAppAmt.Text = "";
        }
        protected void chkOpLevel1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel1.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel1.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have Level 1 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (lblLevel1Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel1Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                else
                {
                    if (lblLevel1Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel1Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }

            }
        }
        protected void chkOpLevel2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel2.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel2.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have Level 2 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkOpLevel1.Checked == false)
                    {
                        chkOpLevel2.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 Approval Should Be Done Before Level 2 Approval..");
                        return;
                    }
                    if (lblLevel2Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel2Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel2Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel2Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                    {
                        hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkOpLevel3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel3.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel3.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 3 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkOpLevel1.Checked == false || chkOpLevel2.Checked == false)
                    {
                        chkOpLevel3.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2 Approval Should Be Done Before Level 3 Approval..");
                        return;
                    }
                    if (lblLevel3Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel3Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel3Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel3Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                    {
                        hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkOpLevel4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel4.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel4.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 4 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkOpLevel1.Checked == false || chkOpLevel2.Checked == false || chkOpLevel3.Checked == false)
                    {
                        chkOpLevel4.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2/ Level 3 Approval Should Be Done Before Level 4 Approval..");
                        return;
                    }
                    if (lblLevel4Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel4Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel4Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel4Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                    {
                        hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkOpLevel5_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel5.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel5.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 5 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkOpLevel1.Checked == false || chkOpLevel2.Checked == false || chkOpLevel3.Checked == false || chkOpLevel4.Checked == false)
                    {
                        chkOpLevel5.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2/ Level 3/ Level 4 Approval Should Be Done Before Level 5 Approval..");
                        return;
                    }
                    if (lblLevel5Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel5Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel5Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel5Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                    {
                        hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void chkOpLevel6_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOpLevel6.Checked == true)
            {
                if (Convert.ToInt32(Session[gblValue.RoleId].ToString()) != 1)
                {
                    if (CheckApproval() == false)
                    {
                        chkOpLevel6.Checked = false;
                        gblFuction.AjxMsgPopup("You Donot Have  Level 6 Approval Permission, Please Check Role..");
                        return;
                    }
                    if (chkOpLevel1.Checked == false || chkOpLevel2.Checked == false || chkOpLevel3.Checked == false || chkOpLevel4.Checked == false || chkOpLevel5.Checked == false)
                    {
                        chkOpLevel6.Checked = false;
                        gblFuction.AjxMsgPopup("Level 1 / Level 2/ Level 3/ Level 4/ Level 5 Approval Should Be Done Before Level 6 Approval..");
                        return;
                    }
                    if (lblLevel6Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                    {
                        if (Convert.ToDouble(lblLevel6Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                        {
                            hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                        }
                    }
                }
                if (lblLevel6Amt.Text != "" && txtFinalPDAppAmt.Text != "")
                {
                    if (Convert.ToDouble(lblLevel6Amt.Text) >= Convert.ToDouble(txtFinalPDAppAmt.Text))
                    {
                        hdFinalPDAppStatus.Value = "A"; // A--- Final PD Approve
                    }
                }
            }
        }
        protected void btnFinalApprove_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab For Final PD Approval..");
                    return;
                }
                else
                {
                    if (txtFinalPDDate.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Final PD Approval Date Can Not Be Blank");
                        return;
                    }
                    if (ddlApproveType.SelectedValue == "-1")
                    {
                        gblFuction.AjxMsgPopup("Please Select Final PD Approval Status");
                        return;
                    }
                    if (gblFuction.setDate(txtFinalPDDate.Text) < gblFuction.setDate(txtFinalPDAppDt.Text))
                    {
                        gblFuction.AjxMsgPopup("Final PD Approval Date Can Not Be Less than Loan Application Date");
                        return;
                    }
                    //if (hdFinalLNSanction.Value == "Y")
                    //{
                    //    gblFuction.MsgPopup("After Final Sanction You are Not Allowed To Save/Update Final PD Approval Status..");
                    //    return;
                    //}
                    if (ddlApproveType.SelectedValue != "N")
                    {
                        string vMsg = "";
                        vMsg = oMem.GetPDCondForFinalApprove(ViewState["LoanAppId"].ToString(), gblFuction.setDate(txtFinalPDDate.Text));
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return;
                        }
                    }

                    string pLevel1 = "", pLevel2 = "", pLevel3 = "", pLevel4 = "", pLevel5 = "", pLevel6 = "", pFinalPDApprove = "";
                    if (chkOpLevel1.Checked == true)
                        pLevel1 = "Y";
                    if (chkOpLevel2.Checked == true)
                        pLevel2 = "Y";
                    if (chkOpLevel3.Checked == true)
                        pLevel3 = "Y";
                    if (chkOpLevel4.Checked == true)
                        pLevel4 = "Y";
                    if (chkOpLevel5.Checked == true)
                        pLevel5 = "Y";
                    if (chkOpLevel6.Checked == true)
                        pLevel6 = "Y";
                    pFinalPDApprove = hdFinalPDAppStatus.Value.ToString();
                    decimal pSancAmt = 0;
                    if (txtFinalPDAppAmt.Text == "" || txtFinalPDAppAmt.Text == "0")
                    {
                        gblFuction.AjxMsgPopup("Sanction Amount Can Not Be Empty or zero");
                        return;
                    }
                    pSancAmt = Convert.ToDecimal(txtFinalPDAppAmt.Text);

                    Int32 vErr = oMem.SavePDFinalApprove(ViewState["LoanAppId"].ToString(), pFinalPDApprove, gblFuction.setDate(txtFinalPDDate.Text),
                        txtFinalRemarks.Text, Convert.ToInt32(Session[gblValue.UserId].ToString()), pLevel1, pLevel2, pLevel3, pLevel4, pLevel5, pLevel6,
                        pSancAmt);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        mView.ActiveViewIndex = 0;
                        tbMem.ActiveTabIndex = 8;
                        LoadPDByRisk(ViewState["LoanAppId"].ToString());
                        ViewAcess();
                        return;
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup(gblPRATAM.DBError);
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
            }
        }
        #endregion

        #region PDQuestionAnswer

        string pUserType = "";
        protected void lbQABM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(ViewState["LoanAppId"])) == true)
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            Session["PDDoneBy"] = "BM";
            ViewState["UserType"] = pUserType;
            GetRelationPropertyOwnerTypeOfOwner();
            mView.ActiveViewIndex = 5;
            txtPDQALnApp.Text = ViewState["LoanAppId"].ToString();
            txtPDQADate.Text = Session[gblValue.LoginDate].ToString();

            gvPDQuesAns.DataSource = null;
            gvPDQuesAns.DataBind();
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            dt = OMem.GetPDQuestionAnswerMstByLnAppId(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                gvPDQuesAns.DataSource = dt;
                gvPDQuesAns.DataBind();
            }
            else
            {
                gvPDQuesAns.DataSource = null;
                gvPDQuesAns.DataBind();
            }
            ClearPersonalDt();
            ClearPDInScBusiness();
            ClearPDInScSalary();
            ClearPDInScRental();
            ClearPDInScPension();
            ClearPDInScWages();
            ClearPDPropertyDtl();
            ClearLoanRequireDtl();
            ClearSocialBehaviour();
            ClearNeighbourReference();
            ClearPosNegFeedBack();
            ClearSaving();
        }
        protected void lbQACM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ViewState["LoanAppId"].ToString()) == true)
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            Session["PDDoneBy"] = "CM";
            ViewState["UserType"] = pUserType;
            GetRelationPropertyOwnerTypeOfOwner();
            mView.ActiveViewIndex = 5;
            txtPDQALnApp.Text = ViewState["LoanAppId"].ToString();
            txtPDQADate.Text = Session[gblValue.LoginDate].ToString();


            gvPDQuesAns.DataSource = null;
            gvPDQuesAns.DataBind();
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            dt = OMem.GetPDQuestionAnswerMstByLnAppId(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                gvPDQuesAns.DataSource = dt;
                gvPDQuesAns.DataBind();
            }
            else
            {
                gvPDQuesAns.DataSource = null;
                gvPDQuesAns.DataBind();
            }
            ClearPersonalDt();
            ClearPDInScBusiness();
            ClearPDInScSalary();
            ClearPDInScRental();
            ClearPDInScPension();
            ClearPDInScWages();
            ClearPDPropertyDtl();
            ClearLoanRequireDtl();
            ClearSocialBehaviour();
            ClearNeighbourReference();
            ClearPosNegFeedBack();
            ClearSaving();
        }
        protected void lbQARM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ViewState["LoanAppId"].ToString()) == true)
            {
                gblFuction.AjxMsgPopup("Loan Application No Can Not Be Blank");
                return;
            }
            Session["PDDoneBy"] = "RM";
            ViewState["UserType"] = pUserType;
            GetRelationPropertyOwnerTypeOfOwner();
            mView.ActiveViewIndex = 5;
            txtPDQALnApp.Text = ViewState["LoanAppId"].ToString();
            txtPDQADate.Text = Session[gblValue.LoginDate].ToString();

            gvPDQuesAns.DataSource = null;
            gvPDQuesAns.DataBind();
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            dt = OMem.GetPDQuestionAnswerMstByLnAppId(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                gvPDQuesAns.DataSource = dt;
                gvPDQuesAns.DataBind();
            }
            else
            {
                gvPDQuesAns.DataSource = null;
                gvPDQuesAns.DataBind();
            }
            ClearPersonalDt();
            ClearPDInScBusiness();
            ClearPDInScSalary();
            ClearPDInScRental();
            ClearPDInScPension();
            ClearPDInScWages();
            ClearPDPropertyDtl();
            ClearLoanRequireDtl();
            ClearSocialBehaviour();
            ClearNeighbourReference();
            ClearPosNegFeedBack();
            ClearSaving();
        }
        protected void gvPDQuesAns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vApplicantionID = "", pCustomerType = "";
            vApplicantionID = Convert.ToString(e.CommandArgument);
            ViewState["LoanAppId"] = vApplicantionID;

            if (e.CommandName == "cmdShowInfo")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowQuesAnsInfo");
                string pCustID = gvRow.Cells[1].Text.ToString();
                string pCustType = gvRow.Cells[3].Text.ToString();
                if (pCustType == "Applicant")
                    pCustomerType = "A";
                else if (pCustType == "Co Applicant")
                    pCustomerType = "C";
                else
                    pCustomerType = "";
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvLoanApp.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ViewState["CustID"] = pCustID.ToString();
                ViewState["CustType"] = pCustomerType.ToString();
                ShowQuestionAnswerDtl(vApplicantionID, pCustID, pCustomerType);
            }
        }
        private void ShowQuestionAnswerDtl(string LnAppId, string pCustID, string pCustType)
        {
            pUserType = Session["PDDoneBy"].ToString();
            // For Personal Details
            GetPDPersonalDtlByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Income Source Business
            GetPDIncomeSourceBusinessByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Income Source Salary
            GetPDIncomeSourceSalaryByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Rental
            GetPDIncomeSourceRentalByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Pension
            GetPDIncomeSourcePensionByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Wages
            GetPDIncomeSourceWagesByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Property Details
            GetPDPropDetailsByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Loan Require Details
            GetPDLoanRequireDetailByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Social Behavoiur
            GetPDSocialBehabiourByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Neighbour Reference
            GetPDNeighbourReferenceCheckByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // For Positive Negetive Observation
            GetPDPositiveNegativeObservationByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);
            // for Invesment and Saving
            GetPDInvestmentSavingByLnAppId(ViewState["LoanAppId"].ToString(), pUserType, pCustID, pCustType);

            GetPDPersonalReerencefByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDApplicantProfileByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDCoApplicantProfileByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDIncomeSourceAggricultureByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDBusinessReference1ByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDBusinessReference2ByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

            GetPDBankBehaviourByLnAppId(ViewState["LoanAppId"].ToString(), pCustType);

        }


        protected void GetCustomerDtlByLoanAppId(string pLoanAppId, string pCustID, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            dt = OMem.GetCustomerDtlByLnAppId(pLoanAppId, pCustID, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtQAAppname.Text = dt.Rows[0]["AppName"].ToString();
                ddlQAAppRelation.SelectedIndex = ddlQAAppRelation.Items.IndexOf(ddlQAAppRelation.Items.FindByValue(Convert.ToString(dt.Rows[0]["RelationId"])));
                ddlQAMStatus.SelectedIndex = ddlQAMStatus.Items.IndexOf(ddlQAMStatus.Items.FindByValue(Convert.ToString(dt.Rows[0]["MaritalStatus"])));
                txtQAAppDOB.Text = dt.Rows[0]["DOB"].ToString();
                txtQAAppAge.Text = dt.Rows[0]["Age"].ToString();
                txtQAPA.Text = dt.Rows[0]["PerAddress"].ToString();
            }
        }
        protected void GetPDPersonalDtlByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPersonalDt();
            dt = OMem.GetPDPersonalDtlByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtQAAppname.Text = dt.Rows[0]["AppName"].ToString();
                txtQAAppnameObj.Text = dt.Rows[0]["AppNameObj"].ToString();
                txtQAAppnameVer.Text = dt.Rows[0]["AppNameVer"].ToString();
                txtQAAppnameRemark.Text = dt.Rows[0]["AppNameRemark"].ToString();

                ddlQAAppRelation.SelectedIndex = ddlQAAppRelation.Items.IndexOf(ddlQAAppRelation.Items.FindByValue(Convert.ToString(dt.Rows[0]["AppRelId"])));
                txtQAAppRelationObj.Text = dt.Rows[0]["AppRelObj"].ToString();
                txtQAAppRelationVer.Text = dt.Rows[0]["AppRelVer"].ToString();
                txtQAAppRelationRemark.Text = dt.Rows[0]["AppRelRemark"].ToString();

                txtQAAppDOB.Text = dt.Rows[0]["DOB"].ToString();
                txtQAAppDOBObj.Text = dt.Rows[0]["DOBObj"].ToString();
                txtQAAppDOBVer.Text = dt.Rows[0]["DOBVer"].ToString();
                txtQAAppDOBRemark.Text = dt.Rows[0]["DOBRemark"].ToString();

                txtQAAppAge.Text = dt.Rows[0]["Age"].ToString();
                txtQAAppAgeObj.Text = dt.Rows[0]["AgeObj"].ToString();
                txtQAAppAgeVer.Text = dt.Rows[0]["AgeVer"].ToString();
                txtQAAppAgeRemark.Text = dt.Rows[0]["AgeRemark"].ToString();

                ddlQAMStatus.SelectedIndex = ddlQAMStatus.Items.IndexOf(ddlQAMStatus.Items.FindByValue(Convert.ToString(dt.Rows[0]["MaritalId"])));
                txtQAMStatusObj.Text = dt.Rows[0]["MaritalObj"].ToString();
                txtQAMStatusVer.Text = dt.Rows[0]["MaritalVer"].ToString();
                txtQAMStatusRemark.Text = dt.Rows[0]["MaritalRemark"].ToString();

                ddlQAChild.SelectedIndex = ddlQAChild.Items.IndexOf(ddlQAChild.Items.FindByValue(Convert.ToString(dt.Rows[0]["NoOfChild"])));
                txtQAChildObj.Text = dt.Rows[0]["NoOfChildObj"].ToString();
                txtQAChildVer.Text = dt.Rows[0]["NoOfChildVer"].ToString();
                txtQAChildRemark.Text = dt.Rows[0]["NoOfChildRemark"].ToString();

                ddlQAEM.SelectedIndex = ddlQAEM.Items.IndexOf(ddlQAEM.Items.FindByValue(Convert.ToString(dt.Rows[0]["EarMem"])));
                txtQAEMObj.Text = dt.Rows[0]["EarMemObj"].ToString();
                txtQAEMVer.Text = dt.Rows[0]["EarMemVer"].ToString();
                txtQAEMRemark.Text = dt.Rows[0]["EarMemRemark"].ToString();

                ddlQAPO.SelectedIndex = ddlQAPO.Items.IndexOf(ddlQAPO.Items.FindByValue(Convert.ToString(dt.Rows[0]["PropOwn"])));
                txtQAPOObj.Text = dt.Rows[0]["PropOwnObj"].ToString();
                txtQAPOVer.Text = dt.Rows[0]["PropOwnVer"].ToString();
                txtQAPORemark.Text = dt.Rows[0]["PropOwnRemark"].ToString();

                ddlQAOT.SelectedIndex = ddlQAOT.Items.IndexOf(ddlQAOT.Items.FindByValue(Convert.ToString(dt.Rows[0]["OwnType"])));
                txtQAOTObj.Text = dt.Rows[0]["OwnTypeObj"].ToString();
                txtQAOTVer.Text = dt.Rows[0]["OwnTypeVer"].ToString();
                txtQAOTRemark.Text = dt.Rows[0]["OwnTypeRemark"].ToString();

                txtQAHO.Text = dt.Rows[0]["HouseObs"].ToString();
                txtQAHOObj.Text = dt.Rows[0]["HouseObsObj"].ToString();
                txtQAHOVer.Text = dt.Rows[0]["HouseObsVer"].ToString();
                txtQAHORemark.Text = dt.Rows[0]["HouseObsRemark"].ToString();

                txtQAPA.Text = dt.Rows[0]["PerAddress"].ToString();
                txtQAPAObj.Text = dt.Rows[0]["PerAddressObj"].ToString();
                txtQAPAVer.Text = dt.Rows[0]["PerAddressVer"].ToString();
                txtQAPARemark.Text = dt.Rows[0]["PerAddressRemark"].ToString();

                txtQAMob.Text = dt.Rows[0]["MobNo"].ToString();
                txtQAMobObj.Text = dt.Rows[0]["MobNoObj"].ToString();
                txtQAMobVer.Text = dt.Rows[0]["MobNoVer"].ToString();
                txtQAMobRemark.Text = dt.Rows[0]["MobNoRemark"].ToString();

                ddlQASchool.SelectedIndex = ddlQASchool.Items.IndexOf(ddlQASchool.Items.FindByValue(Convert.ToString(dt.Rows[0]["School"])));
                txtQASchoolObj.Text = dt.Rows[0]["SchoolObj"].ToString();
                txtQASchoolVer.Text = dt.Rows[0]["SchoolVer"].ToString();
                txtQASchoolRemark.Text = dt.Rows[0]["SchoolRemark"].ToString();
            }
            else
            {
                GetCustomerDtlByLoanAppId(pLoanAppId, pCustId, pCustType);
            }
        }
        protected void GetPDPersonalReerencefByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPersonalReference();
            dt = OMem.GetPDPersonalReferencefByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtRPHName.Text = dt.Rows[0]["Name"].ToString();
                txtRPHContactNo.Text = dt.Rows[0]["ContactNo"].ToString();
                ddlRPHRelation.SelectedIndex = ddlRPHRelation.Items.IndexOf(ddlRPHRelation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Relation"])));
                txtRPHResidance.Text = dt.Rows[0]["Residence"].ToString();
                ddlRPHOccupation.SelectedIndex = ddlRPHOccupation.Items.IndexOf(ddlRPHOccupation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Occupation"])));
                ddlRPHNoofYears.SelectedIndex = ddlRPHNoofYears.Items.IndexOf(ddlRPHNoofYears.Items.FindByValue(Convert.ToString(dt.Rows[0]["KnowHimYears"])));
            }
        }
        protected void GetPDApplicantProfileByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearApplicantProfile();
            dt = OMem.GetPDApplicantProfileByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlAppCoOper.SelectedIndex = ddlAppCoOper.Items.IndexOf(ddlAppCoOper.Items.FindByValue(Convert.ToString(dt.Rows[0]["CoOperative"])));
                ddlAppAccuInfo.SelectedIndex = ddlAppAccuInfo.Items.IndexOf(ddlAppAccuInfo.Items.FindByValue(Convert.ToString(dt.Rows[0]["Accuracy"])));

                ddlAppBusiKnow.SelectedIndex = ddlAppBusiKnow.Items.IndexOf(ddlAppBusiKnow.Items.FindByValue(Convert.ToString(dt.Rows[0]["Business"])));
                ddlAppHouseHold.SelectedIndex = ddlAppHouseHold.Items.IndexOf(ddlAppHouseHold.Items.FindByValue(Convert.ToString(dt.Rows[0]["Household"])));

                ddlAppSavingCapacity.SelectedIndex = ddlAppSavingCapacity.Items.IndexOf(ddlAppSavingCapacity.Items.FindByValue(Convert.ToString(dt.Rows[0]["Savings"])));
                ddlAppQualityInventory.SelectedIndex = ddlAppQualityInventory.Items.IndexOf(ddlAppQualityInventory.Items.FindByValue(Convert.ToString(dt.Rows[0]["Inventroy"])));

                ddlAppPhyFitness.SelectedIndex = ddlAppPhyFitness.Items.IndexOf(ddlAppPhyFitness.Items.FindByValue(Convert.ToString(dt.Rows[0]["PhysicalFitness"])));
                ddlAppFamilySuppot.SelectedIndex = ddlAppFamilySuppot.Items.IndexOf(ddlAppFamilySuppot.Items.FindByValue(Convert.ToString(dt.Rows[0]["FamilySupport"])));
            }
        }
        protected void GetPDCoApplicantProfileByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearCoApplicantProfile();
            dt = OMem.GetPDCoApplicantProfileByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlCoAppCoOper.SelectedIndex = ddlCoAppCoOper.Items.IndexOf(ddlCoAppCoOper.Items.FindByValue(Convert.ToString(dt.Rows[0]["CoOperative"])));
                ddlCoAppAccuInfo.SelectedIndex = ddlCoAppAccuInfo.Items.IndexOf(ddlCoAppAccuInfo.Items.FindByValue(Convert.ToString(dt.Rows[0]["Accuracy"])));

                ddlCoAppBusiKnow.SelectedIndex = ddlCoAppBusiKnow.Items.IndexOf(ddlCoAppBusiKnow.Items.FindByValue(Convert.ToString(dt.Rows[0]["Business"])));
                ddlCoAppHouseHold.SelectedIndex = ddlCoAppHouseHold.Items.IndexOf(ddlCoAppHouseHold.Items.FindByValue(Convert.ToString(dt.Rows[0]["Household"])));

                ddlCoAppSavingCapacity.SelectedIndex = ddlCoAppSavingCapacity.Items.IndexOf(ddlCoAppSavingCapacity.Items.FindByValue(Convert.ToString(dt.Rows[0]["Savings"])));
                ddlCoAppQualityInventory.SelectedIndex = ddlCoAppQualityInventory.Items.IndexOf(ddlCoAppQualityInventory.Items.FindByValue(Convert.ToString(dt.Rows[0]["Inventroy"])));

                ddlCoAppPhyFitness.SelectedIndex = ddlCoAppPhyFitness.Items.IndexOf(ddlCoAppPhyFitness.Items.FindByValue(Convert.ToString(dt.Rows[0]["PhysicalFitness"])));
                ddlCoAppFamilySuppot.SelectedIndex = ddlCoAppFamilySuppot.Items.IndexOf(ddlCoAppFamilySuppot.Items.FindByValue(Convert.ToString(dt.Rows[0]["FamilySupport"])));
            }
        }
        protected void GetPDIncomeSourceAggricultureByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScAggriculture();
            dt = OMem.GetPDIncomeSourceAggricultureByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtQAAggriTotalInc.Text = dt.Rows[0]["YearlyIncome"].ToString();
                ddlQAAggriIncomeFreq.SelectedIndex = ddlQAAggriIncomeFreq.Items.IndexOf(ddlQAAggriIncomeFreq.Items.FindByValue(Convert.ToString(dt.Rows[0]["IncomeFrequency"])));
                ddlQAAggriLandArea.SelectedIndex = ddlQAAggriLandArea.Items.IndexOf(ddlQAAggriLandArea.Items.FindByValue(Convert.ToString(dt.Rows[0]["AreaOfLand"])));
                ddlQAAggriSelfFarm.SelectedIndex = ddlQAAggriSelfFarm.Items.IndexOf(ddlQAAggriSelfFarm.Items.FindByValue(Convert.ToString(dt.Rows[0]["SelfFarmingYN"])));
                ddlQAAggriLeased.SelectedIndex = ddlQAAggriLeased.Items.IndexOf(ddlQAAggriLeased.Items.FindByValue(Convert.ToString(dt.Rows[0]["LeasedYN"])));
                ddlQAAggriCrops.SelectedIndex = ddlQAAggriCrops.Items.IndexOf(ddlQAAggriCrops.Items.FindByValue(Convert.ToString(dt.Rows[0]["Typeofcrops"])));
            }
        }
        protected void GetPDBusinessReference1ByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDBusinessReference1();
            dt = OMem.GetPDBusinessReference1ByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtRPO1Name.Text = dt.Rows[0]["Name"].ToString();
                txtRPO1ContactNo.Text = dt.Rows[0]["ContactNo"].ToString();
                ddlRPO1Relation.SelectedIndex = ddlRPO1Relation.Items.IndexOf(ddlRPO1Relation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Relation"])));
                txtRPO1Residance.Text = dt.Rows[0]["Residence"].ToString();
                ddlRPO1Occupation.SelectedIndex = ddlRPO1Occupation.Items.IndexOf(ddlRPO1Occupation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Occupation"])));
                ddlRPO1NoofYears.SelectedIndex = ddlRPO1NoofYears.Items.IndexOf(ddlRPO1NoofYears.Items.FindByValue(Convert.ToString(dt.Rows[0]["KnowHimYears"])));
                txtRPO1AppPayIssue.Text = dt.Rows[0]["PaymentIssue"].ToString();
                txtRPO1OfficePlace.Text = dt.Rows[0]["PlaceOfOffice"].ToString();
                txtRPO1AppSuppBuyer.Text = dt.Rows[0]["Supplier"].ToString();
            }
        }
        protected void GetPDBusinessReference2ByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDBusinessReference2();
            dt = OMem.GetPDBusinessReference2ByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtRPO2Name.Text = dt.Rows[0]["Name"].ToString();
                txtRPO2ContactNo.Text = dt.Rows[0]["ContactNo"].ToString();
                ddlRPO2Relation.SelectedIndex = ddlRPO2Relation.Items.IndexOf(ddlRPO2Relation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Relation"])));
                txtRPO2Residance.Text = dt.Rows[0]["Residence"].ToString();
                ddlRPO2Occupation.SelectedIndex = ddlRPO2Occupation.Items.IndexOf(ddlRPO2Occupation.Items.FindByValue(Convert.ToString(dt.Rows[0]["Occupation"])));
                ddlRPO2NoofYears.SelectedIndex = ddlRPO2NoofYears.Items.IndexOf(ddlRPO2NoofYears.Items.FindByValue(Convert.ToString(dt.Rows[0]["KnowHimYears"])));
                txtRPO2AppPayIssue.Text = dt.Rows[0]["PaymentIssue"].ToString();
                txtRPO2OfficePlace.Text = dt.Rows[0]["PlaceOfOffice"].ToString();
                txtRPO2AppSuppBuyer.Text = dt.Rows[0]["Supplier"].ToString();
            }
        }
        protected void GetPDBankBehaviourByLnAppId(string pLoanAppId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDBankBehaviour();
            dt = OMem.GetPDBankBehaviourByLnAppId(pLoanAppId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtQABankAccNo.Text = dt.Rows[0]["AccNo"].ToString();
                txtQABankAccType.Text = dt.Rows[0]["AccType"].ToString();
                txtQACurrentBal.Text = dt.Rows[0]["CurrentBal"].ToString();
                txtQABalanceMonth1.Text = dt.Rows[0]["Month1Bal"].ToString();
                txtQABalanceMonth2.Text = dt.Rows[0]["Month2Bal"].ToString();
                txtQABalanceMonth3.Text = dt.Rows[0]["Month3Bal"].ToString();
                txtQATranNo1.Text = dt.Rows[0]["Month1Tran"].ToString();
                txtQATranNo2.Text = dt.Rows[0]["Month2Tran"].ToString();
                txtQATranNo3.Text = dt.Rows[0]["Month3Tran"].ToString();
                txtQAMinChrgLst3Month.Text = dt.Rows[0]["MinBal3Months"].ToString();
                txtQAChqueReturnLst3Mnth.Text = dt.Rows[0]["ChqueReturns3Months"].ToString();
            }
        }
        protected void GetPDIncomeSourceBusinessByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScBusiness();
            dt = OMem.GetPDIncomeSourceBusinessByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {
                
                txtGrossSalesBus.Text = dt.Rows[0]["GrossSales"].ToString();
                txtGrossSalesBusObj.Text = dt.Rows[0]["GrossSalesObj"].ToString();
                txtGrossSalesBusVer.Text = dt.Rows[0]["GrossSalesVer"].ToString();
                txtGrossSalesBusRemark.Text = dt.Rows[0]["GrossSalesRemark"].ToString();

                txtGrossMarginBus.Text = dt.Rows[0]["GrossMargin"].ToString();
                txtGrossMarginBusObj.Text = dt.Rows[0]["GrossMarginObj"].ToString();
                txtGrossMarginBusVer.Text = dt.Rows[0]["GrossMarginVer"].ToString();
                txtGrossMarginBusRemark.Text = dt.Rows[0]["GrossMarginRemark"].ToString();

                txtTotIncBus.Text = dt.Rows[0]["TotalIncome"].ToString();
                txtTotIncBusObj.Text = dt.Rows[0]["TotalIncomeObj"].ToString();
                txtTotIncBusVer.Text = dt.Rows[0]["TotalIncomeVer"].ToString();
                txtTotIncBusRemark.Text = dt.Rows[0]["TotalIncomeRemark"].ToString();

                txtQABN.Text = dt.Rows[0]["BusName"].ToString();
                txtQABNObj.Text = dt.Rows[0]["BusNameObj"].ToString();
                txtQABNVer.Text = dt.Rows[0]["BusNameVer"].ToString();
                txtQABNRemark.Text = dt.Rows[0]["BusNameRemark"].ToString();

                ddlQABT.SelectedIndex = ddlQABT.Items.IndexOf(ddlQABT.Items.FindByValue(Convert.ToString(dt.Rows[0]["BusTypeId"])));
                txtQABTObj.Text = dt.Rows[0]["BusTypeObj"].ToString();
                txtQABTVer.Text = dt.Rows[0]["BusTypeVer"].ToString();
                txtQABTRemark.Text = dt.Rows[0]["BusTypeRemark"].ToString();


                ddlQABS.SelectedIndex = ddlQABS.Items.IndexOf(ddlQABS.Items.FindByValue(Convert.ToString(dt.Rows[0]["BusStabId"])));
                txtQABSObj.Text = dt.Rows[0]["BusStabObj"].ToString();
                txtQABSVer.Text = dt.Rows[0]["BusStabVer"].ToString();
                txtQABSRemark.Text = dt.Rows[0]["BusStabRemark"].ToString();

                txtQABA.Text = dt.Rows[0]["BusAddress"].ToString();
                txtQABAObj.Text = dt.Rows[0]["BusAddressObj"].ToString();
                txtQABAVer.Text = dt.Rows[0]["BusAddressVer"].ToString();
                txtQABARemark.Text = dt.Rows[0]["BusAddressRemark"].ToString();


                txtQANA.Text = dt.Rows[0]["NoOfEmp"].ToString();
                txtQANAObj.Text = dt.Rows[0]["NoOfEmpObj"].ToString();
                txtQANAVer.Text = dt.Rows[0]["NoOfEmpVer"].ToString();
                txtQANARemark.Text = dt.Rows[0]["NoOfEmpRemark"].ToString();

                ddlQASS.SelectedIndex = ddlQASS.Items.IndexOf(ddlQASS.Items.FindByValue(Convert.ToString(dt.Rows[0]["StockSeen"])));
                txtQASSObj.Text = dt.Rows[0]["StockSeenObj"].ToString();
                txtQASSVer.Text = dt.Rows[0]["StockSeenVer"].ToString();
                txtQASSRemark.Text = dt.Rows[0]["StockSeenRemark"].ToString();

                txtQAVN1.Text = dt.Rows[0]["VendNm1"].ToString();
                txtQAVN1Obj.Text = dt.Rows[0]["VendNm1Obj"].ToString();
                txtQAVN1Ver.Text = dt.Rows[0]["VendNm1Ver"].ToString();
                txtQAVN1Remark.Text = dt.Rows[0]["VendNm1Remark"].ToString();

                txtQAVed1Mob.Text = dt.Rows[0]["MobNo1"].ToString();
                txtQAVed1MobObj.Text = dt.Rows[0]["MobNo1Obj"].ToString();
                txtQAVed1MobVer.Text = dt.Rows[0]["MobNo1Ver"].ToString();
                txtQAVed1MobRemark.Text = dt.Rows[0]["MobNo1Remark"].ToString();

                txtQAVN2.Text = dt.Rows[0]["VendNm2"].ToString();
                txtQAVN2Obj.Text = dt.Rows[0]["VendNm2Obj"].ToString();
                txtQAVN2Ver.Text = dt.Rows[0]["VendNm2Ver"].ToString();
                txtQAVN2Remark.Text = dt.Rows[0]["VendNm2Remark"].ToString();

                txtQAVN2Mob.Text = dt.Rows[0]["MobNo2"].ToString();
                txtQAVN2MobObj.Text = dt.Rows[0]["MobNo2Obj"].ToString();
                txtQAVN2MobVer.Text = dt.Rows[0]["MobNo2Ver"].ToString();
                txtQAVN2MobRemark.Text = dt.Rows[0]["MobNo2Remark"].ToString();

                txtQABAN.Text = dt.Rows[0]["AppName"].ToString();
                txtQABANObj.Text = dt.Rows[0]["AppNameObj"].ToString();
                txtQABANVer.Text = dt.Rows[0]["AppNameVer"].ToString();
                txtQABANRemark.Text = dt.Rows[0]["AppNameRemark"].ToString();

                txtQAMCCustNo.Text = dt.Rows[0]["NoOfCust"].ToString();
                txtQAMCCustNoObj.Text = dt.Rows[0]["NoOfCustObj"].ToString();
                txtQAMCCustNoVer.Text = dt.Rows[0]["NoOfCustVer"].ToString();
                txtQAMCCustNoRemark.Text = dt.Rows[0]["NoOfCustRemark"].ToString();

                txtQAMCAmt.Text = dt.Rows[0]["CreditAmt"].ToString();
                txtQAMCAmtObj.Text = dt.Rows[0]["CreditAmtObj"].ToString();
                txtQAMCAmtVer.Text = dt.Rows[0]["CreditAmtVer"].ToString();
                txtQAMCAmtRemark.Text = dt.Rows[0]["CreditAmtRemark"].ToString();

                ddlQANB.SelectedIndex = ddlQANB.Items.IndexOf(ddlQANB.Items.FindByValue(Convert.ToString(dt.Rows[0]["NmBoardSeen"])));
                txtQANBObj.Text = dt.Rows[0]["NmBoardSeenObj"].ToString();
                txtQANBVer.Text = dt.Rows[0]["NmBoardSeenVer"].ToString();
                txtQANBRemark.Text = dt.Rows[0]["NmBoardSeenRemark"].ToString();
            }
            else
            {
                ClearPDInScBusiness();
            }
        }
        protected void GetPDIncomeSourceSalaryByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScSalary();
            dt = OMem.GetPDIncomeSourceSalaryByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtQAEmpNm.Text = dt.Rows[0]["EmpName"].ToString();
                txtQAEmpNmObj.Text = dt.Rows[0]["EmpNameObj"].ToString();
                txtQAEmpNmVer.Text = dt.Rows[0]["EmpNameVer"].ToString();
                txtQAEmpNmRemark.Text = dt.Rows[0]["EmpNameRemark"].ToString();

                txtQADes.Text = dt.Rows[0]["Desig"].ToString();
                txtQADesObj.Text = dt.Rows[0]["DesigObj"].ToString();
                txtQADesVer.Text = dt.Rows[0]["DesigVer"].ToString();
                txtQADesRemark.Text = dt.Rows[0]["DesigRemark"].ToString();

                txtQADOJ.Text = dt.Rows[0]["DOJ"].ToString();
                txtQADOJObj.Text = dt.Rows[0]["DOJObj"].ToString();
                txtQADOJVer.Text = dt.Rows[0]["DOJVer"].ToString();
                txtQADOJRemark.Text = dt.Rows[0]["DOJRemark"].ToString();

                txtQAJob.Text = dt.Rows[0]["JobStb"].ToString();
                txtQAJobObj.Text = dt.Rows[0]["JobStbObj"].ToString();
                txtQAJobVer.Text = dt.Rows[0]["JobStbVer"].ToString();
                txtQAJobRemark.Text = dt.Rows[0]["JobStbRemark"].ToString();

                txtQAEmpAdd.Text = dt.Rows[0]["EmpAddress"].ToString();
                txtQAEmpAddObj.Text = dt.Rows[0]["EmpAddressObj"].ToString();
                txtQAEmpAddVer.Text = dt.Rows[0]["EmpAddressVer"].ToString();
                txtQAEmpAddRemark.Text = dt.Rows[0]["EmpAddressRemark"].ToString();

                ddlQASalMode.SelectedIndex = ddlQASalMode.Items.IndexOf(ddlQASalMode.Items.FindByValue(Convert.ToString(dt.Rows[0]["SalCrModeId"])));
                txtQASalModeObj.Text = dt.Rows[0]["SalCrModeObj"].ToString();
                txtQASalModeVer.Text = dt.Rows[0]["SalCrModeVer"].ToString();
                txtQASalModeRemark.Text = dt.Rows[0]["SalCrModeRemark"].ToString();

                ddlQASalType.SelectedIndex = ddlQASalType.Items.IndexOf(ddlQASalType.Items.FindByValue(Convert.ToString(dt.Rows[0]["SalTypeId"])));
                txtQASalTypeObj.Text = dt.Rows[0]["SalTypeObj"].ToString();
                txtQASalTypeVer.Text = dt.Rows[0]["SalTypeVer"].ToString();
                txtQASalTypeRemark.Text = dt.Rows[0]["SalTypeRemark"].ToString();

                txtQASalBNm.Text = dt.Rows[0]["BankName"].ToString();
                txtQASalBNmObj.Text = dt.Rows[0]["BankNameObj"].ToString();
                txtQASalBNmVer.Text = dt.Rows[0]["BankNameVer"].ToString();
                txtQASalBNmRemark.Text = dt.Rows[0]["BankNameRemark"].ToString();

                txtQANetSal.Text = dt.Rows[0]["NetSal"].ToString();
                txtQANetSalObj.Text = dt.Rows[0]["NetSalObj"].ToString();
                txtQANetSalVer.Text = dt.Rows[0]["NetSalVer"].ToString();
                txtQANetSalRemark.Text = dt.Rows[0]["NetSalRemark"].ToString();

                txtQAManNm.Text = dt.Rows[0]["HRName"].ToString();
                txtQAManNmObj.Text = dt.Rows[0]["HRNameObj"].ToString();
                txtQAManNmVer.Text = dt.Rows[0]["HRNameVer"].ToString();
                txtQAManNmRemark.Text = dt.Rows[0]["HRNameRemark"].ToString();

                txtQAManMob.Text = dt.Rows[0]["HRMobNo"].ToString();
                txtQAManMobObj.Text = dt.Rows[0]["HRMobNoObj"].ToString();
                txtQAManMobVer.Text = dt.Rows[0]["HRMobNoVer"].ToString();
                txtQAManMobRemark.Text = dt.Rows[0]["HRMobNoRemark"].ToString();

                txtQAMCNm.Text = dt.Rows[0]["CollName"].ToString();
                txtQAMCNmObj.Text = dt.Rows[0]["CollNameObj"].ToString();
                txtQAMCNmVer.Text = dt.Rows[0]["CollNameVer"].ToString();
                txtQAMCNmRemark.Text = dt.Rows[0]["CollNameRemark"].ToString();

                txtQAMCMob.Text = dt.Rows[0]["CollMobNo"].ToString();
                txtQAMCMobObj.Text = dt.Rows[0]["CollMobNoObj"].ToString();
                txtQAMCMobVer.Text = dt.Rows[0]["CollMobNoVer"].ToString();
                txtQAMCMobRemark.Text = dt.Rows[0]["CollMobNoRemark"].ToString();

                txtQAPreEmpNm.Text = dt.Rows[0]["PreEmpName"].ToString();
                txtQAPreEmpNmObj.Text = dt.Rows[0]["PreEmpNameObj"].ToString();
                txtQAPreEmpNmVer.Text = dt.Rows[0]["PreEmpNameVer"].ToString();
                txtQAPreEmpNmRemark.Text = dt.Rows[0]["PreEmpNameRemark"].ToString();

                txtQAPreEmpAd.Text = dt.Rows[0]["PreEmpAddress"].ToString();
                txtQAPreEmpAdObj.Text = dt.Rows[0]["PreEmpAddressObj"].ToString();
                txtQAPreEmpAdVer.Text = dt.Rows[0]["PreEmpAddressVer"].ToString();
                txtQAPreEmpAdRemark.Text = dt.Rows[0]["PreEmpAddressRemark"].ToString();

                txtQAPreEmpAd.Text = dt.Rows[0]["PreEmpAddress"].ToString();
                txtQAPreEmpAdObj.Text = dt.Rows[0]["PreEmpAddressObj"].ToString();
                txtQAPreEmpAdVer.Text = dt.Rows[0]["PreEmpAddressVer"].ToString();
                txtQAPreEmpAdRemark.Text = dt.Rows[0]["PreEmpAddressRemark"].ToString();

                txtQAPreEmpDOJ.Text = dt.Rows[0]["PreEmpDOJ"].ToString();
                txtQAPreEmpDOJObj.Text = dt.Rows[0]["PreEmpDOJObj"].ToString();
                txtQAPreEmpDOJVer.Text = dt.Rows[0]["PreEmpDOJVer"].ToString();
                txtQAPreEmpDOJRemark.Text = dt.Rows[0]["PreEmpDOJRemark"].ToString();

                txtQAPreEmpDOL.Text = dt.Rows[0]["PreEmpDOL"].ToString();
                txtQAPreEmpDOLObj.Text = dt.Rows[0]["PreEmpDOLObj"].ToString();
                txtQAPreEmpDOLVer.Text = dt.Rows[0]["PreEmpDOLVer"].ToString();
                txtQAPreEmpDOLRemark.Text = dt.Rows[0]["PreEmpDOLRemark"].ToString();

                txtQAPreEmpWorkExp.Text = dt.Rows[0]["PreWorkExp"].ToString();
                txtQAPreEmpWorkExpObj.Text = dt.Rows[0]["PreWorkExpObj"].ToString();
                txtQAPreEmpWorkExpVer.Text = dt.Rows[0]["PreWorkExpVer"].ToString();
                txtQAPreEmpWorkExpRemark.Text = dt.Rows[0]["PreWorkExpRemark"].ToString();

                txtQAPreTotWorkExp.Text = dt.Rows[0]["TotWorkExp"].ToString();
                txtQAPreTotWorkExpObj.Text = dt.Rows[0]["TotWorkExpObj"].ToString();
                txtQAPreTotWorkExpVer.Text = dt.Rows[0]["TotWorkExpVer"].ToString();
                txtQAPreTotWorkExpRemark.Text = dt.Rows[0]["TotWorkExpRemark"].ToString();


            }
            else
            {
                ClearPDInScSalary();
            }
        }
        protected void GetPDIncomeSourceRentalByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScRental();
            dt = OMem.GetPDIncomeSourceRentalByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtRentalAmt.Text = dt.Rows[0]["RentalAmt"].ToString();
                txtRentalAmtObj.Text = dt.Rows[0]["RentalAmtObj"].ToString();
                txtRentalAmtVer.Text = dt.Rows[0]["RentalAmtVer"].ToString();
                txtRentalAmtRemark.Text = dt.Rows[0]["RentalAmtRemark"].ToString();


                ddlQAPropType.SelectedIndex = ddlQAPropType.Items.IndexOf(ddlQAPropType.Items.FindByValue(Convert.ToString(dt.Rows[0]["PropType"])));
                txtQAPropTypeObj.Text = dt.Rows[0]["PropTypeObj"].ToString();
                txtQAPropTypeVer.Text = dt.Rows[0]["PropTypeVer"].ToString();
                txtQAPropTypeRemark.Text = dt.Rows[0]["PropTypeRemark"].ToString();

                txtQARenRoom.Text = dt.Rows[0]["NoOfRentRoom"].ToString();
                txtQARenRoomObj.Text = dt.Rows[0]["NoOfRentRoomObj"].ToString();
                txtQARenRoomVer.Text = dt.Rows[0]["NoOfRentRoomVer"].ToString();
                txtQARenRoomRemark.Text = dt.Rows[0]["NoOfRentRoomRemark"].ToString();

                txtQAProAge.Text = dt.Rows[0]["PropAge"].ToString();
                txtQAProAgeObj.Text = dt.Rows[0]["PropAgeObj"].ToString();
                txtQAProAgeVer.Text = dt.Rows[0]["PropAgeVer"].ToString();
                txtQAProAgeRemark.Text = dt.Rows[0]["PropAgeRemark"].ToString();

                txtQAPropAd.Text = dt.Rows[0]["PropAddress"].ToString();
                txtQAPropAdObj.Text = dt.Rows[0]["PropAddressObj"].ToString();
                txtQAPropAdVer.Text = dt.Rows[0]["PropAddressVer"].ToString();
                txtQAPropAdRemark.Text = dt.Rows[0]["PropAddressRemark"].ToString();

                txtQANOC.Text = dt.Rows[0]["TenantNOC"].ToString();
                txtQANOCObj.Text = dt.Rows[0]["TenantNOCObj"].ToString();
                txtQANOCVer.Text = dt.Rows[0]["TenantNOCVer"].ToString();
                txtQANOCRemark.Text = dt.Rows[0]["TenantNOCRemark"].ToString();

                txtQARent.Text = dt.Rows[0]["RentalDoc"].ToString();
                txtQARentObj.Text = dt.Rows[0]["RentalDocObj"].ToString();
                txtQARentVer.Text = dt.Rows[0]["RentalDocVer"].ToString();
                txtQARentRemark.Text = dt.Rows[0]["RentalDocRemark"].ToString();
            }
            else
            {
                ClearPDInScRental();
            }
        }
        protected void GetPDIncomeSourcePensionByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScPension();
            dt = OMem.GetPDIncomeSourcePensionByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {
                txtPenAmt.Text = dt.Rows[0]["PensionAmt"].ToString();
                txtPenAmtObj.Text = dt.Rows[0]["PensionAmtObj"].ToString();
                txtPenAmtVer.Text = dt.Rows[0]["PensionAmtVer"].ToString();
                txtPenAmtRemark.Text = dt.Rows[0]["PensionAmtRemark"].ToString();

                txtQAPenStDt.Text = dt.Rows[0]["PenStDate"].ToString();
                txtQAPenStDtObj.Text = dt.Rows[0]["PenStDateObj"].ToString();
                txtQAPenStDtVer.Text = dt.Rows[0]["PenStDateVer"].ToString();
                txtQAPenStDtRemark.Text = dt.Rows[0]["PenStDateRemark"].ToString();

                ddlQAPenOrder.SelectedIndex = ddlQAPenOrder.Items.IndexOf(ddlQAPenOrder.Items.FindByValue(Convert.ToString(dt.Rows[0]["PenOrder"])));
                txtQAPenOrderObj.Text = dt.Rows[0]["PenOrderObj"].ToString();
                txtQAPenOrderVer.Text = dt.Rows[0]["PenOrderVer"].ToString();
                txtQAPenOrderRemark.Text = dt.Rows[0]["PenOrderRemark"].ToString();

                txtQAPenBank.Text = dt.Rows[0]["BankName"].ToString();
                txtQAPenBankObj.Text = dt.Rows[0]["BankNameObj"].ToString();
                txtQAPenBankVer.Text = dt.Rows[0]["BankNameVer"].ToString();
                txtQAPenBankRemark.Text = dt.Rows[0]["BankNameRemark"].ToString();

                ddlQAPenInc.SelectedIndex = ddlQAPenInc.Items.IndexOf(ddlQAPenInc.Items.FindByValue(Convert.ToString(dt.Rows[0]["PenIncId"])));
                txtQAPenIncObj.Text = dt.Rows[0]["PenIncObj"].ToString();
                txtQAPenIncVer.Text = dt.Rows[0]["PenIncVer"].ToString();
                txtQAPenIncRemark.Text = dt.Rows[0]["PenIncRemark"].ToString();

                ddlQAPenOrder.SelectedIndex = ddlQAPenOrder.Items.IndexOf(ddlQAPenOrder.Items.FindByValue(Convert.ToString(dt.Rows[0]["PenFromId"])));
                txtQAPenFromObj.Text = dt.Rows[0]["PenFromObj"].ToString();
                txtQAPenFromVer.Text = dt.Rows[0]["PenFromVer"].ToString();
                txtQAPenFromRemark.Text = dt.Rows[0]["PenFromRemark"].ToString();
            }
            else
            {
                ClearPDInScPension();
            }
        }
        protected void GetPDIncomeSourceWagesByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDInScWages();
            dt = OMem.GetPDIncomeSourceWagesByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlQAWorkType.SelectedIndex = ddlQAWorkType.Items.IndexOf(ddlQAWorkType.Items.FindByValue(Convert.ToString(dt.Rows[0]["WorkType"])));
                txtQAWorkTypeObj.Text = dt.Rows[0]["WorkTypeObj"].ToString();
                txtQAWorkTypeVer.Text = dt.Rows[0]["WorkTypeVer"].ToString();
                txtQAWorkTypeRemark.Text = dt.Rows[0]["WorkTypeRemark"].ToString();

                txtQAWorkFrom.Text = dt.Rows[0]["WorkDoingFrom"].ToString();
                txtQAWorkFromObj.Text = dt.Rows[0]["WorkDoingFromObj"].ToString();
                txtQAWorkFromVer.Text = dt.Rows[0]["WorkDoingFromVer"].ToString();
                txtQAWorkFromRemark.Text = dt.Rows[0]["WorkDoingFromRemark"].ToString();

                txtQAWEmpNm.Text = dt.Rows[0]["EmpName"].ToString();
                txtQAWEmpNmObj.Text = dt.Rows[0]["EmpNameObj"].ToString();
                txtQAWEmpNmVer.Text = dt.Rows[0]["EmpNameVer"].ToString();
                txtQAWEmpNmRemark.Text = dt.Rows[0]["EmpNameRemark"].ToString();

                txtQAEmpContNo.Text = dt.Rows[0]["EmpContactNo"].ToString();
                txtQAEmpContNoObj.Text = dt.Rows[0]["EmpContactNoObj"].ToString();
                txtQAEmpContNoVer.Text = dt.Rows[0]["EmpContactNoVer"].ToString();
                txtQAEmpContNoRemark.Text = dt.Rows[0]["EmpContactNoRemark"].ToString();
            }
            else
            {
                ClearPDInScWages();
            }
        }
        protected void GetPDPropDetailsByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPDPropertyDtl();
            dt = OMem.GetPDPropDetailsByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlQAProType.SelectedIndex = ddlQAProType.Items.IndexOf(ddlQAProType.Items.FindByValue(Convert.ToString(dt.Rows[0]["PropTypeId"])));
                txtQAProTypeObj.Text = dt.Rows[0]["PropTypeObj"].ToString();
                txtQAProTypeVer.Text = dt.Rows[0]["PropTypeVer"].ToString();
                txtQAProTypeRemark.Text = dt.Rows[0]["PropTypeRemark"].ToString();

                ddlQAProNature.SelectedIndex = ddlQAProNature.Items.IndexOf(ddlQAProNature.Items.FindByValue(Convert.ToString(dt.Rows[0]["PropNaureId"])));
                txtQAProTypeObj.Text = dt.Rows[0]["PropNaureObj"].ToString();
                txtQAProTypeVer.Text = dt.Rows[0]["PropNaureVer"].ToString();
                txtQAProTypeRemark.Text = dt.Rows[0]["PropNaureRemark"].ToString();

                txtQAPropAdd.Text = dt.Rows[0]["PropAddress"].ToString();
                txtQAPropAddObj.Text = dt.Rows[0]["PropAddressObj"].ToString();
                txtQAPropAddVer.Text = dt.Rows[0]["PropAddressVer"].ToString();
                txtQAPropAddRemark.Text = dt.Rows[0]["PropAddressRemark"].ToString();

                ddlQAProJud.SelectedIndex = ddlQAProJud.Items.IndexOf(ddlQAProJud.Items.FindByValue(Convert.ToString(dt.Rows[0]["PropJudicialId"])));
                txtQAProJudObj.Text = dt.Rows[0]["PropJudicialObj"].ToString();
                txtQAProJudVer.Text = dt.Rows[0]["PropJudicialVer"].ToString();
                txtQAProJudRemark.Text = dt.Rows[0]["PropJudicialRemark"].ToString();
            }
            else
            {
                ClearPDPropertyDtl();
            }
        }
        protected void GetPDLoanRequireDetailByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearLoanRequireDtl();
            dt = OMem.GetPDLoanRequireDetailByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlQALnPurpose.SelectedIndex = ddlQALnPurpose.Items.IndexOf(ddlQALnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["LnPurpose"])));
                txtQALnPurposeObj.Text = dt.Rows[0]["LnPurposeObj"].ToString();
                txtQALnPurposeVer.Text = dt.Rows[0]["LnPurposeVer"].ToString();
                txtQALnPurposeRemark.Text = dt.Rows[0]["LnPurposeRemark"].ToString();

                txtQALnAmt.Text = dt.Rows[0]["ExpLnAmt"].ToString();
                txtQALnAmtObj.Text = dt.Rows[0]["ExpLnAmtObj"].ToString();
                txtQALnAmtVer.Text = dt.Rows[0]["ExpLnAmtVer"].ToString();
                txtQALnAmtRemark.Text = dt.Rows[0]["ExpLnAmtRemark"].ToString();

                txtQALnTenure.Text = dt.Rows[0]["ExpLnTenore"].ToString();
                txtQALnTenureObj.Text = dt.Rows[0]["ExpLnTenoreObj"].ToString();
                txtQALnTenureVer.Text = dt.Rows[0]["ExpLnTenoreVer"].ToString();
                txtQALnTenureRemark.Text = dt.Rows[0]["ExpLnTenoreRemark"].ToString();

                txtQAEMICap.Text = dt.Rows[0]["EMICap"].ToString();
                txtQAEMICapObj.Text = dt.Rows[0]["EMICapObj"].ToString();
                txtQAEMICapVer.Text = dt.Rows[0]["EMICapVer"].ToString();
                txtQAEMICapRemark.Text = dt.Rows[0]["EMICapRemark"].ToString();

                txtQALnOutSt.Text = dt.Rows[0]["TotLnOutstanding"].ToString();
                txtQALnOutStObj.Text = dt.Rows[0]["TotLnOutstandingObj"].ToString();
                txtQALnOutStVer.Text = dt.Rows[0]["TotLnOutstandingVer"].ToString();
                txtQALnOutStRemark.Text = dt.Rows[0]["TotLnOutstandingRemark"].ToString();
            }
            else
            {
                ClearLoanRequireDtl();
            }
        }
        protected void GetPDSocialBehabiourByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearSocialBehaviour();
            dt = OMem.GetPDSocialBehabiourByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                ddlQADrink.SelectedIndex = ddlQADrink.Items.IndexOf(ddlQADrink.Items.FindByValue(Convert.ToString(dt.Rows[0]["DrinkingHabit"])));
                txtQADrinkObj.Text = dt.Rows[0]["DrinkingHabitObj"].ToString();
                txtQADrinkVer.Text = dt.Rows[0]["DrinkingHabitVer"].ToString();
                txtQADrinkRemark.Text = dt.Rows[0]["DrinkingHabitRemark"].ToString();

                ddlQASocAct.SelectedIndex = ddlQASocAct.Items.IndexOf(ddlQASocAct.Items.FindByValue(Convert.ToString(dt.Rows[0]["SocialAct"])));
                txtQASocActObj.Text = dt.Rows[0]["SocialActObj"].ToString();
                txtQASocActVer.Text = dt.Rows[0]["SocialActVer"].ToString();
                txtQASocActRemark.Text = dt.Rows[0]["SocialActRemark"].ToString();

                txtQAComm.Text = dt.Rows[0]["InvAnyCommitment"].ToString();
                txtQACommObj.Text = dt.Rows[0]["InvAnyCommitmentObj"].ToString();
                txtQACommVer.Text = dt.Rows[0]["InvAnyCommitmentVer"].ToString();
                txtQACommRemark.Text = dt.Rows[0]["InvAnyCommitmentRemark"].ToString();
            }
            else
            {
                ClearSocialBehaviour();
            }
        }
        protected void GetPDNeighbourReferenceCheckByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearNeighbourReference();
            dt = OMem.GetPDNeighbourReferenceCheckByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                txtQANeiNm1.Text = dt.Rows[0]["NeibourNm1"].ToString();
                txtQANeiNm1Obj.Text = dt.Rows[0]["NeibourNm1Obj"].ToString();
                txtQANeiNm1Ver.Text = dt.Rows[0]["NeibourNm1Ver"].ToString();
                txtQANeiNm1Remark.Text = dt.Rows[0]["NeibourNm1Remark"].ToString();

                txtQANeiMob1.Text = dt.Rows[0]["NeibourMob1"].ToString();
                txtQANeiMob1Obj.Text = dt.Rows[0]["NeibourMob1Obj"].ToString();
                txtQANeiMob1Ver.Text = dt.Rows[0]["NeibourMob1Ver"].ToString();
                txtQANeiMob1Remark.Text = dt.Rows[0]["NeibourMob1Remark"].ToString();

                txtQAFeedback1.Text = dt.Rows[0]["FeedBack1"].ToString();
                txtQAFeedback1Obj.Text = dt.Rows[0]["FeedBack1Obj"].ToString();
                txtQAFeedback1Ver.Text = dt.Rows[0]["FeedBack1Ver"].ToString();
                txtQAFeedback1Remark.Text = dt.Rows[0]["FeedBack1Remark"].ToString();

                txtQANeiNm2.Text = dt.Rows[0]["NeibourNm2"].ToString();
                txtQANeiNm2Obj.Text = dt.Rows[0]["NeibourNm2Obj"].ToString();
                txtQANeiNm2Ver.Text = dt.Rows[0]["NeibourNm2Ver"].ToString();
                txtQANeiNm2Remark.Text = dt.Rows[0]["NeibourNm2Remark"].ToString();

                txtQANeiMob2.Text = dt.Rows[0]["NeibourMob2"].ToString();
                txtQANeiMob2Obj.Text = dt.Rows[0]["NeibourMob2Obj"].ToString();
                txtQANeiMob2Ver.Text = dt.Rows[0]["NeibourMob2Ver"].ToString();
                txtQANeiMob2Remark.Text = dt.Rows[0]["NeibourMob2Remark"].ToString();

                txtQAFeedback2.Text = dt.Rows[0]["FeedBack2"].ToString();
                txtQAFeedback2Obj.Text = dt.Rows[0]["FeedBack2Obj"].ToString();
                txtQAFeedback2Ver.Text = dt.Rows[0]["FeedBack2Ver"].ToString();
                txtQAFeedback2Remark.Text = dt.Rows[0]["FeedBack2Remark"].ToString();
            }
            else
            {
                ClearNeighbourReference();
            }
        }
        protected void GetPDPositiveNegativeObservationByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearPosNegFeedBack();
            dt = OMem.GetPDPositiveNegativeObservationByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                txtQAPos.Text = dt.Rows[0]["Positive"].ToString();
                txtQAPosObj.Text = dt.Rows[0]["PositiveObj"].ToString();
                txtQAPosVer.Text = dt.Rows[0]["PositiveVer"].ToString();
                txtQAPosRemark.Text = dt.Rows[0]["PositiveRemark"].ToString();

                txtQANeg.Text = dt.Rows[0]["Negative"].ToString();
                txtQANegObj.Text = dt.Rows[0]["NegativeObj"].ToString();
                txtQANegVer.Text = dt.Rows[0]["NegativeVer"].ToString();
                txtQANegRemark.Text = dt.Rows[0]["NegativeRemark"].ToString();
            }
            else
            {
                ClearPosNegFeedBack();
            }
        }
        protected void GetPDInvestmentSavingByLnAppId(string pLoanAppId, string pPDDoneBy, string pCustId, string pCustType)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            ClearSaving();
            dt = OMem.GetPDInvestmentSavingByLnAppId(pLoanAppId, pPDDoneBy, pCustId, pCustType);
            if (dt.Rows.Count > 0)
            {

                txtQAGold.Text = dt.Rows[0]["Gold"].ToString();
                txtQAGoldObj.Text = dt.Rows[0]["GoldObj"].ToString();
                txtQAGoldVer.Text = dt.Rows[0]["GoldVer"].ToString();
                txtQAGoldRemark.Text = dt.Rows[0]["GoldRemark"].ToString();

                txtQABondPaper.Text = dt.Rows[0]["BondPaper"].ToString();
                txtQABondPaperObj.Text = dt.Rows[0]["BondPaperObj"].ToString();
                txtQABondPaperVer.Text = dt.Rows[0]["BondPaperVer"].ToString();
                txtQABondPaperRemark.Text = dt.Rows[0]["BondPaperRemark"].ToString();

                txtQASaving.Text = dt.Rows[0]["DailySaving"].ToString();
                txtQASavingObj.Text = dt.Rows[0]["DailySavingObj"].ToString();
                txtQASavingVer.Text = dt.Rows[0]["DailySavingVer"].ToString();
                txtQASavingRemark.Text = dt.Rows[0]["DailySavingRemark"].ToString();

                txtQATermInsu.Text = dt.Rows[0]["InsuPolicy"].ToString();
                txtQATermInsuObj.Text = dt.Rows[0]["InsuPolicyObj"].ToString();
                txtQATermInsuVer.Text = dt.Rows[0]["InsuPolicyVer"].ToString();
                txtQATermInsuRemark.Text = dt.Rows[0]["InsuPolicyRemark"].ToString();

                txtQAFixedDepo.Text = dt.Rows[0]["FixedDeposit"].ToString();
                txtQAFixedDepoObj.Text = dt.Rows[0]["FixedDepositObj"].ToString();
                txtQAFixedDepoVer.Text = dt.Rows[0]["FixedDepositVer"].ToString();
                txtQAFixedDepoRemark.Text = dt.Rows[0]["FixedDepositRemark"].ToString();

                txtQAPlots.Text = dt.Rows[0]["AggriLand"].ToString();
                txtQAPlotsObj.Text = dt.Rows[0]["AggriLandObj"].ToString();
                txtQAPlotsVer.Text = dt.Rows[0]["AggriLandVer"].ToString();
                txtQAPlotsRemark.Text = dt.Rows[0]["AggriLandRemark"].ToString();
            }
            else
            {
                ClearSaving();
            }
        }
        private void ClearPersonalDt()
        {
            txtQAAppname.Text = "";
            txtQAAppnameObj.Text = "";
            txtQAAppnameVer.Text = "";
            txtQAAppnameRemark.Text = "";

            txtQAAppRelationObj.Text = "";
            txtQAAppRelationVer.Text = "";
            txtQAAppRelationRemark.Text = "";

            txtQAAppDOB.Text = "";
            txtQAAppDOBObj.Text = "";
            txtQAAppDOBVer.Text = "";
            txtQAAppDOBRemark.Text = "";

            txtQAAppAge.Text = "";
            txtQAAppAgeObj.Text = "";
            txtQAAppAgeVer.Text = "";
            txtQAAppAgeRemark.Text = "";

            txtQAMStatusObj.Text = "";
            txtQAMStatusVer.Text = "";
            txtQAMStatusRemark.Text = "";


            txtQAChildObj.Text = "";
            txtQAChildVer.Text = "";
            txtQAChildRemark.Text = "";


            txtQAEMObj.Text = "";
            txtQAEMVer.Text = "";
            txtQAEMRemark.Text = "";


            txtQAPOObj.Text = "";
            txtQAPOVer.Text = "";
            txtQAPORemark.Text = "";


            txtQAOTObj.Text = "";
            txtQAOTVer.Text = "";
            txtQAOTRemark.Text = "";

            txtQAHO.Text = "";
            txtQAHOObj.Text = "";
            txtQAHOVer.Text = "";
            txtQAHORemark.Text = "";

            txtQAPA.Text = "";
            txtQAPAObj.Text = "";
            txtQAPAVer.Text = "";
            txtQAPARemark.Text = "";

            //txtQARN.Text = "";
            //txtQARNObj.Text = "";
            //txtQARNVer.Text = "";
            //txtQARNRemark.Text = "";

            txtQAMob.Text = "";
            txtQAMobObj.Text = "";
            txtQAMobVer.Text = "";
            txtQAMobRemark.Text = "";

            //txtQALeg.Text = "";
            //txtQALegObj.Text = "";
            //txtQALegVer.Text = "";
            //txtQALegRemark.Text = "";

            //txtQAPol.Text = "";
            //txtQAPolObj.Text = "";
            //txtQAPolVer.Text = "";
            //txtQAPolRemark.Text = "";

            //txtQAPolSt1.Text = "";
            //txtQAPolSt1Obj.Text = "";
            //txtQAPolSt1Ver.Text = "";
            //txtQAPolSt1Remark.Text = "";
        }

        private void ClearPersonalReference()
        {
            txtRPHName.Text = "";
            txtRPHContactNo.Text = "";
            ddlRPHRelation.SelectedIndex = -1;
            txtRPHResidance.Text = "";
            ddlRPHOccupation.SelectedIndex = -1;
            ddlRPHNoofYears.SelectedIndex = -1;
        }
        private void ClearApplicantProfile()
        {
            ddlAppCoOper.SelectedIndex = -1;
            ddlAppAccuInfo.SelectedIndex = -1;
            ddlAppBusiKnow.SelectedIndex = -1;
            ddlAppHouseHold.SelectedIndex = -1;
            ddlAppSavingCapacity.SelectedIndex = -1;
            ddlAppQualityInventory.SelectedIndex = -1;
            ddlAppPhyFitness.SelectedIndex = -1;
            ddlAppFamilySuppot.SelectedIndex = -1;
        }
        private void ClearCoApplicantProfile()
        {
            ddlCoAppCoOper.SelectedIndex = -1;
            ddlCoAppAccuInfo.SelectedIndex = -1;
            ddlCoAppBusiKnow.SelectedIndex = -1;
            ddlCoAppHouseHold.SelectedIndex = -1;
            ddlCoAppSavingCapacity.SelectedIndex = -1;
            ddlCoAppQualityInventory.SelectedIndex = -1;
            ddlCoAppPhyFitness.SelectedIndex = -1;
            ddlCoAppFamilySuppot.SelectedIndex = -1;
        }
        private void ClearPDInScAggriculture()
        {
            txtQAAggriTotalInc.Text = "";
            ddlQAAggriIncomeFreq.SelectedIndex = -1;
            ddlQAAggriLandArea.SelectedIndex = -1;
            ddlQAAggriSelfFarm.SelectedIndex = -1;
            ddlQAAggriLeased.SelectedIndex = -1;
            ddlQAAggriCrops.SelectedIndex = -1;
        }
        private void ClearPDBusinessReference1()
        {
            txtRPO1Name.Text = "";
            txtRPO1ContactNo.Text = "";
            ddlRPO1Relation.SelectedIndex = -1;
            txtRPO1Residance.Text = "";
            ddlRPO1Occupation.SelectedIndex = -1;
            ddlRPO1NoofYears.SelectedIndex = -1;
            txtRPO1AppPayIssue.Text = "";
            txtRPO1OfficePlace.Text = "";
            txtRPO1AppSuppBuyer.Text = "";
        }
        private void ClearPDBusinessReference2()
        {
            txtRPO2Name.Text = "";
            txtRPO2ContactNo.Text = "";
            ddlRPO2Relation.SelectedIndex = -1;
            txtRPO2Residance.Text = "";
            ddlRPO2Occupation.SelectedIndex = -1;
            ddlRPO2NoofYears.SelectedIndex = -1;
            txtRPO2AppPayIssue.Text = "";
            txtRPO2OfficePlace.Text = "";
            txtRPO2AppSuppBuyer.Text = "";
        }
        private void ClearPDBankBehaviour()
        {
            txtQABankAccNo.Text = "";
            txtQABankAccType.Text = "";
            txtQACurrentBal.Text = "";
            txtQABalanceMonth1.Text = "";
            txtQABalanceMonth2.Text = "";
            txtQABalanceMonth3.Text = "";
            txtQATranNo1.Text = "";
            txtQATranNo2.Text = "";
            txtQATranNo3.Text = "";
            txtQAMinChrgLst3Month.Text = "";
            txtQAChqueReturnLst3Mnth.Text = "";
        }

        private void ClearPDInScBusiness()
        {
            txtTotIncBus.Text = "0";
            txtTotIncBusObj.Text = "";
            txtTotIncBusVer.Text = "";
            txtTotIncBusRemark.Text = "";

            txtQABN.Text = "";
            txtQABNObj.Text = "";
            txtQABNVer.Text = "";
            txtQABNRemark.Text = "";

            ddlQABT.SelectedIndex = -1;
            txtQABTObj.Text = "";
            txtQABTVer.Text = "";
            txtQABTRemark.Text = "";


            ddlQABS.SelectedIndex = -1;
            txtQABSObj.Text = "";
            txtQABSVer.Text = "";
            txtQABSRemark.Text = "";

            txtQABA.Text = "";
            txtQABAObj.Text = "";
            txtQABAVer.Text = "";
            txtQABARemark.Text = "";


            txtQANA.Text = "";
            txtQANAObj.Text = "";
            txtQANAVer.Text = "";
            txtQANARemark.Text = "";

            ddlQASS.SelectedIndex = -1;
            txtQASSObj.Text = "";
            txtQASSVer.Text = "";
            txtQASSRemark.Text = "";

            txtQAVN1.Text = "";
            txtQAVN1Obj.Text = "";
            txtQAVN1Ver.Text = "";
            txtQAVN1Remark.Text = "";

            txtQAVed1Mob.Text = "";
            txtQAVed1MobObj.Text = "";
            txtQAVed1MobVer.Text = "";
            txtQAVed1MobRemark.Text = "";

            txtQAVN2.Text = "";
            txtQAVN2Obj.Text = "";
            txtQAVN2Ver.Text = "";
            txtQAVN2Remark.Text = "";

            txtQAVN2Mob.Text = "";
            txtQAVN2MobObj.Text = "";
            txtQAVN2MobVer.Text = "";
            txtQAVN2MobRemark.Text = "";

            txtQABAN.Text = "";
            txtQABANObj.Text = "";
            txtQABANVer.Text = "";
            txtQABANRemark.Text = "";

            txtQAMCCustNo.Text = "";
            txtQAMCCustNoObj.Text = "";
            txtQAMCCustNoVer.Text = "";
            txtQAMCCustNoRemark.Text = "";

            txtQAMCAmt.Text = "";
            txtQAMCAmtObj.Text = "";
            txtQAMCAmtVer.Text = "";
            txtQAMCAmtRemark.Text = "";

            ddlQANB.SelectedIndex = -1;
            txtQANBObj.Text = "";
            txtQANBVer.Text = "";
            txtQANBRemark.Text = "";
        }
        private void ClearPDInScSalary()
        {
            txtQAEmpNm.Text = "";
            txtQAEmpNmObj.Text = "";
            txtQAEmpNmVer.Text = "";
            txtQAEmpNmRemark.Text = "";

            txtQADes.Text = "";
            txtQADesObj.Text = "";
            txtQADesVer.Text = "";
            txtQADesRemark.Text = "";

            txtQADOJ.Text = "";
            txtQADOJObj.Text = "";
            txtQADOJVer.Text = "";
            txtQADOJRemark.Text = "";

            txtQAJob.Text = "";
            txtQAJobObj.Text = "";
            txtQAJobVer.Text = "";
            txtQAJobRemark.Text = "";

            txtQAEmpAdd.Text = "";
            txtQAEmpAddObj.Text = "";
            txtQAEmpAddVer.Text = "";
            txtQAEmpAddRemark.Text = "";

            txtQASalModeObj.Text = "";
            txtQASalModeVer.Text = "";
            txtQASalModeRemark.Text = "";


            txtQASalTypeObj.Text = "";
            txtQASalTypeVer.Text = "";
            txtQASalTypeRemark.Text = "";

            txtQASalBNm.Text = "";
            txtQASalBNmObj.Text = "";
            txtQASalBNmVer.Text = "";
            txtQASalBNmRemark.Text = "";

            txtQANetSal.Text = "";
            txtQANetSalObj.Text = "";
            txtQANetSalVer.Text = "";
            txtQANetSalRemark.Text = "";

            txtQAManNm.Text = "";
            txtQAManNmObj.Text = "";
            txtQAManNmVer.Text = "";
            txtQAManNmRemark.Text = "";

            txtQAManMob.Text = "";
            txtQAManMobObj.Text = "";
            txtQAManMobVer.Text = "";
            txtQAManMobRemark.Text = "";

            txtQAMCNm.Text = "";
            txtQAMCNmObj.Text = "";
            txtQAMCNmVer.Text = "";
            txtQAMCNmRemark.Text = "";

            txtQAMCMob.Text = "";
            txtQAMCMobObj.Text = "";
            txtQAMCMobVer.Text = "";
            txtQAMCMobRemark.Text = "";

            txtQAPreEmpNm.Text = "";
            txtQAPreEmpNmObj.Text = "";
            txtQAPreEmpNmVer.Text = "";
            txtQAPreEmpNmRemark.Text = "";

            txtQAPreEmpAd.Text = "";
            txtQAPreEmpAdObj.Text = "";
            txtQAPreEmpAdVer.Text = "";
            txtQAPreEmpAdRemark.Text = "";

            txtQAPreEmpAd.Text = "";
            txtQAPreEmpAdObj.Text = "";
            txtQAPreEmpAdVer.Text = "";
            txtQAPreEmpAdRemark.Text = "";

            txtQAPreEmpDOJ.Text = "";
            txtQAPreEmpDOJObj.Text = "";
            txtQAPreEmpDOJVer.Text = "";
            txtQAPreEmpDOJRemark.Text = "";

            txtQAPreEmpDOL.Text = "";
            txtQAPreEmpDOLObj.Text = "";
            txtQAPreEmpDOLVer.Text = "";
            txtQAPreEmpDOLRemark.Text = "";

            txtQAPreEmpWorkExp.Text = "";
            txtQAPreEmpWorkExpObj.Text = "";
            txtQAPreEmpWorkExpVer.Text = "";
            txtQAPreEmpWorkExpRemark.Text = "";

            txtQAPreTotWorkExp.Text = "";
            txtQAPreTotWorkExpObj.Text = "";
            txtQAPreTotWorkExpVer.Text = "";
            txtQAPreTotWorkExpRemark.Text = "";

        }
        private void ClearPDInScRental()
        {

            txtRentalAmt.Text = "0";
            txtRentalAmtObj.Text = "";
            txtRentalAmtVer.Text = "";
            txtRentalAmtRemark.Text = "";


            txtQAPropTypeObj.Text = "";
            txtQAPropTypeVer.Text = "";
            txtQAPropTypeRemark.Text = "";

            txtQARenRoom.Text = "";
            txtQARenRoomObj.Text = "";
            txtQARenRoomVer.Text = "";
            txtQARenRoomRemark.Text = "";

            txtQAProAge.Text = "";
            txtQAProAgeObj.Text = "";
            txtQAProAgeVer.Text = "";
            txtQAProAgeRemark.Text = "";

            txtQAPropAd.Text = "";
            txtQAPropAdObj.Text = "";
            txtQAPropAdVer.Text = "";
            txtQAPropAdRemark.Text = "";

            txtQANOC.Text = "";
            txtQANOCObj.Text = "";
            txtQANOCVer.Text = "";
            txtQANOCRemark.Text = "";

            txtQARent.Text = "";
            txtQARentObj.Text = "";
            txtQARentVer.Text = "";
            txtQARentRemark.Text = "";
        }
        private void ClearPDInScPension()
        {
            txtPenAmt.Text = "0";
            txtPenAmtObj.Text = "";
            txtPenAmtVer.Text = "";
            txtPenAmtRemark.Text = "";

            txtQAPenStDt.Text = "";
            txtQAPenStDtObj.Text = "";
            txtQAPenStDtVer.Text = "";
            txtQAPenStDtRemark.Text = "";

            txtQAPenOrderObj.Text = "";
            txtQAPenOrderVer.Text = "";
            txtQAPenOrderRemark.Text = "";

            txtQAPenBank.Text = "";
            txtQAPenBankObj.Text = "";
            txtQAPenBankVer.Text = "";
            txtQAPenBankRemark.Text = "";


            txtQAPenIncObj.Text = "";
            txtQAPenIncVer.Text = "";
            txtQAPenIncRemark.Text = "";

            txtQAPenFromObj.Text = "";
            txtQAPenFromVer.Text = "";
            txtQAPenFromRemark.Text = "";
        }
        private void ClearPDInScWages()
        {
            txtQAWorkTypeObj.Text = "";
            txtQAWorkTypeVer.Text = "";
            txtQAWorkTypeRemark.Text = "";

            txtQAWorkFrom.Text = "";
            txtQAWorkFromObj.Text = "";
            txtQAWorkFromVer.Text = "";
            txtQAWorkFromRemark.Text = "";

            txtQAWEmpNm.Text = "";
            txtQAWEmpNmObj.Text = "";
            txtQAWEmpNmVer.Text = "";
            txtQAWEmpNmRemark.Text = "";

            txtQAEmpContNo.Text = "";
            txtQAEmpContNoObj.Text = "";
            txtQAEmpContNoVer.Text = "";
            txtQAEmpContNoRemark.Text = "";
        }
        private void ClearPDPropertyDtl()
        {
            txtQAProTypeObj.Text = "";
            txtQAProTypeVer.Text = "";
            txtQAProTypeRemark.Text = "";

            txtQAProTypeObj.Text = "";
            txtQAProTypeVer.Text = "";
            txtQAProTypeRemark.Text = "";

            txtQAPropAdd.Text = "";
            txtQAPropAddObj.Text = "";
            txtQAPropAddVer.Text = "";
            txtQAPropAddRemark.Text = "";

            txtQAProJudObj.Text = "";
            txtQAProJudVer.Text = "";
            txtQAProJudRemark.Text = "";
        }
        private void ClearLoanRequireDtl()
        {
            txtQALnPurposeObj.Text = "";
            txtQALnPurposeVer.Text = "";
            txtQALnPurposeRemark.Text = "";

            txtQALnAmt.Text = "";
            txtQALnAmtObj.Text = "";
            txtQALnAmtVer.Text = "";
            txtQALnAmtRemark.Text = "";

            txtQALnTenure.Text = "";
            txtQALnTenureObj.Text = "";
            txtQALnTenureVer.Text = "";
            txtQALnTenureRemark.Text = "";

            txtQAEMICap.Text = "";
            txtQAEMICapObj.Text = "";
            txtQAEMICapVer.Text = "";
            txtQAEMICapRemark.Text = "";

            txtQALnOutSt.Text = "";
            txtQALnOutStObj.Text = "";
            txtQALnOutStVer.Text = "";
            txtQALnOutStRemark.Text = "";

        }
        private void ClearSocialBehaviour()
        {
            txtQADrinkObj.Text = "";
            txtQADrinkVer.Text = "";
            txtQADrinkRemark.Text = "";

            txtQASocActObj.Text = "";
            txtQASocActVer.Text = "";
            txtQASocActRemark.Text = "";

            txtQAComm.Text = "";
            txtQACommObj.Text = "";
            txtQACommVer.Text = "";
            txtQACommRemark.Text = "";

        }
        private void ClearNeighbourReference()
        {
            txtQANeiNm1.Text = "";
            txtQANeiNm1Obj.Text = "";
            txtQANeiNm1Ver.Text = "";
            txtQANeiNm1Remark.Text = "";

            txtQANeiMob1.Text = "";
            txtQANeiMob1Obj.Text = "";
            txtQANeiMob1Ver.Text = "";
            txtQANeiMob1Remark.Text = "";

            txtQAFeedback1.Text = "";
            txtQAFeedback1Obj.Text = "";
            txtQAFeedback1Ver.Text = "";
            txtQAFeedback1Remark.Text = "";

            txtQANeiNm2.Text = "";
            txtQANeiNm2Obj.Text = "";
            txtQANeiNm2Ver.Text = "";
            txtQANeiNm2Remark.Text = "";

            txtQANeiMob2.Text = "";
            txtQANeiMob2Obj.Text = "";
            txtQANeiMob2Ver.Text = "";
            txtQANeiMob2Remark.Text = "";

            txtQAFeedback2.Text = "";
            txtQAFeedback2Obj.Text = "";
            txtQAFeedback2Ver.Text = "";
            txtQAFeedback2Remark.Text = "";
        }
        private void ClearPosNegFeedBack()
        {
            txtQAPos.Text = "";
            txtQAPosObj.Text = "";
            txtQAPosVer.Text = "";
            txtQAPosRemark.Text = "";

            txtQANeg.Text = "";
            txtQANegObj.Text = "";
            txtQANegVer.Text = "";
            txtQANegRemark.Text = "";
        }
        private void ClearSaving()
        {
            txtQAGold.Text = "";
            txtQAGoldObj.Text = "";
            txtQAGoldVer.Text = "";
            txtQAGoldRemark.Text = "";

            txtQABondPaper.Text = "";
            txtQABondPaperObj.Text = "";
            txtQABondPaperVer.Text = "";
            txtQABondPaperRemark.Text = "";

            txtQASaving.Text = "";
            txtQASavingObj.Text = "";
            txtQASavingVer.Text = "";
            txtQASavingRemark.Text = "";

            txtQATermInsu.Text = "";
            txtQATermInsuObj.Text = "";
            txtQATermInsuVer.Text = "";
            txtQATermInsuRemark.Text = "";

            txtQAFixedDepo.Text = "";
            txtQAFixedDepoObj.Text ="";
            txtQAFixedDepoVer.Text ="";
            txtQAFixedDepoRemark.Text="";
            
            txtQAPlots.Text = "";
            txtQAPlotsObj.Text = "";
            txtQAPlotsVer.Text = "";
            txtQAPlotsRemark.Text = ""; 
        }

        protected void btnQABack_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
        }
        protected void btnSavePersonalDtl_Click(object sender, EventArgs e)
        {
            //string pLoanAppId, string pPDDoneBy,
            string pAppName = "", pAppNameObj = "", pAppNameVer = "", pAppNameRemark = "";
            Int32 pAppRelId = 0, pAge = 0, pMaritalId = 0, pNoOfChild = 0, pPropOwn = 0, pOwnType = 0;
            string pAppRelObj = "", pAppRelVer = "", pAppRelRemark = "", pDOBObj = "", pDOBVer = "", pDOBRemark = "",
                pAgeObj = "", pAgeVer = "", pAgeRemark = "",
                pMaritalObj = "", pMaritalVer = "", pMaritalRemark = "",
                pNoOfChildObj = "", pNoOfChildVer = "", pNoOfChildRemark = "";
            //  DateTime pDOB
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pEarMem = "", pEarMemObj = "", pEarMemVer = "", pEarMemRemark = "";
            string pPropOwnObj = "", pPropOwnVer = "", pPropOwnRemark = "";
            string pOwnTypeObj = "", pOwnTypeVer = "", pOwnTypeRemark = "";
            string pHouseObs = "", pHouseObsObj = "", pHouseObsVer = "", pHouseObsRemark = "";
            string pPerAddress = "", pPerAddressObj = "", pPerAddressVer = "", pPerAddressRemark = "";
            string pRefNm = "", pRefNmObj = "", pRefNmVer = "", pRefNmRemark = "";
            string pMobNo = "", pMobNoObj = "", pMobNoVer = "", pMobNoRemark = "";
            string pLegalInfo = "", pLegalInfoObj = "", pLegalInfoVer = "", pLegalInfoRemark = "";
            string pPolInfo = "", pPolInfoObj = "", pPolInfoVer = "", pPolInfoRemark = "";
            string pPolStatus = "", pPolStatusObj = "", pPolStatusVer = "", pPolStatusRemark = "";
            string pSchool = "", pSchoolObj = "", pSchoolVer = "", pSchoolRemark = "";
            Int32 pErr = 0;


            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            pAppName = txtQAAppname.Text.ToString();
            pAppNameObj = txtQAAppnameObj.Text.ToString();
            pAppNameVer = txtQAAppnameVer.Text.ToString();
            pAppNameRemark = txtQAAppnameRemark.Text.ToString();

            if (ddlQAAppRelation.SelectedValue.ToString() != "-1")
                pAppRelId = Convert.ToInt32(ddlQAAppRelation.SelectedValue);
            pAppRelObj = txtQAAppRelationObj.Text.ToString();
            pAppRelVer = txtQAAppRelationVer.Text.ToString();
            pAppRelRemark = txtQAAppRelationRemark.Text.ToString();

            if (txtQAAppDOB.Text == "")
            {
                gblFuction.AjxMsgPopup("DOB Can Not Be Empty");
                return;
            }
            DateTime DOB = gblFuction.setDate(txtQAAppDOB.Text);
            pDOBObj = txtQAAppDOBObj.Text.ToString();
            pDOBVer = txtQAAppDOBVer.Text.ToString();
            pDOBRemark = txtQAAppDOBRemark.Text.ToString();

            if (txtQAAppAge.Text.ToString() != "")
                pAge = Convert.ToInt32(txtQAAppAge.Text);
            pAgeObj = txtQAAppAgeObj.Text.ToString();
            pAgeVer = txtQAAppAgeVer.Text.ToString();
            pAgeRemark = txtQAAppAgeRemark.Text.ToString();

            if (ddlQAMStatus.SelectedValue.ToString() != "-1")
                pMaritalId = Convert.ToInt32(ddlQAMStatus.SelectedValue);
            pMaritalObj = txtQAMStatusObj.Text.ToString();
            pMaritalVer = txtQAMStatusVer.Text.ToString();
            pMaritalRemark = txtQAMStatusRemark.Text.ToString();

            pNoOfChild = Convert.ToInt32(ddlQAChild.SelectedValue);
            pNoOfChildObj = txtQAChildObj.Text.ToString();
            pNoOfChildVer = txtQAChildVer.Text.ToString();
            pNoOfChildRemark = txtQAChildRemark.Text.ToString();

            pEarMem = Convert.ToString(ddlQAEM.SelectedValue);
            pEarMemObj = txtQAEMObj.Text.ToString();
            pEarMemVer = txtQAEMVer.Text.ToString();
            pEarMemRemark = txtQAEMRemark.Text.ToString();

            if (ddlQAPO.SelectedValue.ToString() != "-1")
                pPropOwn = Convert.ToInt32(ddlQAPO.SelectedValue);
            pPropOwnObj = txtQAPOObj.Text.ToString();
            pPropOwnVer = txtQAPOVer.Text.ToString();
            pPropOwnRemark = txtQAPORemark.Text.ToString();

            if (ddlQAOT.SelectedValue.ToString() != "-1")
                pOwnType = Convert.ToInt32(ddlQAOT.SelectedValue);
            pOwnTypeObj = txtQAOTObj.Text.ToString();
            pOwnTypeVer = txtQAOTVer.Text.ToString();
            pOwnTypeRemark = txtQAOTRemark.Text.ToString();

            pHouseObs = Convert.ToString(txtQAHO.Text);
            pHouseObsObj = txtQAHOObj.Text.ToString();
            pHouseObsVer = txtQAHOVer.Text.ToString();
            pHouseObsRemark = txtQAHORemark.Text.ToString();

            pPerAddress = Convert.ToString(txtQAPA.Text);
            pPerAddressObj = txtQAPAObj.Text.ToString();
            pPerAddressVer = txtQAPAVer.Text.ToString();
            pPerAddressRemark = txtQAPARemark.Text.ToString();

            //pRefNm = Convert.ToString(txtQARN.Text);
            //pRefNmObj = txtQARNObj.Text.ToString();
            //pRefNmVer = txtQARNVer.Text.ToString();
            //pRefNmRemark = txtQARNRemark.Text.ToString();

            pMobNo = txtQAMob.Text.ToString();
            pMobNoObj = txtQAMobObj.Text.ToString();
            pMobNoVer = txtQAMobVer.Text.ToString();
            pMobNoRemark = txtQAMobRemark.Text.ToString();

            //pLegalInfo = txtQALeg.Text.ToString();
            //pLegalInfoObj = txtQALegObj.Text.ToString();
            //pLegalInfoVer = txtQALegVer.Text.ToString();
            //pLegalInfoRemark = txtQALegRemark.Text.ToString();

            //pPolInfo = txtQAPol.Text.ToString();
            //pPolInfoObj = txtQAPolObj.Text.ToString();
            //pPolInfoVer = txtQAPolVer.Text.ToString();
            //pPolInfoRemark = txtQAPolRemark.Text.ToString();

            //pPolStatus = txtQAPolSt1.Text.ToString();
            //pPolStatusObj = txtQAPolSt1Obj.Text.ToString();
            //pPolStatusVer = txtQAPolSt1Ver.Text.ToString();
            //pPolStatusRemark = txtQAPolSt1Remark.Text.ToString();

            pSchool = ddlQASchool.SelectedValue;
            pSchoolObj = txtQASchoolObj.Text.ToString();
            pSchoolVer = txtQASchoolVer.Text.ToString();
            pSchoolRemark = txtQASchoolRemark.Text.ToString();


            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDPersonalDetail(
                    pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                    pAppName, pAppNameObj, pAppNameVer, pAppNameRemark,
                    pAppRelId, pAppRelObj, pAppRelVer, pAppRelRemark,
                    DOB, pDOBObj, pDOBVer, pDOBRemark,
                    pAge, pAgeObj, pAgeVer, pAgeRemark,
                    pMaritalId, pMaritalObj, pMaritalVer, pMaritalRemark,
                    pNoOfChild, pNoOfChildObj, pNoOfChildVer, pNoOfChildRemark,
                     pEarMem, pEarMemObj, pEarMemVer, pEarMemRemark,
                    pPropOwn, pPropOwnObj, pPropOwnVer, pPropOwnRemark,
                    pOwnType, pOwnTypeObj, pOwnTypeVer, pOwnTypeRemark,
                     pHouseObs, pHouseObsObj, pHouseObsVer, pHouseObsRemark,
                     pPerAddress, pPerAddressObj, pPerAddressVer, pPerAddressRemark,
                     pRefNm, pRefNmObj, pRefNmVer, pRefNmRemark,
                     pMobNo, pMobNoObj, pMobNoVer, pMobNoRemark,
                     pLegalInfo, pLegalInfoObj, pLegalInfoVer, pLegalInfoRemark,
                     pPolInfo, pPolInfoObj, pPolInfoVer, pPolInfoRemark,
                     pPolStatus, pPolStatusObj, pPolStatusVer, pPolStatusRemark, pCreatedBy, pCustID, pCustType
                     , pSchool, pSchoolObj, pSchoolVer, pSchoolRemark);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveIncSrcBusiness_Click(object sender, EventArgs e)
        {
            //string pLoanAppId, string pPDDoneBy,
            string pBusName = "", pBusNameObj = "", pBusNameVer = "", pBusNameRemark = "";
            Int32 pBusTypeId = 0, pBusStabId = 0, pNoOfEmp = 0, pNoOfCust = 0;
            string pBusIncObj = "", pBusIncVer = "", pBusIncRemark = "",
                pBusTypeObj = "", pBusTypeVer = "", pBusTypeRemark = "",
                pBusStabObj = "", pBusStabVer = "", pBusStabRemark = "",
                pBusAddress = "", pBusAddressObj = "", pBusAddressVer = "", pBusAddressRemark = "",
                pNoOfEmpObj = "", pNoOfEmpVer = "", pNoOfEmpRemark = "";
            string pStockSeen = "", pStockSeenObj = "", pStockSeenVer = "", pStockSeenRemark = "";
            string pVendNm1 = "", pVendNm1Obj = "", pVendNm1Ver = "", pVendNm1Remark = "";
            string pMobNo1 = "", pMobNo1Obj = "", pMobNo1Ver = "", pMobNo1Remark = "";
            string pVendNm2 = "", pVendNm2Obj = "", pVendNm2Ver = "", pVendNm2Remark = "";
            string pMobNo2 = "", pMobNo2Obj = "", pMobNo2Ver = "", pMobNo2Remark = "";
            string pBusAppName = "", pBusAppNameObj = "", pBusAppNameVer = "", pBusAppNameRemark = "";
            string pNoOfCustObj = "", pNoOfCustVer = "", pNoOfCustRemark = "";
            decimal pCreditAmt = 0;
            string pCreditAmtObj = "", pCreditAmtVer = "", pCreditAmtRemark = "";
            string pNmBoardSeen = "", pNmBoardSeenObj = "", pNmBoardSeenVer = "", pNmBoardSeenRemark = "";

            Int32 pErr = 0;
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            Decimal pBusInc = 0;
            if (txtTotIncBus.Text != "")
                pBusInc = Convert.ToDecimal(txtTotIncBus.Text);
            pBusIncObj = txtTotIncBusObj.Text.ToString();
            pBusIncVer = txtTotIncBusVer.Text.ToString();
            pBusIncRemark = txtTotIncBusRemark.Text.ToString();

            if (pBusInc <= 0)
            {
                gblFuction.AjxMsgPopup("Income from Business Can Not Be less than or equal to zero..");
                return;
            }

            decimal pGrossSales = 0, pGrossMargin = 0;
            string pGrossSalesObj="", pGrossSalesVer="", pGrossSalesRemark="",
                pGrossMarginObj="", pGrossMarginVer="", pGrossMarginRemark="";

            pGrossSales = txtGrossSalesBus.Text == "" ? 0 : Convert.ToDecimal(txtGrossSalesBus.Text);
            pGrossSalesObj = txtGrossSalesBusObj.Text;
            pGrossSalesVer = txtGrossSalesBusVer.Text;
            pGrossSalesRemark = txtGrossSalesBusRemark.Text;

            pGrossMargin = txtGrossMarginBus.Text == "" ? 0 : Convert.ToDecimal(txtGrossMarginBus.Text);
            pGrossMarginObj = txtGrossMarginBusObj.Text;
            pGrossMarginVer = txtGrossMarginBusVer.Text;
            pGrossMarginRemark = txtGrossMarginBusRemark.Text;


            //pBusInc,pBusIncObj,pBusIncVer,pBusIncRemark

            pBusName = txtQABN.Text.ToString();
            pBusNameObj = txtQABNObj.Text.ToString();
            pBusNameVer = txtQABNVer.Text.ToString();
            pBusNameRemark = txtQABNRemark.Text.ToString();

            if (ddlQABT.SelectedValue.ToString() != "-1")
                pBusTypeId = Convert.ToInt32(ddlQABT.SelectedValue);
            pBusTypeObj = txtQABTObj.Text.ToString();
            pBusTypeVer = txtQABTVer.Text.ToString();
            pBusTypeRemark = txtQABTRemark.Text.ToString();

            if (ddlQABS.SelectedValue.ToString() != "-1")
                pBusStabId = Convert.ToInt32(ddlQABS.SelectedValue);
            pBusStabObj = txtQABSObj.Text.ToString();
            pBusStabVer = txtQABSVer.Text.ToString();
            pBusStabRemark = txtQABSRemark.Text.ToString();

            pBusAddress = txtQABA.Text.ToString();
            pBusAddressObj = txtQABAObj.Text.ToString();
            pBusAddressVer = txtQABAVer.Text.ToString();
            pBusAddressRemark = txtQABARemark.Text.ToString();

            if (txtQANA.Text != "")
                pNoOfEmp = Convert.ToInt32(txtQANA.Text.ToString());
            pNoOfEmpObj = txtQANAObj.Text.ToString();
            pNoOfEmpVer = txtQANAVer.Text.ToString();
            pNoOfEmpRemark = txtQANARemark.Text.ToString();

            if (txtQANA.Text != "")
                pStockSeen = ddlQASS.SelectedValue.ToString();
            pStockSeenObj = txtQASSObj.Text.ToString();
            pStockSeenVer = txtQASSVer.Text.ToString();
            pStockSeenRemark = txtQASSRemark.Text.ToString();

            pVendNm1 = txtQAVN1.Text.ToString();
            pVendNm1Obj = txtQAVN1Obj.Text.ToString();
            pVendNm1Ver = txtQAVN1Ver.Text.ToString();
            pVendNm1Remark = txtQAVN1Remark.Text.ToString();

            pMobNo1 = txtQAVed1Mob.Text.ToString();
            pMobNo1Obj = txtQAVed1MobObj.Text.ToString();
            pMobNo1Ver = txtQAVed1MobVer.Text.ToString();
            pMobNo1Remark = txtQAVed1MobRemark.Text.ToString();

            pVendNm2 = txtQAVN2.Text.ToString();
            pVendNm2Obj = txtQAVN2Obj.Text.ToString();
            pVendNm2Ver = txtQAVN2Ver.Text.ToString();
            pVendNm2Remark = txtQAVN2Remark.Text.ToString();

            pMobNo2 = txtQAVN2Mob.Text.ToString();
            pMobNo2Obj = txtQAVN2MobObj.Text.ToString();
            pMobNo2Ver = txtQAVN2MobVer.Text.ToString();
            pMobNo2Remark = txtQAVN2MobRemark.Text.ToString();

            pBusAppName = txtQABAN.Text.ToString();
            pBusAppNameObj = txtQABANObj.Text.ToString();
            pBusAppNameVer = txtQABANVer.Text.ToString();
            pBusAppNameRemark = txtQABANRemark.Text.ToString();

            if (txtQAMCCustNo.Text != "")
                pNoOfCust = Convert.ToInt32(txtQAMCCustNo.Text.ToString());
            pNoOfCustObj = txtQAMCCustNoObj.Text.ToString();
            pNoOfCustVer = txtQAMCCustNoVer.Text.ToString();
            pNoOfCustRemark = txtQAMCCustNoRemark.Text.ToString();


            if (txtQAMCAmt.Text != "")
                pCreditAmt = Convert.ToDecimal(txtQAMCAmt.Text.ToString());
            pCreditAmtObj = txtQAMCAmtObj.Text.ToString();
            pCreditAmtVer = txtQAMCAmtVer.Text.ToString();
            pCreditAmtRemark = txtQAMCAmtRemark.Text.ToString();

            pNmBoardSeen = ddlQANB.SelectedValue.ToString();
            pNmBoardSeenObj = txtQANBObj.Text.ToString();
            pNmBoardSeenVer = txtQANBVer.Text.ToString();
            pNmBoardSeenRemark = txtQANBRemark.Text.ToString();
            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourceBusiness(
                      pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                      pBusInc, pBusIncObj, pBusIncVer, pBusIncRemark,
                      pBusName, pBusNameObj, pBusNameVer, pBusNameRemark,
                      pBusTypeId, pBusTypeObj, pBusTypeVer, pBusTypeRemark,
                      pBusStabId, pBusStabObj, pBusStabVer, pBusStabRemark,
                      pBusAddress, pBusAddressObj, pBusAddressVer, pBusAddressRemark,
                      pNoOfEmp, pNoOfEmpObj, pNoOfEmpVer, pNoOfEmpRemark,
                      pStockSeen, pStockSeenObj, pStockSeenVer, pStockSeenRemark,
                      pVendNm1, pVendNm1Obj, pVendNm1Ver, pVendNm1Remark,
                      pMobNo1, pMobNo1Obj, pMobNo1Ver, pMobNo1Remark,
                      pVendNm2, pVendNm2Obj, pVendNm2Ver, pVendNm2Remark,
                      pMobNo2, pMobNo2Obj, pMobNo2Ver, pMobNo2Remark,
                      pBusAppName, pBusAppNameObj, pBusAppNameVer, pBusAppNameRemark,
                      pNoOfCust, pNoOfCustObj, pNoOfCustVer, pNoOfCustRemark,
                       pCreditAmt, pCreditAmtObj, pCreditAmtVer, pCreditAmtRemark,
                      pNmBoardSeen, pNmBoardSeenObj, pNmBoardSeenVer, pNmBoardSeenRemark, pCreatedBy,
                      pCustID, pCustType,pGrossSales,pGrossSalesObj,pGrossSalesVer,pGrossSalesRemark,
                      pGrossMargin, pGrossMarginObj, pGrossMarginVer, pGrossMarginRemark );
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveIncSrcSalary_Click(object sender, EventArgs e)
        {
            string pEmpName = "", pEmpNameObj = "", pEmpNameVer = "", pEmpNameRemark = "";
            string pDesig = "", pDesigObj = "", pDesigVer = "", pDesigRemark = "";
            DateTime? pDOJ = null;
            string pDOJObj = "", pDOJVer = "", pDOJRemark = "";
            string pJobStb = "", pJobStbObj = "", pJobStbVer = "", pJobStbRemark = "";
            string pEmpAddress = "", pEmpAddressObj = "", pEmpAddressVer = "", pEmpAddressRemark = "";
            Int32 pSalCrModeId = 0;
            string pSalCrModeObj = "", pSalCrModeVer = "", pSalCrModeRemark = "";
            Int32 pSalTypeId = 0;
            string pSalTypeObj = "", pSalTypeVer = "", pSalTypeRemark = "";
            string pBankName = "", pBankNameObj = "", pBankNameVer = "", pBankNameRemark = "";
            decimal pNetSal = 0;
            string pNetSalObj = "", pNetSalVer = "", pNetSalRemark = "";
            string pHRName = "", pHRNameObj = "", pHRNameVer = "", pHRNameRemark = "";
            string pHRMobNo = "", pHRMobNoObj = "", pHRMobNoVer = "", pHRMobNoRemark = "";
            string pCollName = "", pCollNameObj = "", pCollNameVer = "", pCollNameRemark = "";
            string pCollMobNo = "", pCollMobNoObj = "", pCollMobNoVer = "", pCollMobNoRemark = "";
            string pPreEmpName = "", pPreEmpNameObj = "", pPreEmpNameVer = "", pPreEmpNameRemark = "";
            string pPreEmpAddress = "", pPreEmpAddressObj = "", pPreEmpAddressVer = "", pPreEmpAddressRemark = "";
            DateTime? pPreEmpDOJ = null;
            string pPreEmpDOJObj = "", pPreEmpDOJVer = "", pPreEmpDOJRemark = "";
            DateTime? pPreEmpDOL = null;
            string pPreEmpDOLObj = "", pPreEmpDOLVer = "", pPreEmpDOLRemark = "";
            decimal pPreWorkExp = 0, pTotWorkExp = 0;
            string pPreWorkExpObj = "", pPreWorkExpVer = "", pPreWorkExpRemark = "";
            string pTotWorkExpObj = "", pTotWorkExpVer = "", pTotWorkExpRemark = "";
            Int32 pCreatedBy = 1, pErr = 0;

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }


            pEmpName = txtQAEmpNm.Text.ToString();
            pEmpNameObj = txtQAEmpNmObj.Text.ToString();
            pEmpNameVer = txtQAEmpNmVer.Text.ToString();
            pEmpNameRemark = txtQAEmpNmRemark.Text.ToString();

            pDesig = txtQADes.Text.ToString();
            pDesigObj = txtQADesObj.Text.ToString();
            pDesigVer = txtQADesVer.Text.ToString();
            pDesigRemark = txtQADesRemark.Text.ToString();

            if (txtQADOJ.Text != "")
                pDOJ = gblFuction.setDate(txtQADOJ.Text);
            pDOJObj = txtQADOJObj.Text.ToString();
            pDOJVer = txtQADOJVer.Text.ToString();
            pDOJRemark = txtQADOJRemark.Text.ToString();

            pJobStb = txtQAJob.Text.ToString();
            pJobStbObj = txtQAJobObj.Text.ToString();
            pJobStbVer = txtQAJobVer.Text.ToString();
            pJobStbRemark = txtQAJobRemark.Text.ToString();

            pEmpAddress = txtQAEmpAdd.Text.ToString();
            pEmpAddressObj = txtQAEmpAddObj.Text.ToString();
            pEmpAddressVer = txtQAEmpAddVer.Text.ToString();
            pEmpAddressRemark = txtQAEmpAddRemark.Text.ToString();

            if (ddlQASalMode.SelectedValue != "-1")
                pSalCrModeId = Convert.ToInt32(ddlQASalMode.SelectedValue);
            pSalCrModeObj = txtQASalModeObj.Text.ToString();
            pSalCrModeVer = txtQASalModeVer.Text.ToString();
            pSalCrModeRemark = txtQASalModeRemark.Text.ToString();

            if (ddlQASalType.SelectedValue != "-1")
                pSalTypeId = Convert.ToInt32(ddlQASalType.SelectedValue);
            pSalTypeObj = txtQASalTypeObj.Text.ToString();
            pSalTypeVer = txtQASalTypeVer.Text.ToString();
            pSalTypeRemark = txtQASalTypeRemark.Text.ToString();


            pBankName = txtQASalBNm.Text.ToString();
            pBankNameObj = txtQASalBNmObj.Text.ToString();
            pBankNameVer = txtQASalBNmVer.Text.ToString();
            pBankNameRemark = txtQASalBNmRemark.Text.ToString();

            if (txtQANetSal.Text != "")
                pNetSal = Convert.ToDecimal(txtQANetSal.Text);
            pNetSalObj = txtQANetSalObj.Text.ToString();
            pNetSalVer = txtQANetSalVer.Text.ToString();
            pNetSalRemark = txtQANetSalRemark.Text.ToString();

            pHRName = txtQAManNm.Text.ToString();
            pHRNameObj = txtQAManNmObj.Text.ToString();
            pHRNameVer = txtQAManNmVer.Text.ToString();
            pHRNameRemark = txtQAManNmRemark.Text.ToString();

            pHRMobNo = txtQAManMob.Text.ToString();
            pHRMobNoObj = txtQAManMobObj.Text.ToString();
            pHRMobNoVer = txtQAManMobVer.Text.ToString();
            pHRMobNoRemark = txtQAManMobRemark.Text.ToString();

            pCollName = txtQAMCNm.Text.ToString();
            pCollNameObj = txtQAMCNmObj.Text.ToString();
            pCollNameVer = txtQAMCNmVer.Text.ToString();
            pCollNameRemark = txtQAMCNmRemark.Text.ToString();

            pCollMobNo = txtQAMCMob.Text.ToString();
            pCollMobNoObj = txtQAMCMobObj.Text.ToString();
            pCollMobNoVer = txtQAMCMobVer.Text.ToString();
            pCollMobNoRemark = txtQAMCMobRemark.Text.ToString();

            pPreEmpName = txtQAPreEmpNm.Text.ToString();
            pPreEmpNameObj = txtQAPreEmpNmObj.Text.ToString();
            pPreEmpNameVer = txtQAPreEmpNmVer.Text.ToString();
            pPreEmpNameRemark = txtQAPreEmpNmRemark.Text.ToString();

            pPreEmpAddress = txtQAPreEmpAd.Text.ToString();
            pPreEmpAddressObj = txtQAPreEmpAdObj.Text.ToString();
            pPreEmpAddressVer = txtQAPreEmpAdVer.Text.ToString();
            pPreEmpAddressRemark = txtQAPreEmpAdRemark.Text.ToString();

            if (txtQAPreEmpDOJ.Text != "")
                pPreEmpDOJ = gblFuction.setDate(txtQAPreEmpDOJ.Text);
            pPreEmpDOJObj = txtQAPreEmpDOJObj.Text.ToString();
            pPreEmpDOJVer = txtQAPreEmpDOJVer.Text.ToString();
            pPreEmpDOJRemark = txtQAPreEmpDOJRemark.Text.ToString();

            if (txtQAPreEmpDOL.Text != "")
                pPreEmpDOL = gblFuction.setDate(txtQAPreEmpDOL.Text);
            pPreEmpDOLObj = txtQAPreEmpDOLObj.Text.ToString();
            pPreEmpDOLVer = txtQAPreEmpDOLVer.Text.ToString();
            pPreEmpDOLRemark = txtQAPreEmpDOLRemark.Text.ToString();

            if (txtQAPreEmpWorkExp.Text != "")
                pPreWorkExp = Convert.ToDecimal(txtQAPreEmpWorkExp.Text);
            pPreWorkExpObj = txtQAPreEmpWorkExpObj.Text.ToString();
            pPreWorkExpVer = txtQAPreEmpWorkExpVer.Text.ToString();
            pPreWorkExpRemark = txtQAPreEmpWorkExpRemark.Text.ToString();

            if (txtQAPreTotWorkExp.Text != "")
                pTotWorkExp = Convert.ToDecimal(txtQAPreTotWorkExp.Text);
            pTotWorkExpObj = txtQAPreTotWorkExpObj.Text.ToString();
            pTotWorkExpVer = txtQAPreTotWorkExpVer.Text.ToString();
            pTotWorkExpRemark = txtQAPreTotWorkExpRemark.Text.ToString();

            if (pNetSal <= 0)
            {
                gblFuction.AjxMsgPopup("Monthly Net Salary Can Not Be Zero.. Please Input Net Salary Before Save...");
                return;
            }


            pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourceSalary(
                pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                pEmpName, pEmpNameObj, pEmpNameVer, pEmpNameRemark,
                pDesig, pDesigObj, pDesigVer, pDesigRemark,
                pDOJ, pDOJObj, pDOJVer, pDOJRemark,
                pJobStb, pJobStbObj, pJobStbVer, pJobStbRemark,
                pEmpAddress, pEmpAddressObj, pEmpAddressVer, pEmpAddressRemark,
                pSalCrModeId, pSalCrModeObj, pSalCrModeVer, pSalCrModeRemark,
                pSalTypeId, pSalTypeObj, pSalTypeVer, pSalTypeRemark,
                pBankName, pBankNameObj, pBankNameVer, pBankNameRemark,
                pNetSal, pNetSalObj, pNetSalVer, pNetSalRemark,
                pHRName, pHRNameObj, pHRNameVer, pHRNameRemark,
                pHRMobNo, pHRMobNoObj, pHRMobNoVer, pHRMobNoRemark,
                pCollName, pCollNameObj, pCollNameVer, pCollNameRemark,
                pCollMobNo, pCollMobNoObj, pCollMobNoVer, pCollMobNoRemark,
                pPreEmpName, pPreEmpNameObj, pPreEmpNameVer, pPreEmpNameRemark,
                pPreEmpAddress, pPreEmpAddressObj, pPreEmpAddressVer, pPreEmpAddressRemark,
                pPreEmpDOJ, pPreEmpDOJObj, pPreEmpDOJVer, pPreEmpDOJRemark,
                pPreEmpDOL, pPreEmpDOLObj, pPreEmpDOLVer, pPreEmpDOLRemark,
                pPreWorkExp, pPreWorkExpObj, pPreWorkExpVer, pPreWorkExpRemark,
                pTotWorkExp, pTotWorkExpObj, pTotWorkExpVer, pTotWorkExpRemark, pCreatedBy,
                pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveIncSrcRental_Click(object sender, EventArgs e)
        {
            Int32 pPropType = 0, pNoOfRentRoom = 0, pPropAge = 0, pErr = 0;
            string pRentalAmtObj = "", pRentalAmtVer = "", pRentalAmtRemark = "";
            string pPropTypeObj = "", pPropTypeVer = "", pPropTypeRemark = "";
            string pNoOfRentRoomObj = "", pNoOfRentRoomVer = "", pNoOfRentRoomRemark = "";
            string pPropAgeObj = "", pPropAgeVer = "", pPropAgeRemark = "";
            string pTenantNOC = "", pTenantNOCObj = "", pTenantNOCVer = "", pTenantNOCRemark = "";
            string pPropAddress = "", pPropAddressObj = "", pPropAddressVer = "", pPropAddressRemark = "";
            string pRentalDoc = "", pRentalDocObj = "", pRentalDocVer = "", pRentalDocRemark = "";
            decimal pRentalAmt = 0;
            if (txtRentalAmt.Text != "")
                pRentalAmt = Convert.ToDecimal(txtRentalAmt.Text);
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            if (pRentalAmt <= 0)
            {
                gblFuction.AjxMsgPopup("Rental Amount Can Not Be Zero....");
                txtRentalAmt.Focus();
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(ViewState["CustID"].ToString()) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(ViewState["CustType"].ToString()) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }


            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();



            //pRentalAmt,pRentalAmtObj,pRentalAmtVer,pRentalAmtRemark
            pRentalAmtObj = txtQAPropTypeObj.Text.ToString();
            pRentalAmtVer = txtQAPropTypeVer.Text.ToString();
            pRentalAmtRemark = txtQAPropTypeRemark.Text.ToString();


            if (ddlQAPropType.SelectedValue != "-1")
                pPropType = Convert.ToInt32(ddlQAPropType.SelectedValue);
            pPropTypeObj = txtQAPropTypeObj.Text.ToString();
            pPropTypeVer = txtQAPropTypeVer.Text.ToString();
            pPropTypeRemark = txtQAPropTypeRemark.Text.ToString();

            if (txtQARenRoom.Text != "")
                pNoOfRentRoom = Convert.ToInt32(txtQARenRoom.Text);
            pNoOfRentRoomObj = txtQARenRoomObj.Text.ToString();
            pNoOfRentRoomVer = txtQARenRoomVer.Text.ToString();
            pNoOfRentRoomRemark = txtQARenRoomRemark.Text.ToString();

            if (txtQAProAge.Text != "")
                pPropAge = Convert.ToInt32(txtQAProAge.Text);
            pPropAgeObj = txtQAProAgeObj.Text.ToString();
            pPropAgeVer = txtQAProAgeVer.Text.ToString();
            pPropAgeRemark = txtQAProAgeRemark.Text.ToString();


            pPropAddress = Convert.ToString(txtQAPropAd.Text);
            pPropAddressObj = txtQAPropAdObj.Text.ToString();
            pPropAddressVer = txtQAPropAdVer.Text.ToString();
            pPropAddressRemark = txtQAPropAdRemark.Text.ToString();

            pTenantNOC = Convert.ToString(txtQANOC.Text);
            pTenantNOCObj = txtQANOCObj.Text.ToString();
            pTenantNOCVer = txtQANOCVer.Text.ToString();
            pTenantNOCRemark = txtQANOCRemark.Text.ToString();

            pRentalDoc = Convert.ToString(txtQARent.Text);
            pRentalDocObj = txtQARentObj.Text.ToString();
            pRentalDocVer = txtQARentVer.Text.ToString();
            pRentalDocRemark = txtQARentRemark.Text.ToString();
            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourceRental(
               pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
               pRentalAmt, pRentalAmtObj, pRentalAmtVer, pRentalAmtRemark,
               pPropType, pPropTypeObj, pPropTypeVer, pPropTypeRemark,
               pNoOfRentRoom, pNoOfRentRoomObj, pNoOfRentRoomVer, pNoOfRentRoomRemark,
               pPropAge, pPropAgeObj, pPropAgeVer, pPropAgeRemark,
               pPropAddress, pPropAddressObj, pPropAddressVer, pPropAddressRemark,
               pTenantNOC, pTenantNOCObj, pTenantNOCVer, pTenantNOCRemark,
               pRentalDoc, pRentalDocObj, pRentalDocVer, pRentalDocRemark, pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }

        }
        protected void btnSaveIncSrcPension_Click(object sender, EventArgs e)
        {
            DateTime? pPenStDate = null;
            string pPensionAmtObj = "", pPensionAmtVer = "", pPensionAmtRemark = "";
            string pPenStDateObj = "", pPenStDateVer = "", pPenStDateRemark = "";
            string pPenOrder = "", pPenOrderObj = "", pPenOrderVer = "", pPenOrderRemark = "";
            Int32 pPenIncId = 0, pPenFromId = 0, pErr = 0;
            string pPenIncObj = "", pPenIncVer = "", pPenIncRemark = "";
            string pPenBankName = "", pPenBankNameObj = "", pPenBankNameVer = "", pPenBankNameRemark = "";
            string pPenFromObj = "", pPenFromVer = "", pPenFromRemark = "";
            decimal pPensionAmt = 0;
            if (txtPenAmt.Text != "")
                pPensionAmt = Convert.ToDecimal(txtPenAmt.Text);
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            if (pPensionAmt <= 0)
            {
                gblFuction.AjxMsgPopup("Pension Amount Can Not Be Zero....");
                txtPenAmt.Focus();
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }

            pPensionAmtObj = txtPenAmtObj.Text.ToString();
            pPensionAmtVer = txtPenAmtVer.Text.ToString();
            pPensionAmtRemark = txtPenAmtRemark.Text.ToString();


            if (txtQAPenStDt.Text != "")
                pPenStDate = gblFuction.setDate(txtQAPenStDt.Text);
            pPenStDateObj = txtQAPropTypeObj.Text.ToString();
            pPenStDateVer = txtQAPropTypeVer.Text.ToString();
            pPenStDateRemark = txtQAPropTypeRemark.Text.ToString();


            if (ddlQAPenOrder.SelectedValue != "-1")
                pPenOrder = Convert.ToString(ddlQAPenOrder.SelectedValue);
            pPenOrderObj = txtQAPropTypeObj.Text.ToString();
            pPenOrderVer = txtQAPropTypeVer.Text.ToString();
            pPenOrderRemark = txtQAPropTypeRemark.Text.ToString();


            pPenBankName = txtQAPenBank.Text.ToString();
            pPenBankNameObj = txtQAPenBankObj.Text.ToString();
            pPenBankNameVer = txtQAPenBankVer.Text.ToString();
            pPenBankNameRemark = txtQAPenBankRemark.Text.ToString();

            if (ddlQAPenInc.SelectedValue != "-1")
                pPenIncId = Convert.ToInt32(ddlQAPenInc.SelectedValue);
            pPenIncObj = txtQAPenIncObj.Text.ToString();
            pPenIncVer = txtQAPenIncVer.Text.ToString();
            pPenIncRemark = txtQAPenIncRemark.Text.ToString();

            if (ddlQAPenFrom.SelectedValue != "-1")
                pPenFromId = Convert.ToInt32(ddlQAPenFrom.SelectedValue);
            pPenFromObj = txtQAPenFromObj.Text.ToString();
            pPenFromVer = txtQAPenFromVer.Text.ToString();
            pPenFromRemark = txtQAPenFromRemark.Text.ToString();

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourcePension(
                 pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                 pPensionAmt, pPensionAmtObj, pPensionAmtVer, pPensionAmtRemark,
                 pPenStDate, pPenStDateObj, pPenStDateVer, pPenStDateRemark,
                 pPenOrder, pPenOrderObj, pPenOrderVer, pPenOrderRemark,
                 pPenBankName, pPenBankNameObj, pPenBankNameVer, pPenBankNameRemark,
                 pPenIncId, pPenIncObj, pPenIncVer, pPenIncRemark,
                 pPenFromId, pPenFromObj, pPenFromVer, pPenFromRemark, pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveIncSrcWages_Click(object sender, EventArgs e)
        {

            Int32 pErr = 0;
            String pWorkType = "", pWorkTypeObj = "", pWorkTypeVer = "", pWorkTypeRemark = "";
            string pWorkDoingFrom = "", pWorkDoingFromObj = "", pWorkDoingFromVer = "", pWorkDoingFromRemark = "";
            string pWagesEmpName = "", pWagesEmpNameObj = "", pWagesEmpNameVer = "", pWagesEmpNameRemark = "";
            string pEmpContactNo = "", pEmpContactNoObj = "", pEmpContactNoVer = "", pEmpContactNoRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }

            pWorkType = ddlQAWorkType.SelectedValue.ToString();
            pWorkTypeObj = txtQAWorkTypeObj.Text.ToString();
            pWorkTypeVer = txtQAWorkTypeVer.Text.ToString();
            pWorkTypeRemark = txtQAWorkTypeRemark.Text.ToString();

            pWorkDoingFrom = txtQAWorkFrom.Text.ToString();
            pWorkDoingFromObj = txtQAWorkFromObj.Text.ToString();
            pWorkDoingFromVer = txtQAWorkFromVer.Text.ToString();
            pWorkDoingFromRemark = txtQAWorkFromRemark.Text.ToString();

            pWagesEmpName = txtQAWEmpNm.Text.ToString();
            pWagesEmpNameObj = txtQAWEmpNmObj.Text.ToString();
            pWagesEmpNameVer = txtQAWEmpNmVer.Text.ToString();
            pWagesEmpNameRemark = txtQAWEmpNmRemark.Text.ToString();

            pEmpContactNo = txtQAEmpContNo.Text.ToString();
            pEmpContactNoObj = txtQAEmpContNoObj.Text.ToString();
            pEmpContactNoVer = txtQAEmpContNoVer.Text.ToString();
            pEmpContactNoRemark = txtQAEmpContNoRemark.Text.ToString();


            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourceWages(
                   pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                   pWorkType, pWorkTypeObj, pWorkTypeVer, pWorkTypeRemark,
                   pWorkDoingFrom, pWorkDoingFromObj, pWorkDoingFromVer, pWorkDoingFromRemark,
                   pWagesEmpName, pWagesEmpNameObj, pWagesEmpNameVer, pWagesEmpNameRemark,
                   pEmpContactNo, pEmpContactNoObj, pEmpContactNoVer, pEmpContactNoRemark,
                    pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveIncSrcPropDtl_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0, pProTypeId = 0, pPropNaureId = 0, pPropJudicialId = 0;
            string pProTypeObj = "", pProTypeVer = "", pProTypeRemark = "";
            string pPropNaureObj = "", pPropNaureVer = "", pPropNaureRemark = "";
            string pPropAddress = "", pPropAddressObj = "", pPropAddressVer = "", pPropAddressRemark = "";
            string pPropJudicialObj = "", pPropJudicialVer = "", pPropJudicialRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            if (ddlQAProType.SelectedValue != "-1")
                pProTypeId = Convert.ToInt32(ddlQAProType.SelectedValue.ToString());
            pProTypeObj = txtQAProTypeObj.Text.ToString();
            pProTypeVer = txtQAProTypeVer.Text.ToString();
            pProTypeRemark = txtQAProTypeRemark.Text.ToString();

            if (ddlQAProNature.SelectedValue != "-1")
                pPropNaureId = Convert.ToInt32(ddlQAProNature.SelectedValue.ToString());
            pPropNaureObj = txtQAProNatureObj.Text.ToString();
            pPropNaureVer = txtQAProNatureVer.Text.ToString();
            pPropNaureRemark = txtQAProNatureRemark.Text.ToString();

            pPropAddress = txtQAPropAdd.Text.ToString();
            pPropAddressObj = txtQAPropAddObj.Text.ToString();
            pPropAddressVer = txtQAPropAddVer.Text.ToString();
            pPropAddressRemark = txtQAPropAddRemark.Text.ToString();

            if (ddlQAProJud.SelectedValue != "-1")
                pPropJudicialId = Convert.ToInt32(ddlQAProJud.SelectedValue.ToString());
            pPropJudicialObj = txtQAProNatureObj.Text.ToString();
            pPropJudicialVer = txtQAProNatureVer.Text.ToString();
            pPropJudicialRemark = txtQAProNatureRemark.Text.ToString();


            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDPropertyDetail(
                   pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                   pProTypeId, pProTypeObj, pProTypeVer, pProTypeRemark,
                   pPropNaureId, pPropNaureObj, pPropNaureVer, pPropNaureRemark,
                   pPropAddress, pPropAddressObj, pPropAddressVer, pPropAddressRemark,
                   pPropJudicialId, pPropJudicialObj, pPropJudicialVer, pPropJudicialRemark,
                    pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveLnReqDtl_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0, pExpLnTenore = 0, pLnPurpose = 0;
            decimal pExpLnAmt = 0, pTotLnOutstanding = 0, pEMICap = 0;
            string pLnPurposeObj = "", pLnPurposeVer = "", pLnPurposeRemark = "";
            string pExpLnAmtObj = "", pExpLnAmtVer = "", pExpLnAmtRemark = "";
            string pExpLnTenoreObj = "", pExpLnTenoreVer = "", pExpLnTenoreRemark = "";
            string pEMICapObj = "", pEMICapVer = "", pEMICapRemark = "";
            string pTotLnOutstandingObj = "", pTotLnOutstandingVer = "", pTotLnOutstandingRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }

            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            if (ddlQALnPurpose.SelectedValue != "-1")
                pLnPurpose = Convert.ToInt32(ddlQALnPurpose.SelectedValue.ToString());
            pLnPurposeObj = txtQALnPurposeObj.Text.ToString();
            pLnPurposeVer = txtQALnPurposeVer.Text.ToString();
            pLnPurposeRemark = txtQALnPurposeRemark.Text.ToString();

            if (txtQALnAmt.Text != "")
                pExpLnAmt = Convert.ToDecimal(txtQALnAmt.Text);
            pExpLnAmtObj = txtQALnAmtObj.Text.ToString();
            pExpLnAmtVer = txtQALnAmtVer.Text.ToString();
            pExpLnAmtRemark = txtQALnAmtRemark.Text.ToString();


            if (txtQALnTenure.Text != "")
                pExpLnTenore = Convert.ToInt32(txtQALnTenure.Text);
            pExpLnTenoreObj = txtQALnTenureObj.Text.ToString();
            pExpLnTenoreVer = txtQALnTenureVer.Text.ToString();
            pExpLnTenoreRemark = txtQALnTenureRemark.Text.ToString();

            if (txtQAEMICap.Text != "")
                pEMICap = Convert.ToDecimal(txtQAEMICap.Text);
            pEMICapObj = txtQAEMICapObj.Text.ToString();
            pEMICapVer = txtQAEMICapVer.Text.ToString();
            pEMICapRemark = txtQAEMICapRemark.Text.ToString();

            if (txtQALnOutSt.Text != "")
                pTotLnOutstanding = Convert.ToDecimal(txtQALnOutSt.Text);
            pTotLnOutstandingObj = txtQALnOutStObj.Text.ToString();
            pTotLnOutstandingVer = txtQALnOutStVer.Text.ToString();
            pTotLnOutstandingRemark = txtQALnOutStRemark.Text.ToString();


            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDLoanRequireDetail(
                 pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                 pLnPurpose, pLnPurposeObj, pLnPurposeVer, pLnPurposeRemark,
                 pExpLnAmt, pExpLnAmtObj, pExpLnAmtVer, pExpLnAmtRemark,
                 pExpLnTenore, pExpLnTenoreObj, pExpLnTenoreVer, pExpLnTenoreRemark,
                 pEMICap, pEMICapObj, pEMICapVer, pEMICapRemark,
                 pTotLnOutstanding, pTotLnOutstandingObj, pTotLnOutstandingVer, pTotLnOutstandingRemark,
                 pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveSocBehaviour_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0;
            string pDrinkingHabit = "", pDrinkingHabitObj = "", pDrinkingHabitVer = "", pDrinkingHabitRemark = "";
            string pSocialAct = "", pSocialActObj = "", pSocialActVer = "", pSocialActRemark = "";
            string pInvAnyCommitment = "", pInvAnyCommitmentObj = "", pInvAnyCommitmentVer = "", pInvAnyCommitmentRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }

            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);
            if (ddlQADrink.SelectedValue != "-1")
                pDrinkingHabit = ddlQADrink.SelectedValue.ToString();
            pDrinkingHabitObj = txtQALnPurposeObj.Text.ToString();
            pDrinkingHabitVer = txtQALnPurposeVer.Text.ToString();
            pDrinkingHabitRemark = txtQALnPurposeRemark.Text.ToString();

            if (ddlQASocAct.SelectedValue != "-1")
                pSocialAct = ddlQASocAct.SelectedValue.ToString();
            pSocialActObj = txtQASocActObj.Text.ToString();
            pSocialActVer = txtQASocActVer.Text.ToString();
            pSocialActRemark = txtQASocActRemark.Text.ToString();


            pInvAnyCommitment = txtQAComm.Text.ToString();
            pInvAnyCommitmentObj = txtQACommObj.Text.ToString();
            pInvAnyCommitmentVer = txtQACommVer.Text.ToString();
            pInvAnyCommitmentRemark = txtQACommRemark.Text.ToString();

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDSocialBehabiour(
                 pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                 pDrinkingHabit, pDrinkingHabitObj, pDrinkingHabitVer, pDrinkingHabitRemark,
                 pSocialAct, pSocialActObj, pSocialActVer, pSocialActRemark,
                 pInvAnyCommitment, pInvAnyCommitmentObj, pInvAnyCommitmentVer, pInvAnyCommitmentRemark,
                 pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveNeighRef_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0;
            string pNeibourNm1 = "", pNeibourNm1Obj = "", pNeibourNm1Ver = "", pNeibourNm1Remark = "";
            string pNeibourMob1 = "", pNeibourMob1Obj = "", pNeibourMob1Ver = "", pNeibourMob1Remark = "";
            string pFeedBack1 = "", pFeedBack1Obj = "", pFeedBack1Ver, pFeedBack1Remark = "";
            string pNeibourNm2 = "", pNeibourNm2Obj = "", pNeibourNm2Ver, pNeibourNm2Remark = "";
            string pNeibourMob2 = "", pNeibourMob2Obj = "", pNeibourMob2Ver = "", pNeibourMob2Remark = "";
            string pFeedBack2 = "", pFeedBack2Obj = "", pFeedBack2Ver = "", pFeedBack2Remark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            pNeibourNm1 = txtQANeiNm1.Text.ToString();
            pNeibourNm1Obj = txtQANeiNm1Obj.Text.ToString();
            pNeibourNm1Ver = txtQANeiNm1Ver.Text.ToString();
            pNeibourNm1Remark = txtQANeiNm1Remark.Text.ToString();

            pNeibourMob1 = txtQANeiMob1.Text.ToString();
            pNeibourMob1Obj = txtQANeiMob1Obj.Text.ToString();
            pNeibourMob1Ver = txtQANeiMob1Ver.Text.ToString();
            pNeibourMob1Remark = txtQANeiMob1Remark.Text.ToString();

            pFeedBack1 = txtQAFeedback1.Text.ToString();
            pFeedBack1Obj = txtQAFeedback1Obj.Text.ToString();
            pFeedBack1Ver = txtQAFeedback1Ver.Text.ToString();
            pFeedBack1Remark = txtQAFeedback1Remark.Text.ToString();

            pNeibourNm2 = txtQANeiNm2.Text.ToString();
            pNeibourNm2Obj = txtQANeiNm2Obj.Text.ToString();
            pNeibourNm2Ver = txtQANeiNm2Ver.Text.ToString();
            pNeibourNm2Remark = txtQANeiNm2Remark.Text.ToString();

            pNeibourMob2 = txtQANeiMob2.Text.ToString();
            pNeibourMob2Obj = txtQANeiMob2Obj.Text.ToString();
            pNeibourMob2Ver = txtQANeiMob2Ver.Text.ToString();
            pNeibourMob2Remark = txtQANeiMob2Remark.Text.ToString();

            pFeedBack2 = txtQAFeedback2.Text.ToString();
            pFeedBack2Obj = txtQAFeedback2Obj.Text.ToString();
            pFeedBack2Ver = txtQAFeedback2Ver.Text.ToString();
            pFeedBack2Remark = txtQAFeedback2Remark.Text.ToString();

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDNeighbourReference(
                 pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                 pNeibourNm1, pNeibourNm1Obj, pNeibourNm1Ver, pNeibourNm1Remark,
                 pNeibourMob1, pNeibourMob1Obj, pNeibourMob1Ver, pNeibourMob1Remark,
                 pFeedBack1, pFeedBack1Obj, pFeedBack1Ver, pFeedBack1Remark,
                 pNeibourNm2, pNeibourNm2Obj, pNeibourNm2Ver, pNeibourNm2Remark,
                 pNeibourMob2, pNeibourMob2Obj, pNeibourMob2Ver, pNeibourMob2Remark,
                 pFeedBack2, pFeedBack2Obj, pFeedBack2Ver, pFeedBack2Remark,
                 pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSavePosNegObs_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0;
            string pPositive = "", pPositiveObj = "", pPositiveVer = "", pPositiveRemark = "";
            string pNegative = "", pNegativeObj = "", pNegativeVer = "", pNegativeRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }

            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            pPositive = txtQAPos.Text.ToString();
            pPositiveObj = txtQAPosObj.Text.ToString();
            pPositiveVer = txtQAPosVer.Text.ToString();
            pPositiveRemark = txtQAPosRemark.Text.ToString();

            pNegative = txtQANeg.Text.ToString();
            pNegativeObj = txtQANegObj.Text.ToString();
            pNegativeVer = txtQANegVer.Text.ToString();
            pNegativeRemark = txtQANegRemark.Text.ToString();

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDPositiveNegativeObservation(
                 pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                 pPositive, pPositiveObj, pPositiveVer, pPositiveRemark,
                 pNegative, pNegativeObj, pNegativeVer, pNegativeRemark,
                 pCreatedBy, pCustID, pCustType);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveInvSaving_Click(object sender, EventArgs e)
        {
            Int32 pErr = 0;
            string pGold = "", pGoldObj = "", pGoldVer = "", pGoldRemark = "";
            string pBondPaper = "", pBondPaperObj = "", pBondPaperVer = "", pBondPaperRemark = "";
            string pDailySaving = "", pDailySavingObj = "", pDailySavingVer = "", pDailySavingRemark = "";
            string pInsuPolicy = "", pInsuPolicyObj = "", pInsuPolicyVer = "", pInsuPolicyRemark = "";

            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }

            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            pGold = txtQAGold.Text.ToString();
            pGoldObj = txtQAGoldObj.Text.ToString();
            pGoldVer = txtQAGoldVer.Text.ToString();
            pGoldRemark = txtQAGoldRemark.Text.ToString();

            pBondPaper = txtQABondPaper.Text.ToString();
            pBondPaperObj = txtQABondPaperObj.Text.ToString();
            pBondPaperVer = txtQABondPaperVer.Text.ToString();
            pBondPaperRemark = txtQABondPaperRemark.Text.ToString();

            pDailySaving = txtQASaving.Text.ToString();
            pDailySavingObj = txtQASavingObj.Text.ToString();
            pDailySavingVer = txtQASavingVer.Text.ToString();
            pDailySavingRemark = txtQASavingRemark.Text.ToString();

            pInsuPolicy = txtQATermInsu.Text.ToString();
            pInsuPolicyObj = txtQATermInsuObj.Text.ToString();
            pInsuPolicyVer = txtQATermInsuVer.Text.ToString();
            pInsuPolicyRemark = txtQATermInsuRemark.Text.ToString();

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDInvestmentSaving(
                pLoanAppId, Session["PDDoneBy"].ToString(), pPDDate,
                pGold, pGoldObj, pGoldVer, pGoldRemark,
                pBondPaper, pBondPaperObj, pBondPaperVer, pBondPaperRemark,
                pDailySaving, pDailySavingObj, pDailySavingVer, pDailySavingRemark,
                pInsuPolicy, pInsuPolicyObj, pInsuPolicyVer, pInsuPolicyRemark,
                pCreatedBy, pCustID, pCustType, txtQAFixedDepo.Text, txtQAFixedDepoObj.Text,
                txtQAFixedDepoVer.Text, txtQAFixedDepoRemark.Text, txtQAGold.Text, txtQAGoldObj.Text,
                txtQAGoldVer.Text, txtQAGoldRemark.Text);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveRPH_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDPersonalReference(pLoanAppId, pPDDate, txtRPHName.Text, txtRPHContactNo.Text, ddlRPHRelation.SelectedValue,
                       txtRPHResidance.Text, ddlRPHOccupation.SelectedValue, Convert.ToInt16(ddlRPHNoofYears.SelectedValue), pCustType, pCreatedBy);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnApplicantProfile_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDApplicantProfile(pLoanAppId, pPDDate, ddlAppCoOper.SelectedValue, ddlAppAccuInfo.SelectedValue, ddlAppBusiKnow.SelectedValue, ddlAppHouseHold.SelectedValue
                    , ddlAppSavingCapacity.SelectedValue, ddlAppQualityInventory.SelectedValue, ddlAppPhyFitness.SelectedValue, ddlAppFamilySuppot.SelectedValue, pCustType, pCreatedBy);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnCoApplicantProfile_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDCoApplicantProfile(pLoanAppId, pPDDate, ddlCoAppCoOper.SelectedValue, ddlCoAppAccuInfo.SelectedValue, ddlCoAppBusiKnow.SelectedValue, ddlCoAppHouseHold.SelectedValue
                    , ddlCoAppSavingCapacity.SelectedValue, ddlCoAppQualityInventory.SelectedValue, ddlCoAppPhyFitness.SelectedValue, ddlCoAppFamilySuppot.SelectedValue, pCustType, pCreatedBy);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveRPO1_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDBusinessReference1(pLoanAppId, pPDDate, txtRPO1Name.Text, txtRPO1ContactNo.Text, ddlRPO1Relation.SelectedValue,
                       txtRPO1Residance.Text, ddlRPO1Occupation.SelectedValue, Convert.ToInt16(ddlRPO1NoofYears.SelectedValue), pCustType, pCreatedBy,
                       txtRPO1OfficePlace.Text, txtRPO1AppPayIssue.Text, txtRPO1AppSuppBuyer.Text);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnSaveRPO2_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDBusinessReference2(pLoanAppId, pPDDate, txtRPO2Name.Text, txtRPO2ContactNo.Text, ddlRPO2Relation.SelectedValue,
                       txtRPO2Residance.Text, ddlRPO2Occupation.SelectedValue, Convert.ToInt16(ddlRPO2NoofYears.SelectedValue), pCustType, pCreatedBy,
                       txtRPO2OfficePlace.Text, txtRPO2AppPayIssue.Text, txtRPO2AppSuppBuyer.Text);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        protected void btnQABankBehavor_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDBankBehaviour(pLoanAppId, pPDDate, txtQABankAccNo.Text, txtQABankAccType.Text, Convert.ToDouble(txtQACurrentBal.Text), Convert.ToDouble(txtQABalanceMonth1.Text)
                    , Convert.ToDouble(txtQABalanceMonth2.Text), Convert.ToDouble(txtQABalanceMonth3.Text), Convert.ToDouble(txtQATranNo1.Text), Convert.ToDouble(txtQATranNo2.Text),
                    Convert.ToDouble(txtQATranNo3.Text), Convert.ToDouble(txtQAMinChrgLst3Month.Text), txtQAChqueReturnLst3Mnth.Text, pCustType, pCreatedBy);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }

        protected void btnQASaveAggriculture_Click(object sender, EventArgs e)
        {
            if (txtPDQALnApp.Text == "")
            {
                gblFuction.AjxMsgPopup("Loan Application NoCan Not Be Blank");
                return;
            }
            string pCustID = "", pCustType = "";
            pCustID = ViewState["CustID"].ToString();
            pCustType = ViewState["CustType"].ToString();
            if (ViewState["CustID"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (ViewState["CustType"] == null)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustID) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            if (string.IsNullOrEmpty(pCustType) == true)
            {
                gblFuction.AjxMsgPopup("Please Select Question Answer Details For Entry/Update Question Answer");
                return;
            }
            string pLoanAppId = txtPDQALnApp.Text.ToString();
            DateTime pPDDate = gblFuction.setDate(txtPDQADate.Text);

            Int32 pCreatedBy = Convert.ToInt32(Session[gblValue.UserId].ToString());
            int pErr = 0;
            CMember OMem = new CMember();
            try
            {
                pErr = OMem.SavePDIncomeSourceAggriculture(pLoanAppId, pPDDate, Convert.ToDouble(txtQAAggriTotalInc.Text), ddlQAAggriIncomeFreq.SelectedValue,
                        Convert.ToInt32(ddlQAAggriLandArea.SelectedValue), ddlQAAggriSelfFarm.SelectedValue, ddlQAAggriLeased.SelectedValue, ddlQAAggriCrops.SelectedValue,
                        pCustType, pCreatedBy);
                if (pErr == 0)
                {
                    gblFuction.AjxMsgPopup("Record Save Successfully");
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Save, Data Error");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        #endregion
    }
}
