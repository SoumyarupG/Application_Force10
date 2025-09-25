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
    public partial class CF_LTVComputation : CENTRUMBAse
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
                
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                hdnMaxFileSize.Value = MaxFileSize;
              
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                    GenerateLTVComputationGrid(Convert.ToInt64(hdLeadId.Value));
                    StatusButton("Show");
                    CheckOprtnStatus(Convert.ToInt64(hdLeadId.Value));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
               
                tbBasicDet.ActiveTabIndex = 1;
                
            }
           
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "LTV Computation";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFLTVComputation);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = true;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    btnEdit.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {


                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "LTV Computation", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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
               // btnSave.Enabled = true;
                btnEdit.Enabled = true;
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
                if (txtQuotationCostVal.Text == "0")
                {
                    ViewState["StateEdit"] = "Add";
                }
                else
                {
                    ViewState["StateEdit"] = "Edit";
                }

                StatusButton("Edit");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbBasicDet.ActiveTabIndex = 1;
          //  EnableControl(false);
            StatusButton("Show");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        private void GenerateLTVComputationGrid(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            CDistrict ODis = null;
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }

                ODis = new CDistrict();
                ds = ODis.CF_GetLTVComputationByLeadID(Convert.ToInt64(LeadID));
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ViewState["LTVCal"] = dt;
                    hdLoanAmount.Value = dt.Rows[0]["LoanAmount"].ToString();
                    hdTotalCharges.Value = dt.Rows[0]["TotalCharges"].ToString();
                    hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);

                    txtQuotationCostVal.Text = dt.Rows[0]["QuotationCostVal"].ToString();
                    txtGovCostVal.Text = dt.Rows[0]["GovCostVal"].ToString();

                    //txtMaxEligibleFundingA.Text = dt.Rows[0]["QuotationCostAllowFundPercent"].ToString();
                    //txtMaxEligibleFundingB.Text = dt.Rows[0]["GovCostAllowFundPercent"].ToString();

                    txtMaxEligibleFundingA.Text = "80";
                    txtMaxEligibleFundingB.Text = "90";

                    txtMarginMoney.Text = dt.Rows[0]["MarginMoneyAmount"].ToString();
                    lblMarMon.Text = dt.Rows[0]["MoneyMarginReceiptFN"].ToString();
                    lblMarMonBank.Text = dt.Rows[0]["MoneyMarginBankRflctnFN"].ToString();
                    lblQuotCopy.Text = dt.Rows[0]["QuationCopyFN"].ToString();

                    hdnMarMon.Value = dt.Rows[0]["MoneyMarginReceiptFN"].ToString();
                    hdnMarMonBank.Value = dt.Rows[0]["MoneyMarginBankRflctnFN"].ToString();
                    hdnQuotCopy.Value = dt.Rows[0]["QuationCopyFN"].ToString();

                    Calculation();

                }
                else
                {
                    txtQuotationCostVal.Text = "0";
                    txtGovCostVal.Text = "0";
                    txtMaxEligibleFundingA.Text = "80";
                    txtMaxEligibleFundingB.Text = "90";
                    txtMarginMoney.Text = "0";
                    txtAllowedFundingA.Text = "0";
                    txtAllowedFundingB.Text = "0";
                    txtMaxAllowedFunding.Text = "0";
                    txtFinalLTVWithoutInsuranceA.Text = "0";
                    txtFinalLTVWithInsuranceA.Text = "0";
                }

                tbBasicDet.ActiveTabIndex = 1;
               // StatusButton("Show");
            }

            finally
            {
            }
        }
        protected void txtQuotationCostVal_textChanged(object sender, EventArgs e)
        {

            Calculation();
            // UpFamily.Update();
        }
        protected void txtGovCostVal_textChanged(object sender, EventArgs e)
        {

            Calculation();
            // UpFamily.Update();
        }
        protected void txtMaxEligibleFundingA_textChanged(object sender, EventArgs e)
        {

            Calculation();
            // UpFamily.Update();
        }
        protected void txtMaxEligibleFundingB_TextChanged(object sender, EventArgs e)
        {

            Calculation();
            // UpFamily.Update();
        }
        private void Calculation()
        {
            double vAlloweFundA = 0, vAlloweFundB = 0, vMaxAlloweFund = 0, vQval = 0, vGval = 0,
            vFinalLTVwithout = 0, vFinalLTVwith = 0, vLoanAmount = 0, VInsurCh = 0, vFundA = 0, vFundB;
            if (txtQuotationCostVal.Text == "") txtQuotationCostVal.Text = "0";
            if (txtGovCostVal.Text == "") txtGovCostVal.Text = "0";
            if (txtMaxEligibleFundingA.Text == "") txtMaxEligibleFundingA.Text = "0";
            if (txtMaxEligibleFundingB.Text == "") txtMaxEligibleFundingB.Text = "0";

            vLoanAmount = Convert.ToDouble(hdLoanAmount.Value);
            VInsurCh = Convert.ToDouble(hdTotalCharges.Value);
            vQval = Convert.ToDouble(txtQuotationCostVal.Text);
            vGval = Convert.ToDouble(txtGovCostVal.Text);
            vFundA = Convert.ToDouble(txtMaxEligibleFundingA.Text);
            vFundB = Convert.ToDouble(txtMaxEligibleFundingB.Text);

            txtAllowedFundingA.Text = ""; txtAllowedFundingB.Text = ""; txtMaxAllowedFunding.Text = "";
            txtFinalLTVWithoutInsuranceA.Text = ""; txtFinalLTVWithInsuranceA.Text = "";


            if (vQval > 0 && vFundA > 0)
            {
                vAlloweFundA = (vQval * vFundA) / 100;
            }
            if (vGval > 0 && vFundB > 0)
            {
                vAlloweFundB = (vGval * vFundB) / 100;
            }
            if (vAlloweFundA > 0 && vAlloweFundB > 0)
            {
                if (vAlloweFundA < vAlloweFundB)
                    vMaxAlloweFund = vAlloweFundA;
                else
                    vMaxAlloweFund = vAlloweFundB;

                if (vLoanAmount > 0)
                {
                    vFinalLTVwithout = (vLoanAmount/vMaxAlloweFund)* 100 ;
                    vFinalLTVwith = ((vLoanAmount + VInsurCh) / vMaxAlloweFund) * 100;
                }
                else
                {
                    vFinalLTVwithout = 0;
                    vFinalLTVwith = 0;
                }
            }
            else
            {
                vMaxAlloweFund = 0;
                vFinalLTVwithout = 0;
                vFinalLTVwith = 0;
            }
            txtAllowedFundingA.Text = Convert.ToString(Math.Round(vAlloweFundA, 2));
            txtAllowedFundingB.Text = Convert.ToString(Math.Round(vAlloweFundB, 2));

            txtMaxAllowedFunding.Text = Convert.ToString(Math.Round(vMaxAlloweFund, 2));

            txtFinalLTVWithoutInsuranceA.Text = Convert.ToString(Math.Round(vFinalLTVwithout, 2));

            txtFinalLTVWithInsuranceA.Text = Convert.ToString(Math.Round(vFinalLTVwith, 2));

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
             //   LoadBasicDetailsList(1);
                StatusButton("View");
                ViewState["StateEdit"] = null;
               // ClearControls();
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0;
            string vMarMonIsUpLoad = "", vMarMonBankIsUpLoad = "", vQuotCopyIsUpLoad = "", vMarMonFileNm, vMarMonBankFileNm, vQuotCopyFileNm,
            vErrMsg = "", vBrCode = "",  vFileStorePath = "",
            vMarMonFileExt = "", vMarMonBankFileExt = "", vQuotCopyExt = "";
            double vQuotationCostVal = 0, vGovCostVal = 0, vMaxEligibleFundingA = 0, vMarginMoney = 0, vMaxEligibleFundingB = 0;
            Int32 vMaxFileSize = 0; Int64 vLeadId = 0;
            CDistrict oDis = null;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            vMarMonFileNm = "MarMon";
            vMarMonBankFileNm = "MarMonBank";
            vQuotCopyFileNm = "QuotCopy";

            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }

                vBrCode = Session[gblValue.BrnchCode].ToString();
                vQuotationCostVal = Convert.ToDouble(txtQuotationCostVal.Text);
                vGovCostVal = Convert.ToDouble(txtGovCostVal.Text);
                vMaxEligibleFundingA = Convert.ToDouble(txtMaxEligibleFundingA.Text);
                vMaxEligibleFundingB = Convert.ToDouble(txtMaxEligibleFundingB.Text);
                vMarginMoney = Convert.ToDouble(txtMarginMoney.Text);
                vFileStorePath = DocumentBucketURL;

                #region FileUpload
                //START FOR MarMon Upload
                vMarMonIsUpLoad = fuMarMon.HasFile == true ? "Y" : "N";
                if (vMarMonIsUpLoad == "Y")
                {
                    
                    vMarMonFileExt = System.IO.Path.GetExtension(fuMarMon.PostedFile.FileName);
                    vMarMonFileNm = fuMarMon.HasFile == true ? vMarMonFileNm + vMarMonFileExt : "";
                    byte[] vfuMarMon = fuMarMon.FileBytes;
                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vMarMonFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(vfuMarMon);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuMarMon.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                }

                else
                {
                    if (Mode == "Edit")
                    {
                        if (lblMarMon.Text != "")
                        {
                            vMarMonIsUpLoad = "Y";
                            vMarMonFileNm = lblMarMon.Text;
                        }
                        else
                        {
                            vMarMonIsUpLoad = "N";
                            vMarMonFileNm = "";
                        }
                    }
                    else
                    {
                        vMarMonFileNm = "";
                    }
                }
                
                
                // END For MarMon


                //START FOR MarMon Upload
               
                vMarMonBankIsUpLoad = fuMarMonBank.HasFile == true ? "Y" : "N";
                if (vMarMonBankIsUpLoad == "Y")
                {
                   
                    vMarMonBankFileExt = System.IO.Path.GetExtension(fuMarMonBank.PostedFile.FileName);
                    vMarMonBankFileNm = fuMarMonBank.HasFile == true ? vMarMonBankFileNm + vMarMonBankFileExt : "";
                    byte[] vfuMarMonBank = fuMarMonBank.FileBytes;
                   
                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vMarMonBankFileExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(vfuMarMonBank);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuMarMonBank.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                }
                else
                {
                    if (Mode == "Edit")
                    {
                        if (lblMarMonBank.Text != "")
                        {
                            vMarMonBankIsUpLoad = "Y";
                            vMarMonBankFileNm = lblMarMonBank.Text;
                        }
                        else
                        {
                            vMarMonBankFileNm = "";
                            vMarMonBankIsUpLoad = "N";
                        }
                    }
                    else
                    {
                        vMarMonBankFileNm = "";
                    }
                }
                
                // END For MarMon

                //START FOR QuotCopy Upload
                
                vQuotCopyIsUpLoad = fuQuotCopy.HasFile == true ? "Y" : "N";
                if (vQuotCopyIsUpLoad == "Y")
                {
                    
                    vQuotCopyExt = System.IO.Path.GetExtension(fuQuotCopy.PostedFile.FileName);
                    vQuotCopyFileNm = fuQuotCopy.HasFile == true ? vQuotCopyFileNm + vQuotCopyExt : "";
                    byte[] vfuQuotCopy = fuQuotCopy.FileBytes;

                    //---------------------------------------------------------------
                    bool ValidPdf = false;
                    if (vQuotCopyExt.ToLower() == ".pdf")
                    {
                        cFileValidate oFile = new cFileValidate();
                        ValidPdf = oFile.ValidatePdf(vfuQuotCopy);
                        if (ValidPdf == false)
                        {
                            gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                            fuQuotCopy.Focus();
                            return false;
                        }
                    }
                    //------------------------------------------------------------
                }
                else
                {
                    if (Mode == "Edit")
                    {
                        if (lblQuotCopy.Text != "")
                        {
                            vQuotCopyIsUpLoad = "Y";
                            vQuotCopyFileNm = lblQuotCopy.Text;
                        }
                        else
                        {
                            vQuotCopyFileNm = "";
                            vQuotCopyIsUpLoad = "N";
                        }
                    }
                    else
                    {
                        vQuotCopyFileNm = "";
                    }
                }
                
                // END For QuotCopy
                #endregion


                oDis = new CDistrict();
                vErr = oDis.CF_SaveLTVComputation(vLeadId, vQuotationCostVal, vGovCostVal, vMaxEligibleFundingA, vMaxEligibleFundingB, vMarginMoney, vMarMonFileNm, vMarMonIsUpLoad, vMarMonBankFileNm, vMarMonBankIsUpLoad, vQuotCopyFileNm, vQuotCopyIsUpLoad, vFileStorePath, vBrCode, this.UserID, Mode, 0, ref vErrMsg);
                if (vErr == 0)
                {
                        if (fuMarMon.HasFile)
                        {
                            SaveMemberImages(fuMarMon, hdLeadId.Value, hdLeadId.Value + "_MarMon", vMarMonFileExt, "N");
                        }
                        if (fuMarMonBank.HasFile)
                        {
                            SaveMemberImages(fuMarMonBank, hdLeadId.Value, hdLeadId.Value + "_MarMonBank", vMarMonBankFileExt, "N");
                        }
                        if (fuQuotCopy.HasFile)
                        {
                            SaveMemberImages(fuQuotCopy, hdLeadId.Value, hdLeadId.Value + "_QuotCopy", vQuotCopyExt, "N");
                        }

                   
                    gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                    vResult = true;
                }
                else
                {
                    gblFuction.MsgPopup(vErrMsg);
                    vResult = false;
                }


                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDis = null;
            }
        }
        #endregion        
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Show":
                    EnableControl(false);
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    EnableControl(true);
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    break;
                case "View":
                    EnableControl(false);
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    txtSearch.Enabled = true;
                    break;
                case "Exit":
                    btnEdit.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":                 
                    btnEdit.Visible = false;                  
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void EnableControl(Boolean Status)
        {
            txtQuotationCostVal.Enabled = Status;
            txtGovCostVal.Enabled = Status;
            txtMaxEligibleFundingA.Enabled = false;
            txtMaxEligibleFundingB.Enabled = false;
            txtMarginMoney.Enabled = Status;
            fuMarMon.Enabled = Status;
            fuMarMonBank.Enabled = Status;
            fuQuotCopy.Enabled = Status;

        }
        private void ClearControls()
        {
            lblBCPNum.Text = "";
            lblApplNm.Text = "";
            txtQuotationCostVal.Text = "0";
            txtGovCostVal.Text = "0";
            txtMaxEligibleFundingA.Text = "80";
            txtMaxEligibleFundingB.Text = "90";
            txtMarginMoney.Text = "0";
            txtAllowedFundingA.Text = "0";
            txtAllowedFundingB.Text = "0";
            txtMaxAllowedFunding.Text = "0";
            txtFinalLTVWithoutInsuranceA.Text = "0";
            txtFinalLTVWithInsuranceA.Text = "0";

        }
        protected void imgMarMon_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnMarMon.Value);
        }
        protected void imgMarMonBank_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnMarMonBank.Value);
        }
        protected void imgQuotCopy_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnQuotCopy.Value);
        }
        protected void ViewImgDoc(string ID, string FileName)
        {
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + ID + "_" + FileName;
            // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);

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
        public string GetBase64Image(string pImageName, string pLeadId)
        {
            string ActNetImage = "", base64image = "";
            byte[] imgByte = null;
            try
            {
                string[] ActNetPath = DocumentBucketURL.Split(',');
                for (int j = 0; j <= ActNetPath.Length - 1; j++)
                {
                    ActNetImage = ActNetPath[j] + pLeadId + "_" + pImageName;
                    if (ValidUrlChk(ActNetImage))
                    {
                        imgByte = DownloadByteImage(ActNetImage);
                        base64image = Convert.ToBase64String(imgByte);
                        break;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return base64image;
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
        #region URLToByte
        public byte[] DownloadByteImage(string URL)
        {
            byte[] imgByte = null;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var wc = new WebClient())
                {
                    imgByte = wc.DownloadData(URL);
                }
            }
            finally { }
            return imgByte;
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