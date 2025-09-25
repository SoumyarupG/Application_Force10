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
using System.Xml;
using SendSms;
using Newtonsoft.Json;
using System.Web;
using System.Net;
using System.Text;

namespace CENTRUMSME.WebPages.Private.Master
{
    public partial class CustomerAllocation : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string pathImage = "", PathKYCImage = "", CustomerId = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                InitBasePage();
                // txtAdmDtApp.Text = Session[gblValue.LoginDate].ToString();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;
                ViewState["CusTID"] = null;
                ViewState["LoanAppId"] = null;
                hdUserID.Value = this.UserID.ToString();
                mView.ActiveViewIndex = 0;
                LoadCompanyList();
                StatusButton("View");
                btnSaveBS.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSaveBS, "").ToString());
                btnSavePL.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSaveBS, "").ToString());
                //txttext.Attributes.Add("onkeypress", "return numericOnly(this);");
            }
            else
            {
                //WebControl wcICausedPostBack = (WebControl)GetControlThatCausedPostBack(sender as Page);
                //int indx = wcICausedPostBack.TabIndex;
                //var ctrl = from control in wcICausedPostBack.Parent.Controls.OfType<WebControl>()
                //           where control.TabIndex > indx
                //           select control;
                //ctrl.DefaultIfEmpty(wcICausedPostBack).First().Focus();
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
                this.PageHeading = "Master Bucket";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCustomermst);
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
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Master Bucket", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
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
        // Population of All Tab's Data on Click of Show Information in Loan Application
        private void ShowLoanRelationDetails(string pLnAppId)
        {
            if (pLnAppId != "")
            {
                // get reference details
                LoadRefList(pLnAppId);
                // get CB Online Details
                LoadCBOnlineList(pLnAppId);
                // get CB Details
                LoadCBList(pLnAppId);
                // get CB Remarks
                GetCBRemarkByLnAppId(pLnAppId);
                // get Credit Bureau Doc
                ShowUploadedCBDoc(pLnAppId);
                // get uploaded FI Documents
                ShowUploadedFIDoc(pLnAppId);
                // get uploaded CB documents
                ShowUploadedDocuments(pLnAppId);
                //get Bank Statement Details
                LoadBankAcList(pLnAppId);
                //get bank Satement Summary Details
                LoadBankAcSummary(pLnAppId);
                // get bank statement Doc
                ShowUploadedBSDoc(pLnAppId);
                // get Bank Statement Remarks
                GetBSRemarkByLnAppId(pLnAppId);
                // get balance sheet list
                LoadBSList(pLnAppId);
                // get profit and loss list
                LoadPLList(pLnAppId);
                // get Fin Statement Uploaded Doc
                LoadFinStatDoc(pLnAppId);
                // get ITR File
                LoadITRFile(pLnAppId);
                // get CAM Data
                LoadCAMList(pLnAppId);
                // get Ratio Data
                LoadRatioData(pLnAppId);
                // get Financial Statement Remarks
                GetFSRemarkByLnAppId(pLnAppId);
                // get All CAM Remarks
                GetCAMRemarkByLnAppId(pLnAppId);
                // Get Combine MOS
                GetCombineMOS(pLnAppId);
                // Get BankToTRRatio
                GetBankToTR(pLnAppId);
            }
        }
        private void popAMQuestion()
        {
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
            DataTable dt10 = new DataTable();
            DataTable dt11 = new DataTable();
            DataTable dt12 = new DataTable();
            DataTable dt13 = new DataTable();
            DataTable dt14 = new DataTable();
            DataTable dt15 = new DataTable();
            DataTable dt16 = new DataTable();
            DataTable dt17 = new DataTable();
            DataTable dt18 = new DataTable();

            CMember OMem = new CMember();
            ds = OMem.GetAMQuestion();

            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            dt3 = ds.Tables[2];
            dt4 = ds.Tables[3];
            dt5 = ds.Tables[4];
            dt6 = ds.Tables[5];
            dt7 = ds.Tables[6];
            dt8 = ds.Tables[7];
            dt9 = ds.Tables[8];
            dt10 = ds.Tables[9];
            dt11 = ds.Tables[10];
            dt12 = ds.Tables[11];
            dt13 = ds.Tables[12];
            dt14 = ds.Tables[13];
            dt15 = ds.Tables[14];
            dt16 = ds.Tables[15];
            dt17 = ds.Tables[16];
            dt18 = ds.Tables[17];
            lblAMQuestion1.Text = ds.Tables[0].Rows[0][0].ToString();
            lblAMQuestion2.Text = ds.Tables[1].Rows[0][0].ToString();
            lblAMQuestion3.Text = ds.Tables[2].Rows[0][0].ToString();
            lblAMQuestion4.Text = ds.Tables[3].Rows[0][0].ToString();
            lblAMQuestion5.Text = ds.Tables[4].Rows[0][0].ToString();
            lblAMQuestion6.Text = ds.Tables[5].Rows[0][0].ToString();
            lblAMQuestion7.Text = ds.Tables[6].Rows[0][0].ToString();
            lblAMQuestion8.Text = ds.Tables[7].Rows[0][0].ToString();
            lblAMQuestion9.Text = ds.Tables[8].Rows[0][0].ToString();
            lblAMQuestion10.Text = ds.Tables[9].Rows[0][0].ToString();
            lblAMQuestion11.Text = ds.Tables[10].Rows[0][0].ToString();
            lblAMQuestion12.Text = ds.Tables[11].Rows[0][0].ToString();
            lblAMQuestion13.Text = ds.Tables[12].Rows[0][0].ToString();
            lblAMQuestion14.Text = ds.Tables[13].Rows[0][0].ToString();
            lblAMQuestion15.Text = ds.Tables[14].Rows[0][0].ToString();
            lblAMQuestion16.Text = ds.Tables[15].Rows[0][0].ToString();
            lblAMQuestion17.Text = ds.Tables[16].Rows[0][0].ToString();
            lblAMQuestion18.Text = ds.Tables[17].Rows[0][0].ToString();

        }
        private void popAMValues()
        {
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
            DataTable dt10 = new DataTable();
            DataTable dt11 = new DataTable();
            DataTable dt12 = new DataTable();
            DataTable dt13 = new DataTable();
            DataTable dt14 = new DataTable();
            DataTable dt15 = new DataTable();
            DataTable dt16 = new DataTable();
            DataTable dt17 = new DataTable();
            DataTable dt18 = new DataTable();

            CMember OMem = new CMember();
            ds = OMem.GetAMValues();

            dt1 = ds.Tables[0];
            dt2 = ds.Tables[1];
            dt3 = ds.Tables[2];
            dt4 = ds.Tables[3];
            dt5 = ds.Tables[4];
            dt6 = ds.Tables[5];
            dt7 = ds.Tables[6];
            dt8 = ds.Tables[7];
            dt9 = ds.Tables[8];
            dt10 = ds.Tables[9];
            dt11 = ds.Tables[10];
            dt12 = ds.Tables[11];
            dt13 = ds.Tables[12];
            dt14 = ds.Tables[13];
            dt15 = ds.Tables[14];
            dt16 = ds.Tables[15];
            dt17 = ds.Tables[16];
            dt18 = ds.Tables[17];



            ddlAMAns1.DataSource = dt1;
            ddlAMAns1.DataValueField = "AMValueId";
            ddlAMAns1.DataTextField = "ValueName";
            ddlAMAns1.DataBind();
            ListItem oli = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns1.Items.Insert(0, oli);
            hfPartId1.Value = dt1.Rows[0]["ParticularId"].ToString();

            ddlAMAns2.DataSource = dt2;
            ddlAMAns2.DataValueField = "AMValueId";
            ddlAMAns2.DataTextField = "ValueName";
            ddlAMAns2.DataBind();
            ListItem oli1 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns2.Items.Insert(0, oli1);
            hfPartId2.Value = dt2.Rows[0]["ParticularId"].ToString();

            ddlAMAns3.DataSource = dt3;
            ddlAMAns3.DataValueField = "AMValueId";
            ddlAMAns3.DataTextField = "ValueName";
            ddlAMAns3.DataBind();
            ListItem oli2 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns3.Items.Insert(0, oli2);
            hfPartId3.Value = dt3.Rows[0]["ParticularId"].ToString();

            ddlAMAns4.DataSource = dt4;
            ddlAMAns4.DataValueField = "AMValueId";
            ddlAMAns4.DataTextField = "ValueName";
            ddlAMAns4.DataBind();
            ListItem oli3 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns4.Items.Insert(0, oli3);
            hfPartId4.Value = dt4.Rows[0]["ParticularId"].ToString();

            ddlAMAns5.DataSource = dt5;
            ddlAMAns5.DataValueField = "AMValueId";
            ddlAMAns5.DataTextField = "ValueName";
            ddlAMAns5.DataBind();
            ListItem oli4 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns5.Items.Insert(0, oli4);
            hfPartId5.Value = dt5.Rows[0]["ParticularId"].ToString();

            ddlAMAns6.DataSource = dt6;
            ddlAMAns6.DataValueField = "AMValueId";
            ddlAMAns6.DataTextField = "ValueName";
            ddlAMAns6.DataBind();
            ListItem oli5 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns6.Items.Insert(0, oli5);
            hfPartId6.Value = dt6.Rows[0]["ParticularId"].ToString();

            ddlAMAns7.DataSource = dt7;
            ddlAMAns7.DataValueField = "AMValueId";
            ddlAMAns7.DataTextField = "ValueName";
            ddlAMAns7.DataBind();
            ListItem oli6 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns7.Items.Insert(0, oli6);
            hfPartId7.Value = dt7.Rows[0]["ParticularId"].ToString();


            ddlAMAns8.DataSource = dt8;
            ddlAMAns8.DataValueField = "AMValueId";
            ddlAMAns8.DataTextField = "ValueName";
            ddlAMAns8.DataBind();
            ListItem oli7 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns8.Items.Insert(0, oli7);
            hfPartId8.Value = dt8.Rows[0]["ParticularId"].ToString();


            ddlAMAns9.DataSource = dt9;
            ddlAMAns9.DataValueField = "AMValueId";
            ddlAMAns9.DataTextField = "ValueName";
            ddlAMAns9.DataBind();
            ListItem oli8 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns9.Items.Insert(0, oli8);
            hfPartId9.Value = dt9.Rows[0]["ParticularId"].ToString();

            ddlAMAns10.DataSource = dt10;
            ddlAMAns10.DataValueField = "AMValueId";
            ddlAMAns10.DataTextField = "ValueName";
            ddlAMAns10.DataBind();
            ListItem oli9 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns10.Items.Insert(0, oli9);
            hfPartId10.Value = dt10.Rows[0]["ParticularId"].ToString();


            ddlAMAns11.DataSource = dt11;
            ddlAMAns11.DataValueField = "AMValueId";
            ddlAMAns11.DataTextField = "ValueName";
            ddlAMAns11.DataBind();
            ListItem oli10 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns11.Items.Insert(0, oli10);
            hfPartId11.Value = dt11.Rows[0]["ParticularId"].ToString();

            ddlAMAns12.DataSource = dt12;
            ddlAMAns12.DataValueField = "AMValueId";
            ddlAMAns12.DataTextField = "ValueName";
            ddlAMAns12.DataBind();
            ListItem oli11 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns12.Items.Insert(0, oli11);
            hfPartId12.Value = dt12.Rows[0]["ParticularId"].ToString();

            ddlAMAns13.DataSource = dt13;
            ddlAMAns13.DataValueField = "AMValueId";
            ddlAMAns13.DataTextField = "ValueName";
            ddlAMAns13.DataBind();
            ListItem oli12 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns13.Items.Insert(0, oli12);
            hfPartId13.Value = dt13.Rows[0]["ParticularId"].ToString();

            ddlAMAns14.DataSource = dt14;
            ddlAMAns14.DataValueField = "AMValueId";
            ddlAMAns14.DataTextField = "ValueName";
            ddlAMAns14.DataBind();
            ListItem oli13 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns14.Items.Insert(0, oli13);
            hfPartId14.Value = dt14.Rows[0]["ParticularId"].ToString();

            ddlAMAns15.DataSource = dt15;
            ddlAMAns15.DataValueField = "AMValueId";
            ddlAMAns15.DataTextField = "ValueName";
            ddlAMAns15.DataBind();
            ListItem oli14 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns15.Items.Insert(0, oli14);
            hfPartId15.Value = dt15.Rows[0]["ParticularId"].ToString();

            ddlAMAns16.DataSource = dt16;
            ddlAMAns16.DataValueField = "AMValueId";
            ddlAMAns16.DataTextField = "ValueName";
            ddlAMAns16.DataBind();
            ListItem oli15 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns16.Items.Insert(0, oli15);
            hfPartId16.Value = dt16.Rows[0]["ParticularId"].ToString();

            ddlAMAns17.DataSource = dt17;
            ddlAMAns17.DataValueField = "AMValueId";
            ddlAMAns17.DataTextField = "ValueName";
            ddlAMAns17.DataBind();
            ListItem oli16 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns17.Items.Insert(0, oli16);
            hfPartId17.Value = dt17.Rows[0]["ParticularId"].ToString();

            ddlAMAns18.DataSource = dt18;
            ddlAMAns18.DataValueField = "AMValueId";
            ddlAMAns18.DataTextField = "ValueName";
            ddlAMAns18.DataBind();
            ListItem oli17 = new ListItem("<--Select-->", "0||0.00");
            ddlAMAns18.Items.Insert(0, oli17);
            hfPartId18.Value = dt18.Rows[0]["ParticularId"].ToString();

        }
        private void popBankDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetBankDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["BankDtl"] = dt;
                    gvBankDtl.DataSource = dt;
                    gvBankDtl.DataBind();
                }
                else
                {
                    gvBankDtl.DataSource = null;
                    gvBankDtl.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popBankIWOWDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetIWOWDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["BankIWOWDtl"] = dt;
                    gvIWOWDtl.DataSource = dt;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    gvIWOWDtl.DataSource = null;
                    gvIWOWDtl.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popITRDetails()
        {
            try
            {
                DataTable dt = null;
                dt = GetITRDetails();
                if (dt.Rows.Count > 0)
                {
                    ViewState["ITRDtl"] = dt;
                    gvITR.DataSource = dt;
                    gvITR.DataBind();
                }
                else
                {
                    gvITR.DataSource = null;
                    gvITR.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void popCBDetails()
        {
            DataTable dt = null;
            try
            {
                dt = GetCBDetails();
                ViewState["CBDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
                }
                else
                {
                    gvCBDtl.DataSource = null;
                    gvCBDtl.DataBind();
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
        private void popBSDetails()
        {
            //DataTable dt = null;
            //dt = GetBSDetails();
            //ViewState["BSDtl"] = dt;
            //gvBS.DataSource = dt;
            //gvBS.DataBind();
        }
        private void popPLDetails()
        {
            DataTable dt = null;
            try
            {
                dt = GetPLDetails();
                ViewState["PLDtl"] = dt;
                if (dt.Rows.Count > 0)
                {
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                }
                else
                {
                    gvIncome.DataSource = null;
                    gvIncome.DataBind();
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
        private DataTable GetBankDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("AccDate", typeof(string));
            dt.Columns.Add("CalDebit", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Transfer", typeof(decimal));
            dt.Columns.Add("LoanAvailed", typeof(decimal));
            dt.Columns.Add("CalCredit", typeof(string));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("SalesFig", typeof(decimal));
            dt.Columns.Add("CCLimit", typeof(decimal));
            dt.Columns.Add("Day5", typeof(decimal));
            dt.Columns.Add("Day10", typeof(decimal));
            dt.Columns.Add("Day15", typeof(decimal));
            dt.Columns.Add("Day20", typeof(decimal));
            dt.Columns.Add("Day25", typeof(decimal));


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
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[4]["SlNo"] = 5;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[5]["SlNo"] = 6;
            return dt;
        }
        private DataTable GetIWOWDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("ChqDate", typeof(string));
            dt.Columns.Add("IWReturn", typeof(decimal));
            dt.Columns.Add("OWReturn", typeof(decimal));
            dt.Columns.Add("EMIReturn", typeof(decimal));
            dt.Columns.Add("ChqClearDate", typeof(string));
            dt.Columns.Add("ChqNo", typeof(string));
            dt.Columns.Add("Reason", typeof(string));
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
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[4]["SlNo"] = 5;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[5]["SlNo"] = 6;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[6]["SlNo"] = 7;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[7]["SlNo"] = 8;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[8]["SlNo"] = 9;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[9]["SlNo"] = 10;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[10]["SlNo"] = 11;
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[11]["SlNo"] = 12;
            return dt;
        }
        private DataTable GetCBDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SLNo", typeof(int));
            // dt.Columns.Add("ApplicationId", typeof(string));//dr[ApplicationId]
            dt.Columns.Add("ReportId", typeof(string));
            dt.Columns.Add("ReportDate", typeof(string));
            dt.Columns.Add("CBNameId", typeof(int));
            dt.Columns.Add("OrgName", typeof(string));
            dt.Columns.Add("LoanType", typeof(string));
            dt.Columns.Add("Active", typeof(string));
            dt.Columns.Add("BankValName", typeof(string));
            dt.Columns.Add("LoanAmt", typeof(float));
            dt.Columns.Add("POSAmt", typeof(float));
            dt.Columns.Add("LoanDate", typeof(string));
            dt.Columns.Add("EMI", typeof(float));
            dt.Columns.Add("Tenure", typeof(int));
            dt.Columns.Add("EMIPaid", typeof(float));
            dt.Columns.Add("PMn1", typeof(string));
            dt.Columns.Add("PMn2", typeof(string));
            dt.Columns.Add("PMn3", typeof(string));
            dt.Columns.Add("PMn4", typeof(string));
            dt.Columns.Add("PMn5", typeof(string));
            dt.Columns.Add("PMn6", typeof(string));
            dt.Columns.Add("PMn7", typeof(string));
            dt.Columns.Add("Remarks", typeof(string));

            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;

        }
        private DataTable GetBSDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("BSDate", typeof(string));
            dt.Columns.Add("SrNo", typeof(int));
            dt.Columns.Add("ParticularsId", typeof(int));
            dt.Columns.Add("Particulars", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Change", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            return dt;
        }
        private DataTable GetPLDetails()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ApplicationId", typeof(string));
            dt.Columns.Add("PLDate", typeof(string));
            dt.Columns.Add("SrNo", typeof(int));
            dt.Columns.Add("ParticularsId", typeof(int));
            dt.Columns.Add("Particulars", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Change", typeof(decimal));
            dt.Columns.Add("Remarks", typeof(string));
            return dt;
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
        private DataTable GetITRDetails()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add("SlNo", typeof(int));
            dt.Columns.Add("ITRFileDate", typeof(string));
            dt.Columns.Add("FinYear", typeof(string));
            dt.Columns.Add("ITRFiledYN", typeof(string));
            dr = dt.NewRow();
            dt.Rows.Add(dr);
            dt.Rows[0]["SlNo"] = 1;
            return dt;
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
        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["BankDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvBankDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvBankDtl.Rows[curRow].FindControl("lblSlNo");
                TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDate");
                TextBox txtCalDebit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalDebit");
                TextBox txtDebit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDebit");
                TextBox txtTransfer = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtTransfer");
                TextBox txtLoanAvailed = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtLoanAvailed");
                TextBox txtCalCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalCredit");
                TextBox txtCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCredit");
                TextBox txtSalesFig = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtSalesFig");
                TextBox txtCCLimit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCCLimit");
                TextBox txtDay5 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay5");
                TextBox txtDay10 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay10");
                TextBox txtDay15 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay15");
                TextBox txtDay20 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay20");
                TextBox txtDay25 = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDay25");
                dt.Rows[curRow][0] = lblSlNo.Text;
                // dt.Rows[curRow][1] = Convert.ToDateTime(txtDate.Text).ToString("dd/MM/yyyy");
                if (txtDate.Text == "")
                {
                    //gblFuction.MsgPopup("Please Select Date to add record");
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtDate.Text);
                }

                if (txtCalDebit.Text != "")
                    dt.Rows[curRow][2] = (txtCalDebit.Text);
                else
                    dt.Rows[curRow][2] = 0;
                if (txtDebit.Text != "")
                    dt.Rows[curRow][3] = txtDebit.Text;
                else
                    dt.Rows[curRow][3] = 0;
                if (txtTransfer.Text != "")
                    dt.Rows[curRow][4] = txtTransfer.Text;
                else
                    dt.Rows[curRow][4] = 0;
                if (txtLoanAvailed.Text != "")
                    dt.Rows[curRow][5] = txtLoanAvailed.Text;
                else
                    dt.Rows[curRow][5] = 0;
                if (txtCalCredit.Text != "")
                    dt.Rows[curRow][6] = (txtCalCredit.Text);
                else
                    dt.Rows[curRow][6] = 0;
                if (txtCredit.Text != "")
                    dt.Rows[curRow][7] = txtCredit.Text;
                else
                    dt.Rows[curRow][7] = 0;
                if (txtSalesFig.Text != "")
                    dt.Rows[curRow][8] = txtSalesFig.Text;
                else
                    dt.Rows[curRow][8] = 0;

                if (txtCCLimit.Text != "")
                    dt.Rows[curRow][9] = txtCCLimit.Text;
                else
                    dt.Rows[curRow][9] = 0;

                if (txtDay5.Text != "")
                    dt.Rows[curRow][10] = txtDay5.Text;
                else
                    dt.Rows[curRow][10] = 0;
                if (txtDay10.Text != "")
                    dt.Rows[curRow][11] = txtDay10.Text;
                else
                    dt.Rows[curRow][11] = 0;
                if (txtDay15.Text != "")
                    dt.Rows[curRow][12] = txtDay15.Text;
                else
                    dt.Rows[curRow][12] = 0;

                if (txtDay20.Text != "")
                    dt.Rows[curRow][13] = txtDay20.Text;
                else
                    dt.Rows[curRow][13] = 0;
                if (txtDay25.Text != "")
                    dt.Rows[curRow][14] = txtDay25.Text;
                else
                    dt.Rows[curRow][14] = 0;
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvBankDtl.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["BankDtl"] = dt;
                gvBankDtl.DataSource = dt;
                gvBankDtl.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddIWOWRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["BankIWOWDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvIWOWDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvIWOWDtl.Rows[curRow].FindControl("lblSlNo");
                TextBox txtChqDate = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtChqDate");
                TextBox txtIWReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtIWReturn");
                TextBox txtOWReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtOWReturn");
                TextBox txtEMIReturn = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtEMIReturn");
                TextBox txtClearedDt = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtClearedDt");
                TextBox txtChequeNo = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtChequeNo");
                TextBox txtReason = (TextBox)gvIWOWDtl.Rows[curRow].FindControl("txtReason");

                dt.Rows[curRow][0] = lblSlNo.Text;
                if (txtChqDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtChqDate.Text);
                }

                if (txtIWReturn.Text != "")
                    dt.Rows[curRow][2] = (txtIWReturn.Text);
                else
                    dt.Rows[curRow][2] = 0;
                if (txtOWReturn.Text != "")
                    dt.Rows[curRow][3] = txtOWReturn.Text;
                else
                    dt.Rows[curRow][3] = 0;
                if (txtEMIReturn.Text != "")
                    dt.Rows[curRow][4] = txtEMIReturn.Text;
                else
                    dt.Rows[curRow][4] = 0;
                if (txtClearedDt.Text != "")
                    dt.Rows[curRow][5] = txtClearedDt.Text;
                else
                    dt.Rows[curRow][5] = null;

                if (txtChequeNo.Text != "")
                    dt.Rows[curRow][6] = txtChequeNo.Text;
                else
                    dt.Rows[curRow][6] = "";
                if (txtReason.Text != "")
                    dt.Rows[curRow][7] = (txtReason.Text);
                else
                    dt.Rows[curRow][7] = "";

                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvIWOWDtl.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["BankIWOWDtl"] = dt;
                gvIWOWDtl.DataSource = dt;
                gvIWOWDtl.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnAddITR_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["ITRDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvITR.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSlNo = (Label)gvITR.Rows[curRow].FindControl("lblSlNo");
                TextBox txtITRFileDate = (TextBox)gvITR.Rows[curRow].FindControl("txtITRFileDate");
                TextBox txtFinYear = (TextBox)gvITR.Rows[curRow].FindControl("txtFinYear");
                Label lblITRFiledYN = (Label)gvITR.Rows[curRow].FindControl("lblITRFiledYN");
                CheckBox chkITRFile = (CheckBox)gvITR.Rows[curRow].FindControl("chkITRFile");

                dt.Rows[curRow][0] = lblSlNo.Text;
                if (txtITRFileDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please Select Date to add record");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtITRFileDate.Text);
                }

                if (txtFinYear.Text != "")
                    dt.Rows[curRow][2] = (txtFinYear.Text);
                else
                {
                    gblFuction.AjxMsgPopup("Please Give Input in Financial Year...");
                    return;
                }
                if (chkITRFile.Checked == true)
                    dt.Rows[curRow][3] = "Y";
                else
                    dt.Rows[curRow][3] = "N";
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SlNo"] = Convert.ToInt32(((Label)gvITR.Rows[vRow].FindControl("lblSlNo")).Text) + 1;
                dt.AcceptChanges();

                ViewState["ITRDtl"] = dt;
                gvITR.DataSource = dt;
                gvITR.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
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
        protected void btnAddCBRow_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)ViewState["CBDtl"];
                int curRow = 0, maxRow = 0, vRow = 0;
                Button txtCur = (Button)sender;
                GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
                curRow = gR.RowIndex;
                maxRow = gvBankDtl.Rows.Count;
                vRow = dt.Rows.Count - 1;
                DataRow dr;
                Label lblSLNoCB = (Label)gvCBDtl.Rows[curRow].FindControl("lblSLNoCB");
                TextBox txtReportId = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtReportId");
                TextBox txtReportDate = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtReportDate");
                Label lblCBNameId = (Label)gvCBDtl.Rows[curRow].FindControl("lblCBNameId");
                DropDownList ddlCBname = (DropDownList)gvCBDtl.Rows[curRow].FindControl("ddlCBname");
                TextBox txtOrgNameCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtOrgNameCB");
                TextBox txtLoanTypeCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanTypeCB");
                Label lblIsActive = (Label)gvCBDtl.Rows[curRow].FindControl("lblIsActive");
                CheckBox chkCBIsActive = (CheckBox)gvCBDtl.Rows[curRow].FindControl("chkCBIsActive");
                TextBox txtBankValName = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtBankValName");
                TextBox txtLoanAmtCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanAmtCB");
                TextBox txtPOSAmt = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPOSAmt");
                TextBox txtLoanDateCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtLoanDateCB");
                TextBox txtEMICB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtEMICB");
                TextBox txtTenureCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtTenureCB");
                TextBox txtEMIPaidCB = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtEMIPaidCB");
                TextBox txtPMn1 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn1");
                TextBox txtPMn2 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn2");
                TextBox txtPMn3 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn3");
                TextBox txtPMn4 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn4");
                TextBox txtPMn5 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn5");
                TextBox txtPMn6 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn6");
                TextBox txtPMn7 = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtPMn7");
                TextBox txtRemarks = (TextBox)gvCBDtl.Rows[curRow].FindControl("txtRemarks");

                dt.Rows[curRow][0] = lblSLNoCB.Text;
                if (txtReportId.Text == "")
                {
                    gblFuction.AjxMsgPopup("Please input Unique Report Id");
                    return;
                }
                else
                {
                    dt.Rows[curRow][1] = (txtReportId.Text);
                }
                if (txtReportDate.Text != "")
                    dt.Rows[curRow][2] = (txtReportDate.Text);
                else
                    dt.Rows[curRow][2] = null;
                if (ddlCBname.SelectedValue != "-1")
                    dt.Rows[curRow][3] = ddlCBname.SelectedValue.ToString();
                else
                    dt.Rows[curRow][3] = "-1";
                if (txtOrgNameCB.Text != "")
                    dt.Rows[curRow][4] = (txtOrgNameCB.Text);
                else
                    dt.Rows[curRow][4] = " ";
                if (txtLoanTypeCB.Text != "")
                    dt.Rows[curRow][5] = txtLoanTypeCB.Text;
                else
                    dt.Rows[curRow][5] = "";
                if (chkCBIsActive.Checked == true)
                    dt.Rows[curRow][6] = "Y";
                else
                    dt.Rows[curRow][6] = "N";
                if (txtBankValName.Text != "")
                    dt.Rows[curRow][7] = txtBankValName.Text;
                else
                    dt.Rows[curRow][7] = "";

                if (txtLoanAmtCB.Text != "")
                    dt.Rows[curRow][8] = txtLoanAmtCB.Text;
                else
                {
                    gblFuction.AjxMsgPopup("Please input Loan Amount..");
                    return;
                }

                if (txtPOSAmt.Text != "")
                    dt.Rows[curRow][9] = txtPOSAmt.Text;
                else
                    dt.Rows[curRow][9] = 0;

                if (txtLoanDateCB.Text != "")
                    dt.Rows[curRow][10] = txtLoanDateCB.Text;
                else
                {
                    gblFuction.AjxMsgPopup("Please input Loan Date");
                    return;
                }
                if (txtEMICB.Text != "")
                    dt.Rows[curRow][11] = txtEMICB.Text;
                else
                    dt.Rows[curRow][11] = 0;
                if (txtTenureCB.Text != "")
                    dt.Rows[curRow][12] = txtTenureCB.Text;
                else
                    dt.Rows[curRow][12] = 0;
                if (txtEMIPaidCB.Text != "")
                    dt.Rows[curRow][13] = txtEMIPaidCB.Text;
                else
                    dt.Rows[curRow][13] = 0;
                if (txtPMn1.Text != "")
                    dt.Rows[curRow][14] = txtPMn1.Text;
                else
                    dt.Rows[curRow][14] = "";
                if (txtPMn2.Text != "")
                    dt.Rows[curRow][15] = txtPMn2.Text;
                else
                    dt.Rows[curRow][15] = "";
                if (txtPMn3.Text != "")
                    dt.Rows[curRow][16] = txtPMn3.Text;
                else
                    dt.Rows[curRow][16] = "";
                if (txtPMn4.Text != "")
                    dt.Rows[curRow][17] = txtPMn4.Text;
                else
                    dt.Rows[curRow][17] = "";
                if (txtPMn5.Text != "")
                    dt.Rows[curRow][18] = txtPMn5.Text;
                else
                    dt.Rows[curRow][18] = "";
                if (txtPMn6.Text != "")
                    dt.Rows[curRow][19] = txtPMn6.Text;
                else
                    dt.Rows[curRow][19] = "";
                if (txtPMn7.Text != "")
                    dt.Rows[curRow][20] = txtPMn7.Text;
                else
                    dt.Rows[curRow][20] = "";
                if (txtRemarks.Text != "")
                    dt.Rows[curRow][21] = txtRemarks.Text;
                else
                    dt.Rows[curRow][21] = "";
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dt.Rows[vRow + 1]["SLNo"] = Convert.ToInt32(((Label)gvCBDtl.Rows[vRow].FindControl("lblSLNoCB")).Text) + 1;
                dt.AcceptChanges();

                ViewState["CBDtl"] = dt;
                gvCBDtl.DataSource = dt;
                gvCBDtl.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void txtCalDebit_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtCalDebits = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalDebit");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;


            TextBox txtDbt = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDebit");

            string s = txtCalDebits.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                i = i + Convert.ToDouble(debit);

            }
            txtDbt.Text = i.ToString("0.00");

        }
        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;

            // TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtDate");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
            if (curRow == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    TextBox txtDate = (TextBox)gvBankDtl.Rows[curRow + i].FindControl("txtDate");
                    DateTime CurrentDate = gblFuction.setDate(txtDate.Text);
                    TextBox txtNewDate = (TextBox)gvBankDtl.Rows[curRow + i + 1].FindControl("txtDate");
                    DateTime NewCurrentDate = CurrentDate.AddMonths(1);
                    txtNewDate.Text = NewCurrentDate.ToString("dd/MM/yyyy");
                }
            }
        }
        protected void txtCalCredit_TextChanged(object sender, EventArgs e)
        {
            int curRow = 0, maxRow = 0, vRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtCalCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCalCredit");
            //GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;


            TextBox txtCredit = (TextBox)gvBankDtl.Rows[curRow].FindControl("txtCredit");

            string s = txtCalCredit.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                i = i + Convert.ToDouble(debit);

            }
            txtCredit.Text = i.ToString("0.00");

        }
        protected void txtAmt_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtAmt = (TextBox)gvLiability.Rows[curRow].FindControl("txtAmt");

            string s = txtAmt.Text;
            // s=s.Replace("-", "+");
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);
            }
            txtAmt.Text = i.ToString("0.00");


            //string s1 = txtAmt.Text;
            //string[] debits1 = s.Split('-');
            //double m = 0;
            //foreach (string debit in debits)
            //{
            //    if (debit != "")
            //        m = m - Convert.ToDouble(debit);
            //}
            //txtAmt.Text = i.ToString("0.00");


            // modify total  liability section

            int a = gvLiability.Rows.Count;
            double liabilty = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvLiability.Rows[x].FindControl("txtAmt");
                string minus = txt.ToString().Substring(0, 1);
                Label id = (Label)gvLiability.Rows[x].FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (p == 5 || p == 6)
                {
                    if (txt.Text != "")
                        liabilty = liabilty - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text != "")
                        liabilty = liabilty + Convert.ToDouble(txt.Text);
                }
            }
            txtTotLiability.Text = liabilty.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtAmt").Focus();
        }
        protected void txtAssetAmt_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtAssetAmt = (TextBox)gvAsset.Rows[curRow].FindControl("txtAssetAmt");

            string s = txtAssetAmt.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtAssetAmt.Text = i.ToString("0.00");



            // modify total asset section

            int a = gvAsset.Rows.Count;
            double asset = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvAsset.Rows[x].FindControl("txtAssetAmt");
                Label id = (Label)gvAsset.Rows[x].FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (txt.Text == "")
                    asset = asset + 0;
                else
                    asset = asset + Convert.ToDouble(txt.Text);

            }
            txtTotAsset.Text = asset.ToString("0.00");

            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtAssetAmt").Focus();
        }
        protected void txtIncome_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtIncome = (TextBox)gvIncome.Rows[curRow].FindControl("txtIncome");

            string s = txtIncome.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtIncome.Text = i.ToString("0.00");

            // modify total income section
            int a = gvIncome.Rows.Count;

            double income = 0;

            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvIncome.Rows[x].FindControl("txtIncome");
                Label id = (Label)gvIncome.Rows[x].FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (p == 8)
                {
                    if (txt.Text != "")
                        income = income - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        income = income + 0;
                    else
                        income = income + Convert.ToDouble(txt.Text);
                }
            }
            txtTotIncome.Text = income.ToString("0.00");
            double expense = 0;
            if (txtTotExpense.Text != "")
                expense = Convert.ToDouble(txtTotExpense.Text);
            double netprofit = 0;
            netprofit = (income - expense);
            txtNetProfit.Text = netprofit.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtIncome").Focus();
        }
        protected void txtExpense_TextChaged(object sender, EventArgs e)
        {
            // total calulation part

            int curRow = 0;
            TextBox txtCur = (TextBox)sender;
            GridViewRow gR = (GridViewRow)txtCur.NamingContainer;
            curRow = gR.RowIndex;
            TextBox txtExpense = (TextBox)gvExpense.Rows[curRow].FindControl("txtExpense");

            string s = txtExpense.Text;
            string[] debits = s.Split('+');
            double i = 0;
            foreach (string debit in debits)
            {
                if (debit != "")
                    i = i + Convert.ToDouble(debit);

            }
            txtExpense.Text = i.ToString("0.00");

            // calcualtion of total expense
            int a = gvExpense.Rows.Count;
            double expense = 0;
            for (int x = 0; x < a; x++)
            {
                TextBox txt = (TextBox)gvExpense.Rows[x].FindControl("txtExpense");
                Label id = (Label)gvExpense.Rows[x].FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (txt.Text == "")
                    expense = expense + 0;
                else
                    expense = expense + Convert.ToDouble(txt.Text);
            }
            double income = 0;
            income = Convert.ToDouble(txtTotIncome.Text);
            txtTotExpense.Text = expense.ToString("0.00");
            double netprofit = 0;
            netprofit = (income - expense);
            txtNetProfit.Text = netprofit.ToString("0.00");
            GridViewRow myRow = ((Control)sender).Parent.Parent as GridViewRow;
            myRow.FindControl("txtExpense").Focus();
        }
        protected void ImDel1_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;

            try
            {
                DataRow dr = null;
                DataTable dtXml = new DataTable();
                dtXml.Columns.Add(new DataColumn("SlNo"));
                dtXml.Columns.Add(new DataColumn("AccDate"));
                dtXml.Columns.Add(new DataColumn("CalDebit"));
                dtXml.Columns.Add(new DataColumn("Debit"));
                dtXml.Columns.Add(new DataColumn("Transfer"));
                dtXml.Columns.Add(new DataColumn("LoanAvailed"));
                dtXml.Columns.Add(new DataColumn("CalCredit"));
                dtXml.Columns.Add(new DataColumn("Credit"));
                dtXml.Columns.Add(new DataColumn("SalesFig"));
                dtXml.Columns.Add(new DataColumn("CCLimit"));
                dtXml.Columns.Add(new DataColumn("Day5"));
                dtXml.Columns.Add(new DataColumn("Day10"));
                dtXml.Columns.Add(new DataColumn("Day15"));
                dtXml.Columns.Add(new DataColumn("Day20"));
                dtXml.Columns.Add(new DataColumn("Day25"));
                foreach (GridViewRow gr in gvBankDtl.Rows)
                {
                    if (((TextBox)gr.FindControl("txtDate")).Text != "")
                    {
                        dr = dtXml.NewRow();
                        dr["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;

                        if (((TextBox)gr.FindControl("txtDate")).Text != "")
                            dr["AccDate"] = ((TextBox)gr.FindControl("txtDate")).Text;
                        else
                        {
                            dr["AccDate"] = DBNull.Value;
                        }
                        dr["CalDebit"] = ((TextBox)gr.FindControl("txtCalDebit")).Text;
                        if (((TextBox)gr.FindControl("txtDebit")).Text != "")
                            dr["Debit"] = ((TextBox)gr.FindControl("txtDebit")).Text;
                        else
                            dr["Debit"] = 0;
                        if (((TextBox)gr.FindControl("txtTransfer")).Text != "")
                            dr["Transfer"] = ((TextBox)gr.FindControl("txtTransfer")).Text;
                        else
                            dr["Transfer"] = 0;
                        if (((TextBox)gr.FindControl("txtLoanAvailed")).Text != "")
                            dr["LoanAvailed"] = ((TextBox)gr.FindControl("txtLoanAvailed")).Text;
                        else
                            dr["LoanAvailed"] = 0;
                        dr["CalCredit"] = ((TextBox)gr.FindControl("txtCalCredit")).Text;
                        if (((TextBox)gr.FindControl("txtCredit")).Text != "")
                            dr["Credit"] = ((TextBox)gr.FindControl("txtCredit")).Text;
                        else
                            dr["Credit"] = 0;
                        if (((TextBox)gr.FindControl("txtSalesFig")).Text != "")
                            dr["SalesFig"] = ((TextBox)gr.FindControl("txtSalesFig")).Text;
                        else
                            dr["SalesFig"] = 0;
                        if (((TextBox)gr.FindControl("txtCCLimit")).Text != "")
                            dr["CCLimit"] = ((TextBox)gr.FindControl("txtCCLimit")).Text;
                        else
                            dr["CCLimit"] = 0;
                        if (((TextBox)gr.FindControl("txtDay5")).Text != "")
                            dr["Day5"] = ((TextBox)gr.FindControl("txtDay5")).Text;
                        else
                            dr["Day5"] = 0;
                        if (((TextBox)gr.FindControl("txtDay10")).Text != "")
                            dr["Day10"] = ((TextBox)gr.FindControl("txtDay10")).Text;
                        else
                            dr["Day10"] = 0;
                        if (((TextBox)gr.FindControl("txtDay15")).Text != "")
                            dr["Day15"] = ((TextBox)gr.FindControl("txtDay15")).Text;
                        else
                            dr["Day15"] = 0;
                        if (((TextBox)gr.FindControl("txtDay20")).Text != "")
                            dr["Day20"] = ((TextBox)gr.FindControl("txtDay20")).Text;
                        else
                            dr["Day20"] = 0;
                        if (((TextBox)gr.FindControl("txtDay25")).Text != "")
                            dr["Day25"] = ((TextBox)gr.FindControl("txtDay25")).Text;
                        else
                            dr["Day25"] = 0;

                        dtXml.Rows.Add(dr);
                        dtXml.AcceptChanges();
                    }
                }
                ViewState["BankDtl"] = dtXml;
                dt = (DataTable)ViewState["BankDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["BankDtl"] = dt;
                    gvBankDtl.DataSource = dt;
                    gvBankDtl.DataBind();
                }
                else
                {
                    popBankDetails();
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
        protected void ImDelIWOW_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                DataRow dr1 = null;
                DataTable dtXml1 = new DataTable();
                dtXml1.Columns.Add(new DataColumn("SlNo"));
                dtXml1.Columns.Add(new DataColumn("ChqDate"));
                dtXml1.Columns.Add(new DataColumn("IWReturn"));
                dtXml1.Columns.Add(new DataColumn("OWReturn"));
                dtXml1.Columns.Add(new DataColumn("EMIReturn"));
                dtXml1.Columns.Add(new DataColumn("ChqClearDate"));
                dtXml1.Columns.Add(new DataColumn("ChqNo"));
                dtXml1.Columns.Add(new DataColumn("Reason"));

                foreach (GridViewRow gr in gvIWOWDtl.Rows)
                {
                    //if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                    //{
                    dr1 = dtXml1.NewRow();
                    dr1["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;
                    if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                        dr1["ChqDate"] = ((TextBox)gr.FindControl("txtChqDate")).Text;
                    else
                    {
                        dr1["ChqDate"] = DBNull.Value;
                    }
                    if (((TextBox)gr.FindControl("txtIWReturn")).Text != "")
                        dr1["IWReturn"] = ((TextBox)gr.FindControl("txtIWReturn")).Text;
                    else
                        dr1["IWReturn"] = 0;
                    if (((TextBox)gr.FindControl("txtOWReturn")).Text != "")
                        dr1["OWReturn"] = ((TextBox)gr.FindControl("txtOWReturn")).Text;
                    else
                        dr1["OWReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtEMIReturn")).Text != "")
                        dr1["EMIReturn"] = ((TextBox)gr.FindControl("txtEMIReturn")).Text;
                    else
                        dr1["EMIReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtClearedDt")).Text != "")
                        dr1["ChqClearDate"] = ((TextBox)gr.FindControl("txtClearedDt")).Text;
                    else
                        dr1["ChqClearDate"] = DBNull.Value;
                    if (((TextBox)gr.FindControl("txtChequeNo")).Text != "")
                        dr1["ChqNo"] = ((TextBox)gr.FindControl("txtChequeNo")).Text;
                    else
                        dr1["ChqNo"] = "";
                    if (((TextBox)gr.FindControl("txtReason")).Text != "")
                        dr1["Reason"] = ((TextBox)gr.FindControl("txtReason")).Text;
                    else
                        dr1["Reason"] = "";
                    dtXml1.Rows.Add(dr1);
                    dtXml1.AcceptChanges();
                    // }
                }
                ViewState["BankIWOWDtl"] = dtXml1;
                dt = (DataTable)ViewState["BankIWOWDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["BankIWOWDtl"] = dt;
                    gvIWOWDtl.DataSource = dt;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    popBankIWOWDetails();
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
        protected void ImDelITR_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["ITRDtl"];
                if (dt.Rows.Count > 1)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["ITRDtl"] = dt;
                    gvITR.DataSource = dt;
                    gvITR.DataBind();
                }
                else
                {
                    popITRDetails();
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
        protected void ImDelCB_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["CBDtl"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["CBDtl"] = dt;
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
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
        protected void ImDelPL_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ImageButton btnDel = (ImageButton)sender;
            GridViewRow gR = (GridViewRow)btnDel.NamingContainer;
            try
            {
                dt = (DataTable)ViewState["PLDtl"];
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[gR.RowIndex].Delete();
                    dt.AcceptChanges();
                    ViewState["PLDtl"] = dt;
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
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
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadCompanyList();
        }
        private void LoadCBList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvCB.DataSource = dt1;
                        gvCB.DataBind();
                    }
                    else
                    {
                        gvCB.DataSource = null;
                        gvCB.DataBind();
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
        private void LoadBankAcList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBankACList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankAcList.DataSource = dt1;
                        gvBankAcList.DataBind();
                    }
                    else
                    {
                        gvBankAcList.DataSource = null;
                        gvBankAcList.DataBind();
                    }
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
                oMem = null;
            }

        }
        private void LoadBankAcSummary(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBankACSummary(pApplicationId);

                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            for (int j = 0; j < dt1.Columns.Count; j++)
                            {
                                if (string.IsNullOrEmpty(dt1.Rows[i][j].ToString()))
                                {
                                    dt1.Rows[i][j] = "0";
                                }
                            }
                        }
                        Session["AccountsDt"] = dt1;
                        gvBankAcSummary.DataSource = dt1;
                        gvBankAcSummary.DataBind();
                    }
                    else
                    {
                        gvBankAcSummary.DataSource = null;
                        gvBankAcSummary.DataBind();
                    }
                }
                else
                {
                    gvBankAcSummary.DataSource = null;
                    gvBankAcSummary.DataBind();
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
                oMem = null;
            }

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
        private void LoadBSList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetBalanceSheetList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvBSList.DataSource = dt1;
                        gvBSList.DataBind();
                    }
                    else
                    {
                        gvBSList.DataSource = null;
                        gvBSList.DataBind();
                    }
                }
                else
                {
                    gvBSList.DataSource = null;
                    gvBSList.DataBind();
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
                oMem = null;
            }

        }
        private void LoadFinStatDoc(string pApplicationId)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetFinStatDocUp(pApplicationId);
                if (dt.Rows.Count > 0)
                {
                    gvBSPLDoc.DataSource = dt;
                    gvBSPLDoc.DataBind();
                }
                else
                {
                    gvBSPLDoc.DataSource = null;
                    gvBSPLDoc.DataBind();
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
        private void LoadITRFile(string pApplicationId)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                dt = oMem.GetITRFile(pApplicationId);
                if (dt.Rows.Count > 0)
                {
                    gvViewITR.DataSource = dt;
                    gvViewITR.DataBind();
                    ViewState["ITRDtl"] = dt;
                }
                else
                {
                    gvViewITR.DataSource = null;
                    gvViewITR.DataBind();
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
        private void LoadPLList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetPLList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvPLList.DataSource = dt1;
                        gvPLList.DataBind();
                    }
                    else
                    {
                        gvPLList.DataSource = null;
                        gvPLList.DataBind();
                    }
                }
                else
                {
                    gvPLList.DataSource = null;
                    gvPLList.DataBind();
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
                oMem = null;
            }

        }
        private void LoadCAMList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCAMList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        gvCAM.DataSource = dt1;
                        gvCAM.DataBind();
                    }
                    else
                    {
                        gvCAM.DataSource = null;
                        gvCAM.DataBind();
                    }

                }
                else
                {
                    gvCAM.DataSource = null;
                    gvCAM.DataBind();
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
        private void LoadRatioData(string pApplicationId)
        {
            GetRatio(pApplicationId);
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
                    //lblAppAdmDate.Text = Convert.ToString(dt1.Rows[0]["DOA"]);
                    //lblAppDOB.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                    ////lblAppDOB
                    //lblCRoName.Text = Convert.ToString(dt1.Rows[0]["EoName"]).Trim();
                    //lblAppSalu.Text = Convert.ToString(dt1.Rows[0]["SalutationName"]).Trim();
                    //lblAppFName.Text = Convert.ToString(dt1.Rows[0]["FName"]).Trim();
                    //lblAppMName.Text = Convert.ToString(dt1.Rows[0]["MName"]).Trim();
                    //lblAppLName.Text = Convert.ToString(dt1.Rows[0]["LName"]).Trim();

                    //lblAppName.Text = Convert.ToString(dt1.Rows[0]["CustName"]).Trim();
                    //lblApplicanNo.Text = Convert.ToString(dt1.Rows[0]["CustNo"]).Trim();

                    //lblAppPhotoIdType.Text = Convert.ToString(dt1.Rows[0]["PhotoID"]).Trim();
                    //lblAppPhotoIdNo.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim();
                    //lblAppAddresIdType.Text = Convert.ToString(dt1.Rows[0]["AddressID"]).Trim();
                    //lblAppAddresIdNo.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim();
                    //lblAppITTypeId.Text = Convert.ToString(dt1.Rows[0]["ITProofID"]).Trim();
                    //lblAppITProfNo.Text = Convert.ToString(dt1.Rows[0]["ITTypeNo"]).Trim();
                    //lblAppTelNo.Text = Convert.ToString(dt1.Rows[0]["ContTelNo"]).Trim();



                    //lblAppMaritalStat.Text = Convert.ToString(dt1.Rows[0]["MaritalName"]).Trim();
                    //lblAppMobNo.Text = Convert.ToString(dt1.Rows[0]["ContMNo"]).Trim();
                    //lblAppOccu.Text = Convert.ToString(dt1.Rows[0]["Occupation"]).Trim();
                    //lblAppFaxNo.Text = Convert.ToString(dt1.Rows[0]["ContFAXNo"]).Trim();
                    //lblAppEmail.Text = Convert.ToString(dt1.Rows[0]["Email"]).Trim();
                    //lblAppGen.Text = Convert.ToString(dt1.Rows[0]["GenderName"]).Trim();
                    //lblAppAge.Text = Convert.ToString(dt1.Rows[0]["Age"]).Trim();
                    //lblAppDOB.Text = Convert.ToString(dt1.Rows[0]["DOB"]);
                    //lblAppQualification.Text = Convert.ToString(dt1.Rows[0]["Qualification"]).Trim();
                    //lblAppReligion.Text = Convert.ToString(dt1.Rows[0]["Religion"]).Trim();
                    //lblAppCast.Text = Convert.ToString(dt1.Rows[0]["Caste"]).Trim();

                    //lblAppYrAtRes.Text = Convert.ToString(dt1.Rows[0]["YearAtRes"]).Trim();
                    //lblAppYrInBus.Text = Convert.ToString(dt1.Rows[0]["YearAtBus"]).Trim();

                    //if (dt1.Rows[0]["IsDirector"].ToString() == "Y")
                    //{
                    //    rlbShowApp.SelectedValue = "D";
                    //}
                    //else if (dt1.Rows[0]["IsPartner"].ToString() == "Y")
                    //{
                    //    rlbShowApp.SelectedValue = "P";
                    //}
                    //else if (dt1.Rows[0]["IsIndividual"].ToString() == "Y")
                    //{
                    //    rlbShowApp.SelectedValue = "I";
                    //}
                    //if (dt1.Rows[0]["IsGPOAAccHolder"].ToString() == "Y")
                    //{
                    //    chkShowGPOAHold.Checked = true;
                    //}

                    //lblAppAdd1Pre.Text = Convert.ToString(dt1.Rows[0]["PreAddress1"]).Trim();
                    //lblAppAdd2Pre.Text = Convert.ToString(dt1.Rows[0]["PreAddress2"]).Trim();
                    //lblAppStatePre.Text = Convert.ToString(dt1.Rows[0]["PreState"]).Trim();
                    //lblAppDistPre.Text = Convert.ToString(dt1.Rows[0]["PreDistrict"]).Trim();
                    //lblAppPINPre.Text = Convert.ToString(dt1.Rows[0]["PrePIN"]).Trim();

                    //lblAppAdd1Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress1"]).Trim();
                    //lblAppAdd2Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress2"]).Trim();
                    //lblAppStatePer.Text = Convert.ToString(dt1.Rows[0]["PerDistrict"]).Trim();
                    //lblAppDistPer.Text = Convert.ToString(dt1.Rows[0]["PerState"]).Trim();
                    //lblAppPINPer.Text = Convert.ToString(dt1.Rows[0]["PerPIN"]).Trim();
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

                // get CB Details
                LoadCBList(vLoanAppId);
                //get Bank Statement Details
                LoadBankAcList(vLoanAppId);
                // get balance sheet details
                //LoadBSDetails(vLoanAppId);
                // get balance sheet list
                LoadBSList(vLoanAppId);
                // get profit and loss details
                //LoadPLDetails(vLoanAppId);
                // get profit and loss list
                LoadPLList(vLoanAppId);

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
            DataTable dt1 = null, dt2 = null;
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
                        tbMem.ActiveTabIndex = 1;
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
                        txtAppAddIdNoForShow.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]).Trim();
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
                        txtAppPhotoIdNoForShow.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]).Trim();
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
                    txtCoAppPNo.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    ddlCoAppAddressIdType.SelectedIndex = ddlCoAppAddressIdType.Items.IndexOf(ddlCoAppAddressIdType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AddressId"])));
                    txtCoAppAddressIdNo.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    //ddlCoAppITTypeId.SelectedIndex = ddlCoAppITTypeId.Items.IndexOf(ddlCoAppITTypeId.Items.FindByValue(Convert.ToString(dt1.Rows[0]["ITTypeId"])));
                    //txtCoAppITNo.Text = Convert.ToString(dt1.Rows[0]["ITTypeNo"]);
                    txtCoAppTelNo.Text = Convert.ToString(dt1.Rows[0]["ContTelNo"]);
                    PopMaritalStatus();
                    ddlCoAppMS.SelectedIndex = ddlCoAppMS.Items.IndexOf(ddlCoAppMS.Items.FindByValue(Convert.ToString(dt1.Rows[0]["MaritalStatus"])));
                    txtCoAppMNo.Text = Convert.ToString(dt1.Rows[0]["ContMNo"]);
                    PopOccupation();
                    ddlCoAppOccu.SelectedIndex = ddlCoAppOccu.Items.IndexOf(ddlCoAppOccu.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId"])));
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
                    ddlCustCARel.SelectedIndex = ddlCoAppRel.Items.IndexOf(ddlCoAppRel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["CustCoAppRel"])));
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
                    txtCoAppStatePre.Text = Convert.ToString(dt1.Rows[0]["PreState"]).Trim();
                    txtCoAppDistPre.Text = Convert.ToString(dt1.Rows[0]["PreDistrict"]).Trim();
                    txtCOAppPINPre.Text = Convert.ToString(dt1.Rows[0]["PrePIN"]).Trim();
                    txtCoAppAddress1Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress1"]).Trim();
                    txtCoAppAddress2Per.Text = Convert.ToString(dt1.Rows[0]["PerAddress2"]).Trim();
                    txtCoAppDistPer.Text = Convert.ToString(dt1.Rows[0]["PerDistrict"]).Trim();
                    txtCoAppStatePer.Text = Convert.ToString(dt1.Rows[0]["PerState"]).Trim();
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
                    PopQualification();
                    PopReligion();
                    PopCaste();
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
                    txtComAddIDNo.Text = Convert.ToString(dt1.Rows[0]["AddressIdNo"]);
                    txtCustAnnualInc.Text = Convert.ToString(dt1.Rows[0]["AnnualIncome"]);
                    ddlCustPhotoTypeId.SelectedIndex = ddlCustPhotoTypeId.Items.IndexOf(ddlCustPhotoTypeId.Items.FindByValue(Convert.ToString(dt1.Rows[0]["PhotoId"])));
                    txtCustPhotIDNo.Text = Convert.ToString(dt1.Rows[0]["PhotoIdNo"]);
                    PopRelation();
                    ddlCustRel.SelectedIndex = ddlCustRel.Items.IndexOf(ddlCustRel.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelationId"])));
                    txtCustRelName.Text = Convert.ToString(dt1.Rows[0]["RelativeName"]);
                    PopBankAcType();
                    txtAppName.Text = Convert.ToString(dt1.Rows[0]["ApplicantName"]);
                    txtAppContNo.Text = Convert.ToString(dt1.Rows[0]["AppContactNo"]);

                    if (dt1.Rows[0]["IsRegistered"].ToString() == "Y")
                    {
                        chkComRegCer.Checked = true;

                    }
                    else
                    {
                        chkComRegCer.Checked = false;
                    }
                    txtComRegisNo.Text = dt1.Rows[0]["RegistrationNo"].ToString();
                    txtMAdd1.Text = Convert.ToString(dt1.Rows[0]["MAddress1"]).Trim();
                    txtMAdd2.Text = Convert.ToString(dt1.Rows[0]["MAddress2"]).Trim();
                    txtMState.Text = Convert.ToString(dt1.Rows[0]["MState"]).Trim();
                    txtMDist.Text = Convert.ToString(dt1.Rows[0]["MDistrict"]).Trim();
                    txtMPIN.Text = Convert.ToString(dt1.Rows[0]["MPIN"]).Trim();
                    txtMMobNo.Text = Convert.ToString(dt1.Rows[0]["MMobNo"]).Trim();
                    txtMSTD.Text = Convert.ToString(dt1.Rows[0]["MSTD"]).Trim();
                    txtMTel.Text = Convert.ToString(dt1.Rows[0]["MTelNo"]).Trim();
                    txtGSTRegNo.Text = dt1.Rows[0]["GSTRegNo"].ToString();

                    if (dt1.Rows[0]["SameAddYN"].ToString() == "Y")
                    {
                        chkComSameAdd.Checked = true;
                        txtComRegisNo.Enabled = true;
                    }
                    else
                    {
                        chkComSameAdd.Checked = false;
                        txtComRegisNo.Enabled = false;
                    }

                    txtROffAdd1.Text = Convert.ToString(dt1.Rows[0]["RAddress1"]).Trim();
                    txtROffAdd2.Text = Convert.ToString(dt1.Rows[0]["RAddress2"]).Trim();
                    txtRState.Text = Convert.ToString(dt1.Rows[0]["RState"]).Trim();
                    txtRDist.Text = Convert.ToString(dt1.Rows[0]["RDistrict"]).Trim();
                    txtRPIN.Text = Convert.ToString(dt1.Rows[0]["RPIN"]).Trim();
                    txtRMobNo.Text = Convert.ToString(dt1.Rows[0]["RMobNo"]).Trim();
                    txtRSTD.Text = Convert.ToString(dt1.Rows[0]["RSTD"]).Trim();
                    txtRTel.Text = Convert.ToString(dt1.Rows[0]["RTelNo"]).Trim();
                    PopBusinessType();
                    ddlCustBusType.SelectedIndex = ddlCustBusType.Items.IndexOf(ddlCustBusType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["BusinessTypeId"])));
                    txtCustBusLocation.Text = Convert.ToString(dt1.Rows[0]["BusinessLocation"]);
                    PopOccupation();
                    ddlCustOccupation.SelectedIndex = ddlCustOccupation.Items.IndexOf(ddlCustOccupation.Items.FindByValue(Convert.ToString(dt1.Rows[0]["OccupationId"])));
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
                    txtCustAcYrOfOpen.Text = Convert.ToString(dt1.Rows[0]["YrOfOpening"]);
                    ddlCustACType.SelectedIndex = ddlCustACType.Items.IndexOf(ddlCustACType.Items.FindByValue(Convert.ToString(dt1.Rows[0]["AccountType"])));
                    txtCustContNo.Text = Convert.ToString(dt1.Rows[0]["ContactNo"]);

                    //EMPLOYMENT DETAILS
                    txtCustOrgname.Text = Convert.ToString(dt1.Rows[0]["EmpOrgName"]);
                    txtCustDesig.Text = Convert.ToString(dt1.Rows[0]["EmpDesig"]);
                    txtCustRetiredAge.Text = Convert.ToString(dt1.Rows[0]["EmpRetiredAge"]);
                    txtCustDeptEmpCode.Text = Convert.ToString(dt1.Rows[0]["EmpCode"]);
                    txtCustCurExp.Text = Convert.ToString(dt1.Rows[0]["EmpCurExp"]);
                    txtCustTotExp.Text = Convert.ToString(dt1.Rows[0]["EmpTotExp"]);

                    //BUSINESS DETAILS
                    if (dt1.Rows[0]["BusGSTAppYN"].ToString() == "Y")
                        chkGSTApp.Checked = true;
                    else
                        chkGSTApp.Checked = false;
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

        private void ShowAllInitialLoanApp(string vCustId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataTable dt = new DataTable();
            dt = ca.GetAllInitialLoanApp(vCustId);
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
        private void ShowInitialLoanAppData(string pLnAppId, string vBrCode)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CApplication ca = new CApplication();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            try
            {
                ds = ca.GetInitLoanDtlByLoanId(pLnAppId, vBrCode);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];

                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 1;
                    ViewAcess();
                    hdfApplicationId.Value = pLnAppId;
                    btSaveApplication.Enabled = false;
                    btnUpdateApplication.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppNo.Text = Convert.ToString(dt.Rows[0]["LoanAppNo"]).Trim();
                    txtAppDt.Text = Convert.ToString(dt.Rows[0]["ApplicationDt"]).Trim();
                    PopApplicant();
                    ddlLoanApplicantname.SelectedIndex = ddlLoanApplicantname.Items.IndexOf(ddlLoanApplicantname.Items.FindByValue(Convert.ToString(dt.Rows[0]["CustID"])));
                    PopPurpose();
                    PopLoanType();
                    ddlLnPurpose.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                    ddlLnScheme.SelectedIndex = ddlLnScheme.Items.IndexOf(ddlLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
                    EnableMachinDtl(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
                    txtAppLnAmt.Text = Convert.ToString(dt.Rows[0]["AppAmount"]).Trim();
                    txtTenure.Text = Convert.ToString(dt.Rows[0]["Tenure"]).Trim();
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
                }
                if (dt1.Rows.Count > 0)
                {
                    gvCoAppDtl.DataSource = dt1;
                    gvCoAppDtl.DataBind();
                }
                else
                {
                    // this code used to corelate coapplicant with loan application no (from back end)
                    //DataTable dt10 = new DataTable();
                    //CMember OMem = new CMember();
                    //string vCustId = ddlLoanApplicantname.SelectedValue.ToString();
                    //try
                    //{
                    //    dt10 = OMem.GetCoAppByCustId(vCustId);
                    //    if (dt10.Rows.Count > 0)
                    //    {
                    //        gvCoAppDtl.DataSource = dt10;
                    //        gvCoAppDtl.DataBind();
                    //    }
                    //    else
                    //    {
                    //        gvCoAppDtl.DataSource = null;
                    //        gvCoAppDtl.DataBind();

                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                    //finally
                    //{
                    //    dt10 = null;
                    //    OMem = null;
                    //}


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

        private void GetCombineMOS(string LnAppId)
        {
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetCombineMOS(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    lblCombMOS.Text = dt.Rows[0]["CombMOS"].ToString();
                }
                else
                {
                    lblCombMOS.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void GetBankToTR(string LnAppId)
        {
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetBankToTR(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    lblBankingToTR.Text = dt.Rows[0]["BankingtoTR"].ToString();
                }
                else
                {
                    lblBankingToTR.Text = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void GetCAMCIBILRec(string LnAppId)
        {
            CMember Omem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                ds = Omem.GetCIBILRecByLnAppId(LnAppId);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    if (dt.Rows.Count > 0)
                    {
                        txtExistingEMI.Text = dt.Rows[0]["TotEMI"].ToString();
                        SumLiability();
                    }
                    else
                    {
                        txtExistingEMI.Text = "0";
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        txtCIBILLnOutst.Text = dt1.Rows[0]["CIBILPOS"].ToString();
                        SumDebt();
                    }
                    else
                    {
                        txtCIBILLnOutst.Text = "0";
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        txtKFIExpos.Text = dt2.Rows[0]["KFIPOS"].ToString();
                        SumDebt();
                    }
                    else
                    {
                        txtKFIExpos.Text = "0";
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void GetCAMBankAcRec(string LnAppId)
        {
            CMember Omem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                ds = Omem.GetCAMBankAcRec(LnAppId);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtCCorOD.Text = dt.Rows[0]["CC/ODLmt"].ToString();
                        SumDebt();
                    }
                    else
                    {
                        txtCCorOD.Text = "0";
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void GetFinStatRecordForCAM(string LnAppId)
        {
            CMember Omem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                ds = Omem.GetFinStatRecordForCAM(LnAppId);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        txtTotEquityManual.Text = dt.Rows[0]["TotEquity"].ToString();
                        CalculateDE();
                    }
                    else
                    {
                        txtTotEquityManual.Text = "0";
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        txtPBDIT.Text = dt1.Rows[0]["PBDITAmt/Month"].ToString();
                        CalculateDSCR();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        private void ShowCBDetails(string pCBId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember ca = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = ca.GetCBDtlbyCBid(Convert.ToInt32(pCBId));
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 4;
                    ViewAcess();
                    hdfCBId.Value = Convert.ToString(dt.Rows[0]["CBId"]).Trim();
                    btnSaveCB.Enabled = false;
                    btnUpdateCB.Enabled = true;
                    btnDeleteCB.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtLnAppCB.Text = Convert.ToString(dt.Rows[0]["ApplicationId"]).Trim();
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
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
            }
        }
        private void ShowCBDetailsByAppId(string LnAppId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember ca = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = ca.GetCBDtlbyAppId(LnAppId);
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 4;
                    ViewAcess();
                    hdfCBId.Value = Convert.ToString(dt.Rows[0]["CBId"]).Trim();
                    btnSaveCB.Enabled = false;
                    btnUpdateCB.Enabled = true;
                    btnDeleteCB.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtLnAppCB.Text = Convert.ToString(dt.Rows[0]["ApplicationId"]).Trim();
                    gvCBDtl.DataSource = dt;
                    gvCBDtl.DataBind();
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
            }
        }
        private void ShowBankAcDetails(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetBankAcDtlbyAcId(Convert.ToInt32(pBankAcId));
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[2];
                dt4 = ds.Tables[3];
                if (dt1.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 5;
                    ViewAcess();
                    hfBankAcId.Value = Convert.ToString(dt1.Rows[0]["BankAccId"]).Trim();
                    btnBankAcSave.Enabled = false;
                    btnBankAcUpdate.Enabled = true;
                    btnDeleteBankAc.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppIdBankAc.Text = Convert.ToString(dt1.Rows[0]["ApplicationId"]).Trim();
                    txtAcHolName.Text = Convert.ToString(dt1.Rows[0]["AcHolName"]).Trim();
                    txtBankAcSrNo.Text = Convert.ToString(dt1.Rows[0]["SerialNo"]).Trim();
                    txtBankName.Text = Convert.ToString(dt1.Rows[0]["BankName"]).Trim();
                    ddlACType.SelectedValue = Convert.ToString(dt1.Rows[0]["AccType"]).Trim();
                    txtAccNo.Text = Convert.ToString(dt1.Rows[0]["AccNo"]).Trim();
                    txtBankSinYr.Text = Convert.ToString(dt1.Rows[0]["BankingSinceYr"]).Trim();
                    txtBankAcRemarks.Text = Convert.ToString(dt1.Rows[0]["Remarks"]).Trim();
                    txtTransLimit.Text = Convert.ToString(dt1.Rows[0]["TransLimit"]).Trim();
                    txtCurrBalance.Text = Convert.ToString(dt1.Rows[0]["CurrntBalance"]).Trim();
                    if (Convert.ToString(dt1.Rows[0]["ShowinCombMOS"]) == "Y")
                        chkCombMOS.Checked = true;
                    else
                        chkCombMOS.Checked = false;
                    if (Convert.ToString(dt1.Rows[0]["SowInAgreement"]) == "Y")
                        chkShowAgrmnt.Checked = true;
                    else
                        chkShowAgrmnt.Checked = false;
                }
                if (dt2.Rows.Count > 0)
                {
                    ViewState["BankDtl"] = dt2;
                    gvBankDtl.DataSource = dt2;
                    gvBankDtl.DataBind();
                }
                else
                {
                    gvBankDtl.DataSource = null;
                    gvBankDtl.DataBind();
                    popBankDetails();

                }
                if (dt3.Rows.Count > 0)
                {
                    hfBankStatDocId.Value = Convert.ToString(dt3.Rows[0]["DocUpID"]).Trim();
                }
                if (dt4.Rows.Count > 0)
                {
                    ViewState["BankIWOWDtl"] = dt4;
                    gvIWOWDtl.DataSource = dt4;
                    gvIWOWDtl.DataBind();
                }
                else
                {
                    gvIWOWDtl.DataSource = null;
                    gvIWOWDtl.DataBind();
                    popBankIWOWDetails();
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
                dt2 = null;
                dt3 = null;
            }
        }
        private void AccountDetails(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetBankAcDtlbyAcId(Convert.ToInt32(pBankAcId));
                dt1 = ds.Tables[0];
                dt2 = ds.Tables[1];
                dt3 = ds.Tables[3];
                if (dt2.Rows.Count > 0)
                {
                    gvAcDtl.DataSource = dt2;
                    gvAcDtl.DataBind();
                }
                else
                {
                    gvAcDtl.DataSource = null;
                    gvAcDtl.DataBind();
                }
                if (dt3.Rows.Count > 0)
                {
                    gvChqReturn.DataSource = dt3;
                    gvChqReturn.DataBind();
                }
                else
                {
                    gvChqReturn.DataSource = null;
                    gvChqReturn.DataBind();
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
                dt2 = null;
                dt3 = null;
            }
        }
        private void ShowBankStatDiffbyAcId(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Omem.GetBankStatDiffbyAcId(Convert.ToInt32(pBankAcId));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
                    else
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
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
                dt2 = null;
            }
        }
        private void ShowMOSbyAcId(string pBankAcId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                ds = Omem.GetMOSAcId(Convert.ToInt32(pBankAcId));
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt2.Rows.Count > 0)
                    {
                        ViewState["Records"] = dt2.Rows[0]["Records"].ToString();
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        gvBankMOS.DataSource = dt1;
                        gvBankMOS.DataBind();
                    }
                    else
                    {
                        gvBankMOS.DataSource = null;
                        gvBankMOS.DataBind();
                    }
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
                dt2 = null;
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
                    mView.ActiveViewIndex = 8;
                    ViewAcess();
                    hfRefId.Value = Convert.ToString(dt1.Rows[0]["RefId"]).Trim();
                    btnSaveRef.Enabled = false;
                    btnUpdateRef.Enabled = true;
                    //btnDelRef.Enabled = true;
                    txtAppNoRef.Text = Convert.ToString(dt1.Rows[0]["ApplicationId"]).Trim();
                    txtRefName.Text = Convert.ToString(dt1.Rows[0]["RefName"]).Trim();
                    txtRefAddr.Text = Convert.ToString(dt1.Rows[0]["RefAddress"]).Trim();
                    txtRefMNo.Text = Convert.ToString(dt1.Rows[0]["RefMob"]).Trim();
                    PopRelation();
                    ddlRelWApp.SelectedIndex = ddlRelWApp.Items.IndexOf(ddlRelWApp.Items.FindByValue(Convert.ToString(dt1.Rows[0]["RelWithApplicant"])));

                    txtOffTelNoRef.Text = Convert.ToString(dt1.Rows[0]["OffTelNo"]).Trim();
                    txtCompNameRef.Text = Convert.ToString(dt1.Rows[0]["CompanyName"]).Trim();
                    txtStatus.Text = Convert.ToString(dt1.Rows[0]["Status"]).Trim();
                    txtVarifiedby.Text = Convert.ToString(dt1.Rows[0]["VarifiedBy"]);
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
        private void ShowCAMDetailsById(int pCAMId, string pAppId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataSet ds = new DataSet();

            try
            {
                ds = Omem.GetCAMDtlById(pCAMId, pAppId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    popAMQuestion();
                    popAMValues();
                    PopPurpose();
                    PopLoanType();
                    if (dt1.Rows.Count > 0)
                    {
                        mView.ActiveViewIndex = 9;
                        ViewAcess();
                        hfCAMId.Value = Convert.ToString(dt1.Rows[0]["CAMId"]).Trim();
                        btnSaveCAM.Enabled = false;
                        btnUpdateCAM.Enabled = true;
                        btnDelAM.Enabled = true;
                        txtAppNoAM.Text = dt1.Rows[0]["LoanAppId"].ToString();
                        ddlLnPur.SelectedIndex = ddlLnPur.Items.IndexOf(ddlLnPur.Items.FindByValue(Convert.ToString(dt1.Rows[0]["LoanPurposeId"])));
                        ddlCAMLnScheme.SelectedIndex = ddlCAMLnScheme.Items.IndexOf(ddlCAMLnScheme.Items.FindByValue(Convert.ToString(dt1.Rows[0]["LoanTypeId"])));
                        AssementMatrixEnable(Convert.ToInt32(dt1.Rows[0]["LoanTypeId"]));
                        txtLnAmt.Text = dt1.Rows[0]["AppLnAmt"].ToString();
                        txtProLnAmt.Text = dt1.Rows[0]["ProLnAmt"].ToString();
                        txtProInstRate.Text = dt1.Rows[0]["ProFIntRate"].ToString();
                        txtAppTenure.Text = dt1.Rows[0]["AppTenure"].ToString();
                        txtProTenure.Text = dt1.Rows[0]["ProTenure"].ToString();
                        txtProNoofInst.Text = dt1.Rows[0]["ProNoOfInst"].ToString();
                        txtTermLn.Text = dt1.Rows[0]["TermLoan"].ToString();
                        txtCCorOD.Text = dt1.Rows[0]["CCorOD"].ToString();
                        txtCIBILLnOutst.Text = dt1.Rows[0]["CIBILLoanOS"].ToString();
                        txtKFIExpos.Text = dt1.Rows[0]["KFIExistExpos"].ToString();
                        txtProExpos.Text = dt1.Rows[0]["ProExpos"].ToString();
                        txtTotalDebtManual.Text = dt1.Rows[0]["TotalDebt"].ToString();
                        txtExistingEMI.Text = dt1.Rows[0]["ExistEMI"].ToString();
                        txtProEMI.Text = dt1.Rows[0]["ProEMI"].ToString();
                        txtCCInstSav.Text = dt1.Rows[0]["CCIntSave"].ToString();
                        txtTotMonlylia.Text = dt1.Rows[0]["TotalMonLiability"].ToString();
                        txtCAMRemarks.Text = dt1.Rows[0]["Remarks"].ToString();
                        txttotScore.Text = dt1.Rows[0]["TotalAMScore"].ToString();
                        txtTotScorePer.Text = dt1.Rows[0]["TotalAMScorePercen"].ToString();
                        txtTotEquityManual.Text = dt1.Rows[0]["TotalEquity"].ToString();
                        txtDEManual.Text = dt1.Rows[0]["DERatio"].ToString();
                        txtTotDebtComments.Text = dt1.Rows[0]["TotDebtComments"].ToString();
                        txtPBDIT.Text = dt1.Rows[0]["PBDIT"].ToString();
                        txtCustDocNameCR.Text = dt1.Rows[0]["DSCR"].ToString();
                        txtTotEquityComments.Text = dt1.Rows[0]["TotEquityComments"].ToString();
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        ddlAMAns1.SelectedValue = dt2.Rows[0]["Value1"].ToString();
                        ddlAMAns2.SelectedValue = dt2.Rows[0]["Value2"].ToString();
                        ddlAMAns3.SelectedValue = dt2.Rows[0]["Value3"].ToString();
                        ddlAMAns4.SelectedValue = dt2.Rows[0]["Value4"].ToString();
                        ddlAMAns5.SelectedValue = dt2.Rows[0]["Value5"].ToString();
                        ddlAMAns6.SelectedValue = dt2.Rows[0]["Value6"].ToString();
                        ddlAMAns7.SelectedValue = dt2.Rows[0]["Value7"].ToString();
                        ddlAMAns8.SelectedValue = dt2.Rows[0]["Value8"].ToString();
                        ddlAMAns9.SelectedValue = dt2.Rows[0]["Value9"].ToString();
                        ddlAMAns10.SelectedValue = dt2.Rows[0]["Value10"].ToString();
                        ddlAMAns11.SelectedValue = dt2.Rows[0]["Value11"].ToString();
                        ddlAMAns12.SelectedValue = dt2.Rows[0]["Value12"].ToString();
                        ddlAMAns13.SelectedValue = dt2.Rows[0]["Value13"].ToString();
                        ddlAMAns14.SelectedValue = dt2.Rows[0]["Value14"].ToString();
                        ddlAMAns15.SelectedValue = dt2.Rows[0]["Value15"].ToString();
                        ddlAMAns16.SelectedValue = dt2.Rows[0]["Value16"].ToString();
                        ddlAMAns17.SelectedValue = dt2.Rows[0]["Value17"].ToString();
                        ddlAMAns18.SelectedValue = dt2.Rows[0]["Value18"].ToString();
                    }
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
                dt2 = null;
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
            }

            if (e.CommandName == "cmdDownld")
            {
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetCoAppDocByCoAppId(vCoAppID);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }
        }
        protected void gvBankAcList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vBankAcId = "";
            vBankAcId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
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
                ShowBankAcDetails(vBankAcId);
            }
            else if (e.CommandName == "cmdShowMOS")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShowDiff = (LinkButton)gvRow.FindControl("btnShowDiff");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
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
                btnShowDiff.ForeColor = System.Drawing.Color.White;
                btnShowDiff.Font.Bold = true;
                ShowMOSbyAcId(vBankAcId);

            }
            else if (e.CommandName == "cmdShowDtl")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShowDtl = (LinkButton)gvRow.FindControl("btnShowDtl");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
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
                btnShowDtl.ForeColor = System.Drawing.Color.White;
                btnShowDtl.Font.Bold = true;
                AccountDetails(vBankAcId);
            }
            else if (e.CommandName == "cmdDownld")
            {
                GridViewRow gvRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgAttachmnt = (ImageButton)gvRow.FindControl("imgDownload");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvBankAcList.Rows)
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
                imgAttachmnt.ForeColor = System.Drawing.Color.White;
                imgAttachmnt.Font.Bold = true;


                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetBankDocUpByBankAcId(Convert.ToInt32(vBankAcId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }
        }
        protected void gvCB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vCBId = "";
            //vCBId = Convert.ToString(e.CommandArgument);

            string vLnAppId = "";
            vLnAppId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCB.Rows)
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
                ShowCBDetailsByAppId(vLnAppId);
                //ShowCBDetails(vCBId);

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
            else if (e.CommandName == "cmdShowInfo")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                ShowLoanRelationDetails(vApplicantionID);
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
        protected void gvCAM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCAMID = "";
            vCAMID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCAM.Rows)
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
                ShowCAMDetailsById(Convert.ToInt32(vCAMID), ViewState["LoanAppId"].ToString());
            }
        }
        protected void gvCoAppDtl_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            //string vCBId = "";
            //vCBId = Convert.ToString(e.CommandArgument);

            string vLnAppId = "";
            vLnAppId = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShow")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShow");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvCoAppDtl.Rows)
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
            }
            else if (e.CommandName == "cmdShowInfo")
            {
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == false)
                {
                    GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvRow.RowIndex;
                    string ScoreValue = "";
                    ScoreValue = gvCoAppDtl.Rows[RowIndex].Cells[2].Text.ToString();
                    ScoreValue = ScoreValue.Replace("&nbsp;", "");

                    if (ScoreValue != "")
                    {
                        gblFuction.AjxMsgPopup("Online CB Check Already Done For this Co-Applicant as CB Score Already Showing...");
                        return;
                    }
                    if (txtAppNo.Text == "")
                    {
                        gblFuction.AjxMsgPopup("Please Save Loan Application First, Then Check CB Online");
                        return;
                    }

                    string pLnAppId = "", pCoappId = "";
                    pLnAppId = txtAppNo.Text.ToString();
                    pCoappId = e.CommandArgument.ToString();

                    XmlDocument doc = new XmlDocument();
                    XmlDeclaration xDeclare = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement documentRoot = doc.DocumentElement;
                    doc.InsertBefore(xDeclare, documentRoot);
                    XmlElement DCRequest = (XmlElement)doc.AppendChild(doc.CreateElement("DCRequest"));
                    DCRequest.SetAttribute("xmlns", "http://transunion.com/dc/extsvc");
                    XmlElement Authentication = (XmlElement)DCRequest.AppendChild(doc.CreateElement("Authentication"));

                    Authentication.SetAttribute("type", "OnDemand");
                    XmlElement UserId = (XmlElement)Authentication.AppendChild(doc.CreateElement("UserId"));
                    UserId.InnerText = "KUDOS_Dev_User";
                    XmlElement Password = (XmlElement)Authentication.AppendChild(doc.CreateElement("Password"));
                    Password.InnerText = "Password@123";

                    XmlElement RequestInfo = (XmlElement)DCRequest.AppendChild(doc.CreateElement("RequestInfo"));
                    XmlElement ExecutionMode = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("ExecutionMode"));
                    ExecutionMode.InnerText = "NewWithContext";
                    XmlElement SolutionSetId = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("SolutionSetId"));
                    SolutionSetId.InnerText = "2081";
                    XmlElement SolutionSetVersion = (XmlElement)RequestInfo.AppendChild(doc.CreateElement("SolutionSetVersion"));
                    SolutionSetVersion.InnerText = "2";

                    XmlElement Fields = (XmlElement)DCRequest.AppendChild(doc.CreateElement("Fields"));
                    XmlElement Field1 = (XmlElement)Fields.AppendChild(doc.CreateElement("Field"));
                    Field1.SetAttribute("key", "Applicants");

                    string AppLnAmt = Convert.ToInt32(Convert.ToDecimal(txtAppLnAmt.Text)).ToString();
                    DataTable dt = new DataTable();
                    CMember CMem = new CMember();
                    dt = CMem.GetCAXMLRecForCIBIL(e.CommandArgument.ToString());
                    string strField1 = dt.Rows[0][0].ToString();
                    strField1 = strField1.Replace("&amp;", "");
                    XmlCDataSection CData = doc.CreateCDataSection(strField1);
                    Field1.AppendChild(CData);
                    XmlElement Field2 = (XmlElement)Fields.AppendChild(doc.CreateElement("Field"));
                    Field2.SetAttribute("key", "ApplicationData");
                    string strField2 = "<ApplicationData><Purpose>51</Purpose><Amount>" + AppLnAmt + "</Amount><ScoreType>01</ScoreType><GSTStateCode>27</GSTStateCode><MemberCode>NB67098899_C2C</MemberCode><Password>lbW+juakg0</Password><CibilBureauFlag>FALSE</CibilBureauFlag><DSTuNtcFlag>FALSE</DSTuNtcFlag><IDVerificationFlag>FALSE</IDVerificationFlag><MFIBureauFlag>FALSE</MFIBureauFlag><NTCProductType>AL</NTCProductType><ConsumerConsentForUIDAIAuthentication>Y</ConsumerConsentForUIDAIAuthentication><MFIEnquiryAmount>100000</MFIEnquiryAmount><MFILoanPurpose>40</MFILoanPurpose><MFICenterReferenceNo></MFICenterReferenceNo><MFIBranchReferenceNo></MFIBranchReferenceNo><FormattedReport>False</FormattedReport></ApplicationData>";

                    XmlCDataSection CData1 = doc.CreateCDataSection(strField2);
                    Field2.AppendChild(CData1);
                    string strFinalXML = doc.InnerXml;

                    string vRtnXML = string.Empty;
                    AuthSms oAuth = new AuthSms();
                    //vRtnXML = @"{""Result"":true,""Data"":""\u003c?xml version=\""1.0\""?\u003e\u003cDCResponse\u003e\u003cStatus\u003eSuccess\u003c/Status\u003e\u003cAuthentication\u003e\u003cStatus\u003eSuccess\u003c/Status\u003e\u003cToken\u003e81f04e58-8640-4347-b420-39afa32924c4\u003c/Token\u003e\u003c/Authentication\u003e\u003cResponseInfo\u003e\u003cApplicationId\u003e360073862\u003c/ApplicationId\u003e\u003cSolutionSetInstanceId\u003e23d9ba33-0759-4009-a170-b9de501a4cff\u003c/SolutionSetInstanceId\u003e\u003cCurrentQueue\u003e\u003c/CurrentQueue\u003e\u003c/ResponseInfo\u003e\u003cContextData\u003e\u003cField key=\""Applicant\""\u003e\u003cApplicant\u003e\r\n\u003cAccounts\u003e\r\n\u003cAccount\u003e\r\n\u003cAccountNumber\u003e\r\n\u003c/AccountNumber\u003e\r\n\u003c/Account\u003e\r\n\u003c/Accounts\u003e\r\n\u003cMemberOtherId1Type\u003e\r\n\u003c/MemberOtherId1Type\u003e\r\n\u003cMemberOtherId1\u003e\r\n\u003c/MemberOtherId1\u003e\r\n\u003cMemberOtherId2Type\u003e\r\n\u003c/MemberOtherId2Type\u003e\r\n\u003cMemberOtherId2\u003e\r\n\u003c/MemberOtherId2\u003e\r\n\u003cMemberOtherId3Type\u003e\r\n\u003c/MemberOtherId3Type\u003e\r\n\u003cMemberOtherId3\u003e\r\n\u003c/MemberOtherId3\u003e\r\n\u003cKeyPersonName\u003e\r\n\u003c/KeyPersonName\u003e\r\n\u003cKeyPersonRelation\u003e\r\n\u003c/KeyPersonRelation\u003e\r\n\u003cMemberRelationName1\u003e\r\n\u003c/MemberRelationName1\u003e\r\n\u003cMemberRelationType1\u003e\r\n\u003c/MemberRelationType1\u003e\r\n\u003cMemberRelationName2\u003e\r\n\u003c/MemberRelationName2\u003e\r\n\u003cMemberRelationType2\u003e\r\n\u003c/MemberRelationType2\u003e\r\n\u003cMemberRelationName3\u003e\r\n\u003c/MemberRelationName3\u003e\r\n\u003cMemberRelationType3\u003e\r\n\u003c/MemberRelationType3\u003e\r\n\u003cMemberRelationName4\u003e\r\n\u003c/MemberRelationName4\u003e\r\n\u003cMemberRelationType4\u003e\r\n\u003c/MemberRelationType4\u003e\r\n\u003cNomineeName\u003e\r\n\u003c/NomineeName\u003e\r\n\u003cNomineeRelation\u003e\r\n\u003c/NomineeRelation\u003e\r\n\u003cAddresses\u003e\r\n\u003cAddress\u003e\r\n\u003cStateCode\u003e27\u003c/StateCode\u003e\r\n\u003cResidenceType\u003e01\u003c/ResidenceType\u003e\r\n\u003cPinCode\u003e411027\u003c/PinCode\u003e\r\n\u003cCity\u003ePUNE\u003c/City\u003e\r\n\u003cAddressType\u003e01\u003c/AddressType\u003e\r\n\u003cAddressLine5\u003e\r\n\u003c/AddressLine5\u003e\r\n\u003cAddressLine4\u003e\r\n\u003c/AddressLine4\u003e\r\n\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\r\n\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\r\n\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\r\n\u003c/Address\u003e\r\n\u003c/Addresses\u003e\r\n\u003cTelephones\u003e\r\n\u003cTelephone\u003e\r\n\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\r\n\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\r\n\u003cTelephoneExtension\u003e\r\n\u003c/TelephoneExtension\u003e\r\n\u003c/Telephone\u003e\r\n\u003c/Telephones\u003e\r\n\u003cIdentifiers\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e01\u003c/IdType\u003e\r\n\u003cIdNumber\u003eBACPP6592D\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e07\u003c/IdType\u003e\r\n\u003cIdNumber\u003e170650081989\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003c/Identifiers\u003e\r\n\u003cCompanyName\u003e\r\n\u003c/CompanyName\u003e\r\n\u003cEmailAddress\u003e\r\n\u003c/EmailAddress\u003e\r\n\u003cGender\u003eMALE\u003c/Gender\u003e\r\n\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\r\n\u003cApplicantLastName\u003ePATEL\u003c/ApplicantLastName\u003e\r\n\u003cApplicantMiddleName\u003e\r\n\u003c/ApplicantMiddleName\u003e\r\n\u003cApplicantFirstName\u003eKISHOR MEKHJI\u003c/ApplicantFirstName\u003e\r\n\u003cDsCibilBureau\u003e\r\n\u003cDsCibilBureauData\u003e\r\n\u003cRequest\u003e\u003cRequest\u003e\u003cConsumerDetails\u003e\u003cCreditReportInquiry\u003e\u003cHeader\u003e\u003cSegmentTag\u003e TUEF \u003c/SegmentTag\u003e\u003cVersion\u003e 12 \u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cFutureUse1\u003e\u003c/FutureUse1\u003e\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\u003cPurpose\u003e10\u003c/Purpose\u003e\u003cAmount\u003e100000\u003c/Amount\u003e\u003cFutureUse2\u003e\u003c/FutureUse2\u003e\u003cScoreType\u003e01\u003c/ScoreType\u003e\u003cOutputFormat\u003e01\u003c/OutputFormat\u003e\u003cResponseSize\u003e1\u003c/ResponseSize\u003e\u003cMediaType\u003eCC\u003c/MediaType\u003e\u003cAuthenticationMethod\u003eL\u003c/AuthenticationMethod\u003e\u003c/Header\u003e\u003cNames\u003e\u003cName\u003e \u003cConsumerName1\u003eKISHOR MEKHJI\u003c/ConsumerName1\u003e \u003cConsumerName2\u003e\u003c/ConsumerName2\u003e \u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e \u003cConsumerName4\u003e\u003c/ConsumerName4\u003e \u003cConsumerName5\u003e\u003c/ConsumerName5\u003e \u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e \u003cGender\u003eMALE\u003c/Gender\u003e \u003c/Name\u003e\u003c/Names\u003e\u003cIdentifications\u003e \u003cIdentification\u003e\u003cPanNo\u003eBACPP6592D\u003c/PanNo\u003e\u003cPassportNumber\u003e\u003c/PassportNumber\u003e\u003cDLNo\u003e\u003c/DLNo\u003e\u003cVoterId\u003e\u003c/VoterId\u003e\u003cUId\u003e\u003c/UId\u003e\u003cRationCardNo\u003e\u003c/RationCardNo\u003e\u003cAdditionalID1\u003e170650081989\u003c/AdditionalID1\u003e\u003cAdditionalID2\u003e\u003c/AdditionalID2\u003e\u003c/Identification\u003e\u003c/Identifications\u003e\u003cTelephones\u003e\u003cTelephone\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneExtension\u003e\u003c/TelephoneExtension\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/Telephone\u003e\u003c/Telephones\u003e\u003cAddresses\u003e\u003cAddress\u003e\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\u003cAddressLine4\u003e\u003c/AddressLine4\u003e\u003cAddressLine5\u003e\u003c/AddressLine5\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003c/Address\u003e\u003c/Addresses\u003e\u003c/CreditReportInquiry\u003e\u003c/ConsumerDetails\u003e\u003c/Request\u003e\u003c/Request\u003e\r\n\u003c/DsCibilBureauData\u003e\r\n\u003cDsCibilBureauStatus\u003e\r\n\u003cTrail\u003e\u003c/Trail\u003e\r\n\u003c/DsCibilBureauStatus\u003e\r\n\u003cResponse\u003e\r\n\u003cCibilBureauResponse\u003e\r\n\u003cBureauResponseRaw\u003eTUEF12360073862                  0000NB67098899_C2C                100263130912228092018170049PN03N010107KISHORE0212MEKHJI PATEL0305PATEL07081708198608012ID03I010102010210BACPP6592DID03I020102060212492403246555PT03T01011102048606018030203PT03T020114211412345 EXT.030200PT03T0301109823413581030203PT03T0401109823413581030201EC03C010119BERABAZAR@GMAIL.COMEC03C020124BERABAZAR@REDIFFMAIL.COMEM03E01010252020831082018030204SC10CIBILTUSCR010201020210030828092018040500819130201250210PA03A010109PUNE PUNE06022707064110270802021008160520189001YPA03A020115AUNDH CAMP PUNE06022707064110270802020902011008190320189001YPA03A030140FLAT NO. B-202 HERITAGE BUILDING OLD SAN0227SANGAMNAGAR PUNE AUNDH CAMP0325PUNE NEAR NATIONAL SCHOOL0602270706411027080201100817102017PA03A040139S NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND0235FLOOR FLAT NO B 202 SANGAM NAGAR NR0338NATIONAL SCHOOL OLD SANGVI PUNE 4110270602270706411027080204100831032015TL04T0010213NOT DISCLOSED040210050110808270620180908060820181108230920181304208414031002809007000000300801092018310801072018440203TL04T0020213NOT DISCLOSED04020505011080810052018090805072018110831072018120620000013061915402809000000000300801072018310801052018390236TL04T0030205KUDOS0310000000056604025105014080828032018090805072018110831072018120632000013063021542815000XXX000000XXX300801072018310801032018380516.00400513156TL04T0040213NOT DISCLOSED04025005014080828032018090830062018110830062018120630000013062731982812STDSTDSTDSTD300801062018310801032018TL04T0050213NOT DISCLOSED040210050110808100120180908290620181108310720181205287661306-50000282100000000000000000000030080107201831080101201836053000037046000450550000TL04T0060213NOT DISCLOSED04020505011080813122017090831072018110831072018120650000013064275402824000XXX000000000000000000300801072018310801122017TL04T0070213NOT DISCLOSED04020105011080824102017090810082018110831082018120684800013067279882833000000000000000000000000000000000300801082018310801102017TL04T0080213NOT DISCLOSED040252050140808070720171108310820181206100000130102821000000000000000000000300801082018310801022018350200380520.003902124004926345044000TL04T0090213NOT DISCLOSED040205050110808270920160908141220171008141220171108311220171206450000130102848000000000000000000000000000000000000000000000000300801122017310801092016TL04T0100205KUDOS03041164040251050140808080620160908010120171008270320171108310720171206300000130102836000XXXXXXXXXXXXXXX000XXXXXX000000000300801072017310801082016TL04T0110213NOT DISCLOSED0402510501108083005201609080508201811083108201812064150001306132429285400000000000000000000000000000000000000000000000000000029300000000000000000000000000000003008010820183108010520163902364005160654402034506417690TL04T0120213NOT DISCLOSED040205050110808310820150908270920161008280920161108300920161206301000130102839000000000000000000XXX000000000000000000300801092016310801092015TL04T0130213NOT DISCLOSED04020305011080806052015090810082018110831082018120670000013066467882854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328800350201380514.85390317840049797440203TL04T0140213NOT DISCLOSED0402020501108081103201509081005201511083105201512071100000130710955742803000300801052015310801052015TL04T0150213NOT DISCLOSED040202050110808110320150908100820181108310820181207110000013069875602854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328000350201380511.853903178400513202440203TL04T0160213NOT DISCLOSED040205050110808310720140908310820151008020920151108300920151206225000130102842000000000000000000000XXX000000000000000000300801092015310801082014TL04T0170213NOT DISCLOSED040210050110808300720130908210520151008101220151108311220151205318441302-428540000000000000000000000000850860530860560250260550860562936055026000026023000000XXX00000000000030080112201531080107201336051500044020345045310TL04T0180213NOT DISCLOSED040252050130808200220130908231020171008200220181108300620181206724500130142854000000000000STDXXX030030000000000000XXX000STDXXX0000002954000000000000000000000000XXXXXX000000000000000000000000300801062018310801072015390260400527168440203TL04T0190213NOT DISCLOSED0402050501108082507201209083107201410080208201411083008201412061500001301028540000000000000000000000000000000000000000000000000000002921000000000000000000000300801082014310801092011TL04T0200213NOT DISCLOSED0402020501408081104201209081004201510081004201511083004201512071200000130102854000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD2954STDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD300801042015310801052012350201400514286440203TL04T0210213NOT DISCLOSED040213050110808311020110908241120141008241120141108301120141205370001301028540000000000000000000000000000000000000000000000000260002954000000000000000000000000000000000000000000000000000000300801112014310801122011TL04T0220213NOT DISCLOSED04021305011080831102011090829062015100829062015110830062015120537000130102854000000000000000148117086056025025000000000000000000000295400000000000000000000000000000000000000000000000000000030080106201531080107201244020345043037TL04T0230213NOT DISCLOSED040210050110808010620110908280820171108020720181205638711305-176528540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801072018310801082015360568700TL04T0240213NOT DISCLOSED040213050110808210120100908060220131008060220131108280220131205357661301028540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801022013310801032010IQ04I0010108280920180405KUDOS0502100606100000IQ04I0020108260820180413NOT DISCLOSED050203060813000000IQ04I0030108170820180413NOT DISCLOSED050203060810000000IQ04I0040108020820180413NOT DISCLOSED0502050606200000IQ04I0050108170620180413NOT DISCLOSED05020006041000IQ04I0060108240520180413NOT DISCLOSED0502050606100000IQ04I0070108160520180413NOT DISCLOSED0502050606200000IQ04I0080108100520180413NOT DISCLOSED0502610606500000IQ04I0090108080520180413NOT DISCLOSED0502050606218000IQ04I0100108030420180413NOT DISCLOSED05021006041000IQ04I0110108300320180413NOT DISCLOSED05025106071000000IQ04I0120108290320180413NOT DISCLOSED050206060560000IQ04I0130108280320180413NOT DISCLOSED05025106071000000IQ04I0140108260320180413NOT DISCLOSED0502000606266000IQ04I0150108210320180413NOT DISCLOSED05025106071000000IQ04I0160108210320180413NOT DISCLOSED05020006011IQ04I0170108210320180413NOT DISCLOSED05025206071000000IQ04I0180108200320180405KUDOS0502610606100000IQ04I0190108190320180413NOT DISCLOSED05020006011IQ04I0200108160320180413NOT DISCLOSED05025106071500000IQ04I0210108050220180413NOT DISCLOSED05021006041000IQ04I0220108050120180413NOT DISCLOSED050210060510000IQ04I0230108151120170413NOT DISCLOSED0502050606600000IQ04I0240108141120170413NOT DISCLOSED050210060510000IQ04I0250108131120170413NOT DISCLOSED0502050606100000IQ04I0260108101120170413NOT DISCLOSED050210060550000IQ04I0270108041120170413NOT DISCLOSED05021006041000IQ04I0280108171020170413NOT DISCLOSED0502010606500000IQ04I0290108161020170413NOT DISCLOSED0502010606838000IQ04I0300108290820170413NOT DISCLOSED050210060510000IQ04I0310108120820170413NOT DISCLOSED0502610606500000IQ04I0320108020820170413NOT DISCLOSED05020206071000000IQ04I0330108120520170413NOT DISCLOSED050210060550000IQ04I0340108220420170413NOT DISCLOSED05020006071000000IQ04I0350108220420170413NOT DISCLOSED05025106071500000IQ04I0360108210420170413NOT DISCLOSED05025106071000000IQ04I0370108110420170413NOT DISCLOSED0502510606400000IQ04I0380108210320170405KUDOS0502510606200000IQ04I0390108240920160413NOT DISCLOSED0502050606500000IQ04I0400108090920160413NOT DISCLOSED0502050606500000IQ04I0410108170820160413NOT DISCLOSED050210060550000IQ04I0420108120820160413NOT DISCLOSED0502050606500000IQ04I0430108120820160413NOT DISCLOSED0502050606500000IQ04I0440108240620160413NOT DISCLOSED0502020606500000IQ04I0450108260520160413NOT DISCLOSED0502050606800000IQ04I0460108260520160405KUDOS0502000606500000IQ04I0470108310120160413NOT DISCLOSED0502050606800000IQ04I0480108050820150413NOT DISCLOSED0502050606400000IQ04I0490108050520150413NOT DISCLOSED0502020606700000IQ04I0500108260220150413NOT DISCLOSED050210060510000IQ04I0510108250220150413NOT DISCLOSED05020206072000000IQ04I0520108240720140413NOT DISCLOSED0502050606300000IQ04I0530108100720140413NOT DISCLOSED050206060550000IQ04I0540108250420140413NOT DISCLOSED0502050606300000IQ04I0550108120920130413NOT DISCLOSED0502050606400000IQ04I0560108240820130413NOT DISCLOSED050210060550000IQ04I0570108250720130413NOT DISCLOSED050210060550000IQ04I0580108170720130413NOT DISCLOSED050210060510000IQ04I0590108160720130413NOT DISCLOSED0502050606200000IQ04I0600108060320130413NOT DISCLOSED0502050606500000IQ04I0610108120220130413NOT DISCLOSED0502150606700000IQ04I0620108020120130413NOT DISCLOSED050210060510000IQ04I0630108271120120413NOT DISCLOSED0502510606800000IQ04I0640108240920120413NOT DISCLOSED05020206071400000IQ04I0650108310720120413NOT DISCLOSED0502510606500000IQ04I0660108130720120413NOT DISCLOSED0502050606200000IQ04I0670108100720120413NOT DISCLOSED0502050606200000IQ04I0680108100520120413NOT DISCLOSED0502050606300000IQ04I0690108210420120405KUDOS0502510606200000IQ04I0700108130320120413NOT DISCLOSED0502050606700000IQ04I0710108100320120413NOT DISCLOSED05020206071300000IQ04I0720108120120120413NOT DISCLOSED0502050606500000IQ04I0730108040120120413NOT DISCLOSED0502050606500000IQ04I0740108021120110413NOT DISCLOSED0502510606300000IQ04I0750108221020110413NOT DISCLOSED050213060537000IQ04I0760108221020110413NOT DISCLOSED050213060537000IQ04I0770108040820110413NOT DISCLOSED0502050606500000IQ04I0780108020820110413NOT DISCLOSED0502050606500000IQ04I0790108300720110413NOT DISCLOSED0502050606300000IQ04I0800108260720110413NOT DISCLOSED05021506071000000IQ04I0810108150720110413NOT DISCLOSED050202060510000IQ04I0820108150720110413NOT DISCLOSED0502000606900000IQ04I0830108050720110413NOT DISCLOSED05021006041000IQ04I0840108110520110413NOT DISCLOSED050205060510000IQ04I0850108030520110413NOT DISCLOSED05020306071500000IQ04I0860108290420110413NOT DISCLOSED050210060510000IQ04I0870108260420110413NOT DISCLOSED0502510606300000IQ04I0880108130420110413NOT DISCLOSED05020206072000000IQ04I0890108120420110413NOT DISCLOSED05020206071500000IQ04I0900108111020100413NOT DISCLOSED05020206071600000IQ04I0910108081020100413NOT DISCLOSED05020206071600000IQ04I0920108120120100413NOT DISCLOSED050213060535766IQ04I0930108041220090413NOT DISCLOSED050213060537000ES0700105210102**\u003c/BureauResponseRaw\u003e\r\n\u003cBureauResponseXml\u003e\u003cCreditReport\u003e\u003cHeader\u003e\u003cSegmentTag\u003eTUEF\u003c/SegmentTag\u003e\u003cVersion\u003e12\u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cMemberCode\u003eNB67098899_C2C                \u003c/MemberCode\u003e\u003cSubjectReturnCode\u003e1\u003c/SubjectReturnCode\u003e\u003cEnquiryControlNumber\u003e002631309122\u003c/EnquiryControlNumber\u003e\u003cDateProcessed\u003e28092018\u003c/DateProcessed\u003e\u003cTimeProcessed\u003e170049\u003c/TimeProcessed\u003e\u003c/Header\u003e\u003cNameSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eN01\u003c/SegmentTag\u003e\u003cConsumerName1FieldLength\u003e07\u003c/ConsumerName1FieldLength\u003e\u003cConsumerName1\u003eKISHORE\u003c/ConsumerName1\u003e\u003cConsumerName2FieldLength\u003e12\u003c/ConsumerName2FieldLength\u003e\u003cConsumerName2\u003eMEKHJI PATEL\u003c/ConsumerName2\u003e\u003cConsumerName3FieldLength\u003e05\u003c/ConsumerName3FieldLength\u003e\u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e\u003cDateOfBirthFieldLength\u003e08\u003c/DateOfBirthFieldLength\u003e\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\u003cGenderFieldLength\u003e01\u003c/GenderFieldLength\u003e\u003cGender\u003e2\u003c/Gender\u003e\u003c/NameSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI01\u003c/SegmentTag\u003e\u003cIDType\u003e01\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e10\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003eBACPP6592D\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI02\u003c/SegmentTag\u003e\u003cIDType\u003e06\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e12\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003e492403246555\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT01\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e11\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e02048606018\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT02\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e14\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e211412345 EXT.\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e00\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT03\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT04\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC01\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e19\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@GMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC02\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e24\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@REDIFFMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmploymentSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eE01\u003c/SegmentTag\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cDateReportedCertified\u003e31082018\u003c/DateReportedCertified\u003e\u003cOccupationCode\u003e04\u003c/OccupationCode\u003e\u003c/EmploymentSegment\u003e\u003cScoreSegment\u003e\u003cLength\u003e10\u003c/Length\u003e\u003cScoreName\u003eCIBILTUSCR\u003c/ScoreName\u003e\u003cScoreCardName\u003e01\u003c/ScoreCardName\u003e\u003cScoreCardVersion\u003e10\u003c/ScoreCardVersion\u003e\u003cScoreDate\u003e28092018\u003c/ScoreDate\u003e\u003cScore\u003e00819\u003c/Score\u003e\u003cExclusionCode9FieldLength\u003e02\u003c/ExclusionCode9FieldLength\u003e\u003cExclusionCode9\u003e01\u003c/ExclusionCode9\u003e\u003cReasonCode1FieldLength\u003e02\u003c/ReasonCode1FieldLength\u003e\u003cReasonCode1\u003e10\u003c/ReasonCode1\u003e\u003c/ScoreSegment\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA01\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e09\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003ePUNE PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cDateReported\u003e16052018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA02\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e15\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eAUNDH CAMP PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003cDateReported\u003e19032018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA03\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e40\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eFLAT NO. B-202 HERITAGE BUILDING OLD SAN\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e27\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eSANGAMNAGAR PUNE AUNDH CAMP\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e25\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003ePUNE NEAR NATIONAL SCHOOL\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cDateReported\u003e17102017\u003c/DateReported\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA04\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e39\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eS NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e35\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eFLOOR FLAT NO B 202 SANGAM NAGAR NR\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e38\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003eNATIONAL SCHOOL OLD SANGVI PUNE 411027\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e04\u003c/AddressCategory\u003e\u003cDateReported\u003e31032015\u003c/DateReported\u003e\u003c/Address\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT001\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27062018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e23092018\u003c/DateReportedAndCertified\u003e\u003cCurrentBalanceFieldLength\u003e04\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e2084\u003c/CurrentBalance\u003e\u003cAmountOverdueFieldLength\u003e03\u003c/AmountOverdueFieldLength\u003e\u003cAmountOverdue\u003e100\u003c/AmountOverdue\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e007000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072018\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT002\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10052018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e191540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052018\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT003\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e10\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e0000000566\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e320000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e302154\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e15\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000XXX\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e16.00\u003c/RateOfInterest\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13156\u003c/EmiAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT004\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e50\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e30062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e273198\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e12\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003eSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT005\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10012018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e28766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-50000\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01012018\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e30000\u003c/CreditLimit\u003e\u003cCashLimitFieldLength\u003e04\u003c/CashLimitFieldLength\u003e\u003cCashLimit\u003e6000\u003c/CashLimit\u003e\u003cActualPaymentAmountFieldLength\u003e05\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e50000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT006\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e13122017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e500000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e427540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e24\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT007\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e01\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e24102017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e848000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e727988\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e33\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01102017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT008\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e07072017\u003c/DateOpenedOrDisbursed\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01022018\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e00\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e20.00\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e12\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9263\u003c/EmiAmount\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e4000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT009\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27092016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e14122017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e14122017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e450000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e48\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01122017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT010\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e04\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e1164\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e08062016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e01012017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e27032017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31072017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e36\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXXXXXXXXXXXXXX000XXXXXX000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT011\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30052016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e415000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e132429\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e30\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052016\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e16065\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e06\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e417690\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT012\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31082015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e27092016\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e28092016\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092016\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e301000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e39\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092016\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT013\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e03\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e06052015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e700000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e646788\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328800\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e14.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9797\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT014\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10052015\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31052015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e07\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e1095574\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e03\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01052015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT015\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e987560\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328000\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e11.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13202\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT016\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31072014\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31082015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02092015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e225000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e42\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082014\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT017\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30072013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e21052015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10122015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e31844\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e02\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000085086053086056025026055086056\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e36\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e055026000026023000000XXX000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01122015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072013\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e15000\u003c/CreditLimit\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e5310\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT018\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e3\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e20022013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e23102017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e20022018\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e724500\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000STDXXX030030000000000000XXX000STDXXX000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000XXXXXX000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072015\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e60\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e27168\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT019\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e25072012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02082014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30082014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e150000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e21\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT020\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11042012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10042015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10042015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30042015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003eSTDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01042015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052012\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e14286\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT021\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e24112014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e24112014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30112014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000026000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01112014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT022\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e29062015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000148117086056025025000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072012\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e3037\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT023\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e01062011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e28082017\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e02072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e63871\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e05\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-1765\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082015\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e68700\u003c/CreditLimit\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT024\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e21012010\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06022013\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e06022013\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e28022013\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e35766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01022013\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032010\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI001\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28092018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI002\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e13000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI003\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI004\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI005\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17062018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI006\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI007\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI008\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI009\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e218000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI010\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03042018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI011\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI012\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e60000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI013\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI014\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e266000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI015\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI016\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI017\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e52\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI018\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e20032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI019\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e19032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI020\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI021\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05022018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI022\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05012018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI023\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI024\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e14112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI025\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI026\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI027\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI028\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI029\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e838000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI030\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI031\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI032\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI033\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12052017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI034\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI035\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI036\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI037\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI038\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI039\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI040\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e09092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI041\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI042\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI043\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI044\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24062016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI045\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI046\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI047\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31012016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI048\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05082015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI049\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05052015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI050\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI051\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI052\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI053\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI054\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25042014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI055\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12092013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI056\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24082013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI057\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI058\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI059\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI060\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e06032013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI061\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12022013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI062\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02012013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI063\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e27112012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI064\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI065\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI066\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI067\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI068\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI069\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI070\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI071\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI072\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI073\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI074\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02112011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI075\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI076\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI077\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI078\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI079\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI080\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI081\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI082\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e900000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI083\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI084\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI085\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI086\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI087\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI088\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI089\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI090\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI091\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI092\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e35766\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI093\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04122009\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnd\u003e\u003cSegmentTag\u003eES07\u003c/SegmentTag\u003e\u003cTotalLength\u003e0010521\u003c/TotalLength\u003e\u003c/End\u003e\u003c/CreditReport\u003e\u003c/BureauResponseXml\u003e\r\n\u003cSecondaryReportXml\u003e\u003cRoot\u003e\u003c/Root\u003e\u003c/SecondaryReportXml\u003e\r\n\u003cIsSucess\u003eTrue\u003c/IsSucess\u003e\r\n\u003c/CibilBureauResponse\u003e\r\n\u003c/Response\u003e\r\n\u003c/DsCibilBureau\u003e\r\n\u003c/Applicant\u003e\r\n\u003c/Field\u003e\u003cField key=\""Applicants\""\u003e\u003cApplicants\u003e\r\n\u003cApplicant\u003e\r\n\u003cAccounts\u003e\r\n\u003cAccount\u003e\r\n\u003cAccountNumber\u003e\r\n\u003c/AccountNumber\u003e\r\n\u003c/Account\u003e\r\n\u003c/Accounts\u003e\r\n\u003cMemberOtherId1Type\u003e\r\n\u003c/MemberOtherId1Type\u003e\r\n\u003cMemberOtherId1\u003e\r\n\u003c/MemberOtherId1\u003e\r\n\u003cMemberOtherId2Type\u003e\r\n\u003c/MemberOtherId2Type\u003e\r\n\u003cMemberOtherId2\u003e\r\n\u003c/MemberOtherId2\u003e\r\n\u003cMemberOtherId3Type\u003e\r\n\u003c/MemberOtherId3Type\u003e\r\n\u003cMemberOtherId3\u003e\r\n\u003c/MemberOtherId3\u003e\r\n\u003cKeyPersonName\u003e\r\n\u003c/KeyPersonName\u003e\r\n\u003cKeyPersonRelation\u003e\r\n\u003c/KeyPersonRelation\u003e\r\n\u003cMemberRelationName1\u003e\r\n\u003c/MemberRelationName1\u003e\r\n\u003cMemberRelationType1\u003e\r\n\u003c/MemberRelationType1\u003e\r\n\u003cMemberRelationName2\u003e\r\n\u003c/MemberRelationName2\u003e\r\n\u003cMemberRelationType2\u003e\r\n\u003c/MemberRelationType2\u003e\r\n\u003cMemberRelationName3\u003e\r\n\u003c/MemberRelationName3\u003e\r\n\u003cMemberRelationType3\u003e\r\n\u003c/MemberRelationType3\u003e\r\n\u003cMemberRelationName4\u003e\r\n\u003c/MemberRelationName4\u003e\r\n\u003cMemberRelationType4\u003e\r\n\u003c/MemberRelationType4\u003e\r\n\u003cNomineeName\u003e\r\n\u003c/NomineeName\u003e\r\n\u003cNomineeRelation\u003e\r\n\u003c/NomineeRelation\u003e\r\n\u003cAddresses\u003e\r\n\u003cAddress\u003e\r\n\u003cStateCode\u003e27\u003c/StateCode\u003e\r\n\u003cResidenceType\u003e01\u003c/ResidenceType\u003e\r\n\u003cPinCode\u003e411027\u003c/PinCode\u003e\r\n\u003cCity\u003ePUNE\u003c/City\u003e\r\n\u003cAddressType\u003e01\u003c/AddressType\u003e\r\n\u003cAddressLine5\u003e\r\n\u003c/AddressLine5\u003e\r\n\u003cAddressLine4\u003e\r\n\u003c/AddressLine4\u003e\r\n\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\r\n\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\r\n\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\r\n\u003c/Address\u003e\r\n\u003c/Addresses\u003e\r\n\u003cTelephones\u003e\r\n\u003cTelephone\u003e\r\n\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\r\n\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\r\n\u003cTelephoneExtension\u003e\r\n\u003c/TelephoneExtension\u003e\r\n\u003c/Telephone\u003e\r\n\u003c/Telephones\u003e\r\n\u003cIdentifiers\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e01\u003c/IdType\u003e\r\n\u003cIdNumber\u003eBACPP6592D\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003cIdentifier\u003e\r\n\u003cIdType\u003e07\u003c/IdType\u003e\r\n\u003cIdNumber\u003e170650081989\u003c/IdNumber\u003e\r\n\u003c/Identifier\u003e\r\n\u003c/Identifiers\u003e\r\n\u003cCompanyName\u003e\r\n\u003c/CompanyName\u003e\r\n\u003cEmailAddress\u003e\r\n\u003c/EmailAddress\u003e\r\n\u003cGender\u003eMALE\u003c/Gender\u003e\r\n\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\r\n\u003cApplicantLastName\u003ePATEL\u003c/ApplicantLastName\u003e\r\n\u003cApplicantMiddleName\u003e\r\n\u003c/ApplicantMiddleName\u003e\r\n\u003cApplicantFirstName\u003eKISHOR MEKHJI\u003c/ApplicantFirstName\u003e\r\n\u003cDsCibilBureau\u003e\r\n\u003cDsCibilBureauData\u003e\r\n\u003cRequest\u003e\u003cRequest\u003e\u003cConsumerDetails\u003e\u003cCreditReportInquiry\u003e\u003cHeader\u003e\u003cSegmentTag\u003e TUEF \u003c/SegmentTag\u003e\u003cVersion\u003e 12 \u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cFutureUse1\u003e\u003c/FutureUse1\u003e\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\u003cPurpose\u003e10\u003c/Purpose\u003e\u003cAmount\u003e100000\u003c/Amount\u003e\u003cFutureUse2\u003e\u003c/FutureUse2\u003e\u003cScoreType\u003e01\u003c/ScoreType\u003e\u003cOutputFormat\u003e01\u003c/OutputFormat\u003e\u003cResponseSize\u003e1\u003c/ResponseSize\u003e\u003cMediaType\u003eCC\u003c/MediaType\u003e\u003cAuthenticationMethod\u003eL\u003c/AuthenticationMethod\u003e\u003c/Header\u003e\u003cNames\u003e\u003cName\u003e \u003cConsumerName1\u003eKISHOR MEKHJI\u003c/ConsumerName1\u003e \u003cConsumerName2\u003e\u003c/ConsumerName2\u003e \u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e \u003cConsumerName4\u003e\u003c/ConsumerName4\u003e \u003cConsumerName5\u003e\u003c/ConsumerName5\u003e \u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e \u003cGender\u003eMALE\u003c/Gender\u003e \u003c/Name\u003e\u003c/Names\u003e\u003cIdentifications\u003e \u003cIdentification\u003e\u003cPanNo\u003eBACPP6592D\u003c/PanNo\u003e\u003cPassportNumber\u003e\u003c/PassportNumber\u003e\u003cDLNo\u003e\u003c/DLNo\u003e\u003cVoterId\u003e\u003c/VoterId\u003e\u003cUId\u003e\u003c/UId\u003e\u003cRationCardNo\u003e\u003c/RationCardNo\u003e\u003cAdditionalID1\u003e170650081989\u003c/AdditionalID1\u003e\u003cAdditionalID2\u003e\u003c/AdditionalID2\u003e\u003c/Identification\u003e\u003c/Identifications\u003e\u003cTelephones\u003e\u003cTelephone\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneExtension\u003e\u003c/TelephoneExtension\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/Telephone\u003e\u003c/Telephones\u003e\u003cAddresses\u003e\u003cAddress\u003e\u003cAddressLine1\u003eSR NO 4/1/73, FL B-202, SHREE HARITAGE B\u003c/AddressLine1\u003e\u003cAddressLine2\u003eLD, B WING,  SAGER NR NATIONAL SCHOOL, P\u003c/AddressLine2\u003e\u003cAddressLine3\u003eRIYADARSHINI NGR, OLD SANGAVI, PUNE \u003c/AddressLine3\u003e\u003cAddressLine4\u003e\u003c/AddressLine4\u003e\u003cAddressLine5\u003e\u003c/AddressLine5\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003c/Address\u003e\u003c/Addresses\u003e\u003c/CreditReportInquiry\u003e\u003c/ConsumerDetails\u003e\u003c/Request\u003e\u003c/Request\u003e\r\n\u003c/DsCibilBureauData\u003e\r\n\u003cDsCibilBureauStatus\u003e\r\n\u003cTrail\u003e\u003c/Trail\u003e\r\n\u003c/DsCibilBureauStatus\u003e\r\n\u003cResponse\u003e\r\n\u003cCibilBureauResponse\u003e\r\n\u003cBureauResponseRaw\u003eTUEF12360073862                  0000NB67098899_C2C                100263130912228092018170049PN03N010107KISHORE0212MEKHJI PATEL0305PATEL07081708198608012ID03I010102010210BACPP6592DID03I020102060212492403246555PT03T01011102048606018030203PT03T020114211412345 EXT.030200PT03T0301109823413581030203PT03T0401109823413581030201EC03C010119BERABAZAR@GMAIL.COMEC03C020124BERABAZAR@REDIFFMAIL.COMEM03E01010252020831082018030204SC10CIBILTUSCR010201020210030828092018040500819130201250210PA03A010109PUNE PUNE06022707064110270802021008160520189001YPA03A020115AUNDH CAMP PUNE06022707064110270802020902011008190320189001YPA03A030140FLAT NO. B-202 HERITAGE BUILDING OLD SAN0227SANGAMNAGAR PUNE AUNDH CAMP0325PUNE NEAR NATIONAL SCHOOL0602270706411027080201100817102017PA03A040139S NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND0235FLOOR FLAT NO B 202 SANGAM NAGAR NR0338NATIONAL SCHOOL OLD SANGVI PUNE 4110270602270706411027080204100831032015TL04T0010213NOT DISCLOSED040210050110808270620180908060820181108230920181304208414031002809007000000300801092018310801072018440203TL04T0020213NOT DISCLOSED04020505011080810052018090805072018110831072018120620000013061915402809000000000300801072018310801052018390236TL04T0030205KUDOS0310000000056604025105014080828032018090805072018110831072018120632000013063021542815000XXX000000XXX300801072018310801032018380516.00400513156TL04T0040213NOT DISCLOSED04025005014080828032018090830062018110830062018120630000013062731982812STDSTDSTDSTD300801062018310801032018TL04T0050213NOT DISCLOSED040210050110808100120180908290620181108310720181205287661306-50000282100000000000000000000030080107201831080101201836053000037046000450550000TL04T0060213NOT DISCLOSED04020505011080813122017090831072018110831072018120650000013064275402824000XXX000000000000000000300801072018310801122017TL04T0070213NOT DISCLOSED04020105011080824102017090810082018110831082018120684800013067279882833000000000000000000000000000000000300801082018310801102017TL04T0080213NOT DISCLOSED040252050140808070720171108310820181206100000130102821000000000000000000000300801082018310801022018350200380520.003902124004926345044000TL04T0090213NOT DISCLOSED040205050110808270920160908141220171008141220171108311220171206450000130102848000000000000000000000000000000000000000000000000300801122017310801092016TL04T0100205KUDOS03041164040251050140808080620160908010120171008270320171108310720171206300000130102836000XXXXXXXXXXXXXXX000XXXXXX000000000300801072017310801082016TL04T0110213NOT DISCLOSED0402510501108083005201609080508201811083108201812064150001306132429285400000000000000000000000000000000000000000000000000000029300000000000000000000000000000003008010820183108010520163902364005160654402034506417690TL04T0120213NOT DISCLOSED040205050110808310820150908270920161008280920161108300920161206301000130102839000000000000000000XXX000000000000000000300801092016310801092015TL04T0130213NOT DISCLOSED04020305011080806052015090810082018110831082018120670000013066467882854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328800350201380514.85390317840049797440203TL04T0140213NOT DISCLOSED0402020501108081103201509081005201511083105201512071100000130710955742803000300801052015310801052015TL04T0150213NOT DISCLOSED040202050110808110320150908100820181108310820181207110000013069875602854000000000000000000000000000000000000000000000000000000295400000000000000000000000000000000000000000000000000000030080108201831080109201534072328000350201380511.853903178400513202440203TL04T0160213NOT DISCLOSED040205050110808310720140908310820151008020920151108300920151206225000130102842000000000000000000000XXX000000000000000000300801092015310801082014TL04T0170213NOT DISCLOSED040210050110808300720130908210520151008101220151108311220151205318441302-428540000000000000000000000000850860530860560250260550860562936055026000026023000000XXX00000000000030080112201531080107201336051500044020345045310TL04T0180213NOT DISCLOSED040252050130808200220130908231020171008200220181108300620181206724500130142854000000000000STDXXX030030000000000000XXX000STDXXX0000002954000000000000000000000000XXXXXX000000000000000000000000300801062018310801072015390260400527168440203TL04T0190213NOT DISCLOSED0402050501108082507201209083107201410080208201411083008201412061500001301028540000000000000000000000000000000000000000000000000000002921000000000000000000000300801082014310801092011TL04T0200213NOT DISCLOSED0402020501408081104201209081004201510081004201511083004201512071200000130102854000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD2954STDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD300801042015310801052012350201400514286440203TL04T0210213NOT DISCLOSED040213050110808311020110908241120141008241120141108301120141205370001301028540000000000000000000000000000000000000000000000000260002954000000000000000000000000000000000000000000000000000000300801112014310801122011TL04T0220213NOT DISCLOSED04021305011080831102011090829062015100829062015110830062015120537000130102854000000000000000148117086056025025000000000000000000000295400000000000000000000000000000000000000000000000000000030080106201531080107201244020345043037TL04T0230213NOT DISCLOSED040210050110808010620110908280820171108020720181205638711305-176528540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801072018310801082015360568700TL04T0240213NOT DISCLOSED040213050110808210120100908060220131008060220131108280220131205357661301028540000000000000000000000000000000000000000000000000000002954000000000000000000000000000000000000000000000000000000300801022013310801032010IQ04I0010108280920180405KUDOS0502100606100000IQ04I0020108260820180413NOT DISCLOSED050203060813000000IQ04I0030108170820180413NOT DISCLOSED050203060810000000IQ04I0040108020820180413NOT DISCLOSED0502050606200000IQ04I0050108170620180413NOT DISCLOSED05020006041000IQ04I0060108240520180413NOT DISCLOSED0502050606100000IQ04I0070108160520180413NOT DISCLOSED0502050606200000IQ04I0080108100520180413NOT DISCLOSED0502610606500000IQ04I0090108080520180413NOT DISCLOSED0502050606218000IQ04I0100108030420180413NOT DISCLOSED05021006041000IQ04I0110108300320180413NOT DISCLOSED05025106071000000IQ04I0120108290320180413NOT DISCLOSED050206060560000IQ04I0130108280320180413NOT DISCLOSED05025106071000000IQ04I0140108260320180413NOT DISCLOSED0502000606266000IQ04I0150108210320180413NOT DISCLOSED05025106071000000IQ04I0160108210320180413NOT DISCLOSED05020006011IQ04I0170108210320180413NOT DISCLOSED05025206071000000IQ04I0180108200320180405KUDOS0502610606100000IQ04I0190108190320180413NOT DISCLOSED05020006011IQ04I0200108160320180413NOT DISCLOSED05025106071500000IQ04I0210108050220180413NOT DISCLOSED05021006041000IQ04I0220108050120180413NOT DISCLOSED050210060510000IQ04I0230108151120170413NOT DISCLOSED0502050606600000IQ04I0240108141120170413NOT DISCLOSED050210060510000IQ04I0250108131120170413NOT DISCLOSED0502050606100000IQ04I0260108101120170413NOT DISCLOSED050210060550000IQ04I0270108041120170413NOT DISCLOSED05021006041000IQ04I0280108171020170413NOT DISCLOSED0502010606500000IQ04I0290108161020170413NOT DISCLOSED0502010606838000IQ04I0300108290820170413NOT DISCLOSED050210060510000IQ04I0310108120820170413NOT DISCLOSED0502610606500000IQ04I0320108020820170413NOT DISCLOSED05020206071000000IQ04I0330108120520170413NOT DISCLOSED050210060550000IQ04I0340108220420170413NOT DISCLOSED05020006071000000IQ04I0350108220420170413NOT DISCLOSED05025106071500000IQ04I0360108210420170413NOT DISCLOSED05025106071000000IQ04I0370108110420170413NOT DISCLOSED0502510606400000IQ04I0380108210320170405KUDOS0502510606200000IQ04I0390108240920160413NOT DISCLOSED0502050606500000IQ04I0400108090920160413NOT DISCLOSED0502050606500000IQ04I0410108170820160413NOT DISCLOSED050210060550000IQ04I0420108120820160413NOT DISCLOSED0502050606500000IQ04I0430108120820160413NOT DISCLOSED0502050606500000IQ04I0440108240620160413NOT DISCLOSED0502020606500000IQ04I0450108260520160413NOT DISCLOSED0502050606800000IQ04I0460108260520160405KUDOS0502000606500000IQ04I0470108310120160413NOT DISCLOSED0502050606800000IQ04I0480108050820150413NOT DISCLOSED0502050606400000IQ04I0490108050520150413NOT DISCLOSED0502020606700000IQ04I0500108260220150413NOT DISCLOSED050210060510000IQ04I0510108250220150413NOT DISCLOSED05020206072000000IQ04I0520108240720140413NOT DISCLOSED0502050606300000IQ04I0530108100720140413NOT DISCLOSED050206060550000IQ04I0540108250420140413NOT DISCLOSED0502050606300000IQ04I0550108120920130413NOT DISCLOSED0502050606400000IQ04I0560108240820130413NOT DISCLOSED050210060550000IQ04I0570108250720130413NOT DISCLOSED050210060550000IQ04I0580108170720130413NOT DISCLOSED050210060510000IQ04I0590108160720130413NOT DISCLOSED0502050606200000IQ04I0600108060320130413NOT DISCLOSED0502050606500000IQ04I0610108120220130413NOT DISCLOSED0502150606700000IQ04I0620108020120130413NOT DISCLOSED050210060510000IQ04I0630108271120120413NOT DISCLOSED0502510606800000IQ04I0640108240920120413NOT DISCLOSED05020206071400000IQ04I0650108310720120413NOT DISCLOSED0502510606500000IQ04I0660108130720120413NOT DISCLOSED0502050606200000IQ04I0670108100720120413NOT DISCLOSED0502050606200000IQ04I0680108100520120413NOT DISCLOSED0502050606300000IQ04I0690108210420120405KUDOS0502510606200000IQ04I0700108130320120413NOT DISCLOSED0502050606700000IQ04I0710108100320120413NOT DISCLOSED05020206071300000IQ04I0720108120120120413NOT DISCLOSED0502050606500000IQ04I0730108040120120413NOT DISCLOSED0502050606500000IQ04I0740108021120110413NOT DISCLOSED0502510606300000IQ04I0750108221020110413NOT DISCLOSED050213060537000IQ04I0760108221020110413NOT DISCLOSED050213060537000IQ04I0770108040820110413NOT DISCLOSED0502050606500000IQ04I0780108020820110413NOT DISCLOSED0502050606500000IQ04I0790108300720110413NOT DISCLOSED0502050606300000IQ04I0800108260720110413NOT DISCLOSED05021506071000000IQ04I0810108150720110413NOT DISCLOSED050202060510000IQ04I0820108150720110413NOT DISCLOSED0502000606900000IQ04I0830108050720110413NOT DISCLOSED05021006041000IQ04I0840108110520110413NOT DISCLOSED050205060510000IQ04I0850108030520110413NOT DISCLOSED05020306071500000IQ04I0860108290420110413NOT DISCLOSED050210060510000IQ04I0870108260420110413NOT DISCLOSED0502510606300000IQ04I0880108130420110413NOT DISCLOSED05020206072000000IQ04I0890108120420110413NOT DISCLOSED05020206071500000IQ04I0900108111020100413NOT DISCLOSED05020206071600000IQ04I0910108081020100413NOT DISCLOSED05020206071600000IQ04I0920108120120100413NOT DISCLOSED050213060535766IQ04I0930108041220090413NOT DISCLOSED050213060537000ES0700105210102**\u003c/BureauResponseRaw\u003e\r\n\u003cBureauResponseXml\u003e\u003cCreditReport\u003e\u003cHeader\u003e\u003cSegmentTag\u003eTUEF\u003c/SegmentTag\u003e\u003cVersion\u003e12\u003c/Version\u003e\u003cReferenceNumber\u003e360073862\u003c/ReferenceNumber\u003e\u003cMemberCode\u003eNB67098899_C2C                \u003c/MemberCode\u003e\u003cSubjectReturnCode\u003e1\u003c/SubjectReturnCode\u003e\u003cEnquiryControlNumber\u003e002631309122\u003c/EnquiryControlNumber\u003e\u003cDateProcessed\u003e28092018\u003c/DateProcessed\u003e\u003cTimeProcessed\u003e170049\u003c/TimeProcessed\u003e\u003c/Header\u003e\u003cNameSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eN01\u003c/SegmentTag\u003e\u003cConsumerName1FieldLength\u003e07\u003c/ConsumerName1FieldLength\u003e\u003cConsumerName1\u003eKISHORE\u003c/ConsumerName1\u003e\u003cConsumerName2FieldLength\u003e12\u003c/ConsumerName2FieldLength\u003e\u003cConsumerName2\u003eMEKHJI PATEL\u003c/ConsumerName2\u003e\u003cConsumerName3FieldLength\u003e05\u003c/ConsumerName3FieldLength\u003e\u003cConsumerName3\u003ePATEL\u003c/ConsumerName3\u003e\u003cDateOfBirthFieldLength\u003e08\u003c/DateOfBirthFieldLength\u003e\u003cDateOfBirth\u003e17081986\u003c/DateOfBirth\u003e\u003cGenderFieldLength\u003e01\u003c/GenderFieldLength\u003e\u003cGender\u003e2\u003c/Gender\u003e\u003c/NameSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI01\u003c/SegmentTag\u003e\u003cIDType\u003e01\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e10\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003eBACPP6592D\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cIDSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eI02\u003c/SegmentTag\u003e\u003cIDType\u003e06\u003c/IDType\u003e\u003cIDNumberFieldLength\u003e12\u003c/IDNumberFieldLength\u003e\u003cIDNumber\u003e492403246555\u003c/IDNumber\u003e\u003c/IDSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT01\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e11\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e02048606018\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT02\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e14\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e211412345 EXT.\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e00\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT03\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e03\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cTelephoneSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eT04\u003c/SegmentTag\u003e\u003cTelephoneNumberFieldLength\u003e10\u003c/TelephoneNumberFieldLength\u003e\u003cTelephoneNumber\u003e9823413581\u003c/TelephoneNumber\u003e\u003cTelephoneType\u003e01\u003c/TelephoneType\u003e\u003c/TelephoneSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC01\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e19\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@GMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmailContactSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eC02\u003c/SegmentTag\u003e\u003cEmailIDFieldLength\u003e24\u003c/EmailIDFieldLength\u003e\u003cEmailID\u003eBERABAZAR@REDIFFMAIL.COM\u003c/EmailID\u003e\u003c/EmailContactSegment\u003e\u003cEmploymentSegment\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eE01\u003c/SegmentTag\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cDateReportedCertified\u003e31082018\u003c/DateReportedCertified\u003e\u003cOccupationCode\u003e04\u003c/OccupationCode\u003e\u003c/EmploymentSegment\u003e\u003cScoreSegment\u003e\u003cLength\u003e10\u003c/Length\u003e\u003cScoreName\u003eCIBILTUSCR\u003c/ScoreName\u003e\u003cScoreCardName\u003e01\u003c/ScoreCardName\u003e\u003cScoreCardVersion\u003e10\u003c/ScoreCardVersion\u003e\u003cScoreDate\u003e28092018\u003c/ScoreDate\u003e\u003cScore\u003e00819\u003c/Score\u003e\u003cExclusionCode9FieldLength\u003e02\u003c/ExclusionCode9FieldLength\u003e\u003cExclusionCode9\u003e01\u003c/ExclusionCode9\u003e\u003cReasonCode1FieldLength\u003e02\u003c/ReasonCode1FieldLength\u003e\u003cReasonCode1\u003e10\u003c/ReasonCode1\u003e\u003c/ScoreSegment\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA01\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e09\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003ePUNE PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cDateReported\u003e16052018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA02\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e15\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eAUNDH CAMP PUNE\u003c/AddressLine1\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e02\u003c/AddressCategory\u003e\u003cResidenceCode\u003e01\u003c/ResidenceCode\u003e\u003cDateReported\u003e19032018\u003c/DateReported\u003e\u003cEnrichedThroughEnquiry\u003eY\u003c/EnrichedThroughEnquiry\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA03\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e40\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eFLAT NO. B-202 HERITAGE BUILDING OLD SAN\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e27\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eSANGAMNAGAR PUNE AUNDH CAMP\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e25\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003ePUNE NEAR NATIONAL SCHOOL\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e01\u003c/AddressCategory\u003e\u003cDateReported\u003e17102017\u003c/DateReported\u003e\u003c/Address\u003e\u003cAddress\u003e\u003cAddressSegmentTag\u003ePA\u003c/AddressSegmentTag\u003e\u003cLength\u003e03\u003c/Length\u003e\u003cSegmentTag\u003eA04\u003c/SegmentTag\u003e\u003cAddressLine1FieldLength\u003e39\u003c/AddressLine1FieldLength\u003e\u003cAddressLine1\u003eS NO 4 1 62 CTM 3117 SHREE HERITAGE 2ND\u003c/AddressLine1\u003e\u003cAddressLine2FieldLength\u003e35\u003c/AddressLine2FieldLength\u003e\u003cAddressLine2\u003eFLOOR FLAT NO B 202 SANGAM NAGAR NR\u003c/AddressLine2\u003e\u003cAddressLine3FieldLength\u003e38\u003c/AddressLine3FieldLength\u003e\u003cAddressLine3\u003eNATIONAL SCHOOL OLD SANGVI PUNE 411027\u003c/AddressLine3\u003e\u003cStateCode\u003e27\u003c/StateCode\u003e\u003cPinCodeFieldLength\u003e06\u003c/PinCodeFieldLength\u003e\u003cPinCode\u003e411027\u003c/PinCode\u003e\u003cAddressCategory\u003e04\u003c/AddressCategory\u003e\u003cDateReported\u003e31032015\u003c/DateReported\u003e\u003c/Address\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT001\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27062018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e23092018\u003c/DateReportedAndCertified\u003e\u003cCurrentBalanceFieldLength\u003e04\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e2084\u003c/CurrentBalance\u003e\u003cAmountOverdueFieldLength\u003e03\u003c/AmountOverdueFieldLength\u003e\u003cAmountOverdue\u003e100\u003c/AmountOverdue\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e007000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072018\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT002\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10052018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e191540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e09\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052018\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT003\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e10\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e0000000566\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e320000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e302154\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e15\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000XXX\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e16.00\u003c/RateOfInterest\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13156\u003c/EmiAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT004\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e50\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e28032018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e30062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e273198\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e12\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003eSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032018\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT005\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e10012018\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e28766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-50000\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01012018\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e30000\u003c/CreditLimit\u003e\u003cCashLimitFieldLength\u003e04\u003c/CashLimitFieldLength\u003e\u003cCashLimit\u003e6000\u003c/CashLimit\u003e\u003cActualPaymentAmountFieldLength\u003e05\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e50000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT006\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e13122017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e500000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e427540\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e24\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT007\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e01\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e24102017\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e848000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e727988\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e33\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01102017\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT008\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e07072017\u003c/DateOpenedOrDisbursed\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e21\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01022018\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e00\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e20.00\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e12\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9263\u003c/EmiAmount\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e4000\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT009\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e27092016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e14122017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e14122017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e450000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e48\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01122017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT010\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e05\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eKUDOS\u003c/ReportingMemberShortName\u003e\u003cAccountNumberFieldLength\u003e04\u003c/AccountNumberFieldLength\u003e\u003cAccountNumber\u003e1164\u003c/AccountNumber\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e08062016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e01012017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e27032017\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31072017\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e300000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e36\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000XXXXXXXXXXXXXXX000XXXXXX000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01072017\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082016\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT011\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e51\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30052016\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e05082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e415000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e132429\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e30\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052016\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e36\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e16065\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e06\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e417690\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT012\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31082015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e27092016\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e28092016\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092016\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e301000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e39\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092016\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT013\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e03\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e06052015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e700000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e646788\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328800\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e14.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e04\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e9797\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT014\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10052015\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31052015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e07\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e1095574\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e03\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01052015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052015\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT015\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11032015\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10082018\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e31082018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1100000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e06\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e987560\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092015\u003c/PaymentHistoryEndDate\u003e\u003cValueOfCollateralFieldLength\u003e07\u003c/ValueOfCollateralFieldLength\u003e\u003cValueOfCollateral\u003e2328000\u003c/ValueOfCollateral\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cRateOfInterestFieldLength\u003e05\u003c/RateOfInterestFieldLength\u003e\u003cRateOfInterest\u003e11.85\u003c/RateOfInterest\u003e\u003cRepaymentTenureFieldLength\u003e03\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e178\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e13202\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT016\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31072014\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31082015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02092015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30092015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e225000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e42\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000XXX000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistoryStartDate\u003e01092015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082014\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT017\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e30072013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e21052015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10122015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e31122015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e31844\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e02\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000085086053086056025026055086056\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e36\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e055026000026023000000XXX000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01122015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072013\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e15000\u003c/CreditLimit\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e5310\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT018\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e52\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e3\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e20022013\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e23102017\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e20022018\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e724500\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e4\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000STDXXX030030000000000000XXX000STDXXX000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000XXXXXX000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072015\u003c/PaymentHistoryEndDate\u003e\u003cRepaymentTenureFieldLength\u003e02\u003c/RepaymentTenureFieldLength\u003e\u003cRepaymentTenure\u003e60\u003c/RepaymentTenure\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e27168\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT019\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e05\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e25072012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e31072014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e02082014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30082014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e06\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e150000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e21\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01082014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01092011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT020\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e02\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e4\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e11042012\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e10042015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e10042015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30042015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e07\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e1200000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000STDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003eSTDSTDSTDSTDSTDSTDSTDSTDXXXSTDSTDSTDSTDSTDSTDSTDSTDSTD\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01042015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01052012\u003c/PaymentHistoryEndDate\u003e\u003cTypeOfCollateralFieldLength\u003e02\u003c/TypeOfCollateralFieldLength\u003e\u003cTypeOfCollateral\u003e01\u003c/TypeOfCollateral\u003e\u003cEmiAmountFieldLength\u003e05\u003c/EmiAmountFieldLength\u003e\u003cEmiAmount\u003e14286\u003c/EmiAmount\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT021\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e24112014\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e24112014\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30112014\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000026000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01112014\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01122011\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT022\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e31102011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e29062015\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e29062015\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e30062015\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e37000\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000148117086056025025000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01062015\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01072012\u003c/PaymentHistoryEndDate\u003e\u003cPaymentFrequency\u003e03\u003c/PaymentFrequency\u003e\u003cActualPaymentAmountFieldLength\u003e04\u003c/ActualPaymentAmountFieldLength\u003e\u003cActualPaymentAmount\u003e3037\u003c/ActualPaymentAmount\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT023\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e10\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e01062011\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e28082017\u003c/DateOfLastPayment\u003e\u003cDateReportedAndCertified\u003e02072018\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e63871\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e05\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e-1765\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01072018\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01082015\u003c/PaymentHistoryEndDate\u003e\u003cCreditLimitFieldLength\u003e05\u003c/CreditLimitFieldLength\u003e\u003cCreditLimit\u003e68700\u003c/CreditLimit\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cAccount\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eT024\u003c/SegmentTag\u003e\u003cAccount_Summary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003c/Account_Summary_Segment_Fields\u003e\u003cAccount_NonSummary_Segment_Fields\u003e\u003cReportingMemberShortNameFieldLength\u003e13\u003c/ReportingMemberShortNameFieldLength\u003e\u003cReportingMemberShortName\u003eNOT DISCLOSED\u003c/ReportingMemberShortName\u003e\u003cAccountType\u003e13\u003c/AccountType\u003e\u003cOwenershipIndicator\u003e1\u003c/OwenershipIndicator\u003e\u003cDateOpenedOrDisbursed\u003e21012010\u003c/DateOpenedOrDisbursed\u003e\u003cDateOfLastPayment\u003e06022013\u003c/DateOfLastPayment\u003e\u003cDateClosed\u003e06022013\u003c/DateClosed\u003e\u003cDateReportedAndCertified\u003e28022013\u003c/DateReportedAndCertified\u003e\u003cHighCreditOrSanctionedAmountFieldLength\u003e05\u003c/HighCreditOrSanctionedAmountFieldLength\u003e\u003cHighCreditOrSanctionedAmount\u003e35766\u003c/HighCreditOrSanctionedAmount\u003e\u003cCurrentBalanceFieldLength\u003e01\u003c/CurrentBalanceFieldLength\u003e\u003cCurrentBalance\u003e0\u003c/CurrentBalance\u003e\u003cPaymentHistory1FieldLength\u003e54\u003c/PaymentHistory1FieldLength\u003e\u003cPaymentHistory1\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory1\u003e\u003cPaymentHistory2FieldLength\u003e54\u003c/PaymentHistory2FieldLength\u003e\u003cPaymentHistory2\u003e000000000000000000000000000000000000000000000000000000\u003c/PaymentHistory2\u003e\u003cPaymentHistoryStartDate\u003e01022013\u003c/PaymentHistoryStartDate\u003e\u003cPaymentHistoryEndDate\u003e01032010\u003c/PaymentHistoryEndDate\u003e\u003c/Account_NonSummary_Segment_Fields\u003e\u003c/Account\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI001\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28092018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI002\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e13000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI003\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e08\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI004\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI005\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17062018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI006\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI007\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI008\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI009\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08052018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e218000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI010\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03042018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI011\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI012\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e60000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI013\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e28032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI014\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e266000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI015\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI016\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI017\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e52\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI018\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e20032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI019\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e19032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e01\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI020\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16032018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI021\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05022018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI022\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05012018\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI023\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI024\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e14112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI025\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e100000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI026\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI027\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04112017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI028\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI029\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16102017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e01\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e838000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI030\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI031\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e61\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI032\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI033\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12052017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI034\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI035\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI036\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI037\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11042017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI038\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21032017\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI039\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI040\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e09092016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI041\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI042\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI043\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12082016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI044\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24062016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI045\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI046\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26052016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI047\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31012016\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI048\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05082015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI049\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05052015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI050\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI051\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25022015\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI052\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI053\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e06\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI054\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25042014\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI055\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12092013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI056\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24082013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI057\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e25072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e50000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI058\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e17072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI059\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e16072013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI060\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e06032013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI061\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12022013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI062\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02012013\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI063\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e27112012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e800000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI064\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e24092012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1400000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI065\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e31072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI066\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI067\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10072012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI068\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10052012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI069\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e21042012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e05\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eKUDOS\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e200000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI070\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e700000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI071\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e10032012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI072\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI073\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04012012\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI074\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02112011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI075\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI076\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e22102011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI077\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI078\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e02082011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI079\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e30072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI080\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e15\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI081\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI082\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e15072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e00\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e900000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI083\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e05072011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e04\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI084\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e05\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI085\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e03052011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e03\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI086\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e29042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e10\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e10000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI087\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e26042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e51\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e06\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e300000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI088\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e13042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e2000000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI089\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12042011\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1500000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI090\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e11102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI091\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e08102010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e02\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e07\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e1600000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI092\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e12012010\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e35766\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnquiry\u003e\u003cLength\u003e04\u003c/Length\u003e\u003cSegmentTag\u003eI093\u003c/SegmentTag\u003e\u003cDateOfEnquiryFields\u003e04122009\u003c/DateOfEnquiryFields\u003e\u003cEnquiringMemberShortNameFieldLength\u003e13\u003c/EnquiringMemberShortNameFieldLength\u003e\u003cEnquiringMemberShortName\u003eNOT DISCLOSED\u003c/EnquiringMemberShortName\u003e\u003cEnquiryPurpose\u003e13\u003c/EnquiryPurpose\u003e\u003cEnquiryAmountFieldLength\u003e05\u003c/EnquiryAmountFieldLength\u003e\u003cEnquiryAmount\u003e37000\u003c/EnquiryAmount\u003e\u003c/Enquiry\u003e\u003cEnd\u003e\u003cSegmentTag\u003eES07\u003c/SegmentTag\u003e\u003cTotalLength\u003e0010521\u003c/TotalLength\u003e\u003c/End\u003e\u003c/CreditReport\u003e\u003c/BureauResponseXml\u003e\r\n\u003cSecondaryReportXml\u003e\u003cRoot\u003e\u003c/Root\u003e\u003c/SecondaryReportXml\u003e\r\n\u003cIsSucess\u003eTrue\u003c/IsSucess\u003e\r\n\u003c/CibilBureauResponse\u003e\r\n\u003c/Response\u003e\r\n\u003c/DsCibilBureau\u003e\r\n\u003c/Applicant\u003e\r\n\u003c/Applicants\u003e\r\n\u003c/Field\u003e\u003cField key=\""ApplicationData\""\u003e\u003cApplicationData\u003e\r\n\u003cFormattedReport\u003eFalse\u003c/FormattedReport\u003e\r\n\u003cMFIBranchReferenceNo\u003e\r\n\u003c/MFIBranchReferenceNo\u003e\r\n\u003cMFICenterReferenceNo\u003e\r\n\u003c/MFICenterReferenceNo\u003e\r\n\u003cMFILoanPurpose\u003e40\u003c/MFILoanPurpose\u003e\r\n\u003cMFIEnquiryAmount\u003e10000000\u003c/MFIEnquiryAmount\u003e\r\n\u003cConsumerConsentForUIDAIAuthentication\u003eY\u003c/ConsumerConsentForUIDAIAuthentication\u003e\r\n\u003cNTCProductType\u003eAL\u003c/NTCProductType\u003e\r\n\u003cMFIBureauFlag\u003eFALSE\u003c/MFIBureauFlag\u003e\r\n\u003cIDVerificationFlag\u003eFALSE\u003c/IDVerificationFlag\u003e\r\n\u003cDSTuNtcFlag\u003eFALSE\u003c/DSTuNtcFlag\u003e\r\n\u003cCibilBureauFlag\u003eFALSE\u003c/CibilBureauFlag\u003e\r\n\u003cPassword\u003elbW+juakg0\u003c/Password\u003e\r\n\u003cMemberCode\u003eNB67098899_C2C\u003c/MemberCode\u003e\r\n\u003cGSTStateCode\u003e27\u003c/GSTStateCode\u003e\r\n\u003cScoreType\u003e01\u003c/ScoreType\u003e\r\n\u003cAmount\u003e100000\u003c/Amount\u003e\r\n\u003cPurpose\u003e10\u003c/Purpose\u003e\r\n\u003cUser\u003eKUDOS_Dev_User\u003c/User\u003e\r\n\u003cBusinessUnitId\u003e410\u003c/BusinessUnitId\u003e\r\n\u003cApplicationId\u003e360073862\u003c/ApplicationId\u003e\r\n\u003cSolutionSetId\u003e2081\u003c/SolutionSetId\u003e\r\n\u003cEnvironmentTypeId\u003e2\u003c/EnvironmentTypeId\u003e\r\n\u003cEnvironmentType\u003eProduction\u003c/EnvironmentType\u003e\r\n\u003cMilestone\u003e\r\n\u003cStep\u003eCapture Quick Application Data\u003c/Step\u003e\r\n\u003cStep\u003eCIBIL Bureau call\u003c/Step\u003e\r\n\u003cStep\u003eIDVision Score\u003c/Step\u003e\r\n\u003cStep\u003eDomain Verification\u003c/Step\u003e\r\n\u003c/Milestone\u003e\r\n\u003cStart\u003e2018-09-28T11:30:48.5782469Z\u003c/Start\u003e\r\n\u003cInputValReasonCodes\u003e\r\n\u003c/InputValReasonCodes\u003e\r\n\u003cDTTrail\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eCapture Quick Application Data\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eCIBIL Bureau call\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:01.3260023\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eIDVision Score\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003cStep\u003e\r\n\u003cName\u003eDomain Verification\u003c/Name\u003e\r\n\u003cDuration\u003e00:00:00.0156001\u003c/Duration\u003e\r\n\u003c/Step\u003e\r\n\u003c/DTTrail\u003e\r\n\u003c/ApplicationData\u003e\r\n\u003c/Field\u003e\u003cField key=\""Decision\""\u003ePass\u003c/Field\u003e\u003c/ContextData\u003e\u003c/DCResponse\u003e""}";
                    try
                    {
                        vRtnXML = oAuth.SendCIBIL(strFinalXML);
                    }
                    catch (Exception ex)
                    { gblFuction.AjxMsgPopup(ex.Message); }

                    //RootObjectResponse ror = new RootObjectResponse();

                    RootObjectResponse ror = JsonConvert.DeserializeObject<RootObjectResponse>(vRtnXML);

                    vRtnXML = ror.Data;


                    if (Convert.ToBoolean(ror.Result) == true)
                    {
                        Int32 vErr = 0;
                        string vResult = "", vstatus = "", vReportId = "", vScoreValue = "";
                        XmlDocument xd = new XmlDocument();
                        xd.LoadXml(vRtnXML);
                        XmlNodeList xnStatus = xd.SelectNodes("/DCResponse/Status");
                        foreach (XmlNode xn in xnStatus)
                        {
                            vstatus = xn.InnerText;
                        }
                        if (vstatus == "Success")
                        {
                            XmlNodeList xnList = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Decision']");
                            foreach (XmlNode xn in xnList)
                            {
                                vResult = xn.InnerText;
                            }
                            if (vResult == "Pass")
                            {

                                // Get ReportId from Response XML
                                XmlNodeList XnReport = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Applicants']/Applicants/Applicant/DsCibilBureau/Response/CibilBureauResponse/BureauResponseXml/CreditReport/Header/EnquiryControlNumber");
                                foreach (XmlNode xn in XnReport)
                                {
                                    vReportId = xn.InnerText;
                                }
                                // Get ScoreValue from Response XML
                                XmlNodeList XnScoreValue = xd.SelectNodes("/DCResponse/ContextData/Field[@key='Applicants']/Applicants/Applicant/DsCibilBureau/Response/CibilBureauResponse/BureauResponseXml/CreditReport/ScoreSegment/Score");
                                foreach (XmlNode xn in XnScoreValue)
                                {
                                    vScoreValue = xn.InnerText;
                                }
                                string vErrDesc = "";
                                vErr = CMem.CibilXMLPurser(Session[gblValue.BrnchCode].ToString(), Convert.ToInt32(hdUserID.Value), gblFuction.setDate(Session[gblValue.LoginDate].ToString()), vRtnXML, ref vErrDesc);
                                if (vErr == 0)
                                {
                                    // gblFuction.AjxMsgPopup("Success");
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup(vErrDesc);
                                    return;
                                }
                                // Save Respone XML
                                vErr = CMem.SaveCIBILResponseXML(vReportId, vRtnXML);
                                if (vErr == 0)
                                {
                                    //
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Response XML Save Failed..");
                                    return;
                                }

                                // Update LoanAppCoApp Relation Table with ReportId and Score

                                vErr = CMem.UpdateLnAppCoApp(pLnAppId, pCoappId, vReportId, vScoreValue);
                                if (vErr == 0)
                                {
                                    //
                                }
                                else
                                {
                                    gblFuction.AjxMsgPopup("Loan Application and Co-Applicant Relation Update Failed..");
                                    return;
                                }

                                string BrCode = Session[gblValue.BrnchCode].ToString();
                                ShowInitialLoanAppData(pLnAppId, BrCode);
                                ResetCIBILGrid();
                                LoadCBOnlineList(pLnAppId);
                            }
                            else
                            {
                                gblFuction.AjxMsgPopup("Failed");
                                return;
                            }
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Not Success");
                            return;
                        }


                    }
                    else
                    {
                        gblFuction.AjxMsgPopup("failed by Cibil");
                        return;
                    }
                }
            }
        }
        protected void ResetCIBILGrid()
        {
            gvCBEnqMstDtl.DataSource = null;
            gvCBEnqMstDtl.DataBind();
            gvCBAddress.DataSource = null;
            gvCBAddress.DataBind();
            gvCBAccounts.DataSource = null;
            gvCBAccounts.DataBind();
            gvCBEnqDtl.DataSource = null;
            gvCBEnqDtl.DataBind();
        }
        protected void gvOnlineCBList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vBrCode = Session[gblValue.BrnchCode].ToString();


            string vReportID = "";
            vReportID = Convert.ToString(e.CommandArgument);
            if (e.CommandName == "cmdShowCBOnlineRec")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnShowInfo");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnShowInfo");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    ResetCIBILGrid();
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;

                }
                GetCBOnlineRecord(vReportID);

            }
            else if (e.CommandName == "cmdExcel")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnDwnldExcel");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnDwnldExcel");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;
                }
                DowloadOnlineCBRecord(vReportID);
            }
            else if (e.CommandName == "cmdExport")
            {
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnShow = (LinkButton)gvRow.FindControl("btnExport");
                System.Drawing.Color backColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                System.Drawing.Color foreColor = System.Drawing.ColorTranslator.FromHtml("#333333");
                foreach (GridViewRow gr in gvOnlineCBList.Rows)
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
                    LinkButton lb = (LinkButton)gr.FindControl("btnExport");
                    lb.ForeColor = System.Drawing.Color.Black;
                    lb.Font.Bold = false;
                }
                gvRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#33C7FF");
                gvRow.ForeColor = System.Drawing.Color.White;
                gvRow.Font.Bold = true;
                btnShow.ForeColor = System.Drawing.Color.White;
                btnShow.Font.Bold = true;
                if (string.IsNullOrEmpty(e.CommandArgument.ToString()) == true)
                {
                    gblFuction.AjxMsgPopup("No CB Record Found...");
                    return;
                }
                ExportCBOnlineRecord(vReportID);
                string vLnAppNo = gvRow.Cells[1].Text.ToString().Trim();
                LoadCBList(vLnAppNo);
            }
        }
        protected void GetCBOnlineRecord(string pReportId)
        {
            DataSet ds = new DataSet();
            DataTable dtApp = new DataTable();
            DataTable dtAdd = new DataTable();
            DataTable dtAccSum = new DataTable();
            DataTable dtEnqSum = new DataTable();
            DataTable dtAcc = new DataTable();
            DataTable dtEnqDtl = new DataTable();


            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineRecord(pReportId);
                if (ds.Tables.Count > 0)
                {
                    dtApp = ds.Tables[0];
                    dtAdd = ds.Tables[1];
                    dtAccSum = ds.Tables[2];
                    dtEnqSum = ds.Tables[3];
                    dtAcc = ds.Tables[4];
                    dtEnqDtl = ds.Tables[5];
                    // Applicant
                    if (dtApp.Rows.Count > 0)
                    {
                        gvCBEnqMstDtl.DataSource = dtApp;
                        gvCBEnqMstDtl.DataBind();
                    }
                    else
                    {
                        gvCBEnqMstDtl.DataSource = null;
                        gvCBEnqMstDtl.DataBind();
                    }
                    // Address
                    if (dtAdd.Rows.Count > 0)
                    {
                        gvCBAddress.DataSource = dtAdd;
                        gvCBAddress.DataBind();
                    }
                    else
                    {
                        gvCBAddress.DataSource = null;
                        gvCBAddress.DataBind();
                    }
                    // Account Summary
                    if (dtAccSum.Rows.Count > 0)
                    {
                        gvAccSummary.DataSource = dtAccSum;
                        gvAccSummary.DataBind();
                    }
                    else
                    {
                        gvAccSummary.DataSource = null;
                        gvAccSummary.DataBind();
                    }
                    // Enquiry Summary
                    if (dtEnqSum.Rows.Count > 0)
                    {
                        gvEnqSummary.DataSource = dtEnqSum;
                        gvEnqSummary.DataBind();
                    }
                    else
                    {
                        gvEnqSummary.DataSource = null;
                        gvEnqSummary.DataBind();
                    }
                    // Account Details
                    if (dtAcc.Rows.Count > 0)
                    {
                        gvCBAccounts.DataSource = dtAcc;
                        gvCBAccounts.DataBind();
                    }
                    else
                    {
                        gvCBAccounts.DataSource = null;
                        gvCBAccounts.DataBind();
                    }
                    // Enquiry Details
                    if (dtEnqDtl.Rows.Count > 0)
                    {
                        gvCBEnqDtl.DataSource = dtEnqDtl;
                        gvCBEnqDtl.DataBind();
                    }
                    else
                    {
                        gvCBEnqDtl.DataSource = null;
                        gvCBEnqDtl.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtApp = null;
                dtAdd = null;
                dtAccSum = null;
                dtEnqSum = null;
                dtAcc = null;
                dtEnqDtl = null;
                oMem = null;
            }

        }
        protected void DowloadOnlineCBRecord(string pReportId)
        {
            DataSet ds = new DataSet();
            DataTable dtApp = new DataTable();
            DataTable dtAdd = new DataTable();
            DataTable dtAccSum = new DataTable();
            DataTable dtEnqSum = new DataTable();
            DataTable dtAcc = new DataTable();
            DataTable dtEnqDtl = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineRecord(pReportId);
                if (ds.Tables.Count > 0)
                {
                    dtApp = ds.Tables[0];
                    dtAdd = ds.Tables[1];
                    dtAccSum = ds.Tables[2];
                    dtEnqSum = ds.Tables[3];
                    dtAcc = ds.Tables[4];
                    dtEnqDtl = ds.Tables[5];


                    string vFileNm = "";
                    System.Web.UI.WebControls.DataGrid DataGrid1 = new System.Web.UI.WebControls.DataGrid();
                    string vTitle = "Online CB Record";
                    vFileNm = "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "_OnlineCBReport.xls";
                    Response.ClearContent();
                    Response.AddHeader("content-disposition", vFileNm);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";

                    HttpContext.Current.Response.Write("<style> .txt" + "\r\n" + "{mso-style-parent:style0;mso-number-format:\"" + @"\@" + "\"" + ";} " + "\r\n" + "</style>");
                    Response.Write("<table border='1' cellpadding='0' width='100%'>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='5'>" + gblValue.CompName + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress1(vBrCode) + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><font size='3'>" + CGblIdGenerator.GetBranchAddress2(vBrCode) + " </font></b></td></tr>");
                    //Response.Write("<tr><td align=center' colspan='" + dtAdd.Columns.Count + "'><b><font size='3'> Branch - " + vBrCode + " - " + vBranch + " </font></b></td></tr>");
                    Response.Write("<tr><td align=center' colspan='" + dtAcc.Columns.Count + "'><b><u><font size='3'>  " + vTitle + "</font></u></b></td></tr>");

                    int TotCol = dtAcc.Columns.Count;


                    // For Applicant
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtApp.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtApp.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtApp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtApp.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtApp.Columns.Count; j++)
                            {
                                if (dtApp.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "AadhaarNo")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "TelephoneNumber")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "ScoreValue")
                                {
                                    Response.Write("<td nowrap class='txt'  style='text-align: right;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "DateOfRequest")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtApp.Columns[j].ColumnName == "CustDOB")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtApp.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");
                    // For Address
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAdd.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtAdd.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtAdd.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAdd.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAdd.Columns.Count; j++)
                            {
                                if (dtAdd.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtAdd.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Accounts Summary
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAccSum.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtAccSum.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtAccSum.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAccSum.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAccSum.Columns.Count; j++)
                            {
                                if (dtAccSum.Columns[j].ColumnName == "Recent Date")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Oldest Date")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Total Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Over Due Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAccSum.Columns[j].ColumnName == "Zero Balance Accounts")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtAccSum.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }

                    }
                    Response.Write("<tr></tr>");

                    // For Enquiry Summary
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtEnqSum.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtEnqSum.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtEnqSum.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEnqSum.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtEnqSum.Columns.Count; j++)
                            {
                                if (dtEnqSum.Columns[j].ColumnName == "Recent")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Total Enquiry")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 30 Days")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 12 Months")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqSum.Columns[j].ColumnName == "Past 24 Months")
                                {
                                    Response.Write("<td nowrap style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtEnqSum.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Accounts

                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtAcc.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("</tr>");
                    if (dtAcc.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtAcc.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtAcc.Columns.Count; j++)
                            {
                                if (dtAcc.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistory1")
                                {
                                    Response.Write("<td nowrap class='txt' style='width:300px;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistory2")
                                {
                                    Response.Write("<td nowrap class='txt' style='width:300px;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateOpenedOrDisbursed")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateOfLastPayment")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateClosed")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "DateReportedAndCertified")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistoryStartDate")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "PaymentHistoryEndDate")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;' >" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Account Type")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "WrittenOffAndSettled")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Ownership Type")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtAcc.Columns[j].ColumnName == "Active/Close")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("<tr></tr>");

                    // For Enquiry Details
                    Response.Write("<tr>");
                    foreach (DataColumn dtCol in dtEnqDtl.Columns)
                    {
                        Response.Write("<td style='text-align: center;color: White;background-color:#0897F6;'><b>" + dtCol.ColumnName + "<b></td>");
                    }
                    Response.Write("<td align=center' colspan='" + (TotCol - dtEnqDtl.Columns.Count) + "'></td>");
                    Response.Write("</tr>");
                    if (dtEnqDtl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtEnqDtl.Rows)
                        {
                            Response.Write("<tr style='height:20px;'>");
                            for (int j = 0; j < dtEnqDtl.Columns.Count; j++)
                            {
                                if (dtEnqDtl.Columns[j].ColumnName == "ReportID")
                                {
                                    Response.Write("<td nowrap class='txt'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqDtl.Columns[j].ColumnName == "Date Of Enquiry")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else if (dtEnqDtl.Columns[j].ColumnName == "Enquiry Purpose")
                                {
                                    Response.Write("<td nowrap class='txt' style='text-align: center;'>" + Convert.ToString(dr[j]) + "</td>");
                                }
                                else
                                {
                                    Response.Write("<td nowrap >" + Convert.ToString(dr[j]) + "</td>");
                                }
                            }
                            Response.Write("<td align=center' colspan='" + (TotCol - dtEnqDtl.Columns.Count) + "'></td>");
                            Response.Write("</tr>");
                        }
                    }
                    Response.Write("</table>");

                    Response.Flush();
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtApp = null;
                dtAdd = null;
                dtAccSum = null;
                dtEnqSum = null;
                dtAcc = null;
                dtEnqDtl = null;
                oMem = null;
            }
        }
        protected void ExportCBOnlineRecord(string pReportId)
        {
            CMember OMem = new CMember();
            Int32 vErr = 0;
            try
            {
                vErr = OMem.ExportCBOnlineRecord(pReportId, Convert.ToInt32(hdUserID.Value), 0);
                if (vErr == 1)
                {
                    gblFuction.AjxMsgPopup("Export to Credit Bureau Successfully Done");
                    return;
                }
                else
                {
                    gblFuction.AjxMsgPopup("Data Not Exported,Data Error");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
            }
        }
        private void LoadCBOnlineList(string pApplicationId)
        {
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable();
            CMember oMem = new CMember();
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vLogDt = gblFuction.setDate(Session[gblValue.LoginDate].ToString());
            try
            {
                ds = oMem.GetCBOnlineList(pApplicationId);
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        gvOnlineCBList.DataSource = dt1;
                        gvOnlineCBList.DataBind();
                    }
                    else
                    {
                        gvOnlineCBList.DataSource = null;
                        gvOnlineCBList.DataBind();
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
        private void GetRatio(string pLoanAppId)
        {
            string BrCode = Session[gblValue.BrnchCode].ToString();
            CMember Omem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = Omem.GetRatio(pLoanAppId);
                if (dt.Rows.Count > 0)
                {
                    gvRatio.DataSource = dt;
                    gvRatio.DataBind();
                }
                else
                {
                    gvRatio.DataSource = null;
                    gvRatio.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Omem = null;
                dt = null;
            }
        }
        double totDebit = 0;
        double totCredit = 0;
        double totTrans = 0;
        double totSales = 0;
        double TotMOS = 0;
        double AvgMOS = 0;
        double NoofRecord = 0;
        double TotCreditSum = 0;
        double TotAvgUlilization = 0;
        double AvgAvgUlilization = 0;
        protected void gvBankDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // SetTabIndexes();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDebit = (TextBox)e.Row.FindControl("txtDebit");
                if (txtDebit.Text != "")
                    totDebit += Double.Parse(txtDebit.Text);
                TextBox txtCredit = (TextBox)e.Row.FindControl("txtCredit");
                if (txtCredit.Text != "")
                    totCredit += Double.Parse(txtCredit.Text);
                TextBox txtTransfer = (TextBox)e.Row.FindControl("txtTransfer");
                if (txtTransfer.Text != "")
                    totTrans += Double.Parse(txtTransfer.Text);
                TextBox txtSalesFig = (TextBox)e.Row.FindControl("txtSalesFig");
                if (txtSalesFig.Text != "")
                    totSales += Double.Parse(txtSalesFig.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotDebit = (Label)e.Row.FindControl("lblTotDebit");
                lblTotDebit.Text = totDebit.ToString();
                Label lblTotCredit = (Label)e.Row.FindControl("lblTotCredit");
                lblTotCredit.Text = totCredit.ToString();
                Label lblTotTrans = (Label)e.Row.FindControl("lblTotTrans");
                lblTotTrans.Text = totTrans.ToString();
                Label lblTotSales = (Label)e.Row.FindControl("lblTotSales");
                lblTotSales.Text = totSales.ToString();
            }
        }
        protected void gvAcDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // SetTabIndexes();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblDebit = (Label)e.Row.FindControl("lblDebit");
                if (lblDebit.Text != "")
                    totDebit += Double.Parse(lblDebit.Text);
                Label lblCredit = (Label)e.Row.FindControl("lblCredit");
                if (lblCredit.Text != "")
                    totCredit += Double.Parse(lblCredit.Text);
                Label lblTransfer = (Label)e.Row.FindControl("lblTransfer");
                if (lblTransfer.Text != "")
                    totTrans += Double.Parse(lblTransfer.Text);
                Label lblSalesFig = (Label)e.Row.FindControl("lblSalesFig");
                if (lblSalesFig.Text != "")
                    totSales += Double.Parse(lblSalesFig.Text);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotDebit = (Label)e.Row.FindControl("lblTotDebit");
                lblTotDebit.Text = totDebit.ToString();
                Label lblTotCredit = (Label)e.Row.FindControl("lblTotCredit");
                lblTotCredit.Text = totCredit.ToString();
                Label lblTotTrans = (Label)e.Row.FindControl("lblTotTrans");
                lblTotTrans.Text = totTrans.ToString();
                Label lblTotSales = (Label)e.Row.FindControl("lblTotSales");
                lblTotSales.Text = totSales.ToString();
            }
        }
        protected void gvBankMOS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ViewState["Records"].ToString() != string.Empty)
                    NoofRecord = Convert.ToDouble(ViewState["Records"]);
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblMOS = (Label)e.Row.FindControl("lblPOS");
                    Label lblCreditSum = (Label)e.Row.FindControl("lblCreditSum");
                    Label lblAvgUtili = (Label)e.Row.FindControl("lblAvgUtili");
                    if (lblMOS.Text != "")
                        TotMOS += Double.Parse(lblMOS.Text);
                    if (lblCreditSum.Text != "")
                        TotCreditSum += Double.Parse(lblCreditSum.Text);
                    if (lblAvgUtili.Text != "")
                        TotAvgUlilization += Double.Parse(lblAvgUtili.Text);
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (ViewState["Records"].ToString() != string.Empty)
                    {
                        // Calculation of average MOS Accounwise
                        AvgMOS = Math.Round((TotMOS / NoofRecord), 2);
                        Label lbAveragePOS = (Label)e.Row.FindControl("lbAveragePOS");
                        lbAveragePOS.Text = AvgMOS.ToString("0.00");

                        // Calculation of Total Credit Summation Accounwise
                        Label lblTotCreditSum = (Label)e.Row.FindControl("lblTotCreditSum");
                        lblTotCreditSum.Text = TotCreditSum.ToString("0.00");

                        // Calculation of Average of Average Utilization

                        AvgAvgUlilization = Math.Round((TotAvgUlilization / NoofRecord), 2);
                        Label lblAvgAvgUtili = (Label)e.Row.FindControl("lblAvgAvgUtili");
                        lblAvgAvgUtili.Text = AvgAvgUlilization.ToString("0.00");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SetTabIndexes()
        {
            short currentTabIndex = 0;
            //txtBankAcRemarks.TabIndex = ++currentTabIndex;

            foreach (GridViewRow gvr in gvBankDtl.Rows)
            {
                TextBox txtDate = (TextBox)gvr.FindControl("txtDate");
                txtDate.TabIndex = ++currentTabIndex;

                TextBox txtCalDebit = (TextBox)gvr.FindControl("txtCalDebit");
                txtCalDebit.TabIndex = ++currentTabIndex;

                TextBox txtCalCredit = (TextBox)gvr.FindControl("txtCalCredit");
                txtCalCredit.TabIndex = ++currentTabIndex;

                TextBox txtTransfer = (TextBox)gvr.FindControl("txtTransfer");
                txtTransfer.TabIndex = ++currentTabIndex;

                TextBox txtSalesFig = (TextBox)gvr.FindControl("txtSalesFig");
                txtSalesFig.TabIndex = ++currentTabIndex;

                TextBox txtLoanAvailed = (TextBox)gvr.FindControl("txtLoanAvailed");
                txtLoanAvailed.TabIndex = ++currentTabIndex;


                TextBox txtDay5 = (TextBox)gvr.FindControl("txtDay5");
                txtDay5.TabIndex = ++currentTabIndex;

                TextBox txtDay10 = (TextBox)gvr.FindControl("txtDay10");
                txtDay10.TabIndex = ++currentTabIndex;
                TextBox txtDay15 = (TextBox)gvr.FindControl("txtDay15");
                txtDay15.TabIndex = ++currentTabIndex;
                TextBox txtDay20 = (TextBox)gvr.FindControl("txtDay20");
                txtDay20.TabIndex = ++currentTabIndex;
                TextBox txtDay25 = (TextBox)gvr.FindControl("txtDay25");
                txtDay25.TabIndex = ++currentTabIndex;
            }

            //someLinkAfterGridView.TabIndex = ++currentTabIndex;
        }

        double liabilty = 0;
        double asset = 0;
        double income = 0;
        double expense = 0;
        double netproamt = 0;
        protected void gvLiability_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtAmt");
                Label id = (Label)e.Row.FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);
                if (p == 5 || p == 6)
                {
                    if (txt.Text == "")
                        liabilty = liabilty + 0;
                    else
                        liabilty = liabilty - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        liabilty = liabilty + 0;
                    else
                        liabilty = liabilty + Convert.ToDouble(txt.Text);
                }

            }
            txtTotLiability.Text = liabilty.ToString("0.00");
        }
        protected void gvAsset_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtAssetAmt");
                Label id = (Label)e.Row.FindControl("lblParticularsId");
                int p = Convert.ToInt32(id.Text);

                if (txt.Text == "")
                    asset = asset + 0;
                else
                    asset = asset + Convert.ToDouble(txt.Text);
            }
            txtTotAsset.Text = asset.ToString("0.00");
        }

        protected void gvIncome_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtIncome");
                Label id = (Label)e.Row.FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (p == 8)
                {
                    if (txt.Text != "")
                        income = income - Convert.ToDouble(txt.Text);
                }
                else
                {
                    if (txt.Text == "")
                        income = income + 0;
                    else
                        income = income + Convert.ToDouble(txt.Text);
                }
            }
            txtTotIncome.Text = income.ToString("0.00");
            if (txtTotExpense.Text != "")
                expense = Convert.ToDouble(txtTotExpense.Text);
            netproamt = (income - expense);
            txtNetProfit.Text = netproamt.ToString("0.00");
        }
        protected void gvExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txt = (TextBox)e.Row.FindControl("txtExpense");
                Label id = (Label)e.Row.FindControl("lblParticularsIdPL");
                int p = Convert.ToInt32(id.Text);
                if (txt.Text == "")
                    expense = expense + 0;
                else
                    expense = expense + Convert.ToDouble(txt.Text);
            }
            txtTotExpense.Text = expense.ToString("0.00");
            if (txtTotIncome.Text != "")
                income = Convert.ToDouble(txtTotIncome.Text);
            netproamt = (income - expense);
            txtNetProfit.Text = netproamt.ToString("0.00");
        }
        protected void btnModifyPL_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            try
            {
                if (txtModPLDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Profit/Loss Date can not be left blank...");
                    return;
                }
                else
                {
                    string StrDD, StrMM, StrYYYY;
                    string pDate = txtModPLDate.Text.ToString();
                    StrDD = pDate.Substring(0, 2);
                    StrMM = pDate.Substring(3, 2);
                    StrYYYY = pDate.Substring(6, 4);
                    if (StrDD != "31")
                    {
                        gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                        return;
                    }
                    else if (StrMM != "03")
                    {
                        gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                        return;
                    }
                }
                string vAppID = ViewState["LoanAppId"].ToString();
                DateTime vPLDate = gblFuction.setDate(txtModPLDate.Text);

                ds = oMem.GetPLDtlbyPLId(vAppID, vPLDate);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2];
                    dt3 = ds.Tables[3];
                }
                if (dt.Rows.Count > 0)
                {
                    mView.ActiveViewIndex = 7;
                    ViewAcess();
                    hfPLId.Value = Convert.ToString(dt.Rows[0]["PLId"]).Trim();
                    btnSavePL.Enabled = false;
                    btnUpdatePL.Enabled = true;
                    btnDelPL.Enabled = true;
                    ViewState["StateEdit"] = "Edit";
                    txtAppIdPL.Text = Convert.ToString(dt.Rows[0]["AppId"]).Trim();
                    txtPLDate.Text = Convert.ToString(dt.Rows[0]["PLDate"]).Trim();
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                    txtModPLDate.Text = "";
                }
                if (dt1.Rows.Count > 0)
                {
                    gvExpense.DataSource = dt1;
                    gvExpense.DataBind();
                }
                else
                {
                    gvExpense.DataSource = null;
                    gvExpense.DataBind();
                }
                if (dt2.Rows.Count > 0)
                {
                    hfPLBSDocUpId.Value = dt2.Rows[0]["DocUpID"].ToString();
                }
                if (dt3.Rows.Count > 0)
                {
                    gvITR.DataSource = dt3;
                    gvITR.DataBind();
                    ViewState["ITRDtl"] = dt3;
                }
                else
                {
                    gvITR.DataSource = null;
                    gvITR.DataBind();
                    popITRDetails();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oMem = null;
                ds = null;
                dt = null;
                dt1 = null;
                dt2 = null;
                dt3 = null;
            }
        }
        protected void btnModifyBS_Click(object sender, EventArgs e)
        {
            if (txtModBSData.Text == "")
            {
                gblFuction.AjxMsgPopup("Balance Sheet Date can not be left blank...");
                return;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtModBSData.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return;
                }
            }
            string vAppID = ViewState["LoanAppId"].ToString();
            DateTime vBSDate = gblFuction.setDate(txtModBSData.Text);
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            ds = oMem.GetBSDtlbyBSid(vAppID, vBSDate);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt1 = ds.Tables[1];
            }
            if (dt.Rows.Count > 0)
            {
                mView.ActiveViewIndex = 6;
                ViewAcess();
                hfBSId.Value = Convert.ToString(dt.Rows[0]["BSId"]).Trim();
                btnSaveBS.Enabled = false;
                btnUpdateBS.Enabled = true;
                btnDelBS.Enabled = true;
                ViewState["StateEdit"] = "Edit";
                txtAppNoBS.Text = Convert.ToString(dt.Rows[0]["AppId"]).Trim();
                txtBSDate.Text = Convert.ToString(dt.Rows[0]["BSDate"]).Trim();
                gvLiability.DataSource = dt;
                gvLiability.DataBind();
                txtModBSData.Text = "";
            }
            if (dt1.Rows.Count > 0)
            {
                gvAsset.DataSource = dt1;
                gvAsset.DataBind();
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
                ViewAcess();
                ClearCoApplicant();
                PopCoAppCompanyType();
                PopCoAppPropertyType();
                PopCustomer();
                ddlCustName.SelectedValue = ViewState["CusTID"].ToString();
                PopReligion();
                PopCaste();
                PopMaritalStatus();
                PopGender();
                PopRelation();
                PopOccupation();
                PopQualification();
                popIDProof();
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
                    gblFuction.AjxMsgPopup("Kindly Login To " + vCustBrName.ToString() + " Branch For Loan Application as Customer Is Created From Same Branch...");
                    return;
                }
                ClearApplication();
                mView.ActiveViewIndex = 1;
                txtAppDt.Text = Session[gblValue.LoginDate].ToString();
                txtLnAppPassDt.Text = Session[gblValue.LoginDate].ToString();
                ViewAcess();
                PopApplicant();
                ddlLoanApplicantname.SelectedValue = ViewState["CusTID"].ToString();
                ddlLoanApplicantname_SelectedIndexChanged(sender, e);
                //  ddlLoanApplicantname.Enabled = false;
                popSourceName();
                PopPurpose();
                PopLoanType();
                popMLAsset();
                btSaveApplication.Enabled = true;
                btnUpdateApplication.Enabled = false;
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
                    mView.ActiveViewIndex = 8;
                    ViewAcess();
                    ViewState["LoanAppId"] = dt.Rows[0]["LoanAppId"].ToString();
                    txtAppNoRef.Text = ViewState["LoanAppId"].ToString();
                    ClearRefControl();
                    PopRelation();
                    popRefDetails();
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
        protected void btnAddCB_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Credit Bureau Information..");
                    return;
                }
                else
                {
                    dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["LnAppPassYN"].ToString() != "Y")
                        {
                            gblFuction.MsgPopup("You Can Not Add CB Record as This Application remains in Hold/Rejected/Cancelled State in  Loan Application Stage...");
                            return;
                        }
                        mView.ActiveViewIndex = 4;
                        ViewAcess();
                        txtLnAppCB.Text = ViewState["LoanAppId"].ToString();
                        popCBDetails();
                        PopCBName();
                        btnUpdateCB.Enabled = false;
                        btnDeleteCB.Enabled = false;
                        btnSaveCB.Enabled = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please Select Pass in Loan Application Approval Section...");
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
                oMem = null;
            }
        }
        protected void btnAddBankAc_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtCBPass = new DataTable();
            DataTable dtSlNo = new DataTable();
            Int32 MaxSlNo = 0;
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Bank Account");
                    return;
                }
                else
                {

                    txtAppIdBankAc.Text = ViewState["LoanAppId"].ToString();
                    dtCBPass = oMem.GetCBPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtCBPass.Rows.Count > 0)
                    {
                        if (dtCBPass.Rows[0]["CBPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Bank Statement Record as Credit Bureau remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick CB Pass/Reject Checkbox to add Bank Statement Record... ");
                        return;
                    }
                    dtSlNo = oMem.GetMaxBankAcSlNo(ViewState["LoanAppId"].ToString());
                    if (dtSlNo.Rows.Count > 0)
                    {
                        MaxSlNo = Convert.ToInt32(dtSlNo.Rows[0]["MaxSlNo"]) + 1;
                    }
                    txtBankAcSrNo.Text = MaxSlNo.ToString();
                    mView.ActiveViewIndex = 5;
                    ViewAcess();
                    btnBankAcUpdate.Enabled = false;
                    btnDeleteBankAc.Enabled = false;
                    btnBankAcSave.Enabled = true;
                    popBankDetails();
                    popBankIWOWDetails();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtCBPass = null;
                oMem = null;
            }
        }
        protected void btnAddAM_Click(object sender, EventArgs e)
        {
            DataTable dtFSPass = new DataTable();
            CMember OMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add CAM Record..");
                    return;
                }
                else
                {
                    // Branch Code Checking
                    string vLABrCode = "", vBrCode = "", vLABrName = "";
                    vBrCode = (string)Session[gblValue.BrnchCode];
                    GetBrCodeByLnAppId(ViewState["LoanAppId"].ToString());
                    if (hdLABrCode.ToString() != "")
                    {
                        vLABrCode = hdLABrCode.Value.ToString();
                        vLABrName = hdLABrName.Value.ToString();
                    }
                    if (vLABrCode != vBrCode)
                    {
                        gblFuction.AjxMsgPopup("Kindly Login To " + vLABrName.ToString() + " Branch For CAM Entry as Loan Application Done From Same Branch...");
                        return;
                    }


                    txtAppNoAM.Text = ViewState["LoanAppId"].ToString();
                    dtFSPass = OMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtFSPass.Rows.Count > 0)
                    {
                        if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add CAM Record as Financial Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to add CAM Record... ");
                        return;
                    }
                    mView.ActiveViewIndex = 9;
                    ViewAcess();
                    btnUpdateCAM.Enabled = false;
                    btnDelAM.Enabled = false;
                    btnSaveCAM.Enabled = true;
                    popAMQuestion();
                    popAMValues();
                    GetLoanAppDtlByLnAppId(ViewState["LoanAppId"].ToString());
                    GetCAMCIBILRec(ViewState["LoanAppId"].ToString());
                    GetCAMBankAcRec(ViewState["LoanAppId"].ToString());
                    GetFinStatRecordForCAM(ViewState["LoanAppId"].ToString());
                    string st = ddlCAMLnScheme.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dtFSPass = null;
                OMem = null;
            }
        }
        protected void GetLoanAppDtlByLnAppId(string LoanID)
        {
            CApplication OApp = new CApplication();
            DataTable dt = new DataTable();
            try
            {
                dt = OApp.GetLoanAppDtlLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    PopPurpose();
                    PopLoanType();
                    ddlLnPur.SelectedIndex = ddlLnPurpose.Items.IndexOf(ddlLnPurpose.Items.FindByValue(Convert.ToString(dt.Rows[0]["PurposeID"])));
                    ddlCAMLnScheme.SelectedIndex = ddlCAMLnScheme.Items.IndexOf(ddlCAMLnScheme.Items.FindByValue(Convert.ToString(dt.Rows[0]["LoanTypeId"])));
                    AssementMatrixEnable(Convert.ToInt32(dt.Rows[0]["LoanTypeId"]));
                    txtLnAmt.Text = dt.Rows[0]["AppAmount"].ToString();
                    txtAppTenure.Text = dt.Rows[0]["Tenure"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OApp = null;
                dt = null;
            }
        }
        protected void GetCBRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetCBRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfCBRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtCBPOsRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtCBNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtCBMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    txtCIBILScore.Text = dt.Rows[0]["CIBILScore"].ToString();
                    txtCIBILScoreDate.Text = dt.Rows[0]["CIBILScoreDate"].ToString();
                    if (dt.Rows[0]["CBPassYN"].ToString() == "Y")
                        chkCBPass.Checked = true;
                    else
                        chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = dt.Rows[0]["CBPassorRejDate"].ToString();
                    txtCBRejReason.Text = dt.Rows[0]["CBRejReason"].ToString();
                    btnSaveCBRemark.Enabled = false;
                    btnUpdateCBRemark.Enabled = true;
                }
                else
                {
                    hfCBRemarkID.Value = "";
                    txtCBPOsRemark.Text = "";
                    txtCBNegRemark.Text = "";
                    txtCBMiti.Text = "";
                    txtCIBILScore.Text = "";
                    chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = "";
                    txtCBRejReason.Text = "";
                    btnSaveCBRemark.Enabled = true;
                    btnUpdateCBRemark.Enabled = false;
                    chkCBPass.Checked = false;
                    txtCBPassRejDate.Text = "";
                    txtCBRejReason.Text = "";
                    txtCIBILScoreDate.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void GetBSRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetBSRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfBSRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtBSPosRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtBSNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtBSMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    if (dt.Rows[0]["BSPassYN"].ToString() == "Y")
                        chkBSPass.Checked = true;
                    else
                        chkBSPass.Checked = false;
                    txtBSPassRejDate.Text = dt.Rows[0]["BSPassorRejDate"].ToString();
                    txtBSRejReason.Text = dt.Rows[0]["BSRejReason"].ToString();
                    btnSaveBSRemark.Enabled = false;
                    btnUpdateBSRemark.Enabled = true;
                }
                else
                {
                    hfBSRemarkID.Value = "";
                    txtBSPosRemark.Text = "";
                    txtBSNegRemark.Text = "";
                    txtBSMiti.Text = "";
                    chkBSPass.Checked = true;
                    txtBSPassRejDate.Text = "";
                    txtBSRejReason.Text = "";
                    btnSaveBSRemark.Enabled = true;
                    btnUpdateBSRemark.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void GetFSRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetFSRemarksByLoanId(LoanID);
                if (dt.Rows.Count > 0)
                {
                    hfFSRemarkID.Value = dt.Rows[0]["RemarksID"].ToString();
                    txtFSPosRemark.Text = dt.Rows[0]["Positive"].ToString();
                    txtFSNegRemark.Text = dt.Rows[0]["Negetive"].ToString();
                    txtFSMiti.Text = dt.Rows[0]["Mitigation"].ToString();
                    if (dt.Rows[0]["FSPassYN"].ToString() == "Y")
                        chkFSPass.Checked = true;
                    else
                        chkFSPass.Checked = false;
                    txtFSPassRejDate.Text = dt.Rows[0]["FSPassorRejDate"].ToString();
                    txtFSRejReason.Text = dt.Rows[0]["FSRejReason"].ToString();
                    btnSaveFSRemark.Enabled = false;
                    btnUpdateFSRemark.Enabled = true;
                }
                else
                {
                    hfFSRemarkID.Value = "";
                    txtFSPosRemark.Text = "";
                    txtFSNegRemark.Text = "";
                    txtFSMiti.Text = "";
                    txtFSPassRejDate.Text = "";
                    txtFSRejReason.Text = "";
                    chkFSPass.Checked = false;
                    btnSaveFSRemark.Enabled = true;
                    btnUpdateFSRemark.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void GetCAMRemarkByLnAppId(string LoanID)
        {
            CMember OMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dtCM = new DataTable();
            DataTable dtCB = new DataTable();
            DataTable dtBS = new DataTable();
            DataTable dtFS = new DataTable();
            DataTable dtPD1 = new DataTable();
            DataTable dtPD2 = new DataTable();
            try
            {
                ds = OMem.GetCAMRemarksByLoanId(LoanID);
                if (ds.Tables.Count > 0)
                {
                    dtCM = ds.Tables[0];
                    dtCB = ds.Tables[1];
                    dtBS = ds.Tables[2];
                    dtFS = ds.Tables[3];
                    dtPD1 = ds.Tables[4];
                    dtPD2 = ds.Tables[5];
                    // Populate CAM Remarks Section.....
                    if (dtCM.Rows.Count > 0)
                    {
                        hfCAMRemarkID.Value = dtCM.Rows[0]["RemarksID"].ToString();
                        txtCAMPosRemark.Text = dtCM.Rows[0]["Positive"].ToString();
                        txtCAMNegRemark.Text = dtCM.Rows[0]["Negetive"].ToString();
                        txtCAMMiti.Text = dtCM.Rows[0]["Mitigation"].ToString();
                        if (dtCM.Rows[0]["CAMPassYN"].ToString() == "Y")
                            chkCAMPass.Checked = true;
                        else
                            chkCAMPass.Checked = false;
                        txtCAMPassRejDt.Text = dtCM.Rows[0]["CAMPassorRejDate"].ToString();
                        txtCAMRejRemark.Text = dtCM.Rows[0]["CAMRejReason"].ToString();
                        btnSaveCAMRemark.Enabled = false;
                        btnUpdateCAMRemark.Enabled = true;
                    }
                    else
                    {
                        chkCAMPass.Checked = false;
                        txtCAMPassRejDt.Text = "";
                        txtCAMRejRemark.Text = "";
                        hfCAMRemarkID.Value = "";
                        txtCAMPosRemark.Text = "";
                        txtCAMNegRemark.Text = "";
                        txtCAMMiti.Text = "";
                        btnSaveCAMRemark.Enabled = true;
                        btnUpdateCAMRemark.Enabled = false;
                    }
                    //Populate Credit Bureau Remarks Section... 
                    if (dtCB.Rows.Count > 0)
                    {
                        hfCAMCBRemarkID.Value = dtCB.Rows[0]["RemarksID"].ToString();
                        txtCBPosPoint.Text = dtCB.Rows[0]["Positive"].ToString();
                        txtCBNegPoint.Text = dtCB.Rows[0]["Negetive"].ToString();
                        txtCBMitiPoint.Text = dtCB.Rows[0]["Mitigation"].ToString();
                        txtCAMCIBILScore.Text = dtCB.Rows[0]["CIBILScore"].ToString();
                        txtCAMCIBILScoreDate.Text = dtCB.Rows[0]["CIBILScoreDate"].ToString();
                        btnCAMCBUpdateRemark.Enabled = true;
                    }
                    else
                    {
                        hfCAMCBRemarkID.Value = "";
                        txtCBPosPoint.Text = "";
                        txtCBNegPoint.Text = "";
                        txtCBMitiPoint.Text = "";
                        txtCAMCIBILScore.Text = "";
                        txtCAMCIBILScoreDate.Text = "";
                        btnCAMCBUpdateRemark.Enabled = false;
                    }
                    //Populate Bank Statement Remarks Section... 
                    if (dtBS.Rows.Count > 0)
                    {
                        hfCAMBSRemarkID.Value = dtBS.Rows[0]["RemarksID"].ToString();
                        txtBSPosPoint.Text = dtBS.Rows[0]["Positive"].ToString();
                        txtBSNegPoint.Text = dtBS.Rows[0]["Negetive"].ToString();
                        txtBSMitiPoint.Text = dtBS.Rows[0]["Mitigation"].ToString();
                        btnCAMBSUpdateRemark.Enabled = true;
                    }
                    else
                    {
                        hfCAMBSRemarkID.Value = "";
                        txtBSPosPoint.Text = "";
                        txtBSNegPoint.Text = "";
                        txtBSMitiPoint.Text = "";
                        btnCAMBSUpdateRemark.Enabled = false;
                    }
                    //Populate Financial Statement Remarks Section... 
                    if (dtFS.Rows.Count > 0)
                    {
                        hfCAMFSRemarkID.Value = dtFS.Rows[0]["RemarksID"].ToString();
                        txtFSPosPoint.Text = dtFS.Rows[0]["Positive"].ToString();
                        txtFSNegPoint.Text = dtFS.Rows[0]["Negetive"].ToString();
                        txtFSMitiPoint.Text = dtFS.Rows[0]["Mitigation"].ToString();
                        btnCAMFSUpdateRemark.Enabled = true;
                    }
                    else
                    {
                        hfCAMFSRemarkID.Value = "";
                        txtFSPosPoint.Text = "";
                        txtFSNegPoint.Text = "";
                        txtFSMitiPoint.Text = "";
                        btnCAMFSUpdateRemark.Enabled = false;
                    }
                    //Populate PD1 Approval Section... 
                    if (dtPD1.Rows.Count > 0)
                    {
                        hfPD1ApprovalId.Value = dtPD1.Rows[0]["RemarksID"].ToString();
                        if (dtPD1.Rows[0]["PD1PassYN"].ToString() == "Y")
                            chkPD1Pass.Checked = true;
                        else
                            chkPD1Pass.Checked = false;
                        txtPD1PassRejDt.Text = dtPD1.Rows[0]["PD1PassorRejDate"].ToString();
                        txtPD1RejReason.Text = dtPD1.Rows[0]["PD1RejReason"].ToString();
                        btnSavePD1Approval.Enabled = false;
                        btnUpdatePD1Approval.Enabled = true;
                    }
                    else
                    {
                        hfPD1ApprovalId.Value = "";
                        chkPD1Pass.Checked = false;
                        txtPD1PassRejDt.Text = "";
                        txtPD1RejReason.Text = "";
                        btnSavePD1Approval.Enabled = true;
                        btnUpdatePD1Approval.Enabled = false;
                    }

                    //Populate PD2 Approval Section... 
                    if (dtPD2.Rows.Count > 0)
                    {
                        hfPD2ApprovalId.Value = dtPD2.Rows[0]["RemarksID"].ToString();
                        if (dtPD2.Rows[0]["PD2PassYN"].ToString() == "Y")
                            chkPD2Pass.Checked = true;
                        else
                            chkPD2Pass.Checked = false;
                        txtPD2PassRejDt.Text = dtPD2.Rows[0]["PD2PassorRejDate"].ToString();
                        txtPD2RejReason.Text = dtPD2.Rows[0]["PD2RejReason"].ToString();
                        btnSavePD2Approval.Enabled = false;
                        btnUpdatePD2Approval.Enabled = true;
                    }
                    else
                    {
                        hfPD2ApprovalId.Value = "";
                        chkPD2Pass.Checked = false;
                        txtPD2PassRejDt.Text = "";
                        txtPD2RejReason.Text = "";
                        btnSavePD2Approval.Enabled = true;
                        btnUpdatePD2Approval.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dtCM = null;
                dtCB = null;
                dtBS = null;
                dtFS = null;
                dtPD1 = null;
                dtPD2 = null;
            }
        }
        protected void btnAddCom_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 3;
            ViewAcess();
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
            vStatus = Convert.ToString((Request[txtStatus.UniqueID] as string == null) ? txtStatus.Text : Request[txtStatus.UniqueID] as string);
            vVarifiedBy = Convert.ToString((Request[txtVarifiedby.UniqueID] as string == null) ? txtVarifiedby.Text : Request[txtVarifiedby.UniqueID] as string);
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
        protected void btnAddFinStat_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataTable dtBSPass = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Financial Data");
                    return;
                }
                else
                {
                    txtAppNoBS.Text = ViewState["LoanAppId"].ToString();
                    dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtBSPass.Rows.Count > 0)
                    {
                        if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Financial Statement as Bank Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add Financial Statement... ");
                        return;
                    }
                    mView.ActiveViewIndex = 6;
                    ViewAcess();
                    ClearBSControl();
                    GetComGLForBalShet();
                    //  popBSDetails();
                    // PopCompanyGL();
                    btnSaveBS.Enabled = true;
                    btnUpdateBS.Enabled = false;
                    btnDelBS.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt = null;
                dtBSPass = null;
                oMem = null;
            }
        }
        protected void btnAddPLStat_Click(object sender, EventArgs e)
        {
            DataTable dt1 = new DataTable();
            DataTable dtBSPass = new DataTable();
            CMember oMem = new CMember();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Financial Data");
                    return;
                }
                else
                {
                    txtAppIdPL.Text = ViewState["LoanAppId"].ToString();
                    dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtBSPass.Rows.Count > 0)
                    {
                        if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add PL Statement as Bank Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add PL Statement... ");
                        return;
                    }
                    mView.ActiveViewIndex = 7;
                    ViewAcess();
                    ClearPLControl();
                    GetComGLForPL();

                    // PopCompPLLed();
                    btnSavePL.Enabled = true;
                    btnUpdatePL.Enabled = false;
                    btnDelPL.Enabled = false;

                    dt1 = oMem.GetITRDetailsByLoanId(ViewState["LoanAppId"].ToString());
                    if (dt1.Rows.Count > 0)
                    {
                        gvITR.DataSource = dt1;
                        gvITR.DataBind();
                    }
                    else
                    {
                        popITRDetails();
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
                dtBSPass = null;
                oMem = null;
            }

        }
        protected void btnBackApplication_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 2;
            ViewAcess();

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
        protected void btnSaveCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveCBRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 3;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
            }
        }
        protected void btnUpdateCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCBRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
                hdfCBId.Value = "";
            }
        }
        protected void btnDeleteCB_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveCBRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 4;
                ViewAcess();
                LoadCBList(ViewState["LoanAppId"].ToString());
                gvCBDtl.DataSource = null;
                gvCBDtl.DataBind();
                ViewState["CBDtl"] = null;
                popCBDetails();
                hdfCBId.Value = "";
            }
        }
        protected void btnBackCB_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 4;
            ViewAcess();
            hdfCBId.Value = "";
        }
        protected void btnBackBS_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 6;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnBackPL_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 6;
            ViewAcess();
            ClearPLControl();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnSaveBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveBSRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                popBSDetails();
            }
        }
        protected void btnUpdateBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveBSRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                //  LoadCBList();
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                //LoadBSDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDelBS_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveBSRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                gvLiability.DataSource = null;
                gvLiability.DataBind();
                gvAsset.DataSource = null;
                gvAsset.DataBind();
                ViewState["BSDtl"] = null;
                //LoadBSDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());

            }
        }
        protected void btnSavePL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SavePLRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                // LoadPLDetails(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                popPLDetails();
                ClearPLControl();
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnUpdatePL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SavePLRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                //  LoadCBList();
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                ClearPLControl();
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDelPL_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SavePLRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 6;
                ViewAcess();
                gvIncome.DataSource = null;
                gvIncome.DataBind();
                ViewState["PLDtl"] = null;
                //LoadPLDetails(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnSaveCAM_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveCAMRecords(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCAMControl();
                LoadCAMList(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnUpdateCAM_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";

            if (SaveCAMRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCAMControl();
                LoadCAMList(ViewState["LoanAppId"].ToString());
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnSaveComProfile_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveComBackground(vStateEdit) == true)
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
                //mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCompanyBackground();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnUpdateComProfile_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveComBackground(vStateEdit) == true)
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
                //mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCompanyBackground();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDelComProfile_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveComBackground(vStateEdit) == true)
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
                // mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCompanyBackground();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }

        }
        protected void btnSaveGrpComInfo_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveGrpComInfo(vStateEdit) == true)
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
                // mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearGrpCompInfo();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnUpdateGrpComInfo_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveGrpComInfo(vStateEdit) == true)
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
                // mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearGrpCompInfo();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnDeleteGrpComInfo_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveGrpComInfo(vStateEdit) == true)
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
                // mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearGrpCompInfo();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
                //LoadCAMList(ViewState["LoanAppId"].ToString());
                // LoadRefList(ViewState["LoanAppId"].ToString());
            }

        }
        protected void btnSaveProBack_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SavePromoterBackground(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearPromoterBackground();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
            }

        }
        protected void btnSaveCBRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Credit Bureau Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveCBRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnUpdateCBRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Credit Bureau Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCBRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnCAMCBUpdateRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Credit Bureau Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCAMCBRemarks(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnSaveBSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Bank Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveBSRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnUpdateBSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Bank Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveBSRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnCAMBSUpdateRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Bank Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCAMBSRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnSaveFSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add Financial Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveFSRemarks(vStateEdit) == true)
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
                }
            }
        }
        protected void btnUpdateFSRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Financial Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveFSRemarks(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnCAMFSUpdateRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update Financial Statement Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCAMFSRemarks(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnSaveCAMRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Add CAM Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SaveCAMRemarks(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnUpdateCAMRemark_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update CAM Remarks..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SaveCAMRemarks(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnSavePD1Approval_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Save PD1 Approval..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SavePD1Approval(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnUpdatePD1Approval_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update PD1 Approval..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SavePD1Approval(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnSavePD2Approval_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Save PD1 Approval..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Save";
                if (SavePD2Approval(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnUpdatePD2Approval_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Update PD2 Approval..");
            }
            else
            {
                Label lblMsg = (Label)Master.FindControl("lblMsg");
                string vStateEdit = "Edit";
                if (SavePD2Approval(vStateEdit) == true)
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
                    LoadBSList(ViewState["LoanAppId"].ToString());
                    LoadPLList(ViewState["LoanAppId"].ToString());
                }
            }
        }
        protected void btnUpdateProBack_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SavePromoterBackground(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearPromoterBackground();
            }
        }
        protected void btnDelProBack_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SavePromoterBackground(vStateEdit) == true)
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
                // mView.ActiveViewIndex = 9;
                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearPromoterBackground();
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
                ViewAcess();
                LoadRefList(ViewState["LoanAppId"].ToString());
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
                ViewAcess();
                LoadRefList(ViewState["LoanAppId"].ToString());
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
                ViewAcess();
                LoadRefList(ViewState["LoanAppId"].ToString());
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
        protected void btnChkLsUpdate_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveCheckListDtl(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCheckList();
                LoadBSList(ViewState["LoanAppId"].ToString());
                LoadPLList(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnChkLsback_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 7;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnBankAcSave_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Save";
            if (SaveBankRecord(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 5;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                ClearBankAccControl();
                popBankDetails();
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());

                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnBankAcUpdate_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Edit";
            if (SaveBankRecord(vStateEdit) == true)
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
                //  mView.ActiveViewIndex = 0;
                //  tbMem.ActiveTabIndex = 4;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());

                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
        }
        protected void btnBackBankAc_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 5;
            ViewAcess();
            LoadBankAcSummary(ViewState["LoanAppId"].ToString());

        }
        protected void btnDeleteBankAc_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";
            if (SaveBankRecord(vStateEdit) == true)
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
                tbMem.ActiveTabIndex = 5;
                ViewAcess();
                LoadBankAcList(ViewState["LoanAppId"].ToString());
                ClearBankAccControl();
                popBankDetails();
                LoadBankAcSummary(ViewState["LoanAppId"].ToString());
                // Get Combine MOS
                GetCombineMOS(ViewState["LoanAppId"].ToString());
                // Get BankToTRRatio
                GetBankToTR(ViewState["LoanAppId"].ToString());
            }
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
                ViewAcess();
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
                ViewAcess();
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
                ViewAcess();
                ClearApplication();
                ShowAllInitialLoanApp(ViewState["CusTID"].ToString());
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnDelAM_Click(object sender, EventArgs e)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            string vStateEdit = "Delete";

            if (SaveCAMRecords(vStateEdit) == true)
            {
                if (vStateEdit == "Save")
                    lblMsg.Text = gblPRATAM.SaveMsg;
                else if (vStateEdit == "Edit")
                    lblMsg.Text = gblPRATAM.EditMsg;

                mView.ActiveViewIndex = 0;
                tbMem.ActiveTabIndex = 7;
                ViewAcess();
                ClearCAMControl();
                LoadCAMList(ViewState["LoanAppId"].ToString());
                //  LoadList();
                StatusButton("View");
                ViewState["StateEdit"] = null;
            }
        }
        protected void btnBackAM_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 7;
            ViewAcess();
            ClearCAMControl();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
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
                ViewAcess();
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
                ViewAcess();
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
                ViewAcess();
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
                ViewAcess();
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
            ViewAcess();
        }
        protected void btnBackCoApp_Click(object sender, EventArgs e)
        {
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            ViewAcess();
        }
        protected void btnBackCom_Click(object sender, EventArgs e)
        {
            ClearCompany();
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 1;
            ViewAcess();

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
            ViewAcess();
            ClearAppControlForShow();
            gvCoApp.DataSource = null;
            gvCoApp.DataBind();
            StatusButton("View");
        }
        private double Strength(string amvalue)
        {
            string[] st = amvalue.Replace("||", "|").Split('|');
            string strength = st[1].ToString();
            Double retstr = Convert.ToDouble(strength);
            return retstr;
        }
        private int AMValue(string amvalue)
        {
            string[] st = amvalue.Replace("||", "|").Split('|');
            string MValue = st[0].ToString();
            Int32 value = Convert.ToInt32(MValue);
            return value;
        }
        private Boolean SaveCoAppRecords(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            string vCoAppId = "", vCoAppType = "";
            Int32 vComTypeId = 0, vPropertyTypeId = 0, vCustCoAppRel = 0;
            string vCoAppNo = "", vCustId = "", vCoAppSamAdd = "N", vNewId = "", vGuarantor = "", vIsPrimaryCoApp = "";
            string vCoAppNm = "", vAppname = "";
            string vCoAppPreAdd1 = "", vCoAppPreAdd2 = "", vCoAppPreDist = "", vCoAppPreState = "", vCoAppPrePIN = "", vCoAppPerAdd1 = "", vCoAppPerAdd2 = "", vCoAppPerDist = "", vCoAppPerState = "", vCoAppPerPIN = "";

            Int32 vErr = 0, vCoAppPId = 0, vCoAppAId = 0, vRelationId = 0;
            string vCoAppANo = "", vCoAppPNo = "", vRelativeName = "";
            string vCoAppIsDir = "", vCoAppPart = "", vCoAppPropietor = "", vCoAppSpouse = "", vSameAddAsApp = "", vIsActive = "";
            string vCoAppMNo = "", vCoAppTelNo = "", vCoAppEmail = "", vCoAppFaxNo = "";

            Int32 vCoAppAge = 0, vCoApMarital = 0, vCoApCast = 0, vCoApOccuId = 0, vCoApRelig = 0, vCoApGen = 0, vCoAppQulId = 0, vCoAppYAR = 0, vCoAppYIB = 0;
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
                if (vCoAppAge <= 17)
                {
                    gblFuction.MsgPopup("Minimum Age Limit Of Co-Applicant/Guarantor is 18 yrs");
                    return false;
                }
                vCoApGen = Convert.ToInt32((Request[ddlCoAppGender.UniqueID] as string == null) ? ddlCoAppGender.SelectedValue : Request[ddlCoAppGender.UniqueID] as string);
                vCoApRelig = Convert.ToInt32((Request[ddlCoAppRel.UniqueID] as string == null) ? ddlCoAppRel.SelectedValue : Request[ddlCoAppRel.UniqueID] as string);
                vCoApCast = Convert.ToInt32((Request[ddlCoAppCast.UniqueID] as string == null) ? ddlCoAppCast.SelectedValue : Request[ddlCoAppCast.UniqueID] as string);
                vCoApOccuId = Convert.ToInt32((Request[ddlCoAppOccu.UniqueID] as string == null) ? ddlCoAppOccu.SelectedValue : Request[ddlCoAppOccu.UniqueID] as string);
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
                if (ddlCoAppPhotoIdType.SelectedValue == "-1")
                {
                    gblFuction.MsgPopup("Please Select Photo ID Type");
                    ddlCoAppPhotoIdType.Focus();
                    return false;
                }
                if (txtCoAppPNo.Text == "")
                {
                    gblFuction.MsgPopup("Photo ID No Can Not Be Blank...");
                    txtCoAppPNo.Focus();
                    return false;
                }
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
                vCoAppPreState = Convert.ToString((Request[txtCoAppStatePre.UniqueID] as string == null) ? txtCoAppStatePre.Text : Request[txtCoAppStatePre.UniqueID] as string);
                vCoAppPrePIN = Convert.ToString((Request[txtCOAppPINPre.UniqueID] as string == null) ? txtCOAppPINPre.Text : Request[txtCOAppPINPre.UniqueID] as string);
                if (chkCoAppAd.Checked == true)
                    vCoAppSamAdd = "Y";
                vCoAppPerAdd1 = Convert.ToString((Request[txtCoAppAddress1Per.UniqueID] as string == null) ? txtCoAppAddress1Per.Text : Request[txtCoAppAddress1Per.UniqueID] as string);
                vCoAppPerAdd2 = Convert.ToString((Request[txtCoAppAddress2Per.UniqueID] as string == null) ? txtCoAppAddress2Per.Text : Request[txtCoAppAddress2Per.UniqueID] as string);
                vCoAppPerDist = Convert.ToString((Request[txtCoAppDistPer.UniqueID] as string == null) ? txtCoAppDistPer.Text : Request[txtCoAppDistPer.UniqueID] as string);
                vCoAppPerState = Convert.ToString((Request[txtCoAppStatePer.UniqueID] as string == null) ? txtCoAppStatePer.Text : Request[txtCoAppStatePer.UniqueID] as string);
                vCoAppPerPIN = Convert.ToString((Request[txtCoAppPINPer.UniqueID] as string == null) ? txtCoAppPINPer.Text : Request[txtCoAppPINPer.UniqueID] as string);
                vCoAppAId = Convert.ToInt32((Request[ddlCoAppAddressIdType.UniqueID] as string == null) ? ddlCoAppAddressIdType.SelectedValue : Request[ddlCoAppAddressIdType.UniqueID] as string);


                vCoAppPId = Convert.ToInt32((Request[ddlCoAppPhotoIdType.UniqueID] as string == null) ? ddlCoAppPhotoIdType.SelectedValue : Request[ddlCoAppPhotoIdType.UniqueID] as string);
                vCoAppANo = Convert.ToString((Request[txtCoAppAddressIdNo.UniqueID] as string == null) ? txtCoAppAddressIdNo.Text : Request[txtCoAppAddressIdNo.UniqueID] as string);
                vCoAppPNo = Convert.ToString((Request[txtCoAppPNo.UniqueID] as string == null) ? txtCoAppPNo.Text : Request[txtCoAppPNo.UniqueID] as string);
                vCoAppMNo = Convert.ToString((Request[txtCoAppMNo.UniqueID] as string == null) ? txtCoAppMNo.Text : Request[txtCoAppMNo.UniqueID] as string);
                vCoAppTelNo = Convert.ToString((Request[txtCoAppTelNo.UniqueID] as string == null) ? txtCoAppTelNo.Text : Request[txtCoAppTelNo.UniqueID] as string);
                vCoAppEmail = Convert.ToString((Request[txtCoAppEmail.UniqueID] as string == null) ? txtCoAppEmail.Text : Request[txtCoAppEmail.UniqueID] as string);
                vCoAppFaxNo = Convert.ToString((Request[txtCoAppFaxNo.UniqueID] as string == null) ? txtCoAppFaxNo.Text : Request[txtCoAppFaxNo.UniqueID] as string);

                if (vCoAppPerAdd1 == "" || vCoAppPerDist == "" || vCoAppPerPIN == "")
                {
                    gblFuction.MsgPopup("Any of the field like Permanent Address 1,District,PIN can Not be blank....");
                    txtCoAppAddress1Per.Focus();
                    return false;
                }
                if (chkIsGuarantor.Checked == true)
                    vGuarantor = "Y";
                else
                    vGuarantor = "N";
                if (chkIsPrimaryCoApp.Checked == true)
                    vIsPrimaryCoApp = "Y";
                else
                    vIsPrimaryCoApp = "N";

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
                int YrInOpe = Convert.ToInt32(txtCAAcYrOfOpen.Text.ToString());
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
                        vMsg = oMem.GetMemBrByIDTypeNo(Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue), txtCoAppPNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Save");
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
                        vMsg = oMem.GetAddressIDNoByAddIDNo(Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue), txtCoAppAddressIdNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Save");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppPNo);
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

                    ////  if (ValidateCoAppField() == false) return false;
                    //vErr = oMem.SaveCoApplicant(ref vNewId, vCoAppId, ref vCoAppNo, vCustId, vCoAppDOA,
                    //    vCoAppNm, vCoAppDOB, vCoAppAge, vCoApGen, vCoApRelig, vCoApCast, vCoApOccuId, vCoApMarital, vCoAppQulId, vCoAppPreAdd1, vCoAppPreAdd2,
                    //    vCoAppPreDist, vCoAppPreState, vCoAppPrePIN, vCoAppSamAdd,
                    //    vCoAppPerAdd1, vCoAppPerAdd2, vCoAppPerDist, vCoAppPerState, vCoAppPerPIN, vCoAppAId, vCoAppANo, vCoAppPId, vCoAppPNo, vCoAppMNo,
                    //    vCoAppTelNo, vCoAppEmail,
                    //    vCoAppFaxNo, vCoAppIsDir, vCoAppPart, vCoAppPropietor, vCoAppSpouse, vSameAddAsApp,
                    //    vBrCode, vCoAppYAR, vCoAppYIB, Convert.ToInt32(hdUserID.Value), "Save", 0, vGuarantor, vIsActive, vShareHolPer,
                    //    vCoAppType, vComTypeId, vPropertyTypeId, vAppname, vIsPrimaryCoApp, vCustCoAppRel, vRelationId, vRelativeName, vXml,
                    //    pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                    //    pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                    //    pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pCommunAddress, pResidentialStatus,1,1);
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
                        vMsg = oMem.GetMemBrByIDTypeNo(Convert.ToInt32(ddlCoAppPhotoIdType.SelectedValue), txtCoAppPNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Edit");
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
                        vMsg = oMem.GetAddressIDNoByAddIDNo(Convert.ToInt32(ddlCoAppAddressIdType.SelectedValue), txtCoAppAddressIdNo.Text.Trim().ToString(), vBrCode, vCoAppId, "Edit");
                        if (vMsg != "")
                        {
                            gblFuction.MsgPopup(vMsg);
                            //tdMsg.Visible = true;
                            lblValMsgCoApp.Text = vMsg;
                            this.Page.SetFocus(txtCoAppAddressIdNo);
                            return false;
                        }
                    }

                    //vErr = oMem.SaveCoApplicant(ref vNewId, vCoAppId, ref vCoAppNo, vCustId, vCoAppDOA,
                    //    vCoAppNm, vCoAppDOB, vCoAppAge, vCoApGen, vCoApRelig, vCoApCast, vCoApOccuId, vCoApMarital, vCoAppQulId, vCoAppPreAdd1, vCoAppPreAdd2,
                    //    vCoAppPreDist, vCoAppPreState, vCoAppPrePIN, vCoAppSamAdd,
                    //    vCoAppPerAdd1, vCoAppPerAdd2, vCoAppPerDist, vCoAppPerState, vCoAppPerPIN, vCoAppAId, vCoAppANo, vCoAppPId, vCoAppPNo,
                    //    vCoAppMNo, vCoAppTelNo, vCoAppEmail, vCoAppFaxNo, vCoAppIsDir, vCoAppPart, vCoAppPropietor, vCoAppSpouse, vSameAddAsApp,
                    //    vBrCode, vCoAppYAR, vCoAppYIB, Convert.ToInt32(hdUserID.Value), "Edit", 0, vGuarantor,
                    //    vIsActive, vShareHolPer, vCoAppType, vComTypeId, vPropertyTypeId, vAppname, vIsPrimaryCoApp, vCustCoAppRel, vRelationId,
                    //    vRelativeName, vXml,
                    //     pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                    //    pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                    //    pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pCommunAddress, pResidentialStatus, 1, 1);

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
                //else if (Mode == "Delete")
                //{
                //    oMem = new CMember();
                //    oGbl = new CGblIdGenerator();
                //    vRec = oGbl.ChkDelete(vMemId, "MemberId", "LoanApplication");
                //    if (vRec > 0)
                //    {
                //        //gblFuction.AjxMsgPopup("Member has already applied for Loan ...Can't be Deleted");
                //        tdMsg.Visible = true;
                //        lblValMsg.Text = "Member has already applied for Loan ...Can't be Deleted";
                //        return false;
                //    }

                //    oCGT = new CCGT();
                //    vRec = oCGT.ChkCBAppr(vMemId, vBrCode);
                //    if (vRec > 0)
                //    {
                //        //gblFuction.AjxMsgPopup("CGT already Passed ...Can't be Deleted");
                //        tdMsg.Visible = true;
                //        lblValMsg.Text = "CGT already Passed ...Can't be Deleted";
                //        return false;
                //    }

                //    vErr = oMem.SaveApplicant(ref vNewId, vApplicantId, ref vApplicanNo, vAppDOA, vGrpId, vEOID, vSalutation, vFName, vMName, vLName,
                //          vApplicantname, vAppDOB, vAppAge, vGen, vRelig, vCast, vOccuId, vMarital, vQual, vPreAdd1, vPreAdd2, vPreDist, vPreState, vPrePIN, vSamAdd,
                //          vPerAdd1, vPerAdd2, vPerDist, vPerState, vPerPIN, vAppAddId, vAppAddNo, vAppPhotoId, vAppPhotoIdNo, vITTypeId,
                //          vAppITTypeIdNo, vContMNo, vContTelNo, vContEmail, vContFaxNo, vIsDir, vIsPart, vIsIndiv, vIsGOPAAcHold,
                //          vBrCode, vYrAtResApp, vYrinBusiApp, this.UserID, "Delete", 0,1,1);

                //    if (vErr > 0)
                //    {
                //        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                //        tdMsg.Visible = false;
                //        lblValMsg.Text = "";
                //        vResult = true;
                //    }
                //    else
                //    {
                //        //gblFuction.MsgPopup(gblPRATAM.DBError);
                //        tdMsg.Visible = true;
                //        lblValMsg.Text = gblPRATAM.DBError;
                //        vResult = false;
                //    }
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
            Int32 vErr = 0, vComAId = 0, vComPID = 0, pOccupationId = 0, pBusinessTypeId, pGenderId = 0, pAge = 0, vRelationId = 0;
            string vComPANNo = "", vComAIDNO = "", vComPIDNO = "", vIsRegister = "", vRegistNo = "", vGSTRegistNo = "", vRelativeName = "";
            Int32 vCompanyTypeId = 0, vSectorId = 0, vSubSectorId = 0, vPropertyTypeId = 0;
            string vMsg = "";
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
                if (pAge <= 20)
                {
                    gblFuction.MsgPopup("Customer Age Must be Above 20 yrs...");
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
                vMState = Convert.ToString((Request[txtMState.UniqueID] as string == null) ? txtMState.Text : Request[txtMState.UniqueID] as string);
                vMDistrict = Convert.ToString((Request[txtMDist.UniqueID] as string == null) ? txtMDist.Text : Request[txtMDist.UniqueID] as string);
                vMPIN = Convert.ToString((Request[txtMPIN.UniqueID] as string == null) ? txtMPIN.Text : Request[txtMPIN.UniqueID] as string);
                vMMobNo = Convert.ToString((Request[txtMMobNo.UniqueID] as string == null) ? txtMMobNo.Text : Request[txtMMobNo.UniqueID] as string);
                vMSTD = Convert.ToString((Request[txtMSTD.UniqueID] as string == null) ? txtMSTD.Text : Request[txtMSTD.UniqueID] as string);
                vMTelNo = Convert.ToString((Request[txtMTel.UniqueID] as string == null) ? txtMTel.Text : Request[txtMTel.UniqueID] as string);
                if (vMAddress1 == "" || vMState == "" || vMPIN == "")
                {
                    gblFuction.MsgPopup("Mailing Address1/State/PIN No Can not be Blank....");
                    txtMAdd1.Focus();
                    return false;
                }
                if (chkComSameAdd.Checked == true)
                    vComSamAdd = "Y";
                vRAddress1 = Convert.ToString((Request[txtROffAdd1.UniqueID] as string == null) ? txtROffAdd1.Text : Request[txtROffAdd1.UniqueID] as string);
                vRAddress2 = Convert.ToString((Request[txtROffAdd2.UniqueID] as string == null) ? txtROffAdd2.Text : Request[txtROffAdd2.UniqueID] as string);
                vRState = Convert.ToString((Request[txtRState.UniqueID] as string == null) ? txtRState.Text : Request[txtRState.UniqueID] as string);
                vRDistrict = Convert.ToString((Request[txtRDist.UniqueID] as string == null) ? txtRDist.Text : Request[txtRDist.UniqueID] as string);
                vRPIN = Convert.ToString((Request[txtRPIN.UniqueID] as string == null) ? txtRPIN.Text : Request[txtRPIN.UniqueID] as string);
                vRMobNo = Convert.ToString((Request[txtRMobNo.UniqueID] as string == null) ? txtRMobNo.Text : Request[txtRMobNo.UniqueID] as string);
                vRSTD = Convert.ToString((Request[txtRSTD.UniqueID] as string == null) ? txtRSTD.Text : Request[txtRSTD.UniqueID] as string);
                vRTelNo = Convert.ToString((Request[txtRTel.UniqueID] as string == null) ? txtRTel.Text : Request[txtRTel.UniqueID] as string);
                int pCustRelId = Convert.ToInt32((Request[ddlCustReligion.UniqueID] as string == null) ? ddlCustReligion.SelectedValue : Request[ddlCustReligion.UniqueID] as string);

                pOccupationId = Convert.ToInt32((Request[ddlCustOccupation.UniqueID] as string == null) ? ddlCustOccupation.SelectedValue : Request[ddlCustOccupation.UniqueID] as string);
                pBusinessTypeId = Convert.ToInt32((Request[ddlCustBusType.UniqueID] as string == null) ? ddlCustBusType.SelectedValue : Request[ddlCustBusType.UniqueID] as string);
                string pBusinessLocation = txtCustBusLocation.Text.ToString();
                decimal pAnnualIncome = 0;
                if (txtCustAnnualInc.Text != "")
                    pAnnualIncome = Convert.ToDecimal(txtCustAnnualInc.Text);
                pGenderId = Convert.ToInt32((Request[ddlCustGen.UniqueID] as string == null) ? ddlCustGen.SelectedValue : Request[ddlCustGen.UniqueID] as string);


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
                    // Check For Duplicate PAN No
                    dt = oMem.ChkDupPANNo(vComPANNo);
                    if (Convert.ToInt32(dt.Rows[0]["PANRec"]) > 0)
                    {
                        gblFuction.MsgPopup("PAN No already applied for Another Customer... PAN No Can not be duplicate...");
                        txtComPanNo.Focus();
                        return false;
                    }

                    // Check For Duplicate Address Id No
                    dt1 = oMem.ChkDupAddressIdNo(vComAIDNO);
                    if (Convert.ToInt32(dt1.Rows[0]["AddressIdNo"]) > 0)
                    {
                        gblFuction.MsgPopup("AddressId No already applied for Another Customer... It Can not be duplicate...");
                        txtComAddIDNo.Focus();
                        return false;
                    }
                    vErr = oMem.SaveCompanyNew(ref vNewId, vCompanyId, ref vCompanyNo, vCompanyTypeId, vCompanyName, vDOF, vDOB, vPropertyTypeId, vOtherPropertyDtls, vWebSite, vEmail,
                        vComPANNo, vComAId, vComAIDNO, vIsRegister, vRegistNo, vSectorId, vSubSectorId, vMAddress1, vMAddress2, vMState, vMDistrict,
                        vMPIN, vMMobNo, vMSTD, vMTelNo, vComSamAdd, vRAddress1, vRAddress2, vRState, vRDistrict, vRPIN, vRMobNo, vRSTD, vRTelNo,
                        vBrCode, Convert.ToInt32(hdUserID.Value), "Save", 0, vAppName, vAppContNo, vGSTRegistNo, vCustType,
                        vComPID, vComPIDNO, pOccupationId, pBusinessTypeId, pBusinessLocation, pAnnualIncome, pGenderId, pAge, vRelationId, vRelativeName,
                        pCustQuaId, pCustReligionId, pCustCasteId, pYrInRe, pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                        pConNo, pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml,1,1,"",0,"",-1,"N",-1,"N",
                        "",gblFuction.setDate("01/01/1900"),"","",-1,-1,-1);
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

                    CustomerId = vCompanyId;
                    //  if (ValidateCompanyFields() == false) return false;
                    oMem = new CMember();
                    vErr = oMem.SaveCompanyNew(ref vNewId, vCompanyId, ref vCompanyNo, vCompanyTypeId, vCompanyName, vDOF, vDOB, vPropertyTypeId, vOtherPropertyDtls, vWebSite, vEmail,
                        vComPANNo, vComAId, vComAIDNO, vIsRegister, vRegistNo, vSectorId, vSubSectorId, vMAddress1, vMAddress2, vMState, vMDistrict,
                        vMPIN, vMMobNo, vMSTD, vMTelNo, vComSamAdd, vRAddress1, vRAddress2, vRState, vRDistrict, vRPIN, vRMobNo, vRSTD, vRTelNo,
                        vBrCode, Convert.ToInt32(hdUserID.Value), "Edit", 0, vAppName, vAppContNo, vGSTRegistNo, vCustType,
                        vComPID, vComPIDNO, pOccupationId, pBusinessTypeId, pBusinessLocation, pAnnualIncome, pGenderId, pAge, vRelationId, vRelativeName,
                        pCustQuaId, pCustReligionId, pCustCasteId, pYrInRe, pPhyChallengedYN, pAcHolName, pACNo, pBankName, pIFSCCode, YrInOpe, pCustACType,
                        pConNo, pEmpOrgName, pEmpEmpDesig, pEmpRetAge, pEmpCode, pEmpCurExp, pEmpTotExp, chkGSTAppYN, pBusGSTNo, pBusLandMark, pBusAddress1, pBusAddress2,
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml, 1, 1, "", 0, "", -1, "N", -1, "N",
                         "", gblFuction.setDate("01/01/1900"), "", "", -1, -1, -1
                      );

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
                                FileExten = System.IO.Path.GetExtension(fuCustAddProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCustAddProofFront, CustomerId, "AddressImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCustAddProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCustAddProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCustAddProofBack, CustomerId, "AddressImageBack", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Address Proof Back Section");
                                    return false;
                                }
                            }

                            if (fuCustPhotProofFront.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCustPhotProofFront.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCustPhotProofFront, CustomerId, "PhotoImageFront", "Edit", "N", PathKYCImage);
                                }
                                else
                                {
                                    gblFuction.MsgPopup("Only Image file like .png,.jpg,.jpeg,.gif Can Be Uploaded in Photo Proof Front Section");
                                    return false;
                                }
                            }
                            if (fuCustPhotoProofBack.HasFile)
                            {
                                FileExten = System.IO.Path.GetExtension(fuCustPhotoProofBack.FileName).ToLower();
                                if (FileExten == ".png" || FileExten == ".jpg" || FileExten == ".jpeg" || FileExten == ".gif")
                                {
                                    string vMessage = SaveMemberImages(fuCustPhotoProofBack, CustomerId, "PhotoImageBack", "Edit", "N", PathKYCImage);
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
                        pBusLocality, pBusCity, pBusPIN, pBusState, pBusMob, pBusPhone, pMaritalStat, pCommunAddress, pResidentialStatus, vXml, 1, 1, "", 0, "", -1, "N", -1, "N",
                        "", gblFuction.setDate("01/01/1900"), "", "", -1, -1, -1
                       );

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
            string vStateEdit = string.Empty, vBrCode = string.Empty, vAppNo = string.Empty, vApplicantId = "", vAppId = "", vMachDtl = "", vErrDesc="",
                vAddTerms = "";
            Int32 vErr = 0, vPurpId = 0, vTenure = 0, vYrNo = 0, vLnTypeId = 0, vSourceId = 0;
            decimal vLnAmt = 0;

            CApplication oCG = new CApplication();
            string vXml = "", vXmlAsset = "", vPassYN = "", vRejReason = "";
            vBrCode = (string)Session[gblValue.BrnchCode];
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
                else if (txtTenure.Text == "")
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be Empty...");
                    txtTenure.Focus();
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
                foreach (GridViewRow gr in gvCoAppDtl.Rows)
                {
                    if (((CheckBox)gr.FindControl("chkCoApp")).Checked == true)
                    {
                        dr = dtXml.NewRow();
                        dr["CoApplicantId"] = ((Label)gr.FindControl("lblCoApplicantId")).Text;
                        dr["CoApplicantName"] = ((Label)gr.FindControl("lblCoAppName")).Text;
                        dr["ReportID"] = ((Label)gr.FindControl("lblReportID")).Text;
                        dr["ScoreValue"] = ((Label)gr.FindControl("lblScoreValue")).Text;
                        dtXml.Rows.Add(dr);
                        dtXml.AcceptChanges();
                    }
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
                if (Request[txtTenure.UniqueID] as string != "")
                    vTenure = Convert.ToInt32(Request[txtTenure.UniqueID] as string);
                if (vTenure == 0)
                {
                    gblFuction.AjxMsgPopup("Tenure Can Not Be zero...");
                    txtTenure.Focus();
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

                vStateEdit = Convert.ToString(ViewState["StateEdit"]);
                vAppId = hdfApplicationId.Value;
                vMachDtl = txtLnPurposeDetails.Text.ToString().Trim();


                if (Mode == "Save")
                {
                    //if (ValidateFieldsForLnApp() == false) return false;

                    vErr = oCG.SaveInitialApplication(ref vAppNo, vApplicantId, vAppDt, vPurpId, vLnAmt, vTenure, "N",
                      vBrCode, Convert.ToInt32(hdUserID.Value), "I", vYrNo, vLnTypeId, vMachDtl, vSourceId, vXml, vXmlAsset, vPassYN, vPassorRejDate,
                      vRejReason, vAddTerms, ref vErrDesc);
                    if (vErr == 0)
                    {
                        ViewState["AppId"] = vAppId;
                        txtAppNo.Text = vAppNo;
                        ViewState["LoanAppId"] = vAppNo;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
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
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
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
        private Boolean SaveCBRecords(string Mode)
        {
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();

            dtXml.Columns.Add("SLNo", typeof(int));
            dtXml.Columns.Add("ReportId", typeof(string));
            dtXml.Columns.Add("ReportDate", typeof(string));
            dtXml.Columns.Add("CBNameId", typeof(int));
            dtXml.Columns.Add("OrgName", typeof(string));
            dtXml.Columns.Add("LoanType", typeof(string));
            dtXml.Columns.Add("Active", typeof(string));
            dtXml.Columns.Add("BankValName", typeof(string));
            dtXml.Columns.Add("LoanAmt", typeof(float));
            dtXml.Columns.Add("POSAmt", typeof(float));
            dtXml.Columns.Add("LoanDate", typeof(string));
            dtXml.Columns.Add("EMI", typeof(float));
            dtXml.Columns.Add("Tenure", typeof(int));
            dtXml.Columns.Add("EMIPaid", typeof(float));
            dtXml.Columns.Add("PMn1", typeof(string));
            dtXml.Columns.Add("PMn2", typeof(string));
            dtXml.Columns.Add("PMn3", typeof(string));
            dtXml.Columns.Add("PMn4", typeof(string));
            dtXml.Columns.Add("PMn5", typeof(string));
            dtXml.Columns.Add("PMn6", typeof(string));
            dtXml.Columns.Add("PMn7", typeof(string));
            dtXml.Columns.Add("Remarks", typeof(string));

            foreach (GridViewRow gr in gvCBDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtReportId")).Text != "")
                {
                    dr = dtXml.NewRow();

                    dr["SLNo"] = ((Label)gr.FindControl("lblSLNoCB")).Text;
                    dr["ReportId"] = ((TextBox)gr.FindControl("txtReportId")).Text;

                    if (((TextBox)gr.FindControl("txtReportDate")).Text != "")
                        dr["ReportDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtReportDate")).Text);
                    else
                        dr["ReportDate"] = null;
                    if (((DropDownList)gr.FindControl("ddlCBname")).SelectedIndex == -1)
                    {
                        ((Label)gr.FindControl("lblCBNameId")).Text = "-1";
                    }
                    else
                    {
                        ((Label)gr.FindControl("lblCBNameId")).Text = ((DropDownList)gr.FindControl("ddlCBname")).SelectedValue.ToString();
                    }
                    dr["CBNameId"] = ((Label)gr.FindControl("lblCBNameId")).Text;
                    dr["OrgName"] = ((TextBox)gr.FindControl("txtOrgNameCB")).Text;
                    dr["LoanType"] = ((TextBox)gr.FindControl("txtLoanTypeCB")).Text;


                    if (((CheckBox)gr.FindControl("chkCBIsActive")).Checked == true)
                    {
                        ((Label)gr.FindControl("lblIsActive")).Text = "Y";
                    }
                    else
                    {
                        ((Label)gr.FindControl("lblIsActive")).Text = "N";
                    }
                    dr["Active"] = ((Label)gr.FindControl("lblIsActive")).Text;
                    dr["BankValName"] = ((TextBox)gr.FindControl("txtBankValName")).Text;
                    dr["LoanAmt"] = ((TextBox)gr.FindControl("txtLoanAmtCB")).Text;
                    dr["POSAmt"] = ((TextBox)gr.FindControl("txtPOSAmt")).Text;
                    if (((TextBox)gr.FindControl("txtLoanDateCB")).Text != "")
                        dr["LoanDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtLoanDateCB")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Loan Date to Save record");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtEMICB")).Text != "")
                        dr["EMI"] = ((TextBox)gr.FindControl("txtEMICB")).Text;
                    else
                        dr["EMI"] = 0;
                    if (((TextBox)gr.FindControl("txtTenureCB")).Text != "")
                        dr["Tenure"] = ((TextBox)gr.FindControl("txtTenureCB")).Text;
                    else
                        dr["Tenure"] = 0;
                    if (((TextBox)gr.FindControl("txtEMIPaidCB")).Text != "")
                        dr["EMIPaid"] = ((TextBox)gr.FindControl("txtEMIPaidCB")).Text;
                    else
                        dr["EMIPaid"] = 0;
                    if (((TextBox)gr.FindControl("txtPMn1")).Text.ToString() != "")
                        dr["PMn1"] = ((TextBox)gr.FindControl("txtPMn1")).Text.ToString();
                    else
                        dr["PMn1"] = "";

                    if (((TextBox)gr.FindControl("txtPMn2")).Text.ToString() != "")
                        dr["PMn2"] = ((TextBox)gr.FindControl("txtPMn2")).Text.ToString();
                    else
                        dr["PMn2"] = "";

                    if (((TextBox)gr.FindControl("txtPMn3")).Text.ToString() != "")
                        dr["PMn3"] = ((TextBox)gr.FindControl("txtPMn3")).Text.ToString();
                    else
                        dr["PMn3"] = "";
                    if (((TextBox)gr.FindControl("txtPMn4")).Text.ToString() != "")
                        dr["PMn4"] = ((TextBox)gr.FindControl("txtPMn4")).Text.ToString();
                    else
                        dr["PMn4"] = "";
                    if (((TextBox)gr.FindControl("txtPMn5")).Text.ToString() != "")
                        dr["PMn5"] = ((TextBox)gr.FindControl("txtPMn5")).Text.ToString();
                    else
                        dr["PMn5"] = "";
                    if (((TextBox)gr.FindControl("txtPMn6")).Text.ToString() != "")
                        dr["PMn6"] = ((TextBox)gr.FindControl("txtPMn6")).Text.ToString();
                    else
                        dr["PMn6"] = "";
                    if (((TextBox)gr.FindControl("txtPMn7")).Text.ToString() != "")
                        dr["PMn7"] = ((TextBox)gr.FindControl("txtPMn7")).Text.ToString();
                    else
                        dr["PMn7"] = "";
                    if (((TextBox)gr.FindControl("txtRemarks")).Text.ToString() != "")
                        dr["Remarks"] = ((TextBox)gr.FindControl("txtRemarks")).Text.ToString();
                    else
                        dr["Remarks"] = "";
                    dtXml.Rows.Add(dr);
                }


                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            string vXml = "";
            string vCBLnAppId = "";
            CMember oMem = new CMember();
            CGblIdGenerator oGbl = null;
            try
            {
                vCBLnAppId = (Request[txtLnAppCB.UniqueID] as string == null) ? txtLnAppCB.Text : Request[txtLnAppCB.UniqueID] as string;
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00 AM", "").Trim();
                }
                Int32 vErr = 0, vCBId = 0;
                string vMsg = "";


                if (hdfCBId.Value.ToString() != "")
                {
                    vCBId = Convert.ToInt32(hdfCBId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        //tdMsg.Visible = true;
                        lblValMsgCB.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        lblValMsgCB.Text = vMsg;
                        txtCustNoApp.Text = "";
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
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
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vCBLnAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report already made for this Loan Application No.. You can not delete CB Record..");
                        return false;
                    }
                    vErr = oMem.SaveCBData(vCBId, vCBLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
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
                oGbl = null;
            }
        }
        private Boolean SaveCBRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vCBRemarksId = 0;
            string vLnAppId = "", vCBPositive = "", vCBNegative = "", vCBMitigation = "", vRemarksDesc = "", vCBPass = "", vCBRejReason = "";
            Decimal vCIBILScore = 0;
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            //  DateTime vCBPassRejDate = null;
            CGblIdGenerator oGbl = null;
            try
            {
                dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Save CB Remarks as This Loan Application remains in Rejected State...");
                        return false;
                    }
                }
                if (hfCBRemarkID.Value != "")
                {
                    vCBRemarksId = Convert.ToInt32(hfCBRemarkID.Value);
                }
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCBPositive = Convert.ToString((Request[txtCBPOsRemark.UniqueID] as string == null) ? txtCBPOsRemark.Text : Request[txtCBPOsRemark.UniqueID] as string);
                vCBNegative = Convert.ToString((Request[txtCBNegRemark.UniqueID] as string == null) ? txtCBNegRemark.Text : Request[txtCBNegRemark.UniqueID] as string);
                vCBMitigation = Convert.ToString((Request[txtCBMiti.UniqueID] as string == null) ? txtCBMiti.Text : Request[txtCBMiti.UniqueID] as string);
                if (txtCIBILScore.Text != "")
                {
                    vCIBILScore = Convert.ToDecimal((Request[txtCIBILScore.UniqueID] as string == null) ? txtCIBILScore.Text : Request[txtCIBILScore.UniqueID] as string);
                    if (vCIBILScore > 0 && txtCIBILScoreDate.Text == "")
                    {
                        gblFuction.AjxMsgPopup("At the time of input CIBIL Score it is mandatory to give CIBIL Score Date...");
                        txtCIBILScoreDate.Focus();
                        return false;
                    }
                }

                if (chkCBPass.Checked == true)
                    vCBPass = "Y";
                else
                    vCBPass = "N";
                if (txtCBPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("CIBIL Pass/Rejection Date Can Not Be Empty...");
                    txtCBPassRejDate.Focus();
                    return false;
                }
                DateTime vCIBILScoreDate = gblFuction.setDate(txtCIBILScoreDate.Text);
                DateTime vCBPassRejDate = gblFuction.setDate(txtCBPassRejDate.Text);
                vCBRejReason = Convert.ToString((Request[txtCBRejReason.UniqueID] as string == null) ? txtCBRejReason.Text : Request[txtCBRejReason.UniqueID] as string);
                vRemarksDesc = "CB";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least on field to Save CB Remarks Record...");
                    //    return false;
                    //}
                    vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "I", "Save", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {
                    //if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least on field to Update CB Remarks Record...");
                    //    return false;
                    //}
                    vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveCBRemarks(ref vCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "D", "Delete", vCIBILScore, vCBPass, vCBPassRejDate, vCBRejReason, vCIBILScoreDate);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
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
                dt = null;
            }
        }
        private Boolean SaveCAMCBRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vCAMCBRemarksId = 0;
            string vLnAppId = "", vCBPositive = "", vCBNegative = "", vCBMitigation = "", vRemarksDesc = "";
            decimal vCIBILScore = 0;
            CMember oMem = new CMember();
            try
            {
                if (hfCAMCBRemarkID.Value != "")
                {
                    vCAMCBRemarksId = Convert.ToInt32(hfCAMCBRemarkID.Value);
                }
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCBPositive = Convert.ToString((Request[txtCBPosPoint.UniqueID] as string == null) ? txtCBPosPoint.Text : Request[txtCBPosPoint.UniqueID] as string);
                vCBNegative = Convert.ToString((Request[txtCBNegPoint.UniqueID] as string == null) ? txtCBNegPoint.Text : Request[txtCBNegPoint.UniqueID] as string);
                vCBMitigation = Convert.ToString((Request[txtCBMitiPoint.UniqueID] as string == null) ? txtCBMitiPoint.Text : Request[txtCBMitiPoint.UniqueID] as string);
                if (txtCAMCIBILScore.Text != "")
                {
                    vCIBILScore = Convert.ToDecimal((Request[txtCAMCIBILScore.UniqueID] as string == null) ? txtCAMCIBILScore.Text : Request[txtCAMCIBILScore.UniqueID] as string);
                    if (vCIBILScore > 0 && txtCAMCIBILScoreDate.Text == "")
                    {
                        gblFuction.AjxMsgPopup("At the time of updating CIBIL Score it is mandatory to give CIBIL Score Date");
                        txtCAMCIBILScoreDate.Focus();
                        return false;
                    }

                }
                DateTime vCIBILScoreDate = gblFuction.setDate(txtCAMCIBILScoreDate.Text);

                vRemarksDesc = "CB";

                if (Mode == "Edit")
                {
                    if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least on field to Update Credit Bureau Remarks Record...");
                        return false;
                    }
                    vErr = oMem.SaveCAMCBRemarks(ref vCAMCBRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit", vCIBILScore, vCIBILScoreDate);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
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
                oMem = null;
            }
        }
        private Boolean SaveBSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vBSRemarksId = 0;
            string vLnAppId = "", vBSPositive = "", vBSNegative = "", vBSMitigation = "", vRemarksDesc = "", vBSPass = "", vBSRejReason = "";
            CMember oMem = new CMember();
            DataTable dtCBPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                txtAppIdBankAc.Text = ViewState["LoanAppId"].ToString();
                dtCBPass = oMem.GetCBPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtCBPass.Rows.Count > 0)
                {
                    if (dtCBPass.Rows[0]["CBPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Add Bank Statement Remarks as Credit Bureau remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick CB Pass/Reject Checkbox to add Bank Statement Remarks... ");
                    return false;
                }
                if (hfBSRemarkID.Value != "")
                {
                    vBSRemarksId = Convert.ToInt32(hfBSRemarkID.Value);
                }
                if (chkBSPass.Checked == true)
                    vBSPass = "Y";
                else
                    vBSPass = "N";
                if (txtBSPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Bank Statement Pass/Rejection Date Can Not Be Empty...");
                    txtBSPassRejDate.Focus();
                    return false;
                }
                DateTime vCBPassRejDate = gblFuction.setDate(txtBSPassRejDate.Text);
                vBSRejReason = Convert.ToString((Request[txtBSRejReason.UniqueID] as string == null) ? txtBSRejReason.Text : Request[txtBSRejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vBSPositive = Convert.ToString((Request[txtBSPosRemark.UniqueID] as string == null) ? txtBSPosRemark.Text : Request[txtBSPosRemark.UniqueID] as string);
                vBSNegative = Convert.ToString((Request[txtBSNegRemark.UniqueID] as string == null) ? txtBSNegRemark.Text : Request[txtBSNegRemark.UniqueID] as string);
                vBSMitigation = Convert.ToString((Request[txtBSMiti.UniqueID] as string == null) ? txtBSMiti.Text : Request[txtBSMiti.UniqueID] as string);
                vRemarksDesc = "BS";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vBSPositive == "" && vBSNegative == "" && vBSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Save Bank Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                        Convert.ToInt32(hdUserID.Value), "I", "Save", vBSPass, vCBPassRejDate, vBSRejReason);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {
                    //if (vBSPositive == "" && vBSNegative == "" && vBSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Update Bank Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                        Convert.ToInt32(hdUserID.Value), "E", "Edit", vBSPass, vCBPassRejDate, vBSRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveBSRemarks(ref vBSRemarksId, vRemarksDesc, vLnAppId, vBSPositive, vBSNegative, vBSMitigation,
                            Convert.ToInt32(hdUserID.Value), "D", "Delete", vBSPass, vCBPassRejDate, vBSRejReason);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
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
        private Boolean SaveCAMBSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vCAMBSRemarksId = 0;
            string vLnAppId = "", vCBPositive = "", vCBNegative = "", vCBMitigation = "", vRemarksDesc = "";
            CMember oMem = new CMember();
            try
            {
                if (hfCAMBSRemarkID.Value != "")
                {
                    vCAMBSRemarksId = Convert.ToInt32(hfCAMBSRemarkID.Value);
                }
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCBPositive = Convert.ToString((Request[txtBSPosPoint.UniqueID] as string == null) ? txtBSPosPoint.Text : Request[txtBSPosPoint.UniqueID] as string);
                vCBNegative = Convert.ToString((Request[txtBSNegPoint.UniqueID] as string == null) ? txtBSNegPoint.Text : Request[txtBSNegPoint.UniqueID] as string);
                vCBMitigation = Convert.ToString((Request[txtBSMitiPoint.UniqueID] as string == null) ? txtBSMitiPoint.Text : Request[txtBSMitiPoint.UniqueID] as string);
                vRemarksDesc = "BS";

                if (Mode == "Edit")
                {
                    if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least one field to Update Bank Statement Remarks Record...");
                        return false;
                    }
                    vErr = oMem.SaveCAMBSRemarks(ref vCAMBSRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
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
                oMem = null;
            }
        }
        private Boolean SaveFSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vFSRemarksId = 0;
            string vLnAppId = "", vFSPositive = "", vFSNegative = "", vFSMitigation = "", vRemarksDesc = "", vFinStatPass = "", vFinStatRejReason = "";
            CMember oMem = new CMember();
            DataTable dtBSPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                dtBSPass = oMem.GetBSPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtBSPass.Rows.Count > 0)
                {
                    if (dtBSPass.Rows[0]["BSPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Add Financial Statement Remarks as Bank Statement remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick Bank Statement Pass/Reject Checkbox to add Financial Statement Remarks... ");
                    return false;
                }
                if (hfFSRemarkID.Value != "")
                {
                    vFSRemarksId = Convert.ToInt32(hfFSRemarkID.Value);
                }
                if (txtFSPassRejDate.Text == "")
                {
                    gblFuction.AjxMsgPopup("Financial Statement Pass/Rejection Date Can Not Be Blank...");
                    txtFSPassRejDate.Focus();
                    return false;
                }
                if (chkFSPass.Checked == true)
                    vFinStatPass = "Y";
                else
                    vFinStatPass = "N";
                DateTime vFinStatPassDate = gblFuction.setDate(txtFSPassRejDate.Text);
                vFinStatRejReason = Convert.ToString((Request[txtFSRejReason.UniqueID] as string == null) ? txtFSPosRemark.Text : Request[txtFSRejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vFSPositive = Convert.ToString((Request[txtFSPosRemark.UniqueID] as string == null) ? txtFSPosRemark.Text : Request[txtFSPosRemark.UniqueID] as string);
                vFSNegative = Convert.ToString((Request[txtFSNegRemark.UniqueID] as string == null) ? txtFSNegRemark.Text : Request[txtFSNegRemark.UniqueID] as string);
                vFSMitigation = Convert.ToString((Request[txtFSMiti.UniqueID] as string == null) ? txtFSMiti.Text : Request[txtFSMiti.UniqueID] as string);
                vRemarksDesc = "FS";
                // Remarks Description...
                //CB----Credit Bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vFSPositive == "" && vFSNegative == "" && vFSMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Save Financial Statement Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "I", "Save", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {
                    if (vFSPositive == "" && vFSNegative == "" && vFSMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least on field to Update Financial Statement Remarks...");
                        return false;
                    }
                    vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    if (vRec <= 0)
                    {
                        vErr = oMem.SaveFSRemarks(ref vFSRemarksId, vRemarksDesc, vLnAppId, vFSPositive, vFSNegative, vFSMitigation, Convert.ToInt32(hdUserID.Value), "D", "Delete", vFinStatPass, vFinStatPassDate, vFinStatRejReason);
                        if (vErr > 0)
                        {
                            gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                            vResult = true;
                        }
                        else
                        {
                            gblFuction.MsgPopup(gblPRATAM.DBError);
                            vResult = false;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.RecordUseMsg);
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
        private Boolean SaveCAMFSRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vCAMFSRemarksId = 0;
            string vLnAppId = "", vCBPositive = "", vCBNegative = "", vCBMitigation = "", vRemarksDesc = "";
            CMember oMem = new CMember();
            try
            {
                if (hfCAMFSRemarkID.Value != "")
                {
                    vCAMFSRemarksId = Convert.ToInt32(hfCAMFSRemarkID.Value);
                }
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCBPositive = Convert.ToString((Request[txtFSPosPoint.UniqueID] as string == null) ? txtFSPosPoint.Text : Request[txtFSPosPoint.UniqueID] as string);
                vCBNegative = Convert.ToString((Request[txtFSNegPoint.UniqueID] as string == null) ? txtFSNegPoint.Text : Request[txtFSNegPoint.UniqueID] as string);
                vCBMitigation = Convert.ToString((Request[txtFSMitiPoint.UniqueID] as string == null) ? txtFSMitiPoint.Text : Request[txtFSMitiPoint.UniqueID] as string);
                vRemarksDesc = "FS";

                if (Mode == "Edit")
                {
                    if (vCBPositive == "" && vCBNegative == "" && vCBMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least one field to Update Financial Statement Remarks Record...");
                        return false;
                    }
                    vErr = oMem.SaveCAMFSRemarks(ref vCAMFSRemarksId, vRemarksDesc, vLnAppId, vCBPositive, vCBNegative, vCBMitigation, Convert.ToInt32(hdUserID.Value), "E", "Edit");
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
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
                oMem = null;
            }
        }
        private Boolean SaveCAMRemarks(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vRec = 0, vCAMRemarksId = 0;
            string vLnAppId = "", vCAMPositive = "", vCAMNegative = "", vCAMMitigation = "", vRemarksDesc = "", vCAMPass = "", vCAMRejReason = ""; ;
            CMember oMem = new CMember();
            DataTable dtFSPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                dtFSPass = oMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtFSPass.Rows.Count > 0)
                {
                    if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Add CAM Remark as Financial Statement remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to add CAM Remark... ");
                    return false;
                }
                if (hfCAMRemarkID.Value != "")
                {
                    vCAMRemarksId = Convert.ToInt32(hfCAMRemarkID.Value);
                }
                if (txtCAMPassRejDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("CAM Pass/Rejection Date Can Not Be Blank...");
                    txtCAMPassRejDt.Focus();
                    return false;
                }
                if (chkCAMPass.Checked == true)
                    vCAMPass = "Y";
                else
                    vCAMPass = "N";
                DateTime vCAMPassRejDate = gblFuction.setDate(txtCAMPassRejDt.Text);
                vCAMRejReason = Convert.ToString((Request[txtCAMRejRemark.UniqueID] as string == null) ? txtCAMRejRemark.Text : Request[txtCAMRejRemark.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vCAMPositive = Convert.ToString((Request[txtCAMPosRemark.UniqueID] as string == null) ? txtCAMPosRemark.Text : Request[txtCAMPosRemark.UniqueID] as string);
                vCAMNegative = Convert.ToString((Request[txtCAMNegRemark.UniqueID] as string == null) ? txtCAMNegRemark.Text : Request[txtCAMNegRemark.UniqueID] as string);
                vCAMMitigation = Convert.ToString((Request[txtCAMMiti.UniqueID] as string == null) ? txtCAMMiti.Text : Request[txtCAMMiti.UniqueID] as string);
                vRemarksDesc = "CM";
                // Remarks Description...
                //CB----Credit bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    //if (vCAMPositive == "" && vCAMNegative == "" && vCAMMitigation == "")
                    //{
                    //    gblFuction.MsgPopup("Please Give Input at least one field to Save CAM  Remarks...");
                    //    return false;
                    //}
                    vErr = oMem.SaveCAMRemarks(ref vCAMRemarksId, vRemarksDesc, vLnAppId, vCAMPositive, vCAMNegative, vCAMMitigation,
                        Convert.ToInt32(hdUserID.Value), "I", "Save", vCAMPass, vCAMPassRejDate, vCAMRejReason);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {
                    if (vCAMPositive == "" && vCAMNegative == "" && vCAMMitigation == "")
                    {
                        gblFuction.MsgPopup("Please Give Input at least one field to Update CAM Remarks...");
                        return false;
                    }
                    vErr = oMem.SaveCAMRemarks(ref vCAMRemarksId, vRemarksDesc, vLnAppId, vCAMPositive, vCAMNegative, vCAMMitigation,
                        Convert.ToInt32(hdUserID.Value), "E", "Edit", vCAMPass, vCAMPassRejDate, vCAMRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {

                    vErr = oMem.SaveCAMRemarks(ref vCAMRemarksId, vRemarksDesc, vLnAppId, vCAMPositive, vCAMNegative, vCAMMitigation,
                        Convert.ToInt32(hdUserID.Value), "D", "Delete", vCAMPass, vCAMPassRejDate, vCAMRejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                oMem = null;
            }
        }
        private Boolean SavePD1Approval(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vPD1ApprovalId = 0;
            string vLnAppId = "", vRemarksDesc = "", vPD1Pass = "", vPD1RejReason = ""; ;
            CMember oMem = new CMember();
            DataTable dtFSPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                dtFSPass = oMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtFSPass.Rows.Count > 0)
                {
                    if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Update PD1 Approval as Financial Statement remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to Update PD1 Approval... ");
                    return false;
                }
                if (hfPD1ApprovalId.Value != "")
                {
                    vPD1ApprovalId = Convert.ToInt32(hfPD1ApprovalId.Value);
                }
                if (txtPD1PassRejDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("PD1 Pass/Rejection Date Can Not Be Blank...");
                    txtPD1PassRejDt.Focus();
                    return false;
                }
                if (chkPD1Pass.Checked == true)
                    vPD1Pass = "Y";
                else
                    vPD1Pass = "N";
                DateTime vPD1assRejDate = gblFuction.setDate(txtPD1PassRejDt.Text);
                vPD1RejReason = Convert.ToString((Request[txtPD1RejReason.UniqueID] as string == null) ? txtPD1RejReason.Text : Request[txtPD1RejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vRemarksDesc = "PD1";
                // Remarks Description...
                //CB----Credit bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    vErr = oMem.SavePD1Approval(ref vPD1ApprovalId, vRemarksDesc, vLnAppId,
                        Convert.ToInt32(hdUserID.Value), "I", "Save", vPD1Pass, vPD1assRejDate, vPD1RejReason);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {

                    vErr = oMem.SavePD1Approval(ref vPD1ApprovalId, vRemarksDesc, vLnAppId, Convert.ToInt32(hdUserID.Value), "E", "Edit",
                        vPD1Pass, vPD1assRejDate, vPD1RejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {

                    vErr = oMem.SavePD1Approval(ref vPD1ApprovalId, vRemarksDesc, vLnAppId,
                        Convert.ToInt32(hdUserID.Value), "D", "Delete", vPD1Pass, vPD1assRejDate, vPD1RejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                oMem = null;
            }
        }
        private Boolean SavePD2Approval(string Mode)
        {
            Boolean vResult = false;
            string vStateEdit = Convert.ToString(ViewState["StateEdit"]);
            Int32 vErr = 0, vPD2ApprovalId = 0;
            string vLnAppId = "", vRemarksDesc = "", vPD2Pass = "", vPD2RejReason = ""; ;
            CMember oMem = new CMember();
            DataTable dtFSPass = new DataTable();
            CGblIdGenerator oGbl = null;
            try
            {
                dtFSPass = oMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                if (dtFSPass.Rows.Count > 0)
                {
                    if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                    {
                        gblFuction.MsgPopup("You Can Not Update PD2 Approval as Financial Statement remains in Rejected State ...");
                        return false;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to Save PD2 Approval... ");
                    return false;
                }
                if (hfPD2ApprovalId.Value != "")
                {
                    vPD2ApprovalId = Convert.ToInt32(hfPD2ApprovalId.Value);
                }
                if (txtPD2PassRejDt.Text == "")
                {
                    gblFuction.AjxMsgPopup("PD2 Pass/Rejection Date Can Not Be Blank...");
                    txtPD2PassRejDt.Focus();
                    return false;
                }
                if (chkPD2Pass.Checked == true)
                    vPD2Pass = "Y";
                else
                    vPD2Pass = "N";
                DateTime vPD2assRejDate = gblFuction.setDate(txtPD2PassRejDt.Text);
                vPD2RejReason = Convert.ToString((Request[txtPD2RejReason.UniqueID] as string == null) ? txtPD2RejReason.Text : Request[txtPD2RejReason.UniqueID] as string);
                vLnAppId = ViewState["LoanAppId"].ToString();
                vRemarksDesc = "PD2";
                // Remarks Description...
                //CB----Credit bureau Remarks
                //BS----Bank Statement Remarks
                //FS----Financial Statement Remarks
                //CM----CAM Remarks
                //PD1--Personal Discussion 1
                //PD2-- Personal Discussion 2
                if (Mode == "Save")
                {
                    oGbl = new CGblIdGenerator();
                    vErr = oMem.SavePD2Approval(ref vPD2ApprovalId, vRemarksDesc, vLnAppId,
                        Convert.ToInt32(hdUserID.Value), "I", "Save", vPD2Pass, vPD2assRejDate, vPD2RejReason);
                    if (vErr > 0)
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
                else if (Mode == "Edit")
                {

                    vErr = oMem.SavePD2Approval(ref vPD2ApprovalId, vRemarksDesc, vLnAppId, Convert.ToInt32(hdUserID.Value), "E", "Edit",
                        vPD2Pass, vPD2assRejDate, vPD2RejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {

                    vErr = oMem.SavePD2Approval(ref vPD2ApprovalId, vRemarksDesc, vLnAppId,
                        Convert.ToInt32(hdUserID.Value), "D", "Delete", vPD2Pass, vPD2assRejDate, vPD2RejReason);
                    if (vErr > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
                oMem = null;
            }
        }
        private Boolean SaveBankRecord(string Mode)
        {
            // Generate DataTable For Applicant bank Account Details Section
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add(new DataColumn("SlNo"));
            dtXml.Columns.Add(new DataColumn("AccDate"));
            dtXml.Columns.Add(new DataColumn("Debit"));
            dtXml.Columns.Add(new DataColumn("Transfer"));
            dtXml.Columns.Add(new DataColumn("LoanAvailed"));
            dtXml.Columns.Add(new DataColumn("Credit"));
            dtXml.Columns.Add(new DataColumn("SalesFig"));
            dtXml.Columns.Add(new DataColumn("CCLimit"));
            dtXml.Columns.Add(new DataColumn("Day5"));
            dtXml.Columns.Add(new DataColumn("Day10"));
            dtXml.Columns.Add(new DataColumn("Day15"));
            dtXml.Columns.Add(new DataColumn("Day20"));
            dtXml.Columns.Add(new DataColumn("Day25"));
            foreach (GridViewRow gr in gvBankDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtDate")).Text != "")
                {
                    dr = dtXml.NewRow();
                    dr["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;

                    if (((TextBox)gr.FindControl("txtDate")).Text != "")
                        dr["AccDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Date to add record");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtDebit")).Text != "")
                        dr["Debit"] = ((TextBox)gr.FindControl("txtDebit")).Text;
                    else
                        dr["Debit"] = 0;
                    if (((TextBox)gr.FindControl("txtTransfer")).Text != "")
                        dr["Transfer"] = ((TextBox)gr.FindControl("txtTransfer")).Text;
                    else
                        dr["Transfer"] = 0;
                    if (((TextBox)gr.FindControl("txtLoanAvailed")).Text != "")
                        dr["LoanAvailed"] = ((TextBox)gr.FindControl("txtLoanAvailed")).Text;
                    else
                        dr["LoanAvailed"] = 0;
                    if (((TextBox)gr.FindControl("txtCredit")).Text != "")
                        dr["Credit"] = ((TextBox)gr.FindControl("txtCredit")).Text;
                    else
                        dr["Credit"] = 0;
                    if (((TextBox)gr.FindControl("txtSalesFig")).Text != "")
                        dr["SalesFig"] = ((TextBox)gr.FindControl("txtSalesFig")).Text;
                    else
                        dr["SalesFig"] = 0;
                    if (((TextBox)gr.FindControl("txtCCLimit")).Text != "")
                        dr["CCLimit"] = ((TextBox)gr.FindControl("txtCCLimit")).Text;
                    else
                        dr["CCLimit"] = 0;
                    if (((TextBox)gr.FindControl("txtDay5")).Text != "")
                        dr["Day5"] = ((TextBox)gr.FindControl("txtDay5")).Text;
                    else
                        dr["Day5"] = 0;
                    if (((TextBox)gr.FindControl("txtDay10")).Text != "")
                        dr["Day10"] = ((TextBox)gr.FindControl("txtDay10")).Text;
                    else
                        dr["Day10"] = 0;
                    if (((TextBox)gr.FindControl("txtDay15")).Text != "")
                        dr["Day15"] = ((TextBox)gr.FindControl("txtDay15")).Text;
                    else
                        dr["Day15"] = 0;
                    if (((TextBox)gr.FindControl("txtDay20")).Text != "")
                        dr["Day20"] = ((TextBox)gr.FindControl("txtDay20")).Text;
                    else
                        dr["Day20"] = 0;
                    if (((TextBox)gr.FindControl("txtDay25")).Text != "")
                        dr["Day25"] = ((TextBox)gr.FindControl("txtDay25")).Text;
                    else
                        dr["Day25"] = 0;

                    dtXml.Rows.Add(dr);
                    dtXml.AcceptChanges();
                }
            }
            dtXml.TableName = "Table1";

            // Generate DataTable For Applicant bank Account IWOW Details Section
            DataRow dr1 = null;
            DataTable dtXml1 = new DataTable();
            dtXml1.Columns.Add(new DataColumn("SlNo"));
            dtXml1.Columns.Add(new DataColumn("ChqDate"));
            dtXml1.Columns.Add(new DataColumn("IWReturn"));
            dtXml1.Columns.Add(new DataColumn("OWReturn"));
            dtXml1.Columns.Add(new DataColumn("EMIReturn"));
            dtXml1.Columns.Add(new DataColumn("ChqClearDate"));
            dtXml1.Columns.Add(new DataColumn("ChqNo"));
            dtXml1.Columns.Add(new DataColumn("Reason"));

            foreach (GridViewRow gr in gvIWOWDtl.Rows)
            {
                if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                {
                    dr1 = dtXml1.NewRow();
                    dr1["SlNo"] = ((Label)gr.FindControl("lblSlNo")).Text;
                    if (((TextBox)gr.FindControl("txtChqDate")).Text != "")
                        dr1["ChqDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtChqDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please Select Date to add record in Cheque Return Details Section");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtIWReturn")).Text != "")
                        dr1["IWReturn"] = ((TextBox)gr.FindControl("txtIWReturn")).Text;
                    else
                        dr1["IWReturn"] = 0;
                    if (((TextBox)gr.FindControl("txtOWReturn")).Text != "")
                        dr1["OWReturn"] = ((TextBox)gr.FindControl("txtOWReturn")).Text;
                    else
                        dr1["OWReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtEMIReturn")).Text != "")
                        dr1["EMIReturn"] = ((TextBox)gr.FindControl("txtEMIReturn")).Text;
                    else
                        dr1["EMIReturn"] = 0;

                    if (((TextBox)gr.FindControl("txtClearedDt")).Text != "")
                        dr1["ChqClearDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtClearedDt")).Text);
                    else
                        dr1["ChqClearDate"] = null;
                    if (((TextBox)gr.FindControl("txtChequeNo")).Text != "")
                        dr1["ChqNo"] = ((TextBox)gr.FindControl("txtChequeNo")).Text;
                    else
                        dr1["ChqNo"] = "";
                    if (((TextBox)gr.FindControl("txtReason")).Text != "")
                        dr1["Reason"] = ((TextBox)gr.FindControl("txtReason")).Text;
                    else
                        dr1["Reason"] = "";
                    dtXml1.Rows.Add(dr1);
                    dtXml1.AcceptChanges();
                }
            }
            dtXml1.TableName = "Table2";

            // Declaring variable for uploading bank statement document section
            string vAttchBankDocType = "";
            byte[] vAttachBankDoc = null;
            Int32 vAttachBankDocUpID = 0;
            string vAttchBankDocName = "";


            Boolean vResult = false;
            Int32 vBankAccId = 0, vError = 0, vNewId = 0, vSrNo = 0;
            string vApplicationId = "", vAcHolName = "", vBankname = "", vAccNo = "", vCombMOS = "", vShowAgrmnt = "",
                vAccType = "", vRemarks = "", vXml = "", vXmlIWOW = "";
            Double vTransLimit = 0, vCurrbal = 0, vBankingYr = 0;

            if (hfBankAcId.Value != "")
            {
                vBankAccId = Convert.ToInt32(hfBankAcId.Value);
            }
            if (txtBankAcSrNo.Text != "")
                vSrNo = Convert.ToInt32((Request[txtBankAcSrNo.UniqueID] as string == null) ? txtBankAcSrNo.Text : Request[txtBankAcSrNo.UniqueID] as string);
            vApplicationId = Convert.ToString((Request[txtAppIdBankAc.UniqueID] as string == null) ? txtAppIdBankAc.Text : Request[txtAppIdBankAc.UniqueID] as string);
            vAcHolName = Convert.ToString((Request[txtAcHolName.UniqueID] as string == null) ? txtAcHolName.Text : Request[txtAcHolName.UniqueID] as string);
            vBankname = Convert.ToString((Request[txtBankName.UniqueID] as string == null) ? txtBankName.Text : Request[txtBankName.UniqueID] as string);

            if (Convert.ToString((Request[txtAccNo.UniqueID] as string == null) ? txtAccNo.Text : Request[txtAccNo.UniqueID] as string) == "")
            {
                gblFuction.MsgPopup("Please Give Input Bank Account No...");
                return false;
            }
            if (Convert.ToString((Request[txtBankName.UniqueID] as string == null) ? txtBankName.Text : Request[txtBankName.UniqueID] as string) == "")
            {
                gblFuction.MsgPopup("Please Give Input Bank Name...");
                return false;
            }
            vAccNo = Convert.ToString((Request[txtAccNo.UniqueID] as string == null) ? txtAccNo.Text : Request[txtAccNo.UniqueID] as string);
            vAccType = Convert.ToString((Request[ddlACType.UniqueID] as string == null) ? ddlACType.SelectedValue : Request[ddlACType.UniqueID] as string);
            if (vAccType == "0")
            {
                gblFuction.MsgPopup("Please Select Bank Account Type..");
                return false;
            }
            vRemarks = Convert.ToString((Request[txtBankAcRemarks.UniqueID] as string == null) ? txtBankAcRemarks.Text : Request[txtBankAcRemarks.UniqueID] as string);
            if (txtBankSinYr.Text != "")
                vBankingYr = Convert.ToDouble((Request[txtBankSinYr.UniqueID] as string == null) ? txtBankSinYr.Text : Request[txtBankSinYr.UniqueID] as string);
            if (txtTransLimit.Text != "")
                vTransLimit = Convert.ToDouble((Request[txtTransLimit.UniqueID] as string == null) ? txtTransLimit.Text : Request[txtTransLimit.UniqueID] as string);
            if (txtCurrBalance.Text != "")
                vCurrbal = Convert.ToDouble((Request[txtCurrBalance.UniqueID] as string == null) ? txtCurrBalance.Text : Request[txtCurrBalance.UniqueID] as string);
            if (chkCombMOS.Checked == true)
                vCombMOS = "Y";
            else
                vCombMOS = "N";
            if (chkShowAgrmnt.Checked == true)
                vShowAgrmnt = "Y";
            else
                vShowAgrmnt = "N";
            CMember oMem = new CMember();
            try
            {

                if (hfBankStatDocId.Value != "")
                {
                    vAttachBankDocUpID = Convert.ToInt32(hfBankStatDocId.Value);
                }

                // Upload Bank Statement Documents Section
                if (fileuplodBankStat.PostedFile.InputStream.Length > 4194304)
                {
                    gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                    return false;
                }
                if (fileuplodBankStat.HasFile)
                {
                    vAttachBankDoc = new byte[fileuplodBankStat.PostedFile.InputStream.Length + 1];
                    fileuplodBankStat.PostedFile.InputStream.Read(vAttachBankDoc, 0, vAttachBankDoc.Length);
                    vAttchBankDocType = System.IO.Path.GetExtension(fileuplodBankStat.FileName).ToLower();
                    vAttchBankDocName = System.IO.Path.GetFileNameWithoutExtension(fileuplodBankStat.FileName);
                }

                //Generate XML For Applicant bank Account Details Section
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                //Generate XML For Applicant bank Account IWOW Details Section
                using (StringWriter oSW1 = new StringWriter())
                {
                    dtXml1.WriteXml(oSW1);
                    vXmlIWOW = oSW1.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                if (Mode == "Save")
                {
                    // Check For Duplicate Bank Acc No...
                    DataTable dtDupBankRec = new DataTable();
                    dtDupBankRec = oMem.ChkDupBankStatRec(vApplicationId, vAccNo);
                    if (Convert.ToInt32(dtDupBankRec.Rows[0]["BankAcRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Bank Account No already exist... Kindly Check...");
                        return false;
                    }
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks,
                        vTransLimit, vCurrbal, Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Save", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
                    {
                        hfBankAcId.Value = Convert.ToInt32(vNewId).ToString();
                        ViewState["BankAcId"] = vNewId;
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }

                }
                else if (Mode == "Edit")
                {
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks,
                        vTransLimit, vCurrbal, Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Edit", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
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
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    // Check For Deleting Record()
                    DataTable dtDelBankRec = new DataTable();
                    dtDelBankRec = oMem.ChkDelBankRecord(vApplicationId);
                    if (Convert.ToInt32(dtDelBankRec.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report alredy made.. Can Not Delete Record..");
                        return false;
                    }
                    vError = oMem.SaveBankStatement(ref vNewId, vBankAccId, vSrNo, vApplicationId, vAcHolName, vBankname, vAccType, vAccNo, vBankingYr, vRemarks, vTransLimit, vCurrbal,
                            Convert.ToInt32(hdUserID.Value), vXml, vXmlIWOW, "Delete", 0, vAttachBankDocUpID, vAttachBankDoc, vAttchBankDocType, vAttchBankDocName, vCombMOS, vShowAgrmnt);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
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
        private Boolean SaveBSRecords(string Mode)
        {

            Boolean vResult = false;
            DataRow dr = null;
            if (txtBSDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Balance Sheet Date can not be left blank...");
                return false;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtBSDate.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return false;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return false;
                }
            }
            Double TotLiability = 0;
            Double TotAsset = 0;
            if (txtTotLiability.Text != "")
                TotLiability = Convert.ToDouble(txtTotLiability.Text);
            if (txtTotAsset.Text != "")
                TotAsset = Convert.ToDouble(txtTotAsset.Text);
            //if (TotAsset - TotLiability < 0.03)
            //{
            //    gblFuction.AjxMsgPopup("Difference between Total Asset and Total Liability must be greater than .03");
            //    return false;
            //}
            DateTime vBSDate = gblFuction.setDate(txtBSDate.Text);
            string vAppId = txtAppNoBS.Text.ToString().Trim();


            DataTable dtXml = new DataTable();
            dtXml.Columns.Add("ParticularsId", typeof(int));
            dtXml.Columns.Add("Particulars", typeof(string));
            dtXml.Columns.Add("Amount", typeof(float));
            foreach (GridViewRow gr in gvLiability.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsId")).Text; ;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticulars")).Text;
                if (((TextBox)gr.FindControl("txtAmt")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtAmt")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            foreach (GridViewRow gr in gvAsset.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsId")).Text; ;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticulars")).Text;
                if (((TextBox)gr.FindControl("txtAssetAmt")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtAssetAmt")).Text;

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
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                Int32 vErr = 0, vBSId = 0;
                string vMsg = "";


                if (hfBSId.Value.ToString() != "")
                {
                    vBSId = Convert.ToInt32(hfBSId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    DataTable dtBS = new DataTable();
                    dtBS = oMem.DuplicateBSRecByLnNo(vAppId, vBSDate, Mode);
                    if (Convert.ToInt32(dtBS.Rows[0]["BSRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Data Already Exist in this BS Date and Loan Application No... Kindly Check Balance Sheet Summary...");
                        return false;
                    }
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
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
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vErr == 1)
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
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made for this Loan Application No.. You can not delete PL Record..");
                        return false;
                    }
                    vErr = oMem.SaveBSData(vBSId, vAppId, vBSDate, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
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
        private Boolean SavePLRecords(string Mode)
        {

            if (txtPLDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Profit/Loss Date can not be left blank...");
                return false;
            }
            else
            {
                string StrDD, StrMM, StrYYYY;
                string pDate = txtPLDate.Text.ToString();
                StrDD = pDate.Substring(0, 2);
                StrMM = pDate.Substring(3, 2);
                StrYYYY = pDate.Substring(6, 4);
                if (StrDD != "31")
                {
                    gblFuction.AjxMsgPopup("Day Part of selected date should be 31...");
                    return false;
                }
                else if (StrMM != "03")
                {
                    gblFuction.AjxMsgPopup("Month Part of selected date should be 03...");
                    return false;
                }
            }
            DateTime vPLDate = gblFuction.setDate(txtPLDate.Text);
            string vAppId = txtAppIdPL.Text.ToString().Trim();
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();

            dtXml.Columns.Add("ParticularsId", typeof(int));
            dtXml.Columns.Add("Particulars", typeof(string));
            dtXml.Columns.Add("Amount", typeof(float));

            foreach (GridViewRow gr in gvIncome.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsIdPL")).Text;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticularsPL")).Text;
                if (((TextBox)gr.FindControl("txtIncome")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtIncome")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            foreach (GridViewRow gr in gvExpense.Rows)
            {
                dr = dtXml.NewRow();

                dr["ParticularsId"] = ((Label)gr.FindControl("lblParticularsIdPL")).Text;
                dr["Particulars"] = ((Label)gr.FindControl("lblParticularsPL")).Text;
                if (((TextBox)gr.FindControl("txtExpense")).Text == "")
                    dr["Amount"] = 0;
                else
                    dr["Amount"] = ((TextBox)gr.FindControl("txtExpense")).Text;

                dtXml.Rows.Add(dr);
                dtXml.AcceptChanges();
            }
            dtXml.TableName = "Table1";

            DataTable dtXmlITR = new DataTable();
            dtXmlITR.Columns.Add("ITRFileDate", typeof(string));
            dtXmlITR.Columns.Add("FinYear", typeof(string));
            dtXmlITR.Columns.Add("ITRFiledYN", typeof(string));
            foreach (GridViewRow gr in gvITR.Rows)
            {
                if (((TextBox)gr.FindControl("txtITRFileDate")).Text != "")
                {
                    dr = dtXmlITR.NewRow();
                    if (((TextBox)gr.FindControl("txtITRFileDate")).Text != "")
                        dr["ITRFileDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtITRFileDate")).Text);
                    else
                    {
                        gblFuction.AjxMsgPopup("Please give input ITR Filing Date....");
                        return false;
                    }
                    if (((TextBox)gr.FindControl("txtFinYear")).Text != "")
                        dr["FinYear"] = ((TextBox)gr.FindControl("txtFinYear")).Text;
                    else
                    {
                        gblFuction.AjxMsgPopup("Please give input Financial Year....");
                        return false;
                    }
                    if (((CheckBox)gr.FindControl("chkITRFile")).Checked == true)
                        dr["ITRFiledYN"] = "Y";
                    else
                        dr["ITRFiledYN"] = "N";

                    dtXmlITR.Rows.Add(dr);
                    dtXmlITR.AcceptChanges();
                }
            }
            dtXmlITR.TableName = "Table2";



            string vXml = "";
            string vITRXml = "";
            CMember oMem = new CMember();

            string vBSPLAtachDocType = "";
            byte[] vBSPLAttachDoc = null;
            Int32 vBSPLDocUpID = 0;
            string vBSPLAttachDocName = "";

            try
            {
                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                using (StringWriter oSW = new StringWriter())
                {
                    dtXmlITR.WriteXml(oSW);
                    vITRXml = oSW.ToString().Replace(" ", "").Trim().Replace("12:00:00AM", "").Trim();
                }
                Int32 vErr = 0, vPLId = 0;
                string vMsg = "";


                if (hfPLId.Value.ToString() != "")
                {
                    vPLId = Convert.ToInt32(hfPLId.Value);
                }

                if (hfPLBSDocUpId.Value != "")
                {
                    vBSPLDocUpID = Convert.ToInt32(hfPLBSDocUpId.Value);
                }
                if (fileuplodPLBS.PostedFile.InputStream.Length > 4194304)
                {
                    gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                    return false;
                }
                if (fileuplodPLBS.HasFile)
                {
                    vBSPLAttachDoc = new byte[fileuplodPLBS.PostedFile.InputStream.Length + 1];
                    fileuplodPLBS.PostedFile.InputStream.Read(vBSPLAttachDoc, 0, vBSPLAttachDoc.Length);
                    vBSPLAtachDocType = System.IO.Path.GetExtension(fileuplodPLBS.FileName).ToLower();
                    vBSPLAttachDocName = System.IO.Path.GetFileNameWithoutExtension(fileuplodPLBS.FileName);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.DuplicatePLRecByLnNo(vAppId, vPLDate, Mode);
                    if (Convert.ToInt32(dtPL.Rows[0]["PLRec"]) > 0)
                    {
                        gblFuction.MsgPopup("Data Already Exist in this PL Date and Loan Application No... Kindly Check Profit and Loss Summary...");
                        return false;
                    }
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Save", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
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
                    //DataTable dtPL = new DataTable();
                    //dtPL = oMem.DuplicatePLRecByLnNo(vAppId, vPLDate, Mode);
                    //if (Convert.ToInt32(dtPL.Rows[0]["PLRec"]) > 0)
                    //{
                    //    gblFuction.MsgPopup("Data Already Exist in this PL Date and Loan Application No... Kindly Check Profit and Loss Summary...");
                    //    return false;
                    //}
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Edit", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
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
                    DataTable dtPL = new DataTable();
                    dtPL = oMem.ChkDelPLRecord(vAppId);
                    if (Convert.ToInt32(dtPL.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made for this Loan Application No.. You can not delete PL Record..");
                        return false;
                    }
                    vErr = oMem.SavePLData(vPLId, vAppId, vPLDate, vXml, vITRXml, Convert.ToInt32(hdUserID.Value), "Delete", 0, vBSPLDocUpID, vBSPLAttachDoc, vBSPLAtachDocType, vBSPLAttachDocName);
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
        private Boolean SaveRefRecords(string Mode)
        {
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
        private Boolean SaveCAMRecords(string Mode)
        {
            Label lblMsg = (Label)Master.FindControl("lblMsg");
            Boolean vResult = false;
            string pCustId = ViewState["CusTID"].ToString();
            string pBranchCode = Session[gblValue.BrnchCode].ToString();
            string vAppId = "", pRemarks = "", pNewSancId = "", pTotDebtComments = "", pTotEquityComments = "";
            Int32 vErr = 0, vPurpId = 0, vLnTypeId = 0, pCAMId = 0, pAPPTenure = 0, pProTenure = 0, pNoOfInstal = 0, pNewId = 0;
            decimal pAppLnAmt = 0, pProLnAmt = 0, pProIntRate = 0, pTermLn = 0, pCCOD = 0, pCIBILLoanOS = 0, pKFIExistExpos = 0, pProExpos = 0, pTotalDebt = 0,

                pExistEMI = 0, pProEMI = 0, pCCIntSave = 0, pTotMonLia = 0, pTotalAMScore = 0, pTotEquity = 0, pDERatio = 0, pDSCR = 0, pPBDIT = 0;
            Int32 pPartId1 = 0, pPartId2 = 0, pPartId3 = 0, pPartId4 = 0, pPartId5 = 0, pPartId6 = 0, pPartId7 = 0, pPartId8 = 0, pPartId9 = 0, pPartId10 = 0, pPartId11 = 0,
                pPartId12 = 0, pPartId13 = 0, pPartId14 = 0, pPartId15 = 0, pPartId16 = 0, pPartId17 = 0, pPartId18 = 0;

            Int32 pAMValueId1 = 0, pAMValueId2 = 0, pAMValueId3 = 0, pAMValueId4 = 0, pAMValueId5 = 0, pAMValueId6 = 0, pAMValueId7 = 0, pAMValueId8 = 0, pAMValueId9 = 0,
                pAMValueId10 = 0, pAMValueId11 = 0, pAMValueId12 = 0, pAMValueId13 = 0, pAMValueId14 = 0, pAMValueId15 = 0, pAMValueId16 = 0, pAMValueId17 = 0, pAMValueId18 = 0;
            decimal pStr1 = 0, pStr2 = 0, pStr3 = 0, pStr4 = 0, pStr5 = 0, pStr6 = 0, pStr7 = 0, pStr8 = 0, pStr9 = 0, pStr10 = 0, pStr11 = 0, pStr12 = 0, pStr13 = 0, pStr14 = 0,
                pStr15 = 0, pStr16 = 0, pStr17 = 0, pStr18 = 0, pTotalAMScorePercen = 0;
            DateTime vAppDt = gblFuction.setDate(txtAppDt.Text);
            CApplication oCA = new CApplication();
            try
            {
                if (hfCAMId.Value != "")
                    pCAMId = Convert.ToInt32(hfCAMId.Value);
                vAppId = txtAppNoAM.Text.ToString();
                if (vAppId == "")
                {
                    gblFuction.AjxMsgPopup("Loan Application No can Not be Blank,Click on Show Details on Loan Application....");
                    return false;

                }
                pRemarks = txtCAMRemarks.Text.ToString();
                pTotDebtComments = txtTotDebtComments.Text.ToString().Trim();
                pTotEquityComments = txtTotEquityComments.Text.ToString().Trim();

                if (Request[ddlLnPur.UniqueID] as string != "-1")
                    vPurpId = Convert.ToInt32(Request[ddlLnPur.UniqueID] as string);
                if (Request[ddlCAMLnScheme.UniqueID] as string != "-1")
                    vLnTypeId = Convert.ToInt32(Request[ddlCAMLnScheme.UniqueID] as string);

                pAppLnAmt = Convert.ToDecimal(txtLnAmt.Text);
                if (txtProLnAmt.Text != "")
                    pProLnAmt = Convert.ToDecimal(txtProLnAmt.Text);
                if (txtProInstRate.Text != "")
                    pProIntRate = Convert.ToDecimal(txtProInstRate.Text);
                if (txtAppTenure.Text != "")
                    pAPPTenure = Convert.ToInt32(Request[txtAppTenure.UniqueID] as string);
                if (txtProTenure.Text != "")
                    pProTenure = Convert.ToInt32(Request[txtProTenure.UniqueID] as string);
                if (txtProNoofInst.Text != "")
                    pNoOfInstal = Convert.ToInt32(Request[txtProNoofInst.UniqueID] as string);

                if (((Request[txtTermLn.UniqueID] as string == null) ? txtTermLn.Text : Request[txtTermLn.UniqueID] as string) != "")
                    pTermLn = Convert.ToDecimal((Request[txtTermLn.UniqueID] as string == null) ? txtTermLn.Text : Request[txtTermLn.UniqueID] as string);
                if (txtCCorOD.Text != "")
                    pCCOD = Convert.ToDecimal(txtCCorOD.Text);
                if (txtCIBILLnOutst.Text != "")
                    pCIBILLoanOS = Convert.ToDecimal(txtCIBILLnOutst.Text);
                if (txtKFIExpos.Text != "")
                    pKFIExistExpos = Convert.ToDecimal(txtKFIExpos.Text);
                if (txtProExpos.Text != "")
                    pProExpos = Convert.ToDecimal(txtProExpos.Text);
                if (txtTotalDebtManual.Text != "")
                    pTotalDebt = Convert.ToDecimal(txtTotalDebtManual.Text);



                if (txtExistingEMI.Text != "")
                    pExistEMI = Convert.ToDecimal(txtExistingEMI.Text);
                if (txtProEMI.Text != "")
                    pProEMI = Convert.ToDecimal(txtProEMI.Text);
                if (txtCCInstSav.Text != "")
                    pCCIntSave = Convert.ToDecimal(txtCCInstSav.Text);
                if (txtTotMonlylia.Text != "")
                    pTotMonLia = Convert.ToDecimal(txtTotMonlylia.Text);
                if (txttotScore.Text != "")
                    pTotalAMScore = Convert.ToDecimal(txttotScore.Text);

                if (txtTotEquityManual.Text != "")
                    pTotEquity = Convert.ToDecimal(txtTotEquityManual.Text);
                if (txtDEManual.Text != "")
                    pDERatio = Convert.ToDecimal(txtDEManual.Text);
                if (txtPBDIT.Text != "")
                    pPBDIT = Convert.ToDecimal(txtPBDIT.Text);
                if (txtCustDocNameCR.Text != "")
                    pDSCR = Convert.ToDecimal(txtCustDocNameCR.Text);



                if (hfPartId1.Value != "")
                    pPartId1 = Convert.ToInt32(hfPartId1.Value);
                if (hfPartId2.Value != "")
                    pPartId2 = Convert.ToInt32(hfPartId2.Value);
                if (hfPartId3.Value != "")
                    pPartId3 = Convert.ToInt32(hfPartId3.Value);
                if (hfPartId4.Value != "")
                    pPartId4 = Convert.ToInt32(hfPartId4.Value);
                if (hfPartId5.Value != "")
                    pPartId5 = Convert.ToInt32(hfPartId5.Value);
                if (hfPartId6.Value != "")
                    pPartId6 = Convert.ToInt32(hfPartId6.Value);
                if (hfPartId7.Value != "")
                    pPartId7 = Convert.ToInt32(hfPartId7.Value);
                if (hfPartId8.Value != "")
                    pPartId8 = Convert.ToInt32(hfPartId8.Value);
                if (hfPartId9.Value != "")
                    pPartId9 = Convert.ToInt32(hfPartId9.Value);
                if (hfPartId10.Value != "")
                    pPartId10 = Convert.ToInt32(hfPartId10.Value);
                if (hfPartId11.Value != "")
                    pPartId11 = Convert.ToInt32(hfPartId11.Value);
                if (hfPartId12.Value != "")
                    pPartId12 = Convert.ToInt32(hfPartId12.Value);
                if (hfPartId13.Value != "")
                    pPartId13 = Convert.ToInt32(hfPartId13.Value);
                if (hfPartId14.Value != "")
                    pPartId14 = Convert.ToInt32(hfPartId14.Value);
                if (hfPartId15.Value != "")
                    pPartId15 = Convert.ToInt32(hfPartId15.Value);
                if (hfPartId16.Value != "")
                    pPartId16 = Convert.ToInt32(hfPartId16.Value);
                if (hfPartId17.Value != "")
                    pPartId17 = Convert.ToInt32(hfPartId17.Value);
                if (hfPartId18.Value != "")
                    pPartId18 = Convert.ToInt32(hfPartId18.Value);


                if (Request[ddlAMAns1.UniqueID] as string != "-1")
                {
                    pAMValueId1 = AMValue(ddlAMAns1.SelectedValue.ToString());
                    pStr1 = Convert.ToDecimal(Strength(ddlAMAns1.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns2.UniqueID] as string != "-1")
                {
                    pAMValueId2 = AMValue(ddlAMAns2.SelectedValue.ToString());
                    pStr2 = Convert.ToDecimal(Strength(ddlAMAns2.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns3.UniqueID] as string != "-1")
                {
                    pAMValueId3 = AMValue(ddlAMAns3.SelectedValue.ToString());
                    pStr3 = Convert.ToDecimal(Strength(ddlAMAns3.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns4.UniqueID] as string != "-1")
                {
                    pAMValueId4 = AMValue(ddlAMAns4.SelectedValue.ToString());
                    pStr4 = Convert.ToDecimal(Strength(ddlAMAns4.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns5.UniqueID] as string != "-1")
                {
                    pAMValueId5 = AMValue(ddlAMAns5.SelectedValue.ToString());
                    pStr5 = Convert.ToDecimal(Strength(ddlAMAns5.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns6.UniqueID] as string != "-1")
                {
                    pAMValueId6 = AMValue(ddlAMAns6.SelectedValue.ToString());
                    pStr6 = Convert.ToDecimal(Strength(ddlAMAns6.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns7.UniqueID] as string != "-1")
                {
                    pAMValueId7 = AMValue(ddlAMAns7.SelectedValue.ToString());
                    pStr7 = Convert.ToDecimal(Strength(ddlAMAns7.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns8.UniqueID] as string != "-1")
                {
                    pAMValueId8 = AMValue(ddlAMAns8.SelectedValue.ToString());
                    pStr8 = Convert.ToDecimal(Strength(ddlAMAns8.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns9.UniqueID] as string != "-1")
                {
                    pAMValueId9 = AMValue(ddlAMAns9.SelectedValue.ToString());
                    pStr9 = Convert.ToDecimal(Strength(ddlAMAns9.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns10.UniqueID] as string != "-1")
                {
                    pAMValueId10 = AMValue(ddlAMAns10.SelectedValue.ToString());
                    pStr10 = Convert.ToDecimal(Strength(ddlAMAns10.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns11.UniqueID] as string != "-1")
                {
                    pAMValueId11 = AMValue(ddlAMAns11.SelectedValue.ToString());
                    pStr11 = Convert.ToDecimal(Strength(ddlAMAns11.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns12.UniqueID] as string != "-1")
                {
                    pAMValueId12 = AMValue(ddlAMAns12.SelectedValue.ToString());
                    pStr12 = Convert.ToDecimal(Strength(ddlAMAns12.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns13.UniqueID] as string != "-1")
                {
                    pAMValueId13 = AMValue(ddlAMAns13.SelectedValue.ToString());
                    pStr13 = Convert.ToDecimal(Strength(ddlAMAns13.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns14.UniqueID] as string != "-1")
                {
                    pAMValueId14 = AMValue(ddlAMAns14.SelectedValue.ToString());
                    pStr14 = Convert.ToDecimal(Strength(ddlAMAns14.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns15.UniqueID] as string != "-1")
                {
                    pAMValueId15 = AMValue(ddlAMAns15.SelectedValue.ToString());
                    pStr15 = Convert.ToDecimal(Strength(ddlAMAns15.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns16.UniqueID] as string != "-1")
                {
                    pAMValueId16 = AMValue(ddlAMAns16.SelectedValue.ToString());
                    pStr16 = Convert.ToDecimal(Strength(ddlAMAns16.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns17.UniqueID] as string != "-1")
                {
                    pAMValueId17 = AMValue(ddlAMAns17.SelectedValue.ToString());
                    pStr17 = Convert.ToDecimal(Strength(ddlAMAns17.SelectedValue.ToString()));
                }
                if (Request[ddlAMAns18.UniqueID] as string != "-1")
                {
                    pAMValueId18 = AMValue(ddlAMAns18.SelectedValue.ToString());
                    pStr18 = Convert.ToDecimal(Strength(ddlAMAns18.SelectedValue.ToString()));
                }
                if (txtTotScorePer.Text != "")
                    pTotalAMScorePercen = Convert.ToDecimal(txtTotScorePer.Text);


                if (Mode == "Save")
                {
                    // if (ValidateFieldsForLnApp() == false) return false;
                    DataTable dtDupCAMRec = new DataTable();
                    CMember OMem = new CMember();
                    dtDupCAMRec = OMem.ChkDupCAMRec(vAppId);
                    if (Convert.ToInt32(dtDupCAMRec.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.AjxMsgPopup("CAM Details already exist on this Loan No... CAM Record can not be duplicated...");
                        return false;
                    }
                    vErr = oCA.SaveCAM(pNewId, pCAMId, vAppId, vPurpId, vLnTypeId, pAppLnAmt, pProLnAmt, pProIntRate, pAPPTenure, pProTenure, pNoOfInstal, pTermLn, pCCOD, pCIBILLoanOS,
                      pKFIExistExpos, pProExpos, pTotalDebt, pExistEMI, pProEMI, pCCIntSave, pTotMonLia, pTotalAMScore, pRemarks,
                      pPartId1, pPartId2, pPartId3, pPartId4, pPartId5, pPartId6, pPartId7, pPartId8, pPartId9, pPartId10, pPartId11, pPartId12, pPartId13, pPartId14, pPartId15,
                      pPartId16, pPartId17, pPartId18, pAMValueId1, pAMValueId2, pAMValueId3, pAMValueId4, pAMValueId5, pAMValueId6, pAMValueId7, pAMValueId8, pAMValueId9,
                      pAMValueId10, pAMValueId11, pAMValueId12, pAMValueId13, pAMValueId14, pAMValueId15, pAMValueId16, pAMValueId17, pAMValueId18,
                      pStr1, pStr2, pStr3, pStr4, pStr5, pStr6, pStr7, pStr8, pStr9, pStr10, pStr11, pStr12, pStr13, pStr14, pStr15, pStr16, pStr17, pStr18, pCustId, pNewSancId,
                      pBranchCode, Convert.ToInt32(hdUserID.Value), "Save", 0, pTotalAMScorePercen, pTotEquity, pDERatio, pTotDebtComments, pPBDIT, pDSCR, pTotEquityComments);

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
                else if (Mode == "Edit")
                {
                    vErr = oCA.SaveCAM(pNewId, pCAMId, vAppId, vPurpId, vLnTypeId, pAppLnAmt, pProLnAmt, pProIntRate, pAPPTenure, pProTenure, pNoOfInstal, pTermLn, pCCOD, pCIBILLoanOS,
                      pKFIExistExpos, pProExpos, pTotalDebt, pExistEMI, pProEMI, pCCIntSave, pTotMonLia, pTotalAMScore, pRemarks,
                      pPartId1, pPartId2, pPartId3, pPartId4, pPartId5, pPartId6, pPartId7, pPartId8, pPartId9, pPartId10, pPartId11, pPartId12, pPartId13, pPartId14, pPartId15,
                      pPartId16, pPartId17, pPartId18, pAMValueId1, pAMValueId2, pAMValueId3, pAMValueId4, pAMValueId5, pAMValueId6, pAMValueId7, pAMValueId8, pAMValueId9,
                      pAMValueId10, pAMValueId11, pAMValueId12, pAMValueId13, pAMValueId14, pAMValueId15, pAMValueId16, pAMValueId17, pAMValueId18,
                      pStr1, pStr2, pStr3, pStr4, pStr5, pStr6, pStr7, pStr8, pStr9, pStr10, pStr11, pStr12, pStr13, pStr14, pStr15, pStr16, pStr17, pStr18, pCustId, pNewSancId,
                      pBranchCode, Convert.ToInt32(hdUserID.Value), "Edit", 0, pTotalAMScorePercen, pTotEquity, pDERatio, pTotDebtComments, pPBDIT, pDSCR, pTotEquityComments);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.EditMsg);
                        vResult = true;
                    }

                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dtDelCAMRec = new DataTable();
                    CMember OMem = new CMember();
                    dtDelCAMRec = OMem.ChkDelCAMRecord(vAppId);
                    if (Convert.ToInt32(dtDelCAMRec.Rows[0]["SancRec"]) > 0)
                    {
                        gblFuction.AjxMsgPopup("Onec CAM has been created, it can not be deleted as it is linked with Loan Sanction...");
                        return false;
                    }
                    vErr = oCA.SaveCAM(pNewId, pCAMId, vAppId, vPurpId, vLnTypeId, pAppLnAmt, pProLnAmt, pProIntRate, pAPPTenure, pProTenure, pNoOfInstal, pTermLn, pCCOD, pCIBILLoanOS,
                       pKFIExistExpos, pProExpos, pTotalDebt, pExistEMI, pProEMI, pCCIntSave, pTotMonLia, pTotalAMScore, pRemarks,
                       pPartId1, pPartId2, pPartId3, pPartId4, pPartId5, pPartId6, pPartId7, pPartId8, pPartId9, pPartId10, pPartId11, pPartId12, pPartId13, pPartId14, pPartId15,
                       pPartId16, pPartId17, pPartId18, pAMValueId1, pAMValueId2, pAMValueId3, pAMValueId4, pAMValueId5, pAMValueId6, pAMValueId7, pAMValueId8, pAMValueId9,
                       pAMValueId10, pAMValueId11, pAMValueId12, pAMValueId13, pAMValueId14, pAMValueId15, pAMValueId16, pAMValueId17, pAMValueId18,
                       pStr1, pStr2, pStr3, pStr4, pStr5, pStr6, pStr7, pStr8, pStr9, pStr10, pStr11, pStr12, pStr13, pStr14, pStr15, pStr16, pStr17, pStr18, pCustId, pNewSancId,
                       pBranchCode, Convert.ToInt32(hdUserID.Value), "Delete", 0, pTotalAMScorePercen, pTotEquity, pDERatio, pTotDebtComments, pPBDIT, pDSCR, pTotEquityComments);
                    if (vErr == 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
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
        private Boolean SaveComBackground(string Mode)
        {
            Boolean vResult = false;
            Int32 vError = 0, vNewId = 0, vComBackId = 0;
            string vApplicationId = "", vComBackInfo = "";

            if (hfComBackID.Value != "")
            {
                vComBackId = Convert.ToInt32(hfComBackID.Value);
            }

            vApplicationId = Convert.ToString((Request[txtLnAppNoComBack.UniqueID] as string == null) ? txtLnAppNoComBack.Text : Request[txtLnAppNoComBack.UniqueID] as string);
            vComBackInfo = Convert.ToString((Request[txtComProBack.UniqueID] as string == null) ? txtComProBack.Text : Request[txtComProBack.UniqueID] as string);
            if (vApplicationId == "")
            {
                gblFuction.MsgPopup("Loan Application No Can Not Be Blank... ");
                return false;
            }
            if (vComBackInfo == "")
            {
                gblFuction.MsgPopup("Company Background Information can not be blank...");
                txtComProBack.Focus();
                return false;
            }
            CMember oMem = new CMember();
            try
            {

                if (Mode == "Save")
                {
                    vError = oMem.SaveComBackground(ref vNewId, vComBackId, vApplicationId, vComBackInfo, Convert.ToInt32(hdUserID.Value), "Save", 0);

                    if (vError > 0)
                    {
                        hfComBackID.Value = Convert.ToInt32(vNewId).ToString();
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }

                }
                else if (Mode == "Edit")
                {
                    vError = oMem.SaveComBackground(ref vNewId, vComBackId, vApplicationId, vComBackInfo, Convert.ToInt32(hdUserID.Value), "Edit", 0);

                    if (vError > 0)
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
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dt = new DataTable();
                    dt = oMem.ChkDelComBack(vApplicationId);
                    if (Convert.ToInt32(dt.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made... Can Not Delete Company Background...");
                        return false;
                    }
                    vError = oMem.SaveComBackground(ref vNewId, vComBackId, vApplicationId, vComBackInfo, Convert.ToInt32(hdUserID.Value), "Delete", 0);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
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
        private Boolean SaveGrpComInfo(string Mode)
        {
            Boolean vResult = false;
            Int32 vError = 0, vNewId = 0, vGrpComBackId = 0;
            string vApplicationId = "", vGrpComInfo = "";

            if (hdGrpComInfoId.Value != "")
            {
                vGrpComBackId = Convert.ToInt32(hdGrpComInfoId.Value);
            }

            vApplicationId = Convert.ToString((Request[txtLnAppNoGrpComInfo.UniqueID] as string == null) ? txtLnAppNoGrpComInfo.Text : Request[txtLnAppNoGrpComInfo.UniqueID] as string);
            vGrpComInfo = Convert.ToString((Request[txtGrpComInfo.UniqueID] as string == null) ? txtGrpComInfo.Text : Request[txtGrpComInfo.UniqueID] as string);

            if (vGrpComInfo == "")
            {
                gblFuction.MsgPopup("Group Company Information can not be blank...");
                txtGrpComInfo.Focus();
                return false;
            }

            CMember oMem = new CMember();
            try
            {

                if (Mode == "Save")
                {
                    vError = oMem.SaveGrpCompInfo(ref vNewId, vGrpComBackId, vApplicationId, vGrpComInfo, Convert.ToInt32(hdUserID.Value), "Save", 0);

                    if (vError > 0)
                    {
                        hfComBackID.Value = Convert.ToInt32(vNewId).ToString();
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }

                }
                else if (Mode == "Edit")
                {
                    vError = oMem.SaveGrpCompInfo(ref vNewId, vGrpComBackId, vApplicationId, vGrpComInfo, Convert.ToInt32(hdUserID.Value), "Edit", 0);

                    if (vError > 0)
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
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dt = new DataTable();
                    dt = oMem.ChkDelComBack(vApplicationId);
                    if (Convert.ToInt32(dt.Rows[0]["CAMRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report has been made... Can Not Delete Group Company Information...");
                        return false;
                    }
                    vError = oMem.SaveGrpCompInfo(ref vNewId, vGrpComBackId, vApplicationId, vGrpComInfo, Convert.ToInt32(hdUserID.Value), "Delete", 0);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
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
        private Boolean SavePromoterBackground(string Mode)
        {
            Boolean vResult = false;
            Int32 vError = 0, vNewId = 0, vPromoterBackID = 0;
            string vApplicationId = "", vPromBakground = "";

            if (hfPromoBackID.Value != "")
            {
                vPromoterBackID = Convert.ToInt32(hfPromoBackID.Value);
            }

            vApplicationId = Convert.ToString((Request[txtLnAppNoProBack.UniqueID] as string == null) ? txtLnAppNoProBack.Text : Request[txtLnAppNoProBack.UniqueID] as string);
            vPromBakground = Convert.ToString((Request[txtProBack.UniqueID] as string == null) ? txtProBack.Text : Request[txtProBack.UniqueID] as string);
            if (vPromBakground == "")
            {
                gblFuction.MsgPopup("Promoter Backgrounf Information can not be blank...");
                txtProBack.Focus();
                return false;
            }
            CMember oMem = new CMember();
            try
            {
                if (Mode == "Save")
                {
                    vError = oMem.SavePromoBackground(ref vNewId, vPromoterBackID, vApplicationId, vPromBakground, Convert.ToInt32(hdUserID.Value), "Save", 0);

                    if (vError > 0)
                    {
                        hfComBackID.Value = Convert.ToInt32(vNewId).ToString();
                        gblFuction.MsgPopup(gblPRATAM.SaveMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
                        vResult = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup(gblPRATAM.DBError);
                        tdMsg.Visible = true;
                        vResult = false;
                    }
                }
                else if (Mode == "Edit")
                {
                    vError = oMem.SavePromoBackground(ref vNewId, vPromoterBackID, vApplicationId, vPromBakground, Convert.ToInt32(hdUserID.Value), "Edit", 0);
                    if (vError > 0)
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
                        vResult = false;
                    }
                }
                else if (Mode == "Delete")
                {
                    DataTable dt = new DataTable();
                    dt = oMem.ChkDelPromoBack(vApplicationId);
                    if (Convert.ToInt32(dt.Rows[0]["PromoBackRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Report already made for this Loan Application No.. You can not delete Promoter background data..");
                        return false;
                    }
                    vError = oMem.SavePromoBackground(ref vNewId, vPromoterBackID, vApplicationId, vPromBakground, Convert.ToInt32(hdUserID.Value), "Delete", 0);

                    if (vError > 0)
                    {
                        gblFuction.MsgPopup(gblPRATAM.DeleteMsg);
                        tdMsg.Visible = true;
                        lblValMsgApp.Text = "";
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
        private Boolean SaveCheckListDtl(string Mode)
        {
            Boolean vResult = false;
            DataRow dr = null;
            DataTable dtXml = new DataTable();
            dtXml.Columns.Add(new DataColumn("ChkListHeadID"));
            dtXml.Columns.Add(new DataColumn("ChkListHead"));
            dtXml.Columns.Add(new DataColumn("CheckDate"));
            dtXml.Columns.Add(new DataColumn("Remarks"));
            dtXml.Columns.Add(new DataColumn("Comments"));
            dtXml.Columns.Add(new DataColumn("CheckedBy"));

            foreach (GridViewRow gr in gvCheckList.Rows)
            {
                if (((TextBox)gr.FindControl("txtCheckDate")).Text != "" && ((DropDownList)gr.FindControl("ddlCommandId")).SelectedValue != "-1")
                {
                    dr = dtXml.NewRow();
                    dr["ChkListHeadID"] = ((Label)gr.FindControl("lblChkListHeadID")).Text;
                    dr["ChkListHead"] = ((Label)gr.FindControl("lblChkListHead")).Text;
                    dr["CheckDate"] = gblFuction.setDate(((TextBox)gr.FindControl("txtCheckDate")).Text);
                    dr["Remarks"] = ((TextBox)gr.FindControl("txtRemarks")).Text;
                    dr["Comments"] = ((DropDownList)gr.FindControl("ddlCommandId")).SelectedValue;
                    dr["CheckedBy"] = ((TextBox)gr.FindControl("txtCheckedBy")).Text;
                    dtXml.Rows.Add(dr);
                    dtXml.AcceptChanges();
                }
            }
            dtXml.TableName = "Table1";
            Int32 vChkListId = 0;
            string vLnAppId = "";
            if (hfCheckListId.Value != "")
            {
                vChkListId = Convert.ToInt32(hfCheckListId.Value);
            }
            vLnAppId = Convert.ToString((Request[txtChkListLnAppNo.UniqueID] as string == null) ? txtChkListLnAppNo.Text : Request[txtChkListLnAppNo.UniqueID] as string);
            string vXml = "";
            CMember oMem = new CMember();
            try
            {

                using (StringWriter oSW = new StringWriter())
                {
                    dtXml.WriteXml(oSW);
                    vXml = oSW.ToString().Replace("12:00:00AM", "");
                }
                Int32 vErr = 0, vCheckListID = 0;
                string vMsg = "";


                if (hfCheckListId.Value.ToString() != "")
                {
                    vCheckListID = Convert.ToInt32(hfCheckListId.Value);
                }

                if (Mode == "Save")
                {
                    //if (ValidateApplicantField() == false) return false;
                    vErr = oMem.SaveCheckListDtl(vCheckListID, vLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Save", 0);
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
                    vErr = oMem.SaveCheckListDtl(vCheckListID, vLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Edit", 0);
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
                    DataTable dt = new DataTable();
                    dt = oMem.ChkDelCheckList(vLnAppId);
                    if (Convert.ToInt32(dt.Rows[0]["CheckListRec"]) > 0)
                    {
                        gblFuction.MsgPopup("CAM Reort has been completed... Can Not Delete Check List Details...");
                        return false;
                    }
                    vErr = oMem.SaveCheckListDtl(vCheckListID, vLnAppId, vXml, Convert.ToInt32(hdUserID.Value), "Delete", 0);
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
        //private void popSourceDesignation()
        //{
        //    DataTable dt = null;
        //    CGblIdGenerator oGb = null;
        //    try
        //    {
        //        oGb = new CGblIdGenerator();
        //        dt = oGb.PopComboMIS("N", "N", "AA", "SDesignationID", "SDesignation", "SourceDesignationMst", 0, "AA", "AA", gblFuction.setDate(Session[gblValue.LoginDate].ToString()), "0000");
        //        ddlSDesignation.DataSource = dt;
        //        if (dt.Rows.Count > 0)
        //        {
        //            ddlSDesignation.DataTextField = "SDesignation";
        //            ddlSDesignation.DataValueField = "SDesignationID";
        //            ddlSDesignation.DataBind();
        //            ListItem oli1 = new ListItem("<--Select-->", "-1");
        //            ddlSDesignation.Items.Insert(0, oli1);
        //        }
        //    }
        //    finally
        //    {
        //        oGb = null;
        //        dt = null;
        //    }
        //}
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
                }
                else
                {
                    ddlCoAppOccu.DataSource = null;
                    ddlCoAppOccu.DataBind();
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
                }
                else
                {
                    ddlCoAppGender.DataSource = null;
                    ddlCoAppGender.DataBind();
                    ddlCustGen.DataSource = null;
                    ddlCustGen.DataBind();
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
                    ddlCoAppOccu.DataSource = dt;
                    ddlCoAppOccu.DataTextField = "Occupation";
                    ddlCoAppOccu.DataValueField = "OccupationId";
                    ddlCoAppOccu.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlCoAppOccu.Items.Insert(0, oli1);

                    ddlCustOccupation.DataSource = dt;
                    ddlCustOccupation.DataTextField = "Occupation";
                    ddlCustOccupation.DataValueField = "OccupationId";
                    ddlCustOccupation.DataBind();
                    ListItem oli2 = new ListItem("<--Select-->", "-1");
                    ddlCustOccupation.Items.Insert(0, oli2);
                }
                else
                {
                    ddlCoAppOccu.DataSource = null;
                    ddlCoAppOccu.DataBind();
                    ddlCustOccupation.DataSource = null;
                    ddlCustOccupation.DataBind();
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
        private void PopApplicant()
        {
            string vBrCode = (string)Session[gblValue.BrnchCode];
            CMember oMem = new CMember();
            DataTable dt = null;
            try
            {
                dt = oMem.GetApplicantList(vBrCode);
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

                    ddlLnPur.DataSource = dt;
                    ddlLnPur.DataTextField = "PurposeName";
                    ddlLnPur.DataValueField = "PurposeId";
                    ddlLnPur.DataBind();
                    ListItem oli1 = new ListItem("<--Select-->", "-1");
                    ddlLnPur.Items.Insert(0, oli1);
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
        private void PopCBName()
        {
            //CMember oMem = new CMember();
            //DataTable dt = null;
            //try
            //{
            //    dt = oMem.GetCBName();
            //    if (dt.Rows.Count > 0)
            //    {
            //        ddlCBName.DataSource = dt;
            //        ddlCBName.DataTextField = "CBName";
            //        ddlCBName.DataValueField = "CBNameId";
            //        ddlCBName.DataBind();
            //        ListItem oli = new ListItem("<--Select-->", "-1");
            //        ddlCBName.Items.Insert(0, oli);
            //    }
            //    else
            //    {
            //        ddlCBName.DataSource = null;
            //        ddlCBName.DataBind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    dt = null;
            //    oMem = null;
            //}
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

                    //ddlCAMLnScheme

                    ddlCAMLnScheme.DataTextField = "LoanTypeName";
                    ddlCAMLnScheme.DataValueField = "LoanTypeId";
                    ddlCAMLnScheme.DataSource = dt;
                    ddlCAMLnScheme.DataBind();
                    ListItem oItm1 = new ListItem();
                    oItm1.Text = "<--- Select --->";
                    oItm1.Value = "-1";
                    ddlCAMLnScheme.Items.Insert(0, oItm1);
                }
                else
                {
                    ddlLnScheme.DataSource = null;
                    ddlLnScheme.DataBind();
                    ddlCAMLnScheme.DataSource = null;
                    ddlCAMLnScheme.DataBind();
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
        private void GetComGLForBalShet()
        {
            CMember oMem = new CMember();
            DataSet ds = null;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            try
            {
                ds = oMem.GetGenLedForBalSheet();
                if (ds.Tables.Count > 0)
                {
                    dt1 = ds.Tables[0];
                    dt2 = ds.Tables[1];
                    if (dt1.Rows.Count > 0)
                    {
                        gvLiability.DataSource = dt1;
                        gvLiability.DataBind();
                    }
                    else
                    {
                        gvLiability.DataSource = null;
                        gvLiability.DataBind();
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        gvAsset.DataSource = dt2;
                        gvAsset.DataBind();
                    }
                    else
                    {
                        gvAsset.DataSource = null;
                        gvAsset.DataBind();
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
                ds = null;
                oMem = null;
            }
        }
        private void GetComGLForPL()
        {
            CMember oMem = new CMember();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            try
            {
                ds = oMem.GetGenLedForPL();
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                }
                if (dt.Rows.Count > 0)
                {
                    gvIncome.DataSource = dt;
                    gvIncome.DataBind();
                }
                else
                {
                    gvIncome.DataSource = null;
                    gvIncome.DataBind();
                }
                if (dt1.Rows.Count > 0)
                {
                    gvExpense.DataSource = dt1;
                    gvExpense.DataBind();
                }
                else
                {
                    gvExpense.DataSource = null;
                    gvExpense.DataBind();
                }
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
                oMem = null;
            }
        }
        private void GetCheckListHeadList()
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = oMem.GetCheckListHeadList();
                if (dt.Rows.Count > 0)
                {
                    gvCheckList.DataSource = dt;
                    gvCheckList.DataBind();
                }
                else
                {
                    gvCheckList.DataSource = null;
                    gvCheckList.DataBind();
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
        private void GetCheckListComments()
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = oMem.GetCheckListComment();

                if (dt.Rows.Count > 0)
                {
                    gvCheckList.DataSource = dt;
                    gvCheckList.DataBind();
                }
                else
                {
                    gvCheckList.DataSource = null;
                    gvCheckList.DataBind();
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
            if (txtMState.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Mailing State..");
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
            if (txtRState.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Office State..");
                vResult = false;
            }
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

            if (Request[ddlCoAppPhotoIdType.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Photo Proof Id..");
                vResult = false;
            }

            if (txtCoAppPNo.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Photo Proof Id No..");
                vResult = false;
            }


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
            if (Request[ddlCoAppDirePart.UniqueID] as string == "-1")
            {
                gblFuction.MsgPopup("Please select Director/Partner field..");
                vResult = false;
            }


            if (txtCoAppAddress1Pre.Text.Trim() == "")
            {
                gblFuction.MsgPopup("Please input Present Address 1..");
                vResult = false;
            }
            if (txtCoAppStatePre.Text.Trim() == "")
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
            if (txtCoAppStatePer.Text.Trim() == "")
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
            txtMStateShow.Text = "";
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
            txtCoAppStatePre.Text = "";
            txtCoAppDistPre.Text = "";
            txtCOAppPINPre.Text = "";
            txtCoAppAddress1Per.Text = "";
            txtCoAppAddress2Per.Text = "";
            txtCoAppStatePer.Text = "";
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
            txtMState.Text = "";
            txtMDist.Text = "";
            txtMPIN.Text = "";
            txtMMobNo.Text = "";
            txtMSTD.Text = "";
            txtMTel.Text = "";
            txtROffAdd1.Text = "";
            txtROffAdd2.Text = "";
            txtRState.Text = "";
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
            txtTenure.Text = "";
            txtLnPurposeDetails.Text = "";
            txtAddTerms.Text = "";
        }
        private void ClearBankAccControl()
        {
            txtAppIdBankAc.Text = "";
            txtAcHolName.Text = "";
            txtBankAcSrNo.Text = "";
            txtBankName.Text = "";
            ddlACType.SelectedValue = "0";
            txtAccNo.Text = "";
            txtBankSinYr.Text = "";
            //gvBankDtl.DataSource = null;
            //gvBankDtl.DataBind();
            txtBankAcRemarks.Text = "";
            txtTransLimit.Text = "";
            txtCurrBalance.Text = "";
        }
        private void ClearCBControl()
        {
            //txtReportDate.Text = "";
            //txtReportId.Text = "";
            //ddlCBName.SelectedIndex = -1;
            //txtOrgNameCB.Text = "";
            //txtSRNo.Text = "";
            //txtLoanTypeCB.Text = "";
            //txtLnDtCB.Text = "";
            //txtLnAmtCB.Text = "";
            //txtEMIAmt.Text = "";
            //txtTenureCB.Text = "";
            //txtEMIPaidCB.Text = "";
            //txtPOSAmtCB.Text = "";

            //chbActiveCB.Checked = false;
            //txtBankValName.Text = "";
            //txtPMn1.Text = "";
            //txtPMn2.Text = "";
            //txtPMn3.Text = "";
            //txtPMn4.Text = "";
            //txtPMn5.Text = "";
            //txtPMn6.Text = "";
        }
        private void ClearBSControl()
        {
            // txtSRNoBS.Text = "";
            //ddlParticulars.SelectedIndex = -1;
            txtBSDate.Text = "";
            txtTotAsset.Text = "";
            txtTotLiability.Text = "";
            //txtBSAmt.Text = "";
            //txtChangeBS.Text = "";
            //txtRemarksBS.Text = "";
        }
        private void ClearPLControl()
        {
            // txtSlNoPL.Text = "";
            //ddlParticularsPL.SelectedIndex = -1;
            txtPLDate.Text = "";
            txtTotIncome.Text = "";
            txtTotExpense.Text = "";
            txtNetProfit.Text = "";
            //txtPLAmt.Text = "";
            //txtChangePL.Text = "";
            //txtRemarksPL.Text = "";
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
            txtStatus.Text = "";
        }
        private void ClearCAMControl()
        {
            txtAppNoAM.Text = "";
            ddlLnPur.SelectedIndex = -1;
            ddlCAMLnScheme.SelectedIndex = -1;
            txtLnAmt.Text = "";
            txtProLnAmt.Text = "";
            txtProInstRate.Text = "";
            txtAppTenure.Text = "";
            txtProTenure.Text = "";
            txtProNoofInst.Text = "";
            txtTermLn.Text = "";
            txtCCorOD.Text = "";
            txtCIBILLnOutst.Text = "";
            txtKFIExpos.Text = "";
            txtProExpos.Text = "";
            txtTotalDebtManual.Text = "";
            txtTotEquityManual.Text = "";
            txtDEManual.Text = "";
            txtTotDebtComments.Text = "";
            txtExistingEMI.Text = "";
            txtProEMI.Text = "";
            txtCCInstSav.Text = "";
            txtTotMonlylia.Text = "";
            txtPBDIT.Text = "";
            txtCustDocNameCR.Text = "";
            txtTotEquityComments.Text = "";
            txtCAMRemarks.Text = "";
            txttotScore.Text = "";
            txtTotScorePer.Text = "";
        }
        private void ClearCompanyBackground()
        {
            txtLnAppNoComBack.Text = "";
            txtComProBack.Text = "";
        }
        private void ClearPromoterBackground()
        {
            txtLnAppNoProBack.Text = "";
            txtProBack.Text = "";
        }
        private void ClearCheckList()
        {
            txtChkListLnAppNo.Text = "";
        }
        private void ClearGrpCompInfo()
        {
            txtLnAppNoGrpComInfo.Text = "";
            txtGrpComInfo.Text = "";
        }
        protected void ddlACType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlACType.SelectedValue == "0")
                {
                    gblFuction.AjxMsgPopup("Please Select Account Type");
                    return;
                }
                else if (ddlACType.SelectedValue == "OD" || ddlACType.SelectedValue == "CC")
                {
                    txtTransLimit.Enabled = true;
                }
                else
                {
                    txtTransLimit.Text = "";
                    txtTransLimit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlAMAns1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns6_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns7_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns8_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns9_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns10_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns11_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns12_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns13_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns14_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns15_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns16_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns17_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        protected void ddlAMAns18_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScoreSum();
        }
        //protected void ddlLnScheme_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ShowMachDetails();
        //}
        //private void ShowMachDetails()
        //{
        //    if (ddlLnScheme.SelectedValue == "2")
        //        txtLnPurposeDetails.Enabled = true;
        //    else
        //    {
        //        txtLnPurposeDetails.Text = "";
        //        txtLnPurposeDetails.Enabled = false;
        //    }

        //}
        protected void ddlCommandId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCommandId = (DropDownList)sender;
                GridViewRow gR = (GridViewRow)ddlCommandId.NamingContainer;
                TextBox txtCheckDate = (TextBox)gR.FindControl("txtCheckDate");
                TextBox txtRemarks = (TextBox)gR.FindControl("txtRemarks");
                TextBox txtCheckedBy = (TextBox)gR.FindControl("txtCheckedBy");
                DropDownList ddlCommand = (DropDownList)gR.FindControl("ddlCommandId");
                if (ddlCommand.SelectedValue == "5")
                {
                    txtCheckDate.Enabled = false;
                    txtRemarks.Enabled = false;
                    txtCheckedBy.Enabled = false;
                }
                else
                {
                    txtCheckDate.Enabled = true;
                    txtRemarks.Enabled = true;
                    txtCheckedBy.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void ddlLnScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlLnScheme.SelectedValue != "-1")
            {
                int LnType = Convert.ToInt32(ddlLnScheme.SelectedValue);
                EnableMachinDtl(LnType);
            }
        }
        protected void txtTermLn_TextChanged(object sender, EventArgs e)
        {
            SumDebt();
        }

        protected void txtCCorOD_TextChanged(object sender, EventArgs e)
        {
            SumDebt();
        }
        protected void txtCIBILLnOutst_TextChanged(object sender, EventArgs e)
        {
            SumDebt();
        }
        protected void txtKFIExpos_TextChanged(object sender, EventArgs e)
        {
            SumDebt();
        }
        protected void txtProExpos_TextChanged(object sender, EventArgs e)
        {
            SumDebt();
        }
        protected void txtTotEquityManual_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateDE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CalculateDE()
        {
            Double TotDebt = 0;
            Double TotEquity = 0;
            Double DE = 0;
            if (txtTotalDebtManual.Text != "")
                TotDebt = Convert.ToDouble(txtTotalDebtManual.Text);
            if (txtTotEquityManual.Text != "")
                TotEquity = Convert.ToDouble(txtTotEquityManual.Text);
            if (TotEquity != 0)
                DE = Math.Round((TotDebt / TotEquity), 2);
            txtDEManual.Text = DE.ToString("0.00");
        }
        protected void txtPBDIT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateDSCR();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void CalculateDSCR()
        {
            Double TotLiability = 0;
            Double PBDIT = 0;
            Double DSCR = 0;
            if (txtTotMonlylia.Text != "")
                TotLiability = Convert.ToDouble(txtTotMonlylia.Text);
            if (txtPBDIT.Text != "")
                PBDIT = Convert.ToDouble(txtPBDIT.Text);
            if (TotLiability != 0)
                DSCR = Math.Round((PBDIT / TotLiability), 2);
            txtCustDocNameCR.Text = DSCR.ToString("0.00");
        }
        protected void txtExistingEMI_TextChanged(object sender, EventArgs e)
        {
            SumLiability();
        }
        protected void txtProEMI_TextChanged(object sender, EventArgs e)
        {
            SumLiability();
        }
        protected void txtCCInstSav_TextChanged(object sender, EventArgs e)
        {
            SumLiability();
        }
        private void ScoreSum()
        {
            double stren1 = 0;
            double stren2 = 0;
            double stren3 = 0;
            double stren4 = 0;
            double stren5 = 0;
            double stren6 = 0;
            double stren7 = 0;
            double stren8 = 0;
            double stren9 = 0;
            double stren10 = 0;
            double stren11 = 0;
            double stren12 = 0;
            double stren13 = 0;
            double stren14 = 0;
            double stren15 = 0;
            double stren16 = 0;
            double stren17 = 0;
            double stren18 = 0;

            double Maxstren1 = 0;
            double Maxstren2 = 0;
            double Maxstren3 = 0;
            double Maxstren4 = 0;
            double Maxstren5 = 0;
            double Maxstren6 = 0;
            double Maxstren7 = 0;
            double Maxstren8 = 0;
            double Maxstren9 = 0;
            double Maxstren10 = 0;
            double Maxstren11 = 0;
            double Maxstren12 = 0;
            double Maxstren13 = 0;
            double Maxstren14 = 0;
            double Maxstren15 = 0;
            double Maxstren16 = 0;
            double Maxstren17 = 0;
            double Maxstren18 = 0;

            double TotScore = 0;
            double TotScoreMax = 0;
            double TotScorePer = 0;
            try
            {
                if (Request[ddlAMAns1.UniqueID] as string == "-1")
                {
                    stren1 = 0;
                }
                else
                {
                    stren1 = Strength(ddlAMAns1.SelectedValue.ToString());
                    Maxstren1 = 3;
                }

                if (Request[ddlAMAns2.UniqueID] as string == "-1")
                {
                    stren2 = 0;
                }
                else
                {
                    stren2 = Strength(ddlAMAns2.SelectedValue.ToString());
                    Maxstren2 = 3;

                }
                if (Request[ddlAMAns3.UniqueID] as string == "-1")
                {
                    stren3 = 0;
                }
                else
                {
                    stren3 = Strength(ddlAMAns3.SelectedValue.ToString());
                    Maxstren3 = 3;
                }
                if (Request[ddlAMAns4.UniqueID] as string == "-1")
                {
                    stren4 = 0;
                }
                else
                {
                    stren4 = Strength(ddlAMAns4.SelectedValue.ToString());
                    Maxstren4 = 3;
                }
                if (Request[ddlAMAns5.UniqueID] as string == "-1")
                {
                    stren5 = 0;
                }
                else
                {
                    stren5 = Strength(ddlAMAns5.SelectedValue.ToString());
                    Maxstren5 = 3;
                }

                if (Request[ddlAMAns6.UniqueID] as string == "-1")
                {
                    stren6 = 0;
                }
                else
                {
                    stren6 = Strength(ddlAMAns6.SelectedValue.ToString());
                    Maxstren6 = 3;
                }
                if (Request[ddlAMAns7.UniqueID] as string == "-1")
                {
                    stren7 = 0;
                }
                else
                {
                    stren7 = Strength(ddlAMAns7.SelectedValue.ToString());
                    Maxstren7 = 3;
                }
                if (Request[ddlAMAns8.UniqueID] as string == "-1")
                {
                    stren8 = 0;
                }
                else
                {
                    stren8 = Strength(ddlAMAns8.SelectedValue.ToString());
                    Maxstren8 = 3;
                }
                if (Request[ddlAMAns9.UniqueID] as string == "-1")
                {
                    stren9 = 0;
                }
                else
                {
                    stren9 = Strength(ddlAMAns9.SelectedValue.ToString());
                    Maxstren9 = 3;
                }
                if (Request[ddlAMAns10.UniqueID] as string == "-1")
                {
                    stren10 = 0;
                }
                else
                {
                    stren10 = Strength(ddlAMAns10.SelectedValue.ToString());
                    Maxstren10 = 3;
                }
                if (Request[ddlAMAns11.UniqueID] as string == "-1")
                {
                    stren11 = 0;
                }
                else
                {
                    stren11 = Strength(ddlAMAns11.SelectedValue.ToString());
                    Maxstren11 = 3;
                }
                if (Request[ddlAMAns12.UniqueID] as string == "-1")
                {
                    stren12 = 0;
                }
                else
                {
                    stren12 = Strength(ddlAMAns12.SelectedValue.ToString());
                    Maxstren12 = 3;
                }
                if (Request[ddlAMAns13.UniqueID] as string == "-1")
                {
                    stren13 = 0;
                }
                else
                {
                    stren13 = Strength(ddlAMAns13.SelectedValue.ToString());
                    Maxstren13 = 3;
                }
                if (Request[ddlAMAns14.UniqueID] as string == "-1")
                {
                    stren14 = 0;
                }
                else
                {
                    stren14 = Strength(ddlAMAns14.SelectedValue.ToString());
                    Maxstren14 = 3;
                }
                if (Request[ddlAMAns15.UniqueID] as string == "-1")
                {
                    stren15 = 0;
                }
                else
                {
                    stren15 = Strength(ddlAMAns15.SelectedValue.ToString());
                    Maxstren15 = 3;
                }
                if (Request[ddlAMAns16.UniqueID] as string == "-1")
                {
                    stren16 = 0;
                }
                else
                {
                    stren16 = Strength(ddlAMAns16.SelectedValue.ToString());
                    Maxstren16 = 3;
                }
                if (Request[ddlAMAns17.UniqueID] as string == "-1")
                {
                    stren17 = 0;
                }
                else
                {
                    stren17 = Strength(ddlAMAns17.SelectedValue.ToString());
                    Maxstren17 = 3;
                }
                if (Request[ddlAMAns18.UniqueID] as string == "-1")
                {
                    stren18 = 0;
                }
                else
                {
                    stren18 = Strength(ddlAMAns18.SelectedValue.ToString());
                    Maxstren18 = 3;
                }
                // In case of Business Loan and Short Term Loan we are not considering value of Machine type and Machine Supplier in Total Score and % Calculation
                if (ddlCAMLnScheme.SelectedValue == "1" || ddlCAMLnScheme.SelectedValue == "3")
                {
                    TotScore = (stren1 + stren2 + stren3 + stren4 + stren5 + stren6 + stren7 + stren8 + stren9 + stren10 + stren11 + stren12 + stren13 + stren14 + stren15 + stren16);
                    TotScoreMax = (Maxstren1 + Maxstren2 + Maxstren3 + Maxstren4 + Maxstren5 + Maxstren6 + Maxstren7 + Maxstren8 + Maxstren9 + Maxstren10 + Maxstren11 + Maxstren12 + Maxstren13 + Maxstren14 + Maxstren15 + Maxstren16);
                    TotScorePer = ((TotScore / TotScoreMax) * 100);
                }
                else
                {
                    TotScore = (stren1 + stren2 + stren3 + stren4 + stren5 + stren6 + stren7 + stren8 + stren9 + stren10 + stren11 + stren12 + stren13 + stren14 + stren15 + stren16 + stren17 + stren18);
                    TotScoreMax = (Maxstren1 + Maxstren2 + Maxstren3 + Maxstren4 + Maxstren5 + Maxstren6 + Maxstren7 + Maxstren8 + Maxstren9 + Maxstren10 + Maxstren11 + Maxstren12 + Maxstren13 + Maxstren14 + Maxstren15 + Maxstren16 + Maxstren17 + Maxstren18);
                    TotScorePer = ((TotScore / TotScoreMax) * 100);
                }
                txttotScore.Text = TotScore.ToString();
                txtTotScorePer.Text = TotScorePer.ToString("0.00");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void SumDebt()
        {
            double termloan = 0;
            double CCOD = 0;
            double CIBILLn = 0;
            double KFIExpos = 0;
            double ProExpose = 0;
            try
            {
                if (txtTermLn.Text != "")
                    termloan = Convert.ToDouble(txtTermLn.Text);

                if (txtCCorOD.Text != "")
                    CCOD = Convert.ToDouble(txtCCorOD.Text);

                if (txtCIBILLnOutst.Text != "")
                    CIBILLn = Convert.ToDouble(txtCIBILLnOutst.Text);

                if (txtKFIExpos.Text != "")
                    KFIExpos = Convert.ToDouble(txtKFIExpos.Text);

                if (txtProExpos.Text != "")
                    ProExpose = Convert.ToDouble(txtProExpos.Text);

                txtTotalDebtManual.Text = Math.Round((termloan + CCOD + CIBILLn + KFIExpos + ProExpose), 2).ToString();
                CalculateDE();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void SumLiability()
        {
            double ExistingEMI = 0;
            double ProposedEMI = 0;
            double CCInstSaving = 0;
            try
            {
                if (txtExistingEMI.Text != "")
                    ExistingEMI = Convert.ToDouble(txtExistingEMI.Text);

                if (txtProEMI.Text != "")
                    ProposedEMI = Convert.ToDouble(txtProEMI.Text);

                if (txtCCInstSav.Text != "")
                    CCInstSaving = Convert.ToDouble(txtCCInstSav.Text);

                txtTotMonlylia.Text = Math.Round((ExistingEMI + ProposedEMI + CCInstSaving), 2).ToString();
                CalculateDSCR();
            }
            catch (Exception ex)
            {
                throw ex;
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
                    txtCoAppStatePre.Text = dt.Rows[0]["MState"].ToString();
                    txtCoAppDistPre.Text = dt.Rows[0]["MDistrict"].ToString();
                    txtCOAppPINPre.Text = dt.Rows[0]["MPIN"].ToString();

                    txtCoAppAddress1Per.Text = dt.Rows[0]["RAddress1"].ToString();
                    txtCoAppAddress2Per.Text = dt.Rows[0]["RAddress2"].ToString();
                    txtCoAppStatePer.Text = dt.Rows[0]["RState"].ToString();
                    txtCoAppDistPer.Text = dt.Rows[0]["RDistrict"].ToString();
                    txtCoAppPINPer.Text = dt.Rows[0]["RPIN"].ToString();
                }
                else
                {
                    txtCoAppAddress1Pre.Text = "";
                    txtCoAppAddress2Pre.Text = "";
                    txtCoAppStatePre.Text = "";
                    txtCoAppDistPre.Text = "";
                    txtCOAppPINPre.Text = "";
                    txtCoAppAddress1Per.Text = "";
                    txtCoAppAddress2Per.Text = "";
                    txtCoAppStatePer.Text = "";
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
                    txtCoAppStatePre.Text = "";
                    txtCoAppDistPre.Text = "";
                    txtCOAppPINPre.Text = "";
                    txtCoAppAddress1Per.Text = "";
                    txtCoAppAddress2Per.Text = "";
                    txtCoAppStatePer.Text = "";
                    txtCoAppDistPer.Text = "";
                    txtCoAppPINPer.Text = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnBackComProfile_Click(object sender, EventArgs e)
        {
            ClearCompanyBackground();
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 7;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnBackProBack_Click(object sender, EventArgs e)
        {
            ClearPromoterBackground();
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 7;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }
        protected void btnBackGrpComInfo_Click(object sender, EventArgs e)
        {
            ClearGrpCompInfo();
            mView.ActiveViewIndex = 0;
            tbMem.ActiveTabIndex = 7;
            ViewAcess();
            LoadBSList(ViewState["LoanAppId"].ToString());
            LoadPLList(ViewState["LoanAppId"].ToString());
        }

        protected void lbComProfile_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            DataTable dtFSPass = new DataTable();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Company Profile Background");
                    return;
                }
                else
                {
                    txtLnAppNoComBack.Text = ViewState["LoanAppId"].ToString();
                    dt = OMem.GetComBackgroundByLoanId(ViewState["LoanAppId"].ToString());
                    dtFSPass = OMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtFSPass.Rows.Count > 0)
                    {
                        if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Company Profile Background as Financial Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to add Company Profile Background... ");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        txtLnAppNoComBack.Text = dt.Rows[0]["LoanAppId"].ToString();
                        txtComProBack.Text = dt.Rows[0]["ComBakground"].ToString();
                        hfComBackID.Value = dt.Rows[0]["ComBackID"].ToString();
                        btnSaveComProfile.Enabled = false;
                        btnUpdateComProfile.Enabled = true;
                        btnDelComProfile.Enabled = true;
                        btnBackComProfile.Enabled = true;
                    }
                    else
                    {
                        txtComProBack.Text = "";
                        btnSaveComProfile.Enabled = true;
                        btnUpdateComProfile.Enabled = false;
                        btnDelComProfile.Enabled = false;
                        btnBackComProfile.Enabled = true;
                    }
                    mView.ActiveViewIndex = 10;
                    ViewAcess();
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
                dtFSPass = null;
            }

        }
        protected void lbPromerBack_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            DataTable dtFSPass = new DataTable();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Promoter Background");
                    return;
                }
                else
                {
                    txtLnAppNoProBack.Text = ViewState["LoanAppId"].ToString();
                    dt = OMem.GetPromoBackgroundDataByLoanId(ViewState["LoanAppId"].ToString());
                    dtFSPass = OMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtFSPass.Rows.Count > 0)
                    {
                        if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Promoter Background as Financial Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to add Promoter Background... ");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        txtLnAppNoProBack.Text = dt.Rows[0]["LoanAppId"].ToString();
                        txtProBack.Text = dt.Rows[0]["PromBakground"].ToString();
                        hfPromoBackID.Value = dt.Rows[0]["PromoterBackID"].ToString();
                        btnSaveProBack.Enabled = false;
                        btnUpdateProBack.Enabled = true;
                        btnDelProBack.Enabled = true;
                        btnBackProBack.Enabled = true;
                    }
                    else
                    {
                        txtProBack.Text = "";
                        btnSaveProBack.Enabled = true;
                        btnUpdateProBack.Enabled = false;
                        btnDelProBack.Enabled = false;
                        btnBackProBack.Enabled = true;
                    }
                    mView.ActiveViewIndex = 11;
                    ViewAcess();
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
        protected void lbGrpComInfo_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            DataTable dtFSPass = new DataTable();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to add Group Company Information");
                    return;
                }
                else
                {
                    txtLnAppNoGrpComInfo.Text = ViewState["LoanAppId"].ToString();
                    dt = OMem.GetGrpComInfoDataByLoanId(ViewState["LoanAppId"].ToString());
                    dtFSPass = OMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtFSPass.Rows.Count > 0)
                    {
                        if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Add Group Company Information as Financial Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to add Group Company Information... ");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        txtLnAppNoGrpComInfo.Text = dt.Rows[0]["LoanAppId"].ToString();
                        txtGrpComInfo.Text = dt.Rows[0]["GrpComInfo"].ToString();
                        hdGrpComInfoId.Value = dt.Rows[0]["GrpCompInfoID"].ToString();
                        btnSaveGrpComInfo.Enabled = false;
                        btnUpdateGrpComInfo.Enabled = true;
                        btnDeleteGrpComInfo.Enabled = true;
                        btnBackGrpComInfo.Enabled = true;
                    }
                    else
                    {
                        txtGrpComInfo.Text = "";
                        btnSaveGrpComInfo.Enabled = true;
                        btnUpdateGrpComInfo.Enabled = false;
                        btnDeleteGrpComInfo.Enabled = false;
                        btnBackGrpComInfo.Enabled = true;
                    }
                    mView.ActiveViewIndex = 13;
                    ViewAcess();
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
                dtFSPass = null;
            }
        }
        protected void lbCheckListDtl_Click(object sender, EventArgs e)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            DataTable dtFSPass = new DataTable();
            try
            {
                if (ViewState["LoanAppId"] == null)
                {
                    gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to update Check List Details..");
                    return;
                }
                else
                {
                    txtChkListLnAppNo.Text = ViewState["LoanAppId"].ToString();
                    //  GetCheckListHeadList();
                    dt = OMem.GetCheckDtlByLoanAppId(ViewState["LoanAppId"].ToString());
                    dtFSPass = OMem.GetFSPassByLoanId(ViewState["LoanAppId"].ToString());
                    if (dtFSPass.Rows.Count > 0)
                    {
                        if (dtFSPass.Rows[0]["FSPassYN"].ToString() == "N")
                        {
                            gblFuction.MsgPopup("You Can Not Update Check List Details as Financial Statement remains in Rejected State ...");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("Please tick Financial Statement Pass/Reject Checkbox to update Check List Details... ");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        gvCheckList.DataSource = dt;
                        gvCheckList.DataBind();
                        hfCheckListId.Value = dt.Rows[0]["CheckListID"].ToString();
                    }
                    mView.ActiveViewIndex = 12;
                    ViewAcess();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
                dtFSPass = null;
            }

        }

        protected void ShowUploadedDocuments(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadDocByLoanId(pLnAppId);
                if (dt.Rows.Count > 0)
                {
                    gvUpDetails.DataSource = dt;
                    gvUpDetails.DataBind();
                }
                else
                {
                    gvUpDetails.DataSource = null;
                    gvUpDetails.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedBSDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadBSDocByLoanId(pLnAppId, "BS");
                if (dt.Rows.Count > 0)
                {
                    gvCustBankDoc.DataSource = dt;
                    gvCustBankDoc.DataBind();
                }
                else
                {
                    gvCustBankDoc.DataSource = null;
                    gvCustBankDoc.DataBind();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedCBDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetUploadCBDocByLoanId(pLnAppId, "CB");
                if (dt.Rows.Count > 0)
                {
                    gvCustCBDoc.DataSource = dt;
                    gvCustCBDoc.DataBind();
                }
                else
                {
                    gvCustCBDoc.DataSource = null;
                    gvCustCBDoc.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void ShowUploadedFIDoc(string pLnAppId)
        {
            CMember OMem = new CMember();
            DataTable dt = new DataTable();
            try
            {
                dt = OMem.GetFIDocByLnAppId(pLnAppId);
                if (dt.Rows.Count > 0)
                {
                    gvFIDocs.DataSource = dt;
                    gvFIDocs.DataBind();
                }
                else
                {
                    gvFIDocs.DataSource = null;
                    gvFIDocs.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OMem = null;
                dt = null;
            }
        }
        protected void gvCheckList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCommandId = (e.Row.FindControl("ddlCommandId") as DropDownList);
                Label lblCommentId = (e.Row.FindControl("lblCommandID") as Label);
                try
                {
                    dt = oMem.GetCheckListComment();
                    if (dt.Rows.Count > 0)
                    {
                        ddlCommandId.DataSource = dt;
                        ddlCommandId.DataTextField = "CommentName";
                        ddlCommandId.DataValueField = "CommentID";
                        ddlCommandId.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCommandId.Items.Insert(0, oli1);
                    }
                    string Comment = lblCommentId.Text;
                    if (Comment != "-1")
                        ddlCommandId.SelectedValue = Comment;
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
        }
        protected void btnComUpload_Click(object sender, EventArgs e)
        {

            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Company Documents..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload1.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select Company CB File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "1.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload1.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Company CB File Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in Company CB File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }
            }

        }
        protected void btnCustUpload_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Customer Documents..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload2.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select Customer CB File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "2.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload2.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Customer CB File Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in Customer CB File Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnCoAppUpload1_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Co Apllicant Document1..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload3.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Co-Applicant CB File (i) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "3.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload3.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Co-Applicant CB File (i) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Co-Applicant CB File (i) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnCoAppUpload2_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Co Apllicant Document2..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload4.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Co-Applicant CB File (ii) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "4.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload4.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Co-Applicant CB File (ii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Co-Applicant CB File (ii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload1_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document1..");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload5.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (i) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "5.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload5.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (i) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (i) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload2_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document2...");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload6.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (ii) File to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "6.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload6.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (ii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (ii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload3_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document3....");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload7.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (iii)  to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "7.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload7.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (iii) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (iii) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void btnGuranUpload4_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Guarantor Document4....");
            }
            else
            {
                Int32 vErr = 0, vDel = 0;
                string vLnAppId = "", vFileName = "", vFilePath = "";
                vLnAppId = ViewState["LoanAppId"].ToString();
                CMember OMem = new CMember();
                try
                {
                    if (!fileUpload8.HasFile)
                    {
                        gblFuction.AjxMsgPopup("Please Select  Guarantor CB File (iv) to upload");
                    }
                    else
                    {
                        //vFileName = Path.GetFileName(fileUpload1.PostedFile.FileName);
                        vFileName = vLnAppId + "-" + "8.pdf";
                        vFilePath = "../../../CBDocuments/" + vFileName;
                        string path = Server.MapPath("../../../CBDocuments/" + vFileName);
                        FileInfo file = new FileInfo(path);
                        if (file.Exists)
                        {
                            file.Delete();
                            vDel = OMem.DeleteCBDocuments(vLnAppId, vFileName, vFilePath, 0);
                        }

                        fileUpload8.SaveAs(Server.MapPath("../../../CBDocuments/" + vFileName));
                        vErr = OMem.SaveCBDocuments(vLnAppId, vFileName, vFilePath, Convert.ToInt32(hdUserID.Value), 0);
                        if (vErr > 0)
                        {
                            gblFuction.AjxMsgPopup("Guarantor CB File (iv) Uploaded Successfully");
                            ShowUploadedDocuments(vLnAppId);
                        }
                        else
                        {
                            gblFuction.AjxMsgPopup("Error in  Guarantor CB File (iv) Upload");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    OMem = null;
                }

            }
        }
        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbtn = sender as LinkButton;
                GridViewRow gvrow = lnkbtn.NamingContainer as GridViewRow;
                string filePath = gvUpDetails.DataKeys[gvrow.RowIndex].Value.ToString();
                Response.ContentType = "pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filePath + "\"");
                Response.TransmitFile(Server.MapPath(filePath));
                Response.End();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        double LnAmt = 0;
        double POS = 0;
        double EMI = 0;
        protected void gvCB_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblLnAmt = (Label)e.Row.FindControl("lblLnAmt");
                string active = e.Row.Cells[4].Text.ToString();
                if (lblLnAmt.Text != "" && active == "Active")
                    LnAmt += Double.Parse(lblLnAmt.Text);
                Label lblPOSAmt = (Label)e.Row.FindControl("lblPOSAmt");
                if (lblPOSAmt.Text != "" && active == "Active")
                    POS += Double.Parse(lblPOSAmt.Text);
                Label lblEMI = (Label)e.Row.FindControl("lblEMI");
                if (lblEMI.Text != "" && active == "Active")
                    EMI += Double.Parse(lblEMI.Text);
                //Label lblPMonth7 = (Label)e.Row.FindControl("lblPMonth7");

                //if (lblPMonth7.Text == "")
                //{
                //  //  gvCB.Columns[18].Visible = false;
                //    //gvCB.HeaderRow.Cells[18].Visible = false;
                //    //e.Row.Cells[18].Visible = false;
                //}
                //else
                //{
                //  //  gvCB.Columns[18].Visible =true;
                //    //gvCB.HeaderRow.Cells[18].Visible = true;
                //    //e.Row.Cells[18].Visible = true;
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotLnAmt = (Label)e.Row.FindControl("lblTotLnAmt");
                lblTotLnAmt.Text = LnAmt.ToString();

                Label lblTotPOSAmt = (Label)e.Row.FindControl("lblTotPOSAmt");
                lblTotPOSAmt.Text = POS.ToString();

                Label lblTotEMI = (Label)e.Row.FindControl("lblTotEMI");
                lblTotEMI.Text = EMI.ToString();
            }
        }

        //protected void btnCalTotLia_Click(object sender, EventArgs e)
        //{
        //    int a = gvLiability.Rows.Count;
        //    double liabilty = 0;
        //    for (int x = 0; x < a; x++)
        //    {
        //        TextBox txt = (TextBox)gvLiability.Rows[x].FindControl("txtAmt");
        //        Label id = (Label)gvLiability.Rows[x].FindControl("lblParticularsId");
        //        int p = Convert.ToInt32(id.Text);

        //        if (p == 4 || p == 5)
        //        {
        //            if (txt.Text != "")
        //                liabilty = liabilty - Convert.ToDouble(txt.Text);
        //        }
        //        else
        //        {
        //            if (txt.Text != "")
        //                liabilty = liabilty + Convert.ToDouble(txt.Text);
        //        }
        //    }
        //    txtTotLiability.Text = liabilty.ToString("0.00");
        //}

        //protected void btnTotAsset_Click(object sender, EventArgs e)
        //{
        //    int a = gvAsset.Rows.Count;
        //    double asset = 0;
        //    for (int x = 0; x < a; x++)
        //    {
        //        TextBox txt = (TextBox)gvAsset.Rows[x].FindControl("txtAssetAmt");
        //        Label id = (Label)gvAsset.Rows[x].FindControl("lblParticularsId");
        //        int p = Convert.ToInt32(id.Text);

        //        if (txt.Text == "")
        //            asset = asset + 0;
        //        else
        //            asset = asset + Convert.ToDouble(txt.Text);

        //    }
        //    txtTotAsset.Text = asset.ToString("0.00");
        //}
        //protected void btnTotIncome_Click(object sender, EventArgs e)
        //{
        //    // modify total income section
        //    int a = gvIncome.Rows.Count;

        //    double income = 0;

        //    for (int x = 0; x < a; x++)
        //    {
        //        TextBox txt = (TextBox)gvIncome.Rows[x].FindControl("txtIncome");
        //        Label id = (Label)gvIncome.Rows[x].FindControl("lblParticularsIdPL");
        //        int p = Convert.ToInt32(id.Text);

        //        if (txt.Text == "")
        //            income = income + 0;
        //        else
        //            income = income + Convert.ToDouble(txt.Text);
        //    }
        //    txtTotIncome.Text = income.ToString("0.00");
        //    double expense = 0;
        //    if (txtTotExpense.Text != "")
        //        expense = Convert.ToDouble(txtTotExpense.Text);
        //    double netprofit = 0;
        //    netprofit = (income - expense);
        //    txtNetProfit.Text = netprofit.ToString("0.00");
        //}
        //protected void btnCalTotExp_Click(object sender, EventArgs e)
        //{
        //    int a = gvExpense.Rows.Count;
        //    double expense = 0;
        //    for (int x = 0; x < a; x++)
        //    {
        //        TextBox txt = (TextBox)gvExpense.Rows[x].FindControl("txtExpense");
        //        Label id = (Label)gvExpense.Rows[x].FindControl("lblParticularsIdPL");
        //        int p = Convert.ToInt32(id.Text);
        //        if (txt.Text == "")
        //            expense = expense + 0;
        //        else
        //            expense = expense + Convert.ToDouble(txt.Text);
        //    }
        //    double income = 0;
        //    income = Convert.ToDouble(txtTotIncome.Text);
        //    txtTotExpense.Text = expense.ToString("0.00");
        //    double netprofit = 0;
        //    netprofit = (income - expense);
        //    txtNetProfit.Text = netprofit.ToString("0.00");
        //}
        private void AssementMatrixEnable(Int32 st)
        {

            // For only Machinery Loan Machinary Type and Machinary Supplier selection will be enabled... Otherwise (for BL,STL..) user can not able to select
            if (st == 2)
            {
                ddlAMAns17.Enabled = true;
                ddlAMAns18.Enabled = true;
            }
            else
            {
                ddlAMAns17.Enabled = false;
                ddlAMAns18.Enabled = false;
            }

        }
        protected void gvCustDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string vAttachAppDocUpID = "";
            Int32 vRow = 0;
            DataTable dt = null;
            CDocUpLoad oAD = null;
            try
            {
                vAttachAppDocUpID = Convert.ToString(e.CommandArgument);
                ViewState["DocUpID"] = vAttachAppDocUpID;
                if (e.CommandName == "cmdDownld")
                {
                    string vFileName = "";
                    string vFileTyp = "";
                    GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                    oAD = new CDocUpLoad();
                    dt = oAD.GetDocUpAttachById(vAttachAppDocUpID);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["AttachDoc"].ToString() != "")
                        {
                            byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                            vFileName = dt.Rows[0]["AttachDocName"].ToString();
                            vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = vFileTyp;
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                            BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                            bw.Write(fileData);
                            bw.Close();
                            Response.End();
                        }
                        else
                        {
                            gblFuction.MsgPopup("No Attachment Found");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
            }
            finally
            {
                //dt.Dispose();
            }
        }
        protected void gvBSPLDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 vAttachAppDocUpID = 0;
            DataTable dt = null;
            CDocUpLoad oAD = null;
            try
            {
                vAttachAppDocUpID = Convert.ToInt32(e.CommandArgument);
                ViewState["DocUpID"] = vAttachAppDocUpID;
                if (e.CommandName == "cmdDownld")
                {
                    string vFileName = "";
                    string vFileTyp = "";
                    GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                    oAD = new CDocUpLoad();
                    dt = oAD.GetBSPLDocUpAttachById(vAttachAppDocUpID);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["AttachDoc"].ToString() != "")
                        {
                            byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                            vFileName = dt.Rows[0]["AttachDocName"].ToString();
                            vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                            Response.Clear();
                            Response.Buffer = true;
                            Response.ContentType = vFileTyp;
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                            BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                            bw.Write(fileData);
                            bw.Close();
                            Response.End();
                        }
                        else
                        {
                            gblFuction.MsgPopup("No Attachment Found");
                            return;
                        }
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
            }
            finally
            {
                //dt.Dispose();
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
        protected void GetBrCodeByLnAppId(string pLnAppId)
        {
            DataTable dt = new DataTable();
            CMember OMem = new CMember();
            try
            {
                dt = OMem.GetBrCodeByLnAppId(pLnAppId);
                if (dt.Rows.Count > 0)
                {
                    hdLABrCode.Value = dt.Rows[0]["BranchCode"].ToString();
                    hdLABrName.Value = dt.Rows[0]["BranchName"].ToString();
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
        protected void gvCBDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlCBname = (e.Row.FindControl("ddlCBname") as DropDownList);
                Label lblCBNameId = (e.Row.FindControl("lblCBNameId") as Label);
                Label lblIsActive = (e.Row.FindControl("lblIsActive") as Label);
                CheckBox chkCBIsActive = (e.Row.FindControl("chkCBIsActive") as CheckBox);
                Button btnAddCBRow = (e.Row.FindControl("btnAddCBRow") as Button);
                ImageButton ImDelCB = (e.Row.FindControl("ImDelCB") as ImageButton);
                try
                {
                    dt = oMem.GetCBName();
                    if (dt.Rows.Count > 0)
                    {
                        ddlCBname.DataSource = dt;
                        ddlCBname.DataTextField = "CBName";
                        ddlCBname.DataValueField = "CBNameId";
                        ddlCBname.DataBind();
                        ListItem oli1 = new ListItem("<--Select-->", "-1");
                        ddlCBname.Items.Insert(0, oli1);
                    }
                    string Comment = lblCBNameId.Text;
                    if (Comment != " ")
                        ddlCBname.SelectedValue = Comment;
                    if (lblIsActive.Text == "N" || lblIsActive.Text == "")
                    {
                        chkCBIsActive.Checked = false;
                    }
                    else
                    {
                        chkCBIsActive.Checked = true;
                    }

                    // change visibility of Add and Delete record with in gridview
                    if (hdfCBId.Value != "")
                    {
                        btnAddCBRow.Visible = false;
                        ImDelCB.Visible = false;
                    }
                    else
                    {
                        btnAddCBRow.Visible = true;
                        ImDelCB.Visible = true;
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
        }
        protected void gvBankAcSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    DataTable dt = (DataTable)Session["AccountsDt"];
                    if (dt.Rows.Count > 0)
                    {
                        int y = dt.Columns.Count;
                        int i = 11;
                        //Tot O/W Return No
                        e.Row.Cells[y - 1].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 1)).Sum().ToString();
                        //Tot EMI Return No
                        e.Row.Cells[y - 2].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 2)).Sum().ToString();
                        //Tot IW Return No
                        e.Row.Cells[y - 3].Text = dt.AsEnumerable().Select(x => x.Field<int>(y - 3)).Sum().ToString();
                        //Tot Sales Fig
                        e.Row.Cells[y - 4].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 4)).Sum().ToString();
                        //Tot Credit Sum
                        e.Row.Cells[y - 5].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 5)).Sum().ToString();
                        //Tot O/W Amt
                        e.Row.Cells[y - 6].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 6)).Sum().ToString();
                        //Tot EMI Amt
                        e.Row.Cells[y - 7].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 7)).Sum().ToString();
                        //Tot I/W Amt
                        e.Row.Cells[y - 8].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 8)).Sum().ToString();
                        //Tot Loan Availed
                        e.Row.Cells[y - 9].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 9)).Sum().ToString();
                        //Tot Transfer
                        e.Row.Cells[y - 10].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - 10)).Sum().ToString();
                        for (i = 11; i < y; i++)
                        {
                            // Accountwise Tot Credit
                            e.Row.Cells[y - i].Text = dt.AsEnumerable().Select(x => x.Field<double>(y - i)).Sum().ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void gvITR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblITRFiledYN = (e.Row.FindControl("lblITRFiledYN") as Label);
                CheckBox chkITRFile = (e.Row.FindControl("chkITRFile") as CheckBox);
                Button btnAddITR = (e.Row.FindControl("btnAddITR") as Button);
                ImageButton ImDelITR = (e.Row.FindControl("ImDelITR") as ImageButton);
                try
                {
                    if (lblITRFiledYN.Text == "N" || lblITRFiledYN.Text == "")
                    {
                        chkITRFile.Checked = false;
                    }
                    else
                    {
                        chkITRFile.Checked = true;
                    }

                    // change visibility of Add and Delete record with in gridview
                    //if (hdfCBId.Value != "")
                    //{
                    //    btnAddITR.Visible = false;
                    //    ImDelITR.Visible = false;
                    //}
                    //else
                    //{
                    //    btnAddITR.Visible = true;
                    //    ImDelITR.Visible = true;
                    //}
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
        }
        protected void gvViewITR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblVITRFiledYN = (e.Row.FindControl("lblVITRFiledYN") as Label);
                CheckBox chkVITRFile = (e.Row.FindControl("chkVITRFile") as CheckBox);
                try
                {
                    if (lblVITRFiledYN.Text == "N" || lblVITRFiledYN.Text == "")
                    {
                        chkVITRFile.Checked = false;
                        chkVITRFile.Enabled = false;
                    }
                    else
                    {
                        chkVITRFile.Checked = true;
                        chkVITRFile.Enabled = false;
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
        }
        protected void gvPLList_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            decimal FYear = 0; decimal SYear = 0; decimal TYear = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableHeaderCell NewCell = new TableHeaderCell();
                NewCell.Text = "Change(%)";
                e.Row.Cells.AddAt(5, NewCell);

                TableHeaderCell NewCell1 = new TableHeaderCell();
                NewCell1.Text = "Change(%)";
                e.Row.Cells.AddAt(7, NewCell1);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int SlNo = int.Parse(e.Row.Cells[1].Text);
                if (SlNo == 5 || SlNo == 7 || SlNo == 11 || SlNo == 12 || SlNo == 17 || SlNo == 18 || SlNo == 20 || SlNo == 23 || SlNo == 25 || SlNo == 26)
                {
                    e.Row.BackColor = System.Drawing.Color.SkyBlue;
                    e.Row.Font.Bold = true;
                }

                if (e.Row.Cells[3].Text != "&nbsp;")
                    FYear = Convert.ToDecimal(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;")
                    SYear = Convert.ToDecimal(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;")
                    TYear = Convert.ToDecimal(e.Row.Cells[5].Text);

                TableCell NewCell = new TableCell();
                NewCell.ID = "NewCell";
                if (FYear != 0)
                    NewCell.Text = Math.Round((((SYear - FYear) / FYear) * 100), 2).ToString();
                else
                    NewCell.Text = "0";
                e.Row.Cells.AddAt(5, NewCell);

                TableCell NewCell1 = new TableCell();
                NewCell1.ID = "NewCell1";
                if (SYear != 0)
                    NewCell1.Text = Math.Round((((TYear - SYear) / SYear) * 100), 2).ToString();
                else
                    NewCell1.Text = "0";
                e.Row.Cells.AddAt(7, NewCell1);
            }
        }
        protected void gvBSList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal FYear = 0; decimal SYear = 0; decimal TYear = 0;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                TableHeaderCell NewCell = new TableHeaderCell();
                NewCell.Text = "Change(%)";
                e.Row.Cells.AddAt(5, NewCell);

                TableHeaderCell NewCell1 = new TableHeaderCell();
                NewCell1.Text = "Change(%)";
                e.Row.Cells.AddAt(7, NewCell1);

                //e.Row.Cells[0].Width = new Unit("1%");
                //e.Row.Cells[1].Width = new Unit("1%");
                //e.Row.Cells[2].Width = new Unit("5%");
                //e.Row.Cells[3].Width = new Unit("1%");
                //e.Row.Cells[4].Width = new Unit("1%");
                //e.Row.Cells[5].Width = new Unit("1%");
                //e.Row.Cells[6].Width = new Unit("1%");
                //e.Row.Cells[7].Width = new Unit("1%");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int SlNo = int.Parse(e.Row.Cells[1].Text);
                if (SlNo == 4 || SlNo == 8 || SlNo == 11 || SlNo == 14 || SlNo == 15 || SlNo == 19 || SlNo == 24 || SlNo == 25 || SlNo == 30 || SlNo == 34 || SlNo == 39 || SlNo == 44 || SlNo == 46)
                {
                    e.Row.BackColor = System.Drawing.Color.SkyBlue;
                    e.Row.Font.Bold = true;
                }


                if (e.Row.Cells[3].Text != "&nbsp;")
                    FYear = Convert.ToDecimal(e.Row.Cells[3].Text);
                if (e.Row.Cells[4].Text != "&nbsp;")
                    SYear = Convert.ToDecimal(e.Row.Cells[4].Text);
                if (e.Row.Cells[5].Text != "&nbsp;")
                    TYear = Convert.ToDecimal(e.Row.Cells[5].Text);

                TableCell NewCell = new TableCell();
                NewCell.ID = "NewCell";
                if (FYear != 0)
                    NewCell.Text = Math.Round((((SYear - FYear) / FYear) * 100), 2).ToString();
                else
                    NewCell.Text = "0";
                e.Row.Cells.AddAt(5, NewCell);

                TableCell NewCell1 = new TableCell();
                NewCell1.ID = "NewCell1";
                if (SYear != 0)
                    NewCell1.Text = Math.Round((((TYear - SYear) / SYear) * 100), 2).ToString();
                else
                    NewCell1.Text = "0";
                e.Row.Cells.AddAt(7, NewCell1);

                //e.Row.Cells[0].Width = new Unit("1%");
                //e.Row.Cells[1].Width = new Unit("1%");
                //e.Row.Cells[2].Width = new Unit("5%");
                //e.Row.Cells[3].Width = new Unit("1%");
                //e.Row.Cells[4].Width = new Unit("1%");
                //e.Row.Cells[5].Width = new Unit("1%");
                //e.Row.Cells[6].Width = new Unit("1%");
                //e.Row.Cells[7].Width = new Unit("1%");
            }
        }
        protected void gvRatio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.BackColor = System.Drawing.Color.SkyBlue;
                e.Row.Font.Bold = true;
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
        protected void btnUploadBankStat_Click(object sender, EventArgs e)
        {
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Bank Statement...");
                return;
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchBankDocType = "", vDocDesc = "BS";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";
            CMember oMem = new CMember();
            int pDocID = 0;
            int vError = 0;
            if (txtBSupDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Input Bank Statement Upload Date....");
                return;
            }
            if (fuBankStat.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuBankStat.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuBankStat.HasFile)
            {
                vAttachOffDoc = new byte[fuBankStat.PostedFile.InputStream.Length + 1];
                fuBankStat.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchBankDocType = System.IO.Path.GetExtension(fuBankStat.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuBankStat.FileName);
            }
            DateTime pUploadDate = gblFuction.setDate(txtBSupDate.Text.Trim());
            vError = oMem.SaveCustDoc(pDocID, pLoanAppId, vDocDesc, pUploadDate, vAttachOffDoc, vAttchBankDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("Bank Statement Uploaded Successfully......");
                ShowUploadedBSDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void gvCustBankDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetCustBankDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void btnUploadCBDoc_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload Credit Bureau...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload CB Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchBankDocType = "", vDocDesc = "CB";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";

            int pDocID = 0;
            int vError = 0;
            if (txtCBupDate.Text == "")
            {
                gblFuction.AjxMsgPopup("Please Input Credit Bureau Upload Date....");
                return;
            }
            if (fuCustCBDoc.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuCustCBDoc.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuCustCBDoc.HasFile)
            {
                vAttachOffDoc = new byte[fuCustCBDoc.PostedFile.InputStream.Length + 1];
                fuCustCBDoc.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchBankDocType = System.IO.Path.GetExtension(fuCustCBDoc.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuCustCBDoc.FileName);
            }
            DateTime pUploadDate = gblFuction.setDate(txtCBupDate.Text.Trim());
            vError = oMem.SaveCustDoc(pDocID, pLoanAppId, vDocDesc, pUploadDate, vAttachOffDoc, vAttchBankDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("Credit Bureau Document Uploaded Successfully......");
                ShowUploadedCBDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void gvCustCBDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetCustBankDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void gvFIDocs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdDownld")
            {
                int vDocId = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dt = null;
                CDocUpLoad oAD = null;
                string vFileTyp = "";
                string vFileName = "";
                GridViewRow gvR = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                ImageButton imgDownload = (ImageButton)gvR.FindControl("imgDownload");
                oAD = new CDocUpLoad();
                dt = oAD.GetFIDoc(Convert.ToInt32(vDocId));
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["AttachDoc"].ToString() != "")
                    {
                        byte[] fileData = (byte[])dt.Rows[0]["AttachDoc"];
                        vFileTyp = GetFileType(Convert.ToString(dt.Rows[0]["AttachType"]));
                        vFileName = dt.Rows[0]["AttachDocName"].ToString();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = vFileTyp;
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + vFileName + "" + Convert.ToString(dt.Rows[0]["AttachType"]));
                        BinaryWriter bw = new BinaryWriter(Response.OutputStream);
                        bw.Write(fileData);
                        bw.Close();
                        Response.End();
                    }
                    else
                    {
                        gblFuction.MsgPopup("No Attachment Found");
                        return;
                    }
                }
                else
                {
                    gblFuction.MsgPopup("No Attachment Found");
                    return;
                }
            }

        }
        protected void gvBSList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //
            }
        }
        protected void btnFIOff_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchOffDocType = "", vDocDesc = "Office Address";
            byte[] vAttachOffDoc = null;
            string vAttchOffDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIOffice.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIOffice.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIOffice.HasFile)
            {
                vAttachOffDoc = new byte[fuFIOffice.PostedFile.InputStream.Length + 1];
                fuFIOffice.PostedFile.InputStream.Read(vAttachOffDoc, 0, vAttachOffDoc.Length);
                vAttchOffDocType = System.IO.Path.GetExtension(fuFIOffice.FileName).ToLower();
                vAttchOffDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIOffice.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachOffDoc, vAttchOffDocType, vAttchOffDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void btnFIRes_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchResDocType = "", vDocDesc = "Residence Address";
            byte[] vAttachResDoc = null;
            string vAttchResDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIResidence.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIResidence.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIResidence.HasFile)
            {
                vAttachResDoc = new byte[fuFIResidence.PostedFile.InputStream.Length + 1];
                fuFIResidence.PostedFile.InputStream.Read(vAttachResDoc, 0, vAttachResDoc.Length);
                vAttchResDocType = System.IO.Path.GetExtension(fuFIResidence.FileName).ToLower();
                vAttchResDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIResidence.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachResDoc, vAttchResDocType, vAttchResDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
            }
        }
        protected void btnFIGua_Click(object sender, EventArgs e)
        {
            CMember oMem = new CMember();
            DataTable dt = new DataTable();
            if (ViewState["LoanAppId"] == null)
            {
                gblFuction.MsgPopup("Please Click on Show Information of Loan Application Tab to Upload FI Documents...");
                return;
            }
            dt = oMem.GetLnAppPassYNByLoanid(ViewState["LoanAppId"].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["LnAppPassYN"].ToString() == "N")
                {
                    gblFuction.MsgPopup("You Can Not Upload FI Documents unless it passed from Loan Application Section...");
                    return;
                }
            }
            else
            {
                gblFuction.MsgPopup("Please Tick Pass/Reject in Loan Application...");
            }
            string pLoanAppId = ViewState["LoanAppId"].ToString();
            string vAttchGuaDocType = "", vDocDesc = "Guarantor";
            byte[] vAttachGuaDoc = null;
            string vAttchGuaDocName = "";

            int pDocID = 0;
            int vError = 0;

            if (fuFIGuarantor.HasFile == false)
            {
                gblFuction.MsgPopup("Please Choose a file to attach....");
                return;
            }

            if (fuFIGuarantor.PostedFile.InputStream.Length > 4194304)
            {
                gblFuction.MsgPopup("Maximum upload file Size Is 4(MB).");
                return;
            }
            if (fuFIGuarantor.HasFile)
            {
                vAttachGuaDoc = new byte[fuFIGuarantor.PostedFile.InputStream.Length + 1];
                fuFIGuarantor.PostedFile.InputStream.Read(vAttachGuaDoc, 0, vAttachGuaDoc.Length);
                vAttchGuaDocType = System.IO.Path.GetExtension(fuFIGuarantor.FileName).ToLower();
                vAttchGuaDocName = System.IO.Path.GetFileNameWithoutExtension(fuFIGuarantor.FileName);
            }
            vError = oMem.SaveFIDoc(pDocID, pLoanAppId, vDocDesc, vAttachGuaDoc, vAttchGuaDocType, vAttchGuaDocName, Convert.ToInt32(hdUserID.Value), "Save", 0);
            if (vError == 0)
            {
                gblFuction.AjxMsgPopup("FI Document Uploaded Successfully......");
                ShowUploadedFIDoc(pLoanAppId);
                return;
            }
            else
            {
                gblFuction.MsgPopup(gblPRATAM.DBError);
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
        protected void gvApp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnVerifyCust = (Button)e.Row.FindControl("btnVerifyCust");
                ImageButton btnDownloadCustEnq = (ImageButton)e.Row.FindControl("btnDownloadCustEnq");
                Label lblIsHighmarkEnqDone = (Label)e.Row.FindControl("lblIsHighmarkEnqDone");
                if (lblIsHighmarkEnqDone.Text == "N")
                {
                    btnVerifyCust.Enabled = true;
                    btnVerifyCust.Text = "Highmark Verification";
                    btnDownloadCustEnq.Enabled = false;
                }
                else
                {
                    btnVerifyCust.Enabled = false;
                    btnVerifyCust.Text = "Highmark Verification Done";
                    btnDownloadCustEnq.Enabled = true;
                }
            }
        }
        protected void gvCoAppDtl_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnVerify = (Button)e.Row.FindControl("btnVerify");
                ImageButton btnDownloadCoAppEnq = (ImageButton)e.Row.FindControl("btnDownloadCoAppEnq");
                Label lblIsHighmarkEnqDoneCA = (Label)e.Row.FindControl("lblIsHighmarkEnqDoneCA");
                if (lblIsHighmarkEnqDoneCA.Text == "N")
                {
                    btnVerify.Enabled = true;
                    btnVerify.Text = "Highmark Verification";
                    btnDownloadCoAppEnq.Enabled = false;
                }
                else
                {
                    btnVerify.Enabled = false;
                    btnVerify.Text = "Highmark Verification Done";
                    btnDownloadCoAppEnq.Enabled = true;
                }
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
        protected void btnVerifyCust_Click(object sender, EventArgs e)
        {
            string enq_id = "";
            string new_enquiry_id = "";
            string coa_enq_id = "";
            string coa_new_enquiry_id = "";
            int equi_data_save_status = -1;
            int highmark_data_save_status = -1;
            int[] req_response_flag = { 0, 0, 0, 0 };

            Button btnEqVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
            Label lblHighMarkId = (Label)gR.FindControl("lblCustIDId");
            Label lblInqUniqueRefNo = (Label)gR.FindControl("lblInqUniqueRefNo");
            Button btnVerifyCust = (Button)gR.FindControl("btnVerifyCust");
            ImageButton btnDownloadCustEnq = (ImageButton)gR.FindControl("btnDownloadCustEnq");
            CGblIdGenerator oGbl = new CGblIdGenerator();
            CApplication oObj = new CApplication();
            string vBrchCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            string pMode = "A"; // A-Customer/Applicant,C-CoApplicant
            dt = oGbl.GetHighmarkEnqData(lblHighMarkId.Text, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), pMode);
            if (dt.Rows.Count > 0)
            {

                string vAppDate = Session[gblValue.LoginDate].ToString();

                if (dt.Rows[0]["Aadhaar"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Aadhaar"].ToString().Trim().Length != 12)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper Aadhaar Number of Applicant..!!");
                        return;
                    }
                }
                if (dt.Rows[0]["Pan"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Pan"].ToString().Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper pan Card Number of Applicant..!!");
                        return;
                    }
                }

                if (dt.Rows[0]["Voter"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Voter"].ToString().Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please Update Proper Voter Card Number of Applicant..!!");
                        return;
                    }
                }
                #region HIGHMARK PART

                string responsedata = string.Empty;
                string responsedata_coa = string.Empty;

                #region CREDENTIAL PART

                //UAT CREDENTIALS
                string userId = "pratam_cpu_uat@pratamfin.in";
                string password = "7111CD2E4B75AFC2A03C46363DFCD55D317B1FB1";
                string mbrid = "NBF0001463";
                string productType = "INDV";
                string productVersion = "1.0";
                string reqVolType = "INDV";
                string SUB_MBR_ID = "CENTRUMSME FIN SERV";






                //PRODUCTION CREDENTIALS
                //string userId = "cpu_prod@jarofinance.com";
                //string password = "2E5EBCBC0758EE9D45A4262BD0D9DF58F0C61ED6";
                //string mbrid = "NBF0001463";
                ////string productType = "INDV";
                ////string productVersion = "1.0";
                ////string reqVolType = "INDV";
                //string SUB_MBR_ID = "CENTRUMSME FIN SERV";
                #endregion

                string request_datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                #region APPLICANT PART

                #region DATA MAKE PART

                #region HEADER-SEGMENT PART
                string HEADER_SEGMENT = "<HEADER-SEGMENT>"
                                      + "<PRODUCT-TYP>BASE_PLUS_REPORT</PRODUCT-TYP>"
                                      + "<PRODUCT-VER>2.0</PRODUCT-VER>"
                                      + "<REQ-MBR>" + mbrid + "</REQ-MBR>"
                                      + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                      + "<INQ-DT-TM>" + request_datetime + "</INQ-DT-TM>"
                                      + "<REQ-VOL-TYP>INDV</REQ-VOL-TYP>"
                                      + "<REQ-ACTN-TYP>SUBMIT</REQ-ACTN-TYP>"
                                      + "<TEST-FLG>N</TEST-FLG>"
                                      + "<USER-ID>" + userId + "</USER-ID>"
                                      + "<PWD>" + password + "</PWD>"
                                      + "<AUTH-FLG>Y</AUTH-FLG>"
                                      + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                      + "<RES-FRMT>XML</RES-FRMT>"
                                      + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                                      + "<RES-FRMT-EMBD>N</RES-FRMT-EMBD>"
                                      + "</HEADER-SEGMENT>";
                #endregion

                #region INQUIRY PART
                string APPLICANT_SEGMENT = "<APPLICANT-SEGMENT>"
                                         + "<APPLICANT-NAME><NAME1>" + dt.Rows[0]["MemberName"].ToString().Trim() + "</NAME1>"
                                         + "<NAME2></NAME2><NAME3></NAME3><NAME4></NAME4><NAME5></NAME5></APPLICANT-NAME>"
                                         + "<DOB><DOB-DATE>" + dt.Rows[0]["DOB"] + "</DOB-DATE>"
                                         + "<AGE>" + dt.Rows[0]["Age"] + "</AGE>"
                                         + "<AGE-AS-ON>" + dt.Rows[0]["AsOnDate"] + "</AGE-AS-ON></DOB>"
                                         + "<IDS>"
                                         + "<ID><TYPE>ID03</TYPE><VALUE>" + dt.Rows[0]["Aadhaar"] + "</VALUE></ID>" //aadhaar no
                                         + ((dt.Rows[0]["Pan"] == "") ? "" : "<ID><TYPE>ID07</TYPE><VALUE>" + dt.Rows[0]["Pan"] + "</VALUE></ID>") //pan no (if available)
                                         + ((dt.Rows[0]["Voter"] == "") ? "" : "<ID><TYPE>ID02</TYPE><VALUE>" + dt.Rows[0]["Voter"] + "</VALUE></ID>") //voter id (if available)
                                         + ((dt.Rows[0]["Ration"] == "") ? "" : "<ID><TYPE>ID05</TYPE><VALUE>" + dt.Rows[0]["Ration"] + "</VALUE></ID>") //ration card (if available)
                                         + "</IDS>"
                                         + "<RELATIONS><RELATION><NAME>" + dt.Rows[0]["RelativeName"].ToString() + "</NAME><TYPE>" + setRelationIdForHighMark(dt.Rows[0]["RelationId"].ToString()) + "</TYPE>"
                                         + "</RELATION></RELATIONS><PHONES><PHONE><TELE-NO>" + dt.Rows[0]["ContactNo"].ToString() + "</TELE-NO>"
                                         + "<TELE-NO-TYPE>P03</TELE-NO-TYPE></PHONE></PHONES>"
                                         + "</APPLICANT-SEGMENT>";

                string highmark_address = "";
                string highmark_state = dt.Rows[0]["StateCode"].ToString();


                highmark_address = dt.Rows[0]["Address"].ToString();

                string ADDRESS_SEGMENT = "<ADDRESS-SEGMENT><ADDRESS><TYPE>D01</TYPE><ADDRESS-1>" + highmark_address + "</ADDRESS-1>"
                                       + "<CITY>" + dt.Rows[0]["DistrictName"].ToString() + "</CITY><STATE>" + highmark_state + "</STATE><PIN>" + dt.Rows[0]["PinCode"].ToString() + "</PIN></ADDRESS></ADDRESS-SEGMENT>";

                string APPLICATION_SEGMENT = "<APPLICATION-SEGMENT><INQUIRY-UNIQUE-REF-NO>" + DateTime.Now.ToString("ddMMyyyyHHmmss") + enq_id + "</INQUIRY-UNIQUE-REF-NO>"
                                           + "<CREDT-INQ-PURPS-TYP>ACCT-ORIG</CREDT-INQ-PURPS-TYP><CREDIT-INQUIRY-STAGE>PRE-SCREEN</CREDIT-INQUIRY-STAGE>"
                                           + "<CREDT-REQ-TYP>INDV</CREDT-REQ-TYP><BRANCH-ID>" + Session[gblValue.BrnchCode].ToString() + "</BRANCH-ID>"
                                           + "<LOS-APP-ID>" + lblHighMarkId.Text.ToString() + "</LOS-APP-ID>"
                                           + "<LOAN-AMOUNT>" + txtAppLnAmt.Text + "</LOAN-AMOUNT></APPLICATION-SEGMENT>";

                string INQUIRY = "<INQUIRY>" + APPLICANT_SEGMENT + ADDRESS_SEGMENT + APPLICATION_SEGMENT + "</INQUIRY>";
                #endregion

                #endregion

                #region requestXML PART
                string requestXML = "<REQUEST-REQUEST-FILE>" + HEADER_SEGMENT + INQUIRY + "</REQUEST-REQUEST-FILE>";
                #endregion

                #region REQUEST/RESPONSE PART

                /*UAT URL*/
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse");
                /*********/

                /*PRODUCTION URL*/
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/Inquiry/CPUAction.action");
                /****************/

                httpWebRequest.ContentType = "application/xml; charset=utf-8";
                httpWebRequest.Accept = "application/xml";
                httpWebRequest.Method = "POST";
                httpWebRequest.PreAuthenticate = true;
                //httpWebRequest.Timeout = 1000; //1 minute time out

                httpWebRequest.Headers.Add("inquiryXML", requestXML);
                httpWebRequest.Headers.Add("userId", userId);
                httpWebRequest.Headers.Add("password", password);

                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        var highmarkresult = streamReader.ReadToEnd();
                        responsedata = highmarkresult.ToString().Trim();
                    }
                }
                catch
                {
                    req_response_flag[2] = 1;
                }
                #endregion

                #endregion
                /*SAVE HIGHMARK RECORDS*/
                if (req_response_flag[2] == 1 || req_response_flag[3] == 1)
                {
                    highmark_data_save_status = 0;
                }
                else
                {
                    CMember oMem = new CMember();

                    highmark_data_save_status = oMem.SaveHighmarkData(lblHighMarkId.Text.ToString(), Session[gblValue.LoginDate].ToString(),
                        responsedata, requestXML, "A", Convert.ToInt32(hdUserID.Value));
                    if (highmark_data_save_status == 1)
                    {
                        btnVerifyCust.Enabled = false;
                        btnDownloadCustEnq.Enabled = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Highmark Data Verification Failed For Applicant...");
                        return;
                    }
                }
                /***********************/

                #endregion
            }
        }
        protected void btnVerifyCA_Click(object sender, EventArgs e)
        {
            string enq_id = "";
            string new_enquiry_id = "";
            string coa_enq_id = "";
            string coa_new_enquiry_id = "";
            int equi_data_save_status = -1;
            int highmark_data_save_status = -1;
            int[] req_response_flag = { 0, 0, 0, 0 };

            Button btnEqVerify = (Button)sender;
            GridViewRow gR = (GridViewRow)btnEqVerify.NamingContainer;
            Label lblHighMarkId = (Label)gR.FindControl("lblCoApplicantId");
            Label lblInqUniqueRefNo = (Label)gR.FindControl("lblInqUniqueRefNo");
            Button btnVerify = (Button)gR.FindControl("btnVerifyCA");
            ImageButton btnDownloadCoAppEnq = (ImageButton)gR.FindControl("btnDownloadCoAppEnq");
            CGblIdGenerator oGbl = new CGblIdGenerator();
            CApplication oObj = new CApplication();
            string vBrchCode = Session[gblValue.BrnchCode].ToString();
            DataTable dt = null;
            string pMode = "C"; // A-Customer/Applicant,C-CoApplicant
            dt = oGbl.GetHighmarkEnqData(lblHighMarkId.Text, gblFuction.setDate(Session[gblValue.LoginDate].ToString()), pMode);
            if (dt.Rows.Count > 0)
            {

                string vAppDate = Session[gblValue.LoginDate].ToString();

                if (dt.Rows[0]["Aadhaar"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Aadhaar"].ToString().Trim().Length != 12)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper Aadhaar Number of Co Applicant..!!");
                        return;
                    }
                }
                if (dt.Rows[0]["Pan"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Pan"].ToString().Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please update Proper pan Card Number of Co Applicant..!!");
                        return;
                    }
                }

                if (dt.Rows[0]["Voter"].ToString().Trim() != "")
                {
                    if (dt.Rows[0]["Voter"].ToString().Trim().Length < 10)
                    {
                        gblFuction.AjxMsgPopup("Please Update Proper Voter Card Number of Co Applicant..!!");
                        return;
                    }
                }
                #region HIGHMARK PART

                string responsedata = string.Empty;
                string responsedata_coa = string.Empty;

                #region CREDENTIAL PART

                //UAT CREDENTIALS
                string userId = "pratam_cpu_uat@pratamfin.in";
                string password = "7111CD2E4B75AFC2A03C46363DFCD55D317B1FB1";
                string mbrid = "NBF0001463";
                string productType = "INDV";
                string productVersion = "1.0";
                string reqVolType = "INDV";
                string SUB_MBR_ID = "CENTRUMSME FIN SERV";

                //PRODUCTION CREDENTIALS
                //string userId = "pratam_cpu_uat@pratamfin.in";
                //string password = "7111CD2E4B75AFC2A03C46363DFCD55D317B1FB1";
                //string mbrid = "NBF0001463";
                ////string productType = "INDV";
                ////string productVersion = "1.0";
                ////string reqVolType = "INDV";
                //string SUB_MBR_ID = "CENTRUMSME FIN SERV";
                #endregion

                string request_datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");


                #region CO-APPLICANT PART
                string requestXML_COA = "";

                #region HEADER-SEGMENT PART

                string HEADER_SEGMENT_COA = "<HEADER-SEGMENT>"
                                          + "<PRODUCT-TYP>BASE_PLUS_REPORT</PRODUCT-TYP>"
                                          + "<PRODUCT-VER>2.0</PRODUCT-VER>"
                                          + "<REQ-MBR>" + mbrid + "</REQ-MBR>"
                                          + "<SUB-MBR-ID>" + SUB_MBR_ID + "</SUB-MBR-ID>"
                                          + "<INQ-DT-TM>" + request_datetime + "</INQ-DT-TM>"
                                          + "<REQ-VOL-TYP>INDV</REQ-VOL-TYP>"
                                          + "<REQ-ACTN-TYP>SUBMIT</REQ-ACTN-TYP>"
                                          + "<TEST-FLG>N</TEST-FLG>"
                                          + "<USER-ID>" + userId + "</USER-ID>"
                                          + "<PWD>" + password + "</PWD>"
                                          + "<AUTH-FLG>Y</AUTH-FLG>"
                                          + "<AUTH-TITLE>USER</AUTH-TITLE>"
                                          + "<RES-FRMT>XML</RES-FRMT>"
                                          + "<MEMBER-PRE-OVERRIDE>N</MEMBER-PRE-OVERRIDE>"
                                          + "<RES-FRMT-EMBD>N</RES-FRMT-EMBD>"
                                          + "</HEADER-SEGMENT>";

                #endregion

                #region INQUIRY PART

                string APPLICANT_SEGMENT_COA = "<APPLICANT-SEGMENT>"
                                             + "<APPLICANT-NAME><NAME1>" + dt.Rows[0]["MemberName"].ToString().Trim() + "</NAME1>"
                                             + "<NAME2></NAME2><NAME3></NAME3><NAME4></NAME4><NAME5></NAME5></APPLICANT-NAME>"
                                             + "<DOB><DOB-DATE>" + dt.Rows[0]["DOB"].ToString() + "</DOB-DATE>"
                                             + "<AGE>" + dt.Rows[0]["Age"].ToString() + "</AGE>"
                                             + "<AGE-AS-ON>" + dt.Rows[0]["AsOnDate"].ToString() + "</AGE-AS-ON></DOB>"
                                             + "<IDS>"
                                             + "<ID><TYPE>ID03</TYPE><VALUE>" + dt.Rows[0]["Aadhaar"].ToString() + "</VALUE></ID>" //aadhaar no
                                             + ((dt.Rows[0]["Pan"].ToString() == "") ? "" : "<ID><TYPE>ID07</TYPE><VALUE>" + dt.Rows[0]["Pan"].ToString() + "</VALUE></ID>") //pan no (if available)
                                             + ((dt.Rows[0]["Voter"].ToString() == "") ? "" : "<ID><TYPE>ID02</TYPE><VALUE>" + dt.Rows[0]["Voter"].ToString() + "</VALUE></ID>") //voter id (if available)
                                             + ((dt.Rows[0]["Ration"].ToString() == "") ? "" : "<ID><TYPE>ID05</TYPE><VALUE>" + dt.Rows[0]["Ration"].ToString() + "</VALUE></ID>") //ration card (if available)
                                             + "</IDS>"
                                             + "<RELATIONS><RELATION><NAME>" + dt.Rows[0]["RelativeName"].ToString() + "</NAME><TYPE>" + setRelationIdForHighMark(dt.Rows[0]["RelationId"].ToString()) + "</TYPE>"
                                             + "</RELATION></RELATIONS><PHONES><PHONE><TELE-NO>" + dt.Rows[0]["ContactNo"].ToString() + "</TELE-NO>"
                                             + "<TELE-NO-TYPE>P03</TELE-NO-TYPE></PHONE></PHONES>"
                                             + "</APPLICANT-SEGMENT>";

                string highmark_address_coa = "";
                string highmark_state_coa = dt.Rows[0]["StateCode"].ToString();
                highmark_address_coa = dt.Rows[0]["Address"].ToString();
                string coa_pincode = dt.Rows[0]["PinCode"].ToString();
                string amount_applied = txtAppLnAmt.Text.ToString();

                string ADDRESS_SEGMENT_COA = "<ADDRESS-SEGMENT><ADDRESS><TYPE>D01</TYPE><ADDRESS-1>" + highmark_address_coa + "</ADDRESS-1>"
                                       + "<CITY>" + dt.Rows[0]["DistrictName"].ToString() + "</CITY><STATE>" + highmark_state_coa + "</STATE><PIN>" + coa_pincode + "</PIN></ADDRESS></ADDRESS-SEGMENT>";

                string APPLICATION_SEGMENT_COA = "<APPLICATION-SEGMENT><INQUIRY-UNIQUE-REF-NO>" + DateTime.Now.ToString("ddMMyyyyHHmmss") + coa_enq_id + "</INQUIRY-UNIQUE-REF-NO>"
                                           + "<CREDT-INQ-PURPS-TYP>ACCT-ORIG</CREDT-INQ-PURPS-TYP><CREDIT-INQUIRY-STAGE>PRE-SCREEN</CREDIT-INQUIRY-STAGE>"
                                           + "<CREDT-REQ-TYP>INDV</CREDT-REQ-TYP><BRANCH-ID>" + Session[gblValue.BrnchCode].ToString() + "</BRANCH-ID>"
                                           + "<LOS-APP-ID>" + lblHighMarkId.Text.ToString() + "</LOS-APP-ID>"
                                           + "<LOAN-AMOUNT>" + amount_applied + "</LOAN-AMOUNT></APPLICATION-SEGMENT>";

                string INQUIRY_COA = "<INQUIRY>" + APPLICANT_SEGMENT_COA + ADDRESS_SEGMENT_COA + APPLICATION_SEGMENT_COA + "</INQUIRY>";

                #endregion

                #region requestXML PART

                requestXML_COA = "<REQUEST-REQUEST-FILE>" + HEADER_SEGMENT_COA + INQUIRY_COA + "</REQUEST-REQUEST-FILE>";

                #endregion

                #region REQUEST/RESPONSE PART

                /*UAT URL*/
                var httpWebRequest_coa = (HttpWebRequest)WebRequest.Create("https://test.crifhighmark.com/Inquiry/doGet.service/requestResponse");
                /*********/

                /*PRODUCTION URL*/
                //var httpWebRequest_coa = (HttpWebRequest)WebRequest.Create("https://hub.crifhighmark.com/Inquiry/Inquiry/CPUAction.action");
                /****************/

                httpWebRequest_coa.ContentType = "application/xml; charset=utf-8";
                httpWebRequest_coa.Accept = "application/xml";
                httpWebRequest_coa.Method = "POST";
                httpWebRequest_coa.PreAuthenticate = true;
                //httpWebRequest.Timeout = 1000; //1 minute time out

                httpWebRequest_coa.Headers.Add("inquiryXML", requestXML_COA);
                httpWebRequest_coa.Headers.Add("userId", userId);
                httpWebRequest_coa.Headers.Add("password", password);

                try
                {
                    var httpResponse_coa = (HttpWebResponse)httpWebRequest_coa.GetResponse();

                    using (var streamReader_coa = new StreamReader(httpResponse_coa.GetResponseStream(), Encoding.UTF8))
                    {
                        var highmarkresult_coa = streamReader_coa.ReadToEnd();
                        responsedata_coa = highmarkresult_coa.ToString().Trim();
                    }
                }
                catch
                {
                    req_response_flag[3] = 1;
                }

                #endregion


                #endregion

                /*SAVE HIGHMARK RECORDS*/
                if (req_response_flag[2] == 1 || req_response_flag[3] == 1)
                {
                    highmark_data_save_status = 0;
                }
                else
                {
                    CMember oMem = new CMember();

                    highmark_data_save_status = oMem.SaveHighmarkData(lblHighMarkId.Text.ToString(), Session[gblValue.LoginDate].ToString(),
                        responsedata_coa, requestXML_COA, "C", Convert.ToInt32(hdUserID.Value));
                    if (highmark_data_save_status == 1)
                    {
                        btnVerify.Enabled = false;
                        btnDownloadCoAppEnq.Enabled = true;
                    }
                    else
                    {
                        gblFuction.MsgPopup("Highmark Data Verification Failed For Co Applicant...");
                        return;
                    }
                }
                /***********************/

                #endregion


            }
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
    }
}
