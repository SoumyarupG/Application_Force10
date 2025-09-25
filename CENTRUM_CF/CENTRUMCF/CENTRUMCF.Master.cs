using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CENTRUMBA;
using CENTRUMCA;

namespace CENTRUMCF
{
    public partial class CENTRUMCF : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["USFBCF"] != null)
            {
                if (Convert.ToString(Session["LoginCookies"]) != Request.Cookies["USFBCF"].Value)
                {
                    Response.Redirect("SsnExpr.aspx");
                }
            }
            Response.AppendHeader("Refresh", Convert.ToString(1800) + "; url=SsnExpr.aspx");// 5 hrs
            lblMsg.Text = "";
            CMarquee objMar = new CMarquee();
            DataTable dt = objMar.GetLastMarquee();
            if (dt.Rows.Count > 0)
            {
                lt1.Text = dt.Rows[0]["Marquee"].ToString();
            }
            else
            {
                lt1.Text = "WELCOME TO UNITY SMALL FINANCE BANK - CLIMATE FINANCE";
            }
            if (Convert.ToString(Session[gblValue.BrnchCode]) == "0000")
            {
                //btnConRpt.Visible = false;
                //apMstList.Visible = true;
                //apLnReltd.Visible = true;
                ////apBrRpt.Visible = false;
                //apMontr.Visible = true;
                //apMIS.Visible = true;
                //apActR.Visible = true;
                //apVou.Visible = true;
                //btnAct.Visible = true;
                //mnuAct.Visible = true;
                // btnBon.Visible = false;
            }
            else
            {
                //btnConRpt.Visible = false;
                //apMstList.Visible = true;
                //apLnReltd.Visible = true;
                ////apBrRpt.Visible = true;
                //apMontr.Visible = true;
                //apMIS.Visible = true;
                //apActR.Visible = true;
                //apVou.Visible = true;
                ////btnAct.Visible = false;
                //mnuAct.Visible = true;
                // mnuUty.Visible = false; 
                // btnBon.Visible = false;

            }

            if (!Page.IsPostBack)
            {
                if (Session["MnuId"] == null)
                    MenuStat("Deft");
                else
                {
                    MenuStat(Session["MnuId"].ToString());
                    if (Session["LinkId"] != null)
                        MnuFocus(Session["MnuId"].ToString(), Convert.ToInt32(Session["PaneId"]), Session["LinkId"].ToString());
                }
            }
            lblVersion.Text = String.Format("Version: {0} &nbsp; Dated: {1}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                System.IO.File.GetLastWriteTime(Request.PhysicalApplicationPath.ToString() + "\\bin\\CENTRUMCF.dll").ToString("dd/MMM/yyyy HH:mm:ss"));

        }

        public string getCurrentPageName()
        {
            string pagename = this.cph_Main.Page.GetType().FullName;
            return pagename;
        }

        public string getWindowName()
        {
            if (Session["WindowName"] != null)
            {
                string WindowName = Session["WindowName"].ToString();
                return WindowName;
            }
            else
            {
                return "Invalid";
            }
        }

        public string geErrorPageName()
        {
            string errorpage = Page.ResolveUrl("~/SsnExpr.aspx");
            return errorpage;
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Response.RedirectPermanent("~/WebPages/Public/ErrDisp.aspx", false);
        }

        private void MnuFocus(string pMenu, int vIndex, string lkId)
        {
            switch (pMenu)
            {
                case "LnProc":
                    acLnProc.SelectedIndex = vIndex; ;
                    LinkButton lBtn = (LinkButton)this.acLnProc.FindControl(lkId);
                    lBtn.Style.Add("background", "#95B5D6");
                    btnLnProc.Style.Add("background", "#3da1e0");
                    break;
                case "Opertn":
                    acOprtn.SelectedIndex = vIndex; ;
                    LinkButton lBtnOpr = (LinkButton)this.acOprtn.FindControl(lkId);
                    lBtnOpr.Style.Add("background", "#95B5D6");
                    btnOpr.Style.Add("background", "#3da1e0");
                    break;
                case "Rpt":
                    acRpt.SelectedIndex = vIndex; ;
                    LinkButton lBtnRpt = (LinkButton)this.acRpt.FindControl(lkId);
                    lBtnRpt.Style.Add("background", "#95B5D6");
                    btnRpt.Style.Add("background", "#3da1e0");
                    break;
                case "Mstr":
                    acMstr.SelectedIndex = vIndex; ;
                    LinkButton lBtnMstr = (LinkButton)this.acMstr.FindControl(lkId);
                    lBtnMstr.Style.Add("background", "#95B5D6");
                    btnMstr.Style.Add("background", "#3da1e0");
                    break;
            }
        }

        private void MenuStat(string pMenu)
        {
            switch (pMenu)
            {

                case "LnProc":
                    mnuLnProc.Visible = true;
                    mnuOprtn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuMstr.Visible = false;
                    break;
                case "Opertn":
                    mnuOprtn.Visible = true;
                    mnuLnProc.Visible = false;
                    mnuRpt.Visible = false;
                    mnuMstr.Visible = false;
                    break;
                case "Deft":
                    mnuLnProc.Visible = false;
                    mnuOprtn.Visible = false;
                    mnuRpt.Visible = false;
                    mnuMstr.Visible = false;
                    break;
                case "Rpt":
                    mnuRpt.Visible = true;
                    mnuLnProc.Visible = false;
                    mnuOprtn.Visible = false;
                    mnuMstr.Visible = false;
                    break;
                case "Mstr":
                    mnuMstr.Visible = true;
                    mnuRpt.Visible = false;
                    mnuLnProc.Visible = false;
                    mnuOprtn.Visible = false;
                    break;


            }
        }

        protected void btnMstr_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Mstr");
            btnMstr.Style.Add("background", "#3da1e0");
            btnLnProc.Style.Add("background", "none");
            btnOpr.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
        }

        protected void btnLnProc_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("LnProc");
            btnLnProc.Style.Add("background", "#3da1e0");
            btnOpr.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnMstr.Style.Add("background", "none");
        }

        protected void btnOpr_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Opertn");
            btnOpr.Style.Add("background", "#3da1e0");
            btnLnProc.Style.Add("background", "none");
            btnRpt.Style.Add("background", "none");
            btnMstr.Style.Add("background", "none");
        }

        protected void btnRpt_Click(object sender, EventArgs e)
        {
            if (Session["MnuId"] == null) return;
            MenuStat("Rpt");
            btnRpt.Style.Add("background", "#3da1e0");
            btnLnProc.Style.Add("background", "none");
            btnOpr.Style.Add("background", "none");
            btnMstr.Style.Add("background", "none");
        }

        #region LoanProcessingMenu
        protected void lbNwLd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbNwLd";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (vBrCode == "0000")
                {
                    gblFuction.MsgPopup("Only Branch Can Process This....");
                    return;
                }
            }
            finally
            {
            }
            Response.Redirect("~/WebPages/Private/Master/CF_NewLeadMst.aspx");
        }

        protected void lbLnAppst_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbLnAppst";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (vBrCode == "0000")
                {
                    gblFuction.MsgPopup("Only Branch Can Process This....");
                    return;
                }
            }
            finally
            {
            }
            Response.Redirect("~/WebPages/Private/Master/CF_LoanApplicationStatus.aspx");
        }
        protected void lbLnApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbLnApp";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_FinalDecision.aspx");
            }
            finally
            {
            }
            return;
        }
        protected void lbBCBM_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbBCBM";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_SactionComplianceBCBCM.aspx");
            }
            finally
            {
            }
            return;
        }
        protected void lbHOCredit_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbHOCredit";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_SactionComplianceHO.aspx");
            }
            finally
            {
            }
            return;
        }
        protected void lbBCBOE_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbBCBOE";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_SactionComplianceBOE.aspx");
            }
            finally
            {
            }
            return;
        }

        protected void lbDcmnt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbDcmnt";
            Response.Redirect("~/WebPages/Private/Master/CF_DocumentUpload.aspx");
        }
        protected void lbBasicDtl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbBasicDtl";
            Response.Redirect("~/WebPages/Private/Master/CF_BasicDetails.aspx");
        }

        protected void lbCust360_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbCust360";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }
            Response.Redirect("~/WebPages/Private/Master/Customer360.aspx");
        }

        protected void lbIntrnlChk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbIntrnlChk";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }
            Response.Redirect("~/WebPages/Private/Master/CF_InternalChecks.aspx");
        }
        protected void lbExtrnlChk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbExtrnlChk";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }
            Response.Redirect("~/WebPages/Private/Master/CF_ExternlCheck.aspx");
        }
        protected void lbVenRep_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbVenRep";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }
            Response.Redirect("~/WebPages/Private/Master/CF_BCVendorRep.aspx");
        }
        protected void lbOthrRep_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbOthrRep";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }

            Response.Redirect("~/WebPages/Private/Master/CF_OtherRep.aspx");
        }
        protected void lbEmpBusDet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbEmpBusDet";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode == "0000")
            {
                gblFuction.MsgPopup("Only Branch Can Process This....");
                return;
            }

            Response.Redirect("~/WebPages/Private/Master/CF_BusEmpDet.aspx");
        }
        protected void lbIncmDet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbIncmDet";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Response.Redirect("~/WebPages/Private/Master/CF_IncomeDetails.aspx");
        }
        protected void lbOblgDet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbOblgDet";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Response.Redirect("~/WebPages/Private/Master/CF_ObligationDetails.aspx");
        }
        protected void lbElcConAnlys_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbElcConAnlys";

            Response.Redirect("~/WebPages/Private/Master/CF_ElcConAnlys.aspx");
        }
        protected void lbBankDet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbBankDet";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Response.Redirect("~/WebPages/Private/Master/CF_BankingDetails.aspx");
        }
        protected void lbSolPwrSysDet_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbSolPwrSysDet";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            Response.Redirect("~/WebPages/Private/Master/CF_SolarSystem.aspx");
        }
        protected void lbInsuChrg_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbInsuChrg";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_Insurance_Charges.aspx");
            }
            finally
            {
            }
            return;
            //Response.Redirect("~/WebPages/Private/Master/CF_BasicDetails.aspx");
        }
        protected void lbLtvComp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbLtvComp";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_LTVComputation.aspx");
            }
            finally
            {
            }
            return;
        }
        protected void lbDevition_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbDevition";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_Deviation.aspx");

            }
            finally
            {
            }

        }
        protected void lbSancCon_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbSancCon";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_SanctionCondition.aspx");

            }
            finally
            {
            }

        }
        protected void lbRecmdtn_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "LnProc";
            Session["PaneId"] = acLnProc.SelectedIndex;
            Session["LinkId"] = "lbRecmdtn";
            try
            {
                Response.Redirect("~/WebPages/Private/Master/CF_Recommendation.aspx");
            }
            finally
            {
            }
        }
        protected void lbepcMaster_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mstr";
            Session["PaneId"] = acMstr.SelectedIndex;
            Session["LinkId"] = "lbepcMaster";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            try
            {
                if (vBrCode != "0000")
                {
                    gblFuction.MsgPopup("Only Head Office Can Process This....");
                    return;
                }
            }
            finally
            {
            }
            Response.Redirect("~/WebPages/Private/Master/CF_EPCMaster.aspx");
        }
        protected void lbLoanPara_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mstr";
            Session["PaneId"] = acMstr.SelectedIndex;
            Session["LinkId"] = "lbLoanPara";

            Response.Redirect("~/WebPages/Private/Master/LoanParameter.aspx");
        }
        protected void lbGenPara_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Mstr";
            Session["PaneId"] = acMstr.SelectedIndex;
            Session["LinkId"] = "lbGenPara";

            Response.Redirect("~/WebPages/Private/Master/GeneralParameter.aspx");
        }
        protected void lbCntrlLoanApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCntrlLoanApp";

            Response.Redirect("~/WebPages/Private/Operation/CF_OpsCentralStage1.aspx");
        }
        protected void lbLoanApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbLoanApp";

            Response.Redirect("~/WebPages/Private/Operation/CF_OpsBcBmLoanApplication.aspx");
        }
        protected void lbCntrlSecLoanApp_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCntrlSecLoanApp";

            Response.Redirect("~/WebPages/Private/Operation/CF_OpsCentralStage2.aspx");
        }
        protected void lbCntrlLoanDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCntrlLoanDisb";

            Response.Redirect("~/WebPages/Private/Transaction/CF_LoanDisbursement.aspx");
        }
        protected void lbCntrlTrancheDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCntrlTrancheDisb";

            Response.Redirect("~/WebPages/Private/Transaction/CF_TrancheDisbursement.aspx");
        }
        protected void lbUMRN_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbUMRN";

            Response.Redirect("~/WebPages/Private/Transaction/CF_UMRNUpdate.aspx");
        }
        protected void lbLoanBulkUpLd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbLoanBulkUpLd";

            Response.Redirect("~/WebPages/Private/Transaction/CollectionUpload.aspx");
        }
        protected void lbLoanSnglUpLd_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbLoanSnglUpLd";

            Response.Redirect("~/WebPages/Private/Transaction/LoanRecovry.aspx");
        }
        protected void lbPreMatColl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbPreMatColl";

            Response.Redirect("~/WebPages/Private/Transaction/CF_PrematureColl.aspx");
        }
        protected void lbCollRev_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCollRev";

            Response.Redirect("~/WebPages/Private/Transaction/CollReverseDelete.aspx");
        }
        //
        protected void lbPDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbPDD";

            Response.Redirect("~/WebPages/Private/Transaction/DeathFlaging.aspx");
        }
        protected void lbDD_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbDD";

            Response.Redirect("~/WebPages/Private/Transaction/CF_DeathClosing.aspx");
        }
        protected void lbDDCL_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbDDCL";

            Response.Redirect("~/WebPages/Private/Transaction/CF_DeathDeclare.aspx");
        }
        protected void lbDDU_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbDDU";

            Response.Redirect("~/WebPages/Private/Transaction/CF_ClaimeStatementAndDeathRecipt.aspx");
        }
        protected void lbDownCertificate_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbDownCertificate";

            Response.Redirect("~/WebPages/Private/Transaction/CF_HODwnAllCertificate.aspx");
        }
        protected void lbCancel_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Opertn";
            Session["PaneId"] = acOprtn.SelectedIndex;
            Session["LinkId"] = "lbCancel";

            Response.Redirect("~/WebPages/Private/Transaction/DeathCancelAfterVerification.aspx");
        }
        protected void lbPredb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPredb";

            Response.Redirect("~/WebPages/Private/Report/rptPreDBDtl.aspx");
        }
        protected void lbLoanDisb_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanDisb";

            Response.Redirect("~/WebPages/Private/Report/LoanDisbursement.aspx");
        }
        protected void lbLoanRepay_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanRepay";

            Response.Redirect("~/WebPages/Private/Report/RepaymentSche.aspx");
        }
        protected void lbLoanLedger_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanRepay";

            Response.Redirect("~/WebPages/Private/Report/PartyLedgerNew.aspx");
        }
        protected void lbColRprt_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbLoanRepay";

            Response.Redirect("~/WebPages/Private/Report/CollectionReport.aspx");
        }
        protected void lbCustCh_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbCustCh";

            Response.Redirect("~/WebPages/Private/Report/rptCustomerChargesDtl.aspx");
        }
        protected void lbDeathTag_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDeathTag";

            Response.Redirect("~/WebPages/Private/Report/rptDeathTaggingDtl.aspx");
        }
        protected void lbDeathSetttl_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbDeathSetttl";

            Response.Redirect("~/WebPages/Private/Report/rptDeathSettlement.aspx");
        }
        protected void lbPreMatCol_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbPreMatCol";

            Response.Redirect("~/WebPages/Private/Report/rptPrematureCollection.aspx");
        }
        protected void lbRptHypo_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptHypo";

            Response.Redirect("~/WebPages/Private/Report/rptHypothecationdtl.aspx");
        }
        protected void lbRptCust_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptCust";

            Response.Redirect("~/WebPages/Private/Report/rptCustomerDtl.aspx");
        }
        protected void lbRptEpcBnk_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptEpcBnk";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode != "0000")
            {
                gblFuction.MsgPopup("Only Head Office Can Process This....");
                return;
            }
            else
            {
                Response.Redirect("~/WebPages/Private/Report/rptEpcBankingDtl.aspx");
            }
        }
        protected void lbRptBranch_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptBranch";

            Response.Redirect("~/WebPages/Private/Report/rptBranchMstDtl.aspx");
        }
        protected void lbRptUserMstr_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptUserMstr";

            Response.Redirect("~/WebPages/Private/Report/rptUserMstDtl.aspx");
        }
        protected void lbRptAllEpc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptAllEpc";
            string vBrCode = Session[gblValue.BrnchCode].ToString();
            if (vBrCode != "0000")
            {
                gblFuction.MsgPopup("Only Head Office Can Process This....");
                return;
            }
            else
            {
                Response.Redirect("~/WebPages/Private/Report/rptAllEpcDtl.aspx");
            }
        }
        protected void lbRptSanc_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptSanc";

            Response.Redirect("~/WebPages/Private/Report/rptSanctionCnditn.aspx");
        }
        protected void lbRptCustBank_Click(object sender, EventArgs e)
        {
            Session["MnuId"] = "Rpt";
            Session["PaneId"] = acRpt.SelectedIndex;
            Session["LinkId"] = "lbRptCustBank";

            Response.Redirect("~/WebPages/Private/Report/rptCustomerBankDtl.aspx");
        }
        #endregion

        #region Properties
        public bool Menu
        {
            set
            {
                btnLnProc.Enabled = value;

            }
        }
        public bool Welcome
        {
            set { trHeading.Visible = value; }
        }
        public string PageHeading
        {
            set
            {
                this.lbl_PageName.Text = value;
            }
        }
        public string ShowLoginInfo
        {
            set
            {
                this.lblWlCome.Visible = true;
                this.lblWlCome.Text = value;
            }
        }
        public string ShowBranchName
        {
            set
            {
                this.lblFedrtn.Visible = true;
                this.lblFedrtn.Text = value;
            }
        }
        public string ShowFinYear
        {
            set
            {
                this.lblFinYr.Visible = true;
                this.lblFinYr.Text = value;
            }
        }
        #endregion

        protected void lbLogOut_Click(object sender, EventArgs e)
        {
            //CDayEnd oDE = new CDayEnd();
            //oDE.UpdateUserBranch("",Convert.ToInt32(Session[gblValue.UserId].ToString()),"O");
            //oDE = null;
            CUser oUsr = new CUser();
            int vErr = oUsr.UpdateLogOutDt(Convert.ToInt32(Session[gblValue.LoginId]));
            oUsr = null;
            Session.Abandon();
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            lblWlCome.Text = " Guest";
            Response.Redirect("~/Login.aspx");
        }
        protected void lblChangeBranch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Webpages/Public/FinYear.aspx", false);
        }
        protected void lblHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Public/Main.aspx", false);
        }
        protected void lblSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebPages/Private/Master/Search.aspx", false);
        }

    }
}