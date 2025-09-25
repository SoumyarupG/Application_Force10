using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CENTRUMCA;
using CENTRUMBA;
using System.Data;
using System.IO;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Reporting.WebForms;
using System.Net;
using CENTRUMCF.Service_Equifax;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Xsl;
using System.Web.Hosting;


namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_ObligationDetails : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string MobUrl = ConfigurationManager.AppSettings["MobUrl"];
        string pathImage = "", CCRUserName = "", CCRPassword = "", PCSUserName = "", PCSPassword = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            if (!IsPostBack)
            {
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["Grid"] = null;

                pnlCBDtls.Enabled = false;
                pnlUSFBDtls.Enabled = false;
                pnlOD.Enabled = false;
                hdnMaxFileSize.Value = MaxFileSize;
                if (Session[gblValue.LeadID] != null)
                {
                    GetObligationDtls();
                    StatusButton("Show");

                    Int64 LeadId = Convert.ToInt64(Session[gblValue.LeadID]);
                    CheckOprtnStatus(Convert.ToInt64(LeadId));
                }
                else
                {
                    StatusButton("Close");
                    gblFuction.MsgPopup("Please Select Lead from Loan Application Status screen and proceed...");
                    return;
                }
                tbObligationMaster.ActiveTabIndex = 0;

               
            }

        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Obligation Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFObligationDtl);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    btnEdit.Visible = false;
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Obligation Details", false);
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

                case "Show":
                    EnableControl(false, this.Page);
                    ImgApp.Enabled = true;
                    ImgCoApp.Enabled = true;
                    ImgGua.Enabled = true;
                    ImgCoApp2.Enabled = true;
                    btnDownloadApplEquifax.Enabled = true;
                    btnDownloadCA1Equifax.Enabled = true;
                    btnDownloadCA2Equifax.Enabled = true;
                    btnDownloadGuarEquifax.Enabled = true;
                    btnEdit.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvObli.Enabled = false;
                    break;

                case "View":
                    EnableControl(false, this.Page);
                    ImgApp.Enabled = true;
                    ImgCoApp.Enabled = true;
                    ImgGua.Enabled = true;
                    ImgCoApp2.Enabled = true;
                    btnDownloadApplEquifax.Enabled = true;
                    btnDownloadCA1Equifax.Enabled = true;
                    btnDownloadCA2Equifax.Enabled = true;
                    btnDownloadGuarEquifax.Enabled = true;
                    btnEdit.Enabled = false;

                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    gvObli.Enabled = false;
                    break;
                case "Exit":
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Edit":
                    EnableControl(true, this.Page);
                    btnEdit.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    gvObli.Enabled = true;
                    break;
                case "Close":
                    btnEdit.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false, this.Page);
                    break;
            }
        }
        private void EnableControl(Boolean Status, Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Enabled = Status;
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).Enabled = Status;
                }
                else if (c is FileUpload)
                {
                    ((FileUpload)c).Enabled = Status;
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    EnableControl(Status, c);
                }
            }

        }
        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                if (c is Label)
                {
                    ((Label)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }
                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbObligationMaster.ActiveTabIndex = 0;           
            StatusButton("Show");
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
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

                if (txtAppScore.Text == "0")
                {
                    ViewState["StateEdit"] = "Add";
                }
                else
                {
                    ViewState["StateEdit"] = "Edit";
                }                
                StatusButton("Edit");
                Visibility(hdnCoApp1.Value, hdnCoApp2.Value, hdnGrnr.Value, hdnCoApp3.Value, hdnCoApp4.Value);
            }
            catch (Exception ex)
            {
                throw ex;
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
                //btnSave.Enabled = true;
                btnEdit.Enabled = true;
            }
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
                ViewState["StateEdit"] = null;

                GetObligationDtls();
                StatusButton("Show");
                int activeTabIndex;
                if (int.TryParse(hdActiveTab.Value, out activeTabIndex))
                {
                    tbObligationMaster.ActiveTabIndex = activeTabIndex;
                }
              
            }
        }

        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0;
            string vAppFileNm = "", vAppIsUpLoad = "", vAppScore = "", vCoApp1FileNm = "", vCoApp1IsUpLoad = "", vCoApp1Score = "", vCoApp2FileNm = "", vCoApp2IsUpLoad = "",
                vGFileNm = "", vGIsUpLoad = "", vGScore = "", vErrMsg = "", vBrCode = "", vFileStorePath = "", vCoApp2Score = "", vCoApp3Score = "", vCoApp4Score = "",
             vAppFileExt = "", vCoApp1FileExt = "", vCoApp2FileExt = "", vGuaFileExt = "", vCoApp3FileNm = "", vCoApp3IsUpLoad = "", vCoApp3FileExt = "",
             vCoApp4FileNm = "", vCoApp4IsUpLoad = "", vCoApp4FileExt = "";                     
            DataTable dt = null; DateTime vAppDt = gblFuction.setDate(""), vCoAppDt = gblFuction.setDate(""), vCoApp2Dt = gblFuction.setDate(""), vGuaDt = gblFuction.setDate("");
            string vXmlObli = ""; Int64 vLeadId = 0;
            Int32 vMaxFileSize = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            CMember oMem = null;

            vAppDt = gblFuction.setDate(txtAppDt.Text.ToString());
            vCoAppDt = gblFuction.setDate(txtCoAppDt.Text.ToString());
            vCoApp2Dt = gblFuction.setDate(txtCoApp2Dt.Text.ToString());
            DateTime vCoApp3Dt = gblFuction.setDate(txtCoApp3Dt.Text.ToString());
            DateTime vCoApp4Dt = gblFuction.setDate(txtCoApp4Dt.Text.ToString());
            vGuaDt = gblFuction.setDate(txtGuaDt.Text.ToString());

            DateTime vAppExDt = gblFuction.setDate(txtAppExDt.Text.ToString());
            DateTime vCoAppExDt = gblFuction.setDate(txtCoAppExDt.Text.ToString());
            DateTime vCoApp2ExDt = gblFuction.setDate(txtCoApp2ExDt.Text.ToString());
            DateTime vCoApp3ExDt = gblFuction.setDate(txtCoApp3ExDt.Text.ToString());
            DateTime vCoApp4ExDt = gblFuction.setDate(txtCoApp4ExDt.Text.ToString());
            DateTime vGuaExDt = gblFuction.setDate(txtGuaExDt.Text.ToString());

            if (tbObligationMaster.ActiveTabIndex == 0) hdActiveTab.Value = "0";
            else if (tbObligationMaster.ActiveTabIndex == 1) hdActiveTab.Value = "1";
            else if (tbObligationMaster.ActiveTabIndex == 2) hdActiveTab.Value = "2";
          

            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }

                vBrCode = Session[gblValue.BrnchCode].ToString();


                if (tbObligationMaster.ActiveTabIndex == 2)
                {

                    if (GetData() == true)
                    {
                        dt = (DataTable)ViewState["Obligation"];
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                Int32 vNumOfObli = 0;
                                DataRow[] vrows;
                                vrows = dt.Select("LoanType <> '' and LoanType <> '-1'");
                                vNumOfObli = vrows.Length;
                                if (vNumOfObli == 0)
                                {
                                    gblFuction.MsgPopup("Please insert atleast one Obligation Details");
                                    return false;
                                }

                                dt.AcceptChanges();
                                dt.TableName = "Table";
                                using (StringWriter oSW = new StringWriter())
                                {
                                    dt.WriteXml(oSW);
                                    vXmlObli = oSW.ToString();
                                }
                                oMem = new CMember();

                                vErr = oMem.CF_SaveObligationDtls(vLeadId, vXmlObli, vBrCode, this.UserID, 0, ref vErrMsg);
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
                        }
                    }

                }
                else if (tbObligationMaster.ActiveTabIndex == 0)
                {
                    vAppFileNm = "Applicant";
                    vCoApp1FileNm = "CoApplicant1";
                    vCoApp2FileNm = "CoApplicant2";
                    vCoApp3FileNm = "CoApplicant3";
                    vCoApp4FileNm = "CoApplicant4";
                    vGFileNm = "Gurantor";
                    vAppScore = txtAppScore.Text.ToString().Trim();
                    vCoApp1Score = txtCoAppScore.Text.ToString().Trim();
                    vCoApp2Score = txtApp2Score.Text.ToString().Trim();
                    vCoApp3Score = txtApp3Score.Text.ToString().Trim();
                    vCoApp4Score = txtApp4Score.Text.ToString().Trim();
                    vGScore = txtGuaScore.Text.ToString().Trim();

                    vFileStorePath = DocumentBucketURL;

                    #region FileUpload
                    //START FOR Applicant Upload

                    vAppIsUpLoad = fuAppUpld.HasFile == true ? "Y" : "N";
                    if (vAppIsUpLoad == "Y")
                    {
                        vAppFileExt = System.IO.Path.GetExtension(fuAppUpld.PostedFile.FileName);
                        vAppFileNm = fuAppUpld.HasFile == true ? vAppFileNm + vAppFileExt : "";
                        lblAppFileNm.Text = vAppFileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vAppFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuAppUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuAppUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblAppFileNm.Text != "")
                            {
                                vAppIsUpLoad = "Y";
                                vAppFileNm = lblAppFileNm.Text;
                            }
                            else
                            {
                                vAppFileNm = "";
                                vAppIsUpLoad = "N";
                            }
                        }
                        else
                        {
                            vAppFileNm = "";
                        }

                    }

                    // END For Applicant


                    //START FOR CoApplicant1 Upload

                    vCoApp1IsUpLoad = fuCoAppUpld.HasFile == true ? "Y" : "N";
                    if (vCoApp1IsUpLoad == "Y")
                    {
                        vCoApp1FileExt = System.IO.Path.GetExtension(fuCoAppUpld.PostedFile.FileName);
                        vCoApp1FileNm = fuCoAppUpld.HasFile == true ? vCoApp1FileNm + vCoApp1FileExt : "";
                        lblCoAppFileName.Text = vCoApp1FileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vCoApp1FileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuCoAppUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuCoAppUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblCoAppFileName.Text != "")
                            {
                                vCoApp1IsUpLoad = "Y";
                                vCoApp1FileNm = lblCoAppFileName.Text;
                            }
                            else
                            {
                                vCoApp1IsUpLoad = "N";
                                vCoApp1FileNm = "";
                            }
                        }
                        else
                        {
                            vCoApp1FileNm = "";
                        }
                    }

                    // END For CoApplicant

                    //START FOR CoApplicant2 Upload
                    vCoApp2IsUpLoad = fuCoApp2Upld.HasFile == true ? "Y" : "N";
                    if (vCoApp2IsUpLoad == "Y")
                    {
                        vCoApp2FileExt = System.IO.Path.GetExtension(fuCoApp2Upld.PostedFile.FileName);
                        vCoApp2FileNm = fuCoApp2Upld.HasFile == true ? vCoApp2FileNm + vCoApp2FileExt : "";
                        lblCoApp2FileName.Text = vCoApp2FileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vCoApp2FileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuCoApp2Upld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuCoApp2Upld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblCoApp2FileName.Text != "")
                            {
                                vCoApp2IsUpLoad = "Y";
                                vCoApp2FileNm = lblCoApp2FileName.Text;
                            }
                            else
                            {
                                vCoApp2FileNm = "";
                                vCoApp2IsUpLoad = "N";
                            }
                        }
                        else
                        {
                            vCoApp2FileNm = "";
                        }
                    }
                    // END For CoApplicant2

                    //START FOR CoApplicant3 Upload
                    vCoApp3IsUpLoad = fuCoApp3Upld.HasFile == true ? "Y" : "N";
                    if (vCoApp3IsUpLoad == "Y")
                    {
                        vCoApp3FileExt = System.IO.Path.GetExtension(fuCoApp3Upld.PostedFile.FileName);
                        vCoApp3FileNm = fuCoApp3Upld.HasFile == true ? vCoApp3FileNm + vCoApp3FileExt : "";
                        lblCoApp3FileName.Text = vCoApp3FileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vCoApp3FileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuCoApp3Upld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuCoApp3Upld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblCoApp3FileName.Text != "")
                            {
                                vCoApp3IsUpLoad = "Y";
                                vCoApp3FileNm = lblCoApp3FileName.Text;
                            }
                            else
                            {
                                vCoApp3FileNm = "";
                                vCoApp3IsUpLoad = "N";
                            }
                        }
                        else
                        {
                            vCoApp3FileNm = "";
                        }
                    }
                    // END For CoApplicant3

                    //START FOR CoApplicant4 Upload
                    vCoApp4IsUpLoad = fuCoApp4Upld.HasFile == true ? "Y" : "N";
                    if (vCoApp4IsUpLoad == "Y")
                    {
                        vCoApp4FileExt = System.IO.Path.GetExtension(fuCoApp4Upld.PostedFile.FileName);
                        vCoApp4FileNm = fuCoApp4Upld.HasFile == true ? vCoApp4FileNm + vCoApp4FileExt : "";
                        lblCoApp4FileName.Text = vCoApp4FileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vCoApp4FileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuCoApp4Upld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuCoApp4Upld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblCoApp4FileName.Text != "")
                            {
                                vCoApp4IsUpLoad = "Y";
                                vCoApp4FileNm = lblCoApp4FileName.Text;
                            }
                            else
                            {
                                vCoApp4FileNm = "";
                                vCoApp4IsUpLoad = "N";
                            }
                        }
                        else
                        {
                            vCoApp4FileNm = "";
                        }
                    }
                    // END For CoApplicant4

                    //START FOR Gurantor Upload

                    vGIsUpLoad = fuGuaUpld.HasFile == true ? "Y" : "N";
                    if (vGIsUpLoad == "Y")
                    {
                        vGuaFileExt = System.IO.Path.GetExtension(fuGuaUpld.PostedFile.FileName);
                        vGFileNm = fuGuaUpld.HasFile == true ? vGFileNm + vGuaFileExt : "";
                        lblGuaFileName.Text = vGFileNm;

                        //---------------------------------------------------------------
                        bool ValidPdf = false;
                        if (vGuaFileExt.ToLower() == ".pdf")
                        {
                            cFileValidate oFile = new cFileValidate();
                            ValidPdf = oFile.ValidatePdf(fuGuaUpld.FileBytes);
                            if (ValidPdf == false)
                            {
                                gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                fuGuaUpld.Focus();
                                return false;
                            }
                        }
                        //------------------------------------------------------------
                    }
                    else
                    {
                        if (Mode == "Edit")
                        {
                            if (lblGuaFileName.Text != "")
                            {
                                vGIsUpLoad = "Y";
                                vGFileNm = lblGuaFileName.Text;
                            }
                            else
                            {
                                vGFileNm = "";
                                vGIsUpLoad = "N";
                            }
                        }
                        else
                        {
                            vGFileNm = "";
                        }

                    }

                    // END For Applicant

                    #endregion

                    oMem = new CMember();
                    vErr = oMem.CF_SaveBCCBRpt(vLeadId, vAppFileNm, vAppIsUpLoad, vAppScore, vCoApp1FileNm, vCoApp1IsUpLoad, vCoApp1Score, vGFileNm, vGIsUpLoad, vGScore, vFileStorePath, vBrCode, vCoApp2FileNm, vCoApp2IsUpLoad, vCoApp2Score, this.UserID, Mode, 0, ref vErrMsg, vCoApp3FileNm, vCoApp3IsUpLoad, vCoApp3Score, vCoApp4FileNm, vCoApp4IsUpLoad, vCoApp4Score);
                    if (vErr == 0)
                    {

                        if (fuAppUpld.HasFile)
                        {
                            if (vAppFileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuAppUpld, hdLeadId.Value, hdLeadId.Value + "_Applicant", vAppFileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuAppUpld, hdLeadId.Value, "Applicant", vAppFileExt, "N");
                            }
                        }
                        if (fuCoAppUpld.HasFile)
                        {
                            if (vCoApp1FileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuCoAppUpld, hdLeadId.Value, hdLeadId.Value + "_CoApplicant1", vCoApp1FileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuCoAppUpld, hdLeadId.Value, "CoApplicant1", vCoApp1FileExt, "N");
                            }
                        }
                        if (fuGuaUpld.HasFile)
                        {
                            if (vGuaFileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuGuaUpld, hdLeadId.Value, hdLeadId.Value + "_Gurantor", vGuaFileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuGuaUpld, hdLeadId.Value, "Gurantor", vGuaFileExt, "N");
                            }
                        }
                        if (fuCoApp2Upld.HasFile)
                        {
                            if (vCoApp2FileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuCoApp2Upld, hdLeadId.Value, hdLeadId.Value + "_CoApplicant2", vCoApp2FileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuCoApp2Upld, hdLeadId.Value, "CoApplicant2", vCoApp2FileExt, "N");
                            }
                        }
                        if (fuCoApp3Upld.HasFile)
                        {
                            if (vCoApp3FileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuCoApp3Upld, hdLeadId.Value, hdLeadId.Value + "_CoApplicant3", vCoApp3FileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuCoApp3Upld, hdLeadId.Value, "CoApplicant3", vCoApp3FileExt, "N");
                            }
                        }
                        if (fuCoApp4Upld.HasFile)
                        {
                            if (vCoApp4FileExt.ToLower().Contains(".pdf"))
                            {
                                SaveMemberImages(fuCoApp4Upld, hdLeadId.Value, hdLeadId.Value + "_CoApplicant4", vCoApp4FileExt, "N");
                            }
                            else
                            {
                                SaveMemberImages(fuCoApp4Upld, hdLeadId.Value, "CoApplicant4", vCoApp4FileExt, "N");
                            }
                        }

                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrMsg);
                        vResult = false;
                    }

                }
                else if (tbObligationMaster.ActiveTabIndex == 1)
                {
                    oMem = new CMember();
                    vErr = oMem.CF_SaveBankRpt(vLeadId, hdnApplVerifyEquiYN.Value, Convert.ToInt32(txtApplCBScore.Text), vAppDt, hdnCA1VerifyEquiYN.Value
                        , Convert.ToInt32(txtCA1CBScore.Text), vCoAppDt, hdnCA2VerifyEquiYN.Value, Convert.ToInt32(txtCA2CBScore.Text), vCoApp2Dt,
                        hdnGuarVerifyEquiYN.Value, Convert.ToInt32(txtGuarCBScore.Text), vGuaDt, vBrCode,
                        this.UserID, Mode, 0, ref vErrMsg, vAppExDt, vCoAppExDt, vCoApp2ExDt, vGuaExDt, hdnCA3VerifyEquiYN.Value, Convert.ToInt32(txtCA3CBScore.Text), vCoApp3Dt, vCoApp3ExDt
                        , hdnCA4VerifyEquiYN.Value, Convert.ToInt32(txtCA4CBScore.Text), vCoApp4Dt, vCoApp4ExDt);
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

                return vResult;
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
        #region GetObligationDtls
        protected void GetObligationDtls()
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            ClearControls(this.Page);
            CMember oMem = null;
            try
            {
                if (Session[gblValue.ApplNm] != null)
                {
                    lblApplicantName.Text = Convert.ToString(Session[gblValue.ApplNm]);
                    lblAppName.Text = Convert.ToString(Session[gblValue.ApplNm]);
                    lblApplNm.Text = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    lblBCPNum1.Text = Convert.ToString(Session[gblValue.BCPNO]);
                    lblBCPNum2.Text = Convert.ToString(Session[gblValue.BCPNO]);
                    lblBCPNum.Text = Convert.ToString(Session[gblValue.BCPNO]);
                }
                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                }

                oMem = new CMember();

                ds = oMem.CF_GetObligationDtls(Convert.ToInt64(hdLeadId.Value));
                dt = ds.Tables[0];
              
                if (dt.Rows.Count > 0)
                {
                    hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);

                    txtAppScore.Text = Convert.ToString(dt.Rows[0]["Apt_CBScore"]);
                    txtCoAppScore.Text = Convert.ToString(dt.Rows[0]["CoApt1_CBScore"]);
                    txtApp2Score.Text = Convert.ToString(dt.Rows[0]["CoApt2_CBScore"]);
                    txtApp3Score.Text = Convert.ToString(dt.Rows[0]["CoApt3_CBScore"]);
                    txtApp4Score.Text = Convert.ToString(dt.Rows[0]["CoApt4_CBScore"]);
                    txtGuaScore.Text = Convert.ToString(dt.Rows[0]["Grntr_CBScore"]);

                    lblAppFileNm.Text = Convert.ToString(dt.Rows[0]["Apt_CBRptFileName"]);
                    lblCoAppFileName.Text = Convert.ToString(dt.Rows[0]["CoApt1_CBRptFileName"]);
                    lblCoApp2FileName.Text = Convert.ToString(dt.Rows[0]["CoApt2_CBRptFileName"]);
                    lblCoApp3FileName.Text = Convert.ToString(dt.Rows[0]["CoApt3_CBRptFileName"]);
                    lblCoApp4FileName.Text = Convert.ToString(dt.Rows[0]["CoApt4_CBRptFileName"]);
                    lblGuaFileName.Text = Convert.ToString(dt.Rows[0]["Grntr_CBRptFileName"]);

                    hdnAppFileNm.Value = Convert.ToString(dt.Rows[0]["Apt_CBRptFileName"]);
                    hdnCoAppFileName.Value = Convert.ToString(dt.Rows[0]["CoApt1_CBRptFileName"]);
                    hdnCoApp2FileName.Value = Convert.ToString(dt.Rows[0]["CoApt2_CBRptFileName"]);
                    hdnCoApp3FileName.Value = Convert.ToString(dt.Rows[0]["CoApt3_CBRptFileName"]);
                    hdnCoApp4FileName.Value = Convert.ToString(dt.Rows[0]["CoApt4_CBRptFileName"]);
                    hdnGuaFileName.Value = Convert.ToString(dt.Rows[0]["Grntr_CBRptFileName"]);

                    hdFlag.Value = "Y";

                    hdnApplVerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["Apt_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["Apt_EquiYN"]) == "Y")
                    {
                        txtApplCBScore.Text = Convert.ToString(dt.Rows[0]["Ap_CBScore"]);
                        lblApplVerifyEquiStatus.Text = "Success";
                        btnVerifyApplEqui.Enabled = false;
                    }
                    else
                    {
                        txtApplCBScore.Text = Convert.ToString(dt.Rows[0]["Ap_CBScore"]);
                        lblApplVerifyEquiStatus.Text = "";
                        btnVerifyApplEqui.Enabled = true;
                    }
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["Apt_Dt"]);
                    txtAppExDt.Text = Convert.ToString(dt.Rows[0]["AppExDt"]);


                    hdnCA1VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoApt_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["CoApt_EquiYN"]) == "Y")
                    {
                        txtCA1CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp_CBScore"]);
                        lblCA1VerifyEquiStatus.Text = "Success";
                        btnVerifyCA1Equi.Enabled = false;
                    }
                    else
                    {
                        txtCA1CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp_CBScore"]);
                        lblCA1VerifyEquiStatus.Text = "";
                        btnVerifyCA1Equi.Enabled = true;
                    }
                    txtCoAppDt.Text = Convert.ToString(dt.Rows[0]["CoApt_Dt"]);
                    txtCoAppExDt.Text = Convert.ToString(dt.Rows[0]["CoAppExDt"]);


                    hdnCA2VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoApt2_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["CoApt2_EquiYN"]) == "Y")
                    {
                        txtCA2CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp2_CBScore"]);
                        lblCA2VerifyEquiStatus.Text = "Success";
                        btnVerifyCA2Equi.Enabled = false;
                    }
                    else
                    {
                        txtCA2CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp2_CBScore"]);
                        lblCA2VerifyEquiStatus.Text = "";
                        btnVerifyCA2Equi.Enabled = true;
                    }
                    txtCoApp2Dt.Text = Convert.ToString(dt.Rows[0]["CoApt2_Dt"]);
                    txtCoApp2ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp2ExDt"]);

                    hdnCA3VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]) == "Y")
                    {
                        txtCA3CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp3_CBScore"]);
                        lblCA3VerifyEquiStatus.Text = "Success";
                        btnVerifyCA3Equi.Enabled = false;
                    }
                    else
                    {
                        txtCA3CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp3_CBScore"]);
                        lblCA3VerifyEquiStatus.Text = "";
                        btnVerifyCA3Equi.Enabled = true;
                    }
                    txtCoApp3Dt.Text = Convert.ToString(dt.Rows[0]["CoApt3_Dt"]);
                    txtCoApp3ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp3ExDt"]);

                    hdnCA4VerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["CoAp4_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["CoAp3_EquiYN"]) == "Y")
                    {
                        txtCA4CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp4_CBScore"]);
                        lblCA4VerifyEquiStatus.Text = "Success";
                        btnVerifyCA4Equi.Enabled = false;
                    }
                    else
                    {
                        txtCA4CBScore.Text = Convert.ToString(dt.Rows[0]["CoAp4_CBScore"]);
                        lblCA4VerifyEquiStatus.Text = "";
                        btnVerifyCA4Equi.Enabled = true;
                    }
                    txtCoApp4Dt.Text = Convert.ToString(dt.Rows[0]["CoApt4_Dt"]);
                    txtCoApp4ExDt.Text = Convert.ToString(dt.Rows[0]["CoApp4ExDt"]);

                    hdnGuarVerifyEquiYN.Value = Convert.ToString(dt.Rows[0]["Grntr_EquiYN"]);
                    if (Convert.ToString(dt.Rows[0]["Grntr_EquiYN"]) == "Y")
                    {
                        txtGuarCBScore.Text = Convert.ToString(dt.Rows[0]["Grnt_CBScore"]);
                        lblGuarVerifyEquiStatus.Text = "Success";
                        btnVerifyGuarEqui.Enabled = false;
                    }
                    else
                    {
                        txtGuarCBScore.Text = Convert.ToString(dt.Rows[0]["Grnt_CBScore"]);
                        lblGuarVerifyEquiStatus.Text = "";
                        btnVerifyGuarEqui.Enabled = true;
                    }
                    txtGuaDt.Text = Convert.ToString(dt.Rows[0]["Grntr_Dt"]);
                    txtGuaExDt.Text = Convert.ToString(dt.Rows[0]["GuaExDt"]);


                    lblApplicantName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                    lblAppName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                    hdnCoApp1.Value = Convert.ToString(dt.Rows[0]["CA1Flag"]);
                    hdnCoApp2.Value = Convert.ToString(dt.Rows[0]["CA2Flag"]);
                    hdnCoApp3.Value = Convert.ToString(dt.Rows[0]["CA3Flag"]);
                    hdnCoApp4.Value = Convert.ToString(dt.Rows[0]["CA4Flag"]);
                    hdnGrnr.Value = Convert.ToString(dt.Rows[0]["GFlag"]);



                    pnlCBDtls.Enabled = true;
                    pnlUSFBDtls.Enabled = true;
                    pnlOD.Enabled = true;
                    GenerateObligationGrid();
                    tbObligationMaster.ActiveTabIndex = 0;                  
                }
                else
                {

                    //NOTE : LEAD IS NOT ELIGIBLE FOR OBLIGATION TILL IT BE A MEMBER.
                    pnlCBDtls.Enabled = false;
                    pnlUSFBDtls.Enabled = false;
                    pnlOD.Enabled = false;
                    tbObligationMaster.ActiveTabIndex = 0;
                    gblFuction.AjxMsgPopup("To Continue Please Fillup Customer 360....");
                    return;
                }


            }

            finally
            {
                dt = null;
            }
        }
        #endregion
        private void Visibility(string CoApp1, String CoApp2, string Grnr, String CoApp3, String CoApp4)
        {
            if (CoApp1 == "N")
            {
                fuCoAppUpld.Enabled = false;
                txtCoAppScore.Enabled = false;
                ImgCoApp.Enabled = false;
                txtCA1CBScore.Enabled = false;
                btnVerifyCA1Equi.Enabled = false;
                txtCoAppDt.Enabled = false;


            }
            if (CoApp2 == "N")
            {
                fuCoApp2Upld.Enabled = false;
                ImgCoApp2.Enabled = false;
                txtApp2Score.Enabled = false;
                txtCA2CBScore.Enabled = false;
                btnVerifyCA2Equi.Enabled = false;
                txtCoApp2Dt.Enabled = false;
            }
            if (Grnr == "N")
            {
                fuGuaUpld.Enabled = false;
                ImgGua.Enabled = false;
                txtGuaScore.Enabled = false;
                txtGuarCBScore.Enabled = false;
                btnVerifyGuarEqui.Enabled = false;
                txtGuaDt.Enabled = false;
            }
            if (CoApp3 == "N")
            {
                fuCoApp3Upld.Enabled = false;
                ImgCoApp3.Enabled = false;
                txtApp3Score.Enabled = false;
                txtCA3CBScore.Enabled = false;
                btnVerifyCA3Equi.Enabled = false;
                txtCoApp3Dt.Enabled = false;
            }
            if (CoApp4 == "N")
            {
                fuCoApp4Upld.Enabled = false;
                ImgCoApp4.Enabled = false;
                txtApp4Score.Enabled = false;
                txtCA4CBScore.Enabled = false;
                btnVerifyCA4Equi.Enabled = false;
                txtCoApp4Dt.Enabled = false;
            }
        }
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
        protected void imgApp_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnAppFileNm.Value);
        }
        protected void imgCoApp1_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoAppFileName.Value);
        }
        protected void imgCoApp2_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoApp2FileName.Value);
        }
        protected void imgCoApp3_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoApp3FileName.Value);
        }
        protected void imgCoApp4_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnCoApp4FileName.Value);
        }
        protected void imgGua_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnGuaFileName.Value);
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
        #region ObligationGrid

        private void GenerateObligationGrid()
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateObligationGrid(Convert.ToInt64(hdLeadId.Value));
                dt = ds.Tables[0];
                if (dt.Rows.Count == 0)
                {
                    DataRow dF;
                    dF = dt.NewRow();
                    dt.Rows.Add(dF);
                    dt.AcceptChanges();
                }               
                ViewState["Obligation"] = dt;
                gvObli.DataSource = dt;
                gvObli.DataBind();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        double TotalEMI = 0; lblTotalEMI.Text = "";
                        foreach (GridViewRow gr in gvObli.Rows)
                        {
                            DropDownList ddlFoir = (DropDownList)gvObli.Rows[gr.RowIndex].FindControl("ddlFoir");


                            TextBox txtEMI = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtEMI");

                            if (ddlFoir.SelectedValue == "Y")
                            {
                                TotalEMI += Convert.ToDouble(txtEMI.Text);
                            }
                        }
                        lblTotalEMI.Text = Convert.ToString(TotalEMI);
                    }
                }


            }

            finally
            {
            }
        }

        protected void gvObli_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                DropDownList ddlFoir = (DropDownList)e.Row.FindControl("ddlFoir");
                ddlFoir.SelectedIndex = ddlFoir.Items.IndexOf(ddlFoir.Items.FindByValue(e.Row.Cells[0].Text));

                Label lblSrlNo = (Label)e.Row.FindControl("lblSrlNo");
                lblSrlNo.Text = (e.Row.RowIndex + 1).ToString();

                TextBox txtLoanType = (TextBox)e.Row.FindControl("txtLoanType");
                TextBox txtBankName = (TextBox)e.Row.FindControl("txtBankName");

                TextBox txtLoanAmount = (TextBox)e.Row.FindControl("txtLoanAmount");
                TextBox txtCurrentPos = (TextBox)e.Row.FindControl("txtCurrentPos");

                TextBox txtEMI = (TextBox)e.Row.FindControl("txtEMI");
                TextBox txtRepaymntCmmnt = (TextBox)e.Row.FindControl("txtRepaymntCmmnt");


            }
        }

        protected void ddlFoir_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["Obligation"];
            double TotalEMI = 0; lblTotalEMI.Text = "";
            foreach (GridViewRow gr in gvObli.Rows)
            {
                DropDownList ddlFoir = (DropDownList)gvObli.Rows[gr.RowIndex].FindControl("ddlFoir");


                TextBox txtEMI = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtEMI");

                if (ddlFoir.SelectedValue == "Y")
                {
                    TotalEMI += Convert.ToDouble(txtEMI.Text);
                }
            }
            lblTotalEMI.Text = Convert.ToString(TotalEMI);


        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            DataRow dr;
            if (GetData() == true)
            {
                DataTable dt = (DataTable)ViewState["Obligation"];
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Obligation"] = dt;
                gvObli.DataSource = dt;
                gvObli.DataBind();
                UpFamily.Update();

            }
        }
        protected void btnAddNew_ClickOLD(object sender, EventArgs e)
        {

            DataTable dt = null;
            Int32 vR = 0;
            DataRow dr;
            dt = (DataTable)ViewState["Obligation"];
            if (dt.Rows.Count > 0)
            {
                vR = dt.Rows.Count - 1;

                Label lblSrlNo = (Label)gvObli.Rows[vR].FindControl("lblSrlNo");
                dt.Rows[vR]["SrlNo"] = lblSrlNo.Text == "" ? "0" : lblSrlNo.Text;

                DropDownList ddlFoir = (DropDownList)gvObli.Rows[vR].FindControl("ddlFoir");
                dt.Rows[vR]["Foir"] = ddlFoir.SelectedValue;

                TextBox txtLoanType = (TextBox)gvObli.Rows[vR].FindControl("txtLoanType");
                dt.Rows[vR]["LoanType"] = txtLoanType.Text == "" ? "" : txtLoanType.Text;

                TextBox txtBankName = (TextBox)gvObli.Rows[vR].FindControl("txtBankName");
                dt.Rows[vR]["BankName"] = txtBankName.Text == "" ? "" : txtBankName.Text;

                TextBox txtLoanAmount = (TextBox)gvObli.Rows[vR].FindControl("txtLoanAmount");
                dt.Rows[vR]["LoanAmount"] = txtLoanAmount.Text == "" ? "0" : txtLoanAmount.Text;

                TextBox txtCurrentPos = (TextBox)gvObli.Rows[vR].FindControl("txtCurrentPos");
                dt.Rows[vR]["CurrentPos"] = txtCurrentPos.Text == "" ? "0" : txtCurrentPos.Text;

                TextBox txtEMI = (TextBox)gvObli.Rows[vR].FindControl("txtEMI");
                dt.Rows[vR]["EMI"] = txtEMI.Text == "" ? "0" : txtEMI.Text;

                TextBox txtRepaymntCmmnt = (TextBox)gvObli.Rows[vR].FindControl("txtRepaymntCmmnt");
                dt.Rows[vR]["RepaymntCmmnt"] = txtRepaymntCmmnt.Text == "" ? "" : txtRepaymntCmmnt.Text;
            }

            if (dt.Rows[gvObli.Rows.Count - 1]["LoanType"].ToString() == "")
            {
                gblFuction.AjxMsgPopup("Loan Type is Blank ...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["BankName"].ToString() == "")
            {
                gblFuction.AjxMsgPopup("Bank Name is Blank...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["LoanAmount"].ToString() == "0")
            {
                gblFuction.AjxMsgPopup("Loan Amount is Blank...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["CurrentPos"].ToString() == "0")
            {
                gblFuction.AjxMsgPopup("Current Pos is Blank...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["Foir"].ToString() == "-1")
            {
                gblFuction.AjxMsgPopup("Foir is Blank...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["EMI"].ToString() == "-1")
            {
                gblFuction.AjxMsgPopup("EMI is Blank...");
                return;
            }
            else if (dt.Rows[gvObli.Rows.Count - 1]["RepaymntCmmnt"].ToString() == "-1")
            {
                gblFuction.AjxMsgPopup("Commnets on Repayment is Blank...");
                return;
            }
            else
            {
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Obligation"] = dt;
                gvObli.DataSource = dt;
                gvObli.DataBind();
                UpFamily.Update();
            }
        }

        protected void gvObli_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Obligation"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Obligation"] = dt;
                    gvObli.DataSource = dt;
                    gvObli.DataBind();
                }
                else
                {
                    gblFuction.AjxMsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }



        private Boolean GetData()
        {
            DataTable dt = (DataTable)ViewState["Obligation"];

            if (dt != null)
            {
                foreach (GridViewRow gr in gvObli.Rows)
                {
                    Label lblSrlNo = (Label)gvObli.Rows[gr.RowIndex].FindControl("lblSrlNo");
                    DropDownList ddlFoir = (DropDownList)gvObli.Rows[gr.RowIndex].FindControl("ddlFoir");
                    TextBox txtLoanType = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtLoanType");
                    TextBox txtBankName = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtBankName");

                    TextBox txtLoanAmount = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtLoanAmount");
                    TextBox txtCurrentPos = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtCurrentPos");

                    TextBox txtEMI = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtEMI");
                    TextBox txtRepaymntCmmnt = (TextBox)gvObli.Rows[gr.RowIndex].FindControl("txtRepaymntCmmnt");

                    if (txtLoanType.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Loan Type is Blank ...");
                        return false;
                    }
                    else if (txtBankName.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Bank Name is Blank...");
                        return false;
                    }
                    else if (txtLoanAmount.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Loan Amount is Blank...");
                        return false;
                    }
                    else if (txtCurrentPos.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Current Pos is Blank...");
                        return false;
                    }
                    else if (txtEMI.Text == "")
                    {
                        gblFuction.AjxMsgPopup("EMI is Blank...");
                        return false;
                    }
                    else if (ddlFoir.SelectedIndex <= 0)
                    {
                        gblFuction.AjxMsgPopup("Foir is Blank...");
                        return false;
                    }
                    else if (txtRepaymntCmmnt.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Commnets on Repayment is Blank...");
                        return false;
                    }
                    else
                    {
                        //Inserting data into datatable dt
                        dt.Rows[gr.RowIndex]["Foir"] = ddlFoir.SelectedValue;
                        dt.Rows[gr.RowIndex]["SrlNo"] = Convert.ToInt32(lblSrlNo.Text == "" ? "0" : lblSrlNo.Text);
                        dt.Rows[gr.RowIndex]["LoanType"] = Convert.ToString(txtLoanType.Text == "" ? "" : txtLoanType.Text);
                        dt.Rows[gr.RowIndex]["BankName"] = Convert.ToString(txtBankName.Text == "" ? "" : txtBankName.Text);
                        dt.Rows[gr.RowIndex]["LoanAmount"] = Convert.ToDouble(txtLoanAmount.Text == "" ? "0" : txtLoanAmount.Text);
                        dt.Rows[gr.RowIndex]["CurrentPos"] = Convert.ToDouble(txtCurrentPos.Text == "" ? "0" : txtCurrentPos.Text);
                        dt.Rows[gr.RowIndex]["EMI"] = Convert.ToDouble(txtEMI.Text == "" ? "0" : txtEMI.Text);
                        dt.Rows[gr.RowIndex]["RepaymntCmmnt"] = Convert.ToString(txtRepaymntCmmnt.Text == "" ? "" : txtRepaymntCmmnt.Text);
                    }


                }
                dt.AcceptChanges();
                ViewState["Obligation"] = dt;
                gvObli.DataSource = dt;
                gvObli.DataBind();

            }
            return true;
        }

        #endregion
        protected void btnVerifyApplEqui_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "A", btnVerifyApplEqui, hdnApplVerifyEquiYN, lblApplVerifyEquiStatus, txtApplCBScore, btnDownloadApplEquifax);
        }
        protected void btnVerifyCA1Equi_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "CA1", btnVerifyCA1Equi, hdnCA1VerifyEquiYN, lblCA1VerifyEquiStatus, txtCA1CBScore, btnDownloadCA1Equifax);
        }
        protected void btnVerifyCA2Equi_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "CA2", btnVerifyCA2Equi, hdnCA2VerifyEquiYN, lblCA2VerifyEquiStatus, txtCA2CBScore, btnDownloadCA2Equifax);
        }
        protected void btnVerifyCA3Equi_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "CA3", btnVerifyCA3Equi, hdnCA3VerifyEquiYN, lblCA3VerifyEquiStatus, txtCA3CBScore, btnDownloadCA3Equifax);
        }
        protected void btnVerifyCA4Equi_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "CA4", btnVerifyCA4Equi, hdnCA4VerifyEquiYN, lblCA4VerifyEquiStatus, txtCA4CBScore, btnDownloadCA4Equifax);
        }
        protected void btnVerifyGuarEqui_Click(object sender, EventArgs e)
        {
            VerifyCIBIL(Convert.ToInt64(hdLeadId.Value), "G", btnVerifyGuarEqui, hdnGuarVerifyEquiYN, lblGuarVerifyEquiStatus, txtGuarCBScore, btnDownloadGuarEquifax);
        }

        protected void btnDownloadApplEquifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "A");
        }
        protected void btnDownloadCA1Equifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "CA1");
        }
        protected void btnDownloadCA2Equifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "CA2");
        }
        protected void btnDownloadCA3Equifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "CA3");
        }
        protected void btnDownloadCA4Equifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "CA4");
        }
        protected void btnDownloadGuarEquifax_Click(object sender, EventArgs e)
        {
            GenerateCbilReport(Convert.ToInt64(hdLeadId.Value), "G");
        }

        public void VerifyEquifax(Int64 pLeadId, string pCustomerType, Button btnVerify, HiddenField hdnVerifyYN, Label lblVerifyStatus, TextBox txtScore, ImageButton btnDownload)
        {
            CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
            CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];
            PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
            PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

            DataTable dt = null;
            CApplication oCAp = null;
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            int vErr = 0;
            string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
            string pErrorMsg = "", pStatusDesc = "";
            int pStatus = 0;
            string pRetailScore = "", pMFIScore = "";
            oCAp = new CApplication();
            CEquiFaxDataSubmission oEqui = new CEquiFaxDataSubmission();

            dt = oCAp.GetMemberInfo(pLeadId, "E", pCustomerType);
            if (dt.Rows.Count > 0)
            {
                string pEqXml = "";
                try
                {
                    //*************************** For Live ***************************************************                      
                    WebServiceSoapClient eq = new WebServiceSoapClient();

                    pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                             , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                            dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                             dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                             "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");

                    //*************************************************************************
                    vErr = oCAp.UpdateEquifaxInformation(pLeadId, pCustomerType, pEqXml, vBranch, Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc, ref pRetailScore, ref pMFIScore);
                    if (vErr == 1)
                    {
                        string[] arr = pStatusDesc.Split(',');
                        string[] arr1 = arr[0].Split('=');
                        string vAcceptYN = arr1[1].ToString();
                        if (vAcceptYN == "Y")
                        {
                            btnVerify.Text = "Verified";
                            btnVerify.Enabled = false;
                            lblVerifyStatus.Text = "Success";
                            hdnVerifyYN.Value = "Y";
                            txtScore.Text = pRetailScore + '/' + pMFIScore;
                        }
                        else
                        {
                            btnVerify.Text = "Cancel";
                            btnVerify.Enabled = false;
                            hdnVerifyYN.Value = "Y";
                            lblVerifyStatus.Text = "Success";
                        }
                        gblFuction.AjxMsgPopup(pStatusDesc);
                        btnDownload.Enabled = true;
                    }
                    if (vErr == 5)
                    {
                        gblFuction.AjxMsgPopup("Data Not Saved, Data Error...");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    pErrorMsg = ex.ToString();
                    gblFuction.AjxMsgPopup(pErrorMsg);
                    hdnVerifyYN.Value = "N";
                    lblVerifyStatus.Text = "Failed";
                    btnDownload.Enabled = false;


                }
            }
        }
        private void SetParameterForRptData(Int64 pLeadId, string pType, string pCustType)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CReport oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReport();
                string enqstatusmsg = "";
                ds = oRpt.CF_Equifax_Report(pLeadId, pCustType, ref  enqstatusmsg);
                if (!String.IsNullOrEmpty(enqstatusmsg))
                {
                    gblFuction.MsgPopup("Equifax Error : " + enqstatusmsg);
                    return;
                }
                else
                {
                    if (ds.Tables.Count == 2 && ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0 && ds.Tables[1].Rows.Count == 0)
                        {
                            gblFuction.AjxMsgPopup("New User");
                            return;
                        }
                    }
                }
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt1.TableName = "CBPortDtl";
                dt2.TableName = "CBPortMst";
                dt3.TableName = "CBHistoryMonth";
                if (pType == "PDF")
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptMemberCredit_New.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(ds);
                        rptDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CreditSummaryReport");
                        Response.ClearHeaders();
                        Response.ClearContent();
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
                dt2 = null;
                oRpt = null;
            }
        }

        #region VerifyCIBIL
        public void VerifyCIBIL(Int64 pLeadId, string pApplicanType, Button btnVerify, HiddenField hdnVerifyYN, Label lblVerifyStatus, TextBox txtScore, ImageButton btnDownload)
        {
            List<CibilResponse> row = new List<CibilResponse>();
            string hostURL = "", Requestdata = "", postURL = "", responsedata = "";
            string SuccessStatus = "", REPORT_ID = "", Score = "", DateProcessed = "", TimeProcessed = "", ScoreDate = "",
                       dateReported = "", vErrDescResponse1 = "", vErrorCode = "", vErrorMsg = "";
            string vData = "";
            Int32 vResponse1 = 0;
            DataTable dt = new DataTable();
            CApplication oRepo = null;
            try
            {
                string requestBody = "{\"pLeadId\":\"" + pLeadId.ToString() + "\",\"pApplicanType\":\"" + pApplicanType + "\"}";
                postURL = MobUrl + "VerifyCIBIL";
                HttpWebRequest request = WebRequest.Create(postURL) as HttpWebRequest;
                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Close();
                }
                StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream());
                responsedata = responseReader.ReadToEnd();
                responsedata = responsedata.Replace("\u0000", "");
                responsedata = responsedata.Replace("\\u0000", "");
              
                responsedata = responsedata.Replace("\"", "");

                responsedata = responsedata.Replace("\\", "\"");
                responsedata = responsedata.Replace("\"/", "/");
                string vRequestdataXml = AsString(JsonConvert.DeserializeXmlNode(requestBody, "root"));
                string vResponseXml = AsString(JsonConvert.DeserializeXmlNode(responsedata, "root"));

                vResponse1 = oRepo.CF_SaveTransUnionCBILResponse(pLeadId, SuccessStatus, REPORT_ID, Score, vRequestdataXml, vResponseXml, Convert.ToInt32(Session[gblValue.UserId]), pApplicanType, ref vErrDescResponse1);

                dynamic vJsonObject = JsonConvert.DeserializeObject(responsedata);
                string vErrMsg = Convert.ToString(vJsonObject.VerifyCIBILResult.controlData);
                if (vErrMsg != "No Data Found" || vErrMsg != "No Response")
                {
                        SuccessStatus = vJsonObject.VerifyCIBILResult.controlData.success;                      
                        if (SuccessStatus == "True")
                        {

                            REPORT_ID = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["tuefHeader"].enquiryControlNumber;
                            Score = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["scores"][0]["score"];
                            DateProcessed = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["tuefHeader"].dateProcessed;
                            TimeProcessed = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["tuefHeader"].timeProcessed;
                            ScoreDate = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["scores"][0]["scoreDate"];
                            dateReported = vJsonObject.VerifyCIBILResult.consumerCreditData[0]["addresses"][0]["dateReported"];


                            oRepo = new CApplication();
                            vResponse1 = oRepo.CF_SaveTransUnionCBILResponse(pLeadId, SuccessStatus, REPORT_ID, Score, vRequestdataXml, vResponseXml, Convert.ToInt32(Session[gblValue.UserId]), pApplicanType, ref vErrDescResponse1);
                            if (vResponse1 > 0)
                            {
                                hdnVerifyYN.Value = "N";
                                lblVerifyStatus.Text = "Failed";
                                btnDownload.Enabled = false;

                                vData = vErrDescResponse1;
                                gblFuction.AjxMsgPopup(vData + ". CIBIL Data Not Saved Data Error...");
                            }
                            else
                            {
                                vData = "CBIL Verified Successfully";

                                btnVerify.Text = "Verified";
                                btnVerify.Enabled = false;
                                lblVerifyStatus.Text = "Success";
                                hdnVerifyYN.Value = "Y";
                                txtScore.Text = Score + '/' + Score;
                                btnDownload.Enabled = true;
                                gblFuction.AjxMsgPopup(vData);
                            }
                        }
                        else
                        {
                            oRepo = new CApplication();                           
                            vErrorCode = vJsonObject.VerifyCIBILResult.controlData.errorResponseArray[0].errorCode;
                            vErrorMsg = vJsonObject.VerifyCIBILResult.controlData.errorResponseArray[0].errorMessage;
                            vData = vErrorMsg;
                            vResponse1 = oRepo.CF_SaveTransUnionCBILResponse(pLeadId, SuccessStatus, REPORT_ID, Score, vRequestdataXml, vResponseXml, Convert.ToInt32(Session[gblValue.UserId]), pApplicanType, ref vErrDescResponse1);

                            btnVerify.Text = "Verify";
                            btnVerify.Enabled = true;
                            hdnVerifyYN.Value = "N";
                            lblVerifyStatus.Text = "Failed";

                            btnDownload.Enabled = false;
                            gblFuction.AjxMsgPopup(vData);
                        }
                   
                }
                else
                {
                    gblFuction.AjxMsgPopup(vErrMsg);
                }
            }
            catch (Exception ex)
            {
                hdnVerifyYN.Value = "N";
                lblVerifyStatus.Text = "Failed";
                btnDownload.Enabled = false;

                vData = ex.Message;
                gblFuction.AjxMsgPopup(vData);
            }
        }

        public void GenerateCbilReport(Int64 pLeadId, string pCustType)
        {
            string vReportID = "";
            string vFileName = "", OutputPath = "";

           
            CApplication Obj = new CApplication();
            DataTable dt = new DataTable();
            dt = Obj.CF_GetCIBILResponseByReportOrderNo(pLeadId, pCustType);
            if (dt.Rows.Count > 0)
            {
                vReportID = dt.Rows[0]["CBReportOrderNo"].ToString();

                if (vReportID == "")
                {
                    gblFuction.MsgPopup("Please make sure that the CIBIL already done");
                    return;
                }
                else
                {
                    vFileName = "CentrumCF" + '_' + vReportID + ".html";
                    OutputPath = "C:\\CIBILDocuments" + "\\" + vFileName;
                    XmlDocument xmlDoc = new XmlDocument();
                    XslCompiledTransform xslt = null;
                    xslt = new XslCompiledTransform();
                  
                    string JsonResponse = dt.Rows[0]["CBIL_Response"].ToString();
                    xmlDoc.LoadXml("<Root>" + JsonResponse + "</Root>");

                    XsltSettings xsltSettings = new XsltSettings();
                    xsltSettings.EnableScript = true;                   
                    string xsltPath = HostingEnvironment.MapPath("~/XSLT_Json/java_cir_JSONConverter_v3.xslt");

                    xslt.Load(XmlReader.Create(new StringReader(File.ReadAllText(xsltPath))), xsltSettings, new XmlUrlResolver());

                    string resultHtml = string.Empty;
                    using (XmlReader xmlReader = new XmlTextReader(new StringReader(xmlDoc.DocumentElement.OuterXml)))
                    {
                        using (var sw = new StringWriter())
                        {
                            using (XmlWriter xw = XmlWriter.Create(sw))
                            {
                                xslt.Transform(xmlReader, null, xw);
                                xw.Close();
                            }
                            resultHtml = sw.ToString();
                            //<?xml version="1.0" encoding="utf-16"?><html xmlns="http://www.w3.org/1999/xhtml" xmlns:usr="urn:the-xml-files:xslt"><head><META http-equiv="Content-Type" content="text/html; charset=utf-8" /><title>CIBIL: CONSUMER INFORMATION REPORT</title><style type="text/css">/* CSS Document */body.MainBody {margin:0px; padding:0px; text-align:center;}.maincontainer {margin:0px auto;width:827px; border:0px solid #c9c9c9; color:#000000; font-family:Arial, Helvetica, sans-serif; font-size: 11px;}/* Headaer */.headerlogo1{width:300px;height:65px;background:url(data:image/png;base64,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);background-repeat:no-repeat;margin-bottom: 30px;margin-left: 17px;}}#ccir{margin-top:10px;}.details{width:256px;height:103px;background:url(../images/Details.jpg);}.headerlogo2{background:url(../images/logo2.jpg);width:435px;height:80px;position:absolute;float:left;}.summary{color:#ffffff;font-family:font-family:Arial, Helvetica;font-size:10px;padding-left:54px;}.constitle5{background-color:#CEE0E8;font-family:Arial, Helvetica, sans-serif;font-size:12px;font-weight:bold;color:#305665;padding-left: 300px;}.addressinfo{   font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9; font-weight:bold; padding-left:20px;padding-bottom:2px;}.timezone{width:150px;height:20px;margin-top:14px;margin-left:18px;}.head1 { font-family:"Times New Roman", Times, serif; font-size:16px; color:#34748e; text-transform:uppercase;font-weight:bold; padding-left: 7px; width:610px;}.headtitle1 { font-family:"Times New Roman", Times, serif; font-size:17px; color:#34748e; text-transform:uppercase;font-weight:bold; padding-left: 7px; width:610px;}.headtitle2 { font-family:Arial, Helvetica, sans-serif;font-size:13px; color:#656565;text-transform:uppercase;}.headtitle3 { font-family:Arial, Helvetica, sans-serif;font-size:15px; color:#34748e;text-transform:uppercase; padding-top: 15px; font-weight:normal;}.headtitle4 { font-family:Arial, Helvetica, sans-serif;font-size:16px; color:#34748e;text-transform:uppercase; padding-top: 7px; font-weight:Bold;}.headlabel {font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9; font-weight:normal; padding-left:10px;padding-bottom:2px;}.headlabe2 {font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9; font-weight:normal; padding-left:30px;padding-bottom:2px;}.headvalue {font-family:Arial, Helvetica, sans-serif;text-transform:uppercase;color:#000000;padding-bottom:2px;font-size:12px;}.headborder { border-bottom:3px solid #34748e; padding-top:10px;}.headborder2 { border-bottom:1px solid #818284; padding-top:5px;}/* Consumer Details */.constitle { background-color:#00a6ca;font-family:Arial, Helvetica, sans-serif;font-size:14px; font-weight:bold; color:#ffffff;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1 { background-color:#CEE0E8;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1_shay{ background-color:#6fa7bc;font-family:Arial, Helvetica, sans-serif;font-size:13px;  color:#ffffff;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1_cen { background-color:#CEE0E8;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; text-align:center;color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle4_cen { background-color:#CEE0E8;font-family:Arial, Helvetica, sans-serif;font-size:12px; text-align:center;color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1_cen_shay_o{ background-color:#DDD;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; text-align:center;color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1_cen_shay_yi{ background-color:#FFF;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; text-align:center;color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle1_gry { background-color:#F0F0F0;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; color:#305665;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.consubtitle { font-family:Arial, Helvetica, sans-serif;font-size:10px; color:#005586;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.constitle2 { background-color:#FFF;font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#4f90a9;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle3 { background-color:#FFF;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; color:#346070; padding-left: 3px; padding-top: 5px;padding-bottom: 5px; padding-left:10px; text-transform:uppercase}.constitle3_Cen { background-color:#FFF;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; text-align:center; color:#4f90a9;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle3_Left { background-color:#FFF; border:0px solid #4f90a9; font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; text-align:left; color:#4f90a9;padding-left: 3px; padding-top: 5px;padding-bottom: 0px;}.constitle1_gry { background-color:#E0E0E0;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; color:#000000;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}.constitle4 { background-color:#4f90a9;font-family:Arial, Helvetica, sans-serif;font-size:14px; color:#ffffff;padding-left: 3px; padding-top: 5px;padding-bottom: 5px;}/*.padWhole {padding:5px 15px 10px 20px;}.padWhole2 {padding:15px 15px 0px 20px;}.padWhole3{padding:0px 15px 0px 20px;}.padWhole4 {padding:0px 15px 0px 20px;}.padWhole5{padding:1px 15px 0px 20px;}*/.conLabel{font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:normal;padding-bottom:5px;padding-top:5px;}.conValue{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal;padding-bottom:5px;padding-top:5px;}.conValueAddPad{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal;padding:3px;}.conValueAddPad_Cen{background-color:#FFF;font-size:11px;font-family:Arial, Helvetica, sans-serif;color:#000000; text-align:center;font-weight:normal;padding:5px;}.conValueAddPad_left_shay{background-color:#FFF;font-size:11px;font-family:Arial, Helvetica, sans-serif;color:#000000; text-align:left;font-weight:normal;padding:5px;}.conValueAddPad_AlignC{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal; text-align:center; padding:5px;}.conValueAddPad_AlignC_bld{background-color:#FFF;font-size:14px;font-family:Arial, Helvetica, sans-serif;color:#000000; font-weight:bold; text-align:center; padding:5px;}.conValueAddPad_Cen_shay_yi{background-color:#FFF;font-size:11px;font-family:Arial, Helvetica, sans-serif;color:#000000; text-align:left;font-weight:normal;padding:5px;}.conValueAddPad_Cen_shay_o{background-color:#DDD;font-size:11px;font-family:Arial, Helvetica, sans-serif;color:#000000; text-align:left;font-weight:normal;padding:5px;}.padLeft {padding-left:5px;}.conValuepadLeft{padding-left:5px;background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal;padding-bottom:5px;padding-top:5px;}.conValueBoldAddPad {background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:bold;padding:5px;}.conValueSpec{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:normal;padding-bottom:5px;padding-top:5px;}.conValueSpecAddPad{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:normal;padding:5px;}.conValueSpecAddPad_1{background-color:#CCC;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#305665;font-weight:bold; text-align:center;padding:5px;}.conValueSpecAddPad_cen{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;text-align:center;color:#6D7B8D;font-weight:normal;padding:5px;}.conValueSpecAddPad_shay{background-color:#FFF;font-size:11px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:normal;padding:2px;}.conValueSpecAddPad_shay_2{background-color:#4f90a9;font-size:10px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:normal;padding:2px;}.conValueSpecAddPad_shay1{background-color:#DDD;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#305665;font-weight:bold; text-align:center;padding:5px;}.conValueSpecAddPad_shay3{background-color:#FFF;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#4f90a9;font-weight:bold;padding:2px;}.constitleblue { background-color:#4f90a9;font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#FFF; padding-bottom:3px;}.constitleblueAddPad { background-color:#4f90a9;font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#FFF; padding:5px;}.constitleblueAddPad_Cen { background-color:#4f90a9;font-family:Arial, Helvetica, sans-serif;font-size:12px; text-align:center; color:#FFF; padding:5px;}.constitleblueAddPad2 { background-color:#ffffff;font-family:Arial, Helvetica, sans-serif;font-size:12px; font-weight:bold; color:#457E94; padding:5px; padding-left:10px}.Scorevalue {font-family:Arial, Helvetica, sans-serif;font-size:9px; color:#1a1a1a;text-transform:uppercase;padding-left:5px;border-bottom:1px solid #85afd9;}.Score {font-family:Arial, Helvetica, sans-serif;color:#003365; font-size:32px;border-bottom:1px solid #85afd9;}ol{ padding-left:15px}.AddPad { padding:5px;}.ThreeColtable { background-color:#fff;font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#FFF; padding:1px;}.ThreeColtable_left { background-color:#ffffff;font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#FFF; border-right:1px solid #4f90a9;}.Matrixtable_Subtitle {background-color:#E0E0E0;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:bold;padding:3px;}.Matrixtable_text {background-color:#E0E0E0;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal;padding:3px;}.Matrixtable_text_bld {background-color:#F0F0F0;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#000000; font-weight:bold; padding:3px;}.Matrixtable_text_Blu_bld {background-color:#fff;font-size:12px;font-family:Arial, Helvetica, sans-serif;color:#305867; text-align:center; font-weight:bold; padding:3px;}.Matrixtable_text_Cen { background-color:#fff;font-family:Arial, Helvetica, sans-serif;font-size:12px; text-align:center; color:#000000; padding:5px;}.specBorder{border:1px solid #aaaaaa; padding:5px;}.specconstitle{font-family:Arial, Helvetica, sans-serif;color:#656565;text-transform:uppercase; font-weight:bold; font-size:9px; padding-bottom:3px;}.specconValue{font-family:Arial, Helvetica, sans-serif;color:#1c1c1c;text-transform:uppercase; font-weight:bold;font-size:9px;padding-bottom:3px;}.borderBot{border-bottom:1px solid #CCCCCC;}.daysTitle{font-family:Arial, Helvetica, sans-serif;color:#000;text-transform:uppercase; font-weight:normal;font-size:9px;padding-bottom:3px;}.daysValue{font-family:Arial, Helvetica, sans-serif;color:#000;text-transform:uppercase; font-weight:normal;font-size:9px;padding-bottom:3px;}.endStatement{font-family:Arial, Helvetica, sans-serif;color:#000000;font-weight:normal;font-size:9px;padding-bottom:5px; text-align:justify;}.specBg{background-color:#f3f3f3;}.BGcolor0{ background-color:#4f90A9;}/*.BGcolor0{ background-color:#0070b0;}*/.BGcolor1{ background-color:#A0C5D3;}.BGcolor2{ background-color:#F7F7F7;}.BGcolor3{ background-color:#FFFFFF;}.BGcolor4{ background-color:#DFDFDF;}/*.BGcolor1{ background-color:#31697e;}*//* By Nataraj */.TUCibilheaderlogo1{width:300px;height:65px;background:url(data:image/png;base64,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);background-repeat:no-repeat;margin-bottom: 10px;margin-top: 20px;}.constitleGreyBig { font-family:Arial, Helvetica, sans-serif;font-size:14px; color:#656565;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.constitleRedBig { font-family:Arial, Helvetica, sans-serif;font-size:14px; color:Red;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.conslabelBlueBig { font-family:Arial, Helvetica, sans-serif;font-size:11px; color:#00678e;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;padding-left:5px}.conslabelBlackBig { font-family:Arial, Helvetica, sans-serif;font-size:11px; color:Black;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.conslabelScoreBig { font-family:Arial, Helvetica, sans-serif;font-size:28px; color:#00678e;text-transform:uppercase; padding-bottom: 10px;}.yellowheader {background-color:#fad700;font-weight: 500;font-size: large;}.blueborder { border-bottom:2.5px solid #00a7d4; padding-bottom:5px;}.greyborder25 { border-bottom:2.5px solid #878787; padding-bottom:5px;}.greyborder { border-bottom:0.5px solid #878787; padding-bottom:5px;}.greybordernopadding { border-bottom:1.5px solid #878787;}.greyallborder15 { border:1.5px solid #878787; padding-bottom:5px;}.padAll5 {padding:5px;}.padAll2 {padding:2px;}.BlueLabel{ color:#00a7d4;font-size: 10px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; font-stretch: ultra-expanded;}.BlueLabelBigger1{ color:#00a7d4;font-size: 11.5px; font-weight: 700; font-family: Arial, Helvetica, sans-serif; font-stretch: ultra-expanded;}.BlueLabelBigger2{ font-family:Arial, Helvetica, sans-serif;font-size:11px; color:00a7d4;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.BlueLabelBigger3 { font-family:Arial, Helvetica, sans-serif;font-size:12px; color:#00a7d4;text-transform:uppercase; font-weight:bold; padding-bottom: 10px;}.RedLabel{ color:#d84042;font-size: 10px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; font-stretch: ultra-expanded;}.BlackLabel{ color:Black;font-size: 11px; font-weight: bold; font-family: Arial, Helvetica, sans-serif; font-stretch: ultra-expanded;}.GreyItalicLabel{ color:Grey;font-size: 11px;font-style: italic; font-family: Arial, Helvetica, sans-serif; font-stretch: ultra-expanded;}.alternatetdbggrey {background-color:#f0f0f0;}.alternatetdbgwhite {background-color:#ffffff;}.PhDiv50Width{padding:10px;display: inline-block!important;width:35px}.textalignend {text-align:end}.backclr:nth-of-type(2n) { background-color: #f0f0f0;}</style></head><body class="MainBody"><table cellpadding="0" cellspacing="0" border="0" class="maincontainer"><tr><td width="300px"><div class="TUCibilheaderlogo1" /></td></tr><tr><td><table width="1000" border="0" align="right" class="blueborder" cellpadding="0" cellspacing="0"><tr class="yellowheader"><td width="300px" class="padAll5">CONSUMER CIR</td><td width="330px" /><td width="370px" /></tr><tr><td width="300px"><table><tr><td class="BlueLabel" align="left" colspan="2">CONSUMER:</td><td class="BlackLabel" align="left" colspan="2">SANDEEP SURESH KUMAVAT NA</td></tr><tr><td class="BlueLabel" align="left" colspan="2">MEMBER ID:</td><td class="BlackLabel" align="left" colspan="2">NB43099999_UATC2CNPE</td></tr><tr><td class="BlueLabel" align="left" colspan="2">MEMBER REFERENCE NUMBER:</td><td class="BlackLabel" align="left" colspan="2">NB4678</td></tr></table></td><td width="330px" /><td width="370px"><table><tr><td class="BlueLabel" align="left" colspan="2">DATE:</td><td class="BlackLabel" align="left" colspan="2">06-10-2023</td></tr><tr><td class="BlueLabel" align="left" colspan="2">TIME:</td><td class="BlackLabel" align="left" colspan="2">10:45:27</td></tr><tr><td class="BlueLabel" align="left" colspan="2">CONTROL NUMBER:</td><td class="BlackLabel" align="left" colspan="2">002159343659</td></tr></table></td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greyborder" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">CONSUMER INFORMATION:</td><td width="330px" /><td width="370px" /></tr><tr><td width="600px"><table><tr><td class="GreyItalicLabel" align="left" colspan="2">NAME:</td><td class="BlackLabel" align="left" colspan="2">SANDEEP SURESH KUMAVAT NA</td></tr><tr><td class="GreyItalicLabel" align="left" colspan="2">DATE OF BIRTH:</td><td class="BlackLabel" align="left" colspan="2">18-07-1980</td></tr></table></td><td width="300px"><table><tr><td height="14px" bgcolor="#FFFFFF" /></tr><tr><td class="GreyItalicLabel" align="left" colspan="2">GENDER:</td><td class="BlackLabel" align="left" colspan="2">MALE</td></tr></table></td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">CIBIL TRANSUNION SCORE(S):</td><td width="330px" /><td width="370px" /></tr><tr><td width="150px" class="BlueLabel">SCORE NAME</td><td width="150px" class="BlueLabel">SCORE</td><td width="700px" class="BlueLabel">SCORING FACTORS</td></tr></table></td></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr style="height: 50px;background-color: #ededed!important;"><td width="250px" class="conslabelBlueBig">CREDITVISION® SCORE</td><td width="250px" class="conslabelScoreBig">00656</td><td width="500px" class="conslabelBlackBig padAll5"><ol class="padAll2"><li class="padAll2" xmlns=""> PRESENCE OF DELINQUENCY AS OF RECENT UPDATE</li><li class="padAll2" xmlns=""> HIGH BALANCE BUILD-UP</li><li class="padAll2" xmlns=""> PRESENCE OF SEVERE DELINQUENCY AS OF RECENT UPDATE</li></ol></td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" style="background-color: #ededed!important;padding: 5px;" border="0" align="right" cellpadding="0" cellspacing="0"><tr><td width="800px" class="conslabelBlueBig">POSSIBLE RANGE FORCREDITVISION® SCORE</td><td width="200px" /></tr><tr><td width="600px" class="BlackLabel">Consumer with at least one trade on the bureau in last 36 months</td><td width="400px" class="RedLabel">: 300 (high risk) to 900 (low risk)</td></tr><tr><td width="600px" class="BlackLabel">Consumer not in CIBIL database or history older than 36 months</td><td width="400px" class="RedLabel">: -1</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" class="BlackLabel">* At least one tradeline with information updated in last 36 months is required.</td></tr></table></td></tr><tr><td colspan="4" height="8px" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">IDENTIFICATION(S):</td><td width="330px" /><td width="370px" /></tr><tr><td width="250px" class="BlueLabel">IDENTIFICATION TYPE</td><td width="250px" class="BlueLabel">IDENTIFICATION NUMBER</td><td width="250px" class="BlueLabel">ISSUE DATE</td><td width="250px" class="BlueLabel">EXPIRATION DATE</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">INCOME TAX ID NUMBER (PAN)</td><td width="250px" class="BlackLabel padAll5">ATUPK6975D</td><td width="250px" class="BlackLabel padAll5" /><td width="250px" class="BlackLabel padAll5" /></tr><tr><td width="250px" class="BlackLabel padAll5">UNIVERSAL ID NUMBER (UID)</td><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5" /><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">TELEPHONE(S):</td><td width="330px" /><td width="370px" /></tr><tr><td width="300px" class="BlueLabel">TELEPHONE TYPE</td><td width="330px" class="BlueLabel">TELEPHONE NUMBER</td><td width="370px" class="BlueLabel">TELEPHONE EXTENSION</td></tr><tr class="alternatetdbggrey"><td width="300px" class="BlackLabel padAll5">OFFICE PHONE</td><td width="330px" class="BlackLabel padAll5">8087045035</td><td width="370px" class="BlackLabel padAll5"></td></tr><tr><td width="300px" class="BlackLabel padAll5">NOT CLASSIFIED<sup>(e)</sup></td><td width="330px" class="BlackLabel padAll5">8087045035</td><td width="370px" class="BlackLabel padAll5"></td></tr><tr class="alternatetdbggrey"><td width="300px" class="BlackLabel padAll5">NOT CLASSIFIED<sup>(e)</sup></td><td width="330px" class="BlackLabel padAll5">4444499619</td><td width="370px" class="BlackLabel padAll5"></td></tr><tr><td width="300px" class="BlackLabel padAll5">NOT CLASSIFIED<sup>(e)</sup></td><td width="330px" class="BlackLabel padAll5">450358087</td><td width="370px" class="BlackLabel padAll5"></td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">EMAIL CONTACT(S):</td><td width="330px" /><td width="370px" /></tr><tr><td width="300px" class="BlueLabel">EMAIL ADDRESS</td><td width="330px" class="BlueLabel" /><td width="370px" class="BlueLabel" /></tr><tr class="alternatetdbggrey"><td width="300px" class="BlackLabel padAll5">SANDEEPKUMAVAT75@GMAIL.COM</td><td width="330px" class="BlackLabel padAll5" /><td width="370px" class="BlackLabel padAll5" /></tr><tr><td width="300px" class="BlackLabel padAll5">KUMAVAT31@GMAIL.COM</td><td width="330px" class="BlackLabel padAll5" /><td width="370px" class="BlackLabel padAll5" /></tr><tr class="alternatetdbggrey"><td width="300px" class="BlackLabel padAll5">NOMAIL@NOMAIL.COM</td><td width="330px" class="BlackLabel padAll5" /><td width="370px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="300px" class="constitleGreyBig">ADDRESS(ES):</td><td width="330px" /><td width="370px" /></tr><tr class="alternatetdbggrey"><td width="975px" colspan="3" class="BlueLabelBigger2 padAll5">ADDRESS<sup style="text-transform:lowercase;">(e)</sup>:<span class="conslabelBlackBig padAll5">MUMBAI, MUMBAI,MAHARASHTRA,400010</span></td></tr><tr class="alternatetdbggrey"><td width="300px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">CATEGORY:</span>Residence Address</td><td width="330px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">RESIDENCE CODE:</span>NA</td><td width="370px"><span class="BlueLabelBigger2">DATE REPORTED:</span><span class="conslabelBlackBig padAll5">28-07-2023</span></td></tr><tr><td width="975px" colspan="3" class="BlueLabelBigger2 padAll5">ADDRESS<sup style="text-transform:lowercase;">(e)</sup>:<span class="conslabelBlackBig padAll5">SDAS, SAD MUMBAI,MAHARASHTRA,400010</span></td></tr><tr><td width="300px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">CATEGORY:</span>Residence Address</td><td width="330px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">RESIDENCE CODE:</span>NA</td><td width="370px"><span class="BlueLabelBigger2">DATE REPORTED:</span><span class="conslabelBlackBig padAll5">27-07-2023</span></td></tr><tr class="alternatetdbggrey"><td width="975px" colspan="3" class="BlueLabelBigger2 padAll5">ADDRESS<sup style="text-transform:lowercase;">(e)</sup>:<span class="conslabelBlackBig padAll5">G CORP THANE,THANE,MAHARASHTRA,400608</span></td></tr><tr class="alternatetdbggrey"><td width="300px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">CATEGORY:</span>Residence Address</td><td width="330px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">RESIDENCE CODE:</span>Rented</td><td width="370px"><span class="BlueLabelBigger2">DATE REPORTED:</span><span class="conslabelBlackBig padAll5">08-06-2023</span></td></tr><tr><td width="975px" colspan="3" class="BlueLabelBigger2 padAll5">ADDRESS<sup style="text-transform:lowercase;">(e)</sup>:<span class="conslabelBlackBig padAll5">R MALL PARK SITE,THANE,MAHARASHTRA,400615</span></td></tr><tr><td width="300px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">CATEGORY:</span>Residence Address</td><td width="330px" class="conslabelBlackBig padAll5"><span class="BlueLabelBigger2">RESIDENCE CODE:</span>Rented</td><td width="370px"><span class="BlueLabelBigger2">DATE REPORTED:</span><span class="conslabelBlackBig padAll5">08-05-2023</span></td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="330px" class="constitleGreyBig">EMPLOYMENT INFORMATION:</td><td width="300px" /><td width="370px" /></tr><tr><td width="165px" class="BlueLabel">ACCOUNT TYPE</td><td width="165px" class="BlueLabel">DATE REPORTED</td><td width="165px" class="BlueLabel">OCCUPATION CODE</td><td width="165px" class="BlueLabel">INCOME</td><td width="165px" class="BlueLabel">NET / GROSS INCOME INDICATOR</td><td width="175px" class="BlueLabel">MONTHLY / ANNUAL INCOME INDICATOR</td></tr><tr class="alternatetdbggrey"><td width="165px" class="BlackLabel padAll5">BUSINESS LOAN – GENERAL</td><td width="165px" class="BlackLabel padAll5">28-02-2022</td><td width="165px" class="BlackLabel padAll5">Others</td><td width="165px" class="BlackLabel padAll5">Not Available</td><td width="165px" class="BlackLabel padAll5">Not Available</td><td width="175px" class="BlackLabel padAll5">Not Available</td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="330px" class="constitleGreyBig">SUMMARY:</td><td width="300px" /><td width="370px" /></tr><tr><td width="330px" class="BlueLabelBigger3">ACCOUNT(S)</td><td width="300px" /><td width="370px" /></tr><tr><td width="200px" class="BlueLabel">ACCOUNT TYPE</td><td width="200px" class="BlueLabel">ACCOUNTS</td><td width="200px" class="BlueLabel">ADVANCES</td><td width="200px" class="BlueLabel">BALANCES</td><td width="200px" class="BlueLabel" colspan="2">DATE OPENED</td></tr><tr class="alternatetdbggrey"><td width="200px" class="BlackLabel padAll5">All Accounts</td><td width="200px" class="BlackLabel padAll5" style="border-bottom:1.2px solid #cecece"><span class="GreyItalicLabel">TOTAL:</span>49</td><td width="200px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CR/SANC. AMT:</span>38,48,385</td><td width="200px" class="BlackLabel padAll5" style="border-bottom:1.2px solid #cecece"><span class="GreyItalicLabel">CURRENT:</span>6,41,673</td><td width="200px" colspan="2" class="BlackLabel padAll5" style="border-bottom:1.2px solid #cecece"><span class="GreyItalicLabel">RECENT:</span>06-05-2022</td></tr><tr class="alternatetdbggrey"><td width="200px" class="BlackLabel padAll5" /><td width="200px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OVERDUE:</span>1</td><td width="200px" class="BlackLabel padAll5" /><td width="200px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OVERDUE:</span>14,574</td><td width="200px" colspan="2" class="BlackLabel padAll5"><span class="GreyItalicLabel">OLDEST:</span>12-04-2007</td></tr><tr class="alternatetdbggrey"><td width="200px" class="BlackLabel padAll5" /><td width="200px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ZERO-BALANCE:</span>44</td><td width="200px" class="BlackLabel padAll5" /><td width="200px" class="BlackLabel padAll5" /><td width="200px" colspan="2" class="BlackLabel padAll5" /></tr><tr><td width="330px" class="BlueLabelBigger3">ENQUIRIES</td><td width="300px" /><td width="370px" /></tr><tr><td width="165px" class="BlueLabel">ENQUIRY PURPOSE</td><td width="165px" class="BlueLabel">TOTAL</td><td width="165px" class="BlueLabel">PAST 30 DAYS</td><td width="165px" class="BlueLabel">PAST 12 MONTHS</td><td width="165px" class="BlueLabel">PAST 24 MONTHS</td><td width="165px" class="BlueLabel">RECENT</td></tr><tr class="alternatetdbggrey"><td width="165px" class="BlackLabel padAll5">All Enquiries</td><td width="165px" class="BlackLabel padAll5">124</td><td width="165px" class="BlackLabel padAll5">12</td><td width="165px" class="BlackLabel padAll5">52</td><td width="165px" class="BlackLabel padAll5">27</td><td width="165px" class="BlackLabel padAll5">05-10-2023</td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="330px" class="constitleGreyBig">ACCOUNT(S):</td><td width="300px" /><td width="370px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>19-11-2008</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>12-10-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>19-10-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-08-2012</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-10-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-11-2008</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>26,010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr /><tr /><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">CREDIT FACILITY STATUS:</div> SETTLED</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">131<br />10-10</div><div class="PhDiv50Width" xmlns="">XXX<br />09-10</div><div class="PhDiv50Width" xmlns="">072<br />08-10</div><div class="PhDiv50Width" xmlns="">042<br />07-10</div><div class="PhDiv50Width" xmlns="">XXX<br />06-10</div><div class="PhDiv50Width" xmlns="">013<br />05-10</div><div class="PhDiv50Width" xmlns="">012<br />04-10</div><div class="PhDiv50Width" xmlns="">000<br />03-10</div><div class="PhDiv50Width" xmlns="">XXX<br />02-10</div><div class="PhDiv50Width" xmlns="">000<br />01-10</div><div class="PhDiv50Width" xmlns="">000<br />12-09</div><div class="PhDiv50Width" xmlns="">000<br />11-09</div><div class="PhDiv50Width" xmlns="">000<br />10-09</div><div class="PhDiv50Width" xmlns="">030<br />09-09</div><div class="PhDiv50Width" xmlns="">030<br />08-09</div><div class="PhDiv50Width" xmlns="">000<br />07-09</div><div class="PhDiv50Width" xmlns="">000<br />06-09</div><div class="PhDiv50Width" xmlns="">000<br />05-09</div><div class="PhDiv50Width" xmlns="">000<br />04-09</div><div class="PhDiv50Width" xmlns="">000<br />03-09</div><div class="PhDiv50Width" xmlns="">000<br />02-09</div><div class="PhDiv50Width" xmlns="">000<br />01-09</div><div class="PhDiv50Width" xmlns="">000<br />12-08</div><div class="PhDiv50Width" xmlns="">000<br />11-08</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>04-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>17-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-12-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>4,19,280</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OVERDUE:</span>14,574</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">056<br />06-22</div><div class="PhDiv50Width" xmlns="">058<br />05-22</div><div class="PhDiv50Width" xmlns="">024<br />04-22</div><div class="PhDiv50Width" xmlns="">STD<br />03-22</div><div class="PhDiv50Width" xmlns="">151<br />02-22</div><div class="PhDiv50Width" xmlns="">123<br />01-22</div><div class="PhDiv50Width" xmlns="">082<br />12-21</div><div class="PhDiv50Width" xmlns="">051<br />11-21</div><div class="PhDiv50Width" xmlns="">021<br />10-21</div><div class="PhDiv50Width" xmlns="">STD<br />09-21</div><div class="PhDiv50Width" xmlns="">STD<br />08-21</div><div class="PhDiv50Width" xmlns="">082<br />07-21</div><div class="PhDiv50Width" xmlns="">051<br />06-21</div><div class="PhDiv50Width" xmlns="">003<br />05-21</div><div class="PhDiv50Width" xmlns="">STD<br />04-21</div><div class="PhDiv50Width" xmlns="">STD<br />03-21</div><div class="PhDiv50Width" xmlns="">003<br />02-21</div><div class="PhDiv50Width" xmlns="">003<br />01-21</div><div class="PhDiv50Width" xmlns="">STD<br />12-20</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>14-08-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>08-08-2008</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>11-10-2008</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-05-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-10-2008</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>52,851</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr /><tr /><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">030<br />10-08</div><div class="PhDiv50Width" xmlns="">030<br />09-08</div><div class="PhDiv50Width" xmlns="">000<br />08-08</div><div class="PhDiv50Width" xmlns="">000<br />07-08</div><div class="PhDiv50Width" xmlns="">000<br />06-08</div><div class="PhDiv50Width" xmlns="">000<br />05-08</div><div class="PhDiv50Width" xmlns="">030<br />04-08</div><div class="PhDiv50Width" xmlns="">000<br />03-08</div><div class="PhDiv50Width" xmlns="">000<br />02-08</div><div class="PhDiv50Width" xmlns="">000<br />01-08</div><div class="PhDiv50Width" xmlns="">000<br />12-07</div><div class="PhDiv50Width" xmlns="">000<br />11-07</div><div class="PhDiv50Width" xmlns="">000<br />10-07</div><div class="PhDiv50Width" xmlns="">000<br />09-07</div><div class="PhDiv50Width" xmlns="">000<br />08-07</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>31950</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>06-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>31,950</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>31,950</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>24</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />06-22</div><div class="PhDiv50Width" xmlns="">000<br />05-22</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>14150</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>03-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>14,150</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>14,150</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>24</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />06-22</div><div class="PhDiv50Width" xmlns="">000<br />05-22</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>13300</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>21-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>03-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>03-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>13,100</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>24</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>13,100</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />05-22</div><div class="PhDiv50Width" xmlns="">000<br />04-22</div><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>29800</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>08-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>06-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>06-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>29,800</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>29,800</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />05-22</div><div class="PhDiv50Width" xmlns="">000<br />04-22</div><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>29750</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>07-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>08-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>08-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-12-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>29,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>29,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>TWO-WHEELER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>45000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>PROPERTY</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>30-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>16-04-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-04-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-04-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>40,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>26,917</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>2,200</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>24</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>16</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>22,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />04-22</div><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>27-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>01-05-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>01-05-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-09-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-09-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>1,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>1,064</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>1</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>BUSINESS LOAN – GENERAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>26-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>26-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>28-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>1</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>16-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>26-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-05-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>2,800</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>24</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>3,041</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />05-22</div><div class="PhDiv50Width" xmlns="">000<br />04-22</div><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>13550</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>11-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>21-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>21-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>28-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>13,550</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>13,550</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>10850</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>06-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>11-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>11-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>11-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,050</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>10,050</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>05-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>03-03-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-03-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-03-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>40,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>98850</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>02-01-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>06-01-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>06-01-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-01-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-01-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-01-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>98,550</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>98,550</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />01-22</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>67550</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>19-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>19-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>19-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>19-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>40,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>40,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>08-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>02-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>07-08-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>10-08-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-08-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>44,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>3,663</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>3,663</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div><div class="PhDiv50Width" xmlns="">000<br />01-21</div><div class="PhDiv50Width" xmlns="">000<br />12-20</div><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>05-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>07-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>19,842</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>2,093</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div><div class="PhDiv50Width" xmlns="">000<br />01-21</div><div class="PhDiv50Width" xmlns="">000<br />12-20</div><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>14150</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>24-09-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>19-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>19-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>19-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-10-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-09-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>14,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>14,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />10-20</div><div class="PhDiv50Width" xmlns="">000<br />09-20</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>32300</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>24-08-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>07-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>07-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-07-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>32,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>31,960</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div><div class="PhDiv50Width" xmlns="">000<br />01-21</div><div class="PhDiv50Width" xmlns="">000<br />12-20</div><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div><div class="PhDiv50Width" xmlns="">000<br />09-20</div><div class="PhDiv50Width" xmlns="">000<br />08-20</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>NO COLLATERAL</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>13-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-03-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>07-03-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-03-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-03-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />03-20</div><div class="PhDiv50Width" xmlns="">000<br />02-20</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>NO COLLATERAL</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>24-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>13-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>13-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>29-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>16-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>20-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>20-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,500</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>NO COLLATERAL</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>25-12-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>24-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>24-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-12-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>8,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>01-11-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>05-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>14-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-11-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-11-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>5,290</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>488</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>6,344</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div><div class="PhDiv50Width" xmlns="">000<br />09-20</div><div class="PhDiv50Width" xmlns="">000<br />08-20</div><div class="PhDiv50Width" xmlns="">000<br />07-20</div><div class="PhDiv50Width" xmlns="">000<br />06-20</div><div class="PhDiv50Width" xmlns="">000<br />05-20</div><div class="PhDiv50Width" xmlns="">000<br />04-20</div><div class="PhDiv50Width" xmlns="">000<br />03-20</div><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div><div class="PhDiv50Width" xmlns="">000<br />11-19</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>29-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>30-01-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>19-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>19-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>22,700</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div><div class="PhDiv50Width" xmlns="">000<br />11-19</div><div class="PhDiv50Width" xmlns="">000<br />10-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>10-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>02-06-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>02-06-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2020</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>28,650</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>3,583</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>8</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>3,583</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />06-20</div><div class="PhDiv50Width" xmlns="">000<br />05-20</div><div class="PhDiv50Width" xmlns="">000<br />04-20</div><div class="PhDiv50Width" xmlns="">000<br />03-20</div><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div><div class="PhDiv50Width" xmlns="">000<br />11-19</div><div class="PhDiv50Width" xmlns="">000<br />10-19</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>31750</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>10-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>29-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>29-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>29-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>30,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>30,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />08-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>15050</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>03-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>04-09-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>04-09-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>04-09-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-09-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>15,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>15,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />09-19</div><div class="PhDiv50Width" xmlns="">000<br />08-19</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>45150</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>30-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>15-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>15-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>15-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-10-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>45,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>45,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />10-19</div><div class="PhDiv50Width" xmlns="">000<br />09-19</div><div class="PhDiv50Width" xmlns="">000<br />08-19</div><div class="PhDiv50Width" xmlns="">000<br />07-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>GOLD LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>19500</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>GOLD</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>11-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>30-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>30-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>19,500</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>26</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>19,500</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />07-19</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>01-05-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>06-09-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>02-11-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-11-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-11-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>11,188</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>1,865</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>6</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />11-19</div><div class="PhDiv50Width" xmlns="">000<br />10-19</div><div class="PhDiv50Width" xmlns="">000<br />09-19</div><div class="PhDiv50Width" xmlns="">000<br />08-19</div><div class="PhDiv50Width" xmlns="">000<br />07-19</div><div class="PhDiv50Width" xmlns="">000<br />06-19</div><div class="PhDiv50Width" xmlns="">000<br />05-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>04-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>01-01-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>01-01-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-01-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-01-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />01-21</div><div class="PhDiv50Width" xmlns="">STD<br />12-20</div><div class="PhDiv50Width" xmlns="">STD<br />11-20</div><div class="PhDiv50Width" xmlns="">STD<br />10-20</div><div class="PhDiv50Width" xmlns="">STD<br />09-20</div><div class="PhDiv50Width" xmlns="">STD<br />08-20</div><div class="PhDiv50Width" xmlns="">STD<br />07-20</div><div class="PhDiv50Width" xmlns="">STD<br />06-20</div><div class="PhDiv50Width" xmlns="">STD<br />05-20</div><div class="PhDiv50Width" xmlns="">STD<br />04-20</div><div class="PhDiv50Width" xmlns="">STD<br />03-20</div><div class="PhDiv50Width" xmlns="">STD<br />02-20</div><div class="PhDiv50Width" xmlns="">STD<br />01-20</div><div class="PhDiv50Width" xmlns="">STD<br />12-19</div><div class="PhDiv50Width" xmlns="">STD<br />11-19</div><div class="PhDiv50Width" xmlns="">STD<br />10-19</div><div class="PhDiv50Width" xmlns="">STD<br />09-19</div><div class="PhDiv50Width" xmlns="">STD<br />08-19</div><div class="PhDiv50Width" xmlns="">STD<br />07-19</div><div class="PhDiv50Width" xmlns="">STD<br />06-19</div><div class="PhDiv50Width" xmlns="">STD<br />05-19</div><div class="PhDiv50Width" xmlns="">STD<br />04-19</div><div class="PhDiv50Width" xmlns="">STD<br />03-19</div><div class="PhDiv50Width" xmlns="">STD<br />02-19</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>TWO-WHEELER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL VALUE:</span>86490</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>PROPERTY</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>25-04-2018</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>27-04-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>30-04-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-04-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-04-2021</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2018</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>69,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>2,635</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>36</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>12.5</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>95,755</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div><div class="PhDiv50Width" xmlns="">000<br />01-21</div><div class="PhDiv50Width" xmlns="">000<br />12-20</div><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div><div class="PhDiv50Width" xmlns="">000<br />09-20</div><div class="PhDiv50Width" xmlns="">000<br />08-20</div><div class="PhDiv50Width" xmlns="">000<br />07-20</div><div class="PhDiv50Width" xmlns="">000<br />06-20</div><div class="PhDiv50Width" xmlns="">000<br />05-20</div><div class="PhDiv50Width" xmlns="">000<br />04-20</div><div class="PhDiv50Width" xmlns="">000<br />03-20</div><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div><div class="PhDiv50Width" xmlns="">000<br />11-19</div><div class="PhDiv50Width" xmlns="">000<br />10-19</div><div class="PhDiv50Width" xmlns="">000<br />09-19</div><div class="PhDiv50Width" xmlns="">000<br />08-19</div><div class="PhDiv50Width" xmlns="">000<br />07-19</div><div class="PhDiv50Width" xmlns="">000<br />06-19</div><div class="PhDiv50Width" xmlns="">000<br />05-19</div><div class="PhDiv50Width" xmlns="">000<br />04-19</div><div class="PhDiv50Width" xmlns="">000<br />03-19</div><div class="PhDiv50Width" xmlns="">000<br />02-19</div><div class="PhDiv50Width" xmlns="">000<br />01-19</div><div class="PhDiv50Width" xmlns="">000<br />12-18</div><div class="PhDiv50Width" xmlns="">000<br />11-18</div><div class="PhDiv50Width" xmlns="">000<br />10-18</div><div class="PhDiv50Width" xmlns="">000<br />09-18</div><div class="PhDiv50Width" xmlns="">000<br />08-18</div><div class="PhDiv50Width" xmlns="">000<br />07-18</div><div class="PhDiv50Width" xmlns="">000<br />06-18</div><div class="PhDiv50Width" xmlns="">000<br />05-18</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>27-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>04-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>04-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>28-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-02-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />02-19</div><div class="PhDiv50Width" xmlns="">STD<br />01-19</div><div class="PhDiv50Width" xmlns="">STD<br />12-18</div><div class="PhDiv50Width" xmlns="">STD<br />11-18</div><div class="PhDiv50Width" xmlns="">STD<br />10-18</div><div class="PhDiv50Width" xmlns="">STD<br />09-18</div><div class="PhDiv50Width" xmlns="">STD<br />08-18</div><div class="PhDiv50Width" xmlns="">STD<br />07-18</div><div class="PhDiv50Width" xmlns="">STD<br />06-18</div><div class="PhDiv50Width" xmlns="">STD<br />05-18</div><div class="PhDiv50Width" xmlns="">STD<br />04-18</div><div class="PhDiv50Width" xmlns="">STD<br />03-18</div><div class="PhDiv50Width" xmlns="">STD<br />02-18</div><div class="PhDiv50Width" xmlns="">STD<br />01-18</div><div class="PhDiv50Width" xmlns="">STD<br />12-17</div><div class="PhDiv50Width" xmlns="">STD<br />11-17</div><div class="PhDiv50Width" xmlns="">STD<br />10-17</div><div class="PhDiv50Width" xmlns="">STD<br />09-17</div><div class="PhDiv50Width" xmlns="">STD<br />08-17</div><div class="PhDiv50Width" xmlns="">STD<br />07-17</div><div class="PhDiv50Width" xmlns="">STD<br />06-17</div><div class="PhDiv50Width" xmlns="">STD<br />05-17</div><div class="PhDiv50Width" xmlns="">STD<br />04-17</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>30-03-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>05-11-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>02-12-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-12-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-12-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-03-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>15,900</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>2,272</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>7</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>15,904</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />12-17</div><div class="PhDiv50Width" xmlns="">000<br />11-17</div><div class="PhDiv50Width" xmlns="">000<br />10-17</div><div class="PhDiv50Width" xmlns="">000<br />09-17</div><div class="PhDiv50Width" xmlns="">000<br />08-17</div><div class="PhDiv50Width" xmlns="">000<br />07-17</div><div class="PhDiv50Width" xmlns="">000<br />06-17</div><div class="PhDiv50Width" xmlns="">000<br />05-17</div><div class="PhDiv50Width" xmlns="">000<br />04-17</div><div class="PhDiv50Width" xmlns="">000<br />03-17</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>27-04-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>27-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>27-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-04-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>4,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />04-17</div><div class="PhDiv50Width" xmlns="">STD<br />03-17</div><div class="PhDiv50Width" xmlns="">STD<br />02-17</div><div class="PhDiv50Width" xmlns="">STD<br />01-17</div><div class="PhDiv50Width" xmlns="">STD<br />12-16</div><div class="PhDiv50Width" xmlns="">STD<br />11-16</div><div class="PhDiv50Width" xmlns="">STD<br />10-16</div><div class="PhDiv50Width" xmlns="">STD<br />09-16</div><div class="PhDiv50Width" xmlns="">STD<br />08-16</div><div class="PhDiv50Width" xmlns="">STD<br />07-16</div><div class="PhDiv50Width" xmlns="">STD<br />06-16</div><div class="PhDiv50Width" xmlns="">STD<br />05-16</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>26-02-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>05-11-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>20-11-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-11-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-11-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>22,800</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>1,900</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>15,200</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />11-16</div><div class="PhDiv50Width" xmlns="">000<br />10-16</div><div class="PhDiv50Width" xmlns="">000<br />09-16</div><div class="PhDiv50Width" xmlns="">000<br />08-16</div><div class="PhDiv50Width" xmlns="">000<br />07-16</div><div class="PhDiv50Width" xmlns="">000<br />06-16</div><div class="PhDiv50Width" xmlns="">000<br />05-16</div><div class="PhDiv50Width" xmlns="">000<br />04-16</div><div class="PhDiv50Width" xmlns="">STD<br />03-16</div><div class="PhDiv50Width" xmlns="">STD<br />02-16</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>04-02-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>20-04-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-04-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-04-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">EMI:</span>833</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">REPAYMENT TENURE:</span>12</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">INTEREST RATE:</span>10.25</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />04-15</div><div class="PhDiv50Width" xmlns="">000<br />03-15</div><div class="PhDiv50Width" xmlns="">000<br />02-15</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CONSUMER LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>08-08-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>05-08-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>24-09-2015</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-03-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-03-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>36,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />03-16</div><div class="PhDiv50Width" xmlns="">STD<br />02-16</div><div class="PhDiv50Width" xmlns="">XXX<br />01-16</div><div class="PhDiv50Width" xmlns="">STD<br />12-15</div><div class="PhDiv50Width" xmlns="">XXX<br />11-15</div><div class="PhDiv50Width" xmlns="">XXX<br />10-15</div><div class="PhDiv50Width" xmlns="">STD<br />09-15</div><div class="PhDiv50Width" xmlns="">STD<br />08-15</div><div class="PhDiv50Width" xmlns="">STD<br />07-15</div><div class="PhDiv50Width" xmlns="">STD<br />06-15</div><div class="PhDiv50Width" xmlns="">STD<br />05-15</div><div class="PhDiv50Width" xmlns="">STD<br />04-15</div><div class="PhDiv50Width" xmlns="">STD<br />03-15</div><div class="PhDiv50Width" xmlns="">STD<br />02-15</div><div class="PhDiv50Width" xmlns="">STD<br />01-15</div><div class="PhDiv50Width" xmlns="">STD<br />12-14</div><div class="PhDiv50Width" xmlns="">STD<br />11-14</div><div class="PhDiv50Width" xmlns="">STD<br />10-14</div><div class="PhDiv50Width" xmlns="">STD<br />09-14</div><div class="PhDiv50Width" xmlns="">STD<br />08-14</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>10-04-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>10-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>10-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-06-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>3,20,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />05-16</div><div class="PhDiv50Width" xmlns="">STD<br />04-16</div><div class="PhDiv50Width" xmlns="">STD<br />03-16</div><div class="PhDiv50Width" xmlns="">STD<br />02-16</div><div class="PhDiv50Width" xmlns="">STD<br />01-16</div><div class="PhDiv50Width" xmlns="">STD<br />12-15</div><div class="PhDiv50Width" xmlns="">STD<br />11-15</div><div class="PhDiv50Width" xmlns="">STD<br />10-15</div><div class="PhDiv50Width" xmlns="">XXX<br />09-15</div><div class="PhDiv50Width" xmlns="">STD<br />08-15</div><div class="PhDiv50Width" xmlns="">STD<br />07-15</div><div class="PhDiv50Width" xmlns="">STD<br />06-15</div><div class="PhDiv50Width" xmlns="">XXX<br />05-15</div><div class="PhDiv50Width" xmlns="">XXX<br />04-15</div><div class="PhDiv50Width" xmlns="">XXX<br />03-15</div><div class="PhDiv50Width" xmlns="">XXX<br />02-15</div><div class="PhDiv50Width" xmlns="">XXX<br />01-15</div><div class="PhDiv50Width" xmlns="">XXX<br />12-14</div><div class="PhDiv50Width" xmlns="">STD<br />11-14</div><div class="PhDiv50Width" xmlns="">XXX<br />10-14</div><div class="PhDiv50Width" xmlns="">STD<br />09-14</div><div class="PhDiv50Width" xmlns="">STD<br />08-14</div><div class="PhDiv50Width" xmlns="">STD<br />07-14</div><div class="PhDiv50Width" xmlns="">STD<br />06-14</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>CEASE-TERMINATED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>EDUCATION LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>01-07-2011</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>08-03-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-03-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-03-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-03-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>2,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>1,49,376</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">STD<br />03-14</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">COLLATERAL TYPE:</span>NO COLLATERAL</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>29-12-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>23-03-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>31-03-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-04-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-04-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>61,400</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CREDIT LIMIT:</span>19,000</td></tr><tr /><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />04-19</div><div class="PhDiv50Width" xmlns="">900<br />03-19</div><div class="PhDiv50Width" xmlns="">900<br />02-19</div><div class="PhDiv50Width" xmlns="">900<br />01-19</div><div class="PhDiv50Width" xmlns="">900<br />12-18</div><div class="PhDiv50Width" xmlns="">900<br />11-18</div><div class="PhDiv50Width" xmlns="">900<br />10-18</div><div class="PhDiv50Width" xmlns="">900<br />09-18</div><div class="PhDiv50Width" xmlns="">900<br />08-18</div><div class="PhDiv50Width" xmlns="">900<br />07-18</div><div class="PhDiv50Width" xmlns="">900<br />06-18</div><div class="PhDiv50Width" xmlns="">900<br />05-18</div><div class="PhDiv50Width" xmlns="">900<br />04-18</div><div class="PhDiv50Width" xmlns="">900<br />03-18</div><div class="PhDiv50Width" xmlns="">900<br />02-18</div><div class="PhDiv50Width" xmlns="">900<br />01-18</div><div class="PhDiv50Width" xmlns="">900<br />12-17</div><div class="PhDiv50Width" xmlns="">900<br />11-17</div><div class="PhDiv50Width" xmlns="">900<br />10-17</div><div class="PhDiv50Width" xmlns="">900<br />09-17</div><div class="PhDiv50Width" xmlns="">900<br />08-17</div><div class="PhDiv50Width" xmlns="">900<br />07-17</div><div class="PhDiv50Width" xmlns="">900<br />06-17</div><div class="PhDiv50Width" xmlns="">900<br />05-17</div><div class="PhDiv50Width" xmlns="">900<br />04-17</div><div class="PhDiv50Width" xmlns="">900<br />03-17</div><div class="PhDiv50Width" xmlns="">900<br />02-17</div><div class="PhDiv50Width" xmlns="">900<br />01-17</div><div class="PhDiv50Width" xmlns="">900<br />12-16</div><div class="PhDiv50Width" xmlns="">900<br />11-16</div><div class="PhDiv50Width" xmlns="">900<br />10-16</div><div class="PhDiv50Width" xmlns="">900<br />09-16</div><div class="PhDiv50Width" xmlns="">900<br />08-16</div><div class="PhDiv50Width" xmlns="">900<br />07-16</div><div class="PhDiv50Width" xmlns="">900<br />06-16</div><div class="PhDiv50Width" xmlns="">900<br />05-16</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>14-12-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>11-05-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>16-07-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-07-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-07-2017</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-08-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>33,075</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr /><tr /><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT FREQ:</span>Monthly</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />07-17</div><div class="PhDiv50Width" xmlns="">XXX<br />06-17</div><div class="PhDiv50Width" xmlns="">000<br />05-17</div><div class="PhDiv50Width" xmlns="">000<br />04-17</div><div class="PhDiv50Width" xmlns="">000<br />03-17</div><div class="PhDiv50Width" xmlns="">000<br />02-17</div><div class="PhDiv50Width" xmlns="">000<br />01-17</div><div class="PhDiv50Width" xmlns="">000<br />12-16</div><div class="PhDiv50Width" xmlns="">000<br />11-16</div><div class="PhDiv50Width" xmlns="">000<br />10-16</div><div class="PhDiv50Width" xmlns="">000<br />09-16</div><div class="PhDiv50Width" xmlns="">000<br />08-16</div><div class="PhDiv50Width" xmlns="">XXX<br />07-16</div><div class="PhDiv50Width" xmlns="">000<br />06-16</div><div class="PhDiv50Width" xmlns="">000<br />05-16</div><div class="PhDiv50Width" xmlns="">XXX<br />04-16</div><div class="PhDiv50Width" xmlns="">000<br />03-16</div><div class="PhDiv50Width" xmlns="">XXX<br />02-16</div><div class="PhDiv50Width" xmlns="">000<br />01-16</div><div class="PhDiv50Width" xmlns="">000<br />12-15</div><div class="PhDiv50Width" xmlns="">000<br />11-15</div><div class="PhDiv50Width" xmlns="">000<br />10-15</div><div class="PhDiv50Width" xmlns="">000<br />09-15</div><div class="PhDiv50Width" xmlns="">000<br />08-15</div><div class="PhDiv50Width" xmlns="">000<br />07-15</div><div class="PhDiv50Width" xmlns="">XXX<br />06-15</div><div class="PhDiv50Width" xmlns="">XXX<br />05-15</div><div class="PhDiv50Width" xmlns="">XXX<br />04-15</div><div class="PhDiv50Width" xmlns="">XXX<br />03-15</div><div class="PhDiv50Width" xmlns="">XXX<br />02-15</div><div class="PhDiv50Width" xmlns="">XXX<br />01-15</div><div class="PhDiv50Width" xmlns="">XXX<br />12-14</div><div class="PhDiv50Width" xmlns="">XXX<br />11-14</div><div class="PhDiv50Width" xmlns="">XXX<br />10-14</div><div class="PhDiv50Width" xmlns="">XXX<br />09-14</div><div class="PhDiv50Width" xmlns="">XXX<br />08-14</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>PERSONAL LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>14-12-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>08-11-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>15-11-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-11-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-11-2010</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-12-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>51,240</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />11-10</div><div class="PhDiv50Width" xmlns="">025<br />10-10</div><div class="PhDiv50Width" xmlns="">000<br />09-10</div><div class="PhDiv50Width" xmlns="">000<br />08-10</div><div class="PhDiv50Width" xmlns="">000<br />07-10</div><div class="PhDiv50Width" xmlns="">000<br />06-10</div><div class="PhDiv50Width" xmlns="">000<br />05-10</div><div class="PhDiv50Width" xmlns="">025<br />04-10</div><div class="PhDiv50Width" xmlns="">000<br />03-10</div><div class="PhDiv50Width" xmlns="">025<br />02-10</div><div class="PhDiv50Width" xmlns="">025<br />01-10</div><div class="PhDiv50Width" xmlns="">000<br />12-09</div><div class="PhDiv50Width" xmlns="">000<br />11-09</div><div class="PhDiv50Width" xmlns="">000<br />10-09</div><div class="PhDiv50Width" xmlns="">000<br />09-09</div><div class="PhDiv50Width" xmlns="">000<br />08-09</div><div class="PhDiv50Width" xmlns="">000<br />07-09</div><div class="PhDiv50Width" xmlns="">000<br />06-09</div><div class="PhDiv50Width" xmlns="">000<br />05-09</div><div class="PhDiv50Width" xmlns="">000<br />04-09</div><div class="PhDiv50Width" xmlns="">000<br />03-09</div><div class="PhDiv50Width" xmlns="">000<br />02-09</div><div class="PhDiv50Width" xmlns="">000<br />01-09</div><div class="PhDiv50Width" xmlns="">000<br />12-08</div><div class="PhDiv50Width" xmlns="">000<br />11-08</div><div class="PhDiv50Width" xmlns="">000<br />10-08</div><div class="PhDiv50Width" xmlns="">000<br />09-08</div><div class="PhDiv50Width" xmlns="">000<br />08-08</div><div class="PhDiv50Width" xmlns="">000<br />07-08</div><div class="PhDiv50Width" xmlns="">000<br />06-08</div><div class="PhDiv50Width" xmlns="">000<br />05-08</div><div class="PhDiv50Width" xmlns="">000<br />04-08</div><div class="PhDiv50Width" xmlns="">000<br />03-08</div><div class="PhDiv50Width" xmlns="">000<br />02-08</div><div class="PhDiv50Width" xmlns="">000<br />01-08</div><div class="PhDiv50Width" xmlns="">000<br />12-07</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>26-10-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>30-06-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>30-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-06-2022</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-07-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>22,732</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CREDIT LIMIT:</span>18,000</td></tr><tr /><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACTUAL PAYMENT:</span>13,000</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">CREDIT FACILITY STATUS:</div> SETTLED</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />06-22</div><div class="PhDiv50Width" xmlns="">000<br />05-22</div><div class="PhDiv50Width" xmlns="">000<br />04-22</div><div class="PhDiv50Width" xmlns="">000<br />03-22</div><div class="PhDiv50Width" xmlns="">000<br />02-22</div><div class="PhDiv50Width" xmlns="">000<br />01-22</div><div class="PhDiv50Width" xmlns="">000<br />12-21</div><div class="PhDiv50Width" xmlns="">000<br />11-21</div><div class="PhDiv50Width" xmlns="">000<br />10-21</div><div class="PhDiv50Width" xmlns="">000<br />09-21</div><div class="PhDiv50Width" xmlns="">000<br />08-21</div><div class="PhDiv50Width" xmlns="">000<br />07-21</div><div class="PhDiv50Width" xmlns="">000<br />06-21</div><div class="PhDiv50Width" xmlns="">000<br />05-21</div><div class="PhDiv50Width" xmlns="">000<br />04-21</div><div class="PhDiv50Width" xmlns="">000<br />03-21</div><div class="PhDiv50Width" xmlns="">000<br />02-21</div><div class="PhDiv50Width" xmlns="">000<br />01-21</div><div class="PhDiv50Width" xmlns="">000<br />12-20</div><div class="PhDiv50Width" xmlns="">000<br />11-20</div><div class="PhDiv50Width" xmlns="">000<br />10-20</div><div class="PhDiv50Width" xmlns="">000<br />09-20</div><div class="PhDiv50Width" xmlns="">000<br />08-20</div><div class="PhDiv50Width" xmlns="">000<br />07-20</div><div class="PhDiv50Width" xmlns="">000<br />06-20</div><div class="PhDiv50Width" xmlns="">000<br />05-20</div><div class="PhDiv50Width" xmlns="">000<br />04-20</div><div class="PhDiv50Width" xmlns="">000<br />03-20</div><div class="PhDiv50Width" xmlns="">000<br />02-20</div><div class="PhDiv50Width" xmlns="">000<br />01-20</div><div class="PhDiv50Width" xmlns="">000<br />12-19</div><div class="PhDiv50Width" xmlns="">000<br />11-19</div><div class="PhDiv50Width" xmlns="">000<br />10-19</div><div class="PhDiv50Width" xmlns="">000<br />09-19</div><div class="PhDiv50Width" xmlns="">000<br />08-19</div><div class="PhDiv50Width" xmlns="">000<br />07-19</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>HOUSING LOAN</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>GUARANTOR</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>17-10-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>07-01-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>07-01-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>31-01-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-01-2019</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-02-2016</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">SANCTIONED:</span>3,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr><td colspan="4" height="8px" /></tr><tr><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />01-19</div><div class="PhDiv50Width" xmlns="">STD<br />12-18</div><div class="PhDiv50Width" xmlns="">STD<br />11-18</div><div class="PhDiv50Width" xmlns="">STD<br />10-18</div><div class="PhDiv50Width" xmlns="">STD<br />09-18</div><div class="PhDiv50Width" xmlns="">STD<br />08-18</div><div class="PhDiv50Width" xmlns="">STD<br />07-18</div><div class="PhDiv50Width" xmlns="">STD<br />06-18</div><div class="PhDiv50Width" xmlns="">STD<br />05-18</div><div class="PhDiv50Width" xmlns="">STD<br />04-18</div><div class="PhDiv50Width" xmlns="">STD<br />03-18</div><div class="PhDiv50Width" xmlns="">STD<br />02-18</div><div class="PhDiv50Width" xmlns="">STD<br />01-18</div><div class="PhDiv50Width" xmlns="">000<br />12-17</div><div class="PhDiv50Width" xmlns="">STD<br />11-17</div><div class="PhDiv50Width" xmlns="">STD<br />10-17</div><div class="PhDiv50Width" xmlns="">STD<br />09-17</div><div class="PhDiv50Width" xmlns="">000<br />08-17</div><div class="PhDiv50Width" xmlns="">STD<br />07-17</div><div class="PhDiv50Width" xmlns="">STD<br />06-17</div><div class="PhDiv50Width" xmlns="">STD<br />05-17</div><div class="PhDiv50Width" xmlns="">STD<br />04-17</div><div class="PhDiv50Width" xmlns="">STD<br />03-17</div><div class="PhDiv50Width" xmlns="">STD<br />02-17</div><div class="PhDiv50Width" xmlns="">STD<br />01-17</div><div class="PhDiv50Width" xmlns="">STD<br />12-16</div><div class="PhDiv50Width" xmlns="">STD<br />11-16</div><div class="PhDiv50Width" xmlns="">STD<br />10-16</div><div class="PhDiv50Width" xmlns="">STD<br />09-16</div><div class="PhDiv50Width" xmlns="">STD<br />08-16</div><div class="PhDiv50Width" xmlns="">000<br />07-16</div><div class="PhDiv50Width" xmlns="">STD<br />06-16</div><div class="PhDiv50Width" xmlns="">000<br />05-16</div><div class="PhDiv50Width" xmlns="">000<br />04-16</div><div class="PhDiv50Width" xmlns="">000<br />03-16</div><div class="PhDiv50Width" xmlns="">000<br />02-16</div></td><tr><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr><tr class="alternatetdbggrey"><td width="250px" class="BlueLabel padAll5">ACCOUNT</td><td width="250px" class="BlueLabel padAll5">DATES</td><td width="250px" class="BlueLabel padAll5">AMOUNTS</td><td width="250px" class="BlueLabel padAll5">STATUS</td></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">MEMBER NAME:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">ACCOUNT NUMBER:</span>NOT DISCLOSED</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">TYPE:</span>CREDIT CARD</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OWNERSHIP:</span>INDIVIDUAL</td></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">OPENED:</span>12-04-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">LAST PAYMENT:</span>11-09-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CLOSED:</span>16-12-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><div class="GreyItalicLabel">REPORTED AND CERTIFIED:</div>28-02-2014</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST START:</span>01-12-2009</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">PMT HIST END:</span>01-05-2007</td></tr><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">HIGH CREDIT:</span>27,757</td></tr><tr><td width="250px" class="BlackLabel padAll5"><span class="GreyItalicLabel">CURRENT BALANCE:</span>0</td></tr><tr /><tr /><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td><td width="250px" class="BlackLabel padAll5" style="vertical-align:top"><table><tr><td width="250px" class="BlackLabel padAll5" /></tr></table></td></tr><tr class="alternatetdbggrey"><td width="750px" colspan="3" class="BlueLabel padAll5">DAYS PAST DUE/ASSET CLASSIFICATION (UP TO 36 MONTHS; LEFT TO RIGHT)</td><td width="250px" class="BlueLabel padAll5" /></tr><tr class="alternatetdbggrey"><td colspan="4" height="8px" /></tr><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5"><div class="PhDiv50Width" xmlns="">000<br />12-09</div><div class="PhDiv50Width" xmlns="">000<br />11-09</div><div class="PhDiv50Width" xmlns="">XXX<br />10-09</div><div class="PhDiv50Width" xmlns="">000<br />09-09</div><div class="PhDiv50Width" xmlns="">068<br />08-09</div><div class="PhDiv50Width" xmlns="">037<br />07-09</div><div class="PhDiv50Width" xmlns="">007<br />06-09</div><div class="PhDiv50Width" xmlns="">000<br />05-09</div><div class="PhDiv50Width" xmlns="">XXX<br />04-09</div><div class="PhDiv50Width" xmlns="">000<br />03-09</div><div class="PhDiv50Width" xmlns="">XXX<br />02-09</div><div class="PhDiv50Width" xmlns="">008<br />01-09</div><div class="PhDiv50Width" xmlns="">000<br />12-08</div><div class="PhDiv50Width" xmlns="">000<br />11-08</div><div class="PhDiv50Width" xmlns="">000<br />10-08</div><div class="PhDiv50Width" xmlns="">007<br />09-08</div><div class="PhDiv50Width" xmlns="">008<br />08-08</div><div class="PhDiv50Width" xmlns="">000<br />07-08</div><div class="PhDiv50Width" xmlns="">006<br />06-08</div><div class="PhDiv50Width" xmlns="">000<br />05-08</div><div class="PhDiv50Width" xmlns="">000<br />04-08</div><div class="PhDiv50Width" xmlns="">000<br />03-08</div><div class="PhDiv50Width" xmlns="">000<br />02-08</div><div class="PhDiv50Width" xmlns="">000<br />01-08</div><div class="PhDiv50Width" xmlns="">000<br />12-07</div><div class="PhDiv50Width" xmlns="">000<br />11-07</div><div class="PhDiv50Width" xmlns="">000<br />10-07</div><div class="PhDiv50Width" xmlns="">000<br />09-07</div><div class="PhDiv50Width" xmlns="">000<br />08-07</div><div class="PhDiv50Width" xmlns="">000<br />07-07</div><div class="PhDiv50Width" xmlns="">006<br />06-07</div><div class="PhDiv50Width" xmlns="">000<br />05-07</div></td><tr class="alternatetdbggrey"><td width="1000px" colspan="4" class="BlackLabel padAll5" /></tr></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td width="330px" class="constitleGreyBig">ENQUIRIES:</td><td width="300px" /><td width="370px" /></tr><tr><td width="250px" class="BlueLabel padAll5">MEMBER</td><td width="250px" class="BlueLabel padAll5">ENQUIRY DATE</td><td width="250px" class="BlueLabel padAll5">ENQUIRY PURPOSE</td><td width="250px" class="BlueLabel padAll5">ENQUIRY AMOUNT</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">05-10-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">50,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">05-10-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">9,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">04-10-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">04-10-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">90,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-09-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">3,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-09-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">26-09-2023</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">50</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">21-09-2023</td><td width="250px" class="BlackLabel padAll5">LOAN AGAINST SHARES/SECURITIES</td><td width="250px" class="BlackLabel padAll5">50,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">14-09-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">3,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">13-09-2023</td><td width="250px" class="BlackLabel padAll5">LOAN AGAINST SHARES/SECURITIES</td><td width="250px" class="BlackLabel padAll5">10,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">13-09-2023</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">500</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">07-09-2023</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">49,500</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">02-09-2023</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">49,500</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-09-2023</td><td width="250px" class="BlackLabel padAll5">PROPERTY LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-09-2023</td><td width="250px" class="BlackLabel padAll5">LOAN AGAINST SHARES/SECURITIES</td><td width="250px" class="BlackLabel padAll5">25,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">31-08-2023</td><td width="250px" class="BlackLabel padAll5">PROPERTY LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">31-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">30-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">6,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">24-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">11-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">7,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">09-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">09-08-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">03-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">10,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">03-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">03-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-08-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">31-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">1,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-07-2023</td><td width="250px" class="BlackLabel padAll5">LOAN AGAINST SHARES/SECURITIES</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-07-2023</td><td width="250px" class="BlackLabel padAll5">LOAN AGAINST SHARES/SECURITIES</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">29-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-07-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">7,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">27-07-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">25,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">24-07-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">8,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">18-07-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">5,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">17-07-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">80,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">20-06-2023</td><td width="250px" class="BlackLabel padAll5">MICROFINANCE – BUSINESS LOAN</td><td width="250px" class="BlackLabel padAll5">15,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">08-06-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">05-06-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">24-05-2023</td><td width="250px" class="BlackLabel padAll5">HOUSING LOAN</td><td width="250px" class="BlackLabel padAll5">40,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">20-05-2023</td><td width="250px" class="BlackLabel padAll5">HOUSING LOAN</td><td width="250px" class="BlackLabel padAll5">40,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">11-05-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-05-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-05-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">09-05-2023</td><td width="250px" class="BlackLabel padAll5">MICROFINANCE – BUSINESS LOAN</td><td width="250px" class="BlackLabel padAll5">5,656</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">08-05-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">05-05-2023</td><td width="250px" class="BlackLabel padAll5">MICROFINANCE – BUSINESS LOAN</td><td width="250px" class="BlackLabel padAll5">5,656</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">12-04-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">24,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">12-04-2023</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">36,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">11-04-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-04-2023</td><td width="250px" class="BlackLabel padAll5">HOUSING LOAN</td><td width="250px" class="BlackLabel padAll5">10,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">06-04-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">20,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">14-03-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">85,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">14-02-2023</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">49,500</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">07-02-2023</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">1,500</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">06-10-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">06-10-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">22-09-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">22-09-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">26-08-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">26-08-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-08-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">3,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">24-07-2022</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">22-07-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">20-06-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">BAJAJ FIN LTD</td><td width="250px" class="BlackLabel padAll5">11-06-2022</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">PAYUFINAN</td><td width="250px" class="BlackLabel padAll5">10-06-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">31-05-2022</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">1</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-05-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">22,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">23-05-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">KRAZYBEE</td><td width="250px" class="BlackLabel padAll5">27-04-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">3,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">02-04-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">PAYUFINAN</td><td width="250px" class="BlackLabel padAll5">21-02-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">17-02-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">22,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">15-02-2022</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">14-02-2022</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">20,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-02-2022</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">2,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">WHIZDMFINANCE</td><td width="250px" class="BlackLabel padAll5">08-02-2022</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">08-12-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">5,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">06-12-2021</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">25-11-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">02-11-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">16-09-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">KRAZYBEE</td><td width="250px" class="BlackLabel padAll5">01-09-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">3,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-07-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-07-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-06-2021</td><td width="250px" class="BlackLabel padAll5">TWO-WHEELER LOAN</td><td width="250px" class="BlackLabel padAll5">35,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">24-06-2021</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">16-06-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">13-05-2021</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr><td width="250px" class="BlackLabel padAll5">Home Credit</td><td width="250px" class="BlackLabel padAll5">30-04-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">9,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">28-02-2021</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">2,50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">CEASE-TERMINATED</td><td width="250px" class="BlackLabel padAll5">19-02-2021</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">16-02-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">10-02-2021</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">08-02-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">CITIBANK</td><td width="250px" class="BlackLabel padAll5">24-01-2021</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">08-01-2021</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-12-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-12-2020</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">25-12-2020</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">01-12-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">7,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">27-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">7,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">27-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">6,00,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">23-11-2020</td><td width="250px" class="BlackLabel padAll5">OTHER</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">22-11-2020</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">100</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">20-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">8,00,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">14-11-2020</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">1,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">PAYUFINAN</td><td width="250px" class="BlackLabel padAll5">14-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">03-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">8,50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">03-11-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">8,50,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">20-10-2020</td><td width="250px" class="BlackLabel padAll5">CREDIT CARD</td><td width="250px" class="BlackLabel padAll5">10,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">16-10-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">7,50,000</td></tr><tr class="alternatetdbggrey"><td width="250px" class="BlackLabel padAll5">NOT DISCLOSED</td><td width="250px" class="BlackLabel padAll5">16-10-2020</td><td width="250px" class="BlackLabel padAll5">CONSUMER LOAN</td><td width="250px" class="BlackLabel padAll5">50,000</td></tr><tr><td width="250px" class="BlackLabel padAll5">DMIFINANCE</td><td width="250px" class="BlackLabel padAll5">13-10-2020</td><td width="250px" class="BlackLabel padAll5">PERSONAL LOAN</td><td width="250px" class="BlackLabel padAll5">1,00,000</td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td><table width="1000" border="0" align="right" class="greybordernopadding" cellpadding="0" cellspacing="0"><tr><td colspan="4" class="BlueLabelBigger1 padAll5">END OF REPORT ONSANDEEP SURESH KUMAVAT NA</td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td align="left" colspan="2" class="padWhole"><table cellpadding="0" cellspacing="0" width="100%"><tr><td class="endStatement AddPad " style="font-size:11px;"><p><strong>All information ("Information") contained in this credit information report (CIR) is the current and up to date information collated by TransUnion CIBILLimited based on information provided by its various members ("Members"). By accessing and using the Information, the user acknowledges and acceptsthe following: While TransUnion CIBIL takes reasonable care in preparing the CIR, TransUnion CIBIL shall not be responsible for errors and/or omissionscaused by inaccurate or inadequate information submitted to it. However, TransUnion CIBIL shall take reasonable steps to ensure accurate reproductionof the information submitted by the Members and, to the extent statutorily permitted, it shall correct any such inaccuracies in the CIR. Further, TransUnionCIBIL does not guarantee the adequacy or completeness of the information and/or its suitability for any specific purpose nor is TransUnion CIBILresponsible for any access or reliance on the CIR. The CIR is not a recommendation by TransUnion CIBIL to any Member to (i) lend or not to lend; (ii)enter into or not to enter into any financial transaction with the concerned individual/entity. Credit Scores do not form part of the CIR. The use of the CIR isgoverned by the provisions of the Credit Information Companies (Regulation) Act, 2005, the Credit Information Companies Regulations, 2006, CreditInformation Companies Rules, 2006 and the terms and conditions of the Operating Rules for TransUnion CIBIL and its Members.</strong></p></td></tr></table></td></tr><tr style="margin-top:4px;"><td align="left" colspan="2" class="padWhole" style="padding-top:11px; padding-bottom:7px;" valign="top"><table cellpadding="0" cellspacing="0" width="100%" style="border-top: 1.5px solid #00a6c9;"><tr><td width="100%" align="center" style="font-size:11px;font-weight: bolder">© 2023 TransUnion CIBIL Limited. (Formerly: Credit Information Bureau (India) Limited). All rights reserved</td></tr><tr><td /></tr><tr><td width="100%" align="center" style="font-size:11px;font-weight: bolder">TransUnion CIBIL CIN : U72300MH2000PLC128359</td></tr></table></td></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr><tr><td colspan="4" height="8px" bgcolor="#FFFFFF" /></tr></table></body></html>
                            sw.Close();
                        }
                        xmlReader.Close();
                    }

                    DateTime date = DateTime.Now;
                    File.WriteAllText(OutputPath, resultHtml);

                    // Download File From Location
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName);
                    Response.Flush();

                    Response.WriteFile(OutputPath);
                    Response.End();

                }

            }

            else
            {
                gblFuction.MsgPopup("No Data Found");
                return;
            }


        }
        #endregion
        #region AsString
        public string AsString(XmlDocument xmlDoc)
        {
            string strXmlText = null;
            if (xmlDoc != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (XmlTextWriter tx = new XmlTextWriter(sw))
                    {
                        xmlDoc.WriteTo(tx);
                        strXmlText = sw.ToString();
                    }
                }
            }
            return strXmlText;
        }
        #endregion

    }
    #region Cibil
    [DataContract]
    public class CIBILAddress
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string line1 { get; set; }
        [DataMember]
        public string line2 { get; set; }
        [DataMember]
        public string line3 { get; set; }
        [DataMember]
        public string stateCode { get; set; }
        [DataMember]
        public string pinCode { get; set; }
        [DataMember]
        public string addressCategory { get; set; }
        [DataMember]
        public string residenceCode { get; set; }
    }

    [DataContract]
    public class ConsumerInputSubject
    {
        [DataMember]
        public TuefHeader tuefHeader { get; set; }
        [DataMember]
        public List<Name> names { get; set; }
        [DataMember]
        public List<Id> ids { get; set; }
        [DataMember]
        public List<Telephone> telephones { get; set; }
        [DataMember]
        public List<CIBILAddress> addresses { get; set; }
        [DataMember]
        public List<EnquiryAccount> enquiryAccounts { get; set; }
    }

    [DataContract]
    public class EnquiryAccount
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string accountNumber { get; set; }
    }

    [DataContract]
    public class Id
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string idNumber { get; set; }
        [DataMember]
        public string idType { get; set; }
    }

    [DataContract]
    public class Name
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string birthDate { get; set; }
        [DataMember]
        public string gender { get; set; }
    }

    [DataContract]
    public class Root
    {
        [DataMember]
        public string serviceCode { get; set; }
        [DataMember]
        public string monitoringDate { get; set; }
        [DataMember]
        public ConsumerInputSubject consumerInputSubject { get; set; }
    }

    [DataContract]
    public class Telephone
    {
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string telephoneNumber { get; set; }
        [DataMember]
        public string telephoneType { get; set; }
    }

    [DataContract]
    public class TuefHeader
    {
        [DataMember]
        public string headerType { get; set; }
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public string memberRefNo { get; set; }
        [DataMember]
        public string gstStateCode { get; set; }
        [DataMember]
        public string enquiryMemberUserId { get; set; }
        [DataMember]
        public string enquiryPassword { get; set; }
        [DataMember]
        public string enquiryPurpose { get; set; }
        [DataMember]
        public string enquiryAmount { get; set; }
        [DataMember]
        public string responseSize { get; set; }
        [DataMember]
        public string ioMedia { get; set; }
        [DataMember]
        public string authenticationMethod { get; set; }
    }
    [DataContract]
    public class CibilResponse
    {
        [DataMember]
        public string success { get; set; }
        [DataMember]
        public string score { get; set; }
        [DataMember]
        public string ReportOrderNo { get; set; }

        public CibilResponse(string success, string score, string ReportOrderNo)
        {
            this.success = success;
            this.score = score;
            this.ReportOrderNo = ReportOrderNo;
        }
    }
    #endregion

}