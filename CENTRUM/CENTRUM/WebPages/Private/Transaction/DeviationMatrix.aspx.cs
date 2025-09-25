using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FORCECA;
using System.Data;
using FORCEBA;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;

namespace CENTRUM.WebPages.Private.Transaction
{
    public partial class DeviationMatrix : CENTRUMBase
    {
        string vPathDeviation = ConfigurationManager.AppSettings["PathDeviation"];
        Int32 vPathDeviationMaxLength = Convert.ToInt32(ConfigurationManager.AppSettings["PathDeviationMaxLength"]);
        string vCBUrl = ConfigurationManager.AppSettings["CBUrl"];
        string vCoAppCBUrl = ConfigurationManager.AppSettings["CoAppCBUrl"];

        string DeviationDocBucket = ConfigurationManager.AppSettings["DeviationDocBucket"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["Dose"] = "0";
                PopBranch();
                txtFrmDt.Text = txtToDt.Text = Session[gblValue.LoginDate].ToString();
                StatusButton("View");
                string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
                //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
                txtDesignation.Text = vDesig;
                hdnApplFinalOblig.Value = "0";
                hdnCoApplFinalOblig.Value = "0";
                hdnAssessedIncome.Value = "0";
                hdnFinalOSInclUSFB.Value = "0";
                if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
                {
                    lblCM.Visible = false;
                    ddlCM.Visible = false;
                    ddlAM.Visible = true;
                    lblAM.Visible = true;
                    txtRemarksAM.Visible = false;
                    ddlRAMRemarks.Visible = false;
                    lblRemAM.Visible = false;
                    txtAMRecoAmt.Visible = false;
                    lblRecoAmtAM.Visible = false;
                }
                else if (vDesig == "ARM" || vDesig == "SH")
                {
                    lblCM.Visible = true;
                    ddlCM.Visible = true;
                    ddlAM.Visible = false;
                    lblAM.Visible = false;
                    txtRemarksAM.Visible = true;
                    ddlRAMRemarks.Visible = true;
                    lblRemAM.Visible = true;
                    txtAMRecoAmt.Visible = true;
                    lblRecoAmtAM.Visible = true;
                }
                else if (vDesig == "CM")
                {
                    lblCM.Visible = false;
                    ddlCM.Visible = false;
                    ddlAM.Visible = false;
                    lblAM.Visible = false;
                    txtRemarksAM.Visible = true;
                    lblRemAM.Visible = true;
                }
                else if (vDesig == "TLO")
                {
                    lblCM.Visible = false;
                    ddlCM.Visible = false;
                    ddlAM.Visible = false;
                    lblAM.Visible = false;
                    txtRemarksAM.Visible = true;
                    lblRemAM.Visible = true;
                }
                else if (vDesig == "TLO")
                {
                    lblCM.Visible = false;
                    ddlCM.Visible = false;
                    ddlAM.Visible = false;
                    lblAM.Visible = false;
                    txtRemarksAM.Visible = true;
                    lblRemAM.Visible = true;
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
            ddlAM.SelectedIndex = -1;
            ddlCM.SelectedIndex = -1;
            txtRemarksBM.Text = "";
            txtRemarksAM.Text = "";
            hdnEnqId.Value = "";
            ddlOp.Items.Clear();
            hdnApplFinalOblig.Value = "0";
            hdnCoApplFinalOblig.Value = "0";
            hdnAssessedIncome.Value = "0";
            hdnFinalOSInclUSFB.Value = "0";
            txtRecommendtion.Text = "0";
            chkNonQualify.Checked = false;
            txtBMRecoAmt.Text = "0";
            txtAMRecoAmt.Text = "0";
            ddlRBMRemarks.SelectedIndex = -1;
            ddlRAMRemarks.SelectedIndex = -1;
            lblProposedUSFBEMI.Text = "0";
        }

        private void EnableControl(Boolean Status)
        {
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
            {
                FileUpload1.Enabled = Status;
                FileUpload2.Enabled = Status;
                FileUpload3.Enabled = Status;
                FileUpload4.Enabled = Status;
                FileUpload5.Enabled = Status;
                ImgBtnDown1.Enabled = false;
                ImgBtnDown2.Enabled = false;
                ImgBtnDown3.Enabled = false;
                ImgBtnDown4.Enabled = false;
                ImgBtnDown5.Enabled = false;
                ddlAM.Enabled = Status;
                txtRemarksBM.Enabled = Status;
                txtBMRecoAmt.Enabled = Status;
                ddlRBMRemarks.Enabled = Status;
            }
            else if (vDesig == "ARM" || vDesig == "SH")
            {
                ddlAM.Enabled = false;
                ddlCM.Enabled = Status;
                txtRemarksAM.Enabled = Status;
                FileUpload1.Enabled = false;
                FileUpload2.Enabled = false;
                FileUpload3.Enabled = false;
                FileUpload4.Enabled = false;
                FileUpload5.Enabled = false;
                txtRemarksBM.Enabled = false;
                txtBMRecoAmt.Enabled = false;
                ddlRBMRemarks.Enabled = false;
                ddlRAMRemarks.Enabled = Status;
            }
            gvDtl.Enabled = Status;
            gvApplOblig.Enabled = Status;
            gvCoApplOblig.Enabled = Status;
            txtRecommendtion.Enabled = Status;
            ddlOp.Enabled = Status;
            chkNonQualify.Enabled = Status;
        }

        private void InitBasePage()
        {
            DataTable DtBrCntrl = null;
            try
            {
                this.Menu = false;
                this.PageHeading = "Deviation Matrix";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString();
                this.GetModuleByRole(mnuID.mnuDeviationMatrix);

                if (Session["BrCntrl"] != null)
                {
                    DtBrCntrl = (DataTable)Session["BrCntrl"];
                    if (Convert.ToString(DtBrCntrl.Rows[0]["DeviationJLG"]) == "N")
                    {
                        //gblFuction.AjxMsgPopup("Pre DB is not allowed in this Branch");
                        Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Deviation Matrix", false);
                    }
                }

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
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            pAppMode = (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM") ? "N" : (vDesig == "ARM" || vDesig == "SH") ? "A" : "N";
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
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                DataTable dtMain, dtM = null;
                string vName = gvRow.Cells[5].Text.Trim();
                string vEnqId = gvRow.Cells[18].Text.Trim();
                ViewState["EnqId"] = vEnqId;
                hdnEnqId.Value = vEnqId;
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[19].Text.Trim());
                ViewState["CbId"] = vCbId;
                ViewState["Name"] = vName;
                txtRecommendtion.Text = "0";
                txtAMRecoAmt.Text = "0";
                txtBMRecoAmt.Text = "0";
                dtMain = (DataTable)ViewState["DeviationMain"];
                dtM = dtMain.Select("EnquiryId = '" + vEnqId + "' AND CBID='" + vCbId + "'").CopyToDataTable();
                gvDeviationMatView.DataSource = dtM;
                gvDeviationMatView.DataBind();
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
                DataTable dt, dt1, dt2 = null;
                CDeviationMatrix oCDM = null;
                if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
                {
                    dt = new DataTable();
                    dt1 = new DataTable();
                    dt2 = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "BM");
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    ViewState["DeviationDtl"] = dt;
                    gvDtl.DataSource = dt;
                    gvDtl.DataBind();

                    if (dt.Rows.Count > 0)
                    {
                        //lblTotalOSExclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<decimal>("TotalOSExcUnity")).Sum().ToString();
                        //lblFinalOSInclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<decimal>("TotalOS")).Sum().ToString();
                        //hdnFinalOSInclUSFB.Value = dt.AsEnumerable().Select(t => t.Field<decimal>("TotalOS")).Sum().ToString();
                        //lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        lblTotalOSExclUSFB.Text = dt.AsEnumerable().Select(t => t.Field<decimal>("TotalOSExcUnity")).Sum().ToString();
                        lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        lblProposedUSFBEMI.Text = "0";
                        hdnProposedUSFBEMI.Value = "0";
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        hdnNonQualify.Value = dt.Rows[0]["AssetType"].ToString().Trim();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                        ViewState["Dose"] = Convert.ToDouble(dt.Rows[0]["Dose"]);
                    }

                    gvApplOblig.DataSource = dt1;
                    gvApplOblig.DataBind();
                    ViewState["ApplOblig"] = dt1;
                    if (dt1.Rows.Count > 0)
                    {
                        string vAppTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvApplOblig.FooterRow.Cells[1].Text = vAppTotalEMI;
                        gvApplOblig.FooterRow.Cells[3].Text = vAppTotalEMI;
                        gvApplOblig.FooterRow.Cells[6].Text = "0";
                        gvApplOblig.FooterRow.Cells[9].Text = "0";
                        gvApplOblig.FooterRow.Cells[12].Text = "0";
                        gvApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnApplFinalOblig.Value = vAppTotalEMI;
                    }

                    gvCoApplOblig.DataSource = dt2;
                    gvCoApplOblig.DataBind();
                    ViewState["CoApplOblig"] = dt2;
                    if (dt2.Rows.Count > 0)
                    {
                        string vCoAppTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                        gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                        gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                        gvCoApplOblig.FooterRow.Cells[3].Text = vCoAppTotalEMI;
                        gvCoApplOblig.FooterRow.Cells[6].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[9].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[12].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnCoApplFinalOblig.Value = vCoAppTotalEMI;
                    }
                    // ProposedEmiCalculation();
                    upAppEMI.Update();
                    popRO(vBranchCode, "AM");
                }
                else if (vDesig == "ARM" || vDesig == "SH")
                {
                    dt = new DataTable();
                    dt1 = new DataTable();
                    dt2 = new DataTable();
                    oCDM = new CDeviationMatrix();
                    ds = oCDM.GetDeviationDataById(vEnqId, vCbId, "AM");
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

                        // lblTotalOSExclUSFB.Text = dt.Rows[0]["TotalOSExcUnity"].ToString();
                        //lblFinalOSInclUSFB.Text = dt.Rows[0]["TotalOS"].ToString();
                        // hdnFinalOSInclUSFB.Value = dt.Rows[0]["TotalOS"].ToString();

                        hdnAssessedIncome.Value = dt.AsEnumerable().Select(t => t.Field<double>("Inc")).Sum().ToString();
                        txtBMRecoAmt.Text = dt.Rows[0]["BMRecoAmt"].ToString();
                        chkNonQualify.Checked = dt.Rows[0]["AssetType"].ToString().Trim() == "Q" ? false : true;
                        hdnNonQualify.Value = dt.Rows[0]["AssetType"].ToString().Trim();
                        ViewState["CBEnqDate"] = dt.Rows[0]["CBEnqDate"].ToString();
                        ViewState["Dose"] = Convert.ToDouble(dt.Rows[0]["Dose"]);
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
                        gvApplOblig.FooterRow.Cells[6].Text = vBmFinalEMI;
                        gvApplOblig.FooterRow.Cells[9].Text = "0";
                        gvApplOblig.FooterRow.Cells[12].Text = "0";
                        gvApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnApplFinalOblig.Value = vBmFinalEMI;
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
                        gvCoApplOblig.FooterRow.Cells[6].Text = vBmFinalEMI;
                        gvCoApplOblig.FooterRow.Cells[9].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[12].Text = "0";
                        gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                        hdnCoApplFinalOblig.Value = vBmFinalEMI;
                    }
                    ListItem oLc = new ListItem(dt.Rows[0]["BMRemarks"].ToString().Trim(), dt.Rows[0]["BMRemarks"].ToString().Trim());
                    ddlRBMRemarks.Items.Insert(0, oLc);
                    txtRemarksBM.Text = dt.Rows[0]["BMRemarks"].ToString();
                    // ProposedEmiCalculation();                  
                    popRO(vBranchCode, "CM");
                }
                else
                {
                    gblFuction.MsgPopup("You do not have permission to perform this operation.");
                    return;
                }
                GetOS();
                double vAssessedInc = Convert.ToDouble(hdnAssessedIncome.Value);
                double vFOIR = (Convert.ToDouble(hdnCoApplFinalOblig.Value) + Convert.ToDouble(hdnApplFinalOblig.Value)) / (vAssessedInc > 0 ? vAssessedInc : 1) * 100;
                lblFOIR.Text = string.Format("{0:0.00}", vFOIR);

                popOperation(vDesig);
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

        #region Obligation Dropdown
        protected void ddlBMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtOblig = (DataTable)ViewState["ApplOblig"];
            DropDownList ddlBMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBMConsider.NamingContainer;
            int curRow = row.RowIndex;

            CGblIdGenerator oGb = null;
            DataTable dt, dt1 = null;
            DropDownList ddlBMRemarks = (DropDownList)row.FindControl("ddlBMRemarks");
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");

            if (txtInstallmentAmt.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter EMI Amount.");
                return;
            }
            if (ddlBMConsider.SelectedValue == "Y")
            {
                dtOblig.Rows[curRow][4] = txtInstallmentAmt.Text;
                dtOblig.Rows[curRow][6] = "Y";
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBMRemarks.DataSource = dt1;
                    ddlBMRemarks.DataTextField = "Remarks";
                    ddlBMRemarks.DataValueField = "RemId";
                    ddlBMRemarks.DataBind();
                }
                row.Cells[3].Text = txtInstallmentAmt.Text;
                gvApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[3].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
                row.Cells[17].Text = "Y";
                row.Cells[6].Text = "0";
                txtInstallmentAmt.Enabled = false;
            }
            else
            {
                dtOblig.Rows[curRow][4] = "0";
                dtOblig.Rows[curRow][6] = "N";
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBMRemarks.DataSource = dt1;
                    ddlBMRemarks.DataTextField = "Remarks";
                    ddlBMRemarks.DataValueField = "RemId";
                    ddlBMRemarks.DataBind();
                }
                gvApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[3].Text) - Convert.ToDouble(row.Cells[3].Text));
                row.Cells[3].Text = "0";
                row.Cells[6].Text = "0";
                row.Cells[17].Text = "N";
                txtInstallmentAmt.Enabled = true;
            }
            dtOblig.AcceptChanges();
            ViewState["ApplOblig"] = dtOblig;
            ListItem oLc = null;
            oLc = new ListItem("<--Select-->", "-1");
            ddlBMRemarks.Items.Insert(0, oLc);
            GetOS();
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[3].Text;
            upAppEMI.Update();
            ProposedEmiCalculation();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "RecAmtZero()", true);
        }

        protected void ddlAMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtOblig = (DataTable)ViewState["ApplOblig"];
            DropDownList ddlAMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlAMConsider.NamingContainer;
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");
            int curRow = row.RowIndex;

            CGblIdGenerator oGb = null;
            DataTable dt, dt1 = null;
            DropDownList ddlAMRemarks = (DropDownList)row.FindControl("ddlAMRemarks");

            if (txtInstallmentAmt.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter EMI Amount.");
                return;
            }
            if (ddlAMConsider.SelectedValue == "Y")
            {
                dtOblig.Rows[curRow][10] = txtInstallmentAmt.Text;
                dtOblig.Rows[curRow][9] = "Y";
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlAMRemarks.DataSource = dt1;
                    ddlAMRemarks.DataTextField = "Remarks";
                    ddlAMRemarks.DataValueField = "RemId";
                    ddlAMRemarks.DataBind();
                }
                row.Cells[6].Text = txtInstallmentAmt.Text;
                gvApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[6].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
                row.Cells[19].Text = "Y";
                txtInstallmentAmt.Enabled = false;
            }
            else
            {
                dtOblig.Rows[curRow][10] = "0";
                dtOblig.Rows[curRow][9] = "N";
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlAMRemarks.DataSource = dt1;
                    ddlAMRemarks.DataTextField = "Remarks";
                    ddlAMRemarks.DataValueField = "RemId";
                    ddlAMRemarks.DataBind();
                }
                gvApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[6].Text) - Convert.ToDouble(row.Cells[6].Text));
                row.Cells[6].Text = "0";
                row.Cells[19].Text = "N";
                txtInstallmentAmt.Enabled = true;
            }
            dtOblig.AcceptChanges();
            ViewState["ApplOblig"] = dtOblig;
            ListItem oLc = null;
            oLc = new ListItem("<--Select-->", "-1");
            ddlAMRemarks.Items.Insert(0, oLc);

            GetOS();
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[6].Text;
            upAppEMI.Update();
            ProposedEmiCalculation();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "RecAmtZero()", true);
        }

        protected void ddlCMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlCMConsider.NamingContainer;

            if (ddlCMConsider.SelectedValue == "Y")
            {
                row.Cells[9].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[9].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                gvApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[9].Text) - Convert.ToDouble(row.Cells[9].Text));
                row.Cells[9].Text = "0";
            }
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[9].Text;
            ProposedEmiCalculation();
        }

        protected void ddlTLOConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlTLOConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlTLOConsider.NamingContainer;

            if (ddlTLOConsider.SelectedValue == "Y")
            {
                row.Cells[12].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[12].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                gvApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[12].Text) - Convert.ToDouble(row.Cells[12].Text));
                row.Cells[12].Text = "0";
            }
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[12].Text;
            ProposedEmiCalculation();
        }

        protected void ddlBUHConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBUHConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBUHConsider.NamingContainer;
            if (ddlBUHConsider.SelectedValue == "Y")
            {
                row.Cells[15].Text = row.Cells[1].Text;
                gvApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[15].Text) + Convert.ToDouble(row.Cells[1].Text));
            }
            else
            {
                gvApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvApplOblig.FooterRow.Cells[15].Text) - Convert.ToDouble(row.Cells[15].Text));
                row.Cells[15].Text = "0";
            }
            hdnApplFinalOblig.Value = gvApplOblig.FooterRow.Cells[15].Text;
            ProposedEmiCalculation();
        }

        protected void ddlCoBMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            CGblIdGenerator oGb = null;
            DataTable dt, dt1 = null;
            DropDownList ddlBMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBMConsider.NamingContainer;
            DropDownList ddlBMRemarks = (DropDownList)row.FindControl("ddlBMRemarks");
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");
            if (txtInstallmentAmt.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter EMI Amount.");
                return;
            }
            if (ddlBMConsider.SelectedValue == "Y")
            {
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBMRemarks.DataSource = dt1;
                    ddlBMRemarks.DataTextField = "Remarks";
                    ddlBMRemarks.DataValueField = "RemId";
                    ddlBMRemarks.DataBind();
                }
                row.Cells[3].Text = txtInstallmentAmt.Text;
                gvCoApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[3].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
                row.Cells[6].Text = "0";
                txtInstallmentAmt.Enabled = false;
            }
            else
            {
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlBMRemarks.DataSource = dt1;
                    ddlBMRemarks.DataTextField = "Remarks";
                    ddlBMRemarks.DataValueField = "RemId";
                    ddlBMRemarks.DataBind();
                }

                gvCoApplOblig.FooterRow.Cells[3].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[3].Text) - Convert.ToDouble(row.Cells[3].Text));
                row.Cells[3].Text = "0";
                row.Cells[6].Text = "0";
                txtInstallmentAmt.Enabled = true;
            }
            ListItem oLc = null;
            oLc = new ListItem("<--Select-->", "-1");
            ddlBMRemarks.Items.Insert(0, oLc);
            GetOS();
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[3].Text;
            upCoAppEMI.Update();
            ProposedEmiCalculation();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "RecAmtZero()", true);
        }

        protected void ddlCoAMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlAMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlAMConsider.NamingContainer;
            CGblIdGenerator oGb = null;
            DataTable dt, dt1 = null;
            DropDownList ddlAMRemarks = (DropDownList)row.FindControl("ddlAMRemarks");
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");

            if (txtInstallmentAmt.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Enter EMI Amount.");
                return;
            }
            if (ddlAMConsider.SelectedValue == "Y")
            {
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'Y'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlAMRemarks.DataSource = dt1;
                    ddlAMRemarks.DataTextField = "Remarks";
                    ddlAMRemarks.DataValueField = "RemId";
                    ddlAMRemarks.DataBind();
                }
                row.Cells[6].Text = txtInstallmentAmt.Text;
                gvCoApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[6].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
                txtInstallmentAmt.Enabled = false;
            }
            else
            {
                oGb = new CGblIdGenerator();
                dt = oGb.popDeviationRemarks();
                dt1 = dt.Select("Category = 'N'").CopyToDataTable();
                if (dt1.Rows.Count > 0)
                {
                    ddlAMRemarks.DataSource = dt1;
                    ddlAMRemarks.DataTextField = "Remarks";
                    ddlAMRemarks.DataValueField = "RemId";
                    ddlAMRemarks.DataBind();
                }
                gvCoApplOblig.FooterRow.Cells[6].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[6].Text) - Convert.ToDouble(row.Cells[6].Text));
                row.Cells[6].Text = "0";
                txtInstallmentAmt.Enabled = true;
            }
            ListItem oLc = null;
            oLc = new ListItem("<--Select-->", "-1");
            ddlAMRemarks.Items.Insert(0, oLc);

            GetOS();
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[6].Text;
            upCoAppEMI.Update();
            ProposedEmiCalculation();
        }

        protected void ddlCoCMConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlCMConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlCMConsider.NamingContainer;
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");

            if (ddlCMConsider.SelectedValue == "Y")
            {
                row.Cells[9].Text = txtInstallmentAmt.Text;
                gvCoApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[9].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
            }
            else
            {
                gvCoApplOblig.FooterRow.Cells[9].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[9].Text) - Convert.ToDouble(row.Cells[9].Text));
                row.Cells[9].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[9].Text;
            ProposedEmiCalculation();
        }

        protected void ddlCoTLOConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlTLOConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlTLOConsider.NamingContainer;
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");

            if (ddlTLOConsider.SelectedValue == "Y")
            {
                row.Cells[12].Text = txtInstallmentAmt.Text;
                gvCoApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[12].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
            }
            else
            {
                gvCoApplOblig.FooterRow.Cells[12].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[12].Text) - Convert.ToDouble(row.Cells[12].Text));
                row.Cells[12].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[12].Text;
            ProposedEmiCalculation();
        }

        protected void ddlCoBUHConsider_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBUHConsider = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddlBUHConsider.NamingContainer;
            TextBox txtInstallmentAmt = (TextBox)row.FindControl("txtInstallmentAmt");

            if (ddlBUHConsider.SelectedValue == "Y")
            {
                row.Cells[15].Text = txtInstallmentAmt.Text;
                gvCoApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[15].Text) + Convert.ToDouble(txtInstallmentAmt.Text));
            }
            else
            {
                gvCoApplOblig.FooterRow.Cells[15].Text = Convert.ToString(Convert.ToDouble(gvCoApplOblig.FooterRow.Cells[15].Text) - Convert.ToDouble(row.Cells[15].Text));
                row.Cells[15].Text = "0";
            }
            hdnCoApplFinalOblig.Value = gvCoApplOblig.FooterRow.Cells[15].Text;
            ProposedEmiCalculation();
        }
        #endregion

        protected void gvApplOblig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt, dt1 = null;
            CGblIdGenerator oGb = null;
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
                    TextBox txtInstallmentAmt = (TextBox)e.Row.FindControl("txtInstallmentAmt");

                    ddlBMRemarks.Items.Clear();
                    ddlAMRemarks.Items.Clear();
                    ddlCMRemarks.Items.Clear();
                    ddlTLORemarks.Items.Clear();
                    ddlBUHRemarks.Items.Clear();
                    txtInstallmentAmt.Enabled = false;

                    oGb = new CGblIdGenerator();
                    dt = oGb.popDeviationRemarks();
                    dt1 = dt.Select("Category = '" + e.Row.Cells[17].Text.Trim() + "'").CopyToDataTable();
                    if (dt1.Rows.Count > 0)
                    {
                        ddlBMRemarks.DataSource = dt1;
                        ddlBMRemarks.DataTextField = "Remarks";
                        ddlBMRemarks.DataValueField = "RemId";
                        ddlBMRemarks.DataBind();
                    }


                    ListItem oLc = null;
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlCMRemarks.Items.Insert(0, oLc);
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlTLORemarks.Items.Insert(0, oLc);
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBUHRemarks.Items.Insert(0, oLc);

                    if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
                    {
                        ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = true;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = true;
                        e.Row.Cells[3].Text = e.Row.Cells[17].Text.Trim() == "Y" ? txtInstallmentAmt.Text : "0";
                        // e.Row.Cells[17].Text = "Y";

                        gvApplOblig.Columns[5].Visible = false;
                        gvApplOblig.Columns[6].Visible = false;
                        gvApplOblig.Columns[7].Visible = false;
                        gvApplOblig.Columns[8].Visible = false;
                        gvApplOblig.Columns[9].Visible = false;
                        gvApplOblig.Columns[10].Visible = false;
                        gvApplOblig.Columns[11].Visible = false;
                        gvApplOblig.Columns[12].Visible = false;
                        gvApplOblig.Columns[13].Visible = false;
                        gvApplOblig.Columns[14].Visible = false;
                        gvApplOblig.Columns[15].Visible = false;
                        gvApplOblig.Columns[16].Visible = false;
                    }
                    else if (vDesig == "ARM" || vDesig == "SH")
                    {
                        ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                        ddlAMConsider.SelectedIndex = ddlAMConsider.Items.IndexOf(ddlAMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = true;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;

                        e.Row.Cells[6].Text = e.Row.Cells[17].Text.Trim() == "Y" ? txtInstallmentAmt.Text : "0";
                        gvApplOblig.Columns[8].Visible = false;
                        gvApplOblig.Columns[9].Visible = false;
                        gvApplOblig.Columns[10].Visible = false;
                        gvApplOblig.Columns[11].Visible = false;
                        gvApplOblig.Columns[12].Visible = false;
                        gvApplOblig.Columns[13].Visible = false;
                        gvApplOblig.Columns[14].Visible = false;
                        gvApplOblig.Columns[15].Visible = false;
                        gvApplOblig.Columns[16].Visible = false;
                        gvApplOblig.Columns[21].Visible = false;
                        gvApplOblig.Columns[22].Visible = false;

                        dt = oGb.popDeviationRemarks();
                        dt1 = dt.Select("Category = '" + e.Row.Cells[17].Text.Trim() + "'").CopyToDataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            ddlAMRemarks.DataSource = dt1;
                            ddlAMRemarks.DataTextField = "Remarks";
                            ddlAMRemarks.DataValueField = "RemId";
                            ddlAMRemarks.DataBind();
                        }

                        ddlAMRemarks.Enabled = true;
                    }
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBMRemarks.Items.Insert(0, oLc);
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlAMRemarks.Items.Insert(0, oLc);
                    ddlBMRemarks.SelectedIndex = ddlBMRemarks.Items.IndexOf(ddlBMRemarks.Items.FindByValue(e.Row.Cells[18].Text.Trim()));

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
            CGblIdGenerator oGb = null;
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
                    TextBox txtInstallmentAmt = (TextBox)e.Row.FindControl("txtInstallmentAmt");

                    ddlBMRemarks.Items.Clear();
                    ddlAMRemarks.Items.Clear();
                    ddlCMRemarks.Items.Clear();
                    ddlTLORemarks.Items.Clear();
                    ddlBUHRemarks.Items.Clear();

                    oGb = new CGblIdGenerator();
                    dt = oGb.popDeviationRemarks();
                    dt1 = dt.Select("Category = '" + e.Row.Cells[17].Text.Trim() + "'").CopyToDataTable();
                    if (dt1.Rows.Count > 0)
                    {
                        ddlBMRemarks.DataSource = dt1;
                        ddlBMRemarks.DataTextField = "Remarks";
                        ddlBMRemarks.DataValueField = "RemId";
                        ddlBMRemarks.DataBind();
                    }
                    ListItem oLc = null;
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBMRemarks.Items.Insert(0, oLc);
                    ddlBMRemarks.SelectedIndex = ddlBMRemarks.Items.IndexOf(ddlBMRemarks.Items.FindByValue(e.Row.Cells[18].Text.Trim()));

                    oLc = new ListItem("<--Select-->", "-1");
                    ddlCMRemarks.Items.Insert(0, oLc);
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlTLORemarks.Items.Insert(0, oLc);
                    oLc = new ListItem("<--Select-->", "-1");
                    ddlBUHRemarks.Items.Insert(0, oLc);
                    txtInstallmentAmt.Enabled = false;

                    if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
                    {
                        ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue("Y"));
                        ddlAMRemarks.Enabled = false;
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = true;
                        ddlAMConsider.Enabled = false;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = true;
                        e.Row.Cells[3].Text = e.Row.Cells[17].Text.Trim() == "Y" ? txtInstallmentAmt.Text : "0";

                        gvCoApplOblig.Columns[5].Visible = false;
                        gvCoApplOblig.Columns[6].Visible = false;
                        gvCoApplOblig.Columns[7].Visible = false;
                        gvCoApplOblig.Columns[8].Visible = false;
                        gvCoApplOblig.Columns[9].Visible = false;
                        gvCoApplOblig.Columns[10].Visible = false;
                        gvCoApplOblig.Columns[11].Visible = false;
                        gvCoApplOblig.Columns[12].Visible = false;
                        gvCoApplOblig.Columns[13].Visible = false;
                        gvCoApplOblig.Columns[14].Visible = false;
                        gvCoApplOblig.Columns[15].Visible = false;
                        gvCoApplOblig.Columns[16].Visible = false;

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlAMRemarks.Items.Insert(0, oLc);
                    }
                    else if (vDesig == "ARM" || vDesig == "SH")
                    {
                        ddlBMConsider.SelectedIndex = ddlBMConsider.Items.IndexOf(ddlBMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                        ddlAMConsider.SelectedIndex = ddlAMConsider.Items.IndexOf(ddlAMConsider.Items.FindByValue(e.Row.Cells[17].Text.Trim()));
                        ddlCMRemarks.Enabled = false;
                        ddlTLORemarks.Enabled = false;
                        ddlBUHRemarks.Enabled = false;
                        ddlBMRemarks.Enabled = false;

                        ddlAMConsider.Enabled = true;
                        ddlCMConsider.Enabled = false;
                        ddlTLOConsider.Enabled = false;
                        ddlBUHConsider.Enabled = false;
                        ddlBMConsider.Enabled = false;
                        e.Row.Cells[6].Text = e.Row.Cells[17].Text.Trim() == "Y" ? txtInstallmentAmt.Text : "0";

                        gvCoApplOblig.Columns[8].Visible = false;
                        gvCoApplOblig.Columns[9].Visible = false;
                        gvCoApplOblig.Columns[10].Visible = false;
                        gvCoApplOblig.Columns[11].Visible = false;
                        gvCoApplOblig.Columns[12].Visible = false;
                        gvCoApplOblig.Columns[13].Visible = false;
                        gvCoApplOblig.Columns[14].Visible = false;
                        gvCoApplOblig.Columns[15].Visible = false;
                        gvCoApplOblig.Columns[16].Visible = false;
                        gvCoApplOblig.Columns[21].Visible = false;
                        gvCoApplOblig.Columns[22].Visible = false;

                        oGb = new CGblIdGenerator();
                        dt = oGb.popDeviationRemarks();
                        dt1 = dt.Select("Category = '" + e.Row.Cells[17].Text.Trim() + "'").CopyToDataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            ddlAMRemarks.DataSource = dt1;
                            ddlAMRemarks.DataTextField = "Remarks";
                            ddlAMRemarks.DataValueField = "RemId";
                            ddlAMRemarks.DataBind();
                        }

                        oLc = new ListItem("<--Select-->", "-1");
                        ddlAMRemarks.Items.Insert(0, oLc);
                        ddlAMRemarks.Enabled = true;
                    }
                    //---------------------------------------------------------------------------                                      
                }
            }
            finally
            {

            }

        }

        protected void btnAddDocRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["CoApplOblig"];
                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvCoApplOblig.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                TextBox txtInstitution = (TextBox)gvCoApplOblig.Rows[curRow].FindControl("txtInstitution");
                TextBox txtInstallmentAmt = (TextBox)gvCoApplOblig.Rows[curRow].FindControl("txtInstallmentAmt");

                dt.Rows[curRow][0] = hdnEnqId.Value;
                dt.Rows[curRow][1] = Convert.ToInt32(ViewState["CbId"]);
                dt.Rows[curRow][2] = txtInstitution.Text;
                if (txtInstallmentAmt.Text != "")
                    dt.Rows[curRow][3] = Convert.ToDecimal(txtInstallmentAmt.Text);
                else
                    dt.Rows[curRow][3] = 0;

                string vCoAppTotalEMI = dt.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();

                dr = dt.NewRow();
                dr["EnquiryId"] = hdnEnqId.Value;
                dr["CBID"] = Convert.ToInt32(ViewState["CbId"]);
                dr["CurrentBalance"] = 0;
                dr["InstallmentAmount"] = 0;
                dr["FinalEMIBM"] = 0;
                dr["RemarksBM"] = -1;
                dr["ConsiderBM"] = "N";
                dr["RemarksAM"] = -1;
                dr["ConsiderAM"] = "N";
                dr["FinalEMIAM"] = 0;
                dr["FromCBYN"] = "N";
                dr["SlNo"] = maxRow + 1;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                ViewState["CoApplOblig"] = dt;
                gvCoApplOblig.DataSource = dt;
                gvCoApplOblig.DataBind();

                gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                gvCoApplOblig.FooterRow.Cells[3].Text = vCoAppTotalEMI;
                gvCoApplOblig.FooterRow.Cells[6].Text = "0";
                gvCoApplOblig.FooterRow.Cells[9].Text = "0";
                gvCoApplOblig.FooterRow.Cells[12].Text = "0";
                gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                upCoAppEMI.Update();

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
                dt = (DataTable)ViewState["CoApplOblig"];
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted ");
                        return;
                    }
                    if (gR.Cells[23].Text == "Y")
                    {
                        gblFuction.AjxMsgPopup("Only Manual Obligation Can Be Deleted.");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["CoApplOblig"] = dt;
                    gvCoApplOblig.DataSource = dt;
                    gvCoApplOblig.DataBind();

                    string vCoAppTotalEMI = dt.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                    gvCoApplOblig.FooterRow.Cells[0].Text = "Total";
                    gvCoApplOblig.FooterRow.Cells[1].Text = vCoAppTotalEMI;
                    gvCoApplOblig.FooterRow.Cells[3].Text = "0";
                    gvCoApplOblig.FooterRow.Cells[6].Text = "0";
                    gvCoApplOblig.FooterRow.Cells[9].Text = "0";
                    gvCoApplOblig.FooterRow.Cells[12].Text = "0";
                    gvCoApplOblig.FooterRow.Cells[15].Text = "0";
                    upCoAppEMI.Update();

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

        protected void btnApplObligAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1, dt2 = null;
                DataTable dt = (DataTable)ViewState["ApplOblig"];
                //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
                string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
                int curRow = 0, maxRow = 0, vRow = 0;
                ImageButton txtCur = (ImageButton)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvApplOblig.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                TextBox txtInstitution = (TextBox)gvApplOblig.Rows[curRow].FindControl("txtInstitution");
                TextBox txtInstallmentAmt = (TextBox)gvApplOblig.Rows[curRow].FindControl("txtInstallmentAmt");

                dt.Rows[curRow][0] = hdnEnqId.Value;
                dt.Rows[curRow][1] = Convert.ToInt32(ViewState["CbId"]);
                dt.Rows[curRow][2] = txtInstitution.Text;
                if (txtInstallmentAmt.Text != "")
                    dt.Rows[curRow][3] = Convert.ToDecimal(txtInstallmentAmt.Text);
                else
                    dt.Rows[curRow][3] = 0;

                dt1 = dt.Select("ConsiderBM = 'Y'").CopyToDataTable();
                string vApplAMTotalEMI = "0";
                if (dt.Select("ConsiderAM = 'Y'").Count() > 0)
                {
                    dt2 = dt.Select("ConsiderAM = 'Y'").CopyToDataTable();
                    vApplAMTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                }
                string vApplTotalEMI = dt.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                string vApplBMTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();

                dr = dt.NewRow();
                dr["EnquiryId"] = hdnEnqId.Value;
                dr["CBID"] = Convert.ToInt32(ViewState["CbId"]);
                dr["CurrentBalance"] = 0;
                dr["InstallmentAmount"] = 0;
                dr["FinalEMIBM"] = 0;
                dr["RemarksBM"] = -1;
                dr["ConsiderBM"] = "N";
                dr["RemarksAM"] = -1;
                dr["ConsiderAM"] = "N";
                dr["FinalEMIAM"] = 0;
                dr["FromCBYN"] = "N";
                dr["SlNo"] = maxRow + 1;
                dt.Rows.Add(dr);
                dt.AcceptChanges();

                ViewState["ApplOblig"] = dt;
                gvApplOblig.DataSource = dt;
                gvApplOblig.DataBind();

                gvApplOblig.FooterRow.Cells[0].Text = "Total";
                gvApplOblig.FooterRow.Cells[1].Text = vApplTotalEMI;
                gvApplOblig.FooterRow.Cells[3].Text = vApplBMTotalEMI;
                gvApplOblig.FooterRow.Cells[6].Text = vApplAMTotalEMI;
                gvApplOblig.FooterRow.Cells[9].Text = "0";
                gvApplOblig.FooterRow.Cells[12].Text = "0";
                gvApplOblig.FooterRow.Cells[15].Text = "0";
                upAppEMI.Update();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void ImDelApplOblig_Click(object sender, EventArgs e)
        {
            DataTable dt, dt1, dt2 = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["ApplOblig"];
                if (dt.Rows.Count > 0)
                {
                    if ((dt.Rows.Count - 1) > gR.RowIndex)
                    {
                        gblFuction.AjxMsgPopup("Only Last Row Can Be Deleted.");
                        return;
                    }
                    if (gR.Cells[23].Text == "Y")
                    {
                        gblFuction.AjxMsgPopup("Only Manual Obligation Can Be Deleted.");
                        return;
                    }
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["ApplOblig"] = dt;
                    gvApplOblig.DataSource = dt;
                    gvApplOblig.DataBind();

                    dt1 = dt.Select("ConsiderBM = 'Y'").CopyToDataTable();
                    string vApplAMTotalEMI = "0";
                    if (dt.Select("ConsiderAM = 'Y'").Count() > 0)
                    {
                        dt2 = dt.Select("ConsiderAM = 'Y'").CopyToDataTable();
                        vApplAMTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                    }
                    string vApplTotalEMI = dt.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                    string vApplBMTotalEMI = dt1.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();
                    //string vApplAMTotalEMI = dt2.AsEnumerable().Select(t => t.Field<double>("InstallmentAmount")).Sum().ToString();

                    gvApplOblig.FooterRow.Cells[0].Text = "Total";
                    gvApplOblig.FooterRow.Cells[1].Text = vApplTotalEMI;
                    gvApplOblig.FooterRow.Cells[3].Text = vApplBMTotalEMI;
                    gvApplOblig.FooterRow.Cells[6].Text = vApplAMTotalEMI;
                    gvApplOblig.FooterRow.Cells[9].Text = "0";
                    gvApplOblig.FooterRow.Cells[12].Text = "0";
                    gvApplOblig.FooterRow.Cells[15].Text = "0";
                    upAppEMI.Update();
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

        private void popRO(string pBranch, string pDesig)
        {
            DataTable dt = null;
            CEO oRO = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            oRO = new CEO();
            ListItem oli = new ListItem("<--Select-->", "-1");
            if (pDesig == "AM")
            {
                dt = oRO.PopAmCmByBranch(pBranch, vLogDt, pDesig);
                ddlAM.DataSource = dt;
                ddlAM.DataTextField = "EoName";
                ddlAM.DataValueField = "Eoid";
                ddlAM.DataBind();
                ddlAM.Items.Insert(0, oli);
            }
            if (pDesig == "CM")
            {
                oRO = new CEO();
                dt = oRO.PopAmCmByBranch(pBranch, vLogDt, pDesig);
                ddlCM.DataSource = dt;
                ddlCM.DataTextField = "EoName";
                ddlCM.DataValueField = "Eoid";
                ddlCM.DataBind();
                ddlCM.Items.Insert(0, oli);
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
                StatusButton("View");
                LoadGrid();
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vEnqId = Convert.ToString(ViewState["EnqId"]);
            DataTable dt, dt1 = null;
            string vXmlMain = "", vXmlDtl = "", vXmlApplObligData = "", vXmlCoApplObligData = "";
            Int32 vCBID = Convert.ToInt32(ViewState["CbId"]);
            string vMemberName = Convert.ToString(ViewState["Name"]);
           // string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            string vAssetType = chkNonQualify.Checked == true ? "N" : "Q";

            DataTable dtMain = (DataTable)ViewState["DeviationMain"];
            GetDeviationDtl();
            DataTable dtDtl = (DataTable)ViewState["DeviationDtl"];
            GetAppObligData();
            dt = (DataTable)ViewState["ApplOblig"];
            GetCoAppObligData();
            dt1 = (DataTable)ViewState["CoApplOblig"];

            vXmlApplObligData = dtToXml(dt);
            vXmlCoApplObligData = dtToXml(dt1);

            if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
            {
                if (FileUpload1.PostedFile.ContentLength > vPathDeviationMaxLength)
                {
                    gblFuction.AjxMsgPopup("File 1 size is Exceeded");
                    return false;
                }
                if (FileUpload2.PostedFile.ContentLength > vPathDeviationMaxLength)
                {
                    gblFuction.AjxMsgPopup("File 2 size is Exceeded");
                    return false;
                }
                if (FileUpload3.PostedFile.ContentLength > vPathDeviationMaxLength)
                {
                    gblFuction.AjxMsgPopup("File 3 size is Exceeded");
                    return false;
                }
                //for (int i = dtMain.Rows.Count - 1; i >= 0; i--)
                //{
                //    DataRow dr = dtMain.Rows[i];
                //    if (Convert.ToString(dr["EnquiryId"]) != vEnqId)
                //    {
                //        dr.Delete();
                //        dtMain.AcceptChanges();
                //    }
                //}
                dtMain = dtMain.Select("EnquiryId = '" + vEnqId + "' AND CBID='" + vCBID + "'").CopyToDataTable();
                dtMain.TableName = "Table1";
                dtMain.AcceptChanges();

                double vBMRecoAmt = txtBMRecoAmt.Text == "" ? 0 : Convert.ToDouble(txtBMRecoAmt.Text);
                vXmlMain = dtToXml(dtMain);
                vXmlDtl = dtToXml(dtDtl);
                CDeviationMatrix oCDM = null;
                int vErr = 0;
                string vFileExistYN = "N";
                if (FileUpload1.HasFile || FileUpload2.HasFile || FileUpload3.HasFile)
                {
                    vFileExistYN = "Y";
                }
                if (GetExt(FileUpload1) == ".XLSX")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xlsx) not supported.");
                }
                else if (GetExt(FileUpload1) == ".XLS")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xls) not supported.");
                }
                else if (GetExt(FileUpload2) == ".XLSX")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xlsx) not supported.");
                }
                else if (GetExt(FileUpload2) == ".XLS")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xls) not supported.");
                }
                else if (GetExt(FileUpload3) == ".XLSX")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xlsx) not supported.");
                }
                else if (GetExt(FileUpload3) == ".XLS")
                {
                    vResult = false;
                    gblFuction.MsgPopup("File extention(.xls) not supported.");
                }
                else
                {
                    oCDM = new CDeviationMatrix();
                    vErr = oCDM.SaveDeviationMatrix(vXmlMain, vXmlDtl, vXmlApplObligData, vXmlCoApplObligData, Convert.ToString(Session[gblValue.UserName]), ddlRBMRemarks.SelectedValue, ddlAM.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]), vFileExistYN, "D", 0, vBMRecoAmt, vAssetType);
                    if (vErr > 0)
                    {
                        if (ddlOp.SelectedValue == "C")
                        {
                            oCDM = new CDeviationMatrix();
                            vErr = oCDM.ApproveDeviationMatrix(vEnqId, vCBID, ddlRBMRemarks.SelectedValue, "C", Convert.ToInt32(Session[gblValue.UserId]), 0, 0, 0, vDesig, "", "");
                        }

                        string vPath = vPathDeviation + vEnqId;
                        if (vFileExistYN == "Y")
                        {
                            System.IO.Directory.CreateDirectory(vPath);
                        }
                        if (FileUpload1.HasFile)
                        {
                            string vExt = Path.GetExtension(FileUpload1.PostedFile.FileName);
                            if (MinioYN == "Y")
                            {
                                string vFileName = vExt.ToLower() == ".pdf" ? vEnqId + "_File1" + vExt : "File1" + vExt;
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(FileUpload1.PostedFile);
                                oAC.UploadFileMinio(vFileByte, vFileName, vEnqId, DeviationDocBucket, MinioUrl);
                            }
                            else
                            {
                                FileUpload1.PostedFile.SaveAs(vPath + "/File1" + vExt);
                            }
                        }
                        if (FileUpload2.HasFile)
                        {
                            string vExt = Path.GetExtension(FileUpload2.PostedFile.FileName);
                            if (MinioYN == "Y")
                            {
                                string vFileName = vExt.ToLower() == ".pdf" ? vEnqId + "_File2" + vExt : "File2" + vExt;
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(FileUpload2.PostedFile);
                                oAC.UploadFileMinio(vFileByte, vFileName, vEnqId, DeviationDocBucket, MinioUrl);
                            }
                            else
                            {
                                FileUpload2.PostedFile.SaveAs(vPath + "/File2" + vExt);
                            }
                        }
                        if (FileUpload3.HasFile)
                        {
                            string vExt = Path.GetExtension(FileUpload3.PostedFile.FileName);
                            if (MinioYN == "Y")
                            {
                                string vFileName = vExt.ToLower() == ".pdf" ? vEnqId + "_File3" + vExt : "File3" + vExt;
                                CApiCalling oAC = new CApiCalling();
                                byte[] vFileByte = ConvertFileToByteArray(FileUpload3.PostedFile);
                                oAC.UploadFileMinio(vFileByte, vFileName, vEnqId, DeviationDocBucket, MinioUrl);
                            }
                            else
                            {
                                FileUpload3.PostedFile.SaveAs(vPath + "/File3" + vExt);
                            }
                        }
                        //-----------------------------Send  Mail----------------------------
                        //--CENTR - 5556
                        //oCDM = new CDeviationMatrix();
                        //string vEmail = oCDM.GetEmailIdbyEoid(ddlAM.SelectedValue);
                        //if (vEmail != "")
                        //{
                        //    SendToMail(vEmail, "Enquiry Id-" + vEnqId + " name-" + vMemberName + " has been reffered to next level", "Deviation Recomendation");
                        //}
                        //-------------------------------------------------------------------
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblMarg.DBError);
                        vResult = false;
                    }
                }
            }
            else if (vDesig == "ARM" || vDesig == "SH")
            {
                CDeviationMatrix oCDM = null;
                int vErr = 0;
                oCDM = new CDeviationMatrix();
                if (ddlOp.SelectedValue == "R")
                {
                    vErr = oCDM.SaveDevRecom(vEnqId, vCBID, ddlCM.SelectedValue, ddlRAMRemarks.SelectedValue, vDesig, Convert.ToDouble(txtAMRecoAmt.Text == "" ? "0" : txtAMRecoAmt.Text), vXmlApplObligData, vXmlCoApplObligData);
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
                else
                {
                    vErr = oCDM.ApproveDeviationMatrix(vEnqId, vCBID, ddlRAMRemarks.SelectedValue, ddlOp.SelectedValue, Convert.ToInt32(Session[gblValue.UserId]), 0, 0, 0, vDesig, "", "");
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
            }
            else
            {
                gblFuction.AjxMsgPopup("Reccomendation not allowed.");
                vResult = false;
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

        protected void ImgBtnDown1_Click(object sender, EventArgs e)
        {
            string vPath = "C:/DDHTML/" + Convert.ToString(ViewState["EnqId"]) + "/";
            DownloadFile(vPath, "File1");
        }

        protected void ImgBtnDown2_Click(object sender, EventArgs e)
        {
            string vPath = "C:/DDHTML/" + Convert.ToString(ViewState["EnqId"]) + "/";
            DownloadFile(vPath, "File2");
        }

        protected void ImgBtnDown3_Click(object sender, EventArgs e)
        {
            string vPath = "C:/DDHTML/" + Convert.ToString(ViewState["EnqId"]) + "/";
            DownloadFile(vPath, "File3");
        }

        protected void ImgBtnDown4_Click(object sender, EventArgs e)
        {
            string vPath = "C:/DDHTML/" + Convert.ToString(ViewState["EnqId"]) + "/";
            DownloadFile(vPath, "File4");
        }

        protected void ImgBtnDown5_Click(object sender, EventArgs e)
        {
            string vPath = "C:/DDHTML/" + Convert.ToString(ViewState["EnqId"]) + "/";
            DownloadFile(vPath, "File5");
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

        private string GetExt(FileUpload fu)
        {
            string vExt = Path.GetExtension(fu.PostedFile.FileName);
            return vExt.ToUpper();
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

        private void popOperation(string pDesig)
        {
            ListItem oli = null;
            ddlOp.Items.Clear();
            oli = new ListItem("<--Select-->", "-1");
            if (pDesig == "BM" || pDesig == "UM" || pDesig == "DEM")
            {
                ddlOp.Items.Insert(0, oli);
                oli = new ListItem("Recommendation", "R");
                ddlOp.Items.Insert(1, oli);
                oli = new ListItem("Reject", "C");
                ddlOp.Items.Insert(2, oli);
            }
            else if (pDesig == "ARM" || pDesig == "SH")
            {
                ddlOp.Items.Insert(0, oli);
                oli = new ListItem("Recommendation", "R");
                ddlOp.Items.Insert(1, oli);
                oli = new ListItem("Reject", "C");
                ddlOp.Items.Insert(2, oli);
                oli = new ListItem("Send Back", "S");
                ddlOp.Items.Insert(3, oli);
            }
        }

        protected void gvDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdCbReport")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                string vEnqId = gvRow.Cells[16].Text.Trim();
                Int32 vCbId = Convert.ToInt32(gvRow.Cells[17].Text.Trim());
                string vCBEnqDate = gvRow.Cells[18].Text.Trim();
                string vApplicantType = gvRow.Cells[0].Text.Trim();
                string url = "";
                url = vCBUrl + "?vEnqId=" + vEnqId + "&vCbId=" + vCbId + "&vCBEnqDate=" + vCBEnqDate.Replace("/", "_") + "&vMemberType=" + vApplicantType;
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
                TextBox txtInstitution = (TextBox)gr.FindControl("txtInstitution");
                TextBox txtInstallmentAmt = (TextBox)gr.FindControl("txtInstallmentAmt");

                dt.Rows[gr.RowIndex]["Institution"] = txtInstitution.Text;
                dt.Rows[gr.RowIndex]["InstallmentAmount"] = txtInstallmentAmt.Text;
                dt.Rows[gr.RowIndex]["ConsiderBM"] = ddlBMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBM"] = ddlBMRemarks.SelectedValue;
                dt.Rows[gr.RowIndex]["ConsiderAM"] = ddlAMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksAM"] = ddlAMRemarks.SelectedValue;
                dt.Rows[gr.RowIndex]["FinalEMIBM"] = gr.Cells[3].Text;
                dt.Rows[gr.RowIndex]["FinalEMIAM"] = gr.Cells[6].Text == "" ? "0" : gr.Cells[6].Text;
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
                TextBox txtInstitution = (TextBox)gr.FindControl("txtInstitution");
                TextBox txtInstallmentAmt = (TextBox)gr.FindControl("txtInstallmentAmt");

                dt.Rows[gr.RowIndex]["Institution"] = txtInstitution.Text;
                dt.Rows[gr.RowIndex]["InstallmentAmount"] = txtInstallmentAmt.Text;
                dt.Rows[gr.RowIndex]["ConsiderBM"] = ddlBMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksBM"] = ddlBMRemarks.SelectedValue;
                dt.Rows[gr.RowIndex]["ConsiderAM"] = ddlAMConsider.SelectedValue;
                dt.Rows[gr.RowIndex]["RemarksAM"] = ddlAMRemarks.SelectedValue;
                dt.Rows[gr.RowIndex]["FinalEMIBM"] = gr.Cells[3].Text;
                dt.Rows[gr.RowIndex]["FinalEMIAM"] = gr.Cells[6].Text == "" ? "0" : gr.Cells[6].Text;
            }
        }

        public void GetDeviationDtl()
        {
            DataTable dt = (DataTable)ViewState["DeviationDtl"];
            foreach (GridViewRow gr in gvDtl.Rows)
            {
                TextBox txtIncome = (TextBox)gr.FindControl("txtIncome");
                dt.Rows[gr.RowIndex]["Inc"] = txtIncome.Text == "" ? "0" : txtIncome.Text;
            }
        }

        public void GetOS()
        {
            double vTotalOS = 0, vTotalOSExcUnity = 0;
            //string vDesig = Convert.ToString(Session[gblValue.Designation]).Trim().ToUpper();
            string vDesig = Convert.ToString(Session[gblValue.JLGDeviationCtrl]).Trim().ToUpper();
            string vDDlName = (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM") ? "ddlBMConsider" : (vDesig == "ARM" || vDesig == "SH") ? "ddlAMConsider" : "";
            string InsType = "";
            string vCbEnqDate = ViewState["CBEnqDate"].ToString();

            foreach (GridViewRow gr in gvApplOblig.Rows)
            {
                DropDownList ddlConsider = (DropDownList)gr.FindControl(vDDlName);
                TextBox txtInstitution = (TextBox)gr.FindControl("txtInstitution");
                if (ddlConsider.SelectedValue == "Y")
                {
                    if (gblFuction.setDate(vCbEnqDate) < gblFuction.setDate("05/02/2025"))
                    {
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                        {
                            vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[25].Text);
                        }
                        vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[25].Text);
                    }
                    else
                    {
                        InsType = gr.Cells[27].Text;
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (InsType == "MFI" || InsType == "RUS")
                        {
                            if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                            {
                                vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[25].Text);
                            }
                            vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[25].Text);
                        }
                    }
                }
            }
            foreach (GridViewRow gr in gvCoApplOblig.Rows)
            {
                DropDownList ddlConsider = (DropDownList)gr.FindControl(vDDlName);
                TextBox txtInstitution = (TextBox)gr.FindControl("txtInstitution");
                if (ddlConsider.SelectedValue == "Y")
                {
                    if (gblFuction.setDate(vCbEnqDate) < gblFuction.setDate("05/02/2025"))
                    {
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                        {
                            vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[25].Text);
                        }
                        vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[25].Text);
                    }
                    else
                    {
                        InsType = gr.Cells[27].Text;
                        string vInstitute = txtInstitution.Text.Trim().ToUpper();
                        if (InsType == "MFI" || InsType == "RUS")
                        {
                            if (!vInstitute.Contains("UNITY SMALL") && !vInstitute.Contains("CENTRUM"))
                            {
                                vTotalOSExcUnity = vTotalOSExcUnity + Convert.ToDouble(gr.Cells[25].Text);
                            }
                            vTotalOS = vTotalOS + Convert.ToDouble(gr.Cells[25].Text);
                        }
                    }
                }
            }
            // vTotalOS = (vTotalOSExcUnity - Convert.ToDouble(ViewState["OS"].ToString()));
            lblTotalOSExclUSFB.Text = vTotalOSExcUnity.ToString("0.00");
            upTotalOSExclUnity.Update();
            lblFinalOSInclUSFB.Text = vTotalOS.ToString("0.00");
            hdnFinalOSInclUSFB.Value = vTotalOS.ToString("0.00");
            upFinalOSInclUSFB.Update();
            txtRecommendtion.Text = "0.00";
            uptxtRecoAmt.Update();
        }

        protected void ddlOp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string vDesig = txtDesignation.Text;
            var CancelReason = new List<string> { "Highly Overleveraged", "Poor Track Record", "High Overdue" };
            var RecomReason = new List<string> { "Clear repayment history with NIL Overdue", "Satisfactory RTR of all loans incl. existing USFB loan", "Satisfactory overall repayment history except few instances of Overdue in other loan RTR", "W/Off or Overdue is old. Clear repayment seen in recent times" };
            ListItem oli = new ListItem("<--Select-->", "-1");
            if (vDesig == "BM" || vDesig == "UM" || vDesig == "DEM")
            {
                ddlRBMRemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R")
                {
                    ddlRBMRemarks.DataSource = RecomReason;
                    ddlRBMRemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlRBMRemarks.DataSource = CancelReason;
                    ddlRBMRemarks.DataBind();
                }
                ddlRBMRemarks.Items.Insert(0, oli);
            }
            if (vDesig == "ARM" || vDesig == "SH")
            {
                ddlRAMRemarks.Items.Clear();
                if (ddlOp.SelectedValue == "R")
                {
                    ddlRAMRemarks.DataSource = RecomReason;
                    ddlRAMRemarks.DataBind();
                }
                if (ddlOp.SelectedValue == "C")
                {
                    ddlRAMRemarks.DataSource = CancelReason;
                    ddlRAMRemarks.DataBind();
                }
                ddlRAMRemarks.Items.Insert(0, oli);
            }

        }

        protected void txtRecommendtion_TextChanged(object sender, EventArgs e)
        {
            string vDose = ViewState["Dose"].ToString();
            string vAmt = Convert.ToDouble(vDose) == 0 ? "40000" : "60000";
            string vRecoAmt = txtRecommendtion.Text == "" ? "0" : txtRecommendtion.Text;
            if (Convert.ToDouble(vRecoAmt) > Convert.ToDouble(vAmt))
            {
                gblFuction.AjxMsgPopup("Recomended amount greater than system recomended amount.");
                txtRecommendtion.Text = "0";
                txtRecommendtion.Focus();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "MyFunction", "Recommendtion()", true);
            }
        }

        #region ConvertFileToByteArray
        private byte[] ConvertFileToByteArray(HttpPostedFile postedFile)
        {
            using (Stream stream = postedFile.InputStream)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
        #endregion

    }
}