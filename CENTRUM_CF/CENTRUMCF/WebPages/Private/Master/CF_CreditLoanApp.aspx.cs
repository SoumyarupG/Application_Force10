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

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_CreditLoanApp : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string CFLeadBucketURL = ConfigurationManager.AppSettings["CFLeadBucketURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                tbBasicDet.ActiveTabIndex = 0;
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
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
                this.PageHeading = "Final Decision";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFFinalDec);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Employment/Business Details", false);
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
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                //if (ddlSacComm.SelectedValue == "-1")
                //{
                //    ViewState["StateEdit"] = "Add";
                //}
                //else
                //{
                //    ViewState["StateEdit"] = "Edit";
                //}
                
                StatusButton("Edit");

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 0;
            EnableControl(false);
            StatusButton("View");
        }

       
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(1);
        }
        private void LoadBasicDetailsList(int vPgNo)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
           
            try
            {
                dt = oMem.CF_GetBasicDtlsForLoanApp(vBrCode, txtSearch.Text.Trim());

                if (dt.Rows.Count > 0)
                {

                    gvBasicDet.DataSource = dt;
                    gvBasicDet.DataBind();
                    //hdAssMtdId.Value = Convert.ToString(dt.Rows[0]["AssMtdId"]);
                    //hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                }
                else
                {
                    gvBasicDet.DataSource = null;
                    gvBasicDet.DataBind();
                    //hdAssMtdId.Value = "";
                    //hdLeadId.Value = "";
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

        protected void gvBasicDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadId = 0;
            string vBrCode = "";
            DataSet ds = new DataSet();
            string vStatus = "";
            ClearControls();
            try
            {
                pLeadId = Convert.ToInt64(e.CommandArgument);

                vBrCode = Session[gblValue.BrnchCode].ToString();
                ViewState["LeadId"] = pLeadId;
                hdLeadId.Value = Convert.ToString(pLeadId);
                if (e.CommandName == "cmdShow")
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                    vStatus = (gvRow.Cells[6].Text).ToString();

                    System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                    foreach (GridViewRow gr in gvBasicDet.Rows)
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
                    CDistrict oDist = new CDistrict();
                    if (vStatus != "Pending")
                    {
                        ViewState["StateEdit"] = "Edit";
                        StatusButton("Edit");
                    }
                    else
                    {
                        ViewState["StateEdit"] = "Add";
                        StatusButton("Add");
                    }

                    GenerateFinalDecision(Convert.ToInt64(hdLeadId.Value));
                }
            }
            finally
            {
            }
        }

        private void Calculation()
        {

            double vLoanAmt = 0;
            double vInsuCharge = 0;
            double vROI = Convert.ToDouble(txtROIF.Text);
            double vTenure = Convert.ToDouble(txtTenureF.Text);
            double vFOIR = Convert.ToDouble(hdFOIRF.Value);
            if (hdLoanAmountF.Value == "0")
            {
                vLoanAmt = Convert.ToDouble(txtLoanAmtF.Text);
            }
            else
            {
                vLoanAmt = Convert.ToDouble(hdLoanAmountF.Value);
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

            hdInsuChargeF.Value = vInsuCharge.ToString();
            #endregion


            double vExistingEMI = Convert.ToDouble(hdExistingEMIF.Value);

            txtTotalLoanAmountF.Text = Convert.ToString(vLoanAmt + vInsuCharge);
            txtTotalLoanAmountF.Text = Convert.ToString(vLoanAmt + vInsuCharge);

            txtEMIF.Text = Convert.ToString(emi_calculator((vLoanAmt + vInsuCharge), vROI, vTenure));
            txtTotEMIF.Text = Convert.ToString(Convert.ToDouble(txtEMIF.Text) + vExistingEMI);
        }


        double emi_calculator(double p, double r, double t)
        {
            double emi;

            r = r / (12 * 100); // one month interest
            emi = (p * r * Math.Pow(1 + r, t)) / (Math.Pow(1 + r, t) - 1);

            return (Math.Round(emi, 2));
        }

        private void GenerateFinalDecision(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null;
            CDistrict ODis = null;
            try
            {
                ODis = new CDistrict();
                ds = ODis.CF_GenerateFinalDecision(LeadID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                dt2 = ds.Tables[2];
                dt3 = ds.Tables[3];
                dt4 = ds.Tables[4];
                if (dt2.Rows.Count > 0)
                {
                    ViewState["InsuChrg"] = dt2;
                }
                if (dt.Rows.Count > 0)
                {
                    hdLoanAmountF.Value = dt.Rows[0]["LoanAmount"].ToString();
                    hdInsuChargeF.Value = dt.Rows[0]["InsuCharge"].ToString();
                    hdFOIRF.Value = dt.Rows[0]["FOIR"].ToString();
                    txtFOIRF.Text = dt.Rows[0]["FOIR"].ToString();
                    hdExistingEMIF.Value = dt.Rows[0]["ExistingEMI"].ToString();
                    txtExistingEMIF.Text = dt.Rows[0]["ExistingEMI"].ToString();
                    hdNetSurplusF.Value = dt.Rows[0]["NetSurplus"].ToString();
                    txtMonthlyNetSurplusF.Text = dt.Rows[0]["NetSurplus"].ToString();
                    hdAllowFundF.Value = dt.Rows[0]["AllowFund"].ToString();
                }
                else
                {
                    hdInsuChargeF.Value = "0";
                    hdFOIRF.Value = "0";
                    txtFOIRF.Text = "0";
                    hdExistingEMIF.Value = "0";
                    txtExistingEMIF.Text = "0";
                    hdNetSurplusF.Value = "0";
                    txtMonthlyNetSurplusF.Text = "0";
                }
                if (dt1.Rows.Count > 0)
                {
                    txtLoanAmtF.Text = dt1.Rows[0]["LoanAmt"].ToString();
                    txtROIF.Text = dt1.Rows[0]["ROI"].ToString();
                    txtTenureF.Text = dt1.Rows[0]["Tenure"].ToString();

                    txtIntRiskScoreF.Text = dt1.Rows[0]["InternalScore"].ToString();
                    txtActionF.Text =   dt1.Rows[0]["Action"].ToString();
                    txtHoldRemarksF.Text = dt1.Rows[0]["Remarks"].ToString();
                    lblFileNameF.Text = dt1.Rows[0]["FileName"].ToString();
                    hdnFileNameF.Value = dt1.Rows[0]["FileName"].ToString();

                   
                }
                else
                {
                    txtLoanAmtF.Text = "0";
                    txtROIF.Text = "0";
                    txtTenureF.Text = "0";
                    txtIntRiskScoreF.Text = "0";
                    txtActionF.Text = "0";
                    txtHoldRemarksF.Text = "";
                    lblFileNameF.Text = "";

                }
                if (dt3.Rows.Count > 0)
                {
                    ddlSacComm.SelectedValue = dt3.Rows[0]["SactionComm"].ToString();
                    txtSacRemarks.Text = dt3.Rows[0]["SantionRemarks"].ToString();
                    txtSacDt.Text = dt3.Rows[0]["SantionDate"].ToString();
                    ddlRecommRemarks.SelectedValue = dt3.Rows[0]["RecommendtRemarks"].ToString();
                }
                else
                { 
                    txtSacRemarks.Text= "";
                    txtSacDt.Text = "";
                    ddlSacComm.SelectedValue = "-1";
                    ddlRecommRemarks.SelectedValue = "-1";
                }
                if (dt4.Rows.Count > 0)
                {
                    lblCAMFileName.Text = dt4.Rows[0]["CamFileName"].ToString();
                    hdnCAMFileName.Value = dt4.Rows[0]["CamFileName"].ToString();
                }
                Calculation();
                

                tbBasicDet.ActiveTabIndex = 1;
                StatusButton("Show");
                if (lblFileNameF.Text != "")
                {
                    btnUPAttachmentF.Enabled = true;
                }
                else
                {
                    btnUPAttachmentF.Enabled = false;
                }
                if (lblCAMFileName.Text != "")
                {
                    btnCAM.Enabled = true;
                }
                else
                {
                    btnCAM.Enabled = false;
                }
            }

            finally
            {
            }
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
            //LoadLeadList(cPgNo);
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
            }
        }
        private void EnableControl(Boolean Status)
        {
            ddlSacComm.Enabled = Status;
            txtSacRemarks.Enabled = Status;
            txtSacDt.Enabled = Status;
            ddlRecommRemarks.Enabled = Status;

        }
        private void ClearControls()
        {
            txtLoanAmtF.Text = "0";
            txtROIF.Text = "0";
            txtTenureF.Text = "0";
            txtActionF.Text = "0";
            txtIntRiskScoreF.Text = "0";
            txtHoldRemarksF.Text = "";
            txtTotalLoanAmountF.Text = "0";
            txtEMIF.Text = "0";
            txtExistingEMIF.Text = "0";
            lblFileNameF.Text = "";
            lblCAMFileName.Text = "";
            ddlSacComm.SelectedIndex = -1;
            txtSacRemarks.Text = "";
            txtSacDt.Text = "";
            ddlRecommRemarks.SelectedIndex = -1;
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
                LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
                ClearControls();
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
            Int32 vErr = 1, UID = 0;
            Int64 vLeadId = 0;

            double vLoanAmt = 0, vROI = 0, vTenure = 0, vInternalScore = 0;
            string vAction = "", vRemarks = "",  vIsUpload = "N", 
                vSacComm = "", vSacRemarks = "", vRecommRemarks = "", vIsCamAtt = "N", vIsLetter = "N";


            try
            {
                
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();
                UID = Convert.ToInt32(Session[gblValue.UserId]);
                
                vLoanAmt = Convert.ToDouble(txtLoanAmtF.Text); vAction = txtActionF.Text;
                vROI = Convert.ToDouble(txtROIF.Text); vRemarks = txtHoldRemarksF.Text;
                vTenure = Convert.ToDouble(txtTenureF.Text);
                vInternalScore = Convert.ToDouble(txtIntRiskScoreF.Text);

                vSacComm = ddlSacComm.SelectedValue;
                vSacRemarks = txtSacRemarks.Text;

                DateTime vSacDt = gblFuction.setDate(txtSacDt.Text.ToString());
                vRecommRemarks = ddlRecommRemarks.SelectedValue;


                if (lblFileNameF.Text != "")
                {
                    vIsUpload = "Y";
                }
                if (lblCAMFileName.Text != "")
                {
                    vIsCamAtt = "Y";
                }

                oDis = new CDistrict();
               // vErr = oDis.CF_SaveFinalDecision(vLeadId, vBrCode, vLoanAmt, vROI, vTenure, vInternalScore, vAction, vRemarks, UID, Mode, 0, ref vErrMsg, vSacComm, vSacRemarks, vSacDt, vRecommRemarks, vIsUpload, vIsCamAtt, vIsLetter);
                if (vErr == 0)
                {
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(vErrMsg);
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

        protected void btnSacLetter_Click(object sender, EventArgs e)
        {
            gblFuction.AjxMsgPopup("File will be provided by USFB...");
            return ;
        }
        protected void btnCam_Click(object sender, EventArgs e)
        {
            string ActNetImage = "", vFile = "";
            string[] ActNetPath = CFLeadBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblCAMFileName.Text;
                if (ValidUrlChk(ActNetImage))
                {
                    vFile = ActNetImage;
                    break;
                }
            }
            if (vFile != "")
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblCAMFileName.Text);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }

        protected void btnUPAttachment_Click(object sender, EventArgs e)
        {
           ViewImgDoc(hdLeadId.Value, hdnFileNameF.Value);
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
                gblFuction.MsgPopup("No File Found");
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