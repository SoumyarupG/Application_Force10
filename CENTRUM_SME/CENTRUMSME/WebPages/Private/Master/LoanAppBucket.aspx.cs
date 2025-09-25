using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using CENTRUMBA;
using CENTRUMCA;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using System.Net;
using System.Xml;
using System.Text;
using System.Web;
using PRATAM.Service_Equifax;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class LoanAppBucket : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string pathImage = "", PathKYCImage = "", CustomerId = "", CCRUserName = "", CCRPassword = "", PCSUserName = "", PCSPassword = "";
        string MinioYN = ConfigurationManager.AppSettings["MinioYN"];
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string MemberBucket = ConfigurationManager.AppSettings["MemberBucket"];

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                DOFCom.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                hdUserID.Value = this.UserID.ToString();
                hdnQaYN.Value = "N";
                mView.ActiveViewIndex = 0;
                GetPendCompanyList();
                LoadCompanyList();
                popSpeciallyAbled();
                StatusButton("View");
            }
            else
            {
            }
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
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Application Bucket";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuLnAppBucket);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAppAdd.Visible = false;
                    btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y")
                {
                    btnAppAdd.Visible = true;
                    btnAppEdit.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAppAdd.Visible = true;
                    btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit == "Y" && this.CanDelete != "Y")
                {
                    btnAppAdd.Visible = false;
                    btnAppEdit.Visible = true;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application Bucket", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
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
        private void ViewAcess()
        {
            if (mView.ActiveViewIndex == 0)
            {
                this.Menu = false;
                this.PageHeading = "Customer Master";
                this.GetModuleByRole(mnuID.mnuApplicant);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 1)
            {
                this.Menu = false;
                this.PageHeading = "Loan Application";
                this.GetModuleByRole(mnuID.mnuAppLnApplication);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 2)
            {
                this.Menu = false;
                this.PageHeading = "Co Applicant Information";
                this.GetModuleByRole(mnuID.mnuCoApplicant);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 3)
            {
                this.Menu = false;
                this.PageHeading = "Applicant Information";
                this.GetModuleByRole(mnuID.mnuCompany);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 4)
            {
                this.Menu = false;
                this.PageHeading = "Credit Bureau Information";
                this.GetModuleByRole(mnuID.mnuCB);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 5)
            {
                this.Menu = false;
                this.PageHeading = "Applicant Bank Account Information";
                this.GetModuleByRole(mnuID.mnuBankAC);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 6)
            {
                this.Menu = false;
                this.PageHeading = "Balance Sheet Information";
                this.GetModuleByRole(mnuID.mnuBSStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 7)
            {
                this.Menu = false;
                this.PageHeading = "Profit And Loss Information";
                this.GetModuleByRole(mnuID.mnuPLStatement);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 8)
            {
                this.Menu = false;
                this.PageHeading = "Reference Information";
                this.GetModuleByRole(mnuID.mnuReference);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 9)
            {
                this.Menu = false;
                this.PageHeading = "CAM Details";
                this.GetModuleByRole(mnuID.mnuCAM);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 10)
            {
                this.Menu = false;
                this.PageHeading = "Company Profile Background Information";
                this.GetModuleByRole(mnuID.mnuComBackground);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 11)
            {
                this.Menu = false;
                this.PageHeading = "Promoter Background Information";
                this.GetModuleByRole(mnuID.mnuPromoBackground);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 12)
            {
                this.Menu = false;
                this.PageHeading = "CIBIL Check & Internal Check Details";
                this.GetModuleByRole(mnuID.mnuCheckList);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
            else if (mView.ActiveViewIndex == 13)
            {
                this.Menu = false;
                this.PageHeading = "Information of Group Companies";
                this.GetModuleByRole(mnuID.mnuGrpComBackInfo);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
            }
        }
        private DataTable GetRefDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("RefName", typeof(string));
            dt.Columns.Add("RefAddr", typeof(string));
            dt.Columns.Add("RefMNo", typeof(string));
            dt.Columns.Add("RelWithAppId", typeof(int));
            dt.Columns.Add("RelWithApp", typeof(string));
            dt.Columns.Add("OffTelNo", typeof(string));
            dt.Columns.Add("CompName", typeof(string));
            dt.Columns.Add("PersonalRef", typeof(string));
            dt.Columns.Add("TradeRef", typeof(string));
            dt.Columns.Add("RefType", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("VarifiedBy", typeof(string));
            return dt;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadCompanyList();
        }
        private void LoadRefList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetRefList(pApplicationId);

                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvRefList.DataSource = dt1;
                        gvRefList.DataBind();
                    }
                    else
                    {
                        gvRefList.DataSource = null;
                        gvRefList.DataBind();
                    }
                }
                else
                {
                    gvRefList.DataSource = null;
                    gvRefList.DataBind();
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
        private void StatusButton(String pMode)
        {
            switch (pMode)
            {
                case "Add":
                    EnableControl(true);
                    btnAppAdd.Enabled = false;
                    btnAppEdit.Enabled = false;
                    btnExit.Enabled = false;
                    // ClearControls();
                    break;
                case "Show":
                    btnAppAdd.Enabled = false;
                    btnAppEdit.Enabled = true;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Edit":
                    btnAppAdd.Enabled = false;
                    btnAppEdit.Enabled = true;
                    btnExit.Enabled = false;
                    EnableControl(true);
                    break;
                case "View":
                    btnAppAdd.Enabled = true;
                    btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Delete":
                    btnAppAdd.Enabled = true;
                    btnAppEdit.Enabled = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
                case "Exit":
                    btnAppAdd.Visible = false;
                    btnAppEdit.Visible = false;
                    btnExit.Enabled = true;
                    EnableControl(false);
                    break;
            }
        }
        private void ShowApplicantData(string pAppId, string vBrCode)
        {
            DataTable dt1 = null, dt2 = null;
            DataSet ds = null;
            hdfAppIdShow.Value = "";
            CMember oMem = null;
            // string vEoId = "", vIndv = "", vAdd = "", vDrop = "", vGrpId = "", vMarketID = "";
            try
            {
                oMem = new CMember();

                if (pAppId == "-1") return;
                ds = oMem.GetMemberDetails(pAppId, vBrCode);
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                if (dt1.Rows.Count > 0)
                {
                    tbMem.ActiveTabIndex = 1;
                    btnAppAdd.Enabled = false;
                    ViewState["StateEdit"] = "Edit";
                    ViewState["CusTID"] = Convert.ToString(dt1.Rows[0]["CustId"]);
                    hdfAppIdShow.Value = Convert.ToString(dt1.Rows[0]["CustId"]).Trim();
                    StatusButton("Show");
                }
                if (dt2.Rows.Count > 0)
                {
                    gvCoApp.DataSource = dt2;
                    gvCoApp.DataBind();
                }

                DataTable dt = new DataTable();
                dt = oMem.GetLoanAppNoByCustid(ViewState["CusTID"].ToString());
                ViewState["LoanAppId"] = dt.Rows[0]["LoanAppId"].ToString();
                string vLoanAppId = ViewState["LoanAppId"].ToString();
                // get initial loan application
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                // get reference details
                LoadRefList(vLoanAppId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                dt1 = null;
                dt2 = null;

                ds = null;
            }
        }
        private void ShowCompanyData(string pAppId, string vBrCode)
        {
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null;
            DataSet ds = null;
            hdfAppIdShow.Value = "";
            CMember oMem = null;
            try
            {
                oMem = new CMember();

                if (pAppId == "-1") return;
                ds = oMem.ShowCompanyDetails(pAppId, vBrCode, Convert.ToInt32(Session[gblValue.UserId]));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        tbMem.ActiveTabIndex = 2;
                        btnAppAdd.Enabled = false;
                        ViewState["StateEdit"] = "Edit";
                        ViewState["CusTID"] = Convert.ToString(dt1.Rows[0]["CustId"]);
                        hdfAppIdShow.Value = Convert.ToString(dt1.Rows[0]["CustId"]).Trim();
                        txtCustTypeShow.Text = Convert.ToString(dt1.Rows[0]["CustType"]).Trim();
                        txtDOFShow.Text = Convert.ToString(dt1.Rows[0]["DOF"]);
                        txtDOBShow.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                        txtAppNoShow.Text = Convert.ToString(dt1.Rows[0]["CustId"]);
                        txtComNameShow.Text = Convert.ToString(dt1.Rows[0]["CompanyName"]).Trim();
                        lblComType.Text = Convert.ToString(dt1.Rows[0]["CpmTypeName"]).Trim();
                        txtAppAddresIdTypeShow.Text = Convert.ToString(dt1.Rows[0]["AddressID"]).Trim();

                        if (Convert.ToString(dt1.Rows[0]["AddressID"]).Trim() == "Aadhar")
                        {
                            txtAppAddIdNoForShow.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Length - 4, 4));
                        }
                        else
                        {
                            txtAppAddIdNoForShow.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim();
                        }

                        txtPropertyTypeShow.Text = Convert.ToString(dt1.Rows[0]["PropertypeName"]).Trim();
                        lblOtherPropType.Text = Convert.ToString(dt1.Rows[0]["OtherPropertyType"]).Trim();
                        lblWebsite.Text = Convert.ToString(dt1.Rows[0]["Website"]).Trim();
                        txtEmailShow.Text = Convert.ToString(dt1.Rows[0]["Email"]).Trim();
                        txtPANShow.Text = Convert.ToString(dt1.Rows[0]["PANNo"]).Trim().ToUpper();
                        if (dt1.Rows[0]["IsRegistered"].ToString() == "Y")
                        {
                            chkIsReg.Checked = true;
                        }
                        else
                        {
                            chkIsReg.Checked = false;
                        }
                        lblRegistrationNo.Text = Convert.ToString(dt1.Rows[0]["RegistrationNo"]).Trim();
                        lblSector.Text = Convert.ToString(dt1.Rows[0]["Sector"]).Trim();
                        lblSubSector.Text = Convert.ToString(dt1.Rows[0]["SubSector"]).Trim();
                        txtAppnameShow.Text = Convert.ToString(dt1.Rows[0]["ApplicantName"]).Trim();
                        txtAppContNoShow.Text = Convert.ToString(dt1.Rows[0]["AppContactNo"]).Trim();

                        txtMAddress1Show.Text = Convert.ToString(dt1.Rows[0]["MAddress1"]).Trim();
                        txtMAddress2Show.Text = Convert.ToString(dt1.Rows[0]["MAddress2"]).Trim();
                        txtMStateShow.Text = Convert.ToString(dt1.Rows[0]["MState"]).Trim();
                        txtMDistrictShow.Text = Convert.ToString(dt1.Rows[0]["MDistrict"]).Trim();
                        txtMMobNoShow.Text = Convert.ToString(dt1.Rows[0]["MMobNo"]).Trim();
                        txtMSTDShow.Text = Convert.ToString(dt1.Rows[0]["MSTD"]).Trim();
                        txtMTelShow.Text = Convert.ToString(dt1.Rows[0]["MTelNo"]).Trim();
                        txtMPINShow.Text = Convert.ToString(dt1.Rows[0]["MPIN"]).Trim();
                        txtRAddress1Show.Text = Convert.ToString(dt1.Rows[0]["RAddress1"]).Trim();
                        txtRAddress2Show.Text = Convert.ToString(dt1.Rows[0]["RAddress2"]).Trim();
                        txtRStateShow.Text = Convert.ToString(dt1.Rows[0]["RState"]).Trim();
                        txtRDistrictShow.Text = Convert.ToString(dt1.Rows[0]["RDistrict"]).Trim();
                        txtRMobNoShow.Text = Convert.ToString(dt1.Rows[0]["RMobNo"]).Trim();
                        txtRSTDShow.Text = Convert.ToString(dt1.Rows[0]["RSTD"]).Trim();
                        txtRTelNoShow.Text = Convert.ToString(dt1.Rows[0]["RTelNo"]).Trim();
                        txtRPINShow.Text = Convert.ToString(dt1.Rows[0]["RPIN"]).Trim();
                        lblGSTRegistrationNo.Text = Convert.ToString(dt1.Rows[0]["GSTRegNo"]).Trim();

                        txtAppPhotoIdTypeShow.Text = Convert.ToString(dt1.Rows[0]["PhotoId"]).Trim();

                        if (Convert.ToString(dt1.Rows[0]["PhotoId"]).Trim() == "Aadhar")
                        {
                            txtAppPhotoIdNoForShow.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Length - 4, 4));
                        }
                        else
                        {
                            txtAppPhotoIdNoForShow.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim();
                        }

                        txtGenShow.Text = Convert.ToString(dt1.Rows[0]["Gender"]).Trim();
                        txtOccShow.Text = Convert.ToString(dt1.Rows[0]["Occupation"]).Trim();
                        txtAnnIncShow.Text = Convert.ToString(dt1.Rows[0]["AnnualIncome"]);
                        txtBusAddShow.Text = Convert.ToString(dt1.Rows[0]["BusinessLocation"]);
                        txtBusTypeShow.Text = Convert.ToString(dt1.Rows[0]["BusinessTypeName"]);
                        txtAgeShow.Text = Convert.ToString(dt1.Rows[0]["Age"]);
                        txtCustReltiveShow.Text = Convert.ToString(dt1.Rows[0]["RelativeName"]);
                        txtCustRelShow.Text = Convert.ToString(dt1.Rows[0]["Relation"]);
                        StatusButton("Show");
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        gvCoApp.DataSource = dt2;
                        gvCoApp.DataBind();
                    }
                    else
                    {
                        gvCoApp.DataSource = null;
                        gvCoApp.DataBind();

                    }
                }
                // get initial loan application
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                dt1 = null;
                dt2 = null;
                ds = null;
            }
        }
        private void ShowCoAppData(string pCoAppId, string pBrCode)
        {
            DataTable dt1 = null, dt2 = null;
            DataSet ds = null;
            CMember oMem = new CMember();
            try
            {
                ClearCoApplicant();
                if (pCoAppId == "-1") return;
                ds = oMem.GetCoAppDetails(pCoAppId, pBrCode, Convert.ToInt32(Session[gblValue.UserId]));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                }
                dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 2;
                    // ViewAcess();
                    btnSaveCoApp.Enabled = false;
                    btnUpdateCoApp.Enabled = true;
                    //btnBackApp.Enabled = true;
                    hdfCoAppID.Value = Convert.ToString(dt1.Rows[0]["CoApplicantId"]);
                    PopCustomer();
                    ddlCustName.SelectedIndex = ddlCustName.Items.IndexOf(ddlCustName.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CustId"])));
                    txtCoAppDOA.Text = Convert.ToString(dt1.Rows[0]["DOA"]);
                    txtCoAppName.Text = Convert.ToString(dt1.Rows[0]["CoAppName"]);
                    txtCoAppNo.Text = Convert.ToString(dt1.Rows[0]["CoApplicantNo"]);
                    popIDProof();
                    ddlCoAppPhotoIdType.SelectedIndex = ddlCoAppPhotoIdType.Items.IndexOf(ddlCoAppPhotoIdType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PhotoId"])));
                    if (Convert.ToInt32(dt1.Rows[0]["PhotoId"]) == 6)
                    {
                        txtCoAppPNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Length - 4, 4));
                        hdnCoApplAadharNo.Value = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    }
                    else
                    {
                        txtCoAppPNo.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    }
                    ddlCoAppAddressIdType.SelectedIndex = ddlCoAppAddressIdType.Items.IndexOf(ddlCoAppAddressIdType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AddressId"])));
                    if (Convert.ToInt32(dt1.Rows[0]["AddressId"]) == 6)
                    {
                        txtCoAppAddressIdNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Length - 4, 4));
                        hdnCoApplAadharNo.Value = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    }
                    else
                    {
                        txtCoAppAddressIdNo.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    }

                    //ddlCoAppITTypeId.SelectedIndex = ddlCoAppITTypeId.Items.IndexOf(ddlCoAppITTypeId.Items.FindByValue(Convert.ToString(dt1.Rows[0]["ITTypeId"])));
                    //txtCoAppITNo.Text = Convert.ToString(dt1.Rows[0]["ITTypeNo"]);
                    txtCoAppTelNo.Text = Convert.ToString(dt1.Rows[0]["ContTelNo"]);
                    PopMaritalStatus();
                    ddlCoAppMS.SelectedIndex = ddlCoAppMS.Items.IndexOf(ddlCoAppMS.Items.FindByValue(Convert.ToString(dt1.Rows[0]["MaritalStatus"])));
                    txtCoAppMNo.Text = Convert.ToString(dt1.Rows[0]["ContMNo"]);
                    PopOccupation();
                    ddlCoAppOccu.SelectedIndex = ddlCoAppOccu.Items.IndexOf(ddlCoAppOccu.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId"])));
                    ddlCoAppOccu2.SelectedIndex = ddlCoAppOccu2.Items.IndexOf(ddlCoAppOccu2.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId2"])));
                    ddlCoAppOccu3.SelectedIndex = ddlCoAppOccu3.Items.IndexOf(ddlCoAppOccu3.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId3"])));

                    txtCABusinessName.Text = Convert.ToString(dt1.Rows[0]["BusinessName"]);
                    txtCoAppFaxNo.Text = Convert.ToString(dt1.Rows[0]["ContFAXNo"]);
                    txtCoAppEmail.Text = Convert.ToString(dt1.Rows[0]["Email"]);
                    PopGender();
                    ddlCoAppGender.SelectedIndex = ddlCoAppGender.Items.IndexOf(ddlCoAppGender.Items.FindByValue(Convert.ToString(dt1.Rows[0]["Gender"])));
                    txtCoAppAge.Text = Convert.ToString(dt1.Rows[0]["Age"]);
                    txtCoAppDOB.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                    PopQualification();
                    ddlCoAppQuali.SelectedIndex = ddlCoAppQuali.Items.IndexOf(ddlCoAppQuali.Items.FindByValue(Convert.ToString(dt1.Rows[0]["Qualification"])));
                    PopReligion();
                    ddlCoAppRel.SelectedIndex = ddlCoAppRel.Items.IndexOf(ddlCoAppRel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["ReligionId"])));
                    PopCaste();
                    PopRelation();
                    popState();
                    ddlCustCARel.SelectedIndex = ddlCustCARel.Items.IndexOf(ddlCustCARel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CustCoAppRel"])));
                    ddlCoAppCast.SelectedIndex = ddlCoAppCast.Items.IndexOf(ddlCoAppCast.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CastId"])));
                    txtCoAppYratRes.Text = Convert.ToString(dt1.Rows[0]["YearAtRes"]);
                    txtCoAppYrinBusiness.Text = Convert.ToString(dt1.Rows[0]["YearAtBus"]);

                    ddlCARel.SelectedIndex = ddlCARel.Items.IndexOf(ddlCARel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelationId"])));
                    txtCARelName.Text = Convert.ToString(dt1.Rows[0]["RelativeName"]);
                    //if (dt1.Rows[0]["IsDirector"].ToString() == "Y")
                    //{
                    //    ddlCoAppDirePart.SelectedValue = "D";
                    //}
                    //else if (dt1.Rows[0]["IsPartner"].ToString() == "Y")
                    //{
                    //    ddlCoAppDirePart.SelectedValue = "P";
                    //}
                    //else if (dt1.Rows[0]["IsPropietor"].ToString() == "Y")
                    //{
                    //    ddlCoAppDirePart.SelectedValue = "R";
                    //}
                    //else if (dt1.Rows[0]["IsSpouse"].ToString() == "Y")
                    //{
                    //    ddlCoAppDirePart.SelectedValue = "S";
                    //}
                    //else
                    //{
                    //    ddlCoAppDirePart.SelectedValue = "N";
                    //}
                    if (dt1.Rows[0]["IsGuarantor"].ToString() == "Y")
                    {
                        chkIsGuarantor.Checked = true;
                    }
                    if (dt1.Rows[0]["IsPrimaryCoAppYN"].ToString() == "Y")
                    {
                        chkIsPrimaryCoApp.Checked = true;
                    }
                    else
                    {
                        chkIsPrimaryCoApp.Checked = false;
                    }
                    if (dt1.Rows[0]["IsNominee"].ToString() == "Y")
                    {
                        chkIsNominee.Checked = true;
                    }
                    else
                    {
                        chkIsNominee.Checked = false;
                    }
                    if (dt1.Rows[0]["SameAddYN"].ToString() == "Y")
                    {
                        chkCoAppAd.Checked = true;
                    }
                    if (dt1.Rows[0]["IsSameAddAsApp"].ToString() == "Y")
                    {
                        chkCoAppSameAdd.Checked = true;
                    }
                    if (dt1.Rows[0]["IsActive"].ToString() == "Y")
                    {
                        chkIsActive.Checked = true;
                    }
                    txtShareHold.Text = dt1.Rows[0]["ShareHolPer"].ToString();
                    txtCoAppAddress1Pre.Text = Convert.ToString(dt1.Rows[0]["PreAddress1"]).Trim();
                    txtCoAppAddress2Pre.Text = Convert.ToString(dt1.Rows[0]["PreAddress2"]).Trim();
                    ddlCoAppStatePre.SelectedIndex = ddlCoAppStatePre.Items.IndexOf(ddlCoAppStatePre.Items.FindByText(Convert.ToString(dt1.Rows[0]["PreState"])));


                    //txtCoAppStatePre.Text = Convert.ToString(dt1.Rows[0]["PreState"]).Trim();
                    txtCoAppDistPre.Text = Convert.ToString(dt1.Rows[0]["PreDistrict"]).Trim();
                    txtCOAppPINPre.Text = Convert.ToString(dt1.Rows[0]["PrePIN"]).Trim();
                    txtCoAppAddress1Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress1"]).Trim();
                    txtCoAppAddress2Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress2"]).Trim();
                    txtCoAppDistPer.Text = Convert.ToString(dt1.Rows[0]["PerDistrict"]).Trim();
                    ddlCoAppStatePer.SelectedIndex = ddlCoAppStatePer.Items.IndexOf(ddlCoAppStatePer.Items.FindByText(Convert.ToString(dt1.Rows[0]["PerState"])));
                    //txtCoAppStatePer.Text = Convert.ToString(dt1.Rows[0]["PerState"]).Trim();
                    txtCoAppPINPer.Text = Convert.ToString(dt1.Rows[0]["PerPIN"]).Trim();
                    //  ddlCoAppType.SelectedValue = Convert.ToString(dt1.Rows[0]["CoAppType"]).Trim();
                    ddlCoAppType.SelectedIndex = ddlCoAppType.Items.IndexOf(ddlCoAppType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CoAppType"])));
                    PopCoAppCompanyType();
                    PopCoAppPropertyType();
                    ddlCoAppComType.SelectedIndex = ddlCoAppComType.Items.IndexOf(ddlCoAppComType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CompTypeID"])));
                    ddlCoAppProType.SelectedIndex = ddlCoAppProType.Items.IndexOf(ddlCoAppProType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PropertyTypeId"])));
                    txtApp.Text = dt1.Rows[0]["AppName"].ToString();

                    PopBankAcType();

                    ddlCAPhyChallenge.SelectedIndex = ddlCAPhyChallenge.Items.IndexOf(ddlCAPhyChallenge.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PhyChallangeYN"])));
                    txtCAACHoldname.Text = Convert.ToString(dt1.Rows[0]["ACHolderName"]);
                    txtCABankName.Text = Convert.ToString(dt1.Rows[0]["BankName"]);
                    txtCAAccNo.Text = Convert.ToString(dt1.Rows[0]["ACNo"]);
                    txtCAIFSCCode.Text = Convert.ToString(dt1.Rows[0]["IFSCCode"]);
                    txtCAAcYrOfOpen.Text = Convert.ToString(dt1.Rows[0]["YrOfOpening"]);
                    ddlCAACType.SelectedIndex = ddlCustACType.Items.IndexOf(ddlCustACType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AccountType"])));

                    //EMPLOYMENT DETAILS
                    txtCAOrgname.Text = Convert.ToString(dt1.Rows[0]["EmpOrgName"]);
                    txtCADesig.Text = Convert.ToString(dt1.Rows[0]["EmpDesig"]);
                    txtCARetiredAge.Text = Convert.ToString(dt1.Rows[0]["EmpRetiredAge"]);
                    txtCADeptEmpCode.Text = Convert.ToString(dt1.Rows[0]["EmpCode"]);
                    txtCACurExp.Text = Convert.ToString(dt1.Rows[0]["EmpCurExp"]);
                    txtCATotExp.Text = Convert.ToString(dt1.Rows[0]["EmpTotExp"]);

                    //BUSINESS DETAILS
                    if (dt1.Rows[0]["BusGSTAppYN"].ToString() == "Y")
                        chkGSTApp.Checked = true;
                    else
                        chkGSTApp.Checked = false;
                    txtCAGSTNo.Text = Convert.ToString(dt1.Rows[0]["BusGSTNo"]);
                    txtCABusLandmark.Text = Convert.ToString(dt1.Rows[0]["BusLandMark"]);

                    txtCABusAdd1.Text = Convert.ToString(dt1.Rows[0]["BusAddress1"]);
                    txtCABusAdd2.Text = Convert.ToString(dt1.Rows[0]["BusAddress2"]);
                    txtCABusLocality.Text = Convert.ToString(dt1.Rows[0]["BusLocality"]);
                    txtCABusCity.Text = Convert.ToString(dt1.Rows[0]["BusCity"]);
                    txtCABusPin.Text = Convert.ToString(dt1.Rows[0]["BusPIN"]);
                    txtCABusState.Text = Convert.ToString(dt1.Rows[0]["BusState"]);
                    txtCABusMob.Text = Convert.ToString(dt1.Rows[0]["BusMob"]);
                    txtCABusPh.Text = Convert.ToString(dt1.Rows[0]["BusPhone"]);

                }
                if (dt2.Rows.Count > 0)
                {
                    gvCADep.DataSource = dt2;
                    gvCADep.DataBind();
                }
                else
                {
                    GetCADependentDetails();
                }

                if (Convert.ToString(dt1.Rows[0]["IdVerifyYN"]) == "Y")
                {
                    ddlCoAppAddressIdType.Enabled = false;
                    txtCoAppAddressIdNo.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                ds = null;
                oMem = null;
            }
        }

        private void ShowCompanyDetails(string pCompanyId, string pBrCode)
        {
            DataTable dt1 = null, dt2 = null;
            DataSet ds = null;
            CMember oMem = new CMember();
            try
            {
                ClearCompany();
                if (pCompanyId == "-1") return;
                ds = oMem.GetCompanyDetails(pCompanyId, pBrCode, Convert.ToInt32(Session[gblValue.UserId]));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                }
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 3;
                    //ViewAcess();
                    btnSaveCom.Enabled = false;
                    btnUpdateCom.Enabled = true;
                    btnDeleteCom.Enabled = true;
                    btnBackCom.Enabled = true;
                    int vSectorid = Convert.ToInt32(dt1.Rows[0]["SectorId"]);
                    hdfComId.Value = Convert.ToString(dt1.Rows[0]["CustId"]);
                    txtCustNoApp.Text = Convert.ToString(dt1.Rows[0]["CustId"]);
                    rbComType.SelectedValue = Convert.ToString(dt1.Rows[0]["CustType"]);
                    DOFCom.Text = Convert.ToString(dt1.Rows[0]["DOF"]);
                    txtCustDOB.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                    txtCustAge.Text = Convert.ToString(dt1.Rows[0]["Age"]);
                    txtComName.Text = Convert.ToString(dt1.Rows[0]["CompanyName"]);
                    PopGender();

                    popQualification();
                    PopReligion();
                    PopCaste();
                    popState();
                    ddlCustGen.SelectedIndex = ddlCustGen.Items.IndexOf(ddlCustGen.Items.FindByValue(Convert.ToString(dt1.Rows[0]["GenderId"])));
                    PopCompanyType();
                    ddlComType.SelectedIndex = ddlComType.Items.IndexOf(ddlComType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CompanyTypeID"])));
                    PopPropertyType();
                    ddlComProType.SelectedIndex = ddlComProType.Items.IndexOf(ddlComProType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PropertyTypeId"])));
                    txtOtherProperDtl.Text = Convert.ToString(dt1.Rows[0]["OtherPropertyType"]);
                    PopSector();
                    ddlComSec.SelectedIndex = ddlComSec.Items.IndexOf(ddlComSec.Items.FindByValue(Convert.ToString(dt1.Rows[0]["SectorId"])));
                    PopSubSector(vSectorid);
                    ddlComSubSec.SelectedIndex = ddlComSubSec.Items.IndexOf(ddlComSubSec.Items.FindByValue(Convert.ToString(dt1.Rows[0]["SubSectorId"])));
                    txtComWebSite.Text = Convert.ToString(dt1.Rows[0]["Website"]);
                    txtComEmail.Text = Convert.ToString(dt1.Rows[0]["Email"]);
                    txtComPanNo.Text = Convert.ToString(dt1.Rows[0]["PANNo"]).ToUpper();
                    popIDProof();
                    ddlComAddTypeID.SelectedIndex = ddlComAddTypeID.Items.IndexOf(ddlComAddTypeID.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AddressId"])));
                    if (Convert.ToInt32(dt1.Rows[0]["AddressId"]) == 6)
                    {
                        txtComAddIDNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim().Length - 4, 4));
                        hdnApplAadharNo.Value = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    }
                    else
                    {
                        txtComAddIDNo.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    }
                    ddlCustPhotoTypeId.SelectedIndex = ddlCustPhotoTypeId.Items.IndexOf(ddlCustPhotoTypeId.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PhotoId"])));
                    if (Convert.ToInt32(dt1.Rows[0]["PhotoId"]) == 6)
                    {
                        txtCustPhotIDNo.Text = String.Format("{0}{1}", "********", Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Substring(Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim().Length - 4, 4));
                        hdnApplAadharNo.Value = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    }
                    else
                    {
                        txtCustPhotIDNo.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    }

                    txtCustAnnualInc.Text = Convert.ToString(dt1.Rows[0]["AnnualIncome"]);
                    txtCustAnnualInc.Enabled = false;
                    PopRelation();
                    ddlCustRel.SelectedIndex = ddlCustRel.Items.IndexOf(ddlCustRel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelationId"])));
                    txtCustRelName.Text = Convert.ToString(dt1.Rows[0]["RelativeName"]);
                    PopBankAcType();
                    txtAppName.Text = Convert.ToString(dt1.Rows[0]["ApplicantName"]);
                    txtAppContNo.Text = Convert.ToString(dt1.Rows[0]["AppContactNo"]);

                    chkComRegCer.Checked = dt1.Rows[0]["IsRegistered"].ToString() == "Y" ? true : false;

                    txtComRegisNo.Text = dt1.Rows[0]["RegistrationNo"].ToString();
                    txtMAdd1.Text = Convert.ToString(dt1.Rows[0]["MAddress1"]).Trim();
                    txtMAdd2.Text = Convert.ToString(dt1.Rows[0]["MAddress2"]).Trim();
                    ddlMState.SelectedIndex = ddlMState.Items.IndexOf(ddlMState.Items.FindByText(Convert.ToString(dt1.Rows[0]["MState"])));
                    //txtMState.Text = Convert.ToString(dt1.Rows[0]["MState"]).Trim();
                    txtMDist.Text = Convert.ToString(dt1.Rows[0]["MDistrict"]).Trim();
                    txtMPIN.Text = Convert.ToString(dt1.Rows[0]["MPIN"]).Trim();
                    txtMMobNo.Text = Convert.ToString(dt1.Rows[0]["MMobNo"]).Trim();
                    txtMSTD.Text = Convert.ToString(dt1.Rows[0]["MSTD"]).Trim();
                    txtMTel.Text = Convert.ToString(dt1.Rows[0]["MTelNo"]).Trim();
                    txtGSTRegNo.Text = dt1.Rows[0]["GSTRegNo"].ToString();

                    chkComSameAdd.Checked = dt1.Rows[0]["SameAddYN"].ToString() == "Y" ? true : false;
                    txtComRegisNo.Enabled = dt1.Rows[0]["SameAddYN"].ToString() == "Y" ? true : false;

                    txtROffAdd1.Text = Convert.ToString(dt1.Rows[0]["RAddress1"]).Trim();
                    txtROffAdd2.Text = Convert.ToString(dt1.Rows[0]["RAddress2"]).Trim();
                    ddlRState.SelectedIndex = ddlRState.Items.IndexOf(ddlRState.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RState"])));
                    //txtRState.Text = Convert.ToString(dt1.Rows[0]["RState"]).Trim();
                    txtRDist.Text = Convert.ToString(dt1.Rows[0]["RDistrict"]).Trim();
                    txtRPIN.Text = Convert.ToString(dt1.Rows[0]["RPIN"]).Trim();
                    txtRMobNo.Text = Convert.ToString(dt1.Rows[0]["RMobNo"]).Trim();
                    txtRSTD.Text = Convert.ToString(dt1.Rows[0]["RSTD"]).Trim();
                    txtRTel.Text = Convert.ToString(dt1.Rows[0]["RTelNo"]).Trim();
                    //-------------------------------Address Freez----------------------------------
                    if (Convert.ToInt32(Session[gblValue.UserId]) != 1)
                    {
                        txtROffAdd1.Enabled = false;
                        txtROffAdd2.Enabled = false;
                        ddlRState.Enabled = false;
                        txtRDist.Enabled = false;
                        txtRPIN.Enabled = false;
                        txtRMobNo.Enabled = false;
                        txtRSTD.Enabled = false;
                        txtRTel.Enabled = false;
                        txtMMobNo.Enabled = false;
                        txtCustContNo.Enabled = false;
                        txtAppContNo.Enabled = false;
                        ddlCustPhotoTypeId.Enabled = false;
                        txtCustPhotIDNo.Enabled = false;
                    }
                    //---------------------------------------------------------------------------
                    PopBusinessType();
                    ddlCustBusType.SelectedIndex = ddlCustBusType.Items.IndexOf(ddlCustBusType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["BusinessTypeId"])));
                    txtCustBusLocation.Text = Convert.ToString(dt1.Rows[0]["BusinessLocation"]);
                    PopOccupation();
                    ddlCustOccupation.SelectedIndex = ddlCustOccupation.Items.IndexOf(ddlCustOccupation.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId"])));
                    ddlCustOccupation2.SelectedIndex = ddlCustOccupation2.Items.IndexOf(ddlCustOccupation2.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId2"])));
                    ddlCustOccupation3.SelectedIndex = ddlCustOccupation3.Items.IndexOf(ddlCustOccupation3.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId3"])));
                    PopMaritalStatus();
                    ddlCustMS.SelectedIndex = ddlCustMS.Items.IndexOf(ddlCustMS.Items.FindByValue(Convert.ToString(dt1.Rows[0]["MaritalStatus"])));
                    ddlCustResdStatus.SelectedIndex = ddlCustResdStatus.Items.IndexOf(ddlCustResdStatus.Items.FindByValue(Convert.ToString(dt1.Rows[0]["ResidentialStatus"])));
                    ddlCustCommAddress.SelectedIndex = ddlCustCommAddress.Items.IndexOf(ddlCustCommAddress.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CommunAddress"])));

                    ddlCustQual.SelectedIndex = ddlCustQual.Items.IndexOf(ddlCustQual.Items.FindByValue(Convert.ToString(dt1.Rows[0]["QualificationId"])));
                    ddlCustReligion.SelectedIndex = ddlCustReligion.Items.IndexOf(ddlCustReligion.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RligionId"])));
                    ddlCustCaste.SelectedIndex = ddlCustCaste.Items.IndexOf(ddlCustCaste.Items.FindByValue(Convert.ToString(dt1.Rows[0]["Caste"])));
                    txtYrInCurRes.Text = Convert.ToString(dt1.Rows[0]["NoOfYrInCurRes"]);
                    ddlCustPhyChallenge.SelectedIndex = ddlCustPhyChallenge.Items.IndexOf(ddlCustPhyChallenge.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PhyChallangeYN"])));
                    txtCustACHoldname.Text = Convert.ToString(dt1.Rows[0]["ACHolderName"]);
                    txtCustBankName.Text = Convert.ToString(dt1.Rows[0]["BankName"]);
                    txtCustAccNo.Text = Convert.ToString(dt1.Rows[0]["ACNo"]);
                    txtCustIFSCCode.Text = Convert.ToString(dt1.Rows[0]["IFSCCode"]);
                    hdnCustIFSCCode.Value = Convert.ToString(dt1.Rows[0]["IFSCCode"]);
                    txtCustAcYrOfOpen.Text = Convert.ToString(dt1.Rows[0]["YrOfOpening"]);
                    ddlCustACType.SelectedIndex = ddlCustACType.Items.IndexOf(ddlCustACType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AccountType"])));
                    txtCustContNo.Text = Convert.ToString(dt1.Rows[0]["ContactNo"]);

                    //new added for mhf name relation

                    txtMHFName.Text = Convert.ToString(dt1.Rows[0]["MFHName"]);
                    ddlMHFRelation.SelectedIndex = ddlMHFRelation.Items.IndexOf(ddlMHFRelation.Items.FindByValue(Convert.ToString(dt1.Rows[0]["MHFRelation"])));

                    //Nomenee Details
                    txtNomineeName.Text = Convert.ToString(dt1.Rows[0]["NomineeName"]);
                    txtNomineeDOB.Text = Convert.ToString(dt1.Rows[0]["NomineeDOB"]);
                    txtNomineePin.Text = Convert.ToString(dt1.Rows[0]["NomineePinCode"]);
                    txtNomineeAddress.Text = Convert.ToString(dt1.Rows[0]["NomineeAddress"]);
                    ddlNomineeGender.SelectedIndex = ddlNomineeGender.Items.IndexOf(ddlNomineeGender.Items.FindByValue(Convert.ToString(dt1.Rows[0]["NomineeGender"])));
                    ddlNomineeRel.SelectedIndex = ddlNomineeRel.Items.IndexOf(ddlNomineeRel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["NomineeRelation"])));
                    ddlNomineeState.SelectedIndex = ddlNomineeState.Items.IndexOf(ddlNomineeState.Items.FindByValue(Convert.ToString(dt1.Rows[0]["NomineeState"])));

                    //EMPLOYMENT DETAILS
                    txtCustOrgname.Text = Convert.ToString(dt1.Rows[0]["EmpOrgName"]);
                    txtCustDesig.Text = Convert.ToString(dt1.Rows[0]["EmpDesig"]);
                    txtCustRetiredAge.Text = Convert.ToString(dt1.Rows[0]["EmpRetiredAge"]);
                    txtCustDeptEmpCode.Text = Convert.ToString(dt1.Rows[0]["EmpCode"]);
                    txtCustCurExp.Text = Convert.ToString(dt1.Rows[0]["EmpCurExp"]);
                    txtCustTotExp.Text = Convert.ToString(dt1.Rows[0]["EmpTotExp"]);

                    //BUSINESS DETAILS
                    chkGSTApp.Checked = dt1.Rows[0]["BusGSTAppYN"].ToString() == "Y" ? true : false;

                    txtCustGSTNo.Text = Convert.ToString(dt1.Rows[0]["BusGSTNo"]);
                    txtCustBusLandmark.Text = Convert.ToString(dt1.Rows[0]["BusLandMark"]);

                    txtCustBusAdd1.Text = Convert.ToString(dt1.Rows[0]["BusAddress1"]);
                    txtCustBusAdd2.Text = Convert.ToString(dt1.Rows[0]["BusAddress2"]);
                    txtCustBusLocality.Text = Convert.ToString(dt1.Rows[0]["BusLocality"]);
                    txtCustBusCity.Text = Convert.ToString(dt1.Rows[0]["BusCity"]);
                    txtCustBusPin.Text = Convert.ToString(dt1.Rows[0]["BusPIN"]);
                    txtCustBusState.Text = Convert.ToString(dt1.Rows[0]["BusState"]);
                    txtCustBusMob.Text = Convert.ToString(dt1.Rows[0]["BusMob"]);
                    txtCustBusPh.Text = Convert.ToString(dt1.Rows[0]["BusPhone"]);

                    txtBusinessName.Text = Convert.ToString(dt1.Rows[0]["BusinessName"]);
                    txtYrInCurBusi.Text = Convert.ToString(dt1.Rows[0]["YearsInCurrBusiness"]);

                    ddlAbledYN.SelectedIndex = ddlAbledYN.Items.IndexOf(ddlAbledYN.Items.FindByValue(Convert.ToString(dt1.Rows[0]["IsAbledYN"])));
                    ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue(Convert.ToString(dt1.Rows[0]["MinorityYN"])));
                    ddlSpclAbled.SelectedIndex = ddlSpclAbled.Items.IndexOf(ddlSpclAbled.Items.FindByValue(Convert.ToString(dt1.Rows[0]["SpeciallyAbled"])));

                    if (Convert.ToString(dt1.Rows[0]["IsAbledYN"]) == "Y")
                    {
                        ddlSpclAbled.Enabled = true;
                    }

                    GetDependentDetails();
                }
                if (dt2.Rows.Count > 0)
                {
                    gvCustDep.DataSource = dt2;
                    gvCustDep.DataBind();
                }
                else
                {
                    dt2 = GetDependentDetails();
                    gvCustDep.DataSource = dt2;
                    gvCustDep.DataBind();
                }

                if (Convert.ToString(dt1.Rows[0]["IdVerifyYN"]) == "Y")
                {
                    ddlComAddTypeID.Enabled = false;
                    txtComAddIDNo.Enabled = false;
                    //txtCustPhotIDNo.Enabled = false;
                    //ddlCustPhotoTypeId.Enabled = false;
                }
                btnUpdateCom.Enabled = Convert.ToString(dt1.Rows[0]["EditAllowYN"]) == "N" ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt1 = null;
                ds = null;
                oMem = null;
            }

        }
        protected void GetBrCodeByCustId(string pCustId)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            try
            {
                dt = OMem.GetBrCodeByCustId(pCustId);
                if (dt.Rows.Count > 0)
                {
                    hdCustBrCode.Value = dt.Rows[0]["BranchCode"].ToString();
                    hdCustBrName.Value = dt.Rows[0]["BranchName"].ToString();
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
        private void ShowAllInitialLoanApp(string vCustId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            ds = ca.GetLnAppList(vCustId);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
            }

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
                if (dt1.Rows.Count > 0)
                {
                    gvRefList.DataSource = dt1;
                    gvRefList.DataBind();
                }
                else
                {
                    gvRefList.DataSource = dt1;
                    gvRefList.DataBind();
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
                ds = null;
                dt1 = null;
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
            DataTable dt4 = new DataTable();
            try
            {
                ds = ca.GetInitLoanDtlByLoanId(pLnAppId, vBrCode);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];
                    dt4 = ds.Tables[4];
                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 1;
                    // ViewAcess();
                    hdfApplicationId.Value = pLnAppId;
                    btSaveApplication.Enabled = false;
                    btnUpdateApplication.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppNo.Text = Convert.ToString(dt.Rows[0]["LoanAppNo"]).Trim();
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["ApplicationDt"]).Trim();
                    PopApplicant(BrCode);
                    ddlLoanApplicantname.SelectedIndex = ddlLoanApplicantname.Items.IndexOf(ddlLoanApplicantname.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustID"])));
                    PopPurpose();
                    PopLoanType();
                    ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                    ddlLnScheme.SelectedIndex = ddlLnScheme.Items.IndexOf(ddlLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
                    EnableMachinDtl(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
                    txtAppLnAmt.Text = Convert.ToString(dt.Rows[0]["AppAmount"]).Trim();
                    ddlTenure.SelectedIndex = ddlTenure.Items.IndexOf(ddlTenure.Items.FindByValue(Convert.ToString(dt.Rows[0]["Tenure"])));
                    // txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]).Trim();
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
                    ddlLoanApplicantname.Enabled = false;
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

                foreach (GridViewRow gr in gvApp.Rows)
                {

                    Label lblReportID = (Label)gr.FindControl("lblReportID");
                    Label lblIsEquifaxEnqDone = (Label)gr.FindControl("lblIsEquifaxEnqDone");
                    LinkButton btnVerifyEquifax = (LinkButton)gr.FindControl("btnVerifyEquifax");
                    ImageButton btnDownloadCustEnq = (ImageButton)gr.FindControl("btnDownloadCustEnq");

                    if (lblIsEquifaxEnqDone.Text == "Y")
                    {
                        btnVerifyEquifax.Enabled = false;
                        btnDownloadCustEnq.Enabled = true;
                    }
                    else
                    {
                        btnVerifyEquifax.Enabled = true;
                        btnDownloadCustEnq.Enabled = false;
                    }

                }

                foreach (GridViewRow gr in gvCoAppDtl.Rows)
                {

                    Label lblReportID = (Label)gr.FindControl("lblReportID");
                    Label lblIsEquifaxEnqDone = (Label)gr.FindControl("lblIsEquifaxEnqDone");
                    LinkButton btnEquifaxVerifyCA = (LinkButton)gr.FindControl("btnEquifaxVerifyCA");
                    ImageButton btnDownloadCoAppEnq = (ImageButton)gr.FindControl("btnDownloadCoAppEnq");
                    if (lblIsEquifaxEnqDone.Text == "Y")
                    {
                        btnEquifaxVerifyCA.Enabled = false;
                        btnDownloadCoAppEnq.Enabled = true;
                    }
                    else
                    {
                        btnEquifaxVerifyCA.Enabled = true;
                        btnDownloadCoAppEnq.Enabled = false;
                    }

                }
                if (dt4.Rows.Count > 0)
                {
                    hdnQaYN.Value = "Y";
                }
                FetchBindData(dt4);
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
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {
                // get reference details
                LoadRefList(pLnAppId);
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
        protected void ImDelRef_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["RefDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["RefDtl"] = dt;
                    gvRef.DataSource = dt;
                    gvRef.DataBind();
                }
                else
                {
                    popRefDetails();
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
        private void ShowRefDetails(string pRefId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetRefDtlRefId(Convert.ToInt32(pRefId));
                dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 4;
                    // ViewAcess();
                    hfRefId.Value = Convert.ToString(dt1.Rows[0]["RefId"]).Trim();
                    btnSaveRef.Enabled = false;
                    btnUpdateRef.Enabled = true;
                    //btnDelRef.Enabled = true;
                    txtAppNoRef.Text = Convert.ToString(dt1.Rows[0]["ApplicationId"]).Trim();
                    txtRefName.Text = Convert.ToString(dt1.Rows[0]["RefName"]).Trim();
                    txtRefAddr.Text = Convert.ToString(dt1.Rows[0]["RefAddress"]).Trim();
                    txtRefMNo.Text = Convert.ToString(dt1.Rows[0]["RefMob"]).Trim();
                    PopRelation();
                    PopVarifiedBy();
                    ddlRelWApp.SelectedIndex = ddlRelWApp.Items.IndexOf(ddlRelWApp.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelWithApplicant"])));

                    txtOffTelNoRef.Text = Convert.ToString(dt1.Rows[0]["OffTelNo"]).Trim();
                    txtCompNameRef.Text = Convert.ToString(dt1.Rows[0]["CompanyName"]).Trim();
                    ddlStatus.SelectedValue = Convert.ToString(dt1.Rows[0]["Status"]).Trim();
                    ddlVarifiedBy.SelectedItem.Text = Convert.ToString(dt1.Rows[0]["VarifiedBy"]);
                    if (dt1.Rows[0]["PersonalRef"].ToString() == "Y")
                    {
                        rblRefType.SelectedValue = "P";
                    }
                    else if (dt1.Rows[0]["TradeRef"].ToString() == "Y")
                    {
                        rblRefType.SelectedValue = "P";
                    }
                    else
                    {
                        rblRefType.SelectedIndex = -1;
                    }
                    gvRef.DataSource = null;
                    gvRef.DataBind();
                    ViewState["RefDtl"] = null;
                    popRefDetails();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                ds = null;
                dt1 = null;
            }
        }
        private void LoadCompanyList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCompanyList(vBrCode, vLogDt, txtSearch.Text.Trim());
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvCompany.DataSource = dt1;
                        gvCompany.DataBind();
                    }
                    else
                    {
                        gvCompany.DataSource = null;
                        gvCompany.DataBind();
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
        private void GetPendCompanyList()
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPendCompanyList(vBrCode, vLogDt);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvPendApp.DataSource = dt1;
                        gvPendApp.DataBind();
                    }
                    else
                    {
                        gvPendApp.DataSource = null;
                        gvPendApp.DataBind();
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
        private void popRefDetails()
        {
            DataTable dt = null;
            try
            {
                dt = GetRefDetails();
                ViewState["RefDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvRef.DataSource = dt;
                    gvRef.DataBind();
                }
                else
                {
                    gvRef.DataSource = null;
                    gvRef.DataBind();
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
        protected void gvPendApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCompanyID = "";
            vCompanyID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvPendApp.Rows)
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
                //gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowCompanyData(vCompanyID, vBrCode);
            }
        }
        protected void gvCompany_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCompanyID = "";
            vCompanyID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCompany.Rows)
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
                //gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#151B54");
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowCompanyData(vCompanyID, vBrCode);
            }
        }
        protected void gvCoApp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCoAppID = "";
            vCoAppID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");

                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCoApp.Rows)
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
                ShowCoAppData(vCoAppID, vBrCode);
                txtCoAppName.Enabled = false;

                ddlCoAppAddressIdType.Enabled = (ddlCoAppAddressIdType.SelectedValue == "15") ? false : true;
                txtCoAppAddressIdNo.Enabled = (ddlCoAppAddressIdType.SelectedValue == "15") ? false : true;
                ddlCoAppPhotoIdType.Enabled = (ddlCoAppPhotoIdType.SelectedValue == "15") ? false : true;
                txtCoAppPNo.Enabled = (ddlCoAppPhotoIdType.SelectedValue == "15") ? false : true;
            }
        }
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

                string vDocUploadYN = gvRow.Cells[7].Text.Trim();
                if (vDocUploadYN == "N")
                {
                    gblFuction.AjxMsgPopup("Please Upload Document First.");
                    return;
                }
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
        }
        protected void gvRefList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vRefId = "";
            vRefId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvRefList.Rows)
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
                gvRefList.Font.Bold = true;
                btnShow.Font.Bold = true;
                ShowRefDetails(vRefId);
            }
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }
        protected void btnAppAdd_Click(object sender, EventArgs e)
        {
            PopReligion();
            PopCaste();
            PopMaritalStatus();
            PopGender();
            PopOccupation();
            PopQualification();
            popIDProof();
            //  popCO();
            mView.ActiveViewIndex = 1;
            //btnSaveApp.Enabled = true;
            //btnUpdateApp.Enabled = false;
        }
        protected void btnAppEdit_Click(object sender, EventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vApplicantID = "";
            vApplicantID = Convert.ToString(hdfAppIdShow.Value);
            ShowCompanyDetails(vApplicantID, vBrCode);
            txtComName.Enabled = false;

            ddlComAddTypeID.Enabled = (ddlComAddTypeID.SelectedValue == "15") ? false : true;
            txtComAddIDNo.Enabled = (ddlComAddTypeID.SelectedValue == "15") ? false : true;
            ddlCustPhotoTypeId.Enabled = (ddlCustPhotoTypeId.SelectedValue == "15") ? false : true;
            txtCustPhotIDNo.Enabled = (ddlCustPhotoTypeId.SelectedValue == "15") ? false : true;
            ddlSpclAbled.Enabled = false;
        }

        protected void btnAddCoApp_Click(object sender, EventArgs e)
        {
            if (ViewState["CusTID"] == null)
            {
                gblFuction.MsgPopup("Please Selct Customer to add Co Applicant");
                return;
            }
            else
            {
                // Branch Code Checking
                string vCustBrCode = "", vBrCode = "", vCustBrName = "";
                vBrCode = (string)Session[gblValue.BrnchCode];
                GetBrCodeByCustId(ViewState["CusTID"].ToString());
                if (hdCustBrCode.ToString() != "")
                {
                    vCustBrCode = hdCustBrCode.Value.ToString();
                    vCustBrName = hdCustBrName.Value.ToString();
                }
                if (vCustBrCode != vBrCode)
                {
                    gblFuction.AjxMsgPopup("Kindly Login To " + vCustBrName.ToString() + " Branch To Add CoApplicant as Customer Is Created From Same Branch...");
                    return;
                }
                mView.ActiveViewIndex = 2;
                // ViewAcess();
                ClearCoApplicant();
                PopCoAppCompanyType();
                PopCoAppPropertyType();
                PopCustomer();
                ddlCustName.SelectedValue = ViewState["CusTID"].ToString();
                PopReligion();
                PopCaste();
                PopMaritalStatus();
                PopGender();
                popState();
                PopOccupation();
                PopQualification();
                popIDProof();
                PopRelation();
                PopBankAcType();
                btnSaveCoApp.Enabled = true;
                btnUpdateCoApp.Enabled = false;
            }
        }

        protected void btnNewApplication_Click(object sender, EventArgs e)
        {
            if (ViewState["CusTID"] == null)
            {
                gblFuction.MsgPopup("Please Selct Customer to Apply Loan");
                return;
            }
            else
            {
                string vCustBrCode = "", vBrCode = "", vCustBrName = "";
                vBrCode = (string)Session[gblValue.BrnchCode];
                GetBrCodeByCustId(ViewState["CusTID"].ToString());
                if (hdCustBrCode.ToString() != "")
                {
                    vCustBrCode = hdCustBrCode.Value.ToString();
                    vCustBrName = hdCustBrName.Value.ToString();
                }
                if (vCustBrCode != vBrCode)
                {
                    gblFuction.AjxMsgPopup("Kindly Login To " + vCustBrName.ToString() + " Branch For Loan Application as Customer Is Created From Same Branch...");
                    return;
                }
                ClearApplication();
                mView.ActiveViewIndex = 1;
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtLnAppPassDt.Text = Session[gblValue.LoginDate].ToString();
                // ViewAcess();
                PopApplicant(vCustBrCode);
                // ddlLoanApplicantname.SelectedValue = ViewState["CusTID"].ToString();
                ddlLoanApplicantname.SelectedIndex = ddlLoanApplicantname.Items.IndexOf(ddlLoanApplicantname.Items.FindByValue(Convert.ToString(ViewState["CusTID"])));
                //ddlLoanApplicantname_SelectedIndexChanged(sender, e);
                //  ddlLoanApplicantname.Enabled = false;
                popSourceName();
                popCustForHighmark();
                popCoApplForHighmark();
                PopPurpose();
                PopLoanType();
                popMLAsset();
                btSaveApplication.Enabled = true;
                btnUpdateApplication.Enabled = false;

                //HtmlTableRow tRow = new HtmlTableRow();
                //for (int i = 1; i < 3; i++)
                //{
                //    HtmlTableCell tb = new HtmlTableCell();
                //    tb.InnerHtml = "text <br/>";
                //    tRow.Controls.Add(tb);
                //}
                //tblHeading.Rows.Add(tRow);
                //BindData();
                tpProxies.Visible = false;
            }
        }

        private void popCustForHighmark()
        {
            DataTable dt = new DataTable();
            CApplication OApp = new CApplication();
            string pCustId = ViewState["CusTID"].ToString();
            dt = OApp.GetCustForHighmark(pCustId);
            if (dt.Rows.Count > 0)
            {
                gvApp.DataSource = dt;
                gvApp.DataBind();
            }
            else
            {
                gvApp.DataSource = null;
                gvApp.DataBind();

            }
        }

        private void popCoApplForHighmark()
        {
            DataTable dt = new DataTable();
            CApplication OApp = new CApplication();
            string pCustId = ViewState["CusTID"].ToString();
            dt = OApp.GetCustForHighmark(pCustId, "C");
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

        protected void btnNewRef_Click(object sender, EventArgs e)
        {
            if (ViewState["CusTID"] == null)
            {
                gblFuction.MsgPopup("Please Selct Customer to add Reference");
                return;
            }
            else
            {
                DataTable dt = new DataTable();
                CMember oMem = new CMember();
                dt = oMem.GetLoanAppNoByCustid(ViewState["CusTID"].ToString());
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 4;
                    //  ViewAcess();
                    ViewState["LoanAppId"] = dt.Rows[0]["LoanAppId"].ToString();
                    txtAppNoRef.Text = ViewState["LoanAppId"].ToString();
                    ClearRefControl();
                    PopRelation();
                    popRefDetails();
                    PopVarifiedBy();
                    btnSaveRef.Enabled = true;
                    btnUpdateRef.Enabled = false;
                    //btnDelRef.Enabled = false;
                }
                else
                {
                    gblFuction.MsgPopup("Please Complete Loan Application To Add Reference Details...");
                }
            }
        }
        protected void btnAddCom_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 3;
            // ViewAcess();
            PopCompanyType();
            PopPropertyType();
            PopSector();
            popIDProof();
            btnSaveCom.Enabled = true;
            btnUpdateCom.Enabled = false;
            btnDeleteCom.Enabled = false;
        }
        protected void btnAddRef_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["RefDtl"];
            DataRow dr = dt.NewRow();
            string vBrCode = Session[gblValue.BrnchCode].ToString();

            Int32 vRelWithAppId = 0;
            string vLnAppId = "", vRefName = "", vRefAddr = "", vRefMNo = "", vOffTelNo = "", vRelWithApp = "", vCompName = "", vPersonalRef = "",
                vTradeRef = "", vRefType = "", vStatus = "", vVarifiedBy = "";

            vLnAppId = Convert.ToString((Request[txtAppNoRef.UniqueID] as string == null) ? txtAppNoRef.Text : Request[txtAppNoRef.UniqueID] as string);
            vRefName = Convert.ToString((Request[txtRefName.UniqueID] as string == null) ? txtRefName.Text : Request[txtRefName.UniqueID] as string);
            vRefAddr = Convert.ToString((Request[txtRefAddr.UniqueID] as string == null) ? txtRefAddr.Text : Request[txtRefAddr.UniqueID] as string);
            vRefMNo = Convert.ToString((Request[txtRefMNo.UniqueID] as string == null) ? txtRefMNo.Text : Request[txtRefMNo.UniqueID] as string);
            vOffTelNo = Convert.ToString((Request[txtOffTelNoRef.UniqueID] as string == null) ? txtOffTelNoRef.Text : Request[txtOffTelNoRef.UniqueID] as string);
            vCompName = Convert.ToString((Request[txtCompNameRef.UniqueID] as string == null) ? txtCompNameRef.Text : Request[txtCompNameRef.UniqueID] as string);
            vRelWithApp = Convert.ToString((Request[ddlRelWApp.SelectedItem.Text] as string == null) ? ddlRelWApp.SelectedItem.Text : Request[ddlRelWApp.SelectedItem.Text] as string);
            if (ddlRelWApp.SelectedIndex != -1)
                vRelWithAppId = Convert.ToInt32((Request[ddlRelWApp.UniqueID] as string == null) ? ddlRelWApp.SelectedValue : Request[ddlRelWApp.UniqueID] as string);
            if (rblRefType.SelectedValue == "P")
            {
                vPersonalRef = "Y";
                vTradeRef = "N";
                vRefType = "Personal";
            }
            else
            {
                vPersonalRef = "N";
                vTradeRef = "Y";
                vRefType = "Trade";
            }
            vStatus = Convert.ToString((Request[ddlStatus.UniqueID] as string == null) ? ddlStatus.SelectedValue : Request[ddlStatus.UniqueID] as string);
            vVarifiedBy = Convert.ToString(ddlVarifiedBy.SelectedItem.Text.ToString());
            dr[0] = vLnAppId;//dr[ApplicationId]
            dr[1] = vRefName;//dr[RefName]
            dr[2] = vRefAddr;//dr[RefAddr]
            dr[3] = vRefMNo;//dr[RefMNo]
            dr[4] = vRelWithAppId;//dr[RelWithAppId]
            dr[5] = vRelWithApp;//dr[RelWithApp]
            dr[6] = vOffTelNo;//dr[OffTelNo]
            dr[7] = vCompName;//dr[CompName]
            dr[8] = vPersonalRef;//dr[PersonalRef]
            dr[9] = vTradeRef;//dr[TradeRef]
            dr[10] = vRefType;//dr[TradeRef]
            dr[11] = vStatus;//dr[Status]
            dr[12] = vVarifiedBy;//dr[VarifiedBy]
            dt.Rows.Add(dr);
            dt.AcceptChanges();

            ViewState["RefDtl"] = dt;
            if (dt.Rows.Count > 0)
            {
                gvRef.DataSource = dt;
                gvRef.DataBind();
                ClearRefControl();
            }
        }
        protected void btnBackApplication_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 2;
            //  ViewAcess();

        }
        protected void btnUpdateApp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCompanyRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                LoadCompanyList();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                // ClearControls();
                ClearAppControlForShow();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnSaveRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                // ViewAcess();
                // LoadRefList(ViewState["LoanAppId"].ToString());
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                gvRef.DataSource = null;
                gvRef.DataBind();
                ViewState["RefDtl"] = null;
                popRefDetails();
                ClearRefControl();
            }
        }
        protected void btnUpdateRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                // ViewAcess();
                // LoadRefList(ViewState["LoanAppId"].ToString());
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                gvRef.DataSource = null;
                gvRef.DataBind();
                ViewState["RefDtl"] = null;
                popRefDetails();
                ClearRefControl();
            }
        }
        protected void btnDelRef_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveRefRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                // ViewAcess();
                // LoadRefList(ViewState["LoanAppId"].ToString());
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                //gvRef.DataSource = null;
                //gvRef.DataBind();
                //ViewState["PLDtl"] = null;

            }
        }
        protected void btnBackRef_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 2;
            ViewAcess();
        }
        protected void btSaveApplication_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";

            if (SaveLoanAppRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 2;
                //  ViewAcess();
                ClearApplication();
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                StatusButton("View");
                ViewState["StateEdit"] = null;
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
                tbMem.ActiveTabIndex = 2;
                // ViewAcess();
                ClearApplication();
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
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
                tbMem.ActiveTabIndex = 2;
                //  ViewAcess();
                ClearApplication();
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnSaveCoApp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveCoAppRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                LoadCompanyList();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                ShowCompanyData(ViewState["CusTID"].ToString(), "0000");
                //  ViewAcess();
                ClearCoApplicant();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnUpdateCoApp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCoAppRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                LoadCompanyList();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                // ViewAcess();
                ClearCoApplicant();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDelCoApp_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveCoAppRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                LoadCompanyList();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 1;
                // ViewAcess();
                ClearCoApplicant();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnSaveCom_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveCompanyRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                //  LoadGrid();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                ViewAcess();
                ClearCompany();
                LoadCompanyList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnUpdateCom_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCompanyRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                //  LoadGrid();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                // ViewAcess();
                ClearCompany();
                LoadCompanyList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDeleteCom_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveCompanyRecords(vStateEdit) == true)
            {
                switch (vStateEdit)
                {
                    case "Save":
                        lblMsg.Text = gblPRATAM.SaveMsg;
                        break;
                    case "Edit":
                        lblMsg.Text = gblPRATAM.EditMsg;
                        break;
                    case "Delete":
                        lblMsg.Text = gblPRATAM.DeleteMsg;
                        break;
                }
                //  LoadGrid();
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 0;
                // ViewAcess();
                ClearCompany();
                LoadCompanyList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnBackApp_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            // ViewAcess();
        }
        protected void btnBackCoApp_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            //  ViewAcess();
        }
        protected void btnBackCom_Click(object sender, EventArgs e)
        {
            ClearCompany();
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            // ViewAcess();

        }
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    Label lblMsg = (Label)Master.FindControl("lblMsg");
        //    try
        //    {
        //        if (this.CanDelete == "N")
        //        {
        //            gblFuction.MsgPopup(MsgAccess.Del);
        //            return;
        //        }
        //        if (SaveRecords("Delete") == true)
        //        {
        //            lblMsg.Text = gblPRATAM.DeleteMsg;
        //            // LoadList();
        //            StatusButton("Delete");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        protected void btnBackAppDtl_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 0;
            // ViewAcess();
            ClearAppControlForShow();
            gvCoApp.DataSource = null;
            gvCoApp.DataBind();
            StatusButton("View");
        }
        private Boolean SaveCoAppRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCoAppId = "", vCoAppType = "";
            Int32 vComTypeId = 0, vPropertyTypeId = 0, vCustCoAppRel = 0;
            string vCoAppNo = "", vCustId = "", vCoAppSamAdd = "N", vNewId = "", vGuarantor = "", vIsPrimaryCoApp = "", vNominee = "";
            string vCoAppNm = "", vAppname = "";
            string vCoAppPreAdd1 = "", vCoAppPreAdd2 = "", vCoAppPreDist = "", vCoAppPreState = "", vCoAppPrePIN = "", vCoAppPerAdd1 = "", vCoAppPerAdd2 = "", vCoAppPerDist = "", vCoAppPerState = "", vCoAppPerPIN = "";

            Int32 vErr = 0, vCoAppPId = 0, vCoAppAId = 0, vRelationId = 0, YrInOpe = 0;
            string vCoAppANo = "", vCoAppPNo = "", vRelativeName = "";
            string vCoAppIsDir = "", vCoAppPart = "", vCoAppPropietor = "", vCoAppSpouse = "", vSameAddAsApp = "", vIsActive = "";
            string vCoAppMNo = "", vCoAppTelNo = "", vCoAppEmail = "", vCoAppFaxNo = "";

            Int32 vCoAppAge = 0, vCoApMarital = 0, vCoApCast = 0, vCoApOccuId = 0, vCoApRelig = 0, vCoApGen = 0, vCoAppQulId = 0, vCoAppYAR = 0, vCoAppYIB = 0,
                vCoApOccuId2 = 0, vCoApOccuId3 = 0;
            Decimal vShareHolPer = 0;
            PathKYCImage = "G:\\WebApps\\CentrumSmeMobService\\CoApplicantKYCImage";
            vCoAppType = (Request[ddlCoAppType.UniqueID] as string == null) ? ddlCoAppType.SelectedValue : Request[ddlCoAppType.UniqueID] as string;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Please Log In To Branch to Save/Update Co Applicant Records");
                return false;
            }
            if (ddlCoAppType.SelectedValue == "P")
            {
                if (ddlCoAppGender.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Co Applicant Gender...");
                    ddlCoAppGender.Focus();
                    return false;
                }
                if (ddlCoAppRel.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Co Applicant Religion...");
                    ddlCoAppRel.Focus();
                    return false;
                }
                if (ddlCoAppCast.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Co Applicant Cast...");
                    ddlCoAppCast.Focus();
                    return false;
                }
                if (ddlCoAppMS.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Co Applicant Marital Status...");
                    ddlCoAppMS.Focus();
                    return false;
                }
                if (txtCoAppDOB.Text == "")
                {
                    gblFuction.MsgPopup("Please Input DOB Field...");
                    txtCoAppDOB.Focus();
                    return false;
                }
                if (txtCoAppDOA.Text == "")
                {
                    gblFuction.MsgPopup("Please Input Admission Date Field...");
                    txtCoAppDOA.Focus();
                    return false;
                }
                vCoAppAge = Convert.ToInt32((Request[txtCoAppAge.UniqueID] as string == null) ? txtCoAppAge.Text : Request[txtCoAppAge.UniqueID] as string);
                if (vCoAppAge < 18)
                {
                    gblFuction.MsgPopup("Minimum Age Limit Of Co-Applicant/Guarantor is 18 yrs");
                    return false;
                }
                if (ddlCustCARel.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select CoApplicant-Applicant Relation...");
                    ddlCustCARel.Focus();
                    return false;
                }
                vCoApGen = Convert.ToInt32((Request[ddlCoAppGender.UniqueID] as string == null) ? ddlCoAppGender.SelectedValue : Request[ddlCoAppGender.UniqueID] as string);
                vCoApRelig = Convert.ToInt32((Request[ddlCoAppRel.UniqueID] as string == null) ? ddlCoAppRel.SelectedValue : Request[ddlCoAppRel.UniqueID] as string);
                vCoApCast = Convert.ToInt32((Request[ddlCoAppCast.UniqueID] as string == null) ? ddlCoAppCast.SelectedValue : Request[ddlCoAppCast.UniqueID] as string);
                vCoApOccuId = Convert.ToInt32((Request[ddlCoAppOccu.UniqueID] as string == null) ? ddlCoAppOccu.SelectedValue : Request[ddlCoAppOccu.UniqueID] as string);
                vCoApOccuId2 = Convert.ToInt32((Request[ddlCoAppOccu2.UniqueID] as string == null) ? ddlCoAppOccu2.SelectedValue : Request[ddlCoAppOccu2.UniqueID] as string);
                vCoApOccuId3 = Convert.ToInt32((Request[ddlCoAppOccu3.UniqueID] as string == null) ? ddlCoAppOccu3.SelectedValue : Request[ddlCoAppOccu3.UniqueID] as string);


                vCoApMarital = Convert.ToInt32((Request[ddlCoAppMS.UniqueID] as string == null) ? ddlCoAppMS.SelectedValue : Request[ddlCoAppMS.UniqueID] as string);
                vCustCoAppRel = Convert.ToInt32((Request[ddlCustCARel.UniqueID] as string == null) ? ddlCustCARel.SelectedValue : Request[ddlCustCARel.UniqueID] as string);
                if (ddlCARel.SelectedValue != "-1")
                {
                    vRelationId = Convert.ToInt32((Request[ddlCARel.UniqueID] as string == null) ? ddlCARel.SelectedValue : Request[ddlCARel.UniqueID] as string);
                }
                vRelativeName = txtCARelName.Text.ToString();
            }
            else
            {
                if (ddlCoAppComType.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Company Type when Company is co applicant...");
                    ddlCoAppComType.Focus();
                    return false;
                }
                if (ddlCoAppProType.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Property Type when Company is co applicant...");
                    ddlCoAppProType.Focus();
                    return false;
                }
                vComTypeId = Convert.ToInt32((Request[ddlCoAppComType.UniqueID] as string == null) ? ddlCoAppComType.SelectedValue : Request[ddlCoAppComType.UniqueID] as string);
                vPropertyTypeId = Convert.ToInt32((Request[ddlCoAppProType.UniqueID] as string == null) ? ddlCoAppProType.SelectedValue : Request[ddlCoAppProType.UniqueID] as string);
                vAppname = (Request[txtApp.UniqueID] as string == null) ? txtApp.Text : Request[txtApp.UniqueID] as string;
            }
            DateTime vCoAppDOA = gblFuction.setDate(txtCoAppDOA.Text);
            DateTime vCoAppDOB = gblFuction.setDate(txtCoAppDOB.Text);
            string vMsg = "";
            CMember oMem = null;
            CGblIdGenerator oGbl = null;
            try
            {


                vCoAppId = hdfCoAppID.Value;
                if (ddlCustName.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Customer to add Co-Applicant");
                    ddlCustName.Focus();
                    return false;
                }
                vCustId = (Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string;
                vCoAppNm = Convert.ToString((Request[txtCoAppName.UniqueID] as string == null) ? txtCoAppName.Text : Request[txtCoAppName.UniqueID] as string);
                if (vCoAppNm == "")
                {
                    gblFuction.MsgPopup("Co Applicant Name can not be blank..");
                    txtCoAppName.Focus();
                    return false;
                }
                //if (ddlCoAppPhotoIdType.SelectedValue == "-1")
                //{
                //    gblFuction.MsgPopup("Please Select Photo ID Type");
                //    ddlCoAppPhotoIdType.Focus();
                //    return false;
                //}
                //if (txtCoAppPNo.Text == "")
                //{
                //    gblFuction.MsgPopup("Photo ID No Can Not Be Blank...");
                //    txtCoAppPNo.Focus();
                //    return false;
                //}
                if (txtCoAppMNo.Text == "")
                {
                    gblFuction.MsgPopup("CoApp Contact No Can Not Be Blank...");
                    txtCoAppMNo.Focus();
                    return false;
                }
                vCoAppQulId = Convert.ToInt32((Request[ddlCoAppQuali.UniqueID] as string == null) ? ddlCoAppQuali.SelectedValue : Request[ddlCoAppQuali.UniqueID] as string);
                vCoAppPreAdd1 = Convert.ToString((Request[txtCoAppAddress1Pre.UniqueID] as string == null) ? txtCoAppAddress1Pre.Text : Request[txtCoAppAddress1Pre.UniqueID] as string);
                vCoAppPreAdd2 = Convert.ToString((Request[txtCoAppAddress2Pre.UniqueID] as string == null) ? txtCoAppAddress2Pre.Text : Request[txtCoAppAddress2Pre.UniqueID] as string);
                vCoAppPreDist = Convert.ToString((Request[txtCoAppDistPre.UniqueID] as string == null) ? txtCoAppDistPre.Text : Request[txtCoAppDistPre.UniqueID] as string);
                vCoAppPreState = Convert.ToString((Request[ddlCoAppStatePre.UniqueID] as string == null) ? ddlCoAppStatePre.SelectedValue : Request[ddlCoAppStatePre.UniqueID] as string);
                vCoAppPrePIN = Convert.ToString((Request[txtCOAppPINPre.UniqueID] as string == null) ? txtCOAppPINPre.Text : Request[txtCOAppPINPre.UniqueID] as string);
                if (vCoAppPreAdd1 == "" || vCoAppPreDist == "" || vCoAppPreState == "-1" || vCoAppPrePIN == "")
                {
                    gblFuction.MsgPopup("Any of the field like Present Address 1,State,District,PIN can Not be blank....");
                    txtCoAppAddress1Pre.Focus();
                    return false;
                }
                if (chkCoAppAd.Checked == true)
                    vCoAppSamAdd = "Y";
                vCoAppPerAdd1 = Convert.ToString((Request[txtCoAppAddress1Per.UniqueID] as string == null) ? txtCoAppAddress1Per.Text : Request[txtCoAppAddress1Per.UniqueID] as string);
                vCoAppPerAdd2 = Convert.ToString((Request[txtCoAppAddress2Per.UniqueID] as string == null) ? txtCoAppAddress2Per.Text : Request[txtCoAppAddress2Per.UniqueID] as string);
                vCoAppPerDist = Convert.ToString((Request[txtCoAppDistPer.UniqueID] as string == null) ? txtCoAppDistPer.Text : Request[txtCoAppDistPer.UniqueID] as string);
                vCoAppPerState = Convert.ToString((Request[ddlCoAppStatePer.UniqueID] as string == null) ? ddlCoAppStatePer.SelectedValue : Request[ddlCoAppStatePer.UniqueID] as string);
                vCoAppPerPIN = Convert.ToString((Request[txtCoAppPINPer.UniqueID] as string == null) ? txtCoAppPINPer.Text : Request[txtCoAppPINPer.UniqueID] as string);
                if (vCoAppPerAdd1 == "" || vCoAppPerDist == "" || vCoAppPerState == "-1" || vCoAppPerPIN == "")
                {
                    gblFuction.MsgPopup("Any of the field like Permanent Address 1,State,District,PIN can Not be blank....");
                    txtCoAppAddress1Per.Focus();
                    return false;
                }

                vCoAppAId = Convert.ToInt32((Request[ddlCoAppAddressIdType.UniqueID] as string == null) ? ddlCoAppAddressIdType.SelectedValue : Request[ddlCoAppAddressIdType.UniqueID] as string);


                vCoAppPId = Convert.ToInt32((Request[ddlCoAppPhotoIdType.UniqueID] as string == null) ? ddlCoAppPhotoIdType.SelectedValue : Request[ddlCoAppPhotoIdType.UniqueID] as string);
                vCoAppANo = Convert.ToString((Request[txtCoAppAddressIdNo.UniqueID] as string == null) ? txtCoAppAddressIdNo.Text : Request[txtCoAppAddressIdNo.UniqueID] as string);
                vCoAppPNo = Convert.ToString((Request[txtCoAppPNo.UniqueID] as string == null) ? txtCoAppPNo.Text : Request[txtCoAppPNo.UniqueID] as string);
                vCoAppMNo = Convert.ToString((Request[txtCoAppMNo.UniqueID] as string == null) ? txtCoAppMNo.Text : Request[txtCoAppMNo.UniqueID] as string);
                vCoAppTelNo = Convert.ToString((Request[txtCoAppTelNo.UniqueID] as string == null) ? txtCoAppTelNo.Text : Request[txtCoAppTelNo.UniqueID] as string);
                vCoAppEmail = Convert.ToString((Request[txtCoAppEmail.UniqueID] as string == null) ? txtCoAppEmail.Text : Request[txtCoAppEmail.UniqueID] as string);
                vCoAppFaxNo = Convert.ToString((Request[txtCoAppFaxNo.UniqueID] as string == null) ? txtCoAppFaxNo.Text : Request[txtCoAppFaxNo.UniqueID] as string);


                if (chkIsGuarantor.Checked == true)
                    vGuarantor = "Y";
                else
                    vGuarantor = "N";
                if (chkIsPrimaryCoApp.Checked == true)
                    vIsPrimaryCoApp = "Y";
                else
                    vIsPrimaryCoApp = "N";
                if (chkIsNominee.Checked == true)
                    vNominee = "Y";
                else
                    vNominee = "N";
                if (chkCoAppSameAdd.Checked == true)
                    vSameAddAsApp = "Y";
                else
                    vSameAddAsApp = "N";

                if (chkIsActive.Checked == true)
                    vIsActive = "Y";
                else
                    vIsActive = "N";

                if (txtCoAppYratRes.Text != "")
                    vCoAppYAR = Convert.ToInt32((Request[txtCoAppYratRes.UniqueID] as string == null) ? txtCoAppYratRes.Text : Request[txtCoAppYratRes.UniqueID] as string);
                if (txtCoAppYrinBusiness.Text != "")
                    vCoAppYIB = Convert.ToInt32((Request[txtCoAppYrinBusiness.UniqueID] as string == null) ? txtCoAppYrinBusiness.Text : Request[txtCoAppYrinBusiness.UniqueID] as string);
                if (txtShareHold.Text != "")
                    vShareHolPer = Convert.ToDecimal((Request[txtShareHold.UniqueID] as string == null) ? txtShareHold.Text : Request[txtShareHold.UniqueID] as string);


                // BANK ACCOUNT DETAILS
                //decimal pYrInRe = 0;
                //if (txtYrInCurRes.Text != "")
                //    pYrInRe = Convert.ToDecimal(txtYrInCurRes.Text);
                string pPhyChallengedYN = ddlCAPhyChallenge.SelectedValue.ToString();
                string pAcHolName = txtCAACHoldname.Text.ToUpper();
                string pBankName = txtCABankName.Text.ToUpper();
                string pACNo = txtCAAccNo.Text.ToString();
                string pIFSCCode = txtCAIFSCCode.Text.ToString();
                if (txtCAAcYrOfOpen.Text.ToString() != "")
                {
                    YrInOpe = Convert.ToInt32(txtCAAcYrOfOpen.Text.ToString());
                }
                int pCustACType = Convert.ToInt32((Request[ddlCAACType.UniqueID] as string == null) ? ddlCAACType.SelectedValue : Request[ddlCAACType.UniqueID] as string);





                // EMPLOYMENT DETAILS
                int pEmpCurExp = 0, pEmpTotExp = 0, pEmpRetAge = 0;
                string pEmpOrgName = txtCAOrgname.Text.ToString();
                string pEmpEmpDesig = txtCADesig.Text.ToString();
                if (txtCARetiredAge.Text != "")
                    pEmpRetAge = Convert.ToInt32(txtCARetiredAge.Text);
                string pEmpCode = txtCADeptEmpCode.Text.ToString();
                if (txtCACurExp.Text != "")
                    pEmpCurExp = Convert.ToInt32(txtCACurExp.Text);
                if (txtCATotExp.Text != "")
                    pEmpTotExp = Convert.ToInt32(txtCATotExp.Text);

                // BUSINESS DETAILS
                string chkGSTAppYN = "N";
                if (chkCAGSTApp.Checked == true)
                    chkGSTAppYN = "Y";
                string pBusGSTNo = txtCAGSTNo.Text.ToString().ToUpper();
                string pBusLandMark = txtCABusLandmark.Text.ToString();
                string pBusAddress1 = txtCABusAdd1.Text.ToString();
                string pBusAddress2 = txtCABusAdd2.Text.ToString();
                string pBusLocality = txtCABusLocality.Text.ToString();
                string pBusCity = txtCABusCity.Text.ToString();
                string pBusPIN = txtCABusPin.Text.ToString();
                string pBusState = txtCABusState.Text.ToString();
                string pBusMob = txtCABusMob.Text.ToString();
                string pBusPhone = txtCABusPh.Text.ToString();


                string pCommunAddress = Convert.ToString((Request[ddlCACommAddress.UniqueID] as string == null) ? ddlCACommAddress.SelectedValue : Request[ddlCACommAddress.UniqueID] as string);
                string pResidentialStatus = Convert.ToString((Request[ddlCAResdStatus.UniqueID] as string == null) ? ddlCAResdStatus.SelectedValue : Request[ddlCAResdStatus.UniqueID] as string);



                // Dependent Details
                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add("SLNo", typeof(int));
                dtXml.Columns.Add("Name", typeof(string));
                dtXml.Columns.Add("RelationId", typeof(int));
                dtXml.Columns.Add("Age", typeof(int));
                dtXml.Columns.Add("OccupationId", typeof(int));

                foreach (GridViewRow gr in gvCADep.Rows)
                {
                    if (((TextBox)gr.FindControl("txtCADependentNm")).Text != "")
                    {
                        dr = dtXml.NewRow();

                        dr["SLNo"] = ((Label)gr.FindControl("lblSLNoCADep")).Text;
                        dr["Name"] = ((TextBox)gr.FindControl("txtCADependentNm")).Text;
                        if (((DropDownList)gr.FindControl("ddlCADepRel")).SelectedIndex == -1)
                        {
                            ((Label)gr.FindControl("lblCADepRelId")).Text = "-1";
                        }
                        else
                        {
                            ((Label)gr.FindControl("lblCADepRelId")).Text = ((DropDownList)gr.FindControl("ddlCADepRel")).SelectedValue.ToString();
                        }
                        dr["RelationId"] = ((Label)gr.FindControl("lblCADepRelId")).Text;
                        dr["Age"] = ((TextBox)gr.FindControl("txtCADepAge")).Text;
                        if (((DropDownList)gr.FindControl("ddlCADepOcc")).SelectedIndex == -1)
                        {
                            ((Label)gr.FindControl("lblCADepOccId")).Text = "-1";
                        }
                        else
                        {
                            ((Label)gr.FindControl("lblCADepOccId")).Text = ((DropDownList)gr.FindControl("ddlCADepOcc")).SelectedValue.ToString();
                        }
                        dr["OccupationId"] = ((Label)gr.FindControl("lblCADepOccId")).Text;
                        dtXml.Rows.Add(dr);
                    }
                    dtXml.AcceptChanges();
                }
                dtXml.TableName = "Table1";
                string vXml = "";
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
                }

                if (Mode == "Save")
                {
                    oMem = new CMember();

                    if (ddlCoAppPhotoIdType.SelectedIndex.ToString() != "-1" && txtCoAppPNo.Text.Trim() != "")
                    {
                        vMsg = "";
                        vMsg = oMem.GetMemBrByIDTypeNo(Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue), Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue) == 6 ? hdnCoApplAadharNo.Value : txtCoAppPNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Save");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppPNo);
                            return false;
                        }
                    }
                    if (ddlCoAppAddressIdType.SelectedIndex.ToString() != "-1" && txtCoAppAddressIdNo.Text.Trim() != "")
                    {
                        vMsg = "";
                        vMsg = oMem.GetAddressIDNoByAddIDNo(Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue), Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue) == 6 ? hdnCoApplAadharNo.Value : txtCoAppAddressIdNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Save");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppPNo);
                            return false;
                        }
                    }
                    if (vNominee == "Y")
                    {
                        vMsg = "";
                        vMsg = oMem.CheckDuplicateNomineeForCust(vBrCode, vCustId, "Save", vCoAppId);
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }
                    DataTable dt = new DataTable();
                    dt = oMem.ChkDupCOApp(vCustId, vCoAppNm);
                    if (Convert.ToInt32(dt.Rows[0]["CoAppRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Same Co Applicant Name already linked with this Customer... Kindly Check");
                        return false;
                    }

                    //  if (ValidateCoAppField() == false) return false;
                    vErr = oMem.SaveCoApplicant(ref vNewId, vCoAppId, ref vCoAppNo, vCustId, vCoAppDOA,
                        vCoAppNm, vCoAppDOB, vCoAppAge, vCoApGen, vCoApRelig, vCoApCast, vCoApOccuId, vCoApMarital, vCoAppQulId, vCoAppPreAdd1, vCoAppPreAdd2,
                        vCoAppPreDist, vCoAppPreState, vCoAppPrePIN, vCoAppSamAdd,
                        vCoAppPerAdd1, vCoAppPerAdd2, vCoAppPerDist, vCoAppPerState, vCoAppPerPIN, vCoAppAId, vCoAppAId == 6 ? hdnCoApplAadharNo.Value : vCoAppANo,
                        vCoAppPId, vCoAppPId == 6 ? hdnCoApplAadharNo.Value : vCoAppPNo, vCoAppMNo, vCoAppTelNo, vCoAppEmail,
                        vCoAppFaxNo, vCoAppIsDir, vCoAppPart, vCoAppPropietor, vCoAppSpouse, vSameAddAsApp,
                        vBrCode, vCoAppYAR, vCoAppYIB, Convert.ToInt32(hdUserID.Value), "Save", 0, vGuarantor, vNominee, vIsActive, vShareHolPer,
                        vCoAppType, vComTypeId, vPropertyTypeId, vAppname, vIsPrimaryCoApp, vCustCoAppRel, vRelationId, vRelativeName, vXml,
                        pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                        pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pCommunAddress, pResidentialStatus, vCoApOccuId2, vCoApOccuId3, txtCABusinessName.Text);
                    if (vErr > 0)
                    {
                        string vNewCoAppId = vNewId;

                        string FileExten = "";
                        try
                        {
                            if (fuCAAddProofFront.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAAddProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAAddProofFront, vNewCoAppId, "AddressImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCAAddProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAAddProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAAddProofBack, vNewCoAppId, "AddressImageBack", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Back Section");
                                    return false;
                                }
                            }

                            if (fuCAPhotProofFront.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAPhotProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAPhotProofFront, vNewCoAppId, "PhotoImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCAPhotProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAPhotProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAPhotProofBack, vNewCoAppId, "PhotoImageBack", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Back Section");
                                    return false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        //  hdnMemId.Value = vNewId.Trim();
                        ViewState["MemId"] = vNewId;
                        txtCoAppNo.Text = vCoAppNo;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        // tdMsg.Visible = true;
                        lblValMsgCoApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        //gblFuction.MsgPopup(gblPRATAM.DBError);
                        // tdMsg.Visible = true;
                        lblValMsgCoApp.Text = vMsg;
                        lblValMsgCoApp.Text = "";
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {

                    oMem = new CMember();
                    oGbl = new CGblIdGenerator();
                    // Duplicate Photo Id No Checking....
                    if (ddlCoAppPhotoIdType.SelectedIndex.ToString() != "-1" && txtCoAppPNo.Text.Trim() != "")
                    {
                        vMsg = "";
                        vMsg = oMem.GetMemBrByIDTypeNo(Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue), Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue) == 6 ? hdnCoApplAadharNo.Value : txtCoAppPNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Edit");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppPNo);
                            return false;
                        }
                    }
                    // Duplicate Address Id No Checking....
                    if (ddlCoAppAddressIdType.SelectedIndex.ToString() != "-1" && txtCoAppAddressIdNo.Text.Trim() != "")
                    {
                        vMsg = "";
                        vMsg = oMem.GetAddressIDNoByAddIDNo(Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue), Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue) == 6 ? hdnCoApplAadharNo.Value : txtCoAppAddressIdNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Edit");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppAddressIdNo);
                            return false;
                        }
                    }
                    if (vNominee == "Y")
                    {
                        vMsg = "";
                        vMsg = oMem.CheckDuplicateNomineeForCust(vBrCode, vCustId, "Edit", vCoAppId);
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            return false;
                        }
                    }
                    vErr = oMem.SaveCoApplicant(ref vNewId, vCoAppId, ref vCoAppNo, vCustId, vCoAppDOA,
                        vCoAppNm, vCoAppDOB, vCoAppAge, vCoApGen, vCoApRelig, vCoApCast, vCoApOccuId, vCoApMarital, vCoAppQulId, vCoAppPreAdd1, vCoAppPreAdd2,
                        vCoAppPreDist, vCoAppPreState, vCoAppPrePIN, vCoAppSamAdd,
                        vCoAppPerAdd1, vCoAppPerAdd2, vCoAppPerDist, vCoAppPerState, vCoAppPerPIN, vCoAppAId, vCoAppAId == 6 ? hdnCoApplAadharNo.Value : vCoAppANo,
                        vCoAppPId, vCoAppPId == 6 ? hdnCoApplAadharNo.Value : vCoAppPNo,
                        vCoAppMNo, vCoAppTelNo, vCoAppEmail, vCoAppFaxNo, vCoAppIsDir, vCoAppPart, vCoAppPropietor, vCoAppSpouse, vSameAddAsApp,
                        vBrCode, vCoAppYAR, vCoAppYIB, Convert.ToInt32(hdUserID.Value), "Edit", 0, vGuarantor, vNominee,
                        vIsActive, vShareHolPer, vCoAppType, vComTypeId, vPropertyTypeId, vAppname, vIsPrimaryCoApp, vCustCoAppRel, vRelationId,
                        vRelativeName, vXml,
                         pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                        pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pCommunAddress, pResidentialStatus, vCoApOccuId2, vCoApOccuId3, txtCABusinessName.Text);

                    if (vErr > 0)
                    {
                        string FileExten = "";
                        try
                        {
                            if (fuCAAddProofFront.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAAddProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAAddProofFront, vCoAppId, "AddressImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCAAddProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAAddProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAAddProofBack, vCoAppId, "AddressImageBack", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Back Section");
                                    return false;
                                }
                            }

                            if (fuCAPhotProofFront.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAPhotProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAPhotProofFront, vCoAppId, "PhotoImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCAPhotProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCAPhotProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCAPhotProofBack, vCoAppId, "PhotoImageBack", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Back Section");
                                    return false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        // tdMsg.Visible = false;
                        lblValMsgCoApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        //gblFuction.MsgPopup(gblPRATAM.DBError);
                        lblValMsgCoApp.Text = gblPRATAM.DBError;
                        //tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oMem = new CMember();
                    vErr = oMem.SaveCoApplicant(ref vNewId, vCoAppId, ref vCoAppNo, vCustId, vCoAppDOA,
                         vCoAppNm, vCoAppDOB, vCoAppAge, vCoApGen, vCoApRelig, vCoApCast, vCoApOccuId, vCoApMarital, vCoAppQulId, vCoAppPreAdd1, vCoAppPreAdd2,
                         vCoAppPreDist, vCoAppPreState, vCoAppPrePIN, vCoAppSamAdd,
                         vCoAppPerAdd1, vCoAppPerAdd2, vCoAppPerDist, vCoAppPerState, vCoAppPerPIN, vCoAppAId, vCoAppAId == 6 ? hdnCoApplAadharNo.Value : vCoAppANo,
                         vCoAppPId, vCoAppPId == 6 ? hdnCoApplAadharNo.Value : vCoAppPNo,
                         vCoAppMNo, vCoAppTelNo, vCoAppEmail, vCoAppFaxNo, vCoAppIsDir, vCoAppPart, vCoAppPropietor, vCoAppSpouse, vSameAddAsApp,
                         vBrCode, vCoAppYAR, vCoAppYIB, Convert.ToInt32(hdUserID.Value), "Delete", 0, vGuarantor, vNominee,
                         vIsActive, vShareHolPer, vCoAppType, vComTypeId, vPropertyTypeId, vAppname, vIsPrimaryCoApp, vCustCoAppRel, vRelationId,
                         vRelativeName, vXml,
                          pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                         pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                         pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pCommunAddress, pResidentialStatus, vCoApOccuId2, vCoApOccuId3, txtCABusinessName.Text);

                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup("Co Applicant Record Deleted Successfully");
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);

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
                oGbl = null;
            }

        }
        private Boolean SaveCompanyRecords(string Mode)
        {
            Boolean vResult = false;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Please Log In To Branch to Save/Update Customer Records");
                return false;
            }
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCompanyId = "";
            if (DOFCom.Text == "")
            {
                gblFuction.MsgPopup("Company Formation Date Can Not Be Blank...");
                DOFCom.Focus();
                return false;
            }
            DateTime vDOF = gblFuction.setDate(DOFCom.Text);
            DateTime vDOB = gblFuction.setDate(txtCustDOB.Text);

            string vCompanyNo = "", vComSamAdd = "N", vNewId = "", vCompanyName = "", vOtherPropertyDtls = "", vWebSite = "", vEmail = "";
            string vMAddress1 = "", vMAddress2 = "", vMState = "", vMDistrict = "", vMPIN = "", vMMobNo = "", vMSTD = "", vMTelNo = "", vRAddress1 = "",
                vRAddress2 = "", vRState = "", vRDistrict = "", vRPIN = "", vRMobNo = "", vRSTD = "", vRTelNo = "", vAppName = "", vAppContNo = "",
                vCustType = "";
            Int32 vErr = 0, vComAId = 0, vComPID = 0, pOccupationId = 0, pOccupationId2 = 0, pOccupationId3 = 0, pBusinessTypeId,
                pGenderId = 0, pAge = 0, vRelationId = 0, vMfhRelationId = 0, vSpeciallyAbled = -1;
            string vComPANNo = "", vComAIDNO = "", vComPIDNO = "", vIsRegister = "", vRegistNo = "", vGSTRegistNo = "", vRelativeName = "", vMfhName = "";
            Int32 vCompanyTypeId = 0, vSectorId = 0, vSubSectorId = 0, vPropertyTypeId = 0;
            string vMsg = "", vIsAbledYN = "", vMinorityYN = "";
            CMember oMem = new CMember();
            //PathKYCImage = ConfigurationManager.AppSettings["PathKYCImage"];
            PathKYCImage = "G:\\WebApps\\CentrumSmeMobService\\CustomerKYCImage";

            //CCGT oCGT = null;
            try
            {
                if (hdfComId.Value != "")
                {
                    vCompanyId = hdfComId.Value;
                    vCompanyNo = hdfComId.Value;
                }
                vCompanyName = Convert.ToString(txtComName.Text.ToUpper().Trim().Replace("'", ""));
                if (vCompanyName == "")
                {
                    gblFuction.MsgPopup("Company Name can not be blank...");
                    txtComName.Focus();
                    return false;
                }
                vCompanyTypeId = Convert.ToInt32((Request[ddlComType.UniqueID] as string == null) ? ddlComType.SelectedValue : Request[ddlComType.UniqueID] as string);

                vCustType = rbComType.SelectedValue.ToString();
                vAppName = Convert.ToString(txtAppName.Text.ToUpper().Trim());
                vAppContNo = Convert.ToString(txtAppContNo.Text.ToUpper().Trim());
                if (Convert.ToString((Request[txtCustAge.UniqueID] as string == null) ? txtCustAge.Text : Request[txtCustAge.UniqueID] as string) != "")
                {
                    pAge = Convert.ToInt32((Request[txtCustAge.UniqueID] as string == null) ? txtCustAge.Text : Request[txtCustAge.UniqueID] as string);
                }
                if (pAge < 18)
                {
                    gblFuction.MsgPopup("Customer Age Must be Above 18 yrs...");
                    return false;
                }
                vPropertyTypeId = Convert.ToInt32((Request[ddlComProType.UniqueID] as string == null) ? ddlComProType.SelectedValue : Request[ddlComProType.UniqueID] as string);
                vOtherPropertyDtls = Convert.ToString((Request[txtOtherProperDtl.UniqueID] as string == null) ? txtOtherProperDtl.Text : Request[txtOtherProperDtl.UniqueID] as string);
                if (ddlComSec.SelectedValue != "-1")
                {
                    vSectorId = Convert.ToInt32((Request[ddlComSec.UniqueID] as string == null) ? ddlComSec.SelectedValue : Request[ddlComSec.UniqueID] as string);
                }
                if (ddlComSubSec.SelectedValue != "")
                {
                    vSubSectorId = Convert.ToInt32((Request[ddlComSubSec.UniqueID] as string == null) ? ddlComSubSec.SelectedValue : Request[ddlComSubSec.UniqueID] as string);
                }


                if (ddlCustRel.SelectedValue != "-1")
                {
                    vRelationId = Convert.ToInt32((Request[ddlCustRel.UniqueID] as string == null) ? ddlCustRel.SelectedValue : Request[ddlCustRel.UniqueID] as string);
                }
                vRelativeName = txtCustRelName.Text.ToString();
                vWebSite = Convert.ToString((Request[txtComWebSite.UniqueID] as string == null) ? txtComWebSite.Text : Request[txtComWebSite.UniqueID] as string);
                vEmail = Convert.ToString((Request[txtComEmail.UniqueID] as string == null) ? txtComEmail.Text : Request[txtComEmail.UniqueID] as string);
                //if (txtComPanNo.Text == "")
                //{
                //    gblFuction.MsgPopup("PAN No can not be blank....");
                //    txtComPanNo.Focus();
                //    return false;
                //}
                //else if (txtComPanNo.Text.Length != 10)
                //{
                //    gblFuction.MsgPopup("PAN No must be 10 character...");
                //    txtComPanNo.Focus();
                //    return false;
                //}
                //else
                //{
                //    vComPANNo = Convert.ToString((Request[txtComPanNo.UniqueID] as string == null) ? txtComPanNo.Text.ToUpper() : Request[txtComPanNo.UniqueID] as string);
                //}

                if (ddlComAddTypeID.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Company Address Type ID....");
                    ddlComAddTypeID.Focus();
                    return false;
                }
                if (txtComAddIDNo.Text == "")
                {
                    gblFuction.MsgPopup("Company Address ID No can not be blank..");
                    txtComAddIDNo.Focus();
                    return false;
                }

                vMfhRelationId = Convert.ToInt32((Request[ddlMHFRelation.UniqueID] as string == null) ? ddlMHFRelation.SelectedValue : Request[ddlMHFRelation.UniqueID] as string);
                vMfhName = Convert.ToString((Request[txtMHFName.UniqueID] as string == null) ? txtMHFName.Text : Request[txtMHFName.UniqueID] as string);

                vComAId = Convert.ToInt32((Request[ddlComAddTypeID.UniqueID] as string == null) ? ddlComAddTypeID.SelectedValue : Request[ddlComAddTypeID.UniqueID] as string);
                vComAIDNO = Convert.ToString((Request[txtComAddIDNo.UniqueID] as string == null) ? txtComAddIDNo.Text : Request[txtComAddIDNo.UniqueID] as string);
                vComPID = Convert.ToInt32((Request[ddlCustPhotoTypeId.UniqueID] as string == null) ? ddlCustPhotoTypeId.SelectedValue : Request[ddlCustPhotoTypeId.UniqueID] as string);
                vComPIDNO = Convert.ToString((Request[txtCustPhotIDNo.UniqueID] as string == null) ? txtCustPhotIDNo.Text : Request[txtCustPhotIDNo.UniqueID] as string);
                if (chkComRegCer.Checked == true)
                    vIsRegister = "Y";
                else
                    vIsRegister = "N";
                vRegistNo = Convert.ToString((Request[txtComRegisNo.UniqueID] as string == null) ? txtComRegisNo.Text : Request[txtComRegisNo.UniqueID] as string);
                vGSTRegistNo = Convert.ToString((Request[txtGSTRegNo.UniqueID] as string == null) ? txtGSTRegNo.Text : Request[txtGSTRegNo.UniqueID] as string);

                vMAddress1 = Convert.ToString((Request[txtMAdd1.UniqueID] as string == null) ? txtMAdd1.Text : Request[txtMAdd1.UniqueID] as string);
                vMAddress2 = Convert.ToString((Request[txtMAdd2.UniqueID] as string == null) ? txtMAdd2.Text : Request[txtMAdd2.UniqueID] as string);
                vMState = Convert.ToString((Request[ddlMState.UniqueID] as string == null) ? ddlMState.SelectedItem.Text : Request[ddlMState.UniqueID] as string);
                vMDistrict = Convert.ToString((Request[txtMDist.UniqueID] as string == null) ? txtMDist.Text : Request[txtMDist.UniqueID] as string);
                vMPIN = Convert.ToString((Request[txtMPIN.UniqueID] as string == null) ? txtMPIN.Text : Request[txtMPIN.UniqueID] as string);
                vMMobNo = Convert.ToString((Request[txtMMobNo.UniqueID] as string == null) ? txtMMobNo.Text : Request[txtMMobNo.UniqueID] as string);
                vMSTD = Convert.ToString((Request[txtMSTD.UniqueID] as string == null) ? txtMSTD.Text : Request[txtMSTD.UniqueID] as string);
                vMTelNo = Convert.ToString((Request[txtMTel.UniqueID] as string == null) ? txtMTel.Text : Request[txtMTel.UniqueID] as string);
                if (vMAddress1 == "" || vMState == "-1" || vMPIN == "" || vMDistrict == "")
                {
                    gblFuction.MsgPopup("Any of the field of Present Address1/State/PIN/District No Can not be Blank....");
                    txtMAdd1.Focus();
                    return false;
                }
                if (chkComSameAdd.Checked == true)
                    vComSamAdd = "Y";
                vRAddress1 = Convert.ToString((Request[txtROffAdd1.UniqueID] as string == null) ? txtROffAdd1.Text : Request[txtROffAdd1.UniqueID] as string);
                vRAddress2 = Convert.ToString((Request[txtROffAdd2.UniqueID] as string == null) ? txtROffAdd2.Text : Request[txtROffAdd2.UniqueID] as string);
                vRState = Convert.ToString((Request[ddlRState.UniqueID] as string == null) ? ddlRState.SelectedValue : Request[ddlRState.UniqueID] as string);
                vRDistrict = Convert.ToString((Request[txtRDist.UniqueID] as string == null) ? txtRDist.Text : Request[txtRDist.UniqueID] as string);
                vRPIN = Convert.ToString((Request[txtRPIN.UniqueID] as string == null) ? txtRPIN.Text : Request[txtRPIN.UniqueID] as string);
                vRMobNo = Convert.ToString((Request[txtRMobNo.UniqueID] as string == null) ? txtRMobNo.Text : Request[txtRMobNo.UniqueID] as string);
                vRSTD = Convert.ToString((Request[txtRSTD.UniqueID] as string == null) ? txtRSTD.Text : Request[txtRSTD.UniqueID] as string);
                vRTelNo = Convert.ToString((Request[txtRTel.UniqueID] as string == null) ? txtRTel.Text : Request[txtRTel.UniqueID] as string);
                int pCustRelId = Convert.ToInt32((Request[ddlCustReligion.UniqueID] as string == null) ? ddlCustReligion.SelectedValue : Request[ddlCustReligion.UniqueID] as string);

                if (vRAddress1 == "" || vRState == "-1" || vRPIN == "" || vRDistrict == "")
                {
                    gblFuction.MsgPopup("Any of the field of Permenant Address1/State/PIN/District No Can not be Blank....");
                    txtMAdd1.Focus();
                    return false;
                }

                pOccupationId = Convert.ToInt32((Request[ddlCustOccupation.UniqueID] as string == null) ? ddlCustOccupation.SelectedValue : Request[ddlCustOccupation.UniqueID] as string);
                pOccupationId2 = Convert.ToInt32((Request[ddlCustOccupation2.UniqueID] as string == null) ? ddlCustOccupation2.SelectedValue : Request[ddlCustOccupation2.UniqueID] as string);
                pOccupationId3 = Convert.ToInt32((Request[ddlCustOccupation3.UniqueID] as string == null) ? ddlCustOccupation3.SelectedValue : Request[ddlCustOccupation3.UniqueID] as string);

                pBusinessTypeId = Convert.ToInt32((Request[ddlCustBusType.UniqueID] as string == null) ? ddlCustBusType.SelectedValue : Request[ddlCustBusType.UniqueID] as string);
                string pBusinessLocation = txtCustBusLocation.Text.ToString();
                decimal pAnnualIncome = 0;
                if (txtCustAnnualInc.Text != "")
                    pAnnualIncome = Convert.ToDecimal(txtCustAnnualInc.Text);
                pGenderId = Convert.ToInt32((Request[ddlCustGen.UniqueID] as string == null) ? ddlCustGen.SelectedValue : Request[ddlCustGen.UniqueID] as string);

                vSpeciallyAbled = Convert.ToInt32(ddlSpclAbled.SelectedValue);
                vMinorityYN = ddlMinorityYN.SelectedValue;
                vIsAbledYN = ddlAbledYN.SelectedValue;


                //Newly Added
                int pCustQuaId = Convert.ToInt32((Request[ddlCustQual.UniqueID] as string == null) ? ddlCustQual.SelectedValue : Request[ddlCustQual.UniqueID] as string);
                int pCustReligionId = Convert.ToInt32((Request[ddlCustReligion.UniqueID] as string == null) ? ddlCustReligion.SelectedValue : Request[ddlCustReligion.UniqueID] as string);
                int pCustCasteId = Convert.ToInt32((Request[ddlCustCaste.UniqueID] as string == null) ? ddlCustCaste.SelectedValue : Request[ddlCustCaste.UniqueID] as string);
                //pCustQuaId,pCustReligionId,pCustCasteId,pYrInRe,pPhyChallengedYN,pAcHolName,pACNo,pBankName,pIFSCCode,YrInOpe,pCustACType

                // BANK ACCOUNT DETAILS
                decimal pYrInRe = 0;
                if (txtYrInCurRes.Text != "")
                    pYrInRe = Convert.ToDecimal(txtYrInCurRes.Text);
                string pPhyChallengedYN = ddlCustPhyChallenge.SelectedValue.ToString();
                string pAcHolName = txtCustACHoldname.Text.ToUpper();
                string pBankName = txtCustBankName.Text.ToUpper();
                string pACNo = txtCustAccNo.Text.ToString();
                string pIFSCCode = txtCustIFSCCode.Text.ToString();
                int YrInOpe = Convert.ToInt32(txtCustAcYrOfOpen.Text.ToString());
                int pCustACType = Convert.ToInt32((Request[ddlCustACType.UniqueID] as string == null) ? ddlCustACType.SelectedValue : Request[ddlCustACType.UniqueID] as string);
                string pConNo = txtCustContNo.Text.ToString();


                // EMPLOYMENT DETAILS
                int pEmpCurExp = 0, pEmpTotExp = 0, pEmpRetAge = 0;
                string pEmpOrgName = txtCustOrgname.Text.ToString();
                string pEmpEmpDesig = txtCustDesig.Text.ToString();
                if (txtCustRetiredAge.Text != "")
                    pEmpRetAge = Convert.ToInt32(txtCustRetiredAge.Text);
                string pEmpCode = txtCustDeptEmpCode.Text.ToString();
                if (txtCustCurExp.Text != "")
                    pEmpCurExp = Convert.ToInt32(txtCustCurExp.Text);
                if (txtCustTotExp.Text != "")
                    pEmpTotExp = Convert.ToInt32(txtCustTotExp.Text);

                // BUSINESS DETAILS
                string chkGSTAppYN = "N";
                if (chkGSTApp.Checked == true)
                    chkGSTAppYN = "Y";
                string pBusGSTNo = txtCustGSTNo.Text.ToString().ToUpper();
                string pBusLandMark = txtCustBusLandmark.Text.ToString();
                string pBusAddress1 = txtCustBusAdd1.Text.ToString();
                string pBusAddress2 = txtCustBusAdd2.Text.ToString();
                string pBusLocality = txtCustBusLocality.Text.ToString();
                string pBusCity = txtCustBusCity.Text.ToString();
                string pBusPIN = txtCustBusPin.Text.ToString();
                string pBusState = txtCustBusState.Text.ToString();
                string pBusMob = txtCustBusMob.Text.ToString();
                string pBusPhone = txtCustBusPh.Text.ToString();

                string pNomineeName = txtNomineeName.Text;
                string pNomineeDOB = txtNomineeDOB.Text;
                string pNomineePinCode = txtNomineePin.Text;
                string pNomineeAddress = txtNomineeAddress.Text;
                int pNomineeGender = Convert.ToInt32(ddlNomineeGender.SelectedValue);
                int pNomineeRelation = Convert.ToInt32(ddlNomineeRel.SelectedValue);
                int pNomineeState = Convert.ToInt32(ddlNomineeState.SelectedValue);


                int pMaritalStat = 0;
                pMaritalStat = Convert.ToInt32((Request[ddlCustMS.UniqueID] as string == null) ? ddlCustMS.SelectedValue : Request[ddlCustMS.UniqueID] as string);
                string pCommunAddress = Convert.ToString((Request[ddlCustCommAddress.UniqueID] as string == null) ? ddlCustCommAddress.SelectedValue : Request[ddlCustCommAddress.UniqueID] as string);
                string pResidentialStatus = Convert.ToString((Request[ddlCustResdStatus.UniqueID] as string == null) ? ddlCustResdStatus.SelectedValue : Request[ddlCustResdStatus.UniqueID] as string);
                //pEmpOrgName,pEmpEmpDesig,pEmpRetAge,pEmpCode,pEmpCurExp,pEmpTotExp,chkGSTAppYN,pBusGSTNo,pBusLandMark,pBusAddress1,pBusAddress2,pBusLocality,
                //pBusCity,pBusPIN,pBusState,pBusMob,pBusPhone
                //pMaritalStat,pCommunAddress,pResidentialStatus


                // Dependent Details
                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add("SLNo", typeof(int));
                dtXml.Columns.Add("Name", typeof(string));
                dtXml.Columns.Add("RelationId", typeof(int));
                dtXml.Columns.Add("Age", typeof(int));
                dtXml.Columns.Add("OccupationId", typeof(int));

                foreach (GridViewRow gr in gvCustDep.Rows)
                {
                    if (((TextBox)gr.FindControl("txtCustDependentNm")).Text != "")
                    {
                        dr = dtXml.NewRow();

                        dr["SLNo"] = ((Label)gr.FindControl("lblSLNoCustDep")).Text;
                        dr["Name"] = ((TextBox)gr.FindControl("txtCustDependentNm")).Text;
                        if (((DropDownList)gr.FindControl("ddlCustDepRel")).SelectedIndex == -1)
                        {
                            ((Label)gr.FindControl("lblCustDepRelId")).Text = "-1";
                        }
                        else
                        {
                            ((Label)gr.FindControl("lblCustDepRelId")).Text = ((DropDownList)gr.FindControl("ddlCustDepRel")).SelectedValue.ToString();
                        }
                        dr["RelationId"] = ((Label)gr.FindControl("lblCustDepRelId")).Text;
                        dr["Age"] = ((TextBox)gr.FindControl("txtCustDepAge")).Text;
                        if (((DropDownList)gr.FindControl("ddlCustDepOcc")).SelectedIndex == -1)
                        {
                            ((Label)gr.FindControl("lblCustDepOccId")).Text = "-1";
                        }
                        else
                        {
                            ((Label)gr.FindControl("lblCustDepOccId")).Text = ((DropDownList)gr.FindControl("ddlCustDepOcc")).SelectedValue.ToString();
                        }
                        dr["OccupationId"] = ((Label)gr.FindControl("lblCustDepOccId")).Text;
                        dtXml.Rows.Add(dr);
                    }
                    dtXml.AcceptChanges();
                }
                dtXml.TableName = "Table1";
                string vXml = "";
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
                }

                if (Mode == "Save")
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    // Check For Duplicate PAN No
                    dt = oMem.ChkDupPANNo(vComPANNo);
                    if (Convert.ToInt32(dt.Rows[0]["PANRec"]) > 0)
                    {
                        gblFuction.MsgPopup("PAN No already applied for Another Customer... PAN No Can not be duplicate...");
                        txtComPanNo.Focus();
                        return false;
                    }

                    // Check For Duplicate Address Id No
                    dt1 = oMem.ChkDupAddressIdNo(vComAId == 6 ? hdnApplAadharNo.Value : vComAIDNO);
                    if (Convert.ToInt32(dt1.Rows[0]["AddressIdNo"]) > 0)
                    {
                        gblFuction.MsgPopup("AddressId No already applied for Another Customer... It Can not be duplicate...");
                        txtComAddIDNo.Focus();
                        return false;
                    }
                    vErr = oMem.SaveCompanyNew(ref vNewId, vCompanyId, ref vCompanyNo, vCompanyTypeId, vCompanyName, vDOF, vDOB, vPropertyTypeId, vOtherPropertyDtls, vWebSite, vEmail,
                        vComPANNo, vComAId, vComAId == 6 ? hdnApplAadharNo.Value : vComAIDNO, vIsRegister, vRegistNo, vSectorId, vSubSectorId, vMAddress1, vMAddress2, vMState, vMDistrict,
                        vMPIN, vMMobNo, vMSTD, vMTelNo, vComSamAdd, vRAddress1, vRAddress2, vRState, vRDistrict, vRPIN, vRMobNo, vRSTD, vRTelNo,
                        vBrCode, Convert.ToInt32(hdUserID.Value), "Save", 0, vAppName, vAppContNo, vGSTRegistNo, vCustType,
                        vComPID, vComPID == 6 ? hdnApplAadharNo.Value : vComPIDNO, pOccupationId, pBusinessTypeId, pBusinessLocation, pAnnualIncome, pGenderId, pAge, vRelationId, vRelativeName,
                        pCustQuaId, pCustReligionId, pCustCasteId, pYrInRe, pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                        pConNo, pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml,
                        pOccupationId2, pOccupationId3, txtBusinessName.Text, Convert.ToInt16(txtYrInCurBusi.Text), vMfhName, vMfhRelationId, vIsAbledYN,
                        vSpeciallyAbled, vMinorityYN,pNomineeName,gblFuction.setDate(pNomineeDOB),pNomineePinCode,pNomineeAddress,pNomineeGender,pNomineeRelation,
                        pNomineeState);
                    if (vErr > 0)
                    {
                        hdfComId.Value = vNewId.Trim();
                        ViewState["MemId"] = vNewId;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        //tdMsg.Visible = true;
                        lblValMsgCompany.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        // tdMsg.Visible = true;
                        lblValMsgCompany.Text = vMsg;
                        txtCustNoApp.Text = "";
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    DataTable dt2 = new DataTable();
                    CustomerId = vCompanyId;
                    //  if (ValidateCompanyFields() == false) return false;
                    oMem = new CMember();
                    string vErrMsg = "";
                    vErr = oMem.chkDDup(CustomerId, vComAId == 6 ? hdnApplAadharNo.Value : vComAIDNO, vComPID == 6 ? hdnApplAadharNo.Value : vComPIDNO, vMMobNo, pACNo, ref vErrMsg);
                    if (vErr > 0)
                    {
                        gblFuction.AjxMsgPopup(vErrMsg);
                        return false;
                    }
                    int vErr1 = oMem.chkDdupMEL_OWN(vComAId == 6 ? hdnApplAadharNo.Value : vComAIDNO, vComPID == 6 ? hdnApplAadharNo.Value : vComPIDNO, "", vBrCode, ref vErrMsg, vCompanyId);
                    if (vErr1 == 99 || vErr1 == 0 || vErr1 == 57)
                    {
                        if (vErr1 == 99)
                        {
                            string[] arr1 = vErrMsg.Split('#');
                            vCompanyNo = arr1[1];
                        }

                        oMem = new CMember();
                        vErr = oMem.SaveCompanyNew(ref vNewId, vCompanyId, ref vCompanyNo, vCompanyTypeId, vCompanyName, vDOF, vDOB, vPropertyTypeId, vOtherPropertyDtls, vWebSite, vEmail,
                            vComPANNo, vComAId, vComAId == 6 ? hdnApplAadharNo.Value : vComAIDNO, vIsRegister, vRegistNo, vSectorId, vSubSectorId, vMAddress1, vMAddress2, vMState, vMDistrict,
                            vMPIN, vMMobNo, vMSTD, vMTelNo, vComSamAdd, vRAddress1, vRAddress2, vRState, vRDistrict, vRPIN, vRMobNo, vRSTD, vRTelNo,
                            vBrCode, Convert.ToInt32(hdUserID.Value), "Edit", 0, vAppName, vAppContNo, vGSTRegistNo, vCustType,
                            vComPID, vComPID == 6 ? hdnApplAadharNo.Value : vComPIDNO, pOccupationId, pBusinessTypeId, pBusinessLocation, pAnnualIncome, pGenderId, pAge, vRelationId, vRelativeName,
                            pCustQuaId, pCustReligionId, pCustCasteId, pYrInRe, pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                            pConNo, pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                            pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml,
                            pOccupationId2, pOccupationId3, txtBusinessName.Text, Convert.ToInt16(txtYrInCurBusi.Text), vMfhName, vMfhRelationId, vIsAbledYN,
                        vSpeciallyAbled, vMinorityYN, pNomineeName, gblFuction.setDate(pNomineeDOB), pNomineePinCode, pNomineeAddress, pNomineeGender, pNomineeRelation,
                        pNomineeState);

                        if (vErr > 0)
                        {
                            //gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            // tdMsg.Visible = false;
                            lblValMsgCompany.Text = "";
                            vResult = true;
                            string FileExten = "";
                            try
                            {
                                if (fuCustAddProofFront.HasFile)
                                {
                                    //FileExten = System.IO.Path.GetExtension(fuCustAddProofFront.FileName).ToLower();
                                    //if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                    //{
                                    //    string vMessage = SaveMemberImages(fuCustAddProofFront, CustomerId, "AddressImageFront", "Edit", "N", PathKYCImage);
                                    //}
                                    //else
                                    //{
                                    //    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Front Section");
                                    //    return false;
                                    //} 
                                    SaveMemberImage(fuCustAddProofFront, vCompanyId, "Applicant KYC ID 1 Front", "Edit", "N", "");
                                }
                                if (fuCustAddProofBack.HasFile)
                                {
                                    //FileExten = System.IO.Path.GetExtension(fuCustAddProofBack.FileName).ToLower();
                                    //if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                    //{
                                    //    string vMessage = SaveMemberImages(fuCustAddProofBack, CustomerId, "AddressImageBack", "Edit", "N", PathKYCImage);
                                    //}
                                    //else
                                    //{
                                    //    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Back Section");
                                    //    return false;
                                    //}
                                    SaveMemberImage(fuCustAddProofBack, vCompanyId, "Applicant KYC ID 1 Back", "Edit", "N", "");
                                }

                                if (fuCustPhotProofFront.HasFile)
                                {
                                    //FileExten = System.IO.Path.GetExtension(fuCustPhotProofFront.FileName).ToLower();
                                    //if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                    //{
                                    //    string vMessage = SaveMemberImages(fuCustPhotProofFront, CustomerId, "PhotoImageFront", "Edit", "N", PathKYCImage);
                                    //}
                                    //else
                                    //{
                                    //    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Front Section");
                                    //    return false;
                                    //}
                                    SaveMemberImage(fuCustPhotProofFront, vCompanyId, "Applicant KYC ID 2 Front", "Edit", "N", "");
                                }
                                if (fuCustPhotoProofBack.HasFile)
                                {
                                    //FileExten = System.IO.Path.GetExtension(fuCustPhotoProofBack.FileName).ToLower();
                                    //if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                    //{
                                    //    string vMessage = SaveMemberImages(fuCustPhotoProofBack, CustomerId, "PhotoImageBack", "Edit", "N", PathKYCImage);
                                    //}
                                    //else
                                    //{
                                    //    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Back Section");
                                    //    return false;
                                    //}
                                    SaveMemberImage(fuCustPhotoProofBack, vCompanyId, "Applicant KYC ID 2 Back", "Edit", "N", "");
                                }
                                if (fuCustBankPassBook.HasFile)
                                {
                                    SaveMemberImage(fuCustBankPassBook, vCompanyId, "Bank statement", "Edit", "N", "");
                                }

                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            gblFuction.MsgPopup(gblPRATAM.EditMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            lblValMsgCompany.Text = gblPRATAM.DBError;
                            //  tdMsg.Visible = true;
                            vResult = false;
                        }
                    }
                    else
                    {
                        //int vErr2 = oMem.DeleteDuplicateMember(CustomerId);
                        gblFuction.AjxMsgPopup(vErrMsg);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    oMem = new CMember();
                    DataTable ComRec = new DataTable();
                    ComRec = oMem.ChkDelCompanyRecord(vCompanyId);
                    if (Convert.ToInt32(ComRec.Rows[0]["LnAppRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Record Found in Loan Application By this Customer.. Can Not Delete Customer..");
                        return false;
                    }
                    vErr = oMem.SaveCompanyNew(ref vNewId, vCompanyId, ref vCompanyNo, vCompanyTypeId, vCompanyName, vDOF, vDOB, vPropertyTypeId, vOtherPropertyDtls, vWebSite, vEmail,
                         vComPANNo, vComAId, vComAIDNO, vIsRegister, vRegistNo, vSectorId, vSubSectorId, vMAddress1, vMAddress2, vMState, vMDistrict,
                         vMPIN, vMMobNo, vMSTD, vMTelNo, vComSamAdd, vRAddress1, vRAddress2, vRState, vRDistrict, vRPIN, vRMobNo, vRSTD, vRTelNo,
                         vBrCode, Convert.ToInt32(hdUserID.Value), "Delete", 0, vAppName, vAppContNo, vGSTRegistNo, vCustType,
                         vComPID, vComPIDNO, pOccupationId, pBusinessTypeId, pBusinessLocation, pAnnualIncome, pGenderId, pAge, vRelationId, vRelativeName,
                         pCustQuaId, pCustReligionId, pCustCasteId, pYrInRe, pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                         pConNo, pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml,
                         pOccupationId2, pOccupationId3, txtBusinessName.Text, 0, vMfhName, vMfhRelationId, vIsAbledYN,
                        vSpeciallyAbled, vMinorityYN, pNomineeName, gblFuction.setDate(pNomineeDOB), pNomineePinCode, pNomineeAddress, pNomineeGender, pNomineeRelation,
                        pNomineeState);

                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        // tdMsg.Visible = false;
                        lblValMsgCompany.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        //  tdMsg.Visible = true;
                        lblValMsgCompany.Text = gblPRATAM.DBError;
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
        private Boolean SaveLoanAppRecords(string Mode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            if (Session[gblValue.BrnchCode].ToString() == "0000")
            {
                gblFuction.MsgPopup("Please Log In To Branch For Loan Application");
                return false;
            }
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty, vApplicantId = "", vAppId = "", vMachDtl = "", vAddTerms = "";
            Int32 vErr = 0, vPurpId = 0, vTenure = 0, vYrNo = 0, vLnTypeId = 0, vSourceId = 0;
            decimal vLnAmt = 0;

            CApplication oCG = new CApplication();
            string vXml = "", vXmlAsset = "", vPassYN = "", vRejReason = "", vErrDesc = "";
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
                else if (ddlTenure.SelectedValue == "-1")
                {
                    gblFuction.AjxMsgPopup("Please Select Tenure..");
                    ddlTenure.Focus();
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
                    dr["ScoreValue"] = ((Label)gr.FindControl("lblCBScore")).Text;
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

                //if (Request[txtTenure.UniqueID] as string != "")
                //    vTenure = Convert.ToInt32(Request[txtTenure.UniqueID] as string);
                vTenure = Convert.ToInt32((Request[ddlTenure.UniqueID] as string == null) ? ddlTenure.SelectedValue : Request[ddlTenure.UniqueID] as string);
                if (vTenure == 0)
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be zero...");
                    ddlTenure.Focus();
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

                    vErr = oCG.SaveInitialApplication(ref vAppNo, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure, "H",
                      vBrCode, Convert.ToInt32(hdUserID.Value), "I", vYrNo, vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                      vRejReason, vAddTerms, ref vErrDesc);
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
                        ViewState["LoanAppId"] = vAppNo;
                        gblFuction.MsgPopup(vErrDesc);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrDesc);
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
                        ViewState["LoanAppId"] = vAppId;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(vErrDesc);
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
        private Boolean SaveRefRecords(string Mode)
        {
            ViewState["LoanAppId"] = txtAppNoRef.Text.ToString();
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("ApplicationId", typeof(string));
            dtXml.Columns.Add("RefName", typeof(string));
            dtXml.Columns.Add("RefAddr", typeof(string));
            dtXml.Columns.Add("RefMNo", typeof(string));
            dtXml.Columns.Add("RelWithAppId", typeof(int));
            dtXml.Columns.Add("RelWithApp", typeof(string));
            dtXml.Columns.Add("OffTelNo", typeof(string));
            dtXml.Columns.Add("CompName", typeof(string));
            dtXml.Columns.Add("PersonalRef", typeof(string));
            dtXml.Columns.Add("TradeRef", typeof(string));
            dtXml.Columns.Add("RefType", typeof(string));
            dtXml.Columns.Add("Status", typeof(string));
            dtXml.Columns.Add("VarifiedBy", typeof(string));
            foreach (GridViewRow gr in gvRef.Rows)
            {
                dr = dtXml.NewRow();
                dr["ApplicationId"] = ((Label)gr.FindControl("lblAppIdPL")).Text;
                dr["RefName"] = ((Label)gr.FindControl("lblRefName")).Text;
                dr["RefAddr"] = ((Label)gr.FindControl("lblRefAddress")).Text;
                dr["RefMNo"] = ((Label)gr.FindControl("lblRefMob")).Text; ;
                dr["RelWithAppId"] = ((Label)gr.FindControl("lblRelWithAppId")).Text; ;
                dr["RelWithApp"] = ((Label)gr.FindControl("lblRelWithApplicant")).Text; ;
                dr["OffTelNo"] = ((Label)gr.FindControl("lblOffTelNo")).Text; ;
                dr["CompName"] = ((Label)gr.FindControl("lblCompanyName")).Text;
                dr["PersonalRef"] = ((Label)gr.FindControl("lblPersonalRef")).Text; ;
                dr["TradeRef"] = ((Label)gr.FindControl("lblTradeRef")).Text; ;
                dr["RefType"] = ((Label)gr.FindControl("lblRefType")).Text;
                dr["Status"] = ((Label)gr.FindControl("lblStatus")).Text;
                dr["VarifiedBy"] = ((Label)gr.FindControl("lblVarifiedBy")).Text;
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
                Int32 vErr = 0, vRefId = 0;
                string vMsg = "";


                if (hfRefId.Value.ToString() != "")
                {
                    vRefId = Convert.ToInt32(hfRefId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        txtCustNoApp.Text = "";
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = vMsg;
                        txtCustNoApp.Text = "";
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    vErr = oMem.SaveRefData(vRefId, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = false;
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
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
        private void EnableControl(Boolean Status)
        {
            //ddlAppSal.Enabled = Status;
            //txtFName.Enabled = Status;
            //txtMName.Enabled = Status;
            //txtLName.Enabled = Status;
            //ddlCoApp.Enabled = Status;
            //txtAdmDtApp.Enabled = Status;
            //txtCustNameApp.Enabled = Status;
            //txtCustNoApp.Enabled = Status;
            //txtFName.Enabled = Status;
            //txtMName.Enabled = Status;
            //txtLName.Enabled = Status;
            //ddlPhotoIdTypApp.Enabled = Status;
            //txtPhotoNoApp.Enabled = Status;
            //ddlAdrIdTypeApp.Enabled = Status;
            //txtAdrNoApp.Enabled = Status;
            //txtTelNoApp.Enabled = Status;
            //txtCustMobileNo.Enabled = Status;
            //ddlOccuApp.Enabled = Status;
            //ddlGenderApp.Enabled = Status;
            //txtAgeApp.Enabled = Status;
            //txtDOBApp.Enabled = Status;
            //ddlQualApp.Enabled = Status;
            //ddlRelgApp.Enabled = Status;
            //ddlCastApp.Enabled = Status;

            //txtAppPinPre.Enabled = Status;
            //txtAppStatePre.Enabled = Status;
            //txtAppCityPre.Enabled = Status;

            //ddlMSApp.Enabled = Status;
            //txtAppAddress1Pre.Enabled = Status;
            //txtAppAddress2Pre.Enabled = Status;
            //txtCustFaxNo.Enabled = Status;
            //txtCustEmail.Enabled = Status;
            //txtCusITProNo.Enabled = Status;
            //ddlITTypeApp.Enabled = Status;
        }
        private void popQualification()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "QualificationId", "Qualification", "QualificationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppQuali.DataSource = dt;
                    ddlCoAppQuali.DataTextField = "Qualification";
                    ddlCoAppQuali.DataValueField = "QualificationId";
                    ddlCoAppQuali.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppQuali.Items.Insert(0, oli1);

                    ddlCustQual.DataSource = dt;
                    ddlCustQual.DataTextField = "Qualification";
                    ddlCustQual.DataValueField = "QualificationId";
                    ddlCustQual.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustQual.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCoAppQuali.DataSource = null;
                    ddlCoAppQuali.DataBind();
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
        private void popState()
        {
            DataTable dt = null;
            CMember oGb = null;
            try
            {
                oGb = new CMember();
                dt = oGb.GetStateName();
                if (dt.Rows.Count > 0)
                {
                    ddlMState.DataSource = dt;
                    ddlMState.DataTextField = "StateName";
                    ddlMState.DataValueField = "StateId";
                    ddlMState.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlMState.Items.Insert(0, oli1);

                    ddlRState.DataSource = dt;
                    ddlRState.DataTextField = "StateName";
                    ddlRState.DataValueField = "StateName";
                    ddlRState.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlRState.Items.Insert(0, oli2);


                    ddlCoAppStatePre.DataSource = dt;
                    ddlCoAppStatePre.DataTextField = "StateName";
                    ddlCoAppStatePre.DataValueField = "StateName";
                    ddlCoAppStatePre.DataBind();
                    ListItem oli3 = new ListItem("<--Select-->", "-1");
                    ddlCoAppStatePre.Items.Insert(0, oli3);

                    ddlCoAppStatePer.DataSource = dt;
                    ddlCoAppStatePer.DataTextField = "StateName";
                    ddlCoAppStatePer.DataValueField = "StateName";
                    ddlCoAppStatePer.DataBind();
                    ListItem oli4 = new ListItem("<--Select-->", "-1");
                    ddlCoAppStatePer.Items.Insert(0, oli4);

                    ddlNomineeState.DataSource = dt;
                    ddlNomineeState.DataTextField = "StateName";
                    ddlNomineeState.DataValueField = "StateId";
                    ddlNomineeState.DataBind();
                    oli4 = new ListItem("<--Select-->", "-1");
                    ddlNomineeState.Items.Insert(0, oli4);

                }
                else
                {
                    ddlMState.DataSource = null;
                    ddlMState.DataBind();
                    ddlRState.DataSource = null;
                    ddlRState.DataBind();
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
        private void popOccupation()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            try
            {
                oGb = new CGblIdGenerator();
                dt = oGb.PopComboMIS("N", "N", "AA", "OccupationId", "Occupation", "OccupationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppOccu.DataSource = dt;
                    ddlCoAppOccu.DataTextField = "Occupation";
                    ddlCoAppOccu.DataValueField = "OccupationId";
                    ddlCoAppOccu.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu.Items.Insert(0, oli1);

                    ddlCoAppOccu2.DataSource = dt;
                    ddlCoAppOccu2.DataTextField = "Occupation";
                    ddlCoAppOccu2.DataValueField = "OccupationId";
                    ddlCoAppOccu2.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu2.Items.Insert(0, oli2);

                    ddlCoAppOccu3.DataSource = dt;
                    ddlCoAppOccu3.DataTextField = "Occupation";
                    ddlCoAppOccu3.DataValueField = "OccupationId";
                    ddlCoAppOccu3.DataBind();
                    ListItem oli3 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu3.Items.Insert(0, oli3);
                }
                else
                {
                    ddlCoAppOccu.DataSource = null;
                    ddlCoAppOccu.DataBind();
                    ddlCoAppOccu2.DataSource = null;
                    ddlCoAppOccu2.DataBind();
                    ddlCoAppOccu3.DataSource = null;
                    ddlCoAppOccu3.DataBind();
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
        private void popIDProof()
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null;
            CMember oCgt = null;
            try
            {
                oCgt = new CMember();
                ds = oCgt.GetIDProof();
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                if (dt1.Rows.Count > 0)
                {
                    ddlCoAppAddressIdType.DataSource = dt1;
                    ddlCoAppAddressIdType.DataTextField = "IDProofName";
                    ddlCoAppAddressIdType.DataValueField = "IDProofId";
                    ddlCoAppAddressIdType.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppAddressIdType.Items.Insert(0, oli1);

                    ddlComAddTypeID.DataSource = dt1;
                    ddlComAddTypeID.DataTextField = "IDProofName";
                    ddlComAddTypeID.DataValueField = "IDProofId";
                    ddlComAddTypeID.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlComAddTypeID.Items.Insert(0, oli2);


                    //ddlCustAddTypeImage.DataSource = dt1;
                    //ddlCustAddTypeImage.DataTextField = "IDProofName";
                    //ddlCustAddTypeImage.DataValueField = "IDProofId";
                    //ddlCustAddTypeImage.DataBind();
                    //ListItem oli3 = new ListItem("<--Select-->", "-1");
                    //ddlCustAddTypeImage.Items.Insert(0, oli3);

                }
                else
                {
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppAddressIdType.Items.Insert(0, oli1);
                }
                if (dt2.Rows.Count > 0)
                {
                    ddlCoAppPhotoIdType.DataSource = dt2;
                    ddlCoAppPhotoIdType.DataTextField = "IDProofName";
                    ddlCoAppPhotoIdType.DataValueField = "IDProofId";
                    ddlCoAppPhotoIdType.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppPhotoIdType.Items.Insert(0, oli1);

                    ddlCustPhotoTypeId.DataSource = dt2;
                    ddlCustPhotoTypeId.DataTextField = "IDProofName";
                    ddlCustPhotoTypeId.DataValueField = "IDProofId";
                    ddlCustPhotoTypeId.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustPhotoTypeId.Items.Insert(0, oli2);

                    //ddlCustPhotoTypeImage.DataSource = dt2;
                    //ddlCustPhotoTypeImage.DataTextField = "IDProofName";
                    //ddlCustPhotoTypeImage.DataValueField = "IDProofId";
                    //ddlCustPhotoTypeImage.DataBind();
                    //ListItem oli3 = new ListItem("<--Select-->", "-1");
                    //ddlCustPhotoTypeImage.Items.Insert(0, oli3);
                }
                else
                {
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppPhotoIdType.Items.Insert(0, oli1);
                }
                if (dt3.Rows.Count > 0)
                {
                    //ddlCoAppITTypeId.DataSource = dt3;
                    //ddlCoAppITTypeId.DataTextField = "IDProofName";
                    //ddlCoAppITTypeId.DataValueField = "IDProofId";
                    //ddlCoAppITTypeId.DataBind();
                    //ListItem oli1 = new ListItem("<--Select-->", "-1");
                    //ddlCoAppITTypeId.Items.Insert(0, oli1);
                }
                else
                {
                    //ListItem oli1 = new ListItem("<--Select-->", "-1");
                    //ddlCoAppITTypeId.Items.Insert(0, oli1);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ds = null;
                dt1 = null;
                dt2 = null;
                oCgt = null;
            }
        }
        private void PopCustomer()
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            CApplication oCA = new CApplication();
            DataTable dt = null;
            try
            {
                dt = oCA.GetCustomer(vBrCode);
                if (dt.Rows.Count > 0)
                {
                    ddlCustName.DataSource = dt;
                    ddlCustName.DataTextField = "CustomerName";
                    ddlCustName.DataValueField = "CustId";
                    ddlCustName.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCustName.Items.Insert(0, oli);
                }
                else
                {
                    ddlCustName.DataSource = null;
                    ddlCustName.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCA = null;
            }
        }
        private void PopReligion()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(4);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppRel.DataSource = dt;
                    ddlCoAppRel.DataTextField = "Religion";
                    ddlCoAppRel.DataValueField = "ReligionId";
                    ddlCoAppRel.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppRel.Items.Insert(0, oli1);

                    ddlCustReligion.DataSource = dt;
                    ddlCustReligion.DataTextField = "Religion";
                    ddlCustReligion.DataValueField = "ReligionId";
                    ddlCustReligion.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustReligion.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCoAppRel.DataSource = null;
                    ddlCoAppRel.DataBind();
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
        private void PopCaste()
        {
            DataTable dt = null;
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(1);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppCast.DataSource = dt;
                    ddlCoAppCast.DataTextField = "Caste";
                    ddlCoAppCast.DataValueField = "CasteId";
                    ddlCoAppCast.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppCast.Items.Insert(0, oli1);

                    ddlCustCaste.DataSource = dt;
                    ddlCustCaste.DataTextField = "Caste";
                    ddlCustCaste.DataValueField = "CasteId";
                    ddlCustCaste.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustCaste.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCoAppCast.DataSource = null;
                    ddlCoAppCast.DataBind();
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
        private void PopMaritalStatus()
        {
            DataTable dt = null;
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(3);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppMS.DataSource = dt;
                    ddlCoAppMS.DataTextField = "MaritalName";
                    ddlCoAppMS.DataValueField = "MaritalId";
                    ddlCoAppMS.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppMS.Items.Insert(0, oli1);

                    ddlCustMS.DataSource = dt;
                    ddlCustMS.DataTextField = "MaritalName";
                    ddlCustMS.DataValueField = "MaritalId";
                    ddlCustMS.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustMS.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCustMS.DataSource = null;
                    ddlCustMS.DataBind();
                    ddlCoAppMS.DataSource = null;
                    ddlCoAppMS.DataBind();
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
        private void PopGender()
        {
            DataTable dt = null;
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(2);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppGender.DataSource = dt;
                    ddlCoAppGender.DataTextField = "GenderName";
                    ddlCoAppGender.DataValueField = "GenderId";
                    ddlCoAppGender.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppGender.Items.Insert(0, oli1);

                    ddlCustGen.DataSource = dt;
                    ddlCustGen.DataTextField = "GenderName";
                    ddlCustGen.DataValueField = "GenderId";
                    ddlCustGen.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustGen.Items.Insert(0, oli2);

                    ddlNomineeGender.DataSource = dt;
                    ddlNomineeGender.DataTextField = "GenderName";
                    ddlNomineeGender.DataValueField = "GenderId";
                    ddlNomineeGender.DataBind();
                    oli2 = new ListItem("<--Select-->", "-1");
                    ddlNomineeGender.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCoAppGender.DataSource = null;
                    ddlCoAppGender.DataBind();
                    ddlCustGen.DataSource = null;
                    ddlCustGen.DataBind();
                    ddlNomineeGender.DataSource = null;
                    ddlNomineeGender.DataBind();
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

        private void PopOccupation()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(5);
                if (dt.Rows.Count > 0)
                {
                    ddlCustOccupation.DataSource = dt;
                    ddlCustOccupation.DataTextField = "Occupation";
                    ddlCustOccupation.DataValueField = "OccupationId";
                    ddlCustOccupation.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustOccupation.Items.Insert(0, oli2);

                    ddlCustOccupation2.DataSource = dt;
                    ddlCustOccupation2.DataTextField = "Occupation";
                    ddlCustOccupation2.DataValueField = "OccupationId";
                    ddlCustOccupation2.DataBind();
                    ListItem oli3 = new ListItem("<--Select-->", "-1");
                    ddlCustOccupation2.Items.Insert(0, oli3);

                    ddlCustOccupation3.DataSource = dt;
                    ddlCustOccupation3.DataTextField = "Occupation";
                    ddlCustOccupation3.DataValueField = "OccupationId";
                    ddlCustOccupation3.DataBind();
                    ListItem oli4 = new ListItem("<--Select-->", "-1");
                    ddlCustOccupation3.Items.Insert(0, oli4);

                    ddlCoAppOccu.DataSource = dt;
                    ddlCoAppOccu.DataTextField = "Occupation";
                    ddlCoAppOccu.DataValueField = "OccupationId";
                    ddlCoAppOccu.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu.Items.Insert(0, oli1);

                    ddlCoAppOccu2.DataSource = dt;
                    ddlCoAppOccu2.DataTextField = "Occupation";
                    ddlCoAppOccu2.DataValueField = "OccupationId";
                    ddlCoAppOccu2.DataBind();
                    ListItem oli5 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu2.Items.Insert(0, oli5);

                    ddlCoAppOccu3.DataSource = dt;
                    ddlCoAppOccu3.DataTextField = "Occupation";
                    ddlCoAppOccu3.DataValueField = "OccupationId";
                    ddlCoAppOccu3.DataBind();
                    ListItem oli6 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu3.Items.Insert(0, oli6);
                }
                else
                {
                    ddlCustOccupation.DataSource = null;
                    ddlCustOccupation.DataBind();
                    ddlCustOccupation2.DataSource = null;
                    ddlCustOccupation2.DataBind();
                    ddlCustOccupation3.DataSource = null;
                    ddlCustOccupation3.DataBind();
                    ddlCoAppOccu.DataSource = null;
                    ddlCoAppOccu.DataBind();
                    ddlCoAppOccu2.DataSource = null;
                    ddlCoAppOccu2.DataBind();
                    ddlCoAppOccu3.DataSource = null;
                    ddlCoAppOccu3.DataBind();
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
        private void PopQualification()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetGenderAndCastAndMaritalAndReligion(6);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppQuali.DataSource = dt;
                    ddlCoAppQuali.DataTextField = "Qualification";
                    ddlCoAppQuali.DataValueField = "QualificationId";
                    ddlCoAppQuali.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppQuali.Items.Insert(0, oli1);
                }
                else
                {
                    ddlCoAppQuali.DataSource = null;
                    ddlCoAppQuali.DataBind();
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
        private void PopBusinessType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetBusinessType();
                if (dt.Rows.Count > 0)
                {
                    ddlCustBusType.DataSource = dt;
                    ddlCustBusType.DataTextField = "BusinessTypeName";
                    ddlCustBusType.DataValueField = "BusinessTypeId";
                    ddlCustBusType.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCustBusType.Items.Insert(0, oli1);
                }
                else
                {
                    ddlCustBusType.DataSource = null;
                    ddlCustBusType.DataBind();
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
        private void PopApplicant(string pBranchCode)
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetApplicantList(pBranchCode);
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
        private void PopRelation()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetRelationList();
                if (dt.Rows.Count > 0)
                {
                    ddlRelWApp.DataSource = dt;
                    ddlRelWApp.DataTextField = "Relation";
                    ddlRelWApp.DataValueField = "RelationId";
                    ddlRelWApp.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlRelWApp.Items.Insert(0, oli);

                    ddlCustCARel.DataSource = dt;
                    ddlCustCARel.DataTextField = "Relation";
                    ddlCustCARel.DataValueField = "RelationId";
                    ddlCustCARel.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustCARel.Items.Insert(0, oli2);

                    ddlCustRel.DataSource = dt;
                    ddlCustRel.DataTextField = "Relation";
                    ddlCustRel.DataValueField = "RelationId";
                    ddlCustRel.DataBind();
                    ListItem oli3 = new ListItem("<--Select-->", "-1");
                    ddlCustRel.Items.Insert(0, oli3);

                    ddlCARel.DataSource = dt;
                    ddlCARel.DataTextField = "Relation";
                    ddlCARel.DataValueField = "RelationId";
                    ddlCARel.DataBind();
                    ListItem oli4 = new ListItem("<--Select-->", "-1");
                    ddlCARel.Items.Insert(0, oli4);

                    ddlNomineeRel.DataSource = dt;
                    ddlNomineeRel.DataTextField = "Relation";
                    ddlNomineeRel.DataValueField = "RelationId";
                    ddlNomineeRel.DataBind();
                    oli4 = new ListItem("<--Select-->", "-1");
                    ddlNomineeRel.Items.Insert(0, oli4);


                    foreach (DataRow dR in dt.Rows)
                    {
                        int vRelationId = Convert.ToInt32(dR["RelationId"]);
                        if (vRelationId != 2 && vRelationId != 3 && vRelationId != 8)
                        {
                            dR.Delete();
                        }
                    }
                    dt.AcceptChanges();

                    ddlMHFRelation.DataSource = dt;
                    ddlMHFRelation.DataTextField = "Relation";
                    ddlMHFRelation.DataValueField = "RelationId";
                    ddlMHFRelation.DataBind();
                    ListItem oli5 = new ListItem("<--Select-->", "-1");
                    ddlMHFRelation.Items.Insert(0, oli5);


                }
                else
                {
                    ddlRelWApp.DataSource = null;
                    ddlRelWApp.DataBind();
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
        private void PopVarifiedBy()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            dt = oMem.GetVarifiedByForRef();
            if (dt.Rows.Count > 0)
            {
                ddlVarifiedBy.DataSource = dt;
                ddlVarifiedBy.DataTextField = "UserName";
                ddlVarifiedBy.DataValueField = "UserID";
                ddlVarifiedBy.DataBind();
                ListItem oli = new ListItem("<--Select-->", "-1");
                ddlVarifiedBy.Items.Insert(0, oli);
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
        private void PopSector()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetSector();
                if (dt.Rows.Count > 0)
                {
                    ddlComSec.DataSource = dt;
                    ddlComSec.DataTextField = "BusinessTypeName";
                    ddlComSec.DataValueField = "BusinessTypeId";
                    ddlComSec.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlComSec.Items.Insert(0, oli);
                }
                else
                {
                    ddlComSec.DataSource = null;
                    ddlComSec.DataBind();
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
        private void PopSubSector(int Sectorid)
        {
            DataTable dt = null;
            CMember oCm = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                dt = oCm.GetSubSector(Sectorid);
                if (dt.Rows.Count > 0)
                {
                    ddlComSubSec.DataSource = dt;
                    ddlComSubSec.DataTextField = "BusinessSubTypeName";
                    ddlComSubSec.DataValueField = "BusinessSubTypeId";
                    ddlComSubSec.DataBind();
                    ListItem oli = new ListItem("Select", "-1");
                    ddlComSubSec.Items.Insert(0, oli);
                }
                else
                {
                    ddlComSubSec.DataSource = dt;
                    ddlComSubSec.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                oCm = null;
            }
        }
        private void PopCompanyType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyAndPropertyType(1);
                if (dt.Rows.Count > 0)
                {
                    ddlComType.DataSource = dt;
                    ddlComType.DataTextField = "CpmTypeName";
                    ddlComType.DataValueField = "ComTypeId";
                    ddlComType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlComType.Items.Insert(0, oli);
                }
                else
                {
                    ddlComType.DataSource = null;
                    ddlComType.DataBind();
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
        private void PopCoAppCompanyType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyAndPropertyType(1);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppComType.DataSource = dt;
                    ddlCoAppComType.DataTextField = "CpmTypeName";
                    ddlCoAppComType.DataValueField = "ComTypeId";
                    ddlCoAppComType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCoAppComType.Items.Insert(0, oli);
                }
                else
                {
                    ddlCoAppComType.DataSource = null;
                    ddlCoAppComType.DataBind();
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
        private void PopCompanyGL()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyGLDetails();
                if (dt.Rows.Count > 0)
                {
                    //ddlParticulars.DataSource = dt;
                    //ddlParticulars.DataTextField = "AcGenLed";
                    //ddlParticulars.DataValueField = "AcGenLedId";
                    //ddlParticulars.DataBind();
                    //ListItem oli = new ListItem("<--Select-->", "-1");
                    //ddlParticulars.Items.Insert(0, oli);

                    //ddlParticularsPL.DataSource = dt;
                    //ddlParticularsPL.DataTextField = "AcGenLed";
                    //ddlParticularsPL.DataValueField = "AcGenLedId";
                    //ddlParticularsPL.DataBind();
                    //ListItem oli1 = new ListItem("<--Select-->", "-1");
                    //ddlParticularsPL.Items.Insert(0, oli1);
                }
            }
            finally
            {
                dt = null;
                oMem = null;
            }
        }
        //private void PopCompPLLed()
        //{
        //    CMember oMem = new CMember();
        //    DataTable dt = null;
        //    try
        //    {
        //        dt = oMem.GetCompPLLedDetails();
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlParticularsPL.DataSource = dt;
        //            ddlParticularsPL.DataTextField = "PLGenLed";
        //            ddlParticularsPL.DataValueField = "PLGenLedId";
        //            ddlParticularsPL.DataBind();
        //            ListItem oli1 = new ListItem("<--Select-->", "-1");
        //            ddlParticularsPL.Items.Insert(0, oli1);
        //        }
        //    }
        //    finally
        //    {
        //        dt = null;
        //        oMem = null;
        //    }
        //}
        private void PopPropertyType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyAndPropertyType(2);
                if (dt.Rows.Count > 0)
                {
                    ddlComProType.DataSource = dt;
                    ddlComProType.DataTextField = "PropertypeName";
                    ddlComProType.DataValueField = "PropertyTypeID";
                    ddlComProType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlComProType.Items.Insert(0, oli);
                }
                else
                {
                    ddlComProType.DataSource = null;
                    ddlComProType.DataBind();
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
        private void PopCoAppPropertyType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetCompanyAndPropertyType(2);
                if (dt.Rows.Count > 0)
                {
                    ddlCoAppProType.DataSource = dt;
                    ddlCoAppProType.DataTextField = "PropertypeName";
                    ddlCoAppProType.DataValueField = "PropertyTypeID";
                    ddlCoAppProType.DataBind();
                    ListItem oli = new ListItem("<--Select-->", "-1");
                    ddlCoAppProType.Items.Insert(0, oli);
                }
                else
                {
                    ddlCoAppProType.DataSource = null;
                    ddlCoAppProType.DataBind();
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
        private Boolean ValidateCompanyFields()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CApplication oCG = oCG = new CApplication();
            DateTime vAppDate = vLoginDt;

            if (DOFCom.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Company Formation Date can not be black...");
                vResult = false;
            }
            else
            {
                vAppDate = gblFuction.setDate(DOFCom.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    gblFuction.MsgPopup("Company Formation Date should be Financial Year.");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    gblFuction.MsgPopup("Company Formation Date should not be greater than login date.");
                    vResult = false;
                }
            }
            if (txtComName.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Company Name can not be black...");
                vResult = false;
            }
            if (txtComPanNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Company PAN No can not be black...");
                vResult = false;
            }
            if (Request[ddlComAddTypeID.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Address Proof Id..");
                vResult = false;
            }
            if (txtComAddIDNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Address Proof Id No..");
                vResult = false;
            }

            if (Request[ddlComType.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Company Type..");
                vResult = false;
            }
            if (Request[ddlComProType.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Property Type..");
                vResult = false;
            }
            if (Request[ddlComSec.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Sector..");
                vResult = false;
            }
            if (Request[ddlComSubSec.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Sub Sector..");
                vResult = false;
            }
            if (chkComRegCer.Checked == true && txtComRegisNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Company Registration No..");
                vResult = false;
            }
            if (txtMAdd1.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Mailing Address 1..");
                vResult = false;
            }
            if (ddlMState.SelectedValue.ToString().Trim() == "-1")
            {
                gblFuction.MsgPopup("Please Select Present  State..");
                vResult = false;
            }
            if (txtMDist.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Mailing District..");
                vResult = false;
            }
            if (txtMPIN.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Mailing PIN No..");
                vResult = false;
            }
            if (txtMMobNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Mailing Mob No..");
                vResult = false;
            }
            if (txtROffAdd1.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Office Address 1..");
                vResult = false;
            }
            if (ddlRState.SelectedValue.ToString().Trim() == "-1")
            {
                gblFuction.MsgPopup("Please Select Permanent  State..");
                vResult = false;
            }
            //if (txtRState.Text.Trim() == "")
            //{
            //    gblFuction.MsgPopup("Please input Office State..");
            //    vResult = false;
            //}
            if (txtRDist.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Office District..");
                vResult = false;
            }
            if (txtRPIN.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Office PIN No..");
                vResult = false;
            }
            if (txtRMobNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Office Mob No..");
                vResult = false;
            }

            return vResult;
        }
        private Boolean ValidateCoAppField()
        {
            Boolean vResult = true;
            DateTime vFinFrom = gblFuction.setDate(Session[gblValue.FinFromDt].ToString());
            DateTime vFinTo = gblFuction.setDate(Session[gblValue.FinToDt].ToString());
            DateTime vLoginDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            CApplication oCG = oCG = new CApplication();
            DateTime vAppDate = vLoginDt;

            if (txtCoAppDOA.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Co Applicant Admission Date can not be black...");
                vResult = false;
            }
            else
            {
                vAppDate = gblFuction.setDate(txtCoAppDOA.Text);
                if (vAppDate < vFinFrom || vAppDate > vFinTo)
                {
                    gblFuction.MsgPopup("Co Applicant Admission Date should be Financial Year.");
                    vResult = false;
                }
                if (vAppDate > vLoginDt)
                {
                    gblFuction.MsgPopup("Co Applicant Admission Date should not be greater than login date.");
                    vResult = false;
                }
            }

            //if (Request[ddlCoAppPhotoIdType.UniqueID] as string == "-1")
            //{
            //    gblFuction.MsgPopup("Please select Photo Proof Id..");
            //    vResult = false;
            //}

            //if (txtCoAppPNo.Text.Trim() == "")
            //{
            //    gblFuction.MsgPopup("Please input Photo Proof Id No..");
            //    vResult = false;
            //}


            if (Request[ddlCoAppAddressIdType.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Address Proof Id..");
                vResult = false;
            }
            if (txtCoAppAddressIdNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Address Proof Id No..");
                vResult = false;
            }


            if (Request[ddlCoAppRel.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Religion..");
                vResult = false;
            }
            if (Request[ddlCoAppGender.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Gender..");
                vResult = false;
            }
            if (txtCoAppDOB.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Co Applicant DOB can not be blank..");
                vResult = false;
            }
            if (txtCoAppMNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Co Applicant Mob No..");
                vResult = false;
            }
            //if (Request[ddlCoAppDirePart.UniqueID] as string == "-1")
            //{
            //    gblFuction.MsgPopup("Please select Director/Partner field..");
            //    vResult = false;
            //}


            if (txtCoAppAddress1Pre.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Present Address 1..");
                vResult = false;
            }
            if (ddlCoAppStatePre.Text.Trim() == "-1")
            {
                gblFuction.MsgPopup("Please input Present State..");
                vResult = false;
            }
            if (txtCoAppDistPre.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Present District..");
                vResult = false;
            }
            if (txtCOAppPINPre.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Present PIN No..");
                vResult = false;
            }

            if (txtCoAppAddress1Per.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Permanent Address 1..");
                vResult = false;
            }
            if (ddlCoAppStatePer.SelectedValue.Trim() == "-1")
            {
                gblFuction.MsgPopup("Please input Permanent State..");
                vResult = false;
            }
            if (txtCoAppDistPer.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Permanent District..");
                vResult = false;
            }
            if (txtCoAppPINPer.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Permanent PIN No..");
                vResult = false;
            }


            return vResult;
        }
        protected void ClearAppControlForShow()
        {
            txtCustTypeShow.Text = "";
            txtDOFShow.Text = "";
            txtAppNoShow.Text = "";
            txtComNameShow.Text = "";
            lblComType.Text = "";
            txtAppAddresIdTypeShow.Text = "";
            txtAppAddIdNoForShow.Text = "";
            txtPropertyTypeShow.Text = "";
            lblOtherPropType.Text = "";
            lblWebsite.Text = "";
            txtEmailShow.Text = "";
            txtPANShow.Text = "";
            chkIsReg.Checked = false;
            lblRegistrationNo.Text = "";
            lblSector.Text = "";
            lblSubSector.Text = "";
            txtAppnameShow.Text = "";
            txtAppContNoShow.Text = "";
            txtMAddress1Show.Text = "";
            txtMAddress2Show.Text = "";
            txtMDistrictShow.Text = "";
            txtMMobNoShow.Text = "";
            txtMPINShow.Text = "";
            txtMSTDShow.Text = "";
            txtMTelShow.Text = "";
            txtRAddress1Show.Text = "";
            txtRAddress2Show.Text = "";
            txtRStateShow.Text = "";
            txtRDistrictShow.Text = "";
            txtRMobNoShow.Text = "";
            txtRPINShow.Text = "";
            txtRSTDShow.Text = "";
            txtRTelNoShow.Text = "";
        }
        private void ClearCoApplicant()
        {
            ddlCustName.SelectedIndex = -1;
            txtCoAppDOA.Text = Session[gblValue.LoginDate].ToString();
            // ddlCoAppCRO.SelectedIndex = -1;

            txtCoAppName.Text = "";
            txtCoAppNo.Text = "";
            ddlCoAppPhotoIdType.SelectedIndex = -1;
            txtCoAppPNo.Text = "";
            ddlCoAppAddressIdType.SelectedIndex = -1;
            txtCoAppAddressIdNo.Text = "";

            txtCoAppTelNo.Text = "";
            ddlCoAppMS.SelectedIndex = -1;
            txtCoAppMNo.Text = "";
            ddlCoAppOccu.SelectedIndex = -1;
            ddlCoAppOccu2.SelectedIndex = -1;
            ddlCoAppOccu3.SelectedIndex = -1;

            txtCoAppFaxNo.Text = "";
            txtCoAppEmail.Text = "";
            ddlCoAppGender.SelectedIndex = -1;
            txtCoAppAge.Text = "";
            txtCoAppDOB.Text = "";
            ddlCoAppQuali.SelectedIndex = -1;
            ddlCoAppRel.SelectedIndex = -1;
            ddlCoAppCast.SelectedIndex = -1;
            txtCoAppYratRes.Text = "";
            txtCoAppYrinBusiness.Text = "";
            ddlCustCARel.SelectedIndex = -1;
            // chCoApp.Checked = false;
            txtCoAppAddress1Pre.Text = "";
            txtCoAppAddress2Pre.Text = "";
            txtCoAppDistPre.Text = "";
            txtCOAppPINPre.Text = "";
            txtCoAppAddress1Per.Text = "";
            txtCoAppAddress2Per.Text = "";
            // txtCoAppStatePer.Text = "";
            txtCoAppDistPer.Text = "";
            txtCoAppPINPer.Text = "";
            chkCoAppSameAdd.Checked = false;
            txtShareHold.Text = "";
            chkIsActive.Checked = true;
            ddlCoAppAddressIdType.SelectedIndex = -1;
            ddlCoAppPhotoIdType.SelectedIndex = -1;
            txtCoAppAddressIdNo.Text = "";
            txtCoAppPNo.Text = "";
            txtCAACHoldname.Text = "";
            txtCABankName.Text = "";
            txtCAAccNo.Text = "";
            txtCAIFSCCode.Text = "";
            txtCAAcYrOfOpen.Text = "";
            ddlCAACType.SelectedIndex = -1;
            txtCAOrgname.Text = "";
            txtCADesig.Text = "";
            txtCARetiredAge.Text = "";
            txtCADeptEmpCode.Text = "";
            txtCACurExp.Text = "";
            txtCATotExp.Text = "";
            txtCABusLandmark.Text = "";
            txtCABusAdd1.Text = "";


            txtCABusAdd2.Text = "";
            txtCABusLocality.Text = "";
            txtCABusCity.Text = "";
            txtCABusPin.Text = "";
            txtCABusState.Text = "";
            txtCABusMob.Text = "";
            txtCABusPh.Text = "";
            gvCADep.DataSource = null;
            gvCADep.DataBind();
        }
        private void ClearCompany()
        {
            rbComType.SelectedValue = "I";
            DOFCom.Text = "";
            txtComName.Text = "";
            ddlComType.SelectedIndex = -1;
            ddlComProType.SelectedIndex = -1;
            txtOtherProperDtl.Text = "";
            ddlComSec.SelectedIndex = -1;
            ddlComSubSec.SelectedIndex = -1;
            txtComEmail.Text = "";
            txtComPanNo.Text = "";
            ddlComAddTypeID.SelectedIndex = -1;
            txtComAddIDNo.Text = "";
            chkComRegCer.Checked = false;
            txtComRegisNo.Text = "";
            chkComSameAdd.Checked = false;
            txtMAdd1.Text = "";
            txtMAdd2.Text = "";
            txtMDist.Text = "";
            txtMPIN.Text = "";
            txtMMobNo.Text = "";
            txtMSTD.Text = "";
            txtMTel.Text = "";
            txtROffAdd1.Text = "";
            txtROffAdd2.Text = "";
            //txtRState.Text = "";
            txtRDist.Text = "";
            txtRPIN.Text = "";
            txtRMobNo.Text = "";
            txtRSTD.Text = "";
            txtRTel.Text = "";


            txtCustRelName.Text = "";
            ddlCustRel.SelectedIndex = -1;
            ddlCustGen.SelectedIndex = -1;
            txtCustDOB.Text = "";
            txtCustAge.Text = "";
            ddlCustGen.SelectedIndex = -1;
            ddlCustMS.SelectedIndex = -1;
            ddlCustResdStatus.SelectedValue = "N";
            txtCustContNo.Text = "";
            ddlCustQual.SelectedIndex = -1;
            ddlCustReligion.SelectedIndex = -1;
            txtCustAnnualInc.Text = "";
            ddlCustCaste.SelectedIndex = -1;
            txtCustRelName.Text = "";
            ddlCustRel.SelectedIndex = -1;
            txtYrInCurRes.Text = "";
            ddlCustPhyChallenge.SelectedValue = "N";
            ddlCustOccupation.SelectedIndex = -1;
            ddlCustOccupation2.SelectedIndex = -1;
            ddlCustOccupation3.SelectedIndex = -1;

            ddlCustBusType.SelectedIndex = -1;
            ddlCustCommAddress.SelectedValue = "C";
            ddlComAddTypeID.SelectedIndex = -1;
            txtComAddIDNo.Text = "";
            ddlCustPhotoTypeId.SelectedIndex = -1;
            txtCustPhotIDNo.Text = "";
            txtCustACHoldname.Text = "";
            txtCustBankName.Text = "";
            txtCustAccNo.Text = "";
            txtCustIFSCCode.Text = "";
            hdnCustIFSCCode.Value = "";
            txtCustAcYrOfOpen.Text = "";
            ddlCustACType.SelectedIndex = -1;
            txtCustOrgname.Text = "";
            txtCustDesig.Text = "";
            txtCustRetiredAge.Text = "";
            txtCustDeptEmpCode.Text = "";
            txtCustCurExp.Text = "";
            txtCustTotExp.Text = "";
            txtCustGSTNo.Text = "";
            txtCustBusLandmark.Text = "";
            txtCustBusAdd1.Text = "";
            txtCustBusAdd2.Text = "";
            txtCustBusLocality.Text = "";
            txtCustBusCity.Text = "";
            txtCustBusPin.Text = "";
            txtCustBusState.Text = "";
            txtCustBusMob.Text = "";
            txtCustBusPh.Text = "";
            gvCustDep.DataSource = null;
            gvCustDep.DataBind();

            txtNomineeAddress.Text = "";
            txtNomineeDOB.Text = "";
            txtNomineeName.Text = "";
            txtNomineePin.Text = "";
            ddlNomineeState.SelectedIndex = -1;
            ddlNomineeRel.SelectedIndex = -1;
            ddlNomineeGender.SelectedIndex = -1;
        }
        //private void ClearControls()
        //{
        //    txtAdmDtApp.Text = "";
        //    ddlCoApp.SelectedIndex = -1;
        //    ddlSalApp.SelectedIndex = -1;
        //    txtFNameApp.Text = "";
        //    txtMNameApp.Text = "";
        //    txtLNameApp.Text = "";
        //    txtCustNameApp.Text = "";
        //    txtCustNoApp.Text = "";
        //    txtCustNoApp.Text = "";
        //    ddlPhotoIdTypApp.SelectedIndex = -1;
        //    txtPhotoNoApp.Text = "";
        //    ddlAdrIdTypeApp.SelectedIndex = -1;
        //    txtAdrNoApp.Text = "";
        //    ddlITTypeApp.SelectedIndex = -1;
        //    txtITProNoApp.Text = "";
        //    txtTelNoApp.Text = "";
        //    ddlMSApp.SelectedIndex = -1;
        //    txtMobileNoApp.Text = "";
        //    ddlOccuApp.SelectedIndex = -1;
        //    txtFaxNoApp.Text = "";
        //    txtEmailApp.Text = "";
        //    ddlGenderApp.SelectedIndex = -1;
        //    txtAgeApp.Text = "0";
        //    txtDOBApp.Text = "";
        //    ddlOccuApp.SelectedIndex = -1;
        //    ddlQualApp.SelectedIndex = -1;
        //    ddlRelgApp.SelectedIndex = -1;
        //    txtYratResApp.Text = "";
        //    txtYrinBusinessApp.Text = "";
        //    rdbApp.SelectedIndex = -1;
        //    chkGPOAHoldApp.Checked = false;
        //    ddlCastApp.SelectedIndex = -1;
        //    ddlITTypeApp.SelectedIndex = -1;
        //    txtAddress1PreApp.Text = "";
        //    txtAddress2PreApp.Text = "";
        //    txtStatePreApp.Text = "";
        //    txtDistPreApp.Text = "";
        //    txtPinPreApp.Text = "";
        //    txtAddress1PerApp.Text = "";
        //    txtAddress2PerApp.Text = "";
        //    txtStatePerApp.Text = "";
        //    txtDistPerApp.Text = "";
        //    txtPINPerApp.Text = "";

        //    lblDate.Text = "";
        //    lblUser.Text = "";


        //}
        private void ClearApplication()
        {
            txtAppNo.Text = "";
            txtAppDt.Text = "";
            ddlLoanApplicantname.SelectedIndex = -1;
            ddlLnPurpose.SelectedIndex = -1;
            ddlLnScheme.SelectedIndex = -1;
            ddlSourceName.SelectedIndex = -1;
            txtAppLnAmt.Text = "";
            ddlTenure.SelectedIndex = -1;
            txtLnPurposeDetails.Text = "";
            txtAddTerms.Text = "";
        }
        private void ClearRefControl()
        {
            txtRefName.Text = "";
            ddlRelWApp.SelectedIndex = -1;
            txtRefAddr.Text = "";
            txtRefMNo.Text = "";
            txtOffTelNoRef.Text = "";
            rblRefType.SelectedIndex = -1;
            txtOffTelNoRef.Text = "";
            txtCompNameRef.Text = "";
            ddlStatus.SelectedValue = "POSITIVE";
            ddlVarifiedBy.SelectedIndex = -1;
        }
        protected void ddlLnScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLnScheme.SelectedValue != "-1")
            {
                int LnType = Convert.ToInt32(ddlLnScheme.SelectedValue);
                EnableMachinDtl(LnType);
            }
        }
        private void GetCoAppAddress(string pCustId)
        {
            DataTable dt = new DataTable();
            CMember Omem = new CMember();
            try
            {
                dt = Omem.GetCoAppAddress(pCustId);
                if (dt.Rows.Count > 0)
                {
                    txtCoAppAddress1Pre.Text = dt.Rows[0]["MAddress1"].ToString();
                    txtCoAppAddress2Pre.Text = dt.Rows[0]["MAddress2"].ToString();
                    ddlCoAppStatePre.SelectedIndex = ddlCoAppStatePre.Items.IndexOf(ddlCoAppStatePre.Items.FindByValue(Convert.ToString(dt.Rows[0]["MState"])));

                    //  txtCoAppStatePre.Text = dt.Rows[0]["MState"].ToString();
                    txtCoAppDistPre.Text = dt.Rows[0]["MDistrict"].ToString();
                    txtCOAppPINPre.Text = dt.Rows[0]["MPIN"].ToString();

                    txtCoAppAddress1Per.Text = dt.Rows[0]["RAddress1"].ToString();
                    txtCoAppAddress2Per.Text = dt.Rows[0]["RAddress2"].ToString();
                    ddlCoAppStatePer.SelectedIndex = ddlCoAppStatePer.Items.IndexOf(ddlCoAppStatePer.Items.FindByValue(Convert.ToString(dt.Rows[0]["RState"])));
                    // txtCoAppStatePer.Text = dt.Rows[0]["RState"].ToString();
                    txtCoAppDistPer.Text = dt.Rows[0]["RDistrict"].ToString();
                    txtCoAppPINPer.Text = dt.Rows[0]["RPIN"].ToString();
                }
                else
                {
                    txtCoAppAddress1Pre.Text = "";
                    txtCoAppAddress2Pre.Text = "";
                    txtCoAppDistPre.Text = "";
                    txtCOAppPINPre.Text = "";
                    txtCoAppAddress1Per.Text = "";
                    txtCoAppAddress2Per.Text = "";
                    txtCoAppDistPer.Text = "";
                    txtCoAppPINPer.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                Omem = null;
            }
        }
        protected void chkCoAppSameAdd_CheckedChanged(Object sender, EventArgs args)
        {
            try
            {
                if (chkCoAppSameAdd.Checked == true)
                {
                    string pCustid = Convert.ToString((Request[ddlCustName.UniqueID] as string == null) ? ddlCustName.SelectedValue : Request[ddlCustName.UniqueID] as string);
                    GetCoAppAddress(pCustid);
                }
                else
                {
                    txtCoAppAddress1Pre.Text = "";
                    txtCoAppAddress2Pre.Text = "";
                    //txtCoAppStatePre.Text = "";
                    txtCoAppDistPre.Text = "";
                    txtCOAppPINPre.Text = "";
                    txtCoAppAddress1Per.Text = "";
                    txtCoAppAddress2Per.Text = "";
                    txtCoAppDistPer.Text = "";
                    txtCoAppPINPer.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GetFileType(string pFileTyp)
        {
            string vRst = "";
            switch (pFileTyp)
            {
                case ".txt":
                    vRst = "application/notepad";
                    break;
                case ".doc":
                    vRst = "application/ms-word";
                    break;
                case ".docx":
                    vRst = "application/vnd.ms-word.document.12";
                    break;
                case ".xls":
                    vRst = "application/vnd.ms-excel";
                    break;
                case ".xlsx":
                    vRst = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".pdf":
                    vRst = "application/vnd.pdf";
                    break;
                case ".zip":
                    vRst = "application/zip";
                    break;
                case ".rar":
                    vRst = "application/WinRar";
                    break;
                case ".exe":
                    vRst = "application/executable";
                    break;
                default:
                    vRst = "";
                    break;
            }
            return vRst;
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

        //protected void imgbtAddressProof_Click(object sender, EventArgs e)
        //{

        //        pathImage = "E:\\WebApps\\PratamMobService\\CustomerKYCImage\\";
        //        string imgFolder = txtCustNoApp.Text.ToString();
        //        if (imgFolder == "")
        //        {
        //            gblFuction.AjxMsgPopup("No Image Found..!!");
        //            return;
        //        }
        //        // For Front Section
        //        string ImgPath = pathImage + imgFolder + "\\AddressImageFront.png";

        //        if (!Directory.Exists(pathImage + imgFolder))
        //        {
        //            gblFuction.AjxMsgPopup("No Image Found..!!");
        //            return;
        //        }
        //        Response.ContentType = ContentType;
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(ImgPath));
        //        Response.WriteFile(ImgPath);
        //        Response.End();

        //        // For Back Section
        //        ImgPath = "";
        //        ImgPath = pathImage + imgFolder + "\\AddressImageBack.png";

        //        if (!Directory.Exists(pathImage + imgFolder))
        //        {
        //            gblFuction.AjxMsgPopup("No Image Found..!!");
        //            return;
        //        }
        //        Response.ContentType = ContentType;
        //        Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(ImgPath));
        //        Response.WriteFile(ImgPath);
        //        Response.End();

        //}

        private string SaveMemberImages(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath)
        {
            try
            {
                string folderPath = string.Format("{0}/{1}", ImagePath, imageGroup/*, folderName*/);
                System.IO.Directory.CreateDirectory(folderPath);
                string filePath = string.Format("{0}/{1}.png", folderPath, imageName);

                if ((Mode == "Delete"))
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    isImageSaved = "N";
                }
                else
                {
                    if (flup.HasFile)
                    {
                        if (Mode == "Edit")
                        {
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }

                        File.WriteAllBytes(filePath, Convert.FromBase64String(getBase64String(flup)));
                        isImageSaved = "Y";
                    }
                }
            }
            catch (Exception ex)
            {
                //DBUtility.WriteErrorLog(ex);
                //isImageSaved = "N";
                throw ex;
            }

            return isImageSaved;
        }
        private string getBase64String(FileUpload flup)
        {
            string base64String = "";

            try
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(flup.PostedFile.InputStream))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();
                        base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                //DBUtility.WriteErrorLog(ex);
            }

            return base64String;

        }
        //private void clearMemPhoto()
        //{
        //    string imgUrl = "~/Images/no-image-icon.jpg";
        //    imgMemPhoto.ImageUrl = imgUrl;
        //    imgMemIdProof.ImageUrl = imgUrl;
        //    imgMemIdProofBack.ImageUrl = imgUrl;
        //    imgMemAddrProof.ImageUrl = imgUrl;
        //    imgMemAddrProofBack.ImageUrl = imgUrl;
        //    imgMemPassbook.ImageUrl = imgUrl;
        //}
        //private void memberKYC(string HighMarkId)
        //{
        //    pathImage = ConfigurationManager.AppSettings["PathImage"];
        //    string imgFolder = HighMarkId;
        //    imgMemPhoto.ImageUrl = pathImage + imgFolder + "/MemberPhoto.png";
        //    imgMemIdProof.ImageUrl = pathImage + imgFolder + "/PrimaryImageFront.png";
        //    imgMemIdProofBack.ImageUrl = pathImage + imgFolder + "/PrimaryImageBack.png";
        //    imgMemAddrProof.ImageUrl = pathImage + imgFolder + "/AddressImageFront.png";
        //    imgMemAddrProofBack.ImageUrl = pathImage + imgFolder + "/AddressImageBack.png";
        //    imgMemPassbook.ImageUrl = pathImage + imgFolder + "/MemberPassBook.png";
        //}


        protected void btnVerifyCust_Click(object sender, EventArgs e)
        {
            if (txtAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Please save the application first.");
                return;
            }
            if (hdnQaYN.Value == "N")
            {
                gblFuction.AjxMsgPopup("Before Proxie this Operation is not possible.");
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup("Not Avilable.");
                return;
            }
        }

        protected void btnVerifyEquifax_Click(object sender, EventArgs e)
        {
            if (txtAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Please save the application first.");
                return;
            }
            if (hdnQaYN.Value == "N")
            {
                gblFuction.AjxMsgPopup("Before Proxie this Operation is not possible.");
                return;
            }
            else
            {
                CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
                CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

                PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
                PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

                LinkButton btnEqVerify = (LinkButton)sender;
                GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
                Label lblCustIDId = (Label)gR.FindControl("lblCustIDId");
                Label lblEquFaxScore = (Label)gR.FindControl("lblEquFaxScore");
                LinkButton btnVerifyEquifax = (LinkButton)gR.FindControl("btnVerifyEquifax");
                DataTable dt = null;
                CApplication oCAp = null;
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                int vErr = 0;
                string vCustId = lblCustIDId.Text.Trim();
                string vCBID = gR.Cells[8].Text.Trim();
                string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
                string pErrorMsg = "";
                int pStatus = 0;
                string pStatusDesc = "";
                // string pCbType = Convert.ToString(Session[gblValue.CbType]);
                oCAp = new CApplication();
                CEquiFaxDataSubmission oEqui = new CEquiFaxDataSubmission();

                dt = oCAp.GetMemberInfo(vCustId, "E", "A");
                if (dt.Rows.Count > 0)
                {
                    string pEqXml = "";
                    //string pEqXml = "<root><InquiryResponseHeader><ClientID>028FZ00016</ClientID><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>CCR</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>00</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>H</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><IDDetails><seq>2</seq><IDType>O</IDType><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CCRResponse><Status>1</Status><CommercialBureauResponse><Status>1</Status><hit_as_borrower>00</hit_as_borrower><hit_as_guarantor>00</hit_as_guarantor><InquiryResponseHeader><ClientID>028FZ00016</ClientID><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><TranID>6430865</TranID><ProductCode>CCR</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>00</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><CommercialBureauResponseDetails><IDAndContactInfo><CommercialPersonalInfo><roc_BusinessLegalConstitution>false</roc_BusinessLegalConstitution><roc_ClassActivity>false</roc_ClassActivity></CommercialPersonalInfo><CommercialIdentityInfo><roc_CIN>false</roc_CIN></CommercialIdentityInfo></IDAndContactInfo><CommercialCIRSummary><CommercialHeaderDetails><member_name>Indiabulls Housing Finance Ltd.</member_name></CommercialHeaderDetails><SeverityGrid><SeverityGridDetailsMap><_x0032_018-2019 /><_x0032_019-2020 /><_x0032_020-2021 /></SeverityGridDetailsMap></SeverityGrid><EquifaxScoresCommercial /></CommercialCIRSummary><EnquirySummary /></CommercialBureauResponseDetails></CommercialBureauResponse><CIRReportDataLst><InquiryResponseHeader><CustomerCode>IC01</CustomerCode><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>PCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time><HitCode>10</HitCode><CustomerName>IC01</CustomerName></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>Other</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CIRReportData><IDAndContactInfo><PersonalInfo><Name><FullName>PARAMJIT SINGH</FullName></Name><_x0020_AliasName /><DateOfBirth>1975-01-01</DateOfBirth><Gender>Female</Gender><Age><Age>45</Age></Age><PlaceOfBirthInfo /><TotalIncome>50001</TotalIncome></PersonalInfo><IdentityInfo><PANId><seq>1</seq><IdNumber>AJJPS0032N</IdNumber></PANId><VoterID><seq>1</seq><IdNumber>JJG1748623</IdNumber></VoterID><NationalIDCard><seq>1</seq><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><NationalIDCard><seq>2</seq><ReportedDate>2017-10-31</ReportedDate><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard></IdentityInfo><AddressInfo><Seq>1</Seq><ReportedDate>2017-10-31</ReportedDate><Address>W/O PALANISAMY M 9843592974 8/840 A KARUPPARAN NAGAR MUMMOORTHI NAGAR POOLUVAPATTI TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><AddressInfo><Seq>2</Seq><ReportedDate>2012-08-31</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEUMMOORTHI NAGAR POOLUVAPATTI TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><AddressInfo><Seq>3</Seq><ReportedDate>2011-08-31</ReportedDate><Address>W/O PALANISAMY M 9843592974 8/840 A KARUPPARAN NAGAR TIRUPUR</Address><State>TN</State><Postal>641602</Postal></AddressInfo><PhoneInfo><seq>1</seq><typeCode>M</typeCode><ReportedDate>2012-08-31</ReportedDate><Number>9463348097</Number></PhoneInfo><PhoneInfo><seq>2</seq><typeCode>M</typeCode><ReportedDate>2017-10-31</ReportedDate><Number>919843592974</Number></PhoneInfo></IDAndContactInfo><RetailAccountDetails><seq>1</seq><AccountNumber>1363842036193</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>73758</Balance><Open>Yes</Open><SanctionAmount>72000</SanctionAmount><DateReported>2018-01-31</DateReported><DateOpened>2017-10-25</DateOpened><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>72000</InstallmentAmount><AccountStatus>Standard</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-18</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>STD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>2</seq><AccountNumber>1363842032037</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>100000</SanctionAmount><LastPaymentDate>2017-05-18</LastPaymentDate><DateReported>2017-05-31</DateReported><DateOpened>2016-09-09</DateOpened><DateClosed>2017-05-18</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>100000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>05-17</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-16</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>3</seq><AccountNumber>1363842029451</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>73000</SanctionAmount><LastPaymentDate>2016-09-09</LastPaymentDate><DateReported>2016-09-30</DateReported><DateOpened>2016-01-20</DateOpened><DateClosed>2016-09-09</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>73000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>09-16</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-16</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>4</seq><AccountNumber>1363842019129</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>33000</SanctionAmount><LastPaymentDate>2014-01-28</LastPaymentDate><DateReported>2014-01-31</DateReported><DateOpened>2013-08-19</DateOpened><DateClosed>2014-01-28</DateClosed><Reason>Closed Account</Reason><InterestRate>9</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>33000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-14</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-13</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>5</seq><AccountNumber>1363842019353</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>35000</SanctionAmount><LastPaymentDate>2014-01-28</LastPaymentDate><DateReported>2014-01-31</DateReported><DateOpened>2013-09-10</DateOpened><DateClosed>2014-01-28</DateClosed><Reason>Closed Account</Reason><InterestRate>10.45</InterestRate><RepaymentTenure>12</RepaymentTenure><InstallmentAmount>35000</InstallmentAmount><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>01-14</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>6</seq><AccountNumber>1363842014483</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>110000</SanctionAmount><LastPaymentDate>2013-09-05</LastPaymentDate><DateReported>2013-11-30</DateReported><DateOpened>2012-09-12</DateOpened><DateClosed>2013-09-05</DateClosed><Reason>Closed Account</Reason><InterestRate>10.2</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>11-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-12</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>7</seq><AccountNumber>1363842014121</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>40000</SanctionAmount><LastPaymentDate>2013-08-19</LastPaymentDate><DateReported>2013-08-31</DateReported><DateOpened>2012-08-16</DateOpened><DateClosed>2013-08-19</DateClosed><Reason>Closed Account</Reason><InterestRate>10.95</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>08-13</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-13</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-12</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>8</seq><AccountNumber>1363842009411</AccountNumber><Institution>Canara Bank</Institution><AccountType>Business Loan - Priority Sector- Agriculture</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><Open>No</Open><SanctionAmount>50000</SanctionAmount><LastPaymentDate>2012-08-16</LastPaymentDate><DateReported>2012-08-31</DateReported><DateOpened>2011-08-19</DateOpened><DateClosed>2012-08-16</DateClosed><Reason>Closed Account</Reason><InterestRate>11.5</InterestRate><RepaymentTenure>12</RepaymentTenure><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>08-12</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-12</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-11</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-11</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountsSummary><NoOfAccounts>8</NoOfAccounts><NoOfActiveAccounts>1</NoOfActiveAccounts><NoOfWriteOffs>0</NoOfWriteOffs><TotalPastDue>0.00</TotalPastDue><SingleHighestCredit>0.00</SingleHighestCredit><SingleHighestSanctionAmount>72000.00</SingleHighestSanctionAmount><TotalHighCredit>0.00</TotalHighCredit><AverageOpenBalance>73758.00</AverageOpenBalance><SingleHighestBalance>73758.00</SingleHighestBalance><NoOfPastDueAccounts>0</NoOfPastDueAccounts><NoOfZeroBalanceAccounts>0</NoOfZeroBalanceAccounts><RecentAccount>Business Loan - Priority Sector- Agriculture on 25-10-2017</RecentAccount><OldestAccount>Business Loan - Priority Sector- Agriculture on 19-08-2011</OldestAccount><TotalBalanceAmount>73758.00</TotalBalanceAmount><TotalSanctionAmount>72000.00</TotalSanctionAmount><TotalCreditLimit>0.0</TotalCreditLimit><TotalMonthlyPaymentAmount>72000.00</TotalMonthlyPaymentAmount></RetailAccountsSummary><ScoreDetails><Type>ERS</Type><Version>3.1</Version><Name>M001</Name><Value>684</Value><ScoringElements><type>RES</type><seq>1</seq><Description>Number of commercial trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>2</seq><code>7a</code><Description>Delinquency or past due amount occurences</Description></ScoringElements><ScoringElements><type>RES</type><seq>3</seq><code>2f</code><Description>Vintage of trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>4</seq><code>8b</code><Description>Number of agri loan trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>5</seq><code>11a</code><Description>Number of or lack of agri loan trades</Description></ScoringElements></ScoreDetails><Enquiries><seq>0</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:53</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>1</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>2</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:50</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>3</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:49</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>4</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>5</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>6</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:43</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>7</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:42</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>8</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:39</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>9</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>10</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:36</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>11</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:35</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>12</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:32</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>13</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:31</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>14</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:28</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>15</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:27</Time><RequestPurpose>00</RequestPurpose></Enquiries><EnquirySummary><Purpose>ALL</Purpose><Total>318</Total><Past30Days>0</Past30Days><Past12Months>3</Past12Months><Past24Months>318</Past24Months><Recent>2020-02-06</Recent></EnquirySummary><OtherKeyInd><AgeOfOldestTrade>109</AgeOfOldestTrade><NumberOfOpenTrades>0</NumberOfOpenTrades><AllLinesEVERWritten>0.00</AllLinesEVERWritten><AllLinesEVERWrittenIn9Months>0</AllLinesEVERWrittenIn9Months><AllLinesEVERWrittenIn6Months>0</AllLinesEVERWrittenIn6Months></OtherKeyInd><RecentActivities><AccountsDeliquent>0</AccountsDeliquent><AccountsOpened>0</AccountsOpened><TotalInquiries>0</TotalInquiries><AccountsUpdated>0</AccountsUpdated></RecentActivities><DimensionalVariables><TDA_MESMI_CC_PSDAMT_24>0.0</TDA_MESMI_CC_PSDAMT_24><TDA_MESME_INS_PSDAMT_24>0.0</TDA_MESME_INS_PSDAMT_24><TDA_METSU_CC_PSDAMT_3>0.0</TDA_METSU_CC_PSDAMT_3><TDA_SUM_PF_PSDAMT_3>0.0</TDA_SUM_PF_PSDAMT_3></DimensionalVariables></CIRReportData></CIRReportDataLst><CIRReportDataLst><InquiryResponseHeader><CustomerCode>SKSM</CustomerCode><CustRefField>123456</CustRefField><ReportOrderNO>6430865</ReportOrderNO><ProductCode>MCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-09-21</Date><Time>17:15:21</Time><HitCode>10</HitCode><CustomerName>SKSM</CustomerName></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>Other</InquiryPurpose><FirstName>PARAMJIT SINGH</FirstName><InquiryAddresses><seq>1</seq><AddressType>Primary</AddressType><AddressLine1>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEPUR PUNJAB</AddressLine1><State>PB</State><Postal>152002</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9463348097</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AJJPS0032N</IDValue><Source>Inquiry</Source></IDDetails><DOB>1975-01-01</DOB><MFIDetails><FamilyDetails><seq>1</seq><AdditionalNameType>F</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CIRReportData><IDAndContactInfo><PersonalInfo><Name><FullName>PARAMJIT SINGH</FullName></Name><_x0020_AliasName /><DateOfBirth>1975-01-01</DateOfBirth><Gender>Female</Gender><Age><Age>45</Age></Age><PlaceOfBirthInfo /><Occupation>DAILY LABOURER</Occupation><MaritalStatus>Married</MaritalStatus></PersonalInfo><IdentityInfo><PANId><seq>1</seq><ReportedDate>2015-10-14</ReportedDate><IdNumber>AJJPS0032N</IdNumber></PANId><VoterID><seq>1</seq><ReportedDate>2015-05-14</ReportedDate><IdNumber>ABK0198275</IdNumber></VoterID><NationalIDCard><seq>1</seq><ReportedDate>2015-10-14</ReportedDate><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><RationCard><seq>1</seq><ReportedDate>2015-05-14</ReportedDate><IdNumber>746009953378</IdNumber></RationCard></IdentityInfo><AddressInfo><Seq>1</Seq><ReportedDate>2016-05-03</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></AddressInfo><AddressInfo><Seq>2</Seq><ReportedDate>2016-04-13</ReportedDate><Address>456 LONIYA KARBAL LONIYA KARBAL CHHINDWARA CHHINDWARA MADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></AddressInfo><PhoneInfo><seq>1</seq><typeCode>H</typeCode><ReportedDate>2016-05-03</ReportedDate><Number>9463348097</Number></PhoneInfo><PhoneInfo><seq>2</seq><typeCode>H</typeCode><ReportedDate>2016-04-13</ReportedDate><Number>8959844196</Number></PhoneInfo></IDAndContactInfo><MicrofinanceAccountDetails><branchIDMFI>MPGL0936</branchIDMFI><kendraIDMFI>411279</kendraIDMFI><seq>0</seq><id>MPGL09360014615</id><AccountNumber>MPGL09360014615</AccountNumber><CurrentBalance>0</CurrentBalance><Institution>The Ratnakar Bank Limited</Institution><InstitutionType>MFI</InstitutionType><PastDueAmount>0</PastDueAmount><DisbursedAmount>24000</DisbursedAmount><LoanCategory>JLG Individual</LoanCategory><LoanPurpose>SERICULTURE</LoanPurpose><Open>No</Open><SanctionAmount>24000</SanctionAmount><LastPaymentDate>2016-09-02</LastPaymentDate><DateReported>2018-02-28</DateReported><DateOpened>2015-04-08</DateOpened><DateClosed>2016-09-02</DateClosed><Reason>Closed Account</Reason><LoanCycleID>2</LoanCycleID><DateSanctioned>2015-04-08</DateSanctioned><DateApplied>2015-04-08</DateApplied><AppliedAmount>24000</AppliedAmount><NoOfInstallments>52</NoOfInstallments><RepaymentTenure>Bi-weekly</RepaymentTenure><InstallmentAmount>780</InstallmentAmount><KeyPerson><Name>CHHOTE MIYAN</Name><RelationType>Husband</RelationType><associationType>K</associationType></KeyPerson><Nominee><Name>CHHOTE MIYAN</Name><RelationType>Husband</RelationType><associationType>N</associationType></Nominee><AccountStatus>Closed Account</AccountStatus><DaysPastDue>0</DaysPastDue><MaxDaysPastDue>0</MaxDaysPastDue><TypeOfInsurance>L</TypeOfInsurance><NumberOfMeetingsHeld>37</NumberOfMeetingsHeld><source>INDIVIDUAL</source><AdditionalMFIDetails><MFIClientFullname>HAMEEDA VI </MFIClientFullname><MFIDOB>1969-01-01</MFIDOB><MFIGender>Female</MFIGender><MemberId>616672889</MemberId><MFIIdentification><VoterID><IdNumber>ABK0198275</IdNumber></VoterID><NationalIDCard><IdNumber>XXXXXXXXXXXX</IdNumber></NationalIDCard><RationCard><IdNumber>746009953378</IdNumber></RationCard></MFIIdentification><MFIAddress><Seq>1</Seq><ReportedDate>2016-04-13</ReportedDate><Address>456 LONIYA KARBAL LONIYA KARBAL CHHINDWARA CHHINDWARA MADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></MFIAddress><MFIPhones><seq>0</seq><ReportedDate>2016-04-13</ReportedDate><Number>8959844196</Number></MFIPhones></AdditionalMFIDetails><BranchIDMFI>MPGL0936</BranchIDMFI><KendraIDMFI>411279</KendraIDMFI><History24Months><key>02-18</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-18</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>08-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>04-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>03-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>02-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>08-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>07-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>06-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>05-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>04-16</key><PaymentStatus>000</PaymentStatus></History24Months><History24Months><key>03-16</key><PaymentStatus>000</PaymentStatus></History24Months></MicrofinanceAccountDetails><MicrofinanceAccountDetails><branchIDMFI>MPGL0936</branchIDMFI><kendraIDMFI>81699</kendraIDMFI><seq>1</seq><id>MPGL09360010000</id><AccountNumber>MPGL09360010000</AccountNumber><CurrentBalance>0</CurrentBalance><Institution>The Ratnakar Bank Limited</Institution><InstitutionType>MFI</InstitutionType><PastDueAmount>0</PastDueAmount><DisbursedAmount>12000</DisbursedAmount><LoanCategory>Individual</LoanCategory><LoanPurpose>BAKERY</LoanPurpose><Open>No</Open><SanctionAmount>12000</SanctionAmount><LastPaymentDate>2015-04-03</LastPaymentDate><DateReported>2017-01-05</DateReported><DateOpened>2014-04-17</DateOpened><DateClosed>2015-04-03</DateClosed><Reason>Closed Account</Reason><LoanCycleID>1</LoanCycleID><DateSanctioned>2014-04-17</DateSanctioned><DateApplied>2014-04-17</DateApplied><AppliedAmount>12000</AppliedAmount><NoOfInstallments>50</NoOfInstallments><RepaymentTenure>Weekly</RepaymentTenure><InstallmentAmount>285</InstallmentAmount><KeyPerson><Name>SURJIT SINGH</Name><RelationType>Father</RelationType><associationType>K</associationType></KeyPerson><Nominee><Name>SURJIT SINGH</Name><RelationType>Father</RelationType><associationType>N</associationType></Nominee><AccountStatus>Closed Account</AccountStatus><DaysPastDue>0</DaysPastDue><MaxDaysPastDue>0</MaxDaysPastDue><TypeOfInsurance>L</TypeOfInsurance><NumberOfMeetingsHeld>0</NumberOfMeetingsHeld><source>INDIVIDUAL</source><AdditionalMFIDetails><MFIClientFullname>PARAMJIT SINGH</MFIClientFullname><MFIDOB>1975-01-01</MFIDOB><MFIGender>Female</MFIGender><MemberId>616672824</MemberId><MFIIdentification><PANId><IdNumber>AJJPS0032N</IdNumber></PANId></MFIIdentification><MFIAddress><Seq>1</Seq><ReportedDate>2016-05-03</ReportedDate><Address>VILLAGE ASMAN RANDHAWA PO MASTE KE TEH AND DISTT FEROZEADHYA PRADESH</Address><State>MP</State><Postal>480001</Postal></MFIAddress><MFIPhones><seq>0</seq><ReportedDate>2016-05-03</ReportedDate><Number>9463348097</Number></MFIPhones></AdditionalMFIDetails><BranchIDMFI>MPGL0936</BranchIDMFI><KendraIDMFI>81699</KendraIDMFI><History24Months><key>01-17</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>11-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>10-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>09-16</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>08-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>04-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>03-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>02-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>01-16</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>12-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>11-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>10-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>09-15</key><PaymentStatus>*</PaymentStatus></History24Months><History24Months><key>08-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>07-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>06-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months><History24Months><key>05-15</key><PaymentStatus>CLSD</PaymentStatus></History24Months></MicrofinanceAccountDetails><MicrofinanceAccountsSummary><id>INDIVIDUAL</id><NoOfActiveAccounts>0</NoOfActiveAccounts><TotalPastDue>0.00</TotalPastDue><NoOfPastDueAccounts>0</NoOfPastDueAccounts><RecentAccount>MicroFinance Personal Loan on 08-04-2015</RecentAccount><TotalBalanceAmount>0.00</TotalBalanceAmount><TotalMonthlyPaymentAmount>0.00</TotalMonthlyPaymentAmount><TotalWrittenOffAmount>0.00</TotalWrittenOffAmount><Id>INDIVIDUAL</Id></MicrofinanceAccountsSummary><IncomeDetails><occupation>DAILY LABOURER</occupation><monthlyIncome>58000</monthlyIncome><monthlyExpense>0</monthlyExpense><seq>2</seq><reportedDate>2016-05-03</reportedDate></IncomeDetails><IncomeDetails><occupation>DAILY LABOURER</occupation><monthlyIncome>58000</monthlyIncome><monthlyExpense>0</monthlyExpense><seq>1</seq><reportedDate>2016-04-13</reportedDate></IncomeDetails><familyDetailsInfo><numberOfDependents>0</numberOfDependents><relatives><AdditionalNameType>Father</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></relatives><relatives><AdditionalNameType>Father</AdditionalNameType><AdditionalName>SURJIT SINGH</AdditionalName></relatives></familyDetailsInfo><ScoreDetails><Name>M001</Name><Value>53.0</Value></ScoreDetails><Enquiries><seq>0</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>1</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:52</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>2</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:49</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>3</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:48</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>4</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:46</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>5</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:45</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>6</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:42</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>7</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:41</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>8</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>9</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:38</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>10</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:35</Time><RequestPurpose>00</RequestPurpose></Enquiries><Enquiries><seq>11</seq><Institution>CCR MEMBER</Institution><Date>2019-08-14</Date><Time>15:34</Time><RequestPurpose>00</RequestPurpose></Enquiries><EnquirySummary /><OtherKeyInd><NumberOfOpenTrades>0</NumberOfOpenTrades></OtherKeyInd></CIRReportData></CIRReportDataLst></CCRResponse></root>";
                    try
                    {
                        //*************************** For Live ***************************************************                      
                        WebServiceSoapClient eq = new WebServiceSoapClient();

                        //************************************************GenderId 1 For MALE else Female****************************************
                        //if (dt.Rows[0]["GenderId"].ToString() == "1")
                        //{
                        //    pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                        //        , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                        //         dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                        //          dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                        //          "5750", PCSUserName, PCSPassword, "027FP27137", "9GH", " ", "PCS", "ERS", "3.1", "PRO");
                        //}
                        //else
                        //{
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                             dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                              dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                               "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                        //}

                        //*************************************************************************
                        vErr = oCAp.UpdateEquifaxInformation(txtAppNo.Text, Convert.ToInt32(vCBID), pEqXml, vBranch, "", Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                        if (vErr == 1)
                        {
                            string[] arr = pStatusDesc.Split(',');
                            string[] arr1 = arr[0].Split('=');
                            string vAcceptYN = arr1[1].ToString();
                            if (vAcceptYN == "Y")
                            {
                                btnVerifyEquifax.Text = "Verified";
                                btnVerifyEquifax.Enabled = false;
                                lblEquFaxScore.Text = arr[1].ToString();

                            }
                            else
                            {
                                btnVerifyEquifax.Text = "Cancel";
                                btnVerifyEquifax.Enabled = false;
                                lblEquFaxScore.Text = arr[1].ToString();
                            }
                            gblFuction.AjxMsgPopup(pStatusDesc);
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
                    }
                }
            }

        }

        protected void btnEquifaxVerifyCA_Click(object sender, EventArgs e)
        {
            if (txtAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Please save the application first.");
                return;
            }
            if (hdnQaYN.Value == "N")
            {
                gblFuction.AjxMsgPopup("Before Proxie this Operation is not possible.");
                return;
            }
            else
            {
                CCRUserName = ConfigurationManager.AppSettings["CCRUserName"];
                CCRPassword = ConfigurationManager.AppSettings["CCRPassword"];

                PCSUserName = ConfigurationManager.AppSettings["PCSUserName"];
                PCSPassword = ConfigurationManager.AppSettings["PCSPassword"];

                LinkButton btnEqVerify = (LinkButton)sender;
                GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
                Label lblCoApplicantId = (Label)gR.FindControl("lblCoApplicantId");
                Label lblEquCBScore = (Label)gR.FindControl("lblEquCBScore");
                Label lblReportID = (Label)gR.FindControl("lblReportID");
                LinkButton btnVerifyCA = (LinkButton)gR.FindControl("btnVerifyCA");
                LinkButton btnEquifaxVerifyCA = (LinkButton)gR.FindControl("btnEquifaxVerifyCA");
                ImageButton btnDownloadCoAppEnq = (ImageButton)gR.FindControl("btnDownloadCoAppEnq");
                string vCoApplicantId = lblCoApplicantId.Text.Trim();
                string vCBID = gR.Cells[9].Text.Trim();

                string vBrchCode = Session[gblValue.BrnchCode].ToString();
                DataTable dt = null;
                int vErr = 0;
                string pStatusDesc = "";
                string vBranch = Convert.ToString(Session[gblValue.BrnchCode]);
                string pErrorMsg = "";
                int pStatus = 0;
                DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
                CApplication oCAp = new CApplication();
                CEquiFaxDataSubmission oEqui = new CEquiFaxDataSubmission();

                dt = oCAp.GetMemberInfo(vCoApplicantId, "E", "C");
                if (dt.Rows.Count > 0)
                {
                    // string pEqXml = "<root><InquiryResponseHeader><ClientID>027FP27137</ClientID><CustRefField /><ReportOrderNO>81828531</ReportOrderNO><ProductCode>PCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-10-01</Date><Time>18:32:47</Time></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>51</InquiryPurpose><FirstName>RAVIKUMAR LOCHANRAM KANOJIYA</FirstName><InquiryAddresses><seq>1</seq><AddressType>H</AddressType><AddressLine1>ROOM NO -505 NALANDA APT BWING ALIYAWAR JUNG MARG NEAR BCC C,MUMBAI,Maharashtra</AddressLine1><State>MH</State><Postal>400051</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9619592846</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AXRPK6037F</IDValue><Source>Inquiry</Source></IDDetails><IDDetails><seq>2</seq><IDType>M</IDType><IDValue>620586164577</IDValue><Source>Inquiry</Source></IDDetails><DOB>1968-09-01</DOB><MFIDetails><FamilyDetails><seq>1</seq></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CCRResponse><Status>1</Status><CIRReportDataLst><InquiryResponseHeader><CustomerCode>WPDK</CustomerCode><CustRefField /><ReportOrderNO>81828531</ReportOrderNO><ProductCode>PCS</ProductCode><SuccessCode>1</SuccessCode><Date>2020-10-01</Date><Time>18:32:47</Time><HitCode>10</HitCode><CustomerName>WPDK</CustomerName></InquiryResponseHeader><InquiryRequestInfo><InquiryPurpose>Business Loan</InquiryPurpose><FirstName>RAVIKUMAR LOCHANRAM KANOJIYA</FirstName><InquiryAddresses><seq>1</seq><AddressType>H</AddressType><AddressLine1>ROOM NO -505 NALANDA APT BWING ALIYAWAR JUNG MARG NEAR BCC C,MUMBAI,Maharashtra</AddressLine1><State>MH</State><Postal>400051</Postal></InquiryAddresses><InquiryPhones><seq>1</seq><PhoneType>M</PhoneType><Number>9619592846</Number></InquiryPhones><IDDetails><seq>1</seq><IDType>T</IDType><IDValue>AXRPK6037F</IDValue><Source>Inquiry</Source></IDDetails><IDDetails><seq>2</seq><IDType>M</IDType><IDValue>620586164577</IDValue><Source>Inquiry</Source></IDDetails><DOB>1968-09-01</DOB><MFIDetails><FamilyDetails><seq>1</seq></FamilyDetails></MFIDetails></InquiryRequestInfo><Score><Type>ERS</Type><Version>3.1</Version></Score><CIRReportData><IDAndContactInfo><PersonalInfo><Name><FullName>RAVI KUMAR KANOJIA </FullName><FirstName>RAVI </FirstName><MiddleName>KUMAR </MiddleName><LastName>KANOJIA </LastName></Name><_x0020_AliasName /><DateOfBirth>1968-09-01</DateOfBirth><Gender>Male</Gender><Age><Age>52</Age></Age><PlaceOfBirthInfo /></PersonalInfo><IdentityInfo><PANId><seq>1</seq><ReportedDate>2020-05-31</ReportedDate><IdNumber>AXRPK6037E</IdNumber></PANId><PANId><seq>2</seq><ReportedDate>2017-11-30</ReportedDate><IdNumber>AXRPK6037F</IdNumber></PANId></IdentityInfo><AddressInfo><Seq>1</Seq><ReportedDate>2020-05-31</ReportedDate><Address>ROOM NO 505 NALANDA APTS  B WING ALIYAWER JUNG MARG KHAR EAST - SIDHESHWAER BLDG  MUMBAI</Address><State>MH</State><Postal>400051</Postal></AddressInfo><AddressInfo><Seq>2</Seq><ReportedDate>2020-05-31</ReportedDate><Address>ROOM NO 505 NALANDA APTS  B WING ALIYAWER JUNG MARG ROOM  NO 505 NALANDA APTS  MUMBAI</Address><State>MH</State><Postal>400051</Postal></AddressInfo><AddressInfo><Seq>3</Seq><ReportedDate>2017-10-31</ReportedDate><Address>ROOM NO 55 NALANDA APTS  ALIYAWER JUNG MA RG SIDDHARTH NAGAR  BANDRA EAST -  NR CATD INAL SCHOOL  MUMBAI</Address><State>MH</State><Postal>400051</Postal></AddressInfo><AddressInfo><Seq>4</Seq><ReportedDate>2015-09-30</ReportedDate><Address>NALANDA APT B WING 505 5TH ALI YAWAR JUNG MARG KHAR BANDRA E</Address><State>MH</State><Postal>400051</Postal><Type>Primary</Type></AddressInfo><AddressInfo><Seq>5</Seq><ReportedDate>2015-09-30</ReportedDate><Address>GALA NO 161 HALAV POOL CROSS LBS MARG KURLA</Address><State>MH</State><Postal>400070</Postal><Type>Office</Type></AddressInfo><PhoneInfo><seq>1</seq><typeCode>H</typeCode><ReportedDate>2015-09-30</ReportedDate><Number>9619592846</Number></PhoneInfo><PhoneInfo><seq>2</seq><typeCode>M</typeCode><ReportedDate>2020-05-31</ReportedDate><Number>9619592846</Number></PhoneInfo><PhoneInfo><seq>3</seq><typeCode>T</typeCode><ReportedDate>2015-09-30</ReportedDate><Number>9619592846</Number></PhoneInfo><EmailAddressInfo><seq>1</seq><ReportedDate>2015-09-30</ReportedDate><EmailAddress>KANOJIYA.RAVI@GMAIL.COM</EmailAddress></EmailAddressInfo></IDAndContactInfo><RetailAccountDetails><seq>1</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Personal Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>12632</Balance><PastDueAmount>0</PastDueAmount><Open>Yes</Open><SanctionAmount>40000</SanctionAmount><LastPaymentDate>2020-07-15</LastPaymentDate><DateReported>2020-07-31</DateReported><DateOpened>2018-02-21</DateOpened><AccountStatus>Current Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-18</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>2</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>Yes</Open><SanctionAmount>26500</SanctionAmount><LastPaymentDate>2020-01-02</LastPaymentDate><DateReported>2020-07-31</DateReported><DateOpened>2019-05-11</DateOpened><AccountStatus>Current Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-20</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>3</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>21990</SanctionAmount><LastPaymentDate>2019-03-02</LastPaymentDate><DateReported>2020-07-31</DateReported><DateOpened>2018-05-26</DateOpened><DateClosed>2019-05-21</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-20</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>4</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>12999</SanctionAmount><LastPaymentDate>2019-05-02</LastPaymentDate><DateReported>2020-07-31</DateReported><DateOpened>2018-10-31</DateOpened><DateClosed>2019-06-09</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-20</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-20</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>5</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>17900</SanctionAmount><LastPaymentDate>2018-02-02</LastPaymentDate><DateReported>2019-07-31</DateReported><DateOpened>2017-05-15</DateOpened><DateClosed>2018-07-31</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-17</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>6</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>17900</SanctionAmount><LastPaymentDate>2018-02-02</LastPaymentDate><DateReported>2019-07-31</DateReported><DateOpened>2017-08-31</DateOpened><DateClosed>2018-08-26</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-17</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>7</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>28000</SanctionAmount><LastPaymentDate>2018-08-02</LastPaymentDate><DateReported>2019-07-31</DateReported><DateOpened>2017-10-18</DateOpened><DateClosed>2018-09-14</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>07-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-18</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>8</seq><AccountNumber>**********</AccountNumber><Institution>FINANCE</Institution><AccountType>Consumer Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>16999</SanctionAmount><LastPaymentDate>2019-02-02</LastPaymentDate><DateReported>2019-07-31</DateReported><DateOpened>2018-08-05</DateOpened><DateClosed>2019-03-18</DateClosed><Reason>Closed Account</Reason><AccountStatus>Closed Account</AccountStatus><source>INDIVIDUAL</source><History48Months><key>07-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>06-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>05-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>04-19</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>03-19</key><PaymentStatus>*</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>02-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>01-19</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>12-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>11-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>10-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>09-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months><History48Months><key>08-18</key><PaymentStatus>NEW</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>*</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountDetails><seq>9</seq><AccountNumber>**********</AccountNumber><Institution>BANK</Institution><AccountType>Two-wheeler Loan</AccountType><OwnershipType>Individual</OwnershipType><Balance>0</Balance><PastDueAmount>0</PastDueAmount><Open>No</Open><SanctionAmount>54951</SanctionAmount><LastPaymentDate>2018-07-07</LastPaymentDate><DateReported>2018-07-31</DateReported><DateOpened>2015-04-27</DateOpened><DateClosed>2018-07-07</DateClosed><Reason>Closed Account</Reason><TermFrequency>Monthly</TermFrequency><AccountStatus>Closed Account</AccountStatus><AssetClassification>Standard</AssetClassification><source>INDIVIDUAL</source><History48Months><key>07-18</key><PaymentStatus>CLSD</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-18</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-17</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>08-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>07-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>06-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>05-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>04-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>03-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>02-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>01-16</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>12-15</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>11-15</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>10-15</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months><History48Months><key>09-15</key><PaymentStatus>000</PaymentStatus><SuitFiledStatus>*</SuitFiledStatus><AssetClassificationStatus>STD</AssetClassificationStatus></History48Months></RetailAccountDetails><RetailAccountsSummary><NoOfAccounts>9</NoOfAccounts><NoOfActiveAccounts>2</NoOfActiveAccounts><NoOfWriteOffs>0</NoOfWriteOffs><TotalPastDue>0.00</TotalPastDue><MostSevereStatusWithIn24Months>Non-Delnqt</MostSevereStatusWithIn24Months><SingleHighestCredit>0.00</SingleHighestCredit><SingleHighestSanctionAmount>40000.00</SingleHighestSanctionAmount><TotalHighCredit>0.00</TotalHighCredit><AverageOpenBalance>6316.00</AverageOpenBalance><SingleHighestBalance>12632.00</SingleHighestBalance><NoOfPastDueAccounts>0</NoOfPastDueAccounts><NoOfZeroBalanceAccounts>1</NoOfZeroBalanceAccounts><RecentAccount>Consumer Loan on 11-05-2019</RecentAccount><OldestAccount>Two-wheeler Loan on 27-04-2015</OldestAccount><TotalBalanceAmount>12632.00</TotalBalanceAmount><TotalSanctionAmount>66500.00</TotalSanctionAmount><TotalCreditLimit>0.0</TotalCreditLimit><TotalMonthlyPaymentAmount>0.00</TotalMonthlyPaymentAmount></RetailAccountsSummary><ScoreDetails><Type>ERS</Type><Version>3.1</Version><Name>M001</Name><Value>748</Value><ScoringElements><type>RES</type><seq>1</seq><Description>Number of product trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>2</seq><code>7b</code><Description>Number of two-wheeler accounts</Description></ScoringElements><ScoringElements><type>RES</type><seq>3</seq><code>5b</code><Description>Balance amount of home loan trades</Description></ScoringElements><ScoringElements><type>RES</type><seq>4</seq><code>8a</code><Description>Sanctioned amount of or lack of credit card trades</Description></ScoringElements></ScoreDetails><Enquiries><seq>0</seq><Institution>Centrum Microcredit Pvt. Ltd.</Institution><Date>2020-09-28</Date><Time>17:18</Time><RequestPurpose>0E</RequestPurpose></Enquiries><EnquirySummary><Purpose>ALL</Purpose><Total>1</Total><Past30Days>1</Past30Days><Past12Months>1</Past12Months><Past24Months>1</Past24Months><Recent>2020-09-28</Recent></EnquirySummary><OtherKeyInd><AgeOfOldestTrade>66</AgeOfOldestTrade><NumberOfOpenTrades>2</NumberOfOpenTrades><AllLinesEVERWritten>0.00</AllLinesEVERWritten><AllLinesEVERWrittenIn9Months>0</AllLinesEVERWrittenIn9Months><AllLinesEVERWrittenIn6Months>0</AllLinesEVERWrittenIn6Months></OtherKeyInd><RecentActivities><AccountsDeliquent>0</AccountsDeliquent><AccountsOpened>0</AccountsOpened><TotalInquiries>1</TotalInquiries><AccountsUpdated>4</AccountsUpdated></RecentActivities></CIRReportData></CIRReportDataLst></CCRResponse></root>";
                    string pEqXml = "";
                    try
                    {
                        //*************************** For Live ***************************************************                      
                        WebServiceSoapClient eq = new WebServiceSoapClient();

                        //************************************************GenderId 1 For MALE else Female****************************************
                        //if (dt.Rows[0]["GenderId"].ToString() == "1")
                        //{
                        //    pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                        //        , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                        //         dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                        //          dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                        //          "5750", PCSUserName, PCSPassword, "027FP27137", "9GH", " ", "PCS", "ERS", "3.1","PRO");
                        //}
                        //else
                        //{
                        pEqXml = eq.Equifax(dt.Rows[0]["FirstName"].ToString(), dt.Rows[0]["MiddleName"].ToString(), dt.Rows[0]["LastName"].ToString(), dt.Rows[0]["DOB"].ToString()
                            , dt.Rows[0]["AddressType"].ToString(), dt.Rows[0]["AddressLine1"].ToString(), dt.Rows[0]["StateName"].ToString(), dt.Rows[0]["PIN"].ToString(),
                             dt.Rows[0]["MobileNo"].ToString(), dt.Rows[0]["IDType"].ToString(), dt.Rows[0]["IDValue"].ToString(), dt.Rows[0]["AddType"].ToString(),
                              dt.Rows[0]["AddValue"].ToString(), dt.Rows[0]["CoAppRel"].ToString(), dt.Rows[0]["CoAppName"].ToString(),
                              "5750", CCRUserName, CCRPassword, "027FZ01546", "KQ7", "123456", "CCR", "ERS", "3.1", "PRO");
                        //}
                        //*************************************************************************
                        vErr = oCAp.UpdateEquifaxInformation(txtAppNo.Text, Convert.ToInt32(vCBID), pEqXml, vBranch, "", Convert.ToInt32(Session[gblValue.UserId]), vLogDt, "P", pErrorMsg, ref pStatus, ref pStatusDesc);
                        if (vErr == 1)
                        {
                            string[] arr = pStatusDesc.Split(',');
                            string[] arr1 = arr[0].Split('=');
                            string vAcceptYN = arr1[1].ToString();
                            if (vAcceptYN == "Y")
                            {
                                btnEquifaxVerifyCA.Text = "Verified";
                                btnEquifaxVerifyCA.Enabled = false;
                                lblEquCBScore.Text = arr[1].ToString();
                            }
                            else
                            {
                                btnEquifaxVerifyCA.Text = "Cancel";
                                btnEquifaxVerifyCA.Enabled = false;
                                lblEquCBScore.Text = arr[1].ToString();
                            }
                            gblFuction.AjxMsgPopup(pStatusDesc);
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
                    }
                }
            }
        }

        protected void btnVerifyCA_Click(object sender, EventArgs e)
        {
            if (txtAppNo.Text == "")
            {
                gblFuction.AjxMsgPopup("Please save the application first.");
                return;
            }
            if (hdnQaYN.Value == "N")
            {
                gblFuction.AjxMsgPopup("Before Proxie this Operation is not possible.");
                return;
            }
            else
            {
                gblFuction.AjxMsgPopup("Not Avilable.");
                return;
            }
        }
        private string setRelationIdForHighMark(string relation)
        {
            string rel_id = "";
            switch (relation)
            {
                case "2": //FATHER
                    rel_id = "K01";
                    break;
                case "8": //HUSBAND
                    rel_id = "K02";
                    break;
                case "3": //MOTHER
                    rel_id = "K03";
                    break;
                case "6": //SON
                    rel_id = "K04";
                    break;
                case "7": //DAUGHTER
                    rel_id = "K05";
                    break;
                case "15": //WIFE
                    rel_id = "K06";
                    break;
                case "5": //BROTHER
                    rel_id = "K07";
                    break;
                case "10": //MOTHER IN LAW
                    rel_id = "K08";
                    break;
                case "9": //FATHER IN LAW
                    rel_id = "K09";
                    break;
                case "14": //DAUGHTER IN LAW
                    rel_id = "K10";
                    break;
                case "11": //SISTER IN LAW
                    rel_id = "K11";
                    break;
                case "13": //SON IN LAW
                    rel_id = "K12";
                    break;
                case "12": //BROTHER IN LAW
                    rel_id = "K13";
                    break;
                default: //Other
                    rel_id = "K15";
                    break;
            }
            return rel_id;
        }
        protected void btnDownloadCustEnq_Click(object sender, EventArgs e)
        {
            return;
            //Button btnShow = (Button)sender;
            //GridViewRow gR = (GridViewRow)btnShow.NamingContainer;
            //int vCbId =Convert.ToInt32(gR.Cells[8].Text);
            //SetParameterForRptData(vCbId, "PDF");
        }
        protected void btnDownloadCustEnqEquifax_Click(object sender, EventArgs e)
        {
            ImageButton btnShow = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnShow.NamingContainer;
            int vCbId = Convert.ToInt32(gR.Cells[8].Text);
            SetParameterForRptData(vCbId, "PDF");
        }

        protected void btnDownloadCoAppEnqifax_Click(object sender, EventArgs e)
        {
            ImageButton btnShow = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnShow.NamingContainer;
            int vCbId = Convert.ToInt32(gR.Cells[9].Text);
            SetParameterForRptData(vCbId, "PDF");
        }

        protected void btnDownloadCoAppEnq_Click(object sender, EventArgs e)
        {
            ImageButton btnShow = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnShow.NamingContainer;
            Label lblReportID = (Label)gR.FindControl("lblReportID");
            string url = "../Master/PopUpReportWindow.aspx?A0A=" + HttpUtility.UrlEncode(lblReportID.Text);
            string strPopup = "<script language='javascript' ID='script1'>"
            + "window.open('" + url
            + "','new window', 'top=90, left=400, width=600, height=500, dependant=no, location=0, alwaysRaised=yes, menubar=no, resizeable=yes, scrollbars=yes, toolbar=no,status=no, center=yes')"
            + "</script>";
            ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", strPopup, false);
        }
        protected void gvApp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnVerifyEquifax = (LinkButton)e.Row.FindControl("btnVerifyEquifax");
                ImageButton btnDownloadCustEnqEquifax = (ImageButton)e.Row.FindControl("btnDownloadCustEnqEquifax");
                Label lblIsEquifaxEnqDone = (Label)e.Row.FindControl("lblIsEquifaxEnqDone");
                if (lblIsEquifaxEnqDone.Text == "N")
                {
                    btnVerifyEquifax.Enabled = true;
                    btnVerifyEquifax.Text = "Equifax Checking";
                    btnDownloadCustEnqEquifax.Enabled = false;
                }
                else
                {
                    btnVerifyEquifax.Enabled = false;
                    btnVerifyEquifax.Text = "Equifax Checking Done";
                    btnDownloadCustEnqEquifax.Enabled = true;
                }
            }
        }
        protected void gvCoAppDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnVerify = (Button)e.Row.FindControl("btnVerify");
                ImageButton btnDownloadCoAppEnq = (ImageButton)e.Row.FindControl("btnDownloadCoAppEnq");
                ImageButton btnDownloadCoAppEnqifax = (ImageButton)e.Row.FindControl("btnDownloadCoAppEnqifax");
                LinkButton btnEquifaxVerifyCA = (LinkButton)e.Row.FindControl("btnEquifaxVerifyCA");
                Label lblIsActive = (Label)e.Row.FindControl("lblIsActive");
                Label lblIsEquifaxEnqDone = (Label)e.Row.FindControl("lblIsEquifaxEnqDone");
                CheckBox chkCoApp = (CheckBox)e.Row.FindControl("chkCoApp");
                if (lblIsActive.Text == "N")
                {
                    chkCoApp.Checked = false;
                }
                else
                {
                    chkCoApp.Checked = true;
                }

                if (lblIsEquifaxEnqDone.Text == "N")
                {
                    btnEquifaxVerifyCA.Enabled = true;
                    btnEquifaxVerifyCA.Text = "Equifax Checking";
                    btnDownloadCoAppEnqifax.Enabled = false;
                }
                else
                {
                    btnEquifaxVerifyCA.Enabled = false;
                    btnEquifaxVerifyCA.Text = "Equifax Checking Done";
                    btnDownloadCoAppEnqifax.Enabled = true;
                }
            }
        }
        private void PopBankAcType()
        {
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetBankAcctType();
                if (dt.Rows.Count > 0)
                {
                    ddlCustACType.DataSource = dt;
                    ddlCustACType.DataTextField = "AccType";
                    ddlCustACType.DataValueField = "AccTypeID";
                    ddlCustACType.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCustACType.Items.Insert(0, oli1);

                    ddlCAACType.DataSource = dt;
                    ddlCAACType.DataTextField = "AccType";
                    ddlCAACType.DataValueField = "AccTypeID";
                    ddlCAACType.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCAACType.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCustACType.DataSource = null;
                    ddlCustACType.DataBind();
                    ddlCAACType.DataSource = null;
                    ddlCAACType.DataBind();
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
        private DataTable GetDependentDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("RelationId", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("OccupationId", typeof(int));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[1]["SlNo"] = 2;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[2]["SlNo"] = 3;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[3]["SlNo"] = 4;
            dt.AcceptChanges();
            return dt;
        }
        private void GetCADependentDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("RelationId", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("OccupationId", typeof(int));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[1]["SlNo"] = 2;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[2]["SlNo"] = 3;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[3]["SlNo"] = 4;
            dt.AcceptChanges();

            gvCADep.DataSource = dt;
            gvCADep.DataBind();

        }
        protected void gvCustDep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCustDepRel = (e.Row.FindControl("ddlCustDepRel") as DropDownList);
                Label lblCustDepRelId = (e.Row.FindControl("lblCustDepRelId") as Label);
                DropDownList ddlCustDepOcc = (e.Row.FindControl("ddlCustDepOcc") as DropDownList);
                Label lblCustDepOccId = (e.Row.FindControl("lblCustDepOccId") as Label);

                try
                {
                    dt = oMem.GetRelationList();
                    if (dt.Rows.Count > 0)
                    {
                        ddlCustDepRel.DataSource = dt;
                        ddlCustDepRel.DataTextField = "Relation";
                        ddlCustDepRel.DataValueField = "RelationId";
                        ddlCustDepRel.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCustDepRel.Items.Insert(0, oli1);
                    }
                    string CustDepRelId = lblCustDepRelId.Text;
                    if (CustDepRelId != " ")
                        ddlCustDepRel.SelectedValue = CustDepRelId;

                    dt1 = oMem.GetGenderAndCastAndMaritalAndReligion(5);
                    if (dt1.Rows.Count > 0)
                    {
                        ddlCustDepOcc.DataSource = dt1;
                        ddlCustDepOcc.DataTextField = "Occupation";
                        ddlCustDepOcc.DataValueField = "OccupationId";
                        ddlCustDepOcc.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCustDepOcc.Items.Insert(0, oli1);
                    }
                    string CustDepOccId = lblCustDepOccId.Text;
                    if (CustDepOccId != " ")
                        ddlCustDepOcc.SelectedValue = CustDepOccId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                    dt = null;
                    dt1 = null;
                }
            }
        }
        protected void gvCADep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCADepRel = (e.Row.FindControl("ddlCADepRel") as DropDownList);
                Label lblCADepRelId = (e.Row.FindControl("lblCADepRelId") as Label);
                DropDownList ddlCADepOcc = (e.Row.FindControl("ddlCADepOcc") as DropDownList);
                Label lblCADepOccId = (e.Row.FindControl("lblCADepOccId") as Label);

                try
                {
                    dt = oMem.GetRelationList();
                    if (dt.Rows.Count > 0)
                    {
                        ddlCADepRel.DataSource = dt;
                        ddlCADepRel.DataTextField = "Relation";
                        ddlCADepRel.DataValueField = "RelationId";
                        ddlCADepRel.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCADepRel.Items.Insert(0, oli1);
                    }
                    string CADepRelId = lblCADepRelId.Text;
                    if (CADepRelId != " ")
                        ddlCADepRel.SelectedValue = CADepRelId;

                    dt1 = oMem.GetGenderAndCastAndMaritalAndReligion(5);
                    if (dt1.Rows.Count > 0)
                    {
                        ddlCADepOcc.DataSource = dt1;
                        ddlCADepOcc.DataTextField = "Occupation";
                        ddlCADepOcc.DataValueField = "OccupationId";
                        ddlCADepOcc.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCADepOcc.Items.Insert(0, oli1);
                    }
                    string CADepOccId = lblCADepOccId.Text;
                    if (CADepOccId != " ")
                        ddlCADepOcc.SelectedValue = CADepOccId;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dt = null;
                    oMem = null;
                    dt = null;
                    dt1 = null;
                }
            }
        }
        private string GetStateSrtCode(string vStateName)
        {
            switch (vStateName)
            {
                case "ANDAMAN AND NICOBAR ISLANDS": return "AN";
                case "ANDHRA PRADESH": return "AP";
                case "ARUNACHAL PRADESH": return "AR";
                case "ASSAM": return "AS";
                case "BIHAR": return "BR";
                case "CHANDIGARH": return "CH";
                case "CHHATTISGARH": return "CT";
                case "DADRA AND NAGAR HAVELI": return "DN";
                case "DAMAN AND DIU": return "DD";
                case "DELHI": return "DL";
                case "GOA": return "GA";
                case "GUJARAT": return "GJ";
                case "HARYANA": return "HR";
                case "HIMACHAL PRADESH": return "HP";
                case "JAMMU AND KASHMIR": return "JK";
                case "JHARKHAND": return "JH";
                case "KARNATAKA": return "KA";
                case "KERALA": return "KL";
                case "LAKSHADWEEP": return "LD";
                case "MADHYA PRADESH": return "MP";
                case "MAHARASHTRA": return "MH";
                case "MANIPUR": return "MN";
                case "MEGHALAYA": return "ML";
                case "MIZORAM": return "MZ";
                case "NAGALAND": return "NL";
                case "ORISSA": return "OR";
                case "PONDICHERRY": return "PY";
                case "PUNJAB": return "PB";
                case "RAJASTHAN": return "RJ";
                case "SIKKIM": return "SK";
                case "TAMIL NADU": return "TN";
                case "TRIPURA": return "TR";
                case "UTTAR PRADESH": return "UP";
                case "UTTARAKHAND": return "UK";
                case "WEST BENGAL": return "WB";
                default: return "";
            }
        }
        private void BindData()
        {
            CLoanApplication Cla = new CLoanApplication();
            DataTable dtCategory, dtVariables;
            dtCategory = Cla.GetCategory();
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            int n = 0, i = 0;

            foreach (DataRow rows in dtCategory.Rows)
            {
                row = new HtmlTableRow();
                cell = new HtmlTableCell();
                cell.ColSpan = 3;
                cell.InnerHtml = rows["Category"].ToString() + "<br/>";
                cell.Style.Add("Font-Weight", "bold");
                cell.Style.Add("color", "Blue");
                cell.Style.Add("text-decoration", "underline");
                row.Cells.Add(cell);
                tblHeading.Rows.Add(row);

                dtVariables = Cla.GetCategoryVariables(Convert.ToInt32(rows["CategoryID"].ToString()));
                ViewState["Variables"] = dtVariables;
                int j = 1;
                foreach (DataRow rowQuestions in dtVariables.Rows)
                {
                    //Question No Part
                    row = new HtmlTableRow();
                    cell = new HtmlTableCell();
                    Label lblQues = new Label();
                    lblQues.ClientIDMode = ClientIDMode.Static;
                    lblQues.ID = "lblQues_" + n;
                    lblQues.Text = j + "." + rowQuestions["Variable"].ToString();
                    cell.Controls.Add(lblQues);
                    row.Cells.Add(cell);
                    n++;
                    j++;


                    cell = new HtmlTableCell();
                    HiddenField HdnQuesId = new HiddenField();
                    HdnQuesId.ID = "HdnQuesId_" + i;
                    HdnQuesId.Value = rowQuestions["VariableId"].ToString();
                    cell.Controls.Add(HdnQuesId);
                    row.Cells.Add(cell);


                    //Add dynamically dropdowlist for answer
                    cell = new HtmlTableCell();
                    DropDownList ddlAns = new DropDownList();
                    ddlAns.ID = "ddlAns_" + i;
                    ArrayList Options = new ArrayList(Convert.ToString(rowQuestions["Options"]).Split(','));
                    ddlAns.DataSource = Options;
                    ddlAns.DataBind();
                    cell.Controls.Add(ddlAns);
                    row.Cells.Add(cell);
                    /*End */

                    //Add dynamically textbox for comment

                    cell = new HtmlTableCell();
                    TextBox txtCommment = new TextBox();
                    txtCommment.ID = "txtCommment" + i;
                    txtCommment.Attributes.Add("placeholder", "Comment here");
                    cell.Controls.Add(txtCommment);
                    row.Cells.Add(cell);
                    /*End */

                    tblHeading.Rows.Add(row);
                    i++;
                }
            }
        }
        private void FetchBindData(DataTable dt1)
        {
            CLoanApplication Cla = new CLoanApplication();
            DataTable dtCategory, dtVariables;
            dtCategory = Cla.GetCategory();
            HtmlTableRow row = null;
            HtmlTableCell cell = null;
            int n = 0, i = 0;
            tpProxies.Visible = true;
            foreach (DataRow rows in dtCategory.Rows)
            {

                row = new HtmlTableRow();
                cell = new HtmlTableCell();
                cell.ColSpan = 4;
                cell.InnerHtml = rows["Category"].ToString() + "<br/><br/>";
                cell.Style.Add("Font-Weight", "bold");
                cell.Style.Add("color", "Blue");
                cell.Style.Add("text-decoration", "underline");
                row.Cells.Add(cell);
                tblHeading.Rows.Add(row);

                dtVariables = Cla.GetCategoryVariables(Convert.ToInt32(rows["CategoryID"].ToString()));
                ViewState["Variables"] = dtVariables;

                int j = 1;
                foreach (DataRow rowQuestions in dtVariables.Rows)
                {
                    //Question No Part
                    row = new HtmlTableRow();
                    cell = new HtmlTableCell();
                    Label lblQues = new Label();
                    lblQues.ClientIDMode = ClientIDMode.Static;
                    lblQues.ID = "lblQues_" + n;
                    lblQues.Text = j + "." + rowQuestions["Variable"].ToString();
                    cell.Controls.Add(lblQues);
                    row.Cells.Add(cell);
                    n++;
                    j++;


                    cell = new HtmlTableCell();
                    HiddenField HdnQuesId = new HiddenField();
                    HdnQuesId.ID = "HdnQuesId_" + i;
                    HdnQuesId.Value = rowQuestions["VariableId"].ToString();
                    cell.Controls.Add(HdnQuesId);
                    row.Cells.Add(cell);


                    //Add dynamically dropdowlist for answer
                    cell = new HtmlTableCell();
                    DropDownList ddlAns = new DropDownList();
                    ddlAns.ID = "ddlAns_" + i;
                    ArrayList Options = new ArrayList(Convert.ToString(rowQuestions["Options"]).Split(','));
                    ddlAns.DataSource = Options;
                    ddlAns.DataBind();

                    if (dt1.Rows.Count > 0)
                    {
                        ddlAns.SelectedValue = Convert.ToString(dt1.Rows[i]["Options"]).Trim();
                    }
                    cell.Controls.Add(ddlAns);
                    row.Cells.Add(cell);
                    /*End */

                    //Add dynamically textbox for comment

                    cell = new HtmlTableCell();
                    TextBox txtCommment = new TextBox();
                    txtCommment.ID = "txtCommment" + i;
                    txtCommment.Attributes.Add("placeholder", "Comment here");
                    if (dt1.Rows.Count > 0)
                    {
                        txtCommment.Text = Convert.ToString(dt1.Rows[i]["Comment"]).Trim();
                    }
                    cell.Controls.Add(txtCommment);
                    row.Cells.Add(cell);
                    /*End */

                    tblHeading.Rows.Add(row);
                    i++;
                }
            }

        }

        private void SetParameterForRptData(int pCbId, string pType)
        {
            DataSet ds = null;
            DataTable dt1 = null, dt2 = null, dt3 = null, dt4 = null, dt5 = null, dt6 = null, dt7 = null, dt8 = null;
            CReports oRpt = null;
            string vRptPath = "";
            string vBranch = Session[gblValue.BrName].ToString();
            try
            {
                //cvar = 1;
                oRpt = new CReports();
                string enqstatusmsg = "";
                ds = oRpt.Equifax_Report_New(pCbId, ref  enqstatusmsg);
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
                dt4 = ds.Tables[3];
                dt5 = ds.Tables[4];
                dt6 = ds.Tables[5];
                dt7 = ds.Tables[6];
                dt8 = ds.Tables[7];

                //-------------------------------------------------------------------------
                dt4.TableName = "dtRepeatCustTrack";
                dt5.TableName = "dtRepeatCustTrackTimeline";
                dt6.TableName = "dtIndLoanDtl";
                dt7.TableName = "dtRePayTimeline";
                //dt1 = ds.Tables[0];
                //dt2 = ds.Tables[1];
                //dt3 = ds.Tables[2];
                //dt1.TableName = "CBPortDtl";
                //dt2.TableName = "CBPortMst";
                //dt3.TableName = "CBHistoryMonth";
                if (pType == "PDF")
                {
                    vRptPath = Request.PhysicalApplicationPath.ToString() + "Reports\\RptEquifaxCCR.rpt";
                    using (ReportDocument rptDoc = new ReportDocument())
                    {
                        rptDoc.Load(vRptPath);
                        rptDoc.SetDataSource(dt1);
                        rptDoc.Subreports["rptActiveLoan"].SetDataSource(dt2);
                        rptDoc.Subreports["RptCloseLoan"].SetDataSource(dt3);
                        rptDoc.Subreports["rptRepeatCustTrack"].SetDataSource(ds);
                        rptDoc.Subreports["RptIndivLoanDtl"].SetDataSource(ds);
                        rptDoc.Subreports["RptEnquiries"].SetDataSource(dt8);
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


        protected void ddlCustReligion_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMinority();
        }

        private void SetMinority()
        {
            Int32 vCommState = Convert.ToInt32(ddlMState.SelectedValue);
            Int32 vApplReligion = Convert.ToInt32(ddlCustReligion.SelectedValue);
            if ((vCommState == 11 && vApplReligion == 3) || (vCommState == 18 && vApplReligion == 5) || (vCommState == 19 && vApplReligion == 5)
            || (vCommState == 20 && vApplReligion == 5) || (vCommState == 22 && vApplReligion == 4) || (vCommState == 35 && vApplReligion == 3))
            {
                ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("N"));
            }
            else
                if (vApplReligion == 1 || vApplReligion == 9)
                {
                    ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("N"));
                }
                else
                {
                    ddlMinorityYN.SelectedIndex = ddlMinorityYN.Items.IndexOf(ddlMinorityYN.Items.FindByValue("Y"));
                }
        }

        protected void ddlAbledYN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAbledYN.SelectedValue != "Y")
            {
                ddlSpclAbled.Enabled = false;
                ddlSpclAbled.SelectedIndex = -1;
            }
            else
            {
                ddlSpclAbled.Enabled = true;
            }
        }

        private void popSpeciallyAbled()
        {
            DataTable dt = null;
            CGblIdGenerator oGb = null;
            oGb = new CGblIdGenerator();
            dt = oGb.GetSpclAble();

            ddlSpclAbled.DataSource = dt;
            ddlSpclAbled.DataTextField = "Question";
            ddlSpclAbled.DataValueField = "Qno";
            ddlSpclAbled.DataBind();
            ListItem oli = new ListItem("<--Select-->", "-1");
            ddlSpclAbled.Items.Insert(0, oli);

        }

        #region MemberImageUpload

        private string SaveMemberImage(FileUpload flup, string imageGroup, string imageName, string Mode, string isImageSaved, string ImagePath)
        {
            if (MinioYN == "N")
            {
                try
                {
                    string folderPath = string.Format("{0}", ImagePath);
                    System.IO.Directory.CreateDirectory(folderPath);
                    string filePath = string.Format("{0}/{1}.png", folderPath, imageGroup + "_" + imageName);
                    Stream strm = flup.PostedFile.InputStream;
                    var targetFile = filePath;

                    if ((Mode == "Delete"))
                    {
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        isImageSaved = "N";
                    }
                    else
                    {
                        if (flup.HasFile)
                        {
                            if (Mode == "Edit")
                            {
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                            }
                            ReduceImageSize(0.5, strm, targetFile);
                            isImageSaved = "Y";
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                CApiCalling oAC = new CApiCalling();
                byte[] ImgByte = ConvertFileToByteArray(flup.PostedFile);
                isImageSaved = oAC.UploadFileMinio(ResizeImage(ImgByte, 0.8), imageName + ".png", imageGroup, MemberBucket, MinioUrl);
            }
            return isImageSaved;
        }        

        #region ResizeImage
        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        public static byte[] ResizeImage(byte[] imageBytes, double scale)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                using (Bitmap original = new Bitmap(memoryStream))
                {
                    int newWidth = (int)(original.Width * scale);
                    int newHeight = (int)(original.Height * scale);

                    using (Bitmap resized = new Bitmap(original, newWidth, newHeight))
                    {
                        using (MemoryStream resultStream = new MemoryStream())
                        {
                            resized.Save(resultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return resultStream.ToArray();
                        }
                    }
                }
            }
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

    }

}
