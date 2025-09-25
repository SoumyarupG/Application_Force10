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

namespace CENTRUMCF.WebPages.Private.Master
{
    public partial class CF_LoanApplicationStatus : CENTRUMBAse
    {
        protected int vPgNo = 1;
        string MinioUrl = ConfigurationManager.AppSettings["MinioUrl"];
        string BucketURL = ConfigurationManager.AppSettings["BucketURL"];
        string CFDocumentBucket = ConfigurationManager.AppSettings["CFDocumentBucket"];
        string DocumentBucketURL = ConfigurationManager.AppSettings["DocumentBucketURL"];
        private static string vKarzaKey = ConfigurationManager.AppSettings["KarzaKey"];
        string vKarzaEnv = ConfigurationManager.AppSettings["KarzaEnv"];
        string MaxFileSize = ConfigurationManager.AppSettings["MaxFileSize"];
        string CFLeadBucketURL = ConfigurationManager.AppSettings["CFLeadBucketURL"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBasePage();
                ViewState["StateEdit"] = null;
                ViewState["State"] = null;




                MainTabContainer.ActiveTabIndex = 0;
              //
                //StatusButton("View");
                if (Session[gblValue.LoginDate] != null)
                    txtFromDt.Text = gblFuction.putStrDate(gblFuction.setDate(Session[gblValue.LoginDate].ToString()).AddMonths(-1));
                if (Session[gblValue.LoginDate] != null)
                txtToDt.Text = Session[gblValue.LoginDate].ToString();
                if (Session[gblValue.BrName] != null)
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                if (Session[gblValue.LoginDate] != null)
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";

                LoadBasicDetailsList(1);
            }
        }
        private void InitBasePage()
        {
            try
            {
                if (Session[gblValue.BrnchCode].ToString().Trim() == "" || Session[gblValue.BrnchCode].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == null || Session[gblValue.LoginDate].ToString().Trim() == "" || Session[gblValue.UserId].ToString().Trim() == null || Session[gblValue.UserId].ToString().Trim() == "")
                    Response.Redirect("~/login.aspx", false);

                this.Menu = false;
                this.PageHeading = "Loan Application Status";
                this.ShowBranchName = Session[gblValue.BrnchCode].ToString() + " - " + Session[gblValue.BrName].ToString();
                this.ShowFinYear = Session[gblValue.FinYear].ToString() + " ( Login Date " + Session[gblValue.LoginDate].ToString() + " )";
                this.GetModuleByRole(mnuID.mnuCFLAS);
                if (Convert.ToInt32(Session[gblValue.RoleId]) == 1) return;
                if (this.CanView == "Y" && this.CanAdd != "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {

                    //btnEdit.Visible = true;
                    //btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit != "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    //btnEdit.Visible = false;
                    //btnDelete.Visible = false;
                    //btnCancel.Visible = false;
                    //btnSave.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete != "Y" && this.CanProcess != "Y")
                {
                    //btnDelete.Visible = false;

                }
                else if (this.CanView == "Y" && this.CanAdd == "Y" && this.CanEdit == "Y" && this.CanDelete == "Y" && this.CanProcess == "Y")
                {
                }
                else if (this.CanView == "N" || this.CanView == null || this.CanView == "")
                {
                    Response.Redirect("~/WebPages/Public/PageAccess.aspx?mnuTxt=" + "Loan Application Status", false);
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx", false);
            }
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            LoadBasicDetailsList(0);
        }
        private void LoadBasicDetailsList(Int32 pPgIndx)
        {
            DataTable dt = new DataTable();
            CMember oMem = new CMember();
            Int32 vTotRows = 0;
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            DateTime vFromDt = gblFuction.setDate(txtFromDt.Text.ToString());
            DateTime vToDt = gblFuction.setDate(txtToDt.Text.ToString());
            
            try
            {
                dt = oMem.CF_GetApplDetailsList(vBrCode, txtSearch.Text.Trim(), vFromDt, vToDt, pPgIndx, ref vTotRows);
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
            MainTabContainer.ActiveTabIndex = 0;
        }
        protected void btnExit_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx");
        }      
        protected void gvBasicDet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 pLeadID = 0;
            DataSet ds = new DataSet();
           
            try
            {
                
                ViewState["LeadID"] = pLeadID;
               
                GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                LinkButton btnBusEmp = (LinkButton)row.FindControl("btnBusEmp");
                LinkButton btnIncome = (LinkButton)row.FindControl("btnIncome");
                LinkButton btnInsc = (LinkButton)row.FindControl("btnInsc");
                if (btnBusEmp != null)
                {
                    hdEmpStatus.Value = btnBusEmp.Text;
                }
                if (btnIncome != null)
                {
                    hdIncomeStatus.Value = btnIncome.Text;
                }
                if (btnInsc != null)
                {
                    hdInsuStatus.Value = btnIncome.Text;
                }
                hdLeadId.Value = Convert.ToString(e.CommandArgument);
                hdMemberId.Value = row.Cells[0].Text;
                hdBCPNo.Value = row.Cells[3].Text;
                hdApplNm.Value = row.Cells[4].Text;
                hdAssMtdId.Value = row.Cells[7].Text;
                hdAssMtdTypId.Value = row.Cells[8].Text;
                hdMobNo.Value = row.Cells[9].Text;

                Session[gblValue.LeadID] = hdLeadId.Value;
                Session[gblValue.MemberID] = hdMemberId.Value;
                Session[gblValue.BCPNO] = hdBCPNo.Value;
                Session[gblValue.ApplNm] = hdApplNm.Value;
                Session[gblValue.EmpStatus] = hdEmpStatus.Value;
                Session[gblValue.AssMtdId] = hdAssMtdId.Value;
                Session[gblValue.IncomeStatus] = hdIncomeStatus.Value;
                Session[gblValue.AssMtdTypId] = hdAssMtdTypId.Value;
                Session[gblValue.InsuStatus] = hdInsuStatus.Value;
                Session[gblValue.MobNo] = hdMobNo.Value;

                Session["MnuId"] = "LnProc";
                Session["PaneId"] = 1;    


                switch (e.CommandName)
                {
                    case "Cust":
                        Session["LinkId"] = "lbCust360";
                        Response.Redirect("~/WebPages/Private/Master/Customer360.aspx", false);
                        break;
                    case "InChks":
                        Session["LinkId"] = "lbIntrnlChk";
                        Response.Redirect("~/WebPages/Private/Master/CF_InternalChecks.aspx", false);
                        break;
                    case "ExChks":
                        Session["LinkId"] = "lbExtrnlChk";
                        Response.Redirect("~/WebPages/Private/Master/CF_ExternlCheck.aspx", false);
                        break;
                    case "VenRpt":
                        Session["LinkId"] = "lbVenRep";
                        Response.Redirect("~/WebPages/Private/Master/CF_BCVendorRep.aspx", false);
                        break;
                    case "OthRp":
                        Session["LinkId"] = "lbOthrRep";
                        Response.Redirect("~/WebPages/Private/Master/CF_OtherRep.aspx", false);
                        break;
                    case "BusEmp":
                        Session["LinkId"] = "lbEmpBusDet";
                        Response.Redirect("~/WebPages/Private/Master/CF_BusEmpDet.aspx", false);
                        break;
                    case "Income":
                        Session["LinkId"] = "lbIncmDet";
                        Response.Redirect("~/WebPages/Private/Master/CF_IncomeDetails.aspx", false);
                        break;
                    case "Obli":
                        Session["LinkId"] = "lbOblgDet";
                        Response.Redirect("~/WebPages/Private/Master/CF_ObligationDetails.aspx", false);
                        break;
                    case "Elec":
                        Session["LinkId"] = "lbElcConAnlys";
                        Response.Redirect("~/WebPages/Private/Master/CF_ElcConAnlys.aspx", false);
                        break;
                    case "Bank":
                        Session["LinkId"] = "lbBankDet";
                        Response.Redirect("~/WebPages/Private/Master/CF_BankingDetails.aspx", false);
                        break;
                    case "Solar":
                        Session["LinkId"] = "lbSolPwrSysDet";
                        Response.Redirect("~/WebPages/Private/Master/CF_SolarSystem.aspx", false);
                        break;
                    case "Insc":
                        Session["LinkId"] = "lbInsuChrg";
                        Response.Redirect("~/WebPages/Private/Master/CF_Insurance_Charges.aspx", false);
                        break;
                    case "LTV":
                        Session["LinkId"] = "lbLtvComp";
                        Response.Redirect("~/WebPages/Private/Master/CF_LTVComputation.aspx", false);
                        break;
                    case "Dev":
                        Session["LinkId"] = "lbDevition";
                        Response.Redirect("~/WebPages/Private/Master/CF_Deviation.aspx", false);
                        break;
                    case "Sanc":
                        Session["LinkId"] = "lbSancCon";
                        Response.Redirect("~/WebPages/Private/Master/CF_SanctionCondition.aspx", false);
                        break;
                    case "Rec":
                        Session["LinkId"] = "lbRecmdtn";
                        Response.Redirect("~/WebPages/Private/Master/CF_Recommendation.aspx", false);
                        break;
                    case "Doc":
                        Session["LinkId"] = "lbDcmnt";
                        Response.Redirect("~/WebPages/Private/Master/CF_DocumentUpload.aspx", false);
                        break;

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
        

       
    }
}