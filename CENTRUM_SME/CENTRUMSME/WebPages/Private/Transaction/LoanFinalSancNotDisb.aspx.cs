using System;
using System.Data;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Drawing;
using SendSms;

namespace CENTRUMSME.WebPages.Private.Transaction
{
    public partial class LoanFinalSancNotDisb : CENTRUMBAse
    {
        protected int cPgNo = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                hdUserID.Value = this.UserID.ToString();
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
                this.PageHeading = "Loan Final Sanction List(Not Disburse)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuFinalSancNotDisb);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                //if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Final Sanction List(Not Disburse)", false);
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
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    ClearControl();
                    break;
                case "Show":
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = true;
                    //btnDelete.Enabled = true;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    //btnAdd.Enabled = false;
                    //btnEdit.Enabled = false;
                    //btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            // ddlCust.Enabled = Status;
            // ddlSancStatus.Enabled = Status;
            // txtAppDt.Enabled = Status;
            // txtLnAmt.Enabled = Status;
            // ddlAppNo.Enabled = Status;
            // txtLnSancDate.Enabled = Status;
            // txtSancAmt.Enabled = Status;
            // ddlLnSctr.Enabled = Status;
            // ddlLnSchem.Enabled = Status;
            // txtFIntRate.Enabled = Status;
            // txtRIntRate.Enabled = Status;
            // txtInstNo.Enabled = Status;
            // txtPeriod.Enabled = Status;
            // ddlInstType.Enabled = Status;
            // txtEMI.Enabled = Status;
            // txtLPF.Enabled = Status;
            // txtLPFST.Enabled = Status;
            // txtInsuFee.Enabled = Status;
            // txtInsuST.Enabled = Status;
            // txtRepayStDt.Enabled = Status;
            //// txtSTaxAmt.Enabled = Status;
            // txtLPFKKRate.Enabled = Status;
            //// txtSTaxAmt.Enabled = Status;
            // ddlRepayType.Enabled = Status;
            // txtNetDisbAmt.Enabled = Status;
            // chkAdvEMI.Enabled = Status;
            // txtApprovedBy.Enabled = Status;
            // txtApproveDt.Enabled = Status;
            // txtRemarks.Enabled = Status;
            // txtRIntRate.Enabled = Status;
            // txtLPFKKTax.Enabled = Status;
            // txtLPFSBTax.Enabled = Status;
            // txtInsuKKTax.Enabled = Status;
            // txtInsuSBTax.Enabled = Status;
            // txtLPFPer.Enabled = Status;
            // txtSTaxPer.Enabled = Status;
            // txtLPFSBRate.Enabled = Status;
            // txtAppCharge.Enabled = Status;
            // txtStampChrge.Enabled = Status;
            // txtDisbDate.Enabled = Status;
            // txtPreEMIInt.Enabled = Status;
            // txtPreLnBal.Enabled = Status;
        }
        protected void gvLoanSanc_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    dt = oCG.GetFinalSanctionDtl(vSanId);
                    if (dt.Rows.Count > 0)
                    {
                        GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                        System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                        System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                        foreach (GridViewRow gr in gvLoanSanc.Rows)
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

                        lblBranch.Text = dt.Rows[0]["Branch"].ToString();
                        lblCustname.Text = dt.Rows[0]["CustName"].ToString();
                        lblAppName.Text = dt.Rows[0]["ApplicantName"].ToString();
                        lblSancNo.Text = dt.Rows[0]["SanctionID"].ToString();
                        lblLnAppDate.Text = dt.Rows[0]["LnAppDate"].ToString();
                        lblIniSancStatus.Text = dt.Rows[0]["SanctionStatus"].ToString();
                        lblLnAppNo.Text = dt.Rows[0]["LnNo"].ToString();
                        lblLnAppAmt.Text = dt.Rows[0]["AppliedAmt"].ToString();
                        lblIniSancDate.Text = dt.Rows[0]["SanctionDate"].ToString();
                        lblSancAmt.Text = dt.Rows[0]["SanctionAmt"].ToString();
                        lblLnPurpose.Text = dt.Rows[0]["PurposeName"].ToString();


                        lblLnScheme.Text = dt.Rows[0]["LoanTypeName"].ToString();
                        lblFIntRate.Text = dt.Rows[0]["FIntRate"].ToString();
                        lblRIntRate.Text = dt.Rows[0]["RIntRate"].ToString();
                        lblInstNo.Text = dt.Rows[0]["NoOfInstallment"].ToString();
                        lblIntType.Text = dt.Rows[0]["IntType"].ToString();
                        lblLnTenure.Text = dt.Rows[0]["Tenure"].ToString();
                        lblSTaxPer.Text = dt.Rows[0]["LPFSTRate"].ToString();
                        lblEMIAmt.Text = dt.Rows[0]["EMIAmt"].ToString();
                        lblLPFSTax.Text = dt.Rows[0]["LPFSTAmt"].ToString();
                        lblLPFPer.Text = dt.Rows[0]["LPFPer"].ToString();

                        lblRepayType.Text = dt.Rows[0]["RepayType"].ToString();
                        lblLPFAmt.Text = dt.Rows[0]["LPFAmt"].ToString();
                        lblInsuFee.Text = dt.Rows[0]["InsAmt"].ToString();
                        lblInsuFeeSTax.Text = dt.Rows[0]["InsSTAmt"].ToString();

                        lblAppChrge.Text = dt.Rows[0]["ApplCharge"].ToString();
                        lblStampChrg.Text = dt.Rows[0]["StampCharge"].ToString();
                        lblDisbDate.Text = dt.Rows[0]["DisbDate"].ToString();
                        lblFRepayDate.Text = dt.Rows[0]["RepayStartDate"].ToString();
                        lblPreEMIInt.Text = dt.Rows[0]["PreEMIInt"].ToString();
                        lblPreLnBal.Text = dt.Rows[0]["PreLnBal"].ToString();
                        lblTotChrge.Text = dt.Rows[0]["TotalCharge"].ToString();
                        lblNetDisbAmt.Text = dt.Rows[0]["NetDisbAmt"].ToString();
                        lblIniSancRemark.Text = dt.Rows[0]["Remarks"].ToString();

                        lblCGSTPer.Text = dt.Rows[0]["LPFCGSTRate"].ToString();
                        lblCGSTAmt.Text = dt.Rows[0]["LPFCGSTAmt"].ToString();
                        lblSGSTPer.Text = dt.Rows[0]["LPFSGSTRate"].ToString();
                        lblSGSTAmt.Text = dt.Rows[0]["LPFSGSTAmt"].ToString();
                        lblFLDGPer.Text = dt.Rows[0]["FLDGRate"].ToString();
                        lblFLDGAmt.Text = dt.Rows[0]["FLDGAmt"].ToString();


                        if (dt.Rows[0]["AdvEMIRcvYN"].ToString() == "Y")
                        {
                            chkAdvEMI.Checked = true;
                        }
                        else
                        {
                            chkAdvEMI.Checked = false;
                        }
                        if (dt.Rows[0]["SecurityChk1"].ToString() == "Y")
                        {
                            ChkSecurityChk1.Checked = true;
                        }
                        else
                        {
                            ChkSecurityChk1.Checked = false;
                        }
                        if (dt.Rows[0]["SecurityChk2"].ToString() == "Y")
                        {
                            ChkSecurityChk2.Checked = true;
                        }
                        else
                        {
                            ChkSecurityChk2.Checked = false;
                        }
                        if (dt.Rows[0]["SecurityChk3"].ToString() == "Y")
                        {
                            ChkSecurityChk3.Checked = true;
                        }
                        else
                        {
                            ChkSecurityChk3.Checked = false;
                        }

                        txtFApprovedBy.Text = dt.Rows[0]["FinalApprovedBy"].ToString();
                        txtFApproveDt.Text = dt.Rows[0]["FinalApprovedDt"].ToString();
                        txtFRemarks.Text = dt.Rows[0]["FinalSancRemarks"].ToString();
                        if (dt.Rows[0]["FinalSancStatus"].ToString() == "")
                        {
                            ddlFinSancStatus.SelectedIndex = -1;
                        }
                        else
                        {
                            ddlFinSancStatus.SelectedValue = dt.Rows[0]["FinalSancStatus"].ToString();
                        }
                        hdCustId.Value = dt.Rows[0]["CustID"].ToString();
                        tabLoanSanc.ActiveTabIndex = 1;
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
            tabLoanSanc.ActiveTabIndex = 0;
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
                tabLoanSanc.ActiveTabIndex = 0;
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
                Int32 vRows = 0;
                dt = oCA.GetFinalSancNotDisbList(1,ref vRows);
                if (dt.Rows.Count > 0)
                {
                    gvLoanSanc.DataSource = dt;
                    gvLoanSanc.DataBind();
                }
                else
                {
                    gvLoanSanc.DataSource = null;
                    gvLoanSanc.DataBind();
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
        private Boolean SaveRecords(string Mode)
        {
            //Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            string vLnSancId = "", vFinSanctionStatus = "", vFinalApprovedBy = "", vFinRemarks = "";
            Int32 pErr = 0;
            Int32 vErr = 0;

            if (lblIniSancDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Initial Sanction Date can not be blank...");
                return false;
            }


            if (txtFApproveDt.Text == "")
            {
                gblFuction.AjxMsgPopup("Final Approved Date can not be left blank...");
                return false;
            }

            DateTime vFinalSancDate = gblFuction.setDate(txtFApproveDt.Text);
            DateTime vFinalApprvDate = gblFuction.setDate(txtFApproveDt.Text);
            DateTime vInitilSancDate = gblFuction.setDate(lblIniSancDate.Text);
            if (vInitilSancDate > vFinalSancDate)
            {
                gblFuction.AjxMsgPopup("Final Approved Date must be greater or equal to Initial Sanction Date");
                return false;
            }
            CApplication oCA = new CApplication();
            try
            {
                if (lblSancNo.Text != "")
                {
                    vLnSancId = lblSancNo.Text.ToString();
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please Select Sanction No to update record....");
                    return false;
                }
                if (txtFApprovedBy.Text == "")
                {
                    gblFuction.AjxMsgPopup("Final Approved By can not be left blank...");
                    return false;
                }

                vFinSanctionStatus = (Request[ddlFinSancStatus.UniqueID] as string == null) ? ddlFinSancStatus.SelectedValue : Request[ddlFinSancStatus.UniqueID] as string;
                vFinalApprovedBy = (Request[txtFApprovedBy.UniqueID] as string == null) ? txtFApprovedBy.Text : Request[txtFApprovedBy.UniqueID] as string;
                vFinRemarks = (Request[txtFRemarks.UniqueID] as string == null) ? txtFRemarks.Text : Request[txtFRemarks.UniqueID] as string;

                if (Mode == "Save")
                {
                    vErr = oCA.UpdateFinalSanctionDtl(vLnSancId, vFinSanctionStatus, vFinalApprovedBy, vFinalApprvDate, vFinRemarks, pErr);
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
                oCA = null;
            }
        }
        private void ClearControl()
        {
            lblCustname.Text = "";
            lblBranch.Text = "";
            lblAppName.Text = "";
            lblSancNo.Text = "";
            lblLnAppDate.Text = "";
            lblIniSancStatus.Text = "";
            lblLnAppNo.Text = "";
            lblLnAppAmt.Text = "";
            lblIniSancDate.Text = "";
            lblSancAmt.Text = "";
            lblLnPurpose.Text = "";
            lblLnScheme.Text = "";
            lblFIntRate.Text = "";
            lblInstNo.Text = "";
            lblRIntRate.Text = "";
            lblIntType.Text = "";
            lblLnTenure.Text = "";
            lblSTaxPer.Text = "";
            lblEMIAmt.Text = "";
            lblLPFSTax.Text = "";
            lblLPFPer.Text = "";
            lblRepayType.Text = "";
            lblLPFAmt.Text = "";
            lblInsuFee.Text = "";
            lblAppChrge.Text = "";
            lblInsuFeeSTax.Text = "";
            lblDisbDate.Text = "";
            lblFRepayDate.Text = "";
            lblPreEMIInt.Text = "";
            lblStampChrg.Text = "";
            lblCGSTPer.Text = "";
            lblCGSTAmt.Text = "";
            lblSGSTPer.Text = "";
            lblSGSTAmt.Text = "";
            lblFLDGPer.Text = "";
            lblFLDGAmt.Text = "";
            lblTotChrge.Text = "";
            lblPreLnBal.Text = "";
            lblNetDisbAmt.Text = "";
            lblIniSancRemark.Text = "";
            ddlFinSancStatus.SelectedIndex = -1;
            txtFApprovedBy.Text = "";
            txtFApproveDt.Text = "";
            txtFRemarks.Text = "";
        }
        protected void lbCAMRpt_Click(object sender, EventArgs e)
        {
            string vCustId = "";
            string vLoanId = "";
            if (hdCustId.Value != "")
            {
                vCustId = hdCustId.Value.ToString();
            }
            if (lblLnAppNo.Text != "")
            {
                vLoanId = lblLnAppNo.Text.ToString();
            }
            if (vCustId == "" || vLoanId == "")
            {
                gblFuction.AjxMsgPopup("Please Select Sanction No to show CAM Details....");
                return ;
            }
            GetData(vCustId, vLoanId, "PDF");
        }
        private void GetData(string pCustid,string pLoanId,string pFormat)
        {
            string vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\CAMReport.rpt";
            string vBrCode = (string)Session[gblValue.BrnchCode];
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
            CReports oRpt = new CReports();

            try
            {
                oRpt = new CReports();
                using (ReportDocument rptDoc = new ReportDocument())
                {

                    ds = oRpt.rptCAMReport(pCustid, pLoanId);
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
                    }
                    rptDoc.Load(vRptPath);
                    rptDoc.SetDataSource(dt1);


                    rptDoc.Subreports["CoApp.rpt"].SetDataSource(dt2);
                    rptDoc.Subreports["LnRemarks.rpt"].SetDataSource(dt3);
                    rptDoc.Subreports["CIBILCheck.rpt"].SetDataSource(dt4);
                    rptDoc.Subreports["Ratio.rpt"].SetDataSource(dt5);
                    rptDoc.Subreports["Assessment.rpt"].SetDataSource(dt6);
                    rptDoc.Subreports["RefCheck.rpt"].SetDataSource(dt7);
                    rptDoc.Subreports["CAMBankInfo.rpt"].SetDataSource(dt8);
                    rptDoc.Subreports["LnSancApprove.rpt"].SetDataSource(dt9);
                    //Ratio.rpt

                    rptDoc.SetParameterValue("pCmpName", gblValue.CompName);
                    rptDoc.SetParameterValue("pAddress1", CGblIdGenerator.GetBranchAddress1(vBrCode));
                    rptDoc.SetParameterValue("pAddress2", CGblIdGenerator.GetBranchAddress2(vBrCode));
                    rptDoc.SetParameterValue("pBranch", vBrCode);
                    rptDoc.SetParameterValue("pTitle", "CAM Report");
                    if (pFormat == "PDF")
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_CAM_Report");
                    else
                        rptDoc.ExportToHttpResponse(ExportFormatType.Excel, Response, true, DateTime.Now.ToString("yyyyMMdd") + "_CAM_Report");
                    Response.ClearContent();
                    Response.ClearHeaders();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                dt2 = null;
                oRpt = null;
            }
        }
    }
}
