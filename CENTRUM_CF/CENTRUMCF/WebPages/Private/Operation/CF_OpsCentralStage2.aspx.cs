using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using CENTRUMCA;
using CENTRUMBA;
using System.Net;
using System.IO;
using AjaxControlToolkit;
using System.Text;

namespace CENTRUMCF.WebPages.Private.Operation
{
    public partial class CF_OpsCentralStage2 : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string vPathImage = "";
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFDocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        private static string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string CFLeadBucketURL = ConfigurationManager.AppSettings["CFLeadBucketURL"];
        string DocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string FileSize = ConfigurationManager.AppSettings["FileSize"];
        string MaxSelfiSize = ConfigurationManager.AppSettings["MaxSelfiSize"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();

                PopLeadList();
                popCaste();
                PopReligion();
                PopQualification();
                popStateCust();
                PopRelation();
                PopAssesMethod1();
                PopSrchBranch();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["Bank"] = null;
                ViewState["Chq"] = null;
                hdFlag.Value = "N";

                hdSelfiSize.Value = MaxSelfiSize;
                //tbLoanAppDtl.ActiveTabIndex = 0;
                StatusButton("View");
                txtFromDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddMonths(-1));
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                hdnMaxFileSize.Value = MaxFileSize;
                PopLoanScheme();

                if (Session[gblValue.BrnchCode].ToString() != "0000")
                {
                    ddlPendingBranch.SelectedValue = Session[gblValue.BrnchCode].ToString();
                    ddlPendingBranch.Enabled = false;
                }
                else
                {
                    ddlPendingBranch.Enabled = true;
                }

                LoadBasicDetailsList(1);
                tbLoanAppDtl.ActiveTabIndex = 0;
                trSendBk.Visible = false;
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Application(Central OPS-1 Stage)";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFLoanApp2);
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
        #region BusinessDetails
        protected void btnDelete_Click(object sender, EventArgs e)
        {

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
                if (ddlSacComm.SelectedValue == "-1")
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
            GenerateEmpBusDtls();
            EnableControl(false);
            StatusButton("Show");

            //txtMajorActivity.Text = "";
            //txtEntrpriseType.Text = "";
            //txtPSL.Text = "";
            //txtOpsIndustry.Text = "";
            //txtBKYCInDt.Text = "";
        }

        protected void btnExit_Click(object sender, EventArgs e)
        {
            hdFlag.Value = "N";
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }

        private void GenerateEmpBusDtls()
        {
            DataTable dt = new DataTable();
            CDistrict oDist = new CDistrict();
            string vIsFileUpload, vIsUpload;
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            if (ViewState["AssMtdId"] != null)
            {
                hdAssMtdId.Value = ViewState["AssMtdId"].ToString();
            }
            if (ViewState["LeadId"] != null)
            {
                hdLeadId.Value = ViewState["LeadId"].ToString();
            }
            if (hdLeadId.Value == "")
            {
                hdLeadId.Value = "0";
            }
            if (hdAssMtdId.Value == "")
            {
                hdAssMtdId.Value = "0";
            }

            try
            {
                if (hdStatus.Value != "Pending")
                {
                    ViewState["StateEdit"] = "Edit";

                    if (Convert.ToInt64(hdAssMtdId.Value) == 1) // 1 for Salaried which will be used for Employment and the others is for Business
                    {
                        pnlEmp.Visible = true;
                        pnlBus.Visible = false;
                        dt = oDist.CF_GetEmployDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                        if (dt.Rows.Count > 0)
                        {
                            // lblBasicName.Text = btnShow.Text;
                            txtEmplr.Text = Convert.ToString(dt.Rows[0]["EmplrName"]);
                            txtDesig.Text = Convert.ToString(dt.Rows[0]["Designation"]);
                            txtJoinDt.Text = Convert.ToString(dt.Rows[0]["JoinDate"]);
                            txtJobStbly.Text = Convert.ToString(dt.Rows[0]["CrJobStblty"]);
                            txtTotExp.Text = Convert.ToString(dt.Rows[0]["TotExp"]);
                            txtOfcAddrs.Text = Convert.ToString(dt.Rows[0]["OffcAddrs"]);
                            txtDocDesc.Text = Convert.ToString(dt.Rows[0]["DocDescr"]);
                            lblFileName.Text = Convert.ToString(dt.Rows[0]["EmpFileName"]);
                            vIsFileUpload = Convert.ToString(dt.Rows[0]["IsFileUpload"]);
                            if (vIsFileUpload == "Y")
                            {
                                btnEmpDoc.Enabled = true;
                            }
                            else
                            {
                                btnEmpDoc.Enabled = false;
                            }
                            //tbLoanAppDtl.ActiveTabIndex = 14;
                        }
                        else
                        {
                            lblBasicName.Text = "BC Proposal No:- " + btnShow.Text;
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
                            //tbLoanAppDtl.ActiveTabIndex = 14;
                        }
                    }
                    else
                    {
                        pnlEmp.Visible = false;
                        pnlBus.Visible = true;
                        dt = oDist.CF_GetOpsBusinessDtlByLeadID(Convert.ToInt64(hdLeadId.Value), vBrCode);

                        if (dt.Rows.Count > 0)
                        {
                            //lblBasicNamebus.Text = btnShow.Text;
                            //tbLoanAppDtl.ActiveTabIndex = 13;
                            txtBusiName.Text = Convert.ToString(dt.Rows[0]["BusiName"]);
                            txtIndustry.Text = Convert.ToString(dt.Rows[0]["Industry"]);
                            ddlBusiType.SelectedIndex = ddlBusiType.Items.IndexOf(ddlBusiType.Items.FindByValue(dt.Rows[0]["BusiType"].ToString()));
                            txtIncDt.Text = Convert.ToString(dt.Rows[0]["IncorpDate"]);
                            txtBusiVintYrs.Text = Convert.ToString(dt.Rows[0]["BusiVintYrs"]);
                            ddlBusiStabYrs.SelectedIndex = ddlBusiStabYrs.Items.IndexOf(ddlBusiStabYrs.Items.FindByValue(dt.Rows[0]["BusiStabYrs"].ToString()));
                            ddlBusiPremisOwnShip.SelectedIndex = ddlBusiPremisOwnShip.Items.IndexOf(ddlBusiPremisOwnShip.Items.FindByValue(dt.Rows[0]["BusiPremiOwn"].ToString()));

                            txtUdyamRegnNo.Text = Convert.ToString(dt.Rows[0]["UdyamRegnNo"]);
                            txtUdyamRegNo.Text = Convert.ToString(dt.Rows[0]["UdyamRegnNo"]);
                            hdnUdyamRegnCertExt.Value = Convert.ToString(dt.Rows[0]["UdyamRegnFileName"]);
                            txtBKYC2Name.Text = Convert.ToString(dt.Rows[0]["BKYC2Name"]);
                            txtBKYC2No.Text = Convert.ToString(dt.Rows[0]["BKYC2No"]);
                            hdnBKYC2Ext.Value = Convert.ToString(dt.Rows[0]["BKYC2FileName"]);
                            txtBusiAddr.Text = Convert.ToString(dt.Rows[0]["BusiAddr"]);
                            hdnBusiAddrOwnshipExt.Value = Convert.ToString(dt.Rows[0]["BusiAddrOwnFileName"]);
                            txtMajorActivity.Text = Convert.ToString(dt.Rows[0]["MajorActivity"]);
                            txtEntrpriseType.Text = Convert.ToString(dt.Rows[0]["EnterpriseType"]);
                            txtPSL.Text = Convert.ToString(dt.Rows[0]["PSL"]);
                            txtOpsIndustry.Text = Convert.ToString(dt.Rows[0]["OpsIndustry"]);
                            txtBKYCInDt.Text = Convert.ToString(dt.Rows[0]["BKYC2Dt"]);

                            if (dt.Rows[0]["UdyamRegnCertUpYN"].ToString() == "Y")
                            {
                                btnUdyamR.Enabled = true;
                                lblUdyamRegnCertUpYN.Text = hdnUdyamRegnCertExt.Value;
                            }
                            else
                            {
                                btnUdyamR.Enabled = false;
                                lblUdyamRegnCertUpYN.Text = "";
                            }

                            if (dt.Rows[0]["BKYC2UpYN"].ToString() == "Y")
                            {
                                btnBKYC2.Enabled = true;
                                lblBKYC2UpYN.Text = hdnBKYC2Ext.Value;
                            }
                            else
                            {
                                btnBKYC2.Enabled = false;
                                lblBKYC2UpYN.Text = "";
                            }

                            if (dt.Rows[0]["BusiAddrOwnUpYN"].ToString() == "Y")
                            {
                                btnBusAdd.Enabled = true;
                                lblBusiAddrOwnUpYN.Text = hdnBusiAddrOwnshipExt.Value;
                            }
                            else
                            {
                                btnBusAdd.Enabled = false;
                                lblBusiAddrOwnUpYN.Text = "";
                            }
                        }
                    }

                }
                else
                {
                    if (Convert.ToInt32(hdAssMtdId.Value) == 1)
                    {
                        pnlEmp.Visible = true;
                        pnlBus.Visible = false;
                        lblBasicName.Text = btnShow.Text;
                        //tbLoanAppDtl.ActiveTabIndex = 14;
                    }
                    else
                    {
                        pnlEmp.Visible = false;
                        pnlBus.Visible = true;
                        lblBasicNamebus.Text = btnShow.Text;
                        //tbLoanAppDtl.ActiveTabIndex = 13;
                    }
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

        protected void btnValidate_OnClick(object sender, EventArgs e)
        {
            gblFuction.AjxMsgPopup("Details not yet provided...");
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
                GenerateEmpBusDtls();
                StatusButton("Show");
                ViewState["StateEdit"] = null;

                txtUdyamRegNo.Text = txtUdyamRegnNo.Text;
                // ClearControls();
            }
            else
            {
                StatusButton("Edit");
            }
        }


        private Boolean SaveRecords(string Mode)
        {
            Boolean vResult = false;
            string vBrCode = "", vErrMsg = "", vEmpFileName = "", vFileUpload = "", vFileExt = "";
            Int32 vErr = 0, vNewId = 0, pAssMtdId = 0, vMaxFileSize = 0; Int64 vLeadID = 0;
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
            DateTime vBKYCInDt = gblFuction.setDate(txtBKYCInDt.Text.ToString());
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
                    oDist = new CDistrict();
                    vErr = oDist.CF_SaveEmplymntDet(vLeadID, Convert.ToString(txtEmplr.Text.Trim()), Convert.ToString(txtDesig.Text.Trim()), Convert.ToString(txtOfcAddrs.Text.Trim()), vJoinDt
                        , Convert.ToInt32(txtJobStbly.Text), Convert.ToInt32(txtTotExp.Text), Convert.ToString(txtDocDesc.Text.Trim()), vEmpFileName, DocumentBucketURL, this.UserID
                        , vFileUpload, vBrCode, "Save", 0, ref vErrMsg);
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
                else
                {
                    oDist = new CDistrict();
                    vErr = oDist.CF_SaveOpsBusinessDet(vLeadID, vBrCode, Convert.ToString(txtBusiName.Text), Convert.ToString(txtIndustry.Text), Convert.ToString(ddlBusiType.SelectedValue)
                        , gblFuction.setDate(txtIncDt.Text), txtBusiVintYrs.Text, ddlBusiStabYrs.SelectedValue, Convert.ToInt32(ddlBusiPremisOwnShip.SelectedValue)
                        , Convert.ToString(txtUdyamRegnNo.Text.Trim())
                        , UdyamRegnCertUplYN, UdyamRegnCertFileName, DocumentBucketURL, txtBKYC2Name.Text, txtBKYC2No.Text, BKYC2UplYN, BKYC2FileName, DocumentBucketURL
                        , txtBusiAddr.Text, BusiAddrOwnUplYN, BusiAddrOwnFileName, DocumentBucketURL, this.UserID
                        , "Save", 0, ref vErrMsg, Convert.ToString(txtMajorActivity.Text), Convert.ToString(txtEntrpriseType.Text), Convert.ToString(txtPSL.Text)
                        , Convert.ToString(txtOpsIndustry.Text), vBKYCInDt);
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
        #endregion
        #endregion
        #region DocumentUpload
        protected void gvEmpDocument_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int i = e.Row.RowIndex;
                DataTable dtIns = null;
                CApplication oApp = null;
                try
                {
                    oApp = new CApplication();
                    FileUpload FUKYcDoc = (FileUpload)e.Row.FindControl("FUKYcDoc");
                    DropDownList ddlDocType = (DropDownList)e.Row.FindControl("ddlDocType");
                    TextBox txtDocRemarks = (TextBox)e.Row.FindControl("txtDocRemarks");
                    TextBox txtDocPassword = (TextBox)e.Row.FindControl("txtDocPassword");
                    HiddenField hdnDocPassword = (HiddenField)e.Row.FindControl("hdnDocPassword");

                    ddlDocType.Items.Clear();
                    dtIns = oApp.GetDocumentMst("DOC");
                    if (dtIns.Rows.Count > 0)
                    {
                        ddlDocType.DataSource = dtIns;
                        ddlDocType.DataTextField = "DocName";
                        ddlDocType.DataValueField = "DocID";
                        ddlDocType.DataBind();
                    }
                    ListItem oLk = new ListItem("<--Select-->", "-1");
                    ddlDocType.Items.Insert(0, oLk);


                    ddlDocType.SelectedIndex = ddlDocType.Items.IndexOf(ddlDocType.Items.FindByValue(e.Row.Cells[7].Text.Trim()));

                    txtDocPassword.TextMode = TextBoxMode.Password;
                    txtDocPassword.Attributes.Add("value", hdnDocPassword.Value);

                }
                finally
                {

                }
            }

        }

        private void ShowEmpRecordsDocUpld()
        {

            DataTable dt = null;
            CApplication oMem = null;
            try
            {
                oMem = new CApplication();
                dt = oMem.CF_GetDocByLeadId(Convert.ToInt64(hdLeadId.Value));
                if (dt.Rows.Count > 0)
                {
                    gvEmpDocument.DataSource = dt;
                    gvEmpDocument.DataBind();

                }
                else
                {
                    LoadEmpDocUpLd();
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

        private void LoadEmpDocUpLd()
        {
            ViewState["KYCDoc"] = null;
            DataTable dt = new DataTable("Table1");
            dt.Columns.Add("SLNo");
            dt.Columns.Add("DocTypeId");
            dt.Columns.Add("DocType");
            dt.Columns.Add("DocRemarks");
            dt.Columns.Add("DocPassword");
            dt.Columns.Add("Doc_Image", typeof(byte[]));
            dt.Columns.Add("ImageBase64");
            dt.Columns.Add("UploadYN");
            dt.AcceptChanges();
            gvEmpDocument.DataSource = dt;
            gvEmpDocument.DataBind();
        }
        #endregion
        #region MainGrid
        private void PopSrchBranch()
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
                    ddlPendingBranch.DataTextField = "BranchName";
                    ddlPendingBranch.DataValueField = "BranchCode";
                    ddlPendingBranch.DataSource = dt;
                    ddlPendingBranch.DataBind();
                    ListItem oItm1 = new ListItem("<--Select-->", "-1");
                    ddlPendingBranch.Items.Insert(0, oItm1);
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(0);
        }
        private void LoadBasicDetailsList(Int32 pPgIndx)
        {
            string vBrCode = "";
            DataTable dt = new DataTable();
            CMember oMem = new CMember();           
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text.ToString());
            DateTime vToDt = gblFuction.setDate(txtToDt.Text.ToString());
            string VStatus = ddlStatus.SelectedValue;
            Int32 vTotRows = 0;

            if (Session[gblValue.BrnchCode].ToString() == "0000") vBrCode = "0000";
            else vBrCode = ddlPendingBranch.SelectedValue;

            try
            {
                dt = oMem.CF_OPSGenerateCentral2Grid(vBrCode, txtSearch.Text.Trim(), vFromDt, vToDt, VStatus, pPgIndx, ref vTotRows);
                if (dt.Rows.Count > 0)
                {
                    gvBasicDet.DataSource = dt;
                    gvBasicDet.DataBind();

                }
                else
                {
                    gvBasicDet.DataSource = null;
                    gvBasicDet.DataBind();
                }
                lblTotPg.Text = CalTotPages(vTotRows).ToString();
                lblCrPg.Text = vPgNo.ToString();
                if (vPgNo == 1)
                {
                    btnPrev.Enabled = false;
                    if (Int32.Parse(lblTotPg.Text) > 1)
                        btnNext.Enabled = true;
                    else
                        btnNext.Enabled = false;
                }
                else
                {
                    btnPrev.Enabled = true;
                    if (vPgNo == Int32.Parse(lblTotPg.Text))
                        btnNext.Enabled = false;
                    else
                        btnNext.Enabled = true;
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
        private int CalTotPages(double pRows)
        {
            int vPgs = (int)Math.Ceiling(pRows / gblValue.PgSize1);
            return vPgs;
        }
        protected void ChangePage(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Prev":
                    vPgNo = Int32.Parse(lblCrPg.Text) - 1;
                    break;
                case "Next":
                    vPgNo = Int32.Parse(lblCrPg.Text) + 1;
                    break;
            }
            LoadBasicDetailsList(vPgNo);
            tbLoanAppDtl.ActiveTabIndex = 0;
        }
        protected void gvBasicDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadID = 0;
            DataSet ds = new DataSet();

            try
            {

                ClearControls(this.Page);


                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                //hdLeadId.Value = Convert.ToString(e.CommandArgument);

                hdLeadId.Value = row.Cells[0].Text;
                ViewState["LeadId"] = hdLeadId.Value;

                // Get the ProductName from the bound field in the row
                hdBasicId.Value = row.Cells[1].Text;
                ViewState["BId"] = hdBasicId.Value;

                hdMemberId.Value = row.Cells[2].Text;
                hdMemberId.Value = hdMemberId.Value.Replace("&nbsp;", "");
                ViewState["MId"] = hdMemberId.Value;
                if (hdMemberId.Value != "")
                {
                    hdIdFlag.Value = "Y";
                }
                else
                {
                    hdIdFlag.Value = "N";
                }

                hdAssMtdId.Value = row.Cells[3].Text;
                ViewState["AssMtdId"] = hdAssMtdId.Value;
                hdStatus.Value = row.Cells[4].Text;
                ViewState["Status"] = hdStatus.Value;
                hdIncomeStatus.Value = row.Cells[5].Text;
                ViewState["IncomeStatus"] = hdIncomeStatus.Value;
                hdAssMtdTypId.Value = row.Cells[6].Text;
                ViewState["AssMtdTypId"] = hdAssMtdTypId.Value;

                ViewState["BcProNo"] = Convert.ToString(e.CommandArgument);
                ViewState["AppNm"] = row.Cells[10].Text;

                hdnBrCode.Value = row.Cells[21].Text;
                ViewState["BCode"] = hdnBrCode.Value;

                ViewState["BcBmStatus"] = hdBcBmStatus.Value = row.Cells[17].Text;
                ViewState["Stg1Status"] = hdStg1Status.Value = row.Cells[18].Text;
                ViewState["Stg2Status"] = hdStg2Status.Value = row.Cells[19].Text;

                switch (e.CommandName)
                {
                    case "ShowInfo":

                        tabChangeFunction(1);

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ChangeInnerTab", "changeInnerTab();", true);
                        break;
                }

            }
            finally
            {

            }
        }

        protected void tbLoanAppDtl_ActiveTabChanged(object sender, EventArgs e)
        {
            TabContainer tabContainer = (TabContainer)sender;
            int ActiveTabIndex = tabContainer.ActiveTabIndex;
            if (ActiveTabIndex > 0)
            {
                tabChangeFunction(ActiveTabIndex);
            }
            else if (ActiveTabIndex == 0)
            {
                trSendBk.Visible = false;
            }
            this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
            this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
            if (ddlPendingBranch.SelectedValue == "-1" && Session[gblValue.BrnchCode].ToString() != "0000")
            {
                ddlPendingBranch.SelectedValue = Session[gblValue.BrnchCode].ToString();
            }
        }

        private void tabChangeFunction(int ActiveTabIndex)
        {
            ClearLabel();
            string vIsFileUpload, vIsUpload;
            DataTable dt = null, dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null;
            DataSet ds = new DataSet();
            CDistrict oDist = new CDistrict();
            CMember oMem = new CMember();
            CCust360 oC360 = new CCust360();
            CCFIncDtl oDit = new CCFIncDtl();
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            if (ViewState["LeadId"] != null)
            {
                hdLeadId.Value = ViewState["LeadId"].ToString();
            }
            if (ViewState["MId"] != null)
            {
                hdMemberId.Value = ViewState["MId"].ToString();
            }
            if (ViewState["BId"] != null)
            {
                hdBasicId.Value = ViewState["BId"].ToString();
            }
            if (ViewState["AssMtdId"] != null)
            {
                hdAssMtdId.Value = ViewState["AssMtdId"].ToString();
            }
            if (ViewState["Status"] != null)
            {
                hdStatus.Value = ViewState["Status"].ToString();
            }
            if (ViewState["IncomeStatus"] != null)
            {
                hdIncomeStatus.Value = ViewState["IncomeStatus"].ToString();
            }
            if (ViewState["AssMtdTypId"] != null)
            {
                hdAssMtdTypId.Value = ViewState["AssMtdTypId"].ToString();
            }
            if (ViewState["BCode"] != null)
            {
                hdnBrCode.Value = ViewState["BCode"].ToString();
            }
            if (ViewState["BcBmStatus"] != null)
            {
                hdBcBmStatus.Value = ViewState["BcBmStatus"].ToString();
            }
            if (ViewState["Stg1Status"] != null)
            {
                hdStg1Status.Value = ViewState["Stg1Status"].ToString();
            }
            if (ViewState["Stg2Status"] != null)
            {
                hdStg2Status.Value = ViewState["Stg2Status"].ToString();
            }

            if (hdBasicId.Value == "")
            {
                hdBasicId.Value = "0";
            }
            if (hdLeadId.Value == "")
            {
                hdLeadId.Value = "0";
            }
            if (hdAssMtdId.Value == "")
            {
                hdAssMtdId.Value = "0";
            }
            if (hdAssMtdTypId.Value == "")
            {
                hdAssMtdTypId.Value = "0";
            }

            hdTabIndex.Value = Convert.ToString(ActiveTabIndex);
            switch (ActiveTabIndex)
            {
                case 0:
                    trSendBk.Visible = false;
                    break;
                case 1:
                    #region BasicDtls
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmBD.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmBD.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNumBD.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNumBD.Text = "";
                        }

                        dt = oDist.CF_GetBasicDetailsById(Convert.ToInt32(hdBasicId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            PopBranch();
                            PopSolarSystemType();

                            PopSegType();
                            PopAppliEntType();
                            PopEPCMst();
                            PopRO();
                            popState();
                            PopAssesMethod();
                            txtBCPartName.Text = "Choice Finserv Pvt. ltd.";
                            hdBasicId.Value = Convert.ToString(dt.Rows[0]["BasicID"]);
                            hdLeadId.Value = Convert.ToString(dt.Rows[0]["LeadID"]);
                            //ddlBCPropNo.SelectedIndex = ddlBCPropNo.Items.IndexOf(ddlBCPropNo.Items.FindByText(dt.Rows[0]["BCPropNo"].ToString()));
                            //---------------Populate BCPropNo-----------------------
                            ddlBCPropNo.Items.Clear();
                            ddlBCPropNo.DataSource = dt;
                            ddlBCPropNo.DataTextField = "BCPropNo";
                            ddlBCPropNo.DataValueField = "LeadID";
                            ddlBCPropNo.DataBind();
                            //-------------------------------------------------------
                            ddlBCPropNo.SelectedIndex = ddlBCPropNo.Items.IndexOf(ddlBCPropNo.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            txtNatProp.Text = Convert.ToString(dt.Rows[0]["NatOfPrpsl"]);
                            if (Convert.ToString(dt.Rows[0]["BCPartnrName"]).Trim() != "") txtBCPartName.Text = Convert.ToString(dt.Rows[0]["BCPartnrName"]);


                            ddlBCStste.ClearSelection();
                            ListItem oli = new ListItem(Convert.ToString(dt.Rows[0]["BCState"]), "-1");
                            ddlBCStste.Items.Insert(0, oli);
                            ddlBranch.SelectedIndex = ddlBranch.Items.IndexOf(ddlBranch.Items.FindByValue(dt.Rows[0]["BranchCode"].ToString()));
                            ddlBcRM.SelectedIndex = ddlBcRM.Items.IndexOf(ddlBcRM.Items.FindByValue(dt.Rows[0]["BCRMID"].ToString()));
                            ddlEPC.SelectedIndex = ddlEPC.Items.IndexOf(ddlEPC.Items.FindByValue(dt.Rows[0]["EPCId"].ToString()));
                            ddlSolPwrSys.SelectedIndex = ddlSolPwrSys.Items.IndexOf(ddlSolPwrSys.Items.FindByValue(dt.Rows[0]["SolPwrSysId"].ToString()));
                            txtDistnce.Text = Convert.ToString(dt.Rows[0]["SourceDistnc"]);
                            txtApplicant.Text = Convert.ToString(dt.Rows[0]["ApplName"]);
                            txtAppNo.Text = Convert.ToString(dt.Rows[0]["AppNo"]);
                            txtAppDt.Text = Convert.ToString(dt.Rows[0]["AppDate"]);
                            ddlEntityType.SelectedIndex = ddlEntityType.Items.IndexOf(ddlEntityType.Items.FindByValue(dt.Rows[0]["ApplEntTypeId"].ToString()));
                            ddlSegType.SelectedIndex = ddlSegType.Items.IndexOf(ddlSegType.Items.FindByValue(dt.Rows[0]["SegTypeID"].ToString()));
                            ddlAssMethod.ClearSelection();
                            ListItem oli7 = new ListItem(Convert.ToString(dt.Rows[0]["MethodName"]), "-1");
                            ddlAssMethod.Items.Insert(0, oli7);
                            ddlPurFacilty.SelectedIndex = ddlPurFacilty.Items.IndexOf(ddlPurFacilty.Items.FindByValue(dt.Rows[0]["PurposeFclty"].ToString()));
                            ddlPrioSec.SelectedIndex = ddlPrioSec.Items.IndexOf(ddlPrioSec.Items.FindByValue(dt.Rows[0]["PriorSecID"].ToString()));
                            txtPSLClass.Text = Convert.ToString(dt.Rows[0]["PSLClass"]);
                            if (Convert.ToString(dt.Rows[0]["EmpProfile"]) != "")
                            {
                                ddlEmpProfile.SelectedValue = Convert.ToString(dt.Rows[0]["EmpProfile"]);
                            }
                            else
                            {
                                ddlEmpProfile.SelectedIndex = -1;
                            }
                        }
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    #region Customer360
                    try
                    {
                        hdFlag.Value = "N";
                        hdnLoanFlag.Value = "N";
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNum1.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum2.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum3.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum4.Text = ViewState["BcProNo"].ToString();
                            lblBCPNum5.Text = ViewState["BcProNo"].ToString();
                            lblBCPNumAppl.Text = ViewState["BcProNo"].ToString();

                        }
                        else
                        {
                            lblBCPNum1.Text = "";
                            lblBCPNum2.Text = "";
                            lblBCPNum3.Text = "";
                            lblBCPNum4.Text = "";
                            lblBCPNum5.Text = "";
                            lblBCPNumAppl.Text = "";
                        }


                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNm1.Text = ViewState["AppNm"].ToString();
                            lblApplNm2.Text = ViewState["AppNm"].ToString();
                            lblApplNm3.Text = ViewState["AppNm"].ToString();
                            lblApplNm4.Text = ViewState["AppNm"].ToString();
                            lblApplNm5.Text = ViewState["AppNm"].ToString();
                            lblApplNmAppl.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNm1.Text = "";
                            lblApplNm2.Text = "";
                            lblApplNm3.Text = "";
                            lblApplNm4.Text = "";
                            lblApplNm5.Text = "";
                            lblApplNmAppl.Text = "";
                        }

                        ds = oC360.CF_GetMemberDtl(hdMemberId.Value);
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                        dt3 = ds.Tables[3];
                        dt4 = ds.Tables[4];
                        dt5 = ds.Tables[5];

                        if (dt.Rows.Count > 0)
                        {

                            ddlCustomer.ClearSelection();
                            ListItem oli = new ListItem(Convert.ToString(dt.Rows[0]["BCPropNo"]), "-1");
                            ddlCustomer.Items.Insert(0, oli);

                            hdnLeadId.Value = dt.Rows[0]["LeadID"].ToString();
                            hdnAppId.Value = Convert.ToString(dt.Rows[0]["MemberID"]);
                            txtCustId.Text = Convert.ToString(dt.Rows[0]["MemberNo"]);
                            txtCustName.Text = Convert.ToString(dt.Rows[0]["MemberName"]);
                            ddlApplGender.SelectedIndex = ddlApplGender.Items.IndexOf(ddlApplGender.Items.FindByValue(dt.Rows[0]["Gender"].ToString()));
                            txtApplDOB.Text = Convert.ToString(dt.Rows[0]["DoB"]);
                            txtApplAge.Text = Convert.ToString(dt.Rows[0]["PresentAge"]);
                            txtApplAgeMaturity.Text = Convert.ToString(dt.Rows[0]["AgeAtLoanMaturity"]);
                            txtApplPan.Text = Convert.ToString(dt.Rows[0]["PanNo"]);
                            txtApplAadhRefNo.Text = Convert.ToString(dt.Rows[0]["AadhRefNo"]);
                            txtApplAadhNo.Text = Convert.ToString(dt.Rows[0]["AadhMaskedNo"]);

                            txtApplVoterNo.Text = Convert.ToString(dt.Rows[0]["VoterID"]);
                            ddlRelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt.Rows[0]["RelWithApp"].ToString()));
                            txtAppMob.Text = Convert.ToString(dt.Rows[0]["MobNo"]);
                            ddlAppEdu.SelectedIndex = ddlAppEdu.Items.IndexOf(ddlAppEdu.Items.FindByValue(dt.Rows[0]["Education"].ToString()));
                            ddlApplMaritalStatus.SelectedIndex = ddlApplMaritalStatus.Items.IndexOf(ddlApplMaritalStatus.Items.FindByValue(dt.Rows[0]["MaritalStatus"].ToString()));
                            txtNoOfFamilyMem.Text = Convert.ToString(dt.Rows[0]["TotalNoFamMem"]);
                            txtNoOfDependent.Text = Convert.ToString(dt.Rows[0]["NoOfDependents"]);
                            ddlApplCast.SelectedIndex = ddlApplCast.Items.IndexOf(ddlApplCast.Items.FindByValue(dt.Rows[0]["Caste"].ToString()));
                            ddlApplReligion.SelectedIndex = ddlApplReligion.Items.IndexOf(ddlApplReligion.Items.FindByValue(dt.Rows[0]["Religion"].ToString()));
                            ddlAppMinorityYN.SelectedIndex = ddlAppMinorityYN.Items.IndexOf(ddlAppMinorityYN.Items.FindByValue(dt.Rows[0]["Minority"].ToString()));
                            txtApplEmail.Text = Convert.ToString(dt.Rows[0]["Email"]);
                            txtAppPerAddress.Text = Convert.ToString(dt.Rows[0]["PerAdd"]);


                            ddlApplState.SelectedIndex = ddlApplState.Items.IndexOf(ddlApplState.Items.FindByValue(dt.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlApplState.SelectedValue), "AppPer");
                            ddlApplDist.SelectedIndex = ddlApplDist.Items.IndexOf(ddlApplDist.Items.FindByValue(dt.Rows[0]["PerDist"].ToString()));

                            txtApplPin.Text = Convert.ToString(dt.Rows[0]["PerPin"]);
                            txtApplCurrAddress.Text = Convert.ToString(dt.Rows[0]["CurrAdd"]);

                            ddlApplCurrState.SelectedIndex = ddlApplCurrState.Items.IndexOf(ddlApplCurrState.Items.FindByValue(dt.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlApplCurrState.SelectedValue), "AppCurr");
                            ddlApplCurrDist.SelectedIndex = ddlApplCurrDist.Items.IndexOf(ddlApplCurrDist.Items.FindByValue(dt.Rows[0]["CurrDist"].ToString()));

                            txtApplCurrPin.Text = Convert.ToString(dt.Rows[0]["CurrPin"]);
                            txtApplLandMark.Text = Convert.ToString(dt.Rows[0]["CurrLandmark"]);
                            ddlApplOwnshipStatus.SelectedIndex = ddlApplOwnshipStatus.Items.IndexOf(ddlApplOwnshipStatus.Items.FindByValue(dt.Rows[0]["OwnShipStatus"].ToString()));
                            ddlResiStabYrs.SelectedIndex = ddlResiStabYrs.Items.IndexOf(ddlResiStabYrs.Items.FindByValue(dt.Rows[0]["ResiStabYrs"].ToString()));

                            hdnAppPanVerifyYN.Value = Convert.ToString(dt.Rows[0]["PanVerifyYN"]);
                            hdnAppAadhVerifyYN.Value = Convert.ToString(dt.Rows[0]["AadhVerifyYN"]);
                            hdnAppVoterVerifyYN.Value = Convert.ToString(dt.Rows[0]["VoterVerifyYN"]);
                            hdnAppOwnshipExt.Value = Convert.ToString(dt.Rows[0]["OwnShipExt"]);
                            lblAppOwnship.Text = "ApplOwnship" + hdnAppOwnshipExt.Value;

                            hdPanFileNmApp.Value = Convert.ToString(dt.Rows[0]["PanFileNm"]);
                            hdAdharFileNmApp.Value = Convert.ToString(dt.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmApp.Value = Convert.ToString(dt.Rows[0]["VoterFileNm"]);
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp1Id.Value = Convert.ToString(dt1.Rows[0]["CoAppID"]);
                            txtCoApp1Id.Text = Convert.ToString(dt1.Rows[0]["CoAppNo"]);
                            txtCoApp1Name.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                            ddlCoApp1Gender.SelectedIndex = ddlCoApp1Gender.Items.IndexOf(ddlCoApp1Gender.Items.FindByValue(dt1.Rows[0]["Gender"].ToString()));
                            txtCoApp1Dob.Text = Convert.ToString(dt1.Rows[0]["DoB"]);
                            txtCoApp1Age.Text = Convert.ToString(dt1.Rows[0]["PresentAge"]);
                            txtCoApp1AgeAtLoanMaturity.Text = Convert.ToString(dt1.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp1Pan.Text = Convert.ToString(dt1.Rows[0]["PanNo"]);
                            txtCoApp1AadhRefNo.Text = Convert.ToString(dt1.Rows[0]["AadhRefNo"]);
                            txtCoApp1AadhNo.Text = Convert.ToString(dt1.Rows[0]["AadhMaskedNo"]);
                            txtCoApp1Voter.Text = Convert.ToString(dt1.Rows[0]["VoterID"]);
                            ddlCoApp1RelWithApp.SelectedIndex = ddlRelWithApp.Items.IndexOf(ddlRelWithApp.Items.FindByValue(dt1.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp1Mob.Text = Convert.ToString(dt1.Rows[0]["MobNo"]);
                            ddlCoApp1Edu.SelectedIndex = ddlCoApp1Edu.Items.IndexOf(ddlCoApp1Edu.Items.FindByValue(dt1.Rows[0]["Education"].ToString()));
                            ddlCoApp1Marital.SelectedIndex = ddlCoApp1Marital.Items.IndexOf(ddlCoApp1Marital.Items.FindByValue(dt1.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp1NoOfFamily.Text = Convert.ToString(dt1.Rows[0]["TotalNoFamMem"]);
                            txtCoApp1NoOfDependents.Text = Convert.ToString(dt1.Rows[0]["NoOfDependents"]);
                            ddlCoApp1Caste.SelectedIndex = ddlCoApp1Caste.Items.IndexOf(ddlCoApp1Caste.Items.FindByValue(dt1.Rows[0]["Caste"].ToString()));
                            ddlCoApp1Religion.SelectedIndex = ddlCoApp1Religion.Items.IndexOf(ddlCoApp1Religion.Items.FindByValue(dt1.Rows[0]["Religion"].ToString()));
                            ddlCoApp1Minority.SelectedIndex = ddlCoApp1Minority.Items.IndexOf(ddlCoApp1Minority.Items.FindByValue(dt1.Rows[0]["Minority"].ToString()));
                            txtCoApp1Email.Text = Convert.ToString(dt1.Rows[0]["Email"]);
                            txtCoApp1PerResiAdd.Text = Convert.ToString(dt1.Rows[0]["PerAdd"]);


                            ddlCoApp1PerState.SelectedIndex = ddlCoApp1PerState.Items.IndexOf(ddlCoApp1PerState.Items.FindByValue(dt1.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp1PerState.SelectedValue), "CoApp1Per");
                            ddlCoApp1PerDist.SelectedIndex = ddlCoApp1PerDist.Items.IndexOf(ddlCoApp1PerDist.Items.FindByValue(dt1.Rows[0]["PerDist"].ToString()));

                            txtCoApp1PerPin.Text = Convert.ToString(dt1.Rows[0]["PerPin"]);
                            txtCoApp1CurrAdd.Text = Convert.ToString(dt1.Rows[0]["CurrAdd"]);

                            ddlCoApp1CurrState.SelectedIndex = ddlCoApp1CurrState.Items.IndexOf(ddlCoApp1CurrState.Items.FindByValue(dt1.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp1CurrState.SelectedValue), "CoApp1Curr");
                            ddlCoApp1CurrDist.SelectedIndex = ddlCoApp1CurrDist.Items.IndexOf(ddlCoApp1CurrDist.Items.FindByValue(dt1.Rows[0]["CurrDist"].ToString()));

                            txtCoApp1CurrPin.Text = Convert.ToString(dt1.Rows[0]["CurrPin"]);
                            txtCoApp1Landmark.Text = Convert.ToString(dt1.Rows[0]["CurrLandmark"]);
                            ddlCoApp1OwnShip.SelectedIndex = ddlCoApp1OwnShip.Items.IndexOf(ddlCoApp1OwnShip.Items.FindByValue(dt1.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp1ResiStabYrs.SelectedIndex = ddlCoApp1ResiStabYrs.Items.IndexOf(ddlCoApp1ResiStabYrs.Items.FindByValue(dt1.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp1PanVerifyYN.Value = Convert.ToString(dt1.Rows[0]["PanVerifyYN"]);
                            hdnCoApp1AadhVerifyYN.Value = Convert.ToString(dt1.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp1VoterVerifyYN.Value = Convert.ToString(dt1.Rows[0]["VoterVerifyYN"]);
                            hdnCoApp1OwnShipExt.Value = Convert.ToString(dt1.Rows[0]["OwnShipExt"]);
                            lbloApp1OwnShip.Text = "CA1OwnshipProof" + hdnCoApp1OwnShipExt.Value;

                            hdPanFileNmApp1.Value = Convert.ToString(dt1.Rows[0]["PanFileNm"]);
                            hdAdharFileNmApp1.Value = Convert.ToString(dt1.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmApp1.Value = Convert.ToString(dt1.Rows[0]["VoterFileNm"]);
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp2CustId.Value = Convert.ToString(dt2.Rows[0]["CoAppID"]);
                            txtCoApp2CustId.Text = Convert.ToString(dt2.Rows[0]["CoAppNo"]);
                            txtCoApp2CustName.Text = Convert.ToString(dt2.Rows[0]["CoAppName"]);
                            ddlCoApp2Gender.SelectedIndex = ddlCoApp2Gender.Items.IndexOf(ddlCoApp2Gender.Items.FindByValue(dt2.Rows[0]["Gender"].ToString()));
                            txtCoApp2Dob.Text = Convert.ToString(dt2.Rows[0]["DoB"]);
                            txtCoApp2Age.Text = Convert.ToString(dt2.Rows[0]["PresentAge"]);
                            txtCoApp2AgeAtLoanMaturity.Text = Convert.ToString(dt2.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp2Pan.Text = Convert.ToString(dt2.Rows[0]["PanNo"]);
                            txtCoApp2AadhRefNo.Text = Convert.ToString(dt2.Rows[0]["AadhRefNo"]);
                            txtCoApp2AadhNo.Text = Convert.ToString(dt2.Rows[0]["AadhMaskedNo"]);
                            txtCoApp2Voter.Text = Convert.ToString(dt2.Rows[0]["VoterID"]);
                            ddlCoApp2RelWithApp.SelectedIndex = ddlCoApp2RelWithApp.Items.IndexOf(ddlCoApp2RelWithApp.Items.FindByValue(dt2.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp2Mob.Text = Convert.ToString(dt2.Rows[0]["MobNo"]);
                            ddlCoApp2Edu.SelectedIndex = ddlCoApp2Edu.Items.IndexOf(ddlCoApp2Edu.Items.FindByValue(dt2.Rows[0]["Education"].ToString()));
                            ddlCoApp2Marital.SelectedIndex = ddlCoApp2Marital.Items.IndexOf(ddlCoApp2Marital.Items.FindByValue(dt2.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp2NoOfFamily.Text = Convert.ToString(dt2.Rows[0]["TotalNoFamMem"]);
                            txtCoApp2NoOfDependents.Text = Convert.ToString(dt2.Rows[0]["NoOfDependents"]);
                            ddlCoApp2Caste.SelectedIndex = ddlCoApp2Caste.Items.IndexOf(ddlCoApp2Caste.Items.FindByValue(dt2.Rows[0]["Caste"].ToString()));
                            ddlCoApp2Religion.SelectedIndex = ddlCoApp2Religion.Items.IndexOf(ddlCoApp2Religion.Items.FindByValue(dt2.Rows[0]["Religion"].ToString()));
                            ddlCoApp2Minority.SelectedIndex = ddlCoApp2Minority.Items.IndexOf(ddlCoApp2Minority.Items.FindByValue(dt2.Rows[0]["Minority"].ToString()));
                            txtCoApp2Email.Text = Convert.ToString(dt2.Rows[0]["Email"]);
                            txtCoApp2PerResiAdd.Text = Convert.ToString(dt2.Rows[0]["PerAdd"]);


                            ddlCoApp2PerState.SelectedIndex = ddlCoApp2PerState.Items.IndexOf(ddlCoApp2PerState.Items.FindByValue(dt2.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp2PerState.SelectedValue), "CoApp2Per");
                            ddlCoApp2PerDist.SelectedIndex = ddlCoApp2PerDist.Items.IndexOf(ddlCoApp2PerDist.Items.FindByValue(dt2.Rows[0]["PerDist"].ToString()));

                            txtCoApp2PerPin.Text = Convert.ToString(dt2.Rows[0]["PerPin"]);
                            txtCoApp2CurrResiAdd.Text = Convert.ToString(dt2.Rows[0]["CurrAdd"]);

                            ddlCoApp2CurrState.SelectedIndex = ddlCoApp2CurrState.Items.IndexOf(ddlCoApp2CurrState.Items.FindByValue(dt2.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp2CurrState.SelectedValue), "CoApp2Curr");
                            ddlCoApp2CurrDist.SelectedIndex = ddlCoApp2CurrDist.Items.IndexOf(ddlCoApp2CurrDist.Items.FindByValue(dt2.Rows[0]["CurrDist"].ToString()));

                            txtCoApp2CurrPin.Text = Convert.ToString(dt2.Rows[0]["CurrPin"]);
                            txtCoApp2Landmark.Text = Convert.ToString(dt2.Rows[0]["CurrLandmark"]);
                            ddlCoApp2Ownship.SelectedIndex = ddlCoApp2Ownship.Items.IndexOf(ddlCoApp2Ownship.Items.FindByValue(dt2.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp2ResiStabYrs.SelectedIndex = ddlCoApp2ResiStabYrs.Items.IndexOf(ddlCoApp2ResiStabYrs.Items.FindByValue(dt2.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp2PanVerifyYN.Value = Convert.ToString(dt2.Rows[0]["PanVerifyYN"]);
                            hdnCoApp2AadhVerifyYN.Value = Convert.ToString(dt2.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp2VoterVerifyYN.Value = Convert.ToString(dt2.Rows[0]["VoterVerifyYN"]);
                            hdnCoApp2OwnShipExt.Value = Convert.ToString(dt2.Rows[0]["OwnShipExt"]);
                            lblCoApp2OwnShip.Text = "CA2OwnshipProof" + hdnCoApp2OwnShipExt.Value;

                            hdPanFileNmApp2.Value = Convert.ToString(dt2.Rows[0]["PanFileNm"]);
                            hdAdharFileNmApp2.Value = Convert.ToString(dt2.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmApp2.Value = Convert.ToString(dt2.Rows[0]["VoterFileNm"]);
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnGuarId.Value = Convert.ToString(dt3.Rows[0]["CoAppID"]);
                            txtGuarId.Text = Convert.ToString(dt3.Rows[0]["CoAppNo"]);
                            txtGuarName.Text = Convert.ToString(dt3.Rows[0]["CoAppName"]);
                            ddlGuarGender.SelectedIndex = ddlGuarGender.Items.IndexOf(ddlGuarGender.Items.FindByValue(dt3.Rows[0]["Gender"].ToString()));
                            txtGuarDOB.Text = Convert.ToString(dt3.Rows[0]["DoB"]);
                            txtGuarAge.Text = Convert.ToString(dt3.Rows[0]["PresentAge"]);
                            txtGuarAgeAtLoanMaturity.Text = Convert.ToString(dt3.Rows[0]["AgeAtLoanMaturity"]);
                            txtGuarPan.Text = Convert.ToString(dt3.Rows[0]["PanNo"]);
                            txtGuarAadhRefNo.Text = Convert.ToString(dt3.Rows[0]["AadhRefNo"]);
                            txtGuarAadhNo.Text = Convert.ToString(dt3.Rows[0]["AadhMaskedNo"]);
                            txtGuarVoter.Text = Convert.ToString(dt3.Rows[0]["VoterID"]);
                            ddlGuarRelWithApp.SelectedIndex = ddlGuarRelWithApp.Items.IndexOf(ddlGuarRelWithApp.Items.FindByValue(dt3.Rows[0]["RelWithApp"].ToString()));
                            txtGuarMob.Text = Convert.ToString(dt3.Rows[0]["MobNo"]);
                            ddlGuarEdu.SelectedIndex = ddlGuarEdu.Items.IndexOf(ddlGuarEdu.Items.FindByValue(dt3.Rows[0]["Education"].ToString()));
                            ddlGuarMarital.SelectedIndex = ddlGuarMarital.Items.IndexOf(ddlGuarMarital.Items.FindByValue(dt3.Rows[0]["MaritalStatus"].ToString()));
                            txtGuarNoOfFamily.Text = Convert.ToString(dt3.Rows[0]["TotalNoFamMem"]);
                            txtGuarNoOfDependents.Text = Convert.ToString(dt3.Rows[0]["NoOfDependents"]);
                            ddlGuarCaste.SelectedIndex = ddlGuarCaste.Items.IndexOf(ddlGuarCaste.Items.FindByValue(dt3.Rows[0]["Caste"].ToString()));
                            ddlGuarReligion.SelectedIndex = ddlGuarReligion.Items.IndexOf(ddlGuarReligion.Items.FindByValue(dt3.Rows[0]["Religion"].ToString()));
                            ddlGuarMinority.SelectedIndex = ddlGuarMinority.Items.IndexOf(ddlGuarMinority.Items.FindByValue(dt3.Rows[0]["Minority"].ToString()));
                            txtGuarEmail.Text = Convert.ToString(dt3.Rows[0]["Email"]);
                            txtGuarPerResiAdd.Text = Convert.ToString(dt3.Rows[0]["PerAdd"]);

                            ddlGuarPerState.SelectedIndex = ddlGuarPerState.Items.IndexOf(ddlGuarPerState.Items.FindByValue(dt3.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlGuarPerState.SelectedValue), "GuarPer");
                            ddlGuarPerDist.SelectedIndex = ddlGuarPerDist.Items.IndexOf(ddlGuarPerDist.Items.FindByValue(dt3.Rows[0]["PerDist"].ToString()));

                            txtGuarPerPin.Text = Convert.ToString(dt3.Rows[0]["PerPin"]);
                            txtGuarCurrResiAdd.Text = Convert.ToString(dt3.Rows[0]["CurrAdd"]);

                            ddlGuarCurrState.SelectedIndex = ddlGuarCurrState.Items.IndexOf(ddlGuarCurrState.Items.FindByValue(dt3.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlGuarCurrState.SelectedValue), "GuarCurr");
                            ddlGuarCurrDist.SelectedIndex = ddlGuarCurrDist.Items.IndexOf(ddlGuarCurrDist.Items.FindByValue(dt3.Rows[0]["CurrDist"].ToString()));

                            txtGuarCurrPin.Text = Convert.ToString(dt3.Rows[0]["CurrPin"]);
                            txtGuarLandmark.Text = Convert.ToString(dt3.Rows[0]["CurrLandmark"]);
                            ddlGuarOwnStat.SelectedIndex = ddlGuarOwnStat.Items.IndexOf(ddlGuarOwnStat.Items.FindByValue(dt3.Rows[0]["OwnShipStatus"].ToString()));
                            ddlGuarResiStabYrs.SelectedIndex = ddlGuarResiStabYrs.Items.IndexOf(ddlGuarResiStabYrs.Items.FindByValue(dt3.Rows[0]["ResiStabYrs"].ToString()));

                            hdnGuarPanVerifyYN.Value = Convert.ToString(dt3.Rows[0]["PanVerifyYN"]);
                            hdnGuarAadhVerifyYN.Value = Convert.ToString(dt3.Rows[0]["AadhVerifyYN"]);
                            hdnGuarVoterVerifyYN.Value = Convert.ToString(dt3.Rows[0]["VoterVerifyYN"]);
                            hdnGuarOwnShipExt.Value = Convert.ToString(dt3.Rows[0]["OwnShipExt"]);
                            lblGuarOwnShip.Text = "GOwnshipProof" + hdnGuarOwnShipExt.Value;

                            hdPanFileNmGur.Value = Convert.ToString(dt3.Rows[0]["PanFileNm"]);
                            hdAdharFileNmGur.Value = Convert.ToString(dt3.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmGur.Value = Convert.ToString(dt3.Rows[0]["VoterFileNm"]);

                        }
                        if (dt4.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp3CustId.Value = Convert.ToString(dt4.Rows[0]["CoAppID"]);
                            txtCoApp3CustId.Text = Convert.ToString(dt4.Rows[0]["CoAppNo"]);
                            txtCoApp3CustName.Text = Convert.ToString(dt4.Rows[0]["CoAppName"]);
                            ddlCoApp3Gender.SelectedIndex = ddlCoApp3Gender.Items.IndexOf(ddlCoApp3Gender.Items.FindByValue(dt4.Rows[0]["Gender"].ToString()));
                            txtCoApp3Dob.Text = Convert.ToString(dt4.Rows[0]["DoB"]);
                            txtCoApp3Age.Text = Convert.ToString(dt4.Rows[0]["PresentAge"]);
                            txtCoApp3AgeAtLoanMaturity.Text = Convert.ToString(dt4.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp3Pan.Text = Convert.ToString(dt4.Rows[0]["PanNo"]);
                            txtCoApp3AadhRefNo.Text = Convert.ToString(dt4.Rows[0]["AadhRefNo"]);
                            txtCoApp3AadhNo.Text = Convert.ToString(dt4.Rows[0]["AadhMaskedNo"]);
                            txtCoApp3Voter.Text = Convert.ToString(dt4.Rows[0]["VoterID"]);
                            ddlCoApp3RelWithApp.SelectedIndex = ddlCoApp3RelWithApp.Items.IndexOf(ddlCoApp3RelWithApp.Items.FindByValue(dt4.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp3Mob.Text = Convert.ToString(dt4.Rows[0]["MobNo"]);
                            ddlCoApp3Edu.SelectedIndex = ddlCoApp3Edu.Items.IndexOf(ddlCoApp3Edu.Items.FindByValue(dt4.Rows[0]["Education"].ToString()));
                            ddlCoApp3Marital.SelectedIndex = ddlCoApp3Marital.Items.IndexOf(ddlCoApp3Marital.Items.FindByValue(dt4.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp3NoOfFamily.Text = Convert.ToString(dt4.Rows[0]["TotalNoFamMem"]);
                            txtCoApp3NoOfDependents.Text = Convert.ToString(dt4.Rows[0]["NoOfDependents"]);
                            ddlCoApp3Caste.SelectedIndex = ddlCoApp3Caste.Items.IndexOf(ddlCoApp3Caste.Items.FindByValue(dt4.Rows[0]["Caste"].ToString()));
                            ddlCoApp3Religion.SelectedIndex = ddlCoApp3Religion.Items.IndexOf(ddlCoApp3Religion.Items.FindByValue(dt4.Rows[0]["Religion"].ToString()));
                            ddlCoApp3Minority.SelectedIndex = ddlCoApp3Minority.Items.IndexOf(ddlCoApp3Minority.Items.FindByValue(dt4.Rows[0]["Minority"].ToString()));
                            txtCoApp3Email.Text = Convert.ToString(dt4.Rows[0]["Email"]);
                            txtCoApp3PerResiAdd.Text = Convert.ToString(dt4.Rows[0]["PerAdd"]);


                            ddlCoApp3PerState.SelectedIndex = ddlCoApp3PerState.Items.IndexOf(ddlCoApp3PerState.Items.FindByValue(dt4.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp3PerState.SelectedValue), "CoApp3Per");
                            ddlCoApp3PerDist.SelectedIndex = ddlCoApp3PerDist.Items.IndexOf(ddlCoApp3PerDist.Items.FindByValue(dt4.Rows[0]["PerDist"].ToString()));

                            txtCoApp3PerPin.Text = Convert.ToString(dt4.Rows[0]["PerPin"]);
                            txtCoApp3CurrResiAdd.Text = Convert.ToString(dt4.Rows[0]["CurrAdd"]);

                            ddlCoApp3CurrState.SelectedIndex = ddlCoApp3CurrState.Items.IndexOf(ddlCoApp3CurrState.Items.FindByValue(dt4.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp3CurrState.SelectedValue), "CoApp3Curr");
                            ddlCoApp3CurrDist.SelectedIndex = ddlCoApp3CurrDist.Items.IndexOf(ddlCoApp3CurrDist.Items.FindByValue(dt4.Rows[0]["CurrDist"].ToString()));

                            txtCoApp3CurrPin.Text = Convert.ToString(dt4.Rows[0]["CurrPin"]);
                            txtCoApp3Landmark.Text = Convert.ToString(dt4.Rows[0]["CurrLandmark"]);
                            ddlCoApp3Ownship.SelectedIndex = ddlCoApp3Ownship.Items.IndexOf(ddlCoApp3Ownship.Items.FindByValue(dt4.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp3ResiStabYrs.SelectedIndex = ddlCoApp3ResiStabYrs.Items.IndexOf(ddlCoApp3ResiStabYrs.Items.FindByValue(dt4.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp3PanVerifyYN.Value = Convert.ToString(dt4.Rows[0]["PanVerifyYN"]);
                            hdnCoApp3AadhVerifyYN.Value = Convert.ToString(dt4.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp3VoterVerifyYN.Value = Convert.ToString(dt4.Rows[0]["VoterVerifyYN"]);

                            StatusButton("Show");

                            hdnCoApp3OwnShipExt.Value = Convert.ToString(dt4.Rows[0]["OwnShipExt"]);
                            lblCoApp3OwnShip.Text = "CA3OwnshipProof" + hdnCoApp3OwnShipExt.Value;

                            hdPanFileNmApp3.Value = Convert.ToString(dt4.Rows[0]["PanFileNm"]);
                            hdAdharFileNmApp3.Value = Convert.ToString(dt4.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmApp3.Value = Convert.ToString(dt4.Rows[0]["VoterFileNm"]);
                        }
                        if (dt5.Rows.Count > 0)
                        {
                            ddlCustomer.SelectedIndex = ddlCustomer.Items.IndexOf(ddlCustomer.Items.FindByValue(dt.Rows[0]["LeadID"].ToString()));
                            hdnCoApp4CustId.Value = Convert.ToString(dt5.Rows[0]["CoAppID"]);
                            txtCoApp4CustId.Text = Convert.ToString(dt5.Rows[0]["CoAppNo"]);
                            txtCoApp4CustName.Text = Convert.ToString(dt5.Rows[0]["CoAppName"]);
                            ddlCoApp4Gender.SelectedIndex = ddlCoApp4Gender.Items.IndexOf(ddlCoApp4Gender.Items.FindByValue(dt5.Rows[0]["Gender"].ToString()));
                            txtCoApp4Dob.Text = Convert.ToString(dt5.Rows[0]["DoB"]);
                            txtCoApp4Age.Text = Convert.ToString(dt5.Rows[0]["PresentAge"]);
                            txtCoApp4AgeAtLoanMaturity.Text = Convert.ToString(dt5.Rows[0]["AgeAtLoanMaturity"]);
                            txtCoApp4Pan.Text = Convert.ToString(dt5.Rows[0]["PanNo"]);
                            txtCoApp4AadhRefNo.Text = Convert.ToString(dt5.Rows[0]["AadhRefNo"]);
                            txtCoApp4AadhNo.Text = Convert.ToString(dt5.Rows[0]["AadhMaskedNo"]);
                            txtCoApp4Voter.Text = Convert.ToString(dt5.Rows[0]["VoterID"]);
                            ddlCoApp4RelWithApp.SelectedIndex = ddlCoApp4RelWithApp.Items.IndexOf(ddlCoApp4RelWithApp.Items.FindByValue(dt5.Rows[0]["RelWithApp"].ToString()));
                            txtCoApp4Mob.Text = Convert.ToString(dt5.Rows[0]["MobNo"]);
                            ddlCoApp4Edu.SelectedIndex = ddlCoApp4Edu.Items.IndexOf(ddlCoApp4Edu.Items.FindByValue(dt5.Rows[0]["Education"].ToString()));
                            ddlCoApp4Marital.SelectedIndex = ddlCoApp4Marital.Items.IndexOf(ddlCoApp4Marital.Items.FindByValue(dt5.Rows[0]["MaritalStatus"].ToString()));
                            txtCoApp4NoOfFamily.Text = Convert.ToString(dt5.Rows[0]["TotalNoFamMem"]);
                            txtCoApp4NoOfDependents.Text = Convert.ToString(dt5.Rows[0]["NoOfDependents"]);
                            ddlCoApp4Caste.SelectedIndex = ddlCoApp4Caste.Items.IndexOf(ddlCoApp4Caste.Items.FindByValue(dt5.Rows[0]["Caste"].ToString()));
                            ddlCoApp4Religion.SelectedIndex = ddlCoApp4Religion.Items.IndexOf(ddlCoApp4Religion.Items.FindByValue(dt5.Rows[0]["Religion"].ToString()));
                            ddlCoApp4Minority.SelectedIndex = ddlCoApp4Minority.Items.IndexOf(ddlCoApp4Minority.Items.FindByValue(dt5.Rows[0]["Minority"].ToString()));
                            txtCoApp4Email.Text = Convert.ToString(dt5.Rows[0]["Email"]);
                            txtCoApp4PerResiAdd.Text = Convert.ToString(dt5.Rows[0]["PerAdd"]);


                            ddlCoApp4PerState.SelectedIndex = ddlCoApp4PerState.Items.IndexOf(ddlCoApp4PerState.Items.FindByValue(dt5.Rows[0]["PerState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp4PerState.SelectedValue), "CoApp4Per");
                            ddlCoApp4PerDist.SelectedIndex = ddlCoApp4PerDist.Items.IndexOf(ddlCoApp4PerDist.Items.FindByValue(dt5.Rows[0]["PerDist"].ToString()));

                            txtCoApp4PerPin.Text = Convert.ToString(dt5.Rows[0]["PerPin"]);
                            txtCoApp4CurrResiAdd.Text = Convert.ToString(dt5.Rows[0]["CurrAdd"]);

                            ddlCoApp4CurrState.SelectedIndex = ddlCoApp4CurrState.Items.IndexOf(ddlCoApp4CurrState.Items.FindByValue(dt5.Rows[0]["CurrState"].ToString()));
                            PopDistrictByState(Convert.ToInt32(ddlCoApp4CurrState.SelectedValue), "CoApp4Curr");
                            ddlCoApp4CurrDist.SelectedIndex = ddlCoApp4CurrDist.Items.IndexOf(ddlCoApp4CurrDist.Items.FindByValue(dt5.Rows[0]["CurrDist"].ToString()));

                            txtCoApp4CurrPin.Text = Convert.ToString(dt5.Rows[0]["CurrPin"]);
                            txtCoApp4Landmark.Text = Convert.ToString(dt5.Rows[0]["CurrLandmark"]);
                            ddlCoApp4Ownship.SelectedIndex = ddlCoApp4Ownship.Items.IndexOf(ddlCoApp4Ownship.Items.FindByValue(dt5.Rows[0]["OwnShipStatus"].ToString()));
                            ddlCoApp4ResiStabYrs.SelectedIndex = ddlCoApp4ResiStabYrs.Items.IndexOf(ddlCoApp4ResiStabYrs.Items.FindByValue(dt5.Rows[0]["ResiStabYrs"].ToString()));

                            hdnCoApp4PanVerifyYN.Value = Convert.ToString(dt5.Rows[0]["PanVerifyYN"]);
                            hdnCoApp4AadhVerifyYN.Value = Convert.ToString(dt5.Rows[0]["AadhVerifyYN"]);
                            hdnCoApp4VoterVerifyYN.Value = Convert.ToString(dt5.Rows[0]["VoterVerifyYN"]);

                            StatusButton("Show");

                            hdnCoApp4OwnShipExt.Value = Convert.ToString(dt5.Rows[0]["OwnShipExt"]);
                            lblCoApp4OwnShip.Text = "CA4OwnshipProof" + hdnCoApp4OwnShipExt.Value;

                            hdPanFileNmApp4.Value = Convert.ToString(dt5.Rows[0]["PanFileNm"]);
                            hdAdharFileNmApp4.Value = Convert.ToString(dt5.Rows[0]["AdharFileNm"]);
                            hdVoterFileNmApp4.Value = Convert.ToString(dt5.Rows[0]["VoterFileNm"]);
                        }
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                        dt4 = null;
                        dt5 = null;
                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 2:
                    #region Banking
                    try
                    {
                        if (ViewState["AppNm"] != null)
                        {
                            lblBnkApplNm.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblBnkApplNm.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBnkBcProp.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBnkBcProp.Text = "";
                        }
                        txtACNo.Text = "";
                        hdACNo.Value = "";
                        GenerateBankingGrid(Convert.ToInt64(hdLeadId.Value));
                        popGrid(Convert.ToInt64(hdLeadId.Value), "");
                        DisableControl(false, this.Page);
                        btnSaveChq.Enabled = false;
                        btnCancelChq.Enabled = false;
                        btnEditChq.Enabled = false;
                        btnEditB.Enabled = false;
                        ImgChqNo.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                   trSendBk.Visible = true;
                    break;
                case 3:
                    #region Emp/Busdtls
                    try
                    {
                        hdFlag.Value = "N";
                        if (Convert.ToInt64(hdAssMtdId.Value) == 1) // 1 for Salaried which will be used for Employment and the others is for Business
                        {
                            if (ViewState["BcProNo"] != null)
                            {

                                lblBasicName.Text = ViewState["BcProNo"].ToString();
                            }
                            else
                            {
                                lblBasicName.Text = "";
                            }

                            if (ViewState["AppNm"] != null)
                            {
                                lblApplNmED.Text = ViewState["AppNm"].ToString();
                            }
                            else
                            {
                                lblApplNmED.Text = "";
                            }
                        }
                        else
                        {
                            if (ViewState["BcProNo"] != null)
                            {
                                lblBasicNamebus.Text = ViewState["BcProNo"].ToString();
                            }
                            else
                            {
                                lblBasicNamebus.Text = "";
                            }
                            if (ViewState["AppNm"] != null)
                            {
                                lblApplNmBN.Text = ViewState["AppNm"].ToString();

                            }
                            else
                            {
                                lblApplNmBN.Text = "";
                            }
                        }
                        GenerateEmpBusDtls();
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    #region IncomeDetails
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNo.Text = "";
                        }

                        if (ViewState["AppNm"] != null)
                        {
                            lblAppName.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblAppName.Text = "";
                        }


                        GenerateIncomeDetails();
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 4:
                    #region SolarPower
                    try
                    {
                        hdFlag.Value = "N";
                        hdnLoanFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNm.Text = ViewState["AppNm"].ToString();

                        }
                        else
                        {
                            lblApplNm.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNum.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNum.Text = "";
                        }

                        GenerateSolarSystemGrid(Convert.ToInt64(hdLeadId.Value));
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 5:
                    #region LoanApplication
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["BcProNo"] != null)
                        {
                            lblProposalNo.Text = ViewState["BcProNo"].ToString();
                        }
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplicantNm.Text = ViewState["AppNm"].ToString();
                        }

                        ds = oDist.CF_GenerateFinalDecision(Convert.ToInt64(hdLeadId.Value));
                        dt = ds.Tables[0];
                        dt1 = ds.Tables[1];
                        dt2 = ds.Tables[2];
                        dt3 = ds.Tables[3];

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
                            txtActionF.Text = dt1.Rows[0]["Action"].ToString();
                            txtHoldRemarksF.Text = dt1.Rows[0]["Remarks"].ToString();
                            lblFileNameF.Text = dt1.Rows[0]["FileName"].ToString();
                            hdnFileNameF.Value = dt1.Rows[0]["FileName"].ToString();

                            txtSacRemarks.Text = dt1.Rows[0]["SantionRemarks"].ToString();
                            txtSacDt.Text = dt1.Rows[0]["SantionDate"].ToString();
                            txtSacExDt.Text = Convert.ToString(dt1.Rows[0]["SantionExDate"]);

                            txtBcBmStatus.Text = hdBcBmStatus.Value;
                            txtOps1Status.Text = hdStg1Status.Value;
                            txtOps2Status.Text = hdStg2Status.Value;


                            if (dt1.Rows[0]["SactionComm"].ToString() == "")
                            {
                                ListItem oli1 = new ListItem("<--Select-->", "-1");
                                ddlSacComm.Items.Insert(0, oli1);
                            }
                            else
                            {
                                ddlSacComm.SelectedValue = dt1.Rows[0]["SactionComm"].ToString();
                            }
                            if (dt1.Rows[0]["RecommendtRemarks"].ToString() == "")
                            {
                                ListItem oli = new ListItem("<--Select-->", "-1");
                                ddlRecommRemarks.Items.Insert(0, oli);
                            }
                            else
                            {
                                ddlRecommRemarks.SelectedValue = dt1.Rows[0]["RecommendtRemarks"].ToString();
                            }

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
                            txtSacRemarks.Text = "";
                            txtSacDt.Text = "";
                            ddlSacComm.SelectedValue = "-1";
                            ddlRecommRemarks.SelectedValue = "-1";

                        }
                        if (dt3.Rows.Count > 0)
                        {
                            lblCAMFileName.Text = dt3.Rows[0]["CamFileName"].ToString();
                            hdnCAMFileName.Value = dt3.Rows[0]["CamFileName"].ToString();
                        }
                        CalculationF();


                        //  tbBasicDet.ActiveTabIndex = 1;
                        //  StatusButton("Show");
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
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 6:
                    #region Documents
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmDU.Text = ViewState["AppNm"].ToString();

                        }
                        else
                        {
                            lblApplNmDU.Text = "";
                        }

                        if (ViewState["BcProNo"] != null)
                        {
                            txtBCProposalNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            txtBCProposalNo.Text = "";
                        }

                        dt = oDist.CF_GetUpldDocDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                        if (dt.Rows.Count > 0)
                        {
                            txtBCProposalNo.Text = Convert.ToString(dt.Rows[0]["BCPropNo"]);
                            ShowEmpRecordsDocUpld();
                        }
                        else
                        {
                            ShowEmpRecordsDocUpld();

                        }
                        DisableControl(false, this.Page);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ds = null;
                        dt = null;
                        dt1 = null;
                        dt2 = null;
                        dt3 = null;
                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 7:
                    #region SactionComplience
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmS.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmS.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPNmS.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPNmS.Text = "";
                        }
                        GenerateOPSSactionGrid(Convert.ToInt64(hdLeadId.Value));

                        btnEditS.Enabled = true;
                        gvBOEDtl.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 8:
                    #region DisbursmentDocumentList
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblApplNmD.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblApplNmD.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblBCPropNoD.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblBCPropNoD.Text = "";
                        }
                        GenerateDisbursmentList(Convert.ToInt64(hdLeadId.Value));

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 9:
                    #region HypothecationDtls
                    try
                    {
                        hdFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblHypoApplNm.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblHypoApplNm.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblHypoBCNo.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblHypoBCNo.Text = "";
                        }
                        GenerateHypothecationDtls(Convert.ToInt64(hdLeadId.Value));
                        btnHypoEdit.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                    #endregion
                    trSendBk.Visible = true;
                    break;
                case 10:
                    #region LoanDetails
                    try
                    {
                        hdFlag.Value = "N";
                        hdnLoanFlag.Value = "N";
                        if (ViewState["AppNm"] != null)
                        {
                            lblLoanAppNm.Text = ViewState["AppNm"].ToString();
                        }
                        else
                        {
                            lblLoanAppNm.Text = "";
                        }
                        if (ViewState["BcProNo"] != null)
                        {
                            lblLoanBcP.Text = ViewState["BcProNo"].ToString();
                        }
                        else
                        {
                            lblLoanBcP.Text = "";
                        }
                        GenerateLoanAppDtls(Convert.ToInt64(hdLeadId.Value));
                        //btnHypoEdit.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                    #endregion
                    trSendBk.Visible = false;
                    btnEnableDisable();
                    break;                   
            }
            
        }

        
        protected void btnEnableDisable()
        {
            //hdBcBmStatus.Value
            //hdStg1Status.Value
            //hdStg2Status.Value

            if (hdStg2Status.Value == "Reject")
            {               
                btnLoanCancel.Enabled = false;
                btnLoanSave.Enabled = false;
                btnSendBack.Enabled = false;
                txtSendBack.Enabled = false;
            }
            else if (hdStg2Status.Value == "Approve")
            {
                btnLoanCancel.Enabled = false;
                btnLoanSave.Enabled = false;
                btnSendBack.Enabled = false;
                txtSendBack.Enabled = false;
            }
            else if (hdStg2Status.Value == "Pending")
            {               
                btnLoanCancel.Enabled = true;
                btnLoanSave.Enabled = true;
                btnSendBack.Enabled = true;
                txtSendBack.Enabled = true;
            }
            ImgChqNo.Enabled = true;
        }

        #region SolarPower
        private void GenerateSolarSystemGrid(Int64 LeadID)
        {
            ClearControls(this.Page);
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
                ds = ODis.CF_GenerateSolarSystemGrid(LeadID);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtAddress.Text = Convert.ToString(dt.Rows[0]["PropAddress"]);
                    ddlPropOwnership.SelectedIndex = ddlPropOwnership.Items.IndexOf(ddlPropOwnership.Items.FindByValue(dt.Rows[0]["PropOwnership"].ToString()));
                    txtPropOwnerName.Text = Convert.ToString(dt.Rows[0]["PropOwnerName"]);

                    ViewState["SolarId"] = Convert.ToString(dt.Rows[0]["SolPwrSysId"]);
                    ViewState["SolarType"] = Convert.ToString(dt.Rows[0]["SolarType"]);

                }
                DataRow dF;
                //dF = dt.NewRow();
                //dt.Rows.Add(dF);
                //dt.AcceptChanges();
                ViewState["Solar"] = dt;
                gvSolarDtl.DataSource = dt;
                gvSolarDtl.DataBind();
                CalculateTotalSolarCost();
                // tbBankDtl.ActiveTabIndex = 0;
            }

            finally
            {
            }
        }

        private void CalculateTotalSolarCost()
        {
            decimal totalCost = 0, Cal = 0, totalECost = 0;
            decimal vTCost = 0, vACost = 0;
            foreach (GridViewRow gr in gvSolarDtl.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtTotalCost = (TextBox)gr.FindControl("txtTotalCost");
                    TextBox txtAllowedLTV = (TextBox)gr.FindControl("txtAllowedLTV");
                    Label lblEligibleCost = (Label)gr.FindControl("lblEligibleCost");

                    decimal rowCost = 0;
                    decimal.TryParse(txtTotalCost.Text, out rowCost); //parse the cost from the textbox
                    totalCost += Math.Round(rowCost, 2);


                    if (txtTotalCost != null && txtAllowedLTV != null)
                    {
                        if (txtTotalCost.Text != "" && txtAllowedLTV.Text != "")
                        {
                            if (txtTotalCost.Text != "0" && txtAllowedLTV.Text != "0")
                            {
                                vTCost = Convert.ToDecimal(txtTotalCost.Text);
                                vACost = Convert.ToDecimal(txtAllowedLTV.Text);

                                Cal = (vTCost * vACost) / 100;
                                lblEligibleCost.Text = Convert.ToString(Math.Round(Cal, 2));
                            }
                            else
                            {
                                lblEligibleCost.Text = "0";
                            }
                        }
                        else
                        {
                            lblEligibleCost.Text = "0";
                        }
                    }
                    else
                    {
                        lblEligibleCost.Text = "0";
                    }
                    decimal rowECost = 0;
                    decimal.TryParse(lblEligibleCost.Text, out rowECost); //parse the cost from the textbox
                    totalECost += Math.Round(rowECost, 2);

                }


            }
            GridViewRow footerRow = gvSolarDtl.FooterRow;
            if (footerRow != null)
            {
                Label lbltotal = (Label)gvSolarDtl.FooterRow.FindControl("lbltotal");
                Label lbltotalECost = (Label)gvSolarDtl.FooterRow.FindControl("lbltotalECost");
                if (lbltotal != null)
                {
                    if (totalCost > 0)
                        lbltotal.Text = totalCost.ToString();
                    else
                        lbltotal.Text = "0";
                }
                if (lbltotalECost != null)
                {
                    if (totalCost > 0)
                        lbltotalECost.Text = totalECost.ToString();
                    else
                        lbltotalECost.Text = "0";
                }
                lbltotal.ForeColor = System.Drawing.Color.White;
                lbltotalECost.ForeColor = System.Drawing.Color.White;
            }
            UpFamily.Update();
        }
        protected void gvSolarDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Solar"];

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSolPwrSysId = (Label)e.Row.FindControl("lblSolPwrSysId");
                Label lblSolarType = (Label)e.Row.FindControl("lblSolarType");

                if (lblSolPwrSysId != null)
                {
                    if (ViewState["SolarId"] != null)
                        lblSolPwrSysId.Text = ViewState["SolarId"].ToString();
                }
                if (lblSolarType != null)
                {
                    if (ViewState["SolarType"] != null)
                        lblSolarType.Text = ViewState["SolarType"].ToString();
                }
            }

        }
        #endregion
        protected void ClearLabel()
        {
            lblApplNmBD.Text = "";
            lblBCPNumBD.Text = "";

            lblApplNm1.Text = "";
            lblBCPNum1.Text = "";

            lblApplNm2.Text = "";
            lblBCPNum2.Text = "";
            lblApplNmAppl.Text = "";
            lblBCPNumAppl.Text = "";

            lblApplNm3.Text = "";
            lblBCPNum3.Text = "";

            lblApplNmBN.Text = "";
            lblBasicNamebus.Text = "";

            lblApplNmED.Text = "";
            lblBasicName.Text = "";

            lblAppName.Text = "";
            lblBCPNo.Text = "";

            lblBnkApplNm.Text = "";
            lblBnkBcProp.Text = "";

            lblApplicantNm.Text = "";
            lblProposalNo.Text = "";

            lblApplNmDU.Text = "";
            txtBCProposalNo.Text = "";

            lblApplNmD.Text = "";
            lblBCPropNoD.Text = "";

            lblHypoApplNm.Text = "";
            lblHypoBCNo.Text = "";
        }

        private void ClearControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Text = "";
                }
                else if (c is DropDownList)
                {
                    ((DropDownList)c).SelectedIndex = -1;
                }
                else if (c is Label)
                {
                    ((Label)c).Text = "";
                }
                else if (c is HiddenField)
                {
                    ((HiddenField)c).Value = "";
                }

                // Recursively disable controls inside the containers like
                // panels or group controls
                if (c.Controls.Count > 0)
                {
                    ClearControls(c);

                }
            }



            foreach (GridViewRow row in gvBankDtl.Rows) //Banking Details
            {
                // Check if the row is a data row
                if (row.RowType == DataControlRowType.DataRow)
                {
                    // Loop through controls in the row
                    foreach (Control control in row.Controls)
                    {
                        // Clear TextBox controls
                        if (control is TextBox)
                        {
                            ((TextBox)control).Text = string.Empty;
                        }
                        // Clear DropDownList controls
                        else if (control is DropDownList)
                        {
                            ((DropDownList)control).SelectedIndex = -1; // Or set to a default value
                        }
                        // Clear CheckBox controls
                        else if (control is CheckBox)
                        {
                            ((CheckBox)control).Checked = false;
                        }
                        else if (control is Label)
                        {
                            ((Label)control).Text = "";
                        }
                        else if (control is HiddenField)
                        {
                            ((HiddenField)control).Value = "";
                        }
                    }
                }
            }

        }

        private void DisableControl(Boolean Status, Control parent)
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
                    DisableControl(Status, c);

                }

            }
            txtSearch.Enabled = true;
            btnShow.Enabled = true;
            txtFromDt.Enabled = true;
            txtToDt.Enabled = true;
            btnEdit.Enabled = true;
            btnEditB.Enabled = true;
            btnSaveB.Enabled = false;
            btnCancelB.Enabled = false;
            ImgChqNo.Enabled = false;
            lbRseMsg.Enabled = false;
            ddlStatus.Enabled = true;
            ddlAction.Enabled = true;
            txtSendBack.Enabled = true;
            btnSendBack.Enabled = true;

        }

        #endregion
        #region IncomeDetails
        private void GenerateIncomeDetails()
        {
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet ds = new DataSet();
            CCFIncDtl oDit = new CCFIncDtl();
            if (ViewState["IncomeStatus"] != null)
            {
                hdIncomeStatus.Value = ViewState["IncomeStatus"].ToString();
            }
            if (ViewState["AssMtdTypId"] != null)
            {
                hdAssMtdTypId.Value = ViewState["AssMtdTypId"].ToString();
            }
            if (ViewState["LeadId"] != null)
            {
                hdLeadId.Value = ViewState["LeadId"].ToString();
            }

            if (hdLeadId.Value == "")
            {
                hdLeadId.Value = "0";
            }
            if (hdAssMtdTypId.Value == "")
            {
                hdAssMtdTypId.Value = "0";
            }

            try
            {

                if (hdIncomeStatus.Value != "Pending")
                {
                    ddlAssMethodID.SelectedIndex = ddlAssMethodID.Items.IndexOf(ddlAssMethodID.Items.FindByValue(Convert.ToString(hdAssMtdTypId.Value)));

                    ViewState["StateEdit"] = "Edit";

                    ds = oDit.CF_GetIncomeDtlByLeadID(Convert.ToInt64(hdLeadId.Value));
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        txtProfAfterTax.Text = Convert.ToString(dt.Rows[0]["ProfAfterTax"]);
                        txtDepreciation.Text = Convert.ToString(dt.Rows[0]["Depreciation"]);
                        txtAmortization.Text = Convert.ToString(dt.Rows[0]["Amortization"]);
                        txtInterest.Text = Convert.ToString(dt.Rows[0]["Interest"]);
                        txtTaxes.Text = Convert.ToString(dt.Rows[0]["Taxes"]);
                        txtTotalIncAnnual.Text = Convert.ToString(dt.Rows[0]["TotalIncAnnual"]);
                        txtTurnover.Text = Convert.ToString(dt.Rows[0]["Turnover"]);
                        txtGrossProfitMargin.Text = Convert.ToString(dt.Rows[0]["GrossProfitMargin"]);
                        txtGrossIncAnnual.Text = Convert.ToString(dt.Rows[0]["GrossIncAnnual"]);
                        txtNoMonthsConsiBS.Text = Convert.ToString(dt.Rows[0]["NoMonthsConsiBS"]);
                        txtBankTurnMonthBS.Text = Convert.ToString(dt.Rows[0]["BankTurnMonthBS"]);
                        txtProfMargMonthBS.Text = Convert.ToString(dt.Rows[0]["ProfMargMonthBS"]);
                        txtGrossIncMonthly.Text = Convert.ToString(dt.Rows[0]["GrossIncMonthly"]);
                        txtOtherInc.Text = Convert.ToString(dt.Rows[0]["OtherInc"]);
                        txtAverElecBill.Text = Convert.ToString(dt.Rows[0]["AverElecBill"]);
                        txtTotalIncMonthly.Text = Convert.ToString(dt.Rows[0]["TotalIncMonthly"]);
                        txtApplFoirPerc.Text = Convert.ToString(dt.Rows[0]["ApplFoirPerc"]);
                        txtDeviFoirPerc.Text = Convert.ToString(dt.Rows[0]["DeviFoirPerc"]);
                        txtFinalFoirConsi.Text = Convert.ToString(dt.Rows[0]["FinalFoirConsi"]);
                        txtFoirInc.Text = Convert.ToString(dt.Rows[0]["FoirInc"]);
                        txtExistOblig.Text = Convert.ToString(dt.Rows[0]["ExistOblig"]);
                        txtNetIncToUfsbEmi.Text = Convert.ToString(dt.Rows[0]["NetIncToUfsbEmi"]);
                        txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["PerLakhEmi"]);
                        txtLoanEligiblity.Text = Convert.ToString(dt.Rows[0]["LoanEligiblity"]);
                    }

                    // tbLoanAppDtl.ActiveTabIndex = 15;

                    txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : "0";

                }
                else
                {
                    ddlAssMethodID.SelectedIndex = ddlAssMethodID.Items.IndexOf(ddlAssMethodID.Items.FindByValue(Convert.ToString(hdAssMtdTypId.Value)));

                    txtGrossProfitMargin.Text = hdnAssMthId.Value == "3" ? "80" : txtGrossProfitMargin.Text;

                    ViewState["StateEdit"] = "Add";

                    ds = oDit.CF_CalculateEMIAmount(Convert.ToInt64(hdLeadId.Value));
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        txtPerLakhEmi.Text = Convert.ToString(dt.Rows[0]["EMIAmt"]);
                    }

                }

                DynamicTableRow(Convert.ToInt32(hdAssMtdTypId.Value));
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

        public void DynamicTableRow(int AssMthId)
        {
            switch (AssMthId)
            {
                case 1:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 2:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = true;
                    tr2IncDtlMonthBS.Visible = true;

                    break;
                case 3:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 4:
                    trHIncDtlFiP.Visible = true;
                    tr1IncDtlFiP.Visible = true;
                    tr2IncDtlFiP.Visible = true;
                    tr3IncDtlFiP.Visible = true;

                    trHIncDtlCommon.Visible = false;
                    tr1IncDtlCommon.Visible = false;
                    tr2IncDtlCommon.Visible = false;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;
                    break;
                case 5:

                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
                case 6:
                    trHIncDtlFiP.Visible = false;
                    tr1IncDtlFiP.Visible = false;
                    tr2IncDtlFiP.Visible = false;
                    tr3IncDtlFiP.Visible = false;

                    trHIncDtlCommon.Visible = true;
                    tr1IncDtlCommon.Visible = true;
                    tr2IncDtlCommon.Visible = true;

                    tr1IncDtlMonthBS.Visible = false;
                    tr2IncDtlMonthBS.Visible = false;

                    break;
            }

        }
        #endregion
        #region AllCalculations

        private void CalculationF()
        {

            double vLoanAmt = 0;
            double vInsuCharge = 0;
            double vROI = Convert.ToDouble(txtROIF.Text);
            double vTenure = Convert.ToDouble(txtTenureF.Text);
            double vFOIR = Convert.ToDouble(hdFOIRF.Value);
            double EMIAmt = 0, RoundEMIAmt = 0;

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

            EMIAmt = emi_calculator((vLoanAmt + vInsuCharge), vROI, vTenure);
            RoundEMIAmt = Math.Ceiling(EMIAmt);
            txtEMIF.Text = Convert.ToString(RoundEMIAmt);

            txtTotEMIF.Text = Convert.ToString(Math.Ceiling(EMIAmt + vExistingEMI));
        }


        double emi_calculator(double p, double r, double t)
        {
            double emi;

            r = r / (12 * 100); // one month interest
            emi = (p * r * Math.Pow(1 + r, t)) / (Math.Pow(1 + r, t) - 1);

            return (Math.Round(emi, 2));
        }


        #endregion
        #region ImageView

        protected void ViewCustDoc(string ID, string FileName)
        {
            string vBase64String = "", vPdfFile = "", ActNetImage = "";
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
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
                }
                else
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
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }

        #region CustPan
        protected void btnViewAppPan_Click(object sender, EventArgs e)
        {
            if (txtApplPan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmApp.Value);
            }
        }
        protected void btnViewApp1Pan_Click(object sender, EventArgs e)
        {
            if (txtCoApp1Pan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmApp1.Value);
            }
        }
        protected void btnViewApp2Pan_Click(object sender, EventArgs e)
        {
            if (txtCoApp2Pan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmApp2.Value);
            }
        }
        protected void btnViewApp3Pan_Click(object sender, EventArgs e)
        {
            if (txtCoApp3Pan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmApp3.Value);
            }
        }
        protected void btnViewApp4Pan_Click(object sender, EventArgs e)
        {
            if (txtCoApp4Pan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmApp4.Value);
            }
        }
        protected void btnViewGurPan_Click(object sender, EventArgs e)
        {
            if (txtGuarPan.Text == "")
            {
                gblFuction.AjxMsgPopup("PAN No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdPanFileNmGur.Value);
            }
        }
        #endregion

        #region CustAadhar
        protected void btnViewAppAadhr_Click(object sender, EventArgs e)
        {
            if (txtApplAadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmApp.Value);
            }
        }
        protected void btnViewApp1Aadhr_Click(object sender, EventArgs e)
        {
            if (txtCoApp1AadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmApp1.Value);
            }
        }
        protected void btnViewApp2Aadhr_Click(object sender, EventArgs e)
        {
            if (txtCoApp2AadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmApp2.Value);
            }
        }
        protected void btnViewApp3Aadhr_Click(object sender, EventArgs e)
        {
            if (txtCoApp3AadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmApp3.Value);
            }
        }
        protected void btnViewApp4Aadhr_Click(object sender, EventArgs e)
        {
            if (txtCoApp4AadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmApp4.Value);
            }
        }
        protected void btnViewGurAadhr_Click(object sender, EventArgs e)
        {
            if (txtGuarAadhNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Aadhaar No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdAdharFileNmGur.Value);
            }
        }
        #endregion

        #region CustVoter
        protected void btnViewAppVoter_Click(object sender, EventArgs e)
        {
            if (txtApplVoterNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmApp.Value);
            }
        }
        protected void btnViewApp1Voter_Click(object sender, EventArgs e)
        {
            if (txtCoApp1Voter.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmApp1.Value);
            }
        }
        protected void btnViewApp2Voter_Click(object sender, EventArgs e)
        {
            if (txtCoApp2Voter.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmApp2.Value);
            }
        }
        protected void btnViewApp3Voter_Click(object sender, EventArgs e)
        {
            if (txtCoApp3Voter.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmApp3.Value);
            }
        }
        protected void btnViewApp4Voter_Click(object sender, EventArgs e)
        {
            if (txtCoApp4Voter.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmApp4.Value);
            }
        }
        protected void btnViewGurVoter_Click(object sender, EventArgs e)
        {
            if (txtGuarVoter.Text == "")
            {
                gblFuction.AjxMsgPopup("Voter No. is Not Available!!");
            }
            else
            {
                ViewCustDoc(hdnLeadId.Value, hdVoterFileNmGur.Value);
            }
        }
        #endregion

        protected void btnKYCDocmnt_Click(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            GridViewRow gvrow = btn.NamingContainer as GridViewRow;
            Label lbFile = (Label)gvrow.FindControl("lbFile");
            HiddenField hdnDocImage = (HiddenField)gvrow.FindControl("hdnDocImage");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            string vBase64String = "";
            //string vPdfFile = DocumentBucketURL + hdnLoanAppNo.Value + "_" + lbFile.Text;
            if (lbFile.Text != "")
            {
                if (hdnDocImage.Value != "")
                {
                    ImgDocumentImage.ImageUrl = "data:image;base64," + hdnDocImage.Value;
                }
                else if (lbFile.Text.ToLower().Contains(".pdf"))
                {
                    string ActNetImage = "", vPdfFile = "";
                    string[] ActNetPath = DocumentBucketURL.Split(',');
                    for (int j = 0; j <= ActNetPath.Length - 1; j++)
                    {
                        ActNetImage = ActNetPath[j] + hdLeadId.Value + "_" + lbFile.Text;
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
                        Response.AddHeader("Content-Disposition", "attachment;   filename=" + hdLeadId.Value + "_" + lbFile.Text);
                        Response.BinaryWrite(vDoc);
                        Response.Flush();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No File Found");

                    }
                }
                else
                {
                    vBase64String = GetBase64Image(lbFile.Text, hdLeadId.Value);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
            }
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

        protected void btnBusAdd_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBusiAddrOwnshipExt.Value);
        }

        protected void btnBKYC2_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnBKYC2Ext.Value);
        }

        protected void btnUdyamR_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdLeadId.Value, hdnUdyamRegnCertExt.Value);
        }



        #region imgDoc_Click
        protected void imgDoc_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBankStatemenFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion

        #region imgDocB_Click
        protected void imgDocB_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblBsaOutFN");

            ViewImgDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);
        }
        #endregion

        protected void ViewImgDoc(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + lblSrlNo + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(lblSrlNo + "_" + FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + lblSrlNo + "_" + FileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
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
        protected void ViewImgDoc(string ID, string FileName, string CustType, string Ext)
        {
            string vBase64String = "";
            string vPdfFile = DocumentBucketURL + ID + "_" + CustType + "_" + FileName + Ext;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
            if (Ext.Contains(".pdf"))
            {
                WebClient cln = null;
                cln = new WebClient();
                byte[] vDoc = null;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                cln = new WebClient();
                vDoc = cln.DownloadData(vPdfFile);
                Response.AddHeader("Content-Type", "Application/octet-stream");
                Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + CustType + "_" + FileName + Ext);
                Response.BinaryWrite(vDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                vBase64String = GetBase64Image(CustType + "_" + FileName + Ext, ID);
                ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
            }
        }

        protected void ImgOwnshipPhoto_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "ApplOwnship", "A", hdnAppOwnshipExt.Value);
        }

        protected void imgCoApp1OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA1", hdnCoApp1OwnShipExt.Value);
        }

        protected void imgCoApp2OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA2", hdnCoApp2OwnShipExt.Value);
        }
        protected void imgCoApp3OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA3", hdnCoApp3OwnShipExt.Value);
        }
        protected void imgCoApp4OwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "CA4", hdnCoApp4OwnShipExt.Value);
        }

        protected void imgGuarOwnProof_Click(object sender, EventArgs e)
        {
            ViewImgDoc(hdnLeadId.Value, "OwnshipProof", "G", hdnGuarOwnShipExt.Value);
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

        #endregion
        #region PopulateAllDropDown
        private void PopBranch()
        {
            DataTable dt = null;
            CUser oUsr = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                oUsr = new CUser();
                dt = oUsr.GetBranchByUser(Session[gblValue.UserName].ToString(), Convert.ToInt32(Session[gblValue.RoleId]));
                if (Convert.ToString(Session[gblValue.BrnchCode]) != "0000")
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (Convert.ToString(row["BranchCode"]) != Convert.ToString(Session[gblValue.BrnchCode]))
                        {
                            row.Delete();
                        }
                    }
                    dt.AcceptChanges();
                }
                ddlBranch.ClearSelection();
                ddlBranch.DataSource = dt;
                ddlBranch.DataTextField = "BranchName";
                ddlBranch.DataValueField = "BranchCode";
                ddlBranch.DataBind();
                if (vBrCode == "0000")
                {
                    ListItem oli = new ListItem("<--All-->", "A");
                    ddlBranch.Items.Insert(0, oli);
                }
            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopSolarSystemType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSolarPwrSystem();
                ddlSolPwrSys.ClearSelection();
                ddlSolPwrSys.DataSource = dt;
                ddlSolPwrSys.DataTextField = "SystemType";
                ddlSolPwrSys.DataValueField = "SysID";
                ddlSolPwrSys.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSolPwrSys.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAssesMethod1()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(0);

                ddlAssMethodID.DataSource = dt;
                ddlAssMethodID.DataTextField = "MethodName";
                ddlAssMethodID.DataValueField = "MethodID";
                ddlAssMethodID.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethodID.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }
        private void PopAssesMethod()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAssesMethod(Convert.ToInt32(ddlSegType.SelectedValue));
                ddlAssMethod.ClearSelection();
                ddlAssMethod.DataSource = dt;
                ddlAssMethod.DataTextField = "MethodName";
                ddlAssMethod.DataValueField = "MethodID";
                ddlAssMethod.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlAssMethod.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopSegType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetSegType();
                ddlSegType.ClearSelection();
                ddlSegType.DataSource = dt;
                ddlSegType.DataTextField = "SegType";
                ddlSegType.DataValueField = "SegID";
                ddlSegType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlSegType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopAppliEntType()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetAppliEntTypeMst();
                ddlEntityType.ClearSelection();
                ddlEntityType.DataSource = dt;
                ddlEntityType.DataTextField = "AppEntType";
                ddlEntityType.DataValueField = "AppEntID";
                ddlEntityType.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEntityType.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopEPCMst()
        {
            DataTable dt = null;
            CUser oUsr = null;
            try
            {
                oUsr = new CUser();
                dt = oUsr.CF_GetEPCMst();

                ddlEPC.ClearSelection();
                ddlEPC.DataSource = dt;
                ddlEPC.DataTextField = "EpcName";
                ddlEPC.DataValueField = "EpcID";
                ddlEPC.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlEPC.Items.Insert(0, oli);

            }
            finally
            {
                oUsr = null;
                dt = null;
            }
        }

        private void PopRO()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                oRO = new CEO();
                dt = oRO.PopRO(vBrCode, "0", "0", vLogDt, this.UserID);

                ddlBcRM.ClearSelection();
                ddlBcRM.DataSource = dt;
                ddlBcRM.DataTextField = "EoName";
                ddlBcRM.DataValueField = "Eoid";
                ddlBcRM.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBcRM.Items.Insert(0, oli);


            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }

        private void PopBCPropNo()
        {
            DataTable dt = null;
            CEO oRO = null;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ddlBCPropNo.Items.Clear();
                oRO = new CEO();
                dt = oRO.CF_GetBCPropNoByBranch(vBrCode);

                ddlBCPropNo.ClearSelection();
                ddlBCPropNo.DataSource = dt;
                ddlBCPropNo.DataTextField = "BCPropNo";
                ddlBCPropNo.DataValueField = "LeadID";
                ddlBCPropNo.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlBCPropNo.Items.Insert(0, oli);
            }
            finally
            {
                oRO = null;
                dt = null;
            }
        }
        private void popState()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetState();

                ddlBCStste.ClearSelection();
                ddlBCStste.DataSource = dt;
                ddlBCStste.DataTextField = "StateName";
                ddlBCStste.DataValueField = "StateId";
                ddlBCStste.DataBind();
                ddlBCStste.Items.Insert(0, oli1);


            }
            finally
            {
            }
        }

        private void PopLeadList()
        {
            DataTable dt = new DataTable();
            CCust360 oMem = new CCust360();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                dt = oMem.CF_GetLeadFromBasicDtl(vBrCode);

                ddlCustomer.ClearSelection();
                ddlCustomer.DataSource = dt;
                ddlCustomer.DataTextField = "BCPropNo";
                ddlCustomer.DataValueField = "LeadID";
                ddlCustomer.DataBind();

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

        private void popCaste()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetCaste();

                ddlApplCast.DataSource = dt;
                ddlApplCast.DataTextField = "Caste";
                ddlApplCast.DataValueField = "CasteId";
                ddlApplCast.DataBind();
                ddlApplCast.Items.Insert(0, oli1);

                ddlCoApp1Caste.DataSource = dt;
                ddlCoApp1Caste.DataTextField = "Caste";
                ddlCoApp1Caste.DataValueField = "CasteId";
                ddlCoApp1Caste.DataBind();
                ddlCoApp1Caste.Items.Insert(0, oli2);

                ddlCoApp2Caste.DataSource = dt;
                ddlCoApp2Caste.DataTextField = "Caste";
                ddlCoApp2Caste.DataValueField = "CasteId";
                ddlCoApp2Caste.DataBind();
                ddlCoApp2Caste.Items.Insert(0, oli3);

                ddlCoApp3Caste.DataSource = dt;
                ddlCoApp3Caste.DataTextField = "Caste";
                ddlCoApp3Caste.DataValueField = "CasteId";
                ddlCoApp3Caste.DataBind();
                ddlCoApp3Caste.Items.Insert(0, oli5);

                ddlCoApp4Caste.DataSource = dt;
                ddlCoApp4Caste.DataTextField = "Caste";
                ddlCoApp4Caste.DataValueField = "CasteId";
                ddlCoApp4Caste.DataBind();
                ddlCoApp4Caste.Items.Insert(0, oli6);

                ddlGuarCaste.DataSource = dt;
                ddlGuarCaste.DataTextField = "Caste";
                ddlGuarCaste.DataValueField = "CasteId";
                ddlGuarCaste.DataBind();
                ddlGuarCaste.Items.Insert(0, oli4);
            }
            finally
            {
            }
        }

        private void PopReligion()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetReligion();
                ddlApplReligion.DataSource = dt;
                ddlApplReligion.DataTextField = "Religion";
                ddlApplReligion.DataValueField = "ReligionId";
                ddlApplReligion.DataBind();
                ddlApplReligion.Items.Insert(0, oli1);

                ddlCoApp1Religion.DataSource = dt;
                ddlCoApp1Religion.DataTextField = "Religion";
                ddlCoApp1Religion.DataValueField = "ReligionId";
                ddlCoApp1Religion.DataBind();
                ddlCoApp1Religion.Items.Insert(0, oli2);

                ddlCoApp2Religion.DataSource = dt;
                ddlCoApp2Religion.DataTextField = "Religion";
                ddlCoApp2Religion.DataValueField = "ReligionId";
                ddlCoApp2Religion.DataBind();
                ddlCoApp2Religion.Items.Insert(0, oli3);

                ddlCoApp3Religion.DataSource = dt;
                ddlCoApp3Religion.DataTextField = "Religion";
                ddlCoApp3Religion.DataValueField = "ReligionId";
                ddlCoApp3Religion.DataBind();
                ddlCoApp3Religion.Items.Insert(0, oli5);

                ddlCoApp4Religion.DataSource = dt;
                ddlCoApp4Religion.DataTextField = "Religion";
                ddlCoApp4Religion.DataValueField = "ReligionId";
                ddlCoApp4Religion.DataBind();
                ddlCoApp4Religion.Items.Insert(0, oli6);

                ddlGuarReligion.DataSource = dt;
                ddlGuarReligion.DataTextField = "Religion";
                ddlGuarReligion.DataValueField = "ReligionId";
                ddlGuarReligion.DataBind();
                ddlGuarReligion.Items.Insert(0, oli4);
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

        private void PopQualification()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetQualification();

                ddlAppEdu.DataSource = dt;
                ddlAppEdu.DataTextField = "QualificationName";
                ddlAppEdu.DataValueField = "QualificationId";
                ddlAppEdu.DataBind();
                ddlAppEdu.Items.Insert(0, oli1);

                ddlCoApp1Edu.DataSource = dt;
                ddlCoApp1Edu.DataTextField = "QualificationName";
                ddlCoApp1Edu.DataValueField = "QualificationId";
                ddlCoApp1Edu.DataBind();
                ddlCoApp1Edu.Items.Insert(0, oli2);

                ddlCoApp2Edu.DataSource = dt;
                ddlCoApp2Edu.DataTextField = "QualificationName";
                ddlCoApp2Edu.DataValueField = "QualificationId";
                ddlCoApp2Edu.DataBind();
                ddlCoApp2Edu.Items.Insert(0, oli3);

                ddlCoApp3Edu.DataSource = dt;
                ddlCoApp3Edu.DataTextField = "QualificationName";
                ddlCoApp3Edu.DataValueField = "QualificationId";
                ddlCoApp3Edu.DataBind();
                ddlCoApp3Edu.Items.Insert(0, oli5);

                ddlCoApp4Edu.DataSource = dt;
                ddlCoApp4Edu.DataTextField = "QualificationName";
                ddlCoApp4Edu.DataValueField = "QualificationId";
                ddlCoApp4Edu.DataBind();
                ddlCoApp4Edu.Items.Insert(0, oli6);

                ddlGuarEdu.DataSource = dt;
                ddlGuarEdu.DataTextField = "QualificationName";
                ddlGuarEdu.DataValueField = "QualificationId";
                ddlGuarEdu.DataBind();
                ddlGuarEdu.Items.Insert(0, oli4);
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

        private void popStateCust()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");
                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");

                dt = oMem.CF_GetState();

                ddlApplState.ClearSelection();
                ddlApplState.DataSource = dt;
                ddlApplState.DataTextField = "StateName";
                ddlApplState.DataValueField = "StateId";
                ddlApplState.DataBind();
                ddlApplState.Items.Insert(0, oli1);

                ddlApplCurrState.ClearSelection();
                ddlApplCurrState.DataSource = dt;
                ddlApplCurrState.DataTextField = "StateName";
                ddlApplCurrState.DataValueField = "StateId";
                ddlApplCurrState.DataBind();
                ddlApplCurrState.Items.Insert(0, oli2);

                ddlCoApp1PerState.ClearSelection();
                ddlCoApp1PerState.DataSource = dt;
                ddlCoApp1PerState.DataTextField = "StateName";
                ddlCoApp1PerState.DataValueField = "StateId";
                ddlCoApp1PerState.DataBind();
                ddlCoApp1PerState.Items.Insert(0, oli3);

                ddlCoApp1CurrState.ClearSelection();
                ddlCoApp1CurrState.DataSource = dt;
                ddlCoApp1CurrState.DataTextField = "StateName";
                ddlCoApp1CurrState.DataValueField = "StateId";
                ddlCoApp1CurrState.DataBind();
                ddlCoApp1CurrState.Items.Insert(0, oli4);

                ddlCoApp2PerState.ClearSelection();
                ddlCoApp2PerState.DataSource = dt;
                ddlCoApp2PerState.DataTextField = "StateName";
                ddlCoApp2PerState.DataValueField = "StateId";
                ddlCoApp2PerState.DataBind();
                ddlCoApp2PerState.Items.Insert(0, oli5);

                ddlCoApp2CurrState.ClearSelection();
                ddlCoApp2CurrState.DataSource = dt;
                ddlCoApp2CurrState.DataTextField = "StateName";
                ddlCoApp2CurrState.DataValueField = "StateId";
                ddlCoApp2CurrState.DataBind();
                ddlCoApp2CurrState.Items.Insert(0, oli6);

                ddlGuarPerState.ClearSelection();
                ddlGuarPerState.DataSource = dt;
                ddlGuarPerState.DataTextField = "StateName";
                ddlGuarPerState.DataValueField = "StateId";
                ddlGuarPerState.DataBind();
                ddlGuarPerState.Items.Insert(0, oli7);

                ddlGuarCurrState.ClearSelection();
                ddlGuarCurrState.DataSource = dt;
                ddlGuarCurrState.DataTextField = "StateName";
                ddlGuarCurrState.DataValueField = "StateId";
                ddlGuarCurrState.DataBind();
                ddlGuarCurrState.Items.Insert(0, oli8);

                ddlCoApp3PerState.DataSource = dt;
                ddlCoApp3PerState.DataTextField = "StateName";
                ddlCoApp3PerState.DataValueField = "StateId";
                ddlCoApp3PerState.DataBind();
                ddlCoApp3PerState.Items.Insert(0, oli9);

                ddlCoApp3CurrState.DataSource = dt;
                ddlCoApp3CurrState.DataTextField = "StateName";
                ddlCoApp3CurrState.DataValueField = "StateId";
                ddlCoApp3CurrState.DataBind();
                ddlCoApp3CurrState.Items.Insert(0, oli10);

                ddlCoApp4PerState.DataSource = dt;
                ddlCoApp4PerState.DataTextField = "StateName";
                ddlCoApp4PerState.DataValueField = "StateId";
                ddlCoApp4PerState.DataBind();
                ddlCoApp4PerState.Items.Insert(0, oli11);

                ddlCoApp4CurrState.DataSource = dt;
                ddlCoApp4CurrState.DataTextField = "StateName";
                ddlCoApp4CurrState.DataValueField = "StateId";
                ddlCoApp4CurrState.DataBind();
                ddlCoApp4CurrState.Items.Insert(0, oli12);
            }
            finally
            {
            }
        }

        private void PopRelation()
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                dt = oMem.CF_GetRelation();

                ddlRelWithApp.ClearSelection();
                ddlRelWithApp.DataSource = dt;
                ddlRelWithApp.DataTextField = "Relation";
                ddlRelWithApp.DataValueField = "RelationId";
                ddlRelWithApp.DataBind();
                ddlRelWithApp.Items.Insert(0, oli1);

                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i]["Relation"].ToString() == "Self")
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }

                ddlCoApp1RelWithApp.DataSource = dt;
                ddlCoApp1RelWithApp.DataTextField = "Relation";
                ddlCoApp1RelWithApp.DataValueField = "RelationId";
                ddlCoApp1RelWithApp.DataBind();
                ddlCoApp1RelWithApp.Items.Insert(0, oli2);

                ddlCoApp2RelWithApp.DataSource = dt;
                ddlCoApp2RelWithApp.DataTextField = "Relation";
                ddlCoApp2RelWithApp.DataValueField = "RelationId";
                ddlCoApp2RelWithApp.DataBind();
                ddlCoApp2RelWithApp.Items.Insert(0, oli3);

                ddlGuarRelWithApp.DataSource = dt;
                ddlGuarRelWithApp.DataTextField = "Relation";
                ddlGuarRelWithApp.DataValueField = "RelationId";
                ddlGuarRelWithApp.DataBind();
                ddlGuarRelWithApp.Items.Insert(0, oli4);

                ddlCoApp3RelWithApp.DataSource = dt;
                ddlCoApp3RelWithApp.DataTextField = "Relation";
                ddlCoApp3RelWithApp.DataValueField = "RelationId";
                ddlCoApp3RelWithApp.DataBind();
                ddlCoApp3RelWithApp.Items.Insert(0, oli5);

                ddlCoApp4RelWithApp.DataSource = dt;
                ddlCoApp4RelWithApp.DataTextField = "Relation";
                ddlCoApp4RelWithApp.DataValueField = "RelationId";
                ddlCoApp4RelWithApp.DataBind();
                ddlCoApp4RelWithApp.Items.Insert(0, oli6);
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

        private void PopDistrictByState(int pStateId, string pTag)
        {
            CUser oMem = new CUser();
            DataTable dt = null;
            try
            {
                ListItem oli1 = new ListItem("<--Select-->", "-1");
                ListItem oli2 = new ListItem("<--Select-->", "-1");
                ListItem oli3 = new ListItem("<--Select-->", "-1");
                ListItem oli4 = new ListItem("<--Select-->", "-1");
                ListItem oli5 = new ListItem("<--Select-->", "-1");
                ListItem oli6 = new ListItem("<--Select-->", "-1");
                ListItem oli7 = new ListItem("<--Select-->", "-1");
                ListItem oli8 = new ListItem("<--Select-->", "-1");
                ListItem oli9 = new ListItem("<--Select-->", "-1");
                ListItem oli10 = new ListItem("<--Select-->", "-1");
                ListItem oli11 = new ListItem("<--Select-->", "-1");
                ListItem oli12 = new ListItem("<--Select-->", "-1");
                switch (pTag)
                {
                    case "AppPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplDist.ClearSelection();
                        ddlApplDist.Items.Insert(0, oli1);
                        ddlApplDist.DataSource = dt;
                        ddlApplDist.DataTextField = "DistrictName";
                        ddlApplDist.DataValueField = "DistrictId";
                        ddlApplDist.DataBind();
                        break;
                    case "AppCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlApplCurrDist.ClearSelection();
                        ddlApplCurrDist.Items.Insert(0, oli2);
                        ddlApplCurrDist.DataSource = dt;
                        ddlApplCurrDist.DataTextField = "DistrictName";
                        ddlApplCurrDist.DataValueField = "DistrictId";
                        ddlApplCurrDist.DataBind();
                        break;
                    case "CoApp1Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1PerDist.ClearSelection();
                        ddlCoApp1PerDist.Items.Insert(0, oli3);
                        ddlCoApp1PerDist.DataSource = dt;
                        ddlCoApp1PerDist.DataTextField = "DistrictName";
                        ddlCoApp1PerDist.DataValueField = "DistrictId";
                        ddlCoApp1PerDist.DataBind();
                        break;
                    case "CoApp1Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp1CurrDist.ClearSelection();
                        ddlCoApp1CurrDist.Items.Insert(0, oli4);
                        ddlCoApp1CurrDist.DataSource = dt;
                        ddlCoApp1CurrDist.DataTextField = "DistrictName";
                        ddlCoApp1CurrDist.DataValueField = "DistrictId";
                        ddlCoApp1CurrDist.DataBind();
                        break;
                    case "CoApp2Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2PerDist.ClearSelection();
                        ddlCoApp2PerDist.Items.Insert(0, oli5);
                        ddlCoApp2PerDist.DataSource = dt;
                        ddlCoApp2PerDist.DataTextField = "DistrictName";
                        ddlCoApp2PerDist.DataValueField = "DistrictId";
                        ddlCoApp2PerDist.DataBind();
                        break;
                    case "CoApp2Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp2CurrDist.ClearSelection();
                        ddlCoApp2CurrDist.Items.Insert(0, oli6);
                        ddlCoApp2CurrDist.DataSource = dt;
                        ddlCoApp2CurrDist.DataTextField = "DistrictName";
                        ddlCoApp2CurrDist.DataValueField = "DistrictId";
                        ddlCoApp2CurrDist.DataBind();
                        break;
                    case "GuarPer":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarPerDist.ClearSelection();
                        ddlGuarPerDist.Items.Insert(0, oli7);
                        ddlGuarPerDist.DataSource = dt;
                        ddlGuarPerDist.DataTextField = "DistrictName";
                        ddlGuarPerDist.DataValueField = "DistrictId";
                        ddlGuarPerDist.DataBind();
                        break;
                    case "GuarCurr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlGuarCurrDist.ClearSelection();
                        ddlGuarCurrDist.Items.Insert(0, oli8);
                        ddlGuarCurrDist.DataSource = dt;
                        ddlGuarCurrDist.DataTextField = "DistrictName";
                        ddlGuarCurrDist.DataValueField = "DistrictId";
                        ddlGuarCurrDist.DataBind();
                        break;
                    case "CoApp3Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3PerDist.Items.Insert(0, oli9);
                        ddlCoApp3PerDist.DataSource = dt;
                        ddlCoApp3PerDist.DataTextField = "DistrictName";
                        ddlCoApp3PerDist.DataValueField = "DistrictId";
                        ddlCoApp3PerDist.DataBind();
                        break;
                    case "CoApp3Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp3CurrDist.Items.Insert(0, oli10);
                        ddlCoApp3CurrDist.DataSource = dt;
                        ddlCoApp3CurrDist.DataTextField = "DistrictName";
                        ddlCoApp3CurrDist.DataValueField = "DistrictId";
                        ddlCoApp3CurrDist.DataBind();
                        break;
                    case "CoApp4Per":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4PerDist.Items.Insert(0, oli11);
                        ddlCoApp4PerDist.DataSource = dt;
                        ddlCoApp4PerDist.DataTextField = "DistrictName";
                        ddlCoApp4PerDist.DataValueField = "DistrictId";
                        ddlCoApp4PerDist.DataBind();
                        break;
                    case "CoApp4Curr":
                        dt = oMem.CF_GetDisctrictByState(pStateId);
                        ddlCoApp4CurrDist.Items.Insert(0, oli12);
                        ddlCoApp4CurrDist.DataSource = dt;
                        ddlCoApp4CurrDist.DataTextField = "DistrictName";
                        ddlCoApp4CurrDist.DataValueField = "DistrictId";
                        ddlCoApp4CurrDist.DataBind();
                        break;
                }
            }
            finally
            {
            }
        }

        #endregion
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
                    // btnExit.Enabled = false;

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
                    //  btnExit.Enabled = false;
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
            txtMajorActivity.Enabled = Status;
            txtEntrpriseType.Enabled = Status;
            txtPSL.Enabled = Status;
            txtOpsIndustry.Enabled = Status;
            txtBKYCInDt.Enabled = Status;
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
        #region LoanApplication
        protected void btnUPAttachmentF_Click(object sender, EventArgs e)
        {
            ViewImgDocF(hdLeadId.Value, hdnFileNameF.Value);
        }
        protected void ViewImgDocF(string ID, string FileName)
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

        protected void btnSacLetter_Click(object sender, EventArgs e)
        {
            gblFuction.AjxMsgPopup("File will be provided by USFB...");
            return;
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

        #endregion
        #region OPSBankiingDetails
        protected void ClearbankCntrl()
        {
            txtACHolderNm.Text = "";
            txtBankNm.Text = "";
            txtACNo.Text = "";
            hdACNo.Value = "";
            txtReACNo.Text = "";
            txtIFCE.Text = "";
            txtAccType.Text = "";
            txtChq.Text = "";
            txtWaiver.Text = "";
            ddlPenyDrpStatus.SelectedValue = "-1";
            ddlPaymentTran.SelectedIndex = -1;
            ddlPaymntMode.SelectedValue = "-1";
            ddlNachType.SelectedValue = "-1";
            txtNachUMRN.Text = "";
            txtNachRef.Text = "";
        }

        protected void gvBankDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadID = 0;
            DataSet ds = new DataSet();
            ClearbankCntrl();
            try
            {


                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton lbPopData = (LinkButton)gvRow.FindControl("lbPopData");

                Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
                TextBox txtAcHoldNm = (TextBox)gvRow.FindControl("txtAcHoldNm");
                TextBox txtGridAccNo = (TextBox)gvRow.FindControl("txtAccNo");
                TextBox txtBankName = (TextBox)gvRow.FindControl("txtBankName");
                DropDownList ddlAcType = (DropDownList)gvRow.FindControl("ddlAcType");

                txtACHolderNm.Text = Convert.ToString(txtAcHoldNm.Text);
                txtBankNm.Text = Convert.ToString(txtBankName.Text);
                hdACNo.Value = Convert.ToString(txtGridAccNo.Text);
                txtACNo.Attributes.Add("value", hdACNo.Value);
                txtEffDt.Text = Session[gblValue.LoginDate].ToString();

                txtAccType.Text = Convert.ToString(ddlAcType.SelectedItem.Text);
                hdBankFlag.Value = "Y";

                popGrid(Convert.ToInt64(hdLeadId.Value), txtACNo.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        protected void gvBankDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Bank"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAcType = (DropDownList)e.Row.FindControl("ddlAcType");
                ddlAcType.SelectedIndex = ddlAcType.Items.IndexOf(ddlAcType.Items.FindByValue(e.Row.Cells[13].Text));

            }
        }

        private void GenerateBankingGrid(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            DataTable dt1 = null;
            CDistrict ODis = null;
            try
            {
                ODis = new CDistrict();
                ds = ODis.CF_GenerateBankingGrid(LeadID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                ViewState["Bank"] = dt;
                gvBankDtl.DataSource = dt;
                gvBankDtl.DataBind();
                if (dt.Rows.Count > 0)
                {
                    txtRemarks.Text = Convert.ToString(dt.Rows[0]["Remarks"]);
                }
                if (dt1.Rows.Count > 0)
                {
                    hdnTotalEMI.Value = Convert.ToString(dt1.Rows[0]["TotalEMI"]);
                    double TotalABB = 0; lblTotalABB.Text = ""; double Cal = 0;

                    foreach (GridViewRow gr in gvBankDtl.Rows)
                    {
                        TextBox txtABB = (TextBox)gvBankDtl.Rows[gr.RowIndex].FindControl("txtABB");
                        if (txtABB.Text != "")
                        {
                            if (txtABB.Text != "0")
                            {
                                TotalABB += Convert.ToDouble(txtABB.Text);
                            }
                        }
                    }
                    if (TotalABB != 0)
                    {
                        Cal = Math.Round(TotalABB / Convert.ToDouble(hdnTotalEMI.Value), 2);
                        lblTotalABB.Text = Convert.ToString(Cal);
                    }
                    //UpTotalABB.Update();

                }
                else
                {
                    hdnTotalEMI.Value = "0";
                    lblTotalABB.Text = "0";
                    //UpTotalABB.Update();
                }
                //StatusButton("Show");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dt1 = null;
            }
        }

        private void popGrid(Int64 LeadId, string AcNo)
        {

            DataTable dt = null;
            DataSet ds = new DataSet();



            CDistrict oDist = new CDistrict();
            dt = oDist.CF_GetOpsBankingdtls(LeadId, AcNo);

            if (dt.Rows.Count > 0)
            {

                txtACHolderNm.Text = Convert.ToString(dt.Rows[0]["AcHoldName"]);
                txtBankNm.Text = Convert.ToString(dt.Rows[0]["BankName"]);
                txtACNo.Attributes.Add("value", Convert.ToString(dt.Rows[0]["AccNo"]));
                hdACNo.Value = Convert.ToString(dt.Rows[0]["AccNo"]);
                txtIFCE.Text = Convert.ToString(dt.Rows[0]["IFSCCode"]);
                txtAccType.Text = Convert.ToString(dt.Rows[0]["AcType"]);
                txtChq.Text = Convert.ToString(dt.Rows[0]["ChequeCnt"]);
                txtWaiver.Text = Convert.ToString(dt.Rows[0]["WaiverCnt"]);
                if (Convert.ToString(dt.Rows[0]["PennyDropStatus"]) != "")
                {
                    ddlPenyDrpStatus.SelectedValue = Convert.ToString(dt.Rows[0]["PennyDropStatus"]);
                }
                else
                {
                    ddlPenyDrpStatus.SelectedIndex = -1;
                }
                txtRseMsg.Text = Convert.ToString(dt.Rows[0]["ResMsg"]);
                if (Convert.ToString(dt.Rows[0]["PaymentTran"]) != "")
                {
                    ddlPaymentTran.SelectedValue = Convert.ToString(dt.Rows[0]["PaymentTran"]);
                }
                else
                {
                    ddlPaymentTran.SelectedIndex = -1;
                }


                if (Convert.ToString(dt.Rows[0]["EffDate"]) == "")
                {
                    txtEffDt.Text = Session[gblValue.LoginDate].ToString();
                }
                else
                {
                    txtEffDt.Text = Convert.ToString(dt.Rows[0]["EffDate"]);
                }
                if (Convert.ToString(dt.Rows[0]["PaymentMode"]) != "")
                {
                    ddlPaymntMode.SelectedValue = Convert.ToString(dt.Rows[0]["PaymentMode"]);
                }
                else
                {
                    ddlPaymntMode.SelectedIndex = -1;
                }
                if (Convert.ToString(dt.Rows[0]["NachType"]) != "")
                {
                    ddlNachType.SelectedValue = Convert.ToString(dt.Rows[0]["NachType"]);
                }
                else
                {
                    ddlNachType.SelectedIndex = -1;
                }
                txtNachUMRN.Text = Convert.ToString(dt.Rows[0]["NachUMRN"]);
                txtNachRef.Text = Convert.ToString(dt.Rows[0]["NachRefNo"]);
                lblPassbk.Text = Convert.ToString(dt.Rows[0]["PassBkFilenm"]);
                hdPassbk.Value = Convert.ToString(dt.Rows[0]["PassBkFilenm"]);
                lblPhyMandt.Text = Convert.ToString(dt.Rows[0]["PhyMandtFilenm"]);
                hdPhyMandt.Value = Convert.ToString(dt.Rows[0]["PhyMandtFilenm"]);
                if (lblPassbk.Text != "")
                {
                    btnImgPassbk.Enabled = true;
                }
                else
                {
                    btnImgPassbk.Enabled = false;
                }

                GenerateChequeGrid(Convert.ToInt64(hdLeadId.Value), hdACNo.Value);
            }
            else
            {
                txtChq.Text = "0";
                txtWaiver.Text = "0";

            }

            EnableControlBank(false);
            oDist = new CDistrict();
            GenerateBankingGrid(LeadId);
            hdLeadId.Value = Convert.ToString(LeadId);
            txtBankInfo.Text = "1";
            GenerateIncomeDetails();
            btnSaveB.Enabled = false;
            btnCancelB.Enabled = false;
            btnEditB.Enabled = true;
            GenerateEmpBusDtls();
            btnEnableDisable();
        }

        protected void btnEditChq_Click(object sender, EventArgs e)
        {
            gvChq.Enabled = true;
            btnSaveChq.Enabled = false;
            btnCancelChq.Enabled = false;
            btnEditChq.Enabled = false;
            if (hdFlag.Value == "Y")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }
        }
        protected void btnCancelChq_Click(object sender, EventArgs e)
        {
            tbLoanAppDtl.ActiveTabIndex = 2;
            btnSaveChq.Enabled = false;
            btnCancelChq.Enabled = false;
          //  btnEditChq.Enabled = true;
            hdFlag.Value = "N";
        }

       

        #region SaveBank
        protected void btnSaveB_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveRecordsBank(vStateEdit) == true)
            {
                #region ChqDtls
                DataTable dt = (DataTable)ViewState["Chq"];
                DataTable dtUp = (DataTable)ViewState["Uploader"];
                if (dt != null)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        string srlNo = dt.Rows[i]["ChqSlId"].ToString();

                        if (dt.Rows[i]["ByteChqYN"].ToString() == "Y")
                        {
                            byte[] imgArray = (byte[])(dt.Rows[i]["ByteChqDoc"]);

                            string vChqDocName = dt.Rows[i]["AttchFilenm"].ToString();

                            if (vChqDocName.ToLower().Contains(".pdf"))
                            {
                                vChqDocName = hdLeadId.Value.ToString() + "_" + srlNo + "_" + vChqDocName;
                            }
                            else
                            {
                                vChqDocName = srlNo + "_" + vChqDocName;
                            }
                            if (MinioYN == "N")
                            {
                                string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                                System.IO.Directory.CreateDirectory(folderPath);
                                string filePath = string.Format("{0}/{1}", folderPath, vChqDocName);
                                File.WriteAllBytes(filePath, imgArray);
                            }
                            else
                            {
                                CApiCalling oAC = new CApiCalling();
                                oAC.UploadFileMinio(imgArray, vChqDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                            }
                        }

                    }
                }
                if (dtUp != null)
                {
                    if (dtUp.Rows[0]["PsBkByteYN"].ToString() == "Y")
                    {
                        byte[] imgArray = (byte[])(dtUp.Rows[0]["PsBkByteDoc"]);

                        string vDocName = dtUp.Rows[0]["PsBkFileNm"].ToString();

                        if (vDocName.ToLower().Contains(".pdf"))
                        {
                            vDocName = hdLeadId.Value.ToString() + "_" + vDocName;
                        }

                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }

                    if (dtUp.Rows[0]["NachByteYN"].ToString() == "Y")
                    {
                        byte[] imgArray = (byte[])(dtUp.Rows[0]["NachByteDoc"]);

                        string vDocName = dtUp.Rows[0]["NachFileNm"].ToString();

                        if (vDocName.ToLower().Contains(".pdf"))
                        {
                            vDocName = hdLeadId.Value.ToString() + "_" + vDocName;
                        }

                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }
                }

                #endregion

                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);
                popGrid(Convert.ToInt64(hdLeadId.Value), "");
                StatusButtonBank("Show");
                ViewState["StateEdit"] = null;
                ViewState["Chq"] = null;
            }
            else
            {
                StatusButtonBank("Edit");
            }
        }

        private Boolean SaveRecordsBank(string Mode)
        {
            Boolean vResult = false;
            string vBrCode = "", vErrMsg = "";
            Int32 vErr = 0, vMaxFileSize = 0; Int64 vLeadID = 0;
            vMaxFileSize = Convert.ToInt32(MaxFileSize); ;
            string vPassbkYN = "", vPassbkExt = "";
            DataTable dt = null, dtUp = null;
            Int32 vChequeCnt = 0, vWaiverCnt = 0; string vXmlChq = "";
            string vPassbkFileNm = ""; byte[] vFuPsBk = null;
            string vPsBkYN = "N";

            string vNachYN = "", vNachFileNm = "", vNachExt = ""; byte[] vFuNach = null;
            string vNachByteYN = "N";


            if (ViewState["BCode"] != null)
            {
                hdnBrCode.Value = ViewState["BCode"].ToString();
            }
            vBrCode = hdnBrCode.Value;

            if (ViewState["Uploader"] != null)
            {
                dtUp = (DataTable)ViewState["Uploader"];
            }
            else
            {
                dtUp = new DataTable();

                dtUp.Columns.Add("PsBkFileNm", typeof(string));
                dtUp.Columns.Add("PsBkIsUpLd", typeof(string));
                dtUp.Columns.Add("PsBkByteDoc", typeof(byte[]));
                dtUp.Columns.Add("PsBkByteYN", typeof(string));

                dtUp.Columns.Add("NachFileNm", typeof(string));
                dtUp.Columns.Add("NachYN", typeof(string));
                dtUp.Columns.Add("NachByteDoc", typeof(byte[]));
                dtUp.Columns.Add("NachByteYN", typeof(string));
            }


            DateTime vEffDt = gblFuction.setDate(txtEffDt.Text.ToString());
            CDistrict oDist = null;
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadID = Convert.ToInt64(ViewState["LeadId"]);
                }
                if (Convert.ToInt32(txtChq.Text) > 0)
                {
                    vChequeCnt = Convert.ToInt32(txtChq.Text);
                }
                if (Convert.ToInt32(txtWaiver.Text) > 0)
                {
                    vWaiverCnt = Convert.ToInt32(txtWaiver.Text);
                }


                vPassbkYN = fuPassUpld.HasFile == true ? "Y" : "N";
                if (vPassbkYN == "Y")
                {
                    vPassbkFileNm = "Passbook";
                    vFuPsBk = new byte[fuPassUpld.PostedFile.InputStream.Length + 1];
                    fuPassUpld.PostedFile.InputStream.Read(vFuPsBk, 0, vFuPsBk.Length);
                    vPassbkExt = System.IO.Path.GetExtension(fuPassUpld.FileName).ToLower();
                    if ((vPassbkExt.ToLower() != ".pdf") && (vPassbkExt.ToLower() != ".xlx") && (vPassbkExt.ToLower() != ".xlsx"))
                    {
                        vPassbkExt = ".png";
                    }
                    vPassbkFileNm = vPassbkFileNm + vPassbkExt;
                    vPsBkYN = "Y";

                    ViewState["PsBkFileNm"] = vPassbkFileNm;
                    ViewState["PassbkYN"] = vPassbkYN;
                    ViewState["PsBkByteDoc"] = vFuPsBk;
                    ViewState["PsBkByteYN"] = vPsBkYN;
                    hdFileYN.Value = "Y";

                }
                else
                {
                    if (lblPassbk.Text != "")
                    {
                        vPassbkYN = "Y";
                        vPassbkFileNm = lblPassbk.Text;
                    }
                    else
                    {
                        if ((String)ViewState["PsBkByteYN"] != "Y")
                        {
                            gblFuction.AjxMsgPopup("Please Upload Passbook File...");
                            return false;
                        }
                        else
                        {
                            vPassbkYN = "Y";
                            vPassbkFileNm = (String)ViewState["PsBkFileNm"];
                        }

                    }
                }

                //START FOR Nach Upload
                vNachYN = fuNachUpld.HasFile == true ? "Y" : "N";
                if (vNachYN == "Y")
                {
                    vNachFileNm = "PhysicalMandate";
                    vFuNach = new byte[fuPassUpld.PostedFile.InputStream.Length + 1];
                    fuNachUpld.PostedFile.InputStream.Read(vFuNach, 0, vFuNach.Length);
                    vNachExt = System.IO.Path.GetExtension(fuNachUpld.FileName).ToLower();
                    if ((vNachExt.ToLower() != ".pdf") && (vNachExt.ToLower() != ".xlx") && (vNachExt.ToLower() != ".xlsx"))
                    {
                        vNachExt = ".png";
                    }
                    vNachFileNm = vNachFileNm + vNachExt;
                    vNachByteYN = "Y";

                    ViewState["NachFileNm"] = vNachFileNm;
                    ViewState["NachYN"] = vNachYN;
                    ViewState["NachByteDoc"] = vFuNach;
                    ViewState["NachByteYN"] = vNachByteYN;
                    hdNachFileYN.Value = "Y";

                }
                else
                {
                    if ((String)ViewState["NachByteYN"] == "Y")
                    {
                        vNachYN = "Y";
                        vNachFileNm = (string)ViewState["NachFileNm"];
                    }
                    else
                    {
                        if (lblPhyMandt.Text != "")
                        {
                            vNachYN = "Y";
                            vNachFileNm = lblPhyMandt.Text;
                        }
                        else
                        {
                            vNachYN = "N";
                            vNachFileNm = "";
                        }
                    }
                }
                // END For Nach Upload

                if (dtUp.Rows.Count == 0)
                {
                    DataRow row = dtUp.NewRow();
                    row["PsBkFileNm"] = ViewState["PsBkFileNm"];
                    row["PsBkIsUpLd"] = ViewState["PassbkYN"];
                    row["PsBkByteDoc"] = ViewState["PsBkByteDoc"];
                    row["PsBkByteYN"] = ViewState["PsBkByteYN"];
                    row["NachFileNm"] = ViewState["NachFileNm"];
                    row["NachYN"] = ViewState["NachYN"];
                    row["NachByteDoc"] = ViewState["NachByteDoc"];
                    row["NachByteYN"] = ViewState["NachByteYN"];
                    dtUp.Rows.Add(row);
                    dtUp.AcceptChanges();
                    ViewState["Uploader"] = dtUp;
                }


                #region ChqDetails
                if (GetData() == true)
                {

                    if (ViewState["Chq"] != null)
                    {
                        dt = (DataTable)ViewState["Chq"];
                        dt.AcceptChanges();
                        dt.TableName = "Table";
                        using (StringWriter oSW = new StringWriter())
                        {
                            dt.WriteXml(oSW);
                            vXmlChq = oSW.ToString();
                        }
                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Fill Up Cheque Details to Save...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.AjxMsgPopup("Please Fill Up Cheque Details to Save...");
                    return false;
                }

                #endregion

                oDist = new CDistrict();
                vErr = oDist.CF_SaveOPSBankingDtl(Convert.ToInt32(txtBankInfo.Text), vLeadID, vBrCode, Convert.ToString(txtACHolderNm.Text), Convert.ToString(txtAccType.Text), Convert.ToString(txtBankNm.Text),
                   Convert.ToString(hdACNo.Value), Convert.ToString(txtIFCE.Text), Convert.ToString(ddlPenyDrpStatus.SelectedValue), Convert.ToString(txtRseMsg.Text)
                   , Convert.ToString(ddlPaymentTran.SelectedValue), vEffDt, Convert.ToString(ddlPaymntMode.SelectedValue), Convert.ToString(ddlNachType.SelectedValue)
                    , Convert.ToString(txtNachUMRN.Text), Convert.ToString(txtNachRef.Text), vPassbkFileNm, vPassbkYN
                    , DocumentBucketURL, this.UserID, Mode, 0, ref vErrMsg, vChequeCnt, vWaiverCnt, vXmlChq, vNachFileNm, vNachYN);
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
        #endregion

        protected void btnSaveChq_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveChqRecords(vStateEdit) == true)
            {

                gblFuction.AjxMsgPopup("Cheque Details Saved Successfully...");
                LoadBasicDetailsList(1);
              //  btnEditChq.Enabled = true;
                btnSaveChq.Enabled = false;
                btnCancelChq.Enabled = false;
            }
            else
            {
                btnEditChq.Enabled = false;
                btnSaveChq.Enabled = false;
                btnCancelChq.Enabled = false;
            }
            if (hdFlag.Value == "Y")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }
        }

        private Boolean SaveChqRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlChq = "", VDocumentFilePath = "";
            Int32 vMaxFileSize = 0;
            CDistrict oDis = null;
            Int32 UID = 0;
            Int64 vLeadId = 0;

            vMaxFileSize = Convert.ToInt32(MaxFileSize);
            UID = Convert.ToInt32(Session[gblValue.UserId]);
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();



                if (GetData() == true)
                {
                    dt = (DataTable)ViewState["Chq"];
                    Int32 vNumOfObli = 0;
                    DataRow[] vrows;
                    vrows = dt.Select("AccNo <> '' and AccNo <> '-1'");
                    vNumOfObli = vrows.Length;
                    if (vNumOfObli == 0)
                    {
                        gblFuction.MsgPopup("Please insert atleast one Bank Details");
                        return false;
                    }

                    vResult = true;
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

        private Boolean GetData()
        {

            string vfilenameChq = "Cheque", vChqExt;
            int vMaxFileSize = Convert.ToInt32(MaxFileSize); byte[] vfuAttachment = null;

            DataTable dt = (DataTable)ViewState["Chq"];
            foreach (GridViewRow gr in gvChq.Rows)
            {
                Label lblSrlNo = (Label)gvChq.Rows[gr.RowIndex].FindControl("lblSrlNo");
                DropDownList ddlStatus = (DropDownList)gvChq.Rows[gr.RowIndex].FindControl("ddlStatus");
                Label lblBankName = (Label)gvChq.Rows[gr.RowIndex].FindControl("lblBankName");
                Label lblAccNo = (Label)gvChq.Rows[gr.RowIndex].FindControl("lblAccNo");
                TextBox txtChqNo = (TextBox)gvChq.Rows[gr.RowIndex].FindControl("txtChqNo");



                Label lblAttchFN = (Label)gvChq.Rows[gr.RowIndex].FindControl("lblAttchFN");

                FileUpload fuAttachment = (FileUpload)gvChq.Rows[gr.RowIndex].FindControl("fuAttachment");

                if (ddlStatus.SelectedIndex <= 0)
                {
                    gblFuction.MsgPopup("Please Select Status");
                    return false;
                }

                if (ddlStatus.SelectedValue == "Received")
                {
                    if (txtChqNo.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Cheque No. is Blank...");
                        return false;
                    }

                }

                if (fuAttachment.HasFile == true)
                {
                    if (fuAttachment.PostedFile.ContentLength > vMaxFileSize)
                    {
                        gblFuction.AjxMsgPopup("Maximum upload file Size exceed the limit.File Size Should Be Maximum " + Convert.ToString(FileSize));
                        return false;
                    }

                    vfuAttachment = new byte[fuAttachment.PostedFile.InputStream.Length + 1];
                    fuAttachment.PostedFile.InputStream.Read(vfuAttachment, 0, vfuAttachment.Length);
                    vChqExt = System.IO.Path.GetExtension(fuAttachment.FileName).ToLower();
                    if ((vChqExt.ToLower() != ".pdf") && (vChqExt.ToLower() != ".xlx") && (vChqExt.ToLower() != ".xlsx"))
                    {
                        vChqExt = ".png";
                    }
                    dt.Rows[gr.RowIndex]["ByteChqDoc"] = vfuAttachment;
                    dt.Rows[gr.RowIndex]["AttchIsUpld"] = "Y";
                    dt.Rows[gr.RowIndex]["AttchFilenm"] = vfilenameChq + vChqExt;
                    dt.Rows[gr.RowIndex]["FileStoredPath"] = DocumentBucketURL;
                    dt.Rows[gr.RowIndex]["ByteChqYN"] = "Y";
                    //  lblAttchFN.Text = vfilenameChq + vChqExt;
                }
                else
                {
                    if (lblAttchFN.Text == "")
                    {
                        if (ddlStatus.SelectedValue != "Pending")
                        {
                            gblFuction.AjxMsgPopup("Please Select a File to Upload...");
                            return false;
                        }
                    }
                    else
                    {
                        if (dt.Rows[gr.RowIndex]["ByteChqYN"].ToString() == "N")
                        {
                            if (dt.Rows[gr.RowIndex]["AttchIsUpld"].ToString() == "Y")
                            {
                                dt.Rows[gr.RowIndex]["AttchFilenm"] = lblAttchFN.Text;
                            }
                            else
                            {
                                dt.Rows[gr.RowIndex]["AttchIsUpld"] = "N";
                                dt.Rows[gr.RowIndex]["AttchFilenm"] = "";
                                lblAttchFN.Text = "";
                            }
                        }

                    }
                }


                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["ChequeStatus"] = ddlStatus.SelectedValue;
                dt.Rows[gr.RowIndex]["ChqSlId"] = Convert.ToInt32(lblSrlNo.Text == "" ? "" : lblSrlNo.Text);

                dt.Rows[gr.RowIndex]["BankName"] = Convert.ToString(lblBankName.Text == "" ? "" : lblBankName.Text);
                dt.Rows[gr.RowIndex]["AccNo"] = Convert.ToString(lblAccNo.Text == "" ? "" : lblAccNo.Text);
                dt.Rows[gr.RowIndex]["ChequeNo"] = Convert.ToString(txtChqNo.Text == "" ? "" : txtChqNo.Text);


            }
            dt.AcceptChanges();
            ViewState["Chq"] = dt;
            gvChq.DataSource = dt;
            gvChq.DataBind();
            return true;
        }

        protected void UploadBankFile()
        {
            DataTable dtUp = null;
            string vPassbkYN = "", vPassbkFileNm = "", vPassbkExt = ""; byte[] vFuPsBk = null;
            string vPsBkYN = "N";

            string vNachYN = "", vNachFileNm = "", vNachExt = ""; byte[] vFuNach = null;
            string vNachByteYN = "N";

            //START FOR Passbook Upload
            vPassbkYN = fuPassUpld.HasFile == true ? "Y" : "N";
            if (vPassbkYN == "Y")
            {
                vPassbkFileNm = "Passbook";
                vFuPsBk = new byte[fuPassUpld.PostedFile.InputStream.Length + 1];
                fuPassUpld.PostedFile.InputStream.Read(vFuPsBk, 0, vFuPsBk.Length);
                vPassbkExt = System.IO.Path.GetExtension(fuPassUpld.FileName).ToLower();
                if ((vPassbkExt.ToLower() != ".pdf") && (vPassbkExt.ToLower() != ".xlx") && (vPassbkExt.ToLower() != ".xlsx"))
                {
                    vPassbkExt = ".png";
                }
                vPassbkFileNm = vPassbkFileNm + vPassbkExt;
                vPsBkYN = "Y";

                ViewState["PsBkFileNm"] = vPassbkFileNm;
                ViewState["PassbkYN"] = vPassbkYN;
                ViewState["PsBkByteDoc"] = vFuPsBk;
                ViewState["PsBkByteYN"] = vPsBkYN;
                hdFileYN.Value = "Y";

            }
            // END For Passbook Upload


            //START FOR Nach Upload
            vNachYN = fuNachUpld.HasFile == true ? "Y" : "N";
            if (vNachYN == "Y")
            {
                vNachFileNm = "PhysicalMandate";
                vFuNach = new byte[fuPassUpld.PostedFile.InputStream.Length + 1];
                fuNachUpld.PostedFile.InputStream.Read(vFuNach, 0, vFuNach.Length);
                vNachExt = System.IO.Path.GetExtension(fuNachUpld.FileName).ToLower();
                if ((vNachExt.ToLower() != ".pdf") && (vNachExt.ToLower() != ".xlx") && (vNachExt.ToLower() != ".xlsx"))
                {
                    vNachExt = ".png";
                }
                vNachFileNm = vNachFileNm + vNachExt;
                vNachByteYN = "Y";

                ViewState["NachFileNm"] = vNachFileNm;
                ViewState["NachYN"] = vNachYN;
                ViewState["NachByteDoc"] = vFuNach;
                ViewState["NachByteYN"] = vNachByteYN;
                hdNachFileYN.Value = "Y";

            }
            // END For Nach Upload
        }

        protected void ImgChqNo_Click(object sender, EventArgs e)
        {
            Int64 vLeadID = 0;


            UploadBankFile();


            if (hdLeadId.Value != "")
            {
                vLeadID = Convert.ToInt64(hdLeadId.Value);
            }

            ViewChqPop(vLeadID, hdACNo.Value);

            StatusButtonBank("Edit");

            if (txtReACNo.Text != "")
            {
                txtReACNo.Attributes.Add("value", hdACNo.Value);
            }
        }

        protected void ViewChqPop(Int64 LeadID, string AcNo)
        {

            GenerateChequeGrid(LeadID, AcNo);
            if (hdFlag.Value == "Y")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }
        }

        private void GenerateChequeGrid(Int64 LeadID, string AccNo)
        {
            DataSet ds = null;
            DataTable dt = null;
            CDistrict ODis = null;
            DataTable dtChq = null;
            if (ViewState["Chq"] != null)
            {
                dtChq = (DataTable)ViewState["Chq"];
            }
            try
            {
                if (dtChq != null)
                {
                    if (dtChq.Rows.Count > 0)
                    {
                        hdFlag.Value = "Y";
                        gvChq.DataSource = dtChq;
                        gvChq.DataBind();
                    }
                }
                else
                {
                    ODis = new CDistrict();
                    ds = ODis.GenerateChequeGrid(LeadID, AccNo);
                    hdFlag.Value = "Y";
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["Chq"] = dt;
                        gvChq.DataSource = dt;
                        gvChq.DataBind();
                    }
                    else
                    {
                        gvChq.DataSource = null;
                        gvChq.DataBind();
                    }
                }


                CalTotalChq();
                // gvChq.Enabled = false;
                btnSaveChq.Enabled = false;
                btnCancelChq.Enabled = false;
               // btnEditChq.Enabled = true;
                btnEnableDisable();
            }

            finally
            {
            }
        }

        protected void gvChq_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDel")
            {
                DataTable dt = null;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                int index = row.RowIndex;
                dt = (DataTable)ViewState["Chq"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[index].Delete();
                    dt.AcceptChanges();
                    ViewState["Chq"] = dt;
                    gvChq.DataSource = dt;
                    gvChq.DataBind();
                    if (hdFlag.Value == "Y")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
                    }
                }
                else
                {
                    if (hdFlag.Value == "Y")
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
                    }
                    gblFuction.AjxMsgPopup("First Row can not be deleted.");
                    return;
                }
            }
        }

        protected void gvChq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Chq"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                // HiddenField AttchIsUpld = (HiddenField)e.Row.FindControl("AttchIsUpld");
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(lblStatus.Text));

                Label lblAccNo = (Label)e.Row.FindControl("lblAccNo");
                Label lblBankName = (Label)e.Row.FindControl("lblBankName");

                lblAccNo.Text = hdACNo.Value;
                lblBankName.Text = txtBankNm.Text;

            }
        }

        protected void imgAttch_Click(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSrlNo");
            Label lbFile = (Label)gvRow.FindControl("lblAttchFN");

            ViewAttchDoc(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);
        }
        protected void ViewAttchDoc(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + lblSrlNo + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(lblSrlNo + "_" + FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + lblSrlNo + "_" + FileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataRow dr;
            if (GetData() == true)
            {
                DataTable dt = (DataTable)ViewState["Chq"];
                dt.AcceptChanges();
                dr = dt.NewRow();
                dt.Rows.Add(dr);

                ViewState["Chq"] = dt;
                gvChq.DataSource = dt;
                gvChq.DataBind();
                CalTotalChq();
            }
            if (hdFlag.Value == "Y")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }
        }

        protected void txtChqNo_TextChanged(object sender, EventArgs e)
        {
            if (hdFlag.Value == "Y")
            {
                CalTotalChq();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }

        }
        private void CalTotalChq()
        {
            int cnt = 0, Wcnt = 0;

            foreach (GridViewRow gr in gvChq.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtChqNo = (TextBox)gr.FindControl("txtChqNo");
                    DropDownList ddlStatus = (DropDownList)gvChq.Rows[gr.RowIndex].FindControl("ddlStatus");
                    if (txtChqNo != null && !string.IsNullOrEmpty(txtChqNo.Text))
                    {
                        if (ddlStatus != null && !string.IsNullOrEmpty(ddlStatus.SelectedValue))
                        {
                            if (ddlStatus.SelectedValue == "Received")
                            {
                                cnt += 1;
                            }
                        }
                    }
                    if (ddlStatus != null && !string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    {
                        if (ddlStatus.SelectedValue == "Waiver")
                        {
                            Wcnt += 1;
                        }
                    }

                }

            }
            hdChqCnt.Value = Convert.ToString(cnt);
            hdWaiverCnt.Value = Convert.ToString(Wcnt);

            txtChq.Text = Convert.ToString(cnt);
            txtWaiver.Text = Convert.ToString(Wcnt);
            GridViewRow footerRow = gvChq.FooterRow;
            if (footerRow != null)
            {
                Label lblChqtotal = (Label)gvChq.FooterRow.FindControl("lblChqtotal");

                if (lblChqtotal != null)
                {
                    if (cnt > 0)
                        lblChqtotal.Text = Convert.ToString(cnt);
                    else
                        lblChqtotal.Text = "0";
                }

            }

        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdFlag.Value == "Y")
            {
                ddlStatusCheck();
                CalTotalChq();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpVoter();", true);
            }
        }

        private void ddlStatusCheck()
        {
            int Wcnt = 0;
            foreach (GridViewRow gr in gvChq.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlStatus = (DropDownList)gvChq.Rows[gr.RowIndex].FindControl("ddlStatus");

                    if (ddlStatus != null && !string.IsNullOrEmpty(ddlStatus.SelectedValue))
                    {
                        if (ddlStatus.SelectedValue == "Waiver")
                        {
                            Wcnt += 1;
                        }
                    }

                }

            }

            hdWaiverCnt.Value = Convert.ToString(Wcnt);
            txtWaiver.Text = Convert.ToString(Wcnt);
        }

        #region imgPass_Click
        protected void imgPass_Click(object sender, EventArgs e)
        {
            ViewPassImgDoc(hdLeadId.Value, txtBankInfo.Text, lblPassbk.Text);

        }

        protected void ViewPassImgDoc(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
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
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
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
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        #endregion

        protected void imgPhyMandt_Click(object sender, EventArgs e)
        {
            ViewPhyMandt(hdLeadId.Value, lblPhyMandt.Text);
        }
        protected void ViewPhyMandt(string ID, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
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
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
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
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        protected void btnEditB_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                StatusButtonBank("Edit");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StatusButtonBank(String pMode)
        {
            switch (pMode)
            {
                case "Show":
                    btnEditB.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControlBank(false);
                    break;
                case "Edit":
                    btnEditB.Enabled = false;
                    txtBankInfo.Enabled = true;
                   // btnSaveB.Enabled = true;
                  //  btnCancelB.Enabled = true;
                    EnableControlBank(false);
                    break;
                case "View":
                    btnEditB.Enabled = false;
                  //  btnSaveB.Enabled = false;
                   // btnCancelB.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControlBank(false);
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
        private void EnableControlBank(Boolean Status)
        {
            // txtBankInfo.Enabled = Status;
            if (hdBankFlag.Value == "Y")
            {
                txtACHolderNm.Enabled = false;
                txtBankNm.Enabled = false;
                txtACNo.Enabled = false;
                txtAccType.Enabled = false;
            }
            else
            {
                txtACHolderNm.Enabled = Status;
                txtBankNm.Enabled = Status;
                txtACNo.Enabled = Status;
                txtAccType.Enabled = Status;
            }
            txtReACNo.Enabled = Status;
            txtIFCE.Enabled = Status;
            fuPassUpld.Enabled = Status;

            ddlPaymentTran.Enabled = Status;

            ddlPaymntMode.Enabled = Status;
            ddlNachType.Enabled = Status;
            txtNachUMRN.Enabled = Status;
            txtNachRef.Enabled = Status;
            //btnSaveB.Enabled = Status;
            //btnCancelB.Enabled = Status;
            ImgChqNo.Enabled = Status;
            lbRseMsg.Enabled = Status;

            if (ddlNachType.SelectedValue == "Physical")
            {
                fuNachUpld.Enabled = true;
            }
            else
            {
                fuNachUpld.Enabled = false;
            }
        }

        protected void btnCancelB_Click(object sender, EventArgs e)
        {
            popGrid(Convert.ToInt64(hdLeadId.Value), "");
            EnableControlBank(false);
            // ClearControlsBank();
            StatusButtonBank("Show");
        }

        #endregion
        #region OPSSactionCondition
        protected void gvBOEDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkAdd = (CheckBox)e.Row.FindControl("chkAdd");
                if (e.Row.Cells[10].Text == "N")
                {
                    chkAdd.Checked = false;
                }
                else
                {
                    chkAdd.Checked = true;
                }

            }
        }

        private void GenerateOPSSactionGrid(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateOPSSactionGrid(LeadID);
                dt = ds.Tables[0];
                ViewState["BOE"] = dt;
                gvBOEDtl.DataSource = dt;
                gvBOEDtl.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }


        #region imgBCBCM_Click
        protected void imgBCBCM_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblBCBCMFN");

            ViewImgSac(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion

        #region imgHO_Click
        protected void imgHO_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblHOFileNm");

            ViewImgSac(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion

        #region imgBOE_Click
        protected void imgBOE_Click(object sender, EventArgs e)
        {

            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lblSrlNo = (Label)gvRow.FindControl("lblSacId");
            Label lbFile = (Label)gvRow.FindControl("lblBOEFN");

            ViewImgSac(hdLeadId.Value, lblSrlNo.Text, lbFile.Text);

        }
        #endregion

        protected void ViewImgSac(string ID, string lblSrlNo, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
            string[] ActNetPath = DocumentBucketURL.Split(',');
            for (int j = 0; j <= ActNetPath.Length - 1; j++)
            {
                ActNetImage = ActNetPath[j] + ID + "_" + lblSrlNo + "_" + FileName;
                if (ValidUrlChk(ActNetImage))
                {
                    vPdfFile = ActNetImage;
                    break;
                }
            }
            if (vPdfFile != "")
            {
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(lblSrlNo + "_" + FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                }
                else
                {

                    WebClient cln = null;
                    cln = new WebClient();
                    byte[] vDoc = null;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc0 | 0x300 | 0xc00);
                    cln = new WebClient();
                    vDoc = cln.DownloadData(vPdfFile);
                    Response.AddHeader("Content-Type", "Application/octet-stream");
                    Response.AddHeader("Content-Disposition", "attachment;   filename=" + ID + "_" + lblSrlNo + "_" + FileName);
                    Response.BinaryWrite(vDoc);
                    Response.Flush();
                    Response.End();
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }


        protected void btnEditS_Click(object sender, EventArgs e)
        {
            btnSaveS.Enabled = true;
            btnCancelS.Enabled = true;
            gvBOEDtl.Enabled = true;
            btnEditS.Enabled = false;

        }

        private Boolean GetSacData()
        {
            DataTable dt = (DataTable)ViewState["BOE"];
            foreach (GridViewRow gr in gvBOEDtl.Rows)
            {
                Label lblSacId = (Label)gvBOEDtl.Rows[gr.RowIndex].FindControl("lblSacId");
                CheckBox chkAdd = (CheckBox)gvBOEDtl.Rows[gr.RowIndex].FindControl("chkAdd");

                if (chkAdd.Checked)
                {
                    dt.Rows[gr.RowIndex]["IsChecked"] = "Y";
                }
                else
                {
                    dt.Rows[gr.RowIndex]["IsChecked"] = "N";
                }

                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["SanctionId"] = Convert.ToInt32(lblSacId.Text == "" ? "" : lblSacId.Text);

            }
            dt.AcceptChanges();
            ViewState["BOE"] = dt;
            gvBOEDtl.DataSource = dt;
            gvBOEDtl.DataBind();
            return true;
        }
        protected void btnSaveS_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveSactionRecords(vStateEdit) == true)
            {
                LoadBasicDetailsList(1);
                btnEditS.Enabled = true;
                btnSaveS.Enabled = false;
                btnCancelS.Enabled = false;
                gvBOEDtl.Enabled = false;
            }
            else
            {
                btnEditS.Enabled = false;
                btnSaveS.Enabled = true;
                btnCancelS.Enabled = true;
                gvBOEDtl.Enabled = true;
            }

        }


        private Boolean SaveSactionRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlSC = "";

            CMember oMem = null;
            Int32 UID = 0;
            Int64 vLeadId = 0;


            UID = Convert.ToInt32(Session[gblValue.UserId]);
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                vBrCode = Session[gblValue.BrnchCode].ToString();



                if (GetSacData() == true)
                {
                    dt = (DataTable)ViewState["BOE"];

                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlSC = oSW.ToString();
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oMem = new CMember();
                        vErr = oMem.CF_SaveOPSSancDtl(vLeadId, vXmlSC, vBrCode, UID, 0, ref vErrMsg);
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
        protected void btnCancelS_Click(object sender, EventArgs e)
        {
            tbLoanAppDtl.ActiveTabIndex = 0;
            btnSaveS.Enabled = false;
            btnCancelS.Enabled = false;
            btnEditS.Enabled = true;
            gvBOEDtl.Enabled = false;
        }

        #endregion
        #region DisbursmentDocumentList

        protected void gvBCDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["Bank"];
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton imgKotak = (ImageButton)e.Row.FindControl("imgKotak");
                ImageButton imgFile = (ImageButton)e.Row.FindControl("imgFile");

                if (e.Row.RowIndex == 0)
                {
                    imgKotak.Visible = true;
                    imgFile.Visible = false;
                }
                else
                {
                    imgKotak.Visible = false;
                    imgFile.Visible = true;
                }

            }
        }

        #region imgFile_Click
        protected void imgFile_Click(object sender, EventArgs e)
        {

            gblFuction.AjxMsgPopup("Document not given...");
            return;

        }
        #endregion

        #region imgKotak_Click
        protected void imgKotak_Click(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)((ImageButton)sender).NamingContainer;
            Label lbFile = (Label)gvRow.FindControl("lblKotak");

            ViewKotakDoc(hdLeadId.Value, lbFile.Text);

        }

        protected void ViewKotakDoc(string ID, string FileName)
        {
            string ActNetImage = "", vPdfFile = "", vBase64String = "";
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
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
                }
                else
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
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }

        }
        #endregion

        #region imgUpload_Click
        protected void imgUpload_Click(object sender, EventArgs e)
        {

            gblFuction.AjxMsgPopup("Document not given...");
            return;

        }
        #endregion

        #region lbExcMode_Click
        protected void lbExcMode_Click(object sender, EventArgs e)
        {

            gblFuction.AjxMsgPopup("Document not given...");
            return;

        }
        #endregion

        #region imgSelfie_Click
        protected void imgSelfie_Click(object sender, EventArgs e)
        {

            ViewSelfiImg(hdLeadId.Value, hdnSelfieFileName.Value);

        }
        #endregion

        protected void ViewSelfiImg(string ID, string FileName)
        {
            string vBase64String = "";
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
                if (vPdfFile.ToLower().Contains(".png"))
                {
                    vBase64String = GetBase64Image(FileName, ID);
                    ImgDocumentImage.ImageUrl = "data:image;base64," + vBase64String;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUp();", true);
                }
            }
            else
            {
                gblFuction.MsgPopup("No File Found");
            }
        }


        private void GenerateDisbursmentList(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.CF_GetOpsDisbursmentList(LeadID);
                dt = ds.Tables[0];
                ViewState["BCBM"] = dt;
                gvBCDtl.DataSource = dt;
                gvBCDtl.DataBind();
                if (dt.Rows.Count > 0)
                {
                    lblSelfieFileName.Text = Convert.ToString(dt.Rows[0]["SelfiFileNm"]);
                    hdnSelfieFileName.Value = Convert.ToString(dt.Rows[0]["SelfiFileNm"]);
                }

                if (Convert.ToString(dt.Rows[0]["BcBmRemarks"]) == "")
                {
                    btnEditD.Enabled = false;
                    gvBCDtl.Enabled = false;
                    fuSelfie.Enabled = false;
                }
                else
                {
                    btnEditD.Enabled = true;
                    gvBCDtl.Enabled = false;
                    fuSelfie.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        protected void btnEditD_Click(object sender, EventArgs e)
        {
            btnSaveD.Enabled = true;
            btnCancelD.Enabled = true;
            gvBCDtl.Enabled = true;
            btnEditD.Enabled = false;
            fuSelfie.Enabled = true;
        }
        protected void btnCancelD_Click(object sender, EventArgs e)
        {
            GenerateDisbursmentList(Convert.ToInt64(hdLeadId.Value));
            btnSaveD.Enabled = false;
            btnCancelD.Enabled = false;
            btnEditD.Enabled = true;
            gvBCDtl.Enabled = false;
            fuSelfie.Enabled = false;
        }
        private Boolean GetBCData()
        {
            DataTable dt = (DataTable)ViewState["BCBM"];
            string vFileNmKotak = "KotakInsurance", vKotakExt;
            int vMaxFileSize = Convert.ToInt32(MaxFileSize); byte[] vFuKotak = null;
            foreach (GridViewRow gr in gvBCDtl.Rows)
            {
                Label lblDocID = (Label)gvBCDtl.Rows[gr.RowIndex].FindControl("lblDocID");
                Label lblDocType = (Label)gvBCDtl.Rows[gr.RowIndex].FindControl("lblDocType");
                LinkButton lbExcMode = (LinkButton)gvBCDtl.Rows[gr.RowIndex].FindControl("lbExcMode");
                Label lblBMRemarks = (Label)gvBCDtl.Rows[gr.RowIndex].FindControl("lblBMRemarks");
                Label lblKotak = (Label)gvBCDtl.Rows[gr.RowIndex].FindControl("lblKotak");
                TextBox txtCentrlRemarks = (TextBox)gvBCDtl.Rows[gr.RowIndex].FindControl("txtCentrlRemarks");
                Label lblAddedBy = (Label)gvBCDtl.Rows[gr.RowIndex].FindControl("lblAddedBy");

                if (txtCentrlRemarks.Text == "")
                {
                    gblFuction.AjxMsgPopup("Central Ops-1 Stage Remarks is Blank...");
                    return false;
                }

                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["DocID"] = Convert.ToInt32(lblDocID.Text == "" ? "" : lblDocID.Text);
                dt.Rows[gr.RowIndex]["DocName"] = Convert.ToString(lblDocType.Text == "" ? "" : lblDocType.Text);
                dt.Rows[gr.RowIndex]["ExcMode"] = Convert.ToString(lbExcMode.Text == "" ? "" : lbExcMode.Text);
                dt.Rows[gr.RowIndex]["BcBmRemarks"] = Convert.ToString(lblBMRemarks.Text == "" ? "" : lblBMRemarks.Text);
                dt.Rows[gr.RowIndex]["CentrlRemarks"] = Convert.ToString(txtCentrlRemarks.Text == "" ? "" : txtCentrlRemarks.Text);
                dt.Rows[gr.RowIndex]["AddedBy"] = Convert.ToString(lblAddedBy.Text == "" ? "" : lblAddedBy.Text);
                dt.Rows[gr.RowIndex]["UpdatedBy"] = "Central Ops-1 Stage";
                dt.Rows[gr.RowIndex]["KotakIsUpld"] = "Y";
                dt.Rows[gr.RowIndex]["KotakFileNm"] = lblKotak.Text;



            }
            dt.AcceptChanges();
            ViewState["BCBM"] = dt;
            gvBCDtl.DataSource = dt;
            gvBCDtl.DataBind();
            return true;
        }

        protected void btnSaveD_Click(object sender, EventArgs e)
        {
            vPathImage = ConfigurationManager.AppSettings["pathImage"];
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveBCRecords(vStateEdit) == true)
            {
                DataTable dt = (DataTable)ViewState["BCBM"];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[0]["ByteKotakYN"].ToString() == "Y")
                    {
                        byte[] imgArray = (byte[])(dt.Rows[0]["ByteKotakADoc"]);

                        string vBankDocName = dt.Rows[0]["KotakFileNm"].ToString();

                        if (vBankDocName.ToLower().Contains(".pdf"))
                        {
                            vBankDocName = hdLeadId.Value.ToString() + "_" + vBankDocName;
                        }


                        if (MinioYN == "N")
                        {
                            string folderPath = string.Format("{0}/{1}", vPathImage, hdLeadId.Value);
                            System.IO.Directory.CreateDirectory(folderPath);
                            string filePath = string.Format("{0}/{1}", folderPath, vBankDocName);
                            File.WriteAllBytes(filePath, imgArray);
                        }
                        else
                        {
                            CApiCalling oAC = new CApiCalling();
                            oAC.UploadFileMinio(imgArray, vBankDocName, hdLeadId.Value, DocumentBucket, MinioUrl);
                        }
                    }

                }

                GenerateDisbursmentList(Convert.ToInt64(hdLeadId.Value));
                btnEditD.Enabled = true;
                btnSaveD.Enabled = false;
                btnCancelD.Enabled = false;
                gvBCDtl.Enabled = false;
                fuSelfie.Enabled = false;
            }
            else
            {
                btnEditD.Enabled = false;
                btnSaveD.Enabled = true;
                btnCancelD.Enabled = true;
                gvBCDtl.Enabled = true;
                fuSelfie.Enabled = true;
            }

        }


        private Boolean SaveBCRecords(string Mode)
        {
            Boolean vResult = false;
            Int32 vErr = 0;
            string vBrCode = "", vErrMsg = "";
            DataTable dt = null;
            string vXmlBM = "";

            CMember oMem = null;
            Int32 UID = 0;
            Int64 vLeadId = 0;
            string vFileNm = "CustomerSelfi", vIsUpLd = "", vFileExt = "", vFilePath = "";

            vFilePath = DocumentBucketURL;
            UID = Convert.ToInt32(Session[gblValue.UserId]);
            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }


                if (ViewState["BCode"] != null)
                {
                    hdnBrCode.Value = ViewState["BCode"].ToString();
                }
                vBrCode = hdnBrCode.Value;



                if (GetBCData() == true)
                {
                    dt = (DataTable)ViewState["BCBM"];

                    dt.TableName = "Table";
                    using (StringWriter oSW = new StringWriter())
                    {
                        dt.WriteXml(oSW);
                        vXmlBM = oSW.ToString();
                    }

                    vIsUpLd = fuSelfie.HasFile == true ? "Y" : "N";
                    if (vIsUpLd == "Y")
                    {
                        vFileExt = System.IO.Path.GetExtension(fuSelfie.PostedFile.FileName);

                        if ((vFileExt.ToLower() == ".pdf") || (vFileExt.ToLower() == ".xlx") || (vFileExt.ToLower() == ".xlsx"))
                        {
                            gblFuction.AjxMsgPopup("Please Upload Image File...");
                            return false;
                        }
                        else
                        {
                            vFileExt = ".png";
                        }
                        vFileNm = fuSelfie.HasFile == true ? vFileNm + vFileExt : "";

                    }
                    else
                    {
                        if (lblSelfieFileName.Text != "")
                        {
                            vIsUpLd = "Y";
                            vFileNm = lblSelfieFileName.Text;
                        }
                        else
                        {
                            vFileNm = "";
                            vIsUpLd = "N";
                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oMem = new CMember();
                        vErr = oMem.CF_SaveOpsDisbursmentList(vLeadId, vXmlBM, vBrCode, UID, 0, ref vErrMsg, vFileNm, vIsUpLd, vFilePath);
                        if (vErr == 0)
                        {
                            if (fuSelfie.HasFile)
                            {
                                SaveMemberImages(fuSelfie, hdLeadId.Value, "CustomerSelfi", ".png", "N");
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
        #region HypothecationDtls

        private void GenerateHypothecationDtls(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.CF_GetOpsHypothecationdtls(LeadID);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtInstDate.Text = Convert.ToString(dt.Rows[0]["InstDate"]);
                    txtAssetID.Text = Convert.ToString(dt.Rows[0]["AssetDesc"]);
                    hdAssetID.Value = Convert.ToString(dt.Rows[0]["AssetID"]);
                    txtEPCName.Text = Convert.ToString(dt.Rows[0]["EPCName"]);
                    txtKW.Text = Convert.ToString(dt.Rows[0]["KW"]);
                    txtInstAddress.Text = Convert.ToString(dt.Rows[0]["InstAddress"]);
                    txtMountingSystem.Text = Convert.ToString(dt.Rows[0]["MountingSystem"]);
                    txtInverterNo.Text = Convert.ToString(dt.Rows[0]["InverterNo"]);
                    txtSecuritySystem.Text = Convert.ToString(dt.Rows[0]["SecuritySystem"]);
                    txtSolarPanelModel.Text = Convert.ToString(dt.Rows[0]["SolarPanelModel"]);
                    txtInverterDisconn.Text = Convert.ToString(dt.Rows[0]["InverterDisconn"]);
                    txtSolarPanelGPSModel.Text = Convert.ToString(dt.Rows[0]["SolarPanelGPSModel"]);
                    txtACServicePanel.Text = Convert.ToString(dt.Rows[0]["ACServicePanel"]);
                    txtGPStrackerNo.Text = Convert.ToString(dt.Rows[0]["GPStrackerNo"]);
                    txtACMeterNo.Text = Convert.ToString(dt.Rows[0]["ACMeterNo"]);
                    txtMeterNo.Text = Convert.ToString(dt.Rows[0]["MeterNo"]);
                    txtInsurPartner.Text = Convert.ToString(dt.Rows[0]["InsurPartner"]);
                    txtSumAssured.Text = Convert.ToString(dt.Rows[0]["SumAssured"]);
                    txtCoverageMaterial.Text = Convert.ToString(dt.Rows[0]["CoverageMaterial"]);
                    txtFromCovPeriod.Text = Convert.ToString(dt.Rows[0]["FromCovPeriod"]);
                    txtPolicyRefNo.Text = Convert.ToString(dt.Rows[0]["PolicyRefNo"]);
                    txtToCovPeriod.Text = Convert.ToString(dt.Rows[0]["ToCovPeriod"]);
                    txtPolicyNo.Text = Convert.ToString(dt.Rows[0]["PolicyNo"]);
                }
                StatusHypoButton("Show");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        protected void btnHypoCancel_Click(object sender, EventArgs e)
        {
            btnHypoSave.Enabled = false;
            btnHypoCancel.Enabled = false;
            btnHypoEdit.Enabled = true;

            GenerateHypothecationDtls(Convert.ToInt64(hdLeadId.Value));
            //txtInstDate.Text ="";
            //txtMountingSystem.Text ="";
            //txtInverterNo.Text ="";
            //txtSecuritySystem.Text ="";
            //txtSolarPanelModel.Text ="";
            //txtInverterDisconn.Text ="";
            //txtSolarPanelGPSModel.Text ="";
            //txtACServicePanel.Text ="";
            //txtGPStrackerNo.Text ="";
            //txtACMeterNo.Text ="";
            //txtMeterNo.Text ="";
            //txtInsurPartner.Text ="";
            //txtSumAssured.Text ="";
            //txtCoverageMaterial.Text ="";
            //txtFromCovPeriod.Text ="";
            //txtPolicyRefNo.Text ="";
            //txtToCovPeriod.Text ="";
            //txtPolicyNo.Text = "";
            EnableHypoControl(false);
        }

        protected void btnHypoSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveHypoRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);

                GenerateHypothecationDtls(Convert.ToInt64(hdLeadId.Value));
                StatusHypoButton("Show");
                ViewState["StateEdit"] = null;

            }
        }
        private Boolean SaveHypoRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0;

            string vAssetDesc = "", vEPCName = "", vKW = "", vInstAddress = "", vMountingSystem = "",
            vInverterNo = "", vSecuritySystem = "", vSolarPanelModel = "", vInverterDisconn = "", vSolarPanelGPSModel = "",
            vACServicePanel = "", vGPStrackerNo = "", vACMeterNo = "", vMeterNo = "", vInsurPartner = "", vCoverageMaterial = "",
            vPolicyRefNo = "", vPolicyNo = "", vErrMsg = "", vBrCode = "";

            Int32 vSumAssured = 0, vAssetId = 0;

            Int64 vLeadId = 0;
            CMember oMem = null;

            try
            {
                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }

                if (ViewState["BCode"] != null)
                {
                    hdnBrCode.Value = ViewState["BCode"].ToString();
                }
                vBrCode = hdnBrCode.Value;

                DateTime vInstDate = gblFuction.setDate(txtInstDate.Text.ToString());
                DateTime vFromCovPeriod = gblFuction.setDate(txtFromCovPeriod.Text.ToString());
                DateTime vToCovPeriod = gblFuction.setDate(txtToCovPeriod.Text.ToString());

                vAssetDesc = Convert.ToString(txtAssetID.Text);
                vEPCName = Convert.ToString(txtEPCName.Text);
                vKW = Convert.ToString(txtKW.Text);
                vInstAddress = Convert.ToString(txtInstAddress.Text);
                vMountingSystem = Convert.ToString(txtMountingSystem.Text);
                vInverterNo = Convert.ToString(txtInverterNo.Text);
                vSecuritySystem = Convert.ToString(txtSecuritySystem.Text);
                vSolarPanelModel = Convert.ToString(txtSolarPanelModel.Text);
                vInverterDisconn = Convert.ToString(txtInverterDisconn.Text);
                vSolarPanelGPSModel = Convert.ToString(txtSolarPanelGPSModel.Text);
                vACServicePanel = Convert.ToString(txtACServicePanel.Text);
                vGPStrackerNo = Convert.ToString(txtGPStrackerNo.Text);
                vACMeterNo = Convert.ToString(txtACMeterNo.Text);
                vMeterNo = Convert.ToString(txtMeterNo.Text);
                vInsurPartner = Convert.ToString(txtInsurPartner.Text);
                vCoverageMaterial = Convert.ToString(txtCoverageMaterial.Text);
                vPolicyRefNo = Convert.ToString(txtPolicyRefNo.Text);
                vPolicyNo = Convert.ToString(txtPolicyNo.Text);

                if (txtSumAssured.Text == "") txtSumAssured.Text = "0";

                vSumAssured = Convert.ToInt32(txtSumAssured.Text);
                vAssetId = Convert.ToInt32(hdAssetID.Value);

                oMem = new CMember();
                vErr = oMem.CF_SaveOPSHypothecationDtls(vLeadId, vInstDate, vAssetId, vAssetDesc, vEPCName, vKW, vInstAddress, vMountingSystem, vInverterNo, vSecuritySystem,
                    vSolarPanelModel, vInverterDisconn, vSolarPanelGPSModel, vACServicePanel, vGPStrackerNo, vACMeterNo, vMeterNo, vInsurPartner,
                vCoverageMaterial, vPolicyRefNo, vPolicyNo, vSumAssured, vFromCovPeriod, vToCovPeriod, vBrCode, this.UserID, Mode, 0, ref vErrMsg);
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


        protected void btnHypoEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }
                if (txtInstDate.Text == "")
                {
                    ViewState["StateEdit"] = "Add";
                }
                else
                {
                    ViewState["StateEdit"] = "Edit";
                }

                StatusHypoButton("Edit");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void StatusHypoButton(String pMode)
        {
            switch (pMode)
            {
                case "Show":
                    EnableHypoControl(false);
                    btnHypoEdit.Enabled = true;
                    btnHypoSave.Enabled = false;
                    btnHypoCancel.Enabled = false;
                    break;
                case "Edit":
                    EnableHypoControl(true);
                    btnHypoEdit.Enabled = false;
                    btnHypoSave.Enabled = true;
                    btnHypoCancel.Enabled = true;
                    break;
            }
        }

        private void EnableHypoControl(Boolean Status)
        {
            txtInstDate.Enabled = Status;
            txtMountingSystem.Enabled = Status;
            txtInverterNo.Enabled = Status;
            txtSecuritySystem.Enabled = Status;
            txtSolarPanelModel.Enabled = Status;
            txtInverterDisconn.Enabled = Status;
            txtSolarPanelGPSModel.Enabled = Status;
            txtACServicePanel.Enabled = Status;
            txtGPStrackerNo.Enabled = Status;
            txtACMeterNo.Enabled = Status;
            txtMeterNo.Enabled = Status;
            txtInsurPartner.Enabled = Status;
            txtSumAssured.Enabled = Status;
            txtCoverageMaterial.Enabled = Status;
            txtFromCovPeriod.Enabled = Status;
            txtPolicyRefNo.Enabled = Status;
            txtToCovPeriod.Enabled = Status;
            txtPolicyNo.Enabled = Status;
        }

        #endregion
        #region LoanDetails
        protected void gvInsuChrgDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            CalculateTotalCost();

        }
        protected void gvInsuChrgDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkWaiver = (CheckBox)e.Row.FindControl("chkWaiver");

                if (e.Row.Cells[6].Text == "Y")
                {
                    chkWaiver.Checked = true;
                }
                else
                {
                    chkWaiver.Checked = false;
                }

            }
        }
        private void GenerateLoanAppDtls(Int64 LeadID)
        {
            DataSet ds = null;
            DataTable dt = null, dt1 = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                ds = oMem.GenerateCentral2LoanAppDtls(LeadID);
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
                if (dt.Rows.Count > 0)
                {
                    hdnLoanAppId.Value = Convert.ToString(dt.Rows[0]["LoanAppId"]);
                    txtCustomerRefNo.Text = Convert.ToString(dt.Rows[0]["CustRefNo"]);
                    ddlLoanScheme.SelectedIndex = ddlLoanScheme.Items.IndexOf(ddlLoanScheme.Items.FindByValue(dt.Rows[0]["LoanTypeId"].ToString()));
                    txtLoanAmount.Text = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                    hdnTotalLoanAmount.Value = Convert.ToString(dt.Rows[0]["LoanAmt"]);
                    txtTotalLoanAmount.Text = Convert.ToString(dt.Rows[0]["TotalLoanAmt"]);
                    txtMMR.Text = Convert.ToString(dt.Rows[0]["MarginMoneyAmount"]);
                    txtInsuranceOtherCharges.Text = Convert.ToString(dt.Rows[0]["TotalInsuOthrCharges"]);
                    txtInterestRate.Text = Convert.ToString(dt.Rows[0]["ROI"]);
                    txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]);
                    txtEMIAmount.Text = Convert.ToString(dt.Rows[0]["EMIAmount"]);
                    txtInterestType.Text = Convert.ToString(dt.Rows[0]["InterestType"]);
                    txtApplnDt.Text = Convert.ToString(dt.Rows[0]["ApplicationDt"]);
                    txtEMIStartDate.Text = Convert.ToString(dt.Rows[0]["StartDt"]);
                    txtEMIEndDate.Text = Convert.ToString(dt.Rows[0]["EndDt"]);
                    txtProcessingFee.Text = Convert.ToString(dt.Rows[0]["ProcessingFee"]);
                    hdnProcessingFee.Value = Convert.ToString(dt.Rows[0]["ProcessingFee"]);
                    txtPreEMI.Text = Convert.ToString(dt.Rows[0]["PreEMI"]);
                    hdnPreEMI.Value = Convert.ToString(dt.Rows[0]["PreEMI"]);
                    txtTotalCharges.Text = Convert.ToString(dt.Rows[0]["TotalChargeOnly"]);
                    txtTotalInsuranceCharges.Text = Convert.ToString(dt.Rows[0]["TotalInsuOnly"]);
                    txtAPRPFInsurance.Text = Convert.ToString(dt.Rows[0]["APRPFPlusInsu"]);
                    hdnAPRPFInsurance.Value = Convert.ToString(dt.Rows[0]["APRPFPlusInsu"]);
                    txtAPRPf.Text = Convert.ToString(dt.Rows[0]["APRPF"]);
                    hdnAPRPf.Value = Convert.ToString(dt.Rows[0]["APRPF"]);

                    txtTotalDeduction.Text = Convert.ToString(dt.Rows[0]["TotalInsuOthrCharges"]);
                    txtNupayReferenceNo.Text = Convert.ToString(dt.Rows[0]["NupayReferenceNo"]);
                    txtNACHRegistration.Text = Convert.ToString(dt.Rows[0]["NachUMRN"]);
                    txtDisburseAmount.Text = Convert.ToString(dt.Rows[0]["DisbAmt"]);
                    txtDisburseAmount.Text = Convert.ToString(txtDisburseAmount.Text) + "(" + Amtwords(Convert.ToDouble(txtDisburseAmount.Text)) + ")";
                    hdAppDate.Value = Convert.ToString(dt.Rows[0]["BasicAppDate"]);
                    if (dt.Rows[0]["SancStatusStage1"].ToString() != "")
                    {
                        ddlAction.SelectedValue = dt.Rows[0]["SancStatusStage1"].ToString();
                        if (ddlAction.SelectedValue == "Approve")
                        {
                            txtHoldRemarks.Enabled = false;
                        }
                        else
                        {
                            txtHoldRemarks.Enabled = true;
                        }
                    }
                    else
                    {
                        ddlAction.SelectedValue = "-1";
                    }
                    txtHoldRemarks.Text = Convert.ToString(dt.Rows[0]["SancRemarkStage1"]);

                }
                ViewState["Charges"] = dt1;
                gvInsuChrgDtl.DataSource = dt1;
                gvInsuChrgDtl.DataBind();
                CalculateTotalCost();
                StatusLoanButton("Edit");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public string Amtwords(double? numbers, Boolean paisaconversion = false)
        {
            var pointindex = numbers.ToString().IndexOf(".");
            var paisaamt = 0;
            if (pointindex > 0)
            {
                paisaamt = Convert.ToInt32(numbers.ToString().Substring(pointindex + 1));
                // Ensure paisaamt has two digits even if the paisa value has only one digit.
                if (paisaamt < 10) paisaamt *= 10;
            }

            int number = Convert.ToInt32(numbers);

            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";

            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }

            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }

            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens

                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }

            // Adding the paisa part
            if (paisaamt == 0 && !paisaconversion)
            {
                sb.Append("Rupees only");
            }
            else if (paisaamt > 0)
            {
                var paisatext = Amtwords(paisaamt, true);  // recursively call for paisa part.
                sb.AppendFormat("rupees {0}paise only", paisatext);
            }
            return sb.ToString().TrimEnd();
        }

        private void CalculateTotalCost()
        {
            decimal totalChargeCost = 0, totalGSTCost = 0, totalCost = 0,
             OnlyInsuCharges = 0, OnlyOtherCharges = 0, TotLoanAmtWC = 0, InsuOthrChargs = 0;



            foreach (GridViewRow gr in gvInsuChrgDtl.Rows)
            {
                if (gr.RowType == DataControlRowType.DataRow)
                {

                    Label lblChargeAmount = (Label)gr.FindControl("lblChargeAmount");
                    Label lblGSTAmount = (Label)gr.FindControl("lblGSTAmount");
                    Label lblTotalAmount = (Label)gr.FindControl("lblTotalAmount");
                    Label lblChargeID = (Label)gr.FindControl("lblChargeID");
                    CheckBox chkWaiver = (CheckBox)gr.FindControl("chkWaiver");

                    decimal rowChrgCost = 0, rowGSTCost = 0, rowTotCost = 0;



                    decimal.TryParse(lblChargeAmount.Text, out rowChrgCost); //parse the cost from the textbox
                    if (chkWaiver.Checked == false)
                    {
                        totalChargeCost += Math.Round(rowChrgCost, 2);

                    }


                    decimal.TryParse(lblGSTAmount.Text, out rowGSTCost); //parse the cost from the textbox
                    if (chkWaiver.Checked == false)
                    {
                        totalGSTCost += Math.Round(rowGSTCost, 2);
                    }

                    decimal.TryParse(lblTotalAmount.Text, out rowTotCost); //parse the cost from the textbox

                    if (chkWaiver.Checked == false)
                    {
                        totalCost += Math.Round(rowTotCost, 2);



                        if (lblChargeID.Text == "1" || lblChargeID.Text == "2")
                        {
                            OnlyInsuCharges = OnlyInsuCharges + rowTotCost;
                        }
                        if (lblChargeID.Text == "3" || lblChargeID.Text == "4" || lblChargeID.Text == "5" || lblChargeID.Text == "6" || lblChargeID.Text == "7" || lblChargeID.Text == "8")
                        {
                            OnlyOtherCharges = OnlyOtherCharges + rowTotCost;
                        }
                    }
                    else
                    {
                        // For Processing Fee
                        if (lblChargeID.Text == "7")
                        {
                            txtProcessingFee.Text = "0";
                            txtAPRPf.Text = txtInterestRate.Text;
                        }
                        else
                        {
                            txtProcessingFee.Text = hdnProcessingFee.Value;
                            txtAPRPf.Text = hdnAPRPf.Value;
                        }

                        //For Pre-EMI Cost
                        if (lblChargeID.Text == "5")
                        {
                            txtPreEMI.Text = "0";

                        }
                        else
                        {
                            txtPreEMI.Text = hdnPreEMI.Value;

                        }
                    }

                }


            }

            //TotLoanAmtWC = Convert.ToDecimal(hdnTotalLoanAmount.Value) + totalCost;
            //InsuOthrChargs = totalCost;

            //txtTotalInsuranceCharges.Text = Convert.ToString(OnlyInsuCharges);  // only Insurence Charges
            //txtTotalCharges.Text = Convert.ToString(OnlyOtherCharges); // only aLL other charges
            //txtTotalLoanAmount.Text = Convert.ToString(TotLoanAmtWC); // Loan Amt with insurence and Charges
            //txtInsuranceOtherCharges.Text = Convert.ToString(InsuOthrChargs);  // Insurence Charges + other charges
            //txtTotalDeduction.Text = Convert.ToString(InsuOthrChargs);
            //txtDisburseAmount.Text = Convert.ToString(TotLoanAmtWC) + "(" + ConvertToWords(TotLoanAmtWC) + ")";

            GridViewRow footerRow = gvInsuChrgDtl.FooterRow;
            if (footerRow != null)
            {
                Label lblTotChargeAmount = (Label)gvInsuChrgDtl.FooterRow.FindControl("lblTotChargeAmount");
                Label lblTotGSTAmount = (Label)gvInsuChrgDtl.FooterRow.FindControl("lblTotGSTAmount");
                Label lblTotAmount = (Label)gvInsuChrgDtl.FooterRow.FindControl("lblTotAmount");

                if (lblTotChargeAmount != null)
                {
                    if (totalChargeCost > 0)
                        lblTotChargeAmount.Text = totalChargeCost.ToString();
                    else
                        lblTotChargeAmount.Text = "0";
                }
                if (lblTotGSTAmount != null)
                {
                    if (totalGSTCost > 0)
                        lblTotGSTAmount.Text = totalGSTCost.ToString();
                    else
                        lblTotGSTAmount.Text = "0";
                }
                if (lblTotAmount != null)
                {
                    if (totalCost > 0)
                        lblTotAmount.Text = totalCost.ToString();
                    else
                        lblTotAmount.Text = "0";
                }

                lblTotChargeAmount.ForeColor = System.Drawing.Color.White;
                lblTotGSTAmount.ForeColor = System.Drawing.Color.White;
                lblTotAmount.ForeColor = System.Drawing.Color.White;
            }
            //upPnl.Update();
        }

        protected void chkWaiver_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelect = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkSelect.NamingContainer;

            string rowData = row.Cells[1].Text;

            CalculateTotalCost();
        }

        private void PopLoanScheme()
        {
            DataTable dt = null;
            CMember oMem = null;
            try
            {
                oMem = new CMember();
                dt = oMem.CF_GetLoanScheme();

                ddlLoanScheme.DataSource = dt;
                ddlLoanScheme.DataTextField = "LoanTypeName";
                ddlLoanScheme.DataValueField = "LoanTypeId";
                ddlLoanScheme.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlLoanScheme.Items.Insert(0, oli);

            }
            finally
            {
                oMem = null;
                dt = null;
            }
        }

     

        protected void btnLoanEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CanEdit == "N")
                {
                    gblFuction.MsgPopup(MsgAccess.Edit);
                    return;
                }

                StatusLoanButton("Edit");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnLoanSave_Click(object sender, EventArgs e)
        {
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            if (vStateEdit == "Add" || vStateEdit == null)
                vStateEdit = "Save";
            if (SaveLoanRecords(vStateEdit) == true)
            {
                gblFuction.AjxMsgPopup(gblPRATAM.SaveMsg);

                GenerateLoanAppDtls(Convert.ToInt64(hdLeadId.Value));
                LoadBasicDetailsList(1);
                StatusLoanButton("Edit");
                ViewState["StateEdit"] = null;

            }
        }
        private Boolean SaveLoanRecords(string Mode)
        {
            Boolean vResult = false; Int32 vErr = 0;
            DataTable dt = new DataTable();

            string vSacRemakrs = "", vBrCode = "", vSacStatus = "", vLoanAppId = "",
                vErrMsg = "";



            Int64 vLeadId = 0;
            CMember oMem = null;

            try
            {


                if (hdLeadId.Value != "")
                {
                    vLeadId = Convert.ToInt64(hdLeadId.Value);
                }
                Int32 UID = Convert.ToInt32(Session[gblValue.UserId]);

                if (ViewState["BCode"] != null)
                {
                    hdnBrCode.Value = ViewState["BCode"].ToString();
                }
                vBrCode = hdnBrCode.Value;


                vSacStatus = Convert.ToString(ddlAction.SelectedValue);
                vSacRemakrs = Convert.ToString(txtHoldRemarks.Text);
                vLoanAppId = Convert.ToString(hdnLoanAppId.Value);


                oMem = new CMember();
                vErr = oMem.CF_SaveOpsCentralStage2(vLeadId, vLoanAppId, vBrCode, vSacStatus, vSacRemakrs, UID, 0, ref vErrMsg);


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
        private Boolean GetChargesData()
        {
            DataTable dt = (DataTable)ViewState["Charges"];
            foreach (GridViewRow gr in gvInsuChrgDtl.Rows)
            {
                Label lblChargeID = (Label)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("lblChargeID");
                Label lblChargeType = (Label)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("lblChargeType");
                Label lblChargeAmount = (Label)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("lblChargeAmount");
                Label lblGSTAmount = (Label)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("lblGSTAmount");
                CheckBox chkWaiver = (CheckBox)gvInsuChrgDtl.Rows[gr.RowIndex].FindControl("chkWaiver");


                if (chkWaiver.Checked)
                {
                    dt.Rows[gr.RowIndex]["Waiver"] = "Y";

                }
                else
                {
                    dt.Rows[gr.RowIndex]["Waiver"] = "N";
                }

                //Inserting data into datatable dt
                dt.Rows[gr.RowIndex]["ChargeID"] = Convert.ToInt32(lblChargeID.Text == "" ? "" : lblChargeID.Text);
                dt.Rows[gr.RowIndex]["ChargeType"] = Convert.ToString(lblChargeType.Text == "" ? "" : lblChargeType.Text);
                dt.Rows[gr.RowIndex]["ChargeAmount"] = Convert.ToString(lblChargeAmount.Text == "" ? "" : lblChargeAmount.Text);
                dt.Rows[gr.RowIndex]["GSTAmount"] = Convert.ToString(lblGSTAmount.Text == "" ? "" : lblGSTAmount.Text);
                dt.Rows[gr.RowIndex]["AddedBy"] = "BC – BOE / BM Stage";


            }
            dt.AcceptChanges();
            ViewState["Charges"] = dt;
            gvInsuChrgDtl.DataSource = dt;
            gvInsuChrgDtl.DataBind();
            return true;
        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {

            GenerateScheduleGrid();

        }

        protected void ViewSchedulePop()
        {
            GenerateScheduleGrid();

        }

        private void GenerateScheduleGrid()
        {
            DataSet ds = null;
            DataTable dt = null;
            CMember oMem = null;

            double vLoanAmt = 0, vInterest = 0, vInstallNo = 0;
            string vType = "S", vLoanID = "", vIsDisburse = "N";

            DateTime vLoanDt = gblFuction.setDate(txtApplnDt.Text.ToString());
            DateTime vStartDt = gblFuction.setDate(txtEMIStartDate.Text.ToString());

            vLoanAmt = Convert.ToDouble(txtTotalLoanAmount.Text);
            vInterest = Convert.ToDouble(txtInterestRate.Text);
            vInstallNo = Convert.ToDouble(txtTenure.Text);

            try
            {
                oMem = new CMember();
                ds = oMem.GenerateScheduleGrid(vLoanAmt, vInterest, vInstallNo, vLoanDt, vStartDt, vType, vLoanID, vIsDisburse);
                hdnLoanFlag.Value = "Y";
                dt = ds.Tables[0];

                ViewState["Schedule"] = dt;
                gvScheduleDtls.DataSource = dt;
                gvScheduleDtls.DataBind();



                if (hdnLoanFlag.Value == "Y")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "popUpSchedule();", true);
                }

            }

            finally
            {
            }
        }


        protected void btnLoanCancel_Click(object sender, EventArgs e)
        {
            btnLoanSave.Enabled = false;
            btnLoanCancel.Enabled = false;
            btnLoanEdit.Enabled = true;

            GenerateLoanAppDtls(Convert.ToInt64(hdLeadId.Value));

            // EnableHypoControl(false);
        }

        private void StatusLoanButton(String pMode)
        {
            switch (pMode)
            {
                case "Show":

                    btnLoanEdit.Enabled = true;
                    btnLoanSave.Enabled = false;
                    btnLoanCancel.Enabled = false;
                    break;
                case "Edit":

                    btnLoanEdit.Enabled = false;
                    btnLoanSave.Enabled = true;
                    btnLoanCancel.Enabled = true;
                    break;
            }
        }

        private void EnableLoanControl(Boolean Status)
        {
            txtEMIStartDate.Enabled = Status;
            txtNupayReferenceNo.Enabled = Status;

            gvInsuChrgDtl.Enabled = Status;
        }

        protected void txtEMIStartDate_textChanged(object sender, EventArgs e)
        {
            GetScheduleEndDt();
        }

        protected void GetScheduleEndDt()
        {
            DataTable dt = null;
            CMember oMem = null;
            Int32 vInstallNo = 0;

            DateTime vStartDt = gblFuction.setDate(txtEMIStartDate.Text.ToString());

            if (txtTenure.Text == "") txtTenure.Text = "0";
            vInstallNo = Convert.ToInt32(txtTenure.Text);

            oMem = new CMember();
            dt = oMem.GetScheduleEndDt(vInstallNo, vStartDt);

            if (dt.Rows.Count > 0)
            {
                txtEMIEndDate.Text = Convert.ToString(dt.Rows[0]["Enddate"]);
            }
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hdTabIndex.Value == "10")
            {
                ddlActionCheck();
            }
        }

        private bool ddlActionCheck()
        {
            Boolean vResult = true;
            //if (ddlAction.SelectedValue == "-1")
            //{
            //    gblFuction.AjxMsgPopup("Must select Saction Remarks...");
            //    vResult = false;
            //}
            if (ddlAction.SelectedValue == "Approve")
            {
                txtHoldRemarks.Enabled = false;
                txtHoldRemarks.Text = "";
                vResult = true;
            }
            else
            {
                if (txtHoldRemarks.Text.Trim() == "")
                {
                    txtHoldRemarks.Text = "";
                    txtHoldRemarks.Enabled = true;
                   // gblFuction.AjxMsgPopup("Please Enter Remarks ...");
                    vResult = false;
                }
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
        protected void btnSendBack_Click(object sender, EventArgs e)
        {
            SendBkLoanApp();
        }
        private Boolean SendBkLoanApp()
        {
            Boolean vResult = false;
            Int32 vErr = 0;

            Int64 vLeadID = 0; string vSendBkRemarks = "", vErrMsg = "";

            vSendBkRemarks = Convert.ToString(txtSendBack.Text);
            vLeadID = Convert.ToInt64(hdLeadId.Value);
            Int32 UID = Convert.ToInt32(Session[gblValue.UserId]);

            CMember oMem = null;
            try
            {

                oMem = new CMember();
                vErr = oMem.CF_SendBkStageTwo(vLeadID, vSendBkRemarks, UID, 0, ref vErrMsg);
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
    }
}