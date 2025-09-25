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
    public partial class CF_BusEmpDet : CENTRUMBAse
    {
        protected int cPgNo = 1;
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string AllDownloadPath = ConfigurationManager.AppSettings["AllDownloadPath"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string FileSize = ConfigurationManager.AppSettings["FileSize"];

        protected void Page_Load(object sender, EventArgs e)
        {
            InitBasePage();
            ceIncDt.EndDate = DateTime.Now;
            if (!IsPostBack)
            {
                hdnMaxFileSize.Value = MaxFileSize;              
                txtJoinDt.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;

                if (Session[gblValue.LeadID] != null)
                {
                    GetBusEmp();
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
                
                
            }
            
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Employment/Business Details";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFBusEmpDet);
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
              
                StatusButton("Edit");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            StatusButton("Edit");
            btnExit.Enabled = true;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }             
        protected void GetBusEmp()
        {
            Int32  pAssMtdId = 0;
            string vBrCode = "", vIsFileUpload = "";
            DataTable dt, dt1 = null;
            DataSet ds = new DataSet();
            string vStatus = "";
            lblBasicNamebus.Text = "";
            lblApplNm.Text = "";
            string BCPNo = "";
            string ApplNm = "";
            ClearControls();
            try
            {

                vBrCode = Session[gblValue.BrnchCode].ToString();

                if (Session[gblValue.LeadID] != null)
                {
                    hdLeadId.Value = Convert.ToString(Session[gblValue.LeadID]);
                }              
                if (Session[gblValue.EmpStatus] != null)
                {
                    vStatus = (Convert.ToString(Session[gblValue.EmpStatus]));
                }
                if (Session[gblValue.AssMtdId] != null)
                {
                    pAssMtdId = Convert.ToInt32(Session[gblValue.AssMtdId]);
                }
                if (Session[gblValue.ApplNm] != null)
                {
                    ApplNm = Convert.ToString(Session[gblValue.ApplNm]);
                }
                if (Session[gblValue.BCPNO] != null)
                {
                    BCPNo = Convert.ToString(Session[gblValue.BCPNO]);
                }
                hdAssMtdId.Value = Convert.ToString(pAssMtdId);
                  
                CDistrict oDist = new CDistrict();
                if (vStatus != "Pending")
                {
                    
                    StatusButton("Edit");
                    if (pAssMtdId == 1) // 1 for Salaried which will be used for Employment and the others is for Business
                    {
                        pnlEmp.Enabled = true;
                        pnlBus.Enabled = false;
                        dt = oDist.CF_GetEmployDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["StateEdit"] = "Edit";
                            lblBasicName.Text = BCPNo;
                            lblEmpApplNm.Text = ApplNm;
                            txtEmplr.Text = Convert.ToString(dt.Rows[0]["EmplrName"]);
                            txtDesig.Text = Convert.ToString(dt.Rows[0]["Designation"]);
                            txtJoinDt.Text = Convert.ToString(dt.Rows[0]["JoinDate"]);
                            txtJobStbly.Text = Convert.ToString(dt.Rows[0]["CrJobStblty"]);
                            txtTotExp.Text = Convert.ToString(dt.Rows[0]["TotExp"]);
                            txtOfcAddrs.Text = Convert.ToString(dt.Rows[0]["OffcAddrs"]);
                            txtDocDesc.Text = Convert.ToString(dt.Rows[0]["DocDescr"]);
                            lblFileName.Text = Convert.ToString(dt.Rows[0]["EmpFileName"]);
                            hdFileName.Value = Convert.ToString(dt.Rows[0]["EmpFileName"]);
                            vIsFileUpload = Convert.ToString(dt.Rows[0]["IsFileUpload"]);
                            if (vIsFileUpload == "Y")
                            {
                                btnEmpDoc.Enabled = true;
                            }
                            else
                            {
                                btnEmpDoc.Enabled = false;
                            }
                            tbBasicDet.ActiveTabIndex = 1;
                        }
                        else
                        {

                            lblEmpApplNm.Text = ApplNm;
                            lblBasicName.Text = "BC Proposal No:- " + BCPNo;
                            txtEmplr.Text = "";
                            txtDesig.Text = "";
                            txtJoinDt.Text = Session[gblValue.LoginDate].ToString();
                            txtJobStbly.Text = "0";
                            txtTotExp.Text = "0";
                            txtOfcAddrs.Text = "";
                            txtDocDesc.Text = "";
                            lblFileName.Text = "";
                            vIsFileUpload = "N";
                            if (vIsFileUpload == "Y")
                            {
                                btnEmpDoc.Enabled = true;
                            }
                            else
                            {
                                btnEmpDoc.Enabled = false;
                            }
                            tbBasicDet.ActiveTabIndex = 1;
                        }
                    }
                    else
                    {
                        pnlEmp.Enabled = false;
                        pnlBus.Enabled = true;
                        dt = oDist.CF_GetBusinessDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                        if (dt.Rows.Count > 0)
                        {
                            ViewState["StateEdit"] = "Edit";
                            lblBasicNamebus.Text = BCPNo;
                            lblApplNm.Text = ApplNm;
                            tbBasicDet.ActiveTabIndex = 0;
                            txtBusiName.Text = Convert.ToString(dt.Rows[0]["BusiName"]);
                            txtIndustry.Text = Convert.ToString(dt.Rows[0]["Industry"]);
                            ddlBusiType.SelectedIndex = ddlBusiType.Items.IndexOf(ddlBusiType.Items.FindByValue(dt.Rows[0]["BusiType"].ToString()));
                            txtIncDt.Text = Convert.ToString(dt.Rows[0]["IncorpDate"]);
                            txtBusiVintYrs.Text = Convert.ToString(dt.Rows[0]["BusiVintYrs"]);
                            ddlBusiStabYrs.SelectedIndex = ddlBusiStabYrs.Items.IndexOf(ddlBusiStabYrs.Items.FindByValue(dt.Rows[0]["BusiStabYrs"].ToString()));
                            ddlBusiPremisOwnShip.SelectedIndex = ddlBusiPremisOwnShip.Items.IndexOf(ddlBusiPremisOwnShip.Items.FindByValue(dt.Rows[0]["BusiPremiOwn"].ToString()));

                            txtUdyamRegnNo.Text = Convert.ToString(dt.Rows[0]["UdyamRegnNo"]);
                            hdnUdyamRegnCertExt.Value = Convert.ToString(dt.Rows[0]["UdyamRegnFileName"]);
                            txtBKYC2Name.Text = Convert.ToString(dt.Rows[0]["BKYC2Name"]);
                            txtBKYC2No.Text = Convert.ToString(dt.Rows[0]["BKYC2No"]);
                            hdnBKYC2Ext.Value = Convert.ToString(dt.Rows[0]["BKYC2FileName"]);
                            txtBusiAddr.Text = Convert.ToString(dt.Rows[0]["BusiAddr"]);
                            hdnBusiAddrOwnshipExt.Value = Convert.ToString(dt.Rows[0]["BusiAddrOwnFileName"]);

                            if (dt.Rows[0]["UdyamRegnCertUpYN"].ToString() == "Y")
                            {
                                imgUdyamRegnCert.Enabled = true;
                                lblUdyamRegnCertUpYN.Text = hdnUdyamRegnCertExt.Value;
                            }
                            else
                            {
                                imgUdyamRegnCert.Enabled = false;
                                lblUdyamRegnCertUpYN.Text = "";
                            }

                            if (dt.Rows[0]["BKYC2UpYN"].ToString() == "Y")
                            {
                                imgBKYC2.Enabled = true;
                                lblBKYC2UpYN.Text = hdnBKYC2Ext.Value;
                            }
                            else
                            {
                                imgBKYC2.Enabled = false;
                                lblBKYC2UpYN.Text = "";
                            }

                            if (dt.Rows[0]["BusiAddrOwnUpYN"].ToString() == "Y")
                            {
                                imgBusiAddrOwnship.Enabled = true;
                                lblBusiAddrOwnUpYN.Text = hdnBusiAddrOwnshipExt.Value;
                            }
                            else
                            {
                                imgBusiAddrOwnship.Enabled = false;
                                lblBusiAddrOwnUpYN.Text = "";
                            }
                        }
                        else
                        {
                            lblBasicNamebus.Text = BCPNo;
                            lblApplNm.Text = ApplNm;
                            tbBasicDet.ActiveTabIndex = 0;
                        }

                    }

                }
                else
                {
                    ViewState["StateEdit"] = "Add";
                    StatusButton("Add");
                    if (pAssMtdId == 1)
                    {
                        pnlEmp.Enabled = true;
                        pnlBus.Enabled = false;
                        lblBasicName.Text = BCPNo;
                        lblEmpApplNm.Text = ApplNm;
                        tbBasicDet.ActiveTabIndex = 1;
                    }
                    else
                    {
                        pnlEmp.Enabled = false;
                        pnlBus.Enabled = true;
                        lblBasicNamebus.Text = BCPNo;
                        lblApplNm.Text = ApplNm;
                        tbBasicDet.ActiveTabIndex = 0;
                    }
                }

               
                
            }
            finally
            {
                dt = null;
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
                    btnEmpDoc.Enabled = true;

                    break;
                case "Show":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    fuEmpUpld.Enabled = false;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAdd.Enabled = false;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnExit.Enabled = false;
                    btnEmpDoc.Enabled = true;
                    fuEmpUpld.Enabled = true;
                    EnableControl(true);
                    break;
                case "View":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnEmpDoc.Enabled = false;
                    fuEmpUpld.Enabled = false;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAdd.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    btnEmpDoc.Enabled = false;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAdd.Visible = false;
                    btnEdit.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnCancel.Visible = false;
                    btnExit.Enabled = true;
                    break;
                case "Close":
                    btnAdd.Visible = false;
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
            txtEmplr.Enabled = Status;
            txtDesig.Enabled = Status;
            txtJoinDt.Enabled = Status;
            txtTotExp.Enabled = Status;
            txtJobStbly.Enabled = Status;
            txtOfcAddrs.Enabled = Status;
            txtDocDesc.Enabled = Status;

            txtBusiName.Enabled = Status;
            txtIndustry.Enabled = Status;
            ddlBusiType.Enabled = Status;
            txtIncDt.Enabled = Status;
            txtBusiVintYrs.Enabled = Status;
            ddlBusiStabYrs.Enabled = Status;
            ddlBusiPremisOwnShip.Enabled = Status;
            txtUdyamRegnNo.Enabled = Status;
            fuUdyamRegnCert.Enabled = Status;
            imgUdyamRegnCert.Enabled = Status;
            txtBKYC2Name.Enabled = Status;
            txtBKYC2No.Enabled = Status;
            imgBKYC2.Enabled = Status;
            fuBKYC2.Enabled = Status;
            txtBusiAddr.Enabled = Status;
            fuBusiAddrOwnship.Enabled = Status;
            imgBusiAddrOwnship.Enabled = Status;
        }
        private void ClearControls()
        {
            lblEmpApplNm.Text = "";
            lblBasicName.Text = "";
            txtEmplr.Text = "";
            txtDesig.Text = "";
            txtJoinDt.Text = "";
            txtTotExp.Text = "0";
            txtJobStbly.Text = "0";
            txtOfcAddrs.Text = "";
            txtDocDesc.Text = "";
            lblFileName.Text = "";
            btnEmpDoc.Enabled = false;

            txtBusiName.Text = "";
            txtIndustry.Text = "";
            ddlBusiType.SelectedIndex = -1;
            txtIncDt.Text = "";
            txtBusiVintYrs.Text = "";
            ddlBusiStabYrs.SelectedIndex = -1;
            ddlBusiPremisOwnShip.SelectedIndex  = -1;
            txtUdyamRegnNo.Text = "";
            fuUdyamRegnCert.Controls.Clear();
            imgUdyamRegnCert.Controls.Clear();
            txtBKYC2Name.Text = "";
            txtBKYC2No.Text = "";
            imgBKYC2.Controls.Clear();
            fuBKYC2.Controls.Clear();
            txtBusiAddr.Text = "";
            fuBusiAddrOwnship.Controls.Clear();
            imgBusiAddrOwnship.Controls.Clear();

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
                tbBasicDet.ActiveTabIndex = 2;
                StatusButton("Add");
                ClearControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            else vStateEdit = "Edit";
            if (SaveRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                GetBusEmp();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vBrCode = "", vErrMsg = "", vEmpFileName = "", vFileUpload = "", vFileExt = "";
            Int32 vErr = 0, pAssMtdId = 0, vMaxFileSize = 0; Int64 vLeadID = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize); ;
            DateTime vJoinDt = gblFuction.setDate(txtJoinDt.Text.ToString());
            vBrCode = Session[gblValue.BrnchCode].ToString();
            string UdyamRegnCertFileName = "", UdyamRegnCertUplYN = "", UdyamRegnCertExt = "";
            string BKYC2FileName = "", BKYC2UplYN = "", BKYC2Ext = "";
            string BusiAddrOwnFileName = "", BusiAddrOwnUplYN = "", BusiAddrOwnExt = "";
            string vdate = Convert.ToString(Session[gblValue.LoginDate]);
            DateTime vLogDt = gblFuction.setDate(vdate);
            vEmpFileName = "EmploymentProof";
            UdyamRegnCertFileName = "UdyamRegnCert";
            BKYC2FileName = "BKYC2Cert";
            BusiAddrOwnFileName = "BusiAddrOwn";

            CDistrict oDist = null;
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadID = Convert.ToInt64(hdLeadId.Value);
                }
                pAssMtdId = Convert.ToInt32(hdAssMtdId.Value);

                #region EmploymentDetails
                if (pAssMtdId == 1)
                {
                    if (Mode == "Save")
                    {
                        vFileUpload = fuEmpUpld.HasFile == true ? "Y" : "N";
                        if (vFileUpload == "Y")
                        {
                            vFileExt = System.IO.Path.GetExtension(fuEmpUpld.PostedFile.FileName);
                            if (vFileExt.ToLower() == ".pdf")
                            {
                                vEmpFileName = fuEmpUpld.HasFile == true ? vEmpFileName + vFileExt : "";
                            }
                            else
                            {
                                gblFuction.MsgPopup("Only Pdf File Can Be Upload...");
                                return false;
                            }
                            
                            if (fuEmpUpld.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (vFileExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuEmpUpld.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuEmpUpld.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            vEmpFileName = "";
                        }
                        if (Convert.ToString(txtEmplr.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Employer Name Should Not Be Left Blank.");
                            return false;
                        }
                        if (Convert.ToString(txtDesig.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Designation Should Not Be Left Blank.");
                            return false;
                        }
                        if (Convert.ToString(txtJoinDt.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Joining Date Should Not Be Left Blank.");
                            return false;
                        }
                        if (vJoinDt > vLogDt)
                        {
                            gblFuction.MsgPopup("Joining Date Should Greter Than Login Date.");
                            return false;
                        }
                        if (txtJobStbly.Text == "")
                        {
                            gblFuction.MsgPopup("Current Job Stability Should not be blank");
                            txtJobStbly.Text = "0";
                            return false;
                            
                        }
                        if (txtTotExp.Text == "")
                        {
                            gblFuction.MsgPopup("Total Work Exp Should not be blank");
                            txtTotExp.Text = "0";
                            return false;
                        }
                        if (txtOfcAddrs.Text == "")
                        {
                            gblFuction.MsgPopup("Office Address Should not be blank");
                            return false;
                        }
                        if (txtDocDesc.Text == "")
                        {
                            gblFuction.MsgPopup("Document Description Should not be blank");
                            return false;
                        }
                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveEmplymntDet(vLeadID, Convert.ToString(txtEmplr.Text.Trim()), Convert.ToString(txtDesig.Text.Trim()), Convert.ToString(txtOfcAddrs.Text.Trim()), vJoinDt
                            , Convert.ToInt32(txtJobStbly.Text), Convert.ToDecimal(txtTotExp.Text), Convert.ToString(txtDocDesc.Text.Trim()), vEmpFileName, DocumentBucketURL, this.UserID
                            , vFileUpload, vBrCode, "Save", 0, ref vErrMsg);
                        if (vErr == 0)
                        {
                            if (fuEmpUpld.HasFile)
                            {
                                SaveMemberImages(fuEmpUpld, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_EmploymentProof", vFileExt, "N");
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
                    else if (Mode == "Edit")
                    {
                        vFileUpload = fuEmpUpld.HasFile == true ? "Y" : "N";
                        if (vFileUpload == "Y")
                        {
                            vFileExt = System.IO.Path.GetExtension(fuEmpUpld.PostedFile.FileName);
                            if (vFileExt.ToLower() == ".pdf")
                            {
                                vEmpFileName = fuEmpUpld.HasFile == true ? vEmpFileName + vFileExt : "";
                            }
                            else
                            {
                                gblFuction.MsgPopup("Only Pdf File Can Be Upload...");
                                return false;
                            }

                            if (fuEmpUpld.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                                return false;
                            }
                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (vFileExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuEmpUpld.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuEmpUpld.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            if (lblFileName.Text != "")
                            {
                                vEmpFileName = lblFileName.Text;
                                vFileUpload = "Y";
                            }
                            else
                            {
                                vEmpFileName = "";
                                vFileUpload = "N";
                            }
                            
                        }
                        if (Convert.ToString(txtEmplr.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Employer Name Should Not Be Left Blank.");
                            return false;
                        }
                        if (Convert.ToString(txtDesig.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Designation Should Not Be Left Blank.");
                            return false;
                        }
                        if (Convert.ToString(txtJoinDt.Text.Trim()) == "")
                        {
                            gblFuction.MsgPopup("Joining Date Should Not Be Left Blank.");
                            return false;
                        }
                        if (vJoinDt > vLogDt)
                        {
                            gblFuction.MsgPopup("Joining Date Should Greter Than Login Date.");
                            return false;
                        }
                        if (txtJobStbly.Text == "")
                        {
                            gblFuction.MsgPopup("Current Job Stability Should not be blank");
                            txtJobStbly.Text = "0";
                            return false;
                        }
                        if (txtTotExp.Text == "")
                        {
                            gblFuction.MsgPopup("Total Job Exp Should not be blank");
                            txtTotExp.Text = "0";
                            return false;
                        }
                        if (txtOfcAddrs.Text == "")
                        {
                            gblFuction.MsgPopup("Office Address Should not be blank");
                            return false;
                        }
                        if (txtDocDesc.Text == "")
                        {
                            gblFuction.MsgPopup("Document Description Should not be blank");
                            return false;
                        }
                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveEmplymntDet(vLeadID, Convert.ToString(txtEmplr.Text.Trim()), Convert.ToString(txtDesig.Text.Trim()), Convert.ToString(txtOfcAddrs.Text.Trim()), vJoinDt
                            , Convert.ToInt32(txtJobStbly.Text), Convert.ToDecimal(txtTotExp.Text), Convert.ToString(txtDocDesc.Text.Trim()), vEmpFileName, DocumentBucketURL, this.UserID
                            , vFileUpload, vBrCode, "Edit", 0, ref vErrMsg);
                        if (vErr == 0)
                        {
                            if (fuEmpUpld.HasFile)
                            {
                                SaveMemberImages(fuEmpUpld, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_EmploymentProof", vFileExt, "N");
                            }
                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(vErrMsg);
                            vResult = false;
                        }
                    }
                    else
                    {
                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveEmplymntDet(vLeadID, Convert.ToString(txtEmplr.Text.Trim()), Convert.ToString(txtDesig.Text.Trim()), Convert.ToString(txtOfcAddrs.Text.Trim()), vJoinDt
                            , Convert.ToInt32(txtJobStbly.Text), Convert.ToInt32(txtTotExp.Text), Convert.ToString(txtDocDesc.Text.Trim()), vEmpFileName, DocumentBucketURL, this.UserID
                            , vFileUpload, vBrCode, "Delet", 0, ref vErrMsg);
                        if (vErr == 0)
                        {

                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(vErrMsg);
                            vResult = false;
                        }
                    }
                }
                else
                {

                    if (Mode == "Save")
                    {
                        UdyamRegnCertUplYN = fuUdyamRegnCert.HasFile == true ? "Y" : "N";
                        if (UdyamRegnCertUplYN == "Y")
                        {
                            UdyamRegnCertExt = System.IO.Path.GetExtension(fuUdyamRegnCert.PostedFile.FileName);
                            UdyamRegnCertFileName = fuUdyamRegnCert.HasFile == true ? UdyamRegnCertFileName + UdyamRegnCertExt : "";
                            if (fuUdyamRegnCert.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuUdyamRegnCert.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (UdyamRegnCertExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuUdyamRegnCert.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuUdyamRegnCert.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            UdyamRegnCertFileName = "";
                        }


                        BKYC2UplYN = fuBKYC2.HasFile == true ? "Y" : "N";
                        if (BKYC2UplYN == "Y")
                        {
                            BKYC2Ext = System.IO.Path.GetExtension(fuBKYC2.PostedFile.FileName);
                            BKYC2FileName = fuBKYC2.HasFile == true ? BKYC2FileName + BKYC2Ext : "";
                            if (fuBKYC2.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuBKYC2.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (BKYC2Ext.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuBKYC2.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuBKYC2.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            BKYC2FileName = "";
                        }

                        BusiAddrOwnUplYN = fuBusiAddrOwnship.HasFile == true ? "Y" : "N";
                        if (BKYC2UplYN == "Y")
                        {
                            BusiAddrOwnExt = System.IO.Path.GetExtension(fuBusiAddrOwnship.PostedFile.FileName);
                            BusiAddrOwnFileName = fuBusiAddrOwnship.HasFile == true ? BusiAddrOwnFileName + BusiAddrOwnExt : "";
                            if (fuBusiAddrOwnship.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuBusiAddrOwnship.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (BusiAddrOwnExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuBusiAddrOwnship.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuBusiAddrOwnship.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            BusiAddrOwnFileName = "";
                        }

                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveBusinessDet(vLeadID, vBrCode, Convert.ToString(txtBusiName.Text), Convert.ToString(txtIndustry.Text), Convert.ToString(ddlBusiType.SelectedValue)
                            , gblFuction.setDate(txtIncDt.Text), txtBusiVintYrs.Text, ddlBusiStabYrs.SelectedValue, Convert.ToInt32(ddlBusiPremisOwnShip.SelectedValue)
                            , Convert.ToString(txtUdyamRegnNo.Text.Trim())
                            , UdyamRegnCertUplYN, UdyamRegnCertFileName, DocumentBucketURL, txtBKYC2Name.Text, txtBKYC2No.Text, BKYC2UplYN, BKYC2FileName, DocumentBucketURL
                            , txtBusiAddr.Text, BusiAddrOwnUplYN, BusiAddrOwnFileName, DocumentBucketURL, this.UserID
                            , "Save", 0, ref vErrMsg);
                        if (vErr == 0)
                        {
                            if (fuUdyamRegnCert.HasFile)
                            {
                                SaveMemberImages(fuUdyamRegnCert, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_UdyamRegnCert", UdyamRegnCertExt, "N");
                            }
                            if (fuBKYC2.HasFile)
                            {
                                SaveMemberImages(fuBKYC2, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_BKYC2Cert", BKYC2Ext, "N");
                            }

                            if (fuBusiAddrOwnship.HasFile)
                            {
                                SaveMemberImages(fuBusiAddrOwnship, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_BusiAddrOwn", BusiAddrOwnExt, "N");
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
                    else if (Mode == "Edit")
                    {
                        UdyamRegnCertUplYN = fuUdyamRegnCert.HasFile == true ? "Y" : "N";
                        if (UdyamRegnCertUplYN == "Y")
                        {
                            UdyamRegnCertExt = System.IO.Path.GetExtension(fuUdyamRegnCert.PostedFile.FileName);
                            UdyamRegnCertFileName = fuUdyamRegnCert.HasFile == true ? UdyamRegnCertFileName + UdyamRegnCertExt : "";
                            if (fuUdyamRegnCert.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuUdyamRegnCert.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (UdyamRegnCertExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuUdyamRegnCert.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuUdyamRegnCert.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            if (lblUdyamRegnCertUpYN.Text != "")
                            {
                                UdyamRegnCertFileName = lblUdyamRegnCertUpYN.Text;
                                UdyamRegnCertUplYN = "Y";
                            }
                            else
                            {
                                UdyamRegnCertFileName = "";
                                UdyamRegnCertUplYN = "N";
                            }
                           
                        }
                        BKYC2UplYN = fuBKYC2.HasFile == true ? "Y" : "N";
                        if (BKYC2UplYN == "Y")
                        {
                            BKYC2Ext = System.IO.Path.GetExtension(fuBKYC2.PostedFile.FileName);
                            BKYC2FileName = fuBKYC2.HasFile == true ? BKYC2FileName + BKYC2Ext : "";
                            if (fuBKYC2.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuBKYC2.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (BKYC2Ext.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuBKYC2.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuBKYC2.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {
                            if (lblBKYC2UpYN.Text != "")
                            {
                                BKYC2FileName = lblBKYC2UpYN.Text;
                                BKYC2UplYN = "Y";
                            }
                            else
                            {
                                BKYC2FileName = "";
                                BKYC2UplYN = "N";
                            }
                        }
                        BusiAddrOwnUplYN = fuBusiAddrOwnship.HasFile == true ? "Y" : "N";
                        if (BusiAddrOwnUplYN == "Y")
                        {
                            BusiAddrOwnExt = System.IO.Path.GetExtension(fuBusiAddrOwnship.PostedFile.FileName);
                            BusiAddrOwnFileName = fuBusiAddrOwnship.HasFile == true ? BusiAddrOwnFileName + BusiAddrOwnExt : "";
                            if (fuBusiAddrOwnship.PostedFile.ContentLength > vMaxFileSize)
                            {
                                gblFuction.MsgPopup("Maximum upload file Size exceed the limit.");
                                fuBusiAddrOwnship.Focus();
                                return false;
                            }

                            //---------------------------------------------------------------
                            bool ValidPdf = false;
                            if (BusiAddrOwnExt.ToLower() == ".pdf")
                            {
                                cFileValidate oFile = new cFileValidate();
                                ValidPdf = oFile.ValidatePdf(fuBusiAddrOwnship.FileBytes);
                                if (ValidPdf == false)
                                {
                                    gblFuction.AjxMsgPopup("File upload failed: Invalid file content detected.");
                                    fuBusiAddrOwnship.Focus();
                                    return false;
                                }
                            }
                            //------------------------------------------------------------
                        }
                        else
                        {                           
                            if (lblBusiAddrOwnUpYN.Text != "")
                            {
                                BusiAddrOwnFileName = lblBusiAddrOwnUpYN.Text;
                                BusiAddrOwnUplYN = "Y";
                            }
                            else
                            {
                                BusiAddrOwnFileName = "";
                                BusiAddrOwnUplYN = "N";
                            }
                        }
                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveBusinessDet(vLeadID, vBrCode, Convert.ToString(txtBusiName.Text), Convert.ToString(txtIndustry.Text), Convert.ToString(ddlBusiType.SelectedValue)
                        , gblFuction.setDate(txtIncDt.Text), txtBusiVintYrs.Text, ddlBusiStabYrs.SelectedValue, Convert.ToInt32(ddlBusiPremisOwnShip.SelectedValue), Convert.ToString(txtUdyamRegnNo.Text.Trim())
                        , UdyamRegnCertUplYN, UdyamRegnCertFileName, DocumentBucketURL, txtBKYC2Name.Text, txtBKYC2No.Text, BKYC2UplYN, BKYC2FileName, DocumentBucketURL
                        , txtBusiAddr.Text, BusiAddrOwnUplYN, BusiAddrOwnFileName, DocumentBucketURL, this.UserID
                        , "Edit", 0, ref vErrMsg);

                        if (vErr == 0)
                        {
                            if (fuUdyamRegnCert.HasFile)
                            {
                                SaveMemberImages(fuUdyamRegnCert, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_UdyamRegnCert", UdyamRegnCertExt, "N");
                            }
                            if (fuBKYC2.HasFile)
                            {
                                SaveMemberImages(fuBKYC2, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_BKYC2Cert", BKYC2Ext, "N");
                            }
                            if (fuBusiAddrOwnship.HasFile)
                            {
                                SaveMemberImages(fuBusiAddrOwnship, Convert.ToString(vLeadID), Convert.ToString(vLeadID) + "_BusiAddrOwn", BusiAddrOwnExt, "N");
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
                    else
                    {
                        oDist = new CDistrict();
                        vErr = oDist.CF_SaveBusinessDet(vLeadID, vBrCode, Convert.ToString(txtBusiName.Text), Convert.ToString(txtIndustry.Text), Convert.ToString(ddlBusiType.SelectedValue)
                        , gblFuction.setDate(txtIncDt.Text), txtBusiVintYrs.Text, ddlBusiStabYrs.SelectedValue, Convert.ToInt32(ddlBusiPremisOwnShip.SelectedValue), Convert.ToString(txtUdyamRegnNo.Text.Trim())
                        , UdyamRegnCertUplYN, UdyamRegnCertFileName, DocumentBucketURL, txtBKYC2Name.Text, txtBKYC2No.Text, BKYC2UplYN, BKYC2FileName, DocumentBucketURL
                        , txtBusiAddr.Text, BusiAddrOwnUplYN, BusiAddrOwnFileName, DocumentBucketURL, this.UserID, "Delet", 0, ref vErrMsg);
                        if (vErr == 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(vErrMsg);
                            vResult = false;
                        }
                    }

                }

                #endregion
                return vResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDist = null;
            }
        }
        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string ImgExt, string isImageSaved)
        {
            CApiCalling oAC = new CApiCalling();
            byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
            isImageSaved = oAC.UploadFileMinio(ImgByte, imageName + ImgExt, imageGroup, DocumentBucket, MinioUrl);
            return isImageSaved;
        }
        protected void btnEmpDoc_Click(object sender, EventArgs e)
        {

            string ActNetImage = "", vPdfFile = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lblFileName.Text;
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
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lblFileName.Text);
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
        protected void ViewImgDoc(string ID, string FileName)
        {
            string vBase64String = "";
            string vPdfFile = DocumentBucketURL + ID + "_" + FileName;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            if (FileName.Contains(".pdf"))
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
                vBase64String = GetBase64Image(FileName, ID);
                ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
            }
        }
        protected void imgBusiAddrOwnship_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBusiAddrOwnshipExt.Value);
        }
        protected void imgBKYC2_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBKYC2Ext.Value);
        }
        protected void imgUdyamRegnCert_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnUdyamRegnCertExt.Value);
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