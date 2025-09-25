using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using System.IO;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DeviationMatrixApproval : CENTRUMBase
    {
        string vCBUrl = ConfigurationManager.AppSettings["CBUrl"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                //-----------------------------------------
                ViewState["OverDueAmount"] = "0";
                ViewState["Dose"] = "0";
                ViewState["WriteOffAmount"] = "0";
                ViewState["OwnOS"] = "0";
                ViewState["LoanAmt"] = "0";
                //----------------------------------------
                PopBranch();
                txtFrmDt.Text = txtToDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                txtDesignation.Text = Convert.ToString(Session[gblValue.Designation]);
                hdnApplFinalOblig.Value = "0";
                hdnCoApplFinalOblig.Value = "0";
                hdnAssessedIncome.Value = "0";
                hdnFinalOSInclUSFB.Value = "0";
                string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
                if (vDesig == "CM")
                {
                    //lblTLO.Visible = true;
                    //ddlTLO.Visible = true;
                    txtRemarksTLO.Visible = false;
                    lblRemTLO.Visible = false;
                    lblBUH.Visible = false;
                    ddlBUH.Visible = false;
                    lblRemBUH.Visible = false;
                    txtRemarksBUH.Visible = false;
                    ddlCH.Visible = false;
                    lblCH.Visible = false;
                    txtRemarksCH.Visible = false;
                    lblRemCH.Visible = false;
                    lblRecoAmtTLO.Visible = false;
                    txtTLORecoAmt.Visible = false;
                    lblRecoAmtBUH.Visible = false;
                    txtBUHRecoAmt.Visible = false;
                    ddlTLORemarks.Visible = false;
                    ddlBUHRemarks.Visible = false;
                    ddlCHRemarks.Visible = false;
                }
                else if (vDesig == "TLC")
                {
                    lblTLO.Visible = false;
                    ddlTLO.Visible = false;
                    //txtRemarksTLO.Visible = true;
                    //lblRemTLO.Visible = true;
                    //lblBUH.Visible = true;
                    //ddlBUH.Visible = true;
                    lblRemBUH.Visible = false;
                    txtRemarksBUH.Visible = false;
                    ddlCH.Visible = false;
                    lblCH.Visible = false;
                    txtRemarksCH.Visible = false;
                    lblRemCH.Visible = false;
                    lblRecoAmtBUH.Visible = false;
                    txtBUHRecoAmt.Visible = false;
                    ddlBUHRemarks.Visible = false;
                    ddlCHRemarks.Visible = false;
                }
                else if (vDesig == "BUH")
                {
                    lblTLO.Visible = false;
                    ddlTLO.Visible = false;
                    lblBUH.Visible = false;
                    ddlBUH.Visible = false;
                    //lblRemBUH.Visible = true;
                    //txtRemarksBUH.Visible = true;
                    //ddlCH.Visible = true;
                    //lblCH.Visible = true;
                    txtRemarksCH.Visible = false;
                    lblRemCH.Visible = false;
                    ddlCHRemarks.Visible = false;
                }
                else
                {
                    lblTLO.Visible = false;
                    ddlTLO.Visible = false;
                    lblBUH.Visible = false;
                    ddlBUH.Visible = false;
                }

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
                    ClearControls();
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
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }

        private void ClearControls()
        {
            ddlTLO.SelectedIndex = -1;
            ddlBUH.SelectedIndex = -1;
            ddlCH.SelectedIndex = -1;
            ddlOp.Items.Clear();
            txtRemarksBM.Text = "";
            txtRemarksCM.Text = "";
            txtRemarksAM.Text = "";
            txtRemarksTLO.Text = "";
            txtRemarksBUH.Text = "";
            txtRemarksCH.Text = "";
            hdnEnqId.Value = "";
            txtAmount.Text = "0";
            txtUserRecomAmt.Text = "0";
            hdnAmt.Value = "0";
            chkNonQualify.Checked = false;
            txtTLORecoAmt.Text = "0";
            txtCMRecoAmt.Text = "0";
            txtBUHRecoAmt.Text = "0";
            lblProposedUSFBEMI.Text = "0";
            ddlCMRemarks.SelectedIndex = -1;
            ddlTLORemarks.SelectedIndex = -1;
            ddlBUHRemarks.SelectedIndex = -1;
            ddlCHRemarks.SelectedIndex = -1;
        }

        private void EnableControl(Boolean Status)
        {
            FileUpload1.Enabled = false;
            FileUpload2.Enabled = false;
            FileUpload3.Enabled = false;
            FileUpload4.Enabled = false;
            FileUpload5.Enabled = false;
            txtRemarksBM.Enabled = false;
            txtRemarksAM.Enabled = false;
            ddlOp.Enabled = Status;
            txtAmount.Enabled = false;
            txtUserRecomAmt.Enabled = Status;
            gvApplOblig.Enabled = Status;
            gvCoApplOblig.Enabled = Status;
            txtRecommendtion.Enabled = Status;
            chkNonQualify.Enabled = Status;
            txtBMRecoAmt.Enabled = false;
            txtAMRecoAmt.Enabled = false;
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();

            if (vDesig == "CM")
            {
                txtRemarksCM.Enabled = Status;
                ddlTLO.Enabled = Status;
                txtCMRecoAmt.Enabled = Status;
                ddlCMRemarks.Enabled = Status;
            }
            else if (vDesig == "TLC")
            {
                ddlCMRemarks.Enabled = false;
                txtRemarksCM.Enabled = false;
                ddlBUH.Enabled = Status;
                ddlTLO.Enabled = false;
                txtRemarksTLO.Enabled = Status;
                txtTLORecoAmt.Enabled = Status;
                ddlTLORemarks.Enabled = Status;
            }
            else if (vDesig == "BUH")
            {
                ddlTLORemarks.Enabled = false;
                ddlCMRemarks.Enabled = false;
                txtRemarksTLO.Enabled = false;
                ddlBUH.Enabled = false;
                ddlTLO.Enabled = false;
                txtRemarksBUH.Enabled = Status;
                txtBUHRecoAmt.Enabled = Status;
                ddlBUHRemarks.Enabled = Status;
            }
            else if (vDesig == "R&SI")
            {
                ddlTLORemarks.Enabled = false;
                txtRemarksCM.Enabled = false;
                txtRemarksTLO.Enabled = false;
                ddlBUHRemarks.Enabled = false;
                ddlBUH.Enabled = false;
                ddlTLO.Enabled = false;
                txtRemarksBUH.Enabled = false;
                txtRemarksCH.Enabled = Status;
                ddlCHRemarks.Enabled = Status;
            }
        }

        private void InitBasePage()
        {
            try
            {
                this.Menu = false;
                this.PageHeading = "Deviation Matrix Approval";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuDeviationMatrixApproval);
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
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Deviation Matrix", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }

        private void PopBranch()
        {
            CUser oUsr = null;
            DataTable dt = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]), "R");
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            DataTable dt = null;
            CDeviationMatrix oCB = null;
            string pBranch = ddlBranch.SelectedValues.Replace("|", ",");
            string pAppMode = "";
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            pAppMode = vDesig == "CM" ? "C" : vDesig == "TLC" ? "T" : vDesig == "BUH" ? "B" : "R";
            try
            {
                oCB = new CDeviationMatrix();
                dt = oCB.GetDeviationData(gblFuction.setDate(txtFrmDt.Text), gblFuction.setDate(txtToDt.Text), pAppMode, pBranch);
                ViewState["DeviationMain"] = dt;
                gvDeviationMat.DataSource = dt;
                gvDeviationMat.DataBind();
            }
            finally
            {
                dt = null;
                oCB = null;
            }
        }

        protected void gvDeviationMat_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBranchCode = "";
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                DataTable dtMain, dtM = null;
                string vEnqId = gvRow.Cells[18].Text.Trim();
                ViewState["EnqId"] = vEnqId;
                hdnEnqId.Value = vEnqId;
                string vName = gvRow.Cells[5].Text.Trim();
                ViewState["Name"] = vName;
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[19].Text.Trim());
                double vFOIR = 0;
                ViewState["CbId"] = vCbId;
                txtRecommendtion.Text = "0";
                hdnCoApplFinalOblig.Value = "0";
                hdnApplFinalOblig.Value = "0";
                txtBMRecoAmt.Text = "0";
                txtAMRecoAmt.Text = "0";
                txtCMRecoAmt.Text = "0";
                txtTLORecoAmt.Text = "0";
                txtBUHRecoAmt.Text = "0";
                dtMain = (DataTable)ViewState["DeviationMain"];
                dtM = dtMain.Select("EnquiryId = '" + vEnqId + "' AND CBID='" + vCbId + "'").CopyToDataTable();
                ViewState["OS"] = dtM.Rows[0]["OS"].ToString().Trim();
                //-----------------------------------------------------------------------------
                vBranchCode = gvRow.Cells[4].Text.Trim();
                //-----------------------------------------------------------------------------
                foreach (GridViewRow gr in gvDeviationMat.Rows)
                {
                    LinkButton lb = (LinkButton)gr.FindControl("btnShow");
                    lb.ForeColor = System.Drawing.Color.Blue;
                }
                btnShow.ForeColor = System.Drawing.Color.Red;
                //------------------------------------------------------------------------------ 
                DataSet ds = null;
                DataTable dt, dt1, dt2, dt3 = null;
                dt = new DataTable();
                CDeviationMatrix oCDM = null;
                string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();

                if (vDesig == "CM")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "CM");
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];

                    dt3 = new DataTable();
                    oCDM = new CDeviationMatrix();
                    dt3 = oCDM.getOwnOS(vEnqId);
                    ViewState["OwnOS"] = Convert.ToString(dt3.Rows[0]["OwnOs"]);
                    ViewState["LoanAmt"] = Convert.ToString(dt3.Rows[0]["LoanAmt"]);

                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        //lblTotalOSExclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOSExcUnity")).Sum().ToString();
                        //lblFinalOSInclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();
                        //hdnFinalOSInclUSFB.Value = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();

                        lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOS.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        lblProposedUSFBEMI.Text = "0";
                        hdnProposedUSFBEMI.Value = "0";
                        //double vFOIR = (Convert.ToDouble(lblProposedUSFBEMI.Text) > 0 ? Convert.ToDouble(lblProposedUSFBEMI.Text) : 1) / (vAssessedInc > 0 ? vAssessedInc : 1);
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        txtBMRecoAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                        txtAMRecoAmt.Text = dt.Rows[0]["AMRecoAmt"].ToString();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                    }

                    gvApplOblig.DataSource = dt1;
                    gvApplOblig.DataBind();
                    ViewState["ApplOblig"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        string vAppTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvApplOblig.FooterRow.Cells[1].Text = vAppTotalEMI;
                        string vBmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        gvApplOblig.FooterRow.Cells[9].Text = vAmFinalEMI;
                        gvApplOblig.FooterRow.Cells[12].Text = "0";
                        gvApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnApplFinalOblig.Value = vAmFinalEMI;
                    }

                    gvCoApplOblig.DataSource = dt2;
                    gvCoApplOblig.DataBind();
                    ViewState["CoApplOblig"] = dt2;
                    if (dt2.Rows.Count > 0)
                    {
                        string vCoAppTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                        string vBmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        gvCoApplOblig.FooterRow.Cells[9].Text = vAmFinalEMI;
                        gvCoApplOblig.FooterRow.Cells[12].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnCoApplFinalOblig.Value = vAmFinalEMI;
                    }

                    txtRemarksBM.Text = dt.Rows[0]["BMRemarks"].ToString();
                    txtRemarksAM.Text = dt.Rows[0]["AMRemarks"].ToString();
                    txtUserRecomAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                    popRO(vBranchCode, "TLC");
                }
                else if (vDesig == "TLC")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "CM");
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];

                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOS.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        //lblProposedUSFBEMI.Text = Convert.ToString(vAssessedInc / 2);
                        lblProposedUSFBEMI.Text = "0";
                        hdnProposedUSFBEMI.Value = "0";
                        // double vFOIR = (Convert.ToDouble(lblProposedUSFBEMI.Text) > 0 ? Convert.ToDouble(lblProposedUSFBEMI.Text) : 1) / (vAssessedInc > 0 ? vAssessedInc : 1);
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        txtBMRecoAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                        txtAMRecoAmt.Text = dt.Rows[0]["AMRecoAmt"].ToString();
                        txtCMRecoAmt.Text = dt.Rows[0]["CMRecoAmt"].ToString();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                    }

                    gvApplOblig.DataSource = dt1;
                    gvApplOblig.DataBind();
                    ViewState["ApplOblig"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        string vAppTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvApplOblig.FooterRow.Cells[1].Text = vAppTotalEMI;
                        string vBmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        gvApplOblig.FooterRow.Cells[12].Text = vTLOFinalCM;
                        gvApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnApplFinalOblig.Value = vTLOFinalCM;
                    }

                    gvCoApplOblig.DataSource = dt2;
                    gvCoApplOblig.DataBind();
                    ViewState["CoApplOblig"] = dt2;
                    if (dt2.Rows.Count > 0)
                    {
                        string vCoAppTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                        string vBmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        gvCoApplOblig.FooterRow.Cells[12].Text = vTLOFinalCM;
                        gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnCoApplFinalOblig.Value = vTLOFinalCM;
                    }


                    txtRemarksBM.Text = dt.Rows[0]["BMRemarks"].ToString();
                    txtRemarksAM.Text = dt.Rows[0]["AMRemarks"].ToString();
                    txtRemarksCM.Text = dt.Rows[0]["CMRemarks"].ToString();
                    ListItem oLc = new ListItem(dt.Rows[0]["CMRemarks"].ToString().Trim(), dt.Rows[0]["CMRemarks"].ToString().Trim());
                    ddlCMRemarks.Items.Insert(0, oLc);
                    upCMRemarks.Update();
                    txtUserRecomAmt.Text = dt.Rows[0]["CMRecomAmt"].ToString();
                    popRO(vBranchCode, "BUH");
                }
                else if (vDesig == "BUH")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "BUH");
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];

                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        //lblTotalOSExclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOSExcUnity")).Sum().ToString();
                        //lblFinalOSInclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();
                        //hdnFinalOSInclUSFB.Value = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();

                        lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOS.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        lblProposedUSFBEMI.Text = "0";
                        hdnProposedUSFBEMI.Value = "0";
                        // double vFOIR = (Convert.ToDouble(lblProposedUSFBEMI.Text) > 0 ? Convert.ToDouble(lblProposedUSFBEMI.Text) : 1) / (vAssessedInc > 0 ? vAssessedInc : 1);
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        txtBMRecoAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                        txtAMRecoAmt.Text = dt.Rows[0]["AMRecoAmt"].ToString();
                        txtCMRecoAmt.Text = dt.Rows[0]["CMRecomAmt"].ToString();
                        txtTLORecoAmt.Text = dt.Rows[0]["TLORecoAmt"].ToString();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                    }

                    gvApplOblig.DataSource = dt1;
                    gvApplOblig.DataBind();
                    ViewState["ApplOblig"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        string vAppTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvApplOblig.FooterRow.Cells[1].Text = vAppTotalEMI;
                        string vBmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        string vTLOFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[12].Text = vTLOFinalEMI;
                        // gvApplOblig.FooterRow.Cells[15].Text = "0";
                        string vBUHFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[15].Text = vBUHFinalEMI;
                        hdnApplFinalOblig.Value = vBUHFinalEMI;
                    }

                    gvCoApplOblig.DataSource = dt2;
                    gvCoApplOblig.DataBind();
                    ViewState["CoApplOblig"] = dt2;
                    if (dt2.Rows.Count > 0)
                    {
                        string vCoAppTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                        string vBmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        string vTLOFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[12].Text = vTLOFinalEMI;
                        // gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                        string vBUHFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[15].Text = vBUHFinalEMI;
                        hdnCoApplFinalOblig.Value = vBUHFinalEMI;

                    }


                    txtRemarksBM.Text = dt.Rows[0]["BMRemarks"].ToString();
                    txtRemarksAM.Text = dt.Rows[0]["AMRemarks"].ToString();
                    txtRemarksCM.Text = dt.Rows[0]["CMRemarks"].ToString();
                    txtRemarksTLO.Text = dt.Rows[0]["TLORemarks"].ToString();
                    ListItem oLc = new ListItem(dt.Rows[0]["CMRemarks"].ToString().Trim(), dt.Rows[0]["CMRemarks"].ToString().Trim());
                    ddlCMRemarks.Items.Insert(0, oLc);
                    upCMRemarks.Update();
                    ListItem oLc1 = new ListItem(dt.Rows[0]["TLORemarks"].ToString().Trim(), dt.Rows[0]["TLORemarks"].ToString().Trim());
                    ddlTLORemarks.Items.Insert(0, oLc1);
                    upTLORemarks.Update();
                    txtUserRecomAmt.Text = dt.Rows[0]["TLORecoAmt"].ToString();
                    popRO(vBranchCode, "R&SI");
                }
                else if (vDesig == "R&SI")
                {
                    dt = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "CM");
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];

                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        //lblTotalOSExclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOSExcUnity")).Sum().ToString();
                        //lblFinalOSInclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();
                        //hdnFinalOSInclUSFB.Value = dt.AsEnumerable().Select(t => t.Field<double>("TotalOS")).Sum().ToString();

                        lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOS.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        lblProposedUSFBEMI.Text = "0";
                        hdnProposedUSFBEMI.Value = "0";
                        //double vFOIR = (Convert.ToDouble(lblProposedUSFBEMI.Text) > 0 ? Convert.ToDouble(lblProposedUSFBEMI.Text) : 1) / (vAssessedInc > 0 ? vAssessedInc : 1);
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        txtBMRecoAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                        txtAMRecoAmt.Text = dt.Rows[0]["AMRecoAmt"].ToString();
                        txtCMRecoAmt.Text = dt.Rows[0]["CMRecomAmt"].ToString();
                        txtTLORecoAmt.Text = dt.Rows[0]["TLORecoAmt"].ToString();
                        txtBUHRecoAmt.Text = dt.Rows[0]["BUHRecoAmt"].ToString();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                    }

                    gvApplOblig.DataSource = dt1;
                    gvApplOblig.DataBind();
                    ViewState["ApplOblig"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        string vAppTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvApplOblig.FooterRow.Cells[1].Text = vAppTotalEMI;
                        string vBmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        string vTLOFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[12].Text = vTLOFinalEMI;
                        string vBUHFinalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("FinalEMIBUH")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[15].Text = vBUHFinalEMI;
                        hdnApplFinalOblig.Value = vBUHFinalEMI;
                    }

                    gvCoApplOblig.DataSource = dt2;
                    gvCoApplOblig.DataBind();
                    ViewState["CoApplOblig"] = dt2;
                    if (dt2.Rows.Count > 0)
                    {
                        string vCoAppTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                        string vBmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIBM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[3].Text = vBmFinalEMI;
                        string vAmFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIAM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[6].Text = vAmFinalEMI;
                        string vTLOFinalCM = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMICM")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[9].Text = vTLOFinalCM;
                        string vTLOFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMITLO")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[12].Text = vTLOFinalEMI;
                        string vBUHFinalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("FinalEMIBUH")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[15].Text = vBUHFinalEMI;
                        hdnCoApplFinalOblig.Value = vBUHFinalEMI;
                    }

                    txtRemarksBM.Text = dt.Rows[0]["BMRemarks"].ToString();
                    txtRemarksAM.Text = dt.Rows[0]["AMRemarks"].ToString();
                    txtRemarksCM.Text = dt.Rows[0]["CMRemarks"].ToString();
                    txtRemarksTLO.Text = dt.Rows[0]["TLORemarks"].ToString();
                    txtRemarksBUH.Text = dt.Rows[0]["BUHRemarks"].ToString();
                    ListItem oLc = new ListItem(dt.Rows[0]["CMRemarks"].ToString().Trim(), dt.Rows[0]["CMRemarks"].ToString().Trim());
                    ddlCMRemarks.Items.Insert(0, oLc);
                    upCMRemarks.Update();
                    ListItem oLc1 = new ListItem(dt.Rows[0]["TLORemarks"].ToString().Trim(), dt.Rows[0]["TLORemarks"].ToString().Trim());
                    ddlTLORemarks.Items.Insert(0, oLc1);
                    upTLORemarks.Update();
                    ListItem oLc2 = new ListItem(dt.Rows[0]["BUHRemarks"].ToString().Trim(), dt.Rows[0]["BUHRemarks"].ToString().Trim());
                    ddlBUHRemarks.Items.Insert(0, oLc2);
                    upBUHRemarks.Update();
                    txtUserRecomAmt.Text = dt.Rows[0]["BUHRecoAmt"].ToString();
                }
                if (dt.Rows.Count > 0)
                {
                    ViewState["OverDueAmount"] = Convert.ToDouble(dt.Rows[0]["OverDueAmount"]);
                    ViewState["Dose"] = Convert.ToDouble(dt.Rows[0]["Dose"]);
                    ViewState["WriteOffAmount"] = Convert.ToDouble(dt.Rows[0]["WriteOffAmount"]);
                    //popOperation(dt.Rows[0]["WriteOff"].ToString(), Convert.ToDouble(dt.Rows[0]["OverDueAmount"]), Convert.ToInt32(dt.Rows[0]["ActiveLoan"]),
                    //    Convert.ToDouble(dt.Rows[0]["TotalOS"]), Convert.ToString(Session[gblValue.Designation]).Trim(), Convert.ToInt32(dt.Rows[0]["Dose"]),
                    //    Convert.ToInt32(dt.Rows[0]["NoOfWriteOff"]), Convert.ToDouble(dt.Rows[0]["WriteOffAmount"]));
                }
                GetOS();
                double vAssessedInc = Convert.ToDouble(hdnAssessedIncome.Value);
                vFOIR = (Convert.ToDouble(hdnCoApplFinalOblig.Value) + Convert.ToDouble(hdnApplFinalOblig.Value)) / (vAssessedInc > 0 ? vAssessedInc : 1) * 100;
                lblFOIR.Text = string.Format("{0:0.00}", vFOIR);
                tabCgt.ActiveTabIndex = 1;
                StatusButton("Show");
            }

            if (e.CommandName == "cmdCbReport")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string vEnqId = gvRow.Cells[18].Text.Trim();
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[19].Text.Trim());
                string vCBEnqDate = gvRow.Cells[20].Text.Trim();
                //SetParameterForRptData(vEnqId, vCbId, "PDF");
                string url = vCBUrl + "?vEnqId=" + vEnqId + "&vCbId=" + vCbId + "&vCBEnqDate=" + vCBEnqDate.Replace("/", "_");
                string s = "window.open('" + url + "', 'popup_window', 'width=900,height=600,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }

        private void popOperation(string pIsWriteOffYN, double pOverDueAmt, Int32 pNoOfInst, double pTotalOS, string pDesig, int pDose, int pNoOfWriteOff, double pWriteOffAmount)
        {
            ListItem oli = null;
            ddlOp.Items.Clear();
            oli = new ListItem("<--Select-->", "-1");
            //if (pIsWriteOffYN == "N")
            //{
            if (pDesig == "CM")
            {
                if (pDose == 1)
                {
                    if (pWriteOffAmount <= 5000 && pOverDueAmt <= 15000 && pTotalOS <= 250000)
                    {
                        ddlOp.Items.Insert(0, oli);
                        oli = new ListItem("Approve", "A");
                        ddlOp.Items.Insert(1, oli);
                        oli = new ListItem("Reject", "C");
                        ddlOp.Items.Insert(2, oli);
                        oli = new ListItem("Send Back", "S");
                        ddlOp.Items.Insert(3, oli);
                        oli = new ListItem("Recommendation", "R");
                        ddlOp.Items.Insert(4, oli);

                        txtAmount.Text = pDose == 0 ? "40000" : "60000";
                        hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                    }
                    else
                    {
                        ddlOp.Items.Insert(0, oli);
                        oli = new ListItem("Recommendation", "R");
                        ddlOp.Items.Insert(1, oli);
                        oli = new ListItem("Reject", "C");
                        ddlOp.Items.Insert(2, oli);
                        oli = new ListItem("Send Back", "S");
                        ddlOp.Items.Insert(3, oli);
                        txtAmount.Text = pDose == 0 ? "40000" : "60000";
                        hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                    }
                }
                else
                {
                    if (pOverDueAmt <= 15000 && pTotalOS <= 250000)//pNoOfInst <= 4 &&
                    {
                        ddlOp.Items.Insert(0, oli);
                        oli = new ListItem("Approve", "A");
                        ddlOp.Items.Insert(1, oli);
                        oli = new ListItem("Reject", "C");
                        ddlOp.Items.Insert(2, oli);
                        oli = new ListItem("Send Back", "S");
                        ddlOp.Items.Insert(3, oli);
                        oli = new ListItem("Recommendation", "R");
                        ddlOp.Items.Insert(4, oli);
                        //oli = new ListItem("Escalate", "E");
                        //ddlOp.Items.Insert(5, oli);
                        txtAmount.Text = pDose == 0 ? "40000" : "60000";
                        hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                        // txtUserRecomAmt.Text = hdnAmt.Value;
                    }
                    else
                    {
                        ddlOp.Items.Insert(0, oli);
                        oli = new ListItem("Recommendation", "R");
                        ddlOp.Items.Insert(1, oli);
                        //oli = new ListItem("Escalate", "E");
                        //ddlOp.Items.Insert(2, oli);
                        oli = new ListItem("Reject", "C");
                        ddlOp.Items.Insert(2, oli);
                        oli = new ListItem("Send Back", "S");
                        ddlOp.Items.Insert(3, oli);
                        txtAmount.Text = pDose == 0 ? "40000" : "60000";
                        hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                    }
                }
            }
            else if (pDesig == "TLC")
            {
                if (pOverDueAmt <= 25000 && pTotalOS <= 250000 && pWriteOffAmount <= 10000)//pNoOfInst <= 5 &&
                {
                    ddlOp.Items.Insert(0, oli);
                    oli = new ListItem("Approve", "A");
                    ddlOp.Items.Insert(1, oli);
                    oli = new ListItem("Reject", "C");
                    ddlOp.Items.Insert(2, oli);
                    oli = new ListItem("Send Back", "S");
                    ddlOp.Items.Insert(3, oli);
                    oli = new ListItem("Recommendation", "R");
                    ddlOp.Items.Insert(4, oli);
                    //oli = new ListItem("Escalate", "E");
                    //ddlOp.Items.Insert(5, oli);
                    txtAmount.Text = pDose == 0 ? "40000" : "60000";
                    hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                }
                else
                {
                    ddlOp.Items.Insert(0, oli);
                    oli = new ListItem("Recommendation", "R");
                    ddlOp.Items.Insert(1, oli);
                    //oli = new ListItem("Escalate", "E");
                    //ddlOp.Items.Insert(2, oli);
                    oli = new ListItem("Reject", "C");
                    ddlOp.Items.Insert(2, oli);
                    oli = new ListItem("Send Back", "S");
                    ddlOp.Items.Insert(3, oli);
                    txtAmount.Text = pDose == 0 ? "40000" : "60000";
                    hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                }
            }
            else if (pDesig == "BUH")
            {
                ddlOp.Items.Insert(0, oli);
                oli = new ListItem("Reject", "C");
                ddlOp.Items.Insert(1, oli);
                oli = new ListItem("Recommendation", "R");
                ddlOp.Items.Insert(2, oli);
                oli = new ListItem("Send Back", "S");
                ddlOp.Items.Insert(3, oli);
                txtAmount.Text = pDose == 0 ? "40000" : "60000";
                hdnAmt.Value = pDose == 0 ? "40000" : "60000";
            }
            else if (pDesig == "R&SI")
            {
                if (pTotalOS <= 250000)//pNoOfInst <= 6 &&
                {
                    ddlOp.Items.Insert(0, oli);
                    oli = new ListItem("Approve", "A");
                    ddlOp.Items.Insert(1, oli);
                    oli = new ListItem("Reject", "C");
                    ddlOp.Items.Insert(2, oli);
                    oli = new ListItem("Send Back", "S");
                    ddlOp.Items.Insert(3, oli);
                    txtAmount.Text = pDose == 0 ? "40000" : "60000";
                    hdnAmt.Value = pDose == 0 ? "40000" : "60000";
                }
            }
            // }
            //else if (pIsWriteOffYN == "Y")
            //{
            //    if (pDesig == "CM")
            //    {
            //        if (pOverDueAmt <= 4999 && pTotalOS <= 150000 && pNoOfWriteOff <= 1 && pWriteOffAmount <= 5000)//pNoOfInst <= 4 &&
            //        {
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Approve", "A");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            oli = new ListItem("Recommendation", "R");
            //            ddlOp.Items.Insert(4, oli);
            //            oli = new ListItem("Escalate", "E");
            //            ddlOp.Items.Insert(5, oli);
            //            txtAmount.Text = pDose == 0 ? "35000" : pDose == 1 ? "60000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "35000" : pDose == 1 ? "60000" : "80000";
            //            // txtUserRecomAmt.Text = hdnAmt.Value;
            //        }
            //        else
            //        {
            //            //ddlOp.Items.Insert(0, oli);
            //            //oli = new ListItem("Escalate", "E");
            //            //ddlOp.Items.Insert(1, oli);
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Recommendation", "R");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            oli = new ListItem("Escalate", "E");
            //            ddlOp.Items.Insert(4, oli);

            //            txtAmount.Text = pDose == 0 ? "35000" : pDose == 1 ? "60000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "35000" : pDose == 1 ? "60000" : "80000";
            //            //txtUserRecomAmt.Text = hdnAmt.Value;
            //        }
            //    }
            //    else if (pDesig == "TLC")
            //    {
            //        if (pOverDueAmt <= 10000 && pTotalOS <= 175000 && pNoOfWriteOff <= 2 && pWriteOffAmount <= 10000)// pNoOfInst <= 5 &&
            //        {
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Approve", "A");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            oli = new ListItem("Recommendation", "R");
            //            ddlOp.Items.Insert(4, oli);
            //            oli = new ListItem("Escalate", "E");
            //            ddlOp.Items.Insert(5, oli);

            //            txtAmount.Text = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //        }
            //        else
            //        {
            //            //ddlOp.Items.Insert(0, oli);
            //            //oli = new ListItem("Escalate", "E");
            //            //ddlOp.Items.Insert(1, oli);
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Recommendation", "R");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            oli = new ListItem("Escalate", "E");
            //            ddlOp.Items.Insert(4, oli);

            //            txtAmount.Text = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //        }
            //    }
            //    else if (pDesig == "BUH")
            //    {
            //        ddlOp.Items.Insert(0, oli);
            //        oli = new ListItem("Reject", "C");
            //        ddlOp.Items.Insert(1, oli);
            //        oli = new ListItem("Recommendation", "R");
            //        ddlOp.Items.Insert(2, oli);
            //        oli = new ListItem("Escalate", "E");
            //        ddlOp.Items.Insert(3, oli);
            //        txtAmount.Text = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //        hdnAmt.Value = pDose == 0 ? "40000" : pDose == 1 ? "60000" : "80000";
            //    }
            //    else if (pDesig == "R&SI")
            //    {
            //        if (pOverDueAmt <= 25000 && pTotalOS <= 200000 && pNoOfWriteOff <= 2 && pWriteOffAmount <= 25000)//pNoOfInst <= 6 && 
            //        {
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Approve", "A");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            txtAmount.Text = pDose == 0 ? "60000" : pDose == 1 ? "70000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "60000" : pDose == 1 ? "70000" : "80000";
            //        }
            //        else
            //        {
            //            ddlOp.Items.Insert(0, oli);
            //            oli = new ListItem("Approve", "A");
            //            ddlOp.Items.Insert(1, oli);
            //            oli = new ListItem("Reject", "C");
            //            ddlOp.Items.Insert(2, oli);
            //            oli = new ListItem("Send Back", "S");
            //            ddlOp.Items.Insert(3, oli);
            //            txtAmount.Text = pDose == 0 ? "60000" : pDose == 1 ? "70000" : "80000";
            //            hdnAmt.Value = pDose == 0 ? "60000" : pDose == 1 ? "70000" : "80000";
            //        }
            //    }
            //}
        }

        private void popRO(string pBranch, string pDesig)
        {
            DataTable dt = null;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oRO = new CEO();
            ListItem oli = new ListItem("<--Select-->", "-1");
            if (pDesig == "TLC")
            {
                oRO = new CEO();
                dt = oRO.PopAmCmByBranch(pBranch, vLogDt, pDesig);
                ddlTLO.DataSource = dt;
                ddlTLO.DataTextField = "EoName";
                ddlTLO.DataValueField = "Eoid";
                ddlTLO.DataBind();
                ddlTLO.Items.Insert(0, oli);
            }
            if (pDesig == "BUH")
            {
                oRO = new CEO();
                dt = oRO.PopAmCmByBranch(pBranch, vLogDt, pDesig);
                ddlBUH.DataSource = dt;
                ddlBUH.DataTextField = "EoName";
                ddlBUH.DataValueField = "Eoid";
                ddlBUH.DataBind();
                ddlBUH.Items.Insert(0, oli);
            }
            if (pDesig == "R&SI")
            {
                oRO = new CEO();
                dt = oRO.PopAmCmByBranch(pBranch, vLogDt, pDesig);
                ddlCH.DataSource = dt;
                ddlCH.DataTextField = "EoName";
                ddlCH.DataValueField = "Eoid";
                ddlCH.DataBind();
                // ddlCH.Items.Insert(0, oli);
            }
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
                tabCgt.ActiveTabIndex = 1;
                StatusButton("Add");
                ClearControls();
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
            tabCgt.ActiveTabIndex = 0;
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
                gblFuction.MsgPopup(gblMarg.SaveMsg);
                LoadGrid();
                StatusButton("View");
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vEnqId = Convert.ToString(ViewState["EnqId"]);
            Int32 vCBID = Convert.ToInt32(ViewState["CbId"]);
            string vMemberName = Convert.ToString(ViewState["Name"]);
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            DataTable dt, dt1 = null;
            CDeviationMatrix oCDM = null;
            int vErr = 0;
            string vEmail = "";
            string vXmlApplObligData = "", vXmlCoApplObligData = "";
            double pRecomAmt = Convert.ToDouble(txtRecommendtion.Text == "" ? "0" : txtRecommendtion.Text);

            GetAppObligData();
            GetCoAppObligData();
            dt = (DataTable)ViewState["ApplOblig"];
            dt1 = (DataTable)ViewState["CoApplOblig"];
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vXmlApplObligData = oSW.ToString();
            }
            using (StringWriter oSW = new StringWriter())
            {
                dt1.WriteXml(oSW);
                vXmlCoApplObligData = oSW.ToString();
            }

            if (ddlOp.SelectedValue == "R" || ddlOp.SelectedValue == "E")
            {
                oCDM = new CDeviationMatrix();
                if (vDesig == "CM")
                {
                    vErr = oCDM.SaveDevRecom(vEnqId, vCBID, ddlTLO.SelectedValue, ddlCMRemarks.SelectedValue, vDesig, pRecomAmt, vXmlApplObligData, vXmlCoApplObligData);
                    //vEmail = oCDM.GetEmailIdbyEoid(ddlTLO.SelectedValue);
                }
                else if (vDesig == "TLC")
                {
                    vErr = oCDM.SaveDevRecom(vEnqId, vCBID, ddlBUH.SelectedValue, ddlTLORemarks.SelectedValue, vDesig, pRecomAmt, vXmlApplObligData, vXmlCoApplObligData);
                    // vEmail = oCDM.GetEmailIdbyEoid(ddlBUH.SelectedValue);
                }
                else if (vDesig == "BUH")
                {
                    vErr = oCDM.SaveDevRecom(vEnqId, vCBID, ddlCH.SelectedValue, ddlBUHRemarks.SelectedValue, vDesig, pRecomAmt, vXmlApplObligData, vXmlCoApplObligData);
                    // vEmail = oCDM.GetEmailIdbyEoid(ddlCH.SelectedValue);
                }

                if (vErr > 0)
                {
                    vResult = true;
                    //-----------------------------Send  Mail----------------------------
                    //--CENTR - 5556
                    //oCDM = new CDeviationMatrix();
                    //if (vEmail != "")
                    //{
                    //    SendToMail(vEmail, "Enquiry Id-" + vEnqId + " name-" + vMemberName + " has been reffered to next level", "Deviation " + (ddlOp.SelectedValue == "R" ? "Recomendation" : "Esclate"));
                    //}
                    //-------------------------------------------------------------------
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            else
            {
                string vRemarks = vDesig == "CM" ? ddlCMRemarks.SelectedValue : vDesig == "TLC" ? ddlTLORemarks.SelectedValue : vDesig == "BUH" ? ddlBUHRemarks.SelectedValue : ddlCHRemarks.SelectedValue;
                double vFinalObligation = Convert.ToDouble(hdnApplFinalOblig.Value) + Convert.ToDouble(hdnCoApplFinalOblig.Value);

                oCDM = new CDeviationMatrix();
                vErr = oCDM.ApproveDeviationMatrix(vEnqId, vCBID, vRemarks, ddlOp.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]),
                    Convert.ToDouble(txtRecommendtion.Text), Convert.ToDouble(hdnProposedUSFBEMI.Value), vFinalObligation, vDesig, vXmlApplObligData, vXmlCoApplObligData);
                if (vErr > 0)
                {
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(gblMarg.DBError);
                    vResult = false;
                }
            }
            return vResult;
        }

        private String dtToXml(DataTable dt)
        {
            String vSpecXML = "";
            using (StringWriter oSW = new StringWriter())
            {
                dt.WriteXml(oSW);
                vSpecXML = oSW.ToString();
            }
            return vSpecXML;
        }

        private void DownloadFile(string vPath, string vImageName)
        {
            if (File.Exists(vPath + vImageName + ".pdf"))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + vImageName + ".pdf");
                Response.WriteFile(vPath + vImageName + ".pdf");
                Response.End();
            }
            else if (File.Exists(vPath + vImageName + ".jpg"))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + vImageName + ".jpg");
                Response.WriteFile(vPath + vImageName + ".jpg");
                Response.End();
            }
            else if (File.Exists(vPath + vImageName + ".jpeg"))
            {
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + vImageName + ".jpeg");
                Response.WriteFile(vPath + vImageName + ".jpeg");
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No Data Found..");
            }
        }

        public static void SendToMail(string pMail, string pBody, string pSubject)
        {
            string vCompEmailgmail = ConfigurationManager.AppSettings["CompEmail"];
            string vCompPwdgmail = ConfigurationManager.AppSettings["CompPwd"];
            SendEmail oBj = new SendEmail();
            try
            {
                oBj.SendToMailOffice360(pMail, pBody, pSubject, vCompEmailgmail, vCompPwdgmail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oBj = null;
            }
        }

        #region Obligation Dropdown
        protected void ddlBMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBMConsider.NamingContainer;

            if (ddlBMConsider.SelectedValue == "Y")
            {
                row.Cells[3].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[3].Text) + Convert.ToDouble(row.Cells[1].Text));
                row.Cells[17].Text = "Y";
            }
            else
            {
                gvApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[3].Text) - Convert.ToDouble(row.Cells[3].Text));
                row.Cells[3].Text = "0";
                row.Cells[17].Text = "N";
            }
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[3].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlAMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlAMConsider.NamingContainer;

            if (ddlAMConsider.SelectedValue == "Y")
            {
                row.Cells[6].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[6].Text) + Convert.ToDouble(row.Cells[1].Text));
                row.Cells[19].Text = "Y";
            }
            else
            {
                gvApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[6].Text) - Convert.ToDouble(row.Cells[6].Text));
                row.Cells[6].Text = "0";
                row.Cells[19].Text = "N";
            }
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[6].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlCMConsider.NamingContainer;
            DataTable dt, dt1 = null;
            DropDownList ddlCMRemarks = (DropDownList)row.FindControl("ddlCMRemarks");

            if (ddlCMConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlCMRemarks.DataSource = dt1;
                    ddlCMRemarks.DataTextField = "Remarks";
                    ddlCMRemarks.DataValueField = "RemId";
                    ddlCMRemarks.DataBind();
                }

                row.Cells[9].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[9].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlCMRemarks.DataSource = dt1;
                    ddlCMRemarks.DataTextField = "Remarks";
                    ddlCMRemarks.DataValueField = "RemId";
                    ddlCMRemarks.DataBind();
                }

                gvApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[9].Text) - Convert.ToDouble(row.Cells[9].Text));
                row.Cells[9].Text = "0";
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddlCMRemarks.Items.Insert(0, oLc);

            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[9].Text;
            GetOS();
            ProposedEmiCalculation();
        }

        protected void ddlTLOConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt, dt1 = null;
            DropDownList ddlTLOConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlTLOConsider.NamingContainer;
            DropDownList ddlTLORemarks = (DropDownList)row.FindControl("ddlTLORemarks");

            if (ddlTLOConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlTLORemarks.DataSource = dt1;
                    ddlTLORemarks.DataTextField = "Remarks";
                    ddlTLORemarks.DataValueField = "RemId";
                    ddlTLORemarks.DataBind();
                }
                row.Cells[12].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[12].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlTLORemarks.DataSource = dt1;
                    ddlTLORemarks.DataTextField = "Remarks";
                    ddlTLORemarks.DataValueField = "RemId";
                    ddlTLORemarks.DataBind();
                }

                gvApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[12].Text) - Convert.ToDouble(row.Cells[12].Text));
                row.Cells[12].Text = "0";
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddlTLORemarks.Items.Insert(0, oLc);
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[12].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlBUHConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt, dt1 = null;
            DropDownList ddlBUHConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBUHConsider.NamingContainer;
            DropDownList ddlBUHRemarks = (DropDownList)row.FindControl("ddlBUHRemarks");

            if (ddlBUHConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBUHRemarks.DataSource = dt1;
                    ddlBUHRemarks.DataTextField = "Remarks";
                    ddlBUHRemarks.DataValueField = "RemId";
                    ddlBUHRemarks.DataBind();
                }
                row.Cells[15].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[15].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBUHRemarks.DataSource = dt1;
                    ddlBUHRemarks.DataTextField = "Remarks";
                    ddlBUHRemarks.DataValueField = "RemId";
                    ddlBUHRemarks.DataBind();
                }

                gvApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[15].Text) - Convert.ToDouble(row.Cells[15].Text));
                row.Cells[15].Text = "0";
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddlBUHRemarks.Items.Insert(0, oLc);
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[15].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCoBMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBMConsider.NamingContainer;

            if (ddlBMConsider.SelectedValue == "Y")
            {
                row.Cells[3].Text = row.Cells[1].Text;
                gvCoApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[3].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                gvCoApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[3].Text) - Convert.ToDouble(row.Cells[3].Text));
                row.Cells[3].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[3].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCoAMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlAMConsider.NamingContainer;

            if (ddlAMConsider.SelectedValue == "Y")
            {
                row.Cells[6].Text = row.Cells[1].Text;
                gvCoApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[6].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                gvCoApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[6].Text) - Convert.ToDouble(row.Cells[6].Text));
                row.Cells[6].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[6].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCoCMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt, dt1 = null;
            DropDownList ddlCMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlCMConsider.NamingContainer;
            DropDownList ddlCMRemarks = (DropDownList)row.FindControl("ddlCMRemarks");

            if (ddlCMConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlCMRemarks.DataSource = dt1;
                    ddlCMRemarks.DataTextField = "Remarks";
                    ddlCMRemarks.DataValueField = "RemId";
                    ddlCMRemarks.DataBind();
                }

                row.Cells[9].Text = row.Cells[1].Text;
                gvCoApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[9].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlCMRemarks.DataSource = dt1;
                    ddlCMRemarks.DataTextField = "Remarks";
                    ddlCMRemarks.DataValueField = "RemId";
                    ddlCMRemarks.DataBind();
                }
                ListItem oLc = new ListItem("<--Select-->", "-1");
                ddlCMRemarks.Items.Insert(0, oLc);
                gvCoApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[9].Text) - Convert.ToDouble(row.Cells[9].Text));
                row.Cells[9].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[9].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCoTLOConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt, dt1 = null;
            DropDownList ddlTLOConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlTLOConsider.NamingContainer;
            DropDownList ddlTLORemarks = (DropDownList)row.FindControl("ddlTLORemarks");

            if (ddlTLOConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlTLORemarks.DataSource = dt1;
                    ddlTLORemarks.DataTextField = "Remarks";
                    ddlTLORemarks.DataValueField = "RemId";
                    ddlTLORemarks.DataBind();
                }

                row.Cells[12].Text = row.Cells[1].Text;
                gvCoApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[12].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlTLORemarks.DataSource = dt1;
                    ddlTLORemarks.DataTextField = "Remarks";
                    ddlTLORemarks.DataValueField = "RemId";
                    ddlTLORemarks.DataBind();
                }

                gvCoApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[12].Text) - Convert.ToDouble(row.Cells[12].Text));
                row.Cells[12].Text = "0";
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddlTLORemarks.Items.Insert(0, oLc);
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[12].Text;
            GetOS();
            ProposedEmiCalculation();

        }

        protected void ddlCoBUHConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt, dt1 = null;
            DropDownList ddlBUHConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBUHConsider.NamingContainer;
            DropDownList ddlBUHRemarks = (DropDownList)row.FindControl("ddlBUHRemarks");


            if (ddlBUHConsider.SelectedValue == "Y")
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBUHRemarks.DataSource = dt1;
                    ddlBUHRemarks.DataTextField = "Remarks";
                    ddlBUHRemarks.DataValueField = "RemId";
                    ddlBUHRemarks.DataBind();
                }

                row.Cells[15].Text = row.Cells[1].Text;
                gvCoApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[15].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                dt = (DataTable)ViewState["Remarks"];
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBUHRemarks.DataSource = dt1;
                    ddlBUHRemarks.DataTextField = "Remarks";
                    ddlBUHRemarks.DataValueField = "RemId";
                    ddlBUHRemarks.DataBind();
                }

                gvCoApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[15].Text) - Convert.ToDouble(row.Cells[15].Text));
                row.Cells[15].Text = "0";
            }
            ListItem oLc = new ListItem("<--Select-->", "-1");
            ddlBUHRemarks.Items.Insert(0, oLc);
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[15].Text;
            GetOS();
            ProposedEmiCalculation();

        }
        #endregion

        protected void gvApplOblig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt, dt1 = null;
            CGblIdGenerator oGb = null;
           // string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //---------------------------------------------------------------------------
                    DropDownList ddlBMRemarks = (DropDownList)e.Row.FindControl("ddlBMRemarks");
                    DropDownList ddlAMRemarks = (DropDownList)e.Row.FindControl("ddlAMRemarks");
                    DropDownList ddlCMRemarks = (DropDownList)e.Row.FindControl("ddlCMRemarks");
                    DropDownList ddlTLORemarks = (DropDownList)e.Row.FindControl("ddlTLORemarks");
                    DropDownList ddlBUHRemarks = (DropDownList)e.Row.FindControl("ddlBUHRemarks");

                    DropDownList ddlAMConsider = (DropDownList)e.Row.FindControl("ddlAMConsider");
                    DropDownList ddlCMConsider = (DropDownList)e.Row.FindControl("ddlCMConsider");
                    DropDownList ddlTLOConsider = (DropDownList)e.Row.FindControl("ddlTLOConsider");
                    DropDownList ddlBUHConsider = (DropDownList)e.Row.FindControl("ddlBUHConsider");
                    DropDownList ddlBMConsider = (DropDownList)e.Row.FindControl("ddlBMConsider");

                    ddlBMRemarks.Items.Clear();
                    ddlAMRemarks.Items.Clear();
                    ddlCMRemarks.Items.Clear();
                    ddlTLORemarks.Items.Clear();
                    ddlBUHRemarks.Items.Clear();

                    oGb = new CGblIdGenerator();
                    dt = oGb.popDeviationRemarks();
                    ViewState["Remarks"] = dt;

                    if (dt.Rows.Count > 0)
                    {
                        ddlBMRemarks.DataSource = ddlAMRemarks.DataSource = dt;
                        ddlBMRemarks.DataTextField = "Remarks";
                        ddlBMRemarks.DataValueField = "RemId";
                        ddlBMRemarks.DataBind();

                        ddlAMRemarks.DataTextField = "Remarks";
                        ddlAMRemarks.DataValueField = "RemId";
                        ddlAMRemarks.DataBind();

                    }
                    ListItem oLc = null;
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBMRemarks.Items.Insert(0, oLc);
                    ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                    ddlBMRemarks.SelectedIndex = ddlBMRemarks.Items.IndexOf(ddlBMRemarks.Items.FindByValue(e.Row.Cells[18].Text.Trim()));


                    oLc = new ListItem("<--Select-->", "-1");
                    ddlAMRemarks.Items.Insert(0, oLc);
                    ddlAMConsider.SelectedIndex = ddlAMConsider.Items.IndexOf(ddlAMConsider.Items.FindByValue(e.Row.Cells[19].Text.Trim()));
                    ddlAMRemarks.SelectedIndex = ddlAMRemarks.Items.IndexOf(ddlAMRemarks.Items.FindByValue(e.Row.Cells[20].Text.Trim()));

                    if (vDesig == "CM")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[19].Text.Trim() + "'").CopyToDataTable();
                        ddlCMRemarks.DataSource = dt1;
                        ddlCMRemarks.DataTextField = "Remarks";
                        ddlCMRemarks.DataValueField = "RemId";
                        ddlCMRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlCMRemarks.Items.Insert(0, oLc);
                        ddlCMConsider.SelectedIndex = ddlCMConsider.Items.IndexOf(ddlCMConsider.Items.FindByValue(e.Row.Cells[19].Text.Trim()));
                        ddlCMRemarks.SelectedIndex = ddlCMRemarks.Items.IndexOf(ddlCMRemarks.Items.FindByValue(e.Row.Cells[22].Text.Trim()));
                        e.Row.Cells[9].Text = ddlCMConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[21].Text.Trim() + "'").CopyToDataTable();
                        ddlCMRemarks.DataSource = dt1;
                        ddlCMRemarks.DataTextField = "Remarks";
                        ddlCMRemarks.DataValueField = "RemId";
                        ddlCMRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlCMRemarks.Items.Insert(0, oLc);
                        ddlCMConsider.SelectedIndex = ddlCMConsider.Items.IndexOf(ddlCMConsider.Items.FindByValue(e.Row.Cells[21].Text.Trim()));
                        ddlCMRemarks.SelectedIndex = ddlCMRemarks.Items.IndexOf(ddlCMRemarks.Items.FindByValue(e.Row.Cells[22].Text.Trim()));
                        e.Row.Cells[9].Text = ddlCMConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    if (vDesig == "TLC")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[21].Text.Trim() + "'").CopyToDataTable();
                        ddlTLORemarks.DataSource = dt1;
                        ddlTLORemarks.DataTextField = "Remarks";
                        ddlTLORemarks.DataValueField = "RemId";
                        ddlTLORemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlTLORemarks.Items.Insert(0, oLc);
                        ddlTLOConsider.SelectedIndex = ddlTLOConsider.Items.IndexOf(ddlTLOConsider.Items.FindByValue(e.Row.Cells[21].Text.Trim()));
                        ddlTLORemarks.SelectedIndex = ddlTLORemarks.Items.IndexOf(ddlTLORemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[12].Text = ddlTLOConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[23].Text.Trim() + "'").CopyToDataTable();
                        ddlTLORemarks.DataSource = dt1;
                        ddlTLORemarks.DataTextField = "Remarks";
                        ddlTLORemarks.DataValueField = "RemId";
                        ddlTLORemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlTLORemarks.Items.Insert(0, oLc);
                        ddlTLOConsider.SelectedIndex = ddlTLOConsider.Items.IndexOf(ddlTLOConsider.Items.FindByValue(e.Row.Cells[23].Text.Trim()));
                        ddlTLORemarks.SelectedIndex = ddlTLORemarks.Items.IndexOf(ddlTLORemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[12].Text = ddlTLOConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    //------------------------------------22.08.2022(phone call-Amarjeet)----------------------
                    if (vDesig == "BUH")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[23].Text.Trim() + "'").CopyToDataTable();
                        ddlBUHRemarks.DataSource = dt1;
                        ddlBUHRemarks.DataTextField = "Remarks";
                        ddlBUHRemarks.DataValueField = "RemId";
                        ddlBUHRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlBUHRemarks.Items.Insert(0, oLc);
                        ddlBUHConsider.SelectedIndex = ddlBUHConsider.Items.IndexOf(ddlBUHConsider.Items.FindByValue(e.Row.Cells[23].Text.Trim()));
                        ddlBUHRemarks.SelectedIndex = ddlBUHRemarks.Items.IndexOf(ddlBUHRemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[15].Text = ddlBUHConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[25].Text.Trim() + "'").CopyToDataTable();
                        ddlBUHRemarks.DataSource = dt1;
                        ddlBUHRemarks.DataTextField = "Remarks";
                        ddlBUHRemarks.DataValueField = "RemId";
                        ddlBUHRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlBUHRemarks.Items.Insert(0, oLc);
                        ddlBUHConsider.SelectedIndex = ddlBUHConsider.Items.IndexOf(ddlBUHConsider.Items.FindByValue(e.Row.Cells[25].Text.Trim()));
                        ddlBUHRemarks.SelectedIndex = ddlBUHRemarks.Items.IndexOf(ddlBUHRemarks.Items.FindByValue(e.Row.Cells[26].Text.Trim()));
                        e.Row.Cells[15].Text = ddlBUHConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    //-----------------------------------------------------------------------------

                    if (vDesig == "CM")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = true;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = true;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        gvApplOblig.Columns[11].Visible = false;
                        gvApplOblig.Columns[12].Visible = false;
                        gvApplOblig.Columns[13].Visible = false;
                        gvApplOblig.Columns[14].Visible = false;
                        gvApplOblig.Columns[15].Visible = false;
                        gvApplOblig.Columns[16].Visible = false;
                    }
                    else if (vDesig == "TLC")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = true;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = true;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        gvApplOblig.Columns[14].Visible = false;
                        gvApplOblig.Columns[15].Visible = false;
                        gvApplOblig.Columns[16].Visible = false;
                    }
                    else if (vDesig == "BUH")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = true;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = true;
                        ddlBMConsider.Enabled = false;
                    }
                    else if (vDesig == "R&SI")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        ddlBUHConsider.Enabled = true;
                        ddlBUHRemarks.Enabled = true;
                    }
                    else
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;
                    }

                    //---------------------------------------------------------------------------                                      
                }
            }
            finally
            {

            }
        }

        protected void gvCoApplOblig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt, dt1 = null;
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //---------------------------------------------------------------------------
                    DropDownList ddlBMRemarks = (DropDownList)e.Row.FindControl("ddlBMRemarks");
                    DropDownList ddlAMRemarks = (DropDownList)e.Row.FindControl("ddlAMRemarks");
                    DropDownList ddlCMRemarks = (DropDownList)e.Row.FindControl("ddlCMRemarks");
                    DropDownList ddlTLORemarks = (DropDownList)e.Row.FindControl("ddlTLORemarks");
                    DropDownList ddlBUHRemarks = (DropDownList)e.Row.FindControl("ddlBUHRemarks");

                    DropDownList ddlAMConsider = (DropDownList)e.Row.FindControl("ddlAMConsider");
                    DropDownList ddlCMConsider = (DropDownList)e.Row.FindControl("ddlCMConsider");
                    DropDownList ddlTLOConsider = (DropDownList)e.Row.FindControl("ddlTLOConsider");
                    DropDownList ddlBUHConsider = (DropDownList)e.Row.FindControl("ddlBUHConsider");
                    DropDownList ddlBMConsider = (DropDownList)e.Row.FindControl("ddlBMConsider");

                    ddlBMRemarks.Items.Clear();
                    ddlAMRemarks.Items.Clear();
                    ddlCMRemarks.Items.Clear();
                    ddlTLORemarks.Items.Clear();
                    ddlBUHRemarks.Items.Clear();

                    //oGb = new CGblIdGenerator();
                    //dt = oGb.popDeviationRemarks();
                    dt = (DataTable)ViewState["Remarks"];
                    if (dt.Rows.Count > 0)
                    {
                        ddlBMRemarks.DataSource = ddlAMRemarks.DataSource = dt;
                        ddlBMRemarks.DataTextField = "Remarks";
                        ddlBMRemarks.DataValueField = "RemId";
                        ddlBMRemarks.DataBind();

                        ddlAMRemarks.DataTextField = "Remarks";
                        ddlAMRemarks.DataValueField = "RemId";
                        ddlAMRemarks.DataBind();
                    }
                    ListItem oLc = null;
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBMRemarks.Items.Insert(0, oLc);
                    ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                    ddlBMRemarks.SelectedIndex = ddlBMRemarks.Items.IndexOf(ddlBMRemarks.Items.FindByValue(e.Row.Cells[18].Text.Trim()));


                    oLc = new ListItem("<--Select-->", "-1");
                    ddlAMRemarks.Items.Insert(0, oLc);
                    ddlAMConsider.SelectedIndex = ddlAMConsider.Items.IndexOf(ddlAMConsider.Items.FindByValue(e.Row.Cells[19].Text.Trim()));
                    ddlAMRemarks.SelectedIndex = ddlAMRemarks.Items.IndexOf(ddlAMRemarks.Items.FindByValue(e.Row.Cells[20].Text.Trim()));
                    ///-----------------------------------------------------------------------------------------------------------------
                    if (vDesig == "CM")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[19].Text.Trim() + "'").CopyToDataTable();
                        ddlCMRemarks.DataSource = dt1;
                        ddlCMRemarks.DataTextField = "Remarks";
                        ddlCMRemarks.DataValueField = "RemId";
                        ddlCMRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlCMRemarks.Items.Insert(0, oLc);
                        ddlCMConsider.SelectedIndex = ddlCMConsider.Items.IndexOf(ddlCMConsider.Items.FindByValue(e.Row.Cells[19].Text.Trim()));
                        ddlCMRemarks.SelectedIndex = ddlCMRemarks.Items.IndexOf(ddlCMRemarks.Items.FindByValue(e.Row.Cells[22].Text.Trim()));
                        e.Row.Cells[9].Text = ddlCMConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[21].Text.Trim() + "'").CopyToDataTable();
                        ddlCMRemarks.DataSource = dt1;
                        ddlCMRemarks.DataTextField = "Remarks";
                        ddlCMRemarks.DataValueField = "RemId";
                        ddlCMRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlCMRemarks.Items.Insert(0, oLc);
                        ddlCMConsider.SelectedIndex = ddlCMConsider.Items.IndexOf(ddlCMConsider.Items.FindByValue(e.Row.Cells[21].Text.Trim()));
                        ddlCMRemarks.SelectedIndex = ddlCMRemarks.Items.IndexOf(ddlCMRemarks.Items.FindByValue(e.Row.Cells[22].Text.Trim()));
                        e.Row.Cells[9].Text = ddlCMConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    if (vDesig == "TLC")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[21].Text.Trim() + "'").CopyToDataTable();
                        ddlTLORemarks.DataSource = dt1;
                        ddlTLORemarks.DataTextField = "Remarks";
                        ddlTLORemarks.DataValueField = "RemId";
                        ddlTLORemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlTLORemarks.Items.Insert(0, oLc);
                        ddlTLOConsider.SelectedIndex = ddlTLOConsider.Items.IndexOf(ddlTLOConsider.Items.FindByValue(e.Row.Cells[21].Text.Trim()));
                        ddlTLORemarks.SelectedIndex = ddlTLORemarks.Items.IndexOf(ddlTLORemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[12].Text = ddlTLOConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[23].Text.Trim() + "'").CopyToDataTable();
                        ddlTLORemarks.DataSource = dt1;
                        ddlTLORemarks.DataTextField = "Remarks";
                        ddlTLORemarks.DataValueField = "RemId";
                        ddlTLORemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlTLORemarks.Items.Insert(0, oLc);
                        ddlTLOConsider.SelectedIndex = ddlTLOConsider.Items.IndexOf(ddlTLOConsider.Items.FindByValue(e.Row.Cells[23].Text.Trim()));
                        ddlTLORemarks.SelectedIndex = ddlTLORemarks.Items.IndexOf(ddlTLORemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[12].Text = ddlTLOConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    //------------------------------------22.08.2022(phone call-Amarjeet)----------------------
                    if (vDesig == "BUH")
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[23].Text.Trim() + "'").CopyToDataTable();
                        ddlBUHRemarks.DataSource = dt1;
                        ddlBUHRemarks.DataTextField = "Remarks";
                        ddlBUHRemarks.DataValueField = "RemId";
                        ddlBUHRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlBUHRemarks.Items.Insert(0, oLc);
                        ddlBUHConsider.SelectedIndex = ddlBUHConsider.Items.IndexOf(ddlBUHConsider.Items.FindByValue(e.Row.Cells[23].Text.Trim()));
                        ddlBUHRemarks.SelectedIndex = ddlBUHRemarks.Items.IndexOf(ddlBUHRemarks.Items.FindByValue(e.Row.Cells[24].Text.Trim()));
                        e.Row.Cells[15].Text = ddlBUHConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }
                    else
                    {
                        dt1 = dt.Select("Category = '" + e.Row.Cells[25].Text.Trim() + "'").CopyToDataTable();
                        ddlBUHRemarks.DataSource = dt1;
                        ddlBUHRemarks.DataTextField = "Remarks";
                        ddlBUHRemarks.DataValueField = "RemId";
                        ddlBUHRemarks.DataBind();

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlBUHRemarks.Items.Insert(0, oLc);
                        ddlBUHConsider.SelectedIndex = ddlBUHConsider.Items.IndexOf(ddlBUHConsider.Items.FindByValue(e.Row.Cells[25].Text.Trim()));
                        ddlBUHRemarks.SelectedIndex = ddlBUHRemarks.Items.IndexOf(ddlBUHRemarks.Items.FindByValue(e.Row.Cells[26].Text.Trim()));
                        e.Row.Cells[15].Text = ddlBUHConsider.SelectedValue == "Y" ? e.Row.Cells[1].Text : "0";
                    }

                    if (vDesig == "CM")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = true;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = true;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        gvCoApplOblig.Columns[11].Visible = false;
                        gvCoApplOblig.Columns[12].Visible = false;
                        gvCoApplOblig.Columns[13].Visible = false;
                        gvCoApplOblig.Columns[14].Visible = false;
                        gvCoApplOblig.Columns[15].Visible = false;
                        gvCoApplOblig.Columns[16].Visible = false;
                    }
                    else if (vDesig == "TLC")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = true;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = true;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        gvCoApplOblig.Columns[14].Visible = false;
                        gvCoApplOblig.Columns[15].Visible = false;
                        gvCoApplOblig.Columns[16].Visible = false;
                    }
                    else if (vDesig == "BUH")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = true;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = true;
                        ddlBMConsider.Enabled = false;
                    }
                    else if (vDesig == "R&SI")
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        ddlBUHConsider.Enabled = true;
                        ddlBUHRemarks.Enabled = true;
                    }
                    else
                    {
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                    }
                    //---------------------------------------------------------------------------                                        
                }
            }
            finally
            {

            }

        }

        protected void gvDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdCbReport")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string vEnqId = gvRow.Cells[15].Text.Trim();
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[16].Text.Trim());
                string vCBEnqDate = gvRow.Cells[17].Text.Trim();
                string vApplicantType = gvRow.Cells[0].Text.Trim();
                string url = vCBUrl + "?vEnqId=" + vEnqId + "&vCbId=" + vCbId + "&vCBEnqDate=" + vCBEnqDate.Replace("/", "_") + "&vMemberType=" + vApplicantType;
                // string url = vCBUrl + "?vEnqId=" + vEnqId + "&vCbId=" + vCbId + "&vCBEnqDate=" + vCBEnqDate.Replace("/", "_");
                string s = "window.open('" + url + "', '_blank', 'width=900,height=600,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }
        }

        private void ProposedEmiCalculation()
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "calcFOIR()", true);
        }

        public void GetAppObligData()
        {
            DataTable dt = (DataTable)ViewState["ApplOblig"];
            foreach (GridViewRow gr in gvApplOblig.Rows)
            {
                DropDownList ddlBMConsider = (DropDownList)gr.FindControl("ddlBMConsider");
                DropDownList ddlBMRemarks = (DropDownList)gr.FindControl("ddlBMRemarks");
                DropDownList ddlAMConsider = (DropDownList)gr.FindControl("ddlAMConsider");
                DropDownList ddlAMRemarks = (DropDownList)gr.FindControl("ddlAMRemarks");
                DropDownList ddlCMConsider = (DropDownList)gr.FindControl("ddlCMConsider");
                DropDownList ddlCMRemarks = (DropDownList)gr.FindControl("ddlCMRemarks");
                DropDownList ddlTLOConsider = (DropDownList)gr.FindControl("ddlTLOConsider");
                DropDownList ddlTLORemarks = (DropDownList)gr.FindControl("ddlTLORemarks");
                DropDownList ddlBUHConsider = (DropDownList)gr.FindControl("ddlBUHConsider");
                DropDownList ddlBUHRemarks = (DropDownList)gr.FindControl("ddlBUHRemarks");


                dt.Rows[gr.RowIndex]["ConsiderBM"] = ddlBMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBM"] = ddlBMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderAM"] = ddlAMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksAM"] = ddlAMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderCM"] = ddlCMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksCM"] = ddlCMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderTLO"] = ddlTLOConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksTLO"] = ddlTLORemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderBUH"] = ddlBUHConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBUH"] = ddlBUHRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["FinalEMIBM"] = gr.Cells[3].Text;
                dt.Rows[gr.RowIndex]["FinalEMIAM"] = gr.Cells[6].Text;
                dt.Rows[gr.RowIndex]["FinalEMICM"] = dt.Rows[gr.RowIndex]["FinalEMICM"] = gr.Cells[9].Text == "" ? "0" : gr.Cells[9].Text;
                dt.Rows[gr.RowIndex]["FinalEMITLO"] = dt.Rows[gr.RowIndex]["FinalEMITLO"] = gr.Cells[12].Text == "" ? "0" : gr.Cells[12].Text;
                dt.Rows[gr.RowIndex]["FinalEMIBUH"] = dt.Rows[gr.RowIndex]["FinalEMIBUH"] = gr.Cells[15].Text == "" ? "0" : gr.Cells[15].Text;
            }
        }

        public void GetCoAppObligData()
        {
            DataTable dt = (DataTable)ViewState["CoApplOblig"];
            foreach (GridViewRow gr in gvCoApplOblig.Rows)
            {
                DropDownList ddlBMConsider = (DropDownList)gr.FindControl("ddlBMConsider");
                DropDownList ddlBMRemarks = (DropDownList)gr.FindControl("ddlBMRemarks");
                DropDownList ddlAMConsider = (DropDownList)gr.FindControl("ddlAMConsider");
                DropDownList ddlAMRemarks = (DropDownList)gr.FindControl("ddlAMRemarks");
                DropDownList ddlCMConsider = (DropDownList)gr.FindControl("ddlCMConsider");
                DropDownList ddlCMRemarks = (DropDownList)gr.FindControl("ddlCMRemarks");
                DropDownList ddlTLOConsider = (DropDownList)gr.FindControl("ddlTLOConsider");
                DropDownList ddlTLORemarks = (DropDownList)gr.FindControl("ddlTLORemarks");
                DropDownList ddlBUHConsider = (DropDownList)gr.FindControl("ddlBUHConsider");
                DropDownList ddlBUHRemarks = (DropDownList)gr.FindControl("ddlBUHRemarks");

                dt.Rows[gr.RowIndex]["ConsiderBM"] = ddlBMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBM"] = ddlBMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderAM"] = ddlAMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksAM"] = ddlAMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderCM"] = ddlCMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksCM"] = ddlCMRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderTLO"] = ddlTLOConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksTLO"] = ddlTLORemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["ConsiderBUH"] = ddlBUHConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBUH"] = ddlBUHRemarks.SelectedValue;

                dt.Rows[gr.RowIndex]["FinalEMIBM"] = gr.Cells[3].Text;
                dt.Rows[gr.RowIndex]["FinalEMIAM"] = gr.Cells[6].Text;
                dt.Rows[gr.RowIndex]["FinalEMICM"] = dt.Rows[gr.RowIndex]["FinalEMICM"] = gr.Cells[9].Text == "" ? "0" : gr.Cells[9].Text;
                dt.Rows[gr.RowIndex]["FinalEMITLO"] = dt.Rows[gr.RowIndex]["FinalEMITLO"] = gr.Cells[12].Text == "" ? "0" : gr.Cells[12].Text;
                dt.Rows[gr.RowIndex]["FinalEMIBUH"] = dt.Rows[gr.RowIndex]["FinalEMIBUH"] = gr.Cells[15].Text == "" ? "0" : gr.Cells[15].Text;
            }
        }

        protected void ddlOp_SelectedIndexChanged(object sender, EventArgs e)
        {
           // string vDesig = txtDesignation.Text;
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            var CancelReason = new List<string> { "Highly Overleveraged", "Poor Track Record", "High Overdue" };
            var RecomReason = new List<string> { "Clear repayment history with NIL Overdue", "Satisfactory RTR of all loans incl. existing USFB loan", "Satisfactory overall repayment history except few instances of Overdue in other loan RTR", "W/Off or Overdue is old. Clear repayment seen in recent times" };
            ListItem oli = new ListItem("<--Select-->", "-1");
            if (vDesig == "CM")
            {
                ddlCMRemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R" || ddlOp.SelectedValue == "A")
                {
                    ddlCMRemarks.DataSource = RecomReason;
                    ddlCMRemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlCMRemarks.DataSource = CancelReason;
                    ddlCMRemarks.DataBind();
                }
                ddlCMRemarks.Items.Insert(0, oli);
            }
            if (vDesig == "TLC")
            {
                ddlTLORemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R" || ddlOp.SelectedValue == "A")
                {
                    ddlTLORemarks.DataSource = RecomReason;
                    ddlTLORemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlTLORemarks.DataSource = CancelReason;
                    ddlTLORemarks.DataBind();
                }
                ddlTLORemarks.Items.Insert(0, oli);
            }
            if (vDesig == "BUH")
            {
                ddlBUHRemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R" || ddlOp.SelectedValue == "A")
                {
                    ddlBUHRemarks.DataSource = RecomReason;
                    ddlBUHRemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlBUHRemarks.DataSource = CancelReason;
                    ddlBUHRemarks.DataBind();
                }
                ddlBUHRemarks.Items.Insert(0, oli);
            }
            if (vDesig == "R&SI")
            {
                ddlCHRemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R" || ddlOp.SelectedValue == "A")
                {
                    ddlCHRemarks.DataSource = RecomReason;
                    ddlCHRemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlCHRemarks.DataSource = CancelReason;
                    ddlCHRemarks.DataBind();
                }
                ddlCHRemarks.Items.Insert(0, oli);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "VisibleHide()", true);
        }

        protected void txtRecommendtion_TextChanged(object sender, EventArgs e)
        {
            string pOverDueAmt = ViewState["OverDueAmount"].ToString();
            string vDose = ViewState["Dose"].ToString();
            string vWriteOffAmt = ViewState["WriteOffAmount"].ToString();
            string vCbEnqDate = ViewState["CBEnqDate"].ToString();
            string pTotalOS = hdnFinalOS.Value;
           // string pDesig = txtDesignation.Text;
            string pDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            string vRecoAmt = txtRecommendtion.Text == "" ? "0" : txtRecommendtion.Text;
            double pOS = Convert.ToDouble(pTotalOS) + Convert.ToDouble(vRecoAmt);
            double pOwnOS = Convert.ToDouble(ViewState["OwnOS"].ToString());
            double vOldLoanAmt = Convert.ToDouble(ViewState["LoanAmt"].ToString());

            bool vPOSPassYN = (vOldLoanAmt <= 25000 && pOwnOS <= 5000) ? true : (vOldLoanAmt > 25000 && pOwnOS <= 10000) ? true : false;


            ddlOp.Items.Clear();
            Dictionary<string, string> Decision = new Dictionary<string, string>();
            Decision.Add("<--Select-->", "-1");
            Decision.Add("Reject", "C");
            Decision.Add("Send Back", "S");

            string vAmt = Convert.ToDouble(vDose) == 0 ? "40000" : "60000";
            txtAmount.Text = vAmt;
            hdnAmt.Value = vAmt;

            if (Convert.ToDouble(vRecoAmt) > Convert.ToDouble(txtAmount.Text))
            {
                //  Decision.Add("Recommendation", "R");
                gblFuction.AjxMsgPopup("Recomended amount greater than system recomended amount.");
                txtRecommendtion.Focus();
            }
            //else 
            if (pDesig == "CM")
            {
                if (gblFuction.setDate(vCbEnqDate) < gblFuction.setDate("05/02/2025"))
                {
                    if (Convert.ToDouble(vDose) == 0 && Convert.ToDouble(vWriteOffAmt) <= 5000.00 && Convert.ToDouble(pOverDueAmt) <= 5000 && Convert.ToDouble(pOS) <= 250000.00)
                    {
                        Decision.Add("Approve", "A");
                    }
                    else if (Convert.ToDouble(vDose) > 0 && Convert.ToDouble(pOverDueAmt) <= 15000.00 && Convert.ToDouble(pOS) <= 250000.00)
                    {
                        Decision.Add("Approve", "A");
                    }
                    Decision.Add("Recommendation", "R");
                }
                else
                {
                    string InsType = "", vInstitution = "";
                    double pOD = 0, pActiveMFI = 0, pIndebtedness = 0;
                    List<string> Instution = new List<string>();
                    foreach (GridViewRow gr in gvApplOblig.Rows)
                    {
                        if (gr.RowType == DataControlRowType.DataRow)
                        {
                            DropDownList ddlCMConsider = (DropDownList)gr.FindControl("ddlCMConsider");
                            Label lblInstitution = (Label)gr.FindControl("lblInstitution");
                            if (ddlCMConsider.SelectedValue == "Y")
                            {
                                InsType = gr.Cells[29].Text;
                                vInstitution = lblInstitution.Text;
                                if (InsType == "MFI" || InsType == "RUS")
                                {
                                    pOD = pOD + Convert.ToDouble(gr.Cells[28].Text);
                                    pIndebtedness = pIndebtedness + Convert.ToDouble(gr.Cells[27].Text);
                                }
                                if (InsType == "MFI" && !vInstitution.Contains("Unity"))
                                {
                                    //pActiveMFI = pActiveMFI + 1;
                                    Instution.Add(vInstitution);
                                }
                            }
                        }
                    }
                    List<string> DistinctInstitution = Instution.Distinct().ToList();
                    pActiveMFI = DistinctInstitution.Count;
                    pIndebtedness = pIndebtedness + pOwnOS;
                    if (Convert.ToDouble(vDose) == 0 && pActiveMFI < 3 && Convert.ToDouble(pOD) <= 1000.00 && Convert.ToDouble(pIndebtedness) <= 200000.00 && vPOSPassYN == true)
                    {
                        Decision.Add("Approve", "A");
                    }
                    else if (Convert.ToDouble(vDose) > 0 && pActiveMFI < 3 && Convert.ToDouble(pOD) <= 2500.00 && Convert.ToDouble(pIndebtedness) <= 200000.00)
                    {
                        Decision.Add("Approve", "A");
                    }
                    else
                    {
                        Decision.Add("Recommendation", "R");
                    }
                }
            }
            else if (pDesig == "TLC")
            {
                if (Convert.ToDouble(pOverDueAmt) <= 25000.00 && Convert.ToDouble(pOS) <= 250000.00 && vPOSPassYN == true)
                {
                    Decision.Add("Approve", "A");
                }
                Decision.Add("Recommendation", "R");
            }
            else if (pDesig == "BUH")
            {
                Decision.Add("Recommendation", "R");
            }
            else if (pDesig == "R&SI")
            {
                if (Convert.ToDouble(pOS) <= 250000.00)
                {
                    Decision.Add("Approve", "A");
                }
                // Decision.Add("Recommendation", "R");
            }
            //else
            //{
            //    Decision.Add("Recommendation", "R");
            //}
            ddlOp.DataSource = Decision;
            ddlOp.DataTextField = "Key";
            ddlOp.DataValueField = "Value";
            ddlOp.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "Recommendtion()", true);
        }

        public void GetOS()
        {
            double vTotalOS = 0, vTotalOSExcUnity = 0;
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            string vDDlName = vDesig == "CM" ? "ddlCMConsider" : vDesig == "TLC" ? "ddlTLOConsider" : vDesig == "BUH" ? "ddlBUHConsider" : "ddlBUHConsider";
            string InsType = "";
            string vCbEnqDate = ViewState["CBEnqDate"].ToString();

            foreach (GridViewRow gr in gvApplOblig.Rows)
            {
                DropDownList ddlConsider = (DropDownList)gr.FindControl(vDDlName);
                Label txtInstitution = (Label)gr.FindControl("lblInstitution");
                if (ddlConsider.SelectedValue == "Y")
                {
                    if (gblFuction.setDate(vCbEnqDate) < gblFuction.setDate("05/02/2025"))
                    {
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                        {
                            vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[27].Text);

                        }
                        vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[27].Text);
                    }
                    else
                    {
                        InsType = gr.Cells[29].Text;
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (InsType == "MFI" || InsType == "RUS")
                        {
                            if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                            {
                                vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[27].Text);
                            }
                            vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[27].Text);
                        }
                    }
                }
            }
            foreach (GridViewRow gr in gvCoApplOblig.Rows)
            {
                DropDownList ddlConsider = (DropDownList)gr.FindControl(vDDlName);
                Label txtInstitution = (Label)gr.FindControl("lblInstitution");
                if (ddlConsider.SelectedValue == "Y")
                {
                    //string vInstitute = txtInstitution.Text.Trim().ToUpper();
                    //if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                    //{
                    //    vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[27].Text);
                    //}
                    if (gblFuction.setDate(vCbEnqDate) < gblFuction.setDate("05/02/2025"))
                    {
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                        {
                            vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[27].Text);
                        }
                        vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[27].Text);
                    }
                    else
                    {
                        InsType = gr.Cells[29].Text;
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (InsType == "MFI" || InsType == "RUS")
                        {
                            if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                            {
                                vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[27].Text);
                            }
                            vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[27].Text);
                        }
                    }
                }
            }
            // vTotalOS = (vTotalOSExcUnity - Convert.ToDouble(ViewState["OS"].ToString()));
            // vTotalOS = vTotalOSExcUnity;
            lblTotalOSExclUSFB.Text = vTotalOSExcUnity.ToString("0.00");
            upTotalOSExclUnity.Update();
            lblFinalOSInclUSFB.Text = vTotalOS.ToString("0.00");
            hdnFinalOSInclUSFB.Value = vTotalOS.ToString("0.00");
            hdnFinalOS.Value = vTotalOS.ToString("0.00");
            upFinalOSInclUSFB.Update();
            txtRecommendtion.Text = "0.00";
            uptxtRecoAmt.Update();
        }

    }

}