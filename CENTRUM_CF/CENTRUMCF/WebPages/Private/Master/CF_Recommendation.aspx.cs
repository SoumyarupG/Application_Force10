using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using CENTRUMCA;
using CENTRUMBA;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Services;

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_Recommendation : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                hdnMaxFileSize.Value = MaxFileSize;
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                    GenerateRecommendation(Convert.ToInt64(hdLeadId.Value));
                    StatusButton("Show");
                    CheckOprtnStatus(Convert.ToInt64(hdLeadId.Value));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.AjxMsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbBasicDet.ActiveTabIndex = 0;
            }
            
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Recommendation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFRocommendation);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = true;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Recommendation", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.AjxMsgPopup(MsgAccess.Edit);
                    return;
                }
                if (ddlAction.SelectedValue == "-1")
                {
                    ViewState["StateEdit"] = "Add";
                }
                else
                {
                    ViewState["StateEdit"] = "Edit";
                }

                StatusButton("Edit");

                if (ddlAction.SelectedValue == "Recommended")
                {
                    txtHoldRemarks.Enabled = false;
                    txtHoldRemarks.Text = "";
                }
                else
                {
                    txtHoldRemarks.Enabled = true;
                }
             //   btnViewScore.Enabled = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 0;
            //EnableControl(false);
            StatusButton("Show");
        }
        protected void CheckOprtnStatus(Int64 vLeadID)
        {
            Int32 vErr = 0;
            CMember oMem = null;
            oMem = new CMember();
            vErr = oMem.CF_chkOperatnStatus(vLeadID);
            if (vErr == 1)
            {
                btnSave.Enabled = false;
                btnEdit.Enabled = false;
                gblFuction.MsgPopup("This Lead is Under Process at Operation Stage.You can not Change or Update it...");
                return;
            }
            else
            {
                //btnSave.Enabled = true;
                btnEdit.Enabled = true;

            }
        }
        protected void btnRunScore_Click(object sender, EventArgs e)
        {
            CDistrict oDis = null; Int32 vInternalScore = 0;
            oDis = new CDistrict();
            vInternalScore = oDis.CF_GenerateInternalScore(Convert.ToInt64(hdLeadId.Value), 0);

            txtIntRiskScore.Text = Convert.ToString(vInternalScore);
           
        }
        protected void btnScoreCard_Click(object sender, EventArgs e)
        {
            ViewScore();
        }
        protected void ViewScore()
        {
            DataTable dt = new DataTable();
            CDistrict oDis = null;
            oDis = new CDistrict();
            Int32 vSegMentType = 0;
            

            dt = oDis.ViewInternalScore(Convert.ToInt64(hdLeadId.Value));
            if (dt.Rows.Count > 0)
            {
                vSegMentType = Convert.ToInt32(dt.Rows[0]["SegMentType"]);

                if (vSegMentType == 1)
                {
                    trBusiPremi.Visible = false;
                    trBusVintage.Visible = false;
                    trPropOwnership.Visible = true;
                    trTotExp.Visible = true;

                    lblBCPNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                    lblAppName.Text = Convert.ToString(dt.Rows[0]["ApplName"]);

                    txtAge.Text = Convert.ToString(dt.Rows[0]["Age"]);
                    txtAgeScore.Text = Convert.ToString(dt.Rows[0]["AgeScore"]);
                    txtQualification.Text = Convert.ToString(dt.Rows[0]["QualVal"]);
                    txtQualificationScore.Text = Convert.ToString(dt.Rows[0]["QualificationScore"]);
                    txtNoofDep.Text = Convert.ToString(dt.Rows[0]["NoofDep"]);
                    txtNoofDepScore.Text = Convert.ToString(dt.Rows[0]["NoofDepScore"]);
                    txtFOIRA.Text = Convert.ToString(dt.Rows[0]["FOIR"]);
                    txtFOIRScore.Text = Convert.ToString(dt.Rows[0]["FOIRScore"]);
                    txtLTV.Text = Convert.ToString(dt.Rows[0]["LTV"]);
                    txtLTVScore.Text = Convert.ToString(dt.Rows[0]["LTVScore"]);
                    txtCBSVal.Text = Convert.ToString(dt.Rows[0]["CBSVal"]);
                    txtCBSValScore.Text = Convert.ToString(dt.Rows[0]["CBSValScore"]);

                    txtPropOwnership.Text = Convert.ToString(dt.Rows[0]["PropOwnership"]);
                    txtPropOwnershipScore.Text = Convert.ToString(dt.Rows[0]["PropOwnershipScore"]);
                    txtTotExp.Text = Convert.ToString(dt.Rows[0]["TotExp"]);
                    txtTotExpScore.Text = Convert.ToString(dt.Rows[0]["TotExpScore"]);

                    txtEMIAvBill.Text = Convert.ToString(dt.Rows[0]["EMIAvBill"]);
                    txtEMIAvBillScore.Text = Convert.ToString(dt.Rows[0]["EMIAvBillScore"]);
                    txtTotalScore.Text = Convert.ToString(dt.Rows[0]["InternalScore"]);
                }
                else
                {

                    trBusiPremi.Visible = true;
                    trBusVintage.Visible = true;
                    trPropOwnership.Visible = false;
                    trTotExp.Visible = false;

                    lblBCPNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                    lblAppName.Text = Convert.ToString(dt.Rows[0]["ApplName"]);

                    txtAge.Text = Convert.ToString(dt.Rows[0]["Age"]);
                    txtAgeScore.Text = Convert.ToString(dt.Rows[0]["AgeScore"]);
                    txtQualification.Text = Convert.ToString(dt.Rows[0]["Qualification"]);
                    txtQualificationScore.Text = Convert.ToString(dt.Rows[0]["QualificationScore"]);
                    txtNoofDep.Text = Convert.ToString(dt.Rows[0]["NoofDep"]);
                    txtNoofDepScore.Text = Convert.ToString(dt.Rows[0]["NoofDepScore"]);
                    txtFOIRA.Text = Convert.ToString(dt.Rows[0]["FOIR"]);
                    txtFOIRScore.Text = Convert.ToString(dt.Rows[0]["FOIRScore"]);
                    txtLTV.Text = Convert.ToString(dt.Rows[0]["LTV"]);
                    txtLTVScore.Text = Convert.ToString(dt.Rows[0]["LTVScore"]);
                    txtCBSVal.Text = Convert.ToString(dt.Rows[0]["CBSVal"]);
                    txtCBSValScore.Text = Convert.ToString(dt.Rows[0]["CBSValScore"]);
                    txtBusiPremiOwn.Text = Convert.ToString(dt.Rows[0]["BusiPremiOwn"]);
                    txtResBusScore.Text = Convert.ToString(dt.Rows[0]["ResBusScore"]);
                    txtBusiVintage.Text = Convert.ToString(dt.Rows[0]["BusiVintage"]);
                    txtBusiVintageScore.Text = Convert.ToString(dt.Rows[0]["BusiVintageScore"]);
                    txtEMIAvBill.Text = Convert.ToString(dt.Rows[0]["EMIAvBill"]);
                    txtEMIAvBillScore.Text = Convert.ToString(dt.Rows[0]["EMIAvBillScore"]);
                    txtTotalScore.Text = Convert.ToString(dt.Rows[0]["InternalScore"]);
                }


                

            }
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }       
        private void Calculation()
        {

            double vLoanAmt = 0;
            double vInsuCharge = 0;
            double vROI = Convert.ToDouble(txtROI.Text);
            double vTenure = Convert.ToDouble(txtTenure.Text);
            double vFOIR = Convert.ToDouble(hdFOIR.Value);
            double EMIAmt = 0, RoundEMIAmt = 0;
            if (hdLoanAmount.Value == "0")
            {
                vLoanAmt = Convert.ToDouble(txtLoanAmt.Text);
            }
            else
            {
                vLoanAmt = Convert.ToDouble(hdLoanAmount.Value);
            }

         
            #region
            double vChargePercent = 0, vChargeAmount = 0, vGSTPercent = 0, vGSTAmount = 0, vGrandTotal = 0;
            DataTable dt = null;
            dt = (DataTable)ViewState["InsuChrg"];
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vChargePercent = Convert.ToDouble(dt.Rows[i]["ChargePercent"].ToString());
                        vGSTPercent = Convert.ToDouble(dt.Rows[i]["GSTPercent"].ToString());
                        if (vChargePercent == 0)
                        {
                            vChargeAmount = Convert.ToDouble(dt.Rows[i]["ChargeAmount"].ToString());
                        }
                        else
                        {
                            vChargeAmount = Math.Round(((Convert.ToDouble(vLoanAmt) * Convert.ToDouble(vChargePercent)) / 100), 2);
                        }
                        if (vGSTPercent == 0)
                        {
                            vGSTAmount = Convert.ToDouble(dt.Rows[i]["GSTAmount"].ToString());
                        }
                        else
                        {
                            vGSTAmount = Math.Round(((Convert.ToDouble(vChargeAmount) * Convert.ToDouble(vGSTPercent)) / 100), 2);
                        }

                        vGrandTotal = vGrandTotal + vChargeAmount + vGSTAmount;
                    }

                    vInsuCharge = vGrandTotal;
                }
                else
                {
                    vInsuCharge = 0;

                }
            }

            hdInsuCharge.Value = vInsuCharge.ToString();
            #endregion


            double vExistingEMI = Convert.ToDouble(hdExistingEMI.Value);

            txtTotalLoanAmount.Text = Convert.ToString(vLoanAmt + vInsuCharge);
            txtTotalLoanAmount.Text = Convert.ToString(vLoanAmt + vInsuCharge);

            EMIAmt  = emi_calculator((vLoanAmt + vInsuCharge), vROI, vTenure);
            RoundEMIAmt = Math.Ceiling(EMIAmt);
            txtEMI.Text = Convert.ToString(RoundEMIAmt);


            txtTotEMI.Text = Convert.ToString(Math.Ceiling(EMIAmt + vExistingEMI));
        }
        double emi_calculator(double p, double r, double t)
        {
            double emi;

            r = r / (12 * 100); // one month interest
            emi = (p * r * Math.Pow(1 + r, t)) / (Math.Pow(1 + r, t) - 1);

            return (Math.Round(emi, 2));
        }
        protected void txtTenure_textChanged(object sender, EventArgs e)
        {
            Calculation();
            // UpFamily.Update();
        }
        protected void txtLoanAmt_textChanged(object sender, EventArgs e)
        {
            hdLoanAmount.Value = txtLoanAmt.Text;
            Calculation();
            // UpFamily.Update();
        }
        protected void txtROI_textChanged(object sender, EventArgs e)
        {
            Calculation();
            // UpFamily.Update();
        }
        private void GenerateRecommendation(Int64 LeadID)
        {
            ClearControls();
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null, dt2 = null;
            CDistrict ODis = null;
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplicantNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblProposalNo.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }

                ODis = new CDistrict();
                ds = ODis.CF_GenerateRecommendation(LeadID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                if (dt2.Rows.Count > 0)
                {
                    ViewState["InsuChrg"] = dt2;
                }
                if (dt.Rows.Count > 0)
                {
                    hdLoanAmount.Value = dt.Rows[0]["LoanAmount"].ToString();
                    hdInsuCharge.Value = dt.Rows[0]["InsuCharge"].ToString();
                    hdFOIR.Value = dt.Rows[0]["FOIR"].ToString();
                    txtFOIR.Text = dt.Rows[0]["FOIR"].ToString();
                    hdExistingEMI.Value = dt.Rows[0]["ExistingEMI"].ToString();
                    txtExistingEMI.Text = dt.Rows[0]["ExistingEMI"].ToString();
                    hdNetSurplus.Value = dt.Rows[0]["NetSurplus"].ToString();
                    txtMonthlyNetSurplus.Text = dt.Rows[0]["NetSurplus"].ToString();
                    hdAllowFund.Value = dt.Rows[0]["AllowFund"].ToString();
                    
                }
                else
                {
                    hdInsuCharge.Value = "0";
                    hdFOIR.Value = "0";
                    txtFOIR.Text = "0";
                    hdExistingEMI.Value = "0";
                    txtExistingEMI.Text = "0";
                    hdNetSurplus.Value = "0";
                    txtMonthlyNetSurplus.Text = "0";
                }
                if (dt1.Rows.Count > 0)
                {
                    txtLoanAmt.Text = dt1.Rows[0]["LoanAmt"].ToString();
                    txtROI.Text = dt1.Rows[0]["ROI"].ToString();
                    txtTenure.Text = dt1.Rows[0]["Tenure"].ToString();

                    txtIntRiskScore.Text = dt1.Rows[0]["InternalScore"].ToString();
                    ddlAction.SelectedValue = dt1.Rows[0]["Action"].ToString();
                    txtHoldRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                    lblFileName.Text = dt1.Rows[0]["FileName"].ToString();
                    hdnFileName.Value = dt1.Rows[0]["FileName"].ToString();
                    txtFinalDecStatus.Text = dt1.Rows[0]["ApprovedStatus"].ToString();

                    if (ddlAction.SelectedValue == "Recommended")
                    {
                        txtHoldRemarks.Enabled = false;
                        txtHoldRemarks.Text = "";
                    }
                    else
                    {
                        txtHoldRemarks.Enabled = true;
                    }
                }
                else
                {
                    txtLoanAmt.Text = "0";
                    txtROI.Text = "0";
                    txtTenure.Text = "0";
                    txtIntRiskScore.Text = "0";
                    ddlAction.SelectedValue = "-1";
                    txtHoldRemarks.Text = "";
                    lblFileName.Text = "";

                }
                Calculation();
                UpEMI.Update();
                UpTOTEMI.Update();
                UpTotalLoanAmount.Update();

                tbBasicDet.ActiveTabIndex = 0;
              //  StatusButton("Show");
                if (lblFileName.Text != "")
                {
                    btnUPAttachment.Enabled = true;
                }
                else
                {
                    btnUPAttachment.Enabled = false;
                }
            }

            finally
            {
            }
        }       
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    
                    break;
                case "Show":
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;                                 
                    EnableControl(false);
                    break;
                case "Edit":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;                  
                    EnableControl(false);
                    break;
                case "Delete":
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":                   
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtLoanAmt.Enabled = Status;
            txtROI.Enabled = Status;
            txtTenure.Enabled = Status;
            ddlAction.Enabled = Status;
            fuUPAttachmentUpld.Enabled = Status;
            txtHoldRemarks.Enabled = Status;
            btnRunScore.Enabled = Status;
            
            btnScoreCard.Enabled = Status;
            btnUPAttachment.Enabled = Status;

        }
        private void ClearControls()
        {
            lblApplicantNm.Text = "";
            lblProposalNo.Text = "";
            txtLoanAmt.Text = "0";
            txtROI.Text = "0";
            txtTenure.Text = "0";
            ddlAction.SelectedIndex = -1;
            txtIntRiskScore.Text = "0";
            txtHoldRemarks.Text = "";
            txtTotalLoanAmount.Text = "0";
            txtEMI.Text = "0";
            txtExistingEMI.Text = "0";
            lblFileName.Text = "";
        }
        #region Save
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                GenerateRecommendation(Convert.ToInt64(hdLeadId.Value));
                StatusButton("Show");
                ViewState["StateEdit"] = null;
              
            }
            else
            {
                StatusButton("Edit");
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            CDistrict oDis = null;
            string vBrCode = "", vErrMsg = "";
            Int32 vErr = 1, UID = 0, vMaxFileSize = 0;
            Int64 vLeadId = 0;

            double vLoanAmt = 0, vROI = 0, vTenure = 0, vInternalScore = 0;
            string vAction = "", vRemarks = "", vFileName = "", vFileExt = "", vIsUpload = "", vFileStoredPath = "";


            try
            {
                
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }

  
                vBrCode = Session[gblValue.BrnchCode].ToString();
                UID = Convert.ToInt32(Session[gblValue.UserId]);
                vMaxFileSize = Convert.ToInt32(MaxFileSize);
                vFileStoredPath = DocumentBucketURL;
                vFileName = "Recommendation";

                vLoanAmt = Convert.ToDouble(txtLoanAmt.Text); vAction = ddlAction.SelectedValue;
                vROI = Convert.ToDouble(txtROI.Text); vRemarks = txtHoldRemarks.Text;
                vTenure = Convert.ToDouble(txtTenure.Text);
                vInternalScore = Convert.ToDouble(txtIntRiskScore.Text);


                vIsUpload = fuUPAttachmentUpld.HasFile == true ? "Y" : "N";
                    
                if (vIsUpload == "Y")
                {
                    
                    vFileExt = System.IO.Path.GetExtension(fuUPAttachmentUpld.PostedFile.FileName);
                    vFileName = fuUPAttachmentUpld.HasFile == true ? vFileName + vFileExt : "";
                    if (fuUPAttachmentUpld.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.");
                        return false;
                    }
                    if ((vFileExt.ToLower().Trim() != ".pdf") && (vFileExt.ToLower().Trim() != ".xlx") && (vFileExt.ToLower().Trim() != ".xlsx"))
                    {
                        gblFuction.AjxMsgPopup("Only PDF and Excel files are allowed!");
                        return false;
                    }

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(fuUPAttachmentUpld.FileBytes);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuUPAttachmentUpld.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                }
                else
                {
                    if (Mode == "Edit")
                    {
                        if (lblFileName.Text != "")
                        {
                            vIsUpload = "Y";
                            vFileName = lblFileName.Text;
                        }
                        else
                        {
                            vFileName = "";
                            vIsUpload = "N";
                        }
                    }
                    else
                    {
                        vFileName = "";
                    }
                    
                }
 
                oDis = new CDistrict();
                vErr = oDis.CF_SaveRecommendation(vLeadId, vBrCode, vLoanAmt, vROI, vTenure, vInternalScore, vAction, vRemarks, vFileName, vIsUpload, vFileStoredPath, UID, Mode, 0, ref vErrMsg);
                if (vErr == 0)
                {
                    if (vFileExt.ToLower().Contains(".pdf"))
                    {

                        if (fuUPAttachmentUpld.HasFile)
                        {
                            SaveMemberImages(fuUPAttachmentUpld, hdLeadId.Value, hdLeadId.Value + "_" + "Recommendation", vFileExt, "N");
                        }
                    }
                    else
                    {
                        if (fuUPAttachmentUpld.HasFile)
                        {
                            SaveMemberImages(fuUPAttachmentUpld, hdLeadId.Value, "Recommendation", vFileExt, "N");
                        }
                    }
                    gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.AjxMsgPopup(vErrMsg);
                    vResult = false;
                }

               
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return vResult;

        }
        #endregion
        #region SaveMemberImages
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, DocumentBucket, MinioUrl);
            return isImageSaved;
        }
        #endregion
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
        protected void btnUPAttachment_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnFileName.Value);
        }
        protected void ViewImgDoc(string ID, string FileName)
        {
            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + FileName);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.AjxMsgPopup("No File Found");
            }

        }
        public bool ValidUrlChk(string pUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                WebRequest request = WebRequest.Create(pUrl);
                request.Timeout = 5000;
                using (WebResponse response = request.GetResponse())
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}